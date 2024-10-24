using AzureBlobStorage.Models;
using AzureBlobStorage.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureBlobStorage.Controllers
{
    public class ContainerController(IContainerService _containerService) : Controller
    {
        public async Task<IActionResult> Index()
        {      
            var allContainer = await _containerService.GetAllContainer();

            return View(allContainer);
        }

        public async Task<IActionResult> Delete(string containerName)
        {
            await _containerService.DeleteContainer(containerName);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> create()
        {

            return View(new Container());
        }
        [HttpPost]
        public async Task<IActionResult> Create(Container container)
        {
            await _containerService.CreateContainer(container.Name);

            return RedirectToAction(nameof(Index));
        }
    }
}
