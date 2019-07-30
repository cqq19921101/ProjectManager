using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;

namespace LiteOn.GDS.Utility
{
    interface IDOA
    {
        Model_DOAHandler GetStepHandler(Model_DOAHandler pDOAHandler, DataTable dtHead, DataTable dtDetail);

        Model_DOAHandler GetStepHandler_StartForm(Model_DOAHandler pDOAHandler, DataTable dtHead, DataTable dtDetail);

        Hashtable GetMobileFormFields(DataTable dtHead, DataTable dtDetail);
    }
}
