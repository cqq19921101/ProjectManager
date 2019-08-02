$(function () {
    $('#dialogVendorAccountInfo').dialog({
        autoOpen: false,
        resizable: false,
        modal: true,
        open: function (event, ui) {
            var functionButtons = $(this).parent().find('.ui-dialog-buttonset button');

            functionButtons.each(function () {
                $(this).hide();
            });

            var action = $(this).prop('action');

            switch (action) {
                case 'query':
                    $('#tbQuery').show();

                    $('#lblCurrentEmail').text($(this).prop('currentEmail'));

                    enableButton(functionButtons, 'Change');
                    enableButton(functionButtons, 'Close');

                    $(this).dialog('option', 'height', 140);
                    $(this).dialog('option', 'width', 290);
                    break;
                case 'change':
                    $('#tbChange').show();

                    $('#txtNewEmail').val($(this).prop('currentEmail'));

                    enableButton(functionButtons, 'Request E-mail Change');

                    $(this).dialog('option', 'height', 228);
                    $(this).dialog('option', 'width', 388);
                    break;
                case 'review':
                    $('#tbApplyInfo').show();

                    var ID = escape($.trim($(this).prop('ID')));
                    $.ajax({
                        url: __WebAppPathPrefix + '/VMIProcess/ReviewVendorEmailApplyInfo',
                        data: {
                            ID: ID
                        },
                        type: "post",
                        dataType: 'json',
                        async: false,
                        success: function (data) {
                            if (data != '') {
                                $('#dialogVendorAccountInfo').prop('ID', data.ID);
                                $('#dialogVendorAccountInfo').prop('USER_GUID', data.USER_GUID);

                                $('#lblApplicant').text(data.APPLICANT);
                                $('#lblAccount').text(data.ACCOUNT);
                                $('#lblName').text(data.NAME);
                                $('#lblCompanyName').text(data.COMPANY_NAME);
                                $('#lblQQ').text(data.QQ);
                                $('#lblLastLoginDate').text(data.LAST_LOGIN_DATE);
                                $('#lblLastPasswordChangeDate').text(data.LAST_PASSWORD_CHANGE_DATE);
                                $('#cbAccountLockUp').prop('checked', data.IS_LOCKED_OUT);
                                $('#lblApplyInfoCurrentEmail').text(data.PREV_EMAIL);
                                $('#txtApplyInfoNewEmail').val(data.NEW_EMAIL);
                                $('#taApplyInfoReason').val($.parseHTML(data.REASON)[0].data);
                                $('#lblApplicationDate').text(data.CREATE_DATE);
                                $('#taRejectReason').val(data.REJECT_REASON == '' ? '' : $.parseHTML(data.REJECT_REASON)[0].data);

                                if (data.STATUS_ID == "0") { //NEW
                                    enableButton(functionButtons, 'Approve');
                                    enableButton(functionButtons, 'Reject');
                                    $('#dialogVendorAccountInfo').dialog('option', 'height', 470);
                                }
                                else { //REJECT CLOSE
                                    $('#txtApplyInfoNewEmail').attr('disabled', true);
                                    $('#taRejectReason').attr('disabled', true);
                                    $('#dialogVendorAccountInfo').dialog('option', 'height', 430);
                                }
                                $('#taApplyInfoReason').attr('disabled', true);
                            }
                            else {
                                alert("View application info failure. Please contact administrator manager, thanks.")
                            }
                        },
                        error: function (xhr, textStatus, thrownError) {
                            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                        },
                        complete: function (jqXHR, textStatus) {
                        }
                    });

                    $(this).dialog('option', 'width', 500);
                    break;
            }
        },
        close: function () {
            initElement($('#tbQuery'));
            initElement($('#tbChange'));
            initElement($('#tbApplyInfo'));

            if ($(this).prop('action') != 'query') {
                ReloadApplicationgridDataList();
            }

            $(this).prop('action', null);
            $(this).prop('USER_GUID', null);
        },
        buttons: {
            'Change': function () {
                $(this).dialog('close');
                $(this).prop('action', 'change');
                $(this).dialog('open');
            },
            'Request E-mail Change': function () {
                var Account = escape($.trim($(this).prop('account')));
                var BUVendorCode = escape($.trim($(this).prop('BUVendorCode')));
                var NewEmail = escape($.trim($('#txtNewEmail').val()));
                var Reason = escape($.trim($('#taReason').val()));

                if (NewEmail != '' && Reason != '') {
                    if (validateEmail(NewEmail)) {
                        if (confirm("Are you sure?\nThis request will pass to the Account Admin.\nPlease be reminded any result for it, thanks.")) {
                            $.ajax({
                                url: __WebAppPathPrefix + '/VMIProcess/RequestEmailChange',
                                data: {
                                    Account: Account,
                                    BUVendorCode: BUVendorCode,
                                    NewEmail: NewEmail,
                                    Reason: Reason
                                },
                                type: "post",
                                dataType: 'text',
                                async: false,
                                success: function (data) {
                                    switch (data) {
                                        case 'success':
                                            break;
                                        case 'failure':
                                            alert('Request E-mail Change failure. Please contact administrator manager, thanks.');
                                            break;
                                        case 'failure: Same E-mail address cannot be changed':
                                            alert(data);
                                            break;
                                    }
                                },
                                error: function (xhr, textStatus, thrownError) {
                                    $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                                },
                                complete: function (jqXHR, textStatus) {
                                }
                            });
                            $(this).dialog('close');
                        }
                    }
                    else {
                        alert('Wrong E-mail format. Please check it again, thanks.')
                    }
                }
                else {
                    alert('E-mail or Reason cannot be empty, thanks.');
                }
            },
            'Approve': function () {
                var ID = escape($.trim($(this).prop('ID')));
                var USER_GUID = escape($.trim($(this).prop('USER_GUID')));
                var isLocked = escape($.trim($('#cbAccountLockUp').prop('checked')));
                var NEW_EMAIL = escape($.trim($('#txtApplyInfoNewEmail').val()));

                if (!validateEmail(NEW_EMAIL)) {
                    alert('New E-mail wrong formant. Please check it again,thanks.');
                }
                else {
                    $.ajax({
                        url: __WebAppPathPrefix + '/VMIProcess/ApproveVendorEmailChange',
                        data: {
                            ID: ID,
                            USER_GUID: USER_GUID,
                            isLocked: isLocked,
                            NEW_EMAIL: NEW_EMAIL
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == 'failure') {
                                alert('Approve E-mail Change failure. Please contact administrator manager, thanks.');
                            }
                            else if (data == "success") {
                                $('#dialogVendorAccountInfo').dialog('close');
                            }
                        },
                        error: function (xhr, textStatus, thrownError) {
                            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                        },
                        complete: function (jqXHR, textStatus) {
                        }
                    });
                }
            },
            'Reject': function () {
                var ID = escape($.trim($(this).prop('ID')));
                var REJECT_REASON = escape($.trim($('#taRejectReason').val()));

                if (REJECT_REASON != '') {
                    $.ajax({
                        url: __WebAppPathPrefix + '/VMIProcess/RejectVendorEmailChange',
                        data: {
                            ID: ID,
                            REJECT_REASON: REJECT_REASON
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == 'failure') {
                                alert('Reject E-mail Change failure. Please contact administrator manager, thanks.');
                            }
                            else if (data == "success") {
                                $('#dialogVendorAccountInfo').dialog('close');
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
                    alert('Reject Reason cannot be empty. Please check it again, thanks.');
                }
            },
            'Close': function () {
                $(this).dialog('close');
            }
        }
    });

    $('#cbAccountLockUp').click(function () {
        var USER_GUID = escape($.trim($('#dialogVendorAccountInfo').prop('USER_GUID')));
        var isLocked = escape($.trim($(this).prop('checked')));

        $.ajax({
            url: __WebAppPathPrefix + '/VMIProcess/ChangeLockUser',
            data: {
                USER_GUID: USER_GUID,
                isLocked: isLocked
            },
            type: "post",
            dataType: 'text',
            async: false,
            success: function (data) {
                if (data == 'failure') {
                    alert('Request E-mail Change failure. Please contact administrator manager, thanks.');
                }
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
            }
        });
    });
});

function enableButton(btns, btnName) {
    btns.find('.ui-button-text').filter(function () {
        if ($(this).text() == btnName) {
            $(this).parent().show();
            $(this).parent().blur();
        }
    });
}

function validateEmail(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

function initElement(table) {
    $(table).hide();

    $(table).find('td').each(function () {
        var element = $(this).children();
        if (element.length != 0) {
            var tagName = $(element).prop('tagName').toUpperCase();
            switch (tagName) {
                case 'INPUT'://TEXT & CheckBox
                    var type = $(element).attr('type').toUpperCase();
                    if (type == 'TEXT') {
                        $(element).val('');
                    } else if (type == 'CHECKBOX') {
                        $(element).prop('checked', false);
                    }
                    break;
                case 'LABEL':
                    $(element).text('');
                    break;
                case 'TEXTAREA':
                    $(element).val('')
                    break;
                default:
                    break;
            }
            $(element).prop('disabled', null);
        }
    });
}