using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Web.Script.Serialization;
using Lib_SQM_Domain.SharedLibs;
using System.Web;
using System.Text.RegularExpressions;

namespace Lib_SQM_Domain.Modal
{
    

    #region Data Class Definitions
    public class SystemMgmt_Pro
    {
        protected string _BasicInfoGUID;
        protected string _PrincipalProducts;
        protected string _RevenuePer;
        protected string _MOQ;
        protected string _SampleTime;
        protected string _LeadTime;
        protected string _AnnualCapacity;
        protected string _MajorCompetitor;
        protected string _AnnualCapacitySpan;

        public string BasicInfoGUID { get { return this._BasicInfoGUID; } set { this._BasicInfoGUID = value; } }
        public string PrincipalProducts { get { return this._PrincipalProducts; } set { this._PrincipalProducts = value; } }
        public string RevenuePer { get { return this._RevenuePer; } set { this._RevenuePer = value; } }
        public string MOQ { get { return this._MOQ; } set { this._MOQ = value; } }
        public string SampleTime { get { return this._SampleTime; } set { this._SampleTime = value; } }
        public string LeadTime { get { return this._LeadTime; } set { this._LeadTime = value; } }
        public string AnnualCapacity { get { return this._AnnualCapacity; } set { this._AnnualCapacity = value; } }
        public string MajorCompetitor { get { return this._MajorCompetitor; } set { this._MajorCompetitor = value; } }
        public string AnnualCapacitySpan { get { return this._AnnualCapacitySpan; } set { this._AnnualCapacitySpan = value; } }


        public SystemMgmt_Pro() { }
        public SystemMgmt_Pro(string BasicInfoGUID, string PrincipalProducts, string RevenuePer, string MOQ, string SampleTime, string LeadTime, string AnnualCapacity, string MajorCompetitor,string AnnualCapacitySpan)
        {
           this._BasicInfoGUID = BasicInfoGUID;
            this._PrincipalProducts = PrincipalProducts;
            this._RevenuePer = RevenuePer;
            this._MOQ = MOQ;
            this._SampleTime = SampleTime;
            this._LeadTime = LeadTime;
            this._AnnualCapacity = AnnualCapacity;
            this._MajorCompetitor = MajorCompetitor;
            this._AnnualCapacitySpan = AnnualCapacitySpan;

        }
    }

    public class SystemMgmt_Pro_jQGridJSon
    {
        public List<SystemMgmt_Pro> Rows = new List<SystemMgmt_Pro>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }
    #endregion

    public static class SystemMgmt_Pro_Helper
    {
        #region SearchSubFunc
        public static string GetDataToJQGridJson(SqlConnection cn, string BasicInfoGUID)
        {
            return GetDataToJQGridJson(cn, "", BasicInfoGUID);
        }

        public static string GetDataToJQGridJson(SqlConnection cn, string SearchText, string BasicInfoGUID)
        {
            SystemMgmt_Pro_jQGridJSon m = new SystemMgmt_Pro_jQGridJSon();

            string sSearchText = SearchText.Trim();
            string sWhereClause = "";
            if (sSearchText != "")
                sWhereClause += "   PrincipalProducts = @SearchText   ";
            if (sWhereClause.Length != 0)
                sWhereClause =  "  and " + sWhereClause.Substring(0);

            m.Rows.Clear();
            int iRowCount = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT  BasicInfoGUID,PrincipalProducts,RevenuePer,MOQ,SampleTime,LeadTime,AnnualCapacity,");
            sb.Append("  MajorCompetitor,AnnualCapacitySpan From TB_SQM_ProductDescription where  BasicInfoGUID = @BasicInfoGUID");
            sb.Append(sWhereClause + ";");
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                if (sSearchText != "")
                    cmd.Parameters.Add(new SqlParameter("@SearchText", sSearchText));
                cmd.Parameters.Add(new SqlParameter("@BasicInfoGUID", SQMStringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID)));
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    iRowCount++;
                    m.Rows.Add(new SystemMgmt_Pro(
                        dr["BasicInfoGUID"].ToString(),
                        dr["PrincipalProducts"].ToString(),
                        dr["RevenuePer"].ToString(),
                        dr["MOQ"].ToString(),
                        dr["SampleTime"].ToString(),
                        dr["LeadTime"].ToString(),
                        dr["AnnualCapacity"].ToString(),
                        dr["MajorCompetitor"].ToString()
                        , dr["AnnualCapacitySpan"].ToString()
                        ));
                }
                dr.Close();
                dr = null;
            }

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(m);
        }
        #endregion

        #region Create/Edit data check
        private static void UnescapeDataFromWeb(SystemMgmt_Pro DataItem)
        {
            DataItem.PrincipalProducts = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.PrincipalProducts);
            DataItem.MajorCompetitor = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.MajorCompetitor);
            DataItem.MOQ = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.MOQ);
            DataItem.RevenuePer = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.RevenuePer);
            DataItem.SampleTime = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.SampleTime);
            DataItem.LeadTime = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.LeadTime);
            DataItem.AnnualCapacity = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.AnnualCapacity);
            DataItem.AnnualCapacitySpan = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.AnnualCapacitySpan);


        }

        private static string DataCheck(SystemMgmt_Pro DataItem)
        {
            string r = "";
            List<string> e = new List<string>();
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.PrincipalProducts))
                e.Add("Must provide PrincipalProducts.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.RevenuePer))
                e.Add("Must provide RevenuePer.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.MOQ))
                e.Add("Must provide MOQ.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.SampleTime))
                e.Add("Must provide SampleTime.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.LeadTime))
                e.Add("Must provide LeadTime.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.AnnualCapacity))
                e.Add("Must provide AnnualCapacity.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.AnnualCapacitySpan))
                e.Add("Must provide AnnualCapacitySpan.");

            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }
        #endregion

        #region Create data item
        public static string CreateDataItem(SqlConnection cnPortal, SystemMgmt_Pro DataItem)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            if (r != "")
            { return r;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" Insert Into TB_SQM_ProductDescription (BasicInfoGUID,PrincipalProducts,RevenuePer,MOQ,SampleTime,LeadTime,AnnualCapacity,MajorCompetitor,AnnualCapacitySpan)");
                sb.Append(" Values (@BasicInfoGUID,@PrincipalProducts, @RevenuePer,@MOQ,@SampleTime,@LeadTime,@AnnualCapacity,@MajorCompetitor,@AnnualCapacitySpan)");
                SQM_Basic_Helper.InsertPart(sb,"3");
                SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal);
                cmd.Parameters.AddWithValue("@BasicInfoGUID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.BasicInfoGUID));
                cmd.Parameters.AddWithValue("@PrincipalProducts", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.PrincipalProducts));
                cmd.Parameters.AddWithValue("@RevenuePer", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.RevenuePer));
                cmd.Parameters.AddWithValue("@MOQ", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.MOQ));
                cmd.Parameters.AddWithValue("@SampleTime", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SampleTime));
                cmd.Parameters.AddWithValue("@LeadTime", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.LeadTime));
                cmd.Parameters.AddWithValue("@AnnualCapacity", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.AnnualCapacity));
                cmd.Parameters.AddWithValue("@MajorCompetitor", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.MajorCompetitor));
                cmd.Parameters.AddWithValue("@AnnualCapacitySpan", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.AnnualCapacitySpan));

                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;

                return sErrMsg;
            }
        }

     
        #endregion

        #region Edit data item
        public static string EditDataItem(SqlConnection cnPortal, SystemMgmt_Pro DataItem)
        {
            return EditDataItem(cnPortal, DataItem, "", "");
        }

        public static string EditDataItem(SqlConnection cnPortal, SystemMgmt_Pro DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            if (r != "")
            { return r; }
            else
            {
                using (SqlCommand cmd = new SqlCommand("", cnPortal)) { r = EditDataItemSub(cmd, DataItem); }
                if (r != "") { return r; }
                return r;
            }
        }

        private static string EditDataItemSub(SqlCommand cmd, SystemMgmt_Pro DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(" Update TB_SQM_ProductDescription Set");
            sb.Append(" RevenuePer= @RevenuePer,MOQ = @MOQ,SampleTime = @SampleTime,");
            sb.Append(" LeadTime = @LeadTime,AnnualCapacity = @AnnualCapacity,MajorCompetitor = @MajorCompetitor,AnnualCapacitySpan=@AnnualCapacitySpan");
            sb.Append(" Where BasicInfoGUID = @BasicInfoGUID and PrincipalProducts = @PrincipalProducts;");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@BasicInfoGUID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.BasicInfoGUID));
            cmd.Parameters.AddWithValue("@PrincipalProducts", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.PrincipalProducts));
            cmd.Parameters.AddWithValue("@RevenuePer", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.RevenuePer));
            cmd.Parameters.AddWithValue("@MOQ", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.MOQ));
            cmd.Parameters.AddWithValue("@SampleTime", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SampleTime));
            cmd.Parameters.AddWithValue("@LeadTime", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.LeadTime));
            cmd.Parameters.AddWithValue("@AnnualCapacity", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.AnnualCapacity));
            cmd.Parameters.AddWithValue("@MajorCompetitor", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.MajorCompetitor));
            cmd.Parameters.AddWithValue("@AnnualCapacitySpan", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.AnnualCapacitySpan));

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion

        #region Delete data item
        public static string DeleteDataItem(SqlConnection cnPortal, SystemMgmt_Pro DataItem)
        {
            return DeleteDataItem(cnPortal, DataItem, "", "");
        }

        public static string DeleteDataItem(SqlConnection cnPortal, SystemMgmt_Pro DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {

            string r = "";
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.PrincipalProducts))
                return "Must provide PrincipalProducts.";
            else
            {
                using (SqlCommand cmd = new SqlCommand("", cnPortal)) {r = DeleteDataItemSub(cmd, DataItem);}
                if (r != "") { return r; }

                //SqlTransaction tran = cnPortal.BeginTransaction();

                //using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = DeleteDataItemSub(cmd, DataItem); }
                //if (r != "") { tran.Rollback(); return r; }

                //if ((LoginMemberGUID != "") && (RunAsMemberGUID != ""))
                //    using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = DataLockHelper.ReleaseLock(cmd, DataItem.BasicInfoGUID, LoginMemberGUID, RunAsMemberGUID); }
                //if (r != "") { tran.Rollback(); return r; }

                //try { tran.Commit(); }
                //catch (Exception e) { tran.Rollback(); r = "Delete fail.<br />Exception: " + e.ToString(); }
                return r;
            }
        }

        private static string DeleteDataItemSub(SqlCommand cmd, SystemMgmt_Pro DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append("Delete TB_SQM_ProductDescription Where PrincipalProducts = @PrincipalProducts");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@PrincipalProducts", DataItem.PrincipalProducts);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion

    }
}
