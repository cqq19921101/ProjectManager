using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Hosting;

namespace Portal_Web.Helpers
{
    public class ViewDataUploadFilesResult
    {
        public string name { get; set; }
        public int size { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string deleteUrl { get; set; }
        public string thumbnailUrl { get; set; }
        public string deleteType { get; set; }
    }
    public class JsonFiles
    {
        public ViewDataUploadFilesResult[] files;
        public string TempFolder { get; set; }
        public JsonFiles(List<ViewDataUploadFilesResult> filesList)
        {
            files = new ViewDataUploadFilesResult[filesList.Count];
            for (int i = 0; i < filesList.Count; i++)
            {
                files[i] = filesList.ElementAt(i);
            }
        }
    }
    public class FilesHelper
    {
        String DeleteURL = null;
        String DeleteType = null;
        String StorageRoot = null;
        String UrlBase = null;
        String tempPath = null;
        //ex:"~/Files/something/";
        String serverMapPath = null;
        public FilesHelper(String DeleteURL, String DeleteType, String StorageRoot, String UrlBase, String tempPath, String serverMapPath)
        {
            this.DeleteURL = DeleteURL;
            this.DeleteType = DeleteType;
            this.StorageRoot = StorageRoot;
            this.UrlBase = UrlBase;
            this.tempPath = tempPath;
            this.serverMapPath = serverMapPath;
        }
        public void UploadAndShowResults(HttpContextBase ContentBase, List<ViewDataUploadFilesResult> resultList)
        {
            var httpRequest = ContentBase.Request;
            //System.Diagnostics.Debug.WriteLine(Directory.Exists(tempPath));

            String fullPath = Path.Combine(StorageRoot);
            Directory.CreateDirectory(fullPath);
            // Create new folder for thumbs
            //Directory.CreateDirectory(fullPath + "/thumbs/");

            foreach (String inputTagName in httpRequest.Files)
            {

                var headers = httpRequest.Headers;

                var file = httpRequest.Files[inputTagName];
                //System.Diagnostics.Debug.WriteLine(file.FileName);

                if (string.IsNullOrEmpty(headers["X-File-Name"]))
                {

                    UploadWholeFile(ContentBase, resultList);
                }
                //else
                //{

                //    UploadPartialFile(headers["X-File-Name"], ContentBase, resultList);
                //}
            }
        }
        private void UploadWholeFile(HttpContextBase requestContext, List<ViewDataUploadFilesResult> statuses)
        {
            var request = requestContext.Request;
            for (int i = 0; i < request.Files.Count; i++)
            {
                var file = request.Files[i];
                String pathOnServer = Path.Combine(StorageRoot);
                var fullPath = Path.Combine(pathOnServer, Path.GetFileName(file.FileName));
                file.SaveAs(fullPath);

                //Create thumb
                //string[] imageArray = file.FileName.Split('.');
                //if (imageArray.Length != 0)
                //{
                //    String extansion = imageArray[imageArray.Length - 1];
                //    if (extansion != "jpg" && extansion != "png") //Do not create thumb if file is not an image
                //    {

                //    }
                //    else
                //    {
                //        var ThumbfullPath = Path.Combine(pathOnServer, "thumbs");
                //        String fileThumb = file.FileName + ".80x80.jpg";
                //        var ThumbfullPath2 = Path.Combine(ThumbfullPath, fileThumb);
                //        using (MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(fullPath)))
                //        {
                //            var thumbnail = new WebImage(stream).Resize(80, 80);
                //            thumbnail.Save(ThumbfullPath2, "jpg");
                //        }

                //    }
                //}
                statuses.Add(UploadResult(Path.GetFileName(file.FileName), file.ContentLength, file.FileName));
            }
        }
        //private void UploadPartialFile(string fileName, HttpContextBase requestContext, List<ViewDataUploadFilesResult> statuses)
        //{
        //    var request = requestContext.Request;
        //    if (request.Files.Count != 1) throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");
        //    var file = request.Files[0];
        //    var inputStream = file.InputStream;
        //    String patchOnServer = Path.Combine(StorageRoot);
        //    var fullName = Path.Combine(patchOnServer, Path.GetFileName(file.FileName));
        //    var ThumbfullPath = Path.Combine(fullName, Path.GetFileName(file.FileName + ".80x80.jpg"));
        //    ImageHandler handler = new ImageHandler();

        //    var ImageBit = ImageHandler.LoadImage(fullName);
        //    handler.Save(ImageBit, 80, 80, 10, ThumbfullPath);
        //    using (var fs = new FileStream(fullName, FileMode.Append, FileAccess.Write))
        //    {
        //        var buffer = new byte[1024];

        //        var l = inputStream.Read(buffer, 0, 1024);
        //        while (l > 0)
        //        {
        //            fs.Write(buffer, 0, l);
        //            l = inputStream.Read(buffer, 0, 1024);
        //        }
        //        fs.Flush();
        //        fs.Close();
        //    }
        //    statuses.Add(UploadResult(file.FileName, file.ContentLength, file.FileName));
        //}
        public ViewDataUploadFilesResult UploadResult(String FileName, int fileSize, String FileFullPath)
        {
            String getType = System.Web.MimeMapping.GetMimeMapping(FileFullPath);
            String applicationName = HttpContext.Current.Request.ApplicationPath.ToString();
            var result = new ViewDataUploadFilesResult()
            {
                name = FileName,
                size = fileSize,
                type = getType,
                //20170320 edward change for avoid ' ' replace to '+'
                //url = applicationName + UrlBase + HttpUtility.UrlEncode(FileName),
                //deleteUrl = applicationName + DeleteURL + UrlBase + HttpUtility.UrlEncode(FileName),
                url = applicationName + UrlBase + FileName,
                deleteUrl = applicationName + DeleteURL + UrlBase + FileName,
                thumbnailUrl = "",//CheckThumb(getType, FileName),
                deleteType = DeleteType,
            };
            return result;
        }
        public String DeleteFile(String file)
        {
            //System.Diagnostics.Debug.WriteLine("DeleteFile");
            //    var req = HttpContext.Current;
            //System.Diagnostics.Debug.WriteLine(file);
            //StorageRoot = StorageRoot.Substring(0, StorageRoot.LastIndexOf("\\"));
            String fullPath = Path.Combine(HostingEnvironment.MapPath("~/" + file));
            //System.Diagnostics.Debug.WriteLine(fullPath);
            //System.Diagnostics.Debug.WriteLine(System.IO.File.Exists(fullPath));
            //String thumbPath = "/" + file + ".80x80.jpg";
            //String partThumb1 = Path.Combine(StorageRoot, "thumbs");
            //String partThumb2 = Path.Combine(partThumb1, file + ".80x80.jpg");

            //System.Diagnostics.Debug.WriteLine(partThumb2);
            //System.Diagnostics.Debug.WriteLine(System.IO.File.Exists(partThumb2));
            if (System.IO.File.Exists(fullPath))
            {
                //delete thumb 
                //if (System.IO.File.Exists(partThumb2))
                //{
                //    System.IO.File.Delete(partThumb2);
                //}
                System.IO.File.Delete(fullPath);
                String succesMessage = "Ok";
                return succesMessage;
            }
            String failMessage = "Error Delete";
            return failMessage;
        }
    }
}