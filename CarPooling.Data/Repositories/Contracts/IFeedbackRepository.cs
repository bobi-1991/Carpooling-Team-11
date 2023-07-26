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
        Feedback Create(Feedback city);
        Feedback Update(int id, Feedback city);
        Feedback Delete(int id);
    }
}
