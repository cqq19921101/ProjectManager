using Lib_SQM_Domain.SharedLibs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.Script.Serialization;

namespace Lib_SQM_Domain.Model
{
    public class SQEContact
    {
        protected string _SID;
        protected string _Provider;
        protected string _Vendor;
        protected string _jobID;
        protected string _job;
        protected string _Name;
        protected string _Phone;
        protected string _FixedTelephone;
        protected string _Email;
        public string SID { get { return this._SID; } set { this._SID = value; } }
        public string Provider { get { return this._Provider; } set { this._Provider = value; } }
        public string Vendor { get { return this._Vendor; } set { this._Vendor = value; } }
        public string jobID { get { return this._jobID; } set { this._jobID = value; } }
        public string job { get { return this._job; } set { this._job = value; } }
        public string Name { get { return this._Name; } set { this._Name = value; } }
        public string Phone { get { return this._Phone; } set { this._Phone = value; } }
        public string FixedTelephone { get { return this._FixedTelephone; } set { this._FixedTelephone = value; } }
        public string Email { get { return this._Email; } set { this._Email = value; } }

        public SQEContact() { }
        public SQEContact(string SID, string Provider, string Vendor, string jobID, string job, string Name, string Phone, string FixedTelephone, string Email)
        {
            this._SID = SID;
            this._Provider = Provider;
            this._Vendor = Vendor;
            this._jobID = jobID;
            this._job = job;
            this._Name = Name;
            this._Phone = Phone;
            this._FixedTelephone = FixedTelephone;
            this._Email = Email;
        }
    }

    public class SQEContact_jQGridJSon
    {
        public List<SQEContact> Rows = new List<SQEContact>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }

    public static class SQEContact_Helper
    {
        #region Create/Edit data check
        private static void UnescapeDataFromWeb(SQEContact DataItem)
        {

            DataItem.Name = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Name);
            DataItem.Phone = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Phone);
            DataItem.FixedTelephone = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.FixedTelephone);
            DataItem.Email = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Email);
        }
        private static string DataCheck(SQEContact DataItem)
        {
            string r = "";
            List<string> e = new List<string>();
            //if (StringHelper.DataIsNullOrEmpty(DataItem.Provider))
            //    e.Add("Must provide Provider.");
            //if (StringHelper.DataIsNullOrEmpty(DataItem.Vendor))
            //    e.Add("Must provide Vendor.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.job))
                e.Add("Must provide job.");
            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }

        public static string GetJobList(SqlConnection cn)
        {


            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT CID,CNAME FROM [dbo].[TB_SQM_Contact_Type]");

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
        #endregion


        public static string CreateDataItem(SqlConnection cnPortal, SQEContact DataItem, String MemberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);

            if (r != "")
            { return r; }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"

Insert Into TB_SQM_Contact ( VendorCode,TB_SQM_Contact_TypeCID,CName,Phone,FixedTelephone,Email)
Values ( @VendorCode,@TB_SQM_Contact_TypeCID,@CName,@Phone,@FixedTelephone,@Email)

");
                SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal);

                cmd.Parameters.AddWithValue("@VendorCode", MemberGUID);
                cmd.Parameters.AddWithValue("@TB_SQM_Contact_TypeCID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.job));
                cmd.Parameters.AddWithValue("@CName", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Name));
                cmd.Parameters.AddWithValue("@Phone", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Phone));
                cmd.Parameters.AddWithValue("@FixedTelephone", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.FixedTelephone));
                cmd.Parameters.AddWithValue("@Email", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Email));

                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;

                return sErrMsg;
            }
        }

        public static string EditDataItem(SqlConnection cnPortal, SQEContact DataItem)
        {
            return EditDataItem(cnPortal, DataItem, "", "");
        }
        public static string EditDataItem(SqlConnection cnPortal, SQEContact DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            if (r != "")
            { return r; }
            else
            {
                using (SqlCommand cmd = new SqlCommand("", cnPortal)) { r = EditDataItemSub(cmd, DataItem); }
                if (r != "") { return r; }
                //SqlTransaction tran = cnPortal.BeginTransaction();

                ////Update member data
                //using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = EditDataItemSub(cmd, DataItem); }
                //if (r != "") { tran.Rollback(); return r; }

                ////Check lock is still valid
                //bool bLockIsStillValid = false;
                //if ((LoginMemberGUID != "") && (RunAsMemberGUID != ""))
                //    using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { bLockIsStillValid = DataLockHelper.CheckLockIsStillValid(cmd, DataItem.SID, LoginMemberGUID, RunAsMemberGUID); }
                //if (!bLockIsStillValid) { tran.Rollback(); return DataLockHelper.LockString(); }

                ////Release lock
                //if ((LoginMemberGUID != "") && (RunAsMemberGUID != ""))
                //    using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = DataLockHelper.ReleaseLock(cmd, DataItem.SID, LoginMemberGUID, RunAsMemberGUID); }
                //if (r != "") { tran.Rollback(); return r; }

                ////Commit
                //try { tran.Commit(); }
                //catch (Exception e) { tran.Rollback(); r = "Edit fail.<br />Exception: " + e.ToString(); }
                return r;
            }
        }

        private static string EditDataItemSub(SqlCommand cmd, SQEContact DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
Update TB_SQM_Contact Set TB_SQM_Contact_TypeCID=@TB_SQM_Contact_TypeCID,CName=@CName, Phone=@Phone,FixedTelephone=@FixedTelephone,Email=@Email
Where SID = @SID
");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@TB_SQM_Contact_TypeCID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.job));
            cmd.Parameters.AddWithValue("@CName", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Name));
            cmd.Parameters.AddWithValue("@Phone", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Phone));
            cmd.Parameters.AddWithValue("@FixedTelephone", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.FixedTelephone));
            cmd.Parameters.AddWithValue("@Email", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Email));
            cmd.Parameters.AddWithValue("@SID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SID));

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        public static string DeleteDataItem(SqlConnection cnPortal, SQEContact DataItem)
        {
            return DeleteDataItem(cnPortal, DataItem, "", "");
        }
        public static string DeleteDataItem(SqlConnection cnPortal, SQEContact DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            string r = "";
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.SID))
                return "Must provide SID.";
            else
            {
                using (SqlCommand cmd = new SqlCommand("", cnPortal)) { r = DeleteDataItemSub(cmd, DataItem); }
                if (r != "") { return r; }
                //SqlTransaction tran = cnPortal.BeginTransaction();

                ////Delete member data
                //using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = DeleteDataItemSub(cmd, DataItem); }
                //if (r != "") { tran.Rollback(); return r; }

                ////Release lock
                //if ((LoginMemberGUID != "") && (RunAsMemberGUID != ""))
                //    using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = DataLockHelper.ReleaseLock(cmd, DataItem.SID, LoginMemberGUID, RunAsMemberGUID); }
                //if (r != "") { tran.Rollback(); return r; }

                ////Commit
                //try { tran.Commit(); }
                //catch (Exception e) { tran.Rollback(); r = "Delete fail.<br />Exception: " + e.ToString(); }
                return r;
            }
        }
        private static string DeleteDataItemSub(SqlCommand cmd, SQEContact DataItem)
        {
            string sErrMsg = "";

            string sSQL = "Delete TB_SQM_Contact Where SID = @SID;";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@SID", DataItem.SID);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        public static string GetDataToJQGridJson(SqlConnection cn, string SearchText,String MemberGUID)
        {
            string sSearchText = SearchText.Trim();
            string sWhereClause = "";
            if (sSearchText != "")
                sWhereClause += "   C.CName  like '%' + @SearchText + '%'    ";
            if (sWhereClause.Length != 0)
                sWhereClause = "  AND " + sWhereClause.Substring(0);

            StringBuilder sb = new StringBuilder();
            sb.Append(@"
SELECT 
        C.SID
		,C.VendorCode
		,PORTAL_Members.NameInChinese as Vendor
		,CT.CNAME AS job
		,TB_SQM_Contact_TypeCID AS jobID
		,C.CName AS Name
		,TB_VMI_VENDOR_DETAIL.ERP_VNAME AS Provider
		,Phone,FixedTelephone,C.Email 
FROM TB_SQM_Contact AS C  WITH (NOLOCK) 
        INNER JOIN TB_SQM_Contact_Type AS CT ON  CT.CID = C.TB_SQM_Contact_TypeCID 
		--VendorCode
		LEFT JOIN TB_SQM_Member_Vendor_Map AS MVM ON MVM.VendorCode = C.VendorCode
		--USER
		INNER join PORTAL_Members on PORTAL_Members.MemberGUID=C.VendorCode
		--user
		--LEFT JOIN TB_EB_USER U1 ON U1.USER_GUID = MVM.MemberGUID
		 left join TB_VMI_VENDOR_DETAIL    on TB_VMI_VENDOR_DETAIL.ERP_VND=(SELECT [VendorCode] FROM [TB_SQM_Member_Vendor_Map] WHERE [MemberGUID]=@MemberGUID)
WHERE C.VendorCode = @MemberGUID

           ");
            DataTable dt = new DataTable();
            sb.Append(sWhereClause + ";");
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@MemberGUID", MemberGUID));
                cmd.Parameters.Add(new SqlParameter("@SearchText", SearchText));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }
        //public static string GetDataToJQGridJson(SqlConnection cn)
        //{
        //    return GetDataToJQGridJson(cn, "");
        //}
    }
}
