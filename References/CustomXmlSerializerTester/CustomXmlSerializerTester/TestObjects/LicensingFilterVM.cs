using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CallView360_WebApp.ViewModels;

namespace CallView360_WebApp.Areas.RapidRecall.ViewModels.Inbound
{
    public class LicensingFilterVM
    {
        public string FilterBy { get; set; }

        public bool RegionUSA { get; set; }
        public bool RegionUSATerritories { get; set; }
        public bool RegionCanada { get; set; }
        public bool Region800Numbers { get; set; }

        public IEnumerable<TagVM> StateUSA { get; set; }
        public IEnumerable<TagVM> StateUSATerritories { get; set; }
        public IEnumerable<TagVM> StateCanada { get; set; }
        public IEnumerable<TagVM> States
        {
            get
            {
                List<TagVM> Back = new List<TagVM>();
                if (StateUSA != null && StateUSA.Count() > 0)
                    Back.AddRange(StateUSA);
                if (StateUSATerritories != null && StateUSATerritories.Count() > 0)
                    Back.AddRange(StateUSATerritories);
                if (StateCanada != null && StateCanada.Count() > 0)
                    Back.AddRange(StateCanada);
                return Back;
            }
        }

        public IEnumerable<TagVM> AreaCodeUSA { get; set; }
        public IEnumerable<TagVM> AreaCodeStateUSATerritories { get; set; }
        public IEnumerable<TagVM> AreaCodeStateCanada { get; set; }
        public IEnumerable<TagVM> AreaCodes
        {
            get
            {
                List<TagVM> Back = new List<TagVM>();
                if (AreaCodeUSA != null && AreaCodeUSA.Count() > 0)
                    Back.AddRange(AreaCodeUSA);
                if (AreaCodeStateUSATerritories != null && AreaCodeStateUSATerritories.Count() > 0)
                    Back.AddRange(AreaCodeStateUSATerritories);
                if (AreaCodeStateCanada != null && AreaCodeStateCanada.Count() > 0)
                    Back.AddRange(AreaCodeStateCanada);
                return Back;
            }
        }

        public int FilterByAsInt
        {
            get
            {
                switch ((FilterBy ?? "").ToUpper())
                {
                    case "GLOBAL":
                    case "0":
                        return 0;
                    case "REGION":
                    case "1":
                        return 1;
                    case "STATE":
                    case "2":
                        return 2;
                    case "AREACODE":
                    case "3":
                        return 3;
                    default:
                        return -1;
                }
            }
        }
    }
}