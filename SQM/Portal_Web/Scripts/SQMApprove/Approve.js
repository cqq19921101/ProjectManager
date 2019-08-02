$(function () {
    $("#btnAgree").button({
        label: "Approve",
        icons: { primary: "ui-icon-pencil" }
    });

    $("#btnAgree").click(function () {
        //alert($('#hTaskID').val());
        //$(this).removeClass('ui-state-focus');
        //var DoSuccessfully = false;
        $("#btnAgree").attr('disabled', 'disabled');
        $.ajax({
            url: __WebAppPathPrefix + "/SQM/UpdateTaskStatus",
            data: {
                "Status": 'Approve',
                "TaskID": $("#hTaskID").val(),
                "Remark": $("#txtRemark").val()
            },
            type: "post",
            dataType: 'text',
            async: false,
            success: function (data) {
                if (data == "") {
                    DoSuccessfully = true;
                    alert("Operate successfully.");
                    $("[data='oper']").hide();
                    $("[data='result']").html("Approve");
                    $("[data='result']").show();
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
                $("#btnAgree").removeAttr('disabled');
            }
        });
        
        //if (DoSuccessfully) {

        //}
    });

    $('#btnReject').button({
        label: 'Reject',
        icons: { primary: 'ui-icon-arrowthickstop-1-s' }
    });
    $("#btnReject").click(function () {
        $("#btnReject").attr('disabled', 'disabled');
        $.ajax({
            url: __WebAppPathPrefix + "/SQM/UpdateTaskStatus",
            data: {
                "Status": 'Reject',
                "TaskID": $("#hTaskID").val(),
                "Remark": $("#txtRemark").val()
            },
            type: "post",
            dataType: 'text',
            async: false,
            success: function (data) {
                if (data == "") {
                    DoSuccessfully = true;
                    alert("Operate successfully.");
                    $("[data='oper']").hide();
                    $("[data='result']").html("Reject");
                    $("[data='result']").show();
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
                $("#btnReject").removeAttr('disabled');
            }
        });
    });


});