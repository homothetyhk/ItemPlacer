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

namespace ItemPlacer
{
    public partial class ItemPlacer : Form
    {
        string[] fullItemArray;
        string[] fullLocationArray;
        FieldInfo[] defaultShopPresets;
        HashSet<string> fullyPlacedItems;
        HashSet<string> filledLocations;
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
            InitializeComponent();
            fullItemArray = typeof(ItemNames).GetFields().Where(field => field.FieldType == typeof(string)).Select(field => (string)field.GetRawConstantValue()).ToArray();
            fullLocationArray = typeof(LocationNames).GetFields().Where(field => field.FieldType == typeof(string)).Select(field => (string)field.GetRawConstantValue()).ToArray();
            defaultShopPresets = typeof(Shops).GetFields().Where(field => field.FieldType == typeof(Shops.DefaultShopItems) && field.IsLiteral && !field.IsInitOnly).ToArray();
            fullyPlacedItems = new HashSet<string>();
            filledLocations = new HashSet<string>();

            comboBoxItems.Items.AddRange(fullItemArray);
            comboBoxLocations.Items.AddRange(fullLocationArray);
            comboBoxDefaultShopItems.Items.AddRange(defaultShopPresets                
                .Select(field => field.Name).ToArray());
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
            if (!fullItemArray.Contains(item) || !fullLocationArray.Contains(location)) return;

            AddILP(new ILP(item, location, (int)costUpDown.Value));
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
                if (hidePlacedItems.Checked) comboBoxItems.Items.Add(item);
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
            }
            else
            {
                comboBoxLocations.Items.Clear();
                comboBoxLocations.Items.AddRange(fullLocationArray);
            }
        }

        private void hidePlacedItems_CheckedChanged(object sender, EventArgs e)
        {
            if (hidePlacedItems.Checked)
            {
                comboBoxItems.Items.Clear();
                comboBoxItems.Items.AddRange(fullItemArray.Except(fullyPlacedItems).ToArray());
            }
            else
            {
                comboBoxItems.Items.Clear();
                comboBoxItems.Items.AddRange(fullItemArray);
            }
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            fullyPlacedItems.Clear();
            filledLocations.Clear();
            comboBoxItems.Items.Clear();
            comboBoxLocations.Items.Clear();
            comboBoxItems.Items.AddRange(fullItemArray);
            comboBoxLocations.Items.AddRange(fullLocationArray);
            titleBox.Text = string.Empty;
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (XmlManager.TryLoad(openFileDialog1.FileName))
                {
                    resetButton_Click(null, null);
                    foreach (ILP ilp in XmlManager.ILPs) AddILP(ilp);
                    if (comboBoxDefaultShopItems.Items.Contains(XmlManager.defaultShopPreset))
                    {
                        comboBoxDefaultShopItems.SelectedItem = XmlManager.defaultShopPreset;
                    }
                    titleBox.Text = XmlManager.title ?? string.Empty;
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
                    removeBeastsDenHardSave = RemoveBeastsDenHardSave.Checked,
                    skipDreamerTextBeforeDreamNail = SkipDreamerTextBeforeDreamNail.Checked,
                    blockLegEaterDeath = blockLegEaterDeath.Checked,
                    oneBlueHPLifebloodCoreDoor = oneBlueHPLifebloodCoreDoor.Checked,


                    colo1ItemPrompt = colo1Prompt.Checked,
                    colo2ItemPrompt = colo2Prompt.Checked,
                    flowerQuestPrompt = flowerQuestPrompt.Checked,
                    whitePalacePrompt = whitePalacePrompt.Checked
                };

                XmlManager.Save(saveFileDialog1.FileName, listBox1.Items.GetEnumerator(), comboBoxDefaultShopItems.SelectedItem?.ToString(), titleBox.Text,
                    settings, changeStartCheckBox.Checked, startSceneNameBox.Text, numericUpDownX.Value, numericUpDownY.Value);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void changeStart_CheckedChanged(object sender, EventArgs e)
        {
            bool changeStart = changeStartCheckBox.Checked;
            startSceneNameBox.Enabled = changeStart;
            numericUpDownX.Enabled = changeStart;
            numericUpDownY.Enabled = changeStart;
        }
    }
}
