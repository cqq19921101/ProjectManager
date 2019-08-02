using Lib_Portal_Domain.SharedLibs;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Lib_SQM_Domain.Model
{
   public class SQM_RegisterMgmt
    {
        protected string _ERPVND;
        protected string _CORPVND;
        protected string _ERPVName;
        protected string _CorpVName;
        protected string _OfficalName;
        protected string _TelPhone;
        protected string _Fax;
        protected string _Address;
        public string ERPVND { get { return this._ERPVND; } set { this._ERPVND = value; } }
        public string CORPVND { get { return this._CORPVND; } set { this._CORPVND = value; } }
        public string ERPVName { get { return this._ERPVName; } set { this._ERPVName = value; } }
        public string CorpVName { get { return this._CorpVName; } set { this._CorpVName = value; } }
        public string OfficalName { get { return this._OfficalName; } set { this._OfficalName = value; } }
        public string TelPhone { get { return this._TelPhone; } set { this._TelPhone = value; } }
        public string Fax { get { return this._Fax; } set { this._Fax = value; } }
        public string Address { get { return this._Address; } set { this._Address = value; } }
        public SQM_RegisterMgmt() { }
        public SQM_RegisterMgmt(string ERPVND, string CORPVND, string ERPVName, string CorpVName
            , string OfficalName, string TelPhone, string Fax, string Address)
        {
            this._ERPVND = ERPVND;
            this._CORPVND = CORPVND;
            this._ERPVName = ERPVName;
            this._CorpVName = CorpVName;
            this._OfficalName = OfficalName;
            this._TelPhone = TelPhone;
            this._Fax = Fax;
            this._Address = Address;
        }
    }
    public class SQM_RegisterMgmt_jQGridJSon
    {
        public List<SQM_RegisterMgmt> Rows = new List<SQM_RegisterMgmt>();
        public string Total
        {
            get { return Convert.ToInt32(Math.Ceiling((double)(Rows.Count) / 5)).ToString(); }
        }
        public string Page { get { return "1"; } }
        public string Records { get { return Rows.Count.ToString(); } }
    }

    public static class SQM_RegisterMgmt_Helper
    {
        public static string GetDataToJQGridJson(SqlConnection cn)
        {
            return GetDataToJQGridJson(cn, "");
        }

        public static string GetDataToJQGridJson(SqlConnection cn, string SearchText)
        {
            SQM_RegisterMgmt_jQGridJSon m = new SQM_RegisterMgmt_jQGridJSon();
            m.Rows.Clear();
            int iRowCount = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT top 100 [ERP_VND]
      ,[CORP_VND]
      ,[ERP_VNAME]
      ,[CORP_VNAME]
      ,[OFFICIAL_NAME]
      ,[TELPHONE]
      ,[FAX]
      ,[ADDRESS]
      ,[COSTOM_CODE]
      ,[COSTOM_NAME]
      ,[CONTRACT_TYPE]
      ,[CREATE_USER]
      ,[CREATE_FROM]
      ,[CREATE_TIME]
      ,[MODIFY_USER]
      ,[MODIFY_FROM]
      ,[MODIFY_TIME]
      ,[DEL_FLAG]
      ,[BIZ_MODIFY_TIME]
      ,[DECLARATION_TYPE]
      ,[LFURL]
  FROM [TB_VMI_VENDOR_DETAIL]
        ");
            if (!string.IsNullOrEmpty(SearchText))
            {
                sb.Append(" where [TB_VMI_VENDOR_DETAIL].[ERP_VND] LIKE '%' + @ERP_VND + '%' ");
            }

            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.Add(new SqlParameter("@ERP_VND", StringHelper.NullOrEmptyStringIsDBNull(SearchText)));

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    iRowCount++;
                    m.Rows.Add(new SQM_RegisterMgmt(
                         dr["ERP_VND"].ToString(),
                         dr["CORP_VND"].ToString(),
                         dr["ERP_VNAME"].ToString(),
                         dr["CORP_VNAME"].ToString(),
                         dr["OFFICIAL_NAME"].ToString(),
                         dr["TELPHONE"].ToString(),
                         dr["FAX"].ToString(),
                         dr["ADDRESS"].ToString()
                  ));
                }
                dr.Close();
                dr = null;
            }

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(m);

        }
    }
}
