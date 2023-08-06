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
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IToastNotification _toast;
        private readonly IMapper _mapper;
        private readonly IAuthValidator _authValidator;
        public UserController(IUserService userService, IToastNotification toast, IMapper mapper, IAuthValidator authValidator)
        {
            _userService = userService;
            _toast = toast;
            _mapper = mapper;
            _authValidator = authValidator;
        }
        public async Task<IActionResult> ListUsers()
        {

            try
            {
                var users = await _userService.GetAllAsync();
                var listUsers = _mapper.Map<IEnumerable<UserViewModel>>(users);
                return View(listUsers);
            }
            catch (EmptyListException ex)
            {
                _toast.AddErrorToastMessage(ex.Message);
                ViewBag.ErrorTitle = "";
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                var appUser = _mapper.Map<UserViewModel>(user);
                ViewBag.Credentials = Request.Headers["Authorization"];
                return View(appUser);

            }
            catch (EntityNotFoundException ex)
            {
                _toast.AddErrorToastMessage(ex.Message);
                ViewBag.ErrorTitle = "";
                return View("Error");
            }

        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed([FromHeader] string credentials,  string id)
        {
            User loggedUser = await _authValidator.ValidateCredentialAsync(credentials);
            await _userService.DeleteAsync(loggedUser, id);
            return RedirectToAction("ListUsers", "User");
        }
        public async Task<IActionResult> ChangeRoleToManager(string id)
        {
            try
            {
                await _userService.ConvertToManager(id);
                return RedirectToAction("ListUsers", "User");

            }
            catch (EntityNotFoundException ex)
            {
                _toast.AddErrorToastMessage(ex.Message);
                ViewBag.ErrorTitle = "";
                return View("Error");
            }
        }
        public async Task<IActionResult> CurrentUser()
        {
            try
            {

                var userID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var appUser = new UserViewModel();
                if (userID == null)
                {
                    return View(appUser);
                }
                var user = await _userService.GetByIdAsync(userID);
                appUser = _mapper.Map<UserViewModel>(user);


                return View(appUser);

            }
            catch (EntityNotFoundException ex)
            {
                _toast.AddErrorToastMessage(ex.Message);
                ViewBag.ErrorTitle = "";
                return View("Error");
            }
        }
    }
}
