$(function () {
    var gridDataList = $("#gridDataList");
    var dialog = $("#dialogData");

    jQuery("#btnSubmit").click(function () {
        $(this).removeClass('ui-state-focus');
        if (confirm("Confirm to update whole portal menu and associated permissions (roles)?")) {
            $("#hidMenuJSon").val("");

            var m_array = new Array();
            var gridArr = gridDataList.getDataIDs();
            for (var iCnt = 0; iCnt < gridArr.length; iCnt++) {
                var m = new Object();
                var d = gridDataList.jqGrid('getRowData', gridArr[iCnt]);
                m.FunctionGUID = d.FunctionGUID;
                m.MenuLevel = d.MenuLevel;
                m.ParentFunctionGUID = d.ParentFunctionGUID;
                m.IntranetHref = d.IntranetHref;
                m.InternetHref = d.InternetHref;
                if (d.HrefTarget == "_self")
                    m.HrefTarget = 1;
                else
                    m.HrefTarget = 2;
                m.Title_en_US = d.Title_en_US;
                m.Title_zh_CN = d.Title_zh_CN;
                m.Title_zh_TW = d.Title_zh_TW;
                m.RolesString = d.Roles;
                m.ControllerActionString = d.ControllerActions;
                m_array.push(m);
            }
            var m_JsonString = JSON.stringify(m_array);

            $("#hidMenuJSon").val(m_JsonString);
            $("#_PostForm").submit();
        }
    });

    jQuery("#btnCreateSibling").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect')) {   //single select
            dialog.attr('Mode', "cs");
            DialogSetUIByMode(dialog.attr('Mode'));
            dialog.dialog("option", "title", "Create").dialog("open");
        }
    });

    jQuery("#btnCreateChild").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect')) {   //single select
            dialog.attr('Mode', "cc");
            DialogSetUIByMode(dialog.attr('Mode'));
            dialog.dialog("option", "title", "Create").dialog("open");
        }
    });

    jQuery("#btnEdit").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect')) {   //single select
            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                dialog.attr('Mode', "e");
                DialogSetUIByMode(dialog.attr('Mode'));
                dialog.dialog("option", "title", "Edit").dialog("open");
            } else { alert("Please select a row data to edit."); }
        }
    });

    jQuery("#btnDelete").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect')) {   //single select
            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                gridDataList.jqGrid('delTreeNode', RowId);
            } else { alert("Please select a row data to delete."); }
        }
    });

    jQuery("#btnMoveUp").click(function () {
        $(this).removeClass('ui-state-focus');
        var SelRowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (SelRowId) {
            var SelRow = gridDataList.jqGrid('getRowData', SelRowId);
            var pRowId = SelRow.ParentFunctionGUID;

            var AllRowIds = gridDataList.getDataIDs();
            var iSwapIndex = -1;
            var iSelIndex = -1;
            for (var iCnt = 0; iCnt < AllRowIds.length; iCnt++)
            {
                var r = gridDataList.jqGrid('getRowData', AllRowIds[iCnt]);
                if (r.FunctionGUID == SelRowId)
                {
                    iSelIndex = iCnt;
                    break;
                }
                if (r.ParentFunctionGUID == pRowId) iSwapIndex = iCnt;
            }
            
            if (iSwapIndex != -1)
            {
                //Swap Sort Code
                var SelSortCode = SelRow.SortCode;
                gridDataList.jqGrid('setCell', SelRowId, 'SortCode', gridDataList.jqGrid('getRowData', AllRowIds[iSwapIndex]).SortCode);
                gridDataList.jqGrid('setCell', AllRowIds[iSwapIndex], 'SortCode', SelSortCode);

                gridDataList.sortGrid('SortCode', true);
                gridDataList.jqGrid('setSelection', SelRowId);

                var newAllRowIds = gridDataList.getDataIDs();
                //Resort
                for (var iCnt = 0; iCnt < newAllRowIds.length; iCnt++) {
                    gridDataList.jqGrid('setCell', newAllRowIds[iCnt], 'SortCode', iCnt + 1);
                }
            }
        }
    });

    jQuery("#btnMoveDown").click(function () {
        $(this).removeClass('ui-state-focus');
        var SelRowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (SelRowId) {
            var SelRow = gridDataList.jqGrid('getRowData', SelRowId);
            var pRowId = SelRow.ParentFunctionGUID;

            var AllRowIds = gridDataList.getDataIDs();
            var iSwapIndex = -1;
            var iSelIndex = -1;
            for (var iCnt = 0; iCnt < AllRowIds.length; iCnt++) {
                var r = gridDataList.jqGrid('getRowData', AllRowIds[iCnt]);
                if (r.FunctionGUID == SelRowId)
                    iSelIndex = iCnt;
                else
                    if (iSelIndex != -1)
                        if (r.ParentFunctionGUID == pRowId) {
                            iSwapIndex = iCnt;
                            break;
                        }
            }

            if (iSwapIndex != -1) {
                //Swap Sort Code
                var SelSortCode = SelRow.SortCode;
                gridDataList.jqGrid('setCell', SelRowId, 'SortCode', gridDataList.jqGrid('getRowData', AllRowIds[iSwapIndex]).SortCode);
                gridDataList.jqGrid('setCell', AllRowIds[iSwapIndex], 'SortCode', SelSortCode);

                gridDataList.sortGrid('SortCode', true);
                gridDataList.jqGrid('setSelection', SelRowId);

                //Resort
                var newAllRowIds = gridDataList.getDataIDs();
                for (var iCnt = 0; iCnt < newAllRowIds.length; iCnt++) {
                    gridDataList.jqGrid('setCell', newAllRowIds[iCnt], 'SortCode', iCnt + 1);
                }
            }
        }
    });

    jQuery("#btnMoveLeft").click(function () {
        $(this).removeClass('ui-state-focus');
        var SelRowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (SelRowId) {
            var SelRow = gridDataList.jqGrid('getRowData', SelRowId);
            if (SelRow.MenuLevel > 1) {
                var pRowId = SelRow.ParentFunctionGUID;
                var SelRowFunctionGUID = SelRow.FunctionGUID;
                
                //Get Parent node's Parent Key
                var ParentGUIDofParent = "";
                var AllRowIds = gridDataList.getDataIDs();
                for (var iCnt = 0; iCnt < AllRowIds.length; iCnt++)
                {
                    var r = gridDataList.jqGrid('getRowData', AllRowIds[iCnt]);
                    if (r.FunctionGUID == pRowId)
                    {
                        ParentGUIDofParent = r.ParentFunctionGUID;
                        break;
                    }
                }

                //Count how many childs (for fix parent node's icon)
                var iChilds = 0; //iChilds==1 means no childs any more after level up.
                for (var iCnt = 0; iCnt < AllRowIds.length; iCnt++)
                    if (gridDataList.jqGrid('getRowData', AllRowIds[iCnt]).ParentFunctionGUID == SelRow.ParentFunctionGUID) iChilds++;

                //Get index of last node
                var iCurrMenuLevel = parseInt(SelRow.MenuLevel, 10);
                var iStartIndex = -1;
                var iEndIndex = -1;
                for (var iCnt = 0; iCnt < AllRowIds.length; iCnt++)
                {
                    if (iStartIndex == -1) {
                        var r = gridDataList.jqGrid('getRowData', AllRowIds[iCnt]);
                        if (r.FunctionGUID == SelRowFunctionGUID)
                        {
                            iStartIndex = iCnt;
                            iEndIndex = iCnt;
                        }
                    }
                    else
                    {
                        var r = gridDataList.jqGrid('getRowData', AllRowIds[iCnt]);
                        if (parseInt(r.MenuLevel, 10) > iCurrMenuLevel)
                            iEndIndex = iCnt;
                        else
                            break;
                    }
                }

                //fix previous node icon (if required)
                if (iChilds < 2) {
                    iStartIndex--;
                    iEndIndex = AllRowIds.length - 1;
                }

                //backup rows to be moved
                var rows = [];
                for(var iCnt = 0; iCnt<iEndIndex - iStartIndex + 1; iCnt++)
                {
                    var r = gridDataList.jqGrid('getRowData', AllRowIds[iCnt + iStartIndex]);

                    var NewMenuLevel = parseInt(r.MenuLevel, 10) - 1;
                    var PFunGUID = "";
                    var iIndexNeedSpecialCare = 0;
                    if (iChilds < 2) iIndexNeedSpecialCare = 1;
                    if (iCnt == iIndexNeedSpecialCare) {
                        if (ParentGUIDofParent == "")
                            PFunGUID = null
                        else
                            PFunGUID = ParentGUIDofParent;
                    }
                    else {
                        if (r.ParentFunctionGUID == "")
                            PFunGUID = null;
                        else
                            PFunGUID = r.ParentFunctionGUID;
                    }

                    var datarow = {
                        FunctionGUID: r.FunctionGUID,
                        MenuLevel: NewMenuLevel,
                        ParentFunctionGUID: PFunGUID,
                        Title_en_US: r.Title_en_US,
                        Title_zh_CN: r.Title_zh_CN,
                        Title_zh_TW: r.Title_zh_TW,
                        IntranetHref: r.IntranetHref,
                        InternetHref: r.InternetHref,
                        HrefTarget: r.HrefTarget == "_self" ? "1" : "2",
                        Roles: r.Roles,
                        ControllerActions: r.ControllerActions,
                        level: (NewMenuLevel - 1).toString(),
                        parent: PFunGUID,
                        //isLeaf: true,
                        expanded: true,
                        loaded: true
                    };
                    rows.push(datarow);
                }

                if (iChilds < 2)
                    for (var iCnt = iEndIndex; iCnt >= iStartIndex; iCnt--)
                        gridDataList.jqGrid('delTreeNode', AllRowIds[iCnt]);
                else
                    gridDataList.jqGrid('delTreeNode', SelRowId);

                if (iChilds < 2) {
                    var iRStart = -1;
                    var iREnd = -1;
                    var iFirstLevel = parseInt(rows[0].MenuLevel, 10);
                    for (var iCnt = 0; iCnt < rows.length; iCnt++)
                        if (iRStart == -1) {
                            if (rows[iCnt].FunctionGUID == SelRowFunctionGUID) {
                                iRStart = iCnt;
                                iREnd = iCnt;
                            }
                        }
                        else {
                            if (parseInt(rows[iCnt].MenuLevel, 10) > iFirstLevel)
                                iREnd = iCnt;
                            else
                                break;
                        }
                    
                    for (var iCnt = 0; iCnt < rows.length; iCnt++)
                        if ((iCnt < iRStart) ||(iCnt>iREnd))
                            gridDataList.jqGrid('addChildNode', rows[iCnt].FunctionGUID, rows[iCnt].ParentFunctionGUID, rows[iCnt]);
                    for (var iCnt = iRStart; iCnt <= iREnd; iCnt++)
                        gridDataList.jqGrid('addChildNode', rows[iCnt].FunctionGUID, rows[iCnt].ParentFunctionGUID, rows[iCnt]);
                }
                else
                    for (var iCnt = 0; iCnt < rows.length; iCnt++)
                        gridDataList.jqGrid('addChildNode', rows[iCnt].FunctionGUID, rows[iCnt].ParentFunctionGUID, rows[iCnt]);

                rows = [];

                //Resort
                var newAllRowIds = gridDataList.getDataIDs();
                for (var iCnt = 0; iCnt < newAllRowIds.length; iCnt++) {
                    gridDataList.jqGrid('setCell', newAllRowIds[iCnt], 'SortCode', iCnt + 1);
                }

                gridDataList.sortGrid('SortCode', true);
                gridDataList.jqGrid('setSelection', SelRowId);
            }
        }
    });

    jQuery("#btnMoveRight").click(function () {
        $(this).removeClass('ui-state-focus');
        var SelRowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (SelRowId) {
            var SelRow = gridDataList.jqGrid('getRowData', SelRowId);
            var SelRowParentFunctionGUID = SelRow.ParentFunctionGUID;
            var SelRowFunctionGUID = SelRow.FunctionGUID;

            var NewParentFunctionGUID = "";
            var AllRowIds = gridDataList.getDataIDs();
            for (var iCnt = 0; iCnt < AllRowIds.length; iCnt++) {
                var r = gridDataList.jqGrid('getRowData', AllRowIds[iCnt]);
                if (r.FunctionGUID == SelRowFunctionGUID) break;
                if (r.ParentFunctionGUID == SelRowParentFunctionGUID) NewParentFunctionGUID = r.FunctionGUID;
            }

            if (NewParentFunctionGUID != "") {
                //Get index of last node
                var iCurrMenuLevel = parseInt(SelRow.MenuLevel, 10);
                var iStartIndex = -1;
                var iEndIndex = -1;
                for (var iCnt = 0; iCnt < AllRowIds.length; iCnt++) {
                    if (iStartIndex == -1) {
                        var r = gridDataList.jqGrid('getRowData', AllRowIds[iCnt]);
                        if (r.FunctionGUID == SelRowId) {
                            iStartIndex = iCnt;
                            iEndIndex = iCnt;
                        }
                    }
                    else {
                        var r = gridDataList.jqGrid('getRowData', AllRowIds[iCnt]);
                        if (parseInt(r.MenuLevel, 10) > iCurrMenuLevel)
                            iEndIndex = iCnt;
                        else
                            break;
                    }
                }
                
                //backup rows to be moved
                var rows = [];
                for (var iCnt = 0; iCnt < iEndIndex - iStartIndex + 1; iCnt++) {
                    var r = gridDataList.jqGrid('getRowData', AllRowIds[iCnt + iStartIndex]);

                    var NewMenuLevel = parseInt(r.MenuLevel, 10) + 1;
                    var PFunGUID = "";
                    if (iCnt == 0)
                        PFunGUID = NewParentFunctionGUID;
                    else {
                        if (r.ParentFunctionGUID == "")
                            PFunGUID = null;
                        else
                            PFunGUID = r.ParentFunctionGUID;
                    }

                    var datarow = {
                        FunctionGUID: r.FunctionGUID,
                        MenuLevel: NewMenuLevel,
                        ParentFunctionGUID: PFunGUID,
                        Title_en_US: r.Title_en_US,
                        Title_zh_CN: r.Title_zh_CN,
                        Title_zh_TW: r.Title_zh_TW,
                        IntranetHref: r.IntranetHref,
                        InternetHref: r.InternetHref,
                        HrefTarget: r.HrefTarget == "_self" ? "1" : "2",
                        Roles: r.Roles,
                        ControllerActions: r.ControllerActions,
                        level: (NewMenuLevel - 1).toString(),
                        parent: PFunGUID,
                        //isLeaf: true,
                        expanded: true,
                        loaded: true

                        ,SortCode: r.SortCode
                    };
                    rows.push(datarow);
                }

                gridDataList.jqGrid('delTreeNode', SelRowId);

                for (var iCnt = 0; iCnt < rows.length; iCnt++)
                    gridDataList.jqGrid('addChildNode', rows[iCnt].FunctionGUID, rows[iCnt].ParentFunctionGUID, rows[iCnt]);

                rows = [];

                gridDataList.sortGrid('SortCode', true);
                gridDataList.jqGrid('setSelection', SelRowId);
            }
        }
    });
});