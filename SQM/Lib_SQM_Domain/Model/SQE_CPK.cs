using Lib_SQM_Domain.SharedLibs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace Lib_SQM_Domain.Model
{
    public class SQE_CPK
    {
        private string _spc_id;
        private string _LitNo;
        private string _spc_item;
     
        private string _check_6u;
        private string _check_6d;
        private string _usl;
        private string _lsl;
        private string _sl;
        private string _ucl;
        private string _lcl;
        private string _cpk;
        private string _check_9m;
        private string _sample;
        private string _datum;
        public string spc_id { get { return this._spc_id; } set { this._spc_id = value; } }
        public string LitNo { get { return this._LitNo; } set { this._LitNo = value; } }
        public string spc_item { get { return this._spc_item; } set { this._spc_item = value; } }
      
        public string check_6u { get { return this._check_6u; } set { this._check_6u = value; } }
        public string check_6d { get { return this._check_6d; } set { this._check_6d = value; } }
        public string usl { get { return this._usl; } set { this._usl = value; } }
        public string lsl { get { return this._lsl; } set { this._lsl = value; } }
        public string sl { get { return this._sl; } set { this._sl = value; } }
        public string ucl { get { return this._ucl; } set { this._ucl = value; } }
        public string lcl { get { return this._lcl; } set { this._lcl = value; } }
        public string cpk { get { return this._cpk; } set { this._cpk = value; } }
        public string check_9m { get { return this._check_9m; } set { this._check_9m = value; } }
        public string sample { get { return this._sample; } set { this._sample = value; } }
        public string datum { get { return this._datum; } set { this._datum = value; } }
        public SQE_CPK() { }
        public SQE_CPK(
            string spc_id,
            string LitNo,
            string spc_item,
           
            string check_6u,
            string check_6d,
            string usl,
            string lsl,
            string sl,
            string ucl,
            string lcl,
            string cpk,
            string check_9m,
            string sample,
            string datum
            )
        {
            this._spc_id = spc_id;
            this._LitNo = LitNo;
            this._spc_item = spc_item;
           
            this._check_6u = check_6u;
            this._check_6d = check_6d;
            this._usl = usl;
            this._lsl = lsl;
            this._sl = sl;
            this._ucl = ucl;
            this._lcl = lcl;
            this._cpk = cpk;
            this._check_9m = check_9m;
            this._sample = sample;
            this._datum = datum;
        }
    }
    public class SQE_CPK_jQGridJSon
    {
        public List<SQE_CPK> Rows = new List<SQE_CPK>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }
    public static class SQE_CPK_Helper
    {
        public static string GetDataToJQGridJson(SqlConnection cn, String MemberGUID)
        {
            return GetDataToJQGridJson(cn, "", MemberGUID);
        }
        public static string GetDataToJQGridJson(SqlConnection cn, string SearchText, String MemberGUID)
        {
            string sSearchText = SearchText.Trim();
            string sWhereClause = "";
            if (sSearchText != "")
                sWhereClause = "   spc_item like '%' + @SearchText + '%' ";
            if (sWhereClause.Length != 0)
                sWhereClause = "  AND " + sWhereClause;
            string Plant = GetPlant(cn, MemberGUID);
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
SELECT [spc_id],
       [LitNo],
       [spc_item],
       [spc_desc],
       [check_6u],
       [check_6d],
       [usl],
       [lsl],
       [sl],
       [ucl],
       [lcl],
       [update_time],
       [cpk],
       [check_9m],
       [sample],
       [datum]
FROM [TB_SQM_SFC_DATA]
WHERE LitNo IN( 
                SELECT [LitNo]
                FROM [TB_SQM_SFC_MAP]
                WHERE [VoderCode] in (
                         SELECT VendorCode
                         FROM TB_SQM_Member_Vendor_Map
                         WHERE PlantCode = @PlantCode ))
  AND 1 = 1
            ");
            DataTable dt = new DataTable();
            sb.Append(sWhereClause + ";");
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                if (sSearchText != "")
                    cmd.Parameters.Add(new SqlParameter("@SearchText", sSearchText));
                cmd.Parameters.Add(new SqlParameter("@PlantCode", Plant));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return JsonConvert.SerializeObject(dt);
        }

        private static string GetPlant(SqlConnection cn, string MemberGUID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT top 1 [PlantCode]   
  FROM[dbo].[TB_SQM_Member_Plant]
  where MemberGUID = @MemberGUID
 union all
SELECT top 1 [PlantCode] from
  [dbo].[TB_SQM_Member_Vendor_Map]
  where MemberGUID = @MemberGUID");
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.AddWithValue("@MemberGUID", SQMStringHelper.NullOrEmptyStringIsDBNull(MemberGUID));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "";
            }


        }
        private static void UnescapeDataFromWeb(SQE_CPK DataItem)
        {
            DataItem.spc_item = SQMStringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.spc_item);
        }
        private static string DataCheck(SQE_CPK DataItem)
        {
            string r = "";
            List<string> e = new List<string>();
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.LitNo))
                e.Add("Must provide LitNo.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.spc_item))
                e.Add("Must provide spc_item.");
           
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.check_6u))
                e.Add("Must provide check_6u.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.check_6d))
                e.Add("Must provide check_6d.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.usl))
                e.Add("Must provide usl.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.lsl))
                e.Add("Must provide lsl.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.sl))
                e.Add("Must provide sl.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.ucl))
                e.Add("Must provide ucl.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.lcl))
                e.Add("Must provide lcl.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.cpk))
                e.Add("Must provide cpk.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.check_9m))
                e.Add("Must provide check_9m.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.sample))
                e.Add("Must provide sample.");
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.datum))
                e.Add("Must provide datum.");

            for (int iCnt = 0; iCnt < e.Count; iCnt++)
            {
                if (iCnt > 0) r += "<br />";
                r += e[iCnt];
            }

            return r;
        }
        #region Create data item
        public static string CreateDataItem(SqlConnection cnPortal, SQE_CPK DataItem, string MemberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            r = CheckData(cnPortal,DataItem);
            if (r != "")
            { return r; }
            else
            {
                string sSQL = "INSERT INTO [dbo].[TB_SQM_SFC_DATA] ([spc_id],[LitNo],[spc_item],[check_6u],[check_6d],[usl],[lsl],[sl],[ucl],[lcl],[update_time],[cpk],[check_9m],[sample],[datum])";
                sSQL += "Values (@spc_id,@LitNo,@spc_item,@check_6u,@check_6d,@usl,@lsl,@sl,@ucl,@lcl,GetDate(),@cpk,@check_9m,@sample,@datum); ";
                SqlCommand cmd = new SqlCommand(sSQL, cnPortal);
                cmd.Parameters.AddWithValue("@spc_id", getNewID());
                cmd.Parameters.AddWithValue("@LitNo", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.LitNo));
                cmd.Parameters.AddWithValue("@spc_item", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.spc_item));
              
                cmd.Parameters.AddWithValue("@check_6u", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.check_6u));
                cmd.Parameters.AddWithValue("@check_6d", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.check_6d));
                cmd.Parameters.AddWithValue("@usl", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.usl));
                cmd.Parameters.AddWithValue("@lsl", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.lsl));
                cmd.Parameters.AddWithValue("@sl", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.sl));
                cmd.Parameters.AddWithValue("@ucl", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ucl));
                cmd.Parameters.AddWithValue("@lcl", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.lcl));
                cmd.Parameters.AddWithValue("@cpk", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.cpk));
                cmd.Parameters.AddWithValue("@check_9m", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.check_9m));
                cmd.Parameters.AddWithValue("@sample", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.sample));
                cmd.Parameters.AddWithValue("@datum", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.datum));
                string sErrMsg = "";
                try { cmd.ExecuteNonQuery(); }
                catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                cmd = null;
                if (string.IsNullOrEmpty(sErrMsg))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(@"
                    DECLARE  @PlantCode NVARCHAR(MAX)
                    select top 1 @PlantCode=PlantCode
                     FROM TB_SQM_Member_Plant
                     WHERE MemberGUID = @MemberGUID;
                    DECLARE  @VendorCode NVARCHAR(MAX)
                     SELECT top 1 @VendorCode=VendorCode
                         FROM TB_SQM_Member_Vendor_Map
                         WHERE PlantCode=@PlantCode;
                     DECLARE  @Count NVARCHAR(MAX)
                     SELECT @Count=Count(*)
                         FROM TB_SQM_SFC_MAP
                         WHERE LitNo = @LitNo
                          and VoderCode=@VendorCode;
if   @Count<1   
INSERT INTO TB_SQM_SFC_MAP
 ([SID]
           ,[LitNo]
           ,[VoderCode]
         )
     VALUES
           (@SID, 
          @LitNo, 
          @VendorCode
          )
");
                    cmd = new SqlCommand(sb.ToString(), cnPortal);
                    cmd.Parameters.AddWithValue("@SID", Guid.NewGuid());
                    cmd.Parameters.AddWithValue("@LitNo", DataItem.LitNo);
                    cmd.Parameters.AddWithValue("@MemberGUID", MemberGUID);
                    try { cmd.ExecuteNonQuery(); }
                    catch (Exception e) { sErrMsg = "Create fail.<br />Exception: " + e.ToString(); }
                    cmd = null;
                }
                return sErrMsg;
            }
        }

        private static string CheckData(SqlConnection cnPortal, SQE_CPK DataItem)
        {
              StringBuilder sb = new StringBuilder();
            sb.Append(@" 
                     SELECT spc_id
                         FROM TB_SQM_SFC_DATA
                         WHERE LitNo = @LitNo
                          and spc_item=@spc_item; ");

            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cnPortal))
            {
                cmd.Parameters.AddWithValue("@LitNo", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.LitNo));
                cmd.Parameters.AddWithValue("@spc_item", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.spc_item));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return dt.Rows.Count > 0 ?"該測試項目已存在":"" ;
           
        }

        private static string  getNewID()
        {
            return (DateTime.Now.ToFileTime().ToString()).Substring(4); ;
        }
        #endregion

        #region Edit data item
        public static string EditDataItem(SqlConnection cnPortal, SQE_CPK DataItem)
        {
            return EditDataItem(cnPortal, DataItem, "", "");
        }

        public static string EditDataItem(SqlConnection cnPortal, SQE_CPK DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            UnescapeDataFromWeb(DataItem);
            string r = DataCheck(DataItem);
            if (r != "")
            { return r; }
            else
            {
                SqlTransaction tran = cnPortal.BeginTransaction();

                //Update member data
                using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = EditDataItemSub(cmd, DataItem); }
               
                //Commit
                try { tran.Commit(); }
                catch (Exception e) { tran.Rollback(); r = "Edit fail.<br />Exception: " + e.ToString(); }
                return r;
            }
        }

        private static string EditDataItemSub(SqlCommand cmd, SQE_CPK DataItem)
        {
            string sErrMsg = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
  UPDATE [dbo].[TB_SQM_SFC_DATA]
   SET
       [LitNo] = @LitNo
      ,[spc_item] = @spc_item
     
      ,[check_6u] = @check_6u
      ,[check_6d] = @check_6d
      ,[usl] = @usl
      ,[lsl] = @lsl
      ,[sl] = @sl
      ,[ucl] = @ucl
      ,[lcl] = @lcl

      ,[cpk] = @cpk
      ,[check_9m] = @check_9m
      ,[sample] = @sample
      ,[datum] = @datum
 WHERE[spc_id] = @spc_id ");
           
            cmd.CommandText = sb.ToString();
            cmd.Parameters.AddWithValue("@LitNo", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.LitNo));
            cmd.Parameters.AddWithValue("@spc_item", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.spc_item));
        
            cmd.Parameters.AddWithValue("@check_6u", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.check_6u));
            cmd.Parameters.AddWithValue("@check_6d", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.check_6d));
            cmd.Parameters.AddWithValue("@usl", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.usl));
            cmd.Parameters.AddWithValue("@lsl", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.lsl));
            cmd.Parameters.AddWithValue("@sl", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.sl));
            cmd.Parameters.AddWithValue("@ucl", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.ucl));
            cmd.Parameters.AddWithValue("@lcl", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.lcl));
            cmd.Parameters.AddWithValue("@cpk", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.cpk));
            cmd.Parameters.AddWithValue("@check_9m", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.check_9m));
            cmd.Parameters.AddWithValue("@sample", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.sample));
            cmd.Parameters.AddWithValue("@datum", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.datum));
            cmd.Parameters.AddWithValue("@spc_id", SQMStringHelper.NullOrEmptyStringIsDBNull(DataItem.spc_id));
            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Edit fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion

        #region Delete data item
        public static string DeleteDataItem(SqlConnection cnPortal, SQE_CPK DataItem)
        {
            return DeleteDataItem(cnPortal, DataItem, "", "");
        }

        public static string DeleteDataItem(SqlConnection cnPortal, SQE_CPK DataItem, string LoginMemberGUID, string RunAsMemberGUID)
        {
            string r = "";
            if (SQMStringHelper.DataIsNullOrEmpty(DataItem.spc_id))
                return "Must provide  spc_id.";
            else
            {
                SqlTransaction tran = cnPortal.BeginTransaction();

                //Delete member data
                using (SqlCommand cmd = new SqlCommand("", cnPortal, tran)) { r = DeleteDataItemSub(cmd, DataItem); }
                if (r != "") { tran.Rollback(); return r; }

        
                //Commit
                try { tran.Commit(); }
                catch (Exception e) { tran.Rollback(); r = "Delete fail.<br />Exception: " + e.ToString(); }
                return r;
            }
        }

        private static string DeleteDataItemSub(SqlCommand cmd, SQE_CPK DataItem)
        {
            string sErrMsg = "";

            string sSQL = "Delete TB_SQM_SFC_DATA Where spc_id = @spc_id;";
            cmd.CommandText = sSQL;
            cmd.Parameters.AddWithValue("@spc_id", DataItem.spc_id);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception e) { sErrMsg = "Delete fail.<br />Exception: " + e.ToString(); }

            return sErrMsg;
        }
        #endregion
    }
}
