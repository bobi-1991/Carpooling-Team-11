using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services.Contracts;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Carpooling.BusinessLayer.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly UserManager<User> _userManager;
        private readonly ITravelRepository travelRepository;

        public FeedbackService(IFeedbackRepository feedbackRepository, 
            UserManager<User> userManager, 
            ITravelRepository travelRepository)
        {
            _feedbackRepository = feedbackRepository;
            _userManager = userManager;
            this.travelRepository = travelRepository;
        }

        public async Task<Feedback> CreateAsync(Feedback feedback, User user)
        {
            var currentTravel = await this.travelRepository.GetByIdAsync(feedback.TravelId);
            
            if (user.IsBlocked)
            {
                throw new UnauthorizedOperationException("Only non-blocked user can make feedbacks!");
            }
            if (currentTravel.IsCompleted == false)
            {
                throw new UnauthorizedOperationException("Feedback can only be left on completed trips!");
            }

            feedback.Passenger = user;
            feedback.PassengerId = user.Id;

            return await _feedbackRepository.CreateAsync(feedback);
        }

        public async Task<Feedback> DeleteAsync(int id, User user)
        {
            Feedback feedbackToDelete = await GetByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);

            if (user.IsBlocked)
            {
                throw new UnauthorizedOperationException("You do not have permission to delete this feedback!");
            }

            if (feedbackToDelete.PassengerId != user.Id && !roles.Contains("Administrator"))
            {
                throw new UnauthorizedOperationException("You do not have permission to delete this feedback!");
            }

            feedbackToDelete = await _feedbackRepository.DeleteAsync(id);
            return feedbackToDelete;
        }

        public async Task<List<Feedback>> GetAllAsync()
        {
            return await _feedbackRepository.GetAllAsync();
        }

        public async Task<Feedback> GetByIdAsync(int id)
        {
            return await _feedbackRepository.GetByIdAsync(id);
        }

        public async Task<Feedback> UpdateAsync(int id, User user, Feedback feedback)
        {
            Feedback feedbackToUpdate = await GetByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);

            if (user.IsBlocked)
            {
                throw new UnauthorizedOperationException("You do not have permission to delete this feedback!");
            }

            if (feedbackToUpdate.PassengerId != user.Id && !roles.Contains("Administrator"))
            {
                throw new UnauthorizedOperationException("You do not have permission to delete this feedback!");
            }

            feedbackToUpdate = await _feedbackRepository.UpdateAsync(id, feedback);
            return feedbackToUpdate;
        }
    }
}
