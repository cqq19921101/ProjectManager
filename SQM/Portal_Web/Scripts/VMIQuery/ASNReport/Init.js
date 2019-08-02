$(function () {


    var VMI_ASNReportgridDataList = $('#VMI_Query_ASNReport_gridDataList');

    $('.jqTextUpperCase').keyup(function (e) {
        var str = $(this).val();
        str = str.toUpperCase();
        $(this).val(str);
    });

    //Init Function Button
    //Button
    $('#btn_VMIQuery_ASNReport_Query').button({
        label: "Query",
        icons: { primary: 'ui-icon-search' }
    });

    $('#btn_VMIQuery_ASNReport_Print').button({
        label: "Print",
        icons: { primary: 'ui-icon-print' }
    });

    $('#btn_VMIQuery_ASNReport_PrintBarCode').button({
        label: "Print BarCode",
        icons: { primary: 'ui-icon-print' }
    });

    $('#btn_VMIQuery_ASNReport_Plant_Query').button({
        icons: { primary: 'ui-icon-search' }
    });

    $('#btn_VMIQuery_ASNReport_SBU_VDN_Query').button({
        icons: { primary: 'ui-icon-search' }
    });

    //After Init. to Show Menu Function Button
    $('#VMIQuery_ASNReport_tbTopToolBar').show();

    //Set Tyoe Check Default Option
    var radioType = $('input:radio[name=QueryType]');
    if (radioType.is(':checked') === false) {
        radioType.filter('[value=S]').prop('checked', true);
    }

    //datepicker
    var dateformat = $('input[name$="Date"]');
    dateformat.datepicker({
        dateFormat: 'yy/mm/dd',
    });

    //Init date to ASN Date From and To
    $('#dpASNDate_TO').datepicker('setDate', SetCurrentDate(0, 0));
    var date2 = $('#dpASNDate_TO').datepicker('getDate');
    date2.setDate(date2.getDate() - 7)
    $('#dpASNDate_FM').datepicker('setDate', date2);

    //option default
    $("#dropbox_VMIQuery_ASNReport_Status").val("R");
    $("#dropbox_VMIQuery_ASNReport_TradeType").val("");

    //Init jqGrid
    var lang = "en-US";
    var langShort = lang.split('-')[0].toLowerCase();

    if ($.jgrid.hasOwnProperty("regional") && $.jgrid.regional.hasOwnProperty(lang)) {
        $.extend($.jgrid, $.jgrid.regional[lang]);
    } else if ($.jgrid.hasOwnProperty("regional") && $.jgrid.regional.hasOwnProperty(langShort)) {
        $.extend($.jgrid, $.jgrid.regional[langShort]);
    }

    VMI_ASNReportgridDataList.jqGrid({
        url: __WebAppPathPrefix + "/VMIQuery/QueryASNReportInfoJsonWithFilter",
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
                    "Status",
                    "Plant",
                    "Vendor",
                    "ASN Date",
                    "ETA",
                    "BarCode"],
        colModel: [
                    { name: "ASN_NUM", index: "ASN_NUM", width: 130, sortable: false, sorttype: "text" },
                    { name: "STATUS", index: "STATUS", width: 80, sortable: false, sorttype: "text" },
                    { name: "PLANT", index: "PLANT", width: 60, sortable: false, sorttype: "text" },
                    { name: "VENDOR", index: "VENDOR", width: 140, sortable: false, sorttype: "text" },
                    { name: "CREATE_TIME", index: "CREATE_TIME", width: 100, sortable: false, sorttype: "text" },
                    { name: "ETA", index: "ETA", width: 100, sortable: false, sorttype: "text" },
                    { name: "BARCODE", index: "BARCODE", width: 100, sortable: false, sorttype: "text" }
        ],
        multiselect: true,
        width: 850,
        height: 232,
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: "PLANT ,ASN_NUM",
        viewrecords: true,
        loadonce: true,
        pager: '#VMI_Query_ASNReport_gridListPager',
        loadComplete: function () {
            var $this = $(this);

            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '') {
                    setTimeout(function () {
                        $this.triggerHandler('reloadGrid');
                    }, 50);
                }
        }
    });

    VMI_ASNReportgridDataList.jqGrid('navGrid', '#VMI_Query_ASNReport_gridListPager', { edit: false, add: false, del: false, search: false, refresh: false });
});