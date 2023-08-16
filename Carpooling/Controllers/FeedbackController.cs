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
        private readonly IFeedbackService feedbackService;
        private readonly UserManager<User> userManager;
        private readonly ITravelService travelService;
        public FeedbackController(UserManager<User> userManager, IFeedbackService feedbackService, IUserService userService, ITravelService travelService)
        {
            this.userManager = userManager;
            this.feedbackService = feedbackService;
            this.userService = userService;
            this.travelService = travelService;
        }


        [HttpGet]
        public async Task<IActionResult> Create(string participantId, int travelId)
        {
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

                var feedback = new Feedback
                {
                    Rating = feedbackModel.Rating,
                    Comment = feedbackModel.Comment,
                    ReceiverId = participant.Id,
                    GiverId = user.Id,
                    TravelId = travelId
                };

                var feedbackGiver = await userManager.FindByIdAsync(user.Id);
                _ = await this.feedbackService.CreateAsync(feedback, feedbackGiver);

                return RedirectToAction("MyTravels", "Travels");
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
}

