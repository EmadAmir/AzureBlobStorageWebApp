using AzureBlobStorage.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureBlobStorage.Controllers
{
    public class BlobController(IBlobService _blobService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Manage(string containerName)
        {
            var blobObj = await _blobService.GetAllBlobs(containerName);


            return View(blobObj);
        }

        [HttpGet]
        public IActionResult AddFile(string containerName)
        { 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile file, string containerName) 
        { 
            if(file == null || file.Length < 1) return View();

            //original filename = abc_img.png
            //new filename = abc_img_guidhere.png
            var fileName = Path.GetFileNameWithoutExtension(file.FileName)+"_"+Guid.NewGuid()+ Path.GetExtension(file.FileName);
            var result = await _blobService.UploadBlob(fileName, file, containerName);

            if(result)
                return RedirectToAction("Index","Container");

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> ViewFile(string blobName,string containerName)
        {

            return Redirect(await _blobService.GetBlob(blobName,containerName));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteFile(string blobName, string containerName)
        {
            await _blobService.DeleteBlob(blobName,containerName);

            return RedirectToAction("Index","Home");
        }

    }
}
