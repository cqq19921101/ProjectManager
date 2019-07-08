using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Data;
using Liteon.ICM.DataCore;
namespace LiteOn.EA.NPIReport.Utility
{
  

    public class NPI_Tools
    {




        /// <summary>
        /// 生成表單號
        /// </summary>
        /// <param name="CodeName"></param>
        /// <param name="CodeFix"></param>
        /// <returns></returns>
        public static string GetFormNO( string CodeFix)
        {
            string Prefix = CodeFix + DateTime.Today.ToString("yyyyMM");
            SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
            ArrayList opc = new ArrayList();
            opc.Add(DataPara.CreateDataParameter("@CODE1", DbType.String, Prefix, ParameterDirection.Input, 50));
            opc.Add(DataPara.CreateDataParameter("@CODE2", DbType.Int32, null, ParameterDirection.Output, 50));
            string no = string.Empty;
            try
            {
                no = sdb.ExecuteProcScalar("[p_GetNumber]", opc, "@CODE2");

            }
            catch (Exception)
            {
                throw;
            }
            return Prefix + "-" + int.Parse(no).ToString("0000#");
        }

     
    }
}
