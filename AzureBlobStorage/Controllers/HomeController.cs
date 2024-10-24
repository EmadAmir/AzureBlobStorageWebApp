using AzureBlobStorage.Models;
using AzureBlobStorage.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AzureBlobStorage.Controllers
{
    public class HomeController(IContainerService _containerService) : Controller
    {
        

        public async Task<IActionResult> Index()
        {
            List<ContainerAndBlob> containerAndBlobs = await _containerService.GetAllContainerAndBlob();

            string accountName = containerAndBlobs.FirstOrDefault()?.AccountName ?? "Unknown Account";

            ViewBag.AccountName = accountName;

            return View(containerAndBlobs);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
