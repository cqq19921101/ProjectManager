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
    /// plant code NZ01,NZ08
    /// </summary>
    public class DOA_CZ_NA : DOA_Standard
    {

        public DOA_CZ_NA()
        {
        }

        protected override void GetDOABySpeicalRule(string roleCode, DataTable dtHead, DataTable dtDetail)
        {
            string sAPTYP = string.Empty;
            string sLOC = string.Empty;
            string buyerCode = string.Empty;
            string sTemp = string.Empty;
            DataRow drHead = dtHead.Rows[0];

            string sPlant = drHead["WERKF"].ToString().ToUpper().Trim(); //plantCode
            string sCostCenter = drHead["KOSTL"].ToString().ToUpper().Trim();//成本中心
            string sDepartment = drHead["ABTEI"].ToString().ToUpper().Trim();//部門
            //看最後兩碼 FT =成品            
            //其他物料
            string materialType = dtDetail.Rows[0]["MTART"].ToString().Trim().ToUpper();

            string orderType = string.Empty;
            bool jumpFlag = false;//跳關標示
            sAPTYP = drHead["APTYP"].ToString();//AP TYPE
            sLOC = drHead["LOCFM"].ToString();//儲位

            switch (roleCode.ToUpper())
            {
                #region NZ01
                case "NZ01-DEPT00":
                    switch (sAPTYP)
                    {
                        case "I5":
                            oDOAHandler._sHandler = GetDOAHandler_NA2(roleCode, sCostCenter, sDepartment);
                            oDOAHandler._sJump = "N";
                            break;
                        default:
                            break;
                    }
                    break;
                case "NZ01-DEPT01":
                    switch (sAPTYP)
                    {
                        case "I1":
                        case "IA":
                        case "TD":
                            oDOAHandler._sHandler = GetDOAHandler_NA(roleCode, sCostCenter, "");
                            oDOAHandler._sJump = "N";
                            break;
                        default:
                            break;
                    }
                    break;
                case "NZ01-DEPT02":
                    oDOAHandler._sHandler = GetDOAHandler_NA2(roleCode, sCostCenter, sDepartment);
                    oDOAHandler._sJump = "N";
                    break;
                case "NZ01-PMC01":
                    #region
                    switch (sAPTYP)
                    {

                        case "I1":
                            if (materialType.Length == 4 && materialType.Substring(2, 2) == "FT")
                            {

                                oDOAHandler._sHandler = GetPMCHandler("FT");
                            }
                            else
                            {
                                oDOAHandler._sHandler = GetPMCHandler("RH");
                            }
                            oDOAHandler._sJump = "N";
                            break;
                        default:
                            break;
                    }
                    #endregion
                    break;
                case "NZ01-SQE01":
                    #region[IA SQE NZ01 0051 簽 ]
                    switch (sAPTYP)
                    {
                        case "IA":
                            if (sPlant == "NZ01" && sLOC == "0051")
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
                    if (jumpFlag)
                    {
                        DOAJump();
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                    }

                    #endregion
                    break;
                case "NZ01-MC01":
                    #region
                    switch (sAPTYP)
                    {
                        case "IA":
                            //原物料簽核
                            if (materialType.Length == 4 && materialType.Substring(2, 2) == "FT")
                            {
                                jumpFlag = true;
                            }
                            else
                            {
                                jumpFlag = false;
                            }
                            break;
                        case "TD":
                            jumpFlag = false;
                            break;
                        default:
                            jumpFlag = true;
                            break;
                    }
                    if (jumpFlag)
                    {
                        DOAJump();
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                    }
                    #endregion
                    break;
                case "NZ01-PUR01":
                    #region
                    switch (sAPTYP)
                    {
                        case "IA":
                            //原物料簽核
                            if (materialType.Length == 4 && materialType.Substring(2, 2) == "FT")
                            {
                                jumpFlag = true;
                            }
                            else
                            {
                                jumpFlag = false;
                            }
                            break;
                        case "TD":
                            jumpFlag = false;
                            break;
                        default:
                            jumpFlag = true;
                            break;
                    }
                    if (jumpFlag)
                    {
                        DOAJump();
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                    }
                    #endregion
                    break;
                case "NZ01-PC02":
                    #region
                    switch (sAPTYP)
                    {
                        case "IA":
                            if (materialType.Length == 4 && materialType.Substring(2, 2) == "FT")
                            {
                                jumpFlag = false;
                            }
                            else
                            {
                                jumpFlag = true;
                            }
                            break;
                        case "I5":
                            jumpFlag = false;
                            break;
                        default:
                            jumpFlag = true;
                            break;
                    }

                    if (jumpFlag)
                    {
                        DOAJump();
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                    }
                    #endregion
                    break;
                case "NZ01-PMM01":
                    #region
                    switch (sAPTYP)
                    {
                        case "I1":
                        case "IA":
                            switch (sCostCenter)
                            {
                                case "N480":
                                case "N483":
                                    jumpFlag = true;
                                    break;
                                default:
                                    jumpFlag = false;
                                    break;
                            }
                            break;
                        case "TD":
                            jumpFlag = false;
                            break;
                        case "I5":
                            jumpFlag = false;
                            break;
                        default:
                            jumpFlag = false;
                            break;
                    }
                    if (jumpFlag)
                    {
                        DOAJump();
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                    }
                    #endregion
                    break;
                case "NZ01-MD01":
                    #region [NZ51 MD關卡 0072/0011須簽核MD]
                    switch (sAPTYP)
                    {
                        case "IA":
                            //檢查是否成品
                            if (materialType.Length == 4 && materialType.Substring(2, 2) == "FT")
                            {
                                if ((sPlant == "NZ51") && (sLOC == "0072" || sLOC == "0011"))
                                {
                                    jumpFlag = false;
                                }
                                else
                                {
                                    jumpFlag = true;
                                }
                            }
                            else
                            {
                                jumpFlag = true;
                            }

                            break;
                        case "I5":
                            jumpFlag = false;
                            break;
                        default:
                            jumpFlag = true;
                            break;
                    }
                    if (jumpFlag)
                    {
                        DOAJump();
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                    }
                    #endregion
                    break;
                case "NZ01-MD02":
                    #region 根據金額簽核到廠長 大於等於2萬
                    double TWD01 = GetAmount(dtDetail);//匯率
                    //double TWD01 = 100000;
                    if (TWD01 >= 20000)
                    {
                        jumpFlag = false;
                    }
                    else
                    {
                        jumpFlag = true;
                    }

                    if (jumpFlag)
                    {
                        DOAJump();
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                    }
                    break;
                    #endregion
                case "NZ01-BUHEAD":
                    #region 根據金額簽核到BUHEAD 大於等於10萬
                    double TWD00 = GetAmount(dtDetail);//匯率
                    //double TWD00 = 100000;
                    if (TWD00 >= 100000)
                    {
                        jumpFlag = false;
                    }
                    else
                    {
                        jumpFlag = true;
                    }

                    if (jumpFlag)
                    {
                        DOAJump();
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                    }
                    break;
                    #endregion
                case "NZ01-BGHEAD":
                    #region 根据金额判断是否需要签核至BGUHEAD  大於等於25萬
                    double TWD02 = GetAmount(dtDetail);//匯率
                    //double TWD02 = 250001;
                    if (TWD02 >= 250000)
                    {
                        jumpFlag = false;
                    }
                    else
                    {
                        jumpFlag = true;
                    }

                    if (jumpFlag)
                    {
                        DOAJump();
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                    }
                    break;
                    #endregion
                case "NZ01-CEO":
                    #region 根据金额判断是否需要签核至CEO   大於等於500萬
                    double TWD03 = GetAmount(dtDetail);//匯率
                    //double TWD03 = 600000;
                    if (TWD03 >= 5000000)
                    {
                        jumpFlag = false;
                    }
                    else
                    {
                        jumpFlag = true;
                    }

                    if (jumpFlag)
                    {
                        DOAJump();
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                    }
                    break;
                    #endregion
                case "NZ01-CHAIRMAN":
                    #region 根据金额判断是否需要签核至董事長 大於等於1000萬
                    double TWD04 = GetAmount(dtDetail);//匯率
                    //double TWD04 = 100000000;
                    if (TWD04 > 10000000)
                    {
                        jumpFlag = false;
                    }
                    else
                    {
                        jumpFlag = true;
                    }

                    if (jumpFlag)
                    {
                        DOAJump();
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                    }
                    break;
                    #endregion

                #endregion 

                #region NZ08
                case "NZ08-DEPT01":
                case "NZ08-DEPT02":
                    #region
                    switch (sAPTYP)
                    {
                        case "I1":
                            jumpFlag = false;
                            break;
                        case "IA":
                            switch (sCostCenter)
                            {
                                case "ICM470":
                                    jumpFlag = true;
                                    break;
                                default:
                                    jumpFlag = false;
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                    if (jumpFlag)
                    {
                        DOAJump();
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler_NA(roleCode, sCostCenter, "");
                    }
                    break;
                    #endregion

                case "NZ08-PMM01":
                case "NZ08-WH01":
                    #region
                    switch (sAPTYP)
                    {
                        case "I1":
                            jumpFlag = false;
                            break;
                        case "IA":
                            switch (sCostCenter)
                            {
                                case "ICM470":
                                    jumpFlag = true;
                                    break;
                                default:
                                    jumpFlag = false;
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                    if (jumpFlag)
                    {
                        DOAJump();
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                    }

                    break;
                    #endregion
                #endregion

                #region NZ06
                case "NZ06-GA":
                    oDOAHandler._sHandler = GetDOAHandler_NA06(roleCode);//NZ06-GA 修改簽核人抓取方式 2019-06-03
                    oDOAHandler._sJump = "N";
                    break;
                case "NZ06-DEPT01":
                case "NZ06-DEPT02":
                    #region
                    switch (sAPTYP)
                    {
                        case "I1":
                            oDOAHandler._sHandler = GetDOAHandler_NA(roleCode, sCostCenter, "");
                            oDOAHandler._sJump = "N";
                            break;
                        default:
                            break;
                    }

                    break;
                    #endregion
                #endregion
                default:
                    break;

            }



        }

        /// <summary>
        /// 重載-檢查是否並簽--用于任意簽核
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
                        case "NZ01-SQE01":
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
        /// BY CC,DEPT獲取簽核人 
        /// </summary>
        /// <param name="roleCode">關卡名</param>
        /// <param name="value1">CC</param>
        /// <param name="value2">DEPT</param>
        /// <returns>Handler</returns>
        protected string GetDOAHandler_NA(string roleCode, string value1, string value2)
        {
            sql = "SELECT * FROM  TB_GDS_DOA_DETAIL with(nolock) WHERE RoleCode = @RoleCode AND Value1 = @value1";
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@RoleCode", SqlDbType.VarChar, roleCode));
            opc.Add(DataPara.CreateDataParameter("@Value1", SqlDbType.VarChar, value1));
            if (value2.Length > 0)
            {
                sql += " AND Value2 = @value2 ";
                opc.Add(DataPara.CreateDataParameter("@Value2", SqlDbType.VarChar, value2));
                return sdb.GetRowString(sql, opc, "VALUE3");
            }
            return sdb.GetRowString(sql, opc, "VALUE2");
        }

        /// <summary>
        /// BY CC,DEPT獲取簽核人 
        /// </summary>
        /// <param name="roleCode">關卡名</param>
        /// <param name="value1">CC</param>
        /// <param name="value2">DEPT</param>
        /// <returns>Handler</returns>
        protected string GetDOAHandler_NA2(string roleCode, string value1, string value2)
        {
            sql = "SELECT * FROM  TB_GDS_DOA_DETAIL with(nolock) WHERE roleCode = @RoleCode AND value1 = @value1";
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@RoleCode", SqlDbType.VarChar, roleCode));
            opc.Add(DataPara.CreateDataParameter("@value1", SqlDbType.VarChar, value1));
            if (value2.Length > 0)
            {
                sql += " AND value2 = @value2 ";
                opc.Add(DataPara.CreateDataParameter("@value2", SqlDbType.VarChar, value2));
                return sdb.GetRowString(sql, opc, "VALUE3");
            }
            return sdb.GetRowString(sql, opc, "VALUE3");
        }

        /// <summary>
        /// BY CC,DEPT獲取簽核人 
        /// </summary>
        /// <param name="roleCode">關卡名</param>
        /// <param name="value1">CC</param>
        /// <param name="value2">DEPT</param>
        /// <returns>Handler</returns>
        protected string GetDOAHandler_NA06(string roleCode)
        {
            sql = "SELECT * FROM  TB_GDS_DOA_DETAIL with(nolock) WHERE RoleCode = @RoleCode ";
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@RoleCode", SqlDbType.VarChar, roleCode));
            return sdb.GetRowString(sql, opc, "VALUE1");
        }


        /// <summary>
        /// 更據成品/物料獲取PMC簽核人
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetPMCHandler(string type)
        {
            sql = "SELECT * FROM TB_GDS_DOA_DETAIL WHERE RoleCode=@RoleCode";
            string roleCode = string.Empty;
            if (type == "FT")
            {
                roleCode = "NZ01-PC02";
            }
            else
            {
                roleCode = "NZ01-MC01";
            }
            return GetDOAHandler(roleCode);
        }

        private double GetAmount(DataTable dtDetail)
        {
            StringBuilder sbRate = new StringBuilder();
            sbRate.Append(@"select Top 1 UKURS from TB_ExchangeRate
                                            where FCURR = 'CNY' and TOCURR = 'TWD'
                                            order by UPDATE_TIME desc");
            opc.Clear();
            DataTable dtr = sdb.GetDataTable(sbRate.ToString(), opc);
            double rates = double.Parse(dtr.Rows[0]["UKURS"].ToString());

            double amount = 0;
            foreach (DataRow dr in dtDetail.Rows)
            {
                amount += double.Parse(dr["STPRS"].ToString());
            }
            double total = amount;
            double TWD = total * rates;

            return TWD;
        }

    }
}
