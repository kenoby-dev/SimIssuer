using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SimIssuer.Models
{
    public class LoginModel
    {
        [Required]
        public string Realm { get; set; }
        [Required]
        public string UserId { get; set; }
        public string Reply { get; set; }
    }

}