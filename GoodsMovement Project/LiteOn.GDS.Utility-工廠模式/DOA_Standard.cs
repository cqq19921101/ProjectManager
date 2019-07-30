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
    public class DOA_Standard : IDOA
    {
        protected Model_DOAHandler oDOAHandler = new Model_DOAHandler();
        protected ArrayList opc = new ArrayList();
        protected string sql = string.Empty;
        protected SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("SPM"));

        public DOA_Standard()
        {
        }

        /// <summary>
        /// 獲取DOA簽核人
        /// </summary>
        /// <param name="sLogonId">申請人</param>
        /// <param name="sDOA">DOA資訊</param>
        /// <param name="dtHead">單據表頭</param>
        /// <param name="dtDetail">單據DATA</param>
        /// <returns></returns>
        public Model_DOAHandler GetStepHandler(Model_DOAHandler pDOAHandler, DataTable dtHead, DataTable dtDetail)
        {
            oDOAHandler = pDOAHandler;
            //獲取當前簽核角色
            string currentRole = SPMAppLine.GetCurrentApprover(oDOAHandler._sDOA).Replace("{", "").Replace("}", "");
          
            //並簽檢查
            ParallelApprovalCheck(oDOAHandler, currentRole);
            oDOAHandler._sRoleCode = currentRole;
            //檢查是否最后一關簽核角色
            string nextRole = SPMAppLine.GetNextStep(oDOAHandler._sDOA);
            if (nextRole != "*")
            {
                //當前非最後一關簽核角色
                //檢查當前簽核角色是否並簽，是否最後一個並簽人員
                if (oDOAHandler._ParallelFlag == true && oDOAHandler._FinalFlag == false)
                {
                    //並簽,非最後一個人,將關卡打入下一關即END，觸發SPM並簽控制機制
                    oDOAHandler._sEndFlag = "Y";
                    //設置更新並簽FLAG為TRUE
                    oDOAHandler._UpdateParallelFlag = true;
                }
                else
                {

                    //非並簽或並簽且為最後一個人
                    //對DOA XML進行跳關處理
                    try
                    {
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    catch(Exception ex)
                    {
                        DBIO.RecordTraceLog("E", ex.Message, oDOAHandler);
                        throw new Exception("Server or network is busy now,pls try again");
                    }
                    do
                    {
                        //獲取當前簽核角色
                        currentRole = SPMAppLine.GetCurrentApprover(oDOAHandler._sDOA).Replace("{", "").Replace("}", "");
                        //並簽檢查
                        ParallelApprovalCheck(oDOAHandler, currentRole);
                        //CAL FUNCTION 抓取關卡簽核人直至該關卡非跳關狀態
                        StepCheck(dtHead, dtDetail);

                        //防止最後一個簽核角色為選簽時，前置條件不足陷於死循環
                        string currentStep = SPMAppLine.GetCurrentStep(oDOAHandler._sDOA);
                        if (currentStep == "*")
                        {
                            oDOAHandler._sJump = "N";
                            oDOAHandler._sEndFlag = "Y";
                        }
                    }
                    while (oDOAHandler._sJump == "Y");
                }

            }
            else
            {
                //當前為最後一關簽核角色
                //檢查當前簽核角色是否並簽
                if (oDOAHandler._ParallelFlag == true)
                {
                    //並簽,且為最後一個人
                    if (oDOAHandler._FinalFlag == true)
                    {
                        //對DOA XML進行跳關處理
                        oDOAHandler._sEndFlag = "Y";
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
                        //並簽,非最後一個人,將關卡打入下一關即END，觸發SPM並簽控制機制
                        oDOAHandler._sEndFlag = "Y";
                    }
                    //設置更新並簽FLAG為TRUE
                    oDOAHandler._UpdateParallelFlag = true;

                }
                else
                {
                    //非並簽,對DOA XML進行跳關處理
                    oDOAHandler._sEndFlag = "Y"; 
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

            }
            return oDOAHandler;
        }

        /// <summary>
        /// 起單時獲取第一個簽核人獲取DOA簽核人
        /// </summary>
        /// <param name="sLogonId">申請人</param>
        /// <param name="sDOA">DOA資訊</param>
        /// <param name="dtHead">單據表頭</param>
        /// <param name="dtDetail">單據DATA</param>
        /// <returns></returns>
        public Model_DOAHandler GetStepHandler_StartForm(Model_DOAHandler pDOAHandler, DataTable dtHead, DataTable dtDetail)
        {
            oDOAHandler = pDOAHandler;
            oDOAHandler._ParallelFlag = false;
            oDOAHandler._FinalFlag = false;
            do
            {
                StepCheck(dtHead, dtDetail);//CAL FUNCTION 抓取關卡簽核人直至該關卡非跳關狀態
            }
            while (oDOAHandler._sJump == "Y");

            return oDOAHandler;
        }

        /// <summary>
        /// 取行動裝置簽核欄位
        /// </summary>
        /// <param name="dtHead"></param>
        /// <param name="dtDetail"></param>
        /// <returns></returns>
        public virtual Hashtable GetMobileFormFields(DataTable dtHead, DataTable dtDetail)
        {
            Hashtable ht = new Hashtable();

            return ht;
        }

        /// <summary>
        /// DOA簽核角色檢查
        /// </summary>
        /// <param name="dtHead">單據表頭</param>
        /// <param name="dtDetail">單據DATA</param>
        protected void StepCheck(DataTable dtHead, DataTable dtDetail)
        {
            //獲取當前簽核角色
            string curRole = SPMAppLine.GetCurrentApprover(oDOAHandler._sDOA);
            string roleCode = curRole.Replace("{", "").Replace("}", "");
            oDOAHandler._sRoleCode = roleCode;
            //獲取當前簽核角色 DOA設定信息
            Model_DOAOption DOAOption = GetDOAOption(roleCode);
            if (DOAOption._bExist)
            {
                //依DOA設定信息獲取 簽核人
                oDOAHandler._sJump = DOAOption._sJumpOption;
                string buyerCode = string.Empty;
                string pdLine = string.Empty;
                switch (DOAOption._sCheckOption)
                {
                    case "A"://BY固定簽核人
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                        break;
                    case "B"://BY LINE找固定簽核人
                        
                        pdLine = dtHead.Rows[0]["ZPLINE"].ToString().Trim().ToUpper();
                        if (pdLine.Length > 0)
                        {
                            oDOAHandler._sHandler = GetDOAHandler(roleCode, pdLine);
                        }
                        else
                        {
                            GetHandlerErrAlarm(dtHead, "線體信息為空，無法獲取簽核人");
                        }
                        break;
                    case "C"://HARD CODE
                        GetDOABySpeicalRule(roleCode, dtHead, dtDetail);
                        break;
                    case "D"://by buyer code 找固定簽核人
                        buyerCode = dtDetail.Rows[0]["EKGRP"].ToString().Trim().ToUpper();
                        if (buyerCode.Length > 0)
                        {
                            oDOAHandler._sHandler = GetDOAHandlerByBuyerCode(buyerCode);
                        }
                        else
                        {
                            GetHandlerErrAlarm(dtHead, "物料BUYER CODE缺失，無法獲取簽核人");
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                GetHandlerErrAlarm(dtHead, "Role信息缺失，無法獲取簽核人，請聯繫IT");
            }
        }

        #region [DOA standard Methord]
        /// <summary>
        /// 標準DOA-獲取固定簽核人(TYPE A)
        /// </summary>
        /// <param name="roleCode">關卡名</param>
        /// <returns>Handler</returns>
        protected string GetDOAHandler(string roleCode)
        {
            sql = "SELECT * FROM TB_GDS_DOA_DETAIL  with(nolock) WHERE RoleCode = @RoleCode";
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@RoleCode", SqlDbType.VarChar, roleCode));
            return sdb.GetRowString(sql, opc, "VALUE1");
        }

        /// <summary>
        /// 標準DOA-BY LINE/cc獲取簽核人(TYPE B)
        /// </summary>
        /// <param name="roleCode">關卡名</param>
        /// <param name="pdline">LINE</param>
        /// <returns>Handler</returns>
        protected string GetDOAHandler(string roleCode, string value1)
        {
            sql = "SELECT * FROM  TB_GDS_DOA_DETAIL with(nolock) WHERE RoleCode = @RoleCode AND Value1 = @Value1";
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@RoleCode", SqlDbType.VarChar, roleCode));
            opc.Add(DataPara.CreateDataParameter("@Value1", SqlDbType.VarChar, value1));
            return sdb.GetRowString(sql, opc, "VALUE2");
        }


        /// <summary>
        /// 標準DOA-特殊規則獲取簽核人(TYPE C)
        /// </summary>
        /// <param name="roleCode"></param>
        /// <param name="dtHead"></param>
        /// <param name="dtDetail"></param>
        protected virtual void GetDOABySpeicalRule(string roleCode, DataTable dtHead, DataTable dtDetail)
        {
        }

        /// <summary>
        /// 標準DOA-依PUR GROUP 獲取簽核窗口(TYPE D)
        /// </summary>
        /// <param name="buyerCode"></param>
        /// <returns></returns>
        protected string GetDOAHandlerByBuyerCode(string buyerCode)
        {
            sql = "SELECT * FROM TB_GDS_BuyerCode  with(nolock) WHERE PUR_GROUPID = @BuyerCode";
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@BuyerCode", SqlDbType.VarChar, buyerCode));
            return sdb.GetRowString(sql, opc, "USER_ID");
        }


        #endregion [DOA standard Method]

        /// <summary>
        /// 檢查是否並簽
        /// </summary>
        /// <param name="caseId"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        protected virtual void ParallelApprovalCheck(Model_DOAHandler DOAHandler, string roleCode)
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
                    if (totalCount - 1 > actualcount)
                    {
                        oDOAHandler._FinalFlag = false;
                    }
                    else
                    {
                        oDOAHandler._FinalFlag = true;
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
        /// 簽核人抓取異常郵件通知
        /// </summary>
        /// <param name="dtHead">表頭</param>
        /// <param name="handler">處理人</param>
        /// <param name="errReason">異常原因</param>
        protected void GetHandlerErrAlarm(DataTable dtHead, string errReason)
        {
            DataRow drHead = dtHead.Rows[0];
            string sLongonId = drHead["ERNAM"].ToString().Trim().Replace(" ", "");
            string sCaseno = drHead["MBLNR_A"].ToString().Trim();
            string plantCode = drHead["WERKS"].ToString().Trim();

            string sub = "[" + plantCode + "]GoodsMovement--獲取簽核人錯誤";
            string body = "Dear's \r\n";
            body += "      單號-" + sCaseno + ";申請人-" + sLongonId + "\r\n";
            body += errReason + "\r\n";
            SPMBasic SPMBasic_class = new SPMBasic();
            string applicant = SPMBasic_class.GetEMailByEName(sLongonId);
            SendMail mail = new SendMail();
            ArrayList to = new ArrayList();
            if (applicant.Length > 0) to.Add(applicant);
            ArrayList cc = new ArrayList();
            cc.Add(DBIO.GetITWindow(plantCode));
            mail.SendMail_Normal(to, cc, sub, body, false);
        }

        /// <summary>
        /// 獲取設定 DOA OPTION
        /// </summary>
        /// <param name="roleCode"></param>
        /// <returns></returns>
        protected Model_DOAOption GetDOAOption(string roleCode)
        {
            Model_DOAOption DOAOption = new Model_DOAOption();
            sql = "SELECT * FROM TB_GDS_DOA  with(nolock) WHERE RoleCode = @RoleCode";
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@RoleCode", SqlDbType.VarChar, roleCode));
            DataTable dt = sdb.GetDataTable(sql, opc);
            DOAOption._sRoleCode = roleCode;
            if (dt.Rows.Count > 0)
            {
                DOAOption._bExist = true;
                DataRow dr = dt.Rows[0];
                DOAOption._sJumpOption = dr["JumpOption"].ToString();
                DOAOption._sCheckOption = dr["CheckOption"].ToString();
            }
            return DOAOption;
        }

        /// <summary>
        /// 控制關卡向後跳關
        /// </summary>
        /// <param name="jumpFlag"></param>
        protected virtual void DOAJump()
        {
            oDOAHandler._sJump = "Y";//設置跳關標誌
            try
            {
                oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);//DOA XML STR進行跳關處理
            }
            catch (Exception ex)
            {
                DBIO.RecordTraceLog("E", ex.Message, oDOAHandler);
                throw new Exception("Server or network is busy now,pls try again");
            }
        }

    }
}
