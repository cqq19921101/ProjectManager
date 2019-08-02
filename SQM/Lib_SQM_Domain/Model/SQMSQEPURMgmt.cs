using Lib_Portal_Domain.Model;
using Lib_Portal_Domain.SharedLibs;
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
    

    public class SQMSQEPURMgmt
    {
        protected string _VendorCode;
        protected string _ERP_VNAME;
        protected string _PlantCode;
        protected string _PLANT_NAME;
        protected string _SourcerGUID;
        protected string _SourcerName;
        protected string _SQEGUID;
        protected string _SQENAME;
        protected string _RDGUID;
        protected string _RDNAME;
        protected string _RDSGUID;
        protected string _RDSNAME;



        public string VendorCode { get { return this._VendorCode; } set { this._VendorCode = value; } }
        public string ERP_VNAME { get { return this._ERP_VNAME; } set { this._ERP_VNAME = value; } }
        public string PlantCode { get { return this._PlantCode; } set { this._PlantCode = value; } }
        public string PLANT_NAME { get { return this._PLANT_NAME; } set { this._PLANT_NAME = value; } }
        public string SourcerGUID { get { return this._SourcerGUID; } set { this._SourcerGUID = value; } }
        public string SourcerName { get { return this._SourcerName; } set { this._SourcerName = value; } }
        public string SQEGUID { get { return this._SQEGUID; } set { this._SQEGUID = value; } }
        public string SQENAME { get { return this._SQENAME; } set { this._SQENAME = value; } }
        public string RDGUID { get { return this._RDGUID; } set { this._RDGUID = value; } }
        public string RDNAME { get { return this._RDNAME; } set { this._RDNAME = value; } }
        public string RDSGUID { get { return this._RDSGUID; } set { this._RDSGUID = value; } }
        public string RDSNAME { get { return this._RDSNAME; } set { this._RDSNAME = value; } }

        public SQMSQEPURMgmt() { }

        public SQMSQEPURMgmt(string VendorCode, string ERP_VNAME, string PlantCode, string PLANT_NAME, string SourcerGUID, string SourcerName,string SQEGUID,string SQENAME, string RDGUID, string RDNAME, string RDSGUID, string RDSNAME)
        {
            this._VendorCode = VendorCode;
            this._ERP_VNAME = ERP_VNAME;
            this._PlantCode = PlantCode;
            this._PLANT_NAME = PLANT_NAME;
            this._SourcerGUID = SourcerGUID;
            this._SourcerName = SourcerName;
            this._SQEGUID = SQEGUID;
            this._SQENAME = SQENAME;
            this._RDGUID = RDGUID;
            this._RDNAME = RDNAME;
            this._RDSGUID = RDSGUID;
            this._RDSNAME = RDSNAME;
        }
    }
    public class SQMSQEPURMgmt_jQGridJSon
    {
        public List<SQMSQEPURMgmt> Rows = new List<SQMSQEPURMgmt>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }
    public static class SQMSQEPURMgmt_Helper
    {
        #region Create/Edit data check
        private static void UnescapeDataFromWeb(SQMSQEPURMgmt DataItem)
        {
            DataItem.ERP_VNAME = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ERP_VNAME);
            DataItem.PLANT_NAME = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.PLANT_NAME);
            DataItem.SourcerName = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.SourcerName);
            DataItem.SQENAME = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.SQENAME);
        }
        private static string DataCheck(SQMSQEPURMgmt DataItem)
        {
            string r = "";
            List<string> e = new List<string>();

            if (StringHelper.DataIsNullOrEmpty(DataItem.PlantCode))
                e.Add("Must provide PlantCode.");

            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }
        #endregion

        #region Query MemberGUID
        public static string Query_MemberGUIDInfo(SqlConnection cn, string NAME)
        {
            bool bIsWithCondition = false;

            #region DecodeParameter
            //string sBuyer = StringHelper.EmptyOrUnescapedStringViaUrlDecode(Buyer);
            #endregion
            string SMG = StringHelper.EmptyOrUnescapedStringViaUrlDecode(NAME);
            //retRows.Rows.Clear();

            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.Append(@"
   SELECT PORTAL_Members.MemberGUID,
                   NameInChinese
            FROM PORTAL_Members,
                 ( 
                SELECT DISTINCT MEMBERGUID FROM PORTAL_MEMBERROLES
  inner join PORTAL_Roles on PORTAL_Roles.RoleGUID=PORTAL_MEMBERROLES.RoleGUID
    WHERE RoleName LIKE 'SQM%' ) TB_EB_EMPL_FUNC
            WHERE PORTAL_Members.MemberGUID = convert(nvarchar(255), TB_EB_EMPL_FUNC.MEMBERGUID)
            ");
            if(!SMG.Equals(string.Empty))
            {
                sb.Append(" and  NameInChinese like '%' + @NameInChinese + '%' ");
            }

            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {

                cmd.Parameters.AddWithValue("@NameInChinese", NAME.Trim());

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }
        #endregion

        #region Query SBU Vendor Code
        public static string Query_SBUVendorCodeInfo(SqlConnection cn, PortalUserProfile RunAsUser, string ERP_VND)
        {
            bool bIsWithCondition = false;
            #region DecodeParameter
            //string sSBUVendorCode = StringHelper.EmptyOrUnescapedStringViaUrlDecode(SBUVendorCode);
            #endregion
            string SVEN = StringHelper.EmptyOrUnescapedStringViaUrlDecode(ERP_VND);
            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.Append(@"
SELECT DISTINCT ERP_VND,ERP_VNAME 
                        FROM TB_VMI_VENDOR_DETAIL
                        ");
            if (!SVEN.Equals(string.Empty))
            {
                sb.Append(" where  ERP_VND like '%' + @ERP_VND + '%' ");
            }
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.AddWithValue("@ERP_VND", ERP_VND.Trim());

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }
        #endregion

        #region Query Plant Code
        public static string Query_PlantCodeInfo(SqlConnection cn, string Plant)
        {
            bool bIsWithCondition = false;
            #region DecodeParameter
            string sPlant = StringHelper.EmptyOrUnescapedStringViaUrlDecode(Plant);
            #endregion
            string splant = StringHelper.EmptyOrUnescapedStringViaUrlDecode(Plant);

            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.Append(@"
            SELECT DISTINCT PLANT,PLANT_NAME 
            FROM TB_VMI_PLANT 
");
            if (!splant.Equals(string.Empty))
            {
                sb.Append("  where  Plant like '%' + @Plant + '%' ");
            }
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {

                cmd.Parameters.AddWithValue("@Plant", Plant.Trim());

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }
        #endregion

        public static string DeleteDataItem(SqlConnection cnPortal, SQMSQEPURMgmt DataItem)
        {
            return DeleteDataItem(cnPortal, DataItem, "", "");
        }

        public static string DeleteDataItem(SqlConnection cnPortal, SQMSQEPURMgmt DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            string r = "";
            if (StringHelper.DataIsNullOrEmpty(DataItem.VendorCode))
                return "Must provide VendorCode.";
            //if (StringHelper.DataIsNullOrEmpty(DataItem.MemberGUID))
            //    return "Must provide MemberGUID.";
            else
            {
                using (SqlCommand cmd = new SqlCommand("", cnPortal)) { r = DeleteDataItemSub(cmd, DataItem); }
                if (r != "") { return r; }
                return r;
            }
        }

        private static string DeleteDataItemSub(SqlCommand cmd, SQMSQEPURMgmt DataItem)
        {
            string sErrMsg = "";

            string sSQL = "Delete [TB_SQM_Vendor_Related] Where VendorCode = @VendorCode";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@VendorCode", StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.VendorCode));
            //cmd.Parameters.AddWithValue("@VendorCode", StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.VendorCode));
            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }

        public static string GetDataToJQGridJson(SqlConnection cn, string SearchText)
        {
            string sSearchText = SearchText.Trim();
            string sWhereClause = "";
            if (sSearchText != "")
                sWhereClause += " PlantCode   like '%'+ @SearchText+'%'   ";
            if (sWhereClause.Length != 0)
                sWhereClause = "  AND " + sWhereClause.Substring(0);
            SQMSQEPURMgmt_jQGridJSon m = new SQMSQEPURMgmt_jQGridJSon();
            m.Rows.Clear();
            int iRowCount = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
        SELECT SVR.VendorCode,
			        TB_VMI_VENDOR_DETAIL.ERP_VNAME,
                   SVR.PlantCode,
                   TB_VMI_PLANT.PLANT_NAME,
				   SVR.SourcerGUID,
				   U1.NameInChinese as SourcerName,
				   SVR.SQEGUID,
				   U2.NameInChinese as SQENAME,
				   SVR.RDGUID,
				   U3.NameInChinese as  RDNAME,
				   SVR.RDSGUID,
				   U4.NameInChinese as  RDSNAME
            FROM 
			TB_SQM_Vendor_Related SVR
				LEFT OUTER JOIN PORTAL_Members U1 ON U1.MemberGUID = Convert(nvarchar(50),SVR.SourcerGUID)
				LEFT OUTER JOIN PORTAL_Members U2 ON U2.MemberGUID = Convert(nvarchar(50),SVR.SQEGUID)
				LEFT OUTER JOIN PORTAL_Members U3 ON U3.MemberGUID = Convert(nvarchar(50),SVR.RDGUID)
				LEFT OUTER JOIN PORTAL_Members U4 ON U4.MemberGUID = Convert(nvarchar(50),SVR.RDSGUID),
					TB_VMI_VENDOR_DETAIL,
					TB_VMI_PLANT
            WHERE SVR.VendorCode = [TB_VMI_VENDOR_DETAIL].ERP_VND
                  and  PlantCode = [TB_VMI_PLANT].PLANT
");

            if (sSearchText != "")
            {
                sb.Append(sWhereClause);
            }
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                if (sSearchText != "")
                    cmd.Parameters.Add(new SqlParameter("@SearchText", sSearchText));
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    iRowCount++;
                    m.Rows.Add(new SQMSQEPURMgmt(
                        dr["VendorCode"].ToString(),
                        dr["ERP_VNAME"].ToString(),
                         dr["PlantCode"].ToString(),
                         dr["PLANT_NAME"].ToString(),
                          dr["SourcerGUID"].ToString(),
                          dr["SourcerName"].ToString(),
                          dr["SQEGUID"].ToString(),
                          dr["SQENAME"].ToString(),
                          dr["RDGUID"].ToString(),
                          dr["RDNAME"].ToString(),
                          dr["RDSGUID"].ToString(),
                          dr["RDSNAME"].ToString()
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
            return GetDataToJQGridJson(cn, "");
        }

        public static string CreateDataItem(SqlConnection cnPortal, SQMSQEPURMgmt DataItem,string LoginUserGUID)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            int c = 0;
            if (r != "")
            { return r; }
            string sSQL = "Select Count(*) From TB_SQM_Vendor_Related  Where PlantCode = @PlantCode And VendorCode = @VendorCode;";
            using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal))
            {
                cmd.Parameters.Add(new SqlParameter("@PlantCode", DataItem.PlantCode));
                cmd.Parameters.Add(new SqlParameter("@VendorCode", DataItem.VendorCode));
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                    c = (int)dr[0];
                dr.Close();
                dr = null;
            }
            if (c > 0) {
                r = "請檢查輸入的數據是否重複！";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"
            Insert into [TB_SQM_Vendor_Related] (PlantCode,VendorCode,SourcerGUID,SQEGUID,RDGUID,RDSGUID,UpdateDatetime,UpdateUser)
            values(@PlantCode,@VendorCode,@SourcerGUID,@SQEGUID,@RDGUID,@RDSGUID,getDate(),@LoginUserGUID)
            ");


                SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal);
                cmd.Parameters.AddWithValue("@PlantCode", StringHelper.NullOrEmptyStringIsDBNull(DataItem.PlantCode));
                cmd.Parameters.AddWithValue("@VendorCode", StringHelper.NullOrEmptyStringIsDBNull(DataItem.VendorCode));
                cmd.Parameters.AddWithValue("@SourcerGUID", StringHelper.NullOrEmptyStringIsDBNull(DataItem.SourcerGUID));
                cmd.Parameters.AddWithValue("@SQEGUID", StringHelper.NullOrEmptyStringIsDBNull(DataItem.SQEGUID));
                cmd.Parameters.AddWithValue("@RDGUID", StringHelper.NullOrEmptyStringIsDBNull(DataItem.RDGUID));
                cmd.Parameters.AddWithValue("@RDSGUID", StringHelper.NullOrEmptyStringIsDBNull(DataItem.RDSGUID));
                cmd.Parameters.AddWithValue("@LoginUserGUID", StringHelper.NullOrEmptyStringIsDBNull(LoginUserGUID));

                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;
            }
            return r;

        }

        #region Edit data item
        public static string EditDataItem(SqlConnection cnPortal, SQMSQEPURMgmt DataItem)
        {
            return EditDataItem(cnPortal, DataItem, "", "");
        }

        public static string EditDataItem(SqlConnection cnPortal, SQMSQEPURMgmt DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            if (r != "")
            { return r; }
            else
            {
                using (SqlCommand cmd = new SqlCommand("", cnPortal)) { r = EditDataItem(cmd, DataItem); }
                if (r != "") { return r; }
                return r;
            }
        }

        private static string EditDataItem(SqlCommand cmd, SQMSQEPURMgmt DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
            UPdate [TB_SQM_Vendor_Related] SET SourcerGUID = @SourcerGUID , SQEGUID = @SQEGUID,RDGUID =@RDGUID,RDSGUID = @RDSGUID
WHERE PlantCode = @PlantCode AND VendorCode = @VendorCode
");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@SourcerGUID", StringHelper.NullOrEmptyStringIsDBNull(DataItem.SourcerGUID));
            cmd.Parameters.AddWithValue("@SQEGUID", StringHelper.NullOrEmptyStringIsDBNull(DataItem.SQEGUID));
            cmd.Parameters.AddWithValue("@RDGUID", StringHelper.NullOrEmptyStringIsDBNull(DataItem.RDGUID));
            cmd.Parameters.AddWithValue("@RDSGUID", StringHelper.NullOrEmptyStringIsDBNull(DataItem.RDSGUID));
            cmd.Parameters.AddWithValue("@PlantCode", StringHelper.NullOrEmptyStringIsDBNull(DataItem.PlantCode));
            cmd.Parameters.AddWithValue("@VendorCode", StringHelper.NullOrEmptyStringIsDBNull(DataItem.VendorCode));
            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion


    }

}
