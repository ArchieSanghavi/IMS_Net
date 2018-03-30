using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using IMS_Net.Admin.DB;

namespace IMS_Net.Admin
{
    public partial class Service_Line_Module : System.Web.UI.Page
    {
        string strConnections = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;

        SqlCommand cmd;

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString);

        #region event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillGridView();
                r_Error1.Visible = btnUpdate.Visible = false;
                DropDownList();
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataUtility data = null;
            try
            {
                if (Page.IsValid)
                {
                    btnUpdate.Visible = false;
                    SqlParameter[] param ={
                                            new SqlParameter("@ServiceLineCode", ddlServiceLineCode.SelectedValue),
                                            new SqlParameter("@ModuleDescription", txtModuleDescription.Text.Trim()),
                                            new SqlParameter("@Active", rblActive.SelectedValue),
                                            new SqlParameter("@CompanyId", 1000),
                                            new SqlParameter("@CreatedDate", DateTime.Now.ToString()),
                                            new SqlParameter("@CreatedBy", Session["username"])
                                          };

                    data = new DataUtility();

                    data.ExecuteProc("ServiceLineModuleInsert", param);

                    Clear();

                    DropDownList();

                    r_Error1.Visible = true;
                    lblSuccessMsg.Text = "Saved Successfully......";

                    FillGridView();
                }
                else
                {
                    r_Error1.Visible = true;
                    lblErrorMsg.Text = "Error occured.......";
                }
            }
            catch { throw; }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                DataUtility data = null;
                string modulecode = Convert.ToString((sender as Button).CommandArgument);
                if (Page.IsValid)
                {
                    btnUpdate.Visible = true;
                    btnSave.Visible = false;
                    lblErrorMsg.Text = lblSuccessMsg.Text = "";


                    SqlParameter[] param ={
                                            new SqlParameter("@ServiceLineCode", ddlServiceLineCode.SelectedValue),
                                            new SqlParameter("@ModuleCode", modulecode),
                                            new SqlParameter("@ModuleDescription", txtModuleDescription.Text.Trim()),
                                            new SqlParameter("@Active", rblActive.SelectedValue),
                                            new SqlParameter("@UpdatedDate", DateTime.Now.ToString()),
                                            new SqlParameter("@UpdatedBy", Session["username"])
                                          };

                    data = new DataUtility();

                    data.ExecuteProc("ServiceLineModuleInsert", param);

                    Clear();

                    FillGridView();
                    r_Error1.Visible = true;
                    lblSuccessMsg.Text = "Updated Successfully........";
                }
                else
                {
                    r_Error1.Visible = true;
                    lblErrorMsg.Text = "error occured.....";
                }

            }
            catch { throw; }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            try
            {
                btnSave.Visible = false;
                btnUpdate.Visible = true;
                lblErrorMsg.Text = lblSuccessMsg.Text = "";

                string modulecode = Convert.ToString((sender as LinkButton).CommandArgument);
                SqlParameter[] param ={
                                            new SqlParameter("@ModuleCode", modulecode)
                                      };
                ds = new DataSet();
                ds = IMS_Net.Admin.Utility3.Database.GetData(strConnections, "ServicelineModuleGridView", param);

                btnUpdate.CommandArgument = ds.Tables[0].Rows[0]["Module_Code"].ToString();
                ddlServiceLineCode.SelectedValue = ds.Tables[0].Rows[0]["Service_Line_Code"].ToString();
                txtModuleDescription.Text = ds.Tables[0].Rows[0]["Module_Description"].ToString();
                rblActive.SelectedValue = ds.Tables[0].Rows[0]["Active"].ToString();
            }
            catch { throw; }
        }

        #endregion

        #region custom

        public void Clear()
        {
            txtModuleDescription.Text = lblErrorMsg.Text = lblSuccessMsg.Text = "";
            ddlServiceLineCode.ClearSelection();
            rblActive.SelectedValue = "True";
            r_ServiceLineCode.Visible = btnSave.Visible = true;
            r_Error1.Visible = btnUpdate.Visible = false;
        }

        protected void DropDownList()
        {
            try
            {
                cmd = new SqlCommand("select Service_Line_Code,Description from Service_Master where Active='true'", con);
                //cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                ddlServiceLineCode.DataSource = cmd.ExecuteReader();
                ddlServiceLineCode.DataBind();
                con.Close();
            }
            catch { throw; }
        }

        public void FillGridView()
        {
            DataSet ds = null;
            try
            {
                ds = new DataSet();

                SqlParameter[] param ={
                                            new SqlParameter("@ModuleCode", DBNull.Value)
                                      };

                ds = IMS_Net.Admin.Utility3.Database.GetData(strConnections, "ServiceLineModuleGridView", param);

                gvServiceLineModule.DataSource = ds.Tables[0];
                gvServiceLineModule.DataBind();
            }
            catch { throw; }
        }

        public void ServiceLineModule_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvServiceLineModule.PageIndex = e.NewPageIndex;
            this.FillGridView();
        }
        #endregion
    }
}