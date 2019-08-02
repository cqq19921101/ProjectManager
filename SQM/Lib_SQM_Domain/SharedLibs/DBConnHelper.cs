using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;

namespace Lib_SQM_Domain.SharedLibs
{
    //public enum PortalDBType { PortalDB, SPMDB, DataSourceDB, iPowerDB };
    public enum PortalDBType { PortalDB, SPMDB };
    public class DBConnInfo
    {
        private readonly string _SesssionKey;
        private readonly string _ConnectionString;

        public string SessionKey { get { return this._SesssionKey; } }
        public string ConnectionString { get { return this._ConnectionString; } }

        public DBConnInfo(string SessionKey, string ConnectionString)
        {
            this._SesssionKey = SessionKey;
            this._ConnectionString = ConnectionString;
        }
    }

    public static class DBConnHelper
    {
        private static DBConnInfo GetDBSessionKey(PortalDBType DBType)
        {
            DBConnInfo ConnInfo;
            switch (DBType)
            {
                case PortalDBType.SPMDB:
                    ConnInfo = new DBConnInfo("SPMDBConnection", "SPMDBConnString");
                    break;
                //case PortalDBType.iPowerDB:
                //    ConnInfo = new DBConnInfo("iPowerDBConnection", "iPowerDBConnString");
                //    break;
                //case PortalDBType.DataSourceDB:
                //    ConnInfo = new DBConnInfo("DataSourceDBConnection", "DataSourceDBConnString");
                //    break;
                case PortalDBType.PortalDB:
                default:
                    ConnInfo = new DBConnInfo("MVCPortalDBConnection", "MVCPortalDBConnString");
                    break;
            }
            return ConnInfo;
        }

        public static SqlConnection GetPortalDBConnectionOpen(HttpSessionStateBase Session, PortalDBType DBType)
        {
            SqlConnection DBConnection = GetPortalDBConnection(Session, DBType);
            if (DBConnection.State == ConnectionState.Closed)
                DBConnection.Open();
            return DBConnection;
        }

        public static SqlConnection GetPortalDBConnection(HttpSessionStateBase Session, PortalDBType DBType)
        {
            DBConnInfo ConnInfo = GetDBSessionKey(DBType);
            SqlConnection DBConnection = null;
            if (Session[ConnInfo.SessionKey] != null)
                DBConnection = (SqlConnection)Session[ConnInfo.SessionKey];
            else
            {
                DBConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnInfo.ConnectionString].ConnectionString);
                SessionHelper.SetSessionValue(Session, ConnInfo.SessionKey, DBConnection);
            }
            return DBConnection;
        }

        public static void ClosePortalDBConnection(HttpSessionStateBase Session, PortalDBType DBType)
        {
            DBConnInfo ConnInfo = GetDBSessionKey(DBType);
            ((SqlConnection)Session[ConnInfo.SessionKey]).Close();
        }

        public static SqlConnection GetPortalDBConnectionOpen(PortalDBType DBType)
        {
            SqlConnection DBConnection = GetPortalDBConnection(DBType);
            if (DBConnection.State == ConnectionState.Closed)
                DBConnection.Open();
            return DBConnection;
        }

        public static SqlConnection GetPortalDBConnection(PortalDBType DBType)
        {
            DBConnInfo ConnInfo = GetDBSessionKey(DBType);
            return new SqlConnection(ConfigurationManager.ConnectionStrings[ConnInfo.ConnectionString].ConnectionString);
        }
    }
}
