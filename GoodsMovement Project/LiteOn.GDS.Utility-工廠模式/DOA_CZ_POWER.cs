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
    /// plant code 2680,268S
    /// </summary>
    public class DOA_CZ_POWER : DOA_Standard
    {

        public DOA_CZ_POWER()
        {
        }

        /// <summary>
        /// 舊料號判定
        /// </summary>
        /// <param name="roleCode"></param>
        /// <returns></returns>
        protected DataTable GetPartList(string roleCode)
        {
            sql = "SELECT * FROM TB_GDS_DOA_SPECIAL  with(nolock)  WHERE RoleCode = @RoleCode ";
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@RoleCode", SqlDbType.VarChar, roleCode));
            return sdb.GetDataTable(sql, opc);
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
            string sAPTYP = string.Empty;
            string sLOC = string.Empty;
            string buyerCode = string.Empty;
            string sTemp = string.Empty;
            DataRow drHead = dtHead.Rows[0];

            string sCostCenter = drHead["KOSTL"].ToString().ToUpper().Trim();//成本中心
            string sDepartment = drHead["ABTEI"].ToString().ToUpper().Trim();//部門
            string materialType = string.Empty;//成品半成品/原物料

            string orderType = string.Empty;
            bool jumpFlag = false;//跳關標示
            sAPTYP = drHead["APTYP"].ToString();//AP TYPE
            sLOC = drHead["LOCFM"].ToString();//儲位
            DataRow[] orderCheck = dtDetail.Select(" GMFLOW='R'");//重工工單標誌 R:為重工，空為非重工
            if (orderCheck.Length > 0) orderType = "R";
            switch (roleCode.ToUpper())
            {
                case "2680-MFG02"://I2,I7
                    #region [check handler]
                    //檢查APPLIER ID（GDS01-GDS05屬於MFG）
                    switch (sAPTYP)
                    {
                        case "I2":
                            #region
                            switch (drHead["APPER"].ToString().ToUpper())
                            {
                                case "GDS01":
                                case "GDS02":
                                case "GDS03":
                                case "GDS04":
                                case "GDS05":
                                    jumpFlag = false;
                                    break;
                                default:
                                    jumpFlag = true;
                                    break;
                            }
                            #endregion
                            break;
                        case "I7":
                            #region
                            switch (drHead["APPER"].ToString().ToUpper())
                            {
                                case "GDS01":
                                case "GDS02":
                                case "GDS03":
                                case "GDS04":
                                case "GDS05":
                                    jumpFlag = false;
                                    break;
                                default:
                                    //非MFG無需簽核
                                    jumpFlag = true;
                                    break;
                            }


                            #endregion
                            break;
                        default:
                            switch (drHead["APPER"].ToString().ToUpper())
                            {
                                case "GDS01":
                                case "GDS02":
                                case "GDS03":
                                case "GDS04":
                                case "GDS05":
                                    jumpFlag = false;
                                    break;
                                default:
                                    //非MFG無需簽核
                                    jumpFlag = true;
                                    break;
                            }
                            break;
                    }

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
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

                    #endregion [check handler]
                    break;
                case "2680-MFG02-A3"://I7
                    #region [check handler]
                    //檢查APPLIER ID（GDS01-GDS05屬於MFG）
                    switch (sAPTYP)
                    {
                     
                        case "I7":
                            #region
                            switch (drHead["APPER"].ToString().ToUpper())
                            {
                                case "GDS30":
                                    jumpFlag = true;
                                    break;
                                default:
                                    jumpFlag = false;
                                    break;
                            }


                            #endregion
                            break;
                        default:
                            switch (drHead["APPER"].ToString().ToUpper())
                            {
                                case "GDS30":
                                    jumpFlag = true;
                                    break;
                                default:
                                    jumpFlag = false;
                                    break;
                            }
                            break;
                    }

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
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

                    #endregion [check handler]
                    break;
                case "2680-PUR01-A3"://I7
                    jumpFlag = false;
                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
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

                case "2680-WH01-A3":
                    oDOAHandler._sJump = "N";
                    switch (drHead["APPER"].ToString().ToUpper())
                    {
                        case "GDS30":
                            string pdLine = drHead["ZPLINE"].ToString().Trim().ToUpper();
                            oDOAHandler._sHandler = GetDOAHandler(roleCode, pdLine);
                            break;
                        default:
                            oDOAHandler._sHandler = GetDOAHandler(roleCode);
                            break;
                    }
                    break;

                case "2680-MFG01-A3":
                    oDOAHandler._sJump = "N";
                    switch (drHead["APPER"].ToString().ToUpper())
                    {
                        case "GDS30":
                            string pdLine = drHead["ZPLINE"].ToString().Trim().ToUpper();
                            oDOAHandler._sHandler = GetDOAHandler(roleCode, pdLine);
                            break;
                        default:
                            oDOAHandler._sHandler = GetDOAHandler(roleCode);
                            break;
                    }
                    break;

                case "2680-PUR01":           //i2,ic
                    #region
                    DataTable dt2680_PUR01 = GetPartList(roleCode);

                    //PUR窗口依舊料號前兩碼區分負責窗口（料號與窗口對應信息記錄與DB）
                    //只檢查第一顆料的採購
                    #region [loop check window info]

                    sTemp = dtDetail.Rows[0]["MAKTX"].ToString().Trim().ToUpper();

                    for (int j = 2; j < 6; j++)
                    {
                        string sTemp2 = sTemp.Substring(0, 7 - j);
                        DataRow[] al = dt2680_PUR01.Select("OldPartNo='" + sTemp2 + "'");
                        if (al.Length > 0)
                        {
                            oDOAHandler._sHandler = al[0]["handler"].ToString();
                            break;
                        }
                    }
                    if (oDOAHandler._sHandler.Length == 0)
                    {
                        GetHandlerErrAlarm(dtHead, "物料對應採購簽核人資料缺失，無法獲取簽核人，請聯繫IT");
                    }
                    #endregion [loop check window info]
                    #endregion
                    break;
                case "2680-PUR02":          //I1,I7,IF
                    #region
                    DataTable dt2680_PUR02 = new DataTable();
                    switch (sAPTYP)
                    {

                        case "I1":

                            materialType = dtDetail.Rows[0]["MTART"].ToString().Trim().ToUpper();
                            if (materialType.Length == 4 && materialType.Substring(2, 2) != "FT")
                            {
                                //原物料
                                jumpFlag = false;
                            }
                            else
                            {
                                //成品半成品
                                jumpFlag = true;
                            }

                            break;
                        case "I7":
                        case "IF":
                            jumpFlag = false;
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
                        //PUR窗口依舊料號前兩碼區分負責窗口（料號與窗口對應信息記錄與DB）
                        dt2680_PUR02 = GetPartList("2680-PUR01");

                        #region [loop check window info]

                        Hashtable ht = new Hashtable();
                        oDOAHandler._sJump = "N";
                        //檢查每顆料的採購，如有多個則進行並簽 
                        foreach (DataRow dr in dtDetail.Rows)
                        {
                            sTemp = dr["MAKTX"].ToString().Trim().ToUpper();

                            for (int j = 2; j < 6; j++)
                            {
                                string sTemp2 = sTemp.Substring(0, 7 - j);
                                DataRow[] al = dt2680_PUR02.Select("OldPartNo='" + sTemp2 + "'");
                                if (al.Length > 0 && ht.Contains(al[0]["handler"].ToString().ToUpper()) == false)
                                {
                                    ht.Add(al[0]["handler"].ToString().ToUpper(), 0);
                                    if (oDOAHandler._sHandler.Length > 0)
                                    {
                                        oDOAHandler._sHandler += ",";
                                    }
                                    oDOAHandler._sHandler += al[0]["handler"].ToString();
                                    break;
                                }
                            }
                        }

                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, "物料對應採購簽核人資料缺失，無法獲取簽核人，請聯繫IT");
                        }
                        #endregion [loop check window info]
                    }
                    #endregion
                    break;
                case "2680-PUR03":          //I1 PUR主管窗口 2019-01-25
                    #region
                    switch (sAPTYP)
                    {
                        case "I1":
                            materialType = dtDetail.Rows[0]["MTART"].ToString().Trim().ToUpper();
                            if (materialType.Length == 4 && materialType.Substring(2, 2) != "FT")
                            {
                                //原物料
                                jumpFlag = false;
                            }
                            else
                            {
                                //成品半成品
                                jumpFlag = true;
                            }
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
                    else //PUR主管固定人員
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);

                    }
                    break;
                    #endregion
                case "2680-PMM01":
                    jumpFlag = true;
                    //I2/I7如含有IC,PCB,线材等物料加会PMM主管
                    switch (sAPTYP)
                    {

                        case "I2":
                        case "I7":
                            string oldPartNo = string.Empty;
                            foreach (DataRow dr in dtDetail.Rows)
                            {
                                oldPartNo = dr["MAKTX"].ToString();
                                //PCB,IC,线材加会PMM MANAGER
                                if (oldPartNo.StartsWith("01X") || oldPartNo.StartsWith("32") || oldPartNo.StartsWith("04"))
                                {
                                    jumpFlag = false;
                                    break;
                                }
                            }

                            break;
                        case "I1":  //I1,IA 簽PMM主管  2019-01-25
                        case "IA":
                            jumpFlag = false;
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
                case "2680-WH01":  //IC
                    #region
                    //IC單（不良品退料）簽不良品倉管
                    if (sAPTYP == "IC")
                    {
                        oDOAHandler._sHandler = GetDOAHandler(roleCode, sAPTYP);
                        break;
                    }
                    //I2單（溢領）且儲位為0019(AI倉庫)簽AI倉管
                    if (sAPTYP == "I2" && sLOC == "0019")
                    {
                        oDOAHandler._sHandler = GetDOAHandler(roleCode, sAPTYP + "-" + sLOC);
                        break;
                    }
                    //I2單（溢領）且儲位非為0019(AI倉庫)簽MFG倉管
                    //MFG WH窗口依舊料號前兩碼區分負責窗口（料號與窗口對應信息記錄與DB）
                    #region [loop check]
                    DataTable dtP2680_WH01 = GetPartList(roleCode);

                    sTemp = dtDetail.Rows[0]["MAKTX"].ToString().Trim().ToUpper();

                    for (int j = 4; j < 6; j++)
                    {
                        string sTemp2 = sTemp.Substring(0, 7 - j);
                        DataRow[] al = dtP2680_WH01.Select("OldPartNo='" + sTemp2 + "'");
                        if (al.Length > 0)
                        {
                            oDOAHandler._sHandler = al[0]["handler"].ToString();
                            break;
                        }
                    }

                    #endregion [loop check]
                    if (oDOAHandler._sHandler.Length == 0)
                    {
                        GetHandlerErrAlarm(dtHead, "物料對應倉管簽核人資料缺失，無法獲取簽核人，請聯繫IT");
                    }
                    #endregion
                    break;
                case "2680-WH02":  //I1需簽倉庫主管 2019-01-25
                    #region
                    switch (sAPTYP)
                    { 
                        case "I1":
                        case "IA":
                        case "I6":
                            oDOAHandler._sHandler = GetDOAHandler(roleCode);
                             break;
                    }
                    #endregion
                    break;
                case "2680-IQC01":    //IC
                    #region
                    //IQC窗口依舊料號前兩碼區分負責窗口（料號與窗口對應信息記錄與DB）
                    DataTable dt2680_IQC01 = GetPartList(roleCode);
                    #region [loop check]

                    sTemp = dtDetail.Rows[0]["MAKTX"].ToString().Trim().ToUpper();
                    for (int j = 2; j < 6; j++)
                    {
                        string sTemp2 = sTemp.Substring(0, 7 - j);
                        DataRow[] al = dt2680_IQC01.Select("OldPartNo='" + sTemp2 + "'");
                        if (al.Length > 0)
                        {
                            oDOAHandler._sHandler = al[0]["handler"].ToString();
                            break;
                        }
                    }

                    #endregion [loop check]
                    if (oDOAHandler._sHandler.Length == 0)
                    {
                        GetHandlerErrAlarm(dtHead, "物料對應IQC簽核人資料缺失，無法獲取簽核人，請聯繫IT");
                    }
                    #endregion
                    break;
                case "2680-PC01":           //I1,I7.IA
                    #region
                    switch (sAPTYP)
                    {
                        case "I1":
                            materialType = dtDetail.Rows[0]["MTART"].ToString().Trim().ToUpper();
                            if (materialType.Length == 4 && materialType.Substring(2, 2) != "FT")
                            {
                                jumpFlag = true;
                            }
                            else
                            {
                                jumpFlag = false;
                            }
                            break;
                        case "I7":
                            if (orderType == "R")
                            {
                                //重工工單必簽
                                jumpFlag = false;
                            }
                            else
                            {
                                //非重工工單無需簽核
                                jumpFlag = true;
                            }
                            break;
                        case "IA":
                            jumpFlag = false;
                            break;
                        default:
                            jumpFlag = true;
                            break;
                    }

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
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
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, "物料對應關務簽核人資料缺失，無法獲取簽核人，請聯繫IT");
                        }
                    }
                    #endregion
                    break;
                case "2680-DEPT01":         //I1,IA
                    #region
                    switch (sAPTYP)
                    {
                        case "I1":
                        case "IA":
                            oDOAHandler._sHandler = GetDOAHandler_POWER(roleCode, sCostCenter, sDepartment);
                            if (oDOAHandler._sHandler.Length == 0)
                            {
                                GetHandlerErrAlarm(dtHead, string.Format("CostCenter:{0},Department:{1} 信息有誤，無法獲取簽核人", sCostCenter, sDepartment));
                            }
                            break;
                        default:
                            break;
                    }
                    #endregion
                    break;
                case "2680-DEPT02":         //I1,IA
                    #region
                    switch (sAPTYP)
                    {
                        case "I1":
                        case "IA":
                            oDOAHandler._sHandler = GetDOAHandler_POWER(roleCode, sCostCenter, "");
                            if (oDOAHandler._sHandler.Length == 0)
                            {
                                GetHandlerErrAlarm(dtHead, string.Format("CostCenter:{0},Department:{1} 信息有誤，無法獲取簽核人", sCostCenter, sCostCenter));
                            }
                            break;
                        default:
                            break;
                    }
                    #endregion
                    break;
                case "2680-CUST01":         //I1
                    #region
                    string iPart = string.Empty;
                    DataTable dtSpecial = GetPartList("2680-Cust01");
                    switch (sAPTYP)
                    {
                        case "I1":
                            materialType = dtDetail.Rows[0]["MTART"].ToString().Trim().ToUpper();

                            if (materialType.Length == 4 && materialType.Substring(2, 2) != "FT")
                            {
                                int i = 0;
                                foreach (DataRow dr in dtDetail.Rows)
                                {
                                    if (dtSpecial.Select("PartDesc='" + dr["IPART"].ToString() + "'").Length > 0)
                                    {
                                        iPart = dr["IPART"].ToString();
                                        i++;
                                        break;

                                    }
                                }
                                if (i > 0)
                                {
                                    //原物料 簽
                                    jumpFlag = false;
                                }
                                else
                                {
                                    jumpFlag = true;
                                }
                            }
                            else
                            {
                                //成品半成品  跳
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
                        //string iPart = dtDetail.Rows[0]["IPART"].ToString().Trim().ToUpper();
                        oDOAHandler._sJump = "N";//依COST CENTER 獲取簽核人                         
                        oDOAHandler._sHandler = dtSpecial.Select("PartDesc='" + iPart + "'")[0]["handler"].ToString();
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, "物料對應關務簽核人資料缺失，無法獲取簽核人，請聯繫IT");
                        }
                    }

                    #endregion
                    break;
                case "2680-MD01":             //I1,I6
                    #region
                    switch (sAPTYP)
                    {
                        case "I1":
                            materialType = dtDetail.Rows[0]["MTART"].ToString().Trim().ToUpper();
                            if (materialType.Length == 4 && materialType.Substring(2, 2) != "FT")
                            {
                                jumpFlag = true;
                            }
                            else
                            {
                                jumpFlag = false;
                            }
                            break;
                        case "I6":
                            jumpFlag = false;
                            break;
                        case "I2":
                        case "I7":
                            jumpFlag = true;
                            string oldPartNo = string.Empty;
                            foreach (DataRow dr in dtDetail.Rows)
                            {
                                oldPartNo = dr["MAKTX"].ToString();
                                //CASE 累计溢领超0.3%加会MD
                                if (oldPartNo.StartsWith("46"))
                                {
                                    try
                                    {
                                        if (double.Parse(dr["OVER_RATE"].ToString()) > 3)
                                        {
                                            jumpFlag = false;
                                            break;
                                        }
                                    }
                                    catch (Exception)
                                    {
                                    }

                                }
                            }
                            break;
                        default:
                            jumpFlag = true;
                            break;
                    }

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
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
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, "MD簽核人資料缺失，無法獲取簽核人，請聯繫IT");
                        }
                    }

                    #endregion
                    break;
                case "2680-QRA01":
                    #region
                    switch (sAPTYP)
                    {
                        case "I6":
                            oDOAHandler._sHandler = GetDOAHandler_POWER(roleCode, sCostCenter, sDepartment);
                            if (oDOAHandler._sHandler.Length == 0)
                            {
                                GetHandlerErrAlarm(dtHead, string.Format("CostCenter:{0},Department:{1} 信息有誤，無法獲取簽核人", sCostCenter, sDepartment));
                            }
                            break;
                    }
                    break;
                    #endregion
                case "2680-QRA02":
                    #region
                    switch (sAPTYP)
                    {
                        case "I6":
                            oDOAHandler._sHandler = GetDOAHandler_POWER(roleCode, sCostCenter, sDepartment);
                            if (oDOAHandler._sHandler.Length == 0)
                            {
                                GetHandlerErrAlarm(dtHead, string.Format("CostCenter:{0},Department:{1} 信息有誤，無法獲取簽核人", sCostCenter, sDepartment));
                            }
                            break;
                    }
                    break;
                    #endregion
                case "2680-QRA03":
                    #region
                    switch (sAPTYP)
                    {
                        case "I6":
                            oDOAHandler._sHandler = GetDOAHandler_POWER(roleCode, sCostCenter, sDepartment);
                            if (oDOAHandler._sHandler.Length == 0)
                            {
                                GetHandlerErrAlarm(dtHead, string.Format("CostCenter:{0},Department:{1} 信息有誤，無法獲取簽核人", sCostCenter, sDepartment));
                            }
                            break;
                    }
                    break;
                    #endregion
                case "2680-DEPT01-A3": //I1
                   #region
                    switch (sAPTYP)
                    {
                        case "I1":
                            oDOAHandler._sHandler = GetDOAHandler_POWER(roleCode, sCostCenter, "");
                            if (!string.IsNullOrEmpty(oDOAHandler._sHandler))
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
                        oDOAHandler._sHandler = GetDOAHandler_POWER(roleCode, sCostCenter, "");

                    }
                    #endregion
                    break;
                case "2680-DEPT02-A3":         //I1
                    #region
                    switch (sAPTYP)
                    {
                        case "I1":
                            oDOAHandler._sHandler = GetDOAHandler_POWER(roleCode, sCostCenter, "");
                            if (oDOAHandler._sHandler.Length == 0)
                            {
                                GetHandlerErrAlarm(dtHead, string.Format("CostCenter:{0},Department:{1} 信息有誤，無法獲取簽核人", sCostCenter, sCostCenter));
                            }
                            break;
                        default:
                            break;
                    }
                    #endregion
                    break;
                case "2680-MD02":
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
                case "2680-MD03":
                    #region 根據金額簽核到總廠長 大於等於2萬
                    double TWD05 = GetAmount(dtDetail);//匯率
                    //double TWD01 = 100000;
                    if (TWD05 >= 20000)
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
                case "2680-BUHEAD":
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
                case "2680-BGHEAD":
                    #region 根据金额判断是否需要签核至BGUHEAD  大於等於75萬
                    double TWD02 = GetAmount(dtDetail);//匯率
                    //double TWD02 = 250001;
                    if (TWD02 >= 750000)
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
                case "2680-CEO":
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
                case "2680-CHAIRMAN":
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
                default:
                    break;
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
        protected string GetDOAHandler_POWER(string roleCode, string value1, string value2)
        {
            sql = "SELECT * FROM  TB_GDS_DOA_DETAIL with(nolock) WHERE RoleCode = @RoleCode AND Value1 = @Value1";
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@RoleCode", SqlDbType.VarChar, roleCode));
            opc.Add(DataPara.CreateDataParameter("@Value1", SqlDbType.VarChar, value1));
            if (value2.Length > 0)
            {
                sql += " AND Value2 = @Value2 ";
                opc.Add(DataPara.CreateDataParameter("@Value2", SqlDbType.VarChar, value2));
                return sdb.GetRowString(sql, opc, "VALUE3");
            }
            return sdb.GetRowString(sql, opc, "VALUE2");
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
