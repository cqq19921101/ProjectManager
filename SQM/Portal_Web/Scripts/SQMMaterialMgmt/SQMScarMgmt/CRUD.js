$(function () {
    var gridDataList = $("#gridDataList");
    var dialog = $("#dialogData");
   

    jQuery("#btnCreate").click(function () {
        $(this).removeClass('ui-state-focus');
        $("#txtAnomalousTime").val("");
      
        $("#txtLitNo").val("");
   
        $("#txtModel").val("");
  
        $("#txtBitNo").val("");
       
        $("#txtBitNum").val("");
      
        $("#txtbadnessNum").val("");
    
        $("#txtRejectRatio").val("");
        
        $("#ddlAbnormal").val("");
        
        $("#txtVenderCode").val("");
   
        $("#txtbadnessNote").val("");
       
        $("#txtbadnessPic").val("");
        $("#spSCARD1").html("empty");
        $("#txtinitiator").val("");
        $("#txtinitiator").attr("disabled", "disabled");
        $('#D1').hide();
        $('#D9').show();
        $('#btnD9Save').show();
        $('#back').show();
    });

    jQuery("#btnViewEdit").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect')) {   //single select
            SetToViewMode();
        }
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

    $("#txtAnomalousTime").datepicker({
        changeMonth: true,
        changeYear:true,
        dateFormat: 'yy/mm/dd',
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            } catch (err) {
                $(this).datepicker("setDate", '-31d');
            }
        }
    });
    $("#txtAnomalousTime").datepicker("setDate", '-31d');
    $("#txtOverTime1").datepicker({
        changeMonth: true,
        changeYear:true,
        dateFormat: 'yy/mm/dd',
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            } catch (err) {
                $(this).datepicker("setDate", '-31d');
            }
        }
    });
    $("#txtOverTime1").datepicker("setDate", '-31d');

    $("#txtOverTime2").datepicker({
        changeMonth: true,
        changeYear:true,
        dateFormat: 'yy/mm/dd',
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            } catch (err) {
                $(this).datepicker("setDate", '-31d');
            }
        }
    });
    $("#txtOverTime2").datepicker("setDate", '-31d');
    $("#txtOverTime3").datepicker({
        changeMonth: true,
        changeYear:true,
        dateFormat: 'yy/mm/dd',
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            } catch (err) {
                $(this).datepicker("setDate", '-31d');
            }
        }
    });
    $("#txtOverTime3").datepicker("setDate", '-31d');
    $("#txtFinishDate1").datepicker({
        changeMonth: true,
        changeYear:true,
        dateFormat: 'yy/mm/dd',
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            } catch (err) {
                $(this).datepicker("setDate", '-31d');
            }
        }
    });
    $("#txtFinishDate1").datepicker("setDate", '-31d');
    $("#txtFinishDate2").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'yy/mm/dd',
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            } catch (err) {
                $(this).datepicker("setDate", '-31d');
            }
        }
    });
    $("#txtFinishDate2").datepicker("setDate", '-31d');
    $("#txtProductionTime1").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'yy/mm/dd',
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            } catch (err) {
                $(this).datepicker("setDate", '-31d');
            }
        }
    });
    $("#txtProductionTime1").datepicker("setDate", '-31d');
    $("#txtProductionTime2").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'yy/mm/dd',
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            } catch (err) {
                $(this).datepicker("setDate", '-31d');
            }
        }
    });
    $("#txtProductionTime2").datepicker("setDate", '-31d');
    $("#txtProductionTime3").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'yy/mm/dd',
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            } catch (err) {
                $(this).datepicker("setDate", '-31d');
            }
        }
    });
    $("#txtProductionTime3").datepicker("setDate", '-31d');
    $("#txtcommissioningdate1").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'yy/mm/dd',
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            } catch (err) {
                $(this).datepicker("setDate", '-31d');
            }
        }
    });
    $("#txtcommissioningdate1").datepicker("setDate", '-31d');
    $("#txtcommissioningdate2").datepicker({
        changeMonth: true,
        changeYear:true,
        dateFormat: 'yy/mm/dd',
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            } catch (err) {
                $(this).datepicker("setDate", '-31d');
            }
        }
    });
    $("#txtcommissioningdate2").datepicker("setDate", '-31d');
    $("#txtcommissioningdate3").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'yy/mm/dd',
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            } catch (err) {
                $(this).datepicker("setDate", '-31d');
            }
        }
    });
    $("#txtcommissioningdate3").datepicker("setDate", '-31d');
  
   
    var SCARSID;
    var LitNo;
    jQuery("#btnShow").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {   //single select
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            SCARSID = dataRow.SID;
            LitNo = dataRow.LitNo;
            $.ajax({
                url: __WebAppPathPrefix + '/SQMMaterialMgmt/GetSQMSCARData',
                data: { "SID": dataRow.SID },
                type: "post",
                dataType: 'json',
                async: false, // if need page refresh, please remark this option
                success: function (data) {
                    if (data[0].IsDuty) {
                        $("#cb" + "IsDuty").prop('checked', true);
                    }
                    else {
                        $("#cb" + "IsDuty_1").prop('checked', true);
                    }
                    $("#spSCARD1").html(data[0].BadnessPic);
                    $("#txtAnomalousTime").val(new Date(data[0].AnomalousTime).yyyy_mm_dd);
                    $("#txtLitNo").val(data[0].LitNo);
                    $("#txtModel").val(data[0].Model);
                    $("#txtBitNo").val(data[0].BitNo);
                    $("#txtBitNum").val(data[0].BitNum);
                    $("#txtbadnessNum").val(data[0].BadnessNum);
                    $("#txtRejectRatio").val(data[0].RejectRatio);
                    $("#ddlAbnormal").val(data[0].Abnormal);
                    $("#txtVenderCode").val(data[0].VenderCode);
                    $("#txtbadnessNote").val(data[0].BadnessNote);
                   
                    
                    $("#txtinitiator").val(data[0].Initiator);
                    //split
                    $("#txtDutyNote").val(data[0].DutyNote);
                    $("#txtTimmer").val(data[0].Timmer);
                    $("#txtTimmerPhone").val(data[0].TimmerPhone);
                    $("#txtGroupMember").val(data[0].GroupMember);
                    $("#txtProvisional").val(data[0].Provisional);
                    $("#txtInventoryNum1").val(data[0].InventoryNum1);
                    $("#txtInventoryNum2").val(data[0].InventoryNum2);
                    $("#txtInventoryNum3").val(data[0].InventoryNum3);
                    $("#txtDisposeType1").val(data[0].DisposeType1);
                    $("#txtDisposeType2").val(data[0].DisposeType2);
                    $("#txtDisposeType3").val(data[0].DisposeType3);
                    $("#txtOverTime1").val(new Date(data[0].OverTime1).yyyy_mm_dd);
                    $("#txtOverTime2").val(new Date(data[0].OverTime2).yyyy_mm_dd);
                    $("#txtOverTime3").val(new Date(data[0].OverTime3).yyyy_mm_dd);
                    $("#txtReasonAnalysis").val(data[0].ReasonAnalysis);
                    $("#txtImprovementStrategy").val(data[0].ImprovementStrategy);
                    $("#txtDuty1").val(data[0].Duty1);
                    $("#txtFinishDate1").val(data[0].FinishDate1);
                    $("#txtProductionTime1").val(new Date(data[0].ProductionTime1).yyyy_mm_dd);
                    $("#txtProductionTime2").val(new Date(data[0].ProductionTime2).yyyy_mm_dd);
                    $("#txtProductionTime3").val(new Date(data[0].ProductionTime3).yyyy_mm_dd);
                    $("#txtProductionNo1").val(data[0].ProductionNo1);
                    $("#txtProductionNo2").val(data[0].ProductionNo2);
                    $("#txtProductionNo3").val(data[0].ProductionNo3);
                    $("#txtProductionNum1").val(data[0].ProductionNum1);
                    $("#txtProductionNum2").val(data[0].ProductionNum2);
                    $("#txtProductionNum3").val(data[0].ProductionNum3);
                    if (data[0].IsValid1) {
                        $("#cbIsValid1").prop('checked', true);
                    } else {
                        $("#cbIsValid1_1").prop('checked', true);
                    }
                    if (data[0].IsValid2) {
                        $("#cbIsValid2").prop('checked', true);
                    } else {
                        $("#cbIsValid2_1").prop('checked', true);
                    }
                    if (data[0].IsValid3) {
                        $("#cbIsValid3").prop('checked', true);
                    } else {
                        $("#cbIsValid3_1").prop('checked', true);
                    }
                    if (data[0].Isperfect1) {
                        $("#cbIsperfect1").prop('checked', true);
                    } else {
                        $("#cbIsperfect1_1").prop('checked', true);
                    }
                    if (data[0].Isperfect2) {
                        $("#cbIsperfect2").prop('checked', true);
                    } else {
                        $("#cbIsperfect2_1").prop('checked', true);
                    }
                    $("#txtLitList1").val(data[0].LitList1);
                    $("#txtLitList2").val(data[0].LitList2);
                    $("#txtStrategynote").val(data[0].StrategyNote);
                    $("#txtduty2").val(data[0].Duty2);
                    $("#ddlisover").val(data[0].Isover);
                    $("#txtnote").val(data[0].note);
                    $("#txtFinishDate2").val(new Date(data[0].FinishDate2).yyyy_mm_dd);
                    $("#txtFinishDate1").val(new Date(data[0].FinishDate1).yyyy_mm_dd);
                    if (data[0].Commissioningdate !== null && data[0].Commissioningdate !== undefined && data[0].Commissioningdate !== '') {
                        var commissioningdate = new Array(); //定义一数组 
                        commissioningdate = data[0].Commissioningdate.split(";"); //字符分割 
                        $("#txtcommissioningdate1").val(new Date(commissioningdate[0]).yyyy_mm_dd);
                        $("#txtcommissioningdate2").val(new Date(commissioningdate[1]).yyyy_mm_dd);
                        $("#txtcommissioningdate3").val(new Date(commissioningdate[2]).yyyy_mm_dd);
                    }
                    if (data[0].Productionworkorder !== null && data[0].Productionworkorder !== undefined && data[0].Productionworkorder !== '') {
                        var Productionworkorder = new Array(); //定义一数组 
                        Productionworkorder = data[0].Productionworkorder.split(";");
                        $("#txtProductionworkorder1").val(Productionworkorder[0]);
                        $("#txtProductionworkorder2").val(Productionworkorder[1]);
                        $("#txtProductionworkorder3").val(Productionworkorder[2]);
                    }
                    if (data[0].Productionquantity !== null && data[0].Productionquantity !== undefined && data[0].Productionquantity !== '') {
                        var Productionquantity = new Array(); //定义一数组 
                        Productionquantity = data[0].Productionquantity.split(";");
                        $("#txtProductionquantity1").val(Productionquantity[0]);
                        $("#txtProductionquantity2").val(Productionquantity[1]);
                        $("#txtProductionquantity3").val(Productionquantity[2]);
                    }
                    if (data[0].InputNum !== null && data[0].InputNum !== undefined && data[0].InputNum !== '') {
                        var InputNum = new Array(); //定义一数组 
                        InputNum = data[0].InputNum.split(";");
                        $("#txtInputNum1").val(InputNum[0]);
                        $("#txtInputNum2").val(InputNum[1]);
                        $("#txtInputNum3").val(InputNum[2]);
                    }

                    $("#txtSTDSLRR").val(data[0].STDSLRR);
                    if (data[0].rejectOrder !== null && data[0].rejectOrder !== undefined && data[0].rejectOrder !== '') {
                        var rejectOrder = new Array(); //定义一数组 
                        rejectOrder = data[0].rejectOrder.split(";");
                        $("#txtrejectOrder1").val(rejectOrder[0]);
                        $("#txtrejectOrder2").val(rejectOrder[1]);
                        $("#txtrejectOrder3").val(rejectOrder[2]);
                        if (parseInt(rejectOrder[0]) < parseInt(data[0].STDSLRR)) {
                            $("#cbIsvalid4").prop('checked', true);
                        } else {
                            $("#cbIsvalid4_1").prop('checked', true);
                        }
                        if (parseInt(rejectOrder[1]) < parseInt(data[0].STDSLRR)) {
                            $("#cbIsvalid5").prop('checked', true);
                        } else {
                            $("#cbIsvalid5_1").prop('checked', true);
                        }
                        if (parseInt(rejectOrder[2]) < parseInt(data[0].STDSLRR)) {
                            $("#cbIsvalid6").prop('checked', true);
                        } else {
                            $("#cbIsvalid6_1").prop('checked', true);
                        }
                    }
              

                    if (data[0].appovestatus1 == '1' || data[0].appovestatus1 == undefined) {
                        $('#D2 *').removeAttr('disabled');
                        $('#D3 *').removeAttr('disabled');
                        $('#D4 *').removeAttr('disabled');
                        $('#D5 *').removeAttr('disabled');
                        $('#D6 *').attr("disabled", "disabled");
                        $('#D7 *').attr("disabled", "disabled");
                        $('#D8 *').attr("disabled", "disabled");
                    } 
                    if (data[0].appovestatus2 == undefined&&data[0].appovestatus1 == '0' ) {
                        $('#D6 *').removeAttr('disabled');
                        $('#D7 *').removeAttr('disabled');
                        $('#D8 *').attr("disabled", "disabled");
                    }
                    if (data[0].appovestatus2 == '1') {
                        $('#D2 *').removeAttr('disabled');
                        $('#D3 *').removeAttr('disabled');
                        $('#D4 *').removeAttr('disabled');
                        $('#D5 *').removeAttr('disabled');
                        $('#D6 *').attr("disabled", "disabled");
                        $('#D7 *').attr("disabled", "disabled");
                        $('#D8 *').attr("disabled", "disabled");
                    }
                    if (data[0].appovestatus2 == '0' && data[0].appovestatus1 == '0') {
                        $('#D2 *').attr("disabled", "disabled");
                        $('#D3 *').attr("disabled", "disabled");
                        $('#D4 *').attr("disabled", "disabled");
                        $('#D5 *').attr("disabled", "disabled");
                        $('#D6 *').removeAttr('disabled');
                        $('#D7 *').removeAttr('disabled');
                        $('#D8 *').attr("disabled", "disabled");
                    }

               
                  
                    

                   
                },
                error: function (xhr, textStatus, thrownError) {
                    $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                },
                complete: function (jqXHR, textStatus) {

                }
            });
            $('#D1').hide();
            $('#back').show();
            $('#D9').show();
            $('#D9 *').attr("disabled", "disabled");
            $('#D2').show();
            $('#D3').show();
            $('#D4').show();
            $('#D5').show();
            $('#D6').show();
            $('#D7').show();
            $('#D8').show();
            $('#btnD9Save').hide();
            $('#btnD2Save').removeAttr('disabled'); 
        } else { alert("Please select a row data to show."); }

    });

    jQuery("#btnOver").click(function () {
   
        $.ajax({
            url: __WebAppPathPrefix + "/SQMMaterialMgmt/GetScarD8",
            data: {
                "SID": SCARSID,
                "LitNo": LitNo,
                "DateCode": escape($.trim($('#txtDateCode').val()))
            },
            type: "post",
            dataType: 'text',
            async: false,
            success: function (data) {
                if (data == "") {
                    DoSuccessfully = true;
                    alert("Appove successfully.");
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

    jQuery("#btnBack").click(function () {
        $('#D1').show();

        $('#back').hide();
        $('#D2').hide();
        $('#D3').hide();
        $('#D4').hide();
        $('#D5').hide();
        $('#D6').hide();
        $('#D7').hide();
        $('#D8').hide();
        $('#D9').hide();
    });
    jQuery("#btnD2Save").click(function () {
        $(this).removeClass('ui-state-focus');
        var DoSuccessfully = false;
        $.ajax({
            url: __WebAppPathPrefix + "/SQMMaterialMgmt/UpdateSQMSCARData",
            data: {
                "SID": SCARSID,
                "IsDuty": escape($.trim($("#cb" + "IsDuty").is(":checked"))),
                "DutyNote": escape($.trim($('#txtDutyNote').val())),
                "Timmer": escape($.trim($('#txtTimmer').val())),
                "TimmerPhone": escape($.trim($('#txtTimmerPhone').val())),
                "GroupMember": escape($.trim($('#txtGroupMember').val())),
                "Provisional": escape($.trim($('#txtProvisional').val())),
                "InventoryNum1": escape($.trim($('#txtInventoryNum1').val())),
                "DisposeType1": escape($.trim($('#txtDisposeType1').val())),
                "OverTime1": escape($.trim($('#txtOverTime1').val())),
                "InventoryNum2": escape($.trim($('#txtInventoryNum2').val())),
                "DisposeType2": escape($.trim($('#txtDisposeType2').val())),
                "OverTime2": escape($.trim($('#txtOverTime2').val())),
                "InventoryNum3": escape($.trim($('#txtInventoryNum3').val())),
                "DisposeType3": escape($.trim($('#txtDisposeType3').val())),
                "OverTime3": escape($.trim($('#txtOverTime3').val())),
                "ReasonAnalysis": escape($.trim($('#txtReasonAnalysis').val())),
                "ImprovementStrategy": escape($.trim($('#txtImprovementStrategy').val())),
                "Duty1": escape($.trim($('#txtDuty1').val())),
                "FinishDate1": escape($.trim($('#txtFinishDate1').val())),
                "ProductionTime1": escape($.trim($('#txtProductionTime1').val())),
                "ProductionTime2": escape($.trim($('#txtProductionTime2').val())),
                "ProductionTime3": escape($.trim($('#txtProductionTime3').val())),
                "ProductionNo1": escape($.trim($('#txtProductionNo1').val())),
                "ProductionNo2": escape($.trim($('#txtProductionNo2').val())),
                "ProductionNo3": escape($.trim($('#txtProductionNo3').val())),
                "ProductionNum1": escape($.trim($('#txtProductionNum1').val())),
                "ProductionNum2": escape($.trim($('#txtProductionNum2').val())),
                "ProductionNum3": escape($.trim($('#txtProductionNum3').val())),
                "IsValid1": escape($.trim($("#cb" + "IsValid1").is(":checked"))),
                "IsValid2": escape($.trim($("#cb" + "IsValid2").is(":checked"))),
                "IsValid3": escape($.trim($("#cb" + "IsValid3").is(":checked"))),
                "Isperfect1": escape($.trim($("#cb" + "Isperfect1").is(":checked"))),
                "LitList1": escape($.trim($('#txtLitList1').val())),
                "Isperfect2": escape($.trim($("#cb" + "Isperfect2").is(":checked"))),
                "LitList2": escape($.trim($('#txtLitList2').val())),
                "StrategyNote": escape($.trim($('#txtStrategynote').val())),
                "Duty2": escape($.trim($('#txtduty2').val())),
                "FinishDate2": escape($.trim($('#txtFinishDate2').val())),
                "FileNo": escape($.trim($("#dialogData").attr("SCARD2FGUID"))),
                "STDSLRR": escape($.trim($('#txtSTDSLRR').val())),
                "Isover": escape($.trim($('#ddlisover').val())),
                "Note": escape($.trim($('#txtnote').val()))
            },
            type: "post",
            dataType: 'text',
            async: false,
            success: function (data) {
                if (data == "") {
                    DoSuccessfully = true;
                    alert("EditSCAR successfully.");
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
    jQuery("#btnAppove1").click(function () {

        $.ajax({
            url: __WebAppPathPrefix + "/SQMMaterialMgmt/Appove",
            data: {
                "SID": SCARSID,
                "Type": 1
            },
            type: "post",
            dataType: 'text',
            async: false,
            success: function (data) {
                if (data == "") {
                    DoSuccessfully = true;
                    alert("Appove successfully.");
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
    jQuery("#btnAppove2").click(function () {
        $.ajax({
            url: __WebAppPathPrefix + "/SQMMaterialMgmt/Appove",
            data: {
                "SID": SCARSID,
                "Type": 2
            },
            type: "post",
            dataType: 'text',
            async: false,
            success: function (data) {
                if (data == "") {
                    DoSuccessfully = true;
                    alert("Appove successfully.");
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
    jQuery("#btnReject1").click(function () {
        $.ajax({
            url: __WebAppPathPrefix + "/SQMMaterialMgmt/Appove",
            data: {
                "SID": SCARSID,
                "Type": 3
            },
            type: "post",
            dataType: 'text',
            async: false,
            success: function (data) {
                if (data == "") {
                    DoSuccessfully = true;
                    alert("Reject successfully.");
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
    jQuery("#btnReject2").click(function () {
        $.ajax({
            url: __WebAppPathPrefix + "/SQMMaterialMgmt/Appove",
            data: {
                "SID": SCARSID,
                "Type": 4
            },
            type: "post",
            dataType: 'text',
            async: false,
            success: function (data) {
                if (data == "") {
                    DoSuccessfully = true;
                    alert("Reject successfully.");
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

    jQuery("#btnD9Save").click(function () {

        $.ajax({
            url: __WebAppPathPrefix + "/SQMMaterialMgmt/CreateSQMSCAR",
            data: {
                "SID": dialog.attr("SID"),
                "anomalousTime": escape($.trim($("#txtAnomalousTime").val())),
                "LitNo": escape($.trim($("#txtLitNo").val())),
                "Model": escape($.trim($("#txtModel").val())),
                "BitNo": escape($.trim($("#txtBitNo").val())),
                "BitNum": escape($.trim($("#txtBitNum").val())),
                "badnessNum": escape($.trim($("#txtbadnessNum").val())),
                "RejectRatio": escape($.trim($("#txtRejectRatio").val())),
                "Abnormal": escape($.trim($("#ddlAbnormal").val())),
                "VenderCode": escape($.trim($("#txtVendorCode").val())),
                "badnessNote": escape($.trim($("#txtbadnessNote").val())),
                "badnessPic": escape($.trim($("#tbMain1").attr("SCARD1FGUID")))
            },
            type: "post",
            dataType: 'text',
            async: false,
            success: function (data) {
                if (data == "") {
                    DoSuccessfully = true;
                    alert("Create successfully.");
                    $('#D1').show();

                    $('#back').hide();
                    $('#D2').hide();
                    $('#D3').hide();
                    $('#D4').hide();
                    $('#D5').hide();
                    $('#D6').hide();
                    $('#D7').hide();
                    $('#D8').hide();
                    $('#D9').hide();
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
});
Date.prototype.yyyy_mm_dd = function () {
    var date = new Date();
    var yyyy = date.getFullYear();
    var mm = date.getMonth() < 9 ? "0" + (date.getMonth() + 1) : (date.getMonth() + 1); // getMonth() is zero-based
    var dd = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
    return "".concat(yyyy).concat('/').concat(mm).concat('/').concat(dd);
};