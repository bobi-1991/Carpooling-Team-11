using AutoMapper;
using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.BusinessLayer.Validation;
using Carpooling.BusinessLayer.Validation.Contracts;
using Carpooling.Models;
using Carpooling.PaginationHelper;
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
        private readonly IMapper _mapper;
        private readonly IAuthValidator _authValidator;
        public AdminController(IUserService userService, IMapper mapper, IAuthValidator authValidator)
        {
            _userService = userService;
            _mapper = mapper;
            _authValidator = authValidator;
        }
        public async Task<IActionResult> ListUsers(int? pg, string searchQuery)
        {
            try
            {
                var pageSize = 5; // Set your desired page size here

                var users = await _userService.GetAllUsersAsync();

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    users = SearchUsers(users.AsQueryable(), searchQuery);
                }

                if (pg < 1)
                {
                    pg = 1;
                }

                int recsCount = users.Count();

                var pager = new Pager(recsCount, pg ?? 1, pageSize);

                int recSkip = (pager.CurrentPage - 1) * pageSize;

                var data = users
                    .Skip(recSkip)
                    .Take(pager.PageSize)
                    .ToList();

                ViewBag.Pager = pager;

                return View(data);
            }
            catch (EmptyListException ex)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                ViewData["ErrorMessage"] = ex.Message;
                return View("Error");
            }
        }
        private IQueryable<User> SearchUsers(IQueryable<User> users, string searchQuery)
        {
            return users.Where(user =>
                user.UserName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                user.Email.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                (user.PhoneNumber != null && user.PhoneNumber.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
            );
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
            try
            {
                await _userService.BanUserById(id);
                return RedirectToAction("ListUsers", "Admin");
            }
            catch (EntityNotFoundException ex)
            {
                this.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = ex.Message;
                return View("Error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Unblock([FromRoute] string id)
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
        public async Task<IActionResult> UnblockConfirmed([FromRoute] string id)
        {
            try
            {
                await _userService.UnbanUserById(id);
                return RedirectToAction("ListUsers", "Admin");
            }
            catch (EntityNotFoundException ex)
            {
                this.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = ex.Message;
                return View("Error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> ChangeRoleToAdministrator([FromRoute] string id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
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
        public async Task<IActionResult> ChangeRoleToAdministratorConfirmed([FromRoute] string id)
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
