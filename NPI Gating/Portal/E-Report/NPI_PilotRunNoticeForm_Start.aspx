<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NPI_PilotRunNoticeForm_Start.aspx.cs"
    Inherits="NPI_NPI_PilotRunNoticeForm_Start" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="X-UA-Compatible" content="IE=8" />    <title>Untitled Page</title>
    <link rel="stylesheet" href="../Common.css" type="text/css" />
    <style type="text/css">
        .txt_head
        {
            height: 30px;
            width: 170px;
        }
        .ddl_head
        {
            height: 30px;
            width: 170px;
        }
        .txt_content
        {
            height: 30px;
            width: 280px;
        }
        .td_desc
        {
            height: 40px;
            width: 120px;
        }
        .td_display
        {
            height: 40px;
            width: 180px;
        }
        .td_display2
        {
            height: 40px;
            width: 300px;
        }
        table.tb_content td
        {
            border-width: 1px;
            border-style: solid;
        }
    </style>
</head>
<body style="background-color: #DFE8F6;">
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" />
    <ext:Hidden ID="lblLogonId" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="lblSite" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="lblBu" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="lblBuilding" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="Hidden2" runat="server" Text="dewliu">
    </ext:Hidden>
    <ext:Panel ID="pnlMain" runat="server" Padding="0" Layout="Form" Title="" Border="false"
        BodyStyle="background-color: #DFE8F6;">
        <Items>
            <ext:Panel ID="Panel1" runat="server" Layout="Form" Title="" Header="false" BodyStyle="background-color: #DFE8F6;"
                Border="false">
                <Items>
                    <ext:Panel ID="Panel2" runat="server" Layout="Form" Title="" Header="false" BodyStyle="background-color: #DFE8F6;"
                        Border="false">
                        <Content>
                            <table id="tb_subject" runat="server" width="990" style="border-collapse: collapse;
                                border-spacing: inherit; empty-cells: hide; table-layout: auto;">
                                <tr>
                                    <td colspan="6" align="center" style="font-family: 新細明體; font-weight: bold; font-size: 22px;
                                        height: 40px">
                                        新產品試產通知單
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 30px; font-family: 'Times New Roman', Times, serif; font-weight: bold;
                                        font-size: 22px;" align="center" colspan="6">
                                        New Product Pilot Run Notice Form
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2" align="right" style="width: 300px">
                                    </td>
                                    <td rowspan="2" align="right" style="width: 300px">
                                    </td>
                                    <td rowspan="2" align="center" style="width: 300px">
                                    </td>
                                </tr>
                            </table>
                        </Content>
                    </ext:Panel>
                    <ext:Panel ID="Panel9" runat="server" Layout="Form" Title="" Header="false" BodyStyle="background-color: #DFE8F6;"
                        Border="false">
                        <Items>
                            <ext:TableLayout ID="TableLayout1" runat="server" Columns="3">
                                <Cells>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel44" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Padding="10" Width="250" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel45" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Padding="10" Width="250" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel3" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Padding="10" Width="250" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel46" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Padding="10" Width="300" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:TextField ID="txtAPPLY_DATE" runat="server" FieldLabel="<a>申請日期</a><br><a>Apply Date</a>"
                                                    Width="250" LabelSeparator=" " ReadOnly="true">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel13" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Padding="10" Width="300" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:TextField ID="txtDOC_NO" runat="server" FieldLabel="<a>單號</a><br><a>Doc No</a>"
                                                    LabelSeparator=" " Width="250" ReadOnly="true">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel14" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Padding="10" Width="300" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:SelectBox ID="sbProd_group" runat="server" FieldLabel="<a>產品類別</a><br><a>Product Group</a>"
                                                    LabelSeparator=" " Width="230" IndicatorCls="red-text" IndicatorText="*">
                                                    <Items>
                                                        <ext:ListItem Text="Charger" Value="Charger" />
                                                        <ext:ListItem Text="Adapter" Value="Adapter" />
                                                        <ext:ListItem Text="Apple" Value="Apple" />
                                                        <ext:ListItem Text="Others" Value="Others" />
                                                    </Items>
                                                </ext:SelectBox>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel15" runat="server" Header="false" Frame="false" Padding="10"
                                            Width="300" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:TextField ID="txtMODEL_NAME" runat="server" FieldLabel="<a>機種名稱</a><br><a>Model Name</a>"
                                                    LabelSeparator=" " Width="230" IndicatorCls="red-text" IndicatorText="*">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel8" runat="server" Header="false" Frame="false" Padding="10" Width="300"
                                            Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:TextField ID="txtCUSTOMER" runat="server" FieldLabel="<a>客戶</a><br><a>Customer</a>"
                                                    LabelSeparator=" " Width="230" IndicatorCls="red-text" IndicatorText="*">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel50" runat="server" Header="false" Frame="false" Padding="10"
                                            Width="300" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:TextField ID="txtNPI_PM" runat="server" FieldLabel="<a>NPI PM</a><br>" LabelSeparator=" "
                                                    Width="230">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel51" runat="server" Header="false" Frame="false" Padding="10"
                                            Width="300" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:TextField ID="txtSALES_OWNER" runat="server" FieldLabel="<a>業務負責人</a><br><a>Sales Owner</a>"
                                                    LabelSeparator=" " Width="230">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel10" runat="server" Header="false" Frame="false" Padding="10"
                                            Width="300" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:TextField ID="txtME_ENGINEER" runat="server" FieldLabel="<a>ME工程師</a><br><a>ME Engineer</a>"
                                                    LabelSeparator=" " Width="230">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel11" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Padding="10" Width="300" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:TextField ID="txtEE_ENGINEER" runat="server" FieldLabel="<a>EE工程師</a><br><a>EE Engineer</a>"
                                                    LabelSeparator=" " Width="230">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel12" runat="server" Header="false" Frame="false" Padding="10"
                                            Width="300" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:TextField ID="txtCAD_ENGINEER" runat="server" FieldLabel="<a>CAD工程師</a><br><a>CAD Engineer</a>"
                                                    LabelSeparator=" " Width="230">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel52" runat="server" Header="false" Frame="false" Padding="10"
                                            Width="300" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:TextField ID="txtTP_PM" runat="server" FieldLabel="TP PM" LabelSeparator=" "
                                                    Width="230">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                </Cells>
                            </ext:TableLayout>
                        </Items>
                    </ext:Panel>
                    <ext:Panel ID="Panel16" runat="server" Layout="Form" Title="基本資訊" Header="True" BodyStyle="background-color: #DFE8F6;"
                        Border="false" Width="1000">
                        <Items>
                            <ext:TableLayout ID="TableLayout2" runat="server" Columns="4">
                                <Cells>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel17" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="100" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="TextField10" runat="server" FieldLabel="<a>項次</a><br><a>Item</a>">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel18" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="230" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label1" runat="server" FieldLabel="<a>類別</a><br><a>Des</a>">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel19" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="300" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label2" runat="server" FieldLabel="<a>說明</a><br><a>Explanation</a>">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel20" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="300" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label3" runat="server" FieldLabel="<a>備註</a><br><a>Remark</a>">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel21" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="100" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label4" runat="server" FieldLabel="<a>1</a><br><a>&nbsp;</a" LabelSeparator=" ">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel22" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="300" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label5" runat="server" FieldLabel="<a>產品規格描述</a><br><a>Product Des</a>"
                                                    LabelSeparator=" ">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel23114" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Padding="10" Width="300" LabelWidth="1" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:TextArea ID="txtPRODUCT_DES" runat="server" FieldLabel=" " Height="30" LabelSeparator=" "
                                                    Width="280" IndicatorCls="red-text" IndicatorText="*" Disabled="true">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel24" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="300" LabelWidth="1" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:TextArea ID="txtPRODUCT_DES_REMARK" runat="server" FieldLabel=" " Height="30"
                                                    LabelSeparator=" " Width="300" Disabled="true">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel23" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="100" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label6" runat="server" FieldLabel="<a>2</a><br><a>&nbsp;</a" LabelSeparator=" ">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel25" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="230" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label7" runat="server" FieldLabel="<a>量產需求日期</a><br><a>Kick off Plan</a>"
                                                    LabelSeparator=" ">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel26" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Padding="10" Width="300" LabelWidth="1" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:TextArea ID="txtSALES_AREA" runat="server" FieldLabel=" " Height="30" LabelSeparator=" "
                                                    Width="280" IndicatorCls="red-text" IndicatorText="*" Disabled="true">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel27" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="300" LabelWidth="1" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:TextArea ID="txtSALES_AREA_REMARK" runat="server" FieldLabel=" " Height="30"
                                                    LabelSeparator=" " Width="300" Disabled="true">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel28" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="100" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label8" runat="server" FieldLabel="<a>3</a><br><a>&nbsp;</a" LabelSeparator=" ">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel29" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="230" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label9" runat="server" FieldLabel="<a>量產每月需求數</a><br><a>Monthly Volumn forecast</a>"
                                                    LabelSeparator=" ">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel30" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Padding="10" Width="300" LabelWidth="1" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:TextArea ID="txtMP_DATE" runat="server" FieldLabel=" " Height="30" LabelSeparator=" "
                                                    Width="280" IndicatorCls="red-text" IndicatorText="*" Disabled="true">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel31" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="300" LabelWidth="1" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:TextArea ID="txtMP_DATE_REMARK" runat="server" FieldLabel=" " Height="30" LabelSeparator=" "
                                                    Width="300" Disabled="true">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel32" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="100" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label10" runat="server" FieldLabel="<a>4</a><br><a>&nbsp;</a" LabelSeparator=" ">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel33" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="230" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label11" runat="server" FieldLabel="<a>EVT 階段時間</a><br><a>EVT Time</a>"
                                                    LabelSeparator=" ">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel34" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Padding="10" Width="300" LabelWidth="1" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:DateField ID="txtPROJECT_CODE" runat="server" FieldLabel=" " Height="30" LabelSeparator=" "
                                                    Width="280" IndicatorCls="red-text" IndicatorText="*">
                                                </ext:DateField>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel35" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="300" LabelWidth="1" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:TextArea ID="txtPROJECT_CODE_REMARK" runat="server" FieldLabel=" " Height="30"
                                                    LabelSeparator=" " Width="300">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel36" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="100" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label12" runat="server" FieldLabel="<a>5</a><br><a>&nbsp;</a" LabelSeparator=" ">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel37" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="230" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label13" runat="server" FieldLabel="<a>DVT 階段時間</a><br><a>DVT Time</a>"
                                                    LabelSeparator=" ">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel38" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Padding="10" Width="300" LabelWidth="1" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:DateField ID="txtTIME_QUANTITY" runat="server" FieldLabel=" " Height="30" LabelSeparator=" "
                                                    Width="280" IndicatorCls="red-text" IndicatorText="*">
                                                </ext:DateField>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel39" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="300" LabelWidth="1" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:TextArea ID="txtTIME_QUANTITY_REMARK" runat="server" FieldLabel=" " Height="30"
                                                    LabelSeparator=" " Width="300">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel4" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="100" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label16" runat="server" FieldLabel="<a>6</a><br><a>&nbsp;</a" LabelSeparator=" ">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel5" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="230" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label17" runat="server" FieldLabel="<a>PR 階段時間</a><br><a>PR Time</a>"
                                                    LabelSeparator=" ">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel6" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Padding="10" Width="300" LabelWidth="1" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:DateField ID="txtPRPhaseTime" runat="server" FieldLabel=" " Height="30" LabelSeparator=" "
                                                    Width="280">
                                                </ext:DateField>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel7" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="300" LabelWidth="1" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:TextArea ID="txtPRphase_Remark" runat="server" FieldLabel=" " Height="30" LabelSeparator=" "
                                                    Width="300">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel40" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="100" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label14" runat="server" FieldLabel="<a>7</a><br><a>&nbsp;</a" LabelSeparator=" ">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel41" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="230" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label15" runat="server" FieldLabel="<a>其他事項</a><br><a>Others</a>"
                                                    LabelSeparator=" ">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel42" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Padding="10" Width="300" LabelWidth="1" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:TextArea ID="txtOTHERS" runat="server" FieldLabel=" " Height="30" LabelSeparator=" "
                                                    Width="280">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel43" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="300" LabelWidth="1" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:TextArea ID="txtOTHERS_REMARK" runat="server" FieldLabel=" " Height="30" LabelSeparator=" "
                                                    Width="300">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                </Cells>
                            </ext:TableLayout>
                        </Items>
                    </ext:Panel>
                </Items>
            </ext:Panel>
            <ext:Panel ID="Panel49" runat="server" Layout="Form" Frame="True" AutoHeight="true"
                Title="團隊成員維護">
                <Items>
                <ext:Panel ID="Panel53" runat="server" Padding="0" Layout="Form" 
                Frame="false">
                <Items>
                    <ext:Container ID="Container4" runat="server" Layout="ColumnLayout" Height="40">
                        <Items>
                            <ext:Container ID="Container5" runat="server" Layout="FormLayout" ColumnWidth="0.25"
                                LabelWidth="70">
                                <Items>
                                    <ext:RadioGroup ID="rgBatch" runat="server" FieldLabel="批量錄入" Width="400">
                                        <Items>
                                            <ext:Radio ID="rdBatchY" runat="server" BoxLabel="是" Width="40" />
                                            <ext:Radio ID="rdPBatchN" runat="server" BoxLabel="否" Width="40" />
                                        </Items>
                                        <DirectEvents>
                                            <Change OnEvent="RgBatch_Changed">
                                            </Change>
                                        </DirectEvents>
                                    </ext:RadioGroup>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                  
                    <ext:Container ID="ContainerBatch" runat="server" Layout="ColumnLayout" Height="60"
                        Hidden="true">
                        <Items>
                            <ext:Container ID="Container7" runat="server" Layout="FormLayout" ColumnWidth="0.3"
                                LabelWidth="70">
                                <Items>
                                    <ext:FileUploadField ID="FileAttachment" runat="server" FieldLabel="批量上傳" LabelSeparator=" "
                                        ButtonText="請選擇文件" Width="200">
                                    </ext:FileUploadField>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container8" runat="server" Layout="FormLayout" ColumnWidth="0.2"
                                LabelWidth="70">
                                <Items>
                                    <ext:Button ID="btnConfirm1" runat="server" Width="80" Text="上傳" Icon="Accept">
                                        <DirectEvents>
                                            <Click OnEvent="btnConfirm_Click">
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container9" runat="server" Layout="FormLayout" ColumnWidth="0.2"
                                LabelWidth="80">
                                <Items>
                                    <ext:HyperLink ID="lkSample" runat="server" Text="模板下载" NavigateUrl="TeamMember_UploadTemp.xlsx" />
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                </Items>
            </ext:Panel>
                    <ext:Panel ID="pnlApp" runat="server" Layout="Form" AutoHeight="true" Border="false">
                        <Items>
                        
                            <ext:TableLayout ID="TableLayout4" runat="server" Columns="3">
                                <Cells>
                                    <ext:Cell ColSpan="1">
                                        <ext:Container ID="Container25" runat="server" Width="300" LabelWidth="80">
                                            <Items>
                                                <ext:ComboBox ID="cmbType" runat="server" Width="250" TabIndex="1" ValueField="PARAME_VALUE3"
                                                    DisplayField="PARAME_VALUE2" FieldLabel="團隊類型">
                                                    <Store>
                                                        <ext:Store ID="StoreMember" runat="server">
                                                            <Reader>
                                                                <ext:JsonReader>
                                                                    <Fields>
                                                                        <ext:RecordField Name="PARAME_VALUE2" />
                                                                        <ext:RecordField Name="PARAME_VALUE3" />
                                                                    </Fields>
                                                                </ext:JsonReader>
                                                            </Reader>
                                                        </ext:Store>
                                                    </Store>
                                                    <DirectEvents>
                                                        <Select OnEvent="ChangeDept">
                                                        </Select>
                                                    </DirectEvents>
                                                </ext:ComboBox>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                </Cells>
                                <Cells>
                                    <ext:Cell ColSpan="2">
                                        <ext:Container ID="Container38" runat="server" Width="300" LabelWidth="80">
                                            <Items>
                                                <ext:ComboBox ID="cobDept" runat="server" Width="200" ValueField="PARAME_VALUE2"
                                                    DisplayField="PARAME_VALUE2" SelectedIndex="0" TabIndex="1" FieldLabel="部門名稱">
                                                    <Store>
                                                        <ext:Store ID="Store2" runat="server">
                                                            <Reader>
                                                                <ext:JsonReader>
                                                                    <Fields>
                                                                        <ext:RecordField Name="PARAME_VALUE2" />
                                                                    </Fields>
                                                                </ext:JsonReader>
                                                            </Reader>
                                                        </ext:Store>
                                                    </Store>
                                                    <DirectEvents>
                                                        <Select OnEvent="ChangeDept">
                                                        </Select>
                                                    </DirectEvents>
                                                </ext:ComboBox>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                </Cells>
                                <Cells>
                                    <ext:Cell ColSpan="1">
                                        <ext:Container ID="Container39" runat="server" LabelWidth="80" Width="300">
                                            <Items>
                                                <ext:SelectBox ID="sbName" runat="server" Mode="Remote" ValueField="ENAME" DisplayField="CNAME"
                                                    Width="250" FieldLabel="填寫人員">
                                                    <Store>
                                                        <ext:Store ID="Store1" runat="server">
                                                            <Reader>
                                                                <ext:JsonReader>
                                                                    <Fields>
                                                                        <ext:RecordField Name="CNAME" />
                                                                        <ext:RecordField Name="ENAME" />
                                                                    </Fields>
                                                                </ext:JsonReader>
                                                            </Reader>
                                                        </ext:Store>
                                                    </Store>
                                                </ext:SelectBox>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell ColSpan="1">
                                        <ext:Container ID="Container66" runat="server" LabelWidth="80" Width="300">
                                            <Items>
                                                <ext:SelectBox ID="sbReply" runat="server" Mode="Remote" ValueField="ENAME" DisplayField="CNAME"
                                                    Width="200" FieldLabel="回覆人員">
                                                    <Store>
                                                        <ext:Store ID="Store5" runat="server">
                                                            <Reader>
                                                                <ext:JsonReader>
                                                                    <Fields>
                                                                        <ext:RecordField Name="CNAME" />
                                                                        <ext:RecordField Name="ENAME" />
                                                                    </Fields>
                                                                </ext:JsonReader>
                                                            </Reader>
                                                        </ext:Store>
                                                    </Store>
                                                </ext:SelectBox>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                </Cells>
                                <Cells>
                                    <ext:Cell ColSpan="1">
                                        <ext:Container ID="Container45" runat="server" Width="300" LabelWidth="80">
                                            <Items>
                                                <ext:SelectBox ID="sbChecked" runat="server" Mode="Remote" ValueField="ENAME" DisplayField="CNAME"
                                                    Width="200" FieldLabel="審核人員">
                                                    <Store>
                                                        <ext:Store ID="Store4" runat="server">
                                                            <Reader>
                                                                <ext:JsonReader>
                                                                    <Fields>
                                                                        <ext:RecordField Name="CNAME" />
                                                                        <ext:RecordField Name="ENAME" />
                                                                    </Fields>
                                                                </ext:JsonReader>
                                                            </Reader>
                                                        </ext:Store>
                                                    </Store>
                                                </ext:SelectBox>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                </Cells>
                            </ext:TableLayout>
                        </Items>
                    </ext:Panel>
                    <ext:GridPanel ID="grdInfo" runat="server" Height="440" Frame="true" Title="成員列表"
                        Header="false">
                        <Store>
                            <ext:Store runat="server" ID="Store3">
                                <Reader>
                                    <ext:JsonReader>
                                        <Fields>
                                            <ext:RecordField Name="ID" />
                                            <ext:RecordField Name="Category" />
                                            <ext:RecordField Name="DEPT" />
                                            <ext:RecordField Name="WriteEname" />
                                            <ext:RecordField Name="ReplyEName" />
                                            <ext:RecordField Name="CheckedEName" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>
                                <ext:Column DataIndex="ID" Header="ID" Width="30" Hidden="true">
                                </ext:Column>
                                <ext:Column Header="團隊類別" Width="120" DataIndex="Category">
                                </ext:Column>
                                <ext:Column Header="部門名稱" Width="100" DataIndex="DEPT">
                                </ext:Column>
                                <ext:Column Header="填寫人員" Width="150" DataIndex="WriteEname">
                                </ext:Column>
                                <ext:Column Header="回覆人員" Width="150" DataIndex="ReplyEName">
                                </ext:Column>
                                <ext:Column Header="審核人員" Width="150" DataIndex="CheckedEName">
                                </ext:Column>
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:CheckboxSelectionModel ID="CheckboxSelectionModel" runat="server">
                            </ext:CheckboxSelectionModel>
                        </SelectionModel>
                        <TopBar>
                            <ext:Toolbar ID="Toolbar1" runat="server">
                                <Items>
                                    <ext:Button ID="btnAdd" runat="server" Text="保存" Icon="Add">
                                        <DirectEvents>
                                            <Click OnEvent="btnAdd_Click">
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button ID="btnDelete" runat="server" Text="删除" Icon="Delete">
                                        <DirectEvents>
                                            <Click OnEvent="btnDelete_Click">
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{grdInfo}.getRowsValues({selectedOnly:true}))"
                                                        Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                    </ext:GridPanel>
                </Items>
            </ext:Panel>
        </Items>
    </ext:Panel>
    <ext:Panel ID="Panel47" runat="server" Layout="Form" Title="" Width="990" Border="false">
        <Items>
            <ext:Panel ID="Panel48" runat="server" Layout="Form" Title="人员维护" Header="false"
                BodyStyle="background-color: #DFE8F6;" Border="false">
                <Content>
                    <table id="Table1" runat="server" width="990" style="border-collapse: collapse; border-spacing: inherit;
                        empty-cells: hide; table-layout: auto;">
                        <tr>
                            <td rowspan="2" align="right" style="width: 460px">
                                <ext:Button ID="btnSave" runat="server" Text="提交" Width="90" Icon="Accept">
                                    <directevents>
                                                <Click OnEvent="btnSave_click">
                                                </Click>
                                            </directevents>
                                </ext:Button>
                            </td>
                            <td rowspan="2" align="left" style="width: 460px">
                                <ext:Button ID="btnContinue" Disabled="true" runat="server" Text="繼續錄入" Width="90"
                                    Icon="ArrowRefresh">
                                    <directevents>
                                                <Click OnEvent="btnContinue_click">
                                                </Click>
                                            </directevents>
                                </ext:Button>
                            </td>
                        </tr>
                    </table>
                </Content>
            </ext:Panel>
        </Items>
    </ext:Panel>
    </Items> </ext:Panel>
    </form>
</body>
</html>
