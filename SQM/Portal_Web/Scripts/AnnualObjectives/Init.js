$(function () {
   
    //Toolbar Buttons
    $("#btnSearch").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnCreate").button({
        label: "Create",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnViewEdit").button({
        label: "View/Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnDelete").button({
        label: "Delete",
        icons: { primary: "ui-icon-trash" }
    });


    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });
    $('#ddlCID').change(function () {
        $.ajax({
            url: __WebAppPathPrefix + '/SQMBasic/GetCommoditySubList',
            data: { MainID: $('#ddlCID').val() },
            type: "post",
            dataType: 'json',
            async: false, // if need page refresh, please remark this option
            success: function (data) {
                var options = '';
                for (var idx in data) {
                    options += '<option value=' + data[idx].CID + '>' + data[idx].CID + ' ' + data[idx].CNAME + '</option>';
                }
                $('#ddlCCID').html(options);
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
            }
        });
    });
    //材料一級菜單
    $.ajax({
        url: __WebAppPathPrefix + '/SQMBasic/GetCommodityList',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
        type: "post",
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            var options = '';
            for (var idx in data) {
                options += '<option value=' + data[idx].CID + '>' + data[idx].CID + ' ' + data[idx].CNAME + '</option>';
            }
            $('#ddlCID').append(options);
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
    //材料一級菜單
    $.ajax({
        url: __WebAppPathPrefix + '/AnnualObjectives/GetTypeList',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
        type: "post",
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            var options = '';
            for (var idx in data) {
                options += '<option value=' + data[idx].MaterialType + '>' + data[idx].MaterialType + '</option>';
            }
            $('#ddlType').append(options);
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
    //Data List
    var gridDataList = $("#gridDataList");
    gridDataList.jqGrid({
        url: __WebAppPathPrefix + '/AnnualObjectives/LoadJSonWithFilter',
        postData: { SearchText: "" },
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
        colNames: [ 'SID',
                   '','',
                   '材料类别',
                   '材料子类别',
                   '对应材料',
                   '目标类型',
                   'Q1',

                   'Q2',
                   'Q3',
                   'Q4'
                  
                    ],
        colModel: [
            { name: 'SID', index: 'SID', width: 200, sortable: false, hidden: true },
            { name: 'CID', index: 'CID', width: 150, sortable: true, hidden: true },
            { name: 'CCID', index: 'CCID', width: 150, sortable: true, hidden: true },
            { name: 'CName', index: 'CName', width: 150, sortable: true, sorttype: 'text' },
            { name: 'CCName', index: 'CCName', width: 150, sortable: true, sorttype: 'text' },
            { name: 'AType', index: 'AType', width: 150, sortable: true, sorttype: 'text' },
            { name: 'MaterialType', index: 'MaterialType', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Q1', index: 'Q1', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Q2', index: 'Q2', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Q3', index: 'Q3', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Q4', index: 'Q4', width: 150, sortable: true, sorttype: 'text' }
           
        ],
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'SID',
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
    gridDataList.jqGrid('navGrid', '#gridListPager', { edit: false, add: false, del: false, search: false, refresh: false });

    $('#tbMain1').show();
    $('#dialogData').show();
});
