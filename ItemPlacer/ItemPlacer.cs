using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using ItemChanger.Default;
using ItemChanger;
using System.IO;

namespace ItemPlacer
{
    public partial class ItemPlacer : Form
    {
        string[] fullItemArray;
        string[] fullLocationArray;

        FieldInfo[] defaultShopPresets;
        HashSet<string> fullyPlacedItems;
        HashSet<string> filledLocations;

        Dictionary<string, Item> customItems;
        Dictionary<string, Location> customLocations;

        Random rand = new Random();

        Dictionary<string, int> multiplaceItems = new Dictionary<string, int>
        {
            { ItemNames.Grub_Pickup, 46 },
            { ItemNames.Mask_Shard, 16 },
            { ItemNames.Vessel_Fragment, 9 },
            { ItemNames.Charm_Notch, 8 },
            { ItemNames.Pale_Ore, 6 },
            { ItemNames.Wanderers_Journal, 14 },
            { ItemNames.Hallownest_Seal, 17 },
            { ItemNames.Kings_Idol, 8 },
            { ItemNames.Arcane_Egg, 4 },
            { ItemNames.Rancid_Egg, 20 },
            { ItemNames.Simple_Key, 4 },
        };
        HashSet<string> Shops = new HashSet<string>
        {
            LocationNames.Sly, LocationNames.Sly_Key, LocationNames.Iselda, LocationNames.Salubra, LocationNames.Leg_Eater
        };

        public ItemPlacer()
        {
            
            Type type = typeof(ItemChanger.Item);
            Assembly a = System.Reflection.Assembly.GetAssembly(type);
            XmlManager.FetchItemDefs(a.GetManifestResourceStream("ItemChanger.Resources.items.xml"));
            XmlManager.FetchLocationDefs(a.GetManifestResourceStream("ItemChanger.Resources.locations.xml"));
            XmlManager.FetchPlatformScenes(a.GetManifestResourceStream("ItemChanger.Resources.platforms.xml"));
            fullItemArray = typeof(ItemNames).GetFields().Where(field => field.FieldType == typeof(string)).Select(field => (string)field.GetRawConstantValue()).ToArray();
            fullLocationArray = typeof(LocationNames).GetFields().Where(field => field.FieldType == typeof(string)).Select(field => (string)field.GetRawConstantValue()).ToArray();
            defaultShopPresets = typeof(Shops).GetFields().Where(field => field.FieldType == typeof(Shops.DefaultShopItems) && field.IsLiteral && !field.IsInitOnly).ToArray();
            fullyPlacedItems = new HashSet<string>();
            filledLocations = new HashSet<string>();
            customItems = new Dictionary<string, Item>();
            customLocations = new Dictionary<string, Location>();

            InitializeComponent();
            comboBoxItems.Items.AddRange(fullItemArray);
            comboBoxLocations.Items.AddRange(fullLocationArray);

            comboBoxCostType.Items.AddRange(Enum.GetNames(typeof(Location.CostType)));
            comboBoxCostType.SelectedIndex = 0;
            checkBoxEditCosts_CheckedChanged(null, null);

            checkedListBoxDiscardItemPools.Items.AddRange(((IEnumerable<Item.ItemPool>)Enum.GetValues(typeof(Item.ItemPool))).Intersect(XmlManager.Items.Select(item => item.Value.pool)).Select(pool => (object)pool).ToArray());
            for (int i = 0; i < checkedListBoxDiscardItemPools.Items.Count; i++)
            {
                checkedListBoxDiscardItemPools.SetItemChecked(i, true);
            }

            checkedListBoxDiscardLocationPools.Items.AddRange(((IEnumerable<Location.LocationPool>)Enum.GetValues(typeof(Location.LocationPool))).Intersect(XmlManager.Locations.Select(item => item.Value.pool)).Select(pool => (object)pool).ToArray());
            for (int i = 0; i < checkedListBoxDiscardLocationPools.Items.Count; i++)
            {
                checkedListBoxDiscardLocationPools.SetItemChecked(i, true);
            }

            comboBoxDefaultShopItems.Items.AddRange(defaultShopPresets                
                .Select(field => field.Name).ToArray());

            comboBoxGiveActionCust.Items.AddRange(Enum.GetNames(typeof(Item.GiveAction)));
            comboBoxGiveActionCust.SelectedIndex = 0;

            comboBoxItemTypeCust.Items.AddRange(Enum.GetNames(typeof(Item.ItemType)));
            comboBoxItemTypeCust.SelectedIndex = 0;

            comboBoxTemplateItemCust.Items.AddRange(fullItemArray);

            comboBoxSpriteCust.Items.AddRange(XmlManager.spriteNames.Values.ToArray());
            comboBoxNameKeyCust.Items.AddRange((new HashSet<string>(XmlManager.Items.Values.Select(i => i.nameKey))).Where(s => !string.IsNullOrEmpty(s)).ToArray());
            comboBoxShopDescCust.Items.AddRange((new HashSet<string>(XmlManager.Items.Values.Select(i => i.shopDescKey))).Where(s => !string.IsNullOrEmpty(s)).ToArray());

            comboBoxBigSpriteCust.Items.AddRange(XmlManager.bigSpriteNames.Values.ToArray());
            comboBoxTakeKeyCust.Items.AddRange((new HashSet<string>(XmlManager.Items.Values.Select(i => i.takeKey))).Where(s => !string.IsNullOrEmpty(s)).ToArray());
            comboBoxButtonKeyCust.Items.AddRange((new HashSet<string>(XmlManager.Items.Values.Select(i => i.buttonKey))).Where(s => !string.IsNullOrEmpty(s)).ToArray());
            comboBoxDescOneCust.Items.AddRange((new HashSet<string>(XmlManager.Items.Values.Select(i => i.descOneKey))).Where(s => !string.IsNullOrEmpty(s)).ToArray());
            comboBoxDescTwoCust.Items.AddRange((new HashSet<string>(XmlManager.Items.Values.Select(i => i.descTwoKey))).Where(s => !string.IsNullOrEmpty(s)).ToArray());

            comboBoxDefaultGameObjects.Items.AddRange(DefaultGameObjectInfo.defaultObjects.Keys.ToArray());

            checkedListBoxExtraPlatformScenes.Items.AddRange(XmlManager.platformScenes.ToArray());
            for (int i=0; i < checkedListBoxExtraPlatformScenes.Items.Count; i++)
            {
                checkedListBoxExtraPlatformScenes.SetItemChecked(i, true);
            }

            UpdateMultiItemLabel();
        }

        private void UpdateMultiItemLabel()
        {
            Dictionary<string, int> counts = GetAllMultiplaceCounts();
            string label = string.Empty;
            bool first = true;
            foreach (var kvp in counts)
            {
                label += (first ? string.Empty : "\n") + $"{kvp.Key}: {kvp.Value}/{multiplaceItems[kvp.Key]}";
                first = false;
            }
            multiItemCounter.Text = label;
        }

        private int GetMultiplaceItemCount(string item)
        {
            int i = 0;
            foreach (ILP ilp in listBox1.Items)
            {
                if (ilp.item == item) i++;
            }

            return i;
        }

        private Dictionary<string, int> GetAllMultiplaceCounts()
        {
            Dictionary<string, int> counts = new Dictionary<string, int>();
            foreach (var kvp in multiplaceItems) counts.Add(kvp.Key, 0);
            foreach (ILP ilp in listBox1.Items)
            {
                if (counts.ContainsKey(ilp.item)) counts[ilp.item]++;
            }
            return counts;
        }

        private bool CheckItemCountMaxedOut(string item)
        {
            if (!multiplaceItems.ContainsKey(item)) return GetMultiplaceItemCount(item) >= 1;

            return GetMultiplaceItemCount(item) >= multiplaceItems[item];
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (!(comboBoxItems.Text is string item) || !(comboBoxLocations.Text is string location)) return;
            if (!fullItemArray.Contains(item) && !customItems.ContainsKey(item)) return;
            if (!fullLocationArray.Contains(location) && !customLocations.ContainsKey(location)) return;

            bool editCost = checkBoxEditCosts.Checked || Shops.Contains(location);

            if (!editCost)
            {
                AddILP(new ILP(item, location));
            }
            else
            {
                int cost = (int)costUpDown.Value;
                string costType = (string)comboBoxCostType.SelectedItem ?? "None";
                if (Shops.Contains(location))
                {
                    costType = "Geo";
                    cost = cost != 0 ? cost : rand.Next(100, 500);
                }
                AddILP(new ILP(item, location, cost, costType));
            }
        }

        private void AddILP(ILP ilp)
        {
            string item = ilp.item;
            string location = ilp.location;

            listBox1.Items.Add(ilp);

            if (CheckItemCountMaxedOut(item))
            {
                fullyPlacedItems.Add(item);
                if (hidePlacedItems.Checked) comboBoxItems.Items.Remove(item);
            }
            if (!Shops.Contains(location))
            {
                filledLocations.Add(location);
                if (checkBoxHideUsedLocations.Checked) comboBoxLocations.Items.Remove(location);
            }
            UpdateMultiItemLabel();
            costUpDown.Value = 0;
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if (!(listBox1.SelectedItem is ILP ilp)) return;
            RemoveILP(ilp);
        }

        private void RemoveILP(ILP ilp)
        {
            string item = ilp.item;
            string location = ilp.location;

            listBox1.Items.Remove(ilp);

            if (!CheckItemCountMaxedOut(item))
            {
                fullyPlacedItems.Remove(item);
                if (hidePlacedItems.Checked && !comboBoxItems.Items.Contains(item)) comboBoxItems.Items.Add(item);
            }
            if (!Shops.Contains(location))
            {
                filledLocations.Remove(location);
                if (checkBoxHideUsedLocations.Checked) comboBoxLocations.Items.Add(location);
            }
            UpdateMultiItemLabel();
        }

        private void comboBoxDefaultShopItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            FieldInfo field = defaultShopPresets.FirstOrDefault(f => f.Name == comboBoxDefaultShopItems.SelectedItem.ToString());
            if (field != null)
            {
                if (field.GetRawConstantValue() is int value)
                {
                    defaultShopLabel.Text = $"Vanilla Shop Items (preset: {value})";
                }
                else
                {
                    defaultShopLabel.Text = "Error processing value!!!";
                }
            }
            else
            {
                defaultShopLabel.Text = "Vanilla Shop Items (select preset)";
            }
        }

        private void checkBoxHideUsedLocations_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxHideUsedLocations.Checked)
            {
                comboBoxLocations.Items.Clear();
                comboBoxLocations.Items.AddRange(fullLocationArray.Except(filledLocations).ToArray());
                comboBoxLocations.Items.AddRange(customLocations.Keys.Except(filledLocations).ToArray());
            }
            else
            {
                comboBoxLocations.Items.Clear();
                comboBoxLocations.Items.AddRange(fullLocationArray);
                comboBoxLocations.Items.AddRange(customLocations.Keys.ToArray());
            }
        }

        private void hidePlacedItems_CheckedChanged(object sender, EventArgs e)
        {
            if (hidePlacedItems.Checked)
            {
                comboBoxItems.Items.Clear();
                comboBoxItems.Items.AddRange(fullItemArray.Except(fullyPlacedItems).ToArray());
                comboBoxItems.Items.AddRange(customItems.Keys.Except(fullyPlacedItems).ToArray());
            }
            else
            {
                comboBoxItems.Items.Clear();
                comboBoxItems.Items.AddRange(fullItemArray);
                comboBoxItems.Items.AddRange(customItems.Keys.ToArray());
            }
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to reset all data?", "Warning", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }

            listBox1.Items.Clear();
            fullyPlacedItems.Clear();
            filledLocations.Clear();
            comboBoxItems.Items.Clear();
            comboBoxLocations.Items.Clear();
            comboBoxItems.Items.AddRange(fullItemArray);
            comboBoxLocations.Items.AddRange(fullLocationArray);
            discardItemBox.Items.Clear();
            discardLocBox.Items.Clear();
            customItems.Clear();
            customLocations.Clear();
            listBoxCustomText.Items.Clear();

            titleBox.Text = string.Empty;
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (XmlManager.TryLoad(openFileDialog1.FileName))
                {
                    resetButton_Click(null, null);
                    foreach (ILP ilp in XmlManager.xmlILPs) AddILP(ilp);
                    foreach (Item item in XmlManager.xmlCustomItems)
                    {
                        customItems[item.name] = item;
                        if (!XmlManager.xmlILPs.Any(ilp => ilp.item == item.name))
                        {
                            comboBoxItems.Items.Add(item);
                        }
                    }
                    foreach (Location location in XmlManager.xmlCustomLocations)
                    {
                        customLocations[location.name] = location;
                        if (!XmlManager.xmlILPs.Any(ilp => ilp.location == location.name))
                        {
                            comboBoxLocations.Items.Add(location);
                        }
                    }
                    foreach (LanguageEntry entry in XmlManager.xmlCustomText)
                    {
                        listBoxCustomText.Items.Add(entry);
                    }

                    listBoxSpecialActions.Items.AddRange(XmlManager.xmlSpecialActions.ToArray());

                    if (comboBoxDefaultShopItems.Items.Contains(XmlManager.xmlDefaultShopPreset))
                    {
                        comboBoxDefaultShopItems.SelectedItem = XmlManager.xmlDefaultShopPreset;
                    }
                    titleBox.Text = XmlManager.xmlTitle ?? string.Empty;

                    if (XmlManager.xmlChangeStart)
                    {
                        changeStartCheckBox.Checked = true;
                        startSceneNameBox.Text = XmlManager.xmlStartLocation.startSceneName;
                        numericUpDownStartX.Value = (decimal)XmlManager.xmlStartLocation.startX;
                        numericUpDownStartY.Value = (decimal)XmlManager.xmlStartLocation.startY;
                    }

                    forceUseFirstKeyOnWaterways.Checked = XmlManager.settings.forceUseFirstSimpleKeyOnWaterways;
                    unlockAllColosseumTrials.Checked = XmlManager.settings.unlockAllColosseumTrials;
                    reusableCityCrestNoHardSave.Checked = XmlManager.settings.reusableCityCrestWithNoHardSave;
                    openLowerBeastsDenThroughShortcut.Checked = XmlManager.settings.openLowerBeastsDenThroughShortcut;
                    removeBeastsDenHardSave.Checked = XmlManager.settings.removeBeastsDenHardSave;
                    skipDreamerTextBeforeDreamNail.Checked = XmlManager.settings.skipDreamerTextBeforeDreamNail;
                    blockLegEaterDeath.Checked = XmlManager.settings.blockLegEaterDeath;
                    oneBlueHPLifebloodCoreDoor.Checked = XmlManager.settings.oneBlueHPLifebloodCoreDoor;
                    reduceBaldurHP.Checked = XmlManager.settings.reduceBaldurHP;
                    checkBoxTransitionQOL.Checked = XmlManager.settings.transitionQOL;
                    checkBoxMiscSkipFixes.Checked = XmlManager.settings.miscSkipFixes;
                    checkBoxMoveSeerLeft.Checked = XmlManager.settings.moveSeerLeft;
                    checkBoxFixVoidHeart.Checked = XmlManager.settings.fixVoidHeart;
                    checkBoxBlockZoteDeath.Checked = XmlManager.settings.blockZoteDeath;
                    foreach (string sceneName in XmlManager.settings.skipExtraPlatformScenes ?? new HashSet<string>())
                    {
                        checkedListBoxExtraPlatformScenes.SetItemChecked(checkedListBoxExtraPlatformScenes.Items.IndexOf(sceneName), false);
                    }

                    startWithoutFocus.Checked = XmlManager.settings.startWithoutFocus;

                    colo1Prompt.Checked = XmlManager.settings.colo1ItemPrompt;
                    colo2Prompt.Checked = XmlManager.settings.colo2ItemPrompt;
                    flowerQuestPrompt.Checked = XmlManager.settings.flowerQuestPrompt;
                    whitePalacePrompt.Checked = XmlManager.settings.whitePalacePrompt;
                }
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ItemChangerSettings settings = new ItemChangerSettings
                {
                    forceUseFirstSimpleKeyOnWaterways = forceUseFirstKeyOnWaterways.Checked,
                    unlockAllColosseumTrials = unlockAllColosseumTrials.Checked,
                    reusableCityCrestWithNoHardSave = reusableCityCrestNoHardSave.Checked,
                    openLowerBeastsDenThroughShortcut = openLowerBeastsDenThroughShortcut.Checked,
                    removeBeastsDenHardSave = removeBeastsDenHardSave.Checked,
                    skipDreamerTextBeforeDreamNail = skipDreamerTextBeforeDreamNail.Checked,
                    blockLegEaterDeath = blockLegEaterDeath.Checked,
                    oneBlueHPLifebloodCoreDoor = oneBlueHPLifebloodCoreDoor.Checked,
                    reduceBaldurHP = reduceBaldurHP.Checked,
                    transitionQOL = checkBoxTransitionQOL.Checked,
                    miscSkipFixes = checkBoxMiscSkipFixes.Checked,
                    moveSeerLeft = checkBoxMoveSeerLeft.Checked,
                    fixVoidHeart = checkBoxFixVoidHeart.Checked,
                    blockZoteDeath = checkBoxBlockZoteDeath.Checked,


                    startWithoutFocus = startWithoutFocus.Checked,


                    colo1ItemPrompt = colo1Prompt.Checked,
                    colo2ItemPrompt = colo2Prompt.Checked,
                    flowerQuestPrompt = flowerQuestPrompt.Checked,
                    whitePalacePrompt = whitePalacePrompt.Checked,

                    skipExtraPlatformScenes = new HashSet<string>
                    (
                    from string item in checkedListBoxExtraPlatformScenes.Items
                    where !checkedListBoxExtraPlatformScenes.CheckedItems.Contains(item)
                    select item
                    ),
                };

                StartLocation startLocation = new StartLocation
                {
                    startSceneName = startSceneNameBox.Text,
                    startX = (float)numericUpDownStartX.Value,
                    startY = (float)numericUpDownStartY.Value
                };

                XmlManager.Save(
                    saveFileDialog1.FileName, 
                    listBox1.Items.GetEnumerator(), 
                    comboBoxDefaultShopItems.SelectedItem?.ToString(), 
                    titleBox.Text,
                    settings, 
                    changeStartCheckBox.Checked, 
                    startLocation,
                    customItems, 
                    customLocations,
                    listBoxCustomText.Items.GetEnumerator(),
                    listBoxSpecialActions.Items.GetEnumerator()
                    );
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void changeStart_CheckedChanged(object sender, EventArgs e)
        {
            bool changeStart = changeStartCheckBox.Checked;
            startSceneNameBox.Enabled = changeStart;
            numericUpDownStartX.Enabled = changeStart;
            numericUpDownStartY.Enabled = changeStart;
        }

        private void buttonAutofill_Click(object sender, EventArgs e)
        {
            if( MessageBox.Show("This will randomly place all remaining items into the current locations, without regard to whether the result is completeable. Locations will be given default costs, and items placed at shops will receive random prices. If there are any items or locations you do not want to use, please discard them beforehand.",
                "Auto-Fill", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                hidePlacedItems.Checked = true;
                checkBoxEditCosts.Checked = false;
                while (comboBoxItems.Items.Count > 0)
                {
                    comboBoxItems.SelectedItem = RandomMultiSelect();
                    comboBoxLocations.SelectedIndex = rand.Next(comboBoxLocations.Items.Count);
                    while (Shops.Contains(comboBoxLocations.SelectedItem) && comboBoxLocations.Items.Count > 5 && comboBoxLocations.Items.Count > comboBoxItems.Items.Count)
                    {
                        comboBoxLocations.SelectedIndex = rand.Next(comboBoxLocations.Items.Count);
                    }

                    buttonAdd_Click(null, null);
                }
            }
        }

        private string RandomMultiSelect()
        {
            int count = comboBoxItems.Items.Count;
            foreach (string key in multiplaceItems.Keys)
            {
                if (comboBoxItems.Items.Contains(key))
                {
                    count += multiplaceItems[key] - GetMultiplaceItemCount(key) - 1;
                }
            }
            int index = rand.Next(count);
            if (index < comboBoxItems.Items.Count)
            {
                return (string)comboBoxItems.Items[index];
            }
            else
            {
                int adjust = index;
                foreach (string key in multiplaceItems.Keys)
                {
                    adjust += multiplaceItems[key] - GetMultiplaceItemCount(key) - 1;
                    if (index < adjust) return key;
                }
            }
            return (string)comboBoxItems.Items[0];
        }

        private void buttonRandom_Click(object sender, EventArgs e)
        {
            if (comboBoxItems.Items.Count > 0) comboBoxItems.SelectedIndex = rand.Next(comboBoxItems.Items.Count);
            if (comboBoxLocations.Items.Count > 0) comboBoxLocations.SelectedIndex = rand.Next(comboBoxLocations.Items.Count);
        }

        private void buttonDiscardItem_Click(object sender, EventArgs e)
        {
            if (comboBoxItems.SelectedItem == null) return;
            string item = (string)comboBoxItems.SelectedItem;
            comboBoxItems.Items.Remove(item);
            discardItemBox.Items.Add(item);
        }

        private void buttonDiscardLoc_Click(object sender, EventArgs e)
        {
            if (comboBoxLocations.SelectedItem == null) return;
            string loc = (string)comboBoxLocations.SelectedItem;
            comboBoxLocations.Items.Remove(loc);
            discardLocBox.Items.Add(loc);
        }

        private void buttonRecycleItem_Click(object sender, EventArgs e)
        {
            if (discardItemBox.SelectedItem == null) return;
            string item = (string)discardItemBox.SelectedItem;
            comboBoxItems.Items.Add(item);
            discardItemBox.Items.Remove(item);
        }

        private void buttonRecycleLoc_Click(object sender, EventArgs e)
        {
            if (discardLocBox.SelectedItem == null) return;
            string loc = (string)discardLocBox.SelectedItem;
            comboBoxLocations.Items.Add(loc);
            discardLocBox.Items.Remove(loc);
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "- Select items and locations in the drop-down boxes, and use the Add button to place them." +
                "\n- Select an item-location pair from the list and click the Remove button to unplace it." +
                "\n- Select a cost and a cost type to add custom costs to item/locations. " +
                "\n- Costs paired with \"Default\" cost type are ignored. This will result in default costs for grub items, essence items, and so on." +
                "\n- Shops must have positive cost of type geo, and will ignore inputs that do not meet this criterion." +
                "\n- Use the Random button to select a random item and location from the lists." +
                "\n Use the Auto-Fill button to place all remaining locations, with the default counters." +
                "\n- Remove items and locations that you do not intend to use with the Discard buttons.", "Help");
        }

        private void buttonAddCustLoc_Click(object sender, EventArgs e)
        {
            string name = textBoxCustomLocName.Text;
            if (string.IsNullOrWhiteSpace(name) || fullLocationArray.Contains(name) || customLocations.ContainsKey(name))
            {
                MessageBox.Show("This name is reserved. Please choose a different name.", "Error");
                return;
            }

            string sceneName = textBoxCustomLocScene.Text;
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                MessageBox.Show("Please enter a scene name.", "Error");
                return;
            }

            decimal x = numericUpDownCustLocX.Value;
            decimal y = numericUpDownCustLocY.Value;

            MessageBox.Show("The custom location has been added to the main list.");

            customLocations[name] = ItemChanger.Custom.Locations.CreateCustomLocation(name, sceneName, (float)x, (float)y);
            comboBoxLocations.Items.Add(name);

            textBoxCustomLocName.Clear();
            textBoxCustomLocScene.Clear();
            numericUpDownCustLocX.Value = 0;
            numericUpDownCustLocY.Value = 0;

        }

        private void buttonCustLocHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Enter the coordinates and scene where you want your shiny to appear. You can find this information by using the DebugMod in-game menu.", "Help");
        }

        private void comboBoxGiveActionCust_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxBoolNameCust.Clear();
            textBoxIntNameCust.Clear();
            textBoxGeoCust.Clear();
            textBoxEssenceCust.Clear();
            textBoxAltBoolNameCust.Clear();

            textBoxBoolNameCust.Enabled = false;
            textBoxIntNameCust.Enabled = false;
            textBoxGeoCust.Enabled = false;
            textBoxEssenceCust.Enabled = false;
            textBoxAltBoolNameCust.Enabled = false;

            switch (comboBoxGiveActionCust.SelectedItem)
            {
                case nameof(Item.GiveAction.Bool):
                case nameof(Item.GiveAction.Charm):
                case nameof(Item.GiveAction.EquippedCharm):
                case nameof(Item.GiveAction.Map):
                case nameof(Item.GiveAction.Stag):
                    textBoxBoolNameCust.Enabled = true;
                    textBoxAltBoolNameCust.Enabled = true;
                    break;

                case nameof(Item.GiveAction.Int):
                    textBoxIntNameCust.Enabled = true;
                    break;

                case nameof(Item.GiveAction.AddGeo):
                case nameof(Item.GiveAction.SpawnGeo):
                    textBoxGeoCust.Enabled = true;
                    break;

                case nameof(Item.GiveAction.Essence):
                    textBoxEssenceCust.Enabled = true;
                    break;
            }
        }

        private void buttonAddCustItem_Click(object sender, EventArgs e)
        {
            string name = textBoxCustItemName.Text;
            if (string.IsNullOrWhiteSpace(name) || fullItemArray.Contains(name) || customItems.ContainsKey(name))
            {
                MessageBox.Show("This name is reserved. Please choose a different name.", "Error");
                return;
            }

            string typeString = comboBoxItemTypeCust.Text;
            if (!Enum.TryParse(typeString, out Item.ItemType type))
            {
                MessageBox.Show("Please select an Item Type.");
                return;
            }

            string actionString = comboBoxGiveActionCust.Text;
            if (!Enum.TryParse(actionString, out Item.GiveAction action))
            {
                MessageBox.Show("Please select a Give Action.");
                return;
            }

            if (type != Item.ItemType.Geo && action == Item.GiveAction.SpawnGeo)
            {
                MessageBox.Show("GiveAction SpawnGeo is only supported with ItemType Geo.");
                return;
            }

            Item item = new Item { name = name, type = type, action = action };

            switch (action)
            {
                default:
                    if (!string.IsNullOrWhiteSpace(textBoxBoolNameCust.Text)) item.boolName = textBoxBoolNameCust.Text;
                    if (!string.IsNullOrWhiteSpace(textBoxAltBoolNameCust.Text)) item.altBoolName = textBoxAltBoolNameCust.Text;
                    break;
                case Item.GiveAction.EquippedCharm:
                case Item.GiveAction.Charm:
                    if (!item.boolName.StartsWith("gotCharm_") || !Int32.TryParse(item.boolName.Split('_')[1], out int charmNum))
                    {
                        MessageBox.Show("Invalid charm bool. The primary bool name for a charm should be of the form gotCharm_X where X is numeric.");
                        return;
                    }
                    item.charmNum = charmNum;
                    item.equipBoolName = "equippedCharm_" + charmNum;
                    item.notchCost = "charmCost_" + charmNum;
                    goto default;
                case Item.GiveAction.Int:
                    if (!string.IsNullOrWhiteSpace(textBoxIntNameCust.Text)) item.intName = textBoxIntNameCust.Text;
                    break;
                case Item.GiveAction.AddGeo:
                case Item.GiveAction.SpawnGeo:
                    if (!Int32.TryParse(textBoxGeoCust.Text, out int geo))
                    {
                        MessageBox.Show("Invalid geo amount.");
                        return;
                    }
                    item.geo = geo;
                    break;
                case Item.GiveAction.Essence:
                    if (!Int32.TryParse(textBoxGeoCust.Text, out int essence))
                    {
                        MessageBox.Show("Invalid essence amount.");
                        return;
                    }
                    item.essence = essence;
                    break;
            }

            if (!string.IsNullOrWhiteSpace(textBoxAdditiveGroupCust.Text))
            {
                item.additiveGroup = textBoxAdditiveGroupCust.Text;
                item.additiveIndex = (int)numericUpDownAdditiveIndexCust.Value;
            }

            if (string.IsNullOrWhiteSpace(comboBoxNameKeyCust.Text))
            {
                MessageBox.Show("Please select a name key.");
                return;
            }
            item.nameKey = comboBoxNameKeyCust.Text;

            if (string.IsNullOrWhiteSpace(comboBoxShopDescCust.Text))
            {
                if (MessageBox.Show("Invalid shop desc key. Continue anyways?", "Error", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    return;
                }
            }
            else item.shopDescKey = comboBoxShopDescCust.Text;

            if (type == Item.ItemType.Big)
            {
                if (string.IsNullOrWhiteSpace(comboBoxTakeKeyCust.Text)
                    || string.IsNullOrWhiteSpace(comboBoxDescOneCust.Text)
                    || string.IsNullOrWhiteSpace(comboBoxDescTwoCust.Text))
                {
                    if (MessageBox.Show("Invalid big item definition. Continue anyways?", "Error", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                item.takeKey = comboBoxTakeKeyCust.Text;
                item.descOneKey = comboBoxDescOneCust.Text;
                item.descTwoKey = comboBoxDescTwoCust.Text;

                if (!XmlManager.bigSpriteNames.ContainsValue(comboBoxBigSpriteCust.Text) && !XmlManager.spriteNames.ContainsValue(comboBoxSpriteCust.Text))
                {
                    if (MessageBox.Show("Invalid big item sprite. Continue anyways?", "Error", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                else
                {
                    XmlManager.bigSpriteNames[name] = comboBoxBigSpriteCust.Text;
                }
            }
            if (!XmlManager.spriteNames.ContainsValue(comboBoxSpriteCust.Text) && ! XmlManager.bigSpriteNames.ContainsValue(comboBoxSpriteCust.Text))
            {
                if (MessageBox.Show("Invalid item sprite. Continue anyways?", "Error", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    return;
                }
            }
            else
            {
                XmlManager.spriteNames[name] = comboBoxSpriteCust.Text;
            }

            customItems[name] = item;
            comboBoxItems.Items.Add(name);
            MessageBox.Show($"Custom item {name} added to main list.");
        }

        private void comboBoxTemplateItemCust_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!XmlManager.Items.TryGetValue(comboBoxTemplateItemCust.SelectedItem.ToString(), out Item template)) return;

            comboBoxItemTypeCust.SelectedItem = template.type.ToString();
            comboBoxGiveActionCust.SelectedItem = template.action.ToString();

            switch (comboBoxGiveActionCust.SelectedItem)
            {
                case nameof(Item.GiveAction.Bool):
                case nameof(Item.GiveAction.Charm):
                case nameof(Item.GiveAction.EquippedCharm):
                case nameof(Item.GiveAction.Map):
                case nameof(Item.GiveAction.Stag):
                    textBoxBoolNameCust.Text = template.boolName ?? string.Empty;
                    textBoxAltBoolNameCust.Text = template.altBoolName ?? string.Empty;
                    break;

                case nameof(Item.GiveAction.Int):
                    textBoxIntNameCust.Text = template.intName ?? string.Empty;
                    break;

                case nameof(Item.GiveAction.AddGeo):
                case nameof(Item.GiveAction.SpawnGeo):
                    textBoxGeoCust.Text = template.geo.ToString();
                    break;

                case nameof(Item.GiveAction.Essence):
                    textBoxEssenceCust.Text = template.essence.ToString();
                    break;
            }

            textBoxAdditiveGroupCust.Text = template.additiveGroup ?? string.Empty;
            numericUpDownAdditiveIndexCust.Value = template.additiveIndex;

            if (XmlManager.spriteNames.TryGetValue(template.name, out string spriteName))
            {
                comboBoxSpriteCust.SelectedItem = spriteName;
            }
            comboBoxNameKeyCust.SelectedItem = template.nameKey;
            comboBoxShopDescCust.SelectedItem = template.shopDescKey;

            if (template.type != Item.ItemType.Big) return;
            if (XmlManager.bigSpriteNames.TryGetValue(template.name, out string bigSpriteName))
            {
                comboBoxBigSpriteCust.SelectedItem = bigSpriteName;
            }
            comboBoxTakeKeyCust.SelectedItem = template.takeKey;
            comboBoxButtonKeyCust.SelectedItem = template.buttonKey;
            comboBoxDescOneCust.SelectedItem = template.descOneKey;
            comboBoxDescTwoCust.SelectedItem = template.descTwoKey;
        }

        private void buttonCustomTextAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxCustomTextSheet.Text) || string.IsNullOrWhiteSpace(textBoxCustomTextKey.Text))
            {
                MessageBox.Show("Please enter a valid sheet and key.", "Error");
                return;
            }
            listBoxCustomText.Items.Add(new LanguageEntry(
                _sheet: textBoxCustomTextSheet.Text,
                _key: textBoxCustomTextKey.Text,
                _text: textBoxCustomTextVal.Text
                ));
        }

        private void buttonCustomTextRemove_Click(object sender, EventArgs e)
        {
            listBoxCustomText.Items.Remove(listBoxCustomText.SelectedItem);
        }

        private void buttonCustomTextHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Enter a sheet, key, and text.\n" +
                "\nThe game will retrieve the text when it calls for the corresponding sheet and key.\n" +
                "\nThe UI sheet is used for item names and shop descriptions.\n" +
                "\nThe Prompts sheet is used for all parts of the big item popup, except for the item name.\n" +
                "\nFor a custom item, you may freely choose the names of your keys.\n" +
                "\nYou can also modify other text that appears in-game by changing the value of the corresponding sheet/key.\n" +
                "\nYour key will not appear in the drop-down box on the custom item page. However, you can type it manually into the box.", "Help");
        }

        private void buttonCustItemHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Select a template item from the drop-down box to prepopulate the form with values." +
                "\n\n Type Big items have a popup cutscene when collected. Type Geo items spawn geo. All other items have their name and sprite appear at the bottom of the screen." +
                "\n\n Give action determines the effect your item has when collected. Most effects should be self-explanatory. Use the altBoolName field when you need to set two flags true." +
                "\n\n For more complicated give actions, you can use the customAction field of Item in a mod." +
                "\n\n You can use any of the sprites used by ItemChanger for shop sprites or big item sprites, by referring to them by name. For custom sprites, you must load them yourself in a mod." +
                "\n\n The nameKey is used to retrieve the name of your item, as displayed anywhere in the UI. If your item does not have an existing key, you can create one in the Custom Text tab." +
                "\n\n The rightmost column consists of the data needed to build the big item popup." +
                "\n\n The take key is the top text (e.g. \"Taken the\", \"Consumed the\")" +
                "\n\n The button key is the text that tells what button to use for the corresponding item. Referring to actual button names is not currently supported." +
                "\n\n The description keys give the description text below the item.",
                "Help");
        }

        private void comboBoxItemTypeCust_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxBigSpriteCust.Enabled = false;
            comboBoxTakeKeyCust.Enabled = false;
            comboBoxButtonKeyCust.Enabled = false;
            comboBoxDescOneCust.Enabled = false;
            comboBoxDescTwoCust.Enabled = false;
            comboBoxBigSpriteCust.Text = null;
            comboBoxTakeKeyCust.Text = null;
            comboBoxButtonKeyCust.Text = null;
            comboBoxDescOneCust.Text = null;
            comboBoxDescTwoCust.Text = null;
            comboBoxGiveActionCust.Enabled = true;
            textBoxAdditiveGroupCust.Enabled = true;
            numericUpDownAdditiveIndexCust.Enabled = true;

            switch (comboBoxItemTypeCust.SelectedItem)
            {
                case "Big":
                    comboBoxBigSpriteCust.Enabled = true;
                    comboBoxTakeKeyCust.Enabled = true;
                    comboBoxButtonKeyCust.Enabled = true;
                    comboBoxDescOneCust.Enabled = true;
                    comboBoxDescTwoCust.Enabled = true;
                    break;

                case "Geo":
                    comboBoxGiveActionCust.SelectedItem = "SpawnGeo";
                    comboBoxGiveActionCust.Enabled = false;
                    textBoxAdditiveGroupCust.Text = null;
                    textBoxAdditiveGroupCust.Enabled = false;
                    numericUpDownAdditiveIndexCust.Value = 0;
                    numericUpDownAdditiveIndexCust.Enabled = false;

                    break;
            }
        }

        private void buttonDeleteItem_Click(object sender, EventArgs e)
        {
            //if (customItems.ContainsKey((string)discardItemBox.SelectedItem))
            {
                string item = (string)discardItemBox.SelectedItem;
                discardItemBox.Items.Remove(item);
                customItems.Remove(item);
            }
            //else MessageBox.Show("Cannot delete a default item!", "Warning");
        }

        private void buttonDeleteLoc_Click(object sender, EventArgs e)
        {
            //if (customLocations.ContainsKey((string)discardLocBox.SelectedItem))
            {
                string location = (string)discardLocBox.SelectedItem;
                discardLocBox.Items.Remove(location);
                customLocations.Remove(location);
            }
            //else MessageBox.Show("Cannot delete a default location!", "Warning");
        }

        private void discardItemBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (discardItemBox.SelectedItem != null && customItems.ContainsKey((string)discardItemBox.SelectedItem))
            {
                buttonDeleteItem.Enabled = true;
            }
            else
            {
                buttonDeleteItem.Enabled = false;
            }
        }

        private void discardLocBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (discardLocBox.SelectedItem != null && customLocations.ContainsKey((string)discardLocBox.SelectedItem))
            {
                buttonDeleteLoc.Enabled = true;
            }
            else
            {
                buttonDeleteLoc.Enabled = false;
            }
        }

        private void SetDefaultGameObjectControls()
        {
            if (!(comboBoxDefaultGameObjects.SelectedItem is string obj) || !DefaultGameObjectInfo.defaultObjects.TryGetValue(obj, out DefaultGameObjectInfo info)) return;
            numericUpDownDefaultObjectTwoX.Value = 0;
            numericUpDownDefaultObjectTwoY.Value = 0;
            numericUpDownDefaultObjectTwoX.Enabled = info.useDeployTwo;
            numericUpDownDefaultObjectTwoY.Enabled = info.useDeployTwo;
            labelDefaultObjectParamOne.Text = info.paramOneLabel;
            labelDefaultObjectParamTwo.Text = info.paramTwoLabel;
            textBoxDefaultObjectParamOne.Enabled = info.useParamOne;
            textBoxDefaultObjectParamTwo.Enabled = info.useParamTwo;
            labelDefaultObjectOneX.Text = info.deployOneXLabel;
            labelDefaultObjectOneY.Text = info.deployOneYLabel;
            labelDefaultObjectTwoX.Text = info.deployTwoXLabel;
            labelDefaultObjectTwoY.Text = info.deployTwoYLabel;
        }

        private void comboBoxDefaultGameObjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            numericUpDownDefaultObjectTwoX.Value = 0;
            numericUpDownDefaultObjectTwoY.Value = 0;
            numericUpDownDefaultObjectTwoX.Enabled = false;
            numericUpDownDefaultObjectTwoY.Enabled = false;
            textBoxDefaultObjectParamOne.Text = null;
            textBoxDefaultObjectParamTwo.Text = null;
            textBoxDefaultObjectParamOne.Enabled = false;
            textBoxDefaultObjectParamTwo.Enabled = false;

            SetDefaultGameObjectControls();
        }

        private void buttonAddDefaultGameObject_Click(object sender, EventArgs e)
        {
            if (!(comboBoxDefaultGameObjects.SelectedItem is string name) || string.IsNullOrEmpty(name)) return;
            string paramOne = textBoxDefaultObjectParamOne.Text;
            string paramTwo = textBoxDefaultObjectParamTwo.Text;

            switch (name)
            {
                case "Baldur":
                case "Vengefly":
                case "Toll Gate":
                    if (!int.TryParse(paramOne, out int val) || val < 1)
                    {
                        MessageBox.Show("Parameter one must be a positive integer.", "Error");
                        return;
                    }
                    break;
                case "Spike":
                    if (!float.TryParse(paramOne, out _))
                    {
                        MessageBox.Show("Parameter one must be a floating point number.", "Error");
                        return;
                    }
                    break;
            }
            switch (name)
            {
                case "Baldur":
                    if (!bool.TryParse(paramTwo, out _))
                    {
                        MessageBox.Show("Parameter two must be \"true\" or \"false\".", "Error");
                        return;
                    }
                    break;
            }

            listBoxSpecialActions.Items.Add(new AddSpecialGameObjectAction
            {
                specialName = name,
                deployOneX = (float)numericUpDownDefaultObjectOneX.Value,
                deployOneY = (float)numericUpDownDefaultObjectOneY.Value,
                deployTwoX = (float)numericUpDownDefaultObjectTwoX.Value,
                deployTwoY = (float)numericUpDownDefaultObjectTwoY.Value,
                deploySceneName = textBoxDefaultObjectDeployScene.Text,
                paramOne = paramOne,
                paramTwo = paramTwo,
            });
        }

        private void buttonAddPreloadGameObject_Click(object sender, EventArgs e)
        {
            listBoxSpecialActions.Items.Add(new AddPreloadedGameObjectAction
            {
                deploySceneName = textBoxPreloadGameObjectDeployScene.Text,
                originalSceneName = textBoxPreloadGameObjectFromScene.Text,
                originalObjectName = textBoxPreloadGameObjectName.Text,
                deployX = (float)numericUpDownPreloadGameObjectX.Value,
                deployY = (float)numericUpDownPreloadGameObjectY.Value
            });
        }

        private void buttonAddDestroyObject_Click(object sender, EventArgs e)
        {
            listBoxSpecialActions.Items.Add(new DestroyGameObjectAction
            {
                originalSceneName = textBoxDestroyObjectScene.Text,
                originalObjectName = textBoxDestroyObjectName.Text,
                destroyAllThatMatch = checkBoxDestroyObjectNameMatch.Checked,
            });
        }

        private void buttonAddOverrideSceneDarkness_Click(object sender, EventArgs e)
        {
            listBoxSpecialActions.Items.Add(new OverrideDarknessAction
            {
                sceneName = textBoxOverrideDarknessScene.Text,
                darknessLevel = (int)numericUpDownOverrideDarknessLevel.Value,
            });
        }

        private void buttonRemoveSpecialAction_Click(object sender, EventArgs e)
        {
            listBoxSpecialActions.Items.Remove(listBoxSpecialActions.SelectedItem);
        }

        private void checkBoxEditCosts_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxEditCosts.Checked)
            {
                costUpDown.Enabled = true;
                comboBoxCostType.Enabled = true;
                comboBoxCostType.Text = comboBoxCostType.SelectedItem.ToString();
            }
            else
            {
                comboBoxCostType.SelectedItem = "None";
                costUpDown.Value = 0;
                costUpDown.Enabled = false;
                comboBoxCostType.Enabled = false;
            }
        }

        private void checkedListBoxDiscardItemPools_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            Item.ItemPool pool = (Item.ItemPool)checkedListBoxDiscardItemPools.Items[e.Index];
            if (e.NewValue == CheckState.Checked)
            {
                foreach (Item item in XmlManager.Items.Values.Where(i => i.pool == pool))
                {
                    if (discardItemBox.Items.Contains(item.name))
                    {
                        discardItemBox.Items.Remove(item.name);
                        comboBoxItems.Items.Add(item.name);
                    }
                }
            }
            else
            {
                foreach (Item item in XmlManager.Items.Values.Where(i => i.pool == pool))
                {
                    if (comboBoxItems.Items.Contains(item.name))
                    {
                        discardItemBox.Items.Add(item.name);
                        comboBoxItems.Items.Remove(item.name);
                    }
                }
            }
            
        }

        private void checkedListBoxDiscardLocationPools_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            Location.LocationPool pool = (Location.LocationPool)checkedListBoxDiscardLocationPools.Items[e.Index];
            if (e.NewValue == CheckState.Checked)
            {
                foreach (Location loc in XmlManager.Locations.Values.Where(i => i.pool == pool))
                {
                    if (discardLocBox.Items.Contains(loc.name))
                    {
                        discardLocBox.Items.Remove(loc.name);
                        comboBoxLocations.Items.Add(loc.name);
                    }
                }
            }
            else
            {
                foreach (Location location in XmlManager.Locations.Values.Where(i => i.pool == pool))
                {
                    if (comboBoxLocations.Items.Contains(location.name))
                    {
                        discardLocBox.Items.Add(location.name);
                        comboBoxLocations.Items.Remove(location.name);
                    }
                }
            }
        }
    }
}
