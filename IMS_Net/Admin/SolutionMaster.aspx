<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/IMSNetMaster.Master" AutoEventWireup="true" CodeBehind="SolutionMaster.aspx.cs" Inherits="IMS_Net.Admin.SolutionMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
          <asp:UpdatePanel ID="updatePanel2" runat="server" EnableViewState="true">
   <ContentTemplate>
    
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
        
        <tr>
            <td colspan="4" class="tdHeading">
                <asp:Label ID="Label1" runat="server" Text="Solution Master " CssClass="heading"></asp:Label>
            </td>
        </tr>
        <tr id="r_Error1" runat="server" visible="false" >
            <td colspan="4" align="left" class="label Pad-T10 Pad-L15 Pad-B10" style="height: 4px">
                <asp:Label ID="lblinsert" runat="server" ForeColor="Green"></asp:Label>
                <asp:Label ID="lblupdate" runat="server" ForeColor="Green"></asp:Label>
            </td>
        </tr>
        
        <tr>
            <td colspan="4" style="height: 25px" class="label Pad-L15 Pad-T5">
                <asp:Label ID="Label6" runat="server" Text="(*) Marked fields are mandatory " Height="21px"
                    ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>

            <td class="label Pad-L15 Pad-T10" align="left" valign="top" style="white-space: nowrap;" width="15%">
                <asp:Label ID="lbldescription" runat="server">Description:</asp:Label>
                <asp:Label ID="lblerror" runat="server" ForeColor="Red">*</asp:Label>
            </td>
            <td align="left" class="textbox Pad-T10">
                 <asp:TextBox ID="txtdescription" runat="server" Height="70px" Columns="35" MaxLength="150" TextMode="MultiLine"></asp:TextBox>
                   <asp:RequiredFieldValidator ID="RequiredFieldValidator3"  runat="server" ForeColor="Red"  ToolTip="Please fill Description"  Display="Dynamic" SetFocusOnError="True" EnableClientScript="true" ValidationGroup="vg"  ControlToValidate="txtdescription" ErrorMessage="*"></asp:RequiredFieldValidator>
              
               </td>
            <td class="label Pad-T10" align="left" valign="top" width="15%">
                <asp:Label ID="lblactive" runat="server">Active:</asp:Label>
            </td>
            <td class="textbox Pad-T10" align="left" style="vertical-align: top">
               <asp:RadioButtonList ID="rblactive" runat="server" Style="padding-left: 17px;" RepeatDirection="Horizontal">
                    <asp:ListItem Selected="True">True</asp:ListItem>
                    <asp:ListItem>False</asp:ListItem>
               </asp:RadioButtonList>

           </td>
        </tr>
        <tr>
            <td class="label Pad-L15 Pad-T10" align="left" width="15%">
                 <asp:Label ID="lblsolutioncode" Visible="false" runat="server" >Solution Code:</asp:Label>
            </td>
            <td class="textbox Pad-T10" align="left" width="25%">
                 <asp:TextBox ID="txtsolutioncode" Visible="false" runat="server" Width="228px" MaxLength="20"></asp:TextBox>
            </td>
            <td colspan="2" class="label Pad-T10 Pad-L15 Pad-B10"></td>
           </tr>
        
        <tr>
            <td colspan="4" class="label Pad-T10 Pad-L15 Pad-B10">
                        <asp:Button ID="btnsave" runat="server" Text="Save" Width="70px" OnClick="btnsave_Click" ValidationGroup="vg" CssClass="clsButton" />
                        <asp:Button ID="btnupdate" runat="server" Text="Update" Width="70px" ValidationGroup="vg" OnClick="btnupdate_Click" CssClass="clsButton" />
                        <asp:Button ID="btnclear" runat="server" Text="Clear" Width="70px" OnClick="btnclear_Click" CssClass="clsButton" />
                    </td>

        </tr>
        <tr>
            <td colspan="4" class="label Pad-T10 Pad-L15 Pad-B10">
                   
             <asp:GridView ID="GridViewDisplay" runat="server"  CssClass="gridTableData"  GridLines="None" AutoGenerateColumns="false" 
               PageSize="10" AllowPaging="true" OnPageIndexChanging="GridPageChange" >
                 <HeaderStyle CssClass="gridHeader" />
                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />

                <Columns>
                    <asp:BoundField DataField="solution_code" HeaderText="Solution Code" />
                    <asp:BoundField DataField="Description" HeaderText="Description" />
                    <asp:BoundField DataField="Active" HeaderText="Active" />
                    <asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" />
                    <asp:BoundField DataField="CreatedDate" HeaderText="CreatedDate" />
                    <asp:BoundField DataField="UpdatedBy" HeaderText="UpdatedBy" />
                    <asp:BoundField DataField="UpdatedDate" HeaderText="UpdatedDate" />
                     <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkbtn" CommandName="Select" Text="Edit" runat="server" OnClick="lnkbtn_Click"  CommandArgument='<%# Eval("solution_code") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                   
               </Columns>
            </asp:GridView>
             
          </td>
       </tr>

    </table>
           </ContentTemplate>
   
</asp:UpdatePanel>
 

   
    <script type="text/javascript">
        function HideinsertLabel() {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("<%=lblinsert.ClientID %>").style.display = "none";
            }, seconds * 500);

        };

        function HideupdateLabel() {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("<%=lblupdate.ClientID %>").style.display = "none";
            }, seconds * 500);
        };
        function HideupdateLabel() {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("<%=r_Error1.ClientID %>").style.display = "none";
            }, seconds * 500);
        };
        function HideinsertLabel() {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("<%=r_Error1.ClientID %>").style.display = "none";
            }, seconds * 500);
        };
      
</script>
   
</asp:Content>
