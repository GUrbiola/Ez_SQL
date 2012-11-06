using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace CommonLibrary
{
    public abstract class CustomXmlSerializerBase
    {
        static Dictionary<string, IDictionary<string, FieldInfo>> fieldInfoCache = new Dictionary<string, IDictionary<string, FieldInfo>>();        

        protected XmlDocument doc = new XmlDocument();

        protected static IDictionary<string, FieldInfo> GetTypeFieldInfo(Type objType)
        {
            string typeName = objType.FullName;
            IDictionary<string, FieldInfo> fields;
            if (!fieldInfoCache.TryGetValue(typeName, out fields))
            {
                // fetch fields
                FieldInfo[] fieldInfo = objType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic |
                                                          BindingFlags.Public | BindingFlags.DeclaredOnly);

                Dictionary<string, FieldInfo> dict = new Dictionary<string, FieldInfo>(fieldInfo.Length);
                foreach (FieldInfo field in fieldInfo)
                {
                    if (!field.FieldType.IsSubclassOf(typeof(MulticastDelegate)))
                    {
                        object[] attribs = field.GetCustomAttributes(typeof(XmlIgnoreAttribute), false);
                        if (attribs.Length == 0)
                        {
                            dict.Add(field.Name, field);
                        }
                    }
                }

                // check base class as well
                Type baseType = objType.BaseType;
                if (baseType != null && baseType != typeof(object))
                {
                    // should we include this base class?
                    object[] attribs = baseType.GetCustomAttributes(typeof(XmlIgnoreBaseTypeAttribute), false);
                    if (attribs.Length == 0)
                    {
                        IDictionary<string, FieldInfo> baseFields = GetTypeFieldInfo(baseType);
                        // add fields
                        foreach (KeyValuePair<string, FieldInfo> kv in baseFields)
                        {
                            string key = kv.Key;
                            if (dict.ContainsKey(key))
                            {
                                // make field name unique
                                key = "base." + key;
                            }
                            dict.Add(key, kv.Value);
                        }
                    }
                }

                fields = dict;
                fieldInfoCache.Add(typeName, fields);
            }
            return fields;
        }

        protected class TypeInfo
        {
            internal int TypeId;
            internal XmlElement OnlyElement;

            internal void WriteTypeId(XmlElement element)
            {
                element.SetAttribute("typeid", TypeId.ToString());
            }
        }        
    }
}
