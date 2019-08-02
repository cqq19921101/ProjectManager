using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;

namespace Lib_SQM_Domain.SharedLibs
{
    public class LockInfo
    {
        public bool Locked = false;
        public string LockerID = "";
        public string LockerNameC = "";
        public string LockerNameE = "";
        public DateTime LockTime;
    }

    public enum LockResutCode
    {
        Success = 0,
        IsLocked = 1,
        Error = 2
    }

    public class LockResult
    {
        public LockResutCode ResultCode = LockResutCode.Error;
        public string LockerID = "";
        public string LockerTitle = "";
        public DateTime LockTime = DateTime.Now;
        public string FailMessage = "";

        public LockResult() { }
    }

    public static class SQMDataLockHelper
    {
        const int MaxLockPeriodInMinute = 40;

        public static string LockString()
        {
            return PortalGlobalConstantHelper.GetConstant(enumPortalGlobalConstant.LockIsNotValid);
        }

        public static string AcquireLock(string DataKey, string LoginMemberGUID, string RunAsMemberGUID)
        {
            string r = "";

            SqlConnection cnPortal = DBConnHelper.GetPortalDBConnectionOpen(PortalDBType.PortalDB);
            r = AcquireLock(cnPortal, null, DataKey, LoginMemberGUID, RunAsMemberGUID);
            cnPortal.Close();

            return r;
        }

        public static string AcquireLock(SqlConnection cnPortal, SqlTransaction tran, string DataKey, string LoginMemberGUID, string RunAsMemberGUID)
        {
            string r = "";

            string sLockedLoginMemberGUID = "";
            string sLockedRunAsMemberGUID = "";
            DateTime LockTime = DateTime.Now;
            string sSQL = "Select Top 1 LoginMemberGUID, RunAsMemberGUID, LockTime From PORTAL_DataLockTracking with (ROWLOCK) ";
            sSQL += "Where DataGUID = @DataGUID;";
            using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal, tran))
            {
                cmd.CommandText = sSQL;
                cmd.Parameters.AddWithValue("@DataGUID", DataKey);
                try {
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                    if (dr.Read())
                    {
                        r = "l";
                        sLockedLoginMemberGUID = dr["LoginMemberGUID"].ToString();
                        sLockedRunAsMemberGUID = dr["RunAsMemberGUID"].ToString();
                        LockTime = (DateTime)dr["LockTime"];
                    }
                    dr.Close();
                    dr = null;
                }
                catch {
                    r = "e";
                }
            }
            //finally { cmd = null; }
            if (r == "l")
            {
                if ((sLockedLoginMemberGUID.ToLower() == LoginMemberGUID.ToLower()) && (sLockedRunAsMemberGUID.ToLower() == RunAsMemberGUID))
                {
                    sSQL = "Update PORTAL_DataLockTracking with (ROWLOCK) Set LockTime = GETDATE() Where DataGUID = @DataGUID;";
                    using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal, tran))
                    {
                        cmd.Parameters.AddWithValue("@DataGUID", DataKey);
                        try { cmd.ExecuteNonQuery(); r = "ok"; }
                        catch { r = "e"; }
                    }
                }
                else
                {
                    DateTime dtNow = DateTime.Now;
                    if ((dtNow - LockTime).TotalSeconds > (MaxLockPeriodInMinute * 60))
                    {   //Previous lock expired, update lock
                        sSQL = "Update PORTAL_DataLockTracking with (ROWLOCK) Set LockTime = GETDATE(), LoginMemberGUID = @LoginMemberGUID, RunAsMemberGUID = @RunAsMemberGUID Where DataGUID = @DataGUID;";
                        using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal, tran))
                        {
                            cmd.Parameters.AddWithValue("@LoginMemberGUID", LoginMemberGUID);
                            cmd.Parameters.AddWithValue("@RunAsMemberGUID", RunAsMemberGUID);
                            cmd.Parameters.AddWithValue("@DataGUID", DataKey);
                            try { cmd.ExecuteNonQuery(); return "ok"; }
                            catch { r = "e"; }
                        }
                    }
                }
            }
            else
            {
                if (r == "")
                {
                    sSQL = "Insert Into PORTAL_DataLockTracking with (ROWLOCK) (DataGUID, LoginMemberGUID, RunAsMemberGUID) ";
                    sSQL += "Values (@DataGUID, @LoginMemberGUID, @RunAsMemberGUID);";
                    using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal, tran))
                    {
                        cmd.Parameters.AddWithValue("@DataGUID", DataKey);
                        cmd.Parameters.AddWithValue("@LoginMemberGUID", LoginMemberGUID);
                        cmd.Parameters.AddWithValue("@RunAsMemberGUID", RunAsMemberGUID);
                        try { cmd.ExecuteNonQuery(); r = "ok"; }
                        catch { r = "e"; }
                    }
                }
            }

            //if (r != "ok")
            //    tran.Rollback();
            return r;
        }

        public static LockResult AcquireLockV2(SqlConnection cnPortal, SqlTransaction tran, string DataKey, string LoginMemberGUID, string RunAsMemberGUID)
        {
            LockResult Result = new LockResult();

            string sLockedLoginMemberGUID = "";
            string sLockedRunAsMemberGUID = "";
            string sSQL = "Select Top 1 LoginMemberGUID, RunAsMemberGUID, LockTime From PORTAL_DataLockTracking with (ROWLOCK) ";
            sSQL += "Where DataGUID = @DataGUID;";
            using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal, tran))
            {
                cmd.CommandText = sSQL;
                cmd.Parameters.AddWithValue("@DataGUID", DataKey);
                try
                {
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                    if (dr.Read())
                    {
                        Result.ResultCode = LockResutCode.IsLocked;
                        sLockedLoginMemberGUID = dr["LoginMemberGUID"].ToString();
                        sLockedRunAsMemberGUID = dr["RunAsMemberGUID"].ToString();
                        Result.LockTime = (DateTime)dr["LockTime"];
                    }
                    dr.Close();
                    dr = null;
                }
                catch (Exception e)
                {
                    Result.ResultCode = LockResutCode.Error;
                    Result.FailMessage = e.Message;
                    return Result;
                }
            }
            
            if (Result.ResultCode == LockResutCode.IsLocked)
            {
                if ((sLockedLoginMemberGUID.ToLower() == LoginMemberGUID.ToLower()) && (sLockedRunAsMemberGUID.ToLower() == RunAsMemberGUID))
                {
                    sSQL = "Update PORTAL_DataLockTracking with (ROWLOCK) Set LockTime = GETDATE() Where DataGUID = @DataGUID;";
                    using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal, tran))
                    {
                        cmd.Parameters.AddWithValue("@DataGUID", DataKey);
                        try {
                            cmd.ExecuteNonQuery();
                            Result.ResultCode = LockResutCode.Success;
                        }
                        catch (Exception e) {
                            Result.ResultCode = LockResutCode.Error;
                            Result.FailMessage = e.Message;
                            return Result;
                        }
                    }
                }
                else
                {
                    DateTime dtNow = DateTime.Now;
                    if ((dtNow - Result.LockTime).TotalSeconds > (MaxLockPeriodInMinute * 60))
                    {   //Previous lock expired, update lock
                        sSQL = "Update PORTAL_DataLockTracking with (ROWLOCK) Set LockTime = GETDATE(), LoginMemberGUID = @LoginMemberGUID, RunAsMemberGUID = @RunAsMemberGUID Where DataGUID = @DataGUID;";
                        using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal, tran))
                        {
                            cmd.Parameters.AddWithValue("@LoginMemberGUID", LoginMemberGUID);
                            cmd.Parameters.AddWithValue("@RunAsMemberGUID", RunAsMemberGUID);
                            cmd.Parameters.AddWithValue("@DataGUID", DataKey);
                            try
                            {
                                cmd.ExecuteNonQuery();
                                Result.ResultCode = LockResutCode.Success;
                            }
                            catch (Exception e)
                            {
                                Result.ResultCode = LockResutCode.Error;
                                Result.FailMessage = e.Message;
                                return Result;
                            }
                        }
                    }
                    else
                    {   //Current lock is not locked by current user, so return current locker to caller
                        string LockerNameC = "";
                        string LockerNameE = ""; 

                        StringBuilder sb = new StringBuilder();
                        sSQL = "Select Top 1 AccountID, NameInChinese, NameInEnglish From PORTAL_Members with (nolock) Where MemberGUID = @MemberGUID;";
                        using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal, tran))
                        {
                            cmd.Parameters.AddWithValue("@MemberGUID", sLockedRunAsMemberGUID);
                            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                            if (dr.Read())
                            {
                                Result.LockerID = dr["AccountID"].ToString();
                                LockerNameC = dr["NameInChinese"].ToString();
                                LockerNameE = dr["NameInEnglish"].ToString();
                                Result.LockerTitle = ((LockerNameC != "") || (LockerNameE != "")) ? LockerNameC + " (" + LockerNameE + ")" : Result.LockerID;
                            }
                            else
                            {
                                Result.LockerID = "(unknown)";
                                Result.LockerTitle = "(unknown)";
                            }
                            dr.Close();
                            dr.Dispose();
                            dr = null;
                        }
                    }
                }
            }
            else
            {
                sSQL = "Insert Into PORTAL_DataLockTracking with (ROWLOCK) (DataGUID, LoginMemberGUID, RunAsMemberGUID) ";
                sSQL += "Values (@DataGUID, @LoginMemberGUID, @RunAsMemberGUID);";
                using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal, tran))
                {
                    cmd.Parameters.AddWithValue("@DataGUID", DataKey);
                    cmd.Parameters.AddWithValue("@LoginMemberGUID", LoginMemberGUID);
                    cmd.Parameters.AddWithValue("@RunAsMemberGUID", RunAsMemberGUID);
                    try {
                        cmd.ExecuteNonQuery();
                        Result.ResultCode = LockResutCode.Success;
                    }
                    catch (Exception e) {
                        Result.ResultCode = LockResutCode.Error;
                        Result.FailMessage = e.Message;
                        return Result;
                    }
                }
            }

            return Result;
        }

        public static string ReleaseLock(string DataKey, string LoginMemberGUID, string RunAsMemberGUID)
        {
            string r = "";

            SqlConnection cnPortal = DBConnHelper.GetPortalDBConnectionOpen(PortalDBType.PortalDB);
            SqlCommand cmd = new SqlCommand("", cnPortal);
            r = ReleaseLock(cmd, DataKey, LoginMemberGUID, RunAsMemberGUID);
            cmd = null;
            cnPortal.Close();

            return r;
        }

        public static string ReleaseLock(SqlCommand cmd, string DataKey, string LoginMemberGUID, string RunAsMemberGUID)
        {
            string r = "";

            string sSQL = "Delete From PORTAL_DataLockTracking with (ROWLOCK) ";
            sSQL += "Where DataGUID = @DataGUID and LoginMemberGUID = @LoginMemberGUID and RunAsMemberGUID = @RunAsMemberGUID;";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@DataGUID", DataKey);
            cmd.Parameters.AddWithValue("@LoginMemberGUID", LoginMemberGUID);
            cmd.Parameters.AddWithValue("@RunAsMemberGUID", RunAsMemberGUID);
            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { r = e.Message; }

            return r;
        }

        //public static bool CheckLockIsStillValid(SqlConnection cnPortal, string DataKey, string LoginMemberGUID, string RunAsMemberGUID)
        //{
        //    return CheckLockIsStillValid(cnPortal, null, DataKey, LoginMemberGUID, RunAsMemberGUID);
        //}

        public static bool CheckLockIsStillValid(SqlCommand cmd, string DataKey, string LoginMemberGUID, string RunAsMemberGUID)
        {
            bool IsStillValid = false;

            string sSQL = "Select LockTime From PORTAL_DataLockTracking with (ROWLOCK) ";
            sSQL += "Where DataGUID = @DataGUID and LoginMemberGUID = @LoginMemberGUID and RunAsMemberGUID = @RunAsMemberGUID;";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@DataGUID", DataKey);
            cmd.Parameters.AddWithValue("@LoginMemberGUID", LoginMemberGUID);
            cmd.Parameters.AddWithValue("@RunAsMemberGUID", RunAsMemberGUID);
            try
            {
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                if (dr.Read())
                    if ((DateTime.Now - ((DateTime)dr["LockTime"])).TotalSeconds <= ((MaxLockPeriodInMinute - 1) * 60))
                        IsStillValid = true;
                dr.Close();
                dr = null;
            } catch { }

            return IsStillValid;
        }

        public static LockInfo CheckDataLockIsLock(SqlConnection cn, string DataKey, string LoginMemberGUID, string RunAsMemberGUID, bool LockByMeIsNotLock)
        {
            return CheckDataLockIsLock(cn, null, DataKey, LoginMemberGUID, RunAsMemberGUID, LockByMeIsNotLock);
        }

        public static LockInfo CheckDataLockIsLock(SqlConnection cn, SqlTransaction tr, string DataKey, string LoginMemberGUID, string RunAsMemberGUID, bool LockByMeIsNotLock)
        {
            //LockByMeIsNotLock:
	        //  true: 若是本人Lock則不算Lock
	        //  false: 只要有Lock就算Lock

            LockInfo Lock = new LockInfo();
            Lock.Locked = false;

            StringBuilder sb = new StringBuilder();
            sb.Append("Select Top 1 l.LockTime, l.LoginMemberGUID, l.RunAsMemberGUID, m.AccountID, m.NameInChinese, m.NameInEnglish");
            sb.Append(" From PORTAL_DataLockTracking l with (ROWLOCK) Left Join PORTAL_Members m with (nolock) On l.RunAsMemberGUID = m.MemberGUID");
            sb.Append(" Where DataGUID = @DataGUID;");

            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                if(tr!=null)
                    cmd.Transaction = tr;

                cmd.Parameters.AddWithValue("@DataGUID", DataKey);
                try
                {
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                    if (dr.Read())
                    {
                        if ((DateTime.Now - ((DateTime)dr["LockTime"])).TotalSeconds <= ((MaxLockPeriodInMinute - 1) * 60))
                        {
                            Lock.Locked = true;
                            if (LockByMeIsNotLock)
                                if ((dr["LoginMemberGUID"].ToString().ToUpper() == LoginMemberGUID.ToUpper()) &&
                                    (dr["RunAsMemberGUID"].ToString().ToUpper() == RunAsMemberGUID.ToUpper()))
                                    Lock.Locked = false;
                        }
                        //update locker information
                        if (Lock.Locked)
                        {
                            Lock.LockerID = dr["AccountID"].ToString();
                            Lock.LockerNameC = dr["NameInChinese"].ToString();
                            Lock.LockerNameE = dr["NameInEnglish"].ToString();
                        }
                    }
                    dr.Close();
                    dr = null;
                }
                catch
                { }
            }

            return Lock;
        }

        //public static string ReleaseLock(string DataKey, string LoginMemberGUID, string RunAsMemberGUID)
        //{
        //    string r = "";

        //    SqlConnection cnPortal = DBConnHelper.GetPortalDBConnectionOpen(PortalDBType.PortalDB);
        //    string sSQL = "Delete From PORTAL_DataLockTracking ";
        //    sSQL += "Where DataGUID = @DataGUID and LoginMemberGUID = @LoginMemberGUID and RunAsMemberGUID = @RunAsMemberGUID;";
        //    SqlCommand cmd = new SqlCommand(sSQL, cnPortal);
        //    cmd.Parameters.AddWithValue("@DataGUID", DataKey);
        //    cmd.Parameters.AddWithValue("@LoginMemberGUID", LoginMemberGUID);
        //    cmd.Parameters.AddWithValue("@RunAsMemberGUID", RunAsMemberGUID);
        //    try
        //    {
        //        cmd.ExecuteNonQuery();
        //        r = "ok";
        //    }
        //    catch { r = "e"; }
        //    finally { cmd = null; }
        //    cnPortal.Close();

        //    return r;
        //}

        //public static string ReleaseLockviaExistingConnectionAndTransaction(SqlCommand cmd, string DataKey, string LoginMemberGUID, string RunAsMemberGUID)
        //{
        //    string r = "";

        //    string sSQL = "Delete From PORTAL_DataLockTracking ";
        //    sSQL += "Where DataGUID = @DataGUID and LoginMemberGUID = @LoginMemberGUID and RunAsMemberGUID = @RunAsMemberGUID;";
        //    cmd.CommandText = sSQL;
        //    cmd.Parameters.AddWithValue("@DataGUID", DataKey);
        //    cmd.Parameters.AddWithValue("@LoginMemberGUID", LoginMemberGUID);
        //    cmd.Parameters.AddWithValue("@RunAsMemberGUID", RunAsMemberGUID);
        //    try
        //    {
        //        cmd.ExecuteNonQuery();
        //    }
        //    catch(Exception e) { r = e.Message; }

        //    return r;
        //}

        //更嚴格的檢查: 包括自己Lock的也不算
        public static string AcquireLock_OnlyInNoOneLock(string DataKey, string LoginMemberGUID, string RunAsMemberGUID)
        {
            string r = "";

            SqlConnection cnPortal = DBConnHelper.GetPortalDBConnectionOpen(PortalDBType.PortalDB);
            r = AcquireLock_OnlyInNoOneLock(cnPortal, null, DataKey, LoginMemberGUID, RunAsMemberGUID);
            cnPortal.Close();

            return r;
        }

        //更嚴格的檢查: 包括自己Lock的也不算
        public static string AcquireLock_OnlyInNoOneLock(SqlConnection cnPortal, SqlTransaction tran, string DataKey, string LoginMemberGUID, string RunAsMemberGUID)
        {
            string r = "";

            string sLockedLoginMemberGUID = "";
            string sLockedRunAsMemberGUID = "";
            DateTime LockTime = DateTime.Now;
            string sSQL = "Select Top 1 LoginMemberGUID, RunAsMemberGUID, LockTime From PORTAL_DataLockTracking with (ROWLOCK) ";
            sSQL += "Where DataGUID = @DataGUID;";
            using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal, tran))
            {
                cmd.CommandText = sSQL;
                cmd.Parameters.AddWithValue("@DataGUID", DataKey);
                try
                {
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                    if (dr.Read())
                    {
                        r = "l";
                        sLockedLoginMemberGUID = dr["LoginMemberGUID"].ToString();
                        sLockedRunAsMemberGUID = dr["RunAsMemberGUID"].ToString();
                        LockTime = (DateTime)dr["LockTime"];
                    }
                    dr.Close();
                    dr = null;
                }
                catch { r = "e"; }
            }
            //finally { cmd = null; }
            if ((r == "") || ((r == "l")&&((DateTime.Now - LockTime).TotalSeconds >= ((MaxLockPeriodInMinute - 1) * 60))))
            {
                sSQL = "Delete PORTAL_DataLockTracking with (ROWLOCK) Where DataGUID=@DataGUID;";
                sSQL += "Insert Into PORTAL_DataLockTracking with (ROWLOCK) (DataGUID, LoginMemberGUID, RunAsMemberGUID) ";
                sSQL += "Values (@DataGUID, @LoginMemberGUID, @RunAsMemberGUID);";
                using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal, tran))
                {
                    cmd.Parameters.AddWithValue("@DataGUID", DataKey);
                    cmd.Parameters.AddWithValue("@LoginMemberGUID", LoginMemberGUID);
                    cmd.Parameters.AddWithValue("@RunAsMemberGUID", RunAsMemberGUID);
                    try { cmd.ExecuteNonQuery(); r = "ok"; }
                    catch { r = "e"; }
                }
            }

            return r;
        }
    }
}
