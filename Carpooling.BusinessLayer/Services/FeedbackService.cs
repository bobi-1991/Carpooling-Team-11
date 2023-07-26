using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services.Contracts;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.BusinessLayer.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        public FeedbackService(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }
        public Feedback Create(Feedback feedback, User user)
        {
            if (user.IsBlocked == true)
            {
                throw new UnauthorizedOperationException("Only non-banned users can create feedbacks!");
            }
            feedback.Author = user;
            feedback.AuthorId= user.Id;
            return _feedbackRepository.Create(feedback);
        }

        public Feedback Delete(int id, User user)
        {
            Feedback feedbackToDelete = GetById(id);
            if(feedbackToDelete.AuthorId != user.Id && user.IsAdmin == false && user.IsBlocked == true)
            {
                throw new UnauthorizedOperationException("Only owner of the feedback or admin can delete!");
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
            Feedback feedbackToUpdate= GetById(id);
            if (feedbackToUpdate.AuthorId != user.Id && user.IsBlocked == true && user.IsAdmin == false)
            {
                throw new UnauthorizedOperationException("Only owner of the feedback or admin can update!");
            }
            feedbackToUpdate = _feedbackRepository.Update(id, feedback);
            return feedbackToUpdate;
        }
    }
}
