using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib_SQM_Domain.SharedLibs
{
    public static class Portal_Parameters
    {
        private static string _SMTPServer = "10.1.15.143";
        public static string SMTPServer { get { return _SMTPServer; } set { _SMTPServer = value; } }
    }
}
