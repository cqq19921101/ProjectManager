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
using LiteOn.ea.SPM3G.UI;
using Ext.Net;
using LiteOn.EA.DAL;

public partial class template : System.Web.UI.Page
{
    private SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("SPM"));
    ArrayList opc = new ArrayList();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGridPanel(txtMODEL_NAME.Text.Trim());
            txtAPPLY_DATE.Text = DateTime.Now.ToString("yyyy/MM/dd");
            //pnlSignInfo.Visible = false;
        }
        
    }

    protected void grdDocList_RowCommand(object sender, DirectEventArgs e)
    {
        string phase = e.ExtraParams["PHASE"];
        string version = e.ExtraParams["SUB_DOC_PHASE_VERSION"];
        string productName = e.ExtraParams["MODEL_NAME"];
        string doc_No =  e.ExtraParams["DOC_NO"];
        string sub_Doc_No = e.ExtraParams["SUB_DOC_NO"];
        txtDoc_No.Text = doc_No;
        txtSub_No.Text = sub_Doc_No;
        txtCurrentStage.Text = phase + "-" + version;
        txtProductName.Text = productName;
        //txtAPPLY_DATE.Text = DateTime.Now.ToString("yyyy/MM/dd");
        txtCTQFail.Text =  getCTQFailCount(doc_No, sub_Doc_No);
        txtCLCAOpen.Text = getCLCAOpenCount(doc_No, sub_Doc_No);
        txtDFXRpn.Text = getDFXRpnCount(productName, phase);
        //Alert(rgStage.CheckedItems[0].BoxLabel);

    }

    protected void txtMODEL_NAME_change(object sender, DirectEventArgs e)
    {
        string model_Name = txtMODEL_NAME.Text;
        BindGridPanel(model_Name.Trim());

    }

    private string  getCTQFailCount(string doc_No, string sub_Doc_No)
    {
        string ctqFail = "select count(1) as failCount from TB_NPI_APP_CTQ where DOC_NO=@DOC_NO and SUB_DOC_NO=@SUB_DOC_NO and RESULT = 'Fail'";
        opc.Clear();
        opc.Add(DataPara.CreateProcParameter("@DOC_NO", SqlDbType.VarChar, 30, ParameterDirection.Input, doc_No));
        opc.Add(DataPara.CreateProcParameter("@SUB_DOC_NO", SqlDbType.VarChar, 30, ParameterDirection.Input, sub_Doc_No));
        return  sdb.GetRowString(ctqFail, opc, "failCount");
    }

    private string getCLCAOpenCount(string doc_No, string sub_Doc_No)
    {
        string clcaOpen = "select count(1) as openCount from TB_NPI_APP_CLCA where DOC_NO=@DOC_NO and SUB_DOC_NO=@SUB_DOC_NO and RESULT = 'OPEN'";
        opc.Clear();
        opc.Add(DataPara.CreateProcParameter("@DOC_NO", SqlDbType.VarChar, 30, ParameterDirection.Input, doc_No));
        opc.Add(DataPara.CreateProcParameter("@SUB_DOC_NO", SqlDbType.VarChar, 30, ParameterDirection.Input, sub_Doc_No));
        return sdb.GetRowString(clcaOpen, opc, "openCount");
    }

    private string  getDFXRpnCount(string modelName, string Phase)
    {
        string sqlMain = "select * From TB_DFX_Main where model=@model and Phase=@Phase";
        opc.Clear();
        opc.Add(DataPara.CreateProcParameter("@model", SqlDbType.VarChar, 50, ParameterDirection.Input, modelName));
        opc.Add(DataPara.CreateProcParameter("@Phase", SqlDbType.VarChar, 50, ParameterDirection.Input, Phase));
        string dfxNo = sdb.GetRowString(sqlMain, opc, "DFXNo");

        string sqlDFXRPN = "select COUNT(1) as dfxCount From TB_DFX_FMEA where DFXNo=@DFXNo and RPN<90";
        opc.Clear();
        opc.Add(DataPara.CreateProcParameter("@DFXNo", SqlDbType.VarChar, 50, ParameterDirection.Input, dfxNo));
        return  sdb.GetRowString(sqlDFXRPN, opc, "dfxCount");
        
    }

    private void BindGridPanel(string modelName)
    {
        string sql;
        opc.Clear();
        sql = @" select a.*,b.* from TB_NPI_APP_SUB  a 
                 left join TB_NPI_APP_MAIN b on  a.DOC_NO = b.DOC_NO
                 where a.SUB_DOC_PHASE_STATUS = '20' and b.MODEL_NAME like '%"+modelName+"%'";
        DataTable dt = sdb.GetDataTable(sql, opc);
        grdDocList.Store.Primary.DataSource = dt;
        grdDocList.Store.Primary.DataBind();
    }

    private void Alert(string msg)
    {
        X.Msg.Alert("Alert", msg).Show();
    }

}
