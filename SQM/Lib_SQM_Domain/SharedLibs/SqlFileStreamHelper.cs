using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Web.Mvc;
using System.Text;
using System.Text.RegularExpressions;

namespace Lib_SQM_Domain.SharedLibs
{
    public static class SqlFileStreamHelper
    {
        /*
            no exception handling, responsible by owner
         */

        #region InsertFile
        public static void InsertToTable(SqlConnection SqlConn, SqlTransaction TransObj,
                                        string TableName, string FileStreamFieldName, string FileNameFieldName,
                                        string KeyFieldName,
                                        string[] FullFileNames)
        {
            if (FullFileNames.Length > 0)
                foreach (string FullFileName in FullFileNames)
                    InsertToTable(SqlConn, TransObj, TableName, FileStreamFieldName, FileNameFieldName, KeyFieldName, null, null, FullFileName);
        }

        public static void InsertToTable(SqlConnection SqlConn, SqlTransaction TransObj,
                                        string TableName, string FileStreamFieldName, string FileNameFieldName,
                                        string KeyFieldName,
                                        string RefKeyFieldName, string RefKeyValue,
                                        string[] FullFileNames)
        {
            if (FullFileNames.Length > 0)
                foreach (string FullFileName in FullFileNames)
                    InsertToTable(SqlConn, TransObj, TableName, FileStreamFieldName, FileNameFieldName, KeyFieldName, RefKeyFieldName, RefKeyValue, FullFileName);
        }

        public static void InsertToTable(SqlConnection SqlConn, SqlTransaction TransObj,
                                        string TableName, string FileStreamFieldName, string FileNameFieldName,
                                        string KeyFieldName,
                                        string RefKeyFieldName, string RefKeyValue,
                                        List<string> FullFileNames)
        {
            if (FullFileNames.Count > 0)
                foreach (string FullFileName in FullFileNames)
                    InsertToTable(SqlConn, TransObj, TableName, FileStreamFieldName, FileNameFieldName, KeyFieldName, RefKeyFieldName, RefKeyValue, FullFileName);
        }

        public static void InsertToTable(SqlConnection SqlConn, SqlTransaction TransObj,
                                        string TableName, string FileStreamFieldName, string FileNameFieldName,
                                        string KeyFieldName,
                                        string FullFileName)
        {
            InsertToTable(SqlConn, TransObj, TableName, FileStreamFieldName, FileNameFieldName, KeyFieldName, null, null, FullFileName);
        }

        public static void InsertToTable(SqlConnection SqlConn, SqlTransaction TransObj,
                                        string TableName, string FileStreamFieldName, string FileNameFieldName,
                                        string KeyFieldName,
                                        string RefKeyFieldName, string RefKeyValue,
                                        string FullFileName)
        {
            using (SqlCommand cmd = new SqlCommand("", SqlConn, TransObj))
            {
                TableName = TableName.Trim();
                FileStreamFieldName = FileStreamFieldName.Trim();
                FileNameFieldName = FileNameFieldName.Trim();
                KeyFieldName = KeyFieldName.Trim();
                if (RefKeyFieldName != null) RefKeyFieldName = RefKeyFieldName.Trim();
                if (RefKeyValue != null) RefKeyValue = RefKeyValue.Trim();
                FullFileName = FullFileName.Trim();
                FileInfo fi = new FileInfo(FullFileName);

                string sSQL = "DECLARE @NewKeyValue uniqueidentifier SET @NewKeyValue = NEWID();";
                sSQL += "Insert Into " + TableName + " (" + KeyFieldName + ", " + FileStreamFieldName + ", " + FileNameFieldName;
                if (RefKeyFieldName != null) sSQL += ", " + RefKeyFieldName;
                sSQL += ") Values (@NewKeyValue, Cast('' As varbinary(Max)), @FileName";
                if (RefKeyFieldName != null) sSQL += ", @RefKeyValue";
                sSQL += ");";
                sSQL += "Select " + FileStreamFieldName.Trim() + ".PathName() As Path From " + TableName + " Where " + KeyFieldName + " = @NewKeyValue;";

                cmd.CommandText = sSQL;
                cmd.Parameters.AddWithValue("@FileName", fi.Name);
                if (RefKeyFieldName != null) cmd.Parameters.AddWithValue("@RefKeyValue", RefKeyValue);
                string sPathOfFileStreamField = (string)cmd.ExecuteScalar();
                
                cmd.CommandText = "Select GET_FILESTREAM_TRANSACTION_CONTEXT() As TransactionContext";
                byte[] transContext = (byte[])cmd.ExecuteScalar();
                SqlFileStream sqlFS = new SqlFileStream(sPathOfFileStreamField, transContext, FileAccess.ReadWrite);

                byte[] fileData = System.IO.File.ReadAllBytes(fi.FullName);
                sqlFS.Write(fileData, 0, fileData.Length);
                sqlFS.Close();
            }
        }
        public static String InsertToTableSQM(SqlConnection SqlConn, SqlTransaction TransObj, string UpdateUser, string FullFileName)
        {
            String r = "";
            return InsertToTableSQM(SqlConn, TransObj, UpdateUser, FullFileName,"");
        }
        public static String InsertToTableSQM(SqlConnection SqlConn, SqlTransaction TransObj, string UpdateUser, string FullFileName,string validDate)
        {
            String r = "";
            using (SqlCommand cmd = new SqlCommand("", SqlConn, TransObj))
            {
                try
                {
                    FullFileName = FullFileName.Trim();
                    FileInfo fi = new FileInfo(FullFileName);

                    StringBuilder sb = new StringBuilder();
                    sb.Append(@"
                    DECLARE @NewKeyValue uniqueidentifier SET @NewKeyValue = NEWID();
                    INSERT INTO [dbo].[TB_SQM_Files]
                           ([FGUID]
                           ,[FileName]
                           ,[FileContent]
                           ,[UpdateTime]
                           ,[UpdateUser]
                           ,[ValidDate]
                            )
                     VALUES
                           (@NewKeyValue
                           ,@FileName
                           ,Cast('' As varbinary(Max))
                           ,getDate()
                           ,@UpdateUser
                           ,@ValidDate
                           );
                    Select FileContent.PathName() As Path, FGUID From TB_SQM_Files WHERE FGUID=@NewKeyValue;
                    ");
                    String sql = Regex.Replace(sb.ToString(), @"\s+", " ");

                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@FileName", fi.Name);
                    cmd.Parameters.AddWithValue("@UpdateUser", UpdateUser);
                    if (validDate == "")
                    {
                        cmd.Parameters.AddWithValue("@ValidDate", (DBNull.Value));
                    }else
                    {
                        cmd.Parameters.AddWithValue("@ValidDate", validDate);
                    }
                    

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(dr);
                        if (dt.Rows.Count > 0)
                        {
                            string sPathOfFileStreamField = dt.Rows[0]["Path"].ToString();
                            string sFGUID = dt.Rows[0]["FGUID"].ToString();
                            r = sFGUID;
                            cmd.CommandText = "Select GET_FILESTREAM_TRANSACTION_CONTEXT() As TransactionContext";
                            byte[] transContext = (byte[])cmd.ExecuteScalar();
                            SqlFileStream sqlFS = new SqlFileStream(sPathOfFileStreamField, transContext, FileAccess.ReadWrite);

                            byte[] fileData = System.IO.File.ReadAllBytes(fi.FullName);
                            sqlFS.Write(fileData, 0, fileData.Length);
                            sqlFS.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    r = ex.ToString();
                }


            }
            return r;
        }
        #endregion

        #region Insert into "GeneratedFile"
        //public static void InsertGeneratedFileToTable(SqlConnection SqlConn, SqlTransaction TransObj,
        //    string FullFileName,
        //    string LoginUser, string RunAsUser, int? TypeCode)
        //{
        //    InsertGeneratedFileToTable(SqlConn, TransObj, FullFileName, LoginUser, RunAsUser, TypeCode, "");
        //}

        public static void InsertGeneratedFileToTable(SqlConnection SqlConn, SqlTransaction TransObj,
            string FullFileName,
            string LoginUser, string RunAsUser, int? TypeCode,
            string NewKeyValue)
        {
            InsertGeneratedFileToTable(SqlConn, TransObj, FullFileName, LoginUser, RunAsUser, TypeCode, "");
        }

        public static void InsertGeneratedFileToTable(SqlConnection SqlConn, SqlTransaction TransObj,
            string FullFileName, 
            string LoginUser, string RunAsUser, int? TypeCode,
            string NewKeyValue, string NewFilename)
        {
            using (SqlCommand cmd = new SqlCommand("", SqlConn, TransObj))
            {
                FileInfo fi = new FileInfo(FullFileName.Trim());
                string sSQL = "";
                
                if (NewKeyValue == "")
                    sSQL = "DECLARE @NewKeyValue uniqueidentifier SET @NewKeyValue = NEWID();";
                sSQL += "Insert Into CQM_GeneratedFiles (GeneratedDate, FS_GUID, FS_FileName, FS_Source, LoginUser, RunAsUser, TypeCode) Values "
                    + "(GetDate(), @NewKeyValue, @FileName, Cast('' As varbinary(Max)), @LoginUser, @RunAsUser, @TypeCode);"
                    + "Select FS_Source.PathName() As Path From CQM_GeneratedFiles Where FS_GUID = @NewKeyValue;";

                cmd.CommandText = sSQL;
                if(NewKeyValue!="")
                    cmd.Parameters.AddWithValue("@NewKeyValue", NewKeyValue);
                if(NewFilename.Trim()!="")
                    cmd.Parameters.AddWithValue("@FileName", NewFilename.Trim());
                else
                    cmd.Parameters.AddWithValue("@FileName", fi.Name);
                if (LoginUser == null)
                    cmd.Parameters.AddWithValue("@LoginUser", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@LoginUser", LoginUser.Trim());
                if (RunAsUser == null)
                    cmd.Parameters.AddWithValue("@RunAsUser", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@RunAsUser", RunAsUser.Trim());
                if (TypeCode == null)
                    cmd.Parameters.AddWithValue("@TypeCode", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@TypeCode", ((int)TypeCode));
                string sPathOfFileStreamField = (string)cmd.ExecuteScalar();

                cmd.CommandText = "Select GET_FILESTREAM_TRANSACTION_CONTEXT() As TransactionContext";
                byte[] transContext = (byte[])cmd.ExecuteScalar();
                SqlFileStream sqlFS = new SqlFileStream(sPathOfFileStreamField, transContext, FileAccess.ReadWrite);

                byte[] fileData = System.IO.File.ReadAllBytes(fi.FullName);
                sqlFS.Write(fileData, 0, fileData.Length);
                sqlFS.Close();
            }
        }
        #endregion

        #region OutputInStream
        public static FileInfoForOutput OutputInStream(SqlConnection SqlConn, SqlTransaction TransObj,
                                        string TableName, string FileStreamFieldName, string FileNameFieldName,
                                        string KeyFieldName, string KeyValue)
        {
            byte[] buffer = null;
            string sFileName = @"";
            
            using (SqlCommand cmd = new SqlCommand("", SqlConn, TransObj))
            {
                string sSQL = "Select " + FileStreamFieldName + ".PathName(), " + FileNameFieldName + ", GET_FILESTREAM_TRANSACTION_CONTEXT() From " + TableName + " Where " + KeyFieldName + " = @KeyValue;";

                cmd.CommandText = sSQL;
                cmd.Parameters.AddWithValue("@KeyValue", KeyValue);

                SqlDataReader dr = null;
                try
                {
                    dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                    if (dr.Read())
                    {
                        string sPathOfFileStreamField = dr[0].ToString();
                        sFileName = dr[1].ToString();
                        byte[] transContext = (byte[])dr[2];

                        SqlFileStream sfs = new SqlFileStream(sPathOfFileStreamField, transContext, System.IO.FileAccess.Read);

                        buffer = new byte[(int)sfs.Length];
                        sfs.Read(buffer, 0, buffer.Length);
                        sfs.Close();
                    }
                } catch { }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                        dr = null;
                    }
                }
            }
            
            TransObj.Commit();

            return new FileInfoForOutput(buffer, sFileName);
        }
        #endregion

        #region Output from "GeneratedFile"
        public static FileInfoForOutput OutputGeneratedFileInStream(SqlConnection SqlConn, SqlTransaction TransObj, string KeyValue)
        {
            return OutputInStream(SqlConn, TransObj, "CQM_GeneratedFiles", "FS_Source", "FS_FileName", "FS_GUID", KeyValue);
        }
        #endregion
    }
}