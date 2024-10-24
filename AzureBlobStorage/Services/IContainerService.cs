using AzureBlobStorage.Models;

namespace AzureBlobStorage.Services
{
    public interface IContainerService
    {

        Task<List<ContainerAndBlob>> GetAllContainerAndBlob();
        Task<List<string>> GetAllContainer();
        Task CreateContainer(string containerName);
        Task DeleteContainer(string containerName);
    }
}
