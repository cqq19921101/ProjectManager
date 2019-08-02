$(function () {
    var diaToDoVDSEditVDS = $('#dialog_VMIProcess_ToDoVDSEditVDS');

    diaToDoVDSEditVDS.dialog({
        autoOpen: false,
        height: 620,
        width: 500,
        resizable: false,
        modal: true,
        open: function (event, ui) {
            diaToDoVDSEditVDS.find('table#tbMaterialHeader').remove();
            diaToDoVDSEditVDS.find('table#tbDataMeasure').remove();

            var Material = diaToDoVDSEditVDS.prop('Material');
            var Description = diaToDoVDSEditVDS.prop('Description');
            var UOM = diaToDoVDSEditVDS.prop('UOM');
            var SizeDimension = diaToDoVDSEditVDS.prop('Size/Dimension');
            var Remark = diaToDoVDSEditVDS.prop('Remark');
            var DataMeasure = diaToDoVDSEditVDS.prop('DataMeasure');
            // For VDS Material Header
            var table = "<table id='tbMaterialHeader'>" +
                "<tr>" +
                "<td class='tdTitleAlignRight'>Material:</td>" +
                "<td class='spanFieldValueUnderLine'>" + Material + "</td>" +
                "<td class='tdTitleAlignRight'>Description:</td>" +
                "<td class='spanFieldValueUnderLine'>" + Description + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td class='tdTitleAlignRight'>Uom:</td>" +
                "<td class='spanFieldValueUnderLine'>" + UOM + "</td>" +
                "<td class='tdTitleAlignRight'>Size/Dimension:</td>" +
                "<td class='spanFieldValueUnderLine'>" + SizeDimension + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td class='tdTitleAlignRight'>Remark:</td>" +
                "<td colspan='3'>" +
                "<input id='txtMaterialRemark' style='width: 100%' type='text' maxlength='25' />" +
                "</td>" +
                "</tr>" +
                "</table>";

            diaToDoVDSEditVDS.append(table);
            $('#txtMaterialRemark').val(Remark);
            // For VDS Material Data Measure Detail
            table = "<table id='tbDataMeasure' class='defaultTable'>" +
                "<tr style='display:none;'>" +
                "<td colspan=3>Data Measure</td>" +
                "</tr>" +
                "<tr>" +
                "<td class='ui-state-default ui-th-column'></td>" +
                "<td class='ui-state-default ui-th-column'>Demand</td>" +
                "<td class='ui-state-default ui-th-column'>Commit</td>" +
                "</tr>";

            var dataMeasureObjs = $.parseJSON(DataMeasure);
            for (var idx in dataMeasureObjs) {
                var ColName = dataMeasureObjs[idx].ColName;
                var Demand = dataMeasureObjs[idx].Demand;
                var Commit = dataMeasureObjs[idx].Commit;

                if (diaToDoVDSEditVDS.prop('TMType') == 'M') {
                    var d = new Date(ColName);
                    var words = ['Jan ', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
                    ColName = words[d.getMonth()] + '-' + d.getFullYear();
                }

                table += "<tr>" +
                    "<td style='width: 33%; text-align: right;'>" + ColName + "</td>" +
                    "<td style='width: 33%; text-align: right;'>" + Demand + "</td>";

                if (ColName == 'After' || ColName == 'Plan Order') {
                    table += "<td style='width: 33%; text-align: right;'></td>" +
                        "</tr>";
                }
                else {
                    table += "<td style='width: 33%; text-align: right;'>" +
                        "<input class='positiveInteger' type='text' style='text-align: right' value=" + Commit + " />" +
                        "</td>" +
                        "</tr>";
                }
            }
            table += "</table>";

            diaToDoVDSEditVDS.append(table);

            if (diaToDoVDSEditVDS.prop('TMType') != 'M') {
                var total = 0;
                $('#tbDataMeasure tr').each(function (index, value) {
                    var currentDemand = parseInt($(this).find('td:nth(1)').text().replace(',', ''));
                    if (!isNaN(currentDemand)) {
                        total += currentDemand;
                    }
                });

                $('#tbDataMeasure').append("<tr>" +
                    "<td style='width: 33%; text-align: right;'>Total</td>" +
                    "<td style='width: 33%; text-align: right;'>" + numberWithCommas(total) + "</td>" +
                    "<td style='width: 33%; text-align: right;'></td>" +
                    "</tr>");
            }

            diaToDoVDSEditVDS.find('span').remove();
            diaToDoVDSEditVDS.append("<span style='color: red'>Please click 'Submit' button before leaving if the data have been modified.*</span>");

            $('#tbDataMeasure .positiveInteger').each(function (index, value) {
                $(this).change(function () {
                    if (!isNormalInteger($(this).val())) {
                        $(this).val(0);
                        alert('The field must be positive number!');
                    }
                    else {
                        var tmpVal = $(this).val();
                        tmpVal = Number(tmpVal.match(/^\d+(?:\.\d{0,3})?/))
                        tmpVal = numberWithCommas(tmpVal);
                        $(this).val(tmpVal);
                    }
                });
                $(this).focus(function () {
                    var tmpVal = $(this).val();
                    $(this).val(tmpVal.replace(/\,/g, ''));
                });
                $(this).blur(function () {
                    var tmpVal = $(this).val();
                    $(this).val(tmpVal.replace(/\,/g, ''));
                    $(this).val(numberWithCommas($(this).val()));
                })
            });
        },
        buttons: {
            Submit: function () {
                var PLANT = diaToDoVDSEditVDS.prop('PLANT');
                var BUYER = diaToDoVDSEditVDS.prop('BUYER');
                var VENDOR = diaToDoVDSEditVDS.prop('VENDOR');
                var VDS_NUM = diaToDoVDSEditVDS.prop('VDS_NUM');
                var VRSIO = diaToDoVDSEditVDS.prop('VRSIO');
                var TMType = diaToDoVDSEditVDS.prop('TMType');
                var TBNUM = parseInt(diaToDoVDSEditVDS.prop('TBNUM'));
                var Material = diaToDoVDSEditVDS.prop('Material');
                var VDS_LINE = diaToDoVDSEditVDS.prop('VDS_LINE');

                $.ajax({
                    url: __WebAppPathPrefix + '/VMIProcess/CheckVDSEditable',
                    data: {
                        PLANT: PLANT,
                        BUYER: BUYER,
                        VENDOR: VENDOR,
                        VDS_NUM: VDS_NUM,
                        VRSIO: VRSIO,
                        TMType: TMType
                    },
                    type: "post",
                    dataType: 'json',
                    async: false,
                    success: function (data) {
                        if (data != '') {
                            if (data.editable) {
                                var remark = escape($('#tbMaterialHeader #txtMaterialRemark').val());
                                var CBEDQTY = $('#tbDataMeasure tr:eq(2) td:eq(2) input').val();
                                var jsonStrCommit = '';

                                jsonStrCommit += '"VDS_NUM" : "' + VDS_NUM + '", ';
                                jsonStrCommit += '"VRSIO" : "' + VRSIO + '", ';
                                jsonStrCommit += '"MATERIAL" : "' + Material + '", ';
                                jsonStrCommit += '"VDS_LINE" : "' + VDS_LINE + '", ';
                                jsonStrCommit += '"REMARK" : "' + remark + '", ';
                                if (TMType != 'M') {
                                    jsonStrCommit += '"CBEFQTY" : "' + CBEDQTY.replace(/\,/g, '') + '", ';
                                }
                                
                                for (var i = 1; i <= TBNUM; i++) {
                                    var dataRow = i + (TMType == 'M' ? 1 : 2);
                                    var CDMD = $('#tbDataMeasure tr:eq(' + dataRow + ') td:eq(2) input').val();
                                    var CDMDNAME = 'CDMD' + pad2(i);

                                    jsonStrCommit += '"' + CDMDNAME + '": "' + CDMD.replace(/\,/g, '') + '", ';
                                }

                                jsonStrCommit = jsonStrCommit.substring(0, jsonStrCommit.length - 2);
                                jsonStrCommit = '{' + jsonStrCommit + '}';

                                $.ajax({
                                    url: __WebAppPathPrefix + '/VMIProcess/UpdateVDSDataMeasure',
                                    data: {
                                        updateInfo: jsonStrCommit
                                    },
                                    type: "post",
                                    dataType: 'text',
                                    async: false,
                                    success: function (data) {
                                        alert(data);
                                    },
                                    error: function (xhr, textStatus, thrownError) {
                                        $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                                    },
                                    complete: function (jqXHR, textStatus) {
                                    }
                                });

                                var jqGridList;
                                switch (TMType) {
                                    case 'D':
                                        jqGridList = $('#listDaily');
                                        break;
                                    case 'W':
                                        jqGridList = $('#listWeekly');
                                        break;
                                    case 'M':
                                        jqGridList = $('#listMonthly');
                                        break;
                                    case 'X':
                                        jqGridList = $('#listDWM');
                                        break;
                                }
                                jqGridList.jqGrid('setGridParam', {
                                    postData: {
                                        TBNUM: TBNUM,
                                        VDS_NUM: VDS_NUM,
                                        VRSIO: VRSIO
                                    }
                                });
                                jqGridList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
                                jqGridList.jqGrid('destroyFrozenColumns').jqGrid('setFrozenColumns');

                                diaToDoVDSEditVDS.dialog("close");
                            } else {
                                alert(data.message);
                            }
                        }
                    },
                    error: function (xhr, textStatus, thrownError) {
                        $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                    },
                    complete: function (jqXHR, textStatus) {
                    }
                });
            },
            "Close Window": function () {
                $(this).dialog("close");
            }
        },
        close: function () {
            __DialogIsShownNow = false;
        }
    });
})
// EX: 2000 --> 2,000
function numberWithCommas(x) {
    if (x == null) {
        x = '';
    }
    var parts = x.toString().split(".");
    parts[0] = parseInt(parts[0]).toString();
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    while (/0$/.test(parts[1])) {// To check if number have zero at the end
        parts[1] = parts[1].substring(0, parts[1].length - 1); // To trim zero
        if (parts[1].length == 0) {
            parts.splice(1, 1);
        }
    }
    return parts.join(".");
}
// To check if positive integer
function isNormalInteger(str) {
    //return /^\+?(0|[1-9]\d*)$/.test(str);
    //return /^\d+(.\d{1,3})?$/.test(str);
    return /^\d+(.\d+)?$/.test(str);
}
// EX: 1 --> 01, 2 --> 02 ... 10 --> 10
function pad2(number) {
    return (number < 10 ? '0' : '') + number
}