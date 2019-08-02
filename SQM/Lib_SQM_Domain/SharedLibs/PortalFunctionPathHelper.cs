using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib_SQM_Domain.SharedLibs
{
    public static class PortalFunctionPathHelper
    {
        public static string BuildFunctionPath(string PortalFunctionPath)
        {
            StringBuilder sb = new StringBuilder();
            if (PortalFunctionPath.Trim() != "")
            {
                sb.Append("&nbsp;");
                string[] FunctionSegment = PortalFunctionPath.Split('|');
                foreach (string Title in FunctionSegment)
                    sb.Append("&nbsp;<span class='Portal_FunctionPath_Delimiter'>»</span>&nbsp;<span class='Portal_FunctionPath_Name'>" + Title + "</span>");
            }
            return sb.ToString();
        }
    }
}
