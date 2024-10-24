
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;

namespace AzureBlobStorage.Services
{
    public class BlobService(BlobServiceClient _blobServiceClient) : IBlobService
    {
        public async Task<bool> DeleteBlob(string name, string containerName)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            BlobClient blobClient = blobContainerClient.GetBlobClient(name); // It creates a blob client object to upload /download/delete/access metadata for the blob

            bool deleted = await blobClient.DeleteIfExistsAsync();

            return deleted;

        }

        public async Task<List<string>> GetAllBlobs(string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            if (containerClient == null) return null;

            var blobsList =  containerClient.GetBlobsAsync();

            List<string> result = [];

           await  foreach (var blob in blobsList) {
                
                result.Add(blob.Name);
            }
            return result;

        }

        public async Task<string> GetBlob(string name, string containerName)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                
             var blobClient =  containerClient.GetBlobClient(name);

            return blobClient.Uri.AbsoluteUri;
        }

        public async Task<bool> UploadBlob(string name, IFormFile file, string containerName)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            var blobClient = blobContainerClient.GetBlobClient(name);

            var httpHeaders = new BlobHttpHeaders()
            {
                ContentType = file.ContentType
            };

            var result = await blobClient.UploadAsync(file.OpenReadStream(), httpHeaders);

            if (result != null)
            { 
                return true;
            }
            return false;
        }
    }
}
