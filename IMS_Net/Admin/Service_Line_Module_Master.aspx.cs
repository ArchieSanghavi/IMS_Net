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
    public partial class Service_Line_Module_Master : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString);

        SqlCommand cmd;

        SqlDataAdapter da;

        DataTable dt;

        DataSet ds;

        string a = "test";

        #region event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillGridView();
                btnUpdate.Visible = false;
                DropDownList();
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
                if (Page.IsValid)
                {
                    btnUpdate.Visible = false;
                    lblErrorMsg.Text = lblSuccessMsg.Text = "";

                    cmd = new SqlCommand("ServiceLineModuleInsert", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ServiceLineCode", ddlServiceLineCode.SelectedValue);

                    cmd.Parameters.AddWithValue("@ModuleDescription", txtModuleDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@Active", rblActive.SelectedValue);
                    cmd.Parameters.AddWithValue("@CompanyId", 1000);
                    cmd.Parameters.AddWithValue("@CreatedBy", a);
                    cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now.ToString());

                    con.Open();

                    cmd.ExecuteNonQuery();
                    con.Close();
                    Clear();

                    DropDownList();

                    lblSuccessMsg.Text = "Saved Successfully......";

                    FillGridView();
                }
                else
                {
                    lblErrorMsg.Text = "Error occured.......";
                }
            }
            catch (Exception ex) { Response.Write("error" + ex.ToString()); }

           /* finally
            {
                
            }*/
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    btnUpdate.Visible = true;
                    btnSave.Visible = false;
                    lblErrorMsg.Text = lblSuccessMsg.Text = "";

                    string modulecode = Convert.ToString((sender as Button).CommandArgument);

                    cmd = new SqlCommand("ServiceLineModuleUpdate", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.AddWithValue("@Service_Line_Code",ddlServiceLineCode.SelectedValue);
                    cmd.Parameters.AddWithValue("@ModuleCode", modulecode);
                    cmd.Parameters.AddWithValue("@ModuleDescription", txtModuleDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@Active", rblActive.SelectedValue);
                    cmd.Parameters.AddWithValue("@UpdatedBy", a);
                    cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now.ToString());

                    con.Open();

                    cmd.ExecuteNonQuery();

                    Clear();

                    FillGridView();

                    lblSuccessMsg.Text = "Updated Successfully........";
                }
                else
                {
                    lblErrorMsg.Text = "error occured.....";
                }

            }
            catch (Exception ex) { Response.Write("error" + ex.ToString()); }

            /*finally
            {
                con.Close();
            }*/

        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                /*r_ServiceLineCode.Visible=*/btnSave.Visible = false;
                btnUpdate.Visible = true;
                lblErrorMsg.Text = lblSuccessMsg.Text = "";

                string modulecode = Convert.ToString((sender as LinkButton).CommandArgument);

                da = new SqlDataAdapter("ServiceLineModuleGridView", con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;

                da.SelectCommand.Parameters.AddWithValue("@ModuleCode", modulecode);

                dt = new DataTable();

                con.Open();

                da.Fill(dt);

                btnUpdate.CommandArgument = dt.Rows[0]["Module_Code"].ToString();
                ddlServiceLineCode.SelectedValue = dt.Rows[0]["Service_Line_Code"].ToString();
                txtModuleDescription.Text = dt.Rows[0]["Module_Description"].ToString();
                rblActive.SelectedValue = dt.Rows[0]["Active"].ToString();
            }
            catch (Exception ex) { Response.Write("error" + ex.ToString()); }

            finally { con.Close(); }
        }

        #endregion

        #region custom

        public void Clear()
        {
            txtModuleDescription.Text = lblErrorMsg.Text = lblSuccessMsg.Text = "";
            ddlServiceLineCode.ClearSelection();
            rblActive.SelectedValue= "True";
            r_ServiceLineCode.Visible=btnSave.Visible = true;
            btnUpdate.Visible = false;
        }

        protected void DropDownList()
        {
            try
            {
                cmd = new SqlCommand("select Service_Line_Code from Service_Master", con);
                //cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                ddlServiceLineCode.DataSource = cmd.ExecuteReader();
                ddlServiceLineCode.DataBind();
                con.Close();
            }
            catch (Exception ex) { Response.Write(ex); }
        }

        public void FillGridView()
        {
            try
            {
                da = new SqlDataAdapter("ServiceLineModuleView", con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;

                dt = new DataTable();
                
                da.Fill(dt);

                gvServiceLineModule.DataSource = dt;
                gvServiceLineModule.DataBind();
            }
            catch (Exception ex) { Response.Write("error" + ex.ToString()); }
        }

        #endregion
    }
}
