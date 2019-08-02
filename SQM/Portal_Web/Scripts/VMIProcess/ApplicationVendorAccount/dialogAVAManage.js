var _dialogAVAManageHeight = 330;

$(function () {
    $('#dialogAVAManage').dialog({
        autoOpen: false,
        resizable: false,
        width: 1002,
        height: _dialogAVAManageHeight,
        modal: true,
        open: function (event, ui) {
            $this = $(this);
            $this.find('input[type="text"]').blur();
            var COMPANY_NAME_EN = $this.find('#txtCOMPANY_NAME_EN'),
                COMPANY_NAME_ZH = $this.find('#txtCOMPANY_NAME_ZH'),
                EMAIL = $this.find('#txtEMAIL'),
                lbScanFile = $this.find('#lbScanFile'),
                fileupload_uploadAVA = $this.find('#fileupload_uploadAVA'),
                btnExportExcel = $this.find('#btnExportExcel'),
                A_CODE = $this.find('#txtA_CODE'),
                VENDOR_ACCOUNT = $this.find('#txtVENDOR_ACCOUNT'),
                GROUP_NAME = $this.find('#txtGROUP_NAME'),
                trRejectReason = $this.find('.trRejectReason'),
                REJECT_REASON = $this.find('#txtREJECT_REASON'),
                BU_VENDOR_CODE = $this.find('#txtBU_VENDOR_CODE'),
                ddlSITE_ID = $this.find('#ddlSITE_ID'),
                G_CODE = $this.find('#txtG_CODE'),
                lbCREATE_DATE = $this.find('#lbCREATE_DATE'),
                lbCREATE_USER = $this.find('#lbCREATE_USER'),
                lbSTATUS_ID = $this.find('#lbSTATUS_ID'),
                trAccountList = $this.find('#trAccountList');

            A_CODE.on('change keyup paste', function (e) {
                $(this).val($(this).val().toUpperCase());
            });
            VENDOR_ACCOUNT.on('change keyup paste', function (e) {
                $(this).val($(this).val().toUpperCase());
            });
            BU_VENDOR_CODE.on('change keyup paste', function (e) {
                $(this).val($(this).val().toUpperCase());
            });
            G_CODE.on('change keyup paste', function (e) {
                $(this).val($(this).val().toUpperCase());
            });

            var functionButtons = $this.parent().find('.ui-dialog-buttonset button'),
                Status = $this.prop('STATUS_ID'),
                NVA_ID = $this.prop('NVA_ID');

            functionButtons.each(function () {
                $(this).hide();
            });
            fileupload_uploadAVA.hide();
            trAccountList.hide();

            var star = $('<span/>').css({ 'color': 'red', 'font-weight': '900' }).text('*').addClass('star');

            switch (Status) {
                case 'AccountReject':
                case 'MappingReject':
                    getAVAInfo(NVA_ID);
                    trRejectReason.show();
                    $this.dialog('option', 'height', 320);
                    break;
                case 'Cancel':
                    break;
                case 'New':
                    COMPANY_NAME_EN.attr('readonly', false);
                    COMPANY_NAME_EN.parent().prev().prepend(star.clone());

                    COMPANY_NAME_ZH.attr('readonly', false);
                    COMPANY_NAME_ZH.parent().prev().prepend(star.clone());

                    EMAIL.attr('readonly', false);
                    EMAIL.parent().prev().prepend(star.clone());

                    BU_VENDOR_CODE.attr('readonly', false);
                    BU_VENDOR_CODE.parent().prev().prepend(star.clone());

                    ddlSITE_ID.attr('disabled', false);
                    ddlSITE_ID.parent().prev().prepend(star.clone());

                    if (NVA_ID != '') {
                        getAVAInfo(NVA_ID);
                        GROUP_NAME.attr('readonly', false);
                        btnExportExcel.show();
                        functionButtons.find('.ui-button-text:contains("Save")').parent().show();
                        $this.dialog('option', 'height', 355);
                    }
                    else {
                        lbCREATE_DATE.text(getCurrentDateTime().toString());
                        lbCREATE_USER.text(_Requestor);
                        functionButtons.find('.ui-button-text:contains("Submit")').parent().show();
                    }
                    break;
                case 'Close':
                    getAVAInfo(NVA_ID);
                    break;
                case 'Downloaded':
                    getAVAInfo(NVA_ID);

                    COMPANY_NAME_EN.attr('readonly', false);
                    COMPANY_NAME_EN.parent().prev().prepend(star.clone());

                    COMPANY_NAME_ZH.attr('readonly', false);
                    COMPANY_NAME_ZH.parent().prev().prepend(star.clone());

                    EMAIL.attr('readonly', false);
                    EMAIL.parent().prev().prepend(star.clone());

                    GROUP_NAME.attr('readonly', false);
                    //GROUP_NAME.parent().prev().prepend(star.clone());

                    fileupload_uploadAVA.show();
                    btnExportExcel.show();

                    BU_VENDOR_CODE.attr('readonly', false);
                    BU_VENDOR_CODE.parent().prev().prepend(star.clone());

                    ddlSITE_ID.attr('disabled', false);
                    ddlSITE_ID.parent().prev().prepend(star.clone());

                    functionButtons.find('.ui-button-text:contains("Save")').parent().show();
                    $this.dialog('option', 'height', 430);
                    break;
                case 'PaperFlowEnd':
                    getAVAInfo(NVA_ID);
                    if (isAVAAccess()) {
                        COMPANY_NAME_EN.attr('readonly', false);
                        COMPANY_NAME_EN.parent().prev().prepend(star.clone());

                        COMPANY_NAME_ZH.attr('readonly', false);
                        COMPANY_NAME_ZH.parent().prev().prepend(star.clone());

                        EMAIL.attr('readonly', false);
                        EMAIL.parent().prev().prepend(star.clone());

                        A_CODE.attr('readonly', false);
                        A_CODE.parent().prev().prepend(star.clone());

                        GROUP_NAME.attr('readonly', false);
                        GROUP_NAME.parent().prev().prepend(star.clone());

                        trRejectReason.show();
                        REJECT_REASON.attr('readonly', false);

                        BU_VENDOR_CODE.attr('readonly', false);
                        BU_VENDOR_CODE.parent().prev().prepend(star.clone());

                        ddlSITE_ID.attr('disabled', false);
                        ddlSITE_ID.parent().prev().prepend(star.clone());

                        G_CODE.attr('readonly', false);
                        G_CODE.parent().prev().prepend(star.clone());

                        functionButtons.find('.ui-button-text:contains("Save")').parent().show();
                        functionButtons.find('.ui-button-text:contains("Reject")').parent().show();
                        $this.dialog('option', 'height', 360);
                    }
                    break;
                case 'SAPMappingEnd':
                    getAVAInfo(NVA_ID);
                    if (isAVAAccess()) {
                        COMPANY_NAME_EN.attr('readonly', false);
                        COMPANY_NAME_EN.parent().prev().prepend(star.clone());

                        COMPANY_NAME_ZH.attr('readonly', false);
                        COMPANY_NAME_ZH.parent().prev().prepend(star.clone());

                        EMAIL.attr('readonly', false);
                        EMAIL.parent().prev().prepend(star.clone());

                        VENDOR_ACCOUNT.attr('readonly', false);
                        VENDOR_ACCOUNT.parent().prev().prepend(star.clone());

                        trRejectReason.show();
                        REJECT_REASON.attr('readonly', false);

                        BU_VENDOR_CODE.attr('readonly', false);
                        BU_VENDOR_CODE.parent().prev().prepend(star.clone());

                        ddlSITE_ID.attr('disabled', false);
                        ddlSITE_ID.parent().prev().prepend(star.clone());

                        trAccountList.show();
                        var gridAccountList = $('#gridAccountList');
                        gridAccountList.jqGrid('clearGridData');
                        gridAccountList.jqGrid('setGridParam', {
                            postData: {
                                A_CODE: escape($.trim(A_CODE.val()))
                            }
                        });
                        gridAccountList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
                        functionButtons.find('.ui-button-text:contains("Create Vendor Account")').parent().show();
                        functionButtons.find('.ui-button-text:contains("Reject")').parent().show();
                        functionButtons.find('.ui-button-text:contains("Close")').parent().show();
                        $this.dialog('option', 'height', 565);
                    }
                    break;
            }
            lbSTATUS_ID.text(Status);
            functionButtons.find('.ui-button-text:contains("Close Window")').parent().show();

            function getAVAInfo(NVA_ID) {
                $.ajax({
                    url: __WebAppPathPrefix + '/VMIProcess/GetAVAInfo',
                    data: {
                        NVA_ID: escape($.trim(NVA_ID))
                    },
                    type: "post",
                    dataType: 'json',
                    async: false,
                    success: function (data) {
                        if (data) {
                            COMPANY_NAME_EN.val(data[0].COMPANY_NAME_EN);
                            COMPANY_NAME_ZH.val(data[0].COMPANY_NAME_ZH);
                            EMAIL.val(data[0].EMAIL);
                            lbScanFile.text(data[0].FILE_NAME);
                            A_CODE.val(data[0].A_CODE);
                            VENDOR_ACCOUNT.val(data[0].VENDOR_ACCOUNT);
                            GROUP_NAME.val(data[0].GROUP_NAME);
                            REJECT_REASON.val(data[0].REJECT_REASON);
                            BU_VENDOR_CODE.val(data[0].BU_VENDOR_CODE);
                            var SITE_ID = data[0].SITE_ID;
                            ddlSITE_ID.find('OPTION').each(function () {
                                if ($(this).attr('value') == SITE_ID) {
                                    $(this).prop('selected', true);
                                }
                                else {
                                    $(this).prop('selected', false);
                                }
                            });
                            G_CODE.val(data[0].G_CODE);
                            lbCREATE_USER.text(data[0].NAME);
                            lbCREATE_DATE.text(data[0].CREATE_DATE);
                        }
                    },
                    error: function (xhr, textStatus, thrownError) {
                        $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                    },
                    complete: function (jqXHR, textStatus) {
                    }
                });
            }

            function isAVAAccess() {
                showFlag = false;
                $.ajax({
                    url: __WebAppPathPrefix + '/VMIProcess/isAVAAccess',
                    data: {
                        TEXT: escape($.trim($this.prop('STATUS_ID')))
                    },
                    type: "post",
                    dataType: 'text',
                    async: false, // if need page refresh, please remark this option
                    success: function (data) {
                        if (data == "True") {
                            showFlag = true;
                        }
                    },
                    error: function (xhr, textStatus, thrownError) {
                        $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                    },
                    complete: function (jqXHR, textStatus) {
                    }
                });

                return showFlag;
            }
        },
        close: function () {
            // Clear All Fields
            $this.find('#txtCOMPANY_NAME_EN').val('').attr('readonly', true);
            $this.find('#txtCOMPANY_NAME_ZH').val('').attr('readonly', true);
            $this.find('#txtEMAIL').val('').attr('readonly', true);
            $this.find('#lbScanFile').text('');
            $this.find('#fileupload_uploadAVA').hide();
            $.reInit_fileupalod('fileupload_uploadAVA');
            $this.find('#btnExportExcel').hide();
            $this.find('#txtA_CODE').val('').attr('readonly', true);
            $this.find('#txtVENDOR_ACCOUNT').val('').attr('readonly', true);
            $this.find('#txtGROUP_NAME').val('').attr('readonly', true);
            $this.find('.trRejectReason').hide();
            $this.find('#txtREJECT_REASON').val('').attr('readonly', true);
            $this.find('#txtBU_VENDOR_CODE').val('').attr('readonly', true);
            $($this.find('#ddlSITE_ID').find('OPTION')[0]).prop('selected', true);
            $this.find('#ddlSITE_ID').attr('disabled', true);
            $this.find('#txtG_CODE').val('').attr('readonly', true);
            $this.find('#lbCREATE_USER').text('');
            $this.find('#lbCREATE_DATE').text('');
            $this.find('#lbSTATUS_ID').text('');
            $this.find('#trAccountList').hide();
            $this.prop({ NVA_ID: '', STATUS_ID: '' });
            $this.dialog('option', 'height', _dialogAVAManageHeight);
            $this.find('.star').remove();
        },
        buttons: {
            'Submit': function () {
                var COMPANY_NAME_EN = $.trim($(this).find('#txtCOMPANY_NAME_EN').val());
                var COMPANY_NAME_ZH = $.trim($(this).find('#txtCOMPANY_NAME_ZH').val());
                var EMAIL = $.trim($(this).find('#txtEMAIL').val());
                var BU_VENDOR_CODE = $.trim($(this).find('#txtBU_VENDOR_CODE').val());
                var SITE_ID = $.trim($(this).find('#ddlSITE_ID OPTION:Selected').val());

                if ((COMPANY_NAME_EN != '' || COMPANY_NAME_ZH != '') && EMAIL != '' && BU_VENDOR_CODE != '' && SITE_ID != '') {
                    if (!validateEmail(EMAIL)) {
                        alert('Wrong Email format.');
                    }
                    else {
                        var existsAccount = '';
                        $.ajax({
                            url: __WebAppPathPrefix + '/VMIProcess/ExistVendorAccount',
                            data: {
                                BU_VENDOR_CODE: escape($.trim(BU_VENDOR_CODE)),
                                SITE_ID: escape($.trim(SITE_ID))
                            },
                            type: "post",
                            dataType: 'text',
                            async: false, // if need page refresh, please remark this option
                            success: function (data) {
                                if (data != '') {
                                    existsAccount = data;
                                }
                            },
                            error: function (xhr, textStatus, thrownError) {
                                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                            },
                            complete: function (jqXHR, textStatus) {
                            }
                        });

                        if (existsAccount != '') {
                            if (!confirm("此供應商已申請過VMI帳號(帳號: " + existsAccount + "), 應不需申請新的帳號而繼續使用該號即可, 請再次確認是否要提出申請?")) {
                                return;
                            }
                        }
                        
                        $.ajax({
                            url: __WebAppPathPrefix + '/VMIProcess/ProcessAVAItem',
                            data: {
                                NVA_ID: escape($.trim($this.prop('NVA_ID'))),
                                COMPANY_NAME_EN: escape($.trim(COMPANY_NAME_EN)),
                                COMPANY_NAME_ZH: escape($.trim(COMPANY_NAME_ZH)),
                                EMAIL: escape($.trim(EMAIL)),
                                BU_VENDOR_CODE: escape($.trim(BU_VENDOR_CODE)),
                                SITE_ID: escape($.trim(SITE_ID))
                            },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                alert(data);
                                if (data.indexOf('successfully') != -1) {
                                    $('#dialogAVAManage').dialog('close');
                                    $('#ddlStatus Option:Contains("New")').prop('selected', true);
                                    ReloadANAgridDataList();
                                }
                            },
                            error: function (xhr, textStatus, thrownError) {
                                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                            },
                            complete: function (jqXHR, textStatus) {
                            }
                        });
                    }
                }
                else {
                    alert('Please fill the required fields:\r\nCompany Name (English)\r\nCompany Name (Chinese)\r\nE-Mail\r\nBU Vendor Code\r\nRequired Organization');
                }
            },
            'Save': function () {
                var COMPANY_NAME_EN = $.trim($(this).find('#txtCOMPANY_NAME_EN').val());
                var COMPANY_NAME_ZH = $.trim($(this).find('#txtCOMPANY_NAME_ZH').val());
                var EMAIL = $.trim($(this).find('#txtEMAIL').val());
                var ReqData = '';
                var A_CODE = $.trim($(this).find('#txtA_CODE').val());
                var GROUP_NAME = $.trim($(this).find('#txtGROUP_NAME').val());
                var BU_VENDOR_CODE = $.trim($(this).find('#txtBU_VENDOR_CODE').val());
                var SITE_ID = $.trim($(this).find('#ddlSITE_ID OPTION:Selected').val());
                var G_CODE = $.trim($(this).find('#txtG_CODE').val());

                if ($.get_uploadedFileInfo('fileupload_uploadAVA').length != 0) {
                    ReqData = $.extend(ReqData, { "Spec": JSON.stringify($.get_uploadedFileInfo('fileupload_uploadAVA')) });
                    ReqData = $.extend(ReqData, { "SubFolder": _SubFolderForUploadExcel });
                }

                $.ajax({
                    url: __WebAppPathPrefix + '/VMIProcess/ProcessAVAItem',
                    data: {
                        AVAItem: {
                            NVA_ID: escape($.trim($this.prop('NVA_ID'))),
                            COMPANY_NAME_EN: escape($.trim(COMPANY_NAME_EN)),
                            COMPANY_NAME_ZH: escape($.trim(COMPANY_NAME_ZH)),
                            EMAIL: escape($.trim(EMAIL)),
                            A_CODE: escape($.trim(A_CODE)),
                            GROUP_NAME: escape($.trim(GROUP_NAME)),
                            BU_VENDOR_CODE: escape($.trim(BU_VENDOR_CODE)),
                            SITE_ID: escape($.trim(SITE_ID)),
                            G_CODE: escape($.trim(G_CODE)),
                            TEXT: escape($.trim($this.prop('STATUS_ID')))
                        },
                        FA: ReqData
                    },
                    type: "post",
                    dataType: 'text',
                    async: false,
                    success: function (data) {
                        alert(data);
                        if (data == 'Save successfully.') {
                            $('#dialogAVAManage').dialog('close');
                            ReloadANAgridDataList();
                        }
                    },
                    error: function (xhr, textStatus, thrownError) {
                        $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                    },
                    complete: function (jqXHR, textStatus) {
                    }
                });
            },
            'Create Vendor Account': function () {
                var COMPANY_NAME_EN = $.trim($(this).find('#txtCOMPANY_NAME_EN').val());
                var COMPANY_NAME_ZH = $.trim($(this).find('#txtCOMPANY_NAME_ZH').val());
                var EMAIL = $.trim($(this).find('#txtEMAIL').val());
                var A_CODE = $.trim($(this).find('#txtA_CODE').val());
                var VENDOR_ACCOUNT = $.trim($(this).find('#txtVENDOR_ACCOUNT').val());
                var BU_VENDOR_CODE = $.trim($(this).find('#txtBU_VENDOR_CODE').val());
                var SITE_ID = $.trim($(this).find('#ddlSITE_ID OPTION:Selected').val());

                $.ajax({
                    url: __WebAppPathPrefix + '/VMIProcess/CreateVendorAccount',
                    data: {
                        NVA_ID: escape($.trim($this.prop('NVA_ID'))),
                        COMPANY_NAME_EN: escape($.trim(COMPANY_NAME_EN)),
                        COMPANY_NAME_ZH: escape($.trim(COMPANY_NAME_ZH)),
                        EMAIL: escape($.trim(EMAIL)),
                        A_CODE: escape($.trim(A_CODE)),
                        VENDOR_ACCOUNT: escape($.trim(VENDOR_ACCOUNT)),
                        BU_VENDOR_CODE: escape($.trim(BU_VENDOR_CODE)),
                        SITE_ID: escape($.trim(SITE_ID))
                    },
                    type: "post",
                    dataType: 'text',
                    async: false,
                    success: function (data) {
                        alert(data);
                        if (data == 'Create Vendor Account successfully.') {
                            $('#dialogAVAManage').dialog('close');
                            ReloadANAgridDataList();
                        }
                    },
                    error: function (xhr, textStatus, thrownError) {
                        $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                    },
                    complete: function (jqXHR, textStatus) {
                    }
                });
            },
            'Reject': function () {
                var REJECT_REASON = $.trim($('#txtREJECT_REASON').val());
                var A_CODE = $.trim($('#txtA_CODE').val());
                var ReqData = '';

                if (confirm('Are you sure you want to reject this apply?')) {
                    if (REJECT_REASON != '') {
                        $.ajax({
                            url: __WebAppPathPrefix + '/VMIProcess/RejectAVAItem',
                            data: {
                                AVAItem: {
                                    NVA_ID: escape($.trim($this.prop('NVA_ID'))),
                                    TEXT: escape($.trim($this.prop('STATUS_ID'))),
                                    REJECT_REASON: escape($.trim(REJECT_REASON)),
                                    A_CODE: escape($.trim(A_CODE))
                                },
                                FA: ReqData
                            },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                alert(data);
                                if (data == 'Reject successfully.') {
                                    $('#dialogAVAManage').dialog('close');
                                    ReloadANAgridDataList();
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
                        alert('Reject reason is empty.');
                    }
                }
            },
            'Close': function () {
                var COMPANY_NAME_EN = $.trim($(this).find('#txtCOMPANY_NAME_EN').val());
                var COMPANY_NAME_ZH = $.trim($(this).find('#txtCOMPANY_NAME_ZH').val());
                var EMAIL = $.trim($(this).find('#txtEMAIL').val());
                var SITE_ID = $.trim($(this).find('#ddlSITE_ID OPTION:Selected').val());
                var VENDOR_ACCOUNT = $.trim($(this).find('#txtVENDOR_ACCOUNT').val());
                var ReqData = '';

                $.ajax({
                    url: __WebAppPathPrefix + '/VMIProcess/CloseAVAItem',
                    data: {
                        AVAItem: {
                            NVA_ID: escape($.trim($this.prop('NVA_ID'))),
                            TEXT: escape($.trim($this.prop('STATUS_ID'))),
                            COMPANY_NAME_EN: escape($.trim(COMPANY_NAME_EN)),
                            COMPANY_NAME_ZH: escape($.trim(COMPANY_NAME_ZH)),
                            EMAIL: escape($.trim(EMAIL)),
                            SITE_ID: escape($.trim(SITE_ID)),
                            VENDOR_ACCOUNT: escape($.trim(VENDOR_ACCOUNT))
                        },
                        FA: ReqData
                    },
                    type: "post",
                    dataType: 'text',
                    async: false,
                    success: function (data) {
                        alert(data);
                        if (data == 'Close successfully.') {
                            $('#dialogAVAManage').dialog('close');
                            ReloadANAgridDataList();
                        }
                    },
                    error: function (xhr, textStatus, thrownError) {
                        $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                    },
                    complete: function (jqXHR, textStatus) {
                    }
                });
            },
            'Close Window': function () {
                $this.dialog('close');
            }
        }
    });

    $('#btnExportExcel').button({
        label: "Export"
    });

    $('#btnExportExcel').click(function () {
        var dialogAVAManage = $('#dialogAVAManage')
        var NVA_ID = dialogAVAManage.prop('NVA_ID');
        var COMPANY_NAME_EN = $.trim(dialogAVAManage.find('#txtCOMPANY_NAME_EN').val());
        var COMPANY_NAME_ZH = $.trim(dialogAVAManage.find('#txtCOMPANY_NAME_ZH').val());
        var EMAIL = $.trim(dialogAVAManage.find('#txtEMAIL').val());
        var GROUP_NAME = $.trim(dialogAVAManage.find('#txtGROUP_NAME').val());
        var BU_VENDOR_CODE = $.trim(dialogAVAManage.find('#txtBU_VENDOR_CODE').val());
        var SITE_ID = $.trim(dialogAVAManage.find('#ddlSITE_ID OPTION:Selected').val());
        var SITE_NAME = $.trim(dialogAVAManage.find('#ddlSITE_ID OPTION:Selected').text());
        var NAME = dialogAVAManage.find('#lbCREATE_USER').text();
        var CREATE_DATE = dialogAVAManage.find('#lbCREATE_DATE').text();

        if ((COMPANY_NAME_EN != '' || COMPANY_NAME_ZH != '') && EMAIL != '' && BU_VENDOR_CODE != '' && SITE_NAME != '' && NAME != '' && CREATE_DATE != '') {
            if (!validateEmail(EMAIL)) {
                alert('Wrong Email format.');
            }
            else {
                $.ajax({
                    url: __WebAppPathPrefix + '/VMIProcess/ExportAVAForm',
                    data: {
                        NVA_ID: escape(NVA_ID),
                        COMPANY_NAME_EN: escape(COMPANY_NAME_EN),
                        COMPANY_NAME_ZH: escape(COMPANY_NAME_ZH),
                        EMAIL: escape(EMAIL),
                        GROUP_NAME: escape(GROUP_NAME),
                        BU_VENDOR_CODE: escape(BU_VENDOR_CODE),
                        SITE_ID: escape(SITE_ID),
                        SITE_NAME: escape(SITE_NAME),
                        NAME: escape(NAME),
                        CREATE_DATE: escape(CREATE_DATE)
                    },
                    type: "post",
                    dataType: 'json',
                    success: function (data) {
                        if (data.Result) {
                            if (data.FileKey != "") {
                                $("#dialogDownloadSplash_FileKey").val(data.FileKey);
                                $("#dialogDownloadSplash_FileName").val(data.FileName);

                                setTimeout(function () {
                                    $("#dialogDownloadSplash_Form").attr('action', __WebAppPathPrefix + '/VMIProcess/RetrieveFileByFileKey').submit();

                                    dialogAVAManage.dialog('close');
                                    dialogAVAManage.prop({ NVA_ID: NVA_ID, STATUS_ID: 'Downloaded' });
                                    dialogAVAManage.dialog('open');

                                    ReloadANAgridDataList();
                                }, 10);
                            }
                        }
                        else
                            alert("Export failure. Please contact administrator manager.");
                    },
                    error: function (xhr, textStatus, thrownError) {
                        $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                    }
                });
            }
        }
        else {
            alert('Please fill the required fields before export the form:\r\nCompany Name (English)\r\nCompany Name (Chinese)\r\nE-Mail\r\nBU Vendor Code\r\nRequired Organization');
        }
    });

    $('#lbScanFile').click(function () {
        NVA_ID = $('#dialogAVAManage').prop('NVA_ID');
        FILE_NAME = $(this).text();

        $.ajax({
            url: __WebAppPathPrefix + '/VMIProcess/DownloadAVAScanFile',
            data: {
                AVAItem: {
                    NVA_ID: escape(NVA_ID)
                },
                FILE_NAME: escape(FILE_NAME)
            },
            type: "post",
            dataType: 'json',
            success: function (data) {
                if (data.Result) {
                    if (data.FileKey != "") {
                        $("#dialogDownloadSplash_FileKey").val(data.FileKey);
                        $("#dialogDownloadSplash_FileName").val(data.FileName);

                        setTimeout(function () {
                            $("#dialogDownloadSplash_Form").attr('action', __WebAppPathPrefix + '/VMIProcess/RetrieveFileByFileKey').submit();
                        }, 10);
                    }
                }
                else
                    alert("Export failure. Please contact administrator manager.");
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            }
        });
    });
});

Number.prototype.padLeft = function (base, chr) {
    var len = (String(base || 10).length - String(this).length) + 1;
    return len > 0 ? new Array(len).join(chr || '0') + this : this;
}

function getCurrentDateTime() {
    var d = new Date,
        dformat = [d.getFullYear(),
            (d.getMonth() + 1).padLeft(),
            d.getDate().padLeft()].join('-') +
                    ' ' +
                  [d.getHours().padLeft(),
                    d.getMinutes().padLeft()/*,
                    d.getSeconds().padLeft()*/].join(':');
    return dformat;
}

function validateEmail(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}