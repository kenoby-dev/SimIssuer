using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace RelyingParty.Net.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            return View(identity);
        }

    }
}