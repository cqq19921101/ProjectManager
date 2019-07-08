using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;

public partial class NPI_File_Download : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string filePath = Context.Request.QueryString["filePath"].ToString();
            string fileName = Context.Request.QueryString["fileName"].ToString();
            DownloadFiles(fileName, filePath);
        }
    }

    private void DownloadFiles(string fileName, string filePath)
    {
        string Path = Server.MapPath(filePath);//路径

        FileInfo fileInfo = new FileInfo(Path);
        Response.Clear();
        Response.ClearContent();
        Response.ClearHeaders();
        Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName));
        Response.AddHeader("Content-Length", fileInfo.Length.ToString());
        Response.AddHeader("Content-Transfer-Encoding", "binary");
        Response.ContentType = "application/octet-stream";
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
        Response.WriteFile(fileInfo.FullName);
        Response.Flush();
        Response.End();
    }
}
