<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="IMS_Net.WebForm1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .ModalPopupBG
{
    filter: alpha(opacity=50);
    opacity: 0.2;
}

.HellowWorldPopup
{
    min-width:200px;
    min-height:150px;
    background:gray;
    border:solid;
}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
   <asp:scriptmanager id="ScriptManager1" runat="server">
</asp:scriptmanager>

<asp:button id="Button1" runat="server" text="Button" />

<ajaxToolkit:ModalPopupExtender id="ModalPopupExtender1" runat="server" 

	cancelcontrolid="btnCancel"  

	targetcontrolid="Button1" popupcontrolid="Panel1" 

	popupdraghandlecontrolid="Panel1" drag="true" 

	backgroundcssclass="ModalPopupBG">
</ajaxToolkit:ModalPopupExtender>

<asp:panel id="Panel1" style="display: none" runat="server">
  <div class="HellowWorldPopup">
                <div class="PopupHeader" id="PopupHeader">complaint_code</div>
                <div class="PopupBody">
                    <asp:CheckBoxList ID="CheckBoxList1" runat="server">
                       </asp:CheckBoxList>
                </div>
                <div class="Controls">
                    <asp:button id="btnOkay" runat="server" text="Done" OnClick="btnOk_Click" />
                    <asp:button id="btnCancel" runat="server" text="Cancel" />

                 <%--   <input id="btnOkay" type="button" value="Done"   onclick="btnOk_Click" />
                    <input id="btnCancel" type="button" value="Cancel" />--%>
		</div>
        </div>
</asp:panel>
        <asp:TextBox runat="server" ID="txt" TextMode="MultiLine"  ></asp:TextBox>

        
    </div>
    </form>
    <script type="text/javascript">
        function pageLoad() {
            HidePopup();
           // setTimeout(HidePopup,0);
        }

        function ShowPopup() {
           // $find('modalpopup').show();
            $get('Button1').click();
        }

        function HidePopup() {
            //$find('modalpopup').hide();
            $get('btnCancel').click();
        }
</script>
</body>
</html>


<%-- 
con = new SqlConnection(connectionstring);
        con.Open();
        string comboquery = "SELECT [Machine] FROM Department Where active='True'";
        SqlCommand cmd = new SqlCommand(comboquery, con);
        SqlDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            string fil1 = rdr.GetString(0);
            Checkedlistbox1.Items.Add(fil1);
        }
        rdr.Close();


while (rdr.Read())
{
   string mac = rdr.GetString(0);
   ListItem li = new ListItem();
   li.Value = "yourBindedValue";// some value from database column
   li.Text = "yourBindedText";// use mac if its text.
   int index = Checkedlistbox1.Items.IndexOf(li);
   if (index >= 0)
   {
      Checkedlistbox1.SetItemChecked(index, true);
   }


txtIDNumber.Text = frmSearchPopUp.userSelectedResult;
}--%>