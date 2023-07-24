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
    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepository;
        public CityService(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }
        public City Create(City city, User user)
        {
            //ToDo when we have user roles.
            throw new NotImplementedException();
        }

        public City Delete(int id, User user)
        {
            //ToDo when we have user roles.
            throw new NotImplementedException();
        }

        public List<City> GetAll()
        {
            return _cityRepository.GetAll();
        }

        public City GetById(int id)
        {
            return _cityRepository.GetById(id);
        }

        public City GetByName(string name)
        {
            return _cityRepository.GetByName(name);
        }

        public City Update(int id, User user, City city)
        {
            //ToDo when we have user roles.
            throw new NotImplementedException();
        }
    }
}
