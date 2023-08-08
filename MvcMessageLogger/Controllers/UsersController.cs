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
            var userWithMessages = _context.Users.Where(user => user.Id == Id).Include(user => user.Messages).FirstOrDefault(); ;

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

    }
}
