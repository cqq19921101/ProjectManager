using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Web.Script.Serialization;
using Lib_SQM_Domain.SharedLibs;
using System.Web;

namespace Lib_SQM_Domain.Modal
{
    public enum ReturnFieldSets { TableMaintain, ShortList }
    public enum RemoveOptions { Remove1, RemoveAll }
}
