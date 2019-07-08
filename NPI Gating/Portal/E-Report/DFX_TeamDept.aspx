<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DFX_TeamDept.aspx.cs" Inherits="Web_E_Report_DFX_TeamDept" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="X-UA-Compatible" content="IE=8" />    <title></title>
</head>
<body style="background-color: #DFE8F6;">
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" />
    <ext:Hidden ID="lblLogonId" runat="server" Text="">
    </ext:Hidden>
    <ext:Hidden ID="lblSite" runat="server" />
    <ext:Hidden ID="lblBu" runat="server" Text="">
    </ext:Hidden>
      <ext:Hidden ID="lblBuilding" runat="server">
    </ext:Hidden>
    <ext:Panel ID="pnlMain" runat="server" Padding="0" Layout="Form" ButtonAlign="Center"
        Title="" Frame="True" AutoHeight="true">
        <Items>
            <ext:Panel ID="pnlApp" runat="server" Layout="Form"  AutoHeight="true" Border="false">
                <Items>
                    <ext:Container ID="Container1" runat="server" Layout="ColumnLayout" Height="90">
                        <Items>
                            <ext:Container ID="Container2" runat="server" LabelWidth="70" Layout="Form" ColumnWidth="0.25">
                                <Items>
                                 
                                    <ext:ComboBox ID="cobRule" runat="server" FieldLabel="權責" Width="120" TabIndex="2">
                                        <Items>
                                            <ext:ListItem Text="填寫" Value="W" />
                                            <ext:ListItem Text="回覆" Value="R" />
                                            <ext:ListItem Text="填寫&回覆" Value="W&R" />
                                        </Items>
                                    </ext:ComboBox>
                                     <ext:TextField ID="txtHandler" runat="server" FieldLabel="主管" Width="120" TabIndex="4">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container3" runat="server" LabelWidth="70" Layout="Form" ColumnWidth="0.25">
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
                                    <%--   <DirectEvents>
                                            <Change OnEvent="sbSelected_change">
                                            </Change>
                                        </DirectEvents>--%>
                                    </ext:ComboBox>
                                     <ext:TextField ID="txtBuilding" runat="server" FieldLabel="Building" Width="120" TabIndex="1">
                                    </ext:TextField>
                                   
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container4" LabelWidth="70" runat="server" Layout="Form" ColumnWidth="0.25">
                                <Items>
                                    <ext:TextField ID="txtDeptCode" runat="server" FieldLabel="部門編碼" Width="120" TabIndex="3">
                                    </ext:TextField>
                                      
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                </Items>
            </ext:Panel>
            <ext:GridPanel ID="grdInfo" runat="server" Height="440" Frame="true" Title="团队部门列表"
                Header="false">
                <Store>
                    <ext:Store ID="Store1" runat="server">
                        <Reader>
                            <ext:JsonReader>
                                <Fields>
                                    <ext:RecordField Name="ID" />
                                    <ext:RecordField Name="FUNCTION_NAME" />
                                    <ext:RecordField Name="PARAME_NAME" />
                                    <ext:RecordField Name="PARAME_ITEM" />
                                    <ext:RecordField Name="PARAME_VALUE1" />
                                    <ext:RecordField Name="PARAME_VALUE2" />
                                    <ext:RecordField Name="PARAME_VALUE3" />
                                    <ext:RecordField Name="PARAME_VALUE4" />
                                    <ext:RecordField Name="PARAME_VALUE5" />
                                    <ext:RecordField Name="Building" />
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
                        <ext:Column DataIndex="PARAME_VALUE1" Header="部门" Width="100">
                        </ext:Column>
                        <ext:Column DataIndex="PARAME_VALUE2" Header="部门編碼" Width="80">
                        </ext:Column>
                        <ext:Column DataIndex="PARAME_VALUE3" Header="主管" Width="100">
                        </ext:Column>
                        <ext:Column DataIndex="PARAME_VALUE4" Header="權責代碼" Width="100" Hidden="true">
                        </ext:Column>
                        <ext:Column DataIndex="PARAME_VALUE5" Header="權責" Width="100">
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
