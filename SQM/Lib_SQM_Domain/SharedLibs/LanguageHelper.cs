using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lib_SQM_Domain_MUIResources;
using System.Globalization;

namespace Lib_SQM_Domain.SharedLibs
{
    public static class LanguageHelper
    {
        public static string GetApplicationDefaultLocaleString()
        {
            return GetApplicationDefaultLocaleString("");
        }

        public static string GetApplicationDefaultLocaleString(string sUserBrowserLocale)
        {
            string r = "";
            switch (sUserBrowserLocale.ToLower())
            {
                case "zh-tw":
                case "zh-cn":
                    r = sUserBrowserLocale;
                    break;
                default:
                    r = "en-US";
                    break;
            }
            return r;
        }

        public static string GetBasedLanguageOptionsInJavascriptArrayString()
        {
            CultureInfo pCI = Portal_Domain_MUI.Culture;
   
            //string r = "['en-us', 'English'],['zh-tw', '繁體中文'],['zh-cn', '简体中文']";
            StringBuilder sb = new StringBuilder();

            Portal_Domain_MUI.Culture = new CultureInfo("en-US");
            sb.Append("['en-US', '");
            sb.Append(Portal_Domain_MUI.PORTAL_LOCALE_OPTION);
            sb.Append("']");

            Portal_Domain_MUI.Culture = new CultureInfo("zh-TW");
            sb.Append(",['zh-TW', '");
            sb.Append(Portal_Domain_MUI.PORTAL_LOCALE_OPTION);
            sb.Append("']");

            Portal_Domain_MUI.Culture = new CultureInfo("zh-CN");
            sb.Append(",['zh-CN', '");
            sb.Append(Portal_Domain_MUI.PORTAL_LOCALE_OPTION);
            sb.Append("']");

            Portal_Domain_MUI.Culture = pCI;
            
            return sb.ToString();
        }

        public static string GetLoginPageTextResourcesInJavascriptArrayString()
        {
            CultureInfo pCI = Portal_Domain_MUI.Culture;

            //['系統語言：', '帳號：', '密碼：', '登入', '歡迎來到 XXX 入口網站...', 'XXX 入口網站'],
            StringBuilder sb = new StringBuilder();
            string[] sLocaleKey = { "en-US", "zh-TW", "zh-CN" };
            for (int iCnt = 0; iCnt < sLocaleKey.Length; iCnt++)
            {
                if (iCnt > 0) sb.Append(",");
                Portal_Domain_MUI.Culture = new CultureInfo(sLocaleKey[iCnt]);
                sb.Append("['" + Portal_Domain_MUI.PORTAL_LANG + Portal_Domain_MUI.PORTAL_COLON);
                sb.Append("', '" + Portal_Domain_MUI.PORTAL_ACCOUNTID + Portal_Domain_MUI.PORTAL_COLON);
                sb.Append("', '" + Portal_Domain_MUI.PORTAL_PASSWORD + Portal_Domain_MUI.PORTAL_COLON);
                sb.Append("', '" + Portal_Domain_MUI.PORTAL_SIGNIN);
                sb.Append("', '" + Portal_Domain_MUI.PORTAL_WELCOMEMSG);
                sb.Append("', '" + Portal_Domain_MUI.PORTAL_TITLE);
                sb.Append("', '" + Portal_Domain_MUI.PORTAL_REMEMBERME);
                sb.Append("', '" + Portal_Domain_MUI.PORTAL_FORGOTPASSWORD);
                sb.Append("']");
            }

            Portal_Domain_MUI.Culture = pCI;

            return sb.ToString();
        }
    }
}
