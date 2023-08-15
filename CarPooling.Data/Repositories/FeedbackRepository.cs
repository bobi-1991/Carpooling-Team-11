using CarPooling.Data.Data;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarPooling.Data.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly CarPoolingDbContext _context;
        private readonly IUserRepository _userRepository;
        public FeedbackRepository(CarPoolingDbContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        public async Task<Feedback> CreateAsync(Feedback feedback)
        {
            feedback.CreatedOn = DateTime.Now;
            var userToMakeFeedback = await _userRepository.GetByIdAsync(feedback.GiverId);
            var userToReceiveFeedback = await _userRepository.GetByIdAsync(feedback.ReceiverId);
            
            userToMakeFeedback.GivenFeedbacks.Add(feedback);
            userToReceiveFeedback.ReceivedFeedbacks.Add(feedback);

            await _context.Feedbacks.AddAsync(feedback);
            await _context.SaveChangesAsync();

            return feedback;
        }

        public async Task<Feedback> CreateFeedbackForDriverAsync(Feedback feedback)
        {
            feedback.CreatedOn = DateTime.Now;

            var passengerGivingFeedback = await _userRepository.GetByIdAsync(feedback.GiverId);
            var driverToGetFeedback = await _userRepository.GetByIdAsync(feedback.ReceiverId);

            driverToGetFeedback.GivenFeedbacks.Add(feedback);

            await _context.Feedbacks.AddAsync(feedback);

            if (passengerGivingFeedback.ReceivedFeedbacks.Contains(feedback))
            {
                passengerGivingFeedback.ReceivedFeedbacks.Remove(feedback);
            }

            await _context.SaveChangesAsync();
            return feedback;
        }
        public async Task<Feedback> DeleteAsync(int id)
        {
            Feedback feedbackToDelete = await GetByIdAsync(id);

            feedbackToDelete.IsDeleted = true;
            feedbackToDelete.DeletedOn = DateTime.Now;

            await _context.SaveChangesAsync();

            return feedbackToDelete;
        }

        public async Task<List<Feedback>> GetAllAsync()
        {
            return await _context.Feedbacks
                .Where(c => c.IsDeleted == false)
                .Include(x => x.Receiver)
                .Include(x => x.Giver)
                .ToListAsync();
        }

        public async Task<Feedback> GetByIdAsync(int id)
        {
            Feedback feedback = await _context.Feedbacks
                .Where(c => c.IsDeleted == false)
                .FirstOrDefaultAsync(f => f.Id == id);

            return feedback ?? throw new EntityNotFoundException($"Feedback not found with id: {id}!");
        }

        public async Task<Feedback> UpdateAsync(int id, Feedback feedback)
        {
            Feedback feedbackToUpdate = await GetByIdAsync(id);
            feedbackToUpdate.Comment = feedback.Comment;
            feedbackToUpdate.Rating = feedback.Rating;
            feedbackToUpdate.UpdatedOn = DateTime.Now;

            _context.Update(feedbackToUpdate);
            await _context.SaveChangesAsync();

            return feedbackToUpdate;
        }
    }
}