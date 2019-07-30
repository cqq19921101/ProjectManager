using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using LiteOn.EA.DAL;
using System.Collections;
using System.IO;
using LiteOn.ICM.SIC;

/// <summary>
/// GDS_Helper 的摘要描述
/// </summary>
public class GDS_Helper
{
    static string conn = ConfigurationManager.AppSettings["DBConnectionUAT"];
    static SqlDB sdb = new SqlDB(conn);
    private ArrayList opc = new ArrayList();

    public GDS_Helper()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    
    #region XML Datatable互轉
    /// <summary>
    /// datatable convert to xml
    /// </summary>
    /// <param name="dt">datatable</param>
    /// <returns>字符串</returns>
    public string DataTableToXMLStr(DataTable dt)
    {
        StringWriter sw = new StringWriter();
        dt.WriteXml(sw);
        string temp = sw.ToString();
        sw.Close();
        sw.Dispose();
        temp = temp.Replace("&lt;", "(").Replace("&gt;", ")").Replace("&amp;", "/");//替換STR中C#自動轉換后敏感字符<,>,&
        return temp.Replace("<", "&lt1;").Replace(">", "&gt1;");//替換掉<,>，便於存儲於SPM
    }

    /// <summary>
    /// xml convert to datatable
    /// </summary>
    /// <param name="dr">SAP DATA</param>
    /// <param name="type">表身，或表頭</param>
    /// <returns>DataTable</returns>
    public DataTable XMLToDataTable(DataRow dr, string type)
    {
        if (type == "XMLHead")
        {
            DataTable dtHead = LiteOn.GDS.Utility.Tools.BuildHeadTable();
            StringReader sr = new StringReader(dr["xmlhead"].ToString().Trim());
            dtHead.ReadXml(sr);
            return dtHead;
        }
        else
        {
            StringReader sr2 = new StringReader(dr["xmlDetail"].ToString().Trim());
            DataTable dtDetail = LiteOn.GDS.Utility.Tools.BuildDetailTable();
            dtDetail.ReadXml(sr2);
            return dtDetail;
        }
    }
    #endregion

    #region SAP DATA 轉換

    /// <summary>
    /// 根據單號抓取HEAD的資料 將XML轉換成Datatable
    /// </summary>
    /// <param name="FormNo"></param>
    /// <returns></returns>
    public DataTable GetdtHead(string FormNo)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select xmlHead from TB_GDS_DATA where MBLNR_A = @MBLNR_A");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@MBLNR_A", SqlDbType.NVarChar, FormNo));
        DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            return XMLToDataTable(dr, "XMLHead");
        }
        return dt;
    }

    /// <summary>
    /// 根據單號抓取Detail的資料 將XML轉換成Datatable
    /// </summary>
    /// <param name="FormNo"></param>
    /// <returns></returns>
    public DataTable GetdtDetail(string FormNo)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select xmlDetail from TB_GDS_DATA where MBLNR_A = @MBLNR_A");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@MBLNR_A", SqlDbType.NVarChar, FormNo));
        DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            return XMLToDataTable(dr, "XMLDetail");
        }

        return dt;
    }

    #endregion

    #region Call SAP BAPI
   
    /// <summary>
    /// Call SAP BAPI -- Z_BAPI_GET_SPECIAL_DOC  將單子的結果回傳到SAP  W:表示在簽(Submit的時候)，A：表示approve(Approve的時候)，R：表示reject（Reject的時候）
    /// </summary>
    /// <returns></returns>
    public void  PostBackToSAP()
    {


        return true;
    }

    /// <summary>
    /// Call SAP BAPI -- Z_BAPI_GDS_SEND 檢查單據是否過賬  MBLNR欄位不為空 表示已經過賬 返回True 否則返回False
    /// </summary>
    /// <returns></returns>
    public bool CheckFormNoIsPass()
    {
        return true;
    }

        
    #endregion

    #region SQL 數據處理
    /// <summary>
    /// Begin關卡 Insert基本資料數據
    /// </summary>
    public void Insert_Begin(string WERKS,string MBLNR,string MBLNR_A,string KOSTL,string ABTEI,string Applicant,string ZEILE,
        string MATNR, int MENGE, string RTNIF, string Reason, string Remark, string IADocNo, string I6DocNo, string STATUS,  int CASEID)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"INSERT INTO [SPM].[dbo].[TB_GDS_ExceptionHandling]
                   ([WERKS]
                   ,[MBLNR]
                   ,[MBLNR_A]
                   ,[KOSTL]
                   ,[ABTEI]
                   ,[Applicant]
                   ,[ZEILE]
                   ,[MATNR]
                   ,[MENGE]
                   ,[RTNIF]
                   ,[Reason]
                   ,[Remark]
                   ,[IADocNo]
                   ,[I6DocNo]
                   ,[STATUS]
                   ,[CASEID])
             VALUES
                   (@WERKS,
                    @MBLNR,
                    @MBLNR_A,
                    @KOSTL,
                    @ABTEI,
                    @Applicant,
                    @ZEILE,
                    @MATNR,
                    @MENGE,
                    @RTNIF,
                    @Reason,
                    @Remark,
                    @IADocNo,
                    @I6DocNo,
                    @STATUS,
                    @CASEID)");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@WERKS", SqlDbType.NVarChar, WERKS));
        opc.Add(DataPara.CreateDataParameter("@MBLNR", SqlDbType.NVarChar, MBLNR));
        opc.Add(DataPara.CreateDataParameter("@MBLNR_A", SqlDbType.NVarChar, MBLNR_A));
        opc.Add(DataPara.CreateDataParameter("@KOSTL", SqlDbType.NVarChar, KOSTL));
        opc.Add(DataPara.CreateDataParameter("@ABTEI", SqlDbType.NVarChar, ABTEI));
        opc.Add(DataPara.CreateDataParameter("@Applicant", SqlDbType.NVarChar, Applicant));
        opc.Add(DataPara.CreateDataParameter("@ZEILE", SqlDbType.NVarChar, ZEILE));
        opc.Add(DataPara.CreateDataParameter("@MATNR", SqlDbType.NVarChar, MATNR));
        opc.Add(DataPara.CreateDataParameter("@MENGE", SqlDbType.Int, MENGE));
        opc.Add(DataPara.CreateDataParameter("@RTNIF", SqlDbType.NVarChar, RTNIF));
        opc.Add(DataPara.CreateDataParameter("@Reason", SqlDbType.NVarChar, Reason));
        opc.Add(DataPara.CreateDataParameter("@Remark", SqlDbType.NVarChar, Remark));
        opc.Add(DataPara.CreateDataParameter("@IADocNo", SqlDbType.NVarChar, IADocNo));
        opc.Add(DataPara.CreateDataParameter("@I6DocNo", SqlDbType.NVarChar, I6DocNo));
        opc.Add(DataPara.CreateDataParameter("@STATUS", SqlDbType.NVarChar, STATUS));
        opc.Add(DataPara.CreateDataParameter("@CASEID", SqlDbType.Int, CASEID));
        sdb.ExecuteNonQuery(sb.ToString(),opc);

    }
    #endregion


}
