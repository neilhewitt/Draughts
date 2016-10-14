using Draughts.Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Draughts.Site.Controllers
{
    public class GameController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("PlayerNames");
        }
        
        public ActionResult PlayerNames()
        {
            return View(new PlayerNamesModel());
        }

        [HttpPost]
        public ActionResult PlayerNames(PlayerNamesModel model)
        {
            return View(model);
        }
    }
}