using Azure.Identity;
using Azure.Storage.Blobs;
using BlobStorageAuthentication.WebApp.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using System.Text;

namespace BlobStorageAuthentication.WebApp.Controllers
{
    public class UploadWithADController : Controller
    {
        public UserAcquisitionTokenCredential UserAcquisitionTokenCredential { get; }

        public UploadWithADController(UserAcquisitionTokenCredential userAcquisitionTokenCredential)
        {
            UserAcquisitionTokenCredential = userAcquisitionTokenCredential;
        }
        // GET: UploadWithADController
        public ActionResult Index()
        {
            return View();
        }

        // GET: UploadWithADController/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        //    POST: UploadWithADController/Create
        [AuthorizeForScopes(Scopes = new string[]
             { "https://storage.azure.com/user_impersonation" })]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormFile file) 
        {
            try
            {
                var user = HttpContext.User;
                var blobContent = new StringBuilder();
                using StreamReader reader= new StreamReader(file.OpenReadStream());
                while (!reader.EndOfStream) blobContent.AppendLine(reader.ReadLine());
                string containerName = "adauthcontainer";
                string blobEndpoint = "https://anishstrgdemo1.blob.core.windows.net/adauthcontainer";
                Uri blobUri = new Uri(blobEndpoint);

                BlobServiceClient blobServiceClient = new BlobServiceClient(blobUri, UserAcquisitionTokenCredential);
                BlobContainerClient container = blobServiceClient.GetBlobContainerClient(containerName);

                var result = await container.UploadBlobAsync(file.FileName, new MemoryStream(Encoding.UTF8.GetBytes(blobContent.ToString())));

                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: UploadWithADController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UploadWithADController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UploadWithADController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UploadWithADController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
