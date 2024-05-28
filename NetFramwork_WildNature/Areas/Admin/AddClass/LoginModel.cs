using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NetFramwork_WildNature.Areas.Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Vui lòng nhập Username")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Password")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}