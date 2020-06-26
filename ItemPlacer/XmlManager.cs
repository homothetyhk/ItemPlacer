using ItemChanger;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace ItemPlacer
{
    internal static class XmlManager
    {
        public static List<ILP> xmlILPs;
        public static string xmlDefaultShopPreset;
        public static string xmlTitle;
        public static bool xmlChangeStart;
        public static StartLocation xmlStartLocation;
        public static List<Item> xmlCustomItems;
        public static List<Location> xmlCustomLocations;
        public static List<LanguageEntry> xmlCustomText;
        public static List<object> xmlSpecialActions;

        public static ItemChangerSettings settings;

        
        // cached reflection
        static FieldInfo[] specialActionFields = typeof(AddSpecialGameObjectAction).GetFields(BindingFlags.Instance | BindingFlags.Public);
        static FieldInfo[] preloadActionFields = typeof(AddPreloadedGameObjectAction).GetFields(BindingFlags.Instance | BindingFlags.Public);
        static FieldInfo[] destroyActionFields = typeof(DestroyGameObjectAction).GetFields(BindingFlags.Instance | BindingFlags.Public);
        static FieldInfo[] overrideDarknessFields = typeof(OverrideDarknessAction).GetFields(BindingFlags.Instance | BindingFlags.Public);

        public static bool TryLoad(string fileName)
        {
            XmlDocument doc;

            try
            {
                FileStream stream = File.OpenRead(fileName);
                doc = new XmlDocument();
                doc.Load(stream);
                stream.Dispose();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error loading file: " + e);
                return false;
            }

            try
            {
                xmlILPs = new List<ILP>();
                foreach (XmlNode node in doc.SelectNodes("randomizer/ilp"))
                {
                    string item = null;
                    string location = null;
                    int cost = 0;
                    string costType = null;
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        switch (child.Name)
                        {
                            case "item": 
                                item = child.InnerText;
                                break;
                            case "location":
                                location = child.InnerText;
                                break;
                            case "cost":
                                cost = Int32.Parse(child.InnerText);
                                break;
                            case "costType":
                                costType = child.InnerText;
                                break;
                        }
                    }
                    if (string.IsNullOrEmpty(item) || string.IsNullOrEmpty(location)) continue;
                    xmlILPs.Add(new ILP(item, location, cost, costType));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error reading placement data. Xml load aborted: " + e);
                return false;
            }

            try
            {
                xmlDefaultShopPreset = string.Empty;
                foreach (XmlNode node in doc.SelectNodes("randomizer/defaultShopItems"))
                {
                    xmlDefaultShopPreset = node.InnerText;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error reading shop data. Xml load aborted: " + e);
                return false;
            }

            try
            {
                xmlTitle = string.Empty;
                foreach (XmlNode node in doc.SelectNodes("randomizer/title"))
                {
                    xmlTitle = node.InnerText;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error reading title. Xml load aborted: " + e);
                return false;
            }

            try
            {
                if (doc.SelectSingleNode("randomizer/startSceneName") is XmlNode startSceneNode
                    && doc.SelectSingleNode("randomizer/startX") is XmlNode startXNode
                    && doc.SelectSingleNode("randomizer/startY") is XmlNode startYNode)
                {
                    xmlChangeStart = true;
                    xmlStartLocation = new StartLocation
                    {
                        startSceneName = startSceneNode.InnerText,
                        startX = float.Parse(startXNode.InnerText),
                        startY = float.Parse(startYNode.InnerText)
                    };
                }
                else xmlChangeStart = false;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error reading start location changes. Xml load aborted: " + e);
            }


            try
            {
                settings = new ItemChangerSettings();
                foreach (FieldInfo field in typeof(ItemChangerSettings).GetFields(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (doc.SelectSingleNode("randomizer/" + field.Name) is XmlNode node)
                    {
                        if (field.FieldType == typeof(bool)) field.SetValue(settings, bool.Parse(node.InnerText));
                        else if (field.FieldType == typeof(string)) field.SetValue(settings, node.InnerText);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error reading ItemChangerSettings. Xml load aborted: " + e);
                return false;
            }

            try
            {
                xmlCustomItems = new List<Item>();
                foreach (XmlNode node in doc.SelectNodes("randomizer/newItem"))
                {
                    xmlCustomItems.Add(ProcessXmlNodeAsItem(node));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error reading saved custom items. Xml load aborted: " + e);
                return false;
            }

            try
            {
                xmlCustomLocations = new List<Location>();
                foreach (XmlNode node in doc.SelectNodes("randomizer/newLocation"))
                {
                    string name = node.Attributes?["name"].InnerText;
                    string sceneName = null;
                    float x = 0;
                    float y = 0;

                    foreach (XmlNode child in node.ChildNodes)
                    {
                        if (child.Name == "sceneName") sceneName = child.InnerText;
                        if (child.Name == "x") x = float.Parse(child.InnerText);
                        if (child.Name == "y") y = float.Parse(child.InnerText);
                    }
                    xmlCustomLocations.Add(ItemChanger.Custom.Locations.CreateCustomLocation(name, sceneName, x, y));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error reading saved custom locations. Xml load aborted: " + e);
                return false;
            }

            try
            {
                xmlCustomText = new List<LanguageEntry>();
                foreach (XmlNode node in doc.SelectNodes("randomizer/languageEntry"))
                {
                    string sheet = null;
                    string key = null;
                    string text = null;

                    foreach (XmlNode child in node.ChildNodes)
                    {
                        if (child.Name == "sheet") sheet = child.InnerText;
                        if (child.Name == "key") key = child.InnerText;
                        if (child.Name == "text") text = child.InnerText;
                    }
                    xmlCustomText.Add(new LanguageEntry(sheet, key, text));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error reading saved custom text. Xml load aborted: " + e);
                return false;
            }

            try
            {
                xmlSpecialActions = new List<object>();
                foreach (XmlNode node in doc.SelectNodes("randomizer/addPreloadGameObject"))
                {
                    AddPreloadedGameObjectAction action = new AddPreloadedGameObjectAction();
                    foreach (FieldInfo field in preloadActionFields)
                    {
                        if (node[field.Name] is null) continue;

                        if (field.FieldType == typeof(bool))
                        {
                            field.SetValue(action, bool.Parse(node[field.Name].InnerText));
                        }

                        else if (field.FieldType == typeof(string))
                        {
                            field.SetValue(action, node[field.Name].InnerText);
                        }

                        else if (field.FieldType == typeof(float))
                        {
                            field.SetValue(action, float.Parse(node[field.Name].InnerText));
                        }

                        else if (field.FieldType == typeof(int))
                        {
                            field.SetValue(action, int.Parse(node[field.Name].InnerText));
                        }
                    }
                    xmlSpecialActions.Add(action);
                }
                foreach (XmlNode node in doc.SelectNodes("randomizer/addDestroyGameObject"))
                {
                    DestroyGameObjectAction action = new DestroyGameObjectAction();
                    foreach (FieldInfo field in destroyActionFields)
                    {
                        if (node[field.Name] is null) continue;

                        if (field.FieldType == typeof(bool))
                        {
                            field.SetValue(action, bool.Parse(node[field.Name].InnerText));
                        }

                        else if (field.FieldType == typeof(string))
                        {
                            field.SetValue(action, node[field.Name].InnerText);
                        }

                        else if (field.FieldType == typeof(float))
                        {
                            field.SetValue(action, float.Parse(node[field.Name].InnerText));
                        }

                        else if (field.FieldType == typeof(int))
                        {
                            field.SetValue(action, int.Parse(node[field.Name].InnerText));
                        }
                    }
                    xmlSpecialActions.Add(action);
                }

                foreach (XmlNode node in doc.SelectNodes("randomizer/addSpecialGameObject"))
                {
                    AddSpecialGameObjectAction action = new AddSpecialGameObjectAction();
                    foreach (FieldInfo field in specialActionFields)
                    {
                        if (node[field.Name] is null) continue;

                        if (field.FieldType == typeof(bool))
                        {
                            field.SetValue(action, bool.Parse(node[field.Name].InnerText));
                        }

                        else if (field.FieldType == typeof(string))
                        {
                            field.SetValue(action, node[field.Name].InnerText);
                        }

                        else if (field.FieldType == typeof(float))
                        {
                            field.SetValue(action, float.Parse(node[field.Name].InnerText));
                        }

                        else if (field.FieldType == typeof(int))
                        {
                            field.SetValue(action, int.Parse(node[field.Name].InnerText));
                        }
                    }
                    xmlSpecialActions.Add(action);
                }

                foreach (XmlNode node in doc.SelectNodes("randomizer/overrideDarkness"))
                {
                    OverrideDarknessAction action = new OverrideDarknessAction();
                    foreach (FieldInfo field in overrideDarknessFields)
                    {
                        if (node[field.Name] is null) continue;

                        if (field.FieldType == typeof(bool))
                        {
                            field.SetValue(action, bool.Parse(node[field.Name].InnerText));
                        }

                        else if (field.FieldType == typeof(string))
                        {
                            field.SetValue(action, node[field.Name].InnerText);
                        }

                        else if (field.FieldType == typeof(float))
                        {
                            field.SetValue(action, float.Parse(node[field.Name].InnerText));
                        }

                        else if (field.FieldType == typeof(int))
                        {
                            field.SetValue(action, int.Parse(node[field.Name].InnerText));
                        }
                    }
                    xmlSpecialActions.Add(action);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error reading saved special actions. Xml load aborted: " + e);
                return false;
            }

            return true;
        }

        public static void Save(
            string fileName, 
            IEnumerator ilps, 
            string defaultShopPreset, 
            string title, 
            ItemChangerSettings settings, 
            bool changeStart, 
            StartLocation startLocation,
            Dictionary<string,Item> customItems,
            Dictionary<string,Location> customLocations,
            IEnumerator LanguageEntries,
            IEnumerator SpecialActions
            )
        {
            XmlWriter xw = null;
            try
            {
                xw = XmlWriter.Create(fileName, new XmlWriterSettings { Indent = true, });
                xw.WriteStartDocument();
                xw.WriteStartElement("randomizer");
                if (!string.IsNullOrEmpty(title))
                {
                    xw.WriteStartElement("title");
                    xw.WriteString(title);
                    xw.WriteEndElement();
                }
                if (!string.IsNullOrEmpty(defaultShopPreset))
                {
                    xw.WriteStartElement("defaultShopItems");
                    xw.WriteString(defaultShopPreset);
                    xw.WriteEndElement();
                }
                foreach (FieldInfo field in typeof(ItemChangerSettings).GetFields(BindingFlags.Instance | BindingFlags.Public))
                {
                    if (field.FieldType != typeof(bool)) continue;
                    xw.WriteStartElement(field.Name);
                    xw.WriteString(field.GetValue(settings).ToString());
                    xw.WriteEndElement();
                }
                if (changeStart)
                {
                    xw.WriteStartElement("startSceneName");
                    xw.WriteString(startLocation.startSceneName);
                    xw.WriteEndElement();

                    xw.WriteStartElement("startX");
                    xw.WriteString(startLocation.startX.ToString());
                    xw.WriteEndElement();

                    xw.WriteStartElement("startY");
                    xw.WriteString(startLocation.startY.ToString());
                    xw.WriteEndElement();
                }
                while (ilps.MoveNext())
                {
                    if (ilps.Current is ILP ilp)
                    {
                        xw.WriteStartElement("ilp");
                        xw.WriteElementString("item", ilp.item);
                        xw.WriteElementString("location", ilp.location);
                        if (ilp.editCost)
                        {
                            xw.WriteElementString("cost", ilp.cost.ToString());
                            xw.WriteElementString("costType", ilp.costType.ToString());
                        }
                        xw.WriteEndElement();
                    }
                }
                foreach (Item item in customItems.Values)
                {
                    xw.WriteStartElement("newItem");
                    xw.WriteAttributeString("name", item.name);
                    if (item.type != Item.ItemType.Generic)
                    {
                        xw.WriteElementString("type", item.type.ToString());
                    }
                    if (item.action != Item.GiveAction.Bool)
                    {
                        xw.WriteElementString("action", item.action.ToString());
                    }
                    if (item.boolName != null)
                    {
                        xw.WriteElementString("boolName", item.boolName);
                    }
                    if (item.altBoolName != null)
                    {
                        xw.WriteElementString("altBoolName", item.altBoolName);
                    }
                    if (item.intName != null)
                    {
                        xw.WriteElementString("intName", item.intName);
                    }
                    if (item.geo != 0)
                    {
                        xw.WriteElementString("geo", item.geo.ToString());
                    }
                    if (item.essence != 0)
                    {
                        xw.WriteElementString("essence", item.essence.ToString());
                    }
                    if (item.shopPrice != 0)
                    {
                        xw.WriteElementString("shopPrice", item.shopPrice.ToString());
                    }
                    if (item.charmNum != 0)
                    { 
                        xw.WriteElementString("charmNum", item.charmNum.ToString());
                    }
                    if (item.equipBoolName != null)
                    {
                        xw.WriteElementString("equipBoolName", item.equipBoolName);
                    }
                    if (item.notchCost != null)
                    {
                        xw.WriteElementString("notchCost", item.notchCost);
                    }
                    if (spriteNames.TryGetValue(item.name, out string spriteName))
                    {
                        xw.WriteElementString("sprite", spriteName);
                    }
                    if (item.nameKey != null)
                    {
                        xw.WriteElementString("nameKey", item.nameKey);
                    }
                    if (item.shopDescKey != null)
                    {
                        xw.WriteElementString("shopDescKey", item.shopDescKey);
                    }
                    if (bigSpriteNames.TryGetValue(item.name, out string bigSpriteName))
                    {
                        xw.WriteElementString("bigSprite", bigSpriteName);
                    }
                    if (item.takeKey != null)
                    {
                        xw.WriteElementString("takeKey", item.takeKey);
                    }
                    if (item.buttonKey != null)
                    {
                        xw.WriteElementString("buttonKey", item.buttonKey);
                    }
                    if (item.descOneKey != null)
                    {
                        xw.WriteElementString("descOneKey", item.descOneKey);
                    }
                    if (item.descTwoKey != null)
                    {
                        xw.WriteElementString("descTwoKey", item.descTwoKey);
                    }
                    xw.WriteEndElement();
                }

                foreach (Location location in customLocations.Values)
                {
                    xw.WriteStartElement("newLocation");
                    xw.WriteAttributeString("name", location.name);
                    xw.WriteElementString("sceneName", location.sceneName);
                    xw.WriteElementString("newShiny", location.newShiny.ToString());
                    xw.WriteElementString("x", location.x.ToString());
                    xw.WriteElementString("y", location.y.ToString());
                    xw.WriteEndElement();
                }
                while (LanguageEntries.MoveNext())
                {
                    if (LanguageEntries.Current is LanguageEntry entry)
                    {
                        xw.WriteStartElement("languageEntry");
                        xw.WriteElementString("sheet", entry.sheet);
                        xw.WriteElementString("key", entry.key);
                        xw.WriteElementString("text", entry.text);
                        xw.WriteEndElement();
                    }
                }

                

                while (SpecialActions.MoveNext())
                {
                    if (SpecialActions.Current is AddSpecialGameObjectAction addSpecial)
                    {
                        xw.WriteStartElement("addSpecialGameObject");
                        foreach(FieldInfo field in specialActionFields)
                        {
                            if (field.FieldType == typeof(string) && field.GetValue(addSpecial) is string str)
                            {
                                xw.WriteElementString(field.Name, str);
                            }
                            if (field.FieldType == typeof(float) && field.GetValue(addSpecial) is float fl && fl != 0)
                            {
                                xw.WriteElementString(field.Name, fl.ToString());
                            }
                        }
                        xw.WriteEndElement();
                    }
                    else if (SpecialActions.Current is AddPreloadedGameObjectAction addPreload)
                    {
                        xw.WriteStartElement("addPreloadGameObject");
                        foreach (FieldInfo field in preloadActionFields)
                        {
                            if (field.FieldType == typeof(string) && field.GetValue(addPreload) is string str)
                            {
                                xw.WriteElementString(field.Name, str);
                            }
                            if (field.FieldType == typeof(float) && field.GetValue(addPreload) is float fl && fl != 0)
                            {
                                xw.WriteElementString(field.Name, fl.ToString());
                            }
                        }
                        xw.WriteEndElement();
                    }
                    else if (SpecialActions.Current is DestroyGameObjectAction addDestroy)
                    {
                        xw.WriteStartElement("addDestroyGameObject");
                        foreach (FieldInfo field in destroyActionFields)
                        {
                            if (field.FieldType == typeof(string) && field.GetValue(addDestroy) is string str)
                            {
                                xw.WriteElementString(field.Name, str);
                            }
                            if (field.FieldType == typeof(bool))
                            {
                                xw.WriteElementString(field.Name, field.GetValue(addDestroy).ToString());
                            }

                        }
                        xw.WriteEndElement();
                    }
                    else if (SpecialActions.Current is OverrideDarknessAction overrideDarkness)
                    {
                        xw.WriteStartElement("overrideDarkness");
                        foreach (FieldInfo field in overrideDarknessFields)
                        {
                            if (field.FieldType == typeof(string) && field.GetValue(overrideDarkness) is string str)
                            {
                                xw.WriteElementString(field.Name, str);
                            }
                            else if (field.FieldType == typeof(int) && field.GetValue(overrideDarkness) is int i)
                            {
                                xw.WriteElementString(field.Name, i.ToString());
                            }
                        }
                        xw.WriteEndElement();
                    }
                }

                xw.WriteEndElement();
                xw.WriteEndDocument();
                xw.Flush();
                xw.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error saving file: " + e);
            }
            finally
            {
                if (xw != null) xw.Close();
            }
        }

        public static Dictionary<string, Item> Items;
        public static Dictionary<string, string> spriteNames;
        public static Dictionary<string, string> bigSpriteNames;
        private static XmlDocument items;

        public static void FetchItemDefs(Stream itemStream)
        {
            //Type type = typeof(ItemChanger.ItemChanger);
            //Assembly a = type.Assembly;
            //Stream itemStream = a.GetManifestResourceStream("ItemChanger.Resources.items.xml");
            items = new XmlDocument();
            items.Load(itemStream);
            itemStream.Dispose();

            spriteNames = new Dictionary<string, string>();
            bigSpriteNames = new Dictionary<string, string>();

            Items = new Dictionary<string, Item>();
            foreach (XmlNode node in items.SelectNodes("randomizer/item"))
            {
                Item item = ProcessXmlNodeAsItem(node);
                Items.Add(item.name, item);
            }
        }

        static Dictionary<string, FieldInfo> itemFields;
        public static Item ProcessXmlNodeAsItem(XmlNode node)
        {
            if (itemFields == null)
            {
                itemFields = new Dictionary<string, FieldInfo>();
                typeof(Item).GetFields().ToList().ForEach(f => itemFields.Add(f.Name, f));
            }

            XmlAttribute nameAttr = node.Attributes?["name"];
            if (nameAttr == null)
            {
                
                return new Item();
            }

            // Setting as object prevents boxing in FieldInfo.SetValue calls
            object item = new Item();
            itemFields["name"].SetValue(item, nameAttr.InnerText);

            foreach (XmlNode fieldNode in node.ChildNodes)
            {
                if (fieldNode.Name == "sprite")
                {
                    spriteNames[nameAttr.InnerText] = fieldNode.InnerText;
                    continue;
                }
                if (fieldNode.Name == "bigSprite")
                {
                    bigSpriteNames[nameAttr.InnerText] = fieldNode.InnerText;
                    continue;
                }

                if (!itemFields.TryGetValue(fieldNode.Name, out FieldInfo field))
                {
                    
                    continue;
                }

                if (field.FieldType == typeof(string))
                {
                    field.SetValue(item, fieldNode.InnerText);
                }
                else if (field.FieldType == typeof(bool))
                {
                    if (bool.TryParse(fieldNode.InnerText, out bool xmlBool))
                    {
                        field.SetValue(item, xmlBool);
                    }
                    else
                    {
                        
                    }
                }
                else if (field.FieldType == typeof(Item.ItemType))
                {
                    if (Enum.TryParse(fieldNode.InnerText, out Item.ItemType type))
                    {
                        field.SetValue(item, type);
                    }
                    else
                    {
                        
                    }
                }
                else if (field.FieldType == typeof(Item.GiveAction))
                {
                    if (Enum.TryParse(fieldNode.InnerText, out Item.GiveAction type))
                    {
                        field.SetValue(item, type);
                    }
                    else
                    {
                        
                    }
                }
                else if (field.FieldType == typeof(int))
                {
                    if (int.TryParse(fieldNode.InnerText, out int xmlInt))
                    {
                        field.SetValue(item, xmlInt);
                    }
                    else
                    {
                        
                    }
                }
                else if (field.FieldType == typeof(float))
                {
                    if (float.TryParse(fieldNode.InnerText, out float xmlFloat))
                    {
                        field.SetValue(item, xmlFloat);
                    }
                    else
                    {
                        
                    }
                }
                
                else
                {
                    
                }
            }

            
            return (Item)item;
        }

    }
}
