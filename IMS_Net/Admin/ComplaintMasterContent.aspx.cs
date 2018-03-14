using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using IMS_Net.Admin.DB;

namespace IMS_Net.Admin
{
    public partial class ComplaintMaster : System.Web.UI.Page
    {
        string strConnections = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;

        SqlCommand cmd;

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString);

        #region event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                r_Error1.Visible = btnUpdate.Visible = false;
                FillGridView();
                Severity();
            }

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DataUtility data = null;
                if (Page.IsValid)
                {
                    btnUpdate.Visible = false;
                    SqlParameter[] param ={
                                            new SqlParameter("@ComplaintDescription", txtDescription.Text.Trim()),
                                            new SqlParameter("@ComplaintSeverity", ddlSeverity.SelectedValue),
                                            new SqlParameter("@StatusActive", rblActive.SelectedValue),
                                            new SqlParameter("@CreatedDate", DateTime.Now.ToString()),
                                            new SqlParameter("@CreatedBy", Session["username"])
                                          };

                    data = new DataUtility();

                    data.ExecuteProc("ComplaintInsert", param);

                    Clear();

                    Severity();

                    r_Error1.Visible = true;

                    lblSuccessMsg.Text = "Saved Successfully.......";

                    FillGridView();
                }
                else
                {
                    r_Error1.Visible = true;
                    lblErrorMsg.Text = "Error Occured.......";
                }
            }

            catch { throw; }
        }

        protected void lnkView_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            try
            {
                btnSave.Visible = false;
                r_Error1.Visible = false;
                lblSuccessMsg.Text = lblErrorMsg.Text = "";
                btnUpdate.Visible = true;

                string ComplaintCode = Convert.ToString((sender as LinkButton).CommandArgument);
                SqlParameter[] param ={
                                            new SqlParameter("@ComplaintCode", ComplaintCode)
                                      };
                ds = new DataSet();
                ds = IMS_Net.Admin.Utility3.Database.GetData(strConnections, "ComplaintGridView", param);
                /*da.SelectCommand.Parameters.AddWithValue("@ServiceLineCode", servicelinecode);*/
                //dt = new DataTable();
                //dt = ds.Tables[0];
                /* da.Fill(dt);*/

                btnUpdate.CommandArgument = ds.Tables[0].Rows[0]["Complaint_Code"].ToString();
                txtDescription.Text = ds.Tables[0].Rows[0]["Description"].ToString();
                ddlSeverity.SelectedValue = ds.Tables[0].Rows[0]["Severity_Code"].ToString();
                rblActive.SelectedValue = ds.Tables[0].Rows[0]["Active"].ToString();
            }

            catch { throw; }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave.Visible = false;
                string ComplaintCode = Convert.ToString((sender as Button).CommandArgument);
                DataUtility data = null;
                if (Page.IsValid)
                {
                    SqlParameter[] param ={
                                            new SqlParameter("@ComplaintCode", ComplaintCode),
                                            new SqlParameter("@ComplaintDescription", txtDescription.Text.Trim()),
                                            new SqlParameter("@ComplaintSeverity", ddlSeverity.SelectedValue),
                                            new SqlParameter("@StatusActive", rblActive.SelectedValue),
                                            new SqlParameter("@UpdatedDate", DateTime.Now.ToString()),
                                            new SqlParameter("@UpdatedBy", Session["username"])
                                          };
                    data = new DataUtility();

                    data.ExecuteProc("ComplaintInsert", param);

                    Clear();
                    //  Severity();
                    FillGridView();

                    r_Error1.Visible = true;

                    lblSuccessMsg.Text = "Updated Successfully.......";
                }

                else
                {
                    r_Error1.Visible = true;
                    lblErrorMsg.Text = "Error Occured.......";
                }
            }
            catch { throw; }
        }

        #endregion

        #region custom method

        void FillGridView()
        {
            DataSet ds = null;
            try
            {
                ds = new DataSet();

                SqlParameter[] param ={
                                            new SqlParameter("@ComplaintCode", DBNull.Value)
                                      };

                ds = IMS_Net.Admin.Utility3.Database.GetData(strConnections, "ComplaintGridView", param);

                gvComplaint.DataSource = ds.Tables[0];
                gvComplaint.DataBind();
            }

            catch { throw; }
        }

        public void Clear()
        {
            txtDescription.Text = lblErrorMsg.Text = lblSuccessMsg.Text = "";
            ddlSeverity.ClearSelection();
            btnSave.Visible = true;
            btnUpdate.Visible = r_Error1.Visible = false;
            rblActive.SelectedValue = "True";
        }

        public void ComplaintMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvComplaint.PageIndex = e.NewPageIndex;
            this.FillGridView();
        }

        protected void Severity()
        {
            try
            {
                cmd = new SqlCommand("select Severity_Code,Description from SEVERITY_MASTER", con);
                //cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                ddlSeverity.DataSource = cmd.ExecuteReader();
                ddlSeverity.DataBind();
                con.Close();
            }
            catch { throw; }
        }
        #endregion
    }
}