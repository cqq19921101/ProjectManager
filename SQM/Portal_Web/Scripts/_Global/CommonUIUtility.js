//取消focus時的虛線框
$(function () {
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });
    $(':checkbox').focus(function () { this.blur(); });
});


//解決某些環境(某些IE 8)沒有JSON物件可用的問題(無法使用JSON.parse())
function JSonParse(data) {
    var obj;
    if (typeof (JSON) == 'undefined')
        obj = eval("(" + data + ")");
    else
        obj = JSON.parse(data);
    return obj;
}

//解決IE8 Textarea maxlength(default max is 20000)
function getCaret(el) {
    if (el.selectionStart) {
        return el.selectionStart;
    } else if (document.selection) {
        el.focus();
        var r = document.selection.createRange();

        if (r == null) {
            return 0;
        }

        var re = el.createTextRange(),
        rc = re.duplicate();
        re.moveToBookmark(r.getBookmark());
        rc.setEndPoint('EndToStart', re);
        return rc.text.length;
    }
    return 0;
}

function limitPasteData(obj, limit) {
    // Do it with Internet Explorer
    if (window.clipboardData) {
        if (document.selection && document.selection.createRange) {
            obj.focus();
            var modifiedTxtLen;
            // If text has been selected
            if (document.selection.createRange().text.length > 0) {
                modifiedTxtLen = obj.value.length - document.selection.createRange().text.length + window.clipboardData.getData("Text").length;

                if (modifiedTxtLen > limit) {
                    var neededLen = limit - (obj.value.length - document.selection.createRange().text.length);
                    document.selection.createRange().text = window.clipboardData.getData("Text").substring(0, neededLen);
                    return false;
                }
            } else {
                modifiedTxtLen = obj.value.length + window.clipboardData.getData("Text").length;
                if (modifiedTxtLen > limit) {
                    var neededLen = limit - obj.value.length;
                    var neededTxt = window.clipboardData.getData("Text").substring(0, neededLen);
                    var mofifiedText = obj.value.substring(0, getCaret(obj)) + neededTxt + obj.value.substring(getCaret(obj), obj.value.length);
                    obj.value = mofifiedText;
                    return false;
                }
            }
            // If not exceed limit just paste it
            return true;
        }
    }
}

function limitTypingData(obj, limit) {
    // Other browsers may have support for maxlength attribute
    if (navigator.appName == 'Microsoft Internet Explorer') {
        if (obj.value.length > (limit - 1)) {

            if (document.selection && document.selection.createRange) {
                obj.focus();
                // If only text has been selected, allow to type once
                if (document.selection.createRange().text.length > 0) {
                    return true;
                }
            }
            return false;
        }
    }
}

//設定Mega Menu寬度(超過minWidth則width=100%)
$(function () {
    ResetMenuDivWidth();

    $(window).resize(function () {
        ResetMenuDivWidth();
    });

    function ResetMenuDivWidth() {
        var currentWindowWidth = $(window).width();
        var currentPortalBannerWidth = $('.PortalBanner_OutterTable').width();
        $('#menudiv').css('width', (currentWindowWidth >= currentPortalBannerWidth) ? '100%' : currentPortalBannerWidth);
        $('.Portal_FunctionPath_Table').css('width', (currentWindowWidth >= currentPortalBannerWidth) ? '100%' : currentPortalBannerWidth);        
    }
});

$(function () {
    //init data list version 1
    //$.CommonUIUtility_InitDataList = function (objSelect, dataUrl) {
    //    objSelect.find('option').remove().end();
    //    $.ajax({
    //        //url: __WebAppPathPrefix + "/CQMRoleUser/LoadTeamJSonForSelect",
    //        url: dataUrl,
    //        type: "get",
    //        dataType: 'json',
    //        async: false,
    //        success: function (data) {
    //            if (data.length > 0) {
    //                for (var iCnt = 0; iCnt < data.length; iCnt++)
    //                    objSelect.append($("<option></option>").attr("value", data[iCnt].DataValue).text(data[iCnt].DataTitle));
    //            }
    //        },
    //        error: function (xhr, ajaxOptions, thrownError) {
    //            alert(xhr.status + thrownError);
    //        }
    //    });
    //}

    //init data list version 2
    $.CommonUIUtility_InitDataList = function (objSelect, dataUrl, withBlankOption, refData1) {
        objSelect.find('option').remove().end();
        $.ajax({
            url: dataUrl,
            data: {
                "refData1": refData1
            },

            type: "get",
            dataType: 'json',
            async: false,
            success: function (data) {
                if (withBlankOption) {
                    //if (data.length > 1)
                    objSelect.append($("<option></option>").attr("value", "").text(""));
                }
                if (data.length > 0) {
                    for (var iCnt = 0; iCnt < data.length; iCnt++)
                        objSelect.append($("<option></option>").attr("value", data[iCnt].DataValue).text(data[iCnt].DataTitle));
                }
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            }
        });
    }
});

$(function () {
    //For ajax() timeout的標準處理
    $.CommonUIUtility_AjaxErrorHandler = function AjaxErrorHandler(xhr, textStatus, thrownError, UrlForTimeOut) {
        if (xhr.status == "408")
            ShowTimeoutMessageAndRedirectToHome(UrlForTimeOut);
        else
            alert("Error " + xhr.status + ": " + thrownError);
    }

    $.CommonUIUtility_SessionTimeOut = function SessionTimeOut_Type1(UrlForTimeOut) {
        ShowTimeoutMessageAndRedirectToHome(UrlForTimeOut);
    }

    function ShowTimeoutMessageAndRedirectToHome(UrlForTimeOut) {
        //alert("Idle too long, session expired.\n\n (You will be automatic redirect to login page immediately.)");
        //alert("Please close window,and login from [iPOWER] again.");
        window.top.location.replace(UrlForTimeOut);
        window.location = UrlForTimeOut;
    }
});

(function ($) {
    //處理setfocus在某些環境不work的狀況
    jQuery.fn.setfocus = function () {
        return this.each(function () {
            var dom = this;
            setTimeout(function () {
                try { dom.focus(); } catch (e) { }
            }, 0);
        });
    };

    //處理select change事件在iphone上不work的狀況
    $.fn.quickChange = function (handler) {
        return this.each(function () {
            var self = this;
            self.qcindex = self.selectedIndex;
            var interval;
            function handleChange() {
                if (self.selectedIndex != self.qcindex) {
                    self.qcindex = self.selectedIndex;
                    handler.apply(self);
                }
            }
            $(self).focus(function () {
                interval = setInterval(handleChange, 100);
            }).blur(function () { window.clearInterval(interval); })
            .change(handleChange); //also wire the change event in case the interval technique isn't supported (chrome on android)
        });
    };
})(jQuery);

