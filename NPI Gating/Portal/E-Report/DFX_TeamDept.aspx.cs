using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using System.Data;
using System.Data.OleDb;
using LiteOn.EA.Model;
using LiteOn.EA.BLL;
using System.Collections;
using LiteOn.EA.CommonModel;
using LiteOn.EA.Borg.Utility;
using Liteon.ICM.DataCore;
using System.Text;
using LiteOn.EA.NPIReport.Utility;

public partial class Web_E_Report_DFX_TeamDept : System.Web.UI.Page
{


    ArrayList opc = new ArrayList(); 
    static int function_id = 1272;

    private SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
    static string RoleID = System.Configuration.ConfigurationSettings.AppSettings["DFX_MemberRoleID"].ToString();
    static string[] Role = RoleID.Split(',');
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["UserName"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                lblLogonId.Text = Session["UserName"].ToString();
            }
            UserRole UserRole_class = new UserRole();
            if (UserRole_class.checkRole(lblLogonId.Text, function_id) == false)
            {
                Alert("对不起，你没有权限操作!");
                return;
            }
            Model_BorgUserInfo oModel_BorgUserInfo = new Model_BorgUserInfo();
            Borg_User oBorg_User = new Borg_User();
            oModel_BorgUserInfo = oBorg_User.GetUserInfoByLogonId(lblLogonId.Text);
            if (oModel_BorgUserInfo._EXISTS)
            {
                lblSite.Text = Borg_Tools.GetSiteInfo();
                lblBu.Text = oModel_BorgUserInfo._BU;
                txtBuilding.Text = oModel_BorgUserInfo._Building;
                lblBuilding.Text = oModel_BorgUserInfo._Building;
                BindDept();
                BindTeamDept();
                
            }
            
        }
      
      
    }
 
    private string CheckHandler(string handlers)
    {
        string[] handler = handlers.Split(';');
        string handlerMsg = string.Empty;
        foreach (string i in handler)
        {
            SPMBasic spmBasic = new SPMBasic();
            Model_SPMUserInfo userInfo = new Model_SPMUserInfo();
            userInfo = spmBasic.GetUserInfoByEName(i);
            if (!userInfo.Exists)
            {
                handlerMsg += i + ",";
            }
        }

        if (handlerMsg.Length > 0)
        {
            return handlerMsg;
        }
        else
        {
            return "";
        }
    }

    /// <summary>
    /// 綁定部門清單信息
    /// </summary>
    protected void BindTeamDept()
    {
       
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        Model_DFX_PARAM oDFX_PARAM = new Model_DFX_PARAM();
        oDFX_PARAM._FUNCTION_NAME = "TeamMember";
        oDFX_PARAM._PARAME_NAME = "Dept";
        oDFX_PARAM._PARAME_ITEM = lblBu.Text.Trim();
        oDFX_PARAM._Building = txtBuilding.Text.Trim();
        DataTable dtDept = oStandard.PARAME_GetBasicData_Filter(oDFX_PARAM);
        BindData(grdInfo, dtDept);
    }

    #region 【[部門信息數據操作]

    protected void btnInsert_Click(object sender, DirectEventArgs e)
    {
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        string dept = cobDept.SelectedItem.Text;
        string deptCode = txtDeptCode.Text;
        string handlers = txtHandler.Text;
        StringBuilder ErrMsg = new StringBuilder();

        #region [控件值Check]
        if (dept.Length <= 0)
        {
            ErrMsg.Append("請填寫部門</BR>");
        }
        if (cobRule.SelectedIndex < 0)
        {
            ErrMsg.Append("请填寫部门權責</BR>");
        }
        if (cobRule.SelectedItem.Value == "W" || cobRule.SelectedItem.Value == "W&R")
        {
            if (deptCode.Length <= 0)
            {

                ErrMsg.Append("请填寫部门編碼</BR>");

            }
        }

        if (handlers.Length <= 0)
        {
            ErrMsg.Append("請填寫主管英文名稱</BR>");
        }
        string handlerMsg = CheckHandler(handlers);

        if (handlerMsg.Length > 0)
        {

            ErrMsg.Append("主管" + handlerMsg.Substring(0, handlerMsg.Length - 1) + "資料有誤,請確認!</BR>");
            return;
        }

        if (ErrMsg.ToString().Length > 0)
        {
            Alert(ErrMsg.ToString());
            return;
        }

        #endregion

        Model_DFX_PARAM oModel = new Model_DFX_PARAM();
        oModel._FUNCTION_NAME = "TeamMember";
        oModel._PARAME_NAME = "Dept";
        oModel._PARAME_VALUE1 = dept;
        oModel._PARAME_VALUE2 = deptCode;
        oModel._PARAME_VALUE3 = handlers;
        oModel._PARAME_VALUE4 = cobRule.SelectedItem.Value;
        oModel._PARAME_VALUE5 = cobRule.SelectedItem.Text;
        oModel._UPDATE_USER = lblLogonId.Text;
        oModel._UPDATE_TIME = DateTime.Now;
        oModel._PARAME_ITEM = lblBu.Text;
        oModel._Building = txtBuilding.Text;
        oModel._PARAME_TYPE = "0";

        try
        {

            Dictionary<string, object> result = oStandard.PRAME_RecordOperation(oModel, Status_Operation.ADD);
            if ((bool)result["Result"])
            {

              

                Alert("新增團隊成員成功!");
                BindTeamDept();

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
        BindTeamDept();
        ResetField();

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
            enName = row["PARAME_VALUE3"];
            try
            {
                //string[] handler = enName.Split(';');
                //foreach (string i in handler)
                //{
                //    foreach (string list in Role)
                //    {
                        
                //        oStandard.Role_Operation(list, i, "Delete", lblLogonId.Text);
                //    }

                //}
                DelTeamDept(id);
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
            Alert("部門刪除成功!");
        }
        BindTeamDept();

    }

    private void DelTeamDept(string id)
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

                Alert("刪除部門成功!");
                BindTeamDept();

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

    protected void BindDept()
    {
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        Model_APPLICATION_PARAM oModel_Param = new Model_APPLICATION_PARAM();
        oModel_Param._APPLICATION_NAME = "NPI_REPORT";
        oModel_Param._FUNCTION_NAME = "Configuration";
        oModel_Param._PARAME_NAME = "SubscribeDept";
        oModel_Param._PARAME_ITEM = lblBu.Text.Trim();
        oModel_Param._PARAME_VALUE1 = lblBuilding.Text.Trim();
        DataTable dt = oStandard.PARAME_GetBasicData_Filter(oModel_Param);
        BindData(cobDept, dt);

    }
  
    private void BindData(ComboBox cmb, DataTable dt)
    {
        cmb.Store.Primary.DataSource = dt;
        cmb.Store.Primary.DataBind();
    }

    #endregion

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
    
    /// <summary>
    ///重置控件值
    /// </summary>
    private void ResetField()
    {
        cobDept.Text = string.Empty;
        cobRule.Text = string.Empty;
        txtDeptCode.Text = string.Empty;
        txtHandler.Text = string.Empty;

    }

}
