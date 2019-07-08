using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LiteOn.EA.BLL;
using LiteOn.EA.DAL;
using LiteOn.EA.CommonModel;
using Model.Base;
using System.Web.Script.Serialization;
using System.Collections;
using System.Text;
using System.Configuration;

namespace Toilet_Usage_Status.ajax
{
    /// <summary>
    /// Handler1 的摘要描述
    /// </summary>
    public class CheckIsExists : IHttpHandler
    {
        static string conn = ConfigurationManager.AppSettings["ConnectionString"];
        SqlDB sdb = new SqlDB(conn);
        ArrayList opc = new ArrayList();
        public static string Result = "";
        JavaScriptSerializer jss = new JavaScriptSerializer();
        HttpContext context = null;
        ReturnMessage rm = new ReturnMessage();

        public void ProcessRequest(HttpContext context)
        {
            this.context = context;
            string cmd = context.Request.Form["cmd"];
            switch (cmd)
            {
                case "EArea-W":
                    Result = Check(cmd);
                    break;
            }
            context.Response.Write(Result);
        }
        /// <summary>
        /// Check方法  檢查此區域是否有人正在使用  0X30 無人使用 / 0x31 有人使用
        /// </summary>
        /// <param name="command"></param>
        /// <returns>rm</returns>
        public  string Check(string command)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"select Top 1 * from TB_Toilet where 1=1");

                switch (command)
                {
                    case "EArea-W"://E區 女
                        sb.Append(" and DevAddr = '0175963d'");
                        break;
                }
                opc.Clear();
                sb.Append(" order by SID DESC");
                string Result = sdb.GetRowString(sb.ToString(), opc, "DevValue");

                //成功返回 True
                rm.Success = true;
                rm.Info = Result;
            }
            catch (Exception ex)
            {
                //異常返回 False
                rm.Success = false;
                rm.Info = "網頁Error:" + ex.ToString() + "Please Contact IT Support";
            }
            return jss.Serialize(rm);//將結果轉成json回傳到前臺
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}