using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMS_Net.Admin
{
    public partial class Service_Master : System.Web.UI.Page
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
                btnUpdate.Visible = false;
                FillGridView();
            }
        }

        public void Clear()
        {
            txtServicelineCode.Text = txtDescription.Text = lblErrorMsg.Text = lblSuccessMsg.Text = "";
            rblActive.SelectedValue = "True";
            btnSave.Visible = true;
            btnUpdate.Visible = false;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    cmd = new SqlCommand("ServiceLineInsert", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.AddWithValue("@ServiceLineCode",txtServicelineCode.Text.Trim());
                    cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@Active", rblActive.SelectedValue);
                    cmd.Parameters.AddWithValue("@IMS", "1");
                    cmd.Parameters.AddWithValue("@RFC", "0");
                    cmd.Parameters.AddWithValue("@CreatedBy", a);
                    cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now.ToString());
                    /* cmd.Parameters.Add("@Count", SqlDbType.Int);
                     cmd.Parameters["@Count"].Direction = ParameterDirection.Output;*/

                    con.Open();

                    cmd.ExecuteNonQuery();

                    Clear();

                    lblSuccessMsg.Text = "Saved Successfully.......";

                    FillGridView();
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

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    cmd = new SqlCommand("ServiceLineUpdate", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ServiceLineCode", txtServicelineCode.Text.Trim());
                    cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@Active", rblActive.SelectedValue);
                    cmd.Parameters.AddWithValue("@UpdatedBy", a);
                    cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now.ToString());

                    con.Open();

                    cmd.ExecuteNonQuery();

                    Clear();

                    lblSuccessMsg.Text = "Updated Successfully.......";

                    FillGridView();

                    btnSave.Visible = false;
                }

                else
                {
                    lblErrorMsg.Text = "Error Occured.......";
                }
            }

            catch(Exception ex) { Response.Write("error" + ex.ToString()); }

            finally
            {
                con.Close();
            }   
        }

        public void FillGridView()
        {
            try
            {
                da = new SqlDataAdapter("ServiceLineView", con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;

                dt = new DataTable();

                da.Fill(dt);

                gvServiceLine.DataSource = dt;
                gvServiceLine.DataBind();
            }
            catch(Exception ex) { Response.Write("error" + ex.ToString()); }
        }

        protected void lnkView_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave.Visible = false;
                btnUpdate.Visible = true;
                lblErrorMsg.Text = lblSuccessMsg.Text = "";
                string servicelinecode = Convert.ToString((sender as LinkButton).CommandArgument);

                da = new SqlDataAdapter("ServiceLineGridView", con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;

                da.SelectCommand.Parameters.AddWithValue("@ServiceLineCode",servicelinecode);

                dt = new DataTable();

                con.Open();

                da.Fill(dt);

                //txtServicelineCode.Text = servicelinecode.ToString();
                txtServicelineCode.Text = dt.Rows[0]["Service_Line_Code"].ToString();
                txtDescription.Text = dt.Rows[0]["Description"].ToString();
                rblActive.SelectedValue = dt.Rows[0]["Active"].ToString();
            }
            catch(Exception ex) { Response.Write("error" + ex.ToString()); }

            finally
            {
                con.Close();
            }
        }
    }
}