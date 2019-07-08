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
using System.Text;
using LiteOn.EA.DAL;

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
        string Stage = string.Empty;
        string filePath_TW = filePath.Replace("Attachment","Attachment_TW");
        string Path = Server.MapPath(filePath);//路径CZ
        string PathTW = Server.MapPath(filePath_TW);//路径TW

        FileInfo fileInfo = new FileInfo(Path);//CZ
        FileInfo fileInfoTW = new FileInfo(PathTW);//TW

        Response.Clear();
        Response.ClearContent();
        Response.ClearHeaders();
        Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName));
        Response.AddHeader("Content-Length", fileInfo.Length.ToString());
        Response.AddHeader("Content-Transfer-Encoding", "binary");
        Response.ContentType = "application/octet-stream";
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
        Response.WriteFile(fileInfo.FullName);
        //Response.WriteFile(fileInfoTW.FullName);
        Response.Flush();
        Response.End();

    }








}
