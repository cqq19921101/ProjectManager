using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using LiteOn.EA.DAL;
using System.Collections;

namespace LiteOn.GDS.Utility
{

    public class DOA_CZ_LAE : DOA_Standard
    {
        public DOA_CZ_LAE() { }

        protected override void GetDOABySpeicalRule(string roleCode, System.Data.DataTable dtHead, System.Data.DataTable dtDetail)
        {
            #region
            bool jumpFlag = false;//跳關標LA60-Buyer01示
            string buyerCode = string.Empty;
            DataRow drHead = dtHead.Rows[0];
            DataRow drDetail = dtDetail.Rows[0];
            string applydate = drHead["ERDAT"].ToString().Trim().ToUpper();
            string sAPTYP = drHead["APTYP"].ToString();
            string sLOCFrom = drHead["LOCFM"].ToString();
            string sLOCTo = drHead["LOCTO"].ToString();
            string costCenter = drHead["KOSTL"].ToString().ToUpper();
            string sCostCenter = drHead["KOSTL"].ToString().Substring(0, 3).ToUpper().Trim();
            //string sDepartment = drHead["ABTEI"].ToString().ToUpper().Trim();
            string sApplier = drHead["APPER"].ToString().ToUpper().Trim();
            string procurementType = drDetail["BESKZ"].ToString().Trim().ToUpper();
            string workOder = drDetail["AUFNR"].ToString().Trim().ToUpper();//工單號
            //總溢領金額(RMB)
            string sTTLAMT = drHead["TTLAMT"].ToString().Trim();
            //總溢領比例(%)
            string sTTLRATE = drHead["TTLRATE"].ToString().Trim();
            string materialType = string.Empty;
            string PurchasingPerson = string.Empty;
            string plant = drHead["WERKS"].ToString().Trim();
            //string sApplier = drHead["APPER"].ToString();//區域代碼
            string PRHandler = string.Empty;
            string orderType = string.Empty;
            string PartNo = drDetail["MATNR"].ToString().Trim();//料號
            string Product = drDetail["MTART"].ToString().Trim();
            string Batch = drDetail["CHARG"].ToString().Trim();//批次
            string PM = drDetail["PMMAIL"].ToString().Trim();//PM人员邮箱
            //string PM = PMMail.Substring(0, PMMail.IndexOf("@")).Replace(".", "");
            //DataRow[] orderCheck = dtDetail.Select(" GMFLOW='T'");//重工工單標誌 R:為重工，空為非重工,T:為試產工單
            //if (orderCheck.Length > 0) orderType = "T";

            switch (roleCode)
            {
                #region LA60/LA6S
                //Loc.from欄位為0010/Y010/0018/Y018/0019/Y019即為良品
                case "LA60-BuyerManager01":
                    jumpFlag = false;
                    if ((sAPTYP == "T1") && ((sLOCFrom == "0020" && sLOCTo == "0055") || (sLOCFrom == "Y020" && sLOCTo == "Y055")
                       || (sLOCTo == "Y051" || sLOCTo == "0051")))
                        jumpFlag = true;

                    if ((sAPTYP == "T1") && !((sLOCFrom == "0020" && sLOCTo == "0055") || (sLOCFrom == "Y020" && sLOCTo == "Y055")
                       || (sLOCTo == "Y051" || sLOCTo == "0051")) && !(sLOCTo == "0013" || sLOCTo == "Y013"
                       || (sLOCFrom == "0013" && sLOCTo == "0010") || (sLOCFrom == "Y013" && sLOCTo == "Y010")))
                        jumpFlag = true;

                    if ((sAPTYP == "IC" || sAPTYP == "ID") && (sLOCFrom == "0010" || sLOCFrom == "Y010" || sLOCFrom == "0018" || sLOCFrom == "Y018"
                        || sLOCFrom == "0019" || sLOCFrom == "Y019" || sLOCFrom == "0020" || sLOCFrom == "Y020" || sLOCFrom == "Y030" || sLOCFrom == "0030"))
                    {
                        jumpFlag = true;
                    }

                    if ((sAPTYP == "IC" || sAPTYP == "ID") && (sLOCFrom != "0013" && sLOCFrom != "Y013"))
                        jumpFlag = true;

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("採購主管資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                case "LA60-MCManager01":
                    if (((sAPTYP == "IC" || sAPTYP == "ID") && (sLOCFrom == "0010" || sLOCFrom == "Y010" || sLOCFrom == "0018" || sLOCFrom == "Y018"
                        || sLOCFrom == "0019" || sLOCFrom == "Y019" || sLOCFrom == "0020" || sLOCFrom == "Y020" || sLOCFrom == "Y030" || sLOCFrom == "0030")) || ((sAPTYP == "IA" || sAPTYP == "I1")
                        && (sApplier.Trim().ToUpper() == "MC01")))
                    {
                        jumpFlag = true;
                    }
                    else
                    {
                        jumpFlag = false;
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
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("MC主管資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                case "LA60-PMCManager01":
                    if (!String.IsNullOrEmpty(workOder) && workOder.Substring(0, 3).ToUpper() == "ZC2".ToUpper())
                    {
                        jumpFlag = false;
                    }
                    else
                    {
                        jumpFlag = true;
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
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("PMC主管資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                case "LA60-SQE01":
                    jumpFlag = false;
                    if ((sAPTYP == "IC" || sAPTYP == "ID") && (sLOCFrom == "0010" || sLOCFrom == "Y010" || sLOCFrom == "0018" || sLOCFrom == "Y018"
                        || sLOCFrom == "0019" || sLOCFrom == "Y019" || sLOCFrom == "0020" || sLOCFrom == "Y020" || sLOCFrom == "Y030" || sLOCFrom == "0030"))
                    {
                        jumpFlag = true;
                    }

                    if ((sAPTYP == "T1") && ((sLOCFrom == "0020" && sLOCTo == "0055") || (sLOCFrom == "Y020" && sLOCTo == "Y055")
                        || (sLOCTo == "Y051" || sLOCTo == "0051")))
                        jumpFlag = true;

                    if ((sAPTYP == "T1") && !((sLOCFrom == "0020" && sLOCTo == "0055") || (sLOCFrom == "Y020" && sLOCTo == "Y055")
                       || (sLOCTo == "Y051" || sLOCTo == "0051")) && !((sLOCFrom == "0012" && sLOCTo == "0010") || (sLOCFrom == "Y012" && sLOCTo == "Y010")
                       || (sLOCFrom == "0010" && sLOCTo == "0013") || (sLOCFrom == "Y010" && sLOCTo == "Y013") || (sLOCFrom == "0010" && sLOCTo == "0012")
                       || (sLOCFrom == "Y010" && sLOCTo == "Y012") || (sLOCFrom == "0013" && sLOCTo == "0010") || (sLOCFrom == "Y013" && sLOCTo == "Y010")
                       || (sLOCFrom == "0019" && sLOCTo == "0012") || (sLOCFrom == "Y019" && sLOCTo == "Y012") || (sLOCFrom == "0018" && sLOCTo == "0012")
                       || (sLOCFrom == "Y018" && sLOCTo == "Y012") || (sLOCFrom == "0019" && sLOCTo == "0013") || (sLOCFrom == "Y019" && sLOCTo == "Y013")
                       || (sLOCFrom == "0018" && sLOCTo == "0013") || (sLOCFrom == "Y018" && sLOCTo == "Y013")))
                        jumpFlag = true;

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("SQE資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                case "LA60-CustomManager01":
                    //料號第12位為Y非保稅
                    if ((sAPTYP == "I1" || sAPTYP == "IA" || sAPTYP == "IF" || sAPTYP == "IG") && PartNo.Length >= 12)
                    {
                        if (PartNo.Substring(11, 1).ToUpper() == "Y")
                            jumpFlag = true;
                        else
                            jumpFlag = false;
                    }
                    else
                    {
                        jumpFlag = false;
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
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("關務主管資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                case "LA60-ProduceManager01":
                    //T_GDS_DETAIL-MTART欄位,如果其值為:[ZYRH]原材料,其它[ZYHL,ZYBW,ZYVP]則為半品,ZYFT成品
                    if ((sAPTYP == "I1" || sAPTYP == "IA" || sAPTYP == "IF" || sAPTYP == "IG") && Product == "ZYFT" && (Batch.Trim().Length < 3 || Batch.Trim().Substring(0, 3) != "ZC7"))
                        jumpFlag = false;
                    else
                        jumpFlag = true;

                    if ((sAPTYP == "T1") && ((sLOCFrom == "0020" && sLOCTo == "0012") || (sLOCFrom == "Y020" && sLOCTo == "Y012")
                       || (sLOCFrom == "0012" && sLOCTo == "0020") || (sLOCFrom == "Y012" && sLOCTo == "Y020")))
                    {
                        jumpFlag = false;
                    }

                    if ((sAPTYP == "T1") && ((sLOCFrom == "0020" && sLOCTo == "0055") || (sLOCFrom == "Y020" && sLOCTo == "Y055")
                    || (sLOCTo == "Y051" || sLOCTo == "0051")))
                        jumpFlag = true;

                    if ((sAPTYP == "T1") && !((sLOCFrom == "0020" && sLOCTo == "0055") || (sLOCFrom == "Y020" && sLOCTo == "Y055")
                       || (sLOCTo == "Y051" || sLOCTo == "0051")) && !((sLOCFrom == "0020" && sLOCTo == "0012") || (sLOCFrom == "Y020" && sLOCTo == "Y012")
                       || (sLOCFrom == "0012" && sLOCTo == "0020") || (sLOCFrom == "Y012" && sLOCTo == "Y020")))
                        jumpFlag = true;

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("生管主管資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                case "LA60-MC01":
                    //T_GDS_DETAIL-MTART欄位,如果其值為:[ZYRH]原材料,其它[ZYHL,ZYBW,ZYVP]則為半品,ZYFT成品
                    if (((sAPTYP == "IA" || sAPTYP == "I1" || sAPTYP == "IF" || sAPTYP == "IG") && Product == "ZYFT")
                    || ((sAPTYP == "IC" || sAPTYP == "ID") && (sLOCFrom == "0010" || sLOCFrom == "Y010" || sLOCFrom == "0018"
                    || sLOCFrom == "Y018" || sLOCFrom == "0019" || sLOCFrom == "Y019")))
                    {
                        jumpFlag = true;
                    }
                    else
                    {
                        jumpFlag = false;
                    }

                    if ((sAPTYP == "IA" || sAPTYP == "I1") && sApplier.Trim().ToUpper() == "MC01")
                        jumpFlag = true;

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("MC資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                case "LA60-Manager01":
                    oDOAHandler._sHandler = GetDOAHandler(roleCode, sApplier);
                    if (oDOAHandler._sHandler.Length == 0)
                    {
                        GetHandlerErrAlarm(dtHead, string.Format("主管1資料缺失，無法獲取簽核人"));
                    }
                    break;
                case "LA60-Manager02":
                    oDOAHandler._sHandler = GetDOAHandler(roleCode, sApplier);
                    if (oDOAHandler._sHandler.Length == 0)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                    }
                    break;
                case "LA6S-Manager1":
                    oDOAHandler._sHandler = GetDOAHandler(roleCode, sApplier);
                    if (oDOAHandler._sHandler.Length == 0)
                    {
                        GetHandlerErrAlarm(dtHead, string.Format("主管1資料缺失，無法獲取簽核人"));
                    }
                    break;
                case "LA6S-Manager2":
                    oDOAHandler._sHandler = GetDOAHandler(roleCode, sApplier);
                    if (oDOAHandler._sHandler.Length == 0)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                    }
                    break;
                //Loc.from欄位為0010/Y010/0018/Y018/0019/Y019即為良品
                case "LA60-AssistManager01":
                    jumpFlag = true;
                    WhetherJump(sAPTYP, null, "LA60-Ast", applydate, dtDetail, roleCode, procurementType, sTTLAMT, sTTLRATE);
                    if (oDOAHandler._sJump != "Y" && costCenter.Trim() == "LA5143010")
                    {
                        jumpFlag = false;
                    }

                    if (((sAPTYP == "IC" || sAPTYP == "ID") && (sLOCFrom == "0010" || sLOCFrom == "Y010" || sLOCFrom == "0018" || sLOCFrom == "Y018"
                        || sLOCFrom == "0019" || sLOCFrom == "Y019" || sLOCFrom == "0020" || sLOCFrom == "Y020" || sLOCFrom == "Y030" || sLOCFrom == "0030")) || ((sAPTYP == "I3") &&
                        (!String.IsNullOrEmpty(workOder) && workOder.Substring(0, 3).ToUpper() == "ZC2".ToUpper())))
                        jumpFlag = true;

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("MFG副理1資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                case "LA60-AssistManager02":
                    jumpFlag = true;
                    WhetherJump(sAPTYP, null, "LA60-Ast", applydate, dtDetail, roleCode, procurementType, sTTLAMT, sTTLRATE);
                    if (oDOAHandler._sJump != "Y" && costCenter.Trim() != "LA5143010")
                    {
                        jumpFlag = false;
                    }

                    if (((sAPTYP == "IC" || sAPTYP == "ID") && (sLOCFrom == "0010" || sLOCFrom == "Y010" || sLOCFrom == "0018" || sLOCFrom == "Y018"
                        || sLOCFrom == "0019" || sLOCFrom == "Y019" || sLOCFrom == "0020" || sLOCFrom == "Y020" || sLOCFrom == "Y030" || sLOCFrom == "0030")) || ((sAPTYP == "I3") &&
                        (!String.IsNullOrEmpty(workOder) && workOder.Substring(0, 3).ToUpper() == "ZC2".ToUpper())))
                        jumpFlag = true;

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("MFG副理2資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                case "LA60-MD01":
                    if ((sAPTYP == "IC" || sAPTYP == "ID") && (sLOCFrom == "0010" || sLOCFrom == "Y010" || sLOCFrom == "0018" || sLOCFrom == "Y018"
                        || sLOCFrom == "0019" || sLOCFrom == "Y019" || sLOCFrom == "0020" || sLOCFrom == "Y020" || sLOCFrom == "Y030" || sLOCFrom == "0030"))
                        jumpFlag = true;
                    else
                        jumpFlag = false;

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        WhetherJump(sAPTYP, null, "LA60-MD", applydate, dtDetail, roleCode, procurementType, sTTLAMT, sTTLRATE);
                        if (oDOAHandler._sJump != "Y" && oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("廠長資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                case "LA60-BA":
                    jumpFlag = false;
                    if ((sAPTYP == "IC" || sAPTYP == "ID") && (sLOCFrom == "0010" || sLOCFrom == "Y010" || sLOCFrom == "0018" || sLOCFrom == "Y018"
                        || sLOCFrom == "0019" || sLOCFrom == "Y019" || sLOCFrom == "0020" || sLOCFrom == "Y020" || sLOCFrom == "Y030" || sLOCFrom == "0030"))
                    {
                        jumpFlag = true;
                    }

                    if ((sAPTYP == "T1") && ((sLOCFrom == "0020" && sLOCTo == "0055") || (sLOCFrom == "Y020" && sLOCTo == "Y055")
                        || (sLOCTo == "Y051" || sLOCTo == "0051")))
                        jumpFlag = true;

                    if ((sAPTYP == "T1") && !((sLOCFrom == "0020" && sLOCTo == "0055") || (sLOCFrom == "Y020" && sLOCTo == "Y055")
                       || (sLOCTo == "Y051" || sLOCTo == "0051")) && !((sLOCFrom == "0012" && sLOCTo == "0010") || (sLOCFrom == "Y012" && sLOCTo == "Y010")
                       || (sLOCFrom == "0010" && sLOCTo == "0013") || (sLOCFrom == "Y010" && sLOCTo == "Y013") || (sLOCFrom == "0010" && sLOCTo == "0012")
                       || (sLOCFrom == "Y010" && sLOCTo == "Y012") || (sLOCFrom == "0013" && sLOCTo == "0010") || (sLOCFrom == "Y013" && sLOCTo == "Y010")
                       || (sLOCFrom == "0019" && sLOCTo == "0012") || (sLOCFrom == "Y019" && sLOCTo == "Y012") || (sLOCFrom == "0018" && sLOCTo == "0012")
                       || (sLOCFrom == "Y018" && sLOCTo == "Y012") || (sLOCFrom == "0019" && sLOCTo == "0013") || (sLOCFrom == "Y019" && sLOCTo == "Y013")
                       || (sLOCFrom == "0018" && sLOCTo == "0013") || (sLOCFrom == "Y018" && sLOCTo == "Y013")))
                        jumpFlag = true;

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("BA資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;                    //if (jumpFlag)
                //{
                //    oDOAHandler._sJump = "Y";
                //    oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                //}
                //else
                //{
                //    oDOAHandler._sJump = "N";
                //    oDOAHandler._sHandler = GetDOAHandler(roleCode);
                //    if (oDOAHandler._sHandler.Length == 0)
                //    {
                //        GetHandlerErrAlarm(dtHead, string.Format("BA資料缺失，無法獲取簽核人"));
                //    }
                //}
                //break;
                case "LA60-PMManager01":
                    jumpFlag = true;
                    if (((sAPTYP == "IF" || sAPTYP == "IG") && costCenter.Trim().Substring(0, 4) == "LA51")
                        || ((sAPTYP == "IA" || sAPTYP == "I1") && (costCenter.Trim() == "LA1123010" || costCenter.Trim() == "LA5123010")))
                        jumpFlag = true;
                    else
                    {
                        if (costCenter.Trim().Substring(0, 4) != "LA51")
                            jumpFlag = false;
                    }

                    if ((sAPTYP == "IA" || sAPTYP == "I1") && (Product != "ZYFT" || Batch.Trim().Length < 3 || Batch.Trim().Substring(0, 3) != "ZC7"))
                        jumpFlag = true;

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = PM;
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("PM主管1資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                case "LA60-PMManager02":
                    jumpFlag = true;
                    if ((sAPTYP == "IA" || sAPTYP == "I1") && (costCenter.Trim() == "LA1123010" || costCenter.Trim() == "LA5123010"))
                        jumpFlag = true;
                    else
                    {
                        if (costCenter.Trim().Substring(0, 4) == "LA51")
                            jumpFlag = false;
                    }

                    if ((sAPTYP == "IA" || sAPTYP == "I1") && (Product != "ZYFT" || Batch.Trim().Length < 3 || Batch.Trim().Substring(0, 3) != "ZC7"))
                        jumpFlag = true;

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("PM主管2資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                case "LA60-Buyer01":
                    jumpFlag = false;
                    if ((sAPTYP == "IC" || sAPTYP == "ID") && (sLOCFrom == "0010" || sLOCFrom == "Y010" || sLOCFrom == "0018" || sLOCFrom == "Y018" || sLOCFrom == "0019"
                        || sLOCFrom == "Y019" || sLOCFrom == "0020" || sLOCFrom == "Y020" || sLOCFrom == "Y030" || sLOCFrom == "0030"))
                        jumpFlag = true;

                    if ((sAPTYP == "IC" || sAPTYP == "ID") && (sLOCFrom != "0013" && sLOCFrom != "Y013"))
                        jumpFlag = true;

                    if ((sAPTYP == "T1") && ((sLOCFrom == "0020" && sLOCTo == "0055") || (sLOCFrom == "Y020" && sLOCTo == "Y055")
                       || (sLOCTo == "Y051" || sLOCTo == "0051")))
                        jumpFlag = true;

                    if ((sAPTYP == "T1") && !((sLOCFrom == "0020" && sLOCTo == "0055") || (sLOCFrom == "Y020" && sLOCTo == "Y055")
                       || (sLOCTo == "Y051" || sLOCTo == "0051")) && !(sLOCTo == "0013" || sLOCTo == "Y013"
                       || (sLOCFrom == "0013" && sLOCTo == "0010") || (sLOCFrom == "Y013" && sLOCTo == "Y010")))
                        jumpFlag = true;

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        Hashtable ht2 = new Hashtable();
                        foreach (DataRow item in dtDetail.Rows)
                        {
                            //获取人员
                            buyerCode = item["EKGRP"].ToString();
                            if (!string.IsNullOrEmpty(buyerCode))
                            {
                                PRHandler = GetDOAHandlerByBuyerCode(buyerCode);
                                if (ht2.Contains(PRHandler.ToUpper()) == false)
                                {
                                    ht2.Add(PRHandler.ToUpper(), 0);
                                    if (oDOAHandler._sHandler.Length > 0)
                                    {
                                        oDOAHandler._sHandler += ",";
                                    }
                                    oDOAHandler._sHandler += PRHandler;
                                }
                                else
                                {
                                    continue;//如果存在重複的人,跳過繼續
                                }
                            }
                            else
                            {
                                oDOAHandler._sHandler += "," + GetDOAHandler(roleCode);
                            }
                        }
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("採購資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                case "LA6S-MCManager01":
                    oDOAHandler._sHandler = GetDOAHandler(roleCode);
                    if (oDOAHandler._sHandler.Length == 0)
                    {
                        GetHandlerErrAlarm(dtHead, string.Format("MC主管資料缺失，無法獲取簽核人"));
                    }
                    break;
                case "LA6S-CustomManager01":
                    //料號第12位為Y非保稅
                    if (sAPTYP == "IA" && PartNo.Length >= 12)
                    {
                        if (PartNo.Substring(11, 1).ToUpper() == "Y")
                            jumpFlag = true;
                        else
                            jumpFlag = false;
                    }
                    else
                    {
                        jumpFlag = false;
                    }
                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("關務主管資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                case "LA6S-Manager01":
                    oDOAHandler._sHandler = GetDOAHandler(roleCode);
                    if (oDOAHandler._sHandler.Length == 0)
                    {
                        GetHandlerErrAlarm(dtHead, string.Format("主管1資料缺失，無法獲取簽核人"));
                    }
                    break;
                case "LA6S-AssistManager01":
                    if (costCenter.Trim().Substring(0, 4) == "LA51")
                        jumpFlag = false;
                    else
                        jumpFlag = true;

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("副理1資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                case "LA6S-AssistManager02":
                    if (costCenter.Trim() == "LA1143010" || costCenter.Trim() == "LA1143060" || costCenter.Trim() == "LA143061" || costCenter.Trim().Substring(0, 4) != "LA51")
                        jumpFlag = false;
                    else
                        jumpFlag = true;

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("副理2資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                case "LA6S-MD01":
                    oDOAHandler._sHandler = GetDOAHandler(roleCode);
                    if (oDOAHandler._sHandler.Length == 0)
                    {
                        GetHandlerErrAlarm(dtHead, string.Format("廠長資料缺失，無法獲取簽核人"));
                    }
                    break;
                case "LA6S-CostCenterManager01":
                    if (costCenter.Trim() != "LA1143010" && costCenter.Trim() != "LA1143060" && costCenter.Trim() != "LA143061" && costCenter.Trim() != "LA5143010")
                        jumpFlag = false;
                    else
                        jumpFlag = true;

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode, costCenter.Trim());
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("成本中心主管資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                case "LA60-CQE01":
                    jumpFlag = true;
                    if ((sAPTYP == "T1") && ((sLOCFrom == "0020" && sLOCTo == "0012") || (sLOCFrom == "Y020" && sLOCTo == "Y012")
                       || (sLOCFrom == "0012" && sLOCTo == "0020") || (sLOCFrom == "Y012" && sLOCTo == "Y020")))
                    {
                        jumpFlag = false;
                    }

                    if ((sAPTYP == "T1") && ((sLOCFrom == "0020" && sLOCTo == "0055") || (sLOCFrom == "Y020" && sLOCTo == "Y055")
                    || (sLOCTo == "Y051" || sLOCTo == "0051")))
                        jumpFlag = true;

                    if ((sAPTYP == "T1") && !((sLOCFrom == "0020" && sLOCTo == "0055") || (sLOCFrom == "Y020" && sLOCTo == "Y055")
                       || (sLOCTo == "Y051" || sLOCTo == "0051")) && !((sLOCFrom == "0020" && sLOCTo == "0012") || (sLOCFrom == "Y020" && sLOCTo == "Y012")
                       || (sLOCFrom == "0012" && sLOCTo == "0020") || (sLOCFrom == "Y012" && sLOCTo == "Y020")))
                        jumpFlag = true;

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("CQE資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                #endregion

                #region LA70/LA7S
                //Loc.from欄位為0010/Y010/0018/Y018/0019/Y019即為良品
                case "LA70-BuyerManager01":
                    jumpFlag = false;
                    if ((sAPTYP == "T1") && ((sLOCFrom == "0020" && sLOCTo == "0055") || (sLOCFrom == "Y020" && sLOCTo == "Y055")
                       || (sLOCTo == "Y051" || sLOCTo == "0051")))
                        jumpFlag = true;

                    if ((sAPTYP == "T1") && !((sLOCFrom == "0020" && sLOCTo == "0055") || (sLOCFrom == "Y020" && sLOCTo == "Y055")
                       || (sLOCTo == "Y051" || sLOCTo == "0051")) && !(sLOCTo == "0013" || sLOCTo == "Y013"
                       || (sLOCFrom == "0013" && sLOCTo == "0010") || (sLOCFrom == "Y013" && sLOCTo == "Y010")))
                        jumpFlag = true;

                    if ((sAPTYP == "IC" || sAPTYP == "ID") && (sLOCFrom == "0010" || sLOCFrom == "Y010" || sLOCFrom == "0018" || sLOCFrom == "Y018"
                        || sLOCFrom == "0019" || sLOCFrom == "Y019" || sLOCFrom == "0020" || sLOCFrom == "Y020" || sLOCFrom == "Y030" || sLOCFrom == "0030"))
                    {
                        jumpFlag = true;
                    }

                    if ((sAPTYP == "IC" || sAPTYP == "ID") && (sLOCFrom != "0013" && sLOCFrom != "Y013"))
                        jumpFlag = true;

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("採購主管資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                case "LA70-MCManager01":
                    if (((sAPTYP == "IC" || sAPTYP == "ID") && (sLOCFrom == "0010" || sLOCFrom == "Y010" || sLOCFrom == "0018" || sLOCFrom == "Y018"
                        || sLOCFrom == "0019" || sLOCFrom == "Y019" || sLOCFrom == "0020" || sLOCFrom == "Y020" || sLOCFrom == "Y030" || sLOCFrom == "0030")) || ((sAPTYP == "IA" || sAPTYP == "I1")
                        && (sApplier.Trim().ToUpper() == "MC01")))
                    {
                        jumpFlag = true;
                    }
                    else
                    {
                        jumpFlag = false;
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
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("MC主管資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                case "LA70-PMCManager01":
                    if (!String.IsNullOrEmpty(workOder) && workOder.Substring(0, 3).ToUpper() == "ZC2".ToUpper())
                    {
                        jumpFlag = false;
                    }
                    else
                    {
                        jumpFlag = true;
                    }

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = PM;
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("PMC主管資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                case "LA70-SQE01":
                    jumpFlag = false;
                    if ((sAPTYP == "IC" || sAPTYP == "ID") && (sLOCFrom == "0010" || sLOCFrom == "Y010" || sLOCFrom == "0018" || sLOCFrom == "Y018"
                        || sLOCFrom == "0019" || sLOCFrom == "Y019" || sLOCFrom == "0020" || sLOCFrom == "Y020" || sLOCFrom == "Y030" || sLOCFrom == "0030"))
                    {
                        jumpFlag = true;
                    }

                    if ((sAPTYP == "T1") && ((sLOCFrom == "0020" && sLOCTo == "0055") || (sLOCFrom == "Y020" && sLOCTo == "Y055")
                        || (sLOCTo == "Y051" || sLOCTo == "0051")))
                        jumpFlag = true;

                    if ((sAPTYP == "T1") && !((sLOCFrom == "0020" && sLOCTo == "0055") || (sLOCFrom == "Y020" && sLOCTo == "Y055")
                       || (sLOCTo == "Y051" || sLOCTo == "0051")) && !((sLOCFrom == "0012" && sLOCTo == "0010") || (sLOCFrom == "Y012" && sLOCTo == "Y010")
                       || (sLOCFrom == "0010" && sLOCTo == "0013") || (sLOCFrom == "Y010" && sLOCTo == "Y013") || (sLOCFrom == "0010" && sLOCTo == "0012")
                       || (sLOCFrom == "Y010" && sLOCTo == "Y012") || (sLOCFrom == "0013" && sLOCTo == "0010") || (sLOCFrom == "Y013" && sLOCTo == "Y010")
                       || (sLOCFrom == "0019" && sLOCTo == "0012") || (sLOCFrom == "Y019" && sLOCTo == "Y012") || (sLOCFrom == "0018" && sLOCTo == "0012")
                       || (sLOCFrom == "Y018" && sLOCTo == "Y012") || (sLOCFrom == "0019" && sLOCTo == "0013") || (sLOCFrom == "Y019" && sLOCTo == "Y013")
                       || (sLOCFrom == "0018" && sLOCTo == "0013") || (sLOCFrom == "Y018" && sLOCTo == "Y013")))
                        jumpFlag = true;

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("SQE資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                case "LA70-CustomManager01":
                    //料號第12位為Y非保稅
                    if ((sAPTYP == "I1" || sAPTYP == "IA" || sAPTYP == "IF" || sAPTYP == "IG") && PartNo.Length >= 12)
                    {
                        if (PartNo.Substring(11, 1).ToUpper() == "Y")
                            jumpFlag = true;
                        else
                            jumpFlag = false;
                    }
                    else
                    {
                        jumpFlag = false;
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
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("關務主管資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                case "LA70-ProduceManager01":
                    //T_GDS_DETAIL-MTART欄位,如果其值為:[ZYRH]原材料,其它[ZYHL,ZYBW,ZYVP]則為半品,ZYFT成品
                    if ((sAPTYP == "I1" || sAPTYP == "IA" || sAPTYP == "IF" || sAPTYP == "IG") && Product == "ZYFT" && (Batch.Trim().Length < 3 || Batch.Trim().Substring(0, 3) != "ZC7"))
                        jumpFlag = false;
                    else
                        jumpFlag = true;

                    if ((sAPTYP == "T1") && ((sLOCFrom == "0020" && sLOCTo == "0012") || (sLOCFrom == "Y020" && sLOCTo == "Y012")
                       || (sLOCFrom == "0012" && sLOCTo == "0020") || (sLOCFrom == "Y012" && sLOCTo == "Y020")))
                    {
                        jumpFlag = false;
                    }

                    if ((sAPTYP == "T1") && ((sLOCFrom == "0020" && sLOCTo == "0055") || (sLOCFrom == "Y020" && sLOCTo == "Y055")
                    || (sLOCTo == "Y051" || sLOCTo == "0051")))
                        jumpFlag = true;

                    if ((sAPTYP == "T1") && !((sLOCFrom == "0020" && sLOCTo == "0055") || (sLOCFrom == "Y020" && sLOCTo == "Y055")
                       || (sLOCTo == "Y051" || sLOCTo == "0051")) && !((sLOCFrom == "0020" && sLOCTo == "0012") || (sLOCFrom == "Y020" && sLOCTo == "Y012")
                       || (sLOCFrom == "0012" && sLOCTo == "0020") || (sLOCFrom == "Y012" && sLOCTo == "Y020")))
                        jumpFlag = true;

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("生管主管資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                case "LA70-MC01":
                    //T_GDS_DETAIL-MTART欄位,如果其值為:[ZYRH]原材料,其它[ZYHL,ZYBW,ZYVP]則為半品,ZYFT成品
                    if (((sAPTYP == "IA" || sAPTYP == "I1" || sAPTYP == "IF" || sAPTYP == "IG") && Product == "ZYFT")
                    || ((sAPTYP == "IC" || sAPTYP == "ID") && (sLOCFrom == "0010" || sLOCFrom == "Y010" || sLOCFrom == "0018"
                    || sLOCFrom == "Y018" || sLOCFrom == "0019" || sLOCFrom == "Y019")))
                    {
                        jumpFlag = true;
                    }
                    else
                    {
                        jumpFlag = false;
                    }

                    if ((sAPTYP == "IA" || sAPTYP == "I1") && sApplier.Trim().ToUpper() == "MC01")
                        jumpFlag = true;

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("MC資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                case "LA70-Manager01":
                    oDOAHandler._sHandler = GetDOAHandler(roleCode, sApplier);
                    if (oDOAHandler._sHandler.Length == 0)
                    {
                        GetHandlerErrAlarm(dtHead, string.Format("主管1資料缺失，無法獲取簽核人"));
                    }
                    break;
                case "LA70-Manager02":
                    oDOAHandler._sHandler = GetDOAHandler(roleCode, sApplier);
                    if (oDOAHandler._sHandler.Length == 0)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                    }
                    break;
                case "LA7S-Manager1":
                    oDOAHandler._sHandler = GetDOAHandler(roleCode, sApplier);
                    if (oDOAHandler._sHandler.Length == 0)
                    {
                        GetHandlerErrAlarm(dtHead, string.Format("主管1資料缺失，無法獲取簽核人"));
                    }
                    break;
                case "LA7S-Manager2":
                    oDOAHandler._sHandler = GetDOAHandler(roleCode, sApplier);
                    if (oDOAHandler._sHandler.Length == 0)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                    }
                    break;
                //Loc.from欄位為0010/Y010/0018/Y018/0019/Y019即為良品
                case "LA70-AssistManager01":
                    jumpFlag = true;
                    WhetherJump(sAPTYP, null, "LA70-Ast", applydate, dtDetail, roleCode, procurementType, sTTLAMT, sTTLRATE);
                    if (oDOAHandler._sJump != "Y" && costCenter.Trim() == "LA5143010")
                    {
                        jumpFlag = false;
                    }

                    if (((sAPTYP == "IC" || sAPTYP == "ID") && (sLOCFrom == "0010" || sLOCFrom == "Y010" || sLOCFrom == "0018" || sLOCFrom == "Y018"
                        || sLOCFrom == "0019" || sLOCFrom == "Y019" || sLOCFrom == "0020" || sLOCFrom == "Y020" || sLOCFrom == "Y030" || sLOCFrom == "0030")) || ((sAPTYP == "I3") &&
                        (!String.IsNullOrEmpty(workOder) && workOder.Substring(0, 3).ToUpper() == "ZC2".ToUpper())))
                        jumpFlag = true;

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("MFG副理1資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                case "LA70-AssistManager02":
                    jumpFlag = true;
                    WhetherJump(sAPTYP, null, "LA70-Ast", applydate, dtDetail, roleCode, procurementType, sTTLAMT, sTTLRATE);
                    if (oDOAHandler._sJump != "Y" && costCenter.Trim() != "LA5143010")
                    {
                        jumpFlag = false;
                    }

                    if (((sAPTYP == "IC" || sAPTYP == "ID") && (sLOCFrom == "0010" || sLOCFrom == "Y010" || sLOCFrom == "0018" || sLOCFrom == "Y018"
                        || sLOCFrom == "0019" || sLOCFrom == "Y019" || sLOCFrom == "0020" || sLOCFrom == "Y020" || sLOCFrom == "Y030" || sLOCFrom == "0030")) || ((sAPTYP == "I3") &&
                        (!String.IsNullOrEmpty(workOder) && workOder.Substring(0, 3).ToUpper() == "ZC2".ToUpper())))
                        jumpFlag = true;

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("MFG副理2資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                case "LA70-MD01":
                    if ((sAPTYP == "IC" || sAPTYP == "ID") && (sLOCFrom == "0010" || sLOCFrom == "Y010" || sLOCFrom == "0018" || sLOCFrom == "Y018"
                        || sLOCFrom == "0019" || sLOCFrom == "Y019" || sLOCFrom == "0020" || sLOCFrom == "Y020" || sLOCFrom == "Y030" || sLOCFrom == "0030"))
                        jumpFlag = true;
                    else
                        jumpFlag = false;

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        WhetherJump(sAPTYP, null, "LA70-MD", applydate, dtDetail, roleCode, procurementType, sTTLAMT, sTTLRATE);
                        if (oDOAHandler._sJump != "Y" && oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("廠長資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                case "LA70-BA":
                    jumpFlag = false;
                    if ((sAPTYP == "IC" || sAPTYP == "ID") && (sLOCFrom == "0010" || sLOCFrom == "Y010" || sLOCFrom == "0018" || sLOCFrom == "Y018"
                        || sLOCFrom == "0019" || sLOCFrom == "Y019" || sLOCFrom == "0020" || sLOCFrom == "Y020" || sLOCFrom == "Y030" || sLOCFrom == "0030"))
                    {
                        jumpFlag = true;
                    }

                    if ((sAPTYP == "T1") && ((sLOCFrom == "0020" && sLOCTo == "0055") || (sLOCFrom == "Y020" && sLOCTo == "Y055")
                        || (sLOCTo == "Y051" || sLOCTo == "0051")))
                        jumpFlag = true;

                    if ((sAPTYP == "T1") && !((sLOCFrom == "0020" && sLOCTo == "0055") || (sLOCFrom == "Y020" && sLOCTo == "Y055")
                       || (sLOCTo == "Y051" || sLOCTo == "0051")) && !((sLOCFrom == "0012" && sLOCTo == "0010") || (sLOCFrom == "Y012" && sLOCTo == "Y010")
                       || (sLOCFrom == "0010" && sLOCTo == "0013") || (sLOCFrom == "Y010" && sLOCTo == "Y013") || (sLOCFrom == "0010" && sLOCTo == "0012")
                       || (sLOCFrom == "Y010" && sLOCTo == "Y012") || (sLOCFrom == "0013" && sLOCTo == "0010") || (sLOCFrom == "Y013" && sLOCTo == "Y010")
                       || (sLOCFrom == "0019" && sLOCTo == "0012") || (sLOCFrom == "Y019" && sLOCTo == "Y012") || (sLOCFrom == "0018" && sLOCTo == "0012")
                       || (sLOCFrom == "Y018" && sLOCTo == "Y012") || (sLOCFrom == "0019" && sLOCTo == "0013") || (sLOCFrom == "Y019" && sLOCTo == "Y013")
                       || (sLOCFrom == "0018" && sLOCTo == "0013") || (sLOCFrom == "Y018" && sLOCTo == "Y013")))
                        jumpFlag = true;

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("BA資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;                    //if (jumpFlag)
                //{
                //    oDOAHandler._sJump = "Y";
                //    oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                //}
                //else
                //{
                //    oDOAHandler._sJump = "N";
                //    oDOAHandler._sHandler = GetDOAHandler(roleCode);
                //    if (oDOAHandler._sHandler.Length == 0)
                //    {
                //        GetHandlerErrAlarm(dtHead, string.Format("BA資料缺失，無法獲取簽核人"));
                //    }
                //}
                //break;
                case "LA70-PMManager01":
                    jumpFlag = true;
                    if (((sAPTYP == "IF" || sAPTYP == "IG") && costCenter.Trim().Substring(0, 4) == "LA51")
                        || ((sAPTYP == "IA" || sAPTYP == "I1") && (costCenter.Trim() == "LA1123010" || costCenter.Trim() == "LA5123010")))
                        jumpFlag = true;
                    else
                    {
                        if (costCenter.Trim().Substring(0, 4) != "LA51")
                            jumpFlag = false;
                    }

                    if ((sAPTYP == "IA" || sAPTYP == "I1") && (Product != "ZYFT" || Batch.Trim().Length < 3 || Batch.Trim().Substring(0, 3) != "ZC7"))
                        jumpFlag = true;

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("PM主管1資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                case "LA70-PMManager02":
                    jumpFlag = true;
                    if ((sAPTYP == "IA" || sAPTYP == "I1") && (costCenter.Trim() == "LA1123010" || costCenter.Trim() == "LA5123010"))
                        jumpFlag = true;
                    else
                    {
                        if (costCenter.Trim().Substring(0, 4) == "LA51")
                            jumpFlag = false;
                    }

                    if ((sAPTYP == "IA" || sAPTYP == "I1") && (Product != "ZYFT" || Batch.Trim().Length < 3 || Batch.Trim().Substring(0, 3) != "ZC7"))
                        jumpFlag = true;

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("PM主管2資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                case "LA70-Buyer01":
                    jumpFlag = false;
                    if ((sAPTYP == "IC" || sAPTYP == "ID") && (sLOCFrom == "0010" || sLOCFrom == "Y010" || sLOCFrom == "0018" || sLOCFrom == "Y018" || sLOCFrom == "0019"
                        || sLOCFrom == "Y019" || sLOCFrom == "0020" || sLOCFrom == "Y020" || sLOCFrom == "Y030" || sLOCFrom == "0030"))
                        jumpFlag = true;

                    if ((sAPTYP == "IC" || sAPTYP == "ID") && (sLOCFrom != "0013" && sLOCFrom != "Y013"))
                        jumpFlag = true;

                    if ((sAPTYP == "T1") && ((sLOCFrom == "0020" && sLOCTo == "0055") || (sLOCFrom == "Y020" && sLOCTo == "Y055")
                       || (sLOCTo == "Y051" || sLOCTo == "0051")))
                        jumpFlag = true;

                    if ((sAPTYP == "T1") && !((sLOCFrom == "0020" && sLOCTo == "0055") || (sLOCFrom == "Y020" && sLOCTo == "Y055")
                       || (sLOCTo == "Y051" || sLOCTo == "0051")) && !(sLOCTo == "0013" || sLOCTo == "Y013"
                       || (sLOCFrom == "0013" && sLOCTo == "0010") || (sLOCFrom == "Y013" && sLOCTo == "Y010")))
                        jumpFlag = true;

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        Hashtable ht2 = new Hashtable();
                        foreach (DataRow item in dtDetail.Rows)
                        {
                            //获取人员
                            buyerCode = item["EKGRP"].ToString();
                            if (!string.IsNullOrEmpty(buyerCode))
                            {
                                PRHandler = GetDOAHandlerByBuyerCode(buyerCode);
                                if (ht2.Contains(PRHandler.ToUpper()) == false)
                                {
                                    ht2.Add(PRHandler.ToUpper(), 0);
                                    if (oDOAHandler._sHandler.Length > 0)
                                    {
                                        oDOAHandler._sHandler += ",";
                                    }
                                    oDOAHandler._sHandler += PRHandler;
                                }
                                else
                                {
                                    continue;//如果存在重複的人,跳過繼續
                                }
                            }
                            else
                            {
                                oDOAHandler._sHandler += "," + GetDOAHandler(roleCode);
                            }
                        }
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("採購資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                case "LA7S-MCManager01":
                    oDOAHandler._sHandler = GetDOAHandler(roleCode);
                    if (oDOAHandler._sHandler.Length == 0)
                    {
                        GetHandlerErrAlarm(dtHead, string.Format("MC主管資料缺失，無法獲取簽核人"));
                    }
                    break;
                case "LA7S-CustomManager01":
                    //料號第12位為Y非保稅
                    if (sAPTYP == "IA" && PartNo.Length >= 12)
                    {
                        if (PartNo.Substring(11, 1).ToUpper() == "Y")
                            jumpFlag = true;
                        else
                            jumpFlag = false;
                    }
                    else
                    {
                        jumpFlag = false;
                    }
                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("關務主管資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                case "LA7S-Manager01":
                    oDOAHandler._sHandler = GetDOAHandler(roleCode);
                    if (oDOAHandler._sHandler.Length == 0)
                    {
                        GetHandlerErrAlarm(dtHead, string.Format("主管1資料缺失，無法獲取簽核人"));
                    }
                    break;
                case "LA7S-AssistManager01":
                    if (costCenter.Trim().Substring(0, 4) == "LA51")
                        jumpFlag = false;
                    else
                        jumpFlag = true;

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("副理1資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                case "LA7S-AssistManager02":
                    if (costCenter.Trim() == "LA1143010" || costCenter.Trim() == "LA1143060" || costCenter.Trim() == "LA143061" || costCenter.Trim().Substring(0, 4) != "LA51")
                        jumpFlag = false;
                    else
                        jumpFlag = true;

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("副理2資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                case "LA7S-MD01":
                    oDOAHandler._sHandler = GetDOAHandler(roleCode);
                    if (oDOAHandler._sHandler.Length == 0)
                    {
                        GetHandlerErrAlarm(dtHead, string.Format("廠長資料缺失，無法獲取簽核人"));
                    }
                    break;
                case "LA7S-CostCenterManager01":
                    if (costCenter.Trim() != "LA1143010" && costCenter.Trim() != "LA1143060" && costCenter.Trim() != "LA143061" && costCenter.Trim() != "LA5143010")
                        jumpFlag = false;
                    else
                        jumpFlag = true;

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode, costCenter.Trim());
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("成本中心主管資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                case "LA70-CQE01":
                    jumpFlag = true;
                    if ((sAPTYP == "T1") && ((sLOCFrom == "0020" && sLOCTo == "0012") || (sLOCFrom == "Y020" && sLOCTo == "Y012")
                       || (sLOCFrom == "0012" && sLOCTo == "0020") || (sLOCFrom == "Y012" && sLOCTo == "Y020")))
                    {
                        jumpFlag = false;
                    }

                    if ((sAPTYP == "T1") && ((sLOCFrom == "0020" && sLOCTo == "0055") || (sLOCFrom == "Y020" && sLOCTo == "Y055")
                    || (sLOCTo == "Y051" || sLOCTo == "0051")))
                        jumpFlag = true;

                    if ((sAPTYP == "T1") && !((sLOCFrom == "0020" && sLOCTo == "0055") || (sLOCFrom == "Y020" && sLOCTo == "Y055")
                       || (sLOCTo == "Y051" || sLOCTo == "0051")) && !((sLOCFrom == "0020" && sLOCTo == "0012") || (sLOCFrom == "Y020" && sLOCTo == "Y012")
                       || (sLOCFrom == "0012" && sLOCTo == "0020") || (sLOCFrom == "Y012" && sLOCTo == "Y020")))
                        jumpFlag = true;

                    if (jumpFlag)
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    else
                    {
                        oDOAHandler._sJump = "N";
                        oDOAHandler._sHandler = GetDOAHandler(roleCode);
                        if (oDOAHandler._sHandler.Length == 0)
                        {
                            GetHandlerErrAlarm(dtHead, string.Format("CQE資料缺失，無法獲取簽核人"));
                        }
                    }
                    break;
                default:
                    break;
                #endregion

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
            bool flag = true; //默认总金额比kpi金额大。大，则控制当前关卡要签
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
                        amount += double.Parse(overAmount);
                        // atm=double.Parse(overAmt);
                        // number = double.Parse(num);                       
                    }
                }
                double KPI_Amount = double.Parse(controlKPI["OverAmount"].ToString());
                //注：此处与PID的不同，当一张单总金额比 OverAmount 少时，控制跳过当前关卡
                if (amount < KPI_Amount)
                {
                    flag = false;
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
                        case "LC30-QA/PROJHandler01":
                            //case "32A3-WHT0-MM/PIC01":
                            //case "32A3-WHT0-IQC01":
                            //case "32A3-WHT0-MMManager01":
                            //case "LC30-WHT1-WHOperator01":
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
