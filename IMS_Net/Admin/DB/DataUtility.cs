using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net.Mail;

namespace IMS_Net.Admin.DB
{
    public class DataUtility
    {
        //define all global variables.
        SqlCommand mDataCom;
        SqlConnection mCon;
        SqlDataAdapter mDa;

        //this function will open connecton with databases.
        private void OpenConnection()
        {
            //initialize the connection object with parameterized constructer.
            if (mCon == null)
            {
                mCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStr"].ToString());
            }

            //open the connection prior to check the connection state.
            if (mCon.State == ConnectionState.Closed)
            {
                mCon.Open();
            }
            //initialize the command object with no argument constructor.
            mDataCom = new SqlCommand();

            //set the connection to command object.
            mDataCom.Connection = mCon;
        }

        //this functionality will close the connection with database.
        private void CloseConnection()
        {
            if (mCon.State == ConnectionState.Open)
            {
                mCon.Close();
            }
        }

        //this functionality will dispose connection;
        private void DisposeConnection()
        {
            if (mCon != null)
            {
                mCon.Dispose();
                mCon = null;
            }
        }

        //this functionality will execute all DML procedures.
        public string ExecuteStoredProc(string strSPName, SqlParameter[] arProcParams, Int16 Flag)
        {
            OpenConnection();
            int intReturnValue = 0;
            string strMsg = "";

            SqlParameter param = new SqlParameter();

            //set the properties of command object.
            mDataCom.CommandType = CommandType.StoredProcedure;
            mDataCom.CommandText = strSPName;
            // mDataCom.Parameters.Add(param);

            //get the SQL parameters from the array.
            if (mDataCom.Parameters.Count > 0)
            {
                mDataCom.Parameters.Clear();
            }
            else
            {

                foreach (SqlParameter LoopVar_param in arProcParams)
                {
                    param = LoopVar_param;
                    mDataCom.Parameters.Add(param);
                }

                if (Flag == 1)
                {

                    mDataCom.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                    mDataCom.Parameters["@msg"].Direction = ParameterDirection.Output;

                }
                if (Flag == 3)
                {

                    mDataCom.Parameters.Add("@GenCode", SqlDbType.VarChar, 20);
                    mDataCom.Parameters["@GenCode"].Direction = ParameterDirection.Output;

                }
                if (Flag == 2)
                {

                    mDataCom.Parameters.Add("@Approval_SLH_id", SqlDbType.BigInt);
                    mDataCom.Parameters["@Approval_SLH_id"].Direction = ParameterDirection.Output;

                }
                if (Flag == 4)
                {

                    mDataCom.Parameters.Add("@RFC_Comleted_ID_out", SqlDbType.BigInt);
                    mDataCom.Parameters["@RFC_Comleted_ID_out"].Direction = ParameterDirection.Output;

                }

            }
            intReturnValue = mDataCom.ExecuteNonQuery();

            if (Flag == 1)
            {
                strMsg = mDataCom.Parameters["@msg"].Value.ToString();
            }
            if (Flag == 2)
            {
                strMsg = mDataCom.Parameters["@Approval_SLH_id"].Value.ToString();
            }
            if (Flag == 3)
            {
                strMsg = mDataCom.Parameters["@GenCode"].Value.ToString();
            }
            if (Flag == 4)
            {
                strMsg = mDataCom.Parameters["@RFC_Comleted_ID_out"].Value.ToString();
            }

            CloseConnection();
            DisposeConnection();

            return strMsg;
        }

        public bool ExecuteProc(string strSPName, SqlParameter[] arProcParams)
        {
            OpenConnection();
            int intReturnValue = 0;

            SqlParameter param = new SqlParameter();

            //set the properties of command object.
            mDataCom.CommandType = CommandType.StoredProcedure;
            mDataCom.CommandText = strSPName;

            //get the SQL parameters from the array.
            if (mDataCom.Parameters.Count > 0)
            {
                mDataCom.Parameters.Clear();
            }
            else
            {
                foreach (SqlParameter LoopVar_param in arProcParams)
                {
                    param = LoopVar_param;
                    mDataCom.Parameters.Add(param);
                }
            }
            intReturnValue = mDataCom.ExecuteNonQuery();
            CloseConnection();
            DisposeConnection();
            if (intReturnValue.Equals(0))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        //this functionality will execute all stored procedures for DQL statements.


        public DataSet GetDataSetProc(string strSPName, SqlParameter[] arProcParams)
        {
            OpenConnection();

            DataSet ds = new DataSet();

            mDa = new SqlDataAdapter();
            SqlParameter param = new SqlParameter();

            mDataCom.CommandType = CommandType.StoredProcedure;
            mDataCom.CommandText = strSPName;

            if (mDataCom.Parameters.Count > 0)
            {
                mDataCom.Parameters.Clear();
            }
            mDa.SelectCommand = mDataCom;

            foreach (SqlParameter LoopVar_param in arProcParams)
            {
                param = LoopVar_param;
                mDataCom.Parameters.Add(param);
            }

            mDa.Fill(ds);

            CloseConnection();
            DisposeConnection();
            return ds;
        }
        public DataSet GetDataSetProc(string strSPName)
        {
            OpenConnection();
            DataSet ds = new DataSet();
            mDa = new SqlDataAdapter();
            mDataCom.CommandType = CommandType.StoredProcedure;
            mDataCom.CommandText = strSPName;
            mDa.SelectCommand = mDataCom;
            mDa.Fill(ds);
            CloseConnection();
            DisposeConnection();
            return ds;
        }


        //this functionality will check whether particular item exists in databse or not.
        public bool ISExist(string strSPName)
        {
            OpenConnection();

            mDataCom.CommandType = CommandType.StoredProcedure;
            mDataCom.CommandText = strSPName;
            mDataCom.CommandTimeout = 2000;

            int intResult = (int)mDataCom.ExecuteScalar();
            CloseConnection();
            DisposeConnection();

            if (intResult > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //in this functionality i am trying to give vertical view to a datagrid.





        public static DataTable ExecuteProcedure1(string ProcedureName, string strconstring, string serviceline_code, int servinity, float hour_from, float hour_to, float S0_4, float S4_8, float S8_16, float s16_9999, string SLA_desc)
        {
            SqlConnection objcon = new SqlConnection(strconstring);
            SqlCommand cmd = new SqlCommand(ProcedureName, objcon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Service_line_code", SqlDbType.VarChar, 20);
            cmd.Parameters["@Service_line_code"].Value = serviceline_code;
            cmd.Parameters.Add("@serverity_code", SqlDbType.Int);
            cmd.Parameters["@serverity_code"].Value = servinity;
            cmd.Parameters.Add("@Hour_from", SqlDbType.Float);
            cmd.Parameters["@Hour_from"].Value = hour_from;
            cmd.Parameters.Add("@Hour_to", SqlDbType.Float);
            cmd.Parameters["@Hour_to"].Value = hour_to;
            cmd.Parameters.Add("@S0_4", SqlDbType.Float);
            cmd.Parameters["@S0_4"].Value = S0_4;
            cmd.Parameters.Add("@S4_8", SqlDbType.Float);
            cmd.Parameters["@S4_8"].Value = S4_8;
            cmd.Parameters.Add("@S8_16", SqlDbType.Float);
            cmd.Parameters["@S8_16"].Value = S8_16;
            cmd.Parameters.Add("@s16_9999", SqlDbType.Float);
            cmd.Parameters["@s16_9999"].Value = s16_9999;
            cmd.Parameters.Add("@sla_description", SqlDbType.VarChar, 150);
            cmd.Parameters["@sla_description"].Value = SLA_desc;
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adp.Fill(dt);//, strtable);
            adp.Dispose();
            objcon.Close();
            objcon.Dispose();//Dispose the Connection Object
            return dt;

            //catch (Exception ex)
            //{
            //    //string setmsg = ex.ToString();
            //    //return false; 

            //}


        }

        public DataSet Get_Dataset(string sqlQuery)
        {
            string strconString = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
            SqlConnection objcon;
            SqlCommand objcmd;
            SqlDataAdapter objadp;
            DataSet ds;
            objcon = new SqlConnection(strconString);

            try
            {
                objcmd = new SqlCommand(sqlQuery, objcon);
                objcmd.CommandType = CommandType.Text;

                try
                {
                    objcon.Open();
                }
                catch (SqlException ex)
                {
                    throw ex;
                }

                objcmd.ExecuteNonQuery();
                ds = new DataSet();
                objadp = new SqlDataAdapter(objcmd);
                objadp.Fill(ds);
                //objcon.Close();
                objcmd.Dispose();
                objcon.Dispose();
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objcon.State == ConnectionState.Open)
                {
                    objcon.Close();
                }
            }
        }

        public void fillListQuery(DropDownList lst, string strQuery, string strvmember, string strdmember)
        {
            string strconstring = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
            int i;
            SqlConnection objcon;
            SqlCommand objcom;
            DataSet objds = new DataSet();
            SqlDataAdapter objda;

            objcon = new SqlConnection(strconstring);
            //  objcon.Open();

            objcom = new SqlCommand(strQuery, objcon);
            objda = new SqlDataAdapter(strQuery, objcon);
            objda.Fill(objds, "MyTable");
            i = objds.Tables["MyTable"].Rows.Count;
            if (i > 0)
            {
                lst.DataSource = objds.Tables["MyTable"].DefaultView;
                lst.DataTextField = strdmember;
                lst.DataValueField = strvmember;
                lst.DataBind();
                //lst.Items.Insert(0, new ListItem("Please select role", "0"));
                //lst.SelectedIndex = 0;
            }
            else
            {
                //lst.Items.Insert(0, new ListItem("Please select role", "0"));
            }
            objds.Dispose();
            objda.Dispose();
            objcom.Dispose();
            objcon.Close();
            objcon.Dispose();
        }

        //mail Sending provision
        public void sendmail(int Emailflag, string RFCNO, string ClientName, string Priority, string ServiceLine, string ServiceLine_Module, string Application_Incharge, string RFC_Owned_By, string RFC_Summary, string EngineerEmail, string ServiceLineHeadEmail, string UnitLeadEmail, string ChangeManagerEmail, string SLHName, string ULName, string CMName, string EngName, string ClientEmailId, bool? approveStatus)
        {
            try
            {
                //string from = "anil.shah@dabur.com";
                string from = "rfcsupport@dabur.com";
                string to = "";
                string subject = "";
                string body = "";
                string cc = "";
                string cc1 = "";
                string cc2 = "";
                //string RFCNO;


                if (Emailflag == 1)
                {
                    subject = "New RFC is Created by Engineer " + EngName + " whose no is RFC: " + RFCNO + " kindly see the mail to view details";
                }
                else if (Emailflag == 2)
                {
                    subject = "New RFC is Created by Service Line Head (SLH) " + SLHName + " whose no is RFC: " + RFCNO + " kindly see the mail to view details";
                }
                else if (Emailflag == 11)
                {
                    subject = "New RFC is Created by Service Line Head (SLH) " + SLHName + " whose no is RFC: " + RFCNO + " kindly see the mail to view details";
                }
                else if (Emailflag == 3 || Emailflag == 4)
                {
                    subject = "RFC is Approved by Service Line Head (SLH) " + SLHName + " whose no is RFC: " + RFCNO + " kindly see the mail to view details";
                }
                else if (Emailflag == 5 || Emailflag == 6)
                {
                    subject = "RFC is " + (approveStatus.HasValue ? (approveStatus.Value ? "Approved " : "Dis-Approved ") : " ") + " by Unit Lead (UL) " + ULName + " whose no is RFC: " + RFCNO + " kindly see the mail to view details";
                }
                else if (Emailflag == 7 || Emailflag == 8)
                {
                    subject = "RFC is " + (approveStatus.HasValue ? (approveStatus.Value ? "Approved " : "Dis-Approved ") : " ") + " by Change Manager (CM) " + CMName + " whose no is RFC: " + RFCNO + " kindly see the mail to view details";
                }
                else if (Emailflag == 9 || Emailflag == 10)
                {
                    subject = "RFC is Completed by Service Line Head (SLH) " + SLHName + " whose no is RFC: " + RFCNO + " kindly see the mail to view details";
                }
                //implemented on 27th May 2010
                else if (Emailflag == 25)
                {
                    subject = "RFC is Dis-Approved  by Service Line Head (SLH) " + SLHName + " whose no is RFC: " + RFCNO + " kindly see the mail to view details";
                }

                body = "<font color='#330066' size='3' family='Verdana, Arial, Helvetica, sans-serif' weight='bold' background-color='#6633cc' ><br><b>******************RFC DETAILS****************</b><br>";
                body = body + "<p></p><br><b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;RFC No                    : </b>" + RFCNO + "<br>";
                body = body + "<p></p><br><b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Client Name               : </b>" + ClientName + "<br>";
                body = body + "<p></p><br><b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Log Date                  : </b>" + DateTime.Now.ToString();
                body = body + "<p></p><br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Priority                  : </b>" + Priority;
                body = body + "<p></p><br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Technology                : </b>" + ServiceLine;
                body = body + "<p></p><br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Application Name          : </b>" + ServiceLine_Module;
                body = body + "<p></p><br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Application Incharge      : </b>" + Application_Incharge;
                body = body + "<p></p><br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>RFC Owner                 : </b>" + RFC_Owned_By;
                body = body + "<p></p><br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>RFC Summary               : </b>" + RFC_Summary;
                body = body + "<p></p><br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b><font color='red'>NOTE: </b> In order to Login to the application kindy use the Links: " + " Intranet Link:<a href=http://daburweb/RFC/pageauth/login.aspx > http://daburweb/RFC/pageauth/login.aspx </a><br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Internet Link: <a href=http://125.22.2.163/RFC/pageauth/login.aspx > http://125.22.2.163/RFC/pageauth/login.aspx </a> <br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Userid and password are LDAP authenticated.</font>";

                // When Engineer Logged a call, it will go to Engineer and Service Line Head
                if (Emailflag == 1)
                {
                    to = ServiceLineHeadEmail;
                    cc = EngineerEmail;
                    if (ClientEmailId.Trim() != "")
                    {
                        cc1 = ClientEmailId.Trim();



                    }
                }
                // When SLH Logged a call and assigned to Engineer, it will go to Engineer and Service Line Head
                if (Emailflag == 11)
                {
                    to = ServiceLineHeadEmail;
                    cc = EngineerEmail;
                    if (ClientEmailId.Trim() != "")
                    {
                        cc1 = ClientEmailId.Trim();
                    }
                }
                // When Service Line Head Logged a call, it will go to Service Line Head
                else if (Emailflag == 2)
                {
                    to = ServiceLineHeadEmail;
                    cc = ServiceLineHeadEmail;
                    cc2 = EngineerEmail;
                    if (ClientEmailId.Trim() != "")
                    {
                        cc1 = ClientEmailId.Trim();



                    }
                }
                // When Service Line Head Approves a call Logged by engineer, it will go to UnitLead, Engineer and Service Line Head
                else if (Emailflag == 3)
                {
                    to = UnitLeadEmail;
                    cc = EngineerEmail;
                    cc1 = ServiceLineHeadEmail;
                }
                // When ServiceLine Head Approves a call Logged by Service Line Head, it will go to UnitLead, Service Line Head
                else if (Emailflag == 4)
                {
                    to = UnitLeadEmail;
                    cc2 = EngineerEmail;
                    cc = ServiceLineHeadEmail;
                }
                // When Unit Head Approves a call Logged by Service Line Head, it will go to UnitLead, Service Line Head
                else if (Emailflag == 5)
                {
                    to = ChangeManagerEmail;
                    cc = UnitLeadEmail;
                    cc2 = EngineerEmail;
                    cc1 = ServiceLineHeadEmail;
                }
                // When Unit Head Approves a call Logged by Engineer, it will go to UnitLead, Service Line Head
                else if (Emailflag == 6)
                {
                    to = ChangeManagerEmail;
                    cc = UnitLeadEmail;
                    cc1 = ServiceLineHeadEmail;
                    cc2 = EngineerEmail;
                }
                // When Change Manager Approves a call Logged by Engineer, it will go to UnitLead, Service Line Head
                else if (Emailflag == 7)
                {
                    to = ServiceLineHeadEmail;
                    //cc = ChangeManagerEmail;
                    cc1 = UnitLeadEmail;
                    cc2 = EngineerEmail;
                }
                // When Change Manager Approves a call Logged by Service Line Head, it will go to UnitLead, Service Line Head
                else if (Emailflag == 8)
                {
                    to = ServiceLineHeadEmail;
                    //cc = ChangeManagerEmail;
                    cc2 = EngineerEmail;
                    cc1 = UnitLeadEmail;
                }
                // When Service Line Head Completed a call, it will go to Engineer and Service Line Head
                if (Emailflag == 9)
                {
                    to = ServiceLineHeadEmail;
                    cc = EngineerEmail;
                }
                // When Service Line Head COmpleted a call, it will go to Service Line Head
                else if (Emailflag == 10)
                {
                    to = ServiceLineHeadEmail;
                    cc = ServiceLineHeadEmail;
                    cc2 = EngineerEmail;
                }
                //implemented on 27th May 2010
                // When Service Line Head rejects a call, it will go to Engineer and Service Line Head
                if (Emailflag == 25)
                {
                    to = EngineerEmail;
                    //cc = ServiceLineHeadEmail;
                    cc = ServiceLineHeadEmail;
                }

                MailMessage message = new MailMessage(from, to, subject, body);
                SmtpClient emailClient = new SmtpClient("172.26.4.28");
                if (!string.IsNullOrEmpty(cc))
                    message.CC.Add(cc);
                if (cc1 != "")
                {
                    message.CC.Add(cc1);
                }
                if (cc2 != "")
                {
                    message.CC.Add(cc2);
                }
                message.IsBodyHtml = true;
                emailClient.Send(message);

            }
            catch (Exception ex)
            {
                //litStatus.Text = ex.ToString();
                throw new Exception("Please contact to Administrator.");
            }
        }

        //mail Sending provision
        public void Transport_Request_sendmail(int Emailflag, string TransportNo, string ServiceLine, string ServiceLine_Module, string Application_Incharge, string TP_Owned_By, string TP_Summary, string EngineerEmail, string ServiceLineHeadEmail, string UnitLeadEmail, string SLHName, string ULName, string EngName, bool? approveStatus)
        {
            try
            {
                //string from = "anil.shah@dabur.com";
                string from = "rfcsupport@dabur.com";
                string to = "";
                string subject = "";
                string body = "";
                string cc = "";
                string cc1 = "";
                string cc2 = "";
                //string RFCNO;


                if (Emailflag == 1)
                {
                    subject = "New Transport Request is Created by Engineer " + EngName + " whose no is Trasport No: " + TransportNo + " kindly see the mail to view details";
                }
                else if (Emailflag == 2)
                {
                    subject = "New Transport Request is Created by Service Line Head (SLH) " + SLHName + " whose no is Trasport No: " + TransportNo + " kindly see the mail to view details";
                }
                else if (Emailflag == 11)
                {
                    subject = "New Transport Request is Created by Service Line Head (SLH) " + SLHName + " whose no is Trasport No: " + TransportNo + " kindly see the mail to view details";
                }
                else if (Emailflag == 3 || Emailflag == 4)
                {

                    subject = "Transport Request is " + (approveStatus.HasValue ? (approveStatus.Value ? "Approved " : "Dis-Approved ") : " ") + "by Service Line Head (SLH) " + SLHName + " whose no is Trasport No: " + TransportNo + " kindly see the mail to view details";
                }
                body = "<font color='#330066' size='3' family='Verdana, Arial, Helvetica, sans-serif' weight='bold' background-color='#6633cc' ><br><b>******************RFC DETAILS****************</b><br>";
                body = body + "<p></p><br><b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Transport No                    : </b>" + TransportNo + "<br>";
                //body = body + "<p></p><br><b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Client Name               : </b>" + ClientName + "<br>";
                body = body + "<p></p><br><b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Log Date                  : </b>" + DateTime.Now.ToString();
                // body = body + "<p></p><br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Priority                  : </b>" + Priority;
                body = body + "<p></p><br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Technology                : </b>" + ServiceLine;
                body = body + "<p></p><br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Application Name          : </b>" + ServiceLine_Module;
                body = body + "<p></p><br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Application Incharge      : </b>" + Application_Incharge;
                body = body + "<p></p><br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Transport Request Owner   : </b>" + TP_Owned_By;
                body = body + "<p></p><br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Transport Request Summary : </b>" + TP_Summary;

                // When Engineer Logged a call, it will go to Engineer and Service Line Head
                if (Emailflag == 1)
                {
                    to = ServiceLineHeadEmail;
                    cc = EngineerEmail;
                    // if (ClientEmailId.Trim() != "")
                    // {
                    //     cc1 = ClientEmailId.Trim();



                    // }
                }
                // When SLH Logged a call and assigned to Engineer, it will go to Engineer and Service Line Head
                if (Emailflag == 11)
                {
                    to = ServiceLineHeadEmail;
                    cc = EngineerEmail;
                    //  if (ClientEmailId.Trim() != "")
                    //  {
                    //      cc1 = ClientEmailId.Trim();



                    //  }
                }
                // When Service Line Head Logged a call, it will go to Service Line Head
                else if (Emailflag == 2)
                {
                    to = ServiceLineHeadEmail;
                    cc = ServiceLineHeadEmail;
                    // if (ClientEmailId.Trim() != "")
                    // {
                    //     cc1 = ClientEmailId.Trim();



                    // }
                }
                // When Service Line Head Approves a call Logged by engineer, it will go to UnitLead, Engineer and Service Line Head
                else if (Emailflag == 3)
                {
                    to = EngineerEmail;
                    //cc = ServiceLineHeadEmail;
                }
                // When ServiceLine Head Approves a call Logged by Service Line Head, it will go to UnitLead, Service Line Head
                else if (Emailflag == 4)

                    to = ServiceLineHeadEmail;

                MailMessage message = new MailMessage(from, to, subject, body);
                SmtpClient emailClient = new SmtpClient("172.26.4.28");
                if (!string.IsNullOrEmpty(cc))
                    message.CC.Add(cc);
                if (cc1 != "")
                {
                    message.CC.Add(cc1);
                }
                if (cc2 != "")
                {
                    message.CC.Add(cc2);
                }
                message.IsBodyHtml = true;
                emailClient.Send(message);

            }
            catch (Exception ex)
            {
                //litStatus.Text = ex.ToString();
                throw new Exception("Please contact to Administrator.");
            }
        }





    }
}