var ImportListDNlist = [];

$(function () {
    $('#dialogImportListForm').dialog({
        autoOpen: false,
        resizable: false,
        height: 560,
        width: 'auto',
        modal: true,
        open: function () {
            $('button.ui-state-focus').removeClass('ui-state-focus');
            initDialogImportListForm($(this).prop('IMPORT_LIST_NUM'));
        },
        close: function () {
            if ($(this).prop('IMPORT_LIST_NUM') != undefined && $(this).prop('IMPORT_LIST_NUM') != null) {
                if (confirm('Do you want to save Import List before closing this dialog?')) {
                    saveImportList();
                }
            }

            $(this).prop('IMPORT_LIST_NUM', null);
            $('#ddlForwarderCompanyName').children().remove();
            $('#ddlPlant').children().remove();
            $('#gridImportList').jqGrid('clearGridData');
            $('#gridImportList').jqGrid('setGridParam', {
                postData: {
                    IMPORT_LIST_NUM_FM: escape($.trim($('#txtImportListNoFM').val())),
                    IMPORT_LIST_NUM_TO: escape($.trim($('#txtImportListNoTo').val())),
                    IDN_NUM_FM: escape($.trim($('#txtDNNoFM').val())),
                    IDN_NUM_TO: escape($.trim($('#txtDNNoTO').val()))
                }
            });
            $('#gridImportList').jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
            $('#sImportListNum').text('');
            $('#txtDriverName').val('');
            $('#txtVehicleID').val('');
            $('#txtDriverTel').val('');
            $('#sCreateDate').text('');
            $('#txtReceiveAddress').val('');
            $('#txtReceiver').val('');
            $('#txtReceiverTel').val('');
            $('#txtPlanArrivalDate').val('');
            $('#txtPlanArrivalTime').val('');
            //$('#txtPlant').val('');
            $('#importListGridDataList').jqGrid('clearGridData');
            $('#cbIsClose').prop('checked', false);
        }
    });

    $('#btnSave').button({
        label: "Save",
        icons: { primary: 'ui-icon-disk' }
    });

    $('#btnAddDNinList').button({
        label: "Add DN in List",
        icons: { primary: 'ui-icon-plus' }
    });

    $('#btnDelete').button({
        label: "Delete",
        icons: { primary: 'ui-icon-minus' }
    });

    $('#btnDeleteAll').button({
        label: "Delete All",
        icons: { primary: 'ui-icon-minusthick' }
    });

    $('#btnNoticeAndRelease').button({
        label: "Notice & Release",
        icons: { primary: 'ui-icon-mail-closed' }
    });

    $('#btnPrintImportList').button({
        label: "Print Import List",
        icons: { primary: 'ui-icon-print' }
    });

    $("#txtPlanArrivalDate").datepicker({
        changeMonth: true,
        dateFormat: 'yy/mm/dd',
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            }
            catch (err) {
                $(this).val('');
            }
        }
    });

    //$('#txtPlanArrivalTime').inputmask("h:s", { "placeholder": "00:00" });
    $('#txtPlanArrivalTime').inputmask("hh:mm", {
        //placeholder: "00:00",
        insertMode: false,
        showMaskOnHover: false
    });

    $('#importListGridDataList').jqGrid({
        url: __WebAppPathPrefix + "/VMIProcess/GetImportListDetail",
        mtype: "POST",
        datatype: "local",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false
        },
        colNames: [
            "",
            "Item No.",
            "DN No.",
            "ASN_NUM",
            "Material",
            "Name",
            "Qty",
            "T. Net Wgt(KG)",
            "T. Gross Wgt(KG)",
            "T. Packing",
            "Pack Type"],
        colModel: [
            { name: 'CHECK', width: 25, align: "center", editoptions: { value: "True:False" }, editrules: { required: true }, formatter: "checkbox", formatoptions: { disabled: false }, editable: true, sortable: false },
            { name: "ITEM_NO", index: "ITEM_NO", width: 50, sortable: false, sorttype: "text" },
            { name: "IDN_NUM", index: "IDN_NUM", width: 85, sortable: false, sorttype: "text" },
            { name: "ASN_NUM", index: "ASN_NUM", width: 85, sortable: false, sorttype: "text", hidden: true },
            { name: "MATERIAL", index: "MATERIAL", width: 120, sortable: false, sorttype: "text" },
            { name: "ZPNAME2", index: "ZPNAME2", width: 120, sortable: false, sorttype: "text" },
            { name: "ASN_QTY", index: "ASN_QTY", width: 60, align: 'right', sortable: false, sorttype: "text" },
            { name: "TOT_NW", index: "TOT_NW", width: 90, align: 'right', sortable: false, sorttype: "text" },
            { name: "TOT_GW", index: "TOT_GW", width: 105, align: 'right', sortable: false, sorttype: "text" },
            { name: "TOT_SET", index: "TOT_SET", width: 80, align: 'right', sortable: false, sorttype: "text" },
            { name: "PACK_MATH", index: "PACK_MATH", width: 120, sortable: false, sorttype: "text" }],
        shrinkToFit: false,
        hoverrows: false,
        height: 252,
        rowNum: 10,
        viewrecords: true,
        loadonce: true,
        pager: '#importListGridListPager',
        beforeSelectRow: function (rowid, e) {
            var $self = $(this),
                iCol = $.jgrid.getCellIndex($(e.target).closest("td")[0]),
                cm = $self.jqGrid("getGridParam", "colModel"),
                localData = $self.jqGrid("getLocalRow", rowid);

            if (cm[iCol].name === "CHECK" && e.target.tagName.toUpperCase() === "INPUT") {
                localData.CHECK = $(e.target).is(":checked");
                var allRows = $(this).jqGrid('getDataIDs');
                var idx = ImportListDNlist.indexOf(localData.ASN_NUM);

                if (localData.CHECK) {
                    if (idx == -1) {
                        ImportListDNlist.push(localData.ASN_NUM);
                    }
                }
                else {
                    if (idx != -1) {
                        ImportListDNlist.splice(idx, 1);
                    }
                }

                $('#lblImportListDNlist').text(ImportListDNlist);

                for (var i in allRows) {
                    if (localData.ASN_NUM == $('#importListGridDataList').jqGrid('getCell', allRows[i], 'ASN_NUM')) {
                        $('#importListGridDataList').jqGrid('setCell', allRows[i], 'CHECK', localData.CHECK);
                    }
                }
            }

            return true;
        },
        gridComplete: function () {
            var allRows = $(this).jqGrid('getDataIDs');

            for (var i in allRows) {
                $('#importListGridDataList').jqGrid('setCell', allRows[i], 'CHECK', 'False');
            }

            for (var i in ImportListDNlist) {
                for (var j in allRows) {
                    if (ImportListDNlist[i] == $(this).jqGrid('getCell', allRows[j], 'ASN_NUM')) {
                        $('#importListGridDataList').jqGrid('setCell', allRows[j], 'CHECK', 'True');
                    }
                }
            }
        }
    });

    $('#importListGridDataList').jqGrid('navGrid', '#importListGridListPager', { edit: false, add: false, del: false, search: false, refresh: false });

    $('#btnQueryPlant').button({
        icons: { primary: 'ui-icon-search' }
    });

    $('#ddlPlant').change(function () {
        var PLANT = $(this).val();
        var dialogImportListForm = $('#dialogImportListForm');

        if (PLANT != '') {
            $.ajax({
                url: __WebAppPathPrefix + '/VMIProcess/GetPlantReceiveInfo',
                data: {
                    PLANT: escape($.trim(PLANT))
                },
                type: "post",
                dataType: 'json',
                async: false,
                success: function (data) {
                    dialogImportListForm.find('#txtReceiveAddress').val(data.RECEIVE_ADDR);
                    dialogImportListForm.find('#txtReceiver').val(data.RECEIVER);
                    dialogImportListForm.find('#txtReceiverTel').val(data.RECEIVER_TEL);
                },
                error: function (xhr, textStatus, thrownError) {
                    $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                },
                complete: function (jqXHR, textStatus) {
                }
            });
        } else {
            dialogImportListForm.find('#txtReceiveAddress').val('');
            dialogImportListForm.find('#txtReceiver').val('');
            dialogImportListForm.find('#txtReceiverTel').val('');
        }

    });
});

function initDialogImportListForm(IMPORT_LIST_NUM) {
    $('#lblImportListDNlist').text('');
    while (ImportListDNlist.length) { ImportListDNlist.pop(); }
    while (DNList.length) { DNList.pop(); }

    $.ajax({
        url: __WebAppPathPrefix + '/VMIProcess/GetAuthPlant',
        type: "post",
        dataType: 'json',
        async: false,
        success: function (data) {
            $('#ddlPlant').children().remove();

            for (var i in data) {
                $('#ddlPlant').append($('<option/>').attr('value', data[i]).text(data[i]));
            }
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });

    if (IMPORT_LIST_NUM != undefined && IMPORT_LIST_NUM != null) {
        $('#dialogImportListForm').prop('IMPORT_LIST_NUM', IMPORT_LIST_NUM);

        $.ajax({
            url: __WebAppPathPrefix + '/VMIProcess/GetForwarderCompanyNameList',
            data: {
                IMPORT_LIST_NUM: escape($.trim(IMPORT_LIST_NUM))
            },
            type: "post",
            dataType: 'json',
            async: false,
            success: function (data) {
                $('#ddlForwarderCompanyName').children().remove();

                for (var i in data) {
                    $('#ddlForwarderCompanyName').append($('<option/>').attr('value', data[i]).text(data[i].split('@@')[0] + '(' + data[i].split('@@')[1] + ')'));
                }
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
            }
        });

        $.ajax({
            url: __WebAppPathPrefix + '/VMIProcess/GetImportListHeader',
            data: {
                IMPORT_LIST_NUM: escape($.trim(IMPORT_LIST_NUM))
            },
            type: "post",
            dataType: 'json',
            async: false,
            success: function (data) {
                $('#sImportListNum').text(data.IMPORT_LIST_NUM);
                $('#txtDriverName').val(data.DRIVER_NAME);
                $('#txtVehicleID').val(data.VEHICLE_TYPE_ID);
                $('#txtDriverTel').val(data.DRIVER_TEL);
                $('#sCreateDate').text(data.CREATE_TIME);
                $('#txtReceiveAddress').val(data.RECEIVE_ADDR);
                $('#txtReceiver').val(data.RECEIVER);
                $('#txtReceiverTel').val(data.RECEIVER_TEL);
                if (data.PLAN_ARRIVAL_TIME != null) {
                    $('#txtPlanArrivalDate').val(data.PLAN_ARRIVAL_TIME.split(' ')[0]);
                    $('#txtPlanArrivalTime').val(data.PLAN_ARRIVAL_TIME.split(' ')[1]);
                }
                else {
                    $('#txtPlanArrivalDate').val('');
                    $('#txtPlanArrivalTime').val('');
                }
                //$('#txtPlant').val(data.PLANT);
                $('#ddlPlant').val(data.PLANT);
                //$('#ddlForwarderCompanyName').val(data.COMPANY_NAME + '@@' + data.PLANT + '@@' + data.VENDOR);
                $('#ddlForwarderCompanyName').val(data.COMPANY_NAME + '@@' + data.ERP_VND);
                $('#cbIsClose').prop('checked', ((data.IS_CLOSE == 'True') ? true : false));
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
            }
        });

        $('#importListGridDataList').jqGrid('clearGridData');
        $('#importListGridDataList').jqGrid('setGridParam', {
            postData: {
                IMPORT_LIST_NUM: escape($.trim(IMPORT_LIST_NUM))
            }
        });
        $('#importListGridDataList').jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    }
    else {
        $('#importListGridDataList').jqGrid('clearGridData');
    }
}