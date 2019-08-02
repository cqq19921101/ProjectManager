using Lib_Portal_Domain.Model;
using Lib_Portal_Domain.SharedLibs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
//using Lib_SQM_Domain.SharedLibs;

namespace Lib_SQM_Domain.Modal
{
    #region Data Class Definitions
    public class TB_SQM_Manufacturers_BasicInfo
    {
        public string BasicInfoGUID { get; set; }

        public string VendorCode { get; set; }

        public int TB_SQM_Vendor_TypeCID { get; set; }

        public string CompanyName { get; set; }

        public string CompanyAddress { get; set; }

        public string FactoryName { get; set; }

        public string FactoryAddress { get; set; }

        public bool? IsTrader { get; set; }

        public bool? IsSpotTrader { get; set; }

        public string TB_SQM_Commodity_SubCID { get; set; }

        public string TB_SQM_Commodity_SubTB_SQM_CommodityCID { get; set; }

        public string DateInfo { get; set; }

        public string ProvidedName { get; set; }

        public string JobTitle { get; set; }

        public string EnterpriseCategory { get; set; }

        public string OwnerShip { get; set; }

        public string FoundedYear { get; set; }

        public decimal? LastRevenues1 { get; set; }

        public decimal? LastRevenues2 { get; set; }

        public decimal? LastRevenues3 { get; set; }

        public decimal? CurrentRevenues { get; set; }

        public string TurnoverAnalysis { get; set; }

        public int? RevenueGrowthRate1 { get; set; }

        public int? RevenueGrowthRate2 { get; set; }

        public int? RevenueGrowthRate3 { get; set; }

        public int? GrossProfitRate1 { get; set; }

        public int? GrossProfitRate2 { get; set; }

        public int? GrossProfitRate3 { get; set; }

        public string PlanInvestCapital { get; set; }

        public string BankAndAccNumber { get; set; }

        public string TradingCurrency { get; set; }

        public string TradeMode { get; set; }

        public string VMIManageModel { get; set; }

        public string Distance { get; set; }

        public string MinMonthStateDays { get; set; }

        public string BU1TurnoverName { get; set; }

        public string BU2TurnoverName { get; set; }

        public string BU3TurnoverName { get; set; }

        public string BU1Turnover { get; set; }

        public string BU2Turnover { get; set; }

        public string BU3Turnover { get; set; }

        public string CompanyAdvantage { get; set; }

        public string LineQuantitylength { get; set; }

        public string CleanRoom { get; set; }

        public int? RobotQuantity { get; set; }

        public bool? Is3DUG { get; set; }

        public bool? Is3DProE { get; set; }

        public bool? Is2DAutoCAD { get; set; }

        public bool? IsPhotoShop { get; set; }

        public bool? IsIDMapAbility { get; set; }

        public bool? Is3DMapAbility { get; set; }

        public bool? Is2DMapAbility { get; set; }

        public bool? IsMoldflowAbility { get; set; }

        public bool? IsTAAbility { get; set; }

        public bool? IsDesignGuildline { get; set; }

        public bool? IsFMEA { get; set; }

        public bool? IsLessonLearnt { get; set; }

        public string MoldProduceCapacity { get; set; }

    }
    #endregion

    public static class SQMBasic_BasicInfo_Helper
    {
        #region Get Menu
        public static string LoadMenuJSon(SqlConnection cn)
        {
            SystemMgmt_Menu_jQGridJSon m = new SystemMgmt_Menu_jQGridJSon();
            string sSQL = "";

            List<MenuItem2> MenuItems = new List<MenuItem2>();
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT DISTINCT FunctionGUID,");
            sbSql.Append("                 MenuLevel,");
            sbSql.Append("                 IntranetHref,");
            sbSql.Append("                 InternetHref,");
            sbSql.Append("                 HrefTarget,");
            sbSql.Append("                 SortCode,");
            sbSql.Append("                 ParentFunctionGUID");
            sbSql.Append(" FROM(");
            sbSql.Append("     SELECT *");
            sbSql.Append("     FROM PORTAL_FunctionInMenu WITH ( NOLOCK )");
            sbSql.Append("     WHERE FunctionGUID =@FunctionGUID");
            sbSql.Append("        OR ParentFunctionGUID =@FunctionGUID");
            sbSql.Append("     UNION ALL");
            sbSql.Append("     SELECT *");
            sbSql.Append("     FROM PORTAL_FunctionInMenu WITH ( NOLOCK )");
            sbSql.Append("     WHERE ParentFunctionGUID IN(");
            sbSql.Append("              SELECT FunctionGUID");
            sbSql.Append("              FROM PORTAL_FunctionInMenu");
            sbSql.Append("              WHERE FunctionGUID =@FunctionGUID");
            sbSql.Append("                 OR ParentFunctionGUID =@FunctionGUID )) T1");
            sSQL = sbSql.ToString();
            using (SqlCommand cmd = new SqlCommand(sSQL, cn))
            {
                cmd.Parameters.AddWithValue("@FunctionGUID", "A9F15064-EB5D-41D6-8AA4-BAFFE8AA0604");
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    MenuItems.Add(new MenuItem2(
                        dr["FunctionGUID"].ToString().Trim(),
                        (int)dr["MenuLevel"],
                        dr["IntranetHref"].ToString().Trim(),
                        dr["InternetHref"].ToString().Trim(),
                        (dr["HrefTarget"].ToString().Trim() == "_self") ? HrefTarget.self : HrefTarget.blank,
                        (int)dr["SortCode"],
                        dr["ParentFunctionGUID"].ToString().Trim()));
                }
                dr.Close();
                dr = null;
            }

            foreach (MenuItem2 L1Menu in MenuItems.Where(mi => mi.ParentFunctionGUID == "").OrderBy(mi => mi.SortCode))
            {
                L1Menu.level = "0";
                L1Menu.parent = "null";
                m.Rows.Add(L1Menu);
                bool bHasL2Child = false;
                foreach (MenuItem2 L2Menu in MenuItems.Where(mi => mi.ParentFunctionGUID == L1Menu.FunctionGUID).OrderBy(mi => mi.SortCode))
                {
                    bHasL2Child = true;
                    L2Menu.level = "1";
                    L2Menu.parent = L1Menu.FunctionGUID;
                    m.Rows.Add(L2Menu);
                    bool bHasL3Child = false;
                    foreach (MenuItem2 L3Menu in MenuItems.Where(mi => mi.ParentFunctionGUID == L2Menu.FunctionGUID).OrderBy(mi => mi.SortCode))
                    {
                        bHasL3Child = true;
                        L3Menu.level = "2";
                        L3Menu.parent = L2Menu.FunctionGUID;
                        m.Rows.Add(L3Menu);
                    }
                    if (bHasL3Child)
                        L2Menu.isLeaf = false;
                }
                if (bHasL2Child)
                    L1Menu.isLeaf = false;
            }

            MenuItems.Clear();
            MenuItems = null;

            List<FunctionTitle> FunctionTitles = new List<FunctionTitle>();
            sSQL = "Select FunctionGUID, Locale, FunctionTitle from PORTAL_FunctionTitles WITH (NOLOCK);";
            using (SqlCommand cmd = new SqlCommand(sSQL, cn))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    FunctionTitles.Add(new FunctionTitle(
                        dr["FunctionGUID"].ToString().Trim(),
                        dr["Locale"].ToString().Trim(),
                        dr["FunctionTitle"].ToString().Trim()));
                }
                dr.Close();
                dr = null;
            }

            foreach (MenuItem2 mi in m.Rows)
            {
                foreach (FunctionTitle t in FunctionTitles.Where(ft => ft.FunctionGUID == mi.FunctionGUID))
                {
                    switch (t.Locale.ToLower())
                    {
                        case "en-us": mi.Title_en_US = t.Title; break;
                        case "zh-cn": mi.Title_zh_CN = t.Title; break;
                        case "zh-tw": mi.Title_zh_TW = t.Title; break;
                        default: break;
                    }
                }
            }

            FunctionTitles.Clear();
            FunctionTitles = null;

            List<FunctionRole> FunctionRoles = new List<FunctionRole>();
            sSQL = "select RoleGUID, FunctionGUID from PORTAL_RoleFunctions WITH (NOLOCK);";
            using (SqlCommand cmd = new SqlCommand(sSQL, cn))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    FunctionRoles.Add(new FunctionRole(
                        dr["FunctionGUID"].ToString().Trim(),
                        dr["RoleGUID"].ToString().Trim()));

                }
                dr.Close();
                dr = null;
            }

            foreach (MenuItem2 mi in m.Rows)
                foreach (FunctionRole r in FunctionRoles.Where(fr => fr.FunctionGUID == mi.FunctionGUID))
                    mi.Roles.Add(r.RoleGUID);

            FunctionRoles.Clear();
            FunctionRoles = null;

            List<FunctionControllerAction> FunctionControllerActions = new List<FunctionControllerAction>();
            sSQL = "select FunctionGUID, Controller, Action from PORTAL_FunctionControllerActions WITH (NOLOCK);";
            using (SqlCommand cmd = new SqlCommand(sSQL, cn))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    FunctionControllerActions.Add(new FunctionControllerAction(
                        dr["FunctionGUID"].ToString().Trim(),
                        dr["Controller"].ToString().Trim(),
                        dr["Action"].ToString().Trim()));

                }
                dr.Close();
                dr = null;
            }

            foreach (MenuItem2 mi in m.Rows)
                foreach (FunctionControllerAction r in FunctionControllerActions.Where(fca => fca.FunctionGUID == mi.FunctionGUID))
                    mi.ControllerActions.Add(r.Controller + "|" + r.Action);

            FunctionControllerActions.Clear();
            FunctionControllerActions = null;

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(m);
        }
        #endregion

        #region Update Menu
        public static string UpdateMenu(SqlConnection cnPortal, List<MenuItemFromView2> NewMenu)
        {
            //=======================================================================================================
            //"Normalize" NewMenu
            //1. Replace "NEWGUID:" to a .Net generated GUID value
            //2. Resort to iSortCode
            //3. grant permissions to parent function
            int iSortCode = 0;
            foreach (MenuItemFromView2 m in NewMenu)
            {
                iSortCode++;
                m.SortCode = iSortCode;
                if (m.FunctionGUID.Substring(0, 8) == "NEWGUID:")
                {
                    string vFunctionGUID = m.FunctionGUID;
                    string nFunctionGUID = Guid.NewGuid().ToString();
                    m.FunctionGUID = nFunctionGUID;
                    foreach (MenuItemFromView2 m2 in NewMenu)
                        if (m2.ParentFunctionGUID == vFunctionGUID)
                            m2.ParentFunctionGUID = nFunctionGUID;
                }
            }

            List<int> LeafNodes = new List<int>();
            for (int iCnt = 0; iCnt < NewMenu.Count; iCnt++)
            {
                string FID = NewMenu[iCnt].FunctionGUID;
                if (!NewMenu.Exists(mi => mi.ParentFunctionGUID == FID))
                    LeafNodes.Add(iCnt);
                else
                    NewMenu[iCnt].Roles.Clear();
            }

            foreach (int NodeIndex in LeafNodes)
                UpdateAllParentNodeRoles(NodeIndex, NewMenu);

            LeafNodes.Clear();
            LeafNodes = null;
            //=======================================================================================================

            string r = "";
            SqlTransaction tran = cnPortal.BeginTransaction();
            string sSQL = "";

            sSQL = "Delete From PORTAL_RoleFunctions;";
            sSQL += "Delete From PORTAL_FunctionControllerActions;";
            sSQL += "Delete From PORTAL_FunctionTitles;";
            sSQL += "Delete From PORTAL_FunctionInMenu;";
            sSQL += "Delete From PORTAL_Functions;";
            try { using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal, tran)) { cmd.ExecuteNonQuery(); } }
            catch (Exception e) { r = e.Message; }

            foreach (MenuItemFromView2 m in NewMenu)
            {
                //replace new FunctionGUID
                if (m.FunctionGUID.Substring(0, 8) == "NEWGUID:") m.FunctionGUID = Guid.NewGuid().ToString();

                sSQL = "Insert Into PORTAL_Functions(FunctionGUID, FunctionName) Values (@FunctionGUID, @FunctionName);";
                try
                {
                    using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal, tran))
                    {
                        cmd.Parameters.AddWithValue("@FunctionGUID", m.FunctionGUID);
                        cmd.Parameters.AddWithValue("@FunctionName", m.Title_en_US);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception e)
                {
                    r = e.Message;
                    break;
                }

                sSQL = "Insert Into PORTAL_FunctionInMenu(FunctionGUID, MenuLevel, IntranetHref, InternetHref, HrefTarget, SortCode, ParentFunctionGUID) ";
                sSQL += " Values (@FunctionGUID, @MenuLevel, @IntranetHref, @InternetHref, @HrefTarget, @SortCode, @ParentFunctionGUID);";
                try
                {
                    using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal, tran))
                    {
                        cmd.Parameters.AddWithValue("@FunctionGUID", m.FunctionGUID);
                        cmd.Parameters.AddWithValue("@MenuLevel", m.MenuLevel);
                        cmd.Parameters.AddWithValue("@IntranetHref", StringHelper.NullOrEmptyStringIsDBNull(m.IntranetHref));
                        cmd.Parameters.AddWithValue("@InternetHref", StringHelper.NullOrEmptyStringIsDBNull(m.InternetHref));
                        if (m.IntranetHref == "")
                            cmd.Parameters.AddWithValue("@HrefTarget", "_self");
                        else
                            cmd.Parameters.AddWithValue("@HrefTarget", (m.HrefTarget == HrefTarget.self) ? "_self" : "_blank");
                        cmd.Parameters.AddWithValue("@SortCode", m.SortCode);
                        cmd.Parameters.AddWithValue("@ParentFunctionGUID", StringHelper.NullOrEmptyStringIsDBNull(m.ParentFunctionGUID));
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception e) { r = e.Message; break; }

                sSQL = "Insert Into PORTAL_FunctionTitles(FunctionGUID, Locale, FunctionTitle) Values (@FunctionGUID, 'en-US', @FT_enUS);";
                sSQL += "Insert Into PORTAL_FunctionTitles(FunctionGUID, Locale, FunctionTitle) Values (@FunctionGUID, 'zh-CN', @FT_zhCN);";
                sSQL += "Insert Into PORTAL_FunctionTitles(FunctionGUID, Locale, FunctionTitle) Values (@FunctionGUID, 'zh-TW', @FT_zhTW);";
                try
                {
                    using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal, tran))
                    {
                        cmd.Parameters.AddWithValue("@FunctionGUID", m.FunctionGUID);
                        cmd.Parameters.AddWithValue("@FT_enUS", m.Title_en_US);
                        cmd.Parameters.AddWithValue("@FT_zhCN", m.Title_zh_CN);
                        cmd.Parameters.AddWithValue("@FT_zhTW", m.Title_zh_TW);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception e) { r = e.Message; break; }

                foreach (ControllerAction ca in m.ControllerActions)
                {
                    if (r != "") break;

                    sSQL = "Insert Into PORTAL_FunctionControllerActions(FunctionGUID, Controller, Action) Values (@FunctionGUID, @Controller, @Action);";
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal, tran))
                        {
                            cmd.Parameters.AddWithValue("@FunctionGUID", m.FunctionGUID);
                            cmd.Parameters.AddWithValue("@Controller", ca.Controller);
                            cmd.Parameters.AddWithValue("@Action", ca.Action);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception e) { r = e.Message; break; }
                }

                foreach (string rid in m.Roles)
                {
                    if (r != "") break;
                    sSQL = "Insert Into PORTAL_RoleFunctions(RoleGUID, FunctionGUID) Values (@RoleGUID, @FunctionGUID);";
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal, tran))
                        {
                            cmd.Parameters.AddWithValue("@RoleGUID", rid);
                            cmd.Parameters.AddWithValue("@FunctionGUID", m.FunctionGUID);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception e) { r = e.Message; break; }
                }
            }

            if (r == "")
                tran.Commit();
            else
                tran.Rollback();

            return r;
        }

        public static void UpdateAllParentNodeRoles(int NodeIndex, List<MenuItemFromView2> Menu)
        {
            string PFunGUID = Menu[NodeIndex].ParentFunctionGUID;
            if (PFunGUID != "")
            {
                int iParentIndex = -1;
                for (int iCnt = 0; iCnt < Menu.Count; iCnt++)
                    if (Menu[iCnt].FunctionGUID == PFunGUID)
                    {
                        iParentIndex = iCnt;
                        foreach (string RoleGUID in Menu[NodeIndex].Roles)
                            if (!Menu[iCnt].Roles.Contains(RoleGUID))
                                Menu[iCnt].Roles.Add(RoleGUID);
                        break;
                    }
                if (iParentIndex > -1)
                    UpdateAllParentNodeRoles(iParentIndex, Menu);
            }
        }
        #endregion

        #region Get Function Roles
        public static string GetFunctionRoleFullListJSon(SqlConnection cnPortal, string RolesString)
        {
            SystemMgmt_FunctionRole_jQGridJSon r = new SystemMgmt_FunctionRole_jQGridJSon();

            string[] rs = RolesString.Split(',');
            //Array.FindAll(arr, s => s.Equals(target));

            string sSQL = "Select RoleGUID, RoleName From PORTAL_Roles WITH (NOLOCK);";
            using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                    r.Rows.Add(new FunctionRoleFullList(dr["RoleGUID"].ToString(), dr["RoleName"].ToString(),
                        rs.Contains(dr["RoleGUID"].ToString()) ? true : false));
                //r.Rows.Add(new MemberRoleFullList(dr["RoleGUID"].ToString(), dr["RoleName"].ToString(), (dr["Belongs"] == DBNull.Value) ? false : true));
                dr.Close();
                dr = null;
            }

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(r);
        }

        public class FunctionRoleFullList : SystemMgmt_SubFunc
        {
            protected bool _Grants = false;

            public bool Grants { get { return this._Grants; } set { this._Grants = value; } }

            public FunctionRoleFullList(string RoleGUID, string RoleName, bool Grants)
                : base(RoleGUID, RoleName)
            {
                this._Grants = Grants;
            }
        }

        public class SystemMgmt_FunctionRole_jQGridJSon
        {
            public List<FunctionRoleFullList> Rows = new List<FunctionRoleFullList>();
            public string Total
            {
                get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 10)).ToString(); }
            }
            public string Page { get { return "1"; } }
            public string Records { get { return Rows.Count.ToString(); } }
        }
        #endregion
        
        #region Get Sub Func Count
        public static string GetSubFuncCount(SqlConnection cn, string FunctionGUID)
        {
            int r = 0;

            if (FunctionGUID != null)
                if (FunctionGUID != "")
                {
                    string sSQL = "SELECT Count(*) FROM TB_SQM_SUBFUNC_MAP sfm WITH (NOLOCK) ";
                    sSQL += "Where sfm.FunctionGUID = @FunctionGUID;";

                    using (SqlCommand cmd = new SqlCommand(sSQL, cn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@FunctionGUID", FunctionGUID));
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
        
        #region SearchFunc
        public static string GetDataToJQGridJSon(SqlConnection cn)
        {
            return GetDataToJQGridJSon(cn, "");
        }

        public static string GetDataToJQGridJSon(SqlConnection cn, string SearchText)
        {
            SQMMgmt_SubFuncs_jQGridJSon m = new SQMMgmt_SubFuncs_jQGridJSon();

            string sSearchText = SearchText.Trim();
            
            string sWhereClause = "";
            if (sSearchText != "")
                sWhereClause += " and ((SubFuncName like '%' + @SearchText + '%') )";
            
            if (sWhereClause.Length != 0)
                sWhereClause = " Where" + sWhereClause.Substring(4);

            m.Rows.Clear();
            int iRowCount = 0;
            string sSQL = "SELECT Top 100 SubFuncGUID, SubFuncName FROM TB_SQM_SUBFUNC WITH (NOLOCK)";
            sSQL += sWhereClause + " Order By SubFuncGUID;";
            SqlCommand cmd = new SqlCommand(sSQL, cn);
            if (sSearchText != "")
                cmd.Parameters.Add(new SqlParameter("@SearchText", sSearchText));
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                iRowCount++;
                m.Rows.Add(new SQMMgmt_SubFuncs(
                    dr["SubFuncGUID"].ToString(),
                    HttpUtility.HtmlEncode(dr["SubFuncName"].ToString())));
            }
            dr.Close();
            dr = null;
            cmd = null;

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(m);
        }
        #endregion

        #region SearchFuncMap
        public static string GetSubFuncMapToJQGridJSon(SqlConnection cn,String FunctionGUID)
        {
            return GetSubFuncMapToJQGridJSon(cn, "", FunctionGUID);
        }

        public static string GetSubFuncMapToJQGridJSon(SqlConnection cn, string SearchText, String FunctionGUID)
        {
            SQMMgmt_SubFuncsMap_jQGridJSon m = new SQMMgmt_SubFuncsMap_jQGridJSon();

            string sSearchText = SearchText.Trim();

            string sWhereClause = "";
            sWhereClause += " and (SFM.SubFuncGUID=SF.SubFuncGUID)";
            if (FunctionGUID != "")
                sWhereClause += " and (FunctionGUID=@FunctionGUID)";
            if (sSearchText != "")
                sWhereClause += " and ((SubFuncName like '%' + @SearchText + '%') )";

            if (sWhereClause.Length != 0)
                sWhereClause = " Where" + sWhereClause.Substring(4);

            m.Rows.Clear();
            int iRowCount = 0;
            string sSQL = "SELECT Top 100 SFM.FunctionGUID,SFM.SubFuncGUID, SF.SubFuncName FROM TB_SQM_SUBFUNC_MAP SFM,TB_SQM_SUBFUNC SF WITH (NOLOCK)";
            sSQL += sWhereClause + " Order By SFM.SubFuncGUID;";
            SqlCommand cmd = new SqlCommand(sSQL, cn);
            if (sSearchText != "")
                cmd.Parameters.Add(new SqlParameter("@SearchText", sSearchText));
            if (FunctionGUID != "")
                cmd.Parameters.Add(new SqlParameter("@FunctionGUID", FunctionGUID));
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                iRowCount++;
                m.Rows.Add(new SQMMgmt_SubFuncsMap(
                    dr["FunctionGUID"].ToString(),
                    dr["SubFuncGUID"].ToString(),
                    HttpUtility.HtmlEncode(dr["SubFuncName"].ToString())));
            }
            dr.Close();
            dr = null;
            cmd = null;

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(m);
        }
        #endregion

        #region Remove Sub Func(s)
        public static string RemoveaSubFunc(SqlConnection cnPortal, string SubFuncGUID, string FunctionGUID)
        { return RemoveSubFunc(cnPortal, SubFuncGUID, RemoveOptions.Remove1, FunctionGUID); }

        public static string RemoveaSubFunc(SqlConnection cnPortal, string FunctionGUID)
        { return RemoveSubFunc(cnPortal, "", RemoveOptions.RemoveAll, FunctionGUID); }

        private static string RemoveSubFunc(SqlConnection cnPortal, string SubFuncGUID, RemoveOptions RO, string FunctionGUID)
        {
            string r = "";
            string sSQL = "Delete From TB_SQM_SUBFUNC_MAP Where FunctionGUID = @FunctionGUID";
            if (RO == RemoveOptions.Remove1) sSQL += " And SubFuncGUID = @SubFuncGUID;";
            sSQL += ";";
            using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal))
            {
                cmd.Parameters.Add(new SqlParameter("@FunctionGUID", FunctionGUID));
                if (RO == RemoveOptions.Remove1) cmd.Parameters.Add(new SqlParameter("@SubFuncGUID", SubFuncGUID));
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { r = e.Message; }
            }
            return r;
        }
        #endregion

        #region Add a Sub Func
        public static string AddaSubFunc(SqlConnection cnPortal, string FunctionGUID, string SubFuncGUID)
        {
            string r = "";
            int c = 0;
            string sSQL = "Select Count(*) From TB_SQM_SUBFUNC_MAP WITH (NOLOCK) Where FunctionGUID = @FunctionGUID And SubFuncGUID = @SubFuncGUID;";
            using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal))
            {
                cmd.Parameters.Add(new SqlParameter("@FunctionGUID", FunctionGUID));
                cmd.Parameters.Add(new SqlParameter("@SubFuncGUID", SubFuncGUID));
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                    c = (int)dr[0];
                dr.Close();
                dr = null;
            }
            if (c > 0) { r = "The selected sub func is the Function's member already."; }
            else
            {
                sSQL = "Insert Into TB_SQM_SUBFUNC_MAP (FunctionGUID, SubFuncGUID) Values (@FunctionGUID, @SubFuncGUID);";
                using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal))
                {
                    cmd.Parameters.Add(new SqlParameter("@FunctionGUID", FunctionGUID));
                    cmd.Parameters.Add(new SqlParameter("@SubFuncGUID", SubFuncGUID));
                    try { cmd.ExecuteNonQuery(); }
                    catch (Exception e) { r = e.Message; }
                }
            }
            return r;

        }
        #endregion

        #region Get Role Sub Func Count
        public static string GetRoleSubFuncCount(SqlConnection cn, string FunctionGUID, string RoleGUID)
        {
            int r = 0;

            if (FunctionGUID != null)
                if (FunctionGUID != "" && RoleGUID !="")
                {
                    string sSQL = "SELECT Count(*) FROM TB_SQM_ROLE_FUNC_MAP rfm WITH (NOLOCK) ";
                    sSQL += "Where rfm.FunctionGUID = @FunctionGUID";
                    sSQL += " AND rfm.RoleGUID = @RoleGUID;";

                    using (SqlCommand cmd = new SqlCommand(sSQL, cn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@FunctionGUID", FunctionGUID));
                        cmd.Parameters.Add(new SqlParameter("@RoleGUID", RoleGUID));
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
        
        #region SearchRoleFuncMap
        public static string GetRoleSubFuncMapToJQGridJSon(SqlConnection cn, String FunctionGUID, String RoleGUID)
        {
            return GetRoleSubFuncMapToJQGridJSon(cn, "", FunctionGUID, RoleGUID);
        }

        public static string GetRoleSubFuncMapToJQGridJSon(SqlConnection cn, string SearchText, String FunctionGUID, String RoleGUID)
        {
            SQMMgmt_RoleSubFuncsMap_jQGridJSon m = new SQMMgmt_RoleSubFuncsMap_jQGridJSon();
            String sRoleGUID = RoleGUID;
            if (String.IsNullOrEmpty(sRoleGUID))
                sRoleGUID = "-1";
            string sSearchText = SearchText.Trim();

            string sWhereClause = "";
            sWhereClause += " and (SFM.SubFuncGUID=SF.SubFuncGUID)";
            if (FunctionGUID != "")
                sWhereClause += " and (FunctionGUID=@FunctionGUID)";
            if (sRoleGUID != "")
                sWhereClause += " and (RoleGUID=@RoleGUID)";
            if (sSearchText != "")
                sWhereClause += " and ((SubFuncName like '%' + @SearchText + '%') )";

            if (sWhereClause.Length != 0)
                sWhereClause = " Where" + sWhereClause.Substring(4);

            m.Rows.Clear();
            int iRowCount = 0;
            string sSQL = "SELECT Top 100 SFM.FunctionGUID,SFM.RoleGUID,SFM.SubFuncGUID, SF.SubFuncName FROM TB_SQM_ROLE_FUNC_MAP SFM,TB_SQM_SUBFUNC SF WITH (NOLOCK)";
            sSQL += sWhereClause + " Order By SFM.SubFuncGUID;";
            SqlCommand cmd = new SqlCommand(sSQL, cn);
            if (sSearchText != "")
                cmd.Parameters.Add(new SqlParameter("@SearchText", sSearchText));
            if (FunctionGUID != "")
                cmd.Parameters.Add(new SqlParameter("@FunctionGUID", FunctionGUID));
            if (RoleGUID != "")
                cmd.Parameters.Add(new SqlParameter("@RoleGUID", sRoleGUID));
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                iRowCount++;
                m.Rows.Add(new SQMMgmt_RoleSubFuncsMap(
                    dr["FunctionGUID"].ToString(),
                    dr["RoleGUID"].ToString(),
                    dr["SubFuncGUID"].ToString(),
                    HttpUtility.HtmlEncode(dr["SubFuncName"].ToString())));
            }
            dr.Close();
            dr = null;
            cmd = null;

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(m);
        }
        #endregion

        #region Remove Role Sub Func(s)
        public static string RemoveaRoleSubFunc(SqlConnection cnPortal,string FunctionGUID, string RoleGUID, string SubFuncGUID)
        { return RemoveRoleSubFunc(cnPortal, RemoveOptions.Remove1, FunctionGUID, RoleGUID, SubFuncGUID); }

        public static string RemoveaRoleSubFunc(SqlConnection cnPortal, string FunctionGUID, string RoleGUID)
        { return RemoveRoleSubFunc(cnPortal, RemoveOptions.RemoveAll, FunctionGUID, RoleGUID,""); }

        private static string RemoveRoleSubFunc(SqlConnection cnPortal, RemoveOptions RO, string FunctionGUID, string RoleGUID, string SubFuncGUID)
        {
            string r = "";
            string sSQL = "Delete From TB_SQM_ROLE_FUNC_MAP Where FunctionGUID = @FunctionGUID";
            sSQL += " And RoleGUID = @RoleGUID";
            if (RO == RemoveOptions.Remove1) sSQL += " And SubFuncGUID = @SubFuncGUID";
            sSQL += ";";
            using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal))
            {
                cmd.Parameters.Add(new SqlParameter("@FunctionGUID", FunctionGUID));
                cmd.Parameters.Add(new SqlParameter("@RoleGUID", RoleGUID));
                if (RO == RemoveOptions.Remove1) cmd.Parameters.Add(new SqlParameter("@SubFuncGUID", SubFuncGUID));
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { r = e.Message; }
            }
            return r;
        }
        #endregion

        #region Add a Role Sub Func
        public static string AddaRoleSubFunc(SqlConnection cnPortal, string FunctionGUID, string RoleGUID, string SubFuncGUID)
        {
            string r = "";
            int c = 0;
            string sSQL = "Select Count(*) From TB_SQM_ROLE_FUNC_MAP WITH (NOLOCK) Where FunctionGUID = @FunctionGUID And RoleGUID = @RoleGUID And SubFuncGUID = @SubFuncGUID;";
            using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal))
            {
                cmd.Parameters.Add(new SqlParameter("@FunctionGUID", FunctionGUID));
                cmd.Parameters.Add(new SqlParameter("@RoleGUID", RoleGUID));
                cmd.Parameters.Add(new SqlParameter("@SubFuncGUID", SubFuncGUID));
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                    c = (int)dr[0];
                dr.Close();
                dr = null;
            }
            if (c > 0) { r = "The selected sub func is the Role Function's member already."; }
            else
            {
                sSQL = "Insert Into TB_SQM_ROLE_FUNC_MAP (FunctionGUID, RoleGUID, SubFuncGUID) Values (@FunctionGUID, @RoleGUID, @SubFuncGUID);";
                using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal))
                {
                    cmd.Parameters.Add(new SqlParameter("@FunctionGUID", FunctionGUID));
                    cmd.Parameters.Add(new SqlParameter("@RoleGUID", RoleGUID));
                    cmd.Parameters.Add(new SqlParameter("@SubFuncGUID", SubFuncGUID));
                    try { cmd.ExecuteNonQuery(); }
                    catch (Exception e) { r = e.Message; }
                }
            }
            return r;

        }
        #endregion
    }
}
