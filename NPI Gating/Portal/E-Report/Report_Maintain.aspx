<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Report_Maintain.aspx.cs"
    Inherits="Report_Maintain" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
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

    <script type="text/javascript">
        var template = '<span style="color:{0};">{1}</span>';
        var StatusFormat = function (value) {
            return String.format(template, (value == 'Finished') ? "green" : "red", value);
        };
        var winClose = function () {
            return setTimeout("window.close()", 2000);
        }

    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" />
    <ext:Hidden ID="lblLogonId" runat="server" Text="">
    </ext:Hidden>
    <ext:Hidden ID="hdBU" runat="server" Text="">
    </ext:Hidden>
    <ext:Hidden ID="lblPGroup" runat="server" Text="">
    </ext:Hidden>
    <ext:Hidden ID="lblBuilding" runat="server" Text="">
    </ext:Hidden>
    <ext:Hidden ID="lblID_B" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="lblID_G" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="lblID_S" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="lblStatus_B" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="lblStatus_G" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="lblStatus_S" runat="server">
    </ext:Hidden>
    <ext:Panel ID="pnlMain" runat="server" Padding="0" Layout="Form" Title="" Border="false"
        BodyStyle="background-color: #DFE8F6;">
        <Items>
            <ext:Panel ID="pnlApp" runat="server" Layout="Form" Title="檢索條件設定" Frame="True" Border="false">
                <Items>
                    <ext:Container ID="Container6" runat="server" Layout="ColumnLayout" Height="30">
                        <Items>
                            <ext:Container ID="Container5" runat="server" Layout="Form" ColumnWidth="0.2">
                                <Items>
                                    <ext:ComboBox ID="cmbBuilding" runat="server" FieldLabel="Building" Width="110" IndicatorCls="red-text"
                                        IndicatorText="*">
                                        <Items>
                                            <ext:ListItem Text="A6" Value="A6" />
                                            <ext:ListItem Text="GZ" Value="GZ" />
                                        </Items>
                                        <SelectedItem Value="A6" Text="A3" />
                                    </ext:ComboBox>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                    <ext:Container ID="Container3" runat="server" Layout="ColumnLayout" Height="30">
                        <Items>
                            <ext:Container ID="Container1" runat="server" Layout="Form" ColumnWidth="0.2">
                                <Items>
                                    <ext:TextField ID="txtModel" runat="server" FieldLabel="機種" Width="110" LabelSeparator="">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container2" runat="server" Layout="Form" ColumnWidth="0.2">
                                <Items>
                                    <ext:TextField ID="txtPM" runat="server" FieldLabel="NPIPM" Width="110" LabelSeparator="">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                </Items>
            </ext:Panel>
            <ext:GridPanel ID="grdInfo" runat="server" Title="明細列表" Frame="true" AutoScroll="true"
                Height="600" Header="false">
                <Store>
                    <ext:Store ID="Store2" runat="server">
                        <Reader>
                            <ext:JsonReader>
                                <Fields>
                                    <ext:RecordField Name="DOC_NO">
                                    </ext:RecordField>
                                    <ext:RecordField Name="SUB_DOC_NO">
                                    </ext:RecordField>
                                    <ext:RecordField Name="SUB_DOC_PHASE">
                                    </ext:RecordField>
                                    <ext:RecordField Name="PROD_GROUP">
                                    </ext:RecordField>
                                    <ext:RecordField Name="MODEL_NAME">
                                    </ext:RecordField>
                                    <ext:RecordField Name="Date">
                                    </ext:RecordField>
                                    <ext:RecordField Name="NPI_PM">
                                    </ext:RecordField>
                                    <ext:RecordField Name="BU" />
                                    <ext:RecordField Name="BUILDING" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <ColumnModel>
                    <Columns>
                        <ext:CommandColumn Width="70">
                            <Commands>
                                <ext:GridCommand Icon="Pencil" CommandName="UpdateReport" Text="修改">
                                </ext:GridCommand>
                            </Commands>
                        </ext:CommandColumn>
                        <ext:Column Header="試產單號" DataIndex="DOC_NO" Width="150">
                        </ext:Column>
                        <ext:Column Header="WF單號" DataIndex="SUB_DOC_NO" Width="150">
                        </ext:Column>
                        <ext:Column DataIndex="Date" Header="申請日期" Width="90">
                        </ext:Column>
                        <ext:Column DataIndex="BU" Header="BU" Width="60">
                        </ext:Column>
                        <ext:Column DataIndex="NPI_PM" Header="NPI PM" Width="100">
                        </ext:Column>
                        <ext:Column DataIndex="PROD_GROUP" Header="產品類別" Width="80">
                        </ext:Column>
                        <ext:Column DataIndex="MODEL_NAME" Header="機種" Width="140">
                        </ext:Column>
                    </Columns>
                </ColumnModel>
                <SelectionModel>
                    <ext:CheckboxSelectionModel ID="CheckboxSelectionModel3" runat="server">
                    </ext:CheckboxSelectionModel>
                </SelectionModel>
                <TopBar>
                    <ext:Toolbar ID="Toolbar3" runat="server">
                        <Items>
                            <ext:Button ID="btnQuery" runat="server" Text="檢索" Icon="Zoom">
                                <DirectEvents>
                                    <Click OnEvent="btnQuery_click">
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <DirectEvents>
                    <Command OnEvent="grdNPI_RowCommand">
                        <ExtraParams>
                            <ext:Parameter Name="command" Value="command" Mode="Raw" />
                            <ext:Parameter Name="DOC_NO" Value="record.data.DOC_NO" Mode="Raw" />
                            <ext:Parameter Name="SUB_DOC_NO" Value="record.data.SUB_DOC_NO" Mode="Raw" />
                            <ext:Parameter Name="PROD_GROUP" Value="record.data.PROD_GROUP" Mode="Raw" />
                            <ext:Parameter Name="MODEL_NAME" Value="record.data.MODEL_NAME" Mode="Raw" />
                        </ExtraParams>
                    </Command>
                </DirectEvents>
            </ext:GridPanel>
        </Items>
    </ext:Panel>
    <ext:Window ID="winInfo" runat="server" Resizable="false" Icon="Pencil" Modal="true"
        Title="Info" Width="1200" Padding="5" Hidden="true" Layout="Form" AutoHeight="true "
        AutoScroll="true">
        <Items>
            <ext:Panel ID="Panel1" runat="server" Layout="Form" Title="NPI基本資料修改" Border="false"
                BodyStyle="background-color: #DFE8F6;" Padding="5" Header="false">
                <Items>
                    <ext:Container ID="Container4" runat="server" Layout="ColumnLayout" Height="30">
                        <Items>
                            <ext:Container ID="Container734" runat="server" Layout="Form" ColumnWidth="0.33"
                                LabelWidth="120">
                                <Items>
                                    <ext:TextField ID="txtWDOCNO" runat="server" FieldLabel="試產單號" Width="150" Disabled="true">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container348" runat="server" Layout="Form" ColumnWidth="0.33"
                                LabelWidth="120">
                                <Items>
                                    <ext:TextField ID="txtWSubDOCNO" runat="server" FieldLabel="WF單號" Width="150" Disabled="true">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container16" runat="server" Layout="Form" ColumnWidth="0.33" LabelWidth="120">
                                <Items>
                                    <ext:TextField ID="txtWModelName" runat="server" FieldLabel="機種" Width="150" Disabled="true">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                    <ext:Container ID="Container334" runat="server" Layout="ColumnLayout" Height="30">
                        <Items>
                            <ext:Container ID="Container384" runat="server" Layout="Form" ColumnWidth="0.33"
                                LabelWidth="120">
                                <Items>
                                    <ext:ComboBox ID="cmbChoose" runat="server" FieldLabel="修改內容" Width="150">
                                        <Items>
                                            <ext:ListItem Text="基本資料" Value="A" />
                                            <ext:ListItem Text="團隊成員" Value="B" />
                                        </Items>
                                        <DirectEvents>
                                            <Select OnEvent="cbChoose_Event">
                                                <EventMask ShowMask="true" Msg="加載中..." MinDelay="500" />
                                            </Select>
                                        </DirectEvents>
                                    </ext:ComboBox>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                </Items>
            </ext:Panel>
            <ext:Panel ID="pnlBasic" runat="server" Layout="Form" Title="基本資料修改" Border="false"
                BodyStyle="background-color: #DFE8F6;" Padding="5" Hidden="true" Collapsible="True">
                <Items>
                    <ext:Container ID="Container150" runat="server" Layout="ColumnLayout" Height="30">
                        <Items>
                            <ext:Container ID="Container151" runat="server" Layout="Form" ColumnWidth="0.33"
                                LabelWidth="120" Width="383.99">
                                <Items>
                                    <ext:SelectBox ID="sbProd_group" runat="server" FieldLabel="產品類別" LabelSeparator=" "
                                        Width="150" Disabled="true">
                                        <Items>
                                            <ext:ListItem Text="Charger" Value="Charger" />
                                            <ext:ListItem Text="Adapter" Value="Adapter" />
                                            <ext:ListItem Text="Apple" Value="Apple" />
                                            <ext:ListItem Text="Others" Value="Others" />
                                            <ext:ListItem Text="DT" Value="DT" />
                                        </Items>
                                    </ext:SelectBox>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container152" runat="server" Layout="Form" ColumnWidth="0.33"
                                LabelWidth="120" Width="383.99">
                                <Items>
                                    <ext:TextField ID="txtWMODEL_NAME" runat="server" FieldLabel="機種名稱" LabelSeparator=" "
                                        Width="150" Disabled="true">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container153" runat="server" Layout="Form" ColumnWidth="0.33"
                                LabelWidth="120" Width="383.99">
                                <Items>
                                    <ext:TextField ID="txtWCUSTOMER" runat="server" FieldLabel="客戶" LabelSeparator=" "
                                        Width="150" Disabled="true">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                    <ext:Container ID="Container154" runat="server" Layout="ColumnLayout" Height="30">
                        <Items>
                            <ext:Container ID="Container155" runat="server" Layout="Form" ColumnWidth="0.33"
                                LabelWidth="120" Width="383.99">
                                <Items>
                                    <ext:TextField ID="txtWNPIPM" runat="server" FieldLabel="NPI PM" LabelSeparator=" "
                                        Width="150">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container8" runat="server" Layout="Form" ColumnWidth="0.33" LabelWidth="120"
                                Width="383.99">
                                <Items>
                                    <ext:TextField ID="txtWSales" runat="server" FieldLabel="業務負責人" LabelSeparator=" "
                                        Width="150">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container7" runat="server" Layout="Form" ColumnWidth="0.33" LabelWidth="120"
                                Width="383.99">
                                <Items>
                                    <ext:TextField ID="txtWME" runat="server" FieldLabel="ME工程師" LabelSeparator=" " Width="150">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                    <ext:Container ID="Container9" runat="server" Layout="ColumnLayout" Height="30">
                        <Items>
                            <ext:Container ID="Container10" runat="server" Layout="Form" ColumnWidth="0.33" LabelWidth="120"
                                Width="383.99">
                                <Items>
                                    <ext:TextField ID="txtWEE" runat="server" FieldLabel="EE工程師" LabelSeparator=" " Width="150">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container11" runat="server" Layout="Form" ColumnWidth="0.33" LabelWidth="120"
                                Width="383.99">
                                <Items>
                                    <ext:TextField ID="txtWCAD" runat="server" FieldLabel="CAD工程師" LabelSeparator=" "
                                        Width="150">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container12" runat="server" Layout="Form" ColumnWidth="0.33" LabelWidth="120"
                                Width="383.99">
                                <Items>
                                    <ext:TextField ID="txtWTPPM" runat="server" FieldLabel="TP PM" LabelSeparator=" "
                                        Width="150">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                    <ext:Container ID="Container21" runat="server" Layout="ColumnLayout" Height="30">
                        <Items>
                            <ext:Container ID="Container22" runat="server" Layout="Form" ColumnWidth="0.33" LabelWidth="120"
                                Width="383.99">
                                <Items>
                                    <ext:TextField ID="txtEVTDate" runat="server" FieldLabel="EVT階段時間" LabelSeparator=" "
                                        Width="150" Disabled="true">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container23" runat="server" Layout="Form" ColumnWidth="0.33" LabelWidth="120"
                                Width="383.99">
                                <Items>
                                    <ext:TextField ID="txtEVTRemark" runat="server" FieldLabel="Remark" LabelSeparator=" "
                                        Width="300" Disabled="true">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                    <ext:Container ID="Container24" runat="server" Layout="ColumnLayout" Height="30">
                        <Items>
                            <ext:Container ID="Container26" runat="server" Layout="Form" ColumnWidth="0.33" LabelWidth="120"
                                Width="383.99">
                                <Items>
                                    <ext:TextField ID="txtDVTDate" runat="server" FieldLabel="DVT階段時間" LabelSeparator=" "
                                        Width="150" Disabled="true">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container27" runat="server" Layout="Form" ColumnWidth="0.33" LabelWidth="120"
                                Width="383.99">
                                <Items>
                                    <ext:TextField ID="txtDVTRemark" runat="server" FieldLabel="Remark" LabelSeparator=" "
                                        Width="300" Disabled="true">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                    <ext:Container ID="Container28" runat="server" Layout="ColumnLayout" Height="30">
                        <Items>
                            <ext:Container ID="Container29" runat="server" Layout="Form" ColumnWidth="0.33" LabelWidth="120"
                                Width="383.99">
                                <Items>
                                    <ext:TextField ID="txtPRDate" runat="server" FieldLabel="PR階段時間" LabelSeparator=" "
                                        Width="150" Disabled="true">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container30" runat="server" Layout="Form" ColumnWidth="0.33" LabelWidth="120"
                                Width="383.99">
                                <Items>
                                    <ext:TextField ID="txtPRRemark" runat="server" FieldLabel="Remark" LabelSeparator=" "
                                        Width="300" Disabled="true">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                    <ext:Container ID="Container225" runat="server" Layout="Form">
                        <Items>
                            <ext:Button ID="btnSave_B" runat="server" Text="修改" Icon="Disk">
                                <DirectEvents>
                                    <Click OnEvent="btnSave_Click">
                                        <Confirmation ConfirmRequest="true" Message="確認是否修改！" Title="提示" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                        </Items>
                    </ext:Container>
                </Items>
            </ext:Panel>
            <ext:Panel ID="pnlMember" runat="server" Layout="Form" Title="團隊成員變更" Border="false"
                BodyStyle="background-color: #DFE8F6;" Padding="5" Hidden="true" Collapsible="True">
                <Items>
                    <ext:Container ID="Container13" runat="server" Layout="ColumnLayout" Height="30">
                        <Items>
                            <ext:Container ID="Container14" runat="server" Layout="Form" ColumnWidth="0.33" LabelWidth="120"
                                Width="383.99">
                                <Items>
                                    <ext:ComboBox ID="TeamCatagory" runat="server" FieldLabel="分類" LabelSeparator=" "
                                        Width="150">
                                        <Items>
                                            <ext:ListItem Text="DFX TeamMember" Value="DFX TeamMember" />
                                            <ext:ListItem Text="CTQ TeamMember" Value="CTQ TeamMember" />
                                            <ext:ListItem Text="ISSUES TeamMember" Value="ISSUES TeamMember" />
                                            <ext:ListItem Text="PFMEA TeamMember" Value="PFMEA TeamMember" />
                                            <ext:ListItem Text="Prepared TeamMember" Value="Prepared TeamMember" />
                                            <ext:ListItem Text="PM TeamMember" Value="PM TeamMember" />
                                            <ext:ListItem Text="Manager TeamMember" Value="Manager TeamMember" />
                                        </Items>
                                        <DirectEvents>
                                            <Select OnEvent="Select_Catagory">
                                            </Select>
                                        </DirectEvents>
                                    </ext:ComboBox>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container15" runat="server" Layout="Form" ColumnWidth="0.33" LabelWidth="120"
                                Width="383.99">
                                <Items>
                                    <ext:ComboBox ID="cmbDept" runat="server" FieldLabel="部門" Width="150" DisplayField="DEPT"
                                        ValueField="DEPT">
                                        <Store>
                                            <ext:Store ID="Store5" runat="server">
                                                <Reader>
                                                    <ext:JsonReader>
                                                        <Fields>
                                                            <ext:RecordField Name="DEPT" Type="String" />
                                                        </Fields>
                                                    </ext:JsonReader>
                                                </Reader>
                                            </ext:Store>
                                        </Store>
                                        <Items>
                                        </Items>
                                        <DirectEvents>
                                            <Change OnEvent="Select_Dept">
                                            </Change>
                                        </DirectEvents>
                                    </ext:ComboBox>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                    <ext:Container ID="Container17" runat="server" Layout="ColumnLayout" Height="30">
                        <Items>
                            <ext:Container ID="Container18" runat="server" Layout="Form" ColumnWidth="0.33" LabelWidth="120"
                                Width="383.99">
                                <Items>
                                    <ext:TextField ID="txtWrite" runat="server" FieldLabel="WriteEName" LabelSeparator=" "
                                        Width="150">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container19" runat="server" Layout="Form" ColumnWidth="0.33" LabelWidth="120"
                                Width="383.99">
                                <Items>
                                    <ext:TextField ID="txtReply" runat="server" FieldLabel="ReplyEName" LabelSeparator=" "
                                        Width="150">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container20" runat="server" Layout="Form" ColumnWidth="0.33" LabelWidth="120"
                                Width="383.99">
                                <Items>
                                    <ext:TextField ID="txtCheck" runat="server" FieldLabel="CheckedEName" LabelSeparator=" "
                                        Width="150">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                    <ext:Container ID="Container25" runat="server" Layout="Form">
                        <Items>
                            <ext:Button ID="Button1" runat="server" Text="更新" Icon="Disk">
                                <DirectEvents>
                                    <Click OnEvent="btnSave_Member">
                                        <Confirmation ConfirmRequest="true" Message="確認是否修改！" Title="提示" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                        </Items>
                    </ext:Container>
                    <ext:GridPanel ID="grdMember" runat="server" Title="明細列表" Frame="true" AutoScroll="true"
                        AutoHeight="true" Header="false">
                        <Store>
                            <ext:Store ID="Store1" runat="server">
                                <Reader>
                                    <ext:JsonReader>
                                        <Fields>
                                            <ext:RecordField Name="Category">
                                            </ext:RecordField>
                                            <ext:RecordField Name="DEPT">
                                            </ext:RecordField>
                                            <ext:RecordField Name="WriteEname">
                                            </ext:RecordField>
                                            <ext:RecordField Name="ReplyEName">
                                            </ext:RecordField>
                                            <ext:RecordField Name="CheckedEName">
                                            </ext:RecordField>
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>
                                <ext:Column DataIndex="Category" Header="分類" Width="150">
                                </ext:Column>
                                <ext:Column DataIndex="DEPT" Header="部門" Width="80">
                                </ext:Column>
                                <ext:Column DataIndex="WriteEname" Header="Write人員" Width="120">
                                </ext:Column>
                                <ext:Column DataIndex="ReplyEName" Header="Reply人員" Width="120">
                                </ext:Column>
                                <ext:Column DataIndex="CheckedEName" Header="Check人員" Width="120">
                                </ext:Column>
                            </Columns>
                        </ColumnModel>
                    </ext:GridPanel>
                </Items>
            </ext:Panel>
        </Items>
    </ext:Window>
    </form>
</body>
</html>
