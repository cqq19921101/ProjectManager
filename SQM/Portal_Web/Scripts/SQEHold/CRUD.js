$(function () {
    var gridDataList = $("#gridDataList");
    var dialog = $("#dialogData");
   

    jQuery("#btnCreate").click(function () {
       
        $("#ddlMaterialType").val(1);
        $("#txtDept").val("");
        $("#txtHoldNo").val("");
      
        $("#txtVenderCode").val("");
        $("#txtLitNo").val("");
      
        $("#txtSize").val("");
        $("#txtPeriod").val("");
        $("#txtbatch").val("");
        $("#txtUpTime").val("");
        $("#txtEndTime").val("");
        $("#txtReason").val("");
        $("#txtPrincipal1").val("");
        $("#txtNTSNum1").val("");
        $("#txtNTSTime1").val("");
        $("#txtAuditer1").val("");
        $("#txtPrincipal2").val("");
        $("#txtNTSNum2").val("");
        $("#txtNTSTime2").val("");
        $("#txtAuditer2").val("");
        $("#txtPrincipal3").val("");
        $("#txtNTSNum3").val("");
        $("#txtNTSTime3").val("");
        $("#txtAuditer3").val("");
        $("#txtPrincipal4").val("");
        $("#txtNTSNum4").val("");
        $("#txtNTSTime4").val("");
        $("#txtAuditer4").val("");
        $("#txtPrincipal5").val("");
        $("#txtNTSNum5").val("");
        $("#txtNTSTime5").val("");
        $("#txtAuditer5").val("");
        $("#txtPrincipal6").val("");
        $("#txtNTSNum6").val("");
        $("#txtNTSTime6").val("");
        $("#txtAuditer6").val("");
        $("#txtPrincipal7").val("");
        $("#txtNTSNum7").val("");
        $("#txtNTSTime7").val("");
        $("#txtAuditer7").val("");
        $("#txtPrincipal8").val("");
        $("#txtNTSNum8").val("");
        $("#txtNTSTime8").val("");
        $("#txtAuditer8").val("");
        $("#txtPrincipal9").val("");
        $("#txtNTSNum9").val("");
        $("#txtNTSTime9").val("");
        $("#txtAuditer9").val("");
        $("#txtPrincipal10").val("");
        $("#txtNTSNum10").val("");
        $("#txtNTSTime10").val("");
        $("#txtAuditer10").val("");
        $("#txtPrincipal11").val("");
        $("#txtNTSNum11").val("");
        $("#txtNTSTime11").val("");
        $("#txtAuditer11").val("");
        $('#div1').hide();
        $('#table1').show();
        $('#table2').hide();

        $('#back').show();
        $('#btnSave').show();
        $('#btnSave1').hide();
        $('#btnSave2').hide();
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



   

 
    $("#txtCreatTime").datepicker({
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
    $("#txtCreatTime").datepicker("setDate", '-0d');
    $("#txtUpTime").datepicker({
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
    $("#txtUpTime").datepicker("setDate", '-0d');
    $("#txtEndTime").datepicker({
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
    $("#txtEndTime").datepicker("setDate", '-0d');
    $("#txtNTSTime1").datepicker({
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
    $("#txtNTSTime1").datepicker("setDate", '-0d');
    $("#txtNTSTime2").datepicker({
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
    $("#txtNTSTime2").datepicker("setDate", '-0d');
    $("#txtNTSTime3").datepicker({
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
    $("#txtNTSTime3").datepicker("setDate", '-0d');
    $("#txtNTSTime4").datepicker({
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
    $("#txtNTSTime4").datepicker("setDate", '-0d');
    $("#txtNTSTime5").datepicker({
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
    $("#txtNTSTime5").datepicker("setDate", '-0d');
    $("#txtNTSTime6").datepicker({
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
    $("#txtNTSTime6").datepicker("setDate", '-0d');
    $("#txtNTSTime7").datepicker({
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
    $("#txtNTSTime7").datepicker("setDate", '-0d');
    $("#txtNTSTime8").datepicker({
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
    $("#txtNTSTime8").datepicker("setDate", '-0d');
    $("#txtNTSTime9").datepicker({
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
    $("#txtNTSTime9").datepicker("setDate", '-0d');
    $("#txtNTSTime10").datepicker({
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
    $("#txtNTSTime10").datepicker("setDate", '-0d');
    $("#txtNTSTime11").datepicker({
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
    $("#txtNTSTime11").datepicker("setDate", '-0d');

   
    jQuery("#btnShow").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {   //single select
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            SID = dataRow.SID;
            LitNo = dataRow.LitNo;
            $.ajax({
                url: __WebAppPathPrefix + '/Hold/GetHoldData',
                data: { "SID": dataRow.SID },
                type: "post",
                dataType: 'json',
                async: false, // if need page refresh, please remark this option
                success: function (data) {
                    $("#ddlMaterialType").val($.trim(data[0].ReportType));
                    $("#txtDept").val(data[0].Dept);
                    $("#txtHoldNo").val(data[0].HoldNo);
                    $("#txtCreatTime").val(data[0].InsertTime);
                    $("#txtVenderCode").val(data[0].vender);
                    $("#txtLitNo").val(data[0].LitNo);
                    $("#txtSize").val(data[0].Size);
                    $("#txtPeriod").val(data[0].Period);
                    $("#txtbatch").val(data[0].batch);
                    $("#txtUpTime").val(data[0].UpTime);
                    $("#txtEndTime").val(data[0].EndTime);
                    $("#txtReason").val(data[0].Reason);
                    $("#txtPrincipal1").val(data[0].Principal1);
                    $("#txtNTSNum1").val(data[0].NTSNum1);
                    $("#txtNTSTime1").val(data[0].NTSTime1);
                    $("#txtAuditer1").val(data[0].Auditer1);
                    $("#txtPrincipal2").val(data[0].Principal2);
                    $("#txtNTSNum2").val(data[0].NTSNum2);
                    $("#txtNTSTime2").val(data[0].NTSTime2);
                    $("#txtAuditer2").val(data[0].Auditer2);
                    $("#txtPrincipal3").val(data[0].Principal3);
                    $("#txtNTSNum3").val(data[0].NTSNum3);
                    $("#txtNTSTime3").val(data[0].NTSTime3);
                    $("#txtAuditer3").val(data[0].Auditer3);
                    $("#txtPrincipal4").val(data[0].Principal4);
                    $("#txtNTSNum4").val(data[0].NTSNum4);
                    $("#txtNTSTime4").val(data[0].NTSTime4);
                    $("#txtAuditer4").val(data[0].Auditer4);
                    $("#txtPrincipal5").val(data[0].Principal5);
                    $("#txtNTSNum5").val(data[0].NTSNum5);
                    $("#txtNTSTime5").val(data[0].NTSTime5);
                    $("#txtAuditer5").val(data[0].Auditer5);
                    $("#txtPrincipal6").val(data[0].Principal6);
                    $("#txtNTSNum6").val(data[0].NTSNum6);
                    $("#txtNTSTime6").val(data[0].NTSTime6);
                    $("#txtAuditer6").val(data[0].Auditer6);
                    $("#txtPrincipal7").val(data[0].Principal7);
                    $("#txtNTSNum7").val(data[0].NTSNum7);
                    $("#txtNTSTime7").val(data[0].NTSTime7);
                    $("#txtAuditer7").val(data[0].Auditer7);
                    $("#txtPrincipal8").val(data[0].Principal8);
                    $("#txtNTSNum8").val(data[0].NTSNum8);
                    $("#txtNTSTime8").val(data[0].NTSTime8);
                    $("#txtAuditer8").val(data[0].Auditer8);
                    $("#txtPrincipal9").val(data[0].Principal9);
                    $("#txtNTSNum9").val(data[0].NTSNum9);
                    $("#txtNTSTime9").val(data[0].NTSTime9);
                    $("#txtAuditer9").val(data[0].Auditer9);
                    $("#txtPrincipal10").val(data[0].Principal10);
                    $("#txtNTSNum10").val(data[0].NTSNum10);
                    $("#txtNTSTime10").val(data[0].NTSTime10);
                    $("#txtAuditer10").val(data[0].Auditer10);
                    $("#txtPrincipal11").val(data[0].Principal11);
                    $("#txtNTSNum11").val(data[0].NTSNum11);
                    $("#txtNTSTime11").val(data[0].NTSTime11);
                    $("#txtAuditer11").val(data[0].Auditer11);
                    

                  
                    $('#div1').hide();
                    $('#table1').show();
                    $('#table2').hide();
                    $('#table3').hide();
                    $('#back').show();
                    $('#btnSave').hide();
                    $('#btnSave1').hide();
                    $('#btnSave2').hide();
                 
                },
                error: function (xhr, textStatus, thrownError) {
                    $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                },
                complete: function (jqXHR, textStatus) {

                }
            });
       
        } else { alert("Please select a row data to show."); }

    });
    jQuery("#btnDelete").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect')) {   //single select
            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                var DataKey = (gridDataList.jqGrid('getRowData', RowId)).SID;
                //var r = AcquireDataLock(DataKey)
                //if (r == "ok") {
                if (confirm("Confirm to delete selected member (" + gridDataList.jqGrid('getRowData', RowId).SID + ")?\n\n(Note. Data cannot be recovered once deleted)")) {
                    $.ajax({
                        url: __WebAppPathPrefix + "/Hold/Delete",
                        data: { "SID": gridDataList.jqGrid('getRowData', RowId).SID },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                $("#btnSearch").click();
                                alert("Hold delete successfully.");
                            }
                            else {
                                alert("Hold delete fail due to:\n\n" + data);
                            }
                        },
                        error: function (xhr, textStatus, thrownError) {
                            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                        },
                        complete: function (jqXHR, textStatus) {
                            //$("#ajaxLoading").hide();
                        }
                    });
                }
                else {
                    var r = ReleaseDataLock(DataKey);
                    if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
                }
                //}
                //else {
                //    switch (r) {
                //        case "timeout": $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); break;
                //        case "l": alert("Data already lock by other user."); break;
                //        default: alert("Data lock fail or application error."); break;
                //    }
                //}
            } else { alert("Please select a row data to delete."); }
        }
    });

    jQuery("#btnBack").click(function () {
        $('#div1').show();
        $('#table1').hide();
        $('#table2').hide();
        $('#table3').hide();
        $('#back').hide();
        $('#btnSave1').hide();
        $('#btnSave2').hide();
        $("#btnSearch").click();
    });
    jQuery("#btnSave").click(function () {
        $(this).removeClass('ui-state-focus');
        var DoSuccessfully = false;
        $.ajax({
            url: __WebAppPathPrefix + "/Hold/Create",
            data: {
                "ReportType": escape($.trim($('#ddlMaterialType').val())),
                "Dept": escape($.trim($('#txtDept').val())),
                "HoldNo": escape($.trim($('#txtHoldNo').val())),
                "CreatTime": escape($.trim($('#txtCreatTime').val())),
                "Vender": escape($.trim($('#txtVenderCode').val())),
                "LitNo": escape($.trim($('#txtLitNo').val())),
                "Size": escape($.trim($('#txtSize').val())),
                "Period": escape($.trim($('#txtPeriod').val())),
                "batch": escape($.trim($('#txtbatch').val())),
                "UpTime": escape($.trim($('#txtUpTime').val())),
                "EndTime": escape($.trim($('#txtEndTime').val())),
                "Reason": escape($.trim($('#txtReason').val())),
                "Principal1": escape($.trim($('#txtPrincipal1').val())),
                "NTSNum1": escape($.trim($('#txtNTSNum1').val())),
                "NTSTime1": escape($.trim($('#txtNTSTime1').val())),
                "Auditer1": escape($.trim($('#txtAuditer1').val())),
                "Principal2": escape($.trim($('#txtPrincipal2').val())),
                "NTSNum2": escape($.trim($('#txtNTSNum2').val())),
                "NTSTime2": escape($.trim($('#txtNTSTime2').val())),
                "Auditer2": escape($.trim($('#txtAuditer2').val())),
                "Principal3": escape($.trim($('#txtPrincipal3').val())),
                "NTSNum3": escape($.trim($('#txtNTSNum3').val())),
                "NTSTime3": escape($.trim($('#txtNTSTime3').val())),
                "Auditer3": escape($.trim($('#txtAuditer3').val())),
                "Principal4": escape($.trim($('#txtPrincipal4').val())),
                "NTSNum4": escape($.trim($('#txtNTSNum4').val())),
                "NTSTime4": escape($.trim($('#txtNTSTime4').val())),
                "Auditer4": escape($.trim($('#txtAuditer4').val())),
                "Principal5": escape($.trim($('#txtPrincipal5').val())),
                "NTSNum5": escape($.trim($('#txtNTSNum5').val())),
                "NTSTime5": escape($.trim($('#txtNTSTime5').val())),
                "Auditer5": escape($.trim($('#txtAuditer5').val())),
                "Principal6": escape($.trim($('#txtPrincipal6').val())),
                "NTSNum6": escape($.trim($('#txtNTSNum6').val())),
                "NTSTime6": escape($.trim($('#txtNTSTime6').val())),
                "Auditer6": escape($.trim($('#txtAuditer6').val())),
                "Principal7": escape($.trim($('#txtPrincipal7').val())),
                "NTSNum7": escape($.trim($('#txtNTSNum7').val())),
                "NTSTime7": escape($.trim($('#txtNTSTime7').val())),
                "Auditer7": escape($.trim($('#txtAuditer7').val())),
                "Principal8": escape($.trim($('#txtPrincipal8').val())),
                "NTSNum8": escape($.trim($('#txtNTSNum8').val())),
                "NTSTime8": escape($.trim($('#txtNTSTime8').val())),
                "Auditer8": escape($.trim($('#txtAuditer8').val())),
                "Principal9": escape($.trim($('#txtPrincipal9').val())),
                "NTSNum9": escape($.trim($('#txtNTSNum9').val())),
                "NTSTime9": escape($.trim($('#txtNTSTime9').val())),
                "Auditer9": escape($.trim($('#txtAuditer9').val())),
                "Principal10": escape($.trim($('#txtPrincipal10').val())),
                "NTSNum10": escape($.trim($('#txtNTSNum10').val())),
                "NTSTime10": escape($.trim($('#txtNTSTime10').val())),
                "Auditer10": escape($.trim($('#txtAuditer10').val())),
                "Principal11": escape($.trim($('#txtPrincipal11').val())),
                "NTSNum11": escape($.trim($('#txtNTSNum11').val())),
                "NTSTime11": escape($.trim($('#txtNTSTime11').val())),
                "Auditer11": escape($.trim($('#txtAuditer11').val())),
            },
            type: "post",
            dataType: 'text',
            async: false,
            success: function (data) {
                if (data == "") {
                    DoSuccessfully = true;
                    alert("Create successfully.");
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
            $('#div1').show();
            $('#table1').hide();
            $('#table2').hide();
            $('#back').hide();
            $("#btnSearch").click();
        }
    });

    jQuery("#btnQAApprove").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect')) {   //single select
            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                var DataKey = (gridDataList.jqGrid('getRowData', RowId)).SID;
                    $.ajax({
                        url: __WebAppPathPrefix + "/Hold/UpdateStatus",
                        data: { "SID": gridDataList.jqGrid('getRowData', RowId).SID,"status":1 },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                $("#btnSearch").click();
                                alert("Approve successfully.");
                            }
                            else {
                                alert("Approve fail due to:\n\n" + data);
                            }
                        },
                        error: function (xhr, textStatus, thrownError) {
                            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                        },
                        complete: function (jqXHR, textStatus) {
                            //$("#ajaxLoading").hide();
                        }
                    });
                
               
          
            } else { alert("Please select a row data to delete."); }
        }
    });
    jQuery("#btnQAReject").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect')) {   //single select
            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                var DataKey = (gridDataList.jqGrid('getRowData', RowId)).SID;
                $.ajax({
                    url: __WebAppPathPrefix + "/Hold/UpdateStatus",
                    data: { "SID": gridDataList.jqGrid('getRowData', RowId).SID, "status": 0 },
                    type: "post",
                    dataType: 'text',
                    async: false,
                    success: function (data) {
                        if (data == "") {
                            $("#btnSearch").click();
                            alert("Reject successfully.");
                        }
                        else {
                            alert("Reject fail due to:\n\n" + data);
                        }
                    },
                    error: function (xhr, textStatus, thrownError) {
                        $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                    },
                    complete: function (jqXHR, textStatus) {
                        //$("#ajaxLoading").hide();
                    }
                });



            } else { alert("Please select a row data to delete."); }
        }
    });
    jQuery("#btnReject").click(function () {
        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {
        $('#div1').hide();
        $('#table1').hide();
        $('#table2').hide();
        $('#table3').show();
        $('#back').show();
        $('#btnSave').hide();
        $('#btnSave1').hide();
        $('#btnSave2').show();
        $("#txtRejectQuantity").val("");
          } else { alert("Please select a row data ."); }
    });
    jQuery("#btnRelease").click(function () {
        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {
        $('#div1').hide();
        $('#table1').hide();
        $('#table2').show();
        $('#table3').hide();
        $('#back').show();
        $('#btnSave').hide();
        $('#btnSave1').show();
        $('#btnSave2').hide();
        $("#txtReleaseQuantity").val("");
        } else { alert("Please select a row data ."); }
    });
    jQuery("#btnSave1").click(function () {
        $(this).removeClass('ui-state-focus');
          
            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                var DataKey = (gridDataList.jqGrid('getRowData', RowId)).SID;
                $.ajax({
                    url: __WebAppPathPrefix + "/Hold/UpdateReleaseQuantity",
                    data: { "SID": gridDataList.jqGrid('getRowData', RowId).SID, "ReleaseQuantity": escape($.trim($('#txtReleaseQuantity').val())) },
                    type: "post",
                    dataType: 'text',
                    async: false,
                    success: function (data) {
                        if (data == "") {
                            $("#btnSearch").click();
                            alert("Reject successfully.");
                        }
                        else {
                            alert("Reject fail due to:\n\n" + data);
                        }
                    },
                    error: function (xhr, textStatus, thrownError) {
                        $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                    },
                    complete: function (jqXHR, textStatus) {
                        //$("#ajaxLoading").hide();
                    }
                });



          
        }
    });
    jQuery("#btnSave2").click(function () {
        $(this).removeClass('ui-state-focus');
       
            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                var DataKey = (gridDataList.jqGrid('getRowData', RowId)).SID;
                $.ajax({
                    url: __WebAppPathPrefix + "/Hold/UpdateRejectQuantity",
                    data: { "SID": gridDataList.jqGrid('getRowData', RowId).SID, "RejectQuantity": escape($.trim($('#txtRejectQuantity').val())) },
                    type: "post",
                    dataType: 'text',
                    async: false,
                    success: function (data) {
                        if (data == "") {
                            $("#btnSearch").click();
                            alert("Reject successfully.");
                        }
                        else {
                            alert("Reject fail due to:\n\n" + data);
                        }
                    },
                    error: function (xhr, textStatus, thrownError) {
                        $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                    },
                    complete: function (jqXHR, textStatus) {
                        //$("#ajaxLoading").hide();
                    }
                });



            
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