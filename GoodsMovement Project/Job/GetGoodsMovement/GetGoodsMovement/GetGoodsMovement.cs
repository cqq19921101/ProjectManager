using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using LiteOn.ICM.SIC;
using System.Collections;
using System.Data;
using LiteOn.EA.BLL;
using LiteOn.EA.Model;
using LiteOn.EA.DAL;
using LiteOn.GDS.Utility;
using System.Configuration;
namespace GetGoodsMovement
{
    class GetGoodsMovement
    {
        static ArrayList opc = new ArrayList();
        static SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("SPM"));
        static string sql = string.Empty;
        static string appName = "GDS_GetSAPData";
        static void Main(string[] args)
        {
            try
            {
                //Z_BAPI_GDS_SEND();//正式的方法
                Z_BAPI_GDS_SEND_APROV("I119070003", "2680"); //测试根据单号和厂别直接抓数据
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                DBIO.WriteLog("# Fail: " + ex.Message,appName);
            }
        }


        /// <summary>
        /// get sap data 
        /// </summary>
        static public  void Z_BAPI_GDS_SEND()
        {
            Console.WriteLine("- Fetch : Z_BAPI_GDS_SEND");
            StringBuilder sbError = new StringBuilder();
            Hashtable InputParas = new Hashtable();
            Client.ClientParas _clientparas = new Client.ClientParas();
            _clientparas.AppID = "CZ_Workflow";
            _clientparas.SAPFunction = "MM";
            _clientparas.BAPI = "Z_BAPI_GDS_SEND";

            _clientparas.InputParas = InputParas;
            _clientparas.InputTable = new DataTable[2];
            _clientparas.InputTable[0] = new DataTable("Temp");
            _clientparas.InputTable[1] = BuildInputTable_GetGDS();
            _clientparas.OutputParas = new Hashtable();

            try
            {
                bool bReturn = LiteOn.ICM.SIC.Client.getSAPData(ref _clientparas);
                #region[operation]
                if (bReturn)
                {
                    //get sap return data
                    DataTable dtDetail = new DataTable();
                    DataTable dtHead = new DataTable();
                    int iCount = _clientparas.ResultTable.Length;
                    for (int i = 0; i < iCount; i++)
                    {
                        //WriteLog("- Table=" + _clientparas.ResultTable[i].TableName + "- rows = " + _clientparas.ResultTable[i].Rows.Count);
                        Console.WriteLine(_clientparas.ResultTable[i].TableName + " rows = " + _clientparas.ResultTable[i].Rows.Count);
                        if (_clientparas.ResultTable[i].TableName == "T_GDS_DETAIL") dtDetail = _clientparas.ResultTable[i];
                        if (_clientparas.ResultTable[i].TableName == "T_GDS_HEAD") dtHead = _clientparas.ResultTable[i];
                    }

                  if (dtHead.Rows.Count == 0) return;
                    if (dtDetail.Rows.Count == 0) return;
                    //dtHead.WriteXml(@"c:\dtHead.xml");
                    //dtDetail.WriteXml(@"c:\dtDetail.xml");
                    //test
                    //save xml
                    //StringWriter sw = new StringWriter();
                    //dtDetail.WriteXml(@"c:\aa.xml");
                    //dtHead.WriteXml(@"c:\bb.xml");
                    //read xml
                    //DataSet ds = GetDataSetByXmlpath();
                    //dtDetail = ds.Tables[0];

                    // RECORD DATA TO SPM DATABASE
                    
                    DBIO.RecordSAPData(dtHead, dtDetail);//Insert into TB_GDS_DATA


                    //TB_GDS_Mapping表中 CZ:IA.I6.I1(存I1单号和Batch Number 之后作为条件拉出关联的退料单)
                    DBIO.InsertMappingData(dtDetail);

                }
                else
                {
                    Console.WriteLine("# Fail: BAPI RETURN FAIL " + _clientparas.sErrMsg);
                    DBIO.WriteLog("# Fail: BAPI RETURN FAIL" + _clientparas.sErrMsg, appName);
                }
                #endregion[operation]
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                DBIO.WriteLog("# Fail: SIC ERROR" + ex.Message,appName);
            }
        }

        static public string Z_BAPI_GDS_SEND_APROV(string DocNo, string WERKS)
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
                        return "B   API RETURN FAIL " + _clientparas.sErrMsg;
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
        /// 準備GetGoodsMovemt INPUT TABLE（PLANT LIST）
        /// </summary>
        /// <returns></returns>
        static public DataTable BuildInputTable_GetGDS()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT '' AS MANDT , PARAME_VALUE1 AS WERKS  FROM TB_APPLICATION_PARAM ");
            sql.Append("WHERE APPLICATION_NAME = 'GoodsMovement' ");
            sql.Append("AND FUNCTION_NAME = 'GetFormData' AND PARAME_NAME = 'InputParameter' AND PARAME_ITEM = 'PlantCode' ");
            opc.Clear();
            DataTable dt = sdb.GetDataTable(sql.ToString(), opc);
            dt.TableName = "P_PLANT";
            return dt;
        }

        #region 創建臨時表作為Call BAPI的條件

        /// <summary>
        /// 創建回傳SAP時所需要的臨時的Datatable Z_BAPI_GET_SPECIAL_DOC TableName:T_ZTCPCN6D_W
        /// </summary>
        /// <returns></returns>
        static public DataTable BuildUpdateTable()
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
        /// 創建回傳SAP時所需要的臨時的Datatable   Z_BAPI_GDS_SEND  TableName:P_DOC
        /// </summary>
        /// <returns></returns>
        static public DataTable BuildPDOCTable()
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
        static public DataTable BuildPPLANTTable()
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




    }
}
