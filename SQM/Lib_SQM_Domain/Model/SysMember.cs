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
    public class SysMember
    {
        protected string _MemberGUID { get; set; }
        protected string _RoleGUID { get; set; }
        protected string _RoleName { get; set; }
        protected bool _Belongs { get; set; }

        public string MemberGUID { get { return this._MemberGUID; } set { this._MemberGUID = value; } }
        public string RoleGUID { get { return this._RoleGUID; } set { this._RoleGUID = value; } }
        public string RoleName { get { return this._RoleName; } set { this._RoleName = value; } }
        public bool Belongs { get { return this._Belongs; } set { this._Belongs = value; } }
        public SysMember()
        {

        }
        public SysMember(
            string MemberGUID,
            string RoleGUID,
            string RoleName,
            bool Belongs
                )
        {
            this._MemberGUID = MemberGUID;
            this._RoleGUID = RoleGUID;
            this._RoleName = RoleName;
            this._Belongs = Belongs;
        }
    }
    public class SysMember_jQGridJSon
    {
        public List<SysMember> Rows = new List<SysMember>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }
    public static class SysMember_Helper
    {

        private static void UnescapeDataFromWeb(SysMember DataItem)
        {
        }
        private static string DataCheck(SysMember DataItem)
        {
            string r = "";
            List<string> e = new List<string>();
            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }
        public static String GetMemberRoleByMemberListJSon(SqlConnection cn, String MemberGUID, String SubMemberGUID)
        {
            SysMember_jQGridJSon m = new SysMember_jQGridJSon();

            m.Rows.Clear();
            int iRowCount = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
SELECT [RoleGUID]
      ,[RoleName]
  FROM [PORTAL_Roles]
  WHERE [RoleName] like 'SQM%'
");
            if (SubMemberGUID == null)
            {

                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
                {
                    //cmd.Parameters.Add(new SqlParameter("@MemberGUID", MemberGUID));
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        dt.Load(dr);
                    }
                }
                return JsonConvert.SerializeObject(dt);

            }
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                //cmd.Parameters.Add(new SqlParameter("@MemberGUID", MemberGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {

                        iRowCount++;
                        bool Belongs = false;
                        m.Rows.Add(new SysMember(
                        MemberGUID,
                        dr["RoleGUID"].ToString(),
                        dr["RoleName"].ToString(),
                        Belongs
                      ));
                    }
                }
            }
            for (int i = 0; i < m.Rows.Count; i++)
            {
                m.Rows[i].Belongs = GetBelongs(cn, m.Rows[i].RoleGUID.ToString(), SubMemberGUID);
            }
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(m);
        }

        private static bool GetBelongs(SqlConnection cn, string RoleGUID, string SubMemberGUID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
SELECT TOP 1 [RoleGUID]
FROM [PORTAL_MemberRoles]
WHERE RoleGUID=@RoleGUID AND MemberGUID=@MemberGUID
    ");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@RoleGUID", RoleGUID));
                cmd.Parameters.Add(new SqlParameter("@MemberGUID", SubMemberGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["RoleGUID"].ToString() != "")
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

    }
}
