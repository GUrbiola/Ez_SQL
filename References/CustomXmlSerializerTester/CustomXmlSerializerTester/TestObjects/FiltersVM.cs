using System;
using System.Collections.Generic;
using System.Web;
using CallView360_WebApp.Areas.RapidRecall.ViewModels.Inbound;
using System.Xml;
using System.Text;
using CallView360_WebApp.ViewModels;
using System.Linq;

namespace CallView360_WebApp.Areas.RapidRecall.ViewModels.FindNumbers
{
    [Serializable]
    public class FiltersVM
    {
        public PriceVM Price { get; set; }
        public PrefixAndExpressionVM PrefixAndExpression { get; set; }
        public List<string> SpecificNumbers { get; set; }
        public string CallsLast30d { get; set; }
        public string Readiness { get; set; }
        public LicensingFilterVM Availability { get; set; }
        public IEnumerable<TagVM> Tags { get; set; }
        public string ToXML 
        {
            get
            {
                StringBuilder Back = new StringBuilder();
                int aux;
                string Expression, Prefix;

                Expression = PrefixAndExpression == null ? "" : PrefixAndExpression.Expression;
                Prefix = PrefixAndExpression == null ? "" : PrefixAndExpression.Prefix;

                using (XmlWriter Wr = XmlWriter.Create(Back))
                {
                    Wr.WriteStartElement("Filters");
                    
                    #region Filter Price
                    if (Price != null && (!String.IsNullOrEmpty(Price.MinAmount) || !String.IsNullOrEmpty(Price.MaxAmount)))
                    {
                        if (!String.IsNullOrEmpty(Price.MinAmount) && int.TryParse(Price.MinAmount, out aux))
                        {
                            Wr.WriteStartElement("MinPrice");
                            Wr.WriteStartElement("Element");
                            Wr.WriteValue(Price.MinAmount);
                            Wr.WriteEndElement();//End Element -> </Element>
                            Wr.WriteEndElement();//End MinPrice -> </MinPrice>
                        }
                        if (!String.IsNullOrEmpty(Price.MaxAmount) && int.TryParse(Price.MaxAmount, out aux))
                        {
                            Wr.WriteStartElement("MaxAmount");
                            Wr.WriteStartElement("Element");
                            Wr.WriteValue(Price.MaxAmount);
                            Wr.WriteEndElement();//End Element -> </Element>
                            Wr.WriteEndElement();//End MaxAmount -> </MaxAmount>
                        }
                    }
                    #endregion

                    #region Filter Prefixes
                    if (!String.IsNullOrEmpty(Prefix))
                    {
                        Wr.WriteStartElement("Prefixes");
                        string[] Data = Prefix.Split(',');
                        for (int i = 0; i < Data.Length; i++)
                        {
                            if (!String.IsNullOrEmpty(Data[i]))
                            {
                                Wr.WriteStartElement("Element");
                                Wr.WriteValue(Data[i]);
                                Wr.WriteEndElement();//End Element -> </Element>
                            }
                        }
                        Wr.WriteEndElement();//End Prefixes -> </Prefixes>
                    }
                    #endregion

                    #region Filter Expression
                    if (!String.IsNullOrEmpty(Expression))
                    {
                        Wr.WriteStartElement("Expression");
                        Wr.WriteStartElement("Element");
                        Wr.WriteValue(Expression);
                        Wr.WriteEndElement();//End Element -> </Element>
                        Wr.WriteEndElement();//End Expression -> </Expression>
                    }
                    #endregion

                    #region Filter Specific Numbers
                    if (SpecificNumbers != null && SpecificNumbers.Count > 0)
                    {
                        Wr.WriteStartElement("Numbers");
                        foreach (string Number in SpecificNumbers)
                        {
                            Wr.WriteStartElement("Element");
                            Wr.WriteValue(Number);
                            Wr.WriteEndElement();//End Element -> </Element>
                        }
                        Wr.WriteEndElement();//End Numbers -> </Numbers>
                    }
                    #endregion

                    #region Filter Missdials
                    if (!String.IsNullOrEmpty(CallsLast30d) && int.TryParse(CallsLast30d, out aux))
                    {
                        Wr.WriteStartElement("");
                        Wr.WriteStartElement("Element");
                        Wr.WriteValue(CallsLast30d);
                        Wr.WriteEndElement();//End Element -> </Element>
                        Wr.WriteEndElement();//End Missdials -> </Missdials>
                    }
                    #endregion

                    #region Filter Readiness
                    if (Readiness != null)
                    {
                        Wr.WriteStartElement("Readiness");
                        string[] Data = Readiness.ToString().Split(',');
                        for (int i = 0; i < Data.Length; i++)
                        {
                            if (!String.IsNullOrEmpty(Data[i]))
                            {
                                Wr.WriteStartElement("Element");
                                Wr.WriteValue(Data[i]);
                                Wr.WriteEndElement();//End Element -> </Element>
                            }
                        }
                        Wr.WriteEndElement();//End Readiness -> </Readiness>
                    }
                    #endregion

                    #region Filter Availability
                    if (Availability != null)
                    {
                        Wr.WriteStartElement("Availability");
                        switch (Availability.FilterBy)
                        {
                            case "0"://Global
                                Wr.WriteAttributeString("Description", "GlobalLicensing");
                                Wr.WriteAttributeString("LicensingTypeId", "0");
                                Wr.WriteStartElement("Global");
                                Wr.WriteValue("*");
                                Wr.WriteEndElement();//End Global -> </Global>
                                break;
                            case "1"://Region
                                Wr.WriteAttributeString("Description", "RegionLicensing");
                                Wr.WriteAttributeString("LicensingTypeId", "1");
                                
                                if (Availability.RegionUSA)
                                {
                                    Wr.WriteStartElement("Region");
                                    Wr.WriteValue("USA");
                                    Wr.WriteEndElement();//End Region -> </Region>
                                }
                                
                                if (Availability.RegionUSATerritories)
                                {
                                    Wr.WriteStartElement("Region");
                                    Wr.WriteValue("UST");
                                    Wr.WriteEndElement();//End Region -> </Region>
                                }
                                
                                if (Availability.RegionCanada)
                                {
                                    Wr.WriteStartElement("Region");
                                    Wr.WriteValue("Canada");
                                    Wr.WriteEndElement();//End Region -> </Region>
                                }
                                Wr.WriteEndElement();//End Region -> </Region>
                                break;
                            case "2"://State
                                if(Availability.States != null)
                                {
                                    foreach (TagVM State in Availability.States)
                                    {
                                        Wr.WriteStartElement("State");
                                        Wr.WriteValue(State.Id);
                                        Wr.WriteEndElement();//End State -> </State>
                                    }
                                }
                                break;
                            case "3"://AreaCode
                                if(Availability.AreaCodes != null)
                                {
                                    foreach (TagVM AreaCode in Availability.AreaCodes)
                                    {
                                        Wr.WriteStartElement("AreaCode");
                                        Wr.WriteValue(AreaCode.Id);
                                        Wr.WriteEndElement();//End AreaCode -> </AreaCode>
                                    }
                                }
                                break;
                        }
                        Wr.WriteEndElement();//End Availability -> </Availability>
                    }
                    #endregion

                    #region Filter Tags
                    if (Tags != null && Tags.Count() > 0)
                    {
                        Wr.WriteStartElement("Tags");
                        foreach (TagVM Tag in Tags)
                        {
                            Wr.WriteStartElement("Element");
                            Wr.WriteValue(Tag.Id);
                            Wr.WriteEndElement();//End Element -> </Element>
                        }
                        Wr.WriteEndElement();//End Tags -> </Tags>
                    }
                    #endregion


                    Wr.WriteEndElement();//End Filters -> </Filters>
                }
                return Back.ToString();
            }
        }

    }
}