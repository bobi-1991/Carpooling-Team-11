using Carpooling.Service.Dto_s.Requests;
using CarPooling.Data.Models;

namespace Carpooling.Authorization
{
    public interface IAuthorization
    {
        Task<UserRequest> ValidateCredentialAsync(string credentials);
    }
}
