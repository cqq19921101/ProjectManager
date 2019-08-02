using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib_SQM_Domain.SharedLibs
{
    public enum ApproveType
    {
        Approve = 0,
        Reject = 1
    }

    public enum ApproverType
    {
        SuperVisor = 0,
        ViceManager = 1,
        Manager = 2,
        Director = 3,
        BUHEAD = 4,
    }
}
