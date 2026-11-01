using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProductsValidation.Models;
using ProductsValidation.Services;

namespace ProductsValidation.Controllers
{
    public class UsersController : Controller
    {
        private List<User> users;

        public UsersController(Data data)
        {
            users = data.Users;
        }

        public IActionResult Index()
        {
            return View("Index", users);
        }

        public IActionResult View(int id)
        {
            User user = users.Find(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // Форма заповнюється ЛИШЕ з query string
        [HttpGet]
        public IActionResult Create([FromQuery] User user)
        {
            ModelState.Clear(); // не показувати помилки при першому відкритті форми
            user.Id = NextId();
            return View(user);
        }

        // Дані знову беруться лише з query string (тіло форми ігнорується)
        [HttpPost]
        [ActionName("Create")]
        public IActionResult CreatePost([FromQuery] User user)
        {
            user.Id = NextId();

            if (!ModelState.IsValid)
            {
                return View(user);
            }

            users.Add(user);
            return RedirectToAction("View", new { id = user.Id });
        }

        private int NextId()
        {
            return users.Any() ? users.Max(u => u.Id) + 1 : 1;
        }
    }
}