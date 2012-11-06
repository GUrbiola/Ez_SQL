using System;
using System.Collections.Generic;

using System.Web;
using CallView360_WebApp.ViewModels;

namespace CallView360_WebApp.Areas.RapidRecall.ViewModels.FindNumbers
{
    public class FindNumbersGridDataVM
    {

        public string Number { get; set; }
        public string Formatted { get; set; }
        public string Vanity { get; set; }
        public string Tags { get; set; }
        public string Price { get; set; }
        public string Status { get; set; }
        public string Availability { get; set; }
        public string LastCDR { get; set; }
        public int CallsLast30Days { get; set; }
        public string Owner { get; set; }
        public string RespOrg { get; set; }
        public string Vendor { get; set; }

    }
}