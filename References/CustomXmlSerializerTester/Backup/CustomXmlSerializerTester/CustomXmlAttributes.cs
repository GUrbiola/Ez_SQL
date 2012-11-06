using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLibrary
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class XmlIgnoreBaseTypeAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class CustomXmlSerializationOptionsAttribute : Attribute
    {
        public CustomXmlSerializer.SerializationOptions SerializationOptions = new CustomXmlSerializer.SerializationOptions();        

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
}
