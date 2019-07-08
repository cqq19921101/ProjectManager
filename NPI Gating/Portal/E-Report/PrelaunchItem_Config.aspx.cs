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

public partial class Web_E_Report_PrelaunchItem_Config : System.Web.UI.Page
{
    private SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
    ArrayList opc = new ArrayList();
    static int function_id = 1293;

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
            //UserRole UserRole_class = new UserRole();
            //if (UserRole_class.checkRole(lblLogonId.Text, function_id) == false)
            //{
            //    Alert("对不起，你没有权限操作!");
            //    return;
            //}
            if (lblLogonId.Text.ToLower() != "lianzhenyang" && lblLogonId.Text.ToLower() != "xueeryang" && lblLogonId.Text.ToLower() != "jerryachen")
            {
                Alert("对不起，你没有权限操作!");
                Panel8.Disabled = true;
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
            BindDept();
            BindCheckItem("");



        }
      
       

    }

    protected void BindDept()
    {
        DataTable dt = BindParamer("SubscribeDept");
        cobDept.Store.Primary.DataSource = dt;
        cobDept.Store.Primary.DataBind();
       
    }

    protected void CmbDept_SelectedIndexChanged(object sender,DirectEventArgs e)
    {
        BindCheckItem(cobDept.SelectedItem.Value);
    }
   
    protected DataTable BindParamer(string ParameName)
    {
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        Model_APPLICATION_PARAM oModel_Param = new Model_APPLICATION_PARAM();
        oModel_Param._APPLICATION_NAME = "Prelaunch";
        oModel_Param._FUNCTION_NAME = "Configuration";
        oModel_Param._PARAME_NAME = ParameName;
        oModel_Param._PARAME_ITEM = lblBu.Text.Trim();
        oModel_Param._PARAME_VALUE1 = lblBuilding.Text.Trim();
        DataTable dt = oStandard.PARAME_GetBasicData_Filter(oModel_Param);
        return dt;
    }


    protected void btnInsert_Click(object sender, DirectEventArgs e)
    {
       
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        #region[為空驗證]
        string ErrorMsg = "";

        string dept = cobDept.SelectedItem.Text.Trim();
        string CheckItem = txtCheckItem.Text.Trim();
        string AttachmentFlag = cmbAttachment.SelectedItem.Value.Trim();
        string UPDATE_USERID = lblLogonId.Text;

        if (string.IsNullOrEmpty(dept))
        {
            ErrorMsg += "部門,";
        }
        if (string.IsNullOrEmpty(CheckItem))
        {
            ErrorMsg += "項目,";
        }
        if (string.IsNullOrEmpty(AttachmentFlag))
        {
            ErrorMsg += "上傳附件否,";
        }
        if (ErrorMsg.Length > 0)
        {
            Alert(ErrorMsg.Substring(0, ErrorMsg.Length - 1) + "不能為空");
            return;
        }
        #endregion

        Model_PRELAUNCH_CHECKITEMCONFIG oModel = new Model_PRELAUNCH_CHECKITEMCONFIG();
        oModel._Bu = lblBu.Text;
        oModel._Building = lblBuilding.Text;
        oModel._Dept = dept;
        oModel._CheckItem = CheckItem;
        oModel._AttachmentFlag = AttachmentFlag;
        oModel._UpdateUser =UPDATE_USERID;
        oModel._UpdateTime = DateTime.Now;

        try
        {
            Dictionary<string, object> result = oStandard.RecordOperation_PrelaunchItem(oModel, Status_Operation.ADD);
            if ((bool)result["Result"])
            {

                Alert("新增 CheckItem成功!");
                BindCheckItem("");

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

    #region Delete Opertation
    protected void btnDelete_Click(object sender, DirectEventArgs e)
    {
        RowSelectionModel sm = this.grdInfo.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count <= 0)
        {
            Alert("请选择须删除项!");
            return;
        }

        string json = e.ExtraParams["Values"];
        Dictionary<string, string>[] companies = JSON.Deserialize<Dictionary<string, string>[]>(json);
        string ID = string.Empty;
        string CheckItem = string.Empty;
        string errMsg = string.Empty;
        foreach (Dictionary<string, string> row in companies)
        {
            ID = row["ID"].ToString();
            CheckItem = row["CheckItem"].ToString();
            
            try
            {
                DeleteItem(ID);
            }
            catch (Exception ex)
            {
                errMsg += CheckItem + "刪除錯誤:" + ex.Message + "<br/>";
            }

        }
        if (errMsg.Length > 0)
        {
            Alert(errMsg);
        }
        else
        {
           Alert("資料刪除成功!");
        }

    }

    private void DeleteItem(string ID)
    {
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        Model_PRELAUNCH_CHECKITEMCONFIG oModel = new Model_PRELAUNCH_CHECKITEMCONFIG();
        oModel._ID = ID;
        try
        {
            Dictionary<string, object> result = oStandard.RecordOperation_PrelaunchItem(oModel, Status_Operation.DELETE);
            if ((bool)result["Result"])
            {
                Alert("删除成功!");
                BindCheckItem(cobDept.SelectedItem.Text);
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

    #region BatchOperation
    protected  void  RgBatch_Changed(object sender,DirectEventArgs e)
    {

        if (rdBatchY.Checked)
        {
            containerSingle.Hidden = true;
            ContainerBatch.Hidden = false;
        }
        else
        {
            containerSingle.Hidden =false;
            ContainerBatch.Hidden = true;
        }
    }

    protected void btnConfirm_Click(object sender, DirectEventArgs e)
    {

        StringBuilder errMsg = new StringBuilder();
        string file = FileAttachment.FileName;
        //string building = 
        int total_num = 0;
        int ok_num = 0;
        int ng_num = 0;

        if (!FileAttachment.HasFile)
        {
            Alert("請選擇数据文件!");
            return;
        }
        else
        {
            string type = file.Substring(file.LastIndexOf(".") + 1).ToLower();
            if (type == "xlsx")
            {


                //把文件轉成流
                Stream stream = FileAttachment.PostedFile.InputStream;
                if (stream.Length == 8889)
                {
                    Alert("導入的資料表為空,請檢查!");
                    return;
                }
                try
                {

                    DataTable dt = ReadByExcelLibrary(stream);
                    if (dt.Rows.Count > 0)
                    {
                        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
                        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
                        Model_PRELAUNCH_CHECKITEMCONFIG oModel = new Model_PRELAUNCH_CHECKITEMCONFIG();
                        DeleteExist();
                        foreach (DataRow dr in dt.Rows)
                        {

                            total_num = dt.Rows.Count;
                            try
                            {

                                oModel._Bu = lblBu.Text;
                                oModel._Building = lblBuilding.Text;
                                oModel._Dept = dr["Dept"].ToString();
                                oModel._CheckItem = dr["CheckItem"].ToString();
                                oModel._AttachmentFlag =dr["AttachmentFlag"].ToString();
                                oModel._UpdateUser = lblLogonId.Text.Trim(); ;
                                oModel._UpdateTime = DateTime.Now;
                                Dictionary<string, object> result = oStandard.RecordOperation_PrelaunchItem(oModel, Status_Operation.ADD);
                                if ((bool)result["Result"])
                                {
                                   ok_num += 1;

                                }
                                else
                                {
                                   
                                    ng_num += 1;
                                    errMsg.Append(result["Result"].ToString() + "<br/>");
                                }

                            }
                            catch (Exception ex)
                            {
                                Alert(ex.ToString());
                            }

                        }
                       
                    }
                }
                catch (Exception ex)
                {
                    Alert("Excel數據有問題,錯誤信息: " + ex.Message);
                    return;
                }

            }
            else
            {
                Alert("文件類型只能為xlsx");
            }
        }
        BindCheckItem(cobDept.SelectedItem.Text);
        Alert(string.Format("上傳筆數:{0}<BR/>成功筆數:{1}<BR/>失敗筆數:{2}<BR/>錯誤信息:<BR/>{3}", total_num.ToString(), ok_num.ToString(), ng_num.ToString(), errMsg.ToString()));     
    }

    #endregion
   
    protected void BindCheckItem(string Dept)
    {
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        BindData(grdInfo,oStandard.GetPrelaunchCheckItem(lblBu.Text,lblBuilding.Text,Dept));
    }

    private DataTable ReadByExcelLibrary(Stream xlsStream)
    {
        DataTable table = new DataTable();
        using (ExcelPackage package = new ExcelPackage(xlsStream))
        {
            ExcelWorksheet sheet = package.Workbook.Worksheets[1];
            int colCount = sheet.Dimension.End.Column;
            int rowCount = sheet.Dimension.End.Row;
            for (ushort j = 1; j <= colCount; j++)
            {
                table.Columns.Add(new DataColumn(sheet.Cells[1, j].Value.ToString()));
            }
            for (ushort i = 2; i <= rowCount; i++)
            {
                DataRow row = table.NewRow();
                for (ushort j = 1; j <= colCount; j++)
                {
                    row[j - 1] = sheet.Cells[i, j].Value;
                }
                if (!string.IsNullOrEmpty(row[0].ToString()))
                {
                    table.Rows.Add(row);
                }
            }
        }
        return table;
    }

    private void DeleteExist()
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        string sql = "Delete from [TB_Prelaunch_CheckItemConfig] where Building = @Building";
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@Building", DbType.String, lblBuilding.Text));
        sdb.TransactionExecuteNonQuery(sql, opc);
    }

    #region [公共方法]
  
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

  
    #endregion
}
