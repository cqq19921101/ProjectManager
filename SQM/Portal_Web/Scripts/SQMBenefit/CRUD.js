$(function () {
    var gridDataList = $("#gridDataList");
    var dialog = $("#dialogData");
   
       $('input[type=radio][name=Isvalid]').change(function () {
        if (this.value == '3') {
            $("#txtMonth1").removeAttr('disabled');
            $("#txtDay1").removeAttr('disabled');
            $("#txtMonth2").attr("disabled", "disabled");
            $("#txtDay2").attr("disabled", "disabled");
            $("#txtHour").attr("disabled", "disabled");
         
            $("#txtMonth2").val("");
            $("#txtDay2").val("");
            $("#txtHour").val("");
        }
        else if (this.value == '5') {
            $("#txtMonth1").attr("disabled", "disabled");
            $("#txtDay1").attr("disabled", "disabled");
            $("#txtMonth2").removeAttr('disabled');
        $("#txtDay2").removeAttr('disabled');
        $("#txtHour").removeAttr('disabled');
        $("#txtMonth1").val("");
        $("#txtDay1").val("");
   
        } else {
            $("#txtMonth1").attr("disabled", "disabled");
            $("#txtDay1").attr("disabled", "disabled");
            $("#txtMonth2").attr("disabled", "disabled");
            $("#txtDay2").attr("disabled", "disabled");
            $("#txtHour").attr("disabled", "disabled");
     
        }
    });
    jQuery("#btnCreate").click(function () {
        $(this).removeClass('ui-state-focus');
        var today = new Date();//获得当前日期
        var year = today.getFullYear();//获得年份
        var month = today.getMonth() + 1;

        month = (month < 10 ? "0" + month : month);
        var mydate = (year.toString() + month.toString());
        $('#txtBenbfitDate').val(mydate);
        $('#txtVendorCode').val("");
        $('#txtTotolScore').val("");
        $('#txtMaterialType').val("");
        $('#txtClass').val("");
        $('#txtBatchNum1').val("");
        $('#txtRejectNum1').val("");
        $('#txtRejectRate').val("");
        $('#txtTargetValue1').val("");
        $('#txtScore1').val("");
        $('#txtProductionNum').val("");
        $('#txtDefectQuantity').val("");
        $('#txtRejectRatio').val("");
        $('#txtTargetValue2').val("");
        $('#txtScore2').val("");
        $('#txtTotalRecoveryDays').val("");
        $('#txtReplyQuantity').val("");
        $('#txtMDO').val("");
        $('#txtTargetValue3').val("");
        $('#txtScore3').val("");
        $('#txtXRF').val("");
        $('#txtBatchNum2').val("");
        $('#txtRejectNum2').val("");
        $('#txtTargetValue4').val("");
        $('#txtScore4').val("");
        $('#txtA').val("");
        $('#txtB').val("");
        $('#txtC').val("");
        $('#txtD').val("");
        $('#txtE').val("");
        $('#txtF').val("");
        $('#txtG').val("");
        $('#txtH').val("");
        $('#txtScore5').val("");
        $('#txtScore6').val("");
        $('#txtScore7').val("");
        $('#txtNOTE').val("");
        $('#txtIQC').val("");
        $('#txtSQE').val("");
        setRadio("Isvalid", escape($.trim(1)));
        $('#txtSCOURCER').val("");
        $('#txtBUYER').val("");
        $("#txtMonth1").val("");
        $("#txtDay1").val("");
        $("#txtMonth2").val("");
        $("#txtDay2").val("");
        $("#txtHour").val("");
        $('#back').show();
        $('#tb1').hide();
        $('#tb2').show();
        $('#btnSave').show();
        $('#btnSave2').hide();
        $("#txtMonth1").attr("disabled", "disabled");
        $("#txtDay1").attr("disabled", "disabled");
        $("#txtMonth2").attr("disabled", "disabled");
        $("#txtDay2").attr("disabled", "disabled");
        $("#txtHour").attr("disabled", "disabled");
    });

  
    $('#dialog_btn_diaSBUVendor_Search').click(function () {
        $(this).removeClass('ui-state-focus');
        ReloadDiaSBUVendorCodegridDataList();
    });
    function ReloadDiaSBUVendorCodegridDataList() {
        var diaSBUVendorgridData = $('#dialog_VMI_SBUVendor_gridDataList');
        var diatxtSBUVDN = $('#dialog_VMI_txt_SBU_VDN');

        diaSBUVendorgridData.jqGrid('clearGridData');
        diaSBUVendorgridData.jqGrid('setGridParam', { postData: { ERP_VND: escape($.trim(diatxtSBUVDN.val())) } });
        diaSBUVendorgridData.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    }

    jQuery("#btndialogEditData").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            //var r = AcquireDataLock(dialog.attr('SubFuncGUID'))
            //if (r == "ok") {
            dialog.attr('Mode', "e");
            DialogSetUIByMode(dialog.attr('Mode'));
            dialog.dialog("option", "title", "Edit").dialog("open");
            //}
            //else {
            //    switch (r) {
            //        case "timeout": $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); break;
            //        case "l": alert("Data already lock by other user."); break;
            //        default: alert("Data lock fail or application error."); break;
            //    }
            //}
        } else { alert("Please select a row data to edit."); }
    });

    jQuery("#btndialogCancelEdit").click(function () {
        $(this).removeClass('ui-state-focus');
        //var r = ReleaseDataLock(dialog.attr('SID'));
        //if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); else SetToViewMode();
    });

    function SetToViewMode() {
        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            dialog.attr('Mode', "v");
            DialogSetUIByMode(dialog.attr('Mode'));
            dialog.dialog("option", "title", "View").dialog("open");
        } else { alert("Please select a row data to edit."); }
    }

    jQuery("#txtVendorCode").change(function () {
        $.ajax({
            url: __WebAppPathPrefix + '/SQMBenefit/GetSQMBenefitData',
            data: {
                "BenbfitDate": escape($.trim($('#txtBenbfitDate').val())),
                "VendorCode": escape($.trim($('#txtVendorCode').val()))
            },
            type: "post",
            dataType: 'json',
            async: false, // if need page refresh, please remark this option
            success: function (data) {
                $("#txtMaterialType").val(data[0].MaterialType);
                $("#txtBatchNum1").val(data[0].BatchNum1);
                $("#txtRejectNum1").val(data[0].RejectNum1);
               // $("#txtTargetValue1").val(data[0].TargetValue1);
                $("#txtProductionNum").val(data[0].ProductionNum);
                $("#txtDefectQuantity").val(data[0].DefectQuantity);
                //$("#txtTargetValue2").val(data[0].TargetValue2);
                $("#txtXRF").val(data[0].XRF);
                $("#txtBatchNum2").val(data[0].BatchNum2);

            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {

            }
        });
        $.ajax({
            url: __WebAppPathPrefix + '/SQMBenefit/GetSQMAnnual',
            data: {
                "MaterialType": escape($.trim($('#txtMaterialType').val())),        
            },
            type: "post",
            dataType: 'json',
            async: false, // if need page refresh, please remark this option
            success: function (data) {
          
                    $("#txtTargetValue1").val(data[0].Annual1);
              
                    $("#txtTargetValue2").val(data[0].Annual2);
                
              
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {

            }
        });
   
    });

    function setRadio(name, sRadioValue) {        //传入radio的name和选中项的值
        var oRadio = document.getElementsByName(name);
        for (var i = 0; i < oRadio.length; i++) //循环
        {
            if (oRadio[i].value == sRadioValue) //比较值
            {
                oRadio[i].checked = true; //修改选中状态
                break; //停止循环
            }
        }
    }
   
   
  
   
  
    //$("#txtcommissioningdate3").datepicker({
    //    changeMonth: true,
    //    changeYear: true,
    //    dateFormat: 'yy/mm/dd',
    //    onClose: function (selectedDate) {
    //        try {
    //            $.datepicker.parseDate('yy/mm/dd', $(this).val());
    //        } catch (err) {
    //            $(this).datepicker("setDate", '-31d');
    //        }
    //    }
    //});
    //$("#txtcommissioningdate3").datepicker("setDate", '-31d');
  
   
    var benefitSID;
 
    jQuery("#btnShow").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {   //single select
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            benefitSID=dataRow.SID;
            $.ajax({
                url: __WebAppPathPrefix + '/SQMBenefit/GetSQMBenefit',
                data: { "SID": dataRow.SID },
                type: "post",
                dataType: 'json',
                async: false, // if need page refresh, please remark this option
                success: function (data) {
                    $("#txtBenbfitDate").val(data[0].BenbfitDate);
                    $("#txtVendorCode").val(data[0].VendorCode);
                    $("#txtTotolScore").val(data[0].TotolScore);
                    $("#txtMaterialType").val(data[0].MaterialType);
                    $("#txtClass").val(data[0].Class);
                    $("#txtBatchNum1").val(data[0].BatchNum1);
                    $("#txtRejectNum1").val(data[0].RejectNum1);
                    $("#txtRejectRate").val(data[0].RejectRate);
                    $("#txtTargetValue1").val(data[0].TargetValue1);
                    $("#txtScore1").val(data[0].Score1);
                    $("#txtProductionNum").val(data[0].ProductionNum);
                    $("#txtDefectQuantity").val(data[0].DefectQuantity);
                    $("#txtRejectRatio").val(data[0].RejectRatio);
                    $("#txtTargetValue2").val(data[0].TargetValue2);
                    $("#txtScore2").val(data[0].Score2);
                    $("#txtTotalRecoveryDays").val(data[0].TotalRecoveryDays);
                    $("#txtReplyQuantity").val(data[0].ReplyQuantity);
                    $("#txtMDO").val(data[0].MDO);
                    $("#txtTargetValue3").val(data[0].TargetValue3);
                    $("#txtScore3").val(data[0].Score3);
                    $("#txtXRF").val(data[0].XRF);
                    $("#txtBatchNum2").val(data[0].BatchNum2);
                    $("#txtRejectNum2").val(data[0].RejectNum2);
                    $("#txtTargetValue4").val(data[0].TargetValue4);
                    $("#txtScore4").val(data[0].Score4);
                    $("#txtA").val(data[0].A);
                    $("#txtB").val(data[0].B);
                    $("#txtC").val(data[0].C);
                    $("#txtD").val(data[0].D);
                    $("#txtE").val(data[0].E);
                    $("#txtF").val(data[0].F);
                    $("#txtG").val(data[0].G);
                    $("#txtH").val(data[0].H);
                    $("#txtScore5").val(data[0].Score5);
                    $("#txtScore6").val(data[0].Score6);
                    $("#txtScore7").val(data[0].Score7);
                    $("#txtNOTE").val(data[0].NOTE);
                    $("#txtIQC").val(data[0].IQC);
                    $("#txtSQE").val(data[0].SQE);
                    $("#txtSCOURCER").val(data[0].SCOURCER);
                    $("#txtBUYER").val(data[0].BUYER);
                    setRadio("Isvalid", data[0].Isvalid);
                    $("#txtMonth1").val(data[0].Month1);
                    $("#txtDay1").val(data[0].Day1);
                    $("#txtMonth2").val(data[0].Month2);
                    $("#txtDay2").val(data[0].Day2);
                    $("#txtHour").val(data[0].Hour);
                    $('#tb1').hide();
                    $('#back').show();
                    $('#tb2').show();
                    $('#btnSave2').show();
                    $('#btnSave').hide();
                },
                error: function (xhr, textStatus, thrownError) {
                    $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                },
                complete: function (jqXHR, textStatus) {

                }
            });
         
        } else { alert("Please select a row data to show."); }

    });



    jQuery("#btnBack").click(function () {
        $('#tb1').show();
        $('#tb2').hide();
        $('#back').hide();
        $("#btnSearch").click();
    });
    jQuery("#btnSave2").click(function () {
        $(this).removeClass('ui-state-focus');
        var DoSuccessfully = false;
        $.ajax({
            url: __WebAppPathPrefix + "/SQMBenefit/Edit",
            data: {
                "SID": benefitSID,
                "BenbfitDate": escape($.trim($('#txtBenbfitDate').val())),
                "VendorCode": escape($.trim($('#txtVendorCode').val())),
                "TotolScore": escape($.trim($('#txtTotolScore').val())),
                "MaterialType": escape($.trim($('#txtMaterialType').val())),
                "Class": escape($.trim($('#txtClass').val())),
                "BatchNum1": escape($.trim($('#txtBatchNum1').val())),
                "RejectNum1": escape($.trim($('#txtRejectNum1').val())),
                "RejectRate": escape($.trim($('#txtRejectRate').val())),
                "TargetValue1": escape($.trim($('#txtTargetValue1').val())),
                "Score1": escape($.trim($('#txtScore1').val())),
                "ProductionNum": escape($.trim($('#txtProductionNum').val())),
                "DefectQuantity": escape($.trim($('#txtDefectQuantity').val())),
                "RejectRatio": escape($.trim($('#txtRejectRatio').val())),
                "TargetValue2": escape($.trim($('#txtTargetValue2').val())),
                "Score2": escape($.trim($('#txtScore2').val())),
                "TotalRecoveryDays": escape($.trim($('#txtTotalRecoveryDays').val())),
                "ReplyQuantity": escape($.trim($('#txtReplyQuantity').val())),
                "MDO": escape($.trim($('#txtMDO').val())),
                "TargetValue3": escape($.trim($('#txtTargetValue3').val())),
                "Score3": escape($.trim($('#txtScore3').val())),
                "XRF": escape($.trim($('#txtXRF').val())),
                "BatchNum2": escape($.trim($('#txtBatchNum2').val())),
                "RejectNum2": escape($.trim($('#txtRejectNum2').val())),
                "TargetValue4": escape($.trim($('#txtTargetValue4').val())),
                "Score4": escape($.trim($('#txtScore4').val())),
                "A": escape($.trim($('#txtA').val())),
                "B": escape($.trim($('#txtB').val())),
                "C": escape($.trim($('#txtC').val())),
                "D": escape($.trim($('#txtD').val())),
                "E": escape($.trim($('#txtE').val())),
                "F": escape($.trim($('#txtF').val())),
                "G": escape($.trim($('#txtG').val())),
                "H": escape($.trim($('#txtH').val())),
                "Score5": escape($.trim($('#txtScore5').val())),
                "Score6": escape($.trim($('#txtScore6').val())),
                "Score7": escape($.trim($('#txtScore7').val())),
                "NOTE": escape($.trim($('#txtNOTE').val())),
                "IQC": escape($.trim($('#txtIQC').val())),
                "SQE": escape($.trim($('#txtSQE').val())),
                "SCOURCER": escape($.trim($('#txtSCOURCER').val())),
                "BUYER": escape($.trim($('#txtBUYER').val())),
                "Month1": escape($.trim($('#txtMonth1').val())),
                "Day1": escape($.trim($('#txtDay1').val())),
                "Month2": escape($.trim($('#txtMonth2').val())),
                "Day2": escape($.trim($('#txtDay2').val())),
                "Hour": escape($.trim($('#txtHour').val())),
                "Isvalid": $("input[name='Isvalid']:checked").val()



            },
            type: "post",
            dataType: 'text',
            async: false,
            success: function (data) {
                if (data == "") {
                    DoSuccessfully = true;
                    alert("Edit successfully.");

                }
                else {
                    data = data.replace("<br />", "\u000d");
                    alert("error:" + data);
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

        }
    });
    jQuery("#btnSave").click(function () {
        $(this).removeClass('ui-state-focus');
        var DoSuccessfully = false;
        $.ajax({
            url: __WebAppPathPrefix + "/SQMBenefit/Create",
            data: {
                "BenbfitDate": escape($.trim($('#txtBenbfitDate').val())),
                "VendorCode": escape($.trim($('#txtVendorCode').val())),
                "TotolScore": escape($.trim($('#txtTotolScore').val())),
                "MaterialType": escape($.trim($('#txtMaterialType').val())),
                "Class": escape($.trim($('#txtClass').val())),
                "BatchNum1": escape($.trim($('#txtBatchNum1').val())),
                "RejectNum1": escape($.trim($('#txtRejectNum1').val())),
                "RejectRate": escape($.trim($('#txtRejectRate').val())),
                "TargetValue1": escape($.trim($('#txtTargetValue1').val())),
                "Score1": escape($.trim($('#txtScore1').val())),
                "ProductionNum": escape($.trim($('#txtProductionNum').val())),
                "DefectQuantity": escape($.trim($('#txtDefectQuantity').val())),
                "RejectRatio": escape($.trim($('#txtRejectRatio').val())),
                "TargetValue2": escape($.trim($('#txtTargetValue2').val())),
                "Score2": escape($.trim($('#txtScore2').val())),
                "TotalRecoveryDays": escape($.trim($('#txtTotalRecoveryDays').val())),
                "ReplyQuantity": escape($.trim($('#txtReplyQuantity').val())),
                "MDO": escape($.trim($('#txtMDO').val())),
                "TargetValue3": escape($.trim($('#txtTargetValue3').val())),
                "Score3": escape($.trim($('#txtScore3').val())),
                "XRF": escape($.trim($('#txtXRF').val())),
                "BatchNum2": escape($.trim($('#txtBatchNum2').val())),
                "RejectNum2": escape($.trim($('#txtRejectNum2').val())),
                "TargetValue4": escape($.trim($('#txtTargetValue4').val())),
                "Score4": escape($.trim($('#txtScore4').val())),
                "A": escape($.trim($('#txtA').val())),
                "B": escape($.trim($('#txtB').val())),
                "C": escape($.trim($('#txtC').val())),
                "D": escape($.trim($('#txtD').val())),
                "E": escape($.trim($('#txtE').val())),
                "F": escape($.trim($('#txtF').val())),
                "G": escape($.trim($('#txtG').val())),
                "H": escape($.trim($('#txtH').val())),
                "Score5": escape($.trim($('#txtScore5').val())),
                "Score6": escape($.trim($('#txtScore6').val())),
                "Score7": escape($.trim($('#txtScore7').val())),
                "NOTE": escape($.trim($('#txtNOTE').val())),
                "IQC": escape($.trim($('#txtIQC').val())),
                "SQE": escape($.trim($('#txtSQE').val())),
                "SCOURCER": escape($.trim($('#txtSCOURCER').val())),
                "BUYER": escape($.trim($('#txtBUYER').val())),
                "Month1": escape($.trim($('#txtMonth1').val())),
                "Day1": escape($.trim($('#txtDay1').val())),
                "Month2": escape($.trim($('#txtMonth2').val())),
                "Day2": escape($.trim($('#txtDay2').val())),
                "Hour": escape($.trim($('#txtHour').val())),
                "Isvalid": $("input[name='Isvalid']:checked").val()
            },
            type: "post",
            dataType: 'text',
            async: false,
            success: function (data) {
                if (data == "") {
                    DoSuccessfully = true;
                    alert("Create successfully.");
                    $('#tb1').show();
                    $('#tb2').hide();
                    $('#back').hide();
                    $("#btnSearch").click();
                }
                else {
                    data = data.replace("<br />", "\u000d");
                    alert("error:" + data);
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

        }
    });
    jQuery("#btnExcle1").click(function () {
        $.ajax({
            url: __WebAppPathPrefix + '/SQMBenefit/ExportMonth',
            data: {  },
            type: "post",
            dataType: 'text',
            async: false, // if need page refresh, please remark this option
            success: function (data) {
                downloadFile(data);
            }
        });
    });
    jQuery("#btnExcle2").click(function () {
        $.ajax({
            url: __WebAppPathPrefix + '/SQMBenefit/ExportYear',
            data: {},
            type: "post",
            dataType: 'text',
            async: false, // if need page refresh, please remark this option
            success: function (data) {
                downloadFile(data);
            }
        });
    });

    function downloadFile(data) {
        var dom = document.getElementById('ifile');
        dom.src ="http:"+ data;
    }

});
Date.prototype.yyyy_mm_dd = function () {
    var date = new Date();
    var yyyy = date.getFullYear();
    var mm = date.getMonth() < 9 ? "0" + (date.getMonth() + 1) : (date.getMonth() + 1); // getMonth() is zero-based
    var dd = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
    return "".concat(yyyy).concat('/').concat(mm).concat('/').concat(dd);
};