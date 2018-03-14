<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/IMSNetMaster.Master" AutoEventWireup="true" CodeBehind="ServiceMasterContent.aspx.cs" Inherits="IMS_Net.Admin.ServiceMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="udpServiceMaster" runat="server">
        <ContentTemplate>
            <table border="0" cellpadding="0" cellspacing="0" width="100%">

                <tr>
                    <td colspan="4" class="tdHeading">
                        <asp:Label ID="Label1" runat="server" Text="Service Master " CssClass="heading"></asp:Label>
                    </td>
                </tr>

                <tr id="r_Error1" runat="server" visible="false">
                    <td colspan="4" align="left" class="label Pad-T10 Pad-L15 Pad-B10" style="height: 4px">
                        <asp:Label ID="lblSuccessMsg" runat="server" ForeColor="Green"></asp:Label>
                        <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td colspan="4" style="height: 25px" class="label Pad-L15 Pad-T5">
                        <asp:Label ID="Label6" runat="server" Text="(*) Marked fields are mandatory " Height="21px"
                            ForeColor="Red"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td class="label Pad-L15 Pad-T10" align="left" width="15%" valign="top" style="white-space: nowrap">
                        <asp:Label ID="lblServiceLineCode" runat="server" Text="Service Line Code:"></asp:Label>
                        <asp:Label ID="lblCodeError" runat="server" ForeColor="Red" Text="*"></asp:Label>
                    </td>
                    <td class="textbox Pad-T10" width="25%" valign="top" style="white-space: nowrap">
                        <asp:TextBox ID="txtServicelineCode" runat="server" MaxLength="2"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfServiceLineCode" runat="server" ValidationGroup="vgServiceMaster" ErrorMessage="Service Line Code Required"
                            ControlToValidate="txtServiceLineCode" ForeColor="red" SetFocusOnError="true" ToolTip="Please fill Description" Display="Dynamic"
                            EnableClientScript="true"></asp:RequiredFieldValidator>
                    </td>
                    <td class="label Pad-L15 Pad-T10 " align="left" width="15%" valign="top" style="white-space: nowrap">
                        <asp:Label ID="lblDescription" runat="server" Text="Description:"></asp:Label>
                        <asp:Label ID="lblerror" runat="server" ForeColor="Red" Text="*"></asp:Label>
                    </td>
                    <td class="textbox Pad-T10" align="left" width="25%" valign="top" style="white-space: nowrap">
                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="5" Width="228px" MaxLength="150"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfDescription" runat="server" ValidationGroup="vgServiceMaster" ErrorMessage="Description is Required"
                            ControlToValidate="txtDescription" ForeColor="Red" SetFocusOnError="true" ToolTip="Please fill Description" Display="Dynamic"
                            EnableClientScript="true"></asp:RequiredFieldValidator>
                    </td>

                </tr>

                <tr>
                    <td class="label Pad-L15 Pad-T10" align="left" width="15%" valign="top" style="white-space: nowrap">
                        <asp:Label ID="lblActive" runat="server" Text="Active:"></asp:Label>
                    </td>
                    <td class="textbox Pad-T10" width="25%" valign="top" style="white-space: nowrap">
                        <asp:RadioButtonList ID="rblActive" runat="server" Style="padding-left: 17px;" RepeatDirection="Horizontal">
                            <asp:ListItem Text="True" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="False"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td class="label Pad-L15 Pad-T10 " colspan="2"></td>
                </tr>

                <tr>
                    <td colspan="4" class="label Pad-T10 Pad-L15 Pad-B10">&nbsp;
                     <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="vgServiceMaster" CssClass="clsButton" Width="70px" />
                        <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" ValidationGroup="vgServiceMaster" CssClass="clsButton" Width="70px" CommandArgument='<%# Eval("Service_Line_Code") %>' />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" CausesValidation="false" CssClass="clsButton" Width="70px" />
                    </td>
                </tr>

                <tr>
                    <td colspan="4" class="label Pad-T10 Pad-L15 Pad-B10">
                        <asp:GridView ID="gvServiceMaster" runat="server" AutoGenerateColumns="false" CssClass="gridTableData" GridLines="None" AllowPaging="true" 
                            PageSize="10" OnPageIndexChanging="ServiceMaster_PageIndexChanging">
                            <HeaderStyle CssClass="gridHeader" />
                            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />

                            <Columns>
                                <asp:BoundField DataField="Service_Line_Code" HeaderText="Service Line Code" Visible="false"/>
                                <asp:BoundField DataField="Description" HeaderText="Description" />
                                <asp:BoundField DataField="Active" HeaderText="Is Active" />
                                <asp:BoundField DataField="IMS" HeaderText="IMS" Visible="false"/>
                                <asp:BoundField DataField="RFC" HeaderText="RFC" Visible="false"/>
                                <asp:BoundField DataField="CreatedBy" HeaderText="Created By" />
                                <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" />
                                <asp:BoundField DataField="UpdatedBy" HeaderText="Updated By" />
                                <asp:BoundField DataField="UpdatedDate" HeaderText="Updated Date" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkView" runat="server" Text="Edit" OnClick="lnkView_Click"
                                            CommandArgument='<%# Eval("Service_Line_Code") %>' CausesValidation="false">
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
