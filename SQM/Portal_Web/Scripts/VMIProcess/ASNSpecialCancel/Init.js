$(function () {

    var diaASNSpecialCancel = $('#dialog_VMIProcess_ASNSpecialCancelManage');
    var VMI_ASNSpecialCancelgridDataList = $('#VMI_Process_ASNSpecialCancel_gridDataList');

    $('#btn_VMIProcess_ASNSpecialCancel_Query').button({
        label: "Query",
        icons: { primary: 'ui-icon-search' }
    });

    $('#btn_VMIProcess_ASNSpecialCancel_Cancel').button({
        label: "Cancel",
        icons: { primary: 'ui-icon-cancel' }
    });

    $('#btn_VMIProcess_ASNSpecialCancel_Plant_Query').button({
        icons: { primary: 'ui-icon-search' }
    });

    $('#btn_VMIProcess_ASNSpecialCancel_SBU_VDN_Query').button({
        icons: { primary: 'ui-icon-search' }
    });

    //After Init. to Show Menu Function Button
    $('#VMIProcess_ASNSpecialCancel_tbTopToolBar').show();

    //datepicker
    var dateformat = $('input[name$="Date"]');
    dateformat.datepicker({
        dateFormat: 'yy/mm/dd',
    });

    //20160411 edward huang add but not work
    //Init date to ASN Date From and To
    $('#dpASNDate_TO').datepicker('setDate', SetCurrentDate(0, 0));
    var date2 = $('#dpASNDate_TO').datepicker('getDate');
    date2.setDate(date2.getDate() - 7)
    $('#dpASNDate_FM').datepicker('setDate', date2);


    //Init jqGrid
    var lang = "en-US";
    var langShort = lang.split('-')[0].toLowerCase();

    if ($.jgrid.hasOwnProperty("regional") && $.jgrid.regional.hasOwnProperty(lang)) {
        $.extend($.jgrid, $.jgrid.regional[lang]);
    } else if ($.jgrid.hasOwnProperty("regional") && $.jgrid.regional.hasOwnProperty(langShort)) {
        $.extend($.jgrid, $.jgrid.regional[langShort]);
    }

    VMI_ASNSpecialCancelgridDataList.jqGrid({
        url: __WebAppPathPrefix + "/VMIProcess/QueryASNSpecialCancelInfoJsonWithFilter",
        //postData: { },
        mtype: "POST",
        datatype: "local",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false
        },
        colNames: ["ASN No",
                    "Plant",
                    "Vendor",
                    "ASN Date",
                    "ETA"],
        colModel: [
                    {
                        name: "ASN_NUM", index: "ASN_NUM", width: 110, sortable: false, sorttype: "text", classes: "jqGridColumnDataAsLinkWithBlue",
                        formatter: function (cellvalue, option, rowobject) {
                            return cellvalue;
                        }
                    },
                    { name: "PLANT", index: "PLANT", width: 80, sortable: false, sorttype: "text" },
                    { name: "VENDOR", index: "VENDOR", width: 200, sortable: false, sorttype: "text" },
                    { name: "CREATE_TIME", index: "CREATE_TIME", width: 100, sortable: false, sorttype: "text" },
                    { name: "ETA", index: "ETA", width: 100, sortable: false, sorttype: "text" }
        ],
        beforeSelectRow: function (rowid, e) {
            iCol = $.jgrid.getCellIndex($(e.target).closest("td")[0]),
            cm = $(this).jqGrid("getGridParam", "colModel");

            if (cm[iCol].name === "cb") {
                if ($("#jqg_VMI_Process_ASNSpecialCancel_gridDataList_" + rowid).attr("disabled")) {
                    return false;
                }
                return true;
            }
            else if (cm[iCol].name === "ASN_NUM") {
                var rowData = $(this).jqGrid("getRowData", rowid);
                if (rowData.ASN_NUM != "") {
                    __DialogIsShownNow = false;
                    if (!__DialogIsShownNow) {
                        __DialogIsShownNow = true;
                        diaASNSpecialCancel.prop("ASN_NUM", $.trim(rowData.ASN_NUM));

                        ////EnableToDoASNFunction();
                        InitdialogASNSpecialCancelForManage();
                        ReloadASNSpecialCancelManagegridDataList();

                        setTimeout(function () {
                            diaASNSpecialCancel.show();
                            diaASNSpecialCancel.dialog("open");
                            // div with class ui-dialog
                            $('.ui-dialog :button').blur();
                        }, 250);
                    }
                }

                return false;
            }
            else {
                return false;
            }
        },
        multiselect: true,
        width: 650,
        height: 232,
        rowNum: 10,
        autowidth: false,
        shrinkToFit: false,
        //rowList: [10, 20, 30],
        sortname: "PLANT ,ASN_NUM",
        viewrecords: true,
        loadonce: true,
        pager: '#VMI_Process_ASNSpecialCancel_gridListPager',
        loadComplete: function (data) {
            var $this = $(this);

            if (data.Rows !== undefined) {
                if (data.Rows.length == 0) {
                    $("#btn_VMIProcess_ASNSpecialCancel_Cancel").attr("disabled", true);
                }
                else {
                    $("#btn_VMIProcess_ASNSpecialCancel_Cancel").attr("disabled", false);
                }
            }

            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '') {
                    //setTimeout(function () {
                    //    $this.triggerHandler('reloadGrid');
                    //}, 50);
                }
        }
    });

    VMI_ASNSpecialCancelgridDataList.jqGrid('navGrid', '#VMI_Process_ASNSpecialCancel_gridListPager', { edit: false, add: false, del: false, search: false, refresh: false });
});
