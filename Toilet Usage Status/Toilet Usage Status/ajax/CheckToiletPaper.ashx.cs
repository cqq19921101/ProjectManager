using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LiteOn.EA.BLL;
using LiteOn.EA.DAL;
using System.Collections;
using System.Text;
using LiteOn.Corp.IT.EA.CMCC;
using System.Web.Script.Serialization;
using Model.Base;
using System.Data;
using System.Configuration;

namespace Toilet_Usage_Status.ajax
{
    /// <summary>
    /// CheckToiletPaper 的摘要描述
    /// </summary>
    public class CheckToiletPaper : IHttpHandler
    {
        static string conn = ConfigurationManager.AppSettings["ConnectionString"];
        SqlDB sdb = new SqlDB(conn);
        ArrayList opc = new ArrayList();
        HttpContext context = null;
        private string Result = "";
        JavaScriptSerializer jss = new JavaScriptSerializer();
        ReturnMessage rm = new ReturnMessage();
        public void ProcessRequest(HttpContext context)
        {
            this.context = context;
            string command = context.Request.Form["cmd"];
            switch (command)
            {
                case "E-Area"://E區
                case "F-Area"://F區
                case "A-Area"://A區
                case "V-Area"://VIP區
                    Result = CheckAreaPaper(command);
                    break;
            }

            context.Response.Write(Result);//返回值
        }
        /// <summary>
        /// 檢查廁紙容量 百分比  By區域（E,A,F,V）
        /// </summary>
        /// <param name="command"></param>
        /// <returns>DevValue</returns>
        public string CheckAreaPaper(string command)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select Top 1 * from TB_Toilet where 1=1");
                opc.Clear();
                switch (command)
                {
                    case "E-Area":
                        sb.Append(" and DevAddr = '014cbb37'");
                        break;
                    case "F-Area":
                        sb.Append(" and DevAddr = ''");
                        break;
                    case "A-Area":
                        sb.Append(" and DevAddr = ''");
                        break;
                    case "V-Area":
                        sb.Append(" and DevAddr = ''");
                        break;
                }
                sb.Append(" order by SID desc");

                string Result = sdb.GetRowString(sb.ToString(), opc, "DevValue");//獲取廁紙容量
                if (Result == "")//DB該字段為空 表示沒有該位置的資料  返回false  顯示無數據
                {
                    rm.Success = true;
                    rm.Info = "無數據";
                }
                else
                {
                    rm.Success = true;
                    rm.Info = Result;

                }
            }
            catch (Exception ex)
            {
                rm.Success = false;

                rm.Info = "網頁Error:" + ex.ToString() + "Please Contact IT Support";
            }
            return jss.Serialize(rm);

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