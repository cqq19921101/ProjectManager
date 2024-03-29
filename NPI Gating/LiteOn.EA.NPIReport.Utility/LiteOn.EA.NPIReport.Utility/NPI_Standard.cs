using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Liteon.ICM.DataCore;
using LiteOn.EA.CE.Utility;
using LiteOn.EA.ORG.Utility;
using LiteOn.EA.Borg.Utility;
using System.Xml;
using System.IO;
using LiteOn.EA.BLL;
namespace LiteOn.EA.NPIReport.Utility
{
    /// <summary>
    /// Summary description for Leave_Standard.
    /// </summary>
    public class NPI_Standard
    {

       
        protected string _BU;
        protected string _SITE;
        public NPI_Standard(string site, string bu)
        {
            _BU = bu;
            _SITE = site;
        }

        #region[獲取參數表基本設定]
        /// <summary>
        /// [ 獲取參數表設定參數]
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public DataTable GetParameter(Dictionary<string, string> key)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM TB_APPLICATION_PARAM  with(nolock) WHERE 1=1 AND ENABLED='Y' ");
            ArrayList opc = new ArrayList();
            opc.Clear();
            foreach (KeyValuePair<string, string> rows in key)
            {
                if (rows.Value.Length > 0)
                {
                    sql.Append(string.Format("AND {0}=@{1} ", rows.Key, rows.Key));
                    opc.Add(DataPara.CreateDataParameter("@" + rows.Key, DbType.String, rows.Value));
                }
            }
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            return sdb.TransactionExecute(sql.ToString(), opc);
        }

        public DataTable GetSysParameter(Dictionary<string, string> key)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM TB_DFX_PARAM  with(nolock) WHERE 1=1 ");
            ArrayList opc = new ArrayList();
            opc.Clear();
            foreach (KeyValuePair<string, string> rows in key)
            {
                if (rows.Value.Length > 0)
                {
                    sql.Append(string.Format("AND {0}=@{1} ", rows.Key, rows.Key));
                    opc.Add(DataPara.CreateDataParameter("@" + rows.Key, DbType.String, rows.Value));
                }
            }
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            return sdb.TransactionExecute(sql.ToString(), opc);
        }

        public DataTable GetNPIMemeberList(Model_NPI_MEMBER oModel_NPI_Member)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt=new DataTable();
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            sb.Append("SELECT  * FROM TB_NPI_MEMBER With(nolock) where 1=1");
            sb.Append(" AND BU=@BU and BUILDING=@Building");
            ArrayList opc = new ArrayList();
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@BU", DbType.String, oModel_NPI_Member._BU));
            opc.Add(DataPara.CreateDataParameter("@Building", DbType.String, oModel_NPI_Member._BUILDING));
            if(!string.IsNullOrEmpty(oModel_NPI_Member._CATEGORY))
            {
                 sb.Append(" AND CATEGORY=@Catergory ");
                 opc.Add(DataPara.CreateDataParameter("@Catergory", DbType.String, oModel_NPI_Member._CATEGORY));
            }
            if(!string.IsNullOrEmpty(oModel_NPI_Member._DEPT))
            {
                 sb.Append(" AND DEPT=@DEPT ");
                 opc.Add(DataPara.CreateDataParameter("@DEPT", DbType.String, oModel_NPI_Member._DEPT));
            }
     
             dt=sdb.TransactionExecute(sb.ToString(),opc);
             return dt;


        }
        
        /// <summary>
        /// 取Category detail parameter by  category
        /// </summary>
        /// <returns></returns>

        public DataTable PARAME_GetBasicData_Filter(Model_APPLICATION_PARAM oModel_Application_Parma)
        {
            Dictionary<string, string> filterKey = new Dictionary<string, string>();
            filterKey["APPLICATION_NAME"] = oModel_Application_Parma._APPLICATION_NAME;
            filterKey["FUNCTION_NAME"] = oModel_Application_Parma._FUNCTION_NAME;
            filterKey["PARAME_NAME"] = oModel_Application_Parma._PARAME_NAME;
            filterKey["PARAME_ITEM"] = oModel_Application_Parma._PARAME_ITEM;
            filterKey["PARAME_VALUE1"] = oModel_Application_Parma._PARAME_VALUE1;
            filterKey["PARAME_VALUE2"] = oModel_Application_Parma._PARAME_VALUE2;
            filterKey["PARAME_VALUE3"] = oModel_Application_Parma._PARAME_VALUE3;
            return GetParameter(filterKey);
        }

        public DataTable PARAME_GetBasicData_Filter(Model_DFX_PARAM oModel_DFX_PARAM)
        {
            Dictionary<string, string> filterKey = new Dictionary<string, string>();

            filterKey["FUNCTION_NAME"] = oModel_DFX_PARAM._FUNCTION_NAME;
            filterKey["PARAME_NAME"] = oModel_DFX_PARAM._PARAME_NAME;
            filterKey["PARAME_ITEM"] = oModel_DFX_PARAM._PARAME_ITEM;
            filterKey["PARAME_VALUE1"] = oModel_DFX_PARAM._PARAME_VALUE1;
            filterKey["PARAME_VALUE2"] = oModel_DFX_PARAM._PARAME_VALUE2;
            filterKey["PARAME_VALUE3"] = oModel_DFX_PARAM._PARAME_VALUE3;
            filterKey["Building"] = oModel_DFX_PARAM._Building;
            
            return GetSysParameter(filterKey);
        }

        #endregion
             
        #region 数据操作

        /// <summary>
        /// 數據操作
        /// </summary>

        /// <returns></returns>
        public Dictionary<string, object> PRAME_RecordOperation(Model_DFX_PARAM oModel_DFX_PARAM, Status_Operation RecordOperation)
        {

            Dictionary<string, object> result = new Dictionary<string, object>();
            result["ErrMsg"] = string.Empty;
            result["Result"] = false;

            try
            {

                ArrayList opc = new ArrayList();
                opc.Add(DataPara.CreateDataParameter("@P_OP_TYPE", DbType.String, RecordOperation.ToString(), ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_OP_ID", DbType.String, oModel_DFX_PARAM._ID, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_FUNCTION_NAME", DbType.String, oModel_DFX_PARAM._FUNCTION_NAME, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_PARAME_NAME", DbType.String, oModel_DFX_PARAM._PARAME_NAME, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_PARAME_ITEM", DbType.String, oModel_DFX_PARAM._PARAME_ITEM, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_PARAME_VALUE1", DbType.String, oModel_DFX_PARAM._PARAME_VALUE1, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_PARAME_VALUE2", DbType.String, oModel_DFX_PARAM._PARAME_VALUE2, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_PARAME_VALUE3", DbType.String, oModel_DFX_PARAM._PARAME_VALUE3, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_PARAME_VALUE4", DbType.String, oModel_DFX_PARAM._PARAME_VALUE4, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_PARAME_VALUE5", DbType.String, oModel_DFX_PARAM._PARAME_VALUE5, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_Building", DbType.String, oModel_DFX_PARAM._Building, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_UPDATE_USER", DbType.String, oModel_DFX_PARAM._UPDATE_USER, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_UPDATE_TIME", DbType.DateTime, oModel_DFX_PARAM._UPDATE_TIME, ParameterDirection.Input, 8));
                opc.Add(DataPara.CreateDataParameter("@P_PARAME_TYPE", DbType.String, oModel_DFX_PARAM._PARAME_TYPE, ParameterDirection.Input,2));
                opc.Add(DataPara.CreateDataParameter("@Result", DbType.String, null, ParameterDirection.Output, 1000));
                SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
                Hashtable htResult = sdb.ExecuteProc("P_OP_DFX_PARAM", opc);
                //SP執行結果返回固定格式
                //1 OK; 
                //2 NG;ERR MSG
                string tmp = htResult["@Result"].ToString();
                if (tmp.Length >= 3)
                {
                    if (tmp.Substring(0, 2) == "NG")
                    {
                        //接收SP返回的ERR MSG
                        result["ErrMsg"] = tmp.Substring(3, tmp.Length - 3);
                    }
                    if (tmp.Substring(0, 2) == "OK")
                    {
                        result["Result"] = true;
                    }
                }
                else
                {

                    result["ErrMsg"] = "DB ERROR,Pls contact IT";
                }

            }
            catch (Exception ex)
            {
                result["ErrMsg"] = "DB ERROR:" + ex.Message;
            }
            return result;
        }


        public void Role_Operation(string role_id, string user_id, string Type, string UpdateUser)
        {
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("SPM"));
            string sql = string.Empty;
            string sqlInsert = string.Empty;
            ArrayList opc = new ArrayList();
            if (Type == "Add")
            {
                sql = "SELECT ROLE_ID FROM TB_ROLE_LINK WHERE ROLE_ID = @ROLE_ID AND LOGON_ID = @LOGON_ID ";
                opc.Clear();
                opc.Add(DataPara.CreateDataParameter("@ROLE_ID", DbType.Int32, int.Parse(role_id)));
                opc.Add(DataPara.CreateDataParameter("@LOGON_ID", DbType.String, user_id));
                if (sdb.TransactionExecute(sql, opc).Rows.Count <= 0)
                {
                    string sql1 = "(@ROLE_ID, @LOGON_ID,  @UPDATE_USERID, @UPDATE_TIME)";
                    sql = "INSERT INTO TB_ROLE_LINK" + sql1.Replace("@", "") + " VALUES " + sql1;
                    opc.Clear();
                    opc.Add(DataPara.CreateDataParameter("@ROLE_ID", DbType.Int32, int.Parse(role_id)));
                    opc.Add(DataPara.CreateDataParameter("@LOGON_ID", DbType.String, user_id));
                    opc.Add(DataPara.CreateDataParameter("@UPDATE_USERID", DbType.String, UpdateUser));
                    opc.Add(DataPara.CreateDataParameter("@UPDATE_TIME", DbType.DateTime, DateTime.Now));
                    try
                    {
                        sdb.TransactionExecuteNonQuery(sql, opc);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
            else if (Type == "Delete")
            {
                sql = "DELETE TB_ROLE_LINK WHERE ROLE_ID=@ROLE_ID AND LOGON_ID=@LOGON_ID";
                opc.Clear();
                opc.Add(DataPara.CreateDataParameter("@ROLE_ID", DbType.Int32, int.Parse(role_id)));
                opc.Add(DataPara.CreateDataParameter("@LOGON_ID", DbType.String, user_id));

                try
                {
                    sdb.TransactionExecuteNonQuery(sql, opc);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

        }


        /// <summary>
        /// 數據操作
        /// </summary>

        /// <returns></returns>
        public virtual Dictionary<string, object> RecordOperation_NPIMember(Model_NPI_MEMBER oModel_NPI_MEMBER, Status_Operation RecordOperation)
        {

            Dictionary<string, object> result = new Dictionary<string, object>();
            result["ErrMsg"] = string.Empty;
            result["Result"] = false;

            try
            {

                ArrayList opc = new ArrayList();
                opc.Add(DataPara.CreateDataParameter("@P_OP_TYPE", DbType.String, RecordOperation.ToString(), ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_ID", DbType.String, oModel_NPI_MEMBER._ID, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_BU", DbType.String, oModel_NPI_MEMBER._BU, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_BUILDING", DbType.String, oModel_NPI_MEMBER._BUILDING, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_CATEGORY", DbType.String, oModel_NPI_MEMBER._CATEGORY, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_DEPT", DbType.String, oModel_NPI_MEMBER._DEPT, ParameterDirection.Input, 100));
                opc.Add(DataPara.CreateDataParameter("@P_ENAME", DbType.String, oModel_NPI_MEMBER._ENAME, ParameterDirection.Input, 30));
                opc.Add(DataPara.CreateDataParameter("@P_CNAME", DbType.String, oModel_NPI_MEMBER._CNAME, ParameterDirection.Input, 40));
                opc.Add(DataPara.CreateDataParameter("@P_EMAIL", DbType.String, oModel_NPI_MEMBER._EMAIL, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_UPDATE_TIME", DbType.DateTime, oModel_NPI_MEMBER._UPDATE_TIME, ParameterDirection.Input, 8));
                opc.Add(DataPara.CreateDataParameter("@P_UPDATE_USERID", DbType.String, oModel_NPI_MEMBER._UPDATE_USERID, ParameterDirection.Input, 30));
                opc.Add(DataPara.CreateDataParameter("@Result", DbType.String, null, ParameterDirection.Output, 1000));
                SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
                Hashtable htResult = sdb.ExecuteProc("P_OP_NPI_MEMBER", opc);
                //SP執行結果返回固定格式
                //1 OK; 
                //2 NG;ERR MSG
                string tmp = htResult["@Result"].ToString();
                if (tmp.Length >= 3)
                {
                    if (tmp.Substring(0, 2) == "NG")
                    {
                        //接收SP返回的ERR MSG
                        result["ErrMsg"] = tmp.Substring(3, tmp.Length - 3);
                    }
                    if (tmp.Substring(0, 2) == "OK")
                    {
                        result["Result"] = true;
                    }
                }
                else
                {

                    result["ErrMsg"] = "DB ERROR,Pls contact IT";
                }

            }
            catch (Exception ex)
            {
                result["ErrMsg"] = "DB ERROR:" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 數據操作
        /// </summary>

        /// <returns></returns>
        public Dictionary<string, object> RecordOperation_APPMemeber(Model_NPI_APP_MEMBER oModel_NPI_APP_MEMBER, Status_Operation RecordOperation)
        {

            Dictionary<string, object> result = new Dictionary<string, object>();
            result["ErrMsg"] = string.Empty;
            result["Result"] = false;

            try
            {

                ArrayList opc = new ArrayList();
                opc.Add(DataPara.CreateDataParameter("@P_OP_TYPE", DbType.String, RecordOperation.ToString(), ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_ID", DbType.Int32, oModel_NPI_APP_MEMBER._ID, ParameterDirection.Input, 4));
                opc.Add(DataPara.CreateDataParameter("@P_DOC_NO", DbType.String, oModel_NPI_APP_MEMBER._DOC_NO, ParameterDirection.Input, 30));
                opc.Add(DataPara.CreateDataParameter("@P_Category", DbType.String, oModel_NPI_APP_MEMBER._Category, ParameterDirection.Input, 30));
                opc.Add(DataPara.CreateDataParameter("@P_DEPT", DbType.String, oModel_NPI_APP_MEMBER._DEPT, ParameterDirection.Input, 100));
                opc.Add(DataPara.CreateDataParameter("@P_WriteEname", DbType.String, oModel_NPI_APP_MEMBER._WriteEname, ParameterDirection.Input, 30));
                opc.Add(DataPara.CreateDataParameter("@P_WriteCname", DbType.String, oModel_NPI_APP_MEMBER._WriteCname, ParameterDirection.Input, 40));
                opc.Add(DataPara.CreateDataParameter("@P_WriteEmail", DbType.String, oModel_NPI_APP_MEMBER._WriteEmail, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_ReplyEName", DbType.String, oModel_NPI_APP_MEMBER._ReplyEName, ParameterDirection.Input, 30));
                opc.Add(DataPara.CreateDataParameter("@P_ReplyCname", DbType.String, oModel_NPI_APP_MEMBER._ReplyCname, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_ReplyEmai", DbType.String, oModel_NPI_APP_MEMBER._ReplyEmai, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_CheckedEname", DbType.String, oModel_NPI_APP_MEMBER._CheckedEname, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_CheckedEmai", DbType.String, oModel_NPI_APP_MEMBER._CheckedEmail, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_CategoryFlag", DbType.String, oModel_NPI_APP_MEMBER._CategoryFlag, ParameterDirection.Input,1));
                opc.Add(DataPara.CreateDataParameter("@P_Flag", DbType.String, "", ParameterDirection.Input, 2));

                opc.Add(DataPara.CreateDataParameter("@P_UPDATE_TIME", DbType.DateTime, oModel_NPI_APP_MEMBER._UPDATE_TIME, ParameterDirection.Input, 8));
                opc.Add(DataPara.CreateDataParameter("@P_UPDATE_USERID", DbType.String, oModel_NPI_APP_MEMBER._UPDATE_USERID, ParameterDirection.Input, 30));

                opc.Add(DataPara.CreateDataParameter("@Result", DbType.String, null, ParameterDirection.Output, 1000));
                SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
                Hashtable htResult = sdb.ExecuteProc("P_OP_NPI_APP_MEMBER", opc);
                //SP執行結果返回固定格式
                //1 OK; 
                //2 NG;ERR MSG
                string tmp = htResult["@Result"].ToString();
                if (tmp.Length >= 3)
                {
                    if (tmp.Substring(0, 2) == "NG")
                    {
                        //接收SP返回的ERR MSG
                        result["ErrMsg"] = tmp.Substring(3, tmp.Length - 3);
                    }
                    if (tmp.Substring(0, 2) == "OK")
                    {
                        result["Result"] = true;
                    }
                }
                else
                {

                    result["ErrMsg"] = "DB ERROR,Pls contact IT";
                }

            }
            catch (Exception ex)
            {
                result["ErrMsg"] = "DB ERROR:" + ex.Message;
            }
            return result;
        }

        public Dictionary<string, object> RecordOperation_APPIssueList(Model_NPI_APP_ISSUELIST oModel_NPI_APP_ISSUELIST, Status_Operation RecordOperation)
        {

            Dictionary<string, object> result = new Dictionary<string, object>();
            result["ErrMsg"] = string.Empty;
            result["Result"] = false;

            try
            {

                ArrayList opc = new ArrayList();
                opc.Add(DataPara.CreateDataParameter("@P_OP_TYPE", DbType.String, RecordOperation.ToString(), ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_ID", DbType.String, oModel_NPI_APP_ISSUELIST._ID, ParameterDirection.Input, 30));
                opc.Add(DataPara.CreateDataParameter("@P_DOC_NO", DbType.String, oModel_NPI_APP_ISSUELIST._DOC_NO, ParameterDirection.Input, 30));
                opc.Add(DataPara.CreateDataParameter("@P_SUB_DOC_NO", DbType.String, oModel_NPI_APP_ISSUELIST._SUB_DOC_NO, ParameterDirection.Input, 30));
                opc.Add(DataPara.CreateDataParameter("@P_PHASE", DbType.String, oModel_NPI_APP_ISSUELIST._PHASE, ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_STATION", DbType.String, oModel_NPI_APP_ISSUELIST._STATION, ParameterDirection.Input, 100));
                opc.Add(DataPara.CreateDataParameter("@P_ITEMS", DbType.String, oModel_NPI_APP_ISSUELIST._ITEMS, ParameterDirection.Input, 100));
                opc.Add(DataPara.CreateDataParameter("@P_PRIORITYLEVEL", DbType.String, oModel_NPI_APP_ISSUELIST._PRIORITYLEVEL, ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_ISSUE_DESCRIPTION", DbType.String, oModel_NPI_APP_ISSUELIST._ISSUE_DESCRIPTION, ParameterDirection.Input, 400));
                opc.Add(DataPara.CreateDataParameter("@P_ISSUE_LOSSES", DbType.String, oModel_NPI_APP_ISSUELIST._ISSUE_LOSSES, ParameterDirection.Input, 100));
                opc.Add(DataPara.CreateDataParameter("@P_TEMP_MEASURE", DbType.String, oModel_NPI_APP_ISSUELIST._TEMP_MEASURE, ParameterDirection.Input, 100));
                opc.Add(DataPara.CreateDataParameter("@P_IMPROVE_MEASURE", DbType.String, oModel_NPI_APP_ISSUELIST._IMPROVE_MEASURE, ParameterDirection.Input, 2000));
                opc.Add(DataPara.CreateDataParameter("@P_PERSON_IN_CHARGE", DbType.String, oModel_NPI_APP_ISSUELIST._PERSON_IN_CHARGE, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_DUE_DAY", DbType.String, oModel_NPI_APP_ISSUELIST._DUE_DAY, ParameterDirection.Input, 200));
                opc.Add(DataPara.CreateDataParameter("@P_CURRENT_STATUS", DbType.String,Helper.ConvertChinese(oModel_NPI_APP_ISSUELIST._CURRENT_STATUS,"Big5"),ParameterDirection.Input,2000));
                opc.Add(DataPara.CreateDataParameter("@P_AFFIRMACE_MAN", DbType.String, oModel_NPI_APP_ISSUELIST._AFFIRMACE_MAN, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_STAUTS", DbType.String, oModel_NPI_APP_ISSUELIST._STAUTS, ParameterDirection.Input, 200));
                opc.Add(DataPara.CreateDataParameter("@P_TRACKING", DbType.String, oModel_NPI_APP_ISSUELIST._TRACKING, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_REMARK", DbType.String, oModel_NPI_APP_ISSUELIST._REMARK, ParameterDirection.Input, 40));
                opc.Add(DataPara.CreateDataParameter("@P_DEPT", DbType.String, oModel_NPI_APP_ISSUELIST._DEPT, ParameterDirection.Input, 40));
                opc.Add(DataPara.CreateDataParameter("@P_CREATE_TIME", DbType.DateTime, oModel_NPI_APP_ISSUELIST._CREATE_TIME, ParameterDirection.Input, 8));
                opc.Add(DataPara.CreateDataParameter("@P_CREATE_USERID", DbType.String, oModel_NPI_APP_ISSUELIST._CREATE_USERID, ParameterDirection.Input, 30));
                opc.Add(DataPara.CreateDataParameter("@P_UPDATE_TIME", DbType.DateTime, oModel_NPI_APP_ISSUELIST._UPDATE_TIME, ParameterDirection.Input, 8));
                opc.Add(DataPara.CreateDataParameter("@P_UPDATE_USERID", DbType.String, oModel_NPI_APP_ISSUELIST._UPDATE_USERID, ParameterDirection.Input, 30));
                opc.Add(DataPara.CreateDataParameter("@P_FILENAME", DbType.String, oModel_NPI_APP_ISSUELIST._FILENAME, ParameterDirection.Input,50));
                opc.Add(DataPara.CreateDataParameter("@P_FILEPATH", DbType.String, oModel_NPI_APP_ISSUELIST._FILEPATH, ParameterDirection.Input,100));
                opc.Add(DataPara.CreateDataParameter("@P_CLASS", DbType.String, oModel_NPI_APP_ISSUELIST._CLASS, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_MEASURE_DEPTREPLY", DbType.String, oModel_NPI_APP_ISSUELIST._MEASURE_DEPTREPLY, ParameterDirection.Input,2000));
                                                                                  
                opc.Add(DataPara.CreateDataParameter("@Result", DbType.String, null, ParameterDirection.Output, 1000));
                SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
                Hashtable htResult = sdb.ExecuteProc("P_OP_NPI_APP_ISSUELIST", opc);
                //SP執行結果返回固定格式
                //1 OK; 
                //2 NG;ERR MSG
                string tmp = htResult["@Result"].ToString();
                if (tmp.Length >= 3)
                {
                    if (tmp.Substring(0, 2) == "NG")
                    {
                        //接收SP返回的ERR MSG
                        result["ErrMsg"] = tmp.Substring(3, tmp.Length - 3);
                    }
                    if (tmp.Substring(0, 2) == "OK")
                    {
                        result["Result"] = true;
                    }
                }
                else
                {

                    result["ErrMsg"] = "DB ERROR,Pls contact IT";
                }

            }
            catch (Exception ex)
            {
                result["ErrMsg"] = "DB ERROR:" + ex.Message;
            }
            return result;
        }

        public Dictionary<string, object> RecordOperation_APP_SUB(Model_NPI_APP_SUB oModel_NPI_APP_SUB, LiteOn.EA.Borg.Utility.Status_Operation RecordOperation)
        {

            Dictionary<string, object> result = new Dictionary<string, object>();
            result["ErrMsg"] = string.Empty;
            result["Result"] = false;

            try
            {

                ArrayList opc = new ArrayList();
                opc.Add(DataPara.CreateDataParameter("@P_OP_TYPE", DbType.String, RecordOperation.ToString(), ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_DOC_NO", DbType.String, oModel_NPI_APP_SUB._DOC_NO, ParameterDirection.Input, 30));
                opc.Add(DataPara.CreateDataParameter("@P_SUB_DOC_NO", DbType.String, oModel_NPI_APP_SUB._SUB_DOC_NO, ParameterDirection.Input, 30));
                opc.Add(DataPara.CreateDataParameter("@P_SUB_DOC_PHASE", DbType.String, oModel_NPI_APP_SUB._SUB_DOC_PHASE, ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_SUB_DOC_PHASE_A", DbType.String, oModel_NPI_APP_SUB._SUB_DOC_PHASE_A, ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_WorkOrder", DbType.String, oModel_NPI_APP_SUB._WorkOrder, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_SUB_DOC_PHASE_RESULT", DbType.String, oModel_NPI_APP_SUB._SUB_DOC_PHASE_RESULT, ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_SUB_DOC_PHASE_STATUS", DbType.String, oModel_NPI_APP_SUB._SUB_DOC_PHASE_STATUS, ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_SUB_DOC_PHASE_VERSION", DbType.Int32, oModel_NPI_APP_SUB._SUB_DOC_PHASE_VERSION, ParameterDirection.Input, 4));
                opc.Add(DataPara.CreateDataParameter("@P_UPDATE_TIME", DbType.DateTime, oModel_NPI_APP_SUB._UPDATE_TIME, ParameterDirection.Input, 8));
                opc.Add(DataPara.CreateDataParameter("@P_UPDATE_USERID", DbType.String, oModel_NPI_APP_SUB._UPDATE_USERID, ParameterDirection.Input, 30));
                //opc.Add(DataPara.CreateDataParameter("@P_DFX_STATUS", DbType.String, oModel_NPI_APP_SUB._DFX_STATUS, ParameterDirection.Input, 10));

                //opc.Add(DataPara.CreateDataParameter("@P_CTQ_STATUS", DbType.String, oModel_NPI_APP_SUB._CTQ_STATUS, ParameterDirection.Input, 10));
                //opc.Add(DataPara.CreateDataParameter("@P_CLCA_STATUS", DbType.String, oModel_NPI_APP_SUB._CLCA_STATUS, ParameterDirection.Input, 10));
                //opc.Add(DataPara.CreateDataParameter("@P_ISSUES_STATUS", DbType.String, oModel_NPI_APP_SUB._ISSUES_STATUS, ParameterDirection.Input, 10));
                //opc.Add(DataPara.CreateDataParameter("@P_PFMA_STATUS", DbType.String, oModel_NPI_APP_SUB._PFMA_STATUS, ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_CREATE_DATE", DbType.DateTime, oModel_NPI_APP_SUB._CREATE_DATE, ParameterDirection.Input, 8));
                opc.Add(DataPara.CreateDataParameter("@P_Building", DbType.String, oModel_NPI_APP_SUB._Building, ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_CTQ_QTY", DbType.Int32, oModel_NPI_APP_SUB._CTQ_QTY, ParameterDirection.Input, 4));
                opc.Add(DataPara.CreateDataParameter("@P_CLCA_QTY", DbType.Int32, oModel_NPI_APP_SUB._CLCA_QTY, ParameterDirection.Input, 4));
                opc.Add(DataPara.CreateDataParameter("@P_CLCA_BEGIN_TIME", DbType.String, oModel_NPI_APP_SUB._CLCA_BEGIN_TIME, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_CLCA_END_TIME", DbType.String, oModel_NPI_APP_SUB._CLCA_END_TIME, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_LOT_QTY", DbType.Int32, oModel_NPI_APP_SUB._LOT_QTY, ParameterDirection.Input, 4));
                opc.Add(DataPara.CreateDataParameter("@P_PCB_REV", DbType.String, oModel_NPI_APP_SUB._PCB_REV, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_SPEC_REV", DbType.String, oModel_NPI_APP_SUB._SPEC_REV, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_ISSUE_DATE", DbType.DateTime, oModel_NPI_APP_SUB._ISSUE_DATE, ParameterDirection.Input, 8));
                opc.Add(DataPara.CreateDataParameter("@P_INPUT_DATE", DbType.String, oModel_NPI_APP_SUB._INPUT_DATE, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_CUSTOMER", DbType.String, oModel_NPI_APP_SUB._CUSTOMER, ParameterDirection.Input, 100));
                opc.Add(DataPara.CreateDataParameter("@P_LINE", DbType.String, oModel_NPI_APP_SUB._LINE, ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_BOM_REV", DbType.String, oModel_NPI_APP_SUB._BOM_REV, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_CUSTOMER_REV", DbType.String, oModel_NPI_APP_SUB._CUSTOMER_REV, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_RELEASET_DATE", DbType.String, oModel_NPI_APP_SUB._RELEASET_DATE, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_PK_DATE", DbType.String, oModel_NPI_APP_SUB._PK_DATE, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_NeedStartItmes", DbType.String, oModel_NPI_APP_SUB._NeedStartItmes, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_GROD_GROUP", DbType.String, oModel_NPI_APP_SUB._PROD_GROUP, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_CASEID", DbType.Int32, oModel_NPI_APP_SUB._CASEID, ParameterDirection.Input,4));
                opc.Add(DataPara.CreateDataParameter("@P_REMARKM", DbType.String, Helper.ConvertChinese(oModel_NPI_APP_SUB._REMARKM,"Big5") , ParameterDirection.Input, 2000));
                opc.Add(DataPara.CreateDataParameter("@P_MODIFYFLAG", DbType.String, oModel_NPI_APP_SUB._MODIFYFLAG, ParameterDirection.Input, 1));



                opc.Add(DataPara.CreateDataParameter("@Result", DbType.String, null, ParameterDirection.Output, 1000));
                SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
                Hashtable htResult = sdb.ExecuteProc("P_OP_NPI_APP_SUB", opc);
                //SP執行結果返回固定格式
                //1 OK; 
                //2 NG;ERR MSG
                string tmp = htResult["@Result"].ToString();
                if (tmp.Length >= 3)
                {
                    if (tmp.Substring(0, 2) == "NG")
                    {
                        //接收SP返回的ERR MSG
                        result["ErrMsg"] = tmp.Substring(3, tmp.Length - 3);
                    }
                    if (tmp.Substring(0, 2) == "OK")
                    {
                        result["Result"] = true;
                    }
                }
                else
                {

                    result["ErrMsg"] = "DB ERROR,Pls contact IT";
                }

            }
            catch (Exception ex)
            {
                result["ErrMsg"] = "DB ERROR:" + ex.Message;
            }
            return result;
        }

        public string RecordOperation_DFXItemUpload(Model_DFX_Item oModel_DFX_ITEMUpload, Status_Operation RecordOperation)
        {

            string result = string.Empty;


            try
            {

                ArrayList opc = new ArrayList();
                opc.Add(DataPara.CreateDataParameter("@P_OP_TYPE", DbType.String, RecordOperation.ToString(), ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_BU", DbType.String, oModel_DFX_ITEMUpload._BU, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_Building", DbType.String, oModel_DFX_ITEMUpload._BUILDING, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_ItemID", DbType.String, oModel_DFX_ITEMUpload._ItemID, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_Item", DbType.String, oModel_DFX_ITEMUpload._Item, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_ItemType", DbType.String, Helper.ConvertChinese(oModel_DFX_ITEMUpload._ItemType, "Big5"), ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_ItemName", DbType.String, Helper.ConvertChinese(oModel_DFX_ITEMUpload._ItemName,"Big5"), ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_Requirements", DbType.String, Helper.ConvertChinese(oModel_DFX_ITEMUpload._Requirements,"Big5"), ParameterDirection.Input, 400));
                opc.Add(DataPara.CreateDataParameter("@P_ProductType", DbType.String, oModel_DFX_ITEMUpload._ProductType, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_PriorityLevel", DbType.Int16, oModel_DFX_ITEMUpload._PriorityLevel, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_Losses", DbType.String, oModel_DFX_ITEMUpload._Losses, ParameterDirection.Input, 400));
                opc.Add(DataPara.CreateDataParameter("@P_WriteDept", DbType.String, oModel_DFX_ITEMUpload._WriteDept, ParameterDirection.Input, 400));
                opc.Add(DataPara.CreateDataParameter("@P_ReplyDept", DbType.String, oModel_DFX_ITEMUpload._ReplyDept, ParameterDirection.Input, 40));
                opc.Add(DataPara.CreateDataParameter("@P_OldItemType", DbType.String, Helper.ConvertChinese(oModel_DFX_ITEMUpload._OldItemType,"Big5"), ParameterDirection.Input, 400));

                opc.Add(DataPara.CreateDataParameter("@Result", DbType.String, null, ParameterDirection.Output, 1000));
                SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
                Hashtable htResult = sdb.ExecuteProc("P_OP_DFX_ITEMUpload", opc);

                result = htResult["@Result"].ToString();


            }
            catch (Exception ex)
            {
                result = "DB ERROR:" + ex.Message;
            }
            return result;
        }

        public string  RecordOperation_DFXItems(Model_DFX_ITEMBODY oModel_DFX_ITEMBODY, Status_Operation RecordOperation)
        {

            string result = string.Empty;
          

            try
            {

                ArrayList opc = new ArrayList();
                opc.Add(DataPara.CreateDataParameter("@P_OP_TYPE", DbType.String, RecordOperation.ToString(), ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_DFXNo", DbType.String, oModel_DFX_ITEMBODY._DFXNo, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_Item", DbType.String, oModel_DFX_ITEMBODY._Item, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_ItemType", DbType.String, oModel_DFX_ITEMBODY._ItemType, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_ItemName", DbType.String, oModel_DFX_ITEMBODY._ItemName, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_Requirements", DbType.String, oModel_DFX_ITEMBODY._Requirements, ParameterDirection.Input, 400));
                opc.Add(DataPara.CreateDataParameter("@P_Losses", DbType.String, oModel_DFX_ITEMBODY._Losses, ParameterDirection.Input, 400));
                opc.Add(DataPara.CreateDataParameter("@P_Location", DbType.String, oModel_DFX_ITEMBODY._Location, ParameterDirection.Input, 400));

                opc.Add(DataPara.CreateDataParameter("@P_Severity", DbType.String, oModel_DFX_ITEMBODY._Severity, ParameterDirection.Input, 40));
                opc.Add(DataPara.CreateDataParameter("@P_Occurrence", DbType.String, oModel_DFX_ITEMBODY._Occurrence, ParameterDirection.Input, 40));
                opc.Add(DataPara.CreateDataParameter("@P_Detection", DbType.String, oModel_DFX_ITEMBODY._Detection, ParameterDirection.Input, 40));
                opc.Add(DataPara.CreateDataParameter("@P_RPN", DbType.String, oModel_DFX_ITEMBODY._RPN, ParameterDirection.Input, 40));
                opc.Add(DataPara.CreateDataParameter("@P_Class", DbType.String, oModel_DFX_ITEMBODY._Class, ParameterDirection.Input, 40));
                opc.Add(DataPara.CreateDataParameter("@P_PriorityLevel", DbType.String, oModel_DFX_ITEMBODY._PriorityLevel, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_MaxPoints", DbType.String, oModel_DFX_ITEMBODY._MaxPoints, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_DFXPoints", DbType.String, oModel_DFX_ITEMBODY._DFXPoints, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_WriteDept", DbType.String, oModel_DFX_ITEMBODY._WriteDept, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_Compliance", DbType.String, oModel_DFX_ITEMBODY._Compliance, ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_Comments", DbType.String, oModel_DFX_ITEMBODY._Comments, ParameterDirection.Input, 400));
                opc.Add(DataPara.CreateDataParameter("@P_Status", DbType.String, oModel_DFX_ITEMBODY._Status, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_UpdateUser", DbType.String, oModel_DFX_ITEMBODY._UpdateUser, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_UpdateTime", DbType.DateTime, oModel_DFX_ITEMBODY._UpdateTime, ParameterDirection.Input, 8));

                opc.Add(DataPara.CreateDataParameter("@P_Actions", DbType.String, oModel_DFX_ITEMBODY._Actions, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_CompleteDate", DbType.String, oModel_DFX_ITEMBODY._CompletionDate, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_Remark", DbType.String, oModel_DFX_ITEMBODY._Remark, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_Tracking", DbType.String, oModel_DFX_ITEMBODY._Tracking, ParameterDirection.Input, 50));

                opc.Add(DataPara.CreateDataParameter("@Result", DbType.String, null, ParameterDirection.Output, 1000));
                SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
                Hashtable htResult = sdb.ExecuteProc("P_OP_DFX_ITEMBODY", opc);
             
                result = htResult["@Result"].ToString();
                

            }
            catch (Exception ex)
            {
                result = "DB ERROR:" + ex.Message;
            }
            return result;
        }

        public string RecordOperation_CTQInfo(Model_NPI_APP_CTQ oModel_NPI_APP_CTQ, Status_Operation RecordOperation)
        {

            string result = string.Empty;
          

            try
            {

                ArrayList opc = new ArrayList();
                opc.Add(DataPara.CreateDataParameter("@P_OP_TYPE", DbType.String, RecordOperation.ToString(), ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_ID", DbType.Int32, oModel_NPI_APP_CTQ._ID, ParameterDirection.Input, 30));
                opc.Add(DataPara.CreateDataParameter("@P_DOC_NO", DbType.String, oModel_NPI_APP_CTQ._DOC_NO, ParameterDirection.Input, 30));
                opc.Add(DataPara.CreateDataParameter("@P_SUB_DOC_NO", DbType.String, oModel_NPI_APP_CTQ._SUB_DOC_NO, ParameterDirection.Input, 30));
                opc.Add(DataPara.CreateDataParameter("@P_PROD_GROUP", DbType.String, oModel_NPI_APP_CTQ._PROD_GROUP, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_PHASE", DbType.String, oModel_NPI_APP_CTQ._PHASE, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_DEPT", DbType.String, oModel_NPI_APP_CTQ._DEPT, ParameterDirection.Input, 100));
                opc.Add(DataPara.CreateDataParameter("@P_PROCESS", DbType.String, oModel_NPI_APP_CTQ._PROCESS, ParameterDirection.Input, 100));
                opc.Add(DataPara.CreateDataParameter("@P_CTQ", DbType.String, oModel_NPI_APP_CTQ._CTQ, ParameterDirection.Input, 100));
                opc.Add(DataPara.CreateDataParameter("@P_UNIT", DbType.String, oModel_NPI_APP_CTQ._UNIT, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_SPC", DbType.String, oModel_NPI_APP_CTQ._SPC, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_SPEC_LIMIT", DbType.String, oModel_NPI_APP_CTQ._SPEC_LIMIT, ParameterDirection.Input, 100));
                opc.Add(DataPara.CreateDataParameter("@P_CONTROL_TYPE", DbType.String, oModel_NPI_APP_CTQ._CONTROL_TYPE, ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_GOAL", DbType.String, oModel_NPI_APP_CTQ._GOAL, ParameterDirection.Input, 8));
                opc.Add(DataPara.CreateDataParameter("@P_ACT", DbType.String, oModel_NPI_APP_CTQ._ACT, ParameterDirection.Input, 40));
                opc.Add(DataPara.CreateDataParameter("@P_RESULT", DbType.String, oModel_NPI_APP_CTQ._RESULT, ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_Comment", DbType.String, oModel_NPI_APP_CTQ._Comment, ParameterDirection.Input, 1000));
                opc.Add(DataPara.CreateDataParameter("@P_STATUS", DbType.String, oModel_NPI_APP_CTQ._STATUS, ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_DESCRIPTION", DbType.String, oModel_NPI_APP_CTQ._DESCRIPTION, ParameterDirection.Input, 510));
                opc.Add(DataPara.CreateDataParameter("@P_DUTY_DEPT", DbType.String, oModel_NPI_APP_CTQ._DUTY_DEPT, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_DUTY_EMP", DbType.String, oModel_NPI_APP_CTQ._DUTY_EMP, ParameterDirection.Input, 30));
                opc.Add(DataPara.CreateDataParameter("@P_ROOT_CAUSE", DbType.String, oModel_NPI_APP_CTQ._ROOT_CAUSE, ParameterDirection.Input, 510));
                opc.Add(DataPara.CreateDataParameter("@P_D", DbType.String, oModel_NPI_APP_CTQ._D, ParameterDirection.Input, 1));
                opc.Add(DataPara.CreateDataParameter("@P_M", DbType.String, oModel_NPI_APP_CTQ._M, ParameterDirection.Input, 1));
                opc.Add(DataPara.CreateDataParameter("@P_P", DbType.String, oModel_NPI_APP_CTQ._P, ParameterDirection.Input, 1));
                opc.Add(DataPara.CreateDataParameter("@P_E", DbType.String, oModel_NPI_APP_CTQ._E, ParameterDirection.Input, 1));
                opc.Add(DataPara.CreateDataParameter("@P_W", DbType.String, oModel_NPI_APP_CTQ._W, ParameterDirection.Input, 1));
                opc.Add(DataPara.CreateDataParameter("@P_O", DbType.String, oModel_NPI_APP_CTQ._O, ParameterDirection.Input, 1));
                opc.Add(DataPara.CreateDataParameter("@P_TEMPORARY_ACTION", DbType.String, oModel_NPI_APP_CTQ._TEMPORARY_ACTION, ParameterDirection.Input, 510));
                opc.Add(DataPara.CreateDataParameter("@P_CORRECTIVE_PREVENTIVE_ACTION", DbType.String, oModel_NPI_APP_CTQ._CORRECTIVE_PREVENTIVE_ACTION, ParameterDirection.Input, 510));
                opc.Add(DataPara.CreateDataParameter("@P_COMPLETE_DATE", DbType.String, oModel_NPI_APP_CTQ._COMPLETE_DATE, ParameterDirection.Input, 510));
                opc.Add(DataPara.CreateDataParameter("@P_IMPROVEMENT_STATUS", DbType.String, oModel_NPI_APP_CTQ._IMPROVEMENT_STATUS, ParameterDirection.Input, 510));
                opc.Add(DataPara.CreateDataParameter("@P_UPDATE_TIME", DbType.DateTime, oModel_NPI_APP_CTQ._UPDATE_TIME, ParameterDirection.Input, 8));
                opc.Add(DataPara.CreateDataParameter("@P_REPLY_USERID", DbType.String, oModel_NPI_APP_CTQ._REPLY_USERID, ParameterDirection.Input, 30));

                opc.Add(DataPara.CreateDataParameter("@P_W_FILENAME", DbType.String, oModel_NPI_APP_CTQ._W_FILENAME, ParameterDirection.Input, 1000));
                opc.Add(DataPara.CreateDataParameter("@P_W_FILEPATH", DbType.String, oModel_NPI_APP_CTQ._W_FILEPATH, ParameterDirection.Input, 1000));
                opc.Add(DataPara.CreateDataParameter("@P_R_FILENAME", DbType.String, oModel_NPI_APP_CTQ._R_FILENAME, ParameterDirection.Input,1000));
                opc.Add(DataPara.CreateDataParameter("@P_R_FILEPATH", DbType.String, oModel_NPI_APP_CTQ._R_FILEPATH, ParameterDirection.Input, 1000));

                opc.Add(DataPara.CreateDataParameter("@Result", DbType.String, null, ParameterDirection.Output, 1000));
                SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
                Hashtable htResult = sdb.ExecuteProc("P_OP_NPI_APP_CTQ", opc);
                //SP執行結果返回固定格式
                //1 OK; 
                //2 NG;ERR MSG
                result = htResult["@Result"].ToString();

             
            }
            
            catch (Exception ex)
            {
                result = "DB ERROR:" + ex.Message;
            }
            return result;
        }

        public Dictionary<string, object> RecordOperation_PrelaunchStepHandler(Model_PRELAUNCH_STEP_HANDLER oModel_PRELAUNCH_STEP_HANDLER, Status_Operation RecordOperation)
        {

            Dictionary<string, object> result = new Dictionary<string, object>();
            result["ErrMsg"] = string.Empty;
            result["Result"] = false;

            try
            {

                ArrayList opc = new ArrayList();
                opc.Add(DataPara.CreateDataParameter("@P_OP_TYPE", DbType.String, RecordOperation.ToString(), ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_CASEID", DbType.Int32, oModel_PRELAUNCH_STEP_HANDLER._CASEID, ParameterDirection.Input, 4));
                opc.Add(DataPara.CreateDataParameter("@P_FORMNO", DbType.String, oModel_PRELAUNCH_STEP_HANDLER._FORMNO, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_STEP_NAME", DbType.String, oModel_PRELAUNCH_STEP_HANDLER._STEP_NAME, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_DEPT", DbType.String, oModel_PRELAUNCH_STEP_HANDLER._DEPT, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_HANDLER", DbType.String, oModel_PRELAUNCH_STEP_HANDLER._HANDLER, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_UPDATE_TIME", DbType.DateTime, oModel_PRELAUNCH_STEP_HANDLER._UPDATE_TIME, ParameterDirection.Input, 8));
                opc.Add(DataPara.CreateDataParameter("@P_BU", DbType.String, oModel_PRELAUNCH_STEP_HANDLER._BU, ParameterDirection.Input,20));
                opc.Add(DataPara.CreateDataParameter("@P_BUILDING", DbType.String, oModel_PRELAUNCH_STEP_HANDLER._BUILDING, ParameterDirection.Input,20));

                opc.Add(DataPara.CreateDataParameter("@Result", DbType.String, null, ParameterDirection.Output, 1000));
                SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
                Hashtable htResult = sdb.ExecuteProc("P_OP_PRELAUNCH_STEP_HANDLER", opc);
                //SP執行結果返回固定格式
                //1 OK; 
                //2 NG;ERR MSG
                string tmp = htResult["@Result"].ToString();
                if (tmp.Length >= 3)
                {
                    if (tmp.Substring(0, 2) == "NG")
                    {
                        //接收SP返回的ERR MSG
                        result["ErrMsg"] = tmp.Substring(3, tmp.Length - 3);
                    }
                    if (tmp.Substring(0, 2) == "OK")
                    {
                        result["Result"] = true;
                    }
                }
                else
                {

                    result["ErrMsg"] = "DB ERROR,Pls contact IT";
                }

            }
            catch (Exception ex)
            {
                result["ErrMsg"] = "DB ERROR:" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 數據操作
        /// </summary>

        /// <returns></returns>
        public Dictionary<string, object> RecordOperation_Attachement(Model_NPI_APP_ATTACHFILE oModel_NPI_APP_ATTACHFILE, Status_Operation RecordOperation)
        {

            Dictionary<string, object> result = new Dictionary<string, object>();
            result["ErrMsg"] = string.Empty;
            result["Result"] = false;

            try
            {

                ArrayList opc = new ArrayList();
                opc.Add(DataPara.CreateDataParameter("@P_OP_TYPE", DbType.String, RecordOperation.ToString(), ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_ID", DbType.Int32, oModel_NPI_APP_ATTACHFILE._ID, ParameterDirection.Input, 4));
                opc.Add(DataPara.CreateDataParameter("@P_SUB_DOC_NO", DbType.String, oModel_NPI_APP_ATTACHFILE._SUB_DOC_NO, ParameterDirection.Input, 30));
                opc.Add(DataPara.CreateDataParameter("@P_CASEID", DbType.Int32, oModel_NPI_APP_ATTACHFILE._CASEID, ParameterDirection.Input, 4));
                opc.Add(DataPara.CreateDataParameter("@P_FILE_PATH", DbType.String, oModel_NPI_APP_ATTACHFILE._FILE_PATH, ParameterDirection.Input, 255));
                opc.Add(DataPara.CreateDataParameter("@P_FILE_TYPE", DbType.String, oModel_NPI_APP_ATTACHFILE._FILE_TYPE, ParameterDirection.Input, 1));
                opc.Add(DataPara.CreateDataParameter("@P_FILE_NAME", DbType.String, oModel_NPI_APP_ATTACHFILE._FILE_NAME, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_DEPT", DbType.String, oModel_NPI_APP_ATTACHFILE._DEPT, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_REMARK", DbType.String, oModel_NPI_APP_ATTACHFILE._REMARK, ParameterDirection.Input, 100));
                opc.Add(DataPara.CreateDataParameter("@P_APPROVER", DbType.String, oModel_NPI_APP_ATTACHFILE._APPROVER, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_APPROVER_OPINION", DbType.String, oModel_NPI_APP_ATTACHFILE._APPROVER_OPINION, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_APPROVER_DATE", DbType.DateTime, oModel_NPI_APP_ATTACHFILE._APPROVER_DATE, ParameterDirection.Input, 8));
                opc.Add(DataPara.CreateDataParameter("@P_UPDATE_TIME", DbType.DateTime, oModel_NPI_APP_ATTACHFILE._UPDATE_TIME, ParameterDirection.Input, 8));
                opc.Add(DataPara.CreateDataParameter("@P_UPDATE_USERID", DbType.String, oModel_NPI_APP_ATTACHFILE._UPDATE_USERID, ParameterDirection.Input, 30));

                opc.Add(DataPara.CreateDataParameter("@Result", DbType.String, null, ParameterDirection.Output, 1000));
                SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
                Hashtable htResult = sdb.ExecuteProc("P_OP_NPI_APP_ATTACHFILE", opc);
                //SP執行結果返回固定格式
                //1 OK; 
                //2 NG;ERR MSG
                string tmp = htResult["@Result"].ToString();
                if (tmp.Length >= 3)
                {
                    if (tmp.Substring(0, 2) == "NG")
                    {
                        //接收SP返回的ERR MSG
                        result["ErrMsg"] = tmp.Substring(3, tmp.Length - 3);
                    }
                    if (tmp.Substring(0, 2) == "OK")
                    {
                        result["Result"] = true;
                    }
                }
                else
                {

                    result["ErrMsg"] = "DB ERROR,Pls contact IT";
                }

            }
            catch (Exception ex)
            {
                result["ErrMsg"] = "DB ERROR:" + ex.Message;
            }
            return result;
        }

        public Dictionary<string, object> RecordOperation_Result(Model_NPI_APP_RESULT oModel_NPI_APP_RESULT, Status_Operation RecordOperation)
        {

            Dictionary<string, object> result = new Dictionary<string, object>();
            result["ErrMsg"] = string.Empty;
            result["Result"] = false;

            try
            {

                ArrayList opc = new ArrayList();
                opc.Add(DataPara.CreateDataParameter("@P_OP_TYPE", DbType.String, RecordOperation.ToString(), ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_SUB_DOC_NO", DbType.String, oModel_NPI_APP_RESULT._SUB_DOC_NO, ParameterDirection.Input, 30));
                opc.Add(DataPara.CreateDataParameter("@P_CASEID", DbType.Int32, oModel_NPI_APP_RESULT._CASEID, ParameterDirection.Input, 4));
                opc.Add(DataPara.CreateDataParameter("@P_DEPT", DbType.String, oModel_NPI_APP_RESULT._DEPT, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_REMARK", DbType.String, Helper.ConvertChinese(oModel_NPI_APP_RESULT._REMARK,"Big5"),ParameterDirection.Input, 100));
                opc.Add(DataPara.CreateDataParameter("@P_APPROVER", DbType.String, oModel_NPI_APP_RESULT._APPROVER, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_APPROVER_RESULT", DbType.String, oModel_NPI_APP_RESULT._APPROVER_RESULT, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_APPROVER_OPINION", DbType.String, Helper.ConvertChinese(oModel_NPI_APP_RESULT._APPROVER_OPINION,"Big5"), ParameterDirection.Input, 2000));
                opc.Add(DataPara.CreateDataParameter("@P_APPROVER_Levels", DbType.String, oModel_NPI_APP_RESULT._APPROVER_Levels, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_APPROVER_DATE", DbType.DateTime, oModel_NPI_APP_RESULT._APPROVER_DATE, ParameterDirection.Input, 8));
                opc.Add(DataPara.CreateDataParameter("@P_UPDATE_TIME", DbType.DateTime, oModel_NPI_APP_RESULT._UPDATE_TIME, ParameterDirection.Input, 8));
                opc.Add(DataPara.CreateDataParameter("@P_UPDATE_USERID", DbType.String, oModel_NPI_APP_RESULT._UPDATE_USERID, ParameterDirection.Input, 30));

                opc.Add(DataPara.CreateDataParameter("@Result", DbType.String, null, ParameterDirection.Output, 1000));
                SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
                Hashtable htResult = sdb.ExecuteProc("P_OP_NPI_APP_RESULT", opc);
                //SP執行結果返回固定格式
                //1 OK; 
                //2 NG;ERR MSG
                string tmp = htResult["@Result"].ToString();
                if (tmp.Length >= 3)
                {
                    if (tmp.Substring(0, 2) == "NG")
                    {
                        //接收SP返回的ERR MSG
                        result["ErrMsg"] = tmp.Substring(3, tmp.Length - 3);
                    }
                    if (tmp.Substring(0, 2) == "OK")
                    {
                        result["Result"] = true;
                    }
                }
                else
                {

                    result["ErrMsg"] = "DB ERROR,Pls contact IT";
                }

            }
            catch (Exception ex)
            {
                result["ErrMsg"] = "DB ERROR:" + ex.Message;
            }
            return result;
        }

        public Dictionary<string, object> RecordOperation_FMEA(Model_NPI_FMEA oModel_NPI_FMEA, Status_Operation RecordOperation)
        {

            Dictionary<string, object> result = new Dictionary<string, object>();
            result["ErrMsg"] = string.Empty;
            result["Result"] = false;

            try
            {

                ArrayList opc = new ArrayList();
                opc.Add(DataPara.CreateDataParameter("@P_OP_TYPE", DbType.String, RecordOperation.ToString(), ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_SubNo", DbType.String, oModel_NPI_FMEA._SubNo, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_Item", DbType.String, oModel_NPI_FMEA._Item, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_Source", DbType.String, oModel_NPI_FMEA._Source, ParameterDirection.Input, 800));
                opc.Add(DataPara.CreateDataParameter("@P_Stantion", DbType.String, oModel_NPI_FMEA._Stantion, ParameterDirection.Input, 400));
                opc.Add(DataPara.CreateDataParameter("@P_PotentialFailureMode", DbType.String, oModel_NPI_FMEA._PotentialFailureMode, ParameterDirection.Input, 800));
                opc.Add(DataPara.CreateDataParameter("@P_Loess", DbType.String, oModel_NPI_FMEA._Loess, ParameterDirection.Input, 800));
                opc.Add(DataPara.CreateDataParameter("@P_Sev", DbType.Int32, oModel_NPI_FMEA._Sev, ParameterDirection.Input, 4));
                opc.Add(DataPara.CreateDataParameter("@P_PotentialFailure", DbType.String, oModel_NPI_FMEA._PotentialFailure, ParameterDirection.Input, 800));
                opc.Add(DataPara.CreateDataParameter("@P_Occ", DbType.Int32, oModel_NPI_FMEA._Occ, ParameterDirection.Input, 4));
                opc.Add(DataPara.CreateDataParameter("@P_CurrentControls", DbType.String, oModel_NPI_FMEA._CurrentControls, ParameterDirection.Input, 800));
                opc.Add(DataPara.CreateDataParameter("@P_DET", DbType.Int32, oModel_NPI_FMEA._DET, ParameterDirection.Input, 4));
                opc.Add(DataPara.CreateDataParameter("@P_RPN", DbType.Int32, oModel_NPI_FMEA._RPN, ParameterDirection.Input, 4));
                opc.Add(DataPara.CreateDataParameter("@P_RecommendedAction", DbType.String, oModel_NPI_FMEA._RecommendedAction, ParameterDirection.Input, 800));
                opc.Add(DataPara.CreateDataParameter("@P_Resposibility", DbType.String, oModel_NPI_FMEA._Resposibility, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_TargetCompletionDate", DbType.DateTime, oModel_NPI_FMEA._TargetCompletionDate, ParameterDirection.Input, 8));
                opc.Add(DataPara.CreateDataParameter("@P_ActionsTaken", DbType.String, oModel_NPI_FMEA._ActionsTaken, ParameterDirection.Input, 800));
                opc.Add(DataPara.CreateDataParameter("@P_ResultsSev", DbType.Int32, oModel_NPI_FMEA._ResultsSev, ParameterDirection.Input, 4));
                opc.Add(DataPara.CreateDataParameter("@P_ResultsOcc", DbType.Int32, oModel_NPI_FMEA._ResultsOcc, ParameterDirection.Input, 4));
                opc.Add(DataPara.CreateDataParameter("@P_ResultsDet", DbType.Int32, oModel_NPI_FMEA._ResultsDet, ParameterDirection.Input, 4));
                opc.Add(DataPara.CreateDataParameter("@P_ResultsRPN", DbType.Int32, oModel_NPI_FMEA._ResultsRPN, ParameterDirection.Input, 4));
                opc.Add(DataPara.CreateDataParameter("@P_WriteDept", DbType.String, oModel_NPI_FMEA._WriteDept, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_ReplyDept", DbType.String, oModel_NPI_FMEA._ReplyDept, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_Status", DbType.String, oModel_NPI_FMEA._Status, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_Update_User", DbType.String, oModel_NPI_FMEA._Update_User, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_Update_Time", DbType.DateTime, oModel_NPI_FMEA._Update_Time, ParameterDirection.Input, 8));
                opc.Add(DataPara.CreateDataParameter("@P_ID", DbType.String, oModel_NPI_FMEA._ID, ParameterDirection.Input, 8));
                opc.Add(DataPara.CreateDataParameter("@P_FILEPATH", DbType.String, oModel_NPI_FMEA._FILEPATH, ParameterDirection.Input,50));

                opc.Add(DataPara.CreateDataParameter("@P_FILENAME", DbType.String, oModel_NPI_FMEA._FILENAME, ParameterDirection.Input,100));

                opc.Add(DataPara.CreateDataParameter("@Result", DbType.String, null, ParameterDirection.Output, 1000));
                SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
                Hashtable htResult = sdb.ExecuteProc("P_OP_NPI_FMEA", opc);
                //SP執行結果返回固定格式
                //1 OK; 
                //2 NG;ERR MSG
                string tmp = htResult["@Result"].ToString();
                if (tmp.Length >= 3)
                {
                    if (tmp.Substring(0, 2) == "NG")
                    {
                        //接收SP返回的ERR MSG
                        result["ErrMsg"] = tmp.Substring(3, tmp.Length - 3);
                    }
                    if (tmp.Substring(0, 2) == "OK")
                    {
                        result["Result"] = true;
                    }
                }
                else
                {

                    result["ErrMsg"] = "DB ERROR,Pls contact IT";
                }

            }
            catch (Exception ex)
            {
                result["ErrMsg"] = "DB ERROR:" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 數據操作
        /// </summary>

        /// <returns></returns>
        public Dictionary<string, object> RecordOperation_PrelaunchItem(Model_PRELAUNCH_CHECKITEMCONFIG oModel_PRELAUNCH_CHECKITEMCONFIG, Status_Operation RecordOperation)
        {

            Dictionary<string, object> result = new Dictionary<string, object>();
            result["ErrMsg"] = string.Empty;
            result["Result"] = false;

            try
            {

                ArrayList opc = new ArrayList();
                opc.Add(DataPara.CreateDataParameter("@P_OP_TYPE", DbType.String, RecordOperation.ToString(), ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_Bu", DbType.String, oModel_PRELAUNCH_CHECKITEMCONFIG._Bu, ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_Building", DbType.String, oModel_PRELAUNCH_CHECKITEMCONFIG._Building, ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_Dept", DbType.String, oModel_PRELAUNCH_CHECKITEMCONFIG._Dept, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_CheckItem", DbType.String, oModel_PRELAUNCH_CHECKITEMCONFIG._CheckItem, ParameterDirection.Input, 100));
                opc.Add(DataPara.CreateDataParameter("@P_AttachmentFlag", DbType.String, oModel_PRELAUNCH_CHECKITEMCONFIG._AttachmentFlag, ParameterDirection.Input, 1));
                opc.Add(DataPara.CreateDataParameter("@P_UpdateUser", DbType.String, oModel_PRELAUNCH_CHECKITEMCONFIG._UpdateUser, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_UpdateTime", DbType.DateTime, oModel_PRELAUNCH_CHECKITEMCONFIG._UpdateTime, ParameterDirection.Input, 8));
                opc.Add(DataPara.CreateDataParameter("@P_ID", DbType.String, oModel_PRELAUNCH_CHECKITEMCONFIG._ID, ParameterDirection.Input,20));
                opc.Add(DataPara.CreateDataParameter("@Result", DbType.String, null, ParameterDirection.Output, 1000));
                SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
                Hashtable htResult = sdb.ExecuteProc("P_OP_Prelaunch_CheckItemConfig", opc);
                //SP執行結果返回固定格式
                //1 OK; 
                //2 NG;ERR MSG
                string tmp = htResult["@Result"].ToString();
                if (tmp.Length >= 3)
                {
                    if (tmp.Substring(0, 2) == "NG")
                    {
                        //接收SP返回的ERR MSG
                        result["ErrMsg"] = tmp.Substring(3, tmp.Length - 3);
                    }
                    if (tmp.Substring(0, 2) == "OK")
                    {
                        result["Result"] = true;
                    }
                }
                else
                {

                    result["ErrMsg"] = "DB ERROR,Pls contact IT";
                }

            }
            catch (Exception ex)
            {
                result["ErrMsg"] = "DB ERROR:" + ex.Message;
            }
            return result;
        }
        /// 數據操作
        /// </summary>

        /// <returns></returns>
        public Dictionary<string, object> RecordOperation_PrelaunchMain(Model_PRELAUNCH_MAIN oModel_PRELAUNCH_MAIN, Status_Operation RecordOperation)
        {

            Dictionary<string, object> result = new Dictionary<string, object>();
            result["ErrMsg"] = string.Empty;
            result["Result"] = false;

            try
            {

                ArrayList opc = new ArrayList();
                opc.Add(DataPara.CreateDataParameter("@P_OP_TYPE", DbType.String, RecordOperation.ToString(), ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_Bu", DbType.String, oModel_PRELAUNCH_MAIN._Bu, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_Building", DbType.String, oModel_PRELAUNCH_MAIN._Building, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_PilotRunNO", DbType.String, oModel_PRELAUNCH_MAIN._PilotRunNO, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_Applicant", DbType.String, oModel_PRELAUNCH_MAIN._Applicant, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_Model", DbType.String, oModel_PRELAUNCH_MAIN._Model, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_Customer", DbType.String, oModel_PRELAUNCH_MAIN._Customer, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_PCBInRev", DbType.String, oModel_PRELAUNCH_MAIN._PCBInRev, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_PCBOutRev", DbType.String, oModel_PRELAUNCH_MAIN._PCBOutRev, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_PLRev", DbType.String, oModel_PRELAUNCH_MAIN._PLRev, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_Date", DbType.String, oModel_PRELAUNCH_MAIN._Date, ParameterDirection.Input,20));
                opc.Add(DataPara.CreateDataParameter("@P_TP_ME", DbType.String, oModel_PRELAUNCH_MAIN._TP_ME, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_TP_EE", DbType.String, oModel_PRELAUNCH_MAIN._TP_EE, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_TP_PM", DbType.String, oModel_PRELAUNCH_MAIN._TP_PM, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_PM", DbType.String, oModel_PRELAUNCH_MAIN._PM, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_CaseId", DbType.Int32, oModel_PRELAUNCH_MAIN._CaseId, ParameterDirection.Input, 4));
                opc.Add(DataPara.CreateDataParameter("@P_Status", DbType.String, oModel_PRELAUNCH_MAIN._Status, ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_Notes", DbType.String, oModel_PRELAUNCH_MAIN._Notes, ParameterDirection.Input,100));

                opc.Add(DataPara.CreateDataParameter("@Result", DbType.String, null, ParameterDirection.Output, 1000));
                SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
                Hashtable htResult = sdb.ExecuteProc("P_OP_PRELAUNCH_MAIN", opc);
                //SP執行結果返回固定格式
                //1 OK; 
                //2 NG;ERR MSG
                string tmp = htResult["@Result"].ToString();
                if (tmp.Length >= 3)
                {
                    if (tmp.Substring(0, 2) == "NG")
                    {
                        //接收SP返回的ERR MSG
                        result["ErrMsg"] = tmp.Substring(3, tmp.Length - 3);
                    }
                    if (tmp.Substring(0, 2) == "OK")
                    {
                        result["Result"] = true;
                    }
                }
                else
                {

                    result["ErrMsg"] = "DB ERROR,Pls contact IT";
                }

            }
            catch (Exception ex)
            {
                result["ErrMsg"] = "DB ERROR:" + ex.Message;
            }
            return result;
        }
        
        public Dictionary<string, object> RecordOperation_NPIMain(Model_NPI_APP_MAIN_HIS oModel_NPI_APP_MAIN_HIS, Status_Operation RecordOperation)
        {
            
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["ErrMsg"] = string.Empty;
            result["Result"] = false;

            try
            {

                ArrayList opc=new ArrayList();
                opc.Add(DataPara.CreateDataParameter("@P_OP_TYPE", DbType.String, RecordOperation.ToString(), ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_DOC_NO", DbType.String, oModel_NPI_APP_MAIN_HIS._DOC_NO, ParameterDirection.Input, 30));
                opc.Add(DataPara.CreateDataParameter("@P_BU", DbType.String, oModel_NPI_APP_MAIN_HIS._BU, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_BUILDING", DbType.String, oModel_NPI_APP_MAIN_HIS._BUILDING, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_APPLY_DATE", DbType.String, oModel_NPI_APP_MAIN_HIS._APPLY_DATE, ParameterDirection.Input,20));
                opc.Add(DataPara.CreateDataParameter("@P_APPLY_USERID", DbType.String, oModel_NPI_APP_MAIN_HIS._APPLY_USERID, ParameterDirection.Input, 30));
                opc.Add(DataPara.CreateDataParameter("@P_MODEL_NAME", DbType.String, oModel_NPI_APP_MAIN_HIS._MODEL_NAME, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_CUSTOMER", DbType.String, oModel_NPI_APP_MAIN_HIS._CUSTOMER, ParameterDirection.Input, 100));
                opc.Add(DataPara.CreateDataParameter("@P_PRODUCT_TYPE", DbType.String, oModel_NPI_APP_MAIN_HIS._PRODUCT_TYPE, ParameterDirection.Input, 510));
                opc.Add(DataPara.CreateDataParameter("@P_LAYOUT", DbType.String, oModel_NPI_APP_MAIN_HIS._LAYOUT, ParameterDirection.Input, 510));
                opc.Add(DataPara.CreateDataParameter("@P_PHASE", DbType.String, oModel_NPI_APP_MAIN_HIS._PHASE, ParameterDirection.Input, 100));
                opc.Add(DataPara.CreateDataParameter("@P_NEXTSTAGE_DATE", DbType.String, oModel_NPI_APP_MAIN_HIS._NEXTSTAGE_DATE, ParameterDirection.Input, 100));
                opc.Add(DataPara.CreateDataParameter("@P_UPDATE_TIME", DbType.DateTime, oModel_NPI_APP_MAIN_HIS._UPDATE_TIME, ParameterDirection.Input, 8));
                opc.Add(DataPara.CreateDataParameter("@P_UPDATE_USERID", DbType.String, oModel_NPI_APP_MAIN_HIS._UPDATE_USERID, ParameterDirection.Input, 30));
                opc.Add(DataPara.CreateDataParameter("@P_NPI_PM", DbType.String, oModel_NPI_APP_MAIN_HIS._NPI_PM, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_SALES_OWNER", DbType.String, oModel_NPI_APP_MAIN_HIS._SALES_OWNER, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_RD_ENGINEER", DbType.String, oModel_NPI_APP_MAIN_HIS._RD_ENGINEER, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_REMARK", DbType.String, oModel_NPI_APP_MAIN_HIS._REMARK, ParameterDirection.Input, 30));
                opc.Add(DataPara.CreateDataParameter("@P_CASEID", DbType.String, oModel_NPI_APP_MAIN_HIS._CASEID, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_STATUS", DbType.String, oModel_NPI_APP_MAIN_HIS._STATUS, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_PMLOC", DbType.String, oModel_NPI_APP_MAIN_HIS._PMLOC, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_PMEXT", DbType.String, oModel_NPI_APP_MAIN_HIS._PMEXT, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_RDLOC", DbType.String, oModel_NPI_APP_MAIN_HIS._RDLOC, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_RDEXT", DbType.String, oModel_NPI_APP_MAIN_HIS._RDEXT, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_SALESLOC", DbType.String, oModel_NPI_APP_MAIN_HIS._SALESLOC, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@P_SALESEXT", DbType.String, oModel_NPI_APP_MAIN_HIS._SALESEXT, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@Result", DbType.String, null, ParameterDirection.Output, 1000));
                SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
                Hashtable htResult = sdb.ExecuteProc("P_OP_NPI_APP_MAIN_HIS", opc);
                //SP執行結果返回固定格式
                //1 OK; 
                //2 NG;ERR MSG
                string tmp = htResult["@Result"].ToString();
                if (tmp.Length >= 3)
                {
                    if (tmp.Substring(0, 2) == "NG")
                    {
                        //接收SP返回的ERR MSG
                        result["ErrMsg"] = tmp.Substring(3, tmp.Length - 3);
                    }
                    if (tmp.Substring(0, 2) == "OK")
                    {
                        result["Result"] = true;
                    }
                }
                else
                {

                    result["ErrMsg"] = "DB ERROR,Pls contact IT";
                }

            }
            catch (Exception ex)
            {
                result["ErrMsg"] = "DB ERROR:" + ex.Message;
            }
            return result;
        }
        
        #endregion

        public DataTable GetAppMember(string DocNo,string Category)
        {
            StringBuilder sb=new StringBuilder();
            ArrayList opc = new ArrayList();
            opc.Clear();
            sb.Append("select  *FROM ");
            sb.Append(" TB_NPI_APP_MEMBER with(nolock) where 1=1 ");
            sb.Append(" AND DOC_NO=@DocNo");
            opc.Add(DataPara.CreateDataParameter("@DocNo", DbType.String,DocNo));
            
            if (Category.Length>0)
            {
                sb.Append(" And Category=@Category");
                opc.Add(DataPara.CreateDataParameter("@Category", DbType.String,Category));
            }
           
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            DataTable dt= sdb.TransactionExecute(sb.ToString(), opc);
            return dt;
        }

        public virtual DataTable GetMasterInfo(string CaseId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select T1.*,");
            sql.Append("T2.APPLY_USERID,T2.PROD_GROUP,T2.MODEL_NAME,T2.CUSTOMER ");
            sql.Append("From TB_NPI_APP_SUB T1,TB_NPI_APP_MAIN T2  ");
            sql.Append(" where T1.SUB_DOC_PHASE_STATUS=10 ");
            sql.Append(" AND T1.DOC_NO=T2.DOC_NO ");
            sql.Append(" AND T1.CASEID=@CaseId");
            ArrayList opc = new ArrayList();
            opc.Add(DataPara.CreateDataParameter("@CaseId", DbType.String, CaseId));
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            DataTable dt = sdb.TransactionExecute(sql.ToString(), opc);
            return dt;
        }

        public virtual DataTable GetMasterInfoHIS(string CaseId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * ");
            sql.Append("From [TB_NPI_APP_MAIN_HIS]  ");
            sql.Append(" where CASEID=@CaseId");
            ArrayList opc = new ArrayList();
            opc.Add(DataPara.CreateDataParameter("@CaseId", DbType.String, CaseId));
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            DataTable dt = sdb.TransactionExecute(sql.ToString(), opc);
            return dt;
        }

        public string GetUserDept(string Category,string LoginId,string DocNo)
        {
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
           
            StringBuilder sbDept=new StringBuilder();
            string sql = "select  DISTINCT DEPT from TB_NPI_APP_MEMBER where 1=1"
                 +" AND  (WriteEname =@ENAME OR ReplyEName=@ENAME OR CheckedEname=@ENAME)"
               
                 +" AND CategoryFlag=@Category"
                 + " AND DOC_NO=@Doc_No";
            ArrayList opc = new ArrayList();
            opc.Add(DataPara.CreateDataParameter("@ENAME", DbType.String, LoginId));
            opc.Add(DataPara.CreateDataParameter("@Category", DbType.String, Category));
            opc.Add(DataPara.CreateDataParameter("@Doc_No", DbType.String, DocNo));
            DataTable dt = sdb.TransactionExecute(sql, opc);
            foreach (DataRow dr in dt.Rows)
            {
                 sbDept.AppendFormat("{0},",dr["DEPT"].ToString());
            }
            return sbDept.ToString().TrimEnd(',');
        }

        public string GetUserDept(string Category, string LoginId, string DocNo,string stepname,string Category1)
        {
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            ArrayList opc = new ArrayList();
            StringBuilder sbDept = new StringBuilder();
            string sql = "select  DISTINCT DEPT from TB_NPI_APP_MEMBER where 1=1";
                   sql+= " AND CategoryFlag=@Category";
                   sql+=  " AND DOC_NO=@Doc_No";
                   sql += " and Category=@Category1";
            switch(stepname)
            {
                case "Dept.Write":
                    sql+=" AND WriteEname =@ENAME";
                    break;
                case "Dept.Reply":
                    sql+=" AND ReplyEName=@ENAME";
                    break;
                default:
                    sql+=" AND  (WriteEname =@ENAME OR ReplyEName=@ENAME OR CheckedEname=@ENAME)";
                    break;
            }
            opc.Add(DataPara.CreateDataParameter("@ENAME", DbType.String, LoginId));
            opc.Add(DataPara.CreateDataParameter("@Category1", DbType.String, Category1));
            opc.Add(DataPara.CreateDataParameter("@Category", DbType.String, Category));
            opc.Add(DataPara.CreateDataParameter("@Doc_No", DbType.String, DocNo));
            DataTable dt = sdb.TransactionExecute(sql, opc);
            foreach (DataRow dr in dt.Rows)
            {
                sbDept.AppendFormat("{0},", dr["DEPT"].ToString());
            }
            return sbDept.ToString().TrimEnd(',');
        }

        public string GetUserDept_ReplyCheck(string CategoryFlag, string LoginId, string DocNo, string Category)
        {
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));

            StringBuilder sbDept = new StringBuilder();
            string sql = "select  DISTINCT DEPT from TB_NPI_APP_MEMBER where 1=1"
                 + " AND  CheckedEname=@ENAME"

                 + " AND CategoryFlag=@CategoryFlag"
                 + " AND DOC_NO=@Doc_No"
                 + " AND Category=@Category";
            ArrayList opc = new ArrayList();
            opc.Add(DataPara.CreateDataParameter("@ENAME", DbType.String, LoginId));
            opc.Add(DataPara.CreateDataParameter("@CategoryFlag", DbType.String, CategoryFlag));
            opc.Add(DataPara.CreateDataParameter("@Doc_No", DbType.String, DocNo));
            opc.Add(DataPara.CreateDataParameter("@Category", DbType.String, Category));
            DataTable dt = sdb.TransactionExecute(sql, opc);
            foreach (DataRow dr in dt.Rows)
            {
                sbDept.AppendFormat("{0},", dr["DEPT"].ToString());
            }
            return sbDept.ToString().TrimEnd(',');
        }

        public DataTable GetDFXInconformity(string DocNo,string Dept)
        {
              SqlDB sdb= new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
              ArrayList opc = new ArrayList();
              opc.Clear();
              StringBuilder sb=new StringBuilder();
              sb.Append("SELECT Item,WriteDept,RPN,Actions,CompletionDate,Requirements from TB_DFX_ItemBody");
              sb.Append( " WHERE DFXNo=@DocNo");
              sb.Append(" AND Compliance='N' ");
              opc.Add(DataPara.CreateDataParameter("@DocNo", DbType.String, DocNo));
             if (!string.IsNullOrEmpty(Dept))
             {
                 sb.Append(" AND (");
                 string[] DeptCodes = Dept.Split(',');
                 for (int i = 0; i < DeptCodes.Length; i++)
                 {
                     string code = DeptCodes[i];
                     if (i == 0)
                     {
                         sb.Append(" WriteDept='" + code + "'");
                     }
                     else
                     {
                         sb.Append(" OR WriteDept='" + code + "'");
                     }
                 }
                 sb.Append(" )");
             }
               
             DataTable dt=sdb.TransactionExecute(sb.ToString(),opc);
             return dt;
        }

        public DataTable GetDFXInconformity(string DocNo, string Dept,string Type)
        {
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            ArrayList opc = new ArrayList();
            opc.Clear();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * from TB_DFX_ItemBody");
            sb.Append(" WHERE DFXNo=@DocNo");
            opc.Add(DataPara.CreateDataParameter("@DocNo", DbType.String, DocNo));
        
            switch (Type)
            {
                case "Dept.Write":
                case "WriteChecked":
                case "":
                    if (!string.IsNullOrEmpty(Dept))
                    {
                        sb.Append(" AND (");
                        string[] DeptCodes = Dept.Split(',');
                        for (int i = 0; i < DeptCodes.Length; i++)
                        {
                            string code = DeptCodes[i];
                            if (i == 0)
                            {
                                sb.Append(" WriteDept='" + code + "'");
                            }
                            else
                            {
                                sb.Append(" OR WriteDept='" + code + "'");
                            }
                        }
                        sb.Append(" )");
                    }
                    else
                    {
                        sb.Append(" and DFXNo = '111111'");
                    } 
                    break;
                case "Dept.Reply":
                case "ReplyChecked":
              
                    if (!string.IsNullOrEmpty(Dept))
                    {
                        sb.Append(" AND (");
                        string[] DeptCodes = Dept.Split(',');
                        for (int i = 0; i < DeptCodes.Length; i++)
                        {
                            string code = DeptCodes[i];
                            if (i == 0)
                            {
                                sb.Append(" WriteDept='" + code + "'");
                            }
                            else
                            {
                                sb.Append(" OR WriteDept='" + code + "'");
                            }
                        }
                        sb.Append(" )");
                        sb.Append(" AND Compliance='N' ");
                    }
                    else
                    {
                        sb.Append(" and DFXNo = '111111'");
                    } 

                    break;
                case "Begin":
                case "Dept.Prepared":
                case "Dept.Checked":
                case "PM Approver":
                case "PM":
                case "NPI Leader":
                case "TOP Manager":
                    sb.Append(" AND Compliance='N' ");
                    break;
                default:
                    sb.Append(" AND Status<>'Write'");
                    break;

            }

            sb.Append(@" order by ID");           
            
            DataTable dt = sdb.TransactionExecute(sb.ToString(), opc);
            return dt;
        }
       
        public DataTable GetCLCAInconformity(string DocNo,string Dept)
        {
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            ArrayList opc = new ArrayList();
            opc.Clear();
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT *FROM TB_NPI_APP_CTQ");
            sb.Append(" WHERE SUB_DOC_NO=@DocNo");
            sb.Append( " AND IMPROVEMENT_STATUS='Open'");
            opc.Add(DataPara.CreateDataParameter("@DocNo", DbType.String, DocNo));
            if (!string.IsNullOrEmpty(Dept))
            {
                sb.Append(" AND (");
                string[] DeptCodes = Dept.Split(',');
                for (int i = 0; i < DeptCodes.Length; i++)
                {
                    string code = DeptCodes[i];
                    if (i == 0)
                    {
                        sb.Append(" DEPT='" + code + "'");
                    }
                    else
                    {
                        sb.Append(" OR DEPT='" + code + "'");
                    }
                }
                sb.Append(" )");
            }
            DataTable dt = sdb.TransactionExecute(sb.ToString(), opc);
            return dt;
        }


        public DataTable GetCLCAInconformity(string DocNo, string Dept,string Type)
        {
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            ArrayList opc = new ArrayList();
            opc.Clear();
            StringBuilder sb = new StringBuilder();
            DataTable dt=new DataTable();
            decimal ACT_DD = 0;
        
                sb.Append(" select *,'' as GOALStr,'' as ACTStr  from TB_NPI_APP_CTQ");
                sb.Append(" where  SUB_DOC_NO=@SUB_DOC_NO ");

                opc.Add(DataPara.CreateDataParameter("@SUB_DOC_NO", DbType.String, DocNo));

                switch (Type)
                {
                    case "Dept.Write":
                    case "WriteChecked":
                        if (!string.IsNullOrEmpty(Dept))
                        {
                            sb.Append(" AND (");
                            string[] DeptCodes = Dept.Split(',');
                            for (int i = 0; i < DeptCodes.Length; i++)
                            {
                                string code = DeptCodes[i];
                                if (i == 0)
                                {
                                    sb.Append(" DEPT='" + code + "'");
                                }
                                else
                                {
                                    sb.Append(" OR DEPT='" + code + "'");
                                }
                            }
                            sb.Append(" )");
                        }
                        else
                        {
                            sb.Append(" and SUB_DOC_NO = '111111'");
                        }
                        break;
                    case "Dept.Reply":
                    case "Reply":
                    case "ReplyChecked":
                        if (!string.IsNullOrEmpty(Dept))
                        {
                            sb.Append(" AND (");
                            string[] DeptCodes = Dept.Split(',');
                            for (int i = 0; i < DeptCodes.Length; i++)
                            {
                                string code = DeptCodes[i];
                                if (i == 0)
                                {
                                    sb.Append(" DEPT='" + code + "'");
                                }
                                else
                                {
                                    sb.Append(" OR DEPT='" + code + "'");
                                }
                            }
                            sb.Append(" )");
                        }
                        else
                        {
                            sb.Append(" and SUB_DOC_NO = '111111'");
                        }
                        sb.Append("  AND RESULT=@Result");
                        opc.Add(DataPara.CreateDataParameter("@Result", DbType.String, "Fail"));
                        break;
                    case "Begin":
                    case "Dept.Prepared":
                    case "Dept.Checked":
                    case "PM Approver":
                    case "PM":
                    case "NPI Leader":
                    case "TOP Manager":
                        sb.Append("  AND RESULT=@RESULT");
                        opc.Add(DataPara.CreateDataParameter("@RESULT", DbType.String, "Fail"));
                        break;

                    default:
                        sb.Append(" AND STATUS<>'Write'");
                        break;
                }
                dt = sdb.TransactionExecute(sb.ToString(), opc);
                foreach (DataRow dr in dt.Rows)
                {

                    string CONTROL_TYPE = dr["CONTROL_TYPE"].ToString();
                    string GO = dr["GOAL"].ToString();
                    if (CONTROL_TYPE == "Yield%" && GO != "NA")
                    {
                        dr.BeginEdit();
                        decimal GOAL = Convert.ToDecimal(dr["GOAL"].ToString()) * 100;
                        string GOAL_STR = Convert.ToString(GOAL);
                        dr["GOALStr"] = string.Format("{0}%", GOAL_STR);

                        string ACT = dr["ACT"].ToString();
                        if (!string.IsNullOrEmpty(ACT))
                        {
                            if (decimal.TryParse(ACT, out ACT_DD))
                            {
                                dr["ACTStr"] = string.Format("{0}%", ACT);
                            }
                            else
                            {
                                dr["ACTStr"] = ACT;
                            }
                        }
                        dr.EndEdit();
                    }
                    else if (GO == "NA")
                    {
                        dr.BeginEdit();
                        dr["GOALStr"] = dr["GOAL"].ToString();
                        dr.EndEdit();
                    }
                    else
                    {
                        dr.BeginEdit();
                        dr["GOALStr"] = dr["GOAL"].ToString();
                        dr["ACTStr"] = dr["ACT"].ToString();
                        dr.EndEdit();
                    }
               

              
            }
                return dt;
        }
     
        
        public DataTable GetIssuesInconformity(string DocNo,string Dept,string Status)
        {
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            ArrayList opc = new ArrayList();
            opc.Clear();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT *FROM TB_NPI_APP_ISSUELIST");
            sb.Append(" WHERE SUB_DOC_NO=@DocNo");
            if (!string.IsNullOrEmpty(Status))
            {
                sb.Append(" AND TRACKING=@Stauts");
                opc.Add(DataPara.CreateDataParameter("@Stauts", DbType.String,Status));
            }
            opc.Add(DataPara.CreateDataParameter("@DocNo", DbType.String, DocNo));
            if (!string.IsNullOrEmpty(Dept))
            {
                sb.Append(" AND (");
                string[] DeptCodes = Dept.Split(',');
                for (int i = 0; i < DeptCodes.Length; i++)
                {
                    string code = DeptCodes[i];
                    if (i == 0)
                    {
                        sb.Append(" Dept='" + code + "'");
                    }
                    else
                    {
                        sb.Append(" OR Dept='" + code + "'");
                    }
                }
                sb.Append(" )");
            }
            DataTable dt = sdb.TransactionExecute(sb.ToString(), opc);
            return dt;
        }

        public DataTable GetIssuesInconformity(string DocNo, string Dept, string Status,string StepName)
        {
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            ArrayList opc = new ArrayList();
            opc.Clear();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT *FROM TB_NPI_APP_ISSUELIST");
            sb.Append(" WHERE SUB_DOC_NO=@DocNo");
            switch (StepName)
            {
                case "Dept.Prepared":
                case "Dept.Checked":
                    break;
                default:
                    if (!string.IsNullOrEmpty(Dept))
                    {
                        sb.Append(" AND (");
                        string[] DeptCodes = Dept.Split(',');
                        for (int i = 0; i < DeptCodes.Length; i++)
                        {
                            string code = DeptCodes[i];
                            if (i == 0)
                            {
                                sb.Append(" Dept='" + code + "'");
                            }
                            else
                            {
                                sb.Append(" OR Dept='" + code + "'");
                            }
                        }
                        sb.Append(" )");
                    }
                    break;
            }
            if (!string.IsNullOrEmpty(Status))
            {
                sb.Append(" AND TRACKING=@Stauts");
                opc.Add(DataPara.CreateDataParameter("@Stauts", DbType.String, Status));
            }
            opc.Add(DataPara.CreateDataParameter("@DocNo", DbType.String, DocNo));
            
            DataTable dt = sdb.TransactionExecute(sb.ToString(), opc);
            return dt;
        }
     
        public DataTable GetFMEAInconformity(string DocNo, string Dept,String PRN)
        {
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            ArrayList opc = new ArrayList();
            opc.Clear();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT *FROM TB_NPI_FMEA");
            sb.Append(" WHERE SubNo=@DocNo");
            if (!string.IsNullOrEmpty(PRN))
            {
                sb.Append(" AND RPN>@PRN ");
                opc.Add(DataPara.CreateDataParameter("@PRN", DbType.String,PRN));
            }
            opc.Add(DataPara.CreateDataParameter("@DocNo", DbType.String, DocNo));
            if (!string.IsNullOrEmpty(Dept))
            {
                sb.Append(" AND (");
                string[] DeptCodes = Dept.Split(',');
                for (int i = 0; i < DeptCodes.Length; i++)
                {
                    string code = DeptCodes[i];
                    if (i == 0)
                    {
                        sb.Append(" WriteDept='" + code + "'");
                    }
                    else
                    {
                        sb.Append(" OR WriteDept='" + code + "'");
                    }
                }
                sb.Append(" )");
            }
          
            DataTable dt = sdb.TransactionExecute(sb.ToString(), opc);
            return dt;
        }

        public DataTable GetFMEAInconformity(string DocNo, string Dept,string PRN, string StepName)
        {

            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            ArrayList opc = new ArrayList();
            opc.Clear();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT *FROM TB_NPI_FMEA");
            sb.Append(" WHERE SubNo=@DocNo");
            if (!string.IsNullOrEmpty(PRN))
            {
                sb.Append(" AND RPN>@PRN ");
                opc.Add(DataPara.CreateDataParameter("@PRN", DbType.String, PRN));
            }
            opc.Add(DataPara.CreateDataParameter("@DocNo", DbType.String, DocNo));
            switch (StepName)
            {
                default:
                    if (!string.IsNullOrEmpty(Dept))
                    {
                        sb.Append(" AND (");
                        string[] DeptCodes = Dept.Split(',');
                        for (int i = 0; i < DeptCodes.Length; i++)
                        {
                            string code = DeptCodes[i];
                            if (i == 0)
                            {
                                sb.Append(" WriteDept='" + code + "'");
                            }
                            else
                            {
                                sb.Append(" OR WriteDept='" + code + "'");
                            }
                        }
                        sb.Append(" )");
                    }
                    break;
                case "Dept.Prepared":
                case "Dept.Checked":
                    break;
            }
            
            DataTable dt = sdb.TransactionExecute(sb.ToString(), opc);
            return dt;
        }

        public virtual DataTable Get_NPI_Attachment(int CaseID, string Dept)
        {

            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            StringBuilder sb = new StringBuilder();
            sb.Append("select *from TB_NPI_APP_ATTACHFILE WHERE CASEID=@CASEID");

            if (!string.IsNullOrEmpty(Dept))
            {
                sb.Append(" AND (");
                string[] DeptCodes = Dept.Split(',');
                for (int i = 0; i < DeptCodes.Length; i++)
                {
                    string code = DeptCodes[i];
                    if (i == 0)
                    {
                        sb.Append(" DEPT='" + code + "'");
                    }
                    else
                    {
                        sb.Append(" OR DEPT='" + code + "'");
                    }
                }
                sb.Append(" )");
            }

            ArrayList opc = new ArrayList();
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@CASEID", DbType.Int32, CaseID));
            opc.Add(DataPara.CreateDataParameter("@Dept", DbType.String, Dept));
            DataTable dt = sdb.TransactionExecute(sb.ToString(), opc);
            return dt;
        }
     
        public virtual DataTable Get_NPI_AttachmentPR(int CaseID, string Dept)
        {
            
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            StringBuilder sb=new StringBuilder();
            sb.Append("select *from TB_NPI_APP_PR_ATTACHFILE WHERE CASEID=@CASEID");
            
                if (!string.IsNullOrEmpty(Dept))
                {
                    sb.Append(" AND (");
                    string[] DeptCodes = Dept.Split(',');
                    for (int i = 0; i < DeptCodes.Length; i++)
                    {
                        string code = DeptCodes[i];
                        if (i == 0)
                        {
                            sb.Append(" DEPT='" + code + "'");
                        }
                        else
                        {
                            sb.Append(" OR DEPT='" + code + "'");
                        }
                    }
                    sb.Append(" )");
                }
            
            ArrayList opc=new ArrayList();
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@CASEID", DbType.Int32, CaseID));
            opc.Add(DataPara.CreateDataParameter("@Dept", DbType.String, Dept));
            DataTable dt = sdb.TransactionExecute(sb.ToString(), opc);
            return dt;
        }

        public virtual DataTable Get_NPI_Attachment_Issue(int CaseID)
        {

            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            StringBuilder sb = new StringBuilder();
            sb.Append("select *from TB_NPI_APP_ISSUELIST_ATTACHFILE WHERE CASEID=@CASEID");
            ArrayList opc = new ArrayList();
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@CASEID", DbType.Int32, CaseID));
            DataTable dt = sdb.TransactionExecute(sb.ToString(), opc);
            return dt;
        }
      
        public virtual DataTable GetNPI_Result(int CaseID)
        {

            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from TB_NPI_APP_RESULT WHERE CASEID=@CASEID order by ID");
            ArrayList opc = new ArrayList();
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@CASEID", DbType.Int32, CaseID));
            DataTable dt = sdb.TransactionExecute(sb.ToString(), opc);
            return dt;
        }

        public DataTable GetPrelaunchCheckItem(string Bu,string Building,string Dept)
        {
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            StringBuilder sb = new StringBuilder();
            sb.Append("select *from TB_Prelaunch_CheckItemConfig WHERE 1=1 ");
            sb.Append(" AND  Bu=@Bu AND Building=@Building");
            ArrayList opc = new ArrayList();
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@Bu", DbType.String, Bu));
            opc.Add(DataPara.CreateDataParameter("@Building", DbType.String, Building));
            if (!string.IsNullOrEmpty(Dept))
            {
                sb.Append(" And Dept=@Dept");
                opc.Add(DataPara.CreateDataParameter("@Dept", DbType.String,Dept));
            }
            DataTable dt = sdb.TransactionExecute(sb.ToString(), opc);
            return dt;
        }

        public DataTable GetFlowStep(string PlanSn)
        {
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("SPM"));
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT STEPNAME FROM FLOWSTEP WHERE PLANSN=@PlanSn");
            sb.Append(" AND STEPID='3' ORDER BY STEPSN");
            ArrayList opc = new ArrayList();
            opc.Add(DataPara.CreateDataParameter("@PlanSn", DbType.String, PlanSn));
            return sdb.TransactionExecute(sb.ToString(), opc);
        }

        public DataTable GetFlowStep(string bu, string building,string phase)
        {
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            string sql = "SELECT STEP_NAME,SEQ FROM TB_NPI_DOAConfig WITH(NOLOCK) WHERE BU=@bu AND BUILDING=@building and PHASE=@PHASE"
                   + " ORDER BY SEQ DESC";
            ArrayList opc = new ArrayList();
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@bu", DbType.String, bu));
            opc.Add(DataPara.CreateDataParameter("@building", DbType.String, building));
            opc.Add(DataPara.CreateDataParameter("@PHASE", DbType.String,phase));
            DataTable dt = sdb.TransactionExecute(sql, opc);
            return dt;
        }
        
        public DataTable GetPrelaunchStepHander(string FormNo)
        {
            string sql = "SELECT * FROM TB_Prelaunch_Step_Handler WHERE FORMNO=@FormNo";
            ArrayList opc = new ArrayList();
            opc.Clear();
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            opc.Add(DataPara.CreateDataParameter("@FormNo", DbType.String, FormNo));
            return sdb.TransactionExecute(sql, opc);

        }

        public string GetGlobalUserInfo(string logon_id)
        {
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("SPM"));
            ArrayList opc = new ArrayList();
            StringBuilder sbErrMsg = new StringBuilder();
            string sql = string.Empty;
            sql = "SELECT UserId,LogonID,UserName,JOB,EMAIL,cust_1,cust_11,cust_12"
                + " FROM [USER] WITH(NOLOCK) WHERE LOGONID =  @LogonId"
                + " AND ENABLE='Y'";
            opc.Add(DataPara.CreateDataParameter("@LogonId", DbType.String, logon_id));
            DataTable dt = sdb.TransactionExecute(sql.ToString(), opc);
            if (dt.Rows.Count == 0)
            {

                sbErrMsg.AppendFormat("{0} GOBOOK不存在!", logon_id);
            }

            return sbErrMsg.ToString();
        }

        public string FlowStepHandlerConfig(string FormNo,string BU)
        {
              StringBuilder sb = new StringBuilder();
              StringBuilder Msg = new StringBuilder();
             SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
             sb.Append(" SELECT STEP_NAME FROM TB_Prelaunch_DOA");

             sb.Append(" WHERE  STEP_NAME NOT IN	");
             sb.Append("(SELECT DISTINCT STEP_NAME  FROM TB_Prelaunch_Step_Handler");
             sb.Append(" WHERE FORMNO=@FormNo)");
             sb.Append(" AND BU=@BU");
             ArrayList opc = new ArrayList();
             opc.Clear();
             opc.Add(DataPara.CreateDataParameter("@FormNo", DbType.String, FormNo));
             opc.Add(DataPara.CreateDataParameter("@BU", DbType.String, BU));
             DataTable dt = sdb.TransactionExecute(sb.ToString(),opc);

             if (dt.Rows.Count > 0)
             {
                 foreach (DataRow dr in dt.Rows)
                 {
                     Msg.AppendFormat("{0} 关卡签核人未设定", dr["STEPNAME"].ToString());
                 }
             }
             return Msg.ToString();
        }

        public DataTable GetPrelaunchMaster(string CaseId)
        {
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            ArrayList opc = new ArrayList();
            string sql=" SELECT *FROM TB_Prelaunch_Main WITH(NOLOCK)  WHERE CaseId=@CaseId";
            opc.Add(DataPara.CreateDataParameter("@CaseId", DbType.String,CaseId));
            return sdb.TransactionExecute(sql, opc);

        }

        public DataTable GetPrelaunchInconformity(string DocNo, string Handler, string StepName)
        {
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            ArrayList opc = new ArrayList();
            opc.Clear();
            StringBuilder sb = new StringBuilder();
            sb.Append("	SELECT  * FROM TB_Prelaunch_AppCheckItem ");
            sb.Append("	 WHERE Dept IN	");
            sb.Append("	 (SELECT DISTINCT Dept FROM TB_Prelaunch_Step_Handler where HANDLER=@Handler");
            sb.Append("	   AND FORMNO=@FormNo");
            sb.Append("	   AND STEP_NAME=@StepName)");
            sb.Append("	 AND PilotRunNO=@FormNo");

            opc.Add(DataPara.CreateDataParameter("@FormNo", DbType.String,DocNo));
            opc.Add(DataPara.CreateDataParameter("@Handler", DbType.String,Handler));
            opc.Add(DataPara.CreateDataParameter("@StepName", DbType.String,StepName));
           
            DataTable dt = sdb.TransactionExecute(sb.ToString(), opc);
            return dt;
        }

        public DataTable GetPrelaunchInconformity(string DocNo)
        {
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            ArrayList opc = new ArrayList();
            opc.Clear();
            StringBuilder sb = new StringBuilder();
            sb.Append("	SELECT  *FROM TB_Prelaunch_AppCheckItem  with(nolock) ");
            sb.Append("	 WHERE  PilotRunNO=@FormNo");
            opc.Add(DataPara.CreateDataParameter("@FormNo", DbType.String, DocNo));
            DataTable dt = sdb.TransactionExecute(sb.ToString(), opc);
            return dt;
        }

        public string  ValidationApproval(string DocNo, string Handler)
        {
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            ArrayList opc = new ArrayList();
            opc.Clear();
            string ErrMsg = string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.Append("	SELECT  *FROM TB_Prelaunch_AppCheckItem ");
            sb.Append("	 WHERE Dept IN	");
            sb.Append("	 (SELECT DISTINCT Dept FROM TB_Prelaunch_Step_Handler where HANDLER=@Handler");
            sb.Append("	   AND FORMNO=@FormNo");
            sb.Append("	   AND STEP_NAME='Dept.Prepared')");
            sb.Append("	 AND PilotRunNO=@FormNo");
             sb.Append(" AND Status is null");

            opc.Add(DataPara.CreateDataParameter("@FormNo", DbType.String, DocNo));
            opc.Add(DataPara.CreateDataParameter("@Handler", DbType.String, Handler));

            DataTable dt = sdb.TransactionExecute(sb.ToString(), opc);
            if (dt.Rows.Count > 0)
            {
                ErrMsg = "CheckItem未填寫完畢,請確認!";
            }
            return ErrMsg;
        }

        /// <summary>
        /// 获取待签关卡及签核人
        /// </summary>
        /// <param name="caseId"></param>
        /// <returns></returns>
  
        /// <summary>
        /// 获取待签关卡及签核人
        /// </summary>
        /// <param name="caseId"></param>
        /// <returns></returns>
        public virtual Dictionary<string, object> GetNextStepAndHandler(int caseId, string formNo, string stepName)
        {

            Dictionary<string, object> result = new Dictionary<string, object>();
            result["Result"] = false;
            result["ErrMsg"] = string.Empty;
            result["SPMRoutingVariable"] = string.Empty;
            try
            {
                SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
                ArrayList opc = new ArrayList();
                opc.Add(DataPara.CreateDataParameter("@P_BU", DbType.String, _BU, ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_CASE_ID", DbType.Int32, caseId, ParameterDirection.Input, 4));
                opc.Add(DataPara.CreateDataParameter("@P_STEP_NAME", DbType.String, stepName, ParameterDirection.Input, 30));
                opc.Add(DataPara.CreateDataParameter("@P_FORM_NO", DbType.String, formNo, ParameterDirection.Input, 30));
                opc.Add(DataPara.CreateDataParameter("@Result", DbType.String, null, ParameterDirection.Output, 1000));
                string tmp = sdb.ExecuteProcScalar("[P_OP_Prelaunch_NEXT_DOAHandler]", opc, "@Result");
                if (tmp.Length >= 3)
                {
                    if (tmp.Substring(0, 2) == "OK")
                    {
                        result["Result"] = true;
                        result["SPMRoutingVariable"] = tmp.Substring(3, tmp.Length - 3);
                    }

                    if (tmp.Substring(0, 2) == "NG")
                    {
                        result["ErrMsg"] = tmp.Substring(3, tmp.Length - 3);
                    }
                }
                else
                {
                    result["ErrMsg"] = "數據庫錯誤";
                }
            }
            catch (Exception ex)
            {
                result["ErrMsg"] = "DOA Handler获取錯誤，ErrMsg:" + ex.Message + "(" + ex.StackTrace + ")";
            }

            return result;
        }

        /// <summary>
        /// 更新关卡签核状态
        /// </summary>
        /// <param name="caseId"></param>
        /// <param name="step_name"></param>
        /// <returns></returns>
        public virtual Dictionary<string, object> UpdatePrelaunchDOAHandlerStatus(int caseId, string step_name, string handler, string approve_remark, string approve_result)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["Result"] = false;
            result["ErrMsg"] = string.Empty;
            try
            {
                SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
                ArrayList opc = new ArrayList();
                opc.Add(DataPara.CreateDataParameter("@P_OP_TYPE", DbType.String, "APPROVE", ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_CASE_ID", DbType.Int32, caseId, ParameterDirection.Input, 4));
                opc.Add(DataPara.CreateDataParameter("@P_STEP_NAME", DbType.String, step_name, ParameterDirection.Input, 50));
                 opc.Add(DataPara.CreateDataParameter("@P_Handler", DbType.String,handler, ParameterDirection.Input, 50));
                 opc.Add(DataPara.CreateDataParameter("@P_APPROVE_REMARK", DbType.String,approve_remark, ParameterDirection.Input, 50));
                 opc.Add(DataPara.CreateDataParameter("@P_APPROVE_RESULT ", DbType.String,approve_result, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@Result", DbType.String, null, ParameterDirection.Output, 1000));
                string tmp = sdb.ExecuteProcScalar("[P_OP_Prelaunch_DOAHandler]", opc, "@Result");
                if (tmp.Length >= 3)
                {
                    if (tmp.Substring(0, 2) == "OK")
                    {
                        result["Result"] = true;
                    }

                    if (tmp.Substring(0, 2) == "NG")
                    {
                        result["ErrMsg"] = tmp.Substring(3, tmp.Length - 3);
                    }
                }
                else
                {
                    result["ErrMsg"] = "數據庫錯誤";
                }
            }
            catch (Exception ex)
            {
                result["ErrMsg"] = "DOA狀態更新錯誤，ErrMsg:" + ex.Message + "(" + ex.StackTrace + ")";
            }

            return result;
        }

        public string GetNextStepAndHandler(string FormNo)
        {
            string sql = "SELECT  DISTINCT HANDLER FROM TB_Prelaunch_Step_Handler WHERE FORMNO=@FormNo"
               +" AND STEP_NAME='Dept.Prepared'";
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            ArrayList opc = new ArrayList();
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@FormNo", DbType.String, FormNo));
            DataTable dt = sdb.TransactionExecute(sql, opc);
            StringBuilder sbHandler =new StringBuilder();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    sbHandler.AppendFormat("{0},", dr["HANDLER"].ToString());
                }
            }
            return sbHandler.ToString().TrimEnd(',');
        }

        public void UpdatePreAppItem(string Status,string Desc,string CompleteDate,string ID,string Remark,string UpdateUser,string Attachment)
        {
            ArrayList opc = new ArrayList();
            string sql = "UPDATE TB_Prelaunch_AppCheckItem";
            sql += " SET Status=@Stauts,Description=@Desc,Remark=@Remark,";
            sql += " UpateUser=@UpdateUser,UpdateTime=@UpdateTime";
             opc.Add(DataPara.CreateDataParameter("@Stauts", DbType.String, Status));
             opc.Add(DataPara.CreateDataParameter("@Desc", DbType.String, Desc));
             opc.Add(DataPara.CreateDataParameter("@Remark", DbType.String,Remark));
             opc.Add(DataPara.CreateDataParameter("@UpdateUser", DbType.String, UpdateUser));
             opc.Add(DataPara.CreateDataParameter("@UpdateTime", DbType.String, DateTime.Now));

            if(!string.IsNullOrEmpty(CompleteDate.ToString()))
            {
                sql += ",CompleteDate=@CompleteDate";
                 opc.Add(DataPara.CreateDataParameter("@CompleteDate", DbType.String, CompleteDate));
            }
            if (!string.IsNullOrEmpty(Attachment))
            {
                sql += ",AttacheFile=@Attachment";
                opc.Add(DataPara.CreateDataParameter("@Attachment", DbType.String,Attachment));
            }
              sql+=" WHERE ID=@ID";
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            opc.Add(DataPara.CreateDataParameter("@ID", DbType.String,ID));
            sdb.TransactionExecuteNonQuery(sql, opc);

        }

        public string  GetPrelaunchCheckItem(string bu, string building)
        {
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            string sql = " SELECT *FROM TB_Prelaunch_CheckItemConfig"
                         + " WHERE Bu=@Bu AND Building=@Building";
            ArrayList opc = new ArrayList();
            opc.Add(DataPara.CreateDataParameter("@Bu", DbType.String,bu));
            opc.Add(DataPara.CreateDataParameter("@Building", DbType.String,building));
            DataTable dt = sdb.TransactionExecute(sql, opc);
            string ErrMsg = string.Empty;
            if (dt.Rows.Count==0)
            {
                ErrMsg = "CheckList未配置!";
            }
            return ErrMsg;
        }

        public DataTable GetPrelaunchApproveResult(string CaseId)
        {
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            ArrayList opc = new ArrayList();
            opc.Clear();
            StringBuilder sb = new StringBuilder();
            sb.Append("	SELECT  *FROM TB_Prelaunch_Step_Handler  with(nolock) ");
            sb.Append("	 WHERE  CASEID=@CaseId");
            sb.Append("	 order by SEQ");
            opc.Add(DataPara.CreateDataParameter("@CaseId", DbType.String, CaseId));
            DataTable dt = sdb.TransactionExecute(sb.ToString(), opc);
            return dt;
        }

        public DataTable GetNPIApproveResult(string CaseId)
        {
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            ArrayList opc = new ArrayList();
            opc.Clear();
            StringBuilder sb = new StringBuilder();
            sb.Append("	SELECT  *FROM TB_NPI_Step_Handler  with(nolock) ");
            sb.Append("	 WHERE  CASEID=@CaseId");
            sb.Append("	 AND  APPROVE_RESULT IS NOT NULL");
            sb.Append("  ORDER BY SEQ");
            opc.Add(DataPara.CreateDataParameter("@CaseId", DbType.String, CaseId));
            DataTable dt = sdb.TransactionExecute(sb.ToString(), opc);
            return dt;
        }

        #region[DOA处理]

        protected DataTable BuildDOAHanderDataTable()
        {
            DataTable dtDOAHander = new DataTable("Results");

            DataColumn dc1 = new DataColumn("CASE_ID", typeof(int));
            DataColumn dc2 = new DataColumn("FORM_NO", typeof(string));
            DataColumn dc21 = new DataColumn("SEQ", typeof(int));
            DataColumn dc3 = new DataColumn("STEP_NAME", typeof(string));
            DataColumn dc4 = new DataColumn("JUMP_FLAG", typeof(string));
            DataColumn dc5 = new DataColumn("HANDLER", typeof(string));
            DataColumn dc6 = new DataColumn("END_FLAG", typeof(string));
            DataColumn dc7 = new DataColumn("SIGN_FLAG", typeof(string));
            dtDOAHander.Columns.Add(dc1);
            dtDOAHander.Columns.Add(dc2);
            dtDOAHander.Columns.Add(dc21);
            dtDOAHander.Columns.Add(dc3);
            dtDOAHander.Columns.Add(dc4);
            dtDOAHander.Columns.Add(dc5);
            dtDOAHander.Columns.Add(dc6);
            dtDOAHander.Columns.Add(dc7);
            return dtDOAHander;
        }



        /// <summary>
        /// 依DOA设定生成需签核关卡及签核人信息
        /// </summary>
        /// <param name="oModel_BusinessTrip_DOA_Parameter"></param>
        /// <returns></returns>
        public virtual Model_NPI_DOA_HandlerInfo GenerateStepAndHandler(Model_NPI_DOA_Parameter oModel_NPI_DOA_Parameter)
        {
            Model_NPI_DOA_HandlerInfo oModel_NPI_DOA_HandlerInfo= new Model_NPI_DOA_HandlerInfo();
             oModel_NPI_DOA_HandlerInfo._RESULT=true;
            DataTable dtDOAHander = BuildDOAHanderDataTable();
            try
            {
                //獲取DOA设定参数
                string sql = "SELECT * FROM  TB_NPI_DOAConfig  with(nolock) WHERE BUILDING=@BUILDING AND BU = @BU and PHASE=@phase"
                + " AND ENABLED='Y' ORDER BY SEQ";

                ArrayList opc = new ArrayList();
                opc.Add(DataPara.CreateDataParameter("@BUILDING", DbType.String,oModel_NPI_DOA_Parameter._BUILDING));
                opc.Add(DataPara.CreateDataParameter("@BU", DbType.String, oModel_NPI_DOA_Parameter._BU));
                opc.Add(DataPara.CreateDataParameter("@phase", DbType.String,oModel_NPI_DOA_Parameter._PHASE));
                SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
                DataTable dt = sdb.TransactionExecute(sql, opc);
                if (dt.Rows.Count > 0)
                {

                    DataRow drDOAHander;

                    //循环处理所有签核关卡
                    foreach (DataRow drDOAConfig in dt.Rows)
                    {
                        drDOAHander = dtDOAHander.NewRow();
                        drDOAHander["CASE_ID"] = oModel_NPI_DOA_Parameter._CASE_ID;
                        drDOAHander["FORM_NO"] = oModel_NPI_DOA_Parameter._FORM_NO;
                        drDOAHander["SEQ"] = int.Parse(drDOAConfig["SEQ"].ToString());
                        drDOAHander["STEP_NAME"] = drDOAConfig["STEP_NAME"].ToString();
                        drDOAHander["JUMP_FLAG"] = drDOAConfig["JUMP_OPTION"].ToString();
                        drDOAHander["END_FLAG"] = "N";
                        drDOAHander["SIGN_FLAG"] = "N";
                        switch (drDOAConfig["CHECK_OPTION"].ToString())
                        {
                            case "A":
                                GetDOAHandler_Sourcer(ref drDOAHander,drDOAConfig, oModel_NPI_DOA_Parameter);
                                break;
                            
                        }
                        if (drDOAHander["JUMP_FLAG"].ToString() == "N")
                        {
                            ///防呆检查
                            if (drDOAHander["HANDLER"].ToString().Length == 0)
                            {
                                oModel_NPI_DOA_HandlerInfo._RESULT = false;
                                oModel_NPI_DOA_HandlerInfo._ERROR_MSG = string.Format("BU:{0} StepName:{1} handler not define", _BU, drDOAConfig["STEP_NAME"].ToString());
                                break;
                            }
                            else
                            {

                                dtDOAHander.Rows.Add(drDOAHander);
                            }
                        }

                    }

                    oModel_NPI_DOA_HandlerInfo._HANDLER = dtDOAHander;
                }


            }


            catch (Exception ex)
            {
                oModel_NPI_DOA_HandlerInfo._RESULT = false;
                oModel_NPI_DOA_HandlerInfo._ERROR_MSG = string.Format("BU:{0},FORM NO:{1} DOA check fail,ErrMsg:{2}", _BU, oModel_NPI_DOA_Parameter._FORM_NO, ex.Message);
            }
            return oModel_NPI_DOA_HandlerInfo;
        }



        ///// <summary>
        ///// 获取待签关卡及签核人
        ///// </summary>
        ///// <param name="caseId"></param>
        ///// <returns></returns>
        public virtual DataTable GetNextStepAndHandler(int caseId)
        {
            string sql = "select * from TB_NPI_Step_Handler with(nolock) WHERE CASEID=@CASEID AND SIGN_FLAG='N' order by seq";
            ArrayList opc = new ArrayList();
            opc.Add(DataPara.CreateDataParameter("@CASEID", DbType.Int32, caseId));
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            return sdb.TransactionExecute(sql, opc);
        }


        ///// <summary>
        ///// 获取all关卡及签核人
        ///// </summary>
        ///// <param name="caseId"></param>
        ///// <returns></returns>
        public virtual DataTable GetStepAndHandler(int caseId)
        {
            string sql = "select * from TB_DSC_STEP_HANDLER with(nolock) WHERE CASEID=@CASEID  order by seq";
            ArrayList opc = new ArrayList();
            opc.Add(DataPara.CreateDataParameter("@CASEID", DbType.Int32, caseId));
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("SPM"));
            return sdb.TransactionExecute(sql, opc);
        }

        ///// <summary>
        ///// 获取待签关卡及签核人
        ///// </summary>
        ///// <param name="caseId"></param>
        ///// <returns></returns>
        public virtual Dictionary<string, object> GetNextStepAndHandler_NPI(int caseId, string formNo, string stepName,string building,string Phase)
        {

            Dictionary<string, object> result = new Dictionary<string, object>();
            result["Result"] = false;
            result["ErrMsg"] = string.Empty;
            result["SPMRoutingVariable"] = string.Empty;
            try
            {
                SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
                ArrayList opc = new ArrayList();
               
                opc.Add(DataPara.CreateDataParameter("@P_BU", DbType.String, "HIS", ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_BUILDING", DbType.String,building, ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_CASE_ID", DbType.Int32, caseId, ParameterDirection.Input, 4));
                opc.Add(DataPara.CreateDataParameter("@P_STEP_NAME", DbType.String, stepName, ParameterDirection.Input, 30));
                opc.Add(DataPara.CreateDataParameter("@P_FORM_NO", DbType.String, formNo, ParameterDirection.Input, 30));
                opc.Add(DataPara.CreateDataParameter("@P_PHASE", DbType.String,Phase, ParameterDirection.Input, 30));
                opc.Add(DataPara.CreateDataParameter("@Result", DbType.String, null, ParameterDirection.Output, 1000));
                string tmp = sdb.ExecuteProcScalar("[P_OP_NPI_NEXT_DOAHandler]", opc, "@Result");
                if (tmp.Length >= 3)
                {
                    if (tmp.Substring(0, 2) == "OK")
                    {
                        result["Result"] = true;
                        result["SPMRoutingVariable"] = tmp.Substring(3, tmp.Length - 3);
                    }

                    if (tmp.Substring(0, 2) == "NG")
                    {
                        result["ErrMsg"] = tmp.Substring(3, tmp.Length - 3);
                    }
                }
                else
                {
                    result["ErrMsg"] = "數據庫錯誤";
                }
            }
            catch (Exception ex)
            {
                result["ErrMsg"] = "DOA Handler获取錯誤，ErrMsg:" + ex.Message + "(" + ex.StackTrace + ")";
            }

            return result;
        }



        /// <summary>
        /// 标准DOA-A:签核人为固定签核人
        /// </summary>
        /// <param name=drDOAHander">DOA HANDLER信息存储对象</param>
        /// <param name="oModel_DSC_DOA_Parameter">DOA判定参数</param>
        protected virtual void GetDOAHandler_Sourcer(ref DataRow drDOAHander, DataRow drDOAConfig,Model_NPI_DOA_Parameter oModel_NPI_DOA_Parameter)
        {
            string Step_Name=drDOAConfig["STEP_NAME"].ToString();
            string sql="select *from TB_NPI_DOA_DETAIL WITH(NOLOCK)" 
                       +"  WHERE STEP_NAME=@Step_Name"
                       +" and BU=@BU AND BUILDING=@BUILDING AND PHASE=@phase";
            ArrayList opc=new ArrayList();
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@Step_Name",DbType.String,Step_Name));
            opc.Add(DataPara.CreateDataParameter("@BU",DbType.String,oModel_NPI_DOA_Parameter._BU));
            opc.Add(DataPara.CreateDataParameter("@BUILDING",DbType.String,oModel_NPI_DOA_Parameter._BUILDING));
            opc.Add(DataPara.CreateDataParameter("@phase", DbType.String, oModel_NPI_DOA_Parameter._PHASE));
              
            SqlDB sdb=new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            DataTable dt=sdb.TransactionExecute(sql,opc);
            if (dt.Rows.Count> 0)
            {
                StringBuilder sbApprover=new StringBuilder();
                foreach(DataRow dr in dt.Rows)
                {
                    sbApprover.AppendFormat("{0},",dr["ENAME"].ToString());
                }
                drDOAHander["HANDLER"] =sbApprover.ToString().TrimEnd(',');
                drDOAHander["JUMP_FLAG"] = "N";
            }
            else
            {
                drDOAHander["HANDLER"]="";
                drDOAHander["JUMP_FLAG"] = "N";
            }

        }




        #endregion [标准DOA]

        #region
        /// <summary>
        /// 關卡及簽核人資訊寫DB
        /// </summary>
        /// <param name="oModel_DOA_Parameter"></param>
        /// <param name="dtDOAHandler"></param>
        /// <returns></returns>
        public virtual Dictionary<string, object> RecordDOAHandler(string caseid,string formno,string bu,string building,string phase)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["Result"] = false;
            result["ErrMsg"] = string.Empty;
            StringBuilder sb=new StringBuilder();
            sb.Append("INSERT INTO TB_NPI_Step_Handler(CASEID,FORMNO,STEP_NAME,DEPT,HANDLER,UPDATE_TIME,SIGN_FLAG,SEQ)");
            sb.Append(" SELECT @CASEID,@FORMNO,STEP_NAME,DEPT,ENAME,GETDATE(),@SIGN_FLAG,SEQ ");
            sb.Append(" FROM TB_NPI_DOA_DETAIL WHERE BU=@BU AND BUILDING=@BUILDING AND PHASE=@PHASE");
            ArrayList opc = new ArrayList();
            SqlDB sdb_T = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));

            try
            {
               
                opc.Clear();
                opc.Add(DataPara.CreateDataParameter("@CASEID", DbType.Int32, caseid));
                opc.Add(DataPara.CreateDataParameter("@FORMNO", DbType.String, formno));
                opc.Add(DataPara.CreateDataParameter("@SIGN_FLAG", DbType.String, "N"));
                opc.Add(DataPara.CreateDataParameter("@BU", DbType.String,bu));
                opc.Add(DataPara.CreateDataParameter("@BUILDING", DbType.String,building));
                opc.Add(DataPara.CreateDataParameter("@PHASE", DbType.String,phase));
                sdb_T.TransactionExecute(sb.ToString(), opc);
                result["Result"] = true;
            }

            catch (Exception ex)
            {
                
                result["ErrMsg"] = "DOA記錄錯誤，ErrMsg:" + ex.Message + "(" + ex.StackTrace + ")";
            }

            return result;
        }

        /// <summary>
        /// 更新关卡签核状态
        /// </summary>
        /// <param name="caseId"></param>
        /// <param name="step_name"></param>
        /// <returns></returns>
        public virtual Dictionary<string, object> UpdateDOAHandlerStatus(int caseId, string step_name,string handler,string ApproverReslut,string Remark)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["Result"] = false;
            result["ErrMsg"] = string.Empty;
            try
            {
                SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
                ArrayList opc = new ArrayList();
                opc.Add(DataPara.CreateDataParameter("@P_OP_TYPE", DbType.String, "APPROVE", ParameterDirection.Input, 10));
                opc.Add(DataPara.CreateDataParameter("@P_CASE_ID", DbType.Int32, caseId, ParameterDirection.Input, 4));
                opc.Add(DataPara.CreateDataParameter("@P_STEP_NAME", DbType.String, step_name, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_HANDLER", DbType.String, handler, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_APPROVER_RESULT", DbType.String, ApproverReslut, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@P_APPROVER_REMARK", DbType.String, Remark, ParameterDirection.Input,200));
                opc.Add(DataPara.CreateDataParameter("@Result", DbType.String, null, ParameterDirection.Output, 1000));
                string tmp = sdb.ExecuteProcScalar("[P_OP_NPI_DOAHandler_Reslult]", opc, "@Result");
               if (tmp.Length >= 3)
                {
                    if (tmp.Substring(0, 2) == "OK")
                    {
                        result["Result"] = true;
                    }

                    if (tmp.Substring(0, 2) == "NG")
                    {
                        result["ErrMsg"] = tmp.Substring(3, tmp.Length - 3);
                    }
                }
                else
                {
                    result["ErrMsg"] = "數據庫錯誤";
                }
            }
            catch (Exception ex)
            {
                result["ErrMsg"] = "DOA狀態更新錯誤，ErrMsg:" + ex.Message + "(" + ex.StackTrace + ")";
            }

            return result;
        }


        #endregion[DOA处理]
     
        #region [xml 與 Datable 互轉]
        public DataTable CreateXmlToDataTable(string xml)
        {
            DataTable dt = new DataTable();
            try
            {
                StringReader stream = new StringReader(xml);
                DataSet ds = new DataSet();
                ds.ReadXml(stream);
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
                stream.Close();
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {

            }
            return dt;
        }

        #endregion

    }

}
