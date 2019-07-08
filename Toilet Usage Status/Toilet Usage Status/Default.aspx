<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Toilet_Usage_Status.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <link href="layerui/css/layui.css" rel="stylesheet" media="all" />
    <link href="css/base1.css" rel="stylesheet" /><!--Base  css-->

    <script src="js/jquery-1.10.2.min.js"></script>
    <script src="layerui/layui.js"></script>
    <script src="js/Base.js"></script><!--Base js-->
    <script src="js/CheckIsExist.js"></script><!--检查馬桶是否有人 js-->
    <script src="js/CheckToiletPaper.js"></script><!--检查廁紙容量 百分比顯示  js-->
    <title></title>
</head>

<body>
    <form id="form1" runat="server" method="GET">
        <input type="hidden" id="TriggerButton" runat="server" />
        <fieldset class="layui-elem-field layui-field-title" style="margin-top: 20px;">
            <legend>A9 廁所使用狀況(Toilet usage status) </legend>
        </fieldset>
        <div class="layui-row">
            <!--左上(E)-->
            <div class="layui-col-xs12 layui-col-md7" id="EContainer" ">
                <div id="divEW" style="width: auto; float: left;">
                    <div class="picture_div1">
                        <img id="EWSit1" src="image/sit_1.png" />
                        <img src="image/squat_1.png" />
                        <img src="image/squat_1.png" />
                        <img src="image/squat_1.png" />
                    </div>
                    <div class="picture_div1">
                        <img src="image/squat_1.png" />
                        <img src="image/squat_1.png" />
                        <img src="image/squat_1.png" />
                        <img src="image/squat_1.png" />
                        <img src="image/squat_1.png" />
                    </div>
                </div>

                <div id="divEL" style="width:auto; float: left;">
                    <div class="ToiletLogoE">
                        <img id="ELogo" src="image/E.png" />
                    </div>
                    <div class="picture_toiletE">
                        <img id="EPaper" src="image/enough.png" />
                        <div id="EArea1">
                            <span id="EValue"></span>
                        </div>
                    </div>
                </div>

                <div id="divEM" style="width: auto; float: left;">
                    <div class="picture_div_man_left1">
                        <img id="EMSit1" src="image/sit_1.png" />
                        <img src="image/squat_1.png" />
                    </div>

                    <div class="picture_div_man_left1">
                        <img src="image/squat_1.png" />
                    </div>

                </div>
            </div>

            <!--右上(A)-->
            <div class="layui-col-xs12 layui-col-md5" id="AContainer"">
                <div id="divAW" style="width:auto; float: left;">
                    <div class="picture_div1">
                        <img id="AWSit1" src="image/sit_1.png" />
                    </div>
                    <div class="picture_div1">
                        <img src="image/sit_1.png" />
                        <img src="image/sit_1.png" />
                    </div>
                </div>

                <div id="divAL" style="width: auto; float: left;">
                    <div class="ToiletLogoA">
                        <img id="ALogo" src="image/A.png" />
                    </div>
                    <div class="picture_toiletA">
                        <img id="APaper" src="image/enough.png" />
                        <div id="AArea1">
                            <span id="AValue"></span>
                        </div>
                    </div>
                </div>

                <div id="divAM" style="width: auto; float: left;">
                    <div class="picture_div1">
                        <img id="AMSit1" src="image/sit_1.png" />
                    </div>
                    <div class="picture_div1">
                        <img src="image/sit_1.png" />
                        <img src="image/sit_1.png" />
                    </div>
                </div>
            </div>

            <!--左下(F)-->
            <div class="layui-col-xs12 layui-col-md7" id="FContainer" ">
                <div id="divFW" style="width:auto; float: left;">
                    <div class="picture_div1">
                        <img id="FWSit1" src="image/sit_1.png" />
                        <img src="image/squat_1.png" />
                        <img src="image/squat_1.png" />
                        <img src="image/squat_1.png" />
                    </div>
                    <div class="picture_div1">
                        <img src="image/squat_1.png" />
                        <img src="image/squat_1.png" />
                        <img src="image/squat_1.png" />
                        <img src="image/squat_1.png" />
                        <img src="image/squat_1.png" />
                    </div>
                </div>

                <div id="divFL" style="width:auto; float: left;">
                    <div class="ToiletLogoF">
                        <img id="FLogo" src="image/F.png" />
                    </div>
                    <div class="picture_toiletF">
                        <img id="FPaper" src="image/enough.png" />
                        <div id="FArea1">
                            <span id="FValue"></span>
                        </div>
                    </div>
                </div>

                <div id="divFM" style="width:auto; float: left;">
                    <div class="picture_div_man_left1">
                        <img id="FMSit1" src="image/sit_1.png" />
                        <img src="image/squat_1.png" />
                    </div>

                    <div class="picture_div_man_left1">
                        <img src="image/squat_1.png" />
                    </div>

                </div>
            </div>


            <!--右下(V)-->
            <div class="layui-col-xs12 layui-col-md5" id="VContainer">
                <div id="divVW" style="width:auto; float: left;">
                    <div class="picture_div1">
                        <img id="VWSit1" src="image/sit_1.png" />
                    </div>
                    <div class="picture_div1">
                        <img src="image/sit_1.png" />
                        <img src="image/sit_1.png" />
                    </div>
                </div>

                <div id="divVL" style="width: auto; float: left;">
                    <div class="ToiletLogoV">
                        <img id="VLogo" src="image/V.png" />
                    </div>
                    <div class="picture_toiletV">
                        <img id="VPaper" src="image/enough.png" />
                        <div id="VArea1">
                            <span id="VValue"></span>
                        </div>
                    </div>
                </div>

                <div id="divVM" style="width: auto; float: left;">
                    <div class="picture_div1">
                        <img id="VMSit1" src="image/sit_1.png" />
                    </div>
                    <div class="picture_div1">
                        <img src="image/sit_1.png" />
                        <img src="image/sit_1.png" />
                    </div>
                </div>
            </div>

        </div>
    </form>
</body>
</html>
