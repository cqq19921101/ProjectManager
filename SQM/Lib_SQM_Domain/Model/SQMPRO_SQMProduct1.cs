using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using Lib_Portal_Domain.Model;
using Lib_SQM_Domain.SharedLibs;

namespace Lib_SQM_Domain.Model
{
    public class SQMPRO_SQMProduct1
    {
        protected string _VendorCode;
        protected int _HRType = 0;
        protected string _CName = "";
        protected int _EmployeeQty = 0;
        protected int _EmployeePlanned = 0;
        protected int _AverageJobSeniority = 0;
        protected string _Percent = "";
        protected int _Remark = 0;

        public string VendorCode { get { return this._VendorCode; } set { this._VendorCode = value; } }
        public string HRType { get { return Convert.ToString(this._HRType); } set { this._HRType = int.Parse(value); } }
        public string CName { get { return this._CName; } set { this._CName = value; } }
        public string EmployeeQty { get { return Convert.ToString(this._EmployeeQty); } set { this._EmployeeQty = int.Parse(value); } }
        public string EmployeePlanned { get { return Convert.ToString(this._EmployeePlanned); }  set { this._EmployeePlanned = int.Parse(value); } }
        public string AverageJobSeniority { get { return Convert.ToString(this._AverageJobSeniority); } set { this._AverageJobSeniority = int.Parse(value); } }
        public string Percent { get { return _Percent; } }

        public SQMPRO_SQMProduct1() { }

        public SQMPRO_SQMProduct1(int HRType, string CName, int iEmployeeQty, int iEmployeeQtyTotal, int iEmployeePlanned, int iAverageJobSeniority)
        {
            this._HRType = HRType;
            this._CName = CName;
            this._EmployeeQty = iEmployeeQty;
            this._EmployeePlanned = iEmployeePlanned;
            this._AverageJobSeniority = iAverageJobSeniority;
            this._Percent = Convert.ToString(Math.Round(iEmployeeQty * 1.00 / iEmployeeQtyTotal * 100, 2)) + "%";
            this._Remark = iEmployeeQty + iAverageJobSeniority;
        }
    }

    public class SystemMgmt_Product1_jQGridJSon
    {
        public List<SQMPRO_SQMProduct1> Rows = new List<SQMPRO_SQMProduct1>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }


    public static class SQMPRO_SQMProduct1_Helper
    {

        public static string GetDataToJQGridJson(SqlConnection cn, string SearchText, string VendorCode)
        {
            SystemMgmt_Product1_jQGridJSon m = new SystemMgmt_Product1_jQGridJSon();
            string sSearchText = SearchText.Trim();

            m.Rows.Clear();
            int iRowCount = 0;
            string sSQL = "select Top 100 T1.TB_SQM_HR_TYPECID, T2.CNAME, T1.EmployeeQty, (select sum(EmployeeQty) from TB_SQM_HR where VendorCode=@VendorCode) as EmployeeQtyTotal,T1.EmployeePlanned, T1.AverageJobSeniority from TB_SQM_HR T1, TB_SQM_HR_TYPE T2 where T1.TB_SQM_HR_TYPECID=T2.CID and T1.VendorCode=@VendorCode";
            if (sSearchText != "")
                sSQL += " and T2.CNAME=@sSearchText";

            SqlCommand cmd = new SqlCommand(sSQL, cn);
            cmd.Parameters.Add(new SqlParameter("@VendorCode", VendorCode));

            if (sSearchText != "")
                cmd.Parameters.Add(new SqlParameter("@SearchText", sSearchText));

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                iRowCount++;
                m.Rows.Add(new SQMPRO_SQMProduct1(
                    int.Parse(dr["TB_SQM_HR_TYPECID"].ToString()),
                    dr["CNAME"].ToString(),
                    int.Parse(dr["EmployeeQty"].ToString()),
                    int.Parse(dr["EmployeeQtyTotal"].ToString()),
                    int.Parse(dr["EmployeePlanned"].ToString()),
                    int.Parse(dr["AverageJobSeniority"].ToString())
                    ));
            }
            dr.Close();
            dr = null;
            cmd = null;

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(m);
        }

        public static string GetCommodityList(SqlConnection cn)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select CID, CNAME From TB_SQM_HR_TYPE Order By CID;");
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

        public static string GetHRCategoryList(SqlConnection cn, PortalUserProfile RunAsUser)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select CID, CNAME From TB_SQM_HR_TYPE Order By CID;");
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

        public static string GetTotalData(SqlConnection cn, PortalUserProfile RunAsUser, string VendorCode)
        {
            SqlTransaction tran = cn.BeginTransaction();
            string sErrMsg = "";
            DataTable dt = new DataTable();

            string sSQL = "select sum(EmployeeQty) as EmployeeQty, sum(EmployeePlanned) as EmployeePlanned from TB_SQM_HR where VendorCode=@VendorCode";
            using (SqlCommand cmd = new SqlCommand(sSQL, cn, tran))
            {
                cmd.Parameters.AddWithValue("@VendorCode", VendorCode);
                try
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        dt.Load(dr);
                    }
                }
                catch (Exception e)
                {
                    sErrMsg = "GetTotal fail.<br />Exception: " + e.ToString();
                }
            }

            if (sErrMsg == "")
                tran.Commit();
            else
                tran.Rollback();

            return JsonConvert.SerializeObject(dt);
        }

        public static string CreateDataItem(SqlConnection cnPortal, SQMPRO_SQMProduct1 DataItem)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            if (r != "")
            {
                return r;
            }
            else
            {
                SqlTransaction tran = cnPortal.BeginTransaction();
                string sErrMsg = "";

                string sSQL = "Insert into TB_SQM_HR (VendorCode, TB_SQM_HR_TYPECID, EmployeeQty, EmployeePlanned, AverageJobSeniority) Values (@VendorCode, @TB_SQM_HR_TYPECID, @EmployeeQty, @EmployeePlanned, @AverageJobSeniority)";
                using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal, tran))
                {
                    cmd.Parameters.AddWithValue("@VendorCode", DataItem.VendorCode);
                    cmd.Parameters.AddWithValue("@TB_SQM_HR_TYPECID", int.Parse(DataItem.HRType));
                    cmd.Parameters.AddWithValue("@EmployeeQty", int.Parse(DataItem.EmployeeQty));
                    cmd.Parameters.AddWithValue("@EmployeePlanned", int.Parse(DataItem.EmployeePlanned));
                    cmd.Parameters.AddWithValue("@AverageJobSeniority", int.Parse(DataItem.AverageJobSeniority));

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        sErrMsg = "Create fail.<br />Exception: " + ex.ToString();
                    }
                }

                if (sErrMsg == "")
                    tran.Commit();
                else
                    tran.Rollback();

                return sErrMsg;
            }
        }

        private static void UnescapeDataFromWeb(SQMPRO_SQMProduct1 DataItem)
        {
            DataItem.VendorCode = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.VendorCode);
            DataItem.CName = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CName);
        }

        private static string DataCheck(SQMPRO_SQMProduct1 DataItem)
        {
            string r = "";
            List<string> e = new List<string>();
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.VendorCode))
                e.Add("VendorCode is NULL or Empty.");

            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.CName))
                e.Add("Must provide CName.");

            for (int iCnt = 0; iCnt < e.Count; ++iCnt)
            {
                if (iCnt > 0)
                    r += "<br />";

                r += e[iCnt];
            }

            return r;
        }

        public static string EditDataItem(SqlConnection cnPortal, SQMPRO_SQMProduct1 DataItem)
        {
            return EditDataItem(cnPortal, DataItem, "", "");
        }

        public static string EditDataItem(SqlConnection cnPortal, SQMPRO_SQMProduct1 DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);

            if (r != "")
            {
                return r;
            }
            else
            {
                SqlTransaction tran = cnPortal.BeginTransaction();
                using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = EditDataItemSub(cmd, DataItem); }

                if (r != "")
                {
                    tran.Rollback();
                    return r;
                }

                try
                {
                    tran.Commit();
                }
                catch (Exception e)
                {
                    tran.Rollback();
                    r = "Edit fail.<br />Exception: " + e.ToString();
                }
                return r;
            }
        }

        private static string EditDataItemSub(SqlCommand cmd, SQMPRO_SQMProduct1 DataItem)
        {
            string sErrMsg = "";
            string sSQL = "Update TB_SQM_HR set EmployeeQty=@EmployeeQty, EmployeePlanned=@EmployeePlanned, AverageJobSeniority=@AverageJobSeniority where VendorCode=@VendorCode and TB_SQM_HR_TYPECID=@TB_SQM_HR_TYPECID";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@VendorCode", DataItem.VendorCode);
            cmd.Parameters.AddWithValue("@TB_SQM_HR_TYPECID", int.Parse(DataItem.HRType));
            cmd.Parameters.AddWithValue("@EmployeeQty", int.Parse(DataItem.EmployeeQty));
            cmd.Parameters.AddWithValue("@EmployeePlanned", int.Parse(DataItem.EmployeePlanned));
            cmd.Parameters.AddWithValue("@AverageJobSeniority", int.Parse(DataItem.AverageJobSeniority));

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                sErrMsg = "Edit fail.<br />Exception: " + e.ToString();
            }
            return sErrMsg;
        }

        public static string DeleteDataItem(SqlConnection cnPortal, SQMPRO_SQMProduct1 DataItem)
        {
            UnescapeDataFromWeb(DataItem);
            string r = "";
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.CName))
            {
                return "Must provide CName.";
            }
            else
            {
                SqlTransaction tran = cnPortal.BeginTransaction();
                using (SqlCommand cmd = new SqlCommand("", cnPortal, tran))
                {
                    string SQL = "delete from TB_SQM_HR where VendorCode=@VendorCode and TB_SQM_HR_TYPECID=@TB_SQM_HR_TYPECID";
                    cmd.CommandText = SQL;
                    cmd.Parameters.AddWithValue("@VendorCode", DataItem.VendorCode);
                    cmd.Parameters.AddWithValue("@TB_SQM_HR_TYPECID", DataItem.HRType);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        r = "Delete fail.<br />Exception: " + e.ToString();
                    }
                }

                if (r != "")
                {
                    tran.Rollback();
                    return r;
                }

                try
                {
                    tran.Commit();
                }
                catch (Exception e)
                {
                    tran.Rollback();
                    r = "Delete fail.<br />Exception: " + e.ToString();
                }
                return r;
            }
        }

        private static string DeleteDataItemSub(SqlCommand cmd, string SQL, string ParaName, string DataItemKey)
        {
            string sErrMsg = "";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue(ParaName, DataItemKey);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                sErrMsg = "Delete fail.<br />Exception: " + e.ToString();
            }

            return sErrMsg;
        }

        public static string GetMapVendorCode(SqlConnection cn, PortalUserProfile RunAsUser)
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
    }
}
