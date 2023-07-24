using CarPooling.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.BusinessLayer.Services.Contracts
{
    public interface IAddressService
    {
        List<Address> GetAll();
        List<Address> FilterByAddressAndSort(string addressName, string sortBy);
        Address GetById(int id);
        Address GetByName(string title);
        Address Create(Address address, User user);
        Address Update(int id, User user, Address address);
        Address Delete(int id, User user);
    }
}
