using Lib_Portal_Domain.SharedLibs;
using Lib_SQM_Domain.SharedLibs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using Lib_Portal_Domain.Model;
using Lib_VMI_Domain.Model;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Configuration;
using System.Data.OracleClient;
using Lib_SQM_Domain.Modal;

namespace Lib_SQM_Domain.Model
{
    #region SCAR SQE D1
    public class SQMScarMgmt
    {
        protected string _SID;
        protected string _scarNo;
        protected string _initiator;
        protected string _anomalousTime;
        protected string _LitNo;
        protected string _model;
        protected string _BitNo;
        protected string _BitNum;
        protected string _badnessNum;
        protected string _rejectRatio;
        protected string _Abnormal;
        protected string _VenderCode;
        protected string _badnessNote;
        protected string _badnessPic;


        public string SID { get { return this._SID; } set { this._SID = value; } }
        public string scarNo { get { return this._scarNo; } set { this._scarNo = value; } }
        public string initiator { get { return this._initiator; } set { this._initiator = value; } }
        public string anomalousTime { get { return this._anomalousTime; } set { this._anomalousTime = value; } }
        public string LitNo { get { return this._LitNo; } set { this._LitNo = value; } }
        public string model { get { return this._model; } set { this._model = value; } }
        public string BitNo { get { return this._BitNo; } set { this._BitNo = value; } }
        public string BitNum { get { return this._BitNum; } set { this._BitNum = value; } }
        public string badnessNum { get { return this._badnessNum; } set { this._badnessNum = value; } }
        public string rejectRatio { get { return this._rejectRatio; } set { this._rejectRatio = value; } }
        public string Abnormal { get { return this._Abnormal; } set { this._Abnormal = value; } }
        public string VenderCode { get { return this._VenderCode; } set { this._VenderCode = value; } }
        public string badnessNote { get { return this._badnessNote; } set { this._badnessNote = value; } }
        public string badnessPic { get { return this._badnessPic; } set { this._badnessPic = value; } }
        public SQMScarMgmt() { }
        public SQMScarMgmt(string SID,
            string scarNo,
            string initiator,
            string anomalousTime,
            string LitNo,
            string model,
            string BitNo,
            string BitNum,
            string badnessNum,
            string rejectRatio,
            string Abnormal,
            string VenderCode,
            string badnessNote,
            string badnessPic
            )
        {
            this._SID = SID;
            this._scarNo = scarNo;
            this._initiator = initiator;
            this._anomalousTime = anomalousTime;
            this._LitNo = LitNo;
            this._model = model;
            this._BitNo = BitNo;
            this._BitNum = BitNum;
            this._badnessNum = badnessNum;
            this._rejectRatio = rejectRatio;
            this._Abnormal = Abnormal;
            this._VenderCode = VenderCode;
            this._badnessNote = badnessNote;
            this._badnessPic = badnessPic;
        }
    }
    public class SQMScarDataMgmt
    {
        protected string _SID;
        protected string _IsDuty;
        protected string _DutyNote;
        protected string _Timmer;
        protected string _TimmerPhone;
        protected string _GroupMember;
        protected string _Provisional;
        protected string _InventoryNum1;
        protected string _DisposeType1;
        protected string _OverTime1;
        protected string _InventoryNum2;
        protected string _DisposeType2;
        protected string _OverTime2;
        protected string _InventoryNum3;
        protected string _DisposeType3;
        protected string _OverTime3;
        protected string _ReasonAnalysis;
        protected string _ImprovementStrategy;
        protected string _Duty1;
        protected string _FinishDate1;
        protected string _ProductionTime1;
        protected string _ProductionNo1;
        protected string _ProductionNum1;
        protected string _ProductionTime2;
        protected string _ProductionNo2;
        protected string _ProductionNum2;
        protected string _ProductionTime3;
        protected string _ProductionNo3;
        protected string _ProductionNum3;
        protected string _IsValid3;
        protected string _IsValid1;
        protected string _IsValid2;
        protected string _Isperfect1;
        protected string _LitList1;
        protected string _Isperfect2;
        protected string _LitList2;
        protected string _StrategyNote;
        protected string _Duty2;
        protected string _FinishDate2;
        protected string _FileNo;
        protected string _Commissioningdate;
        protected string _Productionworkorder;
        protected string _Productionquantity;
        protected string _InputNum;
        protected string _STDSLRR;
        protected string _rejectOrder;
        protected string _Isvalid;
        protected string _Isover;
        protected string _Note;

        public string SID { get { return this._SID; } set { this._SID = value; } }
        public string IsDuty { get { return this._IsDuty; } set { this._IsDuty = value; } }
        public string DutyNote { get { return this._DutyNote; } set { this._DutyNote = value; } }
        public string Timmer { get { return this._Timmer; } set { this._Timmer = value; } }
        public string TimmerPhone { get { return this._TimmerPhone; } set { this._TimmerPhone = value; } }
        public string GroupMember { get { return this._GroupMember; } set { this._GroupMember = value; } }
        public string Provisional { get { return this._Provisional; } set { this._Provisional = value; } }
        public string InventoryNum1 { get { return this._InventoryNum1; } set { this._InventoryNum1 = value; } }
        public string DisposeType1 { get { return this._DisposeType1; } set { this._DisposeType1 = value; } }
        public string OverTime1 { get { return this._OverTime1; } set { this._OverTime1 = value; } }
        public string InventoryNum2 { get { return this._InventoryNum2; } set { this._InventoryNum2 = value; } }
        public string DisposeType2 { get { return this._DisposeType2; } set { this._DisposeType2 = value; } }
        public string OverTime2 { get { return this._OverTime2; } set { this._OverTime2 = value; } }
        public string InventoryNum3 { get { return this._InventoryNum3; } set { this._InventoryNum3 = value; } }
        public string DisposeType3 { get { return this._DisposeType3; } set { this._DisposeType3 = value; } }
        public string OverTime3 { get { return this._OverTime3; } set { this._OverTime3 = value; } }
        public string ReasonAnalysis { get { return this._ReasonAnalysis; } set { this._ReasonAnalysis = value; } }
        public string ImprovementStrategy { get { return this._ImprovementStrategy; } set { this._ImprovementStrategy = value; } }
        public string Duty1 { get { return this._Duty1; } set { this._Duty1 = value; } }
        public string FinishDate1 { get { return this._FinishDate1; } set { this._FinishDate1 = value; } }
        public string ProductionTime1 { get { return this._ProductionTime1; } set { this._ProductionTime1 = value; } }
        public string ProductionNo1 { get { return this._ProductionNo1; } set { this._ProductionNo1 = value; } }
        public string ProductionNum1 { get { return this._ProductionNum1; } set { this._ProductionNum1 = value; } }
        public string ProductionTime2 { get { return this._ProductionTime2; } set { this._ProductionTime2 = value; } }
        public string ProductionNo2 { get { return this._ProductionNo2; } set { this._ProductionNo2 = value; } }
        public string ProductionNum2 { get { return this._ProductionNum2; } set { this._ProductionNum2 = value; } }
        public string ProductionTime3 { get { return this._ProductionTime3; } set { this._ProductionTime3 = value; } }
        public string ProductionNo3 { get { return this._ProductionNo3; } set { this._ProductionNo3 = value; } }
        public string ProductionNum3 { get { return this._ProductionNum3; } set { this._ProductionNum3 = value; } }
        public string IsValid3 { get { return this._IsValid3; } set { this._IsValid3 = value; } }
        public string IsValid1 { get { return this._IsValid1; } set { this._IsValid1 = value; } }
        public string IsValid2 { get { return this._IsValid2; } set { this._IsValid2 = value; } }
        public string Isperfect1 { get { return this._Isperfect1; } set { this._Isperfect1 = value; } }
        public string LitList1 { get { return this._LitList1; } set { this._LitList1 = value; } }
        public string Isperfect2 { get { return this._Isperfect2; } set { this._Isperfect2 = value; } }
        public string LitList2 { get { return this._LitList2; } set { this._LitList2 = value; } }
        public string StrategyNote { get { return this._StrategyNote; } set { this._StrategyNote = value; } }
        public string Duty2 { get { return this._Duty2; } set { this._Duty2 = value; } }
        public string FinishDate2 { get { return this._FinishDate2; } set { this._FinishDate2 = value; } }
        public string FileNo { get { return this._FileNo; } set { this._FileNo = value; } }
        public string Commissioningdate { get { return this._Commissioningdate; } set { this._Commissioningdate = value; } }
        public string Productionworkorder { get { return this._Productionworkorder; } set { this._Productionworkorder = value; } }
        public string Productionquantity { get { return this._Productionquantity; } set { this._Productionquantity = value; } }
        public string InputNum { get { return this._InputNum; } set { this._InputNum = value; } }
        public string STDSLRR { get { return this._STDSLRR; } set { this._STDSLRR = value; } }
        public string rejectOrder { get { return this._rejectOrder; } set { this._rejectOrder = value; } }
        public string Isvalid { get { return this._Isvalid; } set { this._Isvalid = value; } }
        public string Isover { get { return this._Isover; } set { this._Isover = value; } }
        public string Note { get { return this._Note; } set { this._Note = value; } }

        public SQMScarDataMgmt()
        {

        }
        public SQMScarDataMgmt(
           string SID,
           string IsDuty,
           string DutyNote,
           string Timmer,
           string TimmerPhone,
           string GroupMember,
           string Provisional,
           string InventoryNum1,
           string DisposeType1,
           string OverTime1,
           string InventoryNum2,
           string DisposeType2,
           string OverTime2,
           string InventoryNum3,
           string DisposeType3,
           string OverTime3,
           string ReasonAnalysis,
           string ImprovementStrategy,
           string Duty1,
           string FinishDate1,
           string ProductionTime1,
           string ProductionNo1,
           string ProductionNum1,
           string ProductionTime2,
           string ProductionNo2,
           string ProductionNum2,
           string ProductionTime3,
           string ProductionNo3,
           string ProductionNum3,
           string IsValid3,
           string IsValid1,
           string IsValid2,
           string Isperfect1,
           string LitList1,
           string Isperfect2,
           string LitList2,
           string StrategyNote,
           string Duty2,
           string FinishDate2,
           string FileNo,
           string Commissioningdate,
           string Productionworkorder,
           string Productionquantity,
           string InputNum,
           string STDSLRR,
           string rejectOrder,
           string Isvalid,
           string Isover,
           string Note
            )
        {
            this._SID = SID;
            this._IsDuty = IsDuty;
            this._DutyNote = DutyNote;
            this._Timmer = Timmer;
            this._TimmerPhone = TimmerPhone;
            this._GroupMember = GroupMember;
            this._Provisional = Provisional;
            this._InventoryNum1 = InventoryNum1;
            this._DisposeType1 = DisposeType1;
            this._OverTime1 = OverTime1;
            this._InventoryNum2 = InventoryNum2;
            this._DisposeType2 = DisposeType2;
            this._OverTime2 = OverTime2;
            this._InventoryNum3 = InventoryNum3;
            this._DisposeType3 = DisposeType3;
            this._OverTime3 = OverTime3;
            this._ReasonAnalysis = ReasonAnalysis;
            this._ImprovementStrategy = ImprovementStrategy;
            this._Duty1 = Duty1;
            this._FinishDate1 = FinishDate1;
            this._ProductionTime1 = ProductionTime1;
            this._ProductionNo1 = ProductionNo1;
            this._ProductionNum1 = ProductionNum1;
            this._ProductionTime2 = ProductionTime2;
            this._ProductionNo2 = ProductionNo2;
            this._ProductionNum2 = ProductionNum2;
            this._ProductionTime3 = ProductionTime3;
            this._ProductionNo3 = ProductionNo3;
            this._ProductionNum3 = ProductionNum3;
            this._IsValid3 = IsValid3;
            this._IsValid1 = IsValid1;
            this._IsValid2 = IsValid2;
            this._Isperfect1 = Isperfect1;
            this._LitList1 = LitList1;
            this._Isperfect2 = Isperfect2;
            this._LitList2 = LitList2;
            this._StrategyNote = StrategyNote;
            this._Duty2 = Duty2;
            this._FinishDate2 = FinishDate2;
            this._FileNo = FileNo;
            this._Commissioningdate = Commissioningdate;
            this._Productionworkorder = Productionworkorder;
            this._Productionquantity = Productionquantity;
            this._InputNum = InputNum;
            this._STDSLRR = STDSLRR;
            this._rejectOrder = rejectOrder;
            this._Isvalid = Isvalid;
            this._Isover = Isover;
            this._Note = Note;
        }
    }


    public class SQMScarMgmt_jQGridJSon
    {
        public List<SQMScarMgmt> Rows = new List<SQMScarMgmt>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }


    public static class SQMScarMgmt_Helper
    {


        public static string GetDataToJQGridJson(SqlConnection cn, string MemberGUID)
        {
            return GetDataToJQGridJson(cn, "", MemberGUID);
        }

        public static string GetDataToJQGridJson(SqlConnection cn, string SearchText, string MemberGUID)
        {
            SQMScarMgmt_jQGridJSon m = new SQMScarMgmt_jQGridJSon();
            string urlPre = CommonHelper.urlPre;
            string sSearchText = SearchText.Trim();
            string sWhereClause = "";
            if (sSearchText != "")
                sWhereClause += " and ScarNo like '%' + @SearchText + '%'";


            m.Rows.Clear();
            int iRowCount = 0;
            StringBuilder sSQL = new StringBuilder();
            sSQL.Append(@"

             SELECT  [SID], [ScarNo], PORTAL_Members.NameInChinese as Initiator,
[AnomalousTime],[LitNo],[Model],[BitNo],[BitNum],[BadnessNum],[RejectRatio],[Abnormal],
[VenderCode],[BadnessNote],
case when isover='0' then '正常結案'
when isover='1' then '條件結案'
when isover='2' then '不同意結案'
end BadnessPic 
FROM [TB_SQM_SCAR_REPORT] 
inner join PORTAL_Members on PORTAL_Members.MemberGUID=[TB_SQM_SCAR_REPORT].[Initiator] 
LEFT OUTER JOIN TB_SQM_Files T1 ON TB_SQM_SCAR_REPORT.BadnessPic = T1.FGUID
where VenderCode IN  ( SELECT [VendorCode]
  FROM [dbo].[TB_SQM_Member_Vendor_Map]
  where MemberGUID=@MemberGUID)"
);
            string ssSQL = sSQL.ToString() + sWhereClause + ";";
            using (SqlCommand cmd = new SqlCommand(ssSQL, cn))
            {
                if (sSearchText != "")
                    cmd.Parameters.Add(new SqlParameter("@SearchText", sSearchText));
                cmd.Parameters.Add(new SqlParameter("@MemberGUID", MemberGUID));
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    iRowCount++;
                    string Abnormal = string.Empty;
                    switch (dr["Abnormal"].ToString())
                    {
                        case "0":
                            Abnormal = "進料";
                            break;
                        case "1":
                            Abnormal = "製程";
                            break;
                        case "2":
                            Abnormal = "OQC";
                            break;
                        case "3":
                            Abnormal = "客訴";
                            break;
                        case "4":
                            Abnormal = "可靠測試";
                            break;
                        default:
                            break;
                    }

                    m.Rows.Add(new SQMScarMgmt(
                        dr["SID"].ToString(),
                        dr["ScarNo"].ToString(),
                        dr["Initiator"].ToString(),
                        Convert.ToDateTime(dr["AnomalousTime"].ToString()).ToString("yyyy/MM/dd"),
                        dr["LitNo"].ToString(),
                        dr["Model"].ToString(),
                        dr["BitNo"].ToString(),
                        dr["BitNum"].ToString(),
                        dr["BadnessNum"].ToString(),
                        dr["RejectRatio"].ToString(),

                        Abnormal,
                        dr["VenderCode"].ToString(),
                        dr["BadnessNote"].ToString(),
                        dr["BadnessPic"].ToString()
                        ));
                }
                dr.Close();
                dr = null;
            }

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(m);
        }
        public static string GetSQEDataToJQGridJson(SqlConnection cn, string SearchText, string MemberGUID)
        {
            SQMScarMgmt_jQGridJSon m = new SQMScarMgmt_jQGridJSon();

            string sSearchText = SearchText.Trim();
            string sWhereClause = "";
            if (sSearchText != "")
                sWhereClause += " and ScarNo like '%' + @SearchText + '%'";


            m.Rows.Clear();
            int iRowCount = 0;
            StringBuilder sSQL = new StringBuilder();
            sSQL.Append(@"
  SELECT  [SID], [ScarNo], PORTAL_Members.NameInChinese as Initiator,
[AnomalousTime],[LitNo],[Model],[BitNo],[BitNum],[BadnessNum],[RejectRatio],[Abnormal],
[VenderCode],[BadnessNote],case when isover='0' then '正常結案'
when isover='1' then '條件結案'
when isover='2' then '不同意結案'
end BadnessPic
FROM [TB_SQM_SCAR_REPORT] 
inner join PORTAL_Members on PORTAL_Members.MemberGUID=[TB_SQM_SCAR_REPORT].[Initiator] 
where Initiator = @Initiator"
);
            string ssSQL = sSQL.ToString() + sWhereClause + ";";
            using (SqlCommand cmd = new SqlCommand(ssSQL, cn))
            {
                if (sSearchText != "")
                    cmd.Parameters.Add(new SqlParameter("@SearchText", sSearchText));
                cmd.Parameters.Add(new SqlParameter("@Initiator", MemberGUID));
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    iRowCount++;
                    string Abnormal = string.Empty;
                    switch (dr["Abnormal"].ToString())
                    {
                        case "0":
                            Abnormal = "進料";
                            break;
                        case "1":
                            Abnormal = "製程";
                            break;
                        case "2":
                            Abnormal = "OQC";
                            break;
                        case "3":
                            Abnormal = "客訴";
                            break;
                        case "4":
                            Abnormal = "可靠測試";
                            break;
                        default:
                            break;
                    }

                    m.Rows.Add(new SQMScarMgmt(
                        dr["SID"].ToString(),
                        dr["ScarNo"].ToString(),
                        dr["Initiator"].ToString(),
                        Convert.ToDateTime(dr["AnomalousTime"].ToString()).ToString("yyyy/MM/dd"),
                        dr["LitNo"].ToString(),
                        dr["Model"].ToString(),
                        dr["BitNo"].ToString(),
                        dr["BitNum"].ToString(),
                        dr["BadnessNum"].ToString(),
                        dr["RejectRatio"].ToString(),

                        Abnormal,
                        dr["VenderCode"].ToString(),
                        dr["BadnessNote"].ToString(),
                        dr["BadnessPic"].ToString()
                        ));
                }
                dr.Close();
                dr = null;
            }

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(m);
        }
        public static string UpdateSQMSCARData(SqlConnection cn, SQMScarDataMgmt DataItem, string urlPre)
        {
            UnescapeDataFromWeb(DataItem);
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
   UPDATE [dbo].[TB_SQM_SCAR_REPORT]
   SET
  [IsDuty] = @IsDuty
      ,[DutyNote] = @DutyNote
      ,[Timmer] = @Timmer
      ,[TimmerPhone] = @TimmerPhone
      ,[GroupMember] = @GroupMember
      ,[Provisional] = @Provisional
      ,[InventoryNum1] = @InventoryNum1
      ,[DisposeType1] = @DisposeType1
      ,[OverTime1] = @OverTime1
      ,[InventoryNum2] = @InventoryNum2
      ,[DisposeType2] = @DisposeType2
      ,[OverTime2] = @OverTime2
      ,[InventoryNum3] = @InventoryNum3
      ,[DisposeType3] = @DisposeType3
      ,[OverTime3] = @OverTime3
      ,[ReasonAnalysis] = @ReasonAnalysis
      ,[ImprovementStrategy] = @ImprovementStrategy
      ,[Duty1] = @Duty1
      ,[FinishDate1] = @FinishDate1
      ,[ProductionTime1] = @ProductionTime1
      ,[ProductionNo1] = @ProductionNo1
      ,[ProductionNum1] = @ProductionNum1
      ,[ProductionTime2] = @ProductionTime2
      ,[ProductionNo2] = @ProductionNo2
      ,[ProductionNum2] = @ProductionNum2
      ,[ProductionTime3] = @ProductionTime3
      ,[ProductionNo3] = @ProductionNo3
      ,[ProductionNum3] = @ProductionNum3
      ,[IsValid3] = @IsValid3
      ,[IsValid1] = @IsValid1
      ,[IsValid2] = @IsValid2
      ,[Isperfect1] = @Isperfect1
      ,[LitList1] = @LitList1
      ,[Isperfect2] = @Isperfect2
      ,[LitList2] = @LitList2
      ,[StrategyNote] = @StrategyNote
      ,[Duty2] = @Duty2
      ,[FinishDate2] = @FinishDate2
      ,[FileNo] = @FileNo
      ,STDSLRR=@STDSLRR
      ,[Isover] = @Isover
      ,[note] = @note
 WHERE SID = @SID");
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.AddWithValue("@IsDuty", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.IsDuty));
                cmd.Parameters.AddWithValue("@DutyNote", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.DutyNote));
                cmd.Parameters.AddWithValue("@Timmer", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Timmer));
                cmd.Parameters.AddWithValue("@TimmerPhone", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TimmerPhone));
                cmd.Parameters.AddWithValue("@GroupMember", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.GroupMember));
                cmd.Parameters.AddWithValue("@Provisional", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Provisional));
                cmd.Parameters.AddWithValue("@InventoryNum1", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.InventoryNum1));
                cmd.Parameters.AddWithValue("@DisposeType1", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.DisposeType1));
                cmd.Parameters.AddWithValue("@OverTime1", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.OverTime1));
                cmd.Parameters.AddWithValue("@InventoryNum2", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.InventoryNum2));
                cmd.Parameters.AddWithValue("@DisposeType2", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.DisposeType2));
                cmd.Parameters.AddWithValue("@OverTime2", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.OverTime2));
                cmd.Parameters.AddWithValue("@InventoryNum3", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.InventoryNum3));
                cmd.Parameters.AddWithValue("@DisposeType3", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.DisposeType3));
                cmd.Parameters.AddWithValue("@OverTime3", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.OverTime3));
                cmd.Parameters.AddWithValue("@ReasonAnalysis", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ReasonAnalysis));
                cmd.Parameters.AddWithValue("@ImprovementStrategy", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ImprovementStrategy));
                cmd.Parameters.AddWithValue("@Duty1", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Duty1));
                cmd.Parameters.AddWithValue("@FinishDate1", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.FinishDate1));
                cmd.Parameters.AddWithValue("@ProductionTime1", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ProductionTime1));
                cmd.Parameters.AddWithValue("@ProductionNo1", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ProductionNo1));
                cmd.Parameters.AddWithValue("@ProductionNum1", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ProductionNum1));
                cmd.Parameters.AddWithValue("@ProductionTime2", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ProductionTime2));
                cmd.Parameters.AddWithValue("@ProductionNo2", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ProductionNo2));
                cmd.Parameters.AddWithValue("@IsValid3", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.IsValid3));
                cmd.Parameters.AddWithValue("@ProductionNum2", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ProductionNum2));
                cmd.Parameters.AddWithValue("@ProductionTime3", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ProductionTime3));
                cmd.Parameters.AddWithValue("@ProductionNo3", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ProductionNo3));
                cmd.Parameters.AddWithValue("@ProductionNum3", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ProductionNum3));
                cmd.Parameters.AddWithValue("@IsValid1", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.IsValid1));
                cmd.Parameters.AddWithValue("@IsValid2", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.IsValid2));
                cmd.Parameters.AddWithValue("@Isperfect1", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Isperfect1));
                cmd.Parameters.AddWithValue("@LitList1", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.LitList1));
                cmd.Parameters.AddWithValue("@Isperfect2", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Isperfect2));
                cmd.Parameters.AddWithValue("@LitList2", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.LitList2));
                cmd.Parameters.AddWithValue("@StrategyNote", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.StrategyNote));
                cmd.Parameters.AddWithValue("@Duty2", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Duty2));
                cmd.Parameters.AddWithValue("@FinishDate2", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.FinishDate2));
                cmd.Parameters.AddWithValue("@FileNo", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.FileNo));
                cmd.Parameters.AddWithValue("@STDSLRR", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.STDSLRR));

                cmd.Parameters.AddWithValue("@Isover", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Isover));
                cmd.Parameters.AddWithValue("@note", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Note));
                cmd.Parameters.AddWithValue("@SID", DataItem.SID);
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }
            }
            if (string.IsNullOrEmpty(sErrMsg))
            {
                string Email = SelectEmailbySid(cn, DataItem.SID);
                string ScarNo = SelectScar(cn, DataItem.SID);
                string info = string.Format(
                        "SCAR单"+ ScarNo + "已经更新.请进行审核.网址:<a href='" + urlPre + "'>登錄網址</a>"
                      );
                SendEmail(info, urlPre, Email);
            }


            return sErrMsg;
        }

        private static string SelectScar(SqlConnection cn, string SID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
                select ScarNo from TB_SQM_SCAR_REPORT where SID=@SID
            ");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.AddWithValue("@SID", SQMStringHelper.NullOrEmptyStringIsDBNull(SID));
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

        private static string SelectEmailbySid(SqlConnection cn, string SID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
    SELECT [PrimaryEmail]
  FROM 
 PORTAL_Members 
  where MemberGUID=(select Initiator from TB_SQM_SCAR_REPORT where SID=@SID)
            ");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.AddWithValue("@SID", SQMStringHelper.NullOrEmptyStringIsDBNull(SID));
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

        private static void UnescapeDataFromWeb(SQMScarDataMgmt DataItem)
        {

            DataItem.SID = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.SID);
            DataItem.IsDuty = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.IsDuty);
            DataItem.DutyNote = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.DutyNote);
            DataItem.Timmer = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Timmer);
            DataItem.TimmerPhone = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.TimmerPhone);
            DataItem.GroupMember = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.GroupMember);
            DataItem.Provisional = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Provisional);
            DataItem.InventoryNum1 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.InventoryNum1);
            DataItem.DisposeType1 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.DisposeType1);
            DataItem.OverTime1 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.OverTime1);
            DataItem.InventoryNum2 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.InventoryNum2);
            DataItem.DisposeType2 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.DisposeType2);
            DataItem.OverTime2 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.OverTime2);
            DataItem.InventoryNum3 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.InventoryNum3);
            DataItem.DisposeType3 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.DisposeType3);
            DataItem.OverTime3 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.OverTime3);
            DataItem.ReasonAnalysis = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ReasonAnalysis);
            DataItem.ImprovementStrategy = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ImprovementStrategy);
            DataItem.Duty1 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Duty1);
            DataItem.FinishDate1 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.FinishDate1);
            DataItem.ProductionTime1 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ProductionTime1);
            DataItem.ProductionNo1 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ProductionNo1);
            DataItem.ProductionNum1 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ProductionNum1);
            DataItem.ProductionTime2 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ProductionTime2);
            DataItem.ProductionNo2 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ProductionNo2);
            DataItem.ProductionNum2 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ProductionNum2);
            DataItem.ProductionTime3 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ProductionTime3);
            DataItem.ProductionNo3 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ProductionNo3);
            DataItem.ProductionNum3 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ProductionNum3);
            DataItem.IsValid3 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.IsValid3);
            DataItem.IsValid1 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.IsValid1);
            DataItem.IsValid2 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.IsValid2);
            DataItem.Isperfect1 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Isperfect1);
            DataItem.LitList1 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.LitList1);
            DataItem.Isperfect2 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Isperfect2);
            DataItem.LitList2 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.LitList2);
            DataItem.StrategyNote = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.StrategyNote);
            DataItem.Duty2 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Duty2);
            DataItem.FinishDate2 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.FinishDate2);
            DataItem.FileNo = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.FileNo);
            DataItem.Isover = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Isover);
            DataItem.Note = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Note);
        }
        private static void UnescapeDataFromWeb(SQMScarMgmt DataItem)
        {

            DataItem.anomalousTime = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.anomalousTime);
            DataItem.LitNo = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.LitNo);
            DataItem.model = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.model);
            DataItem.BitNo = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.BitNo);
            DataItem.BitNum = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.BitNum);
            DataItem.badnessNum = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.badnessNum);
            DataItem.rejectRatio = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.rejectRatio);
            DataItem.Abnormal = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Abnormal);
            DataItem.badnessNote = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.badnessNote);
            DataItem.badnessPic = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.badnessPic);
            DataItem.VenderCode = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.VenderCode);
        }
        public static string GetSCarFilesDetail(SqlConnection cn, PortalUserProfile RunAsUser, String FGUID)
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

        private static string DataCheck(SQMScarMgmt DataItem)
        {
            string r = "";
            List<string> e = new List<string>();

            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.anomalousTime))
                e.Add("Must provide anomalousTime.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.LitNo))
                e.Add("Must provide LitNo.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.model))
                e.Add("Must provide model.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.BitNo))
                e.Add("Must provide BitNo.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.BitNum))
                e.Add("Must provide BitNum.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.badnessNum))
                e.Add("Must provide badnessNum.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.rejectRatio))
                e.Add("Must provide rejectRatio.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.Abnormal))
                e.Add("Must provide Abnormal.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.badnessNote))
                e.Add("Must provide badnessNote.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.badnessPic))
                e.Add("Must provide badnessPic.");

            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }



        public static string CreateDataItem(SqlConnection cnPortal, SQMScarMgmt DataItem, string MemberGUID, string localPath, string urlPre)
        {
            UnescapeDataFromWeb(DataItem);
            DataItem.rejectRatio = Convert.ToDouble(DataItem.badnessNum) / Convert.ToDouble(DataItem.BitNum) <= 0.01 ? (Convert.ToDouble(DataItem.badnessNum) / Convert.ToDouble(DataItem.BitNum) * 1000000).ToString() + " DPPM" :( (Convert.ToDouble(DataItem.badnessNum) / Convert.ToDouble(DataItem.BitNum))*100).ToString()+"%";
            string r = DataCheck(DataItem);
            if (r != "")
            { return r; }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"
INSERT INTO [dbo].[TB_SQM_SCAR_REPORT]
           ([ScarNo]
           
           ,[Initiator]
           ,[AnomalousTime]
           ,[LitNo]
           ,[Model]
           ,[BitNo]
           ,[BitNum]
           ,[BadnessNum]
           ,[RejectRatio]
           ,[Abnormal]
           ,[VenderCode]
           ,[BadnessNote]
           ,[BadnessPic]
           )
     VALUES
           (@ScarNo
          
           ,@Initiator
           ,@AnomalousTime
           ,@LitNo
           ,@Model
           ,@BitNo
           ,@BitNum
           ,@BadnessNum
           ,@RejectRatio
           ,@Abnormal
           ,@VenderCode
           ,@BadnessNote
           ,@BadnessPic
          )");
                SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal);
                cmd.Parameters.AddWithValue("@ScarNo", Assignment(cnPortal));
                cmd.Parameters.AddWithValue("@Initiator", SQMStringHelper.NullOrEmptyStringIsDBNull(MemberGUID));
                cmd.Parameters.AddWithValue("@AnomalousTime", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.anomalousTime));
                cmd.Parameters.AddWithValue("@LitNo", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.LitNo));
                cmd.Parameters.AddWithValue("@Model", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.model));
                cmd.Parameters.AddWithValue("@BitNo", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.BitNo));
                cmd.Parameters.AddWithValue("@BitNum", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.BitNum));
                cmd.Parameters.AddWithValue("@BadnessNum", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.badnessNum));
                cmd.Parameters.AddWithValue("@RejectRatio", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.rejectRatio));
                cmd.Parameters.AddWithValue("@Abnormal", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Abnormal));
                cmd.Parameters.AddWithValue("@VenderCode", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.VenderCode));
                cmd.Parameters.AddWithValue("@BadnessNote", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.badnessNote));
                cmd.Parameters.AddWithValue("@BadnessPic", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.badnessPic));

                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;
                if (string.IsNullOrEmpty(sErrMsg))
                {
                    string Email = SelectEmail(cnPortal, DataItem.VenderCode);
                    string info = string.Format(
                            "请登录光宝供应商品质管理系统平台,查看SCAR单.网址:<a href='" + urlPre + "'>登錄網址</a>"
                          );
                    SendEmail(info, urlPre, Email);
                }
                return sErrMsg;
            }
        }

        private static void SendEmail(string info, string urlPre, string Email)
        {
            icm045.CMSHandler MS = new icm045.CMSHandler();
            MS.MailSend("SupplierPortal",
                          "SupplierPortal@liteon.com",
                          Email,
                          //== "" ? "Jerrya.Chen@liteon.com" : sMailTo
                          "",
                          "",
                          "SQM Notice",
                         info,
                          icm045.MailPriority.Normal,
                          icm045.MailFormat.Html,
                          new string[0]);
        }

        private static string SelectEmail(SqlConnection cnPortal, string VenderCode)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
  SELECT [PrimaryEmail]
  FROM TB_SQM_Member_Vendor_Map
  inner join PORTAL_Members on PORTAL_Members.MemberGUID=TB_SQM_Member_Vendor_Map.MemberGUID
  where VendorCode=@VenderCode
            ");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal))
            {
                cmd.Parameters.AddWithValue("@VenderCode", SQMStringHelper.NullOrEmptyStringIsDBNull(VenderCode));
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


        public static string EditDataItem(SqlConnection cnPortal, SQMScarMgmt DataItem)
        {
            return EditDataItem(cnPortal, DataItem, "", "");
        }

        public static string EditDataItem(SqlConnection cnPortal, SQMScarMgmt DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            if (r != "")
            { return r; }
            else
            {
                //SqlTransaction tran = cnPortal.BeginTransaction();

                //Update member data
                using (SqlCommand cmd = new SqlCommand("", cnPortal)) { r = EditDataItemSub(cmd, DataItem); }

                return r;
            }
        }

        private static string EditDataItemSub(SqlCommand cmd, SQMScarMgmt DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
   UPDATE [dbo].[TB_SQM_SCAR_REPORT]
   SET
      [AnomalousTime] = @AnomalousTime
      ,[LitNo] = @LitNo
      ,[Model] = @Model
      ,[BitNo] = @BitNo
      ,[BitNum] = @BitNum
      ,[BadnessNum] = @BadnessNum
      ,[RejectRatio] = @RejectRatio
      ,[Abnormal] = @Abnormal
      ,[VenderCode] = @VenderCode
      ,[BadnessNote] = @BadnessNote
      ,[BadnessPic] = @BadnessPic
 WHERE SID = @SID");


            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@AnomalousTime", StringHelper.NullOrEmptyStringIsDBNull(DataItem.anomalousTime));
            cmd.Parameters.AddWithValue("@LitNo", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.LitNo));
            cmd.Parameters.AddWithValue("@Model", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.model));
            cmd.Parameters.AddWithValue("@BitNo", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.BitNo));
            cmd.Parameters.AddWithValue("@BitNum", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.BitNum));
            cmd.Parameters.AddWithValue("@BadnessNum", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.badnessNum));
            cmd.Parameters.AddWithValue("@RejectRatio", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.rejectRatio));
            cmd.Parameters.AddWithValue("@Abnormal", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Abnormal));
            cmd.Parameters.AddWithValue("@VenderCode", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.VenderCode));
            cmd.Parameters.AddWithValue("@BadnessNote", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.badnessNote));
            cmd.Parameters.AddWithValue("@BadnessPic", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.badnessPic));

            cmd.Parameters.AddWithValue("@SID", DataItem.SID);
            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }

        public static string DeleteDataItem(SqlConnection cnPortal, SQMScarMgmt DataItem)
        {
            return DeleteDataItem(cnPortal, DataItem, "", "");
        }

        public static string DeleteDataItem(SqlConnection cnPortal, SQMScarMgmt DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            string r = "";
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.SID))
                return "Must provide Account SID.";
            else
            {
                using (SqlCommand cmd = new SqlCommand("", cnPortal)) { r = DeleteDataItemSub(cmd, DataItem); }
                if (r != "") { return r; }

                return r;
            }
        }

        private static string DeleteDataItemSub(SqlCommand cmd, SQMScarMgmt DataItem)
        {
            string sErrMsg = "";

            string sSQL = "Delete TB_SQM_SCAR_REPORT Where SID = @SID;";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@SID", DataItem.SID);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        //生成流水號
        public static string Assignment(SqlConnection cnPortal)
        {
            string LastNumStr = getLastNumStr(cnPortal); ;
            string number0 = "";
            DateTime date = System.DateTime.Now;


            if (LastNumStr.Length < 6 || LastNumStr.Substring(0, 2) != "YY")
            {
                return "YY" + "0001";
            }
            else
            {
                int clientnumber = Convert.ToInt32(LastNumStr.Substring(2, LastNumStr.Length - 2)) + 1;
                if (clientnumber.ToString().Length > LastNumStr.Length - 2)
                {
                    return "YY" + clientnumber.ToString();
                }
                else
                {
                    for (int i = 0; i < LastNumStr.Length - 2 - clientnumber.ToString().Length; i++)
                    {
                        number0 += "0";
                    }
                    return "YY" + number0 + clientnumber.ToString();
                }
            }
        }
        private static string getLastNumStr(SqlConnection cn)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select Max(ScarNo) from TB_SQM_SCAR_REPORT");
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

        public static string GetSQMSCARData(SqlConnection cn, string SID)
        {
            string urlPre = CommonHelper.urlPre;
            StringBuilder sb = new StringBuilder();
            sb.Append(
            @"
SELECT [TB_SQM_SCAR_REPORT].[SID]
      ,[ScarNo]
      , PORTAL_Members.NameInChinese as Initiator
      ,[AnomalousTime]
      ,[LitNo]
      ,[Model]
      ,[BitNo]
      ,[BitNum]
      ,[BadnessNum]
      ,[RejectRatio]
      ,[Abnormal]
      ,[VenderCode]
      ,[BadnessNote]
      ,( '<a href=""" + urlPre + @"/SQMBasic/DownloadSQMFile?DataKey=' + CONVERT( NVARCHAR(50), BadnessPic) + '"">' + T1.FileName + '</a>' )  as BadnessPic
      ,[IsDuty]
      ,[DutyNote]
      ,[Timmer]
      ,[TimmerPhone]
      ,[GroupMember]
      ,[Provisional]
      ,[InventoryNum1]
      ,[DisposeType1]
      ,[OverTime1]
      ,[InventoryNum2]
      ,[DisposeType2]
      ,[OverTime2]
      ,[InventoryNum3]
      ,[DisposeType3]
      ,[OverTime3]
      ,[ReasonAnalysis]
      ,[ImprovementStrategy]
      ,[Duty1]
      ,[FinishDate1]
      ,[ProductionTime1]
      ,[ProductionNo1]
      ,[ProductionNum1]
      ,[ProductionTime2]
      ,[ProductionNo2]
      ,[ProductionNum2]
      ,[ProductionTime3]
      ,[ProductionNo3]
      ,[ProductionNum3]
      ,[IsValid3]
      ,[IsValid1]
      ,[IsValid2]
      ,[Isperfect1]
      ,[LitList1]
      ,[Isperfect2]
      ,[LitList2]
      ,[StrategyNote]
      ,[Duty2]
      ,[FinishDate2]
      ,[FileNo]
    ,[commissioningdate]
      ,[Productionworkorder]
      ,[Productionquantity]
      ,[InputNum]
      ,[STDSLRR]
      ,[rejectOrder]
      ,[Isvalid]
      ,[Isover]
      ,[note]
	  ,[appovestatus1]
	  ,[appovestatus2]
  FROM [dbo].[TB_SQM_SCAR_REPORT]
inner join PORTAL_Members on PORTAL_Members.MemberGUID=[TB_SQM_SCAR_REPORT].[Initiator] 
LEFT OUTER JOIN TB_SQM_Files T1 ON TB_SQM_SCAR_REPORT.BadnessPic = T1.FGUID
left join [dbo].[TB_SQM_SCAR_REPORT_APPOVE] on [TB_SQM_SCAR_REPORT].sid=[TB_SQM_SCAR_REPORT_APPOVE].SSID
where TB_SQM_SCAR_REPORT.SID=@SID;
            ");

            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@SID", SID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }

        public static string Appove(SqlConnection cn, string SID, string Type, string localPath,string urlPre)
        {
            string sErrMsg = "";
           DataTable dtinfo= getInfobySID(cn, SID);
            string Email = SelectEmail(cn, dtinfo.Rows[0][1].ToString());
            string ScarNo = dtinfo.Rows[0][0].ToString();
            string info = string.Empty;
            StringBuilder sb = new StringBuilder();

            if (checkAppove(cn, SID).Rows.Count > 0)
            {
                switch (Type)
                {
                    case "1":
                        sb.Append(@"UPDATE [dbo].[TB_SQM_SCAR_REPORT_APPOVE]
   SET 
      [appovestatus1] = 0
 WHERE SSID=@SSID");
                        info = string.Format(
                     "单号"+ ScarNo + "已经审核通过，请登录光宝供应商品质管理系统平台查看.网址:<a href='" + urlPre + "'>登錄網址</a>"
                  );
                        break;
                    case "2":
                        sb.Append(@"
UPDATE [dbo].[TB_SQM_SCAR_REPORT_APPOVE]
   SET 
      [appovestatus2] = 0
 WHERE SSID=@SSID");
                        info = string.Format(
                     "单号" + ScarNo + "已经审核通过，请登录光宝供应商品质管理系统平台查看.网址:<a href='" + urlPre + "'>登錄網址</a>"
                );
                        break;
                    case "3":
                        sb.Append(@"
UPDATE [dbo].[TB_SQM_SCAR_REPORT_APPOVE]
   SET 
      [appovestatus1] =1
 WHERE SSID=@SSID");
                        info = string.Format(
                    "单号" + ScarNo + "审核未通过，请登录光宝供应商品质管理系统平台查看.网址:<a href='" + urlPre + "'>登錄網址</a>"
                );
                        break;
                    case "4":
                        sb.Append(@"
UPDATE [dbo].[TB_SQM_SCAR_REPORT_APPOVE]
   SET 
      [appovestatus2] = 1
 WHERE SSID=@SSID");
                        info = string.Format(
              "单号" + ScarNo + "审核未通过，请登录光宝供应商品质管理系统平台查看.网址:<a href='" + urlPre + "'>登錄網址</a>"
           );
                        break;
                    case "5":
                        sb.Append(@"
UPDATE [dbo].[TB_SQM_SCAR_REPORT_APPOVE]
   SET 
      [appovestatus3] =0
 WHERE SSID=@SSID");
                        info = string.Format(
                  "单号" + ScarNo + "已经审核通过，请登录光宝供应商品质管理系统平台查看.网址:<a href='" + urlPre + "'>登錄網址</a>"
                );
                        break;
                    case "6":
                        sb.Append(@"
UPDATE [dbo].[TB_SQM_SCAR_REPORT_APPOVE]
   SET 
      [appovestatus3] = 1
 WHERE SSID=@SSID");
                        info = string.Format(
                   "单号" + ScarNo + "审核未通过，请登录光宝供应商品质管理系统平台查看.网址:<a href='" + urlPre + "'>登錄網址</a>"
                );
                        break;

                }

            }
            else
            {

                switch (Type)
                {
                    case "1":
                        sb.Append(@"INSERT INTO [dbo].[TB_SQM_SCAR_REPORT_APPOVE]
           ([SSID]
           ,[appovestatus1]
          )
     VALUES
           (@SSID 
           ,0
           
		   )");
                        info = string.Format(
                   "单号" + ScarNo + "已经审核通过，请登录光宝供应商品质管理系统平台查看.网址:<a href='" + urlPre + "'>登錄網址</a>"
                );
                        break;
                    case "2":
                        sb.Append(@"INSERT INTO [dbo].[TB_SQM_SCAR_REPORT_APPOVE]
           ([SSID]
           
           ,[appovestatus2])
     VALUES
           (@SSID
           ,0
		   )");
                        info = string.Format(
                    "单号" + ScarNo + "已经审核通过，请登录光宝供应商品质管理系统平台查看.网址:<a href='" + urlPre + "'>登錄網址</a>"
                );
                        break;
                    case "3":
                        sb.Append(@"INSERT INTO [dbo].[TB_SQM_SCAR_REPORT_APPOVE]
           ([SSID]
           ,[appovestatus1]
          )
     VALUES
           (@SSID
           ,1
		   )");
                        info = string.Format(
                     "单号" + ScarNo + "审核未通过，请登录光宝供应商品质管理系统平台查看.网址:<a href='" + urlPre + "'>登錄網址</a>"
                );
                        break;
                    case "4":
                        sb.Append(@"INSERT INTO [dbo].[TB_SQM_SCAR_REPORT_APPOVE]
           ([SSID]
           ,[appovestatus2])
     VALUES
           (@SSID 
           ,1
		   )");
                        info = string.Format(
                  "单号" + ScarNo + "审核未通过，请登录光宝供应商品质管理系统平台查看.网址:<a href='" + urlPre + "'>登錄網址</a>"
                );
                        break;
                    case "5":
                        sb.Append(@"INSERT INTO [dbo].[TB_SQM_SCAR_REPORT_APPOVE]
           ([SSID]
           ,[appovestatus3]
          )
     VALUES
           (@SSID
           ,0
		   )");
                        info = string.Format(
                 "单号" + ScarNo + "已经审核通过，请登录光宝供应商品质管理系统平台查看.网址:<a href='" + urlPre + "'>登錄網址</a>"
                );
                        break;
                    case "6":
                        sb.Append(@"INSERT INTO [dbo].[TB_SQM_SCAR_REPORT_APPOVE]
           ([SSID]
           ,[appovestatus3])
     VALUES
           (@SSID 
           ,1
		   )");
                        info = string.Format(
   "单号" + ScarNo + "审核未通过，请登录光宝供应商品质管理系统平台查看.网址:<a href='" + urlPre + "'>登錄網址</a>"                );
                        break;

                }

            }


            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@SSID", SID));

                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "fail.<br />Exception: " + e.ToString(); }
            }
            if (string.IsNullOrEmpty(sErrMsg))
            {
                SendEmail(info, urlPre, Email);
            }
            return sErrMsg;
        }

        private static DataTable getInfobySID(SqlConnection cn, string SID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
SELECT
      [ScarNo]
      ,[VenderCode]
  FROM [dbo].[TB_SQM_SCAR_REPORT]
 where SID=@SID");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@SID", SID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return dt;
        }

        private static DataTable checkAppove(SqlConnection cn, string SID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT [SID]
      ,[SSID]
      ,[appovestatus1]
      ,[appovestatus2]
  FROM [TB_SQM_SCAR_REPORT_APPOVE]
 where SSID=@SSID");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@SSID", SID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return dt;
        }

        public static string GetScarD8(SqlConnection cn, string SID, string LitNo, string DateCode,string urlPre)
        {
            string sErrMsg = "";
            string MESconnectionString = ConfigurationManager.ConnectionStrings["MESconnectionString"].ConnectionString;
            OracleConnection oc = new OracleConnection(MESconnectionString);
            oc.Open();
            OracleCommand ocm = oc.CreateCommand();
            ocm.CommandType = CommandType.StoredProcedure;
            ocm.CommandText = "IQC_SQM";
            ocm.Parameters.Add("TPART", OracleType.NVarChar, 255).Direction = ParameterDirection.Input;
            ocm.Parameters["TPART"].Value = LitNo;
            ocm.Parameters.Add("TDATECODE", OracleType.NVarChar, 255).Direction = ParameterDirection.Input;
            ocm.Parameters["TDATECODE"].Value = DateCode;
            ocm.Parameters.Add("TRES", OracleType.NVarChar, 255).Direction = ParameterDirection.Output;
            ocm.Parameters["TRES"].Value = "0";
            ocm.Parameters.Add("PDC", OracleType.NVarChar, 255).Direction = ParameterDirection.Output;
            ocm.Parameters["PDC"].Value = "0";
            ocm.Parameters.Add("PWO", OracleType.NVarChar, 255).Direction = ParameterDirection.Output;
            ocm.Parameters["PWO"].Value = "0";
            ocm.Parameters.Add("PQTY", OracleType.NVarChar, 255).Direction = ParameterDirection.Output;
            ocm.Parameters["PQTY"].Value = "0";
            ocm.Parameters.Add("PSQTY", OracleType.NVarChar, 255).Direction = ParameterDirection.Output;
            ocm.Parameters["PSQTY"].Value = "0";
            ocm.Parameters.Add("PDRATE", OracleType.NVarChar, 255).Direction = ParameterDirection.Output;
            ocm.Parameters["PDRATE"].Value = "0";

            ocm.ExecuteNonQuery();
            string tres = ocm.Parameters["TRES"].Value.ToString();
            string pdc = ocm.Parameters["PDC"].Value.ToString();
            string pwo = ocm.Parameters["PWO"].Value.ToString();
            string pqty = ocm.Parameters["PQTY"].Value.ToString();
            string psqty = ocm.Parameters["PSQTY"].Value.ToString();
            string pdrate = ocm.Parameters["PDRATE"].Value.ToString();
            oc.Close();
            StringBuilder sb = new StringBuilder();
            sb.Append(@"Update  [dbo].[TB_SQM_SCAR_REPORT]
   SET
      [commissioningdate] = @commissioningdate
      ,[Productionworkorder] = @Productionworkorder
      ,[Productionquantity] = @Productionquantity
      ,[InputNum] = @InputNum  
      ,[rejectOrder] = @rejectOrder
 WHERE SID = @SID
          ");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@commissioningdate", pdc));
                cmd.Parameters.Add(new SqlParameter("@Productionworkorder", pwo));
                cmd.Parameters.Add(new SqlParameter("@Productionquantity", pqty));
                cmd.Parameters.Add(new SqlParameter("@InputNum", psqty));
                cmd.Parameters.Add(new SqlParameter("@rejectOrder", pdrate));

                cmd.Parameters.Add(new SqlParameter("@SID", SID));

                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "fail.<br />Exception: " + e.ToString(); }
            }
            if (string.IsNullOrEmpty(sErrMsg))
            {
                DataTable dtinfo = getInfobySID(cn, SID);
                string Email = SelectEmail(cn, dtinfo.Rows[0][1].ToString());
                string ScarNo = dtinfo.Rows[0][0].ToString();
                string info = string.Format(
                        "请登录光宝供应商品质管理系统平台,查看SCAR单号"+ ScarNo + ".网址:<a href='" + urlPre + "'>登錄網址</a>"
                      );
                SendEmail(info,urlPre,Email);
            }
            return sErrMsg;
        }

        public static string UpdateD8(SqlConnection cn, SQMScarDataMgmt DataItem, string urlPre)
        {
            UnescapeDataFromWeb(DataItem);
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
   UPDATE [dbo].[TB_SQM_SCAR_REPORT]
   SET

     [rejectOrder] = @rejectOrder
      ,[Isover] = @Isover
      ,[note] = @note
 WHERE SID = @SID");
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.AddWithValue("@rejectOrder", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.rejectOrder));
            

                cmd.Parameters.AddWithValue("@Isover", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Isover));
                cmd.Parameters.AddWithValue("@note", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Note));
                cmd.Parameters.AddWithValue("@SID", DataItem.SID);
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }
            }
            if (string.IsNullOrEmpty(sErrMsg))
            {
                string Email = SelectEmailbySid(cn, DataItem.SID);
                string ScarNo = SelectScar(cn, DataItem.SID);
                string info = string.Format(
                        "SCAR单" + ScarNo + "工单信息已经更新.请进行结案.网址:<a href='" + urlPre + "'>登錄網址</a>"
                      );
                SendEmail(info, urlPre, Email);
            }


            return sErrMsg;
        }
    }
    #endregion


}

