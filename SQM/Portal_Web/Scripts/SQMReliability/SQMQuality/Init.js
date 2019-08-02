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

    $.ajax({
        url: __WebAppPathPrefix + '/SQMReliability/GetReportTypeList',
        type: "post",
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            var options = '<option value="">-- 請選擇 --</option>';
            for (var idx in data) {
                options += '<option value=' + data[idx].SID + '> ' + data[idx].ReportName + '</option>';
            }
            $('#ddlReportTypeSearch').html(options);
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });

    //Data List
    var cn = ['ReportSID',
        'AQL',
              '報告單號',
               'ReportType',
               '報告類型',
              '供應商',
    '出貨日', '送貨數量', '製造商料號', '光寶料號', '生產批號', '生產日期', '檢驗日', '生產地', '批次', '是否變更製程', '變更說明', '設備', '原料', '品管員', '檢驗結果', '附件'];
    var cm = [
            { name: 'ReportSID', index: 'ReportSID', width: 200, sortable: false, hidden: true },
            { name: 'AQL', index: 'AQL', width: 200, sortable: false, hidden: true },
            { name: 'ReportNo', index: 'ReportNo', width: 150, sortable: true, sorttype: 'text' },
            { name: 'ReportType', index: 'ReportType', width: 200, sortable: false, hidden: true },
            { name: 'ReportName', index: 'ReportName', width: 200, sortable: false },
            { name: 'Supplier', index: 'Supplier', width: 150, sortable: true, sorttype: 'text' },
            { name: 'DeliveryDate', index: 'DeliveryDate', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Qty', index: 'Qty', width: 150, sortable: true, sorttype: 'text' },
            { name: 'SupplierNo', index: 'SupplierNo', width: 150, sortable: true, sorttype: 'text' },
            { name: 'LiteNo', index: 'LiteNo', width: 150, sortable: true, sorttype: 'text' },
            { name: 'LotNo', index: 'LotNo', width: 150, sortable: true, sorttype: 'text' },
            { name: 'DateCode', index: 'DateCode', width: 150, sortable: true, sorttype: 'text' },
            { name: 'OQCDate', index: 'OQCDate', width: 150, sortable: true, sorttype: 'date', formatter: "date", formatoptions: { newformat: "Y/m/d" } },
            { name: 'MFGLocation', index: 'MFGLocation', width: 150, sortable: true, sorttype: 'text' },
            { name: 'SupplierRemark', index: 'SupplierRemark', width: 150, sortable: true, sorttype: 'text' },
            { name: 'isChange', index: 'isChange', width: 200, sortable: true, sorttype: 'text' },
            { name: 'ChangeNote', index: 'ChangeNote', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Equipment', index: 'Equipment', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Material', index: 'Material', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Inspector', index: 'Inspector', width: 150, sortable: true, sorttype: 'text' },
            {
                name: '', index: '', width: 150, sortable: true, sorttype: 'text', classes: "jqGridColumnDataAsLinkWithBlue", formatter: function (cellvalue, options, rowObject) {
                    return "點擊查看";
                }
            },
            { name: '', index: '', width: 150, sortable: true, sorttype: 'text', classes: "jqGridColumnDataAsLinkWithBlue", formatter: function (cellvalue, options, rowObject) { return "點擊查看"; } }

    ];

    var litno;
    var gridDataList = $("#gridDataList");
    gridDataList.jqGrid({
        url: __WebAppPathPrefix + '/SQMReliability/LoadQualityJSonWithFilter',
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
        width: 1300,
        height: "auto",
        colNames: cn,
        colModel: cm,
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'ReportNo',
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
        },
        onCellSelect: function (rowid, iCol, cellcontent, e) {
            $this = $(this);
            if (iCol == 20) {

                $("#quality").hide();
                $("#inspInsp").show();
                $("#inspInspVar").show();
                $("#tbMain1Insp").show();
                $("#tbMain1InspVar").show();
                gridDataList.attr("AQL", $this.jqGrid('getCell', rowid, 'AQL'));
                var gridDataListInsp = $("#gridDataListInsp");
                litno = $this.jqGrid('getCell', rowid, 'LiteNo');
                gridDataListInsp.attr("ReportSID", $this.jqGrid('getCell', rowid, 'ReportSID'));
                gridDataListInsp.jqGrid('setGridParam', { postData: { ReportSID: $this.jqGrid('getCell', rowid, 'ReportSID'), SInspCode: "", SInspDesc: "" } });
                gridDataListInsp.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');

                var gridDataListInspVar = $("#gridDataListInspVar");
                gridDataListInspVar.attr("ReportSID", $this.jqGrid('getCell', rowid, 'ReportSID'));
                gridDataListInspVar.jqGrid('setGridParam', { postData: { ReportSID: $this.jqGrid('getCell', rowid, 'ReportSID'), SInspCode: "", SInspDesc: "" } });
                gridDataListInspVar.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
                getInspCode();
            }
            if (iCol == 21) {

                $("#quality").hide();
                $("#inspFile").show();
                $("#tbMain1File").show();

                var gridDataListFile = $("#gridDataListFile");
                gridDataListFile.attr("ReportSID", $this.jqGrid('getCell', rowid, 'ReportSID'));
               
                gridDataListFile.jqGrid('setGridParam', { postData: { ReportSID: $this.jqGrid('getCell', rowid, 'ReportSID') } });
                gridDataListFile.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
            }
        }
    });
    gridDataList.jqGrid('navGrid', '#gridListPager', { edit: false, add: false, del: false, search: false, refresh: false });


    $('#tbMain1').show();
    $('#dialogData').show();
    $("#inspInsp").hide();
    $("#inspInspVar").hide();
    $("#inspFile").hide();

    function getInspCode()
    {
        $.ajax({
            url: __WebAppPathPrefix + '/SQMReliability/GetInspCodeList',
            data: { Insptype: "", LitNo: litno },
            type: "post",
            dataType: 'json',
            async: false, // if need page refresh, please remark this option
            success: function (data) {
                var options = '';
                for (var idx in data) {
                    options += '<option value=' + data[idx].SID + '> ' + data[idx].Name + '</option>';
                }
                $('#ddlInspCode').html(options);
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
            }
        });
        $.ajax({
            url: __WebAppPathPrefix + '/SQMReliability/GetInspCodeList',
            data: { Insptype: "Variables", LitNo: litno },
            type: "post",
            dataType: 'json',
            async: false, // if need page refresh, please remark this option
            success: function (data) {
                var options = '';
                for (var idx in data) {
                    options += '<option value=' + data[idx].SID + '> ' + data[idx].Name + '</option>';
                }
                $('#vddlInspCode').html(options);
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
            }
        });
        $.ajax({
            url: __WebAppPathPrefix + '/SQMReliability/GetInspCodeList',
            data: { Insptype: "Attributes", LitNo: litno },
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
        $.ajax({
            url: __WebAppPathPrefix + '/SQMReliability/GetInspCodeList',
            data: { Insptype: "Variables", LitNo: litno },
            type: "post",
            dataType: 'json',
            async: false, // if need page refresh, please remark this option
            success: function (data) {
                var options = '<option value="">-- 請選擇 --</option>';
                for (var idx in data) {
                    options += '<option value=' + data[idx].SID + '> ' + data[idx].Name + '</option>';
                }
                $('#ddlSInspCodeVar').html(options);
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
            }
        });
    }
});

