$(function () {
    var diaCorpVendor = $('#dialog_VMI_QueryCorpVendor');
    var diaVMICVgridDataList = $('#dialog_VMI_CorpVendor_gridDataList');

    //Init Function dialog Button
    //Button
    $('#dialog_btn_diaCorpVendor_Search').button({
        label: "Query",
        icons: { primary: 'ui-icon-search' }
    });

    //After Init. to Show Menu Function Button
    $('#dialog_Corp_VDN_tbTopToolBar').show();

    //Init dialog
    diaCorpVendor.dialog({
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
    diaVMICVgridDataList.jqGrid({
        url: __WebAppPathPrefix + "/VMICommon/QueryCorpVendorInfoJsonWithFilter",
        postData: { CorpVendorCode : ""},
        mtype: "POST",
        datatype: "local",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false
        },
        colNames: ["Vendor Code",
                    "Vendor Name"],
        colModel: [
                    {
                        name: "CORP_VND", index: "CORP_VND", width: 80, sortable: false, sorttype: "text", classes: "jqGridColumnDataAsPointer",
                        formatter: function (cellvalue, option, rowobject) {
                            return cellvalue;
                        }
                    },
                    { name: "CORP_VNAME", index: "CORP_VNAME", width: 220, sortable: false, sorttype: "text" }
        ],
        onSelectRow: function (id) {
            var $this = $(this);
            var selRow = $this.jqGrid('getGridParam', 'selrow');

            if (selRow) {
                var rowData = $this.jqGrid('getRowData', selRow);
                $(__SelectorName).val(rowData.CORP_VND);
                diaCorpVendor.dialog("close");
            }
        },
        width: 550,
        height: 235,
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: "CORP_VND",
        viewrecords: true,
        loadonce: true,
        pager: '#dialog_VMI_CorpVendor_gridDataPager',
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

    diaVMICVgridDataList.jqGrid('navGrid', '#dialog_VMI_CorpVendor_gridDataPager', { edit: false, add: false, del: false, search: false, refresh: false });
});