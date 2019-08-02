using Lib_Portal_Domain.Model;
using Lib_SQM_Domain.Modal;
using Lib_SQM_Domain.SharedLibs;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Lib_SQM_Domain.Model
{
    #region CPK
    #region CPK Date Definitions
    public class SQMCPK
    {
        protected string _MemberGUID;
        protected string _reportID;
        protected string _plantCode;
        protected string _Supplier;
        protected string _Description;
        protected string _QCManager;
        protected string _ToolNumber;
        protected string _Inches;
        protected string _Revision;
        protected string _Reviewer;
        protected string _Prepared;

        public string MemberGUID { get { return this._MemberGUID; } set { this._MemberGUID = value; } }
        public string reportID { get { return this._reportID; } set { this._reportID = value; } }
        public string plantCode { get { return this._plantCode; } set { this._plantCode = value; } }
        public string Supplier { get { return this._Supplier; } set { this._Supplier = value; } }
        public string Description { get { return this._Description; } set { this._Description = value; } }
        public string QCManager { get { return this._QCManager; } set { this._QCManager = value; } }
        public string ToolNumber { get { return this._ToolNumber; } set { this._ToolNumber = value; } }
        public string Inches { get { return this._Inches; } set { this._Inches = value; } }
        public string Revision { get { return this._Revision; } set { this._Revision = value; } }
        public string Reviewer { get { return this._Reviewer; } set { this._Reviewer = value; } }
        public string Prepared { get { return this._Prepared; } set { this._Prepared = value; } }

        public SQMCPK() { }

        public SQMCPK(
            string MemberGUID,
            string reportID,
            string plantCode,
            string Supplier,
            string Description,
            string QCManager,
            string ToolNumber,
            string Inches,
            string Revision,
            string Reviewer,
            string Prepared

            )
        {

            this._MemberGUID = MemberGUID;
            this._reportID = reportID;
            this._plantCode = plantCode;
            this._Supplier = Supplier;
            this._Description = Description;
            this._QCManager = QCManager;
            this._ToolNumber = ToolNumber;
            this._Inches = Inches;
            this._Revision = Revision;
            this._Reviewer = Reviewer;
            this._Prepared = Prepared;
        }
    }

    public class SQMCPK_jQGridJSon
    {
        public List<SQMCPK> Rows = new List<SQMCPK>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }
    #endregion

    #region CPK Helper
    public static class SQMCPK_Helper
    {
        #region Search CPK
        public static string GetDataToJQGridJson(SqlConnection cn, String MemberGUID)
        {
            return GetDataToJQGridJson(cn, "", MemberGUID);
        }

        public static String GetDataToJQGridJson(SqlConnection cn, String SearchText, String MemberGUID)
        {
            string sSearchText = SearchText.Trim();
            string sWhereClause = "";
            if (sSearchText != "")
                sWhereClause += "   [plantCode] like '%'+ @SearchText+'%'   ";
            if (sWhereClause.Length != 0)
                sWhereClause = "  AND " + sWhereClause.Substring(0);

            StringBuilder sb = new StringBuilder();
            sb.Append(@"
SELECT [MemberGUID],
       [reportID],
       [plantCode],
       [Supplier],
       [Description],
       [ToolNumber],
       [Inches],
       [Revision],
       [Reviewer],
       [Prepared],
       [ReMark],
       TB_VMI_VENDOR_DETAIL.ERP_VNAME
FROM [TB_SQM_CPK_Report]
     LEFT JOIN TB_VMI_VENDOR_DETAIL ON ERP_VND = ( 
                                                   SELECT VendorCode
                                                   FROM TB_SQM_Member_Vendor_Map
                                                   WHERE MemberGUID = @MemberGUID )
WHERE [MemberGUID] = @MemberGUID
order by reportID desc
            ");
            DataTable dt = new DataTable();
            sb.Append(sWhereClause + ";");
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                if (sSearchText != "")
                    cmd.Parameters.Add(new SqlParameter("@SearchText", sSearchText));
                cmd.Parameters.Add(new SqlParameter("@MemberGUID", MemberGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }
        #endregion

        #region Create/Edit data check
        private static void UnescapeDataFromWeb(SQMCPK DataItem)
        {
            DataItem.plantCode = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.plantCode);
            DataItem.Supplier = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Supplier);
            DataItem.Description = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Description);
            DataItem.ToolNumber = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ToolNumber);
            DataItem.Inches = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Inches);
        }

        private static string DataCheck(SQMCPK DataItem)
        {
            string r = "";
            List<string> e = new List<string>();
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.plantCode))
                e.Add("Must provide Part Number.");
          
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.ToolNumber))
                e.Add("Must provide Part ToolNumber.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.Inches))
                e.Add("Must provide Part Inches.");

            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }
        #endregion

        #region Create data item
        public static string CreateDataItem(SqlConnection cnPortal, SQMCPK DataItem, String MemberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            string reportID = Assignment(cnPortal);
            if (r != "")
            {
                return r;
            }
            else
            {
                //string c = CreateDataExistOrNot(cnPortal, DataItem, MemberGUID);
                //if (c != "")
                //{
                //    return c;
                //}
                //else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(@" 
DECLARE @Supplier nvarchar(50) =(
SELECT ERP_VNAME 
FROM TB_VMI_VENDOR_DETAIL 
WHERE ERP_VND=(
SELECT VendorCode FROM TB_SQM_Member_Vendor_Map WHERE MemberGUID=@MemberGUID)
)
 INSERT INTO [dbo].[TB_SQM_CPK_Report](
  [MemberGUID]
      ,[reportID]
      ,[plantCode]
      ,[Supplier]
      ,[Description]
      ,[ToolNumber]
      ,[Inches]
  )
  VALUES(
  @MemberGUID
      ,@reportID
      ,@plantCode
      ,@Supplier
      ,@Description
      ,@ToolNumber
      ,@Inches
  )
");
                    //SQM_Basic_Helper.InsertPart(sb, "3");
                    SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal);
                    cmd.Parameters.AddWithValue("@MemberGUID", MemberGUID);
                    cmd.Parameters.AddWithValue("@reportID", reportID);
                    cmd.Parameters.AddWithValue("@plantCode", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.plantCode));
                    cmd.Parameters.AddWithValue("@Description", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Description));
                    cmd.Parameters.AddWithValue("@ToolNumber", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ToolNumber));
                    cmd.Parameters.AddWithValue("@Inches", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Inches));

                    string sErrMsg = "";
                    try { cmd.ExecuteNonQuery(); }
                    catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                    cmd = null;

                    return sErrMsg;
                }
            }
        }
        //生成流水號
        public static string Assignment(SqlConnection cnPortal)
        {
            string LastNumStr = getLastNumStr(cnPortal); ;
            string number0 = "";
            DateTime date = System.DateTime.Now;
            string month = date.Month.ToString();
            string day = date.Day.ToString();
            string year = date.Year.ToString();
            if (month.Length < 2)
                month = '0' + month;
            if (day.Length < 2)
                day = '0' + day;
            string ymd = year+  month + day;
            if (LastNumStr.Length < 8 || LastNumStr.Substring(0, 8) != ymd)
            {
                return ymd + "0000001";
            }
            else
            {
                int clientnumber = Convert.ToInt32(LastNumStr.Substring(8, LastNumStr.Length - 8)) + 1;
                if (clientnumber.ToString().Length > LastNumStr.Length - 8)
                {
                    return ymd + clientnumber.ToString();
                }
                else
                {
                    for (int i = 0; i < LastNumStr.Length - 8 - clientnumber.ToString().Length; i++)
                    {
                        number0 += "0";
                    }
                    return ymd + number0 + clientnumber.ToString();
                }
            }
        }

        private static string getLastNumStr(SqlConnection cn)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select Max(reportID) from TB_SQM_CPK_Report");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
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

        public static string CreateDataExistOrNot(SqlConnection cnPortal, SQMCPK DataItem, String MemberGUID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"

SELECT TOP 1 [MemberGUID]
FROM [TB_SQM_CPK_Report]
WHERE [MemberGUID]=@MemberGUID AND [reportID]=@reportID OR [plantCode]=@plantCode

    ");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal))
            {
                cmd.Parameters.Add(new SqlParameter("@MemberGUID", MemberGUID));
                cmd.Parameters.Add(new SqlParameter("@reportID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.reportID)));
                cmd.Parameters.Add(new SqlParameter("@plantCode", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.plantCode)));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
                if (dt.Rows.Count > 0)
                {
                    return "report is exist";
                }
                else
                {
                    return "";
                }

            }
        }
        #endregion

        #region Edit data item

        public static string EditDataItem(SqlConnection cnPortal, SQMCPK DataItem)
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

        private static string EditDataItemSub(SqlCommand cmd, SQMCPK DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"

UPDATE [dbo].[TB_SQM_CPK_Report]
SET  [plantCode]=@plantCode
	,[Description]=@Description
	,[ToolNumber]=@ToolNumber
	,[Inches]=@Inches
WHERE [MemberGUID]=@MemberGUID AND [reportID]=@reportID

");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@MemberGUID", DataItem.MemberGUID);
            cmd.Parameters.AddWithValue("@reportID", DataItem.reportID);
            cmd.Parameters.AddWithValue("@plantCode", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.plantCode));
            cmd.Parameters.AddWithValue("@Description", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Description));
            cmd.Parameters.AddWithValue("@ToolNumber", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ToolNumber));
            cmd.Parameters.AddWithValue("@Inches", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Inches));

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion

        #region Delete data item

        public static string DeleteDataItem(SqlConnection cnPortal, SQMCPK DataItem, String MemberGUID)
        {

            string r = "";
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.reportID))
                return "Must provide reportID.";
            else
            {
                using (SqlCommand cmd = new SqlCommand("", cnPortal)) { r = DeleteDataItemSub(cmd, DataItem, MemberGUID); }
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

        private static string DeleteDataItemSub(SqlCommand cmd, SQMCPK DataItem, String MemberGUID)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
DELETE [dbo].[TB_SQM_CPK_Report]
WHERE [MemberGUID]=@MemberGUID AND [reportID]=@reportID;
DELETE [dbo].[TB_SQM_CPK_Report_sub]
WHERE [plantCode]=@plantCode;
DELETE [TB_SQM_CPK_Report_CTQ] WHERE [reportID]=@reportID;
");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@MemberGUID", MemberGUID);
            cmd.Parameters.AddWithValue("@plantCode", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.plantCode));
            cmd.Parameters.AddWithValue("@reportID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.reportID));

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion

        public static String GetLitNoList( SqlConnection cn, String MemberGUID)
        {
            StringBuilder sb = new StringBuilder();
            string Plant = GetPlant(cn, MemberGUID);
            sb.Append(@"
SELECT DISTINCT       [LitNo]
FROM [TB_SQM_SFC_MAP]
WHERE [VoderCode] in ( 
                     SELECT VendorCode
                         FROM TB_SQM_Member_Vendor_Map
                         WHERE PlantCode = @PlantCode);
");

            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@PlantCode", Plant));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
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
        public static String GetLitNoDataList(string plantCode,SqlConnection cn)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
SELECT [spc_id],
       [LitNo],
       [spc_item],
       [spc_desc],
       [check_6u],
       [check_6d],
       [usl],
       [lsl],
       [sl],
       [ucl],
       [lcl],
       [update_time],
       [cpk],
       [check_9m],
       [sample],
       [datum]
FROM [TB_SQM_SFC_DATA]
WHERE [LitNo] = @LitNo;
");

            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@LitNo", SQMStringHelper.NullOrEmptyStringIsDBNull(plantCode)));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }
        public static String GetLitNoData(String MainID,string plantCode, SqlConnection cn)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
SELECT [spc_id],
       [LitNo],
       [spc_item],
       [spc_desc],
       [check_6u],
       [check_6d],
       [usl],
       [lsl],
       [sl],
       [ucl],
       [lcl],
       [update_time],
       [cpk],
       [check_9m],
       [sample],
       [datum]
FROM [TB_SQM_SFC_DATA]
WHERE [LitNo] = @LitNo
  AND [spc_item] = @spc_item;
");

            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@LitNo",SQMStringHelper.NullOrEmptyStringIsDBNull(plantCode)));
                cmd.Parameters.Add(new SqlParameter("@spc_item", SQMStringHelper.NullOrEmptyStringIsDBNull(MainID)));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }

        public static string UpdateReMark(SqlConnection sqlConnection, string Report, string Remark)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
UPDATE [dbo].[TB_SQM_CPK_Report]
SET ReMark=@ReMark
     
WHERE reportID=@reportID
");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@reportID", Report);
                cmd.Parameters.AddWithValue("@ReMark", SQMStringHelper.NullOrEmptyStringIsDBNull(Remark));
               try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }
            }
              

            return sErrMsg;
        }
    }
    #endregion

    #endregion
    #region CPKSub
    #region CPKSub Date Definitions
    public class SQMCPKSub
    {
        protected string _SID;
        protected string _reportID;
        protected string _plantCode;
        protected string _spc_id;
        protected string _Designator;
        protected string _Unit;
        protected string _Nominal;
        protected string _minNominal;
        protected string _maxNominal;
        protected string _UpperControlLimit;
        protected string _LowerControlLimit;
        protected string _CTQNum;
        protected string _centerline;
        protected string _CPKManager;
        protected string _section;
        protected string _Sixpointrise;
        protected string _Dropdown;
        protected string _NineNearCenter;

        public string SID { get { return this._SID; } set { this._SID = value; } }
        public string reportID { get { return this._reportID; } set { this._reportID = value; } }
        public string plantCode { get { return this._plantCode; } set { this._plantCode = value; } }
        public string spc_id { get { return this._spc_id; } set { this._spc_id = value; } }
        public string Designator { get { return this._Designator; } set { this._Designator = value; } }
        public string Unit { get { return this._Unit; } set { this._Unit = value; } }
        public string Nominal { get { return this._Nominal; } set { this._Nominal = value; } }
        public string minNominal { get { return this._minNominal; } set { this._minNominal = value; } }
        public string maxNominal { get { return this._maxNominal; } set { this._maxNominal = value; } }
        public string UpperControlLimit { get { return this._UpperControlLimit; } set { this._UpperControlLimit = value; } }
        public string LowerControlLimit { get { return this._LowerControlLimit; } set { this._LowerControlLimit = value; } }
        public string CTQNum { get { return this._CTQNum; } set { this._CTQNum = value; } }
        public string centerline { get { return this._centerline; } set { this._centerline = value; } }
        public string CPKManager { get { return this._CPKManager; } set { this._CPKManager = value; } }
        public string section { get { return this._section; } set { this._section = value; } }
        public string Sixpointrise { get { return this._Sixpointrise; } set { this._Sixpointrise = value; } }
        public string Dropdown { get { return this._Dropdown; } set { this._Dropdown = value; } }
        public string NineNearCenter { get { return this._NineNearCenter; } set { this._NineNearCenter = value; } }
        public SQMCPKSub() { }

        public SQMCPKSub(
            string SID,
            string reportID,
            string plantCode,
            string spc_id,
            string Designator,
            string Unit,
            string Nominal,
            string minNominal,
            string maxNominal,
            string UpperControlLimit,
            string LowerControlLimit,
            string CTQNum,
            string centerline,
            string CPKManager,
            string section,
            string Sixpointrise,
            string Dropdown,
            string NineNearCenter
            )
        {
            this._SID = SID;
            this._reportID = reportID;
            this._plantCode = plantCode;
            this._spc_id = spc_id;
            this._Designator = Designator;
            this._Unit = Unit;
            this._Nominal = Nominal;
            this._minNominal = minNominal;
            this._maxNominal = maxNominal;
            this._UpperControlLimit = UpperControlLimit;
            this._LowerControlLimit = LowerControlLimit;
            this._CTQNum = CTQNum;
            this._centerline = centerline;
            this._CPKManager = CPKManager;
            this._section = section;
            this._Sixpointrise = Sixpointrise;
            this._Dropdown = Dropdown;
            this._NineNearCenter = NineNearCenter;
        }
    }

    public class SQMCPKSub_jQGridJSon
    {
        public List<SQMCPKSub> Rows = new List<SQMCPKSub>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }
    #endregion

    #region CPKSub Helper
    public static class SQMCPKSub_Helper
    {
        #region Search CPKSub
        public static String GetDataToJQGridJson(SqlConnection cn, String SearchText, SQMCPKSub DataItem)
        {
            //if (reportID == "")
            //{
            //    reportID = new Guid().ToString();
            //}
            string sSearchText = SearchText.Trim();
            string sWhereClause = "";
            if (sSearchText != "")
                sWhereClause += "   [Designator] like '%'+ @SearchText+'%'   ";
            if (sWhereClause.Length != 0)
                sWhereClause = "  AND " + sWhereClause.Substring(0);

            StringBuilder sb = new StringBuilder();
            sb.Append(@"
SELECT [SID]
     ,[plantCode]--LitNO
     ,[Designator]--spc_item
     ,[Unit]
     ,[Nominal]
     ,[minNominal]
     ,[maxNominal]
     ,[UpperControlLimit]
     ,[LowerControlLimit]
     ,[CTQNum]
     ,[centerline]
     ,[CPKManager]
     ,[section]
     ,case when Sixpointrise='true' then 'Y' else 'N' end as Sixpointrise
     ,case when Dropdown='true' then 'Y' else 'N' end as Dropdown 
     ,case when NineNearCenter='true' then 'Y' else 'N' end as NineNearCenter      
  FROM [TB_SQM_CPK_Report_sub]
  WHERE plantCode=@plantCode
  order by Designator
            ");
            DataTable dt = new DataTable();
            sb.Append(sWhereClause + ";");
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                if (sSearchText != "")
                    cmd.Parameters.Add(new SqlParameter("@SearchText", sSearchText));
                cmd.Parameters.Add(new SqlParameter("@plantCode", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.plantCode)));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }
        #endregion

        #region Create/Edit data check
        private static void UnescapeDataFromWeb(SQMCPKSub DataItem)
        {
            DataItem.spc_id = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.spc_id);
            DataItem.plantCode = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.plantCode);
            DataItem.Designator = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Designator);
            DataItem.Unit = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Unit);
            DataItem.Nominal = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Nominal);
            DataItem.minNominal = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.minNominal);
            DataItem.maxNominal = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.maxNominal);
            DataItem.UpperControlLimit = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.UpperControlLimit);
            DataItem.LowerControlLimit = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.LowerControlLimit);
            DataItem.CTQNum = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CTQNum);
            DataItem.centerline = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.centerline);
            DataItem.CPKManager = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CPKManager);
            DataItem.section = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.section);
            DataItem.Sixpointrise = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Sixpointrise);
            DataItem.Dropdown = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Dropdown);
            DataItem.NineNearCenter = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.NineNearCenter);
        }

        private static string DataCheck(SQMCPKSub DataItem)
        {
            string r = "";
            List<string> e = new List<string>();
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.plantCode))
                e.Add("Must provide  plantCode.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.spc_id))
                e.Add("Must provide spc_id");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.Unit))
                e.Add("Must provide  Unit.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.Nominal))
                e.Add("Must provide  Nominal.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.maxNominal))
                e.Add("Must provide  maxNominal.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.minNominal))
                e.Add("Must provide  minNominal.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.UpperControlLimit))
                e.Add("Must provide  UpperControlLimit.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.LowerControlLimit))
                e.Add("Must provide  LowerControlLimit.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.CTQNum))
                e.Add("Must provide  CTQNum.");

            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }
        #endregion

        #region Create data item
        public static string CreateDataItem(SqlConnection cnPortal, SQMCPKSub DataItem)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            if (r != "")
            {
                return r;
            }
            else if (CheckCreateDataItem(cnPortal, DataItem))
            {
                r = "該項目已存在";
                return r;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" 
INSERT INTO [dbo].[TB_SQM_CPK_Report_sub]
       ( [plantCode],
         [Designator],
         [Unit],
         [Nominal],
         [minNominal],
         [maxNominal],
         [UpperControlLimit],
         [LowerControlLimit],
         [CTQNum],
         [centerline],
         [CPKManager],
         [section],
         [Sixpointrise],
         [Dropdown],
         [NineNearCenter]
       )
VALUES( @plantCode, @Designator, @Unit, @Nominal, @minNominal, @maxNominal, @UpperControlLimit, @LowerControlLimit, @CTQNum, @centerline, @CPKManager, @section, @Sixpointrise, @Dropdown, @NineNearCenter );
");
                //SQM_Basic_Helper.InsertPart(sb, "3");
                SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal);
                cmd.Parameters.AddWithValue("@plantCode", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.plantCode));
                cmd.Parameters.AddWithValue("@Designator", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.spc_id));
                cmd.Parameters.AddWithValue("@Unit", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Unit));
                cmd.Parameters.AddWithValue("@Nominal", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Nominal));
                cmd.Parameters.AddWithValue("@minNominal", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.minNominal));
                cmd.Parameters.AddWithValue("@maxNominal", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.maxNominal));
                cmd.Parameters.AddWithValue("@UpperControlLimit", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.UpperControlLimit));
                cmd.Parameters.AddWithValue("@LowerControlLimit", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.LowerControlLimit));
                cmd.Parameters.AddWithValue("@CTQNum", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.CTQNum));
                cmd.Parameters.AddWithValue("@centerline", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.centerline));
                cmd.Parameters.AddWithValue("@CPKManager", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.CPKManager));
                cmd.Parameters.AddWithValue("@section", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.section));
                cmd.Parameters.AddWithValue("@Sixpointrise", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Sixpointrise));
                cmd.Parameters.AddWithValue("@Dropdown", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Dropdown));
                cmd.Parameters.AddWithValue("@NineNearCenter", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.NineNearCenter));

                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;

                return sErrMsg;
            }
        }
        public static bool CheckCreateDataItem(SqlConnection cnPortal, SQMCPKSub DataItem)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
SELECT TOP 1 [SID] 
FROM [TB_SQM_CPK_Report_sub]
WHERE [plantCode]=@plantCode AND Designator=@Designator
");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal))
            {
                cmd.Parameters.AddWithValue("@plantCode", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.plantCode));
                cmd.Parameters.AddWithValue("@Designator", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.spc_id));

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }


        #endregion

        #region Edit data item

        public static string EditDataItem(SqlConnection cnPortal, SQMCPKSub DataItem)
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

        private static string EditDataItemSub(SqlCommand cmd, SQMCPKSub DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
UPDATE [dbo].[TB_SQM_CPK_Report_sub]
SET 
      [Designator]= @Designator
      ,[Unit]= @Unit
      ,[Nominal]= @Nominal
      ,[minNominal]=@minNominal
      ,[maxNominal]=@maxNominal
      ,[UpperControlLimit]= @UpperControlLimit
      ,[LowerControlLimit]= @LowerControlLimit
      ,[CTQNum]= @CTQNum
      ,[centerline]= @centerline
      ,[CPKManager]= @CPKManager
      ,[section]= @section
      ,[Sixpointrise] = @Sixpointrise
      ,[Dropdown]= @Dropdown
      ,[NineNearCenter]= @NineNearCenter
WHERE SID=@SID
");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@SID", DataItem.SID);
            cmd.Parameters.AddWithValue("@plantCode", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.plantCode));
            cmd.Parameters.AddWithValue("@Designator", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.spc_id));
            cmd.Parameters.AddWithValue("@Unit", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Unit));
            cmd.Parameters.AddWithValue("@Nominal", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Nominal));
            cmd.Parameters.AddWithValue("@minNominal", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.minNominal));
            cmd.Parameters.AddWithValue("@maxNominal", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.maxNominal));
            cmd.Parameters.AddWithValue("@UpperControlLimit", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.UpperControlLimit));
            cmd.Parameters.AddWithValue("@LowerControlLimit", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.LowerControlLimit));
            cmd.Parameters.AddWithValue("@CTQNum", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.CTQNum));
            cmd.Parameters.AddWithValue("@centerline", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.centerline));
            cmd.Parameters.AddWithValue("@CPKManager", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.CPKManager));
            cmd.Parameters.AddWithValue("@section", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.section));
            cmd.Parameters.AddWithValue("@Sixpointrise", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Sixpointrise));
            cmd.Parameters.AddWithValue("@Dropdown", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Dropdown));
            cmd.Parameters.AddWithValue("@NineNearCenter", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.NineNearCenter));
            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion

        #region Delete data item

        public static string DeleteDataItem(SqlConnection cnPortal, SQMCPKSub DataItem)
        {

            string r = "";
         
            
                using (SqlCommand cmd = new SqlCommand("", cnPortal)) { r = DeleteDataItemSub(cmd, DataItem); }
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

        private static string DeleteDataItemSub(SqlCommand cmd, SQMCPKSub DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
DELETE [dbo].[TB_SQM_CPK_Report_sub]
WHERE SID=@SID;
DELETE [TB_SQM_CPK_Report_CTQ] WHERE [reportID]=@reportID;
");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@reportID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.reportID));
            cmd.Parameters.AddWithValue("@SID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SID));
            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion
    }
    #endregion
    #region SQMCPKData
    public class SQMCPKData
    {
        protected string _reportID;
        protected string _CTQ;
        protected string _Designator;
        public string reportID { get { return this._reportID; } set { this._reportID = value; } }
        public string CTQ { get { return this._CTQ; } set { this._CTQ = value; } }
        public string Designator { get { return this._Designator; } set { this._Designator = value; } }
        public SQMCPKData() { }
        public SQMCPKData(string reportID, string CTQ, string Designator)
        {
            this._reportID = reportID;
            this._CTQ = CTQ;
            this._Designator = Designator;
        }
    }
    public class SQMCPKData_jQGridJSon
    {
        public List<SQMCPKData> Rows = new List<SQMCPKData>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }
    #endregion
    #region SQMCPKData Helper
    public static class SQMCPKData_Helper
    {
        #region search
        public static String GetDataToJQGridJson(SqlConnection cn, String SearchText, SQMCPKData DataItem)
        {
            string sSearchText = SearchText.Trim();
            string sWhereClause = "";
            if (sSearchText != "")
                sWhereClause += "   CTQ like '%'+ @SearchText+'%'   ";
            if (sWhereClause.Length != 0)
                sWhereClause = "  AND " + sWhereClause.Substring(0);

            StringBuilder sb = new StringBuilder();
            sb.Append(@"

SELECT [reportID]
      ,[CTQ]
      ,[Designator]
  FROM [TB_SQM_CPK_Report_CTQ]
  WHERE [reportID]=@reportID AND [Designator]=@Designator

            ");
            DataTable dt = new DataTable();
            sb.Append(sWhereClause + ";");
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                if (sSearchText != "")
                    cmd.Parameters.Add(new SqlParameter("@SearchText", sSearchText));
                cmd.Parameters.Add(new SqlParameter("@reportID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.reportID)));
                cmd.Parameters.Add(new SqlParameter("@Designator", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Designator)));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }
        #endregion
        #region Create/Edit data check
        private static void UnescapeDataFromWeb(SQMCPKData DataItem)
        {
            DataItem.reportID = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.reportID);
            DataItem.CTQ = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CTQ);
            DataItem.Designator = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Designator);
        }

        private static string DataCheck(SQMCPKData DataItem)
        {
            string r = "";
            List<string> e = new List<string>();
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.CTQ))
                e.Add("Must provide Part CTQ.");

            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }
        #endregion
        #region Create data item
        public static string CreateDataItem(SqlConnection cnPortal, SQMCPKData DataItem)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            if (r != "")
            {
                return r;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" 
INSERT INTO [dbo].[TB_SQM_CPK_Report_CTQ]([reportID] ,[CTQ] ,[Designator])
VALUES(@reportID ,@CTQ ,@Designator)
");
                //SQM_Basic_Helper.InsertPart(sb, "3");
                SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal);
                cmd.Parameters.AddWithValue("@reportID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.reportID));
                cmd.Parameters.AddWithValue("@CTQ", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.CTQ));
                cmd.Parameters.AddWithValue("@Designator", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Designator));

                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;

                return sErrMsg;
            }
        }


        #endregion
        #region 根据cpk发送alter
        public static void SendEmail(SqlConnection cn, SQMCPKData DataItem, string MemberGUID)
        {
            string urlPre = CommonHelper.urlPre;
            DataTable dt1 = GetNumbyCpk(cn,DataItem);
            DataTable dtNum = GetNumbyData(cn, DataItem);
            bool isLow = false;
            string email = GetEMailbyID(cn,MemberGUID);
            DataTable dt = GetCpkByAll(cn, DataItem, MemberGUID);
            if (dt1.Rows.Count>0)
            {
                if (Convert.ToInt32(dt1.Rows[0][0].ToString()).Equals(dtNum.Rows.Count))
                {
                    isLow = IsneedAlter(isLow, dt, DataItem);

                    if (!isLow)
                    {
                        icm045.CMSHandler MS = new icm045.CMSHandler();
                        MS.MailSend("SupplierPortal",
                                       "SupplierPortal@liteon.com",
                                      email,
                                       "",
                                       "",
                                       "CPK異常",
                                      "Dear <br/> 請進入網址,填写异常信息回复。<br/> <a href='" +urlPre + @"/CPK/CPK?ReportID=" + desEncryptBase64(DataItem.reportID) + "'>網址</a>",
                                       icm045.MailPriority.Normal,
                                       icm045.MailFormat.Html,
                                       new string[0]);
                    }
                }
              
            }
           
        }

        private static DataTable GetNumbyData(SqlConnection cn, SQMCPKData dataItem)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
    SELECT CTQ   FROM [TB_SQM_CPK_Report_CTQ]
where  Designator=@Designator and reportID=@reportID
            ");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@Designator", SQMStringHelper.NullOrEmptyStringIsDBNull(dataItem.Designator)));
                cmd.Parameters.Add(new SqlParameter("@reportID", SQMStringHelper.NullOrEmptyStringIsDBNull(dataItem.reportID)));

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }

            return dt;
        }

        private static DataTable GetNumbyCpk(SqlConnection cn,SQMCPKData dataItem)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
    SELECT CTQNum   FROM [TB_SQM_CPK_Report_sub]
where  Designator=@Designator
            ");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@Designator", SQMStringHelper.NullOrEmptyStringIsDBNull(dataItem.Designator)));

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }

            return dt;
        }

        static String sKEY = "a@123456";
        static String sIV = "a@654321";
        public static String desEncryptBase64(String source)
        {
            String encrypt = "";
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] key = Encoding.ASCII.GetBytes(sKEY);
                byte[] iv = Encoding.ASCII.GetBytes(sIV);
                byte[] dataByteArray = Encoding.UTF8.GetBytes(source);

                des.Key = key;
                des.IV = iv;

                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(dataByteArray, 0, dataByteArray.Length);
                    cs.FlushFinalBlock();
                    encrypt = Convert.ToBase64String(ms.ToArray());
                    StringBuilder sInsertEncrypt = new StringBuilder();
                    for (int i = 0; i < encrypt.Length; i++)
                    {
                        sInsertEncrypt.Append(encrypt[i] + ".");
                    }
                    encrypt = sInsertEncrypt.ToString();
                    encrypt = encrypt.Replace(".+.", ".ADD.");
                    encrypt = encrypt.Replace(".=", "=");
                    encrypt = encrypt.Replace("=.", "=");
                }
                encrypt = System.Web.HttpUtility.HtmlEncode(encrypt);
                //encrypt = Page.Server.UrlEncode(encrypt);
            }
            return encrypt;
        }

        private static string GetEMailbyID(SqlConnection cn, string MemberGUID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
    SELECT PrimaryEmail   FROM [PORTAL_Members]
where  MemberGUID=@MemberGUID
            ");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@MemberGUID", SQMStringHelper.NullOrEmptyStringIsDBNull(MemberGUID)));
               
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }

            return dt.Rows[0][0].ToString();
        }

        private static bool IsneedAlter(bool isLow, DataTable dt, SQMCPKData DataItem)
        {
            Dictionary<string, decimal> list =new  Dictionary<string, decimal>();
            string check_6u = string.Empty;
            string check_6d = string.Empty;
            string check_9m = string.Empty;
            decimal SL = 0;
            foreach (DataRow dr in dt.Rows)
            {
                decimal STDEV = Convert.ToDecimal(dr["STDEV"].ToString());
                decimal USL = Convert.ToDecimal(dr["USL"].ToString());
                decimal LSL = Convert.ToDecimal(dr["LSL"].ToString());
                SL = Convert.ToDecimal(dr["SL"].ToString());
                decimal AVG = Convert.ToDecimal(dr["AVG"].ToString());
                decimal iCPK = Convert.ToDecimal(dr["CPK"].ToString());
                string reportid = dr["reportID"].ToString();
                 check_6u = dr["check_6u"].ToString();
                 check_6d = dr["check_6d"].ToString();
                 check_9m = dr["check_9m"].ToString();
                decimal CA = 0;
                if ((USL - SL) != 0)
                {
                    CA = (AVG - SL) / (USL - SL);
                }
                decimal CP = (USL - LSL) / 6 / STDEV;
                decimal CPK = (1 - Math.Abs(CA)) * CP;
             
                if (reportid.Equals(DataItem.reportID))
                {
                    if (CPK - iCPK > 0)
                    {
                        isLow = true;
                    }
                }
                list.Add(reportid,CPK);
            }
            if (!isLow)
            {
                if (check_6u.Equals("Y"))
                {
                    decimal num = 0;
                    if (list.Count > 6)
                    {
                        foreach (var item in list)
                        {
                            if (item.Value > num)
                            {
                                isLow = true;

                            }
                            else
                            {
                                isLow = false;

                            }
                            num = item.Value;
                        }

                    }
                }
                if (check_6d.Equals("Y"))
                {
                    decimal num = 0;
                    if (list.Count > 6)
                    {
                        foreach (var item in list)
                        {
                            if (item.Value < num)
                            {
                                isLow = true;

                            }
                            else
                            {
                                isLow = false;
                            }
                            num = item.Value;
                        }

                    }
                }
                if (check_9m.Equals("Y"))
                {
                    int i = 0;
                    if (list.Count > 9)
                    {
                        foreach (var item in list)
                        {
                            if (Math.Abs(item.Value - SL) <= 2)
                            {
                                i++;
                            }
                        }
                        if (i.Equals(list.Count))
                        {
                            isLow = true;
                        }
                    }
                }
            }
           

            return isLow;
        }

        private static DataTable GetCpkByAll(SqlConnection cn, SQMCPKData DataItem, string MemberGUID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
        select a.reportID, CONVERT(decimal(18, 3),avg(CTQ) )as AVG  ,
  CONVERT(decimal(18, 3),STDEV([CTQ])) as STDEV,
   [usl],
       [lsl],
       [sl],
       [ucl],
       [lcl],[cpk],[check_6u]
      ,[check_6d] ,[check_9m]
   FROM TB_SQM_CPK_Report a   
   INNER join [TB_SQM_SFC_DATA] b on b.LITNO=a.PLANTCODE 
   inner join [TB_SQM_CPK_Report_CTQ] c on B.spc_item=C.DESIGNATOR AND C.REPORTID=A.reportID
   where designator=@designator  and  a.MemberGUID=@MemberGUID
   group by a.reportID, [usl],[lsl],[sl],[ucl],[lcl],[cpk],[check_6u]
      ,[check_6d] ,[check_9m]
	   order by a.reportID 
            ");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@MemberGUID", SQMStringHelper.NullOrEmptyStringIsDBNull(MemberGUID)));
                cmd.Parameters.Add(new SqlParameter("@Designator", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Designator)));

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }

            return dt;
        }
        #endregion
        #region Statistic
        public static String GetStatisticData(SqlConnection cn, SQMCPKData DataItem)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
SELECT 
	[reportID]
	,[Designator]
	,COUNT([CTQ]) AS countCTQ
	,MAX([CTQ]) AS maxCTQ
	,MIN([CTQ]) AS minCTQ
	,SUM([CTQ]) AS sumCTQ
	,AVG([CTQ]) AS avgCTQ
	,CONVERT(float,STDEV([CTQ])) AS stdevCTQ      
FROM [TB_SQM_CPK_Report_CTQ]
WHERE [reportID]=@reportID AND [Designator]=@Designator
GROUP BY [reportID],[Designator]
            ");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@reportID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.reportID)));
                cmd.Parameters.Add(new SqlParameter("@Designator", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Designator)));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }
        #endregion
    }
    #endregion
    #endregion
}
