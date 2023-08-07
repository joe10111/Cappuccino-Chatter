using Microsoft.AspNetCore.Mvc;
using MvcMessageLogger.Models;
using MvcMessageLogger.DataAccess;

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

            var newMovieId = user.Id;

            return RedirectToAction("Index", new { id = newMovieId });
        }

    }
}
