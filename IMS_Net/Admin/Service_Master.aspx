<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Service_Master.aspx.cs" Inherits="IMS_Net.Admin.Service_Master" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <table border="0">
            <tr>
                <td style="display:none">
                    <asp:TextBox ID="txtServicelineCode" runat="server" MaxLength="2"></asp:TextBox>
                </td>
                <!--<td>
                    <asp:RequiredFieldValidator ID="rfServiceLineCode" runat="server" ErrorMessage="Service Line Code Required" 
                        ControlToValidate="txtServiceLineCode" ForeColor="red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                </td>-->
            </tr>

            <tr>
                <td>
                    <asp:Label ID="lblDescription" runat="server" Text="Description"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine"></asp:TextBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="rfDescription" runat="server" ValidationGroup="vgDescription" ErrorMessage="Description is Required"
                        ControlToValidate="txtDescription" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                </td>
            </tr>

            <tr>
                <td>
                    <asp:Label ID="lblActive" runat="server" Text="Active"></asp:Label>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblActive" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="True" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="False" ></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
        <br />
        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="vgDescription"/>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
        <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" ValidationGroup="vgDescription"/>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
        <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" CausesValidation="false"/>
        <br />
        <asp:Label ID="lblSuccessMsg" runat="server" ForeColor="Green"></asp:Label>
        <br />
        <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red"></asp:Label>
        <br /><br /><br /><br /><br />
        
        <asp:GridView ID="gvServiceLine" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="Service_Line_Code" HeaderText="Service Line Code" />
                <asp:BoundField DataField="Description" HeaderText="Description" />
                <asp:BoundField DataField="Active" HeaderText="Active" />
                <asp:BoundField DataField="IMS" HeaderText="IMS" />
                <asp:BoundField DataField="RFC" HeaderText="RFC" />
                <asp:BoundField DataField="CreatedBy" HeaderText="Created By" />
                <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" />
                <asp:BoundField DataField="UpdatedBy" HeaderText="Updated By" />
                <asp:BoundField DataField="UpdatedDate" HeaderText="Updated Date" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkView" runat="server" Text="View" OnClick="lnkView_Click" CommandArgument='<%# Eval("Service_Line_Code") %>' CausesValidation="false"></asp:LinkButton>
                        
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>
