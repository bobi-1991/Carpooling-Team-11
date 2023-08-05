using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.BusinessLayer.Services.Contracts
{
    public interface IFileUploadService
    {
        string UploadFile(IFormFile uploadedImage);
        string SetUniqueImagePathForBar(IFormFile uploadedImage);
        string SetUniqueImagePathForCocktail(IFormFile uploadedImage);
    }
}
