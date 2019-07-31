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

        }
    }

    protected void btnLink_Click(object sender, DirectEventArgs e)
    {

        DataTable dtHead = oStandard.GetdtHead(txtRDocNo.Text.Trim());
        DataTable dtDetail = oStandard.GetdtDetail(txtRDocNo.Text.Trim());
        if (dtHead.Rows.Count > 0 )//Check是否符合應退未退的條件
        {
            DataRow drHead = dtHead.Rows[0];
            if (drHead["APTYP"].ToString() == "I1")
            {
                if (drHead["RTNIF"].ToString() == "Y")//符合條件時,加載基本資料 
                {
                    txtDocNo.Text = oStandard.CreateFormNo(txtRDocNo.Text.Trim());
                    txtCostCenter.Text = drHead["KOSTL"].ToString();//成本中心
                    txtDepartment.Text = drHead["ABTEI"].ToString();//部門
                    txtApplication.Text = drHead["ERNAM"].ToString();//LoginID
                    txtReturn.Text = drHead["RTNIF"].ToString();// Return Flag
                    txtWERKS.Text = drHead["WERKS"].ToString();
                    txtAPTYP.Text = drHead["APTYP"].ToString();

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

    public void Refresh()
    {
        txtRDocNo.Text = string.Empty;
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
        txtIADocNo.Text = string.Empty;
        txtI6DocNo.Text = string.Empty;
        
    }

    #region Check Return Quantity
    protected void check_workorder(object sender, DirectEventArgs e)
    {
        string Text = txtReturnQuantity.Text.Trim();
        if (!IsNumber(Text))
        {
            Alert("Return Quantity只能是整數！");
            txtReturnQuantity.Text = string.Empty;
            return;
        }
        else
        {

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

    private void Alert(string msg)
    {
        X.Msg.Alert("Alert", msg).Show();
    }



}



