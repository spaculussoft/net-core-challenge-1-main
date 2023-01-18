using FixWithCustomSerialization.Controllers;

namespace FixWithCustomSerialization.Services
{
    public interface IDbService
    {
        Task<MediaFile> GetAsync(string name);

        Task CreateAsync(MediaFile model);
    }
}
