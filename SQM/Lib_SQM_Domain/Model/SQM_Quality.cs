using Lib_Portal_Domain.Model;
using Lib_SQM_Domain.SharedLibs;
using Lib_VMI_Domain.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace Lib_SQM_Domain.Model
{
    #region SQM_Quality
    public class SQM_Quality
    {
        protected string _ReportSID;
        protected string _ReportNo;
        protected string _Supplier;
        protected string _DeliveryDate;
        protected string _Qty;
        protected string _SupplierNo;
        protected string _LiteNo;
        protected string _LotNo;
        protected string _DateCode;
        protected string _OQCDate;
        protected string _MFGLocation;
        protected string _SupplierRemark;
        protected string _isChange;
        protected string _ChangeNote;
        protected string _Equipment;
        protected string _Material;
        protected string _Inspector;
        protected string _Approveder;
        protected string _ReportType;
        protected string _ReportName;
        protected string _AQL;

        public string ReportSID { get { return this._ReportSID; } set { this._ReportSID = value; } }
        public string ReportNo { get { return this._ReportNo; } set { this._ReportNo = value; } }
        public string Supplier { get { return this._Supplier; } set { this._Supplier = value; } }
        public string DeliveryDate { get { return this._DeliveryDate; } set { this._DeliveryDate = value; } }
        public string Qty { get { return this._Qty; } set { this._Qty = value; } }
        public string SupplierNo { get { return this._SupplierNo; } set { this._SupplierNo = value; } }
        public string LiteNo { get { return this._LiteNo; } set { this._LiteNo = value; } }
        public string LotNo { get { return this._LotNo; } set { this._LotNo = value; } }
        public string DateCode { get { return this._DateCode; } set { this._DateCode = value; } }
        public string OQCDate { get { return this._OQCDate; } set { this._OQCDate = value; } }
        public string MFGLocation { get { return this._MFGLocation; } set { this._MFGLocation = value; } }
        public string SupplierRemark { get { return this._SupplierRemark; } set { this._SupplierRemark = value; } }
        public string isChange { get { return this._isChange; } set { this._isChange = value; } }
        public string ChangeNote { get { return this._ChangeNote; } set { this._ChangeNote = value; } }
        public string Equipment { get { return this._Equipment; } set { this._Equipment = value; } }
        public string Material { get { return this._Material; } set { this._Material = value; } }
        public string Inspector { get { return this._Inspector; } set { this._Inspector = value; } }
        public string Approveder { get { return this._Approveder; } set { this._Approveder = value; } }
        public string ReportType { get { return this._ReportType; } set { this._ReportType = value; } }
        public string ReportName { get { return this._ReportName; } set { this._ReportName = value; } }
        public string AQL { get { return this._AQL; } set { this._AQL = value; } }
        public SQM_Quality()
        {

        }
        public SQM_Quality(string ReportSID,
            string ReportNo,
            string Supplier,
            string DeliveryDate,
            string Qty,
            string SupplierNo,
            string LiteNo,
            string LotNo,
            string DateCode,
            string OQCDate,
            string MFGLocation,
            string SupplierRemark,
            string isChange,
            string ChangeNote,
            string Equipment,
            string Material,
            string Inspector,
            string Approveder,
            string ReportType,
            string ReportName,string AQL)
        {
            this._ReportSID = ReportSID;
            this._ReportNo = ReportNo;
            this._Supplier = Supplier;
            this._DeliveryDate = DeliveryDate;
            this._Qty = Qty;
            this._SupplierNo = SupplierNo;
            this._LiteNo = LiteNo;
            this._LotNo = LotNo;
            this._DateCode = DateCode;
            this._OQCDate = OQCDate;
            this._MFGLocation = MFGLocation;
            this._SupplierRemark = SupplierRemark;
            this._isChange = isChange;
            this._ChangeNote = ChangeNote;
            this._Equipment = Equipment;
            this._Material = Material;
            this._Inspector = Inspector;
            this._Approveder = Approveder;
            this._ReportType = ReportType;
            this._ReportName = ReportName;
            this._AQL = AQL;
        }
    }

    public class SQM_Quality_jQGridJSon
    {
        public List<SQM_Quality> Rows = new List<SQM_Quality>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }

    public static class SQM_Quality_Helper
    {
        #region Create/Edit data check
        private static void UnescapeDataFromWeb(SQM_Quality DataItem)
        {
            DataItem.ReportSID = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ReportSID);
            DataItem.ReportNo = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ReportNo);
            DataItem.Supplier = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Supplier);
            DataItem.DeliveryDate = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.DeliveryDate);
            DataItem.Qty = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Qty);
            DataItem.SupplierNo = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.SupplierNo);
            DataItem.LiteNo = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.LiteNo);
            DataItem.LotNo = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.LotNo);
            DataItem.DateCode = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.DateCode);
            DataItem.OQCDate = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.OQCDate);
            DataItem.MFGLocation = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.MFGLocation);
            DataItem.SupplierRemark = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.SupplierRemark);
            DataItem.isChange = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.isChange);
            DataItem.ChangeNote = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ChangeNote);
            DataItem.Equipment = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Equipment);
            DataItem.Material = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Material);
            DataItem.Inspector = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Inspector);
        }
        private static string DataCheck(SQM_Quality DataItem)
        {
            string r = "";
            List<string> e = new List<string>();



            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.DeliveryDate))
                e.Add("Must provide DeliveryDate.");

            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.Qty))
                e.Add("Must provide Qty.");

            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.SupplierNo))
                e.Add("Must provide SupplierNo.");

            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.LiteNo))
                e.Add("Must provide LiteNo.");

            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.LotNo))
                e.Add("Must provide LotNo.");

            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.DateCode))
                e.Add("Must provide DateCode.");

            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.OQCDate))
                e.Add("Must provide OQCDate.");

            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.MFGLocation))
                e.Add("Must provide MFGLocation.");

            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.SupplierRemark))
                e.Add("Must provide SupplierRemark.");

            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.isChange))
                e.Add("Must provide isChange.");

            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.Equipment))
                e.Add("Must provide Equipment.");

            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.Material))
                e.Add("Must provide Material.");

            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.Inspector))
                e.Add("Must provide Inspector.");

            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }
        #endregion

        public static string CreateDataItem(SqlConnection cnPortal, SQM_Quality DataItem, String memberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            if (DataItem.isChange == "0")
            {
                DataItem.ChangeNote = "";
            }
            string r = DataCheck(DataItem);
            string ReportNo = Assignment(cnPortal);
            StringBuilder sb = new StringBuilder();
            if (r != "")
            { return r; }
            else
            {
                sb.Append(@"
DECLARE @Supplier nvarchar(50) =(
SELECT [NAME] 
FROM [TB_EB_USER]
WHERE [USER_GUID]=@MemberGUID
);
Insert Into TB_SQM_Quality_Report (ReportSID,ReportNo,MemberGUID,Supplier,DeliveryDate,Qty,Supplier_No,LiteNo,LotNo,DateCode,OQCDate,MFGLocation,SupplierRemark,isChange,ChangeNote,Equipment,Material,Inspector,ReportType) ");
                sb.Append(@"Values ( @ReportSID,@ReportNo,@MemberGUID,@Supplier,
                    @DeliveryDate,@Qty,@Supplier_No,@LiteNo,@LotNo,@DateCode,@OQCDate,
                    @MFGLocation,@SupplierRemark,@isChange,@ChangeNote,@Equipment,@Material,@Inspector,@ReportType);");
                SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal);

                cmd.Parameters.AddWithValue("@ReportSID", Guid.NewGuid());
                cmd.Parameters.AddWithValue("@ReportNo", ReportNo);
                cmd.Parameters.AddWithValue("@MemberGUID", SQMStringHelper.NullOrEmptyStringIsDBNull(memberGUID));
                cmd.Parameters.AddWithValue("@DeliveryDate", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.DeliveryDate));
                cmd.Parameters.AddWithValue("@Qty", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Qty));
                cmd.Parameters.AddWithValue("@Supplier_No", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SupplierNo));
                cmd.Parameters.AddWithValue("@LiteNo", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.LiteNo));
                cmd.Parameters.AddWithValue("@LotNo", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.LotNo));
                cmd.Parameters.AddWithValue("@DateCode", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.DateCode));
                cmd.Parameters.AddWithValue("@OQCDate", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.OQCDate));
                cmd.Parameters.AddWithValue("@MFGLocation", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.MFGLocation));
                cmd.Parameters.AddWithValue("@SupplierRemark", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SupplierRemark));
                cmd.Parameters.AddWithValue("@isChange", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.isChange));
                cmd.Parameters.AddWithValue("@ChangeNote", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ChangeNote));
                cmd.Parameters.AddWithValue("@Equipment", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Equipment));
                cmd.Parameters.AddWithValue("@Material", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Material));
                cmd.Parameters.AddWithValue("@Inspector", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Inspector));
                cmd.Parameters.AddWithValue("@ReportType", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ReportType));
                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;

                return sErrMsg;
            }
        }

        public static string EditDataItem(SqlConnection cnPortal, SQM_Quality DataItem)
        {
            return EditDataItem(cnPortal, DataItem, "", "");
        }
        public static string EditDataItem(SqlConnection cnPortal, SQM_Quality DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            if (DataItem.isChange == "0")
            {
                DataItem.ChangeNote = "";
            }
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

        private static string EditDataItemSub(SqlCommand cmd, SQM_Quality DataItem)
        {
            string sErrMsg = "";

            string sSQL = "Update TB_SQM_Quality_Report Set Supplier=@Supplier,DeliveryDate=@DeliveryDate,Qty=@Qty,Supplier_No=@Supplier_No,LiteNo=@LiteNo,LotNo=@LotNo,DateCode=@DateCode,OQCDate=@OQCDate,MFGLocation=@MFGLocation,SupplierRemark=@SupplierRemark,isChange=@isChange,ChangeNote=@ChangeNote,Equipment=@Equipment,Material=@Material,Inspector=@Inspector,ReportType=@ReportType";
            sSQL += " Where ReportSID = @ReportSID;";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@Supplier", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Supplier));
            cmd.Parameters.AddWithValue("@DeliveryDate", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.DeliveryDate));
            cmd.Parameters.AddWithValue("@Qty", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Qty));
            cmd.Parameters.AddWithValue("@Supplier_No", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SupplierNo));
            cmd.Parameters.AddWithValue("@LiteNo", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.LiteNo));
            cmd.Parameters.AddWithValue("@LotNo", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.LotNo));
            cmd.Parameters.AddWithValue("@DateCode", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.DateCode));
            cmd.Parameters.AddWithValue("@OQCDate", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.OQCDate));
            cmd.Parameters.AddWithValue("@MFGLocation", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.MFGLocation));
            cmd.Parameters.AddWithValue("@SupplierRemark", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SupplierRemark));
            cmd.Parameters.AddWithValue("@isChange", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.isChange));
            cmd.Parameters.AddWithValue("@ChangeNote", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ChangeNote));
            cmd.Parameters.AddWithValue("@Equipment", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Equipment));
            cmd.Parameters.AddWithValue("@Material", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Material));
            cmd.Parameters.AddWithValue("@Inspector", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Inspector));
            cmd.Parameters.AddWithValue("@ReportSID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ReportSID));
            cmd.Parameters.AddWithValue("@ReportType", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ReportType));

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        public static string DeleteDataItem(SqlConnection cnPortal, SQM_Quality DataItem)
        {
            return DeleteDataItem(cnPortal, DataItem, "", "");
        }
        public static string DeleteDataItem(SqlConnection cnPortal, SQM_Quality DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            string r = "";
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.ReportSID))
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
        private static string DeleteDataItemSub(SqlCommand cmd, SQM_Quality DataItem)
        {
            string sErrMsg = "";

            string sSQL = "Delete TB_SQM_Quality_Report Where ReportSID = @ReportSID;Delete TB_SQM_Quality_Doc_File Where ReportSID = @ReportSID;Delete TB_SQM_Quality_Insp Where ReportSID = @ReportSID;";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@ReportSID", DataItem.ReportSID);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        public static string GetDataToJQGridJson(SqlConnection cn, string SearchText, SQM_Quality DataItem, string memberGUID)
        {
            SQM_Quality_jQGridJSon m = new SQM_Quality_jQGridJSon();
            string sSearchText = SearchText.Trim();
            string sWhereClause = "";
            if (sSearchText != "")
                sWhereClause += " and ReportNo like '%' + @ReportNo + '%'";


            m.Rows.Clear();
            int iRowCount = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
SELECT  [ReportSID]
      ,[ReportNo]
      ,[Supplier]
      ,[DeliveryDate]
      ,[Qty]
      ,[Supplier_No]
      ,[LiteNo]
      ,[LotNo]
      ,[DateCode]
      ,[OQCDate]
      ,[MFGLocation]
      ,[SupplierRemark]
      ,[isChange]
      ,[ChangeNote]
      ,[Equipment]
      ,[Material]
      ,[Inspector]
      ,[Approveder]
      ,ReportType
	  ,[TB_SQM_Quality_Report_Type].ReportName
      ,(SELECT AQL
  FROM TB_SQM_AQL_PLANT_Map m 
  inner join TB_SQM_LitNO_Plant p on p.TB_SQM_AQL_PLANT_SID=m.SSID
  where r.Qty  between PARSENAME(REPLACE(AQLNum,'~','.'),2) and PARSENAME(REPLACE(AQLNum,'~','.'),1)
  and m.AQLType='0' and litNo=r.liteno) as AQL
  FROM [TB_SQM_Quality_Report] r
LEFT JOIN [dbo].[TB_SQM_Quality_Report_Type] ON r.ReportType=SID
        where  r.MemberGUID=@MemberGUID 
           ");
            if (DataItem.ReportType != null)
            {
                sb.Append("AND ReportType=@ReportType");
            }
            if (sSearchText != "")
            {
                sb.Append(sWhereClause + ";");
            }
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                //if (sSearchText != "")
                cmd.Parameters.Add(new SqlParameter("@MemberGUID", memberGUID));
                cmd.Parameters.Add(new SqlParameter("@ReportType", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ReportType)));
                cmd.Parameters.Add(new SqlParameter("@ReportNo", SearchText));
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    iRowCount++;
                    m.Rows.Add(new SQM_Quality(
                        dr["ReportSID"].ToString(),
                        dr["ReportNo"].ToString(),
                        dr["Supplier"].ToString(),
                        dr["DeliveryDate"].ToString(),
                        dr["Qty"].ToString(),
                        dr["Supplier_No"].ToString(),
                        dr["LiteNo"].ToString(),
                        dr["LotNo"].ToString(),
                        dr["DateCode"].ToString(),
                        dr["OQCDate"].ToString(),
                        dr["MFGLocation"].ToString(),
                        dr["SupplierRemark"].ToString(),
                        dr["isChange"].ToString(),
                        dr["ChangeNote"].ToString(),
                        dr["Equipment"].ToString(),
                        dr["Material"].ToString(),
                        dr["Inspector"].ToString(),
                        dr["Approveder"].ToString(),
                        dr["ReportType"].ToString(),
                        dr["ReportName"].ToString(),
                        dr["AQL"].ToString()
                       ));
                }
                dr.Close();
                dr = null;
            }

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(m);
        }
        public static string GetDataToJQGridJson(SqlConnection cn, SQM_Quality DataItem)
        {
            return GetDataToJQGridJson(cn, "", DataItem, "");
        }
        //生成流水號
        public static string Assignment(SqlConnection cnPortal)
        {
            string LastNumStr = getLastNumStr(cnPortal); ;
            string number0 = "";
            DateTime date = System.DateTime.Now;
            string year = date.Year.ToString();
            string month = date.Month.ToString();
            string day = date.Day.ToString();
            if (month.Length < 2)
                month = '0' + month;
            if (day.Length < 2)
                day = '0' + day;
            string ymd = year + month + day;
            if (LastNumStr.Length < 8 || LastNumStr.Substring(0, 8) != ymd)
            {
                return ymd + "001";
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
            sb.Append("Select Max(ReportNo) from TB_SQM_Quality_Report");
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
    }
    #endregion
    #region SQM_QualityInsp
    public class SQM_QualityInsp
    {
        protected string _ReportSID;
        protected string _InspCode;
        protected string _InspCodeID;
        protected string _InspDesc;
        protected string _InspDescID;
        protected string _Standard;
        protected string _CR;
        protected string _MA;
        protected string _MI;
        protected string _InspNum;
        protected string _isOther;
        protected string _Other;
        protected string _Judge;
        protected string _InspResult;
        protected string _UCL;
        protected string _LCL;
        protected string _InspTool;
        protected string _AQL;
        protected string _Insptype;
        public string ReportSID { get { return this._ReportSID; } set { this._ReportSID = value; } }
        public string InspCode { get { return this._InspCode; } set { this._InspCode = value; } }
        public string InspCodeID { get { return this._InspCodeID; } set { this._InspCodeID = value; } }
        public string InspDesc { get { return this._InspDesc; } set { this._InspDesc = value; } }
        public string InspDescID { get { return this._InspDescID; } set { this._InspDescID = value; } }
        public string Standard { get { return this._Standard; } set { this._Standard = value; } }
        public string CR { get { return this._CR; } set { this._CR = value; } }
        public string MA { get { return this._MA; } set { this._MA = value; } }
        public string MI { get { return this._MI; } set { this._MI = value; } }
        public string InspNum { get { return this._InspNum; } set { this._InspNum = value; } }
        public string isOther { get { return this._isOther; } set { this._isOther = value; } }
        public string Other { get { return this._Other; } set { this._Other = value; } }
        public string Judge { get { return this._Judge; } set { this._Judge = value; } }
        public string InspResult { get { return this._InspResult; } set { this._InspResult = value; } }
        public string UCL { get { return this._UCL; } set { this._UCL = value; } }
        public string LCL { get { return this._LCL; } set { this._LCL = value; } }
        public string InspTool { get { return this._InspTool; } set { this._InspTool = value; } }
        public string AQL { get { return this._AQL; } set { this._AQL = value; } }
        public string Insptype { get { return this._Insptype; } set { this._Insptype = value; } }
        public SQM_QualityInsp()
        {

        }
        public SQM_QualityInsp(string ReportSID, string InspCode, string InspCodeID,
            string InspDesc,
            string InspDescID,
            string Standard,
            string CR,
            string MA,
            string MI,
            string InspNum,
            string isOther,
            string Other,
            string Judge,
            string InspResult,
            string UCL,
            string LCL,
            string InspTool,
            string AQL,
            string Insptype

         )
        {
            this._ReportSID = ReportSID;
            this._InspCode = InspCode;
            this._InspCodeID = InspCodeID;
            this._InspDesc = InspDesc;
            this._InspDescID = InspDescID;
            this._Standard = Standard;
            this._CR = CR;
            this._MA = MA;
            this._MI = MI;
            this._InspNum = InspNum;
            this._isOther = isOther;
            this._Other = Other;
            this._Judge = Judge;
            this._InspResult = InspResult;
            this._UCL = UCL;
            this._LCL = LCL;
            this._InspTool = InspTool;
            this._AQL = AQL;
            this._Insptype = Insptype;

        }
    }

    public class SQM_QualityInsp_jQGridJSon
    {
        public List<SQM_QualityInsp> Rows = new List<SQM_QualityInsp>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }

    public static class SQM_QualityInsp_Helper
    {
        #region Create/Edit data check
        private static void UnescapeDataFromWeb(SQM_QualityInsp DataItem)
        {
            DataItem.ReportSID = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ReportSID);
            DataItem.InspCode = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.InspCode);
            DataItem.InspDesc = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.InspDesc);
            DataItem.Standard = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Standard);
            DataItem.CR = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CR);
            DataItem.MA = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.MA);
            DataItem.MI = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.MI);
            DataItem.InspNum = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.InspNum);
            DataItem.isOther = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.isOther);
            DataItem.Other = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Other);
            DataItem.Judge = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Judge);
            DataItem.InspResult = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.InspResult);
            DataItem.InspTool = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.InspTool);
        }
        private static string DataCheck(SQM_QualityInsp DataItem)
        {
            string r = "";
            List<string> e = new List<string>();


            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.Judge))
                e.Add("Must provide Judge.");
            //if (SQMStringHelper.DataIsNullOrEmpty(DataItem.InspResult))
            //    e.Add("Must provide InspResult.");

            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }
        #endregion

        public static string CreateDataItem(SqlConnection cnPortal, SQM_QualityInsp DataItem)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            StringBuilder sb = new StringBuilder();
            if (r != "")
            { return r; }
            else if (CheckCreateDataItem(cnPortal, DataItem))
            {
                r = "該項目已存在";
                return r;
            }
            else
            {
                switch (DataItem.Insptype)
                {
                    case ("Attributes"):
                        sb.Append(@"
INSERT INTO [dbo].[TB_SQM_Quality_Insp]
           ([ReportSID]
           ,[InspCodeID]
           ,[InspDescID]
           ,[Judge]
           ,[InspResult]
           ,[UCL]
           ,[LCL]
           ,[InspTool]
           ,[AQL]
           ,[InspNum]
           ,[CR]
           ,[MA]
           ,[MI])
     VALUES
           (@ReportSID
           ,@InspCodeID 
           ,@InspDescID
           ,@Judge
           ,@InspResult
           ,@UCL
           ,@LCL 
           ,@InspTool 
           ,@AQL 
           ,@InspNum 
           ,@CR 
           ,@MA 
           ,@MI )
");
                        break;
                    case ("Variables"):
                        sb.Append(@"
INSERT INTO [dbo].[TB_SQM_Quality_Insp]
           ([ReportSID]
           ,[InspCodeID]
           ,[InspDescID]
           ,[Judge]
           ,[InspResult]
           ,[UCL]
           ,[LCL]
           ,[InspTool]
           ,[AQL]
           ,[InspNum]
           ,[CR]
           ,[MA]
           ,[MI])
     VALUES
           (@ReportSID
           ,@InspCodeID 
           ,@InspDescID
           ,@Judge
           ,@InspResult
           ,@UCL
           ,@LCL 
           ,@InspTool 
           ,@AQL 
           ,@InspNum 
           ,@CR 
           ,@MA 
           ,@MI )
");
                        break;
                    default:
                        break;
                }
                SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal);

                cmd.Parameters.AddWithValue("@ReportSID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ReportSID));
                cmd.Parameters.AddWithValue("@InspCodeID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.InspCodeID));
                cmd.Parameters.AddWithValue("@InspDescID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.InspDescID));
                cmd.Parameters.AddWithValue("@Judge", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Judge));
                cmd.Parameters.AddWithValue("@InspResult", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.InspResult));
                cmd.Parameters.AddWithValue("@UCL", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.UCL));
                cmd.Parameters.AddWithValue("@LCL", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.LCL));
                cmd.Parameters.AddWithValue("@InspTool", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.InspTool));
                cmd.Parameters.AddWithValue("@AQL", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.AQL));
                cmd.Parameters.AddWithValue("@InspNum", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.InspNum));
                cmd.Parameters.AddWithValue("@CR", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.CR));
                cmd.Parameters.AddWithValue("@MA", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.MA));
                cmd.Parameters.AddWithValue("@MI", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.MI));

                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;

                return sErrMsg;
            }
        }
        public static bool CheckCreateDataItem(SqlConnection cnPortal, SQM_QualityInsp DataItem)
        {
            StringBuilder sb = new StringBuilder();


            sb.Append(@"
SELECT TOP 1 [ReportSID] FROM  TB_SQM_Quality_Insp WHERE ReportSID = @ReportSID AND InspCodeID=@InspCodeID AND InspDescID=@InspDescID
");

            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal))
            {
                cmd.Parameters.AddWithValue("@ReportSID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ReportSID));
                cmd.Parameters.AddWithValue("@InspCodeID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.InspCodeID));
                cmd.Parameters.AddWithValue("@InspDescID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.InspDescID));

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
        public static string EditDataItem(SqlConnection cnPortal, SQM_QualityInsp DataItem)
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

        private static string EditDataItemSub(SqlCommand cmd, SQM_QualityInsp DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
Update TB_SQM_Quality_Insp 
Set
       [Judge] = @Judge
      ,[InspResult] = @InspResult
      ,[UCL] = @UCL
      ,[LCL] = @LCL
      ,[InspTool] = @InspTool
      ,[AQL] = @AQL
      ,[InspNum] = @InspNum
      ,[CR] = @CR
      ,[MA] = @MA
      ,[MI] = @MI
Where ReportSID = @ReportSID and InspCodeID=@InspCodeID
and InspDescID=@InspDescID;
   ");


            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@Judge", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Judge));
            cmd.Parameters.AddWithValue("@InspResult", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.InspResult));
            cmd.Parameters.AddWithValue("@ReportSID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ReportSID));
            cmd.Parameters.AddWithValue("@InspCodeID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.InspCodeID));
            cmd.Parameters.AddWithValue("@InspDescID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.InspDescID));
            cmd.Parameters.AddWithValue("@UCL", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.UCL));
            cmd.Parameters.AddWithValue("@LCL", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.LCL));
            cmd.Parameters.AddWithValue("@InspTool", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.InspTool));
            cmd.Parameters.AddWithValue("@AQL", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.AQL));
            cmd.Parameters.AddWithValue("@InspNum", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.InspNum));
            cmd.Parameters.AddWithValue("@CR", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.CR));
            cmd.Parameters.AddWithValue("@MA", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.MA));
            cmd.Parameters.AddWithValue("@MI", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.MI));


            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        public static string DeleteDataItem(SqlConnection cnPortal, SQM_QualityInsp DataItem)
        {
            return DeleteDataItem(cnPortal, DataItem, "", "");
        }
        public static string DeleteDataItem(SqlConnection cnPortal, SQM_QualityInsp DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            string r = "";
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.ReportSID))
                return "Must provide ReportSID.";
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
        private static string DeleteDataItemSub(SqlCommand cmd, SQM_QualityInsp DataItem)
        {
            string sErrMsg = "";

            string sSQL = "Delete TB_SQM_Quality_Insp Where ReportSID = @ReportSID AND InspCodeID=@InspCodeID ";
            if (DataItem.InspDescID != null)
            {
                sSQL += "AND InspDescID=@InspDescID ;";
            }
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@ReportSID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ReportSID));
            if (DataItem.InspDescID != null)
            {
                cmd.Parameters.AddWithValue("@InspDescID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.InspDescID));
            }
            cmd.Parameters.AddWithValue("@InspCodeID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.InspCodeID));

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        public static string GetDataToJQGridJson(SqlConnection cn, string SInspCode, String SInspDesc, SQM_QualityInsp DataItem)
        {
            if (DataItem.ReportSID == "")
            {
                DataItem.ReportSID = new Guid().ToString();
            }
            //SQM_QualityInsp_jQGridJSon m = new SQM_QualityInsp_jQGridJSon();
            string sWhereClause = "";
            if (SInspCode.Trim() != "" && SInspDesc.Trim() == "")
                sWhereClause += " and InspCodeID = @InspCodeID";
            if (SInspCode.Trim() != "" && SInspDesc.Trim() != "")
                sWhereClause += " and InspCodeID = @InspCodeID and InspDescID=@InspDescID";


            //m.Rows.Clear();
            //int iRowCount = 0;
            StringBuilder sb = new StringBuilder();
            switch (DataItem.Insptype)
            {
                case ("Attributes"):
                    sb.Append(@"
  select ReportSID 
  ,TB_SQM_InspCode.sid as CodeSID
  ,TB_SQM_InspCode_Sub.sid as DescSID
  ,TB_SQM_InspCode.Name as InspCode
  ,TB_SQM_InspCode_Sub.name as InspDesc
  ,TB_SQM_Quality_Insp.InspNum
  ,TB_SQM_InspCode_Map.Standard
  ,TB_SQM_Quality_Insp.CR
  ,TB_SQM_Quality_Insp.MA
  ,TB_SQM_Quality_Insp.MI
  ,TB_SQM_InspCode_Map.Other
  ,TB_SQM_InspCode_Map.isOther
  ,TB_SQM_InspCode.[Insptype]
  ,Judge
  ,InspResult
  from  TB_SQM_Quality_Insp
  inner join TB_SQM_InspCode_Map on TB_SQM_Quality_Insp.[InspCodeID] =TB_SQM_InspCode_Map.sid  and TB_SQM_Quality_Insp.[InspDescID]=TB_SQM_InspCode_Map.SSID
  inner join TB_SQM_InspCode on TB_SQM_InspCode.sid=TB_SQM_InspCode_Map.sid
  inner join TB_SQM_InspCode_Sub on TB_SQM_InspCode_Sub.sid=TB_SQM_InspCode_Map.ssid
WHERE ReportSID = @ReportSID AND TB_SQM_InspCode.[Insptype]='Attributes'
           ");
                    break;
                case ("Variables"):
                    sb.Append(@"
select ReportSID ,
  InspCodeID,
  InspDescID,
  TB_SQM_InspCode.Name as InspCode,
  TB_SQM_InspCode_Sub.Name as InspDesc
  ,TB_SQM_Quality_Insp.InspNum
  ,TB_SQM_Quality_Insp.[UCL]
  ,TB_SQM_Quality_Insp.[LCL]
  ,TB_SQM_Quality_Insp.[InspTool]
  ,TB_SQM_Quality_Insp.[AQL]
  ,TB_SQM_InspCode.[Insptype]
  ,Judge
  from  TB_SQM_Quality_Insp
   inner join TB_SQM_InspCode on TB_SQM_InspCode.sid=TB_SQM_Quality_Insp.InspCodeID
   inner join TB_SQM_InspCode_Sub on TB_SQM_InspCode_Sub.SID=TB_SQM_Quality_Insp.InspDescID
   WHERE ReportSID = @ReportSID AND TB_SQM_InspCode.[Insptype]='Variables'
");
                    break;
                default:
                    break;
            }
            DataTable dt = new DataTable();
            sb.Append(sWhereClause + ";");
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                //if (sSearchText != "")
                cmd.Parameters.Add(new SqlParameter("@ReportSID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ReportSID)));
                if (SInspCode.Trim() != "" && SInspDesc.Trim() == "")
                    cmd.Parameters.Add(new SqlParameter("@InspCodeID", SInspCode));
                if (SInspCode.Trim() != "" && SInspDesc.Trim() != "")
                {
                    cmd.Parameters.Add(new SqlParameter("@InspCodeID", SInspCode));
                    cmd.Parameters.Add(new SqlParameter("@InspDescID", SInspDesc));
                }
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }
        public static string GetDataToJQGridJson(SqlConnection cn, SQM_QualityInsp DataItem)
        {
            return GetDataToJQGridJson(cn, "", "", DataItem);
        }
        #region GetList

        public static String GetInspCodeList(SqlConnection cn, PortalUserProfile RunAsUser, string Insptype, string LitNo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
SELECT TOP 1000 [SID]
      ,[Name]
      ,[Insptype]
FROM [TB_SQM_InspCode]
where 1=1
");
            if (!string.IsNullOrEmpty(LitNo) )
            {
                sb.Append("and LitNo=@LitNo ");
            }
            if (Insptype != "")
            {
                sb.Append("and [Insptype]=@Insptype");
            }
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                if (Insptype != "")
                {
                    cmd.Parameters.Add(new SqlParameter("@Insptype", Insptype));
                }
                if (!string.IsNullOrEmpty(LitNo))
                {
                    cmd.Parameters.Add(new SqlParameter("@LitNo", LitNo));
                }
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }
        public static String GetInspDescList(SqlConnection cn, String MainID)
        {
            if (String.IsNullOrEmpty(MainID))
            {
                return JsonConvert.SerializeObject(new DataTable());
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(@"
SELECT SIS.[SID]
	,SIS.[Name]
	,SIC.Insptype
FROM [dbo].[TB_SQM_InspCode_Sub] AS SIS 
LEFT JOIN  [TB_SQM_InspCode_Map] AS SIM ON SIM.SSID = SIS.SID
LEFT JOIN [dbo].[TB_SQM_InspCode] AS SIC ON SIM.SID=SIC.SID
WHERE SIM.SID = @SID
");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@SID", MainID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }
        public static String GetInspIfisOther(SqlConnection cn, String SID, String SSID)
        {
            StringBuilder sb = new StringBuilder();
            if (SSID != "null" && SSID != "")
            {

                sb.Append(@"
SELECT SIS.[SID]
        ,SIS.[Name]
        ,isOther
        ,Standard
        ,[CR]
        ,[MA]
        ,[MI]
        ,[Other]
        ,[InspNum]
        ,[UCL]
        ,[LCL]
        ,[InspTool]
        ,[AQL]
        ,SIC.[Insptype]
FROM [TB_SQM_InspCode_Map] AS SIM
LEFT JOIN [dbo].[TB_SQM_InspCode_Sub] AS SIS ON SIM.SSID = SIS.SID
LEFT JOIN [dbo].[TB_SQM_InspCode] AS SIC ON SIM.SID=SIC.SID
WHERE SIM.SID = @SID AND SIM.SSID = @SSID
");
            }
            else
            {
                sb.Append(@"
SELECT
        SIM.[InspNum]
        ,SIM.[UCL]
        ,SIM.[LCL]
        ,SIM.[InspTool]
        ,SIM.[AQL]
        ,SIC.[Insptype]
FROM[dbo].[TB_SQM_InspCode] AS SIC
LEFT JOIN[dbo].[TB_SQM_InspCode_Map] AS SIM ON SIC.SID=SIM.SID
WHERE SIC.SID = @SID
");

            }
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@SID", SID));
                if (SSID != "null" && SSID != "")
                {
                    cmd.Parameters.Add(new SqlParameter("@SSID", SSID));
                }
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }
        public static String GetReportTypeList(SqlConnection cn, PortalUserProfile RunAsUser)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
SELECT [SID]
      ,[ReportName]
FROM [TB_SQM_Quality_Report_Type]
");

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
    }
    #endregion
    # region SQM_QualityFile
    public class SQM_QualityFile
    {
        protected string _ReportSID;
        protected string _DocName;
        protected string _DocInspResult;
        protected string _DocNo;
        protected string _FGuid;


        public string ReportSID { get { return this._ReportSID; } set { this._ReportSID = value; } }
        public string DocName { get { return this._DocName; } set { this._DocName = value; } }
        public string DocInspResult { get { return this._DocInspResult; } set { this._DocInspResult = value; } }
        public string DocNo { get { return this._DocNo; } set { this._DocNo = value; } }
        public string FGuid { get { return this._FGuid; } set { this._FGuid = value; } }

        public SQM_QualityFile()
        {

        }
        public SQM_QualityFile(string ReportSID,
            string DocName,
            string DocInspResult,
            string DocNo,
            string FGuid
         )
        {
            this._ReportSID = ReportSID;
            this._DocName = DocName;
            this._DocInspResult = DocInspResult;
            this._DocNo = DocNo;
            this._FGuid = FGuid;


        }
    }

    public class SQM_QualityFile_jQGridJSon
    {
        public List<SQM_QualityFile> Rows = new List<SQM_QualityFile>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }

    public static class SQM_QualityFile_Helper
    {
        #region Create/Edit data check
        private static void UnescapeDataFromWeb(SQM_QualityFile DataItem)
        {
            DataItem.ReportSID = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ReportSID);
            DataItem.DocName = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.DocName);
            DataItem.DocInspResult = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.DocInspResult);
            DataItem.DocNo = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.DocNo);
            DataItem.FGuid = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.FGuid);
        }
        private static string DataCheck(SQM_QualityFile DataItem)
        {
            string r = "";
            List<string> e = new List<string>();

            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.DocName))
                e.Add("Must provide DocName.");


            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }
        #endregion

        public static string CreateDataItem(SqlConnection cnPortal, SQM_QualityFile DataItem, String memberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            StringBuilder sb = new StringBuilder();
            if (r != "")
            { return r; }
            else
            {
                sb.Append("Insert Into TB_SQM_Quality_Doc_File (ReportSID,DocName,DocInspResult,DocNo) ");
                sb.Append(@"Values ( @ReportSID,@DocName,@DocInspResult,@DocNo);");
                SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal);

                cmd.Parameters.AddWithValue("@ReportSID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ReportSID));
                cmd.Parameters.AddWithValue("@DocName", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.DocName));
                cmd.Parameters.AddWithValue("@DocInspResult", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.DocInspResult));
                cmd.Parameters.AddWithValue("@DocNo", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.DocNo));

                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;

                return sErrMsg;
            }
        }

        public static string EditDataItem(SqlConnection cnPortal, SQM_QualityFile DataItem)
        {
            return EditDataItem(cnPortal, DataItem, "", "");
        }
        public static string EditDataItem(SqlConnection cnPortal, SQM_QualityFile DataItem, string LoginMemberGUID, string RunAsMemberGUID)
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

        private static string EditDataItemSub(SqlCommand cmd, SQM_QualityFile DataItem)
        {
            string sErrMsg = "";

            string sSQL = "Update TB_SQM_Quality_Doc_File Set DocInspResult=@DocInspResult,DocNo=@DocNo";
            sSQL += " Where ReportSID = @ReportSID and DocName=@DocName;";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@DocInspResult", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.DocInspResult));
            cmd.Parameters.AddWithValue("@DocNo", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.DocNo));
            cmd.Parameters.AddWithValue("@ReportSID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ReportSID));
            cmd.Parameters.AddWithValue("@DocName", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.DocName));
            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        public static string DeleteDataItem(SqlConnection cnPortal, SQM_QualityFile DataItem)
        {
            return DeleteDataItem(cnPortal, DataItem, "", "");
        }
        public static string DeleteDataItem(SqlConnection cnPortal, SQM_QualityFile DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            string r = "";
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.ReportSID))
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
        private static string DeleteDataItemSub(SqlCommand cmd, SQM_QualityFile DataItem)
        {
            string sErrMsg = "";

            string sSQL = "Delete TB_SQM_Quality_Doc_File Where ReportSID = @ReportSID  and DocNo=@DocNo;";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@ReportSID", DataItem.ReportSID);
            cmd.Parameters.AddWithValue("@DocNo", DataItem.DocNo);
            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        public static string GetDataToJQGridJson(SqlConnection cn, string SearchText, string ReportSID)
        {
            if (ReportSID == "")
            {
                ReportSID = new Guid().ToString();
            }
            SQM_QualityFile_jQGridJSon m = new SQM_QualityFile_jQGridJSon();
            string sSearchText = SearchText.Trim();
            string sWhereClause = "";
            if (sSearchText != "")
                sWhereClause += " and DocName like '%' + @DocName + '%'";


            m.Rows.Clear();
            int iRowCount = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
SELECT  ReportSID
      ,DocName
      ,DocInspResult
      ,DocNo
      ,FGuid
  FROM TB_SQM_Quality_Doc_File
where ReportSID=@ReportSID
           ");
            sb.Append(sWhereClause + ";");
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                //if (sSearchText != "")
                cmd.Parameters.Add(new SqlParameter("@ReportSID", ReportSID));
                if (sSearchText != "")
                    cmd.Parameters.Add(new SqlParameter("@DocName", SearchText));
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    iRowCount++;
                    m.Rows.Add(new SQM_QualityFile(
                        dr["ReportSID"].ToString(),
                        dr["DocName"].ToString(),
                        dr["DocInspResult"].ToString(),
                        dr["DocNo"].ToString(),
                        dr["FGuid"].ToString()

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

        public static string UploadFile(SqlConnection cn, PortalUserProfile RunAsUser, FileAttachmentInfo FA, string sLocalPath, string sLocalUploadPath, HttpServerUtilityBase Server, string RequestApplicationPath)
        {

            String r = "";
            String col = "";
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
            try { tran.Commit(); }
            catch (Exception e) { tran.Rollback(); r = "Upload fail.<br />Exception: " + e.ToString(); }
            //01.del esixt file
            //try
            //{
            //    DataTable dt = new DataTable();
            //    StringBuilder sb = new StringBuilder();
            //    sb.Append(
            //    @"
            //    DECLARE @OldKeyValue uniqueidentifier;

            //    SELECT FGUID FROM TB_SQM_Manufacturers_Reliability_Test_map
            //    WHERE SID = @SID;

            //    UPDATE TB_SQM_Manufacturers_Reliability_Test_map 
            //       SET @file=null;
            //    DELETE FROM TB_SQM_Files
            //    WHERE FGUID =@OldKeyValue
            //    ");
            //    String sql = Regex.Replace(sb.ToString(), @"\s+", " ");
            //    sql = Regex.Replace(sql, @"@col", col);

            //    using (SqlCommand cmd = new SqlCommand(sql, cn, tran))
            //    {
            //        cmd.Parameters.Add(new SqlParameter("@SID", SID));
            //        cmd.ExecuteNonQuery();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    r = ex.ToString();
            //}

            ////02.Update new FGUID
            //try
            //{
            //    StringBuilder sb = new StringBuilder();
            //    sb.Append(
            //    @"
            //    IF NOT EXISTS (SELECT * FROM LiteOnRFQTraining.dbo.TB_SQM_CriticalFile WHERE VendorCode = @VendorCode) 
            //        BEGIN
            //             INSERT INTO LiteOnRFQTraining.dbo.TB_SQM_CriticalFile
            //             (VendorCode) 
            //             VALUES
            //             (@VendorCode) 
            //        END;
            //    UPDATE LiteOnRFQTraining.dbo.TB_SQM_CriticalFile
            //      SET @col = @ColFGUID
            //    WHERE VendorCode = @VendorCode
            //    ");
            //    String sql = Regex.Replace(sb.ToString(), @"\s+", " ");
            //    sql = Regex.Replace(sql, @"@col", col);
            //    using (SqlCommand cmd = new SqlCommand(sql, cn, tran))
            //    {
            //        cmd.Parameters.Add(new SqlParameter("@ColFGUID", FGUID));
            //        cmd.ExecuteNonQuery();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    r += ex.ToString();
            //}

            ////Commit
            //try { tran.Commit(); }
            //catch (Exception e) { tran.Rollback(); r = "Upload fail.<br />Exception: " + e.ToString(); }
            //return r;
            return FGUID;
        }

    }
    #endregion
}
