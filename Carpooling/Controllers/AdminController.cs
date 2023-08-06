using AutoMapper;
using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.BusinessLayer.Validation;
using Carpooling.BusinessLayer.Validation.Contracts;
using Carpooling.Models;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Net;
using System.Security.Authentication;
using System.Security.Claims;

namespace Carpooling.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        //private readonly IToastNotification _toast;
        private readonly IMapper _mapper;
        private readonly IAuthValidator _authValidator;
        public AdminController(IUserService userService, IMapper mapper, IAuthValidator authValidator)
        {
            _userService = userService;
            //_toast = toast;
            _mapper = mapper;
            _authValidator = authValidator;
        }
        public async Task<IActionResult> ListUsers()
        {

            try
            {
                var users = await _userService.GetAllUsersAsync();
                return View(users);
            }
            catch (EmptyListException ex)
            {
                this.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = ex.Message;
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                ViewBag.Credentials = Request.Headers["Authorization"];
                return View(user);

            }
            catch (EntityNotFoundException ex)
            {
                this.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = ex.Message;
                return View("Error");
            }

        }
        [HttpGet]
        public async Task<IActionResult> Block([FromRoute] string id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                ViewBag.Credentials = Request.Headers["Authorization"];
                return View(user);

            }
            catch (EntityNotFoundException ex)
            {
                this.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = ex.Message;
                return View("Error");
            }

        }
        [HttpPost]
        public async Task<IActionResult> BlockConfirmed([FromRoute] string id)
        {
            await _userService.BanUserById(id);
            return RedirectToAction("ListUsers", "Admin");
        }
        [HttpPost]
        public async Task<IActionResult> UnblockConfirmed([FromRoute] string id)
        {
            await _userService.UnbanUserById(id);
            return RedirectToAction("ListUsers", "Admin");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] string id)
        {
            await _userService.DeleteUserWhenAdminAsync(id);
            return RedirectToAction("ListUsers", "Admin");
        }
        [HttpPost]
        public async Task<IActionResult> ChangeRoleToAdministrator([FromRoute] string id)
        {
            try
            {
                await _userService.ConvertToAdministrator(id);
                return RedirectToAction("ListUsers", "Admin");

            }
            catch (EntityNotFoundException ex)
            {
                this.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = ex.Message; ;
                return View("Error");
            }
        }  
    }
}
