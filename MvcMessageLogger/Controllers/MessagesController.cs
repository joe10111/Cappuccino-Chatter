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
        [Route("/users/{Id:int}/messages")]
        public IActionResult CreateMessage(int Id, string content)
        {
            var user = _context.Users.Where(u => u.Id == Id).Include(u => u.Messages).First();
           
            var newMessage = new Message(content);

            newMessage.CreatedAt = DateTime.Now.ToUniversalTime();

            user.Messages.Add(newMessage);
           
            _context.SaveChanges();

            return RedirectToAction("Show", "Users", new { id = Id });
        }

       /* Spent most of my morning on wensday to try to figure this out, 
        * I delted the view but will leave this code here, it cant find the view most times
        * then when it dose it int being passed the correct info 
        * decided that I dont want too add this functinallity becuse it would
        * make me re-structure how I am adding and viewing messages
        * next time :)
        [Route("/users/{userId:int}/messages/{messageId:int}/edit")]
        public IActionResult Edit(int userId, int messageId)
        {
            var message = _context.Messages.Find(messageId);

            ViewData["userId"] = userId;

            return View(message);
        }

        
        [HttpPost]
        [Route("/users/{userId:int}/messages/{Id:int}")]
        public IActionResult Update(int userId, Message message)
        {
            _context.Messages.Update(message);
            _context.SaveChanges();

            return RedirectToAction("show", new { id = userId });
        }
       */
    }
}
