using CarPooling.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Data.Repositories.Contracts
{
    public interface IFeedbackRepository
    {
        List<Feedback> GetAll();
        Feedback GetById(int id);
        Feedback Create(Feedback feedback);
        Feedback Update(int id, Feedback feedback);
        Feedback Delete(int id);
    }
}
