using Lib_SQM_Domain.SharedLibs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib_SQM_Domain.Model
{
    public class SQE_LitNO_Plant
    {
        protected string _SID { get; set; }
        protected string _LitNo { get; set; }
        protected string _Standard1 { get; set; }
        protected string _Standard2 { get; set; }
        protected string _Type { get; set; }
        protected string _TB_SQM_AQL_PLANT_SID { get; set; }
        protected string _PlantName { get; set; }

        public string SID { get { return this._SID; } set { this._SID = value; } }
        public string LitNo { get { return this._LitNo; } set { this._LitNo = value; } }
        public string Standard1 { get { return this._Standard1; } set { this._Standard1 = value; } }
        public string Standard2 { get { return this._Standard2; } set { this._Standard2 = value; } }
        public string Type { get { return this._Type; } set { this._Type = value; } }
        public string TB_SQM_AQL_PLANT_SID { get { return this._TB_SQM_AQL_PLANT_SID; } set { this._TB_SQM_AQL_PLANT_SID = value; } }
        public string PlantName { get { return this._PlantName; } set { this._PlantName = value; } }
        public SQE_LitNO_Plant()
        {

        }
        public SQE_LitNO_Plant(
            string SID,
            string LitNo,
            string Standard1,
            string Standard2,
            string Type,
            string TB_SQM_AQL_PLANT_SID,
            string PlantName
                )
        {
            this._SID = SID;
            this._LitNo = LitNo;
            this._Standard1 = Standard1;
            this._Standard2 = Standard2;
            this._Type = Type;
            this._TB_SQM_AQL_PLANT_SID = TB_SQM_AQL_PLANT_SID;
            this._PlantName = PlantName;
        }
    }
    public class SQE_LitNO_Plant_jQGridJSon
    {
        public List<SQE_LitNO_Plant> Rows = new List<SQE_LitNO_Plant>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }
    public static class SQE_LitNO_Plant_Helper
    {

        private static void UnescapeDataFromWeb(SQE_LitNO_Plant DataItem)
        {
            DataItem.LitNo = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.LitNo);
            DataItem.Standard1 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Standard1);
            DataItem.Standard2 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Standard2);
            DataItem.Type = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Type);
            DataItem.PlantName = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.PlantName);
        }
        private static string DataCheck(SQE_LitNO_Plant DataItem)
        {
            string r = "";
            List<string> e = new List<string>();
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.LitNo))
                e.Add("Must provide LitNo.");
     
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.TB_SQM_AQL_PLANT_SID))
                e.Add("Must provide TB_SQM_AQL_PLANT_SID.");
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
                sWhereClause += " AND  TB_SQM_LitNO_Plant.LitNo like '%'+ @SearchText+'%'   ";
      

            StringBuilder sb = new StringBuilder();
            sb.Append(@"
SELECT   TB_SQM_LitNO_Plant.SID,
         TB_SQM_LitNO_Plant.LitNo,
         TB_SQM_LitNO_Plant.Standard1,
         TB_SQM_LitNO_Plant.Standard2,
         TB_SQM_LitNO_Plant.Type,
         TB_SQM_LitNO_Plant.TB_SQM_AQL_PLANT_SID,
         TB_SQM_AQL_PLANT.PlantName
FROM TB_SQM_LitNO_Plant
LEFT OUTER JOIN TB_SQM_AQL_PLANT
    ON TB_SQM_LitNO_Plant.TB_SQM_AQL_PLANT_SID = TB_SQM_AQL_PLANT.SID
WHERE 1=1
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

        public static string CreateDataItem(SqlConnection sqlConnection, SQE_LitNO_Plant DataItem, string memberGUID)
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
INSERT INTO [dbo].[TB_SQM_LitNO_Plant]
           ([LitNo]
           ,[Standard1]
           ,[Standard2]
           ,[Type]
           ,[TB_SQM_AQL_PLANT_SID])
     VALUES
           (@LitNo
           ,@Standard1
           ,@Standard2
           ,@Type
           ,@TB_SQM_AQL_PLANT_SID)
");
                //SQE_Basic_Helper.InsertPart(sb, "3");
                SqlCommand cmd = new SqlCommand(sb.ToString(), sqlConnection);
                cmd.Parameters.AddWithValue("@LitNo", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.LitNo));
                cmd.Parameters.AddWithValue("@Standard1", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Standard1));
                cmd.Parameters.AddWithValue("@Standard2", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Standard2));
                cmd.Parameters.AddWithValue("@Type", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Type));
                cmd.Parameters.AddWithValue("@TB_SQM_AQL_PLANT_SID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TB_SQM_AQL_PLANT_SID));

                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;

                return sErrMsg;
            }
        }


        public static string EditDataItem(SqlConnection cnPortal, SQE_LitNO_Plant DataItem, string MemberGUID)
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
        private static string EditDataItemSub(SqlCommand cmd, SQE_LitNO_Plant DataItem, string MemberGUID)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
UPDATE [dbo].[TB_SQM_LitNO_Plant]
   SET [LitNo] = @LitNo
      ,[Standard1] = @Standard1
      ,[Standard2] = @Standard2
      ,[Type] = @Type
      ,[TB_SQM_AQL_PLANT_SID] = @TB_SQM_AQL_PLANT_SID
 WHERE [SID]=@SID
");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@SID", DataItem.SID);
            cmd.Parameters.AddWithValue("@LitNo", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.LitNo));
            cmd.Parameters.AddWithValue("@Standard1", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Standard1));
            cmd.Parameters.AddWithValue("@Standard2", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Standard2));
            cmd.Parameters.AddWithValue("@Type", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Type));
            cmd.Parameters.AddWithValue("@TB_SQM_AQL_PLANT_SID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.TB_SQM_AQL_PLANT_SID));

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        public static string DeleteDataItem(SqlConnection cnPortal, SQE_LitNO_Plant DataItem, string memberGUID1, string memberGUID2)
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
        private static string DeleteDataItemSub(SqlCommand cmd, SQE_LitNO_Plant DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
DELETE [dbo].[TB_SQM_LitNO_Plant] WHERE SID=@SID;
");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@SID", DataItem.SID);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        public static string GetLitNoUnit(SqlConnection cn, String LitNo,string MemberGUID)
        {
            if (String.IsNullOrEmpty(LitNo))
            {
                return JsonConvert.SerializeObject(new DataTable());
            }
            string Plant = GetPlant(cn, MemberGUID);
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
              
  
SELECT top 1 [Unit1],[Unit2] FROM [TB_SQM_SFC_MAP]
WHERE LitNo=@LitNo and VoderCode in( SELECT VendorCode
                         FROM TB_SQM_Member_Vendor_Map
                         WHERE PlantCode = @PlantCode)
");
           

            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@LitNo", LitNo));
                cmd.Parameters.Add(new SqlParameter("@PlantCode", Plant));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }

        public static string GetLitNo(SqlConnection cn, string MemberGUID)
        {
            StringBuilder sb = new StringBuilder();
            string Plant = GetPlant(cn, MemberGUID);
            sb.Append(@"SELECT DISTINCT [LitNo] 
FROM[TB_SQM_SFC_MAP]
WHERE[VoderCode] in (
                     SELECT VendorCode
                         FROM TB_SQM_Member_Vendor_Map
                         WHERE PlantCode = @PlantCode); 
");

            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@PlantCode", Plant));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
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
    }
}
