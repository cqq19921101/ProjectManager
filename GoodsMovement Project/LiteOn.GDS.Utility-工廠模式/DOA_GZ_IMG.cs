using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using LiteOn.EA.BLL;
using LiteOn.EA.Model;
using LiteOn.EA.DAL;

namespace LiteOn.GDS.Utility
{
    public class DOA_GZ_IMG : DOA_Standard
    {
        public DOA_GZ_IMG() { }

        protected override void GetDOABySpeicalRule(string roleCode, System.Data.DataTable dtHead, System.Data.DataTable dtDetail)
        {
            #region

            bool jumpFlag = false;//跳關標示
            string buyerCode = string.Empty;
            DataRow drHead = dtHead.Rows[0];
            DataRow drDetail = dtDetail.Rows[0];
            string applydate = drHead["ERDAT"].ToString().Trim().ToUpper();
            string sAPTYP = drHead["APTYP"].ToString();
            string sLOCFrom = drHead["LOCFM"].ToString();
            string sLOCTo = drHead["LOCTO"].ToString();
            string costCenter = drHead["KOSTL"].ToString().ToUpper();
            string sCostCenter = drHead["KOSTL"].ToString().Substring(0, 3).ToUpper().Trim();
            string sDepartment = drHead["ABTEI"].ToString().ToUpper().Trim();
            //string procurementType = drDetail["BESKZ"].ToString().Trim().ToUpper();
            string procurementType = drDetail["GMFLOW"].ToString().Trim().ToUpper();
            //總溢領金額(RMB)
            string sTTLAMT = drHead["TTLAMT"].ToString().Trim();
            //總溢領比例(%)
            string sTTLRATE = drHead["TTLRATE"].ToString().Trim();
            string materialType = string.Empty;
            //string sApplier = drHead["APPER"].ToString();//區域代碼
            
            string orderType = string.Empty;
            DataRow[] orderCheck = dtDetail.Select(" GMFLOW='T'");//重工工單標誌 R:為重工，空為非重工,T:為試產工單
            if (orderCheck.Length > 0) orderType = "T";

            switch (roleCode)
            {               
                case "32A3-Manager01":
                    oDOAHandler._sHandler = GetDOAHandler(roleCode, costCenter);
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("成本中心:{0} 信息有誤，無法獲取簽核人", costCenter));
                        }                   
                    break;
                //物料類型中只有半品時簽PC,否則簽MM
                case "32A3-PC01":
                    List<string> sHandlers = new List<string>();
                    for (int i = 0; i < dtDetail.Rows.Count; i++)
                    {
                        DataRow drMaterial = dtDetail.Rows[i];
                        //材料類型:ZIFT:成品,ZIHL:半成品,ZIRH:原材料
                        if (drMaterial["MTART"].ToString().Trim().ToUpper() != "ZIHL")
                        {
                            jumpFlag = true;
                            break;
                        }
                        //string sMRPController = dtDetail.Rows[i]["DISPO"].ToString().Trim().ToUpper();
                        //string handler = GetDOAHandler(roleCode, sMRPController);
                        string handler = GetDOAHandler(roleCode);
                        if (!sHandlers.Contains(handler))
                        {
                            sHandlers.Add(handler);
                        }

                    }
                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                        
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = string.Join(",", sHandlers.ToArray());
                      
                    }
                    break;
                    //有材料簽MM
                case "32A3-MM01":
                    for (int i = 0; i < dtDetail.Rows.Count; i++)
                    {
                        DataRow drMaterial = dtDetail.Rows[i];
                        //材料類型:ZIFT:成品,ZIHL:半成品,ZIRH:原材料
                        if (drMaterial["MTART"].ToString().Trim().ToUpper() == "ZIRH" || drMaterial["MTART"].ToString().Trim().ToUpper() == "ZIFT")//有材料或成品簽MM
                        {
                            jumpFlag = false;
                            break;
                        }
                        else
                        {
                            jumpFlag = true;
                        }

                    }
                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                       
                    }
                    else
                    {
                        List<string> PurchasingGroup = new List<string>();
                        //获取CC人员
                        foreach (DataRow item in dtDetail.Rows)
                        {
                            string code = item["EKGRP"].ToString();
                            if (!string.IsNullOrEmpty(code))
                            {                               
                              PurchasingGroup.Add(GetDOACC(code));                               
                            }
                            
                        }
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode, costCenter);
                        //oDOAHandler._cc = GetNextCCUserID(GetDOAHandler_Value3(roleCode, costCenter));
                        oDOAHandler._cc=GetNextCCUserID(string.Join(";",PurchasingGroup.ToArray()));
                    }
                    break; 
                case "32A3-SCM01":
                    for (int i = 0; i < dtDetail.Rows.Count; i++)
                    {
                        DataRow drMaterial = dtDetail.Rows[i];
                        //材料類型:ZIFT:成品,ZIHL:半成品,ZIRH:原材料
                        if (drMaterial["MTART"].ToString().Trim().ToUpper() == "ZIRH" || drMaterial["MTART"].ToString().Trim().ToUpper() == "ZIFT")//有材料或成品簽MC
                        {
                            jumpFlag = false;
                            break;
                        }
                        else
                        {
                            jumpFlag = true;
                        }

                    }
                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                    }
                    //oDOAHandler._sHandler = GetDOAHandler(roleCode);
                    break;

                case "32A3-MD01":
                    WhetherJump(sAPTYP, costCenter, costCenter, applydate, dtDetail, roleCode, procurementType, sTTLAMT, sTTLRATE);

                    break;

                default:
                    break;
            }
            #endregion
        }

        private void WhetherJump(string sAPTYP, string sApplier, string costcenter, string applydate, DataTable dtDetail, string roleCode, string procurementType, string sttlamt, string sTTLRATE)
        {
            bool jumpFlag = (CheckKPI(sAPTYP, costcenter, applydate, procurementType, dtDetail, sttlamt, sTTLRATE) == true) ? false : true;
            if (jumpFlag)
            {
                oDOAHandler._sJump = "Y";//不用簽，向后跳關
                oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
            }
            else
            {
                oDOAHandler._sJump = "N";
                oDOAHandler._sHandler = GetDOAHandler(roleCode);

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
        /// 
        /// </summary>
        /// <param name="apType"></param>
        /// <param name="area">區域</param>
        /// <param name="applyDate"></param>
        /// <param name="dtDetail"></param>
        /// <returns></returns>
        private bool CheckKPI(string apType, string costcenter, string applyDate, string procurementType, DataTable dtDetail, string sTTLAMT, string sTTLRATE)
        {
            bool flag = false;
            //取標準KPI設定值
            Dictionary<string, object> controlKPI = GetKPI(apType, costcenter, applyDate, procurementType);
            //未找到KPI設定值，不作檢查
            if (controlKPI["Exist"].ToString() == "N")
            {
                throw new Exception(GetQuarterByDate(Convert.ToDateTime(applyDate)) + " KPI未設定");
                return flag;
            }
         
            else
            {             
                    //循環檢查LINE DATA
                    double amount = 0;
                   // double atm=0;
                    //double number = 0;
                    foreach (DataRow dr in dtDetail.Rows)
                    {
                        //檢查是否有單張金額超標準KPI
                       
                        string overAmount = dr["STPRS"].ToString();  //单价
                       // string num = dr["MENGE"].ToString();//数量

                        //檢查金額是否超工單KPI
                       // string overAmt = dr["Over_AMT"].ToString();                        

                        if (overAmount.Length > 0)
                        {
                            //amount += double.Parse(overAmount);
                            amount = double.Parse(overAmount);
                           // atm=double.Parse(overAmt);
                           // number = double.Parse(num);
                            double kpi=double.Parse(controlKPI["OverAmount"].ToString());
                            //if (kpi <= amount || kpi<=atm )
                            if (kpi <= amount)
                            {                                
                                flag = true;
                                break;
                            }
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
            sql.Append("	t1.ProcurementType,t1.OverRate,t1.OverAmount ");           
            sql.Append(" FROM TB_GDS_DOA_KPI t1  ");           
            sql.Append("WHERE t1.APType=@APType AND t1.CostCenter=@CostCenter AND t1.ControlYear=@ControlYear ");
            sql.Append("AND t1.ControlQuarter=@ControlQuarter");
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

            }
            else
            {
                standardKPI["Exist"] = "N";
                standardKPI["OverRate"] = string.Empty;
                standardKPI["OverAmount"] = string.Empty;
              
            }
            return standardKPI;
        }

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

        protected string GetDOAHandler_Value3(string roleCode, string value1)
        {
            sql = "SELECT * FROM  TB_GDS_DOA_DETAIL with(nolock) WHERE roleCode = @RoleCode AND value1 = @value1  ";
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@RoleCode", SqlDbType.VarChar, roleCode));
            opc.Add(DataPara.CreateDataParameter("@value1", SqlDbType.VarChar, value1));
       
            return sdb.GetRowString(sql, opc, "VALUE3");
        }

        protected string GetDOACC(string purgroupid)
        {
            sql = "SELECT * FROM  TB_GDS_BuyerCode with(nolock) WHERE PUR_GROUPID = @PUR_GROUPID ";
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@PUR_GROUPID", SqlDbType.VarChar, purgroupid));

            return sdb.GetRowString(sql, opc, "USER_ID");
        }


        //Get NextCC List's UserID
        private string GetNextCCUserID(string list)
        {
            string _list = list.Replace(";", "','");
            _list = "'" + _list + "'";

            string sql = "SELECT UserID FROM [User]  with(nolock)  WHERE LogonID in ( " + _list + ")";
            opc.Clear();
            //opc.Add(DataPara.CreateDataParameter("@List", SqlDbType.VarChar, _list ));
            DataTable dt = sdb.GetDataTable(sql, opc);
            _list = "";
            foreach (DataRow drItem in dt.Rows)
            {
                if (_list.Length == 0)
                    _list = drItem["UserID"].ToString();
                else
                    _list = _list + "," + drItem["UserID"].ToString();
            }
            return _list;

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

            ht.Add("M_GRUND", "(見明細欄)");//原因
            ht.Add("M_DESC", "(共用欄，IMG不使用)");//中文描述
            ht.Add("M_TOTALSTPRS", string.Format("{0:N4}", dtDetail.Compute("sum(STPRS)", "")).TrimEnd('0').TrimEnd('.'));//總金額(￥)          

            StringBuilder sbMobile = new StringBuilder();

            for (int i = 0; i < dtDetail.Rows.Count; i++)
            {
                DataRow drDetail = dtDetail.Rows[i];
                sbMobile.AppendLine(string.Format("{0} {1} {2}{3} {4}",
                        Convert.ToString(i + 1),
                        drDetail["MATNR"].ToString().Trim(),//料號
                        drDetail["MENGE"].ToString().Trim(),//數量
                        drDetail["MEINS"].ToString().Trim(),//單位
                        drDetail["REFNO"].ToString().Trim()//原因
                ));
            }
            ht.Add("M_DETAIL", sbMobile.ToString());

            return ht;
        }
    }
}
