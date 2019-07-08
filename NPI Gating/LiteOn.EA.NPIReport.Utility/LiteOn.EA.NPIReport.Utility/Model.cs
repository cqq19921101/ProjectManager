using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace LiteOn.EA.NPIReport.Utility
{
    public class Model_DFX_PARAM
    {
        public string _FUNCTION_NAME;
        public string _ID;
        public string _PARAME_NAME;
        public string _PARAME_ITEM;
        public string _PARAME_VALUE1;
        public string _PARAME_VALUE2;
        public string _PARAME_VALUE3;
        public string _PARAME_VALUE4;
        public string _PARAME_VALUE5;
        public string _Building;
        public string _UPDATE_USER;
        public DateTime _UPDATE_TIME;
        public string _PARAME_TYPE;

        public Model_DFX_PARAM()
        {
            _FUNCTION_NAME = string.Empty;
            _ID = string.Empty;
            _PARAME_NAME = string.Empty;
            _PARAME_ITEM = string.Empty;
            _PARAME_VALUE1 = string.Empty;
            _PARAME_VALUE2 = string.Empty;
            _PARAME_VALUE3 = string.Empty;
            _PARAME_VALUE4 = string.Empty;
            _PARAME_VALUE5 = string.Empty;
            _Building = string.Empty;
            _UPDATE_USER = string.Empty;
            _UPDATE_TIME = DateTime.MaxValue;
            _PARAME_TYPE = string.Empty;

        }
    }

    public class Model_NPI_MEMBER
    {
        public string _ID;
        public string _BU;
        public string _BUILDING;
        public string _CATEGORY;
        public string _DEPT;
        public string _ENAME;
        public string _CNAME;
        public string _EMAIL;
        public string _PHASE;

        public DateTime _UPDATE_TIME;
        public string _UPDATE_USERID;

        public Model_NPI_MEMBER()
        {
            _ID = String.Empty;
            _BU = string.Empty;
            _BUILDING = string.Empty;
            _CATEGORY = string.Empty;
            _DEPT = string.Empty;
            _ENAME = string.Empty;
            _CNAME = string.Empty;
            _EMAIL = string.Empty;
            _UPDATE_TIME = DateTime.MaxValue;
            _UPDATE_USERID = string.Empty;
            _PHASE = string.Empty;

        }
    }

    

    public class Model_APPLICATION_PARAM
    {
        public string _APPLICATION_NAME;
        public string _FUNCTION_NAME;
        public string _PARAME_NAME;
        public string _PARAME_ITEM;
        public string _PARAME_VALUE1;
        public string _PARAME_VALUE2;
        public string _PARAME_VALUE3;
        public string _PARAME_VALUE4;
        public string _REMARK;
        public string _ENABLED;
        public string _UPDATE_USERID;
        public DateTime _UPDATE_TIME;

        public Model_APPLICATION_PARAM()
        {
            _APPLICATION_NAME = string.Empty;
            _FUNCTION_NAME = string.Empty;
            _PARAME_NAME = string.Empty;
            _PARAME_ITEM = string.Empty;
            _PARAME_VALUE1 = string.Empty;
            _PARAME_VALUE2 = string.Empty;
            _PARAME_VALUE3 = string.Empty;
            _PARAME_VALUE4 = string.Empty;
            _REMARK = string.Empty;
            _ENABLED = string.Empty;
            _UPDATE_USERID = string.Empty;
            _UPDATE_TIME = DateTime.MaxValue;

        }
    }

    public class Model_NPI_APP_MEMBER
    {
        public int _ID;
        public string _DOC_NO;
        public string _Category;
        public string _DEPT;
        public string _WriteEname;
        public string _WriteCname;
        public string _WriteEmail;
        public string _ReplyEName;
        public string _ReplyCname;
        public string _ReplyEmai;
        public string _CheckedEname;
        public string _CheckedEmail;
        public string _CategoryFlag;
        public DateTime _UPDATE_TIME;
        public string _UPDATE_USERID;

        public Model_NPI_APP_MEMBER()
        {
            _ID = 0;
            _DOC_NO = string.Empty;
            _Category = string.Empty;
            _DEPT = string.Empty;
            _WriteEname = string.Empty;
            _WriteCname = string.Empty;
            _WriteEmail = string.Empty;
            _ReplyEName = string.Empty;
            _ReplyCname = string.Empty;
            _ReplyEmai = string.Empty;
            _CheckedEname = string.Empty;
            _CheckedEmail = string.Empty;
            _UPDATE_TIME = DateTime.MaxValue;
            _CategoryFlag = string.Empty;
            _UPDATE_USERID = string.Empty;

        }
    }


    public class Model_NPI_APP_ISSUELIST
    {
        public string _ID;
        public string _DOC_NO;
        public string _SUB_DOC_NO;
        public string _PHASE;
        public string _STATION;
        public string _ITEMS;
        public string _PRIORITYLEVEL;
        public string _ISSUE_DESCRIPTION;
        public string _ISSUE_LOSSES;
        public string _TEMP_MEASURE;
        public string _IMPROVE_MEASURE;
        public string _PERSON_IN_CHARGE;
        public string _DUE_DAY;
        public string _CURRENT_STATUS;
        public string _AFFIRMACE_MAN;
        public string _STAUTS;
        public string _TRACKING;
        public string _REMARK;
        public DateTime _CREATE_TIME;
        public string _CREATE_USERID;
        public DateTime _UPDATE_TIME;
        public string _UPDATE_USERID;
        public string _DEPT;
        public string _FILEPATH;
        public string _FILENAME;
        public string _CLASS;
        public string _MEASURE_DEPTREPLY;

        public Model_NPI_APP_ISSUELIST()
        {
            _ID = string.Empty;
            _DOC_NO = string.Empty;
            _SUB_DOC_NO = string.Empty;
            _PHASE = string.Empty;
            _STATION = string.Empty;
            _ITEMS = string.Empty;
            _PRIORITYLEVEL = string.Empty;
            _ISSUE_DESCRIPTION = string.Empty;
            _ISSUE_LOSSES = string.Empty;
            _TEMP_MEASURE = string.Empty;
            _IMPROVE_MEASURE = string.Empty;
            _PERSON_IN_CHARGE = string.Empty;
            _DUE_DAY = string.Empty;
            _CURRENT_STATUS = string.Empty;
            _AFFIRMACE_MAN = string.Empty;
            _STAUTS = string.Empty;
            _TRACKING = string.Empty;
            _REMARK = string.Empty;
            _CREATE_TIME = DateTime.MaxValue;
            _CREATE_USERID = string.Empty;
            _UPDATE_TIME = DateTime.MaxValue;
            _UPDATE_USERID = string.Empty;
            _DEPT = string.Empty;
            _FILENAME = string.Empty;
            _FILEPATH = string.Empty;
            _CLASS = string.Empty;
            _MEASURE_DEPTREPLY = string.Empty;

        }
    }

    public class Model_NPI_APP_SUB
    {
        public string _DOC_NO;
        public string _SUB_DOC_NO;
        public string _Building;
        public string _SUB_DOC_PHASE;
        public string _SUB_DOC_PHASE_A;
        public string _WorkOrder;
        public string _SUB_DOC_PHASE_RESULT;
        public string _SUB_DOC_PHASE_STATUS;
        public int _SUB_DOC_PHASE_VERSION;
        public DateTime _UPDATE_TIME;
        public string _UPDATE_USERID;
        public string _DFX_STATUS;
        public string _CTQ_STATUS;
        public string _CLCA_STATUS;
        public string _ISSUES_STATUS;
        public string _PFMA_STATUS;
        public DateTime _CREATE_DATE;
        public int _CTQ_QTY;
        public int _CLCA_QTY;
        public string _CLCA_BEGIN_TIME;
        public string _CLCA_END_TIME;
        public int _LOT_QTY;
        public string _PCB_REV;
        public string _SPEC_REV;
        public DateTime _ISSUE_DATE;
        public string _INPUT_DATE;
        public string _CUSTOMER;
        public string _LINE;
        public string _BOM_REV;
        public string _CUSTOMER_REV;
        public string _RELEASET_DATE;
        public string _PK_DATE;
        public string _NeedStartItmes;
        public string _PROD_GROUP;
        public int _CASEID;
        public string _MODIFYFLAG;
        public string _REMARKM;

        public string _FROMMODEL;
        public string _REMARK;
        public string _MODELLINK;

        public Model_NPI_APP_SUB()
        {
            _DOC_NO = string.Empty;

            _SUB_DOC_NO = string.Empty;
            _Building = string.Empty;
            _SUB_DOC_PHASE = string.Empty;
            _WorkOrder = string.Empty;
            _SUB_DOC_PHASE_RESULT = string.Empty;
            _SUB_DOC_PHASE_STATUS = string.Empty;
            _SUB_DOC_PHASE_VERSION = 0;
            _UPDATE_TIME = DateTime.MaxValue;
            _UPDATE_USERID = string.Empty;
            _CTQ_STATUS = string.Empty;
            _CLCA_STATUS = string.Empty;
            _ISSUES_STATUS = string.Empty;
            _PFMA_STATUS = string.Empty;
            _CREATE_DATE = DateTime.MaxValue;
            _CTQ_QTY = 0;
            _CLCA_QTY = 0;
            _CLCA_BEGIN_TIME = string.Empty;
            _CLCA_END_TIME = string.Empty;
            _LOT_QTY = 0;
            _PCB_REV = string.Empty;
            _SPEC_REV = string.Empty;
            _ISSUE_DATE = DateTime.MaxValue;
            _INPUT_DATE = string.Empty;
            _CUSTOMER = string.Empty;
            _LINE = string.Empty;
            _BOM_REV = string.Empty;
            _CUSTOMER_REV = string.Empty;
            _RELEASET_DATE = string.Empty;
            _PK_DATE = string.Empty;
            _NeedStartItmes = string.Empty;
            _PROD_GROUP = string.Empty;
            _CASEID = 0;
            _MODIFYFLAG = string.Empty;
            _REMARKM = string.Empty;

            _FROMMODEL = string.Empty;
            _REMARK = string.Empty;
            _MODELLINK = string.Empty;
        }
    }

    public class Model_DFX_ITEMBODY
    {

        public string _DFXNo;
        public string _Item;
        public string _ItemType;
        public string _ItemName;
        public string _Requirements;
        public string _Losses;
        public string _Location;
        public string _Severity;
        public string _Occurrence;
        public string _Detection;
        public string _RPN;
        public string _Class;
        public string _PriorityLevel;
        public string _MaxPoints;
        public string _DFXPoints;
        public string _WriteDept;
        public string _Compliance;
        public string _Comments;
        public string _Status;
        public string _UpdateUser;
        public DateTime _UpdateTime;

        public string _Actions;
        public string _CompletionDate;
        public string _Tracking;
        public string _Remark;
        public Model_DFX_ITEMBODY()
        {

            _DFXNo = string.Empty;
            _Item = string.Empty;
            _ItemType = string.Empty;
            _ItemName = string.Empty;
            _Requirements = string.Empty;
            _Losses = string.Empty;
            _Location = string.Empty;
            _Severity = string.Empty;
            _Occurrence = string.Empty;
            _Detection = string.Empty;
            _RPN = string.Empty;
            _Class = string.Empty;
            _PriorityLevel = string.Empty;
            _MaxPoints = string.Empty;
            _DFXPoints = string.Empty;
            _WriteDept = string.Empty;
            _Compliance = string.Empty;
            _Comments = string.Empty;
            _Status = string.Empty;
            _UpdateUser = string.Empty;
            _UpdateTime = DateTime.MaxValue;
            _Actions = string.Empty;
            _CompletionDate = string.Empty;
            _Tracking = string.Empty;
            _Remark = string.Empty;

        }
    }


    public class Model_NPI_APP_CTQ
    {
        public string _DOC_NO;
        public int _ID;
        public string _SUB_DOC_NO;
        public string _PROD_GROUP;
        public string _PHASE;
        public string _DEPT;
        public string _PROCESS;
        public string _CTQ;
        public string _UNIT;
        public string _SPC;
        public string _SPEC_LIMIT;
        public string _CONTROL_TYPE;
        public float _GOAL;
        public string _ACT;
        public string _RESULT;
        public string _Comment;
        public string _STATUS;
        public string _DESCRIPTION;
        public string _DUTY_DEPT;
        public string _DUTY_EMP;
        public string _ROOT_CAUSE;
        public string _D;
        public string _M;
        public string _P;
        public string _E;
        public string _W;
        public string _O;
        public string _TEMPORARY_ACTION;
        public string _CORRECTIVE_PREVENTIVE_ACTION;
        public string _COMPLETE_DATE;
        public string _IMPROVEMENT_STATUS;
        public DateTime _UPDATE_TIME;
        public string _REPLY_USERID;
        public string _W_FILEPATH;
        public string _W_FILENAME;
        public string _R_FILEPATH;
        public string _R_FILENAME;

        public Model_NPI_APP_CTQ()
        {
            _DOC_NO = string.Empty;
            _ID = 0;
            _SUB_DOC_NO = string.Empty;
            _PROD_GROUP = string.Empty;
            _PHASE = string.Empty;
            _DEPT = string.Empty;
            _PROCESS = string.Empty;
            _CTQ = string.Empty;
            _UNIT = string.Empty;
            _SPC = string.Empty;
            _SPEC_LIMIT = string.Empty;
            _CONTROL_TYPE = string.Empty;
            _GOAL = 0;
            _ACT = string.Empty;
            _RESULT = string.Empty;
            _Comment = string.Empty;
            _STATUS = string.Empty;
            _DESCRIPTION = string.Empty;
            _DUTY_DEPT = string.Empty;
            _DUTY_EMP = string.Empty;
            _ROOT_CAUSE = string.Empty;
            _D = string.Empty;
            _M = string.Empty;
            _P = string.Empty;
            _E = string.Empty;
            _W = string.Empty;
            _O = string.Empty;
            _TEMPORARY_ACTION = string.Empty;
            _CORRECTIVE_PREVENTIVE_ACTION = string.Empty;
            _COMPLETE_DATE = string.Empty;
            _IMPROVEMENT_STATUS = string.Empty;
            _UPDATE_TIME = DateTime.MaxValue;
            _REPLY_USERID = string.Empty;
            _R_FILENAME = string.Empty;
            _R_FILEPATH = string.Empty;
            _W_FILENAME = string.Empty;
            _W_FILEPATH = string.Empty;

        }
    }


    public class Model_NPI_APP_ATTACHFILE
    {
        public string _SUB_DOC_NO;
        public int _CASEID;
        public int _ID;
        public string _FILE_PATH;
        public string _FILE_TYPE;
        public string _FILE_NAME;
        public string _DEPT;
        public string _REMARK;
        public string _APPROVER;
        public string _APPROVER_OPINION;
        public DateTime _APPROVER_DATE;
        public DateTime _UPDATE_TIME;
        public string _UPDATE_USERID;

        public Model_NPI_APP_ATTACHFILE()
        {
            _SUB_DOC_NO = string.Empty;
            _CASEID = 0;
            _ID = 0;
            _FILE_PATH = string.Empty;
            _FILE_TYPE = string.Empty;
            _FILE_NAME = string.Empty;
            _DEPT = string.Empty;
            _REMARK = string.Empty;
            _APPROVER = string.Empty;
            _APPROVER_OPINION = string.Empty;
            _APPROVER_DATE = DateTime.MaxValue;
            _UPDATE_TIME = DateTime.MaxValue;
            _UPDATE_USERID = string.Empty;

        }
    }


    public class Model_NPI_APP_RESULT
    {
        public string _SUB_DOC_NO;
        public int _CASEID;
        public string _DEPT;
        public string _REMARK;
        public string _APPROVER;
        public string _APPROVER_RESULT;
        public string _APPROVER_OPINION;
        public string _APPROVER_Levels;
        public DateTime _APPROVER_DATE;
        public DateTime _UPDATE_TIME;
        public string _UPDATE_USERID;

        public Model_NPI_APP_RESULT()
        {
            _SUB_DOC_NO = string.Empty;
            _CASEID = 0;
            _DEPT = string.Empty;
            _REMARK = string.Empty;
            _APPROVER = string.Empty;
            _APPROVER_RESULT = string.Empty;
            _APPROVER_OPINION = string.Empty;
            _APPROVER_Levels = string.Empty;
            _APPROVER_DATE = DateTime.MaxValue;
            _UPDATE_TIME = DateTime.MaxValue;
            _UPDATE_USERID = string.Empty;

        }
    }

    public class Model_NPI_FMEA
    {
        public string _SubNo;
        public string _Item;
        public string _Source;
        public string _Stantion;
        public string _PotentialFailureMode;
        public string _Loess;
        public int _Sev;
        public string _PotentialFailure;
        public int _Occ;
        public string _CurrentControls;
        public int _DET;
        public int _RPN;
        public string _RecommendedAction;
        public string _Resposibility;
        public DateTime _TargetCompletionDate;
        public string _ActionsTaken;
        public int _ResultsSev;
        public int _ResultsOcc;
        public int _ResultsDet;
        public int _ResultsRPN;
        public string _WriteDept;
        public string _ReplyDept;
        public string _Status;
        public string _Update_User;
        public int _ID;
        public DateTime _Update_Time;
        public string _FILEPATH;
        public string _FILENAME;


        public Model_NPI_FMEA()
        {
            _SubNo = string.Empty;
            _Item = string.Empty;
            _Source = string.Empty;
            _Stantion = string.Empty;
            _PotentialFailureMode = string.Empty;
            _Loess = string.Empty;
            _Sev = 0;
            _PotentialFailure = string.Empty;
            _Occ = 0;
            _CurrentControls = string.Empty;
            _DET = 0;
            _RPN = 0;
            _RecommendedAction = string.Empty;
            _Resposibility = string.Empty;
            _TargetCompletionDate = DateTime.MaxValue;
            _ActionsTaken = string.Empty;
            _ResultsSev = 0;
            _ResultsOcc = 0;
            _ResultsDet = 0;
            _ResultsRPN = 0;
            _WriteDept = string.Empty;
            _ReplyDept = string.Empty;
            _Status = string.Empty;
            _Update_User = string.Empty;
            _Update_Time = DateTime.MaxValue;
            _ID = 0;
            _FILENAME = string.Empty;
            _FILEPATH = string.Empty;

        }
    }

    public class Model_PRELAUNCH_CHECKITEMCONFIG
    {
        public string _Bu;
        public string _Building;
        public string _Dept;
        public string _CheckItem;
        public string _AttachmentFlag;
        public string _UpdateUser;
        public DateTime _UpdateTime;
        public string _ID;

        public Model_PRELAUNCH_CHECKITEMCONFIG()
        {
            _Bu = string.Empty;
            _Building = string.Empty;
            _Dept = string.Empty;
            _CheckItem = string.Empty;
            _AttachmentFlag = string.Empty;
            _UpdateUser = string.Empty;
            _UpdateTime = DateTime.MaxValue;
            _ID = string.Empty;

        }
    }

    public class Model_PRELAUNCH_STEP_HANDLER
    {
        public int _CASEID;
        public string _FORMNO;
        public string _STEP_NAME;
        public string _DEPT;
        public string _HANDLER;
        public String _BU;
        public string _BUILDING;
        public DateTime _UPDATE_TIME;

        public Model_PRELAUNCH_STEP_HANDLER()
        {
            _CASEID = 0;
            _FORMNO = string.Empty;
            _STEP_NAME = string.Empty;
            _DEPT = string.Empty;
            _HANDLER = string.Empty;
            _UPDATE_TIME = DateTime.MaxValue;
            _BU = string.Empty;
            _BUILDING = string.Empty;


        }
    }

    public class Model_PRELAUNCH_MAIN
    {
        public string _Bu;
        public string _Building;
        public string _PilotRunNO;
        public string _Model;
        public string _Customer;
        public string _PCBInRev;
        public string _PCBOutRev;
        public string _PLRev;
        public string _Date;
        public string _TP_ME;
        public string _TP_EE;
        public string _TP_PM;
        public string _PM;
        public int _CaseId;
        public string _Status;
        public string _Applicant;
        public string _Notes;
        public Model_PRELAUNCH_MAIN()
        {
            _Bu = string.Empty;
            _Building = string.Empty;
            _PilotRunNO = string.Empty;
            _Model = string.Empty;
            _Customer = string.Empty;
            _PCBInRev = string.Empty;
            _PCBOutRev = string.Empty;
            _PLRev = string.Empty;
            _Date = string.Empty;
            _TP_ME = string.Empty;
            _TP_EE = string.Empty;
            _TP_PM = string.Empty;
            _PM = string.Empty;
            _CaseId = 0;
            _Status = string.Empty;
            _Applicant = string.Empty;
            _Notes = string.Empty;

        }
    }


    public class Model_NPI_DOA_Parameter
    {
        public int _CASE_ID;
        public string _FORM_NO;
        public string _BU;
        public string _BUILDING;
        public string _APPORVER;
        public string _APPLY;
        public string _FLAG;
        public string _REMARK;
        public string _PHASE;
        public Model_NPI_DOA_Parameter()
        {

            _CASE_ID = 0;
            _FORM_NO = string.Empty;
            _BU = string.Empty;
            _BUILDING = string.Empty;
            _APPORVER = string.Empty;
            _APPLY = string.Empty;
            _FLAG = string.Empty;
            _REMARK = string.Empty;
            _PHASE = string.Empty;
        }
    }


    public class Model_NPI_DOA_HandlerInfo
    {
        public int _CASE_ID;
        public bool _RESULT;//获取签核关卡及签核人结果
        public string _ERROR_MSG;//操作异常反馈信息
        public string _FORM_NO;
        public DataTable _HANDLER;//所有签核关卡及签核人资讯
        public Model_NPI_DOA_HandlerInfo()
        {
            _CASE_ID = 0;
            _RESULT = false;
            _FORM_NO = string.Empty;
            _HANDLER = new DataTable("Results");
            _ERROR_MSG = string.Empty;
        }
    }

    public class Model_NPI_APP_MAIN_HIS
    {
        public string _DOC_NO;
        public string _BU;
        public string _BUILDING;
        public string _APPLY_DATE;
        public string _APPLY_USERID;
        public string _MODEL_NAME;
        public string _CUSTOMER;
        public string _PRODUCT_TYPE;
        public string _LAYOUT;
        public string _PHASE;
        public string _NEXTSTAGE_DATE;
        public DateTime _UPDATE_TIME;
        public string _UPDATE_USERID;
        public string _NPI_PM;
        public string _SALES_OWNER;
        public string _RD_ENGINEER;
        public string _REMARK;
        public string _CASEID;
        public string _STATUS;
        public string _PMLOC;
        public string _PMEXT;
        public string _RDLOC;
        public string _RDEXT;
        public String _SALESLOC;
        public string _SALESEXT;

        public Model_NPI_APP_MAIN_HIS()
        {
            _DOC_NO = string.Empty;
            _BU = string.Empty;
            _BUILDING = string.Empty;
            _APPLY_DATE = string.Empty;
            _APPLY_USERID = string.Empty;
            _MODEL_NAME = string.Empty;
            _CUSTOMER = string.Empty;
            _PRODUCT_TYPE = string.Empty;
            _LAYOUT = string.Empty;
            _PHASE = string.Empty;
            _NEXTSTAGE_DATE = string.Empty;
            _UPDATE_TIME = DateTime.MaxValue;
            _UPDATE_USERID = string.Empty;
            _NPI_PM = string.Empty;
            _SALES_OWNER = string.Empty;
            _RD_ENGINEER = string.Empty;
            _PMLOC=string.Empty;
            _PMEXT=string.Empty;
            _RDLOC=string.Empty;
            _RDEXT=string.Empty;
            _SALESLOC=string.Empty;
            _SALESEXT=string.Empty;
        }
    }

    public class Model_DFX_Item
    {
        public string _BU;
        public string _BUILDING;
        public string _ItemID;
        public string _Item;
        public string _ItemType;
        public string _ItemName;
        public string _Requirements;
        public string _ProductType;
        public int _PriorityLevel;
        public string _Losses;
        public string _WriteDept;
        public string _ReplyDept;
        public string _OldItemType;

        public Model_DFX_Item()
        {
            _BU = string.Empty;
            _BUILDING = string.Empty;
            _ItemID = string.Empty;
            _Item = string.Empty;
            _ItemType = string.Empty;
            _ItemName = string.Empty;
            _Requirements = string.Empty;
            _ProductType = string.Empty;
            _PriorityLevel = 0;
            _Losses = string.Empty;
            _WriteDept = string.Empty;
            _ReplyDept = string.Empty;
            _OldItemType = string.Empty;
        }
    }
}
