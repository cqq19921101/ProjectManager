$(function () {
    jQuery("#btnRunAs").click(function () {
        //var stxtRunAs = $("#txtRunAs").val().trim().toLowerCase();
        var stxtRunAs = $.trim($("#txtRunAs").val().toLowerCase());
        if (stxtRunAs == "") {
            alert("Please input ACCOUNT ID.");
        } else {
            if (confirm("Confirm to Run As selected member (" + stxtRunAs + ")?")) {
                $("#hidRunAsAccount").val(escape(stxtRunAs));
                $("#_RunAsForm").submit();
            }
        }
    });
});