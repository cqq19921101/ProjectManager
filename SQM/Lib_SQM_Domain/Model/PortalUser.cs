using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib_SQM_Domain.Modal
{
    public class PortalUser
    {
        private readonly PortalUserProfile _LoginUser;
        private PortalUserProfile _RunAsUser;

        public PortalUserProfile LoginUser { get { return _LoginUser; } }
        public PortalUserProfile RunAsUser { get { return _RunAsUser; } }

        public PortalUser(PortalUserProfile UserProfile)
        {
            this._LoginUser = UserProfile;
            this._RunAsUser = UserProfile.Duplicate();
        }

        public void RunAs(PortalUserProfile RunAsUserProfile)
        {
            this._RunAsUser = RunAsUserProfile;
        }
    }
}
