using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetFramwork_WildNature.Common
{
    [Serializable]
    public class UserLogin
    {
        public int UserId { get; set; }
        public string Name  { get; set; }
    }
}