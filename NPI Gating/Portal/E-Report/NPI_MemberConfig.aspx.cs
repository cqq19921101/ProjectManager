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
using LiteOn.EA.NPIReport.Utility;
using LiteOn.EA.CommonModel;
using LiteOn.EA.Borg.Utility;

public partial class NPI_MemberConfig : System.Web.UI.Page
{
    private SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
    private ArrayList opc = new ArrayList();
    int function_id = 1277;
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
            if (oModel_BorgUserInfo._EXISTS)
            {
                lblSite.Text = Borg_Tools.GetSiteInfo();
                lblBu.Text = oModel_BorgUserInfo._BU;
                lblBuilding.Text = oModel_BorgUserInfo._Building;
            }
            BindDept();
            Model_NPI_MEMBER oModel_Member = new Model_NPI_MEMBER();
            oModel_Member._BU = lblBu.Text;
            oModel_Member._BUILDING = lblBuilding.Text;
            BindMemberList(oModel_Member);
        }
    }

    protected void sbCategory_selectedChange(object sender, DirectEventArgs e)
    {
        Model_NPI_MEMBER oModel_Member = new Model_NPI_MEMBER();
        oModel_Member._BU = lblBu.Text;
        oModel_Member._BUILDING = lblBuilding.Text;
        oModel_Member._CATEGORY = sbCategory.SelectedItem.Value;
        BindMemberList(oModel_Member);
    }

    protected void btnSave_click(object sender, DirectEventArgs e)
    {
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        #region[為空驗證]
        string ErrorMsg = "";
        string Category = sbCategory.SelectedItem.Value;
        string dept = cobDept.SelectedItem.Text;
        string ename = txtEName.Text;
        string cname = txtCName.Text;
        string email = txtEMail.Text;
        DateTime UPDATE_TIME = Convert.ToDateTime(DateTime.Now.ToShortDateString());
        string UPDATE_USERID = lblLogonId.Text;
        
        if(string.IsNullOrEmpty(Category))
        {
            ErrorMsg +="類別,";
        }
        if (string.IsNullOrEmpty(dept))
        {
            ErrorMsg += "部門,";
        }
        if (string.IsNullOrEmpty(ename))
        {
            ErrorMsg += "英文名稱,";
        }
        if (string.IsNullOrEmpty(cname))
        {
            ErrorMsg += "中文名稱,";
        }
        if (string.IsNullOrEmpty(email))
        {
            ErrorMsg += "郵箱地址,";
        }

        if (ErrorMsg.Length > 0)
        {
            Alert(ErrorMsg.Substring(0,ErrorMsg.Length-1) + "不能為空");
            return;
        }
        #endregion



        Model_NPI_MEMBER oModel = new Model_NPI_MEMBER();
        oModel._BU = lblBu.Text;
        oModel._BUILDING = lblBuilding.Text;
        oModel._CATEGORY = Category;
        oModel._DEPT = dept;
        oModel._ENAME = ename;
        oModel._CNAME = cname;
        oModel._EMAIL = email;
        oModel._UPDATE_USERID = lblLogonId.Text;
        oModel._UPDATE_TIME = DateTime.Now;

        try
        {
            Dictionary<string, object> result = oStandard.RecordOperation_NPIMember(oModel, Status_Operation.ADD);
            if ((bool)result["Result"])
            {
                
                Alert("新增團隊成員成功!");
                BindMemberList(oModel);
                
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

    protected void btnDelete_Click(object sender, DirectEventArgs e)
    {
        string Category = string.Empty;
        string Dept = string.Empty;
        string ename = string.Empty;
        string Id = string.Empty;
        RowSelectionModel sm = grdInfo.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count <= 0)
        {
            Alert("請勾選需刪除的信息!");
            return;
        }

        string json = e.ExtraParams["Values"];
        Dictionary<string, string>[] sele = JSON.Deserialize<Dictionary<string, string>[]>(json);
        StringBuilder msg = new StringBuilder();
        foreach (Dictionary<string, string> row in sele)
        {
            Category = row["CATEGORY"].ToString();
            Dept = row["DEPT"].ToString();
            ename = row["ENAME"].ToString();
            Id = row["ID"].ToString();
            NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
            NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
            Model_NPI_MEMBER oModel = new Model_NPI_MEMBER();
            oModel._ID = Id;
            try
            {

                Dictionary<string, object> result = oStandard.RecordOperation_NPIMember(oModel, Status_Operation.DELETE);
                if ((bool)result["Result"])
                {
                    
                   Alert("删除團隊成員成功!");
                    BindMemberList(oModel);
                }
                else
                {
                    Alert((string)result["ErrMsg"].ToString());
                }
            }
           
            catch (Exception ex)
            {
                msg.Append(string.Format("類別:{0},部門:{1},英文名稱:{2} 刪除作業失敗!ErrMsg:{3}<BR/>", Category, Dept, ename, ex.Message));

            }
        }
        if (msg.Length > 0)
        {
            Alert(msg.ToString());
        }
        else
        {
            Alert(string.Format("刪除作業成功!"));
          
        }

    }

    protected void txtEName_change(object sender, DirectEventArgs e)
    {

        string enName = txtEName.Text.Trim();
        Model_BorgUserInfo oModel_BorgUserInfo = new Model_BorgUserInfo();
        Borg_User oBorg_User = new Borg_User();
        oModel_BorgUserInfo = oBorg_User.GetUserInfoByLogonId(enName);
        if (oModel_BorgUserInfo._EXISTS)
        {
            
            txtCName.Text = oModel_BorgUserInfo._Name;
             txtEMail.Text = oModel_BorgUserInfo._EMAIL;
        }
        else
        {
            Alert("英文名" + enName + "不存在,请确认!");
        }
    }

    private void BindMemberList(Model_NPI_MEMBER oModel_Member)
    {
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        DataTable dt = oStandard.GetNPIMemeberList(oModel_Member);
        BindData(grdInfo, dt);
    }

    protected void cobDept_Select(object sender, DirectEventArgs e)
    {

        Model_NPI_MEMBER oModel_Member = new Model_NPI_MEMBER();
        oModel_Member._BU = lblBu.Text;
        oModel_Member._BUILDING = lblBuilding.Text;
        oModel_Member._CATEGORY = sbCategory.SelectedItem.Value;
        oModel_Member._DEPT = cobDept.SelectedItem.Text.Trim();
        BindMemberList(oModel_Member);
    }

    protected void BindDept()
    {
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        Model_APPLICATION_PARAM oModel_Param=new Model_APPLICATION_PARAM();
        oModel_Param._APPLICATION_NAME="NPI_REPORT";
        oModel_Param._FUNCTION_NAME = "Configuration";
        oModel_Param._PARAME_NAME = "SubscribeDept";
        oModel_Param._PARAME_ITEM = lblBu.Text.Trim();
        oModel_Param._PARAME_VALUE1 = lblBuilding.Text.Trim();
        DataTable dt = oStandard.PARAME_GetBasicData_Filter(oModel_Param);
        BindData(cobDept, dt);
        
    }
    #region [公共方法]
    private void BindData(ComboBox cmb, DataTable dt)
    {
        cmb.Store.Primary.DataSource= dt;
        cmb.Store.Primary.DataBind();
    }

    private void BindData(GridPanel grd, DataTable dt)
    {
        grd.Store.Primary.DataSource = dt;
        grd.Store.Primary.DataBind();
    }

    /// <summary>
    /// 自定義ALERT方法
    /// </summary>
    /// <param name="msg"></param>
    private void Alert(string msg)
    {
        X.Msg.Alert("Alert", msg).Show();
    }

    #endregion
}
