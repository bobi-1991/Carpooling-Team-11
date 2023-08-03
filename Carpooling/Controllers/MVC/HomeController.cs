using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.Models;
using CarPooling.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carpooling.Controllers.MVC
{
    public class HomeController : Controller
    {
        private readonly IUserService userService;

        public HomeController(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await userService.GetAllAsync();

            var vm = new UsersListViewModel 
            {
                Users = users.ToList()
            };

            return View(vm);
        }
    }
}
