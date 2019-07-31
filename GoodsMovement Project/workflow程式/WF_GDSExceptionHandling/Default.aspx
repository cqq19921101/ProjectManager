<%@ Page Title="" Language="C#" MasterPageFile="~/Master/SpmMaster.master" AutoEventWireup="true"
    ValidateRequest="false" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">

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

    <link rel="stylesheet" href="CSS/Common.css" type="text/css" />
    <ext:ResourceManager ID="ResourceManager1" runat="server" />
    <ext:Hidden ID="lblLogonId" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="lblSite" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="lblBu" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="lblBuilding" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="lblStepName" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="lblVQMDEPT" runat="server">
    </ext:Hidden>
    <ext:Panel ID="vMain" runat="server" Layout="FitLayout" Height="560">
        <Items>
            <ext:Panel ID="Panel2" runat="server" Layout="BorderLayout" Border="false" BodyStyle="background-color: #DFE8F6;"
                Header="false">
                <Items>
                    <ext:Panel ID="pnlMain" runat="server" Layout="Form" Border="false" Region="Center"
                        BodyStyle="background-color: transparent;" Title="部門領料_應退未退申請單">
                        <Items>
                            <ext:Panel ID="frmUserInfo" runat="server" Layout="Form" Border="false" Title="申請人基本資料"
                                Region="Center" BodyStyle="background-color: transparent;" Padding="5">
                                <Items>
                                    <ext:Container ID="Container24" runat="server" Layout="ColumnLayout" Height="30">
                                        <Items>
                                            <ext:Container ID="Container26" runat="server" ColumnWidth="0.33" Layout="FormLayout">
                                                <Items>
                                                    <ext:TextField ID="txtLogonID" runat="server" FieldLabel="英文名稱" Width="200px" ReadOnly="true">
                                                    </ext:TextField>
                                                </Items>
                                            </ext:Container>
                                            <ext:Container ID="Container25" runat="server" ColumnWidth="0.33" Layout="FormLayout">
                                                <Items>
                                                    <ext:TextField ID="txtName" runat="server" FieldLabel="中文名稱" Width="200px" ReadOnly="true">
                                                    </ext:TextField>
                                                </Items>
                                            </ext:Container>
                                            <ext:Container ID="Container27" runat="server" ColumnWidth="0.33" Layout="FormLayout">
                                                <Items>
                                                    <ext:TextField ID="txtPlant" runat="server" FieldLabel="事&nbsp; 業&nbsp; 群" Width="200px"
                                                        ReadOnly="true">
                                                    </ext:TextField>
                                                </Items>
                                            </ext:Container>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container ID="Container28" runat="server" Layout="ColumnLayout" Height="30">
                                        <Items>
                                            <ext:Container ID="Container29" runat="server" ColumnWidth="0.33" Layout="FormLayout">
                                                <Items>
                                                    <ext:TextField ID="txtDept" runat="server" FieldLabel="部門名稱" Width="200px" ReadOnly="true">
                                                    </ext:TextField>
                                                </Items>
                                            </ext:Container>
                                            <ext:Container ID="Container30" runat="server" ColumnWidth="0.33" Layout="FormLayout">
                                                <Items>
                                                    <ext:TextField ID="txtEMail" runat="server" FieldLabel="郵件地址" Width="200px" ReadOnly="true">
                                                    </ext:TextField>
                                                </Items>
                                            </ext:Container>
                                            <ext:Container ID="Container31" runat="server" ColumnWidth="0.33" Layout="FormLayout">
                                                <Items>
                                                    <ext:TextField ID="txtExtNO" runat="server" FieldLabel="分機號碼" Width="200px" ReadOnly="true">
                                                    </ext:TextField>
                                                </Items>
                                            </ext:Container>
                                            <ext:Container ID="Container11" runat="server" Layout="ColumnLayout" Hidden="true">
                                                <Items>
                                                    <ext:TextField ID="txtHead" runat="server" FieldLabel="">
                                                    </ext:TextField>
                                                    <ext:TextField ID="txtDetail" runat="server" FieldLabel="">
                                                    </ext:TextField>
                                                    <ext:TextField ID="txtWERKS" runat="server" FieldLabel="">
                                                    </ext:TextField>
                                                    <ext:TextField ID="txtAPTYP" runat="server" FieldLabel="">
                                                    </ext:TextField>
                                                    <ext:TextField ID="txtDOA" runat="server" FieldLabel="">
                                                    </ext:TextField>
                                                </Items>
                                            </ext:Container>
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:Panel>
                            <ext:Panel ID="frmApplyInfo" runat="server" Layout="FormLayout" Border="false" Region="Center"
                                BodyStyle="background-color: transparent;" Padding="5" Title="申請單基本信息">
                                <Items>
                                    <ext:Container ID="Container1" runat="server" Layout="ColumnLayout" Height="30">
                                        <Items>
                                            <ext:Container ID="Container2" runat="server" ColumnWidth="0.33" Layout="FormLayout">
                                                <Items>
                                                    <ext:TextField ID="txtRDocNo" runat="server" FieldLabel="Link 單號(領料單)" Width="200px">
                                                    </ext:TextField>
                                                </Items>
                                            </ext:Container>
                                            <ext:Container ID="Container3" runat="server" ColumnWidth="0.33" Layout="FormLayout">
                                                <Items>
                                                    <ext:Button ID="btnLink" runat="server" Text="Link" Icon="Accept" Hidden="false">
                                                        <DirectEvents>
                                                            <Click OnEvent="btnLink_Click">
                                                            <EventMask ShowMask="true" Msg="加載中..." MinDelay="500" />
                                                            </Click>
                                                        </DirectEvents>
                                                    </ext:Button>
                                                </Items>
                                            </ext:Container>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container ID="Container_row1" runat="server" Layout="ColumnLayout" Height="30">
                                        <Items>
                                            <ext:Container ID="Container6" runat="server" ColumnWidth="0.33" Layout="FormLayout">
                                                <Items>
                                                    <ext:TextField ID="txtDocNo" runat="server" FieldLabel="單&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;號"
                                                        Width="200px" ReadOnly="true">
                                                    </ext:TextField>
                                                </Items>
                                            </ext:Container>
                                            <ext:Container ID="Container7" runat="server" ColumnWidth="0.33" Layout="FormLayout">
                                                <Items>
                                                    <ext:TextField ID="txtCostCenter" runat="server" FieldLabel="Cost Center" Width="200px"
                                                        ReadOnly="true">
                                                    </ext:TextField>
                                                </Items>
                                            </ext:Container>
                                            <ext:Container ID="Container8" runat="server" ColumnWidth="0.33" Layout="FormLayout">
                                                <Items>
                                                    <ext:TextField ID="txtDepartment" runat="server" FieldLabel="Department" Width="200px"
                                                        ReadOnly="true">
                                                    </ext:TextField>
                                                </Items>
                                            </ext:Container>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container ID="Container_row2" runat="server" Layout="ColumnLayout" Height="30">
                                        <Items>
                                            <ext:Container ID="Container10" runat="server" ColumnWidth="0.33" Layout="FormLayout">
                                                <Items>
                                                    <ext:TextField ID="txtApplication" runat="server" FieldLabel="Application" Width="200px"
                                                        ReadOnly="true">
                                                    </ext:TextField>
                                                </Items>
                                            </ext:Container>
                                            <ext:Container ID="Container13" runat="server" ColumnWidth="0.33" Layout="FormLayout">
                                                <Items>
                                                    <ext:TextField ID="txtReturn" runat="server" FieldLabel="Return" Width="200px" ReadOnly="true">
                                                    </ext:TextField>
                                                </Items>
                                            </ext:Container>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container ID="Container17" runat="server" Layout="ColumnLayout" Height="30">
                                        <Items>
                                            <ext:Container ID="Container12" runat="server" ColumnWidth="0.33" Layout="FormLayout">
                                                <Items>
                                                    <ext:ComboBox ID="txtMaterial" runat="server" FieldLabel="Material" Width="200px"
                                                        DisplayField="MATNR" ValueField="MATNR">
                                                        <Store>
                                                            <ext:Store ID="Store12" runat="server">
                                                                <Reader>
                                                                    <ext:JsonReader>
                                                                        <Fields>
                                                                            <ext:RecordField Name="MATNR" Type="String">
                                                                            </ext:RecordField>
                                                                        </Fields>
                                                                    </ext:JsonReader>
                                                                </Reader>
                                                            </ext:Store>
                                                        </Store>
                                                    </ext:ComboBox>
                                                </Items>
                                            </ext:Container>
                                            <ext:Container ID="Container18" runat="server" ColumnWidth="0.33" Layout="FormLayout">
                                                <Items>
                                                    <ext:TextField ID="txtZEILE" runat="server" FieldLabel="Item" Width="200px" ReadOnly="true">
                                                    </ext:TextField>
                                                </Items>
                                            </ext:Container>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container ID="Container5" runat="server" Layout="ColumnLayout" Height="30">
                                        <Items>
                                            <ext:Container ID="Container9" runat="server" ColumnWidth="0.33" Layout="FormLayout">
                                                <Items>
                                                    <ext:TextField ID="txtIADocNo" runat="server" FieldLabel="關聯IA單" Width="200px" ReadOnly="true">
                                                    </ext:TextField>
                                                </Items>
                                            </ext:Container>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container ID="Container16" runat="server" Layout="ColumnLayout" Height="30">
                                        <Items>
                                            <ext:Container ID="Container14" runat="server" ColumnWidth="0.33" Layout="FormLayout">
                                                <Items>
                                                    <ext:TextField ID="txtI6DocNo" runat="server" FieldLabel="關聯I6單" Width="200px" ReadOnly="true">
                                                    </ext:TextField>
                                                </Items>
                                            </ext:Container>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container ID="Container_row3" runat="server" Layout="ColumnLayout" Height="30">
                                        <Items>
                                            <ext:Container ID="Container4" runat="server" ColumnWidth="0.33" Layout="FormLayout">
                                                <Items>
                                                    <ext:TextField ID="txtReturnQuantity" runat="server" FieldLabel="Return Quantity"
                                                        Width="200px">
                                                        <DirectEvents>
                                                            <Change OnEvent="check_workorder">
                                                            </Change>
                                                        </DirectEvents>
                                                    </ext:TextField>
                                                </Items>
                                            </ext:Container>
                                            <ext:Container ID="Container15" runat="server" ColumnWidth="0.33" Layout="FormLayout">
                                                <Items>
                                                    <ext:ComboBox ID="txtReason" runat="server" FieldLabel="Reason" Width="200">
                                                        <Items>
                                                            <ext:ListItem Text="Lost" Value="Lost" />
                                                            <ext:ListItem Text="Other" Value="Other" />
                                                        </Items>
                                                    </ext:ComboBox>
                                                </Items>
                                            </ext:Container>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container ID="Container_row4" runat="server" Layout="ColumnLayout" Height="90">
                                        <Items>
                                            <ext:Container ID="Container19" runat="server" ColumnWidth="0.33" Layout="FormLayout">
                                                <Items>
                                                    <ext:TextArea ID="txtRemark" runat="server" FieldLabel="Remark" Width="400">
                                                    </ext:TextArea>
                                                </Items>
                                            </ext:Container>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container ID="Container20" runat="server" Layout="ColumnLayout" Height="90">
                                        <Items>
                                            <ext:Container ID="Container21" runat="server" ColumnWidth="0.33" Layout="FormLayout">
                                                <Items>
                                                </Items>
                                            </ext:Container>
                                            <ext:Container ID="Container22" runat="server" ColumnWidth="0.33" Layout="FormLayout">
                                                <Items>
                                                </Items>
                                            </ext:Container>
                                            <ext:Container ID="Container23" runat="server" ColumnWidth="0.33" Layout="FormLayout">
                                                <Items>
                                                    <ext:TextField ID="txtAmount" runat="server" FieldLabel="Amount" Width="200" ReadOnly="true">
                                                    </ext:TextField>
                                                </Items>
                                            </ext:Container>
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:Panel>
                </Items>
            </ext:Panel>
        </Items>
    </ext:Panel>
</asp:Content>
