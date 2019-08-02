using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib_SQM_Domain.SharedLibs
{
    public static class MISCHelper
    {
        public static string MemberDisplayName(string MemberGUID, string NameInChinese, string NameInEnglish)
        {
            string r = "";
            if (MemberGUID != "")
                r = NameInChinese + " (" + NameInEnglish + ")";
            return r;
        }

        public static string MemberDisplayName(string NameInChinese, string NameInEnglish)
        {
            string r = "";
            if ((NameInChinese != "") || (NameInEnglish != ""))
                r = NameInChinese + " (" + NameInEnglish + ")";
            return r;
        }
    }
}
