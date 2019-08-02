$(function () {
    var diaSBUVendor = $('#dialog_VMI_QuerySBUVendor');
    var diaVMISBUgridDataList = $('#dialog_VMI_SBUVendor_gridDataList');

    //Init Function dialog Button
    //Button
    $('#dialog_btn_diaSBUVendor_Search').button({
        label: "Query",
        icons: { primary: 'ui-icon-search' }
    });

    //After Init. to Show Menu Function Button
    $('#dialog_SBU_VDN_tbTopToolBar').show();

    //Init dialog
    diaSBUVendor.dialog({
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
    diaVMISBUgridDataList.jqGrid({
        url: __WebAppPathPrefix + "/SQMMailR/QuerySBUVendorMail",
        postData: { SBUVendorCode: "" },
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
                        name: "ERP_VND", index: "ERP_VND", width: 60, sortable: false, sorttype: "text", classes: "jqGridColumnDataAsPointer",
                        formatter: function (cellvalue, option, rowobject) {
                            return cellvalue;
                        }
                    },
                    { name: "ERP_VNAME", index: "ERP_VNAME", width: 260, sortable: false, sorttype: "text" }
        ],
        onSelectRow: function (id) {
            var $this = $(this);
            var selRow = $this.jqGrid('getGridParam', 'selrow');

            if (selRow) {
                var rowData = $this.jqGrid('getRowData', selRow);
                $(__SelectorName).val(rowData.ERP_VND);
                diaSBUVendor.dialog("close");
            }
        },
        width: 550,
        height: 232,
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: "ERP_VND",
        viewrecords: true,
        loadonce: true,
        pager: '#dialog_VMI_SBUVendor_gridDataPager',
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

    diaVMISBUgridDataList.jqGrid('navGrid', '#dialog_VMI_SBUVendor_gridDataPager', { edit: false, add: false, del: false, search: false, refresh: false });
});