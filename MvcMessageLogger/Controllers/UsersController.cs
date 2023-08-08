﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        [Route("users/stats")]
        public IActionResult Stats(int Id)
        {
            var userWithMessages = _context.Users.Include(user => user.Messages);

            return View(userWithMessages);
        }

        // GET: Users/LogIn
        [HttpGet("users/login")]
        public IActionResult LogIn()
        {
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
    }
}
