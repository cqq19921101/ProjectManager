$(function () {
    $.ajax({
        url: __WebAppPathPrefix + '/SQECriticism/GetCriticismType',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
        data: { CriticismCategory: "CriticismCategory", CriticismUnit: "", criticismItem: "" },
        type: "post",
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            //var options = '<option value=-1 Selected>All</option>';
            var options = '';
            for (var idx in data) {
                options += '<option value=' + data[idx].PARAME_NAME + '>' + data[idx].PARAME_ITEM + '</option>';
            }
            $('#ddlCriticismCategoryD').append(options);
            
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
    $('#ddlCriticismCategoryD').change(function () {
        $.ajax({
            url: __WebAppPathPrefix + '/SQECriticism/GetCriticismType',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
            data: { CriticismCategory: "CriticismUnit", CriticismUnit: $('#ddlCriticismCategoryD').val(), criticismItem: "" },
            type: "post",
            dataType: 'json',
            async: false, // if need page refresh, please remark this option
            success: function (data) {
                
                var options = '';
                //var options = '<option value=-1 Selected>All</option>';
                for (var idx in data) {
                    options += '<option value=' + data[idx].PARAME_NAME + '>' + data[idx].PARAME_ITEM + '</option>';
                }
                //$('#ddlCategory').append(options);
                $('#ddlCriticismUnitD').html(options);
                $('#ddlCriticismUnitD').change();
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
            }
        });
    });
    $('#ddlCriticismUnitD').change(function () {
        $.ajax({
            url: __WebAppPathPrefix + '/SQECriticism/GetCriticismType',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
            data: { CriticismCategory: "CriticismItem", CriticismUnit: $('#ddlCriticismCategoryD').val(), criticismItem: $('#ddlCriticismUnitD').val() },
            type: "post",
            dataType: 'json',
            async: false, // if need page refresh, please remark this option
            success: function (data) {
             
                var options = '';
               // var options = '<option value=-1 Selected>All</option>';
                for (var idx in data) {
                    options += '<option value=' + data[idx].PARAME_NAME + '>' + data[idx].PARAME_ITEM + '</option>';
                }
                //$('#ddlCategory').append(options);
                $('#ddlItemNOD').html(options);
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
            }
        });
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

        $("#CriticismMap").hide()
        $('#CriticismtbMain1').hide();
        $('#CriticismgridDataList').hide();
    });
    $("#btnCriticismSearch").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnCriticismCreate").button({
        label: "Create",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnCriticismViewEdit").button({
        label: "View/Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnCriticismDelete").button({
        label: "Delete",
        icons: { primary: "ui-icon-trash" }
    });
    $("#btnCreatCriticism").button({
        label: "CreatCriticism",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnShowCriticism").button({
        label: "ShowCriticism",
        icons: { primary: "ui-icon-search" }
    });
    $('#btnCommit').button({
        label: '导出',
        icons: { primary: 'ui-icon-arrowthickstop-1-s' }
    });
    $('#ddlCriticismUnitD').change()
    $('#ddlItemNOD').change()
    
    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });

    //Data List
    var gridDataList = $("#CriticismgridDataList");
    gridDataList.jqGrid({
        url: __WebAppPathPrefix + '/SQECriticism/LoadCriticismMapJSonWithFilter',
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
        width: 200,
        height: "auto",
        colNames: ['CriticismID',
                    '評鑒項目名稱'

                    ],
        colModel: [
            { name: 'CriticismID', index: 'CriticismID', width: 200, sortable: false, hidden: true },
            { name: 'CriticismName', index: 'CriticismName', width: 150, sortable: true, sorttype: 'text' }
           
        ],
        rowNum: 10,
        //rowList: [10, 20, 30],
        //sortname: '',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#CriticismgridListPager',
        //sort by reload
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
        }
    });
    gridDataList.jqGrid('navGrid', '#CriticismgridListPager', { edit: false, add: false, del: false, search: false, refresh: false });
    var InfogridDataList = $("#InfogridDataList");
    InfogridDataList.jqGrid({
        url: __WebAppPathPrefix + '/SQECriticism/LoadCriticismJSonWithFilter',
        postData: { CriticismID: "" },
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
        colNames: ['評鑒類別',
                    '評鑒單位',
                    '項目序號',
                    '評鑒項目',
                    '全項配分',
                    '適應分數',
                    '實際得分',
                    '評分比率',
                    '評鑒者'
        ],
        colModel: [
            { name: 'CriticismCategory', index: 'CriticismCategory', width: 150, sortable: true, sorttype: 'text' },
            { name: 'CriticismUnit', index: 'CriticismUnit', width: 150, sortable: true, sorttype: 'text' },
            { name: 'ItemNO', index: 'ItemNO', width: 150, sortable: true, sorttype: 'text' },
            { name: 'CriticismItem', index: 'CriticismItem', width: 150, sortable: true, sorttype: 'text' },
            { name: 'ItemPartition', index: 'ItemPartition', width: 150, sortable: true, sorttype: 'text' },
            { name: 'FitnessScore', index: 'FitnessScore', width: 150, sortable: true, sorttype: 'text' },
            { name: 'ActualScore', index: 'ActualScore', width: 150, sortable: true, sorttype: 'text' },
            { name: 'ScoringRatio', index: 'ScoringRatio', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Evaluation', index: 'Evaluation', width: 150, sortable: true, sorttype: 'text' }
                    ],
        rowNum: 20,
        //rowList: [10, 20, 30],
        sortname: 'ItemNO',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#InfogridListPager',
        //sort by reload
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
        }
    });
    InfogridDataList.jqGrid('navGrid', '#InfogridListPager', { edit: false, add: false, del: false, search: false, refresh: false });

    

    //$('#CriticismtbMain1').show();
    //$('#MapdialogData').show();
    //$('#CriticismdialogData').show();
    //$('#InfodialogData').show();
}
);