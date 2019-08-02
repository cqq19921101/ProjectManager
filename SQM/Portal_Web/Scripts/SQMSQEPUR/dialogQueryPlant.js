$(function () {
    var diaPlant = $('#dialog_VMI_QueryPlantInfo');
    var diaVMIPlantgridDataList = $('#dialog_VMI_PlantCode_gridDataList');

    //Init Function dialog Button
    //Button
    $('#dialog_btn_diaPlant_Search').button({
        label: "Query",
        icons: { primary: 'ui-icon-search' }
    });

    //After Init. to Show Menu Function Button
    $('#dialog_Plant_tbTopToolBar').show();

    //Init dialog
    diaPlant.dialog({
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
    diaVMIPlantgridDataList.jqGrid({
        url: __WebAppPathPrefix + "/SQMSQEPUR/QueryPlantSQE",
        postData: { PLANT: "" },
        mtype: "POST",
        datatype: "local",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false
        },
        colNames: ["Plant",
                    "Plant Name"],
        colModel: [
                    {
                        name: "PLANT", index: "PLANT", width: 25, sortable: false, sorttype: "text", classes: "jqGridColumnDataAsPointer",
                        formatter: function (cellvalue, option, rowobject) {
                            return cellvalue;
                        }
                    },
                    { name: "PLANT_NAME", index: "PLANT_NAME", width: 260, sortable: false, sorttype: "text" }
        ],
        onSelectRow: function (id) {
            var $this = $(this);
            var selRow = $this.jqGrid('getGridParam', 'selrow');

            if (selRow) {
                var rowData = $this.jqGrid('getRowData', selRow);
                $(__SelectorName).val(rowData.PLANT);
                diaPlant.dialog("close");
            }
        },
        width: 550,
        height: 232,
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: "PLANT",
        viewrecords: true,
        loadonce: true,
        pager: '#dialog_VMI_PlantCode_gridDataPager',
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

    diaVMIPlantgridDataList.jqGrid('navGrid', '#dialog_VMI_PlantCode_gridDataPager', { edit: false, add: false, del: false, search: false, refresh: false });
});