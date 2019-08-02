using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using System.IO;

namespace Lib_SQM_Domain.SharedLibs
{
    public static class FileHandleHelper
    {
        public static FileInfoForOutput OutputFile(string LocalFilePath, string FileName)
        {
            if (LocalFilePath.Substring(LocalFilePath.Length - 1, 1) != "/")
                LocalFilePath += "/";
            return new FileInfoForOutput(File.ReadAllBytes(LocalFilePath + FileName), FileName);
        }

        public static string GetFixedFileName(HttpBrowserCapabilitiesBase Browser, HttpServerUtilityBase Server, string FileName)
        {
            string sFixedFileName = FileName;
            try
            {
                if (Browser.Browser == "IE" && Convert.ToInt32(Browser.MajorVersion) < 9)
                    sFixedFileName = Server.UrlPathEncode(sFixedFileName);
            }
            catch { }
            return sFixedFileName;
        }

        public static string GetMappedLocalPath(HttpServerUtilityBase Server, string RequestApplicationPath, string sLocalPathBase, string SubPath)
        {
            string sAppPath = RequestApplicationPath;
            if (sAppPath.Substring(sAppPath.Length - 1, 1) != "/")
                sAppPath += "/";
            string sLocalPath = sAppPath + sLocalPathBase;

            if (SubPath != "")
                sLocalPath += SubPath;

            if (sLocalPath.Substring(sLocalPath.Length - 1, 1) != "/")
                sLocalPath += "/";

            return Server.MapPath(sLocalPath);
        }

        public static string GetMappedLocalAppRootPath(HttpServerUtilityBase Server, string RequestApplicationPath)
        {
            string sAppPath = RequestApplicationPath;
            if (sAppPath.Substring(sAppPath.Length - 1, 1) != "/")
                sAppPath += "/";

            return Server.MapPath(sAppPath);
        }

        public static string GetMappedLocalTempPath(HttpServerUtilityBase Server, string RequestApplicationPath)
        {
            string sAppPath = RequestApplicationPath;
            if (sAppPath.Substring(sAppPath.Length - 1, 1) != "/")
                sAppPath += "/";
            string sLocalPath = sAppPath + PortalGlobalConstantHelper.GetConstant(enumPortalGlobalConstant.LocalTempPath);

            if (sLocalPath.Substring(sLocalPath.Length - 1, 1) != "/")
                sLocalPath += "/";

            return Server.MapPath(sLocalPath);
        }
    }
}