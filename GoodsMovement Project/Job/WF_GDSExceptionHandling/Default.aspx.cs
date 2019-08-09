using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web.UI;
using LiteOn.ea.SPM3G.UI;
using System.IO;
using Ext.Net;
using LiteOn.EA.Borg.Utility;
using LiteOn.EA.CommonModel;
using LiteOn.EA.CE.Utility;
using LiteOn.EA.DAL;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Text.RegularExpressions;
using LiteOn.GDS.Utility;

public partial class _Default : System.Web.UI.Page
{
    private GDSExceptionHandlingLogics oFlowLogics;
    private GDSExceptionHandlingUIShadow oUIControls;
    static string conn = ConfigurationManager.AppSettings["DBConnectionUAT"];
    static SqlDB sdb = new SqlDB(conn);
    ArrayList opc = new ArrayList();
    StringBuilder sb = new StringBuilder();
    GDS_Helper oStandard = new GDS_Helper();

    protected void Page_Load(object sender, EventArgs e)
    {
        //內嵌JS
        HtmlGenericControl ctrl = new HtmlGenericControl("script");
        ctrl.Attributes.Add("type", "text/javascript");
        ctrl.Attributes.Add("src", @"JScript.js");
        this.Page.Header.Controls.Add(ctrl);
        SpmMaster _Master = (SpmMaster)Master;
        oUIControls = new GDSExceptionHandlingUIShadow(this);
        oFlowLogics = new GDSExceptionHandlingLogics(this, oUIControls);
        oFlowLogics.PageLoad(_Master.IFormURLPara);


        /// Register MasterPage events
        _Master.MasterPageEvent_EFFormFieldsValidation += new SpmMaster.EFFormFieldsValidationHandler(oFlowLogics.EFFormFieldsValidation);
        _Master.MasterPageEvent_PrepareEFFormFields += new SpmMaster.PrepareEFFormFieldsHandler(oFlowLogics.PrepareEFFormFields);
        _Master.MasterPageEvent_PrepareSPMVariables += new SpmMaster.PrepareSPMVariablesHandler(oFlowLogics.PrepareSPMVariables);
        _Master.MasterPageEvent_InitialContainer += new SpmMaster.InitialContainerHandler(oFlowLogics.InitialContainer);
        _Master.MasterPageEvent_InitialDisableContainer += new SpmMaster.InitialDisableContainerHandler(oFlowLogics.InitialDisableContainer);
        _Master.MasterPageEvent_SPMBeforeSend += new SpmMaster.SPM_BeforeSendHandler(oFlowLogics.SPMBeforeSend);
        _Master.MasterPageEvent_SPMAfterSend += new SpmMaster.SPM_AfterSendHandler(oFlowLogics.SPMAfterSend);
        _Master.MasterPageEvent_SPMRecallProcess += new SpmMaster.SPM_RecallProcessHandler(oFlowLogics.SPMRecallProcess);
        _Master.MasterPageEvent_SPMBackoutProcess += new SpmMaster.SPM_BackoutProcessHandler(oFlowLogics.SPMBackoutProcess);
        _Master.MasterPageEvent_SPMStepComplete += new SpmMaster.SPM_StepCompleteHandler(oFlowLogics.SPMStepComplete);
        _Master.MasterPageEvent_SPMStepActivity += new SpmMaster.SPM_StepActivityHandler(oFlowLogics.SPMStepActivity);
        _Master.MasterPageEvent_Print += new SpmMaster.PrintHandler(oFlowLogics.Print);
        _Master.MasterPageEvent_SPMSendError += new SpmMaster.SPM_SendErrorHandler(oFlowLogics.SPM_SendError);
        // _Master.MasterPageEvent_SPMSendSuccess += new SpmMaster.SPM_SendSuccessHandler(oFlowLogics.SPM_SendSuccess);
        //_Master.MasterPageEvent_SPMSendSuccessNotice += new SpmMaster.SPM_SendSuccessNoticeHandler(oFlowLogics.SPM_SendSuccessNotice); 

        /*
         * Properties of Master Page
         * _Master.Manual                   : string. set link for [Manual]. e.g. "http://yahoo.com.tw"
         * _Master.HelpDesk                 : string. set link for [HelpDesk]. e.g. "http://10.1.13.61/wwwroot.zip";
         * _Master.BannerText1  &
         * _Master.BannerText2              : string. set info to show in [Banner]. e.g. "Example 3"
         * _Master.SelectPersonnelRowLimit  : integer. set the display count for [CCNotice]. e.g. 5
         * _Master.EnableShowProcessLogStepName : boolean. if the Process Log shows [StepName]. e.g. true
         * _Master.HeadLiteral              : string. set Head script. e.g. "<link href=\"Common/style.css\" rel=\"stylesheet\" type=\"text/css\" />"
         * _Master.LogoPath                 : string. set Logo path. e.g. "common/images/logo.gif";
         * _Master.AsyncPostBackTimeout     : integer. set AsyncPostBackTimeout. e.g. 300
         * _Master.WindowPrintEnable        : boolean. enable print function. e.g. true
         * _Master.ReferenceJavaScriptPath  : string array. set referenced script file. e.g. new string[1] { "~/JS/jquery.js" };
         */

        if (!IsPostBack)
        {
            _Master.Manual = "http://home-cz/Sites/CORP_IT/DocLib1/Forms/AllItems.aspx?RootFolder=%2FSites%2FCORP%5FIT%2FDocLib1%2FWorkflow%20%E7%9B%B8%E9%97%9C%E6%93%8D%E4%BD%9C%E6%89%8B%E5%86%8A&FolderCTID=0x012000F6570CA28FE20A4E9C70F18B590D6D16&View={F275949A-55A8-4AA7-8764-3B524F1B40D2}";
            _Master.HelpDesk = "http://www.liteon.com.tw/SPM/Example/Help.zip";
            _Master.BannerText1 = "Liteon CZ";
            _Master.BannerText2 = "GDS_應退未退申請表";
            _Master.SelectPersonnelRowLimit = 5;
            _Master.EnableShowProcessLogStepName = true;
            _Master.HeadLiteral = "<link href=\"Common/style.css\" rel=\"stylesheet\" type=\"text/css\" />";
            //_Master.LogoPath = "common/images/logo.gif";
            _Master.AsyncPostBackTimeout = 300;
            _Master.WindowPrintEnable = false;
            //_Master.ReferenceJavaScriptPath = new string[1] { "~/JS/jquery.js" };

            //set client side script
            //if (_Master.IFormURLPara.HandleType == "1")//Create New Case
            //{
            //_Master.ButtonSubmitClientOnClick = "alert('submit click')";
            //}
            //else if (_Master.IFormURLPara.StepName == "Direct Manager")
            //{
            /*
             * Add required code here.
             * Sample:
             * _Master.ButtonApproveClientOnClick = "alert('approve click')";
             * _Master.ButtonRejectClientOnClick = "alert('reject')";
             * _Master.ButtonAbortClientOnClick = "alert('abort')";
             * _Master.ButtonRecallClientOnClick = "alert('recall')";
            */
            //    _Master.ButtonRejectClientOnClick = "event.returnValue=false;";
            //}

            // Applicant Setting
            LiteOn.ea.SPM3G.UserInfoClass.UserInfoControlSetting lUC = new LiteOn.ea.SPM3G.UserInfoClass.UserInfoControlSetting();
            lUC.SingleSelect = true;
            lUC.Display = false;

            if (_Master.IFormURLPara.StepName.ToUpper().Equals("BEGIN") &&   //Create New Case 
                _Master.IFormURLPara.HandleType == "1")
                lUC.ChangeUserEnabled = true;
            else
                lUC.ChangeUserEnabled = false;

            //ScaleType.Dept Sample
            /*
            LiteOn.ea.SPM3G.UserInfoClass.UserSearhScaleSetting lSetting = new LiteOn.ea.SPM3G.UserInfoClass.UserSearhScaleSetting();
            lSetting.Scale = LiteOn.ea.SPM3G.UserInfoClass.ScaleType.Department;
            lSetting.DeptNo = "50015866";
            lUC.UserSearchControlScaleSetting = lSetting;
            */

            //ScaleType.Custom Sample 2-1
            /*
            LiteOn.ea.SPM3G.UserInfoClass.UserSearhScaleSetting lSetting = new LiteOn.ea.SPM3G.UserInfoClass.UserSearhScaleSetting();
            lSetting.Scale = LiteOn.ea.SPM3G.UserInfoClass.ScaleType.Custom;
            lSetting.CustomScaleSetting = GetCustomSearchSetting(); ;
            lUC.UserSearchControlScaleSetting = lSetting;            
            */
            _Master.ApplicantControlSetting = lUC;
            // ScriptManager.RegisterStartupScript(this.Page, typeof(string), "setListBoxValue", "SPM_onload();", true);


            Model_BorgUserInfo oModel_BorgUserInfo = new Model_BorgUserInfo();
            Borg_User oBorg_User = new Borg_User();
            oModel_BorgUserInfo = oBorg_User.GetUserInfoByLogonId(_Master.IFormURLPara.LoginId);
            if (oModel_BorgUserInfo._EXISTS)
            {
                lblSite.Text = Borg_Tools.GetSiteInfo();
                lblBu.Text = oModel_BorgUserInfo._BU;
                lblLogonId.Text = oModel_BorgUserInfo._LOGON_ID;
            }
            //txtRDocNo.Text = "I119070003";
            //txtWERKS_A.Text = "2680";
        }
    }

    /// <summary>
    /// Link I1單 帶出相關資料
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLink_Click(object sender, DirectEventArgs e)
    {
        string I1DocNo = txtRDocNo.Text.Trim();//Link 單號
        string WERKS_A = txtWERKS_A.Text.Trim();//廠別
        if (I1DocNo.Length == 0 || WERKS_A.Length == 0)
        {
            Alert("領料單號,廠別不能為空!");
            return;
        }
        else
        {
            txtMaterial.Text = string.Empty;
            RefreshMapping();
            DataTable dtHead = oStandard.GetdtHead(txtRDocNo.Text.Trim(),WERKS_A);
            DataTable dtDetail = oStandard.GetdtDetail(txtRDocNo.Text.Trim(), WERKS_A,"");
            if (dtHead.Rows.Count > 0)//Check是否符合應退未退的條件
            {
                DataRow drHead = dtHead.Rows[0];
                DataRow drDetail = dtDetail.Rows[0];
                if (drHead["APTYP"].ToString() == "I1")
                {
                    if (drHead["RTNIF"].ToString() == "Y")//符合條件時,加載基本資料 
                    {
                        txtWERKS.Text = drHead["WERKS"].ToString();
                        if (oStandard.CheckFormNoIsPass(I1DocNo, txtWERKS.Text))//Check單號是否過帳
                        {
                            #region 基本資料加載
                            txtDocNo.Text = oStandard.CreateFormNo(txtRDocNo.Text.Trim());
                            txtCostCenter.Text = drHead["KOSTL"].ToString();//成本中心
                            txtDepartment.Text = drHead["ABTEI"].ToString();//部門
                            txtApplication.Text = drHead["ERNAM"].ToString();//LoginID
                            txtReturn.Text = drHead["RTNIF"].ToString();// Return Flag
                            txtAPTYP.Text = drHead["APTYP"].ToString();
                            BindMaterial(txtRDocNo.Text.Trim());//根據單號綁定料號

                            string xmlstrHead = oStandard.DataTableToXMLStr(dtHead);
                            txtHead.Text = xmlstrHead;//Head
                            string xmlstrDetail = oStandard.DataTableToXMLStr(dtDetail);
                            txtDetail.Text = xmlstrDetail;//Detail
                            string tempWEKS = DOA.GetXMLConfigName(dtHead);//CALL FUNCTION獲取XML配置檔名
                            SettingParser x = new SettingParser(tempWEKS, txtAPTYP.Text);//讀取XML配置檔信息
                            txtDOA.Text = x.ApproveXML.Replace("&lt1;", "<").Replace("&gt1;", ">"); ;//抓取DOA的簽核邏輯

                            //計算總金額
                            double amount = 0;
                            foreach (DataRow dr in dtDetail.Rows)
                            {
                                amount += double.Parse(dr["STPRS"].ToString());
                            }
                            txtAmount.Text = amount.ToString("n");


                            btnQuery.Hidden = false;
                            #endregion

                            #region 綁定Batch Call BAPI抓取已經過帳的領料單中的Batch 材料沒有Batch  成品一定有Batch(沒過帳的單子可能沒有Batch,因此抓取已經過帳的單子)
                            DataTable dtDetail_P = oStandard.GetdtDetail(txtRDocNo.Text.Trim(), txtWERKS.Text);
                            //根據I1單的dtDetail 綁定Batch的下拉框參數(可能為空) 
                            BindBatch(dtDetail_P);
                            #endregion
                        }
                        else
                        {
                            Alert("此I1單未簽核完畢！");
                            Refresh();
                            return;
                        }
                    }
                    else
                    {
                        Alert("此單號不符合應退未退的條件！");
                        Refresh();
                        return;
                    }
                }
                else
                {
                    Alert("Link單號必須是I1單！");
                    Refresh();
                    return;
                }

            }
            else
            {
                Alert("此單號不存在！");
                Refresh();
                return;
            }
        }
    }

    /// <summary>
    /// 按鈕觸發料號
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnQuery_Click(object sender, DirectEventArgs e)
    {
        string LinkDocNo = txtRDocNo.Text.Trim();//Link單號,領料單號
        string Material = txtMaterial.Text.Trim();
        string WERKS = txtWERKS.Text.Trim();
        string BatchNumber = txtBatch.Text.Trim();

        if (txtRDocNo.Text.Length == 0 || txtMaterial.Text.Length == 0)
        {
            Alert("Link的領料單,料號不能為空！");
            RefreshMapping();
            //btnClear.Hidden = true;
            //btnLink.Hidden = false;
            return;
        }
        else if (txtBatch.Text.Length == 0)
        {
            DataTable dtDetail = oStandard.GetdtDetail(txtRDocNo.Text.Trim(), WERKS);
            if (oStandard.CheckBatchIsEmpty(dtDetail))
            {
                if (txtBatch.Text.Length == 0) Alert("此領料單有Batch號,不能為空！"); return;
            }
        }
        else
        {
            //RefreshMapping();
            btnClear.Hidden = false;
            txtMaterial.Disabled = true;
            txtBatch.Disabled = true;
            //Bactn Number 做特殊處理 如果dtDetail中的CHARG為空,則可以為空 Function去Check 

            #region 因單據過帳才有POSTED的值 需重新Call BAPI抓取已過帳單據中的ZEILE_A(Item)、POSTED(領料單數量)
            DataTable dtDetail = oStandard.GetdtDetail(txtRDocNo.Text.Trim(), WERKS);
            DataRow[] drItem = dtDetail.Select(string.Format("CHARG = '{0}' and MATNR = '{1}'", BatchNumber,Material));//放两个条件,CHARG AND MATNR
            if (drItem.Length > 0)
            {
                #region Get POSTED AND ZEILE
                DataTable tempT = new DataTable();
                tempT = drItem[0].Table.Clone();
                DataSet tempDs = new DataSet();
                tempDs.Tables.Add(tempT);
                tempDs.Merge(drItem);
                DataTable dtItem = tempDs.Tables[0];
                //Item
                txtZEILE.Text = dtItem.Rows[0]["ZEILE_A"].ToString();
                //領料單數量  抓Posted
                txtMENGE.Text = dtItem.Rows[0]["POSTED"].ToString();
                #endregion

                #region 抓取Approve的退料單 綁定到grdItems 計算Open數量
                DataTable dtMapping = oStandard.GetMappingList(LinkDocNo, Material, "A", BatchNumber);
                if (dtMapping.Rows.Count > 0)
                {
                    int sum = 0;
                    foreach (DataRow dr in dtMapping.Rows)
                    {
                        if (dr["Posted"].ToString().Length == 0)
                        {
                            int Quantity = int.Parse(dr["MENGE"].ToString());
                            sum += Quantity;

                        }
                        else
                        {
                            int Quantity = int.Parse(dr["POSTED"].ToString() == "0" ? dr["MENGE"].ToString() : dr["POSTED"].ToString());
                            sum += Quantity;

                        }
                    }
                    txtLinkReturn_Q.Text = sum.ToString();

                    grdItems.Store.Primary.LoadData(dtMapping);
                }
                else
                {
                    txtLinkReturn_Q.Text = "0";
                    grdItems.Store.Primary.DataSource = new DataTable();
                    grdItems.Store.Primary.DataBind();
                }
                #endregion

                #region 加載異常單資料 (在簽,Approve) BindGridPanel  -- grdException
                DataTable dtM = oStandard.GetMaster_Exception(LinkDocNo, Material, BatchNumber);
                if (dtM.Rows.Count > 0)
                {
                    //如果此领料单有历史单据 绑定到Grid  歷史回退數量  = 關聯退料單數量(Approve) + 異常單數量(在簽和Approve)
                    var Count = dtM.Compute("Sum(MENGE)", "");//異常單數量

                    txtReturn_A.Text = (int.Parse(Count.ToString()) + int.Parse(txtLinkReturn_Q.Text)).ToString();


                    grdException.Store.Primary.LoadData(dtM);
                }
                else
                {
                    //没有历史异常单,Open数量为0
                    txtReturn_A.Text = txtLinkReturn_Q.Text;
                    grdException.Store.Primary.DataSource = new DataTable();
                    grdException.Store.Primary.DataBind();

                }
                #endregion

                #region 计算 Open数量 (領料單數量 - 歷史單據數量的和)
                int MENGE = int.Parse(txtMENGE.Text.Length == 0 ? "0" : txtMENGE.Text);
                int Return_AA = int.Parse(txtReturn_A.Text.Length == 0 ? "0" : txtReturn_A.Text);
                string Open_Q = (MENGE - Return_AA).ToString();
                if (int.Parse(Open_Q) == 0)
                {
                    txtOpen.Text = Open_Q;
                    Alert("Open數量為0,無法在進行退料作業");
                    //Refresh_Open();
                    return;
                }
                else
                {
                    txtOpen.Text = Open_Q;
                }

                #endregion

            }
            else
            {
                Alert("此料號和Batch號無對應單據!");
                return;
            }
            #endregion

        }
    }

    /// <summary>
    /// 料號加載資料 Clear功能
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, DirectEventArgs e)
    {
        RefreshClear();
    }

    #region Refresh Function

    public void Refresh()
    {
        txtRDocNo.Text = string.Empty;
        txtWERKS_A.Text = string.Empty;
        txtDocNo.Text = string.Empty;
        txtCostCenter.Text = string.Empty;
        txtDepartment.Text = string.Empty;
        txtApplication.Text = string.Empty;
        txtMaterial.Text = string.Empty;
        txtReturnQuantity.Text = string.Empty;
        txtReason.Text = string.Empty;
        txtRemark.Text = string.Empty;
        txtReturn.Text = string.Empty;
        txtAmount.Text = string.Empty;

        txtHead.Text = string.Empty;
        txtDetail.Text = string.Empty;
        txtDOA.Text = string.Empty;
        txtWERKS.Text = string.Empty;
        txtAPTYP.Text = string.Empty;
        btnQuery.Hidden = true;

        //grdException.Store.Primary.DataSource = new DataTable();
        //grdException.Store.Primary.DataBind();

    }

    /// <summary>
    /// Link按鈕 清空選擇料號關聯后的資料
    /// </summary>
    public void RefreshMapping()
    {

        txtMaterial.Text = string.Empty;
        txtReason.Text = string.Empty;
        txtRemark.Text = string.Empty;
        txtMENGE.Text = string.Empty;
        txtReturn_A.Text = string.Empty;
        txtZEILE.Text = string.Empty;
        txtBatch.Text = string.Empty;

        grdItems.Store.Primary.DataSource = new DataTable();
        grdItems.Store.Primary.DataBind();

        grdException.Store.Primary.DataSource = new DataTable();
        grdException.Store.Primary.DataBind();
    }

    public void Refresh_Open()
    {
        txtMaterial.Text = string.Empty;
        txtReason.Text = string.Empty;
        txtRemark.Text = string.Empty;
        txtMENGE.Text = string.Empty;
        txtReturn_A.Text = string.Empty;
        txtZEILE.Text = string.Empty;
        txtBatch.Text = string.Empty;
        txtOpen.Text = string.Empty;
        txtReturnQuantity.Text = string.Empty;

        grdItems.Store.Primary.DataSource = new DataTable();
        grdItems.Store.Primary.DataBind();

        grdException.Store.Primary.DataSource = new DataTable();
        grdException.Store.Primary.DataBind();
    }

    public void RefreshClear()
    {
        txtMaterial.Text = string.Empty;
        txtMENGE.Text = string.Empty;
        txtReturnQuantity.Text = string.Empty;
        txtReturn_A.Text = string.Empty;
        txtZEILE.Text = string.Empty;
        txtReason.Text = string.Empty;
        txtRemark.Text = string.Empty;
        txtOpen.Text = string.Empty;
        txtBatch.Text = string.Empty;

        txtMaterial.Disabled = false;
        txtBatch.Disabled = false;
        btnClear.Hidden = true;

        grdItems.Store.Primary.DataSource = new DataTable();
        grdItems.Store.Primary.DataBind();

        grdException.Store.Primary.DataSource = new DataTable();
        grdException.Store.Primary.DataBind();
    }

    #endregion

    #region Check Return Quantity
    /// <summary>
    /// 檢查Return Quntity欄位是否是數字
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void check_workorder(object sender, DirectEventArgs e)
    {
        string Text = txtReturnQuantity.Text.Trim();
        //先Check输入的是否是整数
        if (!IsNumber(Text))
        {
            Alert("Return Quantity只能是整數！");
            txtReturnQuantity.Text = string.Empty;
            return;
        }
        else
        {
            //输入的Return Quantity和Open数量做对比 先判斷Open大於0,在判斷Open數量 - RQ數量必須大於等於0
            if (int.Parse(txtOpen.Text) - int.Parse(txtReturnQuantity.Text) < 0)
            {
                Alert("回退數量必須小於等於Open數量！");
                txtReturnQuantity.Text = string.Empty;
                return;
            }


        }
    }

    public bool Check(string s)
    {
        string pattern = "^[0-1000]$";
        Regex rx = new Regex(pattern);
        return rx.IsMatch(s);
    }

    public static bool IsNumber(String checkNumber)
    {
        bool isCheck = true;

        if (string.IsNullOrEmpty(checkNumber))
        {
            isCheck = false;
        }
        else
        {
            char[] charNumber = checkNumber.ToCharArray();

            for (int i = 0; i < charNumber.Length; i++)
            {
                if (!Char.IsNumber(charNumber[i]))
                {
                    isCheck = false;
                    break;
                }
            }
        }

        return isCheck;
    }
    #endregion

    #region BindCombox -- 料號、Batch Number
    /// <summary>
    /// 根據單號綁定料號
    /// </summary>
    /// <param name="RDocNo"></param>
    private void BindMaterial(string RDocNo)
    {
        ComboBox[] cbs = new ComboBox[] { txtMaterial };
        DataTable data = oStandard.GetMaterial(RDocNo);
        foreach (ComboBox cb in cbs)
        {
            BindCombox(data, cb);
        }
    }

    /// <summary>
    /// 根據單號綁定Batch Number
    /// </summary>
    /// <param name="RDocNo"></param>
    private void BindBatch(DataTable dtDetail)
    {
        ComboBox[] cbs = new ComboBox[] { txtBatch };
        DataTable data = dtDetail;
        foreach (ComboBox cb in cbs)
        {
            BindCombox(data, cb);
        }
    }

    /// <summary>
    /// Bind Combox的公共方法
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="cb"></param>
    private void BindCombox(DataTable dt, ComboBox cb)
    {
        cb.Store.Primary.DataSource = dt;
        cb.Store.Primary.DataBind();
    }

    #endregion

    private void Alert(string msg)
    {
        X.Msg.Alert("Alert", msg).Show();
    }

}




//Check 领料单数量 - 退料单数量的和 是否大于0 By Batch Number and Material
//string LinkDocNo = txtRDocNo.Text.Trim();//Link單號,領料單號
//string Material = txtMaterial.Text.Trim();//料號
//string BatchNumber = txtBatch.Text.Trim();
//DataTable dtMapping = oStandard.GetMappingList(LinkDocNo, Material, "A", BatchNumber);
//int sum = 0;
//if (dtMapping.Rows.Count > 0)
//{
//    foreach (DataRow dr in dtMapping.Rows)
//    {
//        int Quantity = int.Parse(dr["POSTED"].ToString() == "0" ? dr["MENGE"].ToString() : dr["POSTED"].ToString());
//        sum += Quantity;
//    }
//    int C = int.Parse(txtOpen.Text) - sum;
//    if (C < 0)
//    {
//        Alert("作業失敗,Open數量減去關聯退料單的和小於0!");
//        txtReturnQuantity.Text = string.Empty;
//        return;
//    }
//    else
//    {
//        int RQ = int.Parse(txtReturnQuantity.Text);
//        if (int.Parse(txtOpen.Text) < RQ)
//        {
//            Alert("作業失敗,回退數量不能大於Open數量!");
//            txtReturnQuantity.Text = string.Empty;
//            return;
//        }
//    }
//}
//else
//{
//    Alert("作業失敗,缺少關聯的退料單信息!");
//    txtReturnQuantity.Text = string.Empty;
//    return;

//}



