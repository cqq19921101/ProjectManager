/// <reference path="jquery-1.10.2.min.js" />


$(function () {
    $("#TriggerButton").click(function () {
        $.post("ajax/CheckIsExists.ashx", { "cmd": "EArea-W" }, function (data) {
            var data = eval("(" + data + ")");//解析Json
            if (data.Success == true) {
                if (data.Info == "0x30") {
                    $("#EWSit1").attr("src", "image/sit_1.png");//0x30 無人 圖片替換
                }
                else {
                    $("#EWSit1").attr("src", "image/sit_2.png");//0x31 有人 圖片替換
                }
            }
            else {

            }

        })


    })
})



//E區 女廁 馬桶是否有人
$(document).ready(function () {
    $.post("ajax/CheckIsExists.ashx", { "cmd": "EArea-W" }, function (data) {
        var data = eval("(" + data + ")");//解析Json
        if (data.Success == true) {
            if (data.Info == "0x30") {
                $("#EWSit1").attr("src", "image/sit_1.png");//0x30 無人 圖片替換
            }
            else {
                $("#EWSit1").attr("src", "image/sit_2.png");//0x31 有人 圖片替換
            }
        }
        else {

        }

    })
})

//E區 男廁 馬桶是否有人
$(document).ready(function () {
    $.post("ajax/CheckIsExists.ashx", { "cmd": "EArea-M" }, function (data) {
        var data = eval("(" + data + ")");//解析Json
        if (data.Success == true) {
            if (data.Info == "0x30") {
                $("#ESit1").attr("src", "image/sit_1.png");//0x30 無人 圖片替換
            }
            else {
                $("#ESit1").attr("src", "image/sit_2.png");//0x31 有人 圖片替換
            }
        }
        else {

        }

    })
})

