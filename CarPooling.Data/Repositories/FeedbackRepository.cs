using CarPooling.Data.Data;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Data.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly CarPoolingDbContext _context;
        public FeedbackRepository(CarPoolingDbContext context)
        {
            _context = context;
        }
        public Feedback Create(Feedback feedback)
        {
            feedback.CreatedOn = DateTime.Now;

            _context.Feedbacks.Add(feedback);
            _context.SaveChanges();

            return feedback;
        }

        public Feedback Delete(int id)
        {
            Feedback feedbackToDelete = GetById(id);

            feedbackToDelete.IsDeleted = true;
            feedbackToDelete.DeletedOn = DateTime.Now;

            _context.SaveChanges();

            return feedbackToDelete;
        }

        public List<Feedback> GetAll()
        {
            return _context.Feedbacks
                .Where(c => c.IsDeleted == false)
                .Include(f => f.Passenger)
                .Include(f => f.Comment)
                .Include(f => f.Rating)
                .Include(f => f.Driver)
                .Include(f => f.CreatedOn)
                .Include(f => f.DeletedOn)
                .Include(f => f.UpdatedOn)
                .ToList();
        }

        public Feedback GetById(int id)
        {
            Feedback feedback = _context.Feedbacks
                .Where(c => c.IsDeleted == false)
                .Include(f => f.Passenger)
                .Include(f => f.Rating)
                .Include(f => f.Comment)
                .Include(f => f.Driver)
                .Include(f => f.CreatedOn)
                .Include(f => f.DeletedOn)
                .Include(f => f.UpdatedOn)
                .FirstOrDefault(f => f.Id == id);

            return feedback ?? throw new EntityNotFoundException($"Feedback not found with id: {id}!");
        }
        public Feedback Update(int id, Feedback feedback)
        {
            Feedback feedbackToUpdate = GetById(id);
            feedbackToUpdate.Comment = feedback.Comment;
            feedbackToUpdate.Rating = feedback.Rating;
            feedbackToUpdate.UpdatedOn = DateTime.Now;

            _context.Update(feedbackToUpdate);
            _context.SaveChanges();

            return feedbackToUpdate;
        }
    }
}
