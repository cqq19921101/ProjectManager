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

namespace GetReturnPosted
{
    class GetReturnPosted
    {
        static ArrayList opc = new ArrayList();
        static SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("SPM"));
        static string sql = string.Empty;
        static string appName = "GDS_Return_Posted";
        static void Main(string[] args)
        {
            try
            {
                DataTable dt = GetPosted();
                foreach (DataRow dr in dt.Rows)
                {
                    string DOCNO = dr["MBLNR_A"].ToString();
                    string WERKS = dr["WERKS"].ToString();
                    Z_BAPI_GDS_SEND_Posted(DOCNO, WERKS);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                DBIO.WriteLog("# Fail: " + ex.Message, appName);
            }
        }


        /// <summary>
        /// Call BAPI Get ReturnFomNo Posted
        /// </summary>
        /// <param name="DocNo"></param>
        /// <param name="WERKS"></param>
        /// <returns></returns>
        static public void Z_BAPI_GDS_SEND_Posted(string DocNo, string WERKS)
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
                        DataTable dtDetail = new DataTable();
                        int iCount = _clientparas.ResultTable.Length;
                        for (int i = 0; i < iCount; i++)
                        {
                            Console.WriteLine(_clientparas.ResultTable[i].TableName + " rows = " + _clientparas.ResultTable[i].Rows.Count);
                            if (_clientparas.ResultTable[i].TableName == "T_GDS_DETAIL") dtDetail = _clientparas.ResultTable[i];
                            //if (_clientparas.ResultTable[i].TableName == "T_GDS_HEAD") dtHead = _clientparas.ResultTable[i];
                        }
                        if (dtDetail.Rows.Count == 0) return;
                        UpdatePosted(dtDetail);
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
        }

        /// <summary>
        /// 抓取符合条件的退料单的POSTED的值 签核状态是Approve
        /// </summary>
        /// <returns></returns>
        static public DataTable GetPosted()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"select * From TB_GDS_Mapping WHERE APROV  = @APROV and POSTED = @POSTED");
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@APROV", SqlDbType.NVarChar, "A"));
            opc.Add(DataPara.CreateDataParameter("@POSTED", SqlDbType.NVarChar, ""));
            return sdb.GetDataTable(sb.ToString(), opc);

        }

        /// <summary>
        /// 更新已经Approve的退料单的POSTED的值
        /// </summary>
        /// <param name="MBLNR_A"></param>
        /// <param name="WERKS"></param>
        /// <param name="POSTED"></param>
        static public void UpdatePosted(DataTable dtDetail)
        {
            foreach (DataRow drDetail in dtDetail.Rows)
            {
                string MBLNR_A = drDetail["MBLNR_A"].ToString().Trim();
                string WERKS = drDetail["WERKS"].ToString().Trim();
                string ZEILE_A = drDetail["ZEILE_A"].ToString().Trim();
                string POSTED = drDetail["POSTED"].ToString().Trim();
                StringBuilder sb = new StringBuilder();
                opc.Clear();
                sb.Append(@"Update TB_GDS_Mapping set POSTED = @POSTED where MBLNR_A = @MBLNR_A and WERKS = @WERKS and ZEILE_A = @ZEILE_A");
                opc.Clear();
                opc.Add(DataPara.CreateDataParameter("@MBLNR_A", SqlDbType.NVarChar, MBLNR_A));
                opc.Add(DataPara.CreateDataParameter("@WERKS", SqlDbType.NVarChar, WERKS));
                opc.Add(DataPara.CreateDataParameter("@POSTED", SqlDbType.NVarChar, POSTED));
                opc.Add(DataPara.CreateDataParameter("@ZEILE_A", SqlDbType.NVarChar, ZEILE_A));
                sdb.GetDataTable(sb.ToString(), opc);
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


        #endregion




    }
}
