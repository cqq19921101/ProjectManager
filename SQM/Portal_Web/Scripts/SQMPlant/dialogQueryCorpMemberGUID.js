$(function () {
    var diaCorpMemberGUID = $('#dialog_VMI_QueryCorpMemberGUID');
    var diaVMIMGgridDataList = $('#dialog_VMI_CorpMemberGUID_gridDataList');

    //Init Function dialog Button
    //Button
    $('#dialog_btn_diaCorpMemberGUID_Search').button({
        label: "Query",
        icons: { primary: 'ui-icon-search' }
    });

    //After Init. to Show Menu Function Button
    $('#dialog_Corp_VDN_MGTopToolBar').show();

    //Init dialog
    diaCorpMemberGUID.dialog({
        autoOpen: false,
        height: 490,
        width: 610,
        resizable: false,
        modal: true,
        buttons: {
            Close: function () {
                $(this).dialog("close");
            }
        },
        close: function () {
            __DialogIsShownNow = false;
        }
    });

    //Init gridData
    diaVMIMGgridDataList.jqGrid({
        url: __WebAppPathPrefix + "/SQMPlant/QueryCorpMemberGUIDInfoJsonWithFilter",
        postData: { CorpMemberGUIDCode : ""},
        mtype: "POST",
        datatype: "local",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false
        },
        colNames: ["MemberGUID",
                    "NameInChinese"],
        colModel: [
                    {
                        name: "MemberGUID", index: "MemberGUID", width: 80, sortable: false, sorttype: "text", classes: "jqGridColumnDataAsPointer",
                        formatter: function (cellvalue, option, rowobject) {
                            return cellvalue;
                        }
                    },
                    { name: "NameInChinese", index: "NameInChinese", width: 220, sortable: false, sorttype: "text" }
        ],
        onSelectRow: function (id) {
            var $this = $(this);
            var selRow = $this.jqGrid('getGridParam', 'selrow');

            if (selRow) {
                var rowData = $this.jqGrid('getRowData', selRow);
                $(__SelectorName).val(rowData.MemberGUID);
                diaCorpMemberGUID.dialog("close");
            }
        },
        width: 550,
        height: 235,
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: "UserGUID",
        viewrecords: true,
        loadonce: true,
        pager: '#dialog_VMI_CorpMemberGUID_gridDataPager',
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

    diaVMIMGgridDataList.jqGrid('navGrid', '#dialog_VMI_CorpMemberGUID_gridDataPager', { edit: false, add: false, del: false, search: false, refresh: false });
});