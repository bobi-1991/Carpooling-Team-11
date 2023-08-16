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
        Task<List<Feedback>> GetAllAsync();
        Task<Feedback> GetByIdAsync(int id);
        Task<Feedback> CreateAsync(Feedback feedback, User user);
        Task<Feedback> UpdateAsync(int id, User user, Feedback feedback);
        Task<Feedback> DeleteAsync(int id, User user);
    }
}
