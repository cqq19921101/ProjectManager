/// <reference path="jquery-1.10.2.min.js" />

$(function () {

    $("#TriggerButton").click(function () {
        //E區 廁紙檢查
        $.post("ajax/CheckToiletPaper.ashx", { "cmd": "E-Area" }, function (data) {
            var data = eval("(" + data + ")");//解析Json
            var PercentValue = data.Info;
            if (data.Success == true) {
                if (data.Info == "無數據") {
                    $("#EArea1").addClass("Percent-Font-Green");
                    $("#EPaper").attr("src", "image/Empty.png");//無數據 Empty.png
                    $("#EValue").html(data.Info);
                }
                else {
                    //判斷廁紙容量小於10 字體設置red  大於10  字體設置darkgreen
                    if (PercentValue < 10) {
                        $("#EArea1").addClass("Percent-Font-Red");
                        $("#EPaper").attr("src", "image/notenough.png");//廁紙容量不夠替換圖片

                    }
                    else {
                        $("#EArea1").addClass("Percent-Font-Green");
                        $("#EPaper").attr("src", "image/Enough.png");//廁紙容量夠替換圖片
                    }

                    $("#EValue").html(data.Info + "%");//Value賦值

                }
            }
            else {

            }
        })

        //A區廁紙檢查
        $.post("ajax/CheckToiletPaper.ashx", { "cmd": "A-Area" }, function (data) {
            var data = eval("(" + data + ")");//解析Json
            var PercentValue = data.Info;
            if (data.Success == true) {
                if (data.Info == "無數據") {
                    $("#AArea1").addClass("Percent-Font-Green");
                    $("#APaper").attr("src", "image/Empty.png");//無數據 Empty.png
                    $("#AValue").html(data.Info);
                }
                else {
                    //判斷廁紙容量小於10 字體設置red  大於10  字體設置darkgreen
                    if (PercentValue < 10) {
                        $("#AArea1").addClass("Percent-Font-Red");
                        $("#APaper").attr("src", "image/notenough.png");//廁紙容量不夠替換圖片

                    }
                    else {
                        $("#AArea1").addClass("Percent-Font-Green");
                        $("#APaper").attr("src", "image/Enough.png");//廁紙容量夠替換圖片
                    }

                    $("#AValue").html(data.Info + "%");//Value賦值

                }
            }
            else {

            }
        })

        //F區廁紙檢查
        $.post("ajax/CheckToiletPaper.ashx", { "cmd": "F-Area" }, function (data) {
            var data = eval("(" + data + ")");//解析Json
            var PercentValue = data.Info;
            if (data.Success == true) {
                if (data.Info == "無數據") {
                    $("#FArea1").addClass("Percent-Font-Green");
                    $("#FPaper").attr("src", "image/Empty.png");//無數據 Empty.png
                    $("#FValue").html(data.Info);
                }
                else {
                    //判斷廁紙容量小於10 字體設置red  大於10  字體設置darkgreen
                    if (PercentValue < 10) {
                        $("#FArea1").addClass("Percent-Font-Red");
                        $("#FPaper").attr("src", "image/notenough.png");//廁紙容量不夠替換圖片

                    }
                    else {
                        $("#FArea1").addClass("Percent-Font-Green");
                        $("#FPaper").attr("src", "image/Enough.png");//廁紙容量夠替換圖片
                    }

                    $("#FValue").html(data.Info + "%");//Value賦值

                }
            }
            else {

            }
        })

        //V區廁紙檢查
        $.post("ajax/CheckToiletPaper.ashx", { "cmd": "A-Area" }, function (data) {
            var data = eval("(" + data + ")");//解析Json
            var PercentValue = data.Info;
            if (data.Success == true) {
                if (data.Info == "無數據") {
                    $("#VArea1").addClass("Percent-Font-Green");
                    $("#VPaper").attr("src", "image/Empty.png");//無數據 Empty.png
                    $("#VValue").html(data.Info);
                }
                else {
                    //判斷廁紙容量小於10 字體設置red  大於10  字體設置darkgreen
                    if (PercentValue < 10) {
                        $("#VArea1").addClass("Percent-Font-Red");
                        $("#VPaper").attr("src", "image/notenough.png");//廁紙容量不夠替換圖片

                    }
                    else {
                        $("#VArea1").addClass("Percent-Font-Green");
                        $("#VPaper").attr("src", "image/Enough.png");//廁紙容量夠替換圖片
                    }

                    $("#VValue").html(data.Info + "%");//Value賦值

                }
            }
            else {

            }
        })
    })
})

$(function () {

})


//E區 廁紙檢查
$(document).ready(function () {
    $.post("ajax/CheckToiletPaper.ashx", { "cmd": "E-Area" }, function (data) {
        var data = eval("(" + data + ")");//解析Json
        var PercentValue = data.Info;
        if (data.Success == true) {
            if (data.Info == "無數據") {
                $("#EArea1").addClass("Percent-Font-Green");
                $("#EPaper").attr("src", "image/Empty.png");//無數據 Empty.png
                $("#EValue").html(data.Info);
            }
            else
            {
                //判斷廁紙容量小於10 字體設置red  大於10  字體設置darkgreen
                if (PercentValue < 10) {
                    $("#EArea1").addClass("Percent-Font-Red");
                    $("#EPaper").attr("src", "image/notenough.png");//廁紙容量不夠替換圖片

                }
                else {
                    $("#EArea1").addClass("Percent-Font-Green");
                    $("#EPaper").attr("src", "image/Enough.png");//廁紙容量夠替換圖片
                }

                $("#EValue").html(data.Info + "%");//Value賦值

            }
        }
        else {

        }
    })
})

//F區 廁紙檢查
$(document).ready(function () {
    $.post("ajax/CheckToiletPaper.ashx", { "cmd": "F-Area" }, function (data) {
        var data = eval("(" + data + ")");//解析Json
        var PercentValue = data.Info;
        if (data.Success == true) {
            if (data.Info == "無數據") {
                $("#FArea1").addClass("Percent-Font-Green");
                $("#FPaper").attr("src", "image/Empty.png");//無數據 Empty.png
                $("#FValue").html(data.Info);
            }
            else {
                //判斷廁紙容量小於10 字體設置red  大於10  字體設置darkgreen
                if (PercentValue < 10) {
                    $("#FArea1").addClass("Percent-Font-Red");
                    $("#FPaper").attr("src", "image/notenough.png");//廁紙容量不夠替換圖片

                }
                else {
                    $("#FArea1").addClass("Percent-Font-Green");
                    $("#FPaper").attr("src", "image/Enough.png");//廁紙容量夠替換圖片
                }

                $("#FValue").html(data.Info + "%");//Value賦值

            }
        }
        else {

        }
    })
})

//A區 廁紙檢查
$(document).ready(function () {
    $.post("ajax/CheckToiletPaper.ashx", { "cmd": "A-Area" }, function (data) {
        var data = eval("(" + data + ")");//解析Json
        var PercentValue = data.Info;
        if (data.Success == true) {
            if (data.Info == "無數據") {
                $("#AArea1").addClass("Percent-Font-Green");
                $("#APaper").attr("src", "image/Empty.png");//無數據 Empty.png
                $("#AValue").html(data.Info);
            }
            else {
                //判斷廁紙容量小於10 字體設置red  大於10  字體設置darkgreen
                if (PercentValue < 10) {
                    $("#AArea1").addClass("Percent-Font-Red");
                    $("#APaper").attr("src", "image/notenough.png");//廁紙容量不夠替換圖片

                }
                else {
                    $("#AArea1").addClass("Percent-Font-Green");
                    $("#APaper").attr("src", "image/Enough.png");//廁紙容量夠替換圖片
                }

                $("#AValue").html(data.Info + "%");//Value賦值

            }
        }
        else {

        }
    })
})

//VIP區 廁紙檢查
$(document).ready(function () {
    $.post("ajax/CheckToiletPaper.ashx", { "cmd": "A-Area" }, function (data) {
        var data = eval("(" + data + ")");//解析Json
        var PercentValue = data.Info;
        if (data.Success == true) {
            if (data.Info == "無數據") {
                $("#VArea1").addClass("Percent-Font-Green");
                $("#VPaper").attr("src", "image/Empty.png");//無數據 Empty.png
                $("#VValue").html(data.Info);
            }
            else {
                //判斷廁紙容量小於10 字體設置red  大於10  字體設置darkgreen
                if (PercentValue < 10) {
                    $("#VArea1").addClass("Percent-Font-Red");
                    $("#VPaper").attr("src", "image/notenough.png");//廁紙容量不夠替換圖片

                }
                else {
                    $("#VArea1").addClass("Percent-Font-Green");
                    $("#VPaper").attr("src", "image/Enough.png");//廁紙容量夠替換圖片
                }

                $("#VValue").html(data.Info + "%");//Value賦值

            }
        }
        else {

        }
    })
})