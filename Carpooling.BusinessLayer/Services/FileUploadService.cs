using Carpooling.BusinessLayer.Services.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.BusinessLayer.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IHostingEnvironment _environment;

        public FileUploadService(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        public string UploadFile(IFormFile uploadedImage)
        {

            if (uploadedImage != null)
            {
                string uniqueFileName = GetUniqueFileName(uploadedImage.FileName);
                var uploads = Path.Combine(_environment.WebRootPath, "images");
                var filePath = Path.Combine(uploads, uniqueFileName);
                uploadedImage.CopyTo(new FileStream(filePath, FileMode.Create));
                return "/images/" + uniqueFileName;
            }
            else
            {
                throw new InvalidOperationException("No image is uploaded");
            }

        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }

        public string SetUniqueImagePathForBar(IFormFile uploadedImage)
        {
            string uniqueFileNamePath;
            if (uploadedImage != null)
                uniqueFileNamePath = UploadFile(uploadedImage);
            else
                uniqueFileNamePath = "/images/defaultBarImage.jpg";

            return uniqueFileNamePath;
        }


        public string SetUniqueImagePathForCocktail(IFormFile uploadedImage)
        {
            string uniqueFileNamePath;
            if (uploadedImage != null)
                uniqueFileNamePath = UploadFile(uploadedImage);
            else
                uniqueFileNamePath = "/images/defaultImageCocktail.jpg";

            return uniqueFileNamePath;
        }
    }
}
