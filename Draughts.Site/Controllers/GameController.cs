using Draughts.Core;
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
        private Game _game;

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
            BeginGame(model);
            return RedirectToAction("DisplayBoard");
        }

        private void BeginGame(PlayerNamesModel model)
        {
            bool player1Computer = String.IsNullOrWhiteSpace(model.Player1Name);
            bool player2Computer = String.IsNullOrWhiteSpace(model.Player2Name);
            _game = new Game(player1Computer ? "Computer" : model.Player1Name, player2Computer ? "Computer" : model.Player2Name, new DraughtsPlayer(), player1Computer, player2Computer);
        }
    }
}