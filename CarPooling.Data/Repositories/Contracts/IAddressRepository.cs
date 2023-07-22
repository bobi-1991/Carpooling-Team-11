using CarPooling.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Data.Repositories.Contracts
{
    public interface IAddressRepository
    {
        List<Address> GetAll();
        Address GetById(int id);
        Address GetByName(string name);
        Address Create (Address address);
        Address Update (int id, Address address);
        Address Delete (int id);
    }
}
