using AzureBlobStorage.Models;
using AzureBlobStorage.Services;
using AzureBlobStorage.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AzureBlobStorage.Controllers
{
    public class HomeController(IContainerService _containerService, IBlobService _blobService) : Controller
    {
        

        public async Task<IActionResult> Index()
        {
            List<ContainerAndBlobViewModel> containerAndBlobs = await _containerService.GetAllContainerAndBlob();

            string accountName = containerAndBlobs.FirstOrDefault()?.AccountName ?? "Unknown Account";

            ViewBag.AccountName = accountName;

            return View(containerAndBlobs);
        }

        public async Task<IActionResult> Images()
        {
            return View(await _blobService.GetAllBlobsWithUri("azstorageaccounteamir-images"));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
