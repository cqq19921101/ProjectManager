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
using System.Text;
using System.IO;
using System.Data.SqlClient;
using LiteOn.EA.CommonModel;
using LiteOn.EA.Borg.Utility;
using Liteon.ICM.DataCore;
using LiteOn.EA.NPIReport.Utility;
using OfficeOpenXml;

public partial class Web_E_Report_NPI_TeamMemeber_Modify : System.Web.UI.Page
{
   
    ArrayList opc = new ArrayList();
    static int function_id = 1299;

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
                lblBu.Text = oModel_BorgUserInfo._BU;
                lblBuilding.Text = oModel_BorgUserInfo._Building;
            }

        }
    }

    protected void btnQuery_click(object sender, DirectEventArgs e)
    {
         SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        string prod_group = string.Empty;
        string model_name = string.Empty;
        string formno = string.Empty;
        string apply_date = string.Empty;
        string approve = string.Empty;
        StringBuilder errMsg = new StringBuilder();
        if (sbProd_group.SelectedIndex < 0)
        {
            errMsg.Append("請選擇產品類別");

        }
       
        if (errMsg.Length > 0)
        {
            Alert(errMsg.ToString());
            return;
        }
        prod_group = sbProd_group.SelectedItem.Value.Trim();
        model_name = txtModel.Text.Trim();
        formno = txtFormNo.Text.Trim();
        approve = txtApprove.Text.Trim();

        apply_date = txtApplyDate.SelectedDate.ToShortDateString() == "1/1/0001" ? "" : txtApplyDate.SelectedDate.ToShortDateString();
        StringBuilder sql = new StringBuilder();
        opc.Clear();
        sql.Append(" SELECT T2.*,T1.MODEL_NAME,T1.PROD_GROUP,T1.CUSTOMER FROM TB_NPI_APP_MAIN  T1");
        sql.Append(" LEFT JOIN TB_NPI_APP_MEMBER T2 ON T1.DOC_NO=T2.DOC_NO");
        sql.Append(" WHERE T1.PROD_GROUP=@PROD_GROUP AND T1.BU=@bu AND T1.BUILDING=@building ");
       
        opc.Add(DataPara.CreateDataParameter("@PROD_GROUP", DbType.String, prod_group));
        opc.Add(DataPara.CreateDataParameter("@bu", DbType.String, lblBu.Text));
        opc.Add(DataPara.CreateDataParameter("@building", DbType.String, lblBuilding.Text));
      

        if (model_name.Length > 0)
        {
            sql.Append(" AND T1.MODEL_NAME like '%" + model_name + "%'");
        }

        if (apply_date.Length > 0)
        {
            sql.Append("AND T1.APPLY_DATE=@APPLY_DATE  ");
            opc.Add(DataPara.CreateDataParameter("@APPLY_DATE", DbType.String, apply_date));
        }
        if (formno.Length > 0)
        {
            sql.Append("AND T1.DOC_NO=@FORM_NO  ");
            opc.Add(DataPara.CreateDataParameter("@FORM_NO", DbType.String,formno));
        }
        if (approve.Length > 0)
        {
            sql.Append(" AND (T2.WriteEname=@ENAME OR T2.ReplyEName=@ENAME OR T2.CheckedEName=@ENAME)");
            opc.Add(DataPara.CreateDataParameter("@ENAME", DbType.String, approve));
        }
        DataTable dt = sdb.TransactionExecute(sql.ToString(), opc);
        BindGrid(dt, grdInfo);
    }

    protected void Alert(string Msg)
    {
        if (Msg.Length > 0)
        {
            X.Msg.Alert("提示", Msg).Show();
        }
    }

    protected void BindGrid(DataTable dt, GridPanel grd)
    {
        grd.Store.Primary.DataSource = dt;
        grd.Store.Primary.DataBind();
    }

  

    protected void AfterEdit(object sender, DirectEventArgs e)
    {
       
        string DOC_NO = e.ExtraParams["DOC_NO"];
        string ID = e.ExtraParams["ID"];
        string WriteEname = e.ExtraParams["WriteEname"];
        string ReplyEName = e.ExtraParams["ReplyEName"];
        string CheckedEName = e.ExtraParams["CheckedEName"];
        string WriteEmail = string.Empty;
        string ReplyEmail = string.Empty;
        string CheckedEmail = string.Empty;

        Model_BorgUserInfo oModel_BorgUserInfo = new Model_BorgUserInfo();
        Borg_User oBorg_User = new Borg_User();
        oModel_BorgUserInfo = oBorg_User.GetUserInfoByLogonId(WriteEname);
        if (oModel_BorgUserInfo._EXISTS)
        {
            WriteEmail = oModel_BorgUserInfo._EMAIL;
        }
        else
        {
            Alert("英文名" + WriteEname + "不存在,请确认!");
            return;
        }
        oModel_BorgUserInfo = oBorg_User.GetUserInfoByLogonId(ReplyEName);
        if (oModel_BorgUserInfo._EXISTS)
        {
            ReplyEmail = oModel_BorgUserInfo._EMAIL;
        }
        else
        {
            Alert("英文名" + ReplyEName + "不存在,请确认!");
            return;
        }
        oModel_BorgUserInfo = oBorg_User.GetUserInfoByLogonId(CheckedEName);
        if (oModel_BorgUserInfo._EXISTS)
        {
            CheckedEmail = oModel_BorgUserInfo._EMAIL;
        }
        else
        {
            Alert("英文名" + WriteEname + "不存在,请确认!");
            return;
        }
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        ////數據庫DB更新操作
        string sql = " update TB_NPI_APP_MEMBER SET WriteEname=@writeEname, WriteEmail=@WriteEmail,"
                   +"  ReplyEName=@ReplyEname,ReplyEmai=@ReplyEmail,CheckedEName=@CheckEname,CheckedEmail=@CheckedEmail,"
                   +"  UPDATE_USERID=@Update_Userid,UPDATE_TIME=@Update_time"
                   +" WHERE DOC_NO=@DocNo AND ID=@ID";
        ArrayList opc = new ArrayList();
        opc.Clear();

        opc.Add(DataPara.CreateDataParameter("@WriteEmail", DbType.String, WriteEmail));
        opc.Add(DataPara.CreateDataParameter("@writeEname", DbType.String, WriteEname));
        opc.Add(DataPara.CreateDataParameter("@DocNo", DbType.String,DOC_NO));
        opc.Add(DataPara.CreateDataParameter("@ReplyEname", DbType.String,ReplyEName));
        opc.Add(DataPara.CreateDataParameter("@ReplyEmail", DbType.String,ReplyEmail));
        opc.Add(DataPara.CreateDataParameter("@CheckEname", DbType.String,CheckedEName));
        opc.Add(DataPara.CreateDataParameter("@CheckedEmail", DbType.String,CheckedEmail));
        opc.Add(DataPara.CreateDataParameter("@Update_Userid", DbType.String,lblLogonId.Text.Trim()));
        opc.Add(DataPara.CreateDataParameter("@Update_time", DbType.DateTime,DateTime.Now));
        opc.Add(DataPara.CreateDataParameter("@ID", DbType.String,ID));


        try
        {
            sdb.TransactionExecute(sql, opc);
            txtApprove.Text = string.Empty;
            btnQuery_click(null, null);

        }
        catch (Exception ex)
        {
            Alert(ex.Message);

        }

    }

}
