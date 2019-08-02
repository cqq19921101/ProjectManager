using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Web.Script.Serialization;
using Lib_SQM_Domain.SharedLibs;
using System.Net.Mail;
using System.Data;
using System.Configuration;
using System.Web;

namespace Lib_SQM_Domain.Modal
{
    #region Data Class Definitions
    public class SystemMgmt_Member
    {
        protected string _AccountGUID = "";
        protected string _AccountID = "";
        protected int _MemberType = 0;
        protected string _NameInChinese = "";
        protected string _NameInEnglish = "";
        protected string _PrimaryEmail = "";

        public string AccountGUID { get { return this._AccountGUID; } set { this._AccountGUID = value; } }
        public string AccountID { get { return this._AccountID; } set { this._AccountID = value; } }
        public string MemberType
        {
            get
            {
                string sMemberType = "";
                switch (this._MemberType)
                {
                    case 1: sMemberType = "External Member"; break;
                    default: sMemberType = "Internal Member"; break;
                }
                return sMemberType;
            }
            set
            {
                switch (value)
                {
                    case "External Member": this._MemberType = 1; break;
                    default: this._MemberType = 2; break;
                }
            }
        }
        public string NameInChinese { get { return this._NameInChinese; } set { this._NameInChinese = value; } }
        public string NameInEnglish { get { return this._NameInEnglish; } set { this._NameInEnglish = value; } }
        public string PrimaryEmail { get { return this._PrimaryEmail; } set { this._PrimaryEmail = value; } }
        //public string PasswordHash { get { return this._PasswordHash; } set { this._PasswordHash = value; } }

        public SystemMgmt_Member() { }
        public SystemMgmt_Member(string AccountGUID, string AccountID, int MemberType, string NameInChinese, string NameInEnglish, string PrimaryEmail)
        {
            this._AccountGUID = AccountGUID;
            this._AccountID = AccountID;
            this._MemberType = MemberType;
            this._NameInChinese = NameInChinese;
            this._NameInEnglish = NameInEnglish;
            this._PrimaryEmail = PrimaryEmail;
            //this._PasswordHash = PasswordHash;
        }
    }

    public class SQMMgmt_SubFuncs
    {
        protected string _SubFuncGUID = "";
        protected string _SubFuncName = "";

        public string SubFuncGUID { get { return this._SubFuncGUID; } set { this._SubFuncGUID = value; } }
        public string SubFuncName { get { return this._SubFuncName; } set { this._SubFuncName = value; } }

        public SQMMgmt_SubFuncs() { }
        public SQMMgmt_SubFuncs(string SubFuncGUID, string SubFuncName)
        {
            this._SubFuncGUID = SubFuncGUID;
            this._SubFuncName = SubFuncName;
        }
    }

    public class SQMMgmt_SubFuncs_jQGridJSon
    {
        public List<SQMMgmt_SubFuncs> Rows = new List<SQMMgmt_SubFuncs>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }

    public class SQMMgmt_SubFuncsMap
    {
        protected string _FunctionGUID = "";
        public string FunctionGUID { get { return this._FunctionGUID; } set { this._FunctionGUID = value; } }

        protected string _SubFuncGUID = "";
        public string SubFuncGUID { get { return this._SubFuncGUID; } set { this._SubFuncGUID = value; } }

        protected string _SubFuncName = "";
        public string SubFuncName { get { return this._SubFuncName; } set { this._SubFuncName = value; } }

        public SQMMgmt_SubFuncsMap() { }
        public SQMMgmt_SubFuncsMap(string FunctionGUID, string SubFuncGUID, string SubFuncName)
        {
            this._FunctionGUID = FunctionGUID;
            this._SubFuncGUID = SubFuncGUID;
            this._SubFuncName = SubFuncName;
        }
    }

    public class SQMMgmt_SubFuncsMap_jQGridJSon
    {
        public List<SQMMgmt_SubFuncsMap> Rows = new List<SQMMgmt_SubFuncsMap>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }

    public class SQMMgmt_RoleSubFuncsMap
    {
        protected string _FunctionGUID = "";
        public string FunctionGUID { get { return this._FunctionGUID; } set { this._FunctionGUID = value; } }

        protected string _RoleGUID = "";
        public string RoleGUID { get { return this._RoleGUID; } set { this._RoleGUID = value; } }


        protected string _SubFuncGUID = "";
        public string SubFuncGUID { get { return this._SubFuncGUID; } set { this._SubFuncGUID = value; } }

        protected string _SubFuncName = "";
        public string SubFuncName { get { return this._SubFuncName; } set { this._SubFuncName = value; } }

        public SQMMgmt_RoleSubFuncsMap() { }
        public SQMMgmt_RoleSubFuncsMap(string FunctionGUID, string RoleGUID, string SubFuncGUID, string SubFuncName)
        {
            this._FunctionGUID = FunctionGUID;
            this._RoleGUID = RoleGUID;
            this._SubFuncGUID = SubFuncGUID;
            this._SubFuncName = SubFuncName;
        }
    }

    public class SQMMgmt_RoleSubFuncsMap_jQGridJSon
    {
        public List<SQMMgmt_RoleSubFuncsMap> Rows = new List<SQMMgmt_RoleSubFuncsMap>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }

    public class SystemMgmt_Member_jQGridJSon
    {
        public List<SystemMgmt_Member> Rows = new List<SystemMgmt_Member>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }

    public class InternalMemberInfo
    {
        private readonly SystemMgmt_Member _Member;
        private readonly string _ErrMsg = "";

        public SystemMgmt_Member Member { get { return this._Member; } }
        public string ErrMsg { get { return this._ErrMsg; } }

        public InternalMemberInfo(SystemMgmt_Member Member, string ErrMsg)
        {
            this._Member = Member;
            this._ErrMsg = ErrMsg;
        }
    }
    #endregion

    public static class SystemMgmt_Member_Helper
    {
        #region SearchMember
        public static string GetDataToJQGridJSon(SqlConnection cn)
        {
            return GetDataToJQGridJSon(cn, "", "");
        }

        public static string GetDataToJQGridJSon(SqlConnection cn, string SearchText, string MemberType)
        {
            SystemMgmt_Member_jQGridJSon m = new SystemMgmt_Member_jQGridJSon();

            string sSearchText = SearchText.Trim();
            string sMemberType = MemberType.Trim(); //1:External 2:Internal other:all
            string sWhereClause = "";
            if (sSearchText != "")
                sWhereClause += " and ((AccountID like '%' + @SearchText + '%') Or (NameInChinese Like '%' + @SearchText + '%') Or (NameInEnglish Like '%' + @SearchText + '%'))";
            switch (MemberType)
            {
                case "1": sWhereClause += " and MemberType = 1"; break;
                case "2": sWhereClause += " and MemberType = 2"; break;
            }
            if (sWhereClause.Length != 0)
                sWhereClause = " Where" + sWhereClause.Substring(4);

            m.Rows.Clear();
            int iRowCount = 0;
            string sSQL = "SELECT Top 100 MemberGUID, AccountID, MemberType, NameInChinese, NameInEnglish, PrimaryEmail FROM PORTAL_Members WITH (NOLOCK)";
            sSQL += sWhereClause + " Order By AccountID;";
            SqlCommand cmd = new SqlCommand(sSQL, cn);
            if (sSearchText != "")
                cmd.Parameters.Add(new SqlParameter("@SearchText", sSearchText));
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                iRowCount++;
                m.Rows.Add(new SystemMgmt_Member(
                    dr["MemberGUID"].ToString(),
                    HttpUtility.HtmlEncode(dr["AccountID"].ToString()),
                    (int)dr["MemberType"],
                    //int.Parse(dr["MemberType"].ToString()),
                    HttpUtility.HtmlEncode(dr["NameInChinese"].ToString()),
                    HttpUtility.HtmlEncode(dr["NameInEnglish"].ToString()),
                    HttpUtility.HtmlEncode(dr["PrimaryEmail"].ToString())));
            }
            dr.Close();
            dr = null;
            cmd = null;

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(m);
        }
        #endregion

        #region Dialog-support methods
        public static string GetInternalMemberInfoJSon(SqlConnection cn, string AccountID)
        {
            SystemMgmt_Member Member = new SystemMgmt_Member();
            string ErrMsg = "";

            string sAccountID = "";

            string[] sAccount = AccountID.Trim().Split(new char[] { '\\' });
            if (sAccount.Length == 2)
            {
                if (sAccount[0].Trim().ToLower() == "liteon")
                    sAccountID = sAccount[1];
                else
                    ErrMsg = @"Only LiteOn domain account can be imported.<br>(Format: liteon\[account])";
            }
            else
            {
                ErrMsg = @"Only LiteOn domain account can be imported.<br>(Format: liteon\[account])";
            }

            if (sAccountID != "")
            {
                string sSQL = "Select Top 1 USERNAME, CUST_12, EMAIL From [USER] WITH (NOLOCK) Where LOGONID=@AccountID;";
                using (SqlCommand cmd = new SqlCommand(sSQL, cn))
                {
                    if (sAccountID != "")
                        cmd.Parameters.Add(new SqlParameter("@AccountID", sAccountID));
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        Member = new SystemMgmt_Member("", "", 0, dr["USERNAME"].ToString(), dr["CUST_12"].ToString(), dr["EMAIL"].ToString());
                        ErrMsg = "";
                    }
                    else
                        ErrMsg = @"Account ID not found.";
                    dr.Close();
                    dr = null;
                }
            }

            InternalMemberInfo m = new InternalMemberInfo(Member, ErrMsg);

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(m);
        }
        #endregion

        #region Create/Edit data check
        private static void UnescapeDataFromWeb(SystemMgmt_Member DataItem)
        {
            DataItem.AccountID = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.AccountID);
            DataItem.NameInChinese = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.NameInChinese);
            DataItem.NameInEnglish = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.NameInEnglish);
            DataItem.PrimaryEmail = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.PrimaryEmail);
        }

        private static string DataCheck(SystemMgmt_Member DataItem)
        {
            string r = "";
            List<string> e = new List<string>();
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.AccountID))
                e.Add("Must provide Account ID.");

            //目前暫定檢查規則為中文姓名/英文姓名/Email欄皆為必填
            //if (Member.MemberType == "External Member")
            //{
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.NameInChinese))
                e.Add("Must provide Name (Chinese).");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.NameInEnglish))
                e.Add("Must provide Name (English).");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.PrimaryEmail))
                e.Add("Must provide Primary Email.");
            //}

            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }
        #endregion

        #region Create data item
        public static string CreateDataItem(SqlConnection cnPortal, SystemMgmt_Member DataItem)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            if (r != "")
            { return r; }
            else
            {
                SqlTransaction tran = cnPortal.BeginTransaction();

                string sErrMsg = "";
                Guid sMemberGUID = Guid.NewGuid();
                string sSQL = "Insert Into PORTAL_Members (MemberGUID, AccountID, MemberType, StatusCode, NameInChinese, NameInEnglish, PrimaryEmail) ";
                sSQL += "Values (@MemberGUID, @AccountID, @MemberType, @StatusCode, @NameInChinese, @NameInEnglish, @PrimaryEmail);";
                using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal, tran))
                {
                    cmd.Parameters.AddWithValue("@MemberGUID", sMemberGUID);
                    cmd.Parameters.AddWithValue("@AccountID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.AccountID));
                    int iMemberType = 2, iStatusCode = 2;
                    if (DataItem.MemberType == "External Member")
                    {
                        iMemberType = 1;
                        iStatusCode = 1;
                    }
                    cmd.Parameters.AddWithValue("@MemberType", iMemberType);
                    cmd.Parameters.AddWithValue("@StatusCode", iStatusCode);
                    cmd.Parameters.AddWithValue("@NameInChinese", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.NameInChinese));
                    cmd.Parameters.AddWithValue("@NameInEnglish", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.NameInEnglish));
                    cmd.Parameters.AddWithValue("@PrimaryEmail", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.PrimaryEmail));

                    try { cmd.ExecuteNonQuery(); }
                    catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                }

                if (sErrMsg == "")
                {
                    //外部帳號密碼先設定成123
                    //object oPasswordHash = DBNull.Value;
                    //if (DataItem.MemberType == "External Member") oPasswordHash = "202cb962ac59075b964b07152d234b70";
                    //cmd.Parameters.AddWithValue("@PasswordHash", oPasswordHash);
                    sSQL = "Insert Into PORTAL_Members_SysInfo (MemberGUID, PasswordHash) Values (@MemberGUID, null);";
                    using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal, tran))
                    {
                        cmd.Parameters.AddWithValue("@MemberGUID", sMemberGUID);
                        try { cmd.ExecuteNonQuery(); }
                        catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                    }
                }

                if (sErrMsg == "")
                {   //Send notification mail (external member only)
                    if (DataItem.MemberType == "External Member")
                    {
                        Guid ResetGUID = Guid.NewGuid();
                        sSQL = "Insert Into PORTAL_PasswordResetInfo (ResetGUID, MemberGUID) Values (@ResetGUID, @MemberGUID);";
                        using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal, tran))
                        {
                            cmd.Parameters.AddWithValue("@ResetGUID", ResetGUID);
                            cmd.Parameters.AddWithValue("@MemberGUID", sMemberGUID);
                            try { cmd.ExecuteNonQuery(); }
                            catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                        }

                        //send notification
                        sErrMsg = SendNotification(DataItem.PrimaryEmail, DataItem.NameInEnglish, DataItem.NameInChinese, ResetGUID);
                    }
                }

                if (sErrMsg == "")
                    tran.Commit();
                else
                    tran.Rollback();

                return sErrMsg;
            }
        }
        #endregion

        #region Send Password Reset Notification Mail
        private static string SendNotification(string PrimaryEmail, string NameInEnglish, string NameInChinese, Guid ResetGUID)
        {
            string r = "";

            MailAddress from = new MailAddress("workflow@liteon.com", "XXX Portal");
            MailAddress to = new MailAddress(PrimaryEmail, NameInEnglish);
            MailMessage message = new MailMessage(from, to);

            message.Subject = "Welcome notification! Activate your account now!"; // <--Subject can be change
            message.IsBodyHtml = true;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Hi " + NameInEnglish + " (" + NameInChinese + "),<p>");
            sb.AppendLine(@"Please click <a href='https://mvcqa.liteon.com/Account/Activate?r=" + ResetGUID.ToString() + @"'>this link</a> to activate your account.<p>");
            sb.AppendLine("Welcome! and thanks a lot.");

            message.Body = sb.ToString();
            if (!MailHelper.SendMail(message))
                r = "Fail to send notification mail.";

            return r;
        }
        #endregion

        #region Edit data item
        public static string EditDataItem(SqlConnection cnPortal, SystemMgmt_Member DataItem)
        {
            return EditDataItem(cnPortal, DataItem, "", "");
        }

        public static string EditDataItem(SqlConnection cnPortal, SystemMgmt_Member DataItem, string LoginMemberGUID, string RunAsMemberGUID)
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
                    using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { bLockIsStillValid = SQMDataLockHelper.CheckLockIsStillValid(cmd, DataItem.AccountGUID, LoginMemberGUID, RunAsMemberGUID); }
                if (!bLockIsStillValid) { tran.Rollback(); return SQMDataLockHelper.LockString(); }

                //Release lock
                if ((LoginMemberGUID != "") && (RunAsMemberGUID != ""))
                    using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = SQMDataLockHelper.ReleaseLock(cmd, DataItem.AccountGUID, LoginMemberGUID, RunAsMemberGUID); }
                if (r != "") { tran.Rollback(); return r; }

                //Commit
                try { tran.Commit(); }
                catch (Exception e) { tran.Rollback(); r = "Edit fail.<br />Exception: " + e.ToString(); }
                return r;
            }
        }

        private static string EditDataItemSub(SqlCommand cmd, SystemMgmt_Member DataItem)
        {
            string sErrMsg = "";

            string sSQL = "Update PORTAL_Members Set AccountID = @AccountID, MemberType = @MemberType, ";
            sSQL += "NameInChinese = @NameInChinese, NameInEnglish = @NameInEnglish, PrimaryEmail = @PrimaryEmail ";
            sSQL += "Where MemberGUID = @MemberGUID;";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@MemberGUID", DataItem.AccountGUID);
            cmd.Parameters.AddWithValue("@AccountID", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.AccountID));
            int iMemberType = 2;
            if (DataItem.MemberType == "External Member")
                iMemberType = 1;
            cmd.Parameters.AddWithValue("@MemberType", iMemberType);
            cmd.Parameters.AddWithValue("@NameInChinese", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.NameInChinese));
            cmd.Parameters.AddWithValue("@NameInEnglish", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.NameInEnglish));
            cmd.Parameters.AddWithValue("@PrimaryEmail", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.PrimaryEmail));

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion

        #region Delete data item
        public static string DeleteDataItem(SqlConnection cnPortal, SystemMgmt_Member DataItem)
        {
            return DeleteDataItem(cnPortal, DataItem, "", "");
        }

        public static string DeleteDataItem(SqlConnection cnPortal, SystemMgmt_Member DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            string r = "";
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.AccountGUID))
                return "Must provide Account ID.";
            else
            {
                SqlTransaction tran = cnPortal.BeginTransaction();

                //Delete member data
                using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = DeleteDataItemSub(cmd, "Delete PORTAL_MemberRoles Where MemberGUID = @MemberGUID;", "@MemberGUID", DataItem.AccountGUID); }
                if (r != "") { tran.Rollback(); return r; }
                using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = DeleteDataItemSub(cmd, "Delete PORTAL_RoleAdmins Where MemberGUID = @MemberGUID;", "@MemberGUID", DataItem.AccountGUID); }
                if (r != "") { tran.Rollback(); return r; }
                using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = DeleteDataItemSub(cmd, "Delete PORTAL_Members Where MemberGUID = @MemberGUID;", "@MemberGUID", DataItem.AccountGUID); }
                if (r != "") { tran.Rollback(); return r; }

                //Release lock
                if ((LoginMemberGUID != "") && (RunAsMemberGUID != ""))
                    using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = SQMDataLockHelper.ReleaseLock(cmd, DataItem.AccountGUID, LoginMemberGUID, RunAsMemberGUID); }
                if (r != "") { tran.Rollback(); return r; }

                //Commit
                try { tran.Commit(); }
                catch (Exception e) { tran.Rollback(); r = "Delete fail.<br />Exception: " + e.ToString(); }
                return r;
            }
        }

        private static string DeleteDataItemSub(SqlCommand cmd, string SQL, string ParaName, string DataItemKey)
        {
            string sErrMsg = "";

            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue(ParaName, DataItemKey);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }

        private static string DeleteDataItemSub1(SqlCommand cmd, SystemMgmt_Member DataItem)
        {
            string sErrMsg = "";

            string sSQL = "Delete PORTAL_MemberRoles Where MemberGUID = @MemberGUID;";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@MemberGUID", DataItem.AccountGUID);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }

        private static string DeleteDataItemSub2(SqlCommand cmd, SystemMgmt_Member DataItem)
        {
            string sErrMsg = "";

            string sSQL = "Delete PORTAL_Members Where MemberGUID = @MemberGUID;";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@MemberGUID", DataItem.AccountGUID);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion

        #region Get Member Roles
        public static string GetMemberRoleFullListJSon(SqlConnection cnPortal, string MemberGUID)
        {
            SystemMgmt_MemberRole_jQGridJSon r = new SystemMgmt_MemberRole_jQGridJSon();

            if (MemberGUID != null)
                if (MemberGUID != "")
                {
                    string sSQL = "Select r.RoleGUID, r.RoleName, s.Belongs From PORTAL_Roles r WITH (NOLOCK) Left Join  ";
                    sSQL += "(Select '1' Belongs, RoleGUID From PORTAL_MemberRoles mr WITH (NOLOCK) Where mr.MemberGUID = @MemberGUID) s ";
                    sSQL += "On r.RoleGUID = s.RoleGUID ";
                    sSQL += "Order By r.RoleName;";
                    using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal))
                    {
                        cmd.Parameters.AddWithValue("@MemberGUID", MemberGUID);
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                            r.Rows.Add(new MemberRoleFullList(dr["RoleGUID"].ToString(), HttpUtility.HtmlEncode(dr["RoleName"].ToString()), (dr["Belongs"] == DBNull.Value) ? false : true));
                        dr.Close();
                        dr = null;
                    }
                }

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(r);
        }

        public class MemberRoleFullList : SystemMgmt_SubFunc
        {
            protected bool _Belongs = false;

            public bool Belongs { get { return this._Belongs; } set { this._Belongs = value; } }

            public MemberRoleFullList(string RoleGUID, string RoleName, bool Belongs)
                : base(RoleGUID, RoleName)
            {
                this._Belongs = Belongs;
            }
        }

        public class SystemMgmt_MemberRole_jQGridJSon
        {
            public List<MemberRoleFullList> Rows = new List<MemberRoleFullList>();
            public string Total
            {
                get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 10)).ToString(); }
            }
            public string Page { get { return "1"; } }
            public string Records { get { return Rows.Count.ToString(); } }
        }

        #endregion

        #region Update Member Roles
        public static string UpdateMemberRoles(SqlConnection cnPortal, string MemberGUID, List<string> MemberRoles)
        {
            return UpdateMemberRoles(cnPortal, MemberGUID, MemberRoles, "", "");
        }

        public static string UpdateMemberRoles(SqlConnection cnPortal, string MemberGUID, List<string> MemberRoles, string LoginMemberGUID, string RunAsMemberGUID)
        {
            string r = "";

            SqlTransaction tran = cnPortal.BeginTransaction();

            //1. Delete current member's roles
            using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = DeleteCurrentMemberRoles(cmd, MemberGUID); }
            if (r != "") { tran.Rollback(); return r; }

            //2. Add new selected roles
            if (MemberRoles != null)
            {
                foreach (string RoleGUID in MemberRoles)
                {
                    using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = AddMemberRoles(cmd, MemberGUID, RoleGUID); }
                    if (r != "") break;
                }
                if (r != "") { tran.Rollback(); return r; }
            }

            //Release lock
            if ((LoginMemberGUID != "") && (RunAsMemberGUID != ""))
                using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = SQMDataLockHelper.ReleaseLock(cmd, MemberGUID, LoginMemberGUID, RunAsMemberGUID); }
            if (r != "") { tran.Rollback(); return r; }

            //Commit
            try { tran.Commit(); }
            catch (Exception e) { tran.Rollback(); r = "Edit fail.<br />Exception: " + e.ToString(); }
            return r;
        }

        private static string DeleteCurrentMemberRoles(SqlCommand cmd, string MemberGUID)
        {
            string sErrMsg = "";

            string sSQL = "Delete PORTAL_MemberRoles Where MemberGUID = @MemberGUID;";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@MemberGUID", MemberGUID);
            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "DeleteCurrentMemberRoles fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }

        private static string AddMemberRoles(SqlCommand cmd, string MemberGUID, string RoleGUID)
        {
            string sErrMsg = "";

            string sSQL = "Insert Into PORTAL_MemberRoles (MemberGUID, RoleGUID) Values (@MemberGUID, @RoleGUID);";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@MemberGUID", MemberGUID);
            cmd.Parameters.AddWithValue("@RoleGUID", RoleGUID);
            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "AddMemberRoles fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion

        #region Activate Member
        public static bool ActivateMemberCheck(string ResetGUID)
        {
            bool r = true;

            SqlConnection cnPortal = DBConnHelper.GetPortalDBConnection(PortalDBType.PortalDB);
            cnPortal.Open();
            string sSQL = "Select m.MemberGUID From PORTAL_PasswordResetInfo r WITH (NOLOCK) Join PORTAL_Members m WITH (NOLOCK) On r.MemberGUID = m.MemberGUID ";
            sSQL += "Where r.ResetGUID = @ResetGUID and r.IsActive = 1 and (m.StatusCode = 1 or m.StatusCode = 4);";
            using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal))
            {
                cmd.Parameters.AddWithValue("@ResetGUID", ResetGUID);
                try
                {
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                    if (!dr.Read()) r = false;
                    dr.Close();
                    dr = null;
                }
                catch { r = false; }
            }

            cnPortal.Close();
            cnPortal.Dispose();
            cnPortal = null;

            return r;
        }
        #endregion

        #region Perform Account Activate
        public static bool PerformAccountActivate(string ResetGUID, string Password)
        {
            bool r = true;

            SqlConnection cnPortal = DBConnHelper.GetPortalDBConnection(PortalDBType.PortalDB);
            cnPortal.Open();
            SqlTransaction tran = cnPortal.BeginTransaction();
            string sMemberGUID = "";
            string sSQL = "Select m.MemberGUID From PORTAL_PasswordResetInfo r WITH (NOLOCK) Join PORTAL_Members m WITH (NOLOCK) On r.MemberGUID = m.MemberGUID ";
            sSQL += "Where r.ResetGUID = @ResetGUID and r.IsActive = 1 and (m.StatusCode = 1 or m.StatusCode = 4);";
            using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal, tran))
            {
                cmd.Parameters.AddWithValue("@ResetGUID", ResetGUID);
                try
                {
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                    if (dr.Read()) sMemberGUID = dr["MemberGUID"].ToString();
                    else r = false;
                    dr.Close();
                    dr = null;
                }
                catch { r = false; }
            }
            if (r)
            {
                sSQL = "Update PORTAL_Members Set StatusCode = 2 Where MemberGUID = @MemberGUID;";
                sSQL += "Update PORTAL_PasswordResetInfo Set IsActive = 0 Where ResetGUID = @ResetGUID;";
                sSQL += "Update PORTAL_Members_SysInfo Set PasswordHash = @PasswordHash, LoginFailCount = 0, LastChangePWDTime = GetDate() Where MemberGUID = @MemberGUID;";

                using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal, tran))
                {
                    cmd.Parameters.AddWithValue("@MemberGUID", sMemberGUID);
                    cmd.Parameters.AddWithValue("@ResetGUID", ResetGUID);
                    cmd.Parameters.AddWithValue("@PasswordHash", SecurityHelper.MD5Hash(Password));
                    try { cmd.ExecuteNonQuery(); }
                    catch { r = false; }
                }
            }

            if (r)
                tran.Commit();
            else
                tran.Rollback();

            cnPortal.Close();
            cnPortal.Dispose();
            cnPortal = null;

            return r;
        }
        #endregion

        #region Change Password
        public static string ChangePassword(SqlConnection cnPortal, string MemberGUID, string Password)
        {
            string r = "";

            string sSQL = "Update PORTAL_Members_SysInfo Set PasswordHash = @PasswordHash Where MemberGUID = @MemberGUID;";
            using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal))
            {
                cmd.Parameters.AddWithValue("@MemberGUID", MemberGUID);
                cmd.Parameters.AddWithValue("@PasswordHash", SecurityHelper.MD5Hash(Password));
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { r = e.Message; }
            }

            return r;
        }
        #endregion

        #region Perform Forgot Password
        public static string PerformForgotPassword(string txtAccountID, string txtPrimaryEmail)
        {
            string r = "";

            if (txtAccountID == "") r += "<br />Must provide Account ID.";
            if (txtPrimaryEmail == "") r += "<br />Must provide Primary Email.";
            if (r != "")
                r = r.Substring(6);
            else
            {
                SqlConnection cnPortal = DBConnHelper.GetPortalDBConnection(PortalDBType.PortalDB);
                cnPortal.Open();

                string sMemberGUID = "";
                string sPrimaryEmail = "";
                string sNameInEnglish = "";
                string sNameInChinese = "";
                string sSQL = @"Select MemberGUID, PrimaryEmail, NameInEnglish, NameInChinese From PORTAL_Members WITH (NOLOCK) Where AccountID = @AccountID And PrimaryEmail = @PrimaryEmail;";
                using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal))
                {
                    cmd.Parameters.AddWithValue("@AccountID", txtAccountID);
                    cmd.Parameters.AddWithValue("@PrimaryEmail", txtPrimaryEmail);
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                    if (dr.Read())
                    {
                        sMemberGUID = dr["MemberGUID"].ToString();
                        sPrimaryEmail = dr["PrimaryEmail"].ToString();
                        sNameInEnglish = dr["NameInEnglish"].ToString();
                        sNameInChinese = dr["NameInChinese"].ToString();
                    }
                    else
                        r = "Account information might not correct. Password reset failed.";
                    dr.Close();
                    dr.Dispose();
                    dr = null;
                }

                if (r == "")
                {
                    SqlTransaction tran = cnPortal.BeginTransaction();
                    Guid ResetGUID = Guid.NewGuid();
                    sSQL = "Update PORTAL_Members Set StatusCode = 4 Where MemberGUID = @MemberGUID;";
                    sSQL += "Update PORTAL_PasswordResetInfo Set IsActive = 0 Where MemberGUID = @MemberGUID;";
                    sSQL += "Insert Into PORTAL_PasswordResetInfo (ResetGUID, MemberGUID) Values (@ResetGUID, @MemberGUID);";
                    sSQL += "Update PORTAL_Members_SysInfo Set LastResetPWDTime = GETDATE() Where MemberGUID = @MemberGUID;";
                    using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal, tran))
                    {
                        cmd.Parameters.AddWithValue("@MemberGUID", sMemberGUID);
                        cmd.Parameters.AddWithValue("@ResetGUID", ResetGUID);
                        try { cmd.ExecuteNonQuery(); }
                        catch (Exception e) { r = e.Message; }
                    }

                    if (r == "")
                        r = SendNotification(sPrimaryEmail, sNameInEnglish, sNameInChinese, ResetGUID);

                    if (r == "")
                        tran.Commit();
                    else
                        tran.Rollback();
                }

                cnPortal.Close();
                cnPortal.Dispose();
                cnPortal = null;
            }

            return r;
        }
        #endregion
    }
}
