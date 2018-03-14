<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/IMSNetMaster.Master" AutoEventWireup="true" CodeBehind="Service_Line_ModuleContent.aspx.cs" Inherits="IMS_Net.Admin.Service_Line_Module" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:UpdatePanel ID="udpServiceLineModule" runat="server">
        <ContentTemplate>
            <table width="100%" border="0" cellpadding="0" cellspacing="0">

                <tr>
                    <td colspan="4" class="tdHeading">
                        <asp:Label ID="Label1" runat="server" Text="Service Line Module" CssClass="heading"></asp:Label>
                    </td>
                </tr>

                <tr id="r_Error1" runat="server" visible="false">
                    <td colspan="4" align="left" class="label Pad-T10 Pad-L15 Pad-B10" style="height: 4px">
                        <asp:Label ID="lblSuccessMsg" runat="server" ForeColor="green"></asp:Label>
                        <asp:Label ID="lblErrorMsg" runat="server" ForeColor="red"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td colspan="4" style="height: 25px" class="label Pad-L15 Pad-T5">
                        <asp:Label ID="Label6" runat="server" Text="(*) Marked fields are mandatory " Height="21px"
                            ForeColor="Red"></asp:Label>
                    </td>
                </tr>

                <!--<tr style="display:none">
            <td class="label Pad-L15 Pad-T10" align="left" width="15%" valign="top" style="white-space:nowrap">
                <asp:Label ID="lblModuleCode" runat="server" Text="Module Code"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtModuleCode" runat="server"></asp:TextBox>
            </td>
        </tr>-->

                <tr id="r_ServiceLineCode" runat="server">
                    <td class="label Pad-L15 Pad-T10" align="left" width="15%" valign="top" style="white-space: nowrap">
                        <asp:Label ID="lblServiceLineCode" runat="server" Text="Service Line Code"></asp:Label>
                    </td>
                    <td class="textbox Pad-T10" width="25%" valign="top" style="white-space: nowrap">
                        <asp:DropDownList ID="ddlServiceLineCode" runat="server" DataTextField="Description" DataValueField="Service_Line_Code" AutoPostBack="true" Width="228px"></asp:DropDownList>
                    </td>
                    <td class="label Pad-T10 " align="left" width="15%" valign="top" style="white-space: nowrap">
                        <asp:Label ID="lblModuleDescription" runat="server" Text="Module Description"></asp:Label>
                        <asp:Label ID="lblerror" runat="server" ForeColor="Red" Text="*"></asp:Label>
                    </td>
                    <td class="textbox Pad-T10" align="left" width="25%" valign="top" style="white-space: nowrap">
                        <asp:TextBox ID="txtModuleDescription" runat="server" TextMode="MultiLine" MaxLength="50" Rows="5" Width="228px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvModuleDescription" runat="server" ControlToValidate="txtModuleDescription" ErrorMessage="Description is required"
                            ForeColor="red" SetFocusOnError="true" ToolTip="Please fill Description" Display="Dynamic" EnableClientScript="true"></asp:RequiredFieldValidator>
                    </td>

                </tr>

                <tr>
                    <td class="label Pad-L15 Pad-T10" align="left" width="15%" valign="top" style="white-space: nowrap">
                        <asp:Label ID="lblActive" runat="server" Text="Active"></asp:Label>
                    </td>
                    <td class="textbox Pad-T10" width="25%" valign="top" style="white-space: nowrap" align="left">
                        <asp:RadioButtonList ID="rblActive" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Text="True" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="False"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td colspan="2" class="label Pad-T10 Pad-L15 Pad-B10"></td>
                </tr>

                <tr>
                    <td colspan="4" class="label Pad-T10 Pad-L15 Pad-B10">&nbsp;
        <asp:Button ID="btnSave" runat="server" Text="Save" CausesValidation="true" OnClick="btnSave_Click" CssClass="clsButton" Width="70px" />
                        <asp:Button ID="btnUpdate" runat="server" Text="Update" CausesValidation="true" OnClick="btnUpdate_Click" CommandArgument='<%# Eval("Module_Code") %>' CssClass="clsButton" Width="70px" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CausesValidation="false" OnClick="btnClear_Click" CssClass="clsButton" Width="70px" />
                    </td>
                </tr>

                <tr>
                    <td colspan="4" class="label Pad-T10 Pad-L15 Pad-B10">
                        <asp:GridView ID="gvServiceLineModule" runat="server" AutoGenerateColumns="false" CssClass="gridTableData" GridLines="None" AllowPaging="true" 
                            PageSize="10" OnPageIndexChanging="ServiceLineModule_PageIndexChanging">
                            <HeaderStyle CssClass="gridHeader" />
                            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                            <Columns>
                                <asp:BoundField DataField="Service_Line_Code" HeaderText="Service Line Code" Visible="false" />
                                <asp:BoundField DataField="Description" HeaderText="Service Line Description" />
                                <asp:BoundField DataField="Module_Code" HeaderText="Module Code" Visible="false" />
                                <asp:BoundField DataField="Module_Description" HeaderText="Module Description" />
                                <asp:BoundField DataField="Active" HeaderText="Is Active" />
                                <asp:BoundField DataField="companyid" HeaderText="Company Id" Visible="false"/>
                                <asp:BoundField DataField="CreatedBy" HeaderText="Created By" />
                                <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" />
                                <asp:BoundField DataField="UpdatedBy" HeaderText="Updated By" />
                                <asp:BoundField DataField="UpdatedDate" HeaderText="Updated Date" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkView" CausesValidation="false" runat="server" Text="Edit" 
                                            OnClick="btnView_Click" CommandArgument='<%# Eval("Module_Code") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
        </ContentTemplate>
    </asp:UpdatePanel>
    </table>
</asp:Content>
