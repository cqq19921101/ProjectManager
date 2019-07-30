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
    public class DOA
    {


        public DOA()
        {
        }

        /// <summary>
        /// BY PLANT獲取DOA簽核人
        /// </summary>
        /// <param name="sLogonId">申請人</param>
        /// <param name="sDOA">DOA資訊</param>
        /// <param name="dtHead">表頭</param>
        /// <param name="dtDetail">表身</param>
        /// <returns>DOA簽核人資訊</returns>
        public Model_DOAHandler GetStepHandler(string applicant, string sDOA, DataTable dtHead, DataTable dtDetail, bool startFormFlag)
        {
            Model_DOAHandler oDOAHandler = new Model_DOAHandler();
            DataRow dr = dtHead.Rows[0];
            oDOAHandler._sDOA = sDOA;
            oDOAHandler._sApplicant = applicant;
            oDOAHandler._sPlant = dr["WERKS"].ToString();
            oDOAHandler._sDocYear = dr["MJAHR_A"].ToString();
            oDOAHandler._sDocNo = dr["MBLNR_A"].ToString();

            IDOA oDOA = GetDOA(oDOAHandler._sPlant);
            if (!startFormFlag)
            {
                oDOAHandler = oDOA.GetStepHandler(oDOAHandler, dtHead, dtDetail);
            }
            else
            {
                oDOAHandler = oDOA.GetStepHandler_StartForm(oDOAHandler, dtHead, dtDetail);
            }

            //檢查當前簽核角色是否多簽核人情形
            if (oDOAHandler._ParallelFlag == false && oDOAHandler._sHandler.IndexOf(",") > 0)
            {
                DBIO.RecordParallelApprovalInfo(oDOAHandler);
            }

            return oDOAHandler;

        }

        public Hashtable GetMobileFormFields(DataTable dtHead, DataTable dtDetail)
        {
            IDOA oDOA = GetDOA(dtHead.Rows[0]["WERKS"].ToString());

            return oDOA.GetMobileFormFields(dtHead, dtDetail);
        }

        private IDOA GetDOA(string plant)
        {
            IDOA oDOA = new DOA_Standard();
            switch (plant)
            {
                case "2670":
                case "267S":
                    oDOA = new DOA_CZ_HIS();
                    break;
                case "2675":
                case "26HS":
                    oDOA = new DOA_CZ_HIS_OUT();
                    break;
                case "NZ01":
                case "NZ08":
                case "NZ51":
                case "NZ06":
                    oDOA = new DOA_CZ_NA();
                    break;
                case "LA60":
                case "LA6S":
                case "LA70":
                case "LA7S":
                    oDOA = new DOA_CZ_LAE();
                    break;
                case "2680":
                case "268S":
                    oDOA = new DOA_CZ_POWER();
                    break;
                case "1530":
                case "1550":
                case "3560":
                case "PD00":
                    oDOA = new DOA_CA_POWER();
                    break;
                case "CS01":
                case "CS02":
                    oDOA = new DOA_GZ_ENCLOSURE();
                    break;
                //GZ PID
                case "3285":
                case "3295":
                case "328N":
                case "329N":
                    oDOA = new DOA_GZ_PID();
                    break;
                //GZ IMG
                case "32A3":
                    oDOA = new DOA_GZ_IMG();
                    break;
                //QX NA
                case "NQ01":
                case "NQ08":
                    oDOA = new DOA_QX_NA();
                    break;
                case "NQ51":
                    oDOA = new DOA_QX_NA_NQ51();
                    break;
                //QX G-Tech
                case "CE01":
                    oDOA = new DOA_QX_GT();
                    break;
                case "RZ01":
                case "RZ0S":
                    oDOA = new DOA_CZ_MAG();
                    break;

            }

            return oDOA;

        }


        /// <summary>
        /// 獲取XML配置檔名（部分PLANT同一單據設立多種DOA）
        /// </summary>
        /// <param name="dtHead">標頭</param>
        /// <returns>XML配置檔名</returns>
        public static string GetXMLConfigName(DataTable dtHead)
        {
            DataRow dr = dtHead.Rows[0];
            string sAPTYP = dr["APTYP"].ToString();
            string plantCode = dr["WERKS"].ToString();
            string tempWERKS = plantCode;
            switch (plantCode)
            {
                case "1530":
                case "1550":
                case "3560":
                case "PD00":
                    if (sAPTYP == "IC")
                    {
                        string pdline = dr["ZPLINE"].ToString().Trim().ToUpper();
                        if (pdline.StartsWith("SMD"))
                        {
                            tempWERKS = tempWERKS + "-1";

                        }
                    }
                    break;
                case "2680":
                case "268S":
                    string sKOSTL = dr["KOSTL"].ToString().Trim().ToUpper();
                    if (sKOSTL.StartsWith("5B"))
                    {
                        tempWERKS = tempWERKS + "-" + "A3";
                    }
                    break; 
                default:
                    break;
            }
            return tempWERKS;
        }


        /// <summary>
        /// 是否顯示起單人資訊
        /// </summary>
        /// <returns></returns>
        public static bool ShowApplicantInfo(string plant, string apType)
        {
            bool flag = true;

            switch (plant)
            {
                case "3285":
                case "3295":
                case "32A3":
                case "LA60":
                case "LA6S":
                    flag = false;//使用公共賬號故不顯示
                    break;
                case "CS01":
                case "CS02":
                    flag = false;//CS01使用公共賬號故不顯示
                    break;
                case "2670":
                case "267S":
                case "2675":
                case "26HS":
                case "2680":
                case "268S":
                case "RZ01":
                case "RZ0S":
                    flag = false;//使用公共賬號故不顯示
                    break;
                case "CE01":
                case "NQ01":
                case "NQ51":
                case "NQ08":
                    flag = false;//NQ01使用公共賬號故不顯示
                    break;
                case "NZ01":
                case "NZ51":
                case "NZ08":
                case "NZ06":
                    flag = false;
                    break;
                default: break;//默認顯示
            }
            return flag;
        }

        /// <summary>
        /// 獲取SPM表單SUBJECT
        /// </summary>
        /// <param name="docNo">單號</param>
        /// <param name="plant">廠別代碼</param>

        /// <returns></returns>
        public static string GetSubject(string docNo, string plant)
        {
            string subject = string.Empty;
            switch (plant)
            {
                case "2680":
                    if (docNo.Substring(0, 2) == "I7")
                    {
                        subject = "[ 純溢領 ]" + "Goods Movement [" + docNo + "]-" + plant;
                    }
                    else
                    {
                        subject = "Goods Movement [" + docNo + "]-" + plant;
                    }
                    break;
                default:
                    subject = "Goods Movement [" + docNo + "]-" + plant;
                    break;
            }
            return subject;
        }

        /// <summary>
        /// 動態獲取SPM表單SUBJECT
        /// </summary>
        /// <param name="docNo">單號</param>
        /// <param name="plant">廠別代碼</param>

        /// <returns></returns>
        public static string GetSubject(string subject, DataTable dtHead)
        {

            DataRow dr = dtHead.Rows[0]; ;
            for (int i = 0; i < dtHead.Columns.Count; i++)
            {
                string col = dtHead.Columns[i].ColumnName.ToUpper();
                if (subject.IndexOf("{" + col + "}") >= 0)
                {
                    subject = subject.Replace("{" + col + "}", dr[col].ToString());
                }
                if (subject.IndexOf("{") < 0)
                {
                    break;
                }
            }

            return subject;
        }

        /// <summary>
        /// 獲取SPM表單起單人
        /// </summary>
        /// <param name="plant">廠別</param>
        /// <param name="apType">單據類別</param>
        /// <param name="applicantSAP">SAP中AD ACCOUNT</param>
        /// <returns></returns>
        public static string GetApplicant(string plant, string apType, string applicantSAP)
        {
            string applicant = applicantSAP.Replace(" ", "").Replace(".", "");
            switch (plant)
            {
                case "32A3":
                    applicant = "GZWFAdmin";
                    break;
                  //cz plant
                case "2670":
                case "267S":
                case "2675":
                case "26HS":
                case "2680":
                case "268S":
                case "NZ01":
                case "NZ51":
                case "NZ08":
                case "LA60":
                case "LA6S":
                case "NZ06":
                case "LA70":
                case "LA7S":
                    applicant = "CZWFAdmin";
                    break;
                case "RZ01":
                case "RZ0S":
                    applicant = "MAGOverusage";
                         break;
                case "NQ01":
                case "NQ51":
                case "NQ08":
                    switch (apType)
                    {
                        case "I3":
                        case "IC":
                        case "IE":
                        case "I1":
                        case "IA":
                            applicant = "QXWFAdmin";
                            break;
                    }
                    break;
                case "CE01":
                    switch (apType)
                    {
                        case "I3":
                        case "I5":
                        case "IC":
                            applicant = "QXWFAdmin";
                            break;
                    }
                    break;
            
                   
                default:
                    break;
            }
            return applicant;
        }

        /// <summary>
        /// 是否檢查ReferenceDocNo
        /// </summary>
        /// <param name="plant"></param>
        /// <param name="apType"></param>
        /// <returns></returns>
        public static bool CheckReferenceDocNoFlag(string plant, string apType)
        {
            bool flag = false;
            switch (plant)
            {
                //GZ PID /IMG
                case "3285":
                case "3295":
                case "32A3":
                    flag = true;
                    break;
                case "CS01":
                case "CS02":
                    if (apType == "I3")
                    {
                        flag = true;
                    }
                    break;
                case "267S":
                    if (apType == "I6")
                    {
                        flag = true;
                    }
                    break;
                //QX
                case "NQ01":
                    flag = true;
                    break;
                default:
                    break;
            }
            return flag;
        }

        /// <summary>
        /// 檢查OVER ISSUE
        /// </summary>
        /// <param name="plant"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        static public DataRow CheckOverIssueFlag(string plant, DataRow dr)
        {
            double actOverRate = 0;
            string overRate = dr["ZOVISS_RATE"].ToString().Trim();
            try
            {
                actOverRate = Convert.ToDouble(overRate);
            }
            catch (Exception)
            {
                actOverRate = 0;
            }

            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM TB_APPLICATION_PARAM WHERE APPLICATION_NAME = 'GoodsMovement' ");
            sql.Append("AND FUNCTION_NAME = 'GetFormData' AND PARAME_NAME = 'OverRate' AND PARAME_ITEM = @Plant");
            ArrayList opc = new ArrayList();
            opc.Add(DataPara.CreateDataParameter("@Plant", SqlDbType.VarChar, plant));
            bool flag = false;
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("SPM"));
            DataTable dt = sdb.GetDataTable(sql.ToString(), opc);

            if (dt.Rows.Count > 0)
            {

                if (dt.Rows[0]["PARAME_VALUE1"].ToString() == "*")
                {
                    //不限制物料模式
                    flag = FullyPartCheck(dt, actOverRate);
                }
                else
                {
                    //限制物料模式
                    flag = SpecialPartCheck(dr, dt, actOverRate);
                }
            }
            if (flag)
            {
                dr["ZOVISS"] = "Y";
                return dr;
            }
            else
            {
                return dr;
            }
        }

        /// <summary>
        /// 檢查全部物料
        /// </summary>
        /// <param name="dt">DETAIL TABLE</param>
        /// <param name="actOverRate">實際溢領比例</param>
        /// <returns>TRUE:超出 FALSE:未超出</returns>
        static public bool FullyPartCheck(DataTable dt, double actOverRate)
        {
            bool flag = false;
            double controlRate = 0;
            try
            {
                controlRate = Convert.ToDouble(dt.Rows[0]["PARAME_VALUE3"].ToString().Trim());
            }
            catch (Exception)
            {
                controlRate = 1000;
            }
            if (actOverRate > controlRate)
            {
                flag = true;

            }
            return flag;
        }

        /// <summary>
        /// 檢查指定物料
        /// </summary>
        /// <param name="dt">DETAIL TABLE</param>
        /// <param name="actOverRate">實際溢領比例</param>
        /// <returns>TRUE:超出 FALSE:未超出</returns>
        static public bool SpecialPartCheck(DataRow dr, DataTable dt, double actOverRate)
        {
            bool flag = false;
            #region
            //by part control
            string sMTL = dr["MAKTX"].ToString().Trim().ToUpper();//舊料號
            string sMTLDesc = dr["GROES"].ToString().Trim().ToUpper();//DESC
            for (int j = 2; j < 6; j++)
            {
                string sTemp = sMTL.Substring(0, 7 - j);
                DataRow[] al = dt.Select("PARAME_VALUE1='" + sTemp + "'");//檢查舊料號是否屬於限制物料范圍
                if (al.Length > 0)
                {
                    string sTemp2 = al[0]["PARAME_VALUE2"].ToString().Trim();//部分物料需比對DESC
                    double controlRate = 0;
                    try
                    {
                        controlRate = Convert.ToDouble(al[0]["PARAME_VALUE3"].ToString().Trim());//獲取超耗量警戒值
                    }
                    catch (Exception)
                    {
                        controlRate = 1000;
                    }
                    if (sTemp2.Length > 0)
                    {
                        if (sMTLDesc.StartsWith(sTemp2) && actOverRate > controlRate)
                        {
                            flag = true;
                            break;
                        }
                    }
                    else
                    {
                        if (actOverRate > controlRate)
                        {
                            flag = true;
                            break;
                        }
                    }
                }
            }
            #endregion
            return flag;
        }



        /// <summary>
        /// 各關卡信息驗證
        /// </summary>
        /// <param name="plant">廠別</param>
        /// <param name="apType">單據類別 </param>
        /// <param name="stepId">子關卡ID</param>
        /// <param name="caseId">SPM CASEID</param>
        /// <returns>異常信息</returns>
        static public string FieldsValidationByStepId(string plant, string apType, string stepId, int caseId)
        {
            StringBuilder errMsg = new StringBuilder();
            switch (plant)
            {
                case "2670":
                case "2675":
                    switch (apType)
                    {
                        case "IS":
                            if (stepId == "S1")
                            {
                                errMsg.Append(CheckAttachment(caseId));
                            }
                            break;
                    }
                    break;
            }
            return errMsg.ToString();
        }


        /// <summary>
        /// 檢查是否上傳文件
        /// </summary>
        /// <param name="spm_no">SPM 單號</param>
        /// <returns>返回檢查信息</returns>
        static private string CheckAttachment(int caseId)
        {
            string errMsg = string.Empty;
            //查看是否有上傳附件
            string sql = "SELECT FILENAME FROM ATTACHFILE WITH(NOLOCK) WHERE  CASEID = @CASEID ";
            ArrayList opc = new ArrayList();
            opc.Add(DataPara.CreateDataParameter("@CASEID", SqlDbType.Int, caseId));
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("SPM"));

            try
            {
                DataTable dt = sdb.GetDataTable(sql, opc);
                if (dt.Rows.Count <= 0)
                {
                    errMsg = "Pls check your attachment</BR>";
                }
            }
            catch (Exception)
            {
                throw new Exception("Check attactment status fail");
            }

            return errMsg;
        }


        /// <summary>
        /// 依據當前單據信息獲取關聯單據PLANT
        /// </summary>
        /// <param name="plant"></param>
        /// <param name="apType"></param>
        /// <param name="docNo"></param>
        /// <returns></returns>
        static public string GetReferenceDocPlant(string plant, string apType, string docNo)
        {
            string result = string.Empty;
            result = plant;
            switch (plant)
            {
                case "267S":
                    switch (apType)
                    {
                        case "I6":
                            result = "2670";
                            break;
                    }
                    break;
                case "26HS":
                    switch (apType)
                    {
                        case "I6":
                            result = "2675";
                            break;
                    }
                    break;
            }
            return result;

        }

        /// <summary>
        /// BY PLANT APTYPE管控是否顯示附件
        /// </summary>
        /// <param name="plant"></param>
        /// <param name="apType"></param>
        /// <returns></returns>
        static public bool ShowAttachmentInfo(string plant, string apType)
        {
            bool flag = false;

            switch (plant)
            {
                case "2670":
                case "2675":
                    switch (apType)
                    {
                        case "IS":
                            flag = true;
                            break;
                    }

                    break;
                default: break;
            }
            return flag;
        }
    }
}
