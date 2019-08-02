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
        url: __WebAppPathPrefix + '/SQM/LoadChangeJSonWithFilter',
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
        width: 1350,
        height: "auto",
        colNames: [      
            
            "編號"
            , "變更類型"
            , "供應商"
            , "電話"
            , "起草人"
            ,'規格/料號'
            , "品名"
            , "變更項目類型"
            , "變更內容"
            ,"變更日期"
            , "變更原因"
            , "設計變更需求（確認/資格）"
            , "無消耗"
            , "報廠"
            , "重工"
            , "全檢"
            , "在製品"
            , "庫存數量"
            , "變更後環境影響評估"
            , "是否重新提交PPAP"
            , "供應商承認者"
            , "職位"
            , "要求日期"
                        
        ],
        colModel: [
            { name: 'TZDNo', index: 'TZDNo', width: 150, sortable: true, sorttype: 'text' },
            { name: 'ChangeType', index: 'ChangeType', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Supplier', index: 'Supplier', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Phone', index: 'Phone', width: 150, sortable: true, sorttype: 'text' },
            { name: 'OriginatorName', index: 'OriginatorName', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Spec', index: 'Spec', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Description', index: 'Description', width: 150, sortable: true, sorttype: 'text' },
            { name: 'ChangeItemType', index: 'ChangeItemType', width: 150, sortable: true, sorttype: 'text' },
            { name: 'ProposedChange', index: 'ProposedChange', width: 150, sortable: true, sorttype: 'text' },
            { name: 'ProposedDate', index: 'ProposedDate', width: 150, sortable: true, sorttype: 'date', formatter: "date", formatoptions: { srcformat: "Y/m/d H:i:s", newformat: "Y/m/d H:i:s" } },
            { name: 'ChangeReason', index: 'ChangeReason', width: 150, sortable: true, sorttype: 'text' },
            { name: 'DesignChange', index: 'DesignChange', width: 150, sortable: true, sorttype: 'text', formatter: function (cellValue, options, rowdata, action) { return rowdata.DesignChange == true ? 'Yes' : 'No'; } },
            { name: 'Consume', index: 'Consume', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Scrap', index: 'Scrap', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Rework', index: 'Rework', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Sort', index: 'Sort', width: 150, sortable: true, sorttype: 'text' },
            { name: 'WIP', index: 'WIP', width: 150, sortable: true, sorttype: 'text' },
            { name: 'QtyInStock', index: 'QtyInStock', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Environment', index: 'Environment', width: 150, sortable: true, sorttype: 'text', formatter: function (cellValue, options, rowdata, action) { return rowdata.Environment == true ? 'Yes' : 'No'; } },
            { name: 'PPAP', index: 'PPAP', width: 150, sortable: true, sorttype: 'text', formatter: function (cellValue, options, rowdata, action) { return rowdata.PPAP == true ? 'Yes' : 'No'; } },
            { name: 'SupplierApproval', index: 'SupplierApproval', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Title', index: 'Title', width: 150, sortable: true, sorttype: 'text' },
            { name: 'RequestDate', index: 'RequestDate', width: 150, sortable: true, sorttype: 'date', formatter: "date", formatoptions: { srcformat: "Y/m/d H:i:s", newformat: "Y/m/d H:i:s" } },
        ],
        rowNum: 20,
        //rowList: [10, 20, 30],
        sortname: 'TZDNo',
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