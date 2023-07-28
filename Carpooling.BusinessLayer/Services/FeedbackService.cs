﻿using Carpooling.BusinessLayer.Exceptions;
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

        public async Task<Feedback> CreateAsync(Feedback feedback, User user)
        {
            // ToDo: Implement user roles and check if user can make feedbacks.
            if (user.IsBlocked )
            {
                throw new UnauthorizedOperationException("Only non-blocked user can make feedbacks!");
            }

            feedback.Passenger = user;
            feedback.PassengerId = user.Id;

            return await _feedbackRepository.CreateAsync(feedback);
        }

        public async Task<Feedback> DeleteAsync(int id, User user)
        {
            Feedback feedbackToDelete = await GetByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);

            if (user.IsBlocked && feedbackToDelete.PassengerId != user.Id && roles.Contains("Administrator"))
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

            if (user.IsBlocked && feedbackToUpdate.PassengerId != user.Id && roles.Contains("Administrator"))
            {
                throw new UnauthorizedOperationException("You do not have permission to update this feedback!");
            }

            feedbackToUpdate = await _feedbackRepository.UpdateAsync(id, feedback);
            return feedbackToUpdate;
        }
    }
}
