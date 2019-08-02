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

        //Data List
        var cn = ['SID',
                  '料號',
                  '規格描述1',
                  '規格描述2',
                  '材料類別',
                  'TB_SQM_AQL_PLANT_SID',
                  '抽樣計畫',
        ];
        var cm = [
                { name: 'SID', index: 'SID', width: 200, sortable: false, hidden: true },
                { name: 'LitNo', index: 'LitNo', width: 150, sortable: true, sorttype: 'text' },
                { name: 'Standard1', index: 'Standard1', width: 150, sortable: true, sorttype: 'text' },
                { name: 'Standard2', index: 'Standard2', width: 150, sortable: true, sorttype: 'text' },
                { name: 'Type', index: 'Type', width: 150, sortable: true, sorttype: 'text' },
                { name: 'TB_SQM_AQL_PLANT_SID', index: 'TB_SQM_AQL_PLANT_SID', width: 150, sortable: true, hidden: true },
                { name: 'PlantName', index: 'PlantName', width: 150, sortable: true, sorttype: 'text' },
        ];


        var gridDataList = $("#gridDataList");
        gridDataList.jqGrid({
            url: __WebAppPathPrefix + '/SQMBasic/LoadLitNO_PlantJSonWithFilter',
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
            width: 800,
            height: "auto",
            colNames: cn,
            colModel: cm,
            rowNum: 10,
            //rowList: [10, 20, 30],
            sortname: 'SID',
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
            }
        });
        gridDataList.jqGrid('navGrid', '#gridListPager', { edit: false, add: false, del: false, search: false, refresh: false });

        $('#tbMain1').show();
        $('#dialogData').show();
    });
