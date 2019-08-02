$(function () {
    $('#dialogRoles').dialog({
        autoOpen: false,
        resizable: false,
        width: 360,
        height: 450,
        modal: true,
        buttons: {
            'Submit': function () {
                var allRows = $('#gridRoleList').jqGrid('getDataIDs');
                
                $('#dialogABAManage #btnRoles').parent().find('span.roles').remove();
                for (var i in allRows) {
                    if ($('#gridRoleList').jqGrid('getCell', allRows[i], 'CHECK') == 'True') {
                        
                        $('#dialogABAManage #btnRoles').parent().append($('<span/>').text($('#gridRoleList').jqGrid('getCell', allRows[i], 'RoleName')).addClass('roles').attr('RoleGUID', $('#gridRoleList').jqGrid('getCell', allRows[i], 'RoleGUID')).css('text-decoration', 'underline')).append('  ');
                    }
                }

                $(this).dialog('close');
            }
        }
    });

    //Data List
    var gridRoleList = $("#gridRoleList");
    gridRoleList.jqGrid({
        url: __WebAppPathPrefix + '/VMIProcess/LoadRoles',
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
        colNames: [ '',
                    'RoleGUID',
                    'Role'],
        colModel: [
            { name: 'CHECK', width: 25, align: "center", editoptions: { value: "True:False" }, editrules: { required: true }, formatter: "checkbox", formatoptions: { disabled: false }, editable: true, sortable: false },
            { name: 'RoleGUID', index: 'RoleGUID', width: 200, sortable: false, hidden: true },
            { name: 'RoleName', index: 'RoleName', width: 280, sortable: true, sorttype: 'text' }
        ],
        rowNum: 999,
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#gridRolePager',
        beforeSelectRow: function (rowid, e) {
            var $self = $(this),
                iCol = $.jgrid.getCellIndex($(e.target).closest("td")[0]),
                cm = $self.jqGrid("getGridParam", "colModel"),
                localData = $self.jqGrid("getLocalRow", rowid);
            if (cm[iCol].name === "CHECK" && e.target.tagName.toUpperCase() === "INPUT") {
                // set local grid data
                localData.CHECK = $(e.target).is(":checked");
            }

            return true; // allow selection
        }
    });
    gridRoleList.jqGrid('navGrid', '#gridRolePager', { edit: false, add: false, del: false, search: false, refresh: false });
});