using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using CustomXmlSerializerTester;
using CallView360_WebApp.Areas.RapidRecall.ViewModels.FindNumbers;
using CallView360_WebApp.Areas.RapidRecall.ViewModels.Inbound;
using CallView360_WebApp.ViewModels;
using System.IO;
using System.Runtime.Serialization;

namespace CommonLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            FiltersVM Test;

            Test = new FiltersVM();
            Test.Availability = new LicensingFilterVM()
            {
                AreaCodeUSA = new List<TagVM>() { new TagVM("1", "AcUSA 1", "Au1"), new TagVM("2", "AcUSA 2", "Au2"), new TagVM("3", "AcUSA 3", "Au3"), new TagVM("4", "AcUSA 4", "Au4") },
                AreaCodeStateUSATerritories = new List<TagVM>() { new TagVM("1", "AcUST 1", "Aut1"), new TagVM("2", "AcUST 2", "Aut2"), new TagVM("3", "AcUST 3", "Aut3"), new TagVM("4", "AcUST 4", "Aut4"), new TagVM("5", "AcUST 5", "Aut5") },
                RegionCanada = true,
                RegionUSA = true,
                StateUSATerritories = new List<TagVM>() { new TagVM("1", "SUST 1", "Su1"), new TagVM("2", "SUST 2", "Su2"), new TagVM("3", "SUST 3", "Su3"), new TagVM("4", "SUST 4", "Su4"), new TagVM("5", "SUST 5", "Su5") },
                StateCanada = new List<TagVM>() { new TagVM("1", "SCanada 1", "Sc1"), new TagVM("2", "SCanada 2", "Sc2"), new TagVM("3", "SCanada 3", "Sc3"), new TagVM("4", "SCanada 4", "Sc4"), new TagVM("5", "SCanada 5", "Sc5") },            
            };
            Test.CallsLast30d = "25";
            Test.PrefixAndExpression = new PrefixAndExpressionVM() { Prefix = "TXT", Expression = "8000000GON" };
            Test.Price = new PriceVM() { MinAmount = "100", MaxAmount = "200" };
            Test.Readiness = "Ready,NotReady";
            Test.SpecificNumbers = new List<string>() {"8000000001", "8000000001", "8000000001", "8000000001","8000000001","8000000001","8000000001","8000000001","8000000001","8000000001","8000000001","8000000001","8000000001","8000000001","8000000001","8000000001","8000000001" };
            Test.Tags = new List<TagVM>() { new TagVM("1", "Tag1", "Val1"), new TagVM("2", "Tag2", "Val2"), new TagVM("3", "Tag3", "Val3"),new TagVM("4", "Tag4", "Val4"),new TagVM("5", "Tag5", "Val5"),new TagVM("6", "Tag6", "Val6"),new TagVM("7", "Tag7", "Val7"),new TagVM("8", "Tag8", "Val8"),new TagVM("9", "Tag9", "Val9"),new TagVM("10", "Tag10", "Val10"),new TagVM("10", "Tag10", "Val10"),new TagVM("11", "Tag11", "Val11"),new TagVM("12", "Tag12", "Val12") };

            XmlDocument doc = CustomXmlSerializer.Serialize(Test, 1, "Details");
            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(Test.Serialize());
            doc.Save(@"C:\Users\gonzalo abel\Documents\Testout.xml");

            FiltersVM Test2 = (FiltersVM)CustomXmlDeserializer.Deserialize(doc.OuterXml, 1, new TestMeTypeConverter());            
            //FiltersVM Test2 = Test.Serialize().Deserialize<FiltersVM>();
        }
    }
}
