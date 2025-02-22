namespace sp311_mvc_project.Services.Image
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public void DeleteImage(string path)
        {
            string root = _webHostEnvironment.WebRootPath;
            string imagePath = Path.Combine(root, path);

            if(File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }

        public async Task<string?> SaveImageAsync(IFormFile image, string path)
        {
            try
            {
                var types = image.ContentType.Split("/");

                if (types[0] != "image")
                {
                    return null;
                }

                var imageName = $"{Guid.NewGuid()}.{types[1]}";
                var imagesPath = Path.Combine(_webHostEnvironment.WebRootPath, path);
                var imagePath = Path.Combine(imagesPath, imageName);

                using (var fileStream = File.Create(imagePath))
                {
                    using (var formStream = image.OpenReadStream())
                    {
                        await formStream.CopyToAsync(fileStream);
                    }
                }

                return imageName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
