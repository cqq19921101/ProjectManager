using Lib_Portal_Domain.Model;
using Lib_Portal_Domain.SharedLibs;
//using System.Text;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;

namespace Lib_SQM_Domain.Model
{


    public class SQMMailRMgmt
    {
        protected string _VendorCode;
        protected string _ERP_VNAME;
        protected string _PlantCode;
        protected string _PLANT_NAME;
        protected string _SourcerGUID;
        protected string _SourcerName;
        protected string _SQEGUID;
        protected string _SQENAME;
        protected string _Email;




        public string VendorCode { get { return this._VendorCode; } set { this._VendorCode = value; } }
        public string ERP_VNAME { get { return this._ERP_VNAME; } set { this._ERP_VNAME = value; } }
        public string PlantCode { get { return this._PlantCode; } set { this._PlantCode = value; } }
        public string PLANT_NAME { get { return this._PLANT_NAME; } set { this._PLANT_NAME = value; } }
        public string SourcerGUID { get { return this._SourcerGUID; } set { this._SourcerGUID = value; } }
        public string SourcerName { get { return this._SourcerName; } set { this._SourcerName = value; } }
        public string SQEGUID { get { return this._SQEGUID; } set { this._SQEGUID = value; } }
        public string SQENAME { get { return this._SQENAME; } set { this._SQENAME = value; } }
        public string Email { get { return this._Email; } set { this._Email = value; } }

        public SQMMailRMgmt() { }

        public SQMMailRMgmt(string VendorCode, string PlantCode, string SourcerGUID, string SourcerName, string Email)
        {
            this._VendorCode = VendorCode;
            this._PlantCode = PlantCode;
            this._SourcerGUID = SourcerGUID;
            this._SourcerName = SourcerName;
            this._Email = Email;
        }


    }
    public class SQMMailRMgmt_jQGridJSon
    {
        public List<SQMMailRMgmt> Rows = new List<SQMMailRMgmt>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }
    public static class SQMMailRMgmt_Helper
    {

        #region Query Email
        public static string Query_EmailInfo(SqlConnection cn, string Email)
        {
            StringBuilder sb = new StringBuilder();
            string SEmail = StringHelper.EmptyOrUnescapedStringViaUrlDecode(Email);
            sb.Clear();
            sb.Append(@"
               select distinct Name,Email from [TB_EB_USER] 
 where len(Email) > 0          ");

            if (!SEmail.Equals(string.Empty))
            {
                sb.Append(" and  Email like '%' + @Email + '%' ");
            }

            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.AddWithValue("@Email", Email.Trim());

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }
        #endregion

        #region Query SBU Vendor Code
        public static string Query_SBUVendorCodeInfo(SqlConnection cn, PortalUserProfile RunAsUser, string ERP_VND)
        {
            bool bIsWithCondition = false;
            string SVEN = StringHelper.EmptyOrUnescapedStringViaUrlDecode(ERP_VND);
            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.Append(@"
SELECT DISTINCT ERP_VND,ERP_VNAME 
                        FROM TB_VMI_VENDOR_DETAIL
                        ");
            if (!SVEN.Equals(string.Empty))
            {
                sb.Append(" where  ERP_VND like '%' + @ERP_VND + '%' ");
            }
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.AddWithValue("@ERP_VND", ERP_VND.Trim());

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }
        #endregion

        #region Query Plant Code
        public static string Query_PlantCodeInfo(SqlConnection cn, string Plant, string MemberGUID)
        {
            bool bIsWithCondition = false;
            #region DecodeParameter
            string sPlant = StringHelper.EmptyOrUnescapedStringViaUrlDecode(Plant);
            #endregion
            string splant = StringHelper.EmptyOrUnescapedStringViaUrlDecode(Plant);

            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.Append(@"
            SELECT DISTINCT PLANT,PLANT_NAME 
            FROM TB_VMI_PLANT 
");
            sb.Append(@" where  PLANT in 
            (SELECT DISTINCT PlantCode
  FROM TB_SQM_Member_Plant
  where MemberGUID =@MemberGUID)");
            if (!splant.Equals(string.Empty))
            {
                sb.Append(" and  Plant like '%' + @Plant + '%' ");

            }
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {

                cmd.Parameters.AddWithValue("@Plant", Plant.Trim());
                cmd.Parameters.AddWithValue("@MemberGUID", MemberGUID);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }
        #endregion

        public static void WriteLog(string msg, string Path, bool errflag)
        {
            if (!errflag)
            {
                msg = "[     ] " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss ") + msg;
            }
            else
            {
                msg = "[ERROR] " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss ") + msg;
            }

            string logFile = Path + @"LOG\" + DateTime.Today.ToString("yyyyMMdd") + ".txt";
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
            if (!File.Exists(logFile))
            {
                StreamWriter sw = File.CreateText(logFile);
                try
                {
                    sw.WriteLine(msg);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    sw.Flush();
                    sw.Close();
                }
            }
            else
            {
                StreamWriter sw = File.AppendText(logFile);
                try
                {
                    sw.WriteLine(msg);
                }
                catch (Exception)
                {
                }
                finally
                {
                    sw.Flush();
                    sw.Close();
                }
            }
            //WriteLog2Server(msg, errflag);
        }

        #region Send Mail
        public static string SendMailSQE(SqlConnection cn, string PlantCode, string VendorCode, string Email, string LoginUserGUID, String urlPre, string Path)
        {
            string sMailTo = string.Empty;
            int c = 0;
            StringBuilder sb = new StringBuilder();


            string sSQL = "Select Count(*) From TB_SQM_Vendor_Related  Where PlantCode = @PlantCode And VendorCode = @VendorCode;";
            WriteLog(sSQL, Path, false);
            WriteLog("PlantCode:" + PlantCode + ",VendorCode:" + VendorCode, Path, false);
            using (SqlCommand cmd = new SqlCommand(sSQL, cn))
            {
                cmd.Parameters.Add(new SqlParameter("@PlantCode", StringHelper.NullOrEmptyStringIsDBNull(PlantCode)));
                cmd.Parameters.Add(new SqlParameter("@VendorCode", StringHelper.NullOrEmptyStringIsDBNull(VendorCode)));
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                    c = (int)dr[0];
                dr.Close();
                dr = null;
            }
            if (c > 0)
            {
                return "此賬號已存在！";
            }
            else
            {

                #region insert TB_VENDO_Related
                sb.Clear();
                sb.Append(@"
                        INSERT INTO TB_SQM_Vendor_Related (PlantCode,VendorCode,SourcerGUID) 
                        values(@PlantCode,@VendorCode,@LoginUserGUID)
                    ");
                SqlCommand cmdP = new SqlCommand(sb.ToString(), cn);
                cmdP.Parameters.AddWithValue("@PlantCode", StringHelper.NullOrEmptyStringIsDBNull(PlantCode));
                cmdP.Parameters.AddWithValue("@VendorCode", StringHelper.NullOrEmptyStringIsDBNull(VendorCode));
                cmdP.Parameters.AddWithValue("@LoginUserGUID", StringHelper.NullOrEmptyStringIsDBNull(LoginUserGUID));
                cmdP.ExecuteNonQuery();
                #endregion
                WriteLog(sb.ToString(), Path, false);
                WriteLog("PlantCode:" + PlantCode + ",VendorCode:" + VendorCode + ",LoginUserGUID" + LoginUserGUID, Path, false);



                #region get CORP_VND
                string CORP_VND = string.Empty;
                sb.Clear();
                sb.Append(@"
                        SELECT  VendorCode,CORP_VND,CORP_VNAME,LFURL
                         from TB_VMI_VENDOR_DETAIL,
                         TB_SQM_Vendor_Related
                        WHERE TB_VMI_VENDOR_DETAIL.ERP_VND = TB_SQM_Vendor_Related.VendorCode
                        AND VendorCode =@VendorCode 
");
                DataTable dt1 = new DataTable();
                using (SqlCommand cmdt = new SqlCommand(sb.ToString(), cn))
                {

                    cmdt.Parameters.Add(new SqlParameter("@VendorCode", StringHelper.NullOrEmptyStringIsDBNull(VendorCode)));
                    using (SqlDataReader dr = cmdt.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            CORP_VND = StringHelper.NullOrEmptyStringIsEmptyString(dr["CORP_VND"]).Trim();
                        }
                    }
                }
                #endregion

                WriteLog(sb.ToString(), Path, false);
                WriteLog("VendorCode:" + VendorCode + ",CORP_VND:" + CORP_VND, Path, false);


                #region get sMailTo
                //                sb.Clear();
                //                sb.Append(@"
                //            SELECT TB_SQM_Vendor_Related.VendorCode,
                //                   TB_SQM_Vendor_Related.PlantCode,
                //				   TB_SQM_Vendor_Related.SourcerGUID,
                //				   U1.NAME as SourcerName,
                //				   U1.EMAIL as Email
                //                   FROM 
                //			       TB_EB_USER U1,
                //				   TB_SQM_Vendor_Related
                //            WHERE 
                //			     U1.USER_GUID = Convert(nvarchar(50),TB_SQM_Vendor_Related.SourcerGUID)
                //				and PlantCode = @PlantCode and VendorCode = @VendorCode 
                //");

                //                using (SqlCommand cmda = new SqlCommand(sb.ToString(), cn))
                //                {

                //                    cmda.Parameters.Add(new SqlParameter("@PlantCode", StringHelper.NullOrEmptyStringIsDBNull(PlantCode)));
                //                    cmda.Parameters.Add(new SqlParameter("@VendorCode", StringHelper.NullOrEmptyStringIsDBNull(VendorCode)));
                //                    //cmd.Parameters.Add(new SqlParameter("@Email", StringHelper.NullOrEmptyStringIsDBNull(Email)));
                //                    using (SqlDataReader dr = cmda.ExecuteReader())
                //                    {
                //                        while (dr.Read())
                //                        {
                //                            sMailTo = StringHelper.NullOrEmptyStringIsEmptyString(dr["Email"]).Trim();
                //                        }
                //                    }
                //                }
                #endregion

                #region get AuthorizedID
                //                sb.Clear();
                //                sb.Append(@"
                //            SELECT [AccountID]

                //  FROM [LiteOnRFQTraining].[dbo].[PORTAL_Members]
                //  where MemberGUID=@MemberGUID
                //");
                //                string AuthorizedID = string.Empty;
                //                using (SqlCommand cmda = new SqlCommand(sb.ToString(), cn))
                //                {

                //                    cmda.Parameters.Add(new SqlParameter("@MemberGUID", StringHelper.NullOrEmptyStringIsDBNull(LoginUserGUID)));
                //                    //cmd.Parameters.Add(new SqlParameter("@Email", StringHelper.NullOrEmptyStringIsDBNull(Email)));
                //                    using (SqlDataReader dr = cmda.ExecuteReader())
                //                    {
                //                        while (dr.Read())
                //                        {
                //                            AuthorizedID =@""+ StringHelper.NullOrEmptyStringIsEmptyString(dr["AccountID"]).Trim();
                //                        }
                //                    }
                //                }
                #endregion
                //else
                //{
                //string AuthorizedID = @"liteon\" + LoginUserGUID;
                string NEWUSERID = CORP_VND + "S" + System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 1).ToUpper();
                WriteLog(NEWUSERID, Path, false);
                string strNewPW = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8);

                //                sb.Clear();
                //                sb.Append(@"
                //                            SELECT [AccountID] FROM [PORTAL_Members]
                //                           WHERE AccountID like @AccountID
                //");
                //                DataTable dt2 = new DataTable();
                //                using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
                //                {

                //                    cmd.Parameters.Add(new SqlParameter("@AccountID", StringHelper.NullOrEmptyStringIsDBNull("%" + CORP_VND + "%")));
                //                    using (SqlDataReader dr = cmd.ExecuteReader())
                //                    {
                //                        dt2.Load(dr);
                //                    }
                //                }

                //                if (dt2.Rows.Count > 0)
                //                {
                //                    return "CORP_VND已存在，請檢查后重新輸入！";
                //                }
                //                else
                //                { 

                //調用webservice創建賬號
                int rr;
                string IsTest = ConfigurationManager.AppSettings["IsTest"].ToString();
                WriteLog(IsTest, Path, false);
                try
                {
                    if (IsTest.Equals("false"))
                    {
                        icm131_2.SQM_AccountService sas = new icm131_2.SQM_AccountService();
                        rr = sas.SQM_CreateAccount
                        (
                        @"liteon\jetersun", //AuthorizedID
                        NEWUSERID,       //NewUserID
                         "Aa123456",       //NewUserPassword
                        "2",                //RoleID
                       Email    //eMail
                        );
                    }
                    else
                    {
                        dev281.SQM_AccountService sas = new dev281.SQM_AccountService();
                        rr = sas.SQM_CreateAccount
                        (
                        @"liteon\jetersun", //AuthorizedID
                        NEWUSERID,       //NewUserID
                         "Aa123456",       //NewUserPassword
                        "2",                //RoleID
                       Email    //eMail
                        );
                    }
                }
                catch (Exception)
                {
                    sb.Clear();
                    sb.Append(@"
                       Delete TB_SQM_Vendor_Related 
                       where PlantCode=@PlantCode and VendorCode=@VendorCode
                    ");
                    SqlCommand sqlcmd = new SqlCommand(sb.ToString(), cn);
                    sqlcmd.Parameters.AddWithValue("@PlantCode", StringHelper.NullOrEmptyStringIsDBNull(PlantCode));
                    sqlcmd.Parameters.AddWithValue("@VendorCode", StringHelper.NullOrEmptyStringIsDBNull(VendorCode));
                    sqlcmd.ExecuteNonQuery();
                    return "失败";
                    throw;
                }



                WriteLog(rr.ToString(), Path, false);

                if (rr == 0)
                {

                    //發郵件
                    icm045.CMSHandler MS = new icm045.CMSHandler();
                    MS.MailSend("SupplierPortal",
                                  "SupplierPortal@liteon.com",
                                  Email,
                                  //== "" ? "Jerrya.Chen@liteon.com" : sMailTo
                                  "",
                                  "",
                                  "SQM Notice",
                                  string.Format(
                                    "Dear All:<br/>请登录光宝供应商品质管理系统平台，请于供应商基本资料调查评定模块界面中填写相关调查信息，并在10个工作日内完成。此调查信息非常重要，请仔细阅读调查表中的“郑重声明”与“重要提示”栏位的内容，如实完成填写各项调查内容。<br/><br/>USERID:{0}<br/><br/>Password:{1}<br/><br/>网址:<a href='" + urlPre + "'>登錄網址</a>", NEWUSERID, "Aa123456"
                                  ),
                                  icm045.MailPriority.Normal,
                                  icm045.MailFormat.Html,
                                  new string[0]);

                }
                else
                {
                    switch (rr)
                    {
                        case 1:
                            sb.Clear();
                            sb.Append(@"
                        DELETE FROM TB_SQM_Vendor_Related  
                        WHERE PlantCode = @PlantCode AND VendorCode = @VendorCode
                            ");

                            SqlCommand cmdz = new SqlCommand(sb.ToString(), cn);
                            cmdz.Parameters.AddWithValue("@PlantCode", StringHelper.NullOrEmptyStringIsDBNull(PlantCode));
                            cmdz.Parameters.AddWithValue("@VendorCode", StringHelper.NullOrEmptyStringIsDBNull(VendorCode));
                            cmdz.ExecuteNonQuery();
                            WriteLog(sb.ToString(), Path, false);
                            return "傳入參數錯誤";

                        case 2:
                            sb.Clear();
                            sb.Append(@"
                        DELETE FROM TB_SQM_Vendor_Related  WHERE PlantCode = @PlantCode AND VendorCode = @VendorCode
                            ");

                            SqlCommand cmd2 = new SqlCommand(sb.ToString(), cn);
                            cmd2.Parameters.AddWithValue("@PlantCode", StringHelper.NullOrEmptyStringIsDBNull(PlantCode));
                            cmd2.Parameters.AddWithValue("@VendorCode", StringHelper.NullOrEmptyStringIsDBNull(VendorCode));
                            cmd2.ExecuteNonQuery();
                            WriteLog(sb.ToString(), Path, false);
                            return "非授權使用者";

                        case 3:
                            sb.Clear();
                            sb.Append(@"
                        DELETE FROM TB_SQM_Vendor_Related 
                        WHERE PlantCode = @PlantCode AND VendorCode = @VendorCode
                            ");

                            SqlCommand cmd3 = new SqlCommand(sb.ToString(), cn);
                            cmd3.Parameters.AddWithValue("@PlantCode", StringHelper.NullOrEmptyStringIsDBNull(PlantCode));
                            cmd3.Parameters.AddWithValue("@VendorCode", StringHelper.NullOrEmptyStringIsDBNull(VendorCode));
                            cmd3.ExecuteNonQuery();
                            WriteLog(sb.ToString(), Path, false);
                            return "新帳號格式錯誤";

                        case 4:
                            sb.Clear();
                            sb.Append(@"
                        DELETE FROM TB_SQM_Vendor_Related 
                        WHERE PlantCode = @PlantCode AND VendorCode = @VendorCode
                            ");

                            SqlCommand cmd4 = new SqlCommand(sb.ToString(), cn);
                            cmd4.Parameters.AddWithValue("@PlantCode", StringHelper.NullOrEmptyStringIsDBNull(PlantCode));
                            cmd4.Parameters.AddWithValue("@VendorCode", StringHelper.NullOrEmptyStringIsDBNull(VendorCode));
                            cmd4.ExecuteNonQuery();
                            WriteLog(sb.ToString(), Path, false);
                            return "新帳號已存在";


                        case 5:
                            sb.Clear();
                            sb.Append(@"
                        DELETE FROM TB_SQM_Vendor_Related 
                        WHERE PlantCode = @PlantCode AND VendorCode = @VendorCode
                            ");

                            SqlCommand cmd5 = new SqlCommand(sb.ToString(), cn);
                            cmd5.Parameters.AddWithValue("@PlantCode", StringHelper.NullOrEmptyStringIsDBNull(PlantCode));
                            cmd5.Parameters.AddWithValue("@VendorCode", StringHelper.NullOrEmptyStringIsDBNull(VendorCode));
                            cmd5.ExecuteNonQuery();
                            WriteLog(sb.ToString(), Path, false);
                            return "密碼不符規則";

                        case 6:

                            sb.Clear();
                            sb.Append(@"
                        DELETE FROM TB_SQM_Vendor_Related  
                        WHERE PlantCode = @PlantCode AND VendorCode = @VendorCode
                            ");

                            SqlCommand cmd6 = new SqlCommand(sb.ToString(), cn);
                            cmd6.Parameters.AddWithValue("@PlantCode", StringHelper.NullOrEmptyStringIsDBNull(PlantCode));
                            cmd6.Parameters.AddWithValue("@VendorCode", StringHelper.NullOrEmptyStringIsDBNull(VendorCode));
                            cmd6.ExecuteNonQuery();
                            WriteLog(sb.ToString(), Path, false);
                            return "未提供電子郵件信箱或格式錯誤";


                    }
                    //}
                }

                #region Get MemberGUID
                string MG = string.Empty;
                sb.Clear();
                sb.Append(@"
SELECT  [MemberGUID]
      ,[AccountID]
      ,[MemberType]
      ,[StatusCode]
      ,[NameInChinese]
      ,[NameInEnglish]
      ,[PrimaryEmail]
  FROM [PORTAL_Members] with (Nolock)
  where AccountID = @AccountID");
                DataTable dt3 = new DataTable();
                using (SqlCommand cmd1 = new SqlCommand(sb.ToString(), cn))
                {

                    cmd1.Parameters.Add(new SqlParameter("@AccountID", StringHelper.NullOrEmptyStringIsDBNull(NEWUSERID)));
                    using (SqlDataReader dr = cmd1.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            MG = StringHelper.NullOrEmptyStringIsEmptyString(dr["MemberGUID"]).Trim();
                        }
                    }
                }
                WriteLog(sb.ToString(), Path, false);
                WriteLog("MG:" + MG, Path, false);
                #endregion

                #region INSERT MemberGUID
                sb.Clear();
                sb.Append(@"
                          INSERT INTO [TB_SQM_Member_Vendor_Map] (MemberGUID,PlantCode,VendorCode)
                          values(@MemberGUID,@PlantCode,@VendorCode)
                            ");

                SqlCommand cmd = new SqlCommand(sb.ToString(), cn);
                cmd.Parameters.AddWithValue("@PlantCode", StringHelper.NullOrEmptyStringIsDBNull(PlantCode));
                cmd.Parameters.AddWithValue("@VendorCode", StringHelper.NullOrEmptyStringIsDBNull(VendorCode));
                cmd.Parameters.AddWithValue("@MemberGUID", StringHelper.NullOrEmptyStringIsDBNull(MG));
                cmd.ExecuteNonQuery();
                #endregion
                WriteLog(sb.ToString(), Path, false);

                //}
                return "成功";
            }
            #endregion




            #region Check BackUP
            //            #region Check 
            //            sb.Clear();
            //            sb.Append(@"
            //SELECT TB_SQM_Vendor_Related.VendorCode,
            //                   TB_SQM_Vendor_Related.PlantCode,
            //				   TB_SQM_Vendor_Related.SourcerGUID,
            //				   U1.NAME as SourcerName,
            //				   U1.EMAIL as Email
            //                   FROM
            //                   TB_EB_USER U1,
            //				   TB_SQM_Vendor_Related
            //            WHERE

            //                 U1.USER_GUID = Convert(nvarchar(50), TB_SQM_Vendor_Related.SourcerGUID)

            //                and TB_SQM_Vendor_Related.PlantCode = @PlantCode
            //                and TB_SQM_Vendor_Related.VendorCode = @VendorCode
            //                and U1.Email = @Email
            //");

            //            DataTable dt = new DataTable();
            //            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            //            {

            //                cmd.Parameters.Add(new SqlParameter("@PlantCode", StringHelper.NullOrEmptyStringIsDBNull(PlantCode)));
            //                cmd.Parameters.Add(new SqlParameter("@VendorCode", StringHelper.NullOrEmptyStringIsDBNull(VendorCode)));
            //                cmd.Parameters.Add(new SqlParameter("@Email", StringHelper.NullOrEmptyStringIsDBNull(Email)));
            //                using (SqlDataReader dr = cmd.ExecuteReader())
            //                {
            //                    dt.Load(dr);
            //                }
            //            }

            //            if (dt.Rows.Count == 0)
            //            {
            //                return "";
            //            }
            //            #endregion
            #endregion
        }
    }
}

