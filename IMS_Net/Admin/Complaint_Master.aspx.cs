using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace IMS_Net.Admin
{
    public partial class Complaint_Master1 : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["IMS_NetDB"].ConnectionString);

        SqlCommand cmd;

        SqlDataAdapter da;

        DataTable dt;

        string a = "test";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillGridView();
                btnUpdate.Visible = false;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        public void Clear()
        {
            txtComplaintCode.Text = txtDescription.Text = lblErrorMsg.Text = lblSuccessMsg.Text = "";
            ddlSeverity.ClearSelection();
            btnSave.Visible = true;
            btnUpdate.Visible = false;
            rblActive.SelectedValue = "True";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    btnSave.Visible = true;

                    con.Open();

                    cmd = new SqlCommand("ComplaintInsert", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ComplaintDescription", txtDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@ComplaintSeverity", ddlSeverity.SelectedValue);
                    cmd.Parameters.AddWithValue("@StatusActive", rblActive.SelectedValue);
                    cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now.ToString());
                    cmd.Parameters.AddWithValue("@CreatedBy", a);
                    /*cmd.Parameters.Add("@Result", SqlDbType.Int);
                    cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                    txtComplaintCode.Text= (string)cmd.Parameters["@Result"].Value;*/
                    cmd.ExecuteNonQuery();

                    Clear();

                    FillGridView();


                    lblSuccessMsg.Text = "Saved Successfully.......";
                }

                else
                {
                    lblErrorMsg.Text = "Error Occured.......";
                }
            }

            catch (Exception ex) { Response.Write(ex); }

            finally
            {
                con.Close();
            }
        }

        protected void lnkView_Click(object sender, EventArgs e)
        {
            try
            {
                string ComplaintCode = Convert.ToString((sender as LinkButton).CommandArgument);

                con.Open();

                lblSuccessMsg.Text = "";
                lblErrorMsg.Text = "";
                da = new SqlDataAdapter("ComplaintGridView", con);

                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@ComplaintCode", ComplaintCode);

                dt = new DataTable();

                da.Fill(dt);

                btnUpdate.CommandArgument = dt.Rows[0]["Complaint_Code"].ToString();
               // txtComplaintCode.Text = dt.Rows[0]["Complaint_Code"].ToString();
                txtDescription.Text = dt.Rows[0]["Description"].ToString();
                ddlSeverity.SelectedValue = dt.Rows[0]["Severity_Code"].ToString();
                rblActive.SelectedValue = dt.Rows[0]["Active"].ToString();

                btnSave.Visible = false;
                btnUpdate.Visible = true;

            }

            catch (Exception ex) { Response.Write("error" + ex.ToString()); }

            finally { con.Close(); }
        }

        void FillGridView()
        {
            try
            {
                //con.Open();

                da = new SqlDataAdapter("ComplaintView", con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;

                dt = new DataTable();

                da.Fill(dt);
                gvComplaint.DataSource = dt;
                gvComplaint.DataBind();
            }

            catch (Exception ex) { Response.Write("error" + ex.ToString()); }

            finally
            {
               // con.Close();
            }

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            { 
                if (Page.IsValid)
                {
                    string ComplaintCode = Convert.ToString((sender as Button).CommandArgument);

                    con.Open();

                    cmd = new SqlCommand("ComplaintUpdate", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.AddWithValue("@ComplaintCode", txtComplaintCode.Text);
                    cmd.Parameters.AddWithValue("@ComplaintCode", ComplaintCode);
                    cmd.Parameters.AddWithValue("@ComplaintDescription", txtDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@ComplaintSeverity", ddlSeverity.SelectedValue);
                    cmd.Parameters.AddWithValue("@StatusActive", rblActive.SelectedValue);
                    cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now.ToString());
                    cmd.Parameters.AddWithValue("@UpdatedBy", a);
                    cmd.ExecuteNonQuery();

                    Clear();

                    FillGridView();

                    lblSuccessMsg.Text = "Updated Successfully.......";
                }

                else
                {
                    lblErrorMsg.Text = "Error Occured.......";
                }
            }
            catch (Exception ex) { Response.Write("error" + ex.ToString()); }

            finally
            {
                con.Close(); 
            }
        }
    }
}