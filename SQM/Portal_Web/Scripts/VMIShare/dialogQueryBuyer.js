$(function () {
    var diaBuyer = $('#dialog_VMI_QueryBuyerInfo');
    var diaVMIBuyergridDataList = $('#dialog_VMI_BuyerCode_gridDataList');

    //Init Function dialog Button
    //Button
    $('#dialog_btn_diaBuyer_Search').button({
        label: "Query",
        icons: { primary: 'ui-icon-search' }
    });

    //After Init. to Show Menu Function Button
    $('#dialog_Buyer_tbTopToolBar').show();

    //Init dialog
    diaBuyer.dialog({
        autoOpen: false,
        height: 470,
        width: 580,
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
    diaVMIBuyergridDataList.jqGrid({
        url: __WebAppPathPrefix + "/VMICommon/QueryBuyerCodeInfoJsonWithFilter",
        postData: { BUYER: "" },
        mtype: "POST",
        datatype: "local",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false
        },
        colNames: ["Buyer",
                    "Buyer Name"],
        colModel: [
                    {
                        name: "BUYER", index: "BUYER", width: 25, sortable: false, sorttype: "text", classes: "jqGridColumnDataAsPointer",
                        formatter: function (cellvalue, option, rowobject) {
                            return cellvalue;
                        }
                    },
                    { name: "BUYER_NAME", index: "BUYER_NAME", width: 260, sortable: false, sorttype: "text" }
        ],
        onSelectRow: function (id) {
            var $this = $(this);
            var selRow = $this.jqGrid('getGridParam', 'selrow');

            if (selRow) {
                var rowData = $this.jqGrid('getRowData', selRow);
                $(__SelectorName).val(rowData.BUYER);
                diaBuyer.dialog("close");
            }
        },
        width: 550,
        height: 232,
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: "BUYER",
        viewrecords: true,
        loadonce: true,
        pager: '#dialog_VMI_BuyerCode_gridDataPager',
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

    diaVMIBuyergridDataList.jqGrid('navGrid', '#dialog_VMI_BuyerCode_gridDataPager', { edit: false, add: false, del: false, search: false, refresh: false });
});