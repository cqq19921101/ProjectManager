using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using LiteOn.EA.DAL;
using System.Collections;
using System.IO;
using LiteOn.ICM.SIC;

/// <summary>
/// GDS_Helper 的摘要描述
/// </summary>
public class GDS_Helper
{
    static string conn = ConfigurationManager.AppSettings["DBConnectionUAT"];
    static SqlDB sdb = new SqlDB(conn);
    private ArrayList opc = new ArrayList();

    public GDS_Helper()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region XML Datatable互轉
    /// <summary>
    /// datatable convert to xml
    /// </summary>
    /// <param name="dt">datatable</param>
    /// <returns>字符串</returns>
    public string DataTableToXMLStr(DataTable dt)
    {
        StringWriter sw = new StringWriter();
        dt.WriteXml(sw);
        string temp = sw.ToString();
        sw.Close();
        sw.Dispose();
        temp = temp.Replace("&lt;", "(").Replace("&gt;", ")").Replace("&amp;", "/");//替換STR中C#自動轉換后敏感字符<,>,&
        return temp.Replace("<", "&lt1;").Replace(">", "&gt1;");//替換掉<,>，便於存儲於SPM
    }

    /// <summary>
    /// xml convert to datatable
    /// </summary>
    /// <param name="dr">SAP DATA</param>
    /// <param name="type">表身，或表頭</param>
    /// <returns>DataTable</returns>
    public DataTable XMLToDataTable(DataRow dr, string type)
    {
        if (type == "XMLHead")
        {
            DataTable dtHead = LiteOn.GDS.Utility.Tools.BuildHeadTable();
            StringReader sr = new StringReader(dr["xmlhead"].ToString().Trim());
            dtHead.ReadXml(sr);
            return dtHead;
        }
        else
        {
            StringReader sr2 = new StringReader(dr["xmlDetail"].ToString().Trim());
            DataTable dtDetail = LiteOn.GDS.Utility.Tools.BuildDetailTable();
            dtDetail.ReadXml(sr2);
            return dtDetail;
        }
    }
    #endregion

    #region SAP DATA 轉換

    /// <summary>
    /// 根據單號抓取HEAD的資料 將XML轉換成Datatable
    /// </summary>
    /// <param name="FormNo"></param>
    /// <returns></returns>
    public DataTable GetdtHead(string FormNo)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select xmlHead from TB_GDS_DATA where MBLNR_A = @MBLNR_A");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@MBLNR_A", SqlDbType.NVarChar, FormNo));
        DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            return XMLToDataTable(dr, "XMLHead");
        }
        return dt;
    }

    /// <summary>
    /// 根據單號抓取Detail的資料 將XML轉換成Datatable
    /// </summary>
    /// <param name="FormNo"></param>
    /// <returns></returns>
    public DataTable GetdtDetail(string FormNo)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select xmlDetail from TB_GDS_DATA where MBLNR_A = @MBLNR_A");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@MBLNR_A", SqlDbType.NVarChar, FormNo));
        DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            return XMLToDataTable(dr, "XMLDetail");
        }

        return dt;
    }

    #endregion

    #region Call SAP BAPI

    /// <summary>
    /// Call SAP BAPI -- Z_BAPI_GET_SPECIAL_DOC  將單子的結果回傳到SAP  W:表示在簽(Submit的時候)
    /// </summary>
    /// <param name="DocNo" -- 主單號></param>
    public void PostBackToSAP(int CASEID)
    {
        DataTable dt = GetMaster_Exception(CASEID);
        if (dt.Rows.Count > 0)
        {
            DataTable dtUpdate = BuildUpdateTable();//創建一個臨時Table 作為參數 Call BAPI
            foreach (DataRow dr in dt.Rows)
            {
                DataRow TEMP = dtUpdate.NewRow();
                //TEMP["WERKS"] = dr["WERKS"].ToString();//廠別
                //TEMP["MBLNR"] = dr["MBLNR"].ToString();//主單號
                //TEMP["ZEILE"] = dr["ZEILE"].ToString();//對應的ITEM號
                //TEMP["MBLNR_A"] = dr["MBLNR_A"].ToString();//Link 單號
                //TEMP["MATNR"] = dr["MATNR"].ToString();//料號
                //TEMP["MENGE"] = double.Parse(dr["MENGE"].ToString());//退回數量
                //TEMP["STATUS"] = "W";//狀態 Submit的時候 默認 'W' 回傳給SAP

                TEMP["WERKS"] = dr["WERKS"].ToString();//廠別
                TEMP["MBLNR"] = dr["MBLNR"].ToString();//主單號
                TEMP["ZEILE"] = "001";
                TEMP["MBLNR_A"] = dr["MBLNR_A"].ToString();//Link 單號
                TEMP["MATNR"] = "8A034Z";
                TEMP["MENGE"] = double.Parse("1");
                TEMP["STATUS"] = "W";



                dtUpdate.Rows.Add(TEMP);//創建出新的Datatable
            }
            Z_BAPI_GSD_UPDATE(dtUpdate);//將臨時的Datatable作為Call BAPI的參數
        }

    }

    /// <summary>
    /// Call SAP BAPI -- Z_BAPI_GDS_SEND 檢查單據是否過賬  MBLNR欄位不為空 表示已經過賬 返回True 否則返回False
    /// </summary>
    /// <returns></returns>
    public bool CheckFormNoIsPass(string I1DocNo, string IADocNo, string I6DocNo)
    {

        return true;
    }

    /// <summary>
    /// 創建回傳SAP時所需要的臨時的Datatable
    /// </summary>
    /// <returns></returns>
    public DataTable BuildUpdateTable()
    {
        DataTable dt = new DataTable("T_ZTCPCN6D_W");
        dt.Columns.Add(new DataColumn("WERKS", typeof(String)));
        dt.Columns.Add(new DataColumn("MBLNR", typeof(String)));
        dt.Columns.Add(new DataColumn("ZEILE", typeof(String)));
        dt.Columns.Add(new DataColumn("MBLNR_A", typeof(String)));
        dt.Columns.Add(new DataColumn("MATNR", typeof(String)));
        dt.Columns.Add(new DataColumn("MENGE", typeof(double)));
        dt.Columns.Add(new DataColumn("STATUS", typeof(String)));
        return dt;
    }


    /// <summary>
    /// call bapi 回傳單據狀態給SAP  Z_BAPI_GET_SPECIAL_DOC
    /// </summary>
    /// <param name="upDT"></param>
    /// <returns></returns>
    public  bool Z_BAPI_GSD_UPDATE(DataTable upDT)
    {
        bool flag = false;
        Console.WriteLine("- Fetch : Z_BAPI_GET_SPECIAL_DOC");

        StringBuilder sbError = new StringBuilder();
        Hashtable InputParas = new Hashtable();

        Client.ClientParas _clientparas = new Client.ClientParas();
        _clientparas.AppID = "CZ_Workflow";
        _clientparas.SAPFunction = "MM";
        _clientparas.BAPI = "Z_BAPI_GET_SPECIAL_DOC";

        _clientparas.InputParas = InputParas;
        _clientparas.InputTable = new DataTable[1];
        _clientparas.InputTable[0] = upDT;
        _clientparas.OutputParas = new Hashtable();
        bool bReturn = false; ;
        try
        {
            bReturn = LiteOn.ICM.SIC.Client.getSAPData(ref _clientparas);
            if (bReturn)
            {
                int iCount = _clientparas.ResultTable.Length;
                for (int i = 0; i < iCount; i++)
                {
                    Console.WriteLine("- Table=" + _clientparas.ResultTable[i].TableName + " rows = " + _clientparas.ResultTable[i].Rows.Count);
                    if (_clientparas.ResultTable[i].TableName == "T_ZTCPCN6D_W")
                    {
                    }
                }
                flag = true;
            }
            else
            {
                //DBIO.WriteLog("# Fail: " + _clientparas.sErrMsg, appName);
                //Console.WriteLine("# Fail: " + _clientparas.sErrMsg);
            }
        }
        catch (Exception ex)
        {
            //Console.WriteLine(ex.Message);
            //DBIO.WriteLog("# Fail: " + ex.Message, appName);
        }
        return flag;
    }

    #endregion

    #region SQL 數據處理
    /// <summary>
    /// 抓取異常單主檔資料 By CASEID
    /// </summary>
    /// <param name="CASEID"></param>
    /// <returns></returns>
    public DataTable GetMaster_Exception(int CASEID)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select * from TB_GDS_ExceptionHandling where CASEID = @CASEID");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@CASEID", SqlDbType.Int, CASEID));
        return sdb.GetDataTable(sb.ToString(), opc);
    }

    /// <summary>
    /// 抓取異常單主檔資料 By Link 單號
    /// </summary>
    /// <param name="CASEID"></param>
    /// <returns></returns>
    public DataTable GetMaster_Exception(string MBLNR_A)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select * from TB_GDS_ExceptionHandling where MBLNR_A = @MBLNR_A order by id desc");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@MBLNR_A", SqlDbType.NVarChar, MBLNR_A));
        return sdb.GetDataTable(sb.ToString(), opc);
    }


    /// <summary>
    /// 抓取正常單主檔資料
    /// </summary>
    /// <param name="CASEID"></param>
    /// <returns></returns>
    public DataTable GetGDSDATA(string DocNo)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select * from TB_GDS_DATA where MBLNR_A = @MBLNR_A");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@MBLNR_A", SqlDbType.NVarChar, DocNo));
        return sdb.GetDataTable(sb.ToString(), opc);
    }


    /// <summary>
    /// Begin關卡 Insert基本資料數據
    /// </summary>
    public void Insert_Begin(string WERKS, string MBLNR, string MBLNR_A, string KOSTL, string ABTEI, string Applicant, string ZEILE,
        string MATNR, string MENGE, string RTNIF, string Reason, string Remark, string IADocNo, string I6DocNo, string STATUS, double Amount, string Settingxml, int CASEID)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"INSERT INTO [SPM].[dbo].[TB_GDS_ExceptionHandling]
                   ([WERKS]
                   ,[MBLNR]
                   ,[MBLNR_A]
                   ,[KOSTL]
                   ,[ABTEI]
                   ,[Applicant]
                   ,[ZEILE]
                   ,[MATNR]
                   ,[MENGE]
                   ,[RTNIF]
                   ,[Reason]
                   ,[Remark]
                   ,[IADocNo]
                   ,[I6DocNo]
                   ,[STATUS]
                   ,[Amount]
                   ,[Settingxml]
                   ,[CASEID])
             VALUES
                   (@WERKS,
                    @MBLNR,
                    @MBLNR_A,
                    @KOSTL,
                    @ABTEI,
                    @Applicant,
                    @ZEILE,
                    @MATNR,
                    @MENGE,
                    @RTNIF,
                    @Reason,
                    @Remark,
                    @IADocNo,
                    @I6DocNo,
                    @STATUS,
                    @Amount,
                    @Settingxml,
                    @CASEID)");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@WERKS", SqlDbType.NVarChar, WERKS));
        opc.Add(DataPara.CreateDataParameter("@MBLNR", SqlDbType.NVarChar, MBLNR));
        opc.Add(DataPara.CreateDataParameter("@MBLNR_A", SqlDbType.NVarChar, MBLNR_A));
        opc.Add(DataPara.CreateDataParameter("@KOSTL", SqlDbType.NVarChar, KOSTL));
        opc.Add(DataPara.CreateDataParameter("@ABTEI", SqlDbType.NVarChar, ABTEI));
        opc.Add(DataPara.CreateDataParameter("@Applicant", SqlDbType.NVarChar, Applicant));
        opc.Add(DataPara.CreateDataParameter("@ZEILE", SqlDbType.NVarChar, ZEILE));
        opc.Add(DataPara.CreateDataParameter("@MATNR", SqlDbType.NVarChar, MATNR));
        opc.Add(DataPara.CreateDataParameter("@MENGE", SqlDbType.NVarChar, MENGE));
        opc.Add(DataPara.CreateDataParameter("@RTNIF", SqlDbType.NVarChar, RTNIF));
        opc.Add(DataPara.CreateDataParameter("@Reason", SqlDbType.NVarChar, Reason));
        opc.Add(DataPara.CreateDataParameter("@Remark", SqlDbType.NVarChar, Remark));
        opc.Add(DataPara.CreateDataParameter("@IADocNo", SqlDbType.NVarChar, IADocNo));
        opc.Add(DataPara.CreateDataParameter("@I6DocNo", SqlDbType.NVarChar, I6DocNo));
        opc.Add(DataPara.CreateDataParameter("@STATUS", SqlDbType.NVarChar, STATUS));
        opc.Add(DataPara.CreateDataParameter("@Amount", SqlDbType.Decimal, Amount));
        opc.Add(DataPara.CreateDataParameter("@Settingxml", SqlDbType.NVarChar, Settingxml));
        opc.Add(DataPara.CreateDataParameter("@CASEID", SqlDbType.Int, CASEID));
        sdb.ExecuteNonQuery(sb.ToString(), opc);

    }


    /// <summary>
    /// 每次更新DOA關卡的Settingxml
    /// </summary>
    /// <param name="DocNo"></param>
    public void UpdateSettingxml(string DocNo, string Settingxml)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"Update TB_GDS_ExceptionHandling set Settingxml = @Settingxml  where MBLNR = @MBLNR");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@MBLNR", SqlDbType.NVarChar, DocNo));
        opc.Add(DataPara.CreateDataParameter("@Settingxml", SqlDbType.NVarChar, Settingxml));
        sdb.ExecuteNonQuery(sb.ToString(), opc);
    }

    /// <summary>
    /// 更新单据状态
    /// </summary>
    public void UpdateFormStatus(int CASEID,string Status)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"Update TB_GDS_ExceptionHandling set STATUS = @STATUS where CASEID = @CASEID");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@CASEID", SqlDbType.Int, CASEID));
        opc.Add(DataPara.CreateDataParameter("@STATUS", SqlDbType.Int, Status));
        sdb.ExecuteNonQuery(sb.ToString(), opc);

    }

    /// <summary>
    /// 針對Reject和Approve的單據 更新Flag欄位和StatusRemark
    /// </summary>
    public void UpdateGDSQueue(int CASEID, string StatusMark,string APROV)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"Update TB_GDS_ExceptionHandling set Flag = 'N',StatusMark = @StatusMark,APROV = @APROV where CASEID = @CASEID");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@CASEID", SqlDbType.Int, CASEID));
        opc.Add(DataPara.CreateDataParameter("@StatusMark", SqlDbType.NVarChar, StatusMark));
        opc.Add(DataPara.CreateDataParameter("@APROV", SqlDbType.NVarChar, APROV));
        sdb.ExecuteNonQuery(sb.ToString(), opc);

    }

    /// <summary>
    /// Submit時,程序異常,刪除已新增的單號 By CASEID
    /// </summary>
    /// <param name="CASEID"></param>
    public void DeleteFormNo(int CASEID)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"delete from TB_GDS_ExceptionHandling  where CASEID = @CASEID");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@CASEID", SqlDbType.Int, CASEID));
        sdb.ExecuteNonQuery(sb.ToString(),opc);
    }
    #endregion

    #region 自動生成單號
    public string CreateFormNo(string MBLNR_A)
    {
        string FormNo = string.Empty;
        int NFormNo = 0;
        DataTable dt = GetMaster_Exception(MBLNR_A);
        if (dt.Rows.Count == 0)
        {
            return MBLNR_A + "01";
        }
        else
        {
            FormNo = dt.Rows[0]["MBLNR"].ToString();
            FormNo = FormNo.Remove(0, FormNo.Length - 2);
            NFormNo = Convert.ToInt32(FormNo);
            NFormNo = NFormNo + 1;
            return MBLNR_A + Convert.ToString(NFormNo).PadLeft(2, '0');
        }
    }

    #endregion


}
