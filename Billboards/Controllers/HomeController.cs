using Billboards.Models;
using Microsoft.AspNetCore.Mvc;
using ModelServices.UserServicing;
using System.Collections.Generic;
using Server;

namespace Billboards.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        
        public HomeController(IUserService userService)
        {
            
               _userService = userService;
        }
        public IActionResult Index()
        {
            IEnumerable<User> users = _userService.GetUsers();
            return View(users);
        }


    }
}
