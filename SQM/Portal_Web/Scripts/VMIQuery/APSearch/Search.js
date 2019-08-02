$(function () {
    var diaSBUVendor = $('#dialog_VMI_QuerySBUVendor');

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

    $('#btnAPSearch').click(function (event) {
        $(this).removeClass('ui-state-focus');

        var fromInvoiceDate = $.trim($('#fromInvoiceDate').val());
        var toInvoiceDate = $.trim($('#toInvoiceDate').val());

        var fromPayDate = $.trim($('#fromPayDate').val());
        var toPayDate = $.trim($('#toPayDate').val());

        var boolInvoiceDate = false;
        var boolPayDate = false;

        if (fromInvoiceDate != '' && toInvoiceDate != '') {
            boolInvoiceDate = true;
        }

        if (fromPayDate != '' && toPayDate != '') {
            boolPayDate = true;
        }

        if (boolInvoiceDate || boolPayDate) {
            if (boolInvoiceDate && dateDiff(fromInvoiceDate, toInvoiceDate) > 92) {
                alert('The interval of the Invoice Date is exceed 92 days, please reselect it under 92 days.');
                return;
            }

            if (boolPayDate && dateDiff(fromPayDate, toPayDate) > 92) {
                alert('The interval of the Pay Date is exceed 92 days, please reselect it under 92 days.');
                return;
            }

            $.ajax({
                url: __WebAppPathPrefix + '/VMIQuery/QueryAPSearchVendorList',
                data: {
                    InvoiceNo: escape($.trim($("#txtInvoiceNo").val())),
                    PONo: escape($.trim($("#txtPONo").val())),
                    fromInvoiceDate: escape($.trim($("#fromInvoiceDate").val())),
                    toInvoiceDate: escape($.trim($("#toInvoiceDate").val())),
                    fromPayDate: escape($.trim($("#fromPayDate").val())),
                    toPayDate: escape($.trim($("#toPayDate").val())),
                    fromDueDate: escape($.trim($("#fromDueDate").val())),
                    toDueDate: escape($.trim($("#toDueDate").val())),
                    CompanyCode: escape($.trim($("#txtCompanyCode").val())),
                    VendorCode: escape($.trim($("#txtVendorCode").val())),
                    PayStatus: escape($.trim($("#ddlPayStatus").val())),
                    OrderBy: escape($.trim($("#ddlOrderBy").val()))
                },
                type: "post",
                dataType: 'json',
                //async: false, // if need page refresh, please remark this option
                beforeSend: function () {
                    $("#dialogDownloadSplash").dialog({
                        title: 'Processing Notify',
                        width: 'auto',
                        height: 'auto',
                        open: function (event, ui) {
                            $(this).parent().find('.ui-dialog-titlebar-close').hide();
                            $(this).parent().find('.ui-dialog-buttonpane').hide();
                            $("#lbDiaDownloadMsg").html('Searching...</br></br>Please wait for the operation a moment...');
                        }
                    }).dialog("open");
                },
                success: function (data) {
                    $("#dialogDownloadSplash").dialog('close');
                    $('#ddlVendor option').remove();

                    if (data.length == 0) {
                        alert('No Data');
                    }
                    else if (data[0] == "The interval of the Pay Date is exceed 92 days, please reselect it under 92 days.") {
                        alert(data[0]);
                    }
                    else if (data[0] == "The interval of the Invoice Date is exceed 92 days, please reselect it under 92 days.") {
                        alert(data[0]);
                    }
                    else {
                        var options = '';
                        $(data).each(function (index, value) {
                            options += '<option value="' + value + '">' + value + '</option>';
                        });
                        $('#ddlVendor').html(options);

                        $('#listAPSearch').jqGrid('clearGridData');
                        $('#listAPSearch').jqGrid('setGridParam', {
                            postData: {
                                InvoiceNo: escape($.trim($("#txtInvoiceNo").val())),
                                PONo: escape($.trim($("#txtPONo").val())),
                                fromInvoiceDate: escape($.trim($("#fromInvoiceDate").val())),
                                toInvoiceDate: escape($.trim($("#toInvoiceDate").val())),
                                fromPayDate: escape($.trim($("#fromPayDate").val())),
                                toPayDate: escape($.trim($("#toPayDate").val())),
                                fromDueDate: escape($.trim($("#fromDueDate").val())),
                                toDueDate: escape($.trim($("#toDueDate").val())),
                                CompanyCode: escape($.trim($("#txtCompanyCode").val())),
                                VendorCode: escape($.trim($("#ddlVendor option:selected").val())),
                                PayStatus: escape($.trim($("#ddlPayStatus").val())),
                                OrderBy: escape($.trim($("#ddlOrderBy").val()))
                            }
                        });
                        $('#listAPSearch').jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');

                        $('#dialogAPSearch').dialog('open');
                    }
                },
                error: function (xhr, textStatus, thrownError) {
                    $("#dialogDownloadSplash").dialog('close');
                    $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                },
                complete: function (jqXHR, textStatus) {
                }
            });
        }
        else {
            alert('Please select at least one set of Invoice Date(FM-TO) or Pay Date(FM-TO) first.');
        }
    });

    $('#btnExportAllExcel').click(function () {
        $(this).removeClass('ui-state-focus');
        $.ajax({
            url: __WebAppPathPrefix + '/VMIQuery/APSearchExportAllExcel',
            data: {
                InvoiceNo: escape($.trim($("#txtInvoiceNo").val())),
                PONo: escape($.trim($("#txtPONo").val())),
                fromInvoiceDate: escape($.trim($("#fromInvoiceDate").val())),
                toInvoiceDate: escape($.trim($("#toInvoiceDate").val())),
                fromPayDate: escape($.trim($("#fromPayDate").val())),
                toPayDate: escape($.trim($("#toPayDate").val())),
                fromDueDate: escape($.trim($("#fromDueDate").val())),
                toDueDate: escape($.trim($("#toDueDate").val())),
                CompanyCode: escape($.trim($("#txtCompanyCode").val())),
                VendorCode: escape($.trim($("#txtVendorCode").val())),
                PayStatus: escape($.trim($("#ddlPayStatus").val())),
                OrderBy: escape($.trim($("#ddlOrderBy").val()))
            },
            type: "post",
            dataType: 'json',
            //async: false,
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
                            $("#dialogDownloadSplash_Form").attr('action', __WebAppPathPrefix + '/VMIQuery/RetrieveFileByFileKey').submit();
                            $("#dialogDownloadSplash").dialog("close");
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

    $('#btnExportAllPDF').click(function () {
        $(this).removeClass('ui-state-focus');
        $.ajax({
            url: __WebAppPathPrefix + '/VMIQuery/APSearchExportAllPDF',
            data: {
                InvoiceNo: escape($.trim($("#txtInvoiceNo").val())),
                PONo: escape($.trim($("#txtPONo").val())),
                fromInvoiceDate: escape($.trim($("#fromInvoiceDate").val())),
                toInvoiceDate: escape($.trim($("#toInvoiceDate").val())),
                fromPayDate: escape($.trim($("#fromPayDate").val())),
                toPayDate: escape($.trim($("#toPayDate").val())),
                fromDueDate: escape($.trim($("#fromDueDate").val())),
                toDueDate: escape($.trim($("#toDueDate").val())),
                CompanyCode: escape($.trim($("#txtCompanyCode").val())),
                VendorCode: escape($.trim($("#txtVendorCode").val())),
                PayStatus: escape($.trim($("#ddlPayStatus").val())),
                OrderBy: escape($.trim($("#ddlOrderBy").val()))
            },
            type: "post",
            dataType: 'json',
            //async: false,
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
                            $("#dialogDownloadSplash_Form").attr('action', __WebAppPathPrefix + '/VMIQuery/RetrieveFileByFileKey').submit();
                            $("#dialogDownloadSplash").dialog("close");
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

    $('#btnExportExcel').click(function () {
        $(this).removeClass('ui-state-focus');
        $.ajax({
            url: __WebAppPathPrefix + '/VMIQuery/APSearchExportExcel',
            data: {
                InvoiceNo: escape($.trim($("#txtInvoiceNo").val())),
                PONo: escape($.trim($("#txtPONo").val())),
                fromInvoiceDate: escape($.trim($("#fromInvoiceDate").val())),
                toInvoiceDate: escape($.trim($("#toInvoiceDate").val())),
                fromPayDate: escape($.trim($("#fromPayDate").val())),
                toPayDate: escape($.trim($("#toPayDate").val())),
                fromDueDate: escape($.trim($("#fromDueDate").val())),
                toDueDate: escape($.trim($("#toDueDate").val())),
                CompanyCode: escape($.trim($("#txtCompanyCode").val())),
                VendorCode: escape($.trim($("#ddlVendor option:selected").val())),
                PayStatus: escape($.trim($("#ddlPayStatus").val())),
                OrderBy: escape($.trim($("#ddlOrderBy").val()))
            },
            type: "post",
            dataType: 'json',
            //async: false,
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
                            $("#dialogDownloadSplash_Form").attr('action', __WebAppPathPrefix + '/VMIQuery/RetrieveFileByFileKey').submit();
                            $("#dialogDownloadSplash").dialog("close");
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

function dateDiff(fromDate, toDate) {
    var date1 = new Date(fromDate);
    var date2 = new Date(toDate);
    var timeDiff = Math.abs(date2.getTime() - date1.getTime());
    var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));
    return diffDays;
}