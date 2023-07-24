using CarPooling.Data.Models;

namespace CarPooling.Data.Repositories.Contracts
{
    public interface IAddressRepository
    {
        List<Address> GetAll();
        List<Address> FilterByAddressAndSort(string addressName, string sortBy);
        Address GetById(int id);
        Address GetByName(string name);
        Address Create(Address address);
        Address Update(int id, Address address);
        Address Delete(int id);
    }  
}
