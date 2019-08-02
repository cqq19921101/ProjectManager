var DNList = [];

$(function () {
    $('#dialogAddDNinList').dialog({
        autoOpen: false,
        resizable: false,
        height: 500,
        width: 1160,
        modal: true,
        open: function () {
            while (DNList.length) { DNList.pop(); }
            $('button.ui-state-focus').removeClass('ui-state-focus');
            $('#DNListGridDataList').jqGrid('clearGridData');
            $(this).find('#txtArrivalDateFM').val('');
            $(this).find('#txtArrivalDateTO').val('');
            $(this).find('#txtPlanImportDateFM').val('');
            $(this).find('#txtPlanImportDateTO').val('');
            $(this).find('#txtDNNoFM').val('');
            $(this).find('#txtDNNoTO').val('');
        }
    });

    $('#btnAdd').button({
        label: "Add",
        icons: { primary: 'ui-icon-plus' }
    });

    $('#btnQuery').button({
        label: "Query",
        icons: { primary: 'ui-icon-search' }
    });

    $("#txtArrivalDateFM").datepicker({
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

    $("#txtArrivalDateTO").datepicker({
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

    $("#txtPlanImportDateFM").datepicker({
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

    $("#txtPlanImportDateTO").datepicker({
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

    $('#DNListGridDataList').jqGrid({
        url: __WebAppPathPrefix + "/VMIProcess/QueryDNListForImportList",
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
            "ASN_NUM",
            "ASN_LINE",
            "DN No.",
            "Incoterms",
            "Material",
            "Name",
            "Qty",
            "T. Net Wgt(KG)",
            "T. Gross Wgt(KG)",
            "T. Packing",
            "Pack Type",
            "Arrival Date",
            "Plan Import Date",
            "COMPANY_NAME",
            "VENDOR",
            "PLANT"],
        colModel: [
            { name: 'CHECK', width: 25, align: "center", editoptions: { value: "True:False" }, editrules: { required: true }, formatter: "checkbox", formatoptions: { disabled: false }, editable: true, sortable: false },
            { name: "ASN_NUM", index: "ASN_NUM", width: 50, sortable: false, sorttype: "text", hidden: true },
            { name: "ASN_LINE", index: "ASN_LINE", width: 120, sortable: false, sorttype: "text", hidden: true },
            { name: "IDN_NUM", index: "IDN_NUM", width: 85, sortable: false, sorttype: "text" },
            { name: "INCOTERMS", index: "INCOTERMS", width: 70, sortable: false, sorttype: "text" },
            { name: "MATERIAL", index: "MATERIAL", width: 120, sortable: false, sorttype: "text" },
            { name: "ZPNAME2", index: "ZPNAME2", width: 120, sortable: false, sorttype: "text" },
            { name: "ASN_QTY", index: "ASN_QTY", width: 60, align: 'right', sortable: false, sorttype: "text" },
            { name: "TOT_NW", index: "TOT_NW", width: 90, align: 'right', sortable: false, sorttype: "text" },
            { name: "TOT_GW", index: "TOT_GW", width: 105, align: 'right', sortable: false, sorttype: "text" },
            { name: "TOT_SET", index: "TOT_GW", width: 80, align: 'right', sortable: false, sorttype: "text" },
            { name: "PACK_MATH", index: "PACK_MATH", width: 120, sortable: false, sorttype: "text" },
            { name: "ARRIVAL_DATE", index: "ARRIVAL_DATE", width: 80, sortable: false, sorttype: "text" },
            { name: "PLAN_IMPORT_DATE", index: "PLAN_IMPORT_DATE", width: 100, sortable: false, sorttype: "text" },
            { name: "COMPANY_NAME", index: "COMPANY_NAME", width: 50, sortable: false, sorttype: "text", hidden: true },
            { name: "VENDOR", index: "VENDOR", width: 50, sortable: false, sorttype: "text", hidden: true },
            { name: "PLANT", index: "PLANT", width: 50, sortable: false, sorttype: "text", hidden: true }],
        shrinkToFit: false,
        hoverrows: false,
        height: 252,
        rowNum: 10,
        viewrecords: true,
        loadonce: true,
        pager: '#DNListGridListPager',
        beforeSelectRow: function (rowid, e) {
            var $self = $(this),
                iCol = $.jgrid.getCellIndex($(e.target).closest("td")[0]),
                cm = $self.jqGrid("getGridParam", "colModel"),
                localData = $self.jqGrid("getLocalRow", rowid);

            if (cm[iCol].name === "CHECK" && e.target.tagName.toUpperCase() === "INPUT") {
                localData.CHECK = $(e.target).is(":checked");
                var allRows = $(this).jqGrid('getDataIDs');
                var idx = DNList.indexOf(localData.ASN_NUM);

                if (localData.CHECK) {
                    if (idx == -1) {
                        DNList.push(localData.ASN_NUM);
                    }
                }
                else {
                    if (idx != -1) {
                        DNList.splice(idx, 1);
                    }
                }

                $('#lblDNList').text(DNList);

                for (var i in allRows) {
                    if (localData.ASN_NUM == $('#DNListGridDataList').jqGrid('getCell', allRows[i], 'ASN_NUM')) {
                        $('#DNListGridDataList').jqGrid('setCell', allRows[i], 'CHECK', localData.CHECK);
                    }
                }
            }

            return true;
        },
        gridComplete: function () {
            var allRows = $(this).jqGrid('getDataIDs');

            for (var i in allRows) {
                $('#DNListGridDataList').jqGrid('setCell', allRows[i], 'CHECK', 'False');
            }

            for (var i in DNList) {
                for (var j in allRows) {
                    if (DNList[i] == $(this).jqGrid('getCell', allRows[j], 'ASN_NUM')) {
                        $('#DNListGridDataList').jqGrid('setCell', allRows[j], 'CHECK', 'True');
                    }
                }
            }
        }
    });

    $('#DNListGridDataList').jqGrid('navGrid', '#DNListGridListPager', { edit: false, add: false, del: false, search: false, refresh: false });

    $('#btnAdd').click(function () {
        $(this).removeClass('ui-state-focus');

        var addDNList = $('#lblDNList').text();
        if (addDNList == '') {
            alert('Please select at leaset one DN.');
        }
        else {
            var IMPORT_LIST_NUM = $('#sImportListNum').text();
            var COMPANY_NAME = $('#ddlForwarderCompanyName').val();
            var DRIVER_NAME = $('#txtDriverName').val();
            var VEHICLE_TYPE_ID = $('#txtVehicleID').val();
            var DRIVER_TEL = $('#txtDriverTel').val();
            var RECEIVE_ADDR = $('#txtReceiveAddress').val();
            var RECEIVER = $('#txtReceiver').val();
            var RECEIVER_TEL = $('#txtReceiverTel').val();
            var PLAN_ARRIVAL_TIME = $('#txtPlanArrivalDate').val();
            //var PLANT = $('#txtPlant').val();
            var PLANT = $('#ddlPlant').val();
            var ACTION = $('#sImportListNum').text() == '' ? "CREATE" : "INSERT";

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
                    ASN_NUM: DNList,
                    PLANT: escape($.trim(PLANT))
                },
                type: "post",
                dataType: 'text',
                async: false,
                success: function (data) {
                    if (data.indexOf("IMPORT_LIST_NUM") != -1) {
                        $('#dialogAddDNinList').dialog('close');
                        initDialogImportListForm(data.split(":")[1]);
                        $('#lblDNList').text('');
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
    });

    $('#btnQuery').click(function () {
        $(this).removeClass('ui-state-focus');
        $('#DNListGridDataList').jqGrid('clearGridData');
        $('#DNListGridDataList').jqGrid('setGridParam', {
            postData: {
                ARRIVAL_DATE_FM: escape($.trim($("#dialogAddDNinList #txtArrivalDateFM").val())),
                ARRIVAL_DATE_TO: escape($.trim($("#dialogAddDNinList #txtArrivalDateTO").val())),
                PLAN_IMPORT_DATE_FM: escape($.trim($("#dialogAddDNinList #txtPlanImportDateFM").val())),
                PLAN_IMPORT_DATE_TO: escape($.trim($("#dialogAddDNinList #txtPlanImportDateTO").val())),
                DN_NO_FM: escape($.trim($("#dialogAddDNinList #txtDNNoFM").val())),
                DN_NO_TO: escape($.trim($("#dialogAddDNinList #txtDNNoTO").val()))
            }
        });
        $('#DNListGridDataList').jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });
});