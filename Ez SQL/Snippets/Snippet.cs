using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Ez_SQL.Snippets
{
    /// <summary>
    /// Represents a user-defined SQL code snippet with a name, keyboard shortcut, description,
    /// and a SQL script body. Snippets are stored as <c>.snp</c> files in
    /// <c>DataStorageDir\Snippets\</c> and managed via the <see cref="SnippetEditor"/>.
    /// <para>
    /// The script body may contain placeholder tokens (<c>$table$</c>, <c>$view$</c>,
    /// <c>$procedure$</c>, <c>$fields$</c>) that trigger object-selection dialogs when the
    /// snippet is expanded. The <c>AskFor*</c> computed properties detect the presence of
    /// these tokens so the expansion logic knows which dialogs to show.
    /// </para>
    /// </summary>
    public class Snippet
    {
        /// <summary>Gets or sets the display name of the snippet.</summary>
        public string Name { get; set; }

        /// <summary>Gets or sets the short keyboard trigger string used to expand the snippet.</summary>
        public string ShortCut { get; set; }

        /// <summary>Gets or sets a human-readable description of what the snippet does.</summary>
        public string Description { get; set; }

        /// <summary>Gets or sets the SQL script body, optionally containing placeholder tokens.</summary>
        public string Script { get; set; }

        /// <summary>
        /// Gets whether the script contains the <c>$table$</c> placeholder,
        /// indicating a table-selection dialog is required before expansion.
        /// </summary>
        public bool AskForTable
        {
            get { return Script.ToLower().Contains("$table$"); }
        }

        /// <summary>
        /// Gets whether the script contains the <c>$view$</c> placeholder,
        /// indicating a view-selection dialog is required before expansion.
        /// </summary>
        public bool AskForView
        {
            get { return Script.ToLower().Contains("$view$"); }
        }

        /// <summary>
        /// Gets whether the script contains the <c>$procedure$</c> placeholder,
        /// indicating a procedure-selection dialog is required before expansion.
        /// </summary>
        public bool AskForProcedure
        {
            get { return Script.ToLower().Contains("$procedure$"); }
        }

        /// <summary>
        /// Gets whether the script contains the <c>$fields$</c> placeholder,
        /// indicating a field-selection dialog is required before expansion.
        /// </summary>
        public bool AskForFields
        {
            get { return Script.ToLower().Contains("$fields$"); }
        }

        /// <summary>Initializes a new empty <see cref="Snippet"/>.</summary>
        public Snippet() { }

        /// <summary>Initializes a new <see cref="Snippet"/> with all fields provided.</summary>
        public Snippet(string name, string shortCut, string description, string script)
        {
            Name = name;
            ShortCut = shortCut;
            Description = description;
            Script = script;
        }
        /// <summary>
        /// Serializes this snippet to an XML string using <see cref="System.Xml.XmlWriter"/>.
        /// The produced XML contains <c>Name</c>, <c>ShortCut</c>, <c>Description</c>,
        /// and <c>Script</c> elements nested inside a <c>Snippet</c> root element.
        /// This format is used when saving to a <c>.snp</c> file.
        /// </summary>
        /// <returns>A well-formed XML string representing this snippet.</returns>
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
        /// <summary>
        /// Deserializes this snippet's properties from an XML string produced by <see cref="ToXml"/>.
        /// Populates <see cref="Name"/>, <see cref="ShortCut"/>, <see cref="Description"/>, and <see cref="Script"/>.
        /// </summary>
        /// <param name="Xml">The XML string to parse.</param>
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
        /// <summary>
        /// Creates and returns a new <see cref="Snippet"/> by parsing the given XML string.
        /// Equivalent to constructing a new snippet and calling <see cref="LoadFromXml"/>.
        /// </summary>
        /// <param name="Xml">The XML string to parse.</param>
        /// <returns>A new <see cref="Snippet"/> populated from the XML.</returns>
        public static Snippet CreateFromXml(string Xml)
        {
            Snippet X = new Snippet();
            X.LoadFromXml(Xml);
            return X;
        }
    }
}
