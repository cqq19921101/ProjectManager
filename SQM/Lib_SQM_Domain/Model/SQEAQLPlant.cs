using Lib_Portal_Domain.Model;
using Lib_SQM_Domain.SharedLibs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib_SQE_Domain.Model
{
    #region AQLPlant
    #region AQLPlant Date Definitions
    public class SQEAQLPlant
    {
        protected string _SID;
        protected string _PlantName;

        public string SID { get { return this._SID; } set { this._SID = value; } }
        public string PlantName { get { return this._PlantName; } set { this._PlantName = value; } }

        public SQEAQLPlant() { }

        public SQEAQLPlant(string SID, string PlantName)
        {
            this._SID = SID;
            this._PlantName = PlantName;
        }
    }

    public class SQEAQLPlant_jQGridJSon
    {
        public List<SQEAQLPlant> Rows = new List<SQEAQLPlant>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }
    #endregion

    #region AQLPlant Helper
    public static class SQEAQLPlant_Helper
    {
        #region Search AQLPlant
        public static string GetDataToJQGridJson(SqlConnection cn, SQEAQLPlant DataItem)
        {
            return GetDataToJQGridJson(cn, "", DataItem);
        }

        public static String GetDataToJQGridJson(SqlConnection cn, String SearchText, SQEAQLPlant DataItem)
        {
            string sSearchText = SearchText.Trim();
            string sWhereClause = "";
            if (sSearchText != "")
                sWhereClause += "   PlantName like '%'+ @SearchText+'%'   ";
            if (sWhereClause.Length != 0)
                sWhereClause = "  AND " + sWhereClause.Substring(0);

            StringBuilder sb = new StringBuilder();
            sb.Append(@"
SELECT [SID]
      ,[PlantName]
FROM [TB_SQM_AQL_PLANT]
WHERE SID LIKE '%'
            ");
            DataTable dt = new DataTable();
            sb.Append(sWhereClause + ";");
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                if (sSearchText != "")
                    cmd.Parameters.Add(new SqlParameter("@SearchText", sSearchText));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }
        #endregion

        #region Create/Edit data check
        private static void UnescapeDataFromWeb(SQEAQLPlant DataItem)
        {
            DataItem.PlantName = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.PlantName);
        }

        private static string DataCheck(SQEAQLPlant DataItem)
        {
            string r = "";
            List<string> e = new List<string>();
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.PlantName))
                e.Add("Must provide PlantName.");
            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }
        #endregion

        #region Create data item
        public static string CreateDataItem(SqlConnection cnPortal, SQEAQLPlant DataItem)
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
INSERT INTO [dbo].[TB_SQM_AQL_PLANT]([PlantName])
VALUES(@PlantName)
");
                SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal);
                cmd.Parameters.AddWithValue("@PlantName", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.PlantName));

                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;

                return sErrMsg;
            }
        }


        #endregion

        #region Edit data item
        public static string EditDataItem(SqlConnection cnPortal, SQEAQLPlant DataItem)
        {
            return EditDataItem(cnPortal, DataItem, "", "");
        }

        public static string EditDataItem(SqlConnection cnPortal, SQEAQLPlant DataItem, string LoginMemberGUID, string RunAsMemberGUID)
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

        private static string EditDataItemSub(SqlCommand cmd, SQEAQLPlant DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
UPDATE [dbo].[TB_SQM_AQL_PLANT]
SET [PlantName]=@PlantName
WHERE SID=@SID
");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@SID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SID));
            cmd.Parameters.AddWithValue("@PlantName", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.PlantName));

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion

        #region Delete data item
        public static string DeleteDataItem(SqlConnection cnPortal, SQEAQLPlant DataItem)
        {
            return DeleteDataItem(cnPortal, DataItem, "", "");
        }

        public static string DeleteDataItem(SqlConnection cnPortal, SQEAQLPlant DataItem, string LoginMemberGUID, string RunAsMemberGUID)
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

        private static string DeleteDataItemSub(SqlCommand cmd, SQEAQLPlant DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
DELETE [dbo].[TB_SQM_AQL_PLANT]
WHERE SID=@SID;
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

    #endregion
    #region AQLPlantMap
    #region AQLPlantMap Date Definitions
    public class SQEAQLPlantMap
    {
        protected string _SID;
        protected string _SSID;
        protected string _AQLNum;
        protected string _AQLType;
        protected string _AQL;
        protected string _CR;
        protected string _MA;
        protected string _MI;

        public string SID { get { return this._SID; } set { this._SID = value; } }
        public string SSID { get { return this._SSID; } set { this._SSID = value; } }
        public string AQLNum { get { return this._AQLNum; } set { this._AQLNum = value; } }
        public string AQLType { get { return this._AQLType; } set { this._AQLType = value; } }
        public string AQL { get { return this._AQL; } set { this._AQL = value; } }
        public string CR { get { return this._CR; } set { this._CR = value; } }
        public string MA { get { return this._MA; } set { this._MA = value; } }
        public string MI { get { return this._MI; } set { this._MI = value; } }
        public SQEAQLPlantMap() { }

        public SQEAQLPlantMap(
            string SID
            , string SSID
            , string AQLNum
            , string AQLType
            , string AQL
            , string CR
            , string MA
            , string MI
            )
        {
            this._SID = SID;
            this._SSID = SSID;
            this._AQLNum = AQLNum;
            this._AQLType = AQLType;
            this._AQL = AQL;
            this._CR = CR;
            this._MA = MA;
            this._MI = MI;
        }
    }

    public class SQEAQLPlantMap_jQGridJSon
    {
        public List<SQEAQLPlantMap> Rows = new List<SQEAQLPlantMap>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }
    #endregion

    #region AQLPlantMap Helper
    public static class SQEAQLPlantMap_Helper
    {
        #region Search AQLPlantMap
        public static string GetDataToJQGridJson(SqlConnection cn, SQEAQLPlantMap DataItem)
        {
            return GetDataToJQGridJson(cn, "", DataItem);
        }

        public static String GetDataToJQGridJson(SqlConnection cn, String SearchText, SQEAQLPlantMap DataItem)
        {
            string sSearchText = SearchText.Trim();
            string sWhereClause = "";
            if (sSearchText != "")
                sWhereClause += "   PlantName like '%'+ @SearchText+'%'   ";
            if (sWhereClause.Length != 0)
                sWhereClause = "  AND " + sWhereClause.Substring(0);

            StringBuilder sb = new StringBuilder();
            sb.Append(@"
SELECT [SID]
      ,[SSID]
      ,[AQLNum]
      ,[AQLType]
      ,[AQL]
      ,[CR]
      ,[MA]
      ,[MI]
  FROM [TB_SQM_AQL_PLANT_Map]
WHERE SSID = @SSID
order by sid desc
            ");
            DataTable dt = new DataTable();
            sb.Append(sWhereClause + ";");
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                if (sSearchText != "")
                    cmd.Parameters.Add(new SqlParameter("@SearchText", sSearchText));
                cmd.Parameters.Add(new SqlParameter("@SSID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SSID)));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }
        #endregion

        #region Create/Edit data check
        private static void UnescapeDataFromWeb(SQEAQLPlantMap DataItem)
        {
            DataItem.AQLNum = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.AQLNum);
            DataItem.AQLType = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.AQLType);
            DataItem.AQL = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.AQL);
            DataItem.CR = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CR);
            DataItem.MA = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.MA);
            DataItem.MI = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.MI);
        }

        private static string DataCheck(SQEAQLPlantMap DataItem)
        {
            string r = "";
            List<string> e = new List<string>();
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.AQLNum))
                e.Add("Must provide AQLNum.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.AQLType))
                e.Add("Must provide AQLType.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.AQL))
                e.Add("Must provide AQL.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.CR))
                e.Add("Must provide CR.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.MA))
                e.Add("Must provide MA.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.MI))
                e.Add("Must provide MI.");
            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }
        #endregion

        #region Create data item
        public static string CreateDataItem(SqlConnection cnPortal, SQEAQLPlantMap DataItem)
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
INSERT INTO [dbo].[TB_SQM_AQL_PLANT_Map]
           ([SSID]
           ,[AQLNum]
           ,[AQLType]
           ,[AQL]
           ,[CR]
           ,[MA]
           ,[MI])
     VALUES
           (@SSID 
           ,@AQLNum 
           ,@AQLType 
           ,@AQL
           ,@CR 
           ,@MA 
           ,@MI )
");
                SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal);
                cmd.Parameters.AddWithValue("@SSID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SSID));
                cmd.Parameters.AddWithValue("@AQLNum", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.AQLNum));
                cmd.Parameters.AddWithValue("@AQLType", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.AQLType));
                cmd.Parameters.AddWithValue("@AQL", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.AQL));
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


        #endregion

        #region Edit data item
        public static string EditDataItem(SqlConnection cnPortal, SQEAQLPlantMap DataItem)
        {
            return EditDataItem(cnPortal, DataItem, "", "");
        }

        public static string EditDataItem(SqlConnection cnPortal, SQEAQLPlantMap DataItem, string LoginMemberGUID, string RunAsMemberGUID)
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

        private static string EditDataItemSub(SqlCommand cmd, SQEAQLPlantMap DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
UPDATE [dbo].[TB_SQM_AQL_PLANT_Map]
   SET
       [AQLNum] = @AQLNum
      ,[AQLType] = @AQLType
      ,[AQL] = @AQL
      ,[CR] = @CR
      ,[MA] = @MA
      ,[MI] = @MI
 WHERE SID=@SID AND SSID=@SSID
");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@SID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SID));
            cmd.Parameters.AddWithValue("@SSID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SSID));
            cmd.Parameters.AddWithValue("@AQLNum", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.AQLNum));
            cmd.Parameters.AddWithValue("@AQLType", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.AQLType));
            cmd.Parameters.AddWithValue("@AQL", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.AQL));
            cmd.Parameters.AddWithValue("@CR", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.CR));
            cmd.Parameters.AddWithValue("@MA", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.MA));
            cmd.Parameters.AddWithValue("@MI", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.MI));

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion

        #region Delete data item
        public static string DeleteDataItem(SqlConnection cnPortal, SQEAQLPlantMap DataItem)
        {
            return DeleteDataItem(cnPortal, DataItem, "", "");
        }

        public static string DeleteDataItem(SqlConnection cnPortal, SQEAQLPlantMap DataItem, string LoginMemberGUID, string RunAsMemberGUID)
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

        private static string DeleteDataItemSub(SqlCommand cmd, SQEAQLPlantMap DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
DELETE [dbo].[TB_SQM_AQL_PLANT_Map]
WHERE SID=@SID;
");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@SID", DataItem.SID);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        public static String GetMapAQLType(SqlConnection cn, PortalUserProfile RunAsUser, SQEAQLPlantMap DataItem)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append(@"
SELECT	[SID]
      ,[SSID]
      ,[AQLNum]
      ,[AQLType]
      ,[CR]
      ,[MA]
      ,[MI]
  FROM [TB_SQM_AQL_PLANT_Map]
WHERE SSID=@SID
                    ");

            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.AddWithValue("@SID",SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SID));
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
    #region AQLPlantRule
    #region AQLPlantRule Date Definitions
    public class SQEAQLPlantRule
    {
        protected string _SID;
        protected string _SSID;
        protected string _SSSID;
        protected string _CheckNum;
        protected string _RetreatingNum;
        protected string _AcceptanceNum;
        protected string _AQLType;
        protected string _PlantSID;
        protected string _IsShow;
        public string SID { get { return this._SID; } set { this._SID = value; } }
        public string SSID{ get { return this._SSID; } set { this._SSID = value; } }
        public string SSSID{ get { return this._SSSID; } set { this._SSSID = value; } }
        public string CheckNum{ get { return this._CheckNum; } set { this._CheckNum = value; } }
        public string RetreatingNum{ get { return this._RetreatingNum; } set { this._RetreatingNum = value; } }
        public string AcceptanceNum{ get { return this._AcceptanceNum; } set { this._AcceptanceNum = value; } }
        public string AQLType{ get { return this._AQLType; } set { this._AQLType = value; } }
        public string PlantSID { get { return this._PlantSID; } set { this._PlantSID = value; } }
        public string IsShow { get { return this._IsShow; } set { this._IsShow = value; } }

        public SQEAQLPlantRule() { }

        public SQEAQLPlantRule(
            string SID,
            string SSID,
            string SSSID,
            string CheckNum,
            string RetreatingNum,
            string AcceptanceNum,
            string AQLType,
            string PlantSID,
            string IsShow
            )
        {
            this._SID = SID;
            this._SSID = SSID;
            this._SSSID = SSSID;
            this._CheckNum = CheckNum;
            this._RetreatingNum = RetreatingNum;
            this._AcceptanceNum = AcceptanceNum;
            this._AQLType = AQLType;
            this._PlantSID = PlantSID;
            this._IsShow = IsShow;
        }
    }

    public class SQEAQLPlantRule_jQGridJSon
    {
        public List<SQEAQLPlantRule> Rows = new List<SQEAQLPlantRule>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }
    #endregion

    #region AQLPlantRule Helper
    public static class SQEAQLPlantRule_Helper
    {
        #region Search AQLPlantRule
        public static string GetDataToJQGridJson(SqlConnection cn, SQEAQLPlantRule DataItem)
        {
            return GetDataToJQGridJson(cn, "", DataItem);
        }

        public static String GetDataToJQGridJson(SqlConnection cn, String SearchText, SQEAQLPlantRule DataItem)
        {
            string sSearchText = SearchText.Trim();
            string sWhereClause = "";
            if (sSearchText != "")
                sWhereClause += "   P.PlantName like '%'+ @SearchText+'%'   ";
            if (sWhereClause.Length != 0)
                sWhereClause = "  AND " + sWhereClause.Substring(0);

            string sIsShow = string.Empty;
            string sWhereClauseShow = "";
            if (DataItem.IsShow != null)
            {
                sIsShow = DataItem.IsShow.Trim();
                if (sIsShow != "")
                    sWhereClauseShow += "  IsShow=@IsShow  ";
                if (sWhereClauseShow.Length != 0)
                    sWhereClauseShow = "  AND " + sWhereClauseShow.Substring(0);
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(@"
SELECT R.SID,
         R.SSID,
         P.PlantName,
        R.SSSID,
         R.SSSID as AQLType,
         R.CheckNum,
         R.RetreatingNum,
         R.AcceptanceNum,
         R.AQLType AS AfterAQLType,
         R.PlantSID,
         R.IsShow,
         PS.PlantName AS AfterPlantName
FROM TB_SQM_AQL_PLANT_Rule AS R LEFT OUTER
JOIN TB_SQM_AQL_PLANT AS P
    ON R.SSID = P.SID LEFT OUTER
JOIN TB_SQM_AQL_PLANT AS PS
    ON R.PlantSID = PS.SID
WHERE R.SID LIKE '%'
            ");
            DataTable dt = new DataTable();
            sb.Append(sWhereClause + sWhereClauseShow + ";");
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                if (sSearchText != "")
                    cmd.Parameters.Add(new SqlParameter("@SearchText", sSearchText));
                if (sIsShow != "")
                    cmd.Parameters.Add(new SqlParameter("@IsShow", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.IsShow)));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }
        #endregion

        #region Create/Edit data check
        private static void UnescapeDataFromWeb(SQEAQLPlantRule DataItem)
        {
            DataItem.SSID = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.SSID);
            DataItem.SSSID = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.SSSID);
            DataItem.CheckNum = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CheckNum);
            DataItem.RetreatingNum = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.RetreatingNum);
            DataItem.AcceptanceNum = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.AcceptanceNum);
            DataItem.AQLType = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.AQLType);
            DataItem.PlantSID = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.PlantSID);
        }

        private static string DataCheck(SQEAQLPlantRule DataItem)
        {
            string r = "";
            List<string> e = new List<string>();
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.SSID))
                e.Add("Must provide SSID.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.SSSID))
                e.Add("Must provide SSSID.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.CheckNum))
                e.Add("Must provide CheckNum.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.RetreatingNum))
                e.Add("Must provide RetreatingNum.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.AcceptanceNum))
                e.Add("Must provide AcceptanceNum.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.AQLType))
                e.Add("Must provide AQLType.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.PlantSID))
                e.Add("Must provide PlantSID.");

            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }
        #endregion

        #region Create data item
        public static string CreateDataItem(SqlConnection cnPortal, SQEAQLPlantRule DataItem)
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
INSERT INTO [dbo].[TB_SQM_AQL_PLANT_Rule]
           ([SSID]
           ,[SSSID]
           ,[CheckNum]
           ,[RetreatingNum]
           ,[AcceptanceNum]
           ,[AQLType]
           ,[PlantSID]
           ,[IsShow])
     VALUES
           (@SSID
           ,@SSSID
           ,@CheckNum
           ,@RetreatingNum
           ,@AcceptanceNum
           ,@AQLType
           ,@PlantSID
           ,'1')
");
                SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal);
                cmd.Parameters.AddWithValue("@SSID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SSID));
                cmd.Parameters.AddWithValue("@SSSID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SSSID));
                cmd.Parameters.AddWithValue("@CheckNum", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.CheckNum));
                cmd.Parameters.AddWithValue("@RetreatingNum", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.RetreatingNum));
                cmd.Parameters.AddWithValue("@AcceptanceNum", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.AcceptanceNum));
                cmd.Parameters.AddWithValue("@AQLType", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.AQLType));
                cmd.Parameters.AddWithValue("@PlantSID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.PlantSID));

                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;

                return sErrMsg;
            }
        }


        #endregion

        #region Edit data item
        public static string EditDataItem(SqlConnection cnPortal, SQEAQLPlantRule DataItem)
        {
            return EditDataItem(cnPortal, DataItem, "", "");
        }

        public static string EditDataItem(SqlConnection cnPortal, SQEAQLPlantRule DataItem, string LoginMemberGUID, string RunAsMemberGUID)
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

        private static string EditDataItemSub(SqlCommand cmd, SQEAQLPlantRule DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
UPDATE [dbo].[TB_SQM_AQL_PLANT_Rule]
   SET [SSID] = @SSID
      ,[SSSID] = @SSSID
      ,[CheckNum] = @CheckNum
      ,[RetreatingNum] = @RetreatingNum
      ,[AcceptanceNum] = @AcceptanceNum
      ,[AQLType] = @AQLType
      ,[PlantSID] = @PlantSID
 WHERE SID=@SID
");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@SID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SID));
            cmd.Parameters.AddWithValue("@SSID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SSID));
            cmd.Parameters.AddWithValue("@SSSID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SSSID));
            cmd.Parameters.AddWithValue("@CheckNum", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.CheckNum));
            cmd.Parameters.AddWithValue("@RetreatingNum", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.RetreatingNum));
            cmd.Parameters.AddWithValue("@AcceptanceNum", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.AcceptanceNum));
            cmd.Parameters.AddWithValue("@AQLType", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.AQLType));
            cmd.Parameters.AddWithValue("@PlantSID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.PlantSID));


            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion

        #region Delete data item
        public static string DeleteDataItem(SqlConnection cnPortal, SQEAQLPlantRule DataItem)
        {
            return DeleteDataItem(cnPortal, DataItem, "", "");
        }

        public static string DeleteDataItem(SqlConnection cnPortal, SQEAQLPlantRule DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {

            string r = "";
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.SID))
                return "Must provide SID.";
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.SSID))
                return "Must provide SSID.";
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.SSSID))
                return "Must provide SSSID.";
            else
            {
                using (SqlCommand cmd = new SqlCommand("", cnPortal)) { r = DeleteDataItemSub(cmd, DataItem); }
                if (r != "") { return r; }
                return r;
            }
        }

        private static string DeleteDataItemSub(SqlCommand cmd, SQEAQLPlantRule DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
DELETE [dbo].[TB_SQM_AQL_PLANT_Rule]
WHERE SID=@SID AND SSID=@SSID AND SSSID=@SSSID;
");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@SID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SID));
            cmd.Parameters.AddWithValue("@SSID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SSID));
            cmd.Parameters.AddWithValue("@SSSID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SSSID));

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion

        #region Disabled data item
        public static string DisabledDataItem(SqlConnection cnPortal, SQEAQLPlantRule DataItem)
        {
            return DisabledDataItem(cnPortal, DataItem, "", "");
        }

        public static string DisabledDataItem(SqlConnection cnPortal, SQEAQLPlantRule DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {

            string r = "";
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.SID))
                return "Must provide SID.";
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.SSID))
                return "Must provide SSID.";
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.SSSID))
                return "Must provide SSSID.";
            else
            {
                using (SqlCommand cmd = new SqlCommand("", cnPortal)) { r = DisabledDataItemSub(cmd, DataItem); }
                if (r != "") { return r; }
                return r;
            }
        }

        private static string DisabledDataItemSub(SqlCommand cmd, SQEAQLPlantRule DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
IF 0 = (SELECT [IsShow]
        FROM [TB_SQM_AQL_PLANT_Rule]
        WHERE SID=@SID AND SSID=@SSID AND SSSID=@SSSID) 
	UPDATE [dbo].[TB_SQM_AQL_PLANT_Rule]
    SET [IsShow] = '1'
    WHERE SID=@SID AND SSID=@SSID AND SSSID=@SSSID;
ELSE 
    UPDATE [dbo].[TB_SQM_AQL_PLANT_Rule]
    SET [IsShow] = '0'
    WHERE SID=@SID AND SSID=@SSID AND SSSID=@SSSID;
");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@SID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SID));
            cmd.Parameters.AddWithValue("@SSID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SSID));
            cmd.Parameters.AddWithValue("@SSSID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SSSID));

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion
        public static String GetPlantNameList(SqlConnection cn, PortalUserProfile RunAsUser)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append(@"
SELECT [SID]
      ,[PlantName]
FROM [TB_SQM_AQL_PLANT]
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
        public static String GetPlantMapList(SqlConnection cn, PortalUserProfile RunAsUser, string MainID)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append(@"
SELECT DISTINCT 
      [AQLType]
  FROM [TB_SQM_AQL_PLANT_Map]
  WHERE [SSID]=@SSSID
                    ");

            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.AddWithValue("@SSSID", SQMStringHelper.NullOrEmptyStringIsDBNull(MainID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);

        }
    }
    #endregion

    #endregion
}
