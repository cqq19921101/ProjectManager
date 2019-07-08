<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NPI_CTQConfig.aspx.cs" Inherits="NPI_CTQParameterConfig" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <title>Untitled Page</title>
    <link rel="stylesheet" href="../Common.css" type="text/css" />
</head>
<body style="background-color: #DFE8F6;">
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" />
    <ext:Hidden ID="lblSite" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="lblBu" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="lblBuilding" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="lblLogonId" runat="server">
    </ext:Hidden>
    <ext:Panel ID="pnlMain" runat="server" Padding="0" Layout="Form" Title="" Border="false">
        <Items>
            <ext:Panel ID="pnlApp" runat="server" Layout="Form" Title="CTQ參數维护" Frame="True"
                Border="false">
                <Items>
                    <ext:Container ID="Container18" runat="server" Layout="ColumnLayout" Height="30">
                        <Items>
                            <ext:Container ID="Container19" runat="server" Layout="Form" ColumnWidth="0.25">
                                <Items>
                                    <ext:SelectBox ID="sbBuilding" runat="server" FieldLabel="Building" Width="120" IndicatorCls="red-text"
                                        IndicatorText="*">
                                        <Items>
                                            <ext:ListItem Text="A6" Value="A6" />
                                            <ext:ListItem Text="A3" Value="A3" />
                                        </Items>
                                        <DirectEvents>
                                            <Change OnEvent="Bind_Product">
                                            </Change>
                                        </DirectEvents>
                                    </ext:SelectBox>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                    <ext:Container ID="Container6" runat="server" Layout="ColumnLayout" Height="30">
                        <Items>
                            <ext:Container ID="Container4" runat="server" Layout="Form" ColumnWidth="0.25">
                                <Items>
                                    <ext:ComboBox ID="sbProd_group" runat="server" FieldLabel="產品類別" Width="120" DisplayField="PARAME_VALUE2"
                                        ValueField="PARAME_VALUE2" LabelWidth="80">
                                        <Store>
                                            <ext:Store ID="Store3" runat="server">
                                                <Reader>
                                                    <ext:JsonReader>
                                                        <Fields>
                                                            <ext:RecordField Name="PARAME_VALUE2" Type="String" />
                                                        </Fields>
                                                    </ext:JsonReader>
                                                </Reader>
                                            </ext:Store>
                                        </Store>
                                        <Items>
                                        </Items>
                                        <DirectEvents>
                                            <Change OnEvent="sbSelected_change">
                                            </Change>
                                        </DirectEvents>
                                    </ext:ComboBox>
                                    <%--<ext:SelectBox ID="sbProd_group" runat="server" FieldLabel="產品類別" Width="120" IndicatorCls="red-text"
                                        IndicatorText="*">
                                        <Items>
                                            <ext:ListItem Text="Charger" Value="Charger" />
                                            <ext:ListItem Text="Adapter" Value="Adapter" />
                                            <ext:ListItem Text="Apple" Value="Apple" />
                                            <ext:ListItem Text="Others" Value="Others" />
                                        </Items>
                                        <DirectEvents>
                                            <Change OnEvent="sbSelected_change">
                                            </Change>
                                        </DirectEvents>
                                    </ext:SelectBox>--%>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container2" runat="server" Layout="Form" ColumnWidth="0.25">
                                <Items>
                                    <ext:SelectBox ID="sbPhase" runat="server" FieldLabel="階段" Width="120" IndicatorCls="red-text"
                                        IndicatorText="*">
                                        <Items>
                                            <ext:ListItem Text="EVT" Value="EVT" />
                                            <ext:ListItem Text="DVT" Value="DVT" />
                                            <ext:ListItem Text="PR" Value="PR" />
                                        </Items>
                                        <DirectEvents>
                                            <Change OnEvent="sbSelected_change">
                                            </Change>
                                        </DirectEvents>
                                    </ext:SelectBox>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container3" runat="server" Layout="Form" ColumnWidth="0.25">
                                <Items>
                                    <ext:ComboBox ID="cobDept" runat="server" FieldLabel="部門" Width="120" ValueField="PARAME_VALUE2"
                                        DisplayField="PARAME_VALUE2" SelectedIndex="0" TabIndex="1">
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
                                            <Change OnEvent="sbSelected_change">
                                            </Change>
                                        </DirectEvents>
                                    </ext:ComboBox>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container5" runat="server" Layout="Form" ColumnWidth="0.25">
                                <Items>
                                    <ext:TextField ID="txtProcess" runat="server" FieldLabel="製程/材料" Width="120" IndicatorCls="red-text"
                                        IndicatorText="*">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                    <ext:Container ID="Container1" runat="server" Layout="ColumnLayout" Height="30">
                        <Items>
                            <ext:Container ID="Container7" runat="server" Layout="Form" ColumnWidth="0.25">
                                <Items>
                                    <ext:TextField ID="txtCTQ" runat="server" FieldLabel="CTQ項目" Width="120" IndicatorCls="red-text"
                                        IndicatorText="*">
                                        <DirectEvents>
                                            <Change OnEvent="txtCTQ_Change">
                                            </Change>
                                        </DirectEvents>
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container8" runat="server" Layout="Form" ColumnWidth="0.25">
                                <Items>
                                    <ext:TextField ID="txtUNIT" runat="server" FieldLabel="單位" Width="120" IndicatorCls="red-text"
                                        IndicatorText="*">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container9" runat="server" Layout="Form" ColumnWidth="0.25">
                                <Items>
                                    <ext:TextField ID="txtSPC" runat="server" FieldLabel="SPC" Width="120" Disabled="true">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container10" runat="server" Layout="Form" ColumnWidth="0.25">
                                <Items>
                                    <ext:TextField ID="txtSPEC_LIMIT" runat="server" FieldLabel="SPEC_LIMIT" Width="120"
                                        Disabled="true">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                    <ext:Container ID="Container11" runat="server" Layout="ColumnLayout" Height="30">
                        <Items>
                            <ext:Container ID="Container12" runat="server" Layout="Form" ColumnWidth="0.25">
                                <Items>
                                    <ext:SelectBox ID="sbCONTROL_TYPE" runat="server" FieldLabel="CONTROL_TYPE" Width="120"
                                        IndicatorCls="red-text" IndicatorText="*">
                                        <Items>
                                            <ext:ListItem Text="CPKs" Value="CPKs" />
                                            <ext:ListItem Text="Yield%" Value="Yield%" />
                                            <ext:ListItem Text="PCS" Value="PCS" />
                                            <ext:ListItem Text="DPMO" Value="DPMO" />
                                        </Items>
                                    </ext:SelectBox>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container13" runat="server" Layout="Form" ColumnWidth="0.25">
                                <Items>
                                    <ext:TextField ID="nubGOAL" runat="server" FieldLabel="GOAL" Width="120">
                                    </ext:TextField>
                                    <%--                                    <ext:NumberField ID="nubGOAL" runat="server" FieldLabel="GOAL" Width="120" MinValue = "0" MaxValue="2" 
                                         AllowDecimals="true" DecimalPrecision = "3" IndicatorCls="red-text" IndicatorText="*">
                                    </ext:NumberField>--%>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container16" runat="server" Layout="Form" ColumnWidth="0.25">
                                <Items>
                                    <ext:SelectBox ID="sbSeverity" runat="server" FieldLabel="嚴重程度" Width="120" IndicatorCls="red-text"
                                        IndicatorText="*">
                                        <Items>
                                            <ext:ListItem Text="Critical" Value="Critical" />
                                            <ext:ListItem Text="Major" Value="Major" />
                                            <ext:ListItem Text="Minor" Value="Minor" />
                                        </Items>
                                    </ext:SelectBox>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container15" runat="server" Layout="Form" ColumnWidth="0.25">
                                <Items>
                                    <ext:SelectBox ID="sbFlag" runat="server" FieldLabel="是否需要附件" Width="120" IndicatorCls="red-text"
                                        IndicatorText="*">
                                        <Items>
                                            <ext:ListItem Text="Y" Value="Y" />
                                            <ext:ListItem Text="Option" Value="Option" />
                                        </Items>
                                    </ext:SelectBox>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                    <ext:Container ID="Container17" runat="server" Layout="ColumnLayout" Height="30">
                        <Items>
                            <ext:Container ID="Container14" runat="server" Layout="Form" ColumnWidth="0.25">
                                <Items>
                                    <ext:Button ID="btnSave" runat="server" Text="保存" Icon="Disk">
                                        <DirectEvents>
                                            <Click OnEvent="btnSave_click">
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                </Items>
            </ext:Panel>
            <ext:GridPanel ID="grdInfo" runat="server" Height="440" Frame="true" Title="成員列表"
                Header="false">
                <Store>
                    <ext:Store ID="Store1" runat="server">
                        <Reader>
                            <ext:JsonReader>
                                <Fields>
                                    <ext:RecordField Name="ID" />
                                    <ext:RecordField Name="PROD_GROUP" />
                                    <ext:RecordField Name="PHASE" />
                                    <ext:RecordField Name="DEPT" />
                                    <ext:RecordField Name="PROCESS" />
                                    <ext:RecordField Name="CTQ" />
                                    <ext:RecordField Name="UNIT" />
                                    <ext:RecordField Name="SPC" />
                                    <ext:RecordField Name="SPEC_LIMIT" />
                                    <ext:RecordField Name="CONTROL_TYPE" />
                                    <ext:RecordField Name="GOALStr" />
                                    <ext:RecordField Name="flag" />
                                    <ext:RecordField Name="SERVITY" />
                                    <ext:RecordField Name="UPDATE_USERID" />
                                    <ext:RecordField Name="UPDATE_TIME" Type="Date" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <ColumnModel>
                    <Columns>
                        <ext:Column DataIndex="ID" Header="ID" Width="30" Hidden="true">
                        </ext:Column>
                        <ext:Column DataIndex="PROD_GROUP" Header="產品類別" Width="80">
                        </ext:Column>
                        <ext:Column DataIndex="PHASE" Header="階段" Width="80">
                        </ext:Column>
                        <ext:Column DataIndex="DEPT" Header="部門" Width="80">
                        </ext:Column>
                        <ext:Column DataIndex="PROCESS" Header="製程" Width="80">
                        </ext:Column>
                        <ext:Column DataIndex="CTQ" Header="CTQ項目" Width="180">
                        </ext:Column>
                        <ext:Column DataIndex="UNIT" Header="單位" Width="50">
                        </ext:Column>
                        <ext:Column DataIndex="SERVITY" Header="嚴重程度" Width="100">
                        </ext:Column>
                        <ext:Column DataIndex="CONTROL_TYPE" Header="CONTROL_TYPE" Width="100">
                        </ext:Column>
                        <ext:Column DataIndex="GOALStr" Header="GOAL" Width="80">
                        </ext:Column>
                        <ext:Column DataIndex="flag" Header="附件" Width="50">
                        </ext:Column>
                        <ext:Column DataIndex="UPDATE_USERID" Header="操作人" Width="100">
                        </ext:Column>
                        <ext:DateColumn DataIndex="UPDATE_TIME" Header="更新时间" Width="100" Format="yyyy/MM/dd HH:mm">
                        </ext:DateColumn>
                    </Columns>
                </ColumnModel>
                <SelectionModel>
                    <ext:CheckboxSelectionModel ID="CheckboxSelectionModel" runat="server">
                    </ext:CheckboxSelectionModel>
                </SelectionModel>
                <TopBar>
                    <ext:Toolbar runat="server">
                        <Items>
                            <ext:Button ID="btnDelete" runat="server" Text="删除" Icon="Delete">
                                <DirectEvents>
                                    <Click OnEvent="btnDelete_Click">
                                        <Confirmation ConfirmRequest="true" Message="確認刪除勾選的CTQ記錄?" Title="提示" />
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
    </form>
</body>
</html>
