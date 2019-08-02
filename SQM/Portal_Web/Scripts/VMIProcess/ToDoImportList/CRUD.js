$(function () {
    $('#btnNewImportList').click(function () {
        $(this).removeClass('ui-state-focus');

        $('#dialogImportListForm').dialog('open');
    });

    $('#btnSave').click(function () {
        $(this).removeClass('ui-state-focus');

        saveImportList();
    });

    $('#btnAddDNinList').click(function () {
        $(this).removeClass('ui-state-focus');

        $('#dialogAddDNinList').dialog('open');
    });

    $('#btnDeleteAll').click(function () {
        $(this).removeClass('ui-state-focus');

        var IMPORT_LIST_NUM = $('#sImportListNum').text();

        if (confirm("Are you sure to delete this Import List?")) {
            $.ajax({
                url: __WebAppPathPrefix + '/VMIProcess/DeleteImportList',
                data: {
                    IMPORT_LIST_NUM: escape($.trim(IMPORT_LIST_NUM))
                },
                type: "post",
                dataType: 'text',
                async: false,
                success: function (data) {
                    if (data == 'Success') {
                        alert('Delete successfully.');
                        $('#dialogImportListForm').removeProp('IMPORT_LIST_NUM');
                        $('#dialogImportListForm').dialog('close');
                        $('#gridImportList').jqGrid('clearGridData');
                    }
                    else if (data.indexOf('Error:') != -1) {
                        alert(data);
                    }
                },
                error: function (xhr, textStatus, thrownError) {
                    $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                },
                complete: function (jqXHR, textStatus) {
                }
            });
        }
    });

    $('#btnDelete').click(function () {
        $(this).removeClass('ui-state-focus');

        var ImportListDetails = $('#importListGridDataList').jqGrid('getGridParam', 'data');
        var arrASN_NUM = [];
        var arrDEL_ASN_NUM = [];
        var isChecked = false;
        for (var item in ImportListDetails) {
            var CHECK = ImportListDetails[item].CHECK;
            var ASN_NUM = ImportListDetails[item].ASN_NUM;

            if (CHECK == undefined || CHECK == false) {
                if (arrASN_NUM.indexOf(ASN_NUM) == -1) {
                    arrASN_NUM.push(ASN_NUM);
                }
            }
            else {
                isChecked = true;
                if (arrDEL_ASN_NUM.indexOf(ASN_NUM) == -1) {
                    arrDEL_ASN_NUM.push(ASN_NUM);
                }
            }
        }

        if (isChecked) {
            var IMPORT_LIST_NUM = $('#sImportListNum').text();
            var COMPANY_NAME = $('#ddlForwarderCompanyName').val();
            var DRIVER_NAME = $('#txtDriverName').val();
            var VEHICLE_TYPE_ID = $('#txtVehicleID').val();
            var DRIVER_TEL = $('#txtDriverTel').val();
            var RECEIVE_ADDR = $('#txtReceiveAddress').val();
            var RECEIVER = $('#txtReceiver').val();
            var RECEIVER_TEL = $('#txtReceiverTel').val();
            var PLAN_ARRIVAL_TIME = $('#txtPlanArrivalDate').val() == '' ? '' : $('#txtPlanArrivalDate').val() + ' ' + $('#txtPlanArrivalTime').val();
            //var PLANT = $('#txtPlant').val();
            var PLANT = $('#ddlPlant').val();
            var IS_CLOSE = $('#cbIsClose').prop('Checked');

            $.ajax({
                url: __WebAppPathPrefix + '/VMIProcess/EditImportList',
                data: {
                    ACTION: 'DELETE',
                    IMPORT_LIST_NUM: escape($.trim(IMPORT_LIST_NUM)),
                    COMPANY_NAME: escape($.trim(COMPANY_NAME)),
                    DRIVER_NAME: escape($.trim(DRIVER_NAME)),
                    VEHICLE_TYPE_ID: escape($.trim(VEHICLE_TYPE_ID)),
                    DRIVER_TEL: escape($.trim(DRIVER_TEL)),
                    RECEIVE_ADDR: escape($.trim(RECEIVE_ADDR)),
                    RECEIVER: escape($.trim(RECEIVER)),
                    RECEIVER_TEL: escape($.trim(RECEIVER_TEL)),
                    PLAN_ARRIVAL_TIME: escape($.trim(PLAN_ARRIVAL_TIME)),
                    ASN_NUM: arrASN_NUM,
                    DEL_ASN_NUM: arrDEL_ASN_NUM,
                    PLANT: escape($.trim(PLANT)),
                    IS_CLOSE: escape($.trim(IS_CLOSE))
                },
                type: "post",
                dataType: 'text',
                async: false,
                success: function (data) {
                    if (data.indexOf("IMPORT_LIST_NUM") != -1) {
                        $('#dialogAddDNinList').dialog('close');
                        initDialogImportListForm(data.split(":")[1]);
                    }
                    else if (data.indexOf("Error:") != -1) {
                        alert(data);
                    }
                    else {
                        alert('Error, please contact administrator manager.');
                        console.log(data);
                    }
                },
                error: function (xhr, textStatus, thrownError) {
                    $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                },
                complete: function (jqXHR, textStatus) {
                }
            });
        }
        else {
            alert('Please check at least one DN to delete.');
        }
    });

    $('#btnNoticeAndRelease').click(function () {
        $(this).removeClass('ui-state-focus');

        var IMPORT_LIST_NUM = $('#sImportListNum').text();

        if (IMPORT_LIST_NUM == '') {
            alert('There is no import list number, please save it before releasing.');
        }
        else {
            if (confirm('Do you want to save current Import List before releasing it?')) {
                saveImportList();
            }

            $.ajax({
                url: __WebAppPathPrefix + '/VMIProcess/NoticeAndReleaseImportList',
                data: {
                    IMPORT_LIST_NUM: escape($.trim(IMPORT_LIST_NUM))
                },
                type: "post",
                dataType: 'text',
                async: false, // if need page refresh, please remark this option
                success: function (data) {
                    alert(data);
                },
                error: function (xhr, textStatus, thrownError) {
                    $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                },
                complete: function (jqXHR, textStatus) {
                }
            });
        }
    });

    $('#btnPrintImportList').click(function () {
        $(this).removeClass('ui-state-focus');

        var IMPORT_LIST_NUM = $('#sImportListNum').text();
        $.ajax({
            url: __WebAppPathPrefix + '/VMIProcess/PrintImportListExportPDF',
            data: {
                IMPORT_LIST_NUM: escape($.trim(IMPORT_LIST_NUM))
            },
            type: "post",
            dataType: 'json',
            beforeSend: function () {
                $("#dialogDownloadSplash").dialog({
                    title: 'Download Notify',
                    width: 'auto',
                    height: 'auto',
                    open: function (event, ui) {
                        $(this).parent().find('.ui-dialog-titlebar-close').hide();
                        $(this).parent().find('.ui-dialog-buttonpane').hide();
                        $("#lbDiaDownloadMsg").html('Downloading...</br></br>Please wait for the operation a moment...');
                    }
                }).dialog("open");
            },
            success: function (data) {
                if (data.Result) {
                    if (data.FileKey != "") {
                        $("#dialogDownloadSplash_FileKey").val(data.FileKey);
                        $("#dialogDownloadSplash_FileName").val(data.FileName);
                        setTimeout(function () {
                            $("#dialogDownloadSplash_Form").attr('action', __WebAppPathPrefix + '/VMIProcess/RetrieveFileByFileKey').submit();
                            $("#dialogDownloadSplash").dialog("close");
                        }, 10);
                    }
                }
                else {
                    alert("Error");
                    $("#dialogDownloadSplash").dialog("close");
                }
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                $("#dialogDownloadSplash").dialog("close");
            }
        });
    });
});

function saveImportList() {
    var IMPORT_LIST_NUM = $('#sImportListNum').text();
    var COMPANY_NAME = $('#ddlForwarderCompanyName').val();
    var DRIVER_NAME = $('#txtDriverName').val();
    var VEHICLE_TYPE_ID = $('#txtVehicleID').val();
    var DRIVER_TEL = $('#txtDriverTel').val();
    var RECEIVE_ADDR = $('#txtReceiveAddress').val();
    var RECEIVER = $('#txtReceiver').val();
    var RECEIVER_TEL = $('#txtReceiverTel').val();
    var PLAN_ARRIVAL_TIME = $('#txtPlanArrivalDate').val() == '' ? '' : $('#txtPlanArrivalDate').val() + ' ' + $('#txtPlanArrivalTime').val();
    //var PLANT = $('#txtPlant').val();
    var PLANT = $('#ddlPlant').val();
    var IS_CLOSE = $('#cbIsClose').prop('checked');

    var ImportListDetails = $('#importListGridDataList').jqGrid('getGridParam', 'data');
    var ASN_NUM = [];
    for (var item in ImportListDetails) {
        if (ASN_NUM.indexOf(ImportListDetails[item].ASN_NUM) == -1) {
            ASN_NUM.push(ImportListDetails[item].ASN_NUM);
        }
    }

    var ACTION = $('#sImportListNum').text() == '' ? "CREATE" : "UPDATE";

    $.ajax({
        url: __WebAppPathPrefix + '/VMIProcess/EditImportList',
        data: {
            ACTION: escape($.trim(ACTION)),
            IMPORT_LIST_NUM: escape($.trim(IMPORT_LIST_NUM)),
            COMPANY_NAME: escape($.trim(COMPANY_NAME)),
            DRIVER_NAME: escape($.trim(DRIVER_NAME)),
            VEHICLE_TYPE_ID: escape($.trim(VEHICLE_TYPE_ID)),
            DRIVER_TEL: escape($.trim(DRIVER_TEL)),
            RECEIVE_ADDR: escape($.trim(RECEIVE_ADDR)),
            RECEIVER: escape($.trim(RECEIVER)),
            RECEIVER_TEL: escape($.trim(RECEIVER_TEL)),
            PLAN_ARRIVAL_TIME: escape($.trim(PLAN_ARRIVAL_TIME)),
            ASN_NUM: ASN_NUM,
            PLANT: escape($.trim(PLANT)),
            IS_CLOSE: escape($.trim(IS_CLOSE))
        },
        type: "post",
        dataType: 'text',
        async: false,
        success: function (data) {
            if (data.indexOf("IMPORT_LIST_NUM") != -1) {
                $('#dialogAddDNinList').dialog('close');
                initDialogImportListForm(data.split(":")[1]);
                switch (ACTION) {
                    case 'CREATE':
                        alert('Create new import list successfully.');
                        break;
                    case 'UPDATE':
                        alert('Save import list successfully.')
                        break;
                }
            }
            else {
                alert('Error, please contact administrator manager.');
                console.log(data);
            }
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
}