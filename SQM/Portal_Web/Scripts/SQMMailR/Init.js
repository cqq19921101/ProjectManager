$(function () {
    var gridEmail = $('#gridEmail');
    var diaToDoVDSManage = $('#dialog_VMIProcess_ToDoVDSManage');

    //Init Function Button
    $('#btnQueryEmail').button({
        label: "Send",
        //icons: { primary: 'ui-icon-search' }
    });

    $('#btnOpenQueryPlantDialog').button({
        icons: { primary: 'ui-icon-search' }
    });

    $('#btnOpenQueryVendorCodeDialog').button({
        icons: { primary: 'ui-icon-search' }
    });

    $('#btnOpenQueryEmailDialog').button({
        icons: { primary: 'ui-icon-search' }
    });

    $('#btnVDSCommit').button({
        label: 'Commit',
        icons: { primary: 'ui-icon-check' }
    });

    $('#btnVDSCommitAll').button({
        label: 'Commit All',
        icons: { primary: 'ui-icon-check' }
    });

    $('#btnExportAll').button({
        label: 'Export All',
        icons: { primary: 'ui-icon-arrowreturnthick-1-n' }
    });

    $('#btnExportVDS').button({
        label: 'Export VDS Only',
        icons: { primary: 'ui-icon-arrowreturnthick-1-n' }
    });

    // Query character to upper
    $('input#txtPlant').on('keydown keyup', function () {
        $(this).val($(this).val().toUpperCase());
    });
    $('input#txtVendorCode').on('keydown keyup', function () {
        $(this).val($(this).val().toUpperCase());
    });
    //$('input#txtEmail').on('keydown keyup', function () {
    //    $(this).val($(this).val().toUpperCase());
    //});

    //Init jqGrid
    var lang = "en-US";
    var langShort = lang.split('-')[0].toLowerCase();

    if ($.jgrid.hasOwnProperty("regional") && $.jgrid.regional.hasOwnProperty(lang)) {
        $.extend($.jgrid, $.jgrid.regional[lang]);
    } else if ($.jgrid.hasOwnProperty("regional") && $.jgrid.regional.hasOwnProperty(langShort)) {
        $.extend($.jgrid, $.jgrid.regional[langShort]);
    }
    gridEmail.jqGrid({
        url: __WebAppPathPrefix + "/SQMMailR/LoadSQMMailR",
        mtype: "POST",
        datatype: "local",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false
        },
        colNames: [
                    'VendorCode',
                    'ERP_VNAME',
                    'PlantCode',
                    'PLANT_NAME',
                    'SourcerGUID',
                    'SourcerName',
                    'SQEGUID',
                    'SQENAME',
                    'Email'
        ],
        colModel: [
            { name: 'VendorCode', index: 'VendorCode', width: 100, sortable: false },
            { name: 'ERP_VNAME', index: 'ERP_VNAME', width: 150, sortable: false},
            { name: 'PlantCode', index: 'PlantCode', width: 100, sortable: true, sorttype: 'text'},
            { name: 'PLANT_NAME', index: 'PLANT_NAME', width: 150, sortable: true, sorttype: 'text'},
            { name: 'SourcerGUID', index: 'SourcerGUID', width: 150, sortable: true, sorttype: 'text',hidden:true },
            { name: 'SourcerName', index: 'SourcerName', width: 150, sortable: true, sorttype: 'text' },
            { name: 'SQEGUID', index: 'SQEGUID', width: 150, sortable: true, sorttype: 'text',hidden:true},
            { name: 'SQENAME', index: 'SQENAME', width: 150, sortable: true, sorttype: 'text'},
            { name: 'Email', index: 'Email', width: 250, sortable: true, sorttype: 'text' },

        ],
        width: 700,
        height: 232,
        rowNum: 10,
        viewrecords: true,
        loadonce: true,
        pager: '#gridEmailListPager'
    });
    gridEmail.jqGrid('navGrid', '#gridEmailListPager', { edit: false, add: false, del: false, search: false, refresh: false });
});






 

