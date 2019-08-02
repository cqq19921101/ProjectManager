using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib_SQM_Domain.SharedLibs
{
    public enum enumPortalGlobalConstant
    {
        CookieVersion,
        SMTPServer,
        LocalTempPath,
        LocalPathBase,
        Config_SitePurpose,

        SessionKey_SitePurpose,
        SessionKey_UserSelectedLocale,
        //SessionKey_SessionKey,
        SessionKey_MenuForRunAsUser,
        SessionKey_RememberMeSessionKey,
        SessionKey_FunctionPathString,

        //ApplicationKey_LoginUserListKey,
        SessionKey_SessionLoginUserKey,

        Cookie_WhoAmICookieKey,

        LockIsNotValid,
        PortalFunctionPathPrefix,

        Config_iPowerProjectPropertyUrl,
        SessionKey_iPowerProjectPropertyUrl,
        Config_iPowerTCSPropertyUrl,
        SessionKey_iPowerTCSPropertyUrl,

        IMPLTopNforSelect
    }

    public static class PortalGlobalConstantHelper
    {
        public static string GetConstant(enumPortalGlobalConstant ConstantKey)
        {
            string sContantString = "";

            switch (ConstantKey)
            {
                case enumPortalGlobalConstant.CookieVersion: sContantString = "5"; break;
                case enumPortalGlobalConstant.SMTPServer: sContantString = "10.1.15.143"; break;
                case enumPortalGlobalConstant.LocalTempPath: sContantString = "__Temp__"; break;
                case enumPortalGlobalConstant.LocalPathBase: sContantString = "UploadFile/"; break;
                case enumPortalGlobalConstant.Config_SitePurpose: sContantString = "SitePurpose"; break;

                case enumPortalGlobalConstant.SessionKey_SitePurpose: sContantString = "__SitePurpose"; break;
                case enumPortalGlobalConstant.SessionKey_UserSelectedLocale: sContantString = "__UserSelectedLocale"; break;
                //case enumPortalGlobalConstant.SessionKey_SessionKey: sContantString = "__SessionKey"; break;
                case enumPortalGlobalConstant.SessionKey_MenuForRunAsUser: sContantString = "__MenuForRunAsUser"; break;
                case enumPortalGlobalConstant.SessionKey_RememberMeSessionKey: sContantString = "__RememberMeSessionKey"; break;
                case enumPortalGlobalConstant.SessionKey_FunctionPathString: sContantString = "__FunctionPathString"; break;

                //case enumPortalGlobalConstant.ApplicationKey_LoginUserListKey: sContantString = "__LoginUsers"; break;
                case enumPortalGlobalConstant.SessionKey_SessionLoginUserKey: sContantString = "__LoginUser"; break;

                case enumPortalGlobalConstant.Cookie_WhoAmICookieKey: sContantString = "WhoAmI"; break;

                case enumPortalGlobalConstant.LockIsNotValid: sContantString = "__lock_is_not_valid__"; break;
                case enumPortalGlobalConstant.PortalFunctionPathPrefix: sContantString = "PORTAL_FunPath_"; break;

                case enumPortalGlobalConstant.Config_iPowerProjectPropertyUrl: sContantString = "iPowerProjectPropertyUrl"; break;
                case enumPortalGlobalConstant.SessionKey_iPowerProjectPropertyUrl: sContantString = "__iPowerProjectPropertyUrl"; break;
                case enumPortalGlobalConstant.Config_iPowerTCSPropertyUrl: sContantString = "iPowerTCSPropertyUrl"; break;
                case enumPortalGlobalConstant.SessionKey_iPowerTCSPropertyUrl: sContantString = "__iPowerTCSPropertyUrl"; break;

                case enumPortalGlobalConstant.IMPLTopNforSelect: sContantString = "500"; break;

                default: break;
            }

            return sContantString;
        }
    }
}
