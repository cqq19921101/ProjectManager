$(function () {
    var gridDataList = $("#gridDataList");
    var dialog = $("#dialogData");
   

    jQuery("#btnCreate").click(function () {
        $(this).removeClass('ui-state-focus');
        $("#txtMRBNo").val("");
        $("#txtinitiator").val("");
        $("#txtgovernor").val("");
        $("#txtVenderCode").val("");
        $("#txtSize").val("");
        $("#txtLitNo").val("");
        $("#txtbatch").val("");
        $("#txtchecknumber").val("");
        $("#txtdefect").val("");
        $("#txtrejectratio").val("");
        $("#txtinspector").val("");
        $("#txtcreatetime").val("");
        $("#txtBaddescription").val("");
        
        setRadio("MRBType", escape($.trim(1)));
        setRadio("venderType", escape($.trim(1)));
        setRadio("Baddescriptiontype", escape($.trim(1)));
        setRadio("meettype", escape($.trim(1)));
        $('#div1').hide();
        $('#table1').show();
        $('#table2').hide();
        $('#table3').hide();
        $('#back').show();
        $('#btnSave').show();
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

    $("#txtcreatetime").datepicker({
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
    $("#txtcreatetime").datepicker("setDate", '-31d');
    $("#txtPMsigndate").datepicker({
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
    $("#txtPMsigndate").datepicker("setDate", '-31d');
    $("#txtQrsigndate").datepicker({
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
    $("#txtQrsigndate").datepicker("setDate", '-31d');
    $("#txtPursigndate").datepicker({
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
    $("#txtPursigndate").datepicker("setDate", '-31d');
    $("#txtPmcsigndate").datepicker({
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
    $("#txtPmcsigndate").datepicker("setDate", '-31d');
    $("#txtMfgsigndate").datepicker({
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
    $("#txtMfgsigndate").datepicker("setDate", '-31d');
    $("#txtSmtengsigndate").datepicker({
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
    $("#txtSmtengsigndate").datepicker("setDate", '-31d');
    $("#txtPtesigndate").datepicker({
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
    $("#txtPtesigndate").datepicker("setDate", '-31d');
    $("#txtIesigndate").datepicker({
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
    $("#txtIesigndate").datepicker("setDate", '-31d');
    $("#txtSqMsigndate").datepicker({
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
    $("#txtSqMsigndate").datepicker("setDate", '-31d');
    $("#txtPqesigndate").datepicker({
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
    $("#txtPqesigndate").datepicker("setDate", '-31d');
    $("#txtCqesigndate").datepicker({
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
    $("#txtCqesigndate").datepicker("setDate", '-31d');
    $("#txtRdsigndate").datepicker({
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
    $("#txtRdsigndate").datepicker("setDate", '-31d');
    $("#txtSalessigndate").datepicker({
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
    $("#txtSalessigndate").datepicker("setDate", '-31d');
    $("#txtQrasigndate").datepicker({
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
    $("#txtQrasigndate").datepicker("setDate", '-31d');
    $("#txtMdsigndate").datepicker({
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
    $("#txtMdsigndate").datepicker("setDate", '-31d');
    $("#PMConlineTime").datepicker({
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
    $("#PMConlineTime").datepicker("setDate", '-31d');
    var SID;
    var LitNo;
    jQuery("#btnShow").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {   //single select
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            SID = dataRow.SID;
            LitNo = dataRow.LitNo;
            $.ajax({
                url: __WebAppPathPrefix + '/SQMMRB/GetSQMMRBData',
                data: { "SID": dataRow.SID },
                type: "post",
                dataType: 'json',
                async: false, // if need page refresh, please remark this option
                success: function (data) {
                    $("#txtMRBNo").val(data[0].MRBNo);
                    $("#txtinitiator").val(data[0].Initiator);
                    $("#txtgovernor").val(data[0].Governor);
                    $("#txtVenderCode").val(data[0].VenderCode);
                    $("#txtSize").val(data[0].Size);
                    $("#txtLitNo").val(data[0].LitNo);
                    $("#txtbatch").val(data[0].Batch);
                    $("#txtchecknumber").val(data[0].CheckNumber);
                    $("#txtdefect").val(data[0].Defect);
                    $("#txtrejectratio").val(data[0].RejectRatio);
                    $("#txtinspector").val(data[0].Inspector);
                    $("#ddlPMsign").val(data[0].PMsign);
                    $("#ddlQRsign").val(data[0].ddlQRsign);
                    $("#txtBaddescription").val(data[0].Baddescription);
                    $("#txtmodel").val(data[0].model);
                    $("#txtsite").val(data[0].site);
                    $("#txtduty").val(data[0].duty);
                    $("#txtcompere").val(data[0].compere);
                    $("#txtPuropinion").val(data[0].Puropinion);
                    $("#ddlPURsign").val(data[0].PURsign);
                    $("#txtPmcopinion").val(data[0].Pmcopinion);
                    $("#ddlPMCsign").val(data[0].PMCsign);
                    $("#txtMfgopinion").val(data[0].Mfgopinion);
                    $("#ddlMFGsign").val(data[0].MFGsign);
                    $("#txtSmtengopinion").val(data[0].Smtengopinion);
                    $("#ddlSMTENGsign").val(data[0].SMTENGsign);
                    $("#txtPteopinion").val(data[0].Pteopinion);
                    $("#ddlPTEsign").val(data[0].PTEsign);
                    $("#txtIeopinion").val(data[0].Ieopinion);
                    $("#ddlIEsign").val(data[0].IEsign);
                    $("#txtSqmopinion").val(data[0].Sqmopinion);
                    $("#ddlSQMsign").val(data[0].SQMsign);
                    $("#txtPqeopinion").val(data[0].Pqeopinion);
                    $("#ddlPQEsign").val(data[0].PQEsign);
                    $("#txtCqeopinion").val(data[0].Cqeopinion);
                    $("#ddlCQEsign").val(data[0].CQEsign);
                    $("#txtRdopinion").val(data[0].Rdopinion);
                    $("#ddlRdsign").val(data[0].Rdsign);
                    $("#txtSalesopinion").val(data[0].Salesopinion);
                    $("#ddlSALESsign").val(data[0].SALESsign);
                    $("#txtQraopinion").val(data[0].Qraopinion);
                    $("#ddlQRAsign").val(data[0].QRAsign);
                    $("#txtMdopinion").val(data[0].Mdopinion);
                    $("#ddlMDsign").val(data[0].MDsign);
                    $("#txtPMCworkorder").val(data[0].PMCworkorder);
                    

                    $("#txtcreatetime").val(new Date(data[0].CreateTime).yyyy_mm_dd);
                    $("#txtPMsigndate").val(new Date(data[0].PMsigndate).yyyy_mm_dd);
                    $("#txtQrsigndate").val(new Date(data[0].Qrsigndate).yyyy_mm_dd);
                    $("#txtpurchasetime").val(new Date(data[0].Purchasetime).yyyy_mm_dd);
                    $("#txtPursigndate").val(new Date(data[0].Pursigndate).yyyy_mm_dd);
                    $("#txtPmcsigndate").val(new Date(data[0].Pmcsigndate).yyyy_mm_dd);
                    $("#txtMfgsigndate").val(new Date(data[0].Mfgsigndate).yyyy_mm_dd);
                    $("#txtSmtengsigndate").val(new Date(data[0].Smtengsigndate).yyyy_mm_dd);
                    $("#txtPtesigndate").val(new Date(data[0].Ptesigndate).yyyy_mm_dd);
                    $("#txtIesigndate").val(new Date(data[0].Iesigndate).yyyy_mm_dd);
                    $("#txtSqMsigndate").val(new Date(data[0].SqMsigndate).yyyy_mm_dd);
                    $("#txtPqesigndate").val(new Date(data[0].Pqesigndate).yyyy_mm_dd);
                    $("#txtCqesigndate").val(new Date(data[0].Cqesigndate).yyyy_mm_dd);
                    $("#txtRdsigndate").val(new Date(data[0].Rdsigndate).yyyy_mm_dd);
                    $("#txtSalessigndate").val(new Date(data[0].Salessigndate).yyyy_mm_dd);
                    $("#txtQrasigndate").val(new Date(data[0].Qrasigndate).yyyy_mm_dd);
                    $("#txtMdsigndate").val(new Date(data[0].Mdsigndate).yyyy_mm_dd);
                    $("#txtPMConlineTime").val(new Date(data[0].PMConlineTime).yyyy_mm_dd);
                    setRadio("MRBType",escape($.trim(data[0].MRBType)));
                    setRadio("venderType", escape($.trim(data[0].VenderType)));
                    setRadio("Baddescriptiontype", escape($.trim(data[0].BadDescriptionType)));
                    setRadio("meettype", escape($.trim(data[0].meettype)));
                  
                    $('#div1').hide();
                    $('#table1').show();
                    $('#table2').show();
                    $('#table3').show();
                    $('#back').show();
                    $('#btnSave').show();
                    $('#table1 *').attr("disabled", "disabled");
                    $('#table2 *').attr("disabled", "disabled");
                    $('#txtnote').removeAttr('disabled');
                    $("#txtnote").val(data[0].NOTE);

                },
                error: function (xhr, textStatus, thrownError) {
                    $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                },
                complete: function (jqXHR, textStatus) {

                }
            });
       
        } else { alert("Please select a row data to show."); }

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
  

    jQuery("#btnBack").click(function () {
        $('#div1').show();
        $('#table1').hide();
        $('#table2').hide();
        $('#table3').hide();
        $('#back').hide();
        $("#btnSearch").click();
    });
    jQuery("#btnSave").click(function () {
        $(this).removeClass('ui-state-focus');
        var DoSuccessfully = false;
        $.ajax({
            url: __WebAppPathPrefix + "/SQMMRB/EditSQMMRB",
            data: {
                "SID": SID, 
                "NOTE": escape($.trim($('#txtnote').val())),
                "FileNO": escape($.trim($("#tbMain1").attr("MRBFGUID"))),
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
            $('#div1').show();
            $('#table1').hide();
            $('#table2').hide();
            $('#table3').hide();
            $('#back').hide();
            $("#btnSearch").click();
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