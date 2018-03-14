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
    public partial class ComplaintMapping : System.Web.UI.Page
    {

        String strConnections = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;

        #region events

        protected void Page_Load(object sender, EventArgs e)
        {
            lblinsert.Text = "";
            lblupdate.Text = "";
            r_Error1.Visible = false;
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
                dropdownlist();

                btnupdate.Visible = false;

                FillGrid();
                btnClickpopup();

                btnpopup_Clear();
            }
        }

        /*   protected void btnOk_Click(object sender, EventArgs e)
           {
              var items = new ListItemCollection(); 

               foreach (ListItem item in CheckBoxList1.Items)
               {
                   if (item.Selected)
                       items.Add(item);
                    
               }
            
               string YrStr = "";
               DataUtility data = null;
               DataSet ds = null;
               for (int i = 0; i < CheckBoxList1.Items.Count; i++)
               {
                    if (CheckBoxList1.Items[i].Selected)
                    {
                     YrStr = CheckBoxList1.Items[i].Text;

                     SqlParameter[] sqlParam = {
                                                    new SqlParameter("@Company_Id",1000),
                                                    new SqlParameter("@Service_Line_Code",ddlservicelinecode.SelectedValue),
                                                    new SqlParameter("@Module_Code",ddlmodulecode.SelectedValue),
                                                
                                                    new SqlParameter("@complaint_code",YrStr),
                                                    new SqlParameter("@CreatedBy",Session["UserName"]),
                                                    new SqlParameter("@CreatedDate", DateTime.Now)
                                                 };

                     data = new DataUtility();

                     data.ExecuteProc("insertComplaintMapping", sqlParam);
                     FillGrid();

                     CheckBoxList1.DataSource = ds;
                     CheckBoxList1.DataTextField = "Description";
                     CheckBoxList1.DataValueField = "Complaint_Code";
                     CheckBoxList1.DataBind();
                
                   }

                
               }
          // txtcomplaintcode.Text=YrStr;
           
           }*/

        protected void ddlservicelinecode_SelectedIndexChanged(object sender, EventArgs e)
        {
            binddropdowmlist();
        }

        /*    protected void btnsave_Click(object sender, EventArgs e)
            {
                try
                {
                    DataUtility data = null;
                    string YrStr = "";

                    if (Page.IsValid)
                    {

                        for (int i = 0; i < CheckBoxList1.Items.Count; i++)
                        {
                            if (CheckBoxList1.Items[i].Selected)
                            {
                                YrStr = CheckBoxList1.Items[i].Text;

                                SqlParameter[] sqlParam = {
                                                     new SqlParameter("@Company_Id",1000),
                                                     new SqlParameter("@Service_Line_Code",ddlservicelinecode.SelectedValue),
                                                     new SqlParameter("@Module_Code",ddlmodulecode.SelectedValue),
                                                
                                                     new SqlParameter("@complaint_code",YrStr),
                                                     new SqlParameter("@CreatedBy",Session["UserName"]),
                                                     new SqlParameter("@CreatedDate", DateTime.Now)
                                                  };


                                data = new DataUtility();

                                data.ExecuteProc("insertComplaintMapping", sqlParam);

                           
                            }

                        }
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
            

            }*/

        protected void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                DataUtility data = null;
                if (Page.IsValid)
                {


                    foreach (GridViewRow gvrow in popupgv.Rows)
                    {
                        if (gvrow.RowType == DataControlRowType.DataRow)
                        {
                            //var checkbox = gvrow.FindControl("chkpopup") as CheckBox;
                            CheckBox chkRow = (gvrow.Cells[0].FindControl("chkpopup") as CheckBox);

                            if (chkRow.Checked)
                            {
                                string code = gvrow.Cells[2].Text;

                                SqlParameter[] sqlParam = {
                                                 new SqlParameter("@Company_Id",1000),
                                                 new SqlParameter("@Service_Line_Code",ddlservicelinecode.SelectedValue),
                                                 new SqlParameter("@Module_Code",ddlmodulecode.SelectedValue),
                                                
                                                 new SqlParameter("@complaint_code",code),
                                                 new SqlParameter("@CreatedBy",Session["UserName"]),
                                                 new SqlParameter("@CreatedDate", DateTime.Now),
                                                 new SqlParameter("@Flag", 1)
                                           
                                              };


                                data = new DataUtility();

                                data.ExecuteProc("insertComplaintMapping", sqlParam);



                            }
                        }

                    }
                    ClearData();
                    FillGrid();
                    btnpopup_Clear();
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
                    foreach (GridViewRow gvrow in popupgv.Rows)
                    {
                        if (gvrow.RowType == DataControlRowType.DataRow)
                        {
                            //var checkbox = gvrow.FindControl("chkpopup") as CheckBox;
                            CheckBox chkRow = (gvrow.Cells[0].FindControl("chkpopup") as CheckBox);

                            if (chkRow.Checked)
                            {
                                string code = gvrow.Cells[2].Text;


                                SqlParameter[] sqlParam = {
                                     new SqlParameter("@hfservicelinecode",hfservicelinecode.Value),
                                     new SqlParameter("@hfmodulecode",hfmodulecode.Value),
                                     new SqlParameter("@hfcomplaintcode",hfcomplaintcode.Value),
                                    new SqlParameter("@Service_Line_Code",ddlservicelinecode.SelectedValue),
                                    new SqlParameter("@Module_Code",ddlmodulecode.SelectedValue),

                                    new SqlParameter("@complaint_code",code),
                                    
                                   // new SqlParameter("@complaint_code",CheckBoxList1.SelectedValue),
                                    new SqlParameter("@UpdatedBy",Session["UserName"]),
                                    new SqlParameter("@UpdatedDate", DateTime.Now),
                                    new SqlParameter("@Flag", 2),
                                  
                                };

                                data = new DataUtility();

                                data.ExecuteProc("insertComplaintMapping", sqlParam);

                            }
                        }
                    }

                    btnupdate.Visible = false;

                    btnsave.Visible = true;

                    ClearData();
                    btnpopup_Clear();

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

        protected void btnclear_Click(object sender, EventArgs e)
        {
            ClearData();
            btnpopup_Clear();

            btnupdate.Visible = false;

            btnsave.Visible = true;

        }

        protected void lnkbtn_Click(object sender, EventArgs e)
        {
            string[] commandArgs = (sender as LinkButton).CommandArgument.ToString().Split(new char[] { ';' });
            string ServiceLineCode = commandArgs[0];
            string ModuleCode = commandArgs[1];
            string complaintcode = commandArgs[2];

            DataSet ds = null;
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@Service_Line_Code",ServiceLineCode),
                                    new SqlParameter("@Module_Code",ModuleCode),
                                    new SqlParameter("@complaint_code",complaintcode)
                                   
                                          };

                ds = new DataSet();
                ds = IMS_Net.Admin.Utility3.Database.GetData(strConnections, "GetDataComplaintMapping", sqlParam);

                ddlservicelinecode.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["Service_Line_Code"]);
                binddropdowmlist();
                ddlmodulecode.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["Module_Code"]);
                hfservicelinecode.Value = Convert.ToString(ds.Tables[0].Rows[0]["Service_Line_Code"]);
                hfmodulecode.Value = Convert.ToString(ds.Tables[0].Rows[0]["Module_Code"]);
                hfcomplaintcode.Value = Convert.ToString(ds.Tables[0].Rows[0]["complaint_code"]);
                ChkMatch();


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
            gvcomplaintmapping.PageIndex = e.NewPageIndex;
            this.FillGrid();

        }

        protected void search_textChanged(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(strConnections);
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("SELECT Description,Complaint_Code from COMPLAINT_MASTER where Active='1' AND Description like '" + txtsearch.Text + "%'", con);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            popupgv.DataSource = ds;
            popupgv.DataBind();
            con.Close();
            ModalPopupExtender1.Show();
        }

        protected void btnpopup_Clear()
        {
            foreach (GridViewRow gvrow in popupgv.Rows)
            {
                if (gvrow.RowType == DataControlRowType.DataRow)
                {
                    //var checkbox = gvrow.FindControl("chkpopup") as CheckBox;
                    CheckBox chkRow = (gvrow.Cells[0].FindControl("chkpopup") as CheckBox);
                    chkRow.Checked = false;
                }
            }
        }

        #endregion

        #region CustomMethods

        public void binddropdowmlist()
        {
            String Service_Line_Code = Convert.ToString(ddlservicelinecode.SelectedValue);
            DataSet ds = null;

            try
            {

                SqlParameter[] sqlParam = {
                                    
                                    new SqlParameter("@Service_Line_Code",Service_Line_Code),
                                    
                                              };


                ds = new DataSet();
                ds = IMS_Net.Admin.Utility3.Database.GetData(strConnections, "modulecodeComplaintMaster", sqlParam);




                ddlmodulecode.DataSource = ds;
                ddlmodulecode.DataTextField = "Module_Description";
                ddlmodulecode.DataValueField = "Module_Code";
                ddlmodulecode.DataBind();
                ddlmodulecode.Items.Insert(0, new ListItem("--Select--", "0"));

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

        protected void btnClickpopup()
        {

            SqlConnection con = new SqlConnection(strConnections);
            con.Open();
            string query = "SELECT Description,Complaint_Code from COMPLAINT_MASTER WHERE Active='1'";
            SqlCommand cmd = new SqlCommand(query, con);
            /*  SqlDataReader rdr = cmd.ExecuteReader();
              while (rdr.Read())
              {
                  string fil1 = rdr.GetString(0);
                  chkpopup.Items.Add(fil1);
              }

              rdr.Close();
               */

            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = con;
                sda.SelectCommand = cmd;
                using (DataTable dt = new DataTable())
                {
                    sda.Fill(dt);
                    popupgv.DataSource = dt;
                    popupgv.DataBind();
                }
            }
            //popupgv.DataSource = ds.Tables[0];
            // popupgv.DataBind();

        }

        /*  protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
          {
              foreach (GridViewRow row in popupgv.Rows)
              {
                  if (e.Row.RowType == DataControlRowType.DataRow)
                  {
                      CheckBox Cblpopup = (e.Row.FindControl("Cblpopup") as CheckBox);
                  }
              }
          }*/

        private void dropdownlist()
        {
            DataSet ds = null;

            try
            {


                ds = new DataSet();
                ds = IMS_Net.Admin.Utility3.Database.GetData(strConnections, "ServiceLineComplaintMapping", null);



                ddlservicelinecode.DataSource = ds;
                ddlservicelinecode.DataTextField = "Description";
                ddlservicelinecode.DataValueField = "Service_Line_Code";
                ddlservicelinecode.DataBind();

                ddlservicelinecode.Items.Insert(0, new ListItem("--Select--", "0"));

                ddlmodulecode.Items.Insert(0, new ListItem("--Select--", "0"));

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

        private void FillGrid()
        {
            DataSet ds = null;

            try
            {
                SqlParameter[] sqlParam = {
                                    
                                    new SqlParameter("@Service_Line_Code",DBNull.Value),
                                    new SqlParameter("@Module_Code",DBNull.Value),
                                    new SqlParameter("@complaint_code",DBNull.Value)
                                 
                                              };


                ds = new DataSet();
                ds = IMS_Net.Admin.Utility3.Database.GetData(strConnections, "GetDataComplaintMapping", sqlParam);

                gvcomplaintmapping.DataSource = ds.Tables[0];
                gvcomplaintmapping.DataBind();
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

            txtcompaintcode.Text = "";

            ddlservicelinecode.ClearSelection();
            ddlmodulecode.ClearSelection();


            lblinsert.Text = "";
            lblupdate.Text = "";

            r_Error1.Visible = false;


        }

        public void ChkMatch()
        {
            foreach (GridViewRow gvrow in popupgv.Rows)
            {
                if (gvrow.RowType == DataControlRowType.DataRow)
                {
                    string description = gvrow.Cells[2].Text;

                    if (hfcomplaintcode.Value == description)
                    {
                        //var checkbox = gvrow.FindControl("chkpopup") as CheckBox;
                        CheckBox chkRow = (gvrow.Cells[0].FindControl("chkpopup") as CheckBox);
                        chkRow.Checked = true;

                    }
                }
            }

        }

        #endregion

    }
}