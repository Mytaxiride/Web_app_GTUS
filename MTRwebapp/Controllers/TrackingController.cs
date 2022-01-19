using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MTRwebapp.Controllers
{
    public class TrackingController : Controller
    {
        // GET: Tracking
        public ActionResult Index(string trackno)
        {
            return View();
        }
    }
}