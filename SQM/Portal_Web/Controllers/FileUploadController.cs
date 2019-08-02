using Portal_Web.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Portal_Web.Controllers
{
    public class FileUploadController : Controller
    {
        FilesHelper filesHelper;
        String tempPath = "~/";
        String serverMapPath = "~/UploadFile/";
        private string StorageRoot
        {
            get { return Path.Combine(HostingEnvironment.MapPath(serverMapPath)); }
        }
        private string UrlBase = "/UploadFile/";
        String DeleteURL = "/FileUpload/DeleteFile/?file=";
        String DeleteType = "GET";
        //public FileUploadController()
        //{
        //    //String tempDirectory = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + Guid.NewGuid().ToString().Replace("-", "_") + "/";
        //    String tempDirectory = Request.QueryString["subfolderPath"];
        //    filesHelper = new FilesHelper(DeleteURL, DeleteType, StorageRoot + tempDirectory, UrlBase + tempDirectory, tempPath + tempDirectory, serverMapPath + tempDirectory);
        //}
        // GET: FileUpload
        public ActionResult Index()
        {
            //Determin Upload Folder Name
            ViewBag.UploadFolderName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + Guid.NewGuid().ToString().Replace("-", "_");

            return View();
        }
        [HttpPost]
        public JsonResult Upload()
        {
            String tempDirectory = Request.QueryString["subfolderPath"] + "/";
            filesHelper = new FilesHelper(DeleteURL, DeleteType, StorageRoot + tempDirectory, UrlBase + tempDirectory, tempPath + tempDirectory, serverMapPath + tempDirectory);

            var resultList = new List<ViewDataUploadFilesResult>();

            var CurrentContext = HttpContext;

            filesHelper.UploadAndShowResults(CurrentContext, resultList);
            JsonFiles files = new JsonFiles(resultList);

            bool isEmpty = !resultList.Any();
            if (isEmpty)
            {
                return Json("Error ", "text/html");
            }
            else
            {
                return Json(files, "text/html");
            }
        }
        [HttpGet]
        public JsonResult DeleteFile(string file)
        {
            String tempDirectory = Request.QueryString["subfolderPath"];
            filesHelper = new FilesHelper(DeleteURL, DeleteType, StorageRoot + tempDirectory, UrlBase + tempDirectory, tempPath + tempDirectory, serverMapPath + tempDirectory);

            filesHelper.DeleteFile(file);
            return Json("OK", "text/html", JsonRequestBehavior.AllowGet);
        }
    }
}