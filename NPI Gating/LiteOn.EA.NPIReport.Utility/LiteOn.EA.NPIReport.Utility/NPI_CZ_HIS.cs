using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Liteon.ICM.DataCore;
using LiteOn.EA.Borg.Utility;

namespace LiteOn.EA.NPIReport.Utility
{
	/// <summary>
	/// Summary description for Leave_CZ_HIS.
	/// </summary>
	public class NPI_CZ_HIS:NPI_Standard
	{
       
        public NPI_CZ_HIS(string site, string bu)
            : base(site, bu)
        {
        }
     
        // 重載權限設定
        public override Dictionary<string, object> RecordOperation_NPIMember(Model_NPI_MEMBER oModel_NPI_MEMBER, Status_Operation RecordOperation)
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
                opc.Add(DataPara.CreateDataParameter("@P_PHASE", DbType.String, oModel_NPI_MEMBER._PHASE, ParameterDirection.Input, 30));
               
                opc.Add(DataPara.CreateDataParameter("@Result", DbType.String, null, ParameterDirection.Output, 1000));
                SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
                Hashtable htResult = sdb.ExecuteProc("P_OP_NPI_MEMBER_HIS", opc);
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

        public override DataTable GetMasterInfo(string CaseId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT *from TB_NPI_APP_MAIN_HIS WITH(NOLOCK)");
            sql.Append(" WHERE CASEID=@CASEID");
            ArrayList opc = new ArrayList();
            opc.Add(DataPara.CreateDataParameter("@CaseId", DbType.String, CaseId));
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            DataTable dt = sdb.TransactionExecute(sql.ToString(), opc);
            return dt;
        }

    }
}
