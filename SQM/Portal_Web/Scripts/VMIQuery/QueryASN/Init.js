$(function () {
    var diaQueryASNManage = $('#dialog_VMIQuery_QueryASNManage');
    var VMI_QueryASNgridDataList = $('#VMI_VMIQuery_QueryASN_gridDataList');

    $('.jqTextUpperCase').keyup(function (e) {
        var str = $(this).val();
        str = str.toUpperCase();
        $(this).val(str);
    });

    //Init Function Button
    //Button
    $('#btn_VMIQuery_QueryASN_Query').button({
        label: "Query",
        icons: { primary: 'ui-icon-search' }
    });

    $('#btn_VMIQuery_QueryASN_Plant_Query').button({
        icons: { primary: 'ui-icon-search' }
    });

    $('#btn_VMIQuery_QueryASN_SBU_VDN_Query').button({
        icons: { primary: 'ui-icon-search' }
    });

    //After Init. to Show Menu Function Button
    $('#VMIQuery_QueryASN_tbTopToolBar').show();

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
    $("#dropbox_VMIQuery_QueryASN_Status").val("A");
    $("#dropbox_VMIQuery_QueryASN_TradeType").val("");

    //Init jqGrid
    var lang = "en-US";
    var langShort = lang.split('-')[0].toLowerCase();

    if ($.jgrid.hasOwnProperty("regional") && $.jgrid.regional.hasOwnProperty(lang)) {
        $.extend($.jgrid, $.jgrid.regional[lang]);
    } else if ($.jgrid.hasOwnProperty("regional") && $.jgrid.regional.hasOwnProperty(langShort)) {
        $.extend($.jgrid, $.jgrid.regional[langShort]);
    }

    VMI_QueryASNgridDataList.jqGrid({
        url: __WebAppPathPrefix + "/VMIQuery/QueryASNInfoJsonWithFilter",
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
                    "ETA"],
        colModel: [
                    {
                        name: "ASN_NUM", index: "ASN_NUM", width: 130, sortable: false, sorttype: "text", classes: "jqGridColumnDataAsLinkWithBlue",
                        formatter: function (cellvalue, option, rowobject) {
                            return cellvalue;
                        }
                    },
                    { name: "STATUS", index: "STATUS", width: 80, sortable: false, sorttype: "text" },
                    { name: "PLANT", index: "PLANT", width: 60, sortable: false, sorttype: "text" },
                    { name: "VENDOR", index: "VENDOR", width: 140, sortable: false, sorttype: "text" },
                    { name: "CREATE_TIME", index: "CREATE_TIME", width: 100, sortable: false, sorttype: "text" },
                    { name: "ETA", index: "ETA", width: 100, sortable: false, sorttype: "text" }
        ],
        onCellSelect: function (rowid, iCol, cellcontent, e) {
            var $this = $(this);
            var ASN_NUM = $this.jqGrid('getCell', rowid, 'ASN_NUM');
            var STATUS = $this.jqGrid('getCell', rowid, 'STATUS');
            if (ASN_NUM != "") {
                if (iCol == 0) {
                    if (!__DialogIsShownNow) {
                        __DialogIsShownNow = true;
                        diaQueryASNManage.prop("ASN_NUM", $.trim(ASN_NUM));
                        diaQueryASNManage.prop('STATUS', $.trim(STATUS));

                        ////EnableToDoASNFunction();
                        InitdialogQueryASNHeaderForManage();
                        ReloadQueryASNManagegridDataList();

                        //AuthEditShippingInfoExcuteIsPass
                        AuthEditShippingInfoExcuteIsPass();

                        setTimeout(function () {
                            diaQueryASNManage.show();
                            diaQueryASNManage.dialog("open");
                            // div with class ui-dialog
                            $('.ui-dialog :button').blur();
                        }, 250);
                    }
                }
            }
        },
        width: 650,
        height: 232,
        rowNum: 10,
        autowidth: false,
        shrinkToFit: false,
        //rowList: [10, 20, 30],
        sortname: "PLANT ,ASN_NUM",
        viewrecords: true,
        loadonce: true,
        pager: '#VMI_VMIQuery_QueryASN_gridListPager',
        loadComplete: function () {
            var $this = $(this);

            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '') {
                    setTimeout(function () {
                        $this.triggerHandler('reloadGrid');
                    }, 50);
                }
        },
        beforeProcessing: function (data) {
            if (data.Message != undefined) {
                alert(data.Message);
            }
        }
    });

    VMI_QueryASNgridDataList.jqGrid('navGrid', '#VMI_VMIQuery_QueryASN_gridListPager', { edit: false, add: false, del: false, search: false, refresh: false });
});

function AuthEditShippingInfoExcuteIsPass() {
    var diaQueryASNManage = $('#dialog_VMIQuery_QueryASNManage');
    $.ajax({
        url: __WebAppPathPrefix + '/VMIProcess/AuthEditShippingInfo',
        data: {
            ASNNUM: escape($.trim(diaQueryASNManage.prop("ASN_NUM")))
        },
        type: "post",
        dataType: 'json',
        async: false,
        success: function (data) {
            if (data.Result) {
                $('#dia_btn_VMIQuery_QueryASNManage_EditShipInfo').show();
            }
            else {
                $('#dia_btn_VMIQuery_QueryASNManage_EditShipInfo').hide();
            }
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        }
    });
}