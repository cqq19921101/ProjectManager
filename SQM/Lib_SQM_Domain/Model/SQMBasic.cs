using Aspose.Cells;
using Lib_Portal_Domain.Model;
using Lib_Portal_Domain.SharedLibs;
using Lib_VMI_Domain.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

//using Lib_SQM_Domain.SharedLibs;
//using Lib_Portal_Domain.Model;

namespace Lib_SQM_Domain.Modal
{
    #region Data Class Definitions
    public class TB_SQM_APPLICATION_PARAM
    {
        public int id { get; set; }

        public String APPLICATION_NAME { get; set; }

        public String FUNCTION_NAME { get; set; }

        public String PARAME_NAME { get; set; }

        public String PARAME_ITEM { get; set; }

        public String PARAME_VALUE1 { get; set; }

        public String PARAME_VALUE2 { get; set; }

        public String PARAME_VALUE3 { get; set; }

        public String PARAME_VALUE4 { get; set; }

        public String REMARK { get; set; }

    }
    #endregion

    



    public static class SystemMgmt_BasicInfoType_Helper
    {
        #region Create/Edit data check
        private static void UnescapeDataFromWeb(TB_SQM_Manufacturers_BasicInfo DataItem)
        {
            DataItem.BasicInfoGUID = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.BasicInfoGUID);
            DataItem.VendorCode = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.VendorCode);
            DataItem.TB_SQM_Vendor_TypeCID = DataItem.TB_SQM_Vendor_TypeCID;
            DataItem.TB_SQM_Commodity_SubTB_SQM_CommodityCID = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.TB_SQM_Commodity_SubTB_SQM_CommodityCID);
            DataItem.TB_SQM_Commodity_SubCID = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.TB_SQM_Commodity_SubCID);
        }

        private static String DataCheck(TB_SQM_Manufacturers_BasicInfo DataItem)
        {
            String r = "";
            List<string> e = new List<string>();
            if (DataItem.TB_SQM_Vendor_TypeCID == 0)
            {
                e.Add("Must choose Vendor Type.");
            }
            if (StringHelper.DataIsNullOrEmpty(DataItem.TB_SQM_Commodity_SubTB_SQM_CommodityCID))
            {
                e.Add("Must choose Product Type.");
            }
            if (StringHelper.DataIsNullOrEmpty(DataItem.TB_SQM_Commodity_SubCID))
            {
                e.Add("Must choose Sub Product Type.");
            }
            

            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }
        #endregion

        #region Create data item
        public static String CreateDataItem(SqlConnection cnPortal, TB_SQM_Manufacturers_BasicInfo DataItem)
        {
            UnescapeDataFromWeb(DataItem);
            String r = DataCheck(DataItem);
            r = CheckExist(cnPortal, DataItem);
            if (r != "")
            { return r; }
            else
            {
                String sSQL = "Insert Into TB_SQM_Manufacturers_BasicInfo (BasicInfoGUID, VendorCode, TB_SQM_Vendor_TypeCID, TB_SQM_Commodity_SubTB_SQM_CommodityCID, TB_SQM_Commodity_SubCID,CreateDatetime,UpdateDatetime) ";
                sSQL += "Values (@BasicInfoGUID, @VendorCode, @TB_SQM_Vendor_TypeCID, @TB_SQM_Commodity_SubTB_SQM_CommodityCID, @TB_SQM_Commodity_SubCID,getDate(),getDate());";
                SqlCommand cmd = new SqlCommand(sSQL, cnPortal);
                cmd.Parameters.AddWithValue("@BasicInfoGUID", Guid.NewGuid());
                cmd.Parameters.AddWithValue("@VendorCode", StringHelper.NullOrEmptyStringIsDBNull(DataItem.VendorCode));
                cmd.Parameters.AddWithValue("@TB_SQM_Vendor_TypeCID", DataItem.TB_SQM_Vendor_TypeCID);
                cmd.Parameters.AddWithValue("@TB_SQM_Commodity_SubTB_SQM_CommodityCID", StringHelper.NullOrEmptyStringIsDBNull(DataItem.TB_SQM_Commodity_SubTB_SQM_CommodityCID));
                cmd.Parameters.AddWithValue("@TB_SQM_Commodity_SubCID", StringHelper.NullOrEmptyStringIsDBNull(DataItem.TB_SQM_Commodity_SubCID));

                String sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;

                return sErrMsg;
            }
        }

        /// <summary>
        /// 檢查此類型是否存在
        /// </summary>
        /// <param name="cnPortal"></param>
        /// <param name="DataItem"></param>
        /// <returns></returns>
        public static String CheckExist(SqlConnection cnPortal, TB_SQM_Manufacturers_BasicInfo DataItem)
        {

            String sSQL = @"
                SELECT 'e'
                FROM TB_SQM_Manufacturers_BasicInfo
                WHERE VendorCode = @VendorCode
                AND TB_SQM_Vendor_TypeCID = @TB_SQM_Vendor_TypeCID
                AND TB_SQM_Commodity_SubTB_SQM_CommodityCID = @TB_SQM_Commodity_SubTB_SQM_CommodityCID
                AND TB_SQM_Commodity_SubCID = @TB_SQM_Commodity_SubCID
                ";
            SqlCommand cmd = new SqlCommand(sSQL, cnPortal);
            cmd.Parameters.AddWithValue("@VendorCode", StringHelper.NullOrEmptyStringIsDBNull(DataItem.VendorCode));
            cmd.Parameters.AddWithValue("@TB_SQM_Vendor_TypeCID", DataItem.TB_SQM_Vendor_TypeCID);
            cmd.Parameters.AddWithValue("@TB_SQM_Commodity_SubTB_SQM_CommodityCID", StringHelper.NullOrEmptyStringIsDBNull(DataItem.TB_SQM_Commodity_SubTB_SQM_CommodityCID));
            cmd.Parameters.AddWithValue("@TB_SQM_Commodity_SubCID", StringHelper.NullOrEmptyStringIsDBNull(DataItem.TB_SQM_Commodity_SubCID));

            String sErrMsg = "";
            DataTable dt = new DataTable();
            try
            {
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            catch (Exception e) { sErrMsg = "Check fail.<br />Exception: " + e.ToString(); }
            cmd = null;
            if (dt.Rows.Count > 0)
            {
                sErrMsg = "Already Exist";
            }
            return sErrMsg;

        }
        #endregion

        #region Edit data item
        public static String EditDataItem(SqlConnection cnPortal, TB_SQM_Manufacturers_BasicInfo DataItem)
        {
            return EditDataItem(cnPortal, DataItem, "", "");
        }

        public static String EditDataItem(SqlConnection cnPortal, TB_SQM_Manufacturers_BasicInfo DataItem, String LoginMemberGUID, String RunAsMemberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            String r = DataCheck(DataItem);
            r = CheckExist(cnPortal, DataItem);
            if (r != "")
            { return r; }
            else
            {
                SqlTransaction tran = cnPortal.BeginTransaction();

                //Update member data
                using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = EditDataItemSub(cmd, DataItem); }
                if (r != "") { tran.Rollback(); return r; }

                //Check lock is still valid
                bool bLockIsStillValid = false;
                if ((LoginMemberGUID != "") && (RunAsMemberGUID != ""))
                    using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { bLockIsStillValid = DataLockHelper.CheckLockIsStillValid(cmd, DataItem.BasicInfoGUID, LoginMemberGUID, RunAsMemberGUID); }
                if (!bLockIsStillValid) { tran.Rollback(); return DataLockHelper.LockString(); }

                //Release lock
                if ((LoginMemberGUID != "") && (RunAsMemberGUID != ""))
                    using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = DataLockHelper.ReleaseLock(cmd, DataItem.BasicInfoGUID, LoginMemberGUID, RunAsMemberGUID); }
                if (r != "") { tran.Rollback(); return r; }

                //Commit
                try { tran.Commit(); }
                catch (Exception e) { tran.Rollback(); r = "Edit fail.<br />Exception: " + e.ToString(); }
                return r;
            }
        }

        private static String EditDataItemSub(SqlCommand cmd, TB_SQM_Manufacturers_BasicInfo DataItem)
        {
            String sErrMsg = "";
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append(@"
            Update TB_SQM_Manufacturers_BasicInfo 
            Set TB_SQM_Vendor_TypeCID = @TB_SQM_Vendor_TypeCID 
            ,TB_SQM_Commodity_SubTB_SQM_CommodityCID = @TB_SQM_Commodity_SubTB_SQM_CommodityCID
            ,TB_SQM_Commodity_SubCID = @TB_SQM_Commodity_SubCID
            ,UpdateDatetime = getDate()
            Where BasicInfoGUID = @BasicInfoGUID
            AND VendorCode = @VendorCode;
            ");
            cmd.CommandText = sbSQL.ToString();
            cmd.Parameters.AddWithValue("@BasicInfoGUID", DataItem.BasicInfoGUID);
            cmd.Parameters.AddWithValue("@VendorCode", DataItem.VendorCode);
            cmd.Parameters.AddWithValue("@TB_SQM_Vendor_TypeCID", DataItem.TB_SQM_Vendor_TypeCID);
            cmd.Parameters.AddWithValue("@TB_SQM_Commodity_SubTB_SQM_CommodityCID", DataItem.TB_SQM_Commodity_SubTB_SQM_CommodityCID);
            cmd.Parameters.AddWithValue("@TB_SQM_Commodity_SubCID", DataItem.TB_SQM_Commodity_SubCID);
            

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion

        #region Delete data item
        public static String DeleteDataItem(SqlConnection cnPortal, TB_SQM_Manufacturers_BasicInfo DataItem)
        {
            return DeleteDataItem(cnPortal, DataItem, "", "");
        }

        public static String DeleteDataItem(SqlConnection cnPortal, TB_SQM_Manufacturers_BasicInfo DataItem, String LoginMemberGUID, String RunAsMemberGUID)
        {
            String r = "";
            if (StringHelper.DataIsNullOrEmpty(DataItem.BasicInfoGUID))
                return "Must provide BasicInfoGUID.";
            else
            {
                SqlTransaction tran = cnPortal.BeginTransaction();

                //Delete member data
                using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = DeleteDataItemSub(cmd, DataItem); }
                if (r != "") { tran.Rollback(); return r; }

                //Release lock
                if ((LoginMemberGUID != "") && (RunAsMemberGUID != ""))
                    using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = DataLockHelper.ReleaseLock(cmd, DataItem.BasicInfoGUID, LoginMemberGUID, RunAsMemberGUID); }
                if (r != "") { tran.Rollback(); return r; }

                //Commit
                try { tran.Commit(); }
                catch (Exception e) { tran.Rollback(); r = "Delete fail.<br />Exception: " + e.ToString(); }
                return r;
            }
        }

        private static String DeleteDataItemSub(SqlCommand cmd, TB_SQM_Manufacturers_BasicInfo DataItem)
        {
            String sErrMsg = "";

            String sSQL = "Delete TB_SQM_Manufacturers_BasicInfo Where BasicInfoGUID = @BasicInfoGUID;";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@BasicInfoGUID", DataItem.BasicInfoGUID);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion
    }

    public static class SQM_Mail_Helper
    {
        public static String SendMail(String MailBody)
        {
            

            icm045.CMSHandler MS = new icm045.CMSHandler();
            return MS.MailSend("SupplierPortal",
                           "SupplierPortal@liteon.com",
                           "jeter.sun@liteon.com",
                           "",
                           "",
                           "SQM Notice",
                           MailBody,
                           icm045.MailPriority.Normal,
                           icm045.MailFormat.Html,
                           new string[0]);
        }
    }

    public static class SQM_Basic_Helper
    {
        //public static String RegisterSQMAccount(String ERP_VND)
        //{
        //    SQM_AccountService.SP_CreateAccountRequest CAR = new SQM_AccountService.SP_CreateAccountRequest();


        //    icm045.CMSHandler MS = new icm045.CMSHandler();
        //    return MS.MailSend("SupplierPortal",
        //                   "SupplierPortal@liteon.com",
        //                   "jeter.sun@liteon.com",
        //                   "",
        //                   "",
        //                   "SQM Notice",
        //                   MailBody,
        //                   icm045.MailPriority.Normal,
        //                   icm045.MailFormat.Html,
        //                   new string[0]);
        //}
       
        //public static String GetFillStatusSql(String VendorTypeID,String BasicInfoPartID)
        //{
        //    String r = sSQL_UPDATE_FILL_STATUS;
        //    r = Regex.Replace(sSQL_UPDATE_FILL_STATUS, "@BasicInfoPartID", BasicInfoPartID);
        //    r = Regex.Replace(sSQL_UPDATE_FILL_STATUS, "@VendorTypeID", BasicInfoPartID);
        //}
        public static String GetVendorType(SqlConnection cn, PortalUserProfile RunAsUser)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select (convert(nvarchar(255), CID))as CID, CNAME From TB_SQM_Vendor_Type Order By CID;");
            //sb.Append("Select CID, CNAME From TB_SQM_Commodity Order By CID;");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }
        public static String GetBasicInfoTypes(SqlConnection cn, PortalUserProfile RunAsUser, String VendorCode)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
            SELECT BasicInfoGUID,
                   VendorCode,
                   TB_SQM_Vendor_TypeCID,
                   VT.CNAME AS VTNAME,
                   TB_SQM_Commodity_SubCID,
                   C.CNAME AS CNAME,
                   TB_SQM_Commodity_SubTB_SQM_CommodityCID,
                   CS.CNAME AS CSNAME,
                   ISNULL(AC.Status, 'None') AS Status
            FROM TB_SQM_Manufacturers_BasicInfo BI
                 LEFT OUTER JOIN TB_SQM_Approve_Case AC ON BI.ApproveCaseID = AC.CaseID,
                 TB_SQM_Vendor_Type VT,
                 TB_SQM_Commodity_Sub CS,
                 TB_SQM_Commodity C
            WHERE BI.TB_SQM_Vendor_TypeCID = VT.CID
              AND BI.TB_SQM_Commodity_SubTB_SQM_CommodityCID = C.CID
              AND ( BI.TB_SQM_Commodity_SubTB_SQM_CommodityCID = CS.TB_SQM_CommodityCID
                AND BI.TB_SQM_Commodity_SubCID = CS.CID
                  )
              AND VendorCode =@VendorCode
            ORDER BY CreateDatetime
            ");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@VendorCode", VendorCode));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }

        public static String GetBasicData(SqlConnection cn, PortalUserProfile RunAsUser, TB_SQM_Manufacturers_BasicInfo DataItem)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select TOP 1 * From TB_SQM_Manufacturers_BasicInfo WHERE VendorCode=@VendorCode AND BasicInfoGUID=@BasicInfoGUID");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@VendorCode", DataItem.VendorCode));
                cmd.Parameters.Add(new SqlParameter("@BasicInfoGUID", DataItem.BasicInfoGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }



        public static String GetCommodityList(SqlConnection cn, PortalUserProfile RunAsUser)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select CID, CNAME From TB_SQM_Commodity Order By CID;");

            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }

        public static String GetMapVendorCode(SqlConnection cn, PortalUserProfile RunAsUser)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select TOP 1 VendorCode From TB_SQM_Member_Vendor_Map");
            sb.Append(" WHERE MemberGUID=@MemberGUID");
            String vendorCode = "";
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@MemberGUID", RunAsUser.MemberGUID));
                var vendorCodeScale = cmd.ExecuteScalar();
                vendorCode = (vendorCodeScale == null ? "" : vendorCodeScale.ToString());
            }
            return vendorCode;
        }

        public static String GetCommoditySubList(SqlConnection cn, String MainID)
        {
            if (String.IsNullOrEmpty(MainID))
            {
                return JsonConvert.SerializeObject(new DataTable());
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT CID,CNAME FROM dbo.TB_SQM_Commodity_Sub");
            sb.Append(" WHERE TB_SQM_CommodityCID=@TB_SQM_CommodityCID");
            sb.Append(" Order By CID;");

            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@TB_SQM_CommodityCID", MainID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }
        #region Create/Edit data check
        private static void UnescapeDataFromWeb(TB_SQM_Manufacturers_BasicInfo DataItem)
        {
            DataItem.VendorCode = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.VendorCode);
            DataItem.BasicInfoGUID = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.BasicInfoGUID);
            DataItem.CompanyAddress= StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CompanyAddress);
            DataItem.CompanyAdvantage= StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CompanyAdvantage);
            DataItem.ProvidedName = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ProvidedName);
            DataItem.JobTitle = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.JobTitle);
        }

        private static String DataCheck(TB_SQM_Manufacturers_BasicInfo DataItem)
        {
            String r = "";
            List<string> e = new List<string>();

            if (DataItem.TB_SQM_Vendor_TypeCID == 1 || DataItem.TB_SQM_Vendor_TypeCID == 2 || DataItem.TB_SQM_Vendor_TypeCID == 3)
            {
                if (StringHelper.DataIsNullOrEmpty(DataItem.CompanyName))
                    e.Add("Must provide CompanyName.");
                if (StringHelper.DataIsNullOrEmpty(DataItem.CompanyAddress))
                    e.Add("Must provide CompanyAddress.");
                if (StringHelper.DataIsNullOrEmpty(DataItem.DateInfo))
                    e.Add("Must provide DateInfo.");
                if (StringHelper.DataIsNullOrEmpty(DataItem.ProvidedName))
                    e.Add("Must provide ProvidedName.");
                if (StringHelper.DataIsNullOrEmpty(DataItem.JobTitle))
                    e.Add("Must provide JobTitle.");
            }

            if (DataItem.TB_SQM_Vendor_TypeCID == 1)
            {
                if (StringHelper.DataIsNullOrEmpty(DataItem.FactoryName))
                    e.Add("Must provide FactoryName.");
                if (StringHelper.DataIsNullOrEmpty(DataItem.FactoryAddress))
                    e.Add("Must provide FactoryAddress.");
            }

            //if (StringHelper.DataIsNullOrEmpty(DataItem.VendorCode))
            //    e.Add("Must provide VendorCode Name.");
            //if (StringHelper.DataIsNullOrEmpty(DataItem.EnterpriseCategory))
            //    e.Add("Must provide EnterpriseCategory.");
            //if (StringHelper.DataIsNullOrEmpty(DataItem.OwnerShip))
            //    e.Add("Must provide OwnerShip.");
            //if (StringHelper.DataIsNullOrEmpty(DataItem.FoundedYear.ToString()))
            //    e.Add("Must provide FoundedYear.");
            //if (StringHelper.DataIsNullOrEmpty(DataItem.LastRevenues1.ToString()))
            //    e.Add("Must provide LastRevenues1.");
            //if (StringHelper.DataIsNullOrEmpty(DataItem.LastRevenues2.ToString()))
            //    e.Add("Must provide LastRevenues2.");
            //if (StringHelper.DataIsNullOrEmpty(DataItem.LastRevenues3.ToString()))
            //    e.Add("Must provide LastRevenues3.");
            //if (StringHelper.DataIsNullOrEmpty(DataItem.CurrentRevenues.ToString()))
            //    e.Add("Must provide CurrentRevenues.");
            //if (StringHelper.DataIsNullOrEmpty(DataItem.TurnoverAnalysis))
            //    e.Add("Must provide TurnoverAnalysis.");
            //if (StringHelper.DataIsNullOrEmpty(DataItem.TradingCurrency))
            //    e.Add("Must provide TradingCurrency.");
            //if (StringHelper.DataIsNullOrEmpty(DataItem.TradeMode))
            //    e.Add("Must provide TradeMode.");
            //if (StringHelper.DataIsNullOrEmpty(DataItem.VMIManageModel))
            //    e.Add("Must provide VMIManageModel.");
            //if (StringHelper.DataIsNullOrEmpty(DataItem.Distance))
            //    e.Add("Must provide Distance.");
            //if (StringHelper.DataIsNullOrEmpty(DataItem.MinMonthStateDays))
            //    e.Add("Must provide MinMonthStateDays.");
            //if (StringHelper.DataIsNullOrEmpty(DataItem.BU1TurnoverName))
            //    e.Add("Must provide BU1TurnoverName.");
            //if (StringHelper.DataIsNullOrEmpty(DataItem.BU2TurnoverName))
            //    e.Add("Must provide BU2TurnoverName.");
            //if (StringHelper.DataIsNullOrEmpty(DataItem.BU3TurnoverName))
            //    e.Add("Must provide BU3TurnoverName.");

            //if (StringHelper.DataIsNullOrEmpty(DataItem.FactoryName))
            //    e.Add("Must provide FactoryName.");
            //if (StringHelper.DataIsNullOrEmpty(DataItem.FactoryAddress))
            //    e.Add("Must provide FactoryAddress.");
            //if (StringHelper.DataIsNullOrEmpty(DataItem.DateInfo))
            //    e.Add("Must provide DateInfo.");
            //if (StringHelper.DataIsNullOrEmpty(DataItem.ProvidedName))
            //    e.Add("Must provide ProvidedName.");
            //if (StringHelper.DataIsNullOrEmpty(DataItem.JobTitle))
            //    e.Add("Must provide JobTitle.");
            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }

        private static String DataCheckGenral(TB_SQM_Manufacturers_BasicInfo DataItem)
        {
            String r = "";
            List<string> e = new List<string>();

            //if (DataItem.TB_SQM_Vendor_TypeCID == 1 || DataItem.TB_SQM_Vendor_TypeCID == 2 || DataItem.TB_SQM_Vendor_TypeCID == 3)
            //{
            //    if (StringHelper.DataIsNullOrEmpty(DataItem.CompanyName))
            //        e.Add("Must provide CompanyName.");
            //    if (StringHelper.DataIsNullOrEmpty(DataItem.CompanyAddress))
            //        e.Add("Must provide CompanyAddress.");
            //    if (StringHelper.DataIsNullOrEmpty(DataItem.DateInfo))
            //        e.Add("Must provide DateInfo.");
            //    if (StringHelper.DataIsNullOrEmpty(DataItem.ProvidedName))
            //        e.Add("Must provide ProvidedName.");
            //    if (StringHelper.DataIsNullOrEmpty(DataItem.JobTitle))
            //        e.Add("Must provide JobTitle.");
            //}

            //if (DataItem.TB_SQM_Vendor_TypeCID == 1)
            //{
            //    if (StringHelper.DataIsNullOrEmpty(DataItem.FactoryName))
            //        e.Add("Must provide FactoryName.");
            //    if (StringHelper.DataIsNullOrEmpty(DataItem.FactoryAddress))
            //        e.Add("Must provide FactoryAddress.");
            //}

            //if (StringHelper.DataIsNullOrEmpty(DataItem.VendorCode))
            //    e.Add("Must provide VendorCode Name.");
            if (StringHelper.DataIsNullOrEmpty(DataItem.EnterpriseCategory))
                e.Add("Must provide EnterpriseCategory.");
            if (StringHelper.DataIsNullOrEmpty(DataItem.OwnerShip))
                e.Add("Must provide OwnerShip.");
            if (StringHelper.DataIsNullOrEmpty(DataItem.FoundedYear))
                e.Add("Must provide FoundedYear.");
            if (StringHelper.DataIsNullOrEmpty(DataItem.LastRevenues1.ToString()))
                e.Add("Must provide LastRevenues1.");
            if (StringHelper.DataIsNullOrEmpty(DataItem.LastRevenues2.ToString()))
                e.Add("Must provide LastRevenues2.");
            if (StringHelper.DataIsNullOrEmpty(DataItem.LastRevenues3.ToString()))
                e.Add("Must provide LastRevenues3.");
            if (StringHelper.DataIsNullOrEmpty(DataItem.CurrentRevenues.ToString()))
                e.Add("Must provide CurrentRevenues.");
            if (StringHelper.DataIsNullOrEmpty(DataItem.TurnoverAnalysis))
            {
                e.Add("Must provide TurnoverAnalysis.请输入正确数据");   
            }
           
            if (Checkstring(DataItem.TurnoverAnalysis))
            {
                e.Add("请勿填写特殊字符");
            }
            if (Checkstring(DataItem.RevenueGrowthRate1.ToString()))
            {
                e.Add("请勿填写特殊字符");
            }
            if (Checkstring(DataItem.GrossProfitRate1.ToString()))
            {
                e.Add("请勿填写特殊字符");
            }
            if (StringHelper.DataIsNullOrEmpty(DataItem.TradingCurrency))
                e.Add("Must provide TradingCurrency.");
            if (StringHelper.DataIsNullOrEmpty(DataItem.TradeMode))
                e.Add("Must provide TradeMode.");
            if (StringHelper.DataIsNullOrEmpty(DataItem.VMIManageModel))
                e.Add("Must provide VMIManageModel.");
            if (StringHelper.DataIsNullOrEmpty(DataItem.Distance))
                e.Add("Must provide Distance.");
            if (StringHelper.DataIsNullOrEmpty(DataItem.MinMonthStateDays))
                e.Add("Must provide MinMonthStateDays.");
            //if (StringHelper.DataIsNullOrEmpty(DataItem.BU1TurnoverName))
            //    e.Add("Must provide BU1TurnoverName.");
            //if (StringHelper.DataIsNullOrEmpty(DataItem.BU2TurnoverName))
            //    e.Add("Must provide BU2TurnoverName.");
            //if (StringHelper.DataIsNullOrEmpty(DataItem.BU3TurnoverName))
            //    e.Add("Must provide BU3TurnoverName.");

            //if (StringHelper.DataIsNullOrEmpty(DataItem.FactoryName))
            //    e.Add("Must provide FactoryName.");
            //if (StringHelper.DataIsNullOrEmpty(DataItem.FactoryAddress))
            //    e.Add("Must provide FactoryAddress.");
            //if (StringHelper.DataIsNullOrEmpty(DataItem.DateInfo))
            //    e.Add("Must provide DateInfo.");
            //if (StringHelper.DataIsNullOrEmpty(DataItem.ProvidedName))
            //    e.Add("Must provide ProvidedName.");
            //if (StringHelper.DataIsNullOrEmpty(DataItem.JobTitle))
            //    e.Add("Must provide JobTitle.");
            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

           
            return r;
        }

        private static bool Checkstring(string str)
        {
            bool bl_exist = false;
            if (!string.IsNullOrEmpty(str))
            {
                foreach (char c in str)
                {
                    if ((!char.IsNumber(c))) //既不是字母又不是数字的就认为是特殊字符
                    { bl_exist = true; }
                }
            }
          
            return bl_exist;
        }
        #endregion

        #region Edit data item

        public static String EditVendorType(SqlConnection cnPortal, String VendorCode, String TypeID)
        {
            String sErrMsg = "";
            using (SqlCommand cmd = new SqlCommand("", cnPortal))
            {
                String sSQL = "Update TB_SQM_Manufacturers_BasicInfo Set TB_SQM_Vendor_TypeCID = @TB_SQM_Vendor_TypeCID ";
                sSQL += " Where VendorCode =@VendorCode;";
                cmd.CommandText = sSQL;
                cmd.Parameters.AddWithValue("@TB_SQM_Vendor_TypeCID", TypeID);
                cmd.Parameters.AddWithValue("@VendorCode", VendorCode);

                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }
            }
            return sErrMsg;
        }

        public static String EditDataItem(SqlConnection cnPortal, TB_SQM_Manufacturers_BasicInfo DataItem, String LoginMemberGUID)
        {
            return EditDataItem(cnPortal, DataItem, LoginMemberGUID, "");
        }

        public static String EditDataItem(SqlConnection cnPortal, TB_SQM_Manufacturers_BasicInfo DataItem, String LoginMemberGUID, String RunAsMemberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            String r = DataCheck(DataItem);
            if (r != "")
            { return r; }
            else
            {
                SqlTransaction tran = cnPortal.BeginTransaction();

                //Update member data
                using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = EditDataItemSub(cmd, DataItem); }
                if (r != "") { tran.Rollback(); return r; }

                //Commit
                try { tran.Commit(); }
                catch (Exception e) { tran.Rollback(); r = "Edit fail.<br />Exception: " + e.ToString(); }
                return r;
            }
        }
        private static String EditDataItemSub(SqlCommand cmd, TB_SQM_Manufacturers_BasicInfo DataItem)
        {
            String sErrMsg = "";
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append(" Update TB_SQM_Manufacturers_BasicInfo Set ");
            sbSQL.Append(" CompanyName =@CompanyName,");
            sbSQL.Append(" CompanyAddress =@CompanyAddress,");
            if (DataItem.IsTrader != null)
            {
                sbSQL.Append(" IsTrader =@IsTrader,");
            }
            if (DataItem.IsSpotTrader != null)
            {
                sbSQL.Append(" IsSpotTrader =@IsSpotTrader,");
            }
            sbSQL.Append(" FactoryName =@FactoryName,");
            sbSQL.Append(" FactoryAddress =@FactoryAddress,");
            //sbSQL.Append(" TB_SQM_Commodity_SubCID =@TB_SQM_Commodity_SubCID,");
            //sbSQL.Append(" TB_SQM_Commodity_SubTB_SQM_CommodityCID =@TB_SQM_Commodity_SubTB_SQM_CommodityCID,");
            sbSQL.Append(" DateInfo =@DateInfo,");
            sbSQL.Append(" ProvidedName =@ProvidedName,");
            sbSQL.Append(" JobTitle =@JobTitle,");
            sbSQL.Remove(sbSQL.Length - 1, 1);
            sbSQL.Append("  Where VendorCode =@VendorCode");
            sbSQL.Append("  and BasicInfoGUID =@BasicInfoGUID;");
            InsertPart(sbSQL,"1");

            cmd.CommandText = sbSQL.ToString();
            cmd.Parameters.AddWithValue("@CompanyName", StringIsNull(DataItem.CompanyName));
            cmd.Parameters.AddWithValue("@CompanyAddress", StringIsNull(DataItem.CompanyAddress));
            if (DataItem.IsTrader != null)
            {
                cmd.Parameters.AddWithValue("@IsTrader", DataItem.IsTrader);
            }
            if (DataItem.IsSpotTrader != null)
            {
                cmd.Parameters.AddWithValue("@IsSpotTrader", DataItem.IsSpotTrader);
            }
            cmd.Parameters.AddWithValue("@FactoryName", StringIsNull(DataItem.FactoryName));
            cmd.Parameters.AddWithValue("@FactoryAddress", StringIsNull(DataItem.FactoryAddress));
            //cmd.Parameters.AddWithValue("@TB_SQM_Commodity_SubCID", StringIsNull(DataItem.TB_SQM_Commodity_SubCID));
            //cmd.Parameters.AddWithValue("@TB_SQM_Commodity_SubTB_SQM_CommodityCID", StringIsNull(DataItem.TB_SQM_Commodity_SubTB_SQM_CommodityCID));
            cmd.Parameters.AddWithValue("@DateInfo", StringIsNull(DataItem.DateInfo));
            cmd.Parameters.AddWithValue("@ProvidedName", StringIsNull(DataItem.ProvidedName));
            cmd.Parameters.AddWithValue("@JobTitle", StringIsNull(DataItem.JobTitle));
            cmd.Parameters.AddWithValue("@VendorCode", StringIsNull(DataItem.VendorCode));
            cmd.Parameters.AddWithValue("@BasicInfoGUID", StringIsNull(DataItem.BasicInfoGUID));
            cmd.Parameters.AddWithValue("@TB_SQM_Vendor_TypeCID", DataItem.TB_SQM_Vendor_TypeCID);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }


        public static String EditBasicInfoGenral(SqlConnection cnPortal, TB_SQM_Manufacturers_BasicInfo DataItem, String LoginMemberGUID, String RunAsMemberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            String r = DataCheckGenral(DataItem);
            if (r != "")
            { return r; }
            else
            {
                SqlTransaction tran = cnPortal.BeginTransaction();

                //Update member data
                using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = EditBasicGenral(cmd, DataItem); }
                if (r != "") { tran.Rollback(); return r; }

                //Commit
                try { tran.Commit(); }
                catch (Exception e) { tran.Rollback(); r = "Edit fail.<br />Exception: " + e.ToString(); }
                return r;
            }
        }
        private static String EditBasicGenral(SqlCommand cmd, TB_SQM_Manufacturers_BasicInfo DataItem)
        {
            String sErrMsg = "";

            
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append("Update TB_SQM_Manufacturers_BasicInfo Set ");
            sbSQL.Append(" EnterpriseCategory=@EnterpriseCategory,");
            sbSQL.Append(" OwnerShip=@OwnerShip,");

            sbSQL.Append(" FoundedYear=@FoundedYear,");
            sbSQL.Append(" LastRevenues1=@LastRevenues1,");
            sbSQL.Append(" LastRevenues2=@LastRevenues2,");
            sbSQL.Append(" LastRevenues3=@LastRevenues3,");
            sbSQL.Append(" CurrentRevenues=@CurrentRevenues,");

            sbSQL.Append(" TurnoverAnalysis=@TurnoverAnalysis,");

            sbSQL.Append(" RevenueGrowthRate1=@RevenueGrowthRate1,");
            //sbSQL.Append(" RevenueGrowthRate2=@RevenueGrowthRate2,");
            //sbSQL.Append(" RevenueGrowthRate3=@RevenueGrowthRate3,");
            sbSQL.Append(" GrossProfitRate1=@GrossProfitRate1,");
            //sbSQL.Append(" GrossProfitRate2=@GrossProfitRate2,");
            //sbSQL.Append(" GrossProfitRate3=@GrossProfitRate3,");
            sbSQL.Append(" PlanInvestCapital=@PlanInvestCapital,");
            sbSQL.Append(" BankAndAccNumber=@BankAndAccNumber,");
            sbSQL.Append(" TradingCurrency=@TradingCurrency,");
            sbSQL.Append(" TradeMode=@TradeMode,");
            sbSQL.Append(" VMIManageModel=@VMIManageModel,");
            sbSQL.Append(" Distance=@Distance,");
            sbSQL.Append(" MinMonthStateDays=@MinMonthStateDays,");
            sbSQL.Append(" BU1TurnoverName=@BU1TurnoverName,");
            sbSQL.Append(" BU2TurnoverName=@BU2TurnoverName,");
            sbSQL.Append(" BU3TurnoverName=@BU3TurnoverName,");
            sbSQL.Append(" BU1Turnover=@BU1Turnover,");
            sbSQL.Append(" BU2Turnover=@BU2Turnover,");
            sbSQL.Append(" BU3Turnover=@BU3Turnover,");
            sbSQL.Append(" CompanyAdvantage=@CompanyAdvantage,");
            sbSQL.Remove(sbSQL.Length - 1, 1);
            sbSQL.Append(" Where VendorCode =@VendorCode");
            sbSQL.Append(" AND BasicInfoGUID =@BasicInfoGUID;");
            InsertPart(sbSQL,"2");
            cmd.CommandText = sbSQL.ToString();

            cmd.Parameters.AddWithValue("@EnterpriseCategory", StringIsNull(DataItem.EnterpriseCategory));
            cmd.Parameters.AddWithValue("@OwnerShip", StringIsNull(DataItem.OwnerShip));
            cmd.Parameters.AddWithValue("@FoundedYear", DataItem.FoundedYear);
            cmd.Parameters.AddWithValue("@LastRevenues1", DataItem.LastRevenues1);
            cmd.Parameters.AddWithValue("@LastRevenues2", DataItem.LastRevenues2);
            cmd.Parameters.AddWithValue("@LastRevenues3", DataItem.LastRevenues3);
            cmd.Parameters.AddWithValue("@CurrentRevenues", DataItem.CurrentRevenues);
            cmd.Parameters.AddWithValue("@TurnoverAnalysis", StringIsNull(DataItem.TurnoverAnalysis));
            cmd.Parameters.AddWithValue("@RevenueGrowthRate1", StringIsNull(DataItem.RevenueGrowthRate1.ToString()));
            //cmd.Parameters.AddWithValue("@RevenueGrowthRate2", DataItem.RevenueGrowthRate2);
            //cmd.Parameters.AddWithValue("@RevenueGrowthRate3", DataItem.RevenueGrowthRate3);
            cmd.Parameters.AddWithValue("@GrossProfitRate1", StringIsNull(DataItem.GrossProfitRate1.ToString()));
            //cmd.Parameters.AddWithValue("@GrossProfitRate2", DataItem.GrossProfitRate2);
            //cmd.Parameters.AddWithValue("@GrossProfitRate3", DataItem.GrossProfitRate3);
            cmd.Parameters.AddWithValue("@PlanInvestCapital", StringIsNull(DataItem.PlanInvestCapital));
            cmd.Parameters.AddWithValue("@BankAndAccNumber", StringIsNull(DataItem.BankAndAccNumber));
            cmd.Parameters.AddWithValue("@TradingCurrency", StringIsNull(DataItem.TradingCurrency));
            cmd.Parameters.AddWithValue("@TradeMode", StringIsNull(DataItem.TradeMode));
            cmd.Parameters.AddWithValue("@VMIManageModel", StringIsNull(DataItem.VMIManageModel));
            cmd.Parameters.AddWithValue("@Distance", StringIsNull(DataItem.Distance));
            cmd.Parameters.AddWithValue("@MinMonthStateDays", StringIsNull(DataItem.MinMonthStateDays));
            cmd.Parameters.AddWithValue("@BU1TurnoverName", StringIsNull(DataItem.BU1TurnoverName));
            cmd.Parameters.AddWithValue("@BU2TurnoverName", StringIsNull(DataItem.BU2TurnoverName));
            cmd.Parameters.AddWithValue("@BU3TurnoverName", StringIsNull(DataItem.BU3TurnoverName));
            cmd.Parameters.AddWithValue("@BU1Turnover", StringIsNull(DataItem.BU1Turnover));
            cmd.Parameters.AddWithValue("@BU2Turnover", StringIsNull(DataItem.BU2Turnover));
            cmd.Parameters.AddWithValue("@BU3Turnover", StringIsNull(DataItem.BU3Turnover));
            cmd.Parameters.AddWithValue("@CompanyAdvantage", StringIsNull(DataItem.CompanyAdvantage));

            cmd.Parameters.AddWithValue("@VendorCode", DataItem.VendorCode);
            cmd.Parameters.AddWithValue("@BasicInfoGUID", DataItem.BasicInfoGUID);
            cmd.Parameters.AddWithValue("@TB_SQM_Vendor_TypeCID", DataItem.TB_SQM_Vendor_TypeCID);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        public static StringBuilder InsertPart(StringBuilder sb, string type)
        {
            string sSQL_UPDATE_FILL_STATUS = @"
        ;DELETE FROM TB_SQM_Manufacturers_BasicInfo_Part_Fill_Status
            WHERE BasicInfoGUID=@BasicInfoGUID
            AND BasicInfoPartID=@BasicInfoPartID;

        INSERT INTO TB_SQM_Manufacturers_BasicInfo_Part_Fill_Status
        (BasicInfoGUID, BasicInfoPartID) VALUES
            (@BasicInfoGUID, @BasicInfoPartID);

          DECLARE @isFull BIT
        IF(( SELECT COUNT(1)
             FROM TB_SQM_Manufacturers_BasicInfo_Part_Map
             WHERE VendorTypeID =(SELECT TB_SQM_Vendor_TypeCID from TB_SQM_Manufacturers_BasicInfo where BasicInfoGUID =@BasicInfoGUID) ) = ( SELECT COUNT(1)
                                          FROM TB_SQM_Manufacturers_BasicInfo_Part_Fill_Status
                                          WHERE BasicInfoGUID =@BasicInfoGUID ))
            BEGIN
                SET @isFull = 'true'
            END
        ELSE
            BEGIN
                SET @isFull = 'false'
            END
        UPDATE TB_SQM_Manufacturers_BasicInfo
          SET IsApproved = @isFull
        WHERE BasicInfoGUID =@BasicInfoGUID
        
        UPDATE TB_SQM_Manufacturers_BasicInfo
          SET ApproveCaseID = 0
        WHERE BasicInfoGUID =@BasicInfoGUID
        ";
            sb.Append(Regex.Replace(sSQL_UPDATE_FILL_STATUS, "@BasicInfoPartID", type));
            return sb;
        }
        public static String EditBasicInfoAbility(SqlConnection cnPortal, TB_SQM_Manufacturers_BasicInfo DataItem, String LoginMemberGUID, String RunAsMemberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            String r = DataCheck(DataItem);
            if (r != "")
            { return r; }
            else
            {
                SqlTransaction tran = cnPortal.BeginTransaction();

                //Update member data
                using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = EditBasicInfoAbility(cmd, DataItem); }
                if (r != "") { tran.Rollback(); return r; }

                //Commit
                try { tran.Commit(); }
                catch (Exception e) { tran.Rollback(); r = "Edit fail.<br />Exception: " + e.ToString(); }
                return r;
            }
        }
        private static String EditBasicInfoAbility(SqlCommand cmd, TB_SQM_Manufacturers_BasicInfo DataItem)
        {
            String sErrMsg = "";


            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append("Update TB_SQM_Manufacturers_BasicInfo Set ");
            sbSQL.Append(" Is3DUG=@Is3DUG,");
            sbSQL.Append(" Is3DProE=@Is3DProE,");

            sbSQL.Append(" Is2DAutoCAD=@Is2DAutoCAD,");
            sbSQL.Append(" IsPhotoShop=@IsPhotoShop,");
            sbSQL.Append(" IsIDMapAbility=@IsIDMapAbility,");
            sbSQL.Append(" Is3DMapAbility=@Is3DMapAbility,");
            sbSQL.Append(" Is2DMapAbility=@Is2DMapAbility,");

            sbSQL.Append(" IsMoldflowAbility=@IsMoldflowAbility,");

            sbSQL.Append(" IsTAAbility=@IsTAAbility,");
            sbSQL.Append(" IsDesignGuildline=@IsDesignGuildline,");
            sbSQL.Append(" IsFMEA=@IsFMEA,");
            sbSQL.Append(" IsLessonLearnt=@IsLessonLearnt,");
            sbSQL.Append(" MoldProduceCapacity=@MoldProduceCapacity,");
            sbSQL.Remove(sbSQL.Length - 1, 1);
            sbSQL.Append(" Where VendorCode =@VendorCode");
            sbSQL.Append(" AND BasicInfoGUID =@BasicInfoGUID;");

            InsertPart(sbSQL,"9");
            cmd.CommandText = sbSQL.ToString();

            cmd.Parameters.AddWithValue("@Is3DUG", IsNullToDBNull(DataItem.Is3DUG));
            cmd.Parameters.AddWithValue("@Is3DProE", IsNullToDBNull(DataItem.Is3DProE));
            cmd.Parameters.AddWithValue("@Is2DAutoCAD", IsNullToDBNull(DataItem.Is2DAutoCAD));
            cmd.Parameters.AddWithValue("@IsPhotoShop", IsNullToDBNull(DataItem.IsPhotoShop));
            cmd.Parameters.AddWithValue("@IsIDMapAbility", IsNullToDBNull(DataItem.IsIDMapAbility));
            cmd.Parameters.AddWithValue("@Is3DMapAbility", IsNullToDBNull(DataItem.Is3DMapAbility));
            cmd.Parameters.AddWithValue("@Is2DMapAbility", IsNullToDBNull(DataItem.Is2DMapAbility));
            cmd.Parameters.AddWithValue("@IsMoldflowAbility", IsNullToDBNull(DataItem.IsMoldflowAbility));
            cmd.Parameters.AddWithValue("@IsTAAbility", IsNullToDBNull(DataItem.IsTAAbility));
            cmd.Parameters.AddWithValue("@IsDesignGuildline", IsNullToDBNull(DataItem.IsDesignGuildline));
            cmd.Parameters.AddWithValue("@IsFMEA", IsNullToDBNull(DataItem.IsFMEA));
            cmd.Parameters.AddWithValue("@IsLessonLearnt", IsNullToDBNull(DataItem.IsLessonLearnt));
            cmd.Parameters.AddWithValue("@MoldProduceCapacity", IsNullToDBNull(DataItem.MoldProduceCapacity));

            cmd.Parameters.AddWithValue("@VendorCode", DataItem.VendorCode);
            cmd.Parameters.AddWithValue("@BasicInfoGUID", DataItem.BasicInfoGUID);
            cmd.Parameters.AddWithValue("@TB_SQM_Vendor_TypeCID", DataItem.TB_SQM_Vendor_TypeCID);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }

        static object StringIsNull(String input)
        {
            String s = StringHelper.EmptyOrUnescapedStringViaUrlDecode(input);
            return String.IsNullOrEmpty(s) ? (object)DBNull.Value : s;
        }

        static object IsNullToDBNull(Object input)
        {
            return input==null ? (object)DBNull.Value : input;
        }

        public static String GetParam(SqlConnection cn, PortalUserProfile RunAsUser, TB_SQM_APPLICATION_PARAM DataItem)
        {
            StringBuilder sbSQL = new StringBuilder();
            
            sbSQL.Append(@"
            SELECT id,
                   APPLICATION_NAME,
                   FUNCTION_NAME,
                   PARAME_NAME,
                   PARAME_ITEM,
                   PARAME_VALUE1,
                   PARAME_VALUE2,
                   PARAME_VALUE3,
                   PARAME_VALUE4,
                   REMARK
            FROM TB_SQM_APPLICATION_PARAM
            WHERE 1=1
            ");
            if (!String.IsNullOrEmpty(DataItem.APPLICATION_NAME))
                sbSQL.Append(" AND APPLICATION_NAME =@APPLICATION_NAME");
            if (!String.IsNullOrEmpty(DataItem.FUNCTION_NAME))
                sbSQL.Append(" AND FUNCTION_NAME =@FUNCTION_NAME");
            if (!String.IsNullOrEmpty(DataItem.PARAME_NAME))
                sbSQL.Append(" AND PARAME_NAME =@PARAME_NAME");
            if (!String.IsNullOrEmpty(DataItem.PARAME_ITEM))
                sbSQL.Append(" AND PARAME_ITEM =@PARAME_ITEM");
            if (!String.IsNullOrEmpty(DataItem.PARAME_VALUE1))
                sbSQL.Append(" AND PARAME_VALUE1 =@PARAME_VALUE1");
            if (!String.IsNullOrEmpty(DataItem.PARAME_VALUE2))
                sbSQL.Append(" AND PARAME_VALUE2 =@PARAME_VALUE2");
            if (!String.IsNullOrEmpty(DataItem.PARAME_VALUE3))
                sbSQL.Append(" AND PARAME_VALUE3 =@PARAME_VALUE3");
            if (!String.IsNullOrEmpty(DataItem.PARAME_VALUE4))
                sbSQL.Append(" AND PARAME_VALUE4 =@PARAME_VALUE4");
            if (!String.IsNullOrEmpty(DataItem.REMARK))
                sbSQL.Append(" AND REMARK =@REMARK");

            String sql = Regex.Replace(sbSQL.ToString(), @"\s+", " ");
            //sb.Append("Select CID, CNAME From TB_SQM_Commodity Order By CID;");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                if (!String.IsNullOrEmpty(DataItem.APPLICATION_NAME))
                    cmd.Parameters.AddWithValue("@APPLICATION_NAME", IsNullToDBNull(DataItem.APPLICATION_NAME));
                if (!String.IsNullOrEmpty(DataItem.FUNCTION_NAME))
                    cmd.Parameters.AddWithValue("@FUNCTION_NAME", IsNullToDBNull(DataItem.FUNCTION_NAME));
                if (!String.IsNullOrEmpty(DataItem.PARAME_NAME))
                    cmd.Parameters.AddWithValue("@PARAME_NAME", IsNullToDBNull(DataItem.PARAME_NAME));
                if (!String.IsNullOrEmpty(DataItem.PARAME_ITEM))
                    cmd.Parameters.AddWithValue("@PARAME_ITEM", IsNullToDBNull(DataItem.PARAME_ITEM));
                if (!String.IsNullOrEmpty(DataItem.PARAME_VALUE1))
                    cmd.Parameters.AddWithValue("@PARAME_VALUE1", IsNullToDBNull(DataItem.PARAME_VALUE1));
                if (!String.IsNullOrEmpty(DataItem.PARAME_VALUE2))
                    cmd.Parameters.AddWithValue("@PARAME_VALUE2", IsNullToDBNull(DataItem.PARAME_VALUE2));
                if (!String.IsNullOrEmpty(DataItem.PARAME_VALUE3))
                    cmd.Parameters.AddWithValue("@PARAME_VALUE3", IsNullToDBNull(DataItem.PARAME_VALUE3));
                if (!String.IsNullOrEmpty(DataItem.PARAME_VALUE4))
                    cmd.Parameters.AddWithValue("@PARAME_VALUE4", IsNullToDBNull(DataItem.PARAME_VALUE4));
                if (!String.IsNullOrEmpty(DataItem.REMARK))
                    cmd.Parameters.AddWithValue("@REMARK", IsNullToDBNull(DataItem.REMARK));

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }
        #endregion
        public static void CreatAgentExcel(String filename, SqlConnection sqlConnection, String BasicInfoGUID, String localPath, String urlPre)
        {
            License lic = new License();
            //String AsposeLicPath = "E:\\TW VMI V2\\Portal_Web\\Source\\Aspose.Cells\\Aspose.Cells.lic";
            //lic.SetLicense(AsposeLicPath);
            //String templatePath = "E:\\agent.xlsx";
            lic.SetLicense(localPath + @"Source\Aspose.Cells\Aspose.Cells.lic");
            String templatePath = localPath + @"Source\Template\agent.xlsx";
            Workbook book = new Workbook(templatePath);
            Worksheet sheet0 = book.Worksheets[0];
            BindAgentReport(ref sheet0, sqlConnection, BasicInfoGUID, urlPre);
            book.CalculateFormula();
            object i = book.Worksheets[0].Cells[15, 6].Value;
            EditScore(sqlConnection, BasicInfoGUID, i);
            book.Save(filename, SaveFormat.Xlsx);
        }
        public static void BindAgentReport(ref Worksheet sheet6, SqlConnection sqlConnection, String BasicInfoGUID, String urlPre)
        {
            Cells cells = sheet6.Cells;
            Workbook book1 = new Workbook();
            Style style1 = book1.Styles[book1.Styles.Add()];
            style1.HorizontalAlignment = TextAlignmentType.Center;//文字居中
            style1.VerticalAlignment = TextAlignmentType.Center;
            style1.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin; //应用边界线 左边界线  
            style1.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin; //应用边界线 右边界线  
            style1.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin; //应用边界线 上边界线  
            style1.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin; //应用边界线 下边界线
            #region 模块1
            String CalculateFormula = "=";
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
            SELECT [TB_SQM_Member_Vendor_Map].VendorCode
                  ,[CompanyName]
                  ,[CompanyAddress]
                  ,[FactoryName]
                  ,[FactoryAddress]
                  ,[TB_SQM_Commodity_SubCID]
                  ,[TB_SQM_Commodity_Sub].CNAME
                  ,[DateInfo]
                  ,[ProvidedName]
                  ,[JobTitle]
                  ,[EnterpriseCategory]
                  ,[OwnerShip]
                  ,[FoundedYear]
                  ,[LastRevenues1]
                  ,[LastRevenues2]
                  ,[LastRevenues3]
                  ,[CurrentRevenues]
                  ,[TurnoverAnalysis]
                  ,([RevenueGrowthRate1]+
                  [RevenueGrowthRate2]+
                  [RevenueGrowthRate3])/3 as [RevenueGrowthRate]
                  ,([GrossProfitRate1]+
                  [GrossProfitRate2]+
                  [GrossProfitRate3])/3 as [GrossProfitRate]
                  ,[PlanInvestCapital]
                  ,[BankAndAccNumber]
                  ,[TradingCurrency]
                  ,[TradeMode]
                  ,[VMIManageModel]
                  ,[Distance]
                  ,[MinMonthStateDays]
                  ,[BU1Turnover]
                  ,[BU2Turnover]
                  ,[BU3Turnover]
                  ,[CompanyAdvantage]
              FROM [dbo].[TB_SQM_Manufacturers_BasicInfo]
              inner join [dbo].[TB_SQM_Commodity_Sub]
              on [TB_SQM_Commodity_Sub].[CID]=[TB_SQM_Manufacturers_BasicInfo].[TB_SQM_Commodity_SubCID]
              and  [TB_SQM_Commodity_Sub].[TB_SQM_CommodityCID]=[TB_SQM_Manufacturers_BasicInfo].[TB_SQM_Commodity_SubTB_SQM_CommodityCID]
              inner join [dbo].[TB_SQM_Member_Vendor_Map]
              on [TB_SQM_Member_Vendor_Map].[MemberGUID]=[TB_SQM_Manufacturers_BasicInfo].[VendorCode]
               where BasicInfoGUID=@BasicInfoGUID
              ");

            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                String C6 = dr["VendorCode"] == DBNull.Value ? "" : dr["VendorCode"].ToString();
                cells[5, 2].PutValue(C6);
                String C7 = dr["CompanyName"] == DBNull.Value ? "" : dr["CompanyName"].ToString();
                cells[6, 2].PutValue(C7);
                String C8 = dr["CompanyAddress"] == DBNull.Value ? "" : dr["CompanyAddress"].ToString();
                cells[7, 2].PutValue(C8);
                //String C9 = dr["FactoryName"] == DBNull.Value ? "" : dr["FactoryName"].ToString();
                //cells[8, 2].PutValue(C9);
                //String C10 = dr["FactoryAddress"] == DBNull.Value ? "" : dr["FactoryAddress"].ToString();
                //cells[9, 2].PutValue(C10);
                String C9 = dr["TB_SQM_Commodity_SubCID"] == DBNull.Value ? "" : dr["TB_SQM_Commodity_SubCID"].ToString();
                cells[8, 2].PutValue(C9);
                String C10 = dr["CNAME"] == DBNull.Value ? "" : dr["CNAME"].ToString();
                cells[9, 2].PutValue(C10);
                String C11 = dr["DateInfo"] == DBNull.Value ? "" : dr["DateInfo"].ToString();
                cells[10, 2].PutValue(C11);
                String C12 = dr["ProvidedName"] == DBNull.Value ? "" : dr["ProvidedName"].ToString();
                cells[11, 2].PutValue(C12);
                String C13 = dr["JobTitle"] == DBNull.Value ? "" : dr["JobTitle"].ToString();
                cells[12, 2].PutValue(C13);
                String C16 = dr["EnterpriseCategory"] == DBNull.Value ? "" : dr["EnterpriseCategory"].ToString();
                cells[15, 2].PutValue(C16);
                String C17 = dr["OwnerShip"] == DBNull.Value ? "" : dr["OwnerShip"].ToString();
                cells[16, 2].PutValue(C17);
                String C18 = dr["FoundedYear"] == DBNull.Value ? "" : dr["FoundedYear"].ToString();
                cells[17, 2].PutValue(C18);
                double C19 = dr["LastRevenues1"] == DBNull.Value ? 0 : double.Parse(dr["LastRevenues1"].ToString());
                cells[18, 2].PutValue(C19);
                double D19 = dr["LastRevenues2"] == DBNull.Value ? 0 : double.Parse(dr["LastRevenues2"].ToString());
                cells[18, 3].PutValue(D19);
                double E19 = dr["LastRevenues3"] == DBNull.Value ? 0 : double.Parse(dr["LastRevenues3"].ToString());
                cells[18, 4].PutValue(E19);
                CalculateFormula = CalculateFormula  + CellsHelper.CellIndexToName(18,6);
                String C20 = dr["CurrentRevenues"] == DBNull.Value ? "" : dr["CurrentRevenues"].ToString();
                cells[19, 2].PutValue(C20);
                String C21 = dr["TurnoverAnalysis"] == DBNull.Value ? "" : dr["TurnoverAnalysis"].ToString();
                cells[20, 2].PutValue(C21);
                String C22 = dr["RevenueGrowthRate"] == DBNull.Value ? "" : dr["RevenueGrowthRate"].ToString();
                cells[21, 2].PutValue(C22);
                String C23 = dr["GrossProfitRate"] == DBNull.Value ? "" : dr["GrossProfitRate"].ToString();
                cells[22, 2].PutValue(C23);
                String C24 = dr["PlanInvestCapital"] == DBNull.Value ? "" : dr["PlanInvestCapital"].ToString();
                cells[23, 2].PutValue(C24);
                String C25 = dr["BankAndAccNumber"] == DBNull.Value ? "" : dr["BankAndAccNumber"].ToString();
                cells[24, 2].PutValue(C25);
                String C26 = dr["TradingCurrency"] == DBNull.Value ? "" : dr["TradingCurrency"].ToString();
                cells[25, 2].PutValue(C26);

                String C27 = dr["TradeMode"] == DBNull.Value ? "" : dr["TradeMode"].ToString();
                cells[26, 2].PutValue(C27);

                double C28 = dr["VMIManageModel"] == DBNull.Value ?  0: double.Parse(dr["VMIManageModel"].ToString());
                cells[27, 2].PutValue(C28);
                CalculateFormula = CalculateFormula + "+" + CellsHelper.CellIndexToName(27, 6);
                double C29 = dr["Distance"] == DBNull.Value ? 0 : double.Parse(dr["Distance"].ToString());
                cells[28, 2].PutValue(C29);
                CalculateFormula = CalculateFormula + "-" + CellsHelper.CellIndexToName(28, 6);
                double C30 = dr["MinMonthStateDays"] == DBNull.Value ? 0 : double.Parse(dr["MinMonthStateDays"].ToString());
                cells[29, 2].PutValue(C30);
                CalculateFormula = CalculateFormula + "+" + CellsHelper.CellIndexToName(29, 6);
                double C32 = dr["BU1Turnover"] == DBNull.Value ? 0 : double.Parse(dr["BU1Turnover"].ToString());
                cells[31, 2].PutValue(C32);
                double D32 = dr["BU2Turnover"] == DBNull.Value ? 0 : double.Parse(dr["BU2Turnover"].ToString());
                cells[31, 3].PutValue(D32);
                double E32 = dr["BU3Turnover"] == DBNull.Value ? 0: double.Parse(dr["BU3Turnover"].ToString());
                cells[31, 4].PutValue(E32);
                CalculateFormula = CalculateFormula + "+" + CellsHelper.CellIndexToName(30, 6);
                String C33 = dr["CompanyAdvantage"] == DBNull.Value ? "" : dr["CompanyAdvantage"].ToString();
                cells[32, 2].PutValue(C33);
            }
            #endregion
            #region 模块2
            dt = new DataTable();
            sb = new StringBuilder();
            sb.Append(@"SELECT TOP 4  [PrincipalProducts]
      ,[RevenuePer]
      ,[MOQ]
      ,[SampleTime]
      ,[LeadTime]
      ,[ProductBrand]
      ,SupAndOriName
       ,SupAndOriPlace
      , OfferProxyCertify
      ,UploadCertifyPath 
      ,[MajorCompetitor]
  FROM [TB_SQM_ProductDescription]
    where BasicInfoGUID = @BasicInfoGUID");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            CalculateFormula = CalculateFormula + "-" + CellsHelper.CellIndexToName(39, 6);
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C1 = dr["PrincipalProducts"] == DBNull.Value ? "" : dr["PrincipalProducts"].ToString();
                    cells[36, 2 + i].PutValue(C1);
                    String C2 = dr["RevenuePer"] == DBNull.Value ? "" : dr["RevenuePer"].ToString();
                    cells[37, 2 + i].PutValue(C2);
                    String C3 = dr["MOQ"] == DBNull.Value ? "" : dr["MOQ"].ToString();
                    cells[38, 2 + i].PutValue(C3);
                    double C4 = dr["SampleTime"] == DBNull.Value ? 0:double.Parse( dr["SampleTime"].ToString());
                    cells[39, 2 + i].PutValue(C4);
                   
                    String C5 = dr["LeadTime"] == DBNull.Value ? "" : dr["LeadTime"].ToString();
                    cells[40, 2 + i].PutValue(C5);
                    String C6 = dr["ProductBrand"] == DBNull.Value ? "" : dr["ProductBrand"].ToString();
                    cells[41, 2 + i].PutValue(C6);
                    String C7 = dr["SupAndOriName"] == DBNull.Value ? "" : dr["SupAndOriName"].ToString();
                    cells[42, 2 + i].PutValue(C7);
                    String C8 = dr["SupAndOriPlace"] == DBNull.Value ? "" : dr["SupAndOriPlace"].ToString();
                    cells[43, 2 + i].PutValue(C8);
                    String C9 = dr["OfferProxyCertify"] == DBNull.Value ? "" : dr["OfferProxyCertify"].ToString();
                    cells[44, 2 + i].PutValue(C9);
                    String C10 = dr["UploadCertifyPath"] == DBNull.Value ? "" : dr["UploadCertifyPath"].ToString();
                    cells[45, 2 + i].PutValue(C10);
                    String C11 = dr["MajorCompetitor"] == DBNull.Value ? "" : dr["MajorCompetitor"].ToString();
                    cells[46, 2 + i].PutValue(C11);
                }
                catch (Exception ex)
                {

                }
            }
            #endregion
            #region 模块3
            dt = new DataTable();
            sb = new StringBuilder();
            sb.Append(@"
   SELECT
       [OEMCustomerName]
      ,[BusinessCategory]
      ,[RevenuePer]
  FROM[dbo].[TB_SQM_Customers]
   where BasicInfoGUID=@BasicInfoGUID
           ");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C1 = dr["OEMCustomerName"] == DBNull.Value ? "" : dr["OEMCustomerName"].ToString();
                    cells[49, 2 + i].PutValue(C1);
                    String C2 = dr["BusinessCategory"] == DBNull.Value ? "" : dr["BusinessCategory"].ToString();
                    cells[50, 2 + i].PutValue(C2);
                    String C3 = dr["RevenuePer"] == DBNull.Value ? "" : dr["RevenuePer"].ToString();
                    cells[51, 2 + i].PutValue(C3);
                }
                catch (Exception ex)
                {

                }
            }
            #endregion
            #region 模块4
            dt = new DataTable();
            sb = new StringBuilder();
            sb.Append(@"
            SELECT  [CNAME],
      [EmployeeQty]
      ,[EmployeePlanned]
      ,[AverageJobSeniority]
  FROM [TB_SQM_HR]
  inner join [dbo].[TB_SQM_HR_TYPE] on [TB_SQM_HR_TYPE].[CID]=[TB_SQM_HR].[TB_SQM_HR_TYPECID]
    where BasicInfoGUID=@BasicInfoGUID
    order by [TB_SQM_HR_TYPECID]
             ");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells.InsertRows(55, dt.Rows.Count);
                cells.CopyRows(cells, 55, 56, dt.Rows.Count - 1);
            }
            int row = 55 + (dt.Rows.Count);
            CalculateFormula = CalculateFormula + "+" + CellsHelper.CellIndexToName(54, 6);
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C0 = "4.0" + (i + 2);
                    cells[55 + i, 0].PutValue(C0);
                    String C1 = dr["CNAME"] == DBNull.Value ? "" : dr["CNAME"].ToString();
                    cells[55 + i, 1].PutValue(C1);
                    double C2 = dr["EmployeeQty"] == DBNull.Value ? 0 : double.Parse(dr["EmployeeQty"].ToString());
                    cells[55 + i, 2].PutValue(C2);
                    String C3 = dr["EmployeePlanned"] == DBNull.Value ? "" : dr["EmployeePlanned"].ToString();
                    cells[55 + i, 3].PutValue(C3);
                    String C4 = dr["AverageJobSeniority"] == DBNull.Value ? "" : dr["AverageJobSeniority"].ToString();
                    cells[55 + i, 4].PutValue(C4);
                    cells[55 + i, 6].Formula ="="+ CellsHelper.CellIndexToName(55+i, 2);
                    CalculateFormula = CalculateFormula + "+" + CellsHelper.CellIndexToName(55+i, 6);
                }
                catch (Exception ex)
                {

                }
               
            }
            if (dt.Rows.Count>0)
            {
                cells[54, 2].Formula = "SUM(" + CellsHelper.CellIndexToName(55, 2) + ":" + CellsHelper.CellIndexToName(row - 1, 2) + ")";
                cells[54, 6].Formula = "=" + CellsHelper.CellIndexToName(54, 2);
            }
            #endregion
            #region 模块5
            dt = new DataTable();
            sb = new StringBuilder();
            sb.Append(@" 
                SELECT [CNAME]
                ,[CertificationAuthority]
                ,[CertificationNum]
                ,[CertificationDate]
                ,[ValidDate]
             ,[CertificateImageFGUID]
            FROM[TB_SQM_Certifications]
            inner join[TB_SQM_Certifications_Type] on[TB_SQM_Certifications_Type].CID =[TB_SQM_Certifications].[TB_SQM_Certifications_TypeCID]
             where BasicInfoGUID = @BasicInfoGUID");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            row = row + 2;
            if (dt.Rows.Count > 1)
            {
                cells.InsertRows(row + 1, dt.Rows.Count - 1);
                cells.CopyRows(cells, row + 1, row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C0 = "5.0" + (i + 1);
                    cells[row + i, 0].PutValue(C0);
                    String C1 = dr["CNAME"] == DBNull.Value ? "" : dr["CNAME"].ToString();
                    cells[row + i, 1].PutValue(C1);
                    String C2 = dr["CertificationAuthority"] == DBNull.Value ? "" : dr["CertificationAuthority"].ToString();
                    cells[row + i, 2].PutValue(C2);
                    String C3 = dr["CertificationNum"] == DBNull.Value ? "" : dr["CertificationNum"].ToString();
                    cells[row + i, 3].PutValue(C3);
                    String C4 = dr["CertificationDate"] == DBNull.Value ? "" : dr["CertificationDate"].ToString();
                    cells[row + i, 4].PutValue(C4);
                    String C5 = dr["ValidDate"] == DBNull.Value ? "" : dr["ValidDate"].ToString();
                    cells[row + i, 5].PutValue(C5);
                    String C6 = dr["CertificateImageFGUID"] == DBNull.Value ? "" : dr["CertificateImageFGUID"].ToString();
                    String url = urlPre + "/SQMBasic/DownloadSQMFile?DataKey=" + C6;
                    cells[row + i, 6].PutValue(C6);
                    sheet6.Hyperlinks.Add(row + i, 6, 1, 1, url);
                }
                catch (Exception ex)
                {

                }
            }
            if (dt.Rows.Count>0)
            {
                cells[row + dt.Rows.Count, 2].PutValue(dt.Rows.Count);
                CalculateFormula = CalculateFormula + "+" + CellsHelper.CellIndexToName(row + dt.Rows.Count, 6);
            }
            else
            {
                cells[row + 1, 2].PutValue(dt.Rows.Count);
                CalculateFormula = CalculateFormula + "+" + CellsHelper.CellIndexToName(row + 1, 6);
            }
            cells[14, 6].Formula = CalculateFormula;
            #endregion
        }

        public static void CreatManufacturerExcel(String filename, SqlConnection sqlConnection, String BasicInfoGUID, String localPath, String urlPre)
        {
            License lic = new License();
            //String AsposeLicPath = "E:\\TW VMI V2\\Portal_Web\\Source\\Aspose.Cells\\Aspose.Cells.lic";
            //lic.SetLicense(AsposeLicPath);
            lic.SetLicense(localPath + @"Source\Aspose.Cells\Aspose.Cells.lic");
            String templatePath = localPath + @"Source\Template\manufacturer.xlsx";
            Workbook book = new Workbook(templatePath);
            Worksheet sheet0 = book.Worksheets[0];
            Worksheet sheet1 = book.Worksheets[1];
            Worksheet sheet2 = book.Worksheets[2];
            BindManufacturerReport(ref sheet0, ref sheet1, ref sheet2, sqlConnection, BasicInfoGUID, urlPre);
            book.CalculateFormula();
            object i=  book.Worksheets[0].Cells[17, 6].Value;
            EditScore(sqlConnection, BasicInfoGUID, i);
            book.Save(filename, SaveFormat.Xlsx);

        }
        public static void BindManufacturerReport(ref Worksheet sheet0, ref Worksheet sheet1, ref Worksheet sheet2, SqlConnection sqlConnection, String BasicInfoGUID,String urlPre)
        {
            Cells cells = sheet0.Cells;
            Cells cells1 = sheet1.Cells;
            Cells cells2 = sheet2.Cells;
            String Formula = string.Empty;
            Workbook book1 = new Workbook();
            Style style1 = book1.Styles[book1.Styles.Add()];
            style1.HorizontalAlignment = TextAlignmentType.Center;//文字居中
            style1.VerticalAlignment = TextAlignmentType.Center;
            style1.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin; //应用边界线 左边界线  
            style1.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin; //应用边界线 右边界线  
            style1.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin; //应用边界线 上边界线  
            style1.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin; //应用边界线 下边界线
            String CalculateFormula = "=";
            #region sheet0
            #region  表头以及模块1
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
               SELECT [TB_SQM_Member_Vendor_Map].VendorCode
                  ,[CompanyName]
                  ,[CompanyAddress]
                  ,[FactoryName]
                  ,[FactoryAddress]
                  ,[TB_SQM_Commodity_SubCID]
                  ,[TB_SQM_Commodity_Sub].CNAME
                  ,[DateInfo]
                  ,[ProvidedName]
                  ,[JobTitle]
                  ,[EnterpriseCategory]
                  ,[OwnerShip]
                  ,[FoundedYear]
                  ,[LastRevenues1]
                  ,[LastRevenues2]
                  ,[LastRevenues3]
                  ,[CurrentRevenues]
                  ,[TurnoverAnalysis]
                  ,([RevenueGrowthRate1]+
                  [RevenueGrowthRate2]+
                  [RevenueGrowthRate3])/3 as [RevenueGrowthRate]
                  ,([GrossProfitRate1]+
                  [GrossProfitRate2]+
                  [GrossProfitRate3])/3 as [GrossProfitRate]
                  ,[PlanInvestCapital]
                  ,[BankAndAccNumber]
                  ,[TradingCurrency]
                  ,[TradeMode]
                  ,[VMIManageModel]
                  ,[Distance]
                  ,[MinMonthStateDays]
                  ,[BU1Turnover]
                  ,[BU2Turnover]
                  ,[BU3Turnover]
                  ,[CompanyAdvantage]
              FROM [dbo].[TB_SQM_Manufacturers_BasicInfo]
              inner join [dbo].[TB_SQM_Commodity_Sub]
              on [TB_SQM_Commodity_Sub].[CID]=[TB_SQM_Manufacturers_BasicInfo].[TB_SQM_Commodity_SubCID]
              and  [TB_SQM_Commodity_Sub].[TB_SQM_CommodityCID]=[TB_SQM_Manufacturers_BasicInfo].[TB_SQM_Commodity_SubTB_SQM_CommodityCID]
              inner join [dbo].[TB_SQM_Member_Vendor_Map]
              on convert(nvarchar(50) ,[TB_SQM_Member_Vendor_Map].[MemberGUID])=convert(nvarchar(50),[TB_SQM_Manufacturers_BasicInfo].[VendorCode])
               where BasicInfoGUID=@BasicInfoGUID
              ");

            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            CalculateFormula = CalculateFormula + CellsHelper.CellIndexToName(21, 6);
            CalculateFormula = CalculateFormula +"+"+ CellsHelper.CellIndexToName(30, 6);
            CalculateFormula = CalculateFormula +"-" + CellsHelper.CellIndexToName(31, 6);
            CalculateFormula = CalculateFormula + "+" + CellsHelper.CellIndexToName(32, 6);
            CalculateFormula = CalculateFormula + "+" + CellsHelper.CellIndexToName(33, 6);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                String C6 = dr["VendorCode"] == DBNull.Value ? "" : dr["VendorCode"].ToString();
                cells[5, 2].PutValue(C6);
                String C7 = dr["CompanyName"] == DBNull.Value ? "" : dr["CompanyName"].ToString();
                cells[6, 2].PutValue(C7);
                String C8 = dr["CompanyAddress"] == DBNull.Value ? "" : dr["CompanyAddress"].ToString();
                cells[7, 2].PutValue(C8);
                String C9 = dr["FactoryName"] == DBNull.Value ? "" : dr["FactoryName"].ToString();
                cells[8, 2].PutValue(C9);
                String C10 = dr["FactoryAddress"] == DBNull.Value ? "" : dr["FactoryAddress"].ToString();
                cells[9, 2].PutValue(C10);
                String C11 = dr["TB_SQM_Commodity_SubCID"] == DBNull.Value ? "" : dr["TB_SQM_Commodity_SubCID"].ToString();
                cells[10, 2].PutValue(C11);
                String C12 = dr["CNAME"] == DBNull.Value ? "" : dr["CNAME"].ToString();
                cells[11, 2].PutValue(C12);
                String C13 = dr["DateInfo"] == DBNull.Value ? "" : dr["DateInfo"].ToString();
                cells[12, 2].PutValue(C13);
                String C14 = dr["ProvidedName"] == DBNull.Value ? "" : dr["ProvidedName"].ToString();
                cells[13, 2].PutValue(C14);
                String C15 = dr["JobTitle"] == DBNull.Value ? "" : dr["JobTitle"].ToString();
                cells[14, 2].PutValue(C15);
                String C19 = dr["EnterpriseCategory"] == DBNull.Value ? "" : dr["EnterpriseCategory"].ToString();
                cells[18, 2].PutValue(C19);
                String C20 = dr["OwnerShip"] == DBNull.Value ? "" : dr["OwnerShip"].ToString();
                cells[19, 2].PutValue(C20);
                String C21 = dr["FoundedYear"] == DBNull.Value ? "" : dr["FoundedYear"].ToString();
                cells[20, 2].PutValue(C21);
                double C22 = dr["LastRevenues1"] == DBNull.Value ? 0 :double.Parse( dr["LastRevenues1"].ToString());
                cells[21, 2].PutValue(C22);
                double D22 = dr["LastRevenues2"] == DBNull.Value ? 0 :double.Parse( dr["LastRevenues2"].ToString());
                cells[21, 3].PutValue(D22);
                double E22 = dr["LastRevenues3"] == DBNull.Value ? 0 :double.Parse( dr["LastRevenues3"].ToString());
                cells[21, 4].PutValue(E22);
                String C23 = dr["CurrentRevenues"] == DBNull.Value ? "" : dr["CurrentRevenues"].ToString();
                cells[22, 2].PutValue(C23);
                String C24 = dr["TurnoverAnalysis"] == DBNull.Value ? "" : dr["TurnoverAnalysis"].ToString();
                cells[23, 2].PutValue(C24);
                String C25 = dr["RevenueGrowthRate"] == DBNull.Value ? "" : dr["RevenueGrowthRate"].ToString();
                cells[24, 2].PutValue(C25);
                String C26 = dr["GrossProfitRate"] == DBNull.Value ? "" : dr["GrossProfitRate"].ToString();
                cells[25, 2].PutValue(C26);
                String C27 = dr["PlanInvestCapital"] == DBNull.Value ? "" : dr["PlanInvestCapital"].ToString();
                cells[26, 2].PutValue(C27);
                String C28 = dr["BankAndAccNumber"] == DBNull.Value ? "" : dr["BankAndAccNumber"].ToString();
                cells[27, 2].PutValue(C28);
                String C29 = dr["TradingCurrency"] == DBNull.Value ? "" : dr["TradingCurrency"].ToString();
                cells[28, 2].PutValue(C29);

                String C30 = dr["TradeMode"] == DBNull.Value ? "" : dr["TradeMode"].ToString();
                cells[29, 2].PutValue(C30);

                double C31 = dr["VMIManageModel"] == DBNull.Value ? 0 :double.Parse( dr["VMIManageModel"].ToString());
                cells[30, 2].PutValue(C31);
                double C32 = dr["Distance"] == DBNull.Value ? 0 : double.Parse(dr["Distance"].ToString());
                cells[31, 2].PutValue(C32);
                double C33 = dr["MinMonthStateDays"] == DBNull.Value ? 0 : double.Parse(dr["MinMonthStateDays"].ToString());
                cells[32, 2].PutValue(C33);
                double C35 = dr["BU1Turnover"] == DBNull.Value ? 0 : double.Parse(dr["BU1Turnover"].ToString());
                cells[34, 2].PutValue(C35);
                double D35 = dr["BU2Turnover"] == DBNull.Value ?0 : double.Parse(dr["BU2Turnover"].ToString());
                cells[34, 3].PutValue(D35);
                double E35 = dr["BU3Turnover"] == DBNull.Value ? 0 : double.Parse(dr["BU3Turnover"].ToString());
                cells[34, 4].PutValue(E35);
                String C36 = dr["CompanyAdvantage"] == DBNull.Value ? "" : dr["CompanyAdvantage"].ToString();
                cells[35, 2].PutValue(C36);
            }
            #endregion
            #region 模块2
            dt = new DataTable();
            sb = new StringBuilder();
            sb.Append(@"
SELECT  [PrincipalProducts]
      ,[RevenuePer]
      ,[MOQ]
      ,[SampleTime]
      ,[LeadTime]
      ,AnnualCapacity
      ,[CurrentCapacitySpace]
      ,[MajorCompetitor]
  FROM [TB_SQM_ProductDescription]
    where BasicInfoGUID=@BasicInfoGUID
         ");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            CalculateFormula = CalculateFormula + "-" + CellsHelper.CellIndexToName(42, 6);
            CalculateFormula = CalculateFormula + "+" + CellsHelper.CellIndexToName(45, 6);
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C1 = dr["PrincipalProducts"] == DBNull.Value ? "" : dr["PrincipalProducts"].ToString();
                    cells[39, 2 + i].PutValue(C1);
                    String C2 = dr["RevenuePer"] == DBNull.Value ? "" : dr["RevenuePer"].ToString();
                    cells[40, 2 + i].PutValue(C2);
                    String C3 = dr["MOQ"] == DBNull.Value ? "" : dr["MOQ"].ToString();
                    cells[41, 2 + i].PutValue(C3);
                    double C4 = dr["SampleTime"] == DBNull.Value ? 0 :double.Parse( dr["SampleTime"].ToString());
                    cells[42, 2 + i].PutValue(C4);
                    String C5 = dr["LeadTime"] == DBNull.Value ? "" : dr["LeadTime"].ToString();
                    cells[43, 2 + i].PutValue(C5);
                    String C6 = dr["AnnualCapacity"] == DBNull.Value ? "" : dr["AnnualCapacity"].ToString();
                    cells[44, 2 + i].PutValue(C6);
                    double C7 = dr["CurrentCapacitySpace"] == DBNull.Value ? 0 :double.Parse( dr["CurrentCapacitySpace"].ToString());
                    cells[45, 2 + i].PutValue(C7);
                    String C8 = dr["MajorCompetitor"] == DBNull.Value ? "" : dr["MajorCompetitor"].ToString();
                    cells[46, 2 + i].PutValue(C8);
                    if (i == 3)
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {

                }
            }
            #endregion
            #region 模块3
            dt = new DataTable();
            sb = new StringBuilder();
            sb.Append(@"
                SELECT
       [OEMCustomerName]
      ,[BusinessCategory]
      ,[RevenuePer]
      ,[MajorMaterials]
      ,[MajorSupplier]
      ,[PurchaseRevenuePer]
      ,[HPVendorNum]
  FROM[dbo].[TB_SQM_Customers]
   where BasicInfoGUID=@BasicInfoGUID
                ");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            CalculateFormula = CalculateFormula + "-" + CellsHelper.CellIndexToName(55, 6);
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C1 = dr["OEMCustomerName"] == DBNull.Value ? "" : dr["OEMCustomerName"].ToString();
                    cells[49, 2 + i].PutValue(C1);
                    String C2 = dr["BusinessCategory"] == DBNull.Value ? "" : dr["BusinessCategory"].ToString();
                    cells[50, 2 + i].PutValue(C2);
                    String C3 = dr["RevenuePer"] == DBNull.Value ? "" : dr["RevenuePer"].ToString();
                    cells[51, 2 + i].PutValue(C3);
                    String C4 = dr["MajorMaterials"] == DBNull.Value ? "" : dr["MajorMaterials"].ToString();
                    cells[52, 2 + i].PutValue(C4);
                    String C5 = dr["MajorSupplier"] == DBNull.Value ? "" : dr["MajorSupplier"].ToString();
                    cells[53, 2 + i].PutValue(C5);
                    String C6 = dr["PurchaseRevenuePer"] == DBNull.Value ? "" : dr["PurchaseRevenuePer"].ToString();
                    cells[54, 2 + i].PutValue(C6);
                    double C7 = dr["HPVendorNum"] == DBNull.Value ? 0 :double.Parse( dr["HPVendorNum"].ToString());
                    cells[55, 2 + i].PutValue(C7);
                    if (i == 2)
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {

                }
            }
            #endregion
            #region 模块4
            dt = new DataTable();
            sb = new StringBuilder();
            sb.Append(@"
SELECT  [CNAME],
      [EmployeeQty]
      ,[EmployeePlanned]
      ,[AverageJobSeniority]
  FROM [TB_SQM_HR]
  inner join [dbo].[TB_SQM_HR_TYPE] on [TB_SQM_HR_TYPE].[CID]=[TB_SQM_HR].[TB_SQM_HR_TYPECID]
    where BasicInfoGUID=@BasicInfoGUID
    order by [TB_SQM_HR_TYPECID]
");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells.InsertRows(59, dt.Rows.Count );
                cells.CopyRows(cells, 59, 60, dt.Rows.Count - 1);
            }
            int row = 59 + (dt.Rows.Count);
            double c2 = 0;
            double c3 = 0;
            CalculateFormula = CalculateFormula + "+" + CellsHelper.CellIndexToName(58, 6);
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {

                    DataRow dr = dt.Rows[i];
                    String C0 = "4.0" + (i + 2);
                    cells[59 + i, 0].PutValue(C0);
                    String C1 = dr["CNAME"] == DBNull.Value ? "" : dr["CNAME"].ToString();
                    cells[59 + i, 1].PutValue(C1);
                    double C2 = dr["EmployeeQty"] == DBNull.Value ? 0 : double.Parse( dr["EmployeeQty"].ToString());
                    cells[59 + i, 2].PutValue(C2);
                    c2 =c2+  C2;
                    cells[58, 2].PutValue(c2);
                    double C3 = dr["EmployeePlanned"] == DBNull.Value ? 0 : double.Parse(dr["EmployeePlanned"].ToString());
                    cells[59 + i, 3].PutValue(C3);
                    c3 = c3 + C3;
                    cells[58, 3].PutValue(c3);
                    String C4 = dr["AverageJobSeniority"] == DBNull.Value ? "" : dr["AverageJobSeniority"].ToString();
                    cells[59 + i, 4].PutValue(C4);
                    //CalculateFormula = CalculateFormula + "+" + CellsHelper.CellIndexToName(59 + i, 6);
                }
                catch (Exception ex)
                {

                }
            }
            if (dt.Rows.Count > 0)
            {
                cells[59, 2].Formula = "SUM(" + CellsHelper.CellIndexToName(59, 2) + ":" + CellsHelper.CellIndexToName(row - 1, 2) + ")";
            }
            #endregion
            #region 模块5
            dt = new DataTable();
            sb = new StringBuilder();
            sb.Append(@"
                       SELECT [CNAME]
                ,[CertificationAuthority]
                ,[CertificationNum]
                ,[CertificationDate]
                ,[ValidDate]
             ,[CertificateImageFGUID]
            FROM [TB_SQM_Certifications]
            inner join [TB_SQM_Certifications_Type] on [TB_SQM_Certifications_Type].CID=[TB_SQM_Certifications].[TB_SQM_Certifications_TypeCID]
            where BasicInfoGUID=@BasicInfoGUID
                      ");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            row = row + 2;
            if (dt.Rows.Count > 1)
            {
                cells.InsertRows(row + 1, dt.Rows.Count - 1);
                cells.CopyRows(cells, row + 1, row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {

                    DataRow dr = dt.Rows[i];
                    String C0 = "5.0" + (i + 1);
                    cells[row + i, 0].PutValue(C0);
                    String C1 = dr["CNAME"] == DBNull.Value ? "" : dr["CNAME"].ToString();
                    cells[row + i, 1].PutValue(C1);
                    String C2 = dr["CertificationAuthority"] == DBNull.Value ? "" : dr["CertificationAuthority"].ToString();
                    cells[row + i, 2].PutValue(C2);
                    String C3 = dr["CertificationNum"] == DBNull.Value ? "" : dr["CertificationNum"].ToString();
                    cells[row + i, 3].PutValue(C3);
                    String C4 = dr["CertificationDate"] == DBNull.Value ? "" : dr["CertificationDate"].ToString();
                    cells[row + i, 4].PutValue(C4);
                    String C5 = dr["ValidDate"] == DBNull.Value ? "" : dr["ValidDate"].ToString();
                    cells[row + i, 5].PutValue(C5);
                    String C6 = dr["CertificateImageFGUID"] == DBNull.Value ? "" : dr["CertificateImageFGUID"].ToString();
                    String url = urlPre + "/SQMBasic/DownloadSQMFile?DataKey=" + C6;
                    cells[row + i, 6].PutValue(C6);
                    sheet0.Hyperlinks.Add(row + i, 6, 1, 1, url);

                }
                catch (Exception ex)
                {

                }
            }
            if (dt.Rows.Count > 0)
            {
                cells[row + dt.Rows.Count, 2].PutValue(dt.Rows.Count);
                CalculateFormula = CalculateFormula + "+" + CellsHelper.CellIndexToName(row + dt.Rows.Count, 6);
            }
            else
            {
                cells[row + 1, 2].PutValue(dt.Rows.Count);
                CalculateFormula = CalculateFormula + "+" + CellsHelper.CellIndexToName(row + 1  , 6);
            }
            if (dt.Rows.Count > 1)
            {
                row = row + dt.Rows.Count - 1;
            }
            #endregion
            #region 模块6
            dt = new DataTable();
            sb = new StringBuilder();
            sb.Append(@"
            SELECT top 4 [DeviceName]
      ,[DeviceQuantity]
      ,[DeviceArea]
      ,[DeviceCapacity]
      ,[DevicePrecision]
  FROM [TB_SQM_Equipment]
   where [TB_SQM_Equipment_TypeCID]=1
and BasicInfoGUID=@BasicInfoGUID
            ");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            row = row + 4;
            CalculateFormula = CalculateFormula + "+" + CellsHelper.CellIndexToName(row + 1, 6);
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {

                    DataRow dr = dt.Rows[i];
                    String C1 = dr["DeviceName"] == DBNull.Value ? "" : dr["DeviceName"].ToString();
                    cells[row , 2 + i].PutValue(C1);
                    double C2 = dr["DeviceQuantity"] == DBNull.Value ? 0 :double.Parse( dr["DeviceQuantity"].ToString());
                    cells[row + 1, 2 + i].PutValue(C2);
                    String C3 = dr["DeviceArea"] == DBNull.Value ? "" : dr["DeviceArea"].ToString();
                    cells[row + 2, 2 + i].PutValue(C3);
                    String C4 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells[row + 3, 2 + i].PutValue(C4);
                    String C5 = dr["DevicePrecision"] == DBNull.Value ? "" : dr["DevicePrecision"].ToString();
                    cells[row + 4, 2 + i].PutValue(C5);
                }
                catch (Exception ex)
                {

                }
            }
            CalculateFormula = CalculateFormula + "+" + CellsHelper.CellIndexToName(row + 7, 6);
            row = row + 10;
            #endregion
            #region 模块7
            dt = new DataTable();
            sb = new StringBuilder();
            sb.Append(@"
            SELECT 
     [DeviceName],
[DeviceQuantity],
[DeviceArea],
[TestItem],[DatePurchased]
  FROM [TB_SQM_Equipment]
  where [TB_SQM_Equipment_TypeCID]=2
  and BasicInfoGUID=@BasicInfoGUID
            ");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells.InsertRows(row + 1, dt.Rows.Count - 1);
                cells.CopyRows(cells, row + 1, row + 2, dt.Rows.Count - 2);
            }
            double Num = 0;
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {

                    DataRow dr = dt.Rows[i];
                    String C0 = "7.0" + (i + 1);
                    cells[row + i, 0].PutValue(C0);
                    String C1 = "常规检测设备";
                    cells[row + i, 1].PutValue(C1);
                    String C2 = dr["DeviceName"] == DBNull.Value ? "" : dr["DeviceName"].ToString();
                    cells[row + i, 2].PutValue(C2);
                    double C3 = dr["DeviceQuantity"] == DBNull.Value ? 0 :double.Parse( dr["DeviceQuantity"].ToString());
                    cells[row + i, 3].PutValue(C3);
                    String C4 = dr["DeviceArea"] == DBNull.Value ? "" : dr["DeviceArea"].ToString();
                    cells[row + i, 4].PutValue(C4);
                    String C5 = dr["TestItem"] == DBNull.Value ? "" : dr["TestItem"].ToString();
                    cells[row + i, 5].PutValue(C5);
                    String C6 = dr["DatePurchased"] == DBNull.Value ? "" : dr["DatePurchased"].ToString();
                    cells[row + i, 6].PutValue(C6);
                    Num = Num + C3;
                   
                }
                catch (Exception ex)
                {

                }
            }
            if (dt.Rows.Count > 1)
            {
                row = row + dt.Rows.Count - 1;
            }
            CalculateFormula = CalculateFormula + "+" + CellsHelper.CellIndexToName(row+1 , 6);
            cells[row + 1, 3].PutValue(Num);
            #endregion
            #region 模块8
            dt = new DataTable();
            sb = new StringBuilder();
            sb.Append(@"
                       SELECT 
     [DeviceName],
[DeviceQuantity],
[DeviceArea],
[TestItem],[DatePurchased]
  FROM [TB_SQM_Equipment]
  where [TB_SQM_Equipment_TypeCID]=3
  and BasicInfoGUID=@BasicInfoGUID
            ");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            row = row + 4;
            if (dt.Rows.Count > 1)
            {
                cells.InsertRows(row + 1, dt.Rows.Count - 1);
                cells.CopyRows(cells, row + 1, row + 2, dt.Rows.Count - 2);
            }
            Num = 0;
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {

                    DataRow dr = dt.Rows[i];
                    String C0 = "8.0" + (i + 1);
                    cells[row + i, 0].PutValue(C0);
                    String C1 = "可靠度信赖性测试设备";
                    cells[row + i, 1].PutValue(C1);
                    String C2 = dr["DeviceName"] == DBNull.Value ? "" : dr["DeviceName"].ToString();
                    cells[row + i, 2].PutValue(C2);
                    double C3 = dr["DeviceQuantity"] == DBNull.Value ? 0 :double.Parse( dr["DeviceQuantity"].ToString());
                    cells[row + i, 3].PutValue(C3);
                    String C4 = dr["DeviceArea"] == DBNull.Value ? "" : dr["DeviceArea"].ToString();
                    cells[row + i, 4].PutValue(C4);
                    String C5 = dr["TestItem"] == DBNull.Value ? "" : dr["TestItem"].ToString();
                    cells[row + i, 5].PutValue(C5);
                    String C6 = dr["DatePurchased"] == DBNull.Value ? "" : dr["DatePurchased"].ToString();
                    cells[row + i, 6].PutValue(C6);
                    Num = Num + C3;
                }
                catch (Exception ex)
                {

                }
            }
            if (dt.Rows.Count > 1)
            {
                row = row + dt.Rows.Count - 1;
            }
            CalculateFormula = CalculateFormula + "+" + CellsHelper.CellIndexToName(row +1, 6);
            cells[row + 1, 3].PutValue(Num);
            #endregion
            #region 模块9
            dt = new DataTable();
            sb = new StringBuilder();
            sb.Append(@"
                               SELECT
     [DeviceName],
[DeviceQuantity],
[DeviceArea],
[TestItem],[DatePurchased]
  FROM[TB_SQM_Equipment]
  where[TB_SQM_Equipment_TypeCID] = 4
  and BasicInfoGUID = @BasicInfoGUID
                ");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            row = row + 4;
            if (dt.Rows.Count > 1)
            {
                cells.InsertRows(row + 1, dt.Rows.Count - 1);
                cells.CopyRows(cells, row + 1, row + 2, dt.Rows.Count - 2);
            }
            Num = 0;
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {

                    DataRow dr = dt.Rows[i];
                    String C0 = "9.0" + (i + 1);
                    cells[row + i, 0].PutValue(C0);
                    String C1 = "有害物质测试设备";
                    cells[row + i, 1].PutValue(C1);
                    String C2 = dr["DeviceName"] == DBNull.Value ? "" : dr["DeviceName"].ToString();
                    cells[row + i, 2].PutValue(C2);
                    double C3 = dr["DeviceQuantity"] == DBNull.Value ? 0 :double.Parse( dr["DeviceQuantity"].ToString());
                    cells[row + i, 3].PutValue(C3);
                    String C4 = dr["DeviceArea"] == DBNull.Value ? "" : dr["DeviceArea"].ToString();
                    cells[row + i, 4].PutValue(C4);
                    String C5 = dr["TestItem"] == DBNull.Value ? "" : dr["TestItem"].ToString();
                    cells[row + i, 5].PutValue(C5);
                    String C6 = dr["DatePurchased"] == DBNull.Value ? "" : dr["DatePurchased"].ToString();
                    cells[row + i, 6].PutValue(C6);
                    Num = Num + C3;
                }
                catch (Exception ex)
                {

                }
            }
            if (dt.Rows.Count > 1)
            {
                row = row + dt.Rows.Count - 1;
            }
        
            CalculateFormula = CalculateFormula + "+" + CellsHelper.CellIndexToName(row +1, 6);
            cells[row + 1, 3].PutValue(Num);
            #endregion
            #region 模块10
            dt = new DataTable();
            sb = new StringBuilder();
            sb.Append(@"
                SELECT
     [DeviceName],
[DeviceQuantity],
[DeviceArea],
[DatePurchased], model
  FROM[TB_SQM_Equipment]
  where[TB_SQM_Equipment_TypeCID] = 9
    and BasicInfoGUID = @BasicInfoGUID
                ");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            row = row + 4;
            if (dt.Rows.Count > 1)
            {
                cells.InsertRows(row + 1, dt.Rows.Count - 1);
                cells.CopyRows(cells, row + 1, row + 2, dt.Rows.Count - 2);
            }
            Num = 0;
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {

                    DataRow dr = dt.Rows[i];
                    String C0 = "10.0" + (i + 1);
                    cells[row + i, 0].PutValue(C0);
                    String C1 = dr["DeviceName"] == DBNull.Value ? "" : dr["DeviceName"].ToString();
                    cells[row + i, 1].PutValue(C1);
                    double C2 = dr["DeviceQuantity"] == DBNull.Value ? 0 :double.Parse( dr["DeviceQuantity"].ToString());
                    cells[row + i, 2].PutValue(C2);
                    String C3 = dr["model"] == DBNull.Value ? "" : dr["model"].ToString();
                    cells[row + i, 3].PutValue(C3);
                    String C5 = dr["DeviceArea"] == DBNull.Value ? "" : dr["DeviceArea"].ToString();
                    cells[row + i, 5].PutValue(C5);
                    String C6 = dr["DatePurchased"] == DBNull.Value ? "" : dr["DatePurchased"].ToString();
                    cells[row + i, 6].PutValue(C6);
                    Num = Num + C2;
                }
                catch (Exception ex)
                {

                }
            }
            if (dt.Rows.Count > 1)
            {
                row = row + dt.Rows.Count - 1;
            }
            cells[row + 1, 2].PutValue(Num);
            #endregion
            #region 模块11
            dt = new DataTable();
            sb = new StringBuilder();
            sb.Append(@"
            SELECT [CNAME]
      ,[ProcessName]
      ,[OwnOrOut]
      ,[ExternalSupplierName]
  FROM [TB_SQM_Process]
  inner join [dbo].[TB_SQM_Process_Type] on [TB_SQM_Process_Type].cid=[TB_SQM_Process].[TB_SQM_Process_TypeCID]
    where BasicInfoGUID = @BasicInfoGUID
            ");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            row = row + 4;
            CalculateFormula = CalculateFormula + "+" + CellsHelper.CellIndexToName(row , 6);
            CalculateFormula = CalculateFormula + "+" + CellsHelper.CellIndexToName(row +1, 6);
            CalculateFormula = CalculateFormula + "+" + CellsHelper.CellIndexToName(row +2, 6);
            CalculateFormula = CalculateFormula + "+" + CellsHelper.CellIndexToName(row + 3, 6);
            CalculateFormula = CalculateFormula + "+" + CellsHelper.CellIndexToName(row + 4, 6);
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    if (i > 31)
                    {
                        cells.InsertRows(row + 1, 1);
                        cells.CopyRows(cells, row + 1, row + 2, dt.Rows.Count - 2);
                    }
                    DataRow dr = dt.Rows[i];
                    String C0 = "11.0" + (i + 1);
                    cells[row + i, 0].PutValue(C0);
                    String C1 = dr["CNAME"] == DBNull.Value ? "" : dr["CNAME"].ToString();
                    cells[row + i, 1].PutValue(C1);
                    String C2 = dr["ProcessName"] == DBNull.Value ? "" : dr["ProcessName"].ToString();
                    cells[row + i, 2].PutValue(C2);
                    String C3 = dr["OwnOrOut"] == DBNull.Value ? "" : dr["OwnOrOut"].ToString();
                    if (C3.Equals("0"))
                    {
                        cells[row + i, 3].PutValue(1);
                    }
                    else
                    {
                        cells[row + i, 4].PutValue(1);
                    }

                    String C5 = dr["ExternalSupplierName"] == DBNull.Value ? "" : dr["ExternalSupplierName"].ToString();
                    cells[row + i, 5].PutValue(C5);

                }
                catch (Exception ex)
                {

                }
            }
            if (dt.Rows.Count < 31)
            {
                row = row + 31;
            }
            else
            {
                row = row + dt.Rows.Count - 1;
            }
            CalculateFormula = CalculateFormula + "-" + CellsHelper.CellIndexToName(row + 1, 6);
            #endregion
            #region 模块12
            dt = new DataTable();
            sb = new StringBuilder();
            sb.Append(@"
           SELECT top 1 [Is3DUG]
      ,[Is3DProE]
      ,[Is2DAutoCAD]
      ,[IsPhotoShop]
      ,[IsIDMapAbility]
      ,[Is3DMapAbility]
      ,[Is2DMapAbility]
      ,[IsMoldflowAbility]
      ,[IsTAAbility]
      ,[IsDesignGuildline]
      ,[IsFMEA]
      ,[IsLessonLearnt]
  FROM [TB_SQM_Manufacturers_BasicInfo]
   where BasicInfoGUID = @BasicInfoGUID
            ");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            row = row + 5;

            try
            {
                DataRow dr = dt.Rows[0];
                for (int i = 0; i < dr.Table.Columns.Count; i++)
                {


                    if (dr[i].ToString().Equals("True"))
                    {
                        cells[row + i, 2].PutValue(1);
                    }
                    else
                    {
                        cells[row + i, 2].PutValue(0);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            CalculateFormula = CalculateFormula + "+" + CellsHelper.CellIndexToName(row + 16, 6);
            cells[17,6].Formula = CalculateFormula;
            #endregion
            #endregion
            #region sheet1 塑料
            #region 注塑机
            int sheet1row = 0;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                        DeviceQuantity,
                        DateMade,
                        Model,
                        Brand,
                        Weight,
                        ModelSize 
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                            and BasicInfoGUID=@BasicInfoGUID
                     ");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 5);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 1);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            sheet1row = 3;
            if (dt.Rows.Count > 1)
            {
                cells1.InsertRows(sheet1row + 1, dt.Rows.Count - 1);
                cells1.CopyRows(cells1, sheet1row + 1, sheet1row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells1[sheet1row + i, 2].PutValue(C2);
                    String C3 = dr["DateMade"] == DBNull.Value ? "" : dr["DateMade"].ToString();
                    cells1[sheet1row + i, 3].PutValue(C3);
                    String C4 = dr["Brand"] == DBNull.Value ? "" : dr["Brand"].ToString();
                    cells1[sheet1row + i, 4].PutValue(C4);
                    String C5 = dr["Weight"] == DBNull.Value ? "" : dr["Weight"].ToString();
                    cells1[sheet1row + i, 5].PutValue(C5);
                    String C6 = dr["ModelSize"] == DBNull.Value ? "" : dr["ModelSize"].ToString();
                    cells1[sheet1row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells1[sheet1row + i, 7].PutValue(C7);




                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet1row, 7) + ":" + CellsHelper.CellIndexToName(sheet1row + dt.Rows.Count - 1, 7);
            cells1[sheet1row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet1row = sheet1row + dt.Rows.Count - 1;
            }
            #endregion
            #region 喷涂线
            sheet1row = sheet1row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                        DeviceQuantity,
                        DeviceCapacity,
                        Model,
                        LineLength,
                        GunQty,
                        RoastQty 
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                         and BasicInfoGUID=@BasicInfoGUID
              ");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 5);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 2);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells1.InsertRows(sheet1row + 1, dt.Rows.Count - 1);
                cells1.CopyRows(cells1, sheet1row + 1, sheet1row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells1[sheet1row + i, 2].PutValue(C2);
                    String C3 = dr["LineLength"] == DBNull.Value ? "" : dr["LineLength"].ToString();
                    cells1[sheet1row + i, 3].PutValue(C3);
                    String C4 = dr["GunQty"] == DBNull.Value ? "" : dr["GunQty"].ToString();
                    cells1[sheet1row + i, 4].PutValue(C4);
                    String C5 = dr["RoastQty"] == DBNull.Value ? "" : dr["RoastQty"].ToString();
                    cells1[sheet1row + i, 5].PutValue(C5);
                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells1[sheet1row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells1[sheet1row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet1row, 7) + ":" + CellsHelper.CellIndexToName(sheet1row + dt.Rows.Count - 1, 7);
            cells1[sheet1row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet1row = sheet1row + dt.Rows.Count - 1;
            }
            #endregion
            #region 电镀线
            sheet1row = sheet1row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                        DeviceQuantity,
                        DeviceCapacity,
                        Model,
                        PlatingType
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                             and BasicInfoGUID=@BasicInfoGUID
              ");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 5);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 3);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells1.InsertRows(sheet1row + 1, dt.Rows.Count - 1);
                cells1.CopyRows(cells1, sheet1row + 1, sheet1row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells1[sheet1row + i, 2].PutValue(C2);
                    String C3 = dr["LineLength"] == DBNull.Value ? "" : dr["LineLength"].ToString();
                    cells1[sheet1row + i, 3].PutValue(C3);

                    String C5 = dr["PlatingType"] == DBNull.Value ? "" : dr["PlatingType"].ToString();
                    cells1[sheet1row + i, 5].PutValue(C5);
                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells1[sheet1row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells1[sheet1row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet1row, 7) + ":" + CellsHelper.CellIndexToName(sheet1row + dt.Rows.Count - 1, 7);
            cells1[sheet1row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet1row = sheet1row + dt.Rows.Count - 1;
            }
            #endregion
            #region 加工组装线
            sheet1row = sheet1row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                        DeviceQuantity,
                        DeviceCapacity,
                        Model,
                        LineLength
                       
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                         and BasicInfoGUID=@BasicInfoGUID
                            ");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 5);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 4);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells1.InsertRows(sheet1row + 1, dt.Rows.Count - 1);
                cells1.CopyRows(cells1, sheet1row + 1, sheet1row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells1[sheet1row + i, 2].PutValue(C2);
                    String C3 = dr["LineLength"] == DBNull.Value ? "" : dr["LineLength"].ToString();
                    cells1[sheet1row + i, 3].PutValue(C3);

                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells1[sheet1row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells1[sheet1row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet1row, 7) + ":" + CellsHelper.CellIndexToName(sheet1row + dt.Rows.Count - 1, 7);
            cells1[sheet1row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet1row = sheet1row + dt.Rows.Count - 1;
            }
            #endregion
            #region 热熔机
            sheet1row = sheet1row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                        DeviceQuantity,
                        DeviceCapacity,
                        DateMade,
                        Model,
                        Brand
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                         and BasicInfoGUID=@BasicInfoGUID
              ");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 5);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 5);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells1.InsertRows(sheet1row + 1, dt.Rows.Count - 1);
                cells1.CopyRows(cells1, sheet1row + 1, sheet1row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells1[sheet1row + i, 2].PutValue(C2);
                    String C3 = dr["DateMade"] == DBNull.Value ? "" : dr["DateMade"].ToString();
                    cells1[sheet1row + i, 3].PutValue(C3);
                    String C4 = dr["Brand"] == DBNull.Value ? "" : dr["Brand"].ToString();
                    cells1[sheet1row + i, 4].PutValue(C4);

                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells1[sheet1row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells1[sheet1row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet1row, 7) + ":" + CellsHelper.CellIndexToName(sheet1row + dt.Rows.Count - 1, 7);
            cells1[sheet1row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet1row = sheet1row + dt.Rows.Count - 1;
            }
            #endregion
            #region 丝印机
            sheet1row = sheet1row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                        DeviceQuantity,
                        DeviceCapacity,
                        DateMade,
                        Model,
                        Brand
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                        and BasicInfoGUID=@BasicInfoGUID
                     ");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 5);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 6);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells1.InsertRows(sheet1row + 1, dt.Rows.Count - 1);
                cells1.CopyRows(cells1, sheet1row + 1, sheet1row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells1[sheet1row + i, 2].PutValue(C2);
                    String C3 = dr["DateMade"] == DBNull.Value ? "" : dr["DateMade"].ToString();
                    cells1[sheet1row + i, 3].PutValue(C3);
                    String C4 = dr["Brand"] == DBNull.Value ? "" : dr["Brand"].ToString();
                    cells1[sheet1row + i, 4].PutValue(C4);

                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells1[sheet1row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells1[sheet1row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet1row, 7) + ":" + CellsHelper.CellIndexToName(sheet1row + dt.Rows.Count - 1, 7);
            cells1[sheet1row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet1row = sheet1row + dt.Rows.Count - 1;
            }
            #endregion
            #region 移印机
            sheet1row = sheet1row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                     DeviceQuantity,
                        DeviceCapacity,
                        DateMade,
                        Model,
                        Brand
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                             and BasicInfoGUID=@BasicInfoGUID");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 5);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 7);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells1.InsertRows(sheet1row + 1, dt.Rows.Count - 1);
                cells1.CopyRows(cells1, sheet1row + 1, sheet1row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells1[sheet1row + i, 2].PutValue(C2);
                    String C3 = dr["DateMade"] == DBNull.Value ? "" : dr["DateMade"].ToString();
                    cells1[sheet1row + i, 3].PutValue(C3);
                    String C4 = dr["Brand"] == DBNull.Value ? "" : dr["Brand"].ToString();
                    cells1[sheet1row + i, 4].PutValue(C4);

                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells1[sheet1row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells1[sheet1row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet1row, 7) + ":" + CellsHelper.CellIndexToName(sheet1row + dt.Rows.Count - 1, 7);
            cells1[sheet1row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet1row = sheet1row + dt.Rows.Count - 1;
            }
            #endregion
            #region 镭雕机
            sheet1row = sheet1row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                             DeviceQuantity,
                        DeviceCapacity,
                        DateMade,
                        Model,
                        Brand
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                          and BasicInfoGUID=@BasicInfoGUID");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 5);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 8);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells1.InsertRows(sheet1row + 1, dt.Rows.Count - 1);
                cells1.CopyRows(cells1, sheet1row + 1, sheet1row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells1[sheet1row + i, 2].PutValue(C2);
                    String C3 = dr["DateMade"] == DBNull.Value ? "" : dr["DateMade"].ToString();
                    cells1[sheet1row + i, 3].PutValue(C3);
                    String C4 = dr["Brand"] == DBNull.Value ? "" : dr["Brand"].ToString();
                    cells1[sheet1row + i, 4].PutValue(C4);

                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells1[sheet1row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells1[sheet1row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet1row, 7) + ":" + CellsHelper.CellIndexToName(sheet1row + dt.Rows.Count - 1, 7);
            cells1[sheet1row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet1row = sheet1row + dt.Rows.Count - 1;
            }
            #endregion
            #region (量测设备)
            sheet1row = sheet1row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                   DeviceQuantity,
                        DeviceCapacity,
                        DateMade,
                        Model,
                        Brand
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                        and BasicInfoGUID=@BasicInfoGUID");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 5);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 36);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells1.InsertRows(sheet1row + 1, dt.Rows.Count - 1);
                cells1.CopyRows(cells1, sheet1row + 1, sheet1row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells1[sheet1row + i, 2].PutValue(C2);
                    String C3 = dr["DateMade"] == DBNull.Value ? "" : dr["DateMade"].ToString();
                    cells1[sheet1row + i, 3].PutValue(C3);
                    String C4 = dr["Brand"] == DBNull.Value ? "" : dr["Brand"].ToString();
                    cells1[sheet1row + i, 4].PutValue(C4);

                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells1[sheet1row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells1[sheet1row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet1row, 7) + ":" + CellsHelper.CellIndexToName(sheet1row + dt.Rows.Count - 1, 7);
            cells1[sheet1row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet1row = sheet1row + dt.Rows.Count - 1;
            }
            #endregion
            #region (模具房设备)
            sheet1row = sheet1row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                         DeviceQuantity,
                        DeviceCapacity,
                        DateMade,
                        Model,
                        Brand
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                        and BasicInfoGUID=@BasicInfoGUID");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 5);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 37);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells1.InsertRows(sheet1row + 1, dt.Rows.Count - 1);
                cells1.CopyRows(cells1, sheet1row + 1, sheet1row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells1[sheet1row + i, 2].PutValue(C2);
                    String C3 = dr["DateMade"] == DBNull.Value ? "" : dr["DateMade"].ToString();
                    cells1[sheet1row + i, 3].PutValue(C3);
                    String C4 = dr["Brand"] == DBNull.Value ? "" : dr["Brand"].ToString();
                    cells1[sheet1row + i, 4].PutValue(C4);

                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells1[sheet1row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells1[sheet1row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet1row, 7) + ":" + CellsHelper.CellIndexToName(sheet1row + dt.Rows.Count - 1, 7);
            cells1[sheet1row, 8].Formula = "SUM(" + Formula + ")";
            #endregion
            #endregion
            #region sheet2 五金
            #region 冲压机
            int sheet2row = 0;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                        DeviceQuantity,
                        DeviceCapacity,
                        DateMade,
                        Model,
                        Brand,
                        Weight
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                          and BasicInfoGUID=@BasicInfoGUID");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 6);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 9);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            sheet2row = 3;
            if (dt.Rows.Count > 1)
            {
                cells2.InsertRows(sheet2row + 1, dt.Rows.Count - 1);
                cells2.CopyRows(cells2, sheet2row + 1, sheet2row + 2, dt.Rows.Count - 2);
            }

            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells2[sheet2row + i, 2].PutValue(C2);
                    String C3 = dr["DateMade"] == DBNull.Value ? "" : dr["DateMade"].ToString();
                    cells2[sheet2row + i, 3].PutValue(C3);
                    String C4 = dr["Brand"] == DBNull.Value ? "" : dr["Brand"].ToString();
                    cells2[sheet2row + i, 4].PutValue(C4);
                    String C5 = dr["Weight"] == DBNull.Value ? "" : dr["Weight"].ToString();
                    cells2[sheet2row + i, 5].PutValue(C5);
                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells2[sheet2row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells2[sheet2row + i, 7].PutValue(C7);

                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet2row, 7) + ":" + CellsHelper.CellIndexToName(sheet2row + dt.Rows.Count - 1, 7);
            cells2[sheet2row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet2row = sheet2row + dt.Rows.Count - 1;
            }
            #endregion
            #region 连动式机器人冲压平移线
            sheet2row = sheet2row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                         DeviceQuantity,
                        DeviceCapacity,
                        DateMade,
                        Model,
                        Brand
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                           and BasicInfoGUID=@BasicInfoGUID");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 6);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 10);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells2.InsertRows(sheet2row + 1, dt.Rows.Count - 1);
                cells2.CopyRows(cells2, sheet2row + 1, sheet2row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells2[sheet2row + i, 2].PutValue(C2);
                    String C3 = dr["DateMade"] == DBNull.Value ? "" : dr["DateMade"].ToString();
                    cells2[sheet2row + i, 3].PutValue(C3);
                    String C4 = dr["Brand"] == DBNull.Value ? "" : dr["Brand"].ToString();
                    cells2[sheet2row + i, 4].PutValue(C4);

                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells2[sheet2row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells2[sheet2row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet2row, 7) + ":" + CellsHelper.CellIndexToName(sheet2row + dt.Rows.Count - 1, 7);
            cells2[sheet2row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet2row = sheet2row + dt.Rows.Count - 1;
            }
            #endregion

            #region 模切机
            sheet2row = sheet2row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                         DeviceQuantity,
                        DeviceCapacity,
                        DateMade,
                        Model,
                        Brand
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                           and BasicInfoGUID=@BasicInfoGUID"); 
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 6);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 11);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells2.InsertRows(sheet2row + 1, dt.Rows.Count - 1);
                cells2.CopyRows(cells2, sheet2row + 1, sheet2row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells2[sheet2row + i, 2].PutValue(C2);
                    String C3 = dr["DateMade"] == DBNull.Value ? "" : dr["DateMade"].ToString();
                    cells2[sheet2row + i, 3].PutValue(C3);
                    String C4 = dr["Brand"] == DBNull.Value ? "" : dr["Brand"].ToString();
                    cells2[sheet2row + i, 4].PutValue(C4);

                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells2[sheet2row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells2[sheet2row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet2row, 7) + ":" + CellsHelper.CellIndexToName(sheet2row + dt.Rows.Count - 1, 7);
            cells2[sheet2row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet2row = sheet2row + dt.Rows.Count - 1;
            }



            #endregion
            #region 锁螺丝机
            sheet2row = sheet2row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                         DeviceQuantity,
                        DeviceCapacity,
                        DateMade,
                        Model,
                        Brand
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                           and BasicInfoGUID=@BasicInfoGUID"); 
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 6);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 12);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells2.InsertRows(sheet2row + 1, dt.Rows.Count - 1);
                cells2.CopyRows(cells2, sheet2row + 1, sheet2row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells2[sheet2row + i, 2].PutValue(C2);
                    String C3 = dr["DateMade"] == DBNull.Value ? "" : dr["DateMade"].ToString();
                    cells2[sheet2row + i, 3].PutValue(C3);
                    String C4 = dr["Brand"] == DBNull.Value ? "" : dr["Brand"].ToString();
                    cells2[sheet2row + i, 4].PutValue(C4);

                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells2[sheet2row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells2[sheet2row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet2row, 7) + ":" + CellsHelper.CellIndexToName(sheet2row + dt.Rows.Count - 1, 7);
            cells2[sheet2row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet2row = sheet2row + dt.Rows.Count - 1;

            }


            #endregion
            #region 装铆钉机
            sheet2row = sheet2row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                         DeviceQuantity,
                        DeviceCapacity,
                        DateMade,
                        Model,
                        Brand
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                           and BasicInfoGUID=@BasicInfoGUID"); 
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 6);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 13);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells2.InsertRows(sheet2row + 1, dt.Rows.Count - 1);
                cells2.CopyRows(cells2, sheet2row + 1, sheet2row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells2[sheet2row + i, 2].PutValue(C2);
                    String C3 = dr["DateMade"] == DBNull.Value ? "" : dr["DateMade"].ToString();
                    cells2[sheet2row + i, 3].PutValue(C3);
                    String C4 = dr["Brand"] == DBNull.Value ? "" : dr["Brand"].ToString();
                    cells2[sheet2row + i, 4].PutValue(C4);

                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells2[sheet2row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells2[sheet2row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet2row, 7) + ":" + CellsHelper.CellIndexToName(sheet2row + dt.Rows.Count - 1, 7);
            cells2[sheet2row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet2row = sheet2row + dt.Rows.Count - 1;

            }


            #endregion
            #region 装拉钉机

            sheet2row = sheet2row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                         DeviceQuantity,
                        DeviceCapacity,
                        DateMade,
                        Model,
                        Brand
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                           and BasicInfoGUID=@BasicInfoGUID"); 
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 6);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 38);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells2.InsertRows(sheet2row + 1, dt.Rows.Count - 1);
                cells2.CopyRows(cells2, sheet2row + 1, sheet2row + 2, dt.Rows.Count - 2);
            }

            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells2[sheet2row + i, 2].PutValue(C2);
                    String C3 = dr["DateMade"] == DBNull.Value ? "" : dr["DateMade"].ToString();
                    cells2[sheet2row + i, 3].PutValue(C3);
                    String C4 = dr["Brand"] == DBNull.Value ? "" : dr["Brand"].ToString();
                    cells2[sheet2row + i, 4].PutValue(C4);

                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells2[sheet2row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells2[sheet2row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet2row, 7) + ":" + CellsHelper.CellIndexToName(sheet2row + dt.Rows.Count - 1, 7);
            cells2[sheet2row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet2row = sheet2row + dt.Rows.Count - 1;

            }

            #endregion
            #region 自动冲压铆钉识别机

            sheet2row = sheet2row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                         DeviceQuantity,
                        DeviceCapacity,
                        DateMade,
                        Model,
                        Brand
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                           and BasicInfoGUID=@BasicInfoGUID"); 
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 6);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 14);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells2.InsertRows(sheet2row + 1, dt.Rows.Count - 1);
                cells2.CopyRows(cells2, sheet2row + 1, sheet2row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells2[sheet2row + i, 2].PutValue(C2);
                    String C3 = dr["DateMade"] == DBNull.Value ? "" : dr["DateMade"].ToString();
                    cells2[sheet2row + i, 3].PutValue(C3);
                    String C4 = dr["Brand"] == DBNull.Value ? "" : dr["Brand"].ToString();
                    cells2[sheet2row + i, 4].PutValue(C4);

                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells2[sheet2row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells2[sheet2row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet2row, 7) + ":" + CellsHelper.CellIndexToName(sheet2row + dt.Rows.Count - 1, 7);
            cells2[sheet2row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet2row = sheet2row + dt.Rows.Count - 1;

            }

            #endregion
            #region 机械手全自动點焊机


            sheet2row = sheet2row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                         DeviceQuantity,
                        DeviceCapacity,
                        DateMade,
                        Model,
                        Brand
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                           and BasicInfoGUID=@BasicInfoGUID");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 6);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 15);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells2.InsertRows(sheet2row + 1, dt.Rows.Count - 1);
                cells2.CopyRows(cells2, sheet2row + 1, sheet2row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells2[sheet2row + i, 2].PutValue(C2);
                    String C3 = dr["DateMade"] == DBNull.Value ? "" : dr["DateMade"].ToString();
                    cells2[sheet2row + i, 3].PutValue(C3);
                    String C4 = dr["Brand"] == DBNull.Value ? "" : dr["Brand"].ToString();
                    cells2[sheet2row + i, 4].PutValue(C4);

                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells2[sheet2row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells2[sheet2row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet2row, 7) + ":" + CellsHelper.CellIndexToName(sheet2row + dt.Rows.Count - 1, 7);
            cells2[sheet2row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet2row = sheet2row + dt.Rows.Count - 1;

            }
            #endregion
            #region 半自动點焊机

            sheet2row = sheet2row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                         DeviceQuantity,
                        DeviceCapacity,
                        DateMade,
                        Model,
                        Brand
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                           and BasicInfoGUID=@BasicInfoGUID"); 
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 6);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 16);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells2.InsertRows(sheet2row + 1, dt.Rows.Count - 1);
                cells2.CopyRows(cells2, sheet2row + 1, sheet2row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells2[sheet2row + i, 2].PutValue(C2);
                    String C3 = dr["DateMade"] == DBNull.Value ? "" : dr["DateMade"].ToString();
                    cells2[sheet2row + i, 3].PutValue(C3);
                    String C4 = dr["Brand"] == DBNull.Value ? "" : dr["Brand"].ToString();
                    cells2[sheet2row + i, 4].PutValue(C4);

                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells2[sheet2row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells2[sheet2row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet2row, 7) + ":" + CellsHelper.CellIndexToName(sheet2row + dt.Rows.Count - 1, 7);
            cells2[sheet2row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet2row = sheet2row + dt.Rows.Count - 1;

            }

            #endregion
            #region 手动點焊机

            sheet2row = sheet2row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                         DeviceQuantity,
                        DeviceCapacity,
                        DateMade,
                        Model,
                        Brand
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                           and BasicInfoGUID=@BasicInfoGUID"); 
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 6);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 17);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells2.InsertRows(sheet2row + 1, dt.Rows.Count - 1);
                cells2.CopyRows(cells2, sheet2row + 1, sheet2row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells2[sheet2row + i, 2].PutValue(C2);
                    String C3 = dr["DateMade"] == DBNull.Value ? "" : dr["DateMade"].ToString();
                    cells2[sheet2row + i, 3].PutValue(C3);
                    String C4 = dr["Brand"] == DBNull.Value ? "" : dr["Brand"].ToString();
                    cells2[sheet2row + i, 4].PutValue(C4);

                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells2[sheet2row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells2[sheet2row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet2row, 7) + ":" + CellsHelper.CellIndexToName(sheet2row + dt.Rows.Count - 1, 7);
            cells2[sheet2row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet2row = sheet2row + dt.Rows.Count - 1;

            }

            #endregion
            #region 精密车床

            sheet2row = sheet2row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                         DeviceQuantity,
                        DeviceCapacity,
                        DateMade,
                        Model,
                        Brand
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                           and BasicInfoGUID=@BasicInfoGUID"); 
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 6);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 18);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells2.InsertRows(sheet2row + 1, dt.Rows.Count - 1);
                cells2.CopyRows(cells2, sheet2row + 1, sheet2row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells2[sheet2row + i, 2].PutValue(C2);
                    String C3 = dr["DateMade"] == DBNull.Value ? "" : dr["DateMade"].ToString();
                    cells2[sheet2row + i, 3].PutValue(C3);
                    String C4 = dr["Brand"] == DBNull.Value ? "" : dr["Brand"].ToString();
                    cells2[sheet2row + i, 4].PutValue(C4);

                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells2[sheet2row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells2[sheet2row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet2row, 7) + ":" + CellsHelper.CellIndexToName(sheet2row + dt.Rows.Count - 1, 7);
            cells2[sheet2row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet2row = sheet2row + dt.Rows.Count - 1;
            }


            #endregion
            #region 钣金机
            sheet2row = sheet2row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                         DeviceQuantity,
                        DeviceCapacity,
                        DateMade,
                        Model,
                        Brand
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                           and BasicInfoGUID=@BasicInfoGUID"); 
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 6);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 19);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells2.InsertRows(sheet2row + 1, dt.Rows.Count - 1);
                cells2.CopyRows(cells2, sheet2row + 1, sheet2row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells2[sheet2row + i, 2].PutValue(C2);
                    String C3 = dr["DateMade"] == DBNull.Value ? "" : dr["DateMade"].ToString();
                    cells2[sheet2row + i, 3].PutValue(C3);
                    String C4 = dr["Brand"] == DBNull.Value ? "" : dr["Brand"].ToString();
                    cells2[sheet2row + i, 4].PutValue(C4);

                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells2[sheet2row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells2[sheet2row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet2row, 7) + ":" + CellsHelper.CellIndexToName(sheet2row + dt.Rows.Count - 1, 7);
            cells2[sheet2row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet2row = sheet2row + dt.Rows.Count - 1;
            }



            #endregion
            #region 铝锭押出机
            sheet2row = sheet2row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                         DeviceQuantity,
                        DeviceCapacity,
                        DateMade,
                        Model,
                        Brand
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                           and BasicInfoGUID=@BasicInfoGUID"); 
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 6);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 20);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells2.InsertRows(sheet2row + 1, dt.Rows.Count - 1);
                cells2.CopyRows(cells2, sheet2row + 1, sheet2row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells2[sheet2row + i, 2].PutValue(C2);
                    String C3 = dr["DateMade"] == DBNull.Value ? "" : dr["DateMade"].ToString();
                    cells2[sheet2row + i, 3].PutValue(C3);
                    String C4 = dr["Brand"] == DBNull.Value ? "" : dr["Brand"].ToString();
                    cells2[sheet2row + i, 4].PutValue(C4);

                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells2[sheet2row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells2[sheet2row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet2row, 7) + ":" + CellsHelper.CellIndexToName(sheet2row + dt.Rows.Count - 1, 7);
            cells2[sheet2row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet2row = sheet2row + dt.Rows.Count - 1;

            }


            #endregion
            #region 铝剂型机
            sheet2row = sheet2row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                      DeviceQuantity,
                        DeviceCapacity,
                        DateMade,
                        Model,
                        Brand,
                        Weight
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                           and BasicInfoGUID=@BasicInfoGUID");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 6);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 21);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells2.InsertRows(sheet2row + 1, dt.Rows.Count - 1);
                cells2.CopyRows(cells2, sheet2row + 1, sheet2row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells2[sheet2row + i, 2].PutValue(C2);
                    String C3 = dr["DateMade"] == DBNull.Value ? "" : dr["DateMade"].ToString();
                    cells2[sheet2row + i, 3].PutValue(C3);
                    String C4 = dr["Brand"] == DBNull.Value ? "" : dr["Brand"].ToString();
                    cells2[sheet2row + i, 4].PutValue(C4);
                    String C5 = dr["Weight"] == DBNull.Value ? "" : dr["Weight"].ToString();
                    cells2[sheet2row + i, 5].PutValue(C5);
                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells2[sheet2row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells2[sheet2row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet2row, 7) + ":" + CellsHelper.CellIndexToName(sheet2row + dt.Rows.Count - 1, 7);
            cells2[sheet2row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet2row = sheet2row + dt.Rows.Count - 1;
            }



            #endregion
            #region 压铸机
            sheet2row = sheet2row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                      DeviceQuantity,
                        DeviceCapacity,
                        DateMade,
                        Model,
                        Brand,
                        Weight
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                           and BasicInfoGUID=@BasicInfoGUID");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 6);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 22);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells2.InsertRows(sheet2row + 1, dt.Rows.Count - 1);
                cells2.CopyRows(cells2, sheet2row + 1, sheet2row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells2[sheet2row + i, 2].PutValue(C2);
                    String C3 = dr["DateMade"] == DBNull.Value ? "" : dr["DateMade"].ToString();
                    cells2[sheet2row + i, 3].PutValue(C3);
                    String C4 = dr["Brand"] == DBNull.Value ? "" : dr["Brand"].ToString();
                    cells2[sheet2row + i, 4].PutValue(C4);
                    String C5 = dr["Weight"] == DBNull.Value ? "" : dr["Weight"].ToString();
                    cells2[sheet2row + i, 5].PutValue(C5);
                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells2[sheet2row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells2[sheet2row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet2row, 7) + ":" + CellsHelper.CellIndexToName(sheet2row + dt.Rows.Count - 1, 7);
            cells2[sheet2row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet2row = sheet2row + dt.Rows.Count - 1;
            }



            #endregion
            #region 压铸品沙孔检测机
            sheet2row = sheet2row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                      DeviceQuantity,
                        DeviceCapacity,
                        DateMade,
                        Model,
                        Brand,
                        Weight
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                           and BasicInfoGUID=@BasicInfoGUID");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 6);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 23);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells2.InsertRows(sheet2row + 1, dt.Rows.Count - 1);
                cells2.CopyRows(cells2, sheet2row + 1, sheet2row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells2[sheet2row + i, 2].PutValue(C2);
                    String C3 = dr["DateMade"] == DBNull.Value ? "" : dr["DateMade"].ToString();
                    cells2[sheet2row + i, 3].PutValue(C3);
                    String C4 = dr["Brand"] == DBNull.Value ? "" : dr["Brand"].ToString();
                    cells2[sheet2row + i, 4].PutValue(C4);
                    String C5 = dr["Weight"] == DBNull.Value ? "" : dr["Weight"].ToString();
                    cells2[sheet2row + i, 5].PutValue(C5);
                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells2[sheet2row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells2[sheet2row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet2row, 7) + ":" + CellsHelper.CellIndexToName(sheet2row + dt.Rows.Count - 1, 7);
            cells2[sheet2row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet2row = sheet2row + dt.Rows.Count - 1;
            }



            #endregion
            #region 抛光机

            sheet2row = sheet2row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                          DeviceQuantity,
                        DeviceCapacity,
                        DateMade,
                        Model,
                        Brand
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                           and BasicInfoGUID=@BasicInfoGUID"); 
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 6);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 24);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells2.InsertRows(sheet2row + 1, dt.Rows.Count - 1);
                cells2.CopyRows(cells2, sheet2row + 1, sheet2row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells2[sheet2row + i, 2].PutValue(C2);
                    String C3 = dr["DateMade"] == DBNull.Value ? "" : dr["DateMade"].ToString();
                    cells2[sheet2row + i, 3].PutValue(C3);
                    String C4 = dr["Brand"] == DBNull.Value ? "" : dr["Brand"].ToString();
                    cells2[sheet2row + i, 4].PutValue(C4);

                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells2[sheet2row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells2[sheet2row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet2row, 7) + ":" + CellsHelper.CellIndexToName(sheet2row + dt.Rows.Count - 1, 7);
            cells2[sheet2row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet2row = sheet2row + dt.Rows.Count - 1;
            }


            #endregion
            #region 攻牙机
            sheet2row = sheet2row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                          DeviceQuantity,
                        DeviceCapacity,
                        DateMade,
                        Model,
                        Brand
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                           and BasicInfoGUID=@BasicInfoGUID");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 6);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 25);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells2.InsertRows(sheet2row + 1, dt.Rows.Count - 1);
                cells2.CopyRows(cells2, sheet2row + 1, sheet2row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells2[sheet2row + i, 2].PutValue(C2);
                    String C3 = dr["DateMade"] == DBNull.Value ? "" : dr["DateMade"].ToString();
                    cells2[sheet2row + i, 3].PutValue(C3);
                    String C4 = dr["Brand"] == DBNull.Value ? "" : dr["Brand"].ToString();
                    cells2[sheet2row + i, 4].PutValue(C4);

                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells2[sheet2row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells2[sheet2row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet2row, 7) + ":" + CellsHelper.CellIndexToName(sheet2row + dt.Rows.Count - 1, 7);
            cells2[sheet2row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet2row = sheet2row + dt.Rows.Count - 1;

            }





            #endregion
            #region 打磨机
            sheet2row = sheet2row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                          DeviceQuantity,
                        DeviceCapacity,
                        DateMade,
                        Model,
                        Brand
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                           and BasicInfoGUID=@BasicInfoGUID");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 6);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 26);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells2.InsertRows(sheet2row + 1, dt.Rows.Count - 1);
                cells2.CopyRows(cells2, sheet2row + 1, sheet2row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells2[sheet2row + i, 2].PutValue(C2);
                    String C3 = dr["DateMade"] == DBNull.Value ? "" : dr["DateMade"].ToString();
                    cells2[sheet2row + i, 3].PutValue(C3);
                    String C4 = dr["Brand"] == DBNull.Value ? "" : dr["Brand"].ToString();
                    cells2[sheet2row + i, 4].PutValue(C4);

                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells2[sheet2row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells2[sheet2row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet2row, 7) + ":" + CellsHelper.CellIndexToName(sheet2row + dt.Rows.Count - 1, 7);
            cells2[sheet2row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet2row = sheet2row + dt.Rows.Count - 1;
            }
            #endregion
            #region 拉丝机
            sheet2row = sheet2row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                          DeviceQuantity,
                        DeviceCapacity,
                        DateMade,
                        Model,
                        Brand
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                           and BasicInfoGUID=@BasicInfoGUID");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 6);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 27);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells2.InsertRows(sheet2row + 1, dt.Rows.Count - 1);
                cells2.CopyRows(cells2, sheet2row + 1, sheet2row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells2[sheet2row + i, 2].PutValue(C2);
                    String C3 = dr["DateMade"] == DBNull.Value ? "" : dr["DateMade"].ToString();
                    cells2[sheet2row + i, 3].PutValue(C3);
                    String C4 = dr["Brand"] == DBNull.Value ? "" : dr["Brand"].ToString();
                    cells2[sheet2row + i, 4].PutValue(C4);

                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells2[sheet2row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells2[sheet2row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet2row, 7) + ":" + CellsHelper.CellIndexToName(sheet2row + dt.Rows.Count - 1, 7);
            cells2[sheet2row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet2row = sheet2row + dt.Rows.Count - 1;
            }
            #endregion
            #region 超音波清洗线
            sheet2row = sheet2row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                          DeviceQuantity,
                        DeviceCapacity,
                        DateMade,
                        Model,
                        Brand
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                           and BasicInfoGUID=@BasicInfoGUID");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 6);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 28);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells2.InsertRows(sheet2row + 1, dt.Rows.Count - 1);
                cells2.CopyRows(cells2, sheet2row + 1, sheet2row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells2[sheet2row + i, 2].PutValue(C2);
                    String C3 = dr["DateMade"] == DBNull.Value ? "" : dr["DateMade"].ToString();
                    cells2[sheet2row + i, 3].PutValue(C3);
                    String C4 = dr["Brand"] == DBNull.Value ? "" : dr["Brand"].ToString();
                    cells2[sheet2row + i, 4].PutValue(C4);

                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells2[sheet2row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells2[sheet2row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet2row, 7) + ":" + CellsHelper.CellIndexToName(sheet2row + dt.Rows.Count - 1, 7);
            cells2[sheet2row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet2row = sheet2row + dt.Rows.Count - 1;
            }


            #endregion
            #region 阳极氧化线
            sheet2row = sheet2row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                          DeviceQuantity,
                        DeviceCapacity,
                        LineLength,
                        Model,
                        Anodized
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                           and BasicInfoGUID=@BasicInfoGUID");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 6);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 29);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells2.InsertRows(sheet2row + 1, dt.Rows.Count - 1);
                cells2.CopyRows(cells2, sheet2row + 1, sheet2row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells2[sheet2row + i, 2].PutValue(C2);
                    String C3 = dr["LineLength"] == DBNull.Value ? "" : dr["LineLength"].ToString();
                    cells2[sheet2row + i, 3].PutValue(C3);
                    String C4 = dr["Anodized"] == DBNull.Value ? "" : dr["Anodized"].ToString();
                    cells2[sheet2row + i, 4].PutValue(C4);

                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells2[sheet2row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells2[sheet2row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet2row, 7) + ":" + CellsHelper.CellIndexToName(sheet2row + dt.Rows.Count - 1, 7);
            cells2[sheet2row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet2row = sheet2row + dt.Rows.Count - 1;
            }


            #endregion
            #region 喷涂线
            sheet2row = sheet2row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                          DeviceQuantity,
                        DeviceCapacity,
                        Model,
                        LineLength,
                        RoastQty,
                        SprayingType
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                           and BasicInfoGUID=@BasicInfoGUID");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 6);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 30);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells2.InsertRows(sheet2row + 1, dt.Rows.Count - 1);
                cells2.CopyRows(cells2, sheet2row + 1, sheet2row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells2[sheet2row + i, 2].PutValue(C2);
                    String C3 = dr["LineLength"] == DBNull.Value ? "" : dr["LineLength"].ToString();
                    cells2[sheet2row + i, 3].PutValue(C3);
                    String C4 = dr["RoastQty"] == DBNull.Value ? "" : dr["RoastQty"].ToString();
                    cells2[sheet2row + i, 4].PutValue(C4);
                    String C5 = dr["SprayingType"] == DBNull.Value ? "" : dr["SprayingType"].ToString();
                    cells2[sheet2row + i, 5].PutValue(C5);
                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells2[sheet2row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells2[sheet2row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet2row, 7) + ":" + CellsHelper.CellIndexToName(sheet2row + dt.Rows.Count - 1, 7);
            cells2[sheet2row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet2row = sheet2row + dt.Rows.Count - 1;
            }


            #endregion
            #region 电镀线

            sheet2row = sheet2row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                          DeviceQuantity,
                        DeviceCapacity,
                        Model,
                        LineLength,
                        PlatingType,
                        PlatingCapacity
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                           and BasicInfoGUID=@BasicInfoGUID");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 6);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 31);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells2.InsertRows(sheet2row + 1, dt.Rows.Count - 1);
                cells2.CopyRows(cells2, sheet2row + 1, sheet2row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells2[sheet2row + i, 2].PutValue(C2);
                    String C3 = dr["LineLength"] == DBNull.Value ? "" : dr["LineLength"].ToString();
                    cells2[sheet2row + i, 3].PutValue(C3);
                    String C4 = dr["PlatingCapacity"] == DBNull.Value ? "" : dr["PlatingCapacity"].ToString();
                    cells2[sheet2row + i, 4].PutValue(C4);
                    String C5 = dr["PlatingType"] == DBNull.Value ? "" : dr["PlatingType"].ToString();
                    cells2[sheet2row + i, 5].PutValue(C5);
                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells2[sheet2row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells2[sheet2row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet2row, 7) + ":" + CellsHelper.CellIndexToName(sheet2row + dt.Rows.Count - 1, 7);
            cells2[sheet2row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet2row = sheet2row + dt.Rows.Count - 1;
            }

            #endregion
            #region 加工组装线
            sheet2row = sheet2row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                          DeviceQuantity,
                        DeviceCapacity,
                        Model,
                        LineLength
                       
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                           and BasicInfoGUID=@BasicInfoGUID");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 6);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 32);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells2.InsertRows(sheet2row + 1, dt.Rows.Count - 1);
                cells2.CopyRows(cells2, sheet2row + 1, sheet2row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells2[sheet2row + i, 2].PutValue(C2);
                    String C3 = dr["LineLength"] == DBNull.Value ? "" : dr["LineLength"].ToString();
                    cells2[sheet2row + i, 3].PutValue(C3);

                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells2[sheet2row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells2[sheet2row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet2row, 7) + ":" + CellsHelper.CellIndexToName(sheet2row + dt.Rows.Count - 1, 7);
            cells2[sheet2row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet2row = sheet2row + dt.Rows.Count - 1;

            }

            #endregion
            #region 丝印机
            sheet2row = sheet2row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                          DeviceQuantity,
                        DeviceCapacity,
                        DateMade,
                        Model,
                        Brand
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                           and BasicInfoGUID=@BasicInfoGUID");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 6);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 33);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells2.InsertRows(sheet2row + 1, dt.Rows.Count - 1);
                cells2.CopyRows(cells2, sheet2row + 1, sheet2row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells2[sheet2row + i, 2].PutValue(C2);
                    String C3 = dr["DateMade"] == DBNull.Value ? "" : dr["DateMade"].ToString();
                    cells2[sheet2row + i, 3].PutValue(C3);
                    String C4 = dr["Brand"] == DBNull.Value ? "" : dr["Brand"].ToString();
                    cells2[sheet2row + i, 4].PutValue(C4);

                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells2[sheet2row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells2[sheet2row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet2row, 7) + ":" + CellsHelper.CellIndexToName(sheet2row + dt.Rows.Count - 1, 7);
            cells2[sheet2row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet2row = sheet2row + dt.Rows.Count - 1;

            }

            #endregion
            #region 移印机


            sheet2row = sheet2row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                          DeviceQuantity,
                        DeviceCapacity,
                        DateMade,
                        Model,
                        Brand
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                           and BasicInfoGUID=@BasicInfoGUID");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 6);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 34);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells2.InsertRows(sheet2row + 1, dt.Rows.Count - 1);
                cells2.CopyRows(cells2, sheet2row + 1, sheet2row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells2[sheet2row + i, 2].PutValue(C2);
                    String C3 = dr["DateMade"] == DBNull.Value ? "" : dr["DateMade"].ToString();
                    cells2[sheet2row + i, 3].PutValue(C3);
                    String C4 = dr["Brand"] == DBNull.Value ? "" : dr["Brand"].ToString();
                    cells2[sheet2row + i, 4].PutValue(C4);

                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells2[sheet2row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells2[sheet2row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet2row, 7) + ":" + CellsHelper.CellIndexToName(sheet2row + dt.Rows.Count - 1, 7);
            cells2[sheet2row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet2row = sheet2row + dt.Rows.Count - 1;
            }
            #endregion
            #region 镭雕机

            sheet2row = sheet2row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                          DeviceQuantity,
                        DeviceCapacity,
                        DateMade,
                        Model,
                        Brand
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                           and BasicInfoGUID=@BasicInfoGUID");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 6);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 35);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells2.InsertRows(sheet2row + 1, dt.Rows.Count - 1);
                cells2.CopyRows(cells2, sheet2row + 1, sheet2row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells2[sheet2row + i, 2].PutValue(C2);
                    String C3 = dr["DateMade"] == DBNull.Value ? "" : dr["DateMade"].ToString();
                    cells2[sheet2row + i, 3].PutValue(C3);
                    String C4 = dr["Brand"] == DBNull.Value ? "" : dr["Brand"].ToString();
                    cells2[sheet2row + i, 4].PutValue(C4);

                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells2[sheet2row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells2[sheet2row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet2row, 7) + ":" + CellsHelper.CellIndexToName(sheet2row + dt.Rows.Count - 1, 7);
            cells2[sheet2row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet2row = sheet2row + dt.Rows.Count - 1;
            }

            #endregion
            #region (量测设备)

            sheet2row = sheet2row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                   DeviceQuantity,
                        DeviceCapacity,
                        DateMade,
                        Model,
                        Brand
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                           and BasicInfoGUID=@BasicInfoGUID");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 5);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 36);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells2.InsertRows(sheet2row + 1, dt.Rows.Count - 1);
                cells2.CopyRows(cells2, sheet2row + 1, sheet2row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells2[sheet2row + i, 2].PutValue(C2);
                    String C3 = dr["DateMade"] == DBNull.Value ? "" : dr["DateMade"].ToString();
                    cells2[sheet2row + i, 3].PutValue(C3);
                    String C4 = dr["Brand"] == DBNull.Value ? "" : dr["Brand"].ToString();
                    cells2[sheet2row + i, 4].PutValue(C4);

                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells2[sheet2row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells2[sheet2row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet2row, 7) + ":" + CellsHelper.CellIndexToName(sheet2row + dt.Rows.Count - 1, 7);
            cells2[sheet2row, 8].Formula = "SUM(" + Formula + ")";
            if (dt.Rows.Count > 1)
            {
                sheet2row = sheet2row + dt.Rows.Count - 1;
            }

            #endregion
            #region (模具房设备)

            sheet2row = sheet2row + 3;
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"SELECT DeviceName,
                   DeviceQuantity,
                        DeviceCapacity,
                        DateMade,
                        Model,
                        Brand
                         FROM TB_SQM_Equipment  
                        where [TB_SQM_Equipment_TypeCID]=@TB_SQM_Equipment_TypeCID 
                        and [TB_SQM_Equipment_Special_TypeSCID]=@TB_SQM_Equipment_Special_TypeSCID
                           and BasicInfoGUID=@BasicInfoGUID");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", 6);
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", 37);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells2.InsertRows(sheet2row + 1, dt.Rows.Count - 1);
                cells2.CopyRows(cells2, sheet2row + 1, sheet2row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C2 = dr["Model"] == DBNull.Value ? "" : dr["Model"].ToString();
                    cells2[sheet2row + i, 2].PutValue(C2);
                    String C3 = dr["DateMade"] == DBNull.Value ? "" : dr["DateMade"].ToString();
                    cells2[sheet2row + i, 3].PutValue(C3);
                    String C4 = dr["Brand"] == DBNull.Value ? "" : dr["Brand"].ToString();
                    cells2[sheet2row + i, 4].PutValue(C4);

                    String C6 = dr["DeviceCapacity"] == DBNull.Value ? "" : dr["DeviceCapacity"].ToString();
                    cells2[sheet2row + i, 6].PutValue(C6);
                    double C7 = dr["DeviceQuantity"] == DBNull.Value ? 0 : double.Parse(dr["DeviceQuantity"].ToString());
                    cells2[sheet2row + i, 7].PutValue(C7);
                }
                catch (Exception ex)
                {

                }
            }
            Formula = CellsHelper.CellIndexToName(sheet2row, 7) + ":" + CellsHelper.CellIndexToName(sheet2row + dt.Rows.Count - 1, 7);
            cells2[sheet2row, 8].Formula = "SUM(" + Formula + ")";
            #endregion
            #endregion


        }


        public static void CreatMerchantExcel(String filename, SqlConnection sqlConnection, String BasicInfoGUID, String localPath, String urlPre)
        {
            License lic = new License();
            //String AsposeLicPath = "E:\\TW VMI V2\\Portal_Web\\Source\\Aspose.Cells\\Aspose.Cells.lic";
            //lic.SetLicense(AsposeLicPath);
            //String templatePath = "E:\\merchant.xlsx";
            lic.SetLicense(localPath + @"Source\Aspose.Cells\Aspose.Cells.lic");
            String templatePath = localPath + @"Source\Template\merchant.xlsx";
            Workbook book = new Workbook(templatePath);
            Worksheet sheet0 = book.Worksheets[0];
            BindMerchantReport(ref sheet0, sqlConnection, BasicInfoGUID, urlPre);
            book.CalculateFormula();
            object i = book.Worksheets[0].Cells[14, 6].Value;
            EditScore(sqlConnection, BasicInfoGUID,i);
            book.Save(filename, SaveFormat.Xlsx);
        }
        public static void BindMerchantReport(ref Worksheet sheet6, SqlConnection sqlConnection, String BasicInfoGUID, String urlPre)
        {
            Cells cells = sheet6.Cells;
            Workbook book1 = new Workbook();
            Style style1 = book1.Styles[book1.Styles.Add()];
            style1.HorizontalAlignment = TextAlignmentType.Center;//文字居中
            style1.VerticalAlignment = TextAlignmentType.Center;
            style1.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin; //应用边界线 左边界线  
            style1.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin; //应用边界线 右边界线  
            style1.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin; //应用边界线 上边界线  
            style1.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin; //应用边界线 下边界线
            String CalculateFormula = "=";
            #region 模块1
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
            SELECT [TB_SQM_Member_Vendor_Map].VendorCode
                  ,[CompanyName]
                  ,[CompanyAddress]
                  ,[FactoryName]
                  ,[FactoryAddress]
                  ,IsTrader
                  ,IsSpotTrader
                  ,[TB_SQM_Commodity_SubCID]
                  ,[TB_SQM_Commodity_Sub].CNAME
                  ,[DateInfo]
                  ,[ProvidedName]
                  ,[JobTitle]
                  ,[EnterpriseCategory]
                  ,[OwnerShip]
                  ,[FoundedYear]
                  ,[LastRevenues1]
                  ,[LastRevenues2]
                  ,[LastRevenues3]
                  ,[CurrentRevenues]
                  ,[TurnoverAnalysis]
                  ,([RevenueGrowthRate1]+
                  [RevenueGrowthRate2]+
                  [RevenueGrowthRate3])/3 as [RevenueGrowthRate]
                  ,([GrossProfitRate1]+
                  [GrossProfitRate2]+
                  [GrossProfitRate3])/3 as [GrossProfitRate]
                  ,[PlanInvestCapital]
                  ,[BankAndAccNumber]
                  ,[TradingCurrency]
                  ,[TradeMode]
                  ,[VMIManageModel]
                  ,[Distance]
                  ,[MinMonthStateDays]
                  ,[BU1Turnover]
                  ,[BU2Turnover]
                  ,[BU3Turnover]
                  ,[CompanyAdvantage]
              FROM [dbo].[TB_SQM_Manufacturers_BasicInfo]
              inner join [dbo].[TB_SQM_Commodity_Sub]
              on [TB_SQM_Commodity_Sub].[CID]=[TB_SQM_Manufacturers_BasicInfo].[TB_SQM_Commodity_SubCID]
              and  [TB_SQM_Commodity_Sub].[TB_SQM_CommodityCID]=[TB_SQM_Manufacturers_BasicInfo].[TB_SQM_Commodity_SubTB_SQM_CommodityCID]
              inner join [dbo].[TB_SQM_Member_Vendor_Map]
              on [TB_SQM_Member_Vendor_Map].[MemberGUID]=[TB_SQM_Manufacturers_BasicInfo].[VendorCode]
               where BasicInfoGUID=@BasicInfoGUID
              ");

            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                String C6 = dr["VendorCode"] == DBNull.Value ? "" : dr["VendorCode"].ToString();
                cells[5, 2].PutValue(C6);
                String C7 = dr["CompanyName"] == DBNull.Value ? "" : dr["CompanyName"].ToString();
                cells[6, 2].PutValue(C7);
                String C8 = dr["CompanyAddress"] == DBNull.Value ? "" : dr["CompanyAddress"].ToString();
                cells[7, 2].PutValue(C8);
                //String C9 = dr["FactoryName"] == DBNull.Value ? "" : dr["FactoryName"].ToString();
                //cells[8, 2].PutValue(C9);
                //String C10 = dr["FactoryAddress"] == DBNull.Value ? "" : dr["FactoryAddress"].ToString();
                //cells[9, 2].PutValue(C10);
                if (dr["IsTrader"].ToString().Equals("True"))
                {
                    cells[8, 2].PutValue("貿易商");
                }
                else
                {
                    cells[8, 2].PutValue("現貨商");
                }
                String C10 = dr["TB_SQM_Commodity_SubCID"] == DBNull.Value ? "" : dr["TB_SQM_Commodity_SubCID"].ToString();
                cells[9, 2].PutValue(C10);
                String C11 = dr["CNAME"] == DBNull.Value ? "" : dr["CNAME"].ToString();
                cells[10, 2].PutValue(C11);
                String C12 = dr["DateInfo"] == DBNull.Value ? "" : dr["DateInfo"].ToString();
                cells[11, 2].PutValue(C12);
                String C13 = dr["ProvidedName"] == DBNull.Value ? "" : dr["ProvidedName"].ToString();
                cells[12, 2].PutValue(C13);
                String C15 = dr["JobTitle"] == DBNull.Value ? "" : dr["JobTitle"].ToString();
                cells[13, 2].PutValue(C15);
                String C17 = dr["EnterpriseCategory"] == DBNull.Value ? "" : dr["EnterpriseCategory"].ToString();
                cells[16, 2].PutValue(C17);
                String C18 = dr["OwnerShip"] == DBNull.Value ? "" : dr["OwnerShip"].ToString();
                cells[17, 2].PutValue(C18);
                String C19 = dr["FoundedYear"] == DBNull.Value ? "" : dr["FoundedYear"].ToString();
                cells[18, 2].PutValue(C19);
                double C20 = dr["LastRevenues1"] == DBNull.Value ? 0 :double.Parse( dr["LastRevenues1"].ToString());
                cells[19, 2].PutValue(C20);
                double D20 = dr["LastRevenues2"] == DBNull.Value ? 0 : double.Parse(dr["LastRevenues2"].ToString());
                cells[19, 3].PutValue(D20);
                double E20 = dr["LastRevenues3"] == DBNull.Value ? 0 : double.Parse(dr["LastRevenues3"].ToString());
                cells[19, 4].PutValue(E20);
                CalculateFormula = CalculateFormula + CellsHelper.CellIndexToName(19, 6);
                String C21 = dr["CurrentRevenues"] == DBNull.Value ? "" : dr["CurrentRevenues"].ToString();
                cells[20, 2].PutValue(C21);
                String C22 = dr["TurnoverAnalysis"] == DBNull.Value ? "" : dr["TurnoverAnalysis"].ToString();
                cells[21, 2].PutValue(C22);
                String C23 = dr["RevenueGrowthRate"] == DBNull.Value ? "" : dr["RevenueGrowthRate"].ToString();
                cells[22, 2].PutValue(C23);
                String C24 = dr["GrossProfitRate"] == DBNull.Value ? "" : dr["GrossProfitRate"].ToString();
                cells[23, 2].PutValue(C24);
                String C25 = dr["PlanInvestCapital"] == DBNull.Value ? "" : dr["PlanInvestCapital"].ToString();
                cells[24, 2].PutValue(C25);
                String C26 = dr["BankAndAccNumber"] == DBNull.Value ? "" : dr["BankAndAccNumber"].ToString();
                cells[25, 2].PutValue(C26);
                String C27 = dr["TradingCurrency"] == DBNull.Value ? "" : dr["TradingCurrency"].ToString();
                cells[26, 2].PutValue(C27);

                String C28 = dr["TradeMode"] == DBNull.Value ? "" : dr["TradeMode"].ToString();
                cells[27, 2].PutValue(C28);

                double C29 = dr["VMIManageModel"] == DBNull.Value ? 0: double.Parse(dr["VMIManageModel"].ToString());
                cells[28, 2].PutValue(C29);
                CalculateFormula = CalculateFormula + "+" + CellsHelper.CellIndexToName(28, 6);
                double C30 = dr["Distance"] == DBNull.Value ? 0 : double.Parse(dr["Distance"].ToString());
                cells[29, 2].PutValue(C30);
                CalculateFormula = CalculateFormula + "-" + CellsHelper.CellIndexToName(29, 6);
                double C31 = dr["MinMonthStateDays"] == DBNull.Value ? 0 : double.Parse(dr["MinMonthStateDays"].ToString());
                cells[30, 2].PutValue(C31);
                CalculateFormula = CalculateFormula + "+" + CellsHelper.CellIndexToName(30, 6);
                double C33 = dr["BU1Turnover"] == DBNull.Value ? 0: double.Parse(dr["BU1Turnover"].ToString());
                cells[32, 2].PutValue(C33); 
                double D33 = dr["BU2Turnover"] == DBNull.Value ? 0 : double.Parse(dr["BU2Turnover"].ToString());
                cells[32, 3].PutValue(D33);
                double E33 = dr["BU3Turnover"] == DBNull.Value ? 0 : double.Parse(dr["BU3Turnover"].ToString());
                cells[32, 4].PutValue(E33);
                CalculateFormula = CalculateFormula + "+" + CellsHelper.CellIndexToName(31, 6);
                String C34 = dr["CompanyAdvantage"] == DBNull.Value ? "" : dr["CompanyAdvantage"].ToString();
                cells[33, 2].PutValue(C34);
            }
            #endregion
            #region 模块2
            dt = new DataTable();
            sb = new StringBuilder();
            sb.Append(@"SELECT TOP 4 [PrincipalProducts]
      ,[RevenuePer]
      ,[MOQ]
      ,[SampleTime]
      ,[LeadTime]
      ,SupAndOriName
      ,OfferPlaceCertify
      ,OfferSellCertify
      ,UploadCertifyPath
      ,[MajorCompetitor]
  FROM [TB_SQM_ProductDescription]
    where BasicInfoGUID = @BasicInfoGUID");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            CalculateFormula = CalculateFormula + "-" + CellsHelper.CellIndexToName(40, 6);
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C1 = dr["PrincipalProducts"] == DBNull.Value ? "" : dr["PrincipalProducts"].ToString();
                    cells[37, 2 + i].PutValue(C1);
                    String C2 = dr["RevenuePer"] == DBNull.Value ? "" : dr["RevenuePer"].ToString();
                    cells[38, 2 + i].PutValue(C2);
                    String C3 = dr["MOQ"] == DBNull.Value ? "" : dr["MOQ"].ToString();
                    cells[39, 2 + i].PutValue(C3);
                    double C4 = dr["SampleTime"] == DBNull.Value ? 0 : double.Parse(dr["SampleTime"].ToString());
                    cells[40, 2 + i].PutValue(C4);
                    
                    String C5 = dr["LeadTime"] == DBNull.Value ? "" : dr["LeadTime"].ToString();
                    cells[41, 2 + i].PutValue(C5);
                    String C6 = dr["SupAndOriName"] == DBNull.Value ? "" : dr["SupAndOriName"].ToString();
                    cells[42, 2 + i].PutValue(C6);
                    String C7 = dr["OfferPlaceCertify"] == DBNull.Value ? "" : dr["OfferPlaceCertify"].ToString();
                    cells[43, 2 + i].PutValue(C7);
                    String C8 = dr["OfferSellCertify"] == DBNull.Value ? "" : dr["OfferSellCertify"].ToString();
                    cells[44, 2 + i].PutValue(C8);
                    String C9 = dr["UploadCertifyPath"] == DBNull.Value ? "" : dr["UploadCertifyPath"].ToString();
                    cells[45, 2 + i].PutValue(C9);
                    String C10 = dr["MajorCompetitor"] == DBNull.Value ? "" : dr["MajorCompetitor"].ToString();
                    cells[46, 2 + i].PutValue(C10);
                }
                catch (Exception ex)
                {

                }
            }
            #endregion
            #region 模块3
            dt = new DataTable();
            sb = new StringBuilder();
            sb.Append(@"
   SELECT
       [OEMCustomerName]
      ,[BusinessCategory]
      ,[RevenuePer]
  FROM[dbo].[TB_SQM_Customers]
   where BasicInfoGUID=@BasicInfoGUID
           ");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C1 = dr["OEMCustomerName"] == DBNull.Value ? "" : dr["OEMCustomerName"].ToString();
                    cells[49, 2 + i].PutValue(C1);
                    String C2 = dr["BusinessCategory"] == DBNull.Value ? "" : dr["BusinessCategory"].ToString();
                    cells[50, 2 + i].PutValue(C2);
                    String C3 = dr["RevenuePer"] == DBNull.Value ? "" : dr["RevenuePer"].ToString();
                    cells[51, 2 + i].PutValue(C3);
                }
                catch (Exception ex)
                {

                }
            }
            #endregion
            #region 模块4
            dt = new DataTable();
            sb = new StringBuilder();
            sb.Append(@"
            SELECT  [CNAME],
      [EmployeeQty]
      ,[EmployeePlanned]
      ,[AverageJobSeniority]
  FROM [TB_SQM_HR]
  inner join [dbo].[TB_SQM_HR_TYPE] on [TB_SQM_HR_TYPE].[CID]=[TB_SQM_HR].[TB_SQM_HR_TYPECID]
    where BasicInfoGUID=@BasicInfoGUID
    order by [TB_SQM_HR_TYPECID]
             ");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                cells.InsertRows(55, dt.Rows.Count );
                cells.CopyRows(cells, 55, 56, dt.Rows.Count - 1);
            }
            int row = 55 + (dt.Rows.Count);
            CalculateFormula = CalculateFormula + "+" + CellsHelper.CellIndexToName(54, 6);
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C0 = "4.0" + (i + 2);
                    cells[55 + i, 0].PutValue(C0);
                    String C1 = dr["CNAME"] == DBNull.Value ? "" : dr["CNAME"].ToString();
                    cells[55 + i, 1].PutValue(C1);
                    double C2 = dr["EmployeeQty"] == DBNull.Value ? 0 :double.Parse( dr["EmployeeQty"].ToString());
                    cells[55 + i, 2].PutValue(C2);
                    String C3 = dr["EmployeePlanned"] == DBNull.Value ? "" : dr["EmployeePlanned"].ToString();
                    cells[55 + i, 3].PutValue(C3);
                    String C4 = dr["AverageJobSeniority"] == DBNull.Value ? "" : dr["AverageJobSeniority"].ToString();
                    cells[55 + i, 4].PutValue(C4);
                    cells[55 + i, 6].Formula = "=" + CellsHelper.CellIndexToName(55 + i, 2);
                    CalculateFormula = CalculateFormula + "+" + CellsHelper.CellIndexToName(55+i, 6);
                }
                catch (Exception ex)
                {

                }
            }
            if (dt.Rows.Count>0)
            {
                cells[54,2].Formula="SUM("+ CellsHelper.CellIndexToName(55, 2) + ":"+ CellsHelper.CellIndexToName(row-1, 2) + ")";
                cells[54, 6].Formula = "=" + CellsHelper.CellIndexToName(54, 2);
            }
            #endregion
            #region 模块5
            dt = new DataTable();
            sb = new StringBuilder();
            sb.Append(@" 
                SELECT [CNAME]
                ,[CertificationAuthority]
                ,[CertificationNum]
                ,[CertificationDate]
                ,[ValidDate]
             ,[CertificateImageFGUID]
            FROM[TB_SQM_Certifications]
            inner join[TB_SQM_Certifications_Type] on[TB_SQM_Certifications_Type].CID =[TB_SQM_Certifications].[TB_SQM_Certifications_TypeCID]
             where BasicInfoGUID = @BasicInfoGUID");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            row = row + 2;
            if (dt.Rows.Count > 1)
            {
                cells.InsertRows(row + 1, dt.Rows.Count - 1);
                cells.CopyRows(cells, row + 1, row + 2, dt.Rows.Count - 2);
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    String C0 = "5.0" + (i + 1);
                    cells[row + i, 0].PutValue(C0);
                    String C1 = dr["CNAME"] == DBNull.Value ? "" : dr["CNAME"].ToString();
                    cells[row + i, 1].PutValue(C1);
                    String C2 = dr["CertificationAuthority"] == DBNull.Value ? "" : dr["CertificationAuthority"].ToString();
                    cells[row + i, 2].PutValue(C2);
                    String C3 = dr["CertificationNum"] == DBNull.Value ? "" : dr["CertificationNum"].ToString();
                    cells[row + i, 3].PutValue(C3);
                    String C4 = dr["CertificationDate"] == DBNull.Value ? "" : dr["CertificationDate"].ToString();
                    cells[row + i, 4].PutValue(C4);
                    String C5 = dr["ValidDate"] == DBNull.Value ? "" : dr["ValidDate"].ToString();
                    cells[row + i, 5].PutValue(C5);
                    String C6 = dr["CertificateImageFGUID"] == DBNull.Value ? "" : dr["CertificateImageFGUID"].ToString();
                    String url = urlPre + "/SQMBasic/DownloadSQMFile?DataKey=" + C6;
                    cells[row + i, 6].PutValue(C6);
                    sheet6.Hyperlinks.Add(row + i, 6, 1, 1, url);

                }
                catch (Exception ex)
                {

                }
            }
            if (dt.Rows.Count > 0)
            {
                cells[row + dt.Rows.Count, 2].PutValue(dt.Rows.Count);
                CalculateFormula = CalculateFormula + "+" + CellsHelper.CellIndexToName(row + dt.Rows.Count, 6);
            }
            else
            {
                cells[row + 1, 2].PutValue(dt.Rows.Count);
                CalculateFormula = CalculateFormula + "+" + CellsHelper.CellIndexToName(row + 1, 6);
            }
            cells[15,6].Formula = CalculateFormula;
            #endregion
        }

        public static String CreatExcel(String filename, String filenameNet, SqlConnection sqlConnection, TB_SQM_Manufacturers_BasicInfo dataItem, String localPath,String RunAsUserGUID,String urlPre)
        {
            String r=string.Empty;
            r = CheckPart( sqlConnection,  dataItem); 
            if (r != "")
            {
                return r;
            }
            switch (dataItem.TB_SQM_Vendor_TypeCID)
            {
                case 1:
                    CreatManufacturerExcel(filename, sqlConnection, dataItem.BasicInfoGUID,
                localPath, urlPre);
                    
                    break;
                case 2:
                    CreatMerchantExcel(filename, sqlConnection, dataItem.BasicInfoGUID,
                localPath, urlPre);
                    
                    break;
                case 3:
                    CreatAgentExcel(filename, sqlConnection, dataItem.BasicInfoGUID,
                localPath, urlPre);
                    break;
            }
            //將檔案寫入db中
            r = UploadBasicInfoFile(sqlConnection, RunAsUserGUID, filename, dataItem);
            if (r != "")
            {
                return r;
            }

            //創建簽核流程
            r = SQM_Approve_Case_Helper.CreateApproveCase(sqlConnection, dataItem, filename, filenameNet, urlPre, "1");
            if (r != "")
            {
                return r;
            }

            return r;
            

        }

        private static string CheckPart(SqlConnection sqlConnection, TB_SQM_Manufacturers_BasicInfo dataItem)
        {
            string sErrMsg = string.Empty;
            int iRowCount = 0;
            StringBuilder sb = new StringBuilder();
          
            sb.Append("SELECT TB_SQM_Commodity_SubCID from TB_SQM_Manufacturers_BasicInfo  where BasicInfoGUID  =@BasicInfoGUID");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(dataItem.BasicInfoGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        dataItem.TB_SQM_Commodity_SubCID = dr["TB_SQM_Commodity_SubCID"].ToString();
                    }
                }
            }


            if (dataItem.TB_SQM_Commodity_SubCID.Equals("DL-ME02") || dataItem.TB_SQM_Commodity_SubCID.Equals("DL-ME03") || dataItem.TB_SQM_Commodity_SubCID.Equals("DL-ME04"))
            {
                sb = new StringBuilder();
                sb.Append(@"SELECT [CNAME]   
  FROM [TB_SQM_Manufacturers_BasicInfo_Part_Map]
   inner join [TB_SQM_Manufacturers_BasicInfo_Part] on [TB_SQM_Manufacturers_BasicInfo_Part].CID=[TB_SQM_Manufacturers_BasicInfo_Part_Map].[PartID]
     WHERE VendorTypeID =(SELECT TB_SQM_Vendor_TypeCID from TB_SQM_Manufacturers_BasicInfo where BasicInfoGUID  =@BasicInfoGUID) 
	and [PartID] not in (SELECT [BasicInfoPartID]
                                          FROM TB_SQM_Manufacturers_BasicInfo_Part_Fill_Status
                                          WHERE BasicInfoGUID =@BasicInfoGUID)");
            }
            else
            {
                sb = new StringBuilder();
                sb.Append(@"SELECT [CNAME]   
  FROM [TB_SQM_Manufacturers_BasicInfo_Part_Map]
   inner join [TB_SQM_Manufacturers_BasicInfo_Part] on [TB_SQM_Manufacturers_BasicInfo_Part].CID=[TB_SQM_Manufacturers_BasicInfo_Part_Map].[PartID]
     WHERE VendorTypeID =(SELECT TB_SQM_Vendor_TypeCID from TB_SQM_Manufacturers_BasicInfo where BasicInfoGUID  =@BasicInfoGUID) 
	and [PartID] not in (SELECT [BasicInfoPartID]
                                          FROM TB_SQM_Manufacturers_BasicInfo_Part_Fill_Status
                                          WHERE BasicInfoGUID =@BasicInfoGUID)   and [PartID] <>9");
             
               
            }
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), sqlConnection))
            {
                cmd.Parameters.Add(new SqlParameter("@BasicInfoGUID", dataItem.BasicInfoGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        iRowCount++;
                        sErrMsg = sErrMsg + dr["CNAME"].ToString() + ",";
                    }
                }
            }
            if (!string.IsNullOrEmpty(sErrMsg))
            {
                sErrMsg = sErrMsg.Remove(sErrMsg.Length - 1, 1) + "表單未完成";
            }
            return sErrMsg;
        }

        public static String UploadBasicInfoFile(SqlConnection cn, String RunAsUserGUID, String file ,TB_SQM_Manufacturers_BasicInfo dataItem)
        {
            String r = "";
            String col = "";
            col = "FGUID";

            //00.UploadFileToDB
            SqlTransaction tran = cn.BeginTransaction();
            //String file = sLocalUploadPath + FA.SUBFOLDER + "/" + jo_item.FileName;
            String FGUID = SharedLibs.SqlFileStreamHelper.InsertToTableSQM(cn, tran, RunAsUserGUID, file, "");
            if (FGUID == "")
            {
                tran.Dispose();
                return "can't insert file to DB";
            }

            //01.del esixt file
            try
            {
                DataTable dt = new DataTable();
                StringBuilder sb = new StringBuilder();
                sb.Append(
                @"
                DECLARE @OldKeyValue uniqueidentifier;

                SELECT @OldKeyValue=@col FROM TB_SQM_Manufacturers_BasicInfo
                WHERE VendorCode = @VendorCode
                AND BasicInfoGUID = @BasicInfoGUID;
                
                UPDATE TB_SQM_Manufacturers_BasicInfo 
                   SET @col=null;
                
                DELETE FROM TB_SQM_Files
                WHERE FGUID =@OldKeyValue
                ");
                String sql = Regex.Replace(sb.ToString(), @"\s+", " ");
                sql = Regex.Replace(sql, @"@col", col);

                using (SqlCommand cmd = new SqlCommand(sql, cn, tran))
                {
                    cmd.Parameters.Add(new SqlParameter("@VendorCode", dataItem.VendorCode));
                    cmd.Parameters.Add(new SqlParameter("@BasicInfoGUID", dataItem.BasicInfoGUID));

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                r = ex.ToString();
            }

            //02.Update new FGUID
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(
                @"
                UPDATE TB_SQM_Manufacturers_BasicInfo
                  SET @col = @ColFGUID
                WHERE VendorCode = @VendorCode
                AND BasicInfoGUID = @BasicInfoGUID;
                ");
                String sql = Regex.Replace(sb.ToString(), @"\s+", " ");
                sql = Regex.Replace(sql, @"@col", col);
                using (SqlCommand cmd = new SqlCommand(sql, cn, tran))
                {
                    cmd.Parameters.Add(new SqlParameter("@VendorCode", dataItem.VendorCode));
                    cmd.Parameters.Add(new SqlParameter("@BasicInfoGUID", dataItem.BasicInfoGUID));
                    cmd.Parameters.Add(new SqlParameter("@ColFGUID", FGUID));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                r += ex.ToString();
            }

            //Commit
            try { tran.Commit(); }
            catch (Exception e) { tran.Rollback(); r = "Upload fail.<br />Exception: " + e.ToString(); }
            return r;
        }


        public static String EditScore(SqlConnection cnPortal, String BasicInfoGUID, object Score)
        {
            String sErrMsg = "";
            using (SqlCommand cmd = new SqlCommand("", cnPortal))
            {
                String sSQL = "Update TB_SQM_Manufacturers_BasicInfo Set Score = @Score ";
                sSQL += " Where BasicInfoGUID =@BasicInfoGUID;";
                cmd.CommandText = sSQL;
                cmd.Parameters.AddWithValue("@Score", Score);
                cmd.Parameters.AddWithValue("@BasicInfoGUID", BasicInfoGUID);

                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }
            }
            return sErrMsg;
        }
    }
}
