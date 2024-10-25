using AzureBlobStorage.Models;

namespace AzureBlobStorage.ViewModel
{
    public class ContainerAndBlobViewModel
    {
        public string AccountName { get; set; }
        public List<Container> Container { get; set; }

    }
}
