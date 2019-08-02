function VDSToolBar() {
    // Init Toolbar
    var diaToDVDSManage_tbTopToolBar = $('#dialog_VMIProcess_ToDoVDSManage_tbTopToolBar');
    var tabsVDS = $('#ToDoVDSManageTabs');
    var diaToDVDSManage = $('#dialog_VMIProcess_ToDoVDSManage');
    var PLANT = $.trim(diaToDVDSManage.prop('PLANT'));
    var VENDOR = $.trim(diaToDVDSManage.prop('VENDOR'));
    var BUYER = $.trim(diaToDVDSManage.prop('BUYER'));

    diaToDVDSManage_tbTopToolBar.show();

    $('#btnVDSCommit').click(function () {
        VDSCommit(tabsVDS, PLANT, VENDOR, BUYER, false);
        $(this).blur();
    });

    $('#btnVDSCommitAll').click(function () {
        VDSCommit(tabsVDS, PLANT, VENDOR, BUYER, true);
        $(this).blur();
    });

    $('#btnExportAll').click(function () {
        VDSExportVDS('ExportAll', PLANT, VENDOR, BUYER);
        $(this).blur();
    });

    $('#btnExportVDS').click(function () {
        VDSExportVDS('ExportVDS', PLANT, VENDOR, BUYER);
        $(this).blur();
    });
}

function VDSCommit(tabsVDS, PLANT, VENDOR, BUYER, isCommitAll) {
    var multi_VDS_INFO = new Array();

    if (!isCommitAll) {
        var activeTabIndex = tabsVDS.tabs('option', 'active');
        if (activeTabIndex > 3) {
            alert('Please choice one type to commit!');
        }
        else {
            var tabPanel = tabsVDS.find('[role="tabpanel"]:eq(' + activeTabIndex + ')'),
                VDS_NUM = $.trim(tabPanel.find('td#tdVDS_NUM').text()),
                VRSIO = $.trim(tabPanel.find('td#tdVRSIO').text()),
                TMType = tabsVDS.find('[role="presentation"]:eq(' + activeTabIndex + ')').text(),
                TBNUM = $.trim(tabPanel.find('#tdTBNUM').text()),
                VDS_INFO = new Object();

            VDS_INFO.PLANT = PLANT;
            VDS_INFO.VENDOR = VENDOR;
            VDS_INFO.BUYER = BUYER;
            VDS_INFO.VDS_NUM = VDS_NUM;
            VDS_INFO.VRSIO = VRSIO;
            VDS_INFO.TMType = TMType;
            VDS_INFO.TBNUM = TBNUM;

            multi_VDS_INFO.push(VDS_INFO);
        }
    }
    else {
        for (var i = 0; i <= 3; i++) {
            var tabPanel = tabsVDS.find('[role="tabpanel"]:eq(' + i + ')'),
                VDS_NUM = $.trim(tabPanel.find('td#tdVDS_NUM').text()),
                VRSIO = $.trim(tabPanel.find('td#tdVRSIO').text()),
                TMType = tabsVDS.find('[role="presentation"]:eq(' + i + ')').text(),
                TBNUM = $.trim(tabPanel.find('#tdTBNUM').text()),
                VDS_INFO = new Object(),
                isTabDisplay = tabsVDS.find('[role="presentation"]:eq(' + i + ')').parent().css('display');
            // Only displayed Type can commit
            if (isTabDisplay != 'none') {
                VDS_INFO.PLANT = PLANT;
                VDS_INFO.VENDOR = VENDOR;
                VDS_INFO.BUYER = BUYER;
                VDS_INFO.VDS_NUM = VDS_NUM;
                VDS_INFO.VRSIO = VRSIO;
                VDS_INFO.TMType = TMType;
                VDS_INFO.TBNUM = TBNUM;

                multi_VDS_INFO.push(VDS_INFO);
            }
        }
    }

    $.ajax({
        url: __WebAppPathPrefix + '/VMIProcess/CommitVDS',
        data: {
            VDS_INFO: JSON.stringify(multi_VDS_INFO),
            isCommitAll: isCommitAll
        },
        type: "post",
        dataType: 'json',
        async: false,
        success: function (data) {
            var isCommitable = true, summary = '';

            for (var idx in data) {
                isCommitable = data[idx].Result.editable && isCommitable;
                if (!data[idx].Result.editable) {
                    summary += '\nVDS_NUM: ' + data[idx].VDS_NUM + '; VRSIO: ' + data[idx].VRSIO + '; Reason: ' + data[idx].Result.message + '\n';
                }
            }

            if (!isCommitable) {
                alert('Commit failure:\n' + summary);
            }
            else {
                if (!isCommitAll) {
                    $.ajax({
                        url: __WebAppPathPrefix + '/VMIProcess/GetToDoVDSDetail',
                        data: {
                            VDS_NUM: escape(VDS_NUM),
                            VRSIO: escape(VRSIO),
                            TBNUM: escape(TBNUM)
                        },
                        type: "post",
                        dataType: 'json',
                        async: false, // if need page refresh, please remark this option
                        success: function (data) {
                            if (data != null) {
                                // Reloading grid.....
                                var avtivedGirdID = tabPanel.find('table[id^="list"]').first().attr('id');
                                var avtivedGrid = $('#' + avtivedGirdID);

                                avtivedGrid.jqGrid('setGridParam', {
                                    postData: {
                                        VDS_NUM: escape(VDS_NUM),
                                        VRSIO: escape(VRSIO)
                                    }
                                });
                                avtivedGrid.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
                                avtivedGrid.jqGrid('destroyFrozenColumns').jqGrid('setFrozenColumns');
                            }
                            else {
                                var ToDoVDSManageTabs = $('#ToDoVDSManageTabs');
                                activeIdx = ToDoVDSManageTabs.tabs('option', 'active');
                                var arrDisabledTabIdx = ToDoVDSManageTabs.tabs('option', 'disabled');
                                arrDisabledTabIdx.push(activeIdx);
                                ToDoVDSManageTabs.tabs('option', 'disabled', arrDisabledTabIdx);
                                $('li.ui-state-disabled[role=tab]').hide();

                                // If tabs only show Open PO and Inventory then close the dialog
                                if (ToDoVDSManageTabs.find('ul li').not('.ui-state-disabled').length == 2) {
                                    $('#dialog_VMIProcess_ToDoVDSManage').dialog('close');
                                    ReloadToDoVDSgridDataList();
                                    alert('This To Do VDS has been committed, the dialog will close automatically.');
                                }
                                else {
                                    ToDoVDSManageTabs.tabs('option', 'active', 4); // switch to Inventory tab
                                }
                            }
                        },
                        error: function (xhr, textStatus, thrownError) {
                            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                        },
                        complete: function (jqXHR, textStatus) {
                        }
                    });
                }
                else {
                    $('#dialog_VMIProcess_ToDoVDSManage').dialog('close');
                    ReloadToDoVDSgridDataList();
                    alert('This To Do VDS has been committed, the dialog will close automatically.');
                }
            }
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
}

function VDSExportVDS(Status, PLANT, VENDOR, BUYER) {
    $.ajax({
        url: __WebAppPathPrefix + '/VMIProcess/ToDoVDSExportVDS',
        data: {
            Plant: PLANT,
            VendorCode: VENDOR,
            BuyerCode: BUYER,
            Status: Status
        },
        type: "post",
        dataType: 'json',
        //async: false,
        beforeSend: function () {
            $("#dialogDownloadSplash").dialog({
                title: 'Download Notify',
                width: 'auto',
                height: 'auto',
                open: function (event, ui) {
                    $(this).parent().find('.ui-dialog-titlebar-close').hide();
                    $(this).parent().find('.ui-dialog-buttonpane').hide();
                    $("#lbDiaDownloadMsg").html('Downloading...</br></br>Please wait for the operation a moment...');
                }
            }).dialog("open");
        },
        success: function (data) {
            if (data.Result) {
                if (data.FileKey != "") {
                    $("#dialogDownloadSplash_FileKey").val(data.FileKey);
                    $("#dialogDownloadSplash_FileName").val(data.FileName);

                    setTimeout(function () {
                        $("#dialogDownloadSplash_Form").attr('action', __WebAppPathPrefix + '/VMIProcess/RetrieveFileByFileKey').submit();
                        $("#dialogDownloadSplash").dialog("close");
                    }, 10);
                }
            }
            else
                alert("Export failure. Please contact administrator manager.");
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        }
    });
}