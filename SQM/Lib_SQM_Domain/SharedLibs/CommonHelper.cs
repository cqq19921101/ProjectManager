using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Lib_SQM_Domain.Modal
{
    public class CommonHelper
    {
        public static string urlPre = string.Empty;
        
        public static string getURL(HttpRequestBase Request)
        {
            string appName = Request.ApplicationPath.ToString();
            string serverName = Request.Url.Authority.ToString();
            if (serverName.ToLower() == "localhost")
            {
                serverName = "UAT651";
            }

            string protocol = Regex.Split(Request.Url.AbsoluteUri.ToString(), @"://")[0];
            string urlPre = protocol + "://" + serverName + appName;
            return urlPre;
        }

        public static string getAppName(HttpRequestBase Request)
        {
            string appName = Request.ApplicationPath.ToString();
      
            return appName;
        }
        public static void setUrlPre(HttpRequestBase Request)
        {
            string appName = Request.ApplicationPath.ToString();
            string serverName = Request.Url.Authority.ToString();
            string protocol = Regex.Split(Request.Url.AbsoluteUri.ToString(), @"://")[0];
            urlPre = protocol + "://" + serverName + appName;
        }
    }
}
