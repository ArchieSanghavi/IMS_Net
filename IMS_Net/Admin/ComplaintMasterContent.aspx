<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/IMSNetMaster.Master" AutoEventWireup="true" CodeBehind="ComplaintMasterContent.aspx.cs" Inherits="IMS_Net.Admin.ComplaintMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <asp:UpdatePanel ID="udpComplaintMaster" runat="server"><ContentTemplate>

       <tr>
                    <td colspan="4" class="tdHeading">
                        <asp:Label ID="Label1" runat="server" Text="Complaint Master " CssClass="heading"></asp:Label>
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

                <tr>
                    <td class="label Pad-L15 Pad-T10" align="left" width="15%" valign="top" style="white-space: nowrap">
                        <asp:Label ID="lblDescription" runat="server" Text="Description:"></asp:Label>
                        <asp:Label ID="lblerror" runat="server" ForeColor="Red">*</asp:Label>
                    </td>
                    <td class="textbox Pad-T10" width="25%" valign="top" style="white-space: nowrap">
                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" MaxLength="50" Rows="5" Width="228px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvDescription" ValidationGroup="vgDescription" runat="server" ErrorMessage="*Complaint Code is Required"
                            ForeColor="Red" ControlToValidate="txtDescription" SetFocusOnError="true" ToolTip="Please fill Description" Display="Dynamic" EnableClientScript="true">
                        </asp:RequiredFieldValidator>
                    </td>

                    <td class="label Pad-T10 " align="left" width="15%" valign="top" style="white-space: nowrap">
                        <asp:Label ID="lblSeverityCode" runat="server" Text="Severity:"></asp:Label>
                    </td>
                    <td class="textbox Pad-T10" align="left" width="25%" valign="top" style="white-space: nowrap">
                        <asp:DropDownList ID="ddlSeverity" runat="server" DataTextField="Description" DataValueField="Severity_Code" AutoPostBack="true" Width="228px"></asp:DropDownList>
                    </td>
                </tr>

                <tr>
                    <td class="label Pad-L15 Pad-T10" align="left" width="15%" valign="top" style="white-space: nowrap">
                        <asp:Label ID="lblActive" runat="server" Text="Active:"></asp:Label>
                    </td>
                    <td class="textbox Pad-T10" width="25%" valign="top" style="white-space: nowrap" align="left">
                        <asp:RadioButtonList ID="rblActive" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Text="True" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="False"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td colspan="2" class="label Pad-T10 Pad-L15 Pad-B10"></td>
                    <!--<td class="label Pad-T10 " align="left" width="15%"></td>
             <td class="textbox Pad-T10" align="left" width="25%"></td>-->
                </tr>

                <tr>
                    <td colspan="4" class="label Pad-T10 Pad-L15 Pad-B10">&nbsp;
                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="vgDescription" Width="70px" CssClass="clsButton" />
                        <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" ValidationGroup="vgDescription" CommandArgument='<%# Eval("Complaint_Code") %>' Width="70px" CssClass="clsButton" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" CausesValidation="false" Width="70px" CssClass="clsButton" />
                    </td>
                </tr>

                <tr>
                    <td colspan="4" class="label Pad-T10 Pad-L15 Pad-B10">
                        <asp:GridView ID="gvComplaint" runat="server" AutoGenerateColumns="false" CssClass="gridTableData" GridLines="None" AllowPaging="true" PageSize="10" OnPageIndexChanging="ComplaintMaster_PageIndexChanging">
                            <HeaderStyle CssClass="gridHeader" />
                            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                            <Columns>
                                <asp:BoundField DataField="Complaint_code" HeaderText="Complaint code" Visible="false"/>
                                <asp:BoundField DataField="Description" HeaderText="Complaint Description" />
                                <asp:BoundField DataField="Severity" HeaderText="Complaint Severity" />
                                <asp:BoundField DataField="Active" HeaderText="Is Active" />
                                <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" />
                                <asp:BoundField DataField="CreatedBy" HeaderText="Created By" />
                                <asp:BoundField DataField="UpdatedDate" HeaderText="Updated Date" />
                                <asp:BoundField DataField="UpdatedBy" HeaderText="Updated By" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkView" runat="server" Text="Edit" CommandArgument='<%# Eval("Complaint_Code") %>' 
                                            OnClick="lnkView_Click" CausesValidation="false"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>

      </ContentTemplate></asp:UpdatePanel>
        </table>

</asp:Content>
