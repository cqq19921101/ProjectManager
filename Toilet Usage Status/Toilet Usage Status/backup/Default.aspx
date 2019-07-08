<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Toilet_Usage_Status.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <link href="layerui/css/layui.css" rel="stylesheet" media="all" />
    <link href="css/base1.css" rel="stylesheet" />
    <script src="js/jquery-1.10.2.min.js"></script>
    <script src="layerui/layui.js"></script>
    <script src="js/Base.js"></script><!--Base-->
    <script src="js/CheckIsExist.js"></script><!--检查馬桶是否有人-->
    <script src="js/CheckToiletPaper.js"></script><!--检查廁紙容量 百分比顯示-->
    <title></title>
</head>

<body>
    <form id="form1" runat="server" method="GET">
        <fieldset class="layui-elem-field layui-field-title" style="margin-top: 20px;">
            <legend>A9 廁所使用狀況(Toilet usage status) </legend>
        </fieldset>
        <div class="layui-fluid">
        <div class="layui-row">
            <!--左上(E)-->
            <div  class="layui-col-xs12 layui-col-md7" style="border: 1px solid #000000;">
                <!--女廁-->
                <div class="layui-col-xs12 layui-col-md6">
                    <!--第一層-->
                    <div class="layui-col-xs12 layui-col-md10">
                        <div class="grid-demo grid-demo-bg1 picture_div">
                            <img id="ESit1" src="image/sit_1.png" />
                        </div>
                            <%--<span id="EValue1">test</span>--%>
                        <div class="grid-demo grid-demo-bg1 picture_div">
                            <img src="image/squat_1.png" />
                        </div>
                        <div class="grid-demo grid-demo-bg1 picture_div">
                            <img src="image/squat_1.png" />
                        </div>
                        <div class="grid-demo grid-demo-bg1 picture_div">
                            <img src="image/squat_1.png" />
                        </div>
                    </div>
                    <!--第二層-->
                    <div class="layui-col-xs12 layui-col-md12">
                        <div class="grid-demo grid-demo-bg1 picture_div">
                            <img src="image/squat_1.png" />
                        </div>
                        <div class="grid-demo grid-demo-bg1 picture_div">
                            <img src="image/squat_1.png" />
                        </div>
                        <div class="grid-demo grid-demo-bg1 picture_div">
                            <img src="image/squat_1.png" />
                        </div>
                        <div class="grid-demo grid-demo-bg1 picture_div">
                            <img src="image/squat_1.png" />
                        </div>
                        <div class="grid-demo grid-demo-bg1 picture_div">
                            <img src="image/squat_1.png" />
                        </div>
                    </div>
                </div>
                <!--標識牌 男或女 / 廁紙-->
                <div class="layui-col-xs12 layui-col-md3">
                    <!--第一層 標識牌-->
                    <div class="layui-col-xs12 layui-col-md10">
                        <div class="grid-demo grid-demo-bg1 ToiletLogo">
                            <img src="image/E.png" />
                        </div>
                    </div>
                    <!--第二層 廁紙-->
                    <div class="layui-col-xs12 layui-col-md10">
                        <div class="grid-demo grid-demo-bg1 picture_toilet">
                            <img id ="EPaper" src="image/enough.png" />
                            <div id="EArea1" >
                                <span id ="EValue"></span>
                            </div>
                        </div>
                    </div>
                </div>
                <!--男廁-->
                <div class="layui-col-xs6 layui-col-md3">
                    <div class="layui-col-xs12 layui-col-md12">
                        <div class="grid-demo grid-demo-bg1 picture_div_man_left">
                            <img src="image/squat_1.png" />
                        </div>
                        <div class="grid-demo grid-demo-bg1 picture_div">
                            <img id="EArea-M" src="image/sit_1.png" />
                        </div>
                    </div>
                    <div class="layui-col-xs12 layui-col-md12">
                        <div class="grid-demo grid-demo-bg1 picture_div_man_left">
                            <img src="image/squat_1.png" />
                        </div>
                    </div>
                </div>
            </div>

            <!--右上(A)-->
            <div class="layui-col-xs12 layui-col-md5" style="border: 1px solid #000000;">
                <!--女廁-->
                <div class="layui-col-xs12 layui-col-md4">
                    <!--第一層-->
                    <div class="layui-col-xs12 layui-col-md10">
                        <div class="grid-demo grid-demo-bg1 picture_div">
                            <img src="image/sit_1.png" />
                            <div  style ="text-align:center;color:darkgreen;">
                                <span id ="AValue1"></span>
                            </div>
                        </div>
                    </div>
                    <!--第二層-->
                    <div class="layui-col-xs12 layui-col-md10">
                        <div class="grid-demo grid-demo-bg1 picture_div">
                            <img src="image/sit_1.png" />
                            <div  style ="text-align:center;color:darkgreen;">
                                <span id ="AValue2"></span>
                            </div>
                        </div>
                        <div class="grid-demo grid-demo-bg1 picture_div">
                            <img src="image/sit_1.png" />
                            <div  style ="text-align:center;color:darkgreen;">
                                <span id ="AValue3"></span>
                            </div>
                        </div>

                    </div>
                </div>
                <!--標識牌 男或女 / 廁紙-->
                <div class="layui-col-xs12 layui-col-md4">
                    <!--第一層 標識牌-->
                    <div class="layui-col-xs12 layui-col-md10">
                        <div class="grid-demo grid-demo-bg1 ToiletLogo">
                            <img src="image/A.png" />
                        </div>
                    </div>
                    <!--第二層 廁紙-->
                    <div class="layui-col-xs12 layui-col-md10">
                        <div class="grid-demo grid-demo-bg1 picture_toilet">
                            <img id ="APaper" src="image/enough.png" />
                            <div id="AArea1" >
                                <span id ="AValue"></span>
                            </div>
                        </div>
                    </div>
                </div>
                <!--男廁-->
                <div class="layui-col-xs6 layui-col-md4">
                    <div class="layui-col-xs12 layui-col-md10">
                        <div class="grid-demo grid-demo-bg1 picture_div">
                            <img src="image/sit_1.png" />
                            <div  style ="text-align:center;color:darkgreen;">
                                <span id ="AValue4"></span>
                            </div>
                        </div>
                    </div>
                    <div class="layui-col-xs12 layui-col-md10">
                        <div class="grid-demo grid-demo-bg1 picture_div">
                            <img src="image/sit_1.png" />
                            <div  style ="text-align:center;color:darkgreen;">
                                <span id ="AValue5"></span>
                            </div>
                        </div>
                        <div class="grid-demo grid-demo-bg1 picture_div">
                            <img src="image/sit_1.png" />
                            <div  style ="text-align:center;color:darkgreen;">
                                <span id ="AValue6"></span>
                            </div>
                        </div>

                    </div>

                </div>

            </div>

            <!--左下(F)-->
            <div class="layui-col-xs12 layui-col-md7" style="border: 1px solid #000000; margin-top: 5px">
                <!--女廁-->
                <div class="layui-col-xs12 layui-col-md6">
                    <!--第一層-->
                    <div class="layui-col-xs12 layui-col-md10">
                        <div class="grid-demo grid-demo-bg1 picture_div">
                            <img src="image/sit_1.png" />
                        </div>
                        <div class="grid-demo grid-demo-bg1 picture_div">
                            <img src="image/squat_1.png" />
                        </div>
                        <div class="grid-demo grid-demo-bg1 picture_div">
                            <img src="image/squat_1.png" />
                        </div>
                        <div class="grid-demo grid-demo-bg1 picture_div">
                            <img src="image/squat_1.png" />
                        </div>
                    </div>
                    <!--第二層-->
                    <div class="layui-col-xs12 layui-col-md12">
                        <div class="grid-demo grid-demo-bg1 picture_div">
                            <img src="image/squat_1.png" />
                        </div>
                        <div class="grid-demo grid-demo-bg1 picture_div">
                            <img src="image/squat_1.png" />
                        </div>
                        <div class="grid-demo grid-demo-bg1 picture_div">
                            <img src="image/squat_1.png" />
                        </div>
                        <div class="grid-demo grid-demo-bg1 picture_div">
                            <img src="image/squat_1.png" />
                        </div>
                        <div class="grid-demo grid-demo-bg1 picture_div">
                            <img src="image/squat_1.png" />
                        </div>
                    </div>
                </div>

                <!--標識牌 男或女-->
                <div class="layui-col-xs12 layui-col-md3">
                    <!--第一層 標識牌-->
                    <div class="layui-col-xs12 layui-col-md10">
                        <div class="grid-demo grid-demo-bg1 ToiletLogo">
                            <img src="image/F.png" />
                        </div>
                    </div>
                    <!--第二層 廁紙-->
                    <div class="layui-col-xs12 layui-col-md10">
                        <div class="grid-demo grid-demo-bg1 picture_toilet">
                            <img id ="FPaper" src="image/enough.png" />
                            <div id="FArea1" >
                                <span id ="FValue"></span>
                            </div>
                        </div>
                    </div>
                </div>
                <!--男廁-->
                <div class="layui-col-xs6 layui-col-md3">
                    <div class="layui-col-xs12 layui-col-md12">
                        <div class="grid-demo grid-demo-bg1 picture_div_man_left">
                            <img src="image/squat_1.png" />
                        </div>
                        <div class="grid-demo grid-demo-bg1 picture_div">
                            <img src="image/sit_1.png" />
                        </div>
                    </div>

                    <div class="layui-col-xs12 layui-col-md12">
                        <div class="grid-demo grid-demo-bg1 picture_div_man_left">
                            <img src="image/squat_1.png" />
                        </div>
                    </div>
                </div>
            </div>

            <!--右下(V)-->
            <div class="layui-col-xs12 layui-col-md5" style="border: 1px solid #000000;margin-top: 5px">
                <!--女廁-->
                <div class="layui-col-xs12 layui-col-md4">
                    <!--第一層-->
                    <div class="layui-col-xs12 layui-col-md10">
                        <div class="grid-demo grid-demo-bg1 picture_div">
                            <img src="image/sit_1.png" />
                        </div>
                    </div>
                    <!--第二層-->
                    <div class="layui-col-xs12 layui-col-md10">
                        <div class="grid-demo grid-demo-bg1 picture_div">
                            <img src="image/sit_1.png" />
                        </div>
                        <div class="grid-demo grid-demo-bg1 picture_div">
                            <img src="image/sit_1.png" />
                        </div>

                    </div>
                </div>
                <!--標識牌 男或女 / 廁紙-->
                <div class="layui-col-xs12 layui-col-md4">
                    <!--第一層 標識牌-->
                    <div class="layui-col-xs12 layui-col-md10">
                        <div class="grid-demo grid-demo-bg1 ToiletLogo">
                            <img src="image/V.png" />
                        </div>
                    </div>
                    <!--第二層 廁紙-->
                    <div class="layui-col-xs12 layui-col-md10">
                        <div class="grid-demo grid-demo-bg1 picture_toilet">
                            <img id ="VPaper" src="image/enough.png" />
                            <div id="VArea1" >
                                <span id ="VValue"></span>
                            </div>
                        </div>
                    </div>
                </div>
                <!--男廁-->
                <div class="layui-col-xs6 layui-col-md4">
                    <div class="layui-col-xs12 layui-col-md10">
                        <div class="grid-demo grid-demo-bg1 picture_div">
                            <img src="image/sit_1.png" />
                        </div>
                    </div>
                    <div class="layui-col-xs12 layui-col-md10">
                        <div class="grid-demo grid-demo-bg1 picture_div">
                            <img src="image/sit_1.png" />
                        </div>
                        <div class="grid-demo grid-demo-bg1 picture_div">
                            <img src="image/sit_1.png" />
                        </div>

                    </div>

                </div>

            </div>

        </div>
            </div>
    </form>
</body>
</html>
