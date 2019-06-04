using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using C_Sharp.Models;
using Microsoft.AspNetCore.Identity;

namespace C_Sharp.Controllers
{
    public class HomeController : Controller
    {

        private MyContext dbContext;

        // here we can "inject" our context service into the constructor
        public HomeController(MyContext context)
        {
            dbContext = context;
        }




        //!!!!!!!!!!!  GETS
        //!!!!!!!!!!!  GETS
        //!!!!!!!!!!!  GETS
        //!!!!!!!!!!!  GETS
        //!!!!!!!!!!!  GETS


        public IActionResult Index()
        {
            DateTime CurrentTime = DateTime.Now;
            ViewBag.Now = CurrentTime;
            HttpContext.Session.Clear();
            return View();
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetInt32("LoggedUser") == null)
            {
                return RedirectToAction("Index");
            }

            @ViewBag.UserID = (int)HttpContext.Session.GetInt32("LoggedUser");

            return View();
        }


        //!!!!!!!!!!!  POSTS
        //!!!!!!!!!!!  POSTS
        //!!!!!!!!!!!  POSTS
        //!!!!!!!!!!!  POSTS
        //!!!!!!!!!!!  POSTS




        //TODO: THIS IS FOR MAKING A NEW OBJECT
        //TODO: THIS IS FOR MAKING A NEW OBJECT

        [HttpPost("Create")]
        public IActionResult Create(Object myObject)
        {
            dbContext.Add(myObject);
            dbContext.SaveChanges();
            return View();
        }


        //TODO: THIS IS FOR DELETING AN OBJECT
        //TODO: THIS IS FOR DELETING AN OBJECT


        [HttpGet("delete/{id}")]
        public IActionResult delete(int id)
        {
            // Quote currQuote = dbContext.Quote
            // .SingleOrDefault(q => q.QuoteID == id);



            // dbContext.Remove(currQuote);
            // dbContext.SaveChanges();


            return RedirectToAction("Dashboard");
        }


        //TODO: THIS IS TO ADD A RELATIONSHIP
        //TODO: THIS IS TO ADD A RELATIONSHIP

        [HttpPost("like")]
        public IActionResult Like(Object myObject)
        {
            // newLike.UserID = (int)HttpContext.Session.GetInt32("LoggedUser");
            // dbContext.Add(myObject);
            // dbContext.SaveChanges();


            return RedirectToAction("Dashboard");
        }

        //TODO: THIS IS FOR REMOVING RELATIONSHIP
        //TODO: THIS IS FOR REMOVING RELATIONSHIP

        [HttpGet("unlike/{ID}")]
        public IActionResult Unlike(int ID)
        {

            // Likes currLike = dbContext.Likes
            // .SingleOrDefault(u => u.UserID == HttpContext.Session.GetInt32("LoggedUser") && u.QuoteID == ID);


            // dbContext.Remove(currLike);
            // dbContext.SaveChanges();


            return RedirectToAction("Dashboard");
        }




        //?            END OF WEBSITE FEATURES
        //?            END OF WEBSITE FEATURES
        //?            END OF WEBSITE FEATURES
        //?            END OF WEBSITE FEATURES
        //?            END OF WEBSITE FEATURES
        //!!!!!!!!!!!  POST FOR REGISTER AND LOGIN
        //!!!!!!!!!!!  POST FOR REGISTER AND LOGIN
        //!!!!!!!!!!!  POST FOR REGISTER AND LOGIN
        //!!!!!!!!!!!  POST FOR REGISTER AND LOGIN
        //!!!!!!!!!!!  POST FOR REGISTER AND LOGIN


        [HttpPost]
        [Route("submit")]
        public IActionResult Submit(User NewUser)
        {
            if (ModelState.IsValid)
            {
                if (dbContext.User.Any(u => u.Email == NewUser.Email))
                {

                    ModelState.AddModelError("Email", "Email already in use!");

                    return View("Index");
                }

                // Initializing a PasswordHasher object, providing our User class as its
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                NewUser.Password = Hasher.HashPassword(NewUser, NewUser.Password);
                //Save your user object to the database
                dbContext.Add(NewUser);
                // OR dbContext.Users.Add(newUser);
                dbContext.SaveChanges();
                // Other code
                @ViewBag.Success = "You may now log in.";
                return View("Index");
            }
            return View("Index");
        }

        [HttpPost("log")]
        public IActionResult Log(LoginUser submission)
        {
            var userInDb = dbContext.User.FirstOrDefault(u => u.Email == submission.Email);
            if (ModelState.IsValid)
            {
                // If inital ModelState is valid, query for a user with provided email
                // If no user exists with provided email
                if (userInDb == null)
                {
                    // Add an error to ModelState and return to View!
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("Index");
                }

                // Initialize hasher object
                var hasher = new PasswordHasher<LoginUser>();

                // varify provided password against hash stored in db
                var result = hasher.VerifyHashedPassword(submission, userInDb.Password, submission.Password);

                // result can be compared to 0 for failure
                if (result == 0)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("Index");
                }
            }
            HttpContext.Session.SetInt32("LoggedUser", userInDb.UserID);
            return RedirectToAction("Dashboard");
        }


        //!!!!!!!!!!!  ERRORS
        //!!!!!!!!!!!  ERRORS


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



    }
}
