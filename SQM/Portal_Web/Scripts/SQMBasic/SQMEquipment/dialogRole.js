//initial dialog
function Load() {
    var colname;
    var colmodel;


    $.ajax({
        url: __WebAppPathPrefix + '/SQMBasic/getModel',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
        data: { EquipmentType: $('#ddlEquipmentType').val(), EquipmentSpecialType: $('#ddlEquipmentSpecialType').val() },
        type: "post",
        dataType: 'json',
        async: false,
        success: function (data) {
            for (var idx in data.model) {
                colmodel = data.model[idx]["ColModel"];
                colname = data.model[idx]["Colname"];
            }
            
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
    //Data List
    try {
        $("#SQMEquipmentgridDataList").jqGrid("GridUnload");
    } catch (e) {

    }
    setTimeout(function () {

    }, 200);
    var gridDataList = $("#SQMEquipmentgridDataList");
    var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
    var BasicInfoRowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
    gridDataList.jqGrid({
        url: __WebAppPathPrefix + '/SQMBasic/LoadEquipmentJSonWithFilter',
        postData: { EquipmentType: $('#ddlEquipmentType').val(), EquipmentSpecialType: $('#ddlEquipmentSpecialType').val(), BasicInfoGUID: gridDataListBasicInfoType.jqGrid('getRowData', BasicInfoRowId).BasicInfoGUID},
        type: "post",
        datatype: "json",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false
        },
        width: 900,
        height: "auto",
        colNames: colname,
        colModel: colmodel,
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'DeviceName',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#SQMEquipmentgridListPager',
        //sort by reload
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
        }
    });
    gridDataList.jqGrid('navGrid', '#SQMEquipmentgridListPager', { edit: false, add: false, del: false, search: false, refresh: false });

}
$(function () {
    var dialog = $("#SQMEquipmentdialogData");
    
    //Toolbar Buttons
    $("#btnSQMEquipmentdialogEditData").button({
        label: "Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnSQMEquipmentdialogCancelEdit").button({
        label: "Cancel",
        icons: { primary: "ui-icon-close" }
    });
    $("#txtDatePurchased").datepicker({
        changeMonth: true,
        dateFormat: 'yy/mm/dd',
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            } catch (err) {
                $(this).datepicker("setDate", '-31d');
            }
        }
    });
    $("#txtDatePurchased").datepicker("setDate", '-31d');
    $("#txtDateMade").datepicker({
        changeMonth: true,
        dateFormat: 'yy/mm/dd',
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            } catch (err) {
                $(this).datepicker("setDate", '-31d');
            }
        }
    });
    $("#txtDateMade").datepicker("setDate", '-31d');
    $("#SQMEquipmentdialogData").dialog({
        autoOpen: false,
        height: 345,
        width: 450,
        resizable: false,
        modal: true,
        buttons: {
            OK: function () {
                if (dialog.attr('Mode') == "v") {
                    $(this).dialog("close");
                    
                }
                else {
                    var DoSuccessfully = false;
                    $.ajax({
                        url: __WebAppPathPrefix + ((dialog.attr('Mode') == "c") ? "/SQMBasic/CreateEquipment" : "/SQMBasic/EditEquipment"),
                        data: {
                            "SID": dialog.attr("SID"),
                            "BasicInfoGUID": dialog.attr("BasicInfoGUID"),
                            "TB_SQM_Equipment_TypeCID": escape($.trim($("#ddlEquipmentTypeD").val())),
                            "TB_SQM_Equipment_Special_TypeSCID": escape($.trim($("#ddlEquipmentSpecialTypeD").val())),
                            "DeviceName": escape($.trim($("#txtDeviceName").val())),
                            "DeviceQuantity": escape($.trim($("#txtDeviceQuantity").val())),
                            "DeviceArea": escape($.trim($("#txtDeviceArea").val())),
                            "DeviceCapacity": escape($.trim($("#txtDeviceCapacity").val())),
                            "DevicePrecision": escape($.trim($("#txtDevicePrecision").val())),
                            "TestItem": escape($.trim($("#txtTestItem").val())),
                            "DatePurchased": escape($.trim($("#txtDatePurchased").val())),
                            "DateMade": escape($.trim($("#txtDateMade").val())),
                            "Model": escape($.trim($("#txtModel").val())),
                            "Brand": escape($.trim($("#txtBrand").val())),
                            "Weight": escape($.trim($("#txtWeight").val())),
                            "ModelSize": escape($.trim($("#txtModelSize").val())),
                            "LineLength": escape($.trim($("#txtLineLength").val())),
                            "GunQty": escape($.trim($("#txtGunQty").val())),
                            "RoastQty": escape($.trim($("#txtRoastQty").val())),
                            "PlatingType": escape($.trim($("#txtPlatingType").val())),
                            "Anodized": escape($.trim($("#txtAnodized").val())),
                            "SprayingType": escape($.trim($("#txtSprayingType").val())),
                            "PlatingCapacity": escape($.trim($("#txtPlatingCapacity").val())),
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                DoSuccessfully = true;
                                if (dialog.attr('Mode') == "c")
                                    alert("Equipment create successfully.");

                                else
                                    alert("Equipment edit successfully.");
                               
                            }
                            else {
                                if ((dialog.attr('Mode') != "c") && (data == __LockIsNotValid)) {
                                    alert("Edit time too long, abort current editing.\n\n(Please restart editing if you wish to do it again)");
                                    DoSuccessfully = true;
                                }
                                else
                                    $("#lblDiaErrMsg").html(data);
                            }
                        },
                        error: function (xhr, textStatus, thrownError) {
                            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                        },
                        complete: function (jqXHR, textStatus) {
                            //$("#ajaxLoading").hide();
                        }
                    });
                    if (DoSuccessfully) {
                        $(this).dialog("close");
                        Load();
                    }
                }
            },
            Cancel: function () { $(this).dialog("close"); }
        },
        close: function () {
            if (dialog.attr('Mode') == "e") {
                var r = ReleaseDataLock(dialog.attr('SID'));
                if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
            }
        }
    });
});

//change dialog UI
// c: Create, v: View, e: Edit

function getModel(EquipmentType,EquipmentSpecialType) {
       
        $.ajax({
            url: __WebAppPathPrefix + '/SQMBasic/getModel',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
            data: { EquipmentType: EquipmentType, EquipmentSpecialType: EquipmentSpecialType },
            type: "post",
            dataType: 'json',
            async: false,
            success: function (data) {
                $("#SQMEquipmentdialogData table tr").hide();
                $("#EquipmentType").show();
                for (var idx in data.model) {
                    for (var i in data.model[idx]["Colname"]) {
                        var id = "#" + data.model[idx]["Colname"][i];
                        $(id).show();
                    }
                }
                $("#SQMEquipmentgridDataList");
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
            }
        });
    }
function EquipmentDialogSetUIByMode(Mode) {
    var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
    var BasicInfoRowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
    var dialog = $("#SQMEquipmentdialogData");
    var gridDataList = $("#SQMEquipmentgridDataList");
    switch (Mode) {
        case "c": //Create
            $("#SQMEquipmentdialogDataToolBar").hide();
            $("#SQMEquipmentdialogData table tr").hide();
            dialog.attr('ItemRowId', "");
            dialog.attr('SID', "");
            dialog.attr('BasicInfoGUID', gridDataListBasicInfoType.jqGrid('getRowData', BasicInfoRowId).BasicInfoGUID);
            $("#EquipmentType").show();
            $("#SQMEquipmentErrorMsg").show();
            $("#ddlEquipmentTypeD").val(1);
            $("#ddlEquipmentTypeD").removeAttr('disabled');
            $("#ddlEquipmentSpecialTypeD").val("");
            $("#ddlEquipmentSpecialTypeD").removeAttr('disabled');
            $("#txtDeviceName").val("");
            $("#txtDeviceName").removeAttr('disabled');
            $("#txtDeviceQuantity").val("");
            $("#txtDeviceQuantity").removeAttr('disabled');
            $("#txtDeviceArea").val("");
            $("#txtDeviceArea").removeAttr('disabled');
            $("#txtDeviceCapacity").val("");
            $("#txtDeviceCapacity").removeAttr('disabled');
            $("#txtDevicePrecision").val("");
            $("#txtDevicePrecision").removeAttr('disabled');
            $("#txtTestItem").val("");
            $("#txtTestItem").removeAttr('disabled');
            $("#txtDatePurchased").val("");
            $("#txtDatePurchased").removeAttr('disabled');
            $("#txtDateMade").val("");
            $("#txtDateMade").removeAttr('disabled');
            $("#txtModel").val("");
            $("#txtModel").removeAttr('disabled');
            $("#txtBrand").val("");
            $("#txtBrand").removeAttr('disabled');
            $("#txtWeight").val("");
            $("#txtWeight").removeAttr('disabled');
            $("#txtModelSize").val("");
            $("#txtModelSize").removeAttr('disabled');
            $("#txtLineLength").val("");
            $("#txtLineLength").removeAttr('disabled');
            $("#txtGunQty").val("");
            $("#txtGunQty").removeAttr('disabled');
            $("#txtRoastQty").val("");
            $("#txtRoastQty").removeAttr('disabled');
            $("#txtPlatingType").val("");
            $("#txtPlatingType").removeAttr('disabled');
            $("#txtAnodized").val("");
            $("#txtAnodized").removeAttr('disabled');
            $("#txtSprayingType").val("");
            $("#txtSprayingType").removeAttr('disabled');
            $("#txtPlatingCapacity").val("");
            $("#txtPlatingCapacity").removeAttr('disabled');
            $("#SQMEquipmentErrorMsg").html("");
            getModel("1","");
            break;
        case "v": //View
            $("#btnSQMEquipmentdialogEditData").button("option", "disabled", false);
            $("#btnSQMEquipmentdialogCancelEdit").button("option", "disabled", true);
            $("#SQMEquipmentdialogDataToolBar").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            getModel(dataRow.TB_SQM_Equipment_TypeCID, dataRow.TB_SQM_Equipment_Special_TypeSCID);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('SID', dataRow.SID);
            $("#SQMEquipmentErrorMsg").show();
            $("#ddlEquipmentTypeD").val(dataRow.TB_SQM_Equipment_TypeCID)
            $("#ddlEquipmentTypeD").attr("disabled", "disabled");
            $("#ddlEquipmentSpecialTypeD").val(dataRow.TB_SQM_Equipment_Special_TypeSCID)
            $("#ddlEquipmentSpecialTypeD").attr("disabled", "disabled");
            $("#txtDeviceName").val(dataRow.DeviceName);
            $("#txtDeviceName").attr("disabled", "disabled");
            $("#txtDeviceQuantity").val(dataRow.DeviceQuantity);
            $("#txtDeviceQuantity").attr("disabled", "disabled");
            $("#txtDeviceArea").val(dataRow.DeviceArea);
            $("#txtDeviceArea").attr("disabled", "disabled");
            $("#txtDeviceCapacity").val(dataRow.DeviceCapacity);
            $("#txtDeviceCapacity").attr("disabled", "disabled");
            $("#txtDevicePrecision").val(dataRow.DevicePrecision);
            $("#txtDevicePrecision").attr("disabled", "disabled");
            $("#txtTestItem").val(dataRow.TestItem);
            $("#txtTestItem").attr("disabled", "disabled");
            $("#txtDatePurchased").val(dataRow.DatePurchased);
            $("#txtDatePurchased").attr("disabled", "disabled");
            $("#txtDateMade").val(dataRow.DateMade);
            $("#txtDateMade").attr("disabled", "disabled");
            $("#txtModel").val(dataRow.Model);
            $("#txtModel").attr("disabled", "disabled");
            $("#txtBrand").val(dataRow.Brand);
            $("#txtBrand").attr("disabled", "disabled");
            $("#txtWeight").val(dataRow.width);
            $("#txtWeight").attr("disabled", "disabled");
            $("#txtModelSize").val(dataRow.ModelSize);
            $("#txtModelSize").attr("disabled", "disabled");
            $("#txtLineLength").val(dataRow.LineLength);
            $("#txtLineLength").attr("disabled", "disabled");
            $("#txtGunQty").val(dataRow.GunQty);
            $("#txtGunQty").attr("disabled", "disabled");
            $("#txtRoastQty").val(dataRow.RoastQty);
            $("#txtRoastQty").attr("disabled", "disabled");
            $("#txtPlatingType").val(dataRow.PlatingType);
            $("#txtPlatingType").attr("disabled", "disabled");
            $("#txtAnodized").val(dataRow.Anodized);
            $("#txtAnodized").attr("disabled", "disabled");
            $("#txtSprayingType").val(dataRow.SprayingType);
            $("#txtSprayingType").attr("disabled", "disabled");
            $("#txtPlatingCapacity").val(dataRow.PlatingCapacity);
            $("#txtPlatingCapacity").attr("disabled", "disabled");

            $("#SQMEquipmentErrorMsg").html("");

            break;
        default: //Edit("e")
            $("#btnSQMEquipmentdialogEditData").button("option", "disabled", true);
            $("#btnSQMEquipmentdialogCancelEdit").button("option", "disabled", false);
            $("#SQMEquipmentdialogDataToolBar").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('SID', dataRow.SID);
            getModel(dataRow.TB_SQM_Equipment_TypeCID, dataRow.TB_SQM_Equipment_Special_TypeSCID);
            $("#SQMEquipmentErrorMsg").show();
            $("#ddlEquipmentTypeD").val(dataRow.TB_SQM_Equipment_TypeCID)
            
            $("#ddlEquipmentSpecialTypeD").val(dataRow.TB_SQM_Equipment_Special_TypeSCID)
            
            $("#txtDeviceName").val(dataRow.DeviceName);
            $("#txtDeviceName").removeAttr('disabled');
            $("#txtDeviceQuantity").val(dataRow.DeviceQuantity);
            $("#txtDeviceQuantity").removeAttr('disabled');
            $("#txtDeviceArea").val(dataRow.DeviceArea);
            $("#txtDeviceArea").removeAttr('disabled');
            $("#txtDeviceCapacity").val(dataRow.DeviceCapacity);
            $("#txtDeviceCapacity").removeAttr('disabled');
            $("#txtDevicePrecision").val(dataRow.DevicePrecision);
            $("#txtDevicePrecision").removeAttr('disabled');
            $("#txtTestItem").val(dataRow.TestItem);
            $("#txtTestItem").removeAttr('disabled');
            $("#txtDatePurchased").val(dataRow.DatePurchased);
            $("#txtDatePurchased").removeAttr('disabled');
            $("#txtDateMade").val(dataRow.DateMade);
            $("#txtDateMade").removeAttr('disabled');
            $("#txtModel").val(dataRow.Model);
            $("#txtModel").removeAttr('disabled');
            $("#txtBrand").val(dataRow.Brand);
            $("#txtBrand").removeAttr('disabled');
            $("#txtWeight").val(dataRow.width);
            $("#txtWeight").removeAttr('disabled');
            $("#txtModelSize").val(dataRow.ModelSize);
            $("#txtModelSize").removeAttr('disabled');
            $("#txtLineLength").val(dataRow.LineLength);
            $("#txtLineLength").removeAttr('disabled');
            $("#txtGunQty").val(dataRow.GunQty);
            $("#txtGunQty").removeAttr('disabled');
            $("#txtRoastQty").val(dataRow.RoastQty);
            $("#txtRoastQty").removeAttr('disabled');
            $("#txtPlatingType").val(dataRow.PlatingType);
            $("#txtPlatingType").removeAttr('disabled');
            $("#txtAnodized").val(dataRow.Anodized);
            $("#txtAnodized").removeAttr('disabled');
            $("#txtSprayingType").val(dataRow.SprayingType);
            $("#txtSprayingType").removeAttr('disabled');
            $("#txtPlatingCapacity").val(dataRow.PlatingCapacity);
            $("#txtPlatingCapacity").removeAttr('disabled');
            //$("#txtEmail").val(dataRow.Email);
            //$("#txtEmail").removeAttr('disabled');

            $("#SQMEquipmentErrorMsg").html("");

            break;
    }
}
