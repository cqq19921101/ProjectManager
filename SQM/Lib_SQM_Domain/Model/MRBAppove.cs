using Lib_SQM_Domain.SharedLibs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace Lib_SQM_Domain.Model
{
    public class MRBAppove
    {
        public string _SID;
        public string _Plant;
        public string _DepartmentType;
      
        public string SID { get { return this._SID; } set { this._SID = value; } }
        public string Plant { get { return this._Plant; } set { this._Plant = value; } }
        public string DepartmentType { get { return this._DepartmentType; } set { this._DepartmentType = value; } }
     
        public  MRBAppove()
        {

        }
        public MRBAppove(string SID,string Plant,string DepartmentType)
        {
            this._SID = SID;
            this._Plant = Plant;
            this._DepartmentType = DepartmentType;
        }

    }
    public class MRBAppoveMap
    {
        public string _SID;
        public string _SSID;
        public string _Plant;
        public string _Name;
        public string _Email;
        
        public string SID { get { return this._SID; } set { this._SID = value; } }
        public string SSID { get { return this._SSID; } set { this._SSID = value; } }
        public string Name { get { return this._Name; } set { this._Name = value; } }
        public string Plant { get { return this._Plant; } set { this._Plant = value; } }
        public string Email { get { return this._Email; } set { this._Email = value; } }
       
        public MRBAppoveMap(){
            }
        public MRBAppoveMap(string SID,string SSID, string Plant,string Name,string Email)
        {
            this._SID = SID;
            this._SSID = SSID;
            this._Plant = Plant;
            this._Name = Name;
            this._Email = Email;
           
        }
    }
    public class MRBAppove_jQGridJSon
    {
        public List<MRBAppove> Rows = new List<MRBAppove>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }
    public class MRBAppoveMap_jQGridJSon
    {
        public List<MRBAppoveMap> Rows = new List<MRBAppoveMap>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }

    public static class MRBAppove_Helper
    {
        private static void UnescapeDataFromWeb(MRBAppove DataItem)
        {
            DataItem.Plant = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Plant);
            DataItem.DepartmentType = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.DepartmentType);
           
        }










        private static string DataCheck(MRBAppove DataItem)
        {
            string r = "";
            List<string> e = new List<string>();
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.DepartmentType))
                e.Add("Must provide Department.");
         
            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }
        public static string GetDataToJQGridJson(SqlConnection cn)
        {
            return GetDataToJQGridJson(cn, "");
        }
        public static string GetDataToJQGridJson(SqlConnection cn, string MemberGUID)
        {
            MRBAppove_jQGridJSon m = new MRBAppove_jQGridJSon();
            string sWhereClause = "";
            string Plant = GetPlant(cn, MemberGUID);
            m.Rows.Clear();
            int iRowCount = 0;
            string sSQL = "SELECT [SID],[Plant],[DepartmentType] From TB_SQM_MRB_APPOVER WHERE Plant=@Plant";
           
            using (SqlCommand cmd = new SqlCommand(sSQL, cn))
            {

                cmd.Parameters.AddWithValue("@Plant", SQMStringHelper.NullOrEmptyStringIsDBNull(Plant));
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    iRowCount++;
                    m.Rows.Add(new MRBAppove(
                        dr["SID"].ToString(),
                        dr["Plant"].ToString(),
                        dr["DepartmentType"].ToString()
                        ));
                }
                dr.Close();
                dr = null;
            }

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(m);
        }

        public static string CreateDataItem(SqlConnection cn, MRBAppove DataItem, string MemberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            string Plant = GetPlant(cn,MemberGUID);
            if (r != "")
            { return r; }
            else
            {
                string sSQL = "Insert Into TB_SQM_MRB_APPOVER (Plant, DepartmentType) ";
                sSQL += "Values (@Plant, @DepartmentType);";
                SqlCommand cmd = new SqlCommand(sSQL, cn);
              
                cmd.Parameters.AddWithValue("@Plant", SQMStringHelper.NullOrEmptyStringIsDBNull(Plant));
                cmd.Parameters.AddWithValue("@DepartmentType", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.DepartmentType));
                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;

                return sErrMsg;
            }

        }

        private static string GetPlant(SqlConnection cn, string MemberGUID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT top 1 [PlantCode]   
  FROM[dbo].[TB_SQM_Member_Plant]
  where MemberGUID = @MemberGUID
 union all
SELECT top 1 [PlantCode] from
  [dbo].[TB_SQM_Member_Vendor_Map]
  where MemberGUID = @MemberGUID");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.AddWithValue("@MemberGUID", SQMStringHelper.NullOrEmptyStringIsDBNull(MemberGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "";
            }

           
        }

        public static string EditDataItem(SqlConnection cn, MRBAppove DataItem)
        {
            return EditDataItem(cn, DataItem, "", "");
        }

        public static string EditDataItem(SqlConnection cnPortal, MRBAppove DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
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
                    using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { bLockIsStillValid = SQMDataLockHelper.CheckLockIsStillValid(cmd, DataItem.SID, LoginMemberGUID, RunAsMemberGUID); }
                if (!bLockIsStillValid) { tran.Rollback(); return SQMDataLockHelper.LockString(); }

                //Release lock
                if ((LoginMemberGUID != "") && (RunAsMemberGUID != ""))
                    using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = SQMDataLockHelper.ReleaseLock(cmd, DataItem.SID, LoginMemberGUID, RunAsMemberGUID); }
                if (r != "") { tran.Rollback(); return r; }

                //Commit
                try { tran.Commit(); }
                catch (Exception e) { tran.Rollback(); r = "Edit fail.<br />Exception: " + e.ToString(); }
                return r;
            }
        }
        private static string EditDataItemSub(SqlCommand cmd, MRBAppove DataItem)
        {
            string sErrMsg = "";

            string sSQL = "Update TB_SQM_MRB_APPOVER Set DepartmentType = @DepartmentType ";
            sSQL += "Where SID = @SID;";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@DepartmentType", DataItem.DepartmentType);
            cmd.Parameters.AddWithValue("@SID", DataItem.SID);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        public static string DeleteDataItem(SqlConnection cnPortal, MRBAppove DataItem)
        {
            return DeleteDataItem(cnPortal, DataItem, "", "");
        }
        public static string DeleteDataItem(SqlConnection cn, MRBAppove DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            string r = "";
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.SID))
                return "Must provide sid.";
            else
            {
                //SqlTransaction tran = cn.BeginTransaction();

                //Delete member data
                using (SqlCommand cmd = new SqlCommand("", cn)) { r = DeleteDataItemSub(cmd, DataItem); }
                //if (r != "") { tran.Rollback(); return r; }

                ////Release lock
                //if ((LoginMemberGUID != "") && (RunAsMemberGUID != ""))
                //    using (SqlCommand cmd = new SqlCommand("", cn, tran)) { r = SQMDataLockHelper.ReleaseLock(cmd, DataItem.SID, LoginMemberGUID, RunAsMemberGUID); }
                //if (r != "") { tran.Rollback(); return r; }

                ////Commit
                //try { tran.Commit(); }
                //catch (Exception e) { tran.Rollback(); r = "Delete fail.<br />Exception: " + e.ToString(); }
                return r;
            }

        }
        private static string DeleteDataItemSub(SqlCommand cmd, MRBAppove DataItem)
        {
            string sErrMsg = "";

            string sSQL = "Delete TB_SQM_MRB_APPOVER Where SID = @SID;Delete TB_SQM_MRB_APPOVE_MAP Where SSID = @SID;";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@SID", DataItem.SID);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
    }
    public static class MRBAppoveMap_Helper
    {
        private static void UnescapeDataFromWeb(MRBAppoveMap DataItem)
        {
           
            DataItem.Name = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Name);
            DataItem.Email = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Email);
        }

        private static string DataCheck(MRBAppoveMap DataItem)
        {
            string r = "";
            List<string> e = new List<string>();
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.Name))
                e.Add("Must provide name.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.Email))
                e.Add("Must provide email.");
       
            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }
        public static string GetDataToJQGridJson(SqlConnection cn)
        {
            return GetDataToJQGridJson(cn, "","");
        }
        public static string GetDataToJQGridJson(SqlConnection cn, string SearchText,string MemberGUID)
        {
            MRBAppoveMap_jQGridJSon m = new MRBAppoveMap_jQGridJSon();

            string sSearchText = SearchText.Trim();
            string sWhereClause = "";
            string Plant = GetPlant(cn,  MemberGUID);
            m.Rows.Clear();
            int iRowCount = 0;
            string sSQL = "SELECT [SID],SSID,Plant,[Name],[Email] From TB_SQM_MRB_APPOVE_MAP where SSID=@SSID and Plant=@Plant";
         
            using (SqlCommand cmd = new SqlCommand(sSQL, cn))
            {
               
                cmd.Parameters.Add(new SqlParameter("@SSID", SearchText));
                cmd.Parameters.Add(new SqlParameter("@Plant", Plant));
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    iRowCount++;
                    m.Rows.Add(new MRBAppoveMap(

                        dr["SID"].ToString(),
                        dr["SSID"].ToString(),
                        dr["Plant"].ToString(),
                        dr["Name"].ToString(),
                        dr["Email"].ToString()
                       
                        ));
                }
                dr.Close();
                dr = null;
            }

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(m);
        }
        private static string GetPlant(SqlConnection cn, string MemberGUID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT top 1 [PlantCode]   
  FROM[dbo].[TB_SQM_Member_Plant]
  where MemberGUID = @MemberGUID
 union all
SELECT top 1 [PlantCode] from
  [dbo].[TB_SQM_Member_Vendor_Map]
  where MemberGUID = @MemberGUID");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.AddWithValue("@MemberGUID", SQMStringHelper.NullOrEmptyStringIsDBNull(MemberGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "";
            }


        }
        public static string CreateDataItem(SqlConnection cn, MRBAppoveMap DataItem,string MemberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            string Plant = GetPlant(cn, MemberGUID);
            if (r != "")
            { return r; }
            else
            {
                string sSQL = "Insert Into TB_SQM_MRB_APPOVE_MAP (SSID,Plant, Name,Email) ";
                sSQL += "Values (@SSID,@Plant, @Name,@Email);";
                SqlCommand cmd = new SqlCommand(sSQL, cn);

                cmd.Parameters.AddWithValue("@SSID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SSID));
                cmd.Parameters.AddWithValue("@Plant", SQMStringHelper.NullOrEmptyStringIsDBNull(Plant));
                cmd.Parameters.AddWithValue("@Name", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Name));
                cmd.Parameters.AddWithValue("@Email", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Email));
               
                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;

                return sErrMsg;
            }

        }

        public static string EditDataItem(SqlConnection cn, MRBAppoveMap DataItem)
        {
            return EditDataItem(cn, DataItem, "", "");
        }

        public static string EditDataItem(SqlConnection cnPortal, MRBAppoveMap DataItem, string LoginMemberGUID, string RunAsMemberGUID)
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

                //Update member data
                //using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = EditDataItemSub(cmd, DataItem); }
                //if (r != "") { tran.Rollback(); return r; }

                ////Check lock is still valid
                //bool bLockIsStillValid = false;
                //if ((LoginMemberGUID != "") && (RunAsMemberGUID != ""))
                //    using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { bLockIsStillValid = SQMDataLockHelper.CheckLockIsStillValid(cmd, DataItem.SID, LoginMemberGUID, RunAsMemberGUID); }
                //if (!bLockIsStillValid) { tran.Rollback(); return SQMDataLockHelper.LockString(); }

                ////Release lock
                //if ((LoginMemberGUID != "") && (RunAsMemberGUID != ""))
                //    using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = SQMDataLockHelper.ReleaseLock(cmd, DataItem.SID, LoginMemberGUID, RunAsMemberGUID); }
                //if (r != "") { tran.Rollback(); return r; }

                ////Commit
                //try { tran.Commit(); }
                //catch (Exception e) { tran.Rollback(); r = "Edit fail.<br />Exception: " + e.ToString(); }
                return r;
            }
        }
        private static string EditDataItemSub(SqlCommand cmd, MRBAppoveMap DataItem)
        {
            string sErrMsg = "";

            string sSQL = "Update TB_SQM_MRB_APPOVE_MAP Set Name = @Name,Email=@Email ";
            sSQL += "Where SID = @SID;";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@Name", DataItem.Name);
            cmd.Parameters.AddWithValue("@Email", DataItem.Email);
           
            cmd.Parameters.AddWithValue("@SID", DataItem.SID);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        public static string DeleteDataItem(SqlConnection cnPortal, MRBAppoveMap DataItem)
        {
            return DeleteDataItem(cnPortal, DataItem, "", "");
        }
        public static string DeleteDataItem(SqlConnection cn, MRBAppoveMap DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            string r = "";
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.SID))
                return "Must provide sid.";
            else
            {
                SqlTransaction tran = cn.BeginTransaction();

                //Delete member data
                using (SqlCommand cmd = new SqlCommand("", cn, tran)) { r = DeleteDataItemSub(cmd, DataItem); }
                if (r != "") { tran.Rollback(); return r; }

                //Release lock
                if ((LoginMemberGUID != "") && (RunAsMemberGUID != ""))
                    using (SqlCommand cmd = new SqlCommand("", cn, tran)) { r = SQMDataLockHelper.ReleaseLock(cmd, DataItem.SID, LoginMemberGUID, RunAsMemberGUID); }
                if (r != "") { tran.Rollback(); return r; }

                //Commit
                try { tran.Commit(); }
                catch (Exception e) { tran.Rollback(); r = "Delete fail.<br />Exception: " + e.ToString(); }
                return r;
            }

        }
        private static string DeleteDataItemSub(SqlCommand cmd, MRBAppoveMap DataItem)
        {
            string sErrMsg = "";

            string sSQL = "Delete TB_SQM_MRB_APPOVE_MAP Where SID = @SID;";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@SID", DataItem.SID);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
    }
}
