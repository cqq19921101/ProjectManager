using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Ext.Net;
using LiteOn.EA.DAL;
using System.Collections.Generic;
using System.Text;
using LiteOn.EA.BLL;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using LiteOn.EA.CommonModel;
using LiteOn.EA.Borg.Utility;
using LiteOn.EA.NPIReport.Utility;
using System.Diagnostics;
using Aspose.Pdf;
using Aspose.Pdf.Facades;
using Aspose.Cells;
using Aspose.Cells.Rendering;

public partial class QueryReport : System.Web.UI.Page
{
    SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
    ArrayList opc = new ArrayList();
    int function_id = 1290;
    string sql = string.Empty;
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
                Alert("对不起，你没有权限操作!");
                btnQuery.Disabled = true;
                return;
            }
            else
            {
                Borg_User oBorg_User = new Borg_User();
                Model_BorgUserInfo oModel_BorgUserInfo = new Model_BorgUserInfo();
                oModel_BorgUserInfo = oBorg_User.GetUserInfoByLogonId(lblLogonId.Text);
                if (oModel_BorgUserInfo._EXISTS)
                {
                    lblBu.Text = oModel_BorgUserInfo._BU;
                    //lblBuilding.Text = oModel_BorgUserInfo._Building;
                    lblBuilding.Text = "A6";

                }
                dfBeginTime.SelectedDate = DateTime.Parse("2018-04-01");
                dfEndTime.SelectedDate = DateTime.Today;
                btnQuery_click(null, null);

            }

        }

    }

    #region 頁面部份
    protected void btnQuery_click(object sender, DirectEventArgs e)
    {
        StringBuilder sbALL = new StringBuilder();
        StringBuilder sbPass = new StringBuilder();
        StringBuilder sbFail = new StringBuilder();
        StringBuilder sbPending = new StringBuilder();
        StringBuilder sbReject = new StringBuilder();
        string Model = txtModel.Text.Trim();
        string Customer = txtCustomer.Text.Trim();
        string FormNo = txtFormNo.Text.Trim();
        string Building = cmbBuilding.SelectedItem.Text.ToString();
        string Status = cmbStatus.SelectedItem.Text.ToString();
        string bgDate = dfBeginTime.SelectedDate.ToString("yyyy-MM-dd");
        string endDate = dfEndTime.SelectedDate.ToString("yyyy-MM-dd");

        string ApplyUser = txtApplyUser.Text.Trim();
        string ProductType = cmbProduct.SelectedItem.Text.Trim();
        opc.Clear();
        opc.Add(DataPara.CreateProcParameter("@P_BU", SqlDbType.VarChar, 10, ParameterDirection.Input, "POWER"));
        opc.Add(DataPara.CreateProcParameter("@P_BUILDING", SqlDbType.VarChar, 10, ParameterDirection.Input, Building));
        opc.Add(DataPara.CreateProcParameter("@P_STAUTS", SqlDbType.VarChar, 10, ParameterDirection.Input, Status));
        opc.Add(DataPara.CreateProcParameter("@P_MODEL", SqlDbType.VarChar, 20, ParameterDirection.Input, Model));
        opc.Add(DataPara.CreateProcParameter("@P_CUSTOMER", SqlDbType.VarChar, 30, ParameterDirection.Input, Customer));
        opc.Add(DataPara.CreateProcParameter("@P_ApplyUser", SqlDbType.VarChar, 30, ParameterDirection.Input, ApplyUser));
        opc.Add(DataPara.CreateProcParameter("@P_ProductType", SqlDbType.VarChar, 30, ParameterDirection.Input, ProductType));
        opc.Add(DataPara.CreateProcParameter("@P_FORMNO", SqlDbType.VarChar, 30, ParameterDirection.Input, FormNo));
        opc.Add(DataPara.CreateProcParameter("@P_BEGDATE", SqlDbType.VarChar, 10, ParameterDirection.Input, bgDate));
        opc.Add(DataPara.CreateProcParameter("@P_ENDDATE", SqlDbType.VarChar, 10, ParameterDirection.Input, endDate));
        DataTable dt = sdb.RunProc2("P_GET_NPI_REPORT", opc);
        BingGrid(grdList, dt);

        #region 計算各案件狀態的數量
        sbALL.Append(@"
                        SELECT Count(*)
                        FROM TB_NPI_APP_SUB T1
                        LEFT JOIN TB_NPI_APP_MAIN T2
                        ON T1.DOC_NO=T2.DOC_NO
                        where convert(varchar(10),T1.CREATE_DATE,121)BETWEEN @BeginDate and @EndDate 
                        and T2.BUILDING = @Building
                    ");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@BeginDate", SqlDbType.VarChar, bgDate));
        opc.Add(DataPara.CreateDataParameter("@EndDate", SqlDbType.VarChar, endDate));
        opc.Add(DataPara.CreateDataParameter("@Building", SqlDbType.VarChar, Building));
        if (Customer.Length > 0)
        {
            sbALL.Append(" and T1.CUSTOMER =@CUSTOMER");
            opc.Add(DataPara.CreateDataParameter("@CUSTOMER", SqlDbType.VarChar, Customer));
        }
        if (Model.Length > 0)
        {
            sbALL.Append(" and T2.MODEL_NAME =@Model");
            opc.Add(DataPara.CreateDataParameter("@Model", SqlDbType.VarChar, Model));
        }
        if (ApplyUser.Length > 0)
        {
            sbALL.Append(" and T2.NPI_PM =@NPI_PM");
            opc.Add(DataPara.CreateDataParameter("@NPI_PM", SqlDbType.VarChar, ApplyUser));
        }
        if (cmbProduct.Text.Length > 0)
        {
            sbALL.Append(" and T1.PROD_GROUP =@PROD_GROUP");
            opc.Add(DataPara.CreateDataParameter("@PROD_GROUP", SqlDbType.VarChar, ProductType));
        }
        if (Status.Length > 0 && Status != "ALL")
        {
            sbALL.Append(" and Status =@Status");
            opc.Add(DataPara.CreateDataParameter("@Status", SqlDbType.VarChar, Status));
        }
        DataTable dtALL = sdb.GetDataTable(sbALL.ToString(), opc);
        txtCaseidCount.Text = dtALL.Rows[0][0].ToString();

        sbPass.Append(@"
                        SELECT Count(*)
                        FROM TB_NPI_APP_SUB T1
                        LEFT JOIN TB_NPI_APP_MAIN T2
                        ON T1.DOC_NO=T2.DOC_NO
                        where convert(varchar(10),T1.CREATE_DATE,121)BETWEEN @BeginDate and @EndDate 
                        and T2.BUILDING = @Building and T1.STATUS = 'Finished'
                  ");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@BeginDate", SqlDbType.VarChar, bgDate));
        opc.Add(DataPara.CreateDataParameter("@EndDate", SqlDbType.VarChar, endDate));
        opc.Add(DataPara.CreateDataParameter("@Building", SqlDbType.VarChar, Building));
        DataTable dtPass = sdb.GetDataTable(sbPass.ToString(), opc);
        txtFinishedCount.Text = dtPass.Rows[0][0].ToString();

        sbFail.Append(@"
                        SELECT Count(*)
                        FROM TB_NPI_APP_SUB T1
                        LEFT JOIN TB_NPI_APP_MAIN T2
                        ON T1.DOC_NO=T2.DOC_NO
                        where convert(varchar(10),T1.CREATE_DATE,121)BETWEEN @BeginDate and @EndDate 
                        and T2.BUILDING = @Building and T1.STATUS = 'Fail'
                  ");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@BeginDate", SqlDbType.VarChar, bgDate));
        opc.Add(DataPara.CreateDataParameter("@EndDate", SqlDbType.VarChar, endDate));
        opc.Add(DataPara.CreateDataParameter("@Building", SqlDbType.VarChar, Building));
        if (Customer.Length > 0)
        {
            sbPass.Append(" and T1.CUSTOMER =@CUSTOMER");
            opc.Add(DataPara.CreateDataParameter("@CUSTOMER", SqlDbType.VarChar, Customer));
        }
        if (Model.Length > 0)
        {
            sbPass.Append(" and T2.MODEL_NAME =@Model");
            opc.Add(DataPara.CreateDataParameter("@Model", SqlDbType.VarChar, Model));
        }
        if (ApplyUser.Length > 0)
        {
            sbPass.Append(" and T2.NPI_PM =@NPI_PM");
            opc.Add(DataPara.CreateDataParameter("@NPI_PM", SqlDbType.VarChar, ApplyUser));
        }
        if (cmbProduct.Text.Length > 0)
        {
            sbPass.Append(" and T1.PROD_GROUP =@PROD_GROUP");
            opc.Add(DataPara.CreateDataParameter("@PROD_GROUP", SqlDbType.VarChar, ProductType));
        }
        if (Status.Length > 0 && Status != "ALL")
        {
            sbPass.Append(" and Status =@Status");
            opc.Add(DataPara.CreateDataParameter("@Status", SqlDbType.VarChar, Status));
        }
        DataTable dtFail = sdb.GetDataTable(sbFail.ToString(), opc);
        txtFailCount.Text = dtFail.Rows[0][0].ToString();

        sbPending.Append(@"
                        SELECT Count(*)
                        FROM TB_NPI_APP_SUB T1
                        LEFT JOIN TB_NPI_APP_MAIN T2
                        ON T1.DOC_NO=T2.DOC_NO
                        where convert(varchar(10),T1.CREATE_DATE,121)BETWEEN @BeginDate and @EndDate 
                        and T2.BUILDING = @Building and T1.STATUS = 'Pending'
                  ");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@BeginDate", SqlDbType.VarChar, bgDate));
        opc.Add(DataPara.CreateDataParameter("@EndDate", SqlDbType.VarChar, endDate));
        opc.Add(DataPara.CreateDataParameter("@Building", SqlDbType.VarChar, Building));
        if (Customer.Length > 0)
        {
            sbPending.Append(" and T1.CUSTOMER =@CUSTOMER");
            opc.Add(DataPara.CreateDataParameter("@CUSTOMER", SqlDbType.VarChar, Customer));
        }
        if (Model.Length > 0)
        {
            sbPending.Append(" and T2.MODEL_NAME =@Model");
            opc.Add(DataPara.CreateDataParameter("@Model", SqlDbType.VarChar, Model));
        }
        if (ApplyUser.Length > 0)
        {
            sbPending.Append(" and T2.NPI_PM =@NPI_PM");
            opc.Add(DataPara.CreateDataParameter("@NPI_PM", SqlDbType.VarChar, ApplyUser));
        }
        if (cmbProduct.Text.Length > 0)
        {
            sbPending.Append(" and T1.PROD_GROUP =@PROD_GROUP");
            opc.Add(DataPara.CreateDataParameter("@PROD_GROUP", SqlDbType.VarChar, ProductType));
        }
        if (Status.Length > 0 && Status != "ALL")
        {
            sbPending.Append(" and Status =@Status");
            opc.Add(DataPara.CreateDataParameter("@Status", SqlDbType.VarChar, Status));
        }
        DataTable dtPending = sdb.GetDataTable(sbPending.ToString(), opc);
        txtPendingCount.Text = dtPending.Rows[0][0].ToString();

        sbReject.Append(@"
                        SELECT Count(*)
                        FROM TB_NPI_APP_SUB T1
                        LEFT JOIN TB_NPI_APP_MAIN T2
                        ON T1.DOC_NO=T2.DOC_NO
                        where convert(varchar(10),T1.CREATE_DATE,121)BETWEEN @BeginDate and @EndDate 
                        and T2.BUILDING = @Building and T1.STATUS = 'Reject'
                  ");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@BeginDate", SqlDbType.VarChar, bgDate));
        opc.Add(DataPara.CreateDataParameter("@EndDate", SqlDbType.VarChar, endDate));
        opc.Add(DataPara.CreateDataParameter("@Building", SqlDbType.VarChar, Building));
        if (Customer.Length > 0)
        {
            sbALL.Append(" and T1.CUSTOMER =@CUSTOMER");
            opc.Add(DataPara.CreateDataParameter("@CUSTOMER", SqlDbType.VarChar, Customer));
        }
        if (Model.Length > 0)
        {
            sbReject.Append(" and T2.MODEL_NAME =@Model");
            opc.Add(DataPara.CreateDataParameter("@Model", SqlDbType.VarChar, Model));
        }
        if (ApplyUser.Length > 0)
        {
            sbReject.Append(" and T2.NPI_PM =@NPI_PM");
            opc.Add(DataPara.CreateDataParameter("@NPI_PM", SqlDbType.VarChar, ApplyUser));
        }
        if (cmbProduct.Text.Length > 0)
        {
            sbReject.Append(" and T1.PROD_GROUP =@PROD_GROUP");
            opc.Add(DataPara.CreateDataParameter("@PROD_GROUP", SqlDbType.VarChar, ProductType));
        }
        if (Status.Length > 0 && Status != "ALL")
        {
            sbReject.Append(" and Status =@Status");
            opc.Add(DataPara.CreateDataParameter("@Status", SqlDbType.VarChar, Status));
        }
        DataTable dtReject = sdb.GetDataTable(sbReject.ToString(), opc);
        txtRejectCount.Text = dtReject.Rows[0][0].ToString();
        #endregion
    }

    protected void cmbIssuseType_Selected(object sender, DirectEventArgs e)
    {
        btnQuery_click(null, null);
    }

    protected void cmbStatus_Selected(object sender, DirectEventArgs e)
    {
        btnQuery_click(null, null);
    }

    protected void cmbType_Selected(object sender, DirectEventArgs e)
    {
        btnQuery_click(null, null);
    }

    protected void DOCProcess_Look_Click(object sender, DirectEventArgs e)
    {
        string CASEID = e.ExtraParams["CASEID"];
        string Status = e.ExtraParams["Status"];
        string DocNo = e.ExtraParams["SUB_DOC_NO"];
        string Bu = e.ExtraParams["BU"];
        string Building = e.ExtraParams["BUILDING"];
        string Model = e.ExtraParams["MODEL_NAME"];
        string command = e.ExtraParams["command"];
        switch (command)
        {
            case "DetailInfo":
                string Path = System.Configuration.ConfigurationSettings.AppSettings["WFShowCase"].ToString();
                string URL = Path.Replace("{CASEID}", CASEID);
                X.Js.Call("ReportDetail", URL);
                break;
            case "ExportXLS":
                try
                {
                    CreateDirectory(DocNo, CASEID, Bu, Building);
                    CopyFile(CASEID, DocNo, Bu, Building);
                    ExportToExcel(CASEID, DocNo, Bu, Building, Model);//導出到Excel
                    CompressedfFolder(CASEID, DocNo, Bu, Building);//壓縮文件夾
                    DeleteExistFolder(CASEID, DocNo, Bu, Building);//刪除Source文件夾
                    Alert("Excel生成成功！");

                }
                catch (Exception ex)
                {
                    Alert(ex.Message);
                }
                break;
            case "DownLoadXLS"://Excel
                //將單據的附件文件夾壓縮后供用戶下載
                //壓縮xls文件
                try
                {
                    string floderPath = Server.MapPath("../E-Report/Attachment/" + Model + "_NPI_DOC");
                    string floderPathZip = Server.MapPath("../E-Report/Attachment/" + Model + "_NPI_DOC" + ".zip");
                    string floderPathDownLoad = "../E-Report/Attachment/" + Model + "_NPI_DOC" + ".zip";
                    string fileNameDownLoad = Model + "_NPI_DOC" + ".zip";
                    BonkerZip tool = new BonkerZip();
                    tool.AddFile(floderPath);
                    tool.CompressionZip(floderPathZip);
                    X.Js.Call("NPIFileDownload", fileNameDownLoad, floderPathDownLoad);

                }
                catch (Exception ex)
                {
                    Alert("Error:" + ex.Message);
                } 
                break;
            case "DownLoadPDF"://PDF
                try
                {
                    string floderPathP = Server.MapPath("../E-Report/Attachment/" + Model + "_NPI_PDF");//CZ
                    string floderPathP_TW = Server.MapPath("../E-Report/Attachment_TW/" + Model + "_NPI_PDF");//TW

                    string floderPathZipP = Server.MapPath("../E-Report/Attachment/" + Model + "_NPI_PDF" + ".zip");//cz
                    string floderPathZipP_TW = Server.MapPath("../E-Report/Attachment_TW/" + Model + "_NPI_PDF" + ".zip");//TW

                    string floderPathDownLoadP = "../E-Report/Attachment/" + Model + "_NPI_PDF" + ".zip";
                    string fileNameDownLoadP = Model + "_NPI_PDF" + ".zip";

                    //压缩之前先將Attachment中該機種的pdf文件夾複製到Attachment_TW中,删除非该阶段的文件夹 例如階段為EVT-1時,只保留EVT-1的文件夾,其餘全部刪除
                    DeleteFolderForTW(DocNo);

                    //压缩文件
                    BonkerZip toolPCZ = new BonkerZip();
                    BonkerZip toolPTW = new BonkerZip();
                    toolPCZ.AddFile(floderPathP);//CZ
                    toolPCZ.CompressionZip(floderPathZipP);//CZ

                    toolPTW.AddFile(floderPathP_TW);//TW
                    toolPTW.CompressionZip(floderPathZipP_TW);//TW

                    //生成壓縮檔后,刪除該機種在Attachment_TW文件夾中的Source文件夾
                    DeleteDirectory(floderPathP_TW);

                    X.Js.Call("NPI_File_Download_PDF", fileNameDownLoadP, floderPathDownLoadP);

                }
                catch (Exception ex )
                {
                    Alert("Error:" + ex.Message);
                    return;
                } 
                break;
        }

    }

    protected void cmbBuilding_Select(object sender, DirectEventArgs e)
    {

    }

    /// <summary>
    /// 自定義ALERT方法
    /// </summary>
    /// <param name="msg"></param>
    private void Alert(string msg)
    {
        X.Msg.Alert("Alert", msg).Show();
    }

    private void BingGrid(GridPanel grd, DataTable dt)
    {
        grd.Store.Primary.DataSource = dt;
        grd.Store.Primary.DataBind();
    }

    private void BindCombox(ComboBox cmb, DataTable dt)
    {
        cmb.Store.Primary.DataSource = dt;
        cmb.Store.Primary.DataBind();
    }

    protected void btnToExcel_Click(object sender, EventArgs e)
    {
        string json = GridData.Value.ToString();
        #region
        json = json.Replace("CaseId", "案件編號");
        json = json.Replace("CREATE_DATE", "開單日期");
        json = json.Replace("SUB_DOC_NO", "表單單號");
        json = json.Replace("BU", "BU");
        json = json.Replace("Building", "Building");
        json = json.Replace("PROD_GROUP", "產品類別");
        json = json.Replace("MODEL_NAME", "機種");
        json = json.Replace("STATUS", "簽核狀態");
        json = json.Replace("handler", "待簽核人");
        #endregion

        StoreSubmitDataEventArgs eSubmit = new StoreSubmitDataEventArgs(json, null);
        XmlNode xml = eSubmit.Xml;
        XmlNodeList xm = xml.SelectSingleNode("records").ChildNodes;
        foreach (XmlNode xnl in xm)
        {
            XmlElement xe = (XmlElement)xnl;
            XmlNode xn = xe.SelectSingleNode("id");
            xnl.RemoveChild(xn);
        }

        this.Response.Clear();
        this.Response.ContentType = "application/vnd.ms-excel";
        this.Response.AddHeader("Content-Disposition", "attachment; filename=NPI Report.xls");
        XslCompiledTransform xtExcel = new XslCompiledTransform();
        xtExcel.Load(Server.MapPath("../Excel.xsl"));
        xtExcel.Transform(xml, null, this.Response.OutputStream);
        this.Response.End();
    }

    #endregion

    #region Copy Workflow中上傳的附件到服務器上

    private void CopyFile(string caseID, string DocNo, string Bu, string Building)
    {

        DataTable dtM = GetMaster(DocNo);
        DataRow drM = dtM.Rows[0];
        string Model = drM["MODEL_NAME"].ToString();
        string Stage = drM["SUB_DOC_PHASE"].ToString();
        string ModifyFlag = drM["MODIFY_FLAG"].ToString();
        #region
        string typeIssue = "Issue List Doc";
        string typeCTQ = "MFG CTQ Doc";
        string typeCPK = "Test CPK Doc";
        string typeCPKEE = "EE Doc";
        string typeCPKTE = "TE Doc";
        string typePRSMT = "SMT Doc";
        string typePRIE = "IE Doc";
        string typeCTQSQ = "SQ Doc";
        string typeCTQUQ = "UQ Doc";
        string typeCTQIQC = "IQC Doc";
        string typeCTQAIRI = "AIRI Doc";
        string typeIssuePM = "Issue PM";
        #endregion

        if (ModifyFlag == "N")
        {
            #region Copy Issue List PM
            DirectoryInfo theFolder = new DirectoryInfo(@"\\icm651\NPIGating_Attachment\");//源文件夹
            string sourceFileName = theFolder.ToString() + Model + "_NPI_DOC" + "\\" + Stage + "\\" + typeIssue + "\\" + typeIssuePM;
            //string sourceFileName = @FileName;//源文件夹
            string destFileName = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeIssue + "/" + typeIssuePM);//目标文件夹 Excel
            if (Directory.Exists(sourceFileName) && Directory.Exists(destFileName))
            {
                CopyDirectory(sourceFileName, destFileName);
            }

            #endregion
        }
        else
        {
            #region COPY Modify PM
            DirectoryInfo theFolderM = new DirectoryInfo(@"\\icm651\NPIGating_Attachment\");//源文件夹
            string sourceFileNameM = theFolderM.ToString() + Model + "_NPI_DOC" + "\\" + Stage + "\\" + typeCTQ + "\\" + "PM Doc";
            //string sourceFileName = @FileName;//源文件夹
            string destFileNameM = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeCTQ + "/" + "PM Doc");//目标文件夹 Excel
            if (Directory.Exists(sourceFileNameM) && Directory.Exists(destFileNameM))
            {
                CopyDirectory(sourceFileNameM, destFileNameM);
            }
            #endregion
        }


        #region Copy CPK-EE
        DirectoryInfo theFolderCPKEE = new DirectoryInfo(@"\\icm651\NPIGating_Attachment\");//源文件夹
        string sourceFileNameCPKEE = theFolderCPKEE.ToString() + Model + "_NPI_DOC" + "\\" + Stage + "\\" + typeCPK + "\\" + typeCPKEE;
        //string sourceFileName = @FileName;//源文件夹
        string destFileNameCPKEE = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "\\" + Stage + "\\" + typeCPK + "\\" + typeCPKEE);//目标文件夹 Excel
        if (Directory.Exists(sourceFileNameCPKEE) && Directory.Exists(destFileNameCPKEE))
        {
            CopyDirectory(sourceFileNameCPKEE, destFileNameCPKEE);
        }
        #endregion

        #region Copy CPK-TE
        DirectoryInfo theFolderCPKTE = new DirectoryInfo(@"\\icm651\NPIGating_Attachment\");//源文件夹
        string sourceFileNameCPKTE = theFolderCPKTE.ToString() + Model + "_NPI_DOC" + "\\" + Stage + "\\" + typeCPK + "\\" + typeCPKTE;
        //string sourceFileName = @FileName;//源文件夹
        string destFileNameCPKTE = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "\\" + Stage + "\\" + typeCPK + "\\" + typeCPKTE);//目标文件夹 Excel
        if (Directory.Exists(sourceFileNameCPKTE) && Directory.Exists(destFileNameCPKTE))
        {
            CopyDirectory(sourceFileNameCPKTE, destFileNameCPKTE);
        }
        #endregion

        #region Copy CTQ-SQ
        DirectoryInfo theFolderCPKSQ = new DirectoryInfo(@"\\icm651\NPIGating_Attachment\");//源文件夹
        string sourceFileNameCPKSQ = theFolderCPKSQ.ToString() + Model + "_NPI_DOC" + "\\" + Stage + "\\" + typeCTQ + "\\" + typeCTQSQ;
        //string sourceFileName = @FileName;//源文件夹
        string destFileNameCPKSQ = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "\\" + Stage + "\\" + typeCTQ + "\\" + typeCTQSQ);//目标文件夹 EXCEL
        if (Directory.Exists(sourceFileNameCPKSQ) && Directory.Exists(destFileNameCPKSQ))
        {
            CopyDirectory(sourceFileNameCPKSQ, destFileNameCPKSQ);
        }
        #endregion

        #region Copy CTQ-UQ
        DirectoryInfo theFolderCPKUQ = new DirectoryInfo(@"\\icm651\NPIGating_Attachment\");//源文件夹
        string sourceFileNameCPKUQ = theFolderCPKUQ.ToString() + Model + "_NPI_DOC" + "\\" + Stage + "\\" + typeCTQ + "\\" + typeCTQUQ;
        //string sourceFileName = @FileName;//源文件夹
        string destFileNameCPKUQ = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "\\" + Stage + "\\" + typeCTQ + "\\" + typeCTQUQ);//目标文件夹 EXCEL
        if (Directory.Exists(sourceFileNameCPKUQ) && Directory.Exists(destFileNameCPKUQ))
        {
            CopyDirectory(sourceFileNameCPKUQ, destFileNameCPKUQ);
        }
        #endregion

        #region Copy CTQ-IQC
        DirectoryInfo theFolderCPKIQC = new DirectoryInfo(@"\\icm651\NPIGating_Attachment\");//源文件夹
        string sourceFileNameCPKIQC = theFolderCPKIQC.ToString() + Model + "_NPI_DOC" + "\\" + Stage + "\\" + typeCTQ + "\\" + typeCTQIQC;
        //string sourceFileName = @FileName;//源文件夹
        string destFileNameCPKIQC = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "\\" + Stage + "\\" + typeCTQ + "\\" + typeCTQIQC);//目标文件夹
        if (Directory.Exists(sourceFileNameCPKIQC) && Directory.Exists(destFileNameCPKIQC))
        {
            CopyDirectory(sourceFileNameCPKIQC, destFileNameCPKIQC);
        }
        #endregion

        #region Copy CTQ-AIRI
        DirectoryInfo theFolderCPKAIRI = new DirectoryInfo(@"\\icm651\NPIGating_Attachment\");//源文件夹
        string sourceFileNameCPKAIRI = theFolderCPKAIRI.ToString() + Model + "_NPI_DOC" + "\\" + Stage + "\\" + typeCTQ + "\\" + typeCTQAIRI;
        //string sourceFileName = @FileName;//源文件夹
        string destFileNameCPKAIRI = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "\\" + Stage + "\\" + typeCTQ + "\\" + typeCTQAIRI);//目标文件夹
        if (Directory.Exists(sourceFileNameCPKAIRI) && Directory.Exists(destFileNameCPKAIRI))
        {
            CopyDirectory(sourceFileNameCPKAIRI, destFileNameCPKAIRI);
        }
        #endregion

        #region Copy PR-CTQ-SMT
        DirectoryInfo theFolderPRSMT = new DirectoryInfo(@"\\icm651\NPIGating_Attachment\");//源文件夹
        string sourceFileNamePRSMT = theFolderPRSMT.ToString() + Model + "_NPI_DOC" + "\\" + Stage + "\\" + typeCTQ + "\\" + typePRSMT;
        //string sourceFileName = @FileName;//源文件夹
        string destFileNamePRSMT = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "\\" + Stage + "\\" + typeCTQ + "\\" + typePRSMT);//目标文件夹
        if (Directory.Exists(sourceFileNamePRSMT) && Directory.Exists(destFileNamePRSMT))
        {
            CopyDirectory(sourceFileNamePRSMT, destFileNamePRSMT);
        }
        #endregion

        #region Copy PR-CTQ-IE
        DirectoryInfo theFolderPRIE = new DirectoryInfo(@"\\icm651\NPIGating_Attachment\");//源文件夹
        string sourceFileNamePRIE = theFolderPRIE.ToString() + Model + "_NPI_DOC" + "\\" + Stage + "\\" + typeCTQ + "\\" + typePRIE;
        //string sourceFileName = @FileName;//源文件夹
        string destFileNamePRIE = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "\\" + Stage + "\\" + typeCTQ + "\\" + typePRIE);//目标文件夹
        if (Directory.Exists(sourceFileNamePRIE) && Directory.Exists(destFileNamePRIE))
        {
            CopyDirectory(sourceFileNamePRIE, destFileNamePRIE);
        }
        #endregion


    }

    private void CopyDirectory(string srcPath, string destPath)
    {
        try
        {
            DirectoryInfo dir = new DirectoryInfo(srcPath);
            FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();//獲取目錄下的文件和子目錄
            foreach (FileSystemInfo i in fileinfo)
            {
                if (i is DirectoryInfo)//判斷是否是文件夾
                {
                    if (!Directory.Exists(destPath + "\\" + i.Name))
                    {
                        Directory.CreateDirectory(destPath + "\\" + i.Name);//目標目錄下不存在此文件夾則創建
                    }
                    CopyDir(i.FullName, destPath + "\\" + i.Name);//复制子文件夹
                }
                else
                {
                    File.Copy(i.FullName, destPath + "\\" + i.Name, true);//不是文件夾則複製文件,可覆蓋
                }
            }
        }
        catch (Exception e)
        {
            throw;
        }
    }

    /// <summary>
    /// COPY icm651的文件到icm656指定文件夹中
    /// </summary>
    /// <param name="srcPath"></param>
    /// <param name="destPath"></param>
    private void CopyDir(string srcPath, string aimPath)
    {
        try
        {
            // 检查目标目录是否以目录分割字符结束如果不是则添加
            if (aimPath[aimPath.Length - 1] != System.IO.Path.DirectorySeparatorChar)
            {
                aimPath += System.IO.Path.DirectorySeparatorChar;
            }
            // 判断目标目录是否存在如果不存在则新建
            if (!System.IO.Directory.Exists(aimPath))
            {
                System.IO.Directory.CreateDirectory(aimPath);
            }
            string[] fileList = System.IO.Directory.GetFileSystemEntries(srcPath);
            // 遍历所有的文件和目录
            foreach (string file in fileList)
            {
                // 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                if (System.IO.Directory.Exists(file))
                {
                    CopyDir(file, aimPath + System.IO.Path.GetFileName(file));
                }
                // 否则直接Copy文件
                else
                {
                    System.IO.File.Copy(file, aimPath + System.IO.Path.GetFileName(file), true);
                }
            }
        }
        catch (Exception e)
        {
            throw;
        }
    }

    #endregion

    #region 生成報表
    private void ExportToExcel(string caseID, string DocNo, string Bu, string Building, string Model)
    {
        //X.Js.Call("ExportXLS", caseID, DocNo,Bu,Building);
        try
        {
            ExportXLS(DocNo, caseID, Bu, Building, Model);//生成Excel原始檔 
        }
        catch (Exception ex)
        {

        }
    }

    #region 導出Excel

    private void ExportXLS(string docno, string caseID, string Bu, string Building, string ModelName)
    {
        DataTable dtM = GetMaster(docno);
        DataRow drM = dtM.Rows[0];
        string ModifyFlag = drM["MODIFY_FLAG"].ToString();

        #region 聲明變量
        string MFILE_PATH = string.Empty;
        string MFILE_PATHDFX = string.Empty;
        string MFILE_PATHCTQ = string.Empty;
        string MFILE_PATHIssue = string.Empty;
        string MFILE_PATHFMEA = string.Empty;
        string Model = drM["MODEL_NAME"].ToString();
        string Stage = drM["SUB_DOC_PHASE"].ToString();
        string typeHomePage = "Home Page";
        string typeDFX = "DFX Doc";
        string typeCTQ = "MFG CTQ Doc";
        string typeIssue = "Issue List Doc";
        string typeFMEA = "FMEA List Doc";
        string typeDFXSMT = "SMT DFX";
        string typeDFXAIRI = "AIRI DFX";
        string typeDFXIE = "IE DFX";
        string typeDFXEE = "EE DFX";
        string typeDFXTE = "TE DFX";
        string typeDFXAUTO = "AUTO DFX";
        string typeDFXSQ = "SQ DFX";
        string typeDFXUQ = "UQ DFX";
        string typeDFXDQE = "DQE DFX";
        string typeDFXSafety = "Safety DFX";
        #endregion

        #region 定義文件保存路徑

        #region Excel 原始檔路徑
        string FilePath_HomePage = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeHomePage + "/" + "Home Page.xlsx");
        string FilePath_DFX = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeDFX + "/" + "DFX Report.xlsx");
        string FilePath_CTQ = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeCTQ + "/" + "CTQ Report.xlsx");
        string FilePath_Issue = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeIssue + "/" + "Issue List Report.xlsx");
        string FilePath_FMEA = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeFMEA + "/" + "P-FMEA Report.xlsx");

        string FilePath_DFXSMT = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeDFX + "/" + typeDFXSMT + "/" + "SMT_DFX.xlsx");
        string FilePath_DFXAIRI = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeDFX + "/" + typeDFXAIRI + "/" + "AIRI_DFX.xlsx");
        string FilePath_DFXIE = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeDFX + "/" + typeDFXIE + "/" + "IE_DFX.xlsx");
        string FilePath_DFXEE = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeDFX + "/" + typeDFXEE + "/" + "EE_DFX.xlsx");
        string FilePath_DFXTE = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeDFX + "/" + typeDFXTE + "/" + "TE_DFX.xlsx");
        string FilePath_DFXAUTO = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeDFX + "/" + typeDFXAUTO + "/" + "AUTO_DFX.xlsx");
        string FilePath_DFXSQ = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeDFX + "/" + typeDFXSQ + "/" + "SQ_DFX.xlsx");
        string FilePath_DFXUQ = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeDFX + "/" + typeDFXUQ + "/" + "UQ_DFX.xlsx");
        string FilePath_DFXDQE = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeDFX + "/" + typeDFXDQE + "/" + "DQE_DFX.xlsx");
        string FilePath_DFXSafety = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeDFX + "/" + typeDFXSafety + "/" + "Safety_DFX.xlsx");
        #endregion


        #endregion

        #region 創建Excel PDF方法

        try
        {
            if (ModifyFlag == "N")
            {
                CreateExcel_HomePage(FilePath_HomePage, docno, caseID, Bu, Building);//HomePage
                CreateExcel_CTQ(FilePath_CTQ, docno, caseID, Bu, Building);//CTQ

                CreateExcel_DFX(FilePath_DFX, docno, caseID, Bu, Building, ModelName);//DFX
                CreateExcel_Issue(FilePath_Issue, docno, caseID, Bu, Building);//Issue List
                CreateExcel_FMEA(FilePath_FMEA, docno, caseID, Bu, Building);//FMEA


            }
            else
            {
                CreateExcel_HomePage_Modify(FilePath_HomePage, docno, caseID, Bu, Building);//HomePage
            }
        }
        catch (Exception ex)
        {
            Alert(ex.Message);
        }

        #endregion

    }

    #region Create HomePage Excel

    private void CreateExcel_HomePage(string fileName, string docno, string caseID, string Bu, string Building)
    {
        Aspose.Cells.License lic = new Aspose.Cells.License();
        string AsposeLicPath = System.Configuration.ConfigurationSettings.AppSettings["AsposeLicPath"].ToString();
        lic.SetLicense(AsposeLicPath);
        string templatePathHomePage = Page.Server.MapPath("~/Web/E-Report/NPITemplate/Home_Page.xlsx");
        string templatePathHomePageGZ = Page.Server.MapPath("~/Web/E-Report/NPITemplate/Home_Page_GZ.xlsx");

        if (Building == "A6")
        {
            Aspose.Cells.Workbook book = new Aspose.Cells.Workbook(templatePathHomePage);
            Aspose.Cells.Worksheet sheet0 = book.Worksheets[0];

            BindHomePage(ref sheet0, caseID, Bu, Building, docno);

            #region 檢查導出的Excel文件是否存在
            if (File.Exists(@fileName))//判斷當前路徑文件是否存在,若存在則先刪除在保存  Excel
            {
                File.Delete(@fileName);
                book.Save(fileName, Aspose.Cells.SaveFormat.Xlsx);
            }
            else
            {
                book.Save(fileName, Aspose.Cells.SaveFormat.Xlsx);
            }
            #endregion

            #region 生成壓縮檔后 刪除原始檔的文件夾
            DataTable dtM = GetMaster(docno);
            DataRow drM = dtM.Rows[0];
            string Model = drM["MODEL_NAME"].ToString();
            string Stage = drM["SUB_DOC_PHASE"].ToString();
            string FolderPath = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + "Home Page");
            try
            {
                //Compressedfile(docno, "Home Page");      //壓縮指定文件夾
            }
            catch (Exception ex)
            {

            }
            #endregion
        }
        else if(Building == "GZ")
        {
            Aspose.Cells.Workbook book = new Aspose.Cells.Workbook(templatePathHomePageGZ);
            Aspose.Cells.Worksheet sheet0 = book.Worksheets[0];

            BindHomePage(ref sheet0, caseID, Bu, Building, docno);

            #region 檢查導出的Excel文件是否存在
            if (File.Exists(@fileName))//判斷當前路徑文件是否存在,若存在則先刪除在保存  Excel
            {
                File.Delete(@fileName);
                book.Save(fileName, Aspose.Cells.SaveFormat.Xlsx);
            }
            else
            {
                book.Save(fileName, Aspose.Cells.SaveFormat.Xlsx);
            }
            #endregion

            #region 生成壓縮檔后 刪除原始檔的文件夾
            DataTable dtM = GetMaster(docno);
            DataRow drM = dtM.Rows[0];
            string Model = drM["MODEL_NAME"].ToString();
            string Stage = drM["SUB_DOC_PHASE"].ToString();
            string FolderPath = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + "Home Page");
            try
            {
                //Compressedfile(docno, "Home Page");      //壓縮指定文件夾
            }
            catch (Exception ex)
            {

            }
            #endregion
        }

        string strLog = string.Format("Date:{0} HomePage has create ok!", System.DateTime.Now.ToString("yyyy-MM-dd"));
        WriteLog(strLog);
    }

    private void BindHomePage(ref Aspose.Cells.Worksheet sheet, string caseID, string Bu, string Building, string DocNo)
    {
        //page 格式設定
        SetStyle(ref sheet, Aspose.Cells.PageOrientationType.Landscape);
        Aspose.Cells.Cells cells = sheet.Cells;
        Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
        Aspose.Cells.Style style = wb.Styles[wb.Styles.Add()];
        //string logoPath = Page.Server.MapPath("") + "\\log.png";
        //sheet.Pictures.Add(0, 0, 4, 10, logoPath);

        NPIMgmt oMgmt = new NPIMgmt("CZ", Bu);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();

        #region 填充基本及資料

        DataTable dtMaster = GetMaster(DocNo);//基本資料
        DataRow drM = dtMaster.Rows[0];
        string Model = drM["MODEL_NAME"].ToString();
        string Stage = drM["SUB_DOC_PHASE"].ToString();
        string DOC_NO = drM["DOC_NO"].ToString();
        #region 所有Check關卡人員 ToList
        DataTable dtTo = GetCheck_Person(drM["DOC_NO"].ToString(), "A");
        int len = dtTo.Rows.Count;
        string[] str = new string[len];
        for (int i = 0; i < len; i++)
        {
            str[i] = dtTo.Rows[i]["CheckedEName"].ToString();
        }
        string ToList = string.Join(",", str);
        sheet.Replace("{ToList}", ToList);
        #endregion

        #region Get NPI Leader/Top Manager CCList
        DataTable dtCC = GetCheck_Person(drM["DOC_NO"].ToString(), "A", "B");
        int len1 = dtCC.Rows.Count;
        string[] str1 = new string[len1];
        for (int i = 0; i < len1; i++)
        {
            str1[i] = dtCC.Rows[i]["CheckedEName"].ToString();
        }
        string CCList = string.Join(",", str1);
        sheet.Replace("{CCList}", CCList);
        #endregion

        #region GetCTQ %
        if (Building == "A6")
        {
            string Initial = "INT-FPY (附:Test Plan附件)";
            string PT = "PT-FPY";
            string BI = "BI-FPY (附:Waveform附件)";
            string HI = "H/P-FPY";
            string PF = "PF-FPY (附:OATL FA/CA附件)";
            string ATE = "ATE-FPY";

            DataTable dtInitial = GetCTQPercent(DocNo, Initial);
            DataTable dtPT = GetCTQPercent(DocNo, PT);
            DataTable dtBI = GetCTQPercent(DocNo, BI);
            DataTable dtHI = GetCTQPercent(DocNo, HI);
            DataTable dtPF = GetCTQPercent(DocNo, PF);
            DataTable dtATE = GetCTQPercent(DocNo, ATE);
            //== "" ? "0" : dtInitial.Rows[0]["ACT"].ToString()
            double InitialS = double.Parse(dtInitial.Rows[0]["ACT"].ToString()) / 100;
            double PTS = double.Parse(dtPT.Rows[0]["ACT"].ToString()) / 100;
            double BIS = double.Parse(dtBI.Rows[0]["ACT"].ToString()) / 100;
            double HIS = double.Parse(dtHI.Rows[0]["ACT"].ToString()) / 100;
            double PFS = double.Parse(dtPF.Rows[0]["ACT"].ToString()) / 100;
            double ATES = double.Parse(dtATE.Rows[0]["ACT"].ToString()) / 100;

            double CTQScore = InitialS * PTS * BIS * HIS * PFS * ATES;
            cells[12, 7].PutValue(CTQScore);
            #region 如果沒有該CTQ項目 則為NA
            if (dtInitial.Rows.Count > 0)
            {
                cells[12, 1].PutValue(double.Parse(dtInitial.Rows[0]["ACT"].ToString()) / 100);
            }
            else
            {
                cells[12, 1].PutValue("NA");
            }
            if (dtPT.Rows.Count > 0)
            {
                cells[12, 2].PutValue(double.Parse(dtPT.Rows[0]["ACT"].ToString()) / 100);
            }
            else
            {
                cells[12, 2].PutValue("NA");
            }
            if (dtBI.Rows.Count > 0)
            {
                cells[12, 3].PutValue(double.Parse(dtBI.Rows[0]["ACT"].ToString()) / 100);
            }
            else
            {
                cells[12, 3].PutValue("NA");
            }

            if (dtHI.Rows.Count > 0)
            {
                cells[12, 4].PutValue(double.Parse(dtHI.Rows[0]["ACT"].ToString()) / 100);
            }
            else
            {
                cells[12, 4].PutValue("NA");
            }
            if (dtPF.Rows.Count > 0)
            {
                cells[12, 5].PutValue(double.Parse(dtPF.Rows[0]["ACT"].ToString()) / 100);
            }
            else
            {
                cells[12, 5].PutValue("NA");
            }
            if (dtATE.Rows.Count > 0)
            {
                cells[12, 6].PutValue(double.Parse(dtATE.Rows[0]["ACT"].ToString()) / 100);
            }
            else
            {
                cells[12, 6].PutValue("NA");
            }


            #endregion

        }
        else if(Building == "GZ")
        {
            string Initial = "INT-FPY (附:Test Plan附件)";
            string PT = "PT-FPY";
            string BI = "BI-FPY (附:Waveform附件)";
            string HI = "H/P-FPY";
            string PF = "PF-FPY (附:OATL FA/CA附件)";
            string ATE = "ATE-FPY";

            DataTable dtInitial = GetCTQPercent(DocNo, Initial);
            DataTable dtPT = GetCTQPercent(DocNo, PT);
            DataTable dtBI = GetCTQPercent(DocNo, BI);
            DataTable dtHI = GetCTQPercent(DocNo, HI);
            DataTable dtPF = GetCTQPercent(DocNo, PF);
            DataTable dtATE = GetCTQPercent(DocNo, ATE);
            //== "" ? "0" : dtInitial.Rows[0]["ACT"].ToString()
            double InitialS = double.Parse(dtInitial.Rows[0]["ACT"].ToString()) / 100;
            double PTS = double.Parse(dtPT.Rows[0]["ACT"].ToString()) / 100;
            double BIS = double.Parse(dtBI.Rows[0]["ACT"].ToString()) / 100;
            double HIS = double.Parse(dtHI.Rows[0]["ACT"].ToString()) / 100;
            //double PFS = double.Parse(dtPF.Rows[0]["ACT"].ToString()) / 100;
            double ATES = double.Parse(dtATE.Rows[0]["ACT"].ToString()) / 100;

            double CTQScore = InitialS * PTS * BIS * HIS *  ATES;
            cells[12, 7].PutValue(CTQScore);
            #region 如果沒有該CTQ項目 則為NA
            if (dtInitial.Rows.Count > 0)
            {
                cells[12, 1].PutValue(double.Parse(dtInitial.Rows[0]["ACT"].ToString()) / 100);
            }
            else
            {
                cells[12, 1].PutValue("NA");
            }
            if (dtPT.Rows.Count > 0)
            {
                cells[12, 2].PutValue(double.Parse(dtPT.Rows[0]["ACT"].ToString()) / 100);
            }
            else
            {
                cells[12, 2].PutValue("NA");
            }
            if (dtBI.Rows.Count > 0)
            {
                cells[12, 3].PutValue(double.Parse(dtBI.Rows[0]["ACT"].ToString()) / 100);
            }
            else
            {
                cells[12, 3].PutValue("NA");
            }

            if (dtHI.Rows.Count > 0)
            {
                cells[12, 4].PutValue(double.Parse(dtHI.Rows[0]["ACT"].ToString()) / 100);
            }
            else
            {
                cells[12, 4].PutValue("NA");
            }
            if (dtPF.Rows.Count > 0)
            {
                cells[12, 5].PutValue(double.Parse(dtPF.Rows[0]["ACT"].ToString()) / 100);
            }
            else
            {
                cells[12, 5].PutValue("NA");
            }
            if (dtATE.Rows.Count > 0)
            {
                cells[12, 6].PutValue(double.Parse(dtATE.Rows[0]["ACT"].ToString()) / 100);
            }
            else
            {
                cells[12, 6].PutValue("NA");
            }


            #endregion

        }
        #endregion

        sheet.Replace("{BU}", drM["BU"].ToString() + "-" + drM["BUILDING"].ToString());
        sheet.Replace("{NPI_PM}", drM["NPI_PM"].ToString());
        sheet.Replace("{MODEL_NAME}", drM["MODEL_NAME"].ToString());
        sheet.Replace("{WorkOrder}", drM["WorkOrder"].ToString());
        sheet.Replace("{CUSTOMER}", drM["CUSTOMER"].ToString());
        sheet.Replace("{SUB_DOC_PHASE}", drM["SUB_DOC_PHASE"].ToString());
        sheet.Replace("{LINE}", drM["LINE"].ToString());
        sheet.Replace("{LOT_QTY}", drM["LOT_QTY"].ToString());
        sheet.Replace("{PCB_REV}", drM["PCB_REV"].ToString());
        sheet.Replace("{BOM_REV}", drM["BOM_REV"].ToString());
        sheet.Replace("{SPEC_REV}", drM["SPEC_REV"].ToString());
        sheet.Replace("{CUSTOMER_REV}", drM["CUSTOMER_REV"].ToString());
        sheet.Replace("{SUB_DOC_NO}", drM["SUB_DOC_NO"].ToString());

        sheet.Replace("{UPDATE_TIME}", drM["UPDATE_TIME"].ToString());
        sheet.Replace("{ISSUE_DATE}", drM["ISSUE_DATE"].ToString());
        sheet.Replace("{INPUT_DATE}", drM["INPUT_DATE"].ToString());
        sheet.Replace("{PK_DATE}", drM["PK_DATE"].ToString());

        #endregion

        #region 所有部門的DFX的Score

        #region 獲取DFX各個部門項目權重的N項
        #region AI_RI
        DataTable dtAIRILevel0 = GetDFXLevel(DocNo, "AI_RI", "0");
        DataTable dtAIRILevel1 = GetDFXLevel(DocNo, "AI_RI", "1");
        DataTable dtAIRILevel2 = GetDFXLevel(DocNo, "AI_RI", "2");
        DataTable dtAIRILevel3 = GetDFXLevel(DocNo, "AI_RI", "3");
        string AIRI0 = dtAIRILevel0.Rows[0]["amount"].ToString() == "" ? "0" : dtAIRILevel0.Rows[0]["amount"].ToString();
        string AIRI1 = dtAIRILevel1.Rows[0]["amount"].ToString() == "" ? "0" : dtAIRILevel1.Rows[0]["amount"].ToString();
        string AIRI2 = dtAIRILevel2.Rows[0]["amount"].ToString() == "" ? "0" : dtAIRILevel2.Rows[0]["amount"].ToString();
        string AIRI3 = dtAIRILevel3.Rows[0]["amount"].ToString() == "" ? "0" : dtAIRILevel3.Rows[0]["amount"].ToString();
        sheet.Replace("{AIRI0}", AIRI0);
        sheet.Replace("{AIRI1}", AIRI1);
        sheet.Replace("{AIRI2}", AIRI2);
        sheet.Replace("{AIRI3}", AIRI3);
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
        sheet.Replace("{SMT0}", SMT0);
        sheet.Replace("{SMT1}", SMT1);
        sheet.Replace("{SMT2}", SMT2);
        sheet.Replace("{SMT3}", SMT3);
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
        sheet.Replace("{IE0}", IE0);
        sheet.Replace("{IE1}", IE1);
        sheet.Replace("{IE2}", IE2);
        sheet.Replace("{IE3}", IE3);
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
        sheet.Replace("{DQE0}", DQE0);
        sheet.Replace("{DQE1}", DQE1);
        sheet.Replace("{DQE2}", DQE2);
        sheet.Replace("{DQE3}", DQE3);
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
        sheet.Replace("{EE0}", EE0);
        sheet.Replace("{EE1}", EE1);
        sheet.Replace("{EE2}", EE2);
        sheet.Replace("{EE3}", EE3);
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
        sheet.Replace("{UQ0}", UQ0);
        sheet.Replace("{UQ1}", UQ1);
        sheet.Replace("{UQ2}", UQ2);
        sheet.Replace("{UQ3}", UQ3);
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
        sheet.Replace("{SQ0}", SQ0);
        sheet.Replace("{SQ1}", SQ1);
        sheet.Replace("{SQ2}", SQ2);
        sheet.Replace("{SQ3}", SQ3);

        #endregion

        #endregion

        #region EVT階段 DFX統計  >=80
        if (drM["SUB_DOC_PHASE"].ToString().Contains("EVT"))
        {
            int LevelEVT = 80;
            #region

            #region AIRI
            DataTable dtAIRIS = GetDFXScore(DocNo, "AI_RI");//計算AI_RI的分數
            if (dtAIRIS.Rows[0]["MaxPoints"].ToString() == "")
            {
                cells[15, 2].PutValue(0);
            }
            else
            {
                double AIRIS = (double.Parse(dtAIRIS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtAIRIS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtAIRIS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtAIRIS.Rows[0]["MaxPoints"].ToString())) * 100;
                cells[15, 2].PutValue(AIRIS);
                if (AIRIS >= LevelEVT && AIRI0 == "0")
                {
                    string ResultAIRIP = "PASS";
                    sheet.Replace("{ResultAIRI}", ResultAIRIP);
                    sheet.Replace("{AIRIRemark}", "");
                }
                else if (AIRIS == 0)
                {
                    sheet.Replace("{ResultAIRI}", "NA");
                    sheet.Replace("{AIRIRemark}", "");
                }

                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[15, 7].SetStyle(style);
                    string ResultAIRIF = "FAIL";
                    sheet.Replace("{ResultAIRI}", ResultAIRIF);

                    DataTable dtAIRIF = GetDFXFail(DocNo, "AI_RI");
                    int lenAI = dtAIRIF.Rows.Count;
                    string[] strAI = new string[lenAI];
                    for (int i = 0; i < lenAI; i++)
                    {
                        strAI[i] = dtAIRIF.Rows[i]["Item"].ToString();
                    }
                    string AIRIFail = string.Join(",", strAI);
                    sheet.Replace("{AIRIRemark}", AIRIFail);
                }
            }
            #endregion

            #region SMT
            DataTable dtSMTS = GetDFXScore(DocNo, "SMT");
            if (dtSMTS.Rows[0]["MaxPoints"].ToString() == "")
            {
                cells[16, 2].PutValue(0);
            }
            else
            {
                double SMTS = (double.Parse(dtSMTS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtSMTS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtSMTS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtSMTS.Rows[0]["MaxPoints"].ToString())) * 100;
                cells[16, 2].PutValue(SMTS);
                //sheet.Replace("{SMTS}", SMTS);
                if (SMTS > LevelEVT && SMT0 == "0")
                {
                    string ResultSMTP = "PASS";
                    sheet.Replace("{ResultSMT}", ResultSMTP);
                    sheet.Replace("{SMTRemark}", "");
                }
                else if (SMTS == 0)
                {
                    sheet.Replace("{ResultSMT}", "NA");
                    sheet.Replace("{SMTRemark}", "");
                }

                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[16, 7].SetStyle(style);
                    string ResultSMTF = "FAIL";
                    sheet.Replace("{ResultSMT}", ResultSMTF);

                    DataTable dtSMTF = GetDFXFail(DocNo, "SMT");
                    int lenSMT = dtSMTF.Rows.Count;
                    string[] strSMT = new string[lenSMT];
                    for (int i = 0; i < lenSMT; i++)
                    {
                        strSMT[i] = dtSMTF.Rows[i]["Item"].ToString();
                    }
                    string SMTFail = string.Join(",", strSMT);
                    sheet.Replace("{SMTRemark}", SMTFail);
                }
            }
            #endregion

            #region IE
            DataTable dtIES = GetDFXScore(DocNo, "IE");
            if (dtIES.Rows[0]["MaxPoints"].ToString() == "")
            {
                cells[17, 2].PutValue(0);
            }
            else
            {
                double IES = (double.Parse(dtIES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtIES.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtIES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtIES.Rows[0]["MaxPoints"].ToString())) * 100;
                cells[17, 2].PutValue(IES);
                //sheet.Replace("{IES}", IES);
                if (IES > LevelEVT && IE0 == "0")
                {
                    string ResultIEP = "PASS";
                    sheet.Replace("{ResultIE}", ResultIEP);
                    sheet.Replace("{IERemark}", "");

                }
                else if (IES == 0)
                {
                    sheet.Replace("{ResultIE}", "NA");
                    sheet.Replace("{IERemark}", "");
                }

                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[17, 7].SetStyle(style);
                    string ResultIEF = "FAIL";
                    sheet.Replace("{ResultIE}", ResultIEF);

                    DataTable dtIEF = GetDFXFail(DocNo, "IE");
                    int lenIE = dtIEF.Rows.Count;
                    string[] strIE = new string[lenIE];
                    for (int i = 0; i < lenIE; i++)
                    {
                        strIE[i] = dtIEF.Rows[i]["Item"].ToString();
                    }
                    string IEFail = string.Join(",", strIE);
                    sheet.Replace("{IERemark}", IEFail);
                }
            }
            #endregion

            #region DQE
            DataTable dtDQES = GetDFXScore(DocNo, "DQE");
            if (dtDQES.Rows[0]["MaxPoints"].ToString() == "")
            {
                cells[18, 2].PutValue(0);
            }
            else
            {
                double DQES = (double.Parse(dtDQES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtDQES.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtDQES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtDQES.Rows[0]["MaxPoints"].ToString())) * 100;
                cells[18, 2].PutValue(DQES);
                //sheet.Replace("{DQES}", DQES);
                if (DQES > LevelEVT && DQE0 == "0")
                {
                    string ResultDQEP = "PASS";
                    sheet.Replace("{ResultDQE}", ResultDQEP);
                    sheet.Replace("{DQERemark}", "");
                }
                else if (DQES == 0)
                {
                    sheet.Replace("{ResultDQE}", "NA");
                    sheet.Replace("{DQERemark}", "");
                }

                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[18, 7].SetStyle(style);
                    string ResultDQEF = "FAIL";
                    sheet.Replace("{ResultDQE}", ResultDQEF);

                    DataTable dtDQEF = GetDFXFail(DocNo, "DQE");
                    int lenDQE = dtDQEF.Rows.Count;
                    string[] strDQE = new string[lenDQE];
                    for (int i = 0; i < lenDQE; i++)
                    {
                        strDQE[i] = dtDQEF.Rows[i]["Item"].ToString();
                    }
                    string DQEFail = string.Join(",", strDQE);
                    sheet.Replace("{DQERemark}", DQEFail);
                }
            }

            #endregion

            #region EE
            DataTable dtEES = GetDFXScore(DocNo, "EE");
            if (dtEES.Rows[0]["MaxPoints"].ToString() == "")
            {
                cells[19, 2].PutValue(0);
            }
            else
            {
                double EES = (double.Parse(dtEES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtEES.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtEES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtEES.Rows[0]["MaxPoints"].ToString())) * 100;
                cells[19, 2].PutValue(EES);
                //sheet.Replace("{EES}", EES);
                if (EES > LevelEVT && EE0 == "0")
                {
                    string ResultEEP = "PASS";
                    sheet.Replace("{ResultEE}", ResultEEP);
                    sheet.Replace("{EERemark}", "");
                }
                else if (EES == 0)
                {
                    sheet.Replace("{ResultEE}", "NA");
                    sheet.Replace("{EERemark}", "");
                }
                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[19, 7].SetStyle(style);
                    string ResultEEF = "FAIL";
                    sheet.Replace("{ResultEE}", ResultEEF);

                    DataTable dtEEF = GetDFXFail(DocNo, "EE");
                    int lenEE = dtEEF.Rows.Count;
                    string[] strEE = new string[lenEE];
                    for (int i = 0; i < lenEE; i++)
                    {
                        strEE[i] = dtEEF.Rows[i]["Item"].ToString();
                    }
                    string EEFail = string.Join(",", strEE);
                    sheet.Replace("{EERemark}", EEFail);
                }
            }
            #endregion

            #region UQ
            DataTable dtUQS = GetDFXScore(DocNo, "UQ");
            if (dtUQS.Rows[0]["MaxPoints"].ToString() == "")
            {
                cells[20, 2].PutValue(0);
            }
            else
            {
                double UQS = (double.Parse(dtUQS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtUQS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtUQS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtUQS.Rows[0]["MaxPoints"].ToString())) * 100;
                cells[20, 2].PutValue(UQS);
                //sheet.Replace("{UQS}", UQS);
                if (UQS > LevelEVT && UQ0 == "0")
                {
                    string ResultUQP = "PASS";
                    sheet.Replace("{ResultUQ}", ResultUQP);
                    sheet.Replace("{UQRemark}", "");
                }
                else if (UQS == 0)
                {
                    sheet.Replace("{ResultUQ}", "NA");
                    sheet.Replace("{UQRemark}", "");
                }
                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[20, 7].SetStyle(style);
                    string ResultUQF = "FAIL";
                    sheet.Replace("{ResultUQ}", ResultUQF);

                    DataTable dtUQF = GetDFXFail(DocNo, "UQ");
                    int lenUQ = dtUQF.Rows.Count;
                    string[] strUQ = new string[lenUQ];
                    for (int i = 0; i < lenUQ; i++)
                    {
                        strUQ[i] = dtUQF.Rows[i]["Item"].ToString();
                    }
                    string UQFail = string.Join(",", strUQ);
                    sheet.Replace("{UQRemark}", UQFail);
                }
            }
            #endregion

            #region SQ
            DataTable dtSQS = GetDFXScore(DocNo, "SQ");
            if (dtSQS.Rows[0]["MaxPoints"].ToString() == "")
            {
                cells[21, 2].PutValue(0);
            }
            else
            {
                double SQS = (double.Parse(dtSQS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtSQS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtSQS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtSQS.Rows[0]["MaxPoints"].ToString())) * 100;
                cells[21, 2].PutValue(SQS);
                //sheet.Replace("{SQS}", SQS);
                if (SQS > LevelEVT && SQ0 == "0")
                {
                    string ResultSQP = "PASS";
                    sheet.Replace("{ResultSQ}", ResultSQP);
                    sheet.Replace("{SQRemark}", "");
                }
                else if (SQS == 0)
                {
                    sheet.Replace("{ResultSQ}", "NA");
                    sheet.Replace("{SQRemark}", "");
                }
                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[21, 7].SetStyle(style);
                    string ResultSQF = "FAIL";
                    sheet.Replace("{ResultSQ}", ResultSQF);

                    DataTable dtSQF = GetDFXFail(DocNo, "SQ");
                    int lenSQ = dtSQF.Rows.Count;
                    string[] strSQ = new string[lenSQ];
                    for (int i = 0; i < lenSQ; i++)
                    {
                        strSQ[i] = dtSQF.Rows[i]["Item"].ToString();
                    }
                    string SQFail = string.Join(",", strSQ);
                    sheet.Replace("{SQRemark}", SQFail);
                }
            }
            #endregion

            #endregion
        }
        #endregion

        #region DVT階段 DFX統計  >=90
        else if (drM["SUB_DOC_PHASE"].ToString().Contains("DVT") || drM["SUB_DOC_PHASE"].ToString().Contains("P.Run"))
        {
            int LevelDP = 90;
            #region

            #region AIRI
            DataTable dtAIRIS = GetDFXScore(DocNo, "AI_RI");//計算AI_RI的分數
            if (dtAIRIS.Rows[0]["MaxPoints"].ToString() == "")
            {
                cells[15, 2].PutValue(0);
            }
            else
            {
                double AIRIS = (double.Parse(dtAIRIS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtAIRIS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtAIRIS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtAIRIS.Rows[0]["MaxPoints"].ToString())) * 100;
                cells[15, 2].PutValue(AIRIS);
                if (AIRIS >= LevelDP && AIRI0 == "0" && AIRI1 == "0")
                {
                    string ResultAIRIP = "PASS";
                    sheet.Replace("{ResultAIRI}", ResultAIRIP);
                    sheet.Replace("{AIRIRemark}", "");
                }
                else if (AIRIS == 0)
                {
                    sheet.Replace("{ResultAIRI}", "NA");
                    sheet.Replace("{AIRIRemark}", "");
                }


                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[15, 7].SetStyle(style);
                    string ResultAIRIF = "FAIL";
                    sheet.Replace("{ResultAIRI}", ResultAIRIF);

                    DataTable dtAIRIF = GetDFXFail(DocNo, "AI_RI");
                    int lenAI = dtAIRIF.Rows.Count;
                    string[] strAI = new string[lenAI];
                    for (int i = 0; i < lenAI; i++)
                    {
                        strAI[i] = dtAIRIF.Rows[i]["Item"].ToString();
                    }
                    string AIRIFail = string.Join(",", strAI);
                    sheet.Replace("{AIRIRemark}", AIRIFail);
                }

            }
            #endregion

            #region SMT
            DataTable dtSMTS = GetDFXScore(DocNo, "SMT");
            if (dtSMTS.Rows[0]["MaxPoints"].ToString() == "")
            {
                cells[16, 2].PutValue(0);
            }
            else
            {
                double SMTS = (double.Parse(dtSMTS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtSMTS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtSMTS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtSMTS.Rows[0]["MaxPoints"].ToString())) * 100;
                cells[16, 2].PutValue(SMTS);
                //sheet.Replace("{SMTS}", SMTS);
                if (SMTS > LevelDP && SMT0 == "0" && SMT1 == "0")
                {
                    string ResultSMTP = "PASS";
                    sheet.Replace("{ResultSMT}", ResultSMTP);
                    sheet.Replace("{SMTRemark}", "");
                }
                else if (SMTS == 0)
                {
                    sheet.Replace("{ResultSMT}", "NA");
                    sheet.Replace("{SMTRemark}", "");
                }

                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[16, 7].SetStyle(style);
                    string ResultSMTF = "FAIL";
                    sheet.Replace("{ResultSMT}", ResultSMTF);

                    DataTable dtSMTF = GetDFXFail(DocNo, "SMT");
                    int lenSMT = dtSMTF.Rows.Count;
                    string[] strSMT = new string[lenSMT];
                    for (int i = 0; i < lenSMT; i++)
                    {
                        strSMT[i] = dtSMTF.Rows[i]["Item"].ToString();
                    }
                    string SMTFail = string.Join(",", strSMT);
                    sheet.Replace("{SMTRemark}", SMTFail);
                }
            }
            #endregion

            #region IE
            DataTable dtIES = GetDFXScore(DocNo, "IE");
            if (dtIES.Rows[0]["MaxPoints"].ToString() == "")
            {
                cells[17, 2].PutValue(0);
            }
            else
            {
                double IES = (double.Parse(dtIES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtIES.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtIES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtIES.Rows[0]["MaxPoints"].ToString())) * 100;
                cells[17, 2].PutValue(IES);
                //sheet.Replace("{IES}", IES);
                if (IES > LevelDP && IE0 == "0" && IE1 == "0")
                {
                    string ResultIEP = "PASS";
                    sheet.Replace("{ResultIE}", ResultIEP);
                    sheet.Replace("{IERemark}", "");

                }
                else if (IES == 0)
                {
                    sheet.Replace("{ResultIE}", "NA");
                    sheet.Replace("{IERemark}", "");
                }

                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[17, 7].SetStyle(style);
                    string ResultIEF = "FAIL";
                    sheet.Replace("{ResultIE}", ResultIEF);

                    DataTable dtIEF = GetDFXFail(DocNo, "IE");
                    int lenIE = dtIEF.Rows.Count;
                    string[] strIE = new string[lenIE];
                    for (int i = 0; i < lenIE; i++)
                    {
                        strIE[i] = dtIEF.Rows[i]["Item"].ToString();
                    }
                    string IEFail = string.Join(",", strIE);
                    sheet.Replace("{IERemark}", IEFail);
                }
            }
            #endregion

            #region DQE
            DataTable dtDQES = GetDFXScore(DocNo, "DQE");
            if (dtDQES.Rows[0]["MaxPoints"].ToString() == "")
            {
                cells[18, 2].PutValue(0);
            }
            else
            {
                double DQES = (double.Parse(dtDQES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtDQES.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtDQES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtDQES.Rows[0]["MaxPoints"].ToString())) * 100;
                cells[18, 2].PutValue(DQES);
                //sheet.Replace("{DQES}", DQES);
                if (DQES > LevelDP && DQE0 == "0" && DQE1 == "0")
                {
                    string ResultDQEP = "PASS";
                    sheet.Replace("{ResultDQE}", ResultDQEP);
                    sheet.Replace("{DQERemark}", "");
                }
                else if (DQES == 0)
                {
                    sheet.Replace("{ResultDQE}", "NA");
                    sheet.Replace("{DQERemark}", "");
                }

                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[18, 7].SetStyle(style);
                    string ResultDQEF = "FAIL";
                    sheet.Replace("{ResultDQE}", ResultDQEF);

                    DataTable dtDQEF = GetDFXFail(DocNo, "DQE");
                    int lenDQE = dtDQEF.Rows.Count;
                    string[] strDQE = new string[lenDQE];
                    for (int i = 0; i < lenDQE; i++)
                    {
                        strDQE[i] = dtDQEF.Rows[i]["Item"].ToString();
                    }
                    string DQEFail = string.Join(",", strDQE);
                    sheet.Replace("{DQERemark}", DQEFail);
                }
            }

            #endregion

            #region EE
            DataTable dtEES = GetDFXScore(DocNo, "EE");
            if (dtEES.Rows[0]["MaxPoints"].ToString() == "")
            {
                cells[19, 2].PutValue(0);
            }
            else
            {
                double EES = (double.Parse(dtEES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtEES.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtEES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtEES.Rows[0]["MaxPoints"].ToString())) * 100;
                cells[19, 2].PutValue(EES);
                //sheet.Replace("{EES}", EES);
                if (EES > LevelDP && EE0 == "0" && EE1 == "0")
                {
                    string ResultEEP = "PASS";
                    sheet.Replace("{ResultEE}", ResultEEP);
                    sheet.Replace("{EERemark}", "");
                }
                else if (EES == 0)
                {
                    sheet.Replace("{ResultEE}", "NA");
                    sheet.Replace("{EERemark}", "");
                }

                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[19, 7].SetStyle(style);
                    string ResultEEF = "FAIL";
                    sheet.Replace("{ResultEE}", ResultEEF);

                    DataTable dtEEF = GetDFXFail(DocNo, "EE");
                    int lenEE = dtEEF.Rows.Count;
                    string[] strEE = new string[lenEE];
                    for (int i = 0; i < lenEE; i++)
                    {
                        strEE[i] = dtEEF.Rows[i]["Item"].ToString();
                    }
                    string EEFail = string.Join(",", strEE);
                    sheet.Replace("{EERemark}", EEFail);
                }
            }
            #endregion

            #region UQ
            DataTable dtUQS = GetDFXScore(DocNo, "UQ");
            if (dtUQS.Rows[0]["MaxPoints"].ToString() == "")
            {
                cells[20, 2].PutValue(0);
            }
            else
            {
                double UQS = (double.Parse(dtUQS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtUQS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtUQS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtUQS.Rows[0]["MaxPoints"].ToString())) * 100;
                cells[20, 2].PutValue(UQS);
                //sheet.Replace("{UQS}", UQS);
                if (UQS > LevelDP && UQ0 == "0" && UQ1 == "0")
                {
                    string ResultUQP = "PASS";
                    sheet.Replace("{ResultUQ}", ResultUQP);
                    sheet.Replace("{UQRemark}", "");
                }
                else if (UQS == 0)
                {
                    sheet.Replace("{ResultUQ}", "NA");
                    sheet.Replace("{UQRemark}", "");
                }

                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[20, 7].SetStyle(style);
                    string ResultUQF = "FAIL";
                    sheet.Replace("{ResultUQ}", ResultUQF);

                    DataTable dtUQF = GetDFXFail(DocNo, "UQ");
                    int lenUQ = dtUQF.Rows.Count;
                    string[] strUQ = new string[lenUQ];
                    for (int i = 0; i < lenUQ; i++)
                    {
                        strUQ[i] = dtUQF.Rows[i]["Item"].ToString();
                    }
                    string UQFail = string.Join(",", strUQ);
                    sheet.Replace("{UQRemark}", UQFail);
                }
            }
            #endregion

            #region SQ
            DataTable dtSQS = GetDFXScore(DocNo, "SQ");
            if (dtSQS.Rows[0]["MaxPoints"].ToString() == "")
            {
                cells[21, 2].PutValue(0);
            }
            else
            {
                double SQS = (double.Parse(dtSQS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtSQS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtSQS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtSQS.Rows[0]["MaxPoints"].ToString())) * 100;
                cells[21, 2].PutValue(SQS);
                //sheet.Replace("{SQS}", SQS);
                if (SQS > LevelDP && SQ0 == "0" && SQ1 == "0")
                {
                    string ResultSQP = "PASS";
                    sheet.Replace("{ResultSQ}", ResultSQP);
                    sheet.Replace("{SQRemark}", "");
                }
                else if (SQS == 0)
                {
                    sheet.Replace("{ResultSQ}", "NA");
                    sheet.Replace("{SQRemark}", "");
                }

                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[21, 7].SetStyle(style);
                    string ResultSQF = "FAIL";
                    sheet.Replace("{ResultSQ}", ResultSQF);

                    DataTable dtSQF = GetDFXFail(DocNo, "SQ");
                    int lenSQ = dtSQF.Rows.Count;
                    string[] strSQ = new string[lenSQ];
                    for (int i = 0; i < lenSQ; i++)
                    {
                        strSQ[i] = dtSQF.Rows[i]["Item"].ToString();
                    }
                    string SQFail = string.Join(",", strSQ);
                    sheet.Replace("{SQRemark}", SQFail);
                }
            }
            #endregion

            #endregion
        }



        #endregion

        #endregion

        #region 獲取封面CTQ(所有)
        DataTable dtHomePage = GetCLCAInconformity(DocNo);
        if (dtHomePage.Rows.Count > 0)
        {
            int templateIndexDFX = 23;//模板row起始位置
            int insertIndexEnCounter = templateIndexDFX + 1;//new row起始位置

            cells.InsertRows(insertIndexEnCounter, dtHomePage.Rows.Count - 1);
            cells.CopyRows(cells, templateIndexDFX, insertIndexEnCounter, dtHomePage.Rows.Count - 1); //複製模板row格式至新行
            for (int i = 0; i < dtHomePage.Rows.Count; i++)
            {
                DataRow dr = dtHomePage.Rows[i];
                string FileName = dr["W_FILENAME"].ToString() == "" ? "None" : dr["W_FILENAME"].ToString();
                cells[i + templateIndexDFX, 0].PutValue(dr["DEPT"].ToString());
                cells[i + templateIndexDFX, 1].PutValue(dr["CTQ"].ToString());
                cells[i + templateIndexDFX, 2].PutValue(dr["GOALStr"].ToString());
                cells[i + templateIndexDFX, 3].PutValue(dr["ACTStr"].ToString() == "" ? "NA" : dr["ACTStr"].ToString());
                cells[i + templateIndexDFX, 4].PutValue(dr["RESULT"].ToString());

                //Fail時樣式變更
                if (dr["RESULT"].ToString() == "Fail")
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[i + templateIndexDFX, 4].SetStyle(style);
                }

                //附件超链接
                //if (dr["RESULT"].ToString() == "NA" || FileName == "None") 不清楚当时判断为什么加了个Result为NA的逻辑
                if (FileName == "None")
                {
                    cells[i + templateIndexDFX, 5].PutValue("");
                }
                else
                {
                    //DirectoryInfo theFolder = new DirectoryInfo(@"\\icm651\Attachment\");//源文件夹
                    string theFolder = "http://icm656/czweb/web/E-Report/Attachment/";
                    if (dr["DEPT"].ToString() == "EE" || dr["DEPT"].ToString() == "TE")
                    {
                        string destFileNameCTQ1 = theFolder + Model + "_NPI_DOC" + "/" + Stage + "/" + "Test CPK Doc" + "/" + dr["DEPT"].ToString() + " Doc" + "/" + FileName;
                        style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                        cells[i + templateIndexDFX, 5].PutValue(FileName);
                        cells[i + templateIndexDFX, 5].SetStyle(style);
                        sheet.Hyperlinks.Add(i + templateIndexDFX, 5, 1, 1, destFileNameCTQ1);
                        cells.Merge(i + templateIndexDFX, 5, 1, 3);
                    }
                    else
                    {
                        string destFileNameCTQ2 = theFolder + Model + "_NPI_DOC" + "/" + Stage + "/" + "MFG CTQ Doc" + "/" + dr["DEPT"].ToString().Replace(" ", "") + " Doc" + "/" + FileName;
                        style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                        cells[i + templateIndexDFX, 5].PutValue(FileName);
                        cells[i + templateIndexDFX, 5].SetStyle(style);
                        sheet.Hyperlinks.Add(i + templateIndexDFX, 5, 1, 1, destFileNameCTQ2);
                        cells.Merge(i + templateIndexDFX, 5, 1, 3);
                    }
                }

                //Status
                if (dr["RESULT"].ToString() == "Pass" && dr["IMPROVEMENT_STATUS"].ToString() == "")
                {
                    cells[i + templateIndexDFX, 8].PutValue("Close");
                }
                else if (dr["RESULT"].ToString() == "Fail" && dr["IMPROVEMENT_STATUS"].ToString() == "")
                {
                    cells[i + templateIndexDFX, 8].PutValue("Open");
                }
                else if (dr["RESULT"].ToString() == "NA" && dr["IMPROVEMENT_STATUS"].ToString() == "")
                {
                    cells[i + templateIndexDFX, 8].PutValue("Close");
                }
                else
                {
                    cells[i + templateIndexDFX, 8].PutValue(dr["IMPROVEMENT_STATUS"].ToString());
                }

            }
        }
        #endregion

        #region PR階段 需要的資料
        DataTable dtPR = GetPR(DOC_NO);//抓取上傳的附件資料
        DataTable dtPRResult = GetPRResult(DocNo);//抓取PR關卡的簽核資料
        if (dtPR.Rows.Count > 0)
        {
            int IndexCTQAll = dtHomePage.Rows.Count;//CTQ用到的行數

            int templateIndexDFX = 25 + IndexCTQAll;//模板row起始位置
            int insertIndexEnCounter = templateIndexDFX + 1;//new row起始位置

            cells.InsertRows(insertIndexEnCounter, dtPR.Rows.Count - 1);
            cells.CopyRows(cells, templateIndexDFX, insertIndexEnCounter, dtPR.Rows.Count - 1); //複製模板row格式至新行
            for (int i = 0; i < dtPR.Rows.Count; i++)
            {
                DataRow dr = dtPR.Rows[i];
                string FileName = dr["FILE_NAME"].ToString() == "" ? "None" : dr["FILE_NAME"].ToString();
                string theFolder = "http://icm656/czweb/web/E-Report/Attachment/";
                cells[i + templateIndexDFX, 0].PutValue(dr["DEPT"].ToString());

                string destFileNamePR = theFolder + Model + "_NPI_DOC" + "/" + Stage + "/" + "Test CPK Doc" + "/" + dr["DEPT"].ToString() + " Doc" + "/" + FileName;
                style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                cells[i + templateIndexDFX, 1].PutValue(FileName);
                cells[i + templateIndexDFX, 1].SetStyle(style);
                sheet.Hyperlinks.Add(i + templateIndexDFX, 1, 1, 1, destFileNamePR);

                cells[i + templateIndexDFX, 2].PutValue(dr["FILE_REMARK"].ToString());
                cells.Merge(i + templateIndexDFX, 2, 1, 7);

            }
        }
        #endregion

        #region 抓取Workflow中的關卡和簽核人

        #region PM 固定行
        DataTable dtPM = GetPMPerson(drM["DOC_NO"].ToString());
        DataRow drPM = dtPM.Rows[0];
        sheet.Replace("{NPI_PM}", drPM["NPI_PM"].ToString());
        sheet.Replace("{PM}", drPM["WriteEname"].ToString());
        sheet.Replace("{PM Approver1}", drPM["WriteEname"].ToString());
        sheet.Replace("{PM Approver2}", drPM["CheckedEName"].ToString());
        #endregion

        #region 其他部門簽核人

        DataTable dtWRC = GetWriteReplyChecked(drM["DOC_NO"].ToString(), DocNo);
        if (dtWRC.Rows.Count > 0)
        {
            int IndexCTQAll = dtHomePage.Rows.Count;//CTQ用到的行數
            int IndexPRAll = dtPR.Rows.Count;//PR用到的行數
            if (IndexPRAll == 0)
            {
                int templateIndex = 28 + IndexCTQAll + IndexPRAll;//模板row起始位置
                int insertIndexEnCounter = templateIndex + 1;//new row起始位置
                cells.InsertRows(insertIndexEnCounter, dtWRC.Rows.Count - 1);
                cells.CopyRows(cells, templateIndex, insertIndexEnCounter, dtWRC.Rows.Count - 1); //複製模板row格式至新行
                for (int i = 0; i < dtWRC.Rows.Count; i++)
                {
                    DataRow dr = dtWRC.Rows[i];
                    cells[i + templateIndex, 0].PutValue(dr["DEPT"].ToString());
                    cells[i + templateIndex, 1].PutValue(dr["WriteEname"].ToString());
                    cells[i + templateIndex, 2].PutValue(dr["DEPT"].ToString());
                    cells[i + templateIndex, 3].PutValue(dr["CheckedEName"].ToString());
                    cells[i + templateIndex, 4].PutValue(dr["DEPT"].ToString());
                    cells[i + templateIndex, 5].PutValue(dr["ReplyEName"].ToString());
                    cells[i + templateIndex, 6].PutValue(dr["DEPT"].ToString());
                    cells[i + templateIndex, 7].PutValue(dr["CheckedEName"].ToString() + ";" + Convert.ToDateTime(dr["APPROVER_DATE"].ToString()).ToString("yyyy/MM/dd hh:mm"));
                    cells[i + templateIndex, 8].PutValue(dr["APPROVER_RESULT"].ToString() + ";" + dr["APPROVER_OPINION"].ToString());

                }

            }
            else
            {
                int templateIndex = 27 + IndexCTQAll + IndexPRAll;//模板row起始位置
                int insertIndexEnCounter = templateIndex + 1;//new row起始位置
                cells.InsertRows(insertIndexEnCounter, dtWRC.Rows.Count - 1);
                cells.CopyRows(cells, templateIndex, insertIndexEnCounter, dtWRC.Rows.Count - 1); //複製模板row格式至新行
                for (int i = 0; i < dtWRC.Rows.Count; i++)
                {
                    DataRow dr = dtWRC.Rows[i];
                    cells[i + templateIndex, 0].PutValue(dr["DEPT"].ToString());
                    cells[i + templateIndex, 1].PutValue(dr["WriteEname"].ToString());
                    cells[i + templateIndex, 2].PutValue(dr["DEPT"].ToString());
                    cells[i + templateIndex, 3].PutValue(dr["CheckedEName"].ToString());
                    cells[i + templateIndex, 4].PutValue(dr["DEPT"].ToString());
                    cells[i + templateIndex, 5].PutValue(dr["ReplyEName"].ToString());
                    cells[i + templateIndex, 6].PutValue(dr["DEPT"].ToString());
                    cells[i + templateIndex, 7].PutValue(dr["CheckedEName"].ToString() + ";" + Convert.ToDateTime(dr["APPROVER_DATE"].ToString()).ToString("yyyy/MM/dd hh:mm"));
                    cells[i + templateIndex, 8].PutValue(dr["APPROVER_RESULT"].ToString() + ";" + dr["APPROVER_OPINION"].ToString());

                }

            }


        }


        #endregion

        #region NPI Leader/Top Manager

        DataTable dtNPI = GetLeader(DocNo, "NPI Leader");
        if (dtNPI.Rows.Count > 0)
        {
            DataRow drNPI = dtNPI.Rows[0];
            sheet.Replace("{NPI Leader}", drNPI["APPROVER"].ToString());
            sheet.Replace("{NPITIME}", Convert.ToDateTime(drNPI["APPROVER_DATE"].ToString()).ToString("yyyy/MM/dd"));
            sheet.Replace("{NPI Result}", drNPI["APPROVER_RESULT"].ToString());
            sheet.Replace("{NPI Suggestion}", drNPI["APPROVER_OPINION"].ToString());
        }
        else
        {
            sheet.Replace("{NPI Leader}", "");
            sheet.Replace("{NPITIME}", "");
            sheet.Replace("{NPI Result}", "");
            sheet.Replace("{NPI Suggestion}", "");
        }

        //Top Manager
        DataTable dtManager = GetLeader(DocNo, "TOP Manager");
        if (dtManager.Rows.Count > 0)
        {
            DataRow drManager = dtManager.Rows[0];
            sheet.Replace("{Manager Leader}", drManager["APPROVER"].ToString());
            sheet.Replace("{ManagerTIME}", Convert.ToDateTime(drManager["APPROVER_DATE"].ToString()).ToString("yyyy/MM/dd"));
            sheet.Replace("{Result}", drManager["APPROVER_RESULT"].ToString() + ";" + drManager["APPROVER_OPINION"].ToString());
            sheet.Replace("{Manager Suggestion}", drManager["APPROVER_OPINION"].ToString());
            sheet.Replace("{Manager Result}", drManager["APPROVER_RESULT"].ToString());
        }
        else
        {
            sheet.Replace("{Manager Leader}", "");
            sheet.Replace("{ManagerTIME}", "");
            sheet.Replace("{Manager Result}", "");
            sheet.Replace("{Result}", "");
            sheet.Replace("{Manager Suggestion}", "");
        }

        //QRA
        if (dtManager.Rows.Count > 1)
        {
            DataRow drManager = dtManager.Rows[1];
            sheet.Replace("{QRA Leader}", drManager["APPROVER"].ToString());
            sheet.Replace("{QRATIME}", Convert.ToDateTime(drManager["APPROVER_DATE"].ToString()).ToString("yyyy/MM/dd"));
            sheet.Replace("{QRA Result}", drManager["APPROVER_RESULT"].ToString());
            sheet.Replace("{QRA Suggestion}", drManager["APPROVER_OPINION"].ToString());
        }
        else
        {

            int IndexCTQAll = dtHomePage.Rows.Count;//CTQ用到的行數
            int IndexWRC = dtWRC.Rows.Count;//其他部門簽核人用到的行數
            int IndexPRAll = dtPR.Rows.Count;//PR用到的行數
            if (IndexPRAll == 0)
            {
                int templateIndex = 31 + IndexCTQAll + IndexWRC;
                int ROWQRA = templateIndex;
                cells.DeleteRow(ROWQRA);

            }
            else
            {
                int templateIndex = 31 + IndexCTQAll + IndexWRC;
                int ROWQRA = templateIndex;
                cells.DeleteRow(ROWQRA);

            }

        }
        #endregion

        #endregion
    }

    #endregion

    #region Create HomePage Excel (Modify)

    private void CreateExcel_HomePage_Modify(string fileName, string docno, string caseID, string Bu, string Building)
    {
        Aspose.Cells.License lic = new Aspose.Cells.License();
        string AsposeLicPath = System.Configuration.ConfigurationSettings.AppSettings["AsposeLicPath"].ToString();
        lic.SetLicense(AsposeLicPath);
        string templatePathHomePage = Page.Server.MapPath("~/Web/E-Report/NPITemplate/Home_Page_M.xlsx");

        Aspose.Cells.Workbook book = new Aspose.Cells.Workbook(templatePathHomePage);
        Aspose.Cells.Worksheet sheet0 = book.Worksheets[0];

        BindHomePage_Modify(ref sheet0, caseID, Bu, Building, docno);

        #region 檢查導出的Excel文件是否存在
        if (File.Exists(@fileName))//判斷當前路徑文件是否存在,若存在則先刪除在保存  Excel
        {
            File.Delete(@fileName);
            book.Save(fileName, Aspose.Cells.SaveFormat.Xlsx);
        }
        else
        {
            book.Save(fileName, Aspose.Cells.SaveFormat.Xlsx);
        }
        #endregion
        //Compressedfile(docno, "Home Page");//壓縮指定文件夾
        string strLog = string.Format("Date:{0} HomePage has create ok!", System.DateTime.Now.ToString("yyyy-MM-dd"));
        WriteLog(strLog);
    }

    private void BindHomePage_Modify(ref Aspose.Cells.Worksheet sheet, string caseID, string Bu, string Building, string DocNo)
    {
        //page 格式設定
        SetStyle(ref sheet, Aspose.Cells.PageOrientationType.Landscape);
        Aspose.Cells.Cells cells = sheet.Cells;
        Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
        Aspose.Cells.Style style = wb.Styles[wb.Styles.Add()];
        //string logoPath = Page.Server.MapPath("") + "\\log.png";
        //sheet.Pictures.Add(0, 0, 4, 10, logoPath);
        NPIMgmt oMgmt = new NPIMgmt("CZ", Bu);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();

        #region 填充基本及資料

        DataTable dtMaster = GetMaster(DocNo);//基本資料
        DataRow drM = dtMaster.Rows[0];
        string Model = drM["MODEL_NAME"].ToString();
        string Stage = drM["SUB_DOC_PHASE"].ToString();
        string DOC_NO = drM["DOC_NO"].ToString();
        #region 所有Check關卡人員 ToList
        DataTable dtTo = GetCheck_Person(drM["DOC_NO"].ToString(), "A");
        int len = dtTo.Rows.Count;
        string[] str = new string[len];
        for (int i = 0; i < len; i++)
        {
            str[i] = dtTo.Rows[i]["CheckedEName"].ToString();
        }
        string ToList = string.Join(",", str);
        sheet.Replace("{ToList}", ToList);
        #endregion

        #region Get NPI Leader/Top Manager CCList
        DataTable dtCC = GetCheck_Person(drM["DOC_NO"].ToString(), "A", "B");
        int len1 = dtCC.Rows.Count;
        string[] str1 = new string[len1];
        for (int i = 0; i < len1; i++)
        {
            str1[i] = dtCC.Rows[i]["CheckedEName"].ToString();
        }
        string CCList = string.Join(",", str1);
        sheet.Replace("{CCList}", CCList);
        #endregion


        sheet.Replace("{BU}", drM["BU"].ToString() + "-" + drM["BUILDING"].ToString());
        sheet.Replace("{NPI_PM}", drM["NPI_PM"].ToString());
        sheet.Replace("{MODEL_NAME}", drM["MODEL_NAME"].ToString());
        sheet.Replace("{WorkOrder}", drM["WorkOrder"].ToString());
        sheet.Replace("{CUSTOMER}", drM["CUSTOMER"].ToString());
        sheet.Replace("{SUB_DOC_PHASE}", drM["SUB_DOC_PHASE"].ToString());
        sheet.Replace("{LINE}", drM["LINE"].ToString());
        sheet.Replace("{LOT_QTY}", drM["LOT_QTY"].ToString());
        sheet.Replace("{PCB_REV}", drM["PCB_REV"].ToString());
        sheet.Replace("{BOM_REV}", drM["BOM_REV"].ToString());
        sheet.Replace("{SPEC_REV}", drM["SPEC_REV"].ToString());
        sheet.Replace("{CUSTOMER_REV}", drM["CUSTOMER_REV"].ToString());
        sheet.Replace("{SUB_DOC_NO}", drM["SUB_DOC_NO"].ToString());

        sheet.Replace("{UPDATE_TIME}", drM["UPDATE_TIME"].ToString());
        sheet.Replace("{ISSUE_DATE}", drM["ISSUE_DATE"].ToString());
        sheet.Replace("{INPUT_DATE}", drM["INPUT_DATE"].ToString());
        sheet.Replace("{PK_DATE}", drM["PK_DATE"].ToString());

        sheet.Replace("{FROMMODEL}", drM["FROMMODEL"].ToString());
        sheet.Replace("{REMARK}", drM["REMARK"].ToString());

        #region 母機種超鏈接
        string ModifyFileName = drM["FROMMODEL"].ToString() + "_HomePage.xlsx";
        string destFileName_M = drM["MODELLINK"].ToString();
        style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
        cells[3, 7].PutValue(ModifyFileName);
        cells[3, 7].SetStyle(style);
        sheet.Hyperlinks.Add(3, 7, 1, 1, destFileName_M);
        cells.Merge(3, 7, 1, 2);

        #endregion

        #endregion

        #region PR階段 需要的資料 Modify
        DataTable dtPR = GetPRModify(DOC_NO);//抓取上傳的附件資料以及簽核結果
        if (dtPR.Rows.Count > 0)
        {

            int templateIndexDFX = 12;//模板row起始位置
            int insertIndexEnCounter = templateIndexDFX + 1;//new row起始位置

            cells.InsertRows(insertIndexEnCounter, dtPR.Rows.Count - 1);
            cells.CopyRows(cells, templateIndexDFX, insertIndexEnCounter, dtPR.Rows.Count - 1); //複製模板row格式至新行
            for (int i = 0; i < dtPR.Rows.Count; i++)
            {
                DataRow dr = dtPR.Rows[i];
                string FileName = dr["FILE_NAME"].ToString() == "" ? "None" : dr["FILE_NAME"].ToString();
                string theFolder = "http://icm656/czweb/web/E-Report/Attachment/";
                cells[i + templateIndexDFX, 0].PutValue(dr["DEPT"].ToString());

                string destFileNamePR = theFolder + Model + "_NPI_DOC" + "/" + Stage + "/" + "Test CPK Doc" + "/" + dr["DEPT"].ToString() + " Doc" + "/" + FileName;
                style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                cells[i + templateIndexDFX, 1].PutValue(FileName);
                cells[i + templateIndexDFX, 1].SetStyle(style);
                sheet.Hyperlinks.Add(i + templateIndexDFX, 1, 1, 1, destFileNamePR);


                cells[i + templateIndexDFX, 2].PutValue(dr["FILE_REMARK"].ToString());
                cells.Merge(i + templateIndexDFX, 2, 1, 3);
                cells[i + templateIndexDFX, 5].PutValue(dr["APPROVER"].ToString());
                cells[i + templateIndexDFX, 6].PutValue(dr["APPROVER_RESULT"].ToString());
                cells[i + templateIndexDFX, 7].PutValue(dr["APPROVER_DATE"].ToString());
                cells[i + templateIndexDFX, 8].PutValue(dr["APPROVER_OPINION"].ToString());
            }
        }
        #endregion

        #region PM Approve上傳的母機種報告
        DataTable dtPMModify = GetPMModify(DOC_NO);
        if (dtPMModify.Rows.Count > 0)
        {
            int templateIndexDFX = 12 + dtPR.Rows.Count;//模板row起始位置
            int insertIndexEnCounter = templateIndexDFX + 1;//new row起始位置

            cells.InsertRows(insertIndexEnCounter, dtPMModify.Rows.Count - 1);
            //cells.CopyRows(cells, templateIndexDFX, insertIndexEnCounter, dtPMModify.Rows.Count - 1); //複製模板row格式至新行
            for (int i = 0; i < dtPMModify.Rows.Count; i++)
            {
                DataRow dr = dtPMModify.Rows[i];
                string FileName = dr["FILE_NAME"].ToString() == "" ? "None" : dr["FILE_NAME"].ToString();
                string theFolder = "http://icm656/czweb/web/E-Report/Attachment/";
                cells[i + templateIndexDFX, 0].PutValue("PM");

                string destFileNamePR = theFolder + Model + "_NPI_DOC" + "/" + Stage + "/" + "Test CPK Doc" + "/" + "PM" + " Doc" + "/" + FileName;
                style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                cells[i + templateIndexDFX, 1].PutValue(FileName);
                cells[i + templateIndexDFX, 1].SetStyle(style);
                sheet.Hyperlinks.Add(i + templateIndexDFX, 1, 1, 1, destFileNamePR);

                cells[i + templateIndexDFX, 5].PutValue(dr["CheckedEName"].ToString());
            }
        }

        #endregion

        #region 抓取Workflow中的關卡和簽核人

        #region NPI Leader/Top Manager

        DataTable dtNPI = GetLeader(DocNo, "NPI Leader");
        if (dtNPI.Rows.Count > 0)
        {
            DataRow drNPI = dtNPI.Rows[0];
            sheet.Replace("{NPI Leader}", drNPI["APPROVER"].ToString());
            sheet.Replace("{NPITIME}", Convert.ToDateTime(drNPI["APPROVER_DATE"].ToString()).ToString("yyyy/MM/dd"));
            sheet.Replace("{NPI Result}", drNPI["APPROVER_RESULT"].ToString());
            sheet.Replace("{NPI Suggestion}", drNPI["APPROVER_OPINION"].ToString());
        }
        else
        {
            sheet.Replace("{NPI Leader}", "");
            sheet.Replace("{NPITIME}", "");
            sheet.Replace("{NPI Result}", "");
            sheet.Replace("{NPI Suggestion}", "");
        }

        //Top Manager
        DataTable dtManager = GetLeader(DocNo, "TOP Manager");
        if (dtManager.Rows.Count > 0)
        {
            DataRow drManager = dtManager.Rows[0];
            sheet.Replace("{Manager Leader}", drManager["APPROVER"].ToString());
            sheet.Replace("{ManagerTIME}", Convert.ToDateTime(drManager["APPROVER_DATE"].ToString()).ToString("yyyy/MM/dd"));
            sheet.Replace("{Result}", drManager["APPROVER_RESULT"].ToString() + ";" + drManager["APPROVER_OPINION"].ToString());
            sheet.Replace("{Manager Suggestion}", drManager["APPROVER_OPINION"].ToString());
            sheet.Replace("{Manager Result}", drManager["APPROVER_RESULT"].ToString());
        }
        else
        {
            sheet.Replace("{Manager Leader}", "");
            sheet.Replace("{ManagerTIME}", "");
            sheet.Replace("{Manager Result}", "");
            sheet.Replace("{Result}", "");
            sheet.Replace("{Manager Suggestion}", "");
        }

        //QRA
        if (dtManager.Rows.Count > 1)
        {
            DataRow drManager = dtManager.Rows[1];
            sheet.Replace("{QRA Leader}", drManager["APPROVER"].ToString());
            sheet.Replace("{QRATIME}", Convert.ToDateTime(drManager["APPROVER_DATE"].ToString()).ToString("yyyy/MM/dd"));
            sheet.Replace("{QRA Result}", drManager["APPROVER_RESULT"].ToString());
            sheet.Replace("{QRA Suggestion}", drManager["APPROVER_OPINION"].ToString());
        }
        else
        {

            //int IndexPRAll = dtPR.Rows.Count;//PR用到的行數
            //int templateIndex = 11 + IndexPRAll;
            //int ROWQRA = templateIndex;
            //cells.DeleteRow(ROWQRA);
        }
        #endregion

        #endregion
    }

    #endregion

    #region Create DFX Excel

    private void CreateExcel_DFX(string fileName, string docno, string caseID, string Bu, string Building, string Model)
    {
        Aspose.Cells.License lic = new Aspose.Cells.License();
        string AsposeLicPath = System.Configuration.ConfigurationSettings.AppSettings["AsposeLicPath"].ToString();
        lic.SetLicense(AsposeLicPath);
        string templatePathDFX = Page.Server.MapPath("~/Web/E-Report/NPITemplate/DFX_ReportAll.xlsx");

        Aspose.Cells.Workbook book = new Aspose.Cells.Workbook(templatePathDFX);

        Aspose.Cells.Worksheet sheet0 = book.Worksheets[0]; //DFX DFX Stage Score Summary
        Aspose.Cells.Worksheet sheet1 = book.Worksheets[1]; //DFX HomePage
        Aspose.Cells.Worksheet sheet2 = book.Worksheets[2]; //DFX Issue Report
        Aspose.Cells.Worksheet sheet3 = book.Worksheets[3]; //DFX AIRI
        Aspose.Cells.Worksheet sheet4 = book.Worksheets[4]; //DFX AUTO
        Aspose.Cells.Worksheet sheet5 = book.Worksheets[5]; //DFX DQE
        Aspose.Cells.Worksheet sheet6 = book.Worksheets[6]; //DFX EE
        Aspose.Cells.Worksheet sheet7 = book.Worksheets[7]; //DFX IE
        Aspose.Cells.Worksheet sheet8 = book.Worksheets[8]; //DFX Safety
        Aspose.Cells.Worksheet sheet9 = book.Worksheets[9]; //DFX SMT
        Aspose.Cells.Worksheet sheet10 = book.Worksheets[10]; //DFX SQ
        Aspose.Cells.Worksheet sheet11 = book.Worksheets[11]; //DFX TE
        Aspose.Cells.Worksheet sheet12 = book.Worksheets[12]; //DFX UQ

        BindDFXStage(ref sheet0, caseID, Bu, Building, docno, Model);//DFX Stage Score Summary
        BindDFXHoemPage(ref sheet1, caseID, Bu, Building, docno);//DFX HomePage
        BindDFX(ref sheet2, caseID, Bu, Building, docno);//DFX Issue Report
        BindDFXAIRI(ref sheet3, caseID, Bu, Building, docno);
        BindDFXAUTO(ref sheet4, caseID, Bu, Building, docno);
        BindDFXDQE(ref sheet5, caseID, Bu, Building, docno);
        BindDFXEE(ref sheet6, caseID, Bu, Building, docno);
        BindDFXIE(ref sheet7, caseID, Bu, Building, docno);
        BindDFXSafety(ref sheet8, caseID, Bu, Building, docno);
        BindDFXSMT(ref sheet9, caseID, Bu, Building, docno);
        BindDFXSQ(ref sheet10, caseID, Bu, Building, docno);
        BindDFXTE(ref sheet11, caseID, Bu, Building, docno);
        BindDFXUQ(ref sheet12, caseID, Bu, Building, docno);

        #region 檢查導出的Excel文件是否存在
        if (File.Exists(@fileName))//判斷當前路徑文件是否存在,若存在則先刪除在保存  Excel
        {
            File.Delete(@fileName);
            book.Save(fileName, Aspose.Cells.SaveFormat.Xlsx);
        }
        else
        {
            book.Save(fileName, Aspose.Cells.SaveFormat.Xlsx);
        }
        #endregion


        string strLog = string.Format("Date:{0} DFX Report has create ok!", System.DateTime.Now.ToString("yyyy-MM-dd"));
        WriteLog(strLog);
    }

    private void BindDFX(ref Aspose.Cells.Worksheet sheet, string caseID, string Bu, string Building, string DocNo)
    {
        //page 格式設定
        SetStyle(ref sheet, Aspose.Cells.PageOrientationType.Landscape);
        Aspose.Cells.Cells cells = sheet.Cells;
        //string logoPath = Page.Server.MapPath("") + "\\log.png";
        //sheet.Pictures.Add(0, 0, 4, 10, logoPath);

        NPIMgmt oMgmt = new NPIMgmt("CZ", Bu);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();

        #region 填充基本及資料

        DataTable dtMaster = GetMaster(DocNo);//基本資料
        DataRow drM = dtMaster.Rows[0];

        DataTable dtDept = GetDept(drM["DOC_NO"].ToString());//獲取涉及到的部門 放到數組中
        int len = dtDept.Rows.Count;
        string[] str = new string[len];
        for (int i = 0; i < len; i++)
        {
            str[i] = dtDept.Rows[i]["DEPT"].ToString();
        }
        string TeamMember = string.Join(",", str);

        sheet.Replace("{MODEL_NAME}", drM["MODEL_NAME"].ToString());
        sheet.Replace("{CUSTOMER}", drM["CUSTOMER"].ToString());
        sheet.Replace("{SUB_DOC_PHASE}", drM["SUB_DOC_PHASE"].ToString());
        sheet.Replace("{LINE}", drM["LINE"].ToString());
        sheet.Replace("{LOT_QTY}", drM["LOT_QTY"].ToString());
        sheet.Replace("{PCB_REV}", drM["PCB_REV"].ToString());
        sheet.Replace("{BOM_REV}", drM["BOM_REV"].ToString());
        sheet.Replace("{SPEC_REV}", drM["SPEC_REV"].ToString());
        sheet.Replace("{CUSTOMER_REV}", drM["CUSTOMER_REV"].ToString());
        sheet.Replace("{SUB_DOC_NO}", drM["SUB_DOC_NO"].ToString());
        sheet.Replace("{Team Member}", TeamMember);

        #endregion

        #region 獲取主表資訊
        DataTable dtDFX = oStandard.GetDFXInconformity(DocNo, "", "Begin");

        if (dtDFX.Rows.Count > 0)
        {
            int templateIndexDFX = 5;//模板row起始位置
            int insertIndexEnCounter = templateIndexDFX + 1;//new row起始位置

            cells.InsertRows(insertIndexEnCounter, dtDFX.Rows.Count - 1);
            cells.CopyRows(cells, templateIndexDFX, insertIndexEnCounter, dtDFX.Rows.Count - 1); //複製模板row格式至新行

            for (int i = 0; i < dtDFX.Rows.Count; i++)
            {

                DataRow dr = dtDFX.Rows[i];
                cells[i + templateIndexDFX, 0].PutValue(dr["WriteDept"].ToString());
                cells[i + templateIndexDFX, 1].PutValue(dr["Item"].ToString());
                cells[i + templateIndexDFX, 2].PutValue(dr["ItemType"].ToString());
                cells[i + templateIndexDFX, 3].PutValue(dr["ItemName"].ToString());
                cells[i + templateIndexDFX, 4].PutValue(dr["Requirements"].ToString());
                cells[i + templateIndexDFX, 5].PutValue("");
                cells[i + templateIndexDFX, 6].PutValue(dr["Losses"].ToString());
                cells[i + templateIndexDFX, 7].PutValue(dr["Compliance"].ToString());
                cells[i + templateIndexDFX, 8].PutValue(dr["PriorityLevel"].ToString());
                cells[i + templateIndexDFX, 9].PutValue(dr["MaxPoints"].ToString());
                cells[i + templateIndexDFX, 10].PutValue(dr["DFXPoints"].ToString());
                cells[i + templateIndexDFX, 11].PutValue(dr["Location"].ToString());
                cells[i + templateIndexDFX, 12].PutValue(dr["Comments"].ToString());
                cells[i + templateIndexDFX, 13].PutValue(dr["Actions"].ToString());
                cells[i + templateIndexDFX, 14].PutValue(dr["CompletionDate"].ToString().Length > 0 ? Convert.ToDateTime(dr["CompletionDate"].ToString()).ToString("yyyy/MM/dd") : dr["CompletionDate"].ToString());
                cells[i + templateIndexDFX, 15].PutValue(dr["Tracking"].ToString());
                cells[i + templateIndexDFX, 16].PutValue(dr["Remark"].ToString());


            }


        }

        #endregion

        #region 填充DFX簽核人員
        DataTable dtPM = GetPMPerson(drM["DOC_NO"].ToString()); //PM
        DataTable dtNPI = GetLeader(DocNo, "NPI Leader"); //NPI Leader
        if (dtPM.Rows.Count > 0)
        {
            DataRow drPM = dtPM.Rows[0];
            sheet.Replace("{PM Write}", drPM["WriteEname"].ToString());
            sheet.Replace("{PM Check}", drPM["CheckedEName"].ToString() + ";" + Convert.ToDateTime(drPM["UPDATE_TIME"].ToString()).ToString("yyyy/MM/dd"));
        }
        if (dtNPI.Rows.Count > 0)
        {
            DataRow drNPI = dtNPI.Rows[0];
            sheet.Replace("{NPI Leader}", drNPI["APPROVER"].ToString() + ";" + Convert.ToDateTime(drNPI["APPROVER_DATE"].ToString()).ToString("yyyy/MM/dd"));
        }
        #endregion

    }

    private void BindDFXHoemPage(ref Aspose.Cells.Worksheet sheet, string caseID, string Bu, string Building, string DocNo)
    {
        //page 格式設定
        SetStyle(ref sheet, Aspose.Cells.PageOrientationType.Landscape);
        Aspose.Cells.Cells cells = sheet.Cells;
        Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
        Aspose.Cells.Style style = wb.Styles[wb.Styles.Add()];
        //string logoPath = Page.Server.MapPath("") + "\\log.png";
        //sheet.Pictures.Add(0, 0, 4, 10, logoPath);

        style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;//应用边界线 左边界线
        style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin; //应用边界线 右边界线
        style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;//应用边界线 上边界线
        style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;//应用边界线 下边界线

        NPIMgmt oMgmt = new NPIMgmt("CZ", Bu);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        DataTable dtMaster = GetMaster(DocNo);//基本資料
        DataRow drM = dtMaster.Rows[0];
        DataTable dtResult = DFXScoreMaster(DocNo);
        int count = dtResult.Rows.Count;
        int divisor = count * 1000;
        sheet.Replace("{STAGE}", drM["SUB_DOC_PHASE"].ToString());
        sheet.Replace("{SUB_DOC_NO}", drM["SUB_DOC_NO"].ToString());
        if (dtResult.Rows.Count > 0)
        {
            #region 獲取各個部門的Result
            DataTable dtAIRIR = DFXScoreMaster(DocNo, "AI_RI");
            DataTable dtSMTR = DFXScoreMaster(DocNo, "SMT");
            DataTable dtIER = DFXScoreMaster(DocNo, "IE");
            DataTable dtDQER = DFXScoreMaster(DocNo, "DQE");
            DataTable dtEER = DFXScoreMaster(DocNo, "EE");
            DataTable dtUQR = DFXScoreMaster(DocNo, "UQ");
            DataTable dtSQR = DFXScoreMaster(DocNo, "SQ");

            string AIRIResult = dtAIRIR.Rows[0]["Result"].ToString();
            string SMTResult = dtSMTR.Rows[0]["Result"].ToString();
            string IEResult = dtIER.Rows[0]["Result"].ToString();
            string DQEResult = dtDQER.Rows[0]["Result"].ToString();
            string EEResult = dtEER.Rows[0]["Result"].ToString();
            string UQResult = dtUQR.Rows[0]["Result"].ToString();
            string SQResult = dtSQR.Rows[0]["Result"].ToString();

            #region
            DataTable dtAIRILevel0 = GetDFXLevel(DocNo, "AI_RI", "0");
            DataTable dtAIRILevel1 = GetDFXLevel(DocNo, "AI_RI", "1");
            DataTable dtAIRILevel2 = GetDFXLevel(DocNo, "AI_RI", "2");
            DataTable dtAIRILevel3 = GetDFXLevel(DocNo, "AI_RI", "3");
            string AIRI0 = dtAIRILevel0.Rows[0]["amount"].ToString() == "" ? "0" : dtAIRILevel0.Rows[0]["amount"].ToString();
            string AIRI1 = dtAIRILevel1.Rows[0]["amount"].ToString() == "" ? "0" : dtAIRILevel1.Rows[0]["amount"].ToString();
            string AIRI2 = dtAIRILevel2.Rows[0]["amount"].ToString() == "" ? "0" : dtAIRILevel2.Rows[0]["amount"].ToString();
            string AIRI3 = dtAIRILevel3.Rows[0]["amount"].ToString() == "" ? "0" : dtAIRILevel3.Rows[0]["amount"].ToString();

            DataTable dtSMTLevel0 = GetDFXLevel(DocNo, "SMT", "0");
            DataTable dtSMTLevel1 = GetDFXLevel(DocNo, "SMT", "1");
            DataTable dtSMTLevel2 = GetDFXLevel(DocNo, "SMT", "2");
            DataTable dtSMTLevel3 = GetDFXLevel(DocNo, "SMT", "3");
            string SMT0 = dtSMTLevel0.Rows[0]["amount"].ToString() == "" ? "0" : dtSMTLevel0.Rows[0]["amount"].ToString();
            string SMT1 = dtSMTLevel1.Rows[0]["amount"].ToString() == "" ? "0" : dtSMTLevel1.Rows[0]["amount"].ToString();
            string SMT2 = dtSMTLevel2.Rows[0]["amount"].ToString() == "" ? "0" : dtSMTLevel2.Rows[0]["amount"].ToString();
            string SMT3 = dtSMTLevel3.Rows[0]["amount"].ToString() == "" ? "0" : dtSMTLevel3.Rows[0]["amount"].ToString();

            DataTable dtIELevel0 = GetDFXLevel(DocNo, "IE", "0");
            DataTable dtIELevel1 = GetDFXLevel(DocNo, "IE", "1");
            DataTable dtIELevel2 = GetDFXLevel(DocNo, "IE", "2");
            DataTable dtIELevel3 = GetDFXLevel(DocNo, "IE", "3");
            string IE0 = dtIELevel0.Rows[0]["amount"].ToString() == "" ? "0" : dtIELevel0.Rows[0]["amount"].ToString();
            string IE1 = dtIELevel1.Rows[0]["amount"].ToString() == "" ? "0" : dtIELevel1.Rows[0]["amount"].ToString();
            string IE2 = dtIELevel2.Rows[0]["amount"].ToString() == "" ? "0" : dtIELevel2.Rows[0]["amount"].ToString();
            string IE3 = dtIELevel3.Rows[0]["amount"].ToString() == "" ? "0" : dtIELevel3.Rows[0]["amount"].ToString();

            DataTable dtDQELevel0 = GetDFXLevel(DocNo, "DQE", "0");
            DataTable dtDQELevel1 = GetDFXLevel(DocNo, "DQE", "1");
            DataTable dtDQELevel2 = GetDFXLevel(DocNo, "DQE", "2");
            DataTable dtDQELevel3 = GetDFXLevel(DocNo, "DQE", "3");
            string DQE0 = dtDQELevel0.Rows[0]["amount"].ToString() == "" ? "0" : dtDQELevel0.Rows[0]["amount"].ToString();
            string DQE1 = dtDQELevel1.Rows[0]["amount"].ToString() == "" ? "0" : dtDQELevel1.Rows[0]["amount"].ToString();
            string DQE2 = dtDQELevel2.Rows[0]["amount"].ToString() == "" ? "0" : dtDQELevel2.Rows[0]["amount"].ToString();
            string DQE3 = dtDQELevel3.Rows[0]["amount"].ToString() == "" ? "0" : dtDQELevel3.Rows[0]["amount"].ToString();

            DataTable dtEELevel0 = GetDFXLevel(DocNo, "EE", "0");
            DataTable dtEELevel1 = GetDFXLevel(DocNo, "EE", "1");
            DataTable dtEELevel2 = GetDFXLevel(DocNo, "EE", "2");
            DataTable dtEELevel3 = GetDFXLevel(DocNo, "EE", "3");
            string EE0 = dtEELevel0.Rows[0]["amount"].ToString() == "" ? "0" : dtEELevel0.Rows[0]["amount"].ToString();
            string EE1 = dtEELevel1.Rows[0]["amount"].ToString() == "" ? "0" : dtEELevel1.Rows[0]["amount"].ToString();
            string EE2 = dtEELevel2.Rows[0]["amount"].ToString() == "" ? "0" : dtEELevel2.Rows[0]["amount"].ToString();
            string EE3 = dtEELevel3.Rows[0]["amount"].ToString() == "" ? "0" : dtEELevel3.Rows[0]["amount"].ToString();

            DataTable dtUQLevel0 = GetDFXLevel(DocNo, "UQ", "0");
            DataTable dtUQLevel1 = GetDFXLevel(DocNo, "UQ", "1");
            DataTable dtUQLevel2 = GetDFXLevel(DocNo, "UQ", "2");
            DataTable dtUQLevel3 = GetDFXLevel(DocNo, "UQ", "3");
            string UQ0 = dtUQLevel0.Rows[0]["amount"].ToString() == "" ? "0" : dtUQLevel0.Rows[0]["amount"].ToString();
            string UQ1 = dtUQLevel1.Rows[0]["amount"].ToString() == "" ? "0" : dtUQLevel1.Rows[0]["amount"].ToString();
            string UQ2 = dtUQLevel2.Rows[0]["amount"].ToString() == "" ? "0" : dtUQLevel2.Rows[0]["amount"].ToString();
            string UQ3 = dtUQLevel3.Rows[0]["amount"].ToString() == "" ? "0" : dtUQLevel3.Rows[0]["amount"].ToString();

            DataTable dtSQLevel0 = GetDFXLevel(DocNo, "SQ", "0");
            DataTable dtSQLevel1 = GetDFXLevel(DocNo, "SQ", "1");
            DataTable dtSQLevel2 = GetDFXLevel(DocNo, "SQ", "2");
            DataTable dtSQLevel3 = GetDFXLevel(DocNo, "SQ", "3");
            string SQ0 = dtSQLevel0.Rows[0]["amount"].ToString() == "" ? "0" : dtSQLevel0.Rows[0]["amount"].ToString();
            string SQ1 = dtSQLevel1.Rows[0]["amount"].ToString() == "" ? "0" : dtSQLevel1.Rows[0]["amount"].ToString();
            string SQ2 = dtSQLevel2.Rows[0]["amount"].ToString() == "" ? "0" : dtSQLevel2.Rows[0]["amount"].ToString();
            string SQ3 = dtSQLevel3.Rows[0]["amount"].ToString() == "" ? "0" : dtSQLevel3.Rows[0]["amount"].ToString();

            #endregion

            #region 获取DFX各個部門項目權重的Open項
            #region AIRI
            if (dtAIRIR.Rows.Count > 0)
            {
                #region AI_RI
                DataTable dtAIRILevel0A = GetDFXLevel(DocNo, "AI_RI", "0");
                DataTable dtAIRILevel1A = GetDFXLevel(DocNo, "AI_RI", "1");
                DataTable dtAIRILevel2A = GetDFXLevel(DocNo, "AI_RI", "2");
                DataTable dtAIRILevel3A = GetDFXLevel(DocNo, "AI_RI", "3");
                string AIRI0A = dtAIRILevel0A.Rows[0]["amount"].ToString() == "" ? "0" : dtAIRILevel0A.Rows[0]["amount"].ToString();
                string AIRI1A = dtAIRILevel1A.Rows[0]["amount"].ToString() == "" ? "0" : dtAIRILevel1A.Rows[0]["amount"].ToString();
                string AIRI2A = dtAIRILevel2A.Rows[0]["amount"].ToString() == "" ? "0" : dtAIRILevel2A.Rows[0]["amount"].ToString();
                string AIRI3A = dtAIRILevel3A.Rows[0]["amount"].ToString() == "" ? "0" : dtAIRILevel3A.Rows[0]["amount"].ToString();
                sheet.Replace("{AIRI0}", AIRI0A);
                sheet.Replace("{AIRI1}", AIRI1A);
                sheet.Replace("{AIRI2}", AIRI2A);
                sheet.Replace("{AIRI3}", AIRI3A);
                #endregion
            }
            else
            {
                sheet.Replace("{AIRI0}", "");
                sheet.Replace("{AIRI1}", "");
                sheet.Replace("{AIRI2}", "");
                sheet.Replace("{AIRI3}", "");
            }
            #endregion

            #region SMT
            if (dtSMTR.Rows.Count > 0)
            {
                #region SMT
                DataTable dtSMTLevel0A = GetDFXLevel(DocNo, "SMT", "0");
                DataTable dtSMTLevel1A = GetDFXLevel(DocNo, "SMT", "1");
                DataTable dtSMTLevel2A = GetDFXLevel(DocNo, "SMT", "2");
                DataTable dtSMTLevel3A = GetDFXLevel(DocNo, "SMT", "3");
                string SMT0A = dtSMTLevel0A.Rows[0]["amount"].ToString() == "" ? "0" : dtSMTLevel0A.Rows[0]["amount"].ToString();
                string SMT1A = dtSMTLevel1A.Rows[0]["amount"].ToString() == "" ? "0" : dtSMTLevel1A.Rows[0]["amount"].ToString();
                string SMT2A = dtSMTLevel2A.Rows[0]["amount"].ToString() == "" ? "0" : dtSMTLevel2A.Rows[0]["amount"].ToString();
                string SMT3A = dtSMTLevel3A.Rows[0]["amount"].ToString() == "" ? "0" : dtSMTLevel3A.Rows[0]["amount"].ToString();
                sheet.Replace("{SMT0}", SMT0A);
                sheet.Replace("{SMT1}", SMT1A);
                sheet.Replace("{SMT2}", SMT2A);
                sheet.Replace("{SMT3}", SMT3A);
                #endregion

            }
            else
            {
                sheet.Replace("{SMT0}", "");
                sheet.Replace("{SMT1}", "");
                sheet.Replace("{SMT2}", "");
                sheet.Replace("{SMT3}", "");
            }
            #endregion

            #region IE
            if (dtIER.Rows.Count > 0)
            {
                #region IE
                DataTable dtIELevel0A = GetDFXLevel(DocNo, "IE", "0");
                DataTable dtIELevel1A = GetDFXLevel(DocNo, "IE", "1");
                DataTable dtIELevel2A = GetDFXLevel(DocNo, "IE", "2");
                DataTable dtIELevel3A = GetDFXLevel(DocNo, "IE", "3");
                string IE0A = dtIELevel0A.Rows[0]["amount"].ToString() == "" ? "0" : dtIELevel0A.Rows[0]["amount"].ToString();
                string IE1A = dtIELevel1A.Rows[0]["amount"].ToString() == "" ? "0" : dtIELevel1A.Rows[0]["amount"].ToString();
                string IE2A = dtIELevel2A.Rows[0]["amount"].ToString() == "" ? "0" : dtIELevel2A.Rows[0]["amount"].ToString();
                string IE3A = dtIELevel3A.Rows[0]["amount"].ToString() == "" ? "0" : dtIELevel3A.Rows[0]["amount"].ToString();
                sheet.Replace("{IE0}", IE0A);
                sheet.Replace("{IE1}", IE1A);
                sheet.Replace("{IE2}", IE2A);
                sheet.Replace("{IE3}", IE3A);
                #endregion
            }
            else
            {
                sheet.Replace("{IE0}", "");
                sheet.Replace("{IE1}", "");
                sheet.Replace("{IE2}", "");
                sheet.Replace("{IE3}", "");
            }
            #endregion

            #region DQE
            if (dtDQER.Rows.Count > 0)
            {
                #region DQE
                DataTable dtDQELevel0A = GetDFXLevel(DocNo, "DQE", "0");
                DataTable dtDQELevel1A = GetDFXLevel(DocNo, "DQE", "1");
                DataTable dtDQELevel2A = GetDFXLevel(DocNo, "DQE", "2");
                DataTable dtDQELevel3A = GetDFXLevel(DocNo, "DQE", "3");
                string DQE0A = dtDQELevel0A.Rows[0]["amount"].ToString() == "" ? "0" : dtDQELevel0A.Rows[0]["amount"].ToString();
                string DQE1A = dtDQELevel1A.Rows[0]["amount"].ToString() == "" ? "0" : dtDQELevel1A.Rows[0]["amount"].ToString();
                string DQE2A = dtDQELevel2A.Rows[0]["amount"].ToString() == "" ? "0" : dtDQELevel2A.Rows[0]["amount"].ToString();
                string DQE3A = dtDQELevel3A.Rows[0]["amount"].ToString() == "" ? "0" : dtDQELevel3A.Rows[0]["amount"].ToString();
                sheet.Replace("{DQE0}", DQE0A);
                sheet.Replace("{DQE1}", DQE1A);
                sheet.Replace("{DQE2}", DQE2A);
                sheet.Replace("{DQE3}", DQE3A);
                #endregion

            }
            else
            {
                sheet.Replace("{DQE0}", "");
                sheet.Replace("{DQE1}", "");
                sheet.Replace("{DQE2}", "");
                sheet.Replace("{DQE3}", "");
            }
            #endregion

            #region EE
            if (dtEER.Rows.Count > 0)
            {
                #region EE
                DataTable dtEELevel0A = GetDFXLevel(DocNo, "EE", "0");
                DataTable dtEELevel1A = GetDFXLevel(DocNo, "EE", "1");
                DataTable dtEELevel2A = GetDFXLevel(DocNo, "EE", "2");
                DataTable dtEELevel3A = GetDFXLevel(DocNo, "EE", "3");
                string EE0A = dtEELevel0A.Rows[0]["amount"].ToString() == "" ? "0" : dtEELevel0A.Rows[0]["amount"].ToString();
                string EE1A = dtEELevel1A.Rows[0]["amount"].ToString() == "" ? "0" : dtEELevel1A.Rows[0]["amount"].ToString();
                string EE2A = dtEELevel2A.Rows[0]["amount"].ToString() == "" ? "0" : dtEELevel2A.Rows[0]["amount"].ToString();
                string EE3A = dtEELevel3A.Rows[0]["amount"].ToString() == "" ? "0" : dtEELevel3A.Rows[0]["amount"].ToString();
                sheet.Replace("{EE0}", EE0A);
                sheet.Replace("{EE1}", EE1A);
                sheet.Replace("{EE2}", EE2A);
                sheet.Replace("{EE3}", EE3A);
                #endregion

            }
            else
            {
                sheet.Replace("{EE0}", "");
                sheet.Replace("{EE1}", "");
                sheet.Replace("{EE2}", "");
                sheet.Replace("{EE3}", "");
            }
            #endregion

            #region UQ
            if (dtUQR.Rows.Count > 0)
            {
                #region UQ
                DataTable dtUQLevel0A = GetDFXLevel(DocNo, "UQ", "0");
                DataTable dtUQLevel1A = GetDFXLevel(DocNo, "UQ", "1");
                DataTable dtUQLevel2A = GetDFXLevel(DocNo, "UQ", "2");
                DataTable dtUQLevel3A = GetDFXLevel(DocNo, "UQ", "3");
                string UQ0A = dtUQLevel0A.Rows[0]["amount"].ToString() == "" ? "0" : dtUQLevel0A.Rows[0]["amount"].ToString();
                string UQ1A = dtUQLevel1A.Rows[0]["amount"].ToString() == "" ? "0" : dtUQLevel1A.Rows[0]["amount"].ToString();
                string UQ2A = dtUQLevel2A.Rows[0]["amount"].ToString() == "" ? "0" : dtUQLevel2A.Rows[0]["amount"].ToString();
                string UQ3A = dtUQLevel3A.Rows[0]["amount"].ToString() == "" ? "0" : dtUQLevel3A.Rows[0]["amount"].ToString();
                sheet.Replace("{UQ0}", UQ0A);
                sheet.Replace("{UQ1}", UQ1A);
                sheet.Replace("{UQ2}", UQ2A);
                sheet.Replace("{UQ3}", UQ3A);
                #endregion
            }
            else
            {
                sheet.Replace("{UQ0}", "");
                sheet.Replace("{UQ1}", "");
                sheet.Replace("{UQ2}", "");
                sheet.Replace("{UQ3}", "");
            }
            #endregion

            #region SQ
            if (dtSQR.Rows.Count > 0)
            {
                #region SQ
                DataTable dtSQLevel0A = GetDFXLevel(DocNo, "SQ", "0");
                DataTable dtSQLevel1A = GetDFXLevel(DocNo, "SQ", "1");
                DataTable dtSQLevel2A = GetDFXLevel(DocNo, "SQ", "2");
                DataTable dtSQLevel3A = GetDFXLevel(DocNo, "SQ", "3");
                string SQ0A = dtSQLevel0A.Rows[0]["amount"].ToString() == "" ? "0" : dtSQLevel0A.Rows[0]["amount"].ToString();
                string SQ1A = dtSQLevel1A.Rows[0]["amount"].ToString() == "" ? "0" : dtSQLevel1A.Rows[0]["amount"].ToString();
                string SQ2A = dtSQLevel2A.Rows[0]["amount"].ToString() == "" ? "0" : dtSQLevel2A.Rows[0]["amount"].ToString();
                string SQ3A = dtSQLevel3A.Rows[0]["amount"].ToString() == "" ? "0" : dtSQLevel3A.Rows[0]["amount"].ToString();
                sheet.Replace("{SQ0}", SQ0A);
                sheet.Replace("{SQ1}", SQ1A);
                sheet.Replace("{SQ2}", SQ2A);
                sheet.Replace("{SQ3}", SQ3A);

                #endregion

            }
            else
            {
                sheet.Replace("{SQ0}", "");
                sheet.Replace("{SQ1}", "");
                sheet.Replace("{SQ2}", "");
                sheet.Replace("{SQ3}", "");
            }
            #endregion

            #endregion
            #endregion

            #region 所有部門的DFX的Score

            #region EVT階段 DFX統計  >=80
            if (drM["SUB_DOC_PHASE"].ToString().Contains("EVT"))
            {
                sheet.Replace("{Goal}", "80%");
                int LevelEVT = 80;

                #region AIRI
                DataTable dtAIRIS = GetDFXScore(DocNo, "AI_RI");//計算AI_RI的分數
                double AIRIS = (double.Parse(dtAIRIS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtAIRIS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtAIRIS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtAIRIS.Rows[0]["MaxPoints"].ToString())) * 100;
                cells[4, 2].PutValue(AIRIS.ToString("0.0") + "%");
                if (AIRIS >= LevelEVT && AIRI0 == "0")
                {
                    string ResultAIRIP = "PASS";
                    sheet.Replace("{ResultAIRI}", ResultAIRIP);
                    sheet.Replace("{AIRIRemark}", "");
                }
                else if (AIRIS == 0)
                {
                    sheet.Replace("{ResultAIRI}", "NA");
                    sheet.Replace("{AIRIRemark}", "");
                }
                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[4, 7].SetStyle(style);
                    string ResultAIRIF = "FAIL";
                    sheet.Replace("{ResultAIRI}", ResultAIRIF);

                    DataTable dtAIRIF = GetDFXFail(DocNo, "AI_RI");
                    int lenAI = dtAIRIF.Rows.Count;
                    string[] strAI = new string[lenAI];
                    for (int i = 0; i < lenAI; i++)
                    {
                        strAI[i] = dtAIRIF.Rows[i]["Item"].ToString();
                    }
                    string AIRIFail = string.Join(",", strAI);
                    sheet.Replace("{AIRIRemark}", AIRIFail);
                }
                #endregion

                #region SMT
                DataTable dtSMTS = GetDFXScore(DocNo, "SMT");
                double SMTS = (double.Parse(dtSMTS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtSMTS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtSMTS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtSMTS.Rows[0]["MaxPoints"].ToString())) * 100;
                cells[5, 2].PutValue(SMTS.ToString("0.0") + "%");
                //sheet.Replace("{SMTS}", SMTS);
                if (SMTS > LevelEVT && SMT0 == "0")
                {
                    string ResultSMTP = "PASS";
                    sheet.Replace("{ResultSMT}", ResultSMTP);
                    sheet.Replace("{SMTRemark}", "");
                }
                else if (SMTS == 0)
                {
                    sheet.Replace("{ResultSMT}", "NA");
                    sheet.Replace("{SMTRemark}", "");
                }
                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[5, 7].SetStyle(style);
                    string ResultSMTF = "FAIL";
                    sheet.Replace("{ResultSMT}", ResultSMTF);

                    DataTable dtSMTF = GetDFXFail(DocNo, "SMT");
                    int lenSMT = dtSMTF.Rows.Count;
                    string[] strSMT = new string[lenSMT];
                    for (int i = 0; i < lenSMT; i++)
                    {
                        strSMT[i] = dtSMTF.Rows[i]["Item"].ToString();
                    }
                    string SMTFail = string.Join(",", strSMT);
                    sheet.Replace("{SMTRemark}", SMTFail);
                }
                #endregion

                #region IE
                DataTable dtIES = GetDFXScore(DocNo, "IE");
                double IES = (double.Parse(dtIES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtIES.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtIES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtIES.Rows[0]["MaxPoints"].ToString())) * 100;
                cells[6, 2].PutValue(IES.ToString("0.0") + "%");
                //sheet.Replace("{IES}", IES);
                if (IES > LevelEVT && IE0 == "0")
                {
                    string ResultIEP = "PASS";
                    sheet.Replace("{ResultIE}", ResultIEP);
                    sheet.Replace("{IERemark}", "");

                }
                else if (IES == 0)
                {
                    sheet.Replace("{ResultIE}", "NA");
                    sheet.Replace("{IERemark}", "");
                }
                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[6, 7].SetStyle(style);
                    string ResultIEF = "FAIL";
                    sheet.Replace("{ResultIE}", ResultIEF);

                    DataTable dtIEF = GetDFXFail(DocNo, "IE");
                    int lenIE = dtIEF.Rows.Count;
                    string[] strIE = new string[lenIE];
                    for (int i = 0; i < lenIE; i++)
                    {
                        strIE[i] = dtIEF.Rows[i]["Item"].ToString();
                    }
                    string IEFail = string.Join(",", strIE);
                    sheet.Replace("{IERemark}", IEFail);
                }
                #endregion

                #region DQE
                DataTable dtDQES = GetDFXScore(DocNo, "DQE");
                double DQES = (double.Parse(dtDQES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtDQES.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtDQES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtDQES.Rows[0]["MaxPoints"].ToString())) * 100;
                cells[7, 2].PutValue(DQES.ToString("0.0") + "%");
                //sheet.Replace("{DQES}", DQES);
                if (DQES > LevelEVT && DQE0 == "0")
                {
                    string ResultDQEP = "PASS";
                    sheet.Replace("{ResultDQE}", ResultDQEP);
                    sheet.Replace("{DQERemark}", "");
                }
                else if (DQES == 0)
                {
                    sheet.Replace("{ResultDQE}", "NA");
                    sheet.Replace("{DQERemark}", "");
                }
                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[7, 7].SetStyle(style);
                    string ResultDQEF = "FAIL";
                    sheet.Replace("{ResultDQE}", ResultDQEF);

                    DataTable dtDQEF = GetDFXFail(DocNo, "DQE");
                    int lenDQE = dtDQEF.Rows.Count;
                    string[] strDQE = new string[lenDQE];
                    for (int i = 0; i < lenDQE; i++)
                    {
                        strDQE[i] = dtDQEF.Rows[i]["Item"].ToString();
                    }
                    string DQEFail = string.Join(",", strDQE);
                    sheet.Replace("{DQERemark}", DQEFail);
                }

                #endregion

                #region EE
                DataTable dtEES = GetDFXScore(DocNo, "EE");
                double EES = (double.Parse(dtEES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtEES.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtEES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtEES.Rows[0]["MaxPoints"].ToString())) * 100;
                cells[8, 2].PutValue(EES.ToString("0.0") + "%");
                //sheet.Replace("{EES}", EES);
                if (EES > LevelEVT && EE0 == "0")
                {
                    string ResultEEP = "PASS";
                    sheet.Replace("{ResultEE}", ResultEEP);
                    sheet.Replace("{EERemark}", "");
                }
                else if (EES == 0)
                {
                    sheet.Replace("{ResultEE}", "NA");
                    sheet.Replace("{EERemark}", "");
                }
                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[8, 7].SetStyle(style);
                    string ResultEEF = "FAIL";
                    sheet.Replace("{ResultEE}", ResultEEF);

                    DataTable dtEEF = GetDFXFail(DocNo, "EE");
                    int lenEE = dtEEF.Rows.Count;
                    string[] strEE = new string[lenEE];
                    for (int i = 0; i < lenEE; i++)
                    {
                        strEE[i] = dtEEF.Rows[i]["Item"].ToString();
                    }
                    string EEFail = string.Join(",", strEE);
                    sheet.Replace("{EERemark}", EEFail);
                }
                #endregion

                #region UQ
                DataTable dtUQS = GetDFXScore(DocNo, "UQ");
                double UQS = (double.Parse(dtUQS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtUQS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtUQS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtUQS.Rows[0]["MaxPoints"].ToString())) * 100;
                cells[9, 2].PutValue(UQS.ToString("0.0") + "%");
                //sheet.Replace("{UQS}", UQS);
                if (UQS > LevelEVT && UQ0 == "0")
                {
                    string ResultUQP = "PASS";
                    sheet.Replace("{ResultUQ}", ResultUQP);
                    sheet.Replace("{UQRemark}", "");
                }
                else if (UQS == 0)
                {
                    sheet.Replace("{ResultUQ}", "NA");
                    sheet.Replace("{UQRemark}", "");
                }
                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[9, 7].SetStyle(style);
                    string ResultUQF = "FAIL";
                    sheet.Replace("{ResultUQ}", ResultUQF);

                    DataTable dtUQF = GetDFXFail(DocNo, "UQ");
                    int lenUQ = dtUQF.Rows.Count;
                    string[] strUQ = new string[lenUQ];
                    for (int i = 0; i < lenUQ; i++)
                    {
                        strUQ[i] = dtUQF.Rows[i]["Item"].ToString();
                    }
                    string UQFail = string.Join(",", strUQ);
                    sheet.Replace("{UQRemark}", UQFail);
                }
                #endregion

                #region SQ
                DataTable dtSQS = GetDFXScore(DocNo, "SQ");
                double SQS = (double.Parse(dtSQS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtSQS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtSQS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtSQS.Rows[0]["MaxPoints"].ToString())) * 100;
                cells[10, 2].PutValue(SQS.ToString("0.0") + "%");
                //sheet.Replace("{SQS}", SQS);
                if (SQS > LevelEVT && SQ0 == "0")
                {
                    string ResultSQP = "PASS";
                    sheet.Replace("{ResultSQ}", ResultSQP);
                    sheet.Replace("{SQRemark}", "");
                }
                else if (SQS == 0)
                {
                    sheet.Replace("{ResultSQ}", "NA");
                    sheet.Replace("{SQRemark}", "");
                }
                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[10, 7].SetStyle(style);
                    string ResultSQF = "FAIL";
                    sheet.Replace("{ResultSQ}", ResultSQF);

                    DataTable dtSQF = GetDFXFail(DocNo, "SQ");
                    int lenSQ = dtSQF.Rows.Count;
                    string[] strSQ = new string[lenSQ];
                    for (int i = 0; i < lenSQ; i++)
                    {
                        strSQ[i] = dtSQF.Rows[i]["Item"].ToString();
                    }
                    string SQFail = string.Join(",", strSQ);
                    sheet.Replace("{SQRemark}", SQFail);
                }
                #endregion

                #region 統計總分數,確認狀態(PASS/FAIL)
                double ResultScore = ((AIRIS * 10 + SMTS * 10 + IES * 10 + DQES * 10 + EES * 10 + UQS * 10 + SQS * 10) / divisor) * 100;
                sheet.Replace("{Score}", ResultScore.ToString("0.0") + "%");
                if (AIRIResult == "FAIL" || SMTResult == "FAIL" || IEResult == "FAIL" || DQEResult == "FAIL" || EEResult == "FAIL" || UQResult == "FAIL" || SQResult == "FAIL")
                {
                    sheet.Replace("{Result}", "FAIL");
                }
                else
                {
                    if (ResultScore >= 80)
                    {
                        sheet.Replace("{Result}", "PASS");
                    }
                    else
                    {
                        sheet.Replace("{Result}", "FAIL");
                    }

                }
                #endregion
            }
            #endregion

            #region DVT階段 DFX統計  >=90
            if (drM["SUB_DOC_PHASE"].ToString().Contains("DVT"))
            {
                sheet.Replace("{Goal}", "90%");
                int LevelEVT = 90;

                #region AIRI
                DataTable dtAIRIS = GetDFXScore(DocNo, "AI_RI");//計算AI_RI的分數
                double AIRIS = (double.Parse(dtAIRIS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtAIRIS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtAIRIS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtAIRIS.Rows[0]["MaxPoints"].ToString())) * 100;
                cells[4, 2].PutValue(AIRIS.ToString("0.0") + "%");
                if (AIRIS >= LevelEVT && AIRI0 == "0" && AIRI1 == "0")
                {
                    string ResultAIRIP = "PASS";
                    sheet.Replace("{ResultAIRI}", ResultAIRIP);
                    sheet.Replace("{AIRIRemark}", "");
                }
                else if (AIRIS == 0)
                {
                    sheet.Replace("{ResultAIRI}", "NA");
                    sheet.Replace("{AIRIRemark}", "");
                }
                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[4, 7].SetStyle(style);
                    string ResultAIRIF = "FAIL";
                    sheet.Replace("{ResultAIRI}", ResultAIRIF);

                    DataTable dtAIRIF = GetDFXFail(DocNo, "AI_RI");
                    int lenAI = dtAIRIF.Rows.Count;
                    string[] strAI = new string[lenAI];
                    for (int i = 0; i < lenAI; i++)
                    {
                        strAI[i] = dtAIRIF.Rows[i]["Item"].ToString();
                    }
                    string AIRIFail = string.Join(",", strAI);
                    sheet.Replace("{AIRIRemark}", AIRIFail);
                }
                #endregion

                #region SMT
                DataTable dtSMTS = GetDFXScore(DocNo, "SMT");
                double SMTS = (double.Parse(dtSMTS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtSMTS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtSMTS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtSMTS.Rows[0]["MaxPoints"].ToString())) * 100;
                cells[5, 2].PutValue(SMTS.ToString("0.0") + "%");
                //sheet.Replace("{SMTS}", SMTS);
                if (SMTS > LevelEVT && SMT0 == "0" && SMT1 == "0")
                {
                    string ResultSMTP = "PASS";
                    sheet.Replace("{ResultSMT}", ResultSMTP);
                    sheet.Replace("{SMTRemark}", "");
                }
                else if (SMTS == 0)
                {
                    sheet.Replace("{ResultSMT}", "NA");
                    sheet.Replace("{SMTRemark}", "");
                }
                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[5, 7].SetStyle(style);
                    string ResultSMTF = "FAIL";
                    sheet.Replace("{ResultSMT}", ResultSMTF);

                    DataTable dtSMTF = GetDFXFail(DocNo, "SMT");
                    int lenSMT = dtSMTF.Rows.Count;
                    string[] strSMT = new string[lenSMT];
                    for (int i = 0; i < lenSMT; i++)
                    {
                        strSMT[i] = dtSMTF.Rows[i]["Item"].ToString();
                    }
                    string SMTFail = string.Join(",", strSMT);
                    sheet.Replace("{SMTRemark}", SMTFail);
                }
                #endregion

                #region IE
                DataTable dtIES = GetDFXScore(DocNo, "IE");
                double IES = (double.Parse(dtIES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtIES.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtIES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtIES.Rows[0]["MaxPoints"].ToString())) * 100;
                cells[6, 2].PutValue(IES.ToString("0.0") + "%");
                //sheet.Replace("{IES}", IES);
                if (IES > LevelEVT && IE0 == "0" && IE1 == "0")
                {
                    string ResultIEP = "PASS";
                    sheet.Replace("{ResultIE}", ResultIEP);
                    sheet.Replace("{IERemark}", "");

                }
                else if (IES == 0)
                {
                    sheet.Replace("{ResultIE}", "NA");
                    sheet.Replace("{IERemark}", "");
                }
                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[6, 7].SetStyle(style);
                    string ResultIEF = "FAIL";
                    sheet.Replace("{ResultIE}", ResultIEF);

                    DataTable dtIEF = GetDFXFail(DocNo, "IE");
                    int lenIE = dtIEF.Rows.Count;
                    string[] strIE = new string[lenIE];
                    for (int i = 0; i < lenIE; i++)
                    {
                        strIE[i] = dtIEF.Rows[i]["Item"].ToString();
                    }
                    string IEFail = string.Join(",", strIE);
                    sheet.Replace("{IERemark}", IEFail);
                }
                #endregion

                #region DQE
                DataTable dtDQES = GetDFXScore(DocNo, "DQE");
                double DQES = (double.Parse(dtDQES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtDQES.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtDQES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtDQES.Rows[0]["MaxPoints"].ToString())) * 100;
                cells[7, 2].PutValue(DQES.ToString("0.0") + "%");
                //sheet.Replace("{DQES}", DQES);
                if (DQES > LevelEVT && DQE0 == "0" && DQE1 == "0")
                {
                    string ResultDQEP = "PASS";
                    sheet.Replace("{ResultDQE}", ResultDQEP);
                    sheet.Replace("{DQERemark}", "");
                }
                else if (DQES == 0)
                {
                    sheet.Replace("{ResultDQE}", "NA");
                    sheet.Replace("{DQERemark}", "");
                }
                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[7, 7].SetStyle(style);
                    string ResultDQEF = "FAIL";
                    sheet.Replace("{ResultDQE}", ResultDQEF);

                    DataTable dtDQEF = GetDFXFail(DocNo, "DQE");
                    int lenDQE = dtDQEF.Rows.Count;
                    string[] strDQE = new string[lenDQE];
                    for (int i = 0; i < lenDQE; i++)
                    {
                        strDQE[i] = dtDQEF.Rows[i]["Item"].ToString();
                    }
                    string DQEFail = string.Join(",", strDQE);
                    sheet.Replace("{DQERemark}", DQEFail);
                }

                #endregion

                #region EE
                DataTable dtEES = GetDFXScore(DocNo, "EE");
                double EES = (double.Parse(dtEES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtEES.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtEES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtEES.Rows[0]["MaxPoints"].ToString())) * 100;
                cells[8, 2].PutValue(EES.ToString("0.0") + "%");
                //sheet.Replace("{EES}", EES);
                if (EES > LevelEVT && EE0 == "0" && EE1 == "0")
                {
                    string ResultEEP = "PASS";
                    sheet.Replace("{ResultEE}", ResultEEP);
                    sheet.Replace("{EERemark}", "");
                }
                else if (EES == 0)
                {
                    sheet.Replace("{ResultEE}", "NA");
                    sheet.Replace("{EERemark}", "");
                }
                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[8, 7].SetStyle(style);
                    string ResultEEF = "FAIL";
                    sheet.Replace("{ResultEE}", ResultEEF);

                    DataTable dtEEF = GetDFXFail(DocNo, "EE");
                    int lenEE = dtEEF.Rows.Count;
                    string[] strEE = new string[lenEE];
                    for (int i = 0; i < lenEE; i++)
                    {
                        strEE[i] = dtEEF.Rows[i]["Item"].ToString();
                    }
                    string EEFail = string.Join(",", strEE);
                    sheet.Replace("{EERemark}", EEFail);
                }
                #endregion

                #region UQ
                DataTable dtUQS = GetDFXScore(DocNo, "UQ");
                double UQS = (double.Parse(dtUQS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtUQS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtUQS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtUQS.Rows[0]["MaxPoints"].ToString())) * 100;
                cells[9, 2].PutValue(UQS.ToString("0.0") + "%");
                //sheet.Replace("{UQS}", UQS);
                if (UQS > LevelEVT && UQ0 == "0" && UQ1 == "0")
                {
                    string ResultUQP = "PASS";
                    sheet.Replace("{ResultUQ}", ResultUQP);
                    sheet.Replace("{UQRemark}", "");
                }
                else if (UQS == 0)
                {
                    sheet.Replace("{ResultUQ}", "NA");
                    sheet.Replace("{UQRemark}", "");
                }
                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[9, 7].SetStyle(style);
                    string ResultUQF = "FAIL";
                    sheet.Replace("{ResultUQ}", ResultUQF);

                    DataTable dtUQF = GetDFXFail(DocNo, "UQ");
                    int lenUQ = dtUQF.Rows.Count;
                    string[] strUQ = new string[lenUQ];
                    for (int i = 0; i < lenUQ; i++)
                    {
                        strUQ[i] = dtUQF.Rows[i]["Item"].ToString();
                    }
                    string UQFail = string.Join(",", strUQ);
                    sheet.Replace("{UQRemark}", UQFail);
                }
                #endregion

                #region SQ
                DataTable dtSQS = GetDFXScore(DocNo, "SQ");
                double SQS = (double.Parse(dtSQS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtSQS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtSQS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtSQS.Rows[0]["MaxPoints"].ToString())) * 100;
                cells[10, 2].PutValue(SQS.ToString("0.0") + "%");
                //sheet.Replace("{SQS}", SQS);
                if (SQS > LevelEVT && SQ0 == "0" && SQ1 == "0")
                {
                    string ResultSQP = "PASS";
                    sheet.Replace("{ResultSQ}", ResultSQP);
                    sheet.Replace("{SQRemark}", "");
                }
                else if (SQS == 0)
                {
                    sheet.Replace("{ResultSQ}", "");
                    sheet.Replace("{SQRemark}", "");
                }
                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[10, 7].SetStyle(style);
                    string ResultSQF = "FAIL";
                    sheet.Replace("{ResultSQ}", ResultSQF);

                    DataTable dtSQF = GetDFXFail(DocNo, "SQ");
                    int lenSQ = dtSQF.Rows.Count;
                    string[] strSQ = new string[lenSQ];
                    for (int i = 0; i < lenSQ; i++)
                    {
                        strSQ[i] = dtSQF.Rows[i]["Item"].ToString();
                    }
                    string SQFail = string.Join(",", strSQ);
                    sheet.Replace("{SQRemark}", SQFail);
                }
                #endregion

                #region 統計總分數,確認狀態(PASS/FAIL)
                double ResultScore = ((AIRIS * 10 + SMTS * 10 + IES * 10 + DQES * 10 + EES * 10 + UQS * 10 + SQS * 10) / divisor) * 100;
                sheet.Replace("{Score}", ResultScore.ToString("0.0") + "%");
                if (AIRIResult == "FAIL" || SMTResult == "FAIL" || IEResult == "FAIL" || DQEResult == "FAIL" || EEResult == "FAIL" || UQResult == "FAIL" || SQResult == "FAIL")
                {
                    sheet.Replace("{Result}", "FAIL");
                }
                else
                {
                    if (ResultScore >= 80)
                    {
                        sheet.Replace("{Result}", "PASS");
                    }
                    else
                    {
                        sheet.Replace("{Result}", "FAIL");
                    }

                }
                #endregion
            }
            #endregion

            #region PR階段 DFX統計  >=90
            else if (drM["SUB_DOC_PHASE"].ToString().Contains("P.Run"))
            {
                sheet.Replace("{Goal}", "90%");
                int LevelEVT = 90;

                #region AIRI
                DataTable dtAIRIS = GetDFXScore(DocNo, "AI_RI");//計算AI_RI的分數
                double AIRIS = (double.Parse(dtAIRIS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtAIRIS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtAIRIS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtAIRIS.Rows[0]["MaxPoints"].ToString())) * 100;
                cells[4, 2].PutValue(AIRIS.ToString("0.0") + "%");
                if (AIRIS >= LevelEVT && AIRI0 == "0" && AIRI1 == "0")
                {
                    string ResultAIRIP = "PASS";
                    sheet.Replace("{ResultAIRI}", ResultAIRIP);
                    sheet.Replace("{AIRIRemark}", "");
                }
                else if (AIRIS == 0)
                {
                    sheet.Replace("{ResultAIRI}", "NA");
                    sheet.Replace("{AIRIRemark}", "");
                }
                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[4, 7].SetStyle(style);
                    string ResultAIRIF = "FAIL";
                    sheet.Replace("{ResultAIRI}", ResultAIRIF);

                    DataTable dtAIRIF = GetDFXFail(DocNo, "AI_RI");
                    int lenAI = dtAIRIF.Rows.Count;
                    string[] strAI = new string[lenAI];
                    for (int i = 0; i < lenAI; i++)
                    {
                        strAI[i] = dtAIRIF.Rows[i]["Item"].ToString();
                    }
                    string AIRIFail = string.Join(",", strAI);
                    sheet.Replace("{AIRIRemark}", AIRIFail);
                }
                #endregion

                #region SMT
                DataTable dtSMTS = GetDFXScore(DocNo, "SMT");
                double SMTS = (double.Parse(dtSMTS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtSMTS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtSMTS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtSMTS.Rows[0]["MaxPoints"].ToString())) * 100;
                cells[5, 2].PutValue(SMTS.ToString("0.0") + "%");
                //sheet.Replace("{SMTS}", SMTS);
                if (SMTS > LevelEVT && SMT0 == "0" && SMT1 == "0")
                {
                    string ResultSMTP = "PASS";
                    sheet.Replace("{ResultSMT}", ResultSMTP);
                    sheet.Replace("{SMTRemark}", "");
                }
                else if (SMTS == 0)
                {
                    sheet.Replace("{ResultSMT}", "NA");
                    sheet.Replace("{SMTRemark}", "");
                }
                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[5, 7].SetStyle(style);
                    string ResultSMTF = "FAIL";
                    sheet.Replace("{ResultSMT}", ResultSMTF);

                    DataTable dtSMTF = GetDFXFail(DocNo, "SMT");
                    int lenSMT = dtSMTF.Rows.Count;
                    string[] strSMT = new string[lenSMT];
                    for (int i = 0; i < lenSMT; i++)
                    {
                        strSMT[i] = dtSMTF.Rows[i]["Item"].ToString();
                    }
                    string SMTFail = string.Join(",", strSMT);
                    sheet.Replace("{SMTRemark}", SMTFail);
                }
                #endregion

                #region IE
                DataTable dtIES = GetDFXScore(DocNo, "IE");
                double IES = (double.Parse(dtIES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtIES.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtIES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtIES.Rows[0]["MaxPoints"].ToString())) * 100;
                cells[6, 2].PutValue(IES.ToString("0.0") + "%");
                //sheet.Replace("{IES}", IES);
                if (IES > LevelEVT && IE0 == "0" && IE1 == "0")
                {
                    string ResultIEP = "PASS";
                    sheet.Replace("{ResultIE}", ResultIEP);
                    sheet.Replace("{IERemark}", "");

                }
                else if (IES == 0)
                {
                    sheet.Replace("{ResultIE}", "NA");
                    sheet.Replace("{IERemark}", "");
                }
                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[6, 7].SetStyle(style);
                    string ResultIEF = "FAIL";
                    sheet.Replace("{ResultIE}", ResultIEF);

                    DataTable dtIEF = GetDFXFail(DocNo, "IE");
                    int lenIE = dtIEF.Rows.Count;
                    string[] strIE = new string[lenIE];
                    for (int i = 0; i < lenIE; i++)
                    {
                        strIE[i] = dtIEF.Rows[i]["Item"].ToString();
                    }
                    string IEFail = string.Join(",", strIE);
                    sheet.Replace("{IERemark}", IEFail);
                }
                #endregion

                #region DQE
                DataTable dtDQES = GetDFXScore(DocNo, "DQE");
                double DQES = (double.Parse(dtDQES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtDQES.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtDQES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtDQES.Rows[0]["MaxPoints"].ToString())) * 100;
                cells[7, 2].PutValue(DQES.ToString("0.0") + "%");
                //sheet.Replace("{DQES}", DQES);
                if (DQES > LevelEVT && DQE0 == "0" && DQE1 == "0")
                {
                    string ResultDQEP = "PASS";
                    sheet.Replace("{ResultDQE}", ResultDQEP);
                    sheet.Replace("{DQERemark}", "");
                }
                else if (DQES == 0)
                {
                    sheet.Replace("{ResultDQE}", "NA");
                    sheet.Replace("{DQERemark}", "");
                }
                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[7, 7].SetStyle(style);
                    string ResultDQEF = "FAIL";
                    sheet.Replace("{ResultDQE}", ResultDQEF);

                    DataTable dtDQEF = GetDFXFail(DocNo, "DQE");
                    int lenDQE = dtDQEF.Rows.Count;
                    string[] strDQE = new string[lenDQE];
                    for (int i = 0; i < lenDQE; i++)
                    {
                        strDQE[i] = dtDQEF.Rows[i]["Item"].ToString();
                    }
                    string DQEFail = string.Join(",", strDQE);
                    sheet.Replace("{DQERemark}", DQEFail);
                }

                #endregion

                #region EE
                DataTable dtEES = GetDFXScore(DocNo, "EE");
                double EES = (double.Parse(dtEES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtEES.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtEES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtEES.Rows[0]["MaxPoints"].ToString())) * 100;
                cells[8, 2].PutValue(EES.ToString("0.0") + "%");
                //sheet.Replace("{EES}", EES);
                if (EES > LevelEVT && EE0 == "0" && EE1 == "0")
                {
                    string ResultEEP = "PASS";
                    sheet.Replace("{ResultEE}", ResultEEP);
                    sheet.Replace("{EERemark}", "");
                }
                else if (EES == 0)
                {
                    sheet.Replace("{ResultEE}", "NA");
                    sheet.Replace("{EERemark}", "");
                }
                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[8, 7].SetStyle(style);
                    string ResultEEF = "FAIL";
                    sheet.Replace("{ResultEE}", ResultEEF);

                    DataTable dtEEF = GetDFXFail(DocNo, "EE");
                    int lenEE = dtEEF.Rows.Count;
                    string[] strEE = new string[lenEE];
                    for (int i = 0; i < lenEE; i++)
                    {
                        strEE[i] = dtEEF.Rows[i]["Item"].ToString();
                    }
                    string EEFail = string.Join(",", strEE);
                    sheet.Replace("{EERemark}", EEFail);
                }
                #endregion

                #region UQ
                DataTable dtUQS = GetDFXScore(DocNo, "UQ");
                double UQS = (double.Parse(dtUQS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtUQS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtUQS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtUQS.Rows[0]["MaxPoints"].ToString())) * 100;
                cells[9, 2].PutValue(UQS.ToString("0.0") + "%");
                //sheet.Replace("{UQS}", UQS);
                if (UQS > LevelEVT && UQ0 == "0" && UQ1 == "0")
                {
                    string ResultUQP = "PASS";
                    sheet.Replace("{ResultUQ}", ResultUQP);
                    sheet.Replace("{UQRemark}", "");
                }
                else if (UQS == 0)
                {
                    sheet.Replace("{ResultUQ}", "NA");
                    sheet.Replace("{UQRemark}", "");
                }
                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[9, 7].SetStyle(style);
                    string ResultUQF = "FAIL";
                    sheet.Replace("{ResultUQ}", ResultUQF);

                    DataTable dtUQF = GetDFXFail(DocNo, "UQ");
                    int lenUQ = dtUQF.Rows.Count;
                    string[] strUQ = new string[lenUQ];
                    for (int i = 0; i < lenUQ; i++)
                    {
                        strUQ[i] = dtUQF.Rows[i]["Item"].ToString();
                    }
                    string UQFail = string.Join(",", strUQ);
                    sheet.Replace("{UQRemark}", UQFail);
                }
                #endregion

                #region SQ
                DataTable dtSQS = GetDFXScore(DocNo, "SQ");
                double SQS = (double.Parse(dtSQS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtSQS.Rows[0]["DFXPoints"].ToString())
                            / double.Parse(dtSQS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtSQS.Rows[0]["MaxPoints"].ToString())) * 100;
                cells[10, 2].PutValue(SQS.ToString("0.0") + "%");
                //sheet.Replace("{SQS}", SQS);
                if (SQS > LevelEVT && SQ0 == "0" && SQ1 == "0")
                {
                    string ResultSQP = "PASS";
                    sheet.Replace("{ResultSQ}", ResultSQP);
                    sheet.Replace("{SQRemark}", "");
                }
                else if (SQS == 0)
                {
                    sheet.Replace("{ResultSQ}", "NA");
                    sheet.Replace("{SQRemark}", "");
                }
                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[10, 7].SetStyle(style);
                    string ResultSQF = "FAIL";
                    sheet.Replace("{ResultSQ}", ResultSQF);

                    DataTable dtSQF = GetDFXFail(DocNo, "SQ");
                    int lenSQ = dtSQF.Rows.Count;
                    string[] strSQ = new string[lenSQ];
                    for (int i = 0; i < lenSQ; i++)
                    {
                        strSQ[i] = dtSQF.Rows[i]["Item"].ToString();
                    }
                    string SQFail = string.Join(",", strSQ);
                    sheet.Replace("{SQRemark}", SQFail);
                }
                #endregion
                #region 統計總分數,確認狀態(PASS/FAIL)
                double ResultScore = ((AIRIS * 10 + SMTS * 10 + IES * 10 + DQES * 10 + EES * 10 + UQS * 10 + SQS * 10) / divisor) * 100;
                sheet.Replace("{Score}", ResultScore.ToString("0.0") + "%");
                if (AIRIResult == "FAIL" || SMTResult == "FAIL" || IEResult == "FAIL" || DQEResult == "FAIL" || EEResult == "FAIL" || UQResult == "FAIL" || SQResult == "FAIL")
                {
                    sheet.Replace("{Result}", "FAIL");
                }
                else
                {
                    if (ResultScore >= 80)
                    {
                        sheet.Replace("{Result}", "PASS");
                    }
                    else
                    {
                        sheet.Replace("{Result}", "FAIL");
                    }

                }
                #endregion

            }
            #endregion
            #endregion
        }
        #region 填充AUTO TE部门的OldItemType
        #region AUTO
        DataTable dtAUTODFX = GetAUTOTEResult(drM["SUB_DOC_NO"].ToString(), "AUTO");
        if (dtAUTODFX.Rows.Count > 0)
        {
            int templateIndexDFX = 11;//模板row起始位置
            int insertIndexEnCounter = templateIndexDFX;//new row起始位置
            cells.InsertRows(insertIndexEnCounter, dtAUTODFX.Rows.Count);
            cells.CopyRows(cells, templateIndexDFX, insertIndexEnCounter, dtAUTODFX.Rows.Count); //複製模板row格式至新行
            for (int i = 0; i < dtAUTODFX.Rows.Count; i++)
            {
                DataRow dr = dtAUTODFX.Rows[i];
                DataTable dtAUTOR = GetCompliance(drM["SUB_DOC_NO"].ToString(), "AUTO", dr["OldItemType"].ToString()); //抓取为N的项
                DataTable dtAUTOW = GetDFXWrite(drM["DOC_NO"].ToString(), "AUTO"); //抓取Dept.Write 簽核人
                DataTable dtAUTOC = GetDFXCheck(drM["DOC_NO"].ToString(), "AUTO", drM["SUB_DOC_NO"].ToString());//抓取Reply.Check 簽核人
                cells[i + templateIndexDFX, 0].PutValue("AUTO");
                cells[i + templateIndexDFX, 1].PutValue(dr["OldItemType"].ToString());
                cells[i + templateIndexDFX, 2].PutValue("N/A");
                cells[i + templateIndexDFX, 3].PutValue("0");
                cells[i + templateIndexDFX, 4].PutValue("0");
                cells[i + templateIndexDFX, 5].PutValue("0");
                cells[i + templateIndexDFX, 6].PutValue("0");
                if (dtAUTOR.Rows.Count > 0)
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[i + templateIndexDFX, 7].SetStyle(style);

                    cells[i + templateIndexDFX, 7].PutValue("FAIL");
                    sheet.Replace("{Result}", "FAIL");
                }
                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(0, 0, 0);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[i + templateIndexDFX, 7].SetStyle(style);

                    cells[i + templateIndexDFX, 7].PutValue("PASS");
                }

                if (dtAUTOW.Rows.Count > 0)
                {
                    cells[i + templateIndexDFX, 8].PutValue(dtAUTOW.Rows[0]["WriteEname"].ToString());
                }
                else
                {
                    cells[i + templateIndexDFX, 8].PutValue("");
                }

                if (dtAUTOC.Rows.Count > 0)
                {
                    cells[i + templateIndexDFX, 9].PutValue(dtAUTOC.Rows[0]["APPROVER"].ToString() + ";" + Convert.ToDateTime(dtAUTOC.Rows[0]["APPROVER_DATE"].ToString()).ToString("yyyy/MM/dd hh:mm"));
                }
                else
                {
                    cells[i + templateIndexDFX, 9].PutValue("");
                }

            }

        }
        #endregion

        #region TE
        DataTable dtTEDFX = GetAUTOTEResult(drM["SUB_DOC_NO"].ToString(), "TE");
        if (dtTEDFX.Rows.Count > 0)
        {
            int templateIndexDFX = 11 + dtAUTODFX.Rows.Count;//模板row起始位置
            int insertIndexEnCounter = templateIndexDFX;//new row起始位置
            cells.InsertRows(insertIndexEnCounter, dtTEDFX.Rows.Count);
            cells.CopyRows(cells, templateIndexDFX, insertIndexEnCounter, dtTEDFX.Rows.Count); //複製模板row格式至新行
            for (int i = 0; i < dtTEDFX.Rows.Count; i++)
            {
                DataRow dr = dtTEDFX.Rows[i];
                DataTable dtTER = GetCompliance(drM["SUB_DOC_NO"].ToString(), "TE", dr["OldItemType"].ToString()); //抓取为N的项
                DataTable dtTEW = GetDFXWrite(drM["DOC_NO"].ToString(), "TE"); //抓取Dept.Write 簽核人
                DataTable dtTEC = GetDFXCheck(drM["DOC_NO"].ToString(), "TE", drM["SUB_DOC_NO"].ToString());//抓取Reply.Check 簽核人
                cells[i + templateIndexDFX, 0].PutValue("TE");
                cells[i + templateIndexDFX, 1].PutValue(dr["OldItemType"].ToString());
                cells[i + templateIndexDFX, 2].PutValue("N/A");
                cells[i + templateIndexDFX, 3].PutValue("0");
                cells[i + templateIndexDFX, 4].PutValue("0");
                cells[i + templateIndexDFX, 5].PutValue("0");
                cells[i + templateIndexDFX, 6].PutValue("0");
                if (dtTER.Rows.Count > 0)
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[i + templateIndexDFX, 7].SetStyle(style);

                    cells[i + templateIndexDFX, 7].PutValue("FAIL");
                    sheet.Replace("{Result}", "FAIL");
                }
                else
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(0, 0, 0);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[i + templateIndexDFX, 7].SetStyle(style);

                    cells[i + templateIndexDFX, 7].PutValue("PASS");
                }

                if (dtTEW.Rows.Count > 0)
                {
                    cells[i + templateIndexDFX, 8].PutValue(dtTEW.Rows[0]["WriteEname"].ToString());
                }
                else
                {
                    cells[i + templateIndexDFX, 8].PutValue("");
                }

                if (dtTEC.Rows.Count > 0)
                {
                    cells[i + templateIndexDFX, 9].PutValue(dtTEC.Rows[0]["APPROVER"].ToString() + ";" + Convert.ToDateTime(dtTEC.Rows[0]["APPROVER_DATE"].ToString()).ToString("yyyy/MM/dd hh:mm"));
                }
                else
                {
                    cells[i + templateIndexDFX, 9].PutValue("");
                }

            }

        }
        #endregion
        #endregion

        #region 撈取各部門簽核人

        #region Dept.Write
        DataTable dtAIRIW = GetDFXWrite(drM["DOC_NO"].ToString(), "AI_RI");
        DataTable dtSMTW = GetDFXWrite(drM["DOC_NO"].ToString(), "SMT");
        DataTable dtIEW = GetDFXWrite(drM["DOC_NO"].ToString(), "IE");
        DataTable dtDQEW = GetDFXWrite(drM["DOC_NO"].ToString(), "DQE");
        DataTable dtEEW = GetDFXWrite(drM["DOC_NO"].ToString(), "EE");
        DataTable dtUQW = GetDFXWrite(drM["DOC_NO"].ToString(), "UQ");
        DataTable dtSQW = GetDFXWrite(drM["DOC_NO"].ToString(), "SQ");
        #region
        if (dtAIRIW.Rows.Count > 0)
        {
            sheet.Replace("{AIRIW}", dtAIRIW.Rows[0]["WriteEname"].ToString());
        }
        else
        {
            sheet.Replace("{AIRIW}", "");
        }
        if (dtSMTW.Rows.Count > 0)
        {
            sheet.Replace("{SMTW}", dtSMTW.Rows[0]["WriteEname"].ToString());
        }
        else
        {
            sheet.Replace("{SMTW}", "");
        }

        if (dtIEW.Rows.Count > 0)
        {
            sheet.Replace("{IEW}", dtIEW.Rows[0]["WriteEname"].ToString());
        }
        else
        {
            sheet.Replace("{IEW}", "");
        }

        if (dtDQEW.Rows.Count > 0)
        {
            sheet.Replace("{DQEW}", dtDQEW.Rows[0]["WriteEname"].ToString());
        }
        else
        {
            sheet.Replace("{DQEW}", "");
        }
        if (dtEEW.Rows.Count > 0)
        {
            sheet.Replace("{EEW}", dtEEW.Rows[0]["WriteEname"].ToString());
        }
        else
        {
            sheet.Replace("{EEW}", "");
        }
        if (dtUQW.Rows.Count > 0)
        {
            sheet.Replace("{UQW}", dtUQW.Rows[0]["WriteEname"].ToString());
        }
        else
        {
            sheet.Replace("{UQW}", "");
        }
        if (dtSQW.Rows.Count > 0)
        {
            sheet.Replace("{SQW}", dtSQW.Rows[0]["WriteEname"].ToString());
        }
        else
        {
            sheet.Replace("{SQW}", "");
        }


        #endregion
        #endregion

        #region ReplyCheck
        DataTable dtAIRIC = GetDFXCheck(drM["DOC_NO"].ToString(), "AI_RI", drM["SUB_DOC_NO"].ToString());
        DataTable dtSMTC = GetDFXCheck(drM["DOC_NO"].ToString(), "SMT", drM["SUB_DOC_NO"].ToString());
        DataTable dtIEC = GetDFXCheck(drM["DOC_NO"].ToString(), "IE", drM["SUB_DOC_NO"].ToString());
        DataTable dtEEC = GetDFXCheck(drM["DOC_NO"].ToString(), "EE", drM["SUB_DOC_NO"].ToString());
        DataTable dtDQEC = GetDFXCheck(drM["DOC_NO"].ToString(), "DQE", drM["SUB_DOC_NO"].ToString());
        DataTable dtUQC = GetDFXCheck(drM["DOC_NO"].ToString(), "UQ", drM["SUB_DOC_NO"].ToString());
        DataTable dtSQC = GetDFXCheck(drM["DOC_NO"].ToString(), "SQ", drM["SUB_DOC_NO"].ToString());

        #region
        if (dtAIRIC.Rows.Count > 0)
        {
            sheet.Replace("{AIRIC}", dtAIRIC.Rows[0]["APPROVER"].ToString() + ";" + Convert.ToDateTime(dtAIRIC.Rows[0]["APPROVER_DATE"].ToString()).ToString("yyyy/MM/dd hh:mm"));
        }
        else
        {
            sheet.Replace("{AIRIC}", "");
        }
        if (dtSMTC.Rows.Count > 0)
        {
            sheet.Replace("{SMTC}", dtSMTC.Rows[0]["APPROVER"].ToString() + ";" + Convert.ToDateTime(dtSMTC.Rows[0]["APPROVER_DATE"].ToString()).ToString("yyyy/MM/dd hh:mm"));
        }
        else
        {
            sheet.Replace("{SMTC}", "");
        }

        if (dtIEC.Rows.Count > 0)
        {
            sheet.Replace("{IEC}", dtIEC.Rows[0]["APPROVER"].ToString() + ";" + Convert.ToDateTime(dtIEC.Rows[0]["APPROVER_DATE"].ToString()).ToString("yyyy/MM/dd hh:mm"));
        }
        else
        {
            sheet.Replace("{IEC}", "");
        }

        if (dtDQEC.Rows.Count > 0)
        {
            sheet.Replace("{DQEC}", dtDQEC.Rows[0]["APPROVER"].ToString() + ";" + Convert.ToDateTime(dtDQEC.Rows[0]["APPROVER_DATE"].ToString()).ToString("yyyy/MM/dd hh:mm"));
        }
        else
        {
            sheet.Replace("{DQEC}", "");
        }
        if (dtEEC.Rows.Count > 0)
        {
            sheet.Replace("{EEC}", dtEEC.Rows[0]["APPROVER"].ToString() + ";" + Convert.ToDateTime(dtEEC.Rows[0]["APPROVER_DATE"].ToString()).ToString("yyyy/MM/dd hh:mm"));
        }
        else
        {
            sheet.Replace("{EEC}", "");
        }
        if (dtUQC.Rows.Count > 0)
        {
            sheet.Replace("{UQC}", dtUQC.Rows[0]["APPROVER"].ToString() + ";" + Convert.ToDateTime(dtUQC.Rows[0]["APPROVER_DATE"].ToString()).ToString("yyyy/MM/dd hh:mm"));
        }
        else
        {
            sheet.Replace("{UQC}", "");
        }
        if (dtSQC.Rows.Count > 0)
        {
            sheet.Replace("{SQC}", dtSQC.Rows[0]["APPROVER"].ToString() + ";" + Convert.ToDateTime(dtSQC.Rows[0]["APPROVER_DATE"].ToString()).ToString("yyyy/MM/dd hh:mm"));
        }
        else
        {
            sheet.Replace("{SQC}", "");
        }
        #endregion
        #endregion

        #endregion

    }

    private void BindDFXStage(ref Aspose.Cells.Worksheet sheet, string caseID, string Bu, string Building, string DocNo, string Model)
    {
        //page 格式設定
        SetStyle(ref sheet, Aspose.Cells.PageOrientationType.Landscape);
        Aspose.Cells.Cells cells = sheet.Cells;
        Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
        Aspose.Cells.Style style = wb.Styles[wb.Styles.Add()];

        NPIMgmt oMgmt = new NPIMgmt("CZ", Bu);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        DataTable dtMaster = GetMaster(Model, "");//基本資料 By Model
        DataRow drM = dtMaster.Rows[0];
        sheet.Replace("{BU}", drM["BU"].ToString() + "-" + drM["BUILDING"].ToString());
        sheet.Replace("{Customer}", drM["CUSTOMER"].ToString());
        sheet.Replace("{ModelName}", drM["MODEL_NAME"].ToString());

        if (dtMaster.Rows.Count > 0)
        {
            int templateIndexDFX = 4;//模板row起始位置
            int insertIndexEnCounter = templateIndexDFX ;//new row起始位置

            for (int i = 0; i < dtMaster.Rows.Count; ++i)
            {
                DataRow drMaster = dtMaster.Rows[i];
                DataTable dtResult = DFXScoreMaster(drMaster["SUB_DOC_NO"].ToString());
                int count = dtResult.Rows.Count;
                int divisor = count * 1000;
                cells[4, i + 1].PutValue(drMaster["SUB_DOC_PHASE"].ToString());
                cells[5, i + 1].PutValue(Convert.ToDateTime(drMaster["APPLY_DATE"].ToString()).ToString("yyyy-MM-dd"));
                cells[6, i + 1].PutValue(drMaster["SUB_DOC_NO"].ToString());

                //Safety,Auto,TE 分數一欄为NA
                cells[17, i + 1].PutValue("NA");
                cells[18, i + 1].PutValue("NA");
                cells[19, i + 1].PutValue("NA");
                if (dtResult.Rows.Count > 0)
                {
                    #region 獲取各個部門的Result
                    DataTable dtAIRIR = DFXScoreMaster(drMaster["SUB_DOC_NO"].ToString(), "AI_RI");
                    DataTable dtSMTR = DFXScoreMaster(drMaster["SUB_DOC_NO"].ToString(), "SMT");
                    DataTable dtIER = DFXScoreMaster(drMaster["SUB_DOC_NO"].ToString(), "IE");
                    DataTable dtDQER = DFXScoreMaster(drMaster["SUB_DOC_NO"].ToString(), "DQE");
                    DataTable dtEER = DFXScoreMaster(drMaster["SUB_DOC_NO"].ToString(), "EE");
                    DataTable dtUQR = DFXScoreMaster(drMaster["SUB_DOC_NO"].ToString(), "UQ");
                    DataTable dtSQR = DFXScoreMaster(drMaster["SUB_DOC_NO"].ToString(), "SQ");

                    string AIRIResult = dtAIRIR.Rows[0]["Result"].ToString();
                    string SMTResult = dtSMTR.Rows[0]["Result"].ToString();
                    string IEResult = dtIER.Rows[0]["Result"].ToString();
                    string DQEResult = dtDQER.Rows[0]["Result"].ToString();
                    string EEResult = dtEER.Rows[0]["Result"].ToString();
                    string UQResult = dtUQR.Rows[0]["Result"].ToString();
                    string SQResult = dtSQR.Rows[0]["Result"].ToString();

                    #endregion

                    #region 获取DFX各個部門項目權重的Open項
                    #region AIRI
                    DataTable dtAIRILevel0A = GetDFXLevel(drMaster["SUB_DOC_NO"].ToString(), "AI_RI", "0");
                    DataTable dtAIRILevel1A = GetDFXLevel(drMaster["SUB_DOC_NO"].ToString(), "AI_RI", "1");
                    DataTable dtAIRILevel2A = GetDFXLevel(drMaster["SUB_DOC_NO"].ToString(), "AI_RI", "2");
                    DataTable dtAIRILevel3A = GetDFXLevel(drMaster["SUB_DOC_NO"].ToString(), "AI_RI", "3");
                    string AIRI0A = dtAIRILevel0A.Rows[0]["amount"].ToString() == "" ? "0" : dtAIRILevel0A.Rows[0]["amount"].ToString();
                    string AIRI1A = dtAIRILevel1A.Rows[0]["amount"].ToString() == "" ? "0" : dtAIRILevel1A.Rows[0]["amount"].ToString();
                    string AIRI2A = dtAIRILevel2A.Rows[0]["amount"].ToString() == "" ? "0" : dtAIRILevel2A.Rows[0]["amount"].ToString();
                    string AIRI3A = dtAIRILevel3A.Rows[0]["amount"].ToString() == "" ? "0" : dtAIRILevel3A.Rows[0]["amount"].ToString();
                    #endregion

                    #region SMT
                    DataTable dtSMTLevel0A = GetDFXLevel(drMaster["SUB_DOC_NO"].ToString(), "SMT", "0");
                    DataTable dtSMTLevel1A = GetDFXLevel(drMaster["SUB_DOC_NO"].ToString(), "SMT", "1");
                    DataTable dtSMTLevel2A = GetDFXLevel(drMaster["SUB_DOC_NO"].ToString(), "SMT", "2");
                    DataTable dtSMTLevel3A = GetDFXLevel(drMaster["SUB_DOC_NO"].ToString(), "SMT", "3");
                    string SMT0A = dtSMTLevel0A.Rows[0]["amount"].ToString() == "" ? "0" : dtSMTLevel0A.Rows[0]["amount"].ToString();
                    string SMT1A = dtSMTLevel1A.Rows[0]["amount"].ToString() == "" ? "0" : dtSMTLevel1A.Rows[0]["amount"].ToString();
                    string SMT2A = dtSMTLevel2A.Rows[0]["amount"].ToString() == "" ? "0" : dtSMTLevel2A.Rows[0]["amount"].ToString();
                    string SMT3A = dtSMTLevel3A.Rows[0]["amount"].ToString() == "" ? "0" : dtSMTLevel3A.Rows[0]["amount"].ToString();
                    #endregion

                    #region IE
                    DataTable dtIELevel0A = GetDFXLevel(drMaster["SUB_DOC_NO"].ToString(), "IE", "0");
                    DataTable dtIELevel1A = GetDFXLevel(drMaster["SUB_DOC_NO"].ToString(), "IE", "1");
                    DataTable dtIELevel2A = GetDFXLevel(drMaster["SUB_DOC_NO"].ToString(), "IE", "2");
                    DataTable dtIELevel3A = GetDFXLevel(drMaster["SUB_DOC_NO"].ToString(), "IE", "3");
                    string IE0A = dtIELevel0A.Rows[0]["amount"].ToString() == "" ? "0" : dtIELevel0A.Rows[0]["amount"].ToString();
                    string IE1A = dtIELevel1A.Rows[0]["amount"].ToString() == "" ? "0" : dtIELevel1A.Rows[0]["amount"].ToString();
                    string IE2A = dtIELevel2A.Rows[0]["amount"].ToString() == "" ? "0" : dtIELevel2A.Rows[0]["amount"].ToString();
                    string IE3A = dtIELevel3A.Rows[0]["amount"].ToString() == "" ? "0" : dtIELevel3A.Rows[0]["amount"].ToString();
                    #endregion

                    #region DQE
                    DataTable dtDQELevel0A = GetDFXLevel(drMaster["SUB_DOC_NO"].ToString(), "DQE", "0");
                    DataTable dtDQELevel1A = GetDFXLevel(drMaster["SUB_DOC_NO"].ToString(), "DQE", "1");
                    DataTable dtDQELevel2A = GetDFXLevel(drMaster["SUB_DOC_NO"].ToString(), "DQE", "2");
                    DataTable dtDQELevel3A = GetDFXLevel(drMaster["SUB_DOC_NO"].ToString(), "DQE", "3");
                    string DQE0A = dtDQELevel0A.Rows[0]["amount"].ToString() == "" ? "0" : dtDQELevel0A.Rows[0]["amount"].ToString();
                    string DQE1A = dtDQELevel1A.Rows[0]["amount"].ToString() == "" ? "0" : dtDQELevel1A.Rows[0]["amount"].ToString();
                    string DQE2A = dtDQELevel2A.Rows[0]["amount"].ToString() == "" ? "0" : dtDQELevel2A.Rows[0]["amount"].ToString();
                    string DQE3A = dtDQELevel3A.Rows[0]["amount"].ToString() == "" ? "0" : dtDQELevel3A.Rows[0]["amount"].ToString();
                    sheet.Replace("{DQE0}", DQE0A);
                    sheet.Replace("{DQE1}", DQE1A);
                    sheet.Replace("{DQE2}", DQE2A);
                    sheet.Replace("{DQE3}", DQE3A);
                    #endregion

                    #region EE
                    DataTable dtEELevel0A = GetDFXLevel(drMaster["SUB_DOC_NO"].ToString(), "EE", "0");
                    DataTable dtEELevel1A = GetDFXLevel(drMaster["SUB_DOC_NO"].ToString(), "EE", "1");
                    DataTable dtEELevel2A = GetDFXLevel(drMaster["SUB_DOC_NO"].ToString(), "EE", "2");
                    DataTable dtEELevel3A = GetDFXLevel(drMaster["SUB_DOC_NO"].ToString(), "EE", "3");
                    string EE0A = dtEELevel0A.Rows[0]["amount"].ToString() == "" ? "0" : dtEELevel0A.Rows[0]["amount"].ToString();
                    string EE1A = dtEELevel1A.Rows[0]["amount"].ToString() == "" ? "0" : dtEELevel1A.Rows[0]["amount"].ToString();
                    string EE2A = dtEELevel2A.Rows[0]["amount"].ToString() == "" ? "0" : dtEELevel2A.Rows[0]["amount"].ToString();
                    string EE3A = dtEELevel3A.Rows[0]["amount"].ToString() == "" ? "0" : dtEELevel3A.Rows[0]["amount"].ToString();
                    sheet.Replace("{EE0}", EE0A);
                    sheet.Replace("{EE1}", EE1A);
                    sheet.Replace("{EE2}", EE2A);
                    sheet.Replace("{EE3}", EE3A);
                    #endregion

                    #region UQ
                    DataTable dtUQLevel0A = GetDFXLevel(drMaster["SUB_DOC_NO"].ToString(), "UQ", "0");
                    DataTable dtUQLevel1A = GetDFXLevel(drMaster["SUB_DOC_NO"].ToString(), "UQ", "1");
                    DataTable dtUQLevel2A = GetDFXLevel(drMaster["SUB_DOC_NO"].ToString(), "UQ", "2");
                    DataTable dtUQLevel3A = GetDFXLevel(drMaster["SUB_DOC_NO"].ToString(), "UQ", "3");
                    string UQ0A = dtUQLevel0A.Rows[0]["amount"].ToString() == "" ? "0" : dtUQLevel0A.Rows[0]["amount"].ToString();
                    string UQ1A = dtUQLevel1A.Rows[0]["amount"].ToString() == "" ? "0" : dtUQLevel1A.Rows[0]["amount"].ToString();
                    string UQ2A = dtUQLevel2A.Rows[0]["amount"].ToString() == "" ? "0" : dtUQLevel2A.Rows[0]["amount"].ToString();
                    string UQ3A = dtUQLevel3A.Rows[0]["amount"].ToString() == "" ? "0" : dtUQLevel3A.Rows[0]["amount"].ToString();
                    sheet.Replace("{UQ0}", UQ0A);
                    sheet.Replace("{UQ1}", UQ1A);
                    sheet.Replace("{UQ2}", UQ2A);
                    sheet.Replace("{UQ3}", UQ3A);
                    #endregion

                    #region SQ
                    DataTable dtSQLevel0A = GetDFXLevel(drMaster["SUB_DOC_NO"].ToString(), "SQ", "0");
                    DataTable dtSQLevel1A = GetDFXLevel(drMaster["SUB_DOC_NO"].ToString(), "SQ", "1");
                    DataTable dtSQLevel2A = GetDFXLevel(drMaster["SUB_DOC_NO"].ToString(), "SQ", "2");
                    DataTable dtSQLevel3A = GetDFXLevel(drMaster["SUB_DOC_NO"].ToString(), "SQ", "3");
                    string SQ0A = dtSQLevel0A.Rows[0]["amount"].ToString() == "" ? "0" : dtSQLevel0A.Rows[0]["amount"].ToString();
                    string SQ1A = dtSQLevel1A.Rows[0]["amount"].ToString() == "" ? "0" : dtSQLevel1A.Rows[0]["amount"].ToString();
                    string SQ2A = dtSQLevel2A.Rows[0]["amount"].ToString() == "" ? "0" : dtSQLevel2A.Rows[0]["amount"].ToString();
                    string SQ3A = dtSQLevel3A.Rows[0]["amount"].ToString() == "" ? "0" : dtSQLevel3A.Rows[0]["amount"].ToString();
                    sheet.Replace("{SQ0}", SQ0A);
                    sheet.Replace("{SQ1}", SQ1A);
                    sheet.Replace("{SQ2}", SQ2A);
                    sheet.Replace("{SQ3}", SQ3A);
                    #endregion

                    int Level0 = int.Parse(AIRI0A) + int.Parse(SMT0A) + int.Parse(IE0A) + int.Parse(DQE0A) + int.Parse(EE0A) + int.Parse(UQ0A) + int.Parse(SQ0A);
                    int Level1 = int.Parse(AIRI1A) + int.Parse(SMT1A) + int.Parse(IE1A) + int.Parse(DQE1A) + int.Parse(EE1A) + int.Parse(UQ1A) + int.Parse(SQ1A);
                    int Level2 = int.Parse(AIRI2A) + int.Parse(SMT2A) + int.Parse(IE2A) + int.Parse(DQE2A) + int.Parse(EE2A) + int.Parse(UQ2A) + int.Parse(SQ2A);
                    int Level3 = int.Parse(AIRI3A) + int.Parse(SMT3A) + int.Parse(IE3A) + int.Parse(DQE3A) + int.Parse(EE3A) + int.Parse(UQ3A) + int.Parse(SQ3A);
                    cells[21, i + 1].PutValue(Level0.ToString());
                    cells[22, i + 1].PutValue(Level1.ToString());
                    cells[23, i + 1].PutValue(Level2.ToString());
                    cells[24, i + 1].PutValue(Level3.ToString());

                    #endregion

                    #region 所有部門的DFX的Score

                    #region EVT階段 DFX統計  >=80
                    if (drMaster["SUB_DOC_PHASE"].ToString().Contains("EVT"))
                    {
                        int LevelEVT = 80;

                        #region AIRI
                        DataTable dtAIRIS = GetDFXScore(drMaster["SUB_DOC_NO"].ToString(), "AI_RI");//計算AI_RI的分數
                        double AIRIS = (double.Parse(dtAIRIS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtAIRIS.Rows[0]["DFXPoints"].ToString())
                                    / double.Parse(dtAIRIS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtAIRIS.Rows[0]["MaxPoints"].ToString())) * 100;
                        if (AIRIS == 0)
                        {
                            cells[10, i + 1].PutValue("NA");
                        }
                        else
                        {
                            cells[10, i + 1].PutValue(AIRIS.ToString("0.0") + "%");
                        }
                        #endregion

                        #region SMT
                        DataTable dtSMTS = GetDFXScore(drMaster["SUB_DOC_NO"].ToString(), "SMT");
                        double SMTS = (double.Parse(dtSMTS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtSMTS.Rows[0]["DFXPoints"].ToString())
                                    / double.Parse(dtSMTS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtSMTS.Rows[0]["MaxPoints"].ToString())) * 100;
                        if (SMTS == 0)
                        {
                            cells[11, i + 1].PutValue("NA");
                        }
                        else
                        {
                            cells[11, i + 1].PutValue(SMTS.ToString("0.0") + "%");
                        }
                        #endregion

                        #region IE
                        DataTable dtIES = GetDFXScore(drMaster["SUB_DOC_NO"].ToString(), "IE");
                        double IES = (double.Parse(dtIES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtIES.Rows[0]["DFXPoints"].ToString())
                                    / double.Parse(dtIES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtIES.Rows[0]["MaxPoints"].ToString())) * 100;
                        if (IES == 0)
                        {
                            cells[12, i + 1].PutValue("NA");
                        }
                        else
                        {
                            cells[12, i + 1].PutValue(IES.ToString("0.0") + "%");
                        }
                        #endregion

                        #region DQE
                        DataTable dtDQES = GetDFXScore(drMaster["SUB_DOC_NO"].ToString(), "DQE");
                        double DQES = (double.Parse(dtDQES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtDQES.Rows[0]["DFXPoints"].ToString())
                                    / double.Parse(dtDQES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtDQES.Rows[0]["MaxPoints"].ToString())) * 100;
                        if (DQES == 0)
                        {
                            cells[13, i + 1].PutValue("NA");
                        }
                        else
                        {
                            cells[13, i + 1].PutValue(DQES.ToString("0.0") + "%");
                        }
                        #endregion

                        #region EE
                        DataTable dtEES = GetDFXScore(drMaster["SUB_DOC_NO"].ToString(), "EE");
                        double EES = (double.Parse(dtEES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtEES.Rows[0]["DFXPoints"].ToString())
                                    / double.Parse(dtEES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtEES.Rows[0]["MaxPoints"].ToString())) * 100;
                        if (EES == 0)
                        {
                            cells[14, i + 1].PutValue("NA");
                        }
                        else
                        {
                            cells[14, i + 1].PutValue(EES.ToString("0.0") + "%");
                        }
                        #endregion

                        #region UQ
                        DataTable dtUQS = GetDFXScore(drMaster["SUB_DOC_NO"].ToString(), "UQ");
                        double UQS = (double.Parse(dtUQS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtUQS.Rows[0]["DFXPoints"].ToString())
                                    / double.Parse(dtUQS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtUQS.Rows[0]["MaxPoints"].ToString())) * 100;
                        if (UQS == 0)
                        {
                            cells[15, i + 1].PutValue("NA");
                        }
                        else
                        {
                            cells[15, i + 1].PutValue(UQS.ToString("0.0") + "%");
                        }
                        #endregion

                        #region SQ
                        DataTable dtSQS = GetDFXScore(drMaster["SUB_DOC_NO"].ToString(), "SQ");
                        double SQS = (double.Parse(dtSQS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtSQS.Rows[0]["DFXPoints"].ToString())
                                    / double.Parse(dtSQS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtSQS.Rows[0]["MaxPoints"].ToString())) * 100;
                        if (SQS == 0)
                        {
                            cells[16, i + 1].PutValue("NA");
                        }
                        else
                        {
                            cells[16, i + 1].PutValue(SQS.ToString("0.0") + "%");
                        }
                        #endregion

                        #region 統計總分數,確認狀態(PASS/FAIL)
                        double ResultScore = ((AIRIS * 10 + SMTS * 10 + IES * 10 + DQES * 10 + EES * 10 + UQS * 10 + SQS * 10) / divisor) * 100;
                        cells[7, i + 1].PutValue(ResultScore.ToString("0.0") + "%");
                        if (AIRIResult == "FAIL" || SMTResult == "FAIL" || IEResult == "FAIL" || DQEResult == "FAIL" || EEResult == "FAIL" || UQResult == "FAIL" || SQResult == "FAIL")
                        {
                            cells[8, i + 1].PutValue("FAIL");
                        }
                        else
                        {
                            if (ResultScore >= 80)
                            {
                                cells[8, i + 1].PutValue("PASS");
                            }
                            else
                            {
                                cells[8, i + 1].PutValue("FAIL");
                            }

                        }
                        #endregion
                    }
                    #endregion

                    #region DVT階段 DFX統計  >=90
                    if (drMaster["SUB_DOC_PHASE"].ToString().Contains("DVT"))
                    {
                        int LevelEVT = 90;

                        #region AIRI
                        DataTable dtAIRIS = GetDFXScore(drMaster["SUB_DOC_NO"].ToString(), "AI_RI");//計算AI_RI的分數
                        double AIRIS = (double.Parse(dtAIRIS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtAIRIS.Rows[0]["DFXPoints"].ToString())
                                    / double.Parse(dtAIRIS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtAIRIS.Rows[0]["MaxPoints"].ToString())) * 100;
                        if (AIRIS == 0)
                        {
                            cells[10, i + 1].PutValue("NA");
                        }
                        else
                        {
                            cells[10, i + 1].PutValue(AIRIS.ToString("0.0") + "%");
                        }
                        #endregion

                        #region SMT
                        DataTable dtSMTS = GetDFXScore(drMaster["SUB_DOC_NO"].ToString(), "SMT");
                        double SMTS = (double.Parse(dtSMTS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtSMTS.Rows[0]["DFXPoints"].ToString())
                                    / double.Parse(dtSMTS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtSMTS.Rows[0]["MaxPoints"].ToString())) * 100;
                        if (SMTS == 0)
                        {
                            cells[11, i + 1].PutValue("NA");
                        }
                        else
                        {
                            cells[11, i + 1].PutValue(SMTS.ToString("0.0") + "%");
                        }
                        #endregion

                        #region IE
                        DataTable dtIES = GetDFXScore(drMaster["SUB_DOC_NO"].ToString(), "IE");
                        double IES = (double.Parse(dtIES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtIES.Rows[0]["DFXPoints"].ToString())
                                    / double.Parse(dtIES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtIES.Rows[0]["MaxPoints"].ToString())) * 100;
                        if (IES == 0)
                        {
                            cells[12, i + 1].PutValue("NA");
                        }
                        else
                        {
                            cells[12, i + 1].PutValue(IES.ToString("0.0") + "%");
                        }
                        #endregion

                        #region DQE
                        DataTable dtDQES = GetDFXScore(drMaster["SUB_DOC_NO"].ToString(), "DQE");
                        double DQES = (double.Parse(dtDQES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtDQES.Rows[0]["DFXPoints"].ToString())
                                    / double.Parse(dtDQES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtDQES.Rows[0]["MaxPoints"].ToString())) * 100;
                        if (DQES == 0)
                        {
                            cells[13, i + 1].PutValue("NA");
                        }
                        else
                        {
                            cells[13, i + 1].PutValue(DQES.ToString("0.0") + "%");
                        }
                        #endregion

                        #region EE
                        DataTable dtEES = GetDFXScore(drMaster["SUB_DOC_NO"].ToString(), "EE");
                        double EES = (double.Parse(dtEES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtEES.Rows[0]["DFXPoints"].ToString())
                                    / double.Parse(dtEES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtEES.Rows[0]["MaxPoints"].ToString())) * 100;
                        if (EES == 0)
                        {
                            cells[14, i + 1].PutValue("NA");
                        }
                        else
                        {
                            cells[14, i + 1].PutValue(EES.ToString("0.0") + "%");
                        }
                        #endregion

                        #region UQ
                        DataTable dtUQS = GetDFXScore(drMaster["SUB_DOC_NO"].ToString(), "UQ");
                        double UQS = (double.Parse(dtUQS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtUQS.Rows[0]["DFXPoints"].ToString())
                                    / double.Parse(dtUQS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtUQS.Rows[0]["MaxPoints"].ToString())) * 100;
                        if (UQS == 0)
                        {
                            cells[15, i + 1].PutValue("NA");
                        }
                        else
                        {
                            cells[15, i + 1].PutValue(UQS.ToString("0.0") + "%");
                        }
                        #endregion

                        #region SQ
                        DataTable dtSQS = GetDFXScore(drMaster["SUB_DOC_NO"].ToString(), "SQ");
                        double SQS = (double.Parse(dtSQS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtSQS.Rows[0]["DFXPoints"].ToString())
                                    / double.Parse(dtSQS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtSQS.Rows[0]["MaxPoints"].ToString())) * 100;
                        if (SQS == 0)
                        {
                            cells[16, i + 1].PutValue("NA");
                        }
                        else
                        {
                            cells[16, i + 1].PutValue(SQS.ToString("0.0") + "%");
                        }
                        #endregion

                        #region 統計總分數,確認狀態(PASS/FAIL)
                        double ResultScore = ((AIRIS * 10 + SMTS * 10 + IES * 10 + DQES * 10 + EES * 10 + UQS * 10 + SQS * 10) / divisor) * 100;
                        cells[7, i + 1].PutValue(ResultScore.ToString("0.0") + "%");
                        if (AIRIResult == "FAIL" || SMTResult == "FAIL" || IEResult == "FAIL" || DQEResult == "FAIL" || EEResult == "FAIL" || UQResult == "FAIL" || SQResult == "FAIL")
                        {
                            cells[8, i + 1].PutValue("FAIL");
                        }
                        else
                        {
                            if (ResultScore >= 90)
                            {
                                cells[8, i + 1].PutValue("PASS");
                            }
                            else
                            {
                                cells[8, i + 1].PutValue("FAIL");
                            }

                        }
                        #endregion
                    }
                    #endregion

                    #region PR階段 DFX統計  >=90
                    if (drMaster["SUB_DOC_PHASE"].ToString().Contains("P.Run"))
                    {
                        int LevelEVT = 90;

                        #region AIRI
                        DataTable dtAIRIS = GetDFXScore(drMaster["SUB_DOC_NO"].ToString(), "AI_RI");//計算AI_RI的分數
                        double AIRIS = (double.Parse(dtAIRIS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtAIRIS.Rows[0]["DFXPoints"].ToString())
                                    / double.Parse(dtAIRIS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtAIRIS.Rows[0]["MaxPoints"].ToString())) * 100;
                        if (AIRIS == 0)
                        {
                            cells[10, i + 1].PutValue("NA");
                        }
                        else
                        {
                            cells[10, i + 1].PutValue(AIRIS.ToString("0.0") + "%");
                        }
                        #endregion

                        #region SMT
                        DataTable dtSMTS = GetDFXScore(drMaster["SUB_DOC_NO"].ToString(), "SMT");
                        double SMTS = (double.Parse(dtSMTS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtSMTS.Rows[0]["DFXPoints"].ToString())
                                    / double.Parse(dtSMTS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtSMTS.Rows[0]["MaxPoints"].ToString())) * 100;
                        if (SMTS == 0)
                        {
                            cells[11, i + 1].PutValue("NA");
                        }
                        else
                        {
                            cells[11, i + 1].PutValue(SMTS.ToString("0.0") + "%");
                        }
                        #endregion

                        #region IE
                        DataTable dtIES = GetDFXScore(drMaster["SUB_DOC_NO"].ToString(), "IE");
                        double IES = (double.Parse(dtIES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtIES.Rows[0]["DFXPoints"].ToString())
                                    / double.Parse(dtIES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtIES.Rows[0]["MaxPoints"].ToString())) * 100;
                        if (IES == 0)
                        {
                            cells[12, i + 1].PutValue("NA");
                        }
                        else
                        {
                            cells[12, i + 1].PutValue(IES.ToString("0.0") + "%");
                        }
                        #endregion

                        #region DQE
                        DataTable dtDQES = GetDFXScore(drMaster["SUB_DOC_NO"].ToString(), "DQE");
                        double DQES = (double.Parse(dtDQES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtDQES.Rows[0]["DFXPoints"].ToString())
                                    / double.Parse(dtDQES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtDQES.Rows[0]["MaxPoints"].ToString())) * 100;
                        if (DQES == 0)
                        {
                            cells[13, i + 1].PutValue("NA");
                        }
                        else
                        {
                            cells[13, i + 1].PutValue(DQES.ToString("0.0") + "%");
                        }
                        #endregion

                        #region EE
                        DataTable dtEES = GetDFXScore(drMaster["SUB_DOC_NO"].ToString(), "EE");
                        double EES = (double.Parse(dtEES.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtEES.Rows[0]["DFXPoints"].ToString())
                                    / double.Parse(dtEES.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtEES.Rows[0]["MaxPoints"].ToString())) * 100;
                        if (EES == 0)
                        {
                            cells[14, i + 1].PutValue("NA");
                        }
                        else
                        {
                            cells[14, i + 1].PutValue(EES.ToString("0.0") + "%");
                        }
                        #endregion

                        #region UQ
                        DataTable dtUQS = GetDFXScore(drMaster["SUB_DOC_NO"].ToString(), "UQ");
                        double UQS = (double.Parse(dtUQS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtUQS.Rows[0]["DFXPoints"].ToString())
                                    / double.Parse(dtUQS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtUQS.Rows[0]["MaxPoints"].ToString())) * 100;
                        if (UQS == 0)
                        {
                            cells[15, i + 1].PutValue("NA");
                        }
                        else
                        {
                            cells[15, i + 1].PutValue(UQS.ToString("0.0") + "%");
                        }
                        #endregion

                        #region SQ
                        DataTable dtSQS = GetDFXScore(drMaster["SUB_DOC_NO"].ToString(), "SQ");
                        double SQS = (double.Parse(dtSQS.Rows[0]["DFXPoints"].ToString() == "" ? "0" : dtSQS.Rows[0]["DFXPoints"].ToString())
                                    / double.Parse(dtSQS.Rows[0]["MaxPoints"].ToString() == "" ? "1" : dtSQS.Rows[0]["MaxPoints"].ToString())) * 100;
                        if (SQS == 0)
                        {
                            cells[16, i + 1].PutValue("NA");
                        }
                        else
                        {
                            cells[16, i + 1].PutValue(SQS.ToString("0.0") + "%");
                        }
                        #endregion

                        #region 統計總分數,確認狀態(PASS/FAIL)
                        double ResultScore = ((AIRIS * 10 + SMTS * 10 + IES * 10 + DQES * 10 + EES * 10 + UQS * 10 + SQS * 10) / divisor) * 100;
                        cells[7, i + 1].PutValue(ResultScore.ToString("0.0") + "%");
                        if (AIRIResult == "FAIL" || SMTResult == "FAIL" || IEResult == "FAIL" || DQEResult == "FAIL" || EEResult == "FAIL" || UQResult == "FAIL" || SQResult == "FAIL")
                        {
                            cells[8, i + 1].PutValue("FAIL");
                        }
                        else
                        {
                            if (ResultScore >= 90)
                            {
                                cells[8, i + 1].PutValue("PASS");
                            }
                            else
                            {
                                cells[8, i + 1].PutValue("FAIL");
                            }

                        }
                        #endregion
                    }
                    #endregion

                    #endregion
                }


            }


        }

    }

    #endregion

    #region Create CTQ Excel

    private void CreateExcel_CTQ(string fileName, string docno, string caseID, string Bu, string Building)
    {
        Aspose.Cells.License lic = new Aspose.Cells.License();
        string AsposeLicPath = System.Configuration.ConfigurationSettings.AppSettings["AsposeLicPath"].ToString();
        lic.SetLicense(AsposeLicPath);
        string templatePathCTQ = Page.Server.MapPath("~/Web/E-Report/NPITemplate/CTQ_Report .xlsx");

        DataTable dtMaster = GetMaster(docno);//基本資料
        DataRow drM = dtMaster.Rows[0];
        string Model = drM["MODEL_NAME"].ToString();
        string Stage = drM["SUB_DOC_PHASE"].ToString();

        Aspose.Cells.Workbook book = new Aspose.Cells.Workbook(templatePathCTQ);
        Aspose.Cells.Worksheet sheet0 = book.Worksheets[0];

        BindCTQ(ref sheet0, caseID, Bu, Building, docno);

        #region 檢查導出的Excel文件是否存在
        if (File.Exists(@fileName))//判斷當前路徑文件是否存在,若存在則先刪除在保存  Excel
        {
            File.Delete(@fileName);
            book.Save(fileName, Aspose.Cells.SaveFormat.Xlsx);
        }
        else
        {
            book.Save(fileName, Aspose.Cells.SaveFormat.Xlsx);
        }
        #endregion

        #region 生成壓縮檔后
        string FolderPath = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + "MFG CTQ Doc");
        //Compressedfile(docno, "MFG CTQ Doc");//壓縮指定文件夾
        #endregion

        string strLog = string.Format("Date:{0} CTQ Report has create ok!", System.DateTime.Now.ToString("yyyy-MM-dd"));
        WriteLog(strLog);
    }

    private void BindCTQ(ref Aspose.Cells.Worksheet sheet, string caseID, string Bu, string Building, string DocNo)
    {
        //page 格式設定
        SetStyle(ref sheet, Aspose.Cells.PageOrientationType.Landscape);
        Aspose.Cells.Cells cells = sheet.Cells;
        Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
        Aspose.Cells.Style style = wb.Styles[wb.Styles.Add()];
        //string logoPath = Page.Server.MapPath("") + "\\log.png";
        //sheet.Pictures.Add(0, 0, 4, 10, logoPath);

        NPIMgmt oMgmt = new NPIMgmt("CZ", Bu);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();

        #region 填充基本及資料

        DataTable dtMaster = GetMaster(DocNo);//基本資料
        DataRow drM = dtMaster.Rows[0];

        DataTable dtDept = GetDept(drM["DOC_NO"].ToString());//獲取涉及到的部門 放到數組中
        int len = dtDept.Rows.Count;
        string[] str = new string[len];
        for (int i = 0; i < len; i++)
        {
            str[i] = dtDept.Rows[i]["DEPT"].ToString();
        }
        string TeamMember = string.Join(",", str);

        sheet.Replace("{MODEL_NAME}", drM["MODEL_NAME"].ToString());
        sheet.Replace("{CUSTOMER}", drM["CUSTOMER"].ToString());
        sheet.Replace("{SUB_DOC_PHASE}", drM["SUB_DOC_PHASE"].ToString());
        sheet.Replace("{LINE}", drM["LINE"].ToString());
        sheet.Replace("{LOT_QTY}", drM["LOT_QTY"].ToString());
        sheet.Replace("{PCB_REV}", drM["PCB_REV"].ToString());
        sheet.Replace("{BOM_REV}", drM["BOM_REV"].ToString());
        sheet.Replace("{SPEC_REV}", drM["SPEC_REV"].ToString());
        sheet.Replace("{CUSTOMER_REV}", drM["CUSTOMER_REV"].ToString());
        sheet.Replace("{SUB_DOC_NO}", drM["SUB_DOC_NO"].ToString());
        sheet.Replace("{Team Member}", TeamMember);

        #endregion

        #region 填充TB_NPI_APP_CTQ 所有項目

        DataTable dtCTQ = oStandard.GetCLCAInconformity(DocNo, "", "Finished");
        if (dtCTQ.Rows.Count > 0)
        {
            int templateIndexDFX = 6;//模板row起始位置
            int insertIndexEnCounter = templateIndexDFX + 1;//new row起始位置

            cells.InsertRows(insertIndexEnCounter, dtCTQ.Rows.Count - 1);
            cells.CopyRows(cells, templateIndexDFX, insertIndexEnCounter, dtCTQ.Rows.Count - 1); //複製模板row格式至新行

            for (int i = 0; i < dtCTQ.Rows.Count; i++)
            {

                DataRow dr = dtCTQ.Rows[i];
                cells[i + templateIndexDFX, 0].PutValue(dr["DEPT"].ToString());
                cells[i + templateIndexDFX, 1].PutValue(dr["PROCESS"].ToString());
                cells[i + templateIndexDFX, 2].PutValue(dr["CTQ"].ToString());
                cells[i + templateIndexDFX, 3].PutValue(dr["CONTROL_TYPE"].ToString());
                cells[i + templateIndexDFX, 4].PutValue(dr["GOAL"].ToString());
                cells[i + templateIndexDFX, 5].PutValue(dr["ACT"].ToString());
                cells[i + templateIndexDFX, 6].PutValue(dr["RESULT"].ToString());
                //Fail時樣式變更
                if (dr["RESULT"].ToString() == "Fail")
                {
                    style.Font.Color = System.Drawing.Color.FromArgb(220, 20, 60);
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    style.Font.IsBold = true;
                    style.Font.Size = 14;
                    cells[i + templateIndexDFX, 6].SetStyle(style);
                }
                cells[i + templateIndexDFX, 7].PutValue(dr["DESCRIPTION"].ToString());
                cells[i + templateIndexDFX, 8].PutValue(dr["ROOT_CAUSE"].ToString());
                cells[i + templateIndexDFX, 9].PutValue(dr["D"].ToString());
                cells[i + templateIndexDFX, 10].PutValue(dr["M"].ToString());
                cells[i + templateIndexDFX, 11].PutValue(dr["P"].ToString());
                cells[i + templateIndexDFX, 12].PutValue(dr["E"].ToString());
                cells[i + templateIndexDFX, 13].PutValue(dr["W"].ToString());
                cells[i + templateIndexDFX, 14].PutValue(dr["O"].ToString());
                cells[i + templateIndexDFX, 15].PutValue(dr["TEMPORARY_ACTION"].ToString());
                cells[i + templateIndexDFX, 16].PutValue(dr["CORRECTIVE_PREVENTIVE_ACTION"].ToString());
                cells[i + templateIndexDFX, 17].PutValue(dr["COMPLETE_DATE"].ToString().Length > 0 ? Convert.ToDateTime(dr["COMPLETE_DATE"].ToString()).ToString("yyyy/MM/dd") : dr["COMPLETE_DATE"].ToString());
                cells[i + templateIndexDFX, 18].PutValue(dr["IMPROVEMENT_STATUS"].ToString());

            }


        }

        #endregion

        #region 填充CTQ 簽核人員 PMW PMC NPI
        DataTable dtPM = GetPMPerson(drM["DOC_NO"].ToString()); //PM
        DataTable dtNPI = GetLeader(DocNo, "NPI Leader"); //NPI Leader
        if (dtPM.Rows.Count > 0)
        {
            DataRow drPM = dtPM.Rows[0];
            sheet.Replace("{PM Write}", drPM["WriteEname"].ToString());
            sheet.Replace("{PM Check}", drPM["CheckedEName"].ToString() + ";" + Convert.ToDateTime(drPM["UPDATE_TIME"].ToString()).ToString("yyyy/MM/dd"));
        }
        if (dtNPI.Rows.Count > 0)
        {
            DataRow drNPI = dtNPI.Rows[0];
            sheet.Replace("{NPI Leader}", drNPI["APPROVER"].ToString() + ";" + Convert.ToDateTime(drNPI["APPROVER_DATE"].ToString()).ToString("yyyy/MM/dd"));
        }
        #endregion
    }

    #endregion

    #region Create IssueList Excel

    private void CreateExcel_Issue(string fileName, string docno, string caseID, string Bu, string Building)
    {
        Aspose.Cells.License lic = new Aspose.Cells.License();
        string AsposeLicPath = System.Configuration.ConfigurationSettings.AppSettings["AsposeLicPath"].ToString();
        lic.SetLicense(AsposeLicPath);
        string templatePathIssue = Page.Server.MapPath("~/Web/E-Report/NPITemplate/IssueList_Report.xlsx");

        Aspose.Cells.Workbook book = new Aspose.Cells.Workbook(templatePathIssue);
        Aspose.Cells.Worksheet sheet0 = book.Worksheets[0];
        DataTable dtMaster = GetMaster(docno);//基本資料
        DataRow drM = dtMaster.Rows[0];
        string Model = drM["MODEL_NAME"].ToString();
        string Stage = drM["SUB_DOC_PHASE"].ToString();

        BindIssuesList(ref sheet0, caseID, Bu, Building, docno);

        #region 檢查導出的Excel文件是否存在 導出到Excel文件夾下
        if (File.Exists(@fileName))//判斷當前路徑文件是否存在,若存在則先刪除在保存  Excel
        {
            File.Delete(@fileName);
            book.Save(fileName, Aspose.Cells.SaveFormat.Xlsx);
        }
        else
        {
            book.Save(fileName, Aspose.Cells.SaveFormat.Xlsx);
        }
        #endregion

        #region 生成壓縮檔后 刪除原始檔的文件夾
        string FolderPath = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + "Issue List Doc");
        //Compressedfile(docno, "Issue List Doc");//壓縮指定文件夾
        #endregion

        string strLog = string.Format("Date:{0} Issue List Report has create ok!", System.DateTime.Now.ToString("yyyy-MM-dd"));
        WriteLog(strLog);
    }

    private void BindIssuesList(ref Aspose.Cells.Worksheet sheet, string caseID, string Bu, string Building, string DocNo)
    {
        //page 格式設定
        SetStyle(ref sheet, Aspose.Cells.PageOrientationType.Landscape);
        Aspose.Cells.Cells cells = sheet.Cells;
        //string logoPath = Page.Server.MapPath("") + "\\log.png";
        //sheet.Pictures.Add(0, 0, 4, 10, logoPath);

        NPIMgmt oMgmt = new NPIMgmt("CZ", Bu);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();

        #region 填充基本及資料

        DataTable dtMaster = GetMaster(DocNo);//基本資料
        DataRow drM = dtMaster.Rows[0];

        DataTable dtDept = GetDept(drM["DOC_NO"].ToString());//獲取涉及到的部門 放到數組中
        int len = dtDept.Rows.Count;
        string[] str = new string[len];
        for (int i = 0; i < len; i++)
        {
            str[i] = dtDept.Rows[i]["DEPT"].ToString();
        }
        string TeamMember = string.Join(",", str);

        sheet.Replace("{MODEL_NAME}", drM["MODEL_NAME"].ToString());
        sheet.Replace("{CUSTOMER}", drM["CUSTOMER"].ToString());
        sheet.Replace("{SUB_DOC_PHASE}", drM["SUB_DOC_PHASE"].ToString());
        sheet.Replace("{LINE}", drM["LINE"].ToString());
        sheet.Replace("{LOT_QTY}", drM["LOT_QTY"].ToString());
        sheet.Replace("{PCB_REV}", drM["PCB_REV"].ToString());
        sheet.Replace("{BOM_REV}", drM["BOM_REV"].ToString());
        sheet.Replace("{SPEC_REV}", drM["SPEC_REV"].ToString());
        sheet.Replace("{CUSTOMER_REV}", drM["CUSTOMER_REV"].ToString());
        sheet.Replace("{SUB_DOC_NO}", drM["SUB_DOC_NO"].ToString());
        sheet.Replace("{Team Member}", TeamMember);

        #endregion

        #region 獲取主表資訊
        DataTable dtIssue = oStandard.GetIssuesInconformity(DocNo, "", "");
        if (dtIssue.Rows.Count > 0)
        {
            int templateIndexDFX = 6;//模板row起始位置
            int insertIndexEnCounter = templateIndexDFX + 1;//new row起始位置

            cells.InsertRows(insertIndexEnCounter, dtIssue.Rows.Count - 1);
            cells.CopyRows(cells, templateIndexDFX, insertIndexEnCounter, dtIssue.Rows.Count - 1); //複製模板row格式至新行

            for (int i = 0; i < dtIssue.Rows.Count; i++)
            {

                DataRow dr = dtIssue.Rows[i];
                cells[i + templateIndexDFX, 0].PutValue(dr["DEPT"].ToString());
                cells[i + templateIndexDFX, 1].PutValue(dr["STATION"].ToString());
                cells[i + templateIndexDFX, 2].PutValue(dr["ISSUE_DESCRIPTION"].ToString());
                cells[i + templateIndexDFX, 3].PutValue("");
                cells[i + templateIndexDFX, 4].PutValue(dr["CLASS"].ToString());
                cells[i + templateIndexDFX, 5].PutValue(dr["ISSUE_LOSSES"].ToString());
                cells[i + templateIndexDFX, 6].PutValue(dr["TEMP_MEASURE"].ToString());
                cells[i + templateIndexDFX, 7].PutValue(dr["IMPROVE_MEASURE"].ToString());
                cells[i + templateIndexDFX, 8].PutValue(dr["MEASURE_DEPTREPLY"].ToString());
                cells[i + templateIndexDFX, 9].PutValue(dr["CURRENT_STATUS"].ToString());
                cells[i + templateIndexDFX, 10].PutValue(dr["TRACKING"].ToString());
                if (dr["DUE_DAY"].ToString() == "0001-01-01")
                {
                    cells[i + templateIndexDFX, 11].PutValue("");
                }
                else
                {
                    cells[i + templateIndexDFX, 11].PutValue(dr["DUE_DAY"].ToString().Length > 0 ? dr["DUE_DAY"].ToString() : "");
                }
                cells[i + templateIndexDFX, 12].PutValue(dr["REMARK"].ToString());
            }


        }

        #endregion

        #region 計算Tracking的數量
        DataTable dtO = GetIssueTracking_Count_Open(DocNo);
        DataTable dtC = GetIssueTracking_Count_Closed(DocNo);
        DataTable dtT = GetIssueTracking_Count_Tracking(DocNo);
        string OpenCount = dtO.Rows[0]["O"].ToString();
        string ClosedCount = dtC.Rows[0]["C"].ToString();
        string TrackingCount = dtT.Rows[0]["T"].ToString();

        sheet.Replace("{OpenCount}", OpenCount);
        sheet.Replace("{ClosedCount}", ClosedCount);
        sheet.Replace("{TrackingCount}", TrackingCount);
        #endregion

        #region 填充Issue簽核人員 IEW IEC NPI
        DataTable dtWRC = GetWriteReplyChecked(drM["DOC_NO"].ToString(), DocNo, "IE");
        DataTable dtNPI = GetLeader(DocNo, "NPI Leader"); //NPI Leader
        if (dtWRC.Rows.Count > 0)
        {
            DataRow drWRC = dtWRC.Rows[0];
            sheet.Replace("{IE Write}", drWRC["WriteEname"].ToString());
            sheet.Replace("{IE Check}", drWRC["CheckedEName"].ToString() + ";" + Convert.ToDateTime(drWRC["APPROVER_DATE"].ToString()).ToString("yyyy/MM/dd"));
        }
        if (dtNPI.Rows.Count > 0)
        {
            DataRow drNPI = dtNPI.Rows[0];
            sheet.Replace("{NPI Leader}", drNPI["APPROVER"].ToString() + ";" + Convert.ToDateTime(drNPI["APPROVER_DATE"].ToString()).ToString("yyyy/MM/dd"));
        }
        #endregion

    }

    #endregion

    #region Create FMEAList Excel

    private void CreateExcel_FMEA(string fileName, string docno, string caseID, string Bu, string Building)
    {
        Aspose.Cells.License lic = new Aspose.Cells.License();
        string AsposeLicPath = System.Configuration.ConfigurationSettings.AppSettings["AsposeLicPath"].ToString();
        lic.SetLicense(AsposeLicPath);
        string templatePathFMEA = Page.Server.MapPath("~/Web/E-Report/NPITemplate/P-FMEA_Report.xlsx");

        Aspose.Cells.Workbook book = new Aspose.Cells.Workbook(templatePathFMEA);
        Aspose.Cells.Worksheet sheet0 = book.Worksheets[0];

        BindPFMA(ref sheet0, caseID, Bu, Building, docno);

        #region 檢查導出的Excel文件是否存在
        if (File.Exists(@fileName))//判斷當前路徑文件是否存在,若存在則先刪除在保存  Excel
        {
            File.Delete(@fileName);
            book.Save(fileName, Aspose.Cells.SaveFormat.Xlsx);
        }
        else
        {
            book.Save(fileName, Aspose.Cells.SaveFormat.Xlsx);
        }
        #endregion

        #region 生成壓縮檔后 刪除原始檔的文件夾
        DataTable dtMaster = GetMaster(docno);//基本資料
        DataRow drM = dtMaster.Rows[0];
        string Model = drM["MODEL_NAME"].ToString();
        string Stage = drM["SUB_DOC_PHASE"].ToString();
        string FolderPath = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + "FMEA List Doc");
        //Compressedfile(docno, "FMEA List Doc");//壓縮指定文件夾
        #endregion


        string strLog = string.Format("Date:{0} FMEA List Report has create ok!", System.DateTime.Now.ToString("yyyy-MM-dd"));
        WriteLog(strLog);
    }

    private void BindPFMA(ref Aspose.Cells.Worksheet sheet, string caseID, string Bu, string Building, string DocNo)
    {
        //page 格式設定
        SetStyle(ref sheet, Aspose.Cells.PageOrientationType.Landscape);
        Aspose.Cells.Cells cells = sheet.Cells;
        //string logoPath = Page.Server.MapPath("") + "\\log.png";
        //sheet.Pictures.Add(0, 0, 4, 10, logoPath);

        NPIMgmt oMgmt = new NPIMgmt("CZ", Bu);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();

        #region 填充基本及資料

        DataTable dtMaster = GetMaster(DocNo);//基本資料
        DataRow drM = dtMaster.Rows[0];

        DataTable dtDept = GetDept(drM["DOC_NO"].ToString());//獲取涉及到的部門 放到數組中
        int len = dtDept.Rows.Count;
        string[] str = new string[len];
        for (int i = 0; i < len; i++)
        {
            str[i] = dtDept.Rows[i]["DEPT"].ToString();
        }
        string TeamMember = string.Join(",", str);

        sheet.Replace("{MODEL_NAME}", drM["MODEL_NAME"].ToString());
        sheet.Replace("{CUSTOMER}", drM["CUSTOMER"].ToString());
        sheet.Replace("{SUB_DOC_PHASE}", drM["SUB_DOC_PHASE"].ToString());
        sheet.Replace("{LINE}", drM["LINE"].ToString());
        sheet.Replace("{LOT_QTY}", drM["LOT_QTY"].ToString());
        sheet.Replace("{PCB_REV}", drM["PCB_REV"].ToString());
        sheet.Replace("{BOM_REV}", drM["BOM_REV"].ToString());
        sheet.Replace("{SPEC_REV}", drM["SPEC_REV"].ToString());
        sheet.Replace("{CUSTOMER_REV}", drM["CUSTOMER_REV"].ToString());
        sheet.Replace("{SUB_DOC_NO}", drM["SUB_DOC_NO"].ToString());
        sheet.Replace("{Team Member}", TeamMember);

        #endregion

        #region 獲取主表資訊
        DataTable dtFMEA = oStandard.GetFMEAInconformity(DocNo, "", "", "");

        if (dtFMEA.Rows.Count > 0)
        {
            int templateIndexDFX = 6;//模板row起始位置
            int insertIndexEnCounter = templateIndexDFX + 1;//new row起始位置

            cells.InsertRows(insertIndexEnCounter, dtFMEA.Rows.Count - 1);
            cells.CopyRows(cells, templateIndexDFX, insertIndexEnCounter, dtFMEA.Rows.Count - 1); //複製模板row格式至新行

            for (int i = 0; i < dtFMEA.Rows.Count; i++)
            {

                DataRow dr = dtFMEA.Rows[i];
                cells[i + templateIndexDFX, 0].PutValue(dr["WriteDept"].ToString());
                cells[i + templateIndexDFX, 1].PutValue(dr["Stantion"].ToString());
                cells[i + templateIndexDFX, 2].PutValue(dr["Source"].ToString());
                cells[i + templateIndexDFX, 3].PutValue(dr["PotentialFailureMode"].ToString());
                cells[i + templateIndexDFX, 4].PutValue(dr["Loess"].ToString());
                cells[i + templateIndexDFX, 5].PutValue(dr["Sev"].ToString());
                cells[i + templateIndexDFX, 6].PutValue(dr["Occ"].ToString());
                cells[i + templateIndexDFX, 7].PutValue(dr["DET"].ToString());
                cells[i + templateIndexDFX, 8].PutValue(dr["RPN"].ToString());
                cells[i + templateIndexDFX, 9].PutValue(dr["PotentialFailure"].ToString());
                cells[i + templateIndexDFX, 10].PutValue(dr["ActionsTaken"].ToString());
                cells[i + templateIndexDFX, 11].PutValue(dr["ResultsSev"].ToString());
                cells[i + templateIndexDFX, 12].PutValue(dr["ResultsOcc"].ToString());
                cells[i + templateIndexDFX, 13].PutValue(dr["ResultsDet"].ToString());
                cells[i + templateIndexDFX, 14].PutValue(dr["ResultsRPN"].ToString());
                cells[i + templateIndexDFX, 15].PutValue(dr["Resposibility"].ToString());
                cells[i + templateIndexDFX, 16].PutValue(dr["TargetCompletionDate"].ToString().Length > 0 ? Convert.ToDateTime(dr["TargetCompletionDate"].ToString()).ToString("yyyy/MM/dd") : dr["TargetCompletionDate"].ToString());

            }


        }

        #endregion

        #region 計算FMEA Tracking的數量

        DataTable dtO = GetFMEATracking_Count_Open(DocNo);
        DataTable dtC = GetFMWATracking_Count_Closed(DocNo);
        DataTable dtT = GetFMEATracking_Count_Tracking(DocNo);
        DataTable dtR = GetFMEATracking_Count_RPN(DocNo);
        string OpenCount = dtO.Rows[0]["O"].ToString();
        string ClosedCount = dtC.Rows[0]["C"].ToString();
        string TrackingCount = dtT.Rows[0]["T"].ToString();
        string RPNCount = dtR.Rows[0]["R"].ToString();

        sheet.Replace("{OpenCount}", OpenCount);
        sheet.Replace("{ClosedCount}", ClosedCount);
        sheet.Replace("{TrackingCount}", TrackingCount);
        sheet.Replace("{ImpCount}", RPNCount);

        #endregion

        #region 填充FMEA簽核人員 IEW IEC NPI
        DataTable dtWRC = GetWriteReplyChecked(drM["DOC_NO"].ToString(), DocNo, "IE");
        DataTable dtNPI = GetLeader(DocNo, "NPI Leader"); //NPI Leader
        if (dtWRC.Rows.Count > 0)
        {
            DataRow drWRC = dtWRC.Rows[0];
            sheet.Replace("{IE Write}", drWRC["WriteEname"].ToString());
            sheet.Replace("{IE Check}", drWRC["CheckedEName"].ToString() + ";" + Convert.ToDateTime(drWRC["APPROVER_DATE"].ToString()).ToString("yyyy/MM/dd"));
        }
        if (dtNPI.Rows.Count > 0)
        {
            DataRow drNPI = dtNPI.Rows[0];
            sheet.Replace("{NPI Leader}", drNPI["APPROVER"].ToString() + ";" + Convert.ToDateTime(drNPI["APPROVER_DATE"].ToString()).ToString("yyyy/MM/dd"));
        }
        #endregion

    }
    #endregion

    #region DFX 子目錄

    #region DFX SMT
    private void CreateExcel_DFXSMT(string fileName, string docno, string caseID, string Bu, string Building)
    {
        Aspose.Cells.License lic = new Aspose.Cells.License();
        string AsposeLicPath = System.Configuration.ConfigurationSettings.AppSettings["AsposeLicPath"].ToString();
        lic.SetLicense(AsposeLicPath);
        string templatePath = Page.Server.MapPath("~/Web/E-Report/NPITemplate/SMT_DFX.xlsx");

        Aspose.Cells.Workbook book = new Aspose.Cells.Workbook(templatePath);
        Aspose.Cells.Worksheet sheet0 = book.Worksheets[0];

        BindDFXSMT(ref sheet0, caseID, Bu, Building, docno);

        if (File.Exists(@fileName))//判斷當前路徑文件是否存在,若存在則先刪除在保存
        {
            File.Delete(@fileName);
            book.Save(fileName, Aspose.Cells.SaveFormat.Xlsx);
        }
        else
        {
            book.Save(fileName, Aspose.Cells.SaveFormat.Xlsx);
        }

        string strLog = string.Format("Date:{0} DFX Report has create ok!", System.DateTime.Now.ToString("yyyy-MM-dd"));
        WriteLog(strLog);
    }

    private void BindDFXSMT(ref Aspose.Cells.Worksheet sheet, string caseID, string Bu, string Building, string DocNo)
    {
        //page 格式設定
        SetStyle(ref sheet, Aspose.Cells.PageOrientationType.Landscape);
        Aspose.Cells.Cells cells = sheet.Cells;
        //string logoPath = Page.Server.MapPath("") + "\\log.png";
        //sheet.Pictures.Add(0, 0, 4, 10, logoPath);

        NPIMgmt oMgmt = new NPIMgmt("CZ", Bu);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();

        #region 填充基本及資料

        DataTable dtMaster = GetMaster(DocNo);//基本資料
        DataRow drM = dtMaster.Rows[0];
        DataTable dtMaxPoints = SUMMaxPoints(DocNo, "SMT");
        DataTable dtDFXPoints = SUMDFXPoints(DocNo, "SMT");
        int PossiblePoints = int.Parse(dtMaxPoints.Rows[0]["PossiblePoints"].ToString() == "" ? "0" : dtMaxPoints.Rows[0]["PossiblePoints"].ToString());
        int CompliancePoints = int.Parse(dtDFXPoints.Rows[0]["CompliancePoints"].ToString() == "" ? "0" : dtDFXPoints.Rows[0]["CompliancePoints"].ToString());
        if (PossiblePoints == 0 || CompliancePoints == 0)
        {
            cells[4, 8].PutValue(0);

        }
        else
        {
            double DFXScore = CompliancePoints / PossiblePoints;
            cells[4, 8].PutValue(DFXScore);

        }
        cells[4, 1].PutValue(PossiblePoints);
        cells[4, 4].PutValue(CompliancePoints);

        sheet.Replace("{Dept}", "SMT");
        sheet.Replace("{MODEL_NAME}", drM["MODEL_NAME"].ToString());
        sheet.Replace("{CUSTOMER}", drM["CUSTOMER"].ToString());
        sheet.Replace("{SUB_DOC_PHASE}", drM["SUB_DOC_PHASE"].ToString());
        sheet.Replace("{PCB_REV}", drM["PCB_REV"].ToString());
        sheet.Replace("{BOM_REV}", drM["BOM_REV"].ToString());
        sheet.Replace("{SUB_DOC_NO}", drM["SUB_DOC_NO"].ToString());

        #endregion

        #region//獲取主表資訊
        DataTable dtDFXSMT = GetDFXByDept(DocNo, "SMT");//主表資料
        if (dtDFXSMT.Rows.Count > 0)
        {
            sheet.Replace("{ReviewDate}", Convert.ToDateTime(dtDFXSMT.Rows[0]["UpdateTime"].ToString()).ToString("yyyy/MM/dd"));
            int templateIndexDFX = 6;//模板row起始位置
            int insertIndexEnCounter = templateIndexDFX + 1;//new row起始位置

            cells.InsertRows(insertIndexEnCounter, dtDFXSMT.Rows.Count - 1);
            cells.CopyRows(cells, templateIndexDFX, insertIndexEnCounter, dtDFXSMT.Rows.Count - 1); //複製模板row格式至新行

            for (int i = 0; i < dtDFXSMT.Rows.Count; i++)
            {

                DataRow dr = dtDFXSMT.Rows[i];
                cells[i + templateIndexDFX, 0].PutValue(dr["Item"].ToString());
                cells[i + templateIndexDFX, 1].PutValue(dr["ItemType"].ToString());
                cells[i + templateIndexDFX, 2].PutValue(dr["ItemName"].ToString());
                cells[i + templateIndexDFX, 3].PutValue(dr["Requirements"].ToString());
                cells[i + templateIndexDFX, 4].PutValue("");
                cells[i + templateIndexDFX, 5].PutValue(dr["Losses"].ToString());
                cells[i + templateIndexDFX, 6].PutValue(dr["Location"].ToString());
                cells[i + templateIndexDFX, 7].PutValue(dr["PriorityLevel"].ToString());
                cells[i + templateIndexDFX, 8].PutValue(dr["MaxPoints"].ToString() == "NA" || dr["MaxPoints"].ToString() == "" ? 0 : int.Parse(dr["MaxPoints"].ToString()));
                cells[i + templateIndexDFX, 9].PutValue(dr["DFXPoints"].ToString() == "NA" || dr["MaxPoints"].ToString() == "" ? 0 : int.Parse(dr["DFXPoints"].ToString()));
                cells[i + templateIndexDFX, 10].PutValue(dr["Compliance"].ToString());
                cells[i + templateIndexDFX, 11].PutValue(dr["Comments"].ToString());
            }
        }
        else
        {
            sheet.Replace("{ReviewDate}", "");
        }

        #endregion

        #region 签核人员
        DataTable dtResultW = GetDFXWrite(drM["DOC_NO"].ToString(), "SMT");
        DataTable dtResultC = GetDFXCheck(drM["DOC_NO"].ToString(), "SMT", drM["SUB_DOC_NO"].ToString());
        if (dtResultW.Rows.Count > 0)
        {
            sheet.Replace("{PM Write}", dtResultW.Rows[0]["WriteEname"].ToString());
        }
        else
        {
            sheet.Replace("{PM Write}", "");
        }

        if (dtResultC.Rows.Count > 0)
        {
            sheet.Replace("{PM Check}", dtResultC.Rows[0]["APPROVER"].ToString() + ";" + Convert.ToDateTime(dtResultC.Rows[0]["APPROVER_DATE"].ToString()).ToString("yyyy/MM/dd hh:mm"));
        }
        else
        {
            sheet.Replace("{PM Check}", "");
        }

        #endregion
    }
    #endregion

    #region DFX AIRI
    private void CreateExcel_DFXAIRI(string fileName, string docno, string caseID, string Bu, string Building)
    {
        Aspose.Cells.License lic = new Aspose.Cells.License();
        string AsposeLicPath = System.Configuration.ConfigurationSettings.AppSettings["AsposeLicPath"].ToString();
        lic.SetLicense(AsposeLicPath);
        string templatePath = Page.Server.MapPath("~/Web/E-Report/NPITemplate/SMT_DFX.xlsx");

        Aspose.Cells.Workbook book = new Aspose.Cells.Workbook(templatePath);
        Aspose.Cells.Worksheet sheet0 = book.Worksheets[0];

        BindDFXAIRI(ref sheet0, caseID, Bu, Building, docno);

        if (File.Exists(@fileName))//判斷當前路徑文件是否存在,若存在則先刪除在保存
        {
            File.Delete(@fileName);
            book.Save(fileName, Aspose.Cells.SaveFormat.Xlsx);
        }
        else
        {
            book.Save(fileName, Aspose.Cells.SaveFormat.Xlsx);
        }

        string strLog = string.Format("Date:{0} DFX Report has create ok!", System.DateTime.Now.ToString("yyyy-MM-dd"));
        WriteLog(strLog);
    }

    private void BindDFXAIRI(ref Aspose.Cells.Worksheet sheet, string caseID, string Bu, string Building, string DocNo)
    {
        //page 格式設定
        SetStyle(ref sheet, Aspose.Cells.PageOrientationType.Landscape);
        Aspose.Cells.Cells cells = sheet.Cells;
        //string logoPath = Page.Server.MapPath("") + "\\log.png";
        //sheet.Pictures.Add(0, 0, 4, 10, logoPath);

        NPIMgmt oMgmt = new NPIMgmt("CZ", Bu);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();

        #region 填充基本及資料

        DataTable dtMaster = GetMaster(DocNo);//基本資料
        DataRow drM = dtMaster.Rows[0];

        DataTable dtMaxPoints = SUMMaxPoints(DocNo, "AI_RI");
        DataTable dtDFXPoints = SUMDFXPoints(DocNo, "AI_RI");
        int PossiblePoints = int.Parse(dtMaxPoints.Rows[0]["PossiblePoints"].ToString() == "" ? "0" : dtMaxPoints.Rows[0]["PossiblePoints"].ToString());
        int CompliancePoints = int.Parse(dtDFXPoints.Rows[0]["CompliancePoints"].ToString() == "" ? "0" : dtDFXPoints.Rows[0]["CompliancePoints"].ToString());
        if (PossiblePoints == 0 || CompliancePoints == 0)
        {
            cells[4, 8].PutValue(0);

        }
        else
        {
            double DFXScore = CompliancePoints / PossiblePoints;
            cells[4, 8].PutValue(DFXScore);

        }
        cells[4, 1].PutValue(PossiblePoints);
        cells[4, 4].PutValue(CompliancePoints);


        sheet.Replace("{Dept}", "AIRI");
        sheet.Replace("{MODEL_NAME}", drM["MODEL_NAME"].ToString());
        sheet.Replace("{CUSTOMER}", drM["CUSTOMER"].ToString());
        sheet.Replace("{SUB_DOC_PHASE}", drM["SUB_DOC_PHASE"].ToString());
        sheet.Replace("{PCB_REV}", drM["PCB_REV"].ToString());
        sheet.Replace("{BOM_REV}", drM["BOM_REV"].ToString());
        sheet.Replace("{SUB_DOC_NO}", drM["SUB_DOC_NO"].ToString());
        #endregion

        #region 獲取主表資訊
        DataTable dtDFXAIRI = GetDFXByDept(DocNo, "AI_RI");
        if (dtDFXAIRI.Rows.Count > 0)
        {
            sheet.Replace("{ReviewDate}", Convert.ToDateTime(dtDFXAIRI.Rows[0]["UpdateTime"].ToString()).ToString("yyyy/MM/dd"));
            int templateIndexDFX = 6;//模板row起始位置
            int insertIndexEnCounter = templateIndexDFX + 1;//new row起始位置

            cells.InsertRows(insertIndexEnCounter, dtDFXAIRI.Rows.Count - 1);
            cells.CopyRows(cells, templateIndexDFX, insertIndexEnCounter, dtDFXAIRI.Rows.Count - 1); //複製模板row格式至新行

            for (int i = 0; i < dtDFXAIRI.Rows.Count; i++)
            {

                DataRow dr = dtDFXAIRI.Rows[i];
                cells[i + templateIndexDFX, 0].PutValue(dr["Item"].ToString());
                cells[i + templateIndexDFX, 1].PutValue(dr["ItemType"].ToString());
                cells[i + templateIndexDFX, 2].PutValue(dr["ItemName"].ToString());
                cells[i + templateIndexDFX, 3].PutValue(dr["Requirements"].ToString());
                cells[i + templateIndexDFX, 4].PutValue("");
                cells[i + templateIndexDFX, 5].PutValue(dr["Losses"].ToString());
                cells[i + templateIndexDFX, 6].PutValue(dr["Location"].ToString());
                cells[i + templateIndexDFX, 7].PutValue(dr["PriorityLevel"].ToString());
                cells[i + templateIndexDFX, 8].PutValue(dr["MaxPoints"].ToString() == "NA" || dr["MaxPoints"].ToString() == "" ? 0 : int.Parse(dr["MaxPoints"].ToString()));
                cells[i + templateIndexDFX, 9].PutValue(dr["DFXPoints"].ToString() == "NA" || dr["MaxPoints"].ToString() == "" ? 0 : int.Parse(dr["DFXPoints"].ToString()));
                cells[i + templateIndexDFX, 10].PutValue(dr["Compliance"].ToString());
                cells[i + templateIndexDFX, 11].PutValue(dr["Comments"].ToString());
            }


        }
        else
        {
            sheet.Replace("{ReviewDate}", "");
        }

        #endregion

        #region 签核人员
        DataTable dtResultW = GetDFXWrite(drM["DOC_NO"].ToString(), "AI_RI");
        DataTable dtResultC = GetDFXCheck(drM["DOC_NO"].ToString(), "AI_RI", drM["SUB_DOC_NO"].ToString());
        if (dtResultW.Rows.Count > 0)
        {
            sheet.Replace("{PM Write}", dtResultW.Rows[0]["WriteEname"].ToString());
        }
        else
        {
            sheet.Replace("{PM Write}", "");
        }

        if (dtResultC.Rows.Count > 0)
        {
            sheet.Replace("{PM Check}", dtResultC.Rows[0]["APPROVER"].ToString() + ";" + Convert.ToDateTime(dtResultC.Rows[0]["APPROVER_DATE"].ToString()).ToString("yyyy/MM/dd hh:mm"));
        }
        else
        {
            sheet.Replace("{PM Check}", "");
        }

        #endregion
    }
    #endregion

    #region DFX AUTO
    private void CreateExcel_DFXAUTO(string fileName, string docno, string caseID, string Bu, string Building)
    {
        Aspose.Cells.License lic = new Aspose.Cells.License();
        string AsposeLicPath = System.Configuration.ConfigurationSettings.AppSettings["AsposeLicPath"].ToString();
        lic.SetLicense(AsposeLicPath);
        string templatePath = Page.Server.MapPath("~/Web/E-Report/NPITemplate/SMT_DFX.xlsx");

        Aspose.Cells.Workbook book = new Aspose.Cells.Workbook(templatePath);
        Aspose.Cells.Worksheet sheet0 = book.Worksheets[0];

        BindDFXAUTO(ref sheet0, caseID, Bu, Building, docno);

        if (File.Exists(@fileName))//判斷當前路徑文件是否存在,若存在則先刪除在保存
        {
            File.Delete(@fileName);
            book.Save(fileName, Aspose.Cells.SaveFormat.Xlsx);
        }
        else
        {
            book.Save(fileName, Aspose.Cells.SaveFormat.Xlsx);
        }

        string strLog = string.Format("Date:{0} AUTO Report has create ok!", System.DateTime.Now.ToString("yyyy-MM-dd"));
        WriteLog(strLog);
    }

    private void BindDFXAUTO(ref Aspose.Cells.Worksheet sheet, string caseID, string Bu, string Building, string DocNo)
    {
        //page 格式設定
        SetStyle(ref sheet, Aspose.Cells.PageOrientationType.Landscape);
        Aspose.Cells.Cells cells = sheet.Cells;
        //string logoPath = Page.Server.MapPath("") + "\\log.png";
        //sheet.Pictures.Add(0, 0, 4, 10, logoPath);

        NPIMgmt oMgmt = new NPIMgmt("CZ", Bu);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();

        #region 填充基本及資料

        DataTable dtMaster = GetMaster(DocNo);//基本資料
        DataRow drM = dtMaster.Rows[0];
        DataTable dtMaxPoints = SUMMaxPoints(DocNo, "AUTO");
        DataTable dtDFXPoints = SUMDFXPoints(DocNo, "AUTO");
        int PossiblePoints = int.Parse(dtMaxPoints.Rows[0]["PossiblePoints"].ToString() == "" ? "0" : dtMaxPoints.Rows[0]["PossiblePoints"].ToString());
        int CompliancePoints = int.Parse(dtDFXPoints.Rows[0]["CompliancePoints"].ToString() == "" ? "0" : dtDFXPoints.Rows[0]["CompliancePoints"].ToString());
        if (PossiblePoints == 0 || CompliancePoints == 0)
        {
            cells[4, 8].PutValue(0);

        }
        else
        {
            double DFXScore = CompliancePoints / PossiblePoints;
            cells[4, 8].PutValue(DFXScore);

        }
        cells[4, 1].PutValue(PossiblePoints);
        cells[4, 4].PutValue(CompliancePoints);


        sheet.Replace("{Dept}", "AUTO");
        sheet.Replace("{MODEL_NAME}", drM["MODEL_NAME"].ToString());
        sheet.Replace("{CUSTOMER}", drM["CUSTOMER"].ToString());
        sheet.Replace("{SUB_DOC_PHASE}", drM["SUB_DOC_PHASE"].ToString());
        sheet.Replace("{PCB_REV}", drM["PCB_REV"].ToString());
        sheet.Replace("{BOM_REV}", drM["BOM_REV"].ToString());
        sheet.Replace("{SUB_DOC_NO}", drM["SUB_DOC_NO"].ToString());
        #endregion

        #region 獲取主表資訊
        DataTable dtDFXAUTO = GetDFXByDept(DocNo, "AUTO");
        if (dtDFXAUTO.Rows.Count > 0)
        {
            sheet.Replace("{ReviewDate}", Convert.ToDateTime(dtDFXAUTO.Rows[0]["UpdateTime"].ToString()).ToString("yyyy/MM/dd"));
            int templateIndexDFX = 6;//模板row起始位置
            int insertIndexEnCounter = templateIndexDFX + 1;//new row起始位置

            cells.InsertRows(insertIndexEnCounter, dtDFXAUTO.Rows.Count - 1);
            cells.CopyRows(cells, templateIndexDFX, insertIndexEnCounter, dtDFXAUTO.Rows.Count - 1); //複製模板row格式至新行

            for (int i = 0; i < dtDFXAUTO.Rows.Count; i++)
            {

                DataRow dr = dtDFXAUTO.Rows[i];
                cells[i + templateIndexDFX, 0].PutValue(dr["Item"].ToString());
                cells[i + templateIndexDFX, 1].PutValue(dr["ItemType"].ToString());
                cells[i + templateIndexDFX, 2].PutValue(dr["ItemName"].ToString());
                cells[i + templateIndexDFX, 3].PutValue(dr["Requirements"].ToString());
                cells[i + templateIndexDFX, 4].PutValue("");
                cells[i + templateIndexDFX, 5].PutValue(dr["Losses"].ToString());
                cells[i + templateIndexDFX, 6].PutValue(dr["Location"].ToString());
                cells[i + templateIndexDFX, 7].PutValue(dr["PriorityLevel"].ToString());
                cells[i + templateIndexDFX, 8].PutValue(dr["MaxPoints"].ToString() == "NA" || dr["MaxPoints"].ToString() == "" ? 0 : int.Parse(dr["MaxPoints"].ToString()));
                cells[i + templateIndexDFX, 9].PutValue(dr["DFXPoints"].ToString() == "NA" || dr["MaxPoints"].ToString() == "" ? 0 : int.Parse(dr["DFXPoints"].ToString()));
                cells[i + templateIndexDFX, 10].PutValue(dr["Compliance"].ToString());
                cells[i + templateIndexDFX, 11].PutValue(dr["Comments"].ToString());
            }


        }
        else
        {
            sheet.Replace("{ReviewDate}", "");
        }
        #endregion

        #region 签核人员
        DataTable dtResultW = GetDFXWrite(drM["DOC_NO"].ToString(), "AUTO");
        DataTable dtResultC = GetDFXCheck(drM["DOC_NO"].ToString(), "AUTO", drM["SUB_DOC_NO"].ToString());
        if (dtResultW.Rows.Count > 0)
        {
            sheet.Replace("{PM Write}", dtResultW.Rows[0]["WriteEname"].ToString());
        }
        else
        {
            sheet.Replace("{PM Write}", "");
        }

        if (dtResultC.Rows.Count > 0)
        {
            sheet.Replace("{PM Check}", dtResultC.Rows[0]["APPROVER"].ToString() + ";" + Convert.ToDateTime(dtResultC.Rows[0]["APPROVER_DATE"].ToString()).ToString("yyyy/MM/dd hh:mm"));
        }
        else
        {
            sheet.Replace("{PM Check}", "");
        }

        #endregion
    }
    #endregion

    #region DFX DQE
    private void CreateExcel_DFXDQE(string fileName, string docno, string caseID, string Bu, string Building)
    {
        Aspose.Cells.License lic = new Aspose.Cells.License();
        string AsposeLicPath = System.Configuration.ConfigurationSettings.AppSettings["AsposeLicPath"].ToString();
        lic.SetLicense(AsposeLicPath);
        string templatePath = Page.Server.MapPath("~/Web/E-Report/NPITemplate/SMT_DFX.xlsx");

        Aspose.Cells.Workbook book = new Aspose.Cells.Workbook(templatePath);
        Aspose.Cells.Worksheet sheet0 = book.Worksheets[0];

        BindDFXDQE(ref sheet0, caseID, Bu, Building, docno);

        if (File.Exists(@fileName))//判斷當前路徑文件是否存在,若存在則先刪除在保存
        {
            File.Delete(@fileName);
            book.Save(fileName, Aspose.Cells.SaveFormat.Xlsx);
        }
        else
        {
            book.Save(fileName, Aspose.Cells.SaveFormat.Xlsx);
        }

        string strLog = string.Format("Date:{0} DFX Report has create ok!", System.DateTime.Now.ToString("yyyy-MM-dd"));
        WriteLog(strLog);
    }

    private void BindDFXDQE(ref Aspose.Cells.Worksheet sheet, string caseID, string Bu, string Building, string DocNo)
    {
        //page 格式設定
        SetStyle(ref sheet, Aspose.Cells.PageOrientationType.Landscape);
        Aspose.Cells.Cells cells = sheet.Cells;
        //string logoPath = Page.Server.MapPath("") + "\\log.png";
        //sheet.Pictures.Add(0, 0, 4, 10, logoPath);

        NPIMgmt oMgmt = new NPIMgmt("CZ", Bu);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();

        #region 填充基本及資料

        DataTable dtMaster = GetMaster(DocNo);//基本資料
        DataRow drM = dtMaster.Rows[0];
        DataTable dtMaxPoints = SUMMaxPoints(DocNo, "DQE");
        DataTable dtDFXPoints = SUMDFXPoints(DocNo, "DQE");
        int PossiblePoints = int.Parse(dtMaxPoints.Rows[0]["PossiblePoints"].ToString() == "" ? "0" : dtMaxPoints.Rows[0]["PossiblePoints"].ToString());
        int CompliancePoints = int.Parse(dtDFXPoints.Rows[0]["CompliancePoints"].ToString() == "" ? "0" : dtDFXPoints.Rows[0]["CompliancePoints"].ToString());
        if (PossiblePoints == 0 || CompliancePoints == 0)
        {
            cells[4, 8].PutValue(0);

        }
        else
        {
            double DFXScore = CompliancePoints / PossiblePoints;
            cells[4, 8].PutValue(DFXScore);

        }
        cells[4, 1].PutValue(PossiblePoints);
        cells[4, 4].PutValue(CompliancePoints);

        sheet.Replace("{Dept}", "DQE");
        sheet.Replace("{MODEL_NAME}", drM["MODEL_NAME"].ToString());
        sheet.Replace("{CUSTOMER}", drM["CUSTOMER"].ToString());
        sheet.Replace("{SUB_DOC_PHASE}", drM["SUB_DOC_PHASE"].ToString());
        sheet.Replace("{PCB_REV}", drM["PCB_REV"].ToString());
        sheet.Replace("{BOM_REV}", drM["BOM_REV"].ToString());
        sheet.Replace("{SUB_DOC_NO}", drM["SUB_DOC_NO"].ToString());
        #endregion

        #region//獲取主表資訊
        DataTable dtDFXDQE = GetDFXByDept(DocNo, "DQE");
        if (dtDFXDQE.Rows.Count > 0)
        {
            int templateIndexDFX = 6;//模板row起始位置
            int insertIndexEnCounter = templateIndexDFX + 1;//new row起始位置

            cells.InsertRows(insertIndexEnCounter, dtDFXDQE.Rows.Count - 1);
            cells.CopyRows(cells, templateIndexDFX, insertIndexEnCounter, dtDFXDQE.Rows.Count - 1); //複製模板row格式至新行

            for (int i = 0; i < dtDFXDQE.Rows.Count; i++)
            {
                sheet.Replace("{ReviewDate}", Convert.ToDateTime(dtDFXDQE.Rows[0]["UpdateTime"].ToString()).ToString("yyyy/MM/dd"));
                DataRow dr = dtDFXDQE.Rows[i];
                cells[i + templateIndexDFX, 0].PutValue(dr["Item"].ToString());
                cells[i + templateIndexDFX, 1].PutValue(dr["ItemType"].ToString());
                cells[i + templateIndexDFX, 2].PutValue(dr["ItemName"].ToString());
                cells[i + templateIndexDFX, 3].PutValue(dr["Requirements"].ToString());
                cells[i + templateIndexDFX, 4].PutValue("");
                cells[i + templateIndexDFX, 5].PutValue(dr["Losses"].ToString());
                cells[i + templateIndexDFX, 6].PutValue(dr["Location"].ToString());
                cells[i + templateIndexDFX, 7].PutValue(dr["PriorityLevel"].ToString());
                cells[i + templateIndexDFX, 8].PutValue(dr["MaxPoints"].ToString() == "NA" || dr["MaxPoints"].ToString() == "" ? 0 : int.Parse(dr["MaxPoints"].ToString()));
                cells[i + templateIndexDFX, 9].PutValue(dr["DFXPoints"].ToString() == "NA" || dr["MaxPoints"].ToString() == "" ? 0 : int.Parse(dr["DFXPoints"].ToString()));
                cells[i + templateIndexDFX, 10].PutValue(dr["Compliance"].ToString());
                cells[i + templateIndexDFX, 11].PutValue(dr["Comments"].ToString());
            }


        }
        else
        {
            sheet.Replace("{ReviewDate}", "");
        }
        #endregion

        #region 签核人员
        DataTable dtResultW = GetDFXWrite(drM["DOC_NO"].ToString(), "DQE");
        DataTable dtResultC = GetDFXCheck(drM["DOC_NO"].ToString(), "DQE", drM["SUB_DOC_NO"].ToString());
        if (dtResultW.Rows.Count > 0)
        {
            sheet.Replace("{PM Write}", dtResultW.Rows[0]["WriteEname"].ToString());
        }
        else
        {
            sheet.Replace("{PM Write}", "");
        }

        if (dtResultC.Rows.Count > 0)
        {
            sheet.Replace("{PM Check}", dtResultC.Rows[0]["APPROVER"].ToString() + ";" + Convert.ToDateTime(dtResultC.Rows[0]["APPROVER_DATE"].ToString()).ToString("yyyy/MM/dd hh:mm"));
        }
        else
        {
            sheet.Replace("{PM Check}", "");
        }

        #endregion
    }
    #endregion

    #region DFX Safety
    private void CreateExcel_DFXSafety(string fileName, string docno, string caseID, string Bu, string Building)
    {
        Aspose.Cells.License lic = new Aspose.Cells.License();
        string AsposeLicPath = System.Configuration.ConfigurationSettings.AppSettings["AsposeLicPath"].ToString();
        lic.SetLicense(AsposeLicPath);
        string templatePath = Page.Server.MapPath("~/Web/E-Report/NPITemplate/SMT_DFX.xlsx");

        Aspose.Cells.Workbook book = new Aspose.Cells.Workbook(templatePath);
        Aspose.Cells.Worksheet sheet0 = book.Worksheets[0];

        BindDFXSafety(ref sheet0, caseID, Bu, Building, docno);

        if (File.Exists(@fileName))//判斷當前路徑文件是否存在,若存在則先刪除在保存
        {
            File.Delete(@fileName);
            book.Save(fileName, Aspose.Cells.SaveFormat.Xlsx);
        }
        else
        {
            book.Save(fileName, Aspose.Cells.SaveFormat.Xlsx);
        }

        string strLog = string.Format("Date:{0} DFX Report has create ok!", System.DateTime.Now.ToString("yyyy-MM-dd"));
        WriteLog(strLog);
    }

    private void BindDFXSafety(ref Aspose.Cells.Worksheet sheet, string caseID, string Bu, string Building, string DocNo)
    {
        //page 格式設定
        SetStyle(ref sheet, Aspose.Cells.PageOrientationType.Landscape);
        Aspose.Cells.Cells cells = sheet.Cells;
        //string logoPath = Page.Server.MapPath("") + "\\log.png";
        //sheet.Pictures.Add(0, 0, 4, 10, logoPath);

        NPIMgmt oMgmt = new NPIMgmt("CZ", Bu);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();

        #region 填充基本及資料

        DataTable dtMaster = GetMaster(DocNo);//基本資料
        DataRow drM = dtMaster.Rows[0];
        DataTable dtMaxPoints = SUMMaxPoints(DocNo, "Safety");
        DataTable dtDFXPoints = SUMDFXPoints(DocNo, "Safety");
        int PossiblePoints = int.Parse(dtMaxPoints.Rows[0]["PossiblePoints"].ToString() == "" ? "0" : dtMaxPoints.Rows[0]["PossiblePoints"].ToString());
        int CompliancePoints = int.Parse(dtDFXPoints.Rows[0]["CompliancePoints"].ToString() == "" ? "0" : dtDFXPoints.Rows[0]["CompliancePoints"].ToString());
        if (PossiblePoints == 0 || CompliancePoints == 0)
        {
            cells[4, 8].PutValue(0);

        }
        else
        {
            double DFXScore = CompliancePoints / PossiblePoints;
            cells[4, 8].PutValue(DFXScore);

        }
        cells[4, 1].PutValue(PossiblePoints);
        cells[4, 4].PutValue(CompliancePoints);


        sheet.Replace("{Dept}", "Safety");
        sheet.Replace("{MODEL_NAME}", drM["MODEL_NAME"].ToString());
        sheet.Replace("{CUSTOMER}", drM["CUSTOMER"].ToString());
        sheet.Replace("{SUB_DOC_PHASE}", drM["SUB_DOC_PHASE"].ToString());
        sheet.Replace("{PCB_REV}", drM["PCB_REV"].ToString());
        sheet.Replace("{BOM_REV}", drM["BOM_REV"].ToString());
        sheet.Replace("{SUB_DOC_NO}", drM["SUB_DOC_NO"].ToString());
        #endregion

        #region//獲取主表資訊
        DataTable dtDFXSafety = GetDFXByDept(DocNo, "Safety");
        if (dtDFXSafety.Rows.Count > 0)
        {
            sheet.Replace("{ReviewDate}", dtDFXSafety.Rows[0]["UpdateTime"].ToString().Length > 0 ? Convert.ToDateTime(dtDFXSafety.Rows[0]["UpdateTime"].ToString()).ToString("yyyy/MM/dd") : "");
            int templateIndexDFX = 6;//模板row起始位置
            int insertIndexEnCounter = templateIndexDFX + 1;//new row起始位置

            cells.InsertRows(insertIndexEnCounter, dtDFXSafety.Rows.Count - 1);
            cells.CopyRows(cells, templateIndexDFX, insertIndexEnCounter, dtDFXSafety.Rows.Count - 1); //複製模板row格式至新行

            for (int i = 0; i < dtDFXSafety.Rows.Count; i++)
            {

                DataRow dr = dtDFXSafety.Rows[i];
                cells[i + templateIndexDFX, 0].PutValue(dr["Item"].ToString());
                cells[i + templateIndexDFX, 1].PutValue(dr["ItemType"].ToString());
                cells[i + templateIndexDFX, 2].PutValue(dr["ItemName"].ToString());
                cells[i + templateIndexDFX, 3].PutValue(dr["Requirements"].ToString());
                cells[i + templateIndexDFX, 4].PutValue("");
                cells[i + templateIndexDFX, 5].PutValue(dr["Losses"].ToString());
                cells[i + templateIndexDFX, 6].PutValue(dr["Location"].ToString());
                cells[i + templateIndexDFX, 7].PutValue(dr["PriorityLevel"].ToString());
                cells[i + templateIndexDFX, 8].PutValue(dr["MaxPoints"].ToString() == "NA" || dr["MaxPoints"].ToString() == "" ? 0 : int.Parse(dr["MaxPoints"].ToString()));
                cells[i + templateIndexDFX, 9].PutValue(dr["DFXPoints"].ToString() == "NA" || dr["MaxPoints"].ToString() == "" ? 0 : int.Parse(dr["DFXPoints"].ToString()));
                cells[i + templateIndexDFX, 10].PutValue(dr["Compliance"].ToString());
                cells[i + templateIndexDFX, 11].PutValue(dr["Comments"].ToString());
            }


        }
        else
        {
            sheet.Replace("{ReviewDate}", "");

        }

        #endregion

        #region 签核人员
        DataTable dtResultW = GetDFXWrite(drM["DOC_NO"].ToString(), "Safety");
        DataTable dtResultC = GetDFXCheck(drM["DOC_NO"].ToString(), "Safety", drM["SUB_DOC_NO"].ToString());
        if (dtResultW.Rows.Count > 0)
        {
            sheet.Replace("{PM Write}", dtResultW.Rows[0]["WriteEname"].ToString());
        }
        else
        {
            sheet.Replace("{PM Write}", "");
        }

        if (dtResultC.Rows.Count > 0)
        {
            sheet.Replace("{PM Check}", dtResultC.Rows[0]["APPROVER"].ToString() + ";" + Convert.ToDateTime(dtResultC.Rows[0]["APPROVER_DATE"].ToString()).ToString("yyyy/MM/dd hh:mm"));
        }
        else
        {
            sheet.Replace("{PM Check}", "");
        }

        #endregion
    }
    #endregion

    #region DFX SQ
    private void CreateExcel_DFXSQ(string fileName, string docno, string caseID, string Bu, string Building)
    {
        Aspose.Cells.License lic = new Aspose.Cells.License();
        string AsposeLicPath = System.Configuration.ConfigurationSettings.AppSettings["AsposeLicPath"].ToString();
        lic.SetLicense(AsposeLicPath);
        string templatePath = Page.Server.MapPath("~/Web/E-Report/NPITemplate/SMT_DFX.xlsx");

        Aspose.Cells.Workbook book = new Aspose.Cells.Workbook(templatePath);
        Aspose.Cells.Worksheet sheet0 = book.Worksheets[0];

        BindDFXSQ(ref sheet0, caseID, Bu, Building, docno);

        if (File.Exists(@fileName))//判斷當前路徑文件是否存在,若存在則先刪除在保存
        {
            File.Delete(@fileName);
            book.Save(fileName, Aspose.Cells.SaveFormat.Xlsx);
        }
        else
        {
            book.Save(fileName, Aspose.Cells.SaveFormat.Xlsx);
        }

        string strLog = string.Format("Date:{0} DFX Report has create ok!", System.DateTime.Now.ToString("yyyy-MM-dd"));
        WriteLog(strLog);
    }

    private void BindDFXSQ(ref Aspose.Cells.Worksheet sheet, string caseID, string Bu, string Building, string DocNo)
    {
        //page 格式設定
        SetStyle(ref sheet, Aspose.Cells.PageOrientationType.Landscape);
        Aspose.Cells.Cells cells = sheet.Cells;
        //string logoPath = Page.Server.MapPath("") + "\\log.png";
        //sheet.Pictures.Add(0, 0, 4, 10, logoPath);

        NPIMgmt oMgmt = new NPIMgmt("CZ", Bu);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();

        #region 填充基本及資料

        DataTable dtMaster = GetMaster(DocNo);//基本資料
        DataRow drM = dtMaster.Rows[0];
        DataTable dtMaxPoints = SUMMaxPoints(DocNo, "SQ");
        DataTable dtDFXPoints = SUMDFXPoints(DocNo, "SQ");
        int PossiblePoints = int.Parse(dtMaxPoints.Rows[0]["PossiblePoints"].ToString() == "" ? "0" : dtMaxPoints.Rows[0]["PossiblePoints"].ToString());
        int CompliancePoints = int.Parse(dtDFXPoints.Rows[0]["CompliancePoints"].ToString() == "" ? "0" : dtDFXPoints.Rows[0]["CompliancePoints"].ToString());
        if (PossiblePoints == 0 || CompliancePoints == 0)
        {
            cells[4, 8].PutValue(0);

        }
        else
        {
            double DFXScore = CompliancePoints / PossiblePoints;
            cells[4, 8].PutValue(DFXScore);

        }
        cells[4, 1].PutValue(PossiblePoints);
        cells[4, 4].PutValue(CompliancePoints);


        sheet.Replace("{Dept}", "SQ");
        sheet.Replace("{MODEL_NAME}", drM["MODEL_NAME"].ToString());
        sheet.Replace("{CUSTOMER}", drM["CUSTOMER"].ToString());
        sheet.Replace("{SUB_DOC_PHASE}", drM["SUB_DOC_PHASE"].ToString());
        sheet.Replace("{PCB_REV}", drM["PCB_REV"].ToString());
        sheet.Replace("{BOM_REV}", drM["BOM_REV"].ToString());
        sheet.Replace("{SUB_DOC_NO}", drM["SUB_DOC_NO"].ToString());
        #endregion

        #region//獲取主表資訊
        DataTable dtDFXSQ = GetDFXByDept(DocNo, "SQ");
        if (dtDFXSQ.Rows.Count > 0)
        {
            sheet.Replace("{ReviewDate}", Convert.ToDateTime(dtDFXSQ.Rows[0]["UpdateTime"].ToString()).ToString("yyyy/MM/dd"));
            int templateIndexDFX = 6;//模板row起始位置
            int insertIndexEnCounter = templateIndexDFX + 1;//new row起始位置

            cells.InsertRows(insertIndexEnCounter, dtDFXSQ.Rows.Count - 1);
            cells.CopyRows(cells, templateIndexDFX, insertIndexEnCounter, dtDFXSQ.Rows.Count - 1); //複製模板row格式至新行

            for (int i = 0; i < dtDFXSQ.Rows.Count; i++)
            {

                DataRow dr = dtDFXSQ.Rows[i];
                cells[i + templateIndexDFX, 0].PutValue(dr["Item"].ToString());
                cells[i + templateIndexDFX, 1].PutValue(dr["ItemType"].ToString());
                cells[i + templateIndexDFX, 2].PutValue(dr["ItemName"].ToString());
                cells[i + templateIndexDFX, 3].PutValue(dr["Requirements"].ToString());
                cells[i + templateIndexDFX, 4].PutValue("");
                cells[i + templateIndexDFX, 5].PutValue(dr["Losses"].ToString());
                cells[i + templateIndexDFX, 6].PutValue(dr["Location"].ToString());
                cells[i + templateIndexDFX, 7].PutValue(dr["PriorityLevel"].ToString());
                cells[i + templateIndexDFX, 8].PutValue(dr["MaxPoints"].ToString() == "NA" || dr["MaxPoints"].ToString() == "" ? 0 : int.Parse(dr["MaxPoints"].ToString()));
                cells[i + templateIndexDFX, 9].PutValue(dr["DFXPoints"].ToString() == "NA" || dr["MaxPoints"].ToString() == "" ? 0 : int.Parse(dr["DFXPoints"].ToString()));
                cells[i + templateIndexDFX, 10].PutValue(dr["Compliance"].ToString());
                cells[i + templateIndexDFX, 11].PutValue(dr["Comments"].ToString());
            }


        }
        else
        {
            sheet.Replace("{ReviewDate}", "");
        }
        #endregion

        #region 签核人员
        DataTable dtResultW = GetDFXWrite(drM["DOC_NO"].ToString(), "SQ");
        DataTable dtResultC = GetDFXCheck(drM["DOC_NO"].ToString(), "SQ", drM["SUB_DOC_NO"].ToString());
        if (dtResultW.Rows.Count > 0)
        {
            sheet.Replace("{PM Write}", dtResultW.Rows[0]["WriteEname"].ToString());
        }
        else
        {
            sheet.Replace("{PM Write}", "");
        }

        if (dtResultC.Rows.Count > 0)
        {
            sheet.Replace("{PM Check}", dtResultC.Rows[0]["APPROVER"].ToString() + ";" + Convert.ToDateTime(dtResultC.Rows[0]["APPROVER_DATE"].ToString()).ToString("yyyy/MM/dd hh:mm"));
        }
        else
        {
            sheet.Replace("{PM Check}", "");
        }

        #endregion
    }
    #endregion

    #region DFX EE
    private void CreateExcel_DFXEE(string fileName, string docno, string caseID, string Bu, string Building)
    {
        Aspose.Cells.License lic = new Aspose.Cells.License();
        string AsposeLicPath = System.Configuration.ConfigurationSettings.AppSettings["AsposeLicPath"].ToString();
        lic.SetLicense(AsposeLicPath);
        string templatePath = Page.Server.MapPath("~/Web/E-Report/NPITemplate/SMT_DFX.xlsx");

        Aspose.Cells.Workbook book = new Aspose.Cells.Workbook(templatePath);
        Aspose.Cells.Worksheet sheet0 = book.Worksheets[0];

        BindDFXEE(ref sheet0, caseID, Bu, Building, docno);

        if (File.Exists(@fileName))//判斷當前路徑文件是否存在,若存在則先刪除在保存
        {
            File.Delete(@fileName);
            book.Save(fileName, Aspose.Cells.SaveFormat.Xlsx);
        }
        else
        {
            book.Save(fileName, Aspose.Cells.SaveFormat.Xlsx);
        }

        string strLog = string.Format("Date:{0} DFX Report has create ok!", System.DateTime.Now.ToString("yyyy-MM-dd"));
        WriteLog(strLog);
    }

    private void BindDFXEE(ref Aspose.Cells.Worksheet sheet, string caseID, string Bu, string Building, string DocNo)
    {
        //page 格式設定
        SetStyle(ref sheet, Aspose.Cells.PageOrientationType.Landscape);
        Aspose.Cells.Cells cells = sheet.Cells;
        //string logoPath = Page.Server.MapPath("") + "\\log.png";
        //sheet.Pictures.Add(0, 0, 4, 10, logoPath);

        NPIMgmt oMgmt = new NPIMgmt("CZ", Bu);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();

        #region 填充基本及資料

        DataTable dtMaster = GetMaster(DocNo);//基本資料
        DataRow drM = dtMaster.Rows[0];
        DataTable dtMaxPoints = SUMMaxPoints(DocNo, "EE");
        DataTable dtDFXPoints = SUMDFXPoints(DocNo, "EE");
        int PossiblePoints = int.Parse(dtMaxPoints.Rows[0]["PossiblePoints"].ToString() == "" ? "0" : dtMaxPoints.Rows[0]["PossiblePoints"].ToString());
        int CompliancePoints = int.Parse(dtDFXPoints.Rows[0]["CompliancePoints"].ToString() == "" ? "0" : dtDFXPoints.Rows[0]["CompliancePoints"].ToString());
        if (PossiblePoints == 0 || CompliancePoints == 0)
        {
            cells[4, 8].PutValue(0);

        }
        else
        {
            double DFXScore = CompliancePoints / PossiblePoints;
            cells[4, 8].PutValue(DFXScore);

        }
        cells[4, 1].PutValue(PossiblePoints);
        cells[4, 4].PutValue(CompliancePoints);

        sheet.Replace("{Dept}", "EE");
        sheet.Replace("{MODEL_NAME}", drM["MODEL_NAME"].ToString());
        sheet.Replace("{CUSTOMER}", drM["CUSTOMER"].ToString());
        sheet.Replace("{SUB_DOC_PHASE}", drM["SUB_DOC_PHASE"].ToString());
        sheet.Replace("{PCB_REV}", drM["PCB_REV"].ToString());
        sheet.Replace("{BOM_REV}", drM["BOM_REV"].ToString());
        sheet.Replace("{SUB_DOC_NO}", drM["SUB_DOC_NO"].ToString());
        #endregion

        #region//獲取主表資訊
        DataTable dtDFXEE = GetDFXByDept(DocNo, "EE");
        if (dtDFXEE.Rows.Count > 0)
        {
            sheet.Replace("{ReviewDate}", Convert.ToDateTime(dtDFXEE.Rows[0]["UpdateTime"].ToString()).ToString("yyyy/MM/dd"));
            int templateIndexDFX = 6;//模板row起始位置
            int insertIndexEnCounter = templateIndexDFX + 1;//new row起始位置

            cells.InsertRows(insertIndexEnCounter, dtDFXEE.Rows.Count - 1);
            cells.CopyRows(cells, templateIndexDFX, insertIndexEnCounter, dtDFXEE.Rows.Count - 1); //複製模板row格式至新行

            for (int i = 0; i < dtDFXEE.Rows.Count; i++)
            {

                DataRow dr = dtDFXEE.Rows[i];
                cells[i + templateIndexDFX, 0].PutValue(dr["Item"].ToString());
                cells[i + templateIndexDFX, 1].PutValue(dr["ItemType"].ToString());
                cells[i + templateIndexDFX, 2].PutValue(dr["ItemName"].ToString());
                cells[i + templateIndexDFX, 3].PutValue(dr["Requirements"].ToString());
                cells[i + templateIndexDFX, 4].PutValue("");
                cells[i + templateIndexDFX, 5].PutValue(dr["Losses"].ToString());
                cells[i + templateIndexDFX, 6].PutValue(dr["Location"].ToString());
                cells[i + templateIndexDFX, 7].PutValue(dr["PriorityLevel"].ToString());
                cells[i + templateIndexDFX, 8].PutValue(dr["MaxPoints"].ToString() == "NA" || dr["MaxPoints"].ToString() == "" ? 0 : int.Parse(dr["MaxPoints"].ToString()));
                cells[i + templateIndexDFX, 9].PutValue(dr["DFXPoints"].ToString() == "NA" || dr["MaxPoints"].ToString() == "" ? 0 : int.Parse(dr["DFXPoints"].ToString()));
                cells[i + templateIndexDFX, 10].PutValue(dr["Compliance"].ToString());
                cells[i + templateIndexDFX, 11].PutValue(dr["Comments"].ToString());
            }


        }
        else
        {
            sheet.Replace("{ReviewDate}", "");
        }

        #endregion

        #region 签核人员
        DataTable dtResultW = GetDFXWrite(drM["DOC_NO"].ToString(), "EE");
        DataTable dtResultC = GetDFXCheck(drM["DOC_NO"].ToString(), "EE", drM["SUB_DOC_NO"].ToString());
        if (dtResultW.Rows.Count > 0)
        {
            sheet.Replace("{PM Write}", dtResultW.Rows[0]["WriteEname"].ToString());
        }
        else
        {
            sheet.Replace("{PM Write}", "");
        }

        if (dtResultC.Rows.Count > 0)
        {
            sheet.Replace("{PM Check}", dtResultC.Rows[0]["APPROVER"].ToString() + ";" + Convert.ToDateTime(dtResultC.Rows[0]["APPROVER_DATE"].ToString()).ToString("yyyy/MM/dd hh:mm"));
        }
        else
        {
            sheet.Replace("{PM Check}", "");
        }

        #endregion
    }
    #endregion

    #region DFX IE
    private void CreateExcel_DFXIE(string fileName, string docno, string caseID, string Bu, string Building)
    {
        Aspose.Cells.License lic = new Aspose.Cells.License();
        string AsposeLicPath = System.Configuration.ConfigurationSettings.AppSettings["AsposeLicPath"].ToString();
        lic.SetLicense(AsposeLicPath);
        string templatePath = Page.Server.MapPath("~/Web/E-Report/NPITemplate/SMT_DFX.xlsx");

        Aspose.Cells.Workbook book = new Aspose.Cells.Workbook(templatePath);
        Aspose.Cells.Worksheet sheet0 = book.Worksheets[0];

        BindDFXIE(ref sheet0, caseID, Bu, Building, docno);

        if (File.Exists(@fileName))//判斷當前路徑文件是否存在,若存在則先刪除在保存
        {
            File.Delete(@fileName);
            book.Save(fileName, Aspose.Cells.SaveFormat.Xlsx);
        }
        else
        {
            book.Save(fileName, Aspose.Cells.SaveFormat.Xlsx);
        }

        string strLog = string.Format("Date:{0} DFX Report has create ok!", System.DateTime.Now.ToString("yyyy-MM-dd"));
        WriteLog(strLog);
    }

    private void BindDFXIE(ref Aspose.Cells.Worksheet sheet, string caseID, string Bu, string Building, string DocNo)
    {
        //page 格式設定
        SetStyle(ref sheet, Aspose.Cells.PageOrientationType.Landscape);
        Aspose.Cells.Cells cells = sheet.Cells;
        //string logoPath = Page.Server.MapPath("") + "\\log.png";
        //sheet.Pictures.Add(0, 0, 4, 10, logoPath);

        NPIMgmt oMgmt = new NPIMgmt("CZ", Bu);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();

        #region 填充基本及資料

        DataTable dtMaster = GetMaster(DocNo);//基本資料
        DataRow drM = dtMaster.Rows[0];
        DataTable dtMaxPoints = SUMMaxPoints(DocNo, "IE");
        DataTable dtDFXPoints = SUMDFXPoints(DocNo, "IE");
        int PossiblePoints = int.Parse(dtMaxPoints.Rows[0]["PossiblePoints"].ToString() == "" ? "0" : dtMaxPoints.Rows[0]["PossiblePoints"].ToString());
        int CompliancePoints = int.Parse(dtDFXPoints.Rows[0]["CompliancePoints"].ToString() == "" ? "0" : dtDFXPoints.Rows[0]["CompliancePoints"].ToString());
        if (PossiblePoints == 0 || CompliancePoints == 0)
        {
            cells[4, 8].PutValue(0);

        }
        else
        {
            double DFXScore = CompliancePoints / PossiblePoints;
            cells[4, 8].PutValue(DFXScore);

        }
        cells[4, 1].PutValue(PossiblePoints);
        cells[4, 4].PutValue(CompliancePoints);


        sheet.Replace("{Dept}", "IE");
        sheet.Replace("{MODEL_NAME}", drM["MODEL_NAME"].ToString());
        sheet.Replace("{CUSTOMER}", drM["CUSTOMER"].ToString());
        sheet.Replace("{SUB_DOC_PHASE}", drM["SUB_DOC_PHASE"].ToString());
        sheet.Replace("{PCB_REV}", drM["PCB_REV"].ToString());
        sheet.Replace("{BOM_REV}", drM["BOM_REV"].ToString());
        sheet.Replace("{SUB_DOC_NO}", drM["SUB_DOC_NO"].ToString());
        #endregion

        #region//獲取主表資訊
        DataTable dtDFXIE = GetDFXByDept(DocNo, "IE");
        if (dtDFXIE.Rows.Count > 0)
        {
            sheet.Replace("{ReviewDate}", Convert.ToDateTime(dtDFXIE.Rows[0]["UpdateTime"].ToString()).ToString("yyyy/MM/dd"));
            int templateIndexDFX = 6;//模板row起始位置
            int insertIndexEnCounter = templateIndexDFX + 1;//new row起始位置

            cells.InsertRows(insertIndexEnCounter, dtDFXIE.Rows.Count - 1);
            cells.CopyRows(cells, templateIndexDFX, insertIndexEnCounter, dtDFXIE.Rows.Count - 1); //複製模板row格式至新行

            for (int i = 0; i < dtDFXIE.Rows.Count; i++)
            {

                DataRow dr = dtDFXIE.Rows[i];
                cells[i + templateIndexDFX, 0].PutValue(dr["Item"].ToString());
                cells[i + templateIndexDFX, 1].PutValue(dr["ItemType"].ToString());
                cells[i + templateIndexDFX, 2].PutValue(dr["ItemName"].ToString());
                cells[i + templateIndexDFX, 3].PutValue(dr["Requirements"].ToString());
                cells[i + templateIndexDFX, 4].PutValue("");
                cells[i + templateIndexDFX, 5].PutValue(dr["Losses"].ToString());
                cells[i + templateIndexDFX, 6].PutValue(dr["Location"].ToString());
                cells[i + templateIndexDFX, 7].PutValue(dr["PriorityLevel"].ToString());
                cells[i + templateIndexDFX, 8].PutValue(dr["MaxPoints"].ToString() == "NA" || dr["MaxPoints"].ToString() == "" ? 0 : int.Parse(dr["MaxPoints"].ToString()));
                cells[i + templateIndexDFX, 9].PutValue(dr["DFXPoints"].ToString() == "NA" || dr["MaxPoints"].ToString() == "" ? 0 : int.Parse(dr["DFXPoints"].ToString()));
                cells[i + templateIndexDFX, 10].PutValue(dr["Compliance"].ToString());
                cells[i + templateIndexDFX, 11].PutValue(dr["Comments"].ToString());
            }


        }
        else
        {
            sheet.Replace("{ReviewDate}", "");
        }

        #endregion

        #region 签核人员
        DataTable dtResultW = GetDFXWrite(drM["DOC_NO"].ToString(), "IE");
        DataTable dtResultC = GetDFXCheck(drM["DOC_NO"].ToString(), "IE", drM["SUB_DOC_NO"].ToString());
        if (dtResultW.Rows.Count > 0)
        {
            sheet.Replace("{PM Write}", dtResultW.Rows[0]["WriteEname"].ToString());
        }
        else
        {
            sheet.Replace("{PM Write}", "");
        }

        if (dtResultC.Rows.Count > 0)
        {
            sheet.Replace("{PM Check}", dtResultC.Rows[0]["APPROVER"].ToString() + ";" + Convert.ToDateTime(dtResultC.Rows[0]["APPROVER_DATE"].ToString()).ToString("yyyy/MM/dd hh:mm"));
        }
        else
        {
            sheet.Replace("{PM Check}", "");
        }

        #endregion
    }
    #endregion

    #region DFX TE
    private void CreateExcel_DFXTE(string fileName, string docno, string caseID, string Bu, string Building)
    {
        Aspose.Cells.License lic = new Aspose.Cells.License();
        string AsposeLicPath = System.Configuration.ConfigurationSettings.AppSettings["AsposeLicPath"].ToString();
        lic.SetLicense(AsposeLicPath);
        string templatePath = Page.Server.MapPath("~/Web/E-Report/NPITemplate/SMT_DFX.xlsx");

        Aspose.Cells.Workbook book = new Aspose.Cells.Workbook(templatePath);
        Aspose.Cells.Worksheet sheet0 = book.Worksheets[0];

        BindDFXTE(ref sheet0, caseID, Bu, Building, docno);

        if (File.Exists(@fileName))//判斷當前路徑文件是否存在,若存在則先刪除在保存
        {
            File.Delete(@fileName);
            book.Save(fileName, Aspose.Cells.SaveFormat.Xlsx);
        }
        else
        {
            book.Save(fileName, Aspose.Cells.SaveFormat.Xlsx);
        }

        string strLog = string.Format("Date:{0} DFX Report has create ok!", System.DateTime.Now.ToString("yyyy-MM-dd"));
        WriteLog(strLog);
    }

    private void BindDFXTE(ref Aspose.Cells.Worksheet sheet, string caseID, string Bu, string Building, string DocNo)
    {
        //page 格式設定
        SetStyle(ref sheet, Aspose.Cells.PageOrientationType.Landscape);
        Aspose.Cells.Cells cells = sheet.Cells;
        //string logoPath = Page.Server.MapPath("") + "\\log.png";
        //sheet.Pictures.Add(0, 0, 4, 10, logoPath);

        NPIMgmt oMgmt = new NPIMgmt("CZ", Bu);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();

        #region 填充基本及資料

        DataTable dtMaster = GetMaster(DocNo);//基本資料
        DataRow drM = dtMaster.Rows[0];
        DataTable dtMaxPoints = SUMMaxPoints(DocNo, "TE");
        DataTable dtDFXPoints = SUMDFXPoints(DocNo, "TE");
        int PossiblePoints = int.Parse(dtMaxPoints.Rows[0]["PossiblePoints"].ToString() == "" ? "0" : dtMaxPoints.Rows[0]["PossiblePoints"].ToString());
        int CompliancePoints = int.Parse(dtDFXPoints.Rows[0]["CompliancePoints"].ToString() == "" ? "0" : dtDFXPoints.Rows[0]["CompliancePoints"].ToString());
        if (PossiblePoints == 0 || CompliancePoints == 0)
        {
            cells[4, 8].PutValue(0);

        }
        else
        {
            double DFXScore = CompliancePoints / PossiblePoints;
            cells[4, 8].PutValue(DFXScore);

        }
        cells[4, 1].PutValue(PossiblePoints);
        cells[4, 4].PutValue(CompliancePoints);


        sheet.Replace("{Dept}", "TE");
        sheet.Replace("{MODEL_NAME}", drM["MODEL_NAME"].ToString());
        sheet.Replace("{CUSTOMER}", drM["CUSTOMER"].ToString());
        sheet.Replace("{SUB_DOC_PHASE}", drM["SUB_DOC_PHASE"].ToString());
        sheet.Replace("{PCB_REV}", drM["PCB_REV"].ToString());
        sheet.Replace("{BOM_REV}", drM["BOM_REV"].ToString());
        sheet.Replace("{SUB_DOC_NO}", drM["SUB_DOC_NO"].ToString());
        #endregion

        #region//獲取主表資訊
        DataTable dtDFXTE = GetDFXByDept(DocNo, "TE");
        if (dtDFXTE.Rows.Count > 0)
        {
            sheet.Replace("{ReviewDate}", Convert.ToDateTime(dtDFXTE.Rows[0]["UpdateTime"].ToString()).ToString("yyyy/MM/dd"));
            int templateIndexDFX = 6;//模板row起始位置
            int insertIndexEnCounter = templateIndexDFX + 1;//new row起始位置

            cells.InsertRows(insertIndexEnCounter, dtDFXTE.Rows.Count - 1);
            cells.CopyRows(cells, templateIndexDFX, insertIndexEnCounter, dtDFXTE.Rows.Count - 1); //複製模板row格式至新行

            for (int i = 0; i < dtDFXTE.Rows.Count; i++)
            {

                DataRow dr = dtDFXTE.Rows[i];
                cells[i + templateIndexDFX, 0].PutValue(dr["Item"].ToString());
                cells[i + templateIndexDFX, 1].PutValue(dr["ItemType"].ToString());
                cells[i + templateIndexDFX, 2].PutValue(dr["ItemName"].ToString());
                cells[i + templateIndexDFX, 3].PutValue(dr["Requirements"].ToString());
                cells[i + templateIndexDFX, 4].PutValue("");
                cells[i + templateIndexDFX, 5].PutValue(dr["Losses"].ToString());
                cells[i + templateIndexDFX, 6].PutValue(dr["Location"].ToString());
                cells[i + templateIndexDFX, 7].PutValue(dr["PriorityLevel"].ToString());
                cells[i + templateIndexDFX, 8].PutValue(dr["MaxPoints"].ToString() == "NA" || dr["MaxPoints"].ToString() == "" ? 0 : int.Parse(dr["MaxPoints"].ToString()));
                cells[i + templateIndexDFX, 9].PutValue(dr["DFXPoints"].ToString() == "NA" || dr["MaxPoints"].ToString() == "" ? 0 : int.Parse(dr["DFXPoints"].ToString()));
                cells[i + templateIndexDFX, 10].PutValue(dr["Compliance"].ToString());
                cells[i + templateIndexDFX, 11].PutValue(dr["Comments"].ToString());
            }


        }
        else
        {
            sheet.Replace("{ReviewDate}", "");
        }
        #endregion

        #region 签核人员
        DataTable dtResultW = GetDFXWrite(drM["DOC_NO"].ToString(), "TE");
        DataTable dtResultC = GetDFXCheck(drM["DOC_NO"].ToString(), "TE", drM["SUB_DOC_NO"].ToString());
        if (dtResultW.Rows.Count > 0)
        {
            sheet.Replace("{PM Write}", dtResultW.Rows[0]["WriteEname"].ToString());
        }
        else
        {
            sheet.Replace("{PM Write}", "");
        }

        if (dtResultC.Rows.Count > 0)
        {
            sheet.Replace("{PM Check}", dtResultC.Rows[0]["APPROVER"].ToString() + ";" + Convert.ToDateTime(dtResultC.Rows[0]["APPROVER_DATE"].ToString()).ToString("yyyy/MM/dd hh:mm"));
        }
        else
        {
            sheet.Replace("{PM Check}", "");
        }

        #endregion
    }
    #endregion

    #region DFX UQ
    private void CreateExcel_DFXUQ(string fileName, string docno, string caseID, string Bu, string Building)
    {
        Aspose.Cells.License lic = new Aspose.Cells.License();
        string AsposeLicPath = System.Configuration.ConfigurationSettings.AppSettings["AsposeLicPath"].ToString();
        lic.SetLicense(AsposeLicPath);
        string templatePath = Page.Server.MapPath("~/Web/E-Report/NPITemplate/SMT_DFX.xlsx");

        Aspose.Cells.Workbook book = new Aspose.Cells.Workbook(templatePath);
        Aspose.Cells.Worksheet sheet0 = book.Worksheets[0];

        BindDFXUQ(ref sheet0, caseID, Bu, Building, docno);

        if (File.Exists(@fileName))//判斷當前路徑文件是否存在,若存在則先刪除在保存
        {
            File.Delete(@fileName);
            book.Save(fileName, Aspose.Cells.SaveFormat.Xlsx);
        }
        else
        {
            book.Save(fileName, Aspose.Cells.SaveFormat.Xlsx);
        }

        #region 生成壓縮檔后
        DataTable dtM = GetMaster(docno);
        DataRow drM = dtM.Rows[0];
        string Model = drM["MODEL_NAME"].ToString();
        string Stage = drM["SUB_DOC_PHASE"].ToString();
        string FolderPath = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + "DFX Doc");
        //Compressedfile(docno, "DFX Doc");//壓縮指定文件夾
        #endregion

        string strLog = string.Format("Date:{0} DFX Report has create ok!", System.DateTime.Now.ToString("yyyy-MM-dd"));
        WriteLog(strLog);
    }

    private void BindDFXUQ(ref Aspose.Cells.Worksheet sheet, string caseID, string Bu, string Building, string DocNo)
    {
        //page 格式設定
        SetStyle(ref sheet, Aspose.Cells.PageOrientationType.Landscape);
        Aspose.Cells.Cells cells = sheet.Cells;
        //string logoPath = Page.Server.MapPath("") + "\\log.png";
        //sheet.Pictures.Add(0, 0, 4, 10, logoPath);

        NPIMgmt oMgmt = new NPIMgmt("CZ", Bu);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();

        #region 填充基本及資料

        DataTable dtMaster = GetMaster(DocNo);//基本資料
        DataRow drM = dtMaster.Rows[0];
        DataTable dtMaxPoints = SUMMaxPoints(DocNo, "UQ");
        DataTable dtDFXPoints = SUMDFXPoints(DocNo, "UQ");
        int PossiblePoints = int.Parse(dtMaxPoints.Rows[0]["PossiblePoints"].ToString() == "" ? "0" : dtMaxPoints.Rows[0]["PossiblePoints"].ToString());
        int CompliancePoints = int.Parse(dtDFXPoints.Rows[0]["CompliancePoints"].ToString() == "" ? "0" : dtDFXPoints.Rows[0]["CompliancePoints"].ToString());
        if (PossiblePoints == 0 || CompliancePoints == 0)
        {
            cells[4, 8].PutValue(0);

        }
        else
        {
            double DFXScore = CompliancePoints / PossiblePoints;
            cells[4, 8].PutValue(DFXScore);

        }
        cells[4, 1].PutValue(PossiblePoints);
        cells[4, 4].PutValue(CompliancePoints);

        sheet.Replace("{Dept}", "UQ");
        sheet.Replace("{MODEL_NAME}", drM["MODEL_NAME"].ToString());
        sheet.Replace("{CUSTOMER}", drM["CUSTOMER"].ToString());
        sheet.Replace("{SUB_DOC_PHASE}", drM["SUB_DOC_PHASE"].ToString());
        sheet.Replace("{PCB_REV}", drM["PCB_REV"].ToString());
        sheet.Replace("{BOM_REV}", drM["BOM_REV"].ToString());
        sheet.Replace("{SUB_DOC_NO}", drM["SUB_DOC_NO"].ToString());
        #endregion

        #region//獲取主表資訊
        DataTable dtDFXUQ = GetDFXByDept(DocNo, "UQ");
        if (dtDFXUQ.Rows.Count > 0)
        {
            sheet.Replace("{ReviewDate}", Convert.ToDateTime(dtDFXUQ.Rows[0]["UpdateTime"].ToString()).ToString("yyyy/MM/dd"));
            int templateIndexDFX = 6;//模板row起始位置
            int insertIndexEnCounter = templateIndexDFX + 1;//new row起始位置

            cells.InsertRows(insertIndexEnCounter, dtDFXUQ.Rows.Count - 1);
            cells.CopyRows(cells, templateIndexDFX, insertIndexEnCounter, dtDFXUQ.Rows.Count - 1); //複製模板row格式至新行

            for (int i = 0; i < dtDFXUQ.Rows.Count; i++)
            {

                DataRow dr = dtDFXUQ.Rows[i];
                cells[i + templateIndexDFX, 0].PutValue(dr["Item"].ToString());
                cells[i + templateIndexDFX, 1].PutValue(dr["ItemType"].ToString());
                cells[i + templateIndexDFX, 2].PutValue(dr["ItemName"].ToString());
                cells[i + templateIndexDFX, 3].PutValue(dr["Requirements"].ToString());
                cells[i + templateIndexDFX, 4].PutValue("");
                cells[i + templateIndexDFX, 5].PutValue(dr["Losses"].ToString());
                cells[i + templateIndexDFX, 6].PutValue(dr["Location"].ToString());
                cells[i + templateIndexDFX, 7].PutValue(dr["PriorityLevel"].ToString());
                cells[i + templateIndexDFX, 8].PutValue(dr["MaxPoints"].ToString() == "NA" || dr["MaxPoints"].ToString() == "" ? 0 : int.Parse(dr["MaxPoints"].ToString()));
                cells[i + templateIndexDFX, 9].PutValue(dr["DFXPoints"].ToString() == "NA" || dr["MaxPoints"].ToString() == "" ? 0 : int.Parse(dr["DFXPoints"].ToString()));
                cells[i + templateIndexDFX, 10].PutValue(dr["Compliance"].ToString());
                cells[i + templateIndexDFX, 11].PutValue(dr["Comments"].ToString());
            }

        }
        else
        {
            sheet.Replace("{ReviewDate}", "");
        }
        #endregion

        #region 签核人员
        DataTable dtResultW = GetDFXWrite(drM["DOC_NO"].ToString(), "UQ");
        DataTable dtResultC = GetDFXCheck(drM["DOC_NO"].ToString(), "UQ", drM["SUB_DOC_NO"].ToString());
        if (dtResultW.Rows.Count > 0)
        {
            sheet.Replace("{PM Write}", dtResultW.Rows[0]["WriteEname"].ToString());
        }
        else
        {
            sheet.Replace("{PM Write}", "");
        }

        if (dtResultC.Rows.Count > 0)
        {
            sheet.Replace("{PM Check}", dtResultC.Rows[0]["APPROVER"].ToString() + ";" + Convert.ToDateTime(dtResultC.Rows[0]["APPROVER_DATE"].ToString()).ToString("yyyy/MM/dd hh:mm"));
        }
        else
        {
            sheet.Replace("{PM Check}", "");
        }

        #endregion
    }
    #endregion

    #endregion

    #endregion

    #region 抓取資料的方法

    #region 計算Issue Tracking 數量

    private DataTable GetIssueTracking_Count_Open(string SUB_DOC_NO)
    {
        StringBuilder sbIT = new StringBuilder();
        sbIT.Append(@"select Count(*) as O from TB_NPI_APP_ISSUELIST where TRACKING = @TRACKING AND SUB_DOC_NO = @SUB_DOC_NO");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@TRACKING", SqlDbType.VarChar, "OPEN"));
        opc.Add(DataPara.CreateDataParameter("@SUB_DOC_NO", SqlDbType.VarChar, SUB_DOC_NO));
        DataTable dt = sdb.GetDataTable(sbIT.ToString(), opc);
        return dt;
    }

    private DataTable GetIssueTracking_Count_Closed(string SUB_DOC_NO)
    {
        StringBuilder sbIT = new StringBuilder();
        sbIT.Append(@"select Count(*) as C from TB_NPI_APP_ISSUELIST where TRACKING = @TRACKING AND SUB_DOC_NO = @SUB_DOC_NO");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@TRACKING", SqlDbType.VarChar, "CLOSED"));
        opc.Add(DataPara.CreateDataParameter("@SUB_DOC_NO", SqlDbType.VarChar, SUB_DOC_NO));
        DataTable dt = sdb.GetDataTable(sbIT.ToString(), opc);
        return dt;
    }

    private DataTable GetIssueTracking_Count_Tracking(string SUB_DOC_NO)
    {
        StringBuilder sbIT = new StringBuilder();
        sbIT.Append(@"select Count(*) AS T from TB_NPI_APP_ISSUELIST where TRACKING = @TRACKING AND SUB_DOC_NO = @SUB_DOC_NO");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@TRACKING", SqlDbType.VarChar, "Tracking"));
        opc.Add(DataPara.CreateDataParameter("@SUB_DOC_NO", SqlDbType.VarChar, SUB_DOC_NO));
        DataTable dt = sdb.GetDataTable(sbIT.ToString(), opc);
        return dt;
    }

    #endregion

    #region 計算FMEA Tracking 數量

    private DataTable GetFMEATracking_Count_Open(string SubNo)
    {
        StringBuilder sbIT = new StringBuilder();
        sbIT.Append(@"select Count(*) as O from TB_NPI_FMEA where Resposibility = @Resposibility AND SubNo = @SubNo");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@Resposibility", SqlDbType.VarChar, "OPEN"));
        opc.Add(DataPara.CreateDataParameter("@SubNo", SqlDbType.VarChar, SubNo));
        DataTable dt = sdb.GetDataTable(sbIT.ToString(), opc);
        return dt;
    }

    private DataTable GetFMWATracking_Count_Closed(string SubNo)
    {
        StringBuilder sbIT = new StringBuilder();
        sbIT.Append(@"select Count(*) AS C from TB_NPI_FMEA where Resposibility = @Resposibility AND SubNo = @SubNo");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@Resposibility", SqlDbType.VarChar, "CLOSED"));
        opc.Add(DataPara.CreateDataParameter("@SubNo", SqlDbType.VarChar, SubNo));
        DataTable dt = sdb.GetDataTable(sbIT.ToString(), opc);
        return dt;
    }

    private DataTable GetFMEATracking_Count_Tracking(string SubNo)
    {
        StringBuilder sbIT = new StringBuilder();
        sbIT.Append(@"select Count(*)  AS T from TB_NPI_FMEA where Resposibility = @Resposibility AND SubNo = @SubNo");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@Resposibility", SqlDbType.VarChar, "Tracking"));
        opc.Add(DataPara.CreateDataParameter("@SubNo", SqlDbType.VarChar, SubNo));
        DataTable dt = sdb.GetDataTable(sbIT.ToString(), opc);
        return dt;
    }

    private DataTable GetFMEATracking_Count_RPN(string SubNo)
    {
        StringBuilder sbIT = new StringBuilder();
        sbIT.Append(@"select Count(*)  AS R from TB_NPI_FMEA where RPN > 144 AND SubNo = @SubNo");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@SubNo", SqlDbType.VarChar, SubNo));
        DataTable dt = sdb.GetDataTable(sbIT.ToString(), opc);
        return dt;
    }
    #endregion

    #region 抓取 DFX所有的項目(Y/N/NA) By 部門

    private DataTable GetDFXByDept(string DFXNo, string Dept)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select * from TB_DFX_ItemBody where DFXNo = @DFXNo and WriteDept = @WriteDept");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DFXNo", SqlDbType.VarChar, DFXNo));
        opc.Add(DataPara.CreateDataParameter("@WriteDept", SqlDbType.VarChar, Dept));
        DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
        return dt;
    }

    #endregion

    #region 抓取所有Check關卡的人
    private DataTable GetCheck_Person(string DOC_NO, string Type)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select distinct CheckedEName from TB_NPI_APP_MEMBER where DOC_NO = @DOC_NO and CategoryFlag = @CategoryFlag");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DOC_NO", SqlDbType.VarChar, DOC_NO));
        opc.Add(DataPara.CreateDataParameter("@CategoryFlag", SqlDbType.VarChar, Type));
        DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
        return dt;
    }
    #endregion

    #region  Get CC List
    private DataTable GetCheck_Person(string DOC_NO, string Type, string Type2)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select distinct CheckedEName from TB_NPI_APP_MEMBER where DOC_NO = @DOC_NO and CategoryFlag != @CategoryFlag and CategoryFlag != @CategoryFlag2");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DOC_NO", SqlDbType.VarChar, DOC_NO));
        opc.Add(DataPara.CreateDataParameter("@CategoryFlag", SqlDbType.VarChar, Type));
        opc.Add(DataPara.CreateDataParameter("@CategoryFlag2", SqlDbType.VarChar, Type2));
        DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
        return dt;
    }
    #endregion

    #region HomePage CTQ %
    private DataTable GetCTQPercent(string DOC_NO, string CTQ)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select *  from TB_NPI_APP_CTQ where SUB_DOC_NO = @SUB_DOC_NO and CTQ = @CTQ");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@SUB_DOC_NO", SqlDbType.VarChar, DOC_NO));
        opc.Add(DataPara.CreateDataParameter("@CTQ", SqlDbType.NVarChar, CTQ));
        DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
        return dt;
    }
    #endregion

    #region HomePage DFX Score
    private DataTable GetDFXScore(string DocNO, string Dept)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"SELECT SUM(CONVERT(int,MaxPoints)) AS MaxPoints,SUM(CONVERT(int,DFXPoints)) as DFXPoints
                    FROM [NPI_REPORT].[dbo].[TB_DFX_ItemBody]
                    where DFXNo = @DFXNo and WriteDept = @WriteDept  and MaxPoints  != 'NA'");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DFXNo", SqlDbType.VarChar, DocNO));
        opc.Add(DataPara.CreateDataParameter("@WriteDept", SqlDbType.VarChar, Dept));
        DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
        return dt;
    }
    #endregion

    #region HomePage DFX根據Priority Level(權重性) 0,1,2,3 抓取Item的N項
    private DataTable GetDFXLevel(string DocNo, string Dept, string PriorityLevel)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select Count(*) as amount from TB_DFX_ItemBody 
                    where  DFXNo = @DFXNo and WriteDept = @WriteDept 
                    and Compliance = @Compliance and PriorityLevel = @PriorityLevel");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DFXNo", SqlDbType.VarChar, DocNo));
        opc.Add(DataPara.CreateDataParameter("@WriteDept", SqlDbType.VarChar, Dept));
        opc.Add(DataPara.CreateDataParameter("@Compliance", SqlDbType.VarChar, "N"));
        opc.Add(DataPara.CreateDataParameter("@PriorityLevel", SqlDbType.VarChar, PriorityLevel));
        DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
        return dt;
    }


    #endregion

    #region HomePage CTQ封面
    private DataTable GetCLCAInconformity(string DocNo)
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        ArrayList opc = new ArrayList();
        opc.Clear();
        StringBuilder sb = new StringBuilder();
        DataTable dt = new DataTable();
        decimal ACT_DD = 0;

        sb.Append(" select *,'' as GOALStr,'' as ACTStr  from TB_NPI_APP_CTQ");
        sb.Append(" where  SUB_DOC_NO=@SUB_DOC_NO AND STATUS<>'Write'");
        sb.Append(" order by DEPT ");
        opc.Add(DataPara.CreateDataParameter("@SUB_DOC_NO", SqlDbType.VarChar, DocNo));
        dt = sdb.GetDataTable(sb.ToString(), opc);
        foreach (DataRow dr in dt.Rows)
        {

            string CONTROL_TYPE = dr["CONTROL_TYPE"].ToString();
            string GO = dr["GOAL"].ToString();
            string ACTT = dr["ACT"].ToString();
            if (CONTROL_TYPE == "Yield%" && GO != "NA")
            {
                dr.BeginEdit();
                decimal GOAL = Convert.ToDecimal(dr["GOAL"].ToString()) * 100;
                string GOAL_STR = Convert.ToString(GOAL);
                dr["GOALStr"] = string.Format("{0}%", GOAL_STR);

                string ACT = dr["ACT"].ToString();
                if (!string.IsNullOrEmpty(ACT))
                {
                    if (decimal.TryParse(ACT, out ACT_DD))
                    {
                        dr["ACTStr"] = string.Format("{0}%", ACT);
                    }
                    else
                    {
                        dr["ACTStr"] = ACT;
                    }
                }
                dr.EndEdit();
            }
            else if (GO == "NA" && ACTT == "NA")
            {
                dr.BeginEdit();
                dr["GOALStr"] = dr["GOAL"].ToString();
                dr["ACTStr"] = dr["Act"].ToString();
                dr.EndEdit();
            }
            else if (GO == "NA" && ACTT != "NA")
            {
                dr.BeginEdit();
                dr["GOALStr"] = dr["GOAL"].ToString();
                dr["ACTStr"] = dr["Act"].ToString();
                dr.EndEdit();
            }
            else if (GO == "NA")
            {
                dr.BeginEdit();
                dr["GOALStr"] = dr["GOAL"].ToString();
                dr.EndEdit();
            }
            else
            {
                dr.BeginEdit();
                dr["GOALStr"] = dr["GOAL"].ToString();
                dr["ACTStr"] = dr["ACT"].ToString();
                dr.EndEdit();
            }
        }
        return dt;
    }
    #endregion

    #region HomePage DFX Fail項 By Dept
    private DataTable GetDFXFail(string DOC_NO, string DEPT)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"Select Item,Compliance from TB_DFX_ItemBody where DFXNo =@DFXNo and Compliance=@Compliance and WriteDept = @WriteDept");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DFXNo", SqlDbType.VarChar, DOC_NO));
        opc.Add(DataPara.CreateDataParameter("@WriteDept", SqlDbType.VarChar, DEPT));
        opc.Add(DataPara.CreateDataParameter("@Compliance", SqlDbType.VarChar, "N"));
        DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
        return dt;
    }
    #endregion

    #region HomePage獲取各部門,各關卡的簽核人

    private DataTable GetWriteReplyChecked(string DOC_NO, string SUB_DOC_NO)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"Select distinct T1.DEPT ,T1.WriteEname, T1.ReplyEName,T1.CheckedEName ,
                    T2.APPROVER_DATE ,T2.APPROVER_RESULT,T2.APPROVER_OPINION from TB_NPI_APP_MEMBER  T1 
                    left join TB_NPI_APP_RESULT T2 ON T1.CheckedEName = T2.APPROVER
                    where T1.DOC_NO = @DOC_NO AND T2.SUB_DOC_NO = @SUB_DOC_NO
                    and T1.CategoryFlag ='A'
                    order by T1.DEPT");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DOC_NO", SqlDbType.VarChar, DOC_NO));
        opc.Add(DataPara.CreateDataParameter("@SUB_DOC_NO", SqlDbType.VarChar, SUB_DOC_NO));
        DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
        return dt;
    }

    private DataTable GetWriteReplyChecked(string DOC_NO, string SUB_DOC_NO, string Dept)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"Select distinct T1.DEPT ,T1.WriteEname, T1.ReplyEName,T1.CheckedEName ,
                    T2.APPROVER_DATE ,T2.APPROVER_RESULT,T2.APPROVER_OPINION from TB_NPI_APP_MEMBER  T1 
                    left join TB_NPI_APP_RESULT T2 ON T1.CheckedEName = T2.APPROVER
                    where T1.DOC_NO = @DOC_NO AND T2.SUB_DOC_NO = @SUB_DOC_NO
                    and T1.CategoryFlag ='A' and T2.DEPT = @DEPT
                    order by T1.DEPT");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DOC_NO", SqlDbType.VarChar, DOC_NO));
        opc.Add(DataPara.CreateDataParameter("@SUB_DOC_NO", SqlDbType.VarChar, SUB_DOC_NO));
        opc.Add(DataPara.CreateDataParameter("@DEPT", SqlDbType.VarChar, Dept));
        DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
        return dt;
    }

    #endregion

    #region HomePage 抓取PM起單人/簽核人
    private DataTable GetPMPerson(string DOC_NO)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"Select T1.*,T2.NPI_PM from TB_NPI_APP_MEMBER T1 
                    Left join TB_NPI_APP_MAIN T2 ON T1.DOC_NO = T2.DOC_NO
                    where T1.DOC_NO =@DOC_NO and T1.DEPT=@DEPT
                    ");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DOC_NO", SqlDbType.VarChar, DOC_NO));
        opc.Add(DataPara.CreateDataParameter("@DEPT", SqlDbType.VarChar, "PM"));
        DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
        return dt;
    }
    #endregion

    #region HomePage NPI Leader/Top Manager
    private DataTable GetLeader(string DOC_NO, string APPROVER_Levels)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"Select *from TB_NPI_APP_RESULT 
                    where SUB_DOC_NO = @SUB_DOC_NO and APPROVER_Levels = @APPROVER_Levels
                    ");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@SUB_DOC_NO", SqlDbType.VarChar, DOC_NO));
        opc.Add(DataPara.CreateDataParameter("@APPROVER_Levels", SqlDbType.VarChar, APPROVER_Levels));
        DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
        return dt;
    }
    #endregion

    #region HomePage PR附件資料
    private DataTable GetPR(string DOC_NO)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"Select *from [TB_NPI_APP_PR_ATTACHFILE] 
                    where SUB_DOC_NO = @SUB_DOC_NO 
                    ");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@SUB_DOC_NO", SqlDbType.VarChar, DOC_NO));
        DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
        return dt;
    }

    private DataTable GetPRModify(string DOC_NO)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"Select T1.*,T2.* from [TB_NPI_APP_PR_ATTACHFILE] T1
                    LEFT JOIN dbo.TB_NPI_APP_RESULT T2 on T1.CASEID = T2.CASEID AND T1.DEPT = T2.DEPT
                    where T1.SUB_DOC_NO = @SUB_DOC_NO
                    ");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@SUB_DOC_NO", SqlDbType.VarChar, DOC_NO));
        DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
        return dt;
    }

    private DataTable GetPMModify(string DOC_NO)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"Select T1.*,T2.CheckedEName from dbo.TB_NPI_APP_ISSUELIST_ATTACHFILE T1
                    LEFT JOIN dbo.TB_NPI_APP_MEMBER T2 on T1.SUB_DOC_NO = T2.DOC_NO
                    where T1.SUB_DOC_NO = @SUB_DOC_NO and DEPT ='PM'
                    ");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@SUB_DOC_NO", SqlDbType.VarChar, DOC_NO));
        DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
        return dt;
    }
    #endregion

    #region HomePage PR簽核資料
    private DataTable GetPRResult(string DOC_NO)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"Select * from [TB_NPI_APP_RESULT] 
                    where SUB_DOC_NO = @SUB_DOC_NO and APPROVER_Levels = 'Dept.Checked'
                    ");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@SUB_DOC_NO", SqlDbType.VarChar, DOC_NO));
        DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
        return dt;
    }
    #endregion

    #region DFX Report 撈出TB_DFX_Score的數據
    private DataTable DFXScoreMaster(string DFXNo)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select * from  TB_DFX_Score where  DFXNo=@DFXNo and Result != 'NA'");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DFXNo", SqlDbType.NVarChar, DFXNo));
        DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
        return dt;
    }

    private DataTable DFXScoreMaster(string DFXNo, string Dept)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select * from  TB_DFX_Score where  DFXNo=@DFXNo and Dept = @Dept");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DFXNo", SqlDbType.NVarChar, DFXNo));
        opc.Add(DataPara.CreateDataParameter("@Dept", SqlDbType.NVarChar, Dept));
        DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
        return dt;
    }
    #endregion

    #region 壓縮方法
    protected void Compressedfile(string docno, string FileName)
    {
        DataTable dtMaster = GetMaster(docno);//基本資料
        DataRow drM = dtMaster.Rows[0];
        string Model = drM["MODEL_NAME"].ToString();
        string Stage = drM["SUB_DOC_PHASE"].ToString();
        string floderPath = Server.MapPath("../E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + FileName);
        string floderPathZip = Server.MapPath("../E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + FileName + ".zip");
        BonkerZip tool = new BonkerZip();
        tool.AddFile(floderPath);
        tool.CompressionZip(floderPathZip);
    }

    protected void Compressedfile(string docno, string FileName, string FilePath)
    {
        DataTable dtMaster = GetMaster(docno);//基本資料
        DataRow drM = dtMaster.Rows[0];
        string Model = drM["MODEL_NAME"].ToString();
        string Stage = drM["SUB_DOC_PHASE"].ToString();
        string floderPath = Server.MapPath("../E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + FileName);
        string floderPathZip = Server.MapPath("../E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + FileName + ".zip");
        BonkerZip tool = new BonkerZip();
        tool.AddFile(floderPath);
        bool B = tool.CompressionZip(floderPathZip);
        if (B == true)
        {
            DeleteDirectory(FilePath);
        }
    }
    #endregion

    #region 計算 Maxpoints的和
    private DataTable SUMMaxPoints(string DFXNo, string WriteDept)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select SUM(convert(int,MaxPoints)) as PossiblePoints from  TB_DFX_ItemBody where  DFXNo=@DFXNo and MaxPoints != 'NA' and WriteDept = @WriteDept");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DFXNo", SqlDbType.NVarChar, DFXNo));
        opc.Add(DataPara.CreateDataParameter("@WriteDept", SqlDbType.NVarChar, WriteDept));
        DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
        return dt;
    }
    #endregion

    #region 計算DFX Points的和
    private DataTable SUMDFXPoints(string DFXNo, string WriteDept)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select SUM(convert(int,DFXPoints)) as CompliancePoints from  TB_DFX_ItemBody where  DFXNo=@DFXNo  and WriteDept = @WriteDept");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DFXNo", SqlDbType.NVarChar, DFXNo));
        opc.Add(DataPara.CreateDataParameter("@WriteDept", SqlDbType.NVarChar, WriteDept));
        DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
        return dt;
    }
    #endregion

    #region DFX HomePage 簽核人抓取 Dept.Write
    private DataTable GetDFXWrite(string DocNo, string Dept)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"Select * from [TB_NPI_APP_MEMBER] 
                    where DOC_NO = @DOC_NO and Category = 'DFX TeamMember' and DEPT = @DEPT
                    ");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DOC_NO", SqlDbType.VarChar, DocNo));
        opc.Add(DataPara.CreateDataParameter("@DEPT", SqlDbType.VarChar, Dept));
        DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
        return dt;
    }
    #endregion

    #region DFX HomePage 簽核人抓取 ReplyCheck
    private DataTable GetDFXCheck(string DocNo, string Dept, string SUB_DOC_NO)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
                  select APPROVER,APPROVER_DATE from TB_NPI_APP_RESULT
                  where APPROVER = ( Select CheckedEName from [TB_NPI_APP_MEMBER] 
                    where DOC_NO = @DOC_NO and Category = 'DFX TeamMember' and DEPT = @DEPT)
                    and SUB_DOC_NO = @SUB_DOC_NO                    ");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DOC_NO", SqlDbType.VarChar, DocNo));
        opc.Add(DataPara.CreateDataParameter("@DEPT", SqlDbType.VarChar, Dept));
        opc.Add(DataPara.CreateDataParameter("@SUB_DOC_NO", SqlDbType.VarChar, SUB_DOC_NO));
        DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
        return dt;
    }
    #endregion

    #region DFX HomePage 抓取 AUTO TE Compliance 為 N 的資料
    private DataTable GetCompliance(string DocNo, string Dept, string OldItemType)
    {
        string sql = " Select * from [TB_DFX_ItemBody] where DFXNo = @DFXNo and WriteDept = @WriteDept  and Compliance = 'N'";
        sql += " and OldItemType like N'%" + OldItemType + "%'";
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DFXNo", SqlDbType.VarChar, DocNo));
        opc.Add(DataPara.CreateDataParameter("@WriteDept", SqlDbType.VarChar, Dept));
        DataTable dt = sdb.GetDataTable(sql, opc);
        return dt;
    }
    #endregion

    #region DFX HomePage 抓取AUTO TE的Result
    private DataTable GetAUTOTEResult(string DocNo, string Dept)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"SELECT DISTINCT OldItemType
                  FROM [NPI_REPORT].[dbo].[TB_DFX_ItemBody]
                  where DFXNo = @DFXNo and WriteDept = @WriteDept
                  ORDER BY OldItemType");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DFXNo", SqlDbType.VarChar, DocNo));
        opc.Add(DataPara.CreateDataParameter("@WriteDept", SqlDbType.VarChar, Dept));
        DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
        return dt;
    }
    #endregion

    #region 將指定文件夾下的Excel轉換成PDF 并合併
    private void MergerExcelToPDF(string FilePath, string FileName, string FileNameOK)
    {
        string[] files = Directory.GetFiles(FilePath, "*.xlsx");

        Aspose.Cells.License lic = new Aspose.Cells.License();
        string AsposeAC = System.Configuration.ConfigurationSettings.AppSettings["AsposeAC"].ToString();
        lic.SetLicense(AsposeAC);

        Aspose.Pdf.License lic2 = new Aspose.Pdf.License();
        string AsposeAP = System.Configuration.ConfigurationSettings.AppSettings["AsposePDF"].ToString();
        lic2.SetLicense(AsposeAP);

        List<string> pdfs = new List<string>();

        foreach (string myfile in files)
        {
            string filename = Path.GetFileNameWithoutExtension(myfile);
            Aspose.Cells.Workbook book = new Aspose.Cells.Workbook(myfile);
            string toPdfFullPath = FilePath + @"\" + filename + ".pdf";
            pdfs.Add(toPdfFullPath);
            book.Save(toPdfFullPath, Aspose.Cells.SaveFormat.Pdf);
        }
        PdfFileEditor pdfEditor = new PdfFileEditor();
        string PdfName = FileNameOK + @"\" + FileName + ".pdf";
        bool b = pdfEditor.Concatenate(pdfs.ToArray(), PdfName);
        if (b == true)
        {
            DeleteDirectory(FilePath);
        }
    }
    #endregion

    #region 將指定文件夾下的所有PDF合併成一個PDF
    private void MergerPDF(string FilePath, string File)
    {
        string[] files = Directory.GetFiles(FilePath, "*.pdf");
        Aspose.Pdf.License lic2 = new Aspose.Pdf.License();
        string AsposeAP = System.Configuration.ConfigurationSettings.AppSettings["AsposePDF"].ToString();
        lic2.SetLicense(AsposeAP);
        List<string> pdfs = new List<string>();

        foreach (string myfile in files)
        {
            string filename = Path.GetFileNameWithoutExtension(myfile);
            string toPdfFullPath = FilePath + @"\" + filename + ".pdf";
            pdfs.Add(toPdfFullPath);
        }
        PdfFileEditor pdfEditor = new PdfFileEditor();
        string PdfName = FilePath + @"\" + File + ".pdf";
        bool b = pdfEditor.Concatenate(pdfs.ToArray(), PdfName);
    }
    #endregion

    #region 刪除文件夾
    /// <summary>
    /// 刪除文件夾的方法 避免權限問題的報錯
    /// </summary>
    /// <param name="directoryPath"></param>
    public static void DeleteDirectory(string directoryPath)
    {
        DirectoryInfo dir = new DirectoryInfo(directoryPath);
        dir.Delete(true);
    }
    #endregion

    /// <summary>
    /// 在網站目錄創建文件夾
    /// </summary>
    /// <param name="docno"></param>
    /// <param name="caseID"></param>
    /// <param name="Bu"></param>
    /// <param name="Building"></param>
    private void CreateDirectory(string docno, string caseID, string Bu, string Building)
    {
        DataTable dtM = GetMaster(docno);
        DataRow drM = dtM.Rows[0];
        string ModifyFlag = drM["MODIFY_FLAG"].ToString();
        #region 聲明變量
        string MFILE_PATH = string.Empty;
        string MFILE_PATHDFX = string.Empty;
        string MFILE_PATHCTQ = string.Empty;
        string MFILE_PATHIssue = string.Empty;
        string MFILE_PATHFMEA = string.Empty;
        string MFILE_PATHDFXSMT = string.Empty;
        string MFILE_PATHDFXAIRI = string.Empty;
        string MFILE_PATHDFXAUTO = string.Empty;
        string MFILE_PATHDFXSafety = string.Empty;
        string MFILE_PATHDFXSQ = string.Empty;
        string MFILE_PATHDFXUQ = string.Empty;
        string MFILE_PATHDFXEE = string.Empty;
        string MFILE_PATHDFXIE = string.Empty;
        string MFILE_PATHDFXTE = string.Empty;
        string MFILE_PATHDFXDQE = string.Empty;
        string MFILE_PATHPDF = string.Empty;

        string MFILE_PATHCPK = string.Empty;
        string MFILE_PATHCPKEE = string.Empty;
        string MFILE_PATHCPKTE = string.Empty;
        string MFILE_PATHPRSMT = string.Empty;
        string MFILE_PATHPRIE = string.Empty;
        string MFILE_PATHCTQAIRI = string.Empty;
        string MFILE_PATHCTQSQ = string.Empty;
        string MFILE_PATHCTQUQ = string.Empty;
        string MFILE_PATHCTQIQC = string.Empty;
        string MFILE_PATHIssuePM = string.Empty;
        string MFILE_PATHIssueModify = string.Empty;
        #endregion

        #region
        string Model = drM["MODEL_NAME"].ToString();
        string Stage = drM["SUB_DOC_PHASE"].ToString();
        string typeHomePage = "Home Page";
        string typeDFX = "DFX Doc";
        string typeCTQ = "MFG CTQ Doc";
        string typeIssue = "Issue List Doc";
        string typeFMEA = "FMEA List Doc";
        string typeDFXSMT = "SMT DFX";
        string typeDFXAIRI = "AIRI DFX";
        string typeDFXIE = "IE DFX";
        string typeDFXEE = "EE DFX";
        string typeDFXTE = "TE DFX";
        string typeDFXAUTO = "AUTO DFX";
        string typeDFXSQ = "SQ DFX";
        string typeDFXUQ = "UQ DFX";
        string typeDFXDQE = "DQE DFX";
        string typeDFXSafety = "Safety DFX";

        string typeCPK = "Test CPK Doc";
        string typeCPKEE = "EE Doc";
        string typeCPKTE = "TE Doc";
        string typePRSMT = "SMT Doc";
        string typePRIE = "IE Doc";
        string typeCTQSQ = "SQ Doc";
        string typeCTQUQ = "UQ Doc";
        string typeCTQIQC = "IQC Doc";
        string typeCTQAIRI = "AIRI Doc";
        string typeIssuePM = "Issue PM";
        #endregion

        #region Create HomePage
        string docNoPath = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeHomePage);
        string filepath = (docNoPath);
        bool IsDocNoExist = Directory.Exists(docNoPath);
        MFILE_PATH = "Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeHomePage;
        if (!IsDocNoExist)
        {
            Directory.CreateDirectory(docNoPath);
        }
        #endregion

        #region Create CTQ
        string docNoPathCTQ = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeCTQ);
        string filepathCTQ = (docNoPathCTQ);
        bool IsDocNoExistCTQ = Directory.Exists(docNoPathCTQ);
        MFILE_PATHCTQ = "Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeCTQ;
        if (!IsDocNoExistCTQ)
        {
            Directory.CreateDirectory(docNoPathCTQ);
        }
        #endregion

        if (ModifyFlag == "N")
        {
            #region Create DFX Master
            string docNoPathDFX = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeDFX);
            string filepathDFX = (docNoPathDFX);
            bool IsDocNoExistDFX = Directory.Exists(docNoPathDFX);
            MFILE_PATHDFX = "Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeDFX;
            if (!IsDocNoExistDFX)
            {
                Directory.CreateDirectory(docNoPathDFX);
            }
            #endregion

            #region Create Issue List
            string docNoPathIssue = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeIssue);
            string filepathIssue = (docNoPathIssue);
            bool IsDocNoExistIssue = Directory.Exists(docNoPathIssue);
            MFILE_PATHIssue = "Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeIssue;
            if (!IsDocNoExistIssue)
            {
                Directory.CreateDirectory(docNoPathIssue);
            }
            #endregion

            #region Create FMEA List
            string docNoPathFMEA = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeFMEA);
            string filepathFMEA = (docNoPathFMEA);
            bool IsDocNoExistFMEA = Directory.Exists(docNoPathFMEA);
            MFILE_PATHFMEA = "Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeFMEA;
            if (!IsDocNoExistFMEA)
            {
                Directory.CreateDirectory(docNoPathFMEA);
            }
            #endregion

            #region Test CPK
            string docNoPathCPK = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeCPK);
            string filepathCPK = (docNoPathCPK);
            bool IsDocNoExistCPK = Directory.Exists(docNoPathCPK);
            bool IsDocNoExistCPKPDF = Directory.Exists(docNoPathCPK);
            MFILE_PATHCPK = "Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeCPK;
            if (!IsDocNoExistCPK)
            {
                Directory.CreateDirectory(docNoPathCPK);
            }
            #endregion

            #region Create DFX SMT
            string docNoPathDFXSMT = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeDFX + "/" + typeDFXSMT);
            string filepathDFXSMT = (docNoPathDFXSMT);
            bool IsDocNoExistDFXSMT = Directory.Exists(docNoPathDFXSMT);
            MFILE_PATHDFXSMT = "Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeDFX + "/" + typeDFXSMT;
            if (!IsDocNoExistDFXSMT)
            {
                Directory.CreateDirectory(docNoPathDFXSMT);
            }
            #endregion

            #region Create DFX AIRI
            string docNoPathDFXAIRI = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeDFX + "/" + typeDFXAIRI);
            string filepathDFXAIRI = (docNoPathDFXAIRI);
            bool IsDocNoExistDFXAIRI = Directory.Exists(docNoPathDFXAIRI);
            MFILE_PATHDFXAIRI = "Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeDFX + "/" + typeDFXAIRI;
            if (!IsDocNoExistDFXAIRI)
            {
                Directory.CreateDirectory(docNoPathDFXAIRI);
            }
            #endregion

            #region Create DFX AUTO
            string docNoPathDFXAUTO = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeDFX + "/" + typeDFXAUTO);
            string filepathDFXAUTO = (docNoPathDFXAUTO);
            bool IsDocNoExistDFXAUTO = Directory.Exists(docNoPathDFXAUTO);
            MFILE_PATHDFXAUTO = "Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeDFX + "/" + typeDFXAUTO;
            if (!IsDocNoExistDFXAUTO)
            {
                Directory.CreateDirectory(docNoPathDFXAUTO);
            }
            #endregion

            #region Create DFX Safety
            string docNoPathDFXSafety = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeDFX + "/" + typeDFXSafety);
            string filepathDFXSafety = (docNoPathDFXSafety);
            bool IsDocNoExistDFXSafety = Directory.Exists(docNoPathDFXSafety);
            MFILE_PATHDFXSafety = "Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeDFX + "/" + typeDFXSafety;
            if (!IsDocNoExistDFXSafety)
            {
                Directory.CreateDirectory(docNoPathDFXSafety);
            }
            #endregion

            #region Create DFX SQ
            string docNoPathDFXSQ = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeDFX + "/" + typeDFXSQ);
            string filepathDFXSQ = (docNoPathDFXSQ);
            bool IsDocNoExistDFXSQ = Directory.Exists(docNoPathDFXSQ);
            MFILE_PATHDFXSQ = "Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeDFX + "/" + typeDFXSQ;
            if (!IsDocNoExistDFXSQ)
            {
                Directory.CreateDirectory(docNoPathDFXSQ);
            }
            #endregion

            #region Create DFX UQ
            string docNoPathDFXUQ = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeDFX + "/" + typeDFXUQ);
            string filepathDFXUQ = (docNoPathDFXUQ);
            bool IsDocNoExistDFXUQ = Directory.Exists(docNoPathDFXUQ);
            MFILE_PATHDFXUQ = "Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeDFX + "/" + typeDFXUQ;
            if (!IsDocNoExistDFXUQ)
            {
                Directory.CreateDirectory(docNoPathDFXUQ);
            }
            #endregion

            #region Create DFX EE
            string docNoPathDFXEE = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeDFX + "/" + typeDFXEE);
            string filepathDFXEE = (docNoPathDFXEE);
            bool IsDocNoExistDFXEE = Directory.Exists(docNoPathDFXEE);
            MFILE_PATHDFXEE = "Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeDFX + "/" + typeDFXEE;
            if (!IsDocNoExistDFXEE)
            {
                Directory.CreateDirectory(docNoPathDFXEE);
            }
            #endregion

            #region Create DFX IE
            string docNoPathDFXIE = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeDFX + "/" + typeDFXIE);
            string filepathDFXIE = (docNoPathDFXIE);
            bool IsDocNoExistDFXIE = Directory.Exists(docNoPathDFXIE);
            MFILE_PATHDFXIE = "Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeDFX + "/" + typeDFXIE;
            if (!IsDocNoExistDFXIE)
            {
                Directory.CreateDirectory(docNoPathDFXIE);
            }
            #endregion

            #region Create DFX TE
            string docNoPathDFXTE = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeDFX + "/" + typeDFXTE);
            string filepathDFXTE = (docNoPathDFXTE);
            bool IsDocNoExistDFXTE = Directory.Exists(docNoPathDFXTE);
            MFILE_PATHDFXTE = "Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeDFX + "/" + typeDFXTE;
            if (!IsDocNoExistDFXTE)
            {
                Directory.CreateDirectory(docNoPathDFXTE);
            }
            #endregion

            #region Create DFX DQE
            string docNoPathDFXDQE = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeDFX + "/" + typeDFXDQE);
            string filepathDFXDQE = (docNoPathDFXDQE);
            bool IsDocNoExistDFXDQE = Directory.Exists(docNoPathDFXDQE);
            MFILE_PATHDFXDQE = "Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeDFX + "/" + typeDFXDQE;
            if (!IsDocNoExistDFXDQE)
            {
                Directory.CreateDirectory(docNoPathDFXDQE);
            }
            #endregion

            #region Create Issue PM
            string docNoPathIssuePM = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeIssue + "/" + typeIssuePM);
            string filepathIssuePM = (docNoPathIssuePM);
            bool IsDocNoExistIssuePM = Directory.Exists(docNoPathIssuePM);
            MFILE_PATHIssuePM = "Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeIssue + "/" + typeIssuePM;
            if (!IsDocNoExistIssuePM)
            {
                Directory.CreateDirectory(docNoPathIssuePM);
            }
            #endregion

            #region Create CPKEE
            string docNoPathCPKEE = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeCPK + "/" + typeCPKEE);
            string logFileEE = docNoPathCPKEE + "/" + " " + ".txt";
            string filepathCPKEE = (docNoPathCPKEE);
            bool IsDocNoExistCPKEE = Directory.Exists(docNoPathCPKEE);
            MFILE_PATHCPKEE = "Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeCPK + "/" + typeCPKEE;
            if (!IsDocNoExistCPKEE)
            {
                Directory.CreateDirectory(docNoPathCPKEE);
                if (!File.Exists(logFileEE))
                {
                    //文件不存在則建立
                    StreamWriter sw = File.CreateText(logFileEE);
                }
                FileInfo info = new FileInfo(logFileEE);
                info.Attributes = FileAttributes.Hidden;
            }
            #endregion

            #region Create CPKTE
            string docNoPathCPKTE = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeCPK + "/" + typeCPKTE);
            string logFileTE = docNoPathCPKTE + "/" + " " + ".txt";
            string filepathCPKTE = (docNoPathCPKTE);
            bool IsDocNoExistCPKTE = Directory.Exists(docNoPathCPKTE);
            MFILE_PATHCPKTE = "Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeCPK + "/" + typeCPKTE;
            if (!IsDocNoExistCPKTE)
            {
                Directory.CreateDirectory(docNoPathCPKTE);
                if (!File.Exists(logFileTE))
                {
                    //文件不存在則建立
                    StreamWriter sw = File.CreateText(logFileTE);
                }
                FileInfo info = new FileInfo(logFileTE);
                info.Attributes = FileAttributes.Hidden;
            }
            #endregion



        }
        else
        {
            #region Create Modify PM
            string docNoPathIssueModify = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeCTQ + "/" + "PM Doc");
            string filepathIssueModify = (docNoPathIssueModify);
            bool IsDocNoExistIssueModify = Directory.Exists(docNoPathIssueModify);
            MFILE_PATHIssueModify = "Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeCTQ + "/" + "PM Doc";
            if (!IsDocNoExistIssueModify)
            {
                Directory.CreateDirectory(docNoPathIssueModify);
            }
            #endregion
        }

        //CTQ && TEST CPK的子文件夹
        #region Create CTQ SQ
        string docNoPathCTQSQ = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeCTQ + "/" + typeCTQSQ);
        string logFileSQ = docNoPathCTQSQ + "/" + " " + ".txt";
        string filepathCTQSQ = (docNoPathCTQSQ);
        bool IsDocNoExistCTQSQ = Directory.Exists(docNoPathCTQSQ);
        MFILE_PATHCTQSQ = "Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeCTQ + "/" + typeCTQSQ;
        if (!IsDocNoExistCTQSQ)
        {
            Directory.CreateDirectory(docNoPathCTQSQ);
            if (!File.Exists(logFileSQ))
            {
                //文件不存在則建立
                StreamWriter sw = File.CreateText(logFileSQ);
            }
            FileInfo info = new FileInfo(logFileSQ);
            info.Attributes = FileAttributes.Hidden;

        }
        #endregion

        #region Create CTQ AIRI
        string docNoPathCTQAIRI = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeCTQ + "/" + typeCTQAIRI);
        string logFileAIRI = docNoPathCTQAIRI + "/" + " " + ".txt";
        string filepathCTQAIRI = (docNoPathCTQAIRI);
        bool IsDocNoExistCTQAIRI = Directory.Exists(docNoPathCTQAIRI);
        MFILE_PATHCTQAIRI = "Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeCTQ + "/" + typeCTQAIRI;
        if (!IsDocNoExistCTQAIRI)
        {
            Directory.CreateDirectory(docNoPathCTQAIRI);
            if (!File.Exists(logFileAIRI))
            {
                //文件不存在則建立
                StreamWriter sw = File.CreateText(logFileAIRI);

            }
            FileInfo info = new FileInfo(logFileAIRI);
            info.Attributes = FileAttributes.Hidden;


        }
        #endregion

        #region Create CTQ UQ
        string docNoPathCTQUQ = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeCTQ + "/" + typeCTQUQ);
        string logFileUQ = docNoPathCTQUQ + "/" + " " + ".txt";
        string filepathCTQUQ = (docNoPathCTQUQ);
        bool IsDocNoExistCTQUQ = Directory.Exists(docNoPathCTQUQ);
        MFILE_PATHCTQUQ = "Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeCTQ + "/" + typeCTQUQ;
        if (!IsDocNoExistCTQUQ)
        {
            Directory.CreateDirectory(docNoPathCTQUQ);
            if (!File.Exists(logFileUQ))
            {
                //文件不存在則建立
                StreamWriter sw = File.CreateText(logFileUQ);
            }
            FileInfo info = new FileInfo(logFileUQ);
            info.Attributes = FileAttributes.Hidden;

        }
        #endregion

        #region Create CTQ IQC
        string docNoPathCTQIQC = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeCTQ + "/" + typeCTQIQC);
        string logFileIQC = docNoPathCTQIQC + "/" + " " + ".txt";
        string filepathCTQIQC = (docNoPathCTQIQC);
        bool IsDocNoExistCTQIQC = Directory.Exists(docNoPathCTQIQC);
        MFILE_PATHCTQIQC = "Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeCTQ + "/" + typeCTQIQC;
        if (!IsDocNoExistCTQIQC)
        {
            Directory.CreateDirectory(docNoPathCTQIQC);
            if (!File.Exists(logFileIQC))
            {
                //文件不存在則建立
                StreamWriter sw = File.CreateText(logFileIQC);
            }
            FileInfo info = new FileInfo(logFileIQC);
            info.Attributes = FileAttributes.Hidden;


        }
        #endregion

        #region Create PRSMT CTQ
        string docNoPathPRSMT = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeCTQ + "/" + typePRSMT);
        string logFilePRSMT = docNoPathPRSMT + "/" + " " + ".txt";
        string filepathPRSMT = (docNoPathPRSMT);
        bool IsDocNoExistPRSMT = Directory.Exists(docNoPathPRSMT);
        MFILE_PATHPRSMT = "Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeCTQ + "/" + typePRSMT;
        if (!IsDocNoExistPRSMT)
        {
            Directory.CreateDirectory(docNoPathPRSMT);
            if (!File.Exists(logFilePRSMT))
            {
                //文件不存在則建立
                StreamWriter sw = File.CreateText(logFilePRSMT);
            }
            FileInfo info = new FileInfo(logFilePRSMT);
            info.Attributes = FileAttributes.Hidden;

        }
        #endregion

        #region Create PRIE CTQ
        string docNoPathPRIE = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeCTQ + "/" + typePRIE);
        string logFilePRIE = docNoPathPRIE + "/" + " " + ".txt";
        string filepathPRIE = (docNoPathPRIE);
        bool IsDocNoExistPRIE = Directory.Exists(docNoPathPRIE);
        MFILE_PATHPRIE = "Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeCTQ + "/" + typePRIE;
        if (!IsDocNoExistPRIE)
        {
            Directory.CreateDirectory(docNoPathPRIE);
            if (!File.Exists(logFilePRIE))
            {
                //文件不存在則建立
                StreamWriter sw = File.CreateText(logFilePRIE);
            }
            FileInfo info = new FileInfo(logFilePRIE);
            info.Attributes = FileAttributes.Hidden;

        }
        #endregion
    }

    /// <summary>
    /// 在Attachment_PDF文件夾下 創建該機種的PDF文件夾
    /// </summary>
    /// <param name="docno"></param>
    /// <param name="caseID"></param>
    /// <param name="Bu"></param>
    /// <param name="Building"></param>
    private void CreateDirectory_PDF(string docno)
    {
        DataTable dtM = GetMaster(docno);
        DataRow drM = dtM.Rows[0];
        string Model = drM["MODEL_NAME"].ToString();//抓取機種
        string Stage = drM["SUB_DOC_PHASE"].ToString();//抓取Stage
        DirectoryInfo theFolderTW = new DirectoryInfo(@"\\ICM656\Attachment_TW\");//源文件夹 tw

        #region Create PDF Folder TW

        string docNoPathPDFTW = theFolderTW.ToString() + Model + "_NPI_PDF" ;
        string filepathPDFTW = (docNoPathPDFTW);
        bool IsDocNoExistPDFTW = Directory.Exists(docNoPathPDFTW);
        string MFILE_PATHPDFTW = "Web/E-Report/Attachment_TW/" + Model + "_NPI_PDF" ;
        if (!IsDocNoExistPDFTW)
        {
            Directory.CreateDirectory(docNoPathPDFTW);
        }

        #endregion

    }



    /// <summary>
    /// 獲取主表相關資料
    /// </summary>
    /// <param name="SUB_DOC_NO"></param>
    /// <returns></returns>
    private DataTable GetMaster(string SUB_DOC_NO)
    {
        StringBuilder sbM = new StringBuilder();
        sbM.Append(@"select  T1.*,T2.* from TB_NPI_APP_SUB T1 
                    Left join TB_NPI_APP_MAIN T2 ON T1.DOC_NO = T2.DOC_NO
                    where T1.SUB_DOC_NO = @SUB_DOC_NO");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@SUB_DOC_NO", SqlDbType.VarChar, SUB_DOC_NO));
        DataTable dt = sdb.GetDataTable(sbM.ToString(), opc);
        return dt;
    }

    private DataTable GetMaster(string Model, string Empty)
    {
        StringBuilder sbM = new StringBuilder();
        sbM.Append(@"select  T1.*,T2.* from TB_NPI_APP_SUB T1 
                    Left join TB_NPI_APP_MAIN T2 ON T1.DOC_NO = T2.DOC_NO
                     where T2.MODEL_NAME = @MODEL_NAME
                     order by  T1.UPDATE_TIME ");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@MODEL_NAME", SqlDbType.VarChar, Model));
        DataTable dt = sdb.GetDataTable(sbM.ToString(), opc);
        return dt;
    }


    private DataTable GetDept(string PRNFNO)
    {
        StringBuilder sbM = new StringBuilder();
        sbM.Append(@"select  DISTINCT DEPT FROM  TB_NPI_APP_MEMBER where DOC_NO = @DOC_NO  ");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DOC_NO", SqlDbType.VarChar, PRNFNO));
        DataTable dt = sdb.GetDataTable(sbM.ToString(), opc);
        return dt;
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

    private void WriteLog(string strLog)
    {
        strLog = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + "   " + strLog;

        string sPathFileName = Server.MapPath("~/Web/E-Report/RunResultLog.log");
        FileStream fs;
        StreamWriter sw;
        if (File.Exists(sPathFileName))
        {
            fs = new FileStream(sPathFileName, FileMode.Append, FileAccess.Write);
        }
        else
        {
            fs = new FileStream(sPathFileName, FileMode.Create, FileAccess.Write);
        }

        sw = new StreamWriter(fs);
        sw.WriteLine(strLog);
        sw.Close();
        fs.Close();
    }
    #endregion

    #endregion

    #region 壓縮文件夾
    private void CompressedfFolder(string CASEID, string DocNo, string Bu, string Building)
    {
        try
        {
            Compressedfile(DocNo, "Test CPK Doc");
            Compressedfile(DocNo, "Home Page");
            Compressedfile(DocNo, "MFG CTQ Doc");
            Compressedfile(DocNo, "Issue List Doc");
            Compressedfile(DocNo, "FMEA List Doc");
            Compressedfile(DocNo, "DFX Doc");

        }
        catch (Exception ex)
        {

        }
    }
    #endregion

    #region 删除文件夹
    /// <summary>
    /// 刪除多餘的文件夾
    /// </summary>
    /// <param name="CASEID"></param>
    /// <param name="DocNo"></param>
    /// <param name="Bu"></param>
    /// <param name="Building"></param>
    private void DeleteExistFolder(string CASEID, string DocNo, string Bu, string Building)
    {
        DataTable dtM = GetMaster(DocNo);
        DataRow drM = dtM.Rows[0];
        string Model = drM["MODEL_NAME"].ToString();
        string Stage = drM["SUB_DOC_PHASE"].ToString();
        string typeHomePage = "Home Page";
        string typeDFX = "DFX Doc";
        string typeCTQ = "MFG CTQ Doc";
        string typeIssue = "Issue List Doc";
        string typeFMEA = "FMEA List Doc";
        string typeCPK = "Test CPK Doc";
        string FilePath_HomePage = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeHomePage);
        string FilePath_DFX = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeDFX);
        string FilePath_CTQ = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeCTQ);
        string FilePath_Issue = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeIssue);
        string FilePath_FMEA = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeFMEA);
        string FilePath_CPK = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_DOC" + "/" + Stage + "/" + typeCPK);
        DeleteDirectory(FilePath_HomePage);
        DeleteDirectory(FilePath_DFX);
        DeleteDirectory(FilePath_CTQ);
        DeleteDirectory(FilePath_Issue);
        DeleteDirectory(FilePath_FMEA);
        DeleteDirectory(FilePath_CPK);
    }

    /// <summary>
    /// 壓縮文件夾之前 先創建該機種的pdf文件夾 再刪除非該階段的文件夾內容 只保留該階段的文件夾
    /// </summary>
    /// <param name="DocNo"></param>
    private void DeleteFolderForTW(string DocNo)
    {

        DataTable dtM = GetMaster(DocNo);
        DataRow drM = dtM.Rows[0];
        string Model = drM["MODEL_NAME"].ToString();//抓取機種
        string Stage = drM["SUB_DOC_PHASE"].ToString();//抓取Stage
        DirectoryInfo theFolderPDF = new DirectoryInfo(@"\\ICM656\Attachment_TW\");//目标文件夹

        string FilePath = Server.MapPath("~/Web/E-Report/Attachment_TW/" + Model + "_NPI_PDF");//找到該機種在Attachement_TW文件夾內對應的路徑(機種文件夾)
        string FilePathZip = Server.MapPath("~/Web/E-Report/Attachment_TW/" + Model + "_NPI_PDF" + ".zip");//找到該機種在Attachement_TW文件夾內對應的路徑(機種的Zip檔)
        //string FilePathZip = theFolderPDF.ToString() + Model + "_NPI_PDF" + ".zip";//找到該機種在Attachement_TW文件夾內對應的路徑(機種的Zip檔)

        //執行程式前,先檢查Attachment_TW文件夾內是否存在該機種的Zip檔案,如果有先刪除Zip檔案
        if (File.Exists(FilePathZip))
        {
            File.Delete(FilePathZip);
        }

        CreateDirectory_PDF(DocNo);//創建文件夾 防止系統找不到指定文件夾
        CopyFiletoTWFolder(DocNo);//如果資料不存在 Copy Attachment文件夾的內容到Attachment_TW文件夾

        //遍歷Attachment_TW中該機種的文件夾,獲取文件夾名(Stage),將文件夾名與該機種的階段作比較,刪除該機種文件夾中非該Stage的所有文件夾
        DirectoryInfo info = new DirectoryInfo(FilePath);
        DirectoryInfo[] infoarry = info.GetDirectories("*.*", SearchOption.TopDirectoryOnly);
        foreach (DirectoryInfo var in infoarry)
        {
            string FolderStage = var.Name;
            string DeleteFilePath = FilePath + "/" + FolderStage;
            if (FolderStage.ToUpper() != Stage.ToUpper())
            {
                DeleteDirectory(DeleteFilePath);
            }
        }
    }

    /// <summary>
    /// 將該機種該階段生成的PDF Copy一份到Attachment_TW文件夾
    /// </summary>
    /// <param name="DocNo"></param>
    private void CopyFiletoTWFolder(string DocNo)
    {
        DataTable dtM = GetMaster(DocNo);
        DataRow drM = dtM.Rows[0];
        string Model = drM["MODEL_NAME"].ToString();
        string Stage = drM["SUB_DOC_PHASE"].ToString();

        DirectoryInfo theFolderPDF = new DirectoryInfo(@"\\ICM656\Attachment_TW\");//目标文件夹

        #region Copy Issue List PM

        DirectoryInfo theFolder = new DirectoryInfo(@"\\icm656\Attachment\");//源文件夹

        string sourceFileName = Server.MapPath("~/Web/E-Report/Attachment/" + Model + "_NPI_PDF" ); ;//源文件
        string destFileNamePDF = Server.MapPath("~/Web/E-Report/Attachment_TW/" + Model + "_NPI_PDF");//目标文件夹

        if (Directory.Exists(destFileNamePDF) && Directory.Exists(sourceFileName))
        {
            CopyDirectory(sourceFileName, destFileNamePDF);
        }

        #endregion

    }

    #endregion

}
