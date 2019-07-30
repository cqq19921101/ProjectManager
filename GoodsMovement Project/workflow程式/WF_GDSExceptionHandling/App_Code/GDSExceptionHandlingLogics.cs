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
            //Check此I1單(領料單)和關聯的IA,I6單(退料單)是否都已經過賬 (Call SAP BAPI Z_BAPI_GDS_SEND MBLNR不為空則表示已經過賬)
            if (!oStandard.CheckFormNoIsPass())
            {
                try
                {
                    HandleResult.IsSuccess = false;
                    HandleResult.CustomMessage = "I1(領料單)或者IA,I6(退料單)存在未過賬的單子！";

                }
                catch (Exception ex)
                {

                }
            }            

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

                    }
                    catch (Exception ex)
                    {
                        
                    }
                }

                if (ErrMsg.ToString().Length > 0)
                {
                    HandleResult.IsSuccess = false;
                    HandleResult.CustomMessage = ErrMsg.ToString();
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
                Variables.Add(SPMVariableKey.Subject, "[DocNo] [" + FormFields["txtDocNo".ToUpper()] + "]");
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

            SPMAfterSend_DBIO(SPMTaskVars, FormFields, ref HandleResult, RoutingVariable);
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

            try
            {
                //Insert Submit之後數據到DB中
                oStandard.Insert_Begin(Werks, DocNo, RDocNo, CostCenter, Department, Application, ZEILE, Material, int.Parse(ReturnQuantity), Return, Reason, Remark, IADocNo, I6DocNo, "In Process", int.Parse(CASEID));

                //將Submit後的狀態 W(In Process)回傳給SAP

            }
            catch (Exception ex)
            {
                HandleResult.IsSuccess = false;
                HandleResult.CustomMessage = ex.Message;
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

            switch (RoutingVariable.Key)
            {
                //if Reject,Post Back To SAP
                case SPMRoutingVariableKey.spm_Return:
                    PostBackToSAP(SPMTaskVars, FormFields, Variables, "R");
                    break;
                case SPMRoutingVariableKey.spm_Jump:

                    switch (stepName)
                    {
                        case "MQE":
                            break;
                        case "MQE Leader":
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
        //if (iTaskId < 0)
        //{
        //    return;
        //}
        //string caseID = (string)(SPMTaskVars.ReadDatum("CASEID"));
        //NCRMgmt oNCRMgmt = new NCRMgmt(FormFields["txtSite".ToUpper()], FormFields["txtBU".ToUpper()]);
        //NCR_Standard oNCR_Standard = oNCRMgmt.InitialNCRMgmt();

        //DataTable dt = oNCR_Standard.Get_NCR_Main(int.Parse(caseID));

        //if (dt.Rows.Count > 0)
        //{
        //    string status = dt.Rows[0]["STATUS"].ToString();
        //    if (status == "HR")
        //    {
        //        X.Js.Call("Print('" + string.Format("Print.aspx?CASEID={0}&SITE={1}&BU={2}", caseID, FormFields["txtSite".ToUpper()].ToString(), FormFields["txtBU".ToUpper()].ToString() + "')"));

        //    }
        //    else
        //    {
        //        Alert("此單據未簽核完畢，不可列印!");
        //        return;
        //    }
        //}


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
        //Aspose.Cells.License lic = new Aspose.Cells.License();
        //string AsposeLicPath = System.Configuration.ConfigurationSettings.AppSettings["AsposeLicPath"].ToString();
        //lic.SetLicense(AsposeLicPath);

        //string templatePath = HttpContext.Current.Server.MapPath("") + "\\PrintTemplate.xlsx";

        ////Instantiate a new Workbook object.
        //Aspose.Cells.Workbook book = new Aspose.Cells.Workbook(templatePath);
        //Aspose.Cells.Worksheet sheet = book.Worksheets[0];

        ////設定導出格式
        //sheet.PageSetup.IsPercentScale = false;
        //sheet.PageSetup.FitToPagesWide = 1; //自動縮放為一頁寬
        //sheet.PageSetup.LeftMargin = 0.5;
        //sheet.PageSetup.RightMargin = 0.5;
        //sheet.PageSetup.TopMargin = 0.5;
        //sheet.PageSetup.BottomMargin = 0.5;
        //sheet.PageSetup.Orientation = Aspose.Cells.PageOrientationType.Landscape;//水平
        //NCRMgmt oNCRMgmt = new NCRMgmt(site, bu);
        //NCR_Standard oNCR_Standard = oNCRMgmt.InitialNCRMgmt();

        //DataTable dt = oNCR_Standard.Get_NCR_Main(caseId);
        //if (dt.Rows.Count > 0)
        //{
        //    DataRow dr = dt.Rows[0];
        //    //1.填充公司名稱

        //    sheet.Replace("{COMPANY}", oNCR_Standard.GetCompanyName());

        //    //2.填充出差单master 信息
        //    sheet.Replace("{DEPT_NAME}", dr["DEPT_NAME"].ToString());
        //    sheet.Replace("{COST_CENTER}", dr["COST_CENTER"].ToString());
        //    sheet.Replace("{CREATE_DATE}", DateTime.Parse(dr["CREATE_DATE"].ToString()).ToString("yyyy/MM/dd"));
        //    sheet.Replace("{CATEGORY}", dr["CATEGORY"].ToString());
        //    sheet.Replace("{EMPLOYEE_NAME}", dr["EMPLOYEE_NAME"].ToString());
        //    sheet.Replace("{EMPLOYEE_ID}", dr["EMPLOYEE_ID"].ToString());
        //    sheet.Replace("{TITLE}", dr["JOB"].ToString());
        //    sheet.Replace("{TRIP_PERIOD}", DateTime.Parse(dr["BEGIN_DATE"].ToString()).ToString("yyyy/MM/dd") + "~" + DateTime.Parse(dr["END_DATE"].ToString()).ToString("yyyy/MM/dd"));
        //    sheet.Replace("{TRAFFIC}", double.Parse(dr["TRAFFIC_EXPENSE"].ToString()).ToString("n"));
        //    sheet.Replace("{ACCOMMODATION}", double.Parse(dr["ACCOMMODATION_EXPENSE"].ToString()).ToString("n"));
        //    sheet.Replace("{FOOD}", double.Parse(dr["FOOD_EXPENSE"].ToString()).ToString("n"));
        //    sheet.Replace("{OTHER}", double.Parse(dr["OTHER_EXPENSE"].ToString()).ToString("n"));
        //    sheet.Replace("{TOTAL}", double.Parse(dr["TOTAL_EXPENSE"].ToString()).ToString("n"));
        //    sheet.Replace("{CURRENCY}", dr["CURRENCY"].ToString());
        //    sheet.Replace("{PREPAID}", double.Parse(dr["PREPAID_EXPENSE"].ToString()).ToString("n"));
        //    sheet.Replace("{PREPARATION}", dr["PREPARATION"].ToString());
        //    sheet.Replace("{REMARK}", dr["REMARK"].ToString());

        //    sheet.Replace("{APPLICANT}", dr["EMPLOYEE_NAME"].ToString());
        //    sheet.Replace("{DATE1}", DateTime.Parse(dr["CREATE_DATE"].ToString()).ToString("yyyy/MM/dd"));
        //    DataTable dtStepHandler = oNCR_Standard.GetStepAndHandler(caseId);
        //    DataTable dtTask = oNCR_Standard.Get_NCR_Task(caseId);
        //    DataTable dtTraffic = oNCR_Standard.Get_NCR_Traffic(caseId);
        //    //task起始行index
        //    int taskStartRowIndex = 6;
        //    //traffic起始行index
        //    int trfficeStartRowIndex = 6;
        //    Aspose.Cells.Cells cells = sheet.Cells;

        //    //判斷動態數據（task，traffic）行數，取最大
        //    int dynamicDataRowCount = (dtTask.Rows.Count > dtTraffic.Rows.Count) ? dtTask.Rows.Count : dtTraffic.Rows.Count;

        //    //判斷是否超出默認行數

        //    if (dynamicDataRowCount > 3)
        //    {
        //        //超出部份，自動COPY現有模板ROW
        //        cells.InsertRows(9, dynamicDataRowCount - 3);
        //        cells.CopyRows(cells, taskStartRowIndex, 9, dynamicDataRowCount - 3); //複製模板row格式至新行

        //    }
        //    //3.填充出差单工作安排信息
        //    #region


        //    foreach (DataRow drTask in dtTask.Rows)
        //    {
        //        sheet.Cells[taskStartRowIndex, 0].PutValue(DateTime.Parse(drTask["BEGIN_DATE"].ToString()).ToString("MM/dd") + "~" + DateTime.Parse(drTask["END_DATE"].ToString()).ToString("MM/dd"));
        //        sheet.Cells[taskStartRowIndex, 1].PutValue(drTask["LOCATION"].ToString());
        //        sheet.Cells[taskStartRowIndex, 2].PutValue(drTask["BUSINESS_OBJECT"].ToString());
        //        sheet.Cells[taskStartRowIndex, 3].PutValue(drTask["CONTENT"].ToString());

        //        taskStartRowIndex += 1;
        //    }
        //    //4.填充出差单行程安排信息


        //    foreach (DataRow drTraffic in dtTraffic.Rows)
        //    {
        //        sheet.Cells[trfficeStartRowIndex, 4].PutValue(DateTime.Parse(drTraffic["ID_DATE"].ToString()).ToString("MM/dd"));
        //        sheet.Cells[trfficeStartRowIndex, 6].PutValue(drTraffic["DEPART_PLACE"].ToString());
        //        sheet.Cells[trfficeStartRowIndex, 7].PutValue(drTraffic["DESTINATION"].ToString());


        //        trfficeStartRowIndex += 1;
        //    }
        //    #endregion
        //    //5.填充签核人资讯
        //    #region
        //    DataRow[] drTmp = dtStepHandler.Select("STEP_NAME='職務代理人'");
        //    if (drTmp.Length > 0)
        //    {
        //        sheet.Replace("{DEPUTY}", drTmp[0]["CNAME"].ToString());
        //    }
        //    else
        //    {
        //        sheet.Replace("{DEPUTY}", "-");
        //    }

        //    drTmp = dtStepHandler.Select("STEP_NAME='部門主管'");
        //    if (drTmp.Length > 0)
        //    {
        //        sheet.Replace("{DEPT_MANAGER}", drTmp[0]["CNAME"].ToString());
        //    }
        //    else
        //    {
        //        sheet.Replace("{DEPT_MANAGER}", "-");
        //    }

        //    drTmp = dtStepHandler.Select("STEP_NAME='MD/Function Head'");
        //    if (drTmp.Length > 0)
        //    {
        //        sheet.Replace("{MD/Function Head}", drTmp[0]["CNAME"].ToString());
        //    }
        //    else
        //    {
        //        sheet.Replace("{MD/Function Head}", "-");
        //    }

        //    drTmp = dtStepHandler.Select("STEP_NAME='BU HEAD/Operation Head'");
        //    if (drTmp.Length > 0)
        //    {
        //        sheet.Replace("{BU HEAD/Operation Head}", drTmp[0]["CNAME"].ToString());
        //    }
        //    else
        //    {
        //        sheet.Replace("{BU HEAD/Operation Head}", "-");

        //    }

        //    drTmp = dtStepHandler.Select("STEP_NAME='BG HEAD'");
        //    if (drTmp.Length > 0)
        //    {
        //        sheet.Replace("{BG HEAD}", drTmp[0]["CNAME"].ToString());
        //    }
        //    else
        //    {
        //        sheet.Replace("{BG HEAD}", "-");

        //    }

        //    drTmp = dtStepHandler.Select("STEP_NAME='HR主管'");
        //    if (drTmp.Length > 0)
        //    {
        //        sheet.Replace("{HR MANAGER}", drTmp[0]["CNAME"].ToString());

        //    }
        //    else
        //    {
        //        sheet.Replace("{HR MANAGER}", "-");

        //    }
        //    #endregion
        //    //6.填充签核flow
        //    #region
        //    DataTable dtFlow = Borg_Flow.GetFlowRecord(caseId);
        //    int count = dtFlow.Rows.Count;
        //    int flowStartRowIndex = 19 + (((dynamicDataRowCount - 3) > 0) ? (dynamicDataRowCount - 3) : 0);//處理自動增長ROW count

        //    for (int i = 0; i < dtFlow.Rows.Count; i++)
        //    {
        //        DataRow drFlow = dtFlow.Rows[i];
        //        if (i < dtFlow.Rows.Count - 1)
        //        {
        //            cells.CopyRows(cells, flowStartRowIndex + i, flowStartRowIndex + 1 + i, 1); //複製模板row格式至新行
        //        }

        //        cells[flowStartRowIndex + i, 2].PutValue(drFlow["stage"].ToString());
        //        cells[flowStartRowIndex + i, 3].PutValue(drFlow["time stamp"].ToString());

        //        cells[flowStartRowIndex + i, 4].PutValue(drFlow["to"].ToString());
        //        cells[flowStartRowIndex + i, 6].PutValue((drFlow["comments"].ToString() == "Jump.") ? "Approve." : drFlow["comments"].ToString());


        //    }
        //    #endregion
        //    //Save the workbook as a PDF File
        //    string filename = HttpContext.Current.Server.MapPath("~\\PrintTemplate\\") + dr["FORMNO"].ToString() + ".pdf";
        //    book.Save(filename, Aspose.Cells.SaveFormat.Pdf);
        //    path = filename;
        //}
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


}
