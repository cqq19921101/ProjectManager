using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using Lib_Portal_Domain.Model;
using System.Data;
using Newtonsoft.Json;
using Lib_SQM_Domain.SharedLibs;
using Lib_SQM_Domain.Modal;

namespace Lib_SQM_Domain.Model
{
    public class SQMProcess_ProcessInfo
    {
        protected string _BasicInfoGUID = "";
        protected string _VendorCode = "";
        protected int _ProcessType = 0;
        protected string _CName = "";
        protected string _CNameInput = "";
        protected string _ProcessName = "";
        protected string _OwnOrOut = "";
        protected string _ExternalSupplierName = "";

        public string VendorCode { get { return this._VendorCode; } set { this._VendorCode = value; } }
        public string BasicInfoGUID { get { return this._BasicInfoGUID; } set { this._BasicInfoGUID = value; } }
        public string ProcessType { get { return Convert.ToString(this._ProcessType); } set { this._ProcessType = int.Parse(value); } }
        public string CName { get { return this._CName; } set { this._CName = value; } }
        public string CNameInput { get { return this._CNameInput; } set { this._CNameInput = value; } }
        public string ProcessName { get { return this._ProcessName; } set { this._ProcessName = value; } }
        public string OwnOrOut { get { return this._OwnOrOut; } set { this._OwnOrOut = value; } }
        public string ExternalSupplierName { get { return this._ExternalSupplierName; } set { this._ExternalSupplierName = value; } }

        public SQMProcess_ProcessInfo() { }

        public SQMProcess_ProcessInfo(string BasicInfoGUID, int ProcessType, string CName, string ProcessName, string OwnOrOut, string ExternalSupplierName)
        {
            this._BasicInfoGUID = BasicInfoGUID;
            this._ProcessType = ProcessType;
            this._CName = CName;
            this._ProcessName = ProcessName;
            this._OwnOrOut = OwnOrOut;
            this._ExternalSupplierName = ExternalSupplierName;
        }
    }

    public class SQMProcess_ProcessInfo_jQGridJSon
    {
        public List<SQMProcess_ProcessInfo> Rows = new List<SQMProcess_ProcessInfo>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }

    public static class SQMProcess_ProcessInfo_Helper
    {
        public static string GetDataToJQGridJson(SqlConnection cn, string SearchText, string BasicInfoGUID)
        {
            SQMProcess_ProcessInfo_jQGridJSon m = new SQMProcess_ProcessInfo_jQGridJSon();
            string sSearchText = SearchText.Trim();

            m.Rows.Clear();
            int iRowCount = 0;

            string sSQL = "select T1.BasicInfoGUID, T1.TB_SQM_Process_TypeCID, T2.CNAME, T1.ProcessName, T1.OwnOrOut, T1.ExternalSupplierName from TB_SQM_Process T1, TB_SQM_Process_Type T2 where T1.BasicInfoGUID=@BasicInfoGUID and T1.TB_SQM_Process_TypeCID=T2.CID";
            if (sSearchText != "")
                sSQL += " and (T2.CNAME=@sSearchText or T1.ProcessName=@sSearchText or T1.OwnOrOut=@sSearchText or T1.ExternalSupplierName=@sSearchText)";

            SqlCommand cmd = new SqlCommand(sSQL, cn);
            if (sSearchText != "")
                cmd.Parameters.Add(new SqlParameter("@sSearchText", sSearchText));
            cmd.Parameters.Add(new SqlParameter("@BasicInfoGUID", BasicInfoGUID));
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                iRowCount++;
                m.Rows.Add(new SQMProcess_ProcessInfo(
                    dr["BasicInfoGUID"].ToString(),
                    int.Parse(dr["TB_SQM_Process_TypeCID"].ToString()),
                    dr["CNAME"].ToString(),
                    dr["ProcessName"].ToString(),
                    dr["OwnOrOut"].ToString() == "0" ? "是" : "否",
                    dr["ExternalSupplierName"].ToString()
                    ));
            }
            dr.Close();
            dr = null;
            cmd = null;

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(m);
        }

        public static string GetProcessCategoryList(SqlConnection cn, PortalUserProfile RunAsUser)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select CID, CNAME From TB_SQM_Process_Type Order By CID;");
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

        public static string CreateDataItem(SqlConnection cnPortal, SQMProcess_ProcessInfo DataItem)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            if (r != "")
            {
                return r;
            }
            else
            {
                string sErrMsg = "";
                StringBuilder sbSql = new StringBuilder();
                DataTable dt = new DataTable();
                if (DataItem.CName == "Other")
                {
                    if (string.IsNullOrEmpty(DataItem.CNameInput))
                    {
                        return "Must provide CNameInput.";
                    }
                    sbSql.Append("select * from TB_SQM_Process_Type where CNAME=@CNAME");
                    using (SqlCommand cmd = new SqlCommand(sbSql.ToString(), cnPortal))
                    {
                        cmd.Parameters.AddWithValue("@CNAME", DataItem.CNameInput);
                        SqlDataReader dr = cmd.ExecuteReader();
                        dt.Load(dr);
                    }

                    if (dt.Rows.Count > 0)
                    {
                        sErrMsg = "this CNAME data has exist!";
                        return sErrMsg;
                    }

                    SqlTransaction tran = cnPortal.BeginTransaction();
                    sbSql.Clear();
                    sbSql.Append("Insert into TB_SQM_Process_Type (CNAME) values (@CNAME)");

                    using (SqlCommand cmd = new SqlCommand(sbSql.ToString(), cnPortal, tran))
                    {
                        cmd.Parameters.AddWithValue("@CNAME", DataItem.CNameInput);
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
                    {
                        sbSql.Clear();
                        sbSql.Append("Insert Into TB_SQM_Process (BasicInfoGUID, TB_SQM_Process_TypeCID, ProcessName, OwnOrOut, ExternalSupplierName) ");
                        sbSql.Append("Values (@BasicInfoGUID, (select CID from TB_SQM_Process_Type where CNAME=@CNAME), @ProcessName, @OwnOrOut, @ExternalSupplierName)");
                        SQM_Basic_Helper.InsertPart(sbSql, "8");
                        using (SqlCommand cmd = new SqlCommand(sbSql.ToString(), cnPortal, tran))
                        {
                            cmd.Parameters.AddWithValue("@BasicInfoGUID", DataItem.BasicInfoGUID);
                            cmd.Parameters.AddWithValue("@CNAME", DataItem.CNameInput);
                            cmd.Parameters.AddWithValue("@ProcessName", DataItem.ProcessName);
                            cmd.Parameters.AddWithValue("@OwnOrOut", DataItem.OwnOrOut);
                            cmd.Parameters.AddWithValue("@ExternalSupplierName", DataItem.ExternalSupplierName);
                            try
                            {
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                sErrMsg = "Create fail.<br />Exception: " + ex.ToString();
                            }
                        }
                    }

                    if (sErrMsg == "")
                        tran.Commit();
                    else
                        tran.Rollback();

                    return sErrMsg;
                }
                else
                {
                    sbSql.Append("select * from TB_SQM_Process where BasicInfoGUID=@BasicInfoGUID and TB_SQM_Process_TypeCID=@TB_SQM_Process_TypeCID");
                    using (SqlCommand cmd = new SqlCommand(sbSql.ToString(), cnPortal))
                    {
                        cmd.Parameters.AddWithValue("@TB_SQM_Process_TypeCID", int.Parse(DataItem.ProcessType));
                        cmd.Parameters.AddWithValue("@BasicInfoGUID",DataItem.BasicInfoGUID);
                        SqlDataReader dr = cmd.ExecuteReader();
                        dt.Load(dr);
                    }

                    if (dt.Rows.Count > 0)
                    {
                        sErrMsg = "this CNAME data has exist!";
                        return sErrMsg;
                    }

                    SqlTransaction tran = cnPortal.BeginTransaction();
                    sbSql.Clear();
                    sbSql.Append("Insert into TB_SQM_Process (BasicInfoGUID, TB_SQM_Process_TypeCID, ProcessName, OwnOrOut, ExternalSupplierName) ");
                    sbSql.Append("Values (@BasicInfoGUID, @TB_SQM_Process_TypeCID, @ProcessName, @OwnOrOut, @ExternalSupplierName)");
                    SQM_Basic_Helper.InsertPart(sbSql, "8");
                    using (SqlCommand cmd = new SqlCommand(sbSql.ToString(), cnPortal, tran))
                    {
                        cmd.Parameters.AddWithValue("@BasicInfoGUID", DataItem.BasicInfoGUID);
                        cmd.Parameters.AddWithValue("@TB_SQM_Process_TypeCID", int.Parse(DataItem.ProcessType));
                        cmd.Parameters.AddWithValue("@ProcessName", DataItem.ProcessName);
                        cmd.Parameters.AddWithValue("@OwnOrOut", DataItem.OwnOrOut);
                        cmd.Parameters.AddWithValue("@ExternalSupplierName", DataItem.ExternalSupplierName);

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

        }

        private static void UnescapeDataFromWeb(SQMProcess_ProcessInfo DataItem)
        {
            DataItem.BasicInfoGUID = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.BasicInfoGUID);
            DataItem.VendorCode = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.VendorCode);
            DataItem.CName = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CName);
            DataItem.CNameInput = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CNameInput);
            DataItem.ProcessName = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ProcessName);
            DataItem.OwnOrOut = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.OwnOrOut);
            DataItem.ExternalSupplierName = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ExternalSupplierName);
        }

        private static string DataCheck(SQMProcess_ProcessInfo DataItem)
        {
            string r = "";
            List<string> e = new List<string>();
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.BasicInfoGUID))
                e.Add("BasicInfoGUID Is Null Or Empty");

            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.VendorCode))
                e.Add("VendorCode Is Null Or Empty");

            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.CName))
                e.Add("Must provide CName.");

            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.ProcessName))
                e.Add("Must provide ProcessName.");

            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.OwnOrOut))
                e.Add("Must provide OwnOrOut.");

            ////if (SQMStringHelper.DataIsNullOrEmpty(DataItem.ExternalSupplierName))
            ////    e.Add("Must provide ExternalSupplierName.");

            for (int iCnt = 0; iCnt < e.Count; ++iCnt)
            {
                if (iCnt > 0)
                    r += "<br />";

                r += e[iCnt];
            }

            return r;
        }

        public static string DeleteDataItem(SqlConnection cnPortal, SQMProcess_ProcessInfo DataItem)
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
                    //r = DeleteDataItemSub(cmd, "delete TB_SQM_Process where TB_SQM_Process_TypeCID=@TB_SQM_Process_TypeCID", "@TB_SQM_Process_TypeCID", DataItem.ProcessType);
                    string SQL = "delete from TB_SQM_Process where BasicInfoGUID=@BasicInfoGUID and TB_SQM_Process_TypeCID=@TB_SQM_Process_TypeCID";
                    cmd.CommandText = SQL;
                    cmd.Parameters.AddWithValue("@BasicInfoGUID", DataItem.BasicInfoGUID);
                    cmd.Parameters.AddWithValue("@TB_SQM_Process_TypeCID", DataItem.ProcessType);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        r = "Delete fail.<br />Exception: " + e.ToString();
                    }

                }
                if (r != "") { tran.Rollback(); return r; }

                using (SqlCommand cmd = new SqlCommand("", cnPortal, tran))
                {
                    r = DeleteDataItemSub(cmd, "delete TB_SQM_Process_Type where CID=@CID", "@CID", DataItem.ProcessType);
                }
                if (r != "") { tran.Rollback(); return r; }

                //Commit
                try
                {
                    tran.Commit();
                }
                catch (Exception e)
                {
                    tran.Rollback(); r = "Delete fail.<br />Exception: " + e.ToString();
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

        public static string EditDataItem(SqlConnection cnPortal, SQMProcess_ProcessInfo DataItem, string LoginMemberGUID, string RunAsMemberGUID)
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

        private static string EditDataItemSub(SqlCommand cmd, SQMProcess_ProcessInfo DataItem)
        {
            string sErrMsg = "";
            string sSQL = "Update TB_SQM_Process set ProcessName=@ProcessName, OwnOrOut=@OwnOrOut, ExternalSupplierName=@ExternalSupplierName where BasicInfoGUID=@BasicInfoGUID and TB_SQM_Process_TypeCID=@TB_SQM_Process_TypeCID";

            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@BasicInfoGUID", DataItem.BasicInfoGUID);
            cmd.Parameters.AddWithValue("@TB_SQM_Process_TypeCID", int.Parse(DataItem.ProcessType));
            cmd.Parameters.AddWithValue("@ProcessName", DataItem.ProcessName);
            cmd.Parameters.AddWithValue("@OwnOrOut", DataItem.OwnOrOut);
            cmd.Parameters.AddWithValue("@ExternalSupplierName", DataItem.ExternalSupplierName);

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
