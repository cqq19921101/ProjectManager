using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lib_SQM_Domain.SharedLibs
{
    public class FileInfoForOutput
    {
        protected byte[] _buffer = null;
        protected string _FileName = "";
        protected string _MimeMapping = "";

        public byte[] Buffer { get { return this._buffer; } }
        public string FileName { get { return this._FileName; } }
        public string MimeMapping { get { return this._MimeMapping; } }

        public FileInfoForOutput() { }
        public FileInfoForOutput(byte[] buffer, string FileName)
        {
            this._buffer = buffer;
            this._FileName = FileName;
            this._MimeMapping = System.Web.MimeMapping.GetMimeMapping(FileName);
        }
    }
}