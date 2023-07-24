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

        public List<Address> FilterByAddressAndSort(string addressName, string sortBy)
        {
            IQueryable<Address> addresses = _context.Addresses
                .Where(a => a.Name == addressName)
                .Include(a => a.AddressNumber)
                .Include(a => a.City);
            switch (sortBy)
            {
                case "city":
                    addresses = addresses.OrderBy(a=>a.City); 
                    break;
                case "number":
                    addresses = addresses.OrderBy(a => a.AddressNumber);
                        break;
                default:
                    addresses=addresses.OrderBy(a=>a.AddressNumber);
                    break;
            }
            return addresses.ToList();
        }

        public List<Address> GetAll()
        {
            return _context.Addresses
                .Include(p => p.City)
                .Include(p => p.Users)
                .ToList();
        }

        public Address GetById(int id)
        {
            Address address = _context.Addresses.Where(a=>a.Id == id)
                .Include(p => p.City)
                .Include(p => p.Users)
                .FirstOrDefault();
            return address ?? throw new EntityNotFoundException($"Could not find an address with id: {id}!");
        }

        public Address GetByName(string name)
        {
            Address address = _context.Addresses.Where(a => a.Name == name)
                .Include(p => p.City)
                .Include(p => p.Users)
                .FirstOrDefault();
            return address ?? throw new EntityNotFoundException($"Could not find an address with name: {name}!");
        }

        public Address Update(int id, Address address)
        {
            Address addressToUpdate = GetById(id);
            addressToUpdate.Name = address.Name;
            addressToUpdate.City = address.City;
            _context.Update(addressToUpdate);
            _context.SaveChanges();
            return addressToUpdate;
        }
    }
}
