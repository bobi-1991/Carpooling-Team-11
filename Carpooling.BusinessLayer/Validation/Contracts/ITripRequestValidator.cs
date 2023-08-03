using Carpooling.Service.Dto_s.Requests;
using CarPooling.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.BusinessLayer.Validation.Contracts
{
    public interface ITripRequestValidator
    {
        Task<bool> ValidateIfPassengerAlreadyCreateTripRequest(TripRequestRequest tripRequest);
        Task<string> ValidateStatusOfTripRequest(TripRequest tripRequest, string answer);

    }
}
