using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Lib_SQM_Domain.Model
{
  public static  class Common
    {
        public static string  ExportExcel(DataTable dt,string localPath,string fileName)
        {
            
            try
            {
                //获取指定虚拟路径的物理路径
                string path = localPath + @"Source\Aspose.Cells\Aspose.Cells.lic";

                //读取 License 文件
                Stream stream = (Stream)File.OpenRead(path);

                //注册 License
                Aspose.Cells.License li = new Aspose.Cells.License();
                li.SetLicense(stream);

                //创建一个工作簿
                Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook();

                //创建一个 sheet 表
                Aspose.Cells.Worksheet worksheet = workbook.Worksheets[0];

                //设置 sheet 表名称
                worksheet.Name = dt.TableName;

                Aspose.Cells.Cell cell;

                int rowIndex = 0;   //行的起始下标为 0
                int colIndex = 0;   //列的起始下标为 0

                //设置列名
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    //获取第一行的每个单元格
                    cell = worksheet.Cells[rowIndex, colIndex + i];

                    //设置列名
                    cell.PutValue(dt.Columns[i].ColumnName);
                }

                //跳过第一行，第一行写入了列名
                rowIndex++;

                //写入数据
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        cell = worksheet.Cells[rowIndex + i, colIndex + j];

                        cell.PutValue(dt.Rows[i][j]);
                    }
                }

                //自动列宽
                worksheet.AutoFitColumns();

                //设置导出文件路径
                path = localPath + @"UploadFile\SQM\";

                //设置新建文件路径及名称
              
                string savePath = path + fileName;

                //创建文件
                FileStream file = new FileStream(savePath, FileMode.CreateNew);

                //关闭释放流，不然没办法写入数据
                file.Close();
                file.Dispose();

                //保存至指定路径
                workbook.Save(savePath);

                //System.IO.FileInfo file1 = new System.IO.FileInfo(savePath);
                //HttpContext.Current.Response.Clear();
                //HttpContext.Current.Response.Charset = "GB2312";
                //HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
                //// 添加头信息，为"文件下载/另存为"对话框指定默认文件名 
                //HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                //// 添加头信息，指定文件大小，让浏览器能够显示下载进度 
                //HttpContext.Current.Response.AddHeader("Content-Length", file1.Length.ToString());

                //// 指定返回的是一个不能被客户端读取的流，必须被下载 
                //HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";

                //// 把文件流发送到客户端 
                //HttpContext.Current.Response.WriteFile(file1.FullName);
                //// 停止页面的执行 

                //HttpContext.Current.Response.Flush();
                //HttpContext.Current.Response.End();
                ////或者使用下面的方法，输出到浏览器下载。
                //byte[] bytes = workbook.SaveToStream().ToArray();
                //OutputClient(bytes);

                worksheet = null;
                workbook = null;
          
            }
            catch (Exception ex)
            {

            }
            return fileName;
        }
        public static void OutputClient(byte[] bytes)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.Buffer = true;

           

            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("online; filename={0}.xls", DateTime.Now.ToString("yyyy-MM-dd-HH-mm")));

            HttpContext.Current.Response.Charset = "GB2312";
            HttpContext.Current.Response.ContentEncoding = Encoding.GetEncoding("GB2312");

            HttpContext.Current.Response.BinaryWrite(bytes);
            
            HttpContext.Current.Response.Flush();
            

            
            HttpContext.Current.Response.End();
        }
    }

}
