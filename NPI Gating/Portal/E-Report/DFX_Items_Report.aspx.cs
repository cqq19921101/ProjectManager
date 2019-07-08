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
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Xsl;
public partial class Web_DFX_DFX_Items_Update : System.Web.UI.Page
{

    static int function_id = 1224;
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

            string sql = "SELECT * FROM TB_DFX_Item ";
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("SPM"));
            ArrayList opc = new ArrayList();
            DataTable dt = sdb.GetDataTable(sql, opc);
            BindData(grdInfo, dt);
        }
    }

    protected void btnExport_click(object sender, EventArgs e)
    {
        string json = GridRowData.Value.ToString();
        StoreSubmitDataEventArgs eSubmit = new StoreSubmitDataEventArgs(json, null);
        XmlNode xml = eSubmit.Xml;

        this.Response.Clear();
        this.Response.ContentType = "application/vnd.ms-excel";
        this.Response.AddHeader("Content-Disposition", "attachment; filename=DFXItemList.xls");
        XslCompiledTransform xtExcel = new XslCompiledTransform();
        xtExcel.Load(Server.MapPath("../Excel.xsl"));
        xtExcel.Transform(xml, null, this.Response.OutputStream);
        this.Response.End();

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
