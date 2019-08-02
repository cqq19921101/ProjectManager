$(function () {
    var diaPlant = $('#dialog_VMI_QueryPlantInfo');
    var diaSBUVendor = $('#dialog_VMI_QuerySBUVendor');
    var diaEmail = $('#dialog_VMI_QueryEmailInfo');


    $('#btnQueryEmail').click(function () {
        $(this).removeClass('ui-state-focus');
        ReloadEmailgridDataList();
    });

    $('#dialog_btn_diaEmail_Search').click(function () {
        $(this).removeClass('ui-state-focus');
        ReloadDiaEmailgridDataList();
    });

    $('#dialog_btn_diaSBUVendor_Search').click(function () {
        $(this).removeClass('ui-state-focus');
        ReloadDiaSBUVendorCodegridDataList();
    });

    $('#dialog_btn_diaPlant_Search').click(function () {
        $(this).removeClass('ui-state-focus');
        ReloadDiaPlantCodegridDataList();
    });

    $('#btnOpenQueryPlantDialog').click(function () {
        $(this).removeClass('ui-state-focus');
        if (!__DialogIsShownNow) {
            __DialogIsShownNow = true;
            __SelectorName = '#txtPlant';

            InitdialogPlant();
            ReloadDiaPlantCodegridDataList();

            diaPlant.show();
            diaPlant.dialog("open");
        }
    });

    $('#btnOpenQueryVendorCodeDialog').click(function () {
        $(this).removeClass('ui-state-focus');
        if (!__DialogIsShownNow) {
            __DialogIsShownNow = true;
            __SelectorName = '#txtVendorCode';

            InitdialogSBUVendor();
            ReloadDiaSBUVendorCodegridDataList();

            diaSBUVendor.show();
            diaSBUVendor.dialog("open");
        }
    });

    $('#btnOpenQueryEmailDialog').click(function () {
        $(this).removeClass('ui-state-focus');
        if (!__DialogIsShownNow) {
            __DialogIsShownNow = true;
            __SelectorName = '#txtEmail';

            InitdialogEmail();
            ReloadDiaEmailgridDataList();

            diaEmail.show();
            diaEmail.dialog("open");
        }
    });
})

function InitdialogEmail() {
    $('#dialog_VMI_txt_Email').val("");
}

//Init dialogSBUVendor UI
function InitdialogSBUVendor() {
    $('#dialog_VMI_txt_SBU_VDN').val("");
}
//Init dialogPlantCode UI
function InitdialogPlant() {
    $('#dialog_VMI_txt_Plant').val("");
}

function ReloadDiaPlantCodegridDataList() {
    var diaPlantgridData = $('#dialog_VMI_PlantCode_gridDataList');
    var diatxtPlant = $('#dialog_VMI_txt_Plant');

    diaPlantgridData.jqGrid('clearGridData');
    diaPlantgridData.jqGrid('setGridParam', { postData: { PLANT: escape($.trim(diatxtPlant.val())) } });
    diaPlantgridData.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}

function ReloadDiaSBUVendorCodegridDataList() {
    var diaSBUVendorgridData = $('#dialog_VMI_SBUVendor_gridDataList');
    var diatxtSBUVDN = $('#dialog_VMI_txt_SBU_VDN');

    diaSBUVendorgridData.jqGrid('clearGridData');
    diaSBUVendorgridData.jqGrid('setGridParam', { postData: { ERP_VND: escape($.trim(diatxtSBUVDN.val())) } });
    diaSBUVendorgridData.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}

function ReloadDiaEmailgridDataList() {
    var diaEmailgridData = $('#dialog_VMI_Email_gridDataList');
    var diatxtEmail = $('#dialog_VMI_txt_Email');

    diaEmailgridData.jqGrid('clearGridData');
    diaEmailgridData.jqGrid('setGridParam', { postData: { Email: escape($.trim(diatxtEmail.val())) } });
    diaEmailgridData.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}





function ReloadEmailgridDataList() {
    $.ajax({
        url: __WebAppPathPrefix + "/SQMMailR/SendMailSQE",
        data: {
            PlantCode: escape($.trim($("#txtPlant").val())),
            VendorCode: escape($.trim($("#txtVendorCode").val())),
            Email: escape($.trim($("#txtEmail").val()))
        },


        type: "post",
        dataType: 'text',
        async: false,
        beforeSend: function () {
            $("#dialogDownloadSplash").dialog({
                title: 'Uploading Notify',
                width: 'auto',
                height: 'auto',
                modal: true,
                open: function (event, ui) {
                    $(this).parent().find('.ui-dialog-titlebar-close').hide();
                    $(this).parent().find('.ui-dialog-buttonpane').hide();
                    $("#lbDiaDownloadMsg").html('loading...</br></br>Please wait for the operation a moment...');
                }
            }).dialog("open");
        },
        success: function (data) {
            $("#dialogDownloadSplash").dialog('close');
            if (data == "") {
                alert("此供應商尚未設定對應SQE,請通知SQE維護資料");
            }
            else {
             $("#dialogDownloadSplash").dialog('close');
                switch (data) {
                    case "成功":
                        alert("發送成功");
                        break;


                    case "傳入參數錯誤":
                        alert("傳入參數錯誤");
                        break;


                    case "非授權使用者":
                        alert("非授權使用者");
                        break;


                    case "新帳號格式錯誤":
                        alert("新帳號格式錯誤");
                        break;


                    case "新帳號已存在":
                        alert("新帳號已存在");
                        break;


                    case "密碼不符規則":
                        alert("密碼不符規則");
                        break;


                    case "未提供電子郵件信箱或格式錯誤":
                        alert("未提供電子郵件信箱或格式錯誤");
                        break;

                    case "CORP_VND已存在，請檢查后重新輸入！":
                        alert("CORP_VND已存在，請檢查后重新輸入！");
                        break;

                    case "此賬號已存在！":
                        alert("此賬號已存在！");
                        break;




                }
            }
        }
    });

}


