using Lib_Portal_Domain.SharedLibs;
using Lib_SQM_Domain.Modal;
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

    public class SQM_CustomersMgmt
    {
 
        protected string _BasicInfoGUID;
        protected string _OEMCustomerName;
        protected string _BusinessCategory;
        protected string _RevenuePer;
        protected string _MajorMaterials;
        protected string _MajorSupplier;
        protected string _PurchaseRevenuePer;
    
        public string BasicInfoGUID { get { return this._BasicInfoGUID; } set { this._BasicInfoGUID = value; } }
        public string OEMCustomerName { get { return this._OEMCustomerName; } set { this._OEMCustomerName = value; } }
        public string BusinessCategory { get { return this._BusinessCategory; } set { this._BusinessCategory = value; } }
        public string RevenuePer { get { return this._RevenuePer; } set { this._RevenuePer = value; } }
        public string MajorMaterials { get { return this._MajorMaterials; } set { this._MajorMaterials = value; } }
        public string MajorSupplier { get { return this._MajorSupplier; } set { this._MajorSupplier = value; } }
        public string PurchaseRevenuePer { get { return this._PurchaseRevenuePer; } set { this._PurchaseRevenuePer = value; } }
    

        public SQM_CustomersMgmt() { }

        public SQM_CustomersMgmt(string BasicInfoGUID, string OEMCustomerName, string BusinessCategory, string RevenuePer, string MajorMaterials, string MajorSupplier, string PurchaseRevenuePer)
        {
            this._BasicInfoGUID = BasicInfoGUID;
            this._OEMCustomerName = OEMCustomerName;
            this._BusinessCategory = BusinessCategory;
            this._RevenuePer = RevenuePer;
            this._MajorMaterials = MajorMaterials;
            this._MajorSupplier = MajorSupplier;
            this._PurchaseRevenuePer = PurchaseRevenuePer;
        }
    }
    public class SQM_CustomersMgmt_jQGridJSon
    {
        public List<SQM_CustomersMgmt> Rows = new List<SQM_CustomersMgmt>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }
    public static class SQM_CustomersMgmt_Helper
    {
        #region Create/Edit data check
        private static void UnescapeDataFromWeb(SQM_CustomersMgmt DataItem)
        {
            DataItem.OEMCustomerName = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.OEMCustomerName);
            DataItem.BusinessCategory = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.BusinessCategory);
            DataItem.RevenuePer = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.RevenuePer);
            DataItem.MajorMaterials = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.MajorMaterials);
            DataItem.MajorSupplier = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.MajorSupplier);
            DataItem.PurchaseRevenuePer = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.PurchaseRevenuePer);
            
        }
        private static string DataCheck(SQM_CustomersMgmt DataItem)
        {
            string r = "";
            List<string> e = new List<string>();

            if (StringHelper.DataIsNullOrEmpty(DataItem.OEMCustomerName))
                e.Add("Must provide OEMCustomerName.");
            if (StringHelper.DataIsNullOrEmpty(DataItem.BusinessCategory))
                e.Add("Must provide BusinessCategory.");
            if (StringHelper.DataIsNullOrEmpty(DataItem.RevenuePer))
                e.Add("Must provide RevenuePer.");
            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }

        public static string HPVendorNumUpdate(SqlConnection sqlConnection, string BasicInfoGUID, string hPVendorNum, string LoginMemberGUID, string RunAsMemberGUID)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append("Update TB_SQM_Manufacturers_BasicInfo Set  HPVendorNum=@HPVendorNum");
            sb.Append("  Where BasicInfoGUID = @BasicInfoGUID");
            using (SqlCommand cmd = new SqlCommand("", sqlConnection))
            {
                cmd.CommandText = sb.ToString();
         
                cmd.Parameters.AddWithValue("@HPVendorNum", StringHelper.NullOrEmptyStringIsDBNull(hPVendorNum));
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID));
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }
            }
            return sErrMsg;
        }
        #endregion

        public static string DeleteDataItem(SqlConnection cnPortal, SQM_CustomersMgmt DataItem)
        {
            return DeleteDataItem(cnPortal, DataItem, "", "");
        }
        public static string DeleteDataItem(SqlConnection cnPortal, SQM_CustomersMgmt DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            string r = "";
            if (StringHelper.DataIsNullOrEmpty(DataItem.BasicInfoGUID))
                return "Must provide BasicInfoGUID.";
            if (StringHelper.DataIsNullOrEmpty(DataItem.OEMCustomerName))
            {
                return "Must provide OEMCustomerName.";
            }
            else
            {
                using (SqlCommand cmd = new SqlCommand("", cnPortal)) { r = DeleteDataItemSub(cmd, DataItem); }
                if (r != "") { return r; }
                return r;
            }
        }
        private static string DeleteDataItemSub(SqlCommand cmd, SQM_CustomersMgmt DataItem)
        {
            string sErrMsg = "";

            string sSQL = "Delete TB_SQM_Customers Where BasicInfoGUID = @BasicInfoGUID and OEMCustomerName=@OEMCustomerName;";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.BasicInfoGUID));
            cmd.Parameters.AddWithValue("@OEMCustomerName", StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.OEMCustomerName));
            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        
       public static string GetDataToJQGridJson(SqlConnection cn,string SearchText)
        {
            SQM_CustomersMgmt_jQGridJSon m = new SQM_CustomersMgmt_jQGridJSon();
            m.Rows.Clear();
            int iRowCount = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT [TB_SQM_Customers].[BasicInfoGUID]
      ,[OEMCustomerName]
      ,[BusinessCategory]
      ,[RevenuePer]
      ,[MajorMaterials]
      ,[MajorSupplier]
      ,[PurchaseRevenuePer]
      ,[TB_SQM_Manufacturers_BasicInfo].[HPVendorNum]
        FROM[TB_SQM_Customers]
        inner join[dbo].[TB_SQM_Manufacturers_BasicInfo]
        on[TB_SQM_Manufacturers_BasicInfo].[BasicInfoGUID]=[TB_SQM_Customers].[BasicInfoGUID]
        ");
            if (!string.IsNullOrEmpty(SearchText))
            {
                sb.Append( " where [TB_SQM_Customers].[BasicInfoGUID] = @BasicInfoGUID");
            }
            
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(SearchText)));

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    iRowCount++;
                    m.Rows.Add(new SQM_CustomersMgmt(
                         dr["BasicInfoGUID"].ToString(),
                         dr["OEMCustomerName"].ToString(),
                         dr["BusinessCategory"].ToString(),
                         dr["RevenuePer"].ToString(),
                         dr["MajorMaterials"].ToString(),
                         dr["MajorSupplier"].ToString(),
                         dr["PurchaseRevenuePer"].ToString()
                  ));
                }
                dr.Close();
                dr = null;
            }

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(m);

        }
      

        public static string GetDataToJQGridJson(SqlConnection cn)
        {
            return GetDataToJQGridJson(cn, "");
        }
        public static string CreateDataItem(SqlConnection cnPortal, SQM_CustomersMgmt DataItem)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);

            if (r != "")
            { return r; }
            if (isOver(cnPortal, DataItem.BasicInfoGUID))
            {
                return "OEM客戶最多填寫3个";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"INSERT INTO [dbo].[TB_SQM_Customers]
           ([BasicInfoGUID]
           ,[OEMCustomerName]
           ,[BusinessCategory]
           ,[RevenuePer]
           ,[MajorMaterials]
           ,[MajorSupplier]
           ,[PurchaseRevenuePer]
           )
     VALUES (@BasicInfoGUID,@OEMCustomerName,@BusinessCategory,@RevenuePer,
       @MajorMaterials,@MajorSupplier,@PurchaseRevenuePer
      ); ");
                SQM_Basic_Helper.InsertPart(sb, "4");
                SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal);


                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(DataItem.BasicInfoGUID));
                cmd.Parameters.AddWithValue("@OEMCustomerName", StringHelper.NullOrEmptyStringIsDBNull(DataItem.OEMCustomerName));
                cmd.Parameters.AddWithValue("@BusinessCategory", StringHelper.NullOrEmptyStringIsDBNull(DataItem.BusinessCategory));
                cmd.Parameters.AddWithValue("@RevenuePer", StringHelper.NullOrEmptyStringIsDBNull(DataItem.RevenuePer));
                cmd.Parameters.AddWithValue("@MajorMaterials", StringHelper.NullOrEmptyStringIsDBNull(DataItem.MajorMaterials));
                cmd.Parameters.AddWithValue("@MajorSupplier", StringHelper.NullOrEmptyStringIsDBNull(DataItem.MajorSupplier));
                cmd.Parameters.AddWithValue("@PurchaseRevenuePer", StringHelper.NullOrEmptyStringIsDBNull(DataItem.PurchaseRevenuePer));
             
                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;

                return sErrMsg;
            }
        }
        public static bool isOver(SqlConnection cnPortal,string BasicInfoGUID)
        {
            bool isover = false;
            StringBuilder sb = new StringBuilder();
            DataTable dt = new DataTable();
            sb.Append(@" select OEMCustomerName FROM TB_SQM_Customers  where BasicInfoGUID=@BasicInfoGUID");
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal))
            {
                cmd.Parameters.AddWithValue("@BasicInfoGUID", BasicInfoGUID);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            isover = dt.Rows.Count >2;
            return isover;
        }
        public static string EditDataItem(SqlConnection cnPortal, SQM_CustomersMgmt DataItem)
        {
            return EditDataItem(cnPortal, DataItem, "", "");
        }
        public static string EditDataItem(SqlConnection cnPortal, SQM_CustomersMgmt DataItem, string LoginMemberGUID, string RunAsMemberGUID)
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

        private static string EditDataItemSub(SqlCommand cmd, SQM_CustomersMgmt DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append("Update TB_SQM_Customers Set  BusinessCategory=@BusinessCategory,RevenuePer=@RevenuePer,MajorMaterials=@MajorMaterials, ");
            sb.Append("MajorSupplier=@MajorSupplier,PurchaseRevenuePer=@PurchaseRevenuePer");
            sb.Append(" Where BasicInfoGUID = @BasicInfoGUID and OEMCustomerName=@OEMCustomerName");

            cmd.CommandText = sb.ToString();
             cmd.Parameters.AddWithValue("@BusinessCategory", StringHelper.NullOrEmptyStringIsDBNull(DataItem.BusinessCategory));
            cmd.Parameters.AddWithValue("@RevenuePer", StringHelper.NullOrEmptyStringIsDBNull(DataItem.RevenuePer));
            cmd.Parameters.AddWithValue("@MajorMaterials", StringHelper.NullOrEmptyStringIsDBNull(DataItem.MajorMaterials));
            cmd.Parameters.AddWithValue("@MajorSupplier", StringHelper.NullOrEmptyStringIsDBNull(DataItem.MajorSupplier));
            cmd.Parameters.AddWithValue("@PurchaseRevenuePer", StringHelper.NullOrEmptyStringIsDBNull(DataItem.PurchaseRevenuePer));
            cmd.Parameters.AddWithValue("@OEMCustomerName", DataItem.OEMCustomerName);
            cmd.Parameters.AddWithValue("@BasicInfoGUID", DataItem.BasicInfoGUID);
            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }

    }
}
