using System;
using System.Collections.Generic;

using System.Web;

namespace CallView360_WebApp.Areas.RapidRecall.ViewModels.FindNumbers
{
    public class NumberAvailabilityVM
    {
        public string[] Clients { get; set; }
        public showLicensedOrReservedVM ShowOptions  { get; set; }
        
    }
}