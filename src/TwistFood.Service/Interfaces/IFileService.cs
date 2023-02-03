using Microsoft.AspNetCore.Http;

namespace TwistFood.Service.Interfaces
{
    public interface IFileService
    {
        public Task<string> SaveImageAsync(IFormFile image);
    }
}
