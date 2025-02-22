namespace sp311_mvc_project.Services.Image
{
    public interface IImageService
    {
        Task<string?> SaveImageAsync(IFormFile image, string path);
        void DeleteImage(string path);
    }
}
