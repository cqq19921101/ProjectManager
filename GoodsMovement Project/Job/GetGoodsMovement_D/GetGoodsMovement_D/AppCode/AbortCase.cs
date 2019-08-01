using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;

   public  class AbortCase
    {


      public  static  void AbortTask(int iCaseId)
        {
            try
            {
                IDbConnection cnWF = BorG.SPM.DataTier.DBUTILITY.DbConnection(BorG.SPM.DataTier.BPMCONFIG.GetDbConnectionString());
                cnWF.Open();

                IDbCommand cmdWF = BorG.SPM.DataTier.DBUTILITY.DbCommand("", ref cnWF);

                cmdWF.CommandText = "select TASKID, (SELECT LOGONID FROM [USER] WHERE USERID=HANDLER) AS LOGONID  from [TASK] where CASEID=" + iCaseId.ToString() + " and STEPTYPE=0 order by TASKID";

                IDataReader rd = cmdWF.ExecuteReader();
                if (rd.Read())
                {
                    int iTaskId = (int)rd["TASKID"];
                    string sLogonId = (string)rd["LOGONID"];

                   LogWriter.writeInfo(iCaseId + " " + iTaskId.ToString() + " " + sLogonId);

                    string xmlVariable = "<Variable><SPM_RECALL>Y</SPM_RECALL></Variable>";
                    string xmlData = "";

                    BorG.SPM.Designer.Runtime objEngine = new BorG.SPM.Designer.Runtime(false);

                    try
                    {
                        objEngine.HandleTask(sLogonId, iTaskId, xmlVariable, ref xmlData);
                        objEngine.set_Variables("OPINION", "(逾期系统自动Abort)");
                        objEngine.Send();
                    }
                    catch (Exception ex)
                    {
                        LogWriter.writeEx(ex.Message, ex);
                        if (objEngine.dbConnection.State == ConnectionState.Open)
                        {
                            if (!(objEngine.dbTransaction == null))
                            {
                                objEngine.dbTransaction.Rollback();
                            }
                        }
                    }
                    finally
                    {
                        objEngine.dbConnection.Close();
                        objEngine.dbConnection.Dispose();
                        objEngine.dbConnection = null;
                        objEngine = null;
                    }
                }
                rd.Close();
                cnWF.Close();
                cnWF.Dispose();
                cnWF = null;
            }
            catch (Exception e)
            {
                LogWriter.writeEx(e.Message, e);
                throw (e);
            }
        }


      

}
