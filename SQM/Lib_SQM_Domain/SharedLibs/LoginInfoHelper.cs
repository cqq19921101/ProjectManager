using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web;
using Lib_SQM_Domain.Modal;

namespace Lib_SQM_Domain.SharedLibs
{
    //public class LoginInfoInApplication
    //{
    //    private readonly string _SessionKey;
    //    private readonly DateTime _LoginTime;
    //    private DateTime _LastAccessTime;
    //    private PortalUser _PortalUser;
        
    //    public LoginInfoInApplication(PortalUser user)
    //    {
    //        this._SessionKey = Guid.NewGuid().ToString();
    //        this._LoginTime = System.DateTime.Now;
    //        this._LastAccessTime = this._LoginTime;
    //        this._PortalUser = user;
    //    }

    //    public string SessionKey { get { return this._SessionKey; } }
    //    public DateTime LoginTime { get { return this._LoginTime; } }
    //    public DateTime LastAccessTime { get { return this._LastAccessTime; } set { this._LastAccessTime = value; } }
    //    public PortalUser PortalUser { get { return this._PortalUser; } }
    //}

    //public static class LoginInfoHelper
    //{
    //    private const string LoginUserListKey = "__LoginUsers";

    //    public static string RegisterLoginUser(HttpApplicationStateBase Application, PortalUser user)
    //    {
    //        if (Application[LoginUserListKey] == null)
    //            Application.Add(LoginUserListKey, new Dictionary<string, LoginInfoInApplication>());

    //        LoginInfoInApplication li = new LoginInfoInApplication(user);
    //        ((Dictionary<string, LoginInfoInApplication>)(Application[LoginUserListKey])).Add(li.SessionKey, li);
            
    //        return li.SessionKey;
    //    }

    //    public static void UnRegisterLoginUser(HttpApplicationStateBase Application, string SessionKey)
    //    {
    //        if (Application[LoginUserListKey] != null)
    //        {
    //            try { ((Dictionary<string, LoginInfoInApplication>)Application[LoginUserListKey]).Remove(SessionKey); }
    //            catch { }
    //        }
    //    }

    //    public static void UpdateLastAccessTime(HttpApplicationStateBase Application, string SessionKey)
    //    {
    //        if (Application[LoginUserListKey] != null)
    //        {
    //            if (((Dictionary<string, LoginInfoInApplication>)Application[LoginUserListKey]).ContainsKey(SessionKey))
    //            {
    //                ((Dictionary<string, LoginInfoInApplication>)Application[LoginUserListKey])[SessionKey].LastAccessTime = DateTime.Now;
    //            }
    //        }
    //    }

    //    public static string GetActiveUserList(HttpApplicationStateBase Application)
    //    {
    //        List<LoginInfoInApplication> LoginList = new List<LoginInfoInApplication>();

    //        StringBuilder sbLogins = new StringBuilder();
    //        sbLogins.Append("<table border=\"1\"><tr>");
    //        sbLogins.Append("<td>#</td><td>Login</td><td>Run As</td><td>Login Time</td><td>Last Access Time</td><td>Idle Time (in minute)</td>");
    //        int iCnt = 0;
    //        DateTime dtNow = DateTime.Now;
    //        Dictionary<string, LoginInfoInApplication> Logins = (Dictionary<string, LoginInfoInApplication>)(Application[LoginUserListKey]);
    //        foreach (LoginInfoInApplication LoginInfo in Logins.Values)
    //        {
    //            TimeSpan IdleTime = dtNow - LoginInfo.LastAccessTime;
    //            //if (IdleTime.TotalMinutes <= 20.0)
    //            //{
    //                iCnt++;
    //                sbLogins.Append("<tr>");
    //                sbLogins.Append("<td>" + iCnt.ToString() + "</td>");
    //                sbLogins.Append("<td>" + MISCHelper.MemberDisplayName(LoginInfo.PortalUser.LoginUser.UserCName, LoginInfo.PortalUser.LoginUser.UserEName) + "</td>");
    //                sbLogins.Append("<td>" + MISCHelper.MemberDisplayName(LoginInfo.PortalUser.RunAsUser.UserCName, LoginInfo.PortalUser.RunAsUser.UserEName) + "</td>");
    //                sbLogins.Append("<td>" + LoginInfo.LoginTime.ToString("MM/dd/yyyy HH:mm:ss") + "</td>");
    //                sbLogins.Append("<td>" + LoginInfo.LastAccessTime.ToString("MM/dd/yyyy HH:mm:ss") + "</td>");
    //                sbLogins.Append("<td>" + Math.Round(IdleTime.TotalMinutes, 2).ToString() + "</td>");
    //                sbLogins.Append("</tr>");
    //            //}
    //        }
    //        sbLogins.Append("</table>");

    //        return sbLogins.ToString();
    //    }
    //}
}
