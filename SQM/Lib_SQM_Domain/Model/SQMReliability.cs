using Lib_Portal_Domain.Model;
using Lib_Portal_Domain.SharedLibs;
using Lib_SQM_Domain.Modal;
using Lib_SQM_Domain.SharedLibs;
using Lib_VMI_Domain.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;

namespace Lib_SQM_Domain.Model
{
    #region Reliability
    #region Data Class Definitions
    public class SQMReliability
    {
        protected string _ReliabilitySID { get; set; }
        protected string _TB_SQM_CommodityCID { get; set; }
        protected string _CommodityName { get; set; }
        protected string _TB_SQM_Commodity_SubCID { get; set; }
        protected string _Commodity_SubName { get; set; }
        protected string _TestProjet { get; set; }
        protected string _PlannedTestTime { get; set; }
        protected string _ActualTestTime { get; set; }
        protected string _TestResult { get; set; }
        protected string _TestPeople { get; set; }
        protected string _Note { get; set; }
        protected string _insertime { get; set; }


        public string ReliabilitySID { get { return this._ReliabilitySID; } set { this._ReliabilitySID = value; } }
        public string TB_SQM_CommodityCID { get { return this._TB_SQM_CommodityCID; } set { this._TB_SQM_CommodityCID = value; } }
        public string CommodityName { get { return this._CommodityName; } set { this._CommodityName = value; } }
        public string TB_SQM_Commodity_SubCID { get { return this._TB_SQM_Commodity_SubCID; } set { this._TB_SQM_Commodity_SubCID = value; } }
        public string Commodity_SubName { get { return this._Commodity_SubName; } set { this._Commodity_SubName = value; } }
        public string TestProjet { get { return this._TestProjet; } set { this._TestProjet = value; } }
        public string PlannedTestTime { get { return this._PlannedTestTime; } set { this._PlannedTestTime = value; } }
        public string ActualTestTime { get { return this._ActualTestTime; } set { this._ActualTestTime = value; } }
        public string TestResult { get { return this._TestResult; } set { this._TestResult = value; } }
        public string TestPeople { get { return this._TestPeople; } set { this._TestPeople = value; } }
        public string Note { get { return this._Note; } set { this._Note = value; } }
        public string insertime { get { return this._insertime; } set { this._insertime = value; } }


        public SQMReliability() { }
        public SQMReliability(
                string ReliabilitySID,
                string TB_SQM_CommodityCID,
                string CommodityName,
                string TB_SQM_Commodity_SubCID,
                string Commodity_SubName,
                string TestProjet,
                string PlannedTestTime,
                string ActualTestTime,
                string TestResult,
                string TestPeople,
                string Note,
                string insertime
                )
        {
            this._ReliabilitySID = ReliabilitySID;
            this._TB_SQM_CommodityCID = TB_SQM_CommodityCID;
            this._CommodityName = CommodityName;
            this._TB_SQM_Commodity_SubCID = TB_SQM_Commodity_SubCID;
            this._Commodity_SubName = Commodity_SubName;
            this._TestProjet = TestProjet;
            this._PlannedTestTime = PlannedTestTime;
            this._ActualTestTime = ActualTestTime;
            this._TestResult = TestResult;
            this._TestResult = TestResult;
            this._TestPeople = TestPeople;
            this._Note = Note;
            this._insertime = insertime;

        }
    }

    public class SQMReliability_jQGridJSon
    {
        public List<SQMReliability> Rows = new List<SQMReliability>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }
    #endregion





    #region SQMReliability_Helper
    public static class SQMReliability_Helper
    {
        #region Search
        public static string GetDataToJQGridJson(SqlConnection cn, string MemberGUID)
        {
            return GerDateToJQGridJson(cn, "", MemberGUID);
        }

        //public static string GerDateToJQGridJson(SqlConnection cn, string SearchText, string MemberGUID)
        //{
        //    SQMReliability_jQGridJSon m = new SQMReliability_jQGridJSon();

        //    string sSearchText = SearchText.Trim();
        //    string sWhereClause = "";
        //    if (sSearchText != "")
        //        sWhereClause += "   ReliabilityName = @SearchText   ";
        //    if (sWhereClause.Length != 0)
        //        sWhereClause = "  and " + sWhereClause.Substring(0);

        //    m.Rows.Clear();
        //    int iRowCount = 0;
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(@"
        //    SELECT SID
        //          ,ReliabilityName
        //     FROM LiteOnRFQTraining.dbo.TB_SQM_Manufacturers_Reliability_Test_map
        //     WHERE MemberGUID = @MemberGUID
        //            ");

        //    //LEFT JOIN TB_SQM_Commodity on TB_SQM_Manufacturers_Reliability_Test.TB_SQM_CommodityCID = TB_SQM_Commodity.CID
        //    // LEFT JOIN TB_SQM_Commodity_Sub on TB_SQM_Manufacturers_Reliability_Test.TB_SQM_Commodity_SubCID = TB_SQM_Commodity_Sub.CID



        //    sb.Append(sWhereClause + ";");
        //    using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
        //    {
        //        if (sSearchText != "")
        //            cmd.Parameters.Add(new SqlParameter("@SearchText", sSearchText));
        //        cmd.Parameters.Add(new SqlParameter("@MemberGUID", SQMStringHelper.NullOrEmptyStringIsDBNull(MemberGUID)));
        //        SqlDataReader dr = cmd.ExecuteReader();
        //        while (dr.Read())
        //        {
        //            iRowCount++;
        //            m.Rows.Add(new SQMReliability(
        //                dr["SID"].ToString(),
        //                dr["ReliabilityName"].ToString()
        //                ));
        //        }
        //        dr.Close();
        //        dr = null;
        //    }

        //    JavaScriptSerializer oSerializer = new JavaScriptSerializer();
        //    return oSerializer.Serialize(m);
        //}

        public static String GerDateToJQGridJson(SqlConnection cn, String SearchText, String MemberGUID)
        {
            string sSearchText = SearchText.Trim();
            string sWhereClause = "";
            if (sSearchText != "")
                sWhereClause += "   MRT.TestProjet like '%'+ @SearchText+'%'   ";
            if (sWhereClause.Length != 0)
                sWhereClause = "  AND " + sWhereClause.Substring(0);
            // SELECT [SID]
            //		,ReliabilityName
            //		,ISNULL(AC.Status, 'None') AS Status
            //        FROM TB_SQM_Manufacturers_Reliability_Test_map RTM
            //        LEFT OUTER JOIN TB_SQM_Approve_Case AC ON RTM.[ReliabilitycaseID] = AC.CaseID

            //WHERE MemberGUID = @MemberGUID

            StringBuilder sb = new StringBuilder();
            sb.Append(@"
SELECT MRT.MemberGUID
      --,RTM.ReliabilitycaseID
      --,RTM.FilecaseID
	  ,MRT.ReliabititySID
      ,MRT.TB_SQM_CommodityCID

	  ,TSC.CNAME AS CommodityName
      ,MRT.TB_SQM_Commodity_SubCID
	  ,TSS.CNAME AS Commodity_SubName
      ,MRT.TestProjet
      ,MRT.PlannedTestTime

	  ,MRF.FSID
	  ,MRF.ActualTestTime
      ,MRF.TestResult
      ,MRF.TestPeople
      ,MRF.Note
      ,MRF.insertime

	 ,case when ISNULL(SS.Status,'None')='Finished' then 'approve' else  ISNULL(SS.Status,'None') end as Status 
	  ,case when ISNULL(FS.FileStatus,'None')='Finished' then 'approve' else ISNULL(FS.FileStatus,'None') end AS FileStatus


	  ,F.FileName
      ,F.FGUID
      ,SAT1.Remark AS StateNote
	  ,SAT2.Remark AS FileStateNote
FROM [dbo].[TB_SQM_Manufacturers_Reliability_Test] AS MRT
LEFT JOIN (
SELECT [MemberGUID]
      ,[ReliabilitycaseID]
      ,[FilecaseID]
  FROM [TB_SQM_Manufacturers_Reliability_Test_map]) AS MRTM ON MRT.ReliabititySID = MRTM.MemberGUID
--FILE
LEFT JOIN (
SELECT SID
		,FSID
		,FGuid
		,ActualTestTime
		,TestResult
		,TestPeople

		,Note
		,insertime
	FROM TB_SQM_Manufacturers_Reliability_File 
	INNER JOIN (SELECT MAX(insertime) AS Leastinsertime FROM TB_SQM_Manufacturers_Reliability_File GROUP BY SID) AS LMRF
	ON insertime = LMRF.Leastinsertime
) AS MRF ON MRF.SID = MRT.ReliabititySID
--DDL
LEFT JOIN TB_SQM_Commodity AS TSC ON MRT.TB_SQM_CommodityCID = TSC.CID
LEFT JOIN TB_SQM_Commodity_Sub AS TSS ON MRT.TB_SQM_Commodity_SubCID = TSS.CID
-- STATUS SS
LEFT JOIN (SELECT CaseID,Status,TB_SQM_Manufacturers_Reliability_Test_map.MemberGUID
		FROM TB_SQM_Approve_Case INNER JOIN TB_SQM_Manufacturers_Reliability_Test_map 
ON  ReliabilitycaseID= CaseID ) AS SS ON (SELECT [ReliabilitycaseID] FROM [dbo].[TB_SQM_Manufacturers_Reliability_Test_map] WHERE [MemberGUID] = MRT.ReliabititySID)=SS.CaseID
--STATUS FS
LEFT JOIN (SELECT CaseID,Status AS FileStatus
		FROM TB_SQM_Approve_Case INNER JOIN TB_SQM_Manufacturers_Reliability_Test_map
ON  FileCaseID= CaseID ) AS FS ON  (SELECT [FilecaseID] FROM [dbo].[TB_SQM_Manufacturers_Reliability_Test_map] WHERE [MemberGUID] = MRF.FSID)=FS.CaseID
--FILE
LEFT JOIN TB_SQM_Files AS F ON MRF.FGuid=F.FGUID
--CASENOTE
LEFT JOIN (SELECT TOP 1 CaseID,Remark FROM [TB_SQM_Approve_Task] ORDER BY [UpdateTime] DESC)  AS SAT1 ON (SELECT [ReliabilitycaseID] FROM [dbo].[TB_SQM_Manufacturers_Reliability_Test_map] WHERE [MemberGUID] = MRT.ReliabititySID)=SAT1.CaseID
LEFT JOIN (SELECT TOP 1 CaseID,Remark FROM [TB_SQM_Approve_Task] ORDER BY [UpdateTime] DESC)  AS SAT2 ON (SELECT [FilecaseID] FROM [dbo].[TB_SQM_Manufacturers_Reliability_Test_map] WHERE [MemberGUID] = MRF.FSID)=SAT2.CaseID

WHERE MRT.MemberGUID = @MemberGUID

            ");
            DataTable dt = new DataTable();
            if (sSearchText != "")
            {
                sb.Append(sWhereClause);
            }
            sb.Append(@"order by TB_SQM_CommodityCID,TB_SQM_Commodity_SubCID,insertime desc");
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
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy/MM/dd HH:mm:ss";
            return JsonConvert.SerializeObject(dt,Formatting.Indented,timeFormat);
        }
        public static String GetHistoryDateToJQGridJson(SqlConnection cn, String SID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
SELECT 
	 [SID]
	  ,TSC.CNAME AS CommodityName
      ,MRT.TB_SQM_Commodity_SubCID
	  ,TSS.CNAME AS Commodity_SubName
      ,MRT.TestProjet
      ,MRT.PlannedTestTime

	  ,MRF.ActualTestTime
      ,MRF.TestResult
      ,MRF.TestPeople
      ,MRF.Note
      ,MRF.insertime

	  ,MRT.[TestProjet]
	  ,MRT.[PlannedTestTime]

	,F.FGuid AS FGUID
	,FileName
FROM [dbo].[TB_SQM_Manufacturers_Reliability_File] AS MRF
	LEFT JOIN TB_SQM_Files AS F ON MRF.FGuid=F.FGuid
	LEFT JOIN [dbo].[TB_SQM_Manufacturers_Reliability_Test] AS MRT ON MRF.SID=MRT.[ReliabititySID]
	--ddl
	LEFT JOIN TB_SQM_Commodity AS TSC ON MRT.TB_SQM_CommodityCID = TSC.CID
	LEFT JOIN TB_SQM_Commodity_Sub AS TSS ON MRT.TB_SQM_Commodity_SubCID = TSS.CID

WHERE MRF.SID=@SID
ORDER BY insertime DESC

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
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy/MM/dd HH:mm:ss";
            return JsonConvert.SerializeObject(dt, Formatting.Indented, timeFormat);
        }
        public static string UpdateCaseId(SqlConnection cn,String MemberGUID,string caseID)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
IF NOT EXISTS(SELECT MemberGUID FROM TB_SQM_Manufacturers_Reliability_Test_map  WHERE MemberGUID = @MemberGUID)
    BEGIN
            INSERT INTO TB_SQM_Manufacturers_Reliability_Test_map
            (MemberGUID
		    ,ReliabilitycaseID)
            VALUES
		    (@MemberGUID
		    ,@ReliabilitycaseID)
    END;
Update TB_SQM_Manufacturers_Reliability_Test_map
Set ReliabilitycaseID=@ReliabilitycaseID
Where MemberGUID=@MemberGUID
                        ");

            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.AddWithValue("@ReliabilitycaseID", SQMStringHelper.NullOrEmptyStringIsDBNull(caseID));

                cmd.Parameters.AddWithValue("@MemberGUID",MemberGUID);


                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }
            }
            return sErrMsg;
        }

        #endregion

        #region Create/Edit data check
        private static void UnescapeDataFromWeb(SQMReliability DataItem)
        {
            DataItem.TestProjet = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.TestProjet);
            DataItem.PlannedTestTime = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.PlannedTestTime);
            DataItem.TestPeople= SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.TestPeople);
            DataItem.Note= SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Note);
        }
        private static string DataCheck(SQMReliability DataItem)
        {
            string r = "";
            List<string> e = new List<string>();
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.TestProjet))
                e.Add("Must provide TestProjet.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.PlannedTestTime))
                e.Add("Must provide PlannedTestTime.");

            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }

        #endregion


        #region Create data item
        public static string CreateDataItem(SqlConnection cnPortal, SQMReliability DataItem, string MemberGUID)
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

INSERT INTO TB_SQM_Manufacturers_Reliability_Test
					(ReliabititySID
					,MemberGUID
					,TB_SQM_CommodityCID
					,TB_SQM_Commodity_SubCID
					,TestProjet
					,PlannedTestTime)
VALUES　(@ReliabititySID, @MemberGUID,@TB_SQM_CommodityCID,@TB_SQM_Commodity_SubCID,@TestProjet,@PlannedTestTime);

                    ");

                //SQM_Basic_Helper.InsertPart(sb, "3");
                SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal);
                cmd.Parameters.AddWithValue("@ReliabititySID", Guid.NewGuid());
                cmd.Parameters.AddWithValue("@MemberGUID", MemberGUID);
                cmd.Parameters.AddWithValue("@TB_SQM_CommodityCID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TB_SQM_CommodityCID));
                cmd.Parameters.AddWithValue("@TB_SQM_Commodity_SubCID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TB_SQM_Commodity_SubCID));
                cmd.Parameters.AddWithValue("@TestProjet", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TestProjet));
                cmd.Parameters.AddWithValue("@PlannedTestTime", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.PlannedTestTime));

                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;

                return sErrMsg;
            }
        }
        #endregion

        #region Edit data item
        public static string EditDataItem(SqlConnection cnPortal, SQMReliability DataItem)
        {
            return EditDataItem(cnPortal, DataItem, "", "");
        }

        public static string EditDataItem(SqlConnection cnPortal, SQMReliability DataItem, string LoginMemberGUID, string RunAsMemberGUID)
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

        private static string EditDataItemSub(SqlCommand cmd, SQMReliability DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
 Update [TB_SQM_Manufacturers_Reliability_Test] Set
      [TB_SQM_CommodityCID]=@TB_SQM_CommodityCID
      ,[TB_SQM_Commodity_SubCID]=@TB_SQM_Commodity_SubCID
      ,[TestProjet]=@TestProjet
      ,[PlannedTestTime]=@PlannedTestTime
             Where ReliabititySID = @ReliabititySID

--UPDATE TB_SQM_Manufacturers_Reliability_Test_map
--SET ReliabilitycaseID=NULL
--WHERE MemberGUID=@MemberGUID 
            ");
            cmd.CommandText = sb.ToString();
            //cmd.Parameters.AddWithValue("@FGuid", FGuid);
            cmd.Parameters.AddWithValue("@ReliabititySID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ReliabilitySID));
            cmd.Parameters.AddWithValue("@TB_SQM_CommodityCID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TB_SQM_CommodityCID));
            cmd.Parameters.AddWithValue("@TB_SQM_Commodity_SubCID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TB_SQM_Commodity_SubCID));
            cmd.Parameters.AddWithValue("@TestProjet", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TestProjet));
            cmd.Parameters.AddWithValue("@PlannedTestTime", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.PlannedTestTime));

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }

        #endregion

        #region Update data item

        private static string UpdateDataCheck(SQMReliability DataItem)
        {
            string r = "";
            List<string> e = new List<string>();
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.ActualTestTime))
                e.Add("Must provide ActualTestTime.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.TestPeople))
                e.Add("Must provide TestPeople.");


            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }
        public static string UpdateDataItem(SqlConnection cnPortal, SQMReliability DataItem)
        {
            return UpdateDataItem(cnPortal, DataItem, "", "");
        }

        public static string UpdateDataItem(SqlConnection cnPortal, SQMReliability DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            string r = UpdateDataCheck(DataItem);
            if (r != "")
            { return r; }
            else
            {
                using (SqlCommand cmd = new SqlCommand("", cnPortal)) { r = UpdateDataItemSub(cmd, DataItem); }
                if (r != "") { return r; }
                return r;
            }
        }

        private static string UpdateDataItemSub(SqlCommand cmd, SQMReliability DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();

//            IF NOT EXISTS(SELECT SID FROM dbo.TB_SQM_Manufacturers_Reliability_File WHERE SID = @SID)

//    BEGIN
//        INSERT INTO TB_SQM_Manufacturers_Reliability_File

//        (SID
//        , ActualTestTime
//        , TestResult
//        , TestPeople
//        , Note
//        , insertime)

//        VALUES(@SID, @ActualTestTime, @TestResult, @TestPeople, @Note, @insertime)

//    END;
//            UPDATE TB_SQM_Manufacturers_Reliability_File

//    SET ActualTestTime = @ActualTestTime
//    , TestResult = @TestResult
//    , TestPeople = @TestPeople
//    , Note = @Note
//    , insertime = @insertime
//WHERE SID = @SID

            sb.Append(@"
        DECLARE @OldFGuid uniqueidentifier =(
	        SELECT TOP 1 [FGuid]
	        FROM [TB_SQM_Manufacturers_Reliability_File]
	        WHERE [SID]=@SID
	        ORDER BY [insertime] DESC
        )

		INSERT INTO TB_SQM_Manufacturers_Reliability_File
		(SID
        ,FSID
        ,FGuid
		,ActualTestTime
		,TestResult
		,TestPeople
		,Note
		,insertime)
		VALUES(@SID,@FSID,@OldFGuid,@ActualTestTime,@TestResult,@TestPeople,@Note,@insertime)

                ");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@SID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ReliabilitySID));
            cmd.Parameters.AddWithValue("@FSID", Guid.NewGuid());
            cmd.Parameters.AddWithValue("@ActualTestTime", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ActualTestTime));
            cmd.Parameters.AddWithValue("@TestProjet", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TestProjet));
            cmd.Parameters.AddWithValue("@TestResult", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TestResult));
            cmd.Parameters.AddWithValue("@TestPeople", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TestPeople));
            cmd.Parameters.AddWithValue("@Note", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Note));
            cmd.Parameters.AddWithValue("@insertime", System.DateTime.Now);



            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Update fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }

        #endregion

        #region Delete data item
        public static string DeleteDataItem(SqlConnection cnPortal, SQMReliability DataItem)
        {
            return DeleteDataItem(cnPortal, DataItem, "", "");
        }

        public static string DeleteDataItem(SqlConnection cnPortal, SQMReliability DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {

            string r = "";
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.ReliabilitySID))
                return "Must provide ReliabilitySID.";
            else
            {
                using (SqlCommand cmd = new SqlCommand("", cnPortal)) { r = DeleteDataItemSub(cmd, DataItem); }
                if (r != "") { return r; }
                return r;
            }
        }

        private static string DeleteDataItemSub(SqlCommand cmd, SQMReliability DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
DECLARE @MemberGUID uniqueidentifier = (
	SELECT MemberGUID FROM TB_SQM_Manufacturers_Reliability_Test 
	WHERE ReliabititySID=@SID
)
Delete TB_SQM_Manufacturers_Reliability_File Where SID = @SID;
Delete TB_SQM_Manufacturers_Reliability_Test Where ReliabititySID = @SID;

IF NOT EXISTS(SELECT ReliabititySID FROM TB_SQM_Manufacturers_Reliability_Test  WHERE MemberGUID = @MemberGUID)
    BEGIN
		DELETE FROM TB_SQM_Manufacturers_Reliability_Test_map 
		WHERE MemberGUID=@MemberGUID
    END;

UPDATE TB_SQM_Manufacturers_Reliability_Test_map
SET ReliabilitycaseID=NULL
WHERE MemberGUID=@MemberGUID    
");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@SID", DataItem.ReliabilitySID);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion

        #region Update File
        public static string UploadReliabilityFile(SqlConnection cn, PortalUserProfile RunAsUser, FileAttachmentInfo FA, string sLocalPath, string sLocalUploadPath, HttpServerUtilityBase Server, string RequestApplicationPath, string SID)
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
            //try
            //{
            //    DataTable dt = new DataTable();
            //    StringBuilder sb = new StringBuilder();
            //    sb.Append(
            //    @"
            //    DELETE FROM TB_SQM_Files
            //    WHERE FGUID = (
            //            SELECT [FGuid]
            //            FROM [TB_SQM_Manufacturers_Reliability_File]
	           // WHERE SID = @SID
            //    ");
            //    String sql = Regex.Replace(sb.ToString(), @"\s+", " ");

            //    using (SqlCommand cmd = new SqlCommand(sql, cn, tran))
            //    {
            //        cmd.Parameters.Add(new SqlParameter("@SID", StringHelper.NullOrEmptyStringIsEmptyString(SID)));
            //        cmd.ExecuteNonQuery();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    r = ex.ToString();
            //}

            //02.Update new FGUID
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(
                @"

DECLARE @Oldinsertime datetime =( SELECT MAX(insertime) AS Leastinsertime FROM [dbo].[TB_SQM_Manufacturers_Reliability_File] WHERE SID=@SID ) 
DECLARE @OldActualTestTime nvarchar(50) = (
		SELECT TOP 1 ActualTestTime  FROM [TB_SQM_Manufacturers_Reliability_File]
		WHERE SID=@SID
		ORDER BY insertime DESC
)
DECLARE @OldTestResult nvarchar(50) = (
		SELECT TOP 1 TestResult   FROM [TB_SQM_Manufacturers_Reliability_File]
		WHERE SID=@SID
		ORDER BY insertime DESC
)
DECLARE @OldTestPeople nvarchar(50) = (
		SELECT TOP 1 TestPeople FROM [TB_SQM_Manufacturers_Reliability_File]
		WHERE SID=@SID
		ORDER BY insertime DESC
)
DECLARE @OldNote nvarchar(50) = (
		SELECT TOP 1 Note FROM [TB_SQM_Manufacturers_Reliability_File]
		WHERE SID=@SID
		ORDER BY insertime DESC
)
INSERT INTO TB_SQM_Manufacturers_Reliability_File (
	   [SID]
      ,[FSID]
      ,[FGuid]
      ,[ActualTestTime]
      ,[TestResult]
      ,[TestPeople]
      ,[Note]
      ,[insertime]
) 
VALUES (@SID,@FSID,@FGuid,@OldActualTestTime,@OldTestResult,@OldTestPeople,@OldNote,@insertime) 

                ");
                String sql = Regex.Replace(sb.ToString(), @"\s+", " ");

                using (SqlCommand cmd = new SqlCommand(sql, cn, tran))
                {
                    cmd.Parameters.Add(new SqlParameter("@SID", StringHelper.NullOrEmptyStringIsEmptyString(SID)));
                    cmd.Parameters.Add(new SqlParameter("@FSID", Guid.NewGuid()));
                    cmd.Parameters.Add(new SqlParameter("@FGUID", FGUID));
                    cmd.Parameters.Add(new SqlParameter("@insertime", System.DateTime.Now));
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

        public static string UploadFile(SqlConnection cn, PortalUserProfile RunAsUser, FileAttachmentInfo FA, string SID, string sLocalPath, string sLocalUploadPath, HttpServerUtilityBase Server, string RequestApplicationPath)
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

            //01.del esixt file
            try
            {
                DataTable dt = new DataTable();
                StringBuilder sb = new StringBuilder();
                sb.Append(
                @"
                DECLARE @OldKeyValue uniqueidentifier;

                SELECT FGUID FROM TB_SQM_Manufacturers_Reliability_Test_map
                WHERE SID = @SID;

                UPDATE TB_SQM_Manufacturers_Reliability_Test_map 
                   SET @file=null;
                DELETE FROM TB_SQM_Files
                WHERE FGUID =@OldKeyValue
                ");
                String sql = Regex.Replace(sb.ToString(), @"\s+", " ");
                sql = Regex.Replace(sql, @"@col", col);

                using (SqlCommand cmd = new SqlCommand(sql, cn, tran))
                {
                    cmd.Parameters.Add(new SqlParameter("@SID", SID));
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
                using (SqlCommand cmd = new SqlCommand(sql, cn, tran))
                {
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

        public static bool GetFileExistOrNull(SqlConnection cn, string SID,string FSID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"

SELECT TOP 1 [FGUID] FROM [dbo].[TB_SQM_Files] WHERE[FGUID]=(
  SELECT TOP 100 FGuid
FROM TB_SQM_Manufacturers_Reliability_File
WHERE CONVERT(NVARCHAR(50), SID)  = @SID AND CONVERT(NVARCHAR(50), FSID)=@FSID
ORDER BY insertime DESC
)
    ");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@SID", SID));
                cmd.Parameters.Add(new SqlParameter("@FSID", FSID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["FGuid"].ToString() != "")
                    {
                        return true;
                    }
                    return false;
                }
                else
                {
                    return false;
                }
                
            }

        }
        public static bool GetDataExistOrNull(SqlConnection cn, string ReliabititySID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"

                 SELECT [SID]
                 FROM [TB_SQM_Manufacturers_Reliability_File]
                 WHERE [SID] = @SID
    ");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@SID", ReliabititySID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }
        public static bool CheckStatus(SqlConnection cn, string MemberGuid) {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"

SELECT Status
    FROM TB_SQM_Approve_Case AS AC 
	--FormNo
	INNER JOIN TB_SQM_Manufacturers_Reliability_Test 
	ON FormNo = CONVERT(nvarchar(50), MemberGUID)
	--CaseID
	INNER JOIN TB_SQM_Manufacturers_Reliability_Test_map AS RTM
	ON CaseID=ReliabilitycaseID
WHERE AC.Status !='Finished' 
AND RTM.MemberGUID=@MemberGUID

    ");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@MemberGUID", MemberGuid));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static string UpdateFileCaseId(SqlConnection cn,String FSID, string caseID)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
IF NOT EXISTS(SELECT MemberGUID FROM TB_SQM_Manufacturers_Reliability_Test_map  WHERE MemberGUID = @MemberGUID)
    BEGIN
            INSERT INTO TB_SQM_Manufacturers_Reliability_Test_map
            (MemberGUID
		    ,FilecaseID)
            VALUES
		    (@MemberGUID
		    ,@FilecaseID)
    END;
Update TB_SQM_Manufacturers_Reliability_Test_map
Set FilecaseID=@FilecaseID
Where MemberGUID=@MemberGUID
    ");

            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.AddWithValue("@FileCaseID", SQMStringHelper.NullOrEmptyStringIsDBNull(caseID));

                cmd.Parameters.AddWithValue("@MemberGUID", FSID);


                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }
            }
            return sErrMsg;
        }
        #endregion
    }
    #endregion
    
    #endregion


    #region ReliInfo Data
    public class SQMReliInfo
    {
        protected string _ReliabilitySID { get; set; }
        protected string _TB_SQM_CommodityCID { get; set; }
        protected string _CommodityName { get; set; }
        protected string _TB_SQM_Commodity_SubCID { get; set; }
        protected string _Commodity_SubName { get; set; }
        protected string _TestProjet { get; set; }
        protected string _PlannedTestTime { get; set; }
        protected string _ActualTestTime { get; set; }
        protected string _TestResult { get; set; }
        protected string _TestPeople { get; set; }
        protected string _Note { get; set; }
        protected string _insertime { get; set; }
        protected string _updateTime { get; set; }
        protected string _userName { get; set; }


        public string ReliabilitySID { get { return this._ReliabilitySID; } set { this._ReliabilitySID = value; } }
        public string TB_SQM_CommodityCID { get { return this._TB_SQM_CommodityCID; } set { this._TB_SQM_CommodityCID = value; } }
        public string CommodityName { get { return this._CommodityName; } set { this._CommodityName = value; } }
        public string TB_SQM_Commodity_SubCID { get { return this._TB_SQM_Commodity_SubCID; } set { this._TB_SQM_Commodity_SubCID = value; } }
        public string Commodity_SubName { get { return this._Commodity_SubName; } set { this._Commodity_SubName = value; } }
        public string TestProjet { get { return this._TestProjet; } set { this._TestProjet = value; } }
        public string PlannedTestTime { get { return this._PlannedTestTime; } set { this._PlannedTestTime = value; } }
        public string ActualTestTime { get { return this._ActualTestTime; } set { this._ActualTestTime = value; } }
        public string TestResult { get { return this._TestResult; } set { this._TestResult = value; } }
        public string TestPeople { get { return this._TestPeople; } set { this._TestPeople = value; } }
        public string Note { get { return this._Note; } set { this._Note = value; } }
        public string insertime { get { return this._insertime; } set { this._insertime = value; } }

        public string updateTime { get { return this._updateTime; } set { this._updateTime = value; } }
        public string userName { get { return this._userName; } set { this._userName = value; } }

        public SQMReliInfo() { }
        public SQMReliInfo(
                string ReliabilitySID,
                string TB_SQM_CommodityCID,
                string CommodityName,
                string TB_SQM_Commodity_SubCID,
                string Commodity_SubName,
                string TestProjet,
                string PlannedTestTime,
                string ActualTestTime,
                string TestResult,
                string TestPeople,
                string Note,
                string insertime,
                string updateTime,
                string userName)
        {
            this._ReliabilitySID = ReliabilitySID;
            this._TB_SQM_CommodityCID = TB_SQM_CommodityCID;
            this._CommodityName = CommodityName;
            this._TB_SQM_Commodity_SubCID = TB_SQM_Commodity_SubCID;
            this._Commodity_SubName = Commodity_SubName;
            this._TestProjet = TestProjet;
            this._PlannedTestTime = PlannedTestTime;
            this._ActualTestTime = ActualTestTime;
            this._TestResult = TestResult;
            this._TestResult = TestResult;
            this._TestPeople = TestPeople;
            this._Note = Note;
            this._insertime = insertime;
            this._updateTime = updateTime;
        }

    }

    public class SQMReliInfo_jQGridJSon
    {
        public List<SQMReliInfo> Rows = new List<SQMReliInfo>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }

    #endregion
    #region SQMReliInfo_Helper
    public static class SQMReliInfo_Helper
    {
        #region Search
        public static string GetDataToJQGridJson(SqlConnection cn, string MemberGUID)
        {
            return GerDateToJQGridJson(cn, "","", MemberGUID);
        }

        public static string GerDateToJQGridJson(SqlConnection cn, string SearchText,string PlantText, string MemberGUID)
        {
            string sSearchText = SearchText.Trim();
            string sWhereClause = "";
            if (sSearchText != "")
                sWhereClause += "    ERP_VNAME like '%'+ @SearchText +'%'  ";
            if (sWhereClause.Length != 0)
                sWhereClause = "  and " + sWhereClause.Substring(0);

            string sPlantText = PlantText.Trim();
            string sWhereClausePlant = "";
            if (sPlantText != "")
                sWhereClausePlant += "    Plant = @PlantText ";
            if (sWhereClausePlant.Length != 0)
                sWhereClausePlant = "  and " + sWhereClausePlant.Substring(0);

            StringBuilder sb = new StringBuilder();
            sb.Append(@"
SELECT TB_VMI_PLANT.plant_name,
		TB_VMI_PLANT.Plant
		,[TB_SQM_Member_Vendor_Map].[MemberGUID]
	  ,TB_VMI_VENDOR_DETAIL.ERP_VNAME
	  ,PORTAL_Members.[NameInChinese]
  FROM [TB_SQM_Member_Vendor_Map]
  left join TB_VMI_VENDOR_DETAIL    on TB_VMI_VENDOR_DETAIL.ERP_VND=[TB_SQM_Member_Vendor_Map].[VendorCode]
  left join TB_VMI_PLANT on TB_VMI_PLANT.plant=[TB_SQM_Member_Vendor_Map].[PlantCode]
  left join PORTAL_Members on  PORTAL_Members.[MemberGUID]=[TB_SQM_Member_Vendor_Map].[MemberGUID]
  --where [PlantCode] in (
  --SELECT 
  --    [PlantCode]
  --FROM [TB_SQM_Member_Plant]

  --where [MemberGUID]=@MemberGUID
  --) 
  where plant in (select plantcode from TB_SQM_Member_Plant where MemberGUID=@MemberGUID)
and [TB_SQM_Member_Vendor_Map].[MemberGUID]!=@MemberGUID
    ");

            DataTable dt = new DataTable();
            sb.Append(sWhereClause);
            sb.Append(sWhereClausePlant);
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                if (sSearchText != "")
                    cmd.Parameters.Add(new SqlParameter("@SearchText", sSearchText));
                if (sPlantText != "")
                    cmd.Parameters.Add(new SqlParameter("@PlantText", sPlantText));

                cmd.Parameters.Add(new SqlParameter("@MemberGUID", MemberGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }

        public static bool GetDataExistOrNull(SqlConnection cn, string ReliabititySID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"

                 SELECT [ReliabititySID]
                 FROM dbo.TB_SQM_Manufacturers_Reliability_Test
                 WHERE [ReliabititySID] = @ReliabititySID
    ");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@ReliabititySID", ReliabititySID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        public static string UpdateCaseId(SqlConnection cn, string SID, string caseID)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
                         Update TB_SQM_Manufacturers_Reliability_Test Set
                         ReliabilitycaseID=@ReliabilitycaseID
                         Where SID = @SID;
            ");

            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.AddWithValue("@ReliabilitycaseID", SQMStringHelper.NullOrEmptyStringIsDBNull(caseID));

                cmd.Parameters.AddWithValue("@SID", SQMStringHelper.NullOrEmptyStringIsDBNull(SID));


                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }
            }
            return sErrMsg;
        }

        public static string UpdateFileCaseId(SqlConnection cn, string SID, string caseID)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
                 Update TB_SQM_Manufacturers_Reliability_File Set
                 FileCaseID=@FileCaseID
                 Where SID = @SID;
    ");

            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.AddWithValue("@FileCaseID", SQMStringHelper.NullOrEmptyStringIsDBNull(caseID));

                cmd.Parameters.AddWithValue("@SID", SQMStringHelper.NullOrEmptyStringIsDBNull(SID));


                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }
            }
            return sErrMsg;
        }


        #endregion

        #region Create/Edit data check
        private static void UnescapeDataFromWeb(SQMReliInfo DataItem)
        {
            DataItem.ReliabilitySID = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ReliabilitySID);
            DataItem.TB_SQM_CommodityCID = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.TB_SQM_CommodityCID);
            DataItem.TB_SQM_Commodity_SubCID = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.TB_SQM_Commodity_SubCID);
            DataItem.TestProjet = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.TestProjet);
            DataItem.PlannedTestTime = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.PlannedTestTime);
            DataItem.TestPeople = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.TestPeople);

        }
        private static string DataCheck(SQMReliInfo DataItem)
        {
            string r = "";
            List<string> e = new List<string>();
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.TB_SQM_CommodityCID))
                e.Add("Must provide TB_SQM_CommodityCID.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.TB_SQM_Commodity_SubCID))
                e.Add("Must provide TB_SQM_Commodity_SubCID.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.TestProjet))
                e.Add("Must provide TestProjet.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.PlannedTestTime))
                e.Add("Must provide PlannedTestTime.");


            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }

        private static string UpdateDataCheck(SQMReliInfo DataItem)
        {
            string r = "";
            List<string> e = new List<string>();
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.ActualTestTime))
                e.Add("Must provide ActualTestTime.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.TestPeople))
                e.Add("Must provide TestPeople.");


            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }

        #endregion


        #region Create data item
        public static string CreateDataItem(SqlConnection cnPortal, SQMReliInfo DataItem)
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
                //insert into 表(字段)
                //    select a.ID,b.ID from A a, B b where A.ID = B.ID
                sb.Append(@"
                    Insert Into TB_SQM_Manufacturers_Reliability_Test ([ReliabititySID]
                        ,TB_SQM_CommodityCID
                        ,TB_SQM_Commodity_SubCID
                        ,TestProjet
                        ,PlannedTestTime

                        ,TestResult
                        ,TestPeople
                        ,Note
                        ,insertime
                        ,updateTime
                        ,userName)
                    Values(@ReliabilitySID, @TB_SQM_CommodityCID, @TB_SQM_Commodity_SubCID, 
                        @TestProjet, @PlannedTestTime, 
                        @TestResult, @TestPeople, @Note, 
                        @insertime, @updateTime, @userName)
    ");

                SQM_Basic_Helper.InsertPart(sb, "3");
                SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal);
                cmd.Parameters.AddWithValue("@ReliabilitySID", Guid.NewGuid());
                cmd.Parameters.AddWithValue("@ReliabilitySID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ReliabilitySID));
                cmd.Parameters.AddWithValue("@TB_SQM_CommodityCID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TB_SQM_CommodityCID));
                cmd.Parameters.AddWithValue("@TB_SQM_Commodity_SubCID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TB_SQM_Commodity_SubCID));
                cmd.Parameters.AddWithValue("@TestProjet", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TestProjet));

                cmd.Parameters.AddWithValue("@PlannedTestTime", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.PlannedTestTime));
                cmd.Parameters.AddWithValue("@TestResult", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TestResult));
                cmd.Parameters.AddWithValue("@TestPeople", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TestPeople));
                cmd.Parameters.AddWithValue("@Note", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Note));
                cmd.Parameters.AddWithValue("@insertime", System.DateTime.Now);
                cmd.Parameters.AddWithValue("@updateTime", System.DateTime.Now);
                cmd.Parameters.AddWithValue("@userName", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.userName));

                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;

                return sErrMsg;
            }
        }
        #endregion

        #region Edit data item
        public static string EditDataItem(SqlConnection cnPortal, SQMReliInfo DataItem)
        {
            return EditDataItem(cnPortal, DataItem, "", "");
        }

        public static string EditDataItem(SqlConnection cnPortal, SQMReliInfo DataItem, string LoginMemberGUID, string RunAsMemberGUID)
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

        private static string EditDataItemSub(SqlCommand cmd, SQMReliInfo DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"

    DECLARE @OldTestProjet nvarchar(50) =(
    SELECT TestProjet FROM TB_SQM_Manufacturers_Reliability_Test 
    Where  [ReliabititySID] = @ReliabilitySID and TestProjet=@TestProjet)

    Update TB_SQM_Manufacturers_Reliability_Test Set
                            TB_SQM_CommodityCID=@TB_SQM_CommodityCID,
                            TB_SQM_Commodity_SubCID=@TB_SQM_Commodity_SubCID,
                            TestProjet=@TestProjet,
                            PlannedTestTime=@PlannedTestTime,
                            TestResult=@TestResult,
                            TestPeople=@TestPeople,
                            Note=@Note,
                            updateTime=@updateTime,
                            userName=@userName
    Where  [ReliabititySID] =@ReliabilitySID and TestProjet =@OldTestProjet;

                ");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@ReliabilitySID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ReliabilitySID));
            cmd.Parameters.AddWithValue("@TB_SQM_CommodityCID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TB_SQM_CommodityCID));
            cmd.Parameters.AddWithValue("@TB_SQM_Commodity_SubCID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TB_SQM_Commodity_SubCID));
            cmd.Parameters.AddWithValue("@TestProjet", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TestProjet));
            cmd.Parameters.AddWithValue("@PlannedTestTime", Convert.ToDateTime(DataItem.PlannedTestTime));
            cmd.Parameters.AddWithValue("@TestResult", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TestResult));
            cmd.Parameters.AddWithValue("@TestPeople", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TestPeople));
            cmd.Parameters.AddWithValue("@Note", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Note));
            cmd.Parameters.AddWithValue("@updateTime", System.DateTime.Now);
            cmd.Parameters.AddWithValue("@userName", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.userName));

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }

        #endregion

        #region Update data item
        public static string UpdateDataItem(SqlConnection cnPortal, SQMReliInfo DataItem)
        {
            return UpdateDataItem(cnPortal, DataItem, "", "");
        }

        public static string UpdateDataItem(SqlConnection cnPortal, SQMReliInfo DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            string r = UpdateDataCheck(DataItem);
            if (r != "")
            { return r; }
            else
            {
                using (SqlCommand cmd = new SqlCommand("", cnPortal)) { r = UpdateDataItemSub(cmd, DataItem); }
                if (r != "") { return r; }
                return r;
            }
        }

        private static string UpdateDataItemSub(SqlCommand cmd, SQMReliInfo DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();



            sb.Append(@"

  INSERT INTO [TB_SQM_Manufacturers_Reliability_File]

  ([SID]
  ,[ActualTestTime]
      ,[TestResult]
      ,[TestPeople]
      ,[Note]
      ,[insertime])
	  VALUES(@SID,@ActualTestTime,@TestResult,@TestPeople,@Note,@insertime)

                ");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@SID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ReliabilitySID));
            cmd.Parameters.AddWithValue("@ActualTestTime", Convert.ToDateTime(DataItem.ActualTestTime));
            cmd.Parameters.AddWithValue("@TestProjet", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TestProjet));
            cmd.Parameters.AddWithValue("@TestResult", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TestResult));
            cmd.Parameters.AddWithValue("@TestPeople", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TestPeople));
            cmd.Parameters.AddWithValue("@Note", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Note));
            cmd.Parameters.AddWithValue("@insertime", System.DateTime.Now);



            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Update fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }

        #endregion

        #region Delete data item
        public static string DeleteDataItem(SqlConnection cnPortal, SQMReliInfo DataItem)
        {
            return DeleteDataItem(cnPortal, DataItem, "", "");
        }

        public static string DeleteDataItem(SqlConnection cnPortal, SQMReliInfo DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {

            string r = "";
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.ReliabilitySID))
                return "Must provide ReliabilitySID.";
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.TestProjet))
                return "Must provide TestProjet.";
            else
            {
                using (SqlCommand cmd = new SqlCommand("", cnPortal)) { r = DeleteDataItemSub(cmd, DataItem); }
                if (r != "") { return r; }
                return r;
            }
        }

        private static string DeleteDataItemSub(SqlCommand cmd, SQMReliInfo DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
                Delete TB_SQM_Manufacturers_Reliability_Test Where [ReliabititySID] = @ReliabilitySID and TestProjet =@TestProjet
    ");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@ReliabilitySID", DataItem.ReliabilitySID);
            cmd.Parameters.AddWithValue("@TestProjet", DataItem.TestProjet);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion

        #region Update File
        public static string UploadReliabilityFile(SqlConnection cn, PortalUserProfile RunAsUser, FileAttachmentInfo FA, string sLocalPath, string sLocalUploadPath, HttpServerUtilityBase Server, string RequestApplicationPath, string SID)
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
                    WHERE FGUID = ( SELECT FGUID
    	            FROM TB_SQM_Manufacturers_Reliability_File
    	            WHERE SID = @SID
                    ");
                String sql = Regex.Replace(sb.ToString(), @"\s+", " ");

                using (SqlCommand cmd = new SqlCommand(sql, cn, tran))
                {
                    cmd.Parameters.Add(new SqlParameter("@SID", StringHelper.NullOrEmptyStringIsEmptyString(SID)));
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

                    IF NOT EXISTS(SELECT* FROM TB_SQM_Manufacturers_Reliability_File WHERE SID = @SID)
                        BEGIN
                             INSERT INTO TB_SQM_Manufacturers_Reliability_File
                             (SID,FSID,FGuid)
                             VALUES
                             (@SID,@FSID,@FGuid)
                        END;
                    UPDATE TB_SQM_Manufacturers_Reliability_File
                      SET FGuid = @FGuid
                    WHERE SID = @SID

                    ");
                String sql = Regex.Replace(sb.ToString(), @"\s+", " ");

                using (SqlCommand cmd = new SqlCommand(sql, cn, tran))
                {
                    cmd.Parameters.Add(new SqlParameter("@SID", StringHelper.NullOrEmptyStringIsEmptyString(SID)));
                    cmd.Parameters.Add(new SqlParameter("@FSID", Guid.NewGuid()));
                    cmd.Parameters.Add(new SqlParameter("@FGUID", FGUID));
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

        public static string UploadFile(SqlConnection cn, PortalUserProfile RunAsUser, FileAttachmentInfo FA, string SID, string sLocalPath, string sLocalUploadPath, HttpServerUtilityBase Server, string RequestApplicationPath)
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

            //01.del esixt file
            try
            {
                DataTable dt = new DataTable();
                StringBuilder sb = new StringBuilder();
                sb.Append(
                @"
                    DECLARE @OldKeyValue uniqueidentifier;

                    SELECT FGUID FROM TB_SQM_Manufacturers_Reliability_Test_map
                    WHERE SID = @SID;

                    UPDATE TB_SQM_Manufacturers_Reliability_Test_map 
                       SET @file=null;
                    DELETE FROM TB_SQM_Files
                    WHERE FGUID =@OldKeyValue
                    ");
                String sql = Regex.Replace(sb.ToString(), @"\s+", " ");
                sql = Regex.Replace(sql, @"@col", col);

                using (SqlCommand cmd = new SqlCommand(sql, cn, tran))
                {
                    cmd.Parameters.Add(new SqlParameter("@SID", SID));
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
                using (SqlCommand cmd = new SqlCommand(sql, cn, tran))
                {
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
        #endregion

        #region Plant ddl
        public static String GetPlantListData(SqlConnection cn, PortalUserProfile RunAsUser)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append(@"
SELECT 
	VP.PLANT AS PlantCode
	,ISNULL(VP.PLANT_NAME,'None') AS PlantName
FROM TB_VMI_PLANT AS VP
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

        public static bool GetFileExistOrNull(SqlConnection cn, string ReliabititySID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"

               SELECT [SID]
        FROM TB_SQM_Manufacturers_Reliability_File
        WHERE [SID] = @SID
    ");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@SID", ReliabititySID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }
    }
    #endregion
    #region SQMBAR
    public class SQM_BAR
    {
        protected string _SubFuncGUID = "";
        protected string _SubFuncName = "";

        public string SubFuncGUID { get { return this._SubFuncGUID; } set { this._SubFuncGUID = value; } }
        public string SubFuncName { get { return this._SubFuncName; } set { this._SubFuncName = value; } }

        public SQM_BAR() { }
        public SQM_BAR(string SubFuncGUID, string SubFuncName)
        {
            this._SubFuncGUID = SubFuncGUID;
            this._SubFuncName = SubFuncName;
        }
    }
    public class SQM_BAR_jQGridJSon
    {
        public List<object[]> Rows = new List<object[]>();
        public List<RowModel> model = new List<RowModel>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }
    public static class SQM_BAR_Helper
    {
        public static string getNewDate(SqlConnection cn, string LoginMemberGUID, string designator, string begin, string end)
        {
            
            DataTable CPKDateDt = getCPKDate(cn, LoginMemberGUID, designator, begin,end);
           List<DataTable>  list = new List<DataTable>();
            DataTable dt = new DataTable("Bar");
            DataTable CPKdt = new DataTable();
            if (CPKDateDt.Rows.Count>0)
            {
                DataColumn dc1 = new DataColumn("TIME", Type.GetType("System.String"));
                dt.Columns.Add(dc1);

                foreach (DataRow datedr in CPKDateDt.Rows)
                {
                    DataColumn dc = new DataColumn(datedr["CTQDATE"].ToString(), Type.GetType("System.String"));
                    dt.Columns.Add(dc);
                    CPKdt = getCPKbydate(cn, LoginMemberGUID, designator, datedr["CTQDATE"].ToString());
                    list.Add(CPKdt);
                    }
               
                DataRow dr = dt.NewRow();
                dr[0] = "AVG";
                DataRow dr1 = dt.NewRow();
                dr1[0] = "STDEV";
                DataRow dr3 = dt.NewRow();
                dr3[0] = "CA";
                DataRow dr4 = dt.NewRow();
                dr4[0] = "CP";
                DataRow dr5 = dt.NewRow();
                dr5[0] = "CPK";
                DataRow dr6 = dt.NewRow();
                dr6[0] = "USL";
                DataRow dr7 = dt.NewRow();
                dr7[0] = "UCL";
             
                DataRow dr9 = dt.NewRow();
                dr9[0] = "LCL";
                DataRow dr10 = dt.NewRow();
                dr10[0] = "LSL";

                for (int i = 0; i < CPKDateDt.Rows.Count; i++)
                {
                    DataTable CPKdata = new DataTable();
                    CPKdata = getCPKDateBydate(cn, LoginMemberGUID, designator,CPKDateDt.Rows[i][0].ToString());
                    decimal STDEV =Convert.ToDecimal( CPKdata.Rows[0]["STDEV"].ToString());
                    decimal USL = Convert.ToDecimal(CPKdata.Rows[0]["USL"].ToString());
                    decimal LSL = Convert.ToDecimal(CPKdata.Rows[0]["LSL"].ToString());
                    decimal SL = Convert.ToDecimal(CPKdata.Rows[0]["SL"].ToString());
                    decimal AVG= Convert.ToDecimal(CPKdata.Rows[0]["AVG"].ToString());
                    decimal CA = 0;
                    if ((USL - SL)!=0)
                    {
                         CA = (AVG - SL) / (USL - SL);
                    }
                    decimal CP = (USL - LSL) / 6 / STDEV;
                    decimal CPK = (1 - Math.Abs(CA)) * CP;
                    dr[i + 1] = CPKdata.Rows[0]["AVG"].ToString();
                    dr1[i+1]= CPKdata.Rows[0]["STDEV"].ToString();
                    dr3[i + 1] = CA;
                    dr4[i + 1] = CP;
                    dr5[i + 1] = CPK;
                    dr6[i + 1] = CPKdata.Rows[0]["USL"].ToString();
                    dr7[i + 1] = CPKdata.Rows[0]["UCL"].ToString();
                    dr9[i + 1] = CPKdata.Rows[0]["LCL"].ToString();
                    dr10[i + 1] = CPKdata.Rows[0]["LSL"].ToString();
                }

                


                dt.Rows.Add(dr);
                dt.Rows.Add(dr1);
                dt.Rows.Add(dr3);
                dt.Rows.Add(dr4);
                dt.Rows.Add(dr5);
                dt.Rows.Add(dr6);
                dt.Rows.Add(dr7);
           
                dt.Rows.Add(dr9);
                dt.Rows.Add(dr10);

                for (int i = 0; i < CPKdt.Rows.Count; i++)
                {
                    DataRow dr2 = dt.NewRow();
                    dr2[0] = "樣本" + (i + 1);
                    for (int j =0 ; j < list.Count; j++)
                    {
                        if (i<list[j].Rows.Count)
                        {
                            dr2[j + 1] = list[j].Rows[i][0].ToString();
                        }
                           
                       
                    }
                    dt.Rows.Add(dr2);
                }
            }
            object josn = new JObject(
                new JProperty("Page", 1), 
                new JProperty("Total", Convert.ToInt32(Math.Ceiling((double)(dt.Rows.Count) / 5)).ToString()), 
                new JProperty("Records", dt.Rows.Count)
                );
            JArray jsonObj = JArray.Parse(ToJson(dt));
            josn = Add(josn, "Rows", jsonObj);
            return josn.ToString();
        }
        /// <summary>添加一个属性  
        ///   
        /// </summary>  
        /// <param name="obj">待添加属性的对象</param>  
        /// <param name="key">键名</param>  
        /// <param name="value">值</param>  
        /// <returns>添加属性后的对象</returns>  
        public static object Add(object obj, string key, object value)
        {
            JObject jObj = JObject.Parse(JsonConvert.SerializeObject(obj));
            jObj.Add(new JProperty(key, value));
            return JsonConvert.DeserializeObject(jObj.ToString());
        }
        /// <summary> 
        /// DataTable转为json 
        /// </summary> 
        /// <param name="dt">DataTable</param> 
        /// <returns>json数据</returns> 
        public static string ToJson(DataTable dt)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    result.Add("D" + dc.ColumnName, dr[dc]);
                }
                list.Add(result);
            }
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(list);
        }
        private static DataTable getCPKDateBydate(SqlConnection cn, string LoginMemberGUID, string designator, string date)
        {
            DataTable dt = new DataTable();
        
                StringBuilder sb = new StringBuilder();
                sb.Append(@"
 select CONVERT(decimal(18, 3),avg(CTQ) )as AVG  ,
  CONVERT(decimal(18, 3),STDEV([CTQ])) as STDEV,
   [usl],
       [lsl],
       [sl],
       [ucl],
       [lcl]
   FROM [TB_SQM_CPK_Report_CTQ] a
   left join [TB_SQM_SFC_DATA] b on b.spc_item=a.Designator
  inner join [TB_SQM_CPK_Report] c on c.reportID=a.reportID
   where designator=@designator and  left(a.[reportID],8)=@date and  c.MemberGUID=@MemberGUID
   group by  [usl],[lsl],[sl],[ucl],[lcl]
");


                using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
                {
                cmd.Parameters.Add(new SqlParameter("@MemberGUID", SQMStringHelper.NullOrEmptyStringIsDBNull(LoginMemberGUID)));
                cmd.Parameters.Add(new SqlParameter("@date", SQMStringHelper.NullOrEmptyStringIsDBNull(date)));
                cmd.Parameters.Add(new SqlParameter("@designator", SQMStringHelper.NullOrEmptyStringIsDBNull(designator)));
                using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        dt.Load(dr);
                    }
                }
            
        
            return dt;
        }

        private static DataTable getCPKbydate(SqlConnection cn, string LoginMemberGUID, string designator, string date)
        {
            DataTable dt = new DataTable();
        
                StringBuilder sb = new StringBuilder();
                sb.Append(@"
SELECT [CTQ]
  FROM [TB_SQM_CPK_Report_CTQ] a
 inner join [TB_SQM_CPK_Report] c on c.reportID=a.reportID
where designator=@designator and left(a.[reportID],8)=@date and  c.MemberGUID=@MemberGUID
");


                using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
                {
                cmd.Parameters.Add(new SqlParameter("@MemberGUID", SQMStringHelper.NullOrEmptyStringIsDBNull(LoginMemberGUID)));
                cmd.Parameters.Add(new SqlParameter("@date", SQMStringHelper.NullOrEmptyStringIsDBNull(date)));
                cmd.Parameters.Add(new SqlParameter("@designator", SQMStringHelper.NullOrEmptyStringIsDBNull(designator)));
                using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        dt.Load(dr);
                    }
                }
            
        
            return dt;
        }
        private static DataTable getCPKDate(SqlConnection cn, string LoginMemberGUID, string designator, string begin, string end)
        {
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(designator) && !string.IsNullOrEmpty(begin) &&! string.IsNullOrEmpty(end))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"
  SELECT DISTINCT left(a.[reportID],8) as CTQDATE 
      
  FROM [TB_SQM_CPK_Report_CTQ] a
 inner join [TB_SQM_CPK_Report] c on c.reportID=a.reportID
where designator=@designator and left(a.[reportID],8) between @begin and @end and  c.MemberGUID=@MemberGUID
");


                using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
                {
                    cmd.Parameters.Add(new SqlParameter("@MemberGUID", SQMStringHelper.NullOrEmptyStringIsDBNull(LoginMemberGUID)));
                    cmd.Parameters.Add(new SqlParameter("@designator", SQMStringHelper.NullOrEmptyStringIsDBNull(designator)));
                    cmd.Parameters.Add(new SqlParameter("@begin", SQMStringHelper.NullOrEmptyStringIsDBNull(begin)));
                    cmd.Parameters.Add(new SqlParameter("@end", SQMStringHelper.NullOrEmptyStringIsDBNull(end)));

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        dt.Load(dr);
                    }
                }
            }

            return dt;
        }

        public static string GetModel(SqlConnection cn, string LoginMemberGUID, string designator, string begin, string end)
        {
            SQM_BAR_jQGridJSon m = new SQM_BAR_jQGridJSon();
            DataTable dt = new DataTable();
            dt= getCPKDate(cn, LoginMemberGUID,designator, begin, end);
            List<ColModel> colmodel = new List<ColModel>();
            ArrayList colname = new ArrayList();
            colname.Add("DTIME");
            colmodel.Add(new ColModel("DTIME", "DTIME", 50, false, false));
            foreach (DataRow dr in dt.Rows)
            {
                colname.Add("D"+dr["CTQDATE"].ToString());
                colmodel.Add(new ColModel("D" + dr["CTQDATE"].ToString(), "D" + dr["CTQDATE"].ToString(), 50, false, false));
            }
            string[] ColName = (string[])colname.ToArray(typeof(string));
            m.model.Add(new RowModel(ColName, colmodel));
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(m);
        }

    }
    #endregion
}