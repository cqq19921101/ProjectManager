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
using Lib_VMI_Domain.Model;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using Lib_SQM_Domain.Modal;

namespace Lib_SQM_Domain.Model
{
    public class SQMCertification_CertificationInfo
    {
        protected string _VendorCode = "";
        protected string _BasicInfoGUID = "";
        protected int _CertificationType = 0;
        protected string _CName = "";
        protected string _CNameInput = "";
        protected string _CertificationAuthority = "";
        protected string _CertificationNum = "";
        protected string _CertificationDate = "";
        protected string _ValidDate = "";

        public string VendorCode { get { return this._VendorCode; } set { this._VendorCode = value; } }
        public string BasicInfoGUID { get { return this._BasicInfoGUID; } set { this._BasicInfoGUID = value; } }
        public string CertificationType { get { return Convert.ToString(this._CertificationType); } set { this._CertificationType = int.Parse(value); } }
        public string CName { get { return this._CName; } set { this._CName = value; } }
        public string CNameInput { get { return this._CNameInput; } set { this._CNameInput = value; } }
        public string CertificationAuthority { get { return this._CertificationAuthority; } set { this._CertificationAuthority = value; } }
        public string CertificationNum { get { return this._CertificationNum; } set { this._CertificationNum = value; } }
        public string CertificationDate { get { return this._CertificationDate; } set { this._CertificationDate = value; } }
        public string ValidDate { get { return this._ValidDate; } set { this._ValidDate = value; } }
        public SQMCertification_CertificationInfo() { }
        public SQMCertification_CertificationInfo(string BasicInfoGUID, int CertificationType, string CName, string CertificationAuthority, string CertificationNum, string CertificationDate, string ValidDate)
        {
            this._BasicInfoGUID = BasicInfoGUID;
            this._CertificationType = CertificationType;
            this._CName = CName;
            this._CertificationAuthority = CertificationAuthority;
            this._CertificationNum = CertificationNum;
            this._CertificationDate = CertificationDate;
            this._ValidDate = ValidDate;
        }
    }

    public class SQMCertification_CertificationInfo_jQGridJSon
    {
        public List<SQMCertification_CertificationInfo> Rows = new List<SQMCertification_CertificationInfo>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }

    public static class SQMCertification_CertificationInfo_Helper
    {
        public static string GetDataToJQGridJson(SqlConnection cn, string SearchText, string BasicInfoGUID)
        {
            SQMCertification_CertificationInfo_jQGridJSon m = new SQMCertification_CertificationInfo_jQGridJSon();
            string sSearchText = SearchText.Trim();

            m.Rows.Clear();
            int iRowCount = 0;
            StringBuilder sSQL = new StringBuilder();
            sSQL.Append(@"
SELECT    T1.BasicInfoGUID
		, T1.TB_SQM_Certifications_TypeCID
		, T2.CNAME
		, T1.CertificationAuthority
		, T1.CertificationNum
		, T1.CertificationDate
		, T1.ValidDate 
FROM TB_SQM_Certifications T1, TB_SQM_Certifications_Type T2 
WHERE T1.BasicInfoGUID=@BasicInfoGUID
AND T1.TB_SQM_Certifications_TypeCID=T2.CID
");
            if (sSearchText != "")
                sSQL.Append(" AND (T2.CNAME=@sSearchText or T1.CertificationAuthority=@sSearchText or T1.CertificationNum=@sSearchText)");

            SqlCommand cmd = new SqlCommand(sSQL.ToString(), cn);
            if (sSearchText != "")
                cmd.Parameters.Add(new SqlParameter("@sSearchText", sSearchText));
            if (BasicInfoGUID != "")
                cmd.Parameters.Add(new SqlParameter("@BasicInfoGUID", BasicInfoGUID));
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                iRowCount++;
                m.Rows.Add(new SQMCertification_CertificationInfo(
                    dr["BasicInfoGUID"].ToString(),
                    int.Parse(dr["TB_SQM_Certifications_TypeCID"].ToString()),
                    dr["CNAME"].ToString(),
                    dr["CertificationAuthority"].ToString(),
                    dr["CertificationNum"].ToString(),
                    Convert.ToDateTime(dr["CertificationDate"].ToString()).ToString("yyyy/MM/dd"),
                    Convert.ToDateTime(dr["ValidDate"].ToString()).ToString("yyyy/MM/dd")
                    ));
            }

            dr.Close();
            dr = null;
            cmd = null;

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(m);
        }

        public static string GetCertificationCategoryList(SqlConnection cn, PortalUserProfile RunAsUser)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select CID, CNAME From TB_SQM_Certifications_Type Order By CID;");
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

        public static string CreateDataItem(SqlConnection cnPortal, SQMCertification_CertificationInfo DataItem)
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
                    sbSql.Append(@"
SELECT TOP 1 CID FROM TB_SQM_Certifications_Type 
WHERE CNAME=@CNAME
");
                    using (SqlCommand cmd = new SqlCommand(sbSql.ToString(), cnPortal))
                    {
                        cmd.Parameters.AddWithValue("@CNAME", DataItem.CNameInput);
                        SqlDataReader dr = cmd.ExecuteReader();
                        dt.Load(dr);
                    }

                    if (dt.Rows.Count > 0)
                    {
                        sErrMsg = "this CertificationNameInputed data has exist!";
                        return sErrMsg;
                    }

                    SqlTransaction tran = cnPortal.BeginTransaction();
                    sbSql.Clear();
                    sbSql.Append("Insert into TB_SQM_Certifications_Type (CNAME) values (@CNAME)");

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
                        sbSql.Append(@"
Insert Into TB_SQM_Certifications
(BasicInfoGUID, TB_SQM_Certifications_TypeCID, CertificationAuthority, CertificationNum, CertificationDate, ValidDate) 
Values
(@BasicInfoGUID, (select CID from TB_SQM_Certifications_Type where CNAME=@CNAME), @CertificationAuthority, @CertificationNum, @CertificationDate, @ValidDate)");
                        SQM_Basic_Helper.InsertPart(sbSql, "6");
                        using (SqlCommand cmd = new SqlCommand(sbSql.ToString(), cnPortal, tran))
                        {
                            cmd.Parameters.AddWithValue("@BasicInfoGUID", DataItem.BasicInfoGUID);
                            cmd.Parameters.AddWithValue("@CNAME", DataItem.CNameInput);
                            cmd.Parameters.AddWithValue("@CertificationAuthority", DataItem.CertificationAuthority);
                            cmd.Parameters.AddWithValue("@CertificationNum", DataItem.CertificationNum);
                            cmd.Parameters.AddWithValue("@CertificationDate", Convert.ToDateTime(DataItem.CertificationDate));
                            cmd.Parameters.AddWithValue("@ValidDate", Convert.ToDateTime(DataItem.ValidDate));
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
                    sbSql.Append(@"
SELECT TOP 1 [BasicInfoGUID]
FROM [TB_SQM_Certifications]
WHERE [BasicInfoGUID]=@BasicInfoGUID
AND TB_SQM_Certifications_TypeCID=@TB_SQM_Certifications_TypeCID
");
                    using (SqlCommand cmd = new SqlCommand(sbSql.ToString(), cnPortal))
                    {
                        cmd.Parameters.AddWithValue("@BasicInfoGUID", DataItem.BasicInfoGUID);
                        cmd.Parameters.AddWithValue("@TB_SQM_Certifications_TypeCID", DataItem.CertificationType);
                        SqlDataReader dr = cmd.ExecuteReader();
                        dt.Load(dr);
                    }

                    if (dt.Rows.Count > 0)
                    {
                        sErrMsg = "this Certification data has exist!";
                        return sErrMsg;
                    }

                    SqlTransaction tran = cnPortal.BeginTransaction();
                    sbSql.Clear();
                    sbSql.Append("Insert into TB_SQM_Certifications (BasicInfoGUID, TB_SQM_Certifications_TypeCID, CertificationAuthority, CertificationNum, CertificationDate, ValidDate) ");
                    sbSql.Append("Values (@BasicInfoGUID, @TB_SQM_Certifications_TypeCID, @CertificationAuthority, @CertificationNum, @CertificationDate, @ValidDate)");
                    SQM_Basic_Helper.InsertPart(sbSql, "6");
                    using (SqlCommand cmd = new SqlCommand(sbSql.ToString(), cnPortal, tran))
                    {
                        cmd.Parameters.AddWithValue("@BasicInfoGUID", DataItem.BasicInfoGUID);
                        cmd.Parameters.AddWithValue("@TB_SQM_Certifications_TypeCID", int.Parse(DataItem.CertificationType));
                        cmd.Parameters.AddWithValue("@CertificationAuthority", DataItem.CertificationAuthority);
                        cmd.Parameters.AddWithValue("@CertificationNum", DataItem.CertificationNum);
                        cmd.Parameters.AddWithValue("@CertificationDate", Convert.ToDateTime(DataItem.CertificationDate));
                        cmd.Parameters.AddWithValue("@ValidDate", Convert.ToDateTime(DataItem.ValidDate));

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

        private static void UnescapeDataFromWeb(SQMCertification_CertificationInfo DataItem)
        {
            DataItem.BasicInfoGUID = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.BasicInfoGUID);
            DataItem.VendorCode = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.VendorCode);
            DataItem.CName = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CName);
            DataItem.CNameInput = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CNameInput);
            DataItem.CertificationAuthority = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CertificationAuthority);
            DataItem.CertificationNum = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CertificationNum);
            DataItem.CertificationDate = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CertificationDate);
            DataItem.ValidDate = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ValidDate);
        }

        private static string DataCheck(SQMCertification_CertificationInfo DataItem)
        {
            string r = "";
            List<string> e = new List<string>();
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.BasicInfoGUID))
                e.Add("BasicInfoGUID Is Null Or Empty.");

            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.VendorCode))
                e.Add("VendorCode Is Null Or Empty.");

            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.CName))
                e.Add("Must provide CName.");

            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.CertificationAuthority))
                e.Add("Must provide CertificationAuthority.");

            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.CertificationNum))
                e.Add("Must provide CertificationNum.");

            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.CertificationDate))
                e.Add("Must Provide CertificationDate.");

            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.ValidDate))
                e.Add("Must Provide ValidDate.");

            for (int iCnt = 0; iCnt < e.Count; ++iCnt)
            {
                if (iCnt > 0)
                    r += "<br />";

                r += e[iCnt];
            }

            return r;
        }

        public static string DeleteDataItem(SqlConnection cnPortal, SQMCertification_CertificationInfo DataItem)
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
                
                //delete SQM File 
                using (SqlCommand cmd = new SqlCommand("", cnPortal, tran))
                {
                    string SQL = "delete from TB_SQM_Files where FGUID=(select CertificateImageFGUID from TB_SQM_Certifications where BasicInfoGUID=@BasicInfoGUID and TB_SQM_Certifications_TypeCID = @TB_SQM_Certifications_TypeCID)";
                    cmd.CommandText = SQL;
                    cmd.Parameters.AddWithValue("@BasicInfoGUID", DataItem.BasicInfoGUID);
                    cmd.Parameters.AddWithValue("@TB_SQM_Certifications_TypeCID", DataItem.CertificationType);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        r = "Delete fail.<br />Exception: " + e.ToString();
                    }
                }

                using (SqlCommand cmd = new SqlCommand("", cnPortal, tran))
                {
                    string SQL = "delete from TB_SQM_Certifications where BasicInfoGUID=@BasicInfoGUID and TB_SQM_Certifications_TypeCID=@TB_SQM_Certifications_TypeCID ";
                    cmd.CommandText = SQL;
                    cmd.Parameters.AddWithValue("@BasicInfoGUID", DataItem.BasicInfoGUID);
                    cmd.Parameters.AddWithValue("@TB_SQM_Certifications_TypeCID", DataItem.CertificationType);

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
                    r = DeleteDataItemSub(cmd, "delete TB_SQM_Certifications_Type where CID=@CID", "@CID", DataItem.CertificationType);
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

        public static string EditDataItem(SqlConnection cnPortal, SQMCertification_CertificationInfo DataItem, string LoginMemberGUID, string RunAsMemberGUID)
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

        private static string EditDataItemSub(SqlCommand cmd, SQMCertification_CertificationInfo DataItem)
        {
            string sErrMsg = "";
            string sSQL = "Update TB_SQM_Certifications set CertificationAuthority=@CertificationAuthority, CertificationNum=@CertificationNum, CertificationDate=@CertificationDate, ValidDate=@ValidDate where BasicInfoGUID=@BasicInfoGUID and TB_SQM_Certifications_TypeCID=@TB_SQM_Certifications_TypeCID";

            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@BasicInfoGUID", DataItem.BasicInfoGUID);
            cmd.Parameters.AddWithValue("@TB_SQM_Certifications_TypeCID", int.Parse(DataItem.CertificationType));
            cmd.Parameters.AddWithValue("@CertificationAuthority", DataItem.CertificationAuthority);
            cmd.Parameters.AddWithValue("@CertificationNum", DataItem.CertificationNum);
            cmd.Parameters.AddWithValue("@CertificationDate", DataItem.CertificationDate);
            cmd.Parameters.AddWithValue("@ValidDate", DataItem.ValidDate);

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
            //StringBuilder sb = new StringBuilder();
            //sb.Append("Select TOP 1 VendorCode From TB_SQM_Member_Vendor_Map");
            //sb.Append(" WHERE MemberGUID=@MemberGUID");
            //String vendorCode = "";
            //DataTable dt = new DataTable();
            //using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            //{
            //    cmd.Parameters.Add(new SqlParameter("@MemberGUID", RunAsUser.MemberGUID));
            //    var vendorCodeScale = cmd.ExecuteScalar();
            //    vendorCode = (vendorCodeScale == null ? "" : vendorCodeScale.ToString());
            //}
            return RunAsUser.MemberGUID;
        }

        public static string UploadIntroFile(SqlConnection cn, PortalUserProfile RunAsUser, FileAttachmentInfo FA, string VendorCode, string sLocalPath, string sLocalUploadPath, HttpServerUtilityBase Server, string RequestApplicationPath, String CertificationType, string BasicInfoGUID)
        {
            String r = "";

            JArray ja = JArray.Parse(FA.SPEC);
            dynamic jo_item = (JObject)ja[0];

            //00.UploadFileToDB
            SqlTransaction tran = cn.BeginTransaction();
            String file = sLocalUploadPath + FA.SUBFOLDER + "/" + jo_item.FileName;
            String FGUID = SharedLibs.SqlFileStreamHelper.InsertToTableSQM(cn, tran, RunAsUser.MemberGUID, file);

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
                DELETE FROM TB_SQM_Files
                WHERE FGUID = ( SELECT CertificateImageFGUID
                FROM TB_SQM_Certifications
                WHERE BasicInfoGUID = @BasicInfoGUID and TB_SQM_Certifications_TypeCID=@TB_SQM_Certifications_TypeCID )
                ");
                String sql = Regex.Replace(sb.ToString(), @"\s+", " ");

                using (SqlCommand cmd = new SqlCommand(sql, cn, tran))
                {
                    cmd.Parameters.Add(new SqlParameter("@BasicInfoGUID", BasicInfoGUID));
                    cmd.Parameters.Add(new SqlParameter("@TB_SQM_Certifications_TypeCID", CertificationType));
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
                    UPDATE dbo.TB_SQM_Certifications
                        SET CertificateImageFGUID = @CertificateImageFGUID
                    WHERE BasicInfoGUID = @BasicInfoGUID and TB_SQM_Certifications_TypeCID=@TB_SQM_Certifications_TypeCID
                    ");
                String sql = Regex.Replace(sb.ToString(), @"\s+", " ");
                using (SqlCommand cmd = new SqlCommand(sql, cn, tran))
                {
                    cmd.Parameters.Add(new SqlParameter("@CertificateImageFGUID", FGUID));
                    cmd.Parameters.Add(new SqlParameter("@BasicInfoGUID", BasicInfoGUID));
                    cmd.Parameters.Add(new SqlParameter("@TB_SQM_Certifications_TypeCID", CertificationType));
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
    }
}
