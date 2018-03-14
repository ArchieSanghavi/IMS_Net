using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Configuration;

namespace IMS_Net
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        String strConnections = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                btnClick();
                
              
            }
          
        }
        protected void btnClick()
        {
            SqlConnection con = new SqlConnection(strConnections);
            con.Open();
            string query = "SELECT Complaint_Code from COMPLAINT_MASTER";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                string fil1 = rdr.GetString(0);
                CheckBoxList1.Items.Add(fil1);
            }
            rdr.Close();
        }
        protected void btnOk_Click(object sender, EventArgs e)
        {
           
            string YrStr = "";
              for (int i = 0; i < CheckBoxList1.Items.Count; i++)
              {
                  if (CheckBoxList1.Items[i].Selected)
                  {
                      YrStr += CheckBoxList1.Items[i].Value +"\n";
                  }
              }
              // YrStr = String.Join(";", YrStr.ToArray());
              txt.Text = YrStr;
           
        }
    }
}


/*  List<String> YrStrList = new List<string>();
             // Loop through each item.
             foreach (ListItem item in CheckBoxList1.Items)
             {
                 if (item.Selected)
                 {
                     // If the item is selected, add the value to the list.
                     YrStrList.Add(item.Value);
                 }
                 else
                 {
                     // Item is not selected, do something else.
                 }
             }
             // Join the string together using the ; delimiter.
           String YrStr = String.Join(";", YrStrList.ToArray());
             txt.Text = YrStr;
             // Write to the page the value.
           //  Response.Write(String.Concat("Selected Items: ", YrStr));*/
