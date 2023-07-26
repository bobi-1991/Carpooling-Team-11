using CarPooling.Data.Models;

namespace Carpooling.BusinessLayer.Services.Contracts
{
    public interface IAddressService
    {
        List<Address> GetAll();
        Address GetById(int id);
        Address Create(Address address, User user);
        Address Update(int id, User user, Address address);
        Address Delete(int id, User user);
    }
}
