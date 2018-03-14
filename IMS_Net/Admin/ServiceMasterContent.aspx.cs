using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMS_Net.Admin
{
    public partial class ServiceMaster : System.Web.UI.Page
    {
        string strConnections = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;

        #region event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnUpdate.Visible=r_Error1.Visible = false;
                FillGridView();
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
                string strmsg;
                DataUtility data = null;
                if (Page.IsValid)
                {
                    btnSave.Visible = true;
                    SqlParameter[] param ={
                                            new SqlParameter("@ServiceLineCode",txtServicelineCode.Text.Trim()),
                                            new SqlParameter("@Description", txtDescription.Text.Trim()),       
                                            new SqlParameter("@Active", rblActive.SelectedValue),
                                            new SqlParameter("@IMS", "1"),
                                            new SqlParameter("@RFC", "0"),
                                            new SqlParameter("@CreatedBy", Session["username"]),
                                            new SqlParameter("@CreatedDate", DateTime.Now.ToString()),
                                            new SqlParameter("@Flag", 1)
                                            //new SqlParameter("@msg",SqlDbType.VarChar,100)
                                           };
                      data = new DataUtility();

                    strmsg=data.ExecuteStoredProc("ServiceMasterInsert", param,1);
                    
                    Clear();
                    
                    r_Error1.Visible = true;
                    if (strmsg == "1")
                    {
                        lblErrorMsg.Text = "Service Line Already Exists.......";
                    }
                    else
                    {
                        lblSuccessMsg.Text = "New Service Line Added.......";
                    }
                    FillGridView();
                }

                else
                {
                    r_Error1.Visible = true;
                    lblErrorMsg.Text = "Service Line Error Occured.......";
                }
            }
            catch { throw; }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string strmsg;
                DataUtility data = null;

                if (Page.IsValid)
                {
                    btnSave.Visible = false;
                    //txtServicelineCode.ReadOnly = true;
                   string servicelinecode = Convert.ToString((sender as Button).CommandArgument);

                    SqlParameter[] param ={
                                            new SqlParameter("@ServiceLineCode",servicelinecode),
                                            new SqlParameter("@Description", txtDescription.Text.Trim()),
                                            new SqlParameter("@Active", rblActive.SelectedValue),
                                            new SqlParameter("@IMS", "1"),
                                            new SqlParameter("@RFC", "0"),
                                            new SqlParameter("@UpdatedDate", DateTime.Now.ToString()),
                                            new SqlParameter("@UpdatedBy", Session["username"]),
                                            new SqlParameter("@Flag", 2),
                                  //          new SqlParameter("@msg",DBNull.Value)
                                          };
                    data = new DataUtility();

                    strmsg = data.ExecuteStoredProc("ServiceMasterInsert", param,1);

                    Clear();


                    r_Error1.Visible = true;
                    lblSuccessMsg.Text = "Updated Successfully.......";

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
                btnSave.Visible= r_Error1.Visible = false;
                lblSuccessMsg.Text = lblErrorMsg.Text = "";
                btnUpdate.Visible= txtServicelineCode.ReadOnly  = true;

                string servicelinecode = Convert.ToString((sender as LinkButton).CommandArgument);

                SqlParameter[] param ={
                                            new SqlParameter("@ServiceLineCode", servicelinecode)
                                      };
                ds = new DataSet();
                ds = IMS_Net.Admin.Utility3.Database.GetData(strConnections, "ServiceMasterGridView", param);
                

                btnUpdate.CommandArgument = ds.Tables[0].Rows[0]["Service_Line_Code"].ToString();
                txtServicelineCode.Text= ds.Tables[0].Rows[0]["Service_Line_Code"].ToString();
                txtDescription.Text = ds.Tables[0].Rows[0]["Description"].ToString();
                rblActive.SelectedValue = ds.Tables[0].Rows[0]["Active"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally
            { ds.Dispose(); }
        }

        #endregion

        #region custom method

        public void Clear()
        {
            txtServicelineCode.Text=txtDescription.Text = lblErrorMsg.Text = lblSuccessMsg.Text = "";
            rblActive.SelectedValue = "True";
            btnSave.Visible = true;
            btnUpdate.Visible = r_Error1.Visible = false;
        }

        public void FillGridView()
        {
            DataSet ds = null;
            try
            {
                ds = new DataSet();

                SqlParameter[] param ={
                                            new SqlParameter("@ServiceLineCode", DBNull.Value)
                                      };
                ds = IMS_Net.Admin.Utility3.Database.GetData(strConnections, "ServiceMasterGridView", param);

                gvServiceMaster.DataSource = ds.Tables[0];
                gvServiceMaster.DataBind();
            }
            catch { throw; }
            finally
            { ds.Dispose(); }
        }

        public void ServiceMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvServiceMaster.PageIndex = e.NewPageIndex;
            this.FillGridView();
        }
        #endregion
    }
}