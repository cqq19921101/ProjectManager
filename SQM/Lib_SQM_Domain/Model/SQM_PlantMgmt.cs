using Lib_Portal_Domain.SharedLibs;
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
    

    public class SQM_PlantMgmt
    {
        protected string _MemberGUID;
        protected string _PlantCode;
        protected string _PlantName;
        protected string _Name;
        protected string _Email;
        public string MemberGUID { get { return this._MemberGUID; } set { this._MemberGUID = value; } }
        public string PlantCode { get { return this._PlantCode; } set { this._PlantCode = value; } }
        public string PlantName { get { return this._PlantName; } set { this._PlantName = value; } }
        public string Name { get { return this._Name; } set { this._Name = value; } }
        public string Email { get { return this._Email; } set { this._Email = value; } }

        public SQM_PlantMgmt() { }

        public SQM_PlantMgmt(string MemberGUID,string PlantCode, string PlantName,string Name,string Email)
        {
            this._MemberGUID = MemberGUID;
            this._PlantCode = PlantCode;
            this._PlantName = PlantName;
            this._Name = Name;
            this._Email = Email;
        }
    }
    public class SQM_PlantMgmt_jQGridJSon
    {
        public List<SQM_PlantMgmt> Rows = new List<SQM_PlantMgmt>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }
    public static class SQM_PlantMgmt_Helper
    {
        #region Create/Edit data check
        private static void UnescapeDataFromWeb(SQM_PlantMgmt DataItem)
        {
            DataItem.PlantName = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.PlantName);
            DataItem.Name = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Name);
            DataItem.Email = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Email);
        }
        private static string DataCheck(SQM_PlantMgmt DataItem)
        {
            string r = "";
            List<string> e = new List<string>();

            if (StringHelper.DataIsNullOrEmpty(DataItem.PlantCode))
                e.Add("Must provide PlantCode.");

            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }
        #endregion

        #region Query Buyer Code
        public static string Query_BuyerCodeInfo(SqlConnection cn, string Buyer)
        {
            bool bIsWithCondition = false;

            #region DecodeParameter
            string sBuyer = StringHelper.EmptyOrUnescapedStringViaUrlDecode(Buyer);
            #endregion
            
            //retRows.Rows.Clear();

            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.Append(@"
               SELECT PORTAL_Members.MemberGUID,
                   NameInChinese
            FROM PORTAL_Members,
                 ( 
                SELECT DISTINCT MEMBERGUID FROM PORTAL_MEMBERROLES
  inner join PORTAL_Roles on PORTAL_Roles.RoleGUID=PORTAL_MEMBERROLES.RoleGUID
    WHERE RoleName LIKE 'SQM%' ) TB_EB_EMPL_FUNC
            WHERE PORTAL_Members.MemberGUID = convert(nvarchar(255), TB_EB_EMPL_FUNC.MEMBERGUID)
           
            ");

            if (!string.IsNullOrEmpty(sBuyer))
            {
                sb.Append(@" and NameInChinese  LIKE '%' + @NameInChinese + '%' ");
            }
                DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                if (!string.IsNullOrEmpty(sBuyer))
                {
                    cmd.Parameters.AddWithValue("@NameInChinese", sBuyer);
                }
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }
        #endregion

        public static string DeleteDataItem(SqlConnection cnPortal, SQM_PlantMgmt DataItem)
        {
            return DeleteDataItem(cnPortal, DataItem, "", "");
        }
        public static string DeleteDataItem(SqlConnection cnPortal, SQM_PlantMgmt DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            string r = "";
            if (StringHelper.DataIsNullOrEmpty(DataItem.PlantCode))
                return "Must provide PlantCode.";
            if (StringHelper.DataIsNullOrEmpty(DataItem.MemberGUID))
                return "Must provide MemberGUID.";
            else
            {
                using (SqlCommand cmd = new SqlCommand("", cnPortal)) { r = DeleteDataItemSub(cmd, DataItem); }
                if (r != "") { return r; }
                return r;
            }
        }
        private static string DeleteDataItemSub(SqlCommand cmd, SQM_PlantMgmt DataItem)
        {
            string sErrMsg = "";

            string sSQL = "Delete TB_SQM_Member_Plant Where MemberGUID = @MemberGUID and PlantCode=@PlantCode;";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@MemberGUID", StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.MemberGUID));
            cmd.Parameters.AddWithValue("@PlantCode", StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.PlantCode));
            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }

        public static string GetDataToJQGridJson(SqlConnection cn, string SearchText)
        {
            SQM_PlantMgmt_jQGridJSon m = new SQM_PlantMgmt_jQGridJSon();
            m.Rows.Clear();
            int iRowCount = 0;
            StringBuilder sb = new StringBuilder();
            string sSearchText = SearchText.Trim();
            string sWhereClause = "";
            if (sSearchText != "")
                sWhereClause += " and [PORTAL_Members].NameInChinese like '%' + @SearchText + '%'";

            sb.Append(@"
            SELECT TB_SQM_Member_Plant.MemberGUID,
                   TB_SQM_Member_Plant.PlantCode,
                   TB_VMI_PLANT.plant_name,
                   [PORTAL_Members].NameInChinese as [NAME],
                   [PORTAL_Members].PrimaryEmail as [EMAIL]
            FROM PORTAL_Members,
                 ( 
                         SELECT DISTINCT MEMBERGUID  FROM PORTAL_MEMBERROLES
  inner join PORTAL_Roles on PORTAL_Roles.RoleGUID=PORTAL_MEMBERROLES.RoleGUID
    WHERE RoleName LIKE 'SQM%'  ) TB_EB_EMPL_FUNC,
                 TB_SQM_Member_Plant,
                 TB_VMI_PLANT
            WHERE [PORTAL_Members].MemberGUID = TB_EB_EMPL_FUNC.MEMBERGUID
              AND TB_SQM_Member_Plant.[MemberGUID] = CONVERT(nvarchar(255), [PORTAL_Members].MemberGUID)
              AND TB_SQM_Member_Plant.plantcode = TB_VMI_PLANT.plant
            ");

            string ssSQL = sb.ToString() + sWhereClause + ";";
            using (SqlCommand cmd = new SqlCommand(ssSQL, cn))
            {
                if (sSearchText != "")
                    cmd.Parameters.Add(new SqlParameter("@SearchText", sSearchText));
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    iRowCount++;
                    m.Rows.Add(new SQM_PlantMgmt(
                        dr["MemberGUID"].ToString(),
                        dr["PlantCode"].ToString(),
                         dr["PLANT_NAME"].ToString(),
                         dr["NAME"].ToString(),
                          dr["EMAIL"].ToString()
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
            return GetDataToJQGridJson(cn, "");
        }
        public static string CreateDataItem(SqlConnection cnPortal, SQM_PlantMgmt DataItem,String LoginUserGUID)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);

            if (r != "")
            { return r; }


            StringBuilder sb = new StringBuilder();
            sb.Append(@"
            INSERT INTO [dbo].[TB_SQM_Member_Plant]
                   ( [MemberGUID],
                     [PlantCode],
                     [UpdateDatetime],
                     [UpdateUser]
                   )
            VALUES( @MemberGUID, @PlantCode,getDate(),@LoginUserGUID);
            ");

            SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal);


            cmd.Parameters.AddWithValue("@MemberGUID", StringHelper.NullOrEmptyStringIsDBNull(DataItem.MemberGUID));
            cmd.Parameters.AddWithValue("@PlantCode", StringHelper.NullOrEmptyStringIsDBNull(DataItem.PlantCode));
            cmd.Parameters.AddWithValue("@LoginUserGUID", StringHelper.NullOrEmptyStringIsDBNull(LoginUserGUID));

            string sErrMsg = "";
            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
            cmd = null;

            return sErrMsg;

        }




    }
    
}
