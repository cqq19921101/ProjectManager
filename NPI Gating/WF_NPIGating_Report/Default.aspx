<%@ Page Title="" Language="C#" MasterPageFile="~/Master/SpmMaster.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">

    <script type="text/javascript">
        
     var template = '<span style="color:{0};">{1}</span>';
     var StatusFormat = function (value) 
     {
            return String.format(template, (value=='Finished') ? "green" : "red", value);
     };
   var Result = function (value) {
            return String.format(template, (value=='PASS') ? "green" : "red", value);
        };
  
    var NoteGoHid = function(grid, toolbar, rowIndex, record) 
    {
         var  StepValue=document.getElementById("<%=lblStepName.ClientID%>").value;
         var  StepValue1=document.getElementById("<%=logonid.ClientID%>").value;
        if(StepValue1 == '3')
        {
               toolbar.items.itemAt(0).hide();
               toolbar.items.itemAt(1).hide();
        }
        else
        {
           if (StepValue=='PM'  || StepValue=='ReplyChecked' || StepValue=='Dept.Reply')
            {
               toolbar.items.itemAt(0).hide();
               toolbar.items.itemAt(1).hide();
           }
            else if (StepValue=='Dept.Write' || StepValue=='WriteChecked')
            {
               toolbar.items.itemAt(1).hide();
            }
        }
     } ;  
     
    
    var NoteGoHid3 = function(grid, toolbar, rowIndex, record) 
    {
         var  StepValue=document.getElementById("<%=lblStepName.ClientID%>").value;
         var  StepValue1=document.getElementById("<%=logonid.ClientID%>").value;
        if(StepValue1 == '3')
        {
               toolbar.items.itemAt(0).hide();
               toolbar.items.itemAt(1).hide();
        }
        else
        {
           if (StepValue=='PM'  )
            {
               toolbar.items.itemAt(0).hide();
               toolbar.items.itemAt(1).hide();
           }
            if (StepValue=='Dept.Write' || StepValue=='WriteChecked'  )
            {
               toolbar.items.itemAt(1).hide();
            }
            if (StepValue=='Dept.Reply' || StepValue=='ReplyChecked')
            {
               toolbar.items.itemAt(0).hide();
           }
        }
            
     } ; 
     
         var NoteGoHidPR = function(grid, toolbar, rowIndex, record) 
    {
         var  StepValue=document.getElementById("<%=lblStepName.ClientID%>").value;
         var  StepValue1=document.getElementById("<%=logonid.ClientID%>").value;
        if(StepValue1 == '3')
        {
               toolbar.items.itemAt(0).hide();
        }
        else
        {
           if (StepValue !='Dept.Checked')
            {
               toolbar.items.itemAt(0).hide();
           }
        }
     } ;  
     
     
     //issuelist and FMEAList      
      var NoteGoHid2 = function(grid, toolbar, rowIndex, record) 
    {
         var  StepValue=document.getElementById("<%=lblStepName.ClientID%>").value;
         var  StepValue1=document.getElementById("<%=logonid.ClientID%>").value;
        if(StepValue1 == '3')
        {
               toolbar.items.itemAt(0).hide();
        }
        else
        {
           if (StepValue=='PM')
            {
               toolbar.items.itemAt(0).hide();
           }
            if (StepValue=='Dept.Write' || StepValue=='WriteChecked'  )
            {
               toolbar.items.itemAt(0).hide();
            }
        }
            
     } ; 
       var beforeEdit = function(e)
      {
         var Approver=e.record.data.APPROVER_OPINION; 

         var  StepValue=document.getElementById("<%=lblStepName.ClientID%>").value;
         if (StepValue=='Dept.Checked')
          {
              e.cancel=false; 
          }        

         else
         {
            e.cancel=true;
           alert("無權限編輯此行!");

         }     
      };
 
    </script>

    <script type="text/javascript">
 

    //針對attchment中URL轉化為Hyperlink
    var change_Attachement = function(value)  
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

    <script type="text/javascript">
 

    //針對attchment中URL轉化為Hyperlink
    var change_Attachement_Issue = function(value)  
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

    <script type="text/javascript">
        
         var saveData = function() {
            GridData.setValue();
   };
    </script>

    <script type="text/javascript">
        
            var NPIFileDownload = function(fileName,filePath) {
            var leftvalue = screen.availWidth - 10;
            var topvalue = screen.availHeight - 30;
            var sValue = 'width=400,height=200,top=' + (topvalue - 100) * 0.5 + ',left=' + (leftvalue - 300) * 0.5;
            var NewUrl = 'NPI_File_Download.aspx?filePath='+filePath+'&fileName='+fileName;
            return window.open(NewUrl, '', sValue);
   };
    </script>

    <ext:ResourceManager ID="ResourceManager1" runat="server" />
    <ext:Hidden ID="lblLogonId" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="logonid" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="lblSite" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="lblBu" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="lblBuilding" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="lblStepName" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="lblhyperlink" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="GridData" runat="server" />
    <ext:Window ID="winUpload" runat="server" Header="true" Title="批量上傳" Width="300"
        Hidden="true" Padding="5">
        <Items>
            <ext:Container ID="Container91" runat="server" Height="26">
                <Items>
                    <ext:FileUploadField ID="txtDFXField" runat="server" Width="250" Icon="Attach" />
                </Items>
            </ext:Container>
            <ext:Container ID="Container95" runat="server" Layout="ColumnLayout">
                <Items>
                    <ext:Button ID="btnUploadDate" runat="server" Text="上傳">
                        <DirectEvents>
                            <Click OnEvent="btnUploadData_Click">
                                <EventMask ShowMask="true" Msg="正在上傳,请稍后..." MinDelay="200" />
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                </Items>
            </ext:Container>
        </Items>
    </ext:Window>
    <ext:Window ID="PFMAUpload" runat="server" Header="true" Title="批量上傳" Width="300"
        Hidden="true" Padding="5">
        <Items>
            <ext:Container ID="Container98" runat="server" Height="26">
                <Items>
                    <ext:FileUploadField ID="txtPFMAField" runat="server" Width="250" Icon="Attach" />
                </Items>
            </ext:Container>
            <ext:Container ID="Container99" runat="server" Layout="ColumnLayout">
                <Items>
                    <ext:Button ID="btnPFMAUploadDate" runat="server" Text="上傳">
                        <DirectEvents>
                            <Click OnEvent="btnPFMAUploadData_Click">
                                <EventMask ShowMask="true" Msg="正在上傳,请稍后..." MinDelay="200" />
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                </Items>
            </ext:Container>
        </Items>
    </ext:Window>
    <ext:Window ID="IssuesUpload" runat="server" Header="true" Title="批量上傳" Width="300"
        Hidden="true" Padding="5">
        <Items>
            <ext:Container ID="Container100" runat="server" Height="26">
                <Items>
                    <ext:FileUploadField ID="txtIssuesField" runat="server" Width="250" Icon="Attach" />
                </Items>
            </ext:Container>
            <ext:Container ID="Container101" runat="server" Layout="ColumnLayout">
                <Items>
                    <ext:Button ID="btnIssuesUploadDate" runat="server" Text="上傳">
                        <DirectEvents>
                            <Click OnEvent="btnIssuesUploadData_Click">
                                <EventMask ShowMask="true" Msg="正在上傳,请稍后..." MinDelay="200" />
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                </Items>
            </ext:Container>
        </Items>
    </ext:Window>
    <ext:Panel ID="pnlMain" runat="server" Padding="0" Layout="Form" Title="" Border="false"
        BodyStyle="background-color: #DFE8F6;" AutoHeight="true">
        <Items>
            <ext:Panel ID="pnlBegin" runat="server" Padding="0" Layout="Form" Title="" Border="false">
                <Items>
                    <ext:Panel ID="pnlApp" runat="server" Layout="Form" Title="" Frame="True" Border="false"
                        Collapsible="True">
                        <Items>
                            <ext:Container ID="Container67" runat="server" Layout="ColumnLayout" Height="30">
                                <Items>
                                    <ext:Container ID="Container105" runat="server" LabelWidth="70" Layout="Form" ColumnWidth="0.25">
                                        <Items>
                                            <ext:SelectBox ID="sbBuilding" runat="server" FieldLabel="Building" Width="120" IndicatorCls="red-text"
                                                IndicatorText="*">
                                                <Items>
                                                    <ext:ListItem Text="A6" Value="A6" />
                                                    <ext:ListItem Text="A3" Value="A3" />
                                                    <ext:ListItem Text="GZ" Value="GZ" />
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
                                    <ext:Container ID="Container1" runat="server" LabelWidth="70" Layout="Form" ColumnWidth="0.25">
                                        <Items>
                                            <ext:ComboBox ID="sbProd_group" runat="server" FieldLabel="產品類別" Width="120" DisplayField="PARAME_VALUE2"
                                                ValueField="PARAME_VALUE2" LabelWidth="80">
                                                <Store>
                                                    <ext:Store ID="Store16" runat="server">
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
                                                    <Change OnEvent="sbProd_group_Selected">
                                                    </Change>
                                                </DirectEvents>
                                            </ext:ComboBox>
                                            <%--                                                                                <ext:SelectBox ID="sbProd_group" runat="server" FieldLabel="產品類別" Width="120" IndicatorCls="red-text"
                                                IndicatorText="*">
                                                <Items>
                                                    <ext:ListItem Text="Charger" Value="Charger" />
                                                    <ext:ListItem Text="Adapter" Value="Adapter" />
                                                    <ext:ListItem Text="Apple" Value="Apple" />
                                                    <ext:ListItem Text="Others" Value="Others" />
                                                </Items>
                                                <DirectEvents>
                                                    <Select OnEvent="sbProd_group_Selected">
                                                    </Select>
                                                </DirectEvents>
                                            </ext:SelectBox>--%>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container ID="Container4" runat="server" LabelWidth="70" Layout="Form" ColumnWidth="0.25">
                                        <Items>
                                            <ext:TextField ID="txtModel" runat="server" FieldLabel="機種" Width="120">
                                            </ext:TextField>
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container61" runat="server" Layout="ColumnLayout" Height="30">
                                <Items>
                                    <ext:Container ID="Container11" runat="server" LabelWidth="70" Layout="Form" ColumnWidth="0.25">
                                        <Items>
                                            <ext:TextField ID="txtFinalPN" runat="server" FieldLabel="成品料號" Width="120">
                                            </ext:TextField>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container ID="Container41" runat="server" LabelWidth="70" Layout="Form" ColumnWidth="0.25">
                                        <Items>
                                            <ext:DateField ID="txtApplyDate" runat="server" Width="120" FieldLabel="申請日期" Format="yyyy/MM/dd"
                                                LabelSeparator=" ">
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
                    <ext:Panel ID="pnlStart" runat="server" Resizable="false" AutoHeight="true" Icon="Pencil"
                        Modal="true" Padding="5" Hidden="true" Layout="Form" Frame="true" Title="基本資料"
                        Collapsible="True">
                        <Items>
                            <ext:Panel ID="Panel75" runat="server" Layout="Form" Title="" Header="false" Border="false"
                                Padding="5" BodyStyle="background-color: transparent;">
                                <Items>
                                    <ext:Container ID="Container3" runat="server" Layout="ColumnLayout" Height="130">
                                        <Items>
                                            <ext:Container ID="Container5" runat="server" LabelWidth="120" Layout="Form" ColumnWidth="0.30">
                                                <Items>
                                                    <ext:TextField ID="txtDOC_NO" runat="server" Hidden="true">
                                                    </ext:TextField>
                                                    <ext:TextField ID="txtPROD_GROUP" runat="server" FieldLabel="PROD_GROUP" Width="150"
                                                        ReadOnly="true">
                                                    </ext:TextField>
                                                    <ext:TextField ID="txtModel_R" runat="server" FieldLabel="Model" Width="150">
                                                    </ext:TextField>
                                                    <ext:TextField ID="txtPCB" runat="server" FieldLabel="PCB Rev" Width="150" Disabled="false">
                                                    </ext:TextField>
                                                    <ext:TextField ID="txtSPECRev" runat="server" FieldLabel="SPEC/MTR Rev" Width="150"
                                                        Disabled="false">
                                                    </ext:TextField>
                                                    <ext:DateField ID="txtInputDate" runat="server" FieldLabel="Input Date" Width="150"
                                                        Disabled="false">
                                                    </ext:DateField>
                                                </Items>
                                            </ext:Container>
                                            <ext:Container ID="Container7" runat="server" LabelWidth="120" Layout="Form" ColumnWidth="0.30">
                                                <Items>
                                                    <ext:TextField ID="txtCustomer" runat="server" FieldLabel="Customer" Width="150"
                                                        Disabled="true">
                                                    </ext:TextField>
                                                    <ext:SelectBox ID="sbPhase" runat="server" FieldLabel="Stage" Width="150" IndicatorCls="red-text"
                                                        IndicatorText="*">
                                                        <Items>
                                                            <ext:ListItem Text="EVT-1" Value="EVT-1" />
                                                            <ext:ListItem Text="EVT-2" Value="EVT-2" />
                                                            <ext:ListItem Text="EVT-3" Value="EVT-3" />
                                                            <ext:ListItem Text="EVT-4" Value="EVT-4" />
                                                            <ext:ListItem Text="EVT-5" Value="EVT-5" />
                                                            <ext:ListItem Text="EVT-6" Value="EVT-6" />
                                                            <ext:ListItem Text="DVT-1" Value="DVT-1" />
                                                            <ext:ListItem Text="DVT-2" Value="DVT-2" />
                                                            <ext:ListItem Text="DVT-3" Value="DVT-3" />
                                                            <ext:ListItem Text="DVT-4" Value="DVT-4" />
                                                            <ext:ListItem Text="DVT-5" Value="DVT-5" />
                                                            <ext:ListItem Text="DVT-6" Value="DVT-6" />
                                                            <ext:ListItem Text="DVT-7" Value="DVT-7" />
                                                            <ext:ListItem Text="P.Run-1" Value="P.Run-1" />
                                                            <ext:ListItem Text="P.Run-2" Value="P.Run-2" />
                                                            <ext:ListItem Text="P.Run-3" Value="P.Run-3" />
                                                            <ext:ListItem Text="P.Run-4" Value="P.Run-4" />
                                                            <ext:ListItem Text="P.Run-5" Value="P.Run-5" />
                                                        </Items>
                                                        <DirectEvents>
                                                            <Select OnEvent="sbPhase_Change">
                                                            </Select>
                                                        </DirectEvents>
                                                    </ext:SelectBox>
                                                    <ext:TextField ID="txtBomRev" runat="server" FieldLabel="BOM Rev" Width="150" Disabled="false">
                                                    </ext:TextField>
                                                    <ext:TextField ID="txtCustomerRev" runat="server" FieldLabel="Customer Rev" Width="150"
                                                        Disabled="false">
                                                    </ext:TextField>
                                                    <ext:DateField ID="txtPkDate" runat="server" FieldLabel="PK Date" Width="150" Disabled="false">
                                                    </ext:DateField>
                                                </Items>
                                            </ext:Container>
                                            <ext:Container ID="Container2" runat="server" LabelWidth="120" Layout="Form" ColumnWidth="0.30">
                                                <Items>
                                                    <ext:TextField ID="txtLotQty" runat="server" FieldLabel="Lot Qty" Width="150">
                                                    </ext:TextField>
                                                    <ext:TextField ID="txtLINE" runat="server" FieldLabel="Line" Width="150">
                                                    </ext:TextField>
                                                    <ext:DateField ID="txtIssueDate" runat="server" FieldLabel="Issue Date" Width="150"
                                                        Disabled="true">
                                                    </ext:DateField>
                                                    <ext:Checkbox ID="cbModify" runat="server" FieldLabel="Modify" Width="200" Disabled="true">
                                                        <DirectEvents>
                                                            <Change OnEvent="cbModify_SelectedIndexChanged">
                                                            </Change>
                                                        </DirectEvents>
                                                    </ext:Checkbox>
                                                    <ext:TextField ID="txtWorkOrder" runat="server" FieldLabel="工單號" Width="150">
                                                        <DirectEvents>
                                                            <Change OnEvent="check_workorder">
                                                            </Change>
                                                        </DirectEvents>
                                                    </ext:TextField>
                                                </Items>
                                            </ext:Container>
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:Panel>
                            <ext:Panel ID="pnlmodify" runat="server" Layout="Form" Title="" Header="false" Border="false"
                                Padding="5" BodyStyle="background-color: transparent;" Hidden="true">
                                <Items>
                                    <ext:Container ID="Container108" runat="server" Layout="ColumnLayout" Height="100">
                                        <Items>
                                            <ext:Container ID="Container106" runat="server" LabelWidth="120" Layout="Form" ColumnWidth="0.6">
                                                <Items>
                                                    <ext:TextField ID="txtFromModel" runat="server" FieldLabel="From Model" Width="150">
                                                        <DirectEvents>
                                                            <Change OnEvent="btnFromModel">
                                                            </Change>
                                                        </DirectEvents>
                                                    </ext:TextField>
                                                    <ext:TextArea ID="txtModelRemark" runat="server" FieldLabel="備註說明" Width="400">
                                                    </ext:TextArea>
                                                </Items>
                                            </ext:Container>
                                            <ext:Container ID="Container107" runat="server" LabelWidth="120" Layout="Form" ColumnWidth="0.3">
                                                <Items>
                                                    <ext:HyperLink ID="HyHomePage" runat="server" NavigateUrl="" Text="" Target="_blank"
                                                        FieldLabel="NPI Gating Link" />
                                                </Items>
                                            </ext:Container>
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:Panel>
                            <ext:Panel ID="Panel1" runat="server" Layout="Form" Title="" Header="false" Border="false"
                                Padding="5" BodyStyle="background-color: transparent;">
                                <Items>
                                    <ext:Container ID="Container112" runat="server" LabelWidth="120" Layout="Form">
                                        <Items>
                                            <ext:TextArea ID="txtRemarkM" runat="server" FieldLabel="Remark" LabelWidth="70"
                                                Width="400">
                                            </ext:TextArea>
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:Panel>
                            <ext:Panel ID="pnl15" runat="server" Layout="Form" Title="" Header="false" Border="false"
                                Padding="5" BodyStyle="background-color: transparent;">
                                <Items>
                                    <ext:Container ID="Container109" runat="server" LabelWidth="120" Layout="Form">
                                        <Items>
                                            <ext:TextField ID="txtSub_No" runat="server" FieldLabel="DOC No" LabelWidth="70"
                                                Width="400">
                                            </ext:TextField>
                                            <ext:CheckboxGroup ID="cbStartItem" runat="server" FieldLabel="需啟動項" ColumnsWidths="80,80,80,80"
                                                Cls="x-check-group-alt" TabIndex="1">
                                                <Items>
                                                    <ext:Checkbox ID="chkD" runat="server" BoxLabel="DFX" InputValue="D" />
                                                    <ext:Checkbox ID="chkC" runat="server" BoxLabel="CTQ/CTP" InputValue="C" />
                                                    <ext:Checkbox ID="chkI" runat="server" BoxLabel="IssueList" InputValue="I" />
                                                    <ext:Checkbox ID="chkP" runat="server" BoxLabel="PFMEA" InputValue="P" />
                                                </Items>
                                            </ext:CheckboxGroup>
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:Panel>
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
                                            <ext:RecordField Name="PROD_GROUP" />
                                            <ext:RecordField Name="PHASE" />
                                            <ext:RecordField Name="MODEL_NAME" />
                                            <ext:RecordField Name="CUSTOMER" />
                                            <ext:RecordField Name="APPLY_USERID" />
                                            <ext:RecordField Name="APPLY_DATE" Type="Date" />
                                            <ext:RecordField Name="NPI_PM" />
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
                                <ext:Column DataIndex="APPLY_USERID" Header="申請人" Width="100">
                                </ext:Column>
                                <ext:DateColumn DataIndex="APPLY_DATE" Header="申請日期" Width="100" Format="yyyy/MM/dd">
                                </ext:DateColumn>
                                <ext:Column DataIndex="NPI_PM" Header="PM" Width="100">
                                </ext:Column>
                                <ext:ImageCommandColumn Width="80">
                                    <Commands>
                                        <ext:ImageCommand Icon="ApplicationGo" CommandName="StartForm" Text="Start">
                                            <ToolTip Text="StartForm" />
                                        </ext:ImageCommand>
                                    </Commands>
                                </ext:ImageCommandColumn>
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel ID="CheckboxSelectionModel" runat="server" SingleSelect="true">
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <DirectEvents>
                            <Command OnEvent="grdInfo_RowCommand">
                                <ExtraParams>
                                    <ext:Parameter Name="DOC_NO" Value="record.data.DOC_NO" Mode="Raw" />
                                    <ext:Parameter Name="PROD_GROUP" Value="record.data.PROD_GROUP" Mode="Raw" />
                                    <ext:Parameter Name="ASSY_PN" Value="record.data.ASSY_PN" Mode="Raw" />
                                    <ext:Parameter Name="MODEL_NAME" Value="record.data.MODEL_NAME" Mode="Raw" />
                                    <%--<ext:Parameter Name="FINAL_PN" Value="record.data.FINAL_PN" Mode="Raw" />--%>
                                    <ext:Parameter Name="CUSTOMER" Value="record.data.CUSTOMER" Mode="Raw" />
                                    <ext:Parameter Name="NPI_PM" Value="record.data.NPI_PM" Mode="Raw" />
                                </ExtraParams>
                            </Command>
                        </DirectEvents>
                    </ext:GridPanel>
                </Items>
            </ext:Panel>
            <ext:Panel ID="PanelDFXScore" runat="server" Border="false" Region="Center" Header="false"
                AutoHeight="true" Title="" Padding="5" Layout="Form" BodyStyle="background-color: transparent;"
                Collapsible="True" Hidden="true">
                <Items>
                    <ext:GridPanel ID="gridDFXScore" runat="server" Frame="true" Title="DFX Score Summary"
                        AutoHeight="true">
                        <Store>
                            <ext:Store ID="Store14" runat="server">
                                <Reader>
                                    <ext:JsonReader>
                                        <Fields>
                                            <ext:RecordField Name="Stage" />
                                            <ext:RecordField Name="Dept" />
                                            <ext:RecordField Name="item" />
                                            <ext:RecordField Name="Score" />
                                            <ext:RecordField Name="PriorityLevel0" />
                                            <ext:RecordField Name="PriorityLevel1" />
                                            <ext:RecordField Name="PriorityLevel2" />
                                            <ext:RecordField Name="PriorityLevel3" />
                                            <ext:RecordField Name="Result" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>
                                <ext:Column DataIndex="Stage" Header="Stage" Width="80" Hidden="true">
                                </ext:Column>
                                <ext:Column DataIndex="Dept" Header="Dept" Width="80">
                                </ext:Column>
                                <ext:Column DataIndex="item" Header="DFX item【Score:85/90/90%】" Width="180">
                                </ext:Column>
                                <ext:Column DataIndex="Score" Header="Score %" Width="80">
                                </ext:Column>
                                <ext:Column DataIndex="PriorityLevel0" Header="Most important item open數" Width="150">
                                </ext:Column>
                                <ext:Column DataIndex="PriorityLevel1" Header="Major item open數" Width="120">
                                </ext:Column>
                                <ext:Column DataIndex="PriorityLevel2" Header="Middle item open數" Width="120">
                                </ext:Column>
                                <ext:Column DataIndex="PriorityLevel3" Header="Minor item open數" Width="120">
                                </ext:Column>
                                <ext:Column DataIndex="Result" Header="Result" Width="100">
                                    <Renderer Fn="Result" />                                
                                    </ext:Column>
                            </Columns>
                        </ColumnModel>
                    </ext:GridPanel>
                </Items>
            </ext:Panel>
            <ext:Panel ID="pnlDFXList" runat="server" Border="false" Region="Center" Header="false"
                AutoHeight="true" Title="DFXList" Padding="5" Layout="Form" BodyStyle="background-color: transparent;"
                Collapsible="True" Hidden="true">
                <Items>
                    <ext:Panel ID="pnlDFXInfo" runat="server" Layout="FormLayout" Title="DFX填寫" Border="false"
                        BodyStyle="background-color: #DFE8F6;" Padding="5" Collapsible="True" Hidden="true">
                        <Items>
                            <ext:TableLayout ID="TableLayout2" runat="server" Columns="4">
                                <Cells>
                                    <ext:Cell>
                                        <ext:Container ID="Container16" runat="server" Layout="Form" LabelWidth="80" Width="180">
                                            <Items>
                                                <ext:TextField ID="txtDFXNo" runat="server" FieldLabel="DFX編號" Width="120" ReadOnly="true">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell ColSpan="3">
                                        <ext:Container ID="Container12" runat="server" Width="235" LabelWidth="80">
                                            <Items>
                                                <ext:TextField ID="txtItems" runat="server" FieldLabel="項目" Width="200" ReadOnly="true">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                </Cells>
                                <Cells>
                                    <ext:Cell ColSpan="4">
                                        <ext:Container ID="Container10" runat="server" Width="520" LabelWidth="80">
                                            <Items>
                                                <ext:TextArea ID="txtRequirements" runat="server" FieldLabel="Requirements" Width="928"
                                                    ReadOnly="true">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                </Cells>
                                <Cells>
                                    <ext:Cell>
                                        <ext:Container ID="Container58" runat="server" Width="235" LabelWidth="80" Height="20">
                                            <Items>
                                                <ext:NumberField ID="txtPriority" runat="server" FieldLabel="Priority Level" Width="200"
                                                    MinValue="0" ReadOnly="true">
                                                </ext:NumberField>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Container ID="Container8" runat="server" LabelWidth="120" Width="235">
                                            <Items>
                                                <ext:ComboBox ID="cbCompliance" runat="server" FieldLabel="DFX Compliance" Width="220">
                                                    <Items>
                                                        <ext:ListItem Text="Y" Value="Y" />
                                                        <ext:ListItem Text="N" Value="N" />
                                                        <ext:ListItem Text="NA" Value="NA" />
                                                    </Items>
                                                    <DirectEvents>
                                                        <Select OnEvent="txtPriorityLevel_Change">
                                                        </Select>
                                                    </DirectEvents>
                                                </ext:ComboBox>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Container ID="Container59" runat="server" Width="235" LabelWidth="80" Height="20">
                                            <Items>
                                                <ext:TextField ID="txtMaxPoints" runat="server" FieldLabel="Max Points" Width="200"
                                                    ReadOnly="true">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Container ID="Container60" runat="server" Width="235" LabelWidth="80" Height="20">
                                            <Items>
                                                <ext:TextField ID="txtDFXPoints" runat="server" FieldLabel="DFX Points" Width="200"
                                                    ReadOnly="true">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                </Cells>
                                <Cells>
                                    <ext:Cell ColSpan="4">
                                        <ext:Container ID="Container62" runat="server" LabelWidth="80" Width="520">
                                            <Items>
                                                <ext:TextField ID="txtLocation" runat="server" FieldLabel="Location/Doc" Width="450">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                </Cells>
                                <Cells>
                                    <ext:Cell ColSpan="4">
                                        <ext:Container ID="Container63" runat="server" LabelWidth="80" Width="520">
                                            <Items>
                                                <ext:TextArea ID="txtDFXCommnet" runat="server" FieldLabel="Comment" Width="928">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                </Cells>
                                <Cells>
                                    <ext:Cell ColSpan="4">
                                        <ext:Container ID="Container20" runat="server" Layout="Form" ColumnWidth="0.33" LabelWidth="120">
                                            <Items>
                                                <ext:Button ID="btnSaveDFX" runat="server" Text="保存" Icon="Disk">
                                                    <DirectEvents>
                                                        <Click OnEvent="btnSaveDFX_Click">
                                                        </Click>
                                                    </DirectEvents>
                                                </ext:Button>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                </Cells>
                            </ext:TableLayout>
                        </Items>
                    </ext:Panel>
                    <ext:Panel ID="pnlDFXReply" runat="server" Layout="Form" Title="DFX回覆" Border="false"
                        BodyStyle="background-color: #DFE8F6;" Padding="5" Collapsible="True" Hidden="true">
                        <Items>
                            <%--  <ext:Container ID="Container9" runat="server" Layout="ColumnLayout" Height="80">
                                <Items>
                                    <ext:Container ID="Container22" runat="server" Layout="Form" LabelWidth="120" ColumnWidth="0.3">
                                        <Items>
                                            <ext:Hidden ID="hidDoc" runat="server">
                                            </ext:Hidden>
                                            <ext:Hidden ID="hidItem" runat="server">
                                            </ext:Hidden>
                                            <ext:TextField ID="txtActions" runat="server" FieldLabel="措施實施" Width="120">
                                            </ext:TextField>
                                            <ext:TextArea ID="txtRemark" runat="server" FieldLabel="備註" Width="120">
                                            </ext:TextArea>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container ID="Container23" runat="server" Layout="Form" LabelWidth="120" ColumnWidth="0.3">
                                        <Items>
                                            <ext:DateField ID="txtCompleteDate" runat="server" FieldLabel="目標完成日期" Width="120">
                                            </ext:DateField>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container ID="Container24" runat="server" Layout="Form" LabelWidth="120" ColumnWidth="0.3"
                                        Height="20">
                                        <Items>
                                           <ext:ComboBox ID="cmbDFXStatus" runat="server" FieldLabel="改善狀態" Width="300">
                                                    <Items>
                                                        <ext:ListItem Text="FAIL" Value="FAIL" />
                                                        <ext:ListItem Text="Condition PASS" Value="Condition PASS" />
                                                        <ext:ListItem Text="WAVE" Value="WAVE" />
                                                      
                                                        
                                                      
                                                    </Items>
                                                </ext:ComboBox>
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container25" runat="server" Layout="Form" ColumnWidth="0.33" LabelWidth="120">
                                <Items>
                                    <ext:Button ID="btnDFXReply_Save" runat="server" Text="保存" Icon="Disk">
                                        <DirectEvents>
                                            <Click OnEvent="btnDFXReply_Click">
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Container>--%>
                            <ext:TableLayout ID="TableLayout3" runat="server" Columns="2">
                                <Cells>
                                    <ext:Cell ColSpan="2">
                                        <ext:Container ID="Container22" runat="server" Width="520" LabelWidth="80">
                                            <Items>
                                                <ext:Hidden ID="hidDoc" runat="server">
                                                </ext:Hidden>
                                                <ext:Hidden ID="hidItem" runat="server">
                                                </ext:Hidden>
                                                <ext:TextField ID="Item" runat="server" FieldLabel="項目" Width="250" ReadOnly="true">
                                                </ext:TextField>
                                                <ext:TextField ID="DFXDEPT" runat="server" FieldLabel="部門" Width="250" ReadOnly="true">
                                                </ext:TextField>
                                                <ext:TextField ID="Location" runat="server" FieldLabel="Location" Width="500" ReadOnly="true">
                                                </ext:TextField>
                                                <ext:TextArea ID="Requirements1" runat="server" FieldLabel="Requirements" Width="928"
                                                    ReadOnly="true">
                                                </ext:TextArea>
                                                <ext:TextArea ID="txtActions" runat="server" FieldLabel="措施實施" Width="928">
                                                </ext:TextArea>
                                                <ext:TextArea ID="txtRemark" runat="server" FieldLabel="備註" Width="928">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                </Cells>
                                <Cells>
                                    <ext:Cell ColSpan="1">
                                        <ext:Container ID="Container23" runat="server" Width="520" LabelWidth="80">
                                            <Items>
                                                <ext:DateField ID="txtCompleteDate" runat="server" FieldLabel="目標完成日期" Width="300"
                                                    Format="yyyy/MM/dd">
                                                </ext:DateField>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                </Cells>
                                <Cells>
                                    <ext:Cell ColSpan="1">
                                        <ext:Container ID="Container24" runat="server" Width="520" LabelWidth="80">
                                            <Items>
                                                <ext:ComboBox ID="cmbDFXStatus" runat="server" FieldLabel="改善狀態" Width="300" SelectedIndex="0">
                                                    <Items>
                                                        <ext:ListItem Text="WAIVE" Value="WAIVE" />
                                                        <ext:ListItem Text="FAIL" Value="FAIL" />
                                                        <ext:ListItem Text="PASS" Value="PASS" />
                                                    </Items>
                                                </ext:ComboBox>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                </Cells>
                                <Cells>
                                    <ext:Cell>
                                        <ext:Container ID="Container9" runat="server" Layout="Form" ColumnWidth="0.33" LabelWidth="120">
                                            <Items>
                                                <ext:Button ID="btnDFXReply_Save" runat="server" Text="保存" Icon="Disk">
                                                    <DirectEvents>
                                                        <Click OnEvent="btnDFXReply_Click">
                                                        </Click>
                                                    </DirectEvents>
                                                </ext:Button>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                </Cells>
                            </ext:TableLayout>
                        </Items>
                    </ext:Panel>
                    <ext:GridPanel ID="grdDFXList" runat="server" Frame="true" Title="待處理 DFX 列表" Header="true"
                        AutoScroll="true" Height="350">
                        <Store>
                            <ext:Store ID="Store3" runat="server">
                                <Reader>
                                    <ext:JsonReader>
                                        <Fields>
                                            <ext:RecordField Name="ID" />
                                            <ext:RecordField Name="DFXNo" />
                                            <ext:RecordField Name="Item" />
                                            <ext:RecordField Name="ItemType" />
                                            <ext:RecordField Name="ItemName" />
                                            <ext:RecordField Name="Requirements" />
                                            <ext:RecordField Name="Losses" />
                                            <ext:RecordField Name="Location" />
                                            <ext:RecordField Name="Class" />
                                            <ext:RecordField Name="PriorityLevel" />
                                            <ext:RecordField Name="MaxPoints" />
                                            <ext:RecordField Name="DFXPoints" />
                                            <ext:RecordField Name="WriteDept" />
                                            <ext:RecordField Name="Compliance" />
                                            <ext:RecordField Name="Comments" />
                                            <ext:RecordField Name="Status" />
                                            <ext:RecordField Name="Actions" />
                                            <ext:RecordField Name="FilePath" />
                                            <ext:RecordField Name="CompletionDate" />
                                            <ext:RecordField Name="Tracking" />
                                            <ext:RecordField Name="Remark" />
                                            <ext:RecordField Name="OldItemType" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>
                                <ext:CommandColumn Width="70">
                                    <Commands>
                                        <ext:GridCommand Icon="Pencil" CommandName="Write" Text="填寫">
                                        </ext:GridCommand>
                                        <ext:GridCommand Icon="Pencil" CommandName="Reply" Text="回覆">
                                        </ext:GridCommand>
                                    </Commands>
                                    <PrepareToolbar Fn="NoteGoHid" />
                                </ext:CommandColumn>
                                <ext:Column Header="<a>Items</a><br><a>(項目)</a>" DataIndex="Item" Width="100">
                                </ext:Column>
                                <ext:Column Header="<a>Status</a><br><a>(狀態)</a>" DataIndex="Status" Width="70">
                                    <Renderer Fn="StatusFormat" />
                                </ext:Column>
                                <ext:Column Header="<a>Dept</a><br><a>(部門)</a>" DataIndex="WriteDept" Width="60">
                                </ext:Column>
                                <ext:Column Header="<a>Category</a><br><a>(類型)</a>" DataIndex="ItemType" Width="120">
                                </ext:Column>
                                <ext:Column Header="<a>OldCategory</a><br><a>(舊類別)</a>" DataIndex="OldItemType" Width="140">
                                </ext:Column>
                                <ext:Column Header="<a>ItemName</a><br><a>(項目名稱)</a>" DataIndex="ItemName" Width="102">
                                </ext:Column>
                                <ext:Column Header="<a>Requirements</a><br><a</a>" DataIndex="Requirements" Width="150">
                                </ext:Column>
                                <ext:Column Header="<a>Location</a><br><a</a>" DataIndex="Location" Width="103">
                                </ext:Column>
                                <ext:Column Header="<a>圖片</a><br><a</a>" DataIndex="FilePath" Width="103">
                                    <Renderer Fn="change_Attachement" />
                                </ext:Column>
                                <ext:Column Header="<a>(DFX Complicane)</a>" DataIndex="Compliance" Width="104">
                                </ext:Column>
                                <ext:Column Header="<a>違反損失</a><br><a></a>" DataIndex="Losses" Width="103">
                                </ext:Column>
                                <ext:Column Header="<a>PriorityLevel</a><br><a></a>" DataIndex="PriorityLevel" Width="105">
                                </ext:Column>
                                <ext:Column Header="<a>MaxPoints</a><br><a></a>" DataIndex="MaxPoints" Width="106">
                                </ext:Column>
                                <ext:Column Header="<a>DFXPoints</a><br><a></a>" DataIndex="DFXPoints" Width="107">
                                </ext:Column>
                                <ext:Column Header="<a>Comments</a><br><a></a>" DataIndex="Comments" Width="108">
                                </ext:Column>
                                <ext:Column Header="<a>Actions Taken</a><br><a>(措施實施)</a>" DataIndex="Actions" Width="105">
                                </ext:Column>
                                <ext:Column Header="<a>Completion Date</a><br><a>(目標完成時間)</a>" DataIndex="CompletionDate"
                                    Width="106">
                                </ext:Column>
                                <ext:Column Header="<a>改善狀態</a><br><a></a>" DataIndex="Tracking" Width="107">
                                </ext:Column>
                                <ext:Column Header="<a>Remark</a><br><a>(備註)</a>" DataIndex="Remark" Width="108">
                                </ext:Column>
                            </Columns>
                        </ColumnModel>
                        <DirectEvents>
                            <Command OnEvent="grdDFXList_RowCommand">
                                <ExtraParams>
                                    <ext:Parameter Name="DFXNo" Value="record.data.DFXNo" Mode="Raw" />
                                    <ext:Parameter Name="Item" Value="record.data.Item" Mode="Raw" />
                                    <ext:Parameter Name="ItemType" Value="record.data.ItemType" Mode="Raw" />
                                    <ext:Parameter Name="ItemName" Value="record.data.ItemName" Mode="Raw" />
                                    <ext:Parameter Name="Requirements" Value="record.data.Requirements" Mode="Raw" />
                                    <ext:Parameter Name="PriorityLevel" Value="record.data.PriorityLevel" Mode="Raw" />
                                    <ext:Parameter Name="Compliance" Value="record.data.Compliance" Mode="Raw" />
                                    <ext:Parameter Name="CompletionDate" Value="record.data.CompletionDate" Mode="Raw" />
                                    <ext:Parameter Name="MaxPoints" Value="record.data.MaxPoints" Mode="Raw" />
                                    <ext:Parameter Name="DFXPoints" Value="record.data.DFXPoints" Mode="Raw" />
                                    <ext:Parameter Name="Location" Value="record.data.Location" Mode="Raw" />
                                    <ext:Parameter Name="Comments" Value="record.data.Comments" Mode="Raw" />
                                    <ext:Parameter Name="Actions" Value="record.data.Actions" Mode="Raw" />
                                    <ext:Parameter Name="Tracking" Value="record.data.Tracking" Mode="Raw" />
                                    <ext:Parameter Name="Remark" Value="record.data.Remark" Mode="Raw" />
                                    <ext:Parameter Name="WriteDept" Value="record.data.WriteDept" Mode="Raw" />
                                    <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                </ExtraParams>
                            </Command>
                        </DirectEvents>
                        <SelectionModel>
                            <ext:RowSelectionModel ID="RowSelectionModel2" runat="server" SingleSelect="true" />
                        </SelectionModel>
                        <TopBar>
                            <ext:Toolbar ID="Toolbar5" runat="server">
                                <Items>
                                    <ext:Button ID="btnExport" runat="server" Text="導出Excel" Icon="PageExcel" AutoPostBack="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnExport_click">
                                                <ExtraParams>
                                                    <ext:Parameter Name="values" Mode="Raw" Value="Ext.encode(#{grdDFXList}.getRowsValues({selectedOnly:false}))">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button ID="btnUploadDFX" runat="server" Text="批量上傳" Icon="DatabaseAdd">
                                        <DirectEvents>
                                            <Click OnEvent="btnUploadDFX_Click">
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                    </ext:GridPanel>
                </Items>
            </ext:Panel>
            <ext:Panel ID="pnlCTQList" runat="server" Border="false" Region="Center" AutoHeight="true"
                Title="CTQ" Header="false" Padding="5" Layout="Form" BodyStyle="background-color: transparent;"
                Hidden="true">
                <Items>
                    <ext:Panel ID="pnlCTQInfo" runat="server" Layout="Form" Title="CTQ填寫" Border="false"
                        BodyStyle="background-color: #DFE8F6;" Padding="5" Collapsible="True" Hidden="true">
                        <Items>
                            <ext:TableLayout ID="TableLayout8" runat="server" Columns="3">
                                <Cells>
                                    <ext:Cell ColSpan="2">
                                        <ext:Container ID="Container15" runat="server" LabelWidth="80" Width="200">
                                            <Items>
                                                <ext:Hidden ID="hidId" runat="server" Text="" Hidden="true">
                                                </ext:Hidden>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell ColSpan="1">
                                        <ext:Container ID="Container17" runat="server" LabelWidth="80" Width="200">
                                            <Items>
                                                <ext:Hidden ID="hidGoal" runat="server" Text="" Hidden="true">
                                                </ext:Hidden>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell ColSpan="3">
                                        <ext:Container ID="Container52" runat="server" LabelWidth="80" Width="300">
                                            <Items>
                                                <ext:TextField ID="txtCTQ" runat="server" FieldLabel="CTQ項目" Width="500" ReadOnly="true">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell ColSpan="1">
                                        <ext:Container ID="Container103" runat="server" LabelWidth="80" Width="300">
                                            <Items>
                                                <ext:TextField ID="txtdept_CTQ" runat="server" FieldLabel="部门" Width="280" ReadOnly="true">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell ColSpan="1">
                                        <ext:Container ID="Container53" runat="server" LabelWidth="80" Width="300">
                                            <Items>
                                                <ext:TextField ID="txtGoal" runat="server" FieldLabel="GOAL" Width="280" ReadOnly="true">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell ColSpan="1">
                                        <ext:Container ID="Container76" runat="server" LabelWidth="120" Width="300">
                                            <Items>
                                                <ext:TextField ID="txtControlType" runat="server" FieldLabel="CONTROL_TYPE" Width="280"
                                                    ReadOnly="true">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell ColSpan="1">
                                        <ext:Container ID="Container75" runat="server" LabelWidth="80" Width="300">
                                            <Items>
                                                <ext:TextField ID="txtAct" runat="server" FieldLabel="ACT" Width="280">
                                                    <DirectEvents>
                                                        <Change OnEvent="txtAct_Changed">
                                                        </Change>
                                                    </DirectEvents>
                                                </ext:TextField>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell ColSpan="1">
                                        <ext:Container ID="Container54" runat="server" LabelWidth="80" Width="300">
                                            <Items>
                                                <ext:TextField ID="txtResult" runat="server" FieldLabel="RESULT" Width="280">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell ColSpan="1">
                                        <ext:Container ID="Container102" runat="server" LabelWidth="120" Width="300">
                                            <Items>
                                                <ext:TextField ID="txtflag" runat="server" FieldLabel="是否需要附件" Width="280" ReadOnly="true">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell ColSpan="3">
                                        <ext:Container ID="Container13" runat="server" LabelWidth="170" Width="500">
                                            <Items>
                                                <ext:FileUploadField ID="CTQWriteFUpload" runat="server" FieldLabel="附件上傳:(請用英文或繁體命名)"
                                                    LabelSeparator=" " ButtonText="請選擇文件">
                                                </ext:FileUploadField>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell ColSpan="3">
                                        <ext:Container ID="Container56" runat="server" LabelWidth="80" Width="520">
                                            <Items>
                                                <ext:TextArea ID="txtDescription" runat="server" Text="" Width="928" Hidden="true"
                                                    FieldLabel="<a>Issue Description</a><br><a>(問題描述)</a>">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell ColSpan="3">
                                        <ext:Container ID="Container74" runat="server" LabelWidth="80" Width="520">
                                            <Items>
                                                <ext:TextArea ID="txtRootCause" runat="server" Text="" Width="928" Hidden="true"
                                                    FieldLabel="<a>RootCause</a><br><a>(原因分析)</a>">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell ColSpan="3">
                                        <ext:Container ID="Container14" runat="server" LabelWidth="80" Width="520">
                                            <Items>
                                                <ext:RadioGroup ID="ckgCauseType" runat="server" FieldLabel="原因種類" ColumnsWidths="200,200,200,200"
                                                    Cls="x-check-group-alt" TabIndex="1" Width="928" Hidden="true">
                                                    <Items>
                                                        <%--<ext:Radio ID="rgChangeY" runat="server" BoxLabel="Y" Width="60" />
                                                        <ext:Radio ID="rgChangeN" runat="server" BoxLabel="N" Width="60" />--%>
                                                        <ext:Radio ID="chkDesign" runat="server" BoxLabel="Design(設計)" InputValue="D" />
                                                        <ext:Radio ID="chkM" runat="server" BoxLabel="Material(材料)" InputValue="M" />
                                                        <ext:Radio ID="chKProcess" runat="server" BoxLabel="Process(加工)" InputValue="P" />
                                                        <ext:Radio ID="chkE" runat="server" BoxLabel="Equipment/Machine(機器設備)" InputValue="E" />
                                                        <ext:Radio ID="chkW" runat="server" BoxLabel="Workmanship(作業)" InputValue="W" />
                                                        <ext:Radio ID="chkO" runat="server" BoxLabel="Other(其他)" InputValue="O" />
                                                    </Items>
                                                </ext:RadioGroup>
                                                <%--  <ext:CheckboxGroup ID="ckgCauseType" runat="server" FieldLabel="原因種類" ColumnsWidths="200,200,200,200"
                                                    Cls="x-check-group-alt" TabIndex="1" Width="928" Hidden="true">
                                                    <Items>
                                                        <ext:Checkbox ID="chkDesign" runat="server" BoxLabel="Design(設計)" InputValue="D" />
                                                        <ext:Checkbox ID="chkM" runat="server" BoxLabel="Material(材料)" InputValue="M" />
                                                        <ext:Checkbox ID="chKProcess" runat="server" BoxLabel="Process(加工)" InputValue="P" />
                                                        <ext:Checkbox ID="chkE" runat="server" BoxLabel="Equipment/Machine(機器設備)" InputValue="E" />
                                                        <ext:Checkbox ID="chkW" runat="server" BoxLabel="Workmanship(作業)" InputValue="W" />
                                                        <ext:Checkbox ID="chkO" runat="server" BoxLabel="Other(其他)" InputValue="O" />
                                                    </Items>
                                                </ext:CheckboxGroup>--%>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell ColSpan="1">
                                        <ext:Container ID="Container55" runat="server" LabelWidth="80" Width="300">
                                            <Items>
                                                <ext:Button ID="btnConfirm" runat="server" Text="保存" Icon="Accept">
                                                    <DirectEvents>
                                                        <Click OnEvent="btnConfirm_Click">
                                                        </Click>
                                                    </DirectEvents>
                                                </ext:Button>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                </Cells>
                                <Cells>
                                </Cells>
                            </ext:TableLayout>
                        </Items>
                    </ext:Panel>
                    <ext:Panel ID="pnlCLCAReply" runat="server" Layout="Form" Title="CLCA回覆" Border="false"
                        BodyStyle="background-color: #DFE8F6;" Padding="5" Collapsible="True" Hidden="true">
                        <Items>
                            <ext:TableLayout ID="TableLayout1" runat="server" Columns="2">
                                <Cells>
                                    <ext:Cell ColSpan="2">
                                        <ext:Container ID="Container19" runat="server" Width="520" LabelWidth="80">
                                            <Items>
                                                <ext:TextArea ID="txtCLCAActions" runat="server" FieldLabel="臨時對策" Width="928">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                </Cells>
                                <Cells>
                                    <ext:Cell ColSpan="2">
                                        <ext:Container ID="Container64" runat="server" Width="520" LabelWidth="80">
                                            <Items>
                                                <ext:TextArea ID="txtCLCAPreActions" runat="server" FieldLabel="預防矯正措施" Width="928">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                </Cells>
                                <Cells>
                                    <ext:Cell ColSpan="1">
                                        <ext:Container ID="Container90" runat="server" LabelWidth="80" Width="520">
                                            <Items>
                                                <ext:DateField ID="txtCLCACompleteDate" runat="server" FieldLabel="完成日期" Width="300">
                                                </ext:DateField>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell ColSpan="1">
                                        <ext:Container ID="Container78" runat="server" LabelWidth="80" Width="520">
                                            <Items>
                                                <ext:ComboBox ID="cmbCLCAImproveStatus" runat="server" FieldLabel="改善狀態" Width="300">
                                                    <Items>
                                                        <ext:ListItem Text="Open" Value="Open" />
                                                        <ext:ListItem Text="Close" Value="Close" />
                                                        <ext:ListItem Text="Tracking" Value="Tracking" />
                                                    </Items>
                                                </ext:ComboBox>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                </Cells>
                                <Cells>
                                    <ext:Cell>
                                        <ext:Container ID="Container42" runat="server" Layout="Form" ColumnWidth="0.33" LabelWidth="120">
                                            <Items>
                                                <ext:Button ID="btnCLCAReply_Save" runat="server" Text="保存" Icon="Disk">
                                                    <DirectEvents>
                                                        <Click OnEvent="btnCLCAReply_Click">
                                                        </Click>
                                                    </DirectEvents>
                                                </ext:Button>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                </Cells>
                            </ext:TableLayout>
                        </Items>
                    </ext:Panel>
                    <ext:GridPanel ID="grdCTQInfo" runat="server" Frame="false" Title="待處理CTQ列表" Border="false"
                        Hidden="true" Height="350">
                        <Store>
                            <ext:Store ID="Store2" runat="server">
                                <Reader>
                                    <ext:JsonReader>
                                        <Fields>
                                            <ext:RecordField Name="ID" />
                                            <ext:RecordField Name="DOC_NO" />
                                            <ext:RecordField Name="SUB_DOC_NO" />
                                            <ext:RecordField Name="PROD_GROUP" />
                                            <ext:RecordField Name="PHASE" />
                                            <ext:RecordField Name="DEPT" />
                                            <ext:RecordField Name="PROCESS" />
                                            <ext:RecordField Name="CTQ" />
                                            <ext:RecordField Name="UNIT" />
                                            <ext:RecordField Name="CONTROL_TYPE" />
                                            <ext:RecordField Name="GOAL" />
                                            <ext:RecordField Name="ACT" />
                                            <ext:RecordField Name="GOALStr" />
                                            <ext:RecordField Name="ACTStr" />
                                            <ext:RecordField Name="RESULT" />
                                            <ext:RecordField Name="Comment" />
                                            <ext:RecordField Name="STATUS" />
                                            <ext:RecordField Name="DESCRIPTION" />
                                            <ext:RecordField Name="ROOT_CAUSE" />
                                            <ext:RecordField Name="REPLY_USERID" />
                                            <ext:RecordField Name="TEMPORARY_ACTION" />
                                            <ext:RecordField Name="CORRECTIVE_PREVENTIVE_ACTION" />
                                            <ext:RecordField Name="COMPLETE_DATE" />
                                            <ext:RecordField Name="IMPROVEMENT_STATUS" />
                                            <ext:RecordField Name="UPDATE_TIME" Type="Date" />
                                            <ext:RecordField Name="flag" />
                                            <ext:RecordField Name="D" />
                                            <ext:RecordField Name="M" />
                                            <ext:RecordField Name="P" />
                                            <ext:RecordField Name="E" />
                                            <ext:RecordField Name="W" />
                                            <ext:RecordField Name="O" />
                                            <ext:RecordField Name="W_FILEPATH" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>
                                <ext:RowNumbererColumn>
                                </ext:RowNumbererColumn>
                                <ext:CommandColumn Width="70">
                                    <Commands>
                                        <ext:GridCommand Icon="Pencil" CommandName="Write" Text="填寫">
                                        </ext:GridCommand>
                                        <ext:GridCommand Icon="Pencil" CommandName="Reply" Text="回覆">
                                        </ext:GridCommand>
                                    </Commands>
                                    <PrepareToolbar Fn="NoteGoHid3" />
                                </ext:CommandColumn>
                                <ext:Column DataIndex="DEPT" Header="部門" Width="60">
                                </ext:Column>
                                <ext:Column DataIndex="STATUS" Header="狀態" Width="60">
                                    <Renderer Fn="StatusFormat" />
                                </ext:Column>
                                <ext:Column DataIndex="PROCESS" Header="製程" Width="90">
                                </ext:Column>
                                <ext:Column DataIndex="CTQ" Header="CTQ項目" Width="180">
                                </ext:Column>
                                <ext:Column DataIndex="CONTROL_TYPE" Header="Control Type" Width="90">
                                </ext:Column>
                                <ext:Column DataIndex="GOAL" Header="GOAL" Width="40" Hidden="true">
                                </ext:Column>
                                <ext:Column DataIndex="GOALStr" Header="GOAL" Width="80">
                                </ext:Column>
                                <ext:Column DataIndex="ACT" Header="ACT" Width="40">
                                </ext:Column>
                                <ext:Column DataIndex="UNIT" Header="單位" Width="60">
                                </ext:Column>
                                <ext:Column DataIndex="ACTStr" Header="ACT" Width="60" Hidden="true">
                                </ext:Column>
                                <ext:Column DataIndex="RESULT" Header="RESULT" Width="60">
                                </ext:Column>
                                <ext:Column DataIndex="W_FILEPATH" Header="文件路徑" Width="60">
                                    <Renderer Fn="change_Attachement" />
                                </ext:Column>
                                <ext:Column DataIndex="flag" Header="是否需要附件" Width="100">
                                </ext:Column>
                                <ext:Column DataIndex="D" Header="D" Width="60">
                                </ext:Column>
                                <ext:Column DataIndex="M" Header="M" Width="60">
                                </ext:Column>
                                <ext:Column DataIndex="P" Header="P" Width="60">
                                </ext:Column>
                                <ext:Column DataIndex="E" Header="E" Width="60">
                                </ext:Column>
                                <ext:Column DataIndex="W" Header="W" Width="60">
                                </ext:Column>
                                <ext:Column DataIndex="O" Header="O" Width="60">
                                </ext:Column>
                                <ext:Column DataIndex="IMPROVEMENT_STATUS" Header="改善狀態" Width="100">
                                </ext:Column>
                                <%--<ext:Column DataIndex="Comment" Header="備註" Width="100">
                                </ext:Column>--%>
                                <ext:Column DataIndex="DESCRIPTION" Header="問題描述" Width="60">
                                </ext:Column>
                                <ext:Column DataIndex="ROOT_CAUSE" Header="原因分析" Width="100">
                                </ext:Column>
                                <ext:Column DataIndex="TEMPORARY_ACTION" Header="臨時對策" Width="60">
                                </ext:Column>
                                <ext:Column DataIndex="CORRECTIVE_PREVENTIVE_ACTION" Header="矯正預防措施" Width="100">
                                </ext:Column>
                                <ext:Column DataIndex="COMPLETE_DATE" Header="完成時間" Width="60">
                                </ext:Column>
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <DirectEvents>
                            <Command OnEvent="grdCTQInfo_RowCommand">
                                <ExtraParams>
                                    <ext:Parameter Name="ID" Value="record.data.ID" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="ACT" Value="record.data.ACT" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="DEPT" Value="record.data.DEPT" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="ACTStr" Value="record.data.ACTStr" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="GOAL" Value="record.data.GOAL" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="GOALStr" Value="record.data.GOALStr" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="flag" Value="record.data.flag" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="DESCRIPTION" Value="record.data.DESCRIPTION" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="ROOT_CAUSE" Value="record.data.ROOT_CAUSE" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="RESULT" Value="record.data.RESULT" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="DOC_NO" Value="record.data.DOC_NO" Mode="Raw" />
                                    <ext:Parameter Name="SUB_DOC_NO" Value="record.data.SUB_DOC_NO" Mode="Raw" />
                                    <ext:Parameter Name="CONTROL_TYPE" Value="record.data.CONTROL_TYPE" Mode="Raw" />
                                    <ext:Parameter Name="CTQ" Value="record.data.CTQ" Mode="Raw" />
                                    <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                </ExtraParams>
                            </Command>
                        </DirectEvents>
                        <%-- <BottomBar>
                            <ext:PagingToolbar ID="PagingToolbar2" PageSize="10" runat="server">
                            </ext:PagingToolbar>
                        </BottomBar>--%>
                    </ext:GridPanel>
                </Items>
            </ext:Panel>
            <ext:Panel ID="pnlIssueList" runat="server" Border="false" Region="Center" AutoHeight="true"
                Header="false" Padding="5" Layout="Form" Hidden="true" Collapsible="True" Frame="true"
                Title="ISSUES TRACKING LIST">
                <Items>
                    <ext:Panel ID="pnlIssueBatch" runat="server" Padding="5" Layout="Form" Frame="false"
                        Title="IssuesList" Border="false">
                        <Items>
                            <ext:Container ID="Container35" runat="server" Layout="ColumnLayout" Height="40">
                                <Items>
                                    <ext:Container ID="Container84" runat="server" Layout="FormLayout" ColumnWidth="0.25"
                                        LabelWidth="70">
                                        <Items>
                                            <ext:RadioGroup ID="rgIssueBatch" runat="server" FieldLabel="批量錄入" Width="400">
                                                <Items>
                                                    <ext:Radio ID="rdBatchIssueY" runat="server" BoxLabel="是" Width="40" />
                                                    <ext:Radio ID="rdPBatchIssueN" runat="server" BoxLabel="否" Width="40" />
                                                    <ext:Radio ID="rdPBatchIssueNA" runat="server" BoxLabel="NA" Width="40" />
                                                </Items>
                                                <DirectEvents>
                                                    <Change OnEvent="rgIssueBatch_Changed">
                                                    </Change>
                                                </DirectEvents>
                                            </ext:RadioGroup>
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="ContainerBatch" runat="server" Layout="ColumnLayout" Height="60"
                                Hidden="true">
                                <Items>
                                    <ext:Container ID="Container87" runat="server" Layout="FormLayout" ColumnWidth="0.3"
                                        LabelWidth="70">
                                        <Items>
                                            <ext:HyperLink ID="lkSample" runat="server" Text="模板下载" NavigateUrl="IssuesList.xlsx" />
                                        </Items>
                                    </ext:Container>
                                    <ext:Container ID="Container85" runat="server" Layout="FormLayout" ColumnWidth="0.3"
                                        LabelWidth="70">
                                        <Items>
                                            <ext:FileUploadField ID="IssueFileUpload" runat="server" FieldLabel="批量上傳" LabelSeparator=" "
                                                ButtonText="請選擇文件" Width="200">
                                            </ext:FileUploadField>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container ID="Container86" runat="server" Layout="FormLayout" ColumnWidth="0.2"
                                        LabelWidth="70">
                                        <Items>
                                            <ext:Button ID="btnBatchUpload" runat="server" Width="80" Text="上傳" Icon="Accept">
                                                <DirectEvents>
                                                    <Click OnEvent="btnBatchUpload_Click">
                                                    </Click>
                                                </DirectEvents>
                                            </ext:Button>
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Panel>
                    <ext:Panel ID="pnlIssueInfo" runat="server" Layout="Form" Border="false" BodyStyle="background-color: #DFE8F6;"
                        Padding="5" Collapsible="true" Hidden="true">
                        <Items>
                            <ext:TableLayout ID="TableLayout6" runat="server" Columns="2">
                                <Cells>
                                    <ext:Cell ColSpan="2">
                                        <ext:Container ID="Container50" runat="server" LabelWidth="80" Width="520">
                                            <Items>
                                                <ext:TextField ID="txtProjectStation" runat="server" Text="" Width="300" FieldLabel="站別">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell ColSpan="2">
                                        <ext:Container ID="Container96" runat="server" LabelWidth="80" Width="520">
                                            <Items>
                                                <ext:ComboBox ID="cmbIssuesDept" runat="server" FieldLabel="部門" Width="300" DisplayField="DEPT"
                                                    ValueField="DEPT" TabIndex="1">
                                                    <Store>
                                                        <ext:Store ID="Store12" runat="server">
                                                            <Reader>
                                                                <ext:JsonReader>
                                                                    <Fields>
                                                                        <ext:RecordField Name="DEPT" />
                                                                    </Fields>
                                                                </ext:JsonReader>
                                                            </Reader>
                                                        </ext:Store>
                                                    </Store>
                                                </ext:ComboBox>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell ColSpan="2">
                                        <ext:Container ID="Container26" runat="server" LabelWidth="80" Width="520">
                                            <Items>
                                                <ext:FileUploadField ID="IssuesUploadFile" runat="server" FieldLabel="圖片上傳" ButtonText=""
                                                    Icon="ImageAdd">
                                                </ext:FileUploadField>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell ColSpan="2">
                                        <ext:Container ID="Container51" runat="server" LabelWidth="80" Width="520">
                                            <Items>
                                                <ext:TextArea ID="txtDescription_Issues" runat="server" Text="" Width="900" FieldLabel="問題描述">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell ColSpan="2">
                                        <ext:Container ID="Container73" runat="server" LabelWidth="80" Width="520">
                                            <Items>
                                                <ext:TextArea ID="txtImporveMeasure" runat="server" Text="" Width="900" FieldLabel="建議改善對策">
                                                </ext:TextArea>
                                                <ext:ComboBox ID="cbClass" runat="server" FieldLabel="問題等級" Width="300">
                                                    <Items>
                                                        <ext:ListItem Text="Critical" Value="Critical" />
                                                        <ext:ListItem Text="Major" Value="Major" />
                                                        <ext:ListItem Text="Minor" Value="Minor" />
                                                    </Items>
                                                </ext:ComboBox>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell ColSpan="2">
                                        <ext:Container ID="Container48" runat="server" LabelWidth="80" Width="520">
                                            <Items>
                                                <ext:ComboBox ID="cbLosseItem" runat="server" FieldLabel="違反損失項" Width="300">
                                                    <Items>
                                                        <ext:ListItem Text="品質損失" Value="品質損失" />
                                                        <ext:ListItem Text="成本損失" Value="成本損失" />
                                                        <ext:ListItem Text="人工效率損失" Value="人工效率損失" />
                                                        <ext:ListItem Text="設備效率損失" Value="設備效率損失" />
                                                    </Items>
                                                </ext:ComboBox>
                                                <%--                                                <ext:TextArea ID="txtLosseItem" runat="server" Text="" Width="900" FieldLabel="違反損失項">
                                                </ext:TextArea>--%>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell ColSpan="2">
                                        <ext:Container ID="Container49" runat="server" LabelWidth="80" Width="520">
                                            <Items>
                                                <ext:TextArea ID="txtTempMeasure" runat="server" Text="" Width="900" FieldLabel="臨時對策">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                </Cells>
                            </ext:TableLayout>
                        </Items>
                    </ext:Panel>
                    <ext:Panel ID="pnlIssuesReply" runat="server" Layout="Form" Title="Issues List 回覆"
                        Border="false" BodyStyle="background-color: #DFE8F6;" Padding="5" Collapsible="True"
                        Hidden="true">
                        <Items>
                            <ext:TableLayout ID="TableLayout4" runat="server" Columns="3">
                                <Cells>
                                    <ext:Cell ColSpan="3">
                                        <ext:Container ID="Container25" runat="server" Width="520" LabelWidth="80">
                                            <Items>
                                                <ext:Hidden ID="hidIssuesID" runat="server">
                                                </ext:Hidden>
                                                <ext:TextField ID="ProjectStation" runat="server" FieldLabel="站別" Width="200" ReadOnly="true">
                                                </ext:TextField>
                                                <ext:TextField ID="Dept" runat="server" FieldLabel="部門" Width="200" ReadOnly="true">
                                                </ext:TextField>
                                                <ext:TextArea ID="ISSUE_DESCRIPTION" runat="server" FieldLabel="問題表述" Width="928"
                                                    ReadOnly="true">
                                                </ext:TextArea>
                                                <ext:TextArea ID="IMPROVEMEASURE" runat="server" FieldLabel="建議改善對策" Width="928"
                                                    ReadOnly="true">
                                                </ext:TextArea>
                                                <ext:TextArea ID="txtDutyDeptReply" runat="server" FieldLabel="責任部門對策回覆" Width="928">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                </Cells>
                                <Cells>
                                    <ext:Cell ColSpan="3">
                                        <ext:Container ID="Container38" runat="server" Width="520" LabelWidth="80">
                                            <Items>
                                                <ext:TextArea ID="txtIssuesCurrentStatus" runat="server" FieldLabel="結果確認" Width="928">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                </Cells>
                                <Cells>
                                    <ext:Cell ColSpan="1">
                                        <ext:Container ID="Container39" runat="server" LabelWidth="80" Width="270">
                                            <Items>
                                                <ext:ComboBox ID="cmIssuesStatus" runat="server" FieldLabel="改善狀態" Width="250">
                                                    <Items>
                                                        <ext:ListItem Text="OPEN" Value="OPEN" />
                                                        <ext:ListItem Text="CLOSED" Value="CLOSED" />
                                                        <ext:ListItem Text="Tracking" Value="Tracking" />
                                                    </Items>
                                                </ext:ComboBox>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <%--   <ext:Cell ColSpan="1">
                                        <ext:Container ID="Container40" runat="server" LabelWidth="120" Width="270">
                                            <Items>
                                                <ext:TextField ID="txtIssuesCharge" runat="server" FieldLabel="Person in Charge"
                                                    Width="250" Disabled="true">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>--%>
                                    <ext:Cell ColSpan="2">
                                        <ext:Container ID="Container66" runat="server" LabelWidth="80" Width="270">
                                            <Items>
                                                <ext:DateField ID="txtIssuesDueDay" runat="server" FieldLabel="Due Day" Width="250">
                                                </ext:DateField>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                </Cells>
                                <Cells>
                                    <ext:Cell ColSpan="3">
                                        <ext:Container ID="Container45" runat="server" Width="520" LabelWidth="80">
                                            <Items>
                                                <ext:TextArea ID="txtIssuesRemark" runat="server" FieldLabel="備註" Width="928">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                </Cells>
                                <Cells>
                                    <ext:Cell>
                                        <ext:Container ID="Container65" runat="server" Layout="Form" ColumnWidth="0.33" LabelWidth="120">
                                            <Items>
                                                <ext:Button ID="Button2" runat="server" Text="保存" Icon="Disk">
                                                    <DirectEvents>
                                                        <Click OnEvent="btnIssuesReply_Click">
                                                        </Click>
                                                    </DirectEvents>
                                                </ext:Button>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                </Cells>
                            </ext:TableLayout>
                        </Items>
                    </ext:Panel>
                    <ext:GridPanel ID="grdIssuesList" runat="server" Title="ISSUELIST" TrackMouseOver="true"
                        Frame="true" AutoScroll="true" Height="350">
                        <Store>
                            <ext:Store ID="Store4" runat="server">
                                <Reader>
                                    <ext:JsonReader>
                                        <Fields>
                                            <ext:RecordField Name="ID">
                                            </ext:RecordField>
                                            <ext:RecordField Name="PHASE">
                                            </ext:RecordField>
                                            <ext:RecordField Name="DEPT">
                                            </ext:RecordField>
                                            <ext:RecordField Name="STATION">
                                            </ext:RecordField>
                                            <ext:RecordField Name="ISSUE_DESCRIPTION">
                                            </ext:RecordField>
                                            <ext:RecordField Name="CLASS">
                                            </ext:RecordField>
                                            <ext:RecordField Name="IMPROVE_MEASURE">
                                            </ext:RecordField>
                                            <ext:RecordField Name="MEASURE_DEPTREPLY">
                                            </ext:RecordField>
                                            <ext:RecordField Name="CURRENT_STATUS">
                                            </ext:RecordField>
                                            <ext:RecordField Name="TRACKING">
                                            </ext:RecordField>
                                            <ext:RecordField Name="DUE_DAY">
                                            </ext:RecordField>
                                            <ext:RecordField Name="REMARK">
                                            </ext:RecordField>
                                            <ext:RecordField Name="ITEMS">
                                            </ext:RecordField>
                                            <ext:RecordField Name="FILENAME">
                                            </ext:RecordField>
                                            <ext:RecordField Name="FILE_PATH">
                                            </ext:RecordField>
                                            <ext:RecordField Name="PRIORITYLEVEL">
                                            </ext:RecordField>
                                            <ext:RecordField Name="ISSUE_LOSSES">
                                            </ext:RecordField>
                                            <ext:RecordField Name="TEMP_MEASURE">
                                            </ext:RecordField>
                                            <ext:RecordField Name="PERSON_IN_CHARGE">
                                            </ext:RecordField>
                                            <ext:RecordField Name="AFFIRMACE_MAN">
                                            </ext:RecordField>
                                            <ext:RecordField Name="STAUTS">
                                            </ext:RecordField>
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>
                                <ext:CommandColumn Width="80">
                                    <Commands>
                                        <%--                                        <ext:GridCommand Icon="Pencil" CommandName="Write" Text="填寫">
                                        </ext:GridCommand>
--%>
                                        <ext:GridCommand Icon="Pencil" CommandName="Reply" Text="回覆">
                                        </ext:GridCommand>
                                    </Commands>
                                    <PrepareToolbar Fn="NoteGoHid2" />
                                </ext:CommandColumn>
                                <ext:Column Header="<a>Project Phase</a><br><a>(階段)</a>" DataIndex="PHASE" Width="100">
                                </ext:Column>
                                <ext:Column Header="<a>Status</a><br><a>(狀況)</a>" DataIndex="STAUTS" Width="180">
                                    <Renderer Fn="StatusFormat" />
                                </ext:Column>
                                <ext:Column Header="<a>Project Station</a><br><a>(站別)</a>" DataIndex="STATION" Width="100">
                                </ext:Column>
                                <ext:Column Header="<a>DEPT</a><br><a>(部門)</a>" DataIndex="DEPT" Width="100">
                                </ext:Column>
                                <ext:Column Header="<a>Issue Description</a><br><a>(問題描述)</a>" DataIndex="ISSUE_DESCRIPTION"
                                    Width="180">
                                </ext:Column>
                                <ext:Column Header="<a>Issue Picture</a><br><a>(問題圖片)</a>" DataIndex="FILE_PATH"
                                    Width="180">
                                    <Renderer Fn="change_Attachement" />
                                </ext:Column>
                                <ext:Column Header="<a>Class</a><br><a>(問題等級)</a>" DataIndex="CLASS" Width="180">
                                </ext:Column>
                                <ext:Column Header="<a>Issue Losses Item</a><br><a>(問題違反損失)</a>" DataIndex="ISSUE_LOSSES"
                                    Width="150">
                                </ext:Column>
                                <ext:Column Header="<a>Temporary Countermeasure</a><br><a>(臨時對策)</a>" DataIndex="TEMP_MEASURE"
                                    Width="180">
                                </ext:Column>
                                <ext:Column Header="<a>Suggest Improve Countermeasure</a><br><a>(建議改善對策)</a>" DataIndex="IMPROVE_MEASURE"
                                    Width="180">
                                </ext:Column>
                                <ext:Column Header="<a>Reply Countermeasure</a><br><a>(責任部門對策回覆)</a>" DataIndex="MEASURE_DEPTREPLY"
                                    Width="100">
                                </ext:Column>
                                <%-- <ext:Column Header="<a>Person in Chare</a>" DataIndex="PERSON_IN_CHARGE" Width="100">
                                </ext:Column>--%>
                                <ext:Column Header="<a>Due Day</a>" DataIndex="DUE_DAY" Width="180">
                                </ext:Column>
                                <ext:Column Header="<a>Current Status</a><br><a>(對策實施狀況及確認結果)</a>" DataIndex="CURRENT_STATUS"
                                    Width="180">
                                </ext:Column>
                                <%-- <ext:Column Header="<a>Affirmance Man</a><br><a>(確認人)</a>" DataIndex="LINK_URL" Width="150">
                                </ext:Column>--%>
                                <ext:Column Header="<a>Status</a><br><a>(改善狀況)</a>" DataIndex="TRACKING" Width="180">
                                </ext:Column>
                                <ext:Column Header="<a>Remark</a><br><a>(備註)</a>" DataIndex="REMARK" Width="180">
                                </ext:Column>
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" runat="server">
                            </ext:CheckboxSelectionModel>
                        </SelectionModel>
                        <TopBar>
                            <ext:Toolbar ID="Toolbar1" runat="server">
                                <Items>
                                    <ext:Button ID="btnAdd" runat="server" Text="新增" Icon="Add">
                                        <DirectEvents>
                                            <Click OnEvent="btnAdd_Click">
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button ID="btnModifySave" runat="server" Text="保存" Icon="Add" Hidden="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnModifySave_Click">
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button ID="btnDelete" runat="server" Text="删除" Icon="Delete">
                                        <DirectEvents>
                                            <Click OnEvent="btnDelete_Click">
                                                <Confirmation ConfirmRequest="true" Message="確認刪除勾選的記錄?" Title="提示" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{grdIssuesList}.getRowsValues({selectedOnly:true}))"
                                                        Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button ID="btnModify" runat="server" Text="修改" Icon="Delete">
                                        <DirectEvents>
                                            <Click OnEvent="btnModify_Click">
                                                <Confirmation ConfirmRequest="true" Message="確認修改勾選的記錄?" Title="提示" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{grdIssuesList}.getRowsValues({selectedOnly:true}))"
                                                        Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button ID="btnIssuesExport" runat="server" Text="導出Excel" Icon="PageExcel" AutoPostBack="true"
                                        Hidden="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnIssuesExport_click">
                                                <ExtraParams>
                                                    <ext:Parameter Name="values" Mode="Raw" Value="Ext.encode(#{grdIssuesList}.getRowsValues({selectedOnly:false}))">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button ID="btnUploadIssues" runat="server" Text="批量上傳" Icon="DatabaseAdd" Hidden="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnUploadIssues_Click">
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <DirectEvents>
                            <Command OnEvent="grdIssuesInfo_RowCommand">
                                <ExtraParams>
                                    <ext:Parameter Name="ID" Value="record.data.ID" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="STATION" Value="record.data.STATION" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="DEPT" Value="record.data.DEPT" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="MEASURE_DEPTREPLY" Value="record.data.MEASURE_DEPTREPLY" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="CURRENT_STATUS" Value="record.data.CURRENT_STATUS" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="TRACKING" Value="record.data.TRACKING" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="REMARK" Value="record.data.REMARK" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="DUE_DAY" Value="record.data.DUE_DAY" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="ISSUE_DESCRIPTION" Value="record.data.ISSUE_DESCRIPTION" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="IMPROVE_MEASURE" Value="record.data.IMPROVE_MEASURE" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                </ExtraParams>
                            </Command>
                        </DirectEvents>
                    </ext:GridPanel>
                </Items>
            </ext:Panel>
            <ext:Panel ID="pnlFMEAList" runat="server" Border="false" Region="Center" AutoHeight="true"
                Title="FMEA List" Header="false" Padding="5" Layout="Form" Frame="true" Hidden="true"
                Collapsible="True">
                <Items>
                    <ext:Panel ID="pnlFMEABatch" runat="server" Padding="0" Layout="Form" Frame="false"
                        Title="FMEA LIST">
                        <Items>
                            <ext:Container ID="Container88" runat="server" Layout="ColumnLayout" Height="40">
                                <Items>
                                    <ext:Container ID="Container89" runat="server" Layout="FormLayout" ColumnWidth="0.25"
                                        LabelWidth="70">
                                        <Items>
                                            <ext:RadioGroup ID="rgFMEABatch" runat="server" FieldLabel="批量錄入" Width="400">
                                                <Items>
                                                    <ext:Radio ID="rdFMEABatchY" runat="server" BoxLabel="是" Width="40" />
                                                    <ext:Radio ID="rdFMEABatchN" runat="server" BoxLabel="否" Width="40" />
                                                    <ext:Radio ID="rdFMEABatchNA" runat="server" BoxLabel="NA" Width="40" />
                                                </Items>
                                                <DirectEvents>
                                                    <Change OnEvent="rgFMEABatch_Changed">
                                                    </Change>
                                                </DirectEvents>
                                            </ext:RadioGroup>
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="ContainerFBatch" runat="server" Layout="ColumnLayout" Height="60"
                                Hidden="true">
                                <Items>
                                    <ext:Container ID="Container94" runat="server" Layout="FormLayout" ColumnWidth="0.2"
                                        LabelWidth="80">
                                        <Items>
                                            <ext:HyperLink ID="HyperLink1" runat="server" Text="模板下载" NavigateUrl="P-FMA.xlsx" />
                                        </Items>
                                    </ext:Container>
                                    <ext:Container ID="Container92" runat="server" Layout="FormLayout" ColumnWidth="0.3"
                                        LabelWidth="70">
                                        <Items>
                                            <ext:FileUploadField ID="FMEABatchUpload" runat="server" FieldLabel="批量上傳" LabelSeparator=" "
                                                ButtonText="請選擇文件" Width="200">
                                            </ext:FileUploadField>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container ID="Container93" runat="server" Layout="FormLayout" ColumnWidth="0.2"
                                        LabelWidth="70">
                                        <Items>
                                            <ext:Button ID="btnFBatchUpload" runat="server" Width="80" Text="上傳" Icon="Accept">
                                                <DirectEvents>
                                                    <Click OnEvent="btnFMEABatchUpload_Click">
                                                    </Click>
                                                </DirectEvents>
                                            </ext:Button>
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Panel>
                    <ext:Panel ID="pnlFMEAWrite" runat="server" Layout="Form" Border="false" BodyStyle="background-color: #DFE8F6;"
                        Padding="5" Title="P-FMEA" Hidden="true">
                        <Items>
                            <ext:TableLayout ID="TableLayout5" runat="server" Columns="4">
                                <Cells>
                                    <ext:Cell ColSpan="1">
                                        <ext:Container ID="Container43" runat="server" Width="220" LabelWidth="80">
                                            <Items>
                                                <ext:TextField ID="txtFMEAStation" runat="server" Text="" Width="200" FieldLabel="工位">
                                                    <DirectEvents>
                                                        <Change OnEvent="txtFMEAStation_Changed">
                                                        </Change>
                                                    </DirectEvents>
                                                </ext:TextField>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell ColSpan="1">
                                        <ext:Container ID="Container69" runat="server" Width="220" LabelWidth="80">
                                            <Items>
                                                <ext:TextField ID="txtFMEAItem" runat="server" Text="" Width="200" FieldLabel="編號"
                                                    ReadOnly="true">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell ColSpan="2">
                                        <ext:Container ID="Container97" runat="server" Width="220" LabelWidth="80">
                                            <Items>
                                                <ext:ComboBox ID="cmbPFMADept" runat="server" FieldLabel="部門" Width="200" DisplayField="DEPT"
                                                    ValueField="DEPT" TabIndex="1">
                                                    <Store>
                                                        <ext:Store ID="Store13" runat="server">
                                                            <Reader>
                                                                <ext:JsonReader>
                                                                    <Fields>
                                                                        <ext:RecordField Name="DEPT" />
                                                                    </Fields>
                                                                </ext:JsonReader>
                                                            </Reader>
                                                        </ext:Store>
                                                    </Store>
                                                </ext:ComboBox>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                </Cells>
                                <Cells>
                                    <ext:Cell ColSpan="4">
                                        <ext:Container ID="Container44" runat="server" Width="520" LabelWidth="80">
                                            <Items>
                                                <ext:TextArea ID="txtFMEASource" runat="server" Text="" Width="928" FieldLabel="問題描述">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell ColSpan="4">
                                        <ext:Container ID="Container70" runat="server" Width="520" LabelWidth="80">
                                            <Items>
                                                <ext:TextArea ID="txtFMEAMode" runat="server" Text="" Width="928" FieldLabel="潛在失效模式">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell ColSpan="4">
                                        <ext:Container ID="Container71" runat="server" Width="520" LabelWidth="80">
                                            <Items>
                                                <ext:ComboBox ID="cbFMEALosseItem" runat="server" FieldLabel="違反損失項" Width="300">
                                                    <Items>
                                                        <ext:ListItem Text="品質損失" Value="品質損失" />
                                                        <ext:ListItem Text="成本損失" Value="成本損失" />
                                                        <ext:ListItem Text="人工效率損失" Value="人工效率損失" />
                                                        <ext:ListItem Text="設備效率損失" Value="設備效率損失" />
                                                    </Items>
                                                </ext:ComboBox>
                                                <%--                                                <ext:TextArea ID="txtFMEALosseItem" runat="server" Text="" Width="928" FieldLabel="違反損失項">
                                                </ext:TextArea>--%>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                </Cells>
                                <Cells>
                                    <ext:Cell ColSpan="1">
                                        <ext:Container ID="Container46" runat="server" LabelWidth="80" Width="200">
                                            <Items>
                                                <ext:NumberField ID="txtFMEASev" runat="server" Text="" Width="180" FieldLabel="嚴重度">
                                                    <DirectEvents>
                                                        <Change OnEvent="Clac_PRN">
                                                        </Change>
                                                    </DirectEvents>
                                                </ext:NumberField>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell ColSpan="1">
                                        <ext:Container ID="Container47" runat="server" LabelWidth="80" Width="200">
                                            <Items>
                                                <ext:NumberField ID="txtFMEAOcc" runat="server" Text="" Width="180" FieldLabel="發生度">
                                                    <DirectEvents>
                                                        <Change OnEvent="Clac_PRN">
                                                        </Change>
                                                    </DirectEvents>
                                                </ext:NumberField>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell ColSpan="1">
                                        <ext:Container ID="Container57" runat="server" LabelWidth="80" Width="200">
                                            <Items>
                                                <ext:NumberField ID="txtFMEADet" runat="server" Text="" Width="180" FieldLabel="偵測度">
                                                    <DirectEvents>
                                                        <Change OnEvent="Clac_PRN">
                                                        </Change>
                                                    </DirectEvents>
                                                </ext:NumberField>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell ColSpan="1">
                                        <ext:Container ID="Container72" runat="server" LabelWidth="80" Width="200">
                                            <Items>
                                                <ext:NumberField ID="txtFMEAPRN" runat="server" Text="" Width="180" FieldLabel="風險優先度"
                                                    ReadOnly="true" MinValue="0">
                                                </ext:NumberField>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                </Cells>
                                <Cells>
                                    <ext:Cell ColSpan="4">
                                        <ext:Container ID="Container68" runat="server" Layout="Form" LabelWidth="80">
                                            <Items>
                                                <ext:TextArea ID="txtFMEACauses" runat="server" Text="" Width="928" FieldLabel="建議改善對策">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                </Cells>
                                <Cells>
                                    <%--                                    <ext:Cell ColSpan="2">
                                        <ext:Container ID="Container67" runat="server" Width="200" LabelWidth="80">
                                            <Items>
                                                <ext:DateField ID="txtFMEAComplete" runat="server" FieldLabel="目標完成時間" Width="200">
                                                </ext:DateField>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
--%>
                                    <ext:Cell ColSpan="2">
                                        <ext:Container ID="Container27" runat="server" Width="300" LabelWidth="80">
                                            <Items>
                                                <ext:FileUploadField ID="FMEAUploadFile" runat="server" FieldLabel="圖片上傳" ButtonText=""
                                                    Icon="ImageAdd">
                                                </ext:FileUploadField>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                </Cells>
                            </ext:TableLayout>
                        </Items>
                    </ext:Panel>
                    <ext:Panel ID="pnlPFMAReply" runat="server" Layout="Form" Title="PFMA回覆" Border="false"
                        BodyStyle="background-color: #DFE8F6;" Padding="5" Collapsible="True" Hidden="true">
                        <Items>
                            <ext:TableLayout ID="TableLayout7" runat="server" Columns="4">
                                <Cells>
                                    <ext:Cell ColSpan="4">
                                        <ext:Container ID="Container28" runat="server" Width="220" LabelWidth="100">
                                            <Items>
                                                <ext:Hidden ID="hidPFMEAID" runat="server">
                                                </ext:Hidden>
                                                <ext:TextField ID="FMEAItem" runat="server" FieldLabel="編號" Width="500" ReadOnly="true">
                                                </ext:TextField>
                                                <ext:TextField ID="FMEAStantion" runat="server" FieldLabel="工位" Width="500" ReadOnly="true">
                                                </ext:TextField>
                                                <ext:TextArea ID="FMEASource" runat="server" FieldLabel="問題描述" Width="928" ReadOnly="true">
                                                </ext:TextArea>
                                                <ext:TextArea ID="FMEAPotentialFailure" runat="server" FieldLabel="建議改善措施" Width="928"
                                                    ReadOnly="true">
                                                </ext:TextArea>
                                                <ext:TextArea ID="FMEAPotentialFailureMode" runat="server" FieldLabel="潛在失效模式" Width="928"
                                                    ReadOnly="true">
                                                </ext:TextArea>
                                                <ext:TextArea ID="txtPReplyActionTaken" runat="server" FieldLabel="實際執行之對策" Width="928">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                </Cells>
                                <Cells>
                                    <ext:Cell ColSpan="1">
                                        <ext:Container ID="Container77" runat="server" LabelWidth="120" Width="200">
                                            <Items>
                                                <ext:NumberField ID="txtPReplySev" runat="server" Text="" Width="180" FieldLabel=" 改善後嚴重(S)"
                                                    MinValue="0">
                                                    <DirectEvents>
                                                        <Change OnEvent="Clac_PRN2">
                                                        </Change>
                                                    </DirectEvents>
                                                </ext:NumberField>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell ColSpan="1">
                                        <ext:Container ID="Container79" runat="server" LabelWidth="120" Width="200">
                                            <Items>
                                                <ext:NumberField ID="txtPReplyOcc" runat="server" Text="" Width="180" FieldLabel="改善後發生率（O）"
                                                    MinValue="0">
                                                    <DirectEvents>
                                                        <Change OnEvent="Clac_PRN2">
                                                        </Change>
                                                    </DirectEvents>
                                                </ext:NumberField>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell ColSpan="1">
                                        <ext:Container ID="Container80" runat="server" LabelWidth="120" Width="200">
                                            <Items>
                                                <ext:NumberField ID="txtPReplyDet" runat="server" Text="" Width="180" FieldLabel=" 改善後偵測度(D)"
                                                    MinValue="0">
                                                    <DirectEvents>
                                                        <Change OnEvent="Clac_PRN2">
                                                        </Change>
                                                    </DirectEvents>
                                                </ext:NumberField>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell ColSpan="1">
                                        <ext:Container ID="Container81" runat="server" LabelWidth="120" Width="200">
                                            <Items>
                                                <ext:NumberField ID="txtPReplyRPN" runat="server" Text="" Width="180" FieldLabel="改善後RPN "
                                                    ReadOnly="true" MinValue="0">
                                                </ext:NumberField>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                </Cells>
                                <Cells>
                                    <ext:Cell ColSpan="2">
                                        <ext:Container ID="Container82" runat="server" Layout="Form" LabelWidth="80">
                                            <Items>
                                                <ext:ComboBox ID="txtPReplyResposibility" runat="server" FieldLabel="處理狀態" Width="120">
                                                    <Items>
                                                        <ext:ListItem Text="OPEN" Value="OPEN" />
                                                        <ext:ListItem Text="CLOSED" Value="CLOSED" />
                                                        <ext:ListItem Text="Tracking" Value="Tracking" />
                                                    </Items>
                                                </ext:ComboBox>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                    <ext:Cell ColSpan="2">
                                        <ext:Container ID="Container40" runat="server" Layout="Form" LabelWidth="80">
                                            <Items>
                                                <ext:DateField ID="txtPReplyCompletDate" runat="server" FieldLabel="目標完成時間" Width="120">
                                                </ext:DateField>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                </Cells>
                                <Cells>
                                    <ext:Cell ColSpan="4">
                                        <ext:Container ID="Container83" runat="server" Width="200" LabelWidth="80">
                                            <Items>
                                                <ext:Button ID="Button1" runat="server" Text="保存" Icon="Disk">
                                                    <DirectEvents>
                                                        <Click OnEvent="btnPReply_Click">
                                                        </Click>
                                                    </DirectEvents>
                                                </ext:Button>
                                            </Items>
                                        </ext:Container>
                                    </ext:Cell>
                                </Cells>
                            </ext:TableLayout>
                        </Items>
                    </ext:Panel>
                    <ext:GridPanel ID="grdPFMAList" runat="server" TrackMouseOver="true" Frame="true"
                        AutoScroll="true" Height="350" Title="FMEALIST">
                        <Store>
                            <ext:Store ID="Store10" runat="server">
                                <Reader>
                                    <ext:JsonReader>
                                        <Fields>
                                            <ext:RecordField Name="ID">
                                            </ext:RecordField>
                                            <ext:RecordField Name="SubNo">
                                            </ext:RecordField>
                                            <ext:RecordField Name="Item">
                                            </ext:RecordField>
                                            <ext:RecordField Name="WriteDept">
                                            </ext:RecordField>
                                            <ext:RecordField Name="Stantion">
                                            </ext:RecordField>
                                            <ext:RecordField Name="Source">
                                            </ext:RecordField>
                                            <ext:RecordField Name="PotentialFailureMode">
                                            </ext:RecordField>
                                            <ext:RecordField Name="PotentialFailure">
                                            </ext:RecordField>
                                            <ext:RecordField Name="ActionsTaken">
                                            </ext:RecordField>
                                            <ext:RecordField Name="ResultsSev">
                                            </ext:RecordField>
                                            <ext:RecordField Name="ResultsOcc">
                                            </ext:RecordField>
                                            <ext:RecordField Name="ResultsDet">
                                            </ext:RecordField>
                                            <ext:RecordField Name="Resposibility">
                                            </ext:RecordField>
                                            <ext:RecordField Name="TargetCompletionDate" Type="Date">
                                            </ext:RecordField>
                                            <ext:RecordField Name="Loess">
                                            </ext:RecordField>
                                            <ext:RecordField Name="Sev">
                                            </ext:RecordField>
                                            <ext:RecordField Name="Occ">
                                            </ext:RecordField>
                                            <ext:RecordField Name="CurrentControls">
                                            </ext:RecordField>
                                            <ext:RecordField Name="DET">
                                            </ext:RecordField>
                                            <ext:RecordField Name="RPN">
                                            </ext:RecordField>
                                            <ext:RecordField Name="RecommendedAction">
                                            </ext:RecordField>
                                            <ext:RecordField Name="ResultsRPN">
                                            </ext:RecordField>
                                            <ext:RecordField Name="ReplyDept">
                                            </ext:RecordField>
                                            <ext:RecordField Name="Status">
                                            </ext:RecordField>
                                            <ext:RecordField Name="Update_User">
                                            </ext:RecordField>
                                            <ext:RecordField Name="Update_Time">
                                            </ext:RecordField>
                                            <ext:RecordField Name="FILE_PATH">
                                            </ext:RecordField>
                                            <ext:RecordField Name="ID">
                                            </ext:RecordField>
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>
                                <ext:CommandColumn Width="80">
                                    <Commands>
                                        <ext:GridCommand Icon="Pencil" CommandName="Reply" Text="回覆">
                                        </ext:GridCommand>
                                    </Commands>
                                    <PrepareToolbar Fn="NoteGoHid2" />
                                </ext:CommandColumn>
                                <ext:Column Header="ID" DataIndex="ID" Width="100">
                                </ext:Column>
                                <ext:Column Header="編號" DataIndex="Item" Width="100">
                                </ext:Column>
                                <ext:Column Header="工位" DataIndex="Stantion" Width="180">
                                </ext:Column>
                                <ext:Column Header="狀態" DataIndex="Status" Width="100">
                                    <Renderer Fn="StatusFormat" />
                                </ext:Column>
                                <ext:Column Header="部門" DataIndex="WriteDept" Width="180">
                                </ext:Column>
                                <ext:Column Header="問題描述" DataIndex="Source" Width="100">
                                </ext:Column>
                                <ext:Column Header="問題圖片" DataIndex="FILE_PATH" Width="100">
                                    <Renderer Fn="change_Attachement" />
                                </ext:Column>
                                <ext:Column Header="潛在失效模式" DataIndex="PotentialFailureMode" Width="180">
                                </ext:Column>
                                <ext:Column Header="違反損失" DataIndex="Loess" Width="100">
                                </ext:Column>
                                <ext:Column Header="嚴重度" DataIndex="Sev" Width="100">
                                </ext:Column>
                                <%-- <ext:Column Header="失效原因" DataIndex="PotentialFailure" Width="100">
                                </ext:Column>--%>
                                <ext:Column Header="發生度" DataIndex="Occ" Width="100">
                                </ext:Column>
                                <%-- <ext:Column Header="目前控制計劃" DataIndex="CurrentControls" Width="180">
                                </ext:Column>--%>
                                <ext:Column Header="偵測度" DataIndex="DET" Width="150">
                                </ext:Column>
                                <ext:Column Header="風險優先度" DataIndex="RPN" Width="180">
                                </ext:Column>
                                <ext:Column Header="建議改善措施" DataIndex="PotentialFailure" Width="150">
                                </ext:Column>
                                <%--  <ext:Column Header="負責人員" DataIndex="Resposibility" Width="100">
                                </ext:Column>--%>
                                <ext:DateColumn Header="目標完成日期" DataIndex="TargetCompletionDate" Width="100" Format="yyyy/MM/dd">
                                </ext:DateColumn>
                                <ext:Column Header="實際執行之對策" DataIndex="ActionsTaken" Width="180">
                                </ext:Column>
                                <ext:Column Header="改善后嚴重度" DataIndex="ResultsSev" Width="150">
                                </ext:Column>
                                <ext:Column Header="改善后發生度" DataIndex="ResultsOcc" Width="180">
                                </ext:Column>
                                <ext:Column Header="改善后偵測度" DataIndex="ResultsDet" Width="150">
                                </ext:Column>
                                <ext:Column Header="改善后風險優先度" DataIndex="ResultsRPN" Width="150">
                                </ext:Column>
                                <ext:Column Header="處理狀態" DataIndex="Resposibility" Width="180">
                                </ext:Column>
                                <%-- <ext:Column Header="產品發展階段" DataIndex="ISSUE_LOSSES" Width="150">
                                </ext:Column>--%>
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:CheckboxSelectionModel ID="CheckboxSelectionModel3" runat="server">
                            </ext:CheckboxSelectionModel>
                        </SelectionModel>
                        <DirectEvents>
                            <Command OnEvent="grdPFMA_RowCommand">
                                <ExtraParams>
                                    <ext:Parameter Name="ID" Value="record.data.ID" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="SubNo" Value="record.data.SubNo" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="Item" Value="record.data.Item" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="Stantion" Value="record.data.Stantion" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="Source" Value="record.data.Source" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="PotentialFailureMode" Value="record.data.PotentialFailureMode"
                                        Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="ActionsTaken" Value="record.data.ActionsTaken" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="ResultsSev" Value="record.data.ResultsSev" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="ResultsOcc" Value="record.data.ResultsOcc" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="ResultsDet" Value="record.data.ResultsDet" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="ResultsRPN" Value="record.data.ResultsRPN" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="TargetCompletionDate" Value="record.data.TargetCompletionDate"
                                        Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="PotentialFailure" Value="record.data.PotentialFailure" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="Resposibility" Value="record.data.Resposibility" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="command" Value="command" Mode="Raw" />
                                </ExtraParams>
                            </Command>
                        </DirectEvents>
                        <TopBar>
                            <ext:Toolbar ID="Toolbar4" runat="server">
                                <Items>
                                    <ext:Button ID="btnPFMAAdd" runat="server" Text="新增" Icon="Add">
                                        <DirectEvents>
                                            <Click OnEvent="btnPAdd_Click">
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button ID="btnFModifySave" runat="server" Text="保存" Icon="Add" Hidden="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnFModifySave_Click">
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button ID="btnPFMADelete" runat="server" Text="删除" Icon="Delete">
                                        <DirectEvents>
                                            <Click OnEvent="btnPFMADelete_Click">
                                                <Confirmation ConfirmRequest="true" Message="確認刪除勾選的記錄?" Title="提示" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{grdPFMAList}.getRowsValues                                                         ({selectedOnly:true}))"
                                                        Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button ID="btnFModify" runat="server" Text="修改" Icon="Delete">
                                        <DirectEvents>
                                            <Click OnEvent="btnFModify_Click">
                                                <Confirmation ConfirmRequest="true" Message="確認修改勾選的記錄?" Title="提示" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{grdPFMAList}.getRowsValues({selectedOnly:true}))"
                                                        Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button ID="btnPFMAExport" runat="server" Text="導出Excel" Icon="PageExcel" AutoPostBack="true"
                                        Hidden="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnPFMAExport_click">
                                                <ExtraParams>
                                                    <ext:Parameter Name="values" Mode="Raw" Value="Ext.encode(#{grdPFMAList}.getRowsValues({selectedOnly:false}))">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button ID="btnUploadPFMA" runat="server" Text="批量上傳" Icon="DatabaseAdd" Hidden="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnUploadPFMA_Click">
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                    </ext:GridPanel>
                </Items>
            </ext:Panel>
            <ext:Panel ID="pnlPrepared" runat="server" Border="false" Region="Center" AutoHeight="true"
                Title="IssueList" Header="false" Padding="5" Layout="Form" BodyStyle="background-color: transparent;"
                Collapsible="True" Hidden="true">
                <Items>
                    <ext:GridPanel ID="grdDFXInconformity" runat="server" Title="DFX不符合項" TrackMouseOver="true"
                        Frame="true" AutoScroll="true" AutoHeight="true" Collapsible="True">
                        <Store>
                            <ext:Store ID="Store5" runat="server">
                                <Reader>
                                    <ext:JsonReader>
                                        <Fields>
                                            <ext:RecordField Name="ID">
                                            </ext:RecordField>
                                            <ext:RecordField Name="Item" />
                                            <ext:RecordField Name="ItemName" />
                                            <ext:RecordField Name="Requirements" />
                                            <ext:RecordField Name="PriorityLevel" />
                                            <ext:RecordField Name="RPN" />
                                            <ext:RecordField Name="WriteDept" />
                                            <ext:RecordField Name="Compliance" />
                                            <ext:RecordField Name="Actions" />
                                            <ext:RecordField Name="CompletionDate" />
                                            <ext:RecordField Name="Tracking" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>
                                <ext:Column Header="部門" DataIndex="WriteDept" Width="60">
                                </ext:Column>
                                <ext:Column Header="條款" DataIndex="Item" Width="100">
                                </ext:Column>
                                <ext:Column Header="Requirements" DataIndex="Requirements" Width="250">
                                </ext:Column>
                                <ext:Column Header="Compliance" DataIndex="Compliance" Width="80">
                                </ext:Column>
                                <ext:Column Header="PriorityLevel" DataIndex="PriorityLevel" Width="80">
                                </ext:Column>
                                <ext:Column Header="改善對策" DataIndex="Actions" Width="180">
                                </ext:Column>
                                <ext:DateColumn Header="完成時間" DataIndex="CompletionDate" Width="150" Format="yyyy/MM/dd">
                                </ext:DateColumn>
                                <ext:Column Header="改善狀態" DataIndex="Tracking" Width="180">
                                </ext:Column>
                            </Columns>
                        </ColumnModel>
                    </ext:GridPanel>
                    <ext:GridPanel ID="grdCLCAInconformity" runat="server" Title="CLCA OPEN項" TrackMouseOver="true"
                        Frame="true" AutoScroll="true" AutoHeight="true" Collapsible="True">
                        <Store>
                            <ext:Store ID="Store6" runat="server">
                                <Reader>
                                    <ext:JsonReader>
                                        <Fields>
                                            <ext:RecordField Name="DEPT" />
                                            <ext:RecordField Name="PROCESS" />
                                            <ext:RecordField Name="CTQ" />
                                            <ext:RecordField Name="ACT" />
                                            <ext:RecordField Name="DESCRIPTION" />
                                            <ext:RecordField Name="CORRECTIVE_PREVENTIVE_ACTION" />
                                            <ext:RecordField Name="IMPROVEMENT_STATUS" />
                                            <ext:RecordField Name="RESULT" />
                                            <ext:RecordField Name="ROOT_CAUSE" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>
                                <ext:Column Header="部門" DataIndex="DEPT" Width="60">
                                </ext:Column>
                                <ext:Column Header="製程" DataIndex="PROCESS" Width="100">
                                </ext:Column>
                                <ext:Column Header="CTQ項目" DataIndex="CTQ" Width="180">
                                </ext:Column>
                                <ext:Column Header="ACT" DataIndex="ACT" Width="60">
                                </ext:Column>
                                <ext:Column Header="Result" DataIndex="RESULT" Width="60">
                                </ext:Column>
                                <ext:Column Header="原因分析" DataIndex="ROOT_CAUSE" Width="170">
                                </ext:Column>
                                <ext:Column Header="矯正預防措施" DataIndex="CORRECTIVE_PREVENTIVE_ACTION" Width="170">
                                </ext:Column>
                                <ext:Column Header="改善狀態" DataIndex="IMPROVEMENT_STATUS" Width="150">
                                </ext:Column>
                            </Columns>
                        </ColumnModel>
                    </ext:GridPanel>
                    <ext:GridPanel ID="grdFMEAInconformity" runat="server" Title="FMEA項" TrackMouseOver="true"
                        Frame="true" AutoScroll="true" AutoHeight="true" Collapsible="True">
                        <Store>
                            <ext:Store ID="Store11" runat="server">
                                <Reader>
                                    <ext:JsonReader>
                                        <Fields>
                                            <ext:RecordField Name="ID">
                                            </ext:RecordField>
                                            <ext:RecordField Name="SubNo">
                                            </ext:RecordField>
                                            <ext:RecordField Name="Item">
                                            </ext:RecordField>
                                            <ext:RecordField Name="Source">
                                            </ext:RecordField>
                                            <ext:RecordField Name="Stantion">
                                            </ext:RecordField>
                                            <ext:RecordField Name="PotentialFailureMode">
                                            </ext:RecordField>
                                            <ext:RecordField Name="Loess">
                                            </ext:RecordField>
                                            <ext:RecordField Name="RPN">
                                            </ext:RecordField>
                                            <ext:RecordField Name="ResultsSev">
                                            </ext:RecordField>
                                            <ext:RecordField Name="ResultsOcc">
                                            </ext:RecordField>
                                            <ext:RecordField Name="ResultsDet">
                                            </ext:RecordField>
                                            <ext:RecordField Name="ResultsRPN">
                                            </ext:RecordField>
                                            <ext:RecordField Name="WriteDept">
                                            </ext:RecordField>
                                            <ext:RecordField Name="ReplyDept">
                                            </ext:RecordField>
                                            <ext:RecordField Name="Status">
                                            </ext:RecordField>
                                            <ext:RecordField Name="Update_User">
                                            </ext:RecordField>
                                            <ext:RecordField Name="Update_Time">
                                            </ext:RecordField>
                                            <ext:RecordField Name="FILE_PATH">
                                            </ext:RecordField>
                                            <ext:RecordField Name="ActionsTaken">
                                            </ext:RecordField>
                                            <ext:RecordField Name="WriteDept">
                                            </ext:RecordField>
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>
                                <ext:Column Header="部門" DataIndex="WriteDept" Width="60">
                                </ext:Column>
                                <ext:Column Header="工位" DataIndex="Stantion" Width="180">
                                </ext:Column>
                                <ext:Column Header="潛在失效模式" DataIndex="PotentialFailureMode" Width="180">
                                </ext:Column>
                                <ext:Column Header="風險優先度" DataIndex="RPN" Width="180">
                                </ext:Column>
                                <ext:Column Header="實際執行之對策" DataIndex="ActionsTaken" Width="180">
                                </ext:Column>
                                <ext:Column Header="改善后風險優先度" DataIndex="ResultsRPN" Width="180">
                                </ext:Column>
                                <%--<ext:Column Header="處理狀態" DataIndex="ISSUE_DESCRIPTION" Width="180">
                                </ext:Column>--%>
                            </Columns>
                        </ColumnModel>
                    </ext:GridPanel>
                    <ext:GridPanel ID="grdIssuesInconformity" runat="server" Title="Issue List Open項"
                        TrackMouseOver="true" Frame="true" AutoScroll="true" AutoHeight="true" Collapsible="True">
                        <Store>
                            <ext:Store ID="Store7" runat="server">
                                <Reader>
                                    <ext:JsonReader>
                                        <Fields>
                                            <ext:RecordField Name="ID">
                                            </ext:RecordField>
                                            <ext:RecordField Name="PHASE">
                                            </ext:RecordField>
                                            <ext:RecordField Name="STATION">
                                            </ext:RecordField>
                                            <ext:RecordField Name="ITEMS">
                                            </ext:RecordField>
                                            <ext:RecordField Name="FILENAME">
                                            </ext:RecordField>
                                            <ext:RecordField Name="FILE_PATH">
                                            </ext:RecordField>
                                            <ext:RecordField Name="PRIORITYLEVEL">
                                            </ext:RecordField>
                                            <ext:RecordField Name="ISSUE_DESCRIPTION">
                                            </ext:RecordField>
                                            <ext:RecordField Name="ISSUE_LOSSES">
                                            </ext:RecordField>
                                            <ext:RecordField Name="TEMP_MEASURE">
                                            </ext:RecordField>
                                            <ext:RecordField Name="IMPROVE_MEASURE">
                                            </ext:RecordField>
                                            <ext:RecordField Name="PERSON_IN_CHARGE">
                                            </ext:RecordField>
                                            <ext:RecordField Name="DUE_DAY">
                                            </ext:RecordField>
                                            <ext:RecordField Name="CURRENT_STATUS">
                                            </ext:RecordField>
                                            <ext:RecordField Name="AFFIRMACE_MAN">
                                            </ext:RecordField>
                                            <ext:RecordField Name="STAUTS">
                                            </ext:RecordField>
                                            <ext:RecordField Name="TRACKING">
                                            </ext:RecordField>
                                            <ext:RecordField Name="REMARK">
                                            </ext:RecordField>
                                            <ext:RecordField Name="DEPT">
                                            </ext:RecordField>
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>
                                <ext:Column Header="部門" DataIndex="DEPT" Width="60">
                                </ext:Column>
                                <ext:Column Header="站別" DataIndex="STATION" Width="100">
                                </ext:Column>
                                <ext:Column Header="問題描述" DataIndex="ISSUE_DESCRIPTION" Width="180">
                                </ext:Column>
                                <ext:Column Header="對策回覆" DataIndex="IMPROVE_MEASURE" Width="180">
                                </ext:Column>
                                <ext:Column Header="改善狀況" DataIndex="TRACKING" Width="180">
                                </ext:Column>
                            </Columns>
                        </ColumnModel>
                    </ext:GridPanel>
                    <ext:FieldSet ID="FieldSetUploadIssues" runat="server" Title="附件上傳(Issue List)" Layout="Form"
                        AutoHeight="true" Hidden="true">
                        <Items>
                            <ext:Container ID="ConAttachment" runat="server" Layout="ColumnLayout" Height="50"
                                LabelWidth="60">
                                <Items>
                                    <ext:Container runat="server" ID="Container104" Layout="Form" ColumnWidth="0.5" LabelWidth="200">
                                        <Items>
                                            <ext:FileUploadField ID="fileMeeting" runat="server" FieldLabel="附件上传(請用繁體或英文命名)"
                                                ButtonText="選擇文件" Width="200">
                                            </ext:FileUploadField>
                                            <ext:Button ID="Button3" runat="server" Text="上傳" Icon="Accept">
                                                <DirectEvents>
                                                    <Click OnEvent="btnConfirm_Click_Issues">
                                                    </Click>
                                                </DirectEvents>
                                            </ext:Button>
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:FieldSet>
                    <ext:GridPanel ID="grdAttachmentIssue" runat="server" Frame="true" StripeRows="true"
                        TrackMouseOver="true" AutoHeight="true" Header="true" Title="Issue List附件" Hidden="true">
                        <TopBar>
                            <ext:Toolbar ID="Toolbar6" runat="server">
                                <Items>
                                    <ext:Button runat="server" ID="btnDelVQM" Text="删除" Icon="Delete">
                                        <DirectEvents>
                                            <Click OnEvent="btnDelete_ClickVQM">
                                                <Confirmation ConfirmRequest="true" Message="確認刪除勾選的記錄?" Title="提示" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{grdAttachmentIssue}.getRowsValues({selectedOnly:true}))"
                                                        Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Store>
                            <ext:Store ID="Store15" runat="server">
                                <Reader>
                                    <ext:JsonReader>
                                        <Fields>
                                            <ext:RecordField Name="SUB_DOC_NO" />
                                            <ext:RecordField Name="FILE_PATH" Type="String" />
                                            <ext:RecordField Name="FILE_NAME" Type="String" />
                                            <ext:RecordField Name="APPROVER" Type="String" />
                                            <ext:RecordField Name="UPDATE_TIME" Type="Date" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                            </ext:Store>
                        </Store>
                        <ColumnModel ID="ColumnModel6" runat="server">
                            <Columns>
                                <ext:Column DataIndex="FILE_NAME" Header="文件名稱" Width="200">
                                </ext:Column>
                                <ext:Column DataIndex="FILE_PATH" Header="文件路徑" Width="200">
                                    <Renderer Fn="change_Attachement_Issue" />
                                </ext:Column>
                                <ext:Column DataIndex="APPROVER" Header="簽核關卡" Width="200">
                                </ext:Column>
                                <ext:DateColumn DataIndex="UPDATE_TIME" Header="上傳時間" Width="200" Format="yyyy/MM/dd HH:mm">
                                </ext:DateColumn>
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:CheckboxSelectionModel ID="CheckboxSelectionModel4" runat="server" />
                        </SelectionModel>
                    </ext:GridPanel>
                    <ext:FieldSet ID="FieldSetUploadModify" runat="server" Title="附件上傳(母機種)" Layout="Form"
                        AutoHeight="true" Hidden="true">
                        <Items>
                            <ext:Container ID="Container110" runat="server" Layout="ColumnLayout" Height="50"
                                LabelWidth="60">
                                <Items>
                                    <ext:Container runat="server" ID="Container111" Layout="Form" ColumnWidth="0.5" LabelWidth="200">
                                        <Items>
                                            <ext:FileUploadField ID="fileMeetingmodify" runat="server" FieldLabel="附件上传(請用繁體或英文命名)"
                                                ButtonText="選擇文件" Width="200">
                                            </ext:FileUploadField>
                                            <ext:Button ID="Button4" runat="server" Text="上傳" Icon="Accept">
                                                <DirectEvents>
                                                    <Click OnEvent="btnConfirm_Click_Modify">
                                                    </Click>
                                                </DirectEvents>
                                            </ext:Button>
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:FieldSet>
                    <ext:GridPanel ID="grdAttachmentModify" runat="server" Frame="true" StripeRows="true"
                        TrackMouseOver="true" AutoHeight="true" Header="true" Title="Modify附件" Hidden="true">
                        <TopBar>
                            <ext:Toolbar ID="Toolbar7" runat="server">
                                <Items>
                                    <ext:Button runat="server" ID="btnDeleteModify" Text="删除" Icon="Delete">
                                        <DirectEvents>
                                            <Click OnEvent="btnDelete_ClickModify">
                                                <Confirmation ConfirmRequest="true" Message="確認刪除勾選的記錄?" Title="提示" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{grdAttachmentModify}.getRowsValues({selectedOnly:true}))"
                                                        Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Store>
                            <ext:Store ID="Store17" runat="server">
                                <Reader>
                                    <ext:JsonReader>
                                        <Fields>
                                            <ext:RecordField Name="SUB_DOC_NO" />
                                            <ext:RecordField Name="FILE_PATH" Type="String" />
                                            <ext:RecordField Name="FILE_NAME" Type="String" />
                                            <ext:RecordField Name="APPROVER" Type="String" />
                                            <ext:RecordField Name="UPDATE_TIME" Type="Date" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                            </ext:Store>
                        </Store>
                        <ColumnModel ID="ColumnModel1" runat="server">
                            <Columns>
                                <ext:Column DataIndex="FILE_NAME" Header="文件名稱" Width="200">
                                </ext:Column>
                                <ext:Column DataIndex="FILE_PATH" Header="文件路徑" Width="200">
                                    <Renderer Fn="change_Attachement_Issue" />
                                </ext:Column>
                                <ext:Column DataIndex="APPROVER" Header="簽核關卡" Width="200">
                                </ext:Column>
                                <ext:DateColumn DataIndex="UPDATE_TIME" Header="上傳時間" Width="200" Format="yyyy/MM/dd HH:mm">
                                </ext:DateColumn>
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:CheckboxSelectionModel ID="CheckboxSelectionModel5" runat="server" />
                        </SelectionModel>
                    </ext:GridPanel>
                </Items>
            </ext:Panel>
            <ext:Panel ID="pnlAttachment" runat="server" Layout="Form" Title="附档" Frame="True"
                LabelWidth="5" Region="Center" AutoHeight="true" Hidden="true">
                <Items>
                    <ext:Panel ID="pnlAttach" runat="server" Layout="Form" Height="50" Hidden="true">
                        <Items>
                            <ext:Container ID="Container29" runat="server" Layout="ColumnLayout" Height="200">
                                <Items>
                                    <ext:Container ID="Container30" runat="server" LabelWidth="70" Layout="Form" ColumnWidth="0.4">
                                        <Items>
                                            <ext:ComboBox ID="cobDept" runat="server" FieldLabel="部門" Width="120" SelectedIndex="0"
                                                TabIndex="1">
                                            </ext:ComboBox>
                                            <ext:TextField ID="txtAttachRemark" runat="server" FieldLabel="文件說明" Width="300">
                                            </ext:TextField>
                                            <ext:TextField ID="txtPRID" runat="server" FieldLabel="ID" Width="120" Hidden="true">
                                            </ext:TextField>
                                            <ext:TextField ID="txtFileName" runat="server" FieldLabel="NAME" Width="120" Hidden="true">
                                            </ext:TextField>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container ID="Container31" runat="server" LabelWidth="120" Layout="Form" ColumnWidth="0.4">
                                        <Items>
                                            <ext:FileUploadField ID="FileAttachment" runat="server" FieldLabel="附件上傳:(請用英文或繁體命名)"
                                                LabelSeparator=" " ButtonText="請選擇文件" Width="200">
                                            </ext:FileUploadField>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container ID="Container32" runat="server" LabelWidth="70" Layout="Form" ColumnWidth="0.2">
                                        <Items>
                                            <ext:Button ID="btnConfirm1" runat="server" Width="80" Text="上傳" Icon="Accept">
                                                <DirectEvents>
                                                    <Click OnEvent="btnConfirm1_Click">
                                                    </Click>
                                                </DirectEvents>
                                            </ext:Button>
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Panel>
                    <ext:GridPanel ID="grdAttachement" runat="server" AutoHeight="true" Header="False"
                        Frame="False" Title="" Hidden="true">
                        <Store>
                            <ext:Store ID="Store8" runat="server">
                                <Reader>
                                    <ext:JsonReader>
                                        <Fields>
                                            <ext:RecordField Name="ID" />
                                            <ext:RecordField Name="CASEID" />
                                            <ext:RecordField Name="FILE_PATH" />
                                            <ext:RecordField Name="FILE_TYPE" />
                                            <ext:RecordField Name="FILE_NAME" />
                                            <ext:RecordField Name="DEPT" />
                                            <ext:RecordField Name="FILE_REMARK" />
                                            <ext:RecordField Name="UPLOADUSER" />
                                            <ext:RecordField Name="UPLOAD_TIME" Type="Date" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>
                                <ext:CommandColumn Width="70">
                                    <Commands>
                                        <ext:GridCommand Icon="Pencil" CommandName="Update" Text="更新">
                                        </ext:GridCommand>
                                    </Commands>
                                    <PrepareToolbar Fn="NoteGoHidPR" />
                                </ext:CommandColumn>
                                <ext:Column Header="ID" DataIndex="ID" Width="90" Hidden="true">
                                </ext:Column>
                                <ext:Column Header="部門" DataIndex="DEPT" Width="90">
                                </ext:Column>
                                <ext:Column Header=" 文件名稱" DataIndex="FILE_PATH" Width="150">
                                    <Renderer Fn="change_Attachement" />
                                </ext:Column>
                                <ext:Column Header="文件說明" DataIndex="FILE_REMARK" Width="150">
                                </ext:Column>
                                <ext:Column Header="FILE_NAME" DataIndex="FILE_NAME" Width="120" Hidden="true">
                                </ext:Column>
                                <ext:Column Header="主管意見" DataIndex="APPROVER_OPINION" Width="240" Hidden="true">
                                </ext:Column>
                                <ext:Column Header="上傳人員" DataIndex="UPLOADUSER" Width="120">
                                </ext:Column>
                                <ext:DateColumn Header="上傳時間" DataIndex="UPLOAD_TIME" Width="120" Format="yyyy/MM/dd">
                                </ext:DateColumn>
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:CheckboxSelectionModel ID="CheckboxSelectionModel2" runat="server" SingleSelect="true">
                            </ext:CheckboxSelectionModel>
                        </SelectionModel>
                        <DirectEvents>
                            <Command OnEvent="grdAttachement_RowCommand">
                                <ExtraParams>
                                    <ext:Parameter Name="ID" Value="record.data.ID" Mode="Raw" />
                                    <ext:Parameter Name="DEPT" Value="record.data.DEPT" Mode="Raw" />
                                    <ext:Parameter Name="FILE_REMARK" Value="record.data.FILE_REMARK" Mode="Raw" />
                                    <ext:Parameter Name="FILE_NAME" Value="record.data.FILE_NAME" Mode="Raw" />
                                </ExtraParams>
                            </Command>
                        </DirectEvents>
                        <TopBar>
                            <ext:Toolbar ID="Toolbar2" runat="server" LabelWidth="75">
                                <Items>
                                    <ext:Button ID="btnDelAttachement" runat="server" Text="删除" Icon="Delete">
                                        <DirectEvents>
                                            <Click OnEvent="btnDelAttachement_click">
                                                <ExtraParams>
                                                    <ext:Parameter Name="values" Mode="Raw" Value="Ext.encode(#{grdAttachement}.getRowsValues({selectedOnly:true}))">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <%--                        <DirectEvents>
                            <AfterEdit OnEvent="AfterEdit" Json="true">
                                <ExtraParams>
                                    <ext:Parameter Name="ID" Value="e.record.data.ID" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="CASEID" Value="e.record.data.CASEID" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="DEPT" Value="e.record.data.DEPT" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="APPROVER_OPINION" Value="e.record.data.APPROVER_OPINION" Mode="Raw">
                                    </ext:Parameter>
                                </ExtraParams>
                            </AfterEdit>
                        </DirectEvents>
                        <Listeners>
                            <BeforeEdit Fn="beforeEdit" />
                        </Listeners>--%>
                    </ext:GridPanel>
                </Items>
            </ext:Panel>
            <ext:Panel ID="pnlResultMain" runat="server" Layout="Form" Title="" Frame="True"
                LabelWidth="5" Region="Center" AutoHeight="true" Hidden="true">
                <Items>
                    <ext:Panel ID="pnlReslut" runat="server" Layout="Form" Height="100" Hidden="true">
                        <Items>
                            <ext:Container ID="Container33" runat="server" Layout="ColumnLayout" Height="30">
                                <Items>
                                    <ext:Container ID="Container34" runat="server" LabelWidth="70" Layout="Form" ColumnWidth="0.30">
                                        <Items>
                                            <ext:TextField ID="txtReslutDept" runat="server" FieldLabel="部門" Width="120" SelectedIndex="0"
                                                TabIndex="1" ReadOnly="true">
                                            </ext:TextField>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container ID="Container36" runat="server" LabelWidth="70" Layout="Form" ColumnWidth="0.45">
                                        <Items>
                                            <ext:RadioGroup ID="rgResult" runat="server" FieldLabel="意見" Width="300">
                                                <Items>
                                                    <ext:Radio ID="rdResultY" runat="server" BoxLabel="PASS" Width="60" />
                                                    <ext:Radio ID="rdResultN" runat="server" BoxLabel="FAIL" Width="60" />
                                                    <ext:Radio ID="rdReulutCondition" runat="server" BoxLabel="Condition Pass" Width="80" />
                                                </Items>
                                            </ext:RadioGroup>
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container18" runat="server" Layout="ColumnLayout" Height="50">
                                <Items>
                                    <ext:Container ID="Container37" runat="server" LabelWidth="70" Layout="Form">
                                        <Items>
                                            <ext:TextArea ID="txtReslutOpinion" runat="server" FieldLabel="備註" Width="560" SelectedIndex="0"
                                                Height="50" TabIndex="1">
                                            </ext:TextArea>
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Panel>
                    <ext:GridPanel ID="grdResult" runat="server" AutoHeight="true" Header="False" Frame="False"
                        Title="" Hidden="true">
                        <Store>
                            <ext:Store ID="Store9" runat="server">
                                <Reader>
                                    <ext:JsonReader>
                                        <Fields>
                                            <ext:RecordField Name="ID" />
                                            <ext:RecordField Name="CASEID" />
                                            <ext:RecordField Name="DEPT" />
                                            <ext:RecordField Name="APPROVER_RESULT" />
                                            <ext:RecordField Name="APPROVER" />
                                            <ext:RecordField Name="APPROVER_OPINION" />
                                            <ext:RecordField Name="APPROVER_Levels" />
                                            <ext:RecordField Name="APPROVER_DATE" Type="Date" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>
                                <ext:Column Header="部門" DataIndex="DEPT" Width="120">
                                </ext:Column>
                                <ext:Column Header="结果" DataIndex="APPROVER_RESULT" Width="100">
                                    <Renderer Fn="Result" />
                                </ext:Column>
                                <ext:Column Header="主管意見" DataIndex="APPROVER_OPINION" Width="350">
                                </ext:Column>
                                <ext:Column Header="簽核人" DataIndex="APPROVER" Width="120">
                                </ext:Column>
                                <ext:Column Header="簽核關卡" DataIndex="APPROVER_Levels" Width="120">
                                </ext:Column>
                                <ext:DateColumn Header="簽核時間" DataIndex="APPROVER_DATE" Width="120" Format="yyyy/MM/dd HH:mm">
                                </ext:DateColumn>
                            </Columns>
                        </ColumnModel>
                        <TopBar>
                            <ext:Toolbar ID="Toolbar3" runat="server" LabelWidth="75">
                                <Items>
                                    <ext:Button ID="btnResultSave" runat="server" Text="保存" Icon="Disk" Hidden="true">
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                    </ext:GridPanel>
                </Items>
            </ext:Panel>
        </Items>
    </ext:Panel>
</asp:Content>
