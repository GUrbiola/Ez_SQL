using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using CustomXmlSerializerTester;

namespace CommonLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            Test1 t = Test1.Get();//new Test1();
            t.A = 1;
            t.B = 2;
            t.Str = "this is a string";
            t.Dt1 = DateTime.Now;
            t.Dt2 = DateTime.Today.AddDays(10);
            t.Arr = new int[] { 1,2,3 };
            t.baseInt = 33;
            t.baseStr = "this is a basestring";
            t.SetValues(15, 3.4);
            t.privDbl = 5.6;
            t.base1 = new Base1();
            t.base1.baseInt = 99;
            t.base1.baseStr = "base1's basestring";
            t.base2 = t.base1;
            t.tEnum = TestEnum.enum2;

            XmlDocument doc = CustomXmlSerializer.Serialize(t, 1, "Test1");
            doc.Save(@"f:\out.xml");

            Test1 t2 = (Test1)CustomXmlDeserializer.Deserialize(doc.OuterXml, 1, new TestMeTypeConverter());            
        }
    }    

    //[XmlIgnoreBaseType]
    class Base1
    {
        public int baseInt;
        public string baseStr;
        protected int protInt = 1;
        private double privDbl = 2.3;        

        public void SetValues(int prot, double priv)
        {
            protInt = prot;
            privDbl = priv;
        }
    }

    //[CustomXmlSerializationOptions(true, false)]
    class Test1 : Base1
    {
        private Test1()
        { }

        public static Test1 Get()
        {
            return new Test1();
        }

        public TestEnum tEnum;
        public int A;
        public int B;
        public string Str;
        public DateTime Dt1;
        public DateTime Dt2;
        public int[] Arr;
        public double privDbl;
        public Base1 base1;
        public Base1 base2;
    }

    enum TestEnum
    {
        enum1 = 1,
        enum2 = 2
    }
}
