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

namespace Lib_SQM_Domain.Modal
{
    

    #region Data Class Definitions
    public class SystemMgmt_Agents
    {
        protected string _BasicInfoGUID;
        protected string _PrincipalProducts;
        protected string _RevenuePer;
        protected string _MOQ;
        protected string _SampleTime;
        protected string _LeadTime;
        protected string _ProductBrand;
        protected string _SupAndOriName;
        protected string _SupAndOriPlace;
        protected string _OfferProxyCertify;
        protected string _OfferProxyFGUID;
        protected string _MajorCompetitor;


        public string BasicInfoGUID { get { return this._BasicInfoGUID; } set { this._BasicInfoGUID = value; } }
        public string PrincipalProducts { get { return this._PrincipalProducts; } set { this._PrincipalProducts = value; } }
        public string RevenuePer { get { return this._RevenuePer; } set { this._RevenuePer = value; } }
        public string MOQ { get { return this._MOQ; } set { this._MOQ = value; } }
        public string SampleTime { get { return this._SampleTime; } set { this._SampleTime = value; } }
        public string LeadTime { get { return this._LeadTime; } set { this._LeadTime = value; } }
        public string ProductBrand { get { return this._ProductBrand; } set { this._ProductBrand = value; } }
        public string SupAndOriName { get { return this._SupAndOriName; } set { this._SupAndOriName = value; } }
        public string SupAndOriPlace { get { return this._SupAndOriPlace; } set { this._SupAndOriPlace = value; } }
        public string OfferProxyCertify { get { return this._OfferProxyCertify; } set { this._OfferProxyCertify = value; } }
        public string OfferProxyFGUID { get { return this._OfferProxyFGUID; } set { this._OfferProxyFGUID = value; } }
        public string MajorCompetitor { get { return this._MajorCompetitor; } set { this._MajorCompetitor = value; } }


        public SystemMgmt_Agents() { }
        public SystemMgmt_Agents(string BasicInfoGUID, string PrincipalProducts, string RevenuePer, string MOQ, string SampleTime, string LeadTime, string ProductBrand, string SupAndOriName, string SupAndOriPlace, string OfferProxyCertify, string OfferProxyFGUID, string MajorCompetitor)
        {
            this._BasicInfoGUID = BasicInfoGUID;
            this._PrincipalProducts = PrincipalProducts;
            this._RevenuePer = RevenuePer;
            this._MOQ = MOQ;
            this._SampleTime = SampleTime;
            this._LeadTime = LeadTime;
            this._ProductBrand = ProductBrand;
            this._SupAndOriName = SupAndOriName;
            this._SupAndOriPlace = SupAndOriPlace;
            this._OfferProxyCertify = OfferProxyCertify;
            this._OfferProxyFGUID = OfferProxyFGUID;
            this._MajorCompetitor = MajorCompetitor;
        }
    }


    public class SystemMgmt_Agents_jQGridJSon
    {
        public List<SystemMgmt_Agents> Rows = new List<SystemMgmt_Agents>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }
    #endregion

    public static class SystemMgmt_Agents_Helper
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
            String vendorCode = "";
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@MemberGUID", RunAsUser.MemberGUID));
                var vendorCodeScale = cmd.ExecuteScalar();
                vendorCode = (vendorCodeScale == null ? "" : vendorCodeScale.ToString());
            }
            return vendorCode;
        }


        #region SearchSubFunc
        public static string GetDataToJQGridJson(SqlConnection cn)
        {
            return GetDataToJQGridJson(cn, "");
        }

        public static string GetDataToJQGridJson(SqlConnection cn, string SearchText)
        {
            SystemMgmt_Agents_jQGridJSon m = new SystemMgmt_Agents_jQGridJSon();

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
            sb.Append(" ProductBrand,SupAndOriName,SupAndOriPlace,OfferProxyCertify,OfferProxyFGUID,MajorCompetitor");
            sb.Append(" From TB_SQM_ProductDescription ");
            sb.Append(sWhereClause + ";");
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                if (sSearchText != "")
                    cmd.Parameters.Add(new SqlParameter("@SearchText", sSearchText));
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    iRowCount++;
                    m.Rows.Add(new SystemMgmt_Agents(
                        dr["BasicInfoGUID"].ToString(),
                        dr["PrincipalProducts"].ToString(),
                        dr["RevenuePer"].ToString(),
                        dr["MOQ"].ToString(),
                        dr["SampleTime"].ToString(),
                        dr["LeadTime"].ToString(),
                        dr["ProductBrand"].ToString(),
                        dr["SupAndOriName"].ToString(),
                        dr["SupAndOriPlace"].ToString(),
                        dr["OfferProxyCertify"].ToString(),
                        dr["OfferProxyFGUID"].ToString(),
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
        private static void UnescapeDataFromWeb(SystemMgmt_Agents DataItem)
        {
            DataItem.BasicInfoGUID = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.BasicInfoGUID);
            DataItem.PrincipalProducts = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.PrincipalProducts);
            DataItem.ProductBrand = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ProductBrand);
            DataItem.MajorCompetitor = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.MajorCompetitor);
            DataItem.MOQ = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.MOQ);
            DataItem.SupAndOriName = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.SupAndOriName);
            DataItem.SupAndOriPlace = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.SupAndOriPlace);

        }

        private static string DataCheck(SystemMgmt_Agents DataItem)
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
            if (StringHelper.DataIsNullOrEmpty(DataItem.ProductBrand))
                e.Add("Must provide ProductBrand.");
            if (StringHelper.DataIsNullOrEmpty(DataItem.SupAndOriName))
                e.Add("Must provide SupAndOriName.");
            if (StringHelper.DataIsNullOrEmpty(DataItem.SupAndOriPlace))
                e.Add("Must provide SupAndOriPlace.");
            if (StringHelper.DataIsNullOrEmpty(DataItem.OfferProxyCertify))
                e.Add("Must provide OfferProxyCertify.");

            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }
        #endregion

        #region Create data item
        public static string CreateDataItem(SqlConnection cnPortal, SystemMgmt_Agents DataItem)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            if (r != "")
            { return r;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Insert Into TB_SQM_ProductDescription (BasicInfoGUID,PrincipalProducts,RevenuePer,MOQ,SampleTime,LeadTime,");
                sb.Append(" ProductBrand,SupAndOriName,SupAndOriPlace,OfferProxyCertify,MajorCompetitor)");
                sb.Append(" Values (@BasicInfoGUID,@PrincipalProducts, @RevenuePer,@MOQ,@SampleTime,@LeadTime,");
                sb.Append(" @ProductBrand,@SupAndOriName,@SupAndOriPlace,@OfferProxyCertify,@MajorCompetitor)");
                SQM_Basic_Helper.InsertPart(sb, "3");
                SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal);
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(DataItem.BasicInfoGUID));
                cmd.Parameters.AddWithValue("@PrincipalProducts", StringHelper.NullOrEmptyStringIsDBNull(DataItem.PrincipalProducts));
                cmd.Parameters.AddWithValue("@RevenuePer", StringHelper.NullOrEmptyStringIsDBNull(DataItem.RevenuePer));
                cmd.Parameters.AddWithValue("@MOQ", StringHelper.NullOrEmptyStringIsDBNull(DataItem.MOQ));
                cmd.Parameters.AddWithValue("@SampleTime", StringHelper.NullOrEmptyStringIsDBNull(DataItem.SampleTime));
                cmd.Parameters.AddWithValue("@LeadTime", StringHelper.NullOrEmptyStringIsDBNull(DataItem.LeadTime));
                cmd.Parameters.AddWithValue("@ProductBrand", StringHelper.NullOrEmptyStringIsDBNull(DataItem.ProductBrand));
                cmd.Parameters.AddWithValue("@SupAndOriName", StringHelper.NullOrEmptyStringIsDBNull(DataItem.SupAndOriName));
                cmd.Parameters.AddWithValue("@SupAndOriPlace", StringHelper.NullOrEmptyStringIsDBNull(DataItem.SupAndOriPlace));
                cmd.Parameters.AddWithValue("@OfferProxyCertify", StringHelper.NullOrEmptyStringIsDBNull(DataItem.OfferProxyCertify));
                //cmd.Parameters.AddWithValue("@OfferProxyFGUID", Guid.NewGuid());
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
        public static string EditDataItem(SqlConnection cnPortal, SystemMgmt_Agents DataItem)
        {
            return EditDataItem(cnPortal, DataItem, "", "");
        }

        public static string EditDataItem(SqlConnection cnPortal, SystemMgmt_Agents DataItem, string LoginMemberGUID, string RunAsMemberGUID)
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

        private static string EditDataItemSub(SqlCommand cmd, SystemMgmt_Agents DataItem)
        {
            string sErrMsg = "";


            StringBuilder sb = new StringBuilder();
            sb.Append("Update TB_SQM_ProductDescription Set");
            sb.Append(" RevenuePer= @RevenuePer,MOQ = @MOQ,SampleTime = @SampleTime,LeadTime = @LeadTime,");
            sb.Append(" ProductBrand= @ProductBrand,SupAndOriName=@SupAndOriName,SupAndOriPlace=@SupAndOriPlace,");
            sb.Append(" OfferProxyCertify = @OfferProxyCertify,MajorCompetitor = @MajorCompetitor");
            sb.Append(" Where BasicInfoGUID = @BasicInfoGUID and PrincipalProducts = @PrincipalProducts");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(DataItem.BasicInfoGUID));
            cmd.Parameters.AddWithValue("@PrincipalProducts", StringHelper.NullOrEmptyStringIsDBNull(DataItem.PrincipalProducts));
            cmd.Parameters.AddWithValue("@RevenuePer", StringHelper.NullOrEmptyStringIsDBNull(DataItem.RevenuePer));
            cmd.Parameters.AddWithValue("@MOQ", StringHelper.NullOrEmptyStringIsDBNull(DataItem.MOQ));
            cmd.Parameters.AddWithValue("@SampleTime", StringHelper.NullOrEmptyStringIsDBNull(DataItem.SampleTime));
            cmd.Parameters.AddWithValue("@LeadTime", StringHelper.NullOrEmptyStringIsDBNull(DataItem.LeadTime));
            cmd.Parameters.AddWithValue("@ProductBrand", StringHelper.NullOrEmptyStringIsDBNull(DataItem.ProductBrand));
            cmd.Parameters.AddWithValue("@SupAndOriName", StringHelper.NullOrEmptyStringIsDBNull(DataItem.SupAndOriName));
            cmd.Parameters.AddWithValue("@SupAndOriPlace", StringHelper.NullOrEmptyStringIsDBNull(DataItem.SupAndOriPlace));
            cmd.Parameters.AddWithValue("@OfferProxyCertify", StringHelper.NullOrEmptyStringIsDBNull(DataItem.OfferProxyCertify));
            cmd.Parameters.AddWithValue("@MajorCompetitor", StringHelper.NullOrEmptyStringIsDBNull(DataItem.MajorCompetitor));

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion

        #region Delete data item
        public static string DeleteDataItem(SqlConnection cnPortal, SystemMgmt_Agents DataItem)
        {
            return DeleteDataItem(cnPortal, DataItem, "", "");
        }

        public static string DeleteDataItem(SqlConnection cnPortal, SystemMgmt_Agents DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {

            string r = "";
            if (StringHelper.DataIsNullOrEmpty(DataItem.PrincipalProducts))
                return "Must provide PrincipalProducts.";
            else
            {
                using (SqlCommand cmd = new SqlCommand("", cnPortal)) {r = DeleteDataItemSub(cmd, DataItem);}
                if (r != "") { return r; }
                return r;
            }
        }

        private static string DeleteDataItemSub(SqlCommand cmd, SystemMgmt_Agents DataItem)
        {
            string sErrMsg = "";


            StringBuilder sb = new StringBuilder();
            sb.Append("Delete TB_SQM_ProductDescription Where PrincipalProducts = @PrincipalProducts");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@PrincipalProducts", DataItem.PrincipalProducts);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion

        #region Upload
        public static string UploadIntroFile(SqlConnection cn, PortalUserProfile RunAsUser, FileAttachmentInfo FA, string VendorCode, string sLocalPath, string sLocalUploadPath, HttpServerUtilityBase Server, string RequestApplicationPath,string PrincipalProducts, string BasicInfoGUID)
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
                WHERE FGUID = ( SELECT OfferProxyFGUID
                FROM TB_SQM_ProductDescription
                WHERE BasicInfoGUID = @BasicInfoGUID and PrincipalProducts = @PrincipalProducts)
                ");
                String sql = Regex.Replace(sb.ToString(), @"\s+", " ");

                using (SqlCommand cmd = new SqlCommand(sql, cn, tran))
                {
                    cmd.Parameters.Add(new SqlParameter("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsEmptyString(BasicInfoGUID)));
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
                  SET OfferProxyFGUID = @OfferProxyFGUID
                WHERE BasicInfoGUID = @BasicInfoGUID and PrincipalProducts = @PrincipalProducts
                ");
                String sql = Regex.Replace(sb.ToString(), @"\s+", " ");

                using (SqlCommand cmd = new SqlCommand(sql, cn, tran))
                {
                    cmd.Parameters.Add(new SqlParameter("@OfferProxyFGUID", FGUID));
                    cmd.Parameters.Add(new SqlParameter("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsEmptyString(BasicInfoGUID)));
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
            SELECT 
            T.BasicInfoGUID,
            T.PrincipalProducts,
            T.RevenuePer,
            T.MOQ,
            T.SampleTime,
            T.LeadTime,
            T.ProductBrand,
            T.SupAndOriName,T.SupAndOriPlace,T.OfferProxyCertify,T.MajorCompetitor,
            T1.UpdateTime as ProxyTime,
            T1.FileName AS ProxyFN,
            ('<a href=""" + urlPre + @"/SQMBasic/DownloadSQMFile?DataKey='+convert(nvarchar(50), T.OfferProxyFGUID)+'"">' + T1.FileName + '</a>') as FileUrlTag
            From TB_SQM_ProductDescription T 
            left outer join TB_SQM_Files T1 ON T.OfferProxyFGUID = T1.FGUID
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

        public static string GetAgentsDetailLoad(SqlConnection cn, PortalUserProfile RunAsUser, string BasicInfoGUID)
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
            T.ProductBrand,
            T.SupAndOriName,T.SupAndOriPlace,T.OfferProxyCertify,T.MajorCompetitor,
            T1.UpdateTime as ProxyTime,
            T1.FileName AS ProxyFN,
            ('<a href=""" + urlPre + @"/SQMBasic/DownloadSQMFile?DataKey='+convert(nvarchar(50), T.OfferProxyFGUID)+'"">' + T1.FileName + '</a>') as FileUrlTag
            From TB_SQM_ProductDescription T 
            left outer join TB_SQM_Files T1 ON T.OfferProxyFGUID = T1.FGUID
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

    #region Data Class Definitions
    public class SystemMgmt_AgentsA2
    {
        protected string _BasicInfoGUID;
        protected string _PrincipalProducts;
        protected string _FactoryName;
        protected string _ProductBrand;
        protected string _FactoryAddress;
        protected string _FactoryNum;
        protected string _FactoryDate;
        protected string _ProductLine;

        public string BasicInfoGUID { get { return this._BasicInfoGUID; } set { this._BasicInfoGUID = value; } }
        public string PrincipalProducts { get { return this._PrincipalProducts; } set { this._PrincipalProducts = value; } }
        public string FactoryName { get { return this._FactoryName; } set { this._FactoryName = value; } }
        public string ProductBrand { get { return this._ProductBrand; } set { this._ProductBrand = value; } }
        public string FactoryAddress { get { return this._FactoryAddress; } set { this._FactoryAddress = value; } }
        public string FactoryNum { get { return this._FactoryNum; } set { this._FactoryNum = value; } }
        public string FactoryDate { get { return this._FactoryDate; } set { this._FactoryDate = value; } }
        public string ProductLine { get { return this._ProductLine; } set { this._ProductLine = value; } }

        public SystemMgmt_AgentsA2() { }
        public SystemMgmt_AgentsA2(string BasicInfoGUID, string PrincipalProducts, string FactoryName, string ProductBrand, string FactoryAddress, string FactoryNum, string FactoryDate, string ProductLine)
        {
            this._BasicInfoGUID = BasicInfoGUID;
            this._PrincipalProducts = PrincipalProducts;
            this._FactoryName = FactoryName;
            this._ProductBrand = ProductBrand;
            this._FactoryAddress = FactoryAddress;
            this._FactoryNum = FactoryNum;
            this._FactoryDate = FactoryDate;
            this._ProductLine = ProductLine;
        }
    }


    public class SystemMgmt_AgentsA2_jQGridJSon
    {
        public List<SystemMgmt_AgentsA2> Rows = new List<SystemMgmt_AgentsA2>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }
    #endregion


    public static class SystemMgmt_AgentsA2_Helper
    { 


        #region SearchSubFunc
        public static string GetDataToJQGridJson(SqlConnection cn)
        {
            return GetDataToJQGridJson(cn, "");
        }

        public static string GetDataToJQGridJson(SqlConnection cn, string SearchText)
        {
            SystemMgmt_AgentsA2_jQGridJSon m = new SystemMgmt_AgentsA2_jQGridJSon();

            string sSearchText = SearchText.Trim();
            string sWhereClause = "";
            if (sSearchText != "")
                sWhereClause += "  FactoryName = @SearchText ";
            if (sWhereClause.Length != 0)
                sWhereClause = " Where" + sWhereClause.Substring(0);

            m.Rows.Clear();
            int iRowCount = 0;

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Top 100 BasicInfoGUID,PrincipalProducts,FactoryName,ProductBrand,FactoryAddress,FactoryNum,");
            sb.Append("  FactoryDate,ProductLine");
            sb.Append("  From TB_SQM_Agent_Vendor ");
            sb.Append(sWhereClause + ";");
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                if (sSearchText != "")
                    cmd.Parameters.Add(new SqlParameter("@SearchText", sSearchText));
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    iRowCount++;
                    m.Rows.Add(new SystemMgmt_AgentsA2(
                        dr["BasicInfoGUID"].ToString(),
                        dr["PrincipalProducts"].ToString(),
                        dr["FactoryName"].ToString(),
                        dr["ProductBrand"].ToString(),
                        dr["FactoryAddress"].ToString(),
                        dr["FactoryNum"].ToString(),
                        dr["FactoryDate"].ToString(),
                        dr["ProductLine"].ToString()
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
        private static void UnescapeDataFromWeb(SystemMgmt_AgentsA2 DataItem)
        {
            DataItem.BasicInfoGUID = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.BasicInfoGUID);
            DataItem.ProductLine = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ProductLine);
            DataItem.FactoryName = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.FactoryName);
            DataItem.PrincipalProducts = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.PrincipalProducts);
            DataItem.ProductBrand = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ProductBrand);
            DataItem.FactoryAddress = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.FactoryAddress);

        }

        private static string DataCheck(SystemMgmt_AgentsA2 DataItem)
        {
            string r = "";
            List<string> e = new List<string>();
            if (StringHelper.DataIsNullOrEmpty(DataItem.PrincipalProducts))
                e.Add("Must provide PrincipalProducts_A.");

            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }
        #endregion

        #region Create data item
        public static string CreateDataItem(SqlConnection cnPortal, SystemMgmt_AgentsA2 DataItem)
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
                sb.Append("Insert Into TB_SQM_Agent_Vendor (BasicInfoGUID,PrincipalProducts,FactoryName,ProductBrand,");
                sb.Append(" FactoryAddress,FactoryNum,FactoryDate,ProductLine)");
                sb.Append(" Values (@BasicInfoGUID,@PrincipalProducts, @FactoryName,@ProductBrand,@FactoryAddress,@FactoryNum,");
                sb.Append(" @FactoryDate,@ProductLine)");
                SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal);
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(DataItem.BasicInfoGUID));
                cmd.Parameters.AddWithValue("@PrincipalProducts", StringHelper.NullOrEmptyStringIsDBNull(DataItem.PrincipalProducts));
                cmd.Parameters.AddWithValue("@FactoryName", StringHelper.NullOrEmptyStringIsDBNull(DataItem.FactoryName));
                cmd.Parameters.AddWithValue("@ProductBrand", StringHelper.NullOrEmptyStringIsDBNull(DataItem.ProductBrand));
                cmd.Parameters.AddWithValue("@FactoryAddress", StringHelper.NullOrEmptyStringIsDBNull(DataItem.FactoryAddress));
                cmd.Parameters.AddWithValue("@FactoryNum", StringHelper.NullOrEmptyStringIsDBNull(DataItem.FactoryNum));
                cmd.Parameters.AddWithValue("@FactoryDate", StringHelper.NullOrEmptyStringIsDBNull(DataItem.FactoryDate));
                cmd.Parameters.AddWithValue("@ProductLine", StringHelper.NullOrEmptyStringIsDBNull(DataItem.ProductLine));


                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;

                return sErrMsg;
            }
        }
        #endregion

        #region Edit data item
        public static string EditDataItem(SqlConnection cnPortal, SystemMgmt_AgentsA2 DataItem)
        {
            return EditDataItem(cnPortal, DataItem, "", "");
        }

        public static string EditDataItem(SqlConnection cnPortal, SystemMgmt_AgentsA2 DataItem, string LoginMemberGUID, string RunAsMemberGUID)
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

        private static string EditDataItemSub(SqlCommand cmd, SystemMgmt_AgentsA2 DataItem)
        {
            string sErrMsg = "";


            StringBuilder sb = new StringBuilder();
            sb.Append("Update TB_SQM_Agent_Vendor Set");
            sb.Append(" FactoryName= @FactoryName,ProductBrand = @ProductBrand,FactoryAddress = @FactoryAddress,");
            sb.Append(" FactoryNum= @FactoryNum,FactoryDate=@FactoryDate,ProductLine=@ProductLine");
            sb.Append("  Where BasicInfoGUID = @BasicInfoGUID and PrincipalProducts = @PrincipalProducts");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(DataItem.BasicInfoGUID));
            cmd.Parameters.AddWithValue("@PrincipalProducts", StringHelper.NullOrEmptyStringIsDBNull(DataItem.PrincipalProducts));
            cmd.Parameters.AddWithValue("@FactoryName", StringHelper.NullOrEmptyStringIsDBNull(DataItem.FactoryName));
            cmd.Parameters.AddWithValue("@ProductBrand", StringHelper.NullOrEmptyStringIsDBNull(DataItem.ProductBrand));
            cmd.Parameters.AddWithValue("@FactoryAddress", StringHelper.NullOrEmptyStringIsDBNull(DataItem.FactoryAddress));
            cmd.Parameters.AddWithValue("@FactoryNum", StringHelper.NullOrEmptyStringIsDBNull(DataItem.FactoryNum));
            cmd.Parameters.AddWithValue("@FactoryDate", StringHelper.NullOrEmptyStringIsDBNull(DataItem.FactoryDate));
            cmd.Parameters.AddWithValue("@ProductLine", StringHelper.NullOrEmptyStringIsDBNull(DataItem.ProductLine));

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion

        #region Delete data item
        public static string DeleteDataItem(SqlConnection cnPortal, SystemMgmt_AgentsA2 DataItem)
        {
            return DeleteDataItem(cnPortal, DataItem, "", "");
        }

        public static string DeleteDataItem(SqlConnection cnPortal, SystemMgmt_AgentsA2 DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {

            string r = "";
            if (StringHelper.DataIsNullOrEmpty(DataItem.FactoryName))
                return "Must provide FactoryName.";
            else
            {
                using (SqlCommand cmd = new SqlCommand("", cnPortal)) { r = DeleteDataItemSub(cmd, DataItem); }
                if (r != "") { return r; }
                return r;
            }
        }

        private static string DeleteDataItemSub(SqlCommand cmd, SystemMgmt_AgentsA2 DataItem)
        {
            string sErrMsg = "";


            StringBuilder sb = new StringBuilder();
            sb.Append("Delete TB_SQM_Agent_Vendor Where FactoryName = @FactoryName");
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@FactoryName", DataItem.FactoryName);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion




    }


}
