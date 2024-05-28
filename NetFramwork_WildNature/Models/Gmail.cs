using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetFramwork_WildNature.Models
{
    public class Gmail
    {
        public string To { get; set; }
        public string From { get; set; }

        public string Sub { get; set; }
        public string Body { get; set; }
    }
}