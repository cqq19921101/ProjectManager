$(function () {
    // Init Dialog
    $('#dialogQueryDailyInventory').dialog({
        autoOpen: false,
        resizable: false,
        modal: true,
        height: 640,
        open: function (event, ui) {
            $this = $(this);
            /* adjusting the dialog size for current browser */
            var dialogWidth;
            var currentWindowWidth = $(window).width();
            if (currentWindowWidth < 100) dialogWidth = 100;
            else if (currentWindowWidth > 1160) dialogWidth = 1210;
            else dialogWidth = currentWindowWidth - 50;
            $this.dialog('option', 'width', dialogWidth);

            $('#tdPlant').text($this.prop('plant'));
            $('#tdVendorCode').text($this.prop('vendorCode'));
            $('#tdVendorName').text($this.prop('vendorName'));

            $('#listDailyInventory').jqGrid('clearGridData');
            $('#listDailyInventory').jqGrid('setGridParam', {
                postData: {
                    plant: escape($.trim($this.prop('plant'))),
                    vendor: escape($.trim($this.prop('vendorCode'))),
                    fromMaterial: escape($.trim($this.prop('fromMaterial'))),
                    toMaterial: escape($.trim($this.prop('toMaterial')))
                }
            });
            $('#listDailyInventory').jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
            $('.ui-dialog :button').blur();
        },
        close: function (event, ui) {
            $('#tdPlant').text('');
            $('#tdVendorCode').text('');
            $('#tdVendorName').text('');
        }
    });
    // Init jqGrid
    $('#listDailyInventory').jqGrid({
        url: __WebAppPathPrefix + '/VMIQuery/QueryDailyInventoryjqGrid',
        mtype: 'POST',
        datatype: "local",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false,
        },
        colNames: [
            'Material',
            'Date',
            'Description',
            'UOM',
            'CPUDT7',
            'CPUDT6',
            'CPUDT5',
            'CPUDT4',
            'CPUDT3',
            'CPUDT2',
            'CPUDT1',
            'Weekly Total'
        ],
        colModel: [
            { name: "Material", sortable: false, width: 60 },
            { name: "Date", sortable: false, width: 120 },
            { name: "Description", sortable: false, width: 120 },
            { name: "UOM", sortable: false, width: 40 },
            { name: "CPUDT7", sortable: false, formatter: 'integer', formatoptions: { thousandsSeparator: ',', defaultValue: '' }, width: 95 },
            { name: "CPUDT6", sortable: false, formatter: 'integer', formatoptions: { thousandsSeparator: ',', defaultValue: '' }, width: 95 },
            { name: "CPUDT5", sortable: false, formatter: 'integer', formatoptions: { thousandsSeparator: ',', defaultValue: '' }, width: 95 },
            { name: "CPUDT4", sortable: false, formatter: 'integer', formatoptions: { thousandsSeparator: ',', defaultValue: '' }, width: 95 },
            { name: "CPUDT3", sortable: false, formatter: 'integer', formatoptions: { thousandsSeparator: ',', defaultValue: '' }, width: 95 },
            { name: "CPUDT2", sortable: false, formatter: 'integer', formatoptions: { thousandsSeparator: ',', defaultValue: '' }, width: 95 },
            { name: "CPUDT1", sortable: false, formatter: 'integer', formatoptions: { thousandsSeparator: ',', defaultValue: '' }, width: 95 },
            { name: "WeeklyTotal", sortable: false, formatter: 'integer', formatoptions: { thousandsSeparator: ',', defaultValue: '' }, width: 95 }
        ],
        pager: '#pagerlistDailyInventory',
        shrinkToFit: false,
        loadonce: false,
        height: 435,
        loadComplete: function (data) {
            /* adjusting the jqGrid size for current browser */
            var gridWidth;
            var currentWindowWidth = $(window).width();
            if (currentWindowWidth < 100) gridWidth = 100;
            else if (currentWindowWidth > 1160) gridWidth = 1160;
            else gridWidth = currentWindowWidth - 70;
            $('#listDailyInventory').jqGrid('setGridWidth', gridWidth);

            if (data.CPUDT != null) {
                $(data.CPUDT).each(function (index, date) {
                    $('#listDailyInventory').jqGrid('setLabel', "CPUDT" + (7 - index), date);
                });
            }
            var allRows = $('#listDailyInventory').jqGrid('getDataIDs');
            for (var i = 0; i < allRows.length; i++) {
                if (i % 6 == 1) {
                    $('table[aria-labelledby="gbox_listDailyInventory"] tr.jqgrow[id="' + i + '"]').css('background', '#CCFFFF');
                }
                if ($('#listDailyInventory').jqGrid('getCell', i, 'Date') == 'GI') {
                    for (var j = 4; j <= 10; j++) {
                        if ($('#listDailyInventory').jqGrid('getCell', i, j) > 0) $('#listDailyInventory').jqGrid('setCell', i, j, '', 'jqGridColumnDataAsLinkWithBlue');
                    }

                }
            }
        },
        onCellSelect: function (rowid, iCol, cellcontent, e) {
            if (iCol >= 4 && iCol <= 10 && $('#listDailyInventory').jqGrid('getCell', rowid, 'Date') == 'GI') {
                if ($('#listDailyInventory').jqGrid('getCell', rowid, iCol) > 0) {
                    var plant = $.trim($('#tdPlant').text());
                    var vendor = $.trim($('#tdVendorCode').text());
                    var material = $.trim($('#listDailyInventory').jqGrid('getCell', rowid - 3, 'Material'));
                    var date = $.trim($('#listDailyInventory').jqGrid('getGridParam', 'colNames')[iCol]);
                    $('#dialogMaterialDoc').prop({
                        plant: plant,
                        vendor: vendor,
                        material: material,
                        date: date
                    });
                    $('#dialogMaterialDoc').dialog('open');
                }
            }
        }
    });

    $('#listDailyInventory').jqGrid('navGrid', '#pagerlistDailyInventory', { edit: false, add: false, del: false, search: false, refresh: false });

    $(window).resize(function () {
        var dialogWidth, gridWidth;
        var currentWindowWidth = $(window).width();
        if (currentWindowWidth < 100) { dialogWidth = 100; gridWidth = 100; }
        else if (currentWindowWidth > 1160) { dialogWidth = 1210; gridWidth = 1160; }
        else { dialogWidth = currentWindowWidth - 50; gridWidth = currentWindowWidth - 70; }

        $('#dialogQueryDailyInventory').dialog('option', 'width', dialogWidth);
        $('#listDailyInventory').jqGrid('setGridWidth', gridWidth);
    });
})