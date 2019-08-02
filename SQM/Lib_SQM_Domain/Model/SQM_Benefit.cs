using Lib_Portal_Domain.SharedLibs;
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
    public class SQM_Benefit
    {
        protected string _SID;
        protected string _Plant;
        protected string _BenbfitNo;
        protected string _VendorCode;
        protected string _BenbfitDate;
        protected string _CID;
        protected string _CCID;
        protected string _CNAME;
        protected string _CCNAME;
        protected string _MaterialType;
        protected string _TotolScore;
        protected string _Class;
        protected string _BatchNum1;
        protected string _RejectNum1;
        protected string _RejectRate;
        protected string _TargetValue1;
        protected string _Score1;
        protected string _ProductionNum;
        protected string _DefectQuantity;
        protected string _RejectRatio;
        protected string _TargetValue2;
        protected string _Score2;
        protected string _TotalRecoveryDays;
        protected string _ReplyQuantity;
        protected string _MDO;
        protected string _TargetValue3;
        protected string _Score3;
        protected string _XRF;
        protected string _BatchNum2;
        protected string _RejectNum2;
        protected string _TargetValue4;
        protected string _Score4;
        protected string _A;
        protected string _B;
        protected string _C;
        protected string _D;
        protected string _E;
        protected string _F;
        protected string _G;
        protected string _H;
        protected string _Score5;
        protected string _Score6;
        protected string _Score7;
        protected string _NOTE;
        protected string _Handle;
        protected string _HandleTIME1;
        protected string _HandleTIME2;
        protected string _IQC;
        protected string _SQE;
        protected string _SCOURCER;
        protected string _BUYER;
        protected string _Month1;
        protected string _Day1;
        protected string _Month2;
        protected string _Day2;
        protected string _Hour;
        protected string _Isvalid;

        public string SID { get { return this._SID; } set { this._SID = value; } }
        public string Plant { get { return this._Plant; } set { this._Plant = value; } }
        public string BenbfitNo { get { return this._BenbfitNo; } set { this._BenbfitNo = value; } }
        public string VendorCode { get { return this._VendorCode; } set { this._VendorCode = value; } }
        public string BenbfitDate { get { return this._BenbfitDate; } set { this._BenbfitDate = value; } }
        public string MaterialType { get { return this._MaterialType; } set { this._MaterialType = value; } }
        public string CID { get { return this._CID; } set { this._CID = value; } }
        public string CCID { get { return this._CCID; } set { this._CCID = value; } }
        public string CNAME { get { return this._CNAME; } set { this._CNAME = value; } }
        public string CCNAME { get { return this._CCNAME; } set { this._CCNAME = value; } }
        public string TotolScore { get { return this._TotolScore; } set { this._TotolScore = value; } }
        public string Class { get { return this._Class; } set { this._Class = value; } }
        public string BatchNum1 { get { return this._BatchNum1; } set { this._BatchNum1 = value; } }
        public string RejectNum1 { get { return this._RejectNum1; } set { this._RejectNum1 = value; } }
        public string RejectRate { get { return this._RejectRate; } set { this._RejectRate = value; } }
        public string TargetValue1 { get { return this._TargetValue1; } set { this._TargetValue1 = value; } }
        public string Score1 { get { return this._Score1; } set { this._Score1 = value; } }
        public string ProductionNum { get { return this._ProductionNum; } set { this._ProductionNum = value; } }
        public string DefectQuantity { get { return this._DefectQuantity; } set { this._DefectQuantity = value; } }
        public string RejectRatio { get { return this._RejectRatio; } set { this._RejectRatio = value; } }
        public string TargetValue2 { get { return this._TargetValue2; } set { this._TargetValue2 = value; } }
        public string Score2 { get { return this._Score2; } set { this._Score2 = value; } }
        public string TotalRecoveryDays { get { return this._TotalRecoveryDays; } set { this._TotalRecoveryDays = value; } }
        public string ReplyQuantity { get { return this._ReplyQuantity; } set { this._ReplyQuantity = value; } }
        public string MDO { get { return this._MDO; } set { this._MDO = value; } }
        public string TargetValue3 { get { return this._TargetValue3; } set { this._TargetValue3 = value; } }
        public string Score3 { get { return this._Score3; } set { this._Score3 = value; } }
        public string XRF { get { return this._XRF; } set { this._XRF = value; } }
        public string BatchNum2 { get { return this._BatchNum2; } set { this._BatchNum2 = value; } }
        public string RejectNum2 { get { return this._RejectNum2; } set { this._RejectNum2 = value; } }
        public string TargetValue4 { get { return this._TargetValue4; } set { this._TargetValue4 = value; } }
        public string Score4 { get { return this._Score4; } set { this._Score4 = value; } }
        public string A { get { return this._A; } set { this._A = value; } }
        public string B { get { return this._B; } set { this._B = value; } }
        public string C { get { return this._C; } set { this._C = value; } }
        public string D { get { return this._D; } set { this._D = value; } }
        public string E { get { return this._E; } set { this._E = value; } }
        public string F { get { return this._F; } set { this._F = value; } }
        public string G { get { return this._G; } set { this._G = value; } }
        public string H { get { return this._H; } set { this._H = value; } }
        public string Score5 { get { return this._Score5; } set { this._Score5 = value; } }
        public string Score6 { get { return this._Score6; } set { this._Score6 = value; } }
        public string Score7 { get { return this._Score7; } set { this._Score7 = value; } }
        public string NOTE { get { return this._NOTE; } set { this._NOTE = value; } }
        public string Handle { get { return this._Handle; } set { this._Handle = value; } }
        public string HandleTIME1 { get { return this._HandleTIME1; } set { this._HandleTIME1 = value; } }
        public string HandleTIME2 { get { return this._HandleTIME2; } set { this._HandleTIME2 = value; } }
        public string IQC { get { return this._IQC; } set { this._IQC = value; } }
        public string SQE { get { return this._SQE; } set { this._SQE = value; } }
        public string SCOURCER { get { return this._SCOURCER; } set { this._SCOURCER = value; } }
        public string BUYER { get { return this._BUYER; } set { this._BUYER = value; } }
        public string Month1 { get { return this._Month1; } set { this._Month1 = value; } }
        public string Day1 { get { return this._Day1; } set { this._Day1 = value; } }
        public string Month2 { get { return this._Month2; } set { this._Month2 = value; } }
        public string Day2 { get { return this._Day2; } set { this._Day2 = value; } }
        public string Hour { get { return this._Hour; } set { this._Hour = value; } }
        public string Isvalid { get { return this._Isvalid; } set { this._Isvalid = value; } }
        public SQM_Benefit()
        {

        }
        public SQM_Benefit(string SID
           , string Plant
           , string BenbfitNo
           , string VendorCode
           , string BenbfitDate
           , string CID
           , string CCID
           , string CNAME
           , string CCNAME
           , string TotolScore
           , string Class
           , string BatchNum1
           , string RejectNum1
           , string RejectRate
           , string TargetValue1
           , string Score1
           , string ProductionNum
           , string DefectQuantity
           , string RejectRatio
           , string TargetValue2
           , string Score2
           , string TotalRecoveryDays
           , string ReplyQuantity
           , string MDO
           , string TargetValue3
           , string Score3
           , string XRF
           , string BatchNum2
           , string RejectNum2
           , string TargetValue4
           , string Score4
           , string A
           , string B
           , string C
           , string D
           , string E
           , string F
           , string G
           , string H
           , string Score5
           , string Score6
           , string Score7
           , string NOTE
           , string Handle
           , string HandleTIME1
           , string HandleTIME2
           , string IQC
           , string SQE
           , string SCOURCER
           , string BUYER
            )
        {
            this._SID = SID;
            this._Plant = Plant;
            this._BenbfitNo = BenbfitNo;
            this._VendorCode = VendorCode;
            this._BenbfitDate = BenbfitDate;
            this._CID = CID;
            this._CCID = CCID;
            this._CNAME = CNAME;
            this._CCNAME = CCNAME;
            this._TotolScore = TotolScore;
            this._Class = Class;
            this._BatchNum1 = BatchNum1;
            this._RejectNum1 = RejectNum1;
            this._RejectRate = RejectRate;
            this._TargetValue1 = TargetValue1;
            this._Score1 = Score1;
            this._ProductionNum = ProductionNum;
            this._DefectQuantity = DefectQuantity;
            this._RejectRatio = RejectRatio;
            this._TargetValue2 = TargetValue2;
            this._Score2 = Score2;
            this._TotalRecoveryDays = TotalRecoveryDays;
            this._ReplyQuantity = ReplyQuantity;
            this._MDO = MDO;
            this._TargetValue3 = TargetValue3;
            this._Score3 = Score3;
            this._XRF = XRF;
            this._BatchNum2 = BatchNum2;
            this._RejectNum2 = RejectNum2;
            this._TargetValue4 = TargetValue4;
            this._Score4 = Score4;
            this._A = A;
            this._B = B;
            this._C = C;
            this._D = D;
            this._E = E;
            this._F = F;
            this._G = G;
            this._H = H;
            this._Score5 = Score5;
            this._Score6 = Score6;
            this._Score7 = Score7;
            this._NOTE = NOTE;
            this._Handle = Handle;
            this._HandleTIME1 = HandleTIME1;
            this._HandleTIME2 = HandleTIME2;
            this._IQC = IQC;
            this._SQE = SQE;
            this._SCOURCER = SCOURCER;
            this._BUYER = BUYER;
        }

        public SQM_Benefit(string SID, string Plant, string BenbfitNo, string VendorCode, string BenbfitDate, string TotolScore, string Class)
        {
            this._SID = SID;
            this._Plant = Plant;
            this._BenbfitNo = BenbfitNo;
            this._VendorCode = VendorCode;
            this._BenbfitDate = BenbfitDate;
            this._TotolScore = TotolScore;
            this._Class = Class;
        }
    }

    public class SQM_Benefit_jQGridJSon
    {
        public List<SQM_Benefit> Rows = new List<SQM_Benefit>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }
    public class Benefit_Helper
    {
        private static void UnescapeDataFromWeb(SQM_Benefit DataItem)
        {

            DataItem.SID = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.SID);
            DataItem.Plant = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Plant);
            DataItem.BenbfitNo = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.BenbfitNo);
            DataItem.VendorCode = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.VendorCode);
            DataItem.BenbfitDate = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.BenbfitDate);
            DataItem.MaterialType = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.MaterialType);
            DataItem.CID = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CID);
            DataItem.CCID = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CCID);
            DataItem.CNAME = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CNAME);
            DataItem.CCNAME = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CCNAME);
            DataItem.TotolScore = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.TotolScore);
            DataItem.Class = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Class);
            DataItem.BatchNum1 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.BatchNum1);
            DataItem.RejectNum1 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.RejectNum1);
            DataItem.RejectRate = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.RejectRate);
            DataItem.TargetValue1 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.TargetValue1);
            DataItem.Score1 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Score1);
            DataItem.ProductionNum = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ProductionNum);
            DataItem.DefectQuantity = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.DefectQuantity);
            DataItem.RejectRatio = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.RejectRatio);
            DataItem.TargetValue2 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.TargetValue2);
            DataItem.Score2 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Score2);
            DataItem.TotalRecoveryDays = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.TotalRecoveryDays);
            DataItem.ReplyQuantity = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ReplyQuantity);
            DataItem.MDO = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.MDO);
            DataItem.TargetValue3 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.TargetValue3);
            DataItem.Score3 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Score3);
            DataItem.XRF = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.XRF);
            DataItem.BatchNum2 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.BatchNum2);
            DataItem.RejectNum2 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.RejectNum2);
            DataItem.TargetValue4 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.TargetValue4);
            DataItem.Score4 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Score4);
            DataItem.A = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.A);
            DataItem.B = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.B);
            DataItem.C = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.C);
            DataItem.D = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.D);
            DataItem.E = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.E);
            DataItem.F = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.F);
            DataItem.G = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.G);
            DataItem.H = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.H);
            DataItem.Score5 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Score5);
            DataItem.Score6 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Score6);
            DataItem.Score7 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Score7);
            DataItem.NOTE = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.NOTE);
            DataItem.Handle = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Handle);
            DataItem.HandleTIME1 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.HandleTIME1);
            DataItem.HandleTIME2 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.HandleTIME2);
            DataItem.IQC = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.IQC);
            DataItem.SQE = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.SQE);
            DataItem.SCOURCER = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.SCOURCER);
            DataItem.BUYER = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.BUYER);
        }
        private static string DataCheck(SQM_Benefit DataItem)
        {
            string r = "";
            List<string> e = new List<string>();
            //if (StringHelper.DataIsNullOrEmpty(DataItem.Provider))
            //    e.Add("Must provide Provider.");
            if (StringHelper.DataIsNullOrEmpty(DataItem.VendorCode))
                e.Add("Must provide VendorCode.");

            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }
        public static String GetSQMBenefitData(SqlConnection cn, SQM_Benefit DataItem, string LoginMemberGUID, string RunAsUser)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
SELECT top 1 [SID]
      ,[Date]
      ,[VendorCode]
      ,[MaterialType]
      ,[BatchNum1]
      ,[RejectNum1]
      ,[ProductionNum]
      ,[DefectQuantity]
      ,[XRF]
      ,[BatchNum2]
  FROM [TB_SQM_Benefit_Data]
  where VendorCode=@VendorCode and [Date]=@Date

                ");

            DataTable dt = new DataTable();

            using (SqlConnection cnSPM = new SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["CZSPMDBConnString2"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand(sb.ToString(), cnSPM))
                {
                    cmd.Parameters.Add(new SqlParameter("@VendorCode", DataItem.VendorCode));
                    cmd.Parameters.Add(new SqlParameter("@Date", DataItem.BenbfitDate));
                    cnSPM.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        dt.Load(dr);
                    }
                }
            }
            return JsonConvert.SerializeObject(dt);
        }

        public static String GetSQMAnnual(SqlConnection cn, string MaterialType, string LoginMemberGUID, string RunAsUser)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"select     ");
            int month = DateTime.Now.Month;
            if (1 <= month && month <= 3)
            {
                sb.Append(@" 
(select Q1  from TB_SQM_Annual_Objectives where AType=@AType and MaterialType='SVLRR') as Annual1,
(select Q1  from TB_SQM_Annual_Objectives where AType=@AType and MaterialType='ILRR') as Annual2
");
            }
            if (4 <= month && month <= 6)
            {
                sb.Append(@"
           (select Q2  from TB_SQM_Annual_Objectives where AType=@AType and MaterialType='SVLRR') as Annual1,
          (select Q2 from TB_SQM_Annual_Objectives where AType=@AType and MaterialType='ILRR') as Annual2
              ");
            }
            if (7 <= month && month <= 9)
            {
                sb.Append(@"(select Q3  from TB_SQM_Annual_Objectives where AType=@AType and MaterialType='SVLRR') as Annual1,
(select Q3  from TB_SQM_Annual_Objectives where AType=@AType and MaterialType='ILRR') as Annual2  ");
            }
            if (10 <= month && month <= 12)
            {
                sb.Append(@"(select Q4  from TB_SQM_Annual_Objectives where AType=@AType and MaterialType='SVLRR') as Annual1,
(select Q4  from TB_SQM_Annual_Objectives where AType=@AType and MaterialType='ILRR') as Annual2    ");
            }

            sb.Append(@" from TB_SQM_Annual_Objectives ");
            DataTable dt = new DataTable();



            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@AType", MaterialType));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }

            return JsonConvert.SerializeObject(dt);
        }
        public static string CreateDataItem(SqlConnection cnPortal, SQM_Benefit DataItem, String memberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            string Plant = GetPlant(cnPortal, memberGUID);
            string BenbfitNo = Assignment(cnPortal);
            if (r != "")
            { return r; }
            else
            {
                string sSQL = @"
                  INSERT INTO [dbo].[TB_SQM_BENFIT_REPORT]
           ([Plant]
           ,[BenbfitNo]
           ,[vendorcode]
           ,[benbfitDate]
           ,[CID]
           ,[CCID]
           ,[MaterialType]
           ,[Totolscore]
           ,[class]
           ,[batchNum1]
           ,[rejectNum1]
           ,[rejectrate]
           ,[targetvalue1]
           ,[score1]
           ,[productionNum]
           ,[defectQuantity]
           ,[rejectratio]
           ,[targetvalue2]
           ,[score2]
           ,[TotalRecoveryDays]
           ,[ReplyQuantity]
           ,[MDO]
           ,[targetvalue3]
           ,[score3]
           ,[XRF]
           ,[batchNum2]
           ,[rejectNum2]
           ,[targetvalue4]
           ,[score4]
           ,[A]
           ,[B]
           ,[C]
           ,[D]
           ,[E]
           ,[F]
           ,[G]
           ,[H]
           ,[score5]
           ,[score6]
           ,[score7]
           ,[NOTE]
           ,[handle]
           ,[handleTIME1]
           ,[HANDLETIME2]
           ,[IQC]
           ,[SQE]
           ,[SCOURCER]
           ,[BUYER]
           ,Month1
           ,Day1
           ,Month2
           ,Day2
           ,Hour
           ,Isvalid
)
     VALUES
           (@Plant
           ,@BenbfitNo
           ,@vendorcode
           ,@benbfitDate
           ,@CID
           ,@CCID
           ,@MaterialType
           ,@Totolscore 
           ,@class
           ,@batchNum1
           ,@rejectNum1
           ,@rejectrate
           ,@targetvalue1
           ,@score1
           ,@productionNum
           ,@defectQuantity
           ,@rejectratio
           ,@targetvalue2
           ,@score2
           ,@TotalRecoveryDays
           ,@ReplyQuantity
           ,@MDO
           ,@targetvalue3
           ,@score3 
           ,@XRF
           ,@batchNum2
           ,@rejectNum2
           ,@targetvalue4
           ,@score4 
           ,@A
           ,@B
           ,@C
           ,@D
           ,@E 
           ,@F 
           ,@G 
           ,@H 
           ,@score5 
           ,@score6 
           ,@score7 
           ,@NOTE 
           ,@handle
           ,@handleTIME1
           ,@HANDLETIME2
           ,@IQC
           ,@SQE
           ,@SCOURCER
           ,@BUYER
           ,@Month1
           ,@Day1
           ,@Month2
           ,@Day2
           ,@Hour
           ,@Isvalid
)
               ";
                SqlCommand cmd = new SqlCommand(sSQL, cnPortal);

                cmd.Parameters.AddWithValue("@Plant", SQMStringHelper.NullOrEmptyStringIsDBNull(Plant));
                cmd.Parameters.AddWithValue("@BenbfitNo", SQMStringHelper.NullOrEmptyStringIsDBNull(BenbfitNo));
                cmd.Parameters.AddWithValue("@vendorcode", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.VendorCode));
                cmd.Parameters.AddWithValue("@benbfitDate", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.BenbfitDate));
                cmd.Parameters.AddWithValue("@Totolscore", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TotolScore));
                cmd.Parameters.AddWithValue("@CID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.CID));
                cmd.Parameters.AddWithValue("@CCID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.CCID));
                cmd.Parameters.AddWithValue("@MaterialType", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.MaterialType));                
                cmd.Parameters.AddWithValue("@class", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Class));
                cmd.Parameters.AddWithValue("@batchNum1", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.BatchNum1));
                cmd.Parameters.AddWithValue("@rejectNum1", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.RejectNum1));
                cmd.Parameters.AddWithValue("@rejectrate", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.RejectRate));
                cmd.Parameters.AddWithValue("@targetvalue1", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TargetValue1));
                cmd.Parameters.AddWithValue("@score1", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Score1));
                cmd.Parameters.AddWithValue("@productionNum", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ProductionNum));
                cmd.Parameters.AddWithValue("@defectQuantity", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.DefectQuantity));
                cmd.Parameters.AddWithValue("@rejectratio", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.RejectRatio));
                cmd.Parameters.AddWithValue("@targetvalue2", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TargetValue2));
                cmd.Parameters.AddWithValue("@score2", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Score2));
                cmd.Parameters.AddWithValue("@TotalRecoveryDays", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TotalRecoveryDays));
                cmd.Parameters.AddWithValue("@ReplyQuantity", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ReplyQuantity));
                cmd.Parameters.AddWithValue("@MDO", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.MDO));
                cmd.Parameters.AddWithValue("@targetvalue3", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TargetValue3));
                cmd.Parameters.AddWithValue("@score3", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Score3));
                cmd.Parameters.AddWithValue("@XRF", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.XRF));
                cmd.Parameters.AddWithValue("@batchNum2", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.BatchNum2));
                cmd.Parameters.AddWithValue("@rejectNum2", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.RejectNum2));
                cmd.Parameters.AddWithValue("@targetvalue4", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TargetValue4));
                cmd.Parameters.AddWithValue("@score4", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Score4));
                cmd.Parameters.AddWithValue("@A", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.A));
                cmd.Parameters.AddWithValue("@B", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.B));
                cmd.Parameters.AddWithValue("@C", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.C));
                cmd.Parameters.AddWithValue("@D", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.D));
                cmd.Parameters.AddWithValue("@E", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.E));
                cmd.Parameters.AddWithValue("@F", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.F));
                cmd.Parameters.AddWithValue("@G", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.G));
                cmd.Parameters.AddWithValue("@H", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.H));
                cmd.Parameters.AddWithValue("@score5", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Score5));
                cmd.Parameters.AddWithValue("@score6", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Score6));
                cmd.Parameters.AddWithValue("@score7", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Score7));
                cmd.Parameters.AddWithValue("@NOTE", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.NOTE));
                cmd.Parameters.AddWithValue("@handle", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Handle));
                cmd.Parameters.AddWithValue("@handleTIME1", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.HandleTIME1));
                cmd.Parameters.AddWithValue("@HANDLETIME2", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.HandleTIME2));
                cmd.Parameters.AddWithValue("@IQC", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.IQC));
                cmd.Parameters.AddWithValue("@SQE", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SQE));
                cmd.Parameters.AddWithValue("@SCOURCER", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SCOURCER));
                cmd.Parameters.AddWithValue("@BUYER", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.BUYER));
                cmd.Parameters.AddWithValue("@Month1", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Month1));
                cmd.Parameters.AddWithValue("@Day1", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Day1));
                cmd.Parameters.AddWithValue("@Month2", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Month2));
                cmd.Parameters.AddWithValue("@Day2", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Day2));
                cmd.Parameters.AddWithValue("@Hour", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Hour));
                cmd.Parameters.AddWithValue("@Isvalid", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Isvalid));
                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;

                return sErrMsg;
            }
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
            sb.Append("Select Max(BenbfitNo) from TB_SQM_BENFIT_REPORT");
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

        public static string EditDataItem(SqlConnection cnPortal, SQM_Benefit DataItem)
        {
            return EditDataItem(cnPortal, DataItem, "", "");
        }
        public static string EditDataItem(SqlConnection cnPortal, SQM_Benefit DataItem, string LoginMemberGUID, string RunAsMemberGUID)
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

        private static string EditDataItemSub(SqlCommand cmd, SQM_Benefit DataItem)
        {
            string sErrMsg = "";

            StringBuilder sSQL = new StringBuilder();
            sSQL.Append(@"Update TB_SQM_BENFIT_REPORT Set  
Totolscore=@Totolscore,
MaterialType=@MaterialType,
class=@class,
batchNum1=@batchNum1,
rejectNum1=@rejectNum1,
rejectrate=@rejectrate,
targetvalue1=@targetvalue1,
score1=@score1,
productionNum=@productionNum,
defectQuantity=@defectQuantity,
rejectratio=@rejectratio,
targetvalue2=@targetvalue2,
score2=@score2,
TotalRecoveryDays=@TotalRecoveryDays,
ReplyQuantity=@ReplyQuantity,
MDO=@MDO,
targetvalue3=@targetvalue3,
score3=@score3,
XRF=@XRF,
batchNum2=@batchNum2,
rejectNum2=@rejectNum2,
targetvalue4=@targetvalue4,
score4=@score4,
A=@A,
B=@B,
C=@C,
D=@D,
E=@E,
F=@F,
G=@G,
H=@H,
score5=@score5,
score6=@score6,
score7=@score7,
NOTE=@NOTE,
handle=@handle,
handleTIME1=@handleTIME1,
HANDLETIME2=@HANDLETIME2,
IQC=@IQC,
SQE=@SQE,
SCOURCER=@SCOURCER,
BUYER=@BUYER,
Month1=@Month1,
Day1=@Day1,
Month2=@Month2,
Day2=@Day2,
Hour=@Hour,
Isvalid=@Isvalid
            where  SID=@SID");

            cmd.CommandText = sSQL.ToString();

            cmd.Parameters.AddWithValue("@Totolscore", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TotolScore));
            cmd.Parameters.AddWithValue("@MaterialType", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.MaterialType));            
            cmd.Parameters.AddWithValue("@class", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Class));
            cmd.Parameters.AddWithValue("@batchNum1", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.BatchNum1));
            cmd.Parameters.AddWithValue("@rejectNum1", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.RejectNum1));
            cmd.Parameters.AddWithValue("@rejectrate", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.RejectRate));
            cmd.Parameters.AddWithValue("@targetvalue1", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TargetValue1));
            cmd.Parameters.AddWithValue("@score1", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Score1));
            cmd.Parameters.AddWithValue("@productionNum", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ProductionNum));
            cmd.Parameters.AddWithValue("@defectQuantity", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.DefectQuantity));
            cmd.Parameters.AddWithValue("@rejectratio", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.RejectRatio));
            cmd.Parameters.AddWithValue("@targetvalue2", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TargetValue2));
            cmd.Parameters.AddWithValue("@score2", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Score2));
            cmd.Parameters.AddWithValue("@TotalRecoveryDays", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TotalRecoveryDays));
            cmd.Parameters.AddWithValue("@ReplyQuantity", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ReplyQuantity));
            cmd.Parameters.AddWithValue("@MDO", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.MDO));
            cmd.Parameters.AddWithValue("@targetvalue3", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TargetValue3));
            cmd.Parameters.AddWithValue("@score3", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Score3));
            cmd.Parameters.AddWithValue("@XRF", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.XRF));
            cmd.Parameters.AddWithValue("@batchNum2", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.BatchNum2));
            cmd.Parameters.AddWithValue("@rejectNum2", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.RejectNum2));
            cmd.Parameters.AddWithValue("@targetvalue4", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TargetValue4));
            cmd.Parameters.AddWithValue("@score4", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Score4));
            cmd.Parameters.AddWithValue("@A", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.A));
            cmd.Parameters.AddWithValue("@B", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.B));
            cmd.Parameters.AddWithValue("@C", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.C));
            cmd.Parameters.AddWithValue("@D", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.D));
            cmd.Parameters.AddWithValue("@E", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.E));
            cmd.Parameters.AddWithValue("@F", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.F));
            cmd.Parameters.AddWithValue("@G", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.G));
            cmd.Parameters.AddWithValue("@H", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.H));
            cmd.Parameters.AddWithValue("@score5", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Score5));
            cmd.Parameters.AddWithValue("@score6", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Score6));
            cmd.Parameters.AddWithValue("@score7", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Score7));
            cmd.Parameters.AddWithValue("@NOTE", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.NOTE));
            cmd.Parameters.AddWithValue("@handle", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Handle));
            cmd.Parameters.AddWithValue("@handleTIME1", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.HandleTIME1));
            cmd.Parameters.AddWithValue("@HANDLETIME2", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.HandleTIME2));
            cmd.Parameters.AddWithValue("@IQC", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.IQC));
            cmd.Parameters.AddWithValue("@SQE", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SQE));
            cmd.Parameters.AddWithValue("@SCOURCER", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SCOURCER));
            cmd.Parameters.AddWithValue("@BUYER", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.BUYER));

            cmd.Parameters.AddWithValue("@Month1", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Month1));
            cmd.Parameters.AddWithValue("@Day1", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Day1));
            cmd.Parameters.AddWithValue("@Month2", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Month2));
            cmd.Parameters.AddWithValue("@Day2", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Day2));
            cmd.Parameters.AddWithValue("@Hour", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Hour));
            cmd.Parameters.AddWithValue("@Isvalid", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Isvalid));
      

            cmd.Parameters.AddWithValue("@SID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SID));

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        public static string DeleteDataItem(SqlConnection cnPortal, SQM_Benefit DataItem)
        {
            return DeleteDataItem(cnPortal, DataItem, "", "");
        }
        public static string DeleteDataItem(SqlConnection cnPortal, SQM_Benefit DataItem, string LoginMemberGUID, string RunAsMemberGUID)
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
        private static string DeleteDataItemSub(SqlCommand cmd, SQM_Benefit DataItem)
        {
            string sErrMsg = "";

            string sSQL = "Delete TB_SQM_BENFIT_REPORT Where SID = @SID;";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@SID", DataItem.SID);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        public static string GetDataToJQGridJson(SqlConnection cn, string SearchText, string MemberGUID)
        {
            SQM_Benefit_jQGridJSon m = new SQM_Benefit_jQGridJSon();
            //string sSearchText = SearchText.Trim();
            //string sWhereClause = "";
            //if (sSearchText != "")
            //    sWhereClause += " and SubFuncName like '%' + @SearchText + '%'";
            //if (sWhereClause.Length != 0)
            //    sWhereClause = " Where" + sWhereClause.Substring(4);

            m.Rows.Clear();
            int iRowCount = 0;
            StringBuilder sb = new StringBuilder();
            string Plant = GetPlant(cn, MemberGUID);
            sb.Append(@"
SELECT [SID]
      ,[Plant]
      ,[BenbfitNo]
      ,[vendorcode]
      ,[benbfitDate]
    
      ,[Totolscore]
,class
  FROM [dbo].[TB_SQM_BENFIT_REPORT]
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
                    m.Rows.Add(new SQM_Benefit(
                         dr["SID"].ToString(),
                      dr["Plant"].ToString(),
                      dr["BenbfitNo"].ToString(),
                      dr["vendorcode"].ToString(),
                      dr["benbfitDate"].ToString(),

                       dr["Totolscore"].ToString(),
                       dr["class"].ToString()
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

        public static string GetSQMBenefit(SqlConnection cn, string SID, string MemberGUID)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(
            @"select SID,Plant,BenbfitNo,VendorCode
                     ,BenbfitDate
                     ,MaterialType
                     ,CID
                     ,CCID
                     ,TotolScore
                     ,Class
                     ,BatchNum1
                     ,RejectNum1
                     ,RejectRate
                     ,TargetValue1
                     ,Score1
                     ,ProductionNum
                     ,DefectQuantity
                     ,RejectRatio
                     ,TargetValue2
                     ,Score2
                     ,TotalRecoveryDays
                     ,ReplyQuantity
                     ,MDO
                     ,TargetValue3
                     ,Score3
                     ,XRF
                     ,BatchNum2
                     ,RejectNum2
                     ,TargetValue4
                     ,Score4
                     ,A
                     ,B
                     ,C
                     ,D
                     ,E
                     ,F
                     ,G
                     ,H
                     ,Score5
                     ,Score6
                     ,Score7
                     ,NOTE
                     ,Handle
                     ,HandleTIME1
                     ,HandleTIME2
                     ,IQC
                     ,SQE
                     ,SCOURCER
                     ,BUYER
                     ,[Month1]
      ,[Day1]
      ,[Month2]
      ,[Day2]
      ,[Hour]
      ,[Isvalid]
from TB_SQM_BENFIT_REPORT
where SID=@SID 

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

        public static string ExportMonth(SqlConnection cn,  string MemberGUID,string localPath,string fileName)
        {
            StringBuilder sb = new StringBuilder();
            string plant = GetPlant(cn,MemberGUID);

            DateTime today = DateTime.Now;//获得当前日期
            string year = today.Year.ToString();//获得年份
            string month = string.Empty;
            if (today.Month-1 < 10)
            {
                month = "0" +( today.Month-1);
            }
            else
            {
                month = (today.Month-1).ToString();
            }
            string benbfitDate = (year + month);
            string msg = string.Empty;
            sb.Append(@"
SELECT 
       [benbfitDate] as '月度'
      ,[vendorcode] as '供應商'
      ,[MaterialType] as '供應材料'
      ,[batchNum1] as '進料總批數'
      ,[rejectNum1] as '判退批數'
      ,[rejectrate] as '批退率'
      ,[targetvalue1] as '目標值'
      ,[score1] as '得分'
      ,[productionNum] as  '投產數'
      ,[defectQuantity] as '不良數'
      ,[rejectratio] as '不良率'
      ,[targetvalue2] as '目標值'
      ,[score2] as '得分'
      ,[TotalRecoveryDays] as '回復總天數'
      ,[ReplyQuantity] as '回復件數'
      ,[MDO] as '平均天數'
      ,[targetvalue3] as '目標值'
      ,[score3] as '得分'
      ,[XRF] as 'XRF測試批數'
      ,[batchNum2] as '判退批數'
      ,[rejectNum2] as '批退率'
      ,[targetvalue4] as '目標值'
      ,[score4] as '得分'
      ,[score5]  as '总扣分'
      ,[score6] as '得分'
      ,[score7] as '得分'
	  ,[Totolscore] as '實際得分'
      ,[class] as '等級'
      ,[NOTE] as '問題描述'
      ,Isvalid as '处理方式'
  FROM [dbo].[TB_SQM_BENFIT_REPORT]
where Plant=@Plant and benbfitDate=@benbfitDate

            ");

            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.AddWithValue("@Plant", SQMStringHelper.NullOrEmptyStringIsDBNull(plant));
                cmd.Parameters.AddWithValue("@benbfitDate", SQMStringHelper.NullOrEmptyStringIsDBNull(benbfitDate));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
         Common.ExportExcel(dt, localPath, fileName);
            return msg;
        }
        public static string ExportYear(SqlConnection cn, string MemberGUID,string localPath,string fileName)
        {
            StringBuilder sb = new StringBuilder();
            string plant = GetPlant(cn, MemberGUID);
            string benbfitDate = DateTime.Now.Year.ToString();
            string msg = string.Empty;
            sb.Append(@"
SELECT [vendorcode]
      ,[MaterialType]
      ,(select [Totolscore] from [TB_SQM_BENFIT_REPORT] where benbfitDate=@benbfitDate+'01' and vendorcode=A.vendorcode) AS '1月得分'
      ,(select [class] from [TB_SQM_BENFIT_REPORT] where benbfitDate=@benbfitDate+'01' and vendorcode=A.vendorcode) AS '1月等级'
	  ,(select [Totolscore] from [TB_SQM_BENFIT_REPORT] where benbfitDate=@benbfitDate+'02' and vendorcode=A.vendorcode) AS '2月得分'
      ,(select [class] from [TB_SQM_BENFIT_REPORT] where benbfitDate=@benbfitDate+'02' and vendorcode=A.vendorcode) AS '2月等级'
	  ,(select [Totolscore] from [TB_SQM_BENFIT_REPORT] where benbfitDate=@benbfitDate+'03' and vendorcode=A.vendorcode) AS '3月得分'
      ,(select [class] from [TB_SQM_BENFIT_REPORT] where benbfitDate=@benbfitDate+'03' and vendorcode=A.vendorcode) AS '3月等级'
	  ,(select [Totolscore] from [TB_SQM_BENFIT_REPORT] where benbfitDate=@benbfitDate+'04' and vendorcode=A.vendorcode) AS '4月得分'
      ,(select [class] from [TB_SQM_BENFIT_REPORT] where benbfitDate=@benbfitDate+'04' and vendorcode=A.vendorcode) AS '4月等级'
	  ,(select [Totolscore] from [TB_SQM_BENFIT_REPORT] where benbfitDate=@benbfitDate+'05' and vendorcode=A.vendorcode) AS '5月得分'
      ,(select [class] from [TB_SQM_BENFIT_REPORT] where benbfitDate=@benbfitDate+'05' and vendorcode=A.vendorcode) AS '5月等级'
	  ,(select [Totolscore] from [TB_SQM_BENFIT_REPORT] where benbfitDate=@benbfitDate+'06' and vendorcode=A.vendorcode) AS '6月得分'
      ,(select [class] from [TB_SQM_BENFIT_REPORT] where benbfitDate=@benbfitDate+'06' and vendorcode=A.vendorcode) AS '6月等级'
	  ,(select [Totolscore] from [TB_SQM_BENFIT_REPORT] where benbfitDate=@benbfitDate+'07' and vendorcode=A.vendorcode) AS '7月得分'
      ,(select [class] from [TB_SQM_BENFIT_REPORT] where benbfitDate=@benbfitDate+'07' and vendorcode=A.vendorcode) AS '7月等级'
	  ,(select [Totolscore] from [TB_SQM_BENFIT_REPORT] where benbfitDate=@benbfitDate+'08' and vendorcode=A.vendorcode) AS '8月得分'
      ,(select [class] from [TB_SQM_BENFIT_REPORT] where benbfitDate=@benbfitDate+'08' and vendorcode=A.vendorcode) AS '8月等级'
	  ,(select [Totolscore] from [TB_SQM_BENFIT_REPORT] where benbfitDate=@benbfitDate+'09' and vendorcode=A.vendorcode) AS '9月得分'
      ,(select [class] from [TB_SQM_BENFIT_REPORT] where benbfitDate=@benbfitDate+'09' and vendorcode=A.vendorcode) AS '9月等级'
	  ,(select [Totolscore] from [TB_SQM_BENFIT_REPORT] where benbfitDate=@benbfitDate+'10' and vendorcode=A.vendorcode) AS '10月得分'
      ,(select [class] from [TB_SQM_BENFIT_REPORT] where benbfitDate=@benbfitDate+'10' and vendorcode=A.vendorcode) AS '10月等级'
	  ,(select [Totolscore] from [TB_SQM_BENFIT_REPORT] where benbfitDate=@benbfitDate+'11' and vendorcode=A.vendorcode) AS '11月得分'
      ,(select [class] from [TB_SQM_BENFIT_REPORT] where benbfitDate=@benbfitDate+'11' and vendorcode=A.vendorcode) AS '11月等级'
	  ,(select [Totolscore] from [TB_SQM_BENFIT_REPORT] where benbfitDate=@benbfitDate+'12' and vendorcode=A.vendorcode) AS '12月得分'
      ,(select [class] from [TB_SQM_BENFIT_REPORT] where benbfitDate=@benbfitDate+'12' and vendorcode=A.vendorcode) AS '12月等级'
  FROM [LiteOnRFQTraining].[dbo].[TB_SQM_BENFIT_REPORT] A
  where benbfitDate like @benbfitDate+'%' and Plant=@Plant
            ");

            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.AddWithValue("@Plant", SQMStringHelper.NullOrEmptyStringIsDBNull(plant));
                cmd.Parameters.AddWithValue("@benbfitDate", SQMStringHelper.NullOrEmptyStringIsDBNull(benbfitDate));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            Common.ExportExcel(dt, localPath, fileName);
            return msg;
        }
    }

}
