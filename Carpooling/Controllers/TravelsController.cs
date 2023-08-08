using AutoMapper;
using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.Models;
using CarPooling.Data.Data;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using CarPooling.Data.Models.Pagination;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        private readonly CarPoolingDbContext dbContext;
        public TravelsController(IUserService userService, ICarService carService, IFeedbackService feedbackService, 
            ITravelService travelService, UserManager<User> userManager, IMapper mapper, CarPoolingDbContext dbContext)
        {
            this.userService = userService;
            this.carService = carService;
            this.feedbackService = feedbackService;
            this.travelService = travelService;
            this.userManager = userManager;
            this.mapper = mapper;
            this.dbContext = dbContext;
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
            if (!User.Identity.IsAuthenticated)
            {
                return Challenge(); 
            }
            if (!this.ModelState.IsValid)
            {
                return this.View(travelViewModel);
            }
            try
            {
                var user = await userManager.Users.Include(c => c.Cars)
                    .SingleAsync(x=>x.UserName.Equals(User.Identity.Name));
                var travel = mapper.Map<Travel>(travelViewModel);
                var createdTravel = await travelService.CreateTravelForMVCAsync(user, travel);
                return this.RedirectToAction("Details", "Travels", new {id = travel.Id});
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
                return View("Error");
            }
        }
        
    }
}
