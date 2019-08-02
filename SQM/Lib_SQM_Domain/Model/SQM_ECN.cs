using Lib_Portal_Domain.Model;
using Lib_SQM_Domain.Modal;
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

namespace Lib_SQM_Domain.Model
{
    public class SQM_ECN
    {
        protected string _SID { get; set; }
        protected string _MemberGUID { get; set; }
        protected string _Supplier { get; set; }
        protected string _Phone { get; set; }
        protected string _OriginatorName { get; set; }
        protected string _Spec { get; set; }
        protected string _Description { get; set; }
        protected string _ChangeItemType { get; set; }
        protected string _ChangeItemTypeSID { get; set; }
        protected string _ChangeType { get; set; }
        protected string _ChangeTypeSID { get; set; }
        protected string _ProposedChange { get; set; }
        protected string _ProposedDate { get; set; }

        protected string _ChangeReason { get; set; }
        protected string _DesignChange { get; set; }
        protected string _Consume { get; set; }
        protected string _Scrap { get; set; }
        protected string _Rework { get; set; }
        protected string _Sort { get; set; }
        protected string _WIP { get; set; }
        protected string _QtyInStock { get; set; }
        protected string _Environment { get; set; }
        protected string _PPAP { get; set; }
        protected string _SupplierApprovalFGUID { get; set; }
        protected string _Title { get; set; }
        protected string _RequestDate { get; set; }
        protected string _ProposedChangeFGUID { get; set; }
        protected string _DesignChangeFGUID { get; set; }

        public string SID { get { return this._SID; } set { this._SID = value; } }
        public string MemberGUID { get { return this._MemberGUID; } set { this._MemberGUID = value; } }
        public string Supplier { get { return this._Supplier; } set { this._Supplier = value; } }
        public string Phone { get { return this._Phone; } set { this._Phone = value; } }
        public string OriginatorName { get { return this._OriginatorName; } set { this._OriginatorName = value; } }
        public string Spec { get { return this._Spec; } set { this._Spec = value; } }
        public string Description { get { return this._Description; } set { this._Description = value; } }
        public string ChangeItemType { get { return this._ChangeItemType; } set { this._ChangeItemType = value; } }
        public string ChangeItemTypeSID { get { return this._ChangeItemTypeSID; } set { this._ChangeItemTypeSID = value; } }
        public string ChangeType { get { return this._ChangeType; } set { this._ChangeType = value; } }
        public string ChangeTypeSID { get { return this._ChangeTypeSID; } set { this._ChangeTypeSID = value; } }
        public string ProposedChange { get { return this._ProposedChange; } set { this._ProposedChange = value; } }
        public string ProposedDate { get { return this._ProposedDate; } set { this._ProposedDate = value; } }
        public string ChangeReason { get { return this._ChangeReason; } set { this._ChangeReason = value; } }
        public string DesignChange { get { return this._DesignChange; } set { this._DesignChange = value; } }
        public string Consume { get { return this._Consume; } set { this._Consume = value; } }
        public string Scrap { get { return this._Scrap; } set { this._Scrap = value; } }
        public string Rework { get { return this._Rework; } set { this._Rework = value; } }
        public string Sort { get { return this._Sort; } set { this._Sort = value; } }
        public string WIP { get { return this._WIP; } set { this._WIP = value; } }

        public string QtyInStock { get { return this._QtyInStock; } set { this._QtyInStock = value; } }
        public string Environment { get { return this._Environment; } set { this._Environment = value; } }
        public string PPAP { get { return this._PPAP; } set { this._PPAP = value; } }
        public string SupplierApprovalFGUID { get { return this._SupplierApprovalFGUID; } set { this._SupplierApprovalFGUID = value; } }
        public string Title { get { return this._Title; } set { this._Title = value; } }
        public string RequestDate { get { return this._RequestDate; } set { this._RequestDate = value; } }
        public string ProposedChangeFGUID { get { return this._ProposedChangeFGUID; } set { this._ProposedChangeFGUID = value; } }
        public string DesignChangeFGUID { get { return this._DesignChangeFGUID; } set { this._DesignChangeFGUID = value; } }
        public SQM_ECN()
        {

        }
        public SQM_ECN(string SID,
            string MemberGUID,
            string Supplier,
            string Phone,
            string OriginatorName,
            string Spec,
            string Description,
            string ChangeItemType,
            string ChangeItemTypeSID,
            string ChangeType,
            string ChangeTypeSID,
            string ProposedChange,
            string ProposedDate,
            string ChangeReason,
            string DesignChange,
            string Consume,
            string Scrap,
            string Rework,
            string Sort,
            string WIP,
            string QtyInStock,
            string Environment,
            string PPAP,
            string SupplierApprovalFGUID,
            string Title,
            string ProposedChangeFGUID,
            string DesignChangeFGUID,
            string RequestDate)
        {
            this._SID = SID;
            this._MemberGUID = MemberGUID;
            this._Supplier = Supplier;
            this._Phone = Phone;
            this._OriginatorName = OriginatorName;
            this._Spec = Spec;
            this._Description = Description;
            this._ChangeItemType = ChangeItemType;
            this._ChangeItemTypeSID = ChangeItemTypeSID;
            this._ChangeType = ChangeType;
            this._ChangeTypeSID = ChangeTypeSID;
            this._ProposedChange = ProposedChange;
            this._ProposedDate = ProposedDate;
            this._ChangeReason = ChangeReason;
            this._DesignChange = DesignChange;
            this._Consume = Consume;
            this._Scrap = Scrap;
            this._Rework = Rework;
            this._Sort = Sort;
            this._WIP = WIP;
            this._QtyInStock = QtyInStock;
            this._Environment = Environment;
            this._PPAP = PPAP;
            this._SupplierApprovalFGUID = SupplierApprovalFGUID;
            this._Title = Title;
            this._RequestDate = RequestDate;
            this._ProposedChangeFGUID = ProposedChangeFGUID;
            this._DesignChangeFGUID = DesignChangeFGUID;

        }
    }
    public class SQM_ECN_jQGridJSon
    {
        public List<SQM_ECN> Rows = new List<SQM_ECN>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }
    public static class SQM_ECN_Helper
    {

        private static void UnescapeDataFromWeb(SQM_ECN DataItem)
        {
            DataItem.Supplier = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Supplier);
            DataItem.Phone = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Phone);
            DataItem.OriginatorName = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.OriginatorName);
            DataItem.Spec = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Spec);
            DataItem.Description = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Description);
            DataItem.Spec = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Spec);
            DataItem.ProposedChange = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ProposedChange);
            DataItem.ChangeReason = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ChangeReason);
            DataItem.Consume = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Consume);
            DataItem.Scrap = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Scrap);
            DataItem.Rework = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Rework);
            DataItem.Sort = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Sort);
            DataItem.WIP = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.WIP);
            DataItem.QtyInStock = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.QtyInStock);
            DataItem.SupplierApprovalFGUID = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.SupplierApprovalFGUID);
            DataItem.Title = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Title);
            DataItem.ProposedChangeFGUID = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ProposedChangeFGUID);
            DataItem.DesignChangeFGUID = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.DesignChangeFGUID);

        }
        private static string DataCheck(SQM_ECN DataItem)
        {
            string r = "";
            List<string> e = new List<string>();
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.Spec))
                e.Add("Must provide Spec.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.Description))
                e.Add("Must provide Description.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.ProposedChange))
                e.Add("Must provide ProposedChange.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.ProposedDate))
                e.Add("Must provide ProposedDate.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.Consume))
                e.Add("Must provide Consume.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.Scrap))
                e.Add("Must provide Scrap.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.Rework))
                e.Add("Must provide Rework.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.Sort))
                e.Add("Must provide Sort.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.WIP))
                e.Add("Must provide WIP.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.QtyInStock))
                e.Add("Must provide QtyInStock.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.SupplierApprovalFGUID))
                e.Add("Must provide SupplierApproval.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.Title))
                e.Add("Must provide Title.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.RequestDate))
                e.Add("Must provide RequestDate.");
            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }
        public static String GetDataToJQGridJson(SqlConnection cn, String SearchText,  String MemberGUID)
        {
            string urlPre = CommonHelper.urlPre;
            string sSearchText = SearchText.Trim();
            string sWhereClause = "";
            if (sSearchText != "")
                sWhereClause += "   TzdNo like '%'+ @SearchText+'%'   ";
            if (sWhereClause.Length != 0)
                sWhereClause = "  AND " + sWhereClause.Substring(0);

            StringBuilder sb = new StringBuilder();
            sb.Append(@"
SELECT C.[SID],
       [TZDNo],
       C.[MemberGUID],
       [Supplier],
       [Phone],
       [OriginatorName],
       [Spec],
       [Description],
       TB_SQM_Supplier_Change_type.SID AS [ChangeTypeSID],
       ChangeName AS [ChangeType],
       TB_SQM_Supplier_ChangeItem_Type.SID AS [ChangeItemTypeSID],
       ChangeItem AS [ChangeItemType],
       [ProposedChange],
       [ProposedDate],
       [ChangeReason],
       [DesignChange],
       [Consume],
       [Scrap],
       [Rework],
       [Sort],
       [WIP],
       [QtyInStock],
       [Environment],
       [PPAP],
       ( '<a href=""" + urlPre + @"/SQMBasic/DownloadSQMFile?DataKey=' + CONVERT( NVARCHAR(50), SupplierApprovalFGUID) + '"">' + T3.FileName + '</a>' ) AS SupplierApproval,
       [Title],
       [RequestDate],
       C.[CaseID],
       A.Status,
       M.[NameInChinese],
       ( '<a href=""" + urlPre + @"/SQMBasic/DownloadSQMFile?DataKey=' + CONVERT( NVARCHAR(50), ProposedChangeFGUID) + '"">' + T1.FileName + '</a>' ) AS ProposedChangeFile,
       ( '<a href=""" + urlPre + @"/SQMBasic/DownloadSQMFile?DataKey=' + CONVERT( NVARCHAR(50), DesignChangeFGUID) + '"">' + T2.FileName + '</a>' ) AS DesignChangeFile
FROM [TB_SQM_Supplier_Change] C
     INNER JOIN TB_SQM_Supplier_Change_type ON TB_SQM_Supplier_Change_type.SID = C.ChangeType
     INNER JOIN TB_SQM_Supplier_ChangeItem_Type ON TB_SQM_Supplier_ChangeItem_Type.SID = C.ChangeItemType
     LEFT OUTER JOIN TB_SQM_Files T1 ON C.ProposedChangeFGUID = T1.FGUID
     LEFT OUTER JOIN TB_SQM_Files T2 ON C.DesignChangeFGUID = T2.FGUID
     LEFT OUTER JOIN TB_SQM_Files T3 ON C.SupplierApprovalFGUID = T3.FGUID
     LEFT OUTER JOIN [dbo].[PORTAL_Members] M ON M.MemberGUID = ( 
                                                                  SELECT FormNo
                                                                  FROM TB_SQM_Approve_Case
                                                                  WHERE CaseID = C.CaseID )
     LEFT OUTER JOIN TB_SQM_Approve_Case A ON C.CaseID = A.CaseID
WHERE 1 = 1 and C.MemberGUID=@MemberGUID;
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

        public static string CreateDataItem(SqlConnection sqlConnection, SQM_ECN DataItem, string memberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            string TzdNo = Assignment(sqlConnection);
            if (r != "")
            {
                return r;
            }
            else
            {

                StringBuilder sb = new StringBuilder();
                sb.Append(@" 
--Supplier
DECLARE @Supplier nvarchar(50) =(
SELECT [NAME] 
FROM [TB_EB_USER]
WHERE [USER_GUID]=@MemberGUID
)
--Phone
DECLARE @Phone nvarchar(50) =(
SELECT TELPHONE 
FROM TB_VMI_VENDOR_DETAIL 
WHERE ERP_VND=(
SELECT VendorCode FROM TB_SQM_Member_Vendor_Map WHERE MemberGUID=@MemberGUID)
)
--OriginatorName
DECLARE @OriginatorName nvarchar(50) =(
SELECT [NameInChinese] FROM [dbo].[PORTAL_Members] WHERE  MemberGUID=@MemberGUID
)
 INSERT INTO [dbo].[TB_SQM_Supplier_Change](
TZDNo,
MemberGUID,
Supplier,
Phone,
OriginatorName,
Spec,
Description,
ChangeItemType,
ChangeType,
ProposedChange,
ProposedDate,
ChangeReason,
DesignChange,
Consume,
Scrap,
Rework,
Sort,
WIP,
QtyInStock,
Environment,
PPAP,
SupplierApprovalFGUID,
Title,
RequestDate,
ProposedChangeFGUID,
DesignChangeFGUID
  )
  VALUES(
@TZDNo,
@MemberGUID,
@Supplier,
@Phone,
@OriginatorName,
@Spec,
@Description,
@ChangeItemType,
@ChangeType,
@ProposedChange,
@ProposedDate,
@ChangeReason,
@DesignChange,
@Consume,
@Scrap,
@Rework,
@Sort,
@WIP,
@QtyInStock,
@Environment,
@PPAP,
@SupplierApprovalFGUID,
@Title,
@RequestDate,
@ProposedChangeFGUID,
@DesignChangeFGUID
  )
");
                //SQM_Basic_Helper.InsertPart(sb, "3");
                SqlCommand cmd = new SqlCommand(sb.ToString(), sqlConnection);
                cmd.Parameters.AddWithValue("@MemberGUID", memberGUID);
                cmd.Parameters.AddWithValue("@TZDNo", TzdNo);
                cmd.Parameters.AddWithValue("@Spec", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Spec));
                cmd.Parameters.AddWithValue("@Description", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Description));
                cmd.Parameters.AddWithValue("@ChangeItemType", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ChangeItemType));
                cmd.Parameters.AddWithValue("@ChangeType", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ChangeType));
                cmd.Parameters.AddWithValue("@ProposedChange", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ProposedChange));
                cmd.Parameters.AddWithValue("@ProposedDate", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ProposedDate));
                cmd.Parameters.AddWithValue("@ChangeReason", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ChangeReason));
                cmd.Parameters.AddWithValue("@DesignChange", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.DesignChange));
                cmd.Parameters.AddWithValue("@Consume", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Consume));
                cmd.Parameters.AddWithValue("@Scrap", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Scrap));
                cmd.Parameters.AddWithValue("@Rework", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Rework));
                cmd.Parameters.AddWithValue("@Sort", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Sort));
                cmd.Parameters.AddWithValue("@WIP", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.WIP));
                cmd.Parameters.AddWithValue("@QtyInStock", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.QtyInStock));
                cmd.Parameters.AddWithValue("@Environment", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Environment));
                cmd.Parameters.AddWithValue("@RequestDate", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.RequestDate));
                cmd.Parameters.AddWithValue("@PPAP", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.PPAP));
                cmd.Parameters.AddWithValue("@SupplierApprovalFGUID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SupplierApprovalFGUID));
                cmd.Parameters.AddWithValue("@Title", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Title));
                cmd.Parameters.AddWithValue("@ProposedChangeFGUID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ProposedChangeFGUID));
                cmd.Parameters.AddWithValue("@DesignChangeFGUID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.DesignChangeFGUID));

                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;

                return sErrMsg;
            }
        }


        public static string EditDataItem(SqlConnection cnPortal, SQM_ECN DataItem)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            if (r != "")
            {
                return r;
            }
            if (DataItem.ProposedChangeFGUID != "")
            {
                using (SqlCommand cmd = new SqlCommand("", cnPortal)) { r = EditProposedChangeFGUID(cmd, DataItem); }
                if (r != "") { return r; }
            }
            if (DataItem.DesignChangeFGUID != "")
            {
                using (SqlCommand cmd = new SqlCommand("", cnPortal)) { r = EditDesignChangeFGUID(cmd, DataItem); }
                if (r != "") { return r; }
            }
            using (SqlCommand cmd = new SqlCommand("", cnPortal)) { r = EditDataItemSub(cmd, DataItem); }
            if (r != "") { return r; }
            return r;
        }
        private static string EditDataItemSub(SqlCommand cmd, SQM_ECN DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"

UPDATE [dbo].[TB_SQM_Supplier_Change]
SET  
Spec=@Spec,
Description=@Description,
ChangeItemType=@ChangeItemType,
ChangeType=@ChangeType,
ProposedChange=@ProposedChange,
ProposedDate=@ProposedDate,
ChangeReason=@ChangeReason,
DesignChange=@DesignChange,
Consume=@Consume,
Scrap=@Scrap,
Rework=@Rework,
Sort=@Sort,
WIP=@WIP,
QtyInStock=@QtyInStock,
Environment=@Environment,
PPAP=@PPAP,
SupplierApprovalFGUID=@SupplierApprovalFGUID,
Title=@Title,
RequestDate=@RequestDate

WHERE SID=@SID

");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@SID", DataItem.SID);
            cmd.Parameters.AddWithValue("@Spec", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Spec));
            cmd.Parameters.AddWithValue("@Description", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Description));
            cmd.Parameters.AddWithValue("@ChangeItemType", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ChangeItemType));
            cmd.Parameters.AddWithValue("@ChangeType", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ChangeType));
            cmd.Parameters.AddWithValue("@ProposedChange", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ProposedChange));
            cmd.Parameters.AddWithValue("@ProposedDate", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ProposedDate));
            cmd.Parameters.AddWithValue("@ChangeReason", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ChangeReason));
            cmd.Parameters.AddWithValue("@DesignChange", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.DesignChange));
            cmd.Parameters.AddWithValue("@Consume", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Consume));
            cmd.Parameters.AddWithValue("@Scrap", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Scrap));
            cmd.Parameters.AddWithValue("@Rework", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Rework));
            cmd.Parameters.AddWithValue("@Sort", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Sort));
            cmd.Parameters.AddWithValue("@WIP", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.WIP));
            cmd.Parameters.AddWithValue("@QtyInStock", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.QtyInStock));
            cmd.Parameters.AddWithValue("@Environment", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Environment));
            cmd.Parameters.AddWithValue("@PPAP", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.PPAP));
            cmd.Parameters.AddWithValue("@SupplierApprovalFGUID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SupplierApprovalFGUID));
            cmd.Parameters.AddWithValue("@Title", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Title));
            cmd.Parameters.AddWithValue("@RequestDate", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.RequestDate));

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        private static string EditProposedChangeFGUID(SqlCommand cmd, SQM_ECN DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"

UPDATE [dbo].[TB_SQM_Supplier_Change]
SET  
ProposedChangeFGUID=@ProposedChangeFGUID,

WHERE SID=@SID

");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@SID", DataItem.SID);
            cmd.Parameters.AddWithValue("@ProposedChangeFGUID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ProposedChangeFGUID));

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        private static string EditDesignChangeFGUID(SqlCommand cmd, SQM_ECN DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"

UPDATE [dbo].[TB_SQM_Supplier_Change]
SET  
DesignChangeFGUID=@DesignChangeFGUID,

WHERE SID=@SID

");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@SID", DataItem.SID);
            cmd.Parameters.AddWithValue("@DesignChangeFGUID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.DesignChangeFGUID));

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        public static string DeleteDataItem(SqlConnection cnPortal, SQM_ECN DataItem, string memberGUID1, string memberGUID2)
        {
            string r = "";
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.SID))
                return "Must provide SID.";
            else
            {
                using (SqlCommand cmd = new SqlCommand("", cnPortal)) { r = DeleteDataItemSub(cmd, DataItem); }
                if (r != "") { return r; }
                return r;
            }
        }
        private static string DeleteDataItemSub(SqlCommand cmd, SQM_ECN DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
DELETE [TB_SQM_Supplier_Change] WHERE SID=@SID;
");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@SID", DataItem.SID);
           
            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }

        public static string Assignment(SqlConnection cnPortal)
        {
            string LastNumStr = getLastNumStr(cnPortal); ;
            string number0 = "";
            DateTime date = System.DateTime.Now;
            string Year = date.Year.ToString();
            string month = date.Month.ToString();
            if (month.Length < 2)
                month = '0' + month;

            string ymd = Year + month;
            if (LastNumStr.Length < 9 || LastNumStr.Substring(0, 6) != ymd)
            {
                return ymd + "001";
            }
            else
            {

                int clientnumber = Convert.ToInt32(LastNumStr.Substring(5, LastNumStr.Length - 6)) + 1;
                if (clientnumber.ToString().Length > LastNumStr.Length - 6)
                {
                    return ymd + clientnumber.ToString();
                }
                else
                {
                    for (int i = 0; i < LastNumStr.Length - 6 - clientnumber.ToString().Length; i++)
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
            sb.Append("Select Max(TZDNo) from TB_SQM_Supplier_Change");
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
        public static String GetChangeTypeList(SqlConnection cn, PortalUserProfile RunAsUser)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT [SID] ,[ChangeName] FROM [TB_SQM_Supplier_Change_type];");

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
        public static String GetChangeItemTypeList(SqlConnection cn, PortalUserProfile RunAsUser)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT [SID] ,[ChangeItem] FROM [TB_SQM_Supplier_ChangeItem_Type];");

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
        public static string UpdateCaseId(SqlConnection cn, SQM_ECN DataItem, string caseID)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
Update [TB_SQM_Supplier_Change]
Set [CaseID]=@CaseID
Where [MemberGUID]=@MemberGUID AND [SID]=@SID
                       ");

            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.AddWithValue("@CaseID", SQMStringHelper.NullOrEmptyStringIsDBNull(caseID));

                cmd.Parameters.AddWithValue("@SID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SID));
                cmd.Parameters.AddWithValue("@MemberGUID",SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.MemberGUID));


                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }
            }
            return sErrMsg;
        }
        public static string UploadFile(SqlConnection cn, PortalUserProfile RunAsUser, FileAttachmentInfo FA, string sLocalPath, string sLocalUploadPath, HttpServerUtilityBase Server, string RequestApplicationPath)
        {
            string r = string.Empty;
            JArray ja = JArray.Parse(FA.SPEC);
            dynamic jo_item = (JObject)ja[0];

            //00.UploadFileToDB
            SqlTransaction tran = cn.BeginTransaction();
            String file = sLocalUploadPath + FA.SUBFOLDER + "/" + jo_item.FileName;
            String FGUID = SharedLibs.SqlFileStreamHelper.InsertToTableSQM(cn, tran, RunAsUser.MemberGUID, file);
            if (FGUID == "")
            {
                tran.Dispose();
                return "fail";
            }
            try { tran.Commit(); }
            catch (Exception e) { tran.Rollback(); r = "Upload fail.<br />Exception: " + e.ToString(); }
            if (r == "")
            {
                return FGUID;
            }
            return "fail";
        }

        public static string GetECNFilesDetail(SqlConnection cn, PortalUserProfile RunAsUser, String FGUID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(
            @"
SELECT   [FGUID]
        ,[FileName]
        ,[FileContent]
        ,[UpdateTime]
        ,[UpdateUser]
        ,[ValidDate]
        ,[SignDate]
FROM [TB_SQM_Files]
WHERE [FGUID]=@FGUID;
            ");
            String sql = Regex.Replace(sb.ToString(), @"\s+", " ");

            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.Add(new SqlParameter("@FGUID", FGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }
    }

}
