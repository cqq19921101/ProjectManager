using Lib_Portal_Domain.Model;
using Lib_Portal_Domain.SharedLibs;
using Lib_VMI_Domain.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
//using Lib_SQM_Domain.SharedLibs;
//using Lib_Portal_Domain.Model;

namespace Lib_SQM_Domain.Modal
{
    #region Data Class Definitions
    public class TB_SQM_CriticalFile
    {
        public string VendorCode { get; set; }

        public Guid? IntroFGUID { get; set; }

        public String EnvirSignedTime { get; set; }

        public Guid? EnvirFGUID { get; set; }

        public bool? IsWaterEmergencySquad { get; set; }

        public bool? IsWaterResponseProgram { get; set; }

        public Guid? WaterResponseFGUID { get; set; }

        public bool? IsElecEmergencySquad { get; set; }

        public bool? IsElecResponseProgram { get; set; }

        public Guid? ElecResponseFGUID { get; set; }

        public bool? IsCMControl { get; set; }

        public Guid? CMControlFGUID { get; set; }

        public bool? IsNonCMQuestionary { get; set; }

        public Guid? NonCMQuestionaryFGUID { get; set; }

        public bool? IsNonCMCommit { get; set; }

        public Guid? NonCMCommitFGUID { get; set; }

        public string EHSOwner { get; set; }

        public string EHSOwnerTitle { get; set; }

        public string PollutionTypes { get; set; }

        public Guid? PollutionReportFGUID { get; set; }

        public Guid? PollutionAgreeFGUID { get; set; }

        public Guid? PollutionAcceptanceFGUID { get; set; }

        public string WasteTypes { get; set; }

        public Guid? WasteFGUID { get; set; }

        public bool? IsOutWasteHandler { get; set; }

        public string ExhaustType { get; set; }

        public Guid? ExhaustFGUID { get; set; }

        public string WasteWaterType { get; set; }

        public Guid? WasteWaterFGUID { get; set; }

        public string NoiseType { get; set; }

        public Guid? NoiseFGUID { get; set; }

        public string PollutionEventType { get; set; }

        public string PollutionEventDesc { get; set; }

        public string PunishmentType { get; set; }

        public string PunishmentDesc { get; set; }

    }

    public class TB_SQM_QualityEvent
    {
        public string VendorCode { get; set; }
        public string QEGUID { get; set; }

        public string QualityEventOccurredTime { get; set; }

        public string QualityEventSummary { get; set; }

        public string QualityEventProvideTime { get; set; }

        public Guid QEFGUID { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string FileName { get; set; }

    }

    public class TB_SQM_WasteHandler
    {
        public string VendorCode { get; set; }
        public string WHGUID { get; set; }

        public string LicenseType { get; set; }

        public string LicenseFGUID { get; set; }

        public string BusinessLicenseFGUID { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string FileName { get; set; }

    }
    #endregion

    public static class SQMBasic_CriticalFile_Helper
    {

        public static string GetCriticalFile(SqlConnection cn, PortalUserProfile RunAsUser, String VendorCode)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select TOP 1 * From TB_SQM_CriticalFile WHERE VendorCode=@VendorCode");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@VendorCode", VendorCode));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }

        


        #region Create/Edit data check
        private static void UnescapeDataFromWeb(TB_SQM_CriticalFile DataItem)
        {
            DataItem.VendorCode = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.VendorCode);
            DataItem.EnvirSignedTime = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.EnvirSignedTime);
            DataItem.EHSOwner = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.EHSOwner);
            DataItem.EHSOwnerTitle = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.EHSOwnerTitle);
            DataItem.PollutionTypes = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.PollutionTypes);
            DataItem.WasteTypes = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.WasteTypes);
        }

        private static string DataCheck(TB_SQM_CriticalFile DataItem)
        {
            string r = "";
            List<string> e = new List<string>();
            if (StringHelper.DataIsNullOrEmpty(DataItem.VendorCode))
                e.Add("Must provide VendorCode Name.");

            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }
            return r;
        }

        
        #endregion

        #region Edit data item
        static object StringIsNull(String input)
        {
            String s = StringHelper.EmptyOrUnescapedStringViaUrlDecode(input);
            return String.IsNullOrEmpty(s) ? (object)DBNull.Value : s;
        }

        static object IsNullToDBNull(Object input)
        {
            return input==null ? (object)DBNull.Value : input;
        }
        #endregion

        #region
        public static string UploadIntroFile(SqlConnection cn, PortalUserProfile RunAsUser, FileAttachmentInfo FA, string VendorCode, string sLocalPath, string sLocalUploadPath, HttpServerUtilityBase Server, string RequestApplicationPath,string Type,string validDate)
        {
            String r = "";
            String col = "";
            switch (Type)
            {
                case "1":
                    col = "IntroFGUID";
                    break;
                case "2":
                    col = "EnvirFGUID";
                    break;
                case "3":
                    col = "WaterResponseFGUID";
                    break;
                case "4":
                    col = "ElecResponseFGUID";
                    break;
                case "5":
                    col = "CMControlFGUID";
                    break;
                case "6":
                    col = "NonCMQuestionaryFGUID";
                    break;
                case "7":
                    col = "NonCMCommitFGUID";
                    break;
                case "8":
                    col = "PollutionReportFGUID";
                    break;
                case "9":
                    col = "PollutionAgreeFGUID";
                    break;
                case "10":
                    col = "PollutionAcceptanceFGUID";
                    break;
                case "11":
                    col = "WasteFGUID";
                    break;
                case "12":
                    col = "ExhaustFGUID";
                    break;
                case "13":
                    col = "WasteWaterFGUID";
                    break;
                case "14":
                    col = "NoiseFGUID";
                    break;
                case "15":
                    col = "";
                    break;
                case "16":
                    col = "";
                    break;
                case "17":
                    col = "";
                    break;
                case "18":
                    col = "";
                    break;
                case "19":
                    col = "";
                    break;
                case "20":
                    col = "";
                    break;
                case "21":
                    col = "";
                    break;

                default:
                    break;
            }

            JArray ja = JArray.Parse(FA.SPEC);
            dynamic jo_item = (JObject)ja[0];

            //00.UploadFileToDB
            SqlTransaction tran = cn.BeginTransaction();
            String file = sLocalUploadPath + FA.SUBFOLDER + "/" + jo_item.FileName;
            String FGUID = SharedLibs.SqlFileStreamHelper.InsertToTableSQM(cn, tran, RunAsUser.MemberGUID, file, validDate);
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
                DECLARE @OldKeyValue uniqueidentifier;

                SELECT @OldKeyValue=@col FROM TB_SQM_CriticalFile
                WHERE VendorCode = @VendorCode;

                UPDATE TB_SQM_CriticalFile 
                   SET @col=null
                WHERE VendorCode = @VendorCode;
                DELETE FROM TB_SQM_Files
                WHERE FGUID =@OldKeyValue
                ");
                String sql = Regex.Replace(sb.ToString(), @"\s+", " ");
                sql = Regex.Replace(sql, @"@col", col);

                using (SqlCommand cmd = new SqlCommand(sql, cn, tran))
                {
                    cmd.Parameters.Add(new SqlParameter("@VendorCode", VendorCode));
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
                IF NOT EXISTS (SELECT * FROM dbo.TB_SQM_CriticalFile WHERE VendorCode = @VendorCode) 
                    BEGIN
                         INSERT INTO dbo.TB_SQM_CriticalFile
                         (VendorCode) 
                         VALUES
                         (@VendorCode) 
                    END;
                UPDATE dbo.TB_SQM_CriticalFile
                  SET @col = @ColFGUID
                WHERE VendorCode = @VendorCode
                ");
                String sql = Regex.Replace(sb.ToString(), @"\s+", " ");
                sql = Regex.Replace(sql, @"@col", col);
                using (SqlCommand cmd = new SqlCommand(sql, cn,tran))
                {
                    cmd.Parameters.Add(new SqlParameter("@VendorCode", VendorCode));
                    cmd.Parameters.Add(new SqlParameter("@ColFGUID", FGUID));
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

        public static FileInfoForOutput DownloadSQMFileByStream(SqlConnection cn, string FGUID)
        {
            StringBuilder sb = new StringBuilder();
            byte[] buffer = null;
            string sFileName = @"";
            FileInfoForOutput fi = null;
            string sFSGUID = StringHelper.EmptyOrUnescapedStringViaUrlDecode(FGUID).Trim();

            sb.Clear();
            sb.Append(@"
            SELECT FileContent.PathName(), FileName, GET_FILESTREAM_TRANSACTION_CONTEXT() 
            FROM TB_SQM_Files 
            WHERE FGUID = @FGUID
            ");

            SqlTransaction tr = cn.BeginTransaction();

            using (SqlCommand cmdQueryFile = new SqlCommand(sb.ToString(), cn, tr))
            {
                cmdQueryFile.Parameters.AddWithValue("@FGUID", sFSGUID);
                try
                {
                    using (SqlDataReader dr = cmdQueryFile.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (dr.Read())
                        {
                            string sPathOfFileStreamField = dr[0].ToString();
                            sFileName = dr[1].ToString();
                            byte[] transContext = (byte[])dr[2];

                            SqlFileStream sfs = new SqlFileStream(sPathOfFileStreamField, transContext, System.IO.FileAccess.Read);

                            buffer = new byte[(int)sfs.Length];
                            sfs.Read(buffer, 0, buffer.Length);
                            sfs.Close();

                            fi = new FileInfoForOutput(buffer, sFileName);
                        }
                    }

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                }
            }

            return fi;
        }

        public static string GetCriticalFilesDetail(SqlConnection cn, PortalUserProfile RunAsUser, String VendorCode)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(
            @"
            SELECT '',
                   IntroFGUID,
                   F1.FILENAME AS IntroFN,
                   F1.UpdateTime AS IntroTime,
                   EnvirFGUID,
                   F2.FILENAME AS EnvirFN,
                   F2.UpdateTime AS EnvirTime,
                   WaterResponseFGUID,
                   F3.FILENAME AS WaterResponseFN,
                   F3.UpdateTime AS WaterResponseTime,
                   ElecResponseFGUID,
                   F4.FILENAME AS ElecResponseFN,
                   F4.UpdateTime AS ElecResponseTime,
                   CMControlFGUID,
                   F5.FILENAME AS CMControlFN,
                   F5.UpdateTime AS CMControlTime,
                   NonCMQuestionaryFGUID,
                   F6.FILENAME AS NonCMQuestionaryFN,
                   F6.UpdateTime AS NonCMQuestionaryTime,
                   NonCMCommitFGUID,
                   F7.FILENAME AS NonCMCommitFN,
                   F7.UpdateTime AS NonCMCommitTime,
                   PollutionReportFGUID,
                   F8.FILENAME AS PollutionReportFN,
                   F8.UpdateTime AS PollutionReportTime,
                   F8.ValidDate AS PollutionReportValidDate,
                   PollutionAgreeFGUID,
                   F9.FILENAME AS PollutionAgreeFN,
                   F9.UpdateTime AS PollutionAgreeTime,
                   F9.ValidDate AS PollutionAgreeValidDate,
                   PollutionAcceptanceFGUID,
                   F10.FILENAME AS PollutionAcceptanceFN,
                   F10.UpdateTime AS PollutionAcceptanceTime,
                   F10.ValidDate AS PollutionAcceptanceValidDate,
                   WasteFGUID,
                   F11.FILENAME AS WasteFN,
                   F11.UpdateTime AS WasteTime,
                   F11.ValidDate AS WasteValidDate,
                   ExhaustFGUID,
                   F12.FILENAME AS ExhaustFN,
                   F12.UpdateTime AS ExhaustTime,
                   F12.ValidDate AS ExhaustValidDate,
                   WasteWaterFGUID,
                   F13.FILENAME AS WasteWaterFN,
                   F13.UpdateTime AS WasteWaterTime,
                   F13.ValidDate AS WasteWaterValidDate,
                   NoiseFGUID,
                   F14.FILENAME AS NoiseFN,
                   F14.UpdateTime AS NoiseTime,
                   F14.ValidDate AS NoiseValidDate
            FROM TB_SQM_CriticalFile CF
                 LEFT OUTER JOIN TB_SQM_FILES F1 ON CF.IntroFGUID = F1.FGUID
                 LEFT OUTER JOIN TB_SQM_FILES F2 ON CF.EnvirFGUID = F2.FGUID
                 LEFT OUTER JOIN TB_SQM_FILES F3 ON CF.WaterResponseFGUID = F3.FGUID
                 LEFT OUTER JOIN TB_SQM_FILES F4 ON CF.ElecResponseFGUID = F4.FGUID
                 LEFT OUTER JOIN TB_SQM_FILES F5 ON CF.CMControlFGUID = F5.FGUID
                 LEFT OUTER JOIN TB_SQM_FILES F6 ON CF.NonCMQuestionaryFGUID = F6.FGUID
                 LEFT OUTER JOIN TB_SQM_FILES F7 ON CF.NonCMCommitFGUID = F7.FGUID
                 LEFT OUTER JOIN TB_SQM_FILES F8 ON CF.PollutionReportFGUID = F8.FGUID
                 LEFT OUTER JOIN TB_SQM_FILES F9 ON CF.PollutionAgreeFGUID = F9.FGUID
                 LEFT OUTER JOIN TB_SQM_FILES F10 ON CF.PollutionAcceptanceFGUID = F10.FGUID
                 LEFT OUTER JOIN TB_SQM_FILES F11 ON CF.WasteFGUID = F11.FGUID
                 LEFT OUTER JOIN TB_SQM_FILES F12 ON CF.ExhaustFGUID = F12.FGUID
                 LEFT OUTER JOIN TB_SQM_FILES F13 ON CF.WasteWaterFGUID = F13.FGUID
                 LEFT OUTER JOIN TB_SQM_FILES F14 ON CF.NoiseFGUID = F14.FGUID
                 where VendorCode=@VendorCode;
            ");
            String sql = Regex.Replace(sb.ToString(), @"\s+", " ");

            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.Add(new SqlParameter("@VendorCode", VendorCode));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }

        public static string LoadKeyInfoTypeJSonWithFilter(SqlConnection cn, string vendorCode)
        {
            SQM_KeyInfoIntroMgmt_jQGridJSon m = new SQM_KeyInfoIntroMgmt_jQGridJSon();

            m.Rows.Clear();
            int iRowCount = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
SELECT TB_VMI_PLANT.plant_name,
[TB_SQM_Member_Vendor_Map].[MemberGUID]
	  ,TB_VMI_VENDOR_DETAIL.ERP_VNAME
	  ,PORTAL_Members.[NameInChinese]
  FROM [TB_SQM_Member_Vendor_Map]
  left join TB_VMI_VENDOR_DETAIL    on TB_VMI_VENDOR_DETAIL.ERP_VND=[TB_SQM_Member_Vendor_Map].[VendorCode]
  left join TB_VMI_PLANT on TB_VMI_PLANT.plant=[TB_SQM_Member_Vendor_Map].[PlantCode]
  left join PORTAL_Members on  PORTAL_Members.[MemberGUID]=[TB_SQM_Member_Vendor_Map].[MemberGUID]
  where [PlantCode] in (
  SELECT 
      [PlantCode]
  FROM [TB_SQM_Member_Plant]
  where [MemberGUID]=@MemberGUID
  )
");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@MemberGUID", StringHelper.NullOrEmptyStringIsDBNull(vendorCode)));

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {

                        iRowCount++;
                        m.Rows.Add(new SQM_KeyInfoIntroMgmt(
                        dr["MemberGUID"].ToString(),
                        dr["plant_name"].ToString(),
                         dr["ERP_VNAME"].ToString(),
                        dr["NameInChinese"].ToString()
                      ));
                    }
                }
            }
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(m);
        }

        public static string EditCriticalFile(SqlConnection cnPortal, TB_SQM_CriticalFile DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            if (r != "")
            { return r; }
            else
            {
                SqlTransaction tran = cnPortal.BeginTransaction();

                //Update member data
                using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = EditCriticalFile(cmd, DataItem); }
                if (r != "") { tran.Rollback(); return r; }

                //Commit
                try { tran.Commit(); }
                catch (Exception e) { tran.Rollback(); r = "Edit fail.<br />Exception: " + e.ToString(); }
                return r;
            }
        }

        private static string EditCriticalFile(SqlCommand cmd, TB_SQM_CriticalFile DataItem)
        {
            string sErrMsg = "";
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append("Update TB_SQM_CriticalFile Set ");
            if (DataItem.EnvirSignedTime != null && DataItem.EnvirSignedTime != "")
                sbSQL.Append(" EnvirSignedTime=@EnvirSignedTime,");
            if (DataItem.IsWaterEmergencySquad != null)
                sbSQL.Append(" IsWaterEmergencySquad=@IsWaterEmergencySquad,");
            if (DataItem.IsWaterResponseProgram != null)
                sbSQL.Append(" IsWaterResponseProgram=@IsWaterResponseProgram,");
            if (DataItem.IsElecEmergencySquad != null)
                sbSQL.Append(" IsElecEmergencySquad=@IsElecEmergencySquad,");
            if (DataItem.IsElecResponseProgram != null)
                sbSQL.Append(" IsElecResponseProgram=@IsElecResponseProgram,");
            if (DataItem.IsCMControl != null)
                sbSQL.Append(" IsCMControl=@IsCMControl,");
            if (DataItem.IsNonCMQuestionary != null)
                sbSQL.Append(" IsNonCMQuestionary=@IsNonCMQuestionary,");
            if (DataItem.IsNonCMCommit != null)
                sbSQL.Append(" IsNonCMCommit=@IsNonCMCommit,");
            if (DataItem.EHSOwner != null && DataItem.EHSOwner != "")
                sbSQL.Append(" EHSOwner=@EHSOwner,");
            if (DataItem.EHSOwnerTitle != null && DataItem.EHSOwnerTitle != "")
                sbSQL.Append(" EHSOwnerTitle=@EHSOwnerTitle,");
            if (DataItem.PollutionTypes != null && DataItem.PollutionTypes != "")
                sbSQL.Append(" PollutionTypes=@PollutionTypes,");
            if (DataItem.WasteTypes != null && DataItem.WasteTypes != "")
                sbSQL.Append(" WasteTypes=@WasteTypes,");
            if (DataItem.IsOutWasteHandler != null)
                sbSQL.Append(" IsOutWasteHandler=@IsOutWasteHandler,");
            if (DataItem.ExhaustType != null && DataItem.ExhaustType != "")
                sbSQL.Append(" ExhaustType=@ExhaustType,");
            if (DataItem.WasteWaterType != null && DataItem.WasteWaterType != "")
                sbSQL.Append(" WasteWaterType=@WasteWaterType,");
            if (DataItem.NoiseType != null && DataItem.NoiseType != "")
                sbSQL.Append(" NoiseType=@NoiseType,");
            if (DataItem.PunishmentType != null && DataItem.PunishmentType != "")
                sbSQL.Append(" PunishmentType=@PunishmentType,");
            if (DataItem.PollutionEventType != null && DataItem.PollutionEventType != "")
                sbSQL.Append(" PollutionEventType=@PollutionEventType,");
            if (DataItem.PollutionEventDesc != null && DataItem.PollutionEventDesc != "")
                sbSQL.Append(" PollutionEventDesc=@PollutionEventDesc,");
            if (DataItem.PunishmentDesc != null && DataItem.PunishmentDesc != "")
                sbSQL.Append(" PunishmentDesc=@PunishmentDesc,");
            //sbSQL.Append(" TurnoverAnalysis=@TurnoverAnalysis,");

            //sbSQL.Append(" RevenueGrowthRate1=@RevenueGrowthRate1,");
            //sbSQL.Append(" RevenueGrowthRate2=@RevenueGrowthRate2,");
            //sbSQL.Append(" RevenueGrowthRate3=@RevenueGrowthRate3,");
            //sbSQL.Append(" GrossProfitRate1=@GrossProfitRate1,");
            //sbSQL.Append(" GrossProfitRate2=@GrossProfitRate2,");
            //sbSQL.Append(" GrossProfitRate3=@GrossProfitRate3,");
            //sbSQL.Append(" PlanInvestCapital=@PlanInvestCapital,");
            //sbSQL.Append(" BankAndAccNumber=@BankAndAccNumber,");
            //sbSQL.Append(" TradingCurrency=@TradingCurrency,");
            //sbSQL.Append(" TradeMode=@TradeMode,");
            //sbSQL.Append(" VMIManageModel=@VMIManageModel,");
            //sbSQL.Append(" Distance=@Distance,");
            //sbSQL.Append(" MinMonthStateDays=@MinMonthStateDays,");
            //sbSQL.Append(" BU1TurnoverName=@BU1TurnoverName,");
            //sbSQL.Append(" BU2TurnoverName=@BU2TurnoverName,");
            //sbSQL.Append(" BU3TurnoverName=@BU3TurnoverName,");
            //sbSQL.Append(" BU1Turnover=@BU1Turnover,");
            //sbSQL.Append(" BU2Turnover=@BU2Turnover,");
            //sbSQL.Append(" BU3Turnover=@BU3Turnover,");
            //sbSQL.Append(" CompanyAdvantage=@CompanyAdvantage,");
            sbSQL.Remove(sbSQL.Length - 1, 1);
            sbSQL.Append(" Where VendorCode =@VendorCode;");

            cmd.CommandText = sbSQL.ToString();

            if (DataItem.EnvirSignedTime != null && DataItem.EnvirSignedTime != "")
                cmd.Parameters.AddWithValue("@EnvirSignedTime", DataItem.EnvirSignedTime);
            if (DataItem.IsWaterEmergencySquad != null)
                cmd.Parameters.AddWithValue("@IsWaterEmergencySquad", DataItem.IsWaterEmergencySquad);
            if (DataItem.IsWaterResponseProgram != null)
                cmd.Parameters.AddWithValue("@IsWaterResponseProgram", DataItem.IsWaterResponseProgram);
            if (DataItem.IsElecEmergencySquad != null)
                cmd.Parameters.AddWithValue("@IsElecEmergencySquad", DataItem.IsElecEmergencySquad);
            if (DataItem.IsElecResponseProgram != null)
                cmd.Parameters.AddWithValue("@IsElecResponseProgram", DataItem.IsElecResponseProgram);
            if (DataItem.IsCMControl != null)
                cmd.Parameters.AddWithValue("@IsCMControl", DataItem.IsCMControl);
            if (DataItem.IsNonCMQuestionary != null)
                cmd.Parameters.AddWithValue("@IsNonCMQuestionary", DataItem.IsNonCMQuestionary);
            if (DataItem.IsNonCMCommit != null)
                cmd.Parameters.AddWithValue("@IsNonCMCommit", DataItem.IsNonCMCommit);
            if (DataItem.EHSOwner != null && DataItem.EHSOwner != "")
                cmd.Parameters.AddWithValue("@EHSOwner", DataItem.EHSOwner);
            if (DataItem.EHSOwnerTitle != null && DataItem.EHSOwnerTitle != "")
                cmd.Parameters.AddWithValue("@EHSOwnerTitle", DataItem.EHSOwnerTitle);
            if (DataItem.PollutionTypes != null && DataItem.PollutionTypes != "")
                cmd.Parameters.AddWithValue("@PollutionTypes", DataItem.PollutionTypes);
            if (DataItem.WasteTypes != null && DataItem.WasteTypes != "")
                cmd.Parameters.AddWithValue("@WasteTypes", DataItem.WasteTypes);
            if (DataItem.IsOutWasteHandler != null )
                cmd.Parameters.AddWithValue("@IsOutWasteHandler", DataItem.IsOutWasteHandler);
            if (DataItem.ExhaustType != null && DataItem.ExhaustType != "")
                cmd.Parameters.AddWithValue("@ExhaustType", DataItem.ExhaustType);
            if (DataItem.WasteWaterType != null && DataItem.WasteWaterType != "")
                cmd.Parameters.AddWithValue("@WasteWaterType", DataItem.WasteWaterType);
            if (DataItem.NoiseType != null && DataItem.NoiseType != "")
                cmd.Parameters.AddWithValue("@NoiseType", DataItem.NoiseType);
            if (DataItem.PunishmentType != null && DataItem.PunishmentType != "")
                cmd.Parameters.AddWithValue("@PunishmentType", DataItem.PunishmentType);
            if (DataItem.PollutionEventType != null && DataItem.PollutionEventType != "")
                cmd.Parameters.AddWithValue("@PollutionEventType", DataItem.PollutionEventType);
            if (DataItem.PollutionEventDesc != null && DataItem.PollutionEventDesc != "")
                cmd.Parameters.AddWithValue("@PollutionEventDesc", DataItem.PollutionEventDesc);
            if (DataItem.PunishmentDesc != null && DataItem.PunishmentDesc != "")
                cmd.Parameters.AddWithValue("@PunishmentDesc", DataItem.PunishmentDesc);
            //cmd.Parameters.AddWithValue("@RevenueGrowthRate1", DataItem.RevenueGrowthRate1);
            //cmd.Parameters.AddWithValue("@RevenueGrowthRate2", DataItem.RevenueGrowthRate2);
            //cmd.Parameters.AddWithValue("@RevenueGrowthRate3", DataItem.RevenueGrowthRate3);
            //cmd.Parameters.AddWithValue("@GrossProfitRate1", DataItem.GrossProfitRate1);
            //cmd.Parameters.AddWithValue("@GrossProfitRate2", DataItem.GrossProfitRate2);
            //cmd.Parameters.AddWithValue("@GrossProfitRate3", DataItem.GrossProfitRate3);
            //cmd.Parameters.AddWithValue("@PlanInvestCapital", StringIsNull(DataItem.PlanInvestCapital));
            //cmd.Parameters.AddWithValue("@BankAndAccNumber", StringIsNull(DataItem.BankAndAccNumber));
            //cmd.Parameters.AddWithValue("@TradingCurrency", StringIsNull(DataItem.TradingCurrency));
            //cmd.Parameters.AddWithValue("@TradeMode", StringIsNull(DataItem.TradeMode));
            //cmd.Parameters.AddWithValue("@VMIManageModel", StringIsNull(DataItem.VMIManageModel));
            //cmd.Parameters.AddWithValue("@Distance", StringIsNull(DataItem.Distance));
            //cmd.Parameters.AddWithValue("@MinMonthStateDays", StringIsNull(DataItem.MinMonthStateDays));
            //cmd.Parameters.AddWithValue("@BU1TurnoverName", StringIsNull(DataItem.BU1TurnoverName));
            //cmd.Parameters.AddWithValue("@BU2TurnoverName", StringIsNull(DataItem.BU2TurnoverName));
            //cmd.Parameters.AddWithValue("@BU3TurnoverName", StringIsNull(DataItem.BU3TurnoverName));
            //cmd.Parameters.AddWithValue("@BU1Turnover", StringIsNull(DataItem.BU1Turnover));
            //cmd.Parameters.AddWithValue("@BU2Turnover", StringIsNull(DataItem.BU2Turnover));
            //cmd.Parameters.AddWithValue("@BU3Turnover", StringIsNull(DataItem.BU3Turnover));
            //cmd.Parameters.AddWithValue("@CompanyAdvantage", StringIsNull(DataItem.CompanyAdvantage));

            cmd.Parameters.AddWithValue("@VendorCode", DataItem.VendorCode);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion

        #region TB_SQM_QualityEvent
        public static string GetQualityEvent(SqlConnection cn, PortalUserProfile RunAsUser, String VendorCode)
        {
            string urlPre = CommonHelper.urlPre;
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
            SELECT TQE.QEGUID,
                   TQE.QualityEventOccurredTime,
                   TQE.QualityEventSummary,
                   TQE.QualityEventProvideTime,
                   TQE.QEFGUID, 
                   TF.FileName,
                   TF.UpdateTime,
                    ('<a href=""" + urlPre + @"/SQMBasic/DownloadSQMFile?DataKey='+convert(nvarchar(50), TQE.QEFGUID)+'"">' + TF.FileName + '</a>') as FileUrlTag
            FROM TB_SQM_QualityEvent TQE
            left outer join TB_SQM_Files TF ON TQE.QEFGUID = TF.FGUID,
                 TB_SQM_QualityEventMap TQEM
            WHERE TQEM.VendorCode = @VendorCode
              AND TQEM.QEGUID = TQE.QEGUID
            ");
            String sql = Regex.Replace(sb.ToString(), @"\s+", " ");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.Add(new SqlParameter("@VendorCode", VendorCode));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }


        private static void UnescapeDataFromWeb(TB_SQM_QualityEvent DataItem)
        {
            DataItem.VendorCode = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.VendorCode);
            DataItem.QualityEventOccurredTime = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.QualityEventOccurredTime);
            DataItem.QualityEventSummary = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.QualityEventSummary);
            DataItem.QualityEventProvideTime = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.QualityEventProvideTime);
            //DataItem.EnvirSignedTime = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.EnvirSignedTime);
        }

        private static string DataCheck(TB_SQM_QualityEvent DataItem)
        {
            string r = "";
            //List<string> e = new List<string>();
            //if (StringHelper.DataIsNullOrEmpty(DataItem.VendorCode))
            //    e.Add("Must provide VendorCode Name.");

            //for (int iCnt = 0; iCnt < e.Count; iCnt++)
            //{
            //    if (iCnt > 0) r += "<br />";
            //    r += e[iCnt];
            //}
            return r;
        }

        public static string CreateDataItemQE(SqlConnection cnPortal, TB_SQM_QualityEvent DataItem)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            if (r != "")
            { return r; }
            else
            {
                string sSQL = @"
                INSERT INTO [dbo].[TB_SQM_QualityEvent]
                       ([QEGUID]
                       ,[QualityEventOccurredTime]
                       ,[QualityEventSummary]
                       ,[QualityEventProvideTime])
                 VALUES
                       (@QEGUID
                       ,@QualityEventOccurredTime
                       ,@QualityEventSummary
                       ,@QualityEventProvideTime
                        );
                INSERT INTO [dbo].[TB_SQM_QualityEventMap]
                       ([QEGUID]
                       ,[VendorCode]
                       )
                 VALUES
                       (@QEGUID
                       ,@VendorCode
                       );
                ";
                sSQL = Regex.Replace(sSQL, @"\s+", " ");
                SqlCommand cmd = new SqlCommand(sSQL, cnPortal);
                
                cmd.Parameters.AddWithValue("@QEGUID", DataItem.QEGUID);
                cmd.Parameters.AddWithValue("@VendorCode", DataItem.VendorCode);
                cmd.Parameters.AddWithValue("@QualityEventOccurredTime", StringHelper.NullOrEmptyStringIsDBNull(DataItem.QualityEventOccurredTime));
                cmd.Parameters.AddWithValue("@QualityEventSummary", StringHelper.NullOrEmptyStringIsDBNull(DataItem.QualityEventSummary));
                cmd.Parameters.AddWithValue("@QualityEventProvideTime", StringHelper.NullOrEmptyStringIsDBNull(DataItem.QualityEventProvideTime));

                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;

                return sErrMsg;
            }
        }

        public static string EditQualityEvent(SqlConnection cnPortal, TB_SQM_QualityEvent DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            if (r != "")
            { return r; }
            else
            {
                SqlTransaction tran = cnPortal.BeginTransaction();

                //Update member data
                using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = EditQualityEvent(cmd, DataItem); }
                if (r != "") { tran.Rollback(); return r; }

                //Commit
                try { tran.Commit(); }
                catch (Exception e) { tran.Rollback(); r = "Edit fail.<br />Exception: " + e.ToString(); }
                return r;
            }
        }

        private static string EditQualityEvent(SqlCommand cmd, TB_SQM_QualityEvent DataItem)
        {
            string sErrMsg = "";
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append("Update TB_SQM_QualityEvent Set ");
            if (DataItem.QualityEventOccurredTime != null)
                sbSQL.Append(" QualityEventOccurredTime=@QualityEventOccurredTime,");
            if (DataItem.QualityEventSummary != null)
                sbSQL.Append(" QualityEventSummary=@QualityEventSummary,");
            if (DataItem.QualityEventProvideTime != null)
                sbSQL.Append(" QualityEventProvideTime=@QualityEventProvideTime,");
            //if (DataItem.QEFGUID != null)
            //    sbSQL.Append(" QEFGUID=@QEFGUID,");
            sbSQL.Remove(sbSQL.Length - 1, 1);
            sbSQL.Append(" Where QEGUID =@QEGUID");
            sbSQL.Append(" AND (SELECT COUNT(1) FROM TB_SQM_QualityEventMap WHERE VendorCode=@VenderCode AND QEGUID=@QEGUID)>0");
            cmd.CommandText = sbSQL.ToString();

            if (DataItem.QualityEventOccurredTime != null)
                cmd.Parameters.AddWithValue("@QualityEventOccurredTime", DataItem.QualityEventOccurredTime);
            if (DataItem.QualityEventSummary != null)
                cmd.Parameters.AddWithValue("@QualityEventSummary", DataItem.QualityEventSummary);
            if (DataItem.QualityEventProvideTime != null)
                cmd.Parameters.AddWithValue("@QualityEventProvideTime", DataItem.QualityEventProvideTime);

            cmd.Parameters.AddWithValue("@QEGUID", DataItem.QEGUID);
            cmd.Parameters.AddWithValue("@VenderCode", DataItem.VendorCode);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }

        public static string UploadQualityEventFile(SqlConnection cn, PortalUserProfile RunAsUser, FileAttachmentInfo FA, string VendorCode, string sLocalPath, string sLocalUploadPath, HttpServerUtilityBase Server, string RequestApplicationPath, string QEGUID)
        {
            String r = "";
            String col = "QEFGUID";


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
                DECLARE @OldKeyValue uniqueidentifier;
                
                SELECT @OldKeyValue=@col
                FROM TB_SQM_QualityEvent QE,
                     TB_SQM_QualityEventMAP QEM
                WHERE QEM.VendorCode = @VendorCode
                  AND QE.QEGUID = @QEGUID
                  AND QEM.QEGUID = @QEGUID;

                UPDATE TB_SQM_QualityEvent
                  SET @col = NULL
                WHERE QEGUID = @QEGUID
                  AND ( SELECT COUNT(1)
                        FROM TB_SQM_QualityEventMAP
                        WHERE QEGUID = @QEGUID
                          AND VendorCode = @VendorCode ) > 0;

                DELETE FROM TB_SQM_Files
                WHERE FGUID =@OldKeyValue;
                ");
                String sql = Regex.Replace(sb.ToString(), @"\s+", " ");
                sql = Regex.Replace(sql, @"@col", col);

                using (SqlCommand cmd = new SqlCommand(sql, cn, tran))
                {
                    cmd.Parameters.Add(new SqlParameter("@VendorCode", VendorCode));
                    cmd.Parameters.Add(new SqlParameter("@QEGUID", QEGUID));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                r = ex.ToString();
                return r;
            }

            //02.Update new FGUID
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(
                @"
                UPDATE dbo.TB_SQM_QualityEvent
                  SET @col = @ColFGUID
                WHERE QEGUID = @QEGUID
                  AND ( SELECT COUNT(1)
                        FROM TB_SQM_QualityEventMAP
                        WHERE QEGUID = @QEGUID
                          AND VendorCode = @VendorCode ) > 0;
                ");
                String sql = Regex.Replace(sb.ToString(), @"\s+", " ");
                sql = Regex.Replace(sql, @"@col", col);
                using (SqlCommand cmd = new SqlCommand(sql, cn, tran))
                {
                    cmd.Parameters.Add(new SqlParameter("@VendorCode", VendorCode));
                    cmd.Parameters.Add(new SqlParameter("@QEGUID", QEGUID));
                    cmd.Parameters.Add(new SqlParameter("@ColFGUID", FGUID));
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

        
        public static string DeleteQualityEvent(SqlConnection cnPortal, TB_SQM_QualityEvent DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            string r = "";
            if (StringHelper.DataIsNullOrEmpty(DataItem.QEGUID))//StringHelper.DataIsNullOrEmpty(DataItem.QEGUID)
                return "Must provide Quality Event ID.";
            else
            {
                SqlTransaction tran = cnPortal.BeginTransaction();

                //Delete member data
                using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = DeleteQualityEvent(cmd, DataItem); }
                if (r != "") { tran.Rollback(); return r; }

                //Release lock
                if ((LoginMemberGUID != "") && (RunAsMemberGUID != ""))
                    using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = DataLockHelper.ReleaseLock(cmd, DataItem.QEGUID, LoginMemberGUID, RunAsMemberGUID); }
                if (r != "") { tran.Rollback(); return r; }

                //Commit
                try { tran.Commit(); }
                catch (Exception e) { tran.Rollback(); r = "Delete fail.<br />Exception: " + e.ToString(); }
                return r;
            }
        }

        private static string DeleteQualityEvent(SqlCommand cmd, TB_SQM_QualityEvent DataItem)
        {
            string sErrMsg = "";

            //string sSQL = @"Delete TB_SQM_QualityEvent Where QEGUID = @QEGUID;";
            string sSQL = @"
            DECLARE @OldKeyValue uniqueidentifier;

            SELECT @OldKeyValue=QEFGUID FROM TB_SQM_QualityEvent
            WHERE ( SELECT COUNT(1) FROM TB_SQM_QualityEventMap WHERE VendorCode = @VendorCode AND QEGUID =@QEGUID) >0
            AND QEGUID =@QEGUID;
            
            DELETE TB_SQM_QualityEventMap
            WHERE VendorCode =@VendorCode
              AND QEGUID =@QEGUID;
            
            DELETE TB_SQM_QualityEvent
            WHERE QEGUID =@QEGUID;

            DELETE FROM TB_SQM_Files
            WHERE FGUID =@OldKeyValue
            ";
            sSQL = Regex.Replace(sSQL, @"\s+", " ");
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@QEGUID", DataItem.QEGUID);
            cmd.Parameters.AddWithValue("@VendorCode", DataItem.VendorCode);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion

        #region TB_SQM_WasteHandler
        public static string GetWasteHandler(SqlConnection cn, PortalUserProfile RunAsUser, String VendorCode)
        {
            string urlPre = CommonHelper.urlPre;
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
            SELECT TWH.WHGUID,
                   TWH.LicenseType,
                   ( TAP.PARAME_VALUE1 ) AS LicenseTypeName,
                   TF1.FileName AS FileName1,
                   TF1.UpdateTime AS UpdateTime1,
                   ( '<a href=""" + urlPre + @"/SQMBasic/DownloadSQMFile?DataKey=' + CONVERT( NVARCHAR(50), TWH.LicenseFGUID) + '"">' + TF1.FileName + '</a>' ) AS FileUrlTag1,
                   TF2.FileName AS FileName2,
                   TF2.UpdateTime AS UpdateTime2,
                   ( '<a href=""" + urlPre + @"/SQMBasic/DownloadSQMFile?DataKey=' + CONVERT( NVARCHAR(50), TWH.BusinessLicenseFGUID) + '"">' + TF2.FileName + '</a>' ) AS FileUrlTag2
            FROM TB_SQM_WasteHandler TWH
                 LEFT OUTER JOIN TB_SQM_Files TF1 ON TWH.LicenseFGUID = TF1.FGUID
                 LEFT OUTER JOIN TB_SQM_Files TF2 ON TWH.BusinessLicenseFGUID = TF2.FGUID
                 LEFT OUTER JOIN TB_SQM_APPLICATION_PARAM TAP ON TAP.PARAME_ITEM = TWH.LicenseType
                                                             AND TAP.APPLICATION_NAME = 'SQM'
                                                             AND TAP.FUNCTION_NAME = 'EHS'
                                                             AND TAP.PARAME_NAME = 'AnswerType',
                 TB_SQM_WasteHandlerMap TWHM
            WHERE TWHM.VendorCode = @VendorCode
              AND TWHM.WHGUID = TWH.WHGUID
            ");
            String sql = Regex.Replace(sb.ToString(), @"\s+", " ");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.Add(new SqlParameter("@VendorCode", VendorCode));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }


        private static void UnescapeDataFromWeb(TB_SQM_WasteHandler DataItem)
        {
            DataItem.VendorCode = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.VendorCode);
            DataItem.LicenseType = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.LicenseType);
            //DataItem.WasteHandlerSummary = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.WasteHandlerSummary);
            //DataItem.WasteHandlerProvideTime = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.WasteHandlerProvideTime);
            //DataItem.EnvirSignedTime = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.EnvirSignedTime);
        }

        private static string DataCheck(TB_SQM_WasteHandler DataItem)
        {
            string r = "";
            //List<string> e = new List<string>();
            //if (StringHelper.DataIsNullOrEmpty(DataItem.VendorCode))
            //    e.Add("Must provide VendorCode Name.");

            //for (int iCnt = 0; iCnt < e.Count; iCnt++)
            //{
            //    if (iCnt > 0) r += "<br />";
            //    r += e[iCnt];
            //}
            return r;
        }

        public static string CreateDataItemWH(SqlConnection cnPortal, TB_SQM_WasteHandler DataItem)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            if (r != "")
            { return r; }
            else
            {
                string sSQL = @"
                INSERT INTO [dbo].[TB_SQM_WasteHandler]
                       ([WHGUID]
                       ,[LicenseType])
                 VALUES
                       (@WHGUID
                       ,@LicenseType
                        );
                INSERT INTO [dbo].[TB_SQM_WasteHandlerMap]
                       ([WHGUID]
                       ,[VendorCode]
                       )
                 VALUES
                       (@WHGUID
                       ,@VendorCode
                       );
                ";
                sSQL = Regex.Replace(sSQL, @"\s+", " ");
                SqlCommand cmd = new SqlCommand(sSQL, cnPortal);

                cmd.Parameters.AddWithValue("@WHGUID", DataItem.WHGUID);
                cmd.Parameters.AddWithValue("@VendorCode", DataItem.VendorCode);
                cmd.Parameters.AddWithValue("@LicenseType", StringHelper.NullOrEmptyStringIsDBNull(DataItem.LicenseType));
                

                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;

                return sErrMsg;
            }
        }

        public static string EditWasteHandler(SqlConnection cnPortal, TB_SQM_WasteHandler DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            if (r != "")
            { return r; }
            else
            {
                SqlTransaction tran = cnPortal.BeginTransaction();

                //Update member data
                using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = EditWasteHandler(cmd, DataItem); }
                if (r != "") { tran.Rollback(); return r; }

                //Commit
                try { tran.Commit(); }
                catch (Exception e) { tran.Rollback(); r = "Edit fail.<br />Exception: " + e.ToString(); }
                return r;
            }
        }

        private static string EditWasteHandler(SqlCommand cmd, TB_SQM_WasteHandler DataItem)
        {
            string sErrMsg = "";
            //StringBuilder sbSQL = new StringBuilder();
            //sbSQL.Append("Update TB_SQM_WasteHandler Set ");
            //if (DataItem.WasteHandlerOccurredTime != null)
            //    sbSQL.Append(" WasteHandlerOccurredTime=@WasteHandlerOccurredTime,");
            //if (DataItem.WasteHandlerSummary != null)
            //    sbSQL.Append(" WasteHandlerSummary=@WasteHandlerSummary,");
            //if (DataItem.WasteHandlerProvideTime != null)
            //    sbSQL.Append(" WasteHandlerProvideTime=@WasteHandlerProvideTime,");
            ////if (DataItem.QEFGUID != null)
            ////    sbSQL.Append(" QEFGUID=@QEFGUID,");
            //sbSQL.Remove(sbSQL.Length - 1, 1);
            //sbSQL.Append(" Where QEGUID =@QEGUID");
            //sbSQL.Append(" AND (SELECT COUNT(1) FROM TB_SQM_WasteHandlerMap WHERE VendorCode=@VenderCode AND QEGUID=@QEGUID)>0");
            //cmd.CommandText = sbSQL.ToString();

            //if (DataItem.WasteHandlerOccurredTime != null)
            //    cmd.Parameters.AddWithValue("@WasteHandlerOccurredTime", DataItem.WasteHandlerOccurredTime);
            //if (DataItem.WasteHandlerSummary != null)
            //    cmd.Parameters.AddWithValue("@WasteHandlerSummary", DataItem.WasteHandlerSummary);
            //if (DataItem.WasteHandlerProvideTime != null)
            //    cmd.Parameters.AddWithValue("@WasteHandlerProvideTime", DataItem.WasteHandlerProvideTime);

            //cmd.Parameters.AddWithValue("@QEGUID", DataItem.QEGUID);
            //cmd.Parameters.AddWithValue("@VenderCode", DataItem.VendorCode);

            //try { cmd.ExecuteNonQuery(); }
            //catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }

        public static string UploadWasteHandlerFile(SqlConnection cn, PortalUserProfile RunAsUser, FileAttachmentInfo FA, string VendorCode, string sLocalPath, string sLocalUploadPath, HttpServerUtilityBase Server, string RequestApplicationPath, string WHGUID, string type)
        {
            String r = "";
            String col = "";
            if (type == "1")
            {
                col = "LicenseFGUID";
            }
            else if (type == "2")
            {
                col = "BusinessLicenseFGUID";
            }

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
                DECLARE @OldKeyValue uniqueidentifier;
                
                SELECT @OldKeyValue=@col
                FROM TB_SQM_WasteHandler WH,
                     TB_SQM_WasteHandlerMAP WHM
                WHERE WHM.VendorCode = @VendorCode
                  AND WH.WHGUID = @WHGUID
                  AND WHM.WHGUID = @WHGUID;

                UPDATE TB_SQM_WasteHandler
                  SET @col = NULL
                WHERE WHGUID = @WHGUID
                  AND ( SELECT COUNT(1)
                        FROM TB_SQM_WasteHandlerMAP
                        WHERE WHGUID = @WHGUID
                          AND VendorCode = @VendorCode ) > 0;

                DELETE FROM TB_SQM_Files
                WHERE FGUID =@OldKeyValue;
                ");
                String sql = Regex.Replace(sb.ToString(), @"\s+", " ");
                sql = Regex.Replace(sql, @"@col", col);

                using (SqlCommand cmd = new SqlCommand(sql, cn, tran))
                {
                    cmd.Parameters.Add(new SqlParameter("@VendorCode", VendorCode));
                    cmd.Parameters.Add(new SqlParameter("@WHGUID", WHGUID));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                r = ex.ToString();
                return r;
            }

            //02.Update new FGUID
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(
                @"
                UPDATE dbo.TB_SQM_WasteHandler
                  SET @col = @ColFGUID
                WHERE WHGUID = @WHGUID
                  AND ( SELECT COUNT(1)
                        FROM TB_SQM_WasteHandlerMAP
                        WHERE WHGUID = @WHGUID
                          AND VendorCode = @VendorCode ) > 0;
                ");
                String sql = Regex.Replace(sb.ToString(), @"\s+", " ");
                sql = Regex.Replace(sql, @"@col", col);
                using (SqlCommand cmd = new SqlCommand(sql, cn, tran))
                {
                    cmd.Parameters.Add(new SqlParameter("@VendorCode", VendorCode));
                    cmd.Parameters.Add(new SqlParameter("@WHGUID", WHGUID));
                    cmd.Parameters.Add(new SqlParameter("@ColFGUID", FGUID));
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
        
        public static string DeleteWasteHandler(SqlConnection cnPortal, TB_SQM_WasteHandler DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            string r = "";
            if (StringHelper.DataIsNullOrEmpty(DataItem.WHGUID))//StringHelper.DataIsNullOrEmpty(DataItem.QEGUID)
                return "Must provide Quality Event ID.";
            else
            {
                SqlTransaction tran = cnPortal.BeginTransaction();

                //Delete member data
                using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = DeleteWasteHandler(cmd, DataItem); }
                if (r != "") { tran.Rollback(); return r; }

                //Release lock
                if ((LoginMemberGUID != "") && (RunAsMemberGUID != ""))
                    using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = DataLockHelper.ReleaseLock(cmd, DataItem.WHGUID, LoginMemberGUID, RunAsMemberGUID); }
                if (r != "") { tran.Rollback(); return r; }

                //Commit
                try { tran.Commit(); }
                catch (Exception e) { tran.Rollback(); r = "Delete fail.<br />Exception: " + e.ToString(); }
                return r;
            }
        }

        private static string DeleteWasteHandler(SqlCommand cmd, TB_SQM_WasteHandler DataItem)
        {
            string sErrMsg = "";

            //string sSQL = @"Delete TB_SQM_WasteHandler Where WHGUID = @WHGUID;";
            string sSQL = @"
            DECLARE @OldKeyValue uniqueidentifier;
            DECLARE @OldKeyValue2 uniqueidentifier;
            SELECT @OldKeyValue=LicenseFGUID,@OldKeyValue2=BusinessLicenseFGUID FROM TB_SQM_WasteHandler
            WHERE ( SELECT COUNT(1) FROM TB_SQM_WasteHandlerMap WHERE VendorCode = @VendorCode AND WHGUID =@WHGUID) >0
            AND WHGUID =@WHGUID;
            
            DELETE TB_SQM_WasteHandlerMap
            WHERE VendorCode =@VendorCode
              AND WHGUID =@WHGUID;
            
            DELETE TB_SQM_WasteHandler
            WHERE WHGUID =@WHGUID;

            DELETE FROM TB_SQM_Files
            WHERE FGUID =@OldKeyValue;
            DELETE FROM TB_SQM_Files
            WHERE FGUID =@OldKeyValue2;
            ";
            sSQL = Regex.Replace(sSQL, @"\s+", " ");
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@WHGUID", DataItem.WHGUID);
            cmd.Parameters.AddWithValue("@VendorCode", DataItem.VendorCode);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion
    }

    public class SQM_KeyInfoIntroMgmt
    {
        protected string _MemberGUID;
        protected string _plantName;
        protected string _Name;
        protected string _NameInChinese;
        public string MemberGUID { get { return this._MemberGUID; } set { this._MemberGUID = value; } }
        public string plantName { get { return this._plantName; } set { this._plantName = value; } }
        public string Name { get { return this._Name; } set { this._Name = value; } }
        public string NameInChinese { get { return this._NameInChinese; } set { this._NameInChinese = value; } }
        public SQM_KeyInfoIntroMgmt()
        {

        }
        public SQM_KeyInfoIntroMgmt(string MemberGUID, string plantName,string Name, string NameInChinese)
        {
            this._MemberGUID = MemberGUID;
            this._plantName = plantName;
            this._Name = Name;
            this._NameInChinese = NameInChinese;
        }
    }
    public class SQM_KeyInfoIntroMgmt_jQGridJSon
    {
        public List<SQM_KeyInfoIntroMgmt> Rows = new List<SQM_KeyInfoIntroMgmt>();

        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }

}
