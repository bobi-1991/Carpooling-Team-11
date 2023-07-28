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
        Task<List<Feedback>> GetAllAsync();
        Task<Feedback> GetByIdAsync(int id);
        Task<Feedback> CreateAsync(Feedback feedback);
        Task<Feedback> UpdateAsync(int id, Feedback feedback);
        Task<Feedback> DeleteAsync(int id);
    }
}
