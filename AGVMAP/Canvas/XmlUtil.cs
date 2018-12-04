using CommonTools;
using System;
using System.Reflection;
using System.Xml;

namespace Canvas
{
	internal class XmlUtil
	{
		public static void AddProperty(string name, object value, XmlWriter wr)
		{
			string text = string.Empty;
			bool flag = value is string;
			if (flag)
			{
				text = (value as string);
			}
			bool flag2 = text.Length == 0 && value.GetType() == typeof(float);
			if (flag2)
			{
				text = XmlConvert.ToString(Math.Round((double)((float)value), 8));
			}
			bool flag3 = text.Length == 0 && value.GetType() == typeof(double);
			if (flag3)
			{
				text = XmlConvert.ToString(Math.Round((double)value, 8));
			}
			bool flag4 = text.Length == 0;
			if (flag4)
			{
				text = value.ToString();
			}
			wr.WriteStartElement("property");
			wr.WriteAttributeString("name", name);
			wr.WriteAttributeString("value", text);
			wr.WriteEndElement();
		}

		public static void ParseProperty(XmlElement node, object dataobject)
		{
			bool flag = node.Name != "property";
			if (!flag)
			{
				string attribute = node.GetAttribute("name");
				string attribute2 = node.GetAttribute("value");
				bool flag2 = attribute.Length == 0 || attribute2.Length == 0;
				if (!flag2)
				{
                    PropertyInfo property = CommonTools.PropertyUtil.GetProperty(dataobject, attribute);
					bool flag3 = property == null || !property.CanWrite;
					if (!flag3)
					{
						try
						{
							object obj = PropertyUtil.ChangeType(attribute2, property.PropertyType);
							bool flag4 = obj != null;
							if (flag4)
							{
								property.SetValue(dataobject, obj, null);
							}
						}
						catch
						{
						}
					}
				}
			}
		}

		public static void ParseProperties(XmlElement itemnode, object dataobject)
		{
			foreach (XmlElement node in itemnode.ChildNodes)
			{
				XmlUtil.ParseProperty(node, dataobject);
			}
		}

		public static void WriteProperties(object dataobject, XmlWriter wr)
		{
			PropertyInfo[] properties = dataobject.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			for (int i = 0; i < properties.Length; i++)
			{
				PropertyInfo propertyInfo = properties[i];
				XmlSerializable xmlSerializable = (XmlSerializable)Attribute.GetCustomAttribute(propertyInfo, typeof(XmlSerializable));
				bool flag = xmlSerializable != null;
				if (flag)
				{
					string name = propertyInfo.Name;
					object value = propertyInfo.GetValue(dataobject, null);
					bool flag2 = value != null;
					if (flag2)
					{
						XmlUtil.AddProperty(name, value, wr);
					}
				}
			}
		}
	}
}
