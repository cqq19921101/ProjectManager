<%@ Page Language="C#" AutoEventWireup="true" CodeFile="template.aspx.cs"
    Inherits="template" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
</head>
<body style="background-color: #DFE8F6;">
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" />
    <ext:Hidden ID="lblLogonId" runat="server" Text="dewliu">
    </ext:Hidden>
    <ext:Panel ID="pnlMain" runat="server" Padding="0" Layout="Form" Title="" Border="false"
        BodyStyle="background-color: #DFE8F6;">
        <Items>
            <ext:Panel ID="Panel1" runat="server" Layout="Form" Title="人员维护" Header="false" BodyStyle="background-color: #DFE8F6;"
                Border="false">
                <Items>
                    <ext:Panel ID="Panel2" runat="server" Layout="Form" Title="" Header="false" BodyStyle="background-color: #DFE8F6;"
                        Border="false">
                        <Content>
                            <table id="tb_subject" runat="server" width="990" style="border-collapse: collapse;
                                border-spacing: inherit; empty-cells: hide; table-layout: auto;">
                                <tr>
                                    <td colspan="6" align="center" style="font-family: 新細明體; font-weight: bold; font-size: 22px;
                                        height: 40px">
                                        試產報告會簽表
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 30px; font-family: 'Times New Roman', Times, serif; font-weight: bold;
                                        font-size: 22px;" align="center" colspan="6">
                                        Pilot Run Report Approval Form
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2" align="right" style="width: 330px">
                                    </td>
                                    <td rowspan="2" align="right" style="width: 330px">
                                    </td>
                                    <td rowspan="2" align="center" style="width: 330px">
                                    </td>
                                </tr>
                            </table>
                        </Content>
                    </ext:Panel>
                    <ext:Panel ID="Panel24" runat="server" Layout="Form" Title="" Header="true" BodyStyle="background-color: #DFE8F6;"
                        Border="false">
                        <Items>
                           <ext:GridPanel ID="grdDocList" runat="server" Height="200" Frame="false" Title="待處理列表"
                                  Border="false">
                              <Store>
                                 <ext:Store ID="Store2" runat="server">
                                  <Reader>
                                     <ext:JsonReader>
                                          <Fields>
                                            <ext:RecordField Name="DOC_NO" />
                                            <ext:RecordField Name="SUB_DOC_NO" />
                                            <ext:RecordField Name="PROD_GROUP" />
                                            <ext:RecordField Name="PHASE" />
                                            <ext:RecordField Name="MODEL_NAME" />
                                            <ext:RecordField Name="ASSY_PN" />
                                            <ext:RecordField Name="FINAL_PN" />
                                            <ext:RecordField Name="PCB_NO" />
                                            <ext:RecordField Name="APPLY_USERID" />
                                            <ext:RecordField Name="APPLY_DATE" Type="Date" />
                                            <ext:RecordField Name="CREATE_DATE" Type="Date" />
                                            <ext:RecordField Name="DOC_PHASE" />
                                            <ext:RecordField Name="SUB_DOC_PHASE_VERSION" />
                                            <ext:RecordField Name="CLCA_QTY" />
                                            <ext:RecordField Name="CLCA_BEGIN_TIME" />
                                            <ext:RecordField Name="CLCA_END_TIME" />
                                         </Fields>
                                   </ext:JsonReader>
                                </Reader>
                               </ext:Store>
                           </Store>
                <ColumnModel>
                    <Columns>
                        <ext:ImageCommandColumn Width="100">
                            <Commands>
                                <ext:ImageCommand Icon="Pencil" CommandName="Confirm" Text="生成試產報告">
                                    <ToolTip Text="生成試產報告" />
                                </ext:ImageCommand>
                            </Commands>
                        </ext:ImageCommandColumn>
                        <ext:Column DataIndex="DOC_NO" Header="DOC NO" Width="100">
                        </ext:Column>
                        <ext:Column DataIndex="PROD_GROUP" Header="產品類別" Width="100">
                        </ext:Column>
                        <ext:Column DataIndex="DOC_PHASE" Header="PHASE" Width="100">
                        </ext:Column>
                        <ext:Column DataIndex="SUB_DOC_PHASE_VERSION" Header="版本" Width="100">
                        </ext:Column>
                        <ext:Column DataIndex="MODEL_NAME" Header="機種名稱" Width="100">
                        </ext:Column>
                        <ext:Column DataIndex="FINAL_PN" Header="成品料號" Width="100">
                        </ext:Column>
                        <ext:Column DataIndex="ASSY_PN" Header="半成品料號" Width="100">
                        </ext:Column>
                        <ext:Column DataIndex="PCB_NO" Header="PCB NO" Width="100">
                        </ext:Column>
                        <ext:Column DataIndex="APPLY_USERID" Header="申請人" Width="100">
                        </ext:Column>
                        <ext:DateColumn DataIndex="APPLY_DATE" Header="申請日期" Width="100" Format="yyyy/MM/dd">
                        </ext:DateColumn>
                    </Columns>
                </ColumnModel>
                <TopBar>
                    <ext:Toolbar ID="Toolbar2" runat="server">
                        <Items>
                           <ext:TextField ID="txtMODEL_NAME" runat="server" FieldLabel="機種名稱:"
                                      Width="250" LabelSeparator=" ">
                                      <DirectEvents>
                                          <Change OnEvent="txtMODEL_NAME_change"></Change>
                                      </DirectEvents>
                           </ext:TextField>
                       </Items>
                   </ext:Toolbar>
                </TopBar>
                <SelectionModel>
                    <ext:RowSelectionModel ID="CheckboxSelectionModel1" runat="server" SingleSelect="true">
                    </ext:RowSelectionModel>
                </SelectionModel>
                <DirectEvents>
                    <Command OnEvent="grdDocList_RowCommand">
                        <ExtraParams>
                            <ext:Parameter Name="DOC_NO" Value="record.data.DOC_NO" Mode="Raw" />
                            <ext:Parameter Name="SUB_DOC_NO" Value="record.data.SUB_DOC_NO" Mode="Raw" />
                            <ext:Parameter Name="MODEL_NAME" Value="record.data.MODEL_NAME" Mode="Raw" />
                            <ext:Parameter Name="FINAL_PN" Value="record.data.FINAL_PN" Mode="Raw" />
                            <ext:Parameter Name="PHASE" Value="record.data.DOC_PHASE" Mode="Raw" />
                            <ext:Parameter Name="SUB_DOC_PHASE_VERSION" Value="record.data.SUB_DOC_PHASE_VERSION"
                                Mode="Raw" />
                            <ext:Parameter Name="CREATE_DATE" Value="record.data.CREATE_DATE" Mode="Raw" />
                            <ext:Parameter Name="CLCA_QTY" Value="record.data.CLCA_QTY" Mode="Raw" />
                            <ext:Parameter Name="CLCA_BEGIN_TIME" Value="record.data.CLCA_BEGIN_TIME" Mode="Raw" />
                            <ext:Parameter Name="CLCA_END_TIME" Value="record.data.CLCA_END_TIME" Mode="Raw" />
                        </ExtraParams>
                    </Command>
                </DirectEvents>
            </ext:GridPanel>
                        </Items>
                    </ext:Panel>
                    <ext:Panel ID="Panel9" runat="server" Layout="Form" Title="基本信息" Header="true" BodyStyle="background-color: #DFE8F6;"
                        Border="false">
                        <Items>
                            <ext:TableLayout ID="TableLayout1" runat="server" Columns="3">
                                <Cells>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel44" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Padding="10" Width="330" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:TextField ID="txtProductName" runat="server" FieldLabel="<a>產品名稱</a><br><a>Product Name</a>:"
                                                    Width="250" LabelSeparator=" " ReadOnly="true">
                                                </ext:TextField>
                                                <ext:TextField ID="txtDoc_No" runat="server" FieldLabel="" Hidden="true">
                                                </ext:TextField>
                                                <ext:TextField ID="txtSub_No" runat="server" FieldLabel="" Hidden="true">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel45" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Padding="10" Width="330" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:TextField ID="txtCurrentStage" runat="server" FieldLabel="<a>當前階段</a><br><a>Current Stage</a>:"
                                                    Width="250" LabelSeparator=" " ReadOnly="true">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel46" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Padding="10" Width="330" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:TextField ID="txtAPPLY_DATE" runat="server" FieldLabel="<a>日期</a><br><a>Date</a>:"
                                                    Width="250" LabelSeparator=" " ReadOnly="true">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    </Cells>
                              </ext:TableLayout>
                        </Items>
                    </ext:Panel>
                    <ext:Panel ID="Panel3" runat="server" Layout="Form" Title="指標" Header="true" BodyStyle="background-color: #DFE8F6;"
                        Border="false">
                        <Items>
                           <ext:TableLayout ID="TableLayout4" runat="server" Columns="4">
                                <Cells>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel4" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Padding="10" Width="250" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label17" Width="250"  runat="server" FieldLabel="<a>試產結果判定下一階段</a>">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell ColSpan="3">
                                        <ext:Panel ID="Panel5" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Padding="10" Width="750" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:RadioGroup ID="rgStage" Disabled="true" runat="server" ItemCls="x-check-group-base" FieldLabel="Next Stage">
                                                <Items>
                                                    <ext:Radio ID="rdEVT" runat="server" BoxLabel="EVT" />
                                                    <ext:Radio ID="rdDVT" runat="server" BoxLabel="DVT" />
                                                    <ext:Radio ID="rdMVT" runat="server" BoxLabel="MVT" />
                                                    <ext:Radio ID="rdControlRun" runat="server" BoxLabel="Control run" />
                                                    <ext:Radio ID="rdMP" runat="server" BoxLabel="MP" />
                                                </Items>
                                               </ext:RadioGroup>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                     <ext:Cell RowSpan="3">
                                        <ext:Panel ID="Panel6" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Padding="10" Width="250"  Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label16"  Width="250"  runat="server" FieldLabel="<a>Summary Report</a><br><a>(汇总试产报告参考值)</a>">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel7" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Padding="10" Width="250" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label18" Width="250"  runat="server" FieldLabel="<a>CTQ汇总结果指标</a>">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                     <ext:Cell>
                                        <ext:Panel ID="Panel8" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Padding="10" Width="250" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label19" Width="250"  runat="server" FieldLabel="<a>CLCA汇总结果指标</a>">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                     <ext:Cell>
                                        <ext:Panel ID="Panel10" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Padding="10" Width="250" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label20" Width="250"  runat="server" FieldLabel="<a>DFX(RPN)汇总结果指标</a>">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel11" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Padding="10" Width="250" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label21" Width="250"  runat="server" FieldLabel="<a>Fail 件数</a>">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel12" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Padding="10" Width="250" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label22" Width="250"  runat="server" FieldLabel="<a>Open 件数</a>">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel13" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Padding="10" Width="250" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label23" Width="250"  runat="server" FieldLabel="<a>< 90分数</a>">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                     <ext:Cell>
                                        <ext:Panel ID="Panel14" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Padding="10" Width="250" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:TextField ID="txtCTQFail" runat="server" FieldLabel=""
                                                    Width="160" LabelSeparator=" " ReadOnly="true">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel15" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Padding="10" Width="250" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:TextField ID="txtCLCAOpen" runat="server" FieldLabel=""
                                                    Width="160" LabelSeparator=" " ReadOnly="true">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel49" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Padding="10" Width="250" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:TextField ID="txtDFXRpn" runat="server" FieldLabel=""
                                                    Width="160" LabelSeparator=" " ReadOnly="true">
                                                </ext:TextField>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                </Cells>
                            </ext:TableLayout>
                        </Items>
                    </ext:Panel>
                    <ext:Panel ID="pnlSignInfo" runat="server" Layout="Form" Title="基本資訊" Header="True" BodyStyle="background-color: #DFE8F6;"
                        Border="false" Width="990">
                        <Items>
                            <ext:TableLayout ID="TableLayout2" runat="server" Columns="4">
                                <Cells>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel17" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="130" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="TextField10" runat="server" FieldLabel="<a>單位</a><br><a>Dept</a>">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                          <ext:Panel ID="Panel18" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="360" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label1" runat="server" FieldLabel="<a>意見/结论</a><br><a>Suggestions/Conclusion</a>">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                          <ext:Panel ID="Panel19" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="210" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label2" runat="server" FieldLabel="<a>簽名/日期</a><br><a>Signature/Date</a>">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                          <ext:Panel ID="Panel20" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="300" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label3" runat="server" FieldLabel="<a>Remark</a>">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel21" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="130" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label4" runat="server" FieldLabel="<a>業務</a><br><a>Sales</a>">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                       <ext:Panel ID="Panel23114" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Padding="10" Width="360" LabelWidth="1" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:TextArea ID="txtPRODUCT_DES" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="330" Disabled="true">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel22" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Padding="10" Width="210" LabelWidth="1" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:TextArea ID="TextArea1" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="200" Disabled="true">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel23" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Padding="10" Width="300" LabelWidth="1" Border="false" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:TextArea ID="TextArea2" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="280" Disabled="true">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel25" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="130" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label5" runat="server" FieldLabel="<a>資材</a><br><a>PMC</a>">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel26" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" LabelWidth="1"  Width="360" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:TextArea ID="TextArea3" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="330">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel27" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="210" LabelWidth="1"  BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:TextArea ID="TextArea4" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="200">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel28" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="300" LabelWidth="1"  BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:TextArea ID="TextArea5" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="280">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel29" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="130" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label6" runat="server" FieldLabel="<a>SMT工程</a><br><a>SMT ENG</a>">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel30" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="360" LabelWidth="1"  BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:TextArea ID="TextArea6" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="330">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel31" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" LabelWidth="1"  Width="210" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:TextArea ID="TextArea7" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="200">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel32" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10"  LabelWidth="1"  Width="300" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:TextArea ID="TextArea8" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="280">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel33" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="130" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label7" runat="server" FieldLabel="<a>工程</a><br><a>PE</a>">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel34" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" LabelWidth="1"  Padding="10" Width="360" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:TextArea ID="TextArea9" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="330">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel35" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" LabelWidth="1" Padding="10" Width="210" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:TextArea ID="TextArea10" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="200">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel36" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" LabelWidth="1" Padding="10" Width="300" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:TextArea ID="TextArea11" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="280">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel37" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="130" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label8" runat="server" FieldLabel="<a>工程</a><br><a>IE</a>">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel38" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" LabelWidth="1" Width="360" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:TextArea ID="TextArea12" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="330">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel39" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" LabelWidth="1"  Width="210" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:TextArea ID="TextArea13" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="200">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel40" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" LabelWidth="1"  Width="300" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:TextArea ID="TextArea14" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="280">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel41" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="130" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label9" runat="server" FieldLabel="<a>製造</a><br><a>MFG</a>">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel42" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" LabelWidth="1"  Width="360" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:TextArea ID="TextArea15" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="330">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel43" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" LabelWidth="1"  Width="210" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:TextArea ID="TextArea16" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="200">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel50" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" LabelWidth="1"  Width="300" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:TextArea ID="TextArea17" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="280">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel51" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="130" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label10" runat="server" FieldLabel="<a>支援工程</a><br><a>RDS</a>">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel52" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="360" LabelWidth="1"  BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:TextArea ID="TextArea18" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="330">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel53" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="210" LabelWidth="1" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:TextArea ID="TextArea19" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="200">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel54" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="300" LabelWidth="1"  BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:TextArea ID="TextArea20" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="280">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel55" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="130"  BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label11" runat="server" FieldLabel="<a>研發</a><br><a>RD&HM</a>">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel56" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" LabelWidth="1"  Width="360" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:TextArea ID="TextArea21" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="330">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel57" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" LabelWidth="1"  Width="210" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:TextArea ID="TextArea22" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="200">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel58" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" LabelWidth="1"  Width="300" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:TextArea ID="TextArea23" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="280">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel16" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="130"  BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label15" runat="server" FieldLabel="<a>研發</a><br><a>RD&SW</a>">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel47" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" LabelWidth="1"  Width="360" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:TextArea ID="TextArea33" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="330">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel48" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" LabelWidth="1"  Width="210" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:TextArea ID="TextArea34" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="200">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel71" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" LabelWidth="1"  Width="300" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:TextArea ID="TextArea35" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="280">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel59" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="130" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label12" runat="server" FieldLabel="<a>專案</a><br><a>NPI</a>">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel60" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" LabelWidth="1"  Width="360" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:TextArea ID="TextArea24" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="330">
                                                </ext:TextArea>
                                                <ext:RadioGroup ID="RadioGroup1" Disabled="true" runat="server" ItemCls="x-check-group-base" FieldLabel="">
                                                 <Items>
                                                   <ext:Radio ID="Radio1" runat="server" BoxLabel="Pass"/>
                                                   <ext:Radio ID="Radio2" runat="server" BoxLabel="Fail" />
                                                 </Items>
                                               </ext:RadioGroup>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel61" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" LabelWidth="1"  Width="210" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:TextArea ID="TextArea25" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="200">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel62" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" LabelWidth="1"  Width="300" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:TextArea ID="TextArea26" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="280">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel63" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="130" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label13" runat="server" FieldLabel="<a>品保</a><br><a>QRA</a>">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel64" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" LabelWidth="1"  Width="360" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:TextArea ID="TextArea27" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="330">
                                                </ext:TextArea>
                                                 <ext:RadioGroup ID="RadioGroup2" runat="server" ItemCls="x-check-group-base" FieldLabel="">
                                                 <Items>
                                                   <ext:Radio ID="Radio3" runat="server" BoxLabel="Pass" />
                                                   <ext:Radio ID="Radio4" runat="server" BoxLabel="Fail" />
                                                 </Items>
                                               </ext:RadioGroup>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel65" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" LabelWidth="1"  Width="210" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:TextArea ID="TextArea28" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="200">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel66" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="300" LabelWidth="1"  BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:TextArea ID="TextArea29" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="280">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel67" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="130" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                                <ext:Label ID="Label14" runat="server" FieldLabel="<a>廠長</a><br><a>MD</a>">
                                                </ext:Label>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel68" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="360" LabelWidth="1" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:TextArea ID="TextArea30" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="330">
                                                </ext:TextArea>
                                                 <ext:RadioGroup ID="RadioGroup3" runat="server" ItemCls="x-check-group-base" FieldLabel="">
                                                 <Items>
                                                   <ext:Radio ID="Radio5" runat="server" BoxLabel="Pass"/>
                                                   <ext:Radio ID="Radio6" runat="server" BoxLabel="Fail" />
                                                 </Items>
                                               </ext:RadioGroup>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel69" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" Width="210" LabelWidth="1"  BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:TextArea ID="TextArea31" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="200">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                    <ext:Cell>
                                        <ext:Panel ID="Panel70" runat="server" Header="false" Frame="false" Title="Basic Table Cell"
                                            Border="false" Padding="10" LabelWidth="1" Width="300" BodyStyle="background-color: #DFE8F6;">
                                            <Items>
                                               <ext:TextArea ID="TextArea32" runat="server" FieldLabel=" " Height="50" LabelSeparator=" "
                                                    Width="280">
                                                </ext:TextArea>
                                            </Items>
                                        </ext:Panel>
                                    </ext:Cell>
                                </Cells>
                            </ext:TableLayout>
                        </Items>
                    </ext:Panel>
                </Items>
            </ext:Panel>
          <%--  <ext:Panel ID="Panel47" runat="server" Layout="Form" Title="" Width="990" Border="false">
                <Items>
                    <ext:Panel ID="Panel48" runat="server" Layout="Form" Title="人员维护" Header="false"
                        BodyStyle="background-color: #DFE8F6;" Border="false">
                        <Content>
                            <table id="Table1" runat="server" width="990" style="border-collapse: collapse; border-spacing: inherit;
                                empty-cells: hide; table-layout: auto;">
                                <tr>
                                    <td rowspan="2" align="right" style="width: 460px">
                                        <ext:Button ID="btnSave" runat="server" Text="提交" Width="90" Icon="Accept">
                                       
                                        </ext:Button>
                                    </td>
                                    <td rowspan="2" align="left" style="width: 460px">
                                        <ext:Button ID="btnContinue" Disabled="true" runat="server" Text="繼續錄入" Width="90"
                                            Icon="ArrowRefresh">
                                            
                                        </ext:Button>
                                    </td>
                                </tr>
                            </table>
                        </Content>
                    </ext:Panel>
                </Items>
            </ext:Panel>--%>
        </Items>
    </ext:Panel>
    </form>
</body>
</html>
