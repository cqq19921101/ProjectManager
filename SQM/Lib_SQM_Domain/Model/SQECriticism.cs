using Aspose.Cells;
using Lib_Portal_Domain.SharedLibs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.Script.Serialization;

namespace Lib_SQM_Domain.Model
{
    #region SQECriticism
    public class SQECriticism
    {

        protected string _CriticismID;
        protected string _DateCriticism;
        protected string _CriticismCategory;
        protected string _CriticismUnit;
        protected string _ItemNO;
        protected string _CriticismItem;
        protected string _ItemPartition;
        protected string _FitnessScore;
        protected string _ActualScore;
        protected string _ScoringRatio;
        protected string _Evaluation;

        public string CriticismID { get { return this._CriticismID; } set { this._CriticismID = value; } }
        public string DateCriticism { get { return this._DateCriticism; } set { this._DateCriticism = value; } }
        public string CriticismCategory { get { return this._CriticismCategory; } set { this._CriticismCategory = value; } }
        public string CriticismUnit { get { return this._CriticismUnit; } set { this._CriticismUnit = value; } }
        public string ItemNO { get { return this._ItemNO; } set { this._ItemNO = value; } }
        public string CriticismItem { get { return this._CriticismItem; } set { this._CriticismItem = value; } }
        public string ItemPartition { get { return this._ItemPartition; } set { this._ItemPartition = value; } }
        public string FitnessScore { get { return this._FitnessScore; } set { this._FitnessScore = value; } }
        public string ActualScore { get { return this._ActualScore; } set { this._ActualScore = value; } }
        public string ScoringRatio { get { return this._ScoringRatio; } set { this._ScoringRatio = value; } }
        public string Evaluation { get { return this._Evaluation; } set { this._Evaluation = value; } }
        public SQECriticism() { }
        public SQECriticism(string CriticismID, string DateCriticism, string CriticismCategory, string CriticismUnit, string ItemNO,
         string CriticismItem, string ItemPartition, string FitnessScore, string ActualScore, string ScoringRatio, string Evaluation)
        {

            this._CriticismID = CriticismID;
            this._DateCriticism = DateCriticism;
            this._CriticismCategory = CriticismCategory;
            this._CriticismUnit = CriticismUnit;
            this._ItemNO = ItemNO;
            this._CriticismItem = CriticismItem;
            this._ItemPartition = ItemPartition;
            this._FitnessScore = FitnessScore;
            this._ActualScore = ActualScore;
            this._ScoringRatio = ScoringRatio;
            this._Evaluation = Evaluation;
        }
    }
    public class SQECriticism_jQGridJSon
    {
        public List<SQECriticism> Rows = new List<SQECriticism>();

        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }
    public static class SQECriticism_Helper
    {
        #region Create/Edit data check
        private static void UnescapeDataFromWeb(SQECriticism DataItem)
        {
            DataItem.CriticismID = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CriticismID);
            DataItem.CriticismCategory = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CriticismCategory);
            DataItem.CriticismUnit = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CriticismUnit);
            DataItem.ItemNO = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ItemNO);
            DataItem.CriticismItem = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CriticismItem);
            DataItem.ItemPartition = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ItemPartition);
            DataItem.FitnessScore = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.FitnessScore);
            DataItem.ActualScore = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ActualScore);
            DataItem.ScoringRatio = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.ScoringRatio);
            DataItem.Evaluation = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.Evaluation);

        }
        private static string DataCheck(SQECriticism DataItem)
        {
            string r = "";
            List<string> e = new List<string>();

            if (StringHelper.DataIsNullOrEmpty(DataItem.ItemPartition))
                e.Add("Must provide ItemPartition.");
            if (StringHelper.DataIsNullOrEmpty(DataItem.FitnessScore))
                e.Add("Must provide FitnessScore.");
            if (StringHelper.DataIsNullOrEmpty(DataItem.ActualScore))
                e.Add("Must provide ActualScore.");
            if (StringHelper.DataIsNullOrEmpty(DataItem.ScoringRatio))
                e.Add("Must provide ScoringRatio.");
            if (StringHelper.DataIsNullOrEmpty(DataItem.Evaluation))
                e.Add("Must provide Evaluation.");
            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }
        #endregion
        public static string DeleteDataItem(SqlConnection cnPortal, SQECriticism DataItem)
        {
            return DeleteDataItem(cnPortal, DataItem, "", "");
        }
        public static string DeleteDataItem(SqlConnection cnPortal, SQECriticism DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            string r = "";
            if (StringHelper.DataIsNullOrEmpty(DataItem.CriticismID))
                return "Must provide CriticismID.";
            else
            {
                using (SqlCommand cmd = new SqlCommand("", cnPortal)) { r = DeleteDataItemSub(cmd, DataItem); }
                if (r != "") { return r; }
                //SqlTransaction tran = cnPortal.BeginTransaction();

                ////Delete member data
                //using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = DeleteDataItemSub(cmd, DataItem); }
                //if (r != "") { tran.Rollback(); return r; }

                ////Release lock
                //if ((LoginMemberGUID != "") && (RunAsMemberGUID != ""))
                //    using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = DataLockHelper.ReleaseLock(cmd, DataItem.SID, LoginMemberGUID, RunAsMemberGUID); }
                //if (r != "") { tran.Rollback(); return r; }

                ////Commit
                //try { tran.Commit(); }
                //catch (Exception e) { tran.Rollback(); r = "Delete fail.<br />Exception: " + e.ToString(); }
                return r;
            }
        }
        public static String CreatExcel(String filename, String filenameNet, SqlConnection sqlConnection, string CriticismID, String localPath, String RunAsUserGUID, String urlPre)
        {
            String r = string.Empty;
            CreatCriticismExcel(filename, sqlConnection, CriticismID,
                localPath, urlPre);
            return r;


        }

        public static void CreatCriticismExcel(String filename, SqlConnection sqlConnection, String BasicInfoGUID, String localPath, String urlPre)
        {
            License lic = new License();
            //String AsposeLicPath = "E:\\TW VMI V2\\Portal_Web\\Source\\Aspose.Cells\\Aspose.Cells.lic";
            //lic.SetLicense(AsposeLicPath);
            lic.SetLicense(localPath + @"Source\Aspose.Cells\Aspose.Cells.lic");
            String templatePath = localPath + @"Source\Template\Criticism.xlsx";
            Workbook book = new Workbook(templatePath);
            Worksheet sheet0 = book.Worksheets[0];

            BindReport(ref sheet0, sqlConnection, BasicInfoGUID, urlPre);
            book.CalculateFormula();
            book.Save(filename, SaveFormat.Xlsx);

        }
        public static void BindReport(ref Worksheet sheet0, SqlConnection cn, String CriticismID, String urlPre)
        {
            Cells cells = sheet0.Cells;
            Workbook book1 = new Workbook();
            Style style1 = book1.Styles[book1.Styles.Add()];
            style1.HorizontalAlignment = TextAlignmentType.Center;//文字居中
            style1.VerticalAlignment = TextAlignmentType.Center;
            style1.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin; //应用边界线 左边界线  
            style1.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin; //应用边界线 右边界线  
            style1.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin; //应用边界线 上边界线  
            style1.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin; //应用边界线 下边界线
            StringBuilder sb = new StringBuilder();
            DataTable dt = new DataTable();
            #region
            sb.Append(@"SELECT [FactoryName],[FactoryAddress],[CNAME],PHONE,QSADateCriticism,QPADateCriticism,
( CASE WHEN( A.TB_SQM_CommodityCID='A' AND A.QSAScore>=0.85) THEN 'P'
 WHEN( A.TB_SQM_CommodityCID='A' AND A.QSAScore>=0.75 AND A.QSAScore<0.85) THEN 'A'
 WHEN( A.TB_SQM_CommodityCID='A' AND A.QSAScore>=0.65 AND A.QSAScore<0.75) THEN 'C'
 WHEN( A.TB_SQM_CommodityCID='A' AND A.QSAScore<0.65) THEN 'U'
 WHEN( A.TB_SQM_CommodityCID='B' AND A.QSAScore>=0.90) THEN 'P'
 WHEN( A.TB_SQM_CommodityCID='B' AND A.QSAScore>=0.80 AND A.QSAScore<0.90) THEN 'A'
 WHEN( A.TB_SQM_CommodityCID='B' AND A.QSAScore>=0.70 AND A.QSAScore<0.80) THEN 'C'
 WHEN( A.TB_SQM_CommodityCID='B' AND A.QSAScore<0.70) THEN 'U'
 WHEN( A.TB_SQM_CommodityCID='C' AND A.QSAScore>=0.90) THEN 'P'
 WHEN( A.TB_SQM_CommodityCID='C' AND A.QSAScore>=0.80 AND A.QSAScore<0.90) THEN 'A'
 WHEN( A.TB_SQM_CommodityCID='C' AND A.QSAScore>=0.70 AND A.QSAScore<0.80) THEN 'C'
 WHEN( A.TB_SQM_CommodityCID='C' AND A.QSAScore<0.70) THEN 'U'
 END) AS QSAScore,(
 CASE WHEN( A.TB_SQM_CommodityCID='A' AND A.QPAScore>=0.85) THEN 'P'
 WHEN( A.TB_SQM_CommodityCID='A' AND A.QPAScore>=0.75 AND A.QPAScore<0.85) THEN 'A'
 WHEN( A.TB_SQM_CommodityCID='A' AND A.QPAScore>=0.65 AND A.QPAScore<0.75) THEN 'C'
 WHEN( A.TB_SQM_CommodityCID='A' AND A.QPAScore<0.65) THEN 'U'
 WHEN( A.TB_SQM_CommodityCID='B' AND A.QPAScore>=0.90) THEN 'P'
 WHEN( A.TB_SQM_CommodityCID='B' AND A.QPAScore>=0.80 AND A.QPAScore<0.90) THEN 'A'
 WHEN( A.TB_SQM_CommodityCID='B' AND A.QPAScore>=0.70 AND A.QPAScore<0.80) THEN 'C'
 WHEN( A.TB_SQM_CommodityCID='B' AND A.QPAScore<0.70) THEN 'U'
 WHEN( A.TB_SQM_CommodityCID='C' AND A.QPAScore>=0.90) THEN 'P'
 WHEN( A.TB_SQM_CommodityCID='C' AND A.QPAScore>=0.80 AND A.QPAScore<0.90) THEN 'A'
 WHEN( A.TB_SQM_CommodityCID='C' AND A.QPAScore>=0.70 AND A.QPAScore<0.80) THEN 'C'
 WHEN( A.TB_SQM_CommodityCID='C' AND A.QPAScore<0.70) THEN 'U'
 END) AS QPAScore,QSAEvaluation,QPAEvaluation
FROM
(SELECT [FactoryName],[FactoryAddress],[TB_SQM_Commodity_Sub].[CNAME],[TB_SQM_Commodity_Sub].TB_SQM_CommodityCID,
(
SELECT TOP(1) Phone FROM [dbo].[TB_SQM_Contact]
INNER JOIN [dbo].[TB_SQM_Manufacturers_BasicInfo] ON [TB_SQM_Contact].[VendorCode]=[TB_SQM_Manufacturers_BasicInfo].[VendorCode]
) AS PHONE,
(
SELECT TOP(1) DateCriticism FROM [dbo].[TB_SQM_Criticism]
INNER JOIN [dbo].[TB_SQM_Criticism_InfoMap] ON [TB_SQM_Criticism_InfoMap].[CriticismID]=[TB_SQM_Criticism].[CriticismID]
INNER JOIN [TB_SQM_Manufacturers_BasicInfo] ON [TB_SQM_Manufacturers_BasicInfo].BasicInfoGUID=[TB_SQM_Criticism_InfoMap].BasicInfoGUID
WHERE [TB_SQM_Criticism].[CriticismCategory]='QSA'
) AS QSADateCriticism,
(
SELECT TOP(1) DateCriticism FROM [dbo].[TB_SQM_Criticism]
INNER JOIN [dbo].[TB_SQM_Criticism_InfoMap] ON [TB_SQM_Criticism_InfoMap].[CriticismID]=[TB_SQM_Criticism].[CriticismID]
INNER JOIN [TB_SQM_Manufacturers_BasicInfo] ON [TB_SQM_Manufacturers_BasicInfo].BasicInfoGUID=[TB_SQM_Criticism_InfoMap].BasicInfoGUID
WHERE [TB_SQM_Criticism].[CriticismCategory]='QPA'
) AS QPADateCriticism,
(
SELECT    (sum([FitnessScore])/sum([ActualScore])) FROM [dbo].[TB_SQM_Criticism]
INNER JOIN [dbo].[TB_SQM_Criticism_InfoMap] ON [TB_SQM_Criticism_InfoMap].[CriticismID]=[TB_SQM_Criticism].[CriticismID]
INNER JOIN [TB_SQM_Manufacturers_BasicInfo] ON [TB_SQM_Manufacturers_BasicInfo].BasicInfoGUID=[TB_SQM_Criticism_InfoMap].BasicInfoGUID
WHERE [TB_SQM_Criticism].[CriticismCategory]='QSA'
) AS QSAScore,
(
SELECT (sum([FitnessScore])/sum([ActualScore])) FROM [dbo].[TB_SQM_Criticism]
INNER JOIN [dbo].[TB_SQM_Criticism_InfoMap] ON [TB_SQM_Criticism_InfoMap].[CriticismID]=[TB_SQM_Criticism].[CriticismID]
INNER JOIN [TB_SQM_Manufacturers_BasicInfo] ON [TB_SQM_Manufacturers_BasicInfo].BasicInfoGUID=[TB_SQM_Criticism_InfoMap].BasicInfoGUID
WHERE [TB_SQM_Criticism].[CriticismCategory]='QPA'
) AS QPAScore,
(
SELECT TOP(1) [Evaluation] FROM [dbo].[TB_SQM_Criticism]
INNER JOIN [dbo].[TB_SQM_Criticism_InfoMap] ON [TB_SQM_Criticism_InfoMap].[CriticismID]=[TB_SQM_Criticism].[CriticismID]
INNER JOIN [TB_SQM_Manufacturers_BasicInfo] ON [TB_SQM_Manufacturers_BasicInfo].BasicInfoGUID=[TB_SQM_Criticism_InfoMap].BasicInfoGUID
WHERE [TB_SQM_Criticism].[CriticismCategory]='QSA'
) AS QSAEvaluation,
(
SELECT TOP(1) [Evaluation] FROM [dbo].[TB_SQM_Criticism]
INNER JOIN [dbo].[TB_SQM_Criticism_InfoMap] ON [TB_SQM_Criticism_InfoMap].[CriticismID]=[TB_SQM_Criticism].[CriticismID]
INNER JOIN [TB_SQM_Manufacturers_BasicInfo] ON [TB_SQM_Manufacturers_BasicInfo].BasicInfoGUID=[TB_SQM_Criticism_InfoMap].BasicInfoGUID
WHERE [TB_SQM_Criticism].[CriticismCategory]='QPA'
) AS QPAEvaluation
FROM [TB_SQM_Manufacturers_BasicInfo]
INNER JOIN [TB_SQM_Commodity_Sub] ON [TB_SQM_Manufacturers_BasicInfo].[TB_SQM_Commodity_SubCID]=[TB_SQM_Commodity_Sub].[CID]
  AND     [TB_SQM_Manufacturers_BasicInfo].[TB_SQM_Commodity_SubTB_SQM_CommodityCID]=[TB_SQM_Commodity_Sub].[TB_SQM_CommodityCID]
  inner join [dbo].[TB_SQM_Criticism_InfoMap] on [TB_SQM_Criticism_InfoMap].BasicInfoGUID=[TB_SQM_Manufacturers_BasicInfo].BasicInfoGUID
WHERE [TB_SQM_Criticism_InfoMap].CriticismID=@CriticismID
 ) AS A 
");

            using (SqlCommand cmd = new SqlCommand("", cn))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.Add(new SqlParameter("@CriticismID", StringHelper.NullOrEmptyStringIsDBNull(CriticismID.Trim())));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                string C6 = dr["FactoryName"] == DBNull.Value ? "" : dr["FactoryName"].ToString();
                cells[5, 2].PutValue(C6);
                string I6 = dr["FactoryAddress"] == DBNull.Value ? "" : dr["FactoryAddress"].ToString();
                cells[5, 8].PutValue(I6);
                string C7 = dr["CNAME"] == DBNull.Value ? "" : dr["CNAME"].ToString();
                cells[6, 2].PutValue(C7);
                string I7 = dr["Phone"] == DBNull.Value ? "" : dr["Phone"].ToString();
                cells[6, 8].PutValue(I7);
                string D8 = dr["QSADateCriticism"] == DBNull.Value ? "" : dr["QSADateCriticism"].ToString();
                cells[7, 3].PutValue(D8);
                string F8 = dr["QPADateCriticism"] == DBNull.Value ? "" : dr["QPADateCriticism"].ToString();
                cells[7, 5].PutValue(F8);
                string J8 = dr["QSAScore"] == DBNull.Value ? "" : dr["QSAScore"].ToString();
                cells[7, 9].PutValue(J8);
                string L8 = dr["QPAScore"] == DBNull.Value ? "" : dr["QPAScore"].ToString();
                cells[7, 11].PutValue(L8);
                string D9 = dr["QSAEvaluation"] == DBNull.Value ? "" : dr["QSAEvaluation"].ToString();
                cells[8, 3].PutValue(D9);
                string H9 = dr["QPAEvaluation"] == DBNull.Value ? "" : dr["QPAEvaluation"].ToString();
                cells[8, 7].PutValue(H9);
            }

            #endregion
            #region
            sb = new StringBuilder();
            dt = new DataTable();
            sb.Append(@"
  select [CriticismCategory]
      ,[CriticismUnit]
      ,[ItemNO]
      ,[CriticismItem]
      ,[ItemPartition]
      ,[FitnessScore]
      ,[ActualScore]
      ,[ScoringRatio]
      ,[Evaluation]
	    FROM [TB_SQM_Criticism]
		  where [CriticismCategory]='QSA' and [CriticismID]=@CriticismID 
");
            using (SqlCommand cmd = new SqlCommand("", cn))
            {
                cmd.CommandText = sb.ToString();
                cmd.Parameters.Add(new SqlParameter("@CriticismID", StringHelper.NullOrEmptyStringIsDBNull(CriticismID)));

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                try
                {
                    DataRow dr = dt.Rows[i];
                    string ItemNO = dr["ItemNO"] == DBNull.Value ? "" : dr["ItemNO"].ToString();
                    if (ItemNO.Equals("A"))
                    {
                        double G18 = dr["ItemPartition"] == DBNull.Value ? 0 : double.Parse(dr["ItemPartition"].ToString());
                        cells[17, 6].PutValue(G18);
                        double H18 = dr["FitnessScore"] == DBNull.Value ? 0 : double.Parse(dr["FitnessScore"].ToString());
                        cells[17, 7].PutValue(H18);
                        double I18 = dr["ActualScore"] == DBNull.Value ? 0 : double.Parse(dr["ActualScore"].ToString());
                        cells[17, 8].PutValue(I18);

                        string K19 = dr["Evaluation"] == DBNull.Value ? "" : dr["AnnualCapacity"].ToString();
                        cells[18, 10].PutValue(K19);
                    }

                    if (ItemNO.Equals("B"))
                    {
                        double G18 = dr["ItemPartition"] == DBNull.Value ? 0 : double.Parse(dr["ItemPartition"].ToString());
                        cells[18, 6].PutValue(G18);
                        double H18 = dr["FitnessScore"] == DBNull.Value ? 0 : double.Parse(dr["FitnessScore"].ToString());
                        cells[18, 7].PutValue(H18);
                        double I18 = dr["ActualScore"] == DBNull.Value ? 0 : double.Parse(dr["ActualScore"].ToString());
                        cells[18, 8].PutValue(I18);

                        string K19 = dr["Evaluation"] == DBNull.Value ? "" : dr["AnnualCapacity"].ToString();
                        cells[18, 10].PutValue(K19);
                    }
                    if (ItemNO.Equals("C"))
                    {
                        double G18 = dr["ItemPartition"] == DBNull.Value ? 0 : double.Parse(dr["ItemPartition"].ToString());
                        cells[19, 6].PutValue(G18);
                        double H18 = dr["FitnessScore"] == DBNull.Value ? 0 : double.Parse(dr["FitnessScore"].ToString());
                        cells[19, 7].PutValue(H18);
                        double I18 = dr["ActualScore"] == DBNull.Value ? 0 : double.Parse(dr["ActualScore"].ToString());
                        cells[19, 8].PutValue(I18);

                        string K19 = dr["Evaluation"] == DBNull.Value ? "" : dr["AnnualCapacity"].ToString();
                        cells[18, 10].PutValue(K19);
                    }
                    if (ItemNO.Equals("D"))
                    {
                        double G18 = dr["ItemPartition"] == DBNull.Value ? 0 : double.Parse(dr["ItemPartition"].ToString());
                        cells[20, 6].PutValue(G18);
                        double H18 = dr["FitnessScore"] == DBNull.Value ? 0 : double.Parse(dr["FitnessScore"].ToString());
                        cells[20, 7].PutValue(H18);
                        double I18 = dr["ActualScore"] == DBNull.Value ? 0 : double.Parse(dr["ActualScore"].ToString());
                        cells[20, 8].PutValue(I18);

                        string K19 = dr["Evaluation"] == DBNull.Value ? "" : dr["AnnualCapacity"].ToString();
                        cells[20, 10].PutValue(K19);
                    }
                    if (ItemNO.Equals("E"))
                    {
                        double G18 = dr["ItemPartition"] == DBNull.Value ? 0 : double.Parse(dr["ItemPartition"].ToString());
                        cells[21, 6].PutValue(G18);
                        double H18 = dr["FitnessScore"] == DBNull.Value ? 0 : double.Parse(dr["FitnessScore"].ToString());
                        cells[21, 7].PutValue(H18);
                        double I18 = dr["ActualScore"] == DBNull.Value ? 0 : double.Parse(dr["ActualScore"].ToString());
                        cells[21, 8].PutValue(I18);

                        string K19 = dr["Evaluation"] == DBNull.Value ? "" : dr["AnnualCapacity"].ToString();
                        cells[18, 10].PutValue(K19);
                    }
                    if (ItemNO.Equals("F"))
                    {
                        double G18 = dr["ItemPartition"] == DBNull.Value ? 0 : double.Parse(dr["ItemPartition"].ToString());
                        cells[21, 6].PutValue(G18);
                        double H18 = dr["FitnessScore"] == DBNull.Value ? 0 : double.Parse(dr["FitnessScore"].ToString());
                        cells[21, 7].PutValue(H18);
                        double I18 = dr["ActualScore"] == DBNull.Value ? 0 : double.Parse(dr["ActualScore"].ToString());
                        cells[21, 8].PutValue(I18);

                        string K19 = dr["Evaluation"] == DBNull.Value ? "" : dr["AnnualCapacity"].ToString();
                        cells[18, 10].PutValue(K19);
                    }
                    if (ItemNO.Equals("G"))
                    {
                        double G18 = dr["ItemPartition"] == DBNull.Value ? 0 : double.Parse(dr["ItemPartition"].ToString());
                        cells[22, 6].PutValue(G18);
                        double H18 = dr["FitnessScore"] == DBNull.Value ? 0 : double.Parse(dr["FitnessScore"].ToString());
                        cells[22, 7].PutValue(H18);
                        double I18 = dr["ActualScore"] == DBNull.Value ? 0 : double.Parse(dr["ActualScore"].ToString());
                        cells[22, 8].PutValue(I18);

                        string K19 = dr["Evaluation"] == DBNull.Value ? "" : dr["AnnualCapacity"].ToString();
                        cells[18, 10].PutValue(K19);
                    }
                    if (ItemNO.Equals("H"))
                    {
                        double G18 = dr["ItemPartition"] == DBNull.Value ? 0 : double.Parse(dr["ItemPartition"].ToString());
                        cells[25, 6].PutValue(G18);
                        double H18 = dr["FitnessScore"] == DBNull.Value ? 0 : double.Parse(dr["FitnessScore"].ToString());
                        cells[25, 7].PutValue(H18);
                        double I18 = dr["ActualScore"] == DBNull.Value ? 0 : double.Parse(dr["ActualScore"].ToString());
                        cells[25, 8].PutValue(I18);

                        string K19 = dr["Evaluation"] == DBNull.Value ? "" : dr["AnnualCapacity"].ToString();
                        cells[25, 11].PutValue(K19);
                    }
                    if (ItemNO.Equals("J"))
                    {
                        double G18 = dr["ItemPartition"] == DBNull.Value ? 0 : double.Parse(dr["ItemPartition"].ToString());
                        cells[26, 6].PutValue(G18);
                        double H18 = dr["FitnessScore"] == DBNull.Value ? 0 : double.Parse(dr["FitnessScore"].ToString());
                        cells[26, 7].PutValue(H18);
                        double I18 = dr["ActualScore"] == DBNull.Value ? 0 : double.Parse(dr["ActualScore"].ToString());
                        cells[26, 8].PutValue(I18);

                        string K19 = dr["Evaluation"] == DBNull.Value ? "" : dr["AnnualCapacity"].ToString();
                        cells[27, 10].PutValue(K19);
                    }
                    if (ItemNO.Equals("K"))
                    {
                        double G18 = dr["ItemPartition"] == DBNull.Value ? 0 : double.Parse(dr["ItemPartition"].ToString());
                        cells[27, 6].PutValue(G18);
                        double H18 = dr["FitnessScore"] == DBNull.Value ? 0 : double.Parse(dr["FitnessScore"].ToString());
                        cells[27, 7].PutValue(H18);
                        double I18 = dr["ActualScore"] == DBNull.Value ? 0 : double.Parse(dr["ActualScore"].ToString());
                        cells[27, 8].PutValue(I18);

                        string K19 = dr["Evaluation"] == DBNull.Value ? "" : dr["AnnualCapacity"].ToString();
                        cells[27, 10].PutValue(K19);
                    }
                    if (ItemNO.Equals("L"))
                    {
                        double G18 = dr["ItemPartition"] == DBNull.Value ? 0 : double.Parse(dr["ItemPartition"].ToString());
                        cells[28, 6].PutValue(G18);
                        double H18 = dr["FitnessScore"] == DBNull.Value ? 0 : double.Parse(dr["FitnessScore"].ToString());
                        cells[28, 7].PutValue(H18);
                        double I18 = dr["ActualScore"] == DBNull.Value ? 0 : double.Parse(dr["ActualScore"].ToString());
                        cells[28, 8].PutValue(I18);

                        string K19 = dr["Evaluation"] == DBNull.Value ? "" : dr["AnnualCapacity"].ToString();
                        cells[27, 10].PutValue(K19);
                    }
                    if (ItemNO.Equals("M"))
                    {
                        double G18 = dr["ItemPartition"] == DBNull.Value ? 0 : double.Parse(dr["ItemPartition"].ToString());
                        cells[29, 6].PutValue(G18);
                        double H18 = dr["FitnessScore"] == DBNull.Value ? 0 : double.Parse(dr["FitnessScore"].ToString());
                        cells[29, 7].PutValue(H18);
                        double I18 = dr["ActualScore"] == DBNull.Value ? 0 : double.Parse(dr["ActualScore"].ToString());
                        cells[29, 8].PutValue(I18);

                        string K19 = dr["Evaluation"] == DBNull.Value ? "" : dr["AnnualCapacity"].ToString();
                        cells[27, 10].PutValue(K19);
                    }
                    if (ItemNO.Equals("N"))
                    {
                        double G18 = dr["ItemPartition"] == DBNull.Value ? 0 : double.Parse(dr["ItemPartition"].ToString());
                        cells[30, 6].PutValue(G18);
                        double H18 = dr["FitnessScore"] == DBNull.Value ? 0 : double.Parse(dr["FitnessScore"].ToString());
                        cells[30, 7].PutValue(H18);
                        double I18 = dr["ActualScore"] == DBNull.Value ? 0 : double.Parse(dr["ActualScore"].ToString());
                        cells[30, 8].PutValue(I18);

                        string K19 = dr["Evaluation"] == DBNull.Value ? "" : dr["AnnualCapacity"].ToString();
                        cells[27, 10].PutValue(K19);
                    }
                    if (ItemNO.Equals("P"))
                    {
                        double G18 = dr["ItemPartition"] == DBNull.Value ? 0 : double.Parse(dr["ItemPartition"].ToString());
                        cells[31, 6].PutValue(G18);
                        double H18 = dr["FitnessScore"] == DBNull.Value ? 0 : double.Parse(dr["FitnessScore"].ToString());
                        cells[31, 7].PutValue(H18);
                        double I18 = dr["ActualScore"] == DBNull.Value ? 0 : double.Parse(dr["ActualScore"].ToString());
                        cells[31, 8].PutValue(I18);

                        string K19 = dr["Evaluation"] == DBNull.Value ? "" : dr["AnnualCapacity"].ToString();
                        cells[27, 10].PutValue(K19);
                    }
                    if (ItemNO.Equals("Q"))
                    {
                        double G18 = dr["ItemPartition"] == DBNull.Value ? 0 : double.Parse(dr["ItemPartition"].ToString());
                        cells[32, 6].PutValue(G18);
                        double H18 = dr["FitnessScore"] == DBNull.Value ? 0 : double.Parse(dr["FitnessScore"].ToString());
                        cells[32, 7].PutValue(H18);
                        double I18 = dr["ActualScore"] == DBNull.Value ? 0 : double.Parse(dr["ActualScore"].ToString());
                        cells[32, 8].PutValue(I18);

                        string K19 = dr["Evaluation"] == DBNull.Value ? "" : dr["AnnualCapacity"].ToString();
                        cells[32, 11].PutValue(K19);
                    }
                }
                catch (Exception ex)
                {

                }
            }
            #endregion

        }


        private static string DeleteDataItemSub(SqlCommand cmd, SQECriticism DataItem)
        {
            string sErrMsg = "";

            string sSQL = "Delete TB_SQM_Criticism Where CriticismID = @CriticismID;";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@CriticismID", StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CriticismID));

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        public static string GetDataToJQGridJson(SqlConnection cn, string CriticismID)
        {
            SQECriticism_jQGridJSon m = new SQECriticism_jQGridJSon();
            string sSearchText = CriticismID.Trim();





            m.Rows.Clear();
            int iRowCount = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
  select [CriticismCategory]
      ,[CriticismUnit]
      ,[ItemNO]
      ,[CriticismItem]
      ,[ItemPartition]
      ,[FitnessScore]
      ,[ActualScore]
      ,[ScoringRatio]
      ,[Evaluation]
	    FROM [TB_SQM_Criticism]
		  where [CriticismCategory]='QSA' and [CriticismID]=@CriticismID

	  union
	   select top(1) 'QSA' as [CriticismCategory],'' as [CriticismUnit],'' as [ItemNO],'合計' as [ItemPartition],
	   (select 
	  sum([ItemPartition]) from [TB_SQM_Criticism] where [CriticismCategory]='QSA' and [CriticismID]=@CriticismID )as [ItemPartition],
	( select   sum([FitnessScore]) from [TB_SQM_Criticism] where [CriticismCategory]='QSA' and [CriticismID]=@CriticismID) as [FitnessScore],
	 ( select sum([ActualScore]) from [TB_SQM_Criticism] where [CriticismCategory]='QSA' and [CriticismID]=@CriticismID) as [ActualScore],
 (select	(sum([FitnessScore])/sum([ActualScore])) from [TB_SQM_Criticism] where [CriticismCategory]='QSA'  and [CriticismID]=@CriticismID)as [ScoringRatio]
  ,(select top(1) [Evaluation]  from [TB_SQM_Criticism] where [CriticismCategory]='QSA'  and [CriticismID]=@CriticismID) as  [Evaluation] from [TB_SQM_Criticism]

   union 

 select [CriticismCategory]
      ,[CriticismUnit]
      ,[ItemNO]
      ,[CriticismItem]
      ,[ItemPartition]
      ,[FitnessScore]
      ,[ActualScore]
      ,[ScoringRatio]
      ,[Evaluation]
	    FROM [TB_SQM_Criticism]
		  where [CriticismCategory]='QPA' and [CriticismID]=@CriticismID

	  union
	   select top(1) 'QPA' as [CriticismCategory],'' as [CriticismUnit],'' as [ItemNO],'合計' as [ItemPartition],
	   (select 
	  sum([ItemPartition]) from [TB_SQM_Criticism] where [CriticismCategory]='QPA'  and [CriticismID]=@CriticismID)as [ItemPartition],
	( select   sum([FitnessScore]) from [TB_SQM_Criticism] where [CriticismCategory]='QPA' and [CriticismID]=@CriticismID) as [FitnessScore],
	 ( select sum([ActualScore]) from [TB_SQM_Criticism] where [CriticismCategory]='QPA' and [CriticismID]=@CriticismID) as [ActualScore],
 (select	(sum([FitnessScore])/sum([ActualScore])) from [TB_SQM_Criticism] where [CriticismCategory]='QPA' and [CriticismID]=@CriticismID )as [ScoringRatio]
  ,(select top(1) [Evaluation] from [TB_SQM_Criticism] where [CriticismCategory]='QPA' and [CriticismID]=@CriticismID ) as [Evaluation] from   [TB_SQM_Criticism]
  ORDER BY [CriticismCategory] ,[ItemNO]
");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@CriticismID", StringHelper.NullOrEmptyStringIsDBNull(sSearchText)));

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }



        public static string GetDataToJQGridJson(SqlConnection cn)
        {
            return GetDataToJQGridJson(cn, "");
        }
        public static string CreateDataItem(SqlConnection cnPortal, SQECriticism DataItem)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);

            if (r != "")
            { return r; }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("INSERT INTO TB_SQM_Criticism ");
                sb.Append("([CriticismID],[DateCriticism],[CriticismCategory],[CriticismUnit],[ItemNO],[CriticismItem],[ItemPartition],[FitnessScore],[ActualScore],[ScoringRatio],[Evaluation]) ");
                sb.Append("Values ( @CriticismID,@DateCriticism,@CriticismCategory,@CriticismUnit,@ItemNO,@CriticismItem,@ItemPartition,@FitnessScore,@ActualScore,@ScoringRatio, @Evaluation);");
                SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal);



                cmd.Parameters.AddWithValue("@CriticismID", StringHelper.NullOrEmptyStringIsDBNull(DataItem.CriticismID));
                cmd.Parameters.AddWithValue("@DateCriticism", DateTime.Now.ToShortDateString());
                cmd.Parameters.AddWithValue("@CriticismCategory", StringHelper.NullOrEmptyStringIsDBNull(DataItem.CriticismCategory));
                cmd.Parameters.AddWithValue("@CriticismUnit", StringHelper.NullOrEmptyStringIsDBNull(DataItem.CriticismUnit));
                cmd.Parameters.AddWithValue("@ItemNO", StringHelper.NullOrEmptyStringIsDBNull(DataItem.ItemNO));
                cmd.Parameters.AddWithValue("@CriticismItem", StringHelper.NullOrEmptyStringIsDBNull(DataItem.CriticismItem));
                cmd.Parameters.AddWithValue("@ItemPartition", StringHelper.NullOrEmptyStringIsDBNull(DataItem.ItemPartition));
                cmd.Parameters.AddWithValue("@FitnessScore", StringHelper.NullOrEmptyStringIsDBNull(DataItem.FitnessScore));
                cmd.Parameters.AddWithValue("@ActualScore", StringHelper.NullOrEmptyStringIsDBNull(DataItem.ActualScore));
                cmd.Parameters.AddWithValue("@ScoringRatio", StringHelper.NullOrEmptyStringIsDBNull(DataItem.ScoringRatio));
                cmd.Parameters.AddWithValue("@Evaluation", StringHelper.NullOrEmptyStringIsDBNull(DataItem.Evaluation));

                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;

                return sErrMsg;
            }
        }

        public static string EditDataItem(SqlConnection cnPortal, SQECriticism DataItem)
        {
            return EditDataItem(cnPortal, DataItem, "", "");
        }
        public static string EditDataItem(SqlConnection cnPortal, SQECriticism DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            if (r != "")
            { return r; }
            else
            {
                using (SqlCommand cmd = new SqlCommand("", cnPortal)) { r = EditDataItemSub(cmd, DataItem); }
                if (r != "") { return r; }
                return r;
            }
        }

        private static string EditDataItemSub(SqlCommand cmd, SQECriticism DataItem)
        {
            string sErrMsg = "";
            //StringBuilder sb = new StringBuilder();
            //sb.Append("Update TB_SQM_Criticism Set  ItemPartition=@ItemPartition,");
            //sb.Append("FitnessScore=@FitnessScore,ActualScore=@ActualScore,ScoringRatio=@ScoringRatio,Evaluation=@Evaluation");
            //sb.Append(" Where SID = @SID");

            //cmd.CommandText = sb.ToString();
            //cmd.Parameters.AddWithValue("@ItemPartition", StringHelper.NullOrEmptyStringIsDBNull(DataItem.ItemPartition));
            //cmd.Parameters.AddWithValue("@FitnessScore", StringHelper.NullOrEmptyStringIsDBNull(DataItem.FitnessScore));
            //cmd.Parameters.AddWithValue("@ActualScore", StringHelper.NullOrEmptyStringIsDBNull(DataItem.ActualScore));
            //cmd.Parameters.AddWithValue("@ScoringRatio", StringHelper.NullOrEmptyStringIsDBNull(DataItem.ScoringRatio));
            //cmd.Parameters.AddWithValue("@Evaluation", StringHelper.NullOrEmptyStringIsDBNull(DataItem.Evaluation));
            //cmd.Parameters.AddWithValue("@SID", StringHelper.NullOrEmptyStringIsDBNull(DataItem.SID));
            //try { cmd.ExecuteNonQuery(); }
            //catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }

        public static string GetCriticismType(SqlConnection cn, string criticismCategory, string criticismUnit, string criticismItem)
        {
            string sWhereClause = "";
            if (string.IsNullOrEmpty(criticismCategory))
            {
                sWhereClause += " and PARAME_VALUE1 is null";
            }
            else
            {
                sWhereClause += " and PARAME_VALUE1=@PARAME_VALUE1";
            }
            if (string.IsNullOrEmpty(criticismUnit))
            {
                sWhereClause += " and PARAME_VALUE2 is null";
            }
            else
            {
                sWhereClause += " and PARAME_VALUE2=@PARAME_VALUE2";
            }
            if (string.IsNullOrEmpty(criticismItem))
            {
                sWhereClause += " and PARAME_VALUE3 is null";
            }
            else
            {
                sWhereClause += " and PARAME_VALUE3=@PARAME_VALUE3";
            }


            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT [PARAME_NAME],[PARAME_ITEM] ");
            sb.Append(" FROM [TB_SQM_APPLICATION_PARAM] ");
            sb.Append(" WHERE  [APPLICATION_NAME]='SQM' and [FUNCTION_NAME]='Criticism'");
            sb.Append(sWhereClause);
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                if (!string.IsNullOrEmpty(criticismCategory))
                {
                    cmd.Parameters.Add(new SqlParameter("@PARAME_VALUE1", StringHelper.NullOrEmptyStringIsDBNull(criticismCategory)));
                }
                if (!string.IsNullOrEmpty(criticismUnit))
                {
                    cmd.Parameters.Add(new SqlParameter("@PARAME_VALUE2", StringHelper.NullOrEmptyStringIsDBNull(criticismUnit)));
                }
                if (!string.IsNullOrEmpty(criticismItem))
                {
                    cmd.Parameters.Add(new SqlParameter("@PARAME_VALUE3", StringHelper.NullOrEmptyStringIsDBNull(criticismItem)));
                }
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }
    }
    #endregion
    #region SQECriticismMap
    public class SQECriticismMap
    {
        protected string _CriticismID;
        protected string _CriticismName;
        public string CriticismID { get { return this._CriticismID; } set { this._CriticismID = value; } }
        public string CriticismName { get { return this._CriticismName; } set { this._CriticismName = value; } }
        public SQECriticismMap()
        {

        }
        public SQECriticismMap(string CriticismID, string CriticismName)
        {
            this._CriticismID = CriticismID;
            this._CriticismName = CriticismName;
        }
    }
    public class SQECriticismMap_jQGridJSon
    {
        public List<SQECriticismMap> Rows = new List<SQECriticismMap>();

        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }
    public static class SQECriticismMap_Helper
    {

        #region Create/Edit data check
        private static void UnescapeDataFromWeb(SQECriticismMap DataItem)
        {
            DataItem.CriticismID = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CriticismID);
            DataItem.CriticismName = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CriticismName);


        }
        private static string DataCheck(SQECriticismMap DataItem)
        {
            string r = "";
            List<string> e = new List<string>();

            //if (StringHelper.DataIsNullOrEmpty(DataItem.VendorCode))
            //    e.Add("Must provide VendorCode.");

            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }
        #endregion

        public static string GetDataToJQGridJson(SqlConnection cn, string SearchText,String MemberGUID)
        {
            string sSearchText = SearchText.Trim();
            string sWhereClause = "";
            if (sSearchText != "")
                sWhereClause += "CriticismName like '%' + @SearchText + '%'";

            if (sWhereClause.Length != 0)
                sWhereClause = " AND " + sWhereClause.Substring(0);

            StringBuilder sb = new StringBuilder();
            sb.Append(@"
SELECT [BasicInfoGUID]
      ,CMAP.[CriticismID]
	  ,CINFO.[CriticismName]
  FROM [TB_SQM_Criticism_InfoMap] AS CMAP
  LEFT JOIN [dbo].[TB_SQM_Criticism_Info] AS CINFO ON CMAP.CriticismID=CINFO.CriticismID
  WHERE CMAP.BasicInfoGUID=@MemberGUID
");

            DataTable dt = new DataTable();
            sb.Append(sWhereClause + ";");
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                if (sSearchText != "")
                    cmd.Parameters.Add(new SqlParameter("@SearchText", sSearchText));
                cmd.Parameters.Add(new SqlParameter("@MemberGUID", MemberGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }
        public static string GetDataToJQGridJson(SqlConnection cn,String MemberGUID)
        {
            return GetDataToJQGridJson(cn, "", MemberGUID);
        }
        public static string CreateDataItem(SqlConnection cnPortal, SQECriticismMap DataItem, String MemberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);

            if (r != "")
            { return r; }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"
INSERT INTO TB_SQM_Criticism_Info 
([CriticismID],[CriticismName],DateCriticism) 
Values ( @CriticismID,@CriticismName,@DateCriticism);

INSERT INTO [dbo].[TB_SQM_Criticism_InfoMap]([BasicInfoGUID],[CriticismID])
VALUES (@BasicInfoGUID,@CriticismID);
");
                SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal);
                string id = Guid.NewGuid().ToString();
                cmd.Parameters.AddWithValue("@CriticismID", id);
                cmd.Parameters.AddWithValue("@BasicInfoGUID", MemberGUID);
                cmd.Parameters.AddWithValue("@CriticismName", StringHelper.NullOrEmptyStringIsDBNull(DataItem.CriticismName));
                cmd.Parameters.AddWithValue("@DateCriticism", DateTime.Now);

                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;

                return sErrMsg;
            }
        }

        public static string EditDataItem(SqlConnection cnPortal, SQECriticismMap DataItem)
        {
            return EditDataItem(cnPortal, DataItem, "", "");
        }
        public static string EditDataItem(SqlConnection cnPortal, SQECriticismMap DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            if (r != "")
            { return r; }
            else
            {
                using (SqlCommand cmd = new SqlCommand("", cnPortal)) { r = EditDataItemSub(cmd, DataItem); }
                if (r != "") { return r; }
                return r;
            }
        }

        private static string EditDataItemSub(SqlCommand cmd, SQECriticismMap DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append("Update TB_SQM_Criticism_Info Set  CriticismName=@CriticismName");
            sb.Append(" Where CriticismID = @CriticismID");

            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@CriticismName", StringHelper.NullOrEmptyStringIsDBNull(DataItem.CriticismName));
            cmd.Parameters.AddWithValue("@CriticismID", StringHelper.NullOrEmptyStringIsDBNull(DataItem.CriticismID));
            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        public static string DeleteDataItem(SqlConnection cnPortal, SQECriticismMap DataItem)
        {
            return DeleteDataItem(cnPortal, DataItem, "", "");
        }
        public static string DeleteDataItem(SqlConnection cnPortal, SQECriticismMap DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            string r = "";
            if (StringHelper.DataIsNullOrEmpty(DataItem.CriticismID))
                return "Must provide CriticismID.";
            else
            {
                using (SqlCommand cmd = new SqlCommand("", cnPortal))
                {

                    r = DeleteDataItemMap(cmd, DataItem.CriticismID);

                }
                using (SqlCommand cmd = new SqlCommand("", cnPortal))
                {

                    r = DeleteCriticism(cmd, DataItem.CriticismID);
                }
                using (SqlCommand cmd = new SqlCommand("", cnPortal))
                {
                    r = DeleteDataItemSub(cmd, DataItem);

                }


                if (r != "") { return r; }

                return r;
            }
        }
        private static string DeleteDataItemSub(SqlCommand cmd, SQECriticismMap DataItem)
        {
            string sErrMsg = "";

            string sSQL = "Delete TB_SQM_Criticism_Info Where CriticismID = @CriticismID;";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@CriticismID", StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.CriticismID));
            string CriticismID = DataItem.CriticismID;
            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        private static string DeleteDataItemMap(SqlCommand cmd, string CriticismID)
        {
            string sErrMsg = "";

            string sSQL = "Delete TB_SQM_Criticism_InfoMap Where CriticismID = @CriticismID;";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@CriticismID", StringHelper.EmptyOrUnescapedStringViaUrlDecode(CriticismID));

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        private static string DeleteCriticism(SqlCommand cmd, string CriticismID)
        {
            string sErrMsg = "";

            string sSQL = "Delete TB_SQM_Criticism Where CriticismID = @CriticismID;";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@CriticismID", StringHelper.EmptyOrUnescapedStringViaUrlDecode(CriticismID));

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        public static string GetCriticismType(SqlConnection cn, string criticismCategory, string criticismUnit, string criticismItem)
        {
            string sWhereClause = "";
            if (string.IsNullOrEmpty(criticismCategory))
            {
                sWhereClause += " and PARAME_VALUE1 is null";
            }
            else
            {
                sWhereClause += " and PARAME_VALUE1=@PARAME_VALUE1";
            }
            if (string.IsNullOrEmpty(criticismUnit))
            {
                sWhereClause += " and PARAME_VALUE2 is null";
            }
            else
            {
                sWhereClause += " and PARAME_VALUE2=@PARAME_VALUE2";
            }
            if (string.IsNullOrEmpty(criticismItem))
            {
                sWhereClause += " and PARAME_VALUE3 is null";
            }
            else
            {
                sWhereClause += " and PARAME_VALUE3=@PARAME_VALUE3";
            }


            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT [PARAME_NAME],[PARAME_ITEM] ");
            sb.Append(" FROM [TB_SQM_APPLICATION_PARAM] ");
            sb.Append(" WHERE  [APPLICATION_NAME]='SQM' and [FUNCTION_NAME]='Criticism'");
            sb.Append(sWhereClause);
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                if (!string.IsNullOrEmpty(criticismCategory))
                {
                    cmd.Parameters.Add(new SqlParameter("@PARAME_VALUE1", StringHelper.NullOrEmptyStringIsDBNull(criticismCategory)));
                }
                if (!string.IsNullOrEmpty(criticismUnit))
                {
                    cmd.Parameters.Add(new SqlParameter("@PARAME_VALUE2", StringHelper.NullOrEmptyStringIsDBNull(criticismUnit)));
                }
                if (!string.IsNullOrEmpty(criticismItem))
                {
                    cmd.Parameters.Add(new SqlParameter("@PARAME_VALUE3", StringHelper.NullOrEmptyStringIsDBNull(criticismItem)));
                }
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }
        public static string CreatInfoMap(SqlConnection cnPortal, string CriticismID, string memberGUID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO TB_SQM_Criticism_InfoMap ");
            sb.Append("(CriticismID,BasicInfoGUID) ");
            sb.Append("Values (@CriticismID,@BasicInfoGUID );");
            SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal);


            cmd.Parameters.AddWithValue("@CriticismID", StringHelper.NullOrEmptyStringIsDBNull(CriticismID));
            cmd.Parameters.AddWithValue("@BasicInfoGUID", memberGUID);
            string sErrMsg = "";
            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
            cmd = null;

            return sErrMsg;

        }

    }
    #endregion
    #region SQE_ManufacturersBasicInfo
    public class SQE_ManufacturersBasicInfo
    {

        protected string _FactoryName;
        protected string _FactoryAddress;
        protected string _CNAME;
        protected string _Phone;
        protected string _QSADateCriticism;
        protected string _QPADateCriticism;
        protected string _QSAScore;
        protected string _QPAScore;
        protected string _QSAEvaluation;
        protected string _QPAEvaluation;


        public string FactoryName { get { return this._FactoryName; } set { this._FactoryName = value; } }
        public string FactoryAddress { get { return this._FactoryAddress; } set { this._FactoryAddress = value; } }
        public string CNAME { get { return this._CNAME; } set { this._CNAME = value; } }
        public string Phone { get { return this._Phone; } set { this._Phone = value; } }
        public string QSADateCriticism { get { return this._QSADateCriticism; } set { this._QSADateCriticism = value; } }
        public string QPADateCriticism { get { return this._QPADateCriticism; } set { this._QPADateCriticism = value; } }
        public string QSAScore { get { return this._QSAScore; } set { this._QSAScore = value; } }
        public string QPAScore { get { return this._QPAScore; } set { this._QPAScore = value; } }
        public string QSAEvaluation { get { return this._QSAEvaluation; } set { this._QSAEvaluation = value; } }
        public string QPAEvaluation { get { return this._QPAEvaluation; } set { this._QPAEvaluation = value; } }
        public SQE_ManufacturersBasicInfo()
        {

        }
        public SQE_ManufacturersBasicInfo(string FactoryName, string FactoryAddress, string CNAME,
            string Phone, string QSADateCriticism, string QPADateCriticism, string QSAScore, string QPAScore, string QSAEvaluation, string QPAEvaluation)
        {

            this._FactoryName = FactoryName;
            this._FactoryAddress = FactoryAddress;
            this._CNAME = CNAME;
            this._Phone = Phone;
            this._QSADateCriticism = QSADateCriticism;
            this._QPADateCriticism = QPADateCriticism;
            this._QSAScore = QSAScore;
            this._QPAScore = QPAScore;
            this._QSAEvaluation = QSAEvaluation;
            this._QPAEvaluation = QPAEvaluation;
        }
    }
    public class SQE_ManufacturersBasicInfo_jQGridJSon
    {
        public List<SQE_ManufacturersBasicInfo> Rows = new List<SQE_ManufacturersBasicInfo>();

        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }
    public static class SQE_ManufacturersBasicInfo_Helper
    {
        public static string GetDataToJQGridJson(SqlConnection cn, string CriticismID)
        {
            SQE_ManufacturersBasicInfo_jQGridJSon m = new SQE_ManufacturersBasicInfo_jQGridJSon();
            m.Rows.Clear();
            int iRowCount = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT [FactoryName],[FactoryAddress],[CNAME],PHONE,QSADateCriticism,QPADateCriticism,
( CASE WHEN( A.TB_SQM_CommodityCID='A' AND A.QSAScore>=0.85) THEN 'P'
 WHEN( A.TB_SQM_CommodityCID='A' AND A.QSAScore>=0.75 AND A.QSAScore<0.85) THEN 'A'
 WHEN( A.TB_SQM_CommodityCID='A' AND A.QSAScore>=0.65 AND A.QSAScore<0.75) THEN 'C'
 WHEN( A.TB_SQM_CommodityCID='A' AND A.QSAScore<0.65) THEN 'U'
 WHEN( A.TB_SQM_CommodityCID='B' AND A.QSAScore>=0.90) THEN 'P'
 WHEN( A.TB_SQM_CommodityCID='B' AND A.QSAScore>=0.80 AND A.QSAScore<0.90) THEN 'A'
 WHEN( A.TB_SQM_CommodityCID='B' AND A.QSAScore>=0.70 AND A.QSAScore<0.80) THEN 'C'
 WHEN( A.TB_SQM_CommodityCID='B' AND A.QSAScore<0.70) THEN 'U'
 WHEN( A.TB_SQM_CommodityCID='C' AND A.QSAScore>=0.90) THEN 'P'
 WHEN( A.TB_SQM_CommodityCID='C' AND A.QSAScore>=0.80 AND A.QSAScore<0.90) THEN 'A'
 WHEN( A.TB_SQM_CommodityCID='C' AND A.QSAScore>=0.70 AND A.QSAScore<0.80) THEN 'C'
 WHEN( A.TB_SQM_CommodityCID='C' AND A.QSAScore<0.70) THEN 'U'
 END) AS QSAScore,(
 CASE WHEN( A.TB_SQM_CommodityCID='A' AND A.QPAScore>=0.85) THEN 'P'
 WHEN( A.TB_SQM_CommodityCID='A' AND A.QPAScore>=0.75 AND A.QPAScore<0.85) THEN 'A'
 WHEN( A.TB_SQM_CommodityCID='A' AND A.QPAScore>=0.65 AND A.QPAScore<0.75) THEN 'C'
 WHEN( A.TB_SQM_CommodityCID='A' AND A.QPAScore<0.65) THEN 'U'
 WHEN( A.TB_SQM_CommodityCID='B' AND A.QPAScore>=0.90) THEN 'P'
 WHEN( A.TB_SQM_CommodityCID='B' AND A.QPAScore>=0.80 AND A.QPAScore<0.90) THEN 'A'
 WHEN( A.TB_SQM_CommodityCID='B' AND A.QPAScore>=0.70 AND A.QPAScore<0.80) THEN 'C'
 WHEN( A.TB_SQM_CommodityCID='B' AND A.QPAScore<0.70) THEN 'U'
 WHEN( A.TB_SQM_CommodityCID='C' AND A.QPAScore>=0.90) THEN 'P'
 WHEN( A.TB_SQM_CommodityCID='C' AND A.QPAScore>=0.80 AND A.QPAScore<0.90) THEN 'A'
 WHEN( A.TB_SQM_CommodityCID='C' AND A.QPAScore>=0.70 AND A.QPAScore<0.80) THEN 'C'
 WHEN( A.TB_SQM_CommodityCID='C' AND A.QPAScore<0.70) THEN 'U'
 END) AS QPAScore,QSAEvaluation,QPAEvaluation
FROM
(SELECT [PORTAL_Members].NameInChinese as [FactoryName],'' as [FactoryAddress],'' as [CNAME],'' as TB_SQM_CommodityCID,
'' AS PHONE,
(
SELECT top 1 DateCriticism FROM [dbo].[TB_SQM_Criticism]
WHERE [TB_SQM_Criticism].[CriticismCategory]='QSA' and CriticismID=@CriticismID
) AS QSADateCriticism,
(
SELECT top 1 DateCriticism FROM [dbo].[TB_SQM_Criticism]
WHERE [TB_SQM_Criticism].[CriticismCategory]='QPA' and CriticismID=@CriticismID
) AS QPADateCriticism,
(
SELECT    (sum([FitnessScore])/sum([ActualScore])) FROM [dbo].[TB_SQM_Criticism]
WHERE [TB_SQM_Criticism].[CriticismCategory]='QSA' and CriticismID=@CriticismID
) AS QSAScore,
(
SELECT (sum([FitnessScore])/sum([ActualScore])) FROM [dbo].[TB_SQM_Criticism]
WHERE [TB_SQM_Criticism].[CriticismCategory]='QPA' and CriticismID=@CriticismID
) AS QPAScore,
(
SELECT top 1 [Evaluation] FROM [dbo].[TB_SQM_Criticism]
WHERE [TB_SQM_Criticism].[CriticismCategory]='QSA' and CriticismID=@CriticismID
) AS QSAEvaluation,
(
SELECT  top 1 [Evaluation] FROM [dbo].[TB_SQM_Criticism]
WHERE [TB_SQM_Criticism].[CriticismCategory]='QPA' and CriticismID=@CriticismID
) AS QPAEvaluation
from [dbo].[PORTAL_Members]
inner join TB_SQM_Criticism_InfoMap on  TB_SQM_Criticism_InfoMap.BasicInfoGUID =[PORTAL_Members].MemberGUID
 ) AS A 
");

            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@CriticismID", StringHelper.NullOrEmptyStringIsDBNull(CriticismID.Trim())));
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    iRowCount++;
                    m.Rows.Add(new SQE_ManufacturersBasicInfo(
                        dr["FactoryName"].ToString(),
                        dr["FactoryAddress"].ToString(),
                        dr["CNAME"].ToString(),
                        dr["Phone"].ToString(),
                        dr["QSADateCriticism"].ToString(),
                        dr["QPADateCriticism"].ToString(),
                        dr["QSAScore"].ToString(),
                        dr["QPAScore"].ToString(),
                        dr["QSAEvaluation"].ToString(),
                        dr["QPAEvaluation"].ToString()
                        ));
                }
                dr.Close();
                dr = null;
            }

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(m);
        }
        public static string GetDataToJQGridJson(SqlConnection cn)
        {
            return GetDataToJQGridJson(cn, "");
        }
    }
    #endregion
}
