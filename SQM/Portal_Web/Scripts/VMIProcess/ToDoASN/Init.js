$(function () {
    var diaToDoASNManage = $('#dialog_VMIProcess_ToDoASNManage');
    var VMI_ToDoASNgridDataList = $('#VMI_Process_ToDoASN_gridDataList');

    //Init Function Button
    //Button
    $('#btn_VMIProcess_ToDoASN_Query').button({
        label: "Query",
        icons: { primary: 'ui-icon-search' }
    });

    $('#btn_VMIProcess_ToDoASN_CreateHeader').button({
        label: "New",
        icons: { primary: 'ui-icon-plus' }
    });

    $('#btn_VMIProcess_ToDoASN_Plant_Query').button({
        icons: { primary: 'ui-icon-search' }
    });

    $('#btnVMIProcess_ToDoASN_SBU_VDN_Query').button({
        icons: { primary: 'ui-icon-search' }
    });

    $('#btn_VMIProcess_ToDoASN_Plant_Query2').button({
        icons: { primary: 'ui-icon-search' }
    });

    $('#btn_VMIProcess_ToDoASN_SBU_VDN_Query2').button({
        icons: { primary: 'ui-icon-search' }
    });



    //After Init. to Show Menu Function Button
    $('#VMIProcess_ToDoASN_tbTopToolBar').show();

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
    date2.setDate(date2.getDate() -7)
    $('#dpASNDate_FM').datepicker('setDate', date2);

    //option default
    $("#dropbox_VMIProcess_ToDoASN_Status").val("");
    $("#dropbox_VMIProcess_ToDoASN_TradeType").val("");

    //Init jqGrid
    var lang = "en-US";
    var langShort = lang.split('-')[0].toLowerCase();

    if ($.jgrid.hasOwnProperty("regional") && $.jgrid.regional.hasOwnProperty(lang)) {
        $.extend($.jgrid, $.jgrid.regional[lang]);
    } else if ($.jgrid.hasOwnProperty("regional") && $.jgrid.regional.hasOwnProperty(langShort)) {
        $.extend($.jgrid, $.jgrid.regional[langShort]);
    }

    VMI_ToDoASNgridDataList.jqGrid({
        url: __WebAppPathPrefix + "/VMIProcess/QueryToDoASNInfoJsonWithFilter",
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
                    { name: "PLANT", index: "PLANT", width: 80, sortable: false, sorttype: "text" },
                    { name: "VENDOR", index: "VENDOR", width: 120, sortable: false, sorttype: "text" },
                    { name: "CREATE_TIME", index: "CREATE_TIME", width: 100, sortable: false, sorttype: "text" },
                    { name: "ETA", index: "ETA", width: 100, sortable: false, sorttype: "text" }
        ],
        onCellSelect: function (rowid, iCol, cellcontent, e) {
            var $this = $(this);
            var ASN_NUM = $this.jqGrid('getCell', rowid, 'ASN_NUM');
            var ETA = $this.jqGrid('getCell', rowid, 'ETA');
            var STATUS = $this.jqGrid('getCell', rowid, 'STATUS');
            if (ASN_NUM != "") {
                if (iCol == 0) {
                    if (!__DialogIsShownNow) {
                        __DialogIsShownNow = true;
                        diaToDoASNManage.prop("ASN_NUM", $.trim(ASN_NUM));
                        diaToDoASNManage.prop("ETA", $.trim(ETA));
                        diaToDoASNManage.prop("STATUS", $.trim(STATUS));

                        //EnableToDoASNFunction();
                        bButtonFunctionEnable = true;
                        InitdialogToDoASNHeaderForManage();
                        ReloadToDoASNManagegridDataList();

                        setTimeout(function () {
                            diaToDoASNManage.show();
                            diaToDoASNManage.dialog("open");
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
        pager: '#VMI_Process_ToDoASN_gridListPager',
        loadComplete: function () {
            var $this = $(this);

            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '') {
                    //setTimeout(function () {
                    //    $this.triggerHandler('reloadGrid');
                    //}, 50);
                }
        }
    });

    VMI_ToDoASNgridDataList.jqGrid('navGrid', '#VMI_Process_ToDoASN_gridListPager', { edit: false, add: false, del: false, search: false, refresh: false });

    if ($('#addReviewingFlag') != undefined && $('#addReviewingFlag').text() != '') {
        if ($('#addReviewingFlag').text() == 'VMI: VMIASNReviewer') {
            $('#dropbox_VMIProcess_ToDoASN_Status').children().remove();

            $('#txt_VMIProcess_ToDoASN_Plant').prop('disabled', true);
            $('#btn_VMIProcess_ToDoASN_Plant_Query').prop('disabled', true);
            $('#txt_VMIProcess_ToDoASN_SBU_VDN').prop('disabled', true);
            $('#btnVMIProcess_ToDoASN_SBU_VDN_Query').prop('disabled', true);
            $('#btn_VMIProcess_ToDoASN_CreateHeader').prop('disabled', true);

            $('#dropbox_VMIProcess_ToDoASN_Status').prop('disabled', true);
            $('input[name=QueryType]').prop('disabled', true)
            $('#dropbox_VMIProcess_ToDoASN_TradeType').val('I').prop('disabled', true);
        }
        $('#dropbox_VMIProcess_ToDoASN_Status').append($('<OPTION/>').attr('value', 'V').text('Reviewing'));
        // $('#dropbox_VMIProcess_ToDoASN_Status').val('V');
        //<option value="">To Do All</option>
    }

    if ($('#txt_VMIProcess_ToDoASN_ASNNoFM').val() != "") {
        ReloadToDoASNgridDataList();
    }  
});

