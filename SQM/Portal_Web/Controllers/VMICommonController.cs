using Lib_Portal_Domain.SharedLibs;
using Lib_Portal_Domain.SupportClasses;
using Lib_VMI_Domain.Model;
using Portal_Web.SupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal_Web.Controllers
{
    [OverrideMemberCulture(Order = 20)]
    [Authorize(Order = 30)]
    [PreventNoLoginUser(Order = 40)]
    [SetPortalSessionStopWatch(Order = 60)]
    public class VMICommonController : Controller
    {
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryCorpVendorInfoJsonWithFilter(string CorpVendorCode)
        {
            string r = VMI_Common_Helper.Query_CorpVendorCodeInfo(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                CorpVendorCode.Trim());
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QuerySBUVendorInfoJsonWithFilter(string SBUVendorCode)
        {
            string r = VMI_Common_Helper.Query_SBUVendorCodeInfo(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                SBUVendorCode.Trim());
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryPlantCodeInfoJsonWithFilter(string PLANT)
        {
            string r = VMI_Common_Helper.Query_PlantCodeInfo(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PLANT.Trim());
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryBuyerCodeInfoJsonWithFilter(string BUYER)
        {
            string r = VMI_Common_Helper.Query_BuyerCodeInfo(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                BUYER.Trim());
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
    }
}