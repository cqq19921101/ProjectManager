<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QueryReport.aspx.cs" Inherits="QueryReport" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <title>Untitled Page</title>

    <script type="text/javascript">
     var template = '<span style="color:{0};">{1}</span>';
     var StatusFormat = function (value) 
     {
            return String.format(template, (value=='PASS') ? "green" : "red", value);
     };
    var ReportDetail = function(URL) 
    {
          window.open (URL, 'newwindow', 'height=100, width=400, top=0, left=0, toolbar=no, menubar=no, scrollbars=no, resizable=no,location=n o, status=no')
    };
    var NoteGoHid = function(grid, toolbar, rowIndex, record) 
    {
           if (record.get("Status") == 'Pending' || record.get("Status") == "Reject")
            {
               toolbar.items.itemAt(1).hide();
           }
     }       
    var ExportXLS = function(caseID,DocNo,Bu,Building) {
        var leftvalue = screen.availWidth - 10;
        var topvalue = screen.availHeight - 30;
        var sValue = 'width=400,height=400,top=' + (topvalue - 100) * 0.5 + ',left=' + (leftvalue - 300) * 0.5;
        var NewUrl = 'Report_Download.aspx?caseID='+caseID+'&DocNo='+DocNo+'&Bu='+Bu+'&Building='+Building;
        return window.open(NewUrl, '', sValue);
    }
        var NPIFileDownload = function(fileName,filePath,DocNo,Model) {
            var leftvalue = screen.availWidth - 10;
            var topvalue = screen.availHeight - 30;
            var sValue = 'width=400,height=200,top=' + (topvalue - 100) * 0.5 + ',left=' + (leftvalue - 300) * 0.5;
            var NewUrl = 'NPI_File_Download.aspx?filePath='+filePath +'&fileName='+fileName;
            return window.open(NewUrl, '', sValue);
        }

        var NPI_File_Download_PDF = function(fileName,filePath,DocNo,Model) {
            var leftvalue = screen.availWidth - 10;
            var topvalue = screen.availHeight - 30;
            var sValue = 'width=400,height=200,top=' + (topvalue - 100) * 0.5 + ',left=' + (leftvalue - 300) * 0.5;
            var NewUrl = 'NPI_File_Download_PDF.aspx?filePath='+filePath +'&fileName='+fileName;
            return window.open(NewUrl, '', sValue);
        }

    </script>

    <script type="text/javascript">
        var saveData = function() 
        {
            GridData.setValue(Ext.encode(grdList.getRowsValues({ selectedOnly: false })));
        }

    </script>
 

    <link rel="stylesheet" href="../Common.css" type="text/css" />
</head>
<body style="background-color: #DFE8F6;">
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" />
    <ext:Hidden ID="lblLogonId" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="lblBu" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="lblBuilding" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="GridData" runat="server" />
    <ext:Panel ID="pnlMain" runat="server" Padding="0" Layout="Form" Title="" Border="false">
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
                                        <DirectEvents>
                                            <Select OnEvent="cmbBuilding_Select">
                                            </Select>
                                        </DirectEvents>
                                    </ext:ComboBox>
                                </Items>
                            </ext:Container>

                            <ext:Container ID="Container2" runat="server" Layout="Form" ColumnWidth="0.2">
                                <Items>
                                    <ext:TextField ID="txtCustomer" runat="server" FieldLabel="客戶" Width="110" LabelSeparator="">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container4" runat="server" Layout="Form" ColumnWidth="0.2">
                                <Items>
                                    <ext:TextField ID="txtModel" runat="server" FieldLabel="機種" Width="110" LabelSeparator="">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container16" runat="server" Layout="Form" ColumnWidth="0.2">
                                <Items>
                                    <ext:ComboBox ID="cmbProduct" runat="server" FieldLabel="產品類別" Width="110" >
                                        <Items>
                                            <ext:ListItem Text="Adapter" Value="Adapter" />
                                            <ext:ListItem Text="Charger" Value="Charger" />
                                            <ext:ListItem Text="Apple" Value="Apple"/>
                                            <ext:ListItem Text="Other" Value="Other" />
                                        </Items>
                                    </ext:ComboBox>                                
                                    </Items>
                            </ext:Container>
                            <ext:Container ID="Container3" runat="server" Layout="Form" ColumnWidth="0.2">
                                <Items>
                                    <ext:TextField ID="txtApplyUser" runat="server" FieldLabel="申请人" Width="110" LabelSeparator="">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                    <ext:Container ID="Container1" runat="server" Layout="ColumnLayout" Height="30">
                        <Items>
                            <ext:Container ID="Container7" runat="server" Layout="Form" ColumnWidth="0.2">
                                <Items>
                                    <ext:DateField ID="dfBeginTime" runat="server" Width="110" FieldLabel="申請開始日期" LabelSeparator=" ">
                                    </ext:DateField>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container8" runat="server" Layout="Form" ColumnWidth="0.2">
                                <Items>
                                    <ext:DateField ID="dfEndTime" runat="server" Width="110" FieldLabel="申請結束日期" LabelSeparator=" ">
                                    </ext:DateField>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container10" runat="server" Layout="Form" ColumnWidth="0.2">
                                <Items>
                                    <ext:TextField ID="txtFormNo" runat="server" FieldLabel="表單編號" LabelSeparator=" "
                                        Width="110">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container9" runat="server" Layout="Form" ColumnWidth="0.2">
                                <Items>
                                    <ext:ComboBox ID="cmbStatus" runat="server" FieldLabel="單據狀態" LabelSeparator="" Width="110">
                                        <Items>
                                            <ext:ListItem Value="" Text="ALL" />
                                            <ext:ListItem Value="Pending" Text="Pending" />
                                            <ext:ListItem Value="Reject" Text="Reject" />
                                            <ext:ListItem Value="Finished" Text="Finished" />
                                            <ext:ListItem Value="Fail" Text="Fail" />
                                        </Items>
                                        <DirectEvents>
                                            <Select OnEvent="cmbStatus_Selected">
                                            </Select>
                                        </DirectEvents>
                                    </ext:ComboBox>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                    <ext:Container ID="Container15" runat="server" Layout="ColumnLayout" Height="30">
                        <Items>
                            <ext:Container ID="Container11" runat="server" Layout="Form" Height="30" ColumnWidth="0.2">
                                <Items>
                                    <ext:TextField ID="txtCaseidCount" runat="server" FieldLabel="案件數量" LabelSeparator=" "
                                        Width="110" ReadOnly="true">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container12" runat="server" Layout="Form" Height="30" ColumnWidth="0.2">
                                <Items>
                                    <ext:TextField ID="txtFinishedCount" runat="server" FieldLabel="Finished數量" LabelSeparator=" "
                                        Width="110" ReadOnly="true">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container13" runat="server" Layout="Form" Height="30" ColumnWidth="0.2">
                                <Items>
                                    <ext:TextField ID="txtFailCount" runat="server" FieldLabel="Fail數量" LabelSeparator=" "
                                        Width="110" ReadOnly="true">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container14" runat="server" Layout="Form" Height="30" ColumnWidth="0.2">
                                <Items>
                                    <ext:TextField ID="txtPendingCount" runat="server" FieldLabel="Pening數量" LabelSeparator=" "
                                        Width="110" ReadOnly="true">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container153" runat="server" Layout="Form" Height="30" ColumnWidth="0.2">
                                <Items>
                                    <ext:TextField ID="txtRejectCount" runat="server" FieldLabel="Reject數量" LabelSeparator=" "
                                        Width="110" ReadOnly="true">
                                    </ext:TextField>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                </Items>
            </ext:Panel>
            <ext:GridPanel ID="grdList" runat="server" Height="440" Frame="true" Title="list"
                Header="false">
                <Store>
                    <ext:Store ID="Store1" runat="server">
                        <Reader>
                            <ext:JsonReader>
                                <Fields>
                                    <ext:RecordField Name="CASEID" />
                                    <ext:RecordField Name="PDF_FLAG" />
                                    <ext:RecordField Name="Date"/>
                                    <ext:RecordField Name="SUB_DOC_NO" />
                                    <ext:RecordField Name="BU" />
                                    <ext:RecordField Name="BUILDING" />
                                    <ext:RecordField Name="CREATE_DATE" />
                                    <ext:RecordField Name="CUSTOMER" />
                                    <ext:RecordField Name="NPI_PM" />
                                    <ext:RecordField Name="PROD_GROUP" />
                                    <ext:RecordField Name="MODEL_NAME" />
                                    <ext:RecordField Name="STATUS" />
                                    <ext:RecordField Name="handler" />                                    
                                    <ext:RecordField Name="Result" />                                    
                                    
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <ColumnModel>
                    <Columns>
                        <ext:CommandColumn Width="220">
                            <Commands>
                                <ext:GridCommand Icon="Vcard" CommandName="DetailInfo" Text="查看">
                                </ext:GridCommand>
                                <ext:GridCommand Icon="Vcard" Hidden="false" CommandName="ExportXLS" Text="生成">
                                </ext:GridCommand>
                               <ext:GridCommand Icon="Vcard" Hidden="false" CommandName="DownLoadXLS" Text="Excel">
                                </ext:GridCommand>
                                <ext:GridCommand Icon="Vcard" Hidden="false" CommandName="DownLoadPDF" Text="PDF">
                                </ext:GridCommand>
                            </Commands>
                             <PrepareToolbar Fn="NoteGoHid" />
                        </ext:CommandColumn>
                        <ext:Column DataIndex="CASEID" Header="案件編號" Width="60">
                        </ext:Column>
                        <ext:Column DataIndex="Date"  Header="申請日期" Width="90" >
                        </ext:Column>
                        <ext:Column DataIndex="SUB_DOC_NO" Header="表單單號" Width="120">
                        </ext:Column>
                        <ext:Column DataIndex="BU" Header="BU" Width="60">
                        </ext:Column>
                        <ext:Column DataIndex="BUILDING" Header="BUILDING" Width="50">
                        </ext:Column>
                        <ext:Column DataIndex="NPI_PM" Header="開單人" Width="100">
                        </ext:Column>
                        <ext:Column DataIndex="PROD_GROUP" Header="產品類別" Width="80">
                        </ext:Column>
                        <ext:Column DataIndex="MODEL_NAME" Header="機種" Width="100">
                        </ext:Column>
                        <ext:Column DataIndex="STATUS" Header="單據狀態" Width="80">
                            <%--<Renderer Fn="StatusFormat" />--%>
                        </ext:Column>                        
                        <ext:Column DataIndex="Result" Header="簽核狀態" Width="110">
                            <Renderer Fn="StatusFormat" />
                        </ext:Column>
                        <ext:Column DataIndex="PDF_FLAG" Header="是否已生成PDF" Width="100">
                        </ext:Column>
                        <ext:Column DataIndex="handler" Header="待簽核人" Width="250">
                        </ext:Column>
                    </Columns>
                </ColumnModel>
                <SelectionModel>
                    <ext:RowSelectionModel ID="CheckboxSelectionModel" runat="server">
                    </ext:RowSelectionModel>
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
                            <ext:Button ID="btnToExcel" runat="server" Text="導出Excel" AutoPostBack="true" OnClick="btnToExcel_Click"
                                Icon="PageExcel">
                                <Listeners>
                                    <Click Fn="saveData" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <BottomBar>
                    <ext:PagingToolbar ID="PagingToolbar1" PageSize="14" runat="server">
                    </ext:PagingToolbar>
                </BottomBar>
                <DirectEvents>
                    <Command OnEvent="DOCProcess_Look_Click">
                        <ExtraParams>
                            <ext:Parameter Name="CASEID" Value="record.data.CASEID" Mode="Raw">
                            </ext:Parameter>
                            <ext:Parameter Name="MODEL_NAME" Value="record.data.MODEL_NAME" Mode="Raw">
                            </ext:Parameter>
                            <ext:Parameter Name="SUB_DOC_NO" Value="record.data.SUB_DOC_NO" Mode="Raw">
                            </ext:Parameter>
                            <ext:Parameter Name="BU" Value="record.data.BU" Mode="Raw">
                            </ext:Parameter>
                            <ext:Parameter Name="BUILDING" Value="record.data.BUILDING" Mode="Raw">
                            </ext:Parameter>
                            <ext:Parameter Name="command" Value="command" Mode="Raw">
                            </ext:Parameter>
                        </ExtraParams>
                    </Command>
                </DirectEvents>
                <View>
                    <ext:GridView  runat="server" StripeRows="true" TrackOver="true" />
                </View>
            </ext:GridPanel>

            </Items>
    </ext:Panel>
    </form>
</body>
</html>
