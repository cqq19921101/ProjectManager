<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DFX_Items_Maintain.aspx.cs"
    Inherits="Web_E_Report_DFX_Items_Maintain" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <title></title>

    <script type="text/javascript">
 

    //針對attchment中URL轉化為Hyperlink
          var changeURL = function(value) 
          {
           if(value==null)
           {
             return value;
           }
           else
           {
                var index = value.lastIndexOf("/");
                if (index > -1) {
                    return '<a href="' + value + '" target="_blank">' + value.substr(index + 1, value.length - index - 1) + '</a>';
                }
                else {
                    return value;
                }
            }
        };
           
    </script>

</head>
<body style="background-color: #DFE8F6;">
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" />
    <ext:Hidden ID="lblSite" runat="server" Text="">
    </ext:Hidden>
    <ext:Hidden ID="lblLogonId" runat="server" Text="">
    </ext:Hidden>
    <ext:Hidden ID="lblBu" runat="server" Text="">
    </ext:Hidden>
    <ext:Hidden ID="lblBuilding" runat="server" Text="">
    </ext:Hidden>
    <ext:Hidden ID="hidId" runat="server" Text="">
    </ext:Hidden>
    <ext:Panel ID="pnlMain" runat="server" Padding="0" Layout="Form" ButtonAlign="Center"
        Title="" Frame="True" AutoHeight="true">
        <Items>
            <ext:Panel ID="Panel8" runat="server" Padding="0" Layout="Form" Title="DFX 項目維護"
                Frame="True">
                <Items>
                    <ext:Container ID="Container8" runat="server" Layout="ColumnLayout" Height="35">
                        <Items>
                            <ext:Container ID="Container2" runat="server" Layout="FormLayout" ColumnWidth="0.2"
                                LabelWidth="70">
                                <Items>
                                    <ext:ComboBox ID="cmbProduct" runat="server" FieldLabel="產品類別" Width="110" TabIndex="2">
                                        <Items>
                                            <ext:ListItem Text="Adapter" Value="Adapter" />
                                            <ext:ListItem Text="Charger" Value="Charger" />
                                            <ext:ListItem Text="Apple" Value="Apple" />
                                            <ext:ListItem Text="Others" Value="Others" />
                                            <ext:ListItem Text="DT" Value="DT" />
                                        </Items>
                                        <DirectEvents>
                                            <Select OnEvent="BindgrdInfoO">
                                            </Select>
                                        </DirectEvents>
                                    </ext:ComboBox>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container9" runat="server" Layout="FormLayout" ColumnWidth="0.2"
                                LabelWidth="70">
                                <Items>
                                    <ext:ComboBox ID="cmbDFXType" runat="server" FieldLabel="DFX版本" Width="110">
                                        <Items>
                                            <ext:ListItem Text="A" Value="A" />
                                            <ext:ListItem Text="B" Value="B" />
                                            <ext:ListItem Text="C" Value="C" />
                                            <ext:ListItem Text="D" Value="D" />
                                            <ext:ListItem Text="Q" Value="Q" />
                                            <ext:ListItem Text="R" Value="R" />
                                            <ext:ListItem Text="S" Value="S" />
                                        </Items>
                                        <DirectEvents>
                                            <Select OnEvent="BindgrdInfoT">
                                            </Select>
                                        </DirectEvents>
                                    </ext:ComboBox>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                    <ext:Container ID="Container7" runat="server" Layout="ColumnLayout" Height="35">
                        <Items>
                            <ext:Container ID="Container1" runat="server" Layout="FormLayout" ColumnWidth="0.2"
                                LabelWidth="70">
                                <Items>
                                    <ext:FileUploadField ID="FileUpload" runat="server" FieldLabel="文件上傳" LabelSeparator=" "
                                        ButtonText="請選擇文件" Width="200">
                                    </ext:FileUploadField>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container4" runat="server" Layout="FormLayout" ColumnWidth="0.05"
                                LabelWidth="120">
                                <Items>
                                    <ext:Button ID="Button1" runat="server" Text="上傳" Icon="Accept">
                                        <DirectEvents>
                                            <Click OnEvent="btnUpload_Click">
                                                <EventMask ShowMask="true" Msg="處理中..." MinDelay="500" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container10" runat="server" Layout="FormLayout" ColumnWidth="0.33"
                                LabelWidth="120">
                                <Items>
                                    <ext:HyperLink ID="lkSample" runat="server" Text="模板下载" NavigateUrl="DFX Upload Temp.xlsx" />
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                    <ext:Container ID="Container18" runat="server" Layout="ColumnLayout" Height="90">
                        <Items>
                            <ext:Container ID="Container20" runat="server" Layout="FormLayout" LabelWidth="70">
                                <Items>
                                    <ext:TextArea ID="txtRequirements" runat="server" LabelSeparator="" FieldLabel="變更原因"
                                        Width="500" Height="80">
                                    </ext:TextArea>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                    <ext:Container ID="Container21" runat="server" Layout="ColumnLayout" Height="90">
                        <Items>
                            <ext:Container ID="Container22" runat="server" Layout="FormLayout" LabelWidth="70">
                                <Items>
                                    <ext:TextArea ID="txtVersionLog" runat="server" LabelSeparator="" FieldLabel="變更記錄"
                                        Width="500" Height="80" ReadOnly ="True">
                                    </ext:TextArea>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                                        <ext:Container ID="Container3" runat="server" Layout="ColumnLayout" Height="50">
                        <Items>
                            <ext:Container ID="Container5" runat="server" Layout="FormLayout" LabelWidth="70"
                                ColumnWidth="0.2">
                                <Items>
                                    <ext:ComboBox ID="cobDept" runat="server" FieldLabel="部門" Width="110" DisplayField="PARAME_VALUE1"
                                        ValueField="PARAME_VALUE2" TabIndex="1">
                                        <Store>
                                            <ext:Store ID="Store4" runat="server">
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
                                            <Select OnEvent="BindgrdInfo">
                                            </Select>
                                        </DirectEvents>
                                    </ext:ComboBox>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container6" runat="server" Layout="FormLayout" LabelWidth="70"
                                ColumnWidth="0.2">
                                <Items>
                                    <ext:TextField ID="txtVersion" runat="server" FieldLabel="當前版本號" Width="110" Disabled="true">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                    <ext:Container ID="ConPicture" runat="server" Layout="ColumnLayout" Height="30" Hidden="false">
                        <Items>
                            <ext:Container ID="Container14" runat="server" Layout="FormLayout" LabelWidth="80">
                                <Items>
                                    <ext:TextField ID="txtID" runat="server" FieldLabel="ID" Width="110" Disabled="true"
                                        Hidden="true">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container15" runat="server" Layout="FormLayout" LabelWidth="70">
                                <Items>
                                    <ext:TextField ID="txtItem" runat="server" FieldLabel="Item" Width="110" ReadOnly ="true">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                    <ext:Container ID="Container11" runat="server" Layout="ColumnLayout" Height="30">
                        <Items>
                            <ext:Container ID="Container12" runat="server" Layout="FormLayout" LabelWidth="70">
                                <Items>
                                    <ext:FileUploadField ID="UploadFile" runat="server" EmptyText="Select an image" FieldLabel="圖片上傳"
                                        ButtonText="" Icon="ImageAdd" Width="600">
                                    </ext:FileUploadField>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container17" runat="server" Layout="FormLayout" LabelWidth="80">
                                <Items>

                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                    <ext:Container ID="Container13" runat="server" Layout="ColumnLayout" Height="30">
                        <Items>
                            <ext:Container ID="Container16" runat="server" Layout="FormLayout" LabelWidth="80" ColumnWidth="0.1">
                                <Items>
                                    <ext:Button ID="Button3" runat="server" Text="圖片上傳" Icon = "Accept">
                                        <DirectEvents>
                                            <Click OnEvent="btnUploadP_Click">
                                                <EventMask ShowMask="true" Msg="處理中..." MinDelay="500" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>                                

                                </Items>
                            </ext:Container>
                       <ext:Container ID="Container19" runat="server" Layout="FormLayout" LabelWidth="80" ColumnWidth="0.1">
                                <Items>
                                    <ext:Button ID="Button2" runat="server" Text="圖片移除" Icon="Delete">
                                        <DirectEvents>
                                            <Click OnEvent="btnDeleteP_Click">
                                            <Confirmation ConfirmRequest="true" Message="確認刪除此項記錄?" Title="提示" />
                                                <EventMask ShowMask="true" Msg="處理中..." MinDelay="500" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>                                
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                </Items>
            </ext:Panel>
            <ext:GridPanel ID="grdInfo" runat="server" Height="300" Frame="true" Title="DFX ITEMS LIST"
                Header="false">
                <Store>
                    <ext:Store ID="Store1" runat="server">
                        <Reader>
                            <ext:JsonReader>
                                <Fields>
                                    <ext:RecordField Name="ID" />
                                    <ext:RecordField Name="Item" />
                                    <ext:RecordField Name="Requirements" />
                                    <ext:RecordField Name="ItemType" />
                                    <ext:RecordField Name="ProductType" />
                                    <ext:RecordField Name="ItemName" />
                                    <ext:RecordField Name="PriorityLevel" Type="Int" />
                                    <ext:RecordField Name="Losses" />
                                    <ext:RecordField Name="WriteDept" />
                                    <ext:RecordField Name="ReplyDept" />
                                    <ext:RecordField Name="FILE_PATH" />
                                    <ext:RecordField Name="ItemID" />
                                    <ext:RecordField Name="OldItemType" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <ColumnModel>
                    <Columns>
                        <ext:ImageCommandColumn Width="60">
                            <Commands>
                                <ext:ImageCommand Icon="FolderUp" CommandName="UpLoad" Text="編輯">
                                    <ToolTip Text="編輯" />
                                </ext:ImageCommand>
                            </Commands>
                        </ext:ImageCommandColumn>
                        <ext:Column DataIndex="WriteDept" Header="填寫部門" Width="60" Sortable="false">
                        </ext:Column>
                        <ext:Column DataIndex="OldItemType" Header="分類" Width="180" Sortable="false">
                        </ext:Column>
                        <ext:Column DataIndex="ItemID" Header="條款序號" Width="80" Sortable="false">
                        </ext:Column>
                        <ext:Column DataIndex="Item" Header="編碼" Width="120" Sortable="false">
                        </ext:Column>
                        <ext:Column DataIndex="ItemType" Header="DFX類別" Width="90" Sortable="false">
                        </ext:Column>
                        <ext:Column DataIndex="ItemName" Header="DFX項目" Width="80" Sortable="false">
                        </ext:Column>
                        <ext:Column DataIndex="Requirements" Header="Requirements" Width="250" Sortable="false">
                        </ext:Column>
                        <ext:Column DataIndex="FILE_PATH" Header="圖片" Width="80" Sortable="false">
                            <Renderer Fn="changeURL" />
                        </ext:Column>
                        <ext:Column DataIndex="Compliance" Header="Compliance" Width="80" Sortable="false">
                        </ext:Column>
                        <ext:Column DataIndex="PriorityLevel" Header="PriorityLevel" Width="80" Sortable="false">
                        </ext:Column>
                        <ext:Column DataIndex="Max Points Avaliable" Header="Max Points Avaliable" Width="80"
                            Sortable="false">
                        </ext:Column>
                        <ext:Column DataIndex="Max Points Avaliable" Header="Max Points Avaliable" Width="80"
                            Sortable="false">
                        </ext:Column>
                        <ext:Column DataIndex="Comments" Header="Comments" Width="80" Sortable="false">
                        </ext:Column>
                        <ext:Column DataIndex="Losses" Header="違反損失" Width="150" Sortable="false">
                        </ext:Column>
                        <ext:Column DataIndex="ReplyDept" Header="回覆部門" Width="70" Sortable="false">
                        </ext:Column>
                    </Columns>
                </ColumnModel>
                <SelectionModel>
                    <ext:CheckboxSelectionModel ID="CheckboxSelectionModel" runat="server">
                    </ext:CheckboxSelectionModel>
                </SelectionModel>
                <DirectEvents>
                    <Command OnEvent="grdInfo_RowCommand">
                        <ExtraParams>
                            <ext:Parameter Name="ID" Value="record.data.ID" Mode="Raw" />
                            <ext:Parameter Name="Item" Value="record.data.Item" Mode="Raw" />
                            <ext:Parameter Name="command" Value="record.data.command" Mode="Raw" />
                        </ExtraParams>
                    </Command>
                </DirectEvents>
            </ext:GridPanel>
        </Items>
    </ext:Panel>
    </form>
</body>
</html>
