using CarPooling.Data.Models;

namespace CarPooling.Data.Repositories.Contracts
{
    public interface IAddressRepository
    {
        List<Address> GetAll();
        Address GetById(int id);
        Address Create(Address address);
        Address Update(int id, Address address);
        Address Delete(int id);
    }  
}
