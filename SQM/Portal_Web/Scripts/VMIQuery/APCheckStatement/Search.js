$(function () {
    var diaPlant = $('#dialog_VMI_QueryPlantInfo');
    var diaSBUVendor = $('#dialog_VMI_QuerySBUVendor');

    $('#btnOpenQueryFromPlantDialog').click(function () {
        $(this).removeClass('ui-state-focus');
        if (!__DialogIsShownNow) {
            __DialogIsShownNow = true;
            __SelectorName = '#txtFromPlant';

            InitdialogPlant();
            ReloadDiaPlantCodegridDataList();

            diaPlant.show();
            diaPlant.dialog("open");
        }
    });

    $('#btnOpenQueryToPlantDialog').click(function () {
        $(this).removeClass('ui-state-focus');
        if (!__DialogIsShownNow) {
            __DialogIsShownNow = true;
            __SelectorName = '#txtToPlant';

            InitdialogPlant();
            ReloadDiaPlantCodegridDataList();

            diaPlant.show();
            diaPlant.dialog("open");
        }
    });

    $('#btnOpenQueryFromVendorCodeDialog').click(function () {
        $(this).removeClass('ui-state-focus');
        if (!__DialogIsShownNow) {
            __DialogIsShownNow = true;
            __SelectorName = '#txtFromVendorCode';

            InitdialogSBUVendor();
            ReloadDiaSBUVendorCodegridDataList();

            diaSBUVendor.show();
            diaSBUVendor.dialog("open");
        }
    });

    $('#btnOpenQueryToVendorCodeDialog').click(function () {
        $(this).removeClass('ui-state-focus');
        if (!__DialogIsShownNow) {
            __DialogIsShownNow = true;
            __SelectorName = '#txtToVendorCode';

            InitdialogSBUVendor();
            ReloadDiaSBUVendorCodegridDataList();

            diaSBUVendor.show();
            diaSBUVendor.dialog("open");
        }
    });

    $('#btnDownloadExcel').click(function () {
        $(this).removeClass('ui-state-focus');
        var dt = new Date();
        var fromVendorCode = $.trim($('#txtFromVendorCode').val());
        var toVendorCode = $.trim($('#txtToVendorCode').val());
        var fromCompanyCode = $.trim($('#txtFromCompanyCode').val());
        var toCompanyCode = $.trim($('#txtToCompanyCode').val());
        var fromPlant = $.trim($('#txtFromPlant').val());
        var toPlant = $.trim($('#txtToPlant').val());
        var fromMaterial = $.trim($('#txtFromMaterial').val());
        var toMaterial = $.trim($('#txtToMaterial').val());
        var description = $.trim($('#txtDescription').val());
        var fromPostDate = $.trim($('#fromPostDate').val());
        var toPostDate = $.trim($('#toPostDate').val());
        var fromGRSlip = $.trim($('#txtFromGRSlip').val());
        var toGRSlip = $.trim($('#txtToGRSlip').val());

        if (fromPostDate == '' || toPostDate == '') {
            alert('Please select Post Date(FM) and Post Date(TO) first.');
        }
        else if (dateDiff(fromPostDate, toPostDate) > 92) {
            alert('The interval of the Post Date is exceed 92 days, please reselect it under 92 days.');
        }
        else {
            $('#tdProcessTime').text($.datepicker.formatDate('yy/mm/dd', dt) + ' ' +
                dt.getHours() + ':' +
                ((dt.getMinutes() < 10) ? '0' : '') + dt.getMinutes());
            $('#tdFromVendorCode').text(fromVendorCode);
            $('#tdToVendorCode').text(toVendorCode);
            $('#tdFromCompanyCode').text(fromCompanyCode);
            $('#tdToCompanyCode').text(toCompanyCode);
            $('#tdFromPlant').text(fromPlant);
            $('#tdToPlant').text(toPlant);
            $('#tdFromMaterial').text(fromMaterial);
            $('#tdToMaterial').text(toMaterial);
            $('#tdDescription').text(description);
            $('#tdFromPostDate').text(fromPostDate);
            $('#tdToPostDate').text(toPostDate);
            $('#tdFromGRSlip').text(fromGRSlip);
            $('#tdToGRSlip').text(toGRSlip);

            $.ajax({
                url: __WebAppPathPrefix + '/VMIQuery/GetAPCheckExcelFileList',
                data: {
                    fromVendorCode: escape(fromVendorCode),
                    toVendorCode: escape(toVendorCode),
                    fromCompanyCode: escape(fromCompanyCode),
                    toCompanyCode: escape(toCompanyCode),
                    fromPlant: escape(fromPlant),
                    toPlant: escape(toPlant),
                    fromMaterial: escape(fromMaterial),
                    toMaterial: escape(toMaterial),
                    description: escape(description),
                    fromPostDate: escape(fromPostDate),
                    toPostDate: escape(toPostDate),
                    fromGRSlip: escape(fromGRSlip),
                    toGRSlip: escape(toGRSlip)
                },
                type: "post",
                dataType: 'json',
                async: false, // if need page refresh, please remark this option
                success: function (data) {
                    if (data.length == 0) {
                        alert('No Data.');
                    }
                    else if (data[0].vendor == 'The interval of the Post Date is exceed 92 days, please reselect it under 92 days.') {
                        alert(data[0].vendor);
                    }
                    else {
                        var filesLink = '';
                        $(data).each(function (idx, item) {
                            filesLink += '<a id="' + item.vendor + '_' + item.plant + '" class="downloadExcel" href="#">' + item.vendor + '_' + item.plant + '_Consign-GIDetail.xlsx' + '</a><br />';
                        });
                        $('#tdFiles').html(filesLink);

                        $('.downloadExcel').css({
                            'text-decoration': 'none',
                            'padding': '5px 0',
                            'display': 'inline-block'
                        });
                        $('.downloadExcel')
                            .mouseover(function () { $(this).css('text-decoration', 'underline'); })
                            .mouseleave(function () { $(this).css('text-decoration', 'none'); })
                            .click(function () {
                                var vendor = $(this).attr('id').split('_')[0];
                                var plant = $(this).attr('id').split('_')[1];
                                var fromVendorCode = vendor;
                                var toVendorCode = vendor;
                                var fromCompanyCode = $.trim($('#txtFromCompanyCode').val());
                                var toCompanyCode = $.trim($('#txtToCompanyCode').val());
                                var fromPlant = plant;
                                var toPlant = plant;
                                var fromMaterial = $.trim($('#txtFromMaterial').val());
                                var toMaterial = $.trim($('#txtToMaterial').val());
                                var description = $.trim($('#txtDescription').val());
                                var fromPostDate = $.trim($('#fromPostDate').val());
                                var toPostDate = $.trim($('#toPostDate').val());
                                var fromGRSlip = $.trim($('#txtFromGRSlip').val());
                                var toGRSlip = $.trim($('#txtToGRSlip').val());

                                $.ajax({
                                    url: __WebAppPathPrefix + '/VMIQuery/APCheckExportExcel',
                                    data: {
                                        fromVendorCode: escape(fromVendorCode),
                                        toVendorCode: escape(toVendorCode),
                                        fromCompanyCode: escape(fromCompanyCode),
                                        toCompanyCode: escape(toCompanyCode),
                                        fromPlant: escape(fromPlant),
                                        toPlant: escape(toPlant),
                                        fromMaterial: escape(fromMaterial),
                                        toMaterial: escape(toMaterial),
                                        description: escape(description),
                                        fromPostDate: escape(fromPostDate),
                                        toPostDate: escape(toPostDate),
                                        fromGRSlip: escape(fromGRSlip),
                                        toGRSlip: escape(toGRSlip)
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
                                        else
                                            alert("Export failure. Please contact administrator manager.");
                                    },
                                    error: function (xhr, textStatus, thrownError) {
                                        $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                                    }
                                });
                            });

                        $('#dialogAPCheckStatementResult').dialog('open');
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

    $('#btnDownloadPDF').click(function () {
        $(this).removeClass('ui-state-focus');

        var downloadType = $('input[name="PDFType"]:checked').val();
        var dt = new Date();
        var fromVendorCode = $.trim($('#txtFromVendorCode').val());
        var toVendorCode = $.trim($('#txtToVendorCode').val());
        var fromCompanyCode = $.trim($('#txtFromCompanyCode').val());
        var toCompanyCode = $.trim($('#txtToCompanyCode').val());
        var fromPlant = $.trim($('#txtFromPlant').val());
        var toPlant = $.trim($('#txtToPlant').val());
        var fromMaterial = $.trim($('#txtFromMaterial').val());
        var toMaterial = $.trim($('#txtToMaterial').val());
        var description = $.trim($('#txtDescription').val());
        var fromPostDate = $.trim($('#fromPostDate').val());
        var toPostDate = $.trim($('#toPostDate').val());
        var fromGRSlip = $.trim($('#txtFromGRSlip').val());
        var toGRSlip = $.trim($('#txtToGRSlip').val());

        if (fromPostDate == '' || toPostDate == '') {
            alert('Please select Post Date(FM) and Post Date(TO) first.');
        }
        else if (dateDiff(fromPostDate, toPostDate) > 92) {
            alert('The interval of the Post Date is exceed 92 days, please reselect it under 92 days.');
        }
        else {
            $('#tdProcessTime').text($.datepicker.formatDate('yy/mm/dd', dt) + ' ' +
                dt.getHours() + ':' +
                ((dt.getMinutes() < 10) ? '0' : '') + dt.getMinutes());
            $('#tdFromVendorCode').text(fromVendorCode);
            $('#tdToVendorCode').text(toVendorCode);
            $('#tdFromCompanyCode').text(fromCompanyCode);
            $('#tdToCompanyCode').text(toCompanyCode);
            $('#tdFromPlant').text(fromPlant);
            $('#tdToPlant').text(toPlant);
            $('#tdFromMaterial').text(fromMaterial);
            $('#tdToMaterial').text(toMaterial);
            $('#tdDescription').text(description);
            $('#tdFromPostDate').text(fromPostDate);
            $('#tdToPostDate').text(toPostDate);
            $('#tdFromGRSlip').text(fromGRSlip);
            $('#tdToGRSlip').text(toGRSlip);

            $.ajax({
                url: __WebAppPathPrefix + '/VMIQuery/GetAPCheckPDFFileList',
                data: {
                    fromVendorCode: escape(fromVendorCode),
                    toVendorCode: escape(toVendorCode),
                    fromCompanyCode: escape(fromCompanyCode),
                    toCompanyCode: escape(toCompanyCode),
                    fromPlant: escape(fromPlant),
                    toPlant: escape(toPlant),
                    fromMaterial: escape(fromMaterial),
                    toMaterial: escape(toMaterial),
                    description: escape(description),
                    fromPostDate: escape(fromPostDate),
                    toPostDate: escape(toPostDate),
                    fromGRSlip: escape(fromGRSlip),
                    toGRSlip: escape(toGRSlip),
                    downloadType: escape(downloadType)
                },
                type: "post",
                dataType: 'json',
                async: false, // if need page refresh, please remark this option
                success: function (data) {
                    if (data.length == 0) {
                        alert('No Data.');
                    }
                    else if (data[0] == 'The interval of the Post Date is exceed 92 days, please reselect it under 92 days.') {
                        alert(data[0]);
                    }
                    else {
                        var filesLink = '';
                        if (downloadType == 'O') {
                            filesLink = '<a id="OneFile" class="downloadPDF" href="#">Consign-GIPDF.pdf</a><br />';
                        }
                        else {
                            $(data).each(function (idx, value) {
                                filesLink += '<a id="' + value + '" class="downloadPDF" href="#">' + value + '_Consign-GIPDF.pdf' + '</a><br />';
                            });
                        }
                        $('#tdFiles').html(filesLink);

                        $('.downloadPDF').css({
                            'text-decoration': 'none',
                            'padding': '5px 0',
                            'display': 'inline-block'
                        });
                        $('.downloadPDF')
                            .mouseover(function () { $(this).css('text-decoration', 'underline'); })
                            .mouseleave(function () { $(this).css('text-decoration', 'none'); })
                            .click(function () {
                                var vendor = '', company = '', plant = '';
                                var arrInfo = $(this).attr('id').split('_');

                                switch (downloadType) {
                                    case 'O':
                                        break;
                                    case 'V':
                                        vendor = arrInfo[0];
                                        break;
                                    case 'VC':
                                        vendor = arrInfo[0];
                                        company = arrInfo[1];
                                        break;
                                    case 'VCP':
                                        vendor = arrInfo[0];
                                        company = arrInfo[1];
                                        plant = arrInfo[2];
                                        break;
                                }

                                var fromVendorCode = ((vendor == '') ? $.trim($('#txtFromVendorCode').val()) : vendor);
                                var toVendorCode = ((vendor == '') ? $.trim($('#txtToVendorCode').val()) : vendor);
                                var fromCompanyCode = ((company == '') ? $.trim($('#txtFromCompanyCode').val()) : company);
                                var toCompanyCode = ((company == '') ? $.trim($('#txtToCompanyCode').val()) : company);
                                var fromPlant = ((plant == '') ? $.trim($('#txtFromPlant').val()) : plant);
                                var toPlant = ((plant == '') ? $.trim($('#txtToPlant').val()) : plant);

                                var fromMaterial = $.trim($('#txtFromMaterial').val());
                                var toMaterial = $.trim($('#txtToMaterial').val());
                                var description = $.trim($('#txtDescription').val());
                                var fromPostDate = $.trim($('#fromPostDate').val());
                                var toPostDate = $.trim($('#toPostDate').val());
                                var fromGRSlip = $.trim($('#txtFromGRSlip').val());
                                var toGRSlip = $.trim($('#txtToGRSlip').val());

                                $.ajax({
                                    url: __WebAppPathPrefix + '/VMIQuery/APCheckExportPDF',
                                    data: {
                                        fromVendorCode: escape(fromVendorCode),
                                        toVendorCode: escape(toVendorCode),
                                        fromCompanyCode: escape(fromCompanyCode),
                                        toCompanyCode: escape(toCompanyCode),
                                        fromPlant: escape(fromPlant),
                                        toPlant: escape(toPlant),
                                        fromMaterial: escape(fromMaterial),
                                        toMaterial: escape(toMaterial),
                                        description: escape(description),
                                        fromPostDate: escape(fromPostDate),
                                        toPostDate: escape(toPostDate),
                                        fromGRSlip: escape(fromGRSlip),
                                        toGRSlip: escape(toGRSlip),
                                        downloadType: escape(downloadType)
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
                        $('#dialogAPCheckStatementResult').dialog('open');
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
});

function dateDiff(fromDate, toDate) {
    var date1 = new Date(fromDate);
    var date2 = new Date(toDate);
    var timeDiff = Math.abs(date2.getTime() - date1.getTime());
    var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));
    return diffDays;
}