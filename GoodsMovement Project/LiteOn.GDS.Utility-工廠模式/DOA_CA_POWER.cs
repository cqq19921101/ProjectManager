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
    /// plant code 1530,1550,3560,PD00
    /// </summary>
    public class DOA_CA_POWER : DOA_Standard
    {

        public DOA_CA_POWER()
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
            string sAPTYP = string.Empty;
            string sLOC = string.Empty;
            string buyerCode = string.Empty;
            DataRow drHead = dtHead.Rows[0];
            sAPTYP = drHead["APTYP"].ToString();
            sLOC = drHead["LOCFM"].ToString();

            switch (roleCode)
            {

                case "1550-MFG02":
                case "1530-MFG02":
                case "3560-MFG02":
                case "PD00-MFG02":
                    #region
                    // 檢查溢領比例是否超限制
                    bool OverFlag = false;
                    for (int i = 0; i < dtDetail.Rows.Count; i++)
                    {
                        DataRow drTemp = dtDetail.Rows[i];
                        if (drTemp["ZOVISS"].ToString().Trim().ToUpper() == "Y")
                        {
                            OverFlag = true;
                        }
                    }
                    //超限制則簽理級主管
                    if (OverFlag)
                    {
                        Model_SPMOrgInfo SPMOrgInfo = new Model_SPMOrgInfo();
                        SPMOrg SPMOrg_class = new SPMOrg();
                        string line = dtHead.Rows[0]["ZPLINE"].ToString().Trim().ToUpper();
                        string surpervisor = GetDOAHandler(roleCode.Replace("MFG02", "MFG01"), line);
                        SPMOrg_class.GetManageListByLogonid(surpervisor, SPMOrgInfo);
                        string managerlist = SPMOrgInfo.fuLi + "," + SPMOrgInfo.jingLi;

                        //判斷理級主管是否 存在
                        if (SPMOrgInfo.jingLi.Length > 0)
                        {
                            oDOAHandler._sHandler = SPMOrgInfo.jingLi;
                            oDOAHandler._sJump = "N";
                        }
                        else
                        {
                            //理級主管不存在，向上找
                            sql = "SELECT JOB FROM [USER] WITH(NOLOCK) WHERE LOGONID = @LongonId ";
                            opc.Clear();
                            opc.Add(DataPara.CreateDataParameter("@LongonId", SqlDbType.VarChar, surpervisor));
                            string job = sdb.GetRowString(sql, opc, "job");
                            if (job == "16" || job == "18")
                            {
                                oDOAHandler._sHandler = surpervisor;
                                oDOAHandler._sJump = "N";
                            }
                            else
                            {
                                oDOAHandler._sHandler = SPMOrg_class.GetParentDeptManager(surpervisor);
                                oDOAHandler._sJump = "N";
                            }
                        }
                    }
                    else
                    {
                        oDOAHandler._sJump = "Y";
                        oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                    }
                    #endregion
                    break;
                case "1550-QC01":
                case "1530-QC01":
                case "3560-QC01":
                case "PD00-QC01":
                    //REASON CODE 為000簽QC
                    if (drHead["GRUND"].ToString().ToUpper() == "0000")
                    {
                        oDOAHandler._sHandler = GetDOAHandler(roleCode.Replace("QC01", "IQC01"));
                    }
                    else
                    {
                        //REASON CODE 非000簽IPQC
                       string  pdLine = drHead["ZPLINE"].ToString().Trim().ToUpper();
                        if (pdLine.Length > 0)
                        {
                            oDOAHandler._sHandler = GetDOAHandler(roleCode.Replace("QC01", "IPQC01"), pdLine);
                        }
                        else
                        {
                            GetHandlerErrAlarm(dtHead, "線體信息為空，無法獲取簽核人");
                        }
                    }

                    break;
                default:
                    break;
            }
            #endregion
        }

    }
}
