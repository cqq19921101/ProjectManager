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
namespace PostBackToSAP_EH
{
    class Program
    {
        static string sql = string.Empty;
        static ArrayList opc = new ArrayList();
        static SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("SPM"));
        static string appName = "GDS_Postback_EH";
        static void Main(string[] args)
        {
            UpdateSAP();
        }


        static public void UpdateSAP()
        {
            sql = "SELECT * FROM  TB_GDS_ExceptionHandling WHERE FLAG = 'N' and Status  order by id";
            DataTable dt = new DataTable();
            dt = sdb.GetDataTable(sql, opc);
            if (dt.Rows.Count > 0)
            {
                DataTable dtUpdate = BuildUpdateTable();
                foreach (DataRow dr in dt.Rows)
                {
                    DataRow TEMP = dtUpdate.NewRow();
                    TEMP["WERKS"] = dr["WERKS"].ToString();
                    TEMP["MBLNR"] = dr["MBLNR"].ToString();
                    TEMP["ZEILE"] = dr["ZEILE"].ToString();
                    TEMP["MBLNR_A"] = dr["MBLNR_A"].ToString();
                    TEMP["MATNR"] = dr["MATNR"].ToString();
                    TEMP["MENGE"] = double.Parse(dr["MENGE"].ToString());
                    TEMP["STATUS"] = dr["STATUS"].ToString().Substring(0,1).ToUpper();


                    //UAT Data
                    //TEMP["WERKS"] = "2680";
                    //TEMP["MBLNR"] = "I119070058-A";
                    //TEMP["ZEILE"] = "001";
                    //TEMP["MBLNR_A"] = "I119070058";
                    //TEMP["MATNR"] = "8A034Z";
                    //TEMP["MENGE"] = double.Parse("1");
                    //TEMP["STATUS"] = "W";


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

    }
}
