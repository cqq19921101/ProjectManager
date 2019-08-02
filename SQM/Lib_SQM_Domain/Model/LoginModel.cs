using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web;

namespace Lib_SQM_Domain.Modal
{
    public class LoginInfo
    {
        protected string _UserID = "";
        protected string _Password = "";
        protected string _UserSelectedLocale = "";

        public string UserID { get { return this._UserID; } set { this._UserID = value; } }
        public string Password { get { return this._Password; } set { this._Password = value; } }
        public string UserSelectedLocale { get { return this._UserSelectedLocale; } set { this._UserSelectedLocale = value; } }

        public LoginInfo() { }
        public LoginInfo(string UserID, string Password, string UserSelectedLocale)
        {
            this._UserID = UserID;
            this._Password = Password;
            this._UserSelectedLocale = UserSelectedLocale;
        }
    }

    public class LoginModel : LoginInfo
    {
        protected string _ErrMsg = "";
        protected bool _RememberMe = false;
        protected string _SessionKeyForCookie = "";
        protected string _Ver = "5";

        public string ErrMsg { get { return this._ErrMsg; } set { this._ErrMsg = value; } }
        public bool RememberMe { get { return this._RememberMe; } set { this._RememberMe = value; } }
        public string SessionKeyForCookie { get { return this._SessionKeyForCookie; } set { this._SessionKeyForCookie = value; } }
        public string Ver { get { return this._Ver; } set { this._Ver = value; } }

        public LoginModel() { }
        public LoginModel(string UserID, string Password, string UserSelectedLocale, string ErrMsg, bool RememberMe, string SessionKeyForCookie, string Ver)
            : base(UserID, Password, UserSelectedLocale)
        {
            this._ErrMsg = ErrMsg;
            this._RememberMe = RememberMe;
            this._SessionKeyForCookie = SessionKeyForCookie;
            this._Ver = Ver;
        }
    }
}
