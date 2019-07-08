/// <reference path="jquery-1.10.2.min.js" />
$(document).ready(function () {
    setInterval(function () {
        var myDate = new Date();
        if (myDate.getHours() == 0 && myDate.getMinutes() == 0 && myDate.getSeconds() == 0)
        {
            window.location.reload();
            $("#TriggerButton").click();
        }
        else {
            $("#TriggerButton").click();
        }
    }, 30000);
});

$(document).ready(function () {
    Load();
    window.onresize = function (event) {
        Load();
    };
})

function Load() {
    //E區
    $('#divEW>div>img').width($('#EContainer').width() * 8.8 / 90) // E区 女 所有圖片

    $('.ToiletLogoE img').width($('#EContainer').width() * 17 / 90)//E区 Logo

    $('.picture_toiletE img').width($('#EContainer').width() * 10 / 90)//E区 廁紙

    $('#divEM>div>img').width($('#EContainer').width() * 8.8 / 90) // E区 男 所有圖片

    $('#EValue').css('font-size', '' + (100 * $('#EValue').parent().parent().width() / 585) + 'px') //E區 文字


    //A區
    $('#divAW>div>img').width($('#AContainer').width() * 12.35 / 90) // A区 女 所有圖片

    $('.ToiletLogoA img').width($('#AContainer').width() * 23.8 / 90)//A区 Logo

    $('.picture_toiletA img').width($('#AContainer').width() * 14 / 90)//A区 廁紙

    $('#divAM>div>img').width($('#AContainer').width() * 11.21 / 90) // E区 男 所有圖片

    $('#AValue').css('font-size', '' + (100 * $('#AValue').parent().parent().width() / 585) + 'px') //A區 文字

    //F區
    $('#divFW>div>img').width($('#FContainer').width() * 8.8 / 90) // F区 女 所有圖片

    $('.ToiletLogoF img').width($('#FContainer').width() * 17 / 90)//F区 Logo

    $('.picture_toiletF img').width($('#FContainer').width() * 10 / 90)//F区 廁紙

    $('#divFM>div>img').width($('#FContainer').width() * 8.8 / 90) // F区 男 所有圖片

    $('#FValue').css('font-size', '' + (100 * $('#FValue').parent().parent().width() / 585) + 'px') //F區 文字

    //V區
    $('#divVW>div>img').width($('#VContainer').width() * 12.35 / 90) // V区 女 所有圖片

    $('.ToiletLogoV img').width($('#VContainer').width() * 23.8 / 90)//V区 Logo

    $('.picture_toiletV img').width($('#VContainer').width() * 14 / 90)//V区 廁紙

    $('#divVM>div>img').width($('#VContainer').width() * 11.21 / 90) // V区 男 所有圖片

    $('#VValue').css('font-size', '' + (100 * $('#VValue').parent().parent().width() / 585) + 'px') //F區 文字

}