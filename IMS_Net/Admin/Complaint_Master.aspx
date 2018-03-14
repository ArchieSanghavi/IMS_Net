<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Complaint_Master.aspx.cs" Inherits="IMS_Net.Admin.Complaint_Master1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1><b>COMPLAINT MASTER</b></h1>
            <hr />
        </div>
        <table border="0">

            <tr>
                <td style="display: none;">
                    <asp:TextBox ID="txtComplaintCode" runat="server"></asp:TextBox>
                </td>
                <!--  <td>
                    <asp:RequiredFieldValidator ID="rfvComplaintCode" runat="server" ErrorMessage="*Complaint Code is Required" 
                        style="float:left;" ForeColor="Red" ControlToValidate="txtComplaintCode" SetFocusOnError="true"></asp:RequiredFieldValidator>
                </td>-->
            </tr>

            <tr>
                <td>
                    <asp:Label ID="lblDescription" runat="server" Text="Description"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="rfvDescription" ValidationGroup="vgDescription" runat="server" ErrorMessage="*Complaint Code is Required"
                        ForeColor="Red" ControlToValidate="txtDescription" SetFocusOnError="true"></asp:RequiredFieldValidator>
                </td>
            </tr>

            <tr>
                <td>
                    <asp:Label ID="lblSeverityCode" runat="server" Text="Severity"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSeverity" runat="server">
                        <asp:ListItem Text="Critical"></asp:ListItem>
                        <asp:ListItem Text="Minor"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>

            <tr>
                <td>
                    <asp:Label ID="lblActive" runat="server" Text="Active"></asp:Label>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblActive" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="True" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="False"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>

        </table>
        <br />
        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="vgDescription" />&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
        <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" ValidationGroup="vgDescription" CommandArgument='<%# Eval("Complaint_Code") %>' />&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
        <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" CausesValidation="false" />
        <br />
        <asp:Label ID="lblSuccessMsg" runat="server" ForeColor="green"></asp:Label><br />
        <asp:Label ID="lblErrorMsg" runat="server" ForeColor="red"></asp:Label>
        <br />
        <br />
        <br />
        <br />
        <asp:GridView ID="gvComplaint" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="Complaint_code" HeaderText="Complaint code" />
                <asp:BoundField DataField="Description" HeaderText="Complaint Description" />
                <asp:BoundField DataField="Severity_Code" HeaderText="Complaint Severity" />
                <asp:BoundField DataField="Active" HeaderText="Active Status" />
                <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" />
                <asp:BoundField DataField="CreatedBy" HeaderText="Created By" />
                <asp:BoundField DataField="UpdatedDate" HeaderText="Updated Date" />
                <asp:BoundField DataField="UpdatedBy" HeaderText="Updated By" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkView" runat="server" Text="View" CommandArgument='<%# Eval("Complaint_Code") %>' OnClick="lnkView_Click" CausesValidation="false"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>
