using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public interface IImageRepository
    {
        Task<ImageUpload> Upload(ImageUpload image);
    }
}
