using CarPooling.Data.Data;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CarPooling.Data.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly CarPoolingDbContext _context;
        public AddressRepository(CarPoolingDbContext context)
        {
            _context = context;
        }
        public Address Create(Address address)
        {
            _context.Addresses.Add(address);
            _context.SaveChanges();
            return address;
        }

        public Address Delete(int id)
        {
            Address addressToRemove=GetById(id);
            addressToRemove.IsDeleted=true;
            _context.SaveChanges();
            return addressToRemove;
        }


        public List<Address> GetAll()
        {
            return _context.Addresses
                .ToList();
        }

        public Address GetById(int id)
        {
            Address address = _context.Addresses.Where(a=>a.Id == id)
                .FirstOrDefault();
            return address ?? throw new EntityNotFoundException($"Could not find an address with id: {id}!");
        }

        public Address Update(int id, Address address)
        {
            Address addressToUpdate = GetById(id);
            _context.Update(addressToUpdate);
            _context.SaveChanges();
            return addressToUpdate;
        }
    }
}
