using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BeltExam.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace BeltExam.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
        public HomeController(MyContext context)
        {
            dbContext=context;
        }
        private void DeleteExpired()
        {
            var expiredToDo=dbContext.ToDos.Where(t=>t.ToDoDate<DateTime.Now);
            if(expiredToDo.Count()>0)
            {
                foreach(var todo in expiredToDo)
                {
                    dbContext.ToDos.Remove(todo);
                }
                dbContext.SaveChanges();
            }
            HttpContext.Session.SetString("deleteExpired", DateTime.Now.ToString());
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Register")]
        public IActionResult Register(User newUser)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                dbContext.Users.Add(newUser);
                dbContext.SaveChanges();
                HttpContext.Session.SetString("FirstName", newUser.FirstName);
                HttpContext.Session.SetInt32("UserId", newUser.UserId);
                return RedirectToAction("Success");
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost("login")]
        public IActionResult Login(UserLogin userSubmission)
        {
            if(ModelState.IsValid)
            {
                // List<User>AllUsers=dbContext.Users.ToList();
                var hasher = new PasswordHasher<UserLogin>();
                var signedInUser = dbContext.Users.FirstOrDefault(u => u.Email == userSubmission.LoginEmail);
                
                if(signedInUser == null)
                {
                    ViewBag.Message="Email/Password is invalid";
                    return View("Index");
                }
                else
                {
                    var result = hasher.VerifyHashedPassword(userSubmission, signedInUser.Password, userSubmission.LoginPassword);
                    if(result==0)
                    {
                        ViewBag.Message="Email/Password is invalid";
                        return View("Index");
                    }
                }
                
                
                HttpContext.Session.SetInt32("UserId", signedInUser.UserId);
                return RedirectToAction("Success");
            }
            else
            {
                return View("Index");
            }
            
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }

        [HttpGet("Success")]
        public IActionResult Success()
        {
            User userInDb=dbContext.Users.FirstOrDefault(u=>u.UserId==(int)HttpContext.Session.GetInt32("UserId"));
            List<ToDo> AllActivities=dbContext.ToDos.Include(e=>e.ToDoCreator).Include(e=>e.Participants).ThenInclude(b=>b.NavUser).OrderBy(d=>d.ToDoDate).ToList();
            if (HttpContext.Session.GetString("deleteExpired")==null)
            {
                DeleteExpired();
            }
            else
            {
                DateTime deleteExpired;
                int day;
                if(DateTime.TryParse(HttpContext.Session.GetString("deleteExpired"), out deleteExpired))
                {
                    day=(DateTime.Now - deleteExpired).Days;
                    if(day>1)
                    {
                        DeleteExpired();
                    }
                }
            }
            if(userInDb==null)
            {
                return RedirectToAction("Logout");
            }
            else
            {

                ViewBag.User=userInDb;
                return View(AllActivities);
            }
        }

        [HttpGet("event/new")]
        public IActionResult AddToDo()
        {
            return View();
        }

        [HttpPost("create/new")]
        public IActionResult Create(ToDo newToDo)
        {
            if (ModelState.IsValid)
            {
                int? userInDb=HttpContext.Session.GetInt32("UserId");
                newToDo.UserId=(int)userInDb;
                dbContext.ToDos.Add(newToDo);
                dbContext.SaveChanges();
                return Redirect($"/show/{newToDo.ToDoId}");
            }
            else{
                return View("AddToDo");
            }
        }

        [HttpGet("cancel/{ToDoId}")]
        public IActionResult Cancel(int ToDoId)
        {
            ToDo ToBeCancelled=dbContext.ToDos.FirstOrDefault(t=>t.ToDoId==ToDoId);
            dbContext.ToDos.Remove(ToBeCancelled);
            dbContext.SaveChanges();
            return RedirectToAction("Success");
        }
        [HttpGet("going/{ToDoId}/{UserId}")]
        public IActionResult Going(int ToDoId, int UserId)
        {
            Banana goingTo=new Banana();
            goingTo.UserId=UserId;
            goingTo.ToDoId=ToDoId;
            dbContext.Bananas.Add(goingTo);
            dbContext.SaveChanges();
            return RedirectToAction("Success");
        }

        [HttpGet("leave/{ToDoId}/{UserId}")]
        public IActionResult Leave(int ToDoId, int UserId)
        {
            Banana toLeave=dbContext.Bananas.FirstOrDefault(b=>b.UserId==UserId && b.ToDoId==ToDoId);
            dbContext.Bananas.Remove(toLeave);
            dbContext.SaveChanges();
            return RedirectToAction("Success");
        }
        [HttpGet("show/{ToDoId}")]
        public IActionResult Show(int ToDoId)
        {
            User userInDb=dbContext.Users.FirstOrDefault(u=>u.UserId==(int)HttpContext.Session.GetInt32("UserId"));
            ToDo toShow=dbContext.ToDos.Include(e=>e.ToDoCreator).Include(t=>t.Participants).ThenInclude(u=>u.NavUser).FirstOrDefault(e=>e.ToDoId==ToDoId);
            ViewBag.User=userInDb;
            return View(toShow);
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
