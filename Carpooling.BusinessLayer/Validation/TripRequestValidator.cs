﻿using Carpooling.BusinessLayer.Validation.Contracts;
using Carpooling.Service.Dto_s.Requests;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories;
using CarPooling.Data.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.BusinessLayer.Validation
{
    public class TripRequestValidator : ITripRequestValidator
    {
        private readonly ITripRequestRepository tripRequestRepository;

        public TripRequestValidator(ITripRequestRepository tripRequestRepository)
        {
            this.tripRequestRepository = tripRequestRepository;
        }

        public async Task<bool> ValidateIfPassengerAlreadyCreateTripRequest(TripRequestRequest tripRequest)
        {
            var trips = await this.tripRequestRepository.GetAllAsync();

            if (trips.Any(x => x.PassengerId.Equals(tripRequest.PassengerId) && x.TravelId.Equals(tripRequest.TravelId)))
            {
                return true;
            }

            return false;
        }
    }
}
