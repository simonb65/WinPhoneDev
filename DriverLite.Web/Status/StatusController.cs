using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DriverLite.Web.Status
{
    public class StatusController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return Content("<html><body>OK! " + DateTimeOffset.Now.ToString("O") + "</body></html>");
        }
    }
}