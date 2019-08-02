using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib_SQM_Domain.Modal
{
    public class PortalUserProfile
    {
        private readonly string _MemberGUID = "";
        private readonly string _AccountID = "";
        private readonly int _MemberType;
        private readonly string _UserCName = "";
        private readonly string _UserEName = "";
        private readonly string _PrimaryEmail = "";
        private readonly List<ProfileRole> _Roles;
        private readonly List<string> _AllowedControllerAction;

        public string MemberGUID { get { return this._MemberGUID; } }
        public string AccountID { get { return this._AccountID; } }
        public int MemberType { get { return this._MemberType; } }
        public string UserCName { get { return this._UserCName; } }
        public string UserEName { get { return this._UserEName; } }
        public string PrimaryEmail { get { return this._PrimaryEmail; } }
        public List<ProfileRole> Roles { get { return this._Roles; } }
        public List<string> AllowedControllerAction { get { return this._AllowedControllerAction; } }

        public PortalUserProfile(string MemberGUID, string AccountID, int MemberType, string UserCName, string UserEName, string PrimaryEmail, List<ProfileRole> Roles, List<string> AllowedControllerAction)
        {
            this._MemberGUID = MemberGUID;
            this._AccountID = AccountID;
            this._MemberType = MemberType;
            this._UserCName = UserCName;
            this._UserEName = UserEName;
            this._PrimaryEmail = PrimaryEmail;
            this._Roles = Roles;
            this._AllowedControllerAction = AllowedControllerAction;
        }

        public PortalUserProfile Duplicate()
        {
            //List<ProfileRole> r = new List<ProfileRole>();
            //foreach(ProfileRole thisR in this._Roles)
            //{
            //    r.Add(new ProfileRole(thisR.RoleGUID, thisR.RoleName));
            //}
            List<ProfileRole> r = this._Roles.ToList();
            List<string> a = this._AllowedControllerAction;
            return new PortalUserProfile(this._MemberGUID, this._AccountID, this._MemberType, this._UserCName, this._UserEName, this.PrimaryEmail, r, a);
        }
    }

    public class ProfileRole
    {
        private readonly string _RoleGUID;
        private readonly string _RoleName;

        public string RoleGUID { get { return this._RoleGUID; } }
        public string RoleName { get { return this._RoleName; } }

        public ProfileRole(string RoleGUID, string RoleName)
        {
            this._RoleGUID = RoleGUID;
            this._RoleName = RoleName;
        }
    }
}
