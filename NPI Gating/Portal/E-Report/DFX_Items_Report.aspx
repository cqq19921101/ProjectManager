<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DFX_Items_Report.aspx.cs"
    Inherits="Web_DFX_DFX_Items_Update" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="X-UA-Compatible" content="IE=8" />    <title></title>

    <script type="text/javascript">
           var saveData = function() {
            GridRowData.setValue(Ext.encode(grdInfo.getRowsValues({ selectedOnly: false })));
        }
    </script>

</head>
<body style="background-color: #DFE8F6;">
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" />
    <ext:Hidden ID="lblLogonId" runat="server" Text="">
    </ext:Hidden>
    <ext:Panel ID="pnlMain" runat="server" Padding="0" Layout="Form" ButtonAlign="Center"
        Title="" Frame="True" AutoHeight="true">
        <Items>
            <ext:Hidden ID="GridRowData" runat="server" />
            <ext:GridPanel ID="grdInfo" runat="server" Height="500" Frame="true" Title="DFX ITEMS LIST"
                Header="false">
                <Store>
                    <ext:Store ID="Store1" runat="server">
                        <Reader>
                            <ext:JsonReader>
                                <Fields>
                                    <ext:RecordField Name="id" />
                                    <ext:RecordField Name="Item" />
                                    <ext:RecordField Name="Requirements" />
                                    <ext:RecordField Name="Severity" Type="Int" />
                                    <ext:RecordField Name="Occurrence" Type="Int" />
                                    <ext:RecordField Name="Detection" Type="Int" />
                                    <ext:RecordField Name="RPN" Type="Int" />
                                    <ext:RecordField Name="PriorityLevel" Type="Int" />
                                    <ext:RecordField Name="MaxPoints" Type="Int" />
                                    <ext:RecordField Name="Losses" />
                                    <ext:RecordField Name="WriteDept" />
                                    <ext:RecordField Name="ReplyDept" />
                                    <ext:RecordField Name="UpdateUser" />
                                    <ext:RecordField Name="UpdateTime" Type="Date" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <ColumnModel>
                    <Columns>
                        <ext:Column DataIndex="Item" Header="項目編號" Width="80">
                        </ext:Column>
                        <ext:Column DataIndex="WriteDept" Header="填寫部門" Width="70">
                        </ext:Column>
                        <ext:Column DataIndex="ReplyDept" Header="回覆部門" Width="70">
                        </ext:Column>
                        <ext:Column DataIndex="Requirements" Header="Requirements" Width="200">
                        </ext:Column>
                        <ext:NumberColumn DataIndex="Severity" Header="Severity" Width="50">
                        </ext:NumberColumn>
                        <ext:NumberColumn DataIndex="Occurrence" Header="Occurrence" Width="70">
                        </ext:NumberColumn>
                        <ext:NumberColumn DataIndex="Detection" Header="Detection" Width="60">
                        </ext:NumberColumn>
                        <ext:NumberColumn DataIndex="RPN" Header="RPN" Width="50">
                        </ext:NumberColumn>
                        <ext:Column DataIndex="PriorityLevel" Header="PriorityLevel" Width="100">
                        </ext:Column>
                        <ext:Column DataIndex="MaxPoints" Header="MaxPoints" Width="100">
                        </ext:Column>
                        <ext:Column DataIndex="Losses" Header="違反損失" Width="200">
                        </ext:Column>
                        <ext:Column DataIndex="UpdateUser" Header="操作人" Width="100">
                        </ext:Column>
                        <ext:DateColumn DataIndex="UpdateTime" Header="更新时间" Width="100" Format="yyyy/MM/dd HH:mm">
                        </ext:DateColumn>
                    </Columns>
                </ColumnModel>
                <SelectionModel>
                    <ext:RowSelectionModel runat="server">
                    </ext:RowSelectionModel>
                </SelectionModel>
                <TopBar>
                    <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <ext:Button runat="server" Text="To Excel" Icon="PageExcel" ID="btnExport" OnClick="btnExport_click"
                                AutoPostBack="true">
                                <Listeners>
                                    <Click Fn="saveData" />
                                </Listeners>
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
