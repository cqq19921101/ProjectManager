$(function () {
    $.ajax({
        url: __WebAppPathPrefix + '/SQMContact/GetJobList',
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
    $("#btnBack").button({
        label: "Back",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnBack").click(function () {
        var gridDataList = $("#SQEContactgridDataList");
        gridDataList.jqGrid('clearGridData');
        $("#Contact").show()
        $("#ContacttbMain1").show();
        $("#ContactgridDataList").show();

        $("#ContactInfo").hide()
        $('#SQEContacttbMain1').hide();
        $('#SQEContactdialogData').hide();

    });
    $("#btnSQEContactSearch").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnSQEContactCreate").button({
        label: "Create",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnSQEContactViewEdit").button({
        label: "View/Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnSQEContactDelete").button({
        label: "Delete",
        icons: { primary: "ui-icon-trash" }
    });
    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });

    //Data List
    var gridDataList = $("#SQEContactgridDataList");
    gridDataList.jqGrid({
        url: __WebAppPathPrefix + '/SQEContact/LoadContact',
        postData: { MemberGUID: "" },
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
        colNames: ['SID',
                    'Provider',
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
            { name: 'Provider', index: 'Provider', width: 150, sortable: true, sorttype: 'text', hidden: true },
            { name: 'Vendor', index: 'Vendor', width: 150, sortable: true, sorttype: 'text', hidden: true },
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
        pager: '#SQEContactgridListPager',
        //sort by reload
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
            //var vendor = $this.jqGrid('getRowData', '1').Vendor;
            //$("#spVendor").html(typeof (vendor) == "undefined" ? 'None' : vendor);
        }
    });
    gridDataList.jqGrid('navGrid', '#SQEContactgridListPager', { edit: false, add: false, del: false, search: false, refresh: false });

    //$("#ContactInfo").show()
    //$('#SQEContacttbMain1').show();
    //$('#SQEContactdialogData').show();
})