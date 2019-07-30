using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using LiteOn.EA.BLL;
using LiteOn.EA.Model;
using LiteOn.EA.DAL;

namespace LiteOn.GDS.Utility
{

    /// <summary>
    /// plant code CS01
    /// </summary>
    public class DOA_GZ_ENCLOSURE : DOA_Standard
    {

        public DOA_GZ_ENCLOSURE()
        {
        }



        /// <summary>
        /// 重載標準DOA-特殊規則獲取簽核人(TYPE C)
        /// </summary>
        /// <param name="roleCode"></param>
        /// <param name="dtHead"></param>
        /// <param name="dtDetail"></param>
        protected override void GetDOABySpeicalRule(string roleCode, DataTable dtHead, DataTable dtDetail)
        {
            DataRow drHead = dtHead.Rows[0];
            DataRow drDetail = dtDetail.Rows[0];
            string reasonCode = drHead["GRUND"].ToString();
            string sAPTYP = drHead["APTYP"].ToString();
            string sLOC = drHead["LOCFM"].ToString();
            string costCenter = drHead["KOSTL"].ToString().ToUpper();
            string procurementType = drDetail["BESKZ"].ToString().Trim().ToUpper();
            string prdSuppervisor = drDetail["FEVOR"].ToString().Trim().ToUpper();
            string applydate = drHead["ERDAT"].ToString().Trim().ToUpper();
            string applier = drHead["APPER"].ToString().Trim().ToUpper();

            bool jumpFlag = false;//跳關標示
            switch (roleCode)
            {
                case "CS01-DEPT01":
                case "CS02-DEPT01":
                    switch (costCenter)
                    {
                        case "B315"://PM
                            oDOAHandler._sJump = "Y";//PM部門 窗口不用簽，向后跳關
                            try
                            {
                                oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                            }
                            catch (Exception ex)
                            {
                                DBIO.RecordTraceLog("E", ex.Message, oDOAHandler);
                                throw new Exception("Server or network is busy now,pls try again");
                            }
                            break;
                        case "B460"://品保部以不同的開單人簽不同的老闆
                            oDOAHandler._sJump = "N";
                            oDOAHandler._sHandler = GetDOAHandler_ENCLOSURE(roleCode, costCenter, applier);
                            break;
                        default:
                            oDOAHandler._sJump = "N";
                            oDOAHandler._sHandler = GetDOAHandler(roleCode, costCenter);
                            break;
                    }
                    break;
                case "CS01-DEPT02":
                case "CS02-DEPT02":
                    if (costCenter.Length > 0)
                    {
                        switch (costCenter)
                        {
                            case "B460"://品保部以不同的開單人簽不同的老闆
                                oDOAHandler._sJump = "N";
                                oDOAHandler._sHandler = GetDOAHandler_ENCLOSURE(roleCode, costCenter, applier);
                                break;
                            default:
                                oDOAHandler._sJump = "N";
                                oDOAHandler._sHandler = GetDOAHandler(roleCode, costCenter);
                                break;
                        }
                    }
                    else
                    {
                        GetHandlerErrAlarm(dtHead, "COST CENTER信息為空，無法獲取簽核人");
                    }
                    break;
                case "CS01-IPQC01":
                case "CS02-IPQC01":
                    switch (sAPTYP)
                    {
                        case "I5":
                            switch (costCenter)
                            {
                                case "B460"://I5單據，CC為CQS時無需簽核 
                                case "B315"://I5單據，CC為PM時無需簽核
                                    jumpFlag = true;
                                    break;
                                default:
                                    jumpFlag = false;
                                    break;
                            }
                            break;
                        default:
                            jumpFlag = false;
                            break;
                    }
                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";//不用簽，向后跳關
                        try
                        {
                            oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                        }
                        catch (Exception ex)
                        {
                            DBIO.RecordTraceLog("E", ex.Message, oDOAHandler);
                            throw new Exception("Server or network is busy now,pls try again");
                        }
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";//依COST CENTER 獲取簽核人
                        oDOAHandler._sHandler = GetDOAHandler(roleCode, prdSuppervisor);
                    }

                    break;
                case "CS01-IQC01":
                case "CS02-IQC01":
                    #region
                    if (sAPTYP == "IC")
                    {
                        switch (costCenter)
                        {
                            case "B410":
                            case "B420":
                            case "B430":
                            case "B440":
                                if (reasonCode == "0021" && procurementType == "F")
                                {
                                    jumpFlag = false;

                                }
                                else
                                {
                                    jumpFlag = true;
                                }
                                break;

                            default:
                                jumpFlag = true;
                                break;
                        }
                    }
                    else
                    {
                        jumpFlag = true;
                    }
                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";//不用簽，向后跳關
                        try
                        {
                            oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                        }
                        catch (Exception ex)
                        {
                            DBIO.RecordTraceLog("E", ex.Message, oDOAHandler);
                            throw new Exception("Server or network is busy now,pls try again");
                        }
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";//依COST CENTER 獲取簽核人
                        oDOAHandler._sHandler = GetDOAHandler(roleCode, costCenter);
                    }
                    #endregion
                    break;
                case "CS01-MC01":
                case "CS02-MC01":
                    #region
                    if (procurementType == "F")
                    {
                        jumpFlag = false;
                    }
                    else
                    {
                        jumpFlag = true;
                    }
                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";//不用簽，向后跳關
                        try
                        {
                            oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                        }
                        catch (Exception ex)
                        {
                            DBIO.RecordTraceLog("E", ex.Message, oDOAHandler);
                            throw new Exception("Server or network is busy now,pls try again");
                        }
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        //依BUYER CODE 獲取簽核人
                        Hashtable ht = new Hashtable();
                        foreach (DataRow dr in dtDetail.Rows)
                        {
                            string buyerCode = dr["EKGRP"].ToString();
                            if (buyerCode.Length > 0)
                            {
                                string buyer = GetDOAHandlerByBuyerCode(buyerCode);
                          
                                if (ht.Contains(buyer) == false)
                                {
                                    ht.Add(buyer, 0);
                                    if (oDOAHandler._sHandler.Length > 0)
                                    {
                                        oDOAHandler._sHandler += ",";
                                    }
                                    oDOAHandler._sHandler += buyer;
                                }
                            }
                        }
                    }
                    #endregion
                    break;
                case "CS01-PC01":
                case "CS02-PC01":
                    #region
                    if (sAPTYP == "IC")
                    {
                        switch (costCenter)
                        {
                            case "B410":
                            case "B420":
                            case "B430":
                            case "B440":
                                if (procurementType == "E")
                                {
                                    jumpFlag = false;
                                }
                                else
                                {
                                    jumpFlag = true;
                                }
                                break;
                            default:
                                jumpFlag = true;
                                break;
                        }
                    }
                    else
                    {
                        if (procurementType == "E")
                        {
                            jumpFlag = false;
                        }
                        else
                        {
                            jumpFlag = true;
                        }
                    }

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";//不用簽，向后跳關
                        try
                        {
                            oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                        }
                        catch (Exception ex)
                        {
                            DBIO.RecordTraceLog("E", ex.Message, oDOAHandler);
                            throw new Exception("Server or network is busy now,pls try again");
                        }
                    }
                    else
                    {
                        //依production Suppervisor 獲取簽核人
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode, prdSuppervisor);
                    }
                    #endregion
                    break;
                case "CS01-CUST01":
                case "CS02-CUST01":
                    #region
                    switch (sAPTYP)
                    {
                        case "I3":
                            if (reasonCode != "0060" && reasonCode != "0064")
                            {
                                oDOAHandler._sJump = "N";
                                oDOAHandler._sHandler = GetDOAHandler(roleCode);
                            }
                            else
                            {
                                oDOAHandler._sJump = "Y";//不用簽，向后跳關
                                try
                                {
                                    oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                                }
                                catch (Exception ex)
                                {
                                    DBIO.RecordTraceLog("E", ex.Message, oDOAHandler);
                                    throw new Exception("Server or network is busy now,pls try again");
                                }
                            }
                            break;
                        case "IC":
                            switch (costCenter)
                            {
                                case "B410":
                                case "B420":
                                case "B430":
                                case "B440":
                                    if (reasonCode == "0056" || reasonCode == "0057")
                                    {
                                        oDOAHandler._sJump = "N";
                                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                                    }
                                    else
                                    {
                                        oDOAHandler._sJump = "Y";//不用簽，向后跳關
                                        try
                                        {
                                            oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                                        }
                                        catch (Exception ex)
                                        {
                                            DBIO.RecordTraceLog("E", ex.Message, oDOAHandler);
                                            throw new Exception("Server or network is busy now,pls try again");
                                        }
                                    }
                                    break;
                                default:
                                    oDOAHandler._sJump = "Y";//不用簽，向后跳關
                                    try
                                    {
                                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                                    }
                                    catch (Exception ex)
                                    {
                                        DBIO.RecordTraceLog("E", ex.Message, oDOAHandler);
                                        throw new Exception("Server or network is busy now,pls try again");
                                    }
                                    break;
                            }

                            break;
                        case "I5":
                            oDOAHandler._sJump = "N";//I5必簽
                            oDOAHandler._sHandler = GetDOAHandler(roleCode);
                            break;
                    }
                    #endregion
                    break;
                case "CS01-MC02":
                case "CS02-MC02":
                    #region
                    if (procurementType == "F")
                    {

                        switch (costCenter)
                        {
                            case "B315":
                            case "B460":   //B315,B460 I5單據MC主管為必簽
                                if (sAPTYP == "I5")
                                {
                                    jumpFlag = false;
                                }
                                else
                                {
                                    //CC B315,B460 其它單據MC主管需檢查KPI
                                    jumpFlag = (CheckKPI(sAPTYP, costCenter, applydate, procurementType, dtDetail) == true) ? false : true;
                                }
                                break;
                            default:
                                //其它CC 所有單據MC主管需檢查KPI
                                jumpFlag = (CheckKPI(sAPTYP, costCenter, applydate, procurementType, dtDetail) == true) ? false : true;
                                break;
                        }
                    }
                    else
                    {
                        jumpFlag = true;
                    }

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";//不用簽，向后跳關
                        try
                        {
                            oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                        }
                        catch (Exception ex)
                        {
                            DBIO.RecordTraceLog("E", ex.Message, oDOAHandler);
                            throw new Exception("Server or network is busy now,pls try again");
                        }
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);

                    }
                    #endregion
                    break;
                case "CS01-PC02":
                case "CS02-PC02":
                    #region
                    if (procurementType == "E")
                    {

                        switch (costCenter)
                        {
                            case "B315":
                            case "B460":   //B315,B460 I5單據PC主管為必簽
                                if (sAPTYP == "I5")
                                {
                                    jumpFlag = false;
                                }
                                else
                                {
                                    //CC B315,B460 其它單據PC主管需檢查KPI
                                    jumpFlag = (CheckKPI(sAPTYP, costCenter, applydate, procurementType, dtDetail) == true) ? false : true;
                                }
                                break;
                            default:
                                //其它CC 所有單據PC主管需檢查KPI
                                jumpFlag = (CheckKPI(sAPTYP, costCenter, applydate, procurementType, dtDetail) == true) ? false : true;
                                break;
                        }
                    }
                    else
                    {
                        jumpFlag = true;
                    }

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";//不用簽，向后跳關
                        try
                        {
                            oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                        }
                        catch (Exception ex)
                        {
                            DBIO.RecordTraceLog("E", ex.Message, oDOAHandler);
                            throw new Exception("Server or network is busy now,pls try again");
                        }
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);

                    }
                    #endregion
                    break;

                case "CS01-LMD01":
                case "CS02-LMD01":
                    #region
                    switch (costCenter)
                    {
                        case "B315":
                        case "B460":   //B315,B460 I5單據LMD主管為必簽
                            if (sAPTYP == "I5")
                            {
                                jumpFlag = false;
                            }
                            else
                            {
                                //CC B315,B460 其它單據LMD主管需檢查KPI
                                jumpFlag = (CheckKPI(sAPTYP, costCenter, applydate, procurementType, dtDetail) == true) ? false : true;
                            }
                            break;
                        default:
                            //其它CC 所有單據PC主管需檢查KPI
                            jumpFlag = (CheckKPI(sAPTYP, costCenter, applydate, procurementType, dtDetail) == true) ? false : true;
                            break;
                    }
                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";//不用簽，向后跳關
                        try
                        {
                            oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                        }
                        catch (Exception ex)
                        {
                            DBIO.RecordTraceLog("E", ex.Message, oDOAHandler);
                            throw new Exception("Server or network is busy now,pls try again");
                        }
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);

                    }
                    #endregion
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// 重載-檢查是否並簽
        /// </summary>
        /// <param name="caseId"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        protected override void ParallelApprovalCheck(Model_DOAHandler DOAHandler, string roleCode)
        {
            try
            {
                sql = "SELECT * FROM TB_GDS_ParallelApproval  with(nolock) WHERE  Plant = @Plant AND DocYear=@DocYear AND DocNo = @DocNo AND RoleCode = @RoleCode ORDER BY UPDATE_TIME DESC ";
                opc.Clear();
                opc.Add(DataPara.CreateDataParameter("@RoleCode", SqlDbType.VarChar, roleCode));
                opc.Add(DataPara.CreateDataParameter("@Plant", SqlDbType.VarChar, DOAHandler._sPlant));
                opc.Add(DataPara.CreateDataParameter("@DocNo", SqlDbType.VarChar, DOAHandler._sDocNo));
                opc.Add(DataPara.CreateDataParameter("@DocYear", SqlDbType.VarChar, DOAHandler._sDocYear));
                DataTable dt = sdb.GetDataTable(sql, opc);

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    oDOAHandler._ParallelFlag = true;
                    int totalCount = int.Parse(dr["totalCount"].ToString());
                    int actualcount = int.Parse(dr["actualcount"].ToString());
                    switch (roleCode)
                    {
                        case "CS01-DEPT01":
                        case "CS01-DEPT02":
                        case "CS02-DEPT01":
                        case "CS02-DEPT02":
                            //任意一個人簽就跳關

                            oDOAHandler._FinalFlag = true;
                            break;
                        default:
                            //全部簽就跳關
                            if (totalCount - 1 > actualcount)
                            {
                                oDOAHandler._FinalFlag = false;
                            }
                            else
                            {
                                oDOAHandler._FinalFlag = true;
                            }

                            break;
                    }

                }
                else
                {
                    oDOAHandler._ParallelFlag = false;
                    oDOAHandler._FinalFlag = false;
                }
            }
            catch (Exception)
            {
                throw new Exception(" DB Access fail,pls contact sys administrator");
            }
        }


        /// <summary>
        /// SAP日期轉標準DATETIME
        /// </summary>
        /// <param name="dateSAP"></param>
        /// <returns></returns>
        private DateTime SAPDateTimeConvert(string dateSAP)
        {

            //對於非標準日期格式yyyyMMdd，采用 TryParseExact轉DATETIME
            DateTime tmp;
            DateTime.TryParseExact(dateSAP, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AdjustToUniversal, out tmp);
            return tmp;
        }

        /// <summary>
        /// 獲取所處季度
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private string GetQuarterByDate(DateTime date)
        {
            string quar = string.Empty;
            switch (date.Month)
            {
                case 1:
                case 2:
                case 3:
                    quar = "Q1";
                    break;

                case 4:
                case 5:
                case 6:
                    quar = "Q2";
                    break;

                case 7:
                case 8:
                case 9:
                    quar = "Q3";
                    break;
                default:
                    quar = "Q4";
                    break;

            }
            return quar;
        }

        /// <summary>
        /// 檢查是否超出該COST CENTER當季度KPI設定時
        /// </summary>
        /// <param name="dtDetail"></param>
        /// <returns></returns>
        private bool CheckKPI(string apType, string costCenter, string applyDate, string procurementType, DataTable dtDetail)
        {
            bool flag = false;
            //取標準KPI設定值
            Dictionary<string, object> controlKPI = GetKPI(apType, costCenter, applyDate, procurementType);

            //未找到KPI設定值，不作檢查
            if (controlKPI["Exist"].ToString() == "N")
            {
                return flag;
            }

            //循環檢查LINE DATA
            foreach (DataRow dr in dtDetail.Rows)
            {


                //確認SAP有傳值（數量比例Over_RATE是必須字段）
                string overRate = dr["Over_RATE"].ToString();
                if (overRate.Length == 0) continue;
                bool specialPartNo = false;
                //檢查是否特殊物料管控
                #region
                if (controlKPI["SpecialRule"].ToString() == "Y")
                {
                    //獲取特殊物料設定KPI
                    DataRow[] drList_Special = (DataRow[])controlKPI["SpecialKPI"];
                    //BY 料號檢查是否匹屬於特殊物料
                    foreach (DataRow specKPI in drList_Special)
                    {
                        //比對新料號前兩碼
                        string partNo = dr["MATNR"].ToString();
                        if (partNo.Length >= 2 && partNo.Substring(0, 2) == specKPI["NewPartNo"].ToString())
                        {
                            specialPartNo = true;
                            switch (specKPI["CheckOption"].ToString())
                            {
                                case "A":
                                    //A:只檢查數量比例是否超特殊物料KPI
                                    if (double.Parse(specKPI["OverRate_spec"].ToString()) < double.Parse(overRate))
                                    {
                                        flag = true;
                                    }
                                    break;
                                default: break;
                            }
                        }
                    }
                }
                #endregion
                //如特殊管制物料超出Special KPI設定，返回，不再作標準KPI檢查
                if (flag) break;
                //如特殊管制物料未超出Special KPI設定，跳至下一個循環
                if (specialPartNo) continue;
                //檢查數量比例是否超標準KPI
                if (double.Parse(controlKPI["OverRate"].ToString()) < double.Parse(overRate))
                {
                    flag = true;
                    break;
                }

                //檢查金額是否超標準KPI
                string overAmount = dr["Over_AMT"].ToString();
                if (overAmount.Length > 0)
                {
                    if (double.Parse(controlKPI["OverAmount"].ToString()) <= double.Parse(overAmount))
                    {
                        flag = true;
                        break;
                    }
                }
            }
            return flag;
        }

        /// <summary>
        /// 獲取設定KPI
        /// </summary>
        /// <param name="apType"></param>
        /// <param name="costCenter"></param>
        /// <param name="applyDate"></param>
        /// <param name="procurementType"></param>
        /// <returns></returns>
        private Dictionary<string, object> GetKPI(string apType, string costCenter, string applyDate, string procurementType)
        {
            DateTime date = SAPDateTimeConvert(applyDate);
            int controlYear = date.Year;
            string controlQuarter = GetQuarterByDate(date);
            Dictionary<string, object> standardKPI = new Dictionary<string, object>();

            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT 	t1.APType,t1.CostCenter,t1.ControlYear,t1.ControlQuarter, ");
            sql.Append("	t1.ProcurementType,t1.OverRate,t1.OverAmount,t2.OverRate  AS OverRate_spec , ");
            sql.Append("	t2.OverAmount AS OverAmount_spec,t2.NewPartNo ,t2.CheckOption ");
            sql.Append(" FROM TB_GDS_DOA_KPI t1 left join TB_GDS_DOA_KPI_Special t2 ");
            sql.Append(" on t1.CostCenter=t2.CostCenter AND t1.APType=t2.APType AND t1.ControlYear=t2.ControlYear ");
            sql.Append("AND t1.ControlQuarter = t2.ControlQuarter AND t1.ProcurementType=t2.ProcurementType ");
            sql.Append("WHERE t1.APType=@APType AND t1.CostCenter=@CostCenter AND t1.ControlYear=@ControlYear ");
            sql.Append("AND t1.ControlQuarter=@ControlQuarter AND t1.ProcurementType=@ProcurementType ");
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@APType", SqlDbType.VarChar, apType));
            opc.Add(DataPara.CreateDataParameter("@CostCenter", SqlDbType.VarChar, costCenter));
            opc.Add(DataPara.CreateDataParameter("@ControlYear", SqlDbType.Int, controlYear));
            opc.Add(DataPara.CreateDataParameter("@ControlQuarter", SqlDbType.VarChar, controlQuarter));
            opc.Add(DataPara.CreateDataParameter("@ProcurementType", SqlDbType.VarChar, procurementType));
            DataTable dt = sdb.GetDataTable(sql.ToString(), opc);

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                standardKPI["Exist"] = "Y";
                standardKPI["OverRate"] = dr["OverRate"].ToString();
                standardKPI["OverAmount"] = dr["OverAmount"].ToString();
                DataRow[] drSpec = dt.Select(" NewPartNo IS not null ");
                if (drSpec.Length > 0)
                {
                    standardKPI["SpecialRule"] = "Y";
                    standardKPI["SpecialKPI"] = drSpec;
                }
                else
                {
                    standardKPI["SpecialRule"] = "N";
                    standardKPI["SpecialKPI"] = drSpec;
                }
            }
            else
            {
                standardKPI["Exist"] = "N";
                standardKPI["OverRate"] = string.Empty;
                standardKPI["OverAmount"] = string.Empty;
                standardKPI["SpecialRule"] = "N";
                standardKPI["SpecialKPI"] = null;
            }
            return standardKPI;
        }

        /// <summary>
        /// BY 成本中心,申請人工號獲取簽核人 
        /// </summary>
        /// <param name="roleCode">關卡名</param>
        /// <param name="value1">成本中心</param>
        /// <param name="value2">申請人工號</param>
        /// <returns>Handler</returns>
        protected string GetDOAHandler_ENCLOSURE(string roleCode, string value1, string value2)
        {
            sql = "SELECT * FROM  TB_GDS_DOA_DETAIL with(nolock) WHERE roleCode = @RoleCode AND value1 = @value1 AND value2 = @value2 ";
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@RoleCode", SqlDbType.VarChar, roleCode));
            opc.Add(DataPara.CreateDataParameter("@value1", SqlDbType.VarChar, value1));
            opc.Add(DataPara.CreateDataParameter("@value2", SqlDbType.VarChar, value2));

            return sdb.GetRowString(sql, opc, "VALUE3");
        }

        /// <summary>
        /// 取行動裝置簽核欄位
        /// </summary>
        /// <param name="dtHead"></param>
        /// <param name="dtDetail"></param>
        /// <returns></returns>
        public override Hashtable GetMobileFormFields(DataTable dtHead, DataTable dtDetail)
        {
            Hashtable ht = new Hashtable();

            DataRow drHead = dtHead.Rows[0];

            //string sAPTYP = drHead["APTYP"].ToString();//單據類型
            ht.Add("M_LOCFM", drHead["LOCFM"].ToString());//Sloc
            ht.Add("M_KOSTL", drHead["KOSTL"].ToString());//成本中心

            sql = "select * from tb_gds_reason where GRUND = @GRUND and BWART = @BWART ";
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@GRUND", SqlDbType.Float, double.Parse(drHead["GRUND"].ToString().Trim())));
            opc.Add(DataPara.CreateDataParameter("@BWART", SqlDbType.VarChar, drHead["BWART"].ToString().Trim()));
            ht.Add("M_GRUND", sdb.GetRowString(sql, opc, "GRTXT"));//原因
            ht.Add("M_DESC", drHead["DESC"].ToString());//中文描述
            ht.Add("M_TOTALSTPRS", string.Format("{0:N4}", dtDetail.Compute("sum(STPRS)", "")).TrimEnd('0').TrimEnd('.'));//總金額(￥)         

            StringBuilder sbMobile = new StringBuilder();

            for (int i = 0; i < dtDetail.Rows.Count; i++)
            {
                DataRow drDetail = dtDetail.Rows[i];
                sbMobile.AppendLine(string.Format("{0} {1} {2}{3} {4}%",
                        Convert.ToString(i + 1),
                        drDetail["MATNR"].ToString().Trim(),//料號
                        drDetail["MENGE"].ToString().Trim(),//數量
                        drDetail["MEINS"].ToString().Trim(),//單位
                        drDetail["OVER_RATE"].ToString().Trim()//累計比例(%)
                ));
            }
            ht.Add("M_DETAIL", sbMobile.ToString());  

            return ht;
        }
    }
}
