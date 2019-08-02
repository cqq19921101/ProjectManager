$(function () {
    //Toolbar Buttons
    $("#txtBeginTime").datepicker({
        changeMonth: true,
        dateFormat: 'yy/mm/dd',
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            } catch (err) {
                $(this).datepicker("setDate", '');
            }
        }
    });

    $("#txtEndTime").datepicker({
        changeMonth: true,
        dateFormat: 'yy/mm/dd',
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            } catch (err) {
                $(this).datepicker("setDate", '');
            }
        }
    });

    $.ajax({
        url: __WebAppPathPrefix + '/SQMReliability/GetLitNoList',
        type: "post",
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            var options = '<option value="">-- 請選擇 --</option>';
            for (var idx in data) {
                options += '<option value=' + data[idx].LitNo + '> ' + data[idx].LitNo + '</option>';
            }
            $('#ddlplantCode1').append(options);
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
    $('#ddlplantCode').change(function () {
        $.ajax({
            url: __WebAppPathPrefix + '/SQMReliability/GetLitNoDataList',
            data: { "plantCode": $("#ddlplantCode").val()},
            type: "post",
            dataType: 'json',
            async: false, // if need page refresh, please remark this option
            success: function (data) {
                var options = '<option value="">-- 請選擇 --</option>';
                for (var idx in data) {
                    options += '<option value=' + data[idx].spc_item + '> ' + data[idx].spc_item + '</option>';
                }
                $('#ddlDesignator').html(options);
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
            }
        });
       
    });
    $('#ddlDesignator').change(function () {
        var colname;
        var colmodel;

   
        $.ajax({
            url: __WebAppPathPrefix + '/SQMReliability/getModel',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
            data: { begin: $('#txtBeginTime').val(), end: $('#txtEndTime').val() ,Designator:$('#ddlDesignator').val()},
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
        try {
            $("#gridDataList").jqGrid("GridUnload");
        } catch (e) {

        }
        setTimeout(function () {

        }, 200);
        var gridDataList = $("#gridDataList");

        gridDataList.jqGrid({
            url: __WebAppPathPrefix + '/SQMReliability/LoadBARJSonWithFilter',
            postData: { begin: $('#txtBeginTime').val(), end: $('#txtEndTime').val(), Designator: $('#ddlDesignator').val() },
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
            rowNum: 20,
            viewrecords: true,
            loadonce: true,
            mtype: 'POST',
            pager: '#gridListPager',
            //sort by reload
            gridComplete: function () {
               
            },
            loadComplete: function () {
                var $this = $(this);
                if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                    if ($this.jqGrid('getGridParam', 'sortname') !== '')
                        setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
                var colNames = $("#gridDataList").jqGrid('getGridParam', 'colNames');
                if (colNames.length>1) {
                    var AVGdata = $("#gridDataList").jqGrid('getRowData', 1);
                    var USLdata = $("#gridDataList").jqGrid('getRowData', 6);
                    var UCLdata = $("#gridDataList").jqGrid('getRowData', 7);
                    var LCLdata = $("#gridDataList").jqGrid('getRowData', 8);
                    var LSLdata = $("#gridDataList").jqGrid('getRowData', 9);
                }
               
                
                var newColumnName = [];
                var AVG = [];
                var USL = [];
                var LSL = [];
                var UCL = [];
                var LCL = [];
                for (var i = 0; i < colNames.length; i++) {
                    if (colNames[i] != "DTIME") {
                        newColumnName.push(colNames[i]);
                        AVG.push(AVGdata[colNames[i]]);
                        USL.push(USLdata[colNames[i]]);
                        LSL.push(LSLdata[colNames[i]]);
                        UCL.push(UCLdata[colNames[i]]);
                        LCL.push(LCLdata[colNames[i]]);
                        
                    }

                }
                var config = {
                    type: 'line',
                    data: {
                        labels: newColumnName,
                        datasets: [{
                            label: 'AVG',
                            backgroundColor: 'rgb(255, 99, 132)',
                            borderColor: 'rgb(255, 99, 132)',
                            data: AVG,
                            fill: false,
                        },
                        {
                            label: 'USL',
                            backgroundColor: 'rgb(255, 159, 64)',
                            borderColor: 'rgb(255, 159, 64)',
                            data: USL,
                            fill: false,
                        },
                        {
                            label: 'LSL',
                            backgroundColor: 'rgb(255, 159, 64)',
                            borderColor: 'rgb(255, 159, 64)',
                            data: LSL,
                            fill: false,
                        },
                     
                        {
                            label: 'UCL',
                            backgroundColor: 'rgb(54, 162, 235)',
                            borderColor: 'rgb(54, 162, 235)',
                            data: UCL,
                            fill: false,
                        },
                        {
                            label: 'LCL',
                            backgroundColor: 'rgb(54, 162, 235)',
                            borderColor: 'rgb(54, 162, 235)',
                            data: LCL,
                            fill: false,
                        }]
                    },
                    options: {
                        responsive: true,
                        title: {
                            display: true,
                            text: 'X-Bar'
                        },
                        tooltips: {
                            enable:false
                            //mode: 'index',
                            //intersect: false,
                        },
                        hover: {
                            mode: 'nearest',
                            intersect: true
                        },
                        scales: {
                            xAxes: [{
                                display: true,
                                scaleLabel: {
                                    display: true,
                                    labelString: 'Date'
                                }
                            }],
                            yAxes: [{
                                display: true,
                                scaleLabel: {
                                    display: true,
                                    labelString: 'Value'
                                }
                            }]
                        }
                    }
                };

                var ctx = document.getElementById('canvas').getContext('2d');
               
                window.myLine = new Chart(ctx, config);
             
           

            }
        });
        gridDataList.jqGrid('navGrid', '#gridListPager', { edit: false, add: false, del: false, search: false, refresh: false });
      
        
    });
  

      
    




 


  
    
      
    
    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });

    $('#tbMain1').show();
    $('#dialogData').show();
});
    