using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SlackOverload.Models;

namespace SlackOverload.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData["Username"] = HttpContext.Session.GetString("username");
            ViewData["UID"] = HttpContext.Session.GetInt32("UID");

            return View();
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogIn(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                TempData["ErrorMsg"] = "User/password error!";
                return View();
            }

            if (username.ToLower() == "nic")
            {
                HttpContext.Session.SetString("username", "Nic");
                HttpContext.Session.SetInt32("UID", 69420);
            }
            else if (username.ToLower() == "della")
            {
                HttpContext.Session.SetString("username", "Della");
                HttpContext.Session.SetInt32("UID", 88888);
            }
            else if (username.ToLower() == "jacob")
            {
                HttpContext.Session.SetString("username", "Jacob");
                HttpContext.Session.SetInt32("UID", 613);
            } 
            else
            {
                TempData["ErrorMsg"] = "User/password error!";
                return View();
            }

            

            return RedirectToAction("Index");

        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();

            return View("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
