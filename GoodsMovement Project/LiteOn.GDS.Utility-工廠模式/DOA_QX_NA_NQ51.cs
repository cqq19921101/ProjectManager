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
    /// plant code NQ51
    /// </summary>
    /// 
    public class DOA_QX_NA_NQ51:DOA_Standard 
    {

        public DOA_QX_NA_NQ51()
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
            string appuser = drHead["ERNAM"].ToString();
            string sAPTYP = drHead["APTYP"].ToString();
            string sLOC = drHead["LOCFM"].ToString();
            string costCenter = drHead["KOSTL"].ToString().ToUpper();
            string procurementType = drDetail["BESKZ"].ToString().Trim().ToUpper();
            string prdSuppervisor = drDetail["FEVOR"].ToString().Trim().ToUpper();
            string applydate = drHead["ERDAT"].ToString().Trim().ToUpper();
            string materialtype = drDetail["MTART"].ToString().Trim().ToUpper();
            string wo = drDetail["AUFNR"].ToString().Trim().ToUpper();
            string mrpcontroller = drDetail["DISPO"].ToString().Trim().ToUpper();
            string extgroup = drDetail["EXTWG"].ToString().Trim().ToUpper();
            //bool jumpFlag = false;//跳關標示
            string appletype = "NNQ-A";
            switch (roleCode)
            {
                #region "Step   NQ51-MFG01"
                case "NQ51-MFG01":
                    //switch (costCenter)
                    //{
                    //    //DIP部門取申請者LogonID為 Handler
                    //    case "N411":
                    //    case "N412":
                    //    case "N413":
                    //    case "N414":
                    //    case "N415":
                    //    case "N416":
                    //    case "N417":
                    //        oDOAHandler._sJump = "N";
                    //        oDOAHandler._sHandler = appuser.Replace(" ","");   
                    //        //
                    //        break;
                    //    default:
                    //        oDOAHandler._sJump = "N";
                    //        oDOAHandler._sHandler = GetDOAHandler_Value4(roleCode,costCenter ,"","");
                    //        break;
                    //}

                    oDOAHandler._sJump = "N";
                    oDOAHandler._sHandler = appuser.Replace(" ","");   
                    break;
                #endregion

                #region "Step   NQ51-MFG02"
                case "NQ51-MFG02":
                    switch (sAPTYP)
                    {
                        #region "I3"
                        case "I3":
                            switch (costCenter)
                            {   //SMT Dept
                                case "N420":
                                case "N421":
                                case "N422":
                                case "N424":
                                case "N425":
                                case "N426":
                                case "N427":
                                case "N428":
                                case "N429":
                                    #region "1.SMT  Dept ; 2.一般材料"  金額判定
                                    if (materialtype == "ZNRH")
                                    {
                                        if (CheckWoOverAmount(dtDetail, 90))
                                        {
                                            oDOAHandler._sHandler = GetDOAHandler_Value4(roleCode,costCenter.Substring(0,3), "", "");
                                            oDOAHandler._sJump = "N";
                                            oDOAHandler._cc = GetNextCCUserID(GetDOAHandler_Value5(roleCode, costCenter.Substring(0, 3), "", "", oDOAHandler._sHandler));
                                        }
                                        else
                                        {
                                            GoJump();
                                        }
                                    }
                                    #endregion
                                    else   // 客提   /   半品、成品 都要簽
                                    {
                                        oDOAHandler._sHandler = GetDOAHandler_Value4(roleCode, costCenter.Substring(0, 3), "", "");
                                        oDOAHandler._sJump = "N";
                                        oDOAHandler._cc = GetNextCCUserID(GetDOAHandler_Value5(roleCode, costCenter.Substring(0, 3), "", "", oDOAHandler._sHandler));
                                    }
                                    break;
                                         
                                default:
                                     GoJump();
                                    
                                    break;


                            }
                            break;

                        #endregion
                        default:
                            GoJump();
                            break;
                    }
                    break;
 
                        
                #endregion

                #region"Step NQ51-PUR01" 
                case "NQ51-PUR01":
                    switch (sAPTYP)
                    {
                        case "I1":
                            if (extgroup == appletype)    //    Apple   料
                            {
                                oDOAHandler._sJump = "N";
                                oDOAHandler._sHandler = GetDOAHandler_Value4(roleCode, sAPTYP, extgroup,"");
                                oDOAHandler._cc = GetNextCCUserID(GetDOAHandler_Value5(roleCode, sAPTYP, extgroup,"", oDOAHandler._sHandler));

                            }
                            else
                            {
                                if (materialtype == "ZNBW"  )
                                {
                                    oDOAHandler._sJump = "N";
                                    oDOAHandler._sHandler = GetDOAHandler_Value4(roleCode, sAPTYP, materialtype, "");
                                    oDOAHandler._cc = GetNextCCUserID(GetDOAHandler_Value5(roleCode, sAPTYP, materialtype, "", oDOAHandler._sHandler));

                                }
                                else
                                    GoJump();
                            }
                            break;
                        case "IA":   //IA Document
                            if (extgroup == appletype)    //    Apple   料
                            {
                                oDOAHandler._sJump = "N";
                                oDOAHandler._sHandler = GetDOAHandler_Value4(roleCode, sAPTYP, extgroup, "");
                                oDOAHandler._cc = GetNextCCUserID(GetDOAHandler_Value5(roleCode, sAPTYP, extgroup, "", oDOAHandler._sHandler));
                            }
                            else
                            {
                                GoJump(); 
                            }
                            break;
                    }
                    break;
                #endregion

                #region "Step NQ51-SA01"
                case "NQ51-SA01":
                    switch (sAPTYP)
                    {
                        case "I1":
                            if (extgroup == appletype)
                            {
                                oDOAHandler._sJump = "N";
                                oDOAHandler._sHandler = GetDOAHandler_Value4(roleCode, sAPTYP, extgroup,"");
                                oDOAHandler._cc = GetNextCCUserID(GetDOAHandler_Value5(roleCode, sAPTYP, extgroup,"", oDOAHandler._sHandler));
                            }
                            else
                            {
                                if (materialtype == "ZNBW")
                                {
                                    oDOAHandler._sJump = "N";
                                    oDOAHandler._sHandler = GetDOAHandler_Value4(roleCode, sAPTYP, materialtype, "");
                                    oDOAHandler._cc = GetNextCCUserID(GetDOAHandler_Value5(roleCode, sAPTYP, materialtype, "", oDOAHandler._sHandler));
                                }
                                else
                                {
                                    if (sLOC == "0018" || sLOC == "0050")
                                    {
                                        oDOAHandler._sJump = "N";
                                        oDOAHandler._sHandler = GetDOAHandler_Value4(roleCode, sAPTYP, sLOC, "");
                                        oDOAHandler._cc = GetNextCCUserID(GetDOAHandler_Value5(roleCode, sAPTYP, sLOC, "", oDOAHandler._sHandler));

                                    }
                                    else
                                    {
                                        GoJump();
                                    }

                                }
                            }
                            break;
                        case "IA":
                            if (extgroup == appletype)
                            {
                                oDOAHandler._sJump = "N";
                                oDOAHandler._sHandler = GetDOAHandler_Value4(roleCode, sAPTYP, extgroup, "");
                                oDOAHandler._cc = GetNextCCUserID(GetDOAHandler_Value5(roleCode, sAPTYP, extgroup, "", oDOAHandler._sHandler));
                            }
                            else
                            {
                                GoJump();
                            }
                            break;
                    }
                    break;
                #endregion

                #region"Step NQ51-MC01"
                case "NQ51-MC01":
                    //string wotype = GetWoType(wo);
                    switch (sAPTYP)
                    {
                        #region "II Doc"
                        case "I1":
                            if (extgroup == appletype)
                            {
                                oDOAHandler._sJump = "N";
                                oDOAHandler._sHandler = GetDOAHandler_Value4(roleCode, sAPTYP, extgroup, "");
                                oDOAHandler._cc = GetNextCCUserID(GetDOAHandler_Value5(roleCode, sAPTYP, extgroup, "", oDOAHandler._sHandler));
                            }
                            else
                            {
                                if (materialtype == "ZNBW")
                                {
                                    oDOAHandler._sJump = "N";
                                    oDOAHandler._sHandler = GetDOAHandler_Value4(roleCode, sAPTYP, materialtype, "");
                                    oDOAHandler._cc = GetNextCCUserID(GetDOAHandler_Value5(roleCode, sAPTYP, materialtype, "", oDOAHandler._sHandler));
                                }
                                else
                                {
                                    switch (sLOC)
                                    {
                                        case "0011": //一般材料
                                        case "0050":
                                        case "0018":
                                            oDOAHandler._sJump = "N";
                                            oDOAHandler._sHandler = GetDOAHandler_Value4(roleCode, sAPTYP, sLOC, "");
                                            oDOAHandler._cc = GetNextCCUserID(GetDOAHandler_Value5(roleCode, sAPTYP, sLOC, "", oDOAHandler._sHandler));
                                            break;
                                        default:

                                            GoJump();
                                            break;

                                    }
                                }
                            }
                            break;
                        #endregion

                        #region "IA Doc"
                        case "IA":
                            if (extgroup == appletype  )
                            {
                                oDOAHandler._sJump = "N";
                                oDOAHandler._sHandler = GetDOAHandler_Value4(roleCode, sAPTYP, extgroup,"");
                                oDOAHandler._cc = GetNextCCUserID(GetDOAHandler_Value5(roleCode, sAPTYP, extgroup, "", oDOAHandler._sHandler));
                            }
                            else
                            {
                                if (sLOC == "0011" || sLOC == "0050" || sLOC == "0018")
                                {
                                    oDOAHandler._sJump = "N";
                                    oDOAHandler._sHandler = GetDOAHandler_Value4(roleCode, sAPTYP, sLOC , "" );
                                    oDOAHandler._cc = GetNextCCUserID(GetDOAHandler_Value5(roleCode, sAPTYP, sLOC , "" , oDOAHandler._sHandler));
                                }
                                else
                                {
                                    GoJump();
                                }
                            }
                            break;
                        #endregion

                    }
                    break;
                #endregion

                #region"Step NQ51-PMC01"
                case "NQ51-PMC01":
                    switch (sAPTYP)
                    {
                        case "I1":
                            if (extgroup == appletype)
                            {
                                oDOAHandler._sJump = "N";
                                oDOAHandler._sHandler = GetDOAHandler_Value4(roleCode, sAPTYP, extgroup,mrpcontroller);
                                oDOAHandler._cc = GetNextCCUserID(GetDOAHandler_Value5(roleCode, sAPTYP, extgroup, mrpcontroller, oDOAHandler._sHandler));
                            }
                            else
                            {
                                if (materialtype == "ZNBW")
                                {
                                    GoJump();
                                }
                                else
                                {
                                    if (sLOC == "0072")
                                    {
                                        if (mrpcontroller == "")
                                            mrpcontroller = "NS1";
                                        oDOAHandler._sJump = "N";
                                        oDOAHandler._sHandler = GetDOAHandler_Value4(roleCode, sAPTYP, sLOC, mrpcontroller);
                                        oDOAHandler._cc = GetNextCCUserID(GetDOAHandler_Value5(roleCode, sAPTYP, sLOC, mrpcontroller, oDOAHandler._sHandler));
                                    }
                                    else
                                    {
                                        GoJump();
                                    }
                                }
                            }
                          
                            break;
                        case "IA":
                            if (extgroup == appletype)
                            {
                                oDOAHandler._sJump = "N";
                                oDOAHandler._sHandler = GetDOAHandler_Value4(roleCode, sAPTYP, extgroup, "");
                                oDOAHandler._cc = GetNextCCUserID(GetDOAHandler_Value5(roleCode, sAPTYP, extgroup, "", oDOAHandler._sHandler));
                            }
                            else  // Not Apple 
                            {
                                if (sLOC == "0072"  )
                                {
                                    if (mrpcontroller.Trim() == "")
                                        mrpcontroller = "NS1";
                                    oDOAHandler._sJump = "N";
                                    oDOAHandler._sHandler = GetDOAHandler_Value4(roleCode, sAPTYP, sLOC, mrpcontroller);
                                    oDOAHandler._cc = GetNextCCUserID(GetDOAHandler_Value5(roleCode, sAPTYP, sLOC, mrpcontroller, oDOAHandler._sHandler));
                                }
                                else
                                    GoJump();
                            }
                            break;

                    }
                    break;
                #endregion

                #region"Step NQ51-WH01"
                case "NQ51-WH01":
                    switch (sAPTYP)
                    {
                        case "I3":
                            oDOAHandler._sJump = "N";
                            oDOAHandler._sHandler = GetDOAHandler_Value4(roleCode,sAPTYP, costCenter.Substring(0,3), materialtype );
                            oDOAHandler._cc =GetNextCCUserID( GetDOAHandler_Value5(roleCode,sAPTYP, costCenter.Substring(0, 3), materialtype, oDOAHandler._sHandler));
                            break;
                        case "IE":
                            oDOAHandler._sJump = "N";
                            oDOAHandler._sHandler = GetDOAHandler_Value4(roleCode, sAPTYP,"","");
                            oDOAHandler._cc =GetNextCCUserID( GetDOAHandler_Value5(roleCode, sAPTYP, "", "", oDOAHandler._sHandler));
                            break;
                        case"IC":
                            string rcode=  reasonCode=="0021"?"0021":"";
                            oDOAHandler._sJump = "N";
                            oDOAHandler._sHandler = GetDOAHandler_Value4(roleCode, sAPTYP, rcode , "");
                            oDOAHandler._cc =GetNextCCUserID( GetDOAHandler_Value5(roleCode, sAPTYP, rcode, "", oDOAHandler._sHandler));
                            break;
                        case "I1":
                            GoJump();
                            break;
                        case "IA":
                            switch (sLOC)
                            {
                                case "0015":
                                    if (materialtype == "ZNBW")
                                    {
                                        oDOAHandler._sJump = "N";
                                        oDOAHandler._sHandler = GetDOAHandler_Value4(roleCode, sAPTYP, materialtype, sLOC);
                                        oDOAHandler._cc = GetNextCCUserID(GetDOAHandler_Value5(roleCode, sAPTYP, materialtype, sLOC, oDOAHandler._sHandler));
                                    }
                                    else
                                        GoJump();
                                    break;
                                default:
                                    GoJump();
                                    break;
                            }
                            break;
                    }
 
                    break; 
                #endregion

                #region "Step   NQ51-DEPT01"
                case "NQ51-DEPT01":
                    oDOAHandler._sJump = "N";
                    oDOAHandler._sHandler = appuser.Replace(" ", "");
                    break;
                #endregion

                #region "Step   NQ51-DEPT02"
                case "NQ51-DEPT02":
                    oDOAHandler._sJump = "N";
                    oDOAHandler._sHandler = GetDOAHandler_Value4(roleCode, costCenter, "","");
                    oDOAHandler._cc = GetNextCCUserID(GetDOAHandler_Value5(roleCode, costCenter,"", "", oDOAHandler._sHandler));
                    break;
                #endregion

                #region "Step   NQ51-PMM01"  
                case "NQ51-PMM01":
                    switch (sAPTYP)
                    {
                        case "I1":
                            if (extgroup == appletype)    //    Apple   料
                            {
                                oDOAHandler._sJump = "N";
                                oDOAHandler._sHandler = GetDOAHandler_Value4(roleCode, sAPTYP, extgroup,"");
                                oDOAHandler._cc = GetNextCCUserID(GetDOAHandler_Value5(roleCode, sAPTYP, extgroup,"", oDOAHandler._sHandler));

                            }
                            else
                            {
                                if (materialtype == "ZNBW")
                                {
                                    oDOAHandler._sJump = "N";
                                    oDOAHandler._sHandler = GetDOAHandler_Value4(roleCode, sAPTYP, materialtype, "");
                                    oDOAHandler._cc = GetNextCCUserID(GetDOAHandler_Value5(roleCode, sAPTYP, materialtype, "", oDOAHandler._sHandler));
                                }
                                else
                                {
                                    if (sLOC == "0011" || sLOC == "0072" || sLOC == "0050" || sLOC=="0018")  //
                                    {
                                        oDOAHandler._sJump = "N";
                                        oDOAHandler._sHandler = GetDOAHandler_Value4(roleCode, sAPTYP, sLOC, "");
                                        oDOAHandler._cc = GetNextCCUserID(GetDOAHandler_Value5(roleCode, sAPTYP, sLOC, "", oDOAHandler._sHandler));
                                    }
                                    else
                                    {
                                        GoJump();
                                    }

                                }
                            } 
                            break;
                        case "IA":  // IA Document
                            if (extgroup == appletype)    //    Apple   料
                            {
                                oDOAHandler._sJump = "N";
                                oDOAHandler._sHandler = GetDOAHandler_Value4(roleCode, sAPTYP, extgroup, "");
                                oDOAHandler._cc = GetNextCCUserID(GetDOAHandler_Value5(roleCode, sAPTYP, extgroup, "", oDOAHandler._sHandler));

                            }
                            else
                            {
                                if (sLOC == "0011" || sLOC == "0072" || sLOC == "0050" || sLOC == "0018")  //
                                {
                                    oDOAHandler._sJump = "N";
                                    oDOAHandler._sHandler = GetDOAHandler_Value4(roleCode, sAPTYP, sLOC, "");
                                    oDOAHandler._cc = GetNextCCUserID(GetDOAHandler_Value5(roleCode, sAPTYP, sLOC, "", oDOAHandler._sHandler));
                                }
                                else
                                {
                                    GoJump();
                                }
                            }
                            break;
                    }
                    break;
                #endregion

                #region "Step   NQ51-CU01"
                case "NQ51-CU01":
                    switch (sAPTYP)
                    {
                        case "I1":
                            if (extgroup == appletype)    //    Apple   料
                            {
                                oDOAHandler._sJump = "N";
                                oDOAHandler._sHandler = GetDOAHandler_Value4(roleCode, sAPTYP, extgroup,"");
                                oDOAHandler._cc = GetNextCCUserID(GetDOAHandler_Value5(roleCode, sAPTYP, extgroup,"", oDOAHandler._sHandler));

                            }
                            else
                            {
                                if (materialtype == "ZNBW")
                                {
                                    oDOAHandler._sJump = "N";
                                    oDOAHandler._sHandler = GetDOAHandler_Value4(roleCode, sAPTYP, materialtype, "");
                                    oDOAHandler._cc = GetNextCCUserID(GetDOAHandler_Value5(roleCode, sAPTYP, materialtype, "", oDOAHandler._sHandler));
                                }

                                else
                                {
                                    if (sLOC == "0011" || sLOC == "0072")  //
                                    {
                                        oDOAHandler._sJump = "N";
                                        oDOAHandler._sHandler = GetDOAHandler_Value4(roleCode, sAPTYP, sLOC, "");
                                        oDOAHandler._cc = GetNextCCUserID(GetDOAHandler_Value5(roleCode, sAPTYP, sLOC, "", oDOAHandler._sHandler));
                                    }
                                    else
                                    {
                                         GoJump();
                                    }

                                }
                            }
                            break;
                        default:
                            GoJump();
                            break;

                    }
                    break;
                #endregion

                #region "Step   NQ01-LMD01"
                case "NQ51-LMD01":
                    switch (sAPTYP)
                    {
                        case "IA":
       
                            if (sLOC == "0072")  //
                            {
                                oDOAHandler._sJump = "N";
                                oDOAHandler._sHandler = GetDOAHandler_Value4(roleCode, sAPTYP, sLOC, "");
                                oDOAHandler._cc = GetNextCCUserID(GetDOAHandler_Value5(roleCode, sAPTYP, sLOC, "", oDOAHandler._sHandler));
                            }
                            else
                            {
                                GoJump();
                               
                            }
                       
                            break;

                    }
                    break;
                #endregion

             }

        }

        /// <summary>ste
        /// BY CC,DEPT獲取簽核人 
        /// </summary>
        /// <param name="roleCode">關卡名</param>
        /// <param name="value1">CC</param>
        /// <param name="value2">DEPT</param>
        /// <returns>Handler</returns>
        protected string GetDOAHandler_NA(string roleCode, string value1, string value2)
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


        protected string GetDOAHandler_Value5(string roleCode, string value1, string value2,string value3,string value4)
        {
            sql = "SELECT * FROM  TB_GDS_DOA_DETAIL with(nolock) WHERE roleCode = @RoleCode AND value1 = @value1 and value2 = @value2 and value3 = @value3 and value4=@value4";
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@RoleCode", SqlDbType.VarChar, roleCode));
            opc.Add(DataPara.CreateDataParameter("@value1", SqlDbType.VarChar, value1));
            opc.Add(DataPara.CreateDataParameter("@value2", SqlDbType.VarChar, value2));
            opc.Add(DataPara.CreateDataParameter("@value3", SqlDbType.VarChar, value3));
            opc.Add(DataPara.CreateDataParameter("@value4", SqlDbType.VarChar, value4));
            return sdb.GetRowString(sql, opc, "VALUE5");
        }

        protected string GetDOAHandler_Value4(string roleCode, string value1, string value2, string value3)
        {
            sql = "SELECT * FROM  TB_GDS_DOA_DETAIL with(nolock) WHERE roleCode = @RoleCode AND value1 = @value1 and value2 = @value2 and value3 = @value3 ";
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@RoleCode", SqlDbType.VarChar, roleCode));
            opc.Add(DataPara.CreateDataParameter("@value1", SqlDbType.VarChar, value1));
            opc.Add(DataPara.CreateDataParameter("@value2", SqlDbType.VarChar, value2));
            opc.Add(DataPara.CreateDataParameter("@value3", SqlDbType.VarChar, value3));
            return sdb.GetRowString(sql, opc, "VALUE4");
        }

        /// <summary>
        /// BY CC,DEPT獲取簽核人 
        /// </summary>
        /// <param name="roleCode">關卡名</param>
        /// <param name="value1">CC</param>
        /// <param name="value2">DEPT</param>
        /// <returns>Handler</returns>
        protected string GetDOAHandler_Value4(string roleCode, string value1)
        {
            sql = "SELECT * FROM  TB_GDS_DOA_DETAIL with(nolock) WHERE roleCode = @RoleCode AND value1 = @value1";
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@RoleCode", SqlDbType.VarChar, roleCode));
            opc.Add(DataPara.CreateDataParameter("@value1", SqlDbType.VarChar, value1));
            return sdb.GetRowString(sql, opc, "VALUE4");
        }

        /// <summary>
        /// BY Wo 匯總金額 並與之標準值比較
        /// </summary>
        /// <param name="dtDetail">工單材料明細</param>
        /// <param name="moneyvlaue">金額標準值</param>
        /// <returns>true;false</returns>
        private bool CheckWoOverAmount(DataTable dtDetail,double moneyvalue)
        {
            DataView view = new DataView(dtDetail);
            string[] items = { "AUFNR" };
            DataTable tablewo = view.ToTable(true, items);

            //by Wo Summary $$
            DataTable dtResult=new DataTable();
            dtResult.Columns.Add("wo");
            dtResult.Columns.Add("amount");

            //summary field
            double moneydb=0.0;
            foreach (DataRow drItem in tablewo.Rows)
            {
                //DataRow drResult=dtResult.NewRow();
                moneydb = Convert.ToDouble(dtDetail.Compute("sum(STPRS)", "AUFNR='" + drItem["AUFNR"] + "'").ToString());
                //drResult["wo"] = drItem["AUFNR"].ToString();
                //drResult["amount"] = moneydb;
                //dtResult.Rows.Add(drResult); 
                if (moneydb > moneyvalue)
                    return true;
                 
            }
            return false;
            //DataRow[] drSelects = dtResult.Select("amount>" + moneydb.ToString());
            //if (drSelects.Length > 0)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }
        

        /// <summary>
        /// 取工單類型 
        /// </summary>
        /// <param name="wo">工單號碼</param>
        /// <returns>工單Type</returns>
        private string GetWoType(string wo)
        {
            //string laststr = wo.Substring(wo.Length - 1, 1);
            //if (isNumberic(laststr))
            //{
            //    laststr = wo.Substring(wo.Length - 2, 1);
            //    return laststr;
            //}
            //else
            //    return laststr;

            string laststr = wo.Substring(8, 1);
            return laststr;

        }

        //判定傳入值是否為數值 
        protected bool isNumberic(string message)
        {
            //判断是否为整数字符串 
            //是的话则将其转换为数字并将其设为out类型的输出值、返回true, 否则为false 
            int result = -1;   //result 定义为out 用来输出值 
            try
            {
                result = Convert.ToInt32(message);
                return true;
            }
            catch
            {
                return false;
            }
        }
        //跳關
        private void GoJump()
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

        //Get NextCC List's UserID
        private string GetNextCCUserID(string list)
        {
            string _list = list.Replace(";", "','");
            _list = "'" + _list + "'";
 
            string sql = "SELECT UserID FROM [User]  with(nolock)  WHERE LogonID in ( " + _list + ")";
            opc.Clear();
            //opc.Add(DataPara.CreateDataParameter("@List", SqlDbType.VarChar, _list ));
            DataTable dt= sdb.GetDataTable(sql,opc);
            _list = "";
            foreach (DataRow drItem in dt.Rows)
            {
                if (_list.Length==0)
                    _list =  drItem["UserID"].ToString();
                else
                    _list = _list + "," + drItem["UserID"].ToString();
            }
            return _list;
            
        }




    }
}
