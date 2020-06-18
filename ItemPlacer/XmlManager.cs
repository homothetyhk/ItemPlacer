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
        public static List<ILP> ILPs;
        public static string defaultShopPreset;
        public static string title;

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
                ILPs = new List<ILP>();
                foreach (XmlNode node in doc.SelectNodes("randomizer/ilp"))
                {
                    string item = null;
                    string location = null;
                    int cost = 0;
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
                        }
                    }
                    if (string.IsNullOrEmpty(item) || string.IsNullOrEmpty(location)) continue;
                    ILPs.Add(new ILP(item, location, cost));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error reading placement data. Xml load aborted: " + e);
                return false;
            }

            try
            {
                defaultShopPreset = string.Empty;
                foreach (XmlNode node in doc.SelectNodes("randomizer/defaultShopItems"))
                {
                    defaultShopPreset = node.InnerText;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error reading shop data. Xml load aborted: " + e);
                return false;
            }

            try
            {
                title = string.Empty;
                foreach (XmlNode node in doc.SelectNodes("randomizer/title"))
                {
                    title = node.InnerText;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error reading title. Xml load aborted: " + e);
                return false;
            }

            return true;
        }

        public static void Save(string fileName, IEnumerator ilps, string defaultShopPreset, string title, ItemChangerSettings settings, bool changeStart, string startScene, decimal x, decimal y)
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
                    xw.WriteString(startScene);
                    xw.WriteEndElement();

                    xw.WriteStartElement("startX");
                    xw.WriteString(x.ToString());
                    xw.WriteEndElement();

                    xw.WriteStartElement("startY");
                    xw.WriteString(y.ToString());
                    xw.WriteEndElement();
                }
                while (ilps.MoveNext())
                {
                    if (ilps.Current is ILP ilp)
                    {
                        xw.WriteStartElement("ilp");
                        xw.WriteElementString("item", ilp.item);
                        xw.WriteElementString("location", ilp.location);
                        if (ilp.cost != 0)
                        {
                            xw.WriteElementString("cost", ilp.cost.ToString());
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

    }
}
