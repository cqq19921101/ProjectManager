using Lib_SQM_Domain.SharedLibs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Lib_SQM_Domain.Model
{
    #region Insp
    #region Insp Date Definitions
    public class SQMInsp
    {
        protected string _SID;
        protected string _LitNo;
        protected string _Name;
        protected string _Insptype;
        public string SID { get { return this._SID; } set { this._SID = value; } }
        public string LitNo { get { return this._LitNo; } set { this._LitNo = value; } }
        public string Name { get { return this._Name; } set { this._Name = value; } }
        public string Insptype { get { return this._Insptype; } set { this._Insptype = value; } }

        public SQMInsp() { }

        public SQMInsp(string SID,string LitNo ,string Name,string Insptype)
        {
            this._SID = SID;
            this._LitNo = LitNo;
            this._Name = Name;
            this._Insptype = Insptype;
        }
    }

    public class SQMInsp_jQGridJSon
    {
        public List<SQMInsp> Rows = new List<SQMInsp>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }
    #endregion

    #region Insp Helper
    public static class SQMInsp_Helper
    {
        #region Search Insp
        public static string GetDataToJQGridJson(SqlConnection cn, SQMInsp DataItem)
        {
            return GetDataToJQGridJson(cn, "", DataItem);
        }

        public static String GetDataToJQGridJson(SqlConnection cn, String SearchText, SQMInsp DataItem)
        {
            string sSearchText = SearchText.Trim();
            string sWhereClause = "";
            if (sSearchText != "")
                sWhereClause += "   Name like '%'+ @SearchText+'%'   ";
            if (sWhereClause.Length != 0)
                sWhereClause = "  AND " + sWhereClause.Substring(0);

            StringBuilder sb = new StringBuilder();
            sb.Append(@"
SELECT SID
,LitNo
     ,Name
     ,Insptype
FROM dbo.TB_SQM_InspCode
WHERE SID LIKE '%'
            ");
            DataTable dt = new DataTable();
            if (DataItem.Insptype!=null)
            {
                sb.Append("AND Insptype = @Insptype");
            }
            sb.Append(sWhereClause + ";");
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                if (sSearchText != "")
                    cmd.Parameters.Add(new SqlParameter("@SearchText", sSearchText));
                if (DataItem.Insptype != null)
                {
                    cmd.Parameters.Add(new SqlParameter("@Insptype",SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Insptype)));
                }
                //cmd.Parameters.Add(new SqlParameter("@MemberGUID", MemberGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }
        #endregion

        #region Create/Edit data check
        private static void UnescapeDataFromWeb(SQMInsp DataItem)
        {
            DataItem.Name = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Name);
            DataItem.Insptype = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Insptype);
            //DataItem.MOQ = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.MOQ);

        }

        private static string DataCheck(SQMInsp DataItem)
        {
            string r = "";
            List<string> e = new List<string>();
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.Name))
                e.Add("Must provide Name.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.Insptype))
                e.Add("Must provide Insptype.");
            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }
        #endregion

        #region Create data item
        public static string CreateDataItem(SqlConnection cnPortal, SQMInsp DataItem)
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
INSERT INTO [dbo].[TB_SQM_InspCode](LitNO,Name,Insptype)
VALUES(@LitNO,@Name,@Insptype)
");
                //SQM_Basic_Helper.InsertPart(sb, "3");
                SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal);
                cmd.Parameters.AddWithValue("@LitNO", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.LitNo));
                cmd.Parameters.AddWithValue("@Name", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Name));
                cmd.Parameters.AddWithValue("@Insptype", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Insptype));

                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;

                return sErrMsg;
            }
        }


        #endregion

        #region Edit data item
        public static string EditDataItem(SqlConnection cnPortal, SQMInsp DataItem)
        {
            return EditDataItem(cnPortal, DataItem, "", "");
        }

        public static string EditDataItem(SqlConnection cnPortal, SQMInsp DataItem, string LoginMemberGUID, string RunAsMemberGUID)
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

        private static string EditDataItemSub(SqlCommand cmd, SQMInsp DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
UPDATE [dbo].[TB_SQM_InspCode]
SET Name=@Name,Insptype=@Insptype
WHERE SID=@SID
");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@SID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SID));
            cmd.Parameters.AddWithValue("@Name", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Name));
            cmd.Parameters.AddWithValue("@Insptype", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Insptype));

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion

        #region Delete data item
        public static string DeleteDataItem(SqlConnection cnPortal, SQMInsp DataItem)
        {
            return DeleteDataItem(cnPortal, DataItem, "", "");
        }

        public static string DeleteDataItem(SqlConnection cnPortal, SQMInsp DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {

            string r = "";
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.SID))
                return "Must provide SID.";
            else
            {
                using (SqlCommand cmd = new SqlCommand("", cnPortal)) { r = DeleteDataItemSub(cmd, DataItem); }
                if (r != "") { return r; }

                //SqlTransaction tran = cnPortal.BeginTransaction();

                //using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = DeleteDataItemSub(cmd, DataItem); }
                //if (r != "") { tran.Rollback(); return r; }

                //if ((LoginMemberGUID != "") && (RunAsMemberGUID != ""))
                //    using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = DataLockHelper.ReleaseLock(cmd, DataItem.BasicInfoGUID, LoginMemberGUID, RunAsMemberGUID); }
                //if (r != "") { tran.Rollback(); return r; }

                //try { tran.Commit(); }
                //catch (Exception e) { tran.Rollback(); r = "Delete fail.<br />Exception: " + e.ToString(); }
                return r;
            }
        }

        private static string DeleteDataItemSub(SqlCommand cmd, SQMInsp DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
DELETE [dbo].[TB_SQM_InspCode]
WHERE SID=@SID;
DELETE [dbo].[TB_SQM_InspCode_Map]
WHERE SSID=@SID
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

    #region InspMap
    #region InspMap Date Definitions
    public class SQMInspMap
    {
        protected string _SID;
        protected string _SSID;
        protected string _InspDesc;
        protected string _InspNum;
        protected string _Standard;
        protected string _InspResult;
        protected string _CR;
        protected string _MA;
        protected string _MI;
        protected string _Other;
        protected string _isOther;
        protected string _UCL;
        protected string _LCL;
        protected string _InspTool;
        protected string _AQL;
        protected string _Insptype;

        public string SID { get { return this._SID; } set { this._SID = value; } }
        public string SSID { get { return this._SSID; } set { this._SSID = value; } }
        public string InspDesc { get { return this._InspDesc; } set { this._InspDesc = value; } }
        public string InspNum { get { return this._InspNum; } set { this._InspNum = value; } }
        public string Standard { get { return this._Standard; } set { this._Standard = value; } }
        public string InspResult { get { return this._InspResult; } set { this._InspResult = value; } }
        public string CR { get { return this._CR; } set { this._CR = value; } }
        public string MA { get { return this._MA; } set { this._MA = value; } }
        public string MI { get { return this._MI; } set { this._MI = value; } }
        public string Other { get { return this._Other; } set { this._Other = value; } }
        public string isOther { get { return this._isOther; } set { this._isOther = value; } }
        public string UCL { get { return this._UCL; } set { this._UCL = value; } }
        public string LCL { get { return this._LCL; } set { this._LCL = value; } }
        public string InspTool { get { return this._InspTool; } set { this._InspTool = value; } }
        public string AQL { get { return this._AQL; } set { this._AQL = value; } }
        public string Insptype { get { return this._Insptype; } set { this._Insptype = value; } }

        public SQMInspMap() { }

        public SQMInspMap(string SID,
                          string SSID,
                          string InspDesc,
                          string InspNum,
                          string Standard,
                          string InspResult,
                          string CR,
                          string MA,
                          string MI,
                          string Other,
                          string isOther,
                          string UCL,
                          string LCL,
                          string InspTool,
                          string AQL,
                          string Insptype
            )
        {
            this._SID = SID;
            this._SSID = SSID;
            this._InspDesc = InspDesc;
            this._InspNum = InspNum;
            this._Standard = Standard;
            this._InspResult = InspResult;
            this._CR = CR;
            this._MA = MA;
            this._MI = MI;
            this._Other = Other;
            this._isOther = isOther;
            this._UCL = UCL;
            this._LCL = LCL;
            this._InspTool = InspTool;
            this._AQL = AQL;
            this._Insptype = Insptype;
        }
    }

    public class SQMInspMap_jQGridJSon
    {
        public List<SQMInspMap> Rows = new List<SQMInspMap>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }
    #endregion

    #region InspMap Helper
    public static class SQMInspMap_Helper
    {
        #region Search Insp
        public static string GetDataToJQGridJson(SqlConnection cn, SQMInspMap DataItem)
        {
            return GetDataToJQGridJson(cn, "", DataItem);
        }

        public static String GetDataToJQGridJson(SqlConnection cn, String SearchText, SQMInspMap DataItem)
        {
            if (DataItem.SID==null)
            {
                DataItem.SID = "";
            }
            string sSearchText = SearchText.Trim();
            string sWhereClause = "";
            if (sSearchText != "")
                sWhereClause += "   SIS.Name like '%'+ @SearchText+'%'   ";
            if (sWhereClause.Length != 0)
                sWhereClause = "  AND " + sWhereClause.Substring(0);
            StringBuilder sb = new StringBuilder();
            switch (DataItem.Insptype)
            {
                case ("Attributes"):
                    sb.Append(@"

SELECT SIM.[SID]
	,SI.Name AS InspCode
    ,SIM.[SSID]
	,SIS.Name AS InspDesc
    ,[InspNum]
    ,[Standard]
    ,[CR]
    ,[MA]
    ,[MI]
    ,[Other]
    ,[isOther]
    ,[Insptype]
FROM [TB_SQM_InspCode_Map] SIM
LEFT JOIN [dbo].[TB_SQM_InspCode] AS SI ON SIM.SID=SI.SID
LEFT JOIN [dbo].[TB_SQM_InspCode_Sub] AS SIS ON SIM.SSID = SIS.SID
WHERE SIM.SID =@SID AND Insptype='Attributes'

            ");
                    break;
                case ("Variables"):
                    sb.Append(@"

SELECT SIM.[SID]
	,SI.Name AS InspCode
    ,SIM.[SSID]
	,SIS.Name AS InspDesc
    ,[InspNum]
    ,[UCL]
    ,[LCL]
    ,[InspTool]
    ,[AQL]
    ,[Insptype]
FROM [TB_SQM_InspCode_Map] SIM
LEFT JOIN [dbo].[TB_SQM_InspCode] AS SI ON SIM.SID=SI.SID
LEFT JOIN [dbo].[TB_SQM_InspCode_Sub] AS SIS ON SIM.SSID = SIS.SID
WHERE SIM.SID =@SID AND Insptype='Variables'

            ");
                    break;
            }
            
            DataTable dt = new DataTable();
            sb.Append(sWhereClause + ";");
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                if (sSearchText != "")
                    cmd.Parameters.Add(new SqlParameter("@SearchText", sSearchText));
                cmd.Parameters.Add(new SqlParameter("@SID",SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SID)));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }
        #endregion

        #region Create/Edit data check
        private static void UnescapeDataFromWeb(SQMInspMap DataItem)
        {

            DataItem.InspDesc = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.InspDesc);
            DataItem.InspNum = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.InspNum);
            DataItem.Standard = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Standard);
            DataItem.InspResult = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.InspResult);
            DataItem.CR = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CR);
            DataItem.MA = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.MA);
            DataItem.MI = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.MI);
            DataItem.Other = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Other);
            DataItem.isOther = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.isOther);
            DataItem.UCL = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.UCL);
            DataItem.LCL = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.LCL);
            DataItem.InspTool = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.InspTool);
            DataItem.AQL = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.AQL);
            DataItem.Insptype = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Insptype);

        }

        private static string DataCheck(SQMInspMap DataItem)
        {
            string r = "";
            List<string> e = new List<string>();
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.InspDesc))
                e.Add("Must provide InspDesc.");
            //if (SQMStringHelper.DataIsNullOrEmpty(DataItem.InspNum))
            //    e.Add("Must provide InspNum.");
            //if (SQMStringHelper.DataIsNullOrEmpty(DataItem.Insptype))
            //    e.Add("Must provide Insptype.");
            //if (SQMStringHelper.DataIsNullOrEmpty(DataItem.RevenuePer))
            //    e.Add("Must provide RevenuePer.");
            //if (SQMStringHelper.DataIsNullOrEmpty(DataItem.MOQ))
            //    e.Add("Must provide MOQ.");
            //if (SQMStringHelper.DataIsNullOrEmpty(DataItem.SampleTime))
            //    e.Add("Must provide SampleTime.");
            //if (SQMStringHelper.DataIsNullOrEmpty(DataItem.LeadTime))
            //    e.Add("Must provide LeadTime.");
            //if (SQMStringHelper.DataIsNullOrEmpty(DataItem.AnnualCapacity))
            //    e.Add("Must provide AnnualCapacity.");

            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }
        #endregion

        #region Create data item
        public static string CreateDataItem(SqlConnection cnPortal, SQMInspMap DataItem)
        {
            if (DataItem.isOther == "1")
            {
                DataItem.CR = "";
                DataItem.MA = "";
                DataItem.MI = "";
            }
            else
            {
                DataItem.Other = "";
            }
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            if (r != "")
            {
                return r;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                switch (DataItem.Insptype)
                {
                    case ("Attributes"):
                        sb.Append(@" 
INSERT INTO [dbo].[TB_SQM_InspCode_Sub](Name)
VALUES(@Name);
DECLARE @SSID bigint =(SELECT @@Identity)
INSERT INTO [dbo].[TB_SQM_InspCode_Map](
    [SID]
    ,[SSID]
    ,[InspNum]
    ,[Standard]
    ,[CR]
    ,[MA]
    ,[MI]
    ,[Other]
    ,[isOther]
)
VALUES (
    @SID
    ,@SSID
    ,@InspNum
    ,@Standard
    ,@CR
    ,@MA
    ,@MI
    ,@Other
    ,@isOther
);
");
                        break;
                    case ("Variables"):
                        sb.Append(@" 
INSERT INTO [dbo].[TB_SQM_InspCode_Sub](Name)
VALUES(@Name);
DECLARE @SSID bigint =(SELECT @@Identity)
INSERT INTO [dbo].[TB_SQM_InspCode_Map](
    [SID]
,[SSID]
    ,[InspNum]

    ,UCL
    ,LCL
    ,InspTool
    ,AQL
)
VALUES (
    @SID
  ,@SSID
    ,@InspNum
    ,@UCL
    ,@LCL
    ,@InspTool
    ,@AQL
);
");
                        break;
                }
                
                //SQM_Basic_Helper.InsertPart(sb, "3");
                SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal);
                cmd.Parameters.AddWithValue("@SID", DataItem.SID);
                cmd.Parameters.AddWithValue("@Name", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.InspDesc));
                cmd.Parameters.AddWithValue("@InspNum", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.InspNum));
                cmd.Parameters.AddWithValue("@Standard", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Standard));
                cmd.Parameters.AddWithValue("@CR", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.MI));
                cmd.Parameters.AddWithValue("@MI", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.MI));
                cmd.Parameters.AddWithValue("@MA", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.MA));
                cmd.Parameters.AddWithValue("@Other", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Other));
                cmd.Parameters.AddWithValue("@isOther", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.isOther));
                cmd.Parameters.AddWithValue("@UCL", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.UCL));
                cmd.Parameters.AddWithValue("@LCL", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.LCL));
                cmd.Parameters.AddWithValue("@InspTool", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.InspTool));
                cmd.Parameters.AddWithValue("@AQL", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.AQL));

                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;

                return sErrMsg;
            }
        }


        #endregion

        #region Edit data item
        public static string EditDataItem(SqlConnection cnPortal, SQMInspMap DataItem)
        {
            return EditDataItem(cnPortal, DataItem, "", "");
        }

        public static string EditDataItem(SqlConnection cnPortal, SQMInspMap DataItem, string LoginMemberGUID, string RunAsMemberGUID)
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

        private static string EditDataItemSub(SqlCommand cmd, SQMInspMap DataItem)
        {
            if (DataItem.isOther == "1")
            {
                DataItem.CR = "";
                DataItem.MA = "";
                DataItem.MI = "";
            }
            else
            {
                DataItem.Other = "";
            }
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            switch (DataItem.Insptype)
            {
                case ("Attributes"):
                    sb.Append(@"
  UPDATE dbo.TB_SQM_InspCode_Map
  SET InspNum=@InspNum
      ,Standard=@Standard
      ,CR=@CR
      ,MA=@MA
      ,MI=@MI
      ,Other=@Other
      ,isOther=@isOther
	  WHERE SID=@SID AND SSID=@SSID;
UPDATE [dbo].[TB_SQM_InspCode_Sub]
SET [Name]=@InspDesc
WHERE SID=@SSID;

");
                    break;
                case ("Variables"):
                    sb.Append(@"
  UPDATE dbo.TB_SQM_InspCode_Map
  SET InspNum=@InspNum
      ,UCL=@UCL
      ,LCL=@LCL
      ,MA=@MA
      ,InspTool=@InspTool
      ,AQL=@AQL
	  WHERE SID=@SID AND SSID=@SSID;
UPDATE [dbo].[TB_SQM_InspCode_Sub]
SET [Name]=@InspDesc
WHERE SID=@SSID
");
                    break;
                default:
                    break;
            }
 
            
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@SID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SID));
            cmd.Parameters.AddWithValue("@SSID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SSID));
            cmd.Parameters.AddWithValue("@Name", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.InspDesc));
            cmd.Parameters.AddWithValue("@InspNum", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.InspNum));
            cmd.Parameters.AddWithValue("@InspDesc", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.InspDesc));
            cmd.Parameters.AddWithValue("@Standard", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Standard));
            cmd.Parameters.AddWithValue("@CR", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.CR));
            cmd.Parameters.AddWithValue("@MI", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.MI));
            cmd.Parameters.AddWithValue("@MA", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.MA));
            cmd.Parameters.AddWithValue("@Other", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Other));
            cmd.Parameters.AddWithValue("@isOther", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.isOther));
            cmd.Parameters.AddWithValue("@UCL", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.UCL));
            cmd.Parameters.AddWithValue("@LCL", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.LCL));
            cmd.Parameters.AddWithValue("@InspTool", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.InspTool));
            cmd.Parameters.AddWithValue("@AQL", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.AQL));

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion

        #region Delete data item
        public static string DeleteDataItem(SqlConnection cnPortal, SQMInspMap DataItem)
        {
            return DeleteDataItem(cnPortal, DataItem, "", "");
        }

        public static string DeleteDataItem(SqlConnection cnPortal, SQMInspMap DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {

            string r = "";
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.SID))
                return "Must provide SID.";
            //if (SQMStringHelper.DataIsNullOrEmpty(DataItem.SSID))
            //    return "Must provide SSID.";
            else
            {
                using (SqlCommand cmd = new SqlCommand("", cnPortal)) { r = DeleteDataItemSub(cmd, DataItem); }
                if (r != "") { return r; }

                //SqlTransaction tran = cnPortal.BeginTransaction();

                //using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = DeleteDataItemSub(cmd, DataItem); }
                //if (r != "") { tran.Rollback(); return r; }

                //if ((LoginMemberGUID != "") && (RunAsMemberGUID != ""))
                //    using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = DataLockHelper.ReleaseLock(cmd, DataItem.BasicInfoGUID, LoginMemberGUID, RunAsMemberGUID); }
                //if (r != "") { tran.Rollback(); return r; }

                //try { tran.Commit(); }
                //catch (Exception e) { tran.Rollback(); r = "Delete fail.<br />Exception: " + e.ToString(); }
                return r;
            }
        }

        private static string DeleteDataItemSub(SqlCommand cmd, SQMInspMap DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            switch (DataItem.Insptype)
            {
                case ("Attributes"):
                    sb.Append(@"
DELETE dbo.TB_SQM_InspCode_Map
WHERE SID=@SID AND SSID=@SSID;
DELETE [dbo].[TB_SQM_InspCode_Sub]
WHERE SID=@SSID;
");
                    cmd.CommandText = sb.ToString();
                    cmd.Parameters.AddWithValue("@SID", DataItem.SID);
                    cmd.Parameters.AddWithValue("@SSID", DataItem.SSID);
                    break;
                case ("Variables"):
                    sb.Append(@"
DELETE dbo.TB_SQM_InspCode_Map
WHERE SID=@SID
");
                    cmd.CommandText = sb.ToString();
                    cmd.Parameters.AddWithValue("@SID", DataItem.SID);
                    break;
                default:
                    break;
            }

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion
    }
    #endregion 
    #endregion
}
