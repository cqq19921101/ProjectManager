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
using System.Web.UI.MobileControls;
using System.Collections.Generic;

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
    public DataTable GetdtHead(string FormNo,string  WERKS)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select xmlHead from TB_GDS_DATA where MBLNR_A = @MBLNR_A and WERKS = @WERKS");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@MBLNR_A", SqlDbType.NVarChar, FormNo));
        opc.Add(DataPara.CreateDataParameter("@WERKS", SqlDbType.NVarChar, WERKS));
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
    public DataTable GetdtDetail(string FormNo,string  WERKS,string Flag)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select xmlDetail from TB_GDS_DATA where MBLNR_A = @MBLNR_A  and WERKS = @WERKS");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@MBLNR_A", SqlDbType.NVarChar, FormNo));
        opc.Add(DataPara.CreateDataParameter("@WERKS", SqlDbType.NVarChar, WERKS));
        DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            return XMLToDataTable(dr, "XMLDetail");
        }

        return dt;
    }

    /// <summary>
    /// 根據單號抓取Detail的資料 將XML轉換成Datatable
    /// </summary>
    /// <param name="FormNo"></param>
    /// <returns></returns>
    public DataTable GetdtDetail(string FormNo,string WERKS)
    {
        return Z_BAPI_GDS_SEND_dtDetail(FormNo, WERKS);
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
                TEMP["WERKS"] = dr["WERKS"].ToString();//廠別
                TEMP["MBLNR"] = dr["MBLNR"].ToString();//主單號
                TEMP["ZEILE"] = dr["ZEILE"].ToString();//對應的ITEM號
                TEMP["MBLNR_A"] = dr["MBLNR_A"].ToString();//Link 單號
                TEMP["MATNR"] = dr["MATNR"].ToString();//料號
                TEMP["MENGE"] = dr["MENGE"].ToString();//退回數量
                TEMP["CHARG"] = dr["CHARG"].ToString();//Batch
                TEMP["REASON"] = dr["Reason"].ToString();//REASON
                TEMP["STATUS"] = "W";//狀態 Submit的時候 默認 'W' 回傳給SAP

                //UAT
                //TEMP["WERKS"] = dr["WERKS"].ToString();//廠別
                //TEMP["MBLNR"] = dr["MBLNR"].ToString();//主單號
                //TEMP["ZEILE"] = "001";
                //TEMP["MBLNR_A"] = dr["MBLNR_A"].ToString();//Link 單號
                //TEMP["MATNR"] = "8A034Z";
                //TEMP["MENGE"] = double.Parse("1");
                //TEMP["MENGE"] = double.Parse("1");
                //TEMP["STATUS"] = "W";



                dtUpdate.Rows.Add(TEMP);//創建出新的Datatable
            }
            Z_BAPI_GSD_UPDATE(dtUpdate);//將臨時的Datatable作為Call BAPI的參數
        }

    }

    /// <summary>
    /// Call SAP BAPI -- Z_BAPI_GDS_SEND 檢查單據是否過賬  MBLNR欄位不為空 表示已經過賬 返回True 否則返回False
    /// </summary>
    /// <returns></returns>
    public bool CheckFormNoIsPass(string I1DocNo,string WERKS)
    {
        string Errmsg = string.Empty;
        Errmsg += Z_BAPI_GDS_SEND_MBLNR(I1DocNo, WERKS);
        if (Errmsg.Length > 0)
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// 檢查退料單的APROV欄位 不為A則過濾掉
    /// </summary>
    /// <param name="ReturnNo"></param>
    /// <param name="WERKS"></param>
    /// <returns></returns>
    public bool CheckReturnFormNo(string ReturnNo, string WERKS)
    {
        string Errmsg = string.Empty;
        Errmsg += Z_BAPI_GDS_SEND_APROV(ReturnNo, WERKS);
        if (Errmsg.Length > 0)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// call bapi 回傳單據狀態給SAP  Z_BAPI_GET_SPECIAL_DOC 將異常處理單的狀態回傳給SAP
    /// </summary>
    /// <param name="upDT"></param>
    /// <returns></returns>
    public bool Z_BAPI_GSD_UPDATE(DataTable upDT)
    {
        bool flag = false;

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

    /// <summary>
    /// Call BAPI 傳入P_DOC表 Check單據是否過帳  MBLNR欄位不為空 返回的Datable -- T_GDS_HEAD
    /// </summary>
    /// <param name="dtPDoc"></param>
    /// <param name="dtPlant"></param>
    public string Z_BAPI_GDS_SEND_MBLNR(string DocNo, string WERKS)
    {
        if (DocNo.Length > 0)
        {
            string Type = DocNo.Substring(0, 2);//得到傳入單號的類型
            int Year = int.Parse("20" + DocNo.Substring(2, 2));//根據單號的第3,4碼 得到年份
            StringBuilder sbError = new StringBuilder();
            Hashtable InputParas = new Hashtable();
            Client.ClientParas _clientparas = new Client.ClientParas();

            #region Get P_DOC
            DataTable dtPDoc = BuildPDOCTable();//創建一個臨時Table 作為參數 Call BAPI
            DataRow TEMP = dtPDoc.NewRow();
            TEMP["WERKKS"] = WERKS;//廠別
            TEMP["MBLNR_A"] = DocNo;//傳入單號
            TEMP["MJAHR_A"] = Year;//年份
            dtPDoc.Rows.Add(TEMP);//創建出新的Datatable
            #endregion

            _clientparas.AppID = "CZ_Workflow";
            _clientparas.SAPFunction = "MM";
            _clientparas.BAPI = "Z_BAPI_GDS_SEND";

            _clientparas.InputParas = InputParas;
            _clientparas.InputTable = new DataTable[1];
            _clientparas.InputTable[0] = dtPDoc;
            _clientparas.OutputParas = new Hashtable();

            try
            {
                bool bReturn = LiteOn.ICM.SIC.Client.getSAPData(ref _clientparas);
                if (bReturn)
                {
                    //get sap return data
                    DataTable dtHead = new DataTable();
                    int iCount = _clientparas.ResultTable.Length;
                    for (int i = 0; i < iCount; i++)
                    {
                        Console.WriteLine(_clientparas.ResultTable[i].TableName + " rows = " + _clientparas.ResultTable[i].Rows.Count);
                        //if (_clientparas.ResultTable[i].TableName == "T_GDS_DETAIL") dtDetail = _clientparas.ResultTable[i];
                        if (_clientparas.ResultTable[i].TableName == "T_GDS_HEAD") dtHead = _clientparas.ResultTable[i];
                    }

                    if (dtHead.Rows.Count == 0) return "Empty";
                    DataRow drHead = dtHead.Rows[0];

                    if (drHead["MBLNR"].ToString().Length == 0)
                    {
                        return Type + "單未過帳";
                    }
                }
                else
                {
                    return "BAPI RETURN FAIL " + _clientparas.sErrMsg;
                }
            }
            catch (Exception ex)
            {
                return "SIC ERROR " + ex.Message;
            }
            return "";
        }
        else
        {
            return "";
        }
    }

    /// <summary>
    /// Call BAPI 傳入P_DOC表 檢查退料單的狀態 過濾不是A的單據  返回的Datable -- T_GDS_HEAD 
    /// </summary>
    /// <param name="dtPDoc"></param>
    /// <param name="dtPlant"></param>
    public string Z_BAPI_GDS_SEND_APROV(string DocNo, string WERKS)
    {
        if (DocNo.Length > 0)
        {
            string Type = DocNo.Substring(0, 2);//得到傳入單號的類型
            int Year = int.Parse("20" + DocNo.Substring(2, 2));//根據單號的第3,4碼 得到年份
            StringBuilder sbError = new StringBuilder();
            Hashtable InputParas = new Hashtable();
            Client.ClientParas _clientparas = new Client.ClientParas();

            #region Get P_DOC
            DataTable dtPDoc = BuildPDOCTable();//創建一個臨時Table 作為參數 Call BAPI
            DataRow TEMP = dtPDoc.NewRow();
            TEMP["WERKKS"] = WERKS;//廠別
            TEMP["MBLNR_A"] = DocNo;//傳入單號
            TEMP["MJAHR_A"] = Year;//年份
            dtPDoc.Rows.Add(TEMP);//創建出新的Datatable
            #endregion

            _clientparas.AppID = "CZ_Workflow";
            _clientparas.SAPFunction = "MM";
            _clientparas.BAPI = "Z_BAPI_GDS_SEND";

            _clientparas.InputParas = InputParas;
            _clientparas.InputTable = new DataTable[1];
            _clientparas.InputTable[0] = dtPDoc;
            _clientparas.OutputParas = new Hashtable();

            try
            {
                bool bReturn = LiteOn.ICM.SIC.Client.getSAPData(ref _clientparas);
                if (bReturn)
                {
                    //get sap return data
                    DataTable dtHead = new DataTable();
                    int iCount = _clientparas.ResultTable.Length;
                    for (int i = 0; i < iCount; i++)
                    {
                        Console.WriteLine(_clientparas.ResultTable[i].TableName + " rows = " + _clientparas.ResultTable[i].Rows.Count);
                        //if (_clientparas.ResultTable[i].TableName == "T_GDS_DETAIL") dtDetail = _clientparas.ResultTable[i];
                        if (_clientparas.ResultTable[i].TableName == "T_GDS_HEAD") dtHead = _clientparas.ResultTable[i];
                    }

                    if (dtHead.Rows.Count == 0) return "Empty";
                    DataRow drHead = dtHead.Rows[0];

                    if (drHead["APROV"].ToString() != "A")
                    {
                        return DocNo + " 單據未Approve!";
                    }
                }
                else
                {
                    return "BAPI RETURN FAIL " + _clientparas.sErrMsg;
                }
            }
            catch (Exception ex)
            {
                return "SIC ERROR " + ex.Message;
            }
            return "";
        }
        else
        {
            return "";
        }
    }

    /// <summary>
    /// Call BAPI 傳入P_DOC表 抓取已經過帳的領料單中 ZEILE_A and POSTED  返回的Datable -- T_GDS_Detail
    /// </summary>
    /// <param name="DocNo">Link 單號 -- I1</param>
    /// <param name="WERKS">廠別</param>
    /// <returns></returns>
    public DataTable Z_BAPI_GDS_SEND_dtDetail(string DocNo, string WERKS)
    {
        DataTable dtDetail = new DataTable();

        if (DocNo.Length > 0)
        {
            string Type = DocNo.Substring(0, 2);//得到傳入單號的類型
            int Year = int.Parse("20" + DocNo.Substring(2, 2));//根據單號的第3,4碼 得到年份
            StringBuilder sbError = new StringBuilder();
            Hashtable InputParas = new Hashtable();
            Client.ClientParas _clientparas = new Client.ClientParas();

            #region Get P_DOC
            DataTable dtPDoc = BuildPDOCTable();//創建一個臨時Table 作為參數 Call BAPI
            DataRow TEMP = dtPDoc.NewRow();
            TEMP["WERKKS"] = WERKS;//廠別
            TEMP["MBLNR_A"] = DocNo;//傳入單號
            TEMP["MJAHR_A"] = Year;//年份
            dtPDoc.Rows.Add(TEMP);//創建出新的Datatable
            #endregion

            _clientparas.AppID = "CZ_Workflow";
            _clientparas.SAPFunction = "MM";
            _clientparas.BAPI = "Z_BAPI_GDS_SEND";

            _clientparas.InputParas = InputParas;
            _clientparas.InputTable = new DataTable[1];
            _clientparas.InputTable[0] = dtPDoc;
            _clientparas.OutputParas = new Hashtable();

            try
            {
                bool bReturn = LiteOn.ICM.SIC.Client.getSAPData(ref _clientparas);
                if (bReturn)
                {
                    //get sap return data
                    int iCount = _clientparas.ResultTable.Length;
                    for (int i = 0; i < iCount; i++)
                    {
                        Console.WriteLine(_clientparas.ResultTable[i].TableName + " rows = " + _clientparas.ResultTable[i].Rows.Count);
                        if (_clientparas.ResultTable[i].TableName == "T_GDS_DETAIL") dtDetail = _clientparas.ResultTable[i];
                        //if (_clientparas.ResultTable[i].TableName == "T_GDS_HEAD") dtHead = _clientparas.ResultTable[i];
                    }

                    if (dtDetail.Rows.Count == 0) return dtDetail;

                }
                else
                {
                }
            }
            catch (Exception ex)
            {

            }
        }
        else
        {

        }
        return dtDetail;

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
        sb.Append(@"select * from TB_GDS_ExceptionHandling where MBLNR_A = @MBLNR_A order by id ");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@MBLNR_A", SqlDbType.NVarChar, MBLNR_A));
        return sdb.GetDataTable(sb.ToString(), opc);
    }

    /// <summary>
    /// 抓取異常單主檔資料 By Link 單號 and Material AND 單據狀態時Approve或者在簽的
    /// </summary>
    /// <param name="CASEID"></param>
    /// <returns></returns>
    public DataTable GetMaster_Exception(string MBLNR_A, string MATNR, string CHARG)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select * from TB_GDS_ExceptionHandling where MBLNR_A = @MBLNR_A and MATNR = @MATNR and Status in ('In Process','Approve') and CHARG = @CHARG order by id ");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@MBLNR_A", SqlDbType.NVarChar, MBLNR_A));
        opc.Add(DataPara.CreateDataParameter("@MATNR", SqlDbType.NVarChar, MATNR));
        opc.Add(DataPara.CreateDataParameter("@CHARG", SqlDbType.NVarChar, CHARG));
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
    public void Insert_Begin(string WERKS, string MBLNR, string MBLNR_A, string KOSTL, string ABTEI, string Applicant, string ZEILE, string MATNR,
        string MENGE, string RTNIF, string Reason, string Remark,string STATUS, double Amount,string CHARG, string Settingxml, int CASEID)
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
                   ,[STATUS]
                   ,[Amount]
                   ,[CHARG]
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
                    @STATUS,
                    @Amount,
                    @CHARG,
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
        opc.Add(DataPara.CreateDataParameter("@MENGE", SqlDbType.Int, int.Parse(MENGE)));
        opc.Add(DataPara.CreateDataParameter("@RTNIF", SqlDbType.NVarChar, RTNIF));
        opc.Add(DataPara.CreateDataParameter("@Reason", SqlDbType.NVarChar, Reason));
        opc.Add(DataPara.CreateDataParameter("@Remark", SqlDbType.NVarChar, Remark));
        opc.Add(DataPara.CreateDataParameter("@STATUS", SqlDbType.NVarChar, STATUS));
        opc.Add(DataPara.CreateDataParameter("@Amount", SqlDbType.Decimal, Amount));
        opc.Add(DataPara.CreateDataParameter("@Settingxml", SqlDbType.NVarChar, Settingxml));
        opc.Add(DataPara.CreateDataParameter("@CHARG", SqlDbType.NVarChar, CHARG));
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
    public void UpdateFormStatus(int CASEID, string Status)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"Update TB_GDS_ExceptionHandling set STATUS = @STATUS where CASEID = @CASEID");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@CASEID", SqlDbType.Int, CASEID));
        opc.Add(DataPara.CreateDataParameter("@STATUS", SqlDbType.NVarChar, Status));
        sdb.ExecuteNonQuery(sb.ToString(), opc);

    }

    /// <summary>
    /// 針對Reject和Approve的單據 更新Flag欄位和StatusRemark
    /// </summary>
    public void UpdateGDSQueue(int CASEID, string StatusMark, string APROV)
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
        sdb.ExecuteNonQuery(sb.ToString(), opc);
    }

    /// <summary>
    /// 抓取Mapping表中相關的料號和對應的Item
    /// </summary>
    /// <param name="RDocNo"></param>
    public DataTable GetMaterial(string RDocNo)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select distinct MATNR from TB_GDS_Mapping  where REFNO = @REFNO");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@REFNO", SqlDbType.NVarChar, RDocNo));
        return sdb.GetDataTable(sb.ToString(), opc);
    }

    /// <summary>
    /// 根據領料單號的dtDetail抓取 Batch號和對應的Item  Batch號有可能為空
    /// </summary>
    /// <param name="RDocNo"></param>
    //public DataTable GetBatchNumber(DataTable dtDetail)
    //{
    //    foreach (DataRow drDetail in dtDetail.Rows)
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        sb.Append(@"select distinct MATNR from TB_GDS_Mapping  where REFNO = @REFNO");
    //        opc.Clear();
    //        opc.Add(DataPara.CreateDataParameter("@REFNO", SqlDbType.NVarChar, RDocNo));
    //        return sdb.GetDataTable(sb.ToString(), opc);
    //    }
    //}


    /// <summary>
    /// 根據領料單單號,料號,退料單類型 抓取Mapping的資料
    /// </summary>
    /// <param name="RDocNo"></param>
    /// <param name="Material"></param>
    /// <param name="Type"></param>
    /// <returns></returns>
    public DataTable GetMappingFormNo(string RDocNo, string Material, string Type)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select  *  from TB_GDS_Mapping  where REFNO = @REFNO and MATNR = @MATNR and Substring(MBLNR_A,1,2) = @Type");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@REFNO", SqlDbType.NVarChar, RDocNo));
        opc.Add(DataPara.CreateDataParameter("@MATNR", SqlDbType.NVarChar, Material));
        opc.Add(DataPara.CreateDataParameter("@Type", SqlDbType.NVarChar, Type));
        return sdb.GetDataTable(sb.ToString(), opc);
    }

    /// <summary>
    /// 根據領料單單號,料號,退料單類型 抓取Mapping的資料
    /// </summary>
    /// <param name="RDocNo"></param>
    /// <param name="Material"></param>
    /// <param name="Type"></param>
    /// <returns></returns>
    //public DataTable GetMappingFormNo(string RDocNo, string Material, string Type)
    //{
    //    StringBuilder sb = new StringBuilder();
    //    sb.Append(@"select  *  from TB_GDS_Mapping  where REFNO = @REFNO and MATNR = @MATNR and Substring(MBLNR_A,1,2) = @Type");
    //    opc.Clear();
    //    opc.Add(DataPara.CreateDataParameter("@REFNO", SqlDbType.NVarChar, RDocNo));
    //    opc.Add(DataPara.CreateDataParameter("@MATNR", SqlDbType.NVarChar, Material));
    //    opc.Add(DataPara.CreateDataParameter("@Type", SqlDbType.NVarChar, Type));
    //    return sdb.GetDataTable(sb.ToString(), opc);
    //}

    /// <summary>
    /// 抓取Mapping的資料 By 領料單單號,料號,簽核狀態
    /// </summary>
    /// <param name="RDocNo"></param>
    /// <param name="Material"></param>
    /// <param name="APROV"></param>
    /// <returns></returns>
    public DataTable GetMappingList(string RDocNo, string Material, string APROV,string Batch)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select  *  from TB_GDS_Mapping  where REFNO = @REFNO and MATNR = @MATNR and APROV = @APROV  ");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@REFNO", SqlDbType.NVarChar, RDocNo));
        opc.Add(DataPara.CreateDataParameter("@MATNR", SqlDbType.NVarChar, Material));
        opc.Add(DataPara.CreateDataParameter("@APROV", SqlDbType.NVarChar, "A"));
        if (Batch.Length > 0)
        {
            sb.Append(" and CHARG = @CHARG");
            opc.Add(DataPara.CreateDataParameter("@CHARG", SqlDbType.NVarChar, Batch));
        }
        sb.Append(" order  by ZEILE_A");
        return sdb.GetDataTable(sb.ToString(), opc);
    }



    /// <summary>
    /// 單據確認Submit后 item和料號作為條件 更新主單號,caseid,單據狀態更新為在簽(W)
    /// </summary>
    /// <param name="ZEILE"></param>
    /// <param name="MATNR"></param>
    /// <param name="MBLNR"></param>
    /// <param name="CASEID"></param>
    /// <param name="STATUS"></param>
    public void UpdateException_Queue(string ZEILE, string MATNR, string MBLNR, string CASEID, string STATUS)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"Update TB_GDS_ExceptionHandling set MBLNR = @MBLNR,CASEID = @CASEID,STATUS = @STATUS where ZEILE = @ZEILE,MATNR = @MATNR");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@ZEILE", SqlDbType.NVarChar, ZEILE));
        opc.Add(DataPara.CreateDataParameter("@MATNR", SqlDbType.NVarChar, MATNR));
        opc.Add(DataPara.CreateDataParameter("@MBLNR", SqlDbType.NVarChar, MBLNR));
        opc.Add(DataPara.CreateDataParameter("@CASEID", SqlDbType.NVarChar, CASEID));
        opc.Add(DataPara.CreateDataParameter("@STATUS", SqlDbType.NVarChar, STATUS));
        sdb.ExecuteNonQuery(sb.ToString(), opc);
    }

    /// <summary>
    /// Check此領料單號是否為空 
    /// </summary>
    /// <param name="dtDetail"></param>
    /// <returns></returns>
    public bool CheckBatchIsEmpty(DataTable dtDetail)
    {
        string Length = string.Empty;
        foreach (DataRow dr in dtDetail.Rows)
        {
            string CHARG = dr["CHARG"].ToString();
            Length += CHARG;
        }
        if (Length.Length == 0)
        {
            return false;
        }
        return true;
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

    #region 創建臨時表作為Call BAPI的條件

    /// <summary>
    /// 創建回傳SAP時所需要的臨時的Datatable Z_BAPI_GET_SPECIAL_DOC TableName:T_ZTCPCN6D_W
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
        dt.Columns.Add(new DataColumn("CHARG", typeof(String)));
        dt.Columns.Add(new DataColumn("REASON", typeof(String)));
        dt.Columns.Add(new DataColumn("STATUS", typeof(String)));
        return dt;
    }

    /// <summary>
    /// 創建回傳SAP時所需要的臨時的Datatable   Z_BAPI_GDS_SEND  TableName:P_DOC
    /// </summary>
    /// <returns></returns>
    public DataTable BuildPDOCTable()
    {
        DataTable dt = new DataTable("P_DOC");
        dt.Columns.Add(new DataColumn("WERKKS", typeof(String)));//2680
        dt.Columns.Add(new DataColumn("MBLNR_A", typeof(String)));//領料單原始單號(Link 單號)
        dt.Columns.Add(new DataColumn("MJAHR_A", typeof(int)));//年份
        return dt;
    }

    /// <summary>
    /// Select DB    Z_BAPI_GDS_SEND TableName:P_DOC 選擇所需要的廠別
    /// </summary>
    /// <returns></returns>
    public DataTable BuildPPLANTTable()
    {
        StringBuilder sql = new StringBuilder();
        sql.Append("SELECT PARAME_VALUE1 AS WERKS  FROM TB_APPLICATION_PARAM ");
        sql.Append("WHERE APPLICATION_NAME = 'GoodsMovement' ");
        sql.Append("AND FUNCTION_NAME = 'GetExceptionHandle' AND PARAME_NAME = 'InputParameter' AND PARAME_ITEM = 'PlantCode' ");
        opc.Clear();
        DataTable dt = sdb.GetDataTable(sql.ToString(), opc);
        dt.TableName = "P_PLANT";
        return dt;
    }

    #endregion

    #region 工具方法
    /// <summary>
    /// 把DataTable的某一列转化为逗号分隔字符串
    /// </summary>
    /// <param name="dataTable"></param>
    /// <param name="strColumn"></param>
    /// <returns></returns>
    //public string DataTableColumnSplit(DataTable dataTable, string strColumn)
    //{
    //    int[] idInts = dataTable.AsEnumerable().Select(d => d.Field<int>(strColumn)).ToArray();
    //    return String.Join(",", idInts);
    //}
    #endregion
}
