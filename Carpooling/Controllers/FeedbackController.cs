using AutoMapper;
using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using Carpooling.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Carpooling.BusinessLayer.Services.Contracts;
using CarPooling.Data.Data;
using Microsoft.EntityFrameworkCore;
using Carpooling.Service.Dto_s.Responses;

namespace Carpooling.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IUserService userService;
        //private readonly ICarService carService;
        private readonly IFeedbackService feedbackService;
        //private readonly ITravelService travelService;
        private readonly UserManager<User> userManager;
        private readonly ITravelService travelService;
        //private readonly IMapper mapper;
        //private readonly CarPoolingDbContext dbContext;
        //private readonly IMapService mapService;

        public FeedbackController(UserManager<User> userManager, IFeedbackService feedbackService, IUserService userService, ITravelService travelService)
        {
            this.userManager = userManager;
            this.feedbackService = feedbackService;
            this.userService = userService;
            this.travelService = travelService;
        }


        //public string AuthorUsername { get; set; }
        //public string DriverUsername { get; set; }
        //public string Comment { get; set; }
        //public int Rating { get; set; }
        //public int TravelID { get; set; }


        [HttpGet]
        public async Task<IActionResult> Create(string participantId, int travelId)
        {
            //var user = await userManager.Users.Include(c => c.Cars)
            //    .SingleAsync(x => x.UserName.Equals(User.Identity.Name));
            //var participant = await this.userService.GetUserByIdAsync(participantId);

            var feedbackModel = new FeedbackViewModel();
            feedbackModel.ParticipantId = participantId;
            feedbackModel.TravelId = travelId;


            return View(feedbackModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(FeedbackViewModel feedbackModel, string participantId, int travelId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Challenge();
            }
            if (!this.ModelState.IsValid)
            {
                return this.View(feedbackModel);
            }
            try
            {
                var user = await userManager.Users.Include(c => c.Cars)
                  .SingleAsync(x => x.UserName.Equals(User.Identity.Name));

                var travel = await travelService.GetTravelAsync(travelId);
                var participant = await this.userService.GetUserByIdAsync(participantId);
                if (travel.DriverId.Equals(user.Id))
                {
                    var feedback = new Feedback
                    {
                        Rating = feedbackModel.Rating,
                        Comment = feedbackModel.Comment,
                        DriverId = user.Id,
                        PassengerId = participant.Id,
                        TravelId = travelId
                    };
                    _ = await this.feedbackService.CreateMVCAsync(feedback);
                }
                else
                {
                    var feedback = new Feedback
                    {
                        Rating = feedbackModel.Rating,
                        Comment = feedbackModel.Comment,
                        DriverId = participant.Id,
                        PassengerId = user.Id,
                        TravelId = travelId
                    };
                    _ = await this.feedbackService.CreateMVCAsync(feedback);
                }
    
                return RedirectToAction("MyTravels", "Personal");
            }
            catch (UnauthorizedOperationException ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                this.ViewData["ErrorMessage"] = ex.Message;
                return View("Error");
            }
            catch (EntityUnauthorizatedException ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                this.ViewData["ErrorMessage"] = ex.Message;
                return View("Error");
            }
            catch (EntityNotFoundException ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = ex.Message;
                return View("Error");
            }

        }
    }

    //[HttpPost]
    //public async Task<IActionResult> CreateFeedback(int travelId, string participantId)
    //{
    //    if (!User.Identity.IsAuthenticated)
    //    {
    //        return Challenge();
    //    }

    //    if (!this.ModelState.IsValid)
    //    {
    //        return this.RedirectToAction("MyTravels", "Personal");
    //    }
    //    try
    //    {
    //        var user = await userManager.Users.Include(c => c.Cars)
    //            .SingleAsync(x => x.UserName.Equals(User.Identity.Name));

    //      //  this.feedbackService.CreateAsync();


    //        return RedirectToAction("MyTravels", "Personal");

    //    }
    //    catch (UnauthorizedOperationException ex)
    //    {
    //        HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
    //        this.ViewData["ErrorMessage"] = ex.Message;
    //        return View("Error");
    //    }
    //    catch (EntityUnauthorizatedException ex)
    //    {
    //        HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
    //        this.ViewData["ErrorMessage"] = ex.Message;
    //        return View("Error");
    //    }
    //    catch (EntityNotFoundException ex)
    //    {
    //        HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
    //        this.ViewData["ErrorMessage"] = ex.Message;
    //        return View("Error");
    //    }
    //}

}

