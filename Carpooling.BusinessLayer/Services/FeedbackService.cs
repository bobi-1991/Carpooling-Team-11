using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services.Contracts;
using CarPooling.Data.Constants;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.BusinessLayer.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly UserManager<User> _userManager;
        public FeedbackService(IFeedbackRepository feedbackRepository, UserManager<User> userManager)
        {
            _feedbackRepository = feedbackRepository;
            _userManager = userManager;
        }
        public Feedback Create(Feedback feedback, User user)
        {
            //ToDo when we have user roles.
            if (user.IsBlocked == true)
            {
                throw new UnauthorizedOperationException("Only non-blocked user can make feedbacks!");
            }

            feedback.Passenger = user;
            feedback.PassengerId = user.Id;

            return _feedbackRepository.Create(feedback);
        }

        public Feedback Delete(int id, User user)
        {
            Feedback feedbackToDelete = GetById(id);
            var roles = _userManager.GetRolesAsync(user).ToString();
            if (user.IsBlocked == true && feedbackToDelete.PassengerId != user.Id && roles.Equals("Admin"))
            {
                throw new UnauthorizedOperationException("You do not have permission to delete this feedback!");
            }
            feedbackToDelete = _feedbackRepository.Delete(id);
            return feedbackToDelete;
        }

        public List<Feedback> GetAll()
        {
            return _feedbackRepository.GetAll();
        }

        public Feedback GetById(int id)
        {
            return _feedbackRepository.GetById(id);
        }

        public Feedback Update(int id, User user, Feedback feedback)
        {
            Feedback feedbackToUpdate = GetById(id);
            var roles = _userManager.GetRolesAsync(user).ToString();
            if (user.IsBlocked == true && feedbackToUpdate.PassengerId != user.Id && roles.Equals("Admin"))
            {
                throw new UnauthorizedOperationException("You do not have permission to update this feedback!");
            }
            feedbackToUpdate = _feedbackRepository.Update(id, feedback);
            return feedbackToUpdate;
        }
    }
}
