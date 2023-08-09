using Microsoft.AspNetCore.Mvc;
using MvcMessageLogger.Models;
using MvcMessageLogger.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace MvcMessageLogger.Controllers
{
    public class UsersController : Controller
    {
        private readonly MvcMessageLoggerContext _context;

        public UsersController(MvcMessageLoggerContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var users = _context.Users;

            return View(users);
        }

        // GET: /Users/1
        [HttpGet]
        [Route("Users/{Id:int}")]
        public IActionResult Show(int Id)
        {
            var userWithMessages = _context.Users.Where(user => user.Id == Id).Include(user => user.Messages).FirstOrDefault();

           return View(userWithMessages);
        }

        public IActionResult New()
        {
            return View();
        }

        // POST: /users
        [HttpPost]
        public IActionResult Index(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            var newUserId = user.Id;

            return RedirectToAction("Index", new { id = newUserId });
        }

        // GET: /Users/:id/edit
        [Route("/users/{id:int}/edit")]
        public IActionResult Edit(int id)
        {
            var user = _context.Users.Find(id);

            return View(user);
        }

        // PUT: /Users/:id
        [HttpPost]
        [Route("/users/{id:int}")]
        public IActionResult Update(User users)
        {
            _context.Users.Update(users);
            _context.SaveChanges();

            return RedirectToAction("show", new { id = users.Id });
        }

        [HttpGet]
        [Route("users/stats")]
        public IActionResult Stats(int Id)
        {
            var userWithMessages = _context.Users.Include(user => user.Messages);

            return View(userWithMessages);
        }

        // GET: Users/LogIn
        [HttpGet("users/login")]
        public IActionResult LogIn(string username)
        {
            ViewBag.Username = username;

            return View();
        }

        [HttpPost("users/login")]
        public IActionResult LogIn(User userToLogin)
        {
            // Fetch the user with the given username from the database
            var user = _context.Users.FirstOrDefault(u => u.Username == userToLogin.Username);

            // If the user exists and the password is correct
            if (user != null && VerifyPassword(user.Password, userToLogin.Password))
            {
                return Json(new { success = true, redirectUrl = Url.Action("Show", "Users", new { Id = user.Id }) });
            }

            // If login fails, send a JSON response.
            return Json(new { success = false, message = "LogIn Failed" });
        }

        private bool VerifyPassword(string storedPassword, string providedPassword)
        {
            return storedPassword == providedPassword;
        }

        // DELETE: /Users/Delete/:id
        [HttpPost]
        [Route("Users/delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.Include(u => u.Messages).FirstOrDefault(u => u.Id == id);

            // Remove all messages associated with the user
            _context.Messages.RemoveRange(user.Messages);

            _context.Users.Remove(user);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
