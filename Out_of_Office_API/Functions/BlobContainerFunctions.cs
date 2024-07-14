using Azure.Storage.Blobs;
using Out_of_Office_API.Data;

namespace Out_of_Office_API.Functions
{
    public static class BlobContainerFunctions
    {
        public static async Task<string> UploadImage(BlobContainerClient container, IFormFile image)
        {
            var blob = container.GetBlobClient($"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}");
            await blob.UploadAsync(image.OpenReadStream());
            return blob.Uri.AbsoluteUri;
        }
        public static async void DeleteImage(BlobContainerClient container, string ImagePath)
        {
            var blob = container.GetBlobClient(Path.GetFileName(ImagePath));
            await blob.DeleteIfExistsAsync();
        }
    }
}
