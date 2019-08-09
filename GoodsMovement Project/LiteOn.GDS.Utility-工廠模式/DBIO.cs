using System;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using System.Text;
using LiteOn.EA.DAL;
using LiteOn.EA.Model;
namespace LiteOn.GDS.Utility
{
    public class DBIO
    {
        static string sql = string.Empty;
        static ArrayList opc = new ArrayList();
        static SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("SPM"));

        /// <summary>
        /// 記錄表單DATA至SPM DB
        /// </summary>
        /// <param name="dtHead">表頭DATATABLE</param>
        /// <param name="dtDetail">表身DATATABLE</param>
        static public void RecordSAPData(DataTable dtHead, DataTable dtDetail)
        {
            foreach (DataRow drHead in dtHead.Rows)
            {
                //get form head data
                DataTable dtHeadTemp = dtHead.Clone();
                dtHeadTemp.ImportRow(drHead);
                //string WERKF = drHead["WERKF"].ToString().Trim();//PLANT From  20120614 Add QX NA
                string WERKS = drHead["WERKS"].ToString().Trim();//PLANT
                string MBLNR_A = drHead["MBLNR_A"].ToString().Trim();//DOC NO
                string MJAHR_A = drHead["MJAHR_A"].ToString().Trim();//DOC YEAR
                string APPER = drHead["APPER"].ToString().Trim();//APPLIER
                string KOSTL = drHead["KOSTL"].ToString().Trim();// COST CENTER
                //get form detail data
                DataRow[] drArr = dtDetail.Select("WERKS = '" + WERKS + "' and MBLNR_A ='" + MBLNR_A + "' and MJAHR_A = " + MJAHR_A);
                if (drArr.Length <= 0) continue;
                // Detail Table
                DataTable dtDetailTemp = dtDetail.Clone();
                for (int i = 0; i < drArr.Length; i++)
                {
                    //檢查是否需作工單超耗量管控PLANT
                    DataRow dr = drArr[i];
                    StringBuilder sql = new StringBuilder();
                    sql.Append("SELECT PARAME_VALUE3 FROM TB_APPLICATION_PARAM WHERE APPLICATION_NAME = 'GoodsMovement' ");
                    sql.Append("AND FUNCTION_NAME = 'GetFormData' AND PARAME_NAME = 'InputParameter' AND PARAME_ITEM = 'PlantCode' AND PARAME_VALUE1 = @Plant ");
                    opc.Clear();
                    opc.Add(DataPara.CreateDataParameter("@Plant", SqlDbType.VarChar, WERKS));   //20130614 Change WERKS to WERKF

               
                    if (sdb.GetRowString(sql.ToString(), opc, "PARAME_VALUE3") == "OverControl")
                    {
                        //依PLANT配置警戒值，判斷是否需管控（調整ZOVISS欄位值，默認為N）
                        dtDetailTemp.ImportRow(DOA.CheckOverIssueFlag(WERKS, dr));
                    }
                    else
                    {
                        dtDetailTemp.ImportRow(dr);
                    }

                }

                string xmlHead = Tools.DataTableToXML(dtHeadTemp);
                string xmlDetail = Tools.DataTableToXML(dtDetailTemp);


                try
                {
                    sql = "INSERT INTO TB_GDS_DATA (XMLHead, XMLDetail, Flag, Update_Time, WERKS, MBLNR_A, MJAHR_A, APPER, KOSTL) VALUES (@XMLHead, @XMLDetail,'N', getdate(), @WERKS, @MBLNR_A, @MJAHR_A , @APPER, @KOSTL) ";
                    opc.Clear();
                    opc.Add(DataPara.CreateDataParameter("@XMLHead", SqlDbType.NText, xmlHead));
                    opc.Add(DataPara.CreateDataParameter("@XMLDetail", SqlDbType.NText, xmlDetail));
                    opc.Add(DataPara.CreateDataParameter("@WERKS", SqlDbType.VarChar, WERKS));  //20130614 Change WERKS to WERKF
                    opc.Add(DataPara.CreateDataParameter("@MBLNR_A", SqlDbType.VarChar, MBLNR_A));
                    opc.Add(DataPara.CreateDataParameter("@MJAHR_A", SqlDbType.VarChar, MJAHR_A));
                    opc.Add(DataPara.CreateDataParameter("@APPER", SqlDbType.VarChar, APPER));
                    opc.Add(DataPara.CreateDataParameter("@KOSTL", SqlDbType.VarChar, KOSTL));
                    sdb.ExecuteNonQuery(sql, opc);
                }
                catch (Exception ex)
                {
                    WriteLog(string.Format("# Fail: record sap data error:{0};WERKS={1};MBLNR_A={2};MJAHR_A={3}", ex.Message, WERKS, MBLNR_A, MJAHR_A), "GDS_GetSAPData");
                }

            }
        }

        /// <summary>
        /// 記錄退料單Mapping的領料單
        /// </summary>
        /// <param name="dtDetail"></param>
        static public void InsertMappingData(DataTable dtDetail)
        {
            foreach (DataRow drDetail in dtDetail.Rows)
            {
                string WERKS = drDetail["WERKS"].ToString().Trim();
                string MBLNR_A = drDetail["MBLNR_A"].ToString().Trim();
                string Type = MBLNR_A.Substring(0,2);

                switch (WERKS)
                { 
                    case "2680":
                    case "268S":
                        #region 退料單 Mapping 領料單 2680 IA单
                        if (Type == "IA" || Type == "I6")
                        {
                            string MJAHR_A = drDetail["MJAHR_A"].ToString().Trim();//年份
                            string ZEILE_A = drDetail["ZEILE_A"].ToString().Trim();//料號對應的Item
                            string MATNR = drDetail["MATNR"].ToString().Trim();//料號
                            string REFNO = drDetail["REFNO"].ToString().Trim();//Mapping的I1單
                            string MENGE = drDetail["MENGE"].ToString().Trim();//數量
                            string CHARG = drDetail["CHARG"].ToString().Trim();//Batch Number
                            StringBuilder sb = new StringBuilder();
                            sb.Append(@"INSERT INTO [SPM].[dbo].[TB_GDS_Mapping]
                               ([WERKS]
                               ,[MBLNR_A]
                               ,[MJAHR_A]
                               ,[ZEILE_A]
                               ,[MATNR]
                               ,[MENGE]
                               ,[POSTED]
                               ,[APROV]
                               ,[REFNO]
                               ,[CHARG]
                                )
                         VALUES
                               (@WERKS,
                                @MBLNR_A,
                                @MJAHR_A,
                                @ZEILE_A,
                                @MATNR,
                                @MENGE,
                                @POSTED,
                                @APROV,
                                @REFNO,
                                @CHARG)");
                            opc.Clear();
                            opc.Add(DataPara.CreateDataParameter("@WERKS", SqlDbType.NVarChar, WERKS));
                            opc.Add(DataPara.CreateDataParameter("@MBLNR_A", SqlDbType.NVarChar, MBLNR_A));
                            opc.Add(DataPara.CreateDataParameter("@MJAHR_A", SqlDbType.NVarChar, MJAHR_A));
                            opc.Add(DataPara.CreateDataParameter("@ZEILE_A", SqlDbType.NVarChar, ZEILE_A));
                            opc.Add(DataPara.CreateDataParameter("@MATNR", SqlDbType.NVarChar, MATNR));
                            opc.Add(DataPara.CreateDataParameter("@MENGE", SqlDbType.NVarChar, MENGE));
                            opc.Add(DataPara.CreateDataParameter("@POSTED", SqlDbType.NVarChar, ""));
                            opc.Add(DataPara.CreateDataParameter("@APROV", SqlDbType.NVarChar, ""));
                            opc.Add(DataPara.CreateDataParameter("@REFNO", SqlDbType.NVarChar, REFNO));
                            opc.Add(DataPara.CreateDataParameter("@CHARG", SqlDbType.NVarChar, CHARG));
                            try
                            {
                                sdb.ExecuteNonQuery(sb.ToString(), opc);

                            }
                            catch (Exception ex)
                            {
                                WriteLog(ex.Message, "Return_SAPDATA");
                            }
                        }
                        #endregion
                        break; 
                }
            
            }
        }

        /// <summary>
        /// 記錄簽核信息至QUEUE
        /// </summary>
        /// <param name="GDS">CONTROL TABLE DATA</param>
        /// <returns>result</returns>
        static public bool AddToQueue(Model_GDS GDS)
        {
            bool flag = false;
            opc.Clear();
            string sql1 = "(@WERKS, @MBLNR_A, @MJAHR_A, @APTYP, @CASEID, @APROV, @APRD, @APRT, @REMARK, @FLAG, @UPDATE_TIME) ";
            sql = "INSERT INTO TB_GDS_QUEUE " + sql1.Replace("@", "") + " VALUES " + sql1 + "";
            opc.Add(DataPara.CreateDataParameter("@WERKS", SqlDbType.VarChar, GDS.WERKS));
            opc.Add(DataPara.CreateDataParameter("@MBLNR_A", SqlDbType.VarChar, GDS.MBLNR_A));
            opc.Add(DataPara.CreateDataParameter("@MJAHR_A", SqlDbType.Float, GDS.MJAHR_A));
            opc.Add(DataPara.CreateDataParameter("@APTYP", SqlDbType.VarChar, GDS.APTYP));
            opc.Add(DataPara.CreateDataParameter("@CASEID", SqlDbType.Float, GDS.CASEID));
            opc.Add(DataPara.CreateDataParameter("@APROV", SqlDbType.VarChar, GDS.APROV));
            opc.Add(DataPara.CreateDataParameter("@APRD", SqlDbType.VarChar, GDS.APRD));
            opc.Add(DataPara.CreateDataParameter("@APRT", SqlDbType.VarChar, GDS.APRT));
            opc.Add(DataPara.CreateDataParameter("@REMARK", SqlDbType.VarChar, GDS.REMARK));
            opc.Add(DataPara.CreateDataParameter("@FLAG", SqlDbType.VarChar, "N"));
            opc.Add(DataPara.CreateDataParameter("@UPDATE_TIME", SqlDbType.DateTime, DateTime.Now));
            try
            {
                sdb.ExecuteNonQuery(sql, opc);
                flag = true;
            }
            catch (Exception ex)
            {
                WriteLog(string.Format("# Fail: record queue data error:{0};WERKS={1};MBLNR_A={2};MJAHR_A={3}", ex.Message, GDS.WERKS, GDS.MBLNR_A, GDS.MJAHR_A), "AddToQueue");
                flag = false;
            }
            return flag;
        }


        /// <summary>
        /// 更新Mapping表中APROV字段(退料单的签核状态)  避免异常单出现非Approve的单子 剛簽核完的單子POSETED欄位沒有值,只有已經簽核完加過帳的單子才會出現
        /// </summary>
        /// <param name="GDS"></param>
        /// <returns></returns>
        static public bool UpdateToMapping(Model_GDS GDS)
        {
            bool flag = false;
            opc.Clear();
            StringBuilder sb = new StringBuilder();
            sb.Append(@"Update TB_GDS_Mapping set APROV = @APROV WHERE MBLNR_A = @MBLNR_A");
            opc.Add(DataPara.CreateDataParameter("@APROV", SqlDbType.VarChar, GDS.APROV));
            opc.Add(DataPara.CreateDataParameter("@MBLNR_A", SqlDbType.VarChar, GDS.MBLNR_A));
            try
            {
                sdb.ExecuteNonQuery(sql, opc);
                flag = true;
            }
            catch (Exception ex)
            {
                WriteLog(string.Format("# Fail: record queue data error:{0};WERKS={1};MBLNR_A={2};MJAHR_A={3}", ex.Message, GDS.WERKS, GDS.MBLNR_A, GDS.MJAHR_A), "UpdateToQueue");
                flag = false;
            }
            return flag;
        }


        /// <summary>
        /// 記錄追溯LOG
        /// </summary>
        /// <param name="log"></param>
        /// <param name="handleType"></param>
        public static void RecordTraceLog(string handleType, string status,Model_DOAHandler oDOAHandler)
        {
            string log = string.Empty;
            switch (handleType)
            {
                case "A":
                    if (oDOAHandler._sEndFlag == "Y")
                    {
                        log = string.Format("DOA end {0} ", status);
                    }
                    else
                    {
                        log = string.Format("Get Hanlder {0},Role={1},handler={2} ", status, oDOAHandler._sRoleCode, oDOAHandler._sHandler);
                    }
              
                    break;
                case "B":
                    if (oDOAHandler._sEndFlag == "Y")
                    {
                        log = string.Format("Update DOA {0}", status);
                    }
                    else
                    {
                        log = string.Format("Update DOA {0},Role={1},handler={2} ", status, oDOAHandler._sRoleCode, oDOAHandler._sHandler);
                    }
           
                    break;
                case "C":
                    log = string.Format("Add queue {0} ", status);
                    break;
                case "D":
                    if (oDOAHandler._sEndFlag == "N")
                    {
                        log = string.Format("Prepare RoutingVariable {0}:spm_Jump DOA({1}) ",status, oDOAHandler._sHandler);
                    }
                    else
                    {
                        log = string.Format("Prepare RoutingVariable {0}:spm_Jump End ", status);
                    }
              
                    break;
                case "E":
                    log = string.Format("DOA Jump ,ErrMsg:{0},DOA string:{1}", status, oDOAHandler._sDOA);
                    break;
            }
            DBIO.WriteTraceLog(log, oDOAHandler);
        }

        /// <summary>
        /// write log to database
        /// </summary>
        /// <param name="logString">err msg</param>
        /// <param name="appName">程序名稱</param>
        static public void WriteLog(string logString, string appName)
        {

            sql = "INSERT INTO TB_GDS_LOG (Application, LogTime, Message) VALUES (@APP, @LOGTIME, @MESSAGE)";
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@APP", SqlDbType.VarChar, appName));
            opc.Add(DataPara.CreateDataParameter("@LOGTIME", SqlDbType.DateTime, DateTime.Now));
            opc.Add(DataPara.CreateDataParameter("@MESSAGE", SqlDbType.VarChar, logString));
            try
            {
                sdb.ExecuteNonQuery(sql, opc);
            }
            catch (Exception ex)
            {
                throw new Exception("# Fail: db access error " + ex.Message);
            }
        }

        /// <summary>
        /// write Trace log to database
        /// </summary>
        /// <param name="logString">log</param>
        /// <param name="appName">程序名稱</param>
        static public void WriteTraceLog(string logString, Model_DOAHandler DOAHandler)
        {

            sql = "INSERT INTO TB_GDS_LOG (Application, LogTime, Message,Plant,DocNo,DocYear) VALUES (@APP, @LOGTIME, @MESSAGE, @Plant, @DocNo, @DocYear)";
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@APP", SqlDbType.VarChar, "DOA Trace"));
            opc.Add(DataPara.CreateDataParameter("@LOGTIME", SqlDbType.DateTime, DateTime.Now));
            opc.Add(DataPara.CreateDataParameter("@MESSAGE", SqlDbType.VarChar, logString));
            opc.Add(DataPara.CreateDataParameter("@Plant", SqlDbType.VarChar, DOAHandler._sPlant));
            opc.Add(DataPara.CreateDataParameter("@DocNo", SqlDbType.VarChar, DOAHandler._sDocNo));
            opc.Add(DataPara.CreateDataParameter("@DocYear", SqlDbType.VarChar, DOAHandler._sDocYear));
            try
            {
                sdb.ExecuteNonQuery(sql, opc);
            }
            catch (Exception ex)
            {
                throw new Exception("# Fail: db access error " + ex.Message);
            }
        }

        /// <summary>
        /// 獲取IT負責人用於異常郵件通知 
        /// </summary>
        /// <param name="plantCode"></param>
        /// <returns></returns>
        static public string GetITWindow(string plantCode)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT PARAME_VALUE2 AS ITWindow  FROM TB_APPLICATION_PARAM  with(nolock) ");
            sql.Append("WHERE APPLICATION_NAME = 'GoodsMovement' ");
            sql.Append("AND FUNCTION_NAME = 'GetFormData' AND PARAME_NAME = 'InputParameter'   ");
            sql.Append("AND PARAME_ITEM = 'PlantCode' AND PARAME_VALUE1 = @PlantCode");
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@PlantCode", SqlDbType.VarChar, plantCode));
            return sdb.GetRowString(sql.ToString(), opc, "ITWindow");
        }

        /// <summary>
        /// 記錄簽核角色為並簽情形
        /// </summary>
        /// <param name="pDOAHandler"></param>
        /// <returns></returns>
        static public bool RecordParallelApprovalInfo(Model_DOAHandler pDOAHandler)
        {
            bool flag = true;
            if (pDOAHandler._sEndFlag == "N")
            {
                string[] handlerList = pDOAHandler._sHandler.Split(',');
                if (handlerList.Length > 1)
                {
                    sql = "INSERT INTO TB_GDS_ParallelApproval (Plant,DocYear,DocNo,RoleCode,TotalCount,ActualCount,UPDATE_TIME) VALUES (@Plant,@DocYear,@DocNo,@RoleCode,@TotalCount,0,getdate()) ";
                    opc.Clear();
                    opc.Add(DataPara.CreateDataParameter("@Plant", SqlDbType.VarChar, pDOAHandler._sPlant));
                    opc.Add(DataPara.CreateDataParameter("@DocYear", SqlDbType.VarChar, pDOAHandler._sDocYear));
                    opc.Add(DataPara.CreateDataParameter("@DocNo", SqlDbType.VarChar, pDOAHandler._sDocNo));
                    opc.Add(DataPara.CreateDataParameter("@RoleCode", SqlDbType.VarChar, pDOAHandler._sRoleCode));
                    opc.Add(DataPara.CreateDataParameter("@TotalCount", SqlDbType.Int, handlerList.Length));
                    try
                    {
                        sdb.ExecuteNonQuery(sql, opc);
                    }
                    catch (Exception ex)
                    {
                        flag = false;
                        throw new Exception("# Fail: db access error " + ex.Message);
                    }
                }
            }

            return flag;
        }


        /// <summary>
        ///更新並簽狀態
        /// </summary>
        /// <param name="pDOAHandler"></param>
        /// <returns></returns>
        static public bool UpdateParallelApprovalStatus(Model_DOAHandler pDOAHandler)
        {
            bool flag = true;
            if (pDOAHandler._ParallelFlag == true)
            {
                sql = "UPDATE TB_GDS_ParallelApproval SET ActualCount=ActualCount+1 WHERE Plant = @Plant AND DocYear=@DocYear AND DocNo = @DocNo AND RoleCode = @RoleCode ";
                opc.Clear();
                opc.Add(DataPara.CreateDataParameter("@Plant", SqlDbType.VarChar, pDOAHandler._sPlant));
                opc.Add(DataPara.CreateDataParameter("@DocYear", SqlDbType.VarChar, pDOAHandler._sDocYear));
                opc.Add(DataPara.CreateDataParameter("@DocNo", SqlDbType.VarChar, pDOAHandler._sDocNo));
                opc.Add(DataPara.CreateDataParameter("@RoleCode", SqlDbType.VarChar, pDOAHandler._sRoleCode));
                try
                {
                    sdb.ExecuteNonQuery(sql, opc);
                }
                catch (Exception ex)
                {
                    flag = false;
                    throw new Exception("# Fail: db access error " + ex.Message);
                }

            }

            return flag;
        }

        /// <summary>
        /// 清除並簽信息
        /// </summary>
        /// <param name="plant"></param>
        /// <param name="docNo"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        static public bool DeleteParallelApprovalInfo(string plant, string docNo, string year)
        {
            bool flag = true;

            sql = "DELETE FROM TB_GDS_ParallelApproval WHERE  Plant = @Plant AND DocYear=@DocYear AND DocNo = @DocNo ";
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@Plant", SqlDbType.VarChar, plant));
            opc.Add(DataPara.CreateDataParameter("@DocYear", SqlDbType.VarChar, year));
            opc.Add(DataPara.CreateDataParameter("@DocNo", SqlDbType.VarChar, docNo));
            try
            {
                sdb.ExecuteNonQuery(sql, opc);
            }
            catch (Exception ex)
            {
                flag = false;
                throw new Exception("# Fail: db access error " + ex.Message);
            }

            return flag;
        }
    }
}
