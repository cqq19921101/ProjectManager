using Lib_Portal_Domain.SharedLibs;
using Lib_SQM_Domain.Modal;
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
    public class SQM_EquipmentMgmt
    {
        protected string _SID;
        protected string _BasicInfoGUID;
        protected string _TB_SQM_Equipment_TypeCID;
        protected string _TB_SQM_Equipment_Special_TypeSCID;
        protected string _DeviceName;
        protected string _DeviceQuantity;
        protected string _DeviceArea;
        protected string _DeviceCapacity;
        protected string _DevicePrecision;
        protected string _TestItem;
        protected string _DatePurchased;
        protected string _DateMade;
        protected string _Model;
        protected string _Brand;
        protected string _Weight;
        protected string _ModelSize;
        protected string _LineLength;
        protected string _GunQty;
        protected string _RoastQty;
        protected string _PlatingType;
        protected string _Anodized;
        protected string _SprayingType;
        protected string _PlatingCapacity;
        public string SID { get { return this._SID; } set { this._SID = value; } }
        public string BasicInfoGUID { get { return this._BasicInfoGUID; } set { this._BasicInfoGUID = value; } }
        public string TB_SQM_Equipment_TypeCID { get { return this._TB_SQM_Equipment_TypeCID; } set { this._TB_SQM_Equipment_TypeCID = value; } }
        public string TB_SQM_Equipment_Special_TypeSCID { get { return this._TB_SQM_Equipment_Special_TypeSCID; } set { this._TB_SQM_Equipment_Special_TypeSCID = value; } }
        public string DeviceName { get { return this._DeviceName; } set { this._DeviceName = value; } }
        public string DeviceQuantity { get { return this._DeviceQuantity; } set { this._DeviceQuantity = value; } }
        public string DeviceArea { get { return this._DeviceArea; } set { this._DeviceArea = value; } }
        public string DeviceCapacity { get { return this._DeviceCapacity; } set { this._DeviceCapacity = value; } }
        public string DevicePrecision { get { return this._DevicePrecision; } set { this._DevicePrecision = value; } }
        public string TestItem { get { return this._TestItem; } set { this._TestItem = value; } }
        public string DatePurchased { get { return this._DatePurchased; } set { this._DatePurchased = value; } }
        public string DateMade { get { return this._DateMade; } set { this._DateMade = value; } }
        public string Model { get { return this._Model; } set { this._Model = value; } }
        public string Brand { get { return this._Brand; } set { this._Brand = value; } }
        public string Weight { get { return this._Weight; } set { this._Weight = value; } }
        public string ModelSize { get { return this._ModelSize; } set { this._ModelSize = value; } }
        public string LineLength { get { return this._LineLength; } set { this._LineLength = value; } }
        public string GunQty { get { return this._GunQty; } set { this._GunQty = value; } }
        public string RoastQty { get { return this._RoastQty; } set { this._RoastQty = value; } }
        public string PlatingType { get { return this._PlatingType; } set { this._PlatingType = value; } }
        public string Anodized { get { return this._Anodized; } set { this._Anodized = value; } }
        public string SprayingType { get { return this._SprayingType; } set { this._SprayingType = value; } }
        public string PlatingCapacity { get { return this._PlatingCapacity; } set { this._PlatingCapacity = value; } }

        public SQM_EquipmentMgmt() { }

        public SQM_EquipmentMgmt(string SID, string BasicInfoGUID, string TB_SQM_Equipment_TypeCID,string TB_SQM_Equipment_Special_TypeSCID,
            string DeviceName, string DeviceQuantity, string DeviceArea, string DeviceCapacity, string DevicePrecision,
            string TestItem, string DatePurchased, string DateMade, string Model, string Brand,
            string Weight, string ModelSize, string LineLength, string GunQty, string RoastQty, string PlatingType, string Anodized, string SprayingType, string PlatingCapacity)
        {
            this._SID = SID;
            this._BasicInfoGUID = BasicInfoGUID;
            this._TB_SQM_Equipment_TypeCID = TB_SQM_Equipment_TypeCID;
            this._TB_SQM_Equipment_Special_TypeSCID= TB_SQM_Equipment_Special_TypeSCID;
            this._DeviceName = DeviceName;
            this._DeviceQuantity = DeviceQuantity;
            this._DeviceArea = DeviceArea;
            this._DeviceCapacity = DeviceCapacity;
            this._DevicePrecision = DevicePrecision;
            this._TestItem = TestItem;
            this._DatePurchased = DatePurchased;
            this._DateMade = DateMade;
            this._Model = Model;
            this._Brand = Brand;
            this._Weight = Weight;
            this._ModelSize = ModelSize;
            this._LineLength = LineLength;
            this._GunQty = GunQty;
            this._RoastQty = RoastQty;
            this._PlatingType = PlatingType;
            this._Anodized = Anodized;
            this._SprayingType = SprayingType;
            this._PlatingCapacity = PlatingCapacity;
        }
    }
    public class SQM_EquipmentMgmt_jQGridJSon
    {
        public List<SQM_EquipmentMgmt> Rows = new List<SQM_EquipmentMgmt>();
        public List<RowModel> model = new List<RowModel>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }
    public static class SQM_EquipmentMgmt_Helper
    {
        #region Create/Edit data check
        private static void UnescapeDataFromWeb(SQM_EquipmentMgmt DataItem)
        {
            DataItem.DeviceName = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.DeviceName);
            DataItem.DeviceQuantity = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.DeviceQuantity);
            DataItem.DeviceArea = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.DeviceArea);
            DataItem.DeviceCapacity = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.DeviceCapacity);
            DataItem.DevicePrecision = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.DevicePrecision);
            DataItem.TestItem = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.TestItem);
            DataItem.DatePurchased = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.DatePurchased);
            DataItem.DateMade = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.DateMade);
            DataItem.Model = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Model);
            DataItem.Brand = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Brand);
            DataItem.Weight = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Weight);
            DataItem.ModelSize = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ModelSize);
            DataItem.LineLength = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.LineLength);
            DataItem.GunQty = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.GunQty);
            DataItem.RoastQty = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.RoastQty);
            DataItem.PlatingType = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.PlatingType);
            DataItem.Anodized = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Anodized);
            DataItem.SprayingType = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.SprayingType);
            DataItem.PlatingCapacity = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.PlatingCapacity);
        }
        private static string DataCheck(SQM_EquipmentMgmt DataItem)
        {
            string r = "";
            List<string> e = new List<string>();

            if (StringHelper.DataIsNullOrEmpty(DataItem.DeviceName))
                e.Add("Must provide DeviceName.");
            if (StringHelper.DataIsNullOrEmpty(DataItem.DeviceQuantity))
                e.Add("Must provide DeviceQuantity.");
          
         
            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }
        #endregion

        public static string DeleteDataItem(SqlConnection cnPortal, SQM_EquipmentMgmt DataItem)
        {
            return DeleteDataItem(cnPortal, DataItem, "", "");
        }
        public static string DeleteDataItem(SqlConnection cnPortal, SQM_EquipmentMgmt DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            string r = "";
            if (StringHelper.DataIsNullOrEmpty(DataItem.SID))
                return "Must provide SID.";
            else
            {
                using (SqlCommand cmd = new SqlCommand("", cnPortal)) { r = DeleteDataItemSub(cmd, DataItem); }
                if (r != "") { return r; }
                //SqlTransaction tran = cnPortal.BeginTransaction();

                ////Delete member data
                //using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = DeleteDataItemSub(cmd, DataItem); }
                //if (r != "") { tran.Rollback(); return r; }

                ////Release lock
                //if ((LoginMemberGUID != "") && (RunAsMemberGUID != ""))
                //    using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = DataLockHelper.ReleaseLock(cmd, DataItem.SID, LoginMemberGUID, RunAsMemberGUID); }
                //if (r != "") { tran.Rollback(); return r; }

                ////Commit
                //try { tran.Commit(); }
                //catch (Exception e) { tran.Rollback(); r = "Delete fail.<br />Exception: " + e.ToString(); }
                return r;
            }
        }
        private static string DeleteDataItemSub(SqlCommand cmd, SQM_EquipmentMgmt DataItem)
        {
            string sErrMsg = "";

            string sSQL = "Delete TB_SQM_Equipment Where SID = @SID;";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@SID", StringHelper.EmptyOrUnescapedStringViaUrlDecode( DataItem.SID));

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }

        public static string GetEquipmentSpecialType(SqlConnection cn, String MainID)
        {
            if (String.IsNullOrEmpty(MainID))
            {
                return JsonConvert.SerializeObject(new DataTable());
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("select [TB_SQM_Equipment_Special_Type].SCID,CName from [dbo].[TB_SQM_Equipment_Special_Type] ");
            sb.Append("inner join [dbo].[TB_SQM_Equipment_Special_Type_Map] on [TB_SQM_Equipment_Special_Type].SCID=[TB_SQM_Equipment_Special_Type_Map].SCID ");
            sb.Append(" where [TB_SQM_Equipment_Special_Type_Map].CID=@CID;");

            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@CID", MainID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }

        public static string GetEquipmentType(SqlConnection cn)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT CID,CNAME FROM [dbo].[TB_SQM_Equipment_Type]");

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
        public static string GetModel(SqlConnection cn, string EquipmentType, string EquipmentSpecialType)
        {
            SQM_EquipmentMgmt_jQGridJSon m = new SQM_EquipmentMgmt_jQGridJSon();
            List<ColModel> colmodel = new List<ColModel>();
            ArrayList colname = new ArrayList();
            string sSearchText = EquipmentType.Trim();
            m.Rows.Clear();
            int iRowCount = 0;
            string sSQL = string.Empty;
            switch (sSearchText)
            {
                case "1":
                    colname.Clear();
                    colmodel.Clear();
                    colname.Add("SID");
                    colname.Add("VendorCode");
                    colname.Add("TB_SQM_Equipment_TypeCID");
                    colname.Add("DeviceName");
                    colname.Add("DeviceQuantity");
                    colname.Add("DeviceArea");
                    colname.Add("DeviceCapacity");
                    colname.Add("DevicePrecision");
                    colmodel.Add(new ColModel("SID", "SID", 150, false, true));
                    colmodel.Add(new ColModel("VendorCode", "VendorCode", 150, false, true));
                    colmodel.Add(new ColModel("TB_SQM_Equipment_TypeCID", "TB_SQM_Equipment_TypeCID", 150, false, true));
                    colmodel.Add(new ColModel("DeviceName", "DeviceName", 150, false, false));
                    colmodel.Add(new ColModel("DeviceQuantity", "DeviceQuantity", 150, false, false));
                    colmodel.Add(new ColModel("DeviceArea", "DeviceArea", 150, false, false));
                    colmodel.Add(new ColModel("DeviceCapacity", "DeviceCapacity", 150, false, false));
                    colmodel.Add(new ColModel("DevicePrecision", "DevicePrecision", 150, false, false));
                    break;
                case "2":
                case "3":
                case "4":
                    colname.Clear();
                    colmodel.Clear();
                    colname.Add("SID");
                    colname.Add("VendorCode");
                    colname.Add("TB_SQM_Equipment_TypeCID");
                    colname.Add("DeviceName");
                    colname.Add("DeviceQuantity");
                    colname.Add("DeviceArea");
                    colname.Add("TestItem");
                    colname.Add("DatePurchased");
                    colmodel.Add(new ColModel("SID", "SID", 150, false, true));
                    colmodel.Add(new ColModel("VendorCode", "VendorCode", 150, false, true));
                    colmodel.Add(new ColModel("TB_SQM_Equipment_TypeCID", "TB_SQM_Equipment_TypeCID", 150, false, true));
                    colmodel.Add(new ColModel("DeviceName", "DeviceName", 150, false, false));
                    colmodel.Add(new ColModel("DeviceQuantity", "DeviceQuantity", 150, false, false));
                    colmodel.Add(new ColModel("DeviceArea", "DeviceArea", 150, false, false));
                    colmodel.Add(new ColModel("TestItem", "TestItem", 150, false, false));
                    colmodel.Add(new ColModel("DatePurchased", "DatePurchased", 150, false, false));
                    break;
                case "5":
                case "6":
                    ArrayList list = getArraybytype(cn, EquipmentType, EquipmentSpecialType);
                    colname.Clear();
                    colmodel.Clear();
                    colname.Add("SID");
                    colname.Add("VendorCode");
                    colname.Add("TB_SQM_Equipment_TypeCID");
                    colname.Add("TB_SQM_Equipment_Special_TypeSCID");
                    colmodel.Add(new ColModel("SID", "SID", 150, false, true));
                    colmodel.Add(new ColModel("VendorCode", "VendorCode", 150, false, true));
                    colmodel.Add(new ColModel("TB_SQM_Equipment_TypeCID", "TB_SQM_Equipment_TypeCID", 150, false,true));
                    colmodel.Add(new ColModel("TB_SQM_Equipment_Special_TypeSCID", "TB_SQM_Equipment_Special_TypeSCID", 150, false, true));
                    foreach (var item in list)
                    {
                        sSQL = sSQL + "," + item;
                        colname.Add(item.ToString());
                        colmodel.Add(new ColModel(item.ToString(), item.ToString(), 150, false, false));
                    }
                    break;
                case "9":
                    colname.Clear();
                    colmodel.Clear();
                    colname.Add("SID");
                    colname.Add("VendorCode");
                    colname.Add("TB_SQM_Equipment_TypeCID");
                    colname.Add("DeviceName");
                    colname.Add("DeviceQuantity");
                    colname.Add("DeviceArea");
                    colname.Add("DatePurchased");
                    colname.Add("Model");
                    colmodel.Add(new ColModel("SID", "SID", 150, false, true));
                    colmodel.Add(new ColModel("VendorCode", "VendorCode", 150, false, true));
                    colmodel.Add(new ColModel("TB_SQM_Equipment_TypeCID", "TB_SQM_Equipment_TypeCID", 150, false, true));
                    colmodel.Add(new ColModel("DeviceName", "DeviceName", 150, false, false));
                    colmodel.Add(new ColModel("DeviceQuantity", "DeviceQuantity", 150, false, false));
                    colmodel.Add(new ColModel("DeviceArea", "DeviceArea", 150, false, false));
                    colmodel.Add(new ColModel("DatePurchased", "DatePurchased", 150, false, false));
                    colmodel.Add(new ColModel("Model", "Model", 150, false, false));
                    break;

            }
            string[] ColName = (string[])colname.ToArray(typeof(string));
            m.model.Add(new RowModel(ColName, colmodel));
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(m);
        }
        public static string GetDataToJQGridJson(SqlConnection cn, string EquipmentType, string EquipmentSpecialType,string BasicInfoGUID)
        {
            SQM_EquipmentMgmt_jQGridJSon m = new SQM_EquipmentMgmt_jQGridJSon();
            string sSearchText = EquipmentType.Trim();
            if (sSearchText.Equals("5")&& string.IsNullOrEmpty(EquipmentSpecialType))
            {
                EquipmentSpecialType = "1";
            }
            else if (sSearchText.Equals("6") && string.IsNullOrEmpty(EquipmentSpecialType))
            {
                EquipmentSpecialType = "10";
            }
            string sWhereClause = "";
            if (sSearchText != "")
                sWhereClause += " and TB_SQM_Equipment_TypeCID=@TB_SQM_Equipment_TypeCID";
            if (EquipmentSpecialType.Trim() != "")
                sWhereClause += " and TB_SQM_Equipment_Special_TypeSCID=@TB_SQM_Equipment_Special_TypeSCID";
            if (BasicInfoGUID.Trim()!="")
            {
                sWhereClause += " and BasicInfoGUID=@BasicInfoGUID";
            }
            if (sWhereClause.Length != 0)
                sWhereClause = " Where" + sWhereClause.Substring(4);

            m.Rows.Clear();
            int iRowCount = 0;
            string sSQL = string.Empty;
           sSQL= "SELECT  [SID],[BasicInfoGUID],[TB_SQM_Equipment_TypeCID],[TB_SQM_Equipment_Special_TypeSCID],[DeviceName],[DeviceQuantity],[DeviceArea],[DeviceCapacity],[DevicePrecision],[TestItem],[DatePurchased],[DateMade],[Model],[Brand],[Weight],[ModelSize],[LineLength],[GunQty],[RoastQty],[PlatingType],[Anodized],[SprayingType],[PlatingCapacity] FROM [TB_SQM_Equipment]";

            sSQL += sWhereClause + ";";
            using (SqlCommand cmd = new SqlCommand(sSQL, cn))
            {
                cmd.Parameters.Add(new SqlParameter("@TB_SQM_Equipment_TypeCID", StringHelper.NullOrEmptyStringIsDBNull(EquipmentType)));
                cmd.Parameters.Add(new SqlParameter("@TB_SQM_Equipment_Special_TypeSCID", StringHelper.NullOrEmptyStringIsDBNull(EquipmentSpecialType)));
                cmd.Parameters.Add(new SqlParameter("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(BasicInfoGUID)));
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    iRowCount++;
                    string DatePurchased = dr["DatePurchased"].ToString();
                    string DateMade = dr["DateMade"].ToString();
                    if (!String.IsNullOrEmpty(dr["DatePurchased"].ToString()))
                    {
                        DatePurchased= Convert.ToDateTime(dr["DatePurchased"].ToString()).ToString("yyyy/MM/dd");
                    }
                    if (!String.IsNullOrEmpty(dr["DateMade"].ToString()))
                    {
                        DateMade= Convert.ToDateTime(dr["DateMade"].ToString()).ToString("yyyy/MM/dd");
                    }
                    m.Rows.Add(new SQM_EquipmentMgmt(
                        dr["SID"].ToString(),
                        dr["BasicInfoGUID"].ToString(),
                        dr["TB_SQM_Equipment_TypeCID"].ToString(),
                        dr["TB_SQM_Equipment_Special_TypeSCID"].ToString(),
                        dr["DeviceName"].ToString(),
                        dr["DeviceQuantity"].ToString(),
                        dr["DeviceArea"].ToString(),
                        dr["DeviceCapacity"].ToString(),
                        dr["DevicePrecision"].ToString(),
                        dr["TestItem"].ToString(),
                        DatePurchased,
                        DateMade,
                        dr["Model"].ToString(),
                        dr["Brand"].ToString(),
                        dr["Weight"].ToString(),
                        dr["ModelSize"].ToString(),
                        dr["LineLength"].ToString(),
                        dr["GunQty"].ToString(),
                        dr["RoastQty"].ToString(),
                        dr["PlatingType"].ToString(),
                        dr["Anodized"].ToString(),
                        dr["SprayingType"].ToString(),
                        dr["PlatingCapacity"].ToString()
                        ));
                }
                dr.Close();
                dr = null;
            }

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(m);
        }
        private static ArrayList getArraybytype(SqlConnection cn, string equipmentType, string equipmentSpecialType)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select  ColumeName from [dbo].[TB_SQM_Equipment_Special_Sub_Type] ");
            sb.Append("inner join [dbo].[TB_SQM_Equipment_Special_Sub_Map] on [dbo].[TB_SQM_Equipment_Special_Sub_Map].SSCID =[dbo].[TB_SQM_Equipment_Special_Sub_Type].SSCID ");
            sb.Append("inner join [dbo].[TB_SQM_Equipment_Special_Type] on [dbo].[TB_SQM_Equipment_Special_Type].SCID =[dbo].[TB_SQM_Equipment_Special_Sub_Map].SCID ");
            sb.Append("inner join [dbo].[TB_SQM_Equipment_Special_Type_Map] on [dbo].[TB_SQM_Equipment_Special_Type_Map].SCID =[dbo].[TB_SQM_Equipment_Special_Type].SCID ");
            sb.Append("inner join [dbo].[TB_SQM_Equipment_Type] on [dbo].[TB_SQM_Equipment_Type].CID =[dbo].[TB_SQM_Equipment_Special_Type_Map].CID ");
            sb.Append("where [TB_SQM_Equipment_Type].CID = @CID and [TB_SQM_Equipment_Special_Type].SCID = @SCID");
            DataTable dt = new DataTable();
            ArrayList list = new ArrayList();
            if (!string.IsNullOrEmpty(equipmentSpecialType))
            {
                using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
                {
                    cmd.Parameters.Add(new SqlParameter("@CID", StringHelper.EmptyOrUnescapedStringViaUrlDecode(equipmentType)));
                    cmd.Parameters.Add(new SqlParameter("@SCID", StringHelper.EmptyOrUnescapedStringViaUrlDecode(equipmentSpecialType)));
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        dt.Load(dr);
                    }
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            list.Add(row["ColumeName"].ToString().Trim());
                        }
                    }
                }
            }
            return list;
        }

        public static string GetDataToJQGridJson(SqlConnection cn)
        {
            return GetDataToJQGridJson(cn, "", "","");
        }
        public static string CreateDataItem(SqlConnection cnPortal, SQM_EquipmentMgmt DataItem)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);

            if (r != "")
            { return r; }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("INSERT INTO TB_SQM_Equipment ([BasicInfoGUID],[TB_SQM_Equipment_TypeCID],[TB_SQM_Equipment_Special_TypeSCID],[DeviceName],[DeviceQuantity],[DeviceArea],[DeviceCapacity]");
                sb.Append(",[DevicePrecision],[TestItem],[DatePurchased],[DateMade],[Model],[Brand],[Weight],[ModelSize],[LineLength],[GunQty],[RoastQty],[PlatingType],[Anodized],[SprayingType],[PlatingCapacity])");
                sb.Append("Values ( @BasicInfoGUID,@TB_SQM_Equipment_TypeCID,@TB_SQM_Equipment_Special_TypeSCID,@DeviceName,@DeviceQuantity,@DeviceArea,@DeviceCapacity,@DevicePrecision,@TestItem,@DatePurchased, @DateMade, @Model, @Brand, @Weight, @ModelSize, @LineLength,@GunQty, @RoastQty, @PlatingType, @Anodized, @SprayingType, @PlatingCapacity);");
                SQM_Basic_Helper.InsertPart(sb, "7");
                SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal);

               
                cmd.Parameters.AddWithValue("@BasicInfoGUID", StringHelper.NullOrEmptyStringIsDBNull(DataItem.BasicInfoGUID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", StringHelper.NullOrEmptyStringIsDBNull(DataItem.TB_SQM_Equipment_TypeCID));
                cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", StringHelper.NullOrEmptyStringIsDBNull(DataItem.TB_SQM_Equipment_Special_TypeSCID));
                cmd.Parameters.AddWithValue("@DeviceName", StringHelper.NullOrEmptyStringIsDBNull(DataItem.DeviceName));
                cmd.Parameters.AddWithValue("@DeviceQuantity", StringHelper.NullOrEmptyStringIsDBNull(DataItem.DeviceQuantity));
                cmd.Parameters.AddWithValue("@DeviceArea", StringHelper.NullOrEmptyStringIsDBNull(DataItem.DeviceArea));
                cmd.Parameters.AddWithValue("@DeviceCapacity", StringHelper.NullOrEmptyStringIsDBNull(DataItem.DeviceCapacity));
                cmd.Parameters.AddWithValue("@DevicePrecision", StringHelper.NullOrEmptyStringIsDBNull(DataItem.DevicePrecision));
                cmd.Parameters.AddWithValue("@TestItem", StringHelper.NullOrEmptyStringIsDBNull(DataItem.TestItem));
                cmd.Parameters.AddWithValue("@DatePurchased", StringHelper.NullOrEmptyStringIsDBNull(DataItem.DatePurchased));
                cmd.Parameters.AddWithValue("@DateMade", StringHelper.NullOrEmptyStringIsDBNull(DataItem.DateMade));
                cmd.Parameters.AddWithValue("@Model", StringHelper.NullOrEmptyStringIsDBNull(DataItem.Model));
                cmd.Parameters.AddWithValue("@Brand", StringHelper.NullOrEmptyStringIsDBNull(DataItem.Brand));
                cmd.Parameters.AddWithValue("@Weight", StringHelper.NullOrEmptyStringIsDBNull(DataItem.Weight));
                cmd.Parameters.AddWithValue("@ModelSize", StringHelper.NullOrEmptyStringIsDBNull(DataItem.ModelSize));
                cmd.Parameters.AddWithValue("@LineLength", StringHelper.NullOrEmptyStringIsDBNull(DataItem.LineLength));
                cmd.Parameters.AddWithValue("@GunQty", StringHelper.NullOrEmptyStringIsDBNull(DataItem.GunQty));
                cmd.Parameters.AddWithValue("@RoastQty", StringHelper.NullOrEmptyStringIsDBNull(DataItem.RoastQty));
                cmd.Parameters.AddWithValue("@PlatingType", StringHelper.NullOrEmptyStringIsDBNull(DataItem.PlatingType));
                cmd.Parameters.AddWithValue("@Anodized", StringHelper.NullOrEmptyStringIsDBNull(DataItem.Anodized));
                cmd.Parameters.AddWithValue("@SprayingType", StringHelper.NullOrEmptyStringIsDBNull(DataItem.SprayingType));
                cmd.Parameters.AddWithValue("@PlatingCapacity", StringHelper.NullOrEmptyStringIsDBNull(DataItem.PlatingCapacity));
                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;

                return sErrMsg;
            }
        }

        public static string EditDataItem(SqlConnection cnPortal, SQM_EquipmentMgmt DataItem)
        {
            return EditDataItem(cnPortal, DataItem, "", "");
        }
        public static string EditDataItem(SqlConnection cnPortal, SQM_EquipmentMgmt DataItem, string LoginMemberGUID, string RunAsMemberGUID)
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

        private static string EditDataItemSub(SqlCommand cmd, SQM_EquipmentMgmt DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append("Update TB_SQM_Equipment Set  TB_SQM_Equipment_TypeCID=@TB_SQM_Equipment_TypeCID,TB_SQM_Equipment_Special_TypeSCID=@TB_SQM_Equipment_Special_TypeSCID,DeviceName=@DeviceName, ");
            sb.Append("DeviceQuantity=@DeviceQuantity,DeviceArea=@DeviceArea,DeviceCapacity=@DeviceCapacity,DevicePrecision=@DevicePrecision,TestItem=@TestItem,DatePurchased=@DatePurchased,DateMade=@DateMade,Model=@Model,Brand=@Brand,Weight=@Weight,ModelSize=@ModelSize,LineLength=@LineLength,GunQty=@GunQty,RoastQty=@RoastQty,PlatingType=@PlatingType,Anodized=@Anodized,SprayingType=@SprayingType,PlatingCapacity=@PlatingCapacity");
            sb.Append(" Where SID = @SID");

            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@TB_SQM_Equipment_TypeCID", StringHelper.NullOrEmptyStringIsDBNull(DataItem.TB_SQM_Equipment_TypeCID));
            cmd.Parameters.AddWithValue("@TB_SQM_Equipment_Special_TypeSCID", StringHelper.NullOrEmptyStringIsDBNull(DataItem.TB_SQM_Equipment_Special_TypeSCID));
            cmd.Parameters.AddWithValue("@DeviceName", StringHelper.NullOrEmptyStringIsDBNull(DataItem.DeviceName));
            cmd.Parameters.AddWithValue("@DeviceQuantity", StringHelper.NullOrEmptyStringIsDBNull(DataItem.DeviceQuantity));
            cmd.Parameters.AddWithValue("@DeviceArea", StringHelper.NullOrEmptyStringIsDBNull(DataItem.DeviceArea));
            cmd.Parameters.AddWithValue("@DeviceCapacity", StringHelper.NullOrEmptyStringIsDBNull(DataItem.DeviceCapacity));
            cmd.Parameters.AddWithValue("@DevicePrecision", StringHelper.NullOrEmptyStringIsDBNull(DataItem.DevicePrecision));
            cmd.Parameters.AddWithValue("@TestItem", StringHelper.NullOrEmptyStringIsDBNull(DataItem.TestItem));
            cmd.Parameters.AddWithValue("@DatePurchased", StringHelper.NullOrEmptyStringIsDBNull(DataItem.DatePurchased));
            cmd.Parameters.AddWithValue("@DateMade", StringHelper.NullOrEmptyStringIsDBNull(DataItem.DateMade));
            cmd.Parameters.AddWithValue("@Model", StringHelper.NullOrEmptyStringIsDBNull(DataItem.Model));
            cmd.Parameters.AddWithValue("@Brand", StringHelper.NullOrEmptyStringIsDBNull(DataItem.Brand));
            cmd.Parameters.AddWithValue("@Weight", StringHelper.NullOrEmptyStringIsDBNull(DataItem.Weight));
            cmd.Parameters.AddWithValue("@ModelSize", StringHelper.NullOrEmptyStringIsDBNull(DataItem.ModelSize));
            cmd.Parameters.AddWithValue("@LineLength", StringHelper.NullOrEmptyStringIsDBNull(DataItem.LineLength));
            cmd.Parameters.AddWithValue("@GunQty", StringHelper.NullOrEmptyStringIsDBNull(DataItem.GunQty));
            cmd.Parameters.AddWithValue("@RoastQty", StringHelper.NullOrEmptyStringIsDBNull(DataItem.RoastQty));
            cmd.Parameters.AddWithValue("@PlatingType", StringHelper.NullOrEmptyStringIsDBNull(DataItem.PlatingType));
            cmd.Parameters.AddWithValue("@Anodized", StringHelper.NullOrEmptyStringIsDBNull(DataItem.Anodized));
            cmd.Parameters.AddWithValue("@SprayingType", StringHelper.NullOrEmptyStringIsDBNull(DataItem.SprayingType));
            cmd.Parameters.AddWithValue("@PlatingCapacity", StringHelper.NullOrEmptyStringIsDBNull(DataItem.PlatingCapacity));

            cmd.Parameters.AddWithValue("@SID", DataItem.SID);
            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }

    }

    public class RowModel
    {

        public string[] Colname;
        
        public List<ColModel> ColModel;
        public RowModel() { }

        public RowModel(string[] colname, List<ColModel> colModel)
        {
            this.ColModel = colModel;
            this.Colname = colname;
        }
    }
   
    public class ColModel
    {
        
        public string name;
        public string index;
        public int width;
        public bool sortable;
        public bool hidden;
        public ColModel() { }

        public ColModel(string name, string index, int width, bool sortable, bool hidden)
        {
            this.name = name;
            this.index = index;
            this.width = width;
            this.sortable = sortable;
            this.hidden = hidden;
        }
    }
}