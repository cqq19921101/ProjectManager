using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Ext.Net;
using System.Text;
using Liteon.ICM.DataCore;
using LiteOn.EA.BLL;
using LiteOn.EA.CommonModel;
using LiteOn.EA.Borg.Utility;
using LiteOn.EA.NPIReport.Utility;
using OfficeOpenXml;
using System.IO;
public partial class NPI_NPI_PilotRunNoticeForm_Start : System.Web.UI.Page
{
    SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
    string sql = string.Empty;
    ArrayList opc = new ArrayList();
    int function_id = 1289;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserName"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        else
        {
            lblLogonId.Text = Session["UserName"].ToString();
        }
        if (!X.IsAjaxRequest)
        {
            UserRole UserRole_class = new UserRole();
            if (UserRole_class.checkRole(lblLogonId.Text, function_id) == false)
            {
                ExtAspNet.Alert.Show("对不起，你没有权限操作!");
                return;
            }
            Model_BorgUserInfo oModel_BorgUserInfo = new Model_BorgUserInfo();
            Borg_User oBorg_User = new Borg_User();
            oModel_BorgUserInfo = oBorg_User.GetUserInfoByLogonId(lblLogonId.Text);
            if (oModel_BorgUserInfo._EXISTS)
            {
                lblSite.Text = Borg_Tools.GetSiteInfo();
                lblBu.Text = oModel_BorgUserInfo._BU;
                lblBuilding.Text = oModel_BorgUserInfo._Building;
            }
            SPMBasic basic = new SPMBasic();
            txtAPPLY_DATE.Text = DateTime.Now.ToString("yyyy/MM/dd");
            txtDOC_NO.Text = basic.GetSPMFormNO("PRNF");
            BindDept();
            BindTeamCategory();
            DataTable dt = BindParamer("AttachmentPath");
            
        }

    }

    protected void ChangeDept(object sender, DirectEventArgs e)
    {
        sbName.Text = string.Empty;
        sbReply.Text = string.Empty;
        sbChecked.Text = string.Empty;
       
        string sql = "SELECT ENAME,CNAME,CATEGORY FROM TB_NPI_MEMBER "
                   + " WHERE DEPT=@Dept"
                   + " AND BU=@BU AND BUILDING=@Building ";

        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@Dept", DbType.String, cobDept.SelectedItem.Text));
        opc.Add(DataPara.CreateDataParameter("@BU", DbType.String,lblBu.Text));
        opc.Add(DataPara.CreateDataParameter("@Building", DbType.String,lblBuilding.Text));
        DataTable dt = sdb.TransactionExecute(sql, opc);
        DataTable dtWindow = new DataTable();
        DataTable dtManger = new DataTable();
        dtWindow = dt.Clone();
        dtManger = dt.Clone();
        DataRow[] rows = dt.Select(" CATEGORY='WINDOW' ");
        DataRow[] Managerrows = dt.Select(" CATEGORY='MANAGER' "); 
        foreach (DataRow row in rows)  
        {
            dtWindow.Rows.Add(row.ItemArray);
        }
        foreach (DataRow row in Managerrows)
        {
            dtManger.Rows.Add(row.ItemArray);
        }
        if (dtManger.Rows.Count == 0 || dtWindow.Rows.Count == 0)
        {
            Alert("部門成員未維護請確認！");
            btnAdd.Disabled = true;
            return;
        }
        else
        {
            btnAdd.Disabled = false;
            BindCombox(sbName, dtWindow);
            BindCombox(sbReply, dtWindow);
            BindCombox(sbChecked, dtManger);
        }

    }
   
   
    protected void btnSave_click(object sender, DirectEventArgs e)
    {
        string errorMsg = string.Empty;
        DataTable data = null;
        int emptyQuantity = 0;
        string apply_Date = txtAPPLY_DATE.Text;
        string APPLY_USERID = lblLogonId.Text;
        string doc_No = txtDOC_NO.Text;
        string prod_Group = sbProd_group.SelectedItem.Text;
        string model_Name = txtMODEL_NAME.Text;
      
        string customer = txtCUSTOMER.Text;
      
        string npi_pm = txtNPI_PM.Text;
        string sales_owner = txtSALES_OWNER.Text;
        string me_engineer = txtME_ENGINEER.Text;
        string ee_engineer = txtEE_ENGINEER.Text;
        string cad_engineer = txtCAD_ENGINEER.Text;
        string tp_pm = txtTP_PM.Text;
        string Product_Des_Remark = txtPRODUCT_DES_REMARK.Text;
        string sales_Area = txtSALES_AREA.Text;
        string sales_Area_Remark = txtSALES_AREA_REMARK.Text;
        string mp_Date =  txtMP_DATE.Text;
        string mp_Date_Remark =  txtMP_DATE_REMARK.Text;
        string project_Code = txtPROJECT_CODE.SelectedDate.ToString("yyyy/MM/dd");
        string project_Code_Remark = txtPROJECT_CODE_REMARK.Text;
        string time_quantity = txtTIME_QUANTITY.SelectedDate.ToString("yyyy/MM/dd");
        string time_quantity_Remark =  txtTIME_QUANTITY_REMARK.Text;
        string others = txtOTHERS.Text;
        string others_Remark = txtOTHERS_REMARK.Text;
        string PRPhaseTime = txtPRPhaseTime.SelectedDate.ToString("yyyy/MM/dd");
        string PRPhaseRemark = txtPRphase_Remark.Text;
        string productDesc = txtPRODUCT_DES.Text;
        DataTable dt = CheckFormNo(model_Name);
        #region 數據驗證


        if (string.IsNullOrEmpty(prod_Group))
        {
            errorMsg += "產品類別,";
        }

        if (string.IsNullOrEmpty(project_Code))
        {
            errorMsg += "EVT 階段時間,";
        }
        if (string.IsNullOrEmpty(time_quantity))
        {
            errorMsg += "DVT 階段時間,";
        }
        if (string.IsNullOrEmpty(PRPhaseTime))
        {
            errorMsg += "PR 階段時間,";
        }
        if (string.IsNullOrEmpty(npi_pm))
        {
            errorMsg += "NPI PM,";
        }
        if (errorMsg.Length > 0)
        {
            Alert(errorMsg.Substring(0, errorMsg.Length - 1) + "不能為空");
            return;
        }
        #endregion
        if (dt.Rows.Count > 0)
        {
            Alert("此機種的試產通知單已經存在,請直接進入workflow系統起單！");
            return;
        }
        else
        {
            #region 數據操作
            string sql = @"INSERT INTO TB_NPI_APP_MAIN
            (
            DOC_NO
           ,BU
           ,BUILDING
           ,PROD_GROUP
           ,APPLY_DATE
           ,APPLY_USERID
           ,MODEL_NAME
           ,CUSTOMER
           ,PRODUCT_DES
           ,SALES_AREA
           ,MP_DATE
           ,PROJECT_CODE
           ,TIME_QUANTITY
           ,OTHERS
           ,PRODUCT_DES_REMARK
           ,SALES_AREA_REMARK
           ,MP_DATE_REMARK
           ,PROJECT_CODE_REMARK
           ,TIME_QUANTITY_REMARK
           ,OTHERS_REMARK
           ,UPDATE_TIME
           ,UPDATE_USERID
           ,NPI_PM
           ,SALES_OWNER
           ,ME_ENGINEER
           ,EE_ENGINEER
           ,CAD_ENGINEER
           ,TP_PM
           ,PRPhaseTime
           ,PRPhaseTime_Remark
          )
     VALUES
           (
             @DOC_NO
           ,@BU
           ,@BUILDING
           ,@PROD_GROUP
           ,@APPLY_DATE
           ,@APPLY_USERID
           ,@MODEL_NAME
           ,@CUSTOMER
           ,@PRODUCT_DES
           ,@SALES_AREA
           ,@MP_DATE
           ,@PROJECT_CODE
           ,@TIME_QUANTITY
           ,@OTHERS
           ,@PRODUCT_DES_REMARK
           ,@SALES_AREA_REMARK
           ,@MP_DATE_REMARK
           ,@PROJECT_CODE_REMARK
           ,@TIME_QUANTITY_REMARK
           ,@OTHERS_REMARK
           ,@UPDATE_TIME
           ,@UPDATE_USERID
           ,@NPI_PM
           ,@SALES_OWNER
           ,@ME_ENGINEER
           ,@EE_ENGINEER
           ,@CAD_ENGINEER
           ,@TP_PM
           ,@PRPhaseTime
           ,@PRPhaseTime_Remark
           )";
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@DOC_NO", DbType.String, doc_No));
            opc.Add(DataPara.CreateDataParameter("@BU", DbType.String, lblBu.Text.Trim()));
            opc.Add(DataPara.CreateDataParameter("@BUILDING", DbType.String, lblBuilding.Text.Trim()));
            opc.Add(DataPara.CreateDataParameter("@PROD_GROUP", DbType.String, prod_Group));
            opc.Add(DataPara.CreateDataParameter("@APPLY_DATE", DbType.DateTime, Convert.ToDateTime(apply_Date)));
            opc.Add(DataPara.CreateDataParameter("@APPLY_USERID", DbType.String, APPLY_USERID));
            opc.Add(DataPara.CreateDataParameter("@MODEL_NAME", DbType.String, model_Name));
            opc.Add(DataPara.CreateDataParameter("@CUSTOMER", DbType.String, customer));
            opc.Add(DataPara.CreateDataParameter("@PRODUCT_DES", DbType.String, productDesc));
            opc.Add(DataPara.CreateDataParameter("@SALES_AREA", DbType.String, sales_Area));
            opc.Add(DataPara.CreateDataParameter("@MP_DATE", DbType.String, mp_Date));
            opc.Add(DataPara.CreateDataParameter("@PROJECT_CODE", DbType.String, project_Code));
            opc.Add(DataPara.CreateDataParameter("@TIME_QUANTITY", DbType.String, time_quantity));
            opc.Add(DataPara.CreateDataParameter("@OTHERS", DbType.String, others));
            opc.Add(DataPara.CreateDataParameter("@PRODUCT_DES_REMARK", DbType.String, Product_Des_Remark));
            opc.Add(DataPara.CreateDataParameter("@SALES_AREA_REMARK", DbType.String, sales_Area_Remark));
            opc.Add(DataPara.CreateDataParameter("@MP_DATE_REMARK", DbType.String, mp_Date_Remark));
            opc.Add(DataPara.CreateDataParameter("@PROJECT_CODE_REMARK", DbType.String, project_Code_Remark));
            opc.Add(DataPara.CreateDataParameter("@TIME_QUANTITY_REMARK", DbType.String, time_quantity_Remark));
            opc.Add(DataPara.CreateDataParameter("@OTHERS_REMARK", DbType.String, others_Remark));
            opc.Add(DataPara.CreateDataParameter("@UPDATE_TIME", DbType.DateTime, DateTime.Now));
            opc.Add(DataPara.CreateDataParameter("@UPDATE_USERID", DbType.String, lblLogonId.Text));
            opc.Add(DataPara.CreateDataParameter("@NPI_PM", DbType.String, npi_pm));
            opc.Add(DataPara.CreateDataParameter("@SALES_OWNER", DbType.String, sales_owner));
            opc.Add(DataPara.CreateDataParameter("@ME_ENGINEER", DbType.String, me_engineer));
            opc.Add(DataPara.CreateDataParameter("@EE_ENGINEER", DbType.String, ee_engineer));
            opc.Add(DataPara.CreateDataParameter("@CAD_ENGINEER", DbType.String, cad_engineer));
            opc.Add(DataPara.CreateDataParameter("@TP_PM", DbType.String, tp_pm));
            opc.Add(DataPara.CreateDataParameter("@PRPhaseTime", DbType.String, PRPhaseTime));
            opc.Add(DataPara.CreateDataParameter("@PRPhaseTime_Remark", DbType.String, PRPhaseRemark));

            try
            {
                sdb.TransactionExecuteNonQuery(sql, opc);
                Alert("新產品試產通知單開立成功！");
                this.btnSave.Disabled = true;
                this.btnContinue.Disabled = false;

            }

            catch (Exception ex)
            {
                Alert(ex + "新產品試產通知單開立失敗！");
            }
            #endregion
        }




    }

    
    protected void btnContinue_click(object sender, DirectEventArgs e)
    {
        SPMBasic basic = new SPMBasic();
        txtAPPLY_DATE.Text = DateTime.Now.ToString("yyyy/MM/dd");
        txtDOC_NO.Text = basic.GetSPMFormNO("PRNF");
      
        sbProd_group.Text = "";
        txtMODEL_NAME.Text = "";
       
        txtCUSTOMER.Text = "";
        txtSALES_OWNER.Text = "";
        txtPRODUCT_DES.Text = "";
        txtPRODUCT_DES_REMARK.Text = "";
        txtSALES_AREA.Text = "";
        txtSALES_AREA_REMARK.Text = "";
        txtMP_DATE.Text = "";
        txtMP_DATE_REMARK.Text = "";
        txtPROJECT_CODE.Text = "";
        txtPROJECT_CODE_REMARK.Text = "";
        txtTIME_QUANTITY.Text = "";
        txtTIME_QUANTITY_REMARK.Text = "";
        txtOTHERS.Text = "";
        txtOTHERS_REMARK.Text = "";
        this.btnContinue.Disabled = true;
        this.btnSave.Disabled = false;
        txtTP_PM.Text = "";
        txtME_ENGINEER.Text = "";
        txtEE_ENGINEER.Text = "";
        txtNPI_PM.Text = "";

    }

    private void BindGrid(DataTable dt, GridPanel grdInfo)
    {
        grdInfo.Store.Primary.DataSource = dt;
        grdInfo.Store.Primary.DataBind();
    }

    protected void BindDept()
    {
        DataTable dt=BindParamer("SubscribeDept");
        BindCombox(cobDept, dt);
    }

    protected void BindTeamCategory()
    {
      
        DataTable dt=BindParamer("SubscribeList");
        BindCombox(cmbType, dt);

    }

    protected void btnAdd_Click(object sender, DirectEventArgs e)
    {
        string Catgory = cmbType.SelectedItem.Text;
        string Dept = cobDept.SelectedItem.Text;
        string WriteEname = sbName.SelectedItem.Value;
        string ReplyEname = sbReply.SelectedItem.Value;
        string CheckedEname = sbChecked.SelectedItem.Value;
        #region  [ Valid Fields]

        StringBuilder ErrMsg = new StringBuilder();
        if (string.IsNullOrEmpty(Catgory))
        {
            ErrMsg.Append("請選擇團隊類別</br>");
        }
        if (string.IsNullOrEmpty(Dept))
        {
            ErrMsg.Append("請選擇部門</br>");
        }
        if (string.IsNullOrEmpty(WriteEname))
        {
            ErrMsg.Append("請選擇填寫人員</br>");
        } 
        if (string.IsNullOrEmpty(ReplyEname))
        {
            ErrMsg.Append("請選擇回覆人員</br>");
        }
        if (ErrMsg.ToString().Length > 0)
        {
            Alert(ErrMsg.ToString());
        }
        #endregion

        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        Model_NPI_APP_MEMBER oModel_NPI_APP_Member = new Model_NPI_APP_MEMBER();
        oModel_NPI_APP_Member._UPDATE_USERID = lblLogonId.Text.Trim();
        oModel_NPI_APP_Member._UPDATE_TIME = DateTime.Today;
        oModel_NPI_APP_Member._Category = Catgory;
        oModel_NPI_APP_Member._DEPT = Dept;
        oModel_NPI_APP_Member._WriteEname = WriteEname;
        oModel_NPI_APP_Member._ReplyEName = ReplyEname;
        oModel_NPI_APP_Member._DOC_NO = txtDOC_NO.Text;
        oModel_NPI_APP_Member._CheckedEname = CheckedEname;
        
        SPMBasic oSpmBasic = new SPMBasic();
        oModel_NPI_APP_Member._ReplyEmai = oSpmBasic.GetEMailByEName(ReplyEname);
        oModel_NPI_APP_Member._WriteEmail = oSpmBasic.GetEMailByEName(WriteEname);
        oModel_NPI_APP_Member._CheckedEmail = oSpmBasic.GetEMailByEName(CheckedEname);
             
        try
        {

            Dictionary<string, object> result = oStandard.RecordOperation_APPMemeber(oModel_NPI_APP_Member, Status_Operation.ADD);
            if ((bool)result["Result"])
            {

                Alert("新增團隊成員成功!");
               
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
        BindMember(txtDOC_NO.Text, string.Empty);
      
    }

    protected void btnDelete_Click(object sender, DirectEventArgs e)
    {
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        RowSelectionModel sm = this.grdInfo.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count <= 0)
        {
            Alert("请选择须删除项!");
            return;
        }
        string json = e.ExtraParams["Values"];
        Dictionary<string, string>[] companies = JSON.Deserialize<Dictionary<string, string>[]>(json);
        string id = string.Empty;
        string dept = string.Empty;
        string enName = string.Empty;
        string errMsg = string.Empty;
        foreach (Dictionary<string, string> row in companies)
        {
            id =row["ID"];
            dept = row["DEPT"];
            try
            {

                DelTeamMember(id);
            }
            catch (Exception ex)
            {
                errMsg += string.Format("刪除部門{0}錯誤:{1}<br/>", dept, ex.Message);
            }
        }
        if (errMsg.Length > 0)
        {
            Alert(errMsg);
        }
        else
        {
            Alert("刪除成功!");
           
        }
        BindMember(txtDOC_NO.Text, string.Empty);
    }

    protected void BindMember(string DocNo,string Category)
    {
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        DataTable dt = oStandard.GetAppMember(DocNo,Category);
        BindGrid(dt,grdInfo);
    }

    private void DelTeamMember(string  id)
    {
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        Model_NPI_APP_MEMBER oModel_NPI_APP_Member = new Model_NPI_APP_MEMBER();
        oModel_NPI_APP_Member._ID =int.Parse(id);
     
        try
        {
            Dictionary<string, object> result = oStandard.RecordOperation_APPMemeber(oModel_NPI_APP_Member, Status_Operation.DELETE);
   
            if ((bool)result["Result"])
            {

                Alert("刪除成功!");
               
            }
            else
            {
                Alert((string)result["ErrMsg"].ToString());
            }
        }
        catch (Exception ex)
        {
            Alert(ex.Message);
        }
    }

    protected DataTable BindParamer(string ParameName)
    {
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        Model_APPLICATION_PARAM oModel_Param = new Model_APPLICATION_PARAM();
        oModel_Param._APPLICATION_NAME = "NPI_REPORT";
        oModel_Param._FUNCTION_NAME = "Configuration";
        oModel_Param._PARAME_NAME =ParameName;
        oModel_Param._PARAME_ITEM = lblBu.Text.Trim();
        oModel_Param._PARAME_VALUE1 = lblBuilding.Text.Trim();
        DataTable dt = oStandard.PARAME_GetBasicData_Filter(oModel_Param);
        return dt;
    }

    #region BatchOperation
    protected void RgBatch_Changed(object sender, DirectEventArgs e)
    {

        if (rdBatchY.Checked)
        {
            pnlApp.Hidden = true;
            ContainerBatch.Hidden = false;
        }
        else
        {
            pnlApp.Hidden = false;
            ContainerBatch.Hidden = true;
        }
    }

    protected void btnConfirm_Click(object sender, DirectEventArgs e)
    {

        StringBuilder errMsg = new StringBuilder();
        StringBuilder errMsg1 = new StringBuilder();
        StringBuilder errMsg2 = new StringBuilder();
        string file = FileAttachment.FileName;
        string Result = string.Empty;
        int total_num = 0;
        int ok_num = 0;
        int ng_num = 0;
        bool isok = true;
        if (sbProd_group.Text.Length == 0)
        {
            Alert("請選擇產品類別！");
            return;
        }
        if (!FileAttachment.HasFile)
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
                Stream stream = FileAttachment.PostedFile.InputStream;
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
                        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
                        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
                        Model_NPI_APP_MEMBER oModel_NPI_APP_Member = new Model_NPI_APP_MEMBER();
                        oModel_NPI_APP_Member._UPDATE_USERID = lblLogonId.Text.Trim();
                        oModel_NPI_APP_Member._UPDATE_TIME = DateTime.Today;
                        oModel_NPI_APP_Member._DOC_NO = txtDOC_NO.Text;



                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string dept = dt.Rows[i]["Dept"].ToString();
                            string Category = dt.Rows[i]["Category"].ToString();
                            string WriteName = dt.Rows[i]["WriteEname"].ToString();
                            string ReplyEName = dt.Rows[i]["ReplyEName"].ToString();
                            string CheckedEName = dt.Rows[i]["CheckedEName"].ToString();

                            DataTable dtW = CheckUserInfo(WriteName);
                            DataTable dtR = CheckUserInfo(ReplyEName);
                            DataTable dtC = CheckUserInfo(CheckedEName);

                            #region 檢查  Write Reply Check人員必須都為空或者都不為空
                            if (Category == "DFX TeamMember" || Category == "CTQ TeamMember" || Category == "ISSUES TeamMember" || Category == "PFMEA TeamMember")
                            {
                                #region
                                if (WriteName.Length == 0 && (ReplyEName.Length > 0 && CheckedEName.Length > 0))
                                {
                                    errMsg.Append("第" + (i + 2) + "行");
                                    errMsg.Append("簽核人員必須都為空後者都不為空</br>");
                                }
                                if (WriteName.Length == 0 && (ReplyEName.Length == 0 && CheckedEName.Length > 0))
                                {
                                    errMsg.Append("第" + (i + 2) + "行");
                                    errMsg.Append("簽核人員必須都為空後者都不為空</br>");
                                }
                                if (WriteName.Length == 0 && (ReplyEName.Length > 0 && CheckedEName.Length == 0))
                                {
                                    errMsg.Append("第" + (i + 2) + "行");
                                    errMsg.Append("簽核人員必須都為空後者都不為空</br>");
                                }

                                if (ReplyEName.Length == 0 && (WriteName.Length > 0 && CheckedEName.Length > 0))
                                {
                                    errMsg.Append("第" + (i + 2) + "行");
                                    errMsg.Append("簽核人員必須都為空後者都不為空</br>");
                                }
                                if (ReplyEName.Length == 0 && (WriteName.Length == 0 && CheckedEName.Length > 0))
                                {
                                    errMsg.Append("第" + (i + 2) + "行");
                                    errMsg.Append("簽核人員必須都為空後者都不為空</br>");
                                }
                                if (ReplyEName.Length == 0 && (WriteName.Length > 0 && CheckedEName.Length == 0))
                                {
                                    errMsg.Append("第" + (i + 2) + "行");
                                    errMsg.Append("簽核人員必須都為空後者都不為空</br>");
                                }


                                if (CheckedEName.Length == 0 && (ReplyEName.Length > 0 && WriteName.Length > 0))
                                {
                                    errMsg.Append("第" + (i + 2) + "行");
                                    errMsg.Append("簽核人員必須都為空後者都不為空</br>");
                                }
                                if (CheckedEName.Length == 0 && (ReplyEName.Length == 0 && WriteName.Length > 0))
                                {
                                    errMsg.Append("第" + (i + 2) + "行");
                                    errMsg.Append("簽核人員必須都為空後者都不為空</br>");
                                }
                                if (CheckedEName.Length == 0 && (ReplyEName.Length > 0 && WriteName.Length == 0))
                                {
                                    errMsg.Append("第" + (i + 2) + "行");
                                    errMsg.Append("簽核人員必須都為空後者都不為空</br>");
                                }


                                #endregion
                            }
                            #endregion

                            #region 檢查關卡名的準確性
                            if (!IsStepName(Category))
                            {
                                isok = false;
                                errMsg.Append("第" + (i + 2) + "行");
                                errMsg.Append("關卡名填寫錯誤!</br>");
                            }
                            #endregion

                            #region 檢查成員名準確性  不為空時 拼寫是否正確
                            if (Category == "DFX TeamMember" || Category == "CTQ TeamMember" || Category == "ISSUES TeamMember" || Category == "PFMEA TeamMember")
                            {
                                if (dtW.Rows.Count == 0 && WriteName.Length > 0)
                                {
                                    isok = false;
                                    errMsg.Append("第" + (i + 2) + "行Write人員不存在</br>");
                                }
                                if (dtR.Rows.Count == 0 && ReplyEName.Length > 0)
                                {
                                    isok = false;
                                    errMsg.Append("第" + (i + 2) + "行Reply人員不存在</br>");
                                }
                                if (dtC.Rows.Count == 0 && CheckedEName.Length > 0)
                                {
                                    isok = false;
                                    errMsg.Append("第" + (i + 2) + "行Check人員不存在</br>");
                                }
                            }
                            else
                            {
                                if (WriteName.Length > 0)
                                {
                                    if (dtW.Rows.Count == 0)
                                    {
                                        isok = false;
                                        errMsg.Append("第" + (i + 2) + "行Write人員不存在</br>");
                                    }

                                }
                                if (ReplyEName.Length > 0)
                                {
                                    if (dtR.Rows.Count == 0)
                                    {
                                        isok = false;
                                        errMsg.Append("第" + (i + 2) + "行Reply人員不存在</br>");
                                    }

                                }
                                if (CheckedEName.Length > 0)
                                {
                                    if (dtC.Rows.Count == 0)
                                    {
                                        isok = false;
                                        errMsg.Append("第" + (i + 2) + "行Check人員不存在</br>");
                                    }

                                }

                            }
                            #endregion

                        }

                        #region 數據處理
                        foreach (DataRow dr in dt.Rows)
                        {

                            total_num = dt.Rows.Count;
                            try
                            {
                                oModel_NPI_APP_Member._Category = dr["Category"].ToString();
                                oModel_NPI_APP_Member._DEPT = dr["Dept"].ToString(); ;
                                oModel_NPI_APP_Member._WriteEname = dr["WriteEname"].ToString();
                                oModel_NPI_APP_Member._ReplyEName = dr["ReplyEname"].ToString();
                                oModel_NPI_APP_Member._CheckedEname = dr["CheckedEname"].ToString();

                                SPMBasic oSpmBasic = new SPMBasic();
                                oModel_NPI_APP_Member._ReplyEmai = oSpmBasic.GetEMailByEName(dr["ReplyEname"].ToString());
                                oModel_NPI_APP_Member._WriteEmail = oSpmBasic.GetEMailByEName(dr["WriteEname"].ToString());
                                oModel_NPI_APP_Member._CheckedEmail = oSpmBasic.GetEMailByEName(dr["CheckedEname"].ToString());

                                if (isok)
                                {
                                    Dictionary<string, object> result = oStandard.RecordOperation_APPMemeber(oModel_NPI_APP_Member, Status_Operation.ADD);

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
                        #endregion

                        if (errMsg.Length == 0)
                        {

                            string DocNo = txtDOC_NO.Text;
                            string ProductType = sbProd_group.Text;
                            string DFXCategory = "DFX TeamMember";
                            string CTQCategory = "CTQ TeamMember";
                            DataTable dtDFXCount = CompareDeptCountDFX(DocNo, ProductType, DFXCategory);
                            DataTable dtCTQCount = CompareDeptCountCTQ(DocNo, ProductType, CTQCategory);
                            string DFXDept = dtDFXCount.Rows[0]["Dept"].ToString();
                            string CTQDept = dtCTQCount.Rows[0]["Dept"].ToString();

                            #region 先判斷DFX CTQ的部門數量是否一致
                            if (DFXDept.Length > 0)
                            {
                                errMsg2.Append("DFX " + DFXDept + "部門未維護</br>");
                            }
                            if (CTQDept.Length> 0)
                            {
                                errMsg2.Append("CTQ " + CTQDept + "部門未維護</br>");
                            }
                            if (errMsg2.Length > 0)
                            {
                                DeleteExist(txtDOC_NO.Text);//有異常則刪除已上傳的資料
                                BindMember(txtDOC_NO.Text, string.Empty);
                                Alert(string.Format("錯誤信息:<BR/>{0}", errMsg2.ToString()));
                                return;
                            }

                            #endregion

                            #region 根據已經Insert的數據進行第二次Check判斷 該部門某個階段都為空 其餘階段也必須為空
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string WriteEname = dt.Rows[i]["WriteEname"].ToString();
                                string ReplyEName = dt.Rows[i]["ReplyEName"].ToString();
                                string CheckedEName = dt.Rows[i]["CheckedEName"].ToString();
                                string WriteDept = dt.Rows[i]["Dept"].ToString();
                                //DataTable dtCheckCount = CheckDFXDept(DocNo, ProductType, Category);//當前團隊
                                if (WriteEname.Length == 0 && ReplyEName.Length == 0 && CheckedEName.Length == 0)
                                {
                                    string DFXType = dt.Rows[i]["Category"].ToString();

                                    #region DFX 對應部門的簽核人
                                    DataTable dtDFX = GetMember(txtDOC_NO.Text, "DFX TeamMember", WriteDept);
                                    string WriteDFX = dtDFX.Rows[0]["WriteEname"].ToString();
                                    string ReplyDFX = dtDFX.Rows[0]["ReplyEName"].ToString();
                                    string CheckedDFX = dtDFX.Rows[0]["CheckedEName"].ToString();
                                    #endregion

                                    #region CTQ 對應部門簽核人
                                    DataTable dtCTQ = GetMember(txtDOC_NO.Text, "CTQ TeamMember", WriteDept);
                                    string WriteCTQ = dtCTQ.Rows[0]["WriteEname"].ToString();
                                    string ReplyCTQ = dtCTQ.Rows[0]["ReplyEName"].ToString();
                                    string CheckedCTQ = dtCTQ.Rows[0]["CheckedEName"].ToString();
                                    #endregion

                                    #region Issue 對應部門簽核人
                                    DataTable dtIssue = GetMember(txtDOC_NO.Text, "ISSUES TeamMember", WriteDept);
                                    string WriteIssue = dtIssue.Rows[0]["WriteEname"].ToString();
                                    string ReplyIssue = dtIssue.Rows[0]["ReplyEName"].ToString();
                                    string CheckedIssue = dtIssue.Rows[0]["CheckedEName"].ToString();
                                    #endregion

                                    #region FMEA 對應部門簽核人
                                    DataTable dtFMEA = GetMember(txtDOC_NO.Text, "PFMEA TeamMember", WriteDept);
                                    string WriteFMEA = dtFMEA.Rows[0]["WriteEname"].ToString();
                                    string ReplyFMEA = dtFMEA.Rows[0]["ReplyEName"].ToString();
                                    string CheckedFMEA = dtFMEA.Rows[0]["CheckedEName"].ToString();
                                    #endregion

                                    #region Check Part 1
                                    if (DFXType == "DFX TeamMember")
                                    {
                                        if (WriteCTQ.Length > 0 || ReplyCTQ.Length > 0 || CheckedCTQ.Length > 0)
                                        {
                                            errMsg1.Append("DFX " + WriteDept + "部門人員為空時, CTQ也必須都為空</br>");
                                        }
                                        if (WriteIssue.Length > 0 || ReplyIssue.Length > 0 || CheckedIssue.Length > 0)
                                        {
                                            errMsg1.Append("DFX " + WriteDept + "部門人員為空時, Issue也必須都為空</br>");
                                        }
                                        if (WriteFMEA.Length > 0 || ReplyFMEA.Length > 0 || CheckedFMEA.Length > 0)
                                        {
                                            errMsg1.Append("DFX " + WriteDept + "部門人員為空時, FMEA也必須都為空</br>");
                                        }
                                    }
                                    if (DFXType == "CTQ TeamMember")
                                    {
                                        if (WriteDFX.Length > 0 || ReplyDFX.Length > 0 || CheckedDFX.Length > 0)
                                        {
                                            errMsg1.Append("CTQ " + WriteDept + "部門人員為空時, DFX也必須都為空</br>");
                                        }
                                        if (WriteIssue.Length > 0 || ReplyIssue.Length > 0 || CheckedIssue.Length > 0)
                                        {
                                            errMsg1.Append("CTQ " + WriteDept + "部門人員為空時, Issue也必須都為空</br>");
                                        }
                                        if (WriteFMEA.Length > 0 || ReplyFMEA.Length > 0 || CheckedFMEA.Length > 0)
                                        {
                                            errMsg1.Append("CTQ " + WriteDept + "部門人員為空時, FMEA也必須都為空</br>");
                                        }
                                    }
                                    if (DFXType == "ISSUES TeamMember")
                                    {
                                        if (WriteDFX.Length > 0 || ReplyDFX.Length > 0 || CheckedDFX.Length > 0)
                                        {
                                            errMsg1.Append("Issue " + WriteDept + "部門人員為空時, DFX也必須都為空</br>");
                                        }
                                        if (WriteCTQ.Length > 0 || ReplyCTQ.Length > 0 || CheckedCTQ.Length > 0)
                                        {
                                            errMsg1.Append("Issue " + WriteDept + "部門人員為空時, CTQ也必須都為空</br>");
                                        }
                                        if (WriteFMEA.Length > 0 || ReplyFMEA.Length > 0 || CheckedFMEA.Length > 0)
                                        {
                                            errMsg1.Append("Issue " + WriteDept + "部門人員為空時, FMEA也必須都為空</br>");
                                        }
                                    }
                                    if (DFXType == "PFMEA TeamMember")
                                    {
                                        if (WriteDFX.Length > 0 || ReplyDFX.Length > 0 || CheckedDFX.Length > 0)
                                        {
                                            errMsg1.Append("FMEA " + WriteDept + "部門人員為空時, DFX也必須都為空</br>");
                                        }
                                        if (WriteCTQ.Length > 0 || ReplyCTQ.Length > 0 || CheckedCTQ.Length > 0)
                                        {
                                            errMsg1.Append("FMEA " + WriteDept + "部門人員為空時, CTQ也必須都為空</br>");
                                        }
                                        if (WriteIssue.Length > 0 || ReplyIssue.Length > 0 || CheckedIssue.Length > 0)
                                        {
                                            errMsg1.Append("FMEA " + WriteDept + "部門人員為空時, Issue也必須都為空</br>");
                                        }
                                    }
                                    #endregion

                                    #region Check Part2
                                    
                                    #endregion
                                }

                                if (errMsg1.Length > 0)
                                {
                                    DeleteExist(txtDOC_NO.Text);//有異常則刪除已上傳的資料
                                    BindMember(txtDOC_NO.Text, string.Empty);
                                    Alert(string.Format("錯誤信息:<BR/>{0}", errMsg1.ToString()));
                                    return;
                                }
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

        //存在錯誤信息 則刪除已新增的數據
        if (errMsg.Length > 0 )
        {
            DeleteExist(txtDOC_NO.Text);
            BindMember(txtDOC_NO.Text, string.Empty);
            Alert(string.Format("錯誤信息:<BR/>{0}", errMsg.ToString()));
        }
        else
        {
            BindMember(txtDOC_NO.Text, string.Empty);
            Alert(string.Format("上傳筆數:{0}<BR/>成功筆數:{1}<BR/>失敗筆數:{2}<BR/>錯誤信息:<BR/>{3}", total_num.ToString(), ok_num.ToString(), ng_num.ToString(), errMsg.ToString()));
        }
    }

    #endregion

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

    private void BindCombox(ComboBox cmb, DataTable dt)
    {
        cmb.Store.Primary.DataSource = dt;
        cmb.Store.Primary.DataBind();
    }
   
    /// <summary>
    /// 自定義ALERT方法
    /// </summary>
    /// <param name="msg"></param>
    private void Alert(string msg)
    {
        X.Msg.Alert("Alert", msg).Show();
    }

    private DataTable CheckUserInfo(string Logonid)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select * from SPM.dbo.[USER] WHERE LOGONID=@LOGONID");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@LOGONID",DbType.String,Logonid));
        return sdb.TransactionExecute(sb.ToString(),opc);
    }

    private bool IsStepName(string str)
    {
        bool Status = false;
        switch (str)
        {
            case "DFX TeamMember":
            case "CTQ TeamMember":
            case "ISSUES TeamMember":
            case "PFMEA TeamMember":
            case "Prepared TeamMember":
            case "PM TeamMember":
            case "Manager TeamMember":
                Status = true;
                break;
        }
        return Status;
    }

    private void DeleteExist(string DocNo)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"delete  from TB_NPI_APP_MEMBER WHERE DOC_NO=@DOC_NO");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DOC_NO", DbType.String, DocNo));
        sdb.TransactionExecuteScalar(sb.ToString(),opc);
    }

    private DataTable CheckFormNo(string ModelName)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select * from TB_NPI_APP_MAIN where MODEL_NAME = @MODEL_NAME");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@MODEL_NAME", DbType.String, ModelName));
        return sdb.TransactionExecute(sb.ToString(), opc);
    }

    private DataTable CheckDFXDept(string  dept,string ProductType)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select distinct writedept from TB_DFX_Item where Building = @Building and Writedept = @Writedept and ProductType = @ProductType");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@Building", DbType.String, lblBuilding.Text));
        opc.Add(DataPara.CreateDataParameter("@Writedept", DbType.String, dept));
        opc.Add(DataPara.CreateDataParameter("@ProductType", DbType.String, ProductType));
        return sdb.TransactionExecute(sb.ToString(), opc);
    }

    private DataTable CompareDeptCountDFX(string DOCNO,string ProductType,string Category)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"SELECT  STUFF((
                    SELECT ';' + WriteDept FROM
                    (select distinct WriteDept from TB_DFX_Item
                    WHERE BU='POWER' AND BUILDING= @P_Building
                    AND ProductType=@ProductType AND WriteDept  NOT IN
                    (SELECT DISTINCT DEPT FROM TB_NPI_APP_MEMBER
                    WHERE DOC_NO=@P_DOC_NO AND Category=@Category)) T1   for xml path('')),1,1,'') as Dept");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@P_Building", DbType.String, lblBuilding.Text));
        opc.Add(DataPara.CreateDataParameter("@P_DOC_NO", DbType.String, DOCNO));
        opc.Add(DataPara.CreateDataParameter("@ProductType", DbType.String, ProductType));
        opc.Add(DataPara.CreateDataParameter("@Category", DbType.String, Category));
        return sdb.TransactionExecute(sb.ToString(), opc);

    }

    private DataTable CompareDeptCountCTQ(string DOCNO, string ProductType, string Category)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"SELECT STUFF((
					      SELECT ';' + DEPT FROM
							(select distinct DEPT from TB_NPI_CTQ
							WHERE BU='POWER' AND BUILDING=@P_Building
							AND PROD_GROUP=@P_GROD_GROUP AND DEPT  NOT IN
							(SELECT DISTINCT DEPT FROM TB_NPI_APP_MEMBER
							WHERE DOC_NO=@P_DOC_NO AND Category=@Category)) T1   for xml path('')),1,1,'')  as Dept");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@P_Building", DbType.String, lblBuilding.Text));
        opc.Add(DataPara.CreateDataParameter("@P_DOC_NO", DbType.String, DOCNO));
        opc.Add(DataPara.CreateDataParameter("@P_GROD_GROUP", DbType.String, ProductType));
        opc.Add(DataPara.CreateDataParameter("@Category", DbType.String, Category));
        return sdb.TransactionExecute(sb.ToString(), opc);

    }



    private DataTable CheckCTQDept(string dept)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select distinct DEPT from TB_NPI_CTQ where BUILDING = @BUILDING and DEPT = @DEPT");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@BUILDING", DbType.String, lblBuilding.Text));
        opc.Add(DataPara.CreateDataParameter("@DEPT", DbType.String, dept));
        return sdb.TransactionExecute(sb.ToString(), opc);
    }

    private DataTable GetMember(string DocNo, string Category,string  dept)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select * from TB_NPI_APP_MEMBER where DOC_NO = @DOC_NO and Category = @Category and DEPT = @DEPT");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DOC_NO", DbType.String, DocNo));
        opc.Add(DataPara.CreateDataParameter("@Category", DbType.String, Category));
        opc.Add(DataPara.CreateDataParameter("@DEPT", DbType.String, dept));
        return sdb.TransactionExecute(sb.ToString(), opc);
    }

}
