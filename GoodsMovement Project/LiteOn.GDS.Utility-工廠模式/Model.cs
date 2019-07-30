using System;
using System.Collections.Generic;
using System.Text;

namespace LiteOn.GDS.Utility
{

    /// <summary>
    /// DOA 簽核資訊MODEL
    /// </summary>
    public class Model_DOAHandler
    {
        public string _sDOA { get; set; }//DOA XML STRING
        public string _sHandler { get; set; }//下一關卡簽核人
        public string _sStepName { get; set; }//下一關卡
        public string _sRoleCode { get; set; }//下一簽核角色
        public string _sApplicant { get; set; }//申請人
        public string _sJump { get; set; }//是否向后跳關
        public string _sEndFlag { get; set; }//是否最后一關
        public string _sPlant { get; set; }//單據PLANT
        public string _sDocNo { get; set; }//單據號
        public string _sDocYear { get; set; }//單據年份

        public bool _ParallelFlag { get; set; }//是否並簽
        public bool _FinalFlag { get; set; }//是否並簽最後一個
        public bool _UpdateParallelFlag { get; set; }//是否更新並簽狀態
        public string _cc { get; set; }  // CC 給相關人員
        public Model_DOAHandler()
        {
            _sDOA = string.Empty;
            _sJump = "N";
            _sEndFlag = "N";
            _sApplicant = string.Empty;
            _sHandler = string.Empty;
            _sStepName = string.Empty;
            _sRoleCode = string.Empty;
            _sPlant = string.Empty;
            _sDocNo = string.Empty;
            _sDocYear = string.Empty;
            _ParallelFlag = false;
            _FinalFlag = false;
            _UpdateParallelFlag = false;
            _cc=string.Empty;
        }
    }


    /// <summary>
    /// DOA 設定參數MODEL
    /// </summary>
    public class Model_DOAOption
    {
        public bool _bExist { get; set; }//設定參數是否存在
        public string _sRoleCode { get; set; }//DOA 角色名
        public string _sCheckOption { get; set; }//DOA 檢查選項
        public string _sJumpOption { get; set; }//DOA 跳關選項
        public Model_DOAOption()
        {
            _bExist = false;
            _sRoleCode = "";
            _sCheckOption = "N";
            _sJumpOption = string.Empty;
        }
    }
}
