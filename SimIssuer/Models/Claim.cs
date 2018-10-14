using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimIssuer.Models
{
    public class Claim
    {
        public string Namespace { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}