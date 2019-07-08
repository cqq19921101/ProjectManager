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
using LiteOn.EA.BLL;
using ExtAspNet;
using System.Text;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using LiteOn.EA.Borg.Utility;
using LiteOn.EA.Model;
using LiteOn.EA.CommonModel;

public partial class Report_Maintain : System.Web.UI.Page
{
    SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
    SqlDB sdbspm = new SqlDB(DataPara.GetDbConnectionString("SPM"));
    string sql = string.Empty;
    ArrayList opc = new ArrayList();
    int function_id = 1437;
    protected void Page_Load(object sender, EventArgs e)
    {
        string logonId = (Session["UserName"] != null) ? Session["UserName"].ToString() : "";
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

        }
        lblLogonId.Text = logonId;

    }

    protected void btnQuery_click(object sender, DirectEventArgs e)
    {
        string errorMsg = string.Empty;
        string Building = cmbBuilding.SelectedItem.Text;
        string PM = txtPM.Text;
        string Model = txtModel.Text;
        if (Building.Length == 0)
        {
            errorMsg += "Building ";
        }
        if (errorMsg.Length > 0)
        {
            Alert(errorMsg.Substring(0, errorMsg.Length - 1) + "不能為空");
            return;
        }

        BindNPIReport(Model, PM);

    }

    protected void grdNPI_RowCommand(object sender, DirectEventArgs e)
    {
        string command = e.ExtraParams["command"];
        string DOCNO = e.ExtraParams["DOC_NO"];
        string SubDOCNO = e.ExtraParams["SUB_DOC_NO"];
        string Logonid = lblLogonId.Text;
        DataTable dt = GetBasic(DOCNO, SubDOCNO);
        switch (command)
        {
            case "UpdateReport":
                //if (dt.Rows[0]["NPI_PM"].ToString().ToLower() == Logonid.ToLower())
                //{
                    Refresh();
                    RefreshBasic();
                    pnlBasic.Hidden = true;
                    pnlMember.Hidden = true;
                    txtWDOCNO.Text = e.ExtraParams["DOC_NO"];
                    txtWSubDOCNO.Text = e.ExtraParams["SUB_DOC_NO"];
                    txtWModelName.Text = e.ExtraParams["MODEL_NAME"];
                    winInfo.Show();
                //}
                //else
                //{
                //    Alert("無法編輯其他人的資料！");
                //    return;
                //}
                break;
        }
    }

    protected void cbChoose_Event(object sender, DirectEventArgs e)
    {
        string Type = cmbChoose.SelectedItem.Value;
        string DOCNO = txtWDOCNO.Text;
        string SubDocNo = txtWSubDOCNO.Text;
        switch (Type)
        {
            case "A":
                RefreshBasic();
                RefreshMember();
                BindBasic(DOCNO);//加載基本資料
                pnlBasic.Hidden = false;
                pnlMember.Hidden = true;
                break;
            case "B":
                RefreshBasic();
                RefreshMember();
                pnlBasic.Hidden = true;
                pnlMember.Hidden = false;
                break;
        }
    }

    protected void btnSave_Click(object sender, DirectEventArgs e)
    {
        string errmsg = string.Empty;
        string errmsg2 = string.Empty;
        string DOCNO = txtWDOCNO.Text;
        string Prod_group = sbProd_group.Text;
        string Model = txtWMODEL_NAME.Text;
        string Customer = txtWCUSTOMER.Text;
        string NPIPM = txtWNPIPM.Text.Trim();
        string Sales = txtWSales.Text.Trim();
        string ME = txtWME.Text.Trim();
        string EE = txtWEE.Text.Trim();
        string CAD = txtWCAD.Text.Trim();
        string TPPM = txtWTPPM.Text.Trim();
        StringBuilder sb = new StringBuilder();

        #region 數據非空驗證
        if (Prod_group.Length == 0)
        {
            errmsg += "產品類別,";
        }
        if (Model.Length == 0)
        {
            errmsg += "機種,";
        }
        if (Customer.Length == 0)
        {
            errmsg += "客戶,";
        }
        if (NPIPM.Length == 0)
        {
            errmsg += "NPI PM,";
        }
        if (Sales.Length == 0)
        {
            errmsg += "業務負責人,";
        }
        if (ME.Length == 0)
        {
            errmsg += "ME工程師,";
        }
        if (EE.Length == 0)
        {
            errmsg += "EE工程師,";
        }
        if (CAD.Length == 0)
        {
            errmsg += "CAD工程師,";
        }
        if (TPPM.Length == 0)
        {
            errmsg += "TP PM ,";
        }

        if (errmsg.Length > 0)
        {
            Alert(errmsg.Substring(0, errmsg.Length - 1) + "不能為空");
            return;
        }

        #endregion

        #region Logonid驗證
        if (!CheckUser(NPIPM))
        {
            errmsg2 += "NPI PM,";
        }
        if (!CheckUser(Sales))
        {
            errmsg2 += "業務負責人,";
        }
        if (!CheckUser(ME))
        {
            errmsg2 += "ME工程師,";
        }
        if (!CheckUser(EE))
        {
            errmsg2 += "EE工程師,";
        }
        if (!CheckUser(CAD))
        {
            errmsg2 += "CAD工程師,";
        }
        if (!CheckUser(TPPM))
        {
            errmsg2 += "TP PM,";
        }

        if (errmsg2.Length > 0)
        {
            Alert(errmsg2.Substring(0, errmsg2.Length - 1) + " 帳號不存在");
            return;
        }
        #endregion

        #region 數據處理
        sb.Append(@"Update TB_NPI_APP_MAIN set PROD_GROUP = @PROD_GROUP,MODEL_NAME = @MODEL_NAME,CUSTOMER =@CUSTOMER,
                    NPI_PM = @NPI_PM,SALES_OWNER = @SALES_OWNER,ME_ENGINEER=@ME_ENGINEER,EE_ENGINEER=@EE_ENGINEER,
                    CAD_ENGINEER = @CAD_ENGINEER,TP_PM = @TP_PM
                    where DOC_NO = @DOC_NO");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DOC_NO", SqlDbType.NVarChar, DOCNO));
        opc.Add(DataPara.CreateDataParameter("@PROD_GROUP", SqlDbType.NVarChar, Prod_group));
        opc.Add(DataPara.CreateDataParameter("@MODEL_NAME", SqlDbType.NVarChar, Model));
        opc.Add(DataPara.CreateDataParameter("@CUSTOMER", SqlDbType.NVarChar, Customer));
        opc.Add(DataPara.CreateDataParameter("@NPI_PM", SqlDbType.NVarChar, NPIPM));
        opc.Add(DataPara.CreateDataParameter("@SALES_OWNER", SqlDbType.NVarChar, Sales));
        opc.Add(DataPara.CreateDataParameter("@ME_ENGINEER", SqlDbType.NVarChar, ME));
        opc.Add(DataPara.CreateDataParameter("@EE_ENGINEER", SqlDbType.NVarChar, EE));
        opc.Add(DataPara.CreateDataParameter("@CAD_ENGINEER", SqlDbType.NVarChar, CAD));
        opc.Add(DataPara.CreateDataParameter("@TP_PM", SqlDbType.NVarChar, TPPM));
        try
        {
            sdb.ExecuteNonQuery(sb.ToString(), opc);
            BindBasic(DOCNO);
            Alert("修改成功！");
        }
        catch (Exception ex)
        {
            Alert(ex.Message);
        }
        #endregion
    }

    protected void Select_Catagory(object sender, DirectEventArgs e)
    {
        cmbDept.Text = string.Empty;
        txtWrite.Text = string.Empty;
        txtReply.Text = string.Empty;
        txtCheck.Text = string.Empty;
        grdMember.GetStore().RemoveAll();

        string DOCNO = txtWDOCNO.Text;
        string catagory = TeamCatagory.SelectedItem.Text;
        BindMemberDept(DOCNO, catagory);
    }

    protected void Select_Dept(object sender, DirectEventArgs e)
    {
        string DOCNO = txtWDOCNO.Text;
        string Category = TeamCatagory.SelectedItem.Text;
        string Dept = cmbDept.Text;
        DataTable dt = GetMemberLogonid(DOCNO,Category,Dept);
        if (dt.Rows.Count > 0)
        { 
            DataRow dr = dt.Rows[0];
            txtWrite.Text = dr["WriteEname"].ToString();
            txtReply.Text = dr["ReplyEName"].ToString();
            txtCheck.Text = dr["CheckedEName"].ToString();
        }
        BindGrid(dt, grdMember);
    }

    protected void btnSave_Member(object sender, DirectEventArgs e)
    {
        string errmsg = string.Empty;
        string errmsg2 = string.Empty;
        string DocNo = txtWDOCNO.Text;
        string Category = TeamCatagory.SelectedItem.Text;
        string Dept = cmbDept.Text.Trim();
        string Write = txtWrite.Text.Trim();
        string Reply = txtReply.Text.Trim();
        string Checked = txtCheck.Text.Trim();
        StringBuilder sb = new StringBuilder();

        #region 數據驗證
        if (Dept.Length == 0)
        {
            errmsg += "部門,";
        }

        if (Category.Length == 0)
        {
            errmsg += "分類,";
        }
        else
        {
            if (Category == "DFX TeamMember" || Category == "CTQ TeamMember" || Category == "ISSUES TeamMember" || Category == "PFMEA TeamMember")
            {
                if (Write.Length == 0 || Reply.Length == 0 || Checked.Length == 0)
                {
                    errmsg += "Write,Reply,Checked人員 ,";
                }
            }
            else
            {
                if (Write.Length == 0 || Checked.Length == 0)
                {
                    errmsg += "Write,Checked人員 ,";
                }

            }
        }
        if (errmsg.Length > 0)
        {
            Alert(errmsg.Substring(0, errmsg.Length - 1) + "不能為空");
            return;
        }
        #endregion

        #region Logonid驗證
        if (!CheckUser(Write))
        {
            errmsg2 += "Write人員,";
        }
        if (!CheckUser(Reply))
        {
            errmsg2 += "Reply人員,";
        }
        if (!CheckUser(Checked))
        {
            errmsg2 += "Checked人員,";
        }
        if (errmsg2.Length > 0)
        {
            Alert(errmsg2.Substring(0, errmsg2.Length - 1) + " 帳號不存在");
            return;
        }
        #endregion

        #region 數據處理
        sb.Append(@"Update TB_NPI_APP_MEMBER set WriteEname = @WriteEname,ReplyEName =@ReplyEName,CheckedEName=@CheckedEName
                    where DOC_NO = @DOC_NO and  DEPT = @DEPT ");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DOC_NO", SqlDbType.VarChar, DocNo));
        if (Category == "DFX TeamMember" || Category == "CTQ TeamMember" || Category == "ISSUES TeamMember" || Category == "PFMEA TeamMember")
        {
            sb.Append(" and Category in ('DFX TeamMember','CTQ TeamMember','ISSUES TeamMember','PFMEA TeamMember') ");
        }
        else
        {
            sb.Append(" and Category = @Category");
            opc.Add(DataPara.CreateDataParameter("@Category", SqlDbType.VarChar, Category));
        }
        opc.Add(DataPara.CreateDataParameter("@DEPT", SqlDbType.VarChar, Dept));
        opc.Add(DataPara.CreateDataParameter("@WriteEname", SqlDbType.VarChar, Write));
        opc.Add(DataPara.CreateDataParameter("@ReplyEName", SqlDbType.VarChar, Reply));
        opc.Add(DataPara.CreateDataParameter("@CheckedEName", SqlDbType.VarChar, Checked));
        try
        {
            sdb.ExecuteNonQuery(sb.ToString(), opc);
            Alert("修改成功！");
            BindMember(DocNo,Category,Dept);
        }
        catch (Exception ex)
        {
            Alert(ex.Message);
            return;
        }
        #endregion
    }
    

    #region 數據處理
    private void Refresh()
    {
        txtWDOCNO.Text = string.Empty;
        txtWSubDOCNO.Text = string.Empty;
        cmbChoose.Text = string.Empty;

    }

    private void RefreshBasic()
    {
        sbProd_group.Text = string.Empty;
        txtWMODEL_NAME.Text = string.Empty;
        txtWCUSTOMER.Text = string.Empty;
        txtWNPIPM.Text = string.Empty;
        txtWSales.Text = string.Empty;
        txtWME.Text = string.Empty;
        txtWEE.Text = string.Empty;
        txtWCAD.Text = string.Empty;
        txtWTPPM.Text = string.Empty;

        txtEVTDate.Text = string.Empty;
        txtEVTRemark.Text = string.Empty;
        txtDVTDate.Text = string.Empty;
        txtDVTRemark.Text = string.Empty;
        txtPRDate.Text = string.Empty;
        txtPRRemark.Text = string.Empty;
    }

    private void RefreshMember()
    {
        TeamCatagory.Text = string.Empty;
        cmbDept.Text = string.Empty;
        txtWrite.Text = string.Empty;
        txtReply.Text = string.Empty;
        txtCheck.Text = string.Empty;
        grdMember.GetStore().RemoveAll();
    }

    private void BindNPIReport(string Model, string PM)
    {
        string Building = cmbBuilding.SelectedItem.Text;
        StringBuilder sb = new StringBuilder();
        sb.Append(@"SELECT T1.*,T2.BU,T2.BUILDING,T2.MODEL_NAME,T2.NPI_PM,
                    CONVERT(varchar(10),T1.CREATE_DATE,120) AS Date,
                    SPM.dbo.GetHandler(T1.CASEID) AS handler
                    FROM TB_NPI_APP_SUB T1
                    LEFT JOIN TB_NPI_APP_MAIN T2
                    ON T1.DOC_NO=T2.DOC_NO
                    where Building = @Building");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@Building", SqlDbType.VarChar, Building));
        if (Model.Length > 0)
        {
            sb.Append(" and T2.MODEL_NAME like '%'+@MODEL_NAME+'%'");
            opc.Add(DataPara.CreateDataParameter("@MODEL_NAME", SqlDbType.VarChar, Model.Trim()));
        }
        if (PM.Length > 0)
        {
            sb.Append(" and T2.NPI_PM  like '%'+@PM+'%'");
            opc.Add(DataPara.CreateDataParameter("@PM", SqlDbType.VarChar, PM.Trim()));
        }
        sb.Append(" order by DOC_NO");
        DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
        grdInfo.Store.Primary.DataSource = dt;
        grdInfo.Store.Primary.DataBind();

    }

    private void Alert(string msg)
    {
        X.Msg.Alert("Alert", msg).Show();
    }

    private void BingGrid(GridPanel grd, DataTable dt)
    {
        grd.Store.Primary.DataSource = dt;
        grd.Store.Primary.DataBind();
    }

    private DataTable GetBasic(string DOCNO, string SubDocNo)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"SELECT T1.*,T2.*,
                    CONVERT(varchar(10),T1.CREATE_DATE,120) AS Date,
                    SPM.dbo.GetHandler(T1.CASEID) AS handler
                    FROM TB_NPI_APP_SUB T1
                    LEFT JOIN TB_NPI_APP_MAIN T2
                    ON T1.DOC_NO=T2.DOC_NO
                    where T1.DOC_NO = @DOC_NO and T1.SUB_DOC_NO = @SUB_DOC_NO");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DOC_NO", SqlDbType.VarChar, DOCNO));
        opc.Add(DataPara.CreateDataParameter("@SUB_DOC_NO", SqlDbType.VarChar, SubDocNo));
        return sdb.GetDataTable(sb.ToString(), opc);
    }

    private bool CheckUser(string logonid)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select * from dbo.[User] where LOGONID =@LOGONID");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@LOGONID", SqlDbType.NVarChar, logonid));
        DataTable dt = sdbspm.GetDataTable(sb.ToString(), opc);
        if (dt.Rows.Count > 0)
        {
            return true;
        }
        return false;
    }

    private void BindBasic(string DOCNO)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"SELECT * from TB_NPI_APP_MAIN where DOC_NO = @DOC_NO ");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DOC_NO", SqlDbType.VarChar, DOCNO));
        DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
        if (dt.Rows.Count > 0)
        {
            DataRow drB = dt.Rows[0];
            sbProd_group.Text = drB["PROD_GROUP"].ToString();
            txtWMODEL_NAME.Text = drB["MODEL_NAME"].ToString();
            txtWCUSTOMER.Text = drB["CUSTOMER"].ToString();
            txtWNPIPM.Text = drB["NPI_PM"].ToString();
            txtWSales.Text = drB["SALES_OWNER"].ToString();
            txtWME.Text = drB["ME_ENGINEER"].ToString();
            txtWEE.Text = drB["EE_ENGINEER"].ToString();
            txtWCAD.Text = drB["CAD_ENGINEER"].ToString();
            txtWTPPM.Text = drB["TP_PM"].ToString();

            txtEVTDate.Text = drB["PROJECT_CODE"].ToString();
            txtEVTRemark.Text = drB["PROJECT_CODE_REMARK"].ToString();
            txtDVTDate.Text = drB["TIME_QUANTITY"].ToString();
            txtDVTRemark.Text = drB["TIME_QUANTITY_REMARK"].ToString();
            txtPRDate.Text = drB["PRPhaseTime"].ToString();
            txtPRRemark.Text = drB["PRPhaseTime_Remark"].ToString();
        }
    }

    private void BindMember(string DOCNO, string Catagory, string Dept)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select  Category,DEPT,WriteEname,ReplyEName,CheckedEName from TB_NPI_APP_MEMBER where DOC_NO = @DOC_NO  and  DEPT = @DEPT ");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DOC_NO", SqlDbType.VarChar, DOCNO));
        opc.Add(DataPara.CreateDataParameter("@DEPT", SqlDbType.VarChar, Dept));
        if (Catagory == "DFX TeamMember" || Catagory == "CTQ TeamMember" || Catagory == "ISSUES TeamMember" || Catagory == "PFMEA TeamMember")
        {
            sb.Append(" and Category in ('DFX TeamMember','CTQ TeamMember','ISSUES TeamMember','PFMEA TeamMember') ");
        }
        else
        {
            sb.Append(" and Category = @Category");
            opc.Add(DataPara.CreateDataParameter("@Category", SqlDbType.VarChar, Catagory));
        }
        DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
        BingGrid(grdMember,dt);
    }
    #endregion

    #region 綁定下拉框參數
    protected void BindMemberDept(string DOCNO, string Catagory)
    {
        ComboBox[] cbs = new ComboBox[] { cmbDept };
        DataTable data = GetMemberDept(DOCNO, Catagory);
        foreach (ComboBox cb in cbs)
        {
            BindCombox(data, cb);
        }
    }

    private DataTable GetMemberDept(string DOCNO, string Catagory)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select  DEPT from TB_NPI_APP_MEMBER where DOC_NO = @DOC_NO AND 
                    Category = @Category and  WriteEname != ''");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DOC_NO", SqlDbType.NVarChar, DOCNO));
        opc.Add(DataPara.CreateDataParameter("@Category", SqlDbType.NVarChar, Catagory));
        return sdb.GetDataTable(sb.ToString(), opc);
    }

    private DataTable GetMemberLogonid(string DOCNO, string Catagory,string Dept)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select  Category,DEPT,WriteEname,ReplyEName,CheckedEName from TB_NPI_APP_MEMBER where DOC_NO = @DOC_NO  and  DEPT = @DEPT ");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@DOC_NO", SqlDbType.VarChar, DOCNO));
        opc.Add(DataPara.CreateDataParameter("@DEPT", SqlDbType.VarChar, Dept));
        if (Catagory == "DFX TeamMember" || Catagory == "CTQ TeamMember" || Catagory == "ISSUES TeamMember" || Catagory == "PFMEA TeamMember")
        {
            sb.Append(" and Category in ('DFX TeamMember','CTQ TeamMember','ISSUES TeamMember','PFMEA TeamMember') ");
        }
        else
        {
            sb.Append(" and Category = @Category");
            opc.Add(DataPara.CreateDataParameter("@Category", SqlDbType.VarChar, Catagory));
        }
        DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
        return sdb.GetDataTable(sb.ToString(), opc);
    }

    private void BindCombox(DataTable dt,ComboBox cmb)
    {
        cmb.Store.Primary.DataSource = dt;
        cmb.Store.Primary.DataBind();
    }

    private void BindGrid(DataTable dt, GridPanel grdInfo)
    {
        grdInfo.Store.Primary.DataSource = dt;
        grdInfo.Store.Primary.DataBind();
    }

    #endregion
}

