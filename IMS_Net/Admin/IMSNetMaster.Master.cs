using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Data.SqlClient;
using System.Web.Services;
using System.Collections.Generic;

namespace IMS_Net.Admin
{
    public partial class IMSNetMaster : System.Web.UI.MasterPage
    {
        #region variableDeclare

        string strCon = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        SqlConnection sqlCon;

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["username"] = "test";
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            //tViewMenu.ExpandDepth = 1;
        }

        private void GetUserName(string userID)
        {
            DataSet ds = new DataSet();
            string strQuery = "Select Name From User_Identity Where User_ID = '" + userID + "'";
            ds = Utility.Database.FindUser(strQuery, strCon);
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //lblUserName.Text = ds.Tables[0].Rows[0]["Name"].ToString() + "(" + Session["RoleName"].ToString().Trim() + ")";

                    //  lblWelcomeMsg.Text = "Welcome:";
                    // lblWelcomeMsg.Visible = true;
                    // lblUserName.Visible = true;
                }
            }

        }

        private void BindMenu()
        {
            //if (Session["UserID"].ToString() != "")
            //{
            //    string role = Session["Role"].ToString();
            //    tViewMenu.Nodes.Clear();
            //    TreeNode rootNode;
            //    rootNode = Utility2.PopulateTreeview.PopulateTreeView(role);
            //    tViewMenu.Nodes.Add(rootNode);
            //    tViewMenu.ExpandAll();
            //    td_TreeView.Visible = true;
            //}
            //else
            //{
            //    tViewMenu.Nodes.Clear();
            //}
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            Response.Redirect("../../IMS_NET/pageauth/Logout.aspx");
        }

        [WebMethod]
        public static string[] GetClients(string prefix)
        {


            DataTable dt = new DataTable();
            List<string> customers = new List<string>();
            SqlParameter[] paramList = {
                                       new SqlParameter("@User",prefix.Trim())
        };

            dt = Utility3.Database.GetData(Convert.ToString(ConfigurationManager.ConnectionStrings["ConStr"]), "GetUsersList", paramList).Tables[0];

            DataTableReader sdr = dt.CreateDataReader();

            using (sdr)
            {
                while (sdr.Read())
                {
                    customers.Add(string.Format("{0}${1}", sdr["Name"], sdr["Emp_Code"]));
                }
            }

            return customers.ToArray();
        }

    }

}
