using Lib_SQM_Domain.SharedLibs;
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
    public class VenderAccount
    {
        public string _AccountID;
        public string _VendorCode;
        public string _NameInChinese;
        public string _NameInEnglish;
        public string _PrimaryEmail;

        public string AccountID { get { return this._AccountID; } set { this._AccountID = value; } }
        public string VendorCode { get { return this._VendorCode; } set { this._VendorCode = value; } }
        public string NameInChinese { get { return this._NameInChinese; } set { this._NameInChinese = value; } }
        public string NameInEnglish { get { return this._NameInEnglish; } set { this._NameInEnglish = value; } }
        public string PrimaryEmail { get { return this._PrimaryEmail; } set { this._PrimaryEmail = value; } }
        public VenderAccount()
        {
        }
        public VenderAccount(string AccountID, string VendorCode, string NameInChinese, string NameInEnglish, string PrimaryEmail)
        {
            this._VendorCode = VendorCode;
            this._AccountID = AccountID;
            this._NameInChinese = NameInChinese;
            this._NameInEnglish = NameInEnglish;
            this._PrimaryEmail = PrimaryEmail;
        }
    }


    public class VenderAccount_jQGridJSon
    {
        public List<VenderAccount> Rows = new List<VenderAccount>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }

    public static class VenderAccount_Helper
    {
        public static string GetDataToJQGridJson(SqlConnection cn, string SearchText1,string SearchText2, string MemberGUID)
        {
            VenderAccount_jQGridJSon m = new VenderAccount_jQGridJSon();
            m.Rows.Clear();
            int iRowCount = 0;
            string sSearchText1 = SearchText1.Trim();
            string sSearchText2 = SearchText2.Trim();
            string sWhereClause = "";
            if (sSearchText1 != "")
                sWhereClause += " and VendorCode like '%' + @SearchText1 + '%'";
            if (sSearchText2 != "")
                sWhereClause += " and AccountID like '%' + @SearchText2 + '%'";
            string Plant = GetPlant(cn, MemberGUID);
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
                 SELECT t1.[MemberGUID]
      ,[PlantCode]
      ,[VendorCode]
	  ,t2.accountid,t2.nameinchinese,t2.nameinenglish,t2.primaryemail
  FROM [TB_SQM_Member_Vendor_Map] t1
  inner join PORTAL_Members t2 on t1.MemberGUID=t2.MemberGUID
  where t2.MemberType='1' and PlantCode=@PlantCode
           ");
            string ssSQL = sb.ToString() + sWhereClause + ";";
            using (SqlCommand cmd = new SqlCommand(ssSQL.ToString(), cn))
            {
                if (sSearchText1 != "")
                    cmd.Parameters.Add(new SqlParameter("@SearchText1", sSearchText1));
                if (sSearchText2 != "")
                    cmd.Parameters.Add(new SqlParameter("@SearchText2", sSearchText2));
                cmd.Parameters.AddWithValue("@PlantCode", SQMStringHelper.NullOrEmptyStringIsDBNull(Plant));
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    iRowCount++;
                    m.Rows.Add(new VenderAccount(
                    dr["AccountID"].ToString(),
                    dr["VendorCode"].ToString(),
                    dr["NameInChinese"].ToString(),
                    dr["NameInEnglish"].ToString(),
                    dr["PrimaryEmail"].ToString()
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
            return GetDataToJQGridJson(cn, "", "","");
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
