<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Service_Line_Module_Master.aspx.cs" Inherits="IMS_Net.Admin.Service_Line_Module_Master" %>

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
            <tr id="r_ServiceLineCode" runat="server">
                <td>
                    <asp:Label ID="lblServiceLineCode" runat="server" Text="Service Line Code"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlServiceLineCode" runat="server" DataTextField="Service_Line_Code" DataValueField="Service_Line_Code" AutoPostBack="true"></asp:DropDownList>
                </td>
            </tr>

            <tr style="display:none">
                <td>
                    <asp:Label ID="lblModuleCode" runat="server" Text="Module Code"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtModuleCode" runat="server"></asp:TextBox>
                </td>
            </tr>

            <tr>
                <td>
                    <asp:Label ID="lblModuleDescription" runat="server" Text="Module Description"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtModuleDescription" runat="server" MaxLength="50" TextMode="MultiLine"></asp:TextBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="rfvModuleDescription" runat="server" ControlToValidate="txtModuleDescription" ErrorMessage="Description is required" 
                        ForeColor="red" SetFocusOnError="true"></asp:RequiredFieldValidator>
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
        <asp:Button ID="btnSave" runat="server" Text="Save" CausesValidation="true" OnClick="btnSave_Click"/>
        <asp:Button ID="btnUpdate" runat="server" Text="Update" CausesValidation="true" OnClick="btnUpdate_Click" CommandArgument='<%# Eval("Module_Code") %>'/>
        <asp:Button ID="btnClear" runat="server" Text="Clear" CausesValidation="false" OnClick="btnClear_Click"/>
        <br />
        <asp:Label ID="lblSuccessMsg" runat="server" ForeColor="green"></asp:Label ><br />
        <asp:Label ID="lblErrorMsg" runat="server" ForeColor="red"></asp:Label>
        <br /><br /><br /><br />

        <asp:GridView ID="gvServiceLineModule" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="Service_Line_Code" HeaderText="Service Line Code" />
                <asp:BoundField DataField="Module_Code" HeaderText="Module Code" />
                <asp:BoundField DataField="Module_Description" HeaderText="Description" />
                <asp:BoundField DataField="Active" HeaderText="Active" />
                <asp:BoundField DataField="companyid" HeaderText="Company Id" />
                <asp:BoundField DataField="CreatedBy" HeaderText="Created By" />
                <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" />
                <asp:BoundField DataField="UpdatedBy" HeaderText="Updated By" />
                <asp:BoundField DataField="UpdatedDate" HeaderText="Updated Date" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkView" CausesValidation="false" runat="server" Text="View" OnClick="btnView_Click" CommandArgument='<%# Eval("Module_Code") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>
