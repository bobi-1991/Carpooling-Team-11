using Carpooling.BusinessLayer.Services.Contracts;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
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
            //ToDo when we have user roles.
            throw new NotImplementedException();
        }

        public Feedback Delete(int id, User user)
        {
            //ToDo when we have user roles.
            throw new NotImplementedException();
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
            //ToDo when we have user roles.
            throw new NotImplementedException();
        }
    }
}
