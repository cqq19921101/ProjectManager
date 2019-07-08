<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NPI_MemberConfig.aspx.cs"
    Inherits="NPI_MemberConfig" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="X-UA-Compatible" content="IE=8" />    <title>Untitled Page</title>
    <link rel="stylesheet" href="../Common.css" type="text/css" />
</head>
<body style="background-color: #DFE8F6;">
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" />
    <ext:Hidden ID="lblLogonId" runat="server" Text=" ">
    </ext:Hidden>
      <ext:Hidden ID="lblSite" runat="server" Text=" ">
    </ext:Hidden>
      <ext:Hidden ID="lblBu" runat="server" Text=" ">
    </ext:Hidden>
      <ext:Hidden ID="lblBuilding" runat="server" Text=" ">
    </ext:Hidden>
    <ext:Panel ID="pnlMain" runat="server" Padding="0" Layout="Form" Title="" Border="false">
        <Items>
            <ext:Panel ID="pnlApp" runat="server" Layout="Form" Title="人员维护" Frame="True" Border="false">
                <Items>
                    <ext:Container ID="Container6" runat="server" Layout="ColumnLayout" Height="30">
                        <Items>
                            <ext:Container ID="Container1" runat="server" LabelWidth="70" Layout="Form" ColumnWidth="0.2">
                                <Items>
                                    <ext:SelectBox ID="sbCategory" runat="server" FieldLabel="類別" Width="120" IndicatorCls="red-text"
                                        IndicatorText="*">
                                        <Items>
                                            <ext:ListItem Text="填寫" Value="WINDOW" />
                                            <ext:ListItem Text="回覆" Value="SUPERVISOR" />
                                            <ext:ListItem Text="審核" Value="MANAGER" />
                                        </Items>
                                        <DirectEvents>
                                            <Change OnEvent="sbCategory_selectedChange">
                                            </Change>
                                        </DirectEvents>
                                    </ext:SelectBox>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container4" runat="server" LabelWidth="70" Layout="Form" ColumnWidth="0.2">
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
                                            <Select OnEvent="cobDept_Select">
                                            </Select>
                                        </DirectEvents>
                                    </ext:ComboBox>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container2" runat="server" LabelWidth="70" Layout="Form" ColumnWidth="0.2">
                                <Items>
                                    <ext:TextField ID="txtEName" runat="server" FieldLabel="英文名稱" Width="120" IndicatorCls="red-text"
                                        IndicatorText="*">
                                        <DirectEvents>
                                            <Change OnEvent="txtEName_change">
                                            </Change>
                                        </DirectEvents>
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container3" runat="server" LabelWidth="70" Layout="Form" ColumnWidth="0.2">
                                <Items>
                                    <ext:TextField ID="txtCName" runat="server" FieldLabel="中文名稱" Width="120" ReadOnly="true">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container5" LabelWidth="70" runat="server" Layout="Form" ColumnWidth="0.2">
                                <Items>
                                    <ext:TextField ID="txtEMail" runat="server" FieldLabel="E-MAIL" Width="120" ReadOnly="true">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                    <ext:Container ID="Container16" runat="server" Layout="Form">
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
            </ext:Panel>
            <ext:GridPanel ID="grdInfo" runat="server" Height="440" Frame="true" Title="成員列表"
                Header="false">
                <Store>
                    <ext:Store ID="Store1" runat="server">
                        <Reader>
                            <ext:JsonReader>
                                <Fields>
                                    <ext:RecordField Name="ID" />
                                    <ext:RecordField Name="CATEGORY" />
                                    <ext:RecordField Name="DEPT" />
                                    <ext:RecordField Name="ENAME" />
                                    <ext:RecordField Name="CNAME" />
                                    <ext:RecordField Name="EMAIL" />
                                    <ext:RecordField Name="UPDATE_USERID" />
                                    <ext:RecordField Name="UPDATE_TIME" Type="Date" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <ColumnModel>
                    <Columns>
                        <ext:Column DataIndex="CATEGORY" Header="類別" Width="100">
                        </ext:Column>
                        <ext:Column DataIndex="DEPT" Header="责任部门" Width="100">
                        </ext:Column>
                        <ext:Column DataIndex="ENAME" Header="英文名称" Width="100">
                        </ext:Column>
                        <ext:Column DataIndex="CNAME" Header="中文名称" Width="100">
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
                                        <Confirmation ConfirmRequest="true" Message="確認刪除勾選的成員記錄?" Title="提示" />
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
