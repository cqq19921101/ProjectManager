using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LiteOn.ea.SPM3G;
using LiteOn.ea.SPM3G.UI;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Text;
using System.Xml;
using Ext.Net;
using Liteon.ICM.DataCore;
using System.IO;
using LiteOn.EA.NPIReport.Utility;
using LiteOn.EA.Borg.Utility;
using LiteOn.EA.CommonModel;

public partial class WF_NPIGating_HIS : System.Web.UI.Page
{
    private NPIGating_HISLogics oFlowLogics;
    private NPIGating_HISUIShadow oUIControls;
    private SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("SPM"));

    ArrayList opc = new ArrayList();

    protected void Page_Load(object sender, EventArgs e)
    {
        SpmMaster _Master = (SpmMaster)Master;
        oUIControls = new NPIGating_HISUIShadow(this);
        oFlowLogics = new NPIGating_HISLogics(this, oUIControls);
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
             lblLogonId.Value = _Master.IFormURLPara.LoginId.Replace(" ", "").ToLower();
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

            _Master.ApplicantControlSetting = lUC;


            Model_BorgUserInfo oModel_BorgUserInfo = new Model_BorgUserInfo();
            Borg_User oBorg_User = new Borg_User();
            oModel_BorgUserInfo = oBorg_User.GetUserInfoByLogonId(_Master.IFormURLPara.LoginId);
            if (oModel_BorgUserInfo._EXISTS)
            {
                lblSite.Text = Borg_Tools.GetSiteInfo();
                lblBu.Text = oModel_BorgUserInfo._BU;
                lblLogonId.Text = oModel_BorgUserInfo._LOGON_ID;
            }
            lblStepName.Text = _Master.IFormURLPara.StepName;
       
        }
    }


    protected void btnConfirm_Click(object sender, DirectEventArgs e)
    {
        SpmMaster _Master = (SpmMaster)Master;
        if (string.IsNullOrEmpty(cmbAttachmentType.SelectedItem.Text))
        {
            Alert("請先選擇上傳的文件類型!");
            return;
        }
        else
        {
            string MFILE_PATH = string.Empty;
            string MFILE_NAME = string.Empty;
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            string File_Type = cmbAttachmentType.SelectedItem.Text.ToString().Trim();
            if (string.IsNullOrEmpty(File_Type))
            {
                Alert("請選擇附件類別");
                return;
            }
            if (!fileMeeting.HasFile)
            {
                Alert("請選擇上傳附件!");
                return;
            }

            int indexMeeting = fileMeeting.FileName.LastIndexOf('.');
            string extMeeting = fileMeeting.FileName.Substring(indexMeeting + 1);
            string fileName = fileMeeting.FileName.Substring(fileMeeting.FileName.LastIndexOf("\\") + 1);
            if (extMeeting != "pdf")
            {
                Alert("附件類型只能為pdf類型!");
                return;
            }


            //創建保存的目錄 根據主單號與子單號
            string type = cmbAttachmentType.SelectedItem.Text.Trim();
            string CaseID = _Master.IFormURLPara.CaseId.ToString();
            string docNoPath = Server.MapPath("~/Attachment/" + CaseID + "/" + type);
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

                MFILE_PATH = "Attachment/" + CaseID + "/" + type + "/" + fileName;
                MFILE_NAME = fileName;

                fileMeeting.PostedFile.SaveAs(docNoPath + "/" + fileName);

                opc.Clear();
                opc.Add(DataPara.CreateDataParameter("@DOC_NO", DbType.String, txtFormNo.Text, ParameterDirection.Input, 30));
                opc.Add(DataPara.CreateDataParameter("@CASE_ID", DbType.Int32,_Master.IFormURLPara.CaseId, ParameterDirection.Input,8));

                opc.Add(DataPara.CreateDataParameter("@MFILE_PATH",DbType.String,MFILE_PATH,ParameterDirection.Input,255));
                opc.Add(DataPara.CreateDataParameter("@MFILE_TYPE",DbType.String,File_Type, ParameterDirection.Input,30 ));
                opc.Add(DataPara.CreateDataParameter("@MFILE_NAME",DbType.String,MFILE_NAME, ParameterDirection.Input,50));
                opc.Add(DataPara.CreateDataParameter("@UPDATE_TIME",DbType.DateTime, DateTime.Now, ParameterDirection.Input,8));
                opc.Add(DataPara.CreateDataParameter("@UPDATE_USERID",DbType.String,lblLogonId.Text, ParameterDirection.Input,20));
                opc.Add(DataPara.CreateDataParameter("@Result",DbType.String, null, ParameterDirection.Output,1000));
                //SP執行結果返回固定格式
                //1 OK; 
                //2 NG;ERR MSG
                string tmp = sdb.ExecuteProcScalar("[P_Upload_NPI_Files]", opc, "@Result");
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
                }

            }
            catch (Exception ex)
            {
                DeleteExistFiles(docNoPath);
                Alert("文件上傳失敗!DB ERROR:" + ex.Message);
            }

            #endregion
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

    private void BindAttachmentList(string CaseId)
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        string sql = "select * from TB_NPI_APP_ATTACHFILE where CASEID=@CaseID";
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@CaseID",DbType.String, CaseId));
        DataTable dtAttachfile = sdb.TransactionExecute(sql, opc);
        grdAttachment.Store.Primary.DataSource = dtAttachfile;
        grdAttachment.Store.Primary.DataBind();

    }


    protected void btnDelete_Click(object sender, DirectEventArgs e)
    {
        SpmMaster _Master = (SpmMaster)Master;
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        string subdoc = string.Empty;
        string filename = string.Empty;
        string filetype = string.Empty;
        string CaseID = _Master.IFormURLPara.CaseId.ToString();
        RowSelectionModel sm = grdAttachment.SelectionModel.Primary as RowSelectionModel;
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
            filetype = row["FILE_TYPE"].ToString();
            try
            {
                string sql = "DELETE FROM TB_NPI_APP_ATTACHFILE WHERE SUB_DOC_NO=@subdoc"
                     + " and FILE_TYPE=@filetype and FILE_NAME=@fileName";
                opc.Clear();
                opc.Add(DataPara.CreateDataParameter("@subdoc",DbType.String, subdoc));
                opc.Add(DataPara.CreateDataParameter("@filetype",DbType.String, filetype));
                opc.Add(DataPara.CreateDataParameter("@fileName",DbType.String, filename));
                sdb.TransactionExecuteNonQuery(sql, opc);

                string sub_docNoPath = Server.MapPath("~/Attachment/" + CaseID + "/" + filetype + "/" + filename);
                bool IsSubDocNoExist = Directory.Exists(sub_docNoPath);
                if (IsSubDocNoExist)
                {
                    DeleteExistFiles(sub_docNoPath);
                }
            }
            catch (Exception ex)
            {
                ErrMsg.Append(string.Format("類別:{0},文件名稱:{1} 刪除作業失敗!ErrMsg:{2}<BR/>", filetype, filename, ex.Message));

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
         SpmMaster _Master = (SpmMaster)Master;
         string CaseID = _Master.IFormURLPara.CaseId.ToString();
         StringBuilder sbType = new StringBuilder();
         int TypeCount = cmbAttachmentType.Items.Count;

         return "abort";


    }

    protected void AfterEdit(object sender, DirectEventArgs e)
    {
        string CaseId = e.ExtraParams["CASEID"];
        string handler = e.ExtraParams["HANDLER"];
        string approver_time = e.ExtraParams["APPROVE_TIME"];
        string approve_result = e.ExtraParams["APPROVE_RESULT"];
        string approve_remark = e.ExtraParams["APPROVE_REMARK"];
        string step_name = e.ExtraParams["STEP_NAME"];
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));


        ////數據庫DB更新操作
        string sql = " UPDATE TB_NPI_Step_Handler set APPROVE_RESULT=@approve_result, "
                  + " APPROVE_REMARK=@approve_remark,SIGN_FLAG='Y',APPROVE_TIME=@approve_time"
                   + " WHERE CASEID=@caseid AND STEP_NAME=@step_name and HANDLER=@handler";
        ArrayList opc = new ArrayList();
        opc.Add(DataPara.CreateDataParameter("@approve_time",DbType.DateTime,DateTime.Now));
        opc.Add(DataPara.CreateDataParameter("@approve_result",DbType.String,approve_result));
        opc.Add(DataPara.CreateDataParameter("@approve_remark",DbType.String,approve_remark));
        opc.Add(DataPara.CreateDataParameter("@handler", DbType.String,handler));
        opc.Add(DataPara.CreateDataParameter("@step_name", DbType.String,step_name));
        opc.Add(DataPara.CreateDataParameter("@caseid",DbType.String,CaseId));
        
        try
        {
            sdb.TransactionExecute(sql, opc);

        }
        catch (Exception ex)
        {
            Alert(ex.Message);

        }

    }


}