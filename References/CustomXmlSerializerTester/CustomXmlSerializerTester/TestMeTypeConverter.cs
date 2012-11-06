using System;
using System.Collections.Generic;
using System.Text;
using CommonLibrary;

namespace CustomXmlSerializerTester
{
    public class TestMeTypeConverter : CustomXmlDeserializer.ITypeConverter
    {
        public void ProcessType(ref string assemblyFullName, ref string typeFullName)
        {
            // translate types to accomodate code movement
            switch (assemblyFullName)
            {
                case "OldTestMe, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null":
                    // the assembly "OldTestMe" was renamed to "TestMe" and additionally its version changed
                    assemblyFullName = "TestMe, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null";
                    // the type "OldTestMe.VirtualList" was moved to another assembly
                    // and now it is called "CommonTools.Lists.VirtualList"
                    if (typeFullName.StartsWith("OldTestMe.VirtualList"))
                    {
                        // the new home is the CommonTools assembly
                        assemblyFullName = "CommonTools, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                        // the namespace changed from "OldTestMe" to "CommonTools.Lists"
                        typeFullName = typeFullName.Replace("OldTestMe", "CommonTools.Lists");
                    }
                    break;
            }            
        }
    }
}
