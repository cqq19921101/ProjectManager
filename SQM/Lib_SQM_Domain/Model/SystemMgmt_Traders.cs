using Lib_Portal_Domain.Model;
using Lib_Portal_Domain.SharedLibs;
using Lib_VMI_Domain.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;



//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using System.Data.SqlClient;
//using System.Web.Script.Serialization;
//using Lib_SQM_Domain.SharedLibs;
//using System.Web;

namespace Lib_SQM_Domain.Modal
{


    #region Data Class Definitions
    public class SystemMgmt_Traders
    {
        protected string _BasicInfoGUID;
        protected string _PrincipalProducts;
        protected string _RevenuePer;
        protected string _MOQ;
        protected string _SampleTime;
        protected string _LeadTime;
        protected string _SupAndOriName;
        protected string _OfferPlaceCertify;
        protected string _OfferPlaceFGUID;
        protected string _OfferSellCertify;
        protected string _OfferSellFGUID;
        protected string _MajorCompetitor;


        public string BasicInfoGUID { get { return this._BasicInfoGUID; } set { this._BasicInfoGUID = value; } }
        public string PrincipalProducts { get { return this._PrincipalProducts; } set { this._PrincipalProducts = value; } }
        public string RevenuePer { get { return this._RevenuePer; } set { this._RevenuePer = value; } }
        public string MOQ { get { return this._MOQ; } set { this._MOQ = value; } }
        public string SampleTime { get { return this._SampleTime; } set { this._SampleTime = value; } }
        public string LeadTime { get { return this._LeadTime; } set { this._LeadTime = value; } }
        public string SupAndOriName { get { return this._SupAndOriName; } set { this._SupAndOriName = value; } }
        public string OfferPlaceCertify { get { return this._OfferPlaceCertify; } set { this._OfferPlaceCertify = value; } }
        public string OfferSellCertify { get { return this._OfferSellCertify; } set { this._OfferSellCertify = value; } }
        public string OfferPlaceFGUID { get { return this._OfferPlaceFGUID; } set { this._OfferPlaceFGUID = value; } }
        public string OfferSellFGUID { get { return this._OfferSellFGUID; } set { this._OfferSellFGUID = value; } }
        public string MajorCompetitor { get { return this._MajorCompetitor; } set { this._MajorCompetitor = value; } }

        public SystemMgmt_Traders() { }
        public SystemMgmt_Traders(string BasicInfoGUID, string PrincipalProducts, string RevenuePer, string MOQ, string SampleTime, string LeadTime, string SupAndOriName, string OfferPlaceCertify, string OfferPlaceFGUID, string OfferSellCertify, string OfferSellFGUID, string MajorCompetitor)
        {
            this._BasicInfoGUID = BasicInfoGUID;
            this._PrincipalProducts = PrincipalProducts;
            this._RevenuePer = RevenuePer;
            this._MOQ = MOQ;
            this._SampleTime = SampleTime;
            this._LeadTime = LeadTime;
            this._SupAndOriName = SupAndOriName;
            this._OfferPlaceCertify = OfferPlaceCertify;
            this._OfferPlaceFGUID = OfferPlaceFGUID;
            this._OfferSellCertify = OfferSellCertify;
            this._OfferSellFGUID = OfferSellFGUID;
            this._MajorCompetitor = MajorCompetitor;
        }
    }

    public class SystemMgmt_Traders_jQGridJSon
    {
        public List<SystemMgmt_Traders> Rows = new List<SystemMgmt_Traders>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }
    #endregion

    public static class SystemMgmt_Traders_Helper
    {

        public static string GetVendorType(SqlConnection cn, PortalUserProfile RunAsUser)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select (convert(nvarchar(255), CID))as CID, CNAME From TB_SQM_Vendor_Type Order By CID;");
            //sb.Append("Select CID, CNAME From TB_SQM_Commodity Order By CID;");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }

        public static string GetBasicData(SqlConnection cn, PortalUserProfile RunAsUser, String VendorCode)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select TOP 1 * From TB_SQM_Manufacturers_BasicInfo WHERE VendorCode=@VendorCode");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@VendorCode", VendorCode));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }

        public static string GetCriticalFile(SqlConnection cn, PortalUserProfile RunAsUser, String BasicInfoGUID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select TOP 1 * From TB_SQM_ProductDescription WHERE BasicInfoGUID=@BasicInfoGUID");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@BasicInfoGUID", BasicInfoGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }

        public static string GetCommodityList(SqlConnection cn, PortalUserProfile RunAsUser)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select CID, CNAME From TB_SQM_Commodity Order By CID;");

            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }

        public static string GetMapVendorCode(SqlConnection cn, PortalUserProfile RunAsUser)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select TOP 1 VendorCode From TB_SQM_Member_Vendor_Map");
            sb.Append(" WHERE MemberGUID=@MemberGUID");
            String VendorCode = "";
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@MemberGUID", RunAsUser.MemberGUID));
                var VendorCodeScale = cmd.ExecuteScalar();
                VendorCode = (VendorCodeScale == null ? "" : VendorCodeScale.ToString());
            }
            return VendorCode;
        }


        #region SearchSubFunc
        public static string GetDataToJQGridJson(SqlConnection cn)
        {
            return GetDataToJQGridJson(cn, "");
        }

        public static string GetDataToJQGridJson(SqlConnection cn, string SearchText)
        {
            SystemMgmt_Traders_jQGridJSon m = new SystemMgmt_Traders_jQGridJSon();

            string sSearchText = SearchText.Trim();
            string sWhereClause = "";
            if (sSearchText != "")
                sWhereClause += "  PrincipalProducts = @SearchText ";
            if (sWhereClause.Length != 0)
                sWhereClause = " Where" + sWhereClause.Substring(0);

            m.Rows.Clear();
            int iRowCount = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Top 100 BasicInfoGUID,PrincipalProducts,RevenuePer,MOQ,SampleTime,LeadTime,");
            sb.Append(" SupAndOriName,OfferPlaceCertify,OfferPlaceFGUID,OfferSellCertify,OfferSellFGUID,MajorCompetitor");
            sb.Append(" From TB_SQM_ProductDescription");
            sb.Append(sWhereClause + ";");
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                if (sSearchText != "")
                    cmd.Parameters.Add(new SqlParameter("@SearchText", sSearchText));
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    iRowCount++;
                    m.Rows.Add(new SystemMgmt_Traders(
                        dr["BasicInfoGUID"].ToString(),
                        dr["PrincipalProducts"].ToString(),
                        dr["RevenuePer"].ToString(),
                        dr["MOQ"].ToString(),
                        dr["SampleTime"].ToString(),
                        dr["LeadTime"].ToString(),
                        dr["SupAndOriName"].ToString(),
                        dr["OfferPlaceCertify"].ToString(),
                        dr["OfferPlaceFGUID"].ToString(),
                        dr["OfferSellCertify"].ToString(),
                        dr["OfferSellFGUID"].ToString(),
                        dr["MajorCompetitor"].ToString()
                        ));
                }
                dr.Close();
                dr = null;
            }

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(m);
        }
        #endregion

        #region Create/Edit data check
        private static void UnescapeDataFromWeb(SystemMgmt_Traders DataItem)
        {
            DataItem.BasicInfoGUID = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.BasicInfoGUID);
            DataItem.PrincipalProducts = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.PrincipalProducts);
            DataItem.MajorCompetitor = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.MajorCompetitor);
            DataItem.MOQ = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.MOQ);


        }

        private static string DataCheck(SystemMgmt_Traders DataItem)
        {
            string r = "";
            List<string> e = new List<string>();
            if (StringHelper.DataIsNullOrEmpty(DataItem.PrincipalProducts))
                e.Add("Must provide PrincipalProducts.");
            if (StringHelper.DataIsNullOrEmpty(DataItem.RevenuePer))
                e.Add("Must provide RevenuePer.");
            if (StringHelper.DataIsNullOrEmpty(DataItem.MOQ))
                e.Add("Must provide MOQ.");
            if (StringHelper.DataIsNullOrEmpty(DataItem.SampleTime))
                e.Add("Must provide SampleTime.");
            if (StringHelper.DataIsNullOrEmpty(DataItem.LeadTime))
                e.Add("Must provide LeadTime.");
            if (StringHelper.DataIsNullOrEmpty(DataItem.SupAndOriName))
                e.Add("Must provide SupAndOriName.");
            if (StringHelper.DataIsNullOrEmpty(DataItem.OfferPlaceCertify))
                e.Add("Must provide OfferPlaceCertify.");
            if (StringHelper.DataIsNullOrEmpty(DataItem.OfferSellCertify))
                e.Add("Must provide OfferSellCertify.");

            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }
        #endregion

        #region Create data item
        public static string CreateDataItem(SqlConnection cnPortal, SystemMgmt_Traders DataItem)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            if (r != "")
            {
                return r;
            }
            else
            {

                StringBuilder sb = new StringBuilder();
                sb.Append("Insert Into TB_SQM_ProductDescription (BasicInfoGUID,PrincipalProducts,RevenuePer,MOQ,SampleTime,LeadTime,");
                sb.Append(" SupAndOriName,OfferPlaceCertify,OfferSellCertify,MajorCompetitor)");
                sb.Append(" Values (@BasicInfoGUID,@PrincipalProducts, @RevenuePer,@MOQ,@SampleTime,@LeadTime,");
                sb.Append(" @SupAndOriName, @OfferPlaceCertify,@OfferSellCertify,@MajorCompetitor)");
                SQM_Basic_Helper.InsertPart(sb, "3");
                SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal);
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(DataItem.BasicInfoGUID));
                cmd.Parameters.AddWithValue("@PrincipalProducts", StringHelper.NullOrEmptyStringIsDBNull(DataItem.PrincipalProducts));
                cmd.Parameters.AddWithValue("@RevenuePer", StringHelper.NullOrEmptyStringIsDBNull(DataItem.RevenuePer));
                cmd.Parameters.AddWithValue("@MOQ", StringHelper.NullOrEmptyStringIsDBNull(DataItem.MOQ));
                cmd.Parameters.AddWithValue("@SampleTime", StringHelper.NullOrEmptyStringIsDBNull(DataItem.SampleTime));
                cmd.Parameters.AddWithValue("@LeadTime", StringHelper.NullOrEmptyStringIsDBNull(DataItem.LeadTime));
                cmd.Parameters.AddWithValue("@SupAndOriName", StringHelper.NullOrEmptyStringIsDBNull(DataItem.SupAndOriName));
                cmd.Parameters.AddWithValue("@OfferPlaceCertify", StringHelper.NullOrEmptyStringIsDBNull(DataItem.OfferPlaceCertify));
                //cmd.Parameters.AddWithValue("@OfferPlaceFGUID", Guid.NewGuid());
                cmd.Parameters.AddWithValue("@OfferSellCertify", StringHelper.NullOrEmptyStringIsDBNull(DataItem.OfferSellCertify));
                //cmd.Parameters.AddWithValue("@OfferSellFGUID", Guid.NewGuid());
                cmd.Parameters.AddWithValue("@MajorCompetitor", StringHelper.NullOrEmptyStringIsDBNull(DataItem.MajorCompetitor));


                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;

                return sErrMsg;
            }
        }
        #endregion

        #region Edit data item
        public static string EditDataItem(SqlConnection cnPortal, SystemMgmt_Traders DataItem)
        {
            return EditDataItem(cnPortal, DataItem, "", "");
        }

        public static string EditDataItem(SqlConnection cnPortal, SystemMgmt_Traders DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            if (r != "")
            { return r; }
            else
            {
                using (SqlCommand cmd = new SqlCommand("", cnPortal)) { r = EditDataItemSub(cmd, DataItem); }
                if (r != "") { return r; }
                return r;
            }
        }

        private static string EditDataItemSub(SqlCommand cmd, SystemMgmt_Traders DataItem)
        {
            string sErrMsg = "";

            StringBuilder sb = new StringBuilder();
            sb.Append("Update TB_SQM_ProductDescription Set");
            sb.Append(" RevenuePer= @RevenuePer,MOQ = @MOQ,SampleTime = @SampleTime,LeadTime = @LeadTime,");
            sb.Append(" SupAndOriName=@SupAndOriName,OfferPlaceCertify=@OfferPlaceCertify,");
            sb.Append(" OfferSellCertify=@OfferSellCertify,MajorCompetitor = @MajorCompetitor");
            sb.Append(" Where BasicInfoGUID = @BasicInfoGUID and PrincipalProducts = @PrincipalProducts");

            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(DataItem.BasicInfoGUID));
            cmd.Parameters.AddWithValue("@PrincipalProducts", StringHelper.NullOrEmptyStringIsDBNull(DataItem.PrincipalProducts));
            cmd.Parameters.AddWithValue("@RevenuePer", StringHelper.NullOrEmptyStringIsDBNull(DataItem.RevenuePer));
            cmd.Parameters.AddWithValue("@MOQ", StringHelper.NullOrEmptyStringIsDBNull(DataItem.MOQ));
            cmd.Parameters.AddWithValue("@SampleTime", StringHelper.NullOrEmptyStringIsDBNull(DataItem.SampleTime));
            cmd.Parameters.AddWithValue("@LeadTime", StringHelper.NullOrEmptyStringIsDBNull(DataItem.LeadTime));
            cmd.Parameters.AddWithValue("@SupAndOriName", StringHelper.NullOrEmptyStringIsDBNull(DataItem.SupAndOriName));
            cmd.Parameters.AddWithValue("@OfferPlaceCertify", StringHelper.NullOrEmptyStringIsDBNull(DataItem.OfferPlaceCertify));
            //cmd.Parameters.AddWithValue("@OfferPlaceFGUID", StringHelper.NullOrEmptyStringIsDBNull(DataItem.OfferPlaceFGUID));
            cmd.Parameters.AddWithValue("@OfferSellCertify", StringHelper.NullOrEmptyStringIsDBNull(DataItem.OfferSellCertify));
            //cmd.Parameters.AddWithValue("@OfferSellFGUID", StringHelper.NullOrEmptyStringIsDBNull(DataItem.OfferSellFGUID));
            cmd.Parameters.AddWithValue("@MajorCompetitor", StringHelper.NullOrEmptyStringIsDBNull(DataItem.MajorCompetitor));

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion

        #region Delete data item
        public static string DeleteDataItem(SqlConnection cnPortal, SystemMgmt_Traders DataItem)
        {
            return DeleteDataItem(cnPortal, DataItem, "", "");
        }

        public static string DeleteDataItem(SqlConnection cnPortal, SystemMgmt_Traders DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {

            string r = "";
            if (StringHelper.DataIsNullOrEmpty(DataItem.PrincipalProducts))
                return "Must provide PrincipalProducts.";
            else
            {
                using (SqlCommand cmd = new SqlCommand("", cnPortal)) { r = DeleteDataItemSub(cmd, DataItem); }
                if (r != "") { return r; }
                return r;
            }
        }

        private static string DeleteDataItemSub(SqlCommand cmd, SystemMgmt_Traders DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(" Delete TB_SQM_ProductDescription Where PrincipalProducts = @PrincipalProducts");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@PrincipalProducts", DataItem.PrincipalProducts);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion

        #region Upload
        public static string UploadIntroFile(SqlConnection cn, PortalUserProfile RunAsUser, FileAttachmentInfo FA, string VendorCode, string sLocalPath, string sLocalUploadPath, HttpServerUtilityBase Server, string RequestApplicationPath,string PrincipalProducts,string BasicInfoGUID)
        {
            String r = "";

            JArray ja = JArray.Parse(FA.SPEC);
            dynamic jo_item = (JObject)ja[0];

            //00.UploadFileToDB
            SqlTransaction tran = cn.BeginTransaction();
            String file = sLocalUploadPath + FA.SUBFOLDER + "/" + jo_item.FileName;
            String FGUID = SharedLibs.SqlFileStreamHelper.InsertToTableSQM(cn, tran, RunAsUser.MemberGUID, file);
            if (FGUID == "")
            {
                tran.Dispose();
                return "can't insert file to DB";
            }

            //01.del esixt file
            try
            {
                DataTable dt = new DataTable();
                StringBuilder sb = new StringBuilder();
                sb.Append(
                @"
                DELETE FROM TB_SQM_Files
                WHERE FGUID = ( SELECT OfferPlaceFGUID
                FROM TB_SQM_ProductDescription
                WHERE BasicInfoGUID = @BasicInfoGUID and PrincipalProducts=@PrincipalProducts)
                ");
                String sql = Regex.Replace(sb.ToString(), @"\s+", " ");

                using (SqlCommand cmd = new SqlCommand(sql, cn, tran))
                {
                    cmd.Parameters.Add(new SqlParameter("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID)));
                    cmd.Parameters.Add(new SqlParameter("@PrincipalProducts", StringHelper.NullOrEmptyStringIsDBNull(PrincipalProducts)));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                r = ex.ToString();
            }

            //02.Update new FGUID
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(
                @"
                UPDATE dbo.TB_SQM_ProductDescription
                  SET OfferPlaceFGUID = @OfferPlaceFGUID
                WHERE BasicInfoGUID = @BasicInfoGUID and PrincipalProducts = @PrincipalProducts
                ");
                String sql = Regex.Replace(sb.ToString(), @"\s+", " ");

                using (SqlCommand cmd = new SqlCommand(sql, cn, tran))
                {
                    cmd.Parameters.Add(new SqlParameter("@BasicInfoGUID", BasicInfoGUID));
                    cmd.Parameters.Add(new SqlParameter("@OfferPlaceFGUID", FGUID));
                    cmd.Parameters.Add(new SqlParameter("@OfferSellFGUID", FGUID));
                    cmd.Parameters.Add(new SqlParameter("@PrincipalProducts", PrincipalProducts));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                r += ex.ToString();
            }

            //Commit
            try { tran.Commit(); }
            catch (Exception e) { tran.Rollback(); r = "Upload fail.<br />Exception: " + e.ToString(); }
            return r;
        }

        public static FileInfoForOutput DownloadSQMFileByStream(SqlConnection cn, string FGUID)
        {
            StringBuilder sb = new StringBuilder();
            byte[] buffer = null;
            string sFileName = @"";
            FileInfoForOutput fi = null;
            string sFSGUID = StringHelper.EmptyOrUnescapedStringViaUrlDecode(FGUID).Trim();

            sb.Clear();
            sb.Append("SELECT FileContent.PathName(), FileName, GET_FILESTREAM_TRANSACTION_CONTEXT() ");
            sb.Append("FROM TB_SQM_Files ");
            sb.Append("WHERE FGUID = @FGUID");

            SqlTransaction tr = cn.BeginTransaction();

            using (SqlCommand cmdQueryFile = new SqlCommand(sb.ToString(), cn, tr))
            {
                cmdQueryFile.Parameters.AddWithValue("@FGUID", sFSGUID);

                try
                {
                    using (SqlDataReader dr = cmdQueryFile.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (dr.Read())
                        {
                            string sPathOfFileStreamField = dr[0].ToString();
                            sFileName = dr[1].ToString();
                            byte[] transContext = (byte[])dr[2];

                            SqlFileStream sfs = new SqlFileStream(sPathOfFileStreamField, transContext, System.IO.FileAccess.Read);

                            buffer = new byte[(int)sfs.Length];
                            sfs.Read(buffer, 0, buffer.Length);
                            sfs.Close();

                            fi = new FileInfoForOutput(buffer, sFileName);
                        }
                    }

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                }
            }

            return fi;
        }

        public static string GetCriticalFilesDetail(SqlConnection cn, PortalUserProfile RunAsUser, string BasicInfoGUID)
        {
            string urlPre = CommonHelper.urlPre;
            StringBuilder sb = new StringBuilder();
            sb.Append(
            @"
              Select * from
              (SELECT 
                        T.BasicInfoGUID,
                        T.PrincipalProducts,
                        T.RevenuePer,
                        T.MOQ,
                        T.SampleTime,
                        T.LeadTime,
                        T.SupAndOriName,
                        T.OfferPlaceCertify,T.OfferPlaceFGUID,T.OfferSellCertify,T.OfferSellFGUID,T.MajorCompetitor,
                         T1.FileName AS PlaceFN,
                        T1.UpdateTime AS PlaceTime,
                         ('<a href="""+ urlPre + @"/SQMTraders/DownloadSQMFile?DataKey='+convert(nvarchar(50), T.OfferPlaceFGUID)+'"">' + T1.FileName + '</a>') as FileUrlP
                        From TB_SQM_ProductDescription T 
                         left outer join TB_SQM_Files T1 ON T.OfferPlaceFGUID = T1.FGUID
                         where T.BasicInfoGUID =@BasicInfoGUID )  A
			            left JOIN
			            (select 
			            T.PrincipalProducts,
			            T1.FileName AS PlaceFN1,
                        T1.UpdateTime AS PlaceTime1,
			            ('<a href=""" + urlPre + @"/SQMTraders/DownloadSQMFile?DataKey='+convert(nvarchar(50), T.OfferSellFGUID)+'"">' + T1.FileName + '</a>') as FileUrlS
			            from TB_SQM_ProductDescription T
			            left outer join TB_SQM_Files T1 ON T.OfferSellFGUID = T1.FGUID 
                         where T.BasicInfoGUID =@BasicInfoGUID ) B on A.PrincipalProducts = B.PrincipalProducts
            ");
            String sql = Regex.Replace(sb.ToString(), @"\s+", " ");

            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.Add(new SqlParameter("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID)));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }



        public static string GetTradersDetailLoad(SqlConnection cn, PortalUserProfile RunAsUser, string BasicInfoGUID)
        {
            string urlPre = CommonHelper.urlPre;
            StringBuilder sb = new StringBuilder();
            sb.Append(
            @"
              Select * from
              (SELECT 
                        T.BasicInfoGUID,
                        T.PrincipalProducts,
                        T.RevenuePer,
                        T.MOQ,
                        T.SampleTime,
                        T.LeadTime,
                        T.SupAndOriName,
                        T.OfferPlaceCertify,T.OfferPlaceFGUID,T.OfferSellCertify,T.OfferSellFGUID,T.MajorCompetitor,
                         T1.FileName AS PlaceFN,
                        T1.UpdateTime AS PlaceTime,
                         ('<a href=""" + urlPre + @"/SQMTraders/DownloadSQMFile?DataKey='+convert(nvarchar(50), T.OfferPlaceFGUID)+'"">' + T1.FileName + '</a>') as FileUrlP
                        From TB_SQM_ProductDescription T 
                         left outer join TB_SQM_Files T1 ON T.OfferPlaceFGUID = T1.FGUID
                         where T.BasicInfoGUID =@BasicInfoGUID )  A
			            left JOIN
			            (select 
			            T.PrincipalProducts,
			            T1.FileName AS PlaceFN1,
                        T1.UpdateTime AS PlaceTime1,
			            ('<a href=""" + urlPre + @"/SQMTraders/DownloadSQMFile?DataKey='+convert(nvarchar(50), T.OfferSellFGUID)+'"">' + T1.FileName + '</a>') as FileUrlS
			            from TB_SQM_ProductDescription T
			            left outer join TB_SQM_Files T1 ON T.OfferSellFGUID = T1.FGUID 
                         where T.BasicInfoGUID =@BasicInfoGUID ) B on A.PrincipalProducts = B.PrincipalProducts
            ");
            String sql = Regex.Replace(sb.ToString(), @"\s+", " ");

            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.Add(new SqlParameter("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID)));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }

        #endregion

        #region Upload2
        public static string UploadIntroFile2(SqlConnection cn, PortalUserProfile RunAsUser, FileAttachmentInfo FA, string VendorCode, string sLocalPath, string sLocalUploadPath, HttpServerUtilityBase Server, string RequestApplicationPath,string PrincipalProducts,string BasicInfoGUID)
        {
            String r = "";

            JArray ja = JArray.Parse(FA.SPEC);
            dynamic jo_item = (JObject)ja[0];

            //00.UploadFileToDB
            SqlTransaction tran = cn.BeginTransaction();
            String file = sLocalUploadPath + FA.SUBFOLDER + "/" + jo_item.FileName;
            String FGUID = SharedLibs.SqlFileStreamHelper.InsertToTableSQM(cn, tran, RunAsUser.MemberGUID, file);
            if (FGUID == "")
            {
                tran.Dispose();
                return "can't insert file to DB";
            }

            //01.del esixt file
            try
            {
                DataTable dt = new DataTable();
                StringBuilder sb = new StringBuilder();
                sb.Append(
                @"
                DELETE FROM TB_SQM_Files
                WHERE FGUID = ( SELECT OfferSellFGUID
                FROM TB_SQM_ProductDescription
                WHERE BasicInfoGUID = @BasicInfoGUID and PrincipalProducts =@PrincipalProducts)
                ");
                String sql = Regex.Replace(sb.ToString(), @"\s+", " ");

                using (SqlCommand cmd = new SqlCommand(sql, cn, tran))
                {
                    cmd.Parameters.Add(new SqlParameter("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID)));
                    cmd.Parameters.Add(new SqlParameter("@PrincipalProducts", StringHelper.NullOrEmptyStringIsDBNull(PrincipalProducts)));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                r = ex.ToString();
            }

            //02.Update new FGUID
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(
                @"
                UPDATE dbo.TB_SQM_ProductDescription
                  SET OfferSellFGUID = @OfferSellFGUID
                WHERE BasicInfoGUID = @BasicInfoGUID and PrincipalProducts = @PrincipalProducts
                ");
                String sql = Regex.Replace(sb.ToString(), @"\s+", " ");

                using (SqlCommand cmd = new SqlCommand(sql, cn, tran))
                {
                    cmd.Parameters.Add(new SqlParameter("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID)));
                    cmd.Parameters.Add(new SqlParameter("@OfferPlaceFGUID", FGUID));
                    cmd.Parameters.Add(new SqlParameter("@OfferSellFGUID", FGUID));
                    cmd.Parameters.Add(new SqlParameter("@PrincipalProducts", StringHelper.NullOrEmptyStringIsDBNull(PrincipalProducts)));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                r += ex.ToString();
            }

            //Commit
            try { tran.Commit(); }
            catch (Exception e) { tran.Rollback(); r = "Upload fail.<br />Exception: " + e.ToString(); }
            return r;
        }

        public static FileInfoForOutput DownloadSQMFileByStream2(SqlConnection cn, string FGUID)
        {
            StringBuilder sb = new StringBuilder();
            byte[] buffer = null;
            string sFileName = @"";
            FileInfoForOutput fi = null;
            string sFSGUID = StringHelper.EmptyOrUnescapedStringViaUrlDecode(FGUID).Trim();

            sb.Clear();
            sb.Append("SELECT FileContent.PathName(), FileName, GET_FILESTREAM_TRANSACTION_CONTEXT() ");
            sb.Append("FROM TB_SQM_Files ");
            sb.Append("WHERE FGUID = @FGUID");

            SqlTransaction tr = cn.BeginTransaction();

            using (SqlCommand cmdQueryFile = new SqlCommand(sb.ToString(), cn, tr))
            {
                cmdQueryFile.Parameters.AddWithValue("@FGUID", sFSGUID);

                try
                {
                    using (SqlDataReader dr = cmdQueryFile.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (dr.Read())
                        {
                            string sPathOfFileStreamField = dr[0].ToString();
                            sFileName = dr[1].ToString();
                            byte[] transContext = (byte[])dr[2];

                            SqlFileStream sfs = new SqlFileStream(sPathOfFileStreamField, transContext, System.IO.FileAccess.Read);

                            buffer = new byte[(int)sfs.Length];
                            sfs.Read(buffer, 0, buffer.Length);
                            sfs.Close();

                            fi = new FileInfoForOutput(buffer, sFileName);
                        }
                    }

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                }
            }

            return fi;
        }

        public static string GetCriticalFilesDetail2(SqlConnection cn, PortalUserProfile RunAsUser, String BasicInfoGUID)
        {
            string urlPre = CommonHelper.urlPre;
            StringBuilder sb = new StringBuilder();
            sb.Append(
            @"
            SELECT 
            T.BasicInfoGUID,
            T.PrincipalProducts,
            T.RevenuePer,
            T.MOQ,
            T.SampleTime,
            T.LeadTime,
            T.SupAndOriName,
            T.OfferPlaceCertify,T.OfferPlaceFGUID,T.OfferSellCertify,T.OfferSellFGUID,T.MajorCompetitor,
             T1.FileName AS PlaceFN,
            T1.UpdateTime AS PlaceTime,
             ('<a href=""" + urlPre + @"/SQMTraders/DownloadSQMFile?DataKey='+convert(nvarchar(50), T.OfferSellFGUID)+'"">' + T1.FileName + '</a>') as FileUrlTag
            From TB_SQM_ProductDescription T 
             left outer join TB_SQM_Files T1 ON T.OfferSellFGUID = T1.FGUID
             where T.BasicInfoGUID = @BasicInfoGUID
            ");
            String sql = Regex.Replace(sb.ToString(), @"\s+", " ");

            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.Add(new SqlParameter("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID)));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }
        #endregion



    }
}
