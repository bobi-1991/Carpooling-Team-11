using Carpooling.Service.Dto_s.Requests;
using CarPooling.Data.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.Service.Dto_s.Responses
{
    public class UserResponse
    {
        public UserResponse()
        {
        }
        public UserResponse(string firstName, string lastName, string username, string email, decimal avgRating)
        {
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Email = email;
            AverageRating = avgRating;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public decimal AverageRating { get; set; }

        // Optional? |

        //public ICollection<Feedback> Feedbacks { get; set; }
        //public ICollection<Travel> TravelHistory { get; set; }
        //public ICollection<Car> Cars { get; set; }

    }
}
