using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimIssuer.Models
{
    public class User
    {
        public string Uid { get; set; }
        public List<Claim> Claims { get; set; }
    }
}