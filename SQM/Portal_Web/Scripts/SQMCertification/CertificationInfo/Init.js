function getCriticism2() {
    //Toolbar Buttons
    $("#btnSQMCertificationSearch").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnSQMCertificationCreate").button({
        label: "Create",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnSQMCertificationViewEdit").button({
        label: "View/Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnSQMCertificationDelete").button({
        label: "Delete",
        icons: { primary: "ui-icon-trash" }
    });

    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });

    //Data List
    var gridSQMCertificationDataList = $("#gridSQMCertificationDataList");
    gridSQMCertificationDataList.jqGrid({
        url: __WebAppPathPrefix + '/SQMCertification/LoadSQMProduct1JSonWithFilter',
        postData: { SearchText: "", BasicInfoGUID: $("#gridDataListBasicInfoType").jqGrid('getRowData', $("#gridDataListBasicInfoType").jqGrid('getGridParam', 'selrow')).BasicInfoGUID},
        type: "post",
        datatype: "json",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false
        },
        width: 600,
        height: "auto",
        colNames: ['BasicInfoGUID',
                    'Certification Type',
                    '品質體系認証',
                    '认证机构',
                    '证书编号',
                    '认证日期',
                    '有效终止日期'],
        colModel: [
            {name: 'BasicInfoGUID', index: 'BasicInfoGUID', width: 200, sortable: false, hidden: true},
            { name: 'CertificationType', index: 'CertificationType', width: 200, sortable: false, hidden: true },
            { name: 'CName', index: 'CName', width: 200, sortable: true, sorttype: 'text' },
            { name: 'CertificationAuthority', index: 'CertificationAuthority', width: 150, sortable: true, sorttype: 'text' },
            { name: 'CertificationNum', index: 'CertificationNum', width: 150, sortable: true, sorttype: 'text' },
            { name: 'CertificationDate', index: 'CertificationDate', width: 150, sortable: true, sorttype: 'text' },
            { name: 'ValidDate', index: 'ValidDate', width: 150, sortable: true, sorttype: 'text' }
        ],
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'CName',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#gridSQMCertificationListPager',
        //sort by reload
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
        }
    });
    gridSQMCertificationDataList.jqGrid('navGrid', '#gridSQMCertificationListPager', { edit: false, add: false, del: false, search: false, refresh: false });

    $('#tbSQMCertificationMain1').show();
    $('#dialogSQMCertificationData').show();

    function loadTypeList() {
        $.ajax({
            url: __WebAppPathPrefix + '/SQMCertification/GetCertificationCategoryList',
            type: "post",
            dataType: 'json',
            async: false,
            success: function (data) {
                var options;
                options = "";
                for (var idx in data) {
                    options += '<option value=' + data[idx].CID + '>' + data[idx].CNAME + '</option>';
                }
                options += '<option value=-1>Other</option>';

                $('#ddlSQMCertificationCName').html(options);
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
            }
        });
    }
    loadTypeList();
    $("#txtSQMCertificationCNameInput").hide();
}
   
