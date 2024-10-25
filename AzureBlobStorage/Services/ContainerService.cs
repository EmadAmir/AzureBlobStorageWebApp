
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureBlobStorage.Models;
using AzureBlobStorage.ViewModel;

namespace AzureBlobStorage.Services
{
    public class ContainerService(BlobServiceClient _blobService) : IContainerService
    {


        public async Task CreateContainer(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobService.GetBlobContainerClient(containerName);

            await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);
        }

        public async Task DeleteContainer(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobService.GetBlobContainerClient(containerName);

            await blobContainerClient.DeleteIfExistsAsync();
        }

        public async Task<List<string>> GetAllContainer()
        {
            List<string> containerList = [];

            await foreach (BlobContainerItem blobContainerItem in _blobService.GetBlobContainersAsync()) 
            { 
                containerList.Add(blobContainerItem.Name);
            }
            return containerList;
        }

        public async Task<List<ContainerAndBlobViewModel>> GetAllContainerAndBlob()
        {
            List<ContainerAndBlobViewModel> containerAndBlobs = new List<ContainerAndBlobViewModel>();

            //ContainerAndBlob containerAndBlob= new ContainerAndBlob();
            //containerAndBlob.AccountName = _blobService.AccountName;

            await foreach (BlobContainerItem blobContainerItem in _blobService.GetBlobContainersAsync()) 
            {
                BlobContainerClient blobContainerClient = _blobService.GetBlobContainerClient(blobContainerItem.Name);

                ContainerAndBlobViewModel containerAndBlob = new ContainerAndBlobViewModel
                {
                    AccountName = _blobService.AccountName,
                    Container = new List<Container>()
                };

                Container container = new Container
                {
                    Name = blobContainerClient.Name,
                    Blobs = new List<Blob>()
                };

                await foreach (BlobItem blobItem in blobContainerClient.GetBlobsAsync())
                {
                    var blobClient = blobContainerClient.GetBlobClient(blobItem.Name);
                    BlobProperties blobProperties = await blobClient.GetPropertiesAsync();

                    string title = blobProperties.Metadata.ContainsKey("title") ? blobProperties.Metadata["title"] : null;

                    string comment = blobProperties.Metadata.TryGetValue("Comment", out string? value) ? value : null;

                    container.Blobs.Add(new Blob
                    {
                        BlobName = blobItem.Name,
                        Title = title,
                        Comment = comment
                    });

                }

                containerAndBlob.Container.Add(container);

                containerAndBlobs.Add(containerAndBlob);
            }

            return containerAndBlobs;

        }
    }
}
