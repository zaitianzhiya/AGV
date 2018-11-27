using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommonTools
{
    public class PropertyUtil
    {
        internal class ColorWrapper
        {
            private int a = 0;

            private int r = 0;

            private int g = 0;

            private int b = 0;

            public int A
            {
                get
                {
                    return this.a;
                }
                set
                {
                    this.a = value;
                }
            }

            public int R
            {
                get
                {
                    return this.r;
                }
                set
                {
                    this.r = value;
                }
            }

            public int G
            {
                get
                {
                    return this.g;
                }
                set
                {
                    this.g = value;
                }
            }

            public int B
            {
                get
                {
                    return this.b;
                }
                set
                {
                    this.b = value;
                }
            }

            public Color Color
            {
                get
                {
                    return Color.FromArgb(this.a, this.r, this.g, this.b);
                }
            }
        }

        private static PropertyInfo GetNestedProperty(object dataobject, string[] nestedfields, int curindex, ref object nestedDataObject)
        {
            nestedDataObject = dataobject;
            string propertyname = nestedfields[curindex];
            PropertyInfo property = PropertyUtil.GetProperty(dataobject, propertyname);
            bool flag = property == null;
            PropertyInfo result;
            if (flag)
            {
                result = null;
            }
            else
            {
                dataobject = property.GetValue(dataobject, null);
                int num = curindex;
                curindex = num + 1;
                bool flag2 = curindex == nestedfields.Length;
                if (flag2)
                {
                    result = property;
                }
                else
                {
                    result = PropertyUtil.GetNestedProperty(dataobject, nestedfields, curindex, ref nestedDataObject);
                }
            }
            return result;
        }

        public static PropertyInfo GetNestedProperty(object dataobject, ref object nestedDataObject, string fullFieldname)
        {
            string[] nestedfields = fullFieldname.Split(new char[]
			{
				'.'
			});
            return PropertyUtil.GetNestedProperty(dataobject, nestedfields, 0, ref nestedDataObject);
        }

        public static PropertyInfo GetNestedProperty(ref object dataobject, string fullFieldname)
        {
            return PropertyUtil.GetNestedProperty(dataobject, ref dataobject, fullFieldname);
        }

        public static PropertyInfo GetProperty(object dataobject, string propertyname)
        {
            bool flag = dataobject == null;
            PropertyInfo result;
            if (flag)
            {
                result = null;
            }
            else
            {
                result = PropertyUtil.GetProperty(dataobject.GetType(), propertyname);
            }
            return result;
        }

        public static PropertyInfo GetProperty(Type dataobjecttype, string propertyname)
        {
            MemberInfo[] member = dataobjecttype.GetMember(propertyname);
            MemberInfo[] array = member;
            PropertyInfo result;
            for (int i = 0; i < array.Length; i++)
            {
                MemberInfo memberInfo = array[i];
                PropertyInfo propertyInfo = memberInfo as PropertyInfo;
                bool flag = propertyInfo != null;
                if (flag)
                {
                    result = propertyInfo;
                    return result;
                }
            }
            result = null;
            return result;
        }

        private static bool IsPrimitive(object obj)
        {
            bool flag = obj == null;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                Type type = obj.GetType();
                bool flag2 = type.IsPrimitive || type.IsEnum || type == typeof(string);
                result = flag2;
            }
            return result;
        }

        public static object ChangeType(object value, Type type)
        {
            bool flag = value.GetType() == type;
            object result;
            if (flag)
            {
                result = value;
            }
            else
            {
                bool flag2 = value.GetType() == typeof(string) && type.IsEnum;
                if (flag2)
                {
                    try
                    {
                        result = Enum.Parse(type, value.ToString());
                        return result;
                    }
                    catch
                    {
                    }
                }
                bool flag3 = !type.IsPrimitive;
                if (flag3)
                {
                    object obj = PropertyUtil.Parse(value.ToString(), type);
                    bool flag4 = obj != null;
                    if (flag4)
                    {
                        result = obj;
                        return result;
                    }
                }
                try
                {
                    result = Convert.ChangeType(value, type);
                    return result;
                }
                catch
                {
                }
                result = null;
            }
            return result;
        }

        public static object Parse(string value, Type type)
        {
            bool flag = type == typeof(SizeF);
            object result;
            if (flag)
            {
                result = PropertyUtil.Parse(new SizeF(0f, 0f), value);
            }
            else
            {
                bool flag2 = type == typeof(PointF);
                if (flag2)
                {
                    result = PropertyUtil.Parse(new PointF(0f, 0f), value);
                }
                else
                {
                    bool flag3 = type == typeof(Color);
                    if (flag3)
                    {
                        value = value.Replace("Color ", "");
                        value = value.Trim(new char[]
						{
							'[',
							']'
						});
                        Color color = Color.FromName(value);
                        bool isKnownColor = color.IsKnownColor;
                        if (isKnownColor)
                        {
                            result = color;
                        }
                        else
                        {
                            PropertyUtil.ColorWrapper colorWrapper = new PropertyUtil.ColorWrapper();
                            PropertyUtil.Parse(colorWrapper, value);
                            result = colorWrapper.Color;
                        }
                    }
                    else
                    {
                        result = null;
                    }
                }
            }
            return result;
        }

        public static void ParseProperty(string fieldname, string svalue, object dataobject)
        {
            bool flag = fieldname.Length == 0 || svalue.Length == 0;
            if (!flag)
            {
                PropertyInfo property = PropertyUtil.GetProperty(dataobject, fieldname);
                bool flag2 = property == null || !property.CanWrite;
                if (!flag2)
                {
                    try
                    {
                        object obj = PropertyUtil.ChangeType(svalue, property.PropertyType);
                        bool flag3 = obj != null;
                        if (flag3)
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

        public static object Parse(object obj, string valuestring)
        {
            valuestring = valuestring.Trim(new char[]
			{
				'{',
				'}'
			});
            string[] array = valuestring.Split(new char[]
			{
				','
			});
            string[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                string text = array2[i];
                string[] array3 = text.Split(new char[]
				{
					'='
				});
                bool flag = array3.Length != 2;
                if (!flag)
                {
                    string fieldname = array3[0].Trim();
                    string svalue = array3[1].Trim();
                    PropertyUtil.ParseProperty(fieldname, svalue, obj);
                }
            }
            return obj;
        }
    }
}
