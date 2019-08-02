//initial dialog
$(function () {
    var dialog = $("#dialogData");

    function GetTimeStamp() {
        var temp = new Date();
        var dateStr = padStr(temp.getFullYear()) +
                        padStr(1 + temp.getMonth()) +
                        padStr(temp.getDate()) +
                        padStr(temp.getHours()) +
                        padStr(temp.getMinutes()) +
                        padStr(temp.getSeconds());
        return dateStr;
    }
    function padStr(i) { return (i < 10) ? "0" + i : "" + i; }

    var gridDataList = $("#gridDataList");
    $("#dialogData").dialog({
        autoOpen: false,
        height: 345,
        width: 510,
        resizable: false,
        modal: true,
        buttons: {
            OK: function () {
                var m = "";
                if ($.trim($("#txtTitle_en_US").val()) == "") m += "<br />Must provide Title (en-US)."
                if ($.trim($("#txtTitle_zh_CN").val()) == "") m += "<br />nMust provide Title (zh-CN)."
                if ($.trim($("#txtTitle_zh_TW").val()) == "") m += "<br />nMust provide Title (zh-TW)."
                if (m != "") $("#lblDiaErrMsg").html(m.substr(6, m.length - 6));
                else {
                    var CurrRowId = gridDataList.jqGrid('getGridParam', 'selrow');
                    var Mode = dialog.attr('Mode');
                    switch (Mode) {
                        case "cs": //Create Sibling
                        case "cc": //Create Child
                            var CurrRow = gridDataList.jqGrid('getRowData', CurrRowId);
                            var NewGUID = "NEWGUID:" + GetTimeStamp();
                            var PFunGUID, MenuLevel;

                            if (CurrRowId) {
                                if (Mode == "cs") {
                                    PFunGUID = CurrRow.ParentFunctionGUID;
                                    MenuLevel = CurrRow.MenuLevel;
                                }
                                else {
                                    PFunGUID = CurrRow.FunctionGUID;
                                    MenuLevel = (parseInt(CurrRow.MenuLevel, 10) + 1).toString();
                                }
                            }
                            else {
                                //treat as "Create level 1 node"
                                PFunGUID = null;
                                MenuLevel = "1";
                            }

                            var level = MenuLevel - 1;
                            var datarow = {
                                FunctionGUID: NewGUID,
                                MenuLevel: MenuLevel,
                                ParentFunctionGUID: PFunGUID,
                                Title_en_US: $.trim($("#txtTitle_en_US").val()),
                                Title_zh_CN: $.trim($("#txtTitle_zh_CN").val()),
                                Title_zh_TW: $.trim($("#txtTitle_zh_TW").val()),
                                IntranetHref: $.trim($("#txtIntranetHref").val()),
                                InternetHref: $.trim($("#txtInternetHref").val()),
                                HrefTarget: $('input[name=DiaHrefTarget]:checked').val() == "_self" ? "1" : "2",
                                Roles: "",
                                ControllerActions: "",
                                level: level.toString(),
                                parent: PFunGUID,
                                isLeaf: true,
                                expanded: true,
                                loaded: true
                            };

                            gridDataList.jqGrid('addChildNode', NewGUID, PFunGUID, datarow);
                            break;
                        default:
                            var CurrRow = gridDataList.jqGrid('getRowData');
                            var datarow = {
                                FunctionGUID: CurrRow.FunctionGUID,
                                MenuLevel: CurrRow.MenuLevel,
                                ParentFunctionGUID: CurrRow.ParentFunctionGUID,
                                Title_en_US: $.trim($("#txtTitle_en_US").val()),
                                Title_zh_CN: $.trim($("#txtTitle_zh_CN").val()),
                                Title_zh_TW: $.trim($("#txtTitle_zh_TW").val()),
                                IntranetHref: $.trim($("#txtIntranetHref").val()),
                                InternetHref: $.trim($("#txtInternetHref").val()),
                                HrefTarget: $('input[name=DiaHrefTarget]:checked').val() == "_self" ? "1" : "2",
                                Roles: CurrRow.Roles,
                                ControllerActions: CurrRow.ControllerActions,
                                level: CurrRow.level,
                                parent: CurrRow.parent,
                                isLeaf: CurrRow.isLeaf,
                                expanded: CurrRow.expanded,
                                loaded: true
                            };
                            gridDataList.jqGrid('setTreeRow', CurrRowId, datarow);
                            break;
                    }
                    $(this).dialog("close");
                }
            },
            Cancel: function () { $(this).dialog("close"); }
        },
        close: function () { }
    });
});

//change dialog UI
// c: Create, v: View, e: Edit
function DialogSetUIByMode(Mode) {
    var dialog = $("#dialogData");
    var gridDataList = $("#gridDataList");
    switch (Mode) {
        case "cs": //Create Sibling
        case "cc": //Create Child
            $("#txtTitle_en_US").val("");
            $("#txtTitle_zh_CN").val("");
            $("#txtTitle_zh_TW").val("");
            $("#txtIntranetHref").val("");
            $("#txtInternetHref").val("");
            $("#radDiaHrefTargetSelf").attr('checked', true);
            $("#lblDiaErrMsg").html("");
            break;
        default: //Edit("e")
            var dataRow = gridDataList.jqGrid('getRowData', gridDataList.jqGrid('getGridParam', 'selrow'));
            $("#txtTitle_en_US").val(dataRow.Title_en_US);
            $("#txtTitle_zh_CN").val(dataRow.Title_zh_CN);
            $("#txtTitle_zh_TW").val(dataRow.Title_zh_TW);
            $("#txtIntranetHref").val(dataRow.IntranetHref);
            $("#txtInternetHref").val(dataRow.InternetHref);
            if (dataRow.HrefTarget == "_self")
                $("#radDiaHrefTargetSelf").attr('checked', true);
            else
                $("#radDiaHrefTargetBlank").attr('checked', true);
            $("#lblDiaErrMsg").html("");
            break;
    }
}