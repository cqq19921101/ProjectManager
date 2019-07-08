using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using System.Data;
using System.Data.OleDb;
using LiteOn.EA.Model;
using LiteOn.EA.BLL;
using System.Collections;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using LiteOn.EA.CommonModel;
using LiteOn.EA.Borg.Utility;
using Liteon.ICM.DataCore;
using LiteOn.EA.NPIReport.Utility;
using OfficeOpenXml;


public partial class Web_E_Report_DFX_Items_Maintain : System.Web.UI.Page
{
    SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
    ArrayList opc = new ArrayList();
    static int function_id = 1274;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserName"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        else
        {
            lblLogonId.Text = Session["UserName"].ToString();
        }
        if (!IsPostBack)
        {
            UserRole UserRole_class = new UserRole();
            if (UserRole_class.checkRole(lblLogonId.Text, function_id) == false)
            {
                Alert("对不起，你没有权限操作!");
                return;
            }
            Model_BorgUserInfo oModel_BorgUserInfo = new Model_BorgUserInfo();
            Borg_User oBorg_User = new Borg_User();
            oModel_BorgUserInfo = oBorg_User.GetUserInfoByLogonId(lblLogonId.Text);
            if (oModel_BorgUserInfo._EXISTS)
            {
                lblBu.Text = oModel_BorgUserInfo._BU;
                lblBuilding.Text = oModel_BorgUserInfo._Building;
            }
            DataTable dt = GetVersion();//版本號
            DataTable dtV = GetVersionLog();//變更記錄

            //抓取當前資料的版本號
            if (dt.Rows.Count > 0)
            {
                string Version = dt.Rows[0]["Item"].ToString();
                txtVersion.Text = Version.Substring(Version.Length - 1, 1);
            }

            //加載變更記錄
            if (dtV.Rows.Count > 0)
            {
                for (int i = 0; i < dtV.Rows.Count; i++)
                {
                    DataRow drv = dtV.Rows[i];
                    string str1 = "舊版本:" + drv["DFXTypeB"].ToString() + "   " + "修改版本:" + drv["DFXTypeE"].ToString() + System.Environment.NewLine;
                    string str2 = "Reason:" + drv["Reason"].ToString() + System.Environment.NewLine;
                    txtVersionLog.Text += string.Format("{0}{1}", str1, str2);
                }
            }

            BindDept();

        }
    }

    private void DeleteItem(string item, string ProductType)
    {
        string sql = "DELETE TB_DFX_Item WHERE Item=@Item and ProductType=@ProductType";
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@Item", DbType.String, item));
        opc.Add(DataPara.CreateDataParameter("@ProductType", DbType.String, ProductType));
        sdb.TransactionExecuteNonQuery(sql, opc);

    }

    protected void btnUpload_Click(object sender, DirectEventArgs e)
    {
        StringBuilder ErrMsg = new StringBuilder();
        NPIMgmt oMgmt = new NPIMgmt(lblSite.Text, lblBu.Text);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
        string File = FileUpload.FileName;
        string ProductType = cmbProduct.SelectedItem.Text;
        string Reason = txtRequirements.Text;
        if (ProductType.Length == 0)
        {
            Alert("請選擇產品類別！");
            return;
        }
        else if (!FileUpload.HasFile)
        {
            Alert("請選擇上傳文件！");
            return;
        }
        else if (txtVersion.Text != cmbDFXType.Text && txtRequirements.Text.Length == 0 && txtVersion.Text != "")
        {
            Alert("版本變更時,需填寫變更原因！");
            return;
        }
        else
        {
            string type = File.Substring(File.LastIndexOf(".") + 1).ToLower();
            if (type == "xlsx")
            {
                Stream stream = FileUpload.PostedFile.InputStream;
                if (stream.Length == 8889)
                {
                    Alert("導入的資料表為空,請檢查!");
                    return;
                }
                DataTable dt = ReadByExcelLibrary(stream);
                if (dt.Rows.Count > 0)
                {
                    Model_DFX_Item oModel = new Model_DFX_Item();
                    DeleteExsit(); // 刪除之前存在的數據
                    #region 數據完整性檢查
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string DFXType = cmbDFXType.SelectedItem.Text;

                        string WriteDept = dt.Rows[i]["WriteDept"].ToString(); // 部門
                        string OldItemType = dt.Rows[i]["OldItemType"].ToString();// Type
                        string ItemID = dt.Rows[i]["ItemID"].ToString(); //Item(1.1,1.2 .... )
                        string Item = dt.Rows[i]["Item"].ToString(); //編號
                        string ItemType = dt.Rows[i]["ItemType"].ToString(); //類別
                        string ItemName = dt.Rows[i]["ItemName"].ToString(); //項目
                        string Requirements = dt.Rows[i]["Requirements"].ToString(); //DF Requirements
                        string PriorityLevel = dt.Rows[i]["PriorityLevel"].ToString(); //Priority Level
                        string ReplyDept = dt.Rows[i]["ReplyDept"].ToString(); //RD Owner
                        string Losses = dt.Rows[i]["Losses"].ToString(); // 違反損失項
                        string DFXTypeSub = Item.Substring(Item.Length - 1, 1); //抓取編碼的DFX版本號判斷

                        DataTable dtitem = CheckItem(Item); //檢查編號是否重複
                        #region 非空檢查 數據輸入檢查
                        if (WriteDept.Length == 0)
                        {
                            ErrMsg.Append("第" + (i + 2) + "行");
                            ErrMsg.Append("部門不能为空!</br>");
                            ErrMsg.Append("<br/>");
                        }
                        else if (OldItemType.Length == 0)
                        {
                            ErrMsg.Append("第" + (i + 2) + "行");
                            ErrMsg.Append("Type不能为空!</br>");
                            ErrMsg.Append("<br/>");
                        }
                        else if (ItemID.Length == 0)
                        {
                            ErrMsg.Append("第" + (i + 2) + "行");
                            ErrMsg.Append("Item不能为空!</br>");
                            ErrMsg.Append("<br/>");
                        }
                        else if (Item.Length == 0 || Item.Length != 11)
                        {
                            ErrMsg.Append("第" + (i + 2) + "行");
                            ErrMsg.Append("編號不能为空且固定11碼!</br>");
                            ErrMsg.Append("<br/>");
                        }
                        else if (ItemType.Length == 0)
                        {
                            ErrMsg.Append("第" + (i + 2) + "行");
                            ErrMsg.Append("類別不能为空!</br>");
                            ErrMsg.Append("<br/>");
                        }
                        else if (ItemName.Length == 0)
                        {
                            ErrMsg.Append("第" + (i + 2) + "行");
                            ErrMsg.Append("編碼不能为空!</br>");
                            ErrMsg.Append("<br/>");
                        }
                        else if (Requirements.Length == 0)
                        {
                            ErrMsg.Append("第" + (i + 2) + "行");
                            ErrMsg.Append("DF Requirements不能为空!</br>");
                            ErrMsg.Append("<br/>");
                        }
                        else if (PriorityLevel.Length == 0)
                        {
                            ErrMsg.Append("第" + (i + 2) + "行");
                            ErrMsg.Append("PriorityLevel不能为空!</br>");
                            ErrMsg.Append("<br/>");
                        }
                        else if (PriorityLevel.Length == 0)
                        {
                            ErrMsg.Append("第" + (i + 2) + "行");
                            ErrMsg.Append("PriorityLevel不能为空!</br>");
                            ErrMsg.Append("<br/>");
                        }
                        else if (ReplyDept.Length == 0)
                        {
                            ErrMsg.Append("第" + (i + 2) + "行");
                            ErrMsg.Append("RD Owner不能为空!</br>");
                            ErrMsg.Append("<br/>");
                        }
                        else if (Losses.Length == 0)
                        {
                            ErrMsg.Append("第" + (i + 2) + "行");
                            ErrMsg.Append("違反損失項不能为空!</br>");
                            ErrMsg.Append("<br/>");
                        }
                        else
                        {
                            //if (DFXTypeSub != DFXType)
                            //{
                            //    ErrMsg.Append("第" + (i + 2) + "行");
                            //    ErrMsg.Append("編碼對應的DFX版本錯誤!</br>");
                            //}
                            if (dtitem.Rows.Count > 0)
                            {
                                ErrMsg.Append("第" + (i + 2) + "行");
                                ErrMsg.Append("該編碼已存在!</br>");
                            }
                            if (!IsWriteDept(WriteDept.Trim()))
                            {
                                ErrMsg.Append("第" + (i + 2) + "行");
                                ErrMsg.Append("部門填寫錯誤!</br>");
                            }
                            if (!IsReplyDept(ReplyDept.Trim()))
                            {
                                ErrMsg.Append("第" + (i + 2) + "行");
                                ErrMsg.Append("RD Owner填寫錯誤!</br>");
                            }
                            if (!IsPrioritylevel(PriorityLevel))
                            {
                                ErrMsg.Append("第" + (i + 2) + "行");
                                ErrMsg.Append("Prioritylevel填寫錯誤!</br>");
                            }
                            if (!IsLossItem(Losses.Trim()))
                            {
                                ErrMsg.Append("第" + (i + 2) + "行");
                                ErrMsg.Append("違反損失項填寫錯誤!</br>");
                            }

                        }
                        #endregion
                    #endregion

                        if (string.IsNullOrEmpty(ErrMsg.ToString()))
                        {

                            oModel._BU = "POWER";
                            oModel._BUILDING = lblBuilding.Text;
                            oModel._ItemID = ItemID;
                            oModel._Item = Item;
                            oModel._ItemType = ItemType;
                            oModel._ItemName = ItemName;
                            oModel._Requirements = Requirements;
                            oModel._ProductType = ProductType;
                            oModel._PriorityLevel = int.Parse(PriorityLevel);
                            oModel._Losses = Losses;
                            oModel._WriteDept = WriteDept;
                            oModel._ReplyDept = ReplyDept;
                            oModel._OldItemType = OldItemType;
                            string tmp = oStandard.RecordOperation_DFXItemUpload(oModel, Status_Operation.ADD);
                            if (tmp.Length >= 3)
                            {
                                if (tmp.Substring(0, 2) == "NG")
                                {
                                    //接收SP返回的ERR MSG
                                    Alert(tmp.Substring(3, tmp.Length - 3));
                                }
                                if (tmp.Substring(0, 2) == "OK")
                                {
                                    if (tmp.Substring(3, tmp.Length - 3) == "1")
                                    {
                                        Alert("上傳成功!DFX信息維護完成!");
                                    }
                                }
                            }
                        }
                        else
                        {
                            Alert(ErrMsg.ToString());
                            //BindDFX();

                        }
                    }
                }
            }
            else
            {
                Alert("文件類型只能是.xlsx");
                return;
            }
        }
        if (ErrMsg.Length > 0)
        {
            DeleteExsit(); // 刪除之前存在的數據
            BindDFX();
        }
        else
        {
            if (txtVersion.Text != cmbDFXType.Text && txtRequirements.Text.Length > 0)
            {
                string DFXTypeB = txtVersion.Text;
                string DFXTypeE = cmbDFXType.SelectedItem.Text;
                InsertVersionLog(DFXTypeB, DFXTypeE, Reason);
            }

            BindDFX();

        }
    }

    protected void grdInfo_RowCommand(object sender, DirectEventArgs e)
    {
        txtItem.Text = e.ExtraParams["Item"].Substring(0, 9);
        txtID.Text = e.ExtraParams["ID"].ToString();
        ConPicture.Hidden = false;
    }

    protected void btnUploadP_Click(object sender, DirectEventArgs e)
    {
        string Product = cmbProduct.SelectedItem.Text;
        string ID = txtID.Text;
        string Item = txtItem.Text;
        string MFILE_PATH = string.Empty;
        string MFILE_NAME = string.Empty;
        string errorMsg = "";

        if (!UploadFile.HasFile)
        {
            Alert("請選擇上傳附件!");
            return;
        }
        if (txtItem.Text.Length == 0)
        {
            errorMsg += "Item,";
        }
        if (errorMsg.Length > 0)
        {
            Alert(errorMsg.Substring(0, errorMsg.Length - 1) + "不能為空");
            return;
        }

        int indexMeeting = UploadFile.FileName.LastIndexOf('.');
        string extMeeting = UploadFile.FileName.Substring(indexMeeting + 1);
        string fileName = UploadFile.FileName.Substring(UploadFile.FileName.LastIndexOf("\\") + 1);
        string[] arraryFile;
        arraryFile = fileName.Split('.');
        string ItemName = arraryFile[0];

        if (extMeeting.ToLower() != "jpg")
        {
            Alert("圖片類型只能是JPG格式!");
            return;
        }
        if (ItemName != Item)
        {
            Alert("圖片名必須和編碼一致！");
            return;
        }
        //檢查是否存在存放當前專案上傳的文件夾
        string docNoPath = Server.MapPath("~/Web/E-Report/DFXPicture/");
        string filepath = (docNoPath + "/" + MFILE_NAME);
        bool IsDocNoExist = Directory.Exists(docNoPath);
        bool IsSubDocNoExist = Directory.Exists(docNoPath);
        if (!IsSubDocNoExist)
        {
            Directory.CreateDirectory(docNoPath);
        }
        DataTable dt = CheckItem2(Item);

        if (dt.Rows.Count == 0)
        {
            #region 文件上傳操作 ADD
            try
            {
                MFILE_PATH = Helper.ConvertChinese("DFXPicture/" + fileName, "Big5");
                MFILE_NAME = fileName;
                string oldfilepath = (docNoPath + "/" + MFILE_NAME);

                UploadFile.PostedFile.SaveAs(docNoPath + "/" + fileName);

                opc.Clear();
                opc.Add(DataPara.CreateDataParameter("@P_Type", DbType.String, "ADD", ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@Item", DbType.String, Item, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@MFILE_PATH", DbType.String, Helper.ConvertChinese(MFILE_PATH, "Big5"), ParameterDirection.Input, 255));
                opc.Add(DataPara.CreateDataParameter("@MFILE_NAME", DbType.String, Helper.ConvertChinese(MFILE_NAME, "Big5"), ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@UPDATE_TIME", DbType.DateTime, DateTime.Now, ParameterDirection.Input, 8));
                opc.Add(DataPara.CreateDataParameter("@UPDATE_USERID", DbType.String, lblLogonId.Text, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@Result", DbType.String, null, ParameterDirection.Output, 1000));
                //SP執行結果返回固定格式
                //1 OK; 
                //2 NG;ERR MSG
                try
                {

                    string tmp = sdb.ExecuteProcScalar("[P_Upload_DFXPicture]", opc, "@Result");
                    if (tmp.Length >= 3)
                    {

                        if (tmp.Substring(0, 2) == "NG")
                        {

                            Alert("文件上傳失敗!" + tmp.Substring(3, tmp.Length - 3));
                            return;
                        }
                        else if (tmp.Substring(0, 2) == "OK")
                        {
                            Alert("文件上傳成功！");
                        }
                    }
                    else
                    {
                        Alert("文件上傳成功！");
                    }
                }
                catch (Exception ex)
                {
                    File.Delete(@oldfilepath); ;//刪除已存在的文件
                    Alert("文件上傳失敗!" + ex.Message);
                }

            }
            catch (Exception ex)
            {
                DeleteExistFiles(docNoPath);
                Alert("文件上傳失敗!DB ERROR:" + ex.Message);
                return;
            }

            #endregion
        }
        else
        {
            #region 文件上傳操作 Updates
            try
            {
                DataRow dr = dt.Rows[0];
                MFILE_PATH = Helper.ConvertChinese("DFXPicture/" + fileName, "Big5");
                MFILE_NAME = fileName;

                string oldfilepath = (docNoPath + "/" + dr["FILE_NAME"].ToString());
                string FilePath1 = (docNoPath + "/" + fileName);

                UploadFile.PostedFile.SaveAs(docNoPath + "/" + fileName);

                opc.Clear();
                opc.Add(DataPara.CreateDataParameter("@P_Type", DbType.String, "Update", ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@Item", DbType.String, Item, ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@MFILE_PATH", DbType.String, Helper.ConvertChinese(MFILE_PATH, "Big5"), ParameterDirection.Input, 255));
                opc.Add(DataPara.CreateDataParameter("@MFILE_NAME", DbType.String, Helper.ConvertChinese(MFILE_NAME, "Big5"), ParameterDirection.Input, 50));
                opc.Add(DataPara.CreateDataParameter("@UPDATE_TIME", DbType.DateTime, DateTime.Now, ParameterDirection.Input, 8));
                opc.Add(DataPara.CreateDataParameter("@UPDATE_USERID", DbType.String, lblLogonId.Text, ParameterDirection.Input, 20));
                opc.Add(DataPara.CreateDataParameter("@Result", DbType.String, null, ParameterDirection.Output, 1000));
                //SP執行結果返回固定格式
                //1 OK; 
                //2 NG;ERR MSG
                try
                {

                    string tmp = sdb.ExecuteProcScalar("[P_Upload_DFXPicture]", opc, "@Result");
                    if (tmp.Length >= 3)
                    {

                        if (tmp.Substring(0, 2) == "NG")
                        {

                            Alert("文件上傳失敗!" + tmp.Substring(3, tmp.Length - 3));
                            File.Delete(@FilePath1);
                            return;
                        }
                        else if (tmp.Substring(0, 2) == "OK")
                        {
                            Alert("文件上傳成功！");
                            File.Delete(@oldfilepath);
                        }
                    }
                    else
                    {
                        Alert("文件上傳成功！");
                    }
                }
                catch (Exception ex)
                {
                    Alert("文件上傳失敗!" + ex.Message);
                }

            }
            catch (Exception ex)
            {
                Alert("文件上傳失敗!DB ERROR:" + ex.Message);
                //File.Delete(@FilePath1);
                return;
            }

            #endregion
        }


        BindDFX(Product);

    }

    protected void btnDeleteP_Click(object sender, DirectEventArgs e)
    {
        string Product = cmbProduct.SelectedItem.Text;
        string ID = txtID.Text;
        string Item = txtItem.Text;
        string MFILE_PATH = string.Empty;
        string MFILE_NAME = string.Empty;
        string errorMsg = "";
        string docNoPath = Server.MapPath("~/Web/E-Report/DFXPicture/");
        if (txtItem.Text.Length == 0)
        {
            errorMsg += "Item,";
        }
        if (errorMsg.Length > 0)
        {
            Alert(errorMsg.Substring(0, errorMsg.Length - 1) + "不能為空");
            return;
        }
        DataTable dt = CheckItem2(Item);
        if (dt.Rows.Count == 0)
        {
            Alert("請先上傳圖片資料！");
            return;
        }
        else
        {
            DataRow dr = dt.Rows[0];
            string oldfilepath = (docNoPath + "/" + dr["FILE_NAME"].ToString());
            StringBuilder sb = new StringBuilder();
            sb.Append(@"delete from TB_DFX_Picture where Item = @Item ");
            opc.Clear();
            opc.Add(DataPara.CreateDataParameter("@Item", DbType.String, Item));
            try
            {
                sdb.TransactionExecuteNonQuery(sb.ToString(), opc);
                Alert("刪除成功！");
                File.Delete(@oldfilepath);
            }
            catch (Exception ex)
            {
                Alert("上傳失敗:" + ex.Message);
            }
        }
        BindDFX(Product);

    }


    #region 附件上傳判斷
    private bool IsPrioritylevel(string str)
    {
        bool Status = false;
        switch (str)
        {
            case "0":
                Status = true;
                break;
            case "1":
                Status = true;
                break;
            case "2":
                Status = true;
                break;
            case "3":
                Status = true;
                break;
        }
        return Status;
    }

    private bool IsLossItem(string str)
    {
        bool Status = false;
        switch (str)
        {
            case "品質損失":
                Status = true;
                break;
            case "成本損失":
                Status = true;
                break;
            case "人工效率損失":
                Status = true;
                break;
            case "設備效率損失":
                Status = true;
                break;
        }
        return Status;
    }

    private bool IsReplyDept(string str)
    {
        bool Status = false;
        switch (str)
        {
            case "CAD":
                Status = true;
                break;
            case "EE":
                Status = true;
                break;
            case "ME":
                Status = true;
                break;
        }
        return Status;
    }

    private bool IsWriteDept(string str)
    {
        bool Status = false;
        switch (str)
        {
            case "AI_RI":
                Status = true;
                break;
            case "AUTO":
                Status = true;
                break;
            case "DQE":
                Status = true;
                break;
            case "EE":
                Status = true;
                break;
            case "IE":
                Status = true;
                break;
            case "SMT":
                Status = true;
                break;
            case "SQ":
                Status = true;
                break;
            case "TE":
                Status = true;
                break;
            case "UQ":
                Status = true;
                break;
        }
        return Status;
    }

    #endregion

    #region [公共方法]
    protected void BindgrdInfo(object sender, DirectEventArgs e)
    {
        string Type = cmbDFXType.SelectedItem.Text;
        string ProductType = cmbProduct.SelectedItem.Text;
        string Dept = cobDept.SelectedItem.Text;
        StringBuilder sb = new StringBuilder();
        sb.Append(@"SELECT T1.[ID]
                  ,T1.[BU]
                  ,T1.[Building]
                  ,T1.[ItemID]
                  ,T1.[Item]
                  ,T1.[ItemType]
                  ,T1.[ItemName]
                  ,T1.[Requirements]
                  ,T1.[ProductType]
                  ,T1.[PriorityLevel]
                  ,T1.[Losses]
                  ,T1.[WriteDept]
                  ,T1.[ReplyDept]
                  ,T1.[UpdateUser]
                  ,T1.[UpdateTime]
                  ,T1.[OldItemType]
                  ,T2.Item,T2.[FILE_NAME],T2.FILE_PATH
                  FROM [NPI_REPORT].[dbo].[TB_DFX_Item] T1
                  left join TB_DFX_Picture T2 ON substring(T1.Item,1,9) = T2.Item
                  where T1.Building = @Building");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@ProductType", DbType.String, ProductType));
        opc.Add(DataPara.CreateDataParameter("@Building", DbType.String, lblBuilding.Text));
        if (cobDept.Text.Length > 0)
        {
            sb.Append(" and T1.WriteDept = @WriteDept");
            opc.Add(DataPara.CreateDataParameter("@WriteDept", DbType.String, Dept));
        }
        if (Type.Length > 0)
        {
            sb.Append(" and SUBSTRING(T1.Item,11,1) = @Item");
            opc.Add(DataPara.CreateDataParameter("@Item", DbType.String, Type));
        }
        DataTable dt = sdb.TransactionExecute(sb.ToString(), opc);
        grdInfo.Store.Primary.DataSource = dt;
        grdInfo.Store.Primary.DataBind();
    }

    protected void BindgrdInfoO(object sender, DirectEventArgs e)
    {
        string Type = cmbDFXType.SelectedItem.Text;
        string ProductType = cmbProduct.SelectedItem.Text;
        string Dept = cobDept.SelectedItem.Text;
        StringBuilder sb = new StringBuilder();
        sb.Append(@"SELECT T1.[ID]
                  ,T1.[BU]
                  ,T1.[Building]
                  ,T1.[ItemID]
                  ,T1.[Item]
                  ,T1.[ItemType]
                  ,T1.[ItemName]
                  ,T1.[Requirements]
                  ,T1.[ProductType]
                  ,T1.[PriorityLevel]
                  ,T1.[Losses]
                  ,T1.[WriteDept]
                  ,T1.[ReplyDept]
                  ,T1.[UpdateUser]
                  ,T1.[UpdateTime]
                  ,T1.[OldItemType]
                  ,T2.Item,T2.[FILE_NAME],T2.FILE_PATH
                  FROM [NPI_REPORT].[dbo].[TB_DFX_Item] T1
                  left join TB_DFX_Picture T2 ON substring(T1.Item,1,9) = T2.Item
                  where T1.Building = @Building");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@ProductType", DbType.String, ProductType));
        opc.Add(DataPara.CreateDataParameter("@Building", DbType.String, lblBuilding.Text));
        if (cobDept.Text.Length > 0)
        {
            sb.Append(" and T1.WriteDept = @WriteDept");
            opc.Add(DataPara.CreateDataParameter("@WriteDept", DbType.String, Dept));
        }
        if (Type.Length > 0)
        {
            sb.Append(" and SUBSTRING(T1.Item,11,1) = @Item");
            opc.Add(DataPara.CreateDataParameter("@Item", DbType.String, Type));
        }

        DataTable dt = sdb.TransactionExecute(sb.ToString(), opc);
        grdInfo.Store.Primary.DataSource = dt;
        grdInfo.Store.Primary.DataBind();
    }

    protected void BindgrdInfoT(object sender, DirectEventArgs e)
    {
        string Type = cmbDFXType.SelectedItem.Text;
        string ProductType = cmbProduct.SelectedItem.Text;
        string Dept = cobDept.SelectedItem.Text;
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select * from TB_DFX_Item where SUBSTRING(Item,11,1) = @Item");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@Item", DbType.String, Type));
        if (cobDept.Text.Length > 0)
        {
            sb.Append(" and WriteDept = @WriteDept");
            opc.Add(DataPara.CreateDataParameter("@WriteDept", DbType.String, Dept));
        }
        if (cmbProduct.Text.Length > 0)
        {
            sb.Append(" and ProductType = @ProductType");
            opc.Add(DataPara.CreateDataParameter("@ProductType", DbType.String, ProductType));
        }
        DataTable dt = sdb.TransactionExecute(sb.ToString(), opc);
        grdInfo.Store.Primary.DataSource = dt;
        grdInfo.Store.Primary.DataBind();
    }

    private DataTable ReadByExcelLibrary(Stream xlsStream)
    {
        DataTable table = new DataTable();
        using (ExcelPackage package = new ExcelPackage(xlsStream))
        {
            ExcelWorksheet sheet = package.Workbook.Worksheets[1];
            int colCount = sheet.Dimension.End.Column;
            int rowCount = sheet.Dimension.End.Row;
            for (ushort j = 1; j <= colCount; j++)
            {
                table.Columns.Add(new DataColumn(sheet.Cells[1, j].Value.ToString()));
            }
            for (ushort i = 2; i <= rowCount; i++)
            {
                DataRow row = table.NewRow();
                for (ushort j = 1; j <= colCount; j++)
                {
                    row[j - 1] = sheet.Cells[i, j].Value;
                }
                if (!string.IsNullOrEmpty(row[0].ToString()))
                {
                    table.Rows.Add(row);
                }
            }
        }
        return table;
    }

    private void BindData(ComboBox combobox, DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            combobox.Store.Primary.DataSource = dt;
        }
        else
        {
            combobox.Store.Primary.DataSource = String.Empty;
        }

        combobox.DataBind();
    }

    private void BindData(GridPanel grd, DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            grd.Store.Primary.DataSource = dt;
        }
        else
        {
            grd.Store.Primary.DataSource = String.Empty;
        }

        grd.DataBind();

    }

    private void Alert(string msg)
    {
        if (msg.Length > 0)
        {
            X.Msg.Alert("提示", msg).Show();
        }

    }

    private void ShowMsg(string msg)
    {
        if (msg.Length > 0)
        {
            X.Msg.Notify("操作提示", msg).Show();
        }
    }

    private DataTable CheckItem(string Item)
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select  Item from TB_DFX_Item where Item =@Item");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@Item", DbType.String, Item));
        return sdb.TransactionExecute(sb.ToString(), opc);
    }

    private void DeleteExsit()
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        StringBuilder sb = new StringBuilder();
        sb.Append(@"delete  from TB_DFX_Item where Building = @Building");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@Building", DbType.String, lblBuilding.Text));
        sdb.TransactionExecuteScalar(sb.ToString(), opc);
    }

    private void BindDFX()
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select * from dbo.TB_DFX_Item where Building = @Building");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@Building", DbType.String, lblBuilding.Text));
        DataTable dt = sdb.TransactionExecute(sb.ToString(), opc);
        grdInfo.Store.Primary.DataSource = dt;
        grdInfo.Store.Primary.DataBind();
    }

    private void BindDept()
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));

        string sql = "SELECT * FROM TB_DFX_PARAM WHERE FUNCTION_NAME=@FUNCTION_NAME"
                + " AND  PARAME_NAME=@PARAME_NAME AND PARAME_VALUE4 LIKE '%W%' AND PARAME_VALUE3 LIKE '%" + lblLogonId.Text.Trim() + "%' "
                + " AND PARAME_ITEM=@BU AND Building=@Building"
                + "  ORDER BY PARAME_VALUE1";
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@FUNCTION_NAME", DbType.String, "TeamMember"));
        opc.Add(DataPara.CreateDataParameter("@PARAME_NAME", DbType.String, "Dept"));
        opc.Add(DataPara.CreateDataParameter("@BU", DbType.String, lblBu.Text));
        opc.Add(DataPara.CreateDataParameter("@Building", DbType.String, lblBuilding.Text));
        DataTable dt = sdb.TransactionExecute(sql, opc);
        BindData(cobDept, dt);

    }

    private DataTable GetVersionLog()
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        string sql = "SELECT * FROM TB_DFX_VersionLog WHERE Building=@Building";
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@Building", DbType.String, lblBuilding.Text));
        return sdb.TransactionExecute(sql, opc);
    }

    

    private DataTable GetVersion()
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select Top 1 Item  from TB_DFX_Item where Building =@Building");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@Building", DbType.String, lblBuilding.Text));
        return sdb.TransactionExecute(sb.ToString(), opc);
    }

    private void InsertVersionLog(string DFXTypeB, string DFXTypeE, string Reason)
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
                    INSERT INTO [NPI_REPORT].[dbo].[TB_DFX_VersionLog]
                               (Building
                               ,[DFXTypeB]
                               ,[DFXTypeE]
                               ,[Reason]
                               ,[UpdateTime]
                               ,[UpdateUser])
                         VALUES
                               ( @Building
                                ,@DFXTypeB
                                ,@DFXTypeE
                                ,@Reason
                                ,@UpdateTime
                                ,@UpdateUser)
                ");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@Building", DbType.String, lblBuilding.Text));
        opc.Add(DataPara.CreateDataParameter("@DFXTypeB", DbType.String, DFXTypeB));
        opc.Add(DataPara.CreateDataParameter("@DFXTypeE", DbType.String, DFXTypeE));
        opc.Add(DataPara.CreateDataParameter("@Reason", DbType.String, Reason));
        opc.Add(DataPara.CreateDataParameter("@UpdateTime", DbType.DateTime, DateTime.Now));
        opc.Add(DataPara.CreateDataParameter("@UpdateUser", DbType.String, lblLogonId.Text));
        sdb.TransactionExecuteScalar(sb.ToString(), opc);
    }

    #region  刪除路徑下已經存在的文件
    private void DeleteExistFiles(string sub_docNoPath)
    {
        DirectoryInfo directory = new DirectoryInfo(sub_docNoPath);
        FileInfo[] files = directory.GetFiles();
        foreach (FileInfo file in files)
        {
            file.Delete();
        }
    }
    #endregion

    #region Bind TB_DFX_Item
    protected void BindDFX(string ProductType)
    {
        string Type = cmbDFXType.SelectedItem.Text;
        string Dept = cobDept.SelectedItem.Text;
        StringBuilder sb = new StringBuilder();
        sb.Append(@"SELECT T1.[ID]
                  ,T1.[BU]
                  ,T1.[Building]
                  ,T1.[ItemID]
                  ,T1.[Item]
                  ,T1.[ItemType]
                  ,T1.[ItemName]
                  ,T1.[Requirements]
                  ,T1.[ProductType]
                  ,T1.[PriorityLevel]
                  ,T1.[Losses]
                  ,T1.[WriteDept]
                  ,T1.[ReplyDept]
                  ,T1.[UpdateUser]
                  ,T1.[UpdateTime]
                  ,T1.[OldItemType]
                  ,T2.Item,T2.[FILE_NAME],T2.FILE_PATH
                  FROM [NPI_REPORT].[dbo].[TB_DFX_Item] T1
                  left join TB_DFX_Picture T2 ON substring(T1.Item,1,9) = T2.Item
                  where T1.Building = @Building");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@ProductType", DbType.String, ProductType));
        opc.Add(DataPara.CreateDataParameter("@Building", DbType.String, lblBuilding.Text));
        if (cobDept.Text.Length > 0)
        {
            sb.Append(" and T1.WriteDept = @WriteDept");
            opc.Add(DataPara.CreateDataParameter("@WriteDept", DbType.String, Dept));
        }
        if (Type.Length > 0)
        {
            sb.Append(" and SUBSTRING(T1.Item,11,1) = @Item");
            opc.Add(DataPara.CreateDataParameter("@Item", DbType.String, Type));
        }
        DataTable dt = sdb.TransactionExecute(sb.ToString(), opc);
        grdInfo.Store.Primary.DataSource = dt;
        grdInfo.Store.Primary.DataBind();
    }
    #endregion

    #region 檢查當前Item是否已存在圖片的記錄
    private DataTable CheckItem2(string Item)
    {
        SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select * from TB_DFX_Picture where Item = @Item");
        opc.Clear();
        opc.Add(DataPara.CreateDataParameter("@Item", DbType.String, Item));
        return sdb.TransactionExecute(sb.ToString(), opc);
    }
    #endregion
    #endregion


}
