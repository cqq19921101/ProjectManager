using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Web.Script.Serialization;
using Lib_SQM_Domain.SharedLibs;
using System.Web;

namespace Lib_SQM_Domain.Modal
{
    

    #region Data Class Definitions
    public class SystemMgmt_SubFunc
    {
        protected string _SubFuncGUID = "";
        protected string _SubFuncName = "";
        
        public string SubFuncGUID { get { return this._SubFuncGUID; } set { this._SubFuncGUID = value; } }
        public string SubFuncName { get { return this._SubFuncName; } set { this._SubFuncName = value; } }
        
        public SystemMgmt_SubFunc() { }
        public SystemMgmt_SubFunc(string SubFuncGUID, string SubFuncName)
        {
            this._SubFuncGUID = SubFuncGUID;
            this._SubFuncName = SubFuncName;
        }
    }

    public class SystemMgmt_SubFunc_jQGridJSon
    {
        public List<SystemMgmt_SubFunc> Rows = new List<SystemMgmt_SubFunc>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }
    #endregion

    public static class SystemMgmt_SubFunc_Helper
    {
        #region SearchSubFunc
        public static string GetDataToJQGridJson(SqlConnection cn)
        {
            return GetDataToJQGridJson(cn, "");
        }

        public static string GetDataToJQGridJson(SqlConnection cn, string SearchText)
        {
            SystemMgmt_SubFunc_jQGridJSon m = new SystemMgmt_SubFunc_jQGridJSon();

            string sSearchText = SearchText.Trim();
            string sWhereClause = "";
            if (sSearchText != "")
                sWhereClause += " and SubFuncName like '%' + @SearchText + '%'";
            if (sWhereClause.Length != 0)
                sWhereClause = " Where" + sWhereClause.Substring(4);

            m.Rows.Clear();
            int iRowCount = 0;
            string sSQL = "SELECT Top 100 SubFuncGUID, SubFuncName From TB_SQM_SUBFUNC WITH (NOLOCK)";
            sSQL += sWhereClause + ";";
            using (SqlCommand cmd = new SqlCommand(sSQL, cn))
            {
                if (sSearchText != "")
                    cmd.Parameters.Add(new SqlParameter("@SearchText", sSearchText));
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    iRowCount++;
                    m.Rows.Add(new SystemMgmt_SubFunc(
                        dr["SubFuncGUID"].ToString(),
                        HttpUtility.HtmlEncode(dr["SubFuncName"].ToString())));
                }
                dr.Close();
                dr = null;
            }

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(m);
        }
        #endregion

        #region Create/Edit data check
        private static void UnescapeDataFromWeb(SystemMgmt_SubFunc DataItem)
        {
            DataItem.SubFuncName = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.SubFuncName);
        }

        private static string DataCheck(SystemMgmt_SubFunc DataItem)
        {
            string r = "";
            List<string> e = new List<string>();
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.SubFuncName))
                e.Add("Must provide SubFunc Name.");

            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }
        #endregion

        #region Create data item
        public static string CreateDataItem(SqlConnection cnPortal, SystemMgmt_SubFunc DataItem)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            if (r != "")
            { return r; }
            else
            {
                string sSQL = "Insert Into TB_SQM_SUBFUNC (SubFuncGUID, SubFuncName) ";
                sSQL += "Values (@SubFuncGUID, @SubFuncName);";
                SqlCommand cmd = new SqlCommand(sSQL, cnPortal);
                cmd.Parameters.AddWithValue("@SubFuncGUID", Guid.NewGuid());
                cmd.Parameters.AddWithValue("@SubFuncName", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SubFuncName));
                
                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;

                return sErrMsg;
            }
        }
        #endregion

        #region Edit data item
        public static string EditDataItem(SqlConnection cnPortal, SystemMgmt_SubFunc DataItem)
        {
            return EditDataItem(cnPortal, DataItem, "", "");
        }

        public static string EditDataItem(SqlConnection cnPortal, SystemMgmt_SubFunc DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            if (r != "")
            { return r; }
            else
            {
                SqlTransaction tran = cnPortal.BeginTransaction();

                //Update member data
                using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = EditDataItemSub(cmd, DataItem); }
                if (r != "") { tran.Rollback(); return r; }

                //Check lock is still valid
                bool bLockIsStillValid = false;
                if ((LoginMemberGUID != "") && (RunAsMemberGUID != ""))
                    using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { bLockIsStillValid = SQMDataLockHelper.CheckLockIsStillValid(cmd, DataItem.SubFuncGUID, LoginMemberGUID, RunAsMemberGUID); }
                if (!bLockIsStillValid) { tran.Rollback(); return SQMDataLockHelper.LockString(); }

                //Release lock
                if ((LoginMemberGUID != "") && (RunAsMemberGUID != ""))
                    using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = SQMDataLockHelper.ReleaseLock(cmd, DataItem.SubFuncGUID, LoginMemberGUID, RunAsMemberGUID); }
                if (r != "") { tran.Rollback(); return r; }

                //Commit
                try { tran.Commit(); }
                catch (Exception e) { tran.Rollback(); r = "Edit fail.<br />Exception: " + e.ToString(); }
                return r;
            }
        }

        private static string EditDataItemSub(SqlCommand cmd, SystemMgmt_SubFunc DataItem)
        {
            string sErrMsg = "";

            string sSQL = "Update TB_SQM_SUBFUNC Set SubFuncName = @SubFuncName ";
            sSQL += "Where SubFuncGUID = @SubFuncGUID;";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@SubFuncName", DataItem.SubFuncName);
            cmd.Parameters.AddWithValue("@SubFuncGUID", DataItem.SubFuncGUID);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion

        #region Delete data item
        public static string DeleteDataItem(SqlConnection cnPortal, SystemMgmt_SubFunc DataItem)
        {
            return DeleteDataItem(cnPortal, DataItem, "", "");
        }

        public static string DeleteDataItem(SqlConnection cnPortal, SystemMgmt_SubFunc DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            string r = "";
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.SubFuncGUID))
                return "Must provide Account ID.";
            else
            {
                SqlTransaction tran = cnPortal.BeginTransaction();

                //Delete member data
                using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = DeleteDataItemSub(cmd, DataItem); }
                if (r != "") { tran.Rollback(); return r; }
                
                //Release lock
                if ((LoginMemberGUID != "") && (RunAsMemberGUID != ""))
                    using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = SQMDataLockHelper.ReleaseLock(cmd, DataItem.SubFuncGUID, LoginMemberGUID, RunAsMemberGUID); }
                if (r != "") { tran.Rollback(); return r; }

                //Commit
                try { tran.Commit(); }
                catch (Exception e) { tran.Rollback(); r = "Delete fail.<br />Exception: " + e.ToString(); }
                return r;
            }
        }

        private static string DeleteDataItemSub(SqlCommand cmd, SystemMgmt_SubFunc DataItem)
        {
            string sErrMsg = "";

            string sSQL = "Delete TB_SQM_SUBFUNC Where SubFuncGUID = @SubFuncGUID;";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@SubFuncGUID", DataItem.SubFuncGUID);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion

        #region Get SubFunc Admins
        public static string GetSubFuncAdminsToJQGridJSon(SqlConnection cn, string SubFuncGUID, string SearchText)
        {
            SystemMgmt_Member_jQGridJSon m = new SystemMgmt_Member_jQGridJSon();

            m.Rows.Clear();
            if (SubFuncGUID != "")
            {
                string sSQL = "SELECT Top 100 m.MemberGUID, m.AccountID, m.NameInChinese, m.NameInEnglish FROM PORTAL_SubFuncAdmins ra WITH (NOLOCK) ";
                sSQL += "Join PORTAL_Members m WITH (NOLOCK) On ra.MemberGUID = m.MemberGUID ";
                sSQL += "Where ra.SubFuncGUID = @SubFuncGUID";

                string sSearchText = SearchText.Trim();
                if (sSearchText != "")
                    sSQL += " And ((m.AccountID like '%' + @SearchText + '%') Or (m.NameInChinese Like '%' + @SearchText + '%') Or (m.NameInEnglish Like '%' + @SearchText + '%'))";
                sSQL += ";";
                using (SqlCommand cmd = new SqlCommand(sSQL, cn))
                {
                    cmd.Parameters.Add(new SqlParameter("@SubFuncGUID", SubFuncGUID));
                    if (sSearchText != "")
                        cmd.Parameters.Add(new SqlParameter("@SearchText", sSearchText));
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        m.Rows.Add(new SystemMgmt_Member(
                            dr["MemberGUID"].ToString(),
                            HttpUtility.HtmlEncode(dr["AccountID"].ToString()),
                            1,
                            HttpUtility.HtmlEncode(dr["NameInChinese"].ToString()),
                            HttpUtility.HtmlEncode(dr["NameInEnglish"].ToString()),
                            ""));
                    }
                    dr.Close();
                    dr = null;
                }
            }

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(m);
        }
        #endregion

        #region Get SubFunc Admin Count
        public static string GetSubFuncAdminsCount(SqlConnection cn, string SubFuncGUID)
        {
            int r = 0;

            if (SubFuncGUID != null)
                if (SubFuncGUID != "")
                {
                    string sSQL = "SELECT Count(*) FROM PORTAL_SubFuncAdmins ra WITH (NOLOCK) Where ra.SubFuncGUID = @SubFuncGUID;";
                    using (SqlCommand cmd = new SqlCommand(sSQL, cn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@SubFuncGUID", SubFuncGUID));
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                            r = (int)dr[0];
                        dr.Close();
                        dr = null;
                    }
                }

            return @"{""count"": " + r.ToString() + "}";
        }
        #endregion

        #region Add a SubFunc Admin
        public static string AddaSubFuncAdmin(SqlConnection cnPortal, string SubFuncGUID, string MemberGUID, string LoginMemberGUID, string RunAsMemberGUID)
        {
            string r = "";


            if (r != "")
            { return r; }
            else
            {
                SqlTransaction tran = cnPortal.BeginTransaction();
                string rl = SQMDataLockHelper.AcquireLock(cnPortal, tran, SubFuncGUID, LoginMemberGUID, RunAsMemberGUID);
                if (rl == "l") { r = "Data already lock by other user."; }
                else
                    if (rl == "e") { r = "Data lock fail or application error."; }
                    else
                    {
                        int c = 0;
                        string sSQL = "Select Count(*) From PORTAL_SubFuncAdmins WITH (NOLOCK) Where SubFuncGUID = @SubFuncGUID And MemberGUID = @MemberGUID;";
                        using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal, tran))
                        {
                            cmd.Parameters.Add(new SqlParameter("@SubFuncGUID", SubFuncGUID));
                            cmd.Parameters.Add(new SqlParameter("@MemberGUID", MemberGUID));
                            SqlDataReader dr = cmd.ExecuteReader();
                            if (dr.Read())
                                c = (int)dr[0];
                            dr.Close();
                            dr = null;
                        }
                        if (c > 0) { r = "The selected member is the SubFunc's admin already."; }
                        else
                        {
                            sSQL = "Insert Into PORTAL_SubFuncAdmins (SubFuncGUID, MemberGUID) Values (@SubFuncGUID, @MemberGUID);";
                            using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal, tran))
                            {
                                cmd.Parameters.Add(new SqlParameter("@SubFuncGUID", SubFuncGUID));
                                cmd.Parameters.Add(new SqlParameter("@MemberGUID", MemberGUID));
                                try { cmd.ExecuteNonQuery(); }
                                catch (Exception e) { r = e.Message; }
                            }
                        }

                        //release Lock
                        using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal, tran))
                        {
                            SQMDataLockHelper.ReleaseLock(cmd, SubFuncGUID, LoginMemberGUID, RunAsMemberGUID);
                        }
                    }

                //Commit
                if (r == "")
                {
                    try { tran.Commit(); }
                    catch (Exception e) { tran.Rollback(); r = "Add Admin fail.<br />Exception: " + e.ToString(); }
                }
                else
                    tran.Rollback();
                return r;
            }
        }
        #endregion

        #region Remove SubFunc Admin(s)
        public static string RemoveaSubFuncAdmin(SqlConnection cnPortal, string SubFuncGUID, string MemberGUID, string LoginMemberGUID, string RunAsMemberGUID)
        { return RemoveSubFuncAdmin(cnPortal, SubFuncGUID, RemoveOptions.Remove1, MemberGUID, LoginMemberGUID, RunAsMemberGUID); }

        public static string RemoveaSubFuncAdmin(SqlConnection cnPortal, string SubFuncGUID, string LoginMemberGUID, string RunAsMemberGUID)
        { return RemoveSubFuncAdmin(cnPortal, SubFuncGUID, RemoveOptions.RemoveAll, "", LoginMemberGUID, RunAsMemberGUID); }

        private static string RemoveSubFuncAdmin(SqlConnection cnPortal, string SubFuncGUID, RemoveOptions RO, string MemberGUID, string LoginMemberGUID, string RunAsMemberGUID)
        {
            string r = "";


            if (r != "")
            { return r; }
            else
            {
                SqlTransaction tran = cnPortal.BeginTransaction();
                string rl = SQMDataLockHelper.AcquireLock(cnPortal, tran, SubFuncGUID, LoginMemberGUID, RunAsMemberGUID);
                if (rl == "l") { r = "Data already lock by other user."; }
                else
                    if (rl == "e") { r = "Data lock fail or application error."; }
                    else
                    {
                        string sSQL = "Delete From PORTAL_SubFuncAdmins Where SubFuncGUID = @SubFuncGUID";
                        if (RO == RemoveOptions.Remove1) sSQL += " And MemberGUID = @MemberGUID;";
                        sSQL += ";";
                        using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal, tran))
                        {
                            cmd.Parameters.Add(new SqlParameter("@SubFuncGUID", SubFuncGUID));
                            if (RO == RemoveOptions.Remove1) cmd.Parameters.Add(new SqlParameter("@MemberGUID", MemberGUID));
                            try { cmd.ExecuteNonQuery(); }
                            catch (Exception e) { r = e.Message; }
                        }

                        //release Lock
                        using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal, tran))
                        {
                            SQMDataLockHelper.ReleaseLock(cmd, SubFuncGUID, LoginMemberGUID, RunAsMemberGUID);
                        }
                    }

                //Commit
                if (r == "")
                {
                    try { tran.Commit(); }
                    catch (Exception e) { tran.Rollback(); r = "Remove admin fail.<br />Exception: " + e.ToString(); }
                }
                else
                    tran.Rollback();
                return r;
            }
        }
        #endregion

        #region Get SubFunc Members
        public static string GetSubFuncMembersToJQGridJSon(SqlConnection cn, string SubFuncGUID, string SearchText)
        {
            SystemMgmt_Member_jQGridJSon m = new SystemMgmt_Member_jQGridJSon();

            m.Rows.Clear();
            if (SubFuncGUID != "")
            {
                string sSQL = "SELECT Top 100 m.MemberGUID, m.AccountID, m.NameInChinese, m.NameInEnglish FROM PORTAL_MemberSubFuncs mr WITH (NOLOCK) ";
                sSQL += "Join PORTAL_Members m On mr.MemberGUID = m.MemberGUID ";
                sSQL += "Where mr.SubFuncGUID = @SubFuncGUID";

                string sSearchText = SearchText.Trim();
                if (sSearchText != "")
                    sSQL += " And ((m.AccountID like '%' + @SearchText + '%') Or (m.NameInChinese Like '%' + @SearchText + '%') Or (m.NameInEnglish Like '%' + @SearchText + '%'))";
                sSQL += ";";
                SqlCommand cmd = new SqlCommand(sSQL, cn);
                cmd.Parameters.Add(new SqlParameter("@SubFuncGUID", SubFuncGUID));
                if (sSearchText != "")
                    cmd.Parameters.Add(new SqlParameter("@SearchText", sSearchText));
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    m.Rows.Add(new SystemMgmt_Member(
                        dr["MemberGUID"].ToString(),
                        HttpUtility.HtmlEncode(dr["AccountID"].ToString()),
                        1,
                        HttpUtility.HtmlEncode(dr["NameInChinese"].ToString()),
                        HttpUtility.HtmlEncode(dr["NameInEnglish"].ToString()),
                        ""));
                }
                dr.Close();
                dr = null;
                cmd = null;
            }

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(m);
        }
        #endregion

        #region Get SubFunc Member Count
        public static string GetSubFuncMembersCount(SqlConnection cn, string SubFuncGUID)
        {
            int r = 0;

            if (SubFuncGUID != null)
                if (SubFuncGUID != "")
                {
                    string sSQL = "SELECT Count(*) FROM PORTAL_MemberSubFuncs mr WITH (NOLOCK) ";
                    sSQL += "Where mr.SubFuncGUID = @SubFuncGUID;";

                    using (SqlCommand cmd = new SqlCommand(sSQL, cn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@SubFuncGUID", SubFuncGUID));
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                            r = (int)dr[0];
                        dr.Close();
                        dr = null;
                    }
                }

            return @"{""count"": " + r.ToString() + "}";
        }
        #endregion

        #region Add a SubFunc Member
        public static string AddaSubFuncMember(SqlConnection cnPortal, string SubFuncGUID, string MemberGUID, string LoginMemberGUID, string RunAsMemberGUID)
        {
            string r = "";


            if (r != "")
            { return r; }
            else
            {
                SqlTransaction tran = cnPortal.BeginTransaction();
                string rl = SQMDataLockHelper.AcquireLock(cnPortal, tran, SubFuncGUID, LoginMemberGUID, RunAsMemberGUID);
                if (rl == "l") { r = "Data already lock by other user."; }
                else
                    if (rl == "e") { r = "Data lock fail or application error."; }
                    else
                    {
                        int c = 0;
                        string sSQL = "Select Count(*) From PORTAL_MemberSubFuncs WITH (NOLOCK) Where SubFuncGUID = @SubFuncGUID And MemberGUID = @MemberGUID;";
                        using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal, tran))
                        {
                            cmd.Parameters.Add(new SqlParameter("@SubFuncGUID", SubFuncGUID));
                            cmd.Parameters.Add(new SqlParameter("@MemberGUID", MemberGUID));
                            SqlDataReader dr = cmd.ExecuteReader();
                            if (dr.Read())
                                c = (int)dr[0];
                            dr.Close();
                            dr = null;
                        }
                        if (c > 0) { r = "The selected member is the SubFunc's member already."; }
                        else
                        {
                            sSQL = "Insert Into PORTAL_MemberSubFuncs (SubFuncGUID, MemberGUID) Values (@SubFuncGUID, @MemberGUID);";
                            using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal, tran))
                            {
                                cmd.Parameters.Add(new SqlParameter("@SubFuncGUID", SubFuncGUID));
                                cmd.Parameters.Add(new SqlParameter("@MemberGUID", MemberGUID));
                                try { cmd.ExecuteNonQuery(); }
                                catch (Exception e) { r = e.Message; }
                            }
                        }

                        //release Lock
                        using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal, tran))
                        {
                            SQMDataLockHelper.ReleaseLock(cmd, SubFuncGUID, LoginMemberGUID, RunAsMemberGUID);
                        }
                    }

                //Commit
                if (r == "")
                {
                    try { tran.Commit(); }
                    catch (Exception e) { tran.Rollback(); r = "Add member fail.<br />Exception: " + e.ToString(); }
                }
                else
                    tran.Rollback();
                return r;
            }
        }
        #endregion

        #region Remove SubFunc Member(s)
        public static string RemoveaSubFuncMember(SqlConnection cnPortal, string SubFuncGUID, string MemberGUID, string LoginMemberGUID, string RunAsMemberGUID)
        { return RemoveSubFuncMember(cnPortal, SubFuncGUID, RemoveOptions.Remove1, MemberGUID, LoginMemberGUID, RunAsMemberGUID); }

        public static string RemoveaSubFuncMember(SqlConnection cnPortal, string SubFuncGUID, string LoginMemberGUID, string RunAsMemberGUID)
        { return RemoveSubFuncMember(cnPortal, SubFuncGUID, RemoveOptions.RemoveAll, "", LoginMemberGUID, RunAsMemberGUID); }

        private static string RemoveSubFuncMember(SqlConnection cnPortal, string SubFuncGUID, RemoveOptions RO, string MemberGUID, string LoginMemberGUID, string RunAsMemberGUID)
        {
            string r = "";


            if (r != "")
            { return r; }
            else
            {
                SqlTransaction tran = cnPortal.BeginTransaction();
                string rl = SQMDataLockHelper.AcquireLock(cnPortal, tran, SubFuncGUID, LoginMemberGUID, RunAsMemberGUID);
                if (rl == "l") { r = "Data already lock by other user."; }
                else
                    if (rl == "e") { r = "Data lock fail or application error."; }
                    else
                    {
                        string sSQL = "Delete From PORTAL_MemberSubFuncs Where SubFuncGUID = @SubFuncGUID";
                        if (RO == RemoveOptions.Remove1) sSQL += " And MemberGUID = @MemberGUID;";
                        sSQL += ";";
                        using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal, tran))
                        {
                            cmd.Parameters.Add(new SqlParameter("@SubFuncGUID", SubFuncGUID));
                            if (RO == RemoveOptions.Remove1) cmd.Parameters.Add(new SqlParameter("@MemberGUID", MemberGUID));
                            try { cmd.ExecuteNonQuery(); }
                            catch (Exception e) { r = e.Message; }
                        }

                        //release Lock
                        using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal, tran))
                        {
                            SQMDataLockHelper.ReleaseLock(cmd, SubFuncGUID, LoginMemberGUID, RunAsMemberGUID);
                        }
                    }

                //Commit
                if (r == "")
                {
                    try { tran.Commit(); }
                    catch (Exception e) { tran.Rollback(); r = "Remove member fail.<br />Exception: " + e.ToString(); }
                }
                else
                    tran.Rollback();
                return r;
            }
        }
        #endregion

        #region SearchDelegatedSubFunc
        public static string GetDelegatedDataToJQGridJson(SqlConnection cn, string SearchText, string MemberGUID)
        {
            SystemMgmt_SubFunc_jQGridJSon m = new SystemMgmt_SubFunc_jQGridJSon();

            string sSearchText = SearchText.Trim();
            string sWhereClause = " Where ra.MemberGUID = @MemberGUID";
            if (sSearchText != "")
                sWhereClause += " and SubFuncName like '%' + @SearchText + '%'";

            m.Rows.Clear();
            int iRowCount = 0;
            string sSQL = "SELECT Top 100 r.SubFuncGUID, r.SubFuncName From TB_SQM_SUBFUNC r WITH (NOLOCK) Join PORTAL_SubFuncAdmins ra WITH (NOLOCK) On r.SubFuncGUID = ra.SubFuncGUID";
            sSQL += sWhereClause + ";";
            using (SqlCommand cmd = new SqlCommand(sSQL, cn))
            {
                cmd.Parameters.Add(new SqlParameter("@MemberGUID", MemberGUID));
                if (sSearchText != "")
                    cmd.Parameters.Add(new SqlParameter("@SearchText", sSearchText));
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    iRowCount++;
                    m.Rows.Add(new SystemMgmt_SubFunc(
                        dr["SubFuncGUID"].ToString(),
                        HttpUtility.HtmlEncode(dr["SubFuncName"].ToString())));
                }
                dr.Close();
                dr = null;
            }

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(m);
        }
        #endregion
    }
}
