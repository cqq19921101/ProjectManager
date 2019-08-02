using Lib_Portal_Domain.SharedLibs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal_Web.Controllers
{
    public class UploadifyController : Controller
    {
        const string sLocalPathBase = "UploadFile/";

        [HttpPost]
        public string Upload(HttpPostedFileBase FileData)
        {
            string sAppPath = Request.ApplicationPath;
            if (sAppPath.Substring(sAppPath.Length - 1, 1) != "/")
                sAppPath += "/";
            string sLocalPath = sAppPath + sLocalPathBase;

            if (Request.QueryString["SubPath"] != null)
            {
                sLocalPath += Request.QueryString["SubPath"];

                //Create SUBPATH if required
                Directory.CreateDirectory(Server.MapPath(sLocalPath));
            }

            if (sLocalPath.Substring(sLocalPath.Length - 1, 1) != "/")
                sLocalPath += "/";

            foreach (string item in Request.Files)
            {
                HttpPostedFileBase postFile = Request.Files[item];
                string file = sLocalPath + postFile.FileName;

                postFile.SaveAs(Server.MapPath(file));

                return System.Guid.NewGuid().ToString();
            }

            return "Upload OK!";
        }

        public ActionResult OutputInStream(string SubPath, string FileName)
        {
            string sAppPath = Request.ApplicationPath;
            if (sAppPath.Substring(sAppPath.Length - 1, 1) != "/")
                sAppPath += "/";
            string sLocalPath = sAppPath + sLocalPathBase;

            FileInfoForOutput fi = FileHandleHelper.OutputFile(Server.MapPath(sLocalPath + SubPath), FileName);
            return File(fi.Buffer, fi.MimeMapping, FileHandleHelper.GetFixedFileName(Request.Browser, Server, fi.FileName));
        }
    }
}