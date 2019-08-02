using Lib_SQM_Domain.SharedLibs;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib_SQM_Domain.Model
{
    public class SQM_SystemMgmt_Member_Helper
    {
        public static string UpdateMemberRoles(SqlConnection cn, string memberGUID, List<string> memberRoles)
        {
            string sErrMsg = "";
            string SmemberRoles = string.Empty;
            StringBuilder sSQL = new StringBuilder();
            foreach (string sTmp in memberRoles)
            {
                if (!sTmp.Equals(string.Empty))
                {
                    if (SmemberRoles.Equals(string.Empty)) SmemberRoles += "'" + sTmp + "'";
                    else SmemberRoles += ",'" + sTmp + "'";
                }
            }
            //删除
            sSQL.Append("DELETE FROM PORTAL_MemberRoles WHERE MemberGUID = @MemberGUID ");
            sSQL.Append(@"AND RoleGUID IN (select RoleGUID from [dbo].[PORTAL_Roles]
where RoleName like 'SQM%');");
            //添加
            sSQL.Append(@"INSERT INTO [dbo].[PORTAL_MemberRoles]
           ([MemberGUID]
           ,[RoleGUID])
SELECT   MemberGUID ,[RoleGUID] from [dbo].[PORTAL_Roles],[PORTAL_Members]
  where MemberGUID=@MemberGUID
  and [RoleGUID] in (" + SmemberRoles+ ")");


            SqlCommand cmd = new SqlCommand(sSQL.ToString(), cn);
            cmd.Parameters.AddWithValue("@MemberGUID", SQMStringHelper.NullOrEmptyStringIsDBNull(memberGUID));
         

      


            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
    }
}
