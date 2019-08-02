var sLocaleOption = "";
var sLocaleString = "";

$(function () {
    sLocaleOption = eval($('#hidLocaleOption').val());
    sLocaleString = eval($('#hidLocaleString').val());
    for (var iCnt = 0; iCnt < sLocaleOption.length; iCnt++) {
        $('#selLocale').append($("<option />").attr("value", sLocaleOption[iCnt][0].toLowerCase()).text(sLocaleOption[iCnt][1]));
    }
    $('#selLocale').val($("#hidUserSelectedLocale").val().toLowerCase());
    RefreshLocaleText();

    //$('#selLocale').change(function () {
    $('#selLocale').quickChange(function () {
        $("#hidUserSelectedLocale").val($('#selLocale').val().toLowerCase());
        RefreshLocaleText();
    });

    $('#txtUserID').setfocus();

    $('#img-buffer').load(function () {
        GenQRCode();
    });

    $('#lblScanQRCode').click(function () {
        $('#scanQRCode').show();
        $('#frmMain').hide();
        $('#divMsg').html('');
        GenQRCode();
    });

    $('#lblNormalLogin').click(function () {
        $('#scanQRCode').hide();
        $('#frmMain').show();
    });
});

function GenQRCode() {
    $('#s_tips').html('');
    if ($('#container canvas').length == 0) {
        $('#container').show();
        var token = GetToken();
        if (token != "") {
            var UUID = inituuid(token);
            if (UUID != "") {
                $('#container').empty().qrcode({
                    render: 'canvas', // render method: 'canvas', 'image' or 'div'
                    minVersion: 1, // version range somewhere in 1 .. 40
                    maxVersion: 40,
                    ecLevel: 'H', // error correction level: 'L', 'M', 'Q' or 'H'
                    left: 0, // offset in pixel if drawn onto existing canvas
                    top: 0,
                    size: 150, // size in pixel
                    fill: "#333333", // code color or image element
                    background: "#ffffff", // background color or image element, null for transparent background
                    text: "https://login.liteon.com/uuid=" + UUID, // content
                    radius: 0, // corner radius relative to module width: 0.0 .. 0.5
                    quiet: 3, // quiet zone in modules
                    mode: 4, // modes 0: normal 1: label strip 2: label box 3: image strip 4: image box
                    mSize: 0.15,
                    mPosX: 0.5,
                    mPosY: 0.5,
                    label: 'no label',
                    fontname: "Ubuntu",
                    fontcolor: '#000',
                    image: $('#img-buffer')[0]
                });
                pushuuid(token, UUID);
            }
        }
    }
}

function GetToken() {
    var token = '';
    $.ajax({
        url: __WebAppPathPrefix + "/Account/GetToken",
        async: false,
        type: "post",
        dataType: 'text',
        success: function (data) {            
            token = data;
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        }
    });
    return token;
}

function inituuid(token) {
    var uuid = '';
    $.ajax({
        url: __WebAppPathPrefix + "/Account/InitUUID",
        data: {
            token: token
        },
        async: false,
        type: "post",
        success: function (data) {
            var d = $.parseJSON(data);
            if (d.ResponseState == 1) {
                uuid = d.ResponseData;
            }
        },
        error: function (a, b, c) {
            console.info(a)
            console.info(b)
            console.info(c)
        }
    })
    return uuid;
}

function pushuuid(token, UUID) {
    $.ajax({
        url: __WebAppPathPrefix + "/Account/PushUUID",
        data: {
            token: token,
            UUID: UUID
        },
        async: false,
        type: "post",
        success: function (data) {
            var d = $.parseJSON(data);
            switch (parseInt(d.ResponseState)) {
                case 5:
                    //等待中且60秒内,3秒再次发送
                    setTimeout(function () {
                        pushuuid(token, UUID);
                    }, 3000)
                    break;
                case 6:
                    //确认中且60秒内 3秒再次发送
                    setTimeout(function () {
                        pushuuid(token, UUID);
                    }, 3000)
                    break;
                case 3:
                    //取消登录,不在发送
                    $('#container').slideUp(1000);
                    $('#container').empty();
                    $('#s_tips').html("已取消\n，請刷新頁面");
                    $('#s_tips').on("click", function () {
                        $('#s_tips').html('');
                        GenQRCode();
                    });
                    document.getElementById('s_tips').style.cursor = "pointer"
                    break;
                case 1:
                    //确认登录，不在发送
                    $('#aaa').html('Loading...');
                    
                    if (d.ResponseState == 1) {
                        $('#hidRunAsMemberGUID').attr('value', d.ResponseObject.LoginID);
                        $('#scanQRCode').submit();
                    }
                    
                    $('#s_tips').slideUp(1000);
                    $('#container').slideUp(1000);
                    $('#container').empty();
                    break;
                case 2:
                    //二维码失效
                    $('#container').empty();
                    $('#s_tips').html("二維碼已失效，請點擊此處重新獲取二維碼。");
                    $('#s_tips').on("click", function () {
                        $('#s_tips').html('');
                        GenQRCode();
                    });
                    document.getElementById('s_tips').style.cursor = "pointer"
                    break;
            }
        }
    })
}

$(function () {
    jQuery("#btnSubmit").click(function () {
        $("#frmMain").attr("action", "");
        $("#frmMain").submit();
    });
});

function RefreshLocaleText() {
    var iLocalID = 0;
    var USL = $("#hidUserSelectedLocale").val();
    if (USL.toLowerCase() == "zh-tw")
        iLocalID = 1;
    else if (USL.toLowerCase() == "zh-cn")
        iLocalID = 2;

    $("#lblLocale").html(sLocaleString[iLocalID][0]);
    $("#lblUserID").html(sLocaleString[iLocalID][1]);
    $("#lblPassword").html(sLocaleString[iLocalID][2]);
    $("#btnSubmit").val(sLocaleString[iLocalID][3]);
    document.title = sLocaleString[iLocalID][5]
    $("#lblWelcome").html(sLocaleString[iLocalID][4]);
    //$("#lblWelcome").html("");
    $("#lblRememberMe").html(sLocaleString[iLocalID][6]);
    $("#aForgotPWD").text(sLocaleString[iLocalID][7]);
    $('#lblScanQRCode').text(sLocaleString[iLocalID][8]);
    $('#lblNormalLogin').text(sLocaleString[iLocalID][9]);
}

function ForgotPWD()
{
    //$("#frmMain").attr("action", "/Account/ForgotPassword");
    $("#frmMain").attr("action", __WebAppPathPrefix + "/Account/ForgotPassword");
    $("#frmMain").submit();
}

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

$(function () {
    //當intput得到focus時自動全選文字
    $("input").focus(function () {
        try{ this.select(); } catch (e) { }
    });

    //當user按下Enter鍵時同時登入
    $("form input").keypress(function (e) {
        if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
            //$('button[type=submit] .default').click();
            $("#btnSubmit").click();
            return false;
        } else {
            return true;
        }
    });
});