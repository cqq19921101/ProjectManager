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
    /// plant code 2670,267s
    /// </summary>
    public class DOA_CZ_HIS : DOA_Standard
    {

        public DOA_CZ_HIS()
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
            #region
            bool jumpFlag = false;//跳關標示
            string buyerCode = string.Empty;
            DataRow drHead = dtHead.Rows[0];
            string sAPTYP = drHead["APTYP"].ToString();
            string sLOCFrom = drHead["LOCFM"].ToString();
            string sLOCTo = drHead["LOCTO"].ToString();
            string sCostCenter = drHead["KOSTL"].ToString().ToUpper().Trim();
            string sDepartment = drHead["ABTEI"].ToString().ToUpper().Trim();
            string materialType = string.Empty;
            switch (roleCode)
            {

                case "2670-PMC01":
                    #region
                    switch (sAPTYP)
                    {
                        case "T1":
                            oDOAHandler._sJump = CheckJump(sLOCFrom, sLOCTo, roleCode, sAPTYP) == true ? "Y" : "N";
                            if (oDOAHandler._sJump == "Y")
                            {
                                oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                            }
                            else
                            {
                                buyerCode = dtDetail.Rows[0]["EKGRP"].ToString();
                                //儲位0010簽MC
                                if (sLOCTo == "0019" && (sLOCFrom == "0030" || sLOCFrom == "0020" || sLOCFrom == "001A"))
                                {
                                    //非儲位0030/0030 轉0019簽PC
                                    oDOAHandler._sHandler = GetDOAHandler(roleCode, "PC");
                                }

                                else
                                {
                                    //簽MC
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
                                }
                            }
                            break;
                        case "I1":
                            //material storage location
                            Hashtable dtSLoc = new Hashtable();
                            dtSLoc.Add("0010", "0010");
                            dtSLoc.Add("0013", "0013");
                            dtSLoc.Add("0012", "0012");
                            dtSLoc.Add("0019", "0019");
                            dtSLoc.Add("SHJ1", "SHJ1");
                            //sepcial storage location(include material/fg/ft)
                            Hashtable dtSLoc2 = new Hashtable();
                            dtSLoc2.Add("0024", "0024");
                            dtSLoc2.Add("0028", "0028");
                          
                            if (dtSLoc.Contains(sLOCFrom))
                            {
                                Hashtable ht = new Hashtable();
                                //檢查每顆料的採購，如有多個則進行並簽 
                                foreach (DataRow dr in dtDetail.Rows)
                                {
                                    buyerCode = dr["EKGRP"].ToString().Trim().ToUpper();
                                    string logonId = GetDOAHandlerByBuyerCode(buyerCode);
                                    if (ht.Contains(logonId) == false)
                                    {
                                        ht.Add(logonId, 0);
                                        if (oDOAHandler._sHandler.Length > 0)
                                        {
                                            oDOAHandler._sHandler += ",";
                                        }
                                        oDOAHandler._sHandler += logonId;
                                        break;
                                    }

                                }
                                if (oDOAHandler._sHandler.Length == 0)
                                {
                                    GetHandlerErrAlarm(dtHead, "物料對應BuyerCode信息有誤，無法獲取簽核人");
                                }
                            }
                            else if (dtSLoc2.Contains(sLOCFrom))
                            {
                                materialType = dtDetail.Rows[0]["MTART"].ToString().Trim().ToUpper();
                                //看最後兩碼 FT =成品
                                //RH = 材料
                                //HL = 半品
                                //VP = 包材
                                if (materialType.Length == 4 && materialType.Substring(2, 2) == "RH")
                                {
                                    buyerCode = dtDetail.Rows[0]["EKGRP"].ToString().Trim().ToUpper();
                                    string logonId =
                                    oDOAHandler._sJump = "N";
                                    //簽MC
                                    oDOAHandler._sHandler = GetDOAHandlerByBuyerCode(buyerCode);
                                }
                                else
                                {
                                    oDOAHandler._sJump = "N";
                                    //簽PC
                                    oDOAHandler._sHandler = GetDOAHandler(roleCode, "PC");
                                }
                            }
                            else
                            {
                                //簽PC
                                oDOAHandler._sHandler = GetDOAHandler(roleCode, "PC");
                            }
                            break;
                        case "IA":
                         
                                materialType = dtDetail.Rows[0]["MTART"].ToString().Trim().ToUpper();
                                //看最後兩碼 FT =成品
                                //RH = 材料
                                //HL = 半品
                                //VP = 包材
                                if (materialType.Length == 4 && materialType.Substring(2, 2) == "RH")
                                {
                                    buyerCode = dtDetail.Rows[0]["EKGRP"].ToString().Trim().ToUpper();
                                    string logonId =
                                    oDOAHandler._sJump = "N";
                                    //簽MC
                                    oDOAHandler._sHandler = GetDOAHandlerByBuyerCode(buyerCode);
                                }
                                else
                                {
                                    oDOAHandler._sJump = "N";
                                    //簽PC
                                    oDOAHandler._sHandler = GetDOAHandler(roleCode, "PC");
                                }
                            break;
                    }
                    break;
                    #endregion
                case "2670-MFG02":
              
                    #region
                    switch (sAPTYP)
                    {
                        case "T1":
                            oDOAHandler._sJump = CheckJump(sLOCFrom, sLOCTo, roleCode, sAPTYP) == true ? "Y" : "N";
                            if (oDOAHandler._sJump == "Y")
                            {
                                oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                            }
                            else
                            {
                                oDOAHandler._sHandler = GetDOAHandler(roleCode, "A");//抓取簽核人空白 Bug 修復 
                            }
                            break;
                        default://應稽核要求 7302N成本中心 I3,I5單第一關簽核人 單獨指定 2019-05-30
                            if (sCostCenter == "7302N")
                            {
                                oDOAHandler._sHandler = GetDOAHandler(roleCode, sCostCenter);
                            }
                            else
                            {
                                oDOAHandler._sHandler = GetDOAHandler(roleCode, "A");
                            }
                            break;
                    }
                    break;
                    #endregion
                case "2670-PMC02":
                
                    #region
                    switch (sAPTYP)
                    {
                        case "T1":
                            //儲位0010簽MC主管
                            if (sLOCFrom == "0010")
                            {
                                oDOAHandler._sHandler = GetDOAHandler(roleCode, "MC");
                                break;
                            }
                            else
                            {
                                //非儲位0010簽PC主管
                                oDOAHandler._sHandler = GetDOAHandler(roleCode, "PC");
                            }
                            break;
                        case "IS":
                        case "I1":
                            //material storage location
                            Hashtable dtSLoc = new Hashtable();
                            dtSLoc.Add("0010", "0010");
                            dtSLoc.Add("0013", "0013");
                            dtSLoc.Add("0019", "0019");
                            dtSLoc.Add("SHJ1", "SHJ1");
                            dtSLoc.Add("0012", "0012");
                            //sepcial storage location(include material/fg/ft)
                            Hashtable dtSLoc2 = new Hashtable();
                            dtSLoc2.Add("0024", "0024");
                            dtSLoc2.Add("0028", "0028");
                            if (dtSLoc.Contains(sLOCFrom))
                            {
                                if (sCostCenter == "7406N" && sDepartment == "MC")
                                {
                                    //本部門申請跳過
                                    oDOAHandler._sJump = "Y";
                                    oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                                }
                                else
                                {
                                    oDOAHandler._sJump = "N";
                                    //簽MC
                                    oDOAHandler._sHandler = GetDOAHandler(roleCode, "MC");
                                }

                            }
                            else if (dtSLoc2.Contains(sLOCFrom))
                            {
                                materialType = dtDetail.Rows[0]["MTART"].ToString().Trim().ToUpper();
                                //看最後兩碼 FT =成品
                                //RH = 材料
                                //HL = 半品
                                //VP = 包材
                                if (materialType.Length == 4 && materialType.Substring(2, 2) == "RH")
                                {
                                    if (sCostCenter == "7406N" && sDepartment == "MC")
                                    {
                                        //本部門申請跳過
                                        oDOAHandler._sJump = "Y";
                                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                                    }
                                    else
                                    {
                                        oDOAHandler._sJump = "N";
                                        //簽MC
                                        oDOAHandler._sHandler = GetDOAHandler(roleCode, "MC");
                                    }
                                }
                                else
                                {
                                    if (sCostCenter == "7406N" && sDepartment == "PC")
                                    {
                                        //本部門申請跳過
                                        oDOAHandler._sJump = "Y";
                                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                                    }
                                    else
                                    {
                                        oDOAHandler._sJump = "N";
                                        //簽PC
                                        oDOAHandler._sHandler = GetDOAHandler(roleCode, "PC");
                                    }
                                }
                            }
                            else
                            {
                                if (sCostCenter == "7406N" && sDepartment == "PC")
                                {
                                    //本部門申請跳過
                                    oDOAHandler._sJump = "Y";
                                    oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                                }
                                else
                                {
                                    oDOAHandler._sJump = "N";
                                    //簽PC
                                    oDOAHandler._sHandler = GetDOAHandler(roleCode, "PC");
                                }

                            }
                            break;

                        case "I6":
                            materialType = dtDetail.Rows[0]["MTART"].ToString().Trim().ToUpper();
                            //看最後兩碼 FT =成品
                            //RH = 材料
                            //HL = 半品
                            //VP = 包材
                            if (materialType.Length == 4 && materialType.Substring(2, 2) == "RH")
                            {
                                if (sCostCenter == "7406N" && sDepartment == "MC")
                                {
                                    //本部門申請跳過
                                    oDOAHandler._sJump = "Y";
                                    oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                                }
                                else
                                {
                                    oDOAHandler._sJump = "N";
                                    //簽MC
                                    oDOAHandler._sHandler = GetDOAHandler(roleCode, "MC");
                                }
                            }
                            else
                            {
                                if (sCostCenter == "7406N" && sDepartment == "PC")
                                {
                                    //本部門申請跳過
                                    oDOAHandler._sJump = "Y";
                                    oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                                }
                                else
                                {
                                    oDOAHandler._sJump = "N";
                                    //簽PC
                                    oDOAHandler._sHandler = GetDOAHandler(roleCode, "PC");
                                }
                            }
                            break;
                        case "IA":
                             materialType = dtDetail.Rows[0]["MTART"].ToString().Trim().ToUpper();
                                //看最後兩碼 FT =成品
                                //RH = 材料
                                //HL = 半品
                                //VP = 包材
                                if (materialType.Length == 4 && materialType.Substring(2, 2) == "RH")
                                {
                                    buyerCode = dtDetail.Rows[0]["EKGRP"].ToString().Trim().ToUpper();
                                    string logonId =
                                    oDOAHandler._sJump = "N";
                                    //簽MC
                                    oDOAHandler._sHandler = GetDOAHandlerByBuyerCode(buyerCode);
                                }
                                else
                                {
                                    oDOAHandler._sJump = "N";
                                    //簽PC
                                    oDOAHandler._sHandler = GetDOAHandler(roleCode, "PC");
                                }
                          
                            break;
                    }

                    break;
                    #endregion
                case "2670-DEPT01":
                
                    #region
                    oDOAHandler._sHandler = GetDOAHandler_HIS(roleCode, sCostCenter, sDepartment);
                    if (oDOAHandler._sHandler.Length == 0)
                    {
                        GetHandlerErrAlarm(dtHead, string.Format("CostCenter:{0},Department:{1} 信息有誤，無法獲取簽核人", sCostCenter, sDepartment));
                    }
                    break;
                    #endregion
                case "2670-DEPT02":
           
                    #region
                    oDOAHandler._sHandler = GetDOAHandler(roleCode, sCostCenter);
                    if (oDOAHandler._sHandler.Length == 0)
                    {
                        oDOAHandler._sJump = "Y";//PCT不用簽，主管與窗口為同一人向后跳關
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
                    }
                    break;
                    #endregion
                case "2670-APP01":
                
                    #region
                    oDOAHandler._sHandler = GetDOAHandler(roleCode, sCostCenter);
                    if (oDOAHandler._sHandler.Length == 0)
                    {
                        GetHandlerErrAlarm(dtHead, string.Format("CostCenter:{0},Department:{1} 信息有誤，無法獲取簽核人", sCostCenter, sDepartment));
                    }
                    break;
                    #endregion
                case "2670-FIN01":
              
                    #region
                    oDOAHandler._sHandler = GetDOAHandler(roleCode);
                    break;
                    #endregion
                case "2670-CUST01":
               
                    #region
                    switch (sAPTYP)
                    {
                        case "IS":
                            jumpFlag = false;
                            break;
                        case "I1":
                            string returnFlag = drHead["RTNIF"].ToString();
                            if (returnFlag == "N")
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
                    break;
                    #endregion
                default:
                    break;
                case "2670-DEPT01-T1":
             
                    #region //costcenter +dept

                    oDOAHandler._sJump = CheckJump(sLOCFrom, sLOCTo, roleCode, sAPTYP) == true ? "Y" : "N";
                    if (oDOAHandler._sJump == "Y")
                    {
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sHandler = GetDOAHandler_HIS(roleCode, sCostCenter, sDepartment);
                    }
                    break;


                    #endregion
                case "2670-DEPT02-T1":
        
                    #region   //costcenter

                    oDOAHandler._sJump = CheckJump(sLOCFrom, sLOCTo, roleCode, sAPTYP) == true ? "Y" : "N";
                    if (oDOAHandler._sJump == "Y")
                    {
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sHandler = GetDOAHandler(roleCode, sCostCenter);
                    }

                    break;
                    #endregion
              
                case "2670-PMC01-T1":
       
                    #region

                    oDOAHandler._sJump = CheckJump(sLOCFrom, sLOCTo, roleCode, sAPTYP) == true ? "Y" : "N";
                    if (oDOAHandler._sJump == "Y")
                    {
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        materialType = dtDetail.Rows[0]["MTART"].ToString().Trim().ToUpper();
                        //看最後兩碼 FT =成品
                        //RH = 材料
                        //HL = 半品
                        //VP = 包材
                        if (materialType.Length == 4 && materialType.Substring(2, 2) == "RH")
                        {

                            buyerCode = dtDetail.Rows[0]["EKGRP"].ToString().Trim().ToUpper();
                            if (buyerCode.Length > 0)
                            {
                                oDOAHandler._sHandler = GetDOAHandlerByBuyerCode(buyerCode);
                            }
                            else
                            {
                                GetHandlerErrAlarm(dtHead, "物料BUYER CODE缺失，無法獲取簽核人");
                            }
                        }
                        else
                        {
                            oDOAHandler._sHandler = GetDOAHandler(roleCode, "PC");

                        }
                    }
                    break;
                    #endregion
                case "2670-SQE01-T1":
             
                case "2670-IQE01-T1":
            
                    oDOAHandler._sJump = CheckJump(sLOCFrom, sLOCTo, roleCode, sAPTYP) == true ? "Y" : "N";
                    if (oDOAHandler._sJump == "Y")
                    {
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        buyerCode = dtDetail.Rows[0]["EKGRP"].ToString().Trim().ToUpper();
                        oDOAHandler._sHandler = GetDOAHandler(roleCode, buyerCode);
                    }
                    break;
                case "2670-QRA01-T1":
             
                    oDOAHandler._sJump = CheckJump(sLOCFrom, sLOCTo, roleCode, sAPTYP) == true ? "Y" : "N";
                    if (oDOAHandler._sJump == "Y")
                    {
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        materialType = dtDetail.Rows[0]["MTART"].ToString().Trim().ToUpper();
                        //看最後兩碼 FT =成品
                        //RH = 材料
                        //HL = 半品
                        //VP = 包材
                        if (materialType.Length == 4 && materialType.Substring(2, 2) == "RH")
                        {
                      
                            oDOAHandler._sHandler = GetDOAHandler(roleCode, "RH");
                        }
                        else
                        {
                            Hashtable dtSLoc = new Hashtable();
                            dtSLoc.Add("0023", "0023");
                            if(dtSLoc.Contains(sLOCFrom) || dtSLoc.Contains(sLOCTo))
                            {
                              oDOAHandler._sHandler = GetDOAHandler_HIS(roleCode, "HL", "A");
                            }
                            else if (sLOCFrom == "0024")
                            {
                                oDOAHandler._sHandler = GetDOAHandler(roleCode, "FT");
                            }
                            else
                            {
                                oDOAHandler._sHandler = GetDOAHandler_HIS(roleCode, "HL", "B");
                            }
                                
                        }
                    }
                    break;
                case "2670-MC01-I3" :
                    oDOAHandler._sJump = CheckJump(sLOCFrom, sLOCTo, roleCode, sAPTYP) == true ? "Y" : "N";
                    if (oDOAHandler._sJump == "Y")
                    {
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        materialType = dtDetail.Rows[0]["MTART"].ToString().Trim().ToUpper();
                        if (materialType.Length == 4 && materialType.Substring(2, 2) == "HL")
                        {
                            oDOAHandler._sHandler = GetDOAHandler(roleCode, "CZ1"); //半成品簽PC人員 CZ1 : Michael Liao 2019/03/06
                        }
                        else
                        {
                            buyerCode = dtDetail.Rows[0]["EKGRP"].ToString().Trim().ToUpper();
                            oDOAHandler._sHandler = GetDOAHandlerByBuyerCode(buyerCode);
                        }
                    }
                    break;
                case "2670-IQC01":
                    switch (sAPTYP)
                    {
                        case "IA":
                            materialType = dtDetail.Rows[0]["MTART"].ToString().Trim().ToUpper();
                                //看最後兩碼 FT =成品
                                //RH = 材料
                                //HL = 半品
                                //VP = 包材
                                if (materialType.Length == 4 && materialType.Substring(2, 2) == "FT")
                                {
                                    oDOAHandler._sHandler = GetDOAHandler(roleCode,"FT"); //簽成品負責人
                                }
                                else
                                {
                                    oDOAHandler._sHandler = GetDOAHandler(roleCode);//簽其他負責人
                                }
                                break;
                        case "IC":
                                oDOAHandler._sHandler = GetDOAHandler(roleCode);//簽其他負責人
                                break;

                    }
                    break;
                case "2670-MFG01-T1":
                case "2670-IQM02-T1":
                case "2670-PC01-T1":
                case "2670-PMC02-T1":
                case "2670-WH01-T1":
                case "2670-MC02-T1":
                    #region 

                    oDOAHandler._sJump = CheckJump(sLOCFrom, sLOCTo, roleCode, sAPTYP) == true ? "Y" : "N";
                    if (oDOAHandler._sJump == "Y")
                    {
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        switch (sAPTYP)
                        {
                            case "IA":
                                materialType = dtDetail.Rows[0]["MTART"].ToString().Trim().ToUpper();
                                //看最後兩碼 FT =成品
                                //RH = 材料
                                //HL = 半品
                                //VP = 包材
                                if (materialType.Length == 4 && materialType.Substring(2, 2) == "FT")
                                {
                                    oDOAHandler._sHandler = GetDOAHandler(roleCode,"FT"); //簽成品負責人
                                }
                                else
                                {
                                    oDOAHandler._sHandler = GetDOAHandler(roleCode);//簽其他負責人
                                }
                                break;
                            default:
                                if (sLOCFrom == "0010" && sLOCTo == "0013" && sAPTYP == "T1") //原材料良品倉轉到原材料不良品倉 2670 簽核人變更  2018-12-24
                                {
                                    oDOAHandler._sHandler = GetDOAHandler(roleCode,"UH");
                                }
                                else
                                {
                                    oDOAHandler._sHandler = GetDOAHandler(roleCode);
                                }
                                break;
                        }

                    }
                    break;
                    #endregion
                case "2670-MC01-T1":
           
                    #region //buyercode

                    oDOAHandler._sJump = CheckJump(sLOCFrom, sLOCTo, roleCode, sAPTYP) == true ? "Y" : "N";
                    if (oDOAHandler._sJump == "Y")
                    {
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        buyerCode = dtDetail.Rows[0]["EKGRP"].ToString().Trim().ToUpper();
                        oDOAHandler._sHandler = GetDOAHandlerByBuyerCode(buyerCode);
                    }

                    break;

                    #endregion

                case "2670-MD02":
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
                case "2670-BUHEAD":
                    #region 根據金額簽核到BUHEAD 大於等於20萬
                    double TWD00 = GetAmount(dtDetail);//匯率
                    //double TWD00 = 100000;
                    if (TWD00 >= 200000)
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
                case "2670-BGHEAD":
                    #region 根据金额判断是否需要签核至BGUHEAD  大於等於50萬
                    double TWD02 = GetAmount(dtDetail);//匯率
                    //double TWD02 = 250001;
                    if (TWD02 >= 500000)
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
                case "2670-CEO":
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
                case "2670-CHAIRMAN":
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
              
            }
            #endregion
        }

        /// <summary>
        /// BY CC,DEPT獲取簽核人 
        /// </summary>
        /// <param name="roleCode">關卡名</param>
        /// <param name="value1">CC</param>
        /// <param name="value2">DEPT</param>
        /// <returns>Handler</returns>
        protected string GetDOAHandler_HIS(string roleCode, string value1, string value2)
        {
            sql = "SELECT * FROM  TB_GDS_DOA_DETAIL with(nolock) WHERE RoleCode = @RoleCode AND Value1 = @value1 ";
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@RoleCode", SqlDbType.VarChar, roleCode));
            opc.Add(DataPara.CreateDataParameter("@value1", SqlDbType.VarChar, value1));
            //判斷部門是否為空
            if (value2.Length > 0)
            {
                sql += " AND Value2 = @value2 ";
                opc.Add(DataPara.CreateDataParameter("@value2", SqlDbType.VarChar, value2));
            }

            return sdb.GetRowString(sql, opc, "VALUE3");
        }

        protected bool CheckJump(string LOCFrom, string LOCTo, string RoleCode, string ApType)
        {
            bool jumpFlag = false;
            string sql = "select *from TB_GDS_DOA_JUMP Where LOCFrom=@From and LOCTo=@To and RoleCode=@RoleCode and ApType=@Type";
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@From", SqlDbType.VarChar, LOCFrom));
            opc.Add(DataPara.CreateDataParameter("@To", SqlDbType.VarChar, LOCTo));
            opc.Add(DataPara.CreateDataParameter("@RoleCode", SqlDbType.VarChar, RoleCode));
            opc.Add(DataPara.CreateDataParameter("@Type", SqlDbType.VarChar, ApType));
            DataTable dt = sdb.GetDataTable(sql, opc);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                if (dr["JumpOption"].ToString() == "Y")
                {
                    jumpFlag = true;
                }
                else
                {
                    jumpFlag = false;
                }
            }
            return jumpFlag;

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
