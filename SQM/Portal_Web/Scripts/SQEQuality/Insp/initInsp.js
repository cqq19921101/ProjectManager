﻿$(function () {
    //Toolbar Buttons
    $("#btnSearchInsp").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnBackInsp").button({
        label: "Back",
        icons: { primary: "ui-icon-pencil" }
    });

    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });
    //ddl
    $.ajax({
        url: __WebAppPathPrefix + '/SQMQualityInsp/GetInspCodeList',
        type: "post",
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            var options = '<option value="">-- 請選擇 --</option>';
            for (var idx in data) {
                options += '<option value=' + data[idx].SID + '> ' + data[idx].Name + '</option>';
            }
            $('#ddlSInspCode').html(options);
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
    //InspCode ddl Change
    $('#ddlSInspCode').change(function () {
        $.ajax({
            url: __WebAppPathPrefix + '/SQMQualityInsp/GetInspDescList',
            data: { MainID: $('#ddlSInspCode').val() },
            type: "post",
            dataType: 'json',
            async: false, // if need page refresh, please remark this option
            success: function (data) {
                var options = '';
                for (var idx in data) {
                    options += '<option value=' + data[idx].SID + '> ' + data[idx].Name + '</option>';
                }
                $('#ddlSInspDesc').html(options);
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
            }
        });
    });
    //Data List
    var gridDataListInsp = $("#gridDataListInsp");
    gridDataListInsp.jqGrid({
        url: __WebAppPathPrefix + '/SQEQuality/LoadQualityInspJSonWithFilter',
        postData: {
            SInspCode: ""
            , SInspDesc: ""
            , ReportSID: ""
        },
        type: "post",
        datatype: "json",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false
        },
        width: "auto",
        height: "auto",
        colNames: [
                    'ReportSID'
                    , 'CodeSID'
                    , 'DescSID'
                    , '檢測項目:'
                    , '檢測說明'
                    , '判定標準'
                    , 'CR'
                    , 'MA'
                    , 'MI'
                    , '自定義項'
                    , '檢測量'
                    , 'isOther'
                    , '結果'
                    , '判定'
        ],
        colModel: [
            { name: 'ReportSID', index: 'ReportSID', width: 100, sorttype: 'text', hidden: true },
            { name: 'CodeSID', index: 'CodeSID', width: 100, sorttype: 'text', hidden: true },
            { name: 'DescSID', index: 'DescSID', width: 150, sortable: true, sorttype: 'text', hidden: true },
            { name: 'InspCode', index: 'InspCode', width: 150, sortable: true, sorttype: 'text' },
            { name: 'InspDesc', index: 'InspDesc', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Standard', index: 'Standard', width: 150, sortable: true, sorttype: 'text' },
            { name: 'CR', index: 'CR', width: 150, sortable: true, sorttype: 'text' },
            { name: 'MA', index: 'MA', width: 150, sortable: true, sorttype: 'text' },
            { name: 'MI', index: 'MI', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Other', index: 'Other', width: 150, sortable: true, sorttype: 'text' },
            { name: 'InspNum', index: 'InspNum', width: 150, sortable: true, sorttype: 'text' },
            { name: 'isOther', index: 'isOther', width: 150, sortable: true, sorttype: 'text', hidden: true },
            { name: 'InspResult', index: 'InspResult', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Judge', index: 'Judge', width: 150, sortable: true, sorttype: 'text' }
        ],
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'Name',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#gridListPagerInsp',
        //sort by reload
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
        }
    });
    gridDataListInsp.jqGrid('navGrid', '#gridListPagerInsp', { edit: false, add: false, del: false, search: false, refresh: false });

    //$('#tbMain1Insp').show();

    //$('#dialogDataInsp').hide();
    //$('#dialogDataDesc').hide();
    //$('#inspDesc').hide();



})