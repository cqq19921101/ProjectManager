$(function () {
    var diaEmail = $('#dialog_VMI_QueryEmailInfo');
    var diaVMIEmailgridDataList = $('#dialog_VMI_Email_gridDataList');

    //Init Function dialog Button
    //Button
    $('#dialog_btn_diaEmail_Search').button({
        label: "Query",
        icons: { primary: 'ui-icon-search' }
    });

    //After Init. to Show Menu Function Button
    $('#dialog_Email_tbTopToolBar').show();

    //Init dialog
    diaEmail.dialog({
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
    diaVMIEmailgridDataList.jqGrid({
        url: __WebAppPathPrefix + "/SQMMailR/QueryCorpEMAIL",
        postData: { Email: "" },
        mtype: "POST",
        datatype: "local",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false
        },
        colNames: ["Email",
                    "Name"],
        colModel: [
                    {
                        name: "Email", index: "Email", width: 50, sortable: false, sorttype: "text", classes: "jqGridColumnDataAsPointer",
                        formatter: function (cellvalue, option, rowobject) {
                            return cellvalue;
                        }
                    },
                    { name: "Name", index: "Name", width: 50, sortable: false, sorttype: "text" }
        ],
        onSelectRow: function (id) {
            var $this = $(this);
            var selRow = $this.jqGrid('getGridParam', 'selrow');

            if (selRow) {
                var rowData = $this.jqGrid('getRowData', selRow);
                $(__SelectorName).val(rowData.Email);
                diaEmail.dialog("close");
            }
        },
        width: 550,
        height: 232,
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: "Email",
        viewrecords: true,
        loadonce: true,
        pager: '#dialog_VMI_Email_gridDataPager',
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

    diaVMIEmailgridDataList.jqGrid('navGrid', '#dialog_VMI_Email_gridDataPager', { edit: false, add: false, del: false, search: false, refresh: false });
});