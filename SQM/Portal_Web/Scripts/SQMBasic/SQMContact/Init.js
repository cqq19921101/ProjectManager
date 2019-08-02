$(function () {
    $.ajax({
        url: __WebAppPathPrefix + '/SQMBasic/GetJobList',
        type: "post",
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            var options = '';
            for (var idx in data) {
                options += '<option value=' + data[idx].CID + '>' + data[idx].CID + ' ' + data[idx].CNAME + '</option>';
            }
            $('#ddlJob').append(options);
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
    //Toolbar Buttons
    $("#btnSQMContactSearch").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnSQMContactCreate").button({
        label: "Create",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnSQMContactViewEdit").button({
        label: "View/Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnSQMContactDelete").button({
        label: "Delete",
        icons: { primary: "ui-icon-trash" }
    });


    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });

    //Data List
    var gridDataList = $("#SQMContactgridDataList");
    gridDataList.jqGrid({
        url: __WebAppPathPrefix + '/SQMBasic/LoadContactJSonWithFilter',
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
                   
                   '供应商',
                   'jobID',
                   '职务',

                   '姓名',
                   '手机号码',
                   '固定电话/分机',
                   '电子邮箱'
                    ],
        colModel: [
            { name: 'SID', index: 'SID', width: 200, sortable: false, hidden: true },
           
            { name: 'Vendor', index: 'Vendor', width: 150, sortable: true, sorttype: 'text' },
            { name: 'jobID', index: 'job', width: 150, sortable: true, hidden: true },
            { name: 'job', index: 'job', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Name', index: 'Name', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Phone', index: 'Phone', width: 150, sortable: true, sorttype: 'text' },
            { name: 'FixedTelephone', index: 'FixedTelephone', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Email', index: 'Email', width: 150, sortable: true, sorttype: 'text' }
        ],
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'Provider',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#SQMContactgridListPager',
        //sort by reload
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
        }
    });
    gridDataList.jqGrid('navGrid', '#SQMContactgridListPager', { edit: false, add: false, del: false, search: false, refresh: false });

    $('#SQMContacttbMain1').show();
    $('#SQMContactdialogData').show();
});
