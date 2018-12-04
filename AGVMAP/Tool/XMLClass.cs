using System;
using System.Collections;
using System.IO;
using System.Xml;

namespace Tools
{
	public class XMLClass
	{
		public static bool AppendXML(string filepath, string rootnodeName, Hashtable nodelist)
		{
			bool flag = !File.Exists(filepath);
			bool result;
			if (flag)
			{
				bool flag2 = !XMLClass.CreateXML(filepath);
				if (flag2)
				{
					result = false;
					return result;
				}
			}
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(filepath);
				XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName(rootnodeName);
				bool flag3 = elementsByTagName.Count > 0;
				XmlNode xmlNode;
				if (flag3)
				{
					xmlNode = elementsByTagName[0];
					xmlNode.ParentNode.RemoveChild(xmlNode);
				}
				XmlNode xmlNode2 = xmlDocument.SelectSingleNode("config");
				xmlNode = xmlDocument.CreateElement(rootnodeName);
				xmlNode2.AppendChild(xmlNode);
				foreach (DictionaryEntry dictionaryEntry in nodelist)
				{
					XmlElement xmlElement = xmlDocument.CreateElement(dictionaryEntry.Key.ToString());
					xmlElement.InnerText = dictionaryEntry.Value.ToString();
					xmlNode.AppendChild(xmlElement);
				}
				xmlDocument.Save(filepath);
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		public static bool CreateXML(string filepath)
		{
			XmlDocument xmlDocument = new XmlDocument();
			XmlNode newChild = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
			xmlDocument.AppendChild(newChild);
			XmlNode newChild2 = xmlDocument.CreateElement("config");
			xmlDocument.AppendChild(newChild2);
			bool result;
			try
			{
				xmlDocument.Save(filepath);
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		public static Hashtable GetXMLByParentNode(string filepath, string pNodeName)
		{
			Hashtable hashtable = new Hashtable();
			bool flag = !File.Exists(filepath);
			Hashtable result;
			if (flag)
			{
				result = hashtable;
			}
			else
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(filepath);
				XmlElement documentElement = xmlDocument.DocumentElement;
				XmlNodeList childNodes = documentElement.ChildNodes;
				foreach (XmlNode xmlNode in childNodes)
				{
					bool flag2 = xmlNode.Name.ToLower() == pNodeName.ToLower();
					if (flag2)
					{
						foreach (XmlNode xmlNode2 in xmlNode.ChildNodes)
						{
							hashtable.Add(xmlNode2.Name, xmlNode2.InnerText);
						}
					}
				}
				result = hashtable;
			}
			return result;
		}
	}
}
