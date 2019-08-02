$(function () {
    $('#ddlEquipmentType').change(function () {
        var colname;
        var colmodel;
        var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
        var BasicInfoRowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
        var BasicInfoGUID = gridDataListBasicInfoType.jqGrid('getRowData', BasicInfoRowId).BasicInfoGUID;
        //alert($('#ddlCategory').val());
        $.ajax({
            url: __WebAppPathPrefix + '/SQMBasic/GetEquipmentSpecialType',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
            data: { MainID: $('#ddlEquipmentType').val() },
            type: "post",
            dataType: 'json',
            async: false, // if need page refresh, please remark this option
            success: function (data) {
                //var options = '<option value=-1 Selected>All</option>';
                var options = '';
                for (var idx in data) {
                    options += '<option value=' + data[idx].SCID + '>' + data[idx].CName + '</option>';
                }
                //$('#ddlCategory').append(options);
                $('#ddlEquipmentSpecialType').html(options);
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
            }
        });
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
                $("#SQMEquipmentgridDataList");
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
        setTimeout(function(){
            
        },200);
        var gridDataList = $("#SQMEquipmentgridDataList");
     
        gridDataList.jqGrid({
            url: __WebAppPathPrefix + '/SQMBasic/LoadEquipmentJSonWithFilter',
            postData: { EquipmentType: $('#ddlEquipmentType').val(), EquipmentSpecialType: "", BasicInfoGUID: BasicInfoGUID },
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

    });
    $('#ddlEquipmentSpecialType').change(function () {
        var colname;
        var colmodel;

        var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
        var BasicInfoRowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
        var BasicInfoGUID = gridDataListBasicInfoType.jqGrid('getRowData', BasicInfoRowId).BasicInfoGUID;
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
                $("#SQMEquipmentgridDataList");
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

        gridDataList.jqGrid({
            url: __WebAppPathPrefix + '/SQMBasic/LoadEquipmentJSonWithFilter',
            postData: { EquipmentType: $('#ddlEquipmentType').val(), EquipmentSpecialType: $('#ddlEquipmentSpecialType').val(), BasicInfoGUID: BasicInfoGUID },
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

    });
    $.ajax({
        url: __WebAppPathPrefix + '/SQMBasic/GetEquipmentType',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
        type: "post",
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            //var options = '<option value=-1 Selected>All</option>';
            var options = '';
            for (var idx in data) {
                options += '<option value=' + data[idx].CID + '>' + data[idx].CNAME + '</option>';
            }
            $('#ddlEquipmentType').append(options);
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
    function init()
    {
        $("#txtDeviceName").val("");

        $("#txtDeviceQuantity").val("");

        $("#txtDeviceArea").val("");

        $("#txtDeviceCapacity").val("");
      
        $("#txtDevicePrecision").val("");
  
        $("#txtTestItem").val("");

        $("#txtDatePurchased").val("");
     
        $("#txtDateMade").val("");

        $("#txtModel").val("");

        $("#txtBrand").val("");
    
        $("#txtWeight").val("");

        $("#txtModelSize").val("");
      
        $("#txtLineLength").val("");
      
        $("#txtGunQty").val("");

        $("#txtRoastQty").val("");
        $("#txtPlatingType").val("");
       
        $("#txtAnodized").val("");
        
        $("#txtSprayingType").val("");
       
        $("#txtPlatingCapacity").val("");
      
        $("#lblDiaErrMsg").html("");
    }
    $('#ddlEquipmentTypeD').change(function () {

        $.ajax({
            url: __WebAppPathPrefix + '/SQMBasic/GetEquipmentSpecialType',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
            data: { MainID: $('#ddlEquipmentTypeD').val() },
            type: "post",
            dataType: 'json',
            async: false, // if need page refresh, please remark this option
            success: function (data) {
                var options = '';
                for (var idx in data) {
                    options += '<option value=' + data[idx].SCID + '>' + data[idx].CName + '</option>';
                }
                //$('#ddlCategory').append(options);
                $('#ddlEquipmentSpecialTypeD').html(options);
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
            }
        });
        $.ajax({
            url: __WebAppPathPrefix + '/SQMBasic/getModel',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
            data: { EquipmentType: $('#ddlEquipmentTypeD').val(), EquipmentSpecialType: $('#ddlEquipmentSpecialTypeD').val() },
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
                        init();
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
    });
    $('#ddlEquipmentSpecialTypeD').change(function () {
       
        $.ajax({
            url: __WebAppPathPrefix + '/SQMBasic/getModel',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
            data: { EquipmentType: $('#ddlEquipmentTypeD').val(), EquipmentSpecialType: $('#ddlEquipmentSpecialTypeD').val() },
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
                        init()
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
    });
  
    $.ajax({
        url: __WebAppPathPrefix + '/SQMBasic/GetEquipmentType',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
        type: "post",
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            //var options = '<option value=-1 Selected>All</option>';
            var options = '';
            for (var idx in data) {
                options += '<option value=' + data[idx].CID + '>' + data[idx].CNAME + '</option>';
            }
            $('#ddlEquipmentTypeD').append(options);
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });

    //Toolbar Buttons
    $("#btnSQMEquipmentCreate").button({
        label: "Create",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnSQMEquipmentViewEdit").button({
        label: "View/Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnSQMEquipmentDelete").button({
        label: "Delete",
        icons: { primary: "ui-icon-trash" }
    });

    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });

    
    $('#SQMEquipmenttbMain1').show();
    $('#SQMEquipmentdialogData').show();
}
)