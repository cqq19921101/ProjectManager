$(function () {
    $('#dialogGroup').dialog({
        autoOpen: false,
        resizable: false,
        height: 400,
        modal: true,
        buttons: {
            'Submit': function () {
                var gridDeptList = $('#gridDeptList'),
                selRowId = gridDeptList.jqGrid('getGridParam', 'selrow'),
                celValue = gridDeptList.jqGrid('getRowData', selRowId);

                $('#dialogABAManage #btnGroup').parent().find('span.group').remove();
                $('#dialogABAManage #btnGroup').parent().append($('<span/>').text(celValue.GROUP_NAME).addClass('group').attr('group_id', celValue.GROUP_ID).css('text-decoration', 'underline'));

                $(this).dialog('close');
            }
        }
    });

    //Data List
    var gridDeptList = $("#gridDeptList");
    gridDeptList.jqGrid({
        url: __WebAppPathPrefix + '/VMIConfigration/LoadDepartment',
        type: "post",
        datatype: "json",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false
        },
        height: "auto",
        colNames: ['GROUP_ID',
                    'GROUP_TYPE',
                    'Group Name',
                    'PARENT_GROUP_ID',
                    'LFT',
                    'RGT',
                    'LEV',
                    'isLeaf',
                    'expanded',
                    'Position'],
        colModel: [
            { name: 'GROUP_ID', index: 'GROUP_ID', width: 200, sortable: false, hidden: true },
            { name: 'GROUP_TYPE', index: 'GROUP_TYPE', width: 150, sortable: true, sorttype: 'text', hidden: true },
            { name: 'GROUP_NAME', index: 'GROUP_NAME', width: 250, sortable: true, sorttype: 'text' },
            { name: 'PARENT_GROUP_ID', index: 'PARENT_GROUP_ID', width: 105, sortable: true, sorttype: 'text', hidden: true },
            { name: 'LFT', index: 'LFT', width: 105, sortable: true, sorttype: 'text', hidden: true },
            { name: 'RGT', index: 'RGT', width: 150, sortable: false, hidden: true },
            { name: 'LEV', index: 'LEV', width: 150, sortable: false, hidden: true },
            { name: 'isLeaf', index: 'isLeaf', width: 150, sortable: false, hidden: true },
            { name: 'expanded', index: 'expanded', width: 150, sortable: false, hidden: true },
            { name: 'Position', index: 'Position', width: 150, sortable: false, hidden: true }
        ],
        loadonce: true,
        mtype: 'POST',
        treeGrid: true,
        treeGridModel: 'nested',
        ExpandColumn: 'GROUP_NAME',
        treeReader: {
            level_field: "LEV",
            left_field: "LFT",
            right_field: "RGT",
            leaf_field: "isLeaf",
            expanded_field: "expanded"
        },
        onSelectRow: function (rowid, status, e) {
            var currentNode = gridDeptList.jqGrid('getRowData', rowid);
            var parentNode = gridDeptList.jqGrid('getNodeParent', currentNode);

            if (parentNode != null && parentNode != undefined) {
                if (parseInt(currentNode.RGT) + 1 == parseInt(parentNode.RGT)) {
                    $('#btnMoveDown').prop('disabled', true);
                }
                else {
                    $('#btnMoveDown').prop('disabled', false);
                }
            }
            else {
                $('#btnMoveDown').prop('disabled', true);
            }

            if (currentNode.Position == 1) {
                $('#btnMoveUp').prop('disabled', true);
            }
            else {
                $('#btnMoveUp').prop('disabled', false);
            }
        }
    });
    gridDeptList.jqGrid('navGrid', '#gridDeptPager', { edit: false, add: false, del: false, search: false, refresh: false });
});