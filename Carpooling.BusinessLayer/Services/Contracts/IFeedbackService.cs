using CarPooling.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.BusinessLayer.Services.Contracts
{
    public interface IFeedbackService
    {
        List<Feedback> GetAll();
        Feedback GetById(int id);
        Feedback Create(Feedback feedback, User user);
        Feedback Update(int id, User user, Feedback feedback);
        Feedback Delete(int id, User user);
    }
}
