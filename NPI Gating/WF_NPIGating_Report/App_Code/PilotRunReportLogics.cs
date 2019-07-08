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
using LiteOn.EA.BLL;
using LiteOn.EA.CommonModel;
using LiteOn.EA.Borg.Utility;
using LiteOn.EA.NPIReport.Utility;
using Liteon.ICM.DataCore;
using System.Collections;
//

// UI controls holder for Example1 
public class PilotRunReportUIShadow : IUIShadow
{
    // Declare controls which show in the web page
    #region Beigin
    public Panel pnlStart;
    public Panel pnlApp;
    public GridPanel grdInfo;
    public TextField txtDOC_NO;
    public TextField txtSub_No;
    public TextField txtPROD_GROUP;
    public TextField txtModel_R;
    public TextField txtLotQty;
    public DateField txtIssueDate; 

    public TextField txtPCB;
    public TextField txtSPECRev;
    public DateField txtInputDate;
    public TextField txtCustomer;
    public SelectBox sbPhase;
    public TextField txtLINE;
    public TextField txtBomRev;
    public TextField txtCustomerRev;
    public DateField txtPkDate;
    public TextField txtWorkOrder;
    public TextArea txtRemarkM;


    public Checkbox chkD;
    public Checkbox chkC;
    public Checkbox chkI;
    public Checkbox chkP;
    public CheckboxGroup cbStartItem;
    public Hidden lblSite;
    public Hidden lblBu;
    public Hidden lblBuilding;
    public Checkbox cbModify;

    public Panel pnlFMEABatch;
    public Button btnFModify;

    public Button btnModify;
    public Panel pnlIssueBatch;
    public Button btnDownLoad;
    public SelectBox sbBuilding; 


    #endregion

    #region Dept.Write
    public GridPanel grdDFXList;
    public Panel pnlDFXList;
    public Panel pnlDFXInfo;

    public GridPanel grdCTQInfo;
    public Panel pnlCTQList;
    public Panel pnlCTQInfo;
    public Panel pnlIssueList;
    public Panel pnlIssueInfo;
    public GridPanel grdIssuesList;
    public Button btnAdd;
    public Button btnDelete;

    public Panel pnlFMEAList;
    public Panel pnlFMEAWrite;
    public Panel pnlFMEAReply;
    public GridPanel grdPFMAList;
    public Button btnPFMAAdd;
    public Button btnPFMADelete;
    public ComboBox cmbIssuesDept;
    public ComboBox cmbPFMADept;
    public Toolbar Toolbar5;

    public Radio rdPBatchIssueNA;
    public Radio rdFMEABatchNA;

    public RadioGroup rgIssueBatch;
    public RadioGroup rgFMEABatch;
    #endregion

    #region  Dept.Reply
    public Panel PanelDFXScore;
    public GridPanel gridDFXScore;
    public Panel pnlDFXReply;
    public TextArea txtActions;
    public DateField txtCompleteDate;
    public CheckboxGroup cbGroupTracking;
    public Checkbox cbEVT;
    public Checkbox cbDVT;
    public Checkbox cbPR;
    public Button btnDFXReply_Click;


    public Panel pnlCLCAReply;
    public TextArea txtCLCAActions;
    public TextArea txtCLCAPreActions;
    public DateField txtCLCACompleteDate;
    public ComboBox cmbCLCAImproveStatus;
    public Button btnCLCAReply_Save;

    #endregion

    #region Dept.Prepared
    public ComboBox cobDept;
    public Panel pnlPrepared;
    public GridPanel grdDFXInconformity;
    public GridPanel grdCLCAInconformity;
    public GridPanel grdIssuesInconformity;
    public GridPanel grdFMEAInconformity;
    public Panel pnlAttachment;
    public TextField txtAttachRemark;
    public FileUploadField FileAttachment;
    public Button btnConfirm1;
    public GridPanel grdAttachement;
    public Button btnDelAttachement;
    public Panel pnlAttach;

    #endregion

    #region PM Approver
    public FieldSet FieldSetUploadIssues;
    public FieldSet FieldSetIssues;
    public Button btnDelVQM;
    public GridPanel grdAttachmentIssue;
    #endregion

    #region Top Manager

    public Panel pnlResultMain;
    public Panel pnlReslut;
    public TextField txtReslutDept;
    public RadioGroup rgResult;
    public Radio rdResultY;
    public Radio rdResultN;
    public Radio rdReulutCondition;
    public TextArea txtReslutOpinion;
    public GridPanel grdResult;


    #endregion

    public PilotRunReportUIShadow(Page oContainer)
        : base(oContainer) { }

    // Initialize controls
    public override void InitShadow(System.Web.UI.WebControls.ContentPlaceHolder oContentPage)
    {
        #region begin
        pnlStart = (Panel)oContentPage.FindControl("pnlStart");
        pnlApp = (Panel)oContentPage.FindControl("pnlApp");
        grdInfo = (GridPanel)oContentPage.FindControl("grdInfo");
        txtDOC_NO = (TextField)oContentPage.FindControl("txtDOC_NO");
        txtSub_No = (TextField)oContentPage.FindControl("txtSub_No");
        txtPROD_GROUP = (TextField)oContentPage.FindControl("txtPROD_GROUP");
        txtModel_R = (TextField)oContentPage.FindControl("txtModel_R");
        txtLotQty = (TextField)oContentPage.FindControl("txtLotQty");
        txtPCB = (TextField)oContentPage.FindControl("txtPCB");
        txtSPECRev = (TextField)oContentPage.FindControl("txtSPECRev");
        txtWorkOrder = (TextField)oContentPage.FindControl("txtWorkOrder");
        txtInputDate = (DateField)oContentPage.FindControl("txtInputDate");
        txtIssueDate = (DateField)oContentPage.FindControl("txtIssueDate");
        txtCustomer = (TextField)oContentPage.FindControl("txtCustomer");
        sbPhase = (SelectBox)oContentPage.FindControl("sbPhase");
        txtLINE = (TextField)oContentPage.FindControl("txtLINE");
        txtBomRev = (TextField)oContentPage.FindControl("txtBomRev");
        txtCustomerRev = (TextField)oContentPage.FindControl("txtCustomerRev");
        txtPkDate = (DateField)oContentPage.FindControl("txtPkDate");
        chkD = (Checkbox)oContentPage.FindControl("chkD");
        chkC = (Checkbox)oContentPage.FindControl("chkC");
        chkI = (Checkbox)oContentPage.FindControl("chkI");
        chkP = (Checkbox)oContentPage.FindControl("chkP");
        cbStartItem = (CheckboxGroup)oContentPage.FindControl("cbStartItem");
        lblSite = (Hidden)oContentPage.FindControl("lblSite");
        lblBu = (Hidden)oContentPage.FindControl("lblBu");
        lblBuilding = (Hidden)oContentPage.FindControl("lblBuilding");
        cbModify = (Checkbox)oContentPage.FindControl("cbModify");
        txtRemarkM = (TextArea)oContentPage.FindControl("txtRemarkM");

        
        pnlFMEABatch = (Panel)oContentPage.FindControl("pnlFMEABatch");
        btnFModify = (Button)oContentPage.FindControl("btnFModify");
        btnModify = (Button)oContentPage.FindControl("btnModify");
        pnlIssueBatch = (Panel)oContentPage.FindControl("pnlIssueBatch");
        sbBuilding = (SelectBox)oContentPage.FindControl("sbBuilding");

        #endregion

        #region Dept.Write
        grdDFXList = (GridPanel)oContentPage.FindControl("grdDFXList");
        pnlDFXList = (Panel)oContentPage.FindControl("pnlDFXList");
        pnlDFXInfo = (Panel)oContentPage.FindControl("pnlDFXInfo");


        grdCTQInfo = (GridPanel)oContentPage.FindControl("grdCTQInfo");
        pnlCTQInfo = (Panel)oContentPage.FindControl("pnlCTQInfo");
        pnlCTQList = (Panel)oContentPage.FindControl("pnlCTQList");

        grdIssuesList = (GridPanel)oContentPage.FindControl("grdIssuesList");
        pnlIssueList = (Panel)oContentPage.FindControl("pnlIssueList");
        pnlIssueInfo = (Panel)oContentPage.FindControl("pnlIssueInfo");
        btnAdd = (Button)oContentPage.FindControl("btnAdd");
        btnDelete = (Button)oContentPage.FindControl("btnDelete");

        grdPFMAList = (GridPanel)oContentPage.FindControl("grdPFMAList");
        pnlFMEAList = (Panel)oContentPage.FindControl("pnlFMEAList");
        pnlFMEAWrite = (Panel)oContentPage.FindControl("pnlFMEAWrite");
        btnPFMAAdd = (Button)oContentPage.FindControl("btnPFMAAdd");
        btnPFMADelete = (Button)oContentPage.FindControl("btnPFMADelete");
        cmbIssuesDept = (ComboBox)oContentPage.FindControl("cmbIssuesDept");
        cmbPFMADept = (ComboBox)oContentPage.FindControl("cmbPFMADept");
        Toolbar5 = (Toolbar)oContentPage.FindControl("Toolbar5");
        rdPBatchIssueNA = (Radio)oContentPage.FindControl("rdPBatchIssueNA");
        rdFMEABatchNA = (Radio)oContentPage.FindControl("rdFMEABatchNA");


        rgIssueBatch = (RadioGroup)oContentPage.FindControl("rgIssueBatch");
        rgFMEABatch = (RadioGroup)oContentPage.FindControl("rgFMEABatch");
        #endregion

        #region Dept.Reply
        gridDFXScore = (GridPanel)oContentPage.FindControl("gridDFXScore");
        PanelDFXScore = (Panel)oContentPage.FindControl("PanelDFXScore");
        pnlDFXReply = (Panel)oContentPage.FindControl("pnlDFXReply");
        txtActions = (TextArea)oContentPage.FindControl("txtActions");
        txtCompleteDate = (DateField)oContentPage.FindControl("txtCompleteDate");
        cbGroupTracking = (CheckboxGroup)oContentPage.FindControl("cbGroupTracking");
        cbEVT = (Checkbox)oContentPage.FindControl("cbEVT");
        cbDVT = (Checkbox)oContentPage.FindControl("cbDVT");
        cbPR = (Checkbox)oContentPage.FindControl("cbPR");
        btnDFXReply_Click = (Button)oContentPage.FindControl("btnDFXReply_Click");

        pnlCLCAReply = (Panel)oContentPage.FindControl("pnlCLCAReply");
        txtCLCAActions = (TextArea)oContentPage.FindControl("txtCLCAActions");
        txtCLCAPreActions = (TextArea)oContentPage.FindControl("txtCLCAPreActions");
        txtCLCACompleteDate = (DateField)oContentPage.FindControl("txtCLCACompleteDate");
        cmbCLCAImproveStatus = (ComboBox)oContentPage.FindControl("cmbCLCAImproveStatus");
        btnCLCAReply_Save = (Button)oContentPage.FindControl("btnCLCAReply_Save");
        #endregion

        #region Dept.Prepared
        cobDept = (ComboBox)oContentPage.FindControl("cobDept");
        pnlPrepared = (Panel)oContentPage.FindControl("pnlPrepared");
        grdDFXInconformity = (GridPanel)oContentPage.FindControl("grdDFXInconformity");
        grdCLCAInconformity = (GridPanel)oContentPage.FindControl("grdCLCAInconformity");
        grdIssuesInconformity = (GridPanel)oContentPage.FindControl("grdIssuesInconformity");
        grdFMEAInconformity = (GridPanel)oContentPage.FindControl("grdFMEAInconformity");
        pnlAttachment = (Panel)oContentPage.FindControl("pnlAttachment");
        txtAttachRemark = (TextField)oContentPage.FindControl("txtAttachRemark");
        FileAttachment = (FileUploadField)oContentPage.FindControl("FileAttachment");
        btnConfirm1 = (Button)oContentPage.FindControl("btnConfirm1");
        grdAttachement = (GridPanel)oContentPage.FindControl("grdAttachement");
        btnDelAttachement = (Button)oContentPage.FindControl("btnDelAttachement");
        btnDownLoad = (Button)oContentPage.FindControl("btnDownLoad");
        pnlAttach = (Panel)oContentPage.FindControl("pnlAttach");

        
        #endregion

        #region PM Approver
        FieldSetUploadIssues = (FieldSet)oContentPage.FindControl("FieldSetUploadIssues");
        FieldSetIssues = (FieldSet)oContentPage.FindControl("FieldSetIssues");
        btnDelVQM = (Button)oContentPage.FindControl("btnDelVQM");
        grdAttachmentIssue = (GridPanel)oContentPage.FindControl("grdAttachmentIssue");
        #endregion

        #region Top Manager
        pnlResultMain = (Panel)oContentPage.FindControl("pnlResultMain");
        pnlReslut = (Panel)oContentPage.FindControl("pnlReslut");
        txtReslutDept = (TextField)oContentPage.FindControl("txtReslutDept");
        txtReslutOpinion = (TextArea)oContentPage.FindControl("txtReslutOpinion");
        rgResult = (RadioGroup)oContentPage.FindControl("rgResult");
        rdResultY = (Radio)oContentPage.FindControl("rdResultY");
        rdResultN = (Radio)oContentPage.FindControl("rdResultN");
        rdReulutCondition = (Radio)oContentPage.FindControl("rdReulutCondition");
        grdResult = (GridPanel)oContentPage.FindControl("grdResult");
        #endregion
    }
}

// Form logics
public class PilotRunReportLogics : ISPMInterfaceContent
{

    private Page oPage;
    private PilotRunReportUIShadow oUIControls;
    private IFormURLPara oPara;
    private ArrayList opc = new ArrayList();
    private string sql = string.Empty;
    public PilotRunReportLogics(object oContainer, IUIShadow UIShadow)
        : base(oContainer)
    {
        this.SetUIShadow(UIShadow);
    }

    // Code for Page_Load
    protected override void PageLoad(object oContainer, IFormURLPara para, IUIShadow UIShadow)
    {
        oPage = (Page)oContainer;
        oUIControls = (PilotRunReportUIShadow)UIShadow;
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


    #endregion

    // Code for 'draft' and 'pending for process'
    public override void InitialContainer(SPMTaskVariables SPMTaskVars, EFFormFields FormFields, ref object oContainer, IUIShadow UIShadow)
    {
        PilotRunReportUIShadow lUIControls = (PilotRunReportUIShadow)UIShadow;

        ShowFormDetail(FormFields, lUIControls, SPMTaskVars, "Pending");

        base.InitialContainer(SPMTaskVars, FormFields, ref oContainer, UIShadow);
    }

    private void ShowFormDetail(EFFormFields FormFields, PilotRunReportUIShadow lUIControls, SPMTaskVariables SPMTaskVars, string Type)
    {

        string caseid = SPMTaskVars.ReadDatum("CASEID").ToString();
        string stepname = SPMTaskVars.ReadDatum("STEPNAME").ToString();
        string HandleType = oPara.HandleType;
        SetMainValueDisable(lUIControls, SPMTaskVars);
        SetMainValue(lUIControls, SPMTaskVars, FormFields);
        switch (Type)
        {
            case "Pending":

                switch (stepname)
                {
                    case "Dept.Write":
                    case "PM":
                    case "Dept.Reply":
                    case "WriteChecked":
                    case "ReplyChecked":
                        SetDeptWriteValue(lUIControls, SPMTaskVars);
                        SetDeptWriteValueDisable(lUIControls, SPMTaskVars);
                        break;
                    case "Dept.Prepared":
                    case "Dept.Checked":
                    case "PM Approver":
                        SetDeptPreparedDisable(lUIControls, stepname);

                        break;
                    case "NPI Leader":
                    case "TOP Manager":
                        SetDeptPreparedDisable(lUIControls, stepname);
                        SetTopMangerValueDisalbe(lUIControls);
                        break;

                }
                break;
            default:
                if (Type == "")
                {
                    oUIControls.rgIssueBatch.Hidden = true;
                    oUIControls.rgFMEABatch.Hidden = true;
                    oUIControls.btnPFMAAdd.Hidden = true;
                    oUIControls.btnPFMADelete.Hidden = true;
                    oUIControls.btnFModify.Hidden = true;
                    oUIControls.btnAdd.Hidden = true;
                    oUIControls.btnDelete.Hidden = true;
                    oUIControls.btnModify.Hidden = true;
                }
                SetDeptWriteValue(lUIControls, SPMTaskVars);
                SetDeptWriteValueDisable(lUIControls, SPMTaskVars);
                SetDeptPreparedDisable(lUIControls, stepname);
                //SetDeptPreparedDisable(lUIControls, stepname);
                SetTopMangerValueDisalbe_Begin(lUIControls);
                lUIControls.PanelDFXScore.Hidden = false;
                //CreateDFXScore(lUIControls.txtSub_No.Text);//生成DFX分數
                DataTable dtScoreM = CheckDFXScore(lUIControls.txtSub_No.Text);
                BindGrid(lUIControls.gridDFXScore, dtScoreM);

                break;
        }
    }

    #region 控件賦值

    protected void SetMainValue(PilotRunReportUIShadow lUIControls, SPMTaskVariables SPMTaskVars, EFFormFields FormFields)
    {
        string caseid = SPMTaskVars.ReadDatum("CASEID").ToString();
        string Site = FormFields["txtSite".ToUpper()];
        string Bu = FormFields["txtBu".ToUpper()];
        NPI_Standard oStandard = new NPI_Standard(Site, Bu);


        DataTable dt = oStandard.GetMasterInfo(caseid);
        if (dt.Rows.Count > 0)
        {

            DataRow dr = dt.Rows[0];

            lUIControls.txtDOC_NO.Text = dr["DOC_NO"].ToString();
            lUIControls.txtSub_No.Text = dr["SUB_DOC_NO"].ToString();
            lUIControls.txtPROD_GROUP.Text = dr["DOC_NO"].ToString();
            lUIControls.txtModel_R.Text = dr["MODEL_NAME"].ToString();
            lUIControls.txtLotQty.Text = dr["LOT_QTY"].ToString();
            lUIControls.txtPCB.Text = dr["PCB_REV"].ToString();
            lUIControls.txtSPECRev.Text = dr["SPEC_REV"].ToString();
            lUIControls.txtInputDate.Text = dr["INPUT_DATE"].ToString();
            lUIControls.txtCustomer.Text = dr["CUSTOMER"].ToString();
            lUIControls.sbPhase.Text = dr["SUB_DOC_PHASE"].ToString();
            lUIControls.txtLINE.Text = dr["LINE"].ToString();
            lUIControls.txtBomRev.Text = dr["BOM_REV"].ToString();
            lUIControls.txtCustomerRev.Text = dr["CUSTOMER_REV"].ToString();
            lUIControls.txtPkDate.Text = dr["PK_DATE"].ToString();
            lUIControls.txtWorkOrder.Text = dr["WorkOrder"].ToString();
            lUIControls.txtIssueDate.Text = dr["ISSUE_DATE"].ToString();
            string NeedStartInfo = dr["NeedStartItmes"].ToString();
            if (NeedStartInfo.Contains("D"))
            {
                lUIControls.chkD.Checked = true;
            }
            if (NeedStartInfo.Contains("C"))
            {
                lUIControls.chkC.Checked = true;
            }
            if (NeedStartInfo.Contains("I"))
            {
                lUIControls.chkI.Checked = true;
            }
            if (NeedStartInfo.Contains("P"))
            {
                lUIControls.chkP.Checked = true;
            }
            lUIControls.cbModify.Checked = (dr["MODIFY_FLAG"].ToString() == "Y") ? true : false;
        }

    }


    protected void SetDeptWriteValue(PilotRunReportUIShadow lUIControls, SPMTaskVariables SPMTaskVars)
    {
        string stepname = SPMTaskVars.ReadDatum("STEPNAME").ToString();
        BindData(lUIControls.cmbIssuesDept, GetUserDeptIF("A", lUIControls.txtPROD_GROUP.Text,stepname));
        BindData(lUIControls.cmbPFMADept, GetUserDeptIF("A", lUIControls.txtPROD_GROUP.Text,stepname));
        BindGrid(oUIControls.grdDFXList, GetDFXItem(lUIControls.txtSub_No.Text, stepname));
        DataTable dt = GetCTQInfo(lUIControls.txtDOC_NO.Text, lUIControls.txtSub_No.Text, stepname);
        BindGrid(lUIControls.grdCTQInfo, dt);
        BindGrid(lUIControls.grdIssuesList, GetIssuesInfo(lUIControls.txtSub_No.Text, stepname));
        BindGrid(lUIControls.grdPFMAList, GetPFMAInfo(lUIControls.txtSub_No.Text, stepname));
        switch (stepname)
        { 
            case "ReplyChecked":
                NPIMgmt oMgmt = new NPIMgmt(oUIControls.lblSite.Text, oUIControls.lblBu.Text);
                NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
                string Dept = oStandard.GetUserDept("A", oPara.LoginId, oUIControls.txtPROD_GROUP.Text);
                lUIControls.txtReslutDept.Text = Dept;
                DataTable dt1 = oStandard.GetNPI_Result(oPara.CaseId);
                BindGrid(lUIControls.grdResult, dt1);//綁定簽核意見

                CreateDFXScore(lUIControls.txtSub_No.Text);//生成DFX分數
                DataTable dtScoreM = CheckDFXScore(lUIControls.txtSub_No.Text);
                BindGrid(lUIControls.gridDFXScore, dtScoreM);

                break;
            case "WriteChecked":
                CreateDFXScore(lUIControls.txtSub_No.Text);//生成DFX分數
                DataTable dtScoreMW = CheckDFXScore(lUIControls.txtSub_No.Text);
                BindGrid(lUIControls.gridDFXScore, dtScoreMW);
                break;
        }


    }

    #region
    protected DataTable GetDFXItem(string subdoc, string StepName)
    {
        SPMTaskVariables SPMTaskVars = new SPMTaskVariables();
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        StringBuilder sb = new StringBuilder();
        NPIMgmt oMgmt = new NPIMgmt(oUIControls.lblSite.Text, oUIControls.lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        if (StepName == "Dept.Reply" || StepName == "Dept.Write")
        {
            string CheckDept = oStandard.GetUserDept("A", oPara.LoginId, oUIControls.txtPROD_GROUP.Text);
            if (CheckDept == "IQC" || CheckDept == "OQC")
            {
                string Dept = oStandard.GetUserDept("A", oPara.LoginId, oUIControls.txtPROD_GROUP.Text);
                DataTable dt = oStandard.GetDFXInconformity(subdoc, Dept, StepName);
                return dt;
            }
            else
            {
                string Dept = GetUserDept("AD", oPara.LoginId, oUIControls.txtPROD_GROUP.Text, StepName);
                DataTable dt = oStandard.GetDFXInconformity(subdoc, Dept, StepName);
                return dt;
            }
        }
        else
        {
            string Dept = oStandard.GetUserDept("A", oPara.LoginId, oUIControls.txtPROD_GROUP.Text);
            DataTable dt = oStandard.GetDFXInconformity(subdoc, Dept, StepName);
            return dt;
        }

    }

    private DataTable GetCTQInfo(string docNo, string subDocNo, string StepName)
    {
        decimal ACT_DD = 0;
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        SPMTaskVariables SPMTaskVars = new SPMTaskVariables();
        NPIMgmt oMgmt = new NPIMgmt(oUIControls.lblSite.Text, oUIControls.lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        if (StepName == "Dept.Reply" || StepName == "Dept.Write")
        {
            string CheckDept = oStandard.GetUserDept("A", oPara.LoginId, oUIControls.txtPROD_GROUP.Text);
            if (CheckDept == "IQC" || CheckDept == "OQC")
            {
                string Dept = oStandard.GetUserDept("A", oPara.LoginId, oUIControls.txtPROD_GROUP.Text);
                DataTable dt = oStandard.GetCLCAInconformity(subDocNo, Dept, StepName);
                return dt;
            }
            else
            {
                string Dept = GetUserDept("AC", oPara.LoginId, oUIControls.txtPROD_GROUP.Text, StepName);
                DataTable dt = oStandard.GetCLCAInconformity(subDocNo, Dept, StepName);
                return dt;
            }
        }
        else
        {
            string Dept = oStandard.GetUserDept("A", oPara.LoginId, oUIControls.txtPROD_GROUP.Text);
            DataTable dt = oStandard.GetCLCAInconformity(subDocNo, Dept, StepName);
            return dt;
        }

    }

    private DataTable GetIssuesInfo(string subdoc, string StepName)
    {
        StringBuilder sb = new StringBuilder();
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        ArrayList opc = new ArrayList();
        SPMTaskVariables SPMTaskVars = new SPMTaskVariables();
        NPIMgmt oMgmt = new NPIMgmt(oUIControls.lblSite.Text, oUIControls.lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        if (StepName == "Dept.Reply" || StepName == "Dept.Write")
        {
            string Dept = GetUserDept("AI", oPara.LoginId, oUIControls.txtPROD_GROUP.Text, StepName); 
            DataTable dt = oStandard.GetIssuesInconformity(subdoc, Dept, string.Empty);
            return dt;
        }
        else
        {
            string Dept = oStandard.GetUserDept("A", oPara.LoginId, oUIControls.txtPROD_GROUP.Text);
            DataTable dt = oStandard.GetIssuesInconformity(subdoc, Dept, string.Empty);
            return dt;
        }


    }

    private DataTable GetPFMAInfo(string subdoc, string StepName)
    {
        StringBuilder sb = new StringBuilder();
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        ArrayList opc = new ArrayList();
        SPMTaskVariables SPMTaskVars = new SPMTaskVariables();
        NPIMgmt oMgmt = new NPIMgmt(oUIControls.lblSite.Text, oUIControls.lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        if (StepName == "Dept.Reply" || StepName == "Dept.Write")
        {
            string Dept = GetUserDept("AF", oPara.LoginId, oUIControls.txtPROD_GROUP.Text, StepName); 
            DataTable dt = oStandard.GetFMEAInconformity(subdoc, Dept, string.Empty);
            return dt;
        }
        else
        {
            string Dept = oStandard.GetUserDept("A", oPara.LoginId, oUIControls.txtPROD_GROUP.Text);
            DataTable dt = oStandard.GetFMEAInconformity(subdoc, Dept, string.Empty);
            return dt;
        }
    }

    #endregion

    protected void SetMainValueDisable(PilotRunReportUIShadow lUIControls, SPMTaskVariables SPMTaskVars)
    {
        string stepname = SPMTaskVars.ReadDatum("STEPNAME").ToString();
        lUIControls.pnlStart.Hidden = false;
        lUIControls.pnlApp.Hidden = true;
        lUIControls.grdInfo.Hidden = true;
        lUIControls.txtDOC_NO.Hidden = true;
        lUIControls.txtSub_No.Hidden = true;
        lUIControls.cbStartItem.Hidden = false;
        lUIControls.txtDOC_NO.ReadOnly = true;
        lUIControls.txtSub_No.ReadOnly = true;
        lUIControls.txtPROD_GROUP.ReadOnly = true;
        lUIControls.txtModel_R.ReadOnly = true;
        lUIControls.txtLotQty.ReadOnly = true;
        lUIControls.txtCustomer.ReadOnly = true;
        lUIControls.sbPhase.ReadOnly = true;
        lUIControls.txtLINE.ReadOnly = true;
        lUIControls.cbStartItem.Disabled = true;
        lUIControls.cbModify.Disabled = true;
        switch (stepname)
        {
            case "PM":
                //lUIControls.txtPCB.ReadOnly = true;
                //lUIControls.txtSPECRev.ReadOnly = true;
                //lUIControls.txtInputDate.ReadOnly = true;
                //lUIControls.txtBomRev.ReadOnly = true;
                //lUIControls.txtCustomerRev.ReadOnly = true;
                //lUIControls.txtPkDate.ReadOnly = true;
                lUIControls.txtWorkOrder.ReadOnly = true;
                break;
            default:
                lUIControls.txtPCB.ReadOnly = true;
                lUIControls.txtSPECRev.ReadOnly = true;
                lUIControls.txtInputDate.ReadOnly = true;
                lUIControls.txtBomRev.ReadOnly = true;
                lUIControls.txtCustomerRev.ReadOnly = true;
                lUIControls.txtPkDate.ReadOnly = true;
                lUIControls.txtWorkOrder.ReadOnly = true;
                break;
        }

    }

    protected void SetDeptWriteValueDisable(PilotRunReportUIShadow lUIControls, SPMTaskVariables SPMTaskVars)
    {
        NPIMgmt oMgmt = new NPIMgmt(oUIControls.lblSite.Text, oUIControls.lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        string stepname = SPMTaskVars.ReadDatum("STEPNAME").ToString();
        string CheckDept = oStandard.GetUserDept("A", oPara.LoginId, oUIControls.txtPROD_GROUP.Text);
        switch (stepname)
        {
            case "Dept.Write":
            case "WriteChecked":
                if (CheckDept == "IQC" && lUIControls.chkP.Checked)
                {
                    lUIControls.pnlFMEAList.Hidden = true;
                }
                else
                {
                    lUIControls.pnlFMEAList.Hidden = false;
                }
                if (GetDFXItem(lUIControls.txtSub_No.Text, stepname).Rows.Count > 0)
                {
                    lUIControls.pnlDFXList.Hidden = false;
                    lUIControls.grdDFXList.Hidden = false;

                }
                if (GetCTQInfo(lUIControls.txtDOC_NO.Text, lUIControls.txtSub_No.Text, stepname).Rows.Count > 0)
                {
                    lUIControls.grdCTQInfo.Hidden = false;
                    lUIControls.pnlCTQList.Hidden = false;

                }

                if (lUIControls.chkI.Checked)
                {
                    lUIControls.pnlIssueList.Hidden = false;
                }
                //if (lUIControls.chkP.Checked)
                //{
                //}
                break;
            case "PM":
                //lUIControls.btnDownLoad.Hidden = true;
                lUIControls.pnlIssueInfo.Hide();
                lUIControls.btnAdd.Hide();
                lUIControls.btnDelete.Hide();
                lUIControls.btnModify.Hide();
                lUIControls.btnFModify.Hide();
                lUIControls.pnlFMEABatch.Hide();
                lUIControls.pnlIssueBatch.Hide();
                lUIControls.Toolbar5.Hide();

                lUIControls.pnlFMEAWrite.Hide();
                lUIControls.btnPFMAAdd.Hide();
                lUIControls.btnPFMADelete.Hide();
                if (GetDFXItem(lUIControls.txtSub_No.Text, stepname).Rows.Count > 0)
                {
                    lUIControls.pnlDFXList.Hidden = false;
                    lUIControls.grdDFXList.Hidden = false;

                }
                if (GetCTQInfo(lUIControls.txtDOC_NO.Text, lUIControls.txtSub_No.Text, stepname).Rows.Count > 0)
                {
                    lUIControls.grdCTQInfo.Hidden = false;
                    lUIControls.pnlCTQList.Hidden = false;
                }
                if (lUIControls.chkI.Checked)
                {
                    lUIControls.pnlIssueList.Hidden = false;
                }
                if (lUIControls.chkP.Checked)
                {
                    lUIControls.pnlFMEAList.Hidden = false;

                }
                break;
            case "Dept.Reply":
                lUIControls.pnlIssueInfo.Hide();
                lUIControls.btnAdd.Hide();
                lUIControls.btnDelete.Hide();

                lUIControls.pnlFMEAWrite.Hide();
                lUIControls.btnPFMAAdd.Hide();
                lUIControls.btnPFMADelete.Hide();
                lUIControls.btnModify.Hide();
                lUIControls.btnFModify.Hide();
                lUIControls.pnlFMEABatch.Hide();
                lUIControls.pnlIssueBatch.Hide();
                lUIControls.Toolbar5.Hide();
                if (GetDFXItem(lUIControls.txtSub_No.Text, stepname).Rows.Count > 0)
                {
                    lUIControls.pnlDFXList.Hidden = false;
                    lUIControls.grdDFXList.Hidden = false;
                }

                if (GetCTQInfo(lUIControls.txtDOC_NO.Text, lUIControls.txtSub_No.Text, stepname).Rows.Count > 0)
                {
                    lUIControls.pnlCTQList.Hidden = false;
                    lUIControls.grdCTQInfo.Hidden = false;
                }
                if (GetIssuesInfo(lUIControls.txtSub_No.Text, stepname).Rows.Count > 0)
                {
                    lUIControls.pnlIssueList.Hidden = false;
                }
                if (GetPFMAInfo(lUIControls.txtSub_No.Text, stepname).Rows.Count > 0)
                {
                    lUIControls.pnlFMEAList.Hidden = false;
                } 

                break;
            case "ReplyChecked":
                lUIControls.PanelDFXScore.Hidden = false;
                lUIControls.pnlResultMain.Hidden = false;
                lUIControls.pnlReslut.Hidden = false;
                lUIControls.grdResult.Hidden = false;
                lUIControls.pnlIssueInfo.Hide();
                lUIControls.btnAdd.Hide();
                lUIControls.btnDelete.Hide();

                lUIControls.pnlFMEAWrite.Hide();
                lUIControls.btnPFMAAdd.Hide();
                lUIControls.btnPFMADelete.Hide();
                lUIControls.btnModify.Hide();
                lUIControls.btnFModify.Hide();
                lUIControls.pnlFMEABatch.Hide();
                lUIControls.pnlIssueBatch.Hide();
                lUIControls.Toolbar5.Hide();
                if (GetDFXItem(lUIControls.txtSub_No.Text, stepname).Rows.Count > 0)
                {
                    lUIControls.pnlDFXList.Hidden = false;
                    lUIControls.grdDFXList.Hidden = false;
                }

                if (GetCTQInfo(lUIControls.txtDOC_NO.Text, lUIControls.txtSub_No.Text, stepname).Rows.Count > 0)
                {
                    lUIControls.pnlCTQList.Hidden = false;
                    lUIControls.grdCTQInfo.Hidden = false;
                }
                if (GetIssuesInfo(lUIControls.txtSub_No.Text, stepname).Rows.Count > 0)
                {
                    lUIControls.pnlIssueList.Hidden = false;
                }
                if (GetPFMAInfo(lUIControls.txtSub_No.Text, stepname).Rows.Count > 0)
                {
                    lUIControls.pnlFMEAList.Hidden = false;
                }
                break;
        }
    }

    protected void SetDeptPreparedDisable(PilotRunReportUIShadow lUIControls, string StepName)
    {
        SPMTaskVariables SPMTaskVars = new SPMTaskVariables();
        NPIMgmt oMgmt = new NPIMgmt(oUIControls.lblSite.Text, oUIControls.lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        string Dept = oStandard.GetUserDept("B", oPara.LoginId, oUIControls.txtPROD_GROUP.Text);
        string[] sDept = Dept.Split(',');
        DataTable dt = oStandard.GetDFXInconformity(oUIControls.txtSub_No.Text, Dept, StepName);
        DataTable dtCLCA = oStandard.GetCLCAInconformity(oUIControls.txtSub_No.Text, Dept, StepName);
        DataTable dtIssues = oStandard.GetIssuesInconformity(oUIControls.txtSub_No.Text, Dept, "OPEN", StepName);
        DataTable dtFMEA = oStandard.GetFMEAInconformity(oUIControls.txtSub_No.Text, Dept, "144", StepName);
        DataTable dtScoreM = CheckDFXScore(lUIControls.txtSub_No.Text);

        lUIControls.pnlPrepared.Hidden = false;
        if (dt.Rows.Count > 0)
        {
            lUIControls.grdDFXInconformity.Hidden = false;
            BindGrid(oUIControls.grdDFXInconformity, dt);
        }
        if (dtCLCA.Rows.Count > 0)
        {
            lUIControls.grdCLCAInconformity.Hidden = false;
            BindGrid(oUIControls.grdCLCAInconformity, dtCLCA);
        }

        if (dtIssues.Rows.Count > 0)
        {
            lUIControls.grdIssuesInconformity.Hidden = false;
            BindGrid(oUIControls.grdIssuesInconformity, dtIssues);
        }
        if (dtFMEA.Rows.Count > 0)
        {
            lUIControls.grdFMEAInconformity.Hidden = false;
            BindGrid(oUIControls.grdFMEAInconformity, dtFMEA);
        }

        switch (StepName)
        {
            case "Dept.Prepared":
                lUIControls.pnlAttach.Hidden = false;

                foreach (string s in sDept)
                {
                    oUIControls.cobDept.Items.Add(new ListItem(s.ToString(), s.ToString()));
                }
                DataTable dtAttacment = oStandard.Get_NPI_AttachmentPR(oPara.CaseId, Dept);
                BindGrid(lUIControls.grdAttachement, dtAttacment);
                break;
            case "Dept.Checked":
                lUIControls.txtReslutDept.Text = Dept;
                //lUIControls.pnlResultMain.Hidden = false;
                //lUIControls.pnlReslut.Hidden = false;
                //lUIControls.grdResult.Hidden = false;
                lUIControls.pnlAttach.Hidden = true;
                lUIControls.btnDelAttachement.Hidden = true;
                DataTable dtAttacment_PR = oStandard.Get_NPI_AttachmentPR(oPara.CaseId, string.Empty);
                BindGrid(lUIControls.grdAttachement, dtAttacment_PR);
                DataTable dtResult = oStandard.GetNPI_Result(oPara.CaseId);
                BindGrid(lUIControls.grdResult, dtResult);

                break;
            case "PM Approver":
                lUIControls.pnlResultMain.Hidden = false;
                lUIControls.grdResult.Hidden = false;
                DataTable dtresult = oStandard.GetNPI_Result(oPara.CaseId);
                BindGrid(lUIControls.grdResult, dtresult);

                lUIControls.grdAttachmentIssue.Hidden = false;
                lUIControls.FieldSetUploadIssues.Hidden = false;
                lUIControls.pnlAttach.Hidden = true;
                lUIControls.btnDelAttachement.Hidden = true;
                DataTable dtAttacmentFile_pm = oStandard.Get_NPI_AttachmentPR(oPara.CaseId, string.Empty);//加載PR階段上傳的附件
                DataTable dtAttacmentFile_pmIssue = oStandard.Get_NPI_Attachment_Issue(oPara.CaseId);//加載PM上傳的IssueList的附件
                BindGrid(lUIControls.grdAttachement, dtAttacmentFile_pm);
                BindGrid(lUIControls.grdAttachmentIssue, dtAttacmentFile_pmIssue);

                CreateDFXScore(lUIControls.txtSub_No.Text);//生成DFX分數
                BindGrid(lUIControls.gridDFXScore, dtScoreM);
                break;
            case "TOP Manager":
            case "NPI Leader":
                lUIControls.grdAttachmentIssue.Hidden = false;
                lUIControls.btnDelVQM.Hidden = true;
                lUIControls.pnlAttach.Hidden = true;
                lUIControls.btnDelAttachement.Hidden = true;
                DataTable dtAttacmentFile = oStandard.Get_NPI_AttachmentPR(oPara.CaseId, string.Empty);
                DataTable dtAttacmentFile_pmIssue1 = oStandard.Get_NPI_Attachment_Issue(oPara.CaseId);
                BindGrid(lUIControls.grdAttachement, dtAttacmentFile);
                BindGrid(lUIControls.grdAttachmentIssue, dtAttacmentFile_pmIssue1);
                CreateDFXScore(lUIControls.txtSub_No.Text);//生成DFX分數
                BindGrid(lUIControls.gridDFXScore, dtScoreM);

                break;

        }
        if (lUIControls.sbPhase.Text.Contains("P.Run"))
        {
            lUIControls.pnlAttachment.Hidden = false;
            lUIControls.grdAttachement.Hidden = false;
        }
    }

    protected void SetTopMangerValueDisalbe(PilotRunReportUIShadow lUIControls)
    {
        lUIControls.pnlResultMain.Hidden = false;
        lUIControls.pnlReslut.Hidden = false;
        lUIControls.grdResult.Hidden = false;
        SPMTaskVariables SPMTaskVars = new SPMTaskVariables();
        NPIMgmt oMgmt = new NPIMgmt(oUIControls.lblSite.Text, oUIControls.lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        string Dept = oStandard.GetUserDept("C", oPara.LoginId, oUIControls.txtPROD_GROUP.Text);
        lUIControls.txtReslutDept.Text = Dept;
        DataTable dt = oStandard.GetNPI_Result(oPara.CaseId);
        BindGrid(lUIControls.grdResult, dt);

    }

    protected void SetTopMangerValueDisalbe_Begin(PilotRunReportUIShadow lUIControls)
    {
        lUIControls.pnlResultMain.Hidden = false;
        lUIControls.grdResult.Hidden = false;
        SPMTaskVariables SPMTaskVars = new SPMTaskVariables();
        NPIMgmt oMgmt = new NPIMgmt(oUIControls.lblSite.Text, oUIControls.lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        string Dept = oStandard.GetUserDept("C", oPara.LoginId, oUIControls.txtPROD_GROUP.Text);
        lUIControls.txtReslutDept.Text = Dept;
        DataTable dt = oStandard.GetNPI_Result(oPara.CaseId);
        BindGrid(lUIControls.grdResult, dt);

    }

    #endregion

    private void DisableField(EFFormFields FormFields, PilotRunReportUIShadow lUIControls, SPMTaskVariables SPMTaskVars, string Type)
    {

        string stepname = SPMTaskVars.ReadDatum("STEPNAME").ToString();
        string caseid = SPMTaskVars.ReadDatum("CASEID").ToString();
        //#region


    }

    public override void InitialDisableContainer(SPMTaskVariables SPMTaskVars, EFFormFields FormFields, ref object oContainer, IUIShadow UIShadow)
    {
        PilotRunReportUIShadow lUIControls = (PilotRunReportUIShadow)UIShadow;
        ShowFormDetail(FormFields, lUIControls, SPMTaskVars, "");
        base.InitialDisableContainer(SPMTaskVars, FormFields, ref oContainer, UIShadow);
    }

    // Validate contols before submit
    public override bool EFFormFieldsValidation(SPMSubmitMethod SubmitMethod, SPMProcessMethod ProcessMethod, SPMTaskVariables SPMTaskVars, ref IInterfaceHandleResult HandleResult, object oContainer, IUIShadow UIShadow)
    {
        PilotRunReportUIShadow lUIControls = (PilotRunReportUIShadow)UIShadow;
        string ErrMsg = string.Empty;
        if (SubmitMethod == SPMSubmitMethod.CreateNewCase)
        {
            #region[ check list]
            ErrMsg = BeginFormFieldsValidation(lUIControls);
            if (ErrMsg.Length > 0)
            {
                HandleResult.IsSuccess = false;
                HandleResult.CustomMessage = ErrMsg;
                return false;
            }
            #endregion
        }
        else
        {
            string stepName = (string)(SPMTaskVars.ReadDatum("STEPNAME"));
            if (stepName == "Begin" || stepName == "開始")
            {
                HandleResult.IsSuccess = false;
                HandleResult.CustomMessage = "退簽單據不可重複送簽!";
            }
            else if (stepName == "PM")
            {
                ErrMsg = PMFormFieldsValidation(lUIControls);
                if (ErrMsg.Length > 0)
                {
                    HandleResult.IsSuccess = false;
                    HandleResult.CustomMessage = ErrMsg;
                }
            }
            else if (stepName == "Dept.Write" || stepName == "WriteChecked")
            {
                #region
                ErrMsg = DeptFormFieldsValidate(lUIControls, SPMTaskVars);
                if (ErrMsg.Length > 0)
                {
                    HandleResult.IsSuccess = false;
                    HandleResult.CustomMessage = ErrMsg;
                }
                #endregion
            }
            else if (stepName == "Dept.Reply")
            {
                #region
                ErrMsg = DeptFormFieldsValidate_Reply(lUIControls, SPMTaskVars);
                if (ErrMsg.Length > 0)
                {
                    HandleResult.IsSuccess = false;
                    HandleResult.CustomMessage = ErrMsg;
                }
                #endregion
            }
            else if (stepName == "Dept.Prepared")
            {
                #region
                ErrMsg = DeptFormFieldsValidate_Prepared(lUIControls, SPMTaskVars);
                if (ErrMsg.Length > 0)
                {
                    HandleResult.IsSuccess = false;
                    HandleResult.CustomMessage = ErrMsg;
                }
                #endregion
            }
            else if (stepName == "PM Approver")
            {
                #region
                ErrMsg = PMCheck(lUIControls, SPMTaskVars);
                if (ErrMsg.Length > 0)
                {
                    HandleResult.IsSuccess = false;
                    HandleResult.CustomMessage = ErrMsg;
                }
                #endregion
            }
                //|| stepName == "Dept.Checked"
            else if (stepName == "TOP Manager" || stepName == "ReplyChecked" || stepName == "NPI Leader" )
            {
                ErrMsg = ValidationManagerApprover(lUIControls);
                if (ErrMsg.Length > 0)
                {
                    HandleResult.IsSuccess = false;
                    HandleResult.CustomMessage = ErrMsg;
                }
            }
        }
        return base.EFFormFieldsValidation(SubmitMethod, ProcessMethod, SPMTaskVars, ref HandleResult, oContainer, UIShadow);
    }

    // Fill SPM's EFFormFieldData
    public override void PrepareEFFormFields(SPMSubmitMethod SubmitMethod, SPMProcessMethod ProcessMethod, SPMTaskVariables TaskVars, ref EFFormFields FormFields, ref IInterfaceHandleResult HandleResult, object oContainer, IUIShadow UIShadow, ref string ApplicantInfo)
    {
        PilotRunReportUIShadow lUIControls = (PilotRunReportUIShadow)UIShadow;
        FormFields.SetOrAdd("txtDoc_No".ToUpper(), lUIControls.txtDOC_NO.Text);
        FormFields.SetOrAdd("txtSub_No".ToUpper(), lUIControls.txtSub_No.Text);
        FormFields.SetOrAdd("txtSite".ToUpper(), lUIControls.lblSite.Text);
        FormFields.SetOrAdd("txtBu".ToUpper(), lUIControls.lblBu.Text);
        FormFields.SetOrAdd("txtBuilding".ToUpper(), lUIControls.lblBuilding.Text);
        FormFields.SetOrAdd("txtPhase".ToUpper(), lUIControls.sbPhase.SelectedItem.Text);
        base.PrepareEFFormFields(SubmitMethod, ProcessMethod, TaskVars, ref FormFields, ref HandleResult, oContainer, UIShadow, ref ApplicantInfo);
    }

    // Fill SPM Variable 
    public override void PrepareSPMVariables(SPMSubmitMethod SubmitMethod, SPMProcessMethod ProcessMethod, SPMTaskVariables SPMTaskVars, ref SPMVariables Variables, ref SPMRoutingVariable RoutingVariable, ref string strSPMUid, string strMemo, string strNotesForNextApprover, EFFormFields FormFields, ref IInterfaceHandleResult HandleResult, ref string SuccessMessage)
    {
        bool isError = false;
        string stepName = string.Empty;
        string errorMsg = string.Empty;
        string DeptWriteMember = string.Empty;
        string DeptMember = string.Empty;
        string doc_No = FormFields["txtDoc_No".ToUpper()];
        string sub_No = FormFields["txtSub_No".ToUpper()];
        string phase = FormFields["txtPhase".ToUpper()];
        Variables.Add(SPMVariableKey.Subject, "[NPIGatingReport] [" + oUIControls.txtModel_R.Text + "] [" + oUIControls.sbPhase.SelectedItem.Text + "] [" + sub_No + "]");
        string Approver = string.Empty;

        //取得單據的當前關卡簽核人
        if (SubmitMethod == SPMSubmitMethod.CreateNewCase)
        {
            if (oUIControls.cbModify.Checked)
            {
                Approver = GetDeptWriteMember(doc_No, "WriteEname", "D");
                RoutingVariable = new SPMRoutingVariable(SPMRoutingVariableKey.spm_Jump, "PM(" + Approver + ")");
            }
            else
            {
                DeptWriteMember = GetDeptWriteMember(doc_No, "WriteEname", "A");
                if (string.IsNullOrEmpty(DeptWriteMember))
                {
                    isError = true;
                    errorMsg += "試產單部門人員未維護!";
                }
                RoutingVariable = new SPMRoutingVariable(SPMRoutingVariableKey.spm_Jump, "Dept.Write(" + DeptWriteMember + ")");
            }
        }
        else
        {
            stepName = SPMTaskVars.ReadDatum("STEPNAME").ToString();


            switch (stepName)
            {
                case "Dept.Write":
                    //Approver = GetDeptWriteMember(doc_No, "CheckedEname", "A");
                    //RoutingVariable = new SPMRoutingVariable(SPMRoutingVariableKey.spm_Jump, "WriteChecked(" + Approver + ")");
                    Approver = GetDeptWriteMember(doc_No, "WriteEname", "D");
                    RoutingVariable = new SPMRoutingVariable(SPMRoutingVariableKey.spm_Assign, "PM(" + Approver + ")");
                    break;
                case "WriteChecked":
                    Approver = GetDeptWriteMember(doc_No, "WriteEname", "D");
                    RoutingVariable = new SPMRoutingVariable(SPMRoutingVariableKey.spm_Jump, "PM(" + Approver + ")");
                    break;

                case "PM":
                    if (oUIControls.cbModify.Checked)
                    {

                        Approver = GetDeptWriteMember(doc_No, "WriteEname", "B");
                        RoutingVariable = new SPMRoutingVariable(SPMRoutingVariableKey.spm_Jump, "Dept.Prepared(" + Approver + ")");
                    }
                    else
                    {
                        Approver = GetDeptWriteMember(doc_No, "ReplyEName", "A");
                        RoutingVariable = new SPMRoutingVariable(SPMRoutingVariableKey.spm_Jump, "Dept.Reply(" + Approver + ")");
                    }
                    break;
                case "Dept.Reply":

                    Approver = GetDeptWriteMember(doc_No, "CheckedEname", "A");
                    RoutingVariable = new SPMRoutingVariable(SPMRoutingVariableKey.spm_Jump, "ReplyChecked(" + Approver + ")");
                    break;
                case "ReplyChecked":
                    if (phase.Contains("EVT") || phase.Contains("DVT"))
                    {

                        Approver = GetPMApprove(doc_No, "D");
                        RoutingVariable = new SPMRoutingVariable(SPMRoutingVariableKey.spm_Assign, "PM Approver(" + Approver + ")");
                    }
                    else
                    {
                        Approver = GetDeptWriteMember(doc_No, "WriteEname", "B");
                        RoutingVariable = new SPMRoutingVariable(SPMRoutingVariableKey.spm_Assign, "Dept.Prepared(" + Approver + ")");
                    }
                    break;
                case "Dept.Prepared":
                    Approver = GetDeptWriteMember(doc_No, "CheckedEname", "B");
                    RoutingVariable = new SPMRoutingVariable(SPMRoutingVariableKey.spm_Jump, "Dept.Checked(" + Approver + ")");
                    break;
                case "Dept.Checked":
                    Approver = GetPMApprove(doc_No, "D");
                    RoutingVariable = new SPMRoutingVariable(SPMRoutingVariableKey.spm_Jump, "PM Approver(" + Approver + ")");
                    break;
                case "PM Approver":
                    Approver = GetDeptWriteMember(doc_No, "WriteEname", "C");
                    RoutingVariable = new SPMRoutingVariable(SPMRoutingVariableKey.spm_Jump, "NPI Leader(" + Approver + ")");
                    break;
                case "NPI Leader":
                    Approver = GetDeptTopmANAGER(doc_No, "C");
                    RoutingVariable = new SPMRoutingVariable(SPMRoutingVariableKey.spm_Jump, "TOP Manager(" + Approver + ")");
                    break;
                    break;
                case "TOP Manager":
                    RoutingVariable = new SPMRoutingVariable(SPMRoutingVariableKey.spm_Jump, "End");
                    break;

            }
        }

        if (isError == true)
        {
            HandleResult.IsSuccess = false;
            HandleResult.CustomMessage = errorMsg;
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

        if (RoutingVariable != null)
        {
            string sRoutingData = string.Empty;
            switch (RoutingVariable.Key)
            {
                case SPMRoutingVariableKey.spm_Return:

                    sRoutingData = RoutingVariable.Data;

                    break;
                case SPMRoutingVariableKey.spm_Jump:


                    break;
            }
        }

        string doc_No = FormFields["txtDoc_No".ToUpper()];
        string sub_No = FormFields["txtSub_No".ToUpper()];
        // Business logic
        if (SubmitMethod != SPMSubmitMethod.CreateNewCase)
        {
            SPMAfterSend_DBIO(SPMTaskVars, FormFields, ref HandleResult, RoutingVariable);
        }
        else
        {
            string NeedStartInfo = GetNeedStartInfo(oUIControls);
            NPI_Standard oStandard = new NPI_Standard(oUIControls.lblSite.Text, oUIControls.lblBu.Text);
            Model_NPI_APP_SUB oModel_AppSUB = new Model_NPI_APP_SUB();
            oModel_AppSUB._DOC_NO = oUIControls.txtDOC_NO.Text;
            oModel_AppSUB._SUB_DOC_NO = oUIControls.txtSub_No.Text;
            if (oUIControls.sbPhase.SelectedItem.Text.Contains("EVT"))
            {
                oModel_AppSUB._SUB_DOC_PHASE_A = "EVT";
            }
            else if (oUIControls.sbPhase.SelectedItem.Text.Contains("DVT"))
            {
                oModel_AppSUB._SUB_DOC_PHASE_A = "DVT";
            }
            else if (oUIControls.sbPhase.SelectedItem.Text.Contains("P.Run"))
            {
                oModel_AppSUB._SUB_DOC_PHASE_A = "PR";
            }
            else
            {
                oModel_AppSUB._SUB_DOC_PHASE_A = "PTT";
            }
            oModel_AppSUB._SUB_DOC_PHASE = oUIControls.sbPhase.SelectedItem.Text;
            oModel_AppSUB._WorkOrder = oUIControls.txtWorkOrder.Text;
            oModel_AppSUB._SUB_DOC_PHASE_RESULT = string.Empty;
            oModel_AppSUB._SUB_DOC_PHASE_STATUS = string.Empty;
            oModel_AppSUB._SUB_DOC_PHASE_VERSION = 0;
            oModel_AppSUB._UPDATE_TIME = DateTime.Now;
            oModel_AppSUB._UPDATE_USERID = oPara.LoginId;
            oModel_AppSUB._Building = oUIControls.sbBuilding.Text;
            oModel_AppSUB._CREATE_DATE = DateTime.Now;
            oModel_AppSUB._LOT_QTY = int.Parse(oUIControls.txtLotQty.Text);
            oModel_AppSUB._PCB_REV = oUIControls.txtPCB.Text;
            oModel_AppSUB._SPEC_REV = oUIControls.txtSPECRev.Text;
            //oModel_AppSUB._ISSUE_DATE = DateTime.Today;
            oModel_AppSUB._INPUT_DATE = oUIControls.txtInputDate.SelectedDate.ToString("yyyy-MM-dd"); ;
            oModel_AppSUB._CUSTOMER = oUIControls.txtCustomer.Text;
            oModel_AppSUB._LINE = oUIControls.txtLINE.Text;
            oModel_AppSUB._BOM_REV = oUIControls.txtBomRev.Text;
            oModel_AppSUB._CUSTOMER_REV = oUIControls.txtCustomerRev.Text;
            oModel_AppSUB._PK_DATE = oUIControls.txtPkDate.SelectedDate.ToString("yyyy-MM-dd");
            oModel_AppSUB._PROD_GROUP = oUIControls.txtPROD_GROUP.Text;
            oModel_AppSUB._NeedStartItmes = NeedStartInfo;
            oModel_AppSUB._DFX_STATUS = oUIControls.chkD.Checked ? "Add" : string.Empty;
            oModel_AppSUB._CTQ_STATUS = oUIControls.chkC.Checked ? "Add" : string.Empty;
            oModel_AppSUB._ISSUES_STATUS = oUIControls.chkI.Checked ? "Add" : string.Empty;
            oModel_AppSUB._PFMA_STATUS = oUIControls.chkP.Checked ? "Add" : string.Empty;
            oModel_AppSUB._MODIFYFLAG = oUIControls.cbModify.Checked ? "Y" : "N";

            oModel_AppSUB._REMARKM = oUIControls.txtRemarkM.Text;
            
            oModel_AppSUB._CASEID = int.Parse(System.Web.HttpUtility.UrlDecode(Variables[SPMVariableKey.CaseId]));
            try
            {

                Dictionary<string, object> result = oStandard.RecordOperation_APP_SUB(oModel_AppSUB, Status_Operation.ADD);
                if ((bool)result["Result"])
                {

                }
                else
                {
                    HandleResult.IsSuccess = false;
                    HandleResult.CustomMessage = (string)result["ErrMsg"];
                }
            }
            catch (Exception ex)
            {
                HandleResult.IsSuccess = false;
                HandleResult.CustomMessage = ex.ToString();
            }
        }
        base.SPMAfterSend(SubmitMethod, SPMTaskVars, Variables, RoutingVariable, FormFields, ref HandleResult);
    }

    private void SPMAfterSend_DBIO(SPMTaskVariables SPMTaskVars, EFFormFields FormFields, ref IInterfaceHandleResult HandleResult, SPMRoutingVariable RoutingVariable)
    {

        try
        {
            string stepName = (string)SPMTaskVars.ReadDatum("STEPNAME");
            string caseid = SPMTaskVars.ReadDatum("CASEID").ToString();
            //string doc_No = FormFields["txtDoc_No".ToUpper()];
            //string sub_No = FormFields["txtSub_No".ToUpper()];

            switch (RoutingVariable.Key)
            {
                case SPMRoutingVariableKey.spm_Recall:
                    //UpdateMasterStatus("Abort", oUIControls.txtSub_No.Text);
                    break;
                case SPMRoutingVariableKey.spm_Jump:
                    switch (stepName)
                    {
                        case "Dept.Write":
                        case "WriteChecked":    //當前部門沒有DFX CTQ項目 Issue Fmea為NA,跳過Reply ReplyChecked關卡
                            NPI_Standard oStandard2 = new NPI_Standard(oUIControls.lblSite.Text, oUIControls.lblBu.Text);
                            Model_NPI_APP_SUB oModel_AppSUB2 = new Model_NPI_APP_SUB();
                            string Dept = oStandard2.GetUserDept("A", oPara.LoginId, oUIControls.txtPROD_GROUP.Text);
                            string[] DeptCodes = Dept.Split(',');
                            for (int i = 0; i < DeptCodes.Length; i++)//抓取部門 做循環處理 檢查CTQ ISSUE FMEA是否存在當前部門的資料 沒有則Update isreply
                            {
                                string Code = DeptCodes[i];
                                DataTable dtCTQR = GetCTQlist(oUIControls.txtSub_No.Text, Code);
                                DataTable dtIssueR = GetIssuelist(oUIControls.txtSub_No.Text, Code);
                                DataTable dtFMEAR = GetFMEAlist(oUIControls.txtSub_No.Text, Code);
                                if (dtCTQR.Rows.Count == 0 && dtIssueR.Rows.Count == 0 && dtFMEAR.Rows.Count == 0)
                                {
                                    UpdateIsReply(oUIControls.txtPROD_GROUP.Text, "N", Code);
                                    Model_NPI_APP_RESULT Result = new Model_NPI_APP_RESULT();
                                    string APPROVER = oPara.LoginId;
                                    string APPROVER_OPINION = "";
                                    string APPROVER_Levels = "ReplyChecked";
                                    string SUB_DOC_NO = oUIControls.txtSub_No.Text;
                                    int CASEID = int.Parse(caseid);
                                    string DEPT = Code;
                                    InsertReplyResult(SUB_DOC_NO, CASEID, Dept, APPROVER, "PASS", APPROVER_OPINION, APPROVER_Levels);
                                }
                                else
                                {
                                    UpdateIsReply(oUIControls.txtPROD_GROUP.Text, "Y", Code);
                                }
                            }
                            break;
                        case "PM":
                            NPI_Standard oStandard = new NPI_Standard(oUIControls.lblSite.Text, oUIControls.lblBu.Text);
                            Model_NPI_APP_SUB oModel_AppSUB = new Model_NPI_APP_SUB();
                            oModel_AppSUB._DOC_NO = oUIControls.txtDOC_NO.Text;
                            oModel_AppSUB._SUB_DOC_NO = oUIControls.txtSub_No.Text;
                            oModel_AppSUB._SUB_DOC_PHASE = oUIControls.sbPhase.SelectedItem.Text;
                            if (oUIControls.sbPhase.SelectedItem.Text.Contains("EVT"))
                            {
                                oModel_AppSUB._SUB_DOC_PHASE_A = "EVT";
                            }
                            else if (oUIControls.sbPhase.SelectedItem.Text.Contains("DVT"))
                            {
                                oModel_AppSUB._SUB_DOC_PHASE_A = "DVT";
                            }
                            else if (oUIControls.sbPhase.SelectedItem.Text.Contains("P.Run"))
                            {
                                oModel_AppSUB._SUB_DOC_PHASE_A = "PR";
                            }
                            else
                            {
                                oModel_AppSUB._SUB_DOC_PHASE_A = "PTT";
                            }
                            oModel_AppSUB._PCB_REV = oUIControls.txtPCB.Text;
                            oModel_AppSUB._SPEC_REV = oUIControls.txtSPECRev.Text;
                            oModel_AppSUB._BOM_REV = oUIControls.txtBomRev.Text;
                            oModel_AppSUB._CUSTOMER_REV = oUIControls.txtCustomerRev.Text;
                            oModel_AppSUB._PK_DATE = oUIControls.txtPkDate.SelectedDate.ToString("yyyy-MM-dd");
                            oModel_AppSUB._INPUT_DATE = oUIControls.txtInputDate.SelectedDate.ToString("yyyy-MM-dd");
                            oModel_AppSUB._CASEID = int.Parse(caseid);
                            try
                            {

                                Dictionary<string, object> result = oStandard.RecordOperation_APP_SUB(oModel_AppSUB, Status_Operation.UPDATE);
                                if ((bool)result["Result"])
                                {

                                }
                                else
                                {
                                    HandleResult.IsSuccess = false;
                                    HandleResult.CustomMessage = (string)result["ErrMsg"];
                                }
                            }
                            catch (Exception ex)
                            {
                                HandleResult.IsSuccess = false;
                                HandleResult.CustomMessage = ex.ToString();
                            }
                            break;
                        case "NPI Leader":
                        //case "Dept.Checked":
                            NPIMgmt oNPI_Mgmt1 = new NPIMgmt(oUIControls.lblSite.Text, oUIControls.lblBu.Text);
                            //NPI_Standard oNPI_Standard1 = oNPI_Mgmt1.InitialLeaveMgmt();
                            NPI_Standard oNPI_Standard1 = oNPI_Mgmt1.InitialLeaveMgmt();
                            Model_NPI_APP_RESULT oModel_NPI_Result1 = new Model_NPI_APP_RESULT();
                            oModel_NPI_Result1._APPROVER = oPara.LoginId;
                            oModel_NPI_Result1._APPROVER_OPINION = oUIControls.txtReslutOpinion.Text;
                            oModel_NPI_Result1._APPROVER_Levels = stepName;
                            oModel_NPI_Result1._SUB_DOC_NO = oUIControls.txtSub_No.Text;
                            oModel_NPI_Result1._CASEID = int.Parse(caseid);
                            oModel_NPI_Result1._DEPT = oUIControls.txtReslutDept.Text;
                            for (int i = 0; i < oUIControls.rgResult.Items.Count; i++)
                            {
                                if (oUIControls.rgResult.Items[i].Checked == true)
                                {
                                    oModel_NPI_Result1._APPROVER_RESULT = oUIControls.rgResult.Items[i].BoxLabel.Trim();
                                }
                            }
                            DataTable dt1 = GetDOAResult(oModel_NPI_Result1._CASEID, oModel_NPI_Result1._APPROVER, oModel_NPI_Result1._DEPT,oModel_NPI_Result1._APPROVER_Levels);
                            if (dt1.Rows.Count > 0)
                            {
                                Dictionary<string, object> result = oNPI_Standard1.RecordOperation_Result(oModel_NPI_Result1, Status_Operation.UPDATE);
                            }
                            else
                            {
                                Dictionary<string, object> result = oNPI_Standard1.RecordOperation_Result(oModel_NPI_Result1, Status_Operation.ADD);
                            }
                            break;
                        case "TOP Manager":
                            NPIMgmt oNPI_Mgmt = new NPIMgmt(oUIControls.lblSite.Text, oUIControls.lblBu.Text);
                            NPI_Standard oNPI_Standard = oNPI_Mgmt.InitialLeaveMgmt();
                            Model_NPI_APP_RESULT oModel_NPI_Result = new Model_NPI_APP_RESULT();
                            oModel_NPI_Result._APPROVER = oPara.LoginId;
                            oModel_NPI_Result._APPROVER_OPINION = oUIControls.txtReslutOpinion.Text;
                            oModel_NPI_Result._APPROVER_Levels = stepName;
                            oModel_NPI_Result._SUB_DOC_NO = oUIControls.txtSub_No.Text;
                            oModel_NPI_Result._CASEID = int.Parse(caseid);
                            oModel_NPI_Result._DEPT = oUIControls.txtReslutDept.Text;
                            for (int i = 0; i < oUIControls.rgResult.Items.Count; i++)
                            {
                                if (oUIControls.rgResult.Items[i].Checked == true)
                                {
                                    oModel_NPI_Result._APPROVER_RESULT = oUIControls.rgResult.Items[i].BoxLabel.Trim();
                                    UpdateMasterStatus("Finished", oUIControls.txtSub_No.Text, oModel_NPI_Result._APPROVER_RESULT);

                                }
                            }
                            DataTable dt = GetDOAResult(oModel_NPI_Result._CASEID, oModel_NPI_Result._APPROVER, oModel_NPI_Result._DEPT,oModel_NPI_Result._APPROVER_Levels);
                            if (dt.Rows.Count > 0)
                            {
                                Dictionary<string, object> result = oNPI_Standard.RecordOperation_Result(oModel_NPI_Result, Status_Operation.UPDATE);
                            }
                            else
                            {
                                Dictionary<string, object> result = oNPI_Standard.RecordOperation_Result(oModel_NPI_Result, Status_Operation.ADD);
                            }
                            break;

                    }
                    break;
                case SPMRoutingVariableKey.spm_Assign:
                    switch (stepName)
                    {
                        case "ReplyChecked":
                            NPIMgmt oNPI_Mgmt1 = new NPIMgmt(oUIControls.lblSite.Text, oUIControls.lblBu.Text);
                            //NPI_Standard oNPI_Standard1 = oNPI_Mgmt1.InitialLeaveMgmt();
                            NPI_Standard oNPI_Standard1 = oNPI_Mgmt1.InitialLeaveMgmt();
                            Model_NPI_APP_RESULT oModel_NPI_Result1 = new Model_NPI_APP_RESULT();
                            oModel_NPI_Result1._APPROVER = oPara.LoginId;
                            oModel_NPI_Result1._APPROVER_OPINION = oUIControls.txtReslutOpinion.Text;
                            oModel_NPI_Result1._APPROVER_Levels = stepName;
                            oModel_NPI_Result1._SUB_DOC_NO = oUIControls.txtSub_No.Text;
                            oModel_NPI_Result1._CASEID = int.Parse(caseid);
                            oModel_NPI_Result1._DEPT = oUIControls.txtReslutDept.Text;
                            for (int i = 0; i < oUIControls.rgResult.Items.Count; i++)
                            {
                                if (oUIControls.rgResult.Items[i].Checked == true)
                                {
                                    oModel_NPI_Result1._APPROVER_RESULT = oUIControls.rgResult.Items[i].BoxLabel.Trim();
                                }
                            }
                            DataTable dt1 = GetDOAResult(oModel_NPI_Result1._CASEID, oModel_NPI_Result1._APPROVER, oModel_NPI_Result1._DEPT,oModel_NPI_Result1._APPROVER_Levels);
                            if (dt1.Rows.Count > 0)
                            {
                                Dictionary<string, object> result = oNPI_Standard1.RecordOperation_Result(oModel_NPI_Result1, Status_Operation.UPDATE);
                            }
                            else
                            {
                                Dictionary<string, object> result = oNPI_Standard1.RecordOperation_Result(oModel_NPI_Result1, Status_Operation.ADD);
                            }
                            break;
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
            PilotRunReportUIShadow lControls = (PilotRunReportUIShadow)UIShadow;
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


    private string GetDeptWriteMember(string DocNo, string Category, string CategoryFlag)
    {
        string member = string.Empty;
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT DISTINCT " + Category + " FROM TB_NPI_APP_MEMBER WHERE DOC_NO=@DocNo");
        sb.Append("  AND CategoryFlag=@CategoryFlag");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DocNo", DbType.String, DocNo));
        opc.Add(DataPara.CreateDataParameter("@CategoryFlag", DbType.String, CategoryFlag));
        DataTable dt = sdb.TransactionExecute(sb.ToString(), opc);
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                member += dr[Category].ToString() + ",";
            }
            return member.Substring(0, member.Length - 1);
        }
        else
        {
            return "";
        }
    }

    private string GetDeptTopmANAGER(string DocNo, string CategoryFlag)
    {
        string member = string.Empty;
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT DISTINCT ReplyEName, CheckedEName FROM TB_NPI_APP_MEMBER WHERE DOC_NO=@DocNo");
        sb.Append("  AND CategoryFlag=@CategoryFlag");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DocNo", DbType.String, DocNo));
        opc.Add(DataPara.CreateDataParameter("@CategoryFlag", DbType.String, CategoryFlag));
        DataTable dt = sdb.TransactionExecute(sb.ToString(), opc);
        member = dt.Rows[0]["ReplyEName"].ToString() + "," + dt.Rows[0]["CheckedEName"].ToString();
        return member;
    }

    private string GetPMApprove(string DocNo, string CategoryFlag)
    {
        string member = string.Empty;
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT DISTINCT WriteEname, CheckedEName FROM TB_NPI_APP_MEMBER WHERE DOC_NO=@DocNo");
        sb.Append("  AND CategoryFlag=@CategoryFlag");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DocNo", DbType.String, DocNo));
        opc.Add(DataPara.CreateDataParameter("@CategoryFlag", DbType.String, CategoryFlag));
        DataTable dt = sdb.TransactionExecute(sb.ToString(), opc);
        member = dt.Rows[0]["WriteEname"].ToString() + "," + dt.Rows[0]["CheckedEName"].ToString();
        return member;
    }

    protected void BindGrid(GridPanel grd, DataTable dt)
    {
        grd.Store.Primary.DataSource = dt;
        grd.Store.Primary.DataBind();
    }

    protected void BindDept()
    {
        SPMTaskVariables SPMTaskVars = new SPMTaskVariables();
        NPIMgmt oMgmt = new NPIMgmt(oUIControls.lblSite.Text, oUIControls.lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        Model_APPLICATION_PARAM oModel_Param = new Model_APPLICATION_PARAM();
        oModel_Param._APPLICATION_NAME = "NPI_REPORT";
        oModel_Param._FUNCTION_NAME = "Configuration";
        oModel_Param._PARAME_NAME = "SubscribeDept";
        oModel_Param._PARAME_ITEM = oUIControls.lblSite.Text.Trim();
        oModel_Param._PARAME_VALUE1 = oUIControls.lblBu.Text.Trim();
        DataTable dt = oStandard.PARAME_GetBasicData_Filter(oModel_Param);
        BindData(oUIControls.cobDept, dt);

    }

    private void BindData(ComboBox cmb, DataTable dt)
    {
        cmb.Store.Primary.DataSource = dt;
        cmb.Store.Primary.DataBind();
    }

    private void UpdateMasterStatus(string Status, string subNo,string Result)
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        string sql = " update TB_NPI_APP_SUB SET Result=@Result,Status = @Status where SUB_DOC_NO=@SubNo";
        ArrayList opc = new ArrayList();
        opc.Add(DataPara.CreateDataParameter("@Status", DbType.String, Status));
        opc.Add(DataPara.CreateDataParameter("@Result", DbType.String, Result));
        opc.Add(DataPara.CreateDataParameter("@SubNo", DbType.String, subNo));
        sdb.TransactionExecute(sql, opc);

    }

    #region BeginFormFiledsValid

    protected string BeginFormFieldsValidation(IUIShadow UIShadow)
    {
        PilotRunReportUIShadow lUIControls = (PilotRunReportUIShadow)UIShadow;
        StringBuilder sbErrMsg = new StringBuilder();
        string doc_no = lUIControls.txtDOC_NO.Text;
        if (doc_no.Length == 0)
        {

            sbErrMsg.Append("表單編號不能為空!</br>");
        }
        if (lUIControls.sbPhase.SelectedIndex < 0)
        {
            sbErrMsg.Append("請選擇階段!</br>");
        }
        if (lUIControls.txtWorkOrder.Text.Length < 0)
        {
            sbErrMsg.Append("請填寫工單號!</br>");
        }
        if (lUIControls.txtPCB.Text.Length == 0)
        {
            sbErrMsg.Append("請填寫PCB!</br>");
        }
        if (lUIControls.txtSPECRev.Text.Length == 0)
        {
            sbErrMsg.Append("請填寫SPECRev!</br>");
        }
        if (lUIControls.txtInputDate.Text == "1/1/0001 12:00:00 AM")
        {
            sbErrMsg.Append("請填寫InputDate!</br>");
        }
        if (lUIControls.txtBomRev.Text.Length == 0)
        {
            sbErrMsg.Append("請填寫BomRev!</br>");
        }
        if (lUIControls.txtCustomerRev.Text.Length == 0)
        {
            sbErrMsg.Append("請填寫CustomerRev!</br>");
        }
        if (lUIControls.txtPkDate.Text == "1/1/0001 12:00:00 AM")
        {
            sbErrMsg.Append("請填寫PkDate!</br>");
        }
        if (!lUIControls.cbModify.Checked)
        {
            if (CheckNeedStartInfo(lUIControls))
            {

                sbErrMsg.Append("请勾选需启动项!</br>");

            }
        }
        return sbErrMsg.ToString();
    }

    private bool CheckNeedStartInfo(IUIShadow UIShadow)
    {
        PilotRunReportUIShadow lUIControls = (PilotRunReportUIShadow)UIShadow;
        int count = 0;

        for (int i = 0; i < lUIControls.cbStartItem.Items.Count; i++)
        {
            if (lUIControls.cbStartItem.Items[i].Checked == false)
            {
                count += 1;
            }
        }
        if (count >= lUIControls.cbStartItem.Items.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private string GetNeedStartInfo(IUIShadow UIShadow)
    {
        PilotRunReportUIShadow lUIControls = (PilotRunReportUIShadow)UIShadow;
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < lUIControls.cbStartItem.Items.Count; i++)
        {
            if (lUIControls.cbStartItem.Items[i].Checked == true)
            {
                sb.AppendFormat("{0};", lUIControls.cbStartItem.Items[i].InputValue);
            }
        }
        return sb.ToString();
    }
    #endregion

    protected string PMFormFieldsValidation(IUIShadow UIShadow)
    {
        PilotRunReportUIShadow lUIControls = (PilotRunReportUIShadow)UIShadow;
        StringBuilder sbErrMsg = new StringBuilder();

        if (string.IsNullOrEmpty(lUIControls.txtPCB.Text))
        {
            sbErrMsg.Append("PCB Rev不能為空！</br>");
        }
        if (string.IsNullOrEmpty(lUIControls.txtSPECRev.Text))
        {
            sbErrMsg.Append("SPEC Rev不能為空！</br>");
        }
        if (string.IsNullOrEmpty(lUIControls.txtInputDate.Text))
        {
            sbErrMsg.Append("InputDate不能為空！</br>");
        }

        if (string.IsNullOrEmpty(lUIControls.txtBomRev.Text))
        {
            sbErrMsg.Append("Bom Rev不能為空！</br>");
        }
        if (string.IsNullOrEmpty(lUIControls.txtCustomerRev.Text))
        {
            sbErrMsg.Append("Customer Rev不能為空！</br>");
        }
        if (string.IsNullOrEmpty(lUIControls.txtPkDate.Text))
        {
            sbErrMsg.Append("PkDate Rev不能為空！</br>");
        }

        return sbErrMsg.ToString();

    }

    protected string ValidationManagerApprover(IUIShadow UIShadow)
    {
        PilotRunReportUIShadow lUIControls = (PilotRunReportUIShadow)UIShadow;
        string sbErrMsg = string.Empty;
        if (!(lUIControls.rdResultY.Checked || lUIControls.rdResultN.Checked || lUIControls.rdReulutCondition.Checked))
        {
            sbErrMsg = "請勾選簽核結果!";
        }
        if (lUIControls.rdResultN.Checked || lUIControls.rdReulutCondition.Checked)
        {
            if (lUIControls.txtReslutOpinion.Text.Length == 0)
            {
                sbErrMsg = "Fail或Condition Pass時,備註信息不能為空!";
            }
        }
        return sbErrMsg;
    }

    protected string PMCheck(IUIShadow UIShadow, SPMTaskVariables SPMTaskVars)
    {
        PilotRunReportUIShadow lUIControls = (PilotRunReportUIShadow)UIShadow;
        string sbErrMsg = string.Empty;
        string caseid = SPMTaskVars.ReadDatum("CASEID").ToString();
        DataTable dt = GetPM_Issues(int.Parse(caseid));
        if (dt.Rows.Count == 0)
        {
            sbErrMsg = "請上傳Issues List附件";
        }
        return sbErrMsg;
    }

    #region 部門填寫FormFiedsValid Write / Reply  / Prepared

    protected string DeptFormFieldsValidate(PilotRunReportUIShadow lUIControls, SPMTaskVariables SPMTaskVars)
    {
        NPIMgmt oMgmt = new NPIMgmt(oUIControls.lblSite.Text, oUIControls.lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        string CheckDept = oStandard.GetUserDept("A", oPara.LoginId, oUIControls.txtPROD_GROUP.Text);
        string stepname = SPMTaskVars.ReadDatum("STEPNAME").ToString();
        StringBuilder ErrMsg = new StringBuilder();
        DataTable dt = GetDFXItem(lUIControls.txtSub_No.Text, stepname);
        DataTable dtCTQ = GetCTQInfo(lUIControls.txtDOC_NO.Text, lUIControls.txtSub_No.Text, stepname);
        DataTable dtIssues = GetIssuesInfo(lUIControls.txtSub_No.Text, stepname);
        DataTable dtFMEA = GetPFMAInfo(lUIControls.txtSub_No.Text, stepname);
        if (dt.Rows.Count > 0)
        {
            DataRow[] rowArray = dt.Select(" STATUS is null or Status='' or Status='Write' ");
            if (rowArray.Length > 0)
            {
                ErrMsg.Append("DFX未填寫完畢!</br>");
            }
            DataRow[] rowArray2 = dt.Select(" Compliance='N' AND (len(Comments)<=0 or Comments is null or len(Losses)<=0 or Losses is null or len(Location)<=0 or Location is null)");
            if (rowArray2.Length > 0)
            {
                ErrMsg.Append("Compliance 為N時,請填寫Losses、Location及Comments!</br>");
            }
        }
        if (dtCTQ.Rows.Count > 0)
        {
            DataRow[] rowArrayCTQ = dtCTQ.Select(" STATUS is null or STATUS='' OR STATUS='Write' ");
            if (rowArrayCTQ.Length > 0)
            {
                ErrMsg.Append("CTQ未填寫完畢!</br>");
            }
        }
        if (lUIControls.chkI.Checked)
        {
            if (dtIssues.Rows.Count == 0 && lUIControls.rdPBatchIssueNA.Checked == false)
            {
                ErrMsg.Append("ISSUES List未填寫,若无内容,请选择NA!</br>");
            }
        }
        if (lUIControls.chkP.Checked && CheckDept != "IQC")
        {
            if (dtFMEA.Rows.Count == 0 && lUIControls.rdFMEABatchNA.Checked == false)
            {
                ErrMsg.Append("FMEA List未填寫,若无内容,请选择NA!</br>");
            }
        }
        return ErrMsg.ToString();
    }

    protected string DeptFormFieldsValidate_Reply(PilotRunReportUIShadow lUIControls, SPMTaskVariables SPMTaskVars)
    {
        string stepname = SPMTaskVars.ReadDatum("STEPNAME").ToString();
        StringBuilder ErrMsg = new StringBuilder();
        DataTable dt = GetDFXItem(lUIControls.txtSub_No.Text, stepname);
        DataTable dtCTQ = GetCTQInfo(lUIControls.txtDOC_NO.Text, lUIControls.txtSub_No.Text, stepname);
        DataTable dtIssues = GetIssuesInfo(lUIControls.txtSub_No.Text, stepname);
        DataTable dtFMEA = GetPFMAInfo(lUIControls.txtSub_No.Text, stepname);
        //if (dt.Rows.Count > 0)
        //{
        //    DataRow[] rowArray = dt.Select(" STATUS is null or Status='' or Status='Reply' ");
        //    if (rowArray.Length > 0)
        //    {
        //        ErrMsg.Append("DFX未填寫完畢!</br>");
        //    }

        //}
        if (dtCTQ.Rows.Count > 0)
        {
            DataRow[] rowArrayCTQ = dtCTQ.Select(" STATUS is null or STATUS='' OR STATUS='Reply' ");
            if (rowArrayCTQ.Length > 0)
            {
                ErrMsg.Append("CTQ未填寫完畢!</br>");
            }
        }
        if (dtIssues.Rows.Count > 0)
        {
            DataRow[] rowArrayIssues = dtIssues.Select(" STAUTS is null or STAUTS='' or STAUTS='Write' ");
            if (rowArrayIssues.Length > 0)
            {
                ErrMsg.Append("IssueList未填寫完畢!</br>");
            }

        }
        if (dtFMEA.Rows.Count > 0)
        {
            DataRow[] rowArrayFMEA = dtFMEA.Select(" STATUS is null or STATUS='' or STATUS='Write' ");
            if (rowArrayFMEA.Length > 0)
            {
                ErrMsg.Append("FMEAList未填寫完畢!</br>");
            }

        } 
        return ErrMsg.ToString();
    }

    protected string DeptFormFieldsValidate_Prepared(PilotRunReportUIShadow lUIControls, SPMTaskVariables SPMTaskVars)
    {
        string stepname = SPMTaskVars.ReadDatum("STEPNAME").ToString();
        string sbErrMsg = string.Empty;
        string caseid = SPMTaskVars.ReadDatum("CASEID").ToString();
        DataTable dt = GetPRAttachment(int.Parse(caseid), oUIControls.cobDept.Text);
        if (dt.Rows.Count == 0)
        {
            sbErrMsg = "請上傳本部門的附件！";
        }
        return sbErrMsg;
    }


    #endregion

    protected DataTable GetUserDept(string Category, string DocNo)
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));

        StringBuilder sbDept = new StringBuilder();
        string sql = "select  DISTINCT DEPT from TB_NPI_APP_MEMBER where 1=1"
             + " AND  (WriteEname =@ENAME OR ReplyEName=@ENAME OR CheckedEname=@ENAME)"

             + " AND CategoryFlag=@Category"
             + " AND DOC_NO=@Doc_No";
        ArrayList opc = new ArrayList();
        opc.Add(DataPara.CreateDataParameter("@ENAME", DbType.String, oPara.LoginId.ToString()));
        opc.Add(DataPara.CreateDataParameter("@Category", DbType.String, Category));
        opc.Add(DataPara.CreateDataParameter("@Doc_No", DbType.String, DocNo));
        DataTable dt = sdb.TransactionExecute(sql, opc);
        return dt;

    }

    //針對Issue and FMEA的下拉部門 
    protected DataTable GetUserDeptIF(string Category, string DocNo,string stepname)
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));

        StringBuilder sbDept = new StringBuilder();
        string sql = "select  DISTINCT DEPT from TB_NPI_APP_MEMBER where 1=1";
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
        ArrayList opc = new ArrayList();
        opc.Add(DataPara.CreateDataParameter("@ENAME", DbType.String, oPara.LoginId.ToString()));
        opc.Add(DataPara.CreateDataParameter("@Category", DbType.String, Category));
        opc.Add(DataPara.CreateDataParameter("@Doc_No", DbType.String, DocNo));
        DataTable dt = sdb.TransactionExecute(sql, opc);
        return dt;

    }

    private DataTable GetDOAResult(int caseid, string APPROVER, string Dept, string Approver_Levels)
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        string sql = "select APPROVER from TB_NPI_APP_RESULT where CASEID=@CASEID and DEPT=@DEPT and APPROVER = @APPROVER and APPROVER_Levels=@APPROVER_Levels";
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@CASEID", DbType.Int32, caseid));
        opc.Add(DataPara.CreateDataParameter("@DEPT", DbType.String, Dept));
        opc.Add(DataPara.CreateDataParameter("@APPROVER", DbType.String, APPROVER));
        opc.Add(DataPara.CreateDataParameter("@APPROVER_Levels", DbType.String, Approver_Levels));
        DataTable dt = sdb.TransactionExecute(sql, opc);
        return dt;
    }

    private DataTable GetPM_Issues(int caseid)
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        string sql = "select FILE_NAME from TB_NPI_APP_ISSUELIST_ATTACHFILE where CASEID=@CASEID ";
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@CASEID", DbType.Int32, caseid));
        DataTable dt = sdb.TransactionExecute(sql, opc);
        return dt;
    }

    private DataTable GetPRAttachment(int caseid, string Dept)
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        string sql = "select FILE_NAME from TB_NPI_APP_PR_ATTACHFILE where CASEID=@CASEID and DEPT=@DEPT";
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@CASEID", DbType.Int32, caseid));
        opc.Add(DataPara.CreateDataParameter("@DEPT", DbType.String, Dept));
        DataTable dt = sdb.TransactionExecute(sql, opc);
        return dt;
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

    private string GetUserDept(string Category, string LoginId, string DocNo, string stepname,string Categoryy)
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        ArrayList opc = new ArrayList();
        StringBuilder sbDept = new StringBuilder();
        string sql = "select  DISTINCT DEPT from TB_NPI_APP_MEMBER where 1=1";
        sql += " AND CategoryFlag=@Category";
        sql += " AND DOC_NO=@Doc_No";
        sql += " and Category = @Categoryy";
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

        opc.Add(DataPara.CreateDataParameter("@Categoryy", DbType.String, Categoryy));
        DataTable dt = sdb.TransactionExecute(sql, opc);
        foreach (DataRow dr in dt.Rows)
        {
            sbDept.AppendFormat("{0},", dr["DEPT"].ToString());
        }
        return sbDept.ToString().TrimEnd(',');
    }

    #region 填充DFX Score所屬的GridPanel
    protected void CreateDFXScore(string DocNo)
    {
        DataTable dt = CheckDFXScore(DocNo);
        DataTable dtMaster = GetMaster(DocNo);//基本資料
        DataRow drM = dtMaster.Rows[0];
        if (dt.Rows.Count == 0)
        {
            #region 獲取 各Level階段的Open數量
            #region AI_RI
            DataTable dtAIRILevel0 = GetDFXLevel(DocNo, "AI_RI", "0");
            DataTable dtAIRILevel1 = GetDFXLevel(DocNo, "AI_RI", "1");
            DataTable dtAIRILevel2 = GetDFXLevel(DocNo, "AI_RI", "2");
            DataTable dtAIRILevel3 = GetDFXLevel(DocNo, "AI_RI", "3");
            string AIRI0 = dtAIRILevel0.Rows[0]["amount"].ToString() == "" ? "0" : dtAIRILevel0.Rows[0]["amount"].ToString();
            string AIRI1 = dtAIRILevel1.Rows[0]["amount"].ToString() == "" ? "0" : dtAIRILevel1.Rows[0]["amount"].ToString();
            string AIRI2 = dtAIRILevel2.Rows[0]["amount"].ToString() == "" ? "0" : dtAIRILevel2.Rows[0]["amount"].ToString();
            string AIRI3 = dtAIRILevel3.Rows[0]["amount"].ToString() == "" ? "0" : dtAIRILevel3.Rows[0]["amount"].ToString();
            #endregion

            #region SMT
            DataTable dtSMTLevel0 = GetDFXLevel(DocNo, "SMT", "0");
            DataTable dtSMTLevel1 = GetDFXLevel(DocNo, "SMT", "1");
            DataTable dtSMTLevel2 = GetDFXLevel(DocNo, "SMT", "2");
            DataTable dtSMTLevel3 = GetDFXLevel(DocNo, "SMT", "3");
            string SMT0 = dtSMTLevel0.Rows[0]["amount"].ToString() == "" ? "0" : dtSMTLevel0.Rows[0]["amount"].ToString();
            string SMT1 = dtSMTLevel1.Rows[0]["amount"].ToString() == "" ? "0" : dtSMTLevel1.Rows[0]["amount"].ToString();
            string SMT2 = dtSMTLevel2.Rows[0]["amount"].ToString() == "" ? "0" : dtSMTLevel2.Rows[0]["amount"].ToString();
            string SMT3 = dtSMTLevel3.Rows[0]["amount"].ToString() == "" ? "0" : dtSMTLevel3.Rows[0]["amount"].ToString();
            #endregion

            #region IE
            DataTable dtIELevel0 = GetDFXLevel(DocNo, "IE", "0");
            DataTable dtIELevel1 = GetDFXLevel(DocNo, "IE", "1");
            DataTable dtIELevel2 = GetDFXLevel(DocNo, "IE", "2");
            DataTable dtIELevel3 = GetDFXLevel(DocNo, "IE", "3");
            string IE0 = dtIELevel0.Rows[0]["amount"].ToString() == "" ? "0" : dtIELevel0.Rows[0]["amount"].ToString();
            string IE1 = dtIELevel1.Rows[0]["amount"].ToString() == "" ? "0" : dtIELevel1.Rows[0]["amount"].ToString();
            string IE2 = dtIELevel2.Rows[0]["amount"].ToString() == "" ? "0" : dtIELevel2.Rows[0]["amount"].ToString();
            string IE3 = dtIELevel3.Rows[0]["amount"].ToString() == "" ? "0" : dtIELevel3.Rows[0]["amount"].ToString();
            #endregion

            #region DQE
            DataTable dtDQELevel0 = GetDFXLevel(DocNo, "DQE", "0");
            DataTable dtDQELevel1 = GetDFXLevel(DocNo, "DQE", "1");
            DataTable dtDQELevel2 = GetDFXLevel(DocNo, "DQE", "2");
            DataTable dtDQELevel3 = GetDFXLevel(DocNo, "DQE", "3");
            string DQE0 = dtDQELevel0.Rows[0]["amount"].ToString() == "" ? "0" : dtDQELevel0.Rows[0]["amount"].ToString();
            string DQE1 = dtDQELevel1.Rows[0]["amount"].ToString() == "" ? "0" : dtDQELevel1.Rows[0]["amount"].ToString();
            string DQE2 = dtDQELevel2.Rows[0]["amount"].ToString() == "" ? "0" : dtDQELevel2.Rows[0]["amount"].ToString();
            string DQE3 = dtDQELevel3.Rows[0]["amount"].ToString() == "" ? "0" : dtDQELevel3.Rows[0]["amount"].ToString();
            #endregion

            #region EE
            DataTable dtEELevel0 = GetDFXLevel(DocNo, "EE", "0");
            DataTable dtEELevel1 = GetDFXLevel(DocNo, "EE", "1");
            DataTable dtEELevel2 = GetDFXLevel(DocNo, "EE", "2");
            DataTable dtEELevel3 = GetDFXLevel(DocNo, "EE", "3");
            string EE0 = dtEELevel0.Rows[0]["amount"].ToString() == "" ? "0" : dtEELevel0.Rows[0]["amount"].ToString();
            string EE1 = dtEELevel1.Rows[0]["amount"].ToString() == "" ? "0" : dtEELevel1.Rows[0]["amount"].ToString();
            string EE2 = dtEELevel2.Rows[0]["amount"].ToString() == "" ? "0" : dtEELevel2.Rows[0]["amount"].ToString();
            string EE3 = dtEELevel3.Rows[0]["amount"].ToString() == "" ? "0" : dtEELevel3.Rows[0]["amount"].ToString();
            #endregion

            #region UQ
            DataTable dtUQLevel0 = GetDFXLevel(DocNo, "UQ", "0");
            DataTable dtUQLevel1 = GetDFXLevel(DocNo, "UQ", "1");
            DataTable dtUQLevel2 = GetDFXLevel(DocNo, "UQ", "2");
            DataTable dtUQLevel3 = GetDFXLevel(DocNo, "UQ", "3");
            string UQ0 = dtUQLevel0.Rows[0]["amount"].ToString() == "" ? "0" : dtUQLevel0.Rows[0]["amount"].ToString();
            string UQ1 = dtUQLevel1.Rows[0]["amount"].ToString() == "" ? "0" : dtUQLevel1.Rows[0]["amount"].ToString();
            string UQ2 = dtUQLevel2.Rows[0]["amount"].ToString() == "" ? "0" : dtUQLevel2.Rows[0]["amount"].ToString();
            string UQ3 = dtUQLevel3.Rows[0]["amount"].ToString() == "" ? "0" : dtUQLevel3.Rows[0]["amount"].ToString();
            #endregion

            #region SQ
            DataTable dtSQLevel0 = GetDFXLevel(DocNo, "SQ", "0");
            DataTable dtSQLevel1 = GetDFXLevel(DocNo, "SQ", "1");
            DataTable dtSQLevel2 = GetDFXLevel(DocNo, "SQ", "2");
            DataTable dtSQLevel3 = GetDFXLevel(DocNo, "SQ", "3");
            string SQ0 = dtSQLevel0.Rows[0]["amount"].ToString() == "" ? "0" : dtSQLevel0.Rows[0]["amount"].ToString();
            string SQ1 = dtSQLevel1.Rows[0]["amount"].ToString() == "" ? "0" : dtSQLevel1.Rows[0]["amount"].ToString();
            string SQ2 = dtSQLevel2.Rows[0]["amount"].ToString() == "" ? "0" : dtSQLevel2.Rows[0]["amount"].ToString();
            string SQ3 = dtSQLevel3.Rows[0]["amount"].ToString() == "" ? "0" : dtSQLevel3.Rows[0]["amount"].ToString();

            #endregion

            #region EVT階段 DFX統計  >=80
            if (drM["SUB_DOC_PHASE"].ToString().Contains("EVT"))
            {
                int LevelEVT = 80;

                #region AIRI
                DataTable dtAIRIS = GetDFXScore(DocNo, "AI_RI");//計算AI_RI的分數
                double AIRIS = (double.Parse(dtAIRIS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtAIRIS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtAIRIS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtAIRIS.Rows[0]["MaxPoints"].ToString())) * 100;
                if (AIRIS != 100 && AIRIS != 0)
                {
                    string AIRISS = String.Format("{0:N1}", AIRIS) + "%";
                    if (AIRIS >= LevelEVT && AIRI0 == "0")
                    {
                        string ResultAIRIP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "AI_RI", "DF-AI/RI scores stage require", AIRISS, AIRI0, AIRI1, AIRI2, AIRI3, ResultAIRIP);
                    }
                    else
                    {
                        string ResultAIRIF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "AI_RI", "DF-AI/RI scores stage require", AIRISS, AIRI0, AIRI1, AIRI2, AIRI3, ResultAIRIF);
                    }
                }
                else if (AIRIS == 0)
                {
                    string AIRISS = "0" + "%";
                    string ResultAIRIF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "AI_RI", "DF-AI/RI scores stage require", AIRISS, AIRI0, AIRI1, AIRI2, AIRI3, ResultAIRIF);
                }
                else
                {
                    string AIRISS = AIRIS.ToString() + "%";
                    if (AIRIS >= LevelEVT && AIRI0 == "0")
                    {
                        string ResultAIRIP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "AI_RI", "DF-AI/RI scores stage require", AIRISS, AIRI0, AIRI1, AIRI2, AIRI3, ResultAIRIP);
                    }
                    else
                    {
                        string ResultAIRIF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "AI_RI", "DF-AI/RI scores stage require", AIRISS, AIRI0, AIRI1, AIRI2, AIRI3, ResultAIRIF);
                    }
                }
                #endregion

                #region SMT
                DataTable dtSMTS = GetDFXScore(DocNo, "SMT");
                double SMTS = (double.Parse(dtSMTS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtSMTS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtSMTS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtSMTS.Rows[0]["MaxPoints"].ToString())) * 100;
                if (SMTS != 100 && SMTS != 0)
                {
                    string SMTSS = String.Format("{0:N1}", SMTS) + "%";
                    if (SMTS > LevelEVT && SMT0 == "0")
                    {
                        string ResultSMTP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SMT", "DF-SMD scores stage require", SMTSS, SMT0, SMT1, SMT2, SMT3, ResultSMTP);
                    }
                    else
                    {
                        string ResultSMTF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SMT", "DF-SMD scores stage require", SMTSS, SMT0, SMT1, SMT2, SMT3, ResultSMTF);
                    }
                }
                else if (SMTS == 0)
                {
                    string SMTSS = "0" + "%";
                    string ResultSMTF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SMT", "DF-SMD scores stage require", SMTSS, SMT0, SMT1, SMT2, SMT3, ResultSMTF);
                }
                else
                {
                    string SMTSS = SMTS.ToString() + "%";
                    if (SMTS > LevelEVT && SMT0 == "0")
                    {
                        string ResultSMTP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SMT", "DF-SMD scores stage require", SMTSS, SMT0, SMT1, SMT2, SMT3, ResultSMTP);
                    }
                    else
                    {
                        string ResultSMTF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SMT", "DF-SMD scores stage require", SMTSS, SMT0, SMT1, SMT2, SMT3, ResultSMTF);
                    }
                }
                #endregion

                #region IE
                DataTable dtIES = GetDFXScore(DocNo, "IE");
                double IES = (double.Parse(dtIES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtIES.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtIES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtIES.Rows[0]["MaxPoints"].ToString())) * 100;
                if (IES != 100 && IES != 0)
                {
                    string IESS = String.Format("{0:N1}", IES) + "%";
                    if (IES > LevelEVT && IE0 == "0")
                    {
                        string ResultIEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "IE", "DF-Process scores stage require", IESS, IE0, IE1, IE2, IE3, ResultIEP);
                    }
                    else
                    {
                        string ResultIEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "IE", "DF-Process scores stage require", IESS, IE0, IE1, IE2, IE3, ResultIEF);
                    }
                }
                else if (IES == 0)
                {
                    string IESS = "0" + "%";
                    string ResultIEF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "IE", "DF-Process scores stage require", IESS, IE0, IE1, IE2, IE3, ResultIEF);
                }
                else
                {
                    string IESS = IES.ToString() + "%";
                    if (IES > LevelEVT && IE0 == "0")
                    {
                        string ResultIEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "IE", "DF-Process scores stage require", IESS, IE0, IE1, IE2, IE3, ResultIEP);
                    }
                    else
                    {
                        string ResultIEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "IE", "DF-Process scores stage require", IESS, IE0, IE1, IE2, IE3, ResultIEF);
                    }
                }
                #endregion

                #region DQE
                DataTable dtDQES = GetDFXScore(DocNo, "DQE");
                double DQES = (double.Parse(dtDQES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtDQES.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtDQES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtDQES.Rows[0]["MaxPoints"].ToString())) * 100;
                if (DQES != 100 && DQES != 0)
                {
                    string DQESS = String.Format("{0:N1}", DQES) + "%";
                    if (DQES > LevelEVT && DQE0 == "0")
                    {
                        string ResultDQEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "DQE", "DF-Reliability scores stage require", DQESS, DQE0, DQE1, DQE2, DQE3, ResultDQEP);
                    }
                    else
                    {
                        string ResultDQEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "DQE", "DF-Reliability scores stage require", DQESS, DQE0, DQE1, DQE2, DQE3, ResultDQEF);
                    }
                }
                else if (DQES == 0)
                {
                    string DQESS = "0" + "%";
                    string ResultDQEF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "DQE", "DF-Reliability scores stage require", DQESS, DQE0, DQE1, DQE2, DQE3, ResultDQEF);
                }
                else
                {
                    string DQESS = DQES.ToString() + "%";
                    if (DQES > LevelEVT && DQE0 == "0")
                    {
                        string ResultDQEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "DQE", "DF-Reliability scores stage require", DQESS, DQE0, DQE1, DQE2, DQE3, ResultDQEP);
                    }
                    else
                    {
                        string ResultDQEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "DQE", "DF-Reliability scores stage require", DQESS, DQE0, DQE1, DQE2, DQE3, ResultDQEF);
                    }
                }

                #endregion

                #region EE
                DataTable dtEES = GetDFXScore(DocNo, "EE");
                double EES = (double.Parse(dtEES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtEES.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtEES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtEES.Rows[0]["MaxPoints"].ToString())) * 100;
                if (EES != 100 && EES != 0)
                {
                    string EESS = String.Format("{0:N1}", EES) + "%";
                    if (EES > LevelEVT && EE0 == "0")
                    {
                        string ResultEEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "EE", "DF-EE scores stage require", EESS, EE0, EE1, EE2, EE3, ResultEEP);
                    }
                    else
                    {
                        string ResultEEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "EE", "DF-EE scores stage require", EESS, EE0, EE1, EE2, EE3, ResultEEF);
                    }
                }
                else if (EES == 0)
                {
                    string EESS = "0" + "%";
                    string ResultEEF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "EE", "DF-EE scores stage require", EESS, EE0, EE1, EE2, EE3, ResultEEF);
                }
                else
                {
                    string EESS = EES.ToString() + "%";
                    if (EES > LevelEVT && EE0 == "0")
                    {
                        string ResultEEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "EE", "DF-EE scores stage require", EESS, EE0, EE1, EE2, EE3, ResultEEP);
                    }
                    else
                    {
                        string ResultEEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "EE", "DF-EE scores stage require", EESS, EE0, EE1, EE2, EE3, ResultEEF);
                    }
                }
                #endregion

                #region UQ
                DataTable dtUQS = GetDFXScore(DocNo, "UQ");
                double UQS = (double.Parse(dtUQS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtUQS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtUQS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtUQS.Rows[0]["MaxPoints"].ToString())) * 100;
                if (UQS != 100 && UQS != 0)
                {
                    string UQSS = String.Format("{0:N1}", UQS) + "%";
                    if (UQS > LevelEVT && UQ0 == "0")
                    {
                        string ResultUQP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "UQ", "DF-UQ scores stage require", UQSS, UQ0, UQ1, UQ2, UQ3, ResultUQP);

                    }
                    else
                    {
                        string ResultUQF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "UQ", "DF-UQ scores stage require", UQSS, UQ0, UQ1, UQ2, UQ3, ResultUQF);

                    }
                }
                else if (UQS == 0)
                {
                    string UQSS = "0" + "%";
                    string ResultUQF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "UQ", "DF-UQ scores stage require", UQSS, UQ0, UQ1, UQ2, UQ3, ResultUQF);
                }
                else
                {
                    string UQSS = UQS.ToString() + "%";
                    if (UQS > LevelEVT && UQ0 == "0")
                    {
                        string ResultUQP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "UQ", "DF-UQ scores stage require", UQSS, UQ0, UQ1, UQ2, UQ3, ResultUQP);

                    }
                    else
                    {
                        string ResultUQF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "UQ", "DF-UQ scores stage require", UQSS, UQ0, UQ1, UQ2, UQ3, ResultUQF);

                    }
                }
                #endregion

                #region SQ
                DataTable dtSQS = GetDFXScore(DocNo, "SQ");
                double SQS = (double.Parse(dtSQS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtSQS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtSQS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtSQS.Rows[0]["MaxPoints"].ToString())) * 100;
                if (SQS != 100 && SQS != 0)
                {
                    string SQSS = String.Format("{0:N1}", SQS) + "%";
                    if (SQS > LevelEVT && SQ0 == "0")
                    {
                        string ResultSQP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SQ", "DF-Soldering scores stage require", SQSS.ToString(), SQ0, SQ1, SQ2, SQ3, ResultSQP);
                    }
                    else
                    {
                        string ResultSQF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SQ", "DF-Soldering scores stage require", SQSS.ToString(), SQ0, SQ1, SQ2, SQ3, ResultSQF);
                    }
                }
                else if (SQS == 0)
                {
                    string SQSS = "0" + "%";
                    string ResultSQF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SQ", "DF-Soldering scores stage require", SQSS.ToString(), SQ0, SQ1, SQ2, SQ3, ResultSQF);
                }
                else
                {
                    string SQSS = SQS.ToString() + "%";
                    if (SQS > LevelEVT && SQ0 == "0")
                    {
                        string ResultSQP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SQ", "DF-Soldering scores stage require", SQSS.ToString(), SQ0, SQ1, SQ2, SQ3, ResultSQP);
                    }
                    else
                    {
                        string ResultSQF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SQ", "DF-Soldering scores stage require", SQSS.ToString(), SQ0, SQ1, SQ2, SQ3, ResultSQF);
                    }
                }
                #endregion


            }
            #endregion

            #region DVT階段 DFX統計  >=90
            else if (drM["SUB_DOC_PHASE"].ToString().Contains("DVT"))
            {
                int LevelEVT = 90;

                #region AIRI
                DataTable dtAIRIS = GetDFXScore(DocNo, "AI_RI");//計算AI_RI的分數
                double AIRIS = (double.Parse(dtAIRIS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtAIRIS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtAIRIS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtAIRIS.Rows[0]["MaxPoints"].ToString())) * 100;
                if (AIRIS != 100 && AIRIS != 0)
                {
                    string AIRISS = String.Format("{0:N1}", AIRIS) + "%";
                    if (AIRIS >= LevelEVT && AIRI0 == "0" && AIRI1 == "0")
                    {
                        string ResultAIRIP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "AI_RI", "DF-AI/RI scores stage require", AIRISS, AIRI0, AIRI1, AIRI2, AIRI3, ResultAIRIP);
                    }
                    else
                    {
                        string ResultAIRIF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "AI_RI", "DF-AI/RI scores stage require", AIRISS, AIRI0, AIRI1, AIRI2, AIRI3, ResultAIRIF);
                    }
                }
                else if (AIRIS == 0)
                {
                    string ResultAIRIF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "AI_RI", "DF-AI/RI scores stage require", "0%", AIRI0, AIRI1, AIRI2, AIRI3, ResultAIRIF);
                }
                else
                {
                    string AIRISS = AIRIS.ToString() + "%";
                    if (AIRIS >= LevelEVT && AIRI0 == "0" && AIRI1 == "0")
                    {
                        string ResultAIRIP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "AI_RI", "DF-AI/RI scores stage require", AIRISS, AIRI0, AIRI1, AIRI2, AIRI3, ResultAIRIP);
                    }
                    else
                    {
                        string ResultAIRIF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "AI_RI", "DF-AI/RI scores stage require", AIRISS, AIRI0, AIRI1, AIRI2, AIRI3, ResultAIRIF);
                    }
                }
                #endregion

                #region SMT
                DataTable dtSMTS = GetDFXScore(DocNo, "SMT");
                double SMTS = (double.Parse(dtSMTS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtSMTS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtSMTS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtSMTS.Rows[0]["MaxPoints"].ToString())) * 100;
                if (SMTS != 100 && SMTS != 0)
                {
                    string SMTSS = String.Format("{0:N1}", SMTS) + "%";
                    if (SMTS > LevelEVT && SMT0 == "0" && SMT1 == "0")
                    {
                        string ResultSMTP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SMT", "DF-SMD scores stage require", SMTSS, SMT0, SMT1, SMT2, SMT3, ResultSMTP);
                    }
                    else
                    {
                        string ResultSMTF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SMT", "DF-SMD scores stage require", SMTSS, SMT0, SMT1, SMT2, SMT3, ResultSMTF);
                    }
                }
                else if (SMTS == 0)
                {
                    string ResultSMTF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SMT", "DF-SMD scores stage require", "0%", SMT0, SMT1, SMT2, SMT3, ResultSMTF);
                }
                else
                {
                    string SMTSS = SMTS.ToString() + "%";
                    if (SMTS > LevelEVT && SMT0 == "0" && SMT1 == "0")
                    {
                        string ResultSMTP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SMT", "DF-SMD scores stage require", SMTSS, SMT0, SMT1, SMT2, SMT3, ResultSMTP);
                    }
                    else
                    {
                        string ResultSMTF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SMT", "DF-SMD scores stage require", SMTSS, SMT0, SMT1, SMT2, SMT3, ResultSMTF);
                    }
                }
                #endregion

                #region IE
                DataTable dtIES = GetDFXScore(DocNo, "IE");
                double IES = (double.Parse(dtIES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtIES.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtIES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtIES.Rows[0]["MaxPoints"].ToString())) * 100;
                if (IES != 100 && IES != 0)
                {
                    string IESS = String.Format("{0:N1}", IES) + "%";
                    if (IES > LevelEVT && IE0 == "0" && IE1 == "0")
                    {
                        string ResultIEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "IE", "DF-Process scores stage require", IESS, IE0, IE1, IE2, IE3, ResultIEP);
                    }
                    else
                    {
                        string ResultIEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "IE", "DF-Process scores stage require", IESS, IE0, IE1, IE2, IE3, ResultIEF);
                    }
                }
                else if (IES == 0)
                {
                    string ResultIEF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "IE", "DF-Process scores stage require", "0%", IE0, IE1, IE2, IE3, ResultIEF);
                }
                else
                {
                    string IESS = IES.ToString() + "%";
                    if (IES > LevelEVT && IE0 == "0" && IE1 == "0")
                    {
                        string ResultIEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "IE", "DF-Process scores stage require", IESS, IE0, IE1, IE2, IE3, ResultIEP);
                    }
                    else
                    {
                        string ResultIEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "IE", "DF-Process scores stage require", IESS, IE0, IE1, IE2, IE3, ResultIEF);
                    }
                }
                #endregion

                #region DQE
                DataTable dtDQES = GetDFXScore(DocNo, "DQE");
                double DQES = (double.Parse(dtDQES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtDQES.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtDQES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtDQES.Rows[0]["MaxPoints"].ToString())) * 100;
                if (DQES != 100 && DQES != 0)
                {
                    string DQESS = String.Format("{0:N1}", DQES) + "%";
                    if (DQES > LevelEVT && DQE0 == "0" && DQE1 == "0")
                    {
                        string ResultDQEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "DQE", "DF-Reliability scores stage require", DQESS, DQE0, DQE1, DQE2, DQE3, ResultDQEP);
                    }
                    else
                    {
                        string ResultDQEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "DQE", "DF-Reliability scores stage require", DQESS, DQE0, DQE1, DQE2, DQE3, ResultDQEF);
                    }
                }
                else if (DQES == 0)
                {
                    string ResultDQEF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "DQE", "DF-Reliability scores stage require", "0%", DQE0, DQE1, DQE2, DQE3, ResultDQEF);
                }
                else
                {
                    string DQESS = DQES.ToString() + "%";
                    if (DQES > LevelEVT && DQE0 == "0" && DQE1 == "0")
                    {
                        string ResultDQEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "DQE", "DF-Reliability scores stage require", DQESS, DQE0, DQE1, DQE2, DQE3, ResultDQEP);
                    }
                    else
                    {
                        string ResultDQEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "DQE", "DF-Reliability scores stage require", DQESS, DQE0, DQE1, DQE2, DQE3, ResultDQEF);
                    }
                }

                #endregion

                #region EE
                DataTable dtEES = GetDFXScore(DocNo, "EE");
                double EES = (double.Parse(dtEES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtEES.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtEES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtEES.Rows[0]["MaxPoints"].ToString())) * 100;
                if (EES != 100 && EES != 0)
                {
                    string EESS = String.Format("{0:N1}", EES) + "%";
                    if (EES > LevelEVT && EE0 == "0" && EE1 == "0")
                    {
                        string ResultEEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "EE", "DF-EE scores stage require", EESS, EE0, EE1, EE2, EE3, ResultEEP);
                    }
                    else
                    {
                        string ResultEEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "EE", "DF-EE scores stage require", EESS, EE0, EE1, EE2, EE3, ResultEEF);
                    }
                }
                else if (EES == 0)
                {
                    string ResultEEF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "EE", "DF-EE scores stage require", "0%", EE0, EE1, EE2, EE3, ResultEEF);
                }
                else
                {
                    string EESS = EES.ToString() + "%";
                    if (EES > LevelEVT && EE0 == "0" && EE1 == "0")
                    {
                        string ResultEEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "EE", "DF-EE scores stage require", EESS, EE0, EE1, EE2, EE3, ResultEEP);
                    }
                    else
                    {
                        string ResultEEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "EE", "DF-EE scores stage require", EESS, EE0, EE1, EE2, EE3, ResultEEF);
                    }
                }
                #endregion

                #region UQ
                DataTable dtUQS = GetDFXScore(DocNo, "UQ");
                double UQS = (double.Parse(dtUQS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtUQS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtUQS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtUQS.Rows[0]["MaxPoints"].ToString())) * 100;
                if (UQS != 100 && UQS != 0)
                {
                    string UQSS = String.Format("{0:N1}", UQS) + "%";
                    if (UQS > LevelEVT && UQ0 == "0" && UQ1 == "0")
                    {
                        string ResultUQP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "UQ", "DF-UQ scores stage require", UQSS, UQ0, UQ1, UQ2, UQ3, ResultUQP);

                    }
                    else
                    {
                        string ResultUQF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "UQ", "DF-UQ scores stage require", UQSS, UQ0, UQ1, UQ2, UQ3, ResultUQF);

                    }
                }
                else if (UQS == 0)
                {
                    string ResultUQF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "UQ", "DF-UQ scores stage require", "0%", UQ0, UQ1, UQ2, UQ3, ResultUQF);
                }
                else
                {
                    string UQSS = UQS.ToString() + "%";
                    if (UQS > LevelEVT && UQ0 == "0" && UQ1 == "0")
                    {
                        string ResultUQP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "UQ", "DF-UQ scores stage require", UQSS, UQ0, UQ1, UQ2, UQ3, ResultUQP);

                    }
                    else
                    {
                        string ResultUQF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "UQ", "DF-UQ scores stage require", UQSS, UQ0, UQ1, UQ2, UQ3, ResultUQF);

                    }
                }
                #endregion

                #region SQ
                DataTable dtSQS = GetDFXScore(DocNo, "SQ");
                double SQS = (double.Parse(dtSQS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtSQS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtSQS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtSQS.Rows[0]["MaxPoints"].ToString())) * 100;
                if (SQS != 100 && SQS != 0)
                {
                    string SQSS = String.Format("{0:N1}", SQS) + "%";
                    if (SQS > LevelEVT && SQ0 == "0" && SQ1 == "0")
                    {
                        string ResultSQP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SQ", "DF-Soldering scores stage require", SQSS.ToString(), SQ0, SQ1, SQ2, SQ3, ResultSQP);
                    }
                    else
                    {
                        string ResultSQF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SQ", "DF-Soldering scores stage require", SQSS.ToString(), SQ0, SQ1, SQ2, SQ3, ResultSQF);
                    }
                }
                else if (SQS == 0)
                {
                    string ResultSQF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SQ", "DF-Soldering scores stage require", "0%", SQ0, SQ1, SQ2, SQ3, ResultSQF);
                }
                else
                {
                    string SQSS = SQS.ToString() + "%";
                    if (SQS > LevelEVT && SQ0 == "0" && SQ1 == "0")
                    {
                        string ResultSQP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SQ", "DF-Soldering scores stage require", SQSS.ToString(), SQ0, SQ1, SQ2, SQ3, ResultSQP);
                    }
                    else
                    {
                        string ResultSQF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SQ", "DF-Soldering scores stage require", SQSS.ToString(), SQ0, SQ1, SQ2, SQ3, ResultSQF);
                    }
                }
                #endregion
            }



            #endregion

            #region PR階段 DFX統計  >=90
            else if (drM["SUB_DOC_PHASE"].ToString().Contains("P.Run"))
            {
                int LevelEVT = 90;

                #region AIRI
                DataTable dtAIRIS = GetDFXScore(DocNo, "AI_RI");//計算AI_RI的分數
                double AIRIS = (double.Parse(dtAIRIS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtAIRIS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtAIRIS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtAIRIS.Rows[0]["MaxPoints"].ToString())) * 100;
                if (AIRIS != 100 && AIRIS != 0)
                {
                    string AIRISS = String.Format("{0:N1}", AIRIS) + "%";
                    if (AIRIS >= LevelEVT && AIRI0 == "0" && AIRI1 == "0" && AIRI2 == "0")
                    {
                        string ResultAIRIP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "AI_RI", "DF-AI/RI scores stage require", AIRISS, AIRI0, AIRI1, AIRI2, AIRI3, ResultAIRIP);
                    }
                    else
                    {
                        string ResultAIRIF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "AI_RI", "DF-AI/RI scores stage require", AIRISS, AIRI0, AIRI1, AIRI2, AIRI3, ResultAIRIF);
                    }
                }
                else if (AIRIS == 0)
                {
                    string ResultAIRIF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "AI_RI", "DF-AI/RI scores stage require", "0%", AIRI0, AIRI1, AIRI2, AIRI3, ResultAIRIF);
                }
                else
                {
                    string AIRISS = AIRIS.ToString() + "%";
                    if (AIRIS >= LevelEVT && AIRI0 == "0" && AIRI1 == "0")
                    {
                        string ResultAIRIP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "AI_RI", "DF-AI/RI scores stage require", AIRISS, AIRI0, AIRI1, AIRI2, AIRI3, ResultAIRIP);
                    }
                    else
                    {
                        string ResultAIRIF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "AI_RI", "DF-AI/RI scores stage require", AIRISS, AIRI0, AIRI1, AIRI2, AIRI3, ResultAIRIF);
                    }
                }
                #endregion

                #region SMT
                DataTable dtSMTS = GetDFXScore(DocNo, "SMT");
                double SMTS = (double.Parse(dtSMTS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtSMTS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtSMTS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtSMTS.Rows[0]["MaxPoints"].ToString())) * 100;
                if (SMTS != 100 && SMTS != 0)
                {
                    string SMTSS = String.Format("{0:N1}", SMTS) + "%";
                    if (SMTS > LevelEVT && SMT0 == "0" && SMT1 == "0" && SMT2 == "0")
                    {
                        string ResultSMTP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SMT", "DF-SMD scores stage require", SMTSS, SMT0, SMT1, SMT2, SMT3, ResultSMTP);
                    }
                    else
                    {
                        string ResultSMTF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SMT", "DF-SMD scores stage require", SMTSS, SMT0, SMT1, SMT2, SMT3, ResultSMTF);
                    }
                }
                else if (SMTS == 0)
                {
                    string ResultSMTF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SMT", "DF-SMD scores stage require", "0%", SMT0, SMT1, SMT2, SMT3, ResultSMTF);
                }
                else
                {
                    string SMTSS = SMTS.ToString() + "%";
                    if (SMTS > LevelEVT && SMT0 == "0" && SMT1 == "0")
                    {
                        string ResultSMTP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SMT", "DF-SMD scores stage require", SMTSS, SMT0, SMT1, SMT2, SMT3, ResultSMTP);
                    }
                    else
                    {
                        string ResultSMTF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SMT", "DF-SMD scores stage require", SMTSS, SMT0, SMT1, SMT2, SMT3, ResultSMTF);
                    }
                }
                #endregion

                #region IE
                DataTable dtIES = GetDFXScore(DocNo, "IE");
                double IES = (double.Parse(dtIES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtIES.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtIES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtIES.Rows[0]["MaxPoints"].ToString())) * 100;
                if (IES != 100 && IES != 0)
                {
                    string IESS = String.Format("{0:N1}", IES) + "%";
                    if (IES > LevelEVT && IE0 == "0" && IE1 == "0" && IE2 == "0")
                    {
                        string ResultIEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "IE", "DF-Process scores stage require", IESS, IE0, IE1, IE2, IE3, ResultIEP);
                    }
                    else
                    {
                        string ResultIEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "IE", "DF-Process scores stage require", IESS, IE0, IE1, IE2, IE3, ResultIEF);
                    }
                }
                else if (IES == 0)
                {
                    string ResultIEF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "IE", "DF-Process scores stage require", "0%", IE0, IE1, IE2, IE3, ResultIEF);
                }
                else
                {
                    string IESS = IES.ToString() + "%";
                    if (IES > LevelEVT && IE0 == "0" && IE1 == "0")
                    {
                        string ResultIEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "IE", "DF-Process scores stage require", IESS, IE0, IE1, IE2, IE3, ResultIEP);
                    }
                    else
                    {
                        string ResultIEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "IE", "DF-Process scores stage require", IESS, IE0, IE1, IE2, IE3, ResultIEF);
                    }
                }
                #endregion

                #region DQE
                DataTable dtDQES = GetDFXScore(DocNo, "DQE");
                double DQES = (double.Parse(dtDQES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtDQES.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtDQES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtDQES.Rows[0]["MaxPoints"].ToString())) * 100;
                if (DQES != 100 && DQES != 0)
                {
                    string DQESS = String.Format("{0:N1}", DQES) + "%";
                    if (DQES > LevelEVT && DQE0 == "0" && DQE1 == "0" && DQE2 == "0")
                    {
                        string ResultDQEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "DQE", "DF-Reliability scores stage require", DQESS, DQE0, DQE1, DQE2, DQE3, ResultDQEP);
                    }
                    else
                    {
                        string ResultDQEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "DQE", "DF-Reliability scores stage require", DQESS, DQE0, DQE1, DQE2, DQE3, ResultDQEF);
                    }
                }
                else if (DQES == 0)
                {
                    string ResultDQEF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "DQE", "DF-Reliability scores stage require", "0%", DQE0, DQE1, DQE2, DQE3, ResultDQEF);
                }
                else
                {
                    string DQESS = DQES.ToString() + "%";
                    if (DQES > LevelEVT && DQE0 == "0" && DQE1 == "0")
                    {
                        string ResultDQEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "DQE", "DF-Reliability scores stage require", DQESS, DQE0, DQE1, DQE2, DQE3, ResultDQEP);
                    }
                    else
                    {
                        string ResultDQEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "DQE", "DF-Reliability scores stage require", DQESS, DQE0, DQE1, DQE2, DQE3, ResultDQEF);
                    }
                }

                #endregion

                #region EE
                DataTable dtEES = GetDFXScore(DocNo, "EE");
                double EES = (double.Parse(dtEES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtEES.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtEES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtEES.Rows[0]["MaxPoints"].ToString())) * 100;
                if (EES != 100 && EES != 0)
                {
                    string EESS = String.Format("{0:N1}", EES) + "%";
                    if (EES > LevelEVT && EE0 == "0" && EE1 == "0" && EE2 == "0")
                    {
                        string ResultEEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "EE", "DF-EE scores stage require", EESS, EE0, EE1, EE2, EE3, ResultEEP);
                    }
                    else
                    {
                        string ResultEEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "EE", "DF-EE scores stage require", EESS, EE0, EE1, EE2, EE3, ResultEEF);
                    }
                }
                else if (EES == 0)
                {
                    string ResultEEF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "EE", "DF-EE scores stage require", "0%", EE0, EE1, EE2, EE3, ResultEEF);
                }
                else
                {
                    string EESS = EES.ToString() + "%";
                    if (EES > LevelEVT && EE0 == "0" && EE1 == "0")
                    {
                        string ResultEEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "EE", "DF-EE scores stage require", EESS, EE0, EE1, EE2, EE3, ResultEEP);
                    }
                    else
                    {
                        string ResultEEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "EE", "DF-EE scores stage require", EESS, EE0, EE1, EE2, EE3, ResultEEF);
                    }
                }
                #endregion

                #region UQ
                DataTable dtUQS = GetDFXScore(DocNo, "UQ");
                double UQS = (double.Parse(dtUQS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtUQS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtUQS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtUQS.Rows[0]["MaxPoints"].ToString())) * 100;
                if (UQS != 100 && UQS != 0)
                {
                    string UQSS = String.Format("{0:N1}", UQS) + "%";
                    if (UQS > LevelEVT && UQ0 == "0" && UQ1 == "0" && UQ2 == "0")
                    {
                        string ResultUQP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "UQ", "DF-UQ scores stage require", UQSS, UQ0, UQ1, UQ2, UQ3, ResultUQP);

                    }
                    else
                    {
                        string ResultUQF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "UQ", "DF-UQ scores stage require", UQSS, UQ0, UQ1, UQ2, UQ3, ResultUQF);

                    }
                }
                else if (UQS == 0)
                {
                    string ResultUQF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "UQ", "DF-UQ scores stage require", "0%", UQ0, UQ1, UQ2, UQ3, ResultUQF);
                }
                else
                {
                    string UQSS = UQS.ToString() + "%";
                    if (UQS > LevelEVT && UQ0 == "0" && UQ1 == "0")
                    {
                        string ResultUQP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "UQ", "DF-UQ scores stage require", UQSS, UQ0, UQ1, UQ2, UQ3, ResultUQP);

                    }
                    else
                    {
                        string ResultUQF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "UQ", "DF-UQ scores stage require", UQSS, UQ0, UQ1, UQ2, UQ3, ResultUQF);

                    }
                }
                #endregion

                #region SQ
                DataTable dtSQS = GetDFXScore(DocNo, "SQ");
                double SQS = (double.Parse(dtSQS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtSQS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtSQS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtSQS.Rows[0]["MaxPoints"].ToString())) * 100;
                if (SQS != 100 && SQS != 0)
                {
                    string SQSS = String.Format("{0:N1}", SQS) + "%";
                    if (SQS > LevelEVT && SQ0 == "0" && SQ1 == "0" && SQ2 == "0")
                    {
                        string ResultSQP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SQ", "DF-Soldering scores stage require", SQSS.ToString(), SQ0, SQ1, SQ2, SQ3, ResultSQP);
                    }
                    else
                    {
                        string ResultSQF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SQ", "DF-Soldering scores stage require", SQSS.ToString(), SQ0, SQ1, SQ2, SQ3, ResultSQF);
                    }
                }
                else if (SQS == 0)
                {
                    string ResultSQF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SQ", "DF-Soldering scores stage require", "0%", SQ0, SQ1, SQ2, SQ3, ResultSQF);
                }
                else
                {
                    string SQSS = SQS.ToString() + "%";
                    if (SQS > LevelEVT && SQ0 == "0" && SQ1 == "0")
                    {
                        string ResultSQP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SQ", "DF-Soldering scores stage require", SQSS.ToString(), SQ0, SQ1, SQ2, SQ3, ResultSQP);
                    }
                    else
                    {
                        string ResultSQF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SQ", "DF-Soldering scores stage require", SQSS.ToString(), SQ0, SQ1, SQ2, SQ3, ResultSQF);
                    }
                }
                #endregion
            }
            #endregion

            #endregion
        }
        else
        {
            DeleteDFXScore(DocNo);//先刪除原始記錄 在計算  防止Writechecked時修改分數

            #region 獲取 各Level階段的Open數量
            #region AI_RI
            DataTable dtAIRILevel0 = GetDFXLevel(DocNo, "AI_RI", "0");
            DataTable dtAIRILevel1 = GetDFXLevel(DocNo, "AI_RI", "1");
            DataTable dtAIRILevel2 = GetDFXLevel(DocNo, "AI_RI", "2");
            DataTable dtAIRILevel3 = GetDFXLevel(DocNo, "AI_RI", "3");
            string AIRI0 = dtAIRILevel0.Rows[0]["amount"].ToString() == "" ? "0" : dtAIRILevel0.Rows[0]["amount"].ToString();
            string AIRI1 = dtAIRILevel1.Rows[0]["amount"].ToString() == "" ? "0" : dtAIRILevel1.Rows[0]["amount"].ToString();
            string AIRI2 = dtAIRILevel2.Rows[0]["amount"].ToString() == "" ? "0" : dtAIRILevel2.Rows[0]["amount"].ToString();
            string AIRI3 = dtAIRILevel3.Rows[0]["amount"].ToString() == "" ? "0" : dtAIRILevel3.Rows[0]["amount"].ToString();
            #endregion

            #region SMT
            DataTable dtSMTLevel0 = GetDFXLevel(DocNo, "SMT", "0");
            DataTable dtSMTLevel1 = GetDFXLevel(DocNo, "SMT", "1");
            DataTable dtSMTLevel2 = GetDFXLevel(DocNo, "SMT", "2");
            DataTable dtSMTLevel3 = GetDFXLevel(DocNo, "SMT", "3");
            string SMT0 = dtSMTLevel0.Rows[0]["amount"].ToString() == "" ? "0" : dtSMTLevel0.Rows[0]["amount"].ToString();
            string SMT1 = dtSMTLevel1.Rows[0]["amount"].ToString() == "" ? "0" : dtSMTLevel1.Rows[0]["amount"].ToString();
            string SMT2 = dtSMTLevel2.Rows[0]["amount"].ToString() == "" ? "0" : dtSMTLevel2.Rows[0]["amount"].ToString();
            string SMT3 = dtSMTLevel3.Rows[0]["amount"].ToString() == "" ? "0" : dtSMTLevel3.Rows[0]["amount"].ToString();
            #endregion

            #region IE
            DataTable dtIELevel0 = GetDFXLevel(DocNo, "IE", "0");
            DataTable dtIELevel1 = GetDFXLevel(DocNo, "IE", "1");
            DataTable dtIELevel2 = GetDFXLevel(DocNo, "IE", "2");
            DataTable dtIELevel3 = GetDFXLevel(DocNo, "IE", "3");
            string IE0 = dtIELevel0.Rows[0]["amount"].ToString() == "" ? "0" : dtIELevel0.Rows[0]["amount"].ToString();
            string IE1 = dtIELevel1.Rows[0]["amount"].ToString() == "" ? "0" : dtIELevel1.Rows[0]["amount"].ToString();
            string IE2 = dtIELevel2.Rows[0]["amount"].ToString() == "" ? "0" : dtIELevel2.Rows[0]["amount"].ToString();
            string IE3 = dtIELevel3.Rows[0]["amount"].ToString() == "" ? "0" : dtIELevel3.Rows[0]["amount"].ToString();
            #endregion

            #region DQE
            DataTable dtDQELevel0 = GetDFXLevel(DocNo, "DQE", "0");
            DataTable dtDQELevel1 = GetDFXLevel(DocNo, "DQE", "1");
            DataTable dtDQELevel2 = GetDFXLevel(DocNo, "DQE", "2");
            DataTable dtDQELevel3 = GetDFXLevel(DocNo, "DQE", "3");
            string DQE0 = dtDQELevel0.Rows[0]["amount"].ToString() == "" ? "0" : dtDQELevel0.Rows[0]["amount"].ToString();
            string DQE1 = dtDQELevel1.Rows[0]["amount"].ToString() == "" ? "0" : dtDQELevel1.Rows[0]["amount"].ToString();
            string DQE2 = dtDQELevel2.Rows[0]["amount"].ToString() == "" ? "0" : dtDQELevel2.Rows[0]["amount"].ToString();
            string DQE3 = dtDQELevel3.Rows[0]["amount"].ToString() == "" ? "0" : dtDQELevel3.Rows[0]["amount"].ToString();
            #endregion

            #region EE
            DataTable dtEELevel0 = GetDFXLevel(DocNo, "EE", "0");
            DataTable dtEELevel1 = GetDFXLevel(DocNo, "EE", "1");
            DataTable dtEELevel2 = GetDFXLevel(DocNo, "EE", "2");
            DataTable dtEELevel3 = GetDFXLevel(DocNo, "EE", "3");
            string EE0 = dtEELevel0.Rows[0]["amount"].ToString() == "" ? "0" : dtEELevel0.Rows[0]["amount"].ToString();
            string EE1 = dtEELevel1.Rows[0]["amount"].ToString() == "" ? "0" : dtEELevel1.Rows[0]["amount"].ToString();
            string EE2 = dtEELevel2.Rows[0]["amount"].ToString() == "" ? "0" : dtEELevel2.Rows[0]["amount"].ToString();
            string EE3 = dtEELevel3.Rows[0]["amount"].ToString() == "" ? "0" : dtEELevel3.Rows[0]["amount"].ToString();
            #endregion

            #region UQ
            DataTable dtUQLevel0 = GetDFXLevel(DocNo, "UQ", "0");
            DataTable dtUQLevel1 = GetDFXLevel(DocNo, "UQ", "1");
            DataTable dtUQLevel2 = GetDFXLevel(DocNo, "UQ", "2");
            DataTable dtUQLevel3 = GetDFXLevel(DocNo, "UQ", "3");
            string UQ0 = dtUQLevel0.Rows[0]["amount"].ToString() == "" ? "0" : dtUQLevel0.Rows[0]["amount"].ToString();
            string UQ1 = dtUQLevel1.Rows[0]["amount"].ToString() == "" ? "0" : dtUQLevel1.Rows[0]["amount"].ToString();
            string UQ2 = dtUQLevel2.Rows[0]["amount"].ToString() == "" ? "0" : dtUQLevel2.Rows[0]["amount"].ToString();
            string UQ3 = dtUQLevel3.Rows[0]["amount"].ToString() == "" ? "0" : dtUQLevel3.Rows[0]["amount"].ToString();
            #endregion

            #region SQ
            DataTable dtSQLevel0 = GetDFXLevel(DocNo, "SQ", "0");
            DataTable dtSQLevel1 = GetDFXLevel(DocNo, "SQ", "1");
            DataTable dtSQLevel2 = GetDFXLevel(DocNo, "SQ", "2");
            DataTable dtSQLevel3 = GetDFXLevel(DocNo, "SQ", "3");
            string SQ0 = dtSQLevel0.Rows[0]["amount"].ToString() == "" ? "0" : dtSQLevel0.Rows[0]["amount"].ToString();
            string SQ1 = dtSQLevel1.Rows[0]["amount"].ToString() == "" ? "0" : dtSQLevel1.Rows[0]["amount"].ToString();
            string SQ2 = dtSQLevel2.Rows[0]["amount"].ToString() == "" ? "0" : dtSQLevel2.Rows[0]["amount"].ToString();
            string SQ3 = dtSQLevel3.Rows[0]["amount"].ToString() == "" ? "0" : dtSQLevel3.Rows[0]["amount"].ToString();

            #endregion

            #region EVT階段 DFX統計  >=80
            if (drM["SUB_DOC_PHASE"].ToString().Contains("EVT"))
            {
                int LevelEVT = 80;

                #region AIRI
                DataTable dtAIRIS = GetDFXScore(DocNo, "AI_RI");//計算AI_RI的分數
                double AIRIS = (double.Parse(dtAIRIS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtAIRIS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtAIRIS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtAIRIS.Rows[0]["MaxPoints"].ToString())) * 100;
                if (AIRIS != 100 && AIRIS != 0)
                {
                    string AIRISS = String.Format("{0:N1}", AIRIS) + "%";
                    if (AIRIS >= LevelEVT && AIRI0 == "0")
                    {
                        string ResultAIRIP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "AI_RI", "DF-AI/RI scores stage require", AIRISS, AIRI0, AIRI1, AIRI2, AIRI3, ResultAIRIP);
                    }
                    else
                    {
                        string ResultAIRIF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "AI_RI", "DF-AI/RI scores stage require", AIRISS, AIRI0, AIRI1, AIRI2, AIRI3, ResultAIRIF);
                    }
                }
                else if (AIRIS == 0)
                {
                    string AIRISS = "0" + "%";
                    string ResultAIRIF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "AI_RI", "DF-AI/RI scores stage require", AIRISS, AIRI0, AIRI1, AIRI2, AIRI3, ResultAIRIF);
                }
                else
                {
                    string AIRISS = AIRIS.ToString() + "%";
                    if (AIRIS >= LevelEVT && AIRI0 == "0")
                    {
                        string ResultAIRIP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "AI_RI", "DF-AI/RI scores stage require", AIRISS, AIRI0, AIRI1, AIRI2, AIRI3, ResultAIRIP);
                    }
                    else
                    {
                        string ResultAIRIF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "AI_RI", "DF-AI/RI scores stage require", AIRISS, AIRI0, AIRI1, AIRI2, AIRI3, ResultAIRIF);
                    }
                }
                #endregion

                #region SMT
                DataTable dtSMTS = GetDFXScore(DocNo, "SMT");
                double SMTS = (double.Parse(dtSMTS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtSMTS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtSMTS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtSMTS.Rows[0]["MaxPoints"].ToString())) * 100;
                if (SMTS != 100 && SMTS != 0)
                {
                    string SMTSS = String.Format("{0:N1}", SMTS) + "%";
                    if (SMTS > LevelEVT && SMT0 == "0")
                    {
                        string ResultSMTP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SMT", "DF-SMD scores stage require", SMTSS, SMT0, SMT1, SMT2, SMT3, ResultSMTP);
                    }
                    else
                    {
                        string ResultSMTF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SMT", "DF-SMD scores stage require", SMTSS, SMT0, SMT1, SMT2, SMT3, ResultSMTF);
                    }
                }
                else if (SMTS == 0)
                {
                    string SMTSS = "0" + "%";
                    string ResultSMTF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SMT", "DF-SMD scores stage require", SMTSS, SMT0, SMT1, SMT2, SMT3, ResultSMTF);
                }
                else
                {
                    string SMTSS = SMTS.ToString() + "%";
                    if (SMTS > LevelEVT && SMT0 == "0")
                    {
                        string ResultSMTP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SMT", "DF-SMD scores stage require", SMTSS, SMT0, SMT1, SMT2, SMT3, ResultSMTP);
                    }
                    else
                    {
                        string ResultSMTF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SMT", "DF-SMD scores stage require", SMTSS, SMT0, SMT1, SMT2, SMT3, ResultSMTF);
                    }
                }
                #endregion

                #region IE
                DataTable dtIES = GetDFXScore(DocNo, "IE");
                double IES = (double.Parse(dtIES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtIES.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtIES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtIES.Rows[0]["MaxPoints"].ToString())) * 100;
                if (IES != 100 && IES != 0)
                {
                    string IESS = String.Format("{0:N1}", IES) + "%";
                    if (IES > LevelEVT && IE0 == "0")
                    {
                        string ResultIEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "IE", "DF-Process scores stage require", IESS, IE0, IE1, IE2, IE3, ResultIEP);
                    }
                    else
                    {
                        string ResultIEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "IE", "DF-Process scores stage require", IESS, IE0, IE1, IE2, IE3, ResultIEF);
                    }
                }
                else if (IES == 0)
                {
                    string IESS = "0" + "%";
                    string ResultIEF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "IE", "DF-Process scores stage require", IESS, IE0, IE1, IE2, IE3, ResultIEF);
                }
                else
                {
                    string IESS = IES.ToString() + "%";
                    if (IES > LevelEVT && IE0 == "0")
                    {
                        string ResultIEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "IE", "DF-Process scores stage require", IESS, IE0, IE1, IE2, IE3, ResultIEP);
                    }
                    else
                    {
                        string ResultIEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "IE", "DF-Process scores stage require", IESS, IE0, IE1, IE2, IE3, ResultIEF);
                    }
                }
                #endregion

                #region DQE
                DataTable dtDQES = GetDFXScore(DocNo, "DQE");
                double DQES = (double.Parse(dtDQES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtDQES.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtDQES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtDQES.Rows[0]["MaxPoints"].ToString())) * 100;
                if (DQES != 100 && DQES != 0)
                {
                    string DQESS = String.Format("{0:N1}", DQES) + "%";
                    if (DQES > LevelEVT && DQE0 == "0")
                    {
                        string ResultDQEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "DQE", "DF-Reliability scores stage require", DQESS, DQE0, DQE1, DQE2, DQE3, ResultDQEP);
                    }
                    else
                    {
                        string ResultDQEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "DQE", "DF-Reliability scores stage require", DQESS, DQE0, DQE1, DQE2, DQE3, ResultDQEF);
                    }
                }
                else if (DQES == 0)
                {
                    string DQESS = "0" + "%";
                    string ResultDQEF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "DQE", "DF-Reliability scores stage require", DQESS, DQE0, DQE1, DQE2, DQE3, ResultDQEF);
                }
                else
                {
                    string DQESS = DQES.ToString() + "%";
                    if (DQES > LevelEVT && DQE0 == "0")
                    {
                        string ResultDQEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "DQE", "DF-Reliability scores stage require", DQESS, DQE0, DQE1, DQE2, DQE3, ResultDQEP);
                    }
                    else
                    {
                        string ResultDQEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "DQE", "DF-Reliability scores stage require", DQESS, DQE0, DQE1, DQE2, DQE3, ResultDQEF);
                    }
                }

                #endregion

                #region EE
                DataTable dtEES = GetDFXScore(DocNo, "EE");
                double EES = (double.Parse(dtEES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtEES.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtEES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtEES.Rows[0]["MaxPoints"].ToString())) * 100;
                if (EES != 100 && EES != 0)
                {
                    string EESS = String.Format("{0:N1}", EES) + "%";
                    if (EES > LevelEVT && EE0 == "0")
                    {
                        string ResultEEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "EE", "DF-EE scores stage require", EESS, EE0, EE1, EE2, EE3, ResultEEP);
                    }
                    else
                    {
                        string ResultEEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "EE", "DF-EE scores stage require", EESS, EE0, EE1, EE2, EE3, ResultEEF);
                    }
                }
                else if (EES == 0)
                {
                    string EESS = "0" + "%";
                    string ResultEEF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "EE", "DF-EE scores stage require", EESS, EE0, EE1, EE2, EE3, ResultEEF);
                }
                else
                {
                    string EESS = EES.ToString() + "%";
                    if (EES > LevelEVT && EE0 == "0")
                    {
                        string ResultEEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "EE", "DF-EE scores stage require", EESS, EE0, EE1, EE2, EE3, ResultEEP);
                    }
                    else
                    {
                        string ResultEEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "EE", "DF-EE scores stage require", EESS, EE0, EE1, EE2, EE3, ResultEEF);
                    }
                }
                #endregion

                #region UQ
                DataTable dtUQS = GetDFXScore(DocNo, "UQ");
                double UQS = (double.Parse(dtUQS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtUQS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtUQS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtUQS.Rows[0]["MaxPoints"].ToString())) * 100;
                if (UQS != 100 && UQS != 0)
                {
                    string UQSS = String.Format("{0:N1}", UQS) + "%";
                    if (UQS > LevelEVT && UQ0 == "0")
                    {
                        string ResultUQP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "UQ", "DF-UQ scores stage require", UQSS, UQ0, UQ1, UQ2, UQ3, ResultUQP);

                    }
                    else
                    {
                        string ResultUQF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "UQ", "DF-UQ scores stage require", UQSS, UQ0, UQ1, UQ2, UQ3, ResultUQF);

                    }
                }
                else if (UQS == 0)
                {
                    string UQSS = "0" + "%";
                    string ResultUQF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "UQ", "DF-UQ scores stage require", UQSS, UQ0, UQ1, UQ2, UQ3, ResultUQF);
                }
                else
                {
                    string UQSS = UQS.ToString() + "%";
                    if (UQS > LevelEVT && UQ0 == "0")
                    {
                        string ResultUQP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "UQ", "DF-UQ scores stage require", UQSS, UQ0, UQ1, UQ2, UQ3, ResultUQP);

                    }
                    else
                    {
                        string ResultUQF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "UQ", "DF-UQ scores stage require", UQSS, UQ0, UQ1, UQ2, UQ3, ResultUQF);

                    }
                }
                #endregion

                #region SQ
                DataTable dtSQS = GetDFXScore(DocNo, "SQ");
                double SQS = (double.Parse(dtSQS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtSQS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtSQS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtSQS.Rows[0]["MaxPoints"].ToString())) * 100;
                if (SQS != 100 && SQS != 0)
                {
                    string SQSS = String.Format("{0:N1}", SQS) + "%";
                    if (SQS > LevelEVT && SQ0 == "0")
                    {
                        string ResultSQP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SQ", "DF-Soldering scores stage require", SQSS.ToString(), SQ0, SQ1, SQ2, SQ3, ResultSQP);
                    }
                    else
                    {
                        string ResultSQF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SQ", "DF-Soldering scores stage require", SQSS.ToString(), SQ0, SQ1, SQ2, SQ3, ResultSQF);
                    }
                }
                else if (SQS == 0)
                {
                    string SQSS = "0" + "%";
                    string ResultSQF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SQ", "DF-Soldering scores stage require", SQSS.ToString(), SQ0, SQ1, SQ2, SQ3, ResultSQF);
                }
                else
                {
                    string SQSS = SQS.ToString() + "%";
                    if (SQS > LevelEVT && SQ0 == "0")
                    {
                        string ResultSQP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SQ", "DF-Soldering scores stage require", SQSS.ToString(), SQ0, SQ1, SQ2, SQ3, ResultSQP);
                    }
                    else
                    {
                        string ResultSQF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SQ", "DF-Soldering scores stage require", SQSS.ToString(), SQ0, SQ1, SQ2, SQ3, ResultSQF);
                    }
                }
                #endregion


            }
            #endregion

            #region DVT階段 DFX統計  >=90
            else if (drM["SUB_DOC_PHASE"].ToString().Contains("DVT"))
            {
                int LevelEVT = 90;

                #region AIRI
                DataTable dtAIRIS = GetDFXScore(DocNo, "AI_RI");//計算AI_RI的分數
                double AIRIS = (double.Parse(dtAIRIS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtAIRIS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtAIRIS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtAIRIS.Rows[0]["MaxPoints"].ToString())) * 100;
                if (AIRIS != 100 && AIRIS != 0)
                {
                    string AIRISS = String.Format("{0:N1}", AIRIS) + "%";
                    if (AIRIS >= LevelEVT && AIRI0 == "0" && AIRI1 == "0")
                    {
                        string ResultAIRIP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "AI_RI", "DF-AI/RI scores stage require", AIRISS, AIRI0, AIRI1, AIRI2, AIRI3, ResultAIRIP);
                    }
                    else
                    {
                        string ResultAIRIF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "AI_RI", "DF-AI/RI scores stage require", AIRISS, AIRI0, AIRI1, AIRI2, AIRI3, ResultAIRIF);
                    }
                }
                else if (AIRIS == 0)
                {
                    string ResultAIRIF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "AI_RI", "DF-AI/RI scores stage require", "0%", AIRI0, AIRI1, AIRI2, AIRI3, ResultAIRIF);
                }
                else
                {
                    string AIRISS = AIRIS.ToString() + "%";
                    if (AIRIS >= LevelEVT && AIRI0 == "0" && AIRI1 == "0")
                    {
                        string ResultAIRIP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "AI_RI", "DF-AI/RI scores stage require", AIRISS, AIRI0, AIRI1, AIRI2, AIRI3, ResultAIRIP);
                    }
                    else
                    {
                        string ResultAIRIF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "AI_RI", "DF-AI/RI scores stage require", AIRISS, AIRI0, AIRI1, AIRI2, AIRI3, ResultAIRIF);
                    }
                }
                #endregion

                #region SMT
                DataTable dtSMTS = GetDFXScore(DocNo, "SMT");
                double SMTS = (double.Parse(dtSMTS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtSMTS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtSMTS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtSMTS.Rows[0]["MaxPoints"].ToString())) * 100;
                if (SMTS != 100 && SMTS != 0)
                {
                    string SMTSS = String.Format("{0:N1}", SMTS) + "%";
                    if (SMTS > LevelEVT && SMT0 == "0" && SMT1 == "0")
                    {
                        string ResultSMTP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SMT", "DF-SMD scores stage require", SMTSS, SMT0, SMT1, SMT2, SMT3, ResultSMTP);
                    }
                    else
                    {
                        string ResultSMTF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SMT", "DF-SMD scores stage require", SMTSS, SMT0, SMT1, SMT2, SMT3, ResultSMTF);
                    }
                }
                else if (SMTS == 0)
                {
                    string ResultSMTF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SMT", "DF-SMD scores stage require", "0%", SMT0, SMT1, SMT2, SMT3, ResultSMTF);
                }
                else
                {
                    string SMTSS = SMTS.ToString() + "%";
                    if (SMTS > LevelEVT && SMT0 == "0" && SMT1 == "0")
                    {
                        string ResultSMTP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SMT", "DF-SMD scores stage require", SMTSS, SMT0, SMT1, SMT2, SMT3, ResultSMTP);
                    }
                    else
                    {
                        string ResultSMTF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SMT", "DF-SMD scores stage require", SMTSS, SMT0, SMT1, SMT2, SMT3, ResultSMTF);
                    }
                }
                #endregion

                #region IE
                DataTable dtIES = GetDFXScore(DocNo, "IE");
                double IES = (double.Parse(dtIES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtIES.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtIES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtIES.Rows[0]["MaxPoints"].ToString())) * 100;
                if (IES != 100 && IES != 0)
                {
                    string IESS = String.Format("{0:N1}", IES) + "%";
                    if (IES > LevelEVT && IE0 == "0" && IE1 == "0")
                    {
                        string ResultIEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "IE", "DF-Process scores stage require", IESS, IE0, IE1, IE2, IE3, ResultIEP);
                    }
                    else
                    {
                        string ResultIEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "IE", "DF-Process scores stage require", IESS, IE0, IE1, IE2, IE3, ResultIEF);
                    }
                }
                else if (IES == 0)
                {
                    string ResultIEF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "IE", "DF-Process scores stage require", "0%", IE0, IE1, IE2, IE3, ResultIEF);
                }
                else
                {
                    string IESS = IES.ToString() + "%";
                    if (IES > LevelEVT && IE0 == "0" && IE1 == "0")
                    {
                        string ResultIEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "IE", "DF-Process scores stage require", IESS, IE0, IE1, IE2, IE3, ResultIEP);
                    }
                    else
                    {
                        string ResultIEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "IE", "DF-Process scores stage require", IESS, IE0, IE1, IE2, IE3, ResultIEF);
                    }
                }
                #endregion

                #region DQE
                DataTable dtDQES = GetDFXScore(DocNo, "DQE");
                double DQES = (double.Parse(dtDQES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtDQES.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtDQES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtDQES.Rows[0]["MaxPoints"].ToString())) * 100;
                if (DQES != 100 && DQES != 0)
                {
                    string DQESS = String.Format("{0:N1}", DQES) + "%";
                    if (DQES > LevelEVT && DQE0 == "0" && DQE1 == "0")
                    {
                        string ResultDQEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "DQE", "DF-Reliability scores stage require", DQESS, DQE0, DQE1, DQE2, DQE3, ResultDQEP);
                    }
                    else
                    {
                        string ResultDQEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "DQE", "DF-Reliability scores stage require", DQESS, DQE0, DQE1, DQE2, DQE3, ResultDQEF);
                    }
                }
                else if (DQES == 0)
                {
                    string ResultDQEF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "DQE", "DF-Reliability scores stage require", "0%", DQE0, DQE1, DQE2, DQE3, ResultDQEF);
                }
                else
                {
                    string DQESS = DQES.ToString() + "%";
                    if (DQES > LevelEVT && DQE0 == "0" && DQE1 == "0")
                    {
                        string ResultDQEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "DQE", "DF-Reliability scores stage require", DQESS, DQE0, DQE1, DQE2, DQE3, ResultDQEP);
                    }
                    else
                    {
                        string ResultDQEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "DQE", "DF-Reliability scores stage require", DQESS, DQE0, DQE1, DQE2, DQE3, ResultDQEF);
                    }
                }

                #endregion

                #region EE
                DataTable dtEES = GetDFXScore(DocNo, "EE");
                double EES = (double.Parse(dtEES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtEES.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtEES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtEES.Rows[0]["MaxPoints"].ToString())) * 100;
                if (EES != 100 && EES != 0)
                {
                    string EESS = String.Format("{0:N1}", EES) + "%";
                    if (EES > LevelEVT && EE0 == "0" && EE1 == "0")
                    {
                        string ResultEEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "EE", "DF-EE scores stage require", EESS, EE0, EE1, EE2, EE3, ResultEEP);
                    }
                    else
                    {
                        string ResultEEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "EE", "DF-EE scores stage require", EESS, EE0, EE1, EE2, EE3, ResultEEF);
                    }
                }
                else if (EES == 0)
                {
                    string ResultEEF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "EE", "DF-EE scores stage require", "0%", EE0, EE1, EE2, EE3, ResultEEF);
                }
                else
                {
                    string EESS = EES.ToString() + "%";
                    if (EES > LevelEVT && EE0 == "0" && EE1 == "0")
                    {
                        string ResultEEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "EE", "DF-EE scores stage require", EESS, EE0, EE1, EE2, EE3, ResultEEP);
                    }
                    else
                    {
                        string ResultEEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "EE", "DF-EE scores stage require", EESS, EE0, EE1, EE2, EE3, ResultEEF);
                    }
                }
                #endregion

                #region UQ
                DataTable dtUQS = GetDFXScore(DocNo, "UQ");
                double UQS = (double.Parse(dtUQS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtUQS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtUQS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtUQS.Rows[0]["MaxPoints"].ToString())) * 100;
                if (UQS != 100 && UQS != 0)
                {
                    string UQSS = String.Format("{0:N1}", UQS) + "%";
                    if (UQS > LevelEVT && UQ0 == "0" && UQ1 == "0")
                    {
                        string ResultUQP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "UQ", "DF-UQ scores stage require", UQSS, UQ0, UQ1, UQ2, UQ3, ResultUQP);

                    }
                    else
                    {
                        string ResultUQF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "UQ", "DF-UQ scores stage require", UQSS, UQ0, UQ1, UQ2, UQ3, ResultUQF);

                    }
                }
                else if (UQS == 0)
                {
                    string ResultUQF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "UQ", "DF-UQ scores stage require", "0%", UQ0, UQ1, UQ2, UQ3, ResultUQF);
                }
                else
                {
                    string UQSS = UQS.ToString() + "%";
                    if (UQS > LevelEVT && UQ0 == "0" && UQ1 == "0")
                    {
                        string ResultUQP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "UQ", "DF-UQ scores stage require", UQSS, UQ0, UQ1, UQ2, UQ3, ResultUQP);

                    }
                    else
                    {
                        string ResultUQF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "UQ", "DF-UQ scores stage require", UQSS, UQ0, UQ1, UQ2, UQ3, ResultUQF);

                    }
                }
                #endregion

                #region SQ
                DataTable dtSQS = GetDFXScore(DocNo, "SQ");
                double SQS = (double.Parse(dtSQS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtSQS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtSQS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtSQS.Rows[0]["MaxPoints"].ToString())) * 100;
                if (SQS != 100 && SQS != 0)
                {
                    string SQSS = String.Format("{0:N1}", SQS) + "%";
                    if (SQS > LevelEVT && SQ0 == "0" && SQ1 == "0")
                    {
                        string ResultSQP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SQ", "DF-Soldering scores stage require", SQSS.ToString(), SQ0, SQ1, SQ2, SQ3, ResultSQP);
                    }
                    else
                    {
                        string ResultSQF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SQ", "DF-Soldering scores stage require", SQSS.ToString(), SQ0, SQ1, SQ2, SQ3, ResultSQF);
                    }
                }
                else if (SQS == 0)
                {
                    string ResultSQF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SQ", "DF-Soldering scores stage require", "0%", SQ0, SQ1, SQ2, SQ3, ResultSQF);
                }
                else
                {
                    string SQSS = SQS.ToString() + "%";
                    if (SQS > LevelEVT && SQ0 == "0" && SQ1 == "0")
                    {
                        string ResultSQP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SQ", "DF-Soldering scores stage require", SQSS.ToString(), SQ0, SQ1, SQ2, SQ3, ResultSQP);
                    }
                    else
                    {
                        string ResultSQF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SQ", "DF-Soldering scores stage require", SQSS.ToString(), SQ0, SQ1, SQ2, SQ3, ResultSQF);
                    }
                }
                #endregion
            }



            #endregion

            #region PR階段 DFX統計  >=90
            else if (drM["SUB_DOC_PHASE"].ToString().Contains("P.Run"))
            {
                int LevelEVT = 90;

                #region AIRI
                DataTable dtAIRIS = GetDFXScore(DocNo, "AI_RI");//計算AI_RI的分數
                double AIRIS = (double.Parse(dtAIRIS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtAIRIS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtAIRIS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtAIRIS.Rows[0]["MaxPoints"].ToString())) * 100;
                if (AIRIS != 100 && AIRIS != 0)
                {
                    string AIRISS = String.Format("{0:N1}", AIRIS) + "%";
                    if (AIRIS >= LevelEVT && AIRI0 == "0" && AIRI1 == "0" && AIRI2 == "0")
                    {
                        string ResultAIRIP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "AI_RI", "DF-AI/RI scores stage require", AIRISS, AIRI0, AIRI1, AIRI2, AIRI3, ResultAIRIP);
                    }
                    else
                    {
                        string ResultAIRIF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "AI_RI", "DF-AI/RI scores stage require", AIRISS, AIRI0, AIRI1, AIRI2, AIRI3, ResultAIRIF);
                    }
                }
                else if (AIRIS == 0)
                {
                    string ResultAIRIF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "AI_RI", "DF-AI/RI scores stage require", "0%", AIRI0, AIRI1, AIRI2, AIRI3, ResultAIRIF);
                }
                else
                {
                    string AIRISS = AIRIS.ToString() + "%";
                    if (AIRIS >= LevelEVT && AIRI0 == "0" && AIRI1 == "0")
                    {
                        string ResultAIRIP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "AI_RI", "DF-AI/RI scores stage require", AIRISS, AIRI0, AIRI1, AIRI2, AIRI3, ResultAIRIP);
                    }
                    else
                    {
                        string ResultAIRIF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "AI_RI", "DF-AI/RI scores stage require", AIRISS, AIRI0, AIRI1, AIRI2, AIRI3, ResultAIRIF);
                    }
                }
                #endregion

                #region SMT
                DataTable dtSMTS = GetDFXScore(DocNo, "SMT");
                double SMTS = (double.Parse(dtSMTS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtSMTS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtSMTS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtSMTS.Rows[0]["MaxPoints"].ToString())) * 100;
                if (SMTS != 100 && SMTS != 0)
                {
                    string SMTSS = String.Format("{0:N1}", SMTS) + "%";
                    if (SMTS > LevelEVT && SMT0 == "0" && SMT1 == "0" && SMT2 == "0")
                    {
                        string ResultSMTP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SMT", "DF-SMD scores stage require", SMTSS, SMT0, SMT1, SMT2, SMT3, ResultSMTP);
                    }
                    else
                    {
                        string ResultSMTF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SMT", "DF-SMD scores stage require", SMTSS, SMT0, SMT1, SMT2, SMT3, ResultSMTF);
                    }
                }
                else if (SMTS == 0)
                {
                    string ResultSMTF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SMT", "DF-SMD scores stage require", "0%", SMT0, SMT1, SMT2, SMT3, ResultSMTF);
                }
                else
                {
                    string SMTSS = SMTS.ToString() + "%";
                    if (SMTS > LevelEVT && SMT0 == "0" && SMT1 == "0")
                    {
                        string ResultSMTP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SMT", "DF-SMD scores stage require", SMTSS, SMT0, SMT1, SMT2, SMT3, ResultSMTP);
                    }
                    else
                    {
                        string ResultSMTF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SMT", "DF-SMD scores stage require", SMTSS, SMT0, SMT1, SMT2, SMT3, ResultSMTF);
                    }
                }
                #endregion

                #region IE
                DataTable dtIES = GetDFXScore(DocNo, "IE");
                double IES = (double.Parse(dtIES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtIES.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtIES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtIES.Rows[0]["MaxPoints"].ToString())) * 100;
                if (IES != 100 && IES != 0)
                {
                    string IESS = String.Format("{0:N1}", IES) + "%";
                    if (IES > LevelEVT && IE0 == "0" && IE1 == "0" && IE2 == "0")
                    {
                        string ResultIEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "IE", "DF-Process scores stage require", IESS, IE0, IE1, IE2, IE3, ResultIEP);
                    }
                    else
                    {
                        string ResultIEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "IE", "DF-Process scores stage require", IESS, IE0, IE1, IE2, IE3, ResultIEF);
                    }
                }
                else if (IES == 0)
                {
                    string ResultIEF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "IE", "DF-Process scores stage require", "0%", IE0, IE1, IE2, IE3, ResultIEF);
                }
                else
                {
                    string IESS = IES.ToString() + "%";
                    if (IES > LevelEVT && IE0 == "0" && IE1 == "0")
                    {
                        string ResultIEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "IE", "DF-Process scores stage require", IESS, IE0, IE1, IE2, IE3, ResultIEP);
                    }
                    else
                    {
                        string ResultIEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "IE", "DF-Process scores stage require", IESS, IE0, IE1, IE2, IE3, ResultIEF);
                    }
                }
                #endregion

                #region DQE
                DataTable dtDQES = GetDFXScore(DocNo, "DQE");
                double DQES = (double.Parse(dtDQES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtDQES.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtDQES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtDQES.Rows[0]["MaxPoints"].ToString())) * 100;
                if (DQES != 100 && DQES != 0)
                {
                    string DQESS = String.Format("{0:N1}", DQES) + "%";
                    if (DQES > LevelEVT && DQE0 == "0" && DQE1 == "0" && DQE2 == "0")
                    {
                        string ResultDQEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "DQE", "DF-Reliability scores stage require", DQESS, DQE0, DQE1, DQE2, DQE3, ResultDQEP);
                    }
                    else
                    {
                        string ResultDQEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "DQE", "DF-Reliability scores stage require", DQESS, DQE0, DQE1, DQE2, DQE3, ResultDQEF);
                    }
                }
                else if (DQES == 0)
                {
                    string ResultDQEF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "DQE", "DF-Reliability scores stage require", "0%", DQE0, DQE1, DQE2, DQE3, ResultDQEF);
                }
                else
                {
                    string DQESS = DQES.ToString() + "%";
                    if (DQES > LevelEVT && DQE0 == "0" && DQE1 == "0")
                    {
                        string ResultDQEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "DQE", "DF-Reliability scores stage require", DQESS, DQE0, DQE1, DQE2, DQE3, ResultDQEP);
                    }
                    else
                    {
                        string ResultDQEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "DQE", "DF-Reliability scores stage require", DQESS, DQE0, DQE1, DQE2, DQE3, ResultDQEF);
                    }
                }

                #endregion

                #region EE
                DataTable dtEES = GetDFXScore(DocNo, "EE");
                double EES = (double.Parse(dtEES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtEES.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtEES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtEES.Rows[0]["MaxPoints"].ToString())) * 100;
                if (EES != 100 && EES != 0)
                {
                    string EESS = String.Format("{0:N1}", EES) + "%";
                    if (EES > LevelEVT && EE0 == "0" && EE1 == "0" && EE2 == "0")
                    {
                        string ResultEEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "EE", "DF-EE scores stage require", EESS, EE0, EE1, EE2, EE3, ResultEEP);
                    }
                    else
                    {
                        string ResultEEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "EE", "DF-EE scores stage require", EESS, EE0, EE1, EE2, EE3, ResultEEF);
                    }
                }
                else if (EES == 0)
                {
                    string ResultEEF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "EE", "DF-EE scores stage require", "0%", EE0, EE1, EE2, EE3, ResultEEF);
                }
                else
                {
                    string EESS = EES.ToString() + "%";
                    if (EES > LevelEVT && EE0 == "0" && EE1 == "0")
                    {
                        string ResultEEP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "EE", "DF-EE scores stage require", EESS, EE0, EE1, EE2, EE3, ResultEEP);
                    }
                    else
                    {
                        string ResultEEF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "EE", "DF-EE scores stage require", EESS, EE0, EE1, EE2, EE3, ResultEEF);
                    }
                }
                #endregion

                #region UQ
                DataTable dtUQS = GetDFXScore(DocNo, "UQ");
                double UQS = (double.Parse(dtUQS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtUQS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtUQS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtUQS.Rows[0]["MaxPoints"].ToString())) * 100;
                if (UQS != 100 && UQS != 0)
                {
                    string UQSS = String.Format("{0:N1}", UQS) + "%";
                    if (UQS > LevelEVT && UQ0 == "0" && UQ1 == "0" && UQ2 == "0")
                    {
                        string ResultUQP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "UQ", "DF-UQ scores stage require", UQSS, UQ0, UQ1, UQ2, UQ3, ResultUQP);

                    }
                    else
                    {
                        string ResultUQF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "UQ", "DF-UQ scores stage require", UQSS, UQ0, UQ1, UQ2, UQ3, ResultUQF);

                    }
                }
                else if (UQS == 0)
                {
                    string ResultUQF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "UQ", "DF-UQ scores stage require", "0%", UQ0, UQ1, UQ2, UQ3, ResultUQF);
                }
                else
                {
                    string UQSS = UQS.ToString() + "%";
                    if (UQS > LevelEVT && UQ0 == "0" && UQ1 == "0")
                    {
                        string ResultUQP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "UQ", "DF-UQ scores stage require", UQSS, UQ0, UQ1, UQ2, UQ3, ResultUQP);

                    }
                    else
                    {
                        string ResultUQF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "UQ", "DF-UQ scores stage require", UQSS, UQ0, UQ1, UQ2, UQ3, ResultUQF);

                    }
                }
                #endregion

                #region SQ
                DataTable dtSQS = GetDFXScore(DocNo, "SQ");
                double SQS = (double.Parse(dtSQS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtSQS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtSQS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtSQS.Rows[0]["MaxPoints"].ToString())) * 100;
                if (SQS != 100 && SQS != 0)
                {
                    string SQSS = String.Format("{0:N1}", SQS) + "%";
                    if (SQS > LevelEVT && SQ0 == "0" && SQ1 == "0" && SQ2 == "0")
                    {
                        string ResultSQP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SQ", "DF-Soldering scores stage require", SQSS.ToString(), SQ0, SQ1, SQ2, SQ3, ResultSQP);
                    }
                    else
                    {
                        string ResultSQF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SQ", "DF-Soldering scores stage require", SQSS.ToString(), SQ0, SQ1, SQ2, SQ3, ResultSQF);
                    }
                }
                else if (SQS == 0)
                {
                    string ResultSQF = "NA";
                    InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SQ", "DF-Soldering scores stage require", "0%", SQ0, SQ1, SQ2, SQ3, ResultSQF);
                }
                else
                {
                    string SQSS = SQS.ToString() + "%";
                    if (SQS > LevelEVT && SQ0 == "0" && SQ1 == "0")
                    {
                        string ResultSQP = "PASS";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SQ", "DF-Soldering scores stage require", SQSS.ToString(), SQ0, SQ1, SQ2, SQ3, ResultSQP);
                    }
                    else
                    {
                        string ResultSQF = "FAIL";
                        InsertDFXScore(DocNo, drM["SUB_DOC_PHASE"].ToString(), "SQ", "DF-Soldering scores stage require", SQSS.ToString(), SQ0, SQ1, SQ2, SQ3, ResultSQF);
                    }
                }
                #endregion
            }
            #endregion

            #endregion
        }


    }

    #endregion

    #region  DFX根據Priority Level(權重性) 0,1,2,3 抓取Item的N項
    protected DataTable GetDFXLevel(string DocNo, string Dept, string PriorityLevel)
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select Count(*) as amount from TB_DFX_ItemBody 
                    where  DFXNo = @DFXNo and WriteDept = @WriteDept 
                    and Compliance = @Compliance and PriorityLevel = @PriorityLevel");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DFXNo",DbType.String, DocNo));
        opc.Add(DataPara.CreateDataParameter("@WriteDept", DbType.String, Dept));
        opc.Add(DataPara.CreateDataParameter("@Compliance", DbType.String, "N"));
        opc.Add(DataPara.CreateDataParameter("@PriorityLevel", DbType.String, PriorityLevel));
        DataTable dt = sdb.TransactionExecute(sb.ToString(),opc);
        return dt;
    }
    #endregion

    #region 刪除DFX Score記錄 By DocNo
    protected void DeleteDFXScore(string DocNo)
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        StringBuilder sb = new StringBuilder();
        sb.Append(@"delete from TB_DFX_Score where DFXNo =@DFXNo");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DFXNo", DbType.String, DocNo));
        sdb.TransactionExecuteScalar(sb.ToString(), opc);
    }
    #endregion

    #region 撈取是否存在此單號的DFX分數的記錄
    protected DataTable CheckDFXScore(string DocNo)
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        StringBuilder sb = new StringBuilder();
        sb.Append(@"Select * from TB_DFX_Score where DFXNo = @DFXNo");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DFXNo", DbType.String, DocNo));
        DataTable dt = sdb.TransactionExecute(sb.ToString(), opc);
        return dt;
    }

    #endregion

    #region Insert TB_DFX_Score
    protected void InsertDFXScore(string DocNo, string Stage, string Dept, string item, string Score, string Level0, string Level1, string Level2, string Level3,string Result)
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
                    INSERT INTO [NPI_REPORT].[dbo].[TB_DFX_Score]
                               ([DFXNo]
                               ,[Stage]
                               ,[Dept]
                               ,[item]
                               ,[Score]
                               ,[PriorityLevel0]
                               ,[PriorityLevel1]
                               ,[PriorityLevel2]
                               ,[PriorityLevel3]
                               ,[Result])
                         VALUES
                               (@DFXNo,
                                @Stage,
                                @Dept,
                                @item,
                                @Score,
                                @PriorityLevel0,
                                @PriorityLevel1,
                                @PriorityLevel2,
                                @PriorityLevel3,
                                @Result)                
                    ");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DFXNo", DbType.String, DocNo));
        opc.Add(DataPara.CreateDataParameter("@Stage", DbType.String, Stage));
        opc.Add(DataPara.CreateDataParameter("@Dept", DbType.String, Dept));
        opc.Add(DataPara.CreateDataParameter("@item", DbType.String, item));
        opc.Add(DataPara.CreateDataParameter("@Score", DbType.String, Score));
        opc.Add(DataPara.CreateDataParameter("@PriorityLevel0", DbType.String, Level0));
        opc.Add(DataPara.CreateDataParameter("@PriorityLevel1", DbType.String, Level1));
        opc.Add(DataPara.CreateDataParameter("@PriorityLevel2", DbType.String, Level2));
        opc.Add(DataPara.CreateDataParameter("@PriorityLevel3", DbType.String, Level3));
        opc.Add(DataPara.CreateDataParameter("@Result", DbType.String, Result));
        sdb.TransactionExecuteScalar(sb.ToString(),opc);
    }
    #endregion

    #region Update TB_NPI_APP_Member
    protected void UpdateIsReply(string DocNo, string Flag, string Dept)
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        StringBuilder sb = new StringBuilder();
        sb.Append(@"Update TB_NPI_APP_MEMBER set IsReply = @IsReply where DOC_NO =@DOC_NO and DEPT = @DEPT");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DOC_NO", DbType.String, DocNo));
        opc.Add(DataPara.CreateDataParameter("@DEPT", DbType.String, Dept));
        opc.Add(DataPara.CreateDataParameter("@IsReply", DbType.String, Flag));
        sdb.TransactionExecuteScalar(sb.ToString(), opc);
    }
    #endregion

    #region GET CTQ Issus / FMEA List By DocNo and DEPT
    protected DataTable GetIssuelist(string DocNo, string DEPT)
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        StringBuilder sb = new StringBuilder();
        sb.Append(@"Select * from [TB_NPI_APP_ISSUELIST] where SUB_DOC_NO = @SUB_DOC_NO and DEPT =@DEPT");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@SUB_DOC_NO", DbType.String, DocNo));
        opc.Add(DataPara.CreateDataParameter("@DEPT", DbType.String, DEPT));
        DataTable dt = sdb.TransactionExecute(sb.ToString(), opc);
        return dt;
    }
    protected DataTable GetFMEAlist(string DocNo, string DEPT)
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        StringBuilder sb = new StringBuilder();
        sb.Append(@"Select * from [TB_NPI_FMEA] where SubNo = @SubNo and WriteDept =@WriteDept");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@SubNo", DbType.String, DocNo));
        opc.Add(DataPara.CreateDataParameter("@WriteDept", DbType.String, DEPT));
        DataTable dt = sdb.TransactionExecute(sb.ToString(), opc);
        return dt;
    }

    protected DataTable GetCTQlist(string DocNo, string DEPT)
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        StringBuilder sb = new StringBuilder();
        sb.Append(@"Select * from [TB_NPI_APP_CTQ] where SUB_DOC_NO = @SUB_DOC_NO and DEPT =@DEPT");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@SUB_DOC_NO", DbType.String, DocNo));
        opc.Add(DataPara.CreateDataParameter("@DEPT", DbType.String, DEPT));
        DataTable dt = sdb.TransactionExecute(sb.ToString(), opc);
        return dt;
    }
    #endregion


    #region WriteChecked關卡檢查部門無需Reply時,直接將PASS的結果Inser到TB_NPI_APP_RESULT
    protected void InsertReplyResult(string DocNo, int caseid, string Dept, string APPROVER, string Result, string Option, string Levels)
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
                    INSERT INTO [NPI_REPORT].[dbo].[TB_NPI_APP_RESULT]
                               ([SUB_DOC_NO]
                               ,[CASEID]
                               ,[DEPT]
                               ,[APPROVER]
                               ,[APPROVER_RESULT]
                               ,[APPROVER_OPINION]
                               ,[APPROVER_Levels]
                               ,[APPROVER_DATE]
                               ,[UPDATE_TIME]
                              )
                         VALUES( @SUB_DOC_NO
                                ,@CASEID
                                ,@DEPT
                                ,@APPROVER
                                ,@APPROVER_RESULT
                                ,@APPROVER_OPINION
                                ,@APPROVER_Levels
                                ,@APPROVER_DATE
                                ,@UPDATE_TIME
                                )
                    ");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@SUB_DOC_NO", DbType.String, DocNo));
        opc.Add(DataPara.CreateDataParameter("@CASEID", DbType.Int32, caseid));
        opc.Add(DataPara.CreateDataParameter("@DEPT", DbType.String, Dept));
        opc.Add(DataPara.CreateDataParameter("@APPROVER", DbType.String, APPROVER));
        opc.Add(DataPara.CreateDataParameter("@APPROVER_RESULT", DbType.String, Result));
        opc.Add(DataPara.CreateDataParameter("@APPROVER_OPINION", DbType.String, Option));
        opc.Add(DataPara.CreateDataParameter("@APPROVER_Levels", DbType.String, Levels));
        opc.Add(DataPara.CreateDataParameter("@APPROVER_DATE", DbType.DateTime, DateTime.Now));
        opc.Add(DataPara.CreateDataParameter("@UPDATE_TIME", DbType.DateTime, DateTime.Now));
        sdb.TransactionExecuteScalar(sb.ToString(), opc);
    }
    #endregion

    #region 計算DFX的分數
    protected DataTable GetDFXScore(string DocNO, string Dept)
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        StringBuilder sb = new StringBuilder();
        sb.Append(@"SELECT SUM(CONVERT(int,MaxPoints)) AS MaxPoints,SUM(CONVERT(int,DFXPoints)) as DFXPoints
                    FROM [NPI_REPORT].[dbo].[TB_DFX_ItemBody]
                    where DFXNo = @DFXNo and WriteDept = @WriteDept  and MaxPoints  != 'NA'");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DFXNo",DbType.String, DocNO));
        opc.Add(DataPara.CreateDataParameter("@WriteDept", DbType.String, Dept));
        DataTable dt = sdb.TransactionExecute(sb.ToString(), opc);
        return dt;
    }
    #endregion

    #region 獲取基本資料
    protected DataTable GetMaster(string SUB_DOC_NO)
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        StringBuilder sbM = new StringBuilder();
        sbM.Append(@"select  T1.*,T2.* from TB_NPI_APP_SUB T1 
                    Left join TB_NPI_APP_MAIN T2 ON T1.DOC_NO = T2.DOC_NO
                    where T1.SUB_DOC_NO = @SUB_DOC_NO");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@SUB_DOC_NO",DbType.String, SUB_DOC_NO));
        DataTable dt = sdb.TransactionExecute(sbM.ToString(), opc);
        return dt;
    }    
    #endregion

}
