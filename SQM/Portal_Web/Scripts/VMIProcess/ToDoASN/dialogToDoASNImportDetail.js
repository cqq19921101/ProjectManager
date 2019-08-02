var jqGridMinusWidth = 84;
var jqGridPageRowNum = 20;
var LastSelectRowID;
var idsOfSelectedRows = [];
$(function () {
    var diaToDoASNManage = $('#dialog_VMIProcess_ToDoASNManage');
    var diaToDoASNImportDetail = $('#dialog_VMIProcess_ToDoASNImportDetail');
    var diaToDoASNImportDetailShowASNQty = $('#dialog_VMIProcess_ToDoASNImportDetailShowASNQty');
    var VMI_ToDoASNImportDetailgridDataList = $('#VMI_Process_ToDoASNImportDetail_gridDataList');

    //Init Function Button
    //Button
    $('#btn_VMIProcess_ToDoASNImportDetail_Query').button({
        label: "Fast Query",
        icons: { primary: 'ui-icon-search' }
    });

    $('#btn_VMIProcess_ToDoASNImportDetail_AdvanceQuery').button({
        label: "Realtime Query",
        icons: { primary: 'ui-icon-search' }
    });

    //After Init. to Show Menu Function Button
    $('#VMIProcess_ToDoASNImportDetail_tbTopToolBar').show();


    $(window).resize(function () {
        var diaToDoASNImportDetailWidth, VMI_ToDoASNImportDetailgridDataListWidth;
        var currentWindowWidth = $(window).width();
        if (currentWindowWidth < 100) { diaToDoASNImportDetailWidth = 100; VMI_ToDoASNImportDetailgridDataListWidth = 100; }
        else if (currentWindowWidth > 1195) { diaToDoASNImportDetailWidth = 1245; VMI_ToDoASNImportDetailgridDataListWidth = 1195; }
        else { diaToDoASNImportDetailWidth = currentWindowWidth - 50; VMI_ToDoASNImportDetailgridDataListWidth = currentWindowWidth - jqGridMinusWidth; }

        diaToDoASNImportDetail.dialog('option', 'width', diaToDoASNImportDetailWidth);
        VMI_ToDoASNImportDetailgridDataList.jqGrid('setGridWidth', VMI_ToDoASNImportDetailgridDataListWidth);
    });

    //Set Tyoe Check Default Option
    var radioType = $('input:radio[name=ImportDetail_QueryType]');
    if (radioType.is(':checked') === false) {
        radioType.filter('[value=A]').prop('checked', true);
    }

    //Init dialog
    diaToDoASNImportDetail.dialog({
        autoOpen: false,
        resizable: false,
        height: 670,
        modal: true,
        open: function (event, ui) {
            $this = $(this);
            LastSelectRowID = undefined;
            idsOfSelectedRows = [];
            /* adjusting the dialog size for current browser */
            var diaToDoASNImportDetailWidth, VMI_ToDoASNImportDetailgridDataListWidth;
            var currentWindowWidth = $(window).width();
            if (currentWindowWidth < 100) { diaToDoASNImportDetailWidth = 100; VMI_ToDoASNImportDetailgridDataListWidth = 100; }
            else if (currentWindowWidth > 1195) { diaToDoASNImportDetailWidth = 1245; VMI_ToDoASNImportDetailgridDataListWidth = 1195; }
            else { diaToDoASNImportDetailWidth = currentWindowWidth - 50; VMI_ToDoASNImportDetailgridDataListWidth = currentWindowWidth - jqGridMinusWidth; }

            $this.dialog('option', 'width', diaToDoASNImportDetailWidth);
            VMI_ToDoASNImportDetailgridDataList.jqGrid('setGridWidth', VMI_ToDoASNImportDetailgridDataListWidth);
        },
        buttons: {
            Submit: function () {
                HandleLastSelectedItem();

                var DoSuccessfully = false;
                var AsnQty;
                var OpenPOQty;
                var ErrorMsg = "";
                var selectedData = [];
                var selectedrows = idsOfSelectedRows;
                if (selectedrows.length > 0) {
                    for (var i = 0; i < selectedrows.length; i++) {
                        selectedData.push(VMI_ToDoASNImportDetailgridDataList.jqGrid("getLocalRow", selectedrows[i]));
                        AsnQty = VMI_ToDoASNImportDetailgridDataList.jqGrid('getLocalRow', selectedrows[i]).Book_ASN_QTY;
                        OpenPOQty = VMI_ToDoASNImportDetailgridDataList.jqGrid('getLocalRow', selectedrows[i]).OPEN_PO_QTY;

                        if (formatFloat(AsnQty, 3) > formatFloat(OpenPOQty, 3)) {
                            if (ErrorMsg == "") {
                                ErrorMsg = "Line:";
                                ErrorMsg += selectedrows[i];
                            }
                            else
                                ErrorMsg += "," + selectedrows[i];
                            //change backgroud clolor
                            VMI_ToDoASNImportDetailgridDataList.jqGrid('setCell', selectedrows[i], "Book_ASN_QTY", "", {
                                'background-color': 'red'
                            });
                        }
                        else {
                            //change backgroud clolor
                            VMI_ToDoASNImportDetailgridDataList.jqGrid('setCell', selectedrows[i], "Book_ASN_QTY", "", {
                                'background-color': 'white'
                            });
                        }
                    }

                    if (ErrorMsg != "")
                        ErrorMsg += " ASN QTY is more than Open PO QTY";

                    if (ErrorMsg == "") {
                        $.ajax({
                            url: __WebAppPathPrefix + ((diaToDoASNImportDetail.attr('Mode') == "B") ? "/VMIProcess/CreateToDoASNImportDetailByBatchInfo" : "/VMIProcess/CreateToDoASNImportDetailByRTInfo"),
                            //data: JSON.stringify({ JsonImport: selectedData }),
                            data: {
                                ASNNUM: escape($.trim(diaToDoASNManage.prop("ASN_NUM"))),
                                ETA: escape($.trim(diaToDoASNManage.prop("ETA"))),
                                "JsonImport": JSON.stringify(selectedData)
                            },
                            type: "post",
                            dataType: 'json',
                            async: false,
                            success: function (data) {
                                if (data.Result) {
                                    DoSuccessfully = true;
                                    //EnableToDoASNFunction();
                                    bButtonFunctionEnable = true;
                                    ReloadToDoASNManagegridDataList();
                                    InitdialogToDoASNHeaderForManage();
                                }
                                else {
                                    if (data.RT == 0) {//Recheck rule
                                        CheckASNQty();
                                    }
                                    else if (data.RT == 1) {//OnlyShowMessage
                                        alert(data.Message);
                                    }
                                }
                            },
                            error: function (xhr, textStatus, thrownError) {
                                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                            },
                            complete: function (jqXHR, textStatus) {
                                //HideAjaxLoading();
                            }
                        });
                    }
                    else {
                        alert(ErrorMsg);
                    }

                    if (DoSuccessfully) {
                        $(this).dialog("close");
                    }
                }
                else {
                    alert("Please choose a Material.\nPlease check it again.");
                }
            },
            Close: function () {
                $(this).dialog("close");
            }
        },
        close: function () {
            __DialogIsShownNow = false;
        }
    });

    //Init JQgrid
    VMI_ToDoASNImportDetailgridDataList.jqGrid({
        //url: __WebAppPathPrefix + "/VMIProcess/QueryToDoASNImportDetailByBatchInfo",
        //postData: { },
        mtype: "POST",
        datatype: "local",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false
        },
        colNames: ["PO No",
                    "PO Line",
                    "Material",
                    "NETWT",
                    "MAKTX",
                    "ASN Qty",
                    "VDS Qty",
                    "OPEN PO QTY",
                    "PO QTY",
                    "GR QTY",
                    "Open ASN Qty",
                    "C/N",
                    "Handing G.",
                    "ZCUSTOMSCHK",
                    "FREEPO",
                    "UOM",
                    "UOM_RATE",
                    "TRADETERM",
                    "Customs Seal",
                    "SLoc",
                    "DOCK",
                    "ZPNAME2",
                    "WAERS"],
        colModel: [
                    { name: "PO_NUM", index: "PO_NUM", width: 90, sortable: false, sorttype: "text" },
                    { name: "PO_LINE", index: "PO_LINE", width: 50, sortable: false, sorttype: "text" },
                    { name: "MATERIAL", index: "MATERIAL", width: 120, sortable: false, sorttype: "text" },
                    { name: "NETWT", index: "NETWT", width: 120, sortable: false, hidden: true, sorttype: "text" },
                    { name: "MAKTX", index: "MAKTX", width: 250, sortable: false, sorttype: "text" },
                    {
                        name: "Book_ASN_QTY", index: "Book_ASN_QTY", width: 70, align: 'right', sortable: false, sorttype: "text", editable: true,
                        formatter: 'decimal', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 0, defaultValue: '0' },
                        editrules: { decimal: true, custom: true, custom_func: jqgirdCellPositiveNumber }
                    },
                    { name: "VDS_Qty", index: "VDS_Qty", width: 70, align: 'right', sortable: false, sorttype: "text" },
                    { name: "OPEN_PO_QTY", index: "OPEN_PO_QTY", width: 90, align: 'right', sortable: false, sorttype: "text" },
                    { name: "PO_QTY", index: "PO_QTY", width: 70, align: 'right', sortable: false, sorttype: "text" },
                    { name: "GR_QTY", index: "GR_QTY", width: 70, align: 'right', sortable: false, sorttype: "text" },
                    {
                        name: "ASN_QTY", index: "ASN_QTY", width: 90, align: 'right', sortable: false, sorttype: "text", //classes: "jqGridColumnDataAsLinkWithBlue",
                        formatter: function (cellvalue, option, rowobject) {
                            return cellvalue;
                        },
                        cellattr: function (rowId, cellValue, rawObject, cm, rdata) {
                            if (rawObject.ASN_QTY != 0) {
                                return ' class="jqGridColumnDataAsLinkWithBlue"';
                            }
                        },
                    },
                    { name: "PSTYP", index: "PSTYP", width: 70, sortable: false, sorttype: "text" },
                    { name: "KEEPER", index: "KEEPER", width: 70, sortable: false, sorttype: "text" },
                    { name: "ZCUSTOMSCHK", index: "ZCUSTOMSCHK", width: 70, sortable: false, sorttype: "text" },
                    { name: "ZFREEPO", index: "ZFREEPO", width: 70, sortable: false, sorttype: "text" },
                    { name: "UOM", index: "UOM", width: 70, sortable: false, sorttype: "text" },
                    { name: "UOM_RATE", index: "UOM_RATE", width: 70, sortable: false, sorttype: "text" },
                    { name: "TRADETERM", index: "TRADETERM", width: 70, sortable: false, hidden: true, sorttype: "text" },
                    { name: "lCUSTOMSSEAL", index: "lCUSTOMSSEAL", width: 100, sortable: false, hidden: true, sorttype: "text" },
                    { name: "LGORT", index: "LGORT", width: 70, sortable: false, hidden: false, sorttype: "text" },
                    { name: "DOCK", index: "DOCK", width: 70, sortable: false, hidden: true, sorttype: "text" },
                    { name: "ZPNAME2", index: "ZPNAME2", width: 70, sortable: false, hidden: true, sorttype: "text" },
                    { name: "WAERS", index: "WAERS", width: 70, sortable: false, hidden: true, sorttype: "text" }
        ],
        afterSaveCell: function (rowid, name, val, iRow, iCol) {
            var $this = $(this);
            var bIsSetSelected = false;
            if (formatFloat(val, 3) != 0) {
                var ch = $(this).find('#' + rowid + ' input[type=checkbox]').prop('checked');
                if (!ch) {
                    var PO_NUM = $this.jqGrid('getCell', rowid, 'PO_NUM');
                    var PO_LINE = $this.jqGrid('getCell', rowid, 'PO_LINE');
                    var MATERIAL = $this.jqGrid('getCell', rowid, 'MATERIAL');
                    $(this).jqGrid('setSelection', rowid, true);
                    if (CheckASNQty() != "") {
                        alert(CheckASNQty());
                    }
                    else {
                        QueryContractCode(diaToDoASNManage.prop("ASN_NUM"), diaToDoASNManage.attr("PLANT"), PO_NUM, PO_LINE, diaToDoASNManage.attr("VENDOR"), MATERIAL, diaToDoASNManage.attr("ETA"), rowid);
                    }
                }
            }
        },
        afterEditCell: function (rowid, iCol, cellcontent, e) {
            if (rowid % jqGridPageRowNum == 0) {
                $control = $("#" + jqGridPageRowNum + "_Book_ASN_QTY");
            }
            else {
                $control = $("#" + rowid % jqGridPageRowNum + "_Book_ASN_QTY");
            }
            if ($control.length > 0) {
                $control[0].select();
            }
        },
        onCellSelect: function (rowid, iCol, cellcontent, e) {
            var $this = $(this);
            var PO_NUM = $this.jqGrid('getCell', rowid, 'PO_NUM');
            var PO_LINE = $this.jqGrid('getCell', rowid, 'PO_LINE');
            var MATERIAL = $this.jqGrid('getCell', rowid, 'MATERIAL');
            var ASNQTY = $this.jqGrid('getCell', rowid, 'ASN_QTY');
            if (PO_NUM != "" && PO_LINE != "") {
                if (iCol == 11 && ASNQTY != 0) {
                    __DialogIsShownNow = false;
                    if (!__DialogIsShownNow) {
                        __DialogIsShownNow = true;
                        diaToDoASNImportDetailShowASNQty.attr("PONUM", $.trim(PO_NUM));
                        diaToDoASNImportDetailShowASNQty.attr("POLINE", $.trim(PO_LINE));
                        ReloadToDoASNImportDetailShowASNQtygridDataList();
                        diaToDoASNImportDetailShowASNQty.show();
                        diaToDoASNImportDetailShowASNQty.dialog("open");
                    }
                }
                else if (iCol == 0) {
                    var ch = $(this).find('#' + rowid + ' input[type=checkbox]').prop('checked');
                    if (ch) {
                        QueryContractCode(diaToDoASNManage.prop("ASN_NUM"), diaToDoASNManage.attr("PLANT"), PO_NUM, PO_LINE, diaToDoASNManage.attr("VENDOR"), MATERIAL, diaToDoASNManage.attr("ETA"), rowid);
                    }
                }
                else if (iCol == 6) {
                    LastSelectRowID = rowid;
                }
            }
        },
        cellEdit: true,
        cellsubmit: 'clientArray',
        editurl: 'clientArray',
        multiselect: true,
        onSelectRow: updateIdsOfSelectedRows,
        onSelectAll: function (aRowids, isSelected) {
            var i, count, id;
            for (i = 0, count = aRowids.length; i < count; i++) {
                id = aRowids[i];
                updateIdsOfSelectedRows(id, isSelected);
            }
        },
        shrinkToFit: false,
        scrollrows: true,
        //width: 1070,
        height: 464,
        rowNum: jqGridPageRowNum,
        //rowList: [10, 20, 30],
        //sortname: "MATERIAL,PO_NUM,PO_LINE",
        sortname: "PO_NUM,PO_LINE",
        viewrecords: true,
        loadonce: true,
        onPaging: function (pgButton) {
            HandleLastSelectedItem();
            var ErrorMsg = CheckASNQty();
            if (CheckASNQty() != "" && ErrorMsg !== undefined) alert(ErrorMsg);
        },
        pager: '#VMI_Process_ToDoASNImportDetail_gridListPager',
        loadError: function (xhr, st, err) {
            alert(xhr.status + " " + xhr.statusText);
            $("#btn_VMIProcess_ToDoASNImportDetail_Query").attr("disabled", false);
            $("#btn_VMIProcess_ToDoASNImportDetail_AdvanceQuery").attr("disabled", false);
        },
        loadComplete: function () {
            var $this = $(this), i, count;;

            for (i = 0, count = idsOfSelectedRows.length; i < count; i++) {
                $this.jqGrid('setSelection', idsOfSelectedRows[i], false);
            }
            if (idsOfSelectedRows !== undefined) {
                ChangeErrorASNQtyBackGroundColor();
            }

            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '') {
                    $("#btn_VMIProcess_ToDoASNImportDetail_Query").attr("disabled", false);
                    $("#btn_VMIProcess_ToDoASNImportDetail_AdvanceQuery").attr("disabled", false);
                }
        }
    });

    VMI_ToDoASNImportDetailgridDataList.jqGrid('navGrid', '#VMI_Process_ToDoASNImportDetail_gridListPager', { edit: false, add: false, del: false, search: false, refresh: false });
});


function InitdialogToDoASNImportDetail(Mode) {
    var diaToDoASNImportDetail = $('#dialog_VMIProcess_ToDoASNImportDetail');
    var VMI_ToDoASNImportDetailgridDataList = $('#VMI_Process_ToDoASNImportDetail_gridDataList');

    __DialogIsShownNow = false;

    $.ajax({
        url: __WebAppPathPrefix + '/VMIProcess/CheckIsEnableToDoASNFunctionByType',
        data: {
            ASN_NUM: escape($.trim($('#dialog_span_VMIProcess_ToDoASNManage_ASNNo').html())),
            bIsAllCheck: false
        },
        type: "post",
        dataType: 'json',
        async: false,
        success: function (data) {
            if (data.Result) {
                if (!__DialogIsShownNow) {
                    __DialogIsShownNow = true;
                    //ReloadToDoASNImportDetailgridDataList();
                    //Init.
                    //Set Tyoe Check Default Option
                    $("#txt_VMIProcess_ToDoASNImportDetail_Material").val("");
                    var radioType = $('input:radio[name=ImportDetail_QueryType]');
                    radioType.filter('[value=A]').prop('checked', true);

                    VMI_ToDoASNImportDetailgridDataList.jqGrid('clearGridData');

                    diaToDoASNImportDetail.attr('Mode', Mode);
                    if (Mode == "B") {
                        VMI_ToDoASNImportDetailgridDataList.jqGrid('hideCol', ["PSTYP", "KEEPER", "ZCUSTOMSCHK", "ZFREEPO", "UOM", "UOM_RATE", "lCUSTOMSSEAL"]);
                    }
                    else {
                        VMI_ToDoASNImportDetailgridDataList.jqGrid('showCol', ["PSTYP", "KEEPER"]).jqGrid('hideCol', ["ZCUSTOMSCHK", "ZFREEPO", "UOM", "UOM_RATE", "lCUSTOMSSEAL"]);
                    }

                    diaToDoASNImportDetail.show();
                    diaToDoASNImportDetail.dialog("open");

                    ReloadToDoASNImportDetailgridDataList();
                }
            }
            else {
                alert("The function is not avaliable to use.");
            }
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
            //HideAjaxLoading();
        }
    });
}

function CheckASNQty() {
    var AsnQty;
    var OpenPOQty;
    var ErrorMsg = "";
    var VMI_ToDoASNImportDetailgridDataList = $('#VMI_Process_ToDoASNImportDetail_gridDataList');
    var selectedrows = idsOfSelectedRows;
    if (selectedrows.length > 0) {
        for (var i = 0; i < selectedrows.length; i++) {
            AsnQty = VMI_ToDoASNImportDetailgridDataList.jqGrid('getLocalRow', selectedrows[i]).Book_ASN_QTY;
            OpenPOQty = VMI_ToDoASNImportDetailgridDataList.jqGrid('getLocalRow', selectedrows[i]).OPEN_PO_QTY;

            if (formatFloat(AsnQty, 3) > formatFloat(OpenPOQty, 3)) {
                if (ErrorMsg == "")
                    ErrorMsg += "Line:" + selectedrows[i];
                else
                    ErrorMsg += "," + selectedrows[i];
                //change backgroud clolor
                VMI_ToDoASNImportDetailgridDataList.jqGrid('setCell', selectedrows[i], "Book_ASN_QTY", "", {
                    'background-color': 'red'
                });
            }
            else {
                //change backgroud clolor
                VMI_ToDoASNImportDetailgridDataList.jqGrid('setCell', selectedrows[i], "Book_ASN_QTY", "", {
                    'background-color': 'white'
                });
            }
        }
        if (ErrorMsg != "")
            ErrorMsg += " ASN QTY is more than Open PO QTY";

        return ErrorMsg;
    }
}

function ChangeErrorASNQtyBackGroundColor() {
    var AsnQty;
    var OpenPOQty;
    var VMI_ToDoASNImportDetailgridDataList = $('#VMI_Process_ToDoASNImportDetail_gridDataList');
    var selectedrows = idsOfSelectedRows;
    if (selectedrows.length > 0) {
        for (var i = 0; i < selectedrows.length; i++) {
            AsnQty = VMI_ToDoASNImportDetailgridDataList.jqGrid('getLocalRow', selectedrows[i]).Book_ASN_QTY;
            OpenPOQty = VMI_ToDoASNImportDetailgridDataList.jqGrid('getLocalRow', selectedrows[i]).OPEN_PO_QTY;

            if (formatFloat(AsnQty, 3) > formatFloat(OpenPOQty, 3)) {
                //change backgroud clolor
                VMI_ToDoASNImportDetailgridDataList.jqGrid('setCell', selectedrows[i], "Book_ASN_QTY", "", {
                    'background-color': 'red'
                });
            }
            else {
                //change backgroud clolor
                VMI_ToDoASNImportDetailgridDataList.jqGrid('setCell', selectedrows[i], "Book_ASN_QTY", "", {
                    'background-color': 'white'
                });
            }
        }
    }
}

function jqgirdCellPositiveNumber(value, colname) {
    if (value < 0)
        return [false, "Please enter positive number."];
    else
        return [true, ""];
}

function QueryContractCode(asnnum, plant, ponum, poline, vendor, material, eta, rowid) {
    var diaToDoASNContractCode = $('#dialog_VMIProcess_ToDoASNContractCode');
    var VMI_ToDoASNImportDetailgridDataList = $('#VMI_Process_ToDoASNImportDetail_gridDataList');

    $.ajax({
        url: __WebAppPathPrefix + '/VMIProcess/QueryContractCodeForASNDetail',
        data: {
            ASNNUM: escape($.trim(asnnum)),
            PLANT: escape($.trim(plant)),
            PO_NUM: escape($.trim(ponum)),
            PO_LINE: escape($.trim(poline)),
            VENDOR: escape($.trim(vendor)),
            MATERIAL: escape($.trim(material)),
            ETA: escape($.trim(eta))
        },
        type: "post",
        dataType: 'json',
        async: false,
        success: function (data) {
            if (data.lCUSTOMSSEAL !== undefined && data.lCUSTOMSSEAL != null) {
                if (data.lCUSTOMSSEAL.length > 0) {
                    VMI_ToDoASNImportDetailgridDataList.jqGrid('showCol', ["lCUSTOMSSEAL"]);
                    if (data.lCUSTOMSSEAL.length == 1) {
                        VMI_ToDoASNImportDetailgridDataList.jqGrid('setCell', rowid, 'lCUSTOMSSEAL', data.lCUSTOMSSEAL[0]);
                    }
                    else {
                        __DialogIsShownNow = false;
                        if (!__DialogIsShownNow) {
                            __DialogIsShownNow = true;
                            var objSelectForCustomsSeal = $('#dialog_dropbox_VMIProcess_ToDoASNContractCode_CustomesSeal');
                            objSelectForCustomsSeal.find('option').remove().end();
                            for (var iCnt = 0; iCnt < data.lCUSTOMSSEAL.length; iCnt++)
                                objSelectForCustomsSeal.append($("<option></option>").attr("value", data.lCUSTOMSSEAL[iCnt]).text(data.lCUSTOMSSEAL[iCnt]));
                            diaToDoASNContractCode.attr("RowID", rowid);
                            diaToDoASNContractCode.show();
                            diaToDoASNContractCode.dialog("open");
                        }
                    }
                }
            }
            else {
                if (!data.Result) {
                    alert(data.Message);
                }
            }
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
            //HideAjaxLoading();
        }
    });
};

function HandleLastSelectedItem() {
    var diaToDoASNManage = $('#dialog_VMIProcess_ToDoASNManage');
    var VMI_ToDoASNImportDetailgridDataList = $('#VMI_Process_ToDoASNImportDetail_gridDataList');

    if (LastSelectRowID !== undefined) {
        var ch = VMI_ToDoASNImportDetailgridDataList.find('#' + LastSelectRowID + ' input[type=checkbox]').prop('checked');

        if (LastSelectRowID % jqGridPageRowNum != 0)
            var lastASN_QTY = $('#' + LastSelectRowID % jqGridPageRowNum + '_Book_ASN_QTY').val();
        else
            var lastASN_QTY = $('#' + jqGridPageRowNum + '_Book_ASN_QTY').val();
        if (lastASN_QTY !== undefined) {;
            VMI_ToDoASNImportDetailgridDataList.jqGrid('getLocalRow', LastSelectRowID).Book_ASN_QTY = lastASN_QTY;
            if (!ch) {
                if (formatFloat(lastASN_QTY, 3) != 0) {
                    VMI_ToDoASNImportDetailgridDataList.jqGrid('setSelection', LastSelectRowID, true);
                }
            }
        }
    }
}

updateIdsOfSelectedRows = function (id, isSelected) {
    var index = $.inArray(id, idsOfSelectedRows);
    if (!isSelected && index >= 0) {
        idsOfSelectedRows.splice(index, 1); // remove id from the list
    } else if (index < 0) {
        idsOfSelectedRows.push(id);
    }
};


