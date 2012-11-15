using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;
using Ez_SQL.ConnectionManagement;
using Ez_SQL.QueryLog;
using System.IO;

namespace Ez_SQL
{
    public static class Globals
    {
        public static List<ConnectionGroup> GetConnections(string filename)
        {
            List<ConnectionGroup> CGs = new List<ConnectionGroup>();
            XmlNodeList Groups, Connections, GroupList;
            //XmlNode Grupo, Conexion;

            if (!System.IO.File.Exists(filename))
                return CGs;
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(filename);

            Groups = xDoc.GetElementsByTagName("Groups");
            GroupList = ((XmlElement)Groups[0]).GetElementsByTagName("Group");
            foreach (XmlNode group in GroupList)
            {
                ConnectionGroup auxgroup = new ConnectionGroup();
                auxgroup.Name = group.Attributes["Name"].Value.ToString();
                Connections = group.ChildNodes;
                Connections = ((XmlElement)Connections[0]).GetElementsByTagName("Connection");
                foreach (XmlNode conexion in Connections)
                {
                    ConnectionManagement.ConnectionInfo auxcon = new ConnectionManagement.ConnectionInfo();
                    auxcon.Name = conexion.Attributes["Name"].Value.ToString();
                    auxcon.ConnectionString = conexion.Attributes["ConnectionString"].Value.ToString();
                    auxgroup.Connections.Add(auxcon);
                }
                CGs.Add(auxgroup);
            }

            return CGs;
        }
        public static void SaveConnections(string filename, List<ConnectionGroup> CGs)
        {
            XmlTextWriter Writer;
            if (filename == "" || CGs == null || CGs.Count == 0)
                return;

            Writer = new XmlTextWriter(filename, System.Text.Encoding.UTF8);
            Writer.Formatting = Formatting.Indented;
            Writer.WriteStartDocument(false);
            Writer.WriteStartElement("Groups");
            foreach (ConnectionGroup gc in CGs)
            {
                Writer.WriteStartElement("Group");
                Writer.WriteAttributeString("Name", gc.Name);
                Writer.WriteStartElement("Connections");
                if (gc.Connections != null && gc.Connections.Count > 0)
                {
                    foreach (ConnectionManagement.ConnectionInfo con in gc.Connections)
                    {
                        Writer.WriteStartElement("Connection");
                        Writer.WriteAttributeString("Name", con.Name);
                        Writer.WriteAttributeString("ConnectionString", con.ConnectionString);
                        Writer.WriteEndElement();
                    }
                }
                Writer.WriteEndElement();
                Writer.WriteEndElement();
            }
            Writer.WriteEndElement();
            Writer.WriteEndDocument();
            Writer.Flush();
            Writer.Close();
        }
        /// <summary>
        /// Converts an image into an icon.
        /// </summary>
        /// <param name="img">The image that shall become an icon</param>
        /// <param name="size">The width and height of the icon. Standard
        /// sizes are 16x16, 32x32, 48x48, 64x64.</param>
        /// <param name="keepAspectRatio">Whether the image should be squashed into a
        /// square or whether whitespace should be put around it.</param>
        /// <returns>An icon!!</returns>
        public static Icon MakeIcon(Image img, int size, bool keepAspectRatio)
        {
            Bitmap square = new Bitmap(size, size); // create new bitmap
            Graphics g = Graphics.FromImage(square); // allow drawing to it

            int x, y, w, h; // dimensions for new image

            if (!keepAspectRatio || img.Height == img.Width)
            {
                // just fill the square
                x = y = 0; // set x and y to 0
                w = h = size; // set width and height to size
            }
            else
            {
                // work out the aspect ratio
                float r = (float)img.Width / (float)img.Height;

                // set dimensions accordingly to fit inside size^2 square
                if (r > 1)
                { // w is bigger, so divide h by r
                    w = size;
                    h = (int)((float)size / r);
                    x = 0; y = (size - h) / 2; // center the image
                }
                else
                { // h is bigger, so multiply w by r
                    w = (int)((float)size * r);
                    h = size;
                    y = 0;
                    x = (size - w) / 2; // center the image
                }
            }

            // make the image shrink nicely by using HighQualityBicubic mode
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(img, x, y, w, h); // draw image with specified dimensions
            g.Flush(); // make sure all drawing operations complete before we get the icon

            // following line would work directly on any image, but then
            // it wouldn't look as nice.
            return Icon.FromHandle(square.GetHicon());
        }
        internal static void SaveToQueryLog(QueryRecord CurQR)
        {
            string FileName = String.Format("{0}\\QueriesLog\\LogFile.xml", MainForm.DataStorageDir);
            bool Appending = File.Exists(FileName);

            using (StreamWriter Writer = new StreamWriter(FileName, true, System.Text.Encoding.UTF8))
            {
                if (!Appending)
                {
                    Writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"no\"?>");
                    Writer.WriteLine("<QueriesRoot>");
                }
                Writer.Write(CurQR.ToString());
                Writer.Close();
            }
        }
    }
}
