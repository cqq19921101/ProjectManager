function CPKDataStatistic() {
    var gridDataListCPKData = $("#gridDataListCPKData");

    var ucl = parseFloat($("#dialogDataCPKSub").attr('UpperControlLimit'));
    var nominal = parseFloat($("#dialogDataCPKSub").attr('Nominal'));
    var lcl = parseFloat($("#dialogDataCPKSub").attr('LowerControlLimit'));
    var CTQNum = parseFloat($("#dialogDataCPKSub").attr('CTQNum'));

    $.ajax({
        url: __WebAppPathPrefix + '/SQMReliability/StatisticCPKData',
        data: {
            reportID: $("#dialogDataCPKSub").attr('reportID')
            , Designator: $("#dialogDataCPKSub").attr('Designator')
        },
        type: "post",
        dataType: 'json',
        async: false,
        success: function (data) {
            if (data.length > 0) {
                var count = parseFloat($.trim(data[0].countCTQ));
                var maxData = parseFloat($.trim(data[0].maxCTQ));
                var minData = parseFloat($.trim(data[0].minCTQ));
                var sumData = parseFloat($.trim(data[0].sumCTQ));
                var average = parseFloat($.trim(data[0].avgCTQ));
                var stdevData = parseFloat($.trim(data[0].stdevCTQ));

                if (count > 0) {
                    var CA = ((ucl - nominal) == 0) ? "數據有誤，請檢查" : parseFloat((average - nominal) / (ucl - nominal));
                    var CP = parseFloat((ucl - lcl) / 6 / stdevData);
                    var CPK = (typeof (CA) == "string") ? "數據有誤，請檢查" : parseFloat((1 - Math.abs(CA)) * CP);

                    $("#spMAX").html(maxData);
                    $("#spMIN").html(minData);
                    $("#spAVERAGE").html(average);
                    $("#spSTDEV").html(stdevData);
                    $("#spCA").html(CA);
                    $("#spCP").html(CP);
                    $("#spCPK").html(CPK);
                    $("#spCTQNum").html(CTQNum - count);
                } else {
                    $("#spMAX").html('');
                    $("#spMIN").html('');
                    $("#spAVERAGE").html('');
                    $("#spSTDEV").html('');
                    $("#spCA").html('');
                    $("#spCP").html('');
                    $("#spCPK").html('');
                    $("#spCTQNum").html('');
                }
            } else {
                $("#spMAX").html('');
                $("#spMIN").html('');
                $("#spAVERAGE").html('');
                $("#spSTDEV").html('');
                $("#spCA").html('');
                $("#spCP").html('');
                $("#spCPK").html('');
                $("#spCTQNum").html(CTQNum);
            }
            
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });

}