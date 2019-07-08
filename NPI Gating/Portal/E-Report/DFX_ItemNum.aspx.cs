using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using System.Data;
using System.Data.OleDb;
using Liteon.ICM.DataCore;
using LiteOn.EA.Model;
using LiteOn.EA.BLL;
using System.Collections;
using System.Text;
using LiteOn.EA.CommonModel;
using LiteOn.EA.Borg.Utility;
using LiteOn.EA.NPIReport.Utility;
using System.Collections;
public partial class Web_E_Report_DFX_ItemNum : System.Web.UI.Page
{
    ArrayList opc = new ArrayList();
    static int function_id = 1275;
    private SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
   
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
                btnInsert.Disabled = true;
                btnDelete.Disabled = true;
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

            BindNumRule(null);
            BindcobNum();
        }
    }
   
    protected void BindcobNum()
    {
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        Model_DFX_PARAM oDFX_PARAM = new Model_DFX_PARAM();
        oDFX_PARAM._FUNCTION_NAME = "NumRule";
        oDFX_PARAM._PARAME_NAME = "CATEGORY";
        oDFX_PARAM._PARAME_ITEM = lblBu.Text.Trim();
        oDFX_PARAM._Building = lblBuilding.Text.Trim();
        DataTable dt = oStandard.PARAME_GetBasicData_Filter(oDFX_PARAM);
        BindData(cobNum, dt);
    }

    protected void cobNum_Select(object sender, DirectEventArgs e)
    {
       
        DataTable dt = new DataTable();
        dt.Columns.Add("PARAME_VALUE1");
        dt.Columns.Add("PARAME_VALUE2");
        DataRow dr = dt.NewRow();
        cobType.Text = "";
        string Type = cobNum.SelectedItem.Value;
        switch (Type)
        {
            case "1":
                dr["PARAME_VALUE1"] = "Dept";
                dr["PARAME_VALUE2"] = "部門";
                dt.Rows.Add(dr);
                BindData(cobType, dt);           
                break;
            case "2":
                dr["PARAME_VALUE1"] = "Type";
                dr["PARAME_VALUE2"] = "類別";
                dt.Rows.Add(dr);
                BindData(cobType, dt);   
                break;
            case "3":
                NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
                NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
                Model_DFX_PARAM oDFX_PARAM = new Model_DFX_PARAM();
                oDFX_PARAM._FUNCTION_NAME = "NumRule";
                oDFX_PARAM._PARAME_NAME = "Type";
                oDFX_PARAM._PARAME_ITEM = lblBu.Text.Trim();
                oDFX_PARAM._Building = lblBuilding.Text.Trim();
                dt = oStandard.PARAME_GetBasicData_Filter(oDFX_PARAM);
                BindData(cobType, dt);
                break;
        }
        BindNumRule(null);
       
    }

    protected void cobType_Select(object sender, DirectEventArgs e)
    {
        BindNumRule(cobType.SelectedItem.Value.Trim());
    }
    
    protected void btnInsert_Click(object sender, DirectEventArgs e)
    {
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        Model_DFX_PARAM oDFX_PARAM = new Model_DFX_PARAM();
        StringBuilder ErrMsg = new StringBuilder();
        string Num = cobNum.SelectedItem.Text.Trim();
        string Type = cobType.SelectedItem.Value.Trim();
        string Value = txtValue.Text.Trim();
        string Code = txtCode.Text.Trim();
        #region [為空驗證]
        if (string.IsNullOrEmpty(Num))
        {
            ErrMsg.Append("請選擇編碼位!/");
        }
        if (string.IsNullOrEmpty(Type))
        {
            ErrMsg.Append("請選擇類型!/");
        }
        if (string.IsNullOrEmpty(Value))
        {
            ErrMsg.Append("請填寫名稱!/");
        }
        if (string.IsNullOrEmpty(Code))
        {
            ErrMsg.Append("請填寫編碼!/");
        }
        if (ErrMsg.ToString().Length > 0)
        {
            Alert(ErrMsg.ToString());
            return;
        }
        #endregion

        Model_DFX_PARAM oModel = new Model_DFX_PARAM();
        oModel._FUNCTION_NAME = "NumRule";
        oModel._PARAME_NAME = Type;
        oModel._PARAME_VALUE1 = Code;
        oModel._PARAME_VALUE2 = Value;
        oModel._UPDATE_USER = lblLogonId.Text;
        oModel._UPDATE_TIME = DateTime.Now;
        oModel._PARAME_ITEM = lblBu.Text;
        oModel._Building = lblBuilding.Text;
        oModel._PARAME_TYPE = "2";

        try
        {

            Dictionary<string, object> result = oStandard.PRAME_RecordOperation(oModel, Status_Operation.ADD);
            if ((bool)result["Result"])
            {
                ShowMsg("新增成功！");
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
        ResetField();
        BindcobNum();
        BindNumRule("");

    }

    protected void btnDelete_Click(object sender, DirectEventArgs e)
    {
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        Model_DFX_PARAM oDFX_PARAM = new Model_DFX_PARAM();

        RowSelectionModel sm = this.grdInfo.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count <= 0)
        {
            Alert("请选择须删除项!");
            return;
        }
        string json = e.ExtraParams["Values"];
        Dictionary<string, string>[] companies = JSON.Deserialize<Dictionary<string, string>[]>(json);
        string id = string.Empty;
        string type = string.Empty;
        string value = string.Empty;
        string errMsg = string.Empty;
        string bu = string.Empty;
        foreach (Dictionary<string, string> row in companies)
        {
            id = row["ID"];
            type = row["PARAME_NAME"];
            value = row["PARAME_VALUE2"];
            bu = row["PARAME_ITEM"];
            try
            {
                DelTeamDept(id);
            }
            catch (Exception ex)
            {
                errMsg += string.Format("刪除錯誤,類型:{0} 名稱:{1},ErrMsg:{2}<br/>", type, value, ex.Message);
            }
        }
        if (errMsg.Length > 0)
        {
            Alert(errMsg);
        }
        else
        {
            ShowMsg("刪除成功!");
        }
        BindNumRule("");
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

                Alert("刪除成功!");


            }
            else
            {
                Alert((string)result["ErrMsg"].ToString());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        BindNumRule("");
    }

    protected void BindNumRule(string Type)
    {
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        Model_DFX_PARAM oDFX_PARAM = new Model_DFX_PARAM();
        oDFX_PARAM._FUNCTION_NAME = "NumRule";
        if (string.IsNullOrEmpty(Type))
        {
            oDFX_PARAM._PARAME_NAME = "Type";
        }
        else
        {
            oDFX_PARAM._PARAME_NAME = Type;
        }
        oDFX_PARAM._PARAME_ITEM = lblBu.Text.Trim();
        oDFX_PARAM._Building = lblBuilding.Text.Trim();
        DataTable dt = oStandard.PARAME_GetBasicData_Filter(oDFX_PARAM);
        BindData(grdInfo, dt);
    }

    private void  ResetField()
    {
       
        txtValue.Text = "";
        txtCode.Text = "";
    }
   
    #region  公共方法
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

    
    #endregion

}
