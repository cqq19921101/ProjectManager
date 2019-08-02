$(function () {
    $("#dialogGroupName").dialog({
        autoOpen: false,
        height: 150,
        width: 250,
        resizable: false,
        modal: true,
        buttons: {
            Submit: function () {
                var Action = $(this).prop('Action');
                var GROUP_TYPE = $(this).prop('GROUP_TYPE');
                var GROUP_NAME = $.trim($(this).find('#name').val());

                switch (Action) {
                    case 'Add':
                        var PARENT_GROUP_ID = $(this).prop('PARENT_GROUP_ID');
                        
                        $.ajax({
                            url: __WebAppPathPrefix + '/VMIConfigration/AddGroup',
                            data: {
                                PARENT_GROUP_ID: escape($.trim(PARENT_GROUP_ID)),
                                GROUP_NAME: escape($.trim(GROUP_NAME)),
                                GROUP_TYPE: escape($.trim(GROUP_TYPE))
                            },
                            type: "post",
                            dataType: 'text',
                            async: false, // if need page refresh, please remark this option
                            success: function (data) {
                                if (data.indexOf('Success') == -1) {
                                    alert(data);
                                }
                                
                                $('#gridDeptList').jqGrid('resetSelection');
                                $('#gridDeptList').trigger('reloadGrid');

                                $("#dialogGroupName").dialog('close');
                            },
                            error: function (xhr, textStatus, thrownError) {
                                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                            },
                            complete: function (jqXHR, textStatus) {
                            }
                        });
                        break;
                    case 'Edit':
                        var PARENT_GROUP_ID = $(this).prop('PARENT_GROUP_ID');
                        var GROUP_ID = $(this).prop('GROUP_ID');

                        $.ajax({
                            url: __WebAppPathPrefix + '/VMIConfigration/EditGroup',
                            data: {
                                PARENT_GROUP_ID: escape($.trim(PARENT_GROUP_ID)),
                                GROUP_ID: escape($.trim(GROUP_ID)),
                                GROUP_NAME: escape($.trim(GROUP_NAME)),
                                GROUP_TYPE: escape($.trim(GROUP_TYPE))
                            },
                            type: "post",
                            dataType: 'text',
                            async: false, // if need page refresh, please remark this option
                            success: function (data) {
                                if (data.indexOf('Success') == -1) {
                                    alert(data);
                                }

                                $('#gridDeptList').jqGrid('resetSelection');
                                $('#gridDeptList').trigger('reloadGrid');

                                $("#dialogGroupName").dialog('close');
                            },
                            error: function (xhr, textStatus, thrownError) {
                                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                            },
                            complete: function (jqXHR, textStatus) {
                            }
                        });
                        break;
                }
            },
            Close: function () {
                $(this).dialog('close');
            }
        },
        close: function () {
            $(this).find('#name').val('');
        }
    });
})