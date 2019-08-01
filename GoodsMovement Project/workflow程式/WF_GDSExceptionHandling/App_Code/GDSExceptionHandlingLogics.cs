using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using LiteOn.ea.SPM3G;
using LiteOn.ea.SPM3G.UI;
using System.IO;
using Ext.Net;
using LiteOn.EA.Borg.Utility;
using LiteOn.EA.CommonModel;
using LiteOn.EA.CE.Utility;
using LiteOn.EA.DAL;
using LiteOn.EA.Model;
using LiteOn.EA.BLL;
using LiteOn.GDS.Utility;
using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;
// UI controls holder for Example1 
public class GDSExceptionHandlingUIShadow : IUIShadow
{
    // Declare controls which show in the web page

    #region Begin
    public Panel vMain;
    public Panel pnlMain;
    public Panel frmUserInfo;
    public Panel frmApplyInfo;

    public TextField txtLogonID;//申請人ENAME
    public TextField txtName;//申請人姓名
    public TextField txtPlant;//申請人廠別
    public TextField txtDept;//申請人部門 
    public TextField txtEMail;//申請人郵件
    public TextField txtExtNO;//申請人 分機

    public TextField txtRDocNo;//Link 單號 
    public Button btnLink;//Link 按鈕
    public TextField txtDocNo;//生成單號
    public TextField txtCostCenter;//成本中心
    public TextField txtDepartment;//部門
    public TextField txtApplication;//申請人
    public ComboBox txtMaterial;//料號
    public TextField txtReturnQuantity;//退料數量
    public ComboBox txtReason;//退料原因
    public TextArea txtRemark;//備註
    public TextField txtReturn;//條件FLAG Flag為Y才符合條件
    public TextField txtIADocNo;//關聯IA單
    public TextField txtI6DocNo;//關聯I6單
    public TextField txtZEILE;//Itme 001
    public TextField txtAmount;//Itme 001

    //SOURCE DATA
    public TextField txtHead;//表頭XML數據
    public TextField txtDOA;//DOA XML數據
    public TextField txtDetail;//表身 XML數據
    public TextField txtWERKS;//WERKS
    public TextField txtAPTYP;//APTYP

    #endregion


    public GDSExceptionHandlingUIShadow(Page oContainer)
        : base(oContainer) { }

    // Initialize controls
    public override void InitShadow(System.Web.UI.WebControls.ContentPlaceHolder oContentPage)
    {
        #region  Begin
        vMain = (Panel)oContentPage.FindControl("vMain");
        pnlMain = (Panel)oContentPage.FindControl("pnlMain");
        frmUserInfo = (Panel)oContentPage.FindControl("frmUserInfo");
        frmApplyInfo = (Panel)oContentPage.FindControl("frmApplyInfo");

        //基本資料
        txtLogonID = (TextField)oContentPage.FindControl("txtLogonID");
        txtName = (TextField)oContentPage.FindControl("txtName");
        txtPlant = (TextField)oContentPage.FindControl("txtPlant");
        txtDept = (TextField)oContentPage.FindControl("txtDept");
        txtEMail = (TextField)oContentPage.FindControl("txtEMail");
        txtExtNO = (TextField)oContentPage.FindControl("txtExtNO");

        //單據資料
        txtRDocNo = (TextField)oContentPage.FindControl("txtRDocNo");
        btnLink = (Button)oContentPage.FindControl("btnLink");
        txtDocNo = (TextField)oContentPage.FindControl("txtDocNo");
        txtCostCenter = (TextField)oContentPage.FindControl("txtCostCenter");
        txtDepartment = (TextField)oContentPage.FindControl("txtDepartment");
        txtApplication = (TextField)oContentPage.FindControl("txtApplication");
        txtMaterial = (ComboBox)oContentPage.FindControl("txtMaterial");
        txtReturnQuantity = (TextField)oContentPage.FindControl("txtReturnQuantity");
        txtReason = (ComboBox)oContentPage.FindControl("txtReason");
        txtRemark = (TextArea)oContentPage.FindControl("txtRemark");
        txtReturn = (TextField)oContentPage.FindControl("txtReturn");
        txtIADocNo = (TextField)oContentPage.FindControl("txtIADocNo");
        txtI6DocNo = (TextField)oContentPage.FindControl("txtI6DocNo");
        txtZEILE = (TextField)oContentPage.FindControl("txtZEILE");
        txtAmount = (TextField)oContentPage.FindControl("txtAmount");

        //SOURCE DATA
        txtHead = (TextField)oContentPage.FindControl("txtHead");
        txtDOA = (TextField)oContentPage.FindControl("txtDOA");
        txtDetail = (TextField)oContentPage.FindControl("txtDetail");
        txtWERKS = (TextField)oContentPage.FindControl("txtWERKS");
        txtAPTYP = (TextField)oContentPage.FindControl("txtAPTYP");
        #endregion
    }
}

// Form logics
public class GDSExceptionHandlingLogics : ISPMInterfaceContent
{
    private Model_DOAHandler DOAHandler = new Model_DOAHandler();
    private Page oPage;
    private GDSExceptionHandlingUIShadow oUIControls;
    private string sql = string.Empty;
    private IFormURLPara oPara;
    GDS_Helper oStandard = new GDS_Helper();

    public GDSExceptionHandlingLogics(object oContainer, IUIShadow UIShadow)
        : base(oContainer)
    {
        this.SetUIShadow(UIShadow);
    }

    // Code for Page_Load
    protected override void PageLoad(object oContainer, IFormURLPara para, IUIShadow UIShadow)
    {
        oPage = (Page)oContainer;
        oUIControls = (GDSExceptionHandlingUIShadow)UIShadow;
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
                    InitialControl_UserInfo();
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

    private void InitialControl_UserInfo()
    {
        #region 申請人基本資料加載
        SPMBasic SPMBasic_class = new SPMBasic();
        Model_SPMUserInfo SPMUserInfo = new Model_SPMUserInfo();
        SPMBasic_class.GetUserInfoByEName(oPara.LoginId, SPMUserInfo);
        if (SPMUserInfo.Exists == true)
        {
            oUIControls.txtLogonID.Text = SPMUserInfo.logonID;
            oUIControls.txtName.Text = SPMUserInfo.userName;
            oUIControls.txtPlant.Text = SPMUserInfo.bu;
            oUIControls.txtDept.Text = SPMUserInfo.deptName;
            oUIControls.txtEMail.Text = SPMUserInfo.email;
            oUIControls.txtExtNO.Text = SPMUserInfo.tel_office;
        }


        #endregion
    }


    private void BindGrid(GridPanel grd, DataTable dt)
    {
        grd.Store.Primary.DataSource = dt;
        grd.Store.Primary.DataBind();
    }


    #endregion

    // Code for 'draft' and 'pending for process'
    public override void InitialContainer(SPMTaskVariables SPMTaskVars, EFFormFields FormFields, ref object oContainer, IUIShadow UIShadow)
    {
        GDSExceptionHandlingUIShadow lUIControls = (GDSExceptionHandlingUIShadow)UIShadow;

        int caseID = int.Parse((string)(SPMTaskVars.ReadDatum("CASEID")));
        string stepName = SPMTaskVars.ReadDatum("STEPNAME").ToString();
        ShowRecord(lUIControls, FormFields, "Process", caseID, SPMTaskVars);
        base.InitialContainer(SPMTaskVars, FormFields, ref oContainer, UIShadow);
    }

    private void ShowRecord(GDSExceptionHandlingUIShadow lUIControls, EFFormFields FormFields, string type, int caseId, SPMTaskVariables SPMTaskVars)
    {
        string caseid = SPMTaskVars.ReadDatum("CASEID").ToString();
        string stepName = (string)(SPMTaskVars.ReadDatum("STEPNAME"));
        GDS_Helper oStandard = new GDS_Helper();
        if (stepName != "Begin")
        {
            #region 抓取 異常單資料 並綁定到控件中
            DataTable dt = oStandard.GetMaster_Exception(int.Parse(caseid));//加載 異常單主表資料
            if (dt.Rows.Count > 0)
            {
                #region 數據加載
                DataRow dr = dt.Rows[0];
                oUIControls.txtDocNo.Text = dr["MBLNR"].ToString();//主單號 
                oUIControls.txtRDocNo.Text = dr["MBLNR_A"].ToString();//Link 單號 
                oUIControls.txtCostCenter.Text = dr["KOSTL"].ToString();//成本中心
                oUIControls.txtDepartment.Text = dr["ABTEI"].ToString();//部門
                oUIControls.txtApplication.Text = dr["Applicant"].ToString();//LoginID
                oUIControls.txtReturn.Text = dr["RTNIF"].ToString();// Return Flag
                oUIControls.txtWERKS.Text = dr["WERKS"].ToString();//廠別
                oUIControls.txtMaterial.Text = dr["MATNR"].ToString();//料號
                oUIControls.txtZEILE.Text = dr["ZEILE"].ToString();//料號對應的ITEM
                oUIControls.txtAmount.Text = dr["Amount"].ToString();//金額
                oUIControls.txtIADocNo.Text = dr["IADocNo"].ToString();//關聯IA單
                oUIControls.txtI6DocNo.Text = dr["I6DocNo"].ToString();//關聯I6單
                oUIControls.txtReason.Text = dr["Reason"].ToString();//REASON
                oUIControls.txtRemark.Text = dr["Remark"].ToString();//REMARK
                oUIControls.txtReturnQuantity.Text = dr["MENGE"].ToString();//RETURN 數量
                oUIControls.txtDOA.Text = dr["Settingxml"].ToString();//Settingxml
                #endregion

                #region 控件顯示
                oUIControls.btnLink.Hidden = true;
                oUIControls.frmUserInfo.Hidden = true;
                oUIControls.txtDocNo.ReadOnly = true;
                oUIControls.txtRDocNo.ReadOnly = true;
                oUIControls.txtCostCenter.ReadOnly = true;
                oUIControls.txtDepartment.ReadOnly = true;
                oUIControls.txtApplication.ReadOnly = true;
                oUIControls.txtReturn.ReadOnly = true;
                oUIControls.txtMaterial.ReadOnly = true;
                oUIControls.txtZEILE.ReadOnly = true;
                oUIControls.txtAmount.ReadOnly = true;
                oUIControls.txtIADocNo.ReadOnly = true;
                oUIControls.txtI6DocNo.ReadOnly = true;
                oUIControls.txtReason.ReadOnly = true;
                oUIControls.txtRemark.ReadOnly = true;
                oUIControls.txtReturnQuantity.ReadOnly = true;
                #endregion
            }
            #endregion


            #region 通過異常單的Link單號 抓取正常單中的Head和Detail
            DataTable dtHead = oStandard.GetdtHead(oUIControls.txtRDocNo.Text.Trim());
            DataTable dtDetail = oStandard.GetdtDetail(oUIControls.txtRDocNo.Text.Trim());
            oUIControls.txtWERKS.Text = dtHead.Rows[0]["WERKS"].ToString();//廠別
            oUIControls.txtAPTYP.Text = dtHead.Rows[0]["APTYP"].ToString();//單據類型
            string xmlstrHead = oStandard.DataTableToXMLStr(dtHead);
            oUIControls.txtHead.Text = xmlstrHead;//Head
            string xmlstrDetail = oStandard.DataTableToXMLStr(dtDetail);
            oUIControls.txtDetail.Text = xmlstrDetail;//Detail
            //string tempWEKS = DOA.GetXMLConfigName(dtHead);//CALL FUNCTION獲取XML配置檔名
            //SettingParser x = new SettingParser(tempWEKS, oUIControls.txtAPTYP.Text);//讀取XML配置檔信息
            //oUIControls.txtDOA.Text = x.ApproveXML.Replace("&lt1;", "<").Replace("&gt1;", ">"); ;//抓取DOA的簽核邏輯
            #endregion

        }


    }

    // Code for 'Notice' and 'Log'. Disable all contols.
    public override void InitialDisableContainer(SPMTaskVariables SPMTaskVars, EFFormFields FormFields, ref object oContainer, IUIShadow UIShadow)
    {
        int caseID = int.Parse((string)(SPMTaskVars.ReadDatum("CASEID")));
        GDSExceptionHandlingUIShadow lUIControls = (GDSExceptionHandlingUIShadow)UIShadow;
        ShowRecord(lUIControls, FormFields, "Log", caseID, SPMTaskVars);
        base.InitialDisableContainer(SPMTaskVars, FormFields, ref oContainer, UIShadow);
    }

    // Validate contols before submit
    public override bool EFFormFieldsValidation(SPMSubmitMethod SubmitMethod, SPMProcessMethod ProcessMethod, SPMTaskVariables SPMTaskVars, ref IInterfaceHandleResult HandleResult, object oContainer, IUIShadow UIShadow)
    {
        GDSExceptionHandlingUIShadow lUIControls = (GDSExceptionHandlingUIShadow)UIShadow;
        GDS_Helper oStandard = new GDS_Helper();
        StringBuilder ErrMsg = new StringBuilder();


        string stepName = (string)(SPMTaskVars.ReadDatum("STEPNAME"));
        string curDOA = lUIControls.txtDOA.Text;
        string plant = lUIControls.txtWERKS.Text;
        string apType = lUIControls.txtAPTYP.Text;

        DOA spmDOA = new DOA();
        string sApplicant = lUIControls.txtApplication.Text.Trim();
        //獲取 表頭及表身DATA
        string formDetail = lUIControls.txtDetail.Text.Trim().Replace("&lt1;", "<").Replace("&gt1;", ">");
        string formHead = lUIControls.txtHead.Text.Trim().Replace("&lt1;", "<").Replace("&gt1;", ">");
        DataTable dtHead = LiteOn.GDS.Utility.Tools.BuildHeadTable();
        System.IO.StringReader reader = new System.IO.StringReader(formHead);
        dtHead.ReadXml(reader);
        System.IO.StringReader reader2 = new System.IO.StringReader(formDetail);
        DataTable dtDetail = LiteOn.GDS.Utility.Tools.BuildDetailTable();
        dtDetail.ReadXml(reader2);
        if (SubmitMethod == SPMSubmitMethod.CreateNewCase)
        {
            #region 欄位的非空驗證

            #endregion

            #region Check單據是否已經過帳
            string I1DocNo = oUIControls.txtRDocNo.Text;
            string IADocNo = oUIControls.txtIADocNo.Text;
            string I6DocNo = oUIControls.txtI6DocNo.Text;
            string WERKS = oUIControls.txtWERKS.Text;
            //Check此I1單(領料單)和關聯的IA,I6單(退料單)是否都已經過賬 (Call SAP BAPI Z_BAPI_GDS_SEND MBLNR不為空則表示已經過賬)
            if (!oStandard.CheckFormNoIsPass(I1DocNo, IADocNo, I6DocNo, WERKS))
            {
                HandleResult.IsSuccess = false;
                HandleResult.CustomMessage = "I1(領料單)或者IA,I6(退料單)存在未過賬的單子！";

            }
            #endregion

        }
        else
        {
            //only  approve action check controls 
            if (SubmitMethod == SPMSubmitMethod.HandleCase_Approve)
            {
                int caseId = int.Parse((string)(SPMTaskVars.ReadDatum("CASEID")));
                string curStepId = SPMAppLine.GetCurrentStep(curDOA);
                //調用FUNCTION作FormFieldsValidation
                string errMsg = DOA.FieldsValidationByStepId(plant, apType, curStepId, caseId);
                if (errMsg.Length > 0)
                {
                    HandleResult.IsSuccess = false;
                    HandleResult.CustomMessage = errMsg;
                }
                else
                {
                    try
                    {
                        //CALL FUNCTION獲取下關簽核人 
                        DOAHandler = spmDOA.GetStepHandler(sApplicant, curDOA, dtHead, dtDetail, false);
                        oStandard.UpdateSettingxml(oUIControls.txtDocNo.Text, DOAHandler._sDOA.ToString());

                    }
                    catch (Exception ex)
                    {
                        HandleResult.IsSuccess = false;
                        HandleResult.CustomMessage = ex.Message;
                    }
                }

                if (ErrMsg.ToString().Length > 0)
                {
                    HandleResult.IsSuccess = false;
                    HandleResult.CustomMessage = ErrMsg.ToString();
                }

                //簽核人防呆檢查
                if (DOAHandler._sEndFlag == "N" && DOAHandler._sHandler.Length == 0)
                {
                    HandleResult.IsSuccess = false;
                    HandleResult.CustomMessage = "Can't find next step handler, pls contact sys administrator";
                }
                string sCurLogonID = (string)(SPMTaskVars.ReadDatum("SYS_LOGONID"));
                string sCurRoleCode = SPMAppLine.GetCurrentApprover(curDOA).Replace("{", "").Replace("}", "");

                //簽核人重復簽核防呆
                if (DOAHandler._sEndFlag == "N" && DOAHandler._sRoleCode == sCurRoleCode && DOAHandler._sHandler.ToUpper() == sCurLogonID.ToUpper())
                {
                    HandleResult.IsSuccess = false;
                    HandleResult.CustomMessage = "Server or network is busy now, pls try again";
                }

            }
        }
        return base.EFFormFieldsValidation(SubmitMethod, ProcessMethod, SPMTaskVars, ref HandleResult, oContainer, UIShadow);
    }

    // Fill SPM's EFFormFieldData
    public override void PrepareEFFormFields(SPMSubmitMethod SubmitMethod, SPMProcessMethod ProcessMethod, SPMTaskVariables TaskVars, ref EFFormFields FormFields, ref IInterfaceHandleResult HandleResult, object oContainer, IUIShadow UIShadow, ref string ApplicantInfo)
    {
        GDSExceptionHandlingUIShadow lUIControls = (GDSExceptionHandlingUIShadow)UIShadow;
        //基本資料
        FormFields.SetOrAdd("txtLogonID".ToUpper(), lUIControls.txtLogonID.Text);
        FormFields.SetOrAdd("txtName".ToUpper(), lUIControls.txtName.Text);
        FormFields.SetOrAdd("txtPlant".ToUpper(), lUIControls.txtPlant.Text);
        FormFields.SetOrAdd("txtDept".ToUpper(), lUIControls.txtDept.Text);
        FormFields.SetOrAdd("txtEMail".ToUpper(), lUIControls.txtEMail.Text);
        FormFields.SetOrAdd("txtExtNO".ToUpper(), lUIControls.txtExtNO.Text);

        FormFields.SetOrAdd("txtRDocNo".ToUpper(), lUIControls.txtRDocNo.Text);
        FormFields.SetOrAdd("txtDocNo".ToUpper(), lUIControls.txtDocNo.Text);
        FormFields.SetOrAdd("txtCostCenter".ToUpper(), lUIControls.txtCostCenter.Text);
        FormFields.SetOrAdd("txtDepartment".ToUpper(), lUIControls.txtDepartment.Text);
        FormFields.SetOrAdd("txtApplication".ToUpper(), lUIControls.txtApplication.Text);
        FormFields.SetOrAdd("txtMaterial".ToUpper(), lUIControls.txtMaterial.Text);
        FormFields.SetOrAdd("txtReturnQuantity".ToUpper(), lUIControls.txtReturnQuantity.Text);
        FormFields.SetOrAdd("txtReason".ToUpper(), lUIControls.txtReason.Text);
        FormFields.SetOrAdd("txtRemark".ToUpper(), lUIControls.txtRemark.Text);
        FormFields.SetOrAdd("txtReturn".ToUpper(), lUIControls.txtReturn.Text);
        FormFields.SetOrAdd("txtIADocNo".ToUpper(), lUIControls.txtIADocNo.Text);
        FormFields.SetOrAdd("txtI6DocNo".ToUpper(), lUIControls.txtI6DocNo.Text);
        FormFields.SetOrAdd("txtZEILE".ToUpper(), lUIControls.txtZEILE.Text);
        FormFields.SetOrAdd("txtAmount".ToUpper(), lUIControls.txtAmount.Text);


        //SOURCE DATA
        FormFields.SetOrAdd("txtHead".ToUpper(), lUIControls.txtHead.Text);
        FormFields.SetOrAdd("txtDetail".ToUpper(), lUIControls.txtDetail.Text);
        FormFields.SetOrAdd("txtDOA".ToUpper(), lUIControls.txtDOA.Text);
        FormFields.SetOrAdd("txtWERKS".ToUpper(), lUIControls.txtWERKS.Text);
        FormFields.SetOrAdd("txtAPTYP".ToUpper(), lUIControls.txtAPTYP.Text);


        base.PrepareEFFormFields(SubmitMethod, ProcessMethod, TaskVars, ref FormFields, ref HandleResult, oContainer, UIShadow, ref ApplicantInfo);
    }

    // Fill SPM Variable 
    public override void PrepareSPMVariables(SPMSubmitMethod SubmitMethod, SPMProcessMethod ProcessMethod, SPMTaskVariables SPMTaskVars, ref SPMVariables Variables, ref SPMRoutingVariable RoutingVariable, ref string strSPMUid, string strMemo, string strNotesForNextApprover, EFFormFields FormFields, ref IInterfaceHandleResult HandleResult, ref string SuccessMessage)
    {

        if (SubmitMethod == SPMSubmitMethod.CreateNewCase)
        {

            GDS_Helper oStandard = new GDS_Helper();
            StringBuilder ErrMsg = new StringBuilder();


            //string stepName = (string)(SPMTaskVars.ReadDatum("STEPNAME"));
            string curDOA = oUIControls.txtDOA.Text;
            string plant = oUIControls.txtWERKS.Text;
            string apType = oUIControls.txtAPTYP.Text;


            DOA spmDOA = new DOA();
            string sApplicant = oUIControls.txtApplication.Text.Trim();
            //獲取 表頭及表身DATA
            string formDetail = oUIControls.txtDetail.Text.Trim().Replace("&lt1;", "<").Replace("&gt1;", ">");
            string formHead = oUIControls.txtHead.Text.Trim().Replace("&lt1;", "<").Replace("&gt1;", ">");
            DataTable dtHead = LiteOn.GDS.Utility.Tools.BuildHeadTable();
            System.IO.StringReader reader = new System.IO.StringReader(formHead);
            dtHead.ReadXml(reader);
            System.IO.StringReader reader2 = new System.IO.StringReader(formDetail);
            DataTable dtDetail = LiteOn.GDS.Utility.Tools.BuildDetailTable();
            dtDetail.ReadXml(reader2);
            try
            {
                //抓取第一關簽核人
                DOAHandler = spmDOA.GetStepHandler(sApplicant, curDOA, dtHead, dtDetail, true);
                Variables.Add(SPMVariableKey.Subject, "[部門領料_應退未退---測試] [" + FormFields["txtDocNo".ToUpper()] + "]");
                RoutingVariable = new SPMRoutingVariable(SPMRoutingVariableKey.spm_Jump, "DOA(" + DOAHandler._sHandler + ")");
            }
            catch (Exception ex)
            {
                HandleResult.IsSuccess = false;
                HandleResult.CustomMessage = ex.Message;
            }
        }
        else
        {
            try
            {
                if (DOAHandler._sEndFlag == "N")
                {
                    //簽核人防呆檢查
                    if (DOAHandler._sHandler.Length == 0)
                    {
                        HandleResult.IsSuccess = false;
                        HandleResult.CustomMessage = "Operation fail(server or network is busy now), pls refresh this page and try again";
                    }
                    else
                    {
                        RoutingVariable = new SPMRoutingVariable(SPMRoutingVariableKey.spm_Jump, "DOA(" + DOAHandler._sHandler + ")");
                        // 20121105 Add QX NA
                        if (DOAHandler._cc.Length > 0)
                            Variables.Add(SPMVariableKey.NextCC, DOAHandler._cc);
                        // 20121105 End QX NA
                    }
                }
                else
                {
                    RoutingVariable = new SPMRoutingVariable(SPMRoutingVariableKey.spm_Jump, "End");
                }

                // DBIO.RecordTraceLog("D", "OK", DOAHandler);
            }
            catch (Exception)
            {
                DBIO.RecordTraceLog("D", "NG", DOAHandler);
                throw;
            }
        }

        base.PrepareSPMVariables(SubmitMethod, ProcessMethod, SPMTaskVars, ref Variables, ref RoutingVariable, ref strSPMUid, strMemo, strNotesForNextApprover, FormFields, ref HandleResult, ref SuccessMessage);
    }

    // Code for 'before send'
    public override void SPMBeforeSend(SPMSubmitMethod SubmitMethod, SPMTaskVariables SPMTaskVars, SPMVariables Variables, SPMRoutingVariable RoutingVariable, ref EFFormFields FormFields, ref IInterfaceHandleResult HandleResult)
    {

        base.SPMBeforeSend(SubmitMethod, SPMTaskVars, Variables, RoutingVariable, ref FormFields, ref HandleResult);
    }

    // Code for 'after send'
    public override void SPMAfterSend(SPMSubmitMethod SubmitMethod, SPMTaskVariables SPMTaskVars, SPMVariables Variables, SPMRoutingVariable RoutingVariable, EFFormFields FormFields, ref IInterfaceHandleResult HandleResult)
    {

        if (SubmitMethod != SPMSubmitMethod.CreateNewCase)
        {

            SPMAfterSend_DBIO(SPMTaskVars, FormFields, ref HandleResult, RoutingVariable, Variables);
        }
        else
        {
            GDS_Helper oStandard = new GDS_Helper();
            string CASEID = System.Web.HttpUtility.UrlDecode(Variables[SPMVariableKey.CaseId]);//CASEID
            string Werks = oUIControls.txtWERKS.Text;
            string DocNo = oUIControls.txtDocNo.Text;//主單號
            string RDocNo = oUIControls.txtRDocNo.Text;//Link 單號 
            string CostCenter = oUIControls.txtCostCenter.Text;//成本中心
            string Department = oUIControls.txtDepartment.Text;//部門
            string Application = oUIControls.txtApplication.Text;//申請人
            string ZEILE = oUIControls.txtZEILE.Text;//ITEM
            string Material = oUIControls.txtMaterial.Text;//料號
            string ReturnQuantity = oUIControls.txtReturnQuantity.Text;//退料數量
            string Reason = oUIControls.txtReason.Text;//退料原因
            string Remark = oUIControls.txtRemark.Text;//備註
            string Return = oUIControls.txtReturn.Text;//條件FLAG Flag為Y才符合條件
            string IADocNo = oUIControls.txtIADocNo.Text;//關聯IA單
            string I6DocNo = oUIControls.txtI6DocNo.Text;//關聯I6單
            string Amount = oUIControls.txtAmount.Text;//金額
            string Settingxml = oUIControls.txtDOA.Text;//Setting
            try
            {
                //Insert Submit之後數據到DB中
                oStandard.Insert_Begin(Werks, DocNo, RDocNo, CostCenter, Department, Application, ZEILE, Material, ReturnQuantity, Return, Reason, Remark, IADocNo, I6DocNo, "In Process", double.Parse(Amount), Settingxml, int.Parse(CASEID));

                //將Submit後的狀態 W(In Process)回傳給SAP Begin關卡直接回傳
                oStandard.PostBackToSAP(int.Parse(CASEID));

            }
            catch (Exception ex)
            {
                //有異常 刪除已經Insert的資料
                oStandard.DeleteFormNo(int.Parse(CASEID));
                HandleResult.IsSuccess = false;
                HandleResult.CustomMessage = ex.Message;
            }


        }
        base.SPMAfterSend(SubmitMethod, SPMTaskVars, Variables, RoutingVariable, FormFields, ref HandleResult);
    }

    private void SPMAfterSend_DBIO(SPMTaskVariables SPMTaskVars, EFFormFields FormFields, ref IInterfaceHandleResult HandleResult, SPMRoutingVariable RoutingVariable, SPMVariables Variables)
    {

        try
        {
            string stepName = (string)SPMTaskVars.ReadDatum("STEPNAME");
            int caseID = int.Parse((string)(SPMTaskVars.ReadDatum("CASEID")));


            switch (RoutingVariable.Key)
            {
                //if Reject,Post Back To SAP
                case SPMRoutingVariableKey.spm_Return:
                    try
                    {
                        PostBackToSAP(SPMTaskVars, FormFields, Variables, "R");
                        oStandard.UpdateFormStatus(caseID, "Reject");

                    }
                    catch (Exception ex)
                    {
                        HandleResult.IsSuccess = false;
                        HandleResult.CustomMessage = ex.Message;
                    }
                    break;
                case SPMRoutingVariableKey.spm_Jump:
                    string curDOA = FormFields["txtDOA".ToUpper()].Replace("&lt1;", "<").Replace("&gt1;", ">");
                    string curStep = SPMAppLine.GetCurrentStep(curDOA);
                    //最後一關結束 將結果拋到DB中 並更新STATUS欄位為 Approve
                    if (curStep == "*")
                    {
                        try
                        {
                            //  IF APPROVE ,POST BACK TO SAP and Update Form Status Approve
                            PostBackToSAP(SPMTaskVars, FormFields, Variables, "A");
                            oStandard.UpdateFormStatus(caseID, "Approve");

                        }
                        catch (Exception ex)
                        {
                            HandleResult.IsSuccess = false;
                            HandleResult.CustomMessage = ex.Message;
                        }
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
        base.Print(iTaskId, SPMTaskVars, FormFields, oContainer, UIShadow);
    }

    public override void SPM_SendError(SPMSubmitMethod SubmitMethod, SPMTaskVariables SPMTaskVars, SPMVariables Variables, SPMRoutingVariable RoutingVariable, EFFormFields FormFields, IInterfaceHandleResult HandleResult)
    {
        string strExceptionFrom = HandleResult.ExceptionFrom;
        string strErrorMessage = HandleResult.CustomMessage;
        base.SPM_SendError(SubmitMethod, SPMTaskVars, Variables, RoutingVariable, FormFields, HandleResult);
    }

    private void Alert(string msg)
    {
        X.Msg.Alert("Alert", msg).Show();
    }

    /// <summary>
    /// 寫本機LOG
    /// </summary>
    /// <param name="msg">LOG信息</param>
    /// <param name="errflag">TRUE:異常：FALSE:正常</param>
    static private void WriteLog(bool errFlag, string msg)
    {

        if (errFlag)
        {
            msg = "[ERROR] " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss ") + msg;
        }
        else
        {
            msg = "[     ] " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss ") + msg;
        }



        string errLogPath = "D:\\TripLog\\";


        string logFile = errLogPath + DateTime.Today.ToString("yyyyMMdd") + ".txt";
        //路徑不存在則建立
        if (!Directory.Exists(errLogPath))
        {
            Directory.CreateDirectory(errLogPath);
        }
        //檢查文件存在
        if (!File.Exists(logFile))
        {
            //文件不存在則建立
            StreamWriter sw = File.CreateText(logFile);
            try
            {
                sw.WriteLine(msg);
            }
            catch (Exception)
            {
            }
            finally
            {
                sw.Flush();
                sw.Close();
            }
        }
        else
        {
            //文件存在則複寫
            StreamWriter sw = File.AppendText(logFile);
            try
            {
                sw.WriteLine(msg);
            }
            catch (Exception)
            {
            }
            finally
            {
                sw.Flush();
                sw.Close();
            }
        }

    }

    private string PrintForm(int caseId, string site, string bu)
    {
        string path = string.Empty;
        return path;
    }

    private void SendMail(string mailTo, string body, string path, string formno)
    {
        Borg_Mail oBorg_Mail = new Borg_Mail();
        ArrayList to = new ArrayList();
        ArrayList cc = new ArrayList();
        string testMode = System.Configuration.ConfigurationSettings.AppSettings["TestMode"].ToString();
        if (testMode == "Y")
        {
            to.Add(System.Configuration.ConfigurationSettings.AppSettings["TestMailReceiver"].ToString());
        }
        else
        {
            to.Add(mailTo);
        }
        string subject = string.Format("NCR Report[{0}] 核准通知", formno);
        oBorg_Mail.SendMail_Attachment(to, cc, subject, body, false, path);
    }


    /// <summary>
    /// 單據狀態咨詢回寫SAP
    /// </summary>
    /// <param name="SPMTaskVars"></param>
    /// <param name="FormFields"></param>
    /// <param name="handlerType"></param>
    private void PostBackToSAP(SPMTaskVariables SPMTaskVars, EFFormFields FormFields, SPMVariables Variables, string handlerType)
    {

        GDS_Helper oStandard = new GDS_Helper();
        string sCurLogonID = (string)(SPMTaskVars.ReadDatum("SYS_LOGONID"));
        int CASEID = int.Parse((string)(SPMTaskVars.ReadDatum("CASEID")));
        string StatusMark = string.Empty;
        string APROV = string.Empty;
        switch (handlerType)
        {
            case "R":
                string sMemo = System.Web.HttpUtility.UrlDecode(Variables[SPMVariableKey.Opinion]);
                StatusMark = "Reject by:" + sCurLogonID + "(" + sMemo + ")";
                APROV = "R";
                break;
            case "A":
                StatusMark = "Approve by:" + sCurLogonID;
                APROV = "A";
                break;
            default:
                StatusMark = "Error";
                break;
        }
        //退簽原因長度防呆
        if (StatusMark.Length > 255) StatusMark = StatusMark.Substring(0, 255);
        //簡繁轉換
        StatusMark = ConvertChinese(StatusMark, "Big5");
        try
        {
            //By 主單號 UPDATE Flag and StatusRemark
            oStandard.UpdateGDSQueue(CASEID, StatusMark, APROV);
        }
        catch (Exception)
        {
            //trace 
            //DBIO.RecordTraceLog("C", "NG", DOAHandler);
            //throw;
        }
    }

    /// <summary>
    /// USER COMMENTS 簡繁體字轉換
    /// </summary>
    /// <param name="SourceString"></param>
    /// <param name="Language"></param>
    /// <returns></returns>
    public static string ConvertChinese(string SourceString, string Language)
    {
        string newString = string.Empty;
        switch (Language)
        {
            case "Big5":
                newString = ChineseConverter.Convert(SourceString, ChineseConversionDirection.SimplifiedToTraditional);
                break;
            case "GB2312":
                newString = ChineseConverter.Convert(SourceString, ChineseConversionDirection.TraditionalToSimplified);
                break;
            default:
                newString = SourceString;
                break;
        }
        return newString;
    }


}
