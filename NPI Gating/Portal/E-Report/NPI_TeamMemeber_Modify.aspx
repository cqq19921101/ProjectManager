<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NPI_TeamMemeber_Modify.aspx.cs"
    Inherits="Web_E_Report_NPI_TeamMemeber_Modify" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
            <ext:Panel ID="pnlBegin" runat="server" Padding="0" Layout="Form" Title="" Border="false">
                <Items>
                    <ext:Panel ID="pnlApp" runat="server" Layout="Form" Title="" Frame="True" Border="false"
                        Collapsible="True">
                        <Items>
                            <ext:Container ID="Container6" runat="server" Layout="ColumnLayout" Height="30">
                                <Items>
                                    <ext:Container ID="Container1" runat="server" LabelWidth="70" Layout="Form" ColumnWidth="0.25">
                                        <Items>
                                            <ext:SelectBox ID="sbProd_group" runat="server" FieldLabel="產品類別" Width="120" IndicatorCls="red-text"
                                                IndicatorText="*">
                                                <Items>
                                                    <ext:ListItem Text="Charger" Value="Charger" />
                                                    <ext:ListItem Text="Adapter" Value="Adapter" />
                                                    <ext:ListItem Text="Apple" Value="Apple" />
                                                    <ext:ListItem Text="Others" Value="Others" />
                                                </Items>
                                            </ext:SelectBox>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container ID="Container4" runat="server" LabelWidth="70" Layout="Form" ColumnWidth="0.25">
                                        <Items>
                                            <ext:TextField ID="txtModel" runat="server" FieldLabel="機種" Width="120">
                                            </ext:TextField>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container ID="Container2" runat="server" LabelWidth="70" Layout="Form" ColumnWidth="0.25">
                                        <Items>
                                            <ext:TextField ID="txtApprove" runat="server" FieldLabel="簽核人員" Width="120">
                                            </ext:TextField>
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container61" runat="server" Layout="ColumnLayout" Height="30">
                                <Items>
                                    <ext:Container ID="Container11" runat="server" LabelWidth="70" Layout="Form" ColumnWidth="0.25">
                                        <Items>
                                            <ext:TextField ID="txtFormNo" runat="server" FieldLabel="表單編號" Width="120">
                                            </ext:TextField>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container ID="Container41" runat="server" LabelWidth="70" Layout="Form" ColumnWidth="0.25">
                                        <Items>
                                            <ext:DateField ID="txtApplyDate" runat="server" Width="120" FieldLabel="申請日期" Format="yyyy/MM/dd">
                                            </ext:DateField>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container ID="Container21" runat="server" LabelWidth="70" Layout="Form" ColumnWidth="0.25">
                                        <Items>
                                            <ext:Button ID="btnQuery" runat="server" Text="查詢" Icon="Zoom">
                                                <DirectEvents>
                                                    <Click OnEvent="btnQuery_click">
                                                    </Click>
                                                </DirectEvents>
                                            </ext:Button>
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Panel>
                    <ext:GridPanel ID="grdInfo" runat="server" Frame="true" Title="" AutoHeight="true"
                        Header="false">
                        <Store>
                            <ext:Store ID="Store1" runat="server">
                                <Reader>
                                    <ext:JsonReader>
                                        <Fields>
                                            <ext:RecordField Name="DOC_NO" />
                                            <ext:RecordField Name="ID" />
                                            <ext:RecordField Name="PROD_GROUP" />
                                            <ext:RecordField Name="PHASE" />
                                            <ext:RecordField Name="MODEL_NAME" />
                                            <ext:RecordField Name="CUSTOMER" />
                                            <ext:RecordField Name="APPLY_USERID" />
                                            <ext:RecordField Name="APPLY_DATE" Type="Date" />
                                            <ext:RecordField Name="NPI_PM" />
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
                                <ext:Column DataIndex="DOC_NO" Header="單據號碼" Width="150">
                                </ext:Column>
                                <ext:Column DataIndex="PROD_GROUP" Header="產品類別" Width="100">
                                </ext:Column>
                                <ext:Column DataIndex="MODEL_NAME" Header="機種名稱" Width="100">
                                </ext:Column>
                                <ext:Column DataIndex="CUSTOMER" Header="客户" Width="100">
                                </ext:Column>
                                <ext:Column Header="團隊類別" Width="120" DataIndex="Category">
                                </ext:Column>
                                <ext:Column Header="部門名稱" Width="100" DataIndex="DEPT">
                                </ext:Column>
                                <ext:Column Header="填寫人員" Width="150" DataIndex="WriteEname">
                                    <Editor>
                                        <ext:TextField ID="txt_WriteEname" runat="server">
                                        </ext:TextField>
                                    </Editor>
                                </ext:Column>
                                <ext:Column Header="回覆人員" Width="150" DataIndex="ReplyEName">
                                    <Editor>
                                        <ext:TextField ID="txt_ReplyEname" runat="server">
                                        </ext:TextField>
                                    </Editor>
                                </ext:Column>
                                <ext:Column Header="審核人員" Width="150" DataIndex="CheckedEName">
                                    <Editor>
                                        <ext:TextField ID="txt_CheckedEname" runat="server">
                                        </ext:TextField>
                                    </Editor>
                                </ext:Column>
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel ID="CheckboxSelectionModel" runat="server" SingleSelect="true">
                            </ext:RowSelectionModel>
                        </SelectionModel>
                             <DirectEvents>
                            <AfterEdit OnEvent="AfterEdit" Json="true">
                                <ExtraParams>
                                    <ext:Parameter Name="DOC_NO" Value="e.record.data.DOC_NO" Mode="Raw">
                                    </ext:Parameter>
                                     <ext:Parameter Name="ID" Value="e.record.data.ID" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="WriteEname" Value="e.record.data.WriteEname" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="ReplyEName" Value="e.record.data.ReplyEName" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="CheckedEName" Value="e.record.data.CheckedEName" Mode="Raw">
                                    </ext:Parameter>                                 
                                </ExtraParams>
                            </AfterEdit>
                        </DirectEvents>
                    </ext:GridPanel>
                </Items>
            </ext:Panel>
        </Items>
    </ext:Panel>
    </form>
</body>
</html>
