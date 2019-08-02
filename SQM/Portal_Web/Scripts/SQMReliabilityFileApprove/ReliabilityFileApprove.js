$(function () {
    $("#btnAgree").button({
        label: "Approve",
        icons: { primary: "ui-icon-pencil" }
    });

    $("#btnAgree").click(function () {
        //alert($('#hTaskID').val());
        //$(this).removeClass('ui-state-focus');
        //var DoSuccessfully = false;
        $("#btnAgree").attr('disabled', 'disabled');
        $.ajax({
            url: __WebAppPathPrefix + "/SQM/UpdateTaskStatus",
            data: {
                "Status": 'Approve',
                "TaskID": $("#hTaskID").val(),
                "Remark": $("#txtRemark").val()
            },
            type: "post",
            dataType: 'text',
            async: false,
            success: function (data) {
                if (data == "") {
                    DoSuccessfully = true;
                    alert("Operate successfully.");
                    $("[data='oper']").hide();
                    $("[data='result']").html("Approve");
                    $("[data='result']").show();
                }
                else {
                    data = data.replace("<br />", "\u000d");
alert("error:" + data);
                }
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
                //$("#ajaxLoading").hide();
                $("#btnAgree").removeAttr('disabled');
            }
        });

        //if (DoSuccessfully) {

        //}
    });

    $('#btnReject').button({
        label: 'Reject',
        icons: { primary: 'ui-icon-arrowthickstop-1-s' }
    });
    $("#btnReject").click(function () {
        $("#btnReject").attr('disabled', 'disabled');
        $.ajax({
            url: __WebAppPathPrefix + "/SQM/UpdateTaskStatus",
            data: {
                "Status": 'Reject',
                "TaskID": $("#hTaskID").val(),
                "Remark": $("#txtRemark").val()
            },
            type: "post",
            dataType: 'text',
            async: false,
            success: function (data) {
                if (data == "") {
                    DoSuccessfully = true;
                    alert("Operate successfully.");
                    $("[data='oper']").hide();
                    $("[data='result']").html("Reject");
                    $("[data='result']").show();
                }
                else {
                    data = data.replace("<br />", "\u000d");
                    alert("error:" + data);
                }
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
                //$("#ajaxLoading").hide();
                $("#btnReject").removeAttr('disabled');
            }
        });
    });

    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });
    var gridDataDataList = $("#gridDataList");
    gridDataDataList.jqGrid({
        url: __WebAppPathPrefix + '/SQM/LoadFileJSonWithFilter',
        postData: { TaskID: $("#hTaskID").val() },
        type: "post",
        datatype: "json",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false
        },
        width: 800,
        height: "auto",
        colNames: [
                   'NameInChinese',
                    'CommodityName',
                    'Commodity_SubName',
                    'TestProjet',
                    'PlannedTestTime',
                    'ActualTestTime',
                    'TestResult',
                    'TestPeople',
                    'Note',
                    'FileName'

        ],
        colModel: [
            { name: 'NameInChinese', index: 'NameInChinese', width: 150, sortable: true, sorttype: 'text' },
            { name: 'CommodityName', index: 'CommodityName', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Commodity_SubName', index: 'Commodity_SubName', width: 150, sortable: true, sorttype: 'text' },
            { name: 'TestProjet', index: 'TestProjet', width: 150, sortable: true, sorttype: 'text' },
            { name: 'PlannedTestTime', index: 'PlannedTestTime', width: 150, sortable: true, sorttype: 'text' },
            { name: 'ActualTestTime', index: 'ActualTestTime', width: 150, sortable: true, sorttype: 'text' },
            { name: 'TestResult', index: 'TestResult', width: 150, sortable: true, sorttype: 'text' },
            { name: 'TestPeople', index: 'TestPeople', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Note', index: 'Note', width: 150, sortable: true, sorttype: 'text' },
            { name: 'FileName', sortable: false, width: 280, classes: "jqGridColumnDataAsLinkWithBlue" },

        ],
        rowNum: 20,
        //rowList: [10, 20, 30],
        sortname: 'TestProjet',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#gridListPager',
        //sort by reload
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
        }
    });
    gridDataDataList.jqGrid('navGrid', '#gridListPager', { edit: false, add: false, del: false, search: false, refresh: false });

});