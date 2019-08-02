using Lib_SQM_Domain.Modal;
using Lib_SQM_Domain.SharedLibs;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Lib_SQM_Domain.Model
{
    public class Hold
    {
        private string _SID;
        private string _ReportType;
        private string _Dept;
        private string _HoldNo;
        private string _CreatTime;
        private string _vender;
        private string _LitNo;
        private string _Size;
        private string _Period;
        private string _Batch;
        private string _UpTime;
        private string _EndTime;
        private string _Reason;
        private string _Principal1;
        private int _NTSNum1;
        private string _NTSTime1;
        private string _Auditer1;
        private string _Principal2;
        private int _NTSNum2;
        private string _NTSTime2;
        private string _Auditer2;
        private string _Principal3;
        private int _NTSNum3;
        private string _NTSTime3;
        private string _Auditer3;
        private string _Principal4;
        private int _NTSNum4;
        private string _NTSTime4;
        private string _Auditer4;
        private string _Principal5;
        private int _NTSNum5;
        private string _NTSTime5;
        private string _Auditer5;
        private string _Principal6;
        private int _NTSNum6;
        private string _NTSTime6;
        private string _Auditer6;
        private string _Principal7;
        private int _NTSNum7;
        private string _NTSTime7;
        private string _Auditer7;
        private string _Principal8;
        private int _NTSNum8;
        private string _NTSTime8;
        private string _Auditer8;
        private string _Principal9;
        private int _NTSNum9;
        private string _NTSTime9;
        private string _Auditer9;
        private string _Principal10;
        private int _NTSNum10;
        private string _NTSTime10;
        private string _Auditer10;
        private string _Principal11;
        private int _NTSNum11;
        private string _NTSTime11;
        private string _Auditer11;

        public string SID { get { return this._SID; } set { this._SID = value; } }
        public string ReportType { get { return this._ReportType; } set { this._ReportType = value; } }
        public string Dept { get { return this._Dept; } set { this._Dept = value; } }
        public string HoldNo { get { return this._HoldNo; } set { this._HoldNo = value; } }
        public string CreatTime { get { return this._CreatTime; } set { this._CreatTime = value; } }
        public string vender { get { return this._vender; } set { this._vender = value; } }
        public string LitNo { get { return this._LitNo; } set { this._LitNo = value; } }
        public string Size { get { return this._Size; } set { this._Size = value; } }
        public string Period { get { return this._Period; } set { this._Period = value; } }
        public string Batch { get { return this._Batch; } set { this._Batch = value; } }
        public string UpTime { get { return this._UpTime; } set { this._UpTime = value; } }
        public string EndTime { get { return this._EndTime; } set { this._EndTime = value; } }
        public string Reason { get { return this._Reason; } set { this._Reason = value; } }
        public string Principal1 { get { return this._Principal1; } set { this._Principal1 = value; } }
        public int NTSNum1 { get { return this._NTSNum1; } set { this._NTSNum1 = value; } }
        public string NTSTime1 { get { return this._NTSTime1; } set { this._NTSTime1 = value; } }
        public string Auditer1 { get { return this._Auditer1; } set { this._Auditer1 = value; } }
        public string Principal2 { get { return this._Principal2; } set { this._Principal2 = value; } }
        public int NTSNum2 { get { return this._NTSNum2; } set { this._NTSNum2 = value; } }
        public string NTSTime2 { get { return this._NTSTime2; } set { this._NTSTime2 = value; } }
        public string Auditer2 { get { return this._Auditer2; } set { this._Auditer2 = value; } }
        public string Principal3 { get { return this._Principal3; } set { this._Principal3 = value; } }
        public int NTSNum3 { get { return this._NTSNum3; } set { this._NTSNum3 = value; } }
        public string NTSTime3 { get { return this._NTSTime3; } set { this._NTSTime3 = value; } }
        public string Auditer3 { get { return this._Auditer3; } set { this._Auditer3 = value; } }
        public string Principal4 { get { return this._Principal4; } set { this._Principal4 = value; } }
        public int NTSNum4 { get { return this._NTSNum4; } set { this._NTSNum4 = value; } }
        public string NTSTime4 { get { return this._NTSTime4; } set { this._NTSTime4 = value; } }
        public string Auditer4 { get { return this._Auditer4; } set { this._Auditer4 = value; } }
        public string Principal5 { get { return this._Principal5; } set { this._Principal5 = value; } }
        public int NTSNum5 { get { return this._NTSNum5; } set { this._NTSNum5 = value; } }
        public string NTSTime5 { get { return this._NTSTime5; } set { this._NTSTime5 = value; } }
        public string Auditer5 { get { return this._Auditer5; } set { this._Auditer5 = value; } }
        public string Principal6 { get { return this._Principal6; } set { this._Principal6 = value; } }
        public int NTSNum6 { get { return this._NTSNum6; } set { this._NTSNum6 = value; } }
        public string NTSTime6 { get { return this._NTSTime6; } set { this._NTSTime6 = value; } }
        public string Auditer6 { get { return this._Auditer6; } set { this._Auditer6 = value; } }
        public string Principal7 { get { return this._Principal7; } set { this._Principal7 = value; } }
        public int NTSNum7 { get { return this._NTSNum7; } set { this._NTSNum7 = value; } }
        public string NTSTime7 { get { return this._NTSTime7; } set { this._NTSTime7 = value; } }
        public string Auditer7 { get { return this._Auditer7; } set { this._Auditer7 = value; } }
        public string Principal8 { get { return this._Principal8; } set { this._Principal8 = value; } }
        public int NTSNum8 { get { return this._NTSNum8; } set { this._NTSNum8 = value; } }
        public string NTSTime8 { get { return this._NTSTime8; } set { this._NTSTime8 = value; } }
        public string Auditer8 { get { return this._Auditer8; } set { this._Auditer8 = value; } }
        public string Principal9 { get { return this._Principal9; } set { this._Principal9 = value; } }
        public int NTSNum9 { get { return this._NTSNum9; } set { this._NTSNum9 = value; } }
        public string NTSTime9 { get { return this._NTSTime9; } set { this._NTSTime9 = value; } }
        public string Auditer9 { get { return this._Auditer9; } set { this._Auditer9 = value; } }
        public string Principal10 { get { return this._Principal10; } set { this._Principal10 = value; } }
        public int NTSNum10 { get { return this._NTSNum10; } set { this._NTSNum10 = value; } }
        public string NTSTime10 { get { return this._NTSTime10; } set { this._NTSTime10 = value; } }
        public string Auditer10 { get { return this._Auditer10; } set { this._Auditer10 = value; } }
        public string Principal11 { get { return this._Principal11; } set { this._Principal11 = value; } }
        public int NTSNum11 { get { return this._NTSNum11; } set { this._NTSNum11 = value; } }
        public string NTSTime11 { get { return this._NTSTime11; } set { this._NTSTime11 = value; } }
        public string Auditer11 { get { return this._Auditer11; } set { this._Auditer11 = value; } }

        public Hold()
        {

        }
        public Hold(string SID,
            string ReportType,
            string Dept,
            string HoldNo,
            string CreatTime,
            string vender,
            string LitNo,
            string Size,
            string Period,
            string Batch,
            string UpTime,
            string EndTime,
            string Reason,
            string Principal1,
            int NTSNum1,
            string NTSTime1,
            string Auditer1,
            string Principal2,
            int NTSNum2,
            string NTSTime2,
            string Auditer2,
            string Principal3,
            int NTSNum3,
            string NTSTime3,
            string Auditer3,
            string Principal4,
            int NTSNum4,
            string NTSTime4,
            string Auditer4,
            string Principal5,
            int NTSNum5,
            string NTSTime5,
            string Auditer5,
            string Principal6,
            int NTSNum6,
            string NTSTime6,
            string Auditer6,
            string Principal7,
            int NTSNum7,
            string NTSTime7,
            string Auditer7,
            string Principal8,
            int NTSNum8,
            string NTSTime8,
            string Auditer8,
            string Principal9,
            int NTSNum9,
            string NTSTime9,
            string Auditer9,
            string Principal10,
            int NTSNum10,
            string NTSTime10,
            string Auditer10,
            string Principal11,
            int NTSNum11,
            string NTSTime11,
            string Auditer11
            )
        {
            this._SID = SID;
            this._ReportType = ReportType;
            this._Dept = Dept;
            this._HoldNo = HoldNo;
            this._CreatTime = CreatTime;
            this._vender = vender;
            this._LitNo = LitNo;
            this._Size = Size;
            this._Period = Period;
            this._Batch = Batch;
            this._UpTime = UpTime;
            this._EndTime = EndTime;
            this._Reason = Reason;
            this._Principal1 = Principal1;
            this._NTSNum1 = NTSNum1;
            this._NTSTime1 = NTSTime1;
            this._Auditer1 = Auditer1;
            this._Principal2 = Principal2;
            this._NTSNum2 = NTSNum2;
            this._NTSTime2 = NTSTime2;
            this._Auditer2 = Auditer2;
            this._Principal3 = Principal3;
            this._NTSNum3 = NTSNum3;
            this._NTSTime3 = NTSTime3;
            this._Auditer3 = Auditer3;
            this._Principal4 = Principal4;
            this._NTSNum4 = NTSNum4;
            this._NTSTime4 = NTSTime4;
            this._Auditer4 = Auditer4;
            this._Principal5 = Principal5;
            this._NTSNum5 = NTSNum5;
            this._NTSTime5 = NTSTime5;
            this._Auditer5 = Auditer5;
            this._Principal6 = Principal6;
            this._NTSNum6 = NTSNum6;
            this._NTSTime6 = NTSTime6;
            this._Auditer6 = Auditer6;
            this._Principal7 = Principal7;
            this._NTSNum7 = NTSNum7;
            this._NTSTime7 = NTSTime7;
            this._Auditer7 = Auditer7;
            this._Principal8 = Principal8;
            this._NTSNum8 = NTSNum8;
            this._NTSTime8 = NTSTime8;
            this._Auditer8 = Auditer8;
            this._Principal9 = Principal9;
            this._NTSNum9 = NTSNum9;
            this._NTSTime9 = NTSTime9;
            this._Auditer9 = Auditer9;
            this._Principal10 = Principal10;
            this._NTSNum10 = NTSNum10;
            this._NTSTime10 = NTSTime10;
            this._Auditer10 = Auditer10;
            this._Principal11 = Principal11;
            this._NTSNum11 = NTSNum11;
            this._NTSTime11 = NTSTime11;
            this._Auditer11 = Auditer11;
        }
    }

    public class HoldData
    {
        private string _SID;
        private string _HoldNo;
        private string _CreatTime;
        private string _vender;
        private string _LitNo;
        private string _Size;
        private string _Period;
        private string _Quantity;
        private string _HoldDays;
        private string _Owner;
        private string _Status;
        private string _ReleaseQuantity;
        private string _RejectQuantity;
        public string SID { get { return this._SID; } set { this._SID = value; } }
        public string HoldNo { get { return this._HoldNo; } set { this._HoldNo = value; } } 
        public string CreatTime { get { return this._CreatTime; } set { this._CreatTime = value; } }
        public string vender { get { return this._vender; } set { this._vender = value; } }
        public string LitNo { get { return this._LitNo; } set { this._LitNo = value; } }
        public string Size { get { return this._Size; } set { this._Size = value; } }
        public string Period { get { return this._Period; } set { this._Period = value; } }
        public string Quantity { get { return this._Quantity; } set { this._Quantity = value; } }
        public string HoldDays { get { return this._HoldDays; } set { this._HoldDays = value; } }
        public string Owner { get { return this._Owner; } set { this._Owner = value; } }
        public string Status { get { return this._Status; } set { this._Status = value; } }
        public string ReleaseQuantity { get { return this._ReleaseQuantity; } set { this._ReleaseQuantity = value; } }
        public string RejectQuantity { get { return this._RejectQuantity; } set { this._RejectQuantity = value; } }
        public HoldData()
        { }
        public HoldData(string SID
            ,string HoldNo
             , string CreatTime
             , string vender
             , string LitNo
             , string Size
             , string Period
             , string Quantity
             , string HoldDays
             , string Owner
             , string Status
             , string ReleaseQuantity
             , string RejectQuantity
              )
        {
            this._SID = SID;
            this._HoldNo = HoldNo;
            this._CreatTime = CreatTime;
            this._vender = vender;
            this._LitNo = LitNo;
            this._Size = Size;
            this._Period = Period;
            this._Quantity = Quantity;
            this._HoldDays = HoldDays;
            this._Owner = Owner;
            this._Status = Status;
            this._ReleaseQuantity = ReleaseQuantity;
            this._RejectQuantity = RejectQuantity;
        }
    }

    public static class Hold_Helper
    {
        public static string GetDataToJQGridJson(SqlConnection cn, string MemberGUID)
        {
            return GetDataToJQGridJson(cn, "", MemberGUID);
        }
        public static string GetDataToJQGridJson(SqlConnection cn, string SearchText, string MemberGUID)
        {
            Hold_jQGridJSon m = new Hold_jQGridJSon();
            string urlPre = CommonHelper.urlPre;
            string sSearchText = SearchText.Trim();
            string plant = GetPlant(cn,MemberGUID);
            string status = string.Empty;
            string sWhereClause = "";
            if (sSearchText != "")
                sWhereClause += " and HoldNo like '%' + @SearchText + '%'";
            m.Rows.Clear();
            int iRowCount = 0;
            StringBuilder sSQL = new StringBuilder();
            sSQL.Append(@"SELECT  [SID],HoldNo
      ,[InsertTime]
      ,[vender]
      ,[LitNo]
      ,[Size]
      ,[Period]
      , (NTSNum1+NTSNum2+NTSNum3+NTSNum4+NTSNum5+NTSNum6+NTSNum7+NTSNum8+NTSNum9+NTSNum10+NTSNum11) as Quantity
      , DATEDIFF(DAY,InsertTime,GETDATE()) as HoldDays
	   ,[NameInChinese] as Owner
	  ,[status]
	  ,ReleaseQuantity
	  ,RejectQuantity	 
  FROM [TB_SQM_HOLD_REPORT]
  inner join [dbo].[PORTAL_Members] on  CONVERT( NVARCHAR(50), [MemberGUID])=creatName where Plant=@Plant and CreatName=@CreatName");

            string ssSQL = sSQL.ToString() + sWhereClause + ";";
            using (SqlCommand cmd = new SqlCommand(ssSQL, cn))
            {
                if (sSearchText != "")
                    cmd.Parameters.Add(new SqlParameter("@SearchText", sSearchText));

                cmd.Parameters.Add(new SqlParameter("@Plant", plant));
                cmd.Parameters.Add(new SqlParameter("@CreatName", MemberGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        switch (dr["status"].ToString().Trim().ToUpper())
                        {
                            case "TRUE":
                                status = "审核通过";
                                break;
                            case "FALSE":
                                status = "审核未通过";
                                break;
                            default:
                                status = "";
                                break;
                        }

                        iRowCount++;
                        m.Rows.Add(new HoldData(
                            dr["SID"].ToString(),
                            dr["HoldNo"].ToString(),
                            dr["InsertTime"].ToString(),
                            dr["vender"].ToString(),
                            dr["LitNo"].ToString(),
                            dr["Size"].ToString(),
                            dr["Period"].ToString(),
                            dr["Quantity"].ToString(),
                            dr["HoldDays"].ToString(),
                            dr["Owner"].ToString(),
                            status,
                            dr["ReleaseQuantity"].ToString(),
                            dr["RejectQuantity"].ToString()


                           ));
                    }
                }

            }
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(m);
        }

        public static string GetSQEDataToJQGridJson(SqlConnection cn, string MemberGUID)
        {
            return GetSQEDataToJQGridJson(cn, "", MemberGUID);
        }
        public static string GetSQEDataToJQGridJson(SqlConnection cn, string SearchText, string MemberGUID)
        {
            Hold_jQGridJSon m = new Hold_jQGridJSon();
            string urlPre = CommonHelper.urlPre;
            string sSearchText = SearchText.Trim();
            string plant = GetPlant(cn, MemberGUID);
            string status = string.Empty;
            string sWhereClause = "";
            if (sSearchText != "")
                sWhereClause += " and HoldNo like '%' + @SearchText + '%'";
            m.Rows.Clear();
            int iRowCount = 0;
            StringBuilder sSQL = new StringBuilder();
            sSQL.Append(@"SELECT  [SID],HoldNo
      ,[InsertTime]
      ,[vender]
      ,[LitNo]
      ,[Size]
      ,[Period]
      , (NTSNum1+NTSNum2+NTSNum3+NTSNum4+NTSNum5+NTSNum6+NTSNum7+NTSNum8+NTSNum9+NTSNum10+NTSNum11) as Quantity
      , DATEDIFF(DAY,InsertTime,GETDATE()) as HoldDays
	   ,[NameInChinese] as Owner
	  ,[status]
	  ,ReleaseQuantity
	  ,RejectQuantity	 
  FROM [TB_SQM_HOLD_REPORT]
  inner join [dbo].[PORTAL_Members] on  CONVERT( NVARCHAR(50), [MemberGUID])=creatName where Plant=@Plant ");

            string ssSQL = sSQL.ToString() + sWhereClause + ";";
            using (SqlCommand cmd = new SqlCommand(ssSQL, cn))
            {
                if (sSearchText != "")
                    cmd.Parameters.Add(new SqlParameter("@SearchText", sSearchText));

                cmd.Parameters.Add(new SqlParameter("@Plant", plant));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        switch (dr["status"].ToString().Trim().ToUpper())
                        {
                            case "TRUE":
                                status = "审核通过";
                                break;
                            case "FALSE":
                                status = "审核未通过";
                                break;
                            default:
                                status = "";
                                break;
                        }

                        iRowCount++;
                        m.Rows.Add(new HoldData(
                            dr["SID"].ToString(),
                            dr["HoldNo"].ToString(),
                            dr["InsertTime"].ToString(),
                            dr["vender"].ToString(),
                            dr["LitNo"].ToString(),
                            dr["Size"].ToString(),
                            dr["Period"].ToString(),
                            dr["Quantity"].ToString(),
                            dr["HoldDays"].ToString(),
                            dr["Owner"].ToString(),
                            status,
                            dr["ReleaseQuantity"].ToString(),
                            dr["RejectQuantity"].ToString()


                           ));
                    }
                }

            }
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(m);
        }

        public static string CreateDataItem(SqlConnection cnPortal, Hold DataItem, string MemberGUID, string localPath, string urlPre)
        {
      
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            string HoldNo = Assignment(cnPortal);
            ArrayList list = new ArrayList();
            list.Add(DataItem.Principal1.ToUpper());
            list.Add(DataItem.Principal2.ToUpper());
            list.Add(DataItem.Principal3.ToUpper());
            list.Add(DataItem.Principal4.ToUpper());
            list.Add(DataItem.Principal5.ToUpper());
            list.Add(DataItem.Principal6.ToUpper());
            list.Add(DataItem.Principal7.ToUpper());
            list.Add(DataItem.Principal8.ToUpper());
            list.Add(DataItem.Principal9.ToUpper());
            list.Add(DataItem.Principal10.ToUpper());
            list.Add(DataItem.Principal11.ToUpper());
            if (r != "")
            {
                return r;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"INSERT INTO TB_SQM_HOLD_REPORT
           (HoldNo
           ,Plant
           ,CreatName
           ,InsertTime
           ,ReportType
           ,Dept
           ,vender
           ,LitNo
           ,Size
           ,Period
           ,batch
           ,UpTime
           ,EndTime
           ,Reason
        )
     VALUES
           (
           @HoldNo
           ,@Plant
           ,@CreatName
           ,@InsertTime
           ,@ReportType
           ,@Dept
           ,@vender
           ,@LitNo
           ,@Size
           ,@Period
           ,@batch
           ,@UpTime
           ,@EndTime
           ,@Reason
           ) 
    ");
                SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal);
                cmd.Parameters.AddWithValue("@HoldNo", SQMStringHelper.NullOrEmptyStringIsDBNull(HoldNo));
                cmd.Parameters.AddWithValue("@Plant", SQMStringHelper.NullOrEmptyStringIsDBNull(GetPlant(cnPortal, MemberGUID)));
                cmd.Parameters.AddWithValue("@CreatName", SQMStringHelper.NullOrEmptyStringIsDBNull(MemberGUID));
                cmd.Parameters.AddWithValue("@InsertTime", SQMStringHelper.NullOrEmptyStringIsDBNull(DateTime.Now.ToString()));
                cmd.Parameters.AddWithValue("@ReportType", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ReportType));
                cmd.Parameters.AddWithValue("@Dept", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Dept));
                cmd.Parameters.AddWithValue("@vender", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.vender));
                cmd.Parameters.AddWithValue("@LitNo", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.LitNo));
                cmd.Parameters.AddWithValue("@Size", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Size));
                cmd.Parameters.AddWithValue("@Period", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Period));
                cmd.Parameters.AddWithValue("@batch", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Batch));
                cmd.Parameters.AddWithValue("@UpTime", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.UpTime));
                cmd.Parameters.AddWithValue("@EndTime", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.EndTime));
                cmd.Parameters.AddWithValue("@Reason", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Reason));
              
              
                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;
                if (string.IsNullOrEmpty(sErrMsg))
                {
                    string Email = string.Empty;
                    foreach (string item in list)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            Email += getEmailbyName(cnPortal, GetPlant(cnPortal, MemberGUID), item) + ";";
                        }
                        
                    }
                    string sbBody = "HOLD编号" + HoldNo + "需审核，请登录系统进行审核";
                    sendMail(Email, sbBody);
                }


                return sErrMsg;
            }
        }

        private static string getEmailbyName(SqlConnection cn, string Plant,string Name)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select PrimaryEmail from PORTAL_Members where UPPER(NameInEnglish)=@Name");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.AddWithValue("@Name", SQMStringHelper.NullOrEmptyStringIsDBNull(Name));
               
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

        private static string getEmail(SqlConnection cn,string Plant)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"Select Email  FROM [dbo].[TB_SQM_MRB_APPOVE_MAP] T1
  inner join[dbo].[TB_SQM_MRB_APPOVER] T2 On T1.SSID = T2.SID
  WHERE T2.[DepartmentType] = 'OA' and Plant=@Plant");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
               
                cmd.Parameters.AddWithValue("@Plant", SQMStringHelper.NullOrEmptyStringIsDBNull(Plant));
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

        private static void sendMail(string Email,string sbBody)
        {
            string sFROM = "SQM@liteon.com.tw";
            String[] aryFile = new string[0];
            icm045.CMSHandler MS = new icm045.CMSHandler();
            String result = MS.MailSend("SupplierPortal",
                    sFROM,//"SupplierPortal@liteon.com"
                     Email,//sTO toDO
                    "Aiden.Zeng@liteon.com;",//toDOJerryA.Chen@liteon.com;Aiden.Zeng@liteon.com;lily.guo@liteon.com
                    "",
                    "HOLD",//"SQM系統簽核"
                    sbBody,
                    icm045.MailPriority.Normal,
                    icm045.MailFormat.Html,
                    aryFile);//fileName string[0]

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
        private static string GetMailByMemberGUID(SqlConnection cn, string MemberGUID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select PrimaryEmail from PORTAL_Members where MemberGUID=@MemberGUID");
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
        public static string GetHoldData(SqlConnection cn, string SID)
        {
            string urlPre = CommonHelper.urlPre;
            StringBuilder sb = new StringBuilder();
            sb.Append(
            @"
              select * FROM TB_SQM_HOLD_REPORT
              where SID=@SID
            ");

            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@SID", SID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }
        public static DataTable GetHoldbySID(SqlConnection cn, string SID)
        {
            string urlPre = CommonHelper.urlPre;
            StringBuilder sb = new StringBuilder();
            sb.Append(
            @"
              select * FROM TB_SQM_HOLD_REPORT
              where SID=@SID
            ");

            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@SID", SID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return dt;
        }
        public static string DeleteDataItem(SqlConnection cnPortal, Hold DataItem)
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
        private static string DeleteDataItemSub(SqlCommand cmd, Hold DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
DELETE TB_SQM_HOLD_REPORT WHERE SID=@SID;
");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@SID", DataItem.SID);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }

        private static void UnescapeDataFromWeb(Hold DataItem)
        {

            DataItem.SID = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.SID);
            DataItem.ReportType = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ReportType);
            DataItem.Dept = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Dept);
            DataItem.HoldNo = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.HoldNo);
            DataItem.CreatTime = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CreatTime);
            DataItem.vender = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.vender);
            DataItem.LitNo = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.LitNo);
            DataItem.Size = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Size);
            DataItem.Period = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Period);
            DataItem.Batch = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Batch);
            DataItem.UpTime = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.UpTime);
            DataItem.EndTime = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.EndTime);
            DataItem.Reason = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Reason);
            DataItem.Principal1 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Principal1);
            DataItem.NTSTime1 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.NTSTime1);
            DataItem.Auditer1 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Auditer1);
            DataItem.Principal2 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Principal2);
           
            DataItem.NTSTime2 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.NTSTime2);
            DataItem.Auditer2 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Auditer2);
            DataItem.Principal3 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Principal3);
           
            DataItem.NTSTime3 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.NTSTime3);
            DataItem.Auditer3 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Auditer3);
            DataItem.Principal4 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Principal4);
            
            DataItem.NTSTime4 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.NTSTime4);
            DataItem.Auditer4 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Auditer4);
            DataItem.Principal5 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Principal5);
          
            DataItem.NTSTime5 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.NTSTime5);
            DataItem.Auditer5 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Auditer5);
            DataItem.Principal6 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Principal6);
          
            DataItem.NTSTime6 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.NTSTime6);
            DataItem.Auditer6 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Auditer6);
            DataItem.Principal7 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Principal7);
            
            DataItem.NTSTime7 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.NTSTime7);
            DataItem.Auditer7 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Auditer7);
            DataItem.Principal8 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Principal8);
  
            DataItem.NTSTime8 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.NTSTime8);
            DataItem.Auditer8 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Auditer8);
            DataItem.Principal9 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Principal9);
          
            DataItem.NTSTime9 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.NTSTime9);
            DataItem.Auditer9 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Auditer9);
            DataItem.Principal10 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Principal10);
            
            DataItem.NTSTime10 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.NTSTime10);
            DataItem.Auditer10 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Auditer10);
            DataItem.Principal11 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Principal11);
           
            DataItem.NTSTime11 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.NTSTime11);
            DataItem.Auditer11 = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Auditer11);







        }
        private static string DataCheck(Hold DataItem)
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
        public static string Assignment(SqlConnection cnPortal)
        {
            string LastNumStr = getLastNumStr(cnPortal); ;
            string number0 = "";
            DateTime date = System.DateTime.Now;
            string year = date.Year.ToString().Substring(2, 2);
            string Month = date.Month.ToString();
            if (Month .Length< 2)
            {
                Month = 0 + Month;
            }
            string Pr = year + Month;
            if (LastNumStr.Length < 10 || LastNumStr.Substring(0, 4) != Pr)
            {
                return Pr + "000001";
            }
            else
            {
                int clientnumber = Convert.ToInt32(LastNumStr.Substring(4, LastNumStr.Length - 4)) + 1;
                if (clientnumber.ToString().Length > LastNumStr.Length - 4)
                {
                    return Pr + clientnumber.ToString();
                }
                else
                {
                    for (int i = 0; i < LastNumStr.Length - 4- clientnumber.ToString().Length; i++)
                    {
                        number0 += "0";
                    }
                    return Pr + number0 + clientnumber.ToString();
                }
            }
        }
        private static string getLastNumStr(SqlConnection cn)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select Max(HoldNo) from TB_SQM_HOLD_REPORT");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
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

        public static string UpdateStatus(SqlConnection cn, string SID, string status)
        {
            string sErrMsg = string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE TB_SQM_HOLD_REPORT set status=@status where SID=@SID");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.AddWithValue("@SID", SQMStringHelper.NullOrEmptyStringIsDBNull(SID));
                cmd.Parameters.AddWithValue("@status", SQMStringHelper.NullOrEmptyStringIsDBNull(status));
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }
            }
            if (string.IsNullOrEmpty(sErrMsg))
            {
                DataTable Holddt = new DataTable();
                string Info = string.Empty;
                Holddt = GetHoldbySID(cn,SID);
                if (status.Equals("1"))
                {
                    Info = "审核通过";
                }
                else
                {
                    Info = "审核未通过";
                }
                foreach (DataRow dr in Holddt.Rows)
                {
                    string Email = GetMailByMemberGUID(cn,dr["CreatName"].ToString());
                    string sbBody = "HOLD编号" + dr["HoldNo"].ToString() + ","+Info+"请登录系统进行后续操作";
                    sendMail(Email, sbBody);
                }  
            }

            return sErrMsg;
        }
        public static string UpdateReleaseQuantity(SqlConnection cn, string HoldNo, string ReleaseQuantity,string MemberGUID)
        {
            string sErrMsg = string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE TB_SQM_HOLD_REPORT set ReleaseQuantity=@ReleaseQuantity where HoldNo=@HoldNo");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.AddWithValue("@HoldNo", SQMStringHelper.NullOrEmptyStringIsDBNull(HoldNo));
                cmd.Parameters.AddWithValue("@ReleaseQuantity", SQMStringHelper.NullOrEmptyStringIsDBNull(ReleaseQuantity));
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }
            }
            if (string.IsNullOrEmpty(sErrMsg))
            {
                string Email = getEmail(cn, GetPlant(cn, MemberGUID));
                string sbBody = "HOLD编号" + HoldNo + "需审核，请登录系统进行审核";
                sendMail(Email, sbBody);
            }

            return sErrMsg;
        }
        public static string UpdateRejectQuantity(SqlConnection cn, string HoldNo, string RejectQuantity,string MemberGUID)
        {
            string sErrMsg = string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE TB_SQM_HOLD_REPORT set RejectQuantity=@RejectQuantity where HoldNo=@HoldNo");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.AddWithValue("@HoldNo", SQMStringHelper.NullOrEmptyStringIsDBNull(HoldNo));
                cmd.Parameters.AddWithValue("@RejectQuantity", SQMStringHelper.NullOrEmptyStringIsDBNull(RejectQuantity));
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }
            }
            if (string.IsNullOrEmpty(sErrMsg))
            {
                string Email = getEmail(cn, GetPlant(cn, MemberGUID));
                string sbBody = "HOLD编号" + HoldNo + "需审核，请登录系统进行审核";
                sendMail(Email, sbBody);
            }

            return sErrMsg;
        }
        public static string UpdateHold(SqlConnection cn,Hold DataItem)
        {
            string sErrMsg = string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.Append(@"UPDATE TB_SQM_HOLD_REPORT set 
                   [Principal1] = @Principal1
      ,[NTSNum1] = @NTSNum1
      ,[NTSTime1] = @NTSTime1
      ,[Auditer1] = @Auditer1
      ,[Principal2] = @Principal2
      ,[NTSNum2] = @NTSNum2
      ,[NTSTime2] = @NTSTime2
      ,[Auditer2] = @Auditer2
      ,[Principal3] = @Principal3
      ,[NTSNum3] = @NTSNum3
      ,[NTSTime3] = @NTSTime3
      ,[Auditer3] = @Auditer3
      ,[Principal4] = @Principal4
      ,[NTSNum4] = @NTSNum4
      ,[NTSTime4] = @NTSTime4
      ,[Auditer4] = @Auditer4
      ,[Principal5] = @Principal5
      ,[NTSNum5] = @NTSNum5
      ,[NTSTime5] = @NTSTime5
      ,[Auditer5] = @Auditer5
      ,[Principal6] = @Principal6
      ,[NTSNum6] = @NTSNum6
      ,[NTSTime6] = @NTSTime6
      ,[Auditer6] = @Auditer6
      ,[Principal7] = @Principal7
      ,[NTSNum7] = @NTSNum7
      ,[NTSTime7] = @NTSTime7
      ,[Auditer7] = @Auditer7
      ,[Principal8] = @Principal8
      ,[NTSNum8] = @NTSNum8
      ,[NTSTime8] = @NTSTime8
      ,[Auditer8] = @Auditer8
      ,[Principal9] = @Principal9
      ,[NTSNum9] = @NTSNum9
      ,[NTSTime9] = @NTSTime9
      ,[Auditer9] = @Auditer9
      ,[Principal10] = @Principal10
      ,[NTSNum10] = @NTSNum10
      ,[NTSTime10] = @NTSTime10
      ,[Auditer10] = @Auditer10
      ,[Principal11] = @Principal11
      ,[NTSNum11] = @NTSNum11
      ,[NTSTime11] = @NTSTime11
      ,[Auditer11] = @Auditer11
                where HoldNo=@HoldNo");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.AddWithValue("@Principal1", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Principal1));
                cmd.Parameters.AddWithValue("@NTSNum1", DataItem.NTSNum1);
                cmd.Parameters.AddWithValue("@NTSTime1", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.NTSTime1));
                cmd.Parameters.AddWithValue("@Auditer1", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Auditer1));
                cmd.Parameters.AddWithValue("@Principal2", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Principal2));
                cmd.Parameters.AddWithValue("@NTSNum2", DataItem.NTSNum2);
                cmd.Parameters.AddWithValue("@NTSTime2", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.NTSTime2));
                cmd.Parameters.AddWithValue("@Auditer2", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Auditer2));
                cmd.Parameters.AddWithValue("@Principal3", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Principal3));
                cmd.Parameters.AddWithValue("@NTSNum3", DataItem.NTSNum3);
                cmd.Parameters.AddWithValue("@NTSTime3", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.NTSTime3));
                cmd.Parameters.AddWithValue("@Auditer3", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Auditer3));
                cmd.Parameters.AddWithValue("@Principal4", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Principal4));
                cmd.Parameters.AddWithValue("@NTSNum4", DataItem.NTSNum4);
                cmd.Parameters.AddWithValue("@NTSTime4", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.NTSTime4));
                cmd.Parameters.AddWithValue("@Auditer4", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Auditer4));
                cmd.Parameters.AddWithValue("@Principal5", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Principal5));
                cmd.Parameters.AddWithValue("@NTSNum5", DataItem.NTSNum5);
                cmd.Parameters.AddWithValue("@NTSTime5", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.NTSTime5));
                cmd.Parameters.AddWithValue("@Auditer5", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Auditer5));
                cmd.Parameters.AddWithValue("@Principal6", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Principal6));
                cmd.Parameters.AddWithValue("@NTSNum6", DataItem.NTSNum6);
                cmd.Parameters.AddWithValue("@NTSTime6", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.NTSTime6));
                cmd.Parameters.AddWithValue("@Auditer6", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Auditer6));
                cmd.Parameters.AddWithValue("@Principal7", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Principal7));
                cmd.Parameters.AddWithValue("@NTSNum7", DataItem.NTSNum7);
                cmd.Parameters.AddWithValue("@NTSTime7", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.NTSTime7));
                cmd.Parameters.AddWithValue("@Auditer7", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Auditer7));
                cmd.Parameters.AddWithValue("@Principal8", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Principal8));
                cmd.Parameters.AddWithValue("@NTSNum8", DataItem.NTSNum8);
                cmd.Parameters.AddWithValue("@NTSTime8", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.NTSTime8));
                cmd.Parameters.AddWithValue("@Auditer8", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Auditer8));
                cmd.Parameters.AddWithValue("@Principal9", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Principal9));
                cmd.Parameters.AddWithValue("@NTSNum9", DataItem.NTSNum9);
                cmd.Parameters.AddWithValue("@NTSTime9", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.NTSTime9));
                cmd.Parameters.AddWithValue("@Auditer9", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Auditer9));
                cmd.Parameters.AddWithValue("@Principal10", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Principal10));
                cmd.Parameters.AddWithValue("@NTSNum10", DataItem.NTSNum10);
                cmd.Parameters.AddWithValue("@NTSTime10", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.NTSTime10));
                cmd.Parameters.AddWithValue("@Auditer10", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Auditer10));
                cmd.Parameters.AddWithValue("@Principal11", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Principal11));
                cmd.Parameters.AddWithValue("@NTSNum11", DataItem.NTSNum11);
                cmd.Parameters.AddWithValue("@NTSTime11", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.NTSTime11));
                cmd.Parameters.AddWithValue("@Auditer11", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Auditer11));
                cmd.Parameters.AddWithValue("@HoldNo", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.HoldNo));
               
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }
            }


            return sErrMsg;
        }
        public class Hold_jQGridJSon
        {
            public List<HoldData> Rows = new List<HoldData>();
            public string Total
            {
                get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
            }
            public string Page { get { return "1"; } }
            public string Records { get { return Rows.Count.ToString(); } }
        }
    }
}

