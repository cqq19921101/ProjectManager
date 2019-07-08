using System;
using System.Collections;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using LiteOn.ea.SPM3G.UI;
using LiteOn.EA.CommonModel;
using LiteOn.EA.Borg.Utility;
using LiteOn.EA.NPIReport.Utility;
using Liteon.ICM.DataCore;
using LiteOn.EA.BLL;
using Ext.Net;
using OfficeOpenXml;
using System.Xml.Linq;
using System.Xml;
using System.Xml.Xsl;
public partial class _Default : System.Web.UI.Page
{
    private PilotRunReportLogics oFlowLogics;
    private PilotRunReportUIShadow oUIControls;
    private SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("SPM"));

    ArrayList opc = new ArrayList();

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl ctrl = new HtmlGenericControl("script");
        ctrl.Attributes.Add("type", "text/javascript");
        this.Page.Header.Controls.Add(ctrl);
        SpmMaster _Master = (SpmMaster)Master;
        oUIControls = new PilotRunReportUIShadow(this);
        oFlowLogics = new PilotRunReportLogics(this, oUIControls);
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
            _Master.Manual = "../PilotRunManual.ppt";
            _Master.HelpDesk = "http://www.liteon.com.tw/SPM/Example/Help.zip";
            _Master.BannerText1 = "";
            _Master.BannerText2 = "試產報告會簽表";
            _Master.SelectPersonnelRowLimit = 5;
            _Master.EnableShowProcessLogStepName = true;
            _Master.HeadLiteral = "<link href=\"Common/style.css\" rel=\"stylesheet\" type=\"text/css\" />";
            //_Master.LogoPath = "common/images/logo.gif";
            _Master.AsyncPostBackTimeout = 300;
            _Master.WindowPrintEnable = true;
            //_Master.ReferenceJavaScriptPath = new string[1] { "~/JS/jquery.js" };

            //set client side script
            if (_Master.IFormURLPara.HandleType == "1")//Create New Case
            {
                //_Master.ButtonSubmitClientOnClick = "alert('submit click')";
            }
            else if (_Master.IFormURLPara.StepName == "Direct Manager")
            {
                /*
                 * Add required code here.
                 * Sample:
                 * _Master.ButtonApproveClientOnClick = "alert('approve click')";
                 * _Master.ButtonRejectClientOnClick = "alert('reject')";
                 * _Master.ButtonAbortClientOnClick = "alert('abort')";
                 * _Master.ButtonRecallClientOnClick = "alert('recall')";
                */
                _Master.ButtonRejectClientOnClick = "event.returnValue=false;";
            }

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
                lblBuilding.Text = oModel_BorgUserInfo._Building;
                lblLogonId.Text = oModel_BorgUserInfo._LOGON_ID;
            }
            lblStepName.Text = _Master.IFormURLPara.StepName;
            logonid.Text = _Master.IFormURLPara.HandleType;
        }
    }

    /* Custom Setting Sample 2-2
    private LiteOn.ea.SPM3G.UserInfoClass.CustomScale GetCustomSearchSetting()
    {   
        LiteOn.ea.SPM3G.UserInfoClass.CustomScale lCustomSetting = new LiteOn.ea.SPM3G.UserInfoClass.CustomScale();
        lCustomSetting.SQLCommandText =
            @"SELECT TOP 50 USERID aa, LOGONID, USERNAME bb, CUST_12 cc  FROM [USER] 
                WHERE (USERNAME LIKE @KeyWord OR 
                LOGONID LIKE @KeyWord OR 
                CUST_12 LIKE @KeyWord) AND USERID > 110 ORDER BY USERID";
        lCustomSetting.KeyWordParameterName = "@KeyWord";
        lCustomSetting.UserIDColumnName = "aa";
        lCustomSetting.LoginIDColumnName = "bb";
        lCustomSetting.UserNameColumnName = "cc";
        return lCustomSetting;
    }
    */

    private void BindGrid(GridPanel grd, DataTable dt)
    {
        grd.Store.Primary.DataSource = dt;
        grd.Store.Primary.DataBind();
    }

    protected void btnQuery_click(object sender, DirectEventArgs e)
    {
        SqlDB sdbNPI = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        string logoinid = lblLogonId.Text;
        string prod_group = string.Empty;
        string model_name = string.Empty;
        string final_pn = string.Empty;
        string apply_date = string.Empty;
        StringBuilder errMsg = new StringBuilder();
        if (sbProd_group.SelectedIndex < 0)
        {
            errMsg.Append("請選擇產品類別");

        }
        if (errMsg.Length > 0)
        {
            Alert(errMsg.ToString());
            return;
        }
        prod_group = sbProd_group.SelectedItem.Value.Trim();
        model_name = txtModel.Text.Trim();
        final_pn = txtFinalPN.Text.Trim();
        apply_date = txtApplyDate.SelectedDate.ToShortDateString() == "1/1/0001" ? "" : txtApplyDate.SelectedDate.ToShortDateString();
        StringBuilder sql = new StringBuilder();
        opc.Clear();
        sql.Append("SELECT * FROM TB_NPI_APP_MAIN WHERE PROD_GROUP=@PROD_GROUP AND BU=@bu AND BUILDING=@building  and NPI_PM = @NPI_PM");
        opc.Add(DataPara.CreateDataParameter("@PROD_GROUP", DbType.String, prod_group));
        opc.Add(DataPara.CreateDataParameter("@bu", DbType.String, lblBu.Text));
        opc.Add(DataPara.CreateDataParameter("@building", DbType.String, lblBuilding.Text));
        opc.Add(DataPara.CreateDataParameter("@NPI_PM", DbType.String, lblLogonId.Text));

        if (model_name.Length > 0)
        {
            sql.Append(" AND MODEL_NAME like '%" + model_name + "%'");
        }

        if (apply_date.Length > 0)
        {
            sql.Append(" AND APPLY_DATE=@APPLY_DATE  ");
            opc.Add(DataPara.CreateDataParameter("@APPLY_DATE", DbType.String, apply_date));
        }
        BindGrid(grdInfo, sdbNPI.TransactionExecute(sql.ToString(), opc));
    }

    protected void grdInfo_RowCommand(object sender, DirectEventArgs e)
    {


        string PM = e.ExtraParams["NPI_PM"]; ;
        SpmMaster _Master = (SpmMaster)Master;
        if (PM.ToLower() == _Master.IFormURLPara.LoginId.ToLower())
        {
            txtDOC_NO.Text = e.ExtraParams["DOC_NO"];
            txtPROD_GROUP.Text = e.ExtraParams["PROD_GROUP"];
            txtModel_R.Text = e.ExtraParams["MODEL_NAME"];
            txtCustomer.Text = e.ExtraParams["CUSTOMER"];
            pnlStart.Show();
            grdInfo.Hide();
        }
        else
        {
            Alert("此帳號不具有起單權限,請確認!");
            return;
        }

    }

    /// <summary>
    /// 自定義ALERT方法
    /// </summary>
    /// <param name="msg"></param>
    private void Alert(string msg)
    {
        X.Msg.Alert("Alert", msg).Show();
    }

    private bool CheckNeedStartInfo()
    {
        int count = 0;

        for (int i = 0; i < cbStartItem.Items.Count; i++)
        {
            if (cbStartItem.Items[i].Checked == false)
            {
                count += 1;
            }
        }
        if (count >= cbStartItem.Items.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Get需启动项
    /// </summary>
    /// <returns></returns>

    private string GetNeedStartInfo()
    {

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < cbStartItem.Items.Count; i++)
        {
            if (cbStartItem.Items[i].Checked == true)
            {
                sb.AppendFormat("{0};", cbStartItem.Items[i].InputValue);
            }
        }
        return sb.ToString();
    }

    protected void btnStart_Click(object sender, DirectEventArgs e)
    {

        string doc_no = txtDOC_NO.Text;
        if (doc_no.Length == 0)
        {
            Alert("頁面失效請重新開啟");
            return;
        }
        if (sbPhase.SelectedIndex < 0)
        {
            Alert("請選擇階段");
            return;
        }

        if (CheckNeedStartInfo())
        {
            Alert("请勾选需启动项!");
            return;
        }

    }

    protected void sbPhase_Change(object sender, DirectEventArgs e)
    {
        string subPhase = sbPhase.SelectedItem.Text.Trim();
        if (subPhase.Contains("EVT"))
        {
            txtSub_No.Text = NPI_Tools.GetFormNO("EVT");
        }
        else if (subPhase.Contains("DVT"))
        {
            txtSub_No.Text = NPI_Tools.GetFormNO("DVT");
        }
        else if (subPhase.Contains("P.Run"))
        {
            txtSub_No.Text = NPI_Tools.GetFormNO("PR");
        }
        else
        {
            txtSub_No.Text = NPI_Tools.GetFormNO("PTT");
        }
    }

    protected void sbProd_group_Selected(object sender, DirectEventArgs e)
    {
        pnlStart.Hide();
        grdInfo.Show();
    }

    protected void cbModify_SelectedIndexChanged(object sender, DirectEventArgs e)
    {
        if (cbModify.Checked)
        {
            for (int i = 0; i < cbStartItem.Items.Count; i++)
            {
                cbStartItem.Items[i].Disabled = true;
                cbStartItem.Items[i].Checked = false;
            }
            txtPCB.Disabled = false;
            txtSPECRev.Disabled = false;
            txtInputDate.Disabled = false;
            txtBomRev.Disabled = false;
            txtCustomerRev.Disabled = false;
            txtPkDate.Disabled = false;

            pnlmodify.Hidden = false;
        }
        else
        {
            cbStartItem.Disabled = false;
            txtPCB.Disabled = true;
            txtSPECRev.Disabled = true;
            txtInputDate.Disabled = true;
            txtBomRev.Disabled = true;
            txtCustomerRev.Disabled = true;
            txtPkDate.Disabled = true;
            txtPCB.Text = string.Empty;
            txtSPECRev.Text = string.Empty;
            txtInputDate.Text = string.Empty;
            txtBomRev.Text = string.Empty;
            txtCustomerRev.Text = string.Empty;
            txtPkDate.Text = string.Empty;

            pnlmodify.Hidden = true;

            txtFromModel.Text = string.Empty;
            //txtLink.Text = string.Empty;
            txtModelRemark.Text = string.Empty;

            HyHomePage.NavigateUrl = string.Empty;
            HyHomePage.Text = string.Empty;

        }
    }

    protected void btnFromModel(object sender, DirectEventArgs e)
    {
        string Model = txtFromModel.Text.Replace(" ", "").ToUpper();
        string Stage = sbPhase.Text;
        DataTable dt = GetNewestModel(Model);
        if (dt.Rows.Count == 0)
        {
            Alert("請確認機種是否輸入正確！");
            txtFromModel.Text = string.Empty;
            lblhyperlink.Text = string.Empty;
            HyHomePage.Text = string.Empty;
            return;
        }
        else if (Stage.Length == 0)
        {
            Alert("請選擇Stage！");
            txtFromModel.Text = string.Empty;
            lblhyperlink.Text = string.Empty;
            HyHomePage.Text = string.Empty;
            return;
        }
        else
        {

            string url = "http://icm656.liteon.com/czweb/web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + "Home Page" + "/" + "Home Page.xlsx";
            HyHomePage.Text = "HomePage.xlsx";
            HyHomePage.NavigateUrl = url;
            lblhyperlink.Text = url;
            Alert("鏈接生成成功！");
        }

    }

    #region[DFXLis Write]


    protected void BindDFXItem(string subdoc, string Type)
    {
        SpmMaster _Master = (SpmMaster)Master;
        string StepName = _Master.IFormURLPara.StepName.ToString();
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        StringBuilder sb = new StringBuilder();
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        if (StepName == "Dept.Reply" || StepName == "Dept.Write")
        {
            string Dept = GetUserDept("AD", lblLogonId.Text, txtPROD_GROUP.Text, StepName);
            DataTable dt = oStandard.GetDFXInconformity(subdoc, Dept, StepName);
            BindGrid(grdDFXList, dt);
        }
        else
        {
            string Dept = oStandard.GetUserDept("A", lblLogonId.Text, txtPROD_GROUP.Text);
            DataTable dt = oStandard.GetDFXInconformity(subdoc, Dept, Type);
            BindGrid(grdDFXList, dt);
        }

    }

    public string GetUserDept(string Category)
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        string loginid = lblLogonId.Text;
        string Dept = string.Empty;
        string sql = "select DISTINCT  DEPT from TB_NPI_APP_MEMBER where WriteEname = @ENAME AND Category=@Category"
            + " AND DOC_NO=@Doc_No";
        ArrayList opc = new ArrayList();
        opc.Add(DataPara.CreateDataParameter("@ENAME", DbType.String, loginid));
        opc.Add(DataPara.CreateDataParameter("@Category", DbType.String, Category));
        opc.Add(DataPara.CreateDataParameter("@Doc_No", DbType.String, txtPROD_GROUP.Text));
        DataTable dt = sdb.TransactionExecute(sql, opc);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            Dept = dr["DEPT"].ToString();
        }
        return Dept;
    }

    protected void grdDFXList_RowCommand(object sender, DirectEventArgs e)
    {
        string command = e.ExtraParams["command"];
        switch (command)
        {
            case "Write":
                RefreshDFXInfo();
                txtDFXNo.Text = e.ExtraParams["DFXNo"];
                txtItems.Text = e.ExtraParams["Item"];
                txtRequirements.Text = e.ExtraParams["Requirements"];
                txtPriority.Text = e.ExtraParams["PriorityLevel"];
                cbCompliance.SelectedItem.Text = e.ExtraParams["Compliance"];
                txtMaxPoints.Text = e.ExtraParams["MaxPoints"];
                txtDFXPoints.Text = e.ExtraParams["DFXPoints"];
                txtLocation.Text = e.ExtraParams["Location"];
                txtDFXCommnet.Text = e.ExtraParams["Comments"];
                break;
            case "Reply":
                RefreshDFXReplyInfo();
                hidDoc.Text = e.ExtraParams["DFXNo"];
                hidItem.Text = e.ExtraParams["Item"];
                Item.Text = e.ExtraParams["Item"];
                Location.Text = e.ExtraParams["Location"];
                Requirements1.Text = e.ExtraParams["Requirements"];
                txtActions.Text = e.ExtraParams["Actions"];
                txtRemark.Text = e.ExtraParams["Remark"];
                cmbDFXStatus.SelectedItem.Text = e.ExtraParams["Tracking"];
                DFXDEPT.Text = e.ExtraParams["WriteDept"];
                txtCompleteDate.Text = e.ExtraParams["CompletionDate"];
                cmbDFXStatus.Text = e.ExtraParams["Tracking"];
                break;
        }





    }

    protected void RefreshDFXInfo()
    {
        pnlDFXInfo.Hidden = false;
        txtDFXNo.Text = "";
        txtItems.Text = "";
        txtRequirements.Text = "";
        cbCompliance.Text = "";
        txtLocation.Text = "";
        txtPriority.Text = "";
        txtMaxPoints.Text = "";
        txtDFXPoints.Text = "";
        txtDFXCommnet.Text = "";



    }

    protected void RefreshDFXReplyInfo()
    {
        pnlDFXReply.Hidden = false;
        txtCompleteDate.Text = "";
        //txtActions.Text = "";
        //txtRemark.Text = "";
        //for (int i = 0; i < cbGroupTracking.Items.Count; i++)
        //{
        //    cbGroupTracking.Items[i].Checked = false;
        //}

    }

    protected void btnSaveDFX_Click(object sender, DirectEventArgs e)
    {
        string DFXNo = txtDFXNo.Text.Trim();
        string Items = txtItems.Text.Trim();
        string Location = txtLocation.Text;

        string MaxPoints = txtMaxPoints.Text.Trim();
        string DFXPoints = txtDFXPoints.Text.Trim();

        string Compliance = cbCompliance.SelectedItem.Text;
        string Comment = txtDFXCommnet.Text.Trim();
        StringBuilder ErrMsg = new StringBuilder();

        #region 控件值驗證是否為空

        if (string.IsNullOrEmpty(Compliance))
        {
            ErrMsg.Append("Compliance不能為空</br>");
        }
        switch (Compliance)
        {
            case "N":
                if (string.IsNullOrEmpty(Location))
                {
                    ErrMsg.Append("Locaiton不能為空</br>");
                }
                if (string.IsNullOrEmpty(Comment))
                {
                    ErrMsg.Append("comments不能為空</br>");
                }

                break;
            default:
                break;
        }
        if (ErrMsg.ToString().Length > 0)
        {
            Alert(ErrMsg.ToString());
            return;
        }
        #endregion

        Model_DFX_ITEMBODY oModel = new Model_DFX_ITEMBODY();
        oModel._DFXNo = DFXNo;
        oModel._Item = Items;
        oModel._MaxPoints = MaxPoints;
        oModel._DFXPoints = DFXPoints;
        oModel._Compliance = Compliance;
        oModel._UpdateUser = lblLogonId.Text.Trim();
        oModel._Location = Location.Trim();
        oModel._Comments = txtDFXCommnet.Text.Trim();
        if (Compliance == "N")
        {
            oModel._Status = "Reply";
        }
        else
        {
            oModel._Status = "Finished";
        }
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();

        try
        {

            string tmp = oStandard.RecordOperation_DFXItems(oModel, Status_Operation.ADD);
            if (tmp.Length >= 3)
            {
                if (tmp.Substring(0, 2) == "NG")
                {
                    //接收SP返回的ERR MSG
                    Alert(tmp.Substring(3, tmp.Length - 3));
                }
                if (tmp.Substring(0, 2) == "OK")
                {
                    if (tmp.Substring(3, tmp.Length - 3) == "1")
                    {
                        Alert("填寫成功!DFX信息維護完成!");

                    }

                }
            }
            else
            {

                Alert("DB ERROR,Pls contact IT");
            }

        }
        catch (Exception ex)
        {
            Alert(ex.ToString());
        }
        pnlDFXInfo.Hide();
        BindDFXItem(DFXNo, "Dept.Write");


    }

    protected void btnDFXReply_Click(object sender, DirectEventArgs e)
    {
        StringBuilder ErrMsg = new StringBuilder();
        #region 控件為空驗證
        string item = Item.Text.Trim();
        string dfxdept = DFXDEPT.Text.Trim();
        string location = Location.Text.Trim();
        string requirement = Requirements1.Text.Trim();
        string Actions = txtActions.Text.Trim();
        string Remark = txtRemark.Text.Trim();
        string CompleteDate = txtCompleteDate.SelectedDate.ToString("yyyy/MM/dd");
        string DFXStatus = cmbDFXStatus.SelectedItem.Text;

        if (item.Length == 0)
        {
            ErrMsg.Append("請回覆項目!</br>");
        }
        if (dfxdept.Length == 0)
        {
            ErrMsg.Append("請回覆部門!</br>");
        }
        if (location.Length == 0)
        {
            ErrMsg.Append("請回覆Location!</br>");
        }
        if (requirement.Length == 0)
        {
            ErrMsg.Append("請回覆Requirements!</br>");
        }
        if (Actions.Length == 0)
        {
            ErrMsg.Append("請回覆改善措施!</br>");
        }
        if (CompleteDate == "0001/01/01")
        {
            ErrMsg.Append("請回覆目標完成日期</br>");
        }
        if (DFXStatus.Length == 0)
        {
            ErrMsg.Append("請回覆改善狀態</br>");
        }

        //StringBuilder sbTracking = new StringBuilder();
        //for (int i = 0; i < cbGroupTracking.Items.Count; i++)
        //{
        //    if (cbGroupTracking.Items[i].Checked == true)
        //    {
        //        sbTracking.AppendFormat("{0};", cbGroupTracking.Items[i].InputValue);
        //    }
        //}
        //if (sbTracking.ToString().Length == 0)
        //{
        //    ErrMsg.Append("請勾選Tracking</br>");
        //}

        if (ErrMsg.Length > 0)
        {
            Alert(ErrMsg.ToString());
            return;
        }
        #endregion
        else
        {
            Model_DFX_ITEMBODY oModel = new Model_DFX_ITEMBODY();
            oModel._DFXNo = hidDoc.Text.Trim();
            oModel._Item = hidItem.Text.Trim();
            oModel._Actions = Actions;
            oModel._CompletionDate = txtCompleteDate.SelectedDate.ToString("yyyy/MM/dd"); ;
            oModel._Remark = Remark;
            oModel._Tracking = cmbDFXStatus.SelectedItem.Text;
            oModel._Status = "Finished";
            NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
            NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
            try
            {

                string tmp = oStandard.RecordOperation_DFXItems(oModel, Status_Operation.UPDATE);
                if (tmp.Length >= 3)
                {
                    if (tmp.Substring(0, 2) == "NG")
                    {
                        //接收SP返回的ERR MSG
                        Alert(tmp.Substring(3, tmp.Length - 3));
                    }
                    if (tmp.Substring(0, 2) == "OK")
                    {

                    }
                }
                else
                {

                    Alert("DB ERROR,Pls contact IT");
                }

            }
            catch (Exception ex)
            {
                Alert(ex.ToString());
            }
        }
        pnlDFXReply.Hide();
        BindDFXItem(hidDoc.Text.Trim(), "Reply");


    }

    protected void txtPriorityLevel_Change(object sender, DirectEventArgs e)
    {

        string Compliance = cbCompliance.SelectedItem.Value;
        if (Compliance != "NA")
        {
            if (Compliance == "Y")
            {
                switch (txtPriority.Text.Trim())
                {
                    case "0":
                        txtMaxPoints.Text = "10";
                        break;
                    case "1":
                        txtMaxPoints.Text = "5";
                        break;
                    case "2":
                        txtMaxPoints.Text = "3";
                        break;
                    case "3":
                        txtMaxPoints.Text = "1";
                        break;
                    case "NA":
                        txtMaxPoints.Text = "FALSE";
                        break;
                }
                txtDFXPoints.Text = txtMaxPoints.Text;
            }
            else
            {
                switch (txtPriority.Text.Trim())
                {
                    case "1":
                        txtMaxPoints.Text = "5";
                        break;
                    case "2":
                        txtMaxPoints.Text = "3";
                        break;
                    case "3":
                        txtMaxPoints.Text = "1";
                        break;
                    case "NA":
                        txtMaxPoints.Text = "FALSE";
                        break;
                }
                txtDFXPoints.Text = "0";
            }
        }
        else
        {
            txtMaxPoints.Text = "NA";
            txtDFXPoints.Text = "0";
        }
    }

    protected void btnExport_click(object sender, DirectEventArgs e)
    {


        string json = e.ExtraParams["values"];

        //json = json.Replace("FormNo", "報修單號");

        StoreSubmitDataEventArgs eSubmit = new StoreSubmitDataEventArgs(json, null);
        XmlNode xml = eSubmit.Xml;
        XmlNodeList xm = xml.SelectSingleNode("records").ChildNodes;
        foreach (XmlNode xnl in xm)
        {
            XmlElement xe = (XmlElement)xnl;
            XmlNode xn = xe.SelectSingleNode("ID");
            xnl.RemoveChild(xn);

            xn = xe.SelectSingleNode("Class");
            xnl.RemoveChild(xn);

            xn = xe.SelectSingleNode("MaxPoints");
            xnl.RemoveChild(xn);

            xn = xe.SelectSingleNode("DFXPoints");
            xnl.RemoveChild(xn);

            xn = xe.SelectSingleNode("Status");
            xnl.RemoveChild(xn);

            xn = xe.SelectSingleNode("Actions");
            xnl.RemoveChild(xn);

            xn = xe.SelectSingleNode("FilePath");
            xnl.RemoveChild(xn);

            xn = xe.SelectSingleNode("CompletionDate");
            xnl.RemoveChild(xn);

            xn = xe.SelectSingleNode("Tracking");
            xnl.RemoveChild(xn);

            xn = xe.SelectSingleNode("Remark");
            xnl.RemoveChild(xn);

            xn = xe.SelectSingleNode("id");
            xnl.RemoveChild(xn);

        }

        this.Response.Clear();
        this.Response.ContentType = "application/vnd.ms-excel";
        this.Response.AddHeader("Content-Disposition", "attachment; filename=DFX_Write.xls");
        XslCompiledTransform xtExcel = new XslCompiledTransform();
        xtExcel.Load(Server.MapPath("Excel.xsl"));
        xtExcel.Transform(xml, null, this.Response.OutputStream);

        this.Response.End();


    }

    protected void btnUploadDFX_Click(object sender, DirectEventArgs e)
    {
        winUpload.Hidden = false;
    }

    protected void btnUploadData_Click(object sender, DirectEventArgs e)
    {
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        StringBuilder ErrMsg = new StringBuilder();
        string file = txtDFXField.FileName;

        int total_num = 0;
        int ok_num = 0;
        int ng_num = 0;
        string DFXNo = string.Empty;

        if (!txtDFXField.HasFile)
        {
            Alert("請選擇数据文件!");
            return;
        }
        else
        {
            string type = file.Substring(file.LastIndexOf(".") + 1).ToLower();
            if (type == "xlsx")
            {


                //把文件轉成流
                Stream stream = txtDFXField.PostedFile.InputStream;
                if (stream.Length == 8889)
                {
                    Alert("導入的資料表為空,請檢查!");
                    return;
                }
                try
                {

                    DataTable dt = ReadByExcelLibrary(stream);
                    if (dt.Rows.Count > 0)
                    {
                        Model_DFX_ITEMBODY oModel = new Model_DFX_ITEMBODY();
                        DFXNo = dt.Rows[0]["DFXNo"].ToString();
                        #region 數據檢查
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string Compliance = dt.Rows[i]["Compliance"].ToString();
                            string Location = dt.Rows[i]["Location"].ToString();
                            string Comments = dt.Rows[i]["Comments"].ToString();
                            string DFXNo1 = dt.Rows[i]["DFXNo"].ToString();
                            if (DFXNo1 != txtSub_No.Text)
                            {
                                ErrMsg.Append("單號必須一致！<br>");
                            }
                            if (Compliance.Length == 0)
                            {
                                ErrMsg.Append("第" + (i + 2) + "行");
                                ErrMsg.Append("Compliance不能为空!</br>");
                                ErrMsg.Append("<br/>");
                            }
                            else
                            {
                                if (!isCompliance(Compliance))
                                {
                                    ErrMsg.Append("第" + (i + 2) + "行");
                                    ErrMsg.Append("Compliance填寫錯誤!</br>");
                                }
                                if (Compliance == "N")
                                {
                                    if (string.IsNullOrEmpty(Location) || string.IsNullOrEmpty(Comments))
                                    {
                                        ErrMsg.Append("第" + (i + 2) + "行");
                                        ErrMsg.Append("儅Compliance為N時，Location或Comments不能為空!</br>");

                                    }
                                }
                            }

                        }
                        #endregion
                        if (string.IsNullOrEmpty(ErrMsg.ToString()))
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {

                                string Priority = dt.Rows[i]["PriorityLevel"].ToString();
                                string MaxPoints = string.Empty;
                                string DFXPoints = string.Empty;
                                string Compliance = dt.Rows[i]["Compliance"].ToString();
                                string Location = dt.Rows[i]["Location"].ToString();
                                string Comments = dt.Rows[i]["Comments"].ToString();
                                try
                                {
                                    #region CalcDFXPoint
                                    if (Compliance != "NA")
                                    {
                                        if (Compliance == "Y")
                                        {
                                            switch (Priority)
                                            {
                                                case "1":
                                                    MaxPoints = "5";
                                                    break;
                                                case "2":
                                                    MaxPoints = "3";
                                                    break;
                                                case "3":
                                                    MaxPoints = "1";
                                                    break;
                                                case "NA":
                                                    MaxPoints = "FALSE";
                                                    break;
                                            }
                                            DFXPoints = MaxPoints;
                                        }
                                        else
                                        {
                                            switch (Priority)
                                            {
                                                case "1":
                                                    MaxPoints = "5";
                                                    break;
                                                case "2":
                                                    MaxPoints = "3";
                                                    break;
                                                case "3":
                                                    MaxPoints = "1";
                                                    break;
                                                case "NA":
                                                    MaxPoints = "FALSE";
                                                    break;
                                            }
                                            DFXPoints = "0";
                                        }
                                    }
                                    else
                                    {
                                        MaxPoints = "NA";
                                        DFXPoints = "0";
                                    }
                                    #endregion
                                    if (Compliance == "N")
                                    {
                                        oModel._Status = "Reply";
                                    }
                                    else
                                    {
                                        oModel._Status = "Finished";
                                    }

                                    oModel._DFXNo = dt.Rows[i]["DFXNo"].ToString();
                                    oModel._Item = dt.Rows[i]["Item"].ToString();
                                    oModel._MaxPoints = MaxPoints;
                                    oModel._DFXPoints = DFXPoints;
                                    oModel._Compliance = Compliance;
                                    oModel._UpdateUser = lblLogonId.Text.Trim();
                                    oModel._Location = dt.Rows[i]["Location"].ToString().Trim();
                                    oModel._Comments = dt.Rows[i]["Comments"].ToString().Trim();
                                    oModel._Losses = dt.Rows[i]["Losses"].ToString();
                                    string tmp = oStandard.RecordOperation_DFXItems(oModel, Status_Operation.ADD);
                                    if (tmp.Length >= 3)
                                    {
                                        if (tmp.Substring(0, 2) == "NG")
                                        {
                                            //接收SP返回的ERR MSG
                                            Alert(tmp.Substring(3, tmp.Length - 3));
                                        }
                                        if (tmp.Substring(0, 2) == "OK")
                                        {
                                            if (tmp.Substring(3, tmp.Length - 3) == "1")
                                            {
                                                Alert("填寫成功!DFX信息維護完成!");
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Alert(ex.ToString());
                                }
                            }


                        }
                        else
                        {
                            Alert(ErrMsg.ToString());
                        }

                    }
                }
                catch (Exception ex)
                {
                    Alert("Excel數據有問題,錯誤信息: " + ex.Message);
                    return;
                }

            }
            else
            {
                Alert("文件類型只能為xlsx");
            }
        }

        txtDFXField.Reset();
        winUpload.Hidden = true;
        BindDFXItem(DFXNo, "Dept.Write");


    }
    #endregion

    #region [CTQList Write]

    protected void grdCTQInfo_RowCommand(object sender, DirectEventArgs e)
    {

        string command = e.ExtraParams["command"];
        switch (command)
        {
            case "Write":
                RefreshCLCAInfo();
                hidId.Text = e.ExtraParams["ID"];

                txtControlType.Text = e.ExtraParams["CONTROL_TYPE"];
                if (txtControlType.Text == "Yield%")
                {
                    txtAct.FieldLabel = "ACT(%):";
                }
                else
                {
                    txtAct.FieldLabel = "ACT:";
                }
                txtGoal.Text = e.ExtraParams["GOALStr"];
                hidGoal.Text = e.ExtraParams["GOAL"];
                txtAct.Text = e.ExtraParams["ACT"];
                txtCTQ.Text = e.ExtraParams["CTQ"];
                txtflag.Text = e.ExtraParams["flag"];
                txtDescription.Text = e.ExtraParams["DESCRIPTION"];
                txtRootCause.Text = e.ExtraParams["ROOT_CAUSE"];
                txtResult.Text = e.ExtraParams["RESULT"];
                txtdept_CTQ.Text = e.ExtraParams["DEPT"];
                if (txtResult.Text == "Fail")
                {
                    txtDescription.Show();
                    txtRootCause.Show();
                    ckgCauseType.Show();
                }
                pnlCTQInfo.Show();
                break;
            case "Reply":
                RefreshCLCAReplyInfo();
                hidDoc.Text = e.ExtraParams["DFXNo"];
                hidItem.Text = e.ExtraParams["Item"];
                hidId.Text = e.ExtraParams["ID"];
                break;
        }
    }

    protected void btnConfirm_Click(object sender, DirectEventArgs e)
    {
        SpmMaster _Master = (SpmMaster)Master;
        string ACT = txtAct.Text;
        string CTQ = txtCTQ.Text;
        string Model = txtModel_R.Text.Trim();
        string Stage = sbPhase.SelectedItem.Text;
        string dept = txtdept_CTQ.Text.Trim();
        decimal ACT_DD = 0;
        string result = JudgeResults();
        Model_NPI_APP_CTQ oModel = new Model_NPI_APP_CTQ();
        oModel._ID = int.Parse(hidId.Text.ToString());
        oModel._DOC_NO = txtDOC_NO.Text;
        oModel._SUB_DOC_NO = txtSub_No.Text;
        oModel._ACT = ACT;
        oModel._RESULT = result;
        StringBuilder sbErrMsg = new StringBuilder();
        if (result == "Fail")
        {
            string Desription = txtDescription.Text.Trim();
            string RootCause = txtRootCause.Text.Trim();
            if (Desription.Length == 0)
            {
                sbErrMsg.Append("問題描述不能為空</br>");

            }
            if (RootCause.Length == 0)
            {
                sbErrMsg.Append("根本原因不能為空</br>");

            }
            if (chkDesign.Checked == false && chkM.Checked == false && chKProcess.Checked == false && chkE.Checked == false &&
                chkW.Checked == false && chkO.Checked == false)
            {
                sbErrMsg.Append("請選擇原因種類</br>");
            }
            if (sbErrMsg.ToString().Length > 0)
            {
                Alert(sbErrMsg.ToString());
                return;
            }
            oModel._DESCRIPTION = txtDescription.Text;
            oModel._ROOT_CAUSE = txtRootCause.Text;
            oModel._M = chkM.Checked ? "V" : string.Empty;
            oModel._P = chKProcess.Checked ? "V" : string.Empty;
            oModel._O = chkO.Checked ? "V" : string.Empty;
            oModel._W = chkW.Checked ? "V" : string.Empty;
            oModel._D = chkDesign.Checked ? "V" : string.Empty;
            oModel._E = chkE.Checked ? "V" : string.Empty;


        }
        if (txtflag.Text == "Y" && !CTQWriteFUpload.HasFile)
        {
            Alert("請上傳附件</br>");
            return;
        }
        string MFILE_NAME = string.Empty;
        string MFILE_PATH = string.Empty;
        if (CTQWriteFUpload.PostedFile.FileName.ToString().Length > 0)
        {
            int indexAttachment = CTQWriteFUpload.FileName.LastIndexOf('.');
            string extAttachment = CTQWriteFUpload.FileName.Substring(indexAttachment + 1);
            MFILE_NAME = CTQWriteFUpload.FileName.Substring(CTQWriteFUpload.FileName.LastIndexOf("\\") + 1);
            FileInfo FlInf = new FileInfo(CTQWriteFUpload.PostedFile.FileName);
            string File_Ext = FlInf.Extension.ToLower();
            String FileTypes = System.Configuration.ConfigurationSettings.AppSettings["UploadFilesTypes"];
            String[] FileType = FileTypes.Split('|');
            bool FileType_Flag = false;
            foreach (String aType in FileType)
            {
                if (File_Ext.Equals("." + aType))
                {
                    FileType_Flag = true;
                    break;
                }
            }
            if (extAttachment != "xlsx")
            {
                Alert("附件類型只能為xlsx類型!");
                return;
            }
            //else
            //{
            //    if (!FileType_Flag)
            //    {
            //        Alert("False只能上傳后綴名為 " + FileTypes + "的文件 .");
            //        return;
            //    }
            //}



            //部门是EE或者TE时,保存到Test CPK文件,其他部门时,保存到MFG CTQ文件夹
            if (dept == "EE" || dept == "TE")
            {
                string type = "Test CPK";
                string CaseID = _Master.IFormURLPara.CaseId.ToString();
                string docNoPath = Server.MapPath("~/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + type + " Doc" + "/" + dept + " Doc");
                string filepath = (docNoPath + "/" + MFILE_NAME);
                bool IsDocNoExist = Directory.Exists(docNoPath);
                MFILE_PATH = "Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + type + " Doc" + "/" + dept + " Doc" + "/" + MFILE_NAME;
                if (!IsDocNoExist)
                {
                    Directory.CreateDirectory(docNoPath);
                }

                try
                {

                    this.CTQWriteFUpload.PostedFile.SaveAs(docNoPath + "/" + MFILE_NAME);

                }
                catch (Exception ee)
                {
                    Alert("False保存文件失败：" + ee.Message + " , " + filepath);
                    return;
                }
            }
            else
            {
                string type = "MFG CTQ";
                string CaseID = _Master.IFormURLPara.CaseId.ToString();
                string docNoPath = Server.MapPath("~/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + type + " Doc" + "/" + dept + " Doc");
                string filepath = (docNoPath + "/" + MFILE_NAME);
                bool IsDocNoExist = Directory.Exists(docNoPath);
                MFILE_PATH = "Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + type + " Doc" + "/" + dept + " Doc" + "/" + MFILE_NAME;
                if (!IsDocNoExist)
                {
                    Directory.CreateDirectory(docNoPath);
                }

                try
                {

                    this.CTQWriteFUpload.PostedFile.SaveAs(docNoPath + "/" + MFILE_NAME);

                }
                catch (Exception ee)
                {
                    Alert("False保存文件失败：" + ee.Message + " , " + filepath);
                    return;
                }
            }

        }
        oModel._STATUS = result == "Fail" ? "Reply" : "Finished";
        oModel._W_FILENAME = MFILE_NAME;
        oModel._W_FILEPATH = MFILE_PATH;
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        string Dept = oStandard.GetUserDept("A", lblLogonId.Text, txtPROD_GROUP.Text);
        try
        {

            string tmp = oStandard.RecordOperation_CTQInfo(oModel, Status_Operation.ADD);
            if (tmp.Length >= 3)
            {
                if (tmp.Substring(0, 2) == "NG")
                {
                    //接收SP返回的ERR MSG
                    Alert(tmp.Substring(3, tmp.Length - 3));
                }
                if (tmp.Substring(0, 2) == "OK")
                {


                    BindCTQInfo(txtSub_No.Text.Trim(), "Dept.Write");

                }
            }
            else
            {

                Alert("DB ERROR,Pls contact IT");
            }

        }
        catch (Exception ex)
        {
            Alert("DB ERROR:" + ex.Message);
        }
        RefreshCLCAInfo();

    }

    protected void RefreshCLCAReplyInfo()
    {
        pnlCLCAReply.Hidden = false;
        txtCLCAActions.Text = string.Empty;
        txtCLCAPreActions.Text = string.Empty;
        txtCLCACompleteDate.Text = string.Empty;
        cmbCLCAImproveStatus.Text = string.Empty;
    }

    protected void txtAct_Changed(object sender, DirectEventArgs e)
    {
        string CTQ = txtCTQ.Text;
        txtResult.Text = JudgeResults();

        if (txtResult.Text == "Fail")
        {

            txtDescription.Show();
            txtRootCause.Show();
            ckgCauseType.Show();
        }
        else
        {
            //ContrainerClca.Hide();
            txtDescription.Hide();
            txtRootCause.Hide();
            ckgCauseType.Hide();
        }

    }

    protected string JudgeResults()
    {
        string result = string.Empty;
        string ACT = txtAct.Text;
        string CTQ = txtCTQ.Text;
        decimal ACT_DD = 0;
        if (string.IsNullOrEmpty(ACT))
        {
            Alert("請輸入ACT欄位的值!");
        }
        if (ACT != "NA" && !decimal.TryParse(ACT, out ACT_DD))
        {
            Alert("ACT欄位的值輸入不合法,必須為數字或NA!");
        }
        if (ACT.ToUpper() == "NA")
        {
            result = "NA";
        }
        else
        {
            if ((txtControlType.Text == "Yield%" || txtControlType.Text == "PCS") && txtGoal.Text != "NA")
            {
                double ACT_D = Convert.ToDouble(ACT) * 0.01;
                double Goal_D = Convert.ToDouble(hidGoal.Text);
                if (ACT_D > 1 || ACT_D <= 0)
                {
                    Alert("ACT值必須在0-100之間,請重新錄入!");

                }
                if (ACT_D >= Goal_D)
                {
                    if (CTQ == "NDF")
                    {
                        result = "Fail";
                    }
                    else
                    {
                        result = "Pass";
                    }
                }
                else
                {
                    if (CTQ == "NDF")
                    {
                        result = "Pass";
                    }
                    else
                    {
                        result = "Fail";
                    }
                }
            }

            if ((txtControlType.Text == "Yield%" || txtControlType.Text == "PCS") && txtGoal.Text == "NA")
            {

                result = "NA";
            }
            else if (txtControlType.Text.Trim() == "CPKs")
            {
                decimal ACT_C = Convert.ToDecimal(ACT);
                decimal Goal_C = Convert.ToDecimal(hidGoal.Text);
                if (ACT_C >= Goal_C)
                {
                    if (CTQ == "NDF")
                    {
                        result = "Fail";
                    }
                    else
                    {
                        result = "Pass";
                    }
                }
                else
                {
                    if (CTQ == "NDF")
                    {
                        result = "Pass";
                    }
                    else
                    {
                        result = "Fail";
                    }
                }
            }
            else if (txtControlType.Text.Trim() == "DPMO")
            {
                decimal ACT_C = Convert.ToDecimal(ACT);
                decimal Goal_C = Convert.ToDecimal(hidGoal.Text);
                if (ACT_C <= Goal_C)
                {
                    if (CTQ == "NDF")
                    {
                        result = "Fail";
                    }
                    else
                    {
                        result = "Pass";
                    }
                }
                else
                {
                    if (CTQ == "NDF")
                    {
                        result = "Pass";
                    }
                    else
                    {
                        result = "Fail";
                    }
                }
            }
        }
        return result;
    }

    protected void RefreshCLCAInfo()
    {
        txtAct.Text = string.Empty;
        txtResult.Text = string.Empty;

        txtDescription.Hide();
        txtRootCause.Hide();
        CTQWriteFUpload.Text = string.Empty;
        //CTQWriteFUpload.PostedFile.FileName = string.Empty;
        txtDescription.Text = string.Empty;
        txtRootCause.Text = string.Empty;
        CTQWriteFUpload.Reset();
        ckgCauseType.Hide();
        for (int i = 0; i < ckgCauseType.Items.Count; i++)
        {

            ckgCauseType.Items[i].Checked = false;

        }

    }

    private void BindCTQInfo(string subDocNo, string Type)
    {
        SpmMaster _Master = (SpmMaster)Master;
        string StepName = _Master.IFormURLPara.StepName.ToString();
        StringBuilder sb = new StringBuilder();
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        if (StepName == "Dept.Reply" || StepName == "Dept.Write")
        {
            string Dept = GetUserDept("AC", lblLogonId.Text, txtPROD_GROUP.Text, StepName);
            DataTable dt = oStandard.GetCLCAInconformity(subDocNo, Dept, Type);
            grdCTQInfo.Store.Primary.DataSource = dt;
            grdCTQInfo.Store.Primary.DataBind();
        }
        else
        {
            string Dept = oStandard.GetUserDept("A", lblLogonId.Text, txtPROD_GROUP.Text);
            DataTable dt = oStandard.GetCLCAInconformity(subDocNo, Dept, Type);
            grdCTQInfo.Store.Primary.DataSource = dt;
            grdCTQInfo.Store.Primary.DataBind();
        }
    }

    protected void btnCLCAReply_Click(object sender, DirectEventArgs e)
    {
        StringBuilder ErrMsg = new StringBuilder();
        string Actions = txtCLCAActions.Text.Trim();
        string CorrectiveAction = txtCLCAPreActions.Text.Trim();
        string CompleteDate = txtCLCACompleteDate.Text.Trim();
        string ImproveStatus = cmbCLCAImproveStatus.SelectedItem.Text.Trim();
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();

        #region 控件為空驗證
        if (Actions.Length == 0)
        {
            ErrMsg.Append("請回覆臨時對策!</br>");
        }
        if (CorrectiveAction.Length == 0)
        {
            ErrMsg.Append("請回覆矯正預防措施</br>");
        }
        if (CompleteDate.Length == 0)
        {
            ErrMsg.Append("請回覆完成時間</br>");
        }
        if (ImproveStatus.Length == 0)
        {
            ErrMsg.Append("請回覆改善狀況</br>");
        }
        if (ErrMsg.Length > 0)
        {
            Alert(ErrMsg.ToString());
            return;
        }
        #endregion
        else
        {
            Model_NPI_APP_CTQ oModel = new Model_NPI_APP_CTQ();
            oModel._TEMPORARY_ACTION = Actions;
            oModel._CORRECTIVE_PREVENTIVE_ACTION = CorrectiveAction;
            oModel._COMPLETE_DATE = CompleteDate;
            oModel._IMPROVEMENT_STATUS = ImproveStatus;
            oModel._STATUS = "Finished";
            oModel._ID = int.Parse(hidId.Text.Trim());

            try
            {

                string tmp = oStandard.RecordOperation_CTQInfo(oModel, Status_Operation.UPDATE);
                if (tmp.Length >= 3)
                {
                    if (tmp.Substring(0, 2) == "NG")
                    {
                        //接收SP返回的ERR MSG
                        Alert(tmp.Substring(3, tmp.Length - 3));
                    }
                    if (tmp.Substring(0, 2) == "OK")
                    {

                    }
                }
                else
                {

                    Alert("DB ERROR,Pls contact IT");
                }

            }
            catch (Exception ex)
            {
                Alert(ex.ToString());
            }
        }
        pnlCLCAReply.Hide();
        string Dept = oStandard.GetUserDept("A", lblLogonId.Text, txtPROD_GROUP.Text);
        BindCTQInfo(txtSub_No.Text, "Reply");

    }


    //protected void cmbSuit_Change(object sender, DirectEventArgs e)
    //{
    //    if (cmbSuit.SelectedItem.Text == "NA")
    //    {
    //        txtAct.Disabled = true;
    //        txtResult.Disabled = true;
    //        CTQWriteFUpload.Disabled = true;
    //        txtAct.Text = string.Empty;
    //        txtResult.Text = string.Empty;
    //        CTQWriteFUpload.Text = string.Empty;
    //    }
    //    else
    //    {
    //        txtAct.Disabled = false;
    //        txtResult.Disabled = false;
    //        CTQWriteFUpload.Disabled = false;
    //    }
    //}


    #endregion

    #region[IssuesList Write]

    protected void btnAdd_Click(object sender, DirectEventArgs e)
    {
        string Model = txtModel_R.Text.Trim();
        string Stage = sbPhase.SelectedItem.Text;
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        string Item = txtCustomer.Text.Trim();
        string Class = string.Empty;
        string Descripiton = txtDescription_Issues.Text.Trim();
        string TempMeasure = txtTempMeasure.Text.Trim();
        string ImporveMeasure = txtImporveMeasure.Text.Trim();
        string LosseItem = cbLosseItem.SelectedItem.Text.Trim();
        string IssueClass = cbClass.SelectedItem.Text.Trim();
        string Station = txtProjectStation.Text.Trim();
        StringBuilder ErrMsg = new StringBuilder();
        SpmMaster _Master = (SpmMaster)Master;

        #region 控件值驗證是否為空

        if (string.IsNullOrEmpty(Station))
        {
            ErrMsg.Append("站别不能為空</br>");
        }
        if (string.IsNullOrEmpty(cmbIssuesDept.SelectedItem.Text))
        {
            ErrMsg.Append("部門不能為空</br>");
        }

        if (string.IsNullOrEmpty(Descripiton))
        {
            ErrMsg.Append("問題描述不能為空</br>");
        }
        if (string.IsNullOrEmpty(ImporveMeasure))
        {
            ErrMsg.Append("建議改善對策不能為空</br>");
        }
        if (string.IsNullOrEmpty(IssueClass))
        {
            ErrMsg.Append("問題等級不能為空</br>");
        }

        if (string.IsNullOrEmpty(LosseItem))
        {
            ErrMsg.Append("違反損失不能為空</br>");
        }
        if (string.IsNullOrEmpty(TempMeasure))
        {
            ErrMsg.Append("臨時對策不能為空</br>");
        }

        if (ErrMsg.ToString().Length > 0)
        {
            Alert(ErrMsg.ToString());
            return;
        }
        #endregion
        string MFILE_NAME = string.Empty;
        string MFILE_PATH = string.Empty;
        if (IssuesUploadFile.PostedFile.FileName.ToString().Length > 0)
        {
            int indexAttachment = IssuesUploadFile.FileName.LastIndexOf('.');
            string extAttachment = IssuesUploadFile.FileName.Substring(indexAttachment + 1);
            MFILE_NAME = IssuesUploadFile.FileName.Substring(IssuesUploadFile.FileName.LastIndexOf("\\") + 1);
            FileInfo FlInf = new FileInfo(IssuesUploadFile.PostedFile.FileName);
            string File_Ext = FlInf.Extension.ToLower();
            String FileTypes = System.Configuration.ConfigurationSettings.AppSettings["UploadFilesTypes"];
            String[] FileType = FileTypes.Split('|');
            bool FileType_Flag = false;
            foreach (String aType in FileType)
            {
                if (File_Ext.Equals("." + aType))
                {
                    FileType_Flag = true;
                    break;
                }
            }
            if (!FileType_Flag)
            {
                Alert("False只能上傳后綴名為 " + FileTypes + "的文件 .");
                return;
            }
            string type = "IssuesList";
            string CaseID = _Master.IFormURLPara.CaseId.ToString();
            string docNoPath = Server.MapPath("~/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + type + " Doc");
            string filepath = (docNoPath + "/" + MFILE_NAME);
            bool IsDocNoExist = Directory.Exists(docNoPath);
            MFILE_PATH = "Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + type + " Doc" + "/" + MFILE_NAME;
            if (!IsDocNoExist)
            {
                Directory.CreateDirectory(docNoPath);
            }

            try
            {

                IssuesUploadFile.PostedFile.SaveAs(docNoPath + "/" + MFILE_NAME);

            }
            catch (Exception ee)
            {
                Alert("False保存文件失败：" + ee.Message + " , " + filepath);
                return;
            }

        }

        Model_NPI_APP_ISSUELIST oModel_IssueList = new Model_NPI_APP_ISSUELIST();
        oModel_IssueList._ITEMS = Item;
        oModel_IssueList._ISSUE_DESCRIPTION = Descripiton;
        oModel_IssueList._CLASS = IssueClass;
        oModel_IssueList._ISSUE_LOSSES = LosseItem;
        oModel_IssueList._TEMP_MEASURE = TempMeasure;
        oModel_IssueList._CREATE_TIME = DateTime.Now;
        oModel_IssueList._CREATE_USERID = lblLogonId.Text.Trim();
        oModel_IssueList._DOC_NO = txtDOC_NO.Text;
        oModel_IssueList._SUB_DOC_NO = txtSub_No.Text;
        oModel_IssueList._PRIORITYLEVEL = Class;
        oModel_IssueList._IMPROVE_MEASURE = txtImporveMeasure.Text.Trim();
        oModel_IssueList._UPDATE_TIME = DateTime.Now;
        oModel_IssueList._UPDATE_USERID = lblLogonId.Text.Trim();
        oModel_IssueList._STATION = Station;
        oModel_IssueList._PHASE = sbPhase.Text.Trim();

        oModel_IssueList._DEPT = cmbIssuesDept.SelectedItem.Text.Trim();
        oModel_IssueList._STAUTS = "Write";
        oModel_IssueList._FILENAME = MFILE_NAME;
        oModel_IssueList._FILEPATH = MFILE_PATH;

        try
        {
            Dictionary<string, object> result = oStandard.RecordOperation_APPIssueList(oModel_IssueList, Status_Operation.ADD);
            if ((bool)result["Result"])
            {
                Alert("新增成功!");
                RefreshIssuesInfo();
                BindIssueList();
            }
            else
            {
                Alert((string)result["ErrMsg"].ToString());
            }
        }
        catch (Exception ex)
        {
            Alert(ex.ToString());

        }
    }

    protected void btnDelete_Click(object sender, DirectEventArgs e)
    {

        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        RowSelectionModel sm = this.grdIssuesList.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count <= 0)
        {
            Alert("请选择须删除项!");
            return;
        }
        string json = e.ExtraParams["Values"];
        Dictionary<string, string>[] companies = JSON.Deserialize<Dictionary<string, string>[]>(json);
        string id = string.Empty;
        string errMsg = string.Empty;
        Model_NPI_APP_ISSUELIST oModel_IssueList = new Model_NPI_APP_ISSUELIST();

        foreach (Dictionary<string, string> row in companies)
        {
            id = row["ID"];
            oModel_IssueList._ID = id;
            try
            {
                Dictionary<string, object> result = oStandard.RecordOperation_APPIssueList(oModel_IssueList, Status_Operation.DELETE);
                if ((bool)result["Result"])
                {
                    Alert("删除成功!");
                    RefreshIssuesInfo();
                    BindIssueList();
                }
                else
                {
                    Alert((string)result["ErrMsg"].ToString());
                }
            }

            catch (Exception ex)
            {
                Alert(ex.ToString());
            }
        }

    }

    protected void BindIssueList()
    {
        SpmMaster _Master = (SpmMaster)Master;
        string StepName = _Master.IFormURLPara.StepName.ToString();
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        StringBuilder sb = new StringBuilder();
        ArrayList opc = new ArrayList();
        if (StepName == "Dept.Reply" || StepName == "Dept.Write")
        {
            string Dept = GetUserDept("AI", lblLogonId.Text, txtPROD_GROUP.Text, StepName);
            DataTable dt = oStandard.GetIssuesInconformity(txtSub_No.Text.Trim(), Dept, string.Empty);
            BindGrid(grdIssuesList, dt);
        }
        else
        {
            string Dept = oStandard.GetUserDept("A", lblLogonId.Text, txtPROD_GROUP.Text);
            DataTable dt = oStandard.GetIssuesInconformity(txtSub_No.Text.Trim(), Dept, string.Empty);
            BindGrid(grdIssuesList, dt);
        }


    }

    protected void grdIssuesInfo_RowCommand(object sender, DirectEventArgs e)
    {
        string command = e.ExtraParams["command"];
        switch (command)
        {

            case "Reply":
                RefreshIssuesReply();
                hidIssuesID.Text = e.ExtraParams["ID"];
                Session["IssuesId"] = e.ExtraParams["ID"];
                ProjectStation.Text = e.ExtraParams["STATION"];
                Dept.Text = e.ExtraParams["DEPT"];
                txtDutyDeptReply.Text = e.ExtraParams["MEASURE_DEPTREPLY"];
                txtIssuesCurrentStatus.Text = e.ExtraParams["CURRENT_STATUS"];
                cmIssuesStatus.SelectedItem.Text = e.ExtraParams["TRACKING"];
                txtIssuesDueDay.Text = e.ExtraParams["DUE_DAY"];
                txtIssuesRemark.Text = e.ExtraParams["REMARK"];
                ISSUE_DESCRIPTION.Text = e.ExtraParams["ISSUE_DESCRIPTION"];
                IMPROVEMEASURE.Text = e.ExtraParams["IMPROVE_MEASURE"];
                break;
        }
    }

    protected void btnIssuesReply_Click(object sender, DirectEventArgs e)
    {
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        string projectStation = ProjectStation.Text.Trim();
        string dept = Dept.Text.Trim();
        string description = ISSUE_DESCRIPTION.Text.Trim();
        string improvemeasure = IMPROVEMEASURE.Text.Trim();

        string DeptReply = txtDutyDeptReply.Text.Trim();
        string IssuesRemark = txtIssuesRemark.Text.Trim();
        string CurrentStatus = txtIssuesCurrentStatus.Text.Trim();
        string IssuesStatus = cmIssuesStatus.SelectedItem.Text.Trim();
        string DueDay = txtIssuesDueDay.SelectedDate.ToString("yyyy-MM-dd");
        StringBuilder ErrMsg = new StringBuilder();
        #region 控件值驗證是否為空
        if (string.IsNullOrEmpty(projectStation))
        {
            ErrMsg.Append("站別不能為空</br>");
        }
        if (string.IsNullOrEmpty(dept))
        {
            ErrMsg.Append("部門不能為空</br>");
        }
        if (string.IsNullOrEmpty(description))
        {
            ErrMsg.Append("問題表述不能為空</br>");
        }
        if (string.IsNullOrEmpty(improvemeasure))
        {
            ErrMsg.Append("建議改善對策不能為空</br>");
        }

        if (string.IsNullOrEmpty(DeptReply))
        {
            ErrMsg.Append("責任部門對策不能為空</br>");
        }

        if (string.IsNullOrEmpty(CurrentStatus))
        {
            ErrMsg.Append("结果确认不能為空</br>");
        }
        if (string.IsNullOrEmpty(IssuesStatus))
        {
            ErrMsg.Append("改善狀態不能為空</br>");
        }
        if (string.IsNullOrEmpty(DueDay))
        {
            ErrMsg.Append("Due Day不能為空</br>");
        }

        if (ErrMsg.ToString().Length > 0)
        {
            Alert(ErrMsg.ToString());
            return;
        }
        #endregion


        Model_NPI_APP_ISSUELIST oModel_IssueList = new Model_NPI_APP_ISSUELIST();
        //oModel_IssueList._PERSON_IN_CHARGE = IssuesCharge;
        oModel_IssueList._CURRENT_STATUS = CurrentStatus;
        oModel_IssueList._REMARK = IssuesRemark;
        oModel_IssueList._DEPT = GetUserDept("ISSUES TeamMember");
        oModel_IssueList._STAUTS = "Finished";
        oModel_IssueList._TRACKING = IssuesStatus;
        oModel_IssueList._ID = Session["IssuesId"].ToString().Trim();
        oModel_IssueList._DUE_DAY = DueDay;
        oModel_IssueList._MEASURE_DEPTREPLY = DeptReply;

        try
        {
            Dictionary<string, object> result = oStandard.RecordOperation_APPIssueList(oModel_IssueList, Status_Operation.UPDATE);
            if ((bool)result["Result"])
            {
                BindIssueList();
                pnlIssuesReply.Hide();
                btnIssuesExport.Hide();
                btnUploadIssues.Hide();
            }
            else
            {
                Alert((string)result["ErrMsg"].ToString());
            }
        }
        catch (Exception ex)
        {
            Alert(ex.ToString());

        }

    }

    protected void RefreshIssuesReply()
    {

        txtIssuesRemark.Text = string.Empty;
        txtIssuesCurrentStatus.Text = string.Empty;
        cmIssuesStatus.Text = string.Empty;
        ProjectStation.Text = string.Empty;
        Dept.Text = string.Empty;
        pnlIssuesReply.Hidden = false;
        btnIssuesExport.Hidden = false;
        btnUploadIssues.Hidden = false;

    }

    protected void RefreshIssuesInfo()
    {
        txtProjectStation.Text = string.Empty;
        cbClass.Text = string.Empty;
        txtDescription_Issues.Text = string.Empty;
        cbLosseItem.Text = string.Empty;
        txtTempMeasure.Text = string.Empty;
        txtImporveMeasure.Text = string.Empty;
        IssuesUploadFile.Text = string.Empty;
        IssuesUploadFile.Reset();
    }

    protected void rgIssueBatch_Changed(object sender, DirectEventArgs e)
    {
        if (rdBatchIssueY.Checked)
        {
            ContainerBatch.Hidden = false;
            pnlIssueInfo.Hidden = true;
        }
        else if (rdPBatchIssueN.Checked)
        {
            ContainerBatch.Hidden = true;
            pnlIssueInfo.Hidden = false;
        }
        else
        {
            ContainerBatch.Hidden = true;
            pnlIssueInfo.Hidden = true;
        }

    }

    protected void btnBatchUpload_Click(object sender, DirectEventArgs e)//write Upload
    {
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        StringBuilder ErrMsg = new StringBuilder();
        string file = IssueFileUpload.FileName;
        int total_num = 0;
        int ok_num = 0;
        int ng_num = 0;
        bool isOk = true;

        if (!IssueFileUpload.HasFile)
        {
            Alert("請選擇数据文件!");
            return;
        }
        else
        {
            string type = file.Substring(file.LastIndexOf(".") + 1).ToLower();
            if (type == "xlsx")
            {


                //把文件轉成流
                Stream stream = IssueFileUpload.PostedFile.InputStream;
                if (stream.Length == 8889)
                {
                    Alert("導入的資料表為空,請檢查!");
                    return;
                }
                try
                {

                    DataTable dt = ReadByExcelLibrary_IssueandFMEA(stream);
                    if (dt.Rows.Count > 0)
                    {
                        Model_NPI_APP_ISSUELIST oModel_IssueList = new Model_NPI_APP_ISSUELIST();

                        oModel_IssueList._CREATE_TIME = DateTime.Now;
                        oModel_IssueList._ITEMS = txtCustomer.Text.Trim();
                        oModel_IssueList._CREATE_USERID = lblLogonId.Text.Trim();
                        oModel_IssueList._DOC_NO = txtDOC_NO.Text;
                        oModel_IssueList._SUB_DOC_NO = txtSub_No.Text;
                        oModel_IssueList._UPDATE_TIME = DateTime.Now;
                        oModel_IssueList._UPDATE_USERID = lblLogonId.Text.Trim();
                        oModel_IssueList._PHASE = sbPhase.Text.Trim();
                        string Dept = oStandard.GetUserDept("A", lblLogonId.Text, txtPROD_GROUP.Text);

                        oModel_IssueList._STAUTS = "Write";
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            oModel_IssueList._ISSUE_DESCRIPTION = dt.Rows[i]["Issue Description"].ToString();
                            oModel_IssueList._CLASS = dt.Rows[i]["Issue Severity"].ToString();
                            oModel_IssueList._ISSUE_LOSSES = Helper.ConvertChinese(dt.Rows[i]["Loss Item"].ToString(), "Big5");
                            oModel_IssueList._TEMP_MEASURE = dt.Rows[i]["Temp Action"].ToString();
                            oModel_IssueList._STATION = dt.Rows[i]["Station"].ToString();
                            oModel_IssueList._IMPROVE_MEASURE = dt.Rows[i]["Suggestion Action"].ToString();
                            oModel_IssueList._REMARK = dt.Rows[i]["REMARK"].ToString();
                            oModel_IssueList._DEPT = dt.Rows[i]["DEPT"].ToString();


                            #region  數據驗證
                            if (string.IsNullOrEmpty(oModel_IssueList._ISSUE_DESCRIPTION))
                            {
                                isOk = false;
                                ErrMsg.Append("第" + (i + 2) + "行Description不能為空</br>");
                            }
                            if (string.IsNullOrEmpty(oModel_IssueList._CLASS))
                            {
                                isOk = false;
                                ErrMsg.Append("第" + (i + 2) + "行Issue Severity不能為空</br>");
                            }
                            if (string.IsNullOrEmpty(oModel_IssueList._ISSUE_LOSSES))
                            {
                                isOk = false;
                                ErrMsg.Append("第" + (i + 2) + "行Loss Item不能為空</br>");
                            }
                            if (string.IsNullOrEmpty(oModel_IssueList._TEMP_MEASURE))
                            {
                                isOk = false;
                                ErrMsg.Append("第" + (i + 2) + "行Temp Action不能為空</br>");
                            }
                            if (string.IsNullOrEmpty(oModel_IssueList._STATION))
                            {
                                isOk = false;
                                ErrMsg.Append("第" + (i + 2) + "行Station不能為空</br>");
                            }
                            if (string.IsNullOrEmpty(oModel_IssueList._IMPROVE_MEASURE))
                            {
                                isOk = false;
                                ErrMsg.Append("第" + (i + 2) + "行Suggestion Action不能為空</br>");
                            }
                            if (!Dept.Contains(dt.Rows[i]["DEPT"].ToString()))
                            {
                                isOk = false;
                                ErrMsg.Append("第" + (i + 2) + "行DEPT不符合</br>");
                            }
                            if (!isIssueSeverity(dt.Rows[i]["Issue Severity"].ToString()))
                            {
                                isOk = false;
                                ErrMsg.Append("第" + (i + 2) + "行Issue Severity不符合</br>");
                            }
                            if (!isLossItem(Helper.ConvertChinese(dt.Rows[i]["Loss Item"].ToString(), "Big5")))
                            {
                                isOk = false;
                                ErrMsg.Append("第" + (i + 2) + "行Loss Item不符合</br>");
                            }
                            if (!Dept.Contains(dt.Rows[i]["DEPT"].ToString()))
                            {
                                isOk = false;
                                ErrMsg.Append("第" + (i + 2) + "行DEPT不符合</br>");
                            }
                            #endregion

                            #region 數據處理
                            total_num = dt.Rows.Count;
                            try
                            {



                                if (isOk)
                                {
                                    Dictionary<string, object> result = oStandard.RecordOperation_APPIssueList(oModel_IssueList, Status_Operation.ADD);

                                    if ((bool)result["Result"])
                                    {
                                        ok_num += 1;

                                    }
                                    else
                                    {

                                        ng_num += 1;
                                        ErrMsg.Append(result["Result"].ToString() + "<br/>");
                                    }




                                }
                                else
                                {
                                    ng_num += 1;

                                }

                            }
                            catch (Exception ex)
                            {
                                Alert(ex.ToString());
                            }
                            #endregion

                        }




                    }
                }
                catch (Exception ex)
                {
                    Alert("Excel數據有問題,錯誤信息: " + ex.Message);
                    return;
                }

            }
            else
            {
                Alert("文件類型只能為xlsx");
                return;
            }
        }
        BindIssueList();
        IssueFileUpload.Reset();
        rdBatchIssueY.Checked = false;
        rdPBatchIssueN.Checked = false;
        Alert(string.Format("上傳筆數:{0}<BR/>成功筆數:{1}<BR/>失敗筆數:{2}<BR/>錯誤信息:<BR/>{3}", total_num.ToString(), ok_num.ToString(), ng_num.ToString(), ErrMsg.ToString()));

    }

    protected void btnModify_Click(object sender, DirectEventArgs e)
    {

        btnAdd.Hidden = true;
        btnModifySave.Hidden = false;
        rdBatchIssueY.Checked = false;
        rdPBatchIssueN.Checked = true;
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        RowSelectionModel sm = this.grdIssuesList.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count <= 0)
        {
            Alert("请选择须更新修改项!");
            return;
        }
        string json = e.ExtraParams["Values"];
        Dictionary<string, string>[] companies = JSON.Deserialize<Dictionary<string, string>[]>(json);
        string id = string.Empty;
        string errMsg = string.Empty;
        Model_NPI_APP_ISSUELIST oModel_IssueList = new Model_NPI_APP_ISSUELIST();
        foreach (Dictionary<string, string> row in companies)
        {

            txtDescription_Issues.Text = row["ISSUE_DESCRIPTION"];
            txtTempMeasure.Text = row["TEMP_MEASURE"];
            cbLosseItem.SelectedItem.Text = row["ISSUE_LOSSES"];
            cbClass.SelectedItem.Text = row["CLASS"];
            txtProjectStation.Text = row["STATION"];
            hidIssuesID.Text = row["ID"].ToString();
            txtImporveMeasure.Text = row["IMPROVE_MEASURE"].ToString();
        }
    }

    protected void btnModifySave_Click(object sender, DirectEventArgs e)
    {
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        StringBuilder ErrMsg = new StringBuilder();
        SpmMaster _Master = (SpmMaster)Master;
        string Model = txtModel_R.Text.Trim();
        string Stage = sbPhase.SelectedItem.Text;
        string MFILE_NAME = string.Empty;
        string MFILE_PATH = string.Empty;
        if (IssuesUploadFile.PostedFile.FileName.ToString().Length > 0)
        {
            int indexAttachment = IssuesUploadFile.FileName.LastIndexOf('.');
            string extAttachment = IssuesUploadFile.FileName.Substring(indexAttachment + 1);
            MFILE_NAME = IssuesUploadFile.FileName.Substring(IssuesUploadFile.FileName.LastIndexOf("\\") + 1);
            FileInfo FlInf = new FileInfo(IssuesUploadFile.PostedFile.FileName);
            string File_Ext = FlInf.Extension.ToLower();
            String FileTypes = System.Configuration.ConfigurationSettings.AppSettings["UploadFilesTypes"];
            String[] FileType = FileTypes.Split('|');
            bool FileType_Flag = false;
            foreach (String aType in FileType)
            {
                if (File_Ext.Equals("." + aType))
                {
                    FileType_Flag = true;
                    break;
                }
            }
            if (!FileType_Flag)
            {
                Alert("False只能上傳后綴名為 " + FileTypes + "的文件 .");
                return;
            }
            string type = "IssuesList";
            string CaseID = _Master.IFormURLPara.CaseId.ToString();
            string docNoPath = Server.MapPath("~/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + type + " DOC");
            string filepath = (docNoPath + "/" + MFILE_NAME);
            bool IsDocNoExist = Directory.Exists(docNoPath);
            MFILE_PATH = "Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + type + " DOC" + "/" + MFILE_NAME;
            if (!IsDocNoExist)
            {
                Directory.CreateDirectory(docNoPath);
            }

            try
            {

                IssuesUploadFile.PostedFile.SaveAs(docNoPath + "/" + MFILE_NAME);

            }
            catch (Exception ee)
            {
                Alert("False保存文件失败：" + ee.Message + " , " + filepath);
                return;
            }

        }

        Model_NPI_APP_ISSUELIST oModel_IssueList = new Model_NPI_APP_ISSUELIST();
        oModel_IssueList._FILENAME = MFILE_NAME;
        oModel_IssueList._FILEPATH = MFILE_PATH;
        //string Item = txtCustomer.Text.Trim();
        //string Class = string.Empty;
        string Descripiton = txtDescription_Issues.Text.Trim();
        string TempMeasure = txtTempMeasure.Text.Trim();
        string LosseItem = cbLosseItem.SelectedItem.Text.Trim();
        string IssueClass = cbClass.SelectedItem.Text.Trim();
        string ImproveMeasure = txtImporveMeasure.Text.Trim();
        //string Station = txtProjectStation.Text.Trim();

        try
        {
            string sql = "UPDATE TB_NPI_APP_ISSUELIST SET FILE_PATH=@file_path,FILE_NAME=@file_name,ISSUE_DESCRIPTION=@ISSUE_DESCRIPTION,"
                        + " IMPROVE_MEASURE=@IMPROVE_MEASURE,TEMP_MEASURE=@TEMP_MEASURE,ISSUE_LOSSES=@ISSUE_LOSSES,CLASS=@CLASS"
                        + " WHERE ID=@id";
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@ISSUE_DESCRIPTION", DbType.String, Descripiton));
            opc.Add(DataPara.CreateDataParameter("@IMPROVE_MEASURE", DbType.String, ImproveMeasure));
            opc.Add(DataPara.CreateDataParameter("@TEMP_MEASURE", DbType.String, TempMeasure));
            opc.Add(DataPara.CreateDataParameter("@ISSUE_LOSSES", DbType.String, LosseItem));
            opc.Add(DataPara.CreateDataParameter("@CLASS", DbType.String, IssueClass));


            opc.Add(DataPara.CreateDataParameter("@file_path", DbType.String, MFILE_PATH));
            opc.Add(DataPara.CreateDataParameter("@file_name", DbType.String, MFILE_NAME));
            opc.Add(DataPara.CreateDataParameter("@id", DbType.String, hidIssuesID.Text));
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            sdb.TransactionExecuteScalar(sql, opc);
            BindIssueList();
            RefreshIssuesInfo();
            btnAdd.Hidden = false;
            btnModifySave.Hidden = true;
            rdBatchIssueY.Checked = false;
            rdPBatchIssueN.Checked = false;

        }        //try
        //{
        //    string sql = "UPDATE TB_NPI_APP_ISSUELIST SET FILE_PATH=@file_path,FILE_NAME=@file_name"
        //                +" WHERE ID=@id";
        //    opc.Clear();
        //    opc.Add(DataPara.CreateDataParameter("@file_path", DbType.String, MFILE_PATH));
        //    opc.Add(DataPara.CreateDataParameter("@file_name", DbType.String,MFILE_NAME));
        //    opc.Add(DataPara.CreateDataParameter("@id", DbType.String, hidIssuesID.Text));
        //    SqlDB sdb=new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        //    sdb.TransactionExecuteScalar(sql, opc);
        //    BindIssueList();
        //    RefreshIssuesInfo();
        //    btnAdd.Hidden = false;
        //    btnModifySave.Hidden = true;
        //    rdBatchIssueY.Checked = false;
        //    rdPBatchIssueN.Checked = false;

        //}
        catch (Exception ex)
        {
            Alert(ex.ToString());

        }
    }

    protected void btnIssuesExport_click(object sender, DirectEventArgs e)
    {
        string templatePath = string.Empty;

        Aspose.Cells.License lic = new Aspose.Cells.License();
        string AsposeLicPath = System.Configuration.ConfigurationSettings.AppSettings["AsposeLicPath"].ToString();
        lic.SetLicense(AsposeLicPath);

        templatePath = Page.Server.MapPath("~/IssuesList.xlsx");
        //Instantiate a new Workbook object.
        Aspose.Cells.Workbook book = new Aspose.Cells.Workbook(templatePath);
        Aspose.Cells.Worksheet sheet0 = book.Worksheets[0];// Issuelist
        // REPLACE PUBLIC FIELDS
        BindIssuelist_Excel(ref sheet0);
        SetColumnAuto(ref sheet0);
        this.Response.Clear();
        book.Save("IssuesList_Excel.xls",
         Aspose.Cells.FileFormatType.Excel97To2003,
         Aspose.Cells.SaveType.OpenInExcel,
         this.Response,
         System.Text.Encoding.UTF8);
    }

    private void BindIssuelist_Excel(ref Aspose.Cells.Worksheet sheet)
    {
        //page 格式設定
        SetStyle(ref sheet, Aspose.Cells.PageOrientationType.Landscape);
        Aspose.Cells.Cells cells = sheet.Cells;

        NPIMgmt oMgmt = new NPIMgmt("CZ", "Power");
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();

        #region//獲取主表資訊
        string Dept = oStandard.GetUserDept("A", lblLogonId.Text, txtPROD_GROUP.Text);
        DataTable dtMaster = oStandard.GetIssuesInconformity(txtSub_No.Text, Dept, string.Empty);

        if (dtMaster.Rows.Count > 0)
        {
            int templateIndexDFX = 1;//模板row起始位置
            int insertIndexEnCounter = templateIndexDFX + 1;//new row起始位置

            cells.InsertRows(insertIndexEnCounter, dtMaster.Rows.Count - 1);
            cells.CopyRows(cells, templateIndexDFX, insertIndexEnCounter, dtMaster.Rows.Count - 1); //複製模板row格式至新行

            for (int i = 0; i < dtMaster.Rows.Count; i++)
            {
                DataRow dr = dtMaster.Rows[i];
                cells[i + templateIndexDFX, 0].PutValue(dr["ID"].ToString());
                cells[i + templateIndexDFX, 1].PutValue(dr["DEPT"].ToString());
                cells[i + templateIndexDFX, 2].PutValue(dr["STATION"].ToString());
                cells[i + templateIndexDFX, 3].PutValue(dr["ISSUE_DESCRIPTION"].ToString());
                cells[i + templateIndexDFX, 4].PutValue(dr["CLASS"].ToString());
                cells[i + templateIndexDFX, 5].PutValue(dr["ISSUE_LOSSES"].ToString());
                cells[i + templateIndexDFX, 6].PutValue(dr["TEMP_MEASURE"].ToString());
                cells[i + templateIndexDFX, 7].PutValue(dr["IMPROVE_MEASURE"].ToString());
                cells[i + templateIndexDFX, 8].PutValue(dr["MEASURE_DEPTREPLY"].ToString());
                cells[i + templateIndexDFX, 9].PutValue(dr["CURRENT_STATUS"].ToString());
                cells[i + templateIndexDFX, 10].PutValue(dr["TRACKING"].ToString());
                cells[i + templateIndexDFX, 11].PutValue(dr["DUE_DAY"].ToString().Length > 0 ? Convert.ToDateTime(dr["DUE_DAY"].ToString()).ToString("yyyy/MM/dd") : dr["DUE_DAY"].ToString());
                cells[i + templateIndexDFX, 12].PutValue(dr["REMARK"].ToString());
            }
        }

        #endregion

    }

    protected void btnUploadIssues_Click(object sender, DirectEventArgs e)
    {
        IssuesUpload.Hidden = false;
    }

    protected void btnIssuesUploadData_Click(object sender, DirectEventArgs e)//I
    {
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        StringBuilder ErrMsg = new StringBuilder();
        string file = txtIssuesField.FileName;
        int total_num = 0;
        int ok_num = 0;
        int ng_num = 0;
        bool isok = true;
        if (!txtIssuesField.HasFile)
        {
            Alert("請選擇数据文件!");
            return;
        }
        else
        {
            string type = file.Substring(file.LastIndexOf(".") + 1).ToLower();
            if (type == "xlsx")
            {


                //把文件轉成流
                Stream stream = txtIssuesField.PostedFile.InputStream;
                if (stream.Length == 8889)
                {
                    Alert("導入的資料表為空,請檢查!");
                    return;
                }
                try
                {

                    DataTable dt = ReadByExcelLibrary(stream);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string projectStation = dt.Rows[i]["STATION"].ToString();
                            string description = dt.Rows[i]["Issue Description"].ToString();
                            string improvemeasure = dt.Rows[i]["Suggestion Action"].ToString();
                            string dept = dt.Rows[i]["DEPT"].ToString();
                            string DeptReply = dt.Rows[i]["MEASURE_DEPTREPLY"].ToString();
                            string IssuesRemark = dt.Rows[i]["REMARK"].ToString();
                            string CurrentStatus = dt.Rows[i]["CURRENT_STATUS"].ToString();
                            string IssuesStatus = dt.Rows[i]["TRACKING"].ToString();
                            string DueDay = dt.Rows[i]["DUE_DAY"].ToString();

                            #region 控件值驗證是否為空
                            if (string.IsNullOrEmpty(projectStation))
                            {
                                isok = false;
                                ErrMsg.Append("第" + (i + 2) + "行站別不能為空</br>");
                            }
                            if (string.IsNullOrEmpty(dept))
                            {
                                isok = false;
                                ErrMsg.Append("第" + (i + 2) + "行部門不能為空</br>");
                            }
                            if (string.IsNullOrEmpty(description))
                            {
                                isok = false;
                                ErrMsg.Append("第" + (i + 2) + "行問題表述不能為空</br>");
                            }
                            if (string.IsNullOrEmpty(improvemeasure))
                            {
                                isok = false;
                                ErrMsg.Append("第" + (i + 2) + "行建議改善對策不能為空</br>");
                            }

                            if (string.IsNullOrEmpty(DeptReply))
                            {
                                isok = false;
                                ErrMsg.Append("第" + (i + 2) + "行責任部門對策不能為空</br>");
                            }

                            if (string.IsNullOrEmpty(CurrentStatus))
                            {
                                isok = false;
                                ErrMsg.Append("第" + (i + 2) + "行结果确认不能為空</br>");
                            }
                            if (string.IsNullOrEmpty(IssuesStatus))
                            {
                                isok = false;
                                ErrMsg.Append("第" + (i + 2) + "行改善狀態不能為空</br>");
                            }
                            if (string.IsNullOrEmpty(DueDay))
                            {
                                isok = false;
                                ErrMsg.Append("第" + (i + 2) + "行Due Day不能為空</br>");
                            }
                            if (!isResposibility(IssuesStatus))
                            {
                                isok = false;
                                ErrMsg.Append("第" + (i + 2) + "行改善狀態有誤</br>");
                            }
                            #endregion
                        }
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            total_num = dt.Rows.Count;

                            string projectStation = dt.Rows[i]["STATION"].ToString();
                            string description = dt.Rows[i]["Issue Description"].ToString();
                            string improvemeasure = dt.Rows[i]["Suggestion Action"].ToString();
                            string dept = dt.Rows[i]["DEPT"].ToString();
                            string DeptReply = dt.Rows[i]["MEASURE_DEPTREPLY"].ToString();
                            string IssuesRemark = dt.Rows[i]["REMARK"].ToString();
                            string CurrentStatus = dt.Rows[i]["CURRENT_STATUS"].ToString();
                            string IssuesStatus = dt.Rows[i]["TRACKING"].ToString();
                            string DueDay = dt.Rows[i]["DUE_DAY"].ToString();



                            Model_NPI_APP_ISSUELIST oModel_IssueList = new Model_NPI_APP_ISSUELIST();
                            oModel_IssueList._CURRENT_STATUS = CurrentStatus;
                            oModel_IssueList._REMARK = IssuesRemark;
                            oModel_IssueList._DEPT = dept;
                            oModel_IssueList._STAUTS = "Finished";
                            oModel_IssueList._TRACKING = IssuesStatus;
                            oModel_IssueList._ID = dt.Rows[i]["ID"].ToString().Trim();
                            oModel_IssueList._DUE_DAY = DueDay;
                            oModel_IssueList._MEASURE_DEPTREPLY = DeptReply;

                            try
                            {
                                if (isok)
                                {


                                    Dictionary<string, object> result = oStandard.RecordOperation_APPIssueList(oModel_IssueList, Status_Operation.UPDATE);
                                    if ((bool)result["Result"])
                                    {
                                        ok_num += 1;

                                        pnlIssuesReply.Hide();
                                        btnIssuesExport.Hide();
                                        btnUploadIssues.Hide();
                                    }
                                    else
                                    {
                                        ng_num += 1;
                                        Alert((string)result["ErrMsg"].ToString());
                                    }
                                }
                                else
                                {
                                    ng_num += 1;
                                }
                            }
                            catch (Exception ex)
                            {
                                Alert(ex.ToString());

                            }

                        }
                    }


                }
                catch (Exception ex)
                {
                    Alert("Excel數據有問題,錯誤信息: " + ex.Message);
                    return;
                }

            }
            else
            {
                ErrMsg.Append("文件類型只能為xlsx");
            }
        }
        BindIssueList();
        RefreshIssuesReply();
        txtIssuesField.Reset();
        IssuesUpload.Hidden = true;
        pnlIssuesReply.Hidden = true;
        btnIssuesExport.Hidden = true;
        btnUploadIssues.Hidden = true;
        Alert(string.Format("上傳筆數:{0}<BR/>成功筆數:{1}<BR/>失敗筆數:{2}<BR/>錯誤信息:<BR/>{3}", total_num.ToString(), ok_num.ToString(), ng_num.ToString(), ErrMsg.ToString()));
    }

    #endregion

    #region PFMA
    protected void btnPAdd_Click(object sender, DirectEventArgs e)
    {
        SpmMaster _Master = (SpmMaster)Master;
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        string Item = txtFMEAItem.Text.Trim();
        string Source = txtFMEASource.Text.Trim();
        string Status = string.Empty;
        string Station = txtFMEAStation.Text.Trim();
        string Mode = txtFMEAMode.Text.Trim();
        string LosseItem = cbFMEALosseItem.SelectedItem.Text.Trim();
        string Sev = txtFMEASev.Text.Trim();
        string Occ = txtFMEAOcc.Text.Trim();
        string Det = txtFMEADet.Text.Trim();
        string PRN = txtFMEAPRN.Text.Trim();
        string Causes = txtFMEACauses.Text.Trim();
        string Model = txtModel_R.Text.Trim();
        string Stage = sbPhase.SelectedItem.Text;
        string Controls = string.Empty;
        StringBuilder ErrMsg = new StringBuilder();

        #region 控件值驗證是否為空
        if (string.IsNullOrEmpty(Station))
        {
            ErrMsg.Append("工位不能為空</br>");
        }
        if (string.IsNullOrEmpty(Item))
        {
            ErrMsg.Append("編號不能為空</br>");
        }
        if (string.IsNullOrEmpty(cmbPFMADept.SelectedItem.Text))
        {
            ErrMsg.Append("部門不能為空</br>");
        }
        if (string.IsNullOrEmpty(Source))
        {
            ErrMsg.Append("問題描述不能為空</br>");
        }
        if (string.IsNullOrEmpty(Mode))
        {
            ErrMsg.Append("潛在失效模式不能為空</br>");
        }
        if (string.IsNullOrEmpty(LosseItem))
        {
            ErrMsg.Append("違反損失不能為空</br>");
        }
        if (string.IsNullOrEmpty(Sev))
        {
            ErrMsg.Append("嚴重度不能為空</br>");
        }
        if (string.IsNullOrEmpty(Occ))
        {
            ErrMsg.Append("發生度不能為空</br>");
        }
        if (string.IsNullOrEmpty(Det))
        {
            ErrMsg.Append("偵測度不能為空</br>");
        }
        if (string.IsNullOrEmpty(PRN))
        {
            ErrMsg.Append("風險優先度不能為空</br>");
        }
        if (string.IsNullOrEmpty(Causes))
        {
            ErrMsg.Append("建議改善對策不能為空</br>");
        }





        //if (string.IsNullOrEmpty(Causes))
        //{
        //    ErrMsg.Append("失效原因不能為空</br>");
        //}
        //  if (string.IsNullOrEmpty(Controls))
        //{
        //    ErrMsg.Append("目前控制計劃不能為空</br>");
        //}
        if (ErrMsg.ToString().Length > 0)
        {
            Alert(ErrMsg.ToString());
            return;
        }
        #endregion

        string MFILE_NAME = string.Empty;
        string MFILE_PATH = string.Empty;

        if (FMEAUploadFile.PostedFile.FileName.ToString().Length > 0)
        {
            int indexAttachment = FMEAUploadFile.FileName.LastIndexOf('.');
            string extAttachment = FMEAUploadFile.FileName.Substring(indexAttachment + 1);
            MFILE_NAME = FMEAUploadFile.FileName.Substring(FMEAUploadFile.FileName.LastIndexOf("\\") + 1);
            FileInfo FlInf = new FileInfo(FMEAUploadFile.PostedFile.FileName);
            string File_Ext = FlInf.Extension.ToLower();
            String FileTypes = System.Configuration.ConfigurationSettings.AppSettings["UploadFilesTypes"];
            String[] FileType = FileTypes.Split('|');
            bool FileType_Flag = false;
            foreach (String aType in FileType)
            {
                if (File_Ext.Equals("." + aType))
                {
                    FileType_Flag = true;
                    break;
                }
            }
            if (!FileType_Flag)
            {
                Alert("False只能上傳后綴名為 " + FileTypes + "的文件 .");
                return;
            }
            string type = "FMEA";
            string CaseID = _Master.IFormURLPara.CaseId.ToString();
            string docNoPath = Server.MapPath("~/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + type + " Doc");

            string filepath = (docNoPath + "/" + MFILE_NAME);
            bool IsDocNoExist = Directory.Exists(docNoPath);
            MFILE_PATH = "Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + type + " Doc" + "/" + MFILE_NAME;
            if (!IsDocNoExist)
            {
                Directory.CreateDirectory(docNoPath);
            }



            try
            {

                FMEAUploadFile.PostedFile.SaveAs(docNoPath + "/" + MFILE_NAME);

            }
            catch (Exception ee)
            {
                Alert("False保存文件失败：" + ee.Message + " , " + filepath);
                return;
            }

        }
        Model_NPI_FMEA oModel_FMEA = new Model_NPI_FMEA();
        oModel_FMEA._SubNo = txtSub_No.Text.Trim();
        oModel_FMEA._Item = txtFMEAItem.Text;
        oModel_FMEA._Source = Source;
        oModel_FMEA._Stantion = Station;
        oModel_FMEA._PotentialFailureMode = Mode;
        oModel_FMEA._Loess = LosseItem;
        oModel_FMEA._Sev = int.Parse(Sev);
        oModel_FMEA._PotentialFailure = Causes;
        oModel_FMEA._Occ = int.Parse(Occ);
        oModel_FMEA._CurrentControls = Controls;
        oModel_FMEA._DET = int.Parse(Det);
        oModel_FMEA._Status = "Write";
        oModel_FMEA._RPN = int.Parse(PRN);
        oModel_FMEA._FILENAME = MFILE_NAME;
        oModel_FMEA._FILEPATH = MFILE_PATH;
        oModel_FMEA._WriteDept = cmbPFMADept.SelectedItem.Text;

        try
        {
            Dictionary<string, object> result = oStandard.RecordOperation_FMEA(oModel_FMEA, Status_Operation.ADD);
            if ((bool)result["Result"])
            {
                Alert("新增成功!");
                BindPFMAList();
                RefreshPFMAWrite();
            }
            else
            {
                Alert((string)result["ErrMsg"].ToString());
            }
        }
        catch (Exception ex)
        {
            Alert(ex.ToString());

        }
    }

    protected void btnPFMADelete_Click(object sender, DirectEventArgs e)
    {

        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        RowSelectionModel sm = this.grdPFMAList.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count <= 0)
        {
            Alert("请选择须删除项!");
            return;
        }
        string json = e.ExtraParams["Values"];
        Dictionary<string, string>[] companies = JSON.Deserialize<Dictionary<string, string>[]>(json);
        string SubDoc = string.Empty;
        string Item = string.Empty;
        string ID = string.Empty;
        string errMsg = string.Empty;
        Model_NPI_FMEA oModel_FMEA = new Model_NPI_FMEA();

        foreach (Dictionary<string, string> row in companies)
        {
            SubDoc = row["SubNo"];
            Item = row["Item"];
            ID = row["ID"];
            oModel_FMEA._SubNo = SubDoc;
            oModel_FMEA._Item = Item;
            oModel_FMEA._ID = int.Parse(ID);
            try
            {
                Dictionary<string, object> result = oStandard.RecordOperation_FMEA(oModel_FMEA, Status_Operation.DELETE);
                if ((bool)result["Result"])
                {
                    Alert("删除成功!");
                    RefreshPFMARely();
                    BindPFMAList();
                }
                else
                {
                    Alert((string)result["ErrMsg"].ToString());
                }
            }

            catch (Exception ex)
            {
                Alert(ex.ToString());
            }
        }
    }

    protected void BindPFMAList()
    {
        SpmMaster _Master = (SpmMaster)Master;
        string StepName = _Master.IFormURLPara.StepName.ToString();
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        StringBuilder sb = new StringBuilder();
        ArrayList opc = new ArrayList();
        if (StepName == "Dept.Reply" || StepName == "Dept.Write")
        {
            string Dept = GetUserDept("AF", lblLogonId.Text, txtPROD_GROUP.Text, StepName);
            DataTable dt = oStandard.GetFMEAInconformity(txtSub_No.Text, Dept, string.Empty);
            BindGrid(grdPFMAList, dt);
        }
        else
        {
            string Dept = oStandard.GetUserDept("A", lblLogonId.Text, txtPROD_GROUP.Text);
            DataTable dt = oStandard.GetFMEAInconformity(txtSub_No.Text, Dept, string.Empty);
            BindGrid(grdPFMAList, dt);
        }

    }

    protected void btnPReply_Click(object sender, DirectEventArgs e)
    {
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        //string Actions=txtPReplyActions.Text.Trim();
        string Resposibility = txtPReplyResposibility.Text.Trim();
        DateTime CompleteDate = txtPReplyCompletDate.SelectedDate;
        string Taken = txtPReplyActionTaken.Text.Trim();
        string ReplySev = txtPReplySev.Text.Trim();
        string RepyOcc = txtPReplyOcc.Text.Trim();
        string RepydDet = txtPReplyDet.Text.Trim();
        string ReplyRPN = txtPReplyRPN.Text.Trim();
        string ReplyResposibility = txtPReplyResposibility.SelectedItem.ToString();
        StringBuilder ErrMsg = new StringBuilder();
        #region 控件值驗證是否為空

        //if (string.IsNullOrEmpty(Actions))
        //{
        //    ErrMsg.Append("問題來源不能為空</br>");
        //}
        //if (string.IsNullOrEmpty(Resposibility))
        //{
        //    ErrMsg.Append("處理狀態不能為空</br>");
        //}
        //if (string.IsNullOrEmpty(CompleteDate))
        //{
        //    ErrMsg.Append("工位不能為空</br>");
        //}
        if (string.IsNullOrEmpty(Taken))
        {
            ErrMsg.Append("實際執行之對策不能為空</br>");
        }
        if (string.IsNullOrEmpty(ReplySev))
        {
            ErrMsg.Append("改善後嚴重不能為空</br>");
        }
        if (string.IsNullOrEmpty(RepyOcc))
        {
            ErrMsg.Append("改善後發生率不能為空</br>");
        }
        if (string.IsNullOrEmpty(RepydDet))
        {
            ErrMsg.Append("改善後偵測度不能為空</br>");
        }
        if (string.IsNullOrEmpty(ReplyResposibility))
        {
            ErrMsg.Append("處理狀態不能為空</br>");
        }
        //if (string.IsNullOrEmpty(PRN))
        //{
        //    ErrMsg.Append("風險優先度不能為空</br>");
        //}
        //if (string.IsNullOrEmpty(Causes))
        //{
        //    ErrMsg.Append("失效原因不能為空</br>");
        //}
        //if (string.IsNullOrEmpty(Controls))
        //{
        //    ErrMsg.Append("目前控制計劃不能為空</br>");
        //}
        if (ErrMsg.ToString().Length > 0)
        {
            Alert(ErrMsg.ToString());
            return;
        }
        #endregion
        Model_NPI_FMEA oModel_FMEA = new Model_NPI_FMEA();

        oModel_FMEA._Resposibility = Resposibility;
        oModel_FMEA._TargetCompletionDate = CompleteDate;
        oModel_FMEA._ActionsTaken = Taken;
        oModel_FMEA._ResultsSev = int.Parse(ReplySev);
        oModel_FMEA._ResultsOcc = int.Parse(RepyOcc);
        oModel_FMEA._ResultsDet = int.Parse(RepydDet);
        oModel_FMEA._ResultsRPN = int.Parse(ReplyRPN);
        oModel_FMEA._WriteDept = GetUserDept("PFMEA TeamMember");

        oModel_FMEA._Status = "Finished";
        Session["Dept"] = oModel_FMEA._WriteDept;
        string ID = Session["ID"].ToString();
        oModel_FMEA._ID = Convert.ToInt32(ID);

        try
        {
            Dictionary<string, object> result = oStandard.RecordOperation_FMEA(oModel_FMEA, Status_Operation.UPDATE);
            if ((bool)result["Result"])
            {
                BindPFMAList();
                pnlPFMAReply.Hide();
                btnPFMAExport.Hide();
                btnUploadPFMA.Hide();
            }
            else
            {
                Alert((string)result["ErrMsg"].ToString());
            }
        }
        catch (Exception ex)
        {
            Alert(ex.ToString());

        }


    }

    protected void grdPFMA_RowCommand(object sender, DirectEventArgs e)
    {
        string command = e.ExtraParams["command"];
        switch (command)
        {

            case "Reply":
                RefreshPFMARely();
                Session["ID"] = e.ExtraParams["ID"];
                Session["SubNo"] = e.ExtraParams["SubNo"];
                FMEAItem.Text = e.ExtraParams["Item"];
                FMEAStantion.Text = e.ExtraParams["Stantion"];
                FMEASource.Text = e.ExtraParams["Source"];
                FMEAPotentialFailureMode.Text = e.ExtraParams["PotentialFailureMode"];
                txtPReplyActionTaken.Text = e.ExtraParams["ActionsTaken"];

                txtPReplySev.Text = e.ExtraParams["ResultsSev"];
                txtPReplyOcc.Text = e.ExtraParams["ResultsOcc"];
                txtPReplyDet.Text = e.ExtraParams["ResultsDet"];
                txtPReplyRPN.Text = e.ExtraParams["ResultsRPN"];
                txtPReplyCompletDate.Text = e.ExtraParams["TargetCompletionDate"];
                FMEAPotentialFailure.Text = e.ExtraParams["PotentialFailure"];
                txtPReplyResposibility.SelectedItem.Text = e.ExtraParams["Resposibility"];
                break;
        }
    }

    protected void RefreshPFMARely()
    {

        txtPReplyResposibility.Text = string.Empty;
        txtPReplyCompletDate.Text = string.Empty;
        txtPReplyActionTaken.Text = string.Empty;
        txtPReplySev.Text = "1";
        txtPReplyOcc.Text = "1";
        txtPReplyDet.Text = "1";
        txtPReplyRPN.Text = string.Empty;

        pnlPFMAReply.Hidden = false;
        btnPFMAExport.Hidden = false;
        btnUploadPFMA.Hidden = false;
    }


    protected void RefreshPFMAWrite()
    {
        txtFMEAItem.Text = string.Empty;
        txtFMEASource.Text = string.Empty;

        txtFMEAStation.Text = string.Empty;
        txtFMEAMode.Text = string.Empty;
        cbFMEALosseItem.SelectedItem.Text = string.Empty;
        txtFMEASev.Text = "0";
        txtFMEAOcc.Text = "0";
        txtFMEADet.Text = "0";
        txtFMEAPRN.Text = string.Empty;
        txtFMEACauses.Text = string.Empty;
        FMEAUploadFile.Text = string.Empty;
        FMEAUploadFile.Reset();
    }

    protected void Clac_PRN(object sender, DirectEventArgs e)
    {
        if (txtFMEASev.Text.Length > 0)
        {
            int FMEASev = int.Parse(txtFMEASev.Text.Trim());
            if (FMEASev > 10 || FMEASev < 1)
            {
                Alert("輸入的數字必須在1~10之間！");
                txtFMEASev.Text = "1";
            }
        }

        if (txtFMEAOcc.Text.Length > 0)
        {
            int FMEAOcc = int.Parse(txtFMEAOcc.Text.Trim());
            if (FMEAOcc > 10 || FMEAOcc < 1)
            {
                Alert("輸入的數字必須在1~10之間！");
                txtFMEAOcc.Text = "1";
            }
        }

        if (txtFMEADet.Text.Length > 0)
        {
            int FMEADet = int.Parse(txtFMEADet.Text.Trim());
            if (FMEADet > 10 || FMEADet < 1)
            {
                Alert("輸入的數字必須在1~10之間！");
                txtFMEADet.Text = "1";
            }
        }
        txtFMEAPRN.Number = txtFMEASev.Number * txtFMEAOcc.Number * txtFMEADet.Number;
    }

    protected void Clac_PRN2(object sender, DirectEventArgs e)
    {

        if (txtPReplySev.Text.Length > 0)
        {
            int PReplySev = int.Parse(txtPReplySev.Text.Trim());
            if (PReplySev > 10 || PReplySev < 1)
            {
                Alert("輸入的數字必須在1~10之間！");
                txtPReplySev.Text = "1";
            }
        }

        if (txtPReplyOcc.Text.Length > 0)
        {
            int PReplyOcc = int.Parse(txtPReplyOcc.Text.Trim());
            if (PReplyOcc > 10 || PReplyOcc < 1)
            {
                Alert("輸入的數字必須在1~10之間！");
                txtPReplyOcc.Text = "1";
            }
        }

        if (txtPReplyDet.Text.Length > 0)
        {
            int PReplyDet = int.Parse(txtPReplyDet.Text.Trim());
            if (PReplyDet > 10 || PReplyDet < 1)
            {
                Alert("輸入的數字必須在1~10之間！");
                txtPReplyDet.Text = "1";
            }
        }
        txtPReplyRPN.Number = txtPReplySev.Number * txtPReplyOcc.Number * txtPReplyDet.Number;
    }

    protected void txtFMEAStation_Changed(object sender, DirectEventArgs e)
    {
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        string PrefixString = string.Empty;
        string Dept = oStandard.GetUserDept("A", lblLogonId.Text, txtPROD_GROUP.Text);
        string Number = GetItemsNumber(txtFMEAStation.Text.Trim());

        if (txtFMEAStation.Text.Length > 0)
        {
            PrefixString = txtFMEAStation.Text.Trim().ToString();

            txtFMEAItem.Text = PrefixString + "-" + Dept + Number;

        }
    }

    protected string GetItemsNumber(string Station)
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        string Itmes = string.Empty;
        string sql = " SELECT  max(SUBSTRING(Item,LEN(Item)-1,2))+1  as Number"
                  + " FROM [NPI_REPORT].[dbo].[TB_NPI_FMEA]"
                  + "WHERE SubNo=@subNo"
                  + " AND Stantion=@Station";
        ArrayList opc = new ArrayList();
        opc.Add(DataPara.CreateDataParameter("@subNo", DbType.String, txtSub_No.Text.Trim()));
        opc.Add(DataPara.CreateDataParameter("@Station", DbType.String, Station));
        DataTable dt = sdb.TransactionExecute(sql, opc);


        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            if (string.IsNullOrEmpty(dr["Number"].ToString()))
            {
                Itmes = "01";
            }
            else
            {
                Itmes = dr["Number"].ToString().PadLeft(2, '0');
            }

        }
        else
        {
            Itmes = "01";
        }
        return Itmes;
    }

    //protected void btnPFMAExport_click(object sender, DirectEventArgs e)
    //{


    //    string json = e.ExtraParams["values"];

    //    //json = json.Replace("FormNo", "報修單號");

    //    StoreSubmitDataEventArgs eSubmit = new StoreSubmitDataEventArgs(json, null);
    //    XmlNode xml = eSubmit.Xml;
    //    XmlNodeList xm = xml.SelectSingleNode("records").ChildNodes;
    //    foreach (XmlNode xnl in xm)
    //    {
    //        XmlElement xe = (XmlElement)xnl;
    //        XmlNode xn = xe.SelectSingleNode("SubNo");
    //        xnl.RemoveChild(xn);

    //        xn = xe.SelectSingleNode("Loess");
    //        xnl.RemoveChild(xn);

    //        xn = xe.SelectSingleNode("ResultsSev");
    //        xnl.RemoveChild(xn);
    //        xn = xe.SelectSingleNode("ResultsOcc");
    //        xnl.RemoveChild(xn);
    //        xn = xe.SelectSingleNode("CurrentControls");
    //        xnl.RemoveChild(xn);
    //        xn = xe.SelectSingleNode("DET");
    //        xnl.RemoveChild(xn);
    //        xn = xe.SelectSingleNode("RPN");
    //        xnl.RemoveChild(xn);
    //        xn = xe.SelectSingleNode("RecommendedAction");
    //        xnl.RemoveChild(xn);


    //        xn = xe.SelectSingleNode("ResultsRPN");
    //        xnl.RemoveChild(xn);
    //        xn = xe.SelectSingleNode("ReplyDept");
    //        xnl.RemoveChild(xn);
    //        xn = xe.SelectSingleNode("Status");
    //        xnl.RemoveChild(xn);
    //        xn = xe.SelectSingleNode("Update_User");
    //        xnl.RemoveChild(xn);
    //        xn = xe.SelectSingleNode("Update_Time");
    //        xnl.RemoveChild(xn);
    //        xn = xe.SelectSingleNode("FILE_PATH");
    //        xnl.RemoveChild(xn);
    //        xn = xe.SelectSingleNode("id");
    //        xnl.RemoveChild(xn);



    //        //xn = xe.SelectSingleNode("ISSUE_DESCRIPTION");
    //        //xnl.RemoveChild(xn);
    //    }

    //    this.Response.Clear();
    //    this.Response.ContentType = "application/vnd.ms-excel";
    //    this.Response.AddHeader("Content-Disposition", "attachment; filename=FMEA_Reply.xls");
    //    XslCompiledTransform xtExcel = new XslCompiledTransform();
    //    xtExcel.Load(Server.MapPath("Excel.xsl"));
    //    xtExcel.Transform(xml, null, this.Response.OutputStream);

    //    this.Response.End();


    //}

    protected void btnPFMAExport_click(object sender, DirectEventArgs e)
    {
        string templatePath = string.Empty;

        Aspose.Cells.License lic = new Aspose.Cells.License();
        string AsposeLicPath = System.Configuration.ConfigurationSettings.AppSettings["AsposeLicPath"].ToString();
        lic.SetLicense(AsposeLicPath);

        templatePath = Page.Server.MapPath("~/P-FMA.xlsx");
        //Instantiate a new Workbook object.
        Aspose.Cells.Workbook book = new Aspose.Cells.Workbook(templatePath);
        Aspose.Cells.Worksheet sheet0 = book.Worksheets[0];// PMFEA
        // REPLACE PUBLIC FIELDS
        BindPFMA(ref sheet0);
        SetColumnAuto(ref sheet0);
        this.Response.Clear();
        book.Save("FMEA_Excel.xls",
         Aspose.Cells.FileFormatType.Excel97To2003,
         Aspose.Cells.SaveType.OpenInExcel,
         this.Response,
         System.Text.Encoding.UTF8);
    }

    private void BindPFMA(ref Aspose.Cells.Worksheet sheet)
    {
        //page 格式設定
        SetStyle(ref sheet, Aspose.Cells.PageOrientationType.Landscape);
        Aspose.Cells.Cells cells = sheet.Cells;

        NPIMgmt oMgmt = new NPIMgmt("CZ", "Power");
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();

        #region//獲取主表資訊
        string Dept = oStandard.GetUserDept("A", lblLogonId.Text, txtPROD_GROUP.Text);
        DataTable dtMaster = oStandard.GetFMEAInconformity(txtSub_No.Text, Dept, string.Empty);

        if (dtMaster.Rows.Count > 0)
        {
            int templateIndexDFX = 1;//模板row起始位置
            int insertIndexEnCounter = templateIndexDFX + 1;//new row起始位置

            cells.InsertRows(insertIndexEnCounter, dtMaster.Rows.Count - 1);
            cells.CopyRows(cells, templateIndexDFX, insertIndexEnCounter, dtMaster.Rows.Count - 1); //複製模板row格式至新行

            for (int i = 0; i < dtMaster.Rows.Count; i++)
            {
                DataRow dr = dtMaster.Rows[i];
                cells[i + templateIndexDFX, 0].PutValue(dr["ID"].ToString());
                cells[i + templateIndexDFX, 1].PutValue(dr["WriteDept"].ToString());
                cells[i + templateIndexDFX, 2].PutValue(dr["Stantion"].ToString());
                cells[i + templateIndexDFX, 3].PutValue(dr["Source"].ToString());
                cells[i + templateIndexDFX, 4].PutValue(dr["PotentialFailureMode"].ToString());
                cells[i + templateIndexDFX, 5].PutValue(dr["Loess"].ToString());
                cells[i + templateIndexDFX, 6].PutValue(dr["Sev"].ToString());
                cells[i + templateIndexDFX, 7].PutValue(dr["Occ"].ToString());
                cells[i + templateIndexDFX, 8].PutValue(dr["DET"].ToString());
                cells[i + templateIndexDFX, 9].PutValue(dr["PotentialFailure"].ToString());
                cells[i + templateIndexDFX, 10].PutValue(dr["ActionsTaken"].ToString());
                cells[i + templateIndexDFX, 11].PutValue(dr["ResultsSev"].ToString());
                cells[i + templateIndexDFX, 12].PutValue(dr["ResultsOcc"].ToString());
                cells[i + templateIndexDFX, 13].PutValue(dr["ResultsDet"].ToString());
                cells[i + templateIndexDFX, 14].PutValue(dr["Resposibility"].ToString());
                cells[i + templateIndexDFX, 15].PutValue(dr["TargetCompletionDate"].ToString().Length > 0 ? Convert.ToDateTime(dr["TargetCompletionDate"].ToString()).ToString("yyyy/MM/dd") : dr["TargetCompletionDate"].ToString());
                //cells[i + templateIndexDFX, 15].PutValue(dr["ResultsOcc"].ToString());
                //cells[i + templateIndexDFX, 16].PutValue(dr["ResultsDet"].ToString());
                //cells[i + templateIndexDFX, 17].PutValue(dr["ResultsRPN"].ToString());
                //cells[i + templateIndexDFX, 18].PutValue(dr["WriteDept"].ToString());
            }
        }

        #endregion

    }


    protected void btnUploadPFMA_Click(object sender, DirectEventArgs e)
    {
        PFMAUpload.Hidden = false;
    }

    protected void btnPFMAUploadData_Click(object sender, DirectEventArgs e)
    {
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        StringBuilder ErrMsg = new StringBuilder();
        string file = txtPFMAField.FileName;
        int total_num = 0;
        int ok_num = 0;
        int ng_num = 0;
        bool isok = true;
        if (!txtPFMAField.HasFile)
        {
            Alert("請選擇数据文件!");
            return;
        }
        else
        {
            string type = file.Substring(file.LastIndexOf(".") + 1).ToLower();
            if (type == "xlsx")
            {


                //把文件轉成流
                Stream stream = txtPFMAField.PostedFile.InputStream;
                if (stream.Length == 8889)
                {
                    Alert("導入的資料表為空,請檢查!");
                    return;
                }
                try
                {

                    DataTable dt = ReadByExcelLibrary(stream);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string ID = dt.Rows[i]["ID"].ToString();
                            string Resposibility = dt.Rows[i]["Resposibility"].ToString();
                            DateTime CompleteDate = Convert.ToDateTime(dt.Rows[i]["TargetCompletionDate"].ToString());
                            string Taken = dt.Rows[i]["ActionsTaken"].ToString();
                            string ReplySev = dt.Rows[i]["ResultsSev"].ToString();
                            string RepyOcc = dt.Rows[i]["ResultsOcc"].ToString();
                            string RepydDet = dt.Rows[i]["ResultsDet"].ToString();
                            string WriteDept = dt.Rows[i]["WriteDept"].ToString();
                            #region 控件值驗證是否為空


                            if (string.IsNullOrEmpty(Taken))
                            {
                                isok = false;
                                ErrMsg.Append("第" + (i + 2) + "行實際執行之對策不能為空</br>");
                            }
                            if (string.IsNullOrEmpty(ReplySev))
                            {
                                isok = false;
                                ErrMsg.Append("第" + (i + 2) + "行改善後嚴重不能為空</br>");

                            }
                            else
                            {
                                int _Sev = int.Parse(ReplySev);
                                if (_Sev >= 1 && _Sev <= 10)
                                {

                                }
                                else
                                {
                                    isok = false;
                                    ErrMsg.Append("第" + (i + 2) + "行嚴重度,發生度,偵測度有誤</br>");
                                }
                            }
                            if (string.IsNullOrEmpty(RepyOcc))
                            {
                                isok = false;
                                ErrMsg.Append("第" + (i + 2) + "行改善後發生率不能為空</br>");
                            }
                            else
                            {
                                int _Occ = int.Parse(RepyOcc);
                                if (_Occ >= 1 && _Occ <= 10)
                                {

                                }
                                else
                                {
                                    isok = false;
                                    ErrMsg.Append("第" + (i + 2) + "行嚴重度,發生度,偵測度有誤</br>");
                                }
                            }
                            if (string.IsNullOrEmpty(RepydDet))
                            {
                                isok = false;
                                ErrMsg.Append("第" + (i + 2) + "行改善後偵測度不能為空</br>");
                            }
                            else
                            {
                                int _Det = int.Parse(RepydDet);
                                if (_Det >= 1 && _Det <= 10)
                                {

                                }
                                else
                                {
                                    isok = false;
                                    ErrMsg.Append("第" + (i + 2) + "行嚴重度,發生度,偵測度有誤</br>");
                                }
                            }
                            if (string.IsNullOrEmpty(Resposibility))
                            {
                                isok = false;
                                ErrMsg.Append("第" + (i + 2) + "行處理狀態不能為空</br>");
                            }
                            if (!isResposibility(Resposibility))
                            {
                                isok = false;
                                ErrMsg.Append("第" + (i + 2) + "行處理狀態有誤</br>");
                            }

                            #endregion
                        }

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            total_num = dt.Rows.Count;
                            string ID = dt.Rows[i]["ID"].ToString();
                            string Resposibility = dt.Rows[i]["Resposibility"].ToString();
                            DateTime CompleteDate = Convert.ToDateTime(dt.Rows[i]["TargetCompletionDate"].ToString());
                            string Taken = dt.Rows[i]["ActionsTaken"].ToString();
                            string ReplySev = dt.Rows[i]["ResultsSev"].ToString();
                            string RepyOcc = dt.Rows[i]["ResultsOcc"].ToString();
                            string RepydDet = dt.Rows[i]["ResultsDet"].ToString();
                            string WriteDept = dt.Rows[i]["WriteDept"].ToString();



                            Model_NPI_FMEA oModel_FMEA = new Model_NPI_FMEA();

                            oModel_FMEA._Resposibility = Resposibility;
                            oModel_FMEA._TargetCompletionDate = CompleteDate;
                            oModel_FMEA._ActionsTaken = Taken;
                            oModel_FMEA._ResultsSev = int.Parse(ReplySev);
                            oModel_FMEA._ResultsOcc = int.Parse(RepyOcc);
                            oModel_FMEA._ResultsDet = int.Parse(RepydDet);
                            oModel_FMEA._ResultsRPN = int.Parse(ReplySev) * int.Parse(RepyOcc) * int.Parse(RepydDet);
                            oModel_FMEA._WriteDept = WriteDept;

                            oModel_FMEA._Status = "Finished";

                            oModel_FMEA._ID = Convert.ToInt32(ID);

                            try
                            {
                                if (isok)
                                {


                                    Dictionary<string, object> result = oStandard.RecordOperation_FMEA(oModel_FMEA, Status_Operation.UPDATE);
                                    if ((bool)result["Result"])
                                    {
                                        ok_num += 1;

                                    }
                                    else
                                    {
                                        ng_num += 1;
                                        Alert((string)result["ErrMsg"].ToString());
                                    }
                                }
                                else
                                {
                                    ng_num += 1;
                                }
                            }
                            catch (Exception ex)
                            {
                                Alert(ex.ToString());

                            }
                        }

                    }


                }
                catch (Exception ex)
                {
                    Alert("Excel數據有問題,錯誤信息: " + ex.Message);
                    return;
                }

            }
            else
            {
                ErrMsg.Append("文件類型只能為xlsx");
            }
        }
        BindPFMAList();
        RefreshPFMARely();
        txtPFMAField.Reset();
        PFMAUpload.Hidden = true;
        pnlPFMAReply.Hidden = true;
        btnPFMAExport.Hidden = true;
        btnUploadPFMA.Hidden = true;
        Alert(string.Format("上傳筆數:{0}<BR/>成功筆數:{1}<BR/>失敗筆數:{2}<BR/>錯誤信息:<BR/>{3}", total_num.ToString(), ok_num.ToString(), ng_num.ToString(), ErrMsg.ToString()));
    }

    protected void btnFMEABatchUpload_Click(object sender, DirectEventArgs e)
    {
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        StringBuilder ErrMsg = new StringBuilder();
        string file = FMEABatchUpload.FileName;
        int total_num = 0;
        int ok_num = 0;
        int ng_num = 0;
        bool isOk = true;
        if (!FMEABatchUpload.HasFile)
        {
            Alert("請選擇数据文件!");
            return;
        }
        else
        {
            string type = file.Substring(file.LastIndexOf(".") + 1).ToLower();
            if (type == "xlsx")
            {


                //把文件轉成流
                Stream stream = FMEABatchUpload.PostedFile.InputStream;
                if (stream.Length == 8889)
                {
                    Alert("導入的資料表為空,請檢查!");
                    return;
                }
                try
                {

                    DataTable dt = ReadByExcelLibrary_IssueandFMEA(stream);
                    if (dt.Rows.Count > 0)
                    {
                        Model_NPI_FMEA oModel_FMEA = new Model_NPI_FMEA();
                        oModel_FMEA._SubNo = txtSub_No.Text.Trim();

                        oModel_FMEA._Status = "Write";

                        string WriteDept = oStandard.GetUserDept("A", lblLogonId.Text, txtPROD_GROUP.Text);



                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            oModel_FMEA._Loess = Helper.ConvertChinese(dt.Rows[i]["Loss Item"].ToString(), "Big5");


                            if (string.IsNullOrEmpty(dt.Rows[i]["Station"].ToString()))
                            {
                                isOk = false;
                                ErrMsg.Append("第" + (i + 2) + "行工位不能為空</br>");
                            }
                            if (string.IsNullOrEmpty(dt.Rows[i]["WriteDept"].ToString()))
                            {
                                isOk = false;
                                ErrMsg.Append("第" + (i + 2) + "行部門不能為空</br>");
                            }
                            if (string.IsNullOrEmpty(dt.Rows[i]["Issue Description"].ToString()))
                            {
                                isOk = false;
                                ErrMsg.Append("第" + (i + 2) + "行問題描述不能為空</br>");
                            }
                            if (string.IsNullOrEmpty(dt.Rows[i]["PotentialFailureMode"].ToString()))
                            {
                                isOk = false;
                                ErrMsg.Append("第" + (i + 2) + "行潛在失效模式不能為空</br>");
                            }
                            if (string.IsNullOrEmpty(dt.Rows[i]["Loss Item"].ToString()))
                            {
                                isOk = false;
                                ErrMsg.Append("第" + (i + 2) + "行違反損失不能為空</br>");
                            }
                            if (string.IsNullOrEmpty(dt.Rows[i]["Sev"].ToString()))
                            {
                                isOk = false;


                                ErrMsg.Append("第" + (i + 2) + "行嚴重度不能為空</br>");
                            }
                            else
                            {
                                oModel_FMEA._Sev = int.Parse(dt.Rows[i]["Sev"].ToString());

                                if (oModel_FMEA._Sev >= 1 && oModel_FMEA._Sev <= 10)
                                {

                                }
                                else
                                {
                                    isOk = false;
                                    ErrMsg.Append("第" + (i + 2) + "行嚴重度,發生度,偵測度有誤</br>");


                                }
                            }
                            if (string.IsNullOrEmpty(dt.Rows[i]["Occ"].ToString()))
                            {
                                isOk = false;

                                ErrMsg.Append("第" + (i + 2) + "行發生度不能為空</br>");
                            }
                            else
                            {
                                oModel_FMEA._Occ = int.Parse(dt.Rows[i]["Occ"].ToString());
                                if (oModel_FMEA._Occ >= 1 && oModel_FMEA._Occ <= 10)
                                {

                                }
                                else
                                {
                                    isOk = false;
                                    ErrMsg.Append("第" + (i + 2) + "行嚴重度,發生度,偵測度有誤</br>");


                                }
                            }
                            if (string.IsNullOrEmpty(dt.Rows[i]["DET"].ToString()))
                            {
                                isOk = false;

                                ErrMsg.Append("第" + (i + 2) + "行偵測度不能為空</br>");
                            }
                            else
                            {
                                oModel_FMEA._DET = int.Parse(dt.Rows[i]["DET"].ToString());
                                if (oModel_FMEA._DET >= 1 && oModel_FMEA._DET <= 10)
                                {

                                }
                                else
                                {
                                    isOk = false;
                                    ErrMsg.Append("第" + (i + 2) + "行嚴重度,發生度,偵測度有誤</br>");


                                }
                            }

                            if (string.IsNullOrEmpty(dt.Rows[i]["Suggestion Action"].ToString()))
                            {
                                isOk = false;
                                ErrMsg.Append("第" + (i + 2) + "行建議改善對策不能為空</br>");
                            }
                            if (!WriteDept.Contains(dt.Rows[i]["WriteDept"].ToString()))
                            {
                                isOk = false;
                                ErrMsg.Append("第" + (i + 2) + "行部門有誤</br>");

                            }
                            if (!isLossItem(oModel_FMEA._Loess))
                            {
                                isOk = false;
                                ErrMsg.Append("第" + (i + 2) + "行違反損失有誤</br>");

                            }


                        }
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            total_num = dt.Rows.Count;
                            try
                            {







                                if (isOk)
                                {
                                    string PrefixString = string.Empty;
                                    string Dept = oStandard.GetUserDept("A", lblLogonId.Text, txtPROD_GROUP.Text);
                                    string Number = string.Empty;
                                    string Item = string.Empty;
                                    if (dt.Rows[i]["Station"].ToString().Length > 0)
                                    {
                                        Number = GetItemsNumber(dt.Rows[i]["Station"].ToString());
                                        PrefixString = dt.Rows[i]["Station"].ToString();
                                        Item = PrefixString + "-" + Dept + Number;
                                    }
                                    oModel_FMEA._Item = Item;
                                    oModel_FMEA._Source = dt.Rows[i]["Issue Description"].ToString();
                                    oModel_FMEA._Stantion = dt.Rows[i]["Station"].ToString();
                                    oModel_FMEA._PotentialFailureMode = dt.Rows[i]["PotentialFailureMode"].ToString();
                                    oModel_FMEA._Loess = Helper.ConvertChinese(dt.Rows[i]["Loss Item"].ToString(), "Big5");
                                    oModel_FMEA._Sev = int.Parse(dt.Rows[i]["Sev"].ToString());
                                    oModel_FMEA._Occ = int.Parse(dt.Rows[i]["Occ"].ToString());

                                    oModel_FMEA._DET = int.Parse(dt.Rows[i]["DET"].ToString());
                                    oModel_FMEA._RPN = int.Parse(dt.Rows[i]["Sev"].ToString()) * int.Parse(dt.Rows[i]["Occ"].ToString()) * int.Parse(dt.Rows[i]["DET"].ToString());
                                    oModel_FMEA._PotentialFailure = dt.Rows[i]["Suggestion Action"].ToString();
                                    oModel_FMEA._WriteDept = dt.Rows[i]["WriteDept"].ToString();
                                    Dictionary<string, object> result = oStandard.RecordOperation_FMEA(oModel_FMEA, Status_Operation.ADD);

                                    if ((bool)result["Result"])
                                    {
                                        ok_num += 1;

                                    }
                                    else
                                    {

                                        ng_num += 1;
                                        ErrMsg.Append(result["Result"].ToString() + "<br/>");
                                    }
                                }
                                else
                                {
                                    ng_num += 1;

                                }


                            }








                            catch (Exception ex)
                            {
                                Alert(ex.ToString());
                            }

                        }

                    }
                }
                catch (Exception ex)
                {
                    Alert("Excel數據有問題,錯誤信息: " + ex.Message);
                    return;
                }

            }
            else
            {
                Alert("文件類型只能為xlsx");
            }
        }
        BindPFMAList();
        RefreshPFMAWrite();
        FMEABatchUpload.Reset();
        rdFMEABatchY.Checked = false;
        rdFMEABatchN.Checked = false;
        Alert(string.Format("上傳筆數:{0}<BR/>成功筆數:{1}<BR/>失敗筆數:{2}<BR/>錯誤信息:<BR/>{3}", total_num.ToString(), ok_num.ToString(), ng_num.ToString(), ErrMsg.ToString()));

    }

    protected void btnFModify_Click(object sender, DirectEventArgs e)
    {
        btnPFMAAdd.Hidden = true;
        btnFModifySave.Hidden = false;
        rdBatchIssueY.Checked = false;
        rdFMEABatchY.Checked = false;
        rdFMEABatchN.Checked = true;
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        RowSelectionModel sm = this.grdPFMAList.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count <= 0)
        {
            Alert("请选择须更新修改项!");
            return;
        }
        string json = e.ExtraParams["Values"];
        Dictionary<string, string>[] companies = JSON.Deserialize<Dictionary<string, string>[]>(json);
        foreach (Dictionary<string, string> row in companies)
        {

            txtFMEAItem.Text = row["Item"].ToString();
            txtFMEASource.Text = row["Source"].ToString();
            txtFMEAStation.Text = row["Stantion"].ToString();
            cbFMEALosseItem.SelectedItem.Text = row["Loess"].ToString();
            cmbPFMADept.SelectedItem.Text = row["WriteDept"].ToString();
            txtFMEASev.Text = row["Sev"].ToString();
            txtFMEAOcc.Text = row["Occ"].ToString();
            txtFMEADet.Text = row["DET"].ToString();
            txtFMEAPRN.Text = row["RPN"].ToString();
            txtFMEAMode.Text = row["PotentialFailureMode"].ToString();
            txtFMEACauses.Text = row["PotentialFailure"].ToString();
            hidPFMEAID.Text = row["ID"].ToString();

        }
    }

    protected void btnFModifySave_Click(object sender, DirectEventArgs e)
    {
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        StringBuilder ErrMsg = new StringBuilder();
        SpmMaster _Master = (SpmMaster)Master;
        string Model = txtModel_R.Text.Trim();
        string Stage = sbPhase.SelectedItem.Text;
        string MFILE_NAME = string.Empty;
        string MFILE_PATH = string.Empty;
        if (FMEAUploadFile.PostedFile.FileName.ToString().Length > 0)
        {
            int indexAttachment = FMEAUploadFile.FileName.LastIndexOf('.');
            string extAttachment = FMEAUploadFile.FileName.Substring(indexAttachment + 1);
            MFILE_NAME = FMEAUploadFile.FileName.Substring(FMEAUploadFile.FileName.LastIndexOf("\\") + 1);
            FileInfo FlInf = new FileInfo(FMEAUploadFile.PostedFile.FileName);
            string File_Ext = FlInf.Extension.ToLower();
            String FileTypes = System.Configuration.ConfigurationSettings.AppSettings["UploadFilesTypes"];
            String[] FileType = FileTypes.Split('|');
            bool FileType_Flag = false;
            foreach (String aType in FileType)
            {
                if (File_Ext.Equals("." + aType))
                {
                    FileType_Flag = true;
                    break;
                }
            }
            if (!FileType_Flag)
            {
                Alert("False只能上傳后綴名為 " + FileTypes + "的文件 .");
                return;
            }
            string type = "FMEA";
            string CaseID = _Master.IFormURLPara.CaseId.ToString();
            string docNoPath = Server.MapPath("~/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + type + " DOC");

            string filepath = (docNoPath + "/" + MFILE_NAME);
            bool IsDocNoExist = Directory.Exists(docNoPath);
            MFILE_PATH = "Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + type + " DOC" + "/" + MFILE_NAME;
            if (!IsDocNoExist)
            {
                Directory.CreateDirectory(docNoPath);
            }

            try
            {

                FMEAUploadFile.PostedFile.SaveAs(docNoPath + "/" + MFILE_NAME);

            }
            catch (Exception ee)
            {
                Alert("False保存文件失败：" + ee.Message + " , " + filepath);
                return;
            }
        }
        Model_NPI_FMEA oModel_FMEA = new Model_NPI_FMEA();
        oModel_FMEA._FILENAME = MFILE_NAME;
        oModel_FMEA._FILEPATH = MFILE_PATH;

        //string Item = txtFMEAItem.Text.Trim();
        //string Status = string.Empty;
        //string Station = txtFMEAStation.Text.Trim();
        string Source = txtFMEASource.Text.Trim();
        string Mode = txtFMEAMode.Text.Trim();
        string LosseItem = cbFMEALosseItem.SelectedItem.Text.Trim();
        string Sev = txtFMEASev.Text.Trim();
        string Occ = txtFMEAOcc.Text.Trim();
        string Det = txtFMEADet.Text.Trim();
        //string PRN = txtFMEAPRN.Text.Trim();
        string Causes = txtFMEACauses.Text.Trim();
        try
        {
            string sql = "UPDATE TB_NPI_FMEA SET FILE_PATH=@file_path,FILE_NAME=@file_name,PotentialFailure=@PotentialFailure,Source=@Source,PotentialFailureMode=@PotentialFailureMode,Loess=@Loess,Sev=@Sev,Occ=@Occ,DET=@DET WHERE ID=@id";

            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@Source", DbType.String, Source));
            opc.Add(DataPara.CreateDataParameter("@PotentialFailureMode", DbType.String, Mode));
            opc.Add(DataPara.CreateDataParameter("@Loess", DbType.String, LosseItem));
            opc.Add(DataPara.CreateDataParameter("@Sev", DbType.String, Sev));
            opc.Add(DataPara.CreateDataParameter("@Occ", DbType.String, Occ));
            opc.Add(DataPara.CreateDataParameter("@DET", DbType.String, Det));
            opc.Add(DataPara.CreateDataParameter("@PotentialFailure", DbType.String, Causes));


            opc.Add(DataPara.CreateDataParameter("@file_path", DbType.String, MFILE_PATH));
            opc.Add(DataPara.CreateDataParameter("@file_name", DbType.String, MFILE_NAME));
            opc.Add(DataPara.CreateDataParameter("@id", DbType.String, hidPFMEAID.Text));
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            sdb.TransactionExecuteScalar(sql, opc);
            BindPFMAList();
            RefreshIssuesInfo();
            btnPFMAAdd.Hidden = false;
            btnFModifySave.Hidden = true;
            rdFMEABatchY.Checked = false;
            rdFMEABatchN.Checked = false;

        }
        //try
        //{
        //    string sql = "UPDATE TB_NPI_FMEA SET FILE_PATH=@file_path,FILE_NAME=@file_name WHERE ID=@id";
        //    opc.Clear();
        //    opc.Add(DataPara.CreateDataParameter("@file_path", DbType.String, MFILE_PATH));
        //    opc.Add(DataPara.CreateDataParameter("@file_name", DbType.String, MFILE_NAME));
        //    opc.Add(DataPara.CreateDataParameter("@id", DbType.String,hidPFMEAID.Text));
        //    SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        //    sdb.TransactionExecuteScalar(sql, opc);
        //    BindPFMAList();
        //    RefreshIssuesInfo();
        //    btnPFMAAdd.Hidden = false;
        //    btnFModifySave.Hidden = true;
        //    rdFMEABatchY.Checked = false;
        //    rdFMEABatchN.Checked = false;

        //}
        catch (Exception ex)
        {
            Alert(ex.ToString());

        }
    }

    protected void rgFMEABatch_Changed(object sender, DirectEventArgs e)
    {
        if (rdFMEABatchY.Checked)
        {
            ContainerFBatch.Hidden = false;
            pnlFMEAWrite.Hidden = true;

        }
        else if (rdFMEABatchN.Checked)
        {
            ContainerFBatch.Hidden = true;
            pnlFMEAWrite.Hidden = false;
        }
        else
        {
            ContainerFBatch.Hidden = true;
            pnlFMEAWrite.Hidden = true;
        }
    }
    #endregion

    #region  PR Attachment
    protected void btnConfirm1_Click(object sender, DirectEventArgs e)
    {
        string dept = cobDept.SelectedItem.Text.Trim();
        if (dept.Length == 0)
        {
            Alert("所屬部門不能為空！");
            return;
        }
        //FileUpload(FileAttachment, "Attach", "Attachment", grdAttachement);
        SpmMaster _Master = (SpmMaster)Master;
        string Model = txtModel_R.Text;
        string Stage = sbPhase.SelectedItem.Text;
        string DOC_NO = txtDOC_NO.Text.Trim();
        string MFILE_PATH = string.Empty;
        string MFILE_NAME = string.Empty;
        string APPROVER = string.Empty;
        string ID = txtPRID.Text == "" ? "0" : txtPRID.Text;
        int PRID = int.Parse(ID);
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));

        if (!FileAttachment.HasFile)
        {
            Alert("請選擇上傳附件!");
            return;
        }


        int indexMeeting = FileAttachment.FileName.LastIndexOf('.');
        string extMeeting = FileAttachment.FileName.Substring(indexMeeting + 1);
        string fileName = FileAttachment.FileName.Substring(FileAttachment.FileName.LastIndexOf("\\") + 1);
        if (extMeeting != "xlsx")
        {
            Alert("附件類型只能為xlsx類型!");
            return;
        }

        //創建保存的目錄 根據主單號與子單號
        string type = "MFG CTQ";
        string CaseID = _Master.IFormURLPara.CaseId.ToString();
        string docNoPath = Server.MapPath("~/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + type + " Doc" + "/" + dept + " Doc");
        string filepath = (docNoPath + "/" + MFILE_NAME);
        bool IsDocNoExist = Directory.Exists(docNoPath);

        bool IsSubDocNoExist = Directory.Exists(docNoPath);
        if (!IsSubDocNoExist)
        {
            Directory.CreateDirectory(docNoPath);
        }

        #region 文件上傳操作
        try
        {
            MFILE_PATH = "Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + type + " Doc" + "/" + dept + " Doc" + "/" + fileName;
            MFILE_NAME = fileName;



            opc.Clear();
            if (_Master.IFormURLPara.StepName == "Dept.Prepared")
            {
                opc.Add(DataPara.CreateDataParameter("@P_OP_TYPE", DbType.String, "ADD", ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@PRID", DbType.Int32, PRID, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@DOC_NO", DbType.String, DOC_NO, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@DEPT", DbType.String, dept, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@CASE_ID", DbType.Int32, _Master.IFormURLPara.CaseId, ParameterDirection.Input, 8));
                opc.Add(DataPara.CreateDataParameter("@MFILE_PATH", DbType.String, MFILE_PATH, ParameterDirection.Input, 2000));
                opc.Add(DataPara.CreateDataParameter("@MFILE_NAME", DbType.String, Helper.ConvertChinese(MFILE_NAME, "Big5"), ParameterDirection.Input, 100));
                opc.Add(DataPara.CreateDataParameter("@FILE_REMARK", DbType.String, Helper.ConvertChinese(txtAttachRemark.Text, "Big5"), ParameterDirection.Input, 2000));
                opc.Add(DataPara.CreateDataParameter("@UPLOAD_TIME", DbType.DateTime, DateTime.Now, ParameterDirection.Input, 8));
                opc.Add(DataPara.CreateDataParameter("@UPLOADUSER", DbType.String, lblLogonId.Text, ParameterDirection.Input, 50));
                //opc.Add(DataPara.CreateDataParameter("@UPDATEUSER", DbType.String, "", ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@Result", DbType.String, null, ParameterDirection.Output, 1000));
            }
            else
            {
                string filepath1 = (docNoPath + "/" + txtFileName.Text);
                bool IsDocNoExist1 = File.Exists(@filepath1);
                if (IsDocNoExist1)
                {
                    File.Delete(@filepath1);
                }

                opc.Add(DataPara.CreateDataParameter("@P_OP_TYPE", DbType.String, "UPDATE", ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@PRID", DbType.Int32, PRID, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@DOC_NO", DbType.String, DOC_NO, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@DEPT", DbType.String, dept, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@CASE_ID", DbType.Int32, _Master.IFormURLPara.CaseId, ParameterDirection.Input, 8));
                opc.Add(DataPara.CreateDataParameter("@MFILE_PATH", DbType.String, MFILE_PATH, ParameterDirection.Input, 2000));
                opc.Add(DataPara.CreateDataParameter("@MFILE_NAME", DbType.String, Helper.ConvertChinese(MFILE_NAME, "Big5"), ParameterDirection.Input, 100));
                opc.Add(DataPara.CreateDataParameter("@FILE_REMARK", DbType.String, Helper.ConvertChinese(txtAttachRemark.Text, "Big5"), ParameterDirection.Input, 2000));
                opc.Add(DataPara.CreateDataParameter("@UPLOAD_TIME", DbType.DateTime, DateTime.Now, ParameterDirection.Input, 8));
                opc.Add(DataPara.CreateDataParameter("@UPLOADUSER", DbType.String, lblLogonId.Text, ParameterDirection.Input, 50));
                //opc.Add(DataPara.CreateDataParameter("@UPDATEUSER", DbType.String, lblLogonId.Text, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@Result", DbType.String, null, ParameterDirection.Output, 1000));

            }
            //SP執行結果返回固定格式
            //1 OK; 
            //2 NG;ERR MSG
            string tmp = sdb.ExecuteProcScalar("P_Upload_NPI_Files_PR", opc, "@Result");
            fileMeeting.PostedFile.SaveAs(docNoPath + "/" + fileName);
            if (tmp.Length >= 3)
            {

                if (tmp.Substring(0, 2) == "NG")
                {

                    Alert("文件上傳失敗!" + tmp.Substring(3, tmp.Length - 3));
                    return;
                }

            }
            else
            {
                DeleteExistFiles(docNoPath);
                Alert("文件上傳失敗!DB ERROR,Pls contact IT");
                return;
            }

        }
        catch (Exception ex)
        {
            DeleteExistFiles(docNoPath);
            Alert("文件上傳失敗!DB ERROR:" + ex.Message);
            return;
        }

        #endregion
        Alert("文件上傳成功！");
        if (_Master.IFormURLPara.StepName == "Dept.Prepared")
        {
            BindAttachmentList_PR(CaseID, dept);
        }
        else
        {
            BindAttachmentList_PR(CaseID);
        }
    }

    private void DeleteFiles(string FilePath)
    {
        bool IsExist = Directory.Exists(FilePath);
        if (IsExist)
        {

            DirectoryInfo directory = new DirectoryInfo(FilePath);
            FileInfo[] files = directory.GetFiles();
            foreach (FileInfo f in files)
            {
                f.Delete();
            }
        }
    }

    protected void btnDelAttachement_click(object sender, DirectEventArgs e)
    {
        string Model = txtModel_R.Text;
        string Stage = sbPhase.SelectedItem.Text;
        string dept = cobDept.SelectedItem.Text.Trim();

        RowSelectionModel sm = grdAttachement.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count == 0)
        {
            Alert("请选择需刪除記錄!");
            return;
        }
        string json = e.ExtraParams["values"].ToString();
        Dictionary<string, string>[] data = JSON.Deserialize<Dictionary<string, string>[]>(json);
        foreach (Dictionary<string, string> rows in data)
        {
            Model_NPI_APP_ATTACHFILE oModel_NPI_Attachement = new Model_NPI_APP_ATTACHFILE();
            int CASEID = int.Parse(rows["CASEID"].ToString());
            int ID = int.Parse(rows["ID"].ToString());
            string MFILE_NAME = rows["FILE_NAME"].ToString();
            NPIMgmt oNPI_Mgmt = new NPIMgmt(lblSite.Text.Trim(), lblBu.Text.Trim());
            NPI_Standard oNPI_Standard = oNPI_Mgmt.InitialLeaveMgmt();

            try
            {
                SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
                Dictionary<string, object> result = new Dictionary<string, object>();
                StringBuilder sb = new StringBuilder();
                sb.Append(@"delete from TB_NPI_APP_PR_ATTACHFILE where ID=@ID AND CASEID=@CASEID");
                opc.Clear();
                opc.Add(DataPara.CreateDataParameter("@ID", DbType.Int32, ID));
                opc.Add(DataPara.CreateDataParameter("@CASEID", DbType.Int32, CASEID));
                try
                {
                    sdb.TransactionExecuteScalar(sb.ToString(), opc);
                    string type = "MFG CTQ";
                    string docNoPath = Server.MapPath("~/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + type + " Doc" + "/" + dept + " Doc");
                    string filepath = (docNoPath + "/" + MFILE_NAME);
                    bool IsDocNoExist = File.Exists(@filepath);
                    if (IsDocNoExist)
                    {
                        File.Delete(@filepath);
                    }
                    DataTable dt = oNPI_Standard.Get_NPI_AttachmentPR(int.Parse(rows["CASEID"].ToString()), cobDept.SelectedItem.Text);
                    BindGrid(grdAttachement, dt);

                }
                catch (Exception ex)
                {

                    Alert(ex.Message);
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
    }

    protected void grdAttachement_RowCommand(object sender, DirectEventArgs e)
    {
        string Dept = GetUserDept("B", lblLogonId.Text, txtPROD_GROUP.Text);
        cobDept.Text = e.ExtraParams["DEPT"].ToString();
        if (Dept.Replace(" ", "") != cobDept.Text)
        {
            Alert("無法編輯其他部門的資料！");
            cobDept.Text = string.Empty;
            txtAttachRemark.Text = string.Empty;
            txtPRID.Text = string.Empty;
            txtFileName.Text = string.Empty;
            return;
        }
        else
        {
            pnlAttach.Hidden = false;
            cobDept.ReadOnly = true;
            txtAttachRemark.Text = e.ExtraParams["FILE_REMARK"].ToString();
            txtPRID.Text = e.ExtraParams["ID"].ToString();
            txtFileName.Text = e.ExtraParams["FILE_NAME"].ToString();
        }
    }


    #endregion

    #region IssuesList 上傳 PM Approver關卡
    protected void btnConfirm_Click_Issues(object sender, DirectEventArgs e)
    {
        SpmMaster _Master = (SpmMaster)Master;
        string Model = txtModel_R.Text;
        string Stage = sbPhase.SelectedItem.Text;
        string DOC_NO = txtDOC_NO.Text.Trim();
        string MFILE_PATH = string.Empty;
        string MFILE_NAME = string.Empty;
        string APPROVER = string.Empty;
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));

        if (!fileMeeting.HasFile)
        {
            Alert("請選擇上傳附件!");
            return;
        }

        int indexMeeting = fileMeeting.FileName.LastIndexOf('.');
        string extMeeting = fileMeeting.FileName.Substring(indexMeeting + 1);
        string fileName = fileMeeting.FileName.Substring(fileMeeting.FileName.LastIndexOf("\\") + 1);
        if (extMeeting != "xlsx")
        {
            Alert("附件類型只能為xlsx類型!");
            return;
        }


        //創建保存的目錄 根據主單號與子單號
        string type = "Issue List";
        string CaseID = _Master.IFormURLPara.CaseId.ToString();
        string docNoPath = Server.MapPath("~/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + type + " Doc" + "/" + "Issue PM");
        string filepath = (docNoPath + "/" + MFILE_NAME);
        bool IsDocNoExist = Directory.Exists(docNoPath);

        bool IsSubDocNoExist = Directory.Exists(docNoPath);
        if (!IsSubDocNoExist)
        {
            Directory.CreateDirectory(docNoPath);
        }

        #region 文件上傳操作
        try
        {
            MFILE_PATH = "Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + type + " Doc" + "/" + "Issue PM" + "/" + fileName;
            MFILE_NAME = fileName;

            fileMeeting.PostedFile.SaveAs(docNoPath + "/" + fileName);

            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@DOC_NO", DbType.String, DOC_NO, ParameterDirection.Input, 30));
            opc.Add(DataPara.CreateDataParameter("@CASE_ID", DbType.Int32, _Master.IFormURLPara.CaseId, ParameterDirection.Input, 8));
            opc.Add(DataPara.CreateDataParameter("@MFILE_PATH", DbType.String, MFILE_PATH, ParameterDirection.Input, 255));
            opc.Add(DataPara.CreateDataParameter("@MFILE_NAME", DbType.String, Helper.ConvertChinese(MFILE_NAME, "Big5"), ParameterDirection.Input, 50));
            opc.Add(DataPara.CreateDataParameter("@APPROVER", DbType.String, _Master.IFormURLPara.StepName, ParameterDirection.Input, 50));
            opc.Add(DataPara.CreateDataParameter("@UPDATE_TIME", DbType.DateTime, DateTime.Now, ParameterDirection.Input, 8));
            opc.Add(DataPara.CreateDataParameter("@UPDATE_USERID", DbType.String, lblLogonId.Text, ParameterDirection.Input, 20));
            opc.Add(DataPara.CreateDataParameter("@Result", DbType.String, null, ParameterDirection.Output, 1000));
            //SP執行結果返回固定格式
            //1 OK; 
            //2 NG;ERR MSG
            string tmp = sdb.ExecuteProcScalar("P_Upload_NPI_Files_Issue", opc, "@Result");
            if (tmp.Length >= 3)
            {

                if (tmp.Substring(0, 2) == "NG")
                {

                    Alert("文件上傳失敗!" + tmp.Substring(3, tmp.Length - 3));
                    return;
                }

            }
            else
            {
                DeleteExistFiles(docNoPath);
                Alert("文件上傳失敗!DB ERROR,Pls contact IT");
                return;
            }

        }
        catch (Exception ex)
        {
            DeleteExistFiles(docNoPath);
            Alert("文件上傳失敗!DB ERROR:" + ex.Message);
            return;
        }

        #endregion
        Alert("文件上傳成功！");
        BindAttachmentList(CaseID);

    }

    protected void btnDelete_ClickVQM(object sender, DirectEventArgs e) //附件删除VQM
    {
        SpmMaster _Master = (SpmMaster)Master;
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        string Model = txtModel_R.Text;
        string Stage = sbPhase.SelectedItem.Text;
        string subdoc = string.Empty;
        string filename = string.Empty;
        string CaseID = _Master.IFormURLPara.CaseId.ToString();
        string NCRNO = txtDOC_NO.Text.Trim();
        RowSelectionModel sm = grdAttachmentIssue.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count <= 0)
        {
            Alert("請勾選需刪除的信息");
            return;
        }
        string json = e.ExtraParams["Values"];
        Dictionary<string, string>[] list = JSON.Deserialize<Dictionary<string, string>[]>(json);
        StringBuilder ErrMsg = new StringBuilder();
        foreach (Dictionary<string, string> row in list)
        {
            subdoc = row["SUB_DOC_NO"].ToString();
            filename = row["FILE_NAME"].ToString();
            try
            {
                string sql = "DELETE FROM TB_NPI_APP_ISSUELIST_ATTACHFILE WHERE SUB_DOC_NO=@subdoc"
                     + " and FILE_NAME=@fileName";
                opc.Clear();
                opc.Add(DataPara.CreateDataParameter("@subdoc", DbType.String, subdoc));
                opc.Add(DataPara.CreateDataParameter("@fileName", DbType.String, filename));
                sdb.TransactionExecuteNonQuery(sql, opc);

                string type = "Issue List";
                string sub_docNoPath = Server.MapPath("~/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + type + " Doc" + "/" + "Issue PM" + filename);
                bool IsSubDocNoExist = Directory.Exists(sub_docNoPath);
                if (IsSubDocNoExist)
                {
                    DeleteExistFiles(sub_docNoPath);
                }
            }
            catch (Exception ex)
            {
                ErrMsg.Append(string.Format("類別:{0},文件名稱:{1} 刪除作業失敗!ErrMsg:{2}<BR/>", filename, ex.Message));

            }

        }
        if (ErrMsg.Length > 0)
        {
            Alert(ErrMsg.ToString());
        }
        else
        {
            Alert("刪除作業成功!");
            BindAttachmentList(CaseID);

        }

    }

    private static void DeleteExistFiles(string sub_docNoPath)
    {
        DirectoryInfo directory = new DirectoryInfo(sub_docNoPath);
        FileInfo[] files = directory.GetFiles();
        foreach (FileInfo file in files)
        {
            file.Delete();
        }
    }

    private void BindAttachmentList(string CaseId)//Issuelist
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        string sql = "select * from TB_NPI_APP_ISSUELIST_ATTACHFILE where CASEID=@CaseID";
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@CaseID", DbType.String, CaseId));
        DataTable dtAttachfile = sdb.TransactionExecute(sql, opc);
        grdAttachmentIssue.Store.Primary.DataSource = dtAttachfile;
        grdAttachmentIssue.Store.Primary.DataBind();

    }

    private void BindAttachmentList_PR(string CaseId, string Dept)//PR
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        string sql = "select * from TB_NPI_APP_PR_ATTACHFILE where CASEID=@CaseID and DEPT=@DEPT";
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@CaseID", DbType.String, CaseId));
        opc.Add(DataPara.CreateDataParameter("@DEPT", DbType.String, Dept));
        DataTable dtAttachfile = sdb.TransactionExecute(sql, opc);
        grdAttachement.Store.Primary.DataSource = dtAttachfile;
        grdAttachement.Store.Primary.DataBind();

    }

    private void BindAttachmentList_PR(string CaseId)//PR
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        string sql = "select * from TB_NPI_APP_PR_ATTACHFILE where CASEID=@CaseID ";
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@CaseID", DbType.String, CaseId));
        DataTable dtAttachfile = sdb.TransactionExecute(sql, opc);
        grdAttachement.Store.Primary.DataSource = dtAttachfile;
        grdAttachement.Store.Primary.DataBind();

    }

    #endregion

    #region Modify 上傳 PM Approver關卡
    protected void btnConfirm_Click_Modify(object sender, DirectEventArgs e)
    {
        SpmMaster _Master = (SpmMaster)Master;
        string Model = txtModel_R.Text;
        string Stage = sbPhase.SelectedItem.Text;
        string DOC_NO = txtDOC_NO.Text.Trim();
        string MFILE_PATH = string.Empty;
        string MFILE_NAME = string.Empty;
        string APPROVER = string.Empty;
        string dept = "PM";
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));

        if (!fileMeetingmodify.HasFile)
        {
            Alert("請選擇上傳附件!");
            return;
        }

        int indexMeeting = fileMeetingmodify.FileName.LastIndexOf('.');
        string extMeeting = fileMeetingmodify.FileName.Substring(indexMeeting + 1);
        string fileName = fileMeetingmodify.FileName.Substring(fileMeetingmodify.FileName.LastIndexOf("\\") + 1);
        //if (extMeeting != "pdf")
        //{
        //    Alert("附件類型只能為pdf類型!");
        //    return;
        //}


        //創建保存的目錄 根據主單號與子單號
        string type = "MFG CTQ";
        string CaseID = _Master.IFormURLPara.CaseId.ToString();
        string docNoPath = Server.MapPath("~/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + type + " Doc" + "/" + dept + " Doc");
        string filepath = (docNoPath + "/" + MFILE_NAME);
        bool IsDocNoExist = Directory.Exists(docNoPath);

        bool IsSubDocNoExist = Directory.Exists(docNoPath);
        if (!IsSubDocNoExist)
        {
            Directory.CreateDirectory(docNoPath);
        }

        #region 文件上傳操作
        try
        {
            MFILE_PATH = "Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + type + " Doc" + "/" + dept + " Doc" + "/" + fileName;
            MFILE_NAME = fileName;

            fileMeetingmodify.PostedFile.SaveAs(docNoPath + "/" + fileName);

            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@DOC_NO", DbType.String, DOC_NO, ParameterDirection.Input, 30));
            opc.Add(DataPara.CreateDataParameter("@CASE_ID", DbType.Int32, _Master.IFormURLPara.CaseId, ParameterDirection.Input, 8));
            opc.Add(DataPara.CreateDataParameter("@MFILE_PATH", DbType.String, MFILE_PATH, ParameterDirection.Input, 255));
            opc.Add(DataPara.CreateDataParameter("@MFILE_NAME", DbType.String, Helper.ConvertChinese(MFILE_NAME, "Big5"), ParameterDirection.Input, 50));
            opc.Add(DataPara.CreateDataParameter("@APPROVER", DbType.String, _Master.IFormURLPara.StepName, ParameterDirection.Input, 50));
            opc.Add(DataPara.CreateDataParameter("@UPDATE_TIME", DbType.DateTime, DateTime.Now, ParameterDirection.Input, 8));
            opc.Add(DataPara.CreateDataParameter("@UPDATE_USERID", DbType.String, lblLogonId.Text, ParameterDirection.Input, 20));
            opc.Add(DataPara.CreateDataParameter("@Result", DbType.String, null, ParameterDirection.Output, 1000));
            //SP執行結果返回固定格式
            //1 OK; 
            //2 NG;ERR MSG
            string tmp = sdb.ExecuteProcScalar("P_Upload_NPI_Files_Modify", opc, "@Result");
            fileMeetingmodify.PostedFile.SaveAs(docNoPath + "/" + fileName);
            if (tmp.Length >= 3)
            {

                if (tmp.Substring(0, 2) == "NG")
                {

                    Alert("文件上傳失敗!" + tmp.Substring(3, tmp.Length - 3));
                    return;
                }

            }
            else
            {
                DeleteExistFiles(docNoPath);
                Alert("文件上傳失敗!DB ERROR,Pls contact IT");
                return;
            }

        }
        catch (Exception ex)
        {
            DeleteExistFiles(docNoPath);
            Alert("文件上傳失敗!DB ERROR:" + ex.Message);
            return;
        }

        #endregion
        Alert("文件上傳成功！");
        BindAttachmentListModify(CaseID);

    }

    protected void btnDelete_ClickModify(object sender, DirectEventArgs e) //附件删除VQM
    {
        SpmMaster _Master = (SpmMaster)Master;
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        string Model = txtModel_R.Text;
        string Stage = sbPhase.SelectedItem.Text;
        string subdoc = string.Empty;
        string filename = string.Empty;
        string dept = "PM";
        string CaseID = _Master.IFormURLPara.CaseId.ToString();
        RowSelectionModel sm = grdAttachmentModify.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count <= 0)
        {
            Alert("請勾選需刪除的信息");
            return;
        }
        string json = e.ExtraParams["Values"];
        Dictionary<string, string>[] list = JSON.Deserialize<Dictionary<string, string>[]>(json);
        StringBuilder ErrMsg = new StringBuilder();
        foreach (Dictionary<string, string> row in list)
        {
            subdoc = row["SUB_DOC_NO"].ToString();
            filename = row["FILE_NAME"].ToString();
            try
            {
                string sql = "DELETE FROM TB_NPI_APP_ISSUELIST_ATTACHFILE WHERE SUB_DOC_NO=@subdoc"
                     + " and FILE_NAME=@fileName";
                opc.Clear();
                opc.Add(DataPara.CreateDataParameter("@subdoc", DbType.String, subdoc));
                opc.Add(DataPara.CreateDataParameter("@fileName", DbType.String, filename));
                sdb.TransactionExecuteNonQuery(sql, opc);

                string type = "MFG CTQ";
                string docNoPath = Server.MapPath("~/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + type + " Doc" + "/" + dept + " Doc");
                string filepath = (docNoPath + "/" + filename);
                bool IsDocNoExist = File.Exists(@filepath);
                if (IsDocNoExist)
                {
                    File.Delete(@filepath);
                }
            }
            catch (Exception ex)
            {
                ErrMsg.Append(string.Format("類別:{0},文件名稱:{1} 刪除作業失敗!ErrMsg:{2}<BR/>", filename, ex.Message));

            }

        }
        if (ErrMsg.Length > 0)
        {
            Alert(ErrMsg.ToString());
        }
        else
        {
            Alert("刪除作業成功!");
            BindAttachmentListModify(CaseID);

        }

    }

    private void BindAttachmentListModify(string CaseId)//Issuelist
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        string sql = "select * from TB_NPI_APP_ISSUELIST_ATTACHFILE where CASEID=@CaseID";
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@CaseID", DbType.String, CaseId));
        DataTable dtAttachfile = sdb.TransactionExecute(sql, opc);
        grdAttachmentModify.Store.Primary.DataSource = dtAttachfile;
        grdAttachmentModify.Store.Primary.DataBind();

    }
    #endregion

    //protected void btnResultSave_Click(object sender, DirectEventArgs e)
    //{

    //    SpmMaster _Master = (SpmMaster)Master;
    //    NPIMgmt oNPI_Mgmt = new NPIMgmt(lblSite.Text.Trim(), lblBu.Text.Trim());
    //    NPI_Standard oNPI_Standard = oNPI_Mgmt.InitialLeaveMgmt();
    //    Model_NPI_APP_RESULT oModel_NPI_Result = new Model_NPI_APP_RESULT();
    //    oModel_NPI_Result._APPROVER = _Master.IFormURLPara.LoginId;
    //    oModel_NPI_Result._APPROVER_OPINION = txtReslutOpinion.Text.Trim();
    //    oModel_NPI_Result._SUB_DOC_NO = txtSub_No.Text.Trim();
    //    oModel_NPI_Result._CASEID = _Master.IFormURLPara.CaseId;
    //    oModel_NPI_Result._DEPT = txtReslutDept.Text.Trim();
    //    for (int i = 0; i < rgResult.Items.Count; i++)
    //    {
    //        if (rgResult.Items[i].Checked == true)
    //        {
    //            oModel_NPI_Result._APPROVER_RESULT = rgResult.Items[i].BoxLabel.Trim();
    //        }
    //    }

    //    try
    //    {
    //        Dictionary<string, object> result = new Dictionary<string, object>();
    //        result = oNPI_Standard.RecordOperation_Result(oModel_NPI_Result, Status_Operation.ADD);

    //        if ((bool)result["Result"])
    //        {
    //            DataTable dt = oNPI_Standard.GetNPI_Result(_Master.IFormURLPara.CaseId);
    //            BindGrid(grdResult, dt);
    //        }
    //        else
    //        {
    //            Alert((string)result["ErrMsg"].ToString());
    //        }

    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}

    private DataTable ReadByExcelLibrary(Stream xlsStream)
    {
        DataTable table = new DataTable();
        using (ExcelPackage package = new ExcelPackage(xlsStream))
        {
            ExcelWorksheet sheet = package.Workbook.Worksheets[1];
            int colCount = sheet.Dimension.End.Column;
            int rowCount = sheet.Dimension.End.Row;
            for (ushort j = 1; j <= colCount; j++)
            {
                table.Columns.Add(new DataColumn(sheet.Cells[1, j].Value.ToString()));
            }
            for (ushort i = 2; i <= rowCount; i++)
            {
                DataRow row = table.NewRow();
                for (ushort j = 1; j <= colCount; j++)
                {
                    row[j - 1] = sheet.Cells[i, j].Value;
                }
                if (!string.IsNullOrEmpty(row[0].ToString()))
                {
                    table.Rows.Add(row);
                }
            }
        }
        return table;
    }

    private DataTable ReadByExcelLibrary_IssueandFMEA(Stream xlsStream)
    {
        DataTable table = new DataTable();
        using (ExcelPackage package = new ExcelPackage(xlsStream))
        {
            ExcelWorksheet sheet = package.Workbook.Worksheets[1];
            int colCount = sheet.Dimension.End.Column;
            int rowCount = sheet.Dimension.End.Row;
            for (ushort j = 1; j <= colCount; j++)
            {
                table.Columns.Add(new DataColumn(sheet.Cells[1, j].Value.ToString()));
            }
            for (ushort i = 2; i <= rowCount; i++)
            {
                DataRow row = table.NewRow();
                for (ushort j = 1; j <= colCount; j++)
                {
                    row[j - 1] = sheet.Cells[i, j].Value;
                }
                if (!string.IsNullOrEmpty(row[1].ToString()))
                {
                    table.Rows.Add(row);
                }
            }
        }
        return table;
    }

    #region 上傳附件時的判斷
    public bool isIssueSeverity(string str)
    {
        bool isIssueSeverity = false;

        switch (str)
        {
            case "Critical":
                isIssueSeverity = true;
                break;
            case "Major":
                isIssueSeverity = true;
                break;
            case "Minor":
                isIssueSeverity = true;
                break;

        }
        return isIssueSeverity;
    }
    public bool isLossItem(string str)
    {
        bool isLossItem = false;
        switch (str)
        {
            case "品質損失":
                isLossItem = true;
                break;
            case "成本損失":
                isLossItem = true;
                break;
            case "人工效率損失":
                isLossItem = true;
                break;
            case "設備效率損失":
                isLossItem = true;
                break;

        }
        return isLossItem;
    }
    public bool isCompliance(string str)
    {
        bool isLossItem = false;
        switch (str)
        {
            case "Y":
                isLossItem = true;
                break;
            case "N":
                isLossItem = true;
                break;
            case "NA":
                isLossItem = true;
                break;
        }
        return isLossItem;
    }
    public bool isResposibility(string str)
    {
        bool isLossItem = false;
        switch (str)
        {
            case "OPEN":
                isLossItem = true;
                break;
            case "CLOSED":
                isLossItem = true;
                break;
            case "Tracking":
                isLossItem = true;
                break;
        }
        return isLossItem;
    }
    #endregion
    protected void btnDownLoad_Click(object sender, DirectEventArgs e)
    {

        SpmMaster _Master = (SpmMaster)Master;
        string CaseID = _Master.IFormURLPara.CaseId.ToString();
        //string Status = e.ExtraParams["Status"];
        string DocNo = txtPROD_GROUP.Text.Trim();
        string Model = txtModel_R.Text;
        string Bu = "Power";
        string Building = "A6";
        string floderPath = Server.MapPath("~/Attachment/" + Model + "_NPI_DOC");
        string floderPathZip = Server.MapPath("~/Attachment/" + Model + "_NPI_DOC" + ".zip");
        string floderPathDownLoad = "~/Attachment/" + Model + "_NPI_DOC" + ".zip";
        string fileNameDownLoad = Model + "_NPI_DOC" + ".zip";
        BonkerZip tool = new BonkerZip();
        tool.AddFile(floderPath);
        tool.CompressionZip(floderPathZip);
        X.Js.Call("NPIFileDownload", fileNameDownLoad, floderPathDownLoad);
    }


    /// <summary>
    /// 設定表頁的列寬自適應
    /// </summary>
    /// <param name="sheet"></param>
    private void SetColumnAuto(ref Aspose.Cells.Worksheet sheet)
    {
        Aspose.Cells.Cells cells = sheet.Cells;

        //获取页面最大列数
        int columnCount = cells.MaxColumn + 1;

        //获取页面最大行数
        int rowCount = cells.MaxRow;
        for (int col = 0; col < columnCount; col++)
        {
            sheet.AutoFitColumn(col, 0, rowCount);
        }
        for (int col = 0; col < columnCount; col++)
        {
            int pixel = cells.GetColumnWidthPixel(col) + 10;
            if (pixel > 255)
            {
                cells.SetColumnWidthPixel(col, 255);
            }
            else
            {
                cells.SetColumnWidthPixel(col, pixel);
            }
        }

    }


    /// <summary>
    /// 設定頁面打印格式
    /// </summary>
    /// <param name="sheet">worksheet</param>
    /// <param name="type">列印模式：直印，橫印</param>
    private void SetStyle(ref Aspose.Cells.Worksheet sheet, Aspose.Cells.PageOrientationType type)
    {

        sheet.PageSetup.IsPercentScale = false;
        sheet.PageSetup.FitToPagesWide = 1; //自動縮放為一頁寬
        sheet.PageSetup.LeftMargin = 0.5;
        sheet.PageSetup.RightMargin = 0.5;
        sheet.PageSetup.TopMargin = 0.5;
        sheet.PageSetup.BottomMargin = 0.5;
        sheet.PageSetup.Orientation = type;

    }

    #region 抓取所屬部門
    private DataTable GetUserDept(string Category, string DocNo)
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));

        StringBuilder sbDept = new StringBuilder();
        string sql = "select  DISTINCT DEPT from TB_NPI_APP_MEMBER where 1=1"
             + " AND  (WriteEname =@ENAME OR ReplyEName=@ENAME OR CheckedEname=@ENAME)"

             + " AND CategoryFlag=@Category"
             + " AND DOC_NO=@Doc_No";
        ArrayList opc = new ArrayList();
        opc.Add(DataPara.CreateDataParameter("@ENAME", DbType.String, lblLogonId.Text));
        opc.Add(DataPara.CreateDataParameter("@Category", DbType.String, Category));
        opc.Add(DataPara.CreateDataParameter("@Doc_No", DbType.String, DocNo));
        DataTable dt = sdb.TransactionExecute(sql, opc);
        return dt;

    }

    public string GetUserDept(string Category, string LoginId, string DocNo)
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));

        StringBuilder sbDept = new StringBuilder();
        string sql = "select  DISTINCT DEPT from TB_NPI_APP_MEMBER where 1=1"
             + " AND  (WriteEname =@ENAME OR ReplyEName=@ENAME OR CheckedEname=@ENAME)"

             + " AND CategoryFlag=@Category"
             + " AND DOC_NO=@Doc_No";
        ArrayList opc = new ArrayList();
        opc.Add(DataPara.CreateDataParameter("@ENAME", DbType.String, LoginId));
        opc.Add(DataPara.CreateDataParameter("@Category", DbType.String, Category));
        opc.Add(DataPara.CreateDataParameter("@Doc_No", DbType.String, DocNo));
        DataTable dt = sdb.TransactionExecute(sql, opc);
        foreach (DataRow dr in dt.Rows)
        {
            sbDept.AppendFormat("{0},", dr["DEPT"].ToString());
        }
        return sbDept.ToString().TrimEnd(',');
    }

    private string GetUserDept(string Category, string LoginId, string DocNo, string stepname)
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        ArrayList opc = new ArrayList();
        StringBuilder sbDept = new StringBuilder();
        string sql = "select  DISTINCT DEPT from TB_NPI_APP_MEMBER where 1=1";
        sql += " AND Flag=@Category";
        sql += " AND DOC_NO=@Doc_No";
        switch (stepname)
        {
            case "Dept.Write":
                sql += " AND WriteEname =@ENAME";
                break;
            case "Dept.Reply":
                sql += " AND ReplyEName=@ENAME";
                break;
            default:
                sql += " AND  (WriteEname =@ENAME OR ReplyEName=@ENAME OR CheckedEname=@ENAME)";
                break;
        }
        opc.Add(DataPara.CreateDataParameter("@ENAME", DbType.String, LoginId));
        opc.Add(DataPara.CreateDataParameter("@Category", DbType.String, Category));
        opc.Add(DataPara.CreateDataParameter("@Doc_No", DbType.String, DocNo));

        DataTable dt = sdb.TransactionExecute(sql, opc);
        foreach (DataRow dr in dt.Rows)
        {
            sbDept.AppendFormat("{0},", dr["DEPT"].ToString());
        }
        return sbDept.ToString().TrimEnd(',');
    }
    #endregion

    #region Check工單號
    protected void check_workorder(object sender, DirectEventArgs e)
    {
        //string Text = txtWorkOrder.Text.Trim();
        //if (!Check(Text))
        //{
        //    Alert("工單號只能是7位的數字！");
        //    txtWorkOrder.Text = string.Empty;
        //    return;
        //}
        //else
        //{

        //}
    }

    public bool Check(string s)
    {
        string pattern = "^[0-9]$";
        Regex rx = new Regex(pattern);
        return rx.IsMatch(s);
    }
    #endregion

    #region 根據Building綁定產品類別
    protected void Bind_Product(object sender, DirectEventArgs e)
    {
        sbProd_group.Text = string.Empty;
        BindProduct();
    }

    private void BindProduct()
    {

        string Building = sbBuilding.SelectedItem.Text;
        DataTable data = GetProduct(Building);
        ComboBox[] cbs = new ComboBox[] { sbProd_group };
        foreach (ComboBox cb in cbs)
        {
            BindCombox(data, cb);
        }

    }

    private DataTable GetProduct(string Building)
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        string sql = " select PARAME_VALUE2 FROM TB_APPLICATION_PARAM where  APPLICATION_NAME ='NPI_REPORT' and  FUNCTION_NAME = 'Product' AND PARAME_VALUE1 = @PARAME_VALUE1 ";
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@PARAME_VALUE1", DbType.String, Building));
        DataTable dt = sdb.TransactionExecute(sql, opc);
        return dt;
    }

    private void BindCombox(DataTable dt, ComboBox cb)
    {
        cb.Store.Primary.DataSource = dt;
        cb.Store.Primary.DataBind();
    }
    #endregion

    #region 倒序排序抓取最新的機種
    protected DataTable GetNewestModel(string Model)
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        StringBuilder sb = new StringBuilder();
        sb.Append(@"  select T1.MODEL_NAME,T2.UPDATE_TIME from TB_NPI_APP_MAIN T1 
                      LEFT jOIN TB_NPI_APP_SUB T2 ON T1.DOC_NO = T2.DOC_NO
                      WHERE T2.STATUS = 'Finished' and T1.MODEL_NAME = @ModelName
                      order by UPDATE_TIME desc");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@ModelName", DbType.String, Model));
        DataTable dt = sdb.TransactionExecute(sb.ToString(), opc);
        return dt;
    }
    #endregion
}

