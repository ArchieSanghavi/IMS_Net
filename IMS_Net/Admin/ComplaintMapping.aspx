<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/IMSNetMaster.Master" AutoEventWireup="true" CodeBehind="ComplaintMapping.aspx.cs" Inherits="IMS_Net.Admin.ComplaintMapping" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <asp:UpdatePanel ID="updatePanel2" runat="server" EnableViewState="true">
   <ContentTemplate>
   
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
           
        <tr>
            <td colspan="4" class="tdHeading">
                <asp:Label ID="Label1" runat="server" Text="Complaint Mapping" CssClass="heading"></asp:Label>
            </td>
        </tr>
        <tr id="r_Error1" runat="server" visible="false">
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
            <td class="label Pad-L15 Pad-T10" align="left" width="15%">
                <asp:Label ID="lblservicelinecode" runat="server">Service Line Code:</asp:Label>
                <asp:Label ID="lblmark" runat="server" ForeColor="Red">*</asp:Label>
            </td>
            <td class="textbox Pad-T10" align="left" width="25%">

                <asp:DropDownList ID="ddlservicelinecode" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlservicelinecode_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvservicelinecode" runat="server" ErrorMessage="please select a service-line-code" ValidationGroup="vg"
                    Display="Dynamic" ControlToValidate="ddlservicelinecode" InitialValue="0" ToolTip="please select a service-line-code" ForeColor="Red"
                    SetFocusOnError="True">*</asp:RequiredFieldValidator>
                <asp:HiddenField ID="hfservicelinecode" runat="server" />
            </td>

            <td class="label  Pad-T10" align="left" width="15%" valign="top">
                <asp:Label ID="lblmodulecode" runat="server">Module Code:</asp:Label>
                <asp:Label ID="Label3" runat="server" ForeColor="Red">*</asp:Label>
            </td>
            <td class="textbox Pad-T10" align="left" width="25%" valign="top">
                <asp:DropDownList ID="ddlmodulecode" runat="server">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvmodulecode" runat="server" ErrorMessage="please select a module-code" ValidationGroup="vg"
                    Display="Dynamic" ControlToValidate="ddlmodulecode" InitialValue="0" ToolTip="please select a module-code" ForeColor="Red"
                    SetFocusOnError="True">*</asp:RequiredFieldValidator>
                <asp:HiddenField ID="hfmodulecode" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="label Pad-L15 Pad-T10 " align="left" width="15%" valign="top">
                <asp:Label ID="lblcomplaintcode" runat="server">Complaint Code:</asp:Label>
                <asp:Label ID="Label4" runat="server" ForeColor="Red">*</asp:Label>
            </td>
            <td class="textbox Pad-T10" align="left" width="25%">
                <asp:Button runat="server" ID="btnpopup" Text="Select Complaint" Width="150px" CausesValidation="false" CssClass="clsButton" Style="vertical-align: top;" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" CancelControlID="btnCancel" OkControlID="btnOkay"
                    TargetControlID="btnpopup" PopupControlID="Panel1" PopupDragHandleControlID="Panel1" Drag="true" BackgroundCssClass="ModalPopupBG">
                </ajaxToolkit:ModalPopupExtender>

                 <asp:Panel ID="Panel1" Style="display: none" runat="server">
                    <div style="min-width: 200px; min-height: 150px; background: gray; border: solid;">
                        <div class="PopupHeader" id="PopupHeader" style="text-align: center;">complaint_code</div>
                        <div class="PopupBody">
                            Search: 
                            <asp:TextBox ID="txtsearch" runat="server" OnTextChanged="search_textChanged" AutoPostBack="true" AutoCompleteType="Search"></asp:TextBox>
                        </div>
                        <div class="PopupBody" style="overflow-y: scroll; height: 100px;">
                                    <asp:GridView ID="popupgv" runat="server" AutoGenerateColumns="false" CssClass="gridTableData">
                                        <HeaderStyle CssClass="gridHeader" />

                                        <Columns>
                                            <asp:TemplateField HeaderText="select">
                                                <ItemTemplate>
                                                    <asp:CheckBox runat="server" ID="chkpopup"  />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Description" HeaderText="Complaint Description" />
                                            <asp:BoundField DataField="Complaint_Code" HeaderText="Complaint Code" />
                                        </Columns>
                                    </asp:GridView>
                          
                        </div>
                        <div class="Controls">
                            <asp:Button ID="btnOkay" runat="server" Text="OK" CssClass="clsButton" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="clsButton" />
                        </div>
                    </div>
                </asp:Panel>
 

                <asp:TextBox runat="server" ID="txtcompaintcode" TextMode="MultiLine" ReadOnly="true" Visible="false"></asp:TextBox>

                <asp:HiddenField ID="hfcomplaintcode" runat="server" />

            </td>
            <td class="label  Pad-T10" colspan="2"></td>
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

                <asp:GridView ID="gvcomplaintmapping" runat="server" CssClass="gridTableData" GridLines="None" AutoGenerateColumns="false"
                    PageSize="10" AllowPaging="true" OnPageIndexChanging="GridPageChange">
                    <HeaderStyle CssClass="gridHeader" />
                    <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />

                    <Columns>
                        <asp:BoundField DataField="Service_Line_Code" HeaderText="Service Line Code" Visible="false" />
                        <asp:BoundField DataField="ServiceLine" HeaderText="Service Line Description" />
                        <asp:BoundField DataField="Module_Code" HeaderText="Module Code" Visible="false" />
                        <asp:BoundField DataField="Module" HeaderText="Module Description" />
                        <asp:BoundField DataField="complaint_code" HeaderText="Complaint Code" Visible="false" />
                        <asp:BoundField DataField="Complaint" HeaderText="Complaint Description" />
                        <asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" />
                        <asp:BoundField DataField="CreatedDate" HeaderText="CreatedDate" />
                        <asp:BoundField DataField="UpdatedBy" HeaderText="UpdatedBy" />
                        <asp:BoundField DataField="UpdatedDate" HeaderText="UpdatedDate" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtn" CommandName="Select" Text="Edit" CommandArgument='<%#Eval("Service_Line_Code") + ";" +Eval("Module_Code")+ ";" +Eval("complaint_code")%>' runat="server" OnClick="lnkbtn_Click" />
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
