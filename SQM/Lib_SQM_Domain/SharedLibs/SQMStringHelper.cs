using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web;

namespace Lib_SQM_Domain.SharedLibs
{
    public static class SQMStringHelper
    {
        public static object NullOrEmptyStringIsDBNull(string data)
        {
            if (data == null)
                return DBNull.Value;
            else
            {
                if (data.Trim() == "")
                    return DBNull.Value;
                else
                    return data.Trim();
            }
        }

        public static string NullOrEmptyStringIsEmptyString(object data)
        {
            return (data == null) ? "" : data.ToString().Trim();
        }

        public static bool DataIsNullOrEmpty(string data)
        {
            bool r = true;
            if (data != null)
                if (data.Trim() != "")
                    r = false;
            return r;
        }

        public static string NormalizeSeperatedDataString(string SourceString, string Seperator)
        {
            string sTargetString = SourceString.Trim();
            string Sep = Seperator.Trim();
            int iLenOfSep = Sep.Length;
            bool bNormalized = false;
            while (!bNormalized)
            {
                if (sTargetString == "")
                {
                    bNormalized = true;
                    break;
                }

                if (sTargetString.Length > iLenOfSep)
                {
                    if (sTargetString.Substring(0, iLenOfSep) == Sep)
                        sTargetString = (sTargetString.Substring(iLenOfSep)).Trim();
                    else
                    {
                        if (sTargetString.Substring(sTargetString.Length - iLenOfSep, iLenOfSep) == Sep)
                            sTargetString = (sTargetString.Substring(0, sTargetString.Length - iLenOfSep)).Trim();
                        else
                        {
                            bNormalized = true;
                            break;
                        }
                    }
                }
            }

            return sTargetString;
        }

        public static bool IsMMDDYYYY(string SourceString)
        {   //01/01/2222
            bool bResult = true;
            string sTargetString = SourceString.Trim();

            if (sTargetString.Length != 10)
                bResult = false;
            else
            {
                if ((sTargetString.Substring(2, 1) != "/") || (sTargetString.Substring(5, 1) != "/"))
                    bResult = false;
                else
                {
                    int MM = 0;
                    int DD = 0;
                    int YYYY = 0;
                    if ((!int.TryParse(sTargetString.Substring(0, 2), out MM))
                        || (!int.TryParse(sTargetString.Substring(3, 2), out DD))
                        || (!int.TryParse(sTargetString.Substring(6), out YYYY)))
                        bResult = false;
                    else
                    {
                        try
                        {
                            DateTime dt = new DateTime(YYYY, MM, DD);
                            if (dt.ToString("MM/dd/yyyy") != sTargetString)
                                bResult = false;
                        }
                        catch
                        {
                            bResult = false;
                        }
                    }
                }
            }

            return bResult;
        }

        public static DateTime MMDDYYYYToDateTime(string MMDDYYYY)
        {
            int MM = int.Parse(MMDDYYYY.Substring(0, 2));
            int DD = int.Parse(MMDDYYYY.Substring(3, 2));
            int YYYY = int.Parse(MMDDYYYY.Substring(6));
            return new DateTime(YYYY, MM, DD);
        }

        public static String EmptyOrUnescapedStringViaUrlDecode(string EscapedString)
        {
            return EscapedString == null ? "" : HttpUtility.UrlDecode(EscapedString.Replace("+", "%2b"));
        }

        public static String EmptyOrUnescapedStringViaHtmlDecode(string EscapedString)
        {
            return EscapedString == null ? "" : HttpUtility.HtmlDecode(EscapedString);
        }
    }
}
