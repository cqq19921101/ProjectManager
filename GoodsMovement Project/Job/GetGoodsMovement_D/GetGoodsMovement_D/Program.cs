using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Configuration;
using System.Collections;
using System.IO;
using Aspose.Cells;
using Aspose.Cells.Rendering;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using LiteOn.EA.DAL;
using LiteOn.EA.Borg.Utility;
using LiteOn.GDS.Utility;
using LiteOn.ICM.SIC;

namespace GetGoodsMovement_D
{
    class GetGoodsMovement_D
    {
        static ArrayList opc = new ArrayList();
        static SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("SPM"));
        static string sql = string.Empty;
        static string appName = "GDS_GetSAPData_D";
        static void Main(string[] args)
        {
            try
            {
                Z_BAPI_GDS_SEND_D();
                //AbortFormNo(int.Parse("320755"));
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                DBIO.WriteLog("# Fail: " + ex.Message, appName);
            }
        }


        /// <summary>
        /// get sap data SAP刪除的單據
        /// </summary>
        static public void Z_BAPI_GDS_SEND_D()
        {
            Console.WriteLine("- Fetch : Z_BAPI_GDS_SEND_D");
            StringBuilder sbError = new StringBuilder();
            Hashtable InputParas = new Hashtable();
            Client.ClientParas _clientparas = new Client.ClientParas();
            _clientparas.AppID = "CZ_Workflow";
            _clientparas.SAPFunction = "MM";
            _clientparas.BAPI = "Z_BAPI_GDS_SEND_D";

            _clientparas.InputParas = InputParas;
            _clientparas.InputTable = new DataTable[1];
            _clientparas.InputTable[0] = BuildInputTable_GetGDS();
            _clientparas.OutputParas = new Hashtable();

            try
            {
                bool bReturn = LiteOn.ICM.SIC.Client.getSAPData(ref _clientparas);
                if (bReturn)
                {
                    //get sap return data
                    DataTable dtD = new DataTable();
                    int iCount = _clientparas.ResultTable.Length;
                    for (int i = 0; i < iCount; i++)
                    {
                        Console.WriteLine(_clientparas.ResultTable[i].TableName + " rows = " + _clientparas.ResultTable[i].Rows.Count);
                        if (_clientparas.ResultTable[i].TableName == "T_GDS_D") dtD = _clientparas.ResultTable[i];
                    }

                    if (dtD.Rows.Count == 0) return;
                    else
                    {
                        foreach (DataRow dr in dtD.Rows)
                        {
                            string WERKS = dr["WERKS"].ToString();
                            string MBLNR_A = dr["MBLNR_A"].ToString();
                            DataTable dt = GetMaster(WERKS,MBLNR_A);
                            if (dt.Rows.Count > 0)
                            {
                                AbortFormNo(int.Parse(dt.Rows[0]["CASEID"].ToString()));
                            }
                        }
                    }

                }
                else
                {
                    Console.WriteLine("# Fail: BAPI RETURN FAIL " + _clientparas.sErrMsg);
                    DBIO.WriteLog("# Fail: BAPI RETURN FAIL" + _clientparas.sErrMsg, appName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                DBIO.WriteLog("# Fail: SIC ERROR" + ex.Message, appName);
            }
        }


        /// <summary>
        /// Abort单号 By CaseID
        /// </summary>
        /// <param name="CASEID"></param>
        static public void AbortFormNo(int CASEID)
        {
            AbortCase.AbortTask(CASEID);

        }


        /// <summary>
        /// 準備GetGoodsMovemt INPUT TABLE（PLANT LIST）
        /// </summary>
        /// <returns></returns>
        static public DataTable BuildInputTable_GetGDS()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT PARAME_VALUE1 AS WERKS  FROM TB_APPLICATION_PARAM ");
            sql.Append("WHERE APPLICATION_NAME = 'GoodsMovement' ");
            sql.Append("AND FUNCTION_NAME = 'GetFormData' AND PARAME_NAME = 'InputParameter' AND PARAME_ITEM = 'PlantCode' ");
            opc.Clear();
            DataTable dt = sdb.GetDataTable(sql.ToString(), opc);
            dt.TableName = "P_PLANT";
            return dt;
        }


        /// <summary>
        /// 抓取对应的资料
        /// </summary>
        /// <param name="WERKS"></param>
        /// <param name="MBLNR_A"></param>
        /// <returns></returns>
        static public DataTable GetMaster(string WERKS,string MBLNR_A)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"select * from TB_GDS_DATA where WERKS = @WERKS AND  MBLNR_A = @MBLNR_A");
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@WERKS", SqlDbType.NVarChar, WERKS));
            opc.Add(DataPara.CreateDataParameter("@MBLNR_A", SqlDbType.NVarChar, MBLNR_A));
            return sdb.GetDataTable(sb.ToString(),opc);
        }


    }
}
