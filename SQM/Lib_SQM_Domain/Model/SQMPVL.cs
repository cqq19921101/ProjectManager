using Aspose.Cells;
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
using System.Text.RegularExpressions;
using System.Security.Cryptography;

//using Lib_SQM_Domain.SharedLibs;
//using Lib_Portal_Domain.Model;

namespace Lib_SQM_Domain.Modal
{
    public class SystemMgmt_PVL
    {
        protected string _BasicInfoGUID;
        protected string _ISChoose;
        public int TB_SQM_Vendor_TypeCID { get; set; }

        public string BasicInfoGUID { get { return this._BasicInfoGUID; } set { this._BasicInfoGUID = value; } }
        public string ISChoose { get { return this._ISChoose; } set { this._ISChoose = value; } }


        public SystemMgmt_PVL() { }
        public SystemMgmt_PVL(string BasicInfoGUID, string ISChoose)
        {
            this._BasicInfoGUID = BasicInfoGUID;
            this._ISChoose = ISChoose;
        }
    }


    public static class SQM_PVL_Helper
    {

        static String sKEY = "a@123456";
        static String sIV = "a@654321";

        //public static string GetVendorType(SqlConnection cn, PortalUserProfile RunAsUser)
        //{
        //	StringBuilder sb = new StringBuilder();
        //	sb.Append("Select (convert(nvarchar(255), CID))as CID, CNAME From TB_SQM_Vendor_Type Order By CID;");
        //	//sb.Append("Select CID, CNAME From TB_SQM_Commodity Order By CID;");
        //	DataTable dt = new DataTable();
        //	using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
        //	{

        //		using (SqlDataReader dr = cmd.ExecuteReader())
        //		{
        //			dt.Load(dr);
        //		}
        //	}
        //	return JsonConvert.SerializeObject(dt);
        //}
        public static string GetBasicInfoTypes(SqlConnection cn, string VTNAME, string CNAME, string CSNAME, String VendorCode)
        {
            StringBuilder sb = new StringBuilder();
            string SVTNAME = StringHelper.EmptyOrUnescapedStringViaUrlDecode(VTNAME);
            string SCNAME = StringHelper.EmptyOrUnescapedStringViaUrlDecode(CNAME);
            string SCSNAME = StringHelper.EmptyOrUnescapedStringViaUrlDecode(CSNAME);
            sb.Append(@"
           SELECT ROW_NUMBER() OVER(ORDER BY X.Score DESC) AS rows,
                   X.Ven as BasicInfoGUID,
                   X.TB_SQM_Vendor_TypeCID,
                   X.TB_SQM_CommodityCID,
                   X.TB_SQM_Commodity_SubCID,
                   X.VTNAME,
                   X.CNAME,
                   X.CSNAME,
                   X.PlantCode,
                   X.VendorCode,
                   X.ISChoose,
                   X.Score,
                   X.Status
            FROM( 
                  SELECT BasicInfoGUID,
                         BI.VendorCode AS Ven,
                         B.VendorCode,
                         B.PlantCode,
                         TB_SQM_Vendor_TypeCID,
                         TB_SQM_Commodity_SubCID,
                         VT.CNAME AS VTNAME,
                         C.CNAME AS CNAME,
                         CS.TB_SQM_CommodityCID,
                         CS.CNAME AS CSNAME,
                         BI.IsChoose AS ISChoose,
                         Score,
                         ISNULL(AC.Status, 'None') AS Status
                  FROM TB_SQM_Manufacturers_BasicInfo BI
                       LEFT OUTER JOIN TB_SQM_Approve_Case AC ON BI.ChooseCaseID = AC.CaseID,
                       TB_SQM_Vendor_Type VT,
                       TB_SQM_Commodity_Sub CS,
                       TB_SQM_Commodity C,
                       TB_SQM_Member_Vendor_Map B
                  WHERE BI.TB_SQM_Vendor_TypeCID = VT.CID
                    AND BI.TB_SQM_Commodity_SubTB_SQM_CommodityCID = C.CID
                    AND BI.TB_SQM_Commodity_SubTB_SQM_CommodityCID = CS.TB_SQM_CommodityCID
                    AND BI.TB_SQM_Commodity_SubCID = CS.CID
                    AND convert(varchar(50),B.[MemberGUID]) = BI.VendorCode ) X --@VendorCode 登入者ID

			");//INNER JOIN [dbo].[TB_SQM_Member_Plant] ON [TB_SQM_Member_Plant].[MemberGUID] = X.Ven
            if (SVTNAME.Equals(string.Empty) || SCNAME.Equals(string.Empty) || SCSNAME.Equals(string.Empty))
            {
                return "";
            }
            else
            {
                sb.Append("  Where   X.TB_SQM_Vendor_TypeCID = @VTNAME  and X.TB_SQM_CommodityCID = @CNAME ");
                sb.Append(" and X.TB_SQM_Commodity_SubCID = @CSNAME");

            }
            sb.Append(@"
            AND X.PlantCode IN( 
                      SELECT DISTINCT PlantCode
                      FROM TB_SQM_Member_Plant
                      WHERE MemberGUID = @VendorCode )
            ");
            sb.Append(" order by X.Score DESC");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@VTNAME", StringHelper.NullOrEmptyStringIsDBNull(VTNAME)));
                cmd.Parameters.Add(new SqlParameter("@CNAME", StringHelper.NullOrEmptyStringIsDBNull(CNAME)));
                cmd.Parameters.Add(new SqlParameter("@CSNAME", StringHelper.NullOrEmptyStringIsDBNull(CSNAME)));
                cmd.Parameters.Add(new SqlParameter("@VendorCode", VendorCode));
                //cmd.Parameters.Add(new SqlParameter("@VendorCode", VendorCode));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }


            return JsonConvert.SerializeObject(dt);
        }

        public static string GetBasicData(SqlConnection cn, PortalUserProfile RunAsUser, TB_SQM_Manufacturers_BasicInfo DataItem)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select TOP 1 * From TB_SQM_Manufacturers_BasicInfo WHERE VendorCode=@VendorCode AND BasicInfoGUID=@BasicInfoGUID");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@VendorCode", DataItem.VendorCode));
                cmd.Parameters.Add(new SqlParameter("@BasicInfoGUID", DataItem.BasicInfoGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }

        public static string GetCommodityList(SqlConnection cn, PortalUserProfile RunAsUser)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select CID, CNAME From TB_SQM_Commodity Order By CID;");

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






        public static string EditPVL(SqlConnection cnPortal, SystemMgmt_PVL DataItem)
        {
            return EditPVL(cnPortal, DataItem, "", "");
        }

        private static void UnescapeDataFromWeb(SystemMgmt_PVL DataItem)
        {
            DataItem.BasicInfoGUID = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.BasicInfoGUID);

        }

        private static string DataCheck(SystemMgmt_PVL DataItem)
        {
            string r = "";
            List<string> e = new List<string>();
            if (StringHelper.DataIsNullOrEmpty(DataItem.BasicInfoGUID))
                e.Add("Must provide BasicInfoGUID.");
            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }

        public static string EditPVL(SqlConnection cnPortal, SystemMgmt_PVL DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            if (r != "")
            { return r; }
            else
            {
                using (SqlCommand cmd = new SqlCommand("", cnPortal)) { r = EditPVLR(cmd, DataItem); }
                if (r != "") { return r; }
                return r;
            }
        }

        private static string EditPVLR(SqlCommand cmd, SystemMgmt_PVL DataItem)
        {
            string sErrMsg = "";
                StringBuilder sb = new StringBuilder();
                sb.Append(@"
         UPDATE [TB_SQM_Manufacturers_BasicInfo] SET ISChoose = @ISChoose
         where BasicInfoGUID = @BasicInfoGUID
                    ");
                cmd.CommandText = sb.ToString();
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(DataItem.BasicInfoGUID));
                cmd.Parameters.AddWithValue("@ISChoose", StringHelper.NullOrEmptyStringIsDBNull(DataItem.ISChoose));
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }
                return sErrMsg;
            //else
            //{

            //}
        }



        #region 送簽

        public static String CreatExcel(String filename, String filenameNet, SqlConnection sqlConnection, SystemMgmt_PVL dataItem, String localPath, String RunAsUserGUID, String urlPre)
        {
            String r;
            r = "";
            //創建簽核流程
            r = SQM_PVL_Helper.CreateApproveCase(sqlConnection, dataItem, "", "", urlPre, "2");
            if (r != "")
            {
                return r;
            }

            return r;


        }


        public static String CreateApproveCase(SqlConnection cnPortal, SystemMgmt_PVL DataItem, String fileName, String fileNameNet, String urlPre, String CaseType)
        {
            #region 共用參數
            String sSQL = "";
            String sErrMsg = "";
            String sCaseID = "";
            #endregion

            UnescapeDataFromWeb(DataItem);
            String r = "";
            r = DataCheck(DataItem);
            r = CheckApprover(cnPortal, DataItem);
            if (r != "")
            { return r; }
            else
            {
                //sSQL = "Insert Into TB_SQM_Manufacturers_BasicInfo (BasicInfoGUID, VendorCode, TB_SQM_Vendor_TypeCID, TB_SQM_Commodity_SubTB_SQM_CommodityCID, TB_SQM_Commodity_SubCID,CreateDatetime,UpdateDatetime) ";
                //sSQL += "Values (@BasicInfoGUID, @VendorCode, @TB_SQM_Vendor_TypeCID, @TB_SQM_Commodity_SubTB_SQM_CommodityCID, @TB_SQM_Commodity_SubCID,getDate(),getDate());";
                #region 創建Case
                sSQL = @"
				DECLARE @NowDT Datetime
				SELECT @NowDT=getDate()
				--Insert into CASE
				Insert Into TB_SQM_Approve_Case(FormNo,Type,Status,CreateTime,UpdateTime)
				Values (@BasicInfoGUID ,@Type,@Status,@NowDT,@NowDT);
				--取得 CaseID
				DECLARE @CaseID nvarchar(50)
				SELECT MAX(CASEID) as CaseID FROM TB_SQM_Approve_Case WHERE FormNo=@BasicInfoGUID 
				";

                {
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand(sSQL, cnPortal);
                    cmd.Parameters.AddWithValue("@BasicInfoGUID", DataItem.BasicInfoGUID);
                    //cmd.Parameters.AddWithValue("@VendorCode", DataItem.VendorCode);
                    cmd.Parameters.AddWithValue("@Type", CaseType);
                    cmd.Parameters.AddWithValue("@Status", "Pending");
                    try
                    {
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            dt.Load(sdr);
                        }
                        sCaseID = dt.Rows[0]["CaseID"].ToString();
                    }
                    catch (Exception e) { sErrMsg = "Check fail.<br />Exception: " + e.ToString(); return sErrMsg; }
                    cmd = null;
                }
                if (sCaseID == "")
                {
                    return "Cerate Case Fail";
                }
                #endregion

                #region 取得初始並行簽核單位,並且迴圈創建Task
                sSQL = @"
				--取得並行簽核單位,並且創建Task
				--DECLARE @CaseID bigint
                --set @CaseID =37
                --DECLARE @BasicInfoGUID nvarchar(50)
                --SET @BasicInfoGUID = '49a92f65-2bb5-4b9b-a85b-71ba7d263828'

                DECLARE @NowDT DATETIME
                SELECT @NowDT = GETDATE()
                --INSERT INTO TB_SQM_Approve_Task (CaseID,StepName,TaskType,ApproverGUID,Status,CreateTime,UpdateTime)
                SELECT LT.CaseID,
                       LT.StepName,
                       LT.TaskType,
                       RT.MGUID AS ApproverGUID,
                       LT.Status,
                       LT.CreateTime,
                       LT.UpdateTime
                FROM(
                    SELECT @CaseID AS CaseID,
                           'Begin' AS StepName,
                           CID AS TaskType,
                           NULL AS ApproverGUID,
                           -1 AS Status,
                           @NowDT AS CreateTime,
                           @NowDT AS UpdateTime
                    FROM TB_SQM_Approver_Type
                    WHERE IsSpecfic = 'false'
                    UNION ALL
                    SELECT @CaseID AS CaseID,
                           'Begin' AS StepName,
                           CID AS TaskType,
                           NULL AS ApproverGUID,
                           -1 AS Status,
                           @NowDT AS CreateTime,
                           @NowDT AS UpdateTime
                    FROM TB_SQM_Approver_Type
                    WHERE IsSpecfic = 'true'
                      AND CID IN(
                             SELECT ACID
                             FROM TB_SQM_Approver_Type_Commodity_Map
                             WHERE CCID IN(
                                      SELECT TB_SQM_Commodity_SubTB_SQM_CommodityCID
                                      FROM TB_SQM_Manufacturers_BasicInfo
                                      WHERE BasicInfoGUID = @BasicInfoGUID ))) LT,
                    (
                    SELECT DISTINCT 1 AS CID,
                           SVR.SourcerGUID AS MGUID
                    FROM TB_SQM_Member_Vendor_Map MVM,
                         TB_SQM_Vendor_Related SVR
                    WHERE MVM.PlantCode = SVR.PlantCode
                      AND MVM.VendorCode = SVR.VendorCode
                    UNION ALL
                    SELECT DISTINCT 2 AS CID,
                           SVR.SourcerGUID AS MGUID
                    FROM TB_SQM_Member_Vendor_Map MVM,
                         TB_SQM_Vendor_Related SVR
                    WHERE MVM.PlantCode = SVR.PlantCode
                      AND MVM.VendorCode = SVR.VendorCode
                    UNION ALL
                    SELECT DISTINCT 3 AS CID,
                           SVR.SourcerGUID AS MGUID
                    FROM TB_SQM_Member_Vendor_Map MVM,
                         TB_SQM_Vendor_Related SVR
                    WHERE MVM.PlantCode = SVR.PlantCode
                      AND MVM.VendorCode = SVR.VendorCode
                    UNION ALL
                    SELECT DISTINCT 4 AS CID,
                           SVR.SourcerGUID AS MGUID
                    FROM TB_SQM_Member_Vendor_Map MVM,
                         TB_SQM_Vendor_Related SVR
                    WHERE MVM.PlantCode = SVR.PlantCode
                      AND MVM.VendorCode = SVR.VendorCode ) RT
                WHERE LT.TaskType = RT.CID;
				";
                DataTable dtBranchs = new DataTable();
                {

                    SqlCommand cmd = new SqlCommand(sSQL, cnPortal);
                    cmd.Parameters.AddWithValue("@BasicInfoGUID", DataItem.BasicInfoGUID);
                    cmd.Parameters.AddWithValue("@CaseID", sCaseID);
                    try
                    {
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            dtBranchs.Load(sdr);
                        }
                    }
                    catch (Exception e) { sErrMsg = "Check fail.<br />Exception: " + e.ToString(); return sErrMsg; }
                    cmd = null;
                }

                if (dtBranchs.Rows.Count == 0)
                {
                    return "there is no Branch finded";
                }
                for (int position = 0; position < dtBranchs.Rows.Count; position++)
                {
                    DataRow dr = dtBranchs.Rows[position];
                    String TaskID = CreateTask(cnPortal, dr["CaseID"].ToString(), dr["StepName"].ToString(), dr["TaskType"].ToString(), dr["ApproverGUID"].ToString(), dr["Status"].ToString());
                    fileNameNet = "";
                    sErrMsg += SendMailByTaskID(cnPortal, TaskID, fileNameNet, urlPre) + Environment.NewLine;
                    //錯誤需怎麼處理?
                }

                #endregion

                #region 更新主檔資料表
                {
                    String sCOL = "";
                    if (CaseType == "1")
                    {
                        sCOL = "ApproveCaseID";
                    }
                    else if (CaseType == "2")
                    {
                        sCOL = "ChooseCaseID";
                    }

                    sSQL = @"
					--更新主檔資料表
					UPDATE TB_SQM_Manufacturers_BasicInfo
					SET @Col=@CaseID
					WHERE BasicInfoGUID=@BasicInfoGUID
					--AND VendorCode=@VendorCode
					";
                    sSQL = Regex.Replace(sSQL, @"@Col", sCOL);
                    SqlCommand cmd = new SqlCommand(sSQL, cnPortal);
                    cmd.Parameters.AddWithValue("@BasicInfoGUID", DataItem.BasicInfoGUID);
                    //cmd.Parameters.AddWithValue("@VendorCode", DataItem.VendorCode);
                    cmd.Parameters.AddWithValue("@CaseID", sCaseID);

                    try
                    {
                        cmd.ExecuteNonQuery();

                    }
                    catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                    cmd = null;
                }
                #endregion

                return sErrMsg;
            }
        }


        public static String SendMailByTaskID(SqlConnection cnPortal, String TaskID, String fileNameNet, String urlPre)
        {
            #region 共用參數
            String sSQL = @"";
            String sFROM = @"";
            String sTO = @"";
            String sTONAME = @"";
            String sSubject = @"";
            #endregion

            #region 取得參數
            sSQL = @"
				DECLARE @CaseID NVARCHAR(50)
				DECLARE @TaskType bigint
				SELECT @CaseID = CaseID,@TaskType=TaskType FROM TB_SQM_Approve_Task WHERE TaskID = @TaskID
				
				SELECT TOP(2) ATA.TASKID,EU.EMAIL,ATA.TaskType
				FROM TB_SQM_Approve_Task ATA,
					 TB_SQM_Approver_Type ATY,
					 TB_EB_USER EU
				WHERE ATA.TaskType = @TaskType
				  AND Convert(nvarchar(50),ATA.ApproverGUID) = Convert(nvarchar(50),EU.USER_GUID)
				  AND ATA.CASEID = @CaseID
				Order By TaskID Desc
			";
            SqlCommand cmd = new SqlCommand(sSQL, cnPortal);
            cmd.Parameters.AddWithValue("@TaskID", TaskID);

            String sErrMsg = "";
            DataTable dt = new DataTable();
            try
            {
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    dt.Load(sdr);
                }
            }
            catch (Exception e) { sErrMsg = "SendMail fail.<br />Exception: " + e.ToString(); return sErrMsg; }
            cmd = null;
            DataRow dr = dt.Rows[0];
            if (dr["TaskType"].ToString() == "1")
            {
                sSubject = "SQM基本調查表簽核";
            }
            else
            {
                sSubject = "SQM供應商選定簽核";
            }

            if (dt.Rows.Count > 1)
            {
                sFROM = dt.Rows[1]["EMAIL"].ToString();
            }
            else
            {
                sFROM = "SQM@liteon.com.tw";
            }
            sTO = dr["EMAIL"].ToString();
            sTONAME = Regex.Replace(sTO, @"@liteon.com", "");
            try
            {
                sTONAME = sTONAME.Split('.')[0];
            }
            catch (Exception ex)
            {

            }
            #endregion

            #region 發送Mail
            StringBuilder sbBody = new StringBuilder();

            sbBody.Append("FROM:" + sFROM + " TO:" + sTO + "<br/>");

            sbBody.Append("Hello ");
            sbBody.Append(sTONAME);
            sbBody.Append("<br/>");
            sbBody.Append("You have received (an) " + sSubject + " request(s) from SQM for process.");
            sbBody.Append("<br/>");

            sbBody.Append("To approve or reject the request(s), please click this<a href='");
            sbBody.Append(urlPre);
            sbBody.Append("/SQM/Approve?TaskID=");
            sbBody.Append(desEncryptBase64(TaskID));
            sbBody.Append("'>簽核網址</a>");
            sbBody.Append("<br/>");
            sbBody.Append("");

            String[] aryFile = new string[] { fileNameNet };
            if (fileNameNet == "")
            {
                aryFile = new string[0];
            }
            icm045.CMSHandler MS = new icm045.CMSHandler();
            String result = MS.MailSend("SupplierPortal",
                    sFROM,//"SupplierPortal@liteon.com"
                    "JerryA.Chen@liteon.com",//sTO toDO
                    "",//toDO
                    "",
                    sSubject,//"SQM系統簽核"
                    sbBody.ToString(),
                    icm045.MailPriority.Normal,
                    icm045.MailFormat.Html,
                    aryFile);//fileName string[0]
            #endregion

            return sErrMsg;

        }


        public static String CheckApprover(SqlConnection cnPortal, SystemMgmt_PVL DataItem)
        {

            String sSQL = @"
				
				SELECT 
					   LT.TaskType,
					   RT.MGUID AS ApproverGUID,
					   SAT.CName
				FROM(
					SELECT CID AS TaskType
					FROM TB_SQM_Approver_Type
					WHERE IsSpecfic = 'false'
					UNION ALL
					SELECT CID AS TaskType
					FROM TB_SQM_Approver_Type
					WHERE IsSpecfic = 'true'
					  AND CID IN(
							 SELECT ACID
							 FROM TB_SQM_Approver_Type_Commodity_Map
							 WHERE CCID IN(
									  SELECT TB_SQM_Commodity_SubTB_SQM_CommodityCID
									  FROM TB_SQM_Manufacturers_BasicInfo
									  WHERE BasicInfoGUID = @BasicInfoGUID ))) LT,
					(
					SELECT 1 AS CID,
						   SVR.SourcerGUID AS MGUID
					FROM TB_SQM_Member_Vendor_Map MVM,
						 TB_SQM_Vendor_Related SVR
					WHERE MVM.PlantCode = SVR.PlantCode
					  AND MVM.VendorCode = SVR.VendorCode
					UNION ALL
					SELECT 2 AS CID,
						   SVR.SourcerGUID AS MGUID
					FROM TB_SQM_Member_Vendor_Map MVM,
						 TB_SQM_Vendor_Related SVR
					WHERE MVM.PlantCode = SVR.PlantCode
					  AND MVM.VendorCode = SVR.VendorCode
					UNION ALL
					SELECT 3 AS CID,
						   SVR.SourcerGUID AS MGUID
					FROM TB_SQM_Member_Vendor_Map MVM,
						 TB_SQM_Vendor_Related SVR
					WHERE MVM.PlantCode = SVR.PlantCode
					  AND MVM.VendorCode = SVR.VendorCode
					UNION ALL
					SELECT 4 AS CID,
						   SVR.SourcerGUID AS MGUID
					FROM TB_SQM_Member_Vendor_Map MVM,
						 TB_SQM_Vendor_Related SVR
					WHERE MVM.PlantCode = SVR.PlantCode
					  AND MVM.VendorCode = SVR.VendorCode ) RT,TB_SQM_Approver_Type SAT
				WHERE LT.TaskType = RT.CID
				AND LT.TaskType =SAT.CID;
				";
            SqlCommand cmd = new SqlCommand(sSQL, cnPortal);
            cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(DataItem.BasicInfoGUID));

            String sErrMsg = "";
            DataTable dt = new DataTable();
            try
            {
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    dt.Load(sdr);
                }
            }
            catch (Exception e) { sErrMsg = "Check fail.<br />Exception: " + e.ToString(); }
            cmd = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                if (dr["ApproverGUID"] == DBNull.Value)
                {
                    sErrMsg += dr["CNAME"] + "尚未設置";
                    if (i < dt.Rows.Count - 1)
                    {
                        sErrMsg += Environment.NewLine;
                    }
                }
            }
            return sErrMsg;

        }


        public static String CreateTask(SqlConnection cnPortal, String CaseID, String StepName, String TaskType, String ApproverGUID, String Status)
        {

            String sSQL = @"
				DECLARE @NowDT Datetime
				SELECT @NowDT=getDate()
				INSERT INTO TB_SQM_Approve_Task (CaseID,StepName,TaskType,ApproverGUID,Status,CreateTime,UpdateTime)
				SELECT @CaseID,@StepName,@TaskType,@ApproverGUID,@Status,@NowDT,@NowDT

				SELECT MAX(TaskID) AS TaskID FROM TB_SQM_Approve_Task WHERE CaseID=@CaseID AND StepName=@StepName AND CreateTime=@NowDT
			";
            SqlCommand cmd = new SqlCommand(sSQL, cnPortal);
            cmd.Parameters.AddWithValue("@CaseID", StringHelper.NullOrEmptyStringIsDBNull(CaseID));
            cmd.Parameters.AddWithValue("@StepName", StringHelper.NullOrEmptyStringIsDBNull(StepName));
            cmd.Parameters.AddWithValue("@TaskType", StringHelper.NullOrEmptyStringIsDBNull(TaskType));
            cmd.Parameters.AddWithValue("@ApproverGUID", StringHelper.NullOrEmptyStringIsDBNull(ApproverGUID));
            cmd.Parameters.AddWithValue("@Status", StringHelper.NullOrEmptyStringIsDBNull(Status));


            String sErrMsg = "";
            DataTable dt = new DataTable();
            try
            {
                return cmd.ExecuteScalar().ToString();
            }
            catch (Exception e) { sErrMsg = "Create Task Fail.<br />Exception: " + e.ToString(); }
            cmd = null;
            if (dt.Rows.Count > 0)
            {
                sErrMsg = "Already Exist";
            }
            return sErrMsg;

        }


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

        public static String desDecryptBase64(String encrypt)
        {
            try
            {
                encrypt = HttpUtility.HtmlDecode(encrypt);
                //encrypt = Server.UrlDecode(encrypt);
                encrypt = encrypt.Replace(".ADD.", ".+.");
                encrypt = encrypt.Replace(" ", "+");
                encrypt = encrypt.Replace(".", "");
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] key = Encoding.ASCII.GetBytes(sKEY);
                byte[] iv = Encoding.ASCII.GetBytes(sIV);
                des.Key = key;
                des.IV = iv;

                byte[] dataByteArray = Convert.FromBase64String(encrypt);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(dataByteArray, 0, dataByteArray.Length);
                        cs.FlushFinalBlock();
                        return Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return "";
        }

        #endregion
    }
}
