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


public partial class Web_E_Report_PrelaunchReport : System.Web.UI.Page
{
    SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
    ArrayList opc = new ArrayList();
    int function_id = 1294;
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
                    lblBuilding.Text = oModel_BorgUserInfo._Building;

                }
                dfBeginTime.SelectedDate = DateTime.Today.AddDays(-1);
                dfEndTime.SelectedDate = DateTime.Today;
                btnQuery_click(null, null);

            }

        }

    }

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
        opc.Clear();
        opc.Add(DataPara.CreateProcParameter("@P_BU", SqlDbType.VarChar, 10, ParameterDirection.Input, lblBu.Text));
        opc.Add(DataPara.CreateProcParameter("@P_BUILDING", SqlDbType.VarChar, 10, ParameterDirection.Input, Building));
        opc.Add(DataPara.CreateProcParameter("@P_STAUTS", SqlDbType.VarChar, 10, ParameterDirection.Input, Status));
        opc.Add(DataPara.CreateProcParameter("@P_MODEL", SqlDbType.VarChar, 20, ParameterDirection.Input, Model));
        opc.Add(DataPara.CreateProcParameter("@P_CUSTOMER", SqlDbType.VarChar, 30, ParameterDirection.Input, Customer));
        opc.Add(DataPara.CreateProcParameter("@P_FORMNO", SqlDbType.VarChar, 30, ParameterDirection.Input, FormNo));
        opc.Add(DataPara.CreateProcParameter("@P_BEGDATE", SqlDbType.VarChar, 10, ParameterDirection.Input, bgDate));
        opc.Add(DataPara.CreateProcParameter("@P_ENDDATE", SqlDbType.VarChar, 10, ParameterDirection.Input, endDate));
        DataTable dt = sdb.RunProc2("P_Get_Prelaucn_Report", opc);
        BingGrid(grdList, dt);

        #region 計算各案件狀態的數量
        sbALL.Append(@"
                    SELECT count(*) FROM TB_Prelaunch_Main where
                    convert(varchar(10),Date,121)BETWEEN @BeginDate and @EndDate 
                    and Building = @Building
                    ");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@BeginDate", SqlDbType.VarChar, bgDate));
        opc.Add(DataPara.CreateDataParameter("@EndDate", SqlDbType.VarChar, endDate));
        opc.Add(DataPara.CreateDataParameter("@Building", SqlDbType.VarChar, Building));
        DataTable dtALL = sdb.GetDataTable(sbALL.ToString(), opc);
        txtCaseidCount.Text = dtALL.Rows[0][0].ToString();

        sbPass.Append(@"
                    SELECT count(*) FROM TB_Prelaunch_Main where
                    convert(varchar(10),Date,121)BETWEEN @BeginDate and @EndDate 
                    and Status = 'Finished' and Building = @Building
                  ");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@BeginDate", SqlDbType.VarChar, bgDate));
        opc.Add(DataPara.CreateDataParameter("@EndDate", SqlDbType.VarChar, endDate));
        opc.Add(DataPara.CreateDataParameter("@Building", SqlDbType.VarChar, Building));
        DataTable dtPass = sdb.GetDataTable(sbPass.ToString(), opc);
        txtFinishedCount.Text = dtPass.Rows[0][0].ToString();

        sbPending.Append(@"
                    SELECT count(*) FROM TB_Prelaunch_Main where
                    convert(varchar(10),Date,121)BETWEEN @BeginDate and @EndDate 
                    and Status = 'Pending' and Building = @Building
                  ");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@BeginDate", SqlDbType.VarChar, bgDate));
        opc.Add(DataPara.CreateDataParameter("@EndDate", SqlDbType.VarChar, endDate));
        opc.Add(DataPara.CreateDataParameter("@Building", SqlDbType.VarChar, Building));
        DataTable dtPending = sdb.GetDataTable(sbPending.ToString(), opc);
        txtPendingCount.Text = dtPending.Rows[0][0].ToString();

        sbReject.Append(@"
                    SELECT count(*) FROM TB_Prelaunch_Main where
                    convert(varchar(10),Date,121)BETWEEN @BeginDate and @EndDate 
                    and Status = 'Reject' and Building = @Building
                  ");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@BeginDate", SqlDbType.VarChar, bgDate));
        opc.Add(DataPara.CreateDataParameter("@EndDate", SqlDbType.VarChar, endDate));
        opc.Add(DataPara.CreateDataParameter("@Building", SqlDbType.VarChar, Building));
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
        string CaseId = e.ExtraParams["CaseId"];
        string Bu = e.ExtraParams["Bu"];
        string Building = e.ExtraParams["Building"];
        string command = e.ExtraParams["command"];
        string FormNo = e.ExtraParams["FormNo"];
        switch (command)
        {
            case "DetailInfo":
                string Path = System.Configuration.ConfigurationSettings.AppSettings["WFShowCase"].ToString();
                string URL = Path.Replace("{CASEID}", CaseId);
                X.Js.Call("ReportDetail", URL);
                break;
            case "ExportXLS":
                ExportToExcel(CaseId,Bu,FormNo);
                break;
        }

    }

    private void ExportToExcel(string caseID,string Bu,string FormNo)
    {
        X.Js.Call("ExportXLS",caseID,Bu,FormNo);
    }

    protected void cmbBuilding_Select(object sender, DirectEventArgs e)
    {
        
    }

    protected void btnToExcel_Click(object sender, EventArgs e)
    {
        string json = GridData.Value.ToString();
        #region
        json = json.Replace("CaseId", "案件編號");
        json = json.Replace("Date", "開單日期");
        json = json.Replace("PilotRunNO", "表單單號");
        json = json.Replace("BU", "BU");
        json = json.Replace("Building", "Building");
        json = json.Replace("Applicant", "開單人");
        json = json.Replace("Model", "機種");
        json = json.Replace("Customer", "客戶");
        json = json.Replace("Status", "簽核狀態");
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
        this.Response.AddHeader("Content-Disposition", "attachment; filename=Prelaunch Report.xls");
        XslCompiledTransform xtExcel = new XslCompiledTransform();
        xtExcel.Load(Server.MapPath("../Excel.xsl"));
        xtExcel.Transform(xml, null, this.Response.OutputStream);
        this.Response.End();
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

   
}