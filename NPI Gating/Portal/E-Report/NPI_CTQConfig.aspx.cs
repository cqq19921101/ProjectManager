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
using LiteOn.EA.DAL;
using Ext.Net;
using System.Text;
using System.Collections.Generic;
using LiteOn.EA.BLL;
using LiteOn.EA.Borg.Utility;
using LiteOn.EA.NPIReport.Utility;
using LiteOn.EA.CommonModel;

public partial class NPI_CTQParameterConfig : System.Web.UI.Page
{
    private SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
    int function_id = 1276;
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
                ExtAspNet.Alert.Show("对不起，你没有权限操作!");
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
            BindCTQList();
        }
    }

    protected void btnDelete_Click(object sender, DirectEventArgs e)
    {
        int id;
        string PROD_GROUP = string.Empty;
        string PHASE = string.Empty;
        string DEPT = string.Empty;
        string PROCESS = string.Empty;
        string CTQ = string.Empty;
        string sql = "delete from  TB_NPI_CTQ where ID=@ID";
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
            id = Convert.ToInt32(row["ID"].ToString());
            PROD_GROUP =  row["PROD_GROUP"].ToString();
            PHASE  =  row["PHASE"].ToString();
            DEPT = row["DEPT"].ToString();
            PROCESS = row["PROCESS"].ToString();
            CTQ = row["CTQ"].ToString();
            try
            {
                ArrayList opc = new ArrayList();
                opc.Add(DataPara.CreateDataParameter("@ID", SqlDbType.Int, id));
                sdb.ExecuteNonQuery(sql, opc);
            }
            catch (Exception ex)
            {
                msg.Append(string.Format("產品類別:{0},階段:{1},部門:{2},製程:{3},CTQ項目:{4} 刪除作業失敗!ErrMsg:{5}<BR/>", PROD_GROUP, PHASE,DEPT,PROCESS,CTQ, ex.Message));

            }
        }
        if (msg.Length > 0)
        {
            Alert(msg.ToString());
        }
        else
        {
            Alert(string.Format("刪除作業成功!"));
            BindCTQList();
        }

    }

    protected void Refresh()
    {
        txtCTQ.Text = "";
        nubGOAL.Text = "";
    }

    protected void btnSave_click(object sender, EventArgs e)
    {
        string ErrorMsg = "";
        string GOAL = nubGOAL.Text;
        string Prod_group = sbProd_group.SelectedItem.Value;
        string PHASE = sbPhase.SelectedItem.Value;
        string dept = cobDept.SelectedItem.Text;
        string process = txtProcess.Text;
        string CTQ = txtCTQ.Text;
        string unit = txtUNIT.Text;
        string spc = txtSPC.Text;
        string SPEC_LIMIT = txtSPEC_LIMIT.Text;
        string CONTROL_TYPE = sbCONTROL_TYPE.SelectedItem.Value;
        string Severity = sbSeverity.SelectedItem.Value;
        string flag = sbFlag.SelectedItem.Value;
        string Building = sbBuilding.SelectedItem.Text;
        //if (!string.IsNullOrEmpty(nubGOAL.Text))
        //{
        //     GOAL = decimal.Parse(nubGOAL.Text);
        //}     
        DateTime UPDATE_TIME = DateTime.Now;
        string UPDATE_USERID = lblLogonId.Text;
        if (string.IsNullOrEmpty(Building))
        {
            ErrorMsg += "Building,";
        }
        if (string.IsNullOrEmpty(Prod_group))
        {
            ErrorMsg += "產品類別,";
        }
        if (string.IsNullOrEmpty(PHASE))
        {
            ErrorMsg += "階段,";
        }
        if (string.IsNullOrEmpty(flag))
        {
            ErrorMsg += "附件狀態,";
        }

        if (string.IsNullOrEmpty(dept))
        {
            ErrorMsg += "部門,";
        }
        if (string.IsNullOrEmpty(process))
        {
            ErrorMsg += "製程,";
        }
        if (string.IsNullOrEmpty(CTQ))
        {
            ErrorMsg += "CTO項目,";
        }
        if (string.IsNullOrEmpty(unit))
        {
            ErrorMsg += "單位,";
        }
        if (string.IsNullOrEmpty(Severity))
        {
            ErrorMsg += "項目嚴重程度,";
        }
        //if (string.IsNullOrEmpty(spc))
        //{
        //    ErrorMsg += "SPC,";
        //}
        //if (string.IsNullOrEmpty(SPEC_LIMIT))
        //{
        //    ErrorMsg += "SPEC_LIMIT,";
        //}
        if (string.IsNullOrEmpty(CONTROL_TYPE))
        {
            ErrorMsg += "CONTROL_TYPE,";
        }
        if (string.IsNullOrEmpty(nubGOAL.Text))
        {
            ErrorMsg += "GOAL,";
        }

        if (ErrorMsg.Length > 0)
        {
            Alert(ErrorMsg.Substring(0, ErrorMsg.Length - 1) + "不能為空");
            return;
        }

        string sqlIsExist = "select 1 from TB_NPI_CTQ where PROD_GROUP = @PROD_GROUP and  PHASE=@PHASE and DEPT=@DEPT and PROCESS=@PROCESS and CTQ=@CTQ";
        ArrayList opcIsExist = new ArrayList();
        opcIsExist.Add(DataPara.CreateDataParameter("@PROD_GROUP", SqlDbType.VarChar, Prod_group));
        opcIsExist.Add(DataPara.CreateDataParameter("@PHASE", SqlDbType.VarChar, PHASE));
        opcIsExist.Add(DataPara.CreateDataParameter("@DEPT", SqlDbType.NVarChar, dept));
        opcIsExist.Add(DataPara.CreateDataParameter("@PROCESS", SqlDbType.NVarChar, process));
        opcIsExist.Add(DataPara.CreateDataParameter("@CTQ", SqlDbType.NVarChar, CTQ));
        DataTable dt = sdb.GetDataTable(sqlIsExist, opcIsExist);
        if (dt.Rows.Count > 0)
        {
            Alert("此CTQ成員已經存在,不可重複添加！");
            return;
        }



        string sql = @"insert into TB_NPI_CTQ 
                     ([PROD_GROUP]
                     ,[PHASE]
                     ,[DEPT]
                     ,[PROCESS]
                     ,[CTQ]
                     ,[UNIT]
                     ,[SPC]
                     ,[SPEC_LIMIT]
                     ,[CONTROL_TYPE]
                     ,[GOAL]
                     ,[SERVITY]
                     ,[flag]
                     ,[UPDATE_TIME]
                     ,[UPDATE_USERID]
                     ,BU
                     ,BUILDING                    
                     )
                      VALUES
                     (@PROD_GROUP
                     ,@PHASE
                     ,@DEPT
                     ,@PROCESS
                     ,@CTQ
                     ,@UNIT
                     ,@SPC
                     ,@SPEC_LIMIT
                     ,@CONTROL_TYPE
                     ,@GOAL
                     ,@SERVITY
                     ,@flag
                     ,@UPDATE_TIME
                     ,@UPDATE_USERID
                     ,@BU
                     ,@BUILDING
                      )";
        ArrayList opc = new ArrayList();
        opc.Add(DataPara.CreateDataParameter("@PROD_GROUP", SqlDbType.VarChar, Prod_group));
        opc.Add(DataPara.CreateDataParameter("@PHASE", SqlDbType.VarChar, PHASE));
        opc.Add(DataPara.CreateDataParameter("@DEPT", SqlDbType.NVarChar, dept));
        opc.Add(DataPara.CreateDataParameter("@PROCESS", SqlDbType.NVarChar, process));
        opc.Add(DataPara.CreateDataParameter("@CTQ", SqlDbType.NVarChar, CTQ));
        opc.Add(DataPara.CreateDataParameter("@UNIT", SqlDbType.VarChar, unit));
        opc.Add(DataPara.CreateDataParameter("@SPC", SqlDbType.NVarChar, spc));
        opc.Add(DataPara.CreateDataParameter("@SPEC_LIMIT", SqlDbType.NVarChar, SPEC_LIMIT));
        opc.Add(DataPara.CreateDataParameter("@CONTROL_TYPE", SqlDbType.VarChar, CONTROL_TYPE));
        opc.Add(DataPara.CreateDataParameter("@GOAL", SqlDbType.NVarChar, GOAL));
        opc.Add(DataPara.CreateDataParameter("@SERVITY", SqlDbType.VarChar, Severity));
        opc.Add(DataPara.CreateDataParameter("@flag", SqlDbType.VarChar, flag));
        opc.Add(DataPara.CreateDataParameter("@UPDATE_TIME", SqlDbType.DateTime, UPDATE_TIME));
        opc.Add(DataPara.CreateDataParameter("@UPDATE_USERID", SqlDbType.VarChar, UPDATE_USERID));
        opc.Add(DataPara.CreateDataParameter("@BU", SqlDbType.VarChar,lblBu.Text));
        opc.Add(DataPara.CreateDataParameter("@BUILDING", SqlDbType.VarChar, Building));
        
        if (sdb.ExecuteNonQuery(sql, opc))
        {
            Alert("新增成功！");
            Refresh();
            BindCTQList();
        }
        else
        {
            Alert("新增失敗！");
            Refresh();
            return;
        }

    }

    private void BindCTQList()
    {
        string sql;
        ArrayList opc = new ArrayList();
        sql = " select *,'' as GOALStr from TB_NPI_CTQ where (PROD_GROUP=@PROD_GROUP or @PROD_GROUP = '') and (PHASE=@PHASE or @PHASE='')"
            + " and BU=@BU AND BUILDING=@Building";
        opc.Add(DataPara.CreateDataParameter("@PROD_GROUP", SqlDbType.VarChar,sbProd_group.SelectedIndex!=-1? sbProd_group.SelectedItem.Value:""));
        opc.Add(DataPara.CreateDataParameter("@PHASE", SqlDbType.VarChar, sbPhase.SelectedIndex!=-1?sbPhase.SelectedItem.Value:""));
        opc.Add(DataPara.CreateDataParameter("@BU", SqlDbType.VarChar,lblBu.Text));
        opc.Add(DataPara.CreateDataParameter("@Building", SqlDbType.VarChar,lblBuilding.Text));
        DataTable dt = sdb.GetDataTable(sql, opc);
        foreach (DataRow dr in dt.Rows)
        {
            string CONTROL_TYPE = dr["CONTROL_TYPE"].ToString();
            string GO = dr["GOAL"].ToString();
            if (CONTROL_TYPE == "Yield%" && GO != "NA")
            {
                dr.BeginEdit();
                decimal GOAL = Convert.ToDecimal(dr["GOAL"].ToString()) * 100;
                string GOAL_STR = Convert.ToString(GOAL);
                dr["GOALStr"] = string.Format("{0}%", GOAL_STR);
                dr.EndEdit();
            }
            else if (GO == "NA")
            {
                dr.BeginEdit();
                dr["GOALStr"] = dr["GOAL"].ToString();
                dr.EndEdit();
            }
            else
            {
                dr.BeginEdit();
                dr["GOALStr"] = dr["GOAL"].ToString();
                dr.EndEdit();
            }


        }
        grdInfo.Store.Primary.DataSource = dt;
        grdInfo.Store.Primary.DataBind();
    }

    protected void sbSelected_change(object sender, DirectEventArgs e)
    {
        BindCTQList();
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

    protected void txtCTQ_Change(object sender, DirectEventArgs e)
    {
        if (Helper.ConvertChinese(txtCTQ.Text.Trim(), "Big5") == Helper.ConvertChinese("植件-AI植件率", "Big5") || Helper.ConvertChinese(txtCTQ.Text.Trim(), "Big5") == Helper.ConvertChinese("SMT贴片-贴片率", "Big5") ||
             Helper.ConvertChinese(txtCTQ.Text.Trim(), "Big5") == Helper.ConvertChinese("植件-RI植件率", "Big5") || Helper.ConvertChinese(txtCTQ.Text.Trim(), "Big5") == Helper.ConvertChinese("剪脚-剪脚率", "Big5")
            || Helper.ConvertChinese(txtCTQ.Text.Trim(), "Big5") == Helper.ConvertChinese("铆合-铆合品质抽检", "Big5") || Helper.ConvertChinese(txtCTQ.Text.Trim(), "Big5") == Helper.ConvertChinese("Mixing Run Un-input", "Big5"))
        {
            nubGOAL.Text = "NA";
            nubGOAL.ReadOnly = true;
        }
    }

    private void BindData(ComboBox cmb, DataTable dt)
    {
        cmb.Store.Primary.DataSource = dt;
        cmb.Store.Primary.DataBind();
    }

    #region 根據Building綁定產品類別
    protected void Bind_Product(object sender, DirectEventArgs e)
    {
        sbProd_group.Text = string.Empty;
        BindProduct();
    }

    private void BindProduct()
    {
        string Building = sbBuilding.SelectedItem.Text;
        DataTable data = GetProduct(Building);
        ComboBox[] cbs = new ComboBox[] { sbProd_group };
        foreach (ComboBox cb in cbs)
        {
            BindCombox(data, cb);
        }

    }

    private DataTable GetProduct(string Building)
    {
        ArrayList opc = new ArrayList();
        string sql = " select PARAME_VALUE2 FROM TB_APPLICATION_PARAM where  APPLICATION_NAME ='NPI_REPORT' and  FUNCTION_NAME = 'Product' AND PARAME_VALUE1 = @PARAME_VALUE1 ";
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@PARAME_VALUE1", SqlDbType.VarChar, Building));
        DataTable dt = sdb.GetDataTable(sql, opc);
        return dt;
    }
    #endregion
    /// <summary>
    /// 自定義ALERT方法
    /// </summary>
    /// <param name="msg"></param>
    private void Alert(string msg)
    {
        X.Msg.Alert("Alert", msg).Show();
    }

    private void BindCombox(DataTable dt, ComboBox cb)
    {
        cb.Store.Primary.DataSource = dt;
        cb.Store.Primary.DataBind();
    }

   
}
