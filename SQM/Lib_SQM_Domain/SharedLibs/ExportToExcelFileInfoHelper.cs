using System;
using System.IO;

namespace Lib_SQM_Domain.SharedLibs
{
    public class ExportToExcelFileInfo
    {
        protected MemoryStream _ms = null;
        protected string _FileName = "";
        protected string _MimeMapping = "";

        public MemoryStream FileMmeoryStream { get { return this._ms; } }
        public string FileName { get { return this._FileName; } }
        public string MimeMapping { get { return this._MimeMapping; } }

        public ExportToExcelFileInfo() { }
        public ExportToExcelFileInfo(MemoryStream ms, string FileName)
        {
            this._ms = ms;
            this._FileName = FileName;
            this._MimeMapping = System.Web.MimeMapping.GetMimeMapping(FileName);
        }
    }
}
