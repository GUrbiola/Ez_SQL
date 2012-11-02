using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Ez_SQL.Snippets
{
    public class Snippet
    {
        public string Name { get; set; }
        public string ShortCut { get; set; }
        public string Description { get; set; }
        public string Script { get; set; }
        public bool AskForTable
        {
            get
            {
                return Script.ToLower().Contains("$table$");
            }
        }
        public bool AskForView
        {
            get
            {
                return Script.ToLower().Contains("$view$");
            }
        }
        public bool AskForProcedure
        {
            get
            {
                return Script.ToLower().Contains("$procedure$");
            }
        }
        public bool AskForFields
        {
            get
            {
                return Script.ToLower().Contains("$fields$");
            }
        }
        public Snippet()
        {
        }
        public Snippet(string name, string shortCut, string description, string script)
        {
            Name = name;
            ShortCut = shortCut;
            Description = description;
            Script = script;
        }
        public string ToXml()
        {
            XmlWriter Xw;
            StringBuilder Sb = new StringBuilder();

            Xw = XmlWriter.Create(Sb, new XmlWriterSettings() { Encoding = Encoding.UTF8, Indent = true, IndentChars = "    " }); 
            Xw.WriteStartDocument(false);
            Xw.WriteStartElement("Snippet");

            Xw.WriteStartElement("Name");
            Xw.WriteString(Name);
            Xw.WriteEndElement();

            Xw.WriteStartElement("ShortCut");
            Xw.WriteString(ShortCut);
            Xw.WriteEndElement();

            Xw.WriteStartElement("Description");
            Xw.WriteString(Description);
            Xw.WriteEndElement();


            Xw.WriteStartElement("Script");
            Xw.WriteString(Script);
            Xw.WriteEndElement();

            Xw.WriteEndElement();
            Xw.WriteEndDocument();
            Xw.Flush();
            Xw.Close();

            return Sb.ToString();
        }
        public void LoadFromXml(string Xml)
        {
            XmlDocument SnippetData = new XmlDocument();
            XmlElement SnippetInfo;
            SnippetData.LoadXml(Xml);
            SnippetInfo = (XmlElement)SnippetData.GetElementsByTagName("Snippet")[0];

            Name = SnippetInfo.GetElementsByTagName("Name")[0].InnerText;
            ShortCut = SnippetInfo.GetElementsByTagName("ShortCut")[0].InnerText;
            Description = SnippetInfo.GetElementsByTagName("Description")[0].InnerText;
            Script = SnippetInfo.GetElementsByTagName("Script")[0].InnerText;
        }
        public static Snippet CreateFromXml(string Xml)
        {
            Snippet X = new Snippet();
            X.LoadFromXml(Xml);
            return X;
        }
    }
}
