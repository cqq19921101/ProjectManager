using Lib_Portal_Domain.Model;
using Lib_SQM_Domain.SharedLibs;
using Newtonsoft.Json;
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
   public class AnnualObjectives
    {
        protected string _SID;
        protected string _Plant;
        protected string _CID;
        protected string _CCID;
        protected string _CName;
        protected string _CCName;
        protected string _AType;
        protected string _MaterialType;
        protected string _Q1;
        protected string _Q2;
        protected string _Q3;
        protected string _Q4;
        public string SID { get { return this._SID; } set { this._SID = value; } }
        public string Plant { get { return this._Plant; } set { this._Plant = value; } }
        public string CID { get { return this._CID; } set { this._CID = value; } }

        public string CCID { get { return this._CCID; } set { this._CCID = value; } }
        public string CName { get { return this._CName; } set { this._CName = value; } }

        public string CCName { get { return this._CCName; } set { this._CCName = value; } }
        public string AType { get { return this._AType; } set { this._AType = value; } }
        public string MaterialType { get { return this._MaterialType; } set { this._MaterialType = value; } }
        
        public string Q1 { get { return this._Q1; } set { this._Q1 = value; } }
        public string Q2 { get { return this._Q2; } set { this._Q2 = value; } }
        public string Q3 { get { return this._Q3; } set { this._Q3 = value; } }
        public string Q4 { get { return this._Q4; } set { this._Q4 = value; } }
        public AnnualObjectives()
        {

        }
        public AnnualObjectives(
            string SID,
            string Plant,
            string CID,
            string CCID,
            string CName,
            string CCName,
          string AType,
          string MaterialType,
            string Q1,
            string Q2,
            string Q3,
            string Q4)
        {
            this._SID = SID;
            this._Plant = Plant;
            this._CID = CID;
            this._CCID = CCID;
            this._CName = CName;
            this._CCName = CCName;
            this._AType = AType;
            this._MaterialType = MaterialType;
            this._Q1 = Q1;
            this._Q2 = Q2;
            this._Q3 = Q3;
            this._Q4 = Q4;
        }
    }

    public class AnnualObjectives_jQGridJSon
    {
        public List<AnnualObjectives> Rows = new List<AnnualObjectives>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }

    public class AnnualObjectives_Helper
    {
        private static void UnescapeDataFromWeb(AnnualObjectives DataItem)
        {

            DataItem.SID = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.SID);
            DataItem.Plant = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Plant);
            DataItem.CID = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CID);
            DataItem.CCID = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CCID);
            DataItem.AType = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.AType);
            DataItem.Q1 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Q1);
            DataItem.Q2 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Q2);
            DataItem.Q3 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Q3);
            DataItem.Q4 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Q4);
        }
        private static string DataCheck(AnnualObjectives DataItem)
        {
            string r = "";
            List<string> e = new List<string>();
            //if (StringHelper.DataIsNullOrEmpty(DataItem.Provider))
            //    e.Add("Must provide Provider.");
            //if (StringHelper.DataIsNullOrEmpty(DataItem.Vendor))
            //    e.Add("Must provide Vendor.");
        
            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }

        public static string CreateDataItem(SqlConnection cnPortal, AnnualObjectives DataItem, String memberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            string Plant = GetPlant(cnPortal, memberGUID);
            if (r != "")
            { return r; }
            else
            {
                string sSQL = "Insert Into TB_SQM_Annual_Objectives ( Plant,TB_SQM_Commodity_SubCID,TB_SQM_Commodity_SubTB_SQM_CommodityCID,AType,MaterialType,Q1,Q2,Q3,Q4) ";
                sSQL += "Values ( @Plant,@TB_SQM_Commodity_SubCID,@TB_SQM_Commodity_SubTB_SQM_CommodityCID,@AType,@MaterialType,@Q1,@Q2,@Q3,@Q4);";
                SqlCommand cmd = new SqlCommand(sSQL, cnPortal);

                cmd.Parameters.AddWithValue("@Plant", Plant);
                cmd.Parameters.AddWithValue("@TB_SQM_Commodity_SubCID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.CID));
                cmd.Parameters.AddWithValue("@TB_SQM_Commodity_SubTB_SQM_CommodityCID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.CCID));
                cmd.Parameters.AddWithValue("@AType", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.AType));
                cmd.Parameters.AddWithValue("@MaterialType", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.MaterialType));
                cmd.Parameters.AddWithValue("@Q1", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Q1));
                cmd.Parameters.AddWithValue("@Q1", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Q1));
                cmd.Parameters.AddWithValue("@Q2", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Q2));
                cmd.Parameters.AddWithValue("@Q3", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Q3));
                cmd.Parameters.AddWithValue("@Q4", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Q4));
                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;

                return sErrMsg;
            }
        }

        public static string EditDataItem(SqlConnection cnPortal, AnnualObjectives DataItem)
        {
            return EditDataItem(cnPortal, DataItem, "", "");
        }
        public static string EditDataItem(SqlConnection cnPortal, AnnualObjectives DataItem, string LoginMemberGUID, string RunAsMemberGUID)
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

        private static string EditDataItemSub(SqlCommand cmd, AnnualObjectives DataItem)
        {
            string sErrMsg = "";

            string sSQL = "Update TB_SQM_Annual_Objectives Set  Q1=@Q1,Q2=@Q2,Q3=@Q3,Q4=@Q4";
            sSQL += " Where SID = @SID;";
            cmd.CommandText = sSQL;
  
            cmd.Parameters.AddWithValue("@Q1", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Q1));
            cmd.Parameters.AddWithValue("@Q2", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Q2));
            cmd.Parameters.AddWithValue("@Q3", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Q3));
            cmd.Parameters.AddWithValue("@Q4", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Q4));
            cmd.Parameters.AddWithValue("@SID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SID));

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        public static string DeleteDataItem(SqlConnection cnPortal, AnnualObjectives DataItem)
        {
            return DeleteDataItem(cnPortal, DataItem, "", "");
        }
        public static String GetTypeList(SqlConnection cn, AnnualObjectives DataItem, string LoginMemberGUID, string RunAsUser)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select distinct MaterialType From TB_SQM_Benefit_Data;");

            DataTable dt = new DataTable();
  
            using (SqlConnection cnSPM = new SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["CZSPMDBConnString2"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand(sb.ToString(), cnSPM))
                {
                    cnSPM.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        dt.Load(dr);
                    }
                }
            }
            return JsonConvert.SerializeObject(dt);
        }
        public static string DeleteDataItem(SqlConnection cnPortal, AnnualObjectives DataItem, string LoginMemberGUID, string RunAsMemberGUID)
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
            sb.Append("Select top 1 PlantCode from TB_SQM_Member_Vendor_Map where MemberGUID=@MemberGUID");
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
        private static string DeleteDataItemSub(SqlCommand cmd, AnnualObjectives DataItem)
        {
            string sErrMsg = "";

            string sSQL = "Delete TB_SQM_Annual_Objectives Where SID = @SID;";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@SID", DataItem.SID);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        public static string GetDataToJQGridJson(SqlConnection cn, string SearchText,string MemberGUID)
        {
            AnnualObjectives_jQGridJSon m = new AnnualObjectives_jQGridJSon();
            //string sSearchText = SearchText.Trim();
            //string sWhereClause = "";
            //if (sSearchText != "")
            //    sWhereClause += " and SubFuncName like '%' + @SearchText + '%'";
            //if (sWhereClause.Length != 0)
            //    sWhereClause = " Where" + sWhereClause.Substring(4);

            m.Rows.Clear();
            int iRowCount = 0;
            StringBuilder sb = new StringBuilder();
            string Plant = GetPlant(cn,MemberGUID);
            sb.Append(@"
SELECT  [SID]
      ,[Plant]

      , b.cname
      ,c.cname as ccname
  ,[TB_SQM_Commodity_SubCID]
      ,[TB_SQM_Commodity_SubTB_SQM_CommodityCID]
      ,AType
       ,MaterialType
      ,[Q1]
      ,[Q2]
      ,[Q3]
      ,[Q4]
  FROM [dbo].[TB_SQM_Annual_Objectives] a
 inner join TB_SQM_Commodity b on b.CID=a.TB_SQM_Commodity_SubCID
  inner join  tb_sqm_commodity_sub c on c.TB_SQM_CommodityCId=b.CID and c.CID=a.TB_SQM_Commodity_SubTB_SQM_CommodityCID
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
                    m.Rows.Add(new AnnualObjectives(
                         dr["SID"].ToString(),
                      dr["Plant"].ToString(),
                      dr["TB_SQM_Commodity_SubCID"].ToString(),
                      dr["TB_SQM_Commodity_SubTB_SQM_CommodityCID"].ToString(),
                      dr["cname"].ToString(),
                      dr["ccname"].ToString(),
                      dr["AType"].ToString(),
                       dr["MaterialType"].ToString(),
                       dr["Q1"].ToString(),
                       dr["Q2"].ToString(),
                       dr["Q3"].ToString(),
                       dr["Q4"].ToString()

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
