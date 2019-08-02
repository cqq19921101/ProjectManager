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
using Lib_SQM_Domain.SharedLibs;
using Lib_SQM_Domain.Model;

//using Lib_SQM_Domain.SharedLibs;
//using Lib_Portal_Domain.Model;

namespace Lib_SQM_Domain.Modal
{
    #region Data Class Definitions
    public class UpdateTaskData
    {
        public String TaskID { get; set; }

        public String Status { get; set; }

        public String Remark { get; set; }

        public String appName { get; set; }

    }

    public class ApproveStepObj
    {
        public String StepName { get; set; }

        public String ApproverGUID { get; set; }

        public String ApproverMail { get; set; }

    }

    public class Model_Org_Manager_List_Title
    {
        #region 屬性
        public string _Supervisor { get; set; }//课级主管
        public string _Vice_Manager { get; set; }//副理
        public string _Manager { get; set; }//經理
        public string _Director { get; set; }//處長
        public string _MD { get; set; }//厂长
        public string _BU_HEAD { get; set; }//事业部主管
        public string _BG_HEAD { get; set; }//事业群主管
        #endregion 屬性

        public Model_Org_Manager_List_Title()
        {
            _Supervisor = string.Empty;
            _Manager = string.Empty;
            _Director = string.Empty;
            _MD = string.Empty;
            _BU_HEAD = string.Empty;
            _BG_HEAD = string.Empty;
        }
    }

    public class Model_Org_Param
    {
        #region 屬性
        public string _DEPT_NO { get; set; }//GOBOOK 部门编码
        public string _LOGON_ID { get; set; }//GOBOOK 部门编码
        public int _DEPT_ID { get; set; }//GOBOOK 部门编码
        public int _ORG_LEVEL { get; set; }//GOBOOK 部门阶层
        #endregion 屬性

        public Model_Org_Param()
        {
            _DEPT_NO = string.Empty;
            _LOGON_ID = string.Empty;
            _ORG_LEVEL = 0;
            _DEPT_ID = 0;
        }

    }

    public class Model_Org_Info
    {
        #region 屬性
        public string _LOGON_ID { get; set; }//GOBOOK 部门编码
        public string _JOB { get; set; }//GOBOOK 部门编码
        #endregion 屬性

        public Model_Org_Info()
        {
            _LOGON_ID = string.Empty;
            _JOB = string.Empty;
        }

    }
    #endregion

    public static class SQM_Approve_Case_Helper
    {
        static String sKEY = "a@123456";
        static String sIV = "a@654321";

        #region Create/Edit data check
        private static void UnescapeDataFromWeb(TB_SQM_Manufacturers_BasicInfo DataItem)
        {
            DataItem.BasicInfoGUID = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.BasicInfoGUID);
            DataItem.VendorCode = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.VendorCode);
        }

        private static String DataCheck(TB_SQM_Manufacturers_BasicInfo DataItem)
        {
            String r = "";
            //List<string> e = new List<string>();
            //if (DataItem.TB_SQM_Vendor_TypeCID == 0)
            //{
            //	e.Add("Must choose Vendor Type.");
            //}
            //if (StringHelper.DataIsNullOrEmpty(DataItem.TB_SQM_Commodity_SubTB_SQM_CommodityCID))
            //{
            //	e.Add("Must choose Product Type.");
            //}
            //if (StringHelper.DataIsNullOrEmpty(DataItem.TB_SQM_Commodity_SubCID))
            //{
            //	e.Add("Must choose Sub Product Type.");
            //}


            //for (int iCnt = 0; iCnt < e.Count; iCnt++)
            //{
            //	if (iCnt > 0) r += "<br />";
            //	r += e[iCnt];
            //}

            return r;
        }
        #endregion

        #region Create Approve Case
        /// <summary>
        /// 創建簽核Case
        /// </summary>
        /// <param name="cnPortal"></param>
        /// <param name="DataItem"></param>
        /// <param name="fileName"></param>
        /// <param name="fileNameNet"></param>
        /// <param name="urlPre"></param>
        /// <param name="CaseType"></param>
        /// <returns></returns>
        public static String CreateApproveCase(SqlConnection cnPortal, TB_SQM_Manufacturers_BasicInfo DataItem, String fileName, String fileNameNet, String urlPre, String CaseType)
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
                       and MemberGUID= (select DISTINCT VendorCode from TB_SQM_Manufacturers_BasicInfo
					where BasicInfoGUID=@BasicInfoGUID)
                    UNION ALL
                    SELECT DISTINCT 2 AS CID,
                           SVR.SourcerGUID AS MGUID
                    FROM TB_SQM_Member_Vendor_Map MVM,
                         TB_SQM_Vendor_Related SVR
                    WHERE MVM.PlantCode = SVR.PlantCode
                      AND MVM.VendorCode = SVR.VendorCode
                       and MemberGUID= (select DISTINCT VendorCode from TB_SQM_Manufacturers_BasicInfo
					where BasicInfoGUID=@BasicInfoGUID)
                    UNION ALL
                    SELECT DISTINCT 3 AS CID,
                           SVR.SourcerGUID AS MGUID
                    FROM TB_SQM_Member_Vendor_Map MVM,
                         TB_SQM_Vendor_Related SVR
                    WHERE MVM.PlantCode = SVR.PlantCode
                      AND MVM.VendorCode = SVR.VendorCode
                      and MemberGUID= (select DISTINCT VendorCode from TB_SQM_Manufacturers_BasicInfo
					where BasicInfoGUID=@BasicInfoGUID)
                    UNION ALL
                    SELECT DISTINCT 4 AS CID,
                           SVR.SourcerGUID AS MGUID
                    FROM TB_SQM_Member_Vendor_Map MVM,
                         TB_SQM_Vendor_Related SVR
                    WHERE MVM.PlantCode = SVR.PlantCode
                      AND MVM.VendorCode = SVR.VendorCode 
                     and MemberGUID= (select DISTINCT VendorCode from TB_SQM_Manufacturers_BasicInfo
					where BasicInfoGUID=@BasicInfoGUID)) RT
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
                    sErrMsg += SendMailByTaskID(cnPortal, TaskID, fileNameNet, urlPre, CaseType);
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

        public static String CreateMRBCase(SqlConnection cnPortal,string MRBNo, string urlPre,string meettype)
        {
            #region 共用參數
            StringBuilder sSQL = new StringBuilder();
            string sErrMsg = string.Empty;
            string sCaseID = string.Empty;
            string r = string.Empty;
            #endregion
            r = CheckMRBApprover(cnPortal);
            if (r != "")
            { return r; }
            else
            {
               
                sSQL.Append( @"
				DECLARE @NowDT Datetime
				SELECT @NowDT=getDate()
				--Insert into CASE
				Insert Into TB_SQM_Approve_Case(FormNo,Type,Status,CreateTime,UpdateTime)
				Values (@BasicInfoGUID ,@Type,@Status,@NowDT,@NowDT);
				--取得 CaseID
				DECLARE @CaseID nvarchar(50)
				SELECT MAX(CASEID) as CaseID FROM TB_SQM_Approve_Case WHERE FormNo=@BasicInfoGUID 
				");

                {
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand(sSQL.ToString(), cnPortal);
                    cmd.Parameters.AddWithValue("@BasicInfoGUID", MRBNo);
                    cmd.Parameters.AddWithValue("@Type", 7);
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
            }
            #region 查询签核关卡
            sSQL = new StringBuilder();
            switch (meettype)
            {
                case "1":
                case "2":
                case "3":
                    sSQL.Append(@"
SELECT 
      [first]
      ,[next]
	  ,[Name]
      ,[Email]
      ,[Job]
  FROM [LiteOnRFQTraining].[dbo].[TB_SQM_MRB_APPOVE_DATA] a
  left join TB_SQM_MRB_APPOVE_MAP b on a.next=b.ssid
where first=-1 AND next<>17
            ");
                    break;
                case "4":
                case "5":
                    sSQL.Append(@"
SELECT 
      [first]
      ,[next]
	  ,[Name]
      ,[Email]
      ,[Job]
  FROM [LiteOnRFQTraining].[dbo].[TB_SQM_MRB_APPOVE_DATA] a
  left join TB_SQM_MRB_APPOVE_MAP b on a.next=b.ssid
where first=-1 and next=17
            ");
                    break;
               
            }
     
            DataTable dtBranchs = new DataTable();
            {

                SqlCommand cmd = new SqlCommand(sSQL.ToString(), cnPortal);
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
                String TaskID = CreateTask(cnPortal, sCaseID, dr["next"].ToString(), "7", "", "-1");
 
                sErrMsg = SendMRBMailByTaskID(cnPortal, TaskID, dr["Name"].ToString(), dr["Email"].ToString(), urlPre);
                //錯誤需怎麼處理?
            }
            #endregion

            #region 更新主檔資料表
            {
                sSQL = new StringBuilder();

                sSQL .Append(@"
					--更新主檔資料表
					UPDATE TB_SQM_MRB_REPORT
					SET CaseID=@CaseID
					WHERE MRBNo=@MRBNo
					
					");
           
                SqlCommand cmd = new SqlCommand(sSQL.ToString(), cnPortal);
                cmd.Parameters.AddWithValue("@MRBNo", MRBNo);
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

        private static string SendMRBMailByTaskID(SqlConnection cnPortal, string TaskID,string Name,string Email,string urlPre)
        {
            string sErrMsg = string.Empty;
            StringBuilder sbBody = new StringBuilder();
            sbBody.Append("Hello ");
            sbBody.Append(Name);
            sbBody.Append("<br/>");
            sbBody.Append("You have received (an) " + "MRB签核" + " request(s) from SQM for process.");
            sbBody.Append("<br/>");

            sbBody.Append("To approve or reject the request(s), please click this<a href='");
            sbBody.Append(urlPre);
            sbBody.Append("/MRBSign/MRBSign?TaskID=");
            sbBody.Append(desEncryptBase64(TaskID));
            sbBody.Append("'>簽核網址</a>");
            sbBody.Append("<br/>");
            sbBody.Append("");
            string sFROM = "SQM@liteon.com.tw";
            String[] aryFile = new string[0];

            icm045.CMSHandler MS = new icm045.CMSHandler();
            String result = MS.MailSend("SupplierPortal",
                    sFROM,//"SupplierPortal@liteon.com"
                     Email,//sTO toDO
                    "Aiden.Zeng@liteon.com;",//toDOJerryA.Chen@liteon.com;Aiden.Zeng@liteon.com;lily.guo@liteon.com
                    "",
                    "MRB签核",//"SQM系統簽核"
                    sbBody.ToString(),
                    icm045.MailPriority.Normal,
                    icm045.MailFormat.Html,
                    aryFile);//fileName string[0]
            #endregion

            return sErrMsg;

        }

        private static string CheckMRBApprover(SqlConnection cnPortal)
        {
            String sSQL = @"
			     select first,next FROM TB_SQM_MRB_APPOVE_DATA where first=-1
				";
            SqlCommand cmd = new SqlCommand(sSQL, cnPortal);
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
            if (dt.Rows.Count > 0)
            {
                sErrMsg = "";
            }
            else
            {
                sErrMsg = "未找到起始关卡";
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

        /// <summary>
        /// 檢查此類型是否存在
        /// </summary>
        /// <param name="cnPortal"></param>
        /// <param name="DataItem"></param>
        /// <returns></returns>
        public static String CheckExist(SqlConnection cnPortal, TB_SQM_Manufacturers_BasicInfo DataItem)
        {

            String sSQL = @"
				SELECT 'e'
				FROM TB_SQM_Manufacturers_BasicInfo
				WHERE VendorCode = @VendorCode
				AND TB_SQM_Vendor_TypeCID = @TB_SQM_Vendor_TypeCID
				AND TB_SQM_Commodity_SubTB_SQM_CommodityCID = @TB_SQM_Commodity_SubTB_SQM_CommodityCID
				AND TB_SQM_Commodity_SubCID = @TB_SQM_Commodity_SubCID
				";
            SqlCommand cmd = new SqlCommand(sSQL, cnPortal);
            cmd.Parameters.AddWithValue("@VendorCode", StringHelper.NullOrEmptyStringIsDBNull(DataItem.VendorCode));
            cmd.Parameters.AddWithValue("@TB_SQM_Vendor_TypeCID", DataItem.TB_SQM_Vendor_TypeCID);
            cmd.Parameters.AddWithValue("@TB_SQM_Commodity_SubTB_SQM_CommodityCID", StringHelper.NullOrEmptyStringIsDBNull(DataItem.TB_SQM_Commodity_SubTB_SQM_CommodityCID));
            cmd.Parameters.AddWithValue("@TB_SQM_Commodity_SubCID", StringHelper.NullOrEmptyStringIsDBNull(DataItem.TB_SQM_Commodity_SubCID));

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
            if (dt.Rows.Count > 0)
            {
                sErrMsg = "Already Exist";
            }
            return sErrMsg;

        }
        /// <summary>
        /// 檢查簽核人是否都已經設定
        /// </summary>
        /// <param name="cnPortal"></param>
        /// <param name="DataItem"></param>
        /// <returns></returns>
        public static String CheckApprover(SqlConnection cnPortal, TB_SQM_Manufacturers_BasicInfo DataItem)
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
        /// <summary>
        /// 發送郵件到簽核人
        /// </summary>
        /// <param name="cnPortal"></param>
        /// <param name="DataItem"></param>
        /// <returns></returns>
        public static String SendMailToApprove(SqlConnection cnPortal, TB_SQM_Manufacturers_BasicInfo DataItem, String fileName, String fileNameNet, String appName)
        {

            String sSQL = @"
				
				SELECT 
					   LT.TaskType,
					   RT.MGUID AS ApproverGUID,
					   SAT.CName,
					   EU.EMAIL
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
					,TB_EB_USER EU
				WHERE LT.TaskType = RT.CID
				AND LT.TaskType =SAT.CID
				AND CONVERT(NVARCHAR(50),RT.MGUID) =EU.USER_GUID;
				";

            sSQL = @"
				DECLARE @CaseID NVARCHAR(50)
				SELECT @CaseID = MAX(CASEID)
				FROM TB_SQM_Approve_Case
				WHERE FormNo = @BasicInfoGUID
				--------------------
				SELECT ATA.TASKID,EU.EMAIL
				FROM TB_SQM_Approve_Task ATA,
					 TB_SQM_Approver_Type ATY,
					 TB_EB_USER EU
				WHERE ATA.TaskType = ATY.CID
				  AND Convert(nvarchar(50),ATA.ApproverGUID) = Convert(nvarchar(50),EU.USER_GUID)
				  AND ATA.CASEID = @CaseID
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
            catch (Exception e) { sErrMsg = "SendMail fail.<br />Exception: " + e.ToString(); }
            cmd = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                //if (dr["EMAIL"] != DBNull.Value)
                //{
                //    icm045.CMSHandler MS = new icm045.CMSHandler();
                //    String result = MS.MailSend("SupplierPortal",
                //            "SupplierPortal@liteon.com",
                //            "jeter.sun@liteon.com;",//dr["EMAIL"].ToString()
                //            "",
                //            "jeter.sun@liteon.com",
                //            "SQM系統簽核",
                //            "Dear <br/> 請確認附件文檔內容，並且進入網址簽核。<br/> <a href='http://uat651/" + appName + "/SQM/Approve?TaskID=" + desEncryptBase64(dr["TASKID"].ToString()) + "'>簽核網址</a>",
                //            icm045.MailPriority.Normal,
                //            icm045.MailFormat.Html,
                //            new string[] { fileNameNet });//fileName string[0]
                //}
            }
            return sErrMsg;

        }

        public static String SendMailByTaskID(SqlConnection cnPortal, String TaskID, String fileNameNet, String urlPre, string type)
        {
            #region 共用參數
            String sSQL = @"";
            String sFROM = @"";
            String sTO = @"";
            String sTONAME = @"";
            String sSubject = @"";
            #endregion

            #region 取得參數
            if (type.Equals("3") || type.Equals("4"))
            {
                sSQL = @"
				--DECLARE @TaskID BIGINT
                --SET @TaskID = 123
                DECLARE @CaseID NVARCHAR(50)
                DECLARE @CaseType NVARCHAR(50)
                DECLARE @TaskType BIGINT
                -----------------------
                SELECT @CaseID = CaseID,
                       @TaskType = TaskType
                FROM TB_SQM_Approve_Task
                WHERE TaskID = @TaskID
                SELECT @CaseType = [Type]
                FROM TB_SQM_Approve_Case
                WHERE CaseID = @CaseID
                -----------------------
                SELECT TOP ( 1 ) ATA.TASKID,
                                 PM.PrimaryEmail,
                                 ATA.TaskType,
                                 ATA.CaseID,
                                 @CaseType AS CaseType
                FROM TB_SQM_Approve_Task ATA,
                     TB_SQM_Approver_Type ATY,
                     PORTAL_Members PM
                WHERE ATA.TaskType = @TaskType
                  AND CONVERT(NVARCHAR(50), ATA.ApproverGUID) = CONVERT(NVARCHAR(50), PM.MemberGUID)
                  AND ATA.CASEID = @CaseID
                  AND ATA.TaskType = ATY.CID
                ORDER BY TaskID DESC
			";
            }
            else
            {
                sSQL = @"
				--DECLARE @TaskID BIGINT
                --SET @TaskID = 123
                DECLARE @CaseID NVARCHAR(50)
                DECLARE @CaseType NVARCHAR(50)
                DECLARE @TaskType BIGINT
                -----------------------
                SELECT @CaseID = CaseID,
                       @TaskType = TaskType
                FROM TB_SQM_Approve_Task
                WHERE TaskID = @TaskID
                SELECT @CaseType = [Type]
                FROM TB_SQM_Approve_Case
                WHERE CaseID = @CaseID
                -----------------------
                SELECT TOP ( 2 ) ATA.TASKID,
                                 PM.PrimaryEmail,
                                 ATA.TaskType,
                                 ATA.CaseID,
                                 @CaseType AS CaseType
                FROM TB_SQM_Approve_Task ATA,
                     TB_SQM_Approver_Type ATY,
                     PORTAL_Members PM
                WHERE ATA.TaskType = @TaskType
                  AND CONVERT(NVARCHAR(50), ATA.ApproverGUID) = CONVERT(NVARCHAR(50), PM.MemberGUID)
                  AND ATA.CASEID = @CaseID
                  AND ATA.TaskType = ATY.CID
                ORDER BY TaskID DESC
			";
            }

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
            if (dr["CaseType"].ToString() == "1")
            {
                sSubject = "SQM基本調查表簽核";
            }
            else if (dr["CaseType"].ToString() == "2")
            {
                sSubject = "SQM供應商選定簽核";
            }
            else if (dr["CaseType"].ToString() == "3")
            {
                sSubject = "SQM供應商信賴性簽核";
            }
            else if (dr["CaseType"].ToString() == "4")
            {
                sSubject = "SQM供應商上傳可靠度報告簽核";
            }
            else if (dr["CaseType"].ToString() == "5" || dr["CaseType"].ToString() == "6")
            {
                sSubject = "SQM供應商變更通知單簽核";
            }

            if (dt.Rows.Count > 1)
            {
                sFROM = dt.Rows[1]["PrimaryEmail"].ToString();
            }
            else
            {
                sFROM = "SQM@liteon.com.tw";
            }
            sTO = dr["PrimaryEmail"].ToString();
            //sTO = "jeter.sun@liteon.com";
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

            //sbBody.Append("FROM:" + sFROM + " TO:" + sTO + "<br/>");

            sbBody.Append("Hello ");
            sbBody.Append(sTONAME);
            sbBody.Append("<br/>");
            sbBody.Append("You have received (an) " + sSubject + " request(s) from SQM for process.");
            sbBody.Append("<br/>");

            sbBody.Append("To approve or reject the request(s), please click this<a href='");
            sbBody.Append(urlPre);
            if (dr["CaseType"].ToString() == "1")
            {
                sbBody.Append("/SQM/Approve?TaskID=");
                sbBody.Append(desEncryptBase64(TaskID));
            }
            else if (dr["CaseType"].ToString() == "2")
            {
                sbBody.Append("/SQM/Approve?TaskID=");
                sbBody.Append(desEncryptBase64(TaskID));
            }
            else if (dr["CaseType"].ToString() == "3")
            {
                sbBody.Append("/SQM/ReliabilityApprove?TaskID=");
                sbBody.Append(desEncryptBase64(TaskID));
            }
            else if (dr["CaseType"].ToString() == "4")
            {
                sbBody.Append("/SQM/ReliabilityFileApprove?TaskID=");
                sbBody.Append(desEncryptBase64(TaskID));
            }
            else if (dr["CaseType"].ToString() == "5" || dr["CaseType"].ToString() == "6")
            {
                sbBody.Append("/SQM/ChangeApprove?TaskID=");
                sbBody.Append(desEncryptBase64(TaskID));
            }
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
                     sTO,//sTO toDO
                    "jeter.sun@liteon.com;Aiden.Zeng@liteon.com;",//toDOJerryA.Chen@liteon.com;Aiden.Zeng@liteon.com;lily.guo@liteon.com
                    "",
                    sSubject,//"SQM系統簽核"
                    sbBody.ToString(),
                    icm045.MailPriority.Normal,
                    icm045.MailFormat.Html,
                    aryFile);//fileName string[0]
            #endregion

            return sErrMsg;

        }
        public static DataTable GetReliabilityByTaskID(SqlConnection cn, SqlConnection cnSPM, String TaskID)
        {
            TaskID = SQM_Approve_Case_Helper.desDecryptBase64(TaskID);
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
			DECLARE @MemberGUID NVARCHAR(50)
			DECLARE @Status NVARCHAR(50)
            DECLARE @TaskStatus int
            SELECT @MemberGUID = FormNO,@Status=AC.Status,@TaskStatus = AT.Status
			FROM TB_SQM_Approve_Task AT,
				 TB_SQM_Approve_Case AC
			WHERE AT.TaskID = @TaskID
			  AND AC.CASEID = AT.CASEID;
         SELECT  NameInChinese,
            TB_SQM_Commodity.CNAME as CommodityName,TB_SQM_Commodity_Sub.CNAME as Commodity_SubName
            ,TestProjet ,PlannedTestTime,@Status as Status,@TaskStatus as TaskStatus
            FROM dbo.TB_SQM_Manufacturers_Reliability_Test
             inner JOIN TB_SQM_Commodity on TB_SQM_Manufacturers_Reliability_Test.TB_SQM_CommodityCID = TB_SQM_Commodity.CID
             inner JOIN TB_SQM_Commodity_Sub on TB_SQM_Manufacturers_Reliability_Test.TB_SQM_Commodity_SubCID = TB_SQM_Commodity_Sub.CID
			 left join (select top 1 SID,FGuid,ActualTestTime,[TestResult],[TestPeople],[Note] from TB_SQM_Manufacturers_Reliability_File  order by insertime desc )  a  on a.sid=TB_SQM_Manufacturers_Reliability_Test.ReliabititySID
			 inner join TB_SQM_Manufacturers_Reliability_Test_map on TB_SQM_Manufacturers_Reliability_Test_map.MemberGUID=TB_SQM_Manufacturers_Reliability_Test.ReliabititySID
             left join PORTAL_Members  on TB_SQM_Manufacturers_Reliability_Test.MemberGUID=PORTAL_Members.MemberGUID
            where TB_SQM_Manufacturers_Reliability_Test.ReliabititySID=@MemberGUID
			");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@TaskID", TaskID));
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    dt.Load(sdr);
                }
            }
            return dt;
            //JsonConvert.SerializeObject(dt);
        }
        public static DataTable GetReliabilityFileByTaskID(SqlConnection cn, SqlConnection cnSPM, String TaskID, string url)
        {
            TaskID = SQM_Approve_Case_Helper.desDecryptBase64(TaskID);
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
			DECLARE @MemberGUID NVARCHAR(50)
			DECLARE @Status NVARCHAR(50)
            DECLARE @TaskStatus int
            SELECT @MemberGUID = FormNO,@Status=AC.Status,@TaskStatus = AT.Status
			FROM TB_SQM_Approve_Task AT,
				 TB_SQM_Approve_Case AC
			WHERE AT.TaskID = @TaskID
			  AND AC.CASEID = AT.CASEID;
  select NameInChinese,
            TB_SQM_Commodity.CNAME as CommodityName,TB_SQM_Commodity_Sub.CNAME as Commodity_SubName
            ,TestProjet ,PlannedTestTime,ActualTestTime
             ,TestResult ,TestPeople,Note,@Status as Status,@TaskStatus as TaskStatus,
('<a href=""'+@url+convert(nvarchar(50), c.FGuid)+'"">' + FileName + '</a>') as FileName

   FROM dbo.TB_SQM_Manufacturers_Reliability_Test
             inner JOIN TB_SQM_Commodity on TB_SQM_Manufacturers_Reliability_Test.TB_SQM_CommodityCID = TB_SQM_Commodity.CID
             inner JOIN TB_SQM_Commodity_Sub on TB_SQM_Manufacturers_Reliability_Test.TB_SQM_Commodity_SubCID = TB_SQM_Commodity_Sub.CID
			 left join (select a.* 
 from [TB_SQM_Manufacturers_Reliability_File] a
 where not exists(select 1 
                  from [TB_SQM_Manufacturers_Reliability_File] b
                  where b.sid=a.sid and b.[insertime]>a.[insertime]) )  c  on c.sid=TB_SQM_Manufacturers_Reliability_Test.ReliabititySID
			 inner join TB_SQM_Manufacturers_Reliability_Test_map on TB_SQM_Manufacturers_Reliability_Test_map.MemberGUID=TB_SQM_Manufacturers_Reliability_Test.ReliabititySID
             left join PORTAL_Members  on TB_SQM_Manufacturers_Reliability_Test.MemberGUID=PORTAL_Members.MemberGUID
			 inner join [TB_SQM_Files]   on TB_SQM_Files.FGUID=c.FGuid
             where c.FSID=@MemberGUID
			");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@TaskID", TaskID));
                cmd.Parameters.Add(new SqlParameter("@url", url));
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    dt.Load(sdr);
                }
            }
            return dt;
            //JsonConvert.SerializeObject(dt);
        }
        public static DataTable GetChangeByTaskID(SqlConnection cn, SqlConnection cnSPM, string TaskID, string url)
        {
            TaskID = SQM_Approve_Case_Helper.desDecryptBase64(TaskID);
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
			DECLARE @MemberGUID NVARCHAR(50)
			DECLARE @Status NVARCHAR(50)
	        DECLARE @CASEID NVARCHAR(50)
            DECLARE @TaskStatus int
            SELECT @MemberGUID = FormNO,@Status=AC.Status,@TaskStatus = AT.Status,@CASEID=AT.CASEID
			FROM TB_SQM_Approve_Task AT,
				 TB_SQM_Approve_Case AC
			WHERE AT.TaskID = @TaskID
			  AND AC.CASEID = AT.CASEID;
       SELECT [SID]
      ,[TZDNo]
      ,[MemberGUID]
      ,[Supplier]
      ,[Phone]
      ,[OriginatorName]
      ,[Spec]
      ,[Description]
      ,[ChangeItemType]
      ,[ChangeType]
      ,[ProposedChange]
      ,[ProposedDate]
      ,[ChangeReason]
      ,[DesignChange]
      ,[Consume]
      ,[Scrap]
      ,[Rework]
      ,[Sort]
      ,[WIP]
      ,[QtyInStock]
      ,[Environment]
      ,[PPAP]
      ,( '<a href=""'+@url+ CONVERT( NVARCHAR(50), SupplierApprovalFGUID) + '"">' + T3.FileName + '</a>' ) AS SupplierApproval
      ,[Title]
      ,[RequestDate]
      ,[CaseID],@Status as Status,@TaskStatus as TaskStatus
   FROM [TB_SQM_Supplier_Change]
  LEFT OUTER JOIN TB_SQM_Files T3 ON SupplierApprovalFGUID = T3.FGUID
   where  MemberGUID=@MemberGUID and CASEID=@CASEID
			");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@TaskID", TaskID));
                cmd.Parameters.Add(new SqlParameter("@url", url));
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    dt.Load(sdr);
                }
            }
            return dt;

        }
        public static DataTable GetBasicInfoTypeByTaskID(SqlConnection cn, SqlConnection cnSPM, String TaskID)
        {
            TaskID = SQM_Approve_Case_Helper.desDecryptBase64(TaskID);
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
			DECLARE @BasicInfoGUID NVARCHAR(50)
			DECLARE @Status NVARCHAR(50)
            DECLARE @TaskStatus int
            SELECT @BasicInfoGUID = FormNO,@Status=AC.Status,@TaskStatus = AT.Status
			FROM TB_SQM_Approve_Task AT,
				 TB_SQM_Approve_Case AC
			WHERE AT.TaskID = @TaskID
			  AND AC.CASEID = AT.CASEID

			SELECT BasicInfoGUID,
				   BI.VendorCode AS MGUID,
				   TB_SQM_Vendor_TypeCID,
				   VT.CNAME AS VTNAME,
				   TB_SQM_Commodity_SubCID,
				   C.CNAME AS CNAME,
				   TB_SQM_Commodity_SubTB_SQM_CommodityCID,
				   CS.CNAME AS CSNAME,
				   @Status AS Status,
                   @TaskStatus AS TaskStatus,
                   AC.Type,
				   PM.AccountID,
				   PM.NameInChinese,
				   VM.PlantCode,
				   VM.VendorCode,
                   BI.FGUID
			FROM TB_SQM_Manufacturers_BasicInfo BI
				 LEFT OUTER JOIN TB_SQM_Approve_Case AC ON BI.ApproveCaseID = AC.CaseID,
				 TB_SQM_Vendor_Type VT,
				 TB_SQM_Commodity_Sub CS,
				 TB_SQM_Commodity C,
				 PORTAL_Members PM,
				 TB_SQM_Member_Vendor_Map VM
			WHERE BI.TB_SQM_Vendor_TypeCID = VT.CID
			  AND BI.TB_SQM_Commodity_SubTB_SQM_CommodityCID = C.CID
			  AND ( BI.TB_SQM_Commodity_SubTB_SQM_CommodityCID = CS.TB_SQM_CommodityCID
				AND BI.TB_SQM_Commodity_SubCID = CS.CID
				  )
			  AND BasicInfoGUID = @BasicInfoGUID
			  AND CONVERT(NVARCHAR(50), BI.VendorCode) = CONVERT(NVARCHAR(50), PM.MemberGUID)
			  AND CONVERT(NVARCHAR(50), BI.VendorCode) = CONVERT(NVARCHAR(50), VM.MemberGUID)
			ORDER BY CreateDatetime
			");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@TaskID", TaskID));
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    dt.Load(sdr);
                }
            }
            return dt;
            //JsonConvert.SerializeObject(dt);
        }

        public static String UpdateTaskStatus(SqlConnection cn, SqlConnection cnSPM, UpdateTaskData DataItem)
        {
            #region 共用參數宣告
            String sSQL = "";
            String sErrMsg = "";
            String CaseID = "";
            String StepName = "";
            String TaskType = "";
            String CaseType = "";
            String NowApproverLOGONID = "";
            String NowApproverGUID = "";
            String nextStepName = "";
            #endregion

            #region 取得資訊
            {
                DataItem.TaskID = SQM_Approve_Case_Helper.desDecryptBase64(DataItem.TaskID);
                StringBuilder sb = new StringBuilder();
                sSQL = @"
			    SELECT AT.CaseID,
				       StepName,
				       TaskType,
				      AC.Type as CaseType,
				      PM.AccountID,PM.MemberGUID
			    FROM TB_SQM_Approve_Task AT,
				     TB_SQM_Approve_Case AC,
				     PORTAL_MEMBERS PM
			    WHERE AT.CASEID = AC.CASEID
			      AND AT.TaskID = @TaskID
			      AND Convert(nvarchar(50),AT.ApproverGUID) = Convert(nvarchar(50),PM.MemberGUID)
			    ";
                DataTable dt = new DataTable();
                {
                    using (SqlCommand cmd = new SqlCommand(sSQL, cn))
                    {
                        //cmd.Parameters.Add(new SqlParameter("@TaskID", DataItem.TaskID));
                        cmd.Parameters.AddWithValue("@TaskID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TaskID));
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            dt.Load(sdr);
                        }
                    }
                }



                DataRow dr = dt.Rows[0];
                CaseID = dr["CaseID"].ToString();
                StepName = dr["StepName"].ToString();
                TaskType = dr["TaskType"].ToString();
                CaseType = dr["CaseType"].ToString();
                NowApproverLOGONID = Regex.Replace(dr["AccountID"].ToString(), @"liteon\\", "");
                NowApproverGUID = dr["MemberGUID"].ToString();
            }
            #endregion

            {
                String status = "";
                if (DataItem.Status == "Approve")
                {
                    status = "0";
                }
                else
                {
                    status = "1";
                }
                sSQL = @"
				UPDATE TB_SQM_Approve_Task SET Status =@Status,Remark=@Remark WHERE TaskID=@TaskID;
				";
                SqlCommand cmd = new SqlCommand(sSQL, cn);
                cmd.Parameters.AddWithValue("@TaskID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TaskID));
                cmd.Parameters.AddWithValue("@Remark", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Remark));
                cmd.Parameters.AddWithValue("@Status", SQMStringHelper.NullOrEmptyStringIsDBNull(status));
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;
            }


            #region 取得下一步
            if (1 == 2)//依照Approve or Reject做對應處理 DataItem.Status == "Reject"
            {
                //插入End
                CreateTask(cn, CaseID, "End", TaskType, "", "1");

            }
            else
            {
                #region 取得下一步驟流程 依照CaseType有不同的Process
                if (DataItem.Status == "Reject")
                {
                    nextStepName = "End";
                }
                else
                {
                    switch (CaseType)
                    {
                        case "1":
                            {
                                if (StepName == "Begin")
                                {
                                    nextStepName = "1";
                                }
                                else if (StepName == "1")
                                {
                                    nextStepName = "2";
                                }
                                //else if (StepName == "2")
                                //{
                                //    nextStepName = "3";
                                //}
                                else if (StepName == "2")
                                {
                                    nextStepName = "End";
                                }
                                break;
                            }
                        case "2":
                            {
                                if (StepName == "Begin")
                                {
                                    nextStepName = "1";
                                }
                                else if (StepName == "1")
                                {
                                    nextStepName = "2";
                                }
                                else if (StepName == "2")
                                {
                                    nextStepName = "3";
                                }
                                //else if (StepName == "3")
                                //{
                                //    nextStepName = "4";
                                //}
                                else if (StepName == "3")
                                {
                                    nextStepName = "End";
                                }

                                if (TaskType == "0")//最後總簽核人
                                {
                                    nextStepName = "End";
                                }
                                break;
                            }
                        case "3":
                            {
                                if (StepName == "Begin")
                                {
                                    nextStepName = "1";
                                }
                                else if (StepName == "1")
                                {

                                    nextStepName = "2";
                                }
                                else if (StepName == "2")
                                {
                                    nextStepName = "End";
                                }


                                break;
                            }

                        case "4":
                            {
                                if (StepName == "Begin")
                                {
                                    nextStepName = "1";
                                }
                                else if (StepName == "1")
                                {

                                    nextStepName = "2";
                                }
                                else if (StepName == "2")
                                {
                                    nextStepName = "End";
                                }
                                break;
                            }
                        case "5":
                            {
                                if (StepName == "Begin")
                                {
                                    nextStepName = "1";
                                }
                                else if (StepName == "1")
                                {

                                    nextStepName = "2";
                                }
                                else if (StepName == "2")
                                {
                                    nextStepName = "End";
                                }
                                break;
                            }
                        default:
                            break;
                    }
                }
                #endregion



                String ApproverGUID = NowApproverGUID;
                #region 依造下一步流程取得對應簽核人的GUID
                if (nextStepName != "End")
                {
                    //toDo
                    ApproveStepObj aso = getNextApproverObj(cn, cnSPM, NowApproverLOGONID, nextStepName);
                    //ApproverGUID = aso.ApproverGUID;
                    nextStepName = aso.StepName;
                    ApproverGUID = @"F3E6AAD0-3E6F-40AB-B08A-04A0E83B8CA2";//jeter todo
                    //B5380729-6BB7-4F37-BFEE-01E646EEEA93 //Jerry

                }
                #endregion

                #region 插入簽核Task,並且發送Mail
                String nextStatus = "-1";
                String nextTaskID = string.Empty;
                if (nextStepName == "End")
                {
                    nextStatus = DataItem.Status == "Reject" ? "1" : "0";
                }
                nextTaskID = CreateTask(cn, CaseID, nextStepName, TaskType, ApproverGUID, nextStatus);
                if (nextStepName != "End")
                {
                    //發Mail toDo
                    String fileNameNet = "";
                    SendMailByTaskID(cn, nextTaskID, fileNameNet, DataItem.appName, CaseType);
                    #endregion
                }
                if (nextStepName == "End")
                {
                    if (CaseType.Equals("5"))
                    {
                        nextTaskID = CreateTask(cn, CaseID, "begin", "1", ApproverGUID, "1");
                        String fileNameNet = "";
                        SendMailByTaskID(cn, nextTaskID, fileNameNet, DataItem.appName, CaseType);
                    }
                    else if (CaseType.Equals("6"))
                    {
                        nextTaskID = CreateTask(cn, CaseID, "begin", "3", ApproverGUID, "1");
                        String fileNameNet = "";
                        SendMailByTaskID(cn, nextTaskID, fileNameNet, DataItem.appName, CaseType);
                    }
                }
            }
            #endregion

            #region forCopy
            #endregion

            #region 檢查Case簽核狀態,決定Reject,Fininsh或是送至BU HEAD簽核

            if (1 == 2)//DataItem.Status == "Reject" && CaseType=="1"
            {
                sSQL = @"
				UPDATE TB_SQM_Approve_Case SET Status ='Reject' WHERE CaseID=@CaseID;
				";
                SqlCommand cmd = new SqlCommand(sSQL, cn);
                cmd.Parameters.AddWithValue("@CaseID", CaseID);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e) { sErrMsg = "Create Case fail.<br />Exception: " + e.ToString(); return sErrMsg; }
                cmd = null;
                return "";
            }

            //調查表只要有人Reject,專案就Reject
            //調查表全數簽核完成時,更新Case為Finished
            //toDO
            if (1 == 1)//DataItem.Status == "Approve" && CaseType == "1"
            {
                sSQL = @"
				--DECLARE @CaseID int
				--SET @CaseID = 32

				DECLARE @CaseType int
				SELECT @CaseType=Type FROM TB_SQM_Approve_Case
				WHERE CaseID = @CaseID

				DECLARE @EndApproveCount int
				SELECT @EndApproveCount=COUNT(1) FROM TB_SQM_Approve_Task
				WHERE CaseID = @CaseID
				AND StepName='End' AND Status = 0

				DECLARE @EndRejectCount int
				SELECT @EndRejectCount=COUNT(1) FROM TB_SQM_Approve_Task
				WHERE CaseID = @CaseID
				AND StepName='End' AND Status = 1

	

				DECLARE @Target int
				SELECT @Target=COUNT(1) FROM (
				SELECT DISTINCT  TaskType
				FROM TB_SQM_Approve_Task
				WHERE CASEID = @CaseID) A

				--bu head關卡
				DECLARE @finalEndCount int
				SEt @finalEndCount = 0

				SELECT @finalEndCount=COUNT(1) FROM TB_SQM_Approve_Task
				WHERE CaseID = @CaseID
				AND StepName='End' AND TaskType = 0

				--SELECT @EndApproveCount,@EndRejectCount,@Target,@finalEndCount
				if (@CaseType = 1)--調查表
					BEGIN
						if (@Target = 0)
							BEGIN
							SELECT 2 AS Result--Pending
							END
						ELSE if (@EndRejectCount >= 1)
							BEGIN
							    SELECT 1 AS Result--Reject
                                UPDATE TB_SQM_Approve_Case
							    SET [Status] = 'Reject'
							    WHERE CASEID = @CaseID
							END
						ELSE if (@EndApproveCount >= @Target)
							BEGIN
							    SELECT 0 AS Result--Approve
							    UPDATE TB_SQM_Approve_Case
							    SET [Status] = 'Finished'
							    WHERE @EndApproveCount >= @Target
							    AND CASEID = @CaseID
							END
						ELSE
							BEGIN
							SELECT 2 AS Result--Pending
							END
					END
				ELSE IF (@CaseType = 2)--AVL PVL
					BEGIN
						if (@finalEndCount > 0)--BUHEAD 已簽核完成
							BEGIN
							DECLARE @finalStatus int
							SET @finalStatus = -1
							SELECT top 1 @finalStatus=isnull(Status,-1) FROM TB_SQM_Approve_Task
							WHERE CaseID = @CaseID
							AND StepName='End' AND TaskType = 0
							if @finalStatus = 0 
								BEGIN
									SELECT 0 AS Result--Approve
									UPDATE TB_SQM_Approve_Case
									SET [Status] = 'Finished'
									WHERE @EndApproveCount >= @Target
									AND CASEID = @CaseID;
                                    UPDATE TB_SQM_Manufacturers_BasicInfo
                                    SET IsChoose='true'
                                    WHERE CONVERT(NVARCHAR(50),BasicInfoGUID)=(SELECT FORMNO FROM TB_SQM_Approve_Case WHERE CASEID = @CaseID) 
								END
							ELSE
								BEGIN
									SELECT 1 AS Result--Reject
									UPDATE TB_SQM_Approve_Case
									SET [Status] = 'Reject'
									WHERE @EndApproveCount >= @Target
									AND CASEID = @CaseID;
                                    UPDATE TB_SQM_Manufacturers_BasicInfo
                                    SET IsChoose='false'
                                    WHERE CONVERT(NVARCHAR(50),BasicInfoGUID)=(SELECT FORMNO FROM TB_SQM_Approve_Case WHERE CASEID = @CaseID) 
								END
							END
						ELSE if (@Target = 0)--異常
							BEGIN
							SELECT 2 AS Result--Pending
							END
						ELSE if (@EndRejectCount >= 1 AND (@EndRejectCount+@EndApproveCount)=@Target)
							BEGIN
							SELECT 3 AS Result--Next 部門意見不一致,等最後md簽核
							END
						ELSE if (@EndApproveCount >= @Target)--全數同意 Finished
							BEGIN
							    SELECT 0 AS Result--Approve
							    UPDATE TB_SQM_Approve_Case
							    SET [Status] = 'Finished'
							    WHERE @EndApproveCount >= @Target
							    AND CASEID = @CaseID;
                                UPDATE TB_SQM_Manufacturers_BasicInfo
                                SET IsChoose='true'
                                WHERE CONVERT(NVARCHAR(50),BasicInfoGUID)=(SELECT FORMNO FROM TB_SQM_Approve_Case WHERE CASEID = @CaseID) 
							END
						ELSE
							BEGIN
							SELECT 2 AS Result--Pending
							END
					END
ELSE IF (@CaseType = 3)
					BEGIN
						if (@Target = 0)
							BEGIN
							SELECT 2 AS Result--Pending
							END
						ELSE if (@EndRejectCount >= 1)
							BEGIN
							    SELECT 1 AS Result--Reject
                                UPDATE TB_SQM_Approve_Case
							    SET [Status] = 'Reject'
							    WHERE CASEID = @CaseID
							END
						ELSE if (@EndApproveCount >= @Target)
							BEGIN
							    SELECT 0 AS Result--Approve
							    UPDATE TB_SQM_Approve_Case
							    SET [Status] = 'Finished'
							    WHERE @EndApproveCount >= @Target
							    AND CASEID = @CaseID
							END
						ELSE
							BEGIN
							SELECT 2 AS Result--Pending
							END
					END
					ELSE IF (@CaseType = 4)
					BEGIN
						if (@Target = 0)
							BEGIN
							SELECT 2 AS Result--Pending
							END
						ELSE if (@EndRejectCount >= 1)
							BEGIN
							    SELECT 1 AS Result--Reject
                                UPDATE TB_SQM_Approve_Case
							    SET [Status] = 'Reject'
							    WHERE CASEID = @CaseID
							END
						ELSE if (@EndApproveCount >= @Target)
							BEGIN
							    SELECT 0 AS Result--Approve
							    UPDATE TB_SQM_Approve_Case
							    SET [Status] = 'Finished'
							    WHERE @EndApproveCount >= @Target
							    AND CASEID = @CaseID
							END
						ELSE
							BEGIN
							SELECT 2 AS Result--Pending
							END
END
	ELSE IF (@CaseType = 5)
					BEGIN
						if (@Target = 0)
							BEGIN
							SELECT 2 AS Result--Pending
							END
						ELSE if (@EndRejectCount >= 1)
							BEGIN
							    SELECT 1 AS Result--Reject
                                UPDATE TB_SQM_Approve_Case
							    SET [Status] = 'Reject'
							    WHERE CASEID = @CaseID
							END
						ELSE if (@EndApproveCount >= @Target)
							BEGIN
							    SELECT 0 AS Result--Approve
							    UPDATE TB_SQM_Approve_Case
							    SET [Status] = 'Finished'
							    WHERE @EndApproveCount >= @Target
							    AND CASEID = @CaseID
							END
						ELSE
							BEGIN
							SELECT 2 AS Result--Pending
							END
END
	ELSE IF (@CaseType = 6)
					BEGIN
						if (@Target = 0)
							BEGIN
							SELECT 2 AS Result--Pending
							END
						ELSE if (@EndRejectCount >= 1)
							BEGIN
							    SELECT 1 AS Result--Reject
                                UPDATE TB_SQM_Approve_Case
							    SET [Status] = 'Reject'
							    WHERE CASEID = @CaseID
							END
						ELSE if (@EndApproveCount >= @Target)
							BEGIN
							    SELECT 0 AS Result--Approve
							    UPDATE TB_SQM_Approve_Case
							    SET [Status] = 'Finished'
							    WHERE @EndApproveCount >= @Target
							    AND CASEID = @CaseID
							END
						ELSE
							BEGIN
							SELECT 2 AS Result--Pending
							END
					END
				";
                DataTable dt = new DataTable();
                {
                    {
                        using (SqlCommand cmd = new SqlCommand(sSQL, cn))
                        {
                            cmd.Parameters.Add(new SqlParameter("@CaseID", CaseID));
                            using (SqlDataReader sdr = cmd.ExecuteReader())
                            {
                                dt.Load(sdr);
                            }
                        }
                    }
                }

                DataRow dr = dt.Rows[0];
                if (dr["Result"].ToString() == "3")//0 Approve,1 Reject,2 Pending,3 next
                {
                    String ApproverGUID = "";
                    ApproveStepObj aso = getNextApproverObj(cn, cnSPM, NowApproverLOGONID, nextStepName);
                    //ApproverGUID = aso.ApproverGUID;
                    //nextStepName = aso.StepName;
                    ApproverGUID = @"B5380729-6BB7-4F37-BFEE-01E646EEEA93";//發Mail

                    nextStepName = "End";
                    String nextStatus = "-1";
                    String nextTaskID = CreateTask(cn, CaseID, nextStepName, TaskType, ApproverGUID, nextStatus);

                    //發Mail toDo
                    String fileNameNet = "";
                    SendMailByTaskID(cn, nextTaskID, fileNameNet, DataItem.appName, CaseType);
                }
            }



            #endregion

            return "";
            //JsonConvert.SerializeObject(dt);
        }

        //toDo
        private static ApproveStepObj getNextApproverObj(SqlConnection cn, SqlConnection cnSPM, string nowApproverLOGONID, String stepName)
        {
            ApproveStepObj aso = new ApproveStepObj();

            #region 共用參數宣告
            String sSQL = "";
            String sErrMsg = "";
            String CaseID = "";
            String sDeptID = "";
            String sLogonID = "";
            String sMail = "";
            String ApproverGUID = "";
            #endregion


            //取得DEPTID,EMAIL
            {
                sSQL = @"
                SELECT TOP 1 DEPTID,
                             U.EMAIL
                FROM [USER] U,
                     DEPTUSER DU
                WHERE U.USERID = DU.USERID
                  AND U.LOGONID = @LOGONID
			    ";
                DataTable dt = new DataTable();
                {

                    using (SqlCommand cmd = new SqlCommand(sSQL, cnSPM))//注意是SPM
                    {
                        cmd.Parameters.Add(new SqlParameter("@LOGONID", nowApproverLOGONID));
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            dt.Load(sdr);
                        }
                    }
                    sDeptID = dt.Rows[0]["DEPTID"].ToString();
                    sMail = dt.Rows[0]["EMAIL"].ToString();
                }
            }

            //透過DEPTID取得結構
            Model_Org_Manager_List_Title cl = GetManagerList_ByTitle(cnSPM, sDeptID);
            if (stepName == "1")//副理
            {
                if (cl._Vice_Manager != null && cl._Vice_Manager != "" && cl._Vice_Manager != "null")
                {
                    sLogonID = cl._Vice_Manager;
                }
                else if (cl._Manager != null && cl._Manager != "null" && cl._Manager != "")
                {
                    stepName = "2";
                    sLogonID = cl._Manager;
                }
            }
            else if (stepName == "2")//經理
            {
                if (cl._Manager != null && cl._Manager != "null" && cl._Manager != "")
                {
                    sLogonID = cl._Manager;
                }
            }


            //透過sLogonID取得MEMBER GUID,跟MAIL
            {
                sSQL = @"
                SELECT TOP 1 DEPTID,
                             U.EMAIL
                FROM [USER] U,
                     DEPTUSER DU
                WHERE U.USERID = DU.USERID
                  AND U.LOGONID = @LOGONID
			    ";
                DataTable dt = new DataTable();
                {

                    using (SqlCommand cmd = new SqlCommand(sSQL, cnSPM))//注意是SPM
                    {
                        cmd.Parameters.Add(new SqlParameter("@LOGONID", sLogonID));
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            dt.Load(sdr);
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        sMail = dt.Rows[0]["EMAIL"].ToString();
                    }

                }
            }
            //
            {
                sSQL = @"
                SELECT MemberGUID,PrimaryEmail
                FROM PORTAL_Members
                WHERE Replace(AccountID,'liteon\','')=@LOGONID
			    ";
                DataTable dt = new DataTable();
                {

                    using (SqlCommand cmd = new SqlCommand(sSQL, cn))//注意是SPM
                    {
                        cmd.Parameters.Add(new SqlParameter("@LOGONID", sLogonID));
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            dt.Load(sdr);
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ApproverGUID = dt.Rows[0]["MemberGUID"].ToString();
                        sMail = dt.Rows[0]["PrimaryEmail"].ToString();
                    }

                }
            }


            aso.ApproverGUID = ApproverGUID;
            aso.ApproverGUID = @"B5380729-6BB7-4F37-BFEE-01E646EEEA93";//toDO
            aso.StepName = stepName;
            aso.ApproverMail = sMail;

            return aso;

        }

        public static string UpdateMRBStatus(SqlConnection cn,SQMMRBData dataItem,string StepName,string Task,string urlPre)
        {
           StringBuilder sSQL = new StringBuilder();
            string TaskID = SQM_Approve_Case_Helper.desDecryptBase64(Task);
            String sErrMsg = "";
            sSQL.Append(@"
SELECT CaseID FROM TB_SQM_Approve_Task
where TaskID=@TaskID 
            ");
            DataTable dt = new DataTable();
            {
                using (SqlCommand cmd1 = new SqlCommand(sSQL.ToString(), cn))
                {
                    //cmd.Parameters.Add(new SqlParameter("@TaskID", DataItem.TaskID));
                    cmd1.Parameters.AddWithValue("@TaskID", SQMStringHelper.NullOrEmptyStringIsDBNull(TaskID));
                    using (SqlDataReader sdr = cmd1.ExecuteReader())
                    {
                        dt.Load(sdr);
                    }
                }
            }
            DataRow dr = dt.Rows[0];
            string CaseID = dr["CaseID"].ToString();
            sSQL = new StringBuilder();
            sSQL.Append( @"
				UPDATE TB_SQM_Approve_Task SET Status =0 WHERE TaskID=@TaskID;
				");
            SqlCommand cmd = new SqlCommand(sSQL.ToString(), cn);
            cmd.Parameters.AddWithValue("@TaskID", SQMStringHelper.NullOrEmptyStringIsDBNull(TaskID));
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
            cmd = null;

            sSQL = new StringBuilder();
            sSQL.Append(@"
SELECT 
      [first]
      ,[next]
	  ,[Name]
      ,[Email]
      ,[Job]
  FROM [LiteOnRFQTraining].[dbo].[TB_SQM_MRB_APPOVE_DATA] a
  left join TB_SQM_MRB_APPOVE_MAP b on a.next=b.ssid
where first=@first 
            ");
            DataTable dtBranchs = new DataTable();
            {

                cmd = new SqlCommand(sSQL.ToString(), cn);
                cmd.Parameters.AddWithValue("@first", SQMStringHelper.NullOrEmptyStringIsDBNull(StepName));
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
                DataRow dr1 = dtBranchs.Rows[position];
                if (dr1["next"].ToString().Equals("-2"))
                {
                    CreateTask(cn, CaseID, "End", "7", "", "-1");
                }
                else
                {
                    string nextTaskID = CreateTask(cn, CaseID, dr1["next"].ToString(), "7", "", "-1");
                    sErrMsg = SendMRBMailByTaskID(cn, TaskID, dr["Name"].ToString(), dr["Email"].ToString(), urlPre);
                }
               
                //錯誤需怎麼處理?
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

        #region Edit data item
        public static String EditDataItem(SqlConnection cnPortal, TB_SQM_Manufacturers_BasicInfo DataItem)
        {
            return EditDataItem(cnPortal, DataItem, "", "");
        }

        public static String EditDataItem(SqlConnection cnPortal, TB_SQM_Manufacturers_BasicInfo DataItem, String LoginMemberGUID, String RunAsMemberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            String r = DataCheck(DataItem);
            r = CheckExist(cnPortal, DataItem);
            if (r != "")
            { return r; }
            else
            {
                SqlTransaction tran = cnPortal.BeginTransaction();

                //Update member data
                using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = EditDataItemSub(cmd, DataItem); }
                if (r != "") { tran.Rollback(); return r; }

                //Check lock is still valid
                bool bLockIsStillValid = false;
                if ((LoginMemberGUID != "") && (RunAsMemberGUID != ""))
                    using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { bLockIsStillValid = DataLockHelper.CheckLockIsStillValid(cmd, DataItem.BasicInfoGUID, LoginMemberGUID, RunAsMemberGUID); }
                if (!bLockIsStillValid) { tran.Rollback(); return DataLockHelper.LockString(); }

                //Release lock
                if ((LoginMemberGUID != "") && (RunAsMemberGUID != ""))
                    using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = DataLockHelper.ReleaseLock(cmd, DataItem.BasicInfoGUID, LoginMemberGUID, RunAsMemberGUID); }
                if (r != "") { tran.Rollback(); return r; }

                //Commit
                try { tran.Commit(); }
                catch (Exception e) { tran.Rollback(); r = "Edit fail.<br />Exception: " + e.ToString(); }
                return r;
            }
        }

        private static String EditDataItemSub(SqlCommand cmd, TB_SQM_Manufacturers_BasicInfo DataItem)
        {
            String sErrMsg = "";
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append(@"
			Update TB_SQM_Manufacturers_BasicInfo 
			Set TB_SQM_Vendor_TypeCID = @TB_SQM_Vendor_TypeCID 
			,TB_SQM_Commodity_SubTB_SQM_CommodityCID = @TB_SQM_Commodity_SubTB_SQM_CommodityCID
			,TB_SQM_Commodity_SubCID = @TB_SQM_Commodity_SubCID
			,UpdateDatetime = getDate()
			Where BasicInfoGUID = @BasicInfoGUID
			AND VendorCode = @VendorCode;
			");
            cmd.CommandText = sbSQL.ToString();
            cmd.Parameters.AddWithValue("@BasicInfoGUID", DataItem.BasicInfoGUID);
            cmd.Parameters.AddWithValue("@VendorCode", DataItem.VendorCode);
            cmd.Parameters.AddWithValue("@TB_SQM_Vendor_TypeCID", DataItem.TB_SQM_Vendor_TypeCID);
            cmd.Parameters.AddWithValue("@TB_SQM_Commodity_SubTB_SQM_CommodityCID", DataItem.TB_SQM_Commodity_SubTB_SQM_CommodityCID);
            cmd.Parameters.AddWithValue("@TB_SQM_Commodity_SubCID", DataItem.TB_SQM_Commodity_SubCID);


            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion

        #region Delete data item
        public static String DeleteDataItem(SqlConnection cnPortal, TB_SQM_Manufacturers_BasicInfo DataItem)
        {
            return SuspendCase(cnPortal, DataItem, "", "");
        }

        public static String SuspendCase(SqlConnection cnPortal, TB_SQM_Manufacturers_BasicInfo DataItem, String LoginMemberGUID, String RunAsMemberGUID)
        {
            String r = "";
            if (StringHelper.DataIsNullOrEmpty(DataItem.BasicInfoGUID))
                return "Must provide BasicInfoGUID.";
            else
            {
                SqlTransaction tran = cnPortal.BeginTransaction();

                //Delete member data
                using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = SuspendCaseSub(cmd, DataItem); }
                if (r != "") { tran.Rollback(); return r; }

                //Release lock
                if ((LoginMemberGUID != "") && (RunAsMemberGUID != ""))
                    using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = DataLockHelper.ReleaseLock(cmd, DataItem.BasicInfoGUID, LoginMemberGUID, RunAsMemberGUID); }
                if (r != "") { tran.Rollback(); return r; }

                //Commit
                try { tran.Commit(); }
                catch (Exception e) { tran.Rollback(); r = "Delete fail.<br />Exception: " + e.ToString(); }
                return r;
            }
        }

        private static String SuspendCaseSub(SqlCommand cmd, TB_SQM_Manufacturers_BasicInfo DataItem)
        {
            String sErrMsg = "";

            String sSQL = @"
            Declare @CaseID bigint
            SELECT @CaseID = ApproveCaseID FROM TB_SQM_Manufacturers_BasicInfo
            WHERE BasicInfoGUID=@BasicInfoGUID
			AND VendorCode=@VendorCode;
            
            UPDATE TB_SQM_Approve_Case
            SET Status='Reject'  
            WHERE CaseID=@CaseID;   
            
			UPDATE TB_SQM_Manufacturers_BasicInfo
			SET ApproveCaseID=''
			WHERE BasicInfoGUID=@BasicInfoGUID
			AND VendorCode=@VendorCode;
            
			";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@BasicInfoGUID", DataItem.BasicInfoGUID);
            cmd.Parameters.AddWithValue("@VendorCode", DataItem.VendorCode);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Suspend fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion

        /// <summary>
        /// 获取指定GOBOOK 部门主管清单，以头衔区分
        /// </summary>
        /// <param name="oModel_Org_Param"></param>
        /// <returns></returns>
        public static Model_Org_Manager_List_Title GetManagerList_ByTitle(SqlConnection cn, String sDeptID)
        {
            Model_Org_Manager_List_Title oModel_Org_Manager_List_Title = new Model_Org_Manager_List_Title();
            string orgStr = "";//
            DataTable dt = new DataTable();
            {
                String sSQL = "";
                sSQL = @"
                DECLARE	@return_value int,
		                @RESULT varchar(1000)

                EXEC	@return_value = [dbo].[P_GET_ORG_SITE]
		                @P_INTPUT_DEPTID = @deptID,
		                @RESULT = @RESULT OUTPUT

                SELECT	@RESULT as N'@RESULT'
                ";
                using (SqlCommand cmd = new SqlCommand(sSQL, cn))//注意是SPM
                {
                    cmd.Parameters.Add(new SqlParameter("@deptID", sDeptID));
                    orgStr = cmd.ExecuteScalar().ToString();
                }

            }

            Model_Org_Info[] oModel_Org_Info_DESC = GetOrgTree(orgStr, "DESC");
            string lastRecord = string.Empty;
            //针对课级主管，部级主管，只取ORG LEVEL最低阶主管
            foreach (Model_Org_Info oModel_Org_Info in oModel_Org_Info_DESC)
            {
                if (oModel_Org_Info != null)
                {


                    switch (oModel_Org_Info._JOB)
                    {
                        case "12":
                        case "14":
                            {
                                //课级
                                try
                                {

                                }
                                catch (Exception)
                                {

                                }
                                oModel_Org_Manager_List_Title._Supervisor = (oModel_Org_Manager_List_Title._Supervisor.Length > 0) ? oModel_Org_Manager_List_Title._Supervisor : oModel_Org_Info._LOGON_ID;

                            }
                            break;
                        case "16":
                            {
                                //副理
                                try
                                {
                                    oModel_Org_Manager_List_Title._Vice_Manager = (oModel_Org_Manager_List_Title._Vice_Manager != null && oModel_Org_Manager_List_Title._Vice_Manager.Length > 0) ? oModel_Org_Manager_List_Title._Vice_Manager : oModel_Org_Info._LOGON_ID;

                                }
                                catch (Exception)
                                {

                                }
                            }
                            break;
                        case "18":
                        case "20":
                            {
                                //經理
                                try
                                {
                                    oModel_Org_Manager_List_Title._Manager = (oModel_Org_Manager_List_Title._Manager.Length > 0) ? oModel_Org_Manager_List_Title._Manager : oModel_Org_Info._LOGON_ID;

                                }
                                catch (Exception)
                                {

                                }
                            }
                            break;
                        case "22":
                        case "24":
                            {
                                //處長
                                try
                                {
                                    oModel_Org_Manager_List_Title._Director = (oModel_Org_Manager_List_Title._Director.Length > 0) ? oModel_Org_Manager_List_Title._Director : oModel_Org_Info._LOGON_ID;

                                }
                                catch (Exception)
                                {

                                }
                            }
                            break;
                        case "26":
                        case "MD":
                            {
                                //厂长
                                try
                                {
                                    oModel_Org_Manager_List_Title._MD = oModel_Org_Info._LOGON_ID;

                                }
                                catch (Exception)
                                {

                                }
                            }
                            break;
                        case "BUHEAD":
                            {
                                //厂长
                                try
                                {
                                    oModel_Org_Manager_List_Title._BU_HEAD = oModel_Org_Info._LOGON_ID;

                                }
                                catch (Exception)
                                {

                                }
                            }
                            break;
                        case "BGHEAD":
                            {
                                //厂长
                                try
                                {
                                    oModel_Org_Manager_List_Title._BG_HEAD = oModel_Org_Info._LOGON_ID;
                                }
                                catch (Exception)
                                {

                                }
                            }
                            break;
                        default:
                            {
                                break;
                            }
                    }
                }
                else
                {
                    continue;
                }
                ////取MD下一阶主管为TOP manager
                //if (oModel_Org_Info._JOB == "MD" && oModel_Org_Manager_List_Title._Top_Manager.Length == 0)
                //{
                //    oModel_Org_Manager_List_Title._Top_Manager = (lastRecord.Length == 0) ? oModel_Org_Manager_List_Title._MD : lastRecord;
                //}
                lastRecord = oModel_Org_Info._LOGON_ID;


            }

            //如果部级主管为空，MD兼部级主管
            if (oModel_Org_Manager_List_Title._Manager.Length == 0)
            {
                oModel_Org_Manager_List_Title._Manager = oModel_Org_Manager_List_Title._MD;
            }
            return oModel_Org_Manager_List_Title;

        }

        public static Model_Org_Info[] GetOrgTree(string orgStr, string orderType)
        {
            if (orgStr.Length > 0)
            {
                string[] tmpList = orgStr.Split(';');
                Model_Org_Info[] oModel_Org_Info = new Model_Org_Info[tmpList.Length];
                int idx = 0;
                Model_Org_Info tmp;
                string s = string.Empty;
                switch (orderType)
                {
                    case "ASC"://按ORG LEVEL顺序排

                        for (int i = 0; i < tmpList.Length; i++)
                        {
                            tmp = new Model_Org_Info();
                            idx = tmpList[tmpList.Length - i - 1].IndexOf("|");
                            s = tmpList[tmpList.Length - i - 1];
                            if (!string.IsNullOrEmpty(s))
                            {
                                tmp._LOGON_ID = s.Substring(0, idx);
                                tmp._JOB = s.Substring(idx + 1, s.Length - idx - 1);
                                oModel_Org_Info[i] = tmp;
                            }

                        }

                        break;
                    case "DESC"://按ORG LEVEL倒序排

                        for (int i = 0; i < tmpList.Length; i++)
                        {
                            tmp = new Model_Org_Info();
                            idx = tmpList[i].IndexOf("|");
                            s = tmpList[i];
                            if (!string.IsNullOrEmpty(s))
                            {
                                tmp._LOGON_ID = s.Substring(0, idx);
                                tmp._JOB = s.Substring(idx + 1, s.Length - idx - 1);
                                oModel_Org_Info[i] = tmp;
                            }

                        }
                        break;
                }
                return oModel_Org_Info;
            }
            else
            {
                Model_Org_Info[] oModel_Org_Info = new Model_Org_Info[0];
                return oModel_Org_Info;
            }



        }

      
    }
    public class TB_SQM_Approve
    {
        private string _FormNo { get; set; }
        private string _Type { get; set; }
        public string FormNo { get { return this._FormNo; } set { this._FormNo = value; } }
        public string Type { get { return this._Type; } set { this._Type = value; } }
        public TB_SQM_Approve(string FormNo, string Type)
        {
            this._FormNo = FormNo;
            this._Type = Type;
        }
    }

    public class TB_SQM_Approve_Task
    {
        private string _ApproverGUID { get; set; }
        private string _CaseID { get; set; }
        private string _Status { get; set; }
        private string _StepName { get; set; }
        private string _TaskType { get; set; }
        public string ApproverGUID { get { return this._ApproverGUID; } set { this._ApproverGUID = value; } }
        public string CaseID { get { return this._CaseID; } set { this._CaseID = value; } }
        public string Status { get { return this._Status; } set { this._Status = value; } }
        public string StepName { get { return this._StepName; } set { this._StepName = value; } }
        public string TaskType { get { return this._TaskType; } set { this._TaskType = value; } }
        public TB_SQM_Approve_Task(string ApproverGUID, string CaseID, string Status, string StepName, string TaskType)
        {
            this._ApproverGUID = ApproverGUID;
            this._CaseID = CaseID;
            this._Status = Status;
            this._StepName = StepName;
            this._TaskType = TaskType;
        }
    }
    public class TB_SQM_Approve_helper
    {
        static String sKEY = "a@123456";
        static String sIV = "a@654321";
        private static void UnescapeDataFromWeb(TB_SQM_Approve DataItem)
        {
            DataItem.FormNo = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.FormNo);
            DataItem.Type = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Type);
        }

        private static String DataCheck(TB_SQM_Approve DataItem)
        {
            String r = "";
            //List<string> e = new List<string>();
            //if (DataItem.TB_SQM_Vendor_TypeCID == 0)
            //{
            //	e.Add("Must choose Vendor Type.");
            //}
            //if (StringHelper.DataIsNullOrEmpty(DataItem.TB_SQM_Commodity_SubTB_SQM_CommodityCID))
            //{
            //	e.Add("Must choose Product Type.");
            //}
            //if (StringHelper.DataIsNullOrEmpty(DataItem.TB_SQM_Commodity_SubCID))
            //{
            //	e.Add("Must choose Sub Product Type.");
            //}


            //for (int iCnt = 0; iCnt < e.Count; iCnt++)
            //{
            //	if (iCnt > 0) r += "<br />";
            //	r += e[iCnt];
            //}

            return r;
        }

        #region Create Approve Case通用版
        /// <summary>
        /// 創建簽核Case
        /// </summary>
        /// <param name="cnPortal"></param>
        /// <param name="DataItem"></param>
        /// <param name="fileName"></param>
        /// <param name="fileNameNet"></param>
        /// <param name="urlPre"></param>
        /// <param name="CaseType"></param>
        /// <returns></returns>
        public static string CreateApproveCase(SqlConnection cnPortal, TB_SQM_Approve DataItem, String fileName, String fileNameNet, String urlPre, ref string CaseID)
        {
            #region 共用參數
            string sSQL = "";
            string sErrMsg = "";
            string sCaseID = "";
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
				Values (@FormNo ,@Type,@Status,@NowDT,@NowDT);
				--取得 CaseID
				DECLARE @CaseID nvarchar(50)
				SELECT MAX(CASEID) as CaseID FROM TB_SQM_Approve_Case WHERE FormNo=@FormNo 
				";

                {
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand(sSQL, cnPortal);
                    cmd.Parameters.AddWithValue("@FormNo", DataItem.FormNo);
                    //cmd.Parameters.AddWithValue("@VendorCode", DataItem.VendorCode);
                    cmd.Parameters.AddWithValue("@Type", DataItem.Type);
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
                CaseID = sCaseID;

                #region 取得初始並行簽核單位,並且迴圈創建Task
                if (DataItem.Type.Equals("1") || DataItem.Type.Equals("2"))
                {
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
 and MemberGUID= (select DISTINCT VendorCode from TB_SQM_Manufacturers_BasicInfo
					where BasicInfoGUID=@BasicInfoGUID)
                    UNION ALL
                    SELECT DISTINCT 2 AS CID,
                           SVR.SourcerGUID AS MGUID
                    FROM TB_SQM_Member_Vendor_Map MVM,
                         TB_SQM_Vendor_Related SVR
                    WHERE MVM.PlantCode = SVR.PlantCode
                      AND MVM.VendorCode = SVR.VendorCode
  and MemberGUID= (select DISTINCT VendorCode from TB_SQM_Manufacturers_BasicInfo
					where BasicInfoGUID=@BasicInfoGUID)
                    UNION ALL
                    SELECT DISTINCT 3 AS CID,
                           SVR.SourcerGUID AS MGUID
                    FROM TB_SQM_Member_Vendor_Map MVM,
                         TB_SQM_Vendor_Related SVR
                    WHERE MVM.PlantCode = SVR.PlantCode
                      AND MVM.VendorCode = SVR.VendorCode
 and MemberGUID= (select DISTINCT VendorCode from TB_SQM_Manufacturers_BasicInfo
					where BasicInfoGUID=@BasicInfoGUID)
                    UNION ALL
                    SELECT DISTINCT 4 AS CID,
                           SVR.SourcerGUID AS MGUID
                    FROM TB_SQM_Member_Vendor_Map MVM,
                         TB_SQM_Vendor_Related SVR
                    WHERE MVM.PlantCode = SVR.PlantCode
                      AND MVM.VendorCode = SVR.VendorCode
 and MemberGUID= (select DISTINCT VendorCode from TB_SQM_Manufacturers_BasicInfo
					where BasicInfoGUID=@BasicInfoGUID)) RT
                WHERE LT.TaskType = RT.CID;
				";
                }
                else if (DataItem.Type.Equals("3"))
                {
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
                    WHERE CID='1'
                    ) LT,
                    (
                    SELECT DISTINCT 1 AS CID,
                           SVR.SourcerGUID AS MGUID
                    FROM TB_SQM_Member_Vendor_Map MVM,
                         TB_SQM_Vendor_Related SVR
                    WHERE MVM.PlantCode = SVR.PlantCode
                      AND MVM.VendorCode = SVR.VendorCode
                      and MVM.MemberGUID= (select DISTINCT MemberGUID from TB_SQM_Manufacturers_Reliability_Test where ReliabititySID=@BasicInfoGUID)
                   ) RT
                WHERE LT.TaskType = RT.CID;
				";
                }
                else if (DataItem.Type.Equals("4"))
                {
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
                    WHERE CID='1'
                    ) LT,
                    (
                    SELECT DISTINCT 1 AS CID,
                           SVR.SourcerGUID AS MGUID
                    FROM TB_SQM_Member_Vendor_Map MVM,
                         TB_SQM_Vendor_Related SVR
                    WHERE MVM.PlantCode = SVR.PlantCode
                      AND MVM.VendorCode = SVR.VendorCode
                      and MVM.MemberGUID= (select DISTINCT MemberGUID from TB_SQM_Manufacturers_Reliability_Test
					  inner join TB_SQM_Manufacturers_Reliability_File on TB_SQM_Manufacturers_Reliability_File.SID=TB_SQM_Manufacturers_Reliability_Test.ReliabititySID
					  where  FSID=@BasicInfoGUID)
                   ) RT
                WHERE LT.TaskType = RT.CID;
				";
                }
                else if (DataItem.Type.Equals("5") || DataItem.Type.Equals("6"))
                {
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
                    WHERE CID='1'
                    ) LT,
                    (
                    SELECT DISTINCT 1 AS CID,
                           SVR.SourcerGUID AS MGUID
                    FROM TB_SQM_Member_Vendor_Map MVM,
                         TB_SQM_Vendor_Related SVR
                    WHERE MVM.PlantCode = SVR.PlantCode
                      AND MVM.VendorCode = SVR.VendorCode
                      and MVM.MemberGUID= (select DISTINCT MemberGUID from TB_SQM_Supplier_Change
					where MemberGUID=@BasicInfoGUID)
                   ) RT
                WHERE LT.TaskType = RT.CID;
				";
                }

                DataTable dtBranchs = new DataTable();
                {

                    SqlCommand cmd = new SqlCommand(sSQL, cnPortal);
                    cmd.Parameters.AddWithValue("@BasicInfoGUID", DataItem.FormNo);
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
                    TB_SQM_Approve_Task taskItem = new TB_SQM_Approve_Task(
                        dr["ApproverGUID"].ToString(),
                         dr["CaseID"].ToString(),
                         dr["Status"].ToString(),
                         dr["StepName"].ToString(),
                         dr["TaskType"].ToString()
                        );
                    String TaskID = CreateTask(cnPortal, taskItem);
                    fileNameNet = "";
                    sErrMsg += SendMailByTaskID(cnPortal, TaskID, fileNameNet, urlPre, DataItem.Type);
                    //錯誤需怎麼處理?
                }

                #endregion

                #region 更新主檔資料表(需调用方实现)
                //           {
                //               String sCOL = "";
                //               if (DataItem.Type == "1")
                //               {
                //                   sCOL = "ApproveCaseID";
                //               }
                //               else if (DataItem.Type == "2")
                //               {
                //                   sCOL = "ChooseCaseID";
                //               }

                //               sSQL = @"
                //--更新主檔資料表
                //UPDATE TB_SQM_Manufacturers_BasicInfo
                //SET @Col=@CaseID
                //WHERE BasicInfoGUID=@BasicInfoGUID
                //--AND VendorCode=@VendorCode
                //";
                //               sSQL = Regex.Replace(sSQL, @"@Col", sCOL);
                //               SqlCommand cmd = new SqlCommand(sSQL, cnPortal);
                //               cmd.Parameters.AddWithValue("@BasicInfoGUID", DataItem.FormNo);
                //               //cmd.Parameters.AddWithValue("@VendorCode", DataItem.VendorCode);
                //               cmd.Parameters.AddWithValue("@CaseID", sCaseID);

                //               try
                //               {
                //                   cmd.ExecuteNonQuery();

                //               }
                //               catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                //               cmd = null;
                //           }
                #endregion

                return sErrMsg;

            }


        }
        #endregion
        # region 檢查簽核人是否都已經設定
        /// <summary>
        /// 檢查簽核人是否都已經設定
        /// </summary>
        /// <param name="cnPortal"></param>
        /// <param name="DataItem"></param>
        /// <returns></returns>
        public static String CheckApprover(SqlConnection cnPortal, TB_SQM_Approve DataItem)
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
            cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(DataItem.FormNo));

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
        #endregion
        #region 创建TASK
        public static String CreateTask(SqlConnection cnPortal, TB_SQM_Approve_Task dataItem)
        {

            String sSQL = @"
				DECLARE @NowDT Datetime
				SELECT @NowDT=getDate()
				INSERT INTO TB_SQM_Approve_Task (CaseID,StepName,TaskType,ApproverGUID,Status,CreateTime,UpdateTime)
				SELECT @CaseID,@StepName,@TaskType,@ApproverGUID,@Status,@NowDT,@NowDT

				SELECT MAX(TaskID) AS TaskID FROM TB_SQM_Approve_Task WHERE CaseID=@CaseID AND StepName=@StepName AND CreateTime=@NowDT
			";
            SqlCommand cmd = new SqlCommand(sSQL, cnPortal);
            cmd.Parameters.AddWithValue("@CaseID", StringHelper.NullOrEmptyStringIsDBNull(dataItem.CaseID));
            cmd.Parameters.AddWithValue("@StepName", StringHelper.NullOrEmptyStringIsDBNull(dataItem.StepName));
            cmd.Parameters.AddWithValue("@TaskType", StringHelper.NullOrEmptyStringIsDBNull(dataItem.TaskType));
            cmd.Parameters.AddWithValue("@ApproverGUID", StringHelper.NullOrEmptyStringIsDBNull(dataItem.ApproverGUID));
            cmd.Parameters.AddWithValue("@Status", StringHelper.NullOrEmptyStringIsDBNull(dataItem.Status));


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
        #endregion
        public static String SendMailByTaskID(SqlConnection cnPortal, String TaskID, String fileNameNet, String urlPre, string type)
        {
            #region 共用參數
            String sSQL = @"";
            String sFROM = @"";
            String sTO = @"";
            String sTONAME = @"";
            String sSubject = @"";
            #endregion
            if (type.Equals("3") || type.Equals("4") || type.Equals("5") || type.Equals("6"))
            {
                sSQL = @"
				--DECLARE @TaskID BIGINT
                --SET @TaskID = 123
                DECLARE @CaseID NVARCHAR(50)
                DECLARE @CaseType NVARCHAR(50)
                DECLARE @TaskType BIGINT
                -----------------------
                SELECT @CaseID = CaseID,
                       @TaskType = TaskType
                FROM TB_SQM_Approve_Task
                WHERE TaskID = @TaskID
                SELECT @CaseType = [Type]
                FROM TB_SQM_Approve_Case
                WHERE CaseID = @CaseID
                -----------------------
                SELECT TOP ( 1 ) ATA.TASKID,
                                 PM.PrimaryEmail,
                                 ATA.TaskType,
                                 ATA.CaseID,
                                 @CaseType AS CaseType
                FROM TB_SQM_Approve_Task ATA,
                     TB_SQM_Approver_Type ATY,
                     PORTAL_Members PM
                WHERE ATA.TaskType = @TaskType
                  AND CONVERT(NVARCHAR(50), ATA.ApproverGUID) = CONVERT(NVARCHAR(50), PM.MemberGUID)
                  AND ATA.CASEID = @CaseID
                  AND ATA.TaskType = ATY.CID
                ORDER BY TaskID DESC
			";
            }
            else
            {
                sSQL = @"
				--DECLARE @TaskID BIGINT
                --SET @TaskID = 123
                DECLARE @CaseID NVARCHAR(50)
                DECLARE @CaseType NVARCHAR(50)
                DECLARE @TaskType BIGINT
                -----------------------
                SELECT @CaseID = CaseID,
                       @TaskType = TaskType
                FROM TB_SQM_Approve_Task
                WHERE TaskID = @TaskID
                SELECT @CaseType = [Type]
                FROM TB_SQM_Approve_Case
                WHERE CaseID = @CaseID
                -----------------------
                SELECT TOP ( 2 ) ATA.TASKID,
                                 PM.PrimaryEmail,
                                 ATA.TaskType,
                                 ATA.CaseID,
                                 @CaseType AS CaseType
                FROM TB_SQM_Approve_Task ATA,
                     TB_SQM_Approver_Type ATY,
                     PORTAL_Members PM
                WHERE ATA.TaskType = @TaskType
                  AND CONVERT(NVARCHAR(50), ATA.ApproverGUID) = CONVERT(NVARCHAR(50), PM.MemberGUID)
                  AND ATA.CASEID = @CaseID
                  AND ATA.TaskType = ATY.CID
                ORDER BY TaskID DESC
			";
            }





            #region 取得參數

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
            if (dr["CaseType"].ToString() == "1")
            {
                sSubject = "SQM基本調查表簽核";
            }
            else if (dr["CaseType"].ToString() == "2")
            {
                sSubject = "SQM供應商選定簽核";
            }
            else if (dr["CaseType"].ToString() == "3")
            {
                sSubject = "SQM供應商信赖性簽核";
            }
            else if (dr["CaseType"].ToString() == "4")
            {
                sSubject = "SQM供應商可靠度报告簽核";
            }
            else if (dr["CaseType"].ToString() == "5" || dr["CaseType"].ToString() == "6")
            {
                sSubject = "SQM供應商變更通知單簽核";
            }
            if (dt.Rows.Count > 1)
            {
                sFROM = dt.Rows[1]["PrimaryEmail"].ToString();
            }
            else
            {
                sFROM = "SQM@liteon.com.tw";
            }
            sTO = dr["PrimaryEmail"].ToString();
            //sTO = "jeter.sun@liteon.com";
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

            //sbBody.Append("FROM:" + sFROM + " TO:" + sTO + "<br/>");

            sbBody.Append("Hello ");
            sbBody.Append(sTONAME);
            sbBody.Append("<br/>");
            sbBody.Append("You have received (an) " + sSubject + " request(s) from SQM for process.");
            sbBody.Append("<br/>");

            sbBody.Append("To approve or reject the request(s), please click this<a href='");
            sbBody.Append(urlPre);
            if (dr["CaseType"].ToString() == "1")
            {
                sbBody.Append("/SQM/Approve?TaskID=");
                sbBody.Append(desEncryptBase64(TaskID));
            }
            else if (dr["CaseType"].ToString() == "2")
            {
                sbBody.Append("/SQM/Approve?TaskID=");
                sbBody.Append(desEncryptBase64(TaskID));
            }
            else if (dr["CaseType"].ToString() == "3")
            {
                sbBody.Append("/SQM/ReliabilityApprove?TaskID=");
                sbBody.Append(desEncryptBase64(TaskID));
            }
            else if (dr["CaseType"].ToString() == "4")
            {
                sbBody.Append("/SQM/ReliabilityFileApprove?TaskID=");
                sbBody.Append(desEncryptBase64(TaskID));
            }
            else if (dr["CaseType"].ToString() == "5" || dr["CaseType"].ToString() == "6")
            {
                sbBody.Append("/SQM/ChangeApprove?TaskID=");
                sbBody.Append(desEncryptBase64(TaskID));
            }

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
                   sTO,//sTO toDO
                    "jeter.sun@liteon.com;Aiden.Zeng@liteon.com",//toDOJerryA.Chen@liteon.com;Aiden.Zeng@liteon.com;Lily.Guo@liteon.com;
                    "",
                    sSubject,//"SQM系統簽核"
                    sbBody.ToString(),
                    icm045.MailPriority.Normal,
                    icm045.MailFormat.Html,
                    aryFile);//fileName string[0]
            #endregion

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

        //首先是source簽核，簽核完成，則根據通知單的項目類型決定是走RD簽核還是SQE簽核

    }


    public class TB_SQM_Approve_helper2
    {
        static String sKEY = "a@123456";
        static String sIV = "a@654321";
        //创建case
        public string createcase(SqlConnection cnPortal, TB_SQM_Approve DataItem, ref string sCaseID)
        {
            #region 共用參數
            string sSQL = "";
            string sErrMsg = "";
            #endregion
            sSQL = @"
				DECLARE @NowDT Datetime
				SELECT @NowDT=getDate()
				--Insert into CASE
				Insert Into TB_SQM_Approve_Case(FormNo,Type,Status,CreateTime,UpdateTime)
				Values (@FormNo ,@Type,@Status,@NowDT,@NowDT);
				--取得 CaseID
				DECLARE @CaseID nvarchar(50)
				SELECT MAX(CASEID) as CaseID FROM TB_SQM_Approve_Case WHERE FormNo=@FormNo 
				";


            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand(sSQL, cnPortal);
            cmd.Parameters.AddWithValue("@FormNo", DataItem.FormNo);
            cmd.Parameters.AddWithValue("@Type", DataItem.Type);
            cmd.Parameters.AddWithValue("@Status", "Pending");
            try
            {
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    dt.Load(sdr);
                }
                sCaseID = dt.Rows[0]["CaseID"].ToString();
            }
            catch (Exception e) { sErrMsg = "Check fail.<br />Exception: " + e.ToString(); }


            return sErrMsg;
        }

        //根据casetype和tasktype查询流程表找到签核人员类型创建task
        //根据发起人的plant、vodercode和签核人员类型找签核人
        //发送邮件
        public string sendmailbytaskid(SqlConnection cnPortal, TB_SQM_Approve DataItem, string urlPre, string TaskID, string fileNameNet)
        {
            string sSQL = @"";
            string sFROM = @"";
            string sTO = @"";
            string sTONAME = @"";
            string sSubject = @"";
            string sErrMsg = "";

            SqlCommand cmd = new SqlCommand(sSQL, cnPortal);
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
            if (dr["CaseType"].ToString() == "1")
            {
                sSubject = "SQM基本調查表簽核";
            }
            else if (dr["CaseType"].ToString() == "2")
            {
                sSubject = "SQM供應商選定簽核";
            }
            else if (dr["CaseType"].ToString() == "3")
            {
                sSubject = "SQM供應商信赖性簽核";
            }
            else if (dr["CaseType"].ToString() == "4")
            {
                sSubject = "SQM供應商可靠度报告簽核";
            }

            if (dt.Rows.Count > 1)
            {
                sFROM = dt.Rows[1]["PrimaryEmail"].ToString();
            }
            else
            {
                sFROM = "SQM@liteon.com.tw";
            }
            sTO = dr["PrimaryEmail"].ToString();
            //sTO = "jeter.sun@liteon.com";
            sTONAME = Regex.Replace(sTO, @"@liteon.com", "");
            try
            {
                sTONAME = sTONAME.Split('.')[0];
            }
            catch (Exception ex)
            {

            }
            StringBuilder sbBody = new StringBuilder();

            //sbBody.Append("FROM:" + sFROM + " TO:" + sTO + "<br/>");

            sbBody.Append("Hello ");
            sbBody.Append(sTONAME);
            sbBody.Append("<br/>");
            sbBody.Append("You have received (an) " + sSubject + " request(s) from SQM for process.");
            sbBody.Append("<br/>");

            sbBody.Append("To approve or reject the request(s), please click this<a href='");
            sbBody.Append(urlPre);
            if (dr["CaseType"].ToString() == "1")
            {
                sbBody.Append("/SQM/Approve?TaskID=");
                sbBody.Append(desEncryptBase64(TaskID));
            }
            else if (dr["CaseType"].ToString() == "2")
            {
                sbBody.Append("/SQM/Approve?TaskID=");
                sbBody.Append(desEncryptBase64(TaskID));
            }
            else if (dr["CaseType"].ToString() == "3")
            {
                sbBody.Append("/SQM/ReliabilityApprove?TaskID=");
                sbBody.Append(desEncryptBase64(TaskID));
            }
            else if (dr["CaseType"].ToString() == "4")
            {
                sbBody.Append("/SQM/ReliabilityFileApprove?TaskID=");
                sbBody.Append(desEncryptBase64(TaskID));
            }

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
                   sTO,//sTO toDO
                    "jeter.sun@liteon.com;Aiden.Zeng@liteon.com",//toDOJerryA.Chen@liteon.com;Aiden.Zeng@liteon.com;Lily.Guo@liteon.com;
                    "",
                    sSubject,//"SQM系統簽核"
                    sbBody.ToString(),
                    icm045.MailPriority.Normal,
                    icm045.MailFormat.Html,
                    aryFile);//fileName string[0]


            return sErrMsg;
        }




        //签核页面签核时找到下一个签核人发送邮件
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
    }
}
