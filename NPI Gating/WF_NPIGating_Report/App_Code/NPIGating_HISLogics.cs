using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.IO;
using System.Text;
using System.Collections.Generic;
using LiteOn.ea.SPM3G;
using LiteOn.ea.SPM3G.UI;
using System.Web;
using Ext.Net;
using LiteOn.EA.NPIReport.Utility;
using Liteon.ICM.DataCore;
using System.Collections;
using System.Linq;
using LiteOn.EA.Borg.Utility;
//

// UI controls holder for Example1 
public class NPIGating_HISUIShadow : IUIShadow
{
    // Declare controls which show in the web page
    #region Beigin

    public Hidden lblSite;
    public Hidden lblBu;
    public Hidden lblBuilding;

    public TextField txtCustomer;
    public TextField txtDate;
    public TextArea txtNotes;
    public ComboBox cmbPhase;
    public ComboBox cmbPlant;
    public ComboBox cbProductType;
    public TextField txtModel;
    public MultiCombo cmbLayout;

    public TextField txtFormNo;
    public TextField  txtPM;
    public TextField  txtRD;
    public TextField  txtSales;

    public ComboBox cmbPM;
    public ComboBox cmbRD;
    public ComboBox cmbSales;
    public NumberField txtPMExt;
    public NumberField txtRDExt;
    public NumberField txtSalesExt;

    public DateField  txtNextStage_BeginDate;
    public Container ConAttachment;

    public Panel pnlCountersign;
    public GridPanel grdResult;
    public Button btnDelete;
    public ComboBox cmbAttachmentType;
    public GridPanel grdAttachment;
    public Panel pnlResult;
    public RadioGroup rgResult;
    public TextArea txtReslutOpinion;
    public Radio rdResultY;
    public Radio rdResultN;
    public Radio rdReulutCondition;

 

    #endregion



    public NPIGating_HISUIShadow(Page oContainer)
        : base(oContainer) { }

    // Initialize controls
    public override void InitShadow(System.Web.UI.WebControls.ContentPlaceHolder oContentPage)
    {
        #region begin

        lblSite = (Hidden)oContentPage.FindControl("lblSite");
        lblBu = (Hidden)oContentPage.FindControl("lblBu");
        lblBuilding = (Hidden)oContentPage.FindControl("lblBuilding");
       

        txtFormNo = (TextField)oContentPage.FindControl("txtFormNo");
        txtCustomer = (TextField)oContentPage.FindControl("txtCustomer");
        txtDate = (TextField)oContentPage.FindControl("txtDate");
        txtNotes = (TextArea)oContentPage.FindControl("txtNotes");

        cmbPhase = (ComboBox)oContentPage.FindControl("cmbPhase");
        cmbPlant = (ComboBox)oContentPage.FindControl("cmbPlant") ;
        cbProductType = (ComboBox)oContentPage.FindControl("cbProductType");
        txtModel = (TextField)oContentPage.FindControl("txtModel"); ;
        cmbLayout = (MultiCombo)oContentPage.FindControl("cmbLayout"); ;

        txtPM=(TextField)oContentPage.FindControl("txtPM");
        txtRD=(TextField)oContentPage.FindControl("txtRD");
        txtSales=(TextField)oContentPage.FindControl("txtSales");
        txtNextStage_BeginDate=(DateField)oContentPage.FindControl("txtNextStage_BeginDate");
        ConAttachment = (Container)oContentPage.FindControl("ConAttachment");


        cmbPM = (ComboBox)oContentPage.FindControl("cmbPM");
        cmbRD = (ComboBox)oContentPage.FindControl("cmbRD");
        cmbSales=(ComboBox)oContentPage.FindControl("cmbSales");
        txtPMExt = (NumberField)oContentPage.FindControl("txtPMExt");
        txtRDExt=(NumberField)oContentPage.FindControl("txtRDExt");
        txtSalesExt=(NumberField)oContentPage.FindControl("txtSalesExt");

        pnlCountersign = (Panel)oContentPage.FindControl("pnlCountersign");
        grdResult=(GridPanel)oContentPage.FindControl("grdResult");
        btnDelete = (Button)oContentPage.FindControl("btnDelete");
        cmbAttachmentType = (ComboBox)oContentPage.FindControl("cmbAttachmentType");
        btnDelete=(Button)oContentPage.FindControl("btnDelete");
        grdAttachment = (GridPanel)oContentPage.FindControl("grdAttachment");

        pnlResult = (Panel)oContentPage.FindControl("pnlResult");
        rgResult = (RadioGroup)oContentPage.FindControl("rgResult");
        txtReslutOpinion=(TextArea)oContentPage.FindControl("txtReslutOpinion");
        rdResultY = (Radio)oContentPage.FindControl("rdResultY");
        rdResultN = (Radio)oContentPage.FindControl("rdResultN");
        rdReulutCondition = (Radio)oContentPage.FindControl("rdReulutCondition");
        #endregion


    }
}

// Form logics
public class NPIGating_HISLogics : ISPMInterfaceContent
{

    private Page oPage;
    private NPIGating_HISUIShadow oUIControls;
    private IFormURLPara oPara;
    private ArrayList opc = new ArrayList();
    private string sql = string.Empty;
    private Model_NPI_DOA_Parameter oModel_NPI_DOA_Parameter;
    private Model_NPI_DOA_HandlerInfo oModel_NPI_DOA_HandlerInfo;
    public NPIGating_HISLogics(object oContainer, IUIShadow UIShadow)
        : base(oContainer)
    {
        this.SetUIShadow(UIShadow);
    }

    // Code for Page_Load
    protected override void PageLoad(object oContainer, IFormURLPara para, IUIShadow UIShadow)
    {
        oPage = (Page)oContainer;
        oUIControls = (NPIGating_HISUIShadow)UIShadow;
        oPara = para;

        this.InitialPageControls();
        base.PageLoad(oContainer, para, UIShadow);
    }

    #region Initial Controls
    private void InitialPageControls()
    {
        if (!oPage.IsPostBack)
        {

        }



        try
        {


            if (!oPage.IsPostBack)
            {
                if (oPara.TaskId < 0)
                {
                    InitialControl_FormInfo();
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {

        }
    }


    private void InitialControl_FormInfo()
    {

        //取單號
        oUIControls.txtFormNo.Text = Borg_Tools.GetFormNO("NPI");
        //日期控件初始化
        oUIControls.txtDate.Text = DateTime.Today.ToString("yyyy/MM/dd");
        //取登陸人信息
        NPIMgmt oMgmt = new NPIMgmt(oUIControls.lblSite.Text, oUIControls.lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        //取登陸人信息
        LiteOn.EA.CommonModel.Model_BorgUserInfo oModel_BorgUserInfo = new LiteOn.EA.CommonModel.Model_BorgUserInfo();
        Borg_User oBorg_User = new Borg_User();
        oModel_BorgUserInfo = oBorg_User.GetUserInfoByLogonId(oPara.LoginId);

    }

    #endregion

    // Code for 'draft' and 'pending for process'
    public override void InitialContainer(SPMTaskVariables SPMTaskVars, EFFormFields FormFields, ref object oContainer, IUIShadow UIShadow)
    {
        NPIGating_HISUIShadow lUIControls = (NPIGating_HISUIShadow)UIShadow;

        ShowFormDetail(FormFields, lUIControls, SPMTaskVars, "Pending");
        DisableField(FormFields, lUIControls, SPMTaskVars, "Pending");

        base.InitialContainer(SPMTaskVars, FormFields, ref oContainer, UIShadow);
    }

    private void ShowFormDetail(EFFormFields FormFields, NPIGating_HISUIShadow lUIControls, SPMTaskVariables SPMTaskVars, string Type)
    {

        string caseid = SPMTaskVars.ReadDatum("CASEID").ToString();
        string stepname = SPMTaskVars.ReadDatum("STEPNAME").ToString();
        SetMainValue(lUIControls, SPMTaskVars, FormFields);
        SetApproveResultValue(lUIControls, SPMTaskVars);
        BindAttachmentList(caseid);
       
    }

    private void DisableField(EFFormFields FormFields, NPIGating_HISUIShadow lUIControls, SPMTaskVariables SPMTaskVars, string Type)
    {

        string stepname = SPMTaskVars.ReadDatum("STEPNAME").ToString();
        string caseid = SPMTaskVars.ReadDatum("CASEID").ToString();
        SetMainFieldDisable(lUIControls, SPMTaskVars);
        
        


    }

    protected void SetMainValue(NPIGating_HISUIShadow lUIControls, SPMTaskVariables SPMTaskVars, EFFormFields FormFields)
    {
        string caseid = SPMTaskVars.ReadDatum("CASEID").ToString();
        string Site = FormFields["txtSite".ToUpper()];
        string Bu ="HIS";
        NPIMgmt oNPIMgmt = new NPIMgmt(Site, Bu);

        NPI_Standard oStandard = oNPIMgmt.InitialLeaveMgmt();

        DataTable dt = oStandard.GetMasterInfoHIS(caseid);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];

            oUIControls.txtFormNo.Text = dr["DOC_NO"].ToString();
            oUIControls.txtCustomer.Text = dr["CUSTOMER"].ToString();
            oUIControls.txtDate.Text = dr["APPLY_DATE"].ToString();
            oUIControls.txtNotes.Text = dr["REMARK"].ToString();
            oUIControls.cmbPhase.Text = dr["PHASE"].ToString();
            oUIControls.cmbPlant.Text = dr["BUILDING"].ToString();
            oUIControls.cbProductType.SelectedItem.Text= dr["PRODUCT_TYPE"].ToString();
            oUIControls.txtModel.Text = dr["MODEL_NAME"].ToString();
            oUIControls.cmbLayout.Text = dr["LAYOUT"].ToString();
            oUIControls.txtPM.Text = dr["NPI_PM"].ToString();
            oUIControls.txtRD.Text = dr["RD_ENGINEER"].ToString();
            oUIControls.txtSales.Text = dr["SALES_OWNER"].ToString();
            oUIControls.txtNextStage_BeginDate.Text = dr["NEXTSTAGE_DATE"].ToString();
            oUIControls.cmbPM.SelectedItem.Text = dr["PM_LOC"].ToString();
            oUIControls.txtPMExt.Text = dr["PM_EXT"].ToString();
            oUIControls.cmbRD.SelectedItem.Text = dr["RD_LOC"].ToString();
            oUIControls.txtRDExt.Text = dr["RD_EXT"].ToString();
            oUIControls.cmbSales.SelectedItem.Text = dr["SALES_LOC"].ToString();
            oUIControls.txtSalesExt.Text = dr["SALES_EXT"].ToString();

        }

    }

    protected void BindAttachmentList(string CaseId)
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        string sql = "select * from TB_NPI_APP_ATTACHFILE where CASEID=@CaseID";
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@CaseID", DbType.String, CaseId));
        DataTable dtAttachfile = sdb.TransactionExecute(sql, opc);
        oUIControls.grdAttachment.Store.Primary.DataSource = dtAttachfile;
        oUIControls.grdAttachment.Store.Primary.DataBind();

    }

    protected void SetMainFieldDisable(NPIGating_HISUIShadow lUIControls, SPMTaskVariables SPMTaskVars)
    {
        string stepname = SPMTaskVars.ReadDatum("STEPNAME").ToString();

        oUIControls.txtCustomer.ReadOnly = true;
        oUIControls.txtDate.ReadOnly = true;
        oUIControls.txtFormNo.ReadOnly = true;
        oUIControls.txtModel.ReadOnly = true;
        oUIControls.txtNextStage_BeginDate.ReadOnly = true;
        oUIControls.txtNotes.ReadOnly = true;
        oUIControls.txtPM.ReadOnly = true;
        oUIControls.txtRD.ReadOnly = true;
        oUIControls.cmbPhase.ReadOnly = true;
        oUIControls.cmbLayout.ReadOnly = true;
        oUIControls.cmbPM.ReadOnly = true;
        oUIControls.cmbRD.ReadOnly = true;
        oUIControls.cmbSales.ReadOnly = true;
        oUIControls.txtPMExt.ReadOnly = true;
        oUIControls.txtRDExt.ReadOnly = true;
        oUIControls.txtSalesExt.ReadOnly = true;
        oUIControls.cbProductType.ReadOnly = true;
        oUIControls.cmbPlant.ReadOnly = true;
        oUIControls.txtSales.ReadOnly = true;
        oUIControls.ConAttachment.Hidden = true;
        oUIControls.btnDelete.Hidden = true;
        oUIControls.pnlCountersign.Hidden = false;
        oUIControls.pnlResult.Hidden = false;
      

        

    }

    protected void SetApproveResultValue(NPIGating_HISUIShadow lUIControls, SPMTaskVariables SPMTaskVars)
    {
        string stepname = SPMTaskVars.ReadDatum("STEPNAME").ToString();
        string caseid = SPMTaskVars.ReadDatum("CASEID").ToString();
        NPIMgmt oMgmt = new NPIMgmt(oUIControls.lblSite.Text, oUIControls.lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        DataTable dt = new DataTable();
        dt = oStandard.GetNPIApproveResult(caseid);
        BindGrid(dt, lUIControls.grdResult);
    }
   
    public override void InitialDisableContainer(SPMTaskVariables SPMTaskVars, EFFormFields FormFields, ref object oContainer, IUIShadow UIShadow)
    {
        NPIGating_HISUIShadow lUIControls = (NPIGating_HISUIShadow)UIShadow;
        ShowFormDetail(FormFields, lUIControls, SPMTaskVars, "");
        DisableField(FormFields, lUIControls, SPMTaskVars, "");
        base.InitialDisableContainer(SPMTaskVars, FormFields, ref oContainer, UIShadow);
    }

    // Validate contols before submit
    public override bool EFFormFieldsValidation(SPMSubmitMethod SubmitMethod, SPMProcessMethod ProcessMethod, SPMTaskVariables SPMTaskVars, ref IInterfaceHandleResult HandleResult, object oContainer, IUIShadow UIShadow)
    {
        NPIGating_HISUIShadow lUIControls = (NPIGating_HISUIShadow)UIShadow;
        string ErrMsg = string.Empty;

        if (SubmitMethod == SPMSubmitMethod.CreateNewCase)
        {
            #region[ check list]

            //ErrMsg = BeginFormFieldsValidation(UIShadow);

            if (ErrMsg.Length > 0)
            {
                HandleResult.IsSuccess = false;
                HandleResult.CustomMessage = ErrMsg;
                return false;
            }
            ////依DOA获取待签关卡及签核人资讯
            if (HandleResult.IsSuccess)
            {
                NPIMgmt oMgmt = new NPIMgmt(lUIControls.lblSite.Text, lUIControls.lblBu.Text);
                NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
                
                Model_NPI_DOA_Parameter oModel_NPI_DOA_Parameter = new Model_NPI_DOA_Parameter();

                //oModel_NPI_DOA_Parameter._BU = "HIS";
                oModel_NPI_DOA_Parameter._BU = lUIControls.lblBu.Text;
                oModel_NPI_DOA_Parameter._BUILDING = lUIControls.cmbPlant.SelectedItem.Text;
                oModel_NPI_DOA_Parameter._CASE_ID = oPara.CaseId;
                oModel_NPI_DOA_Parameter._FORM_NO = lUIControls.txtFormNo.Text;
                oModel_NPI_DOA_Parameter._PHASE = lUIControls.cmbPhase.SelectedItem.Text;

                oModel_NPI_DOA_HandlerInfo = oStandard.GenerateStepAndHandler(oModel_NPI_DOA_Parameter);

                if (!oModel_NPI_DOA_HandlerInfo._RESULT)
                {
                    HandleResult.IsSuccess = false;
                    HandleResult.CustomMessage = oModel_NPI_DOA_HandlerInfo._ERROR_MSG;
                }
            }
            #endregion
        }
        else
        {
            string stepName = (string)(SPMTaskVars.ReadDatum("STEPNAME"));
            if (SubmitMethod != SPMSubmitMethod.CreateNewCase)
            {
                HandleResult.IsSuccess = true;
            }
                if (stepName == "Begin" || stepName == "開始")
            {
                HandleResult.IsSuccess = false;
                HandleResult.CustomMessage = "退簽單據不可重複送簽!";
            }
            else 
            {
                if (!(lUIControls.rdResultY.Checked || lUIControls.rdResultN.Checked || lUIControls.rdReulutCondition.Checked))
                {
                    
                    HandleResult.IsSuccess = false;
                    HandleResult.CustomMessage = "請勾选签核結果!";
                }
             
            }
          
           


        }
        return base.EFFormFieldsValidation(SubmitMethod, ProcessMethod, SPMTaskVars, ref HandleResult, oContainer, UIShadow);
    }

    #region
    public string BeginFormFieldsValidation(IUIShadow UIShadow)
    {
        NPIGating_HISUIShadow lUIControls = (NPIGating_HISUIShadow)UIShadow;
        NPIMgmt oMgmt = new NPIMgmt(lUIControls.lblSite.Text, lUIControls.lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        StringBuilder sbErrMsg = new StringBuilder();
         string ErrMsg=string.Empty;
        if (string.IsNullOrEmpty(lUIControls.txtCustomer.Text))
        {
            sbErrMsg.Append("Custmer不允许为空</br>");
        }
        if (string.IsNullOrEmpty(lUIControls.cmbPlant.SelectedItem.Text))
        {
            sbErrMsg.Append(" 廠別不允许为空</br>");
        }
        if (string.IsNullOrEmpty(lUIControls.cbProductType.SelectedItem.Text))
        {
            sbErrMsg.Append(" 產品類別不允许为空</br>");
        }
        if (string.IsNullOrEmpty(lUIControls.txtModel.Text))
        {
            sbErrMsg.Append("幾種名稱不允许为空</br>");
        }
        if (string.IsNullOrEmpty(lUIControls.cmbPhase.SelectedItem.Text))
        {
            sbErrMsg.Append("階段不允许为空</br>");
        }


        if (string.IsNullOrEmpty(lUIControls.txtPM.Text))
        {
            sbErrMsg.Append("PM不允许为空</br>");
        }
        if (string.IsNullOrEmpty(lUIControls.txtRD.Text))
        {
            sbErrMsg.Append("RD不允许为空</br>");
        }
        if (string.IsNullOrEmpty(lUIControls.txtSales.Text))
        {
            sbErrMsg.Append("Sales不允许为空</br>");
        }
        //if (lUIControls.txtPM.Text.Length > 0)
        //{

        //    ErrMsg= oStandard.GetGlobalUserInfo(lUIControls.txtPM.Text);
        //    if (ErrMsg.Length > 0)
        //    {
        //        sbErrMsg.Append(ErrMsg + "</br>");
        //    }
        //}
        //if (lUIControls.txtRD.Text.Length > 0)
        //{

        //     ErrMsg = oStandard.GetGlobalUserInfo(lUIControls.txtRD.Text);
        //    if (ErrMsg.Length > 0)
        //    {
        //        sbErrMsg.Append(ErrMsg);
        //    }
        //}
        //if (lUIControls.txtSales.Text.Length > 0)
        //{

        //     ErrMsg = oStandard.GetGlobalUserInfo(lUIControls.txtSales.Text);
        //    if (ErrMsg.Length > 0)
        //    {
        //        sbErrMsg.Append(ErrMsg);
        //    }
        //}
      
        string ErrMsgAttachment= GetAttachmentInfo();
        if (ErrMsgAttachment.Length > 0)
        {
            sbErrMsg.Append(ErrMsgAttachment);
        }
        
        return sbErrMsg.ToString();
    }

  
    #endregion
    // Fill SPM's EFFormFieldData
    public override void PrepareEFFormFields(SPMSubmitMethod SubmitMethod, SPMProcessMethod ProcessMethod, SPMTaskVariables TaskVars, ref EFFormFields FormFields, ref IInterfaceHandleResult HandleResult, object oContainer, IUIShadow UIShadow, ref string ApplicantInfo)
    {
        NPIGating_HISUIShadow lUIControls = (NPIGating_HISUIShadow)UIShadow;
        FormFields.SetOrAdd("txtFormNo".ToUpper(), lUIControls.txtFormNo.Text);
        FormFields.SetOrAdd("txtSite".ToUpper(), lUIControls.lblSite.Text);
        FormFields.SetOrAdd("txtBu".ToUpper(), lUIControls.lblBu.Text);
        FormFields.SetOrAdd("txtBuilding".ToUpper(), lUIControls.cmbPlant.SelectedItem.Text);
        base.PrepareEFFormFields(SubmitMethod, ProcessMethod, TaskVars, ref FormFields, ref HandleResult, oContainer, UIShadow, ref ApplicantInfo);
    }

    // Fill SPM Variable 
    public override void PrepareSPMVariables(SPMSubmitMethod SubmitMethod, SPMProcessMethod ProcessMethod, SPMTaskVariables SPMTaskVars, ref SPMVariables Variables, ref SPMRoutingVariable RoutingVariable, ref string strSPMUid, string strMemo, string strNotesForNextApprover, EFFormFields FormFields, ref IInterfaceHandleResult HandleResult, ref string SuccessMessage)
    {
        bool isError = false;
        string stepName = string.Empty;
        string errorMsg = string.Empty;
        string FormNo = FormFields["txtFormNo".ToUpper()];
        string Applicant = oPara.LoginId;
        Variables.Add(SPMVariableKey.Subject, "[NPIReport_Application] [" + FormNo + "] By [" + Applicant + "]");
        string Approver = string.Empty;
        NPIMgmt oMgmt = new NPIMgmt(oUIControls.lblSite.Text, oUIControls.lblBu.Text);
        //NPIMgmt oMgmt = new NPIMgmt("CZ1","HIS");
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();

        if (SubmitMethod == SPMSubmitMethod.CreateNewCase)
        {
            DataTable dtDOAHandler = oModel_NPI_DOA_HandlerInfo._HANDLER;
            if (dtDOAHandler.Rows.Count > 0)
            {
                DataRow dr = dtDOAHandler.Rows[0];
                string step_name = dr["STEP_NAME"].ToString();
                string handler = dr["HANDLER"].ToString();
                RoutingVariable = new SPMRoutingVariable(SPMRoutingVariableKey.spm_NextHandler, step_name + "(" + handler + ")");
            }
            else
            {
                HandleResult.IsSuccess = false;
                HandleResult.CustomMessage = "無法獲取下一關簽核人信息,請聯絡系統管理員!";
            }

        }
        if (SubmitMethod == SPMSubmitMethod.HandleCase_Approve)
        {

            Dictionary<string, object> result = new Dictionary<string, object>();
            stepName = (string)SPMTaskVars.ReadDatum("STEPNAME");
            int caseID = int.Parse((string)(SPMTaskVars.ReadDatum("CASEID")));
            ////获取待签核关卡及签核人资讯
            result = oStandard.GetNextStepAndHandler_NPI(caseID,FormNo,stepName,oUIControls.cmbPlant.SelectedItem.Text,oUIControls.cmbPhase.SelectedItem.Text);
            if (!(bool)result["Result"])//无法找到签核关卡资讯,显示ERROR
            {
                HandleResult.IsSuccess = false;
                HandleResult.CustomMessage = (string)result["ErrMsg"];
            }
            else
            {
                try
                {
                    RoutingVariable = new SPMRoutingVariable(SPMRoutingVariableKey.spm_Jump, (string)result["SPMRoutingVariable"]);
                }
                catch (Exception ex)
                {
                    HandleResult.IsSuccess = false;
                    HandleResult.CustomMessage = "DOA Error:" + ex.Message;
                }
                if (RoutingVariable == null)
                {
                    HandleResult.IsSuccess = false;
                    HandleResult.CustomMessage = "Error: get RoutingVariable fail";
                }
            }

        }
        base.PrepareSPMVariables(SubmitMethod, ProcessMethod, SPMTaskVars, ref Variables, ref RoutingVariable, ref strSPMUid, strMemo, strNotesForNextApprover, FormFields, ref HandleResult, ref SuccessMessage);

    }

    // Code for 'before send'
    public override void SPMBeforeSend(SPMSubmitMethod SubmitMethod, SPMTaskVariables SPMTaskVars, SPMVariables Variables, SPMRoutingVariable RoutingVariable, ref EFFormFields FormFields, ref IInterfaceHandleResult HandleResult)
    {
        if (RoutingVariable != null)
        {
            switch (RoutingVariable.Key)
            {
                case SPMRoutingVariableKey.spm_Return:

                    string value = RoutingVariable.Data;

                    break;
            }
        }

        base.SPMBeforeSend(SubmitMethod, SPMTaskVars, Variables, RoutingVariable, ref FormFields, ref HandleResult);
    }

    // Code for 'after send'
    public override void SPMAfterSend(SPMSubmitMethod SubmitMethod, SPMTaskVariables SPMTaskVars, SPMVariables Variables, SPMRoutingVariable RoutingVariable, EFFormFields FormFields, ref IInterfaceHandleResult HandleResult)
    {
        string caseid= System.Web.HttpUtility.UrlDecode(Variables[SPMVariableKey.CaseId]);

        if (RoutingVariable != null)
        {
            string sRoutingData = string.Empty;
            switch (RoutingVariable.Key)
            {
                case SPMRoutingVariableKey.spm_Return:

                    sRoutingData = RoutingVariable.Data;

                    break;
                case SPMRoutingVariableKey.spm_Jump:

                    sRoutingData = RoutingVariable.Data;
                    break;
            }
        }


        //// Business logic
        if (SubmitMethod != SPMSubmitMethod.CreateNewCase)
        {
            SPMAfterSend_DBIO(SPMTaskVars, FormFields, ref HandleResult, RoutingVariable);
        }
        else
        {

            ////////1.待签核关卡及签核人写DB

            NPIMgmt oNPIMgmt = new NPIMgmt(oUIControls.lblSite.Text, oUIControls.lblBu.Text);
            NPI_Standard oStandard = oNPIMgmt.InitialLeaveMgmt();

            Dictionary<string, object> result = oStandard.RecordDOAHandler(caseid, oUIControls.txtFormNo.Text, oUIControls.lblBu.Text, oUIControls.cmbPlant.SelectedItem.Text,oUIControls.cmbPhase.SelectedItem.Text);
            if (!(bool)result["Result"])
            {
                HandleResult.IsSuccess = false;
                HandleResult.CustomMessage = (string)result["ErrMsg"];
            }

            else
            {
               StringBuilder sbLayout=new StringBuilder();
                if ( oUIControls.cmbLayout.SelectedItems.Count>0)
                {
              
                    foreach (SelectedListItem li in oUIControls.cmbLayout.SelectedItems)
                    {
                        sbLayout.AppendFormat("{0},",li.Text);
                    }
                }

                Model_NPI_APP_MAIN_HIS oModel_Main = new Model_NPI_APP_MAIN_HIS();
                oModel_Main._DOC_NO = oUIControls.txtFormNo.Text.Trim();
                oModel_Main._BU = oUIControls.lblBu.Text;
                oModel_Main._BUILDING = oUIControls.cmbPlant.SelectedItem.Text;
                oModel_Main._APPLY_DATE = oUIControls.txtDate.Text;
                oModel_Main._APPLY_USERID = oPara.LoginId.Trim();
                oModel_Main._MODEL_NAME = oUIControls.txtModel.Text;
                oModel_Main._CUSTOMER = oUIControls.txtCustomer.Text;
                oModel_Main._PRODUCT_TYPE = oUIControls.cbProductType.SelectedItem.Text;
                oModel_Main._LAYOUT = sbLayout.ToString().TrimEnd(',');
                oModel_Main._PHASE = oUIControls.cmbPhase.SelectedItem.Text;
                oModel_Main._NEXTSTAGE_DATE = oUIControls.txtNextStage_BeginDate.SelectedDate.ToString("yyyy/MM/dd");
                oModel_Main._NPI_PM = oUIControls.txtPM.Text;
                oModel_Main._SALES_OWNER = oUIControls.txtSales.Text;
                oModel_Main._RD_ENGINEER = oUIControls.txtRD.Text;
                oModel_Main._REMARK = oUIControls.txtNotes.Text;
                oModel_Main._CASEID = oPara.CaseId.ToString();
                oModel_Main._UPDATE_USERID = oPara.LoginId;
                oModel_Main._STATUS = "Pending";

                oModel_Main._PMLOC = oUIControls.cmbPM.SelectedItem.Text.Trim();
                oModel_Main._PMEXT = oUIControls.txtPMExt.Text.Trim();
                oModel_Main._RDLOC = oUIControls.cmbRD.SelectedItem.Text.Trim();
                oModel_Main._RDEXT = oUIControls.txtRDExt.Text.Trim();
                oModel_Main._SALESLOC = oUIControls.cmbSales.SelectedItem.Text.Trim();
                oModel_Main._SALESEXT = oUIControls.txtSalesExt.Text.Trim();

                result = oStandard.RecordOperation_NPIMain(oModel_Main, Status_Operation.ADD);

                //result = "true";

                if (!(bool)result["Result"])
                {
                    HandleResult.IsSuccess = false;
                    HandleResult.CustomMessage = (string)result["ErrMsg"];
                }

            }
        }
        base.SPMAfterSend(SubmitMethod, SPMTaskVars, Variables, RoutingVariable, FormFields, ref HandleResult);
    }

    private void SPMAfterSend_DBIO(SPMTaskVariables SPMTaskVars, EFFormFields FormFields, ref IInterfaceHandleResult HandleResult, SPMRoutingVariable RoutingVariable)
    {

        try
        {
            string stepName = (string)SPMTaskVars.ReadDatum("STEPNAME");
            int caseID = int.Parse((string)(SPMTaskVars.ReadDatum("CASEID")));
            NPIMgmt oMgmt = new NPIMgmt(oUIControls.lblSite.Text, oUIControls.lblBu.Text);
            NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
            Dictionary<string, object> result = new Dictionary<string, object>();
            Model_PRELAUNCH_MAIN oModel = new Model_PRELAUNCH_MAIN();
            string Handler = oPara.LoginId;
            switch (RoutingVariable.Key)
            {
                case SPMRoutingVariableKey.spm_Recall:
                    //修改本单据数据状态为Abort

                    oModel._CaseId = caseID;
                    oModel._Status = "Abort";

                    result = oStandard.RecordOperation_PrelaunchMain(oModel, Status_Operation.UPDATE);
                    if (!(bool)result["Result"])
                    {
                        HandleResult.IsSuccess = false;
                        HandleResult.CustomMessage = (string)result["ErrMsg"];
                    }

                    break;
                case SPMRoutingVariableKey.spm_Jump:
                    //1.标示当前关卡已核准（如为最后一关，则变更状态为Finished）
                    string ApproverRemark = string.Empty;
                    string ApproverResult=string.Empty;
                    for (int i = 0; i < oUIControls.rgResult.Items.Count; i++)
                    {
                        if (oUIControls.rgResult.Items[i].Checked == true)
                        {
                           
                            ApproverResult = oUIControls.rgResult.Items[i].BoxLabel.Trim();
                        }
                    }
                    result = oStandard.UpdateDOAHandlerStatus(caseID, stepName,Handler,ApproverResult,oUIControls.txtReslutOpinion.Text.Trim());

                    if (!(bool)result["Result"])
                    {
                        HandleResult.IsSuccess = false;
                        HandleResult.CustomMessage = (string)result["ErrMsg"];
                    }
                    break;
            }
        }
        catch (Exception e)
        {
            HandleResult.IsSuccess = false;
            HandleResult.CustomMessage = e.Message;
        }


    }

    // Recall workflow 
    public override void SPMBackoutProcess(SPMSubmitMethod SubmitMethod, SPMTaskVariables SPMTaskVars, SPMVariables Variables, EFFormFields FormFields, ref IInterfaceHandleResult HandleResult)
    {
        base.SPMBackoutProcess(SubmitMethod, SPMTaskVars, Variables, FormFields, ref HandleResult);
    }

    //Abort
    public override void SPMRecallProcess(SPMSubmitMethod SubmitMethod, SPMTaskVariables SPMTaskVars, SPMVariables Variables, EFFormFields FormFields, ref IInterfaceHandleResult HandleResult)
    {
        base.SPMRecallProcess(SubmitMethod, SPMTaskVars, Variables, FormFields, ref HandleResult);
    }

    public override void SPMStepActivity(SPMSubmitMethod SubmitMethod, SPMTaskVariables SPMTaskVars, SPMVariables Variables, EFFormFields FormFields, string NewStepName, ref IInterfaceHandleResult HandleResult)
    {
        base.SPMStepActivity(SubmitMethod, SPMTaskVars, Variables, FormFields, NewStepName, ref HandleResult);
    }

    public override void SPMStepComplete(SPMSubmitMethod SubmitMethod, SPMTaskVariables SPMTaskVars, SPMVariables Variables, EFFormFields FormFields, string CompletedStepName, ref IInterfaceHandleResult HandleResult)
    {
        base.SPMStepComplete(SubmitMethod, SPMTaskVars, Variables, FormFields, CompletedStepName, ref HandleResult);
    }

    public override void Print(int iTaskId, SPMTaskVariables SPMTaskVars, EFFormFields FormFields, object oContainer, IUIShadow UIShadow)
    {
        if (iTaskId < 0)
        {
            NPIGating_HISUIShadow lControls = (NPIGating_HISUIShadow)UIShadow;
        }
        //else
        //{

        //}

        base.Print(iTaskId, SPMTaskVars, FormFields, oContainer, UIShadow);
    }

    public override void SPM_SendError(SPMSubmitMethod SubmitMethod, SPMTaskVariables SPMTaskVars, SPMVariables Variables, SPMRoutingVariable RoutingVariable, EFFormFields FormFields, IInterfaceHandleResult HandleResult)
    {
        string strExceptionFrom = HandleResult.ExceptionFrom;
        string strErrorMessage = HandleResult.CustomMessage;
        base.SPM_SendError(SubmitMethod, SPMTaskVars, Variables, RoutingVariable, FormFields, HandleResult);
    }

    protected void BindGrid(DataTable dt, GridPanel grd)
    {
        grd.Store.Primary.DataSource = dt;
        grd.Store.Primary.DataBind();
    }

    private void Alert(string msg)
    {
        if (msg.Length > 0)
        {
            X.Msg.Alert("提示", msg).Show();
        }

    }

    public string  GetAttachmentInfo()
    {
        
        StringBuilder sbErrMsg=new StringBuilder();
        StringBuilder sbType = new StringBuilder();
        int TypeCount = oUIControls.cmbAttachmentType.Items.Count;
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        string sql = "SELECT  FILE_TYPE FROM TB_NPI_APP_ATTACHFILE WITH(NOLOCK)"
                   + " WHERE 1=1 AND  SUB_DOC_NO=@doc_no";
        ArrayList opc = new ArrayList();
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@doc_no", DbType.String, oUIControls.txtFormNo.Text));
        DataTable dt = sdb.TransactionExecute(sql, opc);

        if (dt.Rows.Count > 0)
        {

            string[] arrUploaed = new string[dt.Rows.Count];
            string[] arrTotal = new string[TypeCount];
            for(int i=0;i<dt.Rows.Count;i++)
            {
                arrUploaed[i] = dt.Rows[i]["FILE_TYPE"].ToString().ToUpper();
            }
            for (int j = 0; j < TypeCount; j++)
            {
                arrTotal[j] = oUIControls.cmbAttachmentType.Items[j].Text.ToString().ToUpper().Trim();
            }

            string[] arrNew = arrTotal.Except(arrUploaed).ToArray();

            for (int m = 0; m < arrNew.Length; m++)
            {

                sbErrMsg.AppendFormat("{0} 附件未上傳!", arrNew[m]);
            }
        }
        else
        {
            sbErrMsg.Append("附件未上傳,請確認！");
        }
        return sbErrMsg.ToString();

    }

  

}