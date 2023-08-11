namespace Carpooling.BusinessLayer.Services.Contracts
{
    public interface IMapService
    {
        Task<(double travelDistance, double travelDuration)> GetDirection(string originCity, string destinationCity, string country, DateTime departureTime);
    }
}