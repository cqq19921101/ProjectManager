<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DFX_ItemNum.aspx.cs" Inherits="Web_E_Report_DFX_ItemNum" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="X-UA-Compatible" content="IE=8" />    <title>編碼資料維護</title>
</head>
<body style="background-color: #DFE8F6;">
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" />
    <ext:Hidden ID="lblLogonId" runat="server" Text="">
    </ext:Hidden>
    <ext:Hidden ID="lblSite" runat="server" Text="">
    </ext:Hidden>
    <ext:Hidden ID="lblBu" runat="server" Text="">
    </ext:Hidden>
    <ext:Hidden ID="lblBuilding" runat="server" Text="">
    </ext:Hidden>
    <ext:Panel ID="pnlMain" runat="server" Padding="0" Layout="Form" ButtonAlign="Center"
        Title="" Frame="True" AutoHeight="true">
        <Items>
            <ext:Panel ID="pnlApp" runat="server" Layout="ColumnLayout" Title="DFX 編碼維護" Frame="True"
                AutoHeight="true">
                <Items>
                    <ext:Panel ID="Panel2" runat="server" Layout="FormLayout" ColumnWidth="0.25" Height="50"
                        LabelWidth="60">
                        <Items>
                            <ext:ComboBox ID="cobNum" runat="server" FieldLabel="編碼位" Width="120" TabIndex="1" DisplayField="PARAME_VALUE1"
                             ValueField="PARAME_VALUE2">
                             <Store>
                                    <ext:Store ID="Store2" runat="server">
                                        <Reader>
                                            <ext:JsonReader>
                                                <Fields>
                                                    <ext:RecordField Name="PARAME_VALUE1" />
                                                    <ext:RecordField Name="PARAME_VALUE2" />
                                                </Fields>
                                            </ext:JsonReader>
                                        </Reader>
                                    </ext:Store>
                                </Store>
                                <DirectEvents>
                                    <Select OnEvent="cobNum_Select">
                                    </Select>
                                </DirectEvents>
                            </ext:ComboBox>
                        </Items>
                    </ext:Panel>
                    <ext:Panel ID="Panel1" runat="server" Layout="FormLayout" ColumnWidth="0.25" Height="50"
                        LabelWidth="60">
                        <Items>
                            <ext:ComboBox ID="cobType" runat="server" FieldLabel="類型" Width="120" ValueField="PARAME_VALUE1"
                                DisplayField="PARAME_VALUE2" TabIndex="2">
                                <Store>
                                    <ext:Store ID="Store14" runat="server">
                                        <Reader>
                                            <ext:JsonReader>
                                                <Fields>
                                                    <ext:RecordField Name="PARAME_VALUE1" />
                                                    <ext:RecordField Name="PARAME_VALUE2" />
                                                </Fields>
                                            </ext:JsonReader>
                                        </Reader>
                                    </ext:Store>
                                </Store>
                                <DirectEvents>
                                    <Select OnEvent="cobType_Select">
                                    </Select>
                                </DirectEvents>
                            </ext:ComboBox>
                        </Items>
                    </ext:Panel>
                    <ext:Panel ID="Panel3" runat="server" Layout="FormLayout" ColumnWidth="0.25" Height="50"
                        LabelWidth="60">
                        <Items>
                            <ext:TextField ID="txtValue" runat="server" FieldLabel="名稱" Width="120" TabIndex="3">
                            </ext:TextField>
                        </Items>
                    </ext:Panel>
                    <ext:Panel ID="Panel4" runat="server" Layout="FormLayout" ColumnWidth="0.25" Height="50"
                        LabelWidth="60">
                        <Items>
                            <ext:TextField ID="txtCode" runat="server" FieldLabel="編碼" Width="120" TabIndex="3">
                            </ext:TextField>
                        </Items>
                    </ext:Panel>
                </Items>
            </ext:Panel>
            <ext:GridPanel ID="grdInfo" runat="server" Height="450" Frame="true" Title="編碼原則基本資料"
                Header="false">
                <Store>
                    <ext:Store ID="Store1" runat="server">
                        <Reader>
                            <ext:JsonReader>
                                <Fields>
                                    <ext:RecordField Name="ID" Type="Int" />
                                    <ext:RecordField Name="FUNCTION_NAME" />
                                    <ext:RecordField Name="PARAME_NAME" />
                                    <ext:RecordField Name="PARAME_ITEM" />
                                    <ext:RecordField Name="PARAME_VALUE1" />
                                    <ext:RecordField Name="PARAME_VALUE2" />
                                    <ext:RecordField Name="UPDATE_USER" />
                                    <ext:RecordField Name="UPDATE_TIME" Type="Date" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <ColumnModel>
                    <Columns>
                        <ext:NumberColumn DataIndex="ID" Header="ID" Hidden="true">
                        </ext:NumberColumn>
                        <ext:Column DataIndex="FUNCTION_NAME" Header="FUNCTION_NAME" Width="100" Hidden="true">
                        </ext:Column>
                        <ext:Column DataIndex="PARAME_NAME" Header="類型" Width="100">
                        </ext:Column>
                        <ext:Column DataIndex="PARAME_VALUE1" Header="編碼" Width="100">
                        </ext:Column>
                        <ext:Column DataIndex="PARAME_VALUE2" Header="名稱" Width="100">
                        </ext:Column>
                        <ext:Column DataIndex="UPDATE_USER" Header="操作者" Width="100">
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
                    <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <ext:Button ID="btnInsert" runat="server" Text="新增" Icon="Add">
                                <DirectEvents>
                                    <Click OnEvent="btnInsert_Click">
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
    </form>
</body>
</html>
