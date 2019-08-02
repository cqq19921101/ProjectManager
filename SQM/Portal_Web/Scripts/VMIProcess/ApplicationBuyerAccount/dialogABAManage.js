var _dialogABAManageHeight = 400;

$(function () {
    $('#dialogABAManage').dialog({
        autoOpen: false,
        resizable: false,
        width: 670,
        height: _dialogABAManageHeight,
        modal: true,
        open: function (event, ui) {
            $this = $(this);
            $this.find('input[type="text"]').blur();
            var BUYER_NAME = $this.find('#txtBUYER_NAME'),
                btnbtnDiaReadInternalUserInfo = $this.find('#btnDiaReadInternalUserInfo'),
                lbBuyerName = $this.find('#lbBuyerName'),
                ddlSITE_ID = $this.find('#ddlSITE_ID'),
                fileupload_uploadABA = $this.find('#fileupload_uploadABA'),
                lbScanFile = $this.find('#lbScanFile'),
                lbCREATE_USER = $this.find('#lbCREATE_USER'),
                lbCREATE_DATE = $this.find('#lbCREATE_DATE'),
                lbSTATUS_ID = $this.find('#lbSTATUS_ID'),
                trRejectReason = $this.find('.trRejectReason'),
                REJECT_REASON = $this.find('#txtREJECT_REASON'),
                trReviewing = $this.find('.trReviewing'),
                btnGroup = $this.find('#btnGroup'),
                btnRoles = $this.find('#btnRoles');

            var functionButtons = $this.parent().find('.ui-dialog-buttonset button'),
                STATUS = $this.prop('STATUS'),
                NBA_ID = $this.prop('NBA_ID');

            functionButtons.each(function () {
                $(this).hide();
            });

            var star = $('<span/>').css({ 'color': 'red', 'font-weight': '900' }).text('*').addClass('star');

            switch (STATUS) {
                case 'New':
                    BUYER_NAME.attr('readonly', false);
                    BUYER_NAME.parent().prev().prepend(star.clone());
                    btnbtnDiaReadInternalUserInfo.show();

                    ddlSITE_ID.attr('disabled', false);
                    ddlSITE_ID.parent().prev().prepend(star.clone());

                    lbScanFile.parent().prev().prepend(star.clone());

                    functionButtons.find('.ui-button-text:contains("Save")').parent().show();
                    functionButtons.find('.ui-button-text:contains("Export Form")').parent().show();

                    fileupload_uploadABA.show();

                    if (NBA_ID != '' && NBA_ID != undefined) {
                        getABAInfo(NBA_ID);

                        functionButtons.find('.ui-button-text:contains("Submit")').parent().show();
                    }
                    else {
                        lbCREATE_DATE.text(getCurrentDateTime().toString());
                        lbCREATE_USER.text(_Requestor);
                    }

                    $this.dialog('option', 'height', 330);
                    break;
                case 'Reviewing':
                    getABAInfo(NBA_ID);

                    BUYER_NAME.hide();

                    fileupload_uploadABA.hide();

                    if (isABAAccess()) {
                        trRejectReason.show();
                        REJECT_REASON.attr('readonly', false);

                        trReviewing.show();
                        btnGroup.show();
                        btnRoles.show();

                        functionButtons.find('.ui-button-text:contains("Reject")').parent().show();
                        functionButtons.find('.ui-button-text:contains("Create Buyer Account")').parent().show();

                        $this.dialog('option', 'height', 320);
                    }
                    else {
                        $this.dialog('option', 'height', 230);
                    }

                    break;
                case 'Reject':
                    getABAInfo(NBA_ID);

                    BUYER_NAME.attr('readonly', false);
                    BUYER_NAME.parent().prev().prepend(star.clone());
                    btnbtnDiaReadInternalUserInfo.show();

                    ddlSITE_ID.attr('disabled', false);
                    ddlSITE_ID.parent().prev().prepend(star.clone());

                    trRejectReason.show();

                    fileupload_uploadABA.show();

                    functionButtons.find('.ui-button-text:contains("Export Form")').parent().show();
                    functionButtons.find('.ui-button-text:contains("Save")').parent().show();
                    functionButtons.find('.ui-button-text:contains("Submit")').parent().show();

                    $this.dialog('option', 'height', 320);
                    break;
                case 'Close':
                    getABAInfo(NBA_ID);

                    BUYER_NAME.hide();

                    fileupload_uploadABA.hide();

                    trReviewing.show();

                    $this.dialog('option', 'height', 220);
                    break;
            }

            lbSTATUS_ID.text(STATUS);
            functionButtons.find('.ui-button-text:contains("Close Window")').parent().show();

            function getABAInfo(NVA_ID) {
                $.ajax({
                    url: __WebAppPathPrefix + '/VMIProcess/GetABAInfo',
                    data: {
                        NBA_ID: escape($.trim(NBA_ID))
                    },
                    type: "post",
                    dataType: 'json',
                    async: false,
                    success: function (data) {
                        if (data) {
                            lbBuyerName.text(data[0].BUYER_NAME);
                            lbScanFile.text(data[0].FILE_NAME);
                            var SITE_ID = data[0].SITE_ID;
                            ddlSITE_ID.find('OPTION').each(function () {
                                if ($(this).attr('value') == SITE_ID) {
                                    $(this).prop('selected', true);
                                }
                                else {
                                    $(this).prop('selected', false);
                                }
                            });
                            lbCREATE_USER.text(data[0].CREATE_USER_NAME);
                            lbCREATE_DATE.text(data[0].CREATE_TIME);
                            REJECT_REASON.val(data[0].REJECT_REASON);

                            $('#dialogABAManage #btnGroup').parent().find('span.group').remove();
                            $('#dialogABAManage #btnGroup').parent().append($('<span/>').text(data[0].Group.GroupName).addClass('group').attr('group_id', data[0].Group.GroupID).css('text-decoration', 'underline'));

                            $('#dialogABAManage #btnRoles').parent().find('span.roles').remove();
                            for (var i in data[0].Roles) {
                                $('#dialogABAManage #btnRoles').parent().append($('<span/>').text(data[0].Roles[i].RoleName).addClass('roles').attr('RoleGUID', data[0].Roles[i].RoleGUID).css('text-decoration', 'underline')).append('  ');
                            }
                        }
                    },
                    error: function (xhr, textStatus, thrownError) {
                        $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                    },
                    complete: function (jqXHR, textStatus) {
                    }
                });
            }

            function isABAAccess() {
                showFlag = false;
                $.ajax({
                    url: __WebAppPathPrefix + '/VMIProcess/isABAAccess',
                    data: {
                        TEXT: escape($.trim($this.prop('STATUS')))
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
            $this.find('#txtBUYER_NAME').val('').attr('readonly', true);
            $this.find('#btnDiaReadInternalUserInfo').hide();
            $this.find('#lbBuyerName').text('');
            $this.find('#ddlSITE_ID').attr('disabled', true);
            $($this.find('#ddlSITE_ID').find('OPTION')[0]).prop('selected', true);
            //$this.find('#fileupload_uploadABA').hide();
            $.reInit_fileupalod('fileupload_uploadABA');
            $this.find('#lbScanFile').text('');
            $this.find('#lbCREATE_USER').text('');
            $this.find('#lbCREATE_DATE').text('');
            $this.find('#lbSTATUS_ID').text('');
            $this.find('.trRejectReason').hide();
            $this.find('#txtREJECT_REASON').val('').attr('readonly', true);
            $this.prop({ NBA_ID: '', STATUS_ID: '' });
            $this.dialog('option', 'height', _dialogABAManageHeight);
            $this.find('.star').remove();
            $('#btnGroup').hide();
            $('#btnRoles').hide();
            $('#dialogABAManage #btnGroup').parent().find('span.group').remove();
            $('#dialogABAManage #btnRoles').parent().find('span.roles').remove();
            $this.find('.trReviewing').hide();
            $this.find('#txtBUYER_NAME').show();
        },
        buttons: {
            'Export Form': function () {
                var dialogABAManage = $('#dialogABAManage')
                var BUYER_NAME = $.trim(dialogABAManage.find('#lbBuyerName').text());
                var SITE_ID = $.trim(dialogABAManage.find('#ddlSITE_ID OPTION:Selected').val());

                if (BUYER_NAME != '' && SITE_ID != '') {
                    $.ajax({
                        url: __WebAppPathPrefix + '/VMIProcess/ExportABAForm',
                        data: {
                            BUYER_NAME: escape(BUYER_NAME),
                            SITE_ID: escape(SITE_ID)
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
                }
                else {
                    alert('Please fill the required fields before exporting the form:\n[Buyer Name]\n[Site]')
                }
            },
            'Save': function () {
                var NBA_ID = $.trim($(this).prop('NBA_ID'));
                var BUYER_NAME = $.trim($(this).find('#lbBuyerName').text());
                var SITE_ID = $.trim($(this).find('#ddlSITE_ID OPTION:Selected').val());
                var ReqData = '';

                if ($.get_uploadedFileInfo('fileupload_uploadABA').length != 0) {
                    ReqData = $.extend(ReqData, { "Spec": JSON.stringify($.get_uploadedFileInfo('fileupload_uploadABA')) });
                    ReqData = $.extend(ReqData, { "SubFolder": _SubFolderForUploadExcel });
                }

                if (BUYER_NAME != '' && SITE_ID != '') {
                    $.ajax({
                        url: __WebAppPathPrefix + '/VMIProcess/SaveABAItem',
                        data: {
                            ABAItem: {
                                NBA_ID: escape($.trim(NBA_ID)),
                                BUYER_NAME: escape($.trim(BUYER_NAME)),
                                SITE_ID: escape($.trim(SITE_ID))
                            },
                            FA: ReqData
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            alert(data);
                            if (data == 'Save successfully.') {
                                $('#dialogABAManage').dialog('close');
                                ReloadABAgridDataList();
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
                    alert('Buyer Name and Site is empty.');
                }
            },
            'Submit': function () {
                var NBA_ID = $(this).prop('NBA_ID');
                var BUYER_NAME = $.trim($(this).find('#lbBuyerName').text());
                var SITE_ID = $.trim($(this).find('#ddlSITE_ID OPTION:Selected').val());
                var CREATE_USER_NAME = $.trim($(this).find('#lbCREATE_USER').text());

                var ReqData = '';

                if ($.get_uploadedFileInfo('fileupload_uploadABA').length != 0) {
                    ReqData = $.extend(ReqData, { "Spec": JSON.stringify($.get_uploadedFileInfo('fileupload_uploadABA')) });
                    ReqData = $.extend(ReqData, { "SubFolder": _SubFolderForUploadExcel });
                }

                if (BUYER_NAME != '' && SITE_ID != '') {
                    $.ajax({
                        url: __WebAppPathPrefix + '/VMIProcess/SubmitABAItem',
                        data: {
                            ABAItem: {
                                NBA_ID: escape($.trim(NBA_ID)),
                                BUYER_NAME: escape($.trim(BUYER_NAME)),
                                SITE_ID: escape($.trim(SITE_ID)),
                                CREATE_USER_NAME: escape($.trim(CREATE_USER_NAME))
                            },
                            FA: ReqData
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            alert(data);
                            if (data.indexOf('successfully') != -1) {
                                $('#dialogABAManage').dialog('close');
                                ReloadABAgridDataList();
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
                    alert('Please fill required fields before submitting form.\nBuyer Name\nSite');
                }
            },
            'Reject': function () {
                var REJECT_REASON = $.trim($('#txtREJECT_REASON').val());
                var BUYER_NAME = $.trim($('#lbBuyerName').text());
                var CREATE_USER_NAME = $.trim($('#lbCREATE_USER').text());

                if (confirm('Are you sure you want to reject this apply?')) {
                    if (REJECT_REASON != '') {
                        $.ajax({
                            url: __WebAppPathPrefix + '/VMIProcess/RejectABAItem',
                            data: {
                                NBA_ID: escape($.trim($this.prop('NBA_ID'))),
                                REJECT_REASON: escape($.trim(REJECT_REASON)),
                                CREATE_USER_NAME: escape($.trim(CREATE_USER_NAME)),
                                BUYER_NAME: escape($.trim(BUYER_NAME))
                            },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                alert(data);
                                if (data == 'Reject successfully.') {
                                    $('#dialogABAManage').dialog('close');
                                    ReloadABAgridDataList();
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
            'Create Buyer Account': function () {
                var NBA_ID = $.trim($(this).prop('NBA_ID'));
                var GROUP_ID = $('#btnGroup').parent().find('span.group').attr('group_id');
                var GROUP_NAME = $('#btnGroup').parent().find('span.group').text();
                var Roles = [];

                $('#btnRoles').parent().find('span.roles').each(function () {
                    Roles.push({ RoleGUID: $(this).attr('roleGUID'), RoleName: $(this).text() });
                });

                if (GROUP_ID == undefined || GROUP_ID == '') {
                    alert('Group is empty.');
                }
                else if (Roles.length == 0) {
                    alert('Roles is empty.');
                }
                else {
                    $.ajax({
                        url: __WebAppPathPrefix + '/VMIProcess/CreateBuyerAccount',
                        data: {
                            NBA_ID: escape($.trim(NBA_ID)),
                            Group: { GroupID: GROUP_ID, GroupName: GROUP_NAME },
                            Roles: Roles
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            alert(data);
                            if (data == 'Create buyer account successfully.') {
                                $('#dialogABAManage').dialog('close');
                                ReloadABAgridDataList();
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
        }
    });

    $('#btnDiaReadInternalUserInfo').button({
        label: "Import"
    });

    $('#btnExportExcel').button({
        label: "Export"
    });

    $('#btnGroup').button({
        label: "Add"
    });

    $('#btnRoles').button({
        label: "Add"
    });

    $('#btnGroup').click(function () {
        $('#dialogGroup').dialog('open');
    });

    $('#btnRoles').click(function () {
        $('#dialogRoles').dialog('open');
    });

    $('#btnExportExcel').click(function () {
        var dialogABAManage = $('#dialogABAManage')
        var BUYER_NAME = $.trim(dialogABAManage.find('#lbBuyerName').text());
        var SITE_ID = $.trim(dialogABAManage.find('#ddlSITE_ID OPTION:Selected').val());

        if (BUYER_NAME != '' && SITE_ID != '') {
            $.ajax({
                url: __WebAppPathPrefix + '/VMIProcess/ExportABAForm',
                data: {
                    BUYER_NAME: escape(BUYER_NAME),
                    SITE_ID: escape(SITE_ID)
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
        }
        else {
            alert('Please fill the required fields before exporting the form:\nBuyer Name\nSite')
        }
    });

    $('#lbScanFile').click(function () {
        NBA_ID = $('#dialogABAManage').prop('NBA_ID');
        FILE_NAME = $(this).text();

        $.ajax({
            url: __WebAppPathPrefix + '/VMIProcess/DownloadABAScanFile',
            data: {
                NBA_ID: escape(NBA_ID),
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