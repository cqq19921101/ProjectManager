using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using System.Data;
using System.Data.OleDb;
using LiteOn.EA.DAL;
using LiteOn.EA.Model;
using LiteOn.EA.BLL;
using System.Collections;
using System.Text;
using LiteOn.EA.CommonModel;
using LiteOn.EA.Borg.Utility;
using LiteOn.EA.NPIReport.Utility;

public partial class Web_E_Report_DFX_TeamMember : System.Web.UI.Page
{

    ArrayList opc = new ArrayList();
    static int function_id = 1273;
    protected SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
    ////static string RoleID = System.Configuration.ConfigurationSettings.AppSettings["DFX_ProcessRoleID"].ToString();
    //static  string[] Role = RoleID.Split(',');
   

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

        if (!IsPostBack)
        {
            UserRole UserRole_class = new UserRole();
            if (UserRole_class.checkRole(lblLogonId.Text, function_id) == false)
            {
                Alert("对不起，你没有权限操作!");
                return;
            }
            Model_BorgUserInfo oModel_BorgUserInfo = new Model_BorgUserInfo();
            Borg_User oBorg_User = new Borg_User();
            oModel_BorgUserInfo = oBorg_User.GetUserInfoByLogonId(lblLogonId.Text);
            if(oModel_BorgUserInfo._EXISTS)
            {
                lblSite.Text = Borg_Tools.GetSiteInfo();
                lblBu.Text = oModel_BorgUserInfo._BU;
                txtBuilding.Text = oModel_BorgUserInfo._Building;
            }
            BindDept();             
        }
    }

    protected void cobDept_Select(object sender, DirectEventArgs e)
    {
        string Dept=cobDept.SelectedItem.Text;
        BindTeamMember(Dept);
    }
     
    protected void txtEnName_Change(object sender, DirectEventArgs e)
    {
        string enName = txtEnName.Text.Replace(" ", "");
        Model_BorgUserInfo oModel_BorgUserInfo = new Model_BorgUserInfo();
        Borg_User oBorg_User = new Borg_User();
        oModel_BorgUserInfo = oBorg_User.GetUserInfoByLogonId(enName);
        if (oModel_BorgUserInfo._EXISTS)
        {
            txtCnName.Text = oModel_BorgUserInfo._Name;

        }
        else
        {
            txtCnName.Text = "";
            Alert("英文名" + enName + "不存在,请确认!");
        }

    }

    #region[數據操作]
    protected void btnInsert_Click(object sender, DirectEventArgs e)
    {
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        string dept = cobDept.SelectedItem.Text;
        string enName = txtEnName.Text.Replace(" ", "");
        string cnName = txtCnName.Text;

        #region [控件值Check]
        if (dept.Length <= 0)
        {
            Alert("请选择责任部门!");
            cobDept.Focus();
            return;
        }

        if (enName.Length <= 0)
        {
            Alert("请填写英文名称!");
            txtEnName.Focus();
            return;
        }

        if (cnName.Length <= 0)
        {
            Alert("中文名称不存在,请确认!");
            return;
        }

 

        #endregion

        Model_DFX_PARAM oModel=new Model_DFX_PARAM();
        oModel._FUNCTION_NAME = "TeamMember";
        oModel._PARAME_NAME = "Member";
        oModel._PARAME_VALUE1 = dept;
        oModel._PARAME_VALUE2 = enName;
        oModel._PARAME_VALUE3 = cnName;
        oModel._UPDATE_USER = lblLogonId.Text;
        oModel._UPDATE_TIME = DateTime.Now;
        oModel._PARAME_ITEM = lblBu.Text;
        oModel._Building = txtBuilding.Text;
        oModel._PARAME_TYPE = "1";

        try
        {

            Dictionary<string, object> result = oStandard.PRAME_RecordOperation(oModel, Status_Operation.ADD);
            if ((bool)result["Result"])
            {
                //foreach (string list in Role)
                //{
                //    oStandard.Role_Operation(list,enName, "Add",lblLogonId.Text);
                //}
                ShowMsg("新增團隊成員成功!");
               
                BindTeamMember(cobDept.SelectedItem.Text);
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
        Reset();
       
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
            id = row["ID"];
            dept = row["PARAME_VALUE1"];
            enName = row["PARAME_VALUE2"];
            try
            {
              
                DelTeamMenber(id,enName);
            }
            catch (Exception ex)
            {
                errMsg += string.Format("删除责任部门{0},英文名称{1}错误:{2}<br/>", dept, enName, ex.Message);
            }
        }
        if (errMsg.Length > 0)
        {
            Alert(errMsg); 
        }
       
        BindTeamMember(cobDept.SelectedItem.Text);

    }

    private void DelTeamMenber(string id,string enName)
    {
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        Model_DFX_PARAM oModel = new Model_DFX_PARAM();
        oModel._ID = id;
        try
        {

            Dictionary<string, object> result = oStandard.PRAME_RecordOperation(oModel, Status_Operation.DELETE);
            if ((bool)result["Result"])
            {

               
                //foreach (string list in Role)
                //{
                //    oStandard.Role_Operation(list,enName, "Delete",null);
                //}
                ShowMsg("删除團隊成員成功!");
                BindTeamMember(cobDept.SelectedItem.Text);
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

    #endregion
   
    
    private void BindTeamMember(string dept)
    {

        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        Model_DFX_PARAM oDFX_PARAM = new Model_DFX_PARAM();
        oDFX_PARAM._FUNCTION_NAME = "TeamMember";
        oDFX_PARAM._PARAME_NAME = "Member";
        oDFX_PARAM._PARAME_ITEM = lblBu.Text.Trim();
        oDFX_PARAM._Building = txtBuilding.Text.Trim();
        oDFX_PARAM._PARAME_VALUE1 = dept;
        DataTable dt = oStandard.PARAME_GetBasicData_Filter(oDFX_PARAM);
        BindData(grdInfo, dt);
    }

    private void BindDept()
    {

        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        Model_DFX_PARAM oDFX_PARAM = new Model_DFX_PARAM();
        oDFX_PARAM._FUNCTION_NAME = "TeamMember";
        oDFX_PARAM._PARAME_NAME = "Dept";
        oDFX_PARAM._PARAME_ITEM = lblBu.Text.Trim();
        oDFX_PARAM._Building = txtBuilding.Text.Trim();
        oDFX_PARAM._PARAME_VALUE3 =lblLogonId.Text;
        DataTable dt = oStandard.PARAME_GetBasicData_Filter(oDFX_PARAM);
        BindData(cobDept, dt);
        if (dt.Rows.Count > 0)
        {
            BindTeamMember(dt.Rows[0]["PARAME_VALUE1"].ToString());
        }
    }

    private void Reset()
    {
        txtEnName.Text= string.Empty;
        txtCnName.Text = string.Empty;
    }

    /****************公共方法*****************/
    private void BindData(ComboBox combobox, DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            combobox.Store.Primary.DataSource = dt;
        }
        else
        {
            combobox.Store.Primary.DataSource = String.Empty;
        }

        combobox.DataBind();
    }

    private void BindData(GridPanel grd, DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            grd.Store.Primary.DataSource = dt;
        }
        else
        {
            grd.Store.Primary.DataSource = String.Empty;
        }

        grd.DataBind();

    }

    private void Alert(string msg)
    {
        if (msg.Length > 0)
        {
            X.Msg.Alert("提示", msg).Show();
        }

    }

    private void ShowMsg(string msg)
    {
        if (msg.Length > 0)
        {
            X.Msg.Notify("操作提示", msg).Show();
        }
    }

  
}
