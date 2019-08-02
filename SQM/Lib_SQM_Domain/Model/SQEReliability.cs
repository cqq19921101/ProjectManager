using Lib_Portal_Domain.Model;
using Lib_Portal_Domain.SharedLibs;
using Lib_SQM_Domain.SharedLibs;
using Lib_VMI_Domain.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;

namespace Lib_SQM_Domain.Model
{
    #region Data Class Definitions
    #region Reliability
    public class SQEReliability
    {
        protected string _SID;
        protected string _ReliabilityName;

        //附件

        public string SID { get { return this._SID; } set { this._SID = value; } }
        public string ReliabilityName { get { return this._ReliabilityName; } set { this._ReliabilityName = value; } }

        public SQEReliability() { }
        public SQEReliability(
            string SID,
            string ReliabilityName)
        {
            this._SID = SID;
            this._ReliabilityName = ReliabilityName;
        }

    }

    public class SQEReliability_jQGridJSon
    {
        public List<SQEReliability> Rows = new List<SQEReliability>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }
    #endregion



    #endregion


    #region Reliability
    public static class SQEReliability_Helper
    {
        #region Search
        public static string GetDataToJQGridJson(SqlConnection cn, string MemberGUID)
        {
            return GerDateToJQGridJson(cn, "", MemberGUID);
        }

        public static string GerDateToJQGridJson(SqlConnection cn, string SearchText, string MemberGUID)
        {
            SQEReliability_jQGridJSon m = new SQEReliability_jQGridJSon();

            string sSearchText = SearchText.Trim();
            string sWhereClause = "";
            if (sSearchText != "")
                sWhereClause += "   ReliabilityName = @SearchText   ";
            if (sWhereClause.Length != 0)
                sWhereClause = "  and " + sWhereClause.Substring(0);

            m.Rows.Clear();
            int iRowCount = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
            SELECT SID
                  ,ReliabilityName
             FROM LiteOnRFQTraining.dbo.TB_SQM_Manufacturers_Reliability_Test_map
             WHERE MemberGUID = @MemberGUID
                    ");

            //LEFT JOIN TB_SQM_Commodity on TB_SQM_Manufacturers_Reliability_Test.TB_SQM_CommodityCID = TB_SQM_Commodity.CID
            // LEFT JOIN TB_SQM_Commodity_Sub on TB_SQM_Manufacturers_Reliability_Test.TB_SQM_Commodity_SubCID = TB_SQM_Commodity_Sub.CID



            sb.Append(sWhereClause + ";");
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                if (sSearchText != "")
                    cmd.Parameters.Add(new SqlParameter("@SearchText", sSearchText));
                cmd.Parameters.Add(new SqlParameter("@MemberGUID", SQMStringHelper.NullOrEmptyStringIsDBNull(MemberGUID)));
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    iRowCount++;
                    m.Rows.Add(new SQEReliability(
                        dr["SID"].ToString(),
                        dr["ReliabilityName"].ToString()
                        ));
                }
                dr.Close();
                dr = null;
            }

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(m);
        }

        public static string UpdateCaseId(SqlConnection cn, string SID, string caseID)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
             Update TB_SQM_Manufacturers_Reliability_Test_map Set
             ReliabilitycaseID=@ReliabilitycaseID
             Where SID = @SID
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

        #endregion

        #region Create/Edit data check
        private static void UnescapeDataFromWeb(SQEReliability DataItem)
        {
            DataItem.ReliabilityName = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ReliabilityName);
        }
        private static string DataCheck(SQEReliability DataItem)
        {
            string r = "";
            List<string> e = new List<string>();
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.ReliabilityName))
                e.Add("Must provide ReliabilityName.");

            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }

        #endregion


        #region Create data item
        public static string CreateDataItem(SqlConnection cnPortal, SQEReliability DataItem, string MemberGUID)
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
                    Insert Into TB_SQM_Manufacturers_Reliability_Test_map 
                        (SID,MemberGUID,ReliabilityName)
                    Values(@SID, @MemberGUID,@ReliabilityName)
                    ");

                //SQM_Basic_Helper.InsertPart(sb, "3");
                SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal);
                cmd.Parameters.AddWithValue("@SID", Guid.NewGuid());
                cmd.Parameters.AddWithValue("@MemberGUID", MemberGUID);
                cmd.Parameters.AddWithValue("@ReliabilityName", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ReliabilityName));

                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;

                return sErrMsg;
            }
        }
        #endregion

        #region Edit data item
        public static string EditDataItem(SqlConnection cnPortal, SQEReliability DataItem)
        {
            return EditDataItem(cnPortal, DataItem, "", "");
        }

        public static string EditDataItem(SqlConnection cnPortal, SQEReliability DataItem, string LoginMemberGUID, string RunAsMemberGUID)
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

        private static string EditDataItemSub(SqlCommand cmd, SQEReliability DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
                     Update TB_SQM_Manufacturers_Reliability_Test_map Set
                     ReliabilityName=@ReliabilityName
                     Where SID = @SID
            ");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@SID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SID));
            cmd.Parameters.AddWithValue("@ReliabilityName", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ReliabilityName));

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }

        #endregion

        #region Delete data item
        public static string DeleteDataItem(SqlConnection cnPortal, SQEReliability DataItem)
        {
            return DeleteDataItem(cnPortal, DataItem, "", "");
        }

        public static string DeleteDataItem(SqlConnection cnPortal, SQEReliability DataItem, string LoginMemberGUID, string RunAsMemberGUID)
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

        private static string DeleteDataItemSub(SqlCommand cmd, SQEReliability DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
Delete TB_SQM_Manufacturers_Reliability_File Where SID = @SID;
Delete TB_SQM_Manufacturers_Reliability_Test Where [ReliabititySID] = @SID;
Delete TB_SQM_Manufacturers_Reliability_Test_map Where SID = @SID;
    
");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@SID", DataItem.SID);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion
    }
    #endregion



    #region ReliInfo Data
    public class SQEReliInfo
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

        public SQEReliInfo() { }
        public SQEReliInfo(
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

    public class SQEReliInfo_jQGridJSon
    {
        public List<SQEReliInfo> Rows = new List<SQEReliInfo>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }

    #endregion
    #region ReliInfo
    public static class SQEReliInfo_Helper
    {
        #region Search
        public static string GetDataToJQGridJson(SqlConnection cn, string MemberGUID)
        {
            return GerDateToJQGridJson(cn, "", MemberGUID);
        }

        public static string GerDateToJQGridJson(SqlConnection cn, string SearchText, string ReliabititySID)
        {
            SQEReliInfo_jQGridJSon m = new SQEReliInfo_jQGridJSon();

            string sSearchText = SearchText.Trim();
            string sWhereClause = "";
            if (sSearchText != "")
                sWhereClause += "   TestProjet = @SearchText   ";
            if (sWhereClause.Length != 0)
                sWhereClause = "  and " + sWhereClause.Substring(0);

            m.Rows.Clear();
            int iRowCount = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
             
             SELECT  [ReliabititySID]
		            ,TB_SQM_Manufacturers_Reliability_Test.TB_SQM_CommodityCID
                    ,TB_SQM_Commodity.CNAME as CommodityName
                    ,TB_SQM_Commodity_SubCID
                    ,TB_SQM_Commodity_Sub.CNAME as Commodity_SubName
		            ,TestProjet
		            ,PlannedTestTime
		            ,ActualTestTime
		            ,TestResult
		            ,TestPeople
		            ,Note
		            ,insertime
		            ,updateTime
		            ,userName
             FROM LiteOnRFQTraining.dbo.TB_SQM_Manufacturers_Reliability_Test
             LEFT JOIN TB_SQM_Commodity on TB_SQM_Manufacturers_Reliability_Test.TB_SQM_CommodityCID = TB_SQM_Commodity.CID
             LEFT JOIN TB_SQM_Commodity_Sub on TB_SQM_Manufacturers_Reliability_Test.TB_SQM_Commodity_SubCID = TB_SQM_Commodity_Sub.CID
             WHERE [ReliabititySID] = @ReliabititySID
");

            sb.Append(sWhereClause + ";");
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                if (sSearchText != "")
                    cmd.Parameters.Add(new SqlParameter("@SearchText", sSearchText));
                cmd.Parameters.Add(new SqlParameter("@ReliabititySID", SQMStringHelper.NullOrEmptyStringIsDBNull(ReliabititySID)));
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    iRowCount++;
                    m.Rows.Add(new SQEReliInfo(

                        dr["ReliabititySID"].ToString(),
                        dr["TB_SQM_CommodityCID"].ToString(),
                        dr["CommodityName"].ToString(),
                        dr["TB_SQM_Commodity_SubCID"].ToString(),
                        dr["Commodity_SubName"].ToString(),
                        dr["TestProjet"].ToString(),
                        dr["PlannedTestTime"].ToString(),
                        dr["ActualTestTime"].ToString(),
                        dr["TestResult"].ToString(),
                        dr["TestPeople"].ToString(),
                        dr["Note"].ToString(),
                        dr["insertime"].ToString(),
                        dr["updateTime"].ToString(),
                        dr["userName"].ToString()

                        ));
                }
                dr.Close();
                dr = null;
            }

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(m);
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
        private static void UnescapeDataFromWeb(SQEReliInfo DataItem)
        {
            DataItem.ReliabilitySID = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ReliabilitySID);
            DataItem.TB_SQM_CommodityCID = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.TB_SQM_CommodityCID);
            DataItem.TB_SQM_Commodity_SubCID = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.TB_SQM_Commodity_SubCID);
            DataItem.TestProjet = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.TestProjet);
            DataItem.PlannedTestTime = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.PlannedTestTime);
            DataItem.TestPeople = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.TestPeople);

        }
        private static string DataCheck(SQEReliInfo DataItem)
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

        #endregion


        #region Create data item
        public static string CreateDataItem(SqlConnection cnPortal, SQEReliInfo DataItem)
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
                //select a.ID,b.ID from A a, B b where A.ID = B.ID
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

                //SQM_Basic_Helper.InsertPart(sb, "3");
                SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal);
                //cmd.Parameters.AddWithValue("@ReliabilitySID", Guid.NewGuid());
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
        public static string EditDataItem(SqlConnection cnPortal, SQEReliInfo DataItem)
        {
            return EditDataItem(cnPortal, DataItem, "", "");
        }

        public static string EditDataItem(SqlConnection cnPortal, SQEReliInfo DataItem, string LoginMemberGUID, string RunAsMemberGUID)
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

        private static string EditDataItemSub(SqlCommand cmd, SQEReliInfo DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
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
                 Where  [ReliabititySID] = @ReliabilitySID
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
            cmd.Parameters.AddWithValue("@UserName", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.userName));

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }

        #endregion

        #region Delete data item
        public static string DeleteDataItem(SqlConnection cnPortal, SQEReliInfo DataItem)
        {
            return DeleteDataItem(cnPortal, DataItem, "", "");
        }

        public static string DeleteDataItem(SqlConnection cnPortal, SQEReliInfo DataItem, string LoginMemberGUID, string RunAsMemberGUID)
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

        private static string DeleteDataItemSub(SqlCommand cmd, SQEReliInfo DataItem)
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
                         (SID,FGuid)
                         VALUES
                         (@SID,@FGuid)
                    END;
                UPDATE TB_SQM_Manufacturers_Reliability_File
                  SET FGuid = @FGuid
                WHERE SID = @SID

                ");
                String sql = Regex.Replace(sb.ToString(), @"\s+", " ");

                using (SqlCommand cmd = new SqlCommand(sql, cn, tran))
                {
                    cmd.Parameters.Add(new SqlParameter("@SID", StringHelper.NullOrEmptyStringIsEmptyString(SID)));
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
                IF NOT EXISTS (SELECT * FROM LiteOnRFQTraining.dbo.TB_SQM_CriticalFile WHERE VendorCode = @VendorCode) 
                    BEGIN
                         INSERT INTO LiteOnRFQTraining.dbo.TB_SQM_CriticalFile
                         (VendorCode) 
                         VALUES
                         (@VendorCode) 
                    END;
                UPDATE LiteOnRFQTraining.dbo.TB_SQM_CriticalFile
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


    }
    #endregion
}
