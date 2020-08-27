using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PersonalSite.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace PersonalSite.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;
        public HomeController(MyContext context)
        {
            _context = context;
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ GET ROUTES ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        [HttpGet]
        [Route("dashboard")]
        public IActionResult Dashboard()
        {
            // if user is not logged in and tries to go to this route then redirect back to home
            if (HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("Index", "User");
            }
            // include a view model that allows all the models to display on one page
            DashboardModel view = new DashboardModel()
            {
                Users = new User(),
            };
            int? user_id = HttpContext.Session.GetInt32("id");
            User curruser = _context.Users.Where(u => u.Id == user_id).SingleOrDefault();

            return View(view);
        }

        // Get Route for About Me from Dashboard Navbar
        [HttpGet]
        [Route("AboutMe")]
        public IActionResult AboutMe()
        {
            if (HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("Index", "User");
            }
            return View();
        }

        // Get Route for My Resume from Dashboard Navbar
        [HttpGet]
        [Route("Resume")]
        public IActionResult Resume()
        {
            if (HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("Index", "User");
            }
            return View();
        }
    }
}

