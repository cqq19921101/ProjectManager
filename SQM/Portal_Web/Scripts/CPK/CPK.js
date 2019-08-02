$(function () {
    $("#btnAgree").button({
        label: "確認",
        icons: { primary: "ui-icon-pencil" }
    });

    $("#btnAgree").click(function () {
        //alert($('#hTaskID').val());
        //$(this).removeClass('ui-state-focus');
        //var DoSuccessfully = false;
        $("#btnAgree").attr('disabled', 'disabled');
        $.ajax({
            url: __WebAppPathPrefix + "/CPK/UpdateReport",
            data: {
               
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
        
        //if (DoSuccessfully) {

        //}
    });

   
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });
   
    
});