using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading;
using System.Xml.Serialization;
using System.Collections;
using System.Reflection;

namespace XmlSerializationExtensions
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class XmlIgnoreBaseTypeAttribute : Attribute
    {
    }
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class CustomXmlSerializationOptionsAttribute : Attribute
    {
        public XmlObjectSerializer.SerializationOptions SerializationOptions = new XmlObjectSerializer.SerializationOptions();

        public CustomXmlSerializationOptionsAttribute(bool useTypeCache, bool useGraphSerialization)
        {
            SerializationOptions.UseTypeCache = useTypeCache;
            SerializationOptions.UseGraphSerialization = useGraphSerialization;
        }
    }
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class XmlSerializeAsCustomTypeAttribute : Attribute
    {
    }
    public abstract class BaseSerializer
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
                            if (field.Name.EndsWith(">k__BackingField"))
                                dict.Add(field.Name.Substring(1, field.Name.IndexOf('>') - 1), field);
                            else
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
    public class XmlObjectSerializer : BaseSerializer
    {

        Dictionary<Type, TypeInfo> typeCache = new Dictionary<Type, TypeInfo>();
        Dictionary<Type, IDictionary<ObjKeyForCache, ObjInfo>> objCache = new Dictionary<Type, IDictionary<ObjKeyForCache, ObjInfo>>();
        int objCacheNextId = 0;
        SerializationOptions options;

        protected XmlObjectSerializer(SerializationOptions opt)
        {
            options = opt;
        }

        void SetTypeInfo(Type objType, XmlElement element)
        {
            if (!options.UseTypeCache)
            {
                // add detailed type information
                WriteTypeToNode(element, objType);
                return;
            }
            TypeInfo typeInfo;
            if (typeCache.TryGetValue(objType, out typeInfo))
            {
                XmlElement onlyElement = typeInfo.OnlyElement;
                if (onlyElement != null)
                {
                    // set the type of the element to be a reference to the type ID
                    // since the element is no longer the only one of this type                    
                    typeInfo.WriteTypeId(onlyElement);
                    onlyElement.RemoveAttribute("type");
                    onlyElement.RemoveAttribute("assembly");
                    typeInfo.OnlyElement = null;
                }
                typeInfo.WriteTypeId(element);
            }
            else
            {
                // add type to cache
                typeInfo = new TypeInfo();
                typeInfo.TypeId = typeCache.Count;
                typeInfo.OnlyElement = element;
                typeCache.Add(objType, typeInfo);
                // add detailed type information
                WriteTypeToNode(element, objType);
            }
        }

        static void WriteTypeToNode(XmlElement element, Type objType)
        {
            element.SetAttribute("type", objType.FullName);
            element.SetAttribute("assembly", objType.Assembly.FullName);
        }

        XmlElement GetTypeInfoNode()
        {
            XmlElement element = doc.CreateElement("TypeCache");
            foreach (KeyValuePair<Type, TypeInfo> kv in typeCache)
            {
                if (kv.Value.OnlyElement == null)
                {
                    // there is more than one element having this type
                    XmlElement e = doc.CreateElement("TypeInfo");
                    kv.Value.WriteTypeId(e);
                    WriteTypeToNode(e, kv.Key);
                    element.AppendChild(e);
                }
            }
            return element.HasChildNodes ? element : null;
        }

        public static XmlDocument Serialize(object obj, int ver, string rootName)
        {
            // determine serialization options
            SerializationOptions serOptions = new SerializationOptions();
            if (obj != null)
            {
                Type objType = obj.GetType();
                object[] attribs = objType.GetCustomAttributes(typeof(CustomXmlSerializationOptionsAttribute), false);
                if (attribs.Length > 0)
                {
                    serOptions = ((CustomXmlSerializationOptionsAttribute)attribs[0]).SerializationOptions;
                }
            }
            // create serializer
            XmlObjectSerializer serializer = new XmlObjectSerializer(serOptions);
            XmlElement element = serializer.SerializeCore(rootName, obj);
            element.SetAttribute("version", ver.ToString());
            element.SetAttribute("culture", Thread.CurrentThread.CurrentCulture.ToString());
            // add typeinfo
            XmlElement typeInfo = serializer.GetTypeInfoNode();
            if (typeInfo != null)
            {
                element.PrependChild(typeInfo);
                element.SetAttribute("hasTypeCache", "true");
            }
            // add serialized data
            serializer.doc.AppendChild(element);
            return serializer.doc;
        }

        bool AddObjToCache(Type objType, object obj, XmlElement element)
        {
            ObjKeyForCache kfc = new ObjKeyForCache(obj);
            IDictionary<ObjKeyForCache, ObjInfo> entry;
            if (objCache.TryGetValue(objType, out entry))
            {
                // look for this particular object                
                ObjInfo objInfoFound;
                if (entry.TryGetValue(kfc, out objInfoFound))
                {
                    // the object has already been added
                    if (objInfoFound.OnlyElement != null)
                    {
                        objInfoFound.WriteObjId(objInfoFound.OnlyElement);
                        objInfoFound.OnlyElement = null;
                    }
                    // write id to element
                    objInfoFound.WriteObjId(element);
                    return false;
                }
            }
            else
            {
                // brand new type in the cache
                entry = new Dictionary<ObjKeyForCache, ObjInfo>(1);
                objCache.Add(objType, entry);
            }
            // object not found, add it
            ObjInfo objInfo = new ObjInfo();
            objInfo.Id = objCacheNextId;
            objInfo.OnlyElement = element;
            entry.Add(kfc, objInfo);
            objCacheNextId++;
            return true;
        }

        static bool CheckForcedSerialization(Type objType)
        {
            object[] attribs = objType.GetCustomAttributes(typeof(XmlSerializeAsCustomTypeAttribute), false);
            return attribs.Length > 0;
        }

        XmlElement SerializeCore(string name, object obj)
        {
            XmlElement element = doc.CreateElement(name);
            if (obj == null)
            {
                element.SetAttribute("value", "null");
                return element;
            }

            Type objType = obj.GetType();

            if (objType.IsClass && objType != typeof(string))
            {
                // check if we have already serialized this object
                if (options.UseGraphSerialization && !AddObjToCache(objType, obj, element))
                {
                    return element;
                }
                // the object has just been added                
                SetTypeInfo(objType, element);

                if (CheckForcedSerialization(objType))
                {
                    // serialize as complex type
                    SerializeComplexType(obj, element);
                    return element;
                }

                IXmlSerializable xmlSer = obj as IXmlSerializable;
                if (xmlSer == null)
                {
                    // does not know about automatic serialization
                    IEnumerable arr = obj as IEnumerable;
                    if (arr == null)
                    {
                        SerializeComplexType(obj, element);
                    }
                    else
                    {
                        foreach (object arrObj in arr)
                        {
                            XmlElement e = SerializeCore(name, arrObj);
                            element.AppendChild(e);
                        }
                    }
                }
                else
                {
                    // can perform the serialization itself
                    StringBuilder sb = new StringBuilder();
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.ConformanceLevel = ConformanceLevel.Fragment;
                    settings.Encoding = Encoding.UTF8;
                    settings.OmitXmlDeclaration = true;
                    XmlWriter wr = XmlWriter.Create(sb, settings);
                    wr.WriteStartElement("value");
                    xmlSer.WriteXml(wr);
                    wr.WriteEndElement();
                    wr.Close();

                    element.InnerXml = sb.ToString();
                }
            }
            else
            {
                // the object has just been added                
                SetTypeInfo(objType, element);

                if (CheckForcedSerialization(objType))
                {
                    // serialize as complex type
                    SerializeComplexType(obj, element);
                    return element;
                }

                if (objType.IsEnum)
                {
                    object val = Enum.Format(objType, obj, "d");
                    element.SetAttribute("value", val.ToString());
                }
                else
                {
                    if (objType.IsPrimitive || objType == typeof(string) ||
                        objType == typeof(DateTime) || objType == typeof(decimal))
                    {
                        element.SetAttribute("value", obj.ToString());
                    }
                    else
                    {
                        // this is most probably a struct
                        SerializeComplexType(obj, element);
                    }
                }
            }

            return element;
        }

        void SerializeComplexType(object obj, XmlElement element)
        {
            Type objType = obj.GetType();
            // get all instance fields
            IDictionary<string, FieldInfo> fields = GetTypeFieldInfo(objType);
            foreach (KeyValuePair<string, FieldInfo> kv in fields)
            {
                // serialize field
                XmlElement e = SerializeCore(kv.Key, kv.Value.GetValue(obj));
                element.AppendChild(e);
            }
        }

        class ObjInfo
        {
            internal int Id;
            internal XmlElement OnlyElement;

            internal void WriteObjId(XmlElement element)
            {
                element.SetAttribute("id", Id.ToString());
            }
        }

        struct ObjKeyForCache : IEquatable<ObjKeyForCache>
        {
            object m_obj;

            public ObjKeyForCache(object obj)
            {
                m_obj = obj;
            }

            public bool Equals(ObjKeyForCache other)
            {
                return object.ReferenceEquals(m_obj, other.m_obj);
            }
        }

        public class SerializationOptions
        {
            public bool UseTypeCache = true;
            public bool UseGraphSerialization = true;
        }
    }
    public class XmlObjectDeserializer : BaseSerializer
    {
        CultureInfo cult;
        Dictionary<int, Type> deserializationTypeCache = null;
        Dictionary<int, object> deserializationObjCache = new Dictionary<int, object>();
        ITypeConverter typeConverter;

        protected XmlObjectDeserializer(ITypeConverter typeConverter)
        {
            this.typeConverter = typeConverter;
        }

        public static object Deserialize(string xml, int maxSupportedVer)
        {
            return Deserialize(xml, maxSupportedVer, null);
        }

        public static object Deserialize(string xml, int maxSupportedVer = 1, ITypeConverter typeConverter = null)
        {
            XmlObjectDeserializer deserializer = new XmlObjectDeserializer(typeConverter);
            deserializer.doc.LoadXml(xml);
            string version = deserializer.doc.DocumentElement.GetAttribute("version");
            if (maxSupportedVer < Convert.ToInt32(version))
            {
                return null;
            }
            string culture = deserializer.doc.DocumentElement.GetAttribute("culture");
            deserializer.cult = new CultureInfo(culture);
            return deserializer.DeserializeCore(deserializer.doc.DocumentElement);
        }

        void DeserializeComplexType(object obj, Type objType, XmlNode firstChild)
        {
            // complex type
            // get the class's fields                                
            IDictionary<string, FieldInfo> dictFields = GetTypeFieldInfo(objType);
            // set values for fields that are found
            for (XmlNode node = firstChild; node != null; node = node.NextSibling)
            {
                string fieldName = node.Name;
                FieldInfo field = null;
                if (dictFields.TryGetValue(fieldName, out field))
                {
                    // field is present, get value
                    object val = DeserializeCore((XmlElement)node);
                    // set value in object
                    field.SetValue(obj, val);
                }
            }
        }

        void LoadTypeCache(XmlElement element)
        {
            XmlNodeList children = element.GetElementsByTagName("TypeInfo");
            deserializationTypeCache = new Dictionary<int, Type>(children.Count);
            foreach (XmlElement child in children)
            {
                int typeId = Convert.ToInt32(child.GetAttribute("typeid"));
                Type objType = InferTypeFromElement(child);
                deserializationTypeCache.Add(typeId, objType);
            }
        }

        object DeserializeCore(XmlElement element)
        {
            // check if this is a reference to another object
            int objId;
            if (int.TryParse(element.GetAttribute("id"), out objId))
            {
                object objCached = GetObjFromCache(objId);
                if (objCached != null)
                {
                    return objCached;
                }
            }
            else
            {
                objId = -1;
            }

            // check for null
            string value = element.GetAttribute("value");
            if (value == "null")
            {
                return null;
            }

            int subItems = element.ChildNodes.Count;
            XmlNode firstChild = element.FirstChild;

            // load type cache if available            
            if (element.GetAttribute("hasTypeCache") == "true")
            {
                LoadTypeCache((XmlElement)firstChild);
                subItems--;
                firstChild = firstChild.NextSibling;
            }
            // get type            
            Type objType;
            string typeId = element.GetAttribute("typeid");
            if (string.IsNullOrEmpty(typeId))
            {
                // no type id so type information must be present
                objType = InferTypeFromElement(element);
            }
            else
            {
                // there is a type id present
                objType = deserializationTypeCache[Convert.ToInt32(typeId)];
            }

            // process enum
            if (objType.IsEnum)
            {
                long val = Convert.ToInt64(value, cult);
                return Enum.ToObject(objType, val);
            }

            // process some simple types
            switch (Type.GetTypeCode(objType))
            {
                case TypeCode.Boolean: return Convert.ToBoolean(value, cult);
                case TypeCode.Byte: return Convert.ToByte(value, cult);
                case TypeCode.Char: return Convert.ToChar(value, cult);
                case TypeCode.DBNull: return DBNull.Value;
                case TypeCode.DateTime: return Convert.ToDateTime(value, cult);
                case TypeCode.Decimal: return Convert.ToDecimal(value, cult);
                case TypeCode.Double: return Convert.ToDouble(value, cult);
                case TypeCode.Int16: return Convert.ToInt16(value, cult);
                case TypeCode.Int32: return Convert.ToInt32(value, cult);
                case TypeCode.Int64: return Convert.ToInt64(value, cult);
                case TypeCode.SByte: return Convert.ToSByte(value, cult);
                case TypeCode.Single: return Convert.ToSingle(value, cult);
                case TypeCode.String: return value;
                case TypeCode.UInt16: return Convert.ToUInt16(value, cult);
                case TypeCode.UInt32: return Convert.ToUInt32(value, cult);
                case TypeCode.UInt64: return Convert.ToUInt64(value, cult);
            }

            // our value
            object obj;

            if (objType.IsArray)
            {
                Type elementType = objType.GetElementType();
                MethodInfo setMethod = objType.GetMethod("Set", new Type[] { typeof(int), elementType });

                ConstructorInfo constructor = objType.GetConstructor(new Type[] { typeof(int) });
                obj = constructor.Invoke(new object[] { subItems });
                // add object to cache if necessary
                if (objId >= 0)
                {
                    deserializationObjCache.Add(objId, obj);
                }

                int i = 0;
                foreach (object val in ValuesFromNode(firstChild))
                {
                    setMethod.Invoke(obj, new object[] { i, val });
                    i++;
                }
                return obj;
            }

            // create a new instance of the object
            obj = Activator.CreateInstance(objType, true);
            // add object to cache if necessary
            if (objId >= 0)
            {
                deserializationObjCache.Add(objId, obj);
            }

            IXmlSerializable xmlSer = obj as IXmlSerializable;
            if (xmlSer == null)
            {
                IList lst = obj as IList;
                if (lst == null)
                {
                    IDictionary dict = obj as IDictionary;
                    if (dict == null)
                    {
                        if (objType == typeof(DictionaryEntry) ||
                            (objType.IsGenericType &&
                             objType.GetGenericTypeDefinition() == typeof(KeyValuePair<,>)))
                        {
                            // load all field contents in a dictionary
                            Dictionary<string, object> properties = new Dictionary<string, object>(element.ChildNodes.Count);
                            for (XmlNode node = firstChild; node != null; node = node.NextSibling)
                            {
                                object val = DeserializeCore((XmlElement)node);
                                properties.Add(node.Name, val);
                            }
                            // return the dictionary
                            return properties;
                        }
                        // complex type
                        DeserializeComplexType(obj, objType, firstChild);
                    }
                    else
                    {
                        // it's a dictionary
                        foreach (object val in ValuesFromNode(firstChild))
                        {
                            // should be a Dictionary                                    
                            Dictionary<string, object> dictVal = (Dictionary<string, object>)val;
                            if (dictVal.ContainsKey("key"))
                            {
                                // should be a KeyValuePair
                                dict.Add(dictVal["key"], dictVal["value"]);
                            }
                            else
                            {
                                // should be a DictionaryEntry
                                dict.Add(dictVal["_key"], dictVal["_value"]);
                            }
                        }
                    }
                }
                else
                {
                    // it's a list
                    foreach (object val in ValuesFromNode(firstChild))
                    {
                        lst.Add(val);
                    }
                }
            }
            else
            {
                // the object can deserialize itself
                StringReader sr = new StringReader(element.InnerXml);
                XmlReader rd = XmlReader.Create(sr);
                xmlSer.ReadXml(rd);
                rd.Close();
                sr.Close();
            }
            return obj;
        }

        IEnumerable ValuesFromNode(XmlNode firstChild)
        {
            for (XmlNode node = firstChild; node != null; node = node.NextSibling)
            {
                yield return DeserializeCore((XmlElement)node);
            }
        }

        object GetObjFromCache(int objId)
        {
            object obj;
            if (deserializationObjCache.TryGetValue(objId, out obj))
            {
                return obj;
            }
            return null;
        }

        Type InferTypeFromElement(XmlElement element)
        {
            Type objType;
            string typeFullName = element.GetAttribute("type");
            string assemblyFullName = element.GetAttribute("assembly");

            if (typeConverter != null)
            {
                typeConverter.ProcessType(ref assemblyFullName, ref typeFullName);
            }

            if (string.IsNullOrEmpty(assemblyFullName))
            {
                // type is directly loadable
                objType = Type.GetType(typeFullName, true);
            }
            else
            {
                Assembly asm = Assembly.Load(assemblyFullName);
                objType = asm.GetType(typeFullName, true);
            }
            return objType;
        }

        public interface ITypeConverter
        {
            void ProcessType(ref string assemblyFullName, ref string typeFullName);
        }
    }
    public static class Extensions
    {
        public static void SerializeToXmlFile(this object obj, string fileName, string root = "Data", int ver = 1)
        {
            XmlDocument xDoc = XmlObjectSerializer.Serialize(obj, ver, root);
            xDoc.Save(fileName);
        }
        public static string SerializeToXmlString(this object obj, string root = "Data", int ver = 1)
        {
            TextWriter tw = new StringWriter();
            XmlDocument xDoc = XmlObjectSerializer.Serialize(obj, ver, root);
            xDoc.Save(tw);
            return tw.ToString();
        }
        public static object DeserializeFromXmlFile(this string fileName, int ver = 1, XmlSerializationExtensions.XmlObjectDeserializer.ITypeConverter typeConverter = null)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            return XmlObjectDeserializer.Deserialize(doc.OuterXml, ver, typeConverter);
        }
        public static object DeserializeFromXmlString(this string xmlString, int ver = 1, XmlSerializationExtensions.XmlObjectDeserializer.ITypeConverter typeConverter = null)
        {
            return XmlObjectDeserializer.Deserialize(xmlString, ver, typeConverter);
        }
    }



}
