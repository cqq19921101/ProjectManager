var jqGridMinusWidth = 84;
var jqGridMinusHeight = 270;
var jqGridVDSHeight = 365;
var jqGridOpenPOHeight = 434;
var jqGridInventoryHeight = 388;

$(function () {
    $(window).resize(function () {
        $('#dialogQueryVDSManage').dialog('option', 'width', $(window).width() - 50);
        $('#listDaily').jqGrid('setGridWidth', ($(window).width() < 100) ? 100 : $(window).width() - jqGridMinusWidth);
        $('#listWeekly').jqGrid('setGridWidth', ($(window).width() < 100) ? 100 : $(window).width() - jqGridMinusWidth);
        $('#listMonthly').jqGrid('setGridWidth', ($(window).width() < 100) ? 100 : $(window).width() - jqGridMinusWidth);
        $('#listDWM').jqGrid('setGridWidth', ($(window).width() < 100) ? 100 : $(window).width() - jqGridMinusWidth);
        $('#listOpenPO').jqGrid('setGridWidth', ($(window).width() < 100) ? 100 : $(window).width() - jqGridMinusWidth);
        $('#listInventory').jqGrid('setGridWidth', ($(window).width() < 100) ? 100 : $(window).width() - jqGridMinusWidth);
    });

    var dialogQueryVDSManage = $('#dialogQueryVDSManage');
    var tabsQueryVDSManage = $('#tabsQueryVDSManage');

    dialogQueryVDSManage.dialog({
        autoOpen: false,
        resizable: false,
        modal: true,
        height: 660,
        open: function (event, ui) {
            $this = $(this);
            /* adjusting the dialog size for current browser */
            $this.dialog('option', 'width', ($(window).width() < 100) ? 100 : $(window).width() - 50);

            var loadingText = $('<div/>').css({
                "position": "absolute",
                "top": "45%",
                "left": "45%",
                "color": "white",
                "font-size": "30px"
            }).text('Loading...');

            var loadingMask = $('<div class="loading" />').css({
                "z-index": "999",
                "background-color": "gray",
                "width": $this.parent().width() + 7,
                "height": $this.parent().height() + 7,
                "position": "absolute",
                "border-radius": "3px",
                "top": $this.parent().css('top'),
                "left": $this.parent().css('left')
            }).append(loadingText);

            loadingMask.appendTo('body');

            setTimeout(function () {
                VDSToolBar();
                var query_type = $this.prop('QUERY_TYPE');
                switch (query_type) {
                    case 'lastVer':
                        $.ajax({
                            url: __WebAppPathPrefix + '/VMIQuery/GetQueryVDSTMType',
                            data: {
                                Plant: escape($.trim($this.prop('PLANT'))),
                                Vendor: escape($.trim($this.prop('VENDOR'))),
                                Buyer: escape($.trim($this.prop('BUYER'))),
                            },
                            type: "post",
                            dataType: 'json',
                            async: false, // if need page refresh, please remark this option
                            success: function (data) {
                                var activeTabs = [];
                                if (data != "") {
                                    var tabIndex = null;
                                    for (var i in data) {
                                        var TMType = escape($.trim(data[i].TMTYPE));

                                        switch (TMType) {
                                            case 'D':
                                                tabIndex = 0; // Daily
                                                break;
                                            case 'W':
                                                tabIndex = 1; // Weekly
                                                break;
                                            case 'M':
                                                tabIndex = 2; // Monthly
                                                break;
                                            case 'X':
                                                tabIndex = 3; // D + W + M
                                                break;
                                        }

                                        activeTabs.push(tabIndex);
                                        // Generate VDS Header Information
                                        $.ajax({
                                            url: __WebAppPathPrefix + '/VMIQuery/GetQueryVDSHeader',
                                            data: {
                                                VDS_NUM: escape($.trim(data[i].VDS_NUM)),
                                                VRSIO: escape($.trim(data[i].VRSIO))
                                            },
                                            type: "post",
                                            dataType: 'json',
                                            async: false,
                                            success: function (data) {
                                                var PLANT = $.trim(data.rows[0].PLANT);
                                                var ERP_VNAME = $.trim(data.rows[0].ERP_VNAME);
                                                var REPDT = $.trim(data.rows[0].REPDT);
                                                var TBNUM = $.trim(data.rows[0].TBNUM);
                                                var VDS_NUM = $.trim(data.rows[0].VDS_NUM);
                                                var VRSIO = $.trim(data.rows[0].VRSIO);
                                                var VENDOR = $.trim(data.rows[0].VENDOR);
                                                var BUYER = $.trim(data.rows[0].PUR_GROUP);
                                                var RELDT = $.trim(data.rows[0].RELDT);

                                                var table = "<table id='tbVDSInfo'>" +
                                                    "<tr>" +
                                                    "<td class='tdTitleAlignRight'>Plant:</td>" +
                                                    "<td id='tdPLANT' class='spanFieldValueUnderLine'>" + PLANT + "</td>" +
                                                    "<td class='tdTitleAlignRight'>Vendor Name:</td>" +
                                                    "<td id='tdERP_VNAME' class='spanFieldValueUnderLine'>" + ERP_VNAME + "</td>" +
                                                    "<td class='tdTitleAlignRight'>Report Date:</td>" +
                                                    "<td id='tdREPDT' class='spanFieldValueUnderLine'>" + REPDT + "</td>" +
                                                    "<td class='tdTitleAlignRight'>VDS Num:</td>" +
                                                    "<td id='tdVDS_NUM' class='spanFieldValueUnderLine'>" + VDS_NUM + "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                    "<td class='tdTitleAlignRight'>Vendor:</td>" +
                                                    "<td id='tdVENDOR' class='spanFieldValueUnderLine'>" + VENDOR + "</td>" +
                                                    "<td class='tdTitleAlignRight'>Buyer:</td>" +
                                                    "<td id='tdBUYER' class='spanFieldValueUnderLine'>" + BUYER + "</td>" +
                                                    "<td class='tdTitleAlignRight'>Released Time:</td>" +
                                                    "<td id='tdRELDT' class='spanFieldValueUnderLine'>" + RELDT + "</td>" +
                                                    "<td class='tdTitleAlignRight'>Version:</td>" +
                                                    "<td id='tdVRSIO' class='spanFieldValueUnderLine'>" + VRSIO + "</td>" +
                                                    "</tr>" +
                                                    "<tr style='display: none'>" +
                                                    "<td id='tdTBNUM'>" + TBNUM + "</td>" +
                                                    "</tr>" +
                                                    "</table>";

                                                var contentID = tabsQueryVDSManage.find("li:nth(" + tabIndex + ") a").attr('href');
                                                $(contentID).find('div').first().html(table);

                                                generateVDSContent(TMType, TBNUM, VDS_NUM, VRSIO);
                                            },
                                            error: function (xhr, textStatus, thrownError) {
                                                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                                            },
                                            complete: function (jqXHR, textStatus) {
                                            }
                                        });
                                    }
                                    // For Open Order TAB
                                    $('#tabsOpenPO div').first().children().remove();

                                    var table = "<table id='tbOpenPOInfo'>" +
                                        "<tr>" +
                                        "<td class='tdTitleAlignRight'>Plant:</td>" +
                                        "<td id='tdPLANT' class='spanFieldValueUnderLine'></td>" +
                                        "<td class='tdTitleAlignRight'>Vendor:</td>" +
                                        "<td id='tdVENDOR' class='spanFieldValueUnderLine'></td>" +
                                        "<td class='tdTitleAlignRight'>Vendor Name:</td>" +
                                        "<td id='tdVENDOR_NAME' class='spanFieldValueUnderLine'></td>" +
                                        "<td class='tdTitleAlignRight'>Datetime:</td>" +
                                        "<td id='tdDateTime' class='spanFieldValueUnderLine'></td>" +
                                        "</tr>" +
                                        "</table>";

                                    $('#tabsOpenPO div').first().html(table);
                                    $('#listOpenPO').jqGrid({
                                        url: __WebAppPathPrefix + '/VMIProcess/QueryOpenPODetail',
                                        mtype: 'POST',
                                        datatype: "local",
                                        jsonReader: {
                                            root: "Rows",
                                            page: "Page",
                                            total: "Total",
                                            records: "Records",
                                            repeatitems: false
                                        },
                                        colNames: [
                                            "Material",
                                            "Description",
                                            "UOM",
                                            "Size/Deimension",
                                            "PO No",
                                            "Line No",
                                            "Delivery Date",
                                            "PO Qty (A)",
                                            "Received Qty (B)",
                                            "In Transit Qty (C)",
                                            "Open Qty (A-B-C)"
                                        ],
                                        colModel: [
                                            { name: "MATERIAL", index: "MATERIAL", width: 80, sortable: false },
                                            { name: "MTLDESC", index: "MTLDESC", width: 120, sortable: false },
                                            { name: "UOM", index: "UOM", width: 80, sortable: false },
                                            { name: "MTLSZDM", index: "MTLSZDM", width: 120, sortable: false },
                                            { name: "PO_NUM", index: "PO_NUM", width: 80, sortable: false },
                                            { name: "PO_LINE", index: "PO_LINE", width: 80, sortable: false },
                                            { name: "DELIV_DATE", index: "DELIV_DATE", width: 80, sortable: false },
                                            { name: "POQTY", index: "POQTY", width: 100, sortable: false, formatter: "integer", thousandsSeparator: ",", align: "right" },
                                            { name: "GRQTY", index: "GRQTY", width: 110, sortable: false, formatter: "integer", thousandsSeparator: ",", align: "right" },
                                            { name: "TRANSIT", index: "TRANSIT", width: 110, sortable: false, formatter: "integer", thousandsSeparator: ",", align: "right" },
                                            { name: "OPENPOQTY", index: "OPENPOQTY", width: 110, sortable: false, formatter: "integer", thousandsSeparator: ",", align: "right" }
                                        ],
                                        pager: '#pagerOpenPO',
                                        viewrecords: false,
                                        shrinkToFit: false,
                                        scrollrows: true,
                                        loadonce: true,
                                        hoverrows: false,
                                        rowNum: 18,
                                        height: jqGridOpenPOHeight,
                                        gridComplete: function () {
                                            /* adjusting the jqGrid size for current browser */
                                            $('#listOpenPO').jqGrid('setGridWidth', ($(window).width() < 100) ? 100 : $(window).width() - jqGridMinusWidth);

                                            var allRows = $('#listOpenPO').jqGrid('getDataIDs');
                                            if (allRows.length > 0) {
                                                // For Open PO Header
                                                $('#tbOpenPOInfo #tdPLANT').text(dialogQueryVDSManage.prop("PLANT"));
                                                $('#tbOpenPOInfo #tdVENDOR').text(dialogQueryVDSManage.prop("VENDOR"));
                                                $('#tbOpenPOInfo #tdVENDOR_NAME').text(dialogQueryVDSManage.prop("VENDOR_NAME"));
                                                $('#tbOpenPOInfo #tdDateTime').text(dialogQueryVDSManage.prop("CURRENT_DATETIME"));
                                            }
                                            for (var i = 0; i < allRows.length; i++) {
                                                var Material = $('#listOpenPO').jqGrid('getCell', allRows[i], 'MATERIAL');
                                                if (Material.indexOf('Total') != -1) {
                                                    $('#listOpenPO').jqGrid('setCell', allRows[i], 'MATERIAL', 'Total');
                                                    $('table[aria-labelledby="gbox_listOpenPO"] tr.jqgrow[id="' + allRows[i] + '"]').css('background', '#CCFFFF');
                                                }
                                            }
                                        },
                                        loadComplete: function () {
                                            $('.loading').remove();
                                        },
                                        loadError: function () {
                                            $('.loading').remove();
                                        },
                                        loadBeforeSend: function () {
                                            var loadingText = $('<div/>').css({
                                                "position": "absolute",
                                                "top": "45%",
                                                "left": "30%",
                                                "color": "white",
                                                "font-size": "30px"
                                            }).text('It will take a while when loading Open PO...');

                                            var loadingMask = $('<div class="loading" />').css({
                                                "z-index": "999",
                                                "background-color": "gray",
                                                "width": $this.parent().width() + 7,
                                                "height": $this.parent().height() + 7,
                                                "position": "absolute",
                                                "border-radius": "3px",
                                                "top": $this.parent().css('top'),
                                                "left": $this.parent().css('left')
                                            }).append(loadingText);

                                            loadingMask.appendTo('body');
                                        }
                                    });
                                    $('#listOpenPO').jqGrid('navGrid', '#pagerOpenPO', { edit: false, add: false, del: false, search: false, refresh: false });
                                    // For Inventory TAB
                                    $('#tabsInventory div').first().children().remove();

                                    var table = "<table id='tbInventoryInfo'>" +
                                        "<tr>" +
                                        "<td class='tdTitleAlignRight'>Plant:</td>" +
                                        "<td id='tdPLANT' class='spanFieldValueUnderLine'></td>" +
                                        "<td class='tdTitleAlignRight'>Vendor:</td>" +
                                        "<td id='tdVENDOR' class='spanFieldValueUnderLine'></td>" +
                                        "<td class='tdTitleAlignRight'>Vendor Name:</td>" +
                                        "<td id='tdVENDOR_NAME' class='spanFieldValueUnderLine'></td>" +
                                        "<td class='tdTitleAlignRight'>Datetime:</td>" +
                                        "<td id='tdDateTime' class='spanFieldValueUnderLine'></td>" +
                                        "</tr>" +
                                        "</table>";

                                    $('#tabsInventory div').first().html(table);
                                    $('#listInventory').jqGrid({
                                        url: __WebAppPathPrefix + '/VMIProcess/QueryInventoryDetail',
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
                                            "Material",
                                            "Data Measures",
                                            "UOM",
                                            "Description",
                                            "Size/Dimension",
                                            "Qty"
                                        ],
                                        colModel: [
                                            { name: "MATERIAL", index: "MATERIAL", width: 140, sortable: false },
                                            { name: "MEASURES", index: "MEASURES", width: 140, sortable: false },
                                            { name: "UOM", index: "UOM", width: 80, sortable: false },
                                            { name: "MTLDESC", index: "MTLDESC", width: 300, sortable: false },
                                            { name: "MTLSZDM", index: "MTLSZDM", width: 300, sortable: false },
                                            { name: "QTY", index: "QTY", width: 80, sortable: false, formatter: 'integer', thousandsSeparator: "," }
                                        ],
                                        pager: '#pagerInventory',
                                        viewrecords: false,
                                        shrinkToFit: false,
                                        scrollrows: true,
                                        loadonce: false,
                                        height: jqGridInventoryHeight,
                                        loadComplete: function () {
                                            /* adjusting the jqGrid size for current browser */
                                            $('#listInventory').jqGrid('setGridWidth', ($(window).width() < 100) ? 100 : $(window).width() - jqGridMinusWidth);

                                            var allRows = $('#listInventory').jqGrid('getDataIDs');
                                            for (var i = 1; i <= allRows.length; i++) {
                                                if (i % 4 == 0) {
                                                    $('table[aria-labelledby="gbox_listInventory"] tr.jqgrow[id="' + i + '"]').css('background', '#CCFFFF');
                                                    var QTY = $('#listInventory').jqGrid('getCell', i, 'QTY');
                                                    if (QTY == 'NaN') {
                                                        $('tr[id=' + i + '].jqgrow td[aria-describedby="listInventory_QTY"]').text('No VDS Data');
                                                    }
                                                }
                                            }
                                        }
                                    });
                                    $('#listInventory').jqGrid('navGrid', '#pagerInventory', { edit: false, add: false, del: false, search: false, refresh: false });
                                    $.ajax({
                                        url: __WebAppPathPrefix + '/VMIProcess/QueryInventoryCPUDT',
                                        data: {
                                            PLANT: escape($.trim(dialogQueryVDSManage.prop("PLANT"))),
                                            VENDOR: escape($.trim(dialogQueryVDSManage.prop("VENDOR")))
                                        },
                                        type: "post",
                                        dataType: 'text',
                                        async: false,
                                        success: function (data) {
                                            if (data != '') {
                                                var CPUDT = data;
                                                $('#tbInventoryInfo #tdPLANT').text(dialogQueryVDSManage.prop("PLANT"));
                                                $('#tbInventoryInfo #tdVENDOR').text(dialogQueryVDSManage.prop("VENDOR"));
                                                $('#tbInventoryInfo #tdVENDOR_NAME').text(dialogQueryVDSManage.prop("VENDOR_NAME"));
                                                $('#tbInventoryInfo #tdDateTime').text(CPUDT);
                                            }
                                        },
                                        error: function (xhr, textStatus, thrownError) {
                                            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                                        },
                                        complete: function (jqXHR, textStatus) {
                                        }
                                    });
                                    // Init jQuery UI Tab
                                    if (tabsQueryVDSManage.tabs('instance') != undefined) {
                                        $('li.ui-state-disabled[role=tab]').show();
                                        tabsQueryVDSManage.tabs('destroy');
                                    }
                                    tabsQueryVDSManage.tabs({
                                        beforeActivate: function (event, ui) {
                                            switch (ui.newTab.context.innerText) {
                                                case 'Daily':
                                                    reloadVDSjqGrid(ui, 'listDaily');
                                                    break;
                                                case 'Weekly':
                                                    reloadVDSjqGrid(ui, 'listWeekly');
                                                    break;
                                                case 'Monthly':
                                                    reloadVDSjqGrid(ui, 'listMonthly');
                                                    break;
                                                case 'D+W+M':
                                                    reloadVDSjqGrid(ui, 'listDWM');
                                                    break;
                                                case 'Open PO':
                                                    $('#listOpenPO').jqGrid('clearGridData');
                                                    $('#listOpenPO').jqGrid('setGridParam', {
                                                        postData: {
                                                            PLANT: escape($.trim(dialogQueryVDSManage.prop("PLANT"))),
                                                            VENDOR: escape($.trim(dialogQueryVDSManage.prop("VENDOR")))
                                                        }
                                                    });
                                                    $('#listOpenPO').jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
                                                    break;
                                                case 'Inventory':
                                                    $('#listInventory').jqGrid('clearGridData');
                                                    $('#listInventory').jqGrid('setGridParam', {
                                                        postData: {
                                                            PLANT: escape($.trim(dialogQueryVDSManage.prop("PLANT"))),
                                                            VENDOR: escape($.trim(dialogQueryVDSManage.prop("VENDOR"))),
                                                            CPUDT: escape($('#tbInventoryInfo #tdDateTime').text())
                                                        }
                                                    });
                                                    $('#listInventory').jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
                                                    break;
                                            }
                                        }
                                    });
                                    tabsQueryVDSManage.tabs("option", "disabled", [0, 1, 2, 3]); // 1: daily 2: weekly 3: monthly 4: d+w+m
                                    for (var i in activeTabs) {
                                        tabsQueryVDSManage.tabs("enable", activeTabs[i]);
                                    }
                                    $('li.ui-state-disabled[role=tab]').hide();
                                    var firstTabIdx = $('li[role=tab]').not($('.ui-state-disabled')).first().index();
                                    tabsQueryVDSManage.tabs("option", "active", firstTabIdx);
                                    if (firstTabIdx == 0) {
                                        reloadVDSjqGrid(ui, 'listDaily');
                                    }
                                }
                            },
                            error: function (xhr, textStatus, thrownError) {
                                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                            },
                            complete: function (jqXHR, textStatus) {
                            }
                        });
                        break;
                    case 'history':
                        var activeTabs = [];
                        var tabIndex = null;
                        var TMType = escape($.trim($this.prop('TMTYPE')));
                        switch (TMType) {
                            case 'D':
                                tabIndex = 0; // Daily
                                break;
                            case 'W':
                                tabIndex = 1; // Weekly
                                break;
                            case 'M':
                                tabIndex = 2; // Monthly
                                break;
                            case 'X':
                                tabIndex = 3; // D + W + M
                                break;
                        }
                        activeTabs.push(tabIndex);
                        // Generate VDS Header Information
                        $.ajax({
                            url: __WebAppPathPrefix + '/VMIQuery/GetQueryVDSHeader',
                            data: {
                                VDS_NUM: escape($.trim($this.prop('VDS_NUM'))),
                                VRSIO: escape($.trim($this.prop('VRSIO')))
                            },
                            type: "post",
                            dataType: 'json',
                            async: false,
                            success: function (data) {
                                var PLANT = $.trim(data.rows[0].PLANT);
                                var ERP_VNAME = $.trim(data.rows[0].ERP_VNAME);
                                var REPDT = $.trim(data.rows[0].REPDT);
                                var TBNUM = $.trim(data.rows[0].TBNUM);
                                var VDS_NUM = $.trim(data.rows[0].VDS_NUM);
                                var VRSIO = $.trim(data.rows[0].VRSIO);
                                var VENDOR = $.trim(data.rows[0].VENDOR);
                                var BUYER = $.trim(data.rows[0].PUR_GROUP);
                                var RELDT = $.trim(data.rows[0].RELDT);

                                var table = "<table id='tbVDSInfo'>" +
                                    "<tr>" +
                                    "<td class='tdTitleAlignRight'>Plant:</td>" +
                                    "<td id='tdPLANT' class='spanFieldValueUnderLine'>" + PLANT + "</td>" +
                                    "<td class='tdTitleAlignRight'>Vendor Name:</td>" +
                                    "<td id='tdERP_VNAME' class='spanFieldValueUnderLine'>" + ERP_VNAME + "</td>" +
                                    "<td class='tdTitleAlignRight'>Report Date:</td>" +
                                    "<td id='tdREPDT' class='spanFieldValueUnderLine'>" + REPDT + "</td>" +
                                    "<td class='tdTitleAlignRight'>VDS Num:</td>" +
                                    "<td id='tdVDS_NUM' class='spanFieldValueUnderLine'>" + VDS_NUM + "</td>" +
                                    "</tr>" +
                                    "<tr>" +
                                    "<td class='tdTitleAlignRight'>Vendor:</td>" +
                                    "<td id='tdVENDOR' class='spanFieldValueUnderLine'>" + VENDOR + "</td>" +
                                    "<td class='tdTitleAlignRight'>Buyer:</td>" +
                                    "<td id='tdBUYER' class='spanFieldValueUnderLine'>" + BUYER + "</td>" +
                                    "<td class='tdTitleAlignRight'>Released Time:</td>" +
                                    "<td id='tdRELDT' class='spanFieldValueUnderLine'>" + RELDT + "</td>" +
                                    "<td class='tdTitleAlignRight'>Version:</td>" +
                                    "<td id='tdVRSIO' class='spanFieldValueUnderLine'>" + VRSIO + "</td>" +
                                    "</tr>" +
                                    "<tr style='display: none'>" +
                                    "<td id='tdTBNUM'>" + TBNUM + "</td>" +
                                    "</tr>" +
                                    "</table>";

                                var contentID = tabsQueryVDSManage.find("li:nth(" + tabIndex + ") a").attr('href');
                                $(contentID).find('div').first().html(table);

                                generateVDSContent(TMType, TBNUM, VDS_NUM, VRSIO);
                            },
                            error: function (xhr, textStatus, thrownError) {
                                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                            },
                            complete: function (jqXHR, textStatus) {
                            }
                        });
                        // For Open Order TAB
                        $('#tabsOpenPO div').first().children().remove();

                        var table = "<table id='tbOpenPOInfo'>" +
                            "<tr>" +
                            "<td class='tdTitleAlignRight'>Plant:</td>" +
                            "<td id='tdPLANT' class='spanFieldValueUnderLine'></td>" +
                            "<td class='tdTitleAlignRight'>Vendor:</td>" +
                            "<td id='tdVENDOR' class='spanFieldValueUnderLine'></td>" +
                            "<td class='tdTitleAlignRight'>Vendor Name:</td>" +
                            "<td id='tdVENDOR_NAME' class='spanFieldValueUnderLine'></td>" +
                            "<td class='tdTitleAlignRight'>Datetime:</td>" +
                            "<td id='tdDateTime' class='spanFieldValueUnderLine'></td>" +
                            "</tr>" +
                            "</table>";

                        $('#tabsOpenPO div').first().html(table);
                        $('#listOpenPO').jqGrid({
                            url: __WebAppPathPrefix + '/VMIProcess/QueryOpenPODetail',
                            mtype: 'POST',
                            datatype: "local",
                            jsonReader: {
                                root: "Rows",
                                page: "Page",
                                total: "Total",
                                records: "Records",
                                repeatitems: false
                            },
                            colNames: [
                                "Material",
                                "Description",
                                "UOM",
                                "Size/Deimension",
                                "PO No",
                                "Line No",
                                "Delivery Date",
                                "PO Qty (A)",
                                "Received Qty (B)",
                                "In Transit Qty (C)",
                                "Open Qty (A-B-C)"
                            ],
                            colModel: [
                                { name: "MATERIAL", index: "MATERIAL", width: 80, sortable: false },
                                { name: "MTLDESC", index: "MTLDESC", width: 120, sortable: false },
                                { name: "UOM", index: "UOM", width: 80, sortable: false },
                                { name: "MTLSZDM", index: "MTLSZDM", width: 120, sortable: false },
                                { name: "PO_NUM", index: "PO_NUM", width: 80, sortable: false },
                                { name: "PO_LINE", index: "PO_LINE", width: 80, sortable: false },
                                { name: "DELIV_DATE", index: "DELIV_DATE", width: 80, sortable: false },
                                { name: "POQTY", index: "POQTY", width: 100, sortable: false, formatter: "integer", thousandsSeparator: ",", align: "right" },
                                { name: "GRQTY", index: "GRQTY", width: 110, sortable: false, formatter: "integer", thousandsSeparator: ",", align: "right" },
                                { name: "TRANSIT", index: "TRANSIT", width: 110, sortable: false, formatter: "integer", thousandsSeparator: ",", align: "right" },
                                { name: "OPENPOQTY", index: "OPENPOQTY", width: 110, sortable: false, formatter: "integer", thousandsSeparator: ",", align: "right" }
                            ],
                            pager: '#pagerOpenPO',
                            viewrecords: false,
                            shrinkToFit: false,
                            scrollrows: true,
                            loadonce: true,
                            hoverrows: false,
                            rowNum: 18,
                            height: jqGridOpenPOHeight,
                            gridComplete: function () {
                                /* adjusting the jqGrid size for current browser */
                                $('#listOpenPO').jqGrid('setGridWidth', ($(window).width() < 100) ? 100 : $(window).width() - jqGridMinusWidth);

                                var allRows = $('#listOpenPO').jqGrid('getDataIDs');
                                if (allRows.length > 0) {
                                    // For Open PO Header
                                    $('#tbOpenPOInfo #tdPLANT').text(dialogQueryVDSManage.prop("PLANT"));
                                    $('#tbOpenPOInfo #tdVENDOR').text(dialogQueryVDSManage.prop("VENDOR"));
                                    $('#tbOpenPOInfo #tdVENDOR_NAME').text(dialogQueryVDSManage.prop("VENDOR_NAME"));
                                    $('#tbOpenPOInfo #tdDateTime').text(dialogQueryVDSManage.prop("CURRENT_DATETIME"));
                                }
                                for (var i = 0; i < allRows.length; i++) {
                                    var Material = $('#listOpenPO').jqGrid('getCell', allRows[i], 'MATERIAL');
                                    if (Material.indexOf('Total') != -1) {
                                        $('#listOpenPO').jqGrid('setCell', allRows[i], 'MATERIAL', 'Total');
                                        $('table[aria-labelledby="gbox_listOpenPO"] tr.jqgrow[id="' + allRows[i] + '"]').css('background', '#CCFFFF');
                                    }
                                }
                            },
                            loadComplete: function () {
                                $('.loading').remove();
                            },
                            loadError: function () {
                                $('.loading').remove();
                            },
                            loadBeforeSend: function () {
                                var loadingText = $('<div/>').css({
                                    "position": "absolute",
                                    "top": "45%",
                                    "left": "30%",
                                    "color": "white",
                                    "font-size": "30px"
                                }).text('It will take a while when loading Open PO...');

                                var loadingMask = $('<div class="loading" />').css({
                                    "z-index": "999",
                                    "background-color": "gray",
                                    "width": $this.parent().width() + 7,
                                    "height": $this.parent().height() + 7,
                                    "position": "absolute",
                                    "border-radius": "3px",
                                    "top": $this.parent().css('top'),
                                    "left": $this.parent().css('left')
                                }).append(loadingText);

                                loadingMask.appendTo('body');
                            }
                        });
                        $('#listOpenPO').jqGrid('navGrid', '#pagerOpenPO', { edit: false, add: false, del: false, search: false, refresh: false });
                        // For Inventory TAB
                        $('#tabsInventory div').first().children().remove();

                        var table = "<table id='tbInventoryInfo'>" +
                            "<tr>" +
                            "<td class='tdTitleAlignRight'>Plant:</td>" +
                            "<td id='tdPLANT' class='spanFieldValueUnderLine'></td>" +
                            "<td class='tdTitleAlignRight'>Vendor:</td>" +
                            "<td id='tdVENDOR' class='spanFieldValueUnderLine'></td>" +
                            "<td class='tdTitleAlignRight'>Vendor Name:</td>" +
                            "<td id='tdVENDOR_NAME' class='spanFieldValueUnderLine'></td>" +
                            "<td class='tdTitleAlignRight'>Datetime:</td>" +
                            "<td id='tdDateTime' class='spanFieldValueUnderLine'></td>" +
                            "</tr>" +
                            "</table>";

                        $('#tabsInventory div').first().html(table);
                        $('#listInventory').jqGrid({
                            url: __WebAppPathPrefix + '/VMIProcess/QueryInventoryDetail',
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
                                "Material",
                                "Data Measures",
                                "UOM",
                                "Description",
                                "Size/Dimension",
                                "Qty"
                            ],
                            colModel: [
                                { name: "MATERIAL", index: "MATERIAL", width: 140, sortable: false },
                                { name: "MEASURES", index: "MEASURES", width: 140, sortable: false },
                                { name: "UOM", index: "UOM", width: 80, sortable: false },
                                { name: "MTLDESC", index: "MTLDESC", width: 300, sortable: false },
                                { name: "MTLSZDM", index: "MTLSZDM", width: 300, sortable: false },
                                { name: "QTY", index: "QTY", width: 80, sortable: false, formatter: 'integer', thousandsSeparator: "," }
                            ],
                            pager: '#pagerInventory',
                            viewrecords: false,
                            shrinkToFit: false,
                            scrollrows: true,
                            loadonce: false,
                            height: jqGridInventoryHeight,
                            loadComplete: function () {
                                /* adjusting the jqGrid size for current browser */
                                $('#listInventory').jqGrid('setGridWidth', ($(window).width() < 100) ? 100 : $(window).width() - jqGridMinusWidth);

                                var allRows = $('#listInventory').jqGrid('getDataIDs');
                                for (var i = 1; i <= allRows.length; i++) {
                                    if (i % 4 == 0) {
                                        $('table[aria-labelledby="gbox_listInventory"] tr.jqgrow[id="' + i + '"]').css('background', '#CCFFFF');
                                        var QTY = $('#listInventory').jqGrid('getCell', i, 'QTY');
                                        if (QTY == 'NaN') {
                                            $('tr[id=' + i + '].jqgrow td[aria-describedby="listInventory_QTY"]').text('No VDS Data');
                                        }
                                    }
                                }
                            }
                        });
                        $('#listInventory').jqGrid('navGrid', '#pagerInventory', { edit: false, add: false, del: false, search: false, refresh: false });
                        $.ajax({
                            url: __WebAppPathPrefix + '/VMIProcess/QueryInventoryCPUDT',
                            data: {
                                PLANT: escape($.trim(dialogQueryVDSManage.prop("PLANT"))),
                                VENDOR: escape($.trim(dialogQueryVDSManage.prop("VENDOR")))
                            },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                if (data != '') {
                                    var CPUDT = data;
                                    $('#tbInventoryInfo #tdPLANT').text(dialogQueryVDSManage.prop("PLANT"));
                                    $('#tbInventoryInfo #tdVENDOR').text(dialogQueryVDSManage.prop("VENDOR"));
                                    $('#tbInventoryInfo #tdVENDOR_NAME').text(dialogQueryVDSManage.prop("VENDOR_NAME"));
                                    $('#tbInventoryInfo #tdDateTime').text(CPUDT);
                                }
                            },
                            error: function (xhr, textStatus, thrownError) {
                                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                            },
                            complete: function (jqXHR, textStatus) {
                            }
                        });
                        // Init jQuery UI Tab
                        if (tabsQueryVDSManage.tabs('instance') != undefined) {
                            $('li.ui-state-disabled[role=tab]').show();
                            tabsQueryVDSManage.tabs('destroy');
                        }
                        tabsQueryVDSManage.tabs({
                            beforeActivate: function (event, ui) {
                                switch (ui.newTab.context.innerText) {
                                    case 'Daily':
                                        reloadVDSjqGrid(ui, 'listDaily');
                                        break;
                                    case 'Weekly':
                                        reloadVDSjqGrid(ui, 'listWeekly');
                                        break;
                                    case 'Monthly':
                                        reloadVDSjqGrid(ui, 'listMonthly');
                                        break;
                                    case 'D+W+M':
                                        reloadVDSjqGrid(ui, 'listDWM');
                                        break;
                                    case 'Open PO':
                                        $('#listOpenPO').jqGrid('clearGridData');
                                        $('#listOpenPO').jqGrid('setGridParam', {
                                            postData: {
                                                PLANT: escape($.trim(dialogQueryVDSManage.prop("PLANT"))),
                                                VENDOR: escape($.trim(dialogQueryVDSManage.prop("VENDOR")))
                                            }
                                        });
                                        $('#listOpenPO').jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
                                        break;
                                    case 'Inventory':
                                        $('#listInventory').jqGrid('clearGridData');
                                        $('#listInventory').jqGrid('setGridParam', {
                                            postData: {
                                                PLANT: escape($.trim(dialogQueryVDSManage.prop("PLANT"))),
                                                VENDOR: escape($.trim(dialogQueryVDSManage.prop("VENDOR"))),
                                                CPUDT: escape($('#tbInventoryInfo #tdDateTime').text())
                                            }
                                        });
                                        $('#listInventory').jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
                                        break;
                                }
                            }
                        });
                        tabsQueryVDSManage.tabs("option", "disabled", [0, 1, 2, 3]); // 1: daily 2: weekly 3: monthly 4: d+w+m
                        for (var i in activeTabs) {
                            tabsQueryVDSManage.tabs("enable", activeTabs[i]);
                        }
                        $('li.ui-state-disabled[role=tab]').hide();
                        var firstTabIdx = $('li[role=tab]').not($('.ui-state-disabled')).first().index();
                        tabsQueryVDSManage.tabs("option", "active", firstTabIdx);
                        if (firstTabIdx == 0) {
                            reloadVDSjqGrid(ui, 'listDaily');
                        }
                        break;
                    case 'verCompare':
                        $.ajax({
                            url: __WebAppPathPrefix + '/VMIQuery/GetQueryVDSTMType',
                            data: {
                                Plant: escape($.trim($this.prop('PLANT'))),
                                Vendor: escape($.trim($this.prop('VENDOR'))),
                                Buyer: escape($.trim($this.prop('BUYER'))),
                            },
                            type: "post",
                            dataType: 'json',
                            async: false, // if need page refresh, please remark this option
                            success: function (data) {
                                var activeTabs = [];
                                if (data != "") {
                                    var tabIndex = null;
                                    for (var i in data) {
                                        var TMType = escape($.trim(data[i].TMTYPE));

                                        switch (TMType) {
                                            case 'D':
                                                tabIndex = 0; // Daily
                                                break;
                                            case 'W':
                                                tabIndex = 1; // Weekly
                                                break;
                                            case 'M':
                                                tabIndex = 2; // Monthly
                                                break;
                                            case 'X':
                                                tabIndex = 3; // D + W + M
                                                break;
                                        }

                                        activeTabs.push(tabIndex);
                                        // Generate VDS Header Information
                                        $.ajax({
                                            url: __WebAppPathPrefix + '/VMIQuery/GetQueryVDSHeader',
                                            data: {
                                                VDS_NUM: escape($.trim(data[i].VDS_NUM)),
                                                VRSIO: escape($.trim(data[i].VRSIO))
                                            },
                                            type: "post",
                                            dataType: 'json',
                                            async: false,
                                            success: function (data) {
                                                var PLANT = $.trim(data.rows[0].PLANT);
                                                var ERP_VNAME = $.trim(data.rows[0].ERP_VNAME);
                                                var REPDT = $.trim(data.rows[0].REPDT);
                                                var TBNUM = $.trim(data.rows[0].TBNUM);
                                                var VDS_NUM = $.trim(data.rows[0].VDS_NUM);
                                                var VRSIO = $.trim(data.rows[0].VRSIO);
                                                var VENDOR = $.trim(data.rows[0].VENDOR);
                                                var BUYER = $.trim(data.rows[0].PUR_GROUP);
                                                var RELDT = $.trim(data.rows[0].RELDT);

                                                var table = "<table id='tbVDSInfo'>" +
                                                    "<tr>" +
                                                    "<td class='tdTitleAlignRight'>Plant:</td>" +
                                                    "<td id='tdPLANT' class='spanFieldValueUnderLine'>" + PLANT + "</td>" +
                                                    "<td class='tdTitleAlignRight'>Vendor Name:</td>" +
                                                    "<td id='tdERP_VNAME' class='spanFieldValueUnderLine'>" + ERP_VNAME + "</td>" +
                                                    "<td class='tdTitleAlignRight'>Report Date:</td>" +
                                                    "<td id='tdREPDT' class='spanFieldValueUnderLine'>" + REPDT + "</td>" +
                                                    "<td class='tdTitleAlignRight'>VDS Num:</td>" +
                                                    "<td id='tdVDS_NUM' class='spanFieldValueUnderLine'>" + VDS_NUM + "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                    "<td class='tdTitleAlignRight'>Vendor:</td>" +
                                                    "<td id='tdVENDOR' class='spanFieldValueUnderLine'>" + VENDOR + "</td>" +
                                                    "<td class='tdTitleAlignRight'>Buyer:</td>" +
                                                    "<td id='tdBUYER' class='spanFieldValueUnderLine'>" + BUYER + "</td>" +
                                                    "<td class='tdTitleAlignRight'>Released Time:</td>" +
                                                    "<td id='tdRELDT' class='spanFieldValueUnderLine'>" + RELDT + "</td>" +
                                                    "<td class='tdTitleAlignRight'>Version:</td>" +
                                                    "<td id='tdVRSIO' class='spanFieldValueUnderLine'>" + VRSIO + "</td>" +
                                                    "</tr>" +
                                                    "<tr style='display: none'>" +
                                                    "<td id='tdTBNUM'>" + TBNUM + "</td>" +
                                                    "</tr>" +
                                                    "</table>";

                                                var contentID = tabsQueryVDSManage.find("li:nth(" + tabIndex + ") a").attr('href');
                                                $(contentID).find('div').first().html(table);

                                                //generateVDSContent(TMType, TBNUM, VDS_NUM, VRSIO);
                                                generateVDSVerCompareContent(TMType, TBNUM, VDS_NUM, VRSIO);
                                            },
                                            error: function (xhr, textStatus, thrownError) {
                                                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                                            },
                                            complete: function (jqXHR, textStatus) {
                                            }
                                        });
                                    }
                                    // For Open Order TAB
                                    $('#tabsOpenPO div').first().children().remove();

                                    var table = "<table id='tbOpenPOInfo'>" +
                                        "<tr>" +
                                        "<td class='tdTitleAlignRight'>Plant:</td>" +
                                        "<td id='tdPLANT' class='spanFieldValueUnderLine'></td>" +
                                        "<td class='tdTitleAlignRight'>Vendor:</td>" +
                                        "<td id='tdVENDOR' class='spanFieldValueUnderLine'></td>" +
                                        "<td class='tdTitleAlignRight'>Vendor Name:</td>" +
                                        "<td id='tdVENDOR_NAME' class='spanFieldValueUnderLine'></td>" +
                                        "<td class='tdTitleAlignRight'>Datetime:</td>" +
                                        "<td id='tdDateTime' class='spanFieldValueUnderLine'></td>" +
                                        "</tr>" +
                                        "</table>";

                                    $('#tabsOpenPO div').first().html(table);
                                    $('#listOpenPO').jqGrid({
                                        url: __WebAppPathPrefix + '/VMIProcess/QueryOpenPODetail',
                                        mtype: 'POST',
                                        datatype: "local",
                                        jsonReader: {
                                            root: "Rows",
                                            page: "Page",
                                            total: "Total",
                                            records: "Records",
                                            repeatitems: false
                                        },
                                        colNames: [
                                            "Material",
                                            "Description",
                                            "UOM",
                                            "Size/Deimension",
                                            "PO No",
                                            "Line No",
                                            "Delivery Date",
                                            "PO Qty (A)",
                                            "Received Qty (B)",
                                            "In Transit Qty (C)",
                                            "Open Qty (A-B-C)"
                                        ],
                                        colModel: [
                                            { name: "MATERIAL", index: "MATERIAL", width: 80, sortable: false },
                                            { name: "MTLDESC", index: "MTLDESC", width: 120, sortable: false },
                                            { name: "UOM", index: "UOM", width: 80, sortable: false },
                                            { name: "MTLSZDM", index: "MTLSZDM", width: 120, sortable: false },
                                            { name: "PO_NUM", index: "PO_NUM", width: 80, sortable: false },
                                            { name: "PO_LINE", index: "PO_LINE", width: 80, sortable: false },
                                            { name: "DELIV_DATE", index: "DELIV_DATE", width: 80, sortable: false },
                                            { name: "POQTY", index: "POQTY", width: 100, sortable: false, formatter: "integer", thousandsSeparator: ",", align: "right" },
                                            { name: "GRQTY", index: "GRQTY", width: 110, sortable: false, formatter: "integer", thousandsSeparator: ",", align: "right" },
                                            { name: "TRANSIT", index: "TRANSIT", width: 110, sortable: false, formatter: "integer", thousandsSeparator: ",", align: "right" },
                                            { name: "OPENPOQTY", index: "OPENPOQTY", width: 110, sortable: false, formatter: "integer", thousandsSeparator: ",", align: "right" }
                                        ],
                                        pager: '#pagerOpenPO',
                                        viewrecords: false,
                                        shrinkToFit: false,
                                        scrollrows: true,
                                        loadonce: true,
                                        hoverrows: false,
                                        rowNum: 18,
                                        height: jqGridOpenPOHeight,
                                        gridComplete: function () {
                                            /* adjusting the jqGrid size for current browser */
                                            $('#listOpenPO').jqGrid('setGridWidth', ($(window).width() < 100) ? 100 : $(window).width() - jqGridMinusWidth);

                                            var allRows = $('#listOpenPO').jqGrid('getDataIDs');
                                            if (allRows.length > 0) {
                                                // For Open PO Header
                                                $('#tbOpenPOInfo #tdPLANT').text(dialogQueryVDSManage.prop("PLANT"));
                                                $('#tbOpenPOInfo #tdVENDOR').text(dialogQueryVDSManage.prop("VENDOR"));
                                                $('#tbOpenPOInfo #tdVENDOR_NAME').text(dialogQueryVDSManage.prop("VENDOR_NAME"));
                                                $('#tbOpenPOInfo #tdDateTime').text(dialogQueryVDSManage.prop("CURRENT_DATETIME"));
                                            }
                                            for (var i = 0; i < allRows.length; i++) {
                                                var Material = $('#listOpenPO').jqGrid('getCell', allRows[i], 'MATERIAL');
                                                if (Material.indexOf('Total') != -1) {
                                                    $('#listOpenPO').jqGrid('setCell', allRows[i], 'MATERIAL', 'Total');
                                                    $('table[aria-labelledby="gbox_listOpenPO"] tr.jqgrow[id="' + allRows[i] + '"]').css('background', '#CCFFFF');
                                                }
                                            }
                                        },
                                        loadComplete: function () {
                                            $('.loading').remove();
                                        },
                                        loadError: function () {
                                            $('.loading').remove();
                                        },
                                        loadBeforeSend: function () {
                                            var loadingText = $('<div/>').css({
                                                "position": "absolute",
                                                "top": "45%",
                                                "left": "30%",
                                                "color": "white",
                                                "font-size": "30px"
                                            }).text('It will take a while when loading Open PO...');

                                            var loadingMask = $('<div class="loading" />').css({
                                                "z-index": "999",
                                                "background-color": "gray",
                                                "width": $this.parent().width() + 7,
                                                "height": $this.parent().height() + 7,
                                                "position": "absolute",
                                                "border-radius": "3px",
                                                "top": $this.parent().css('top'),
                                                "left": $this.parent().css('left')
                                            }).append(loadingText);

                                            loadingMask.appendTo('body');
                                        }
                                    });
                                    $('#listOpenPO').jqGrid('navGrid', '#pagerOpenPO', { edit: false, add: false, del: false, search: false, refresh: false });
                                    // For Inventory TAB
                                    $('#tabsInventory div').first().children().remove();

                                    var table = "<table id='tbInventoryInfo'>" +
                                        "<tr>" +
                                        "<td class='tdTitleAlignRight'>Plant:</td>" +
                                        "<td id='tdPLANT' class='spanFieldValueUnderLine'></td>" +
                                        "<td class='tdTitleAlignRight'>Vendor:</td>" +
                                        "<td id='tdVENDOR' class='spanFieldValueUnderLine'></td>" +
                                        "<td class='tdTitleAlignRight'>Vendor Name:</td>" +
                                        "<td id='tdVENDOR_NAME' class='spanFieldValueUnderLine'></td>" +
                                        "<td class='tdTitleAlignRight'>Datetime:</td>" +
                                        "<td id='tdDateTime' class='spanFieldValueUnderLine'></td>" +
                                        "</tr>" +
                                        "</table>";

                                    $('#tabsInventory div').first().html(table);
                                    $('#listInventory').jqGrid({
                                        url: __WebAppPathPrefix + '/VMIProcess/QueryInventoryDetail',
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
                                            "Material",
                                            "Data Measures",
                                            "UOM",
                                            "Description",
                                            "Size/Dimension",
                                            "Qty"
                                        ],
                                        colModel: [
                                            { name: "MATERIAL", index: "MATERIAL", width: 140, sortable: false },
                                            { name: "MEASURES", index: "MEASURES", width: 140, sortable: false },
                                            { name: "UOM", index: "UOM", width: 80, sortable: false },
                                            { name: "MTLDESC", index: "MTLDESC", width: 300, sortable: false },
                                            { name: "MTLSZDM", index: "MTLSZDM", width: 300, sortable: false },
                                            { name: "QTY", index: "QTY", width: 80, sortable: false, formatter: 'integer', thousandsSeparator: "," }
                                        ],
                                        pager: '#pagerInventory',
                                        viewrecords: false,
                                        shrinkToFit: false,
                                        scrollrows: true,
                                        loadonce: false,
                                        height: jqGridInventoryHeight,
                                        loadComplete: function () {
                                            /* adjusting the jqGrid size for current browser */
                                            $('#listInventory').jqGrid('setGridWidth', ($(window).width() < 100) ? 100 : $(window).width() - jqGridMinusWidth);

                                            var allRows = $('#listInventory').jqGrid('getDataIDs');
                                            for (var i = 1; i <= allRows.length; i++) {
                                                if (i % 4 == 0) {
                                                    $('table[aria-labelledby="gbox_listInventory"] tr.jqgrow[id="' + i + '"]').css('background', '#CCFFFF');
                                                    var QTY = $('#listInventory').jqGrid('getCell', i, 'QTY');
                                                    if (QTY == 'NaN') {
                                                        $('tr[id=' + i + '].jqgrow td[aria-describedby="listInventory_QTY"]').text('No VDS Data');
                                                    }
                                                }
                                            }
                                        }
                                    });
                                    $('#listInventory').jqGrid('navGrid', '#pagerInventory', { edit: false, add: false, del: false, search: false, refresh: false });
                                    $.ajax({
                                        url: __WebAppPathPrefix + '/VMIProcess/QueryInventoryCPUDT',
                                        data: {
                                            PLANT: escape($.trim(dialogQueryVDSManage.prop("PLANT"))),
                                            VENDOR: escape($.trim(dialogQueryVDSManage.prop("VENDOR")))
                                        },
                                        type: "post",
                                        dataType: 'text',
                                        async: false,
                                        success: function (data) {
                                            if (data != '') {
                                                var CPUDT = data;
                                                $('#tbInventoryInfo #tdPLANT').text(dialogQueryVDSManage.prop("PLANT"));
                                                $('#tbInventoryInfo #tdVENDOR').text(dialogQueryVDSManage.prop("VENDOR"));
                                                $('#tbInventoryInfo #tdVENDOR_NAME').text(dialogQueryVDSManage.prop("VENDOR_NAME"));
                                                $('#tbInventoryInfo #tdDateTime').text(CPUDT);
                                            }
                                        },
                                        error: function (xhr, textStatus, thrownError) {
                                            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                                        },
                                        complete: function (jqXHR, textStatus) {
                                        }
                                    });
                                    // Init jQuery UI Tab
                                    if (tabsQueryVDSManage.tabs('instance') != undefined) {
                                        $('li.ui-state-disabled[role=tab]').show();
                                        tabsQueryVDSManage.tabs('destroy');
                                    }
                                    tabsQueryVDSManage.tabs({
                                        beforeActivate: function (event, ui) {
                                            switch (ui.newTab.context.innerText) {
                                                case 'Daily':
                                                    reloadVDSjqGrid(ui, 'listDaily');
                                                    break;
                                                case 'Weekly':
                                                    reloadVDSjqGrid(ui, 'listWeekly');
                                                    break;
                                                case 'Monthly':
                                                    reloadVDSjqGrid(ui, 'listMonthly');
                                                    break;
                                                case 'D+W+M':
                                                    reloadVDSjqGrid(ui, 'listDWM');
                                                    break;
                                                case 'Open PO':
                                                    $('#listOpenPO').jqGrid('clearGridData');
                                                    $('#listOpenPO').jqGrid('setGridParam', {
                                                        postData: {
                                                            PLANT: escape($.trim(dialogQueryVDSManage.prop("PLANT"))),
                                                            VENDOR: escape($.trim(dialogQueryVDSManage.prop("VENDOR")))
                                                        }
                                                    });
                                                    $('#listOpenPO').jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
                                                    break;
                                                case 'Inventory':
                                                    $('#listInventory').jqGrid('clearGridData');
                                                    $('#listInventory').jqGrid('setGridParam', {
                                                        postData: {
                                                            PLANT: escape($.trim(dialogQueryVDSManage.prop("PLANT"))),
                                                            VENDOR: escape($.trim(dialogQueryVDSManage.prop("VENDOR"))),
                                                            CPUDT: escape($('#tbInventoryInfo #tdDateTime').text())
                                                        }
                                                    });
                                                    $('#listInventory').jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
                                                    break;
                                            }
                                        }
                                    });
                                    tabsQueryVDSManage.tabs("option", "disabled", [0, 1, 2, 3]); // 1: daily 2: weekly 3: monthly 4: d+w+m
                                    for (var i in activeTabs) {
                                        tabsQueryVDSManage.tabs("enable", activeTabs[i]);
                                    }
                                    $('li.ui-state-disabled[role=tab]').hide();
                                    var firstTabIdx = $('li[role=tab]').not($('.ui-state-disabled')).first().index();
                                    tabsQueryVDSManage.tabs("option", "active", firstTabIdx);
                                    if (firstTabIdx == 0) {
                                        reloadVDSjqGrid(ui, 'listDaily');
                                    }
                                }
                            },
                            error: function (xhr, textStatus, thrownError) {
                                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                            },
                            complete: function (jqXHR, textStatus) {
                            }
                        });
                        break;
                }

                $('.loading').remove();
            }, 300);
        },
        close: function () {
            $('#listDaily').GridUnload();
            $('#listWeekly').GridUnload();
            $('#listMonthly').GridUnload();
            $('#listDWM').GridUnload();
            $('#listOpenPO').GridUnload();
            $('#listInventory').GridUnload();
            $('#btnExportAll').unbind('click');
            $('#btnExportVDS').unbind('click');

            if (tabsQueryVDSManage.tabs('instance') != undefined) {
                $('li.ui-state-disabled[role=tab]').show();
                tabsQueryVDSManage.tabs('destroy');
            }
            __DialogIsShownNow = false;
        }
    });
})

function generateVDSContent(TMType, TBNUM, VDS_NUM, VRSIO) {
    $.ajax({
        url: __WebAppPathPrefix + '/VMIQuery/GetQueryVDSDetail',
        data: {
            TBNUM: escape(TBNUM),
            VDS_NUM: escape(VDS_NUM),
            VRSIO: escape(VRSIO),
            TMType: escape(TMType)
        },
        type: "post",
        dataType: 'json',
        async: false,
        success: function (data) {
            var tabContentList, tabContentPager, strPager, strTAB, strList;
            colN = data.colNames;
            colM = data.colModel;

            switch (TMType) {
                case 'D':
                    strTAB = '#tabsDaily';
                    strPager = '#pagerDaily';
                    strList = 'listDaily';
                    tabContentList = $('#listDaily');
                    tabContentPager = $(strPager);
                    break;
                case 'W':
                    strTAB = '#tabsWeekly';
                    strPager = '#pagerWeekly';
                    strList = 'listWeekly';
                    tabContentList = $('#listWeekly');
                    tabContentPager = $(strPager);
                    break;
                case 'M':
                    strTAB = '#tabsMonthly';
                    strPager = '#pagerMonthly';
                    strList = 'listMonthly';
                    tabContentList = $('#listMonthly');
                    tabContentPager = $(strPager);
                    break;
                case 'X':
                    strTAB = '#tabsDWM';
                    strPager = '#pagerDWM';
                    strList = 'listDWM';
                    tabContentList = $('#listDWM');
                    tabContentPager = $(strPager);
                    break;
            }
            //tabContentList.GridUnload();
            //tabContentList.jqGrid('clearGridData'); // To prevent cache the previous page number.
            tabContentList.jqGrid({
                url: __WebAppPathPrefix + '/VMIQuery/GetQueryVDSDetail',
                mtype: 'POST',
                datatype: "local",
                jsonReader: {
                    root: "colData",
                    page: "Page",
                    total: "Total",
                    records: "Records",
                    repeatitems: false,
                },
                colNames: colN,
                colModel: colM,
                pager: strPager,
                viewrecords: false,
                shrinkToFit: false,
                scrollrows: true,
                loadonce: false,
                hoverrows: false,
                height: jqGridVDSHeight,
                gridComplete: function () {
                    var $this = $(this);
                    /* adjusting the jqGrid size for current browser */
                    $this.jqGrid('setGridWidth', ($(window).width() < 100) ? 100 : $(window).width() - jqGridMinusWidth);

                    var col = $this.jqGrid('getGridParam', 'colNames');
                    // Format Integer with thousandsSeparator
                    var startFormatCol, endFormatCol;
                    $(col).each(function (index, value) {
                        switch (TMType) {
                            case 'D':
                            case 'W':
                            case 'X':
                                if (value == 'Before') {
                                    startFormatCol = index;
                                }
                                if (value == 'L/T (Lead Time)') {
                                    endFormatCol = index;
                                }
                                break;
                            case 'M':
                                if (value == 'Approval Sheet') {
                                    startFormatCol = index + 1;
                                }
                                if (value == 'Open DN') {
                                    endFormatCol = index;
                                }
                                break;
                        }
                    });

                    for (var i = startFormatCol; i <= endFormatCol; i++) { //Except REMARK & VDS_LINE
                        $this.jqGrid('setColProp', col[i], {
                            formatter: "integer",
                            thousandsSeparator: ",",
                            align: "right"
                        });
                    }

                    var allRows = $this.jqGrid('getDataIDs');

                    for (var i = 0; i < allRows.length; i++) {
                        var rowid = allRows[i];

                        if (rowid % 3 == 1) {
                            $this.find('tr.jqgrow[id="' + rowid + '"]').css('background', '#CCFFFF');
                            $this.find('tr.jqgrow[id="' + rowid + '"] td[aria-describedby="' + strList + '_Y\\/N"]').css('text-align', 'center');
                            $this.find('tr.jqgrow[id="' + rowid + '"] td[aria-describedby="' + strList + '_Y\\/N"] input[type="checkbox"]').click(function () {
                                var checked = $(this).prop('checked');
                                var COMMIT_CTL = checked ? 1 : 0;
                                var currentRowId = $(this).closest('tr').prop('id');
                                var Material = tabContentList.find('tr.jqgrow[id="' + currentRowId + '"] td[aria-describedby="' + strList + '_Material"]').first().text();
                                var VDS_NUM = $(strTAB).find('#tbVDSInfo td#tdVDS_NUM').text();
                                var VRSIO = $(strTAB).find('#tbVDSInfo td#tdVRSIO').text();
                            });
                        }
                        else // To set Material Header Row
                        {
                            $this.find('tr.jqgrow[id="' + rowid + '"] td[aria-describedby="' + strList + '_Y\\/N"] input[type="checkbox"]').remove();
                            $this.jqGrid('setCell', rowid, 'Material', ' ');
                            $this.jqGrid('setCell', rowid, 'Status', ' ');
                            $this.jqGrid('setCell', rowid, 'Commit User', ' ');
                            $this.jqGrid('setCell', rowid, 'Commit Time', ' ');
                            $this.jqGrid('setCell', rowid, 'Uom', ' ');
                            $this.jqGrid('setCell', rowid, 'Description', ' ');
                            $this.jqGrid('setCell', rowid, 'Size/Dimension', ' ');
                            $this.jqGrid('setCell', rowid, 'Approval Sheet', ' ');
                            $this.find('tr.jqgrow[id="' + rowid + '"] td[aria-describedby="' + strList + '_After"]').text('');
                            $this.find('tr.jqgrow[id="' + rowid + '"] td[aria-describedby="' + strList + '_Plan Order"]').text('');
                            $this.find('tr.jqgrow[id="' + rowid + '"] td[aria-describedby="' + strList + '_Total"]').text('');
                            $this.find('tr.jqgrow[id="' + rowid + '"] td[aria-describedby="' + strList + '_Open PO Qty"]').text('');
                            $this.find('tr.jqgrow[id="' + rowid + '"] td[aria-describedby="' + strList + '_Open DN"]').text('');
                            $this.find('tr.jqgrow[id="' + rowid + '"] td[aria-describedby="' + strList + '_Rounding value"]').text('');
                            $this.find('tr.jqgrow[id="' + rowid + '"] td[aria-describedby="' + strList + '_MRB (Material Review Board)"]').text('');
                            $this.find('tr.jqgrow[id="' + rowid + '"] td[aria-describedby="' + strList + '_L/T (Lead Time)"]').text('');

                            if (rowid % 3 == 2) {
                                var remark = $this.jqGrid('getCell', rowid, 'REMARK');
                                $this.jqGrid('setCell', rowid, 'Material', remark);
                            }

                            if (rowid % 3 == 0) { // Cumulate Shortage
                                var beginFlag, endFlag;
                                $(col).each(function (index, value) {
                                    switch (TMType) {
                                        case 'D':
                                        case 'W':
                                        case 'X':
                                            if (value == 'Before') {
                                                beginFlag = index;
                                            }
                                            if (value == 'After') {
                                                endFlag = index - 1;
                                            }
                                            break;
                                        case 'M':
                                            if (value == 'Approval Sheet') {
                                                beginFlag = index + 1;
                                            }
                                            if (value == 'Open DN') {
                                                endFlag = index - 1;
                                            }
                                            break;
                                    }
                                });

                                for (var j = beginFlag; j <= endFlag; j++) {
                                    var commit = $this.jqGrid('getCell', rowid - 1, j);
                                    var demand = $this.jqGrid('getCell', rowid - 2, j);
                                    var acculmateShortage = $this.jqGrid('getCell', rowid, j - 1);
                                    acculmateShortage = ($.trim(acculmateShortage) == '') ? 0 : acculmateShortage;
                                    var currentAcculmateShortage = parseFloat(commit) - parseFloat(demand) + parseFloat(acculmateShortage);
                                    currentAcculmateShortage = currentAcculmateShortage.toFixed(3); // 0.000

                                    if (currentAcculmateShortage < 0) {
                                        $this.jqGrid('setCell', rowid, j, currentAcculmateShortage, { color: "red" });
                                    }
                                    else {
                                        $this.jqGrid('setCell', rowid, j, currentAcculmateShortage);
                                    }
                                }

                                var totalDemand = 0;
                                if (TMType == 'D') {
                                    for (var j = beginFlag; j <= endFlag + 2; j++) { // BEFQTY ~ PLNQTY
                                        totalDemand = totalDemand + parseInt($this.jqGrid('getCell', rowid - 2, j));
                                    }
                                    $this.jqGrid('setCell', rowid - 2, 'Total', totalDemand);
                                }
                                else if (TMType == 'W') {
                                    for (var j = beginFlag; j <= endFlag + 1; j++) { // BEFQTY ~ AFTQTY
                                        totalDemand = totalDemand + parseInt($this.jqGrid('getCell', rowid - 2, j));
                                    }
                                    $this.jqGrid('setCell', rowid - 2, 'Total', totalDemand);
                                }
                            }
                        }

                    }

                    $this.jqGrid().hideCol('REMARK');
                    $this.jqGrid().hideCol('Y/N');
                    $this.jqGrid().hideCol('VDS_LINE');
                }
            });
            tabContentList.jqGrid('navGrid', '#pager', { edit: false, add: false, del: false, search: false, refresh: false });
            tabContentList.jqGrid('destroyFrozenColumns').jqGrid('setFrozenColumns'); // Important!!! Need put set forzen here, otherwise the columns layout will have problems...
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
}

function generateVDSVerCompareContent(TMType, TBNUM, VDS_NUM, VRSIO) {
    $.ajax({
        url: __WebAppPathPrefix + '/VMIQuery/GetQueryVDSVerCompareDetail',
        data: {
            TBNUM: escape(TBNUM),
            VDS_NUM: escape(VDS_NUM),
            VRSIO: escape(VRSIO),
            TMType: escape(TMType)
        },
        type: "post",
        dataType: 'json',
        async: false,
        success: function (data) {
            var tabContentList, tabContentPager, strPager, strTAB, strList;
            colN = data.colNames;
            colM = data.colModel;

            switch (TMType) {
                case 'D':
                    strTAB = '#tabsDaily';
                    strPager = '#pagerDaily';
                    strList = 'listDaily';
                    tabContentList = $('#listDaily');
                    tabContentPager = $(strPager);
                    break;
                case 'W':
                    strTAB = '#tabsWeekly';
                    strPager = '#pagerWeekly';
                    strList = 'listWeekly';
                    tabContentList = $('#listWeekly');
                    tabContentPager = $(strPager);
                    break;
                case 'M':
                    strTAB = '#tabsMonthly';
                    strPager = '#pagerMonthly';
                    strList = 'listMonthly';
                    tabContentList = $('#listMonthly');
                    tabContentPager = $(strPager);
                    break;
                case 'X':
                    strTAB = '#tabsDWM';
                    strPager = '#pagerDWM';
                    strList = 'listDWM';
                    tabContentList = $('#listDWM');
                    tabContentPager = $(strPager);
                    break;
            }
            //tabContentList.GridUnload();
            //tabContentList.jqGrid('clearGridData'); // To prevent cache the previous page number.
            tabContentList.jqGrid({
                url: __WebAppPathPrefix + '/VMIQuery/GetQueryVDSVerCompareDetail',
                mtype: 'POST',
                datatype: "local",
                jsonReader: {
                    root: "colData",
                    page: "Page",
                    total: "Total",
                    records: "Records",
                    repeatitems: false,
                },
                colNames: colN,
                colModel: colM,
                pager: strPager,
                viewrecords: false,
                shrinkToFit: false,
                scrollrows: true,
                loadonce: false,
                hoverrows: false,
                height: jqGridVDSHeight + 70,
                gridComplete: function () {
                    var $this = $(this);
                    /* adjusting the jqGrid size for current browser */
                    $this.jqGrid('setGridWidth', ($(window).width() < 100) ? 100 : $(window).width() - jqGridMinusWidth);

                    var col = $this.jqGrid('getGridParam', 'colNames');
                    // Format Integer with thousandsSeparator
                    var startFormatCol, endFormatCol;
                    $(col).each(function (index, value) {
                        switch (TMType) {
                            case 'D':
                            case 'W':
                            case 'X':
                                if (value == 'Before') {
                                    startFormatCol = index;
                                }
                                if (value == 'L/T (Lead Time)') {
                                    endFormatCol = index;
                                }
                                break;
                            case 'M':
                                if (value == 'Approval Sheet') {
                                    startFormatCol = index + 1;
                                }
                                if (value == 'Open DN') {
                                    endFormatCol = index;
                                }
                                break;
                        }
                    });

                    for (var i = startFormatCol; i <= endFormatCol; i++) { //Except REMARK & VDS_LINE
                        $this.jqGrid('setColProp', col[i], {
                            formatter: 'integer',
                            formatoptions: {
                                thousandsSeparator: ',',
                                defaultValue: ''
                            },
                            align: "right"
                        });
                    }

                    var allRows = $this.jqGrid('getDataIDs');
                    for (var i = 0; i < allRows.length; i++) {
                        var rowid = allRows[i];
                        if (rowid % 6 == 1) {
                            $this.find('tr.jqgrow[id="' + rowid + '"]').css('background', '#CCFFFF');
                        }
                        else {
                            $this.find('tr.jqgrow[id="' + rowid + '"] td[aria-describedby="' + strList + '_Y\\/N"] input[type="checkbox"]').remove();
                            $this.jqGrid('setCell', rowid, 'Material', ' ');
                            $this.jqGrid('setCell', rowid, 'Status', ' ');
                            $this.jqGrid('setCell', rowid, 'Commit User', ' ');
                            $this.jqGrid('setCell', rowid, 'Commit Time', ' ');
                            $this.jqGrid('setCell', rowid, 'Uom', ' ');
                            $this.jqGrid('setCell', rowid, 'Description', ' ');
                            $this.jqGrid('setCell', rowid, 'Size/Dimension', ' ');
                            $this.jqGrid('setCell', rowid, 'Approval Sheet', ' ');
                            $this.find('tr.jqgrow[id="' + rowid + '"] td[aria-describedby="' + strList + '_After"]').text('');
                            $this.find('tr.jqgrow[id="' + rowid + '"] td[aria-describedby="' + strList + '_Plan Order"]').text('');
                            $this.find('tr.jqgrow[id="' + rowid + '"] td[aria-describedby="' + strList + '_Total"]').text('');
                            $this.find('tr.jqgrow[id="' + rowid + '"] td[aria-describedby="' + strList + '_Open PO Qty"]').text('');
                            $this.find('tr.jqgrow[id="' + rowid + '"] td[aria-describedby="' + strList + '_Open DN"]').text('');
                            $this.find('tr.jqgrow[id="' + rowid + '"] td[aria-describedby="' + strList + '_Rounding value"]').text('');
                            $this.find('tr.jqgrow[id="' + rowid + '"] td[aria-describedby="' + strList + '_MRB (Material Review Board)"]').text('');
                            $this.find('tr.jqgrow[id="' + rowid + '"] td[aria-describedby="' + strList + '_L/T (Lead Time)"]').text('');

                            if (rowid % 6 == 2) {
                                var remark = $this.jqGrid('getCell', rowid, 'REMARK');
                                $this.jqGrid('setCell', rowid, 'Material', remark);
                            }

                            if (rowid % 6 == 0) {
                                var beginFlag, endFlag;
                                $(col).each(function (index, value) {
                                    switch (TMType) {
                                        case 'D':
                                        case 'W':
                                        case 'X':
                                            if (value == 'Before') {
                                                beginFlag = index;
                                            }
                                            if (value == 'After') {
                                                endFlag = index - 1;
                                            }
                                            break;
                                        case 'M':
                                            if (value == 'Approval Sheet') {
                                                beginFlag = index + 1;
                                            }
                                            if (value == 'Open DN') {
                                                endFlag = index - 1;
                                            }
                                            break;
                                    }
                                });
                                for (var j = beginFlag; j <= endFlag; j++) {
                                    var commit = $this.jqGrid('getCell', rowid - 4, j);
                                    var demand = $this.jqGrid('getCell', rowid - 5, j);
                                    var acculmateShortage = $this.jqGrid('getCell', rowid - 2, j - 1);
                                    acculmateShortage = ($.trim(acculmateShortage) == '') ? 0 : acculmateShortage;
                                    var currentAcculmateShortage = parseFloat(commit) - parseFloat(demand) + parseFloat(acculmateShortage);
                                    currentAcculmateShortage = currentAcculmateShortage.toFixed(3); // 0.000

                                    if (currentAcculmateShortage < 0) {
                                        $this.jqGrid('setCell', rowid - 2, j, currentAcculmateShortage, { color: "red" });
                                    }
                                    else {
                                        $this.jqGrid('setCell', rowid - 2, j, currentAcculmateShortage);
                                    }
                                }

                                var totalDemand = 0;
                                if (TMType == 'D') {
                                    for (var j = beginFlag; j <= endFlag + 2; j++) { // BEFQTY ~ PLNQTY
                                        totalDemand = totalDemand + parseInt($this.jqGrid('getCell', rowid - 5, j));
                                    }
                                    $this.jqGrid('setCell', rowid - 5, 'Total', totalDemand);
                                }
                                else if (TMType == 'W') {
                                    for (var j = beginFlag; j <= endFlag + 1; j++) { // BEFQTY ~ AFTQTY                                      
                                        totalDemand = totalDemand + parseInt($this.jqGrid('getCell', rowid - 5, j));
                                    }
                                    $this.jqGrid('setCell', rowid - 5, 'Total', totalDemand);
                                }
                            }
                        }
                    }
                    $this.jqGrid().hideCol('REMARK');
                    $this.jqGrid().hideCol('Y/N');
                    $this.jqGrid().hideCol('VDS_LINE');
                }
            });
            tabContentList.jqGrid('navGrid', '#pager', { edit: false, add: false, del: false, search: false, refresh: false });
            tabContentList.jqGrid('destroyFrozenColumns').jqGrid('setFrozenColumns'); // Important!!! Need put set forzen here, otherwise the columns layout will have problems...
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
}

Number.prototype.padLeft = function (base, chr) {
    var len = (String(base || 10).length - String(this).length) + 1;
    return len > 0 ? new Array(len).join(chr || '0') + this : this;
}

function reloadVDSjqGrid(ui, list) {
    if (ui.newPanel != undefined) {
        var tbVDSInfo = ui.newPanel.find('#tbVDSInfo');
    }
    else {
        var tbVDSInfo = $('#' + list.replace('list', 'tabs')).find('#tbVDSInfo');
    }
    var TBNUM = tbVDSInfo.find('#tdTBNUM').text();
    var VDS_NUM = tbVDSInfo.find('#tdVDS_NUM').text();
    var VRSIO = tbVDSInfo.find('#tdVRSIO').text();

    $('#' + list).jqGrid('setGridParam', {
        postData: {
            TBNUM: escape(TBNUM),
            VDS_NUM: escape(VDS_NUM),
            VRSIO: escape(VRSIO)
        }
    });
    $('#' + list).jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}