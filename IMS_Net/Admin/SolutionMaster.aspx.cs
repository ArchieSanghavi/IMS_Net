using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using IMS_Net.Admin.DB;


namespace IMS_Net.Admin
{
    public partial class SolutionMaster : System.Web.UI.Page
    {
       
        string strConnections = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;

       
        #region Events

        protected void btnclear_Click(object sender, EventArgs e)
        {
            ClearData();
           
            btnupdate.Visible = false;
           
            btnsave.Visible = true;

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblinsert.Text = "";
            lblupdate.Text = "";
            ScriptManager.RegisterStartupScript(
                        updatePanel2,
                        this.GetType(),
                        "MyAction",
                        "HideupdateLabel();",
                       
                        true);
            ScriptManager.RegisterStartupScript(
                       updatePanel2,
                       this.GetType(),
                       "MyAction",
                       "HideinsertLabel();",

                       true);
           
            if (!IsPostBack)
            {
                btnupdate.Visible = false;

                FillGrid();
            }

        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                DataUtility data = null;

                if (Page.IsValid)
                {

                    SqlParameter[] sqlParam = {
                                                 new SqlParameter("@Description",txtdescription.Text),
                                                 new SqlParameter("@Active",rblactive.SelectedValue),
                                                 new SqlParameter("@CreatedBy",Session["UserName"]),
                                                 new SqlParameter("@CreatedDate", DateTime.Now)
                                              };

                    data = new DataUtility();

                    data.ExecuteProc("insertupdateSolutionMaster", sqlParam);

                    ClearData();

                    FillGrid();

                    r_Error1.Visible = true;
                  
                    lblinsert.Text = "New Solution Added";
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideinsertLabel();", true);
                  
                }

            }
            catch
            {
                throw;
            }

           
        }

        protected void btnupdate_Click(object sender, EventArgs e)
        {

            
            try
            {
                 DataUtility data = null;

                if (Page.IsValid)
                {

                    SqlParameter[] sqlParam = {
                                    new SqlParameter("@solution_code",txtsolutioncode.Text),
                                    new SqlParameter("@Description",txtdescription.Text),
                                    new SqlParameter("@Active",rblactive.SelectedValue),
                                    new SqlParameter("@UpdatedBy",Session["UserName"]),
                                    new SqlParameter("@UpdatedDate", DateTime.Now)
                                };

                    data = new DataUtility();

                    data.ExecuteProc("insertupdateSolutionMaster", sqlParam);

                    btnupdate.Visible = false;

                    btnsave.Visible = true;

                    ClearData();


                    FillGrid();
                    r_Error1.Visible = true;
                   
                    lblupdate.Text = "Solution Updated";
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideupdateLabel();", true);
                  
                }
                else
                {
                    btnupdate.Visible = true;

                    btnsave.Visible = false;

                }
            }

            catch 
            {
                throw;
            }

            
        }

        protected void lnkbtn_Click(object sender, EventArgs e)
        {

            String SolutionCode = ((sender as LinkButton).CommandArgument);
            DataSet ds = null;
            try
            {

                SqlParameter[] sqlParam = {
                                    new SqlParameter("@solution_code",SolutionCode)
                                              };
                ds = new DataSet();
                ds = IMS_Net.Admin.Utility3.Database.GetData(strConnections, "GetDataById", sqlParam);

                txtsolutioncode.Text = ds.Tables[0].Rows[0]["solution_code"].ToString();
                txtdescription.Text = ds.Tables[0].Rows[0]["Description"].ToString();
                rblactive.SelectedValue = ds.Tables[0].Rows[0]["Active"].ToString();


                btnupdate.Visible = true;
                btnsave.Visible = false;

                r_Error1.Visible = false;

            }
            catch
            {
                throw;
            }
            finally
            {
                ds.Dispose();
            }

        }

        protected void GridPageChange(object sender, GridViewPageEventArgs e)
        {
            GridViewDisplay.PageIndex = e.NewPageIndex;
            this.FillGrid();
        }


        #endregion

        #region CustomMethod

        private void FillGrid()
        {
            DataSet ds = null;

            try
            {
                 

                SqlParameter[] sqlParam = {
                                    new SqlParameter("@solution_code",DBNull.Value)
                                              };
           
                ds = new DataSet();
                ds = IMS_Net.Admin.Utility3.Database.GetData(strConnections, "GetDataById", sqlParam);

                GridViewDisplay.DataSource = ds.Tables[0];
                GridViewDisplay.DataBind();
            }
            catch
            {
                throw;
            }
            finally
            {
                ds.Dispose();
            }
        }

        public void ClearData()
        {

            txtdescription.Text = "";

            rblactive.SelectedValue = "True";

            lblinsert.Text = "";
            lblupdate.Text = "";

            r_Error1.Visible = false;


        }
        
        #endregion

     }

}
