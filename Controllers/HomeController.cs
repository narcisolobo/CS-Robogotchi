using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Robogotchi.Controllers {
    public class HomeController : Controller {

        private string Chance () {
            Random rand = new Random();
            string likeNoLike = "like";
            int chance = rand.Next(1,5);
            if (chance == 1){
                likeNoLike = "nolike";
            }
            return likeNoLike;
        }

        [HttpGet]
        [Route ("")]
        public IActionResult Index () {
            int? fullness = HttpContext.Session.GetInt32 ("Fullness");
            int? happiness = HttpContext.Session.GetInt32 ("Happiness");
            int? beer = HttpContext.Session.GetInt32 ("Beer");
            int? energy = HttpContext.Session.GetInt32 ("Energy");
            string state = HttpContext.Session.GetString ("State");
            string message = HttpContext.Session.GetString ("Message");
            string result = HttpContext.Session.GetString ("Result");

            if (fullness == null) {
                HttpContext.Session.SetInt32 ("Fullness", 20);
            }
            if (happiness == null) {
                HttpContext.Session.SetInt32 ("Happiness", 20);
            }
            if (beer == null) {
                HttpContext.Session.SetInt32 ("Beer", 3);
            }
            if (energy == null) {
                HttpContext.Session.SetInt32 ("Energy", 50);
            }
            if (state == null) {
                HttpContext.Session.SetString ("State", "happy");
            }
            if (message == null) {
                HttpContext.Session.SetString ("Message", "This is Robbie. Please take good care of him!");
            }
            if (result == null) {
                HttpContext.Session.SetString ("Result", "");
            }

            ViewBag.CurrentFullness = HttpContext.Session.GetInt32 ("Fullness");
            ViewBag.CurrentHappiness = HttpContext.Session.GetInt32 ("Happiness");
            ViewBag.CurrentBeer = HttpContext.Session.GetInt32 ("Beer");
            ViewBag.CurrentEnergy = HttpContext.Session.GetInt32 ("Energy");
            ViewBag.CurrentState = HttpContext.Session.GetString ("State");
            ViewBag.CurrentMessage = HttpContext.Session.GetString ("Message");
            ViewBag.CurrentResult = HttpContext.Session.GetString ("Result");

            if (ViewBag.CurrentFullness <= 0 || ViewBag.CurrentHappiness <= 0){
                ViewBag.CurrentState = "boohoo";
                message = "You let Robbie die YOU MONSTER!";
                HttpContext.Session.SetString ("Message", message);
                HttpContext.Session.SetString ("Result", "");
                ViewBag.CurrentMessage = HttpContext.Session.GetString ("Message");
                ViewBag.CurrentResult = HttpContext.Session.GetString ("Result");
            }

            if (ViewBag.CurrentFullness >= 100 && ViewBag.CurrentHappiness >= 100 && ViewBag.CurrentEnergy >= 100){
                ViewBag.CurrentState = "woohoo";
                message = "Robbie is our new overlord! YOU WIN!! (or did you?)";
                HttpContext.Session.SetString ("Message", message);
                HttpContext.Session.SetString ("Result", "");
                ViewBag.CurrentMessage = HttpContext.Session.GetString ("Message");
                ViewBag.CurrentResult = HttpContext.Session.GetString ("Result");
            }

            return View ();
        }

        [HttpPost]
        [Route ("Feed")]
        public IActionResult Feed () {
            Random rand = new Random();
            int? beer = HttpContext.Session.GetInt32 ("Beer");
            int? fullness = HttpContext.Session.GetInt32 ("Fullness");
            string state = HttpContext.Session.GetString ("State");
            string message = HttpContext.Session.GetString ("Message");
            string result = HttpContext.Session.GetString ("Result");
            if (beer > 0) {
                string likeNoLike = Chance();
                if (likeNoLike == "like"){
                    beer -= 1;
                    HttpContext.Session.SetInt32 ("Beer", (int)beer);
                    int fullnessIncrease = rand.Next(5,11);
                    fullness += fullnessIncrease;
                    HttpContext.Session.SetInt32 ("Fullness", (int)fullness);
                    HttpContext.Session.SetString ("Message", "Robbie chugged a beer!");
                    HttpContext.Session.SetString ("Result", $"(Fullness +{fullnessIncrease}, Beer -1)");
                    HttpContext.Session.SetString ("State", "happy");
                } else {
                    beer -= 1;
                    HttpContext.Session.SetInt32 ("Beer", (int)beer);
                    HttpContext.Session.SetString ("Message", "Gross! That beer was spoiled!");
                    HttpContext.Session.SetString ("Result", $"(Fullness +0, Beer -1)");
                    HttpContext.Session.SetString ("State", "mad");
                }
            } else {
                message = "You do not have enough beer for Robbie!";
                HttpContext.Session.SetString ("Message", message);
                HttpContext.Session.SetString ("Result", "");
                HttpContext.Session.SetString ("State", "mad");
            }
            return RedirectToAction ("Index");
        }

        [HttpPost]
        [Route ("Play")]
        public IActionResult Play () {
            Random rand = new Random();
            int? happiness = HttpContext.Session.GetInt32 ("Happiness");
            int? energy = HttpContext.Session.GetInt32 ("Energy");
            string state = HttpContext.Session.GetString ("State");
            string message = HttpContext.Session.GetString ("Message");
            string result = HttpContext.Session.GetString ("Result");
            if (energy >= 5){
                string likeNoLike = Chance();
                if (likeNoLike == "like"){
                    energy -= 5;
                    HttpContext.Session.SetInt32 ("Energy", (int)energy);
                    int happinessIncrease = rand.Next(5,11);
                    happiness += happinessIncrease;
                    HttpContext.Session.SetInt32 ("Happiness", (int)happiness);
                    HttpContext.Session.SetString ("Message", "You played with Robbie!");
                    HttpContext.Session.SetString ("Result", $"(Happiness +{happinessIncrease}, Energy -5)");
                    HttpContext.Session.SetString ("State", "happy");
                } else {
                    energy -= 5;
                    HttpContext.Session.SetInt32 ("Energy", (int)energy);
                    HttpContext.Session.SetString ("Message", "Robbie doesn't want to play!");
                    HttpContext.Session.SetString ("Result", "(Happiness +0, Energy -5)");
                    HttpContext.Session.SetString ("State", "mad");
                }
            } else {
                message = "Robbie is too tired to play!";
                HttpContext.Session.SetString ("Message", message);
                HttpContext.Session.SetString ("Result", "");
                HttpContext.Session.SetString ("State", "mad");
            }
            return RedirectToAction ("Index");
        }
        
        [HttpPost]
        [Route ("Work")]
        public IActionResult Work () {
            Random rand = new Random();
            int? energy = HttpContext.Session.GetInt32 ("Energy");
            int? beer = HttpContext.Session.GetInt32 ("Beer");
            string state = HttpContext.Session.GetString ("State");
            string message = HttpContext.Session.GetString ("Message");
            string result = HttpContext.Session.GetString ("Result");
            if (energy >= 5){
                energy -= 5;
                HttpContext.Session.SetInt32 ("Energy", (int)energy);
                int beerIncrease = rand.Next(1,4);
                beer += beerIncrease;
                HttpContext.Session.SetInt32 ("Beer", (int)beer);
                HttpContext.Session.SetString ("Message", "Robbie went to work!");
                HttpContext.Session.SetString ("Result", $"(Beer +{beerIncrease}, Energy -5)");
                HttpContext.Session.SetString ("State", "happy");
            } else {
                message = "Robbie is too tired to work!";
                HttpContext.Session.SetString ("Message", message);
                HttpContext.Session.SetString ("Result", "");
                HttpContext.Session.SetString ("State", "mad");
            }
            return RedirectToAction ("Index");
        }
        
        [HttpPost]
        [Route ("Charge")]
        public IActionResult Charge () {
            int? energy = HttpContext.Session.GetInt32 ("Energy");
            int? fullness = HttpContext.Session.GetInt32 ("Fullness");
            int? happiness = HttpContext.Session.GetInt32 ("Happiness");
            string state = HttpContext.Session.GetString ("State");
            string message = HttpContext.Session.GetString ("Message");
            string result = HttpContext.Session.GetString ("Result");
            energy += 15;
            HttpContext.Session.SetInt32 ("Energy", (int)energy);
            fullness -= 5;
            HttpContext.Session.SetInt32 ("Fullness", (int)fullness);
            happiness -= 5;
            HttpContext.Session.SetInt32 ("Happiness", (int)happiness);
            message = "Robbie recharged his batteries!";
            HttpContext.Session.SetString ("Message", message);
            HttpContext.Session.SetString ("Result", "(Fullness -5, Happiness -5, Energy +15");
            HttpContext.Session.SetString ("State", "happy");
            return RedirectToAction ("Index");
        }
        
        [HttpPost]
        [Route ("Reset")]
        public IActionResult Reset () {
            HttpContext.Session.Clear();
            return RedirectToAction ("Index");
        }
    }
}