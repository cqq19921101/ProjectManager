<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DFX_TeamMember.aspx.cs" Inherits="Web_E_Report_DFX_TeamMember" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="X-UA-Compatible" content="IE=8" />    <title></title>
</head>
<body  style="background-color: #DFE8F6;">
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" />
    <ext:Hidden ID="lblLogonId" runat="server" Text="">
    </ext:Hidden>
         <ext:Hidden ID="lblSite" runat="server" Text=""/>
      <ext:Hidden ID="lblBu" runat="server" Text=""/>
        <ext:Hidden ID="txtBuilding" runat="server" Text=""/>
  
    <ext:Panel ID="pnlMain" runat="server" Padding="0" Layout="Form" ButtonAlign="Center"
        Title="" Frame="True" AutoHeight="true">
        <Items>
            <ext:Panel ID="pnlApp" runat="server" Layout="ColumnLayout" Title="团队成员维护" Frame="True"
                AutoHeight="true">
                <Items>
                    <ext:Panel ID="Panel2" runat="server" Layout="FormLayout" ColumnWidth="0.25" Height="25"
                        LabelWidth="60">
                        <Items>
                            <ext:ComboBox ID="cobDept" runat="server" FieldLabel="責任部門" Width="120" ValueField="PARAME_VALUE1"
                                DisplayField="PARAME_VALUE1" SelectedIndex="0" TabIndex="1">
                                <Store>
                                    <ext:Store runat="server">
                                        <Reader>
                                            <ext:JsonReader>
                                                <Fields>
                                                    <ext:RecordField Name="PARAME_VALUE1" />
                                                </Fields>
                                            </ext:JsonReader>
                                        </Reader>
                                    </ext:Store>
                                </Store>
                                <DirectEvents>
                                    <Select OnEvent="cobDept_Select"></Select>
                                </DirectEvents>
                            </ext:ComboBox>
                        </Items>
                    </ext:Panel>
                    <ext:Panel ID="Panel1" runat="server" Layout="FormLayout" ColumnWidth="0.25" Height="25"
                        LabelWidth="60">
                        <Items>
                            <ext:TextField ID="txtEnName" runat="server" FieldLabel="英文名稱" Width="120" TabIndex="2">
                                <DirectEvents>
                                    <Change OnEvent="txtEnName_Change">
                                    </Change>
                                </DirectEvents>
                            </ext:TextField>
                        </Items>
                    </ext:Panel>
                    <ext:Panel ID="Panel3" runat="server" Layout="FormLayout" ColumnWidth="0.25" Height="25"
                        LabelWidth="60">
                        <Items>
                            <ext:TextField ID="txtCnName" runat="server" FieldLabel="中文名稱" Width="120" TabIndex="3" ReadOnly="true">
                            </ext:TextField>
                        </Items>
                    </ext:Panel>
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
                                    <ext:RecordField Name="Building" />
                                    <ext:RecordField Name="FUNCTION_NAME" />
                                    <ext:RecordField Name="PARAME_NAME" />
                                    <ext:RecordField Name="PARAME_ITEM" />
                                    <ext:RecordField Name="PARAME_VALUE1" />
                                    <ext:RecordField Name="PARAME_VALUE2" />
                                    <ext:RecordField Name="PARAME_VALUE3" />
                                    <ext:RecordField Name="UPDATE_USER" />
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
                          <ext:Column DataIndex="Building" Header="Building" Width="100">
                        </ext:Column>
                        <ext:Column DataIndex="FUNCTION_NAME" Header="ID" Width="100" Hidden="true">
                        </ext:Column>
                        <ext:Column DataIndex="PARAME_NAME" Header="ID" Width="100" Hidden="true">
                        </ext:Column>
                        <ext:Column DataIndex="PARAME_ITEM" Header="ID" Width="100" Hidden="true">
                        </ext:Column>
                        <ext:Column DataIndex="PARAME_VALUE1" Header="责任部门" Width="100">
                        </ext:Column>
                        <ext:Column DataIndex="PARAME_VALUE2" Header="英文名称" Width="100">
                        </ext:Column>
                        <ext:Column DataIndex="PARAME_VALUE3" Header="中文名称" Width="100">
                        </ext:Column>
                        <ext:Column DataIndex="UPDATE_USER" Header="操作人" Width="100">
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
