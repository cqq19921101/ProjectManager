using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace Lib_SQM_Domain.SharedLibs
{
    public static class SessionHelper
    {
        public static string GetSessionStringValue(HttpSessionState Session, string Key)
        {
            string r = "";
            if (Session[Key] == null)
                Session.Add(Key, "");
            else
                r = (string)Session[Key];
            return r;
        }

        public static void SetSessionValue(HttpSessionState Session, string Key, object Value)
        {
            if (Session[Key] == null)
                Session.Add(Key, Value);
            else
                Session[Key] = Value;
        }

        public static string GetSessionStringValue(HttpSessionStateBase Session, string Key)
        {
            string r = "";
            if (Session[Key] == null)
                Session.Add(Key, "");
            else
                r = (string)Session[Key];
            return r;
        }

        public static object GetSessionObjectValue(HttpSessionStateBase Session, string Key)
        {
            object r = null;
            if (Session[Key] == null)
                Session.Add(Key, null);
            else
                r = Session[Key];
            return r;
        }

        public static void SetSessionValue(HttpSessionStateBase Session, string Key, object Value)
        {
            if (Session[Key] == null)
                Session.Add(Key, Value);
            else
                Session[Key] = Value;
        }
    }
}
