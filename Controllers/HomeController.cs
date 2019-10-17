using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WeddingPlanner.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WeddingPlanner.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
     
        // here we can "inject" our context service into the constructor
        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        //LOGIN ANG REG ROUTES
        
        //LOGIN AND REG PAGE
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Create")]
        public IActionResult Create(User user)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);
                dbContext.Add(user);
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("userId", user.UserId);
                HttpContext.Session.SetString("userName", user.FirstName);
                return RedirectToAction("WeddingPlanner");
            }
            else
            {
                return View("Index");
            }
        }

        //LOGIN POST ROUTE
        [HttpPost("Login")]
        public IActionResult Login(LoginUser userSubmission)
        {
            if(ModelState.IsValid)
            {
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == userSubmission.LoginEmail);
                if(userInDb == null)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid Email/Password");
                    return View("Index");
                }
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.LoginPassword);
                if(result == 0)
                {
                    ModelState.AddModelError("LoginPassword", "Username/password combination incorrect");
                    return View("Index");
                }
                HttpContext.Session.SetInt32("userId", userInDb.UserId);
                HttpContext.Session.SetString("userName", userInDb.FirstName);
                return RedirectToAction("WeddingPlanner");
            }
            else
            {
                return View("Index");
            }
        }

        //WEDDING PLANNER ROUTES


        //WEDDING PLANNER MAIN PAGE (ALL EVENTS)
        [HttpGet("WeddingPlanner")]
        public IActionResult WeddingPlanner()
        {
            if(HttpContext.Session.GetString("userName") == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                List<Wedding> AllWeddings = dbContext.Weddings
                    .Include(w => w.Guests)
                    .ThenInclude(w => w.User)
                    .ToList();
                foreach(Wedding wed in AllWeddings)
                {
                    if(Convert.ToDateTime(wed.Date) < DateTime.Now)
                    {
                        dbContext.Weddings.Remove(wed);
                        dbContext.SaveChanges();
                    }
                }
                ViewBag.CurrentUser = HttpContext.Session.GetInt32("userId");
                return View(AllWeddings);
            }
        }

        //NEW WEDDING FORM PAGE
        [HttpGet("NewWedding")]
        public IActionResult NewWedding()
        {
            if(HttpContext.Session.GetString("userName") == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        //NEW WEDDING POST ROUTE 
        [HttpPost("CreateWedding")]
        public IActionResult CreateWedding(Wedding wedding)
        {
            if(ModelState.IsValid)
            {
                int? userId = HttpContext.Session.GetInt32("userId");
                wedding.Creator = (int)userId;
                dbContext.Add(wedding);
                dbContext.SaveChanges();
                return RedirectToAction("WeddingDetails", new{WedID = wedding.WeddingId});
            }
            else
            {
                return View("NewWedding");
            }
        }

        //VIEW SPECIFIC WEDDING PAGE
        [HttpGet("WeddingDetails/{WedId}")]
        public IActionResult WeddingDetails(int WedID)
        {
            if(HttpContext.Session.GetString("userName") == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                Wedding thisWedding = dbContext.Weddings
                    .Include(u => u.Guests)
                    .ThenInclude(u => u.User)
                    .FirstOrDefault(w => w.WeddingId == WedID);
                return View(thisWedding);
            }
        }

        [HttpGet("RSVP/{id}")]
        public IActionResult RSVP(int id)
        {
            if(HttpContext.Session.GetString("userName") == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                int? UserId = HttpContext.Session.GetInt32("userId");
                Wedding thisWedding = dbContext.Weddings
                    .Include(u => u.Guests)
                    .ThenInclude(u => u.User)
                    .FirstOrDefault(w => w.WeddingId == id);
                User CurrentUser = dbContext.Users.FirstOrDefault(i => i.UserId == UserId);
                var WantingToRsvp = dbContext.Attendees.FirstOrDefault(w => w.WeddingId == id && w.UserId == CurrentUser.UserId);
                if(WantingToRsvp == null)
                {
                    Attendee guest = new Attendee
                    {
                        UserId = (int)UserId,
                        WeddingId = id
                    };
                    dbContext.Add(guest);
                    dbContext.SaveChanges();
                }
                return RedirectToAction("WeddingPlanner");
            }
        }

        [HttpGet("unRSVP/{id}")]
        public IActionResult unRSVP(int id)
        {
            if(HttpContext.Session.GetString("userName") == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                int? UserId = HttpContext.Session.GetInt32("userId");
                Wedding thisWedding = dbContext.Weddings
                    .Include(u => u.Guests)
                    .ThenInclude(u => u.User)
                    .FirstOrDefault(w => w.WeddingId == id);
                User CurrentUser = dbContext.Users.FirstOrDefault(i => i.UserId == UserId);
                var WantingToRsvp = dbContext.Attendees.FirstOrDefault(w => w.WeddingId == id && w.UserId == CurrentUser.UserId);
                if(WantingToRsvp != null)
                {
                    thisWedding.Guests.Remove(WantingToRsvp);
                    dbContext.SaveChanges();
                }
                return RedirectToAction("WeddingPlanner");
            }
        }

        //DELETE WEDDING FROM DATABASE
        [HttpGet("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            Wedding thisWedding = dbContext.Weddings
                .FirstOrDefault(w => w.WeddingId == id);
            System.Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");
            System.Console.WriteLine(thisWedding.Creator);
            User thisUser = dbContext.Users.FirstOrDefault(u => u.UserId == thisWedding.Creator);
            int? userId = HttpContext.Session.GetInt32("userId");
            if((int)userId != thisUser.UserId)
            {
                return RedirectToAction("WeddingPlanner");
            }
            else
            {
                dbContext.Weddings.Remove(thisWedding);
                dbContext.SaveChanges();
                return RedirectToAction("WeddingPlanner");
            }
        }


        //LOGIN AND REG CLEAR SESSION/LOGOUT ROUTE
        [HttpGet("LogOut")]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
