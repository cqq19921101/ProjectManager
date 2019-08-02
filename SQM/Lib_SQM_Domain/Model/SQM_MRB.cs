using Lib_Portal_Domain.Model;
using Lib_Portal_Domain.SharedLibs;
using Lib_SQM_Domain.Modal;
using Lib_SQM_Domain.SharedLibs;
using Lib_VMI_Domain.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace Lib_SQM_Domain.Model
{

    public class SQMMRB
    {
        public string _SID;
        public string _MRBNo;
        public string _CreateTime;
        public string _LitNo;
        public string _VenderCode;
        public string _Initiator;
        public string _meettype;
        public string _NOTE;
        public string _Size;
        public string _Batch;
        public string _Baddescription;
        public string _Ieisproduce;
        public string _IEDeductionsorder;
        public string SID { get { return this._SID; } set { this._SID = value; } }

        public string MRBNo { get { return this._MRBNo; } set { this._MRBNo = value; } }
        public string CreateTime { get { return this._CreateTime; } set { this._CreateTime = value; } }
        public string LitNo { get { return this._LitNo; } set { this._LitNo = value; } }
        public string VenderCode { get { return this._VenderCode; } set { this._VenderCode = value; } }
        public string Initiator { get { return this._Initiator; } set { this._Initiator = value; } }
        public string meettype { get { return this._meettype; } set { this._meettype = value; } }
        public string NOTE { get { return this._NOTE; } set { this._NOTE = value; } }
        public string Size { get { return this._Size; } set { this._Size = value; } }
        public string Batch { get { return this._Batch; } set { this._Batch = value; } }
        public string Baddescription { get { return this._Baddescription; } set { this._Baddescription = value; } }
        public string Ieisproduce { get { return this._Ieisproduce; } set { this._Ieisproduce = value; } }
        public string IEDeductionsorder { get { return this._IEDeductionsorder; } set { this._IEDeductionsorder = value; } }
        public SQMMRB() { }
        public SQMMRB(string SID, string MRBNo, string CreateTime, string LitNo, string VenderCode, string Initiator, string meettype, string NOTE,string Size,string Batch,string Baddescription, string Ieisproduce, string IEDeductionsorder)
        {
            this._SID = SID;
            this._MRBNo = MRBNo;
            this._CreateTime = CreateTime;
            this._LitNo = LitNo;
            this._VenderCode = VenderCode;
            this._Initiator = Initiator;
            this._meettype = meettype;
            this._NOTE = NOTE;
            this._Size = Size;
            this._Batch = Batch;
            this._Baddescription = Baddescription;
            this._Ieisproduce = Ieisproduce;
            this._IEDeductionsorder = IEDeductionsorder;
        }
    }

    public class SQMMRBData
    {
        public string _SID;
        public string _MRBType;
        public string _MRBNo;
        public string _Initiator;
        public string _Governor;
        public string _CreateName;
        public string _CreateTime;
        public string _VenderType;
        public string _VenderCode;
        public string _LitNo;
        public string _Size;
        public string _Batch;
        public string _CheckNumber;
        public string _Defect;
        public string _RejectRatio;
        public string _Inspector;
        public string _BadDescriptionType;
        public string _Baddescription;
        public string _meettype;
        public string _PMsign;
        public string _PMsigndate;
        public string _QRsign;
        public string _Qrsigndate;
        public string _purchasetime;
        public string _model;
        public string _site;
        public string _duty;
        public string _compere;
        public string _Puropinion;
        public string _PURsign;
        public string _Pursigndate;
        public string _Pmcopinion;
        public string _PMCsign;
        public string _Pmcsigndate;
        public string _PMCworkorder;
        public string _PMConlineTime;
        public string _Mfgopinion;
        public string _MFGsign;
        public string _Mfgsigndate;
        public string _Smtengopinion;
        public string _SMTENGsign;
        public string _Smtengsigndate;
        public string _Pteopinion;
        public string _PTEsign;
        public string _Ptesigndate;
        public string _Ieopinion;
        public string _IEsign;
        public string _Iesigndate;
        public string _Ieisproduce;
        public string _IEDeductionsorder;
        public string _Sqmopinion;
        public string _SQMsign;
        public string _SqMsigndate;
        public string _Pqeopinion;
        public string _PQEsign;
        public string _Pqesigndate;
        public string _Cqeopinion;
        public string _CQEsign;
        public string _Cqesigndate;
        public string _Rdopinion;
        public string _Rdsign;
        public string _Rdsigndate;
        public string _Salesopinion;
        public string _SALESsign;
        public string _Salessigndate;
        public string _Qraopinion;
        public string _QRAsign;
        public string _Qrasigndate;
        public string _Mdopinion;
        public string _MDsign;
        public string _Mdsigndate;
        public string _NOTE;
        public string _FileNo;
        public string SID { get { return this._SID; } set { this._SID = value; } }
        public string MRBType { get { return this._MRBType; } set { this._MRBType = value; } }
        public string MRBNo { get { return this._MRBNo; } set { this._MRBNo = value; } }
        public string Initiator { get { return this._Initiator; } set { this._Initiator = value; } }
        public string Governor { get { return this._Governor; } set { this._Governor = value; } }
        public string CreateName { get { return this._CreateName; } set { this._CreateName = value; } }
        public string CreateTime { get { return this._CreateTime; } set { this._CreateTime = value; } }
        public string VenderType { get { return this._VenderType; } set { this._VenderType = value; } }
        public string VenderCode { get { return this._VenderCode; } set { this._VenderCode = value; } }
        public string LitNo { get { return this._LitNo; } set { this._LitNo = value; } }
        public string Size { get { return this._Size; } set { this._Size = value; } }
        public string Batch { get { return this._Batch; } set { this._Batch = value; } }
        public string CheckNumber { get { return this._CheckNumber; } set { this._CheckNumber = value; } }
        public string Defect { get { return this._Defect; } set { this._Defect = value; } }
        public string RejectRatio { get { return this._RejectRatio; } set { this._RejectRatio = value; } }
        public string Inspector { get { return this._Inspector; } set { this._Inspector = value; } }
        public string BadDescriptionType { get { return this._BadDescriptionType; } set { this._BadDescriptionType = value; } }
        public string Baddescription { get { return this._Baddescription; } set { this._Baddescription = value; } }
        public string meettype { get { return this._meettype; } set { this._meettype = value; } }
        public string PMsign { get { return this._PMsign; } set { this._PMsign = value; } }
        public string PMsigndate { get { return this._PMsigndate; } set { this._PMsigndate = value; } }
        public string QRsign { get { return this._QRsign; } set { this._QRsign = value; } }
        public string Qrsigndate { get { return this._Qrsigndate; } set { this._Qrsigndate = value; } }
        public string purchasetime { get { return this._purchasetime; } set { this._purchasetime = value; } }
        public string model { get { return this._model; } set { this._model = value; } }
        public string site { get { return this._site; } set { this._site = value; } }
        public string duty { get { return this._duty; } set { this._duty = value; } }
        public string compere { get { return this._compere; } set { this._compere = value; } }
        public string Puropinion { get { return this._Puropinion; } set { this._Puropinion = value; } }
        public string PURsign { get { return this._PURsign; } set { this._PURsign = value; } }
        public string Pursigndate { get { return this._Pursigndate; } set { this._Pursigndate = value; } }
        public string Pmcopinion { get { return this._Pmcopinion; } set { this._Pmcopinion = value; } }
        public string PMCsign { get { return this._PMCsign; } set { this._PMCsign = value; } }
        public string Pmcsigndate { get { return this._Pmcsigndate; } set { this._Pmcsigndate = value; } }
        public string PMCworkorder { get { return this._PMCworkorder; } set { this._PMCworkorder = value; } }
        public string PMConlineTime { get { return this._PMConlineTime; } set { this._PMConlineTime = value; } }
        public string Mfgopinion { get { return this._Mfgopinion; } set { this._Mfgopinion = value; } }
        public string MFGsign { get { return this._MFGsign; } set { this._MFGsign = value; } }
        public string Mfgsigndate { get { return this._Mfgsigndate; } set { this._Mfgsigndate = value; } }
        public string Smtengopinion { get { return this._Smtengopinion; } set { this._Smtengopinion = value; } }
        public string SMTENGsign { get { return this._SMTENGsign; } set { this._SMTENGsign = value; } }
        public string Smtengsigndate { get { return this._Smtengsigndate; } set { this._Smtengsigndate = value; } }
        public string Pteopinion { get { return this._Pteopinion; } set { this._Pteopinion = value; } }
        public string PTEsign { get { return this._PTEsign; } set { this._PTEsign = value; } }
        public string Ptesigndate { get { return this._Ptesigndate; } set { this._Ptesigndate = value; } }
        public string Ieopinion { get { return this._Ieopinion; } set { this._Ieopinion = value; } }
        public string IEsign { get { return this._IEsign; } set { this._IEsign = value; } }
        public string Iesigndate { get { return this._Iesigndate; } set { this._Iesigndate = value; } }
        public string Ieisproduce { get { return this._Ieisproduce; } set { this._Ieisproduce = value; } }
        public string IEDeductionsorder { get { return this._IEDeductionsorder; } set { this._IEDeductionsorder = value; } }
        public string Sqmopinion { get { return this._Sqmopinion; } set { this._Sqmopinion = value; } }
        public string SQMsign { get { return this._SQMsign; } set { this._SQMsign = value; } }
        public string SqMsigndate { get { return this._SqMsigndate; } set { this._SqMsigndate = value; } }
        public string Pqeopinion { get { return this._Pqeopinion; } set { this._Pqeopinion = value; } }
        public string PQEsign { get { return this._PQEsign; } set { this._PQEsign = value; } }
        public string Pqesigndate { get { return this._Pqesigndate; } set { this._Pqesigndate = value; } }
        public string Cqeopinion { get { return this._Cqeopinion; } set { this._Cqeopinion = value; } }
        public string CQEsign { get { return this._CQEsign; } set { this._CQEsign = value; } }
        public string Cqesigndate { get { return this._Cqesigndate; } set { this._Cqesigndate = value; } }
        public string Rdopinion { get { return this._Rdopinion; } set { this._Rdopinion = value; } }
        public string Rdsign { get { return this._Rdsign; } set { this._Rdsign = value; } }
        public string Rdsigndate { get { return this._Rdsigndate; } set { this._Rdsigndate = value; } }
        public string Salesopinion { get { return this._Salesopinion; } set { this._Salesopinion = value; } }
        public string SALESsign { get { return this._SALESsign; } set { this._SALESsign = value; } }
        public string Salessigndate { get { return this._Salessigndate; } set { this._Salessigndate = value; } }
        public string Qraopinion { get { return this._Qraopinion; } set { this._Qraopinion = value; } }
        public string QRAsign { get { return this._QRAsign; } set { this._QRAsign = value; } }
        public string Qrasigndate { get { return this._Qrasigndate; } set { this._Qrasigndate = value; } }
        public string Mdopinion { get { return this._Mdopinion; } set { this._Mdopinion = value; } }
        public string MDsign { get { return this._MDsign; } set { this._MDsign = value; } }
        public string Mdsigndate { get { return this._Mdsigndate; } set { this._Mdsigndate = value; } }
        public string NOTE { get { return this._NOTE; } set { this._NOTE = value; } }
        public string FileNo { get { return this._FileNo; } set { this._FileNo = value; } }

        public SQMMRBData() { }
        public SQMMRBData(
            string  SID
            ,string MRBType
           , string MRBNo
           , string Initiator
           , string Governor
           , string CreateName
           , string CreateTime
           , string VenderType
           , string VenderCode
           , string LitNo
           , string Size
           , string Batch
           , string CheckNumber
           , string Defect
           , string RejectRatio
           , string Inspector
           , string BadDescriptionType
           , string Baddescription
           , string meettype
           , string PMsign
           , string PMsigndate
           , string QRsign
           , string Qrsigndate
           , string purchasetime
           , string model
           , string site
           , string duty
           , string compere
           , string Puropinion
           , string PURsign
           , string Pursigndate
           , string Pmcopinion
           , string PMCsign
           , string Pmcsigndate
           , string PMCworkorder
           , string PMConlineTime
           , string Mfgopinion
           , string MFGsign
           , string Mfgsigndate
           , string Smtengopinion
           , string SMTENGsign
           , string Smtengsigndate
           , string Pteopinion
           , string PTEsign
           , string Ptesigndate
           , string Ieopinion
           , string IEsign
           , string Iesigndate
           , string Ieisproduce
           , string IEDeductionsorder
           , string Sqmopinion
           , string SQMsign
           , string SqMsigndate
           , string Pqeopinion
           , string PQEsign
           , string Pqesigndate
           , string Cqeopinion
           , string CQEsign
           , string Cqesigndate
           , string Rdopinion
           , string Rdsign
           , string Rdsigndate
           , string Salesopinion
           , string SALESsign
           , string Salessigndate
           , string Qraopinion
           , string QRAsign
           , string Qrasigndate
           , string Mdopinion
           , string MDsign
           , string Mdsigndate
           , string NOTE
            ,string FileNo
            )
        {
            this._SID = SID;
            this._MRBType = MRBType;
            this._MRBNo = MRBNo;
            this._Initiator = Initiator;
            this._Governor = Governor;
            this._CreateName = CreateName;
            this._CreateTime = CreateTime;
            this._VenderType = VenderType;
            this._VenderCode = VenderCode;
            this._LitNo = LitNo;
            this._Size = Size;
            this._Batch = Batch;
            this._CheckNumber = CheckNumber;
            this._Defect = Defect;
            this._RejectRatio = RejectRatio;
            this._Inspector = Inspector;
            this._BadDescriptionType = BadDescriptionType;
            this._Baddescription = Baddescription;
            this._meettype = meettype;
            this._PMsign = PMsign;
            this._PMsigndate = PMsigndate;
            this._QRsign = QRsign;
            this._Qrsigndate = Qrsigndate;
            this._purchasetime = purchasetime;
            this._model = model;
            this._site = site;
            this._duty = duty;
            this._compere = compere;
            this._Puropinion = Puropinion;
            this._PURsign = PURsign;
            this._Pursigndate = Pursigndate;
            this._Pmcopinion = Pmcopinion;
            this._PMCsign = PMCsign;
            this._Pmcsigndate = Pmcsigndate;
            this._PMCworkorder = PMCworkorder;
            this._PMConlineTime = PMConlineTime;
            this._Mfgopinion = Mfgopinion;
            this._MFGsign = MFGsign;
            this._Mfgsigndate = Mfgsigndate;
            this._Smtengopinion = Smtengopinion;
            this._SMTENGsign = SMTENGsign;
            this._Smtengsigndate = Smtengsigndate;
            this._Pteopinion = Pteopinion;
            this._PTEsign = PTEsign;
            this._Ptesigndate = Ptesigndate;
            this._Ieopinion = Ieopinion;
            this._IEsign = IEsign;
            this._Iesigndate = Iesigndate;
            this._Ieisproduce = Ieisproduce;
            this._IEDeductionsorder = IEDeductionsorder;
            this._Sqmopinion = Sqmopinion;
            this._SQMsign = SQMsign;
            this._SqMsigndate = SqMsigndate;
            this._Pqeopinion = Pqeopinion;
            this._PQEsign = PQEsign;
            this._Pqesigndate = Pqesigndate;
            this._Cqeopinion = Cqeopinion;
            this._CQEsign = CQEsign;
            this._Cqesigndate = Cqesigndate;
            this._Rdopinion = Rdopinion;
            this._Rdsign = Rdsign;
            this._Rdsigndate = Rdsigndate;
            this._Salesopinion = Salesopinion;
            this._SALESsign = SALESsign;
            this._Salessigndate = Salessigndate;
            this._Qraopinion = Qraopinion;
            this._QRAsign = QRAsign;
            this._Qrasigndate = Qrasigndate;
            this._Mdopinion = Mdopinion;
            this._MDsign = MDsign;
            this._Mdsigndate = Mdsigndate;
            this._NOTE = NOTE;
            this._FileNo = FileNo;
        }



    }
    public static class SQMMRB_Helper
    {
        public static string GetDataToJQGridJson(SqlConnection cn, string MemberGUID)
        {
            return GetDataToJQGridJson(cn, "", MemberGUID);
        }
     


        public static string GetMRBData(SqlConnection cn, string SID)
        {
            string urlPre = CommonHelper.urlPre;
            StringBuilder sb = new StringBuilder();
            sb.Append(
            @"
              select * FROM TB_SQM_MRB_REPORT
              where SID=@SID
            ");

            DataTable dt = new DataTable();
            using (SqlConnection cnSPM = new SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["CZSPMDBConnString"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand(sb.ToString(), cnSPM))
                {
                    cnSPM.Open();
                    cmd.Parameters.Add(new SqlParameter("@SID", SID));
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        dt.Load(dr);
                    }
                }
            }
            return JsonConvert.SerializeObject(dt);
        }
      
        public static string GetDataToJQGridJson(SqlConnection cn, string SearchText, string MemberGUID)
        {
            SQMMRB_jQGridJSon m = new SQMMRB_jQGridJSon();
            string urlPre = CommonHelper.urlPre;
            string sSearchText = SearchText.Trim();
            string meettype = string.Empty;
            string Ieisproduce = string.Empty;
            string sWhereClause = "";
            if (sSearchText != "")
                sWhereClause += " and MRBNO like '%' + @SearchText + '%'";


            m.Rows.Clear();
            int iRowCount = 0;
            StringBuilder sSQL = new StringBuilder();
            sSQL.Append(@"
                        select SID,MRBNo,CreateTime,LitNo,Size,VenderCode,Batch,Baddescription,Initiator,meettype,NOTE,Ieisproduce,IEDeductionsorder  FROM [dbo].[TB_SQM_MRB_REPORT]  where 1=1
             ");
            string ssSQL = sSQL.ToString() + sWhereClause + ";";
            using (SqlConnection cnSPM = new SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["CZSPMDBConnString"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand(ssSQL, cnSPM))
                {
                    cnSPM.Open();
                    if (sSearchText != "")
                        cmd.Parameters.Add(new SqlParameter("@SearchText", sSearchText));

                  
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        switch (dr["meettype"].ToString().Trim())
                        {
                            case "A":
                                meettype = "篩選";
                                break;
                            case "B":
                                meettype = "重工";
                                break;
                            case "C":
                                meettype = "特采使用";
                                break;
                            case "D":
                                meettype = "退回供應商";
                                break;
                            case "E":
                                meettype = "廠內報廢";
                                break;
                        }
                        switch (dr["Ieisproduce"].ToString())
                        {
                            case "A":
                                Ieisproduce = "YES";
                                break;
                            case "B":
                                Ieisproduce = "NO";
                                break;
                          
                        }
                        iRowCount++;
                        m.Rows.Add(new SQMMRB(
                              dr["SID"].ToString(),
                              dr["MRBNo"].ToString(),
                              Convert.ToDateTime(dr["CreateTime"].ToString()).ToString("yyyy/MM/dd"),
                              dr["LitNo"].ToString(),
                              dr["VenderCode"].ToString(),
                              dr["Initiator"].ToString(),
                              meettype,
                              dr["NOTE"].ToString(),
                              dr["Size"].ToString(),
                              dr["Batch"].ToString(),
                              dr["Baddescription"].ToString(),
                              Ieisproduce,
                              dr["IEDeductionsorder"].ToString()

                               ));
                    }
                    dr.Close();
                    dr = null;
                }
            }

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(m);
        }

        public static string CreateDataItem(SqlConnection cnPortal, SQMMRBData DataItem, string MemberGUID, string localPath, string urlPre)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            string MRBNo = Assignment(cnPortal);
            if (r != "")
            { return r; }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"
INSERT INTO [dbo].[TB_SQM_MRB_REPORT]
           ([MRBType]
           ,[MRBNo]
           ,Plant
           ,[Initiator]
           ,[Governor]
           ,[CreateName]
           ,[CreateTime]
           ,[VenderType]
           ,[VenderCode]
           ,[LitNo]
           ,[Size]
           ,[Batch]
           ,[CheckNumber]
           ,[Defect]
           ,[RejectRatio]
           ,[Inspector]
           ,[BadDescriptionType]
           ,[Baddescription]
           ,[meettype]
           ,[PMsign]
           ,[PMsigndate]
           ,[QRsign]
           ,[Qrsigndate]
           ,[purchasetime]
           ,[model]
           ,[site]
           ,[duty]
           ,[compere]
           ,[Puropinion]
           ,[PURsign]
           ,[Pursigndate]
           ,[Pmcopinion]
           ,[PMCsign]
           ,[Pmcsigndate]
           ,[PMCworkorder]
           ,[PMConlineTime]
           ,[Mfgopinion]
           ,[MFGsign]
           ,[Mfgsigndate]
           ,[Smtengopinion]
           ,[SMTENGsign]
           ,[Smtengsigndate]
           ,[Pteopinion]
           ,[PTEsign]
           ,[Ptesigndate]
           ,[Ieopinion]
           ,[IEsign]
           ,[Iesigndate]
           ,[Ieisproduce]
           ,[IEDeductionsorder]
           ,[Sqmopinion]
           ,[SQMsign]
           ,[SqMsigndate]
           ,[Pqeopinion]
           ,[PQEsign]
           ,[Pqesigndate]
           ,[Cqeopinion]
           ,[CQEsign]
           ,[Cqesigndate]
           ,[Rdopinion]
           ,[Rdsign]
           ,[Rdsigndate]
           ,[Salesopinion]
           ,[SALESsign]
           ,[Salessigndate]
           ,[Qraopinion]
           ,[QRAsign]
           ,[Qrasigndate]
           ,[Mdopinion]
           ,[MDsign]
           ,[Mdsigndate]
           ,[NOTE])
     VALUES
           (@MRBType
           ,@MRBNo
           ,@Plant
           ,@Initiator
           ,@Governor
           ,@CreateName
           ,@CreateTime
           ,@VenderType
           ,@VenderCode
           ,@LitNo
           ,@Size
           ,@Batch
           ,@CheckNumber
           ,@Defect
           ,@RejectRatio
           ,@Inspector
           ,@BadDescriptionType
           ,@Baddescription
           ,@meettype
           ,@PMsign
           ,@PMsigndate
           ,@QRsign
           ,@Qrsigndate
           ,@purchasetime
           ,@model
           ,@site
           ,@duty
           ,@compere
           ,@Puropinion
           ,@PURsign
           ,@Pursigndate
           ,@Pmcopinion
           ,@PMCsign
           ,@Pmcsigndate
           ,@PMCworkorder
           ,@PMConlineTime
           ,@Mfgopinion
           ,@MFGsign
           ,@Mfgsigndate
           ,@Smtengopinion
           ,@SMTENGsign
           ,@Smtengsigndate
           ,@Pteopinion
           ,@PTEsign
           ,@Ptesigndate
           ,@Ieopinion
           ,@IEsign
           ,@Iesigndate
           ,@Ieisproduce
           ,@IEDeductionsorder
           ,@Sqmopinion
           ,@SQMsign
           ,@SqMsigndate
           ,@Pqeopinion
           ,@PQEsign
           ,@Pqesigndate
           ,@Cqeopinion
           ,@CQEsign
           ,@Cqesigndate
           ,@Rdopinion
           ,@Rdsign
           ,@Rdsigndate
           ,@Salesopinion
           ,@SALESsign
           ,@Salessigndate
           ,@Qraopinion
           ,@QRAsign
           ,@Qrasigndate
           ,@Mdopinion
           ,@MDsign
           ,@Mdsigndate
           ,@NOTE                   )
          ");
                SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal);
                cmd.Parameters.AddWithValue("@MRBType", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.MRBType));
                cmd.Parameters.AddWithValue("@MRBNo", MRBNo);
                cmd.Parameters.AddWithValue("@Plant", GetPlant(cnPortal,MemberGUID));
                cmd.Parameters.AddWithValue("@Initiator", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Initiator));
                cmd.Parameters.AddWithValue("@Governor", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Governor));
                cmd.Parameters.AddWithValue("@CreateName", SQMStringHelper.NullOrEmptyStringIsDBNull(MemberGUID));
                cmd.Parameters.AddWithValue("@CreateTime", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.CreateTime));
                cmd.Parameters.AddWithValue("@VenderType", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.VenderType));
                cmd.Parameters.AddWithValue("@VenderCode", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.VenderCode));
                cmd.Parameters.AddWithValue("@LitNo", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.LitNo));
                cmd.Parameters.AddWithValue("@Size", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Size));
                cmd.Parameters.AddWithValue("@Batch", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Batch));
                cmd.Parameters.AddWithValue("@CheckNumber", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.CheckNumber));
                cmd.Parameters.AddWithValue("@Defect", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Defect));
                cmd.Parameters.AddWithValue("@RejectRatio", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.RejectRatio));
                cmd.Parameters.AddWithValue("@Inspector", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Inspector));
                cmd.Parameters.AddWithValue("@BadDescriptionType", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.BadDescriptionType));
                cmd.Parameters.AddWithValue("@Baddescription", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Baddescription));
                cmd.Parameters.AddWithValue("@meettype", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.meettype));
                cmd.Parameters.AddWithValue("@PMsign", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.PMsign));
                cmd.Parameters.AddWithValue("@PMsigndate", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.PMsigndate));
                cmd.Parameters.AddWithValue("@QRsign", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.QRsign));
                cmd.Parameters.AddWithValue("@Qrsigndate", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Qrsigndate));
                cmd.Parameters.AddWithValue("@purchasetime", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.purchasetime));
                cmd.Parameters.AddWithValue("@model", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.model));
                cmd.Parameters.AddWithValue("@site", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.site));
                cmd.Parameters.AddWithValue("@duty", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.duty));
                cmd.Parameters.AddWithValue("@compere", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.compere));
                cmd.Parameters.AddWithValue("@Puropinion", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Puropinion));
                cmd.Parameters.AddWithValue("@PURsign", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.PURsign));
                cmd.Parameters.AddWithValue("@Pursigndate", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Pursigndate));
                cmd.Parameters.AddWithValue("@Pmcopinion", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Pmcopinion));
                cmd.Parameters.AddWithValue("@PMCsign", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.PMCsign));
                cmd.Parameters.AddWithValue("@Pmcsigndate", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Pmcsigndate));
                cmd.Parameters.AddWithValue("@PMCworkorder", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.PMCworkorder));
                cmd.Parameters.AddWithValue("@PMConlineTime", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.PMConlineTime));
                cmd.Parameters.AddWithValue("@Mfgopinion", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Mfgopinion));
                cmd.Parameters.AddWithValue("@MFGsign", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.MFGsign));
                cmd.Parameters.AddWithValue("@Mfgsigndate", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Mfgsigndate));
                cmd.Parameters.AddWithValue("@Smtengopinion", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Smtengopinion));
                cmd.Parameters.AddWithValue("@SMTENGsign", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SMTENGsign));
                cmd.Parameters.AddWithValue("@Smtengsigndate", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Smtengsigndate));
                cmd.Parameters.AddWithValue("@Pteopinion", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Pteopinion));
                cmd.Parameters.AddWithValue("@PTEsign", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.PTEsign));
                cmd.Parameters.AddWithValue("@Ptesigndate", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Ptesigndate));
                cmd.Parameters.AddWithValue("@Ieopinion", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Ieopinion));
                cmd.Parameters.AddWithValue("@IEsign", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.IEsign));
                cmd.Parameters.AddWithValue("@Iesigndate", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Iesigndate));
                cmd.Parameters.AddWithValue("@Ieisproduce", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Ieisproduce));
                cmd.Parameters.AddWithValue("@IEDeductionsorder", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.IEDeductionsorder));
                cmd.Parameters.AddWithValue("@Sqmopinion", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Sqmopinion));
                cmd.Parameters.AddWithValue("@SQMsign", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SQMsign));
                cmd.Parameters.AddWithValue("@SqMsigndate", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SqMsigndate));
                cmd.Parameters.AddWithValue("@Pqeopinion", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Pqeopinion));
                cmd.Parameters.AddWithValue("@PQEsign", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.PQEsign));
                cmd.Parameters.AddWithValue("@Pqesigndate", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Pqesigndate));
                cmd.Parameters.AddWithValue("@Cqeopinion", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Cqeopinion));
                cmd.Parameters.AddWithValue("@CQEsign", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.CQEsign));
                cmd.Parameters.AddWithValue("@Cqesigndate", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Cqesigndate));
                cmd.Parameters.AddWithValue("@Rdopinion", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Rdopinion));
                cmd.Parameters.AddWithValue("@Rdsign", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Rdsign));
                cmd.Parameters.AddWithValue("@Rdsigndate", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Rdsigndate));
                cmd.Parameters.AddWithValue("@Salesopinion", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Salesopinion));
                cmd.Parameters.AddWithValue("@SALESsign", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.SALESsign));
                cmd.Parameters.AddWithValue("@Salessigndate", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Salessigndate));
                cmd.Parameters.AddWithValue("@Qraopinion", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Qraopinion));
                cmd.Parameters.AddWithValue("@QRAsign", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.QRAsign));
                cmd.Parameters.AddWithValue("@Qrasigndate", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Qrasigndate));
                cmd.Parameters.AddWithValue("@Mdopinion", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Mdopinion));
                cmd.Parameters.AddWithValue("@MDsign", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.MDsign));
                cmd.Parameters.AddWithValue("@Mdsigndate", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.Mdsigndate));
                cmd.Parameters.AddWithValue("@NOTE", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.NOTE));

                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;
               
                return sErrMsg;
            }
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
        public static string EditDataItem(SqlConnection cnPortal, SQMMRBData DataItem)
        {
            return EditDataItem(cnPortal, DataItem, "", "");
        }
        private static string DataCheck(SQMMRBData DataItem)
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
        public static string EditDataItem(SqlConnection cnPortal, SQMMRBData DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            if (r != "")
            { return r; }
            else
            {
                //SqlTransaction tran = cnPortal.BeginTransaction();

                //Update member data
                using (SqlCommand cmd = new SqlCommand("", cnPortal)) { r = EditDataItemSub(cmd, DataItem); }

                return r;
            }
        }
        private static void UnescapeDataFromWeb(SQMMRBData DataItem)
        {

            DataItem.SID = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.SID);
            DataItem.MRBType = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.MRBType);
            DataItem.MRBNo = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.MRBNo);
            DataItem.Initiator = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Initiator);
            DataItem.Governor = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Governor);
            DataItem.CreateName = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CreateName);
            DataItem.CreateTime = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CreateTime);
            DataItem.VenderType = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.VenderType);
            DataItem.VenderCode = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.VenderCode);
            DataItem.LitNo = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.LitNo);
            DataItem.Size = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Size);
            DataItem.Batch = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Batch);
            DataItem.CheckNumber = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CheckNumber);
            DataItem.Defect = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Defect);
            DataItem.RejectRatio = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.RejectRatio);
            DataItem.Inspector = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Inspector);
            DataItem.BadDescriptionType = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.BadDescriptionType);
            DataItem.Baddescription = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Baddescription);
            DataItem.meettype = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.meettype);
            DataItem.PMsign = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.PMsign);
            DataItem.PMsigndate = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.PMsigndate);
            DataItem.QRsign = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.QRsign);
            DataItem.Qrsigndate = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Qrsigndate);
            DataItem.purchasetime = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.purchasetime);
            DataItem.model = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.model);
            DataItem.site = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.site);
            DataItem.duty = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.duty);
            DataItem.compere = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.compere);
            DataItem.Puropinion = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Puropinion);
            DataItem.PURsign = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.PURsign);
            DataItem.Pursigndate = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Pursigndate);
            DataItem.Pmcopinion = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Pmcopinion);
            DataItem.PMCsign = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.PMCsign);
            DataItem.Pmcsigndate = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Pmcsigndate);
            DataItem.PMCworkorder = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.PMCworkorder);
            DataItem.PMConlineTime = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.PMConlineTime);
            DataItem.Mfgopinion = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Mfgopinion);
            DataItem.MFGsign = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.MFGsign);
            DataItem.Mfgsigndate = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Mfgsigndate);
            DataItem.Smtengopinion = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Smtengopinion);
            DataItem.SMTENGsign = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.SMTENGsign);
            DataItem.Smtengsigndate = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Smtengsigndate);
            DataItem.Pteopinion = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Pteopinion);
            DataItem.PTEsign = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.PTEsign);
            DataItem.Ptesigndate = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Ptesigndate);
            DataItem.Ieopinion = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Ieopinion);
            DataItem.IEsign = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.IEsign);
            DataItem.Iesigndate = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Iesigndate);
            DataItem.Ieisproduce = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Ieisproduce);
            DataItem.IEDeductionsorder = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.IEDeductionsorder);
            DataItem.Sqmopinion = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Sqmopinion);
            DataItem.SQMsign = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.SQMsign);
            DataItem.SqMsigndate = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.SqMsigndate);
            DataItem.Pqeopinion = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Pqeopinion);
            DataItem.PQEsign = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.PQEsign);
            DataItem.Pqesigndate = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Pqesigndate);
            DataItem.Cqeopinion = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Cqeopinion);
            DataItem.CQEsign = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CQEsign);
            DataItem.Cqesigndate = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Cqesigndate);
            DataItem.Rdopinion = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Rdopinion);
            DataItem.Rdsign = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Rdsign);
            DataItem.Rdsigndate = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Rdsigndate);
            DataItem.Salesopinion = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Salesopinion);
            DataItem.SALESsign = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.SALESsign);
            DataItem.Salessigndate = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Salessigndate);
            DataItem.Qraopinion = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Qraopinion);
            DataItem.QRAsign = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.QRAsign);
            DataItem.Qrasigndate = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Qrasigndate);
            DataItem.Mdopinion = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Mdopinion);
            DataItem.MDsign = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.MDsign);
            DataItem.Mdsigndate = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Mdsigndate);
            DataItem.NOTE = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.NOTE);
        
        }

        private static string EditDataItemSub(SqlCommand cmd, SQMMRBData DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
UPDATE [dbo].[TB_SQM_MRB_REPORT]
 SET      
      [NOTE] = @NOTE,FileNo=@FileNo
 WHERE SID = @SID");


           
            using (SqlConnection cnSPM = new SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["CZSPMDBConnString"].ConnectionString))
            {

                using (SqlCommand sqlcmd = new SqlCommand(sb.ToString(), cnSPM))
                {
                    cnSPM.Open();

                    sqlcmd.Parameters.AddWithValue("@NOTE", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.NOTE));
                    sqlcmd.Parameters.AddWithValue("@FileNo", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.FileNo));
                    sqlcmd.Parameters.AddWithValue("@SID", DataItem.SID);
                    try { sqlcmd.ExecuteNonQuery(); } catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }
                }
            }
           

            return sErrMsg;
        }
        public static string Assignment(SqlConnection cnPortal)
        {
            string LastNumStr = getLastNumStr(cnPortal); ;
            string number0 = "";
            DateTime date = System.DateTime.Now;


            if (LastNumStr.Length < 11 || LastNumStr.Substring(0, 3) != "DMA")
            {
                return "DMA" + date.Year.ToString().Substring(2, 2) + "000001";
            }
            else
            {
                int clientnumber = Convert.ToInt32(LastNumStr.Substring(4, LastNumStr.Length - 5)) + 1;
                if (clientnumber.ToString().Length > LastNumStr.Length - 5)
                {
                    return "DMA" + date.Year.ToString().Substring(2, 2) + clientnumber.ToString();
                }
                else
                {
                    for (int i = 0; i < LastNumStr.Length - 5 - clientnumber.ToString().Length; i++)
                    {
                        number0 += "0";
                    }
                    return "DMA" + date.Year.ToString().Substring(2, 2) + number0 + clientnumber.ToString();
                }
            }
        }
        private static string getLastNumStr(SqlConnection cn)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select Max(MRBNo) from TB_SQM_MRB_REPORT");
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
        public static string UploadFile(SqlConnection cn, PortalUserProfile RunAsUser, FileAttachmentInfo FA, string sLocalPath, string sLocalUploadPath, HttpServerUtilityBase Server, string RequestApplicationPath)
        {
            string r = string.Empty;
            JArray ja = JArray.Parse(FA.SPEC);
            dynamic jo_item = (JObject)ja[0];

            //00.UploadFileToDB
            SqlTransaction tran = cn.BeginTransaction();
            String file = sLocalUploadPath + FA.SUBFOLDER + "/" + jo_item.FileName;
            String FGUID = SharedLibs.SqlFileStreamHelper.InsertToTableSQM(cn, tran, RunAsUser.MemberGUID, file);
            if (FGUID == "")
            {
                tran.Dispose();
                return "fail";
            }
            try { tran.Commit(); }
            catch (Exception e) { tran.Rollback(); r = "Upload fail.<br />Exception: " + e.ToString(); }
            if (r == "")
            {
                return FGUID;
            }
            return "fail";
        }
        public static string GetMRBFilesDetail(SqlConnection cn, PortalUserProfile RunAsUser, String FGUID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(
            @"
SELECT   [FGUID]
        ,[FileName]
        ,[FileContent]
        ,[UpdateTime]
        ,[UpdateUser]
        ,[ValidDate]
        ,[SignDate]
FROM [TB_SQM_Files]
WHERE [FGUID]=@FGUID;
            ");
            String sql = Regex.Replace(sb.ToString(), @"\s+", " ");

            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.Add(new SqlParameter("@FGUID", FGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }

    }
    public class SQMMRB_jQGridJSon
    {
        public List<SQMMRB> Rows = new List<SQMMRB>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }
}
