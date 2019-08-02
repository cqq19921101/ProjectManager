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
using System.Web;

namespace Lib_SQE_Domain.Model
{
    public class SQE_UD
    {
        protected string _SID { get; set; }
        protected string _UD { get; set; }
        protected string _Plan { get; set; }
        protected string _UDType { get; set; }
        protected string _ReMark { get; set; }
        protected string _UpdateUser { get; set; }
        protected string _updateTime { get; set; }

        public string SID { get { return this._SID; } set { this._SID = value; } }
        public string UD { get { return this._UD; } set { this._UD = value; } }
        public string Plan { get { return this._Plan; } set { this._Plan = value; } }
        public string UDType { get { return this._UDType; } set { this._UDType = value; } }
        public string ReMark { get { return this._ReMark; } set { this._ReMark = value; } }
        public string UpdateUser { get { return this._UpdateUser; } set { this._UpdateUser = value; } }
        public string updateTime { get { return this._updateTime; } set { this._updateTime = value; } }
        public SQE_UD()
        {

        }
        public SQE_UD(
            string SID,
            string UD,
            string Plan,
            string UDType,
            string ReMark,
            string UpdateUser,
            string updateTime
                )
        {
            this._SID = SID;
            this._UD = UD;
            this._Plan = Plan;
            this._UDType = UDType;
            this._ReMark = ReMark;
            this._UpdateUser = UpdateUser;
            this._updateTime = updateTime;

        }
    }
    public class SQE_UD_jQGridJSon
    {
        public List<SQE_UD> Rows = new List<SQE_UD>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }
    public static class SQE_UD_Helper
    {

        private static void UnescapeDataFromWeb(SQE_UD DataItem)
        {
            DataItem.UD = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.UD);
            DataItem.Plan = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Plan);
            DataItem.UDType = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.UDType);
            DataItem.ReMark = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ReMark);
            DataItem.UpdateUser = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.UpdateUser);
            DataItem.updateTime = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.updateTime);
        }
        private static string DataCheck(SQE_UD DataItem)
        {
            string r = "";
            List<string> e = new List<string>();
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.UD))
                e.Add("Must provide UD.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.Plan))
                e.Add("Must provide Plan.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.UDType))
                e.Add("Must provide UDType.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.ReMark))
                e.Add("Must provide ReMark.");
            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }
        public static String GetDataToJQGridJson(SqlConnection cn, String SearchText, String MemberGUID)
        {
            string sSearchText = SearchText.Trim();
            string sWhereClause = "";
            if (sSearchText != "")
                sWhereClause += "   UD like '%'+ @SearchText+'%'   ";
            if (sWhereClause.Length != 0)
                sWhereClause = "  AND " + sWhereClause.Substring(0);

            StringBuilder sb = new StringBuilder();
            sb.Append(@"
SELECT [SID]
      ,[UD]
      ,[Plan]
      ,[UDType]
      ,[ReMark]
      ,[UpdateUser]
      ,[updateTime]
  FROM [TB_SQM_UD]
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

        public static string CreateDataItem(SqlConnection sqlConnection, SQE_UD DataItem, string memberGUID)
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
DECLARE @UpdateUser nvarchar(50) = (
    SELECT
        [NameInChinese]
    FROM
        [PORTAL_Members]
    WHERE
        [MemberGUID] = @MemberGUID
)
INSERT INTO [dbo].[TB_SQM_UD] (
        [UD],
        [Plan],
        [UDType],
        [ReMark],
        [UpdateUser],
        [updateTime]
    )
VALUES
    (
        @UD,
        @Plan,
        @UDType,
        @ReMark,
        @UpdateUser,
        GetDate()
    )
");
                //SQE_Basic_Helper.InsertPart(sb, "3");
                SqlCommand cmd = new SqlCommand(sb.ToString(), sqlConnection);
                cmd.Parameters.AddWithValue("@MemberGUID", memberGUID);
                cmd.Parameters.AddWithValue("@UD", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.UD));
                cmd.Parameters.AddWithValue("@Plan", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Plan));
                cmd.Parameters.AddWithValue("@UDType", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.UDType));
                cmd.Parameters.AddWithValue("@ReMark", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ReMark));

                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;

                return sErrMsg;
            }
        }


        public static string EditDataItem(SqlConnection cnPortal, SQE_UD DataItem, string MemberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            if (r != "")
            { return r; }
            else
            {
                using (SqlCommand cmd = new SqlCommand("", cnPortal)) { r = EditDataItemSub(cmd, DataItem, MemberGUID); }
                if (r != "") { return r; }
                return r;
            }
        }
        private static string EditDataItemSub(SqlCommand cmd, SQE_UD DataItem, string MemberGUID)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
DECLARE @UpdateUser nvarchar(50) = (
    SELECT
        [NameInChinese]
    FROM
        [PORTAL_Members]
    WHERE
        [MemberGUID] = @MemberGUID
)
UPDATE [dbo].[TB_SQM_UD]
   SET [UD] = @UD
      ,[Plan] = @Plan
      ,[UDType] = @UDType
      ,[ReMark] = @ReMark
      ,[UpdateUser] = @UpdateUser
      ,[updateTime] = GetDate()
 WHERE [SID]=@SID
");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@SID", DataItem.SID);
            cmd.Parameters.AddWithValue("@MemberGUID", MemberGUID);
            cmd.Parameters.AddWithValue("@UD", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.UD));
            cmd.Parameters.AddWithValue("@Plan", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Plan));
            cmd.Parameters.AddWithValue("@UDType", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.UDType));
            cmd.Parameters.AddWithValue("@ReMark", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ReMark));

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        public static string DeleteDataItem(SqlConnection cnPortal, SQE_UD DataItem, string memberGUID1, string memberGUID2)
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
        private static string DeleteDataItemSub(SqlCommand cmd, SQE_UD DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
DELETE [dbo].[TB_SQM_UD] WHERE SID=@SID;
");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@SID", DataItem.SID);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }

    }
}
