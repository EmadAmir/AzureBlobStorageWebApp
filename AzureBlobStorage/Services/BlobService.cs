
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using AzureBlobStorage.Models;

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

        public async Task<List<Blob>> GetAllBlobsWithUri(string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobs = containerClient.GetBlobsAsync();
            var blobsList = new List<Blob>();
            await foreach (var blob in blobs) 
            { 
                var blobClient = containerClient.GetBlobClient(blob.Name);

                BlobProperties blobProperties = await blobClient.GetPropertiesAsync();

                string? title = blobProperties.Metadata.ContainsKey("title") ? blobProperties.Metadata["title"] : null;

                string? comment = blobProperties.Metadata.TryGetValue("Comment", out string? value) ? value : null;

                Blob blobIndividual = new Blob()
                {
                    Uri = blobClient.Uri.AbsoluteUri,
                    Title = title,
                    Comment = comment
                };

                blobsList.Add(blobIndividual);

            }

            return blobsList;

        }

        public async Task<string> GetBlob(string name, string containerName)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                
             var blobClient =  containerClient.GetBlobClient(name);

            return blobClient.Uri.AbsoluteUri;
        }

        public async Task<bool> UploadBlob(string name, IFormFile file, string containerName , Blob blob)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            var blobClient = blobContainerClient.GetBlobClient(name);

            var httpHeaders = new BlobHttpHeaders()
            {
                ContentType = file.ContentType
            };

            IDictionary<string, string> metadata = new Dictionary<string, string>();

            metadata.Add("title",blob.Title);
            metadata.Add("comment", blob.Comment);

            var result = await blobClient.UploadAsync(file.OpenReadStream(), httpHeaders,metadata);

            if (result != null)
            { 
                return true;
            }
            return false;
        }
    }
}
