$(function () {
    var diaCorpRDSGUID = $('#dialog_VMI_QueryCorpRDSGUID');
    var diaVMIRDSgridDataList = $('#dialog_VMI_CorpRDSGUID_gridDataList');

    //Init Function dialog Button
    //Button
    $('#dialog_btn_diaCorpRDSGUID_Search').button({
        label: "Query",
        icons: { primary: 'ui-icon-search' }
    });

    //After Init. to Show Menu Function Button
    $('#dialog_Corp_VDN_RDSTopToolBar').show();

    //Init dialog
    diaCorpRDSGUID.dialog({
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
    diaVMIRDSgridDataList.jqGrid({
        url: __WebAppPathPrefix + "/SQMSQEPUR/QueryCorpMemberGUID",
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
                $(__SelectorRDS).val(rowData.NameInChinese);
                diaCorpRDSGUID.dialog("close");
            }
        },
        width: 550,
        height: 235,
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: "UserGUID",
        viewrecords: true,
        loadonce: true,
        pager: '#dialog_VMI_CorpRDSGUID_gridDataPager',
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

    diaVMIRDSgridDataList.jqGrid('navGrid', '#dialog_VMI_CorpRDSGUID_gridDataPager', { edit: false, add: false, del: false, search: false, refresh: false });
});