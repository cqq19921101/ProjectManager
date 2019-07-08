<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrelaunchItem_Config.aspx.cs"
    Inherits="Web_E_Report_PrelaunchItem_Config" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="X-UA-Compatible" content="IE=8" />    <title></title>

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
    <ext:Panel ID="pnlMain" runat="server" Padding="0" Layout="Form" ButtonAlign="Center"
        Title="" Frame="True" AutoHeight="true">
        <Items>
            <ext:Panel ID="Panel8" runat="server" Padding="0" Layout="Form" Title="Prelaunch 項目維護"
                Frame="True">
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
                    <ext:Container ID="containerSingle" runat="server" Layout="ColumnLayout" Height="60">
                        <Items>
                            <ext:Container ID="Container2" runat="server" Layout="FormLayout" ColumnWidth="0.25"
                                LabelWidth="70">
                                <Items>
                                    <ext:ComboBox ID="cobDept" runat="server" FieldLabel="部門" Width="110" DisplayField="PARAME_VALUE2"
                                        ValueField="PARAME_VALUE2" TabIndex="1">
                                        <Store>
                                            <ext:Store ID="Store4" runat="server">
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
                                            <Select OnEvent="CmbDept_SelectedIndexChanged" />
                                        </DirectEvents>
                                    </ext:ComboBox>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container1" runat="server" Layout="FormLayout" ColumnWidth="0.3"
                                LabelWidth="70">
                                <Items>
                                    <ext:TextField ID="txtCheckItem" runat="server" FieldLabel="項目" Width="250">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container3" runat="server" Layout="FormLayout" ColumnWidth="0.3"
                                LabelWidth="80">
                                <Items>
                                    <ext:ComboBox ID="cmbAttachment" runat="server" FieldLabel="上傳附件否" Width="110" TabIndex="2">
                                        <Items>
                                            <ext:ListItem Text="Y" Value="Y" />
                                            <ext:ListItem Text="N" Value="N" />
                                        </Items>
                                        <SelectedItem Value="N" Text="N" />
                                    </ext:ComboBox>
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
                                    <ext:HyperLink ID="lkSample" runat="server" Text="模板下载" NavigateUrl="PreUploadTemp.xlsx" />
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                </Items>
            </ext:Panel>
            <ext:GridPanel ID="grdInfo" runat="server" Height="300" Frame="true" Title=" ITEMS LIST"
                Header="false">
                <Store>
                    <ext:Store ID="Store1" runat="server">
                        <Reader>
                            <ext:JsonReader>
                                <Fields>
                                    <ext:RecordField Name="ID" />
                                    <ext:RecordField Name="Dept" />
                                    <ext:RecordField Name="CheckItem" />
                                    <ext:RecordField Name="AttachmentFlag" />
                                    <ext:RecordField Name="UpdateUser" />
                                    <ext:RecordField Name="UpdateTime" Type="Date" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <ColumnModel>
                    <Columns>
                        <ext:Column DataIndex="Dept" Header="Dept" Width="80">
                        </ext:Column>
                        <ext:Column DataIndex="CheckItem" Header="CheckItem" Width="250">
                        </ext:Column>
                        <ext:Column DataIndex="AttachmentFlag" Header="AttachmentFlag" Width="100">
                        </ext:Column>
                        <ext:Column DataIndex="UpdateUser" Header="UpdatUser" Width="100">
                        </ext:Column>
                        <ext:DateColumn DataIndex="UpdateTime" Header="UpdateTime" Width="100" Format="yyyy/MM/dd HH:mm">
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
