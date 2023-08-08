using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcMessageLogger.DataAccess;
using MvcMessageLogger.Models;

namespace MvcMessageLogger.Controllers
{
    public class MessagesController : Controller
    {
        private readonly MvcMessageLoggerContext _context;

        public MessagesController(MvcMessageLoggerContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var messages = _context.Messages;

            return View(messages);
        }

        // POST: /Messages
        [HttpPost]
        [Route("/users/{Id:int}")]
        public IActionResult CreateMessage(int Id, string content)
        {
            var user = _context.Users.Where(u => u.Id == Id).Include(u => u.Messages).First();
           
            var newMessage = new Message(content);

            newMessage.CreatedAt = DateTime.Now.ToUniversalTime();

            user.Messages.Add(newMessage);
           
            _context.SaveChanges();

            return RedirectToAction("Show", "Users", new { id = Id });
        }
    }
}
