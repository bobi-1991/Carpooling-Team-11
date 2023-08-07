using AutoMapper;
using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.Models;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using CarPooling.Data.Models.Pagination;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Carpooling.Controllers
{
    public class TravelsController : Controller
    {
        private readonly IUserService userService;
        private readonly ICarService carService;
        private readonly IFeedbackService feedbackService;
        private readonly ITravelService travelService;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        public TravelsController(IUserService userService, ICarService carService, IFeedbackService feedbackService, ITravelService travelService, UserManager<User> userManager, IMapper mapper)
        {
            this.userService = userService;
            this.carService = carService;
            this.feedbackService = feedbackService;
            this.travelService = travelService;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] TravelQueryParameters searchQuery)
        {
            PaginatedList<Travel> travels = await this.travelService.FilterBy(searchQuery);

            return this.View(travels);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var travel = await this.travelService.GetByIdAsync(id);

                return this.View(travel);
            }
            catch (EntityNotFoundException e)
            {
                this.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = e.Message;

                return this.View("Error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var travelViewModel = new TravelViewModel();
            return this.View(travelViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(TravelViewModel travelViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(travelViewModel);
            }
            try
            {
                var user = await userManager.GetUserAsync(User);
                var travel = mapper.Map<Travel>(travelViewModel);
                var createdTravel = travelService.CreateTravelForMVCAsync(user, travel);
                return this.RedirectToAction("Details", "Travels", new { id = createdTravel.Id });
            }
            catch (UnauthorizedOperationException ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                this.ViewData["ErrorMessage"] = ex.Message;
                return View(travelViewModel);
            }
            catch(EntityUnauthorizatedException ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                this.ViewData["ErrorMessage"] = ex.Message;
                return View(travelViewModel);
            }
            catch(EntityNotFoundException ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = ex.Message;
                return View(travelViewModel);
            }
        }
        
    }
}
