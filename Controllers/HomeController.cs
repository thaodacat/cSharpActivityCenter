using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using activityCenter.Models;
using activityCenter.Contexts;
using Microsoft.EntityFrameworkCore;

namespace activityCenter.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context; 

        public HomeController(MyContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost ("Register")]
        public IActionResult Register (User user) 
        {
            if (ModelState.IsValid) 
            {
                if (_context.Users.Any (u => u.Email == user.Email)) 
                {
                    ModelState.AddModelError ("Email", "Email already in use!");
                    return View ("Index");
                } 
                else 
                {
                    PasswordHasher<User> Hasher = new PasswordHasher<User> ();
                    user.Password = Hasher.HashPassword (user, user.Password);
                    _context.Users.Add (user);
                    _context.SaveChanges ();
                    HttpContext.Session.SetInt32 ("userid", user.UserId);
                    return RedirectToAction ("Dashboard");
                }
            }
            return View ("Index");
        }


        [HttpPost ("Login")]
        public IActionResult Login (Login userLogin) 
        {
            if (ModelState.IsValid) 
            {
                var userInDb = _context.Users.FirstOrDefault (u => u.Email == userLogin.LoginEmail);
                if (userInDb == null) 
                {
                    ModelState.AddModelError ("LoginEmail", "Invalid Email/Password");
                    return View ("Index");
                } 
                else 
                {
                    var hasher = new PasswordHasher<Login> ();
                    var result = hasher.VerifyHashedPassword (userLogin, userInDb.Password, userLogin.LoginPassword);
                    if (result == 0) {
                        ModelState.AddModelError ("LoginEmail", "Invalid Email/Password");
                        return View ("Index");
                    } 
                    else 
                    {
                        HttpContext.Session.SetInt32 ("userid", userInDb.UserId);
                        HttpContext.Session.SetString ("FirstName", userInDb.FirstName);
                        return RedirectToAction("Dashboard");
                    }
                }
            } 
            else 
            {
                return View ("Index");
            }
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard () 
        {
            int? IdinSession = HttpContext.Session.GetInt32 ("userid");
            if (IdinSession == null) 
            {
                return RedirectToAction ("Index");
            }
            ViewBag.User = _context.Users
                .FirstOrDefault (u => u.UserId == IdinSession);
            List<Activityy> activityys = _context.Activityys
                                .Include(a => a.Organizer)
                                .Include(a => a.Guests)
                                .OrderBy(a => a.Date)
                                .ThenBy(a => a.Time)
                                // .Include(a => a.Time )
                                .ToList();
            activityys.RemoveAll (a => a.Date < DateTime.Now && a.Time < DateTime.Now);
            return View("Dashboard",activityys);
        }

        [HttpGet("new/activity")]
        public IActionResult NewActivity()
        {
            int? IdinSession = HttpContext.Session.GetInt32 ("userid");
            if (IdinSession == null)
            {
                return RedirectToAction("Index");
            }
            return View("CreateActivity");
        }


        [HttpPost ("create/activity")]
        public IActionResult CreateActivity (Activityy newactivityy) 
        {
            if (ModelState.IsValid) 
            {
                int? IdinSession = HttpContext.Session.GetInt32 ("userid");
                newactivityy.UserId = (int) IdinSession;
                _context.Activityys.Add(newactivityy);
                _context.SaveChanges();
                return RedirectToAction ("Dashboard");
            } 
            else 
            {
                return View ("CreateActivity");
            }
        }


        [HttpGet("activity/{activityyId}")]
        public IActionResult ActivityInfo(int activityyId)
        {
            int? IdinSession = HttpContext.Session.GetInt32 ("userid");
            if (IdinSession == null) 
            {
                return RedirectToAction ("Index");
            }
            ViewBag.User = _context.Users
                .FirstOrDefault (u => u.UserId == IdinSession);
            var activityy = _context.Activityys
                            .Include(p => p.Organizer)
                            .Include(p => p.Guests)
                            .ThenInclude(rsvp => rsvp.Guest)
                            .FirstOrDefault(p => p.ActivityyID == activityyId);
            
            return View(activityy);
        }


        [HttpGet("activity/{activityyId}/rsvp")]

        public IActionResult RSVP(int activityyId)
        {
            int? IdinSession = HttpContext.Session.GetInt32 ("userid");
            if (IdinSession == null) 
            {
                return RedirectToAction ("Index");
            }
            Attend rsvp = new Attend();
            rsvp.ActivityyID = activityyId;
            rsvp.UserID = (int) IdinSession;
            _context.Add(rsvp);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("activity/{activityyID}/leave")]
        public IActionResult unRSVP(int activityyID)
        {
            int? IdinSession = HttpContext.Session.GetInt32 ("userid");
            if (IdinSession == null) 
            {
                return RedirectToAction ("Index");
            }
            Attend unrsvp = _context.Attends
                .FirstOrDefault(a => a.ActivityyID == activityyID && a.UserID == (int) IdinSession);
            _context.Remove(unrsvp);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("activity/{activityyId}/delete")]
        public IActionResult DeleteActivity(int activityyId)
        {
            int? IdinSession = HttpContext.Session.GetInt32 ("userid");
            if (IdinSession == null) 
            {
                return RedirectToAction ("Index");
            }
            Activityy rsvp = _context.Activityys
                .FirstOrDefault( p => p.ActivityyID == activityyId);
            _context.Activityys.Remove(rsvp);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }




        [HttpGet ("logout")]
        public IActionResult Logout () {
            HttpContext.Session.Clear ();
            return Redirect ("/");
        }







    }
}






