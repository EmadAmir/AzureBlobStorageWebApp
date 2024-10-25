using AzureBlobStorage.ViewModel;

namespace AzureBlobStorage.Services
{
    public interface IContainerService
    {

        Task<List<ContainerAndBlobViewModel>> GetAllContainerAndBlob();
        Task<List<string>> GetAllContainer();
        Task CreateContainer(string containerName);
        Task DeleteContainer(string containerName);
    }
}
