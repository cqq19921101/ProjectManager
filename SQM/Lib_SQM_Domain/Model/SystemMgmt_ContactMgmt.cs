using Lib_SQM_Domain.SharedLibs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using Lib_Portal_Domain.Model;
using Aspose.Cells;
using System.Collections;

namespace Lib_SQM_Domain.Model
{
    public class SQMMgmt_Contact
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

        public SQMMgmt_Contact() { }
        public SQMMgmt_Contact(string SID,  string Vendor, string jobID, string job, string Name, string Phone, string FixedTelephone, string Email)
        {
            this._SID = SID;
            this._Vendor = Vendor;
            this._jobID = jobID;
            this._job = job;
            this._Name = Name;
            this._Phone = Phone;
            this._FixedTelephone = FixedTelephone;
            this._Email = Email;
        }
    }

    public class SQMMgmt_Contact_jQGridJSon
    {
        public List<SQMMgmt_Contact> Rows = new List<SQMMgmt_Contact>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }

    public static class SQMMgmt_Contact_Helper
    {
        #region Create/Edit data check
        private static void UnescapeDataFromWeb(SQMMgmt_Contact DataItem)
        {

            DataItem.Name = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Name);
            DataItem.Phone = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Phone);
            DataItem.FixedTelephone = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.FixedTelephone);
            DataItem.Email = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Email);
        }
        private static string DataCheck(SQMMgmt_Contact DataItem)
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

   
        public static string CreateDataItem(SqlConnection cnPortal, SQMMgmt_Contact DataItem,String memberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);

            if (r != "")
            { return r; }
            else
            {
                string sSQL = "Insert Into TB_SQM_Contact ( VendorCode,TB_SQM_Contact_TypeCID,CName,Phone,FixedTelephone,Email) ";
                sSQL += "Values ( @VendorCode,@TB_SQM_Contact_TypeCID,@CName,@Phone,@FixedTelephone,@Email);";
                SqlCommand cmd = new SqlCommand(sSQL, cnPortal);

                cmd.Parameters.AddWithValue("@VendorCode", memberGUID);
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

        public static string EditDataItem(SqlConnection cnPortal, SQMMgmt_Contact DataItem)
        {
            return EditDataItem(cnPortal, DataItem, "", "");
        }
        public static string EditDataItem(SqlConnection cnPortal, SQMMgmt_Contact DataItem, string LoginMemberGUID, string RunAsMemberGUID)
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

        private static string EditDataItemSub(SqlCommand cmd, SQMMgmt_Contact DataItem)
        {
            string sErrMsg = "";

            string sSQL = "Update TB_SQM_Contact Set TB_SQM_Contact_TypeCID=@TB_SQM_Contact_TypeCID,CName=@CName, Phone=@Phone,FixedTelephone=@FixedTelephone,Email=@Email";
            sSQL += " Where SID = @SID;";
            cmd.CommandText = sSQL;
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
        public static string DeleteDataItem(SqlConnection cnPortal, SQMMgmt_Contact DataItem)
        {
            return DeleteDataItem(cnPortal, DataItem, "", "");
        }
        public static string DeleteDataItem(SqlConnection cnPortal, SQMMgmt_Contact DataItem, string LoginMemberGUID, string RunAsMemberGUID)
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
        private static string DeleteDataItemSub(SqlCommand cmd, SQMMgmt_Contact DataItem)
        {
            string sErrMsg = "";

            string sSQL = "Delete TB_SQM_Contact Where SID = @SID;";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@SID", DataItem.SID);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        public static string GetDataToJQGridJson(SqlConnection cn, string SearchText,string MemberGUID)
        {
            SQMMgmt_Contact_jQGridJSon m = new SQMMgmt_Contact_jQGridJSon();
            string sSearchText = SearchText.Trim();
            string sWhereClause = "";
            if (sSearchText != "")
                sWhereClause += " and TB_SQM_Contact.CName like '%' + @SearchText + '%'";
            //if (sWhereClause.Length != 0)
            //    sWhereClause = " Where" + sWhereClause.Substring(4);

            m.Rows.Clear();
            int iRowCount = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
SELECT  SID, PORTAL_Members.NameInChinese as Vendor,TB_SQM_Contact_Type.CNAME as job,TB_SQM_Contact_TypeCID
            ,TB_SQM_Contact.CName as Name,Phone,FixedTelephone,Email From TB_SQM_Contact WITH (NOLOCK) 
        inner join TB_SQM_Contact_Type on  TB_SQM_Contact_Type.CID = TB_SQM_Contact.TB_SQM_Contact_TypeCID 
  
		inner join PORTAL_Members on PORTAL_Members.MemberGUID=TB_SQM_Contact.VendorCode
        where  TB_SQM_Contact.VendorCode=@VendorCode
           ");
            string ssSQL = sb.ToString()+ sWhereClause + ";";
            using (SqlCommand cmd = new SqlCommand(ssSQL, cn))
            {
                
                    cmd.Parameters.Add(new SqlParameter("@VendorCode", MemberGUID));
                if (sSearchText != "")
                    cmd.Parameters.Add(new SqlParameter("@SearchText", sSearchText));
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    iRowCount++;
                    m.Rows.Add(new SQMMgmt_Contact(
                        dr["SID"].ToString(),
                     
                        dr["Vendor"].ToString(),
                        dr["TB_SQM_Contact_TypeCID"].ToString(),
                        dr["job"].ToString(),
                        dr["Name"].ToString(),
                        dr["Phone"].ToString(),
                        dr["FixedTelephone"].ToString(),
                        dr["Email"].ToString()
                       ));
                }
                dr.Close();
                dr = null;
            }

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(m);
        }
        public static string GetDataToJQGridJson(SqlConnection cn)
        {
            return GetDataToJQGridJson(cn, "","");
        }
    }
}