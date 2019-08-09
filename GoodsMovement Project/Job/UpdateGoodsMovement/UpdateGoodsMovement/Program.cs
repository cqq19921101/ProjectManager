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
namespace UpdateGoodsMovement
{
    class Program
    {
        static string sql = string.Empty;
        static ArrayList opc = new ArrayList();
        static SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("SPM"));
        static string appName = "GDS_Postback";
        static void Main(string[] args)
        {
            UpdateSAP();
        }


        static public void UpdateSAP()
        {
            sql = "SELECT * FROM  TB_GDS_QUEUE WHERE FLAG = 'N' order by id";
            DataTable dt = new DataTable();
            dt = sdb.GetDataTable(sql, opc);
            if (dt.Rows.Count > 0)
            {
                DataTable dtUpdate = BuildUpdateTable();
                foreach (DataRow dr in dt.Rows)
                {
                    DataRow TEMP = dtUpdate.NewRow();
                    TEMP["WERKS"] = dr["WERKS"].ToString().Trim();
                    TEMP["MBLNR_A"] = dr["MBLNR_A"].ToString().Trim();
                    TEMP["MJAHR_A"] = double.Parse(dr["MJAHR_A"].ToString().Trim());
                    TEMP["APTYP"] = dr["APTYP"].ToString().Trim();
                    TEMP["CASEID"] = double.Parse(dr["CASEID"].ToString().Trim());
                    TEMP["APROV"] = dr["APROV"].ToString().Trim();
                    TEMP["APRD"] = dr["APRD"].ToString().Trim();
                    TEMP["APRT"] = dr["APRT"].ToString().Trim();
                    TEMP["REMARK"] = dr["REMARK"].ToString().Trim();

                    dtUpdate.Rows.Add(TEMP);
                }
                Z_BAPI_GSD_UPDATE(dtUpdate);
            }
        }

        /// <summary>
        /// call bapi 
        /// </summary>
        /// <param name="upDT"></param>
        /// <returns></returns>
        public static bool Z_BAPI_GSD_UPDATE(DataTable upDT)
        {
            bool flag = false;
            Console.WriteLine("- Fetch : Z_BAPI_GDS_UPDATE");

            StringBuilder sbError = new StringBuilder();
            Hashtable InputParas = new Hashtable();

            Client.ClientParas _clientparas = new Client.ClientParas();
            _clientparas.AppID = "CZ_Workflow";
            _clientparas.SAPFunction = "MM";
            _clientparas.BAPI = "Z_BAPI_GDS_UPDATE";

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
                        // WriteLog("- Table=" + _clientparas.ResultTable[i].TableName + "- rows = " + _clientparas.ResultTable[i].Rows.Count);

                        if (_clientparas.ResultTable[i].TableName == "T_GDS_UPD")
                        {
                            DataTable DT = _clientparas.ResultTable[i];
                            foreach (DataRow dr in DT.Rows)
                            {
                                opc.Clear();
                                opc.Add(DataPara.CreateDataParameter("@WERKS", SqlDbType.VarChar, dr["WERKS"].ToString().Trim()));
                                opc.Add(DataPara.CreateDataParameter("@MBLNR_A", SqlDbType.VarChar, dr["MBLNR_A"].ToString().Trim()));
                                opc.Add(DataPara.CreateDataParameter("@MJAHR_A", SqlDbType.VarChar, dr["MJAHR_A"].ToString().Trim()));
                                opc.Add(DataPara.CreateDataParameter("@APROV", SqlDbType.VarChar, dr["APROV"].ToString().Trim()));
                                //普通單更新消息隊列 TB_GDS_QUEUE
                                sql = "UPDATE TB_GDS_QUEUE SET Flag='Y',UPDATE_TIME=GETDATE() WHERE WERKS = @WERKS AND MBLNR_A = @MBLNR_A AND MJAHR_A = @MJAHR_A and APROV = @APROV AND FLAG='N' ";
                                try
                                {
                                    sdb.ExecuteNonQuery(sql, opc);
                                }
                                catch (Exception ex)
                                {
                                    DBIO.WriteLog("# Fail: UPDATE TB_GDS_QUEUE FLAG FAIL" + ex.Message,appName);
                                }
                            }
                        }
                    }
                    flag = true;
                }
                else
                {
                    DBIO.WriteLog("# Fail: " + _clientparas.sErrMsg, appName);
                    Console.WriteLine("# Fail: " + _clientparas.sErrMsg);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                DBIO.WriteLog("# Fail: " + ex.Message, appName);
            }
            return flag;
        }

        public static DataTable BuildUpdateTable()
        {
            DataTable dt = new DataTable("T_GDS_UPDATE");
            dt.Columns.Add(new DataColumn("WERKS", typeof(String)));
            dt.Columns.Add(new DataColumn("MBLNR_A", typeof(String)));
            dt.Columns.Add(new DataColumn("MJAHR_A", typeof(double)));
            dt.Columns.Add(new DataColumn("APTYP", typeof(String)));
            dt.Columns.Add(new DataColumn("CASEID", typeof(double)));
            dt.Columns.Add(new DataColumn("APROV", typeof(String)));
            dt.Columns.Add(new DataColumn("APRD", typeof(String)));
            dt.Columns.Add(new DataColumn("APRT", typeof(String)));
            dt.Columns.Add(new DataColumn("REMARK", typeof(String)));
            return dt;
        }

        /// <summary>
        /// 更新異常單消息隊列中的單據簽核狀態
        /// </summary>
        /// <param name="MBLNR_A"></param>
        public static void UpdateExceptionQueue(string MBLNR_A)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"Update TB_GDS_Exception_QUEUE set  Status = @Status  where MBLNR_A = @MBLNR_A ");
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@MBLNR_A", SqlDbType.NVarChar, MBLNR_A));
            sdb.ExecuteNonQuery(sb.ToString(),opc);
        }

    }
}
