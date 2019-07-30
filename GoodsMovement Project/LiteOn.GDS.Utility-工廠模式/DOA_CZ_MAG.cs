using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using LiteOn.EA.BLL;
using LiteOn.EA.Model;
using LiteOn.EA.DAL;

namespace LiteOn.GDS.Utility
{
    public class DOA_CZ_MAG : DOA_Standard
    {

        public DOA_CZ_MAG()
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
            string sCostCenter = drHead["KOSTL"].ToString().ToUpper().Trim();
            switch (roleCode)
            {
                case "RZ01-DEPT00":
                    switch (sAPTYP)
                    {

                        case "T1":
                            switch (sCostCenter)
                            {
                                case "5M303":
                       
                                    oDOAHandler._sJump = "Y";
                                    oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                                    break;
                                default:
                                    oDOAHandler._sJump = "N";
                                    oDOAHandler._sHandler = GetDOAHandler_MAG(roleCode, sCostCenter, "");
                                    break;
                            }
                            break;

                        default:
                            oDOAHandler._sJump = "Y";
                            oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                            break;
                    }
                    break;
                case "RZ01-DEPT02":
                    switch (sAPTYP)
                    {

                        case "T1":
                            switch (sCostCenter)
                            {
                                case "5M303":
                                case "5M307":
                                    oDOAHandler._sJump = "Y";
                                    oDOAHandler._sDOA = SPMAppLine.GetApproveXML(oDOAHandler._sDOA);
                                    break;
                                default:
                                    oDOAHandler._sJump = "N";
                                    oDOAHandler._sHandler = GetDOAHandler(roleCode);
                                    break;
                            }
                            break;

                        default:
                            oDOAHandler._sJump = "N";
                            oDOAHandler._sHandler = GetDOAHandler(roleCode);
                            break;
                    }
                    break;
                default:
                    break;

            #endregion
            }
        }


        protected string GetDOAHandler_MAG(string roleCode, string value1, string value2)
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
            return sdb.GetRowString(sql, opc, "VALUE2");
        }
    }
       
 }

