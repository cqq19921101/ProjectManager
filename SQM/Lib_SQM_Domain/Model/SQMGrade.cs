using Lib_Portal_Domain.SharedLibs;
using Lib_SQM_Domain.SharedLibs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Lib_SQM_Domain.Model
{
  public  class SQMGrade
    {
        protected string _SID;
        protected string _Plant;
        protected string _CID;
        protected string _CCID;
        protected string _CName;
        protected string _CCName;
        protected string _NAME;
        protected string _Grade;
        protected string _RowID;

        public string SID { get { return this._SID; } set { this._SID = value; } }
        public string Plant { get { return this._Plant; } set { this._Plant = value; } }
        public string CID { get { return this._CID; } set { this._CID = value; } }

        public string CCID { get { return this._CCID; } set { this._CCID = value; } }
        public string CName { get { return this._CName; } set { this._CName = value; } }

        public string CCName { get { return this._CCName; } set { this._CCName = value; } }
        public string NAME { get { return this._NAME; } set { this._NAME = value; } }
        public string Grade { get { return this._Grade; } set { this._Grade = value; } }
        public string RowID { get { return this._RowID; } set { this._RowID = value; } }
        public SQMGrade()
        {

        }

        public SQMGrade(string SID,
            string Plant,
            string CID,
            string CCID,
            string CName,
            string CCName,
            string NAME,
            string Grade,
            string RowID)
        {
            this._SID = SID;
            this._Plant = Plant;
            this._CID = CID;
            this._CCID = CCID;
            this._CName = CName;
            this._CCName = CCName;
            this._NAME = NAME;
            this._Grade = Grade;
            this._RowID = RowID;
        }
    }

    public class SQMGrade_jQGridJSon
    {
        public List<SQMGrade> Rows = new List<SQMGrade>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }
    public class SQMGrade_Helper
    {
        private static void UnescapeDataFromWeb(SQMGrade DataItem)
        {

            DataItem.SID = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.SID);
            DataItem.Plant = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Plant);
            DataItem.CID = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CID);
            DataItem.CCID = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CCID);
            DataItem.CName = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CName);
            DataItem.CCName = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CCName);
            DataItem.NAME = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.NAME);
            DataItem.Grade= SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Grade);
            DataItem.RowID = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.RowID);
        }
        private static string DataCheck(SQMGrade DataItem)
        {
            string r = "";
            List<string> e = new List<string>();
            if (StringHelper.DataIsNullOrEmpty(DataItem.NAME))
               e.Add("Must provide NAME.");
            if (StringHelper.DataIsNullOrEmpty(DataItem.Grade))
                e.Add("Must provide Grade.");
            if (StringHelper.DataIsNullOrEmpty(DataItem.RowID))
                e.Add("Must provide RowID.");
            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "\u000d";
                r += e[iCnt];
            }

            return r;
        }

        public static string CreateDataItem(SqlConnection cnPortal, SQMGrade DataItem, String memberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            string Plant = GetPlant(cnPortal, memberGUID);
            if (r != "")
            { return r; }
            else
            {
                string sSQL = "Insert Into TB_SQM_Grade ( Plant,CID,CCID,NAME,Grade,RowID) ";
                sSQL += "Values ( @Plant,@CID,@CCID,@NAME,@Grade,@RowID);";
                SqlCommand cmd = new SqlCommand(sSQL, cnPortal);

                cmd.Parameters.AddWithValue("@Plant", Plant);
                cmd.Parameters.AddWithValue("@CID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.CID));
                cmd.Parameters.AddWithValue("@CCID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.CCID));
                cmd.Parameters.AddWithValue("@NAME", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.NAME));
                cmd.Parameters.AddWithValue("@Grade", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Grade));
                cmd.Parameters.AddWithValue("@RowID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.RowID));
                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;

                return sErrMsg;
            }
        }

        public static string EditDataItem(SqlConnection cnPortal, SQMGrade DataItem)
        {
            return EditDataItem(cnPortal, DataItem, "", "");
        }
        public static string EditDataItem(SqlConnection cnPortal, SQMGrade DataItem, string LoginMemberGUID, string RunAsMemberGUID)
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

        private static string EditDataItemSub(SqlCommand cmd, SQMGrade DataItem)
        {
            string sErrMsg = "";

            string sSQL = "Update TB_SQM_Grade Set CID=@CID,CCID=@CCID, NAME=@NAME,Grade=@Grade,RowID=@RowID  ";
            sSQL += " Where SID = @SID;";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@CID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.CID));
            cmd.Parameters.AddWithValue("@CCID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.CCID));
            cmd.Parameters.AddWithValue("@NAME", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.NAME));
            cmd.Parameters.AddWithValue("@Grade", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Grade));
            cmd.Parameters.AddWithValue("@RowID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.RowID));
            cmd.Parameters.AddWithValue("@SID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SID));

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        public static string DeleteDataItem(SqlConnection cnPortal, SQMGrade DataItem)
        {
            return DeleteDataItem(cnPortal, DataItem, "", "");
        }
        public static string DeleteDataItem(SqlConnection cnPortal, SQMGrade DataItem, string LoginMemberGUID, string RunAsMemberGUID)
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
        private static string DeleteDataItemSub(SqlCommand cmd, SQMGrade DataItem)
        {
            string sErrMsg = "";

            string sSQL = "Delete TB_SQM_Grade Where SID = @SID;";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@SID", DataItem.SID);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        public static string GetDataToJQGridJson(SqlConnection cn, string SearchText, string MemberGUID)
        {
            SQMGrade_jQGridJSon m = new SQMGrade_jQGridJSon();
            //string sSearchText = SearchText.Trim();
            //string sWhereClause = "";
            //if (sSearchText != "")
            //    sWhereClause += " and SubFuncName like '%' + @SearchText + '%'";
            //if (sWhereClause.Length != 0)
            //    sWhereClause = " Where" + sWhereClause.Substring(4);

            m.Rows.Clear();
            int iRowCount = 0;
            StringBuilder sb = new StringBuilder();
            string Plant = GetPlant(cn, MemberGUID);
            sb.Append(@"
SELECT [SID]
      ,[Plant]
      ,a.[CID]
      ,[CCID]
	   ,b.cname
      ,c.cname as ccname
      ,[NAME]
      ,[Grade]
      ,RowID
  FROM [dbo].[TB_SQM_Grade] a
   inner join TB_SQM_Commodity b on b.CID=a.CID
  inner join  tb_sqm_commodity_sub c on c.TB_SQM_CommodityCId=b.CID and c.CID=a.CCID
where Plant=@Plant
           ");
            ////sSQL += sWhereClause + ";";
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                //if (sSearchText != "")
                cmd.Parameters.Add(new SqlParameter("@Plant", Plant));
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    iRowCount++;
                    m.Rows.Add(new SQMGrade(
                         dr["SID"].ToString(),
                      dr["Plant"].ToString(),
                      dr["CID"].ToString(),
                      dr["CCID"].ToString(),
                      dr["cname"].ToString(),
                      dr["ccname"].ToString(),
                       dr["NAME"].ToString(),
                       dr["Grade"].ToString(),
                    dr["RowID"].ToString()
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
            return GetDataToJQGridJson(cn, "", "");
        }
    }
}
