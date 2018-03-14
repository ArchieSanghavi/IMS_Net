using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.IO;
using System.Globalization;
using System.Data.OleDb;

namespace IMS_Net.Admin
{
    public class Utility
    {

        public bool error_flag = false;


        public Utility()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public class classDropDownListBox
        {

            //Function to Fill the Fill Drop Down List Box. accept Query.
            public static void fillList(DropDownList lst, string strtablename, string strvmember, string strdmember, string strconstring)
            {
                int i;
                SqlConnection objcon;
                SqlCommand objcom;
                DataSet objds = new DataSet();
                SqlDataAdapter objda;

                string strquery = "select Distinct " + strvmember + "," + strdmember + " from " + strtablename + " order by " + strdmember + "";
                objcon = new SqlConnection(strconstring);
                objcon.Open();
                objcom = new SqlCommand(strquery, objcon);
                objda = new SqlDataAdapter(strquery, objcon);
                objda.Fill(objds, "MyTable");
                i = objds.Tables["MyTable"].Rows.Count;
                if (i > 0)
                {
                    lst.DataSource = objds.Tables["MyTable"].DefaultView;
                    lst.DataTextField = strdmember;
                    lst.DataValueField = strvmember;
                    lst.DataBind();

                }
                else { lst.Items.Insert(0, new ListItem("---- Select ----", "0")); }
                objds.Dispose();
                objda.Dispose();
                objcom.Dispose();
                objcon.Close();
                objcon.Dispose();
            }
            public static void fillListQuery(DropDownList lst, string strQuery, string strvmember, string strdmember, string strconstring)
            {
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
                    //lst.Items.Insert(0, new ListItem("---- Please Select ----", "0"));
                    // lst.SelectedIndex = 0;
                }
                else
                { //lst.Items.Insert(0, new ListItem("---- Please Select ----", "0"));
                }
                objds.Dispose();
                objda.Dispose();
                objcom.Dispose();
                objcon.Close();
                objcon.Dispose();
            }
            public static void fillListStatic(DropDownList lst, string strQuery, string strvmember, string strdmember, string strconstring)
            {
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
                    lst.Items.Insert(0, new ListItem("{Select]", "0"));
                    lst.SelectedIndex = 0;
                }
                else { lst.Items.Insert(0, new ListItem("---- Select ----", "0")); }
                objds.Dispose();
                objda.Dispose();
                objcom.Dispose();
                objcon.Close();
                objcon.Dispose();
            }
        }

        public class Database
        {
            //Functions to Get Users.
            private static string mailServer = ConfigurationManager.AppSettings["MailServer"];
            private static string mailServerPort = ConfigurationManager.AppSettings["MailServerPort"];
            private static string mailFromAddress = ConfigurationManager.AppSettings["MailFromAddress"];
            private static int expiryDays = Convert.ToInt32(ConfigurationManager.AppSettings["Feedback-ExpiryDays"]);
            private static string applicationURL = ConfigurationManager.AppSettings["ApplicationURL"].ToString();
            private static string sendReassignCallSMS = ConfigurationManager.AppSettings["SendReassignCallSMS"].ToString();
            private static string sendCloseCallSMS = ConfigurationManager.AppSettings["SendCloseCallSMS"].ToString();
            private static string CallReassignTemplate = "A Ticket is assigned to you:" + Environment.NewLine + "Call No:[[CALLNO]]" + Environment.NewLine + "Date and time: [[CALLLOGDATE]]" + Environment.NewLine + "Username:[[USERNAME]]";
            private static string CallCompleteTemplate = "Call No [[CALLNO]] has been closed." + Environment.NewLine + "Date and time: [[CALLLOGDATE]]" + Environment.NewLine + "Eng.Name: [[ENGINEERNAME]]";
            private static string feedbackEmailBody = "<div style=\"display: inline-block; margin: 0px; max-width: 100%; min-width: 220px; vertical-align: top; width: 100%;\" class=\"stack-column\"><table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">    <tbody>        <tr>            <td style=\"padding: 28px;\">            <table style=\"font-size: 14px; text-align: center;\" width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">                <tbody>                    <tr>                        <td style=\"padding: 20px; border: 1px solid #dadada; color: #000000; background-color: #f4f4f4;\" class=\"center-on-narrow\">                        <table style=\"width: 100%; background-color: #f4f4f4;\" cellspacing=\"0\" cellpadding=\"0\">                            <tbody>                                <tr>                                    <td style=\"padding: 0px 0px 10px; text-align: left; color: #000000; line-height: 1.3; font-size: 17px;font-family:sans-serif\">                                    <span class=\"aQJ\" data-term=\"goog_751101856\" tabindex=\"0\">									Dear <strong> [[USERNAME]] </strong>,									<br />									<br />									Thanks for using Incident Management Services.									<br />									<br />									<br />									We would appreciate if you can spend 5 minute's to answer some questions about your service experience, which will be helpful to serve you better in future.                                                                        </span>                                    </td>                                </tr>                                <tr>                                    <td style=\"padding: 0px 0px 10px; text-align: left; color: #000000; line-height: 1.3; font-size: 17px; font-weight: bold;font-size: 17px;font-family:sans-serif\">                                    Please click on the 'Take a short feedback' button below, to fill a feedback for your IMS request \"[[COMPLAINT]]\".  </td>                                </tr>								<tr>									<td style=\"padding: 0px 0px 10px; text-align: left; color: #000000; line-height: 1.3; font-size: 17px;font-size: 17px;font-family:sans-serif\">									This invitation expires on [[EXPIRAYDATE]].									<br />									<br />									<br />									<br />									Regards,									<br />									<strong>Incident Management Support and Services.</strong>									</td>								</tr>                            </tbody>                        </table>                        </td>                    </tr>                    <tr>                        <td style=\"padding-top:10px; text-align: center; font-family: sans-serif; font-size: 18px; line-height: 20px; color: #555555;\">							<table style=\"margin: auto; width: 100%;\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\">                            <tbody>                                <tr>                                    <td style=\"border-radius: 3px; text-align: center; background: #1ca353 none repeat scroll 0% 0%;\" class=\"button-td\">                                    <a href=\"[[URL]]\" style=\"border: 15px solid #1ca353; font-family: sans-serif; font-size: 18px; line-height: 1.1; text-align: center; text-decoration: none; display: block; border-radius: 3px; font-weight: bold; background: #1ca353 none repeat scroll 0% 0%;\" class=\"button-a\">                                    &nbsp;<span style=\"color: #ffffff;\">Take a short feedback</span>&nbsp;                                    </a>                                    </td>                                </tr>                            </tbody>                        </table>                        <!-- Button : END -->                        </td>                    </tr>                </tbody>            </table>            </td>        </tr>    </tbody></table></div>";
            private static string customerReminderBody = "<div style=\"display: inline-block; margin 0px; max-width 100%; min-width 220px; vertical-align top; width 100%;\" class=\"stack-column\"><table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">    <tbody>        <tr>          <td style=\"padding: 28px;\">           <table style=\"font-size: 14px; text-align center;\" width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">           <tbody>                <tr>                     <td style=\"padding: 20px; border 1px solid #dadada; color #000000; background-color #f4f4f4;\" class=\"center-on-narrow\">       <table style=\"width: 100%; background-color #f4f4f4;\" cellspacing=\"0\" cellpadding=\"0\">                  <tbody>                 <tr>                                    <td style=\"padding: 0px 0px 10px; text-align left; color #000000; line-height 1.3; font-size 17px;font-family:sans-serif\">                                    <span class=\"aQJ\" data-term=\"goog_751101856\" tabindex=\"0\">									Dear <strong> [[USERNAME]] </strong>,									<br />									<br />	Reference: <b> [[CALLNO]] </b>		<br />									<br />									We seek your inputs for " + "\"CUSTOMER ACTION\"" + ", for an above-mentioned ticket. <br />            </td>                                </tr>                                <tr>                                    <td style=\"padding: 0px 0px 10px; text-align left; color #000000; line-height 1.3; font-size 17px; font-weight bold;font-size 17px;font-family:sans-serif\">                                    Request to you select, the option for closing the loop.  </td>                                </tr>	 <tr>                                    <td style=\"padding: 0px 0px 10px; text-align left; color #000000; line-height 1.3; font-size 17px; font-weight bold;font-size 17px;font-family:sans-serif\">                                    Feel free to have further assistance from Helpdesk. </td>                                </tr>   <tr>                                    <td style=\"padding: 0px 0px 10px; text-align left; color #000000; line-height 1.3; font-size 17px; font-weight bold;font-size 17px;font-family:sans-serif\">                                    The ticket shall automatically CLOSE if not responded within three (3) days.  </td>                                </tr>						<tr>                                    <td style=\"padding: 0px 0px 10px; text-align left; color #000000; line-height 1.3; font-size 17px; font-weight bold;font-size 17px;font-family:sans-serif\">                  <br /><br />          Regards, <br /> <strong>Incident Management Support and Services.</strong> </td>                                </tr>																						</td>								</tr>                            </tbody>                        </table>                        </td>                    </tr>                    <tr>                        <td style=\"padding-top:10px; text-align center; font-family sans-serif; font-size 18px; line-height 20px; color #555555;\">                      </td>                    </tr>                </tbody>            </table>            </td>        </tr>    </tbody></table></div>";
            private static string SMTPUserName = ConfigurationManager.AppSettings["SMTPUserName"];
            private static string SMTPPassword = ConfigurationManager.AppSettings["SMTPPassword"];
            public static int ExecuteNonQueryRe(string procName, IDataParameter[] procParams, string strConn)
            {
                //Method variables

                SqlConnection cnx = null;
                SqlCommand cmd = null;
                int rowaffected = 0;
                try
                {
                    //Setup command object
                    cmd = new SqlCommand(procName);
                    cmd.CommandType = CommandType.StoredProcedure;
                    for (int index = 0; index < procParams.Length; index++)
                    {
                        cmd.Parameters.Add(procParams[index]);
                    }


                    cnx = new SqlConnection(strConn);
                    cmd.Connection = cnx;
                    if (cnx.State == ConnectionState.Closed)
                    {
                        cnx.Open();
                    }

                    //Execute the command
                    rowaffected = cmd.ExecuteNonQuery();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    cnx.Close();
                    cnx.Dispose();
                    if (cmd != null) cmd.Dispose();
                }

                return rowaffected;
            }

            public static SqlParameter CreateParameter(string paramName, SqlDbType paramType, object paramValue, ParameterDirection direction)
            {
                SqlParameter returnVal = new SqlParameter(paramName, paramType);
                returnVal.Value = paramValue;
                returnVal.Direction = direction;
                return returnVal;
            }

            public static DataSet Get_Dataset(string strconString, string sqlQuery)
            {
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

            public static DataSet FindUser(string strquery, string strConstring)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                DataSet objds;
                objcon = new SqlConnection(strConstring);
                try
                {
                    objcmd = new SqlCommand(strquery, objcon);
                    objadp = new SqlDataAdapter(objcmd);
                    objds = new DataSet();
                    try
                    {
                        if (objcon.State == ConnectionState.Closed)
                        {
                            objcon.Open();
                        }
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    objcmd.ExecuteNonQuery();
                    objadp.Fill(objds);
                    objcon.Close();
                    //objcon.Close();
                    objcmd.Dispose();
                    objcon.Dispose();
                    return objds;
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

            //// new code 04 apr 12
            public static DataSet getSticker(string strquery, string strConstring)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                DataSet objds;
                objcon = new SqlConnection(strConstring);
                try
                {
                    objcmd = new SqlCommand(strquery, objcon);
                    objadp = new SqlDataAdapter(objcmd);
                    objds = new DataSet();
                    try
                    {
                        if (objcon.State == ConnectionState.Closed)
                        {
                            objcon.Open();
                        }
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    objcmd.ExecuteNonQuery();
                    objadp.Fill(objds);
                    //objcon.Close();
                    objcmd.Dispose();
                    objcon.Dispose();
                    return objds;
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
            //// new code 04 apr 12

            public static DataSet finddata(string strquery, string strConString)
            {
                string result = "";
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter adp;


                objcon = new SqlConnection(strConString);
                try
                {

                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }

                    objcmd = new SqlCommand(strquery, objcon);
                    adp = new SqlDataAdapter(objcmd);
                    objcmd.ExecuteNonQuery();
                    DataSet ds = new DataSet();
                    adp.Fill(ds);
                    return ds;
                    ds.Dispose();
                    objcon.Close();
                    objcmd.Dispose();
                    objcon.Dispose();
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

            // new code 04 Apr 12


            // end new code apr 12

            public static string UpdateUserPassword(string strquery, string strConstring)
            {

                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                DataSet objds;
                objcon = new SqlConnection(strConstring);
                try
                {
                    objcmd = new SqlCommand(strquery, objcon);

                    try
                    {
                        if (objcon.State == ConnectionState.Closed)
                        {
                            objcon.Open();
                        }
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    objcmd.ExecuteNonQuery();
                    string msg = "You have successfully updated";
                    objcmd.Dispose();
                    return msg;
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

            bool error_flag = false;

            public static string RFCfinddata(string strquery, string strconstring)
            {
                string result = "";
                SqlConnection objcon;
                SqlCommand objcom;
                SqlDataReader objreader;
                objcon = new SqlConnection(strconstring);
                objcon.Open();
                objcom = new SqlCommand(strquery, objcon);
                objreader = objcom.ExecuteReader();
                while (objreader.Read())
                {
                    result = objreader[0].ToString();
                }
                objreader.Close();
                objreader.Dispose();
                objcon.Close();
                objcom.Dispose();
                objcon.Dispose();
                return result;
            }

            public static DataSet GetRFCDetailsByRFCNumber(string ProcName, string StrConString, string rfc_no)
            {
                DataSet objds;
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                objcon = new SqlConnection(StrConString);

                try
                {
                    objcmd = new SqlCommand(ProcName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objadp = new SqlDataAdapter(objcmd);
                    objds = new DataSet();
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }

                    objcmd.Parameters.Add("@rfc_no", SqlDbType.VarChar).Value = rfc_no;
                    objcmd.ExecuteNonQuery();
                    objadp.Fill(objds);
                    objcon.Dispose();
                    return objds;


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

            public static DataSet GetRFCRaisedByUserID(string ProcName, string StrConString, string user_id)
            {
                DataSet objds;
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                objcon = new SqlConnection(StrConString);

                try
                {
                    objcmd = new SqlCommand(ProcName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objadp = new SqlDataAdapter(objcmd);
                    objds = new DataSet();
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }

                    objcmd.Parameters.Add("@user_id", SqlDbType.VarChar).Value = user_id;
                    objcmd.ExecuteNonQuery();
                    objadp.Fill(objds);
                    objcon.Dispose();
                    return objds;
                    //objcon.Close();
                    //objcmd.Dispose();


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

            public static DataSet GetCallHistoryByCallNo(string ProcName, string StrConString, string call_no)
            {
                DataSet objds;
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                objcon = new SqlConnection(StrConString);
                try
                {
                    objcmd = new SqlCommand(ProcName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objadp = new SqlDataAdapter(objcmd);
                    objds = new DataSet();
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }

                    objcmd.Parameters.Add("@call_no", SqlDbType.VarChar).Value = call_no;
                    objcmd.ExecuteNonQuery();
                    objadp.Fill(objds);
                    objcon.Dispose();
                    return objds;
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

            public static DataSet fillList(string ProcName, string strConString)//common method to get dataset for a procedure
            {
                DataSet objds;
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                objcon = new SqlConnection(strConString);

                try
                {
                    objcmd = new SqlCommand(ProcName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;


                    objadp = new SqlDataAdapter(objcmd);
                    objds = new DataSet();
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    objcmd.ExecuteNonQuery();
                    objadp.Fill(objds);
                    objcon.Dispose();
                    return objds;
                    //objcon.Close();
                    //objcmd.Dispose();


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
            public static string InsertRFCDetail(string strconString, string RFC_Number, int Priority, string userID, string location_id, string department_id, string contact_number, string client_name, string client_email, string change_request_type, string company_id, string service_Line_ID, string service_Line_ID_description, string service_line_module, string service_line_module_description, string processes_supported, string requiremnets, string reason_for_change, string business_benefits, string business_risks_expected, string created_by, string created_ip, string created_on, string file)
            {
                bool error_flag = false;
                SqlConnection objcon;
                SqlCommand objcmd;
                objcon = new SqlConnection(strconString);
                string procName = "usp_rfc_InsertRFCDetails";
                string strMsg = "", strResult = "";
                try
                {


                    objcmd = new SqlCommand(procName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;

                    objcmd.Parameters.Add("@RFC_No", SqlDbType.VarChar).Value = RFC_Number;
                    objcmd.Parameters.Add("@Priority", SqlDbType.Int).Value = Priority;
                    objcmd.Parameters.Add("@user_ID", SqlDbType.VarChar).Value = userID;
                    objcmd.Parameters.Add("@location_id", SqlDbType.VarChar).Value = location_id;
                    objcmd.Parameters.Add("@department_id", SqlDbType.VarChar).Value = department_id;
                    objcmd.Parameters.Add("@contact_number", SqlDbType.VarChar).Value = contact_number;
                    objcmd.Parameters.Add("@client_name", SqlDbType.VarChar).Value = client_name;
                    objcmd.Parameters.Add("@client_email", SqlDbType.VarChar).Value = client_email;
                    objcmd.Parameters.Add("@company_id", SqlDbType.VarChar).Value = client_email;
                    objcmd.Parameters.Add("@service_line", SqlDbType.VarChar).Value = service_Line_ID;
                    objcmd.Parameters.Add("@service_line_description", SqlDbType.VarChar).Value = service_Line_ID_description;
                    objcmd.Parameters.Add("@service_line_module", SqlDbType.VarChar, 100).Value = service_line_module;
                    objcmd.Parameters.Add("@service_line_module_description", SqlDbType.VarChar, 100).Value = service_line_module_description;
                    objcmd.Parameters.Add("@change_request_type", SqlDbType.VarChar).Value = change_request_type;
                    objcmd.Parameters.Add("@processes_supported", SqlDbType.VarChar).Value = processes_supported;
                    objcmd.Parameters.Add("@requirements", SqlDbType.VarChar).Value = requiremnets;
                    objcmd.Parameters.Add("@reason_for_change", SqlDbType.VarChar).Value = reason_for_change;
                    objcmd.Parameters.Add("@business_benefits_expected", SqlDbType.VarChar).Value = business_benefits;
                    objcmd.Parameters.Add("@business_risk_expected", SqlDbType.VarChar).Value = business_risks_expected;
                    objcmd.Parameters.Add("@created_by", SqlDbType.VarChar).Value = created_by;
                    objcmd.Parameters.Add("@created_ip", SqlDbType.VarChar).Value = created_ip;
                    objcmd.Parameters.Add("@created_on", SqlDbType.DateTime).Value = Convert.ToDateTime(created_on);
                    objcmd.Parameters.Add("@file", SqlDbType.VarChar).Value = file;
                    objcmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                    objcmd.Parameters["@msg"].Direction = ParameterDirection.Output;

                    objcon.Open();
                    objcmd.ExecuteNonQuery();
                    strMsg = objcmd.Parameters["@msg"].Value.ToString();
                    objcmd.Dispose();
                    objcon.Dispose();
                }
                catch (Exception ex)
                {
                    error_flag = true;
                    throw ex;
                }
                finally
                {
                    if (objcon.State == ConnectionState.Open)
                    {

                        objcon.Close();
                    }
                }
                if (error_flag)
                {
                    strResult = "error";
                }
                else
                {
                    if (strMsg == "Record has been inserted" && error_flag == false)
                    { strResult = "success"; }
                    else if (strMsg == "RFC number already exists")
                        strResult = "RFC number already exists";
                }
                return strResult;
            }

            public static string GetServiceLineHead(string strConString, string serviceline, string companyID)
            {
                bool error_flag = false;
                SqlConnection objcon;
                SqlCommand objcmd;
                objcon = new SqlConnection(strConString);
                string procName = "usp_rfc_GetServiceLineHead";
                string serviceLineHead = "";
                try
                {
                    objcmd = new SqlCommand(procName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@service_line_code", SqlDbType.VarChar).Value = serviceline;
                    objcmd.Parameters.Add("@companyid", SqlDbType.VarChar).Value = companyID;
                    objcon.Open();
                    serviceLineHead = Convert.ToString(objcmd.ExecuteScalar());
                    objcmd.Dispose();
                    objcon.Dispose();
                }
                catch (Exception ex)
                {
                    error_flag = true;
                    throw ex;
                }
                finally
                {
                    if (objcon.State == ConnectionState.Open)
                    {

                        objcon.Close();
                    }
                }
                if (error_flag)
                {
                    return "error";
                }
                else
                {
                    return serviceLineHead;
                }

            }

            public static string GetMaxPriorityByRFCNumber(string strconString, string rfc_number)
            {
                bool error_flag = false;
                SqlConnection objcon;
                SqlCommand objcmd;
                objcon = new SqlConnection(strconString);
                string procName = "usp_rfc_GetMaxPriorityByRFCNumber";
                string MaxPriority = "";
                try
                {

                    objcmd = new SqlCommand(procName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@rfc_number", SqlDbType.VarChar).Value = rfc_number;
                    objcon.Open();
                    MaxPriority = Convert.ToString(objcmd.ExecuteScalar());
                    objcmd.Dispose();
                    objcon.Dispose();


                }
                catch (Exception ex)
                {
                    error_flag = true;
                    throw ex;
                }
                finally
                {
                    if (objcon.State == ConnectionState.Open)
                    {

                        objcon.Close();
                    }
                }
                if (error_flag)
                {
                    return "error";
                }
                else
                {
                    return MaxPriority;
                }


            }

            public static string GetMaxRFCNumber(string strconString, string serviceline)
            {
                bool error_flag = false;
                SqlConnection objcon;
                SqlCommand objcmd;
                objcon = new SqlConnection(strconString);
                string procName = "usp_rfc_GetMaxRFCNumber";
                string MaxRFCNumber = "";
                try
                {

                    objcmd = new SqlCommand(procName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@service_line_code", SqlDbType.VarChar).Value = serviceline;
                    objcon.Open();
                    MaxRFCNumber = Convert.ToString(objcmd.ExecuteScalar());
                    objcmd.Dispose();
                    objcon.Dispose();


                }
                catch (Exception ex)
                {
                    error_flag = true;
                    throw ex;
                }
                finally
                {
                    if (objcon.State == ConnectionState.Open)
                    {

                        objcon.Close();
                    }
                }
                if (error_flag)
                {
                    return "error";
                }
                else
                {
                    return MaxRFCNumber;
                }
            }

            public static string GetRequestID(string strconString, string flag)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                objcon = new SqlConnection(strconString);

                try
                {
                    string autoCode;
                    string procName = "usp_Code_Gen";
                    objcmd = new SqlCommand(procName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                    objcmd.Parameters.Add("@Service_Line_Code", SqlDbType.VarChar).Value = "";
                    objcmd.Parameters.Add("@Module_Code", SqlDbType.VarChar).Value = "";
                    objcmd.Parameters.Add("@Complaint_Code", SqlDbType.VarChar).Value = "";
                    objcmd.Parameters.Add("@GenCode", SqlDbType.VarChar, 20);
                    objcmd.Parameters["@GenCode"].Direction = ParameterDirection.Output;
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    objcmd.ExecuteNonQuery();
                    autoCode = objcmd.Parameters["@GenCode"].Value.ToString();
                    //objcon.Close();
                    objcmd.Dispose();
                    objcon.Dispose();
                    return autoCode;
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

            public static string GetRequestID(string strconString, string slCode, string moduleCode, string complaint_code, string flag)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                objcon = new SqlConnection(strconString);

                try
                {
                    string autoCode;
                    string procName = "usp_Code_Gen";
                    objcmd = new SqlCommand(procName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                    objcmd.Parameters.Add("@Service_Line_Code", SqlDbType.VarChar).Value = slCode;
                    objcmd.Parameters.Add("@Module_Code", SqlDbType.VarChar).Value = moduleCode;
                    objcmd.Parameters.Add("@Complaint_Code", SqlDbType.VarChar).Value = complaint_code;
                    objcmd.Parameters.Add("@GenCode", SqlDbType.VarChar, 20);
                    objcmd.Parameters["@GenCode"].Direction = ParameterDirection.Output;
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    objcmd.ExecuteNonQuery();
                    autoCode = objcmd.Parameters["@GenCode"].Value.ToString();
                    //objcon.Close();
                    objcmd.Dispose();
                    objcon.Dispose();
                    return autoCode;
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

            public static DataSet Get_CallBookingInfo(string procName, string strconString, string ServLine, string Module,
                string FromDate, string ToDate, string Comp_ID, string PrmStrUserId, string PrmStrRole)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                DataSet ds;
                objcon = new SqlConnection(strconString);

                try
                {

                    objcmd = new SqlCommand(procName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@service_line_code", SqlDbType.VarChar, 20).Value = ServLine;
                    objcmd.Parameters.Add("@module_code", SqlDbType.VarChar, 20).Value = Module;
                    objcmd.Parameters.Add("@F_DATE", SqlDbType.SmallDateTime).Value = Convert.ToDateTime(FromDate).ToShortDateString();
                    objcmd.Parameters.Add("@TO_DATE", SqlDbType.SmallDateTime).Value = Convert.ToDateTime(ToDate).ToShortDateString();
                    objcmd.Parameters.Add("@COMP_ID", SqlDbType.VarChar, 20).Value = Comp_ID;
                    objcmd.Parameters.Add("@USER_ID", SqlDbType.VarChar, 20).Value = PrmStrUserId;
                    objcmd.Parameters.Add("@ROLE_ID", SqlDbType.VarChar, 20).Value = PrmStrRole;
                    objcmd.CommandTimeout = 0;
                    //try
                    //{
                    //    objcon.Open();
                    //}
                    //catch (SqlException ex)
                    //{
                    //    throw ex;
                    //}

                    //objcmd.ExecuteNonQuery();
                    ds = new DataSet();
                    objadp = new SqlDataAdapter(objcmd);
                    objadp.Fill(ds);
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

            // Vipin 08 Oct 13
            public static DataSet Get_CallBifurgationInfo(string procName, string strconString, string ServLine, string Module,
               string FromDate, string ToDate, string Comp_ID, string PrmStrUserId, string PrmStrRole)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                DataSet ds;
                objcon = new SqlConnection(strconString);

                try
                {

                    objcmd = new SqlCommand(procName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@SERVICE_LINE_CODE", SqlDbType.VarChar, 20).Value = ServLine;
                    objcmd.Parameters.Add("@MODULE_CODE", SqlDbType.VarChar, 20).Value = Module;
                    objcmd.Parameters.Add("@F_DATE", SqlDbType.SmallDateTime).Value = Convert.ToDateTime(FromDate).ToShortDateString();
                    objcmd.Parameters.Add("@TO_DATE", SqlDbType.SmallDateTime).Value = Convert.ToDateTime(ToDate).ToShortDateString();
                    objcmd.Parameters.Add("@COMP_ID", SqlDbType.VarChar, 20).Value = Comp_ID;
                    objcmd.Parameters.Add("@USER_ID", SqlDbType.VarChar, 20).Value = PrmStrUserId;
                    objcmd.Parameters.Add("@ROLE_ID", SqlDbType.VarChar, 20).Value = PrmStrRole;
                    objcmd.CommandTimeout = 0;
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


            public static DataSet Get_CallBookingInfoFM(string procName, string strconString, string ServLine, string Module,
              string FromDate, string ToDate, string Comp_ID, string PrmStrUserId, string PrmStrRole, string vStatus_Code, string vEngg_Type)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                DataSet ds;
                objcon = new SqlConnection(strconString);

                try
                {

                    objcmd = new SqlCommand(procName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@service_line_code", SqlDbType.VarChar, 20).Value = ServLine;
                    objcmd.Parameters.Add("@module_code", SqlDbType.VarChar, 20).Value = Module;
                    objcmd.Parameters.Add("@F_DATE", SqlDbType.SmallDateTime).Value = Convert.ToDateTime(FromDate).ToShortDateString();
                    objcmd.Parameters.Add("@TO_DATE", SqlDbType.SmallDateTime).Value = Convert.ToDateTime(ToDate).ToShortDateString();
                    objcmd.Parameters.Add("@COMP_ID", SqlDbType.VarChar, 20).Value = Comp_ID;
                    objcmd.Parameters.Add("@USER_ID", SqlDbType.VarChar, 20).Value = PrmStrUserId;
                    objcmd.Parameters.Add("@ROLE_ID", SqlDbType.VarChar, 20).Value = PrmStrRole;
                    objcmd.Parameters.Add("@vStatus_Code", SqlDbType.VarChar, 20).Value = vStatus_Code;
                    objcmd.Parameters.Add("@vEnggType", SqlDbType.VarChar, 4).Value = vEngg_Type;

                    objcmd.CommandTimeout = 0;
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

            public static DataSet Get_CallViewInfo(string procName, string strconString,
                string FromDate, string ToDate, string assigned_to, string service_line)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                DataSet ds;
                objcon = new SqlConnection(strconString);

                try
                {
                    objcmd = new SqlCommand(procName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;

                    objcmd.Parameters.Add("@F_DATE", SqlDbType.SmallDateTime).Value = Convert.ToDateTime(FromDate).ToShortDateString();
                    objcmd.Parameters.Add("@TO_DATE", SqlDbType.SmallDateTime).Value = Convert.ToDateTime(ToDate).ToShortDateString();
                    objcmd.Parameters.Add("@Assigned_to", SqlDbType.VarChar, 20).Value = assigned_to;
                    objcmd.Parameters.Add("@service_line_code", SqlDbType.VarChar, 20).Value = service_line;
                    // objcmd.Parameters.Add("@status", SqlDbType.VarChar, 20).Value = Status;

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


            /* new function vipin 28 apr 14*/

            public static DataSet Get_AttendedBy(string procName, string strconString,
              string FromDate, string ToDate, string vCall_Category, string service_line)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                DataSet ds;
                objcon = new SqlConnection(strconString);

                try
                {
                    objcmd = new SqlCommand(procName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;

                    objcmd.Parameters.Add("@F_DATE", SqlDbType.SmallDateTime).Value = Convert.ToDateTime(FromDate).ToShortDateString();
                    objcmd.Parameters.Add("@TO_DATE", SqlDbType.SmallDateTime).Value = Convert.ToDateTime(ToDate).ToShortDateString();
                    objcmd.Parameters.Add("@service_line_code", SqlDbType.VarChar, 20).Value = service_line;
                    objcmd.Parameters.Add("@Call_Category", SqlDbType.VarChar, 20).Value = vCall_Category;

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


            public static DataSet Get_CallCategory(string procName, string strconString)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                DataSet ds;
                objcon = new SqlConnection(strconString);

                try
                {
                    objcmd = new SqlCommand(procName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;

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


            public static DataSet Get_Calls_By_Category(string procName, string strconString,
             string FromDate, string ToDate, string vCall_Category, string service_line, string AttendedBy, string CallNo)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                DataSet ds;
                objcon = new SqlConnection(strconString);

                try
                {
                    objcmd = new SqlCommand(procName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;

                    objcmd.Parameters.Add("@F_DATE", SqlDbType.SmallDateTime).Value = string.IsNullOrEmpty(FromDate) ? null : Convert.ToDateTime(FromDate).ToShortDateString();
                    objcmd.Parameters.Add("@TO_DATE", SqlDbType.SmallDateTime).Value = string.IsNullOrEmpty(ToDate) ? null : Convert.ToDateTime(ToDate).ToShortDateString();
                    objcmd.Parameters.Add("@service_line_code", SqlDbType.VarChar, 20).Value = service_line;
                    objcmd.Parameters.Add("@Call_Category", SqlDbType.VarChar, 20).Value = vCall_Category;
                    objcmd.Parameters.Add("@Assigned_To", SqlDbType.VarChar, 20).Value = AttendedBy;
                    objcmd.Parameters.Add("@Call_No", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(CallNo) ? "%" : CallNo;

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


            /* END   Apr 2014*/
            public static DataSet Get_Call_Complaint_Summary(string procName, string strconString, string fDate, string toDate
                , string strUserId, string flag, string service_line)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                DataSet ds;
                objcon = new SqlConnection(strconString);

                try
                {
                    objcmd = new SqlCommand(procName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@flag", SqlDbType.VarChar, 30).Value = flag;
                    objcmd.Parameters.Add("@FDATE", SqlDbType.SmallDateTime).Value = fDate;
                    objcmd.Parameters.Add("@TODATE", SqlDbType.SmallDateTime).Value = toDate;
                    objcmd.Parameters.Add("@ASSIGNED_TO", SqlDbType.VarChar, 50).Value = strUserId;
                    objcmd.Parameters.Add("@service_line_code", SqlDbType.VarChar, 50).Value = service_line;


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


            public static DataSet Get_Call_SL_Summary(string procName, string strconString, string fDate, string toDate
               , string strUserId, string service_code)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                DataSet ds;
                objcon = new SqlConnection(strconString);

                try
                {
                    objcmd = new SqlCommand(procName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.CommandTimeout = 10000000;
                    objcmd.Parameters.Add("@FDATE", SqlDbType.SmallDateTime).Value = fDate;
                    objcmd.Parameters.Add("@TODATE", SqlDbType.SmallDateTime).Value = toDate;
                    objcmd.Parameters.Add("@ASSIGNED_TO", SqlDbType.VarChar, 50).Value = strUserId;
                    objcmd.Parameters.Add("@service_line_code", SqlDbType.VarChar, 50).Value = service_code;


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

            // Vipin  01 mar 14

            public static DataSet Get_Calls_time_updation(string procName, string strconString, string fDate, string toDate)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                DataSet ds;
                objcon = new SqlConnection(strconString);

                try
                {
                    objcmd = new SqlCommand(procName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;

                    objcmd.Parameters.Add("@FDATE", SqlDbType.SmallDateTime).Value = fDate;
                    objcmd.Parameters.Add("@TODATE", SqlDbType.DateTime).Value = toDate;

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


            public static void update_cumulative_time(string procName, string strConString, string Call_No)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                objcon = new SqlConnection(strConString);
                try
                {
                    objcmd = new SqlCommand(procName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@Call_No", SqlDbType.VarChar).Value = Call_No;

                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }

                    objcmd.ExecuteNonQuery();

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


            // End 

            public static DataSet Get_Call_Severity_Summary(string procName, string strconString, string fDate, string toDate
               , string strUserId, string service_code
               )
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                DataSet ds;
                objcon = new SqlConnection(strconString);

                try
                {
                    objcmd = new SqlCommand(procName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;

                    objcmd.Parameters.Add("@FDATE", SqlDbType.SmallDateTime).Value = fDate;
                    objcmd.Parameters.Add("@TODATE", SqlDbType.DateTime).Value = toDate;
                    objcmd.Parameters.Add("@ASSIGNED_TO", SqlDbType.VarChar, 50).Value = strUserId;
                    objcmd.Parameters.Add("@service_line_code", SqlDbType.VarChar, 50).Value = service_code;

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


            public static DataSet Get_Call_Severity_Call(string procName, string strconString, string fDate, string toDate
              , string strUserId, string service_code, string NoOfHours, string severity, string SLAStatus, string vCall_type
              )
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                DataSet ds;
                objcon = new SqlConnection(strconString);

                try
                {
                    objcmd = new SqlCommand(procName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;

                    objcmd.Parameters.Add("@FDATE", SqlDbType.SmallDateTime).Value = fDate;
                    objcmd.Parameters.Add("@TODATE", SqlDbType.SmallDateTime).Value = toDate;
                    objcmd.Parameters.Add("@ASSIGNED_TO", SqlDbType.VarChar, 50).Value = strUserId;
                    objcmd.Parameters.Add("@SERVICE_LINE", SqlDbType.VarChar, 50).Value = service_code;
                    objcmd.Parameters.Add("@SEVERITY", SqlDbType.VarChar, 30).Value = severity;
                    objcmd.Parameters.Add("@NoOFHours", SqlDbType.Char, 3).Value = NoOfHours;
                    objcmd.Parameters.Add("@SLAStatus", SqlDbType.Char, 3).Value = SLAStatus;
                    // new code 29 mar 12
                    objcmd.Parameters.Add("@vCall_Type", SqlDbType.VarChar, 20).Value = vCall_type;


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


            public static DataSet Get_CallSummaryInfo(string procName, string strconString, string ServLine, string FromDate, string ToDate)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                DataSet ds;
                objcon = new SqlConnection(strconString);

                try
                {
                    objcmd = new SqlCommand(procName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@Service_Line_Code", SqlDbType.VarChar, 20).Value = ServLine;
                    objcmd.Parameters.Add("@From_Date", SqlDbType.VarChar, 20).Value = Convert.ToDateTime(FromDate).ToShortDateString();
                    objcmd.Parameters.Add("@To_Date", SqlDbType.VarChar, 20).Value = Convert.ToDateTime(ToDate).ToShortDateString();

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

            public static DataSet Get_CallLogInfo(string procName, string strconString, string calNo, string flag)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                objcon = new SqlConnection(strconString);
                DataSet ds = new DataSet();
                try
                {
                    objcmd = new SqlCommand(procName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                    objcmd.Parameters.Add("@Call_No", SqlDbType.VarChar).Value = calNo;
                    objcmd.CommandTimeout = 120;
                    objadp = new SqlDataAdapter(objcmd);
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    objcmd.ExecuteNonQuery();
                    objadp.Fill(ds);
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

            public static DataSet Get_SolutionCategory_Reports_Info(string procName, string strconString,
                string slCode, string moduleCode, string fromDate, string toDate)
            {

                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                objcon = new SqlConnection(strconString);
                DataSet ds = new DataSet();
                try
                {
                    string autoCode;
                    objcmd = new SqlCommand(procName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@F_DATE", SqlDbType.VarChar, 20).Value = Convert.ToDateTime(fromDate).ToShortDateString();
                    objcmd.Parameters.Add("@TO_DATE", SqlDbType.VarChar, 20).Value = Convert.ToDateTime(toDate).ToShortDateString();
                    objcmd.Parameters.Add("@Service_Line_Code", SqlDbType.VarChar).Value = slCode;
                    objcmd.Parameters.Add("@Module_Code", SqlDbType.VarChar).Value = moduleCode;
                    // objcmd.Parameters.Add("@Solution_Code", SqlDbType.VarChar).Value = solutionCode;

                    objadp = new SqlDataAdapter(objcmd);
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    objcmd.ExecuteNonQuery();
                    objadp.Fill(ds);
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

            public static DataSet Get_PendingCallInfo(string procName, string strconString, string slCode, string compCode,
                string moduleCode, string call_no, string UserID, string flag, string engg_code, bool isHelpdesk, bool viewASICACall)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                objcon = new SqlConnection(strconString);
                DataSet ds = new DataSet();
                try
                {
                    string autoCode;

                    objcmd = new SqlCommand(procName, objcon);
                    objcmd.CommandTimeout = 120;
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                    objcmd.Parameters.Add("@User_ID", SqlDbType.VarChar).Value = UserID;
                    objcmd.Parameters.Add("@Service_Line_Code", SqlDbType.VarChar).Value = slCode;
                    objcmd.Parameters.Add("@Module_Code", SqlDbType.VarChar).Value = moduleCode;
                    objcmd.Parameters.Add("@Comp_Code", SqlDbType.VarChar).Value = compCode;
                    objcmd.Parameters.Add("@CallNo", SqlDbType.VarChar).Value = call_no;
                    objcmd.Parameters.Add("@Engg_Code", SqlDbType.VarChar).Value = engg_code;
                    objcmd.Parameters.Add("@ViewHelpdeskEngCall", SqlDbType.Bit).Value = isHelpdesk;
                    objcmd.Parameters.Add("@ViewASICACALL", SqlDbType.Bit).Value = viewASICACall;

                    objadp = new SqlDataAdapter(objcmd);

                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }

                    objcmd.ExecuteNonQuery();
                    objadp.Fill(ds);
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

            public static DataSet Get_PendingCallInfo_sorting(string procName, string strconString, string slCode, string compCode,
                string moduleCode, string UserID, string flag, string str_sort)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                objcon = new SqlConnection(strconString);
                DataSet ds = new DataSet();
                try
                {
                    string autoCode;
                    objcmd = new SqlCommand(procName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                    objcmd.Parameters.Add("@User_ID", SqlDbType.VarChar).Value = UserID;
                    objcmd.Parameters.Add("@Service_Line_Code", SqlDbType.VarChar).Value = slCode;
                    objcmd.Parameters.Add("@Module_Code", SqlDbType.VarChar).Value = moduleCode;
                    objcmd.Parameters.Add("@Comp_Code", SqlDbType.VarChar).Value = compCode;
                    objadp = new SqlDataAdapter(objcmd);
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    objcmd.ExecuteNonQuery();
                    objadp.Fill(ds);
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

            public static DataSet Get_LoggedCallSatusInfo(string procName, string strconString,
                string slCode, string UserID, string callStatus, string flag, string compID, string statusFilter)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                objcon = new SqlConnection(strconString);
                DataSet ds = new DataSet();
                try
                {
                    string autoCode;
                    objcmd = new SqlCommand(procName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                    objcmd.Parameters.Add("@User_ID", SqlDbType.VarChar).Value = UserID;
                    objcmd.Parameters.Add("@Service_Line_Code", SqlDbType.VarChar).Value = slCode;
                    objcmd.Parameters.Add("@Call_Status", SqlDbType.VarChar).Value = callStatus;
                    objcmd.Parameters.Add("@comp_id", SqlDbType.VarChar).Value = compID;
                    objcmd.Parameters.Add("@Status_Filter", SqlDbType.VarChar).Value = statusFilter;
                    objadp = new SqlDataAdapter(objcmd);
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    objcmd.ExecuteNonQuery();
                    objadp.Fill(ds);
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

            public static void InsertNewPassword(string strConString, string newPassword, string userID)
            {

                SqlConnection objcon;
                SqlCommand objcmd;
                objcon = new SqlConnection(strConString);

                try
                {
                    DateTime ChangeDate = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss", CultureInfo.InvariantCulture));
                    string str = "Update USER_IDENTITY Set HasDefaultPasswordChange =1,User_Password = '" + newPassword + "',LastPasswordChangeDate = '" + ChangeDate + "' , UpdatedBy = '" + userID + "' , UpdatedDate = '" + ChangeDate + "'  where User_ID = '" + userID + "'";
                    objcmd = new SqlCommand(str, objcon);

                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }

                    objcmd.ExecuteNonQuery();

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

            public static string InsertAdminCreation_Details(string procName, string strConString, string uid,
                string name, string password)
            {

                SqlConnection objcon;
                SqlCommand objcmd;
                objcon = new SqlConnection(strConString);
                try
                {
                    objcmd = new SqlCommand(procName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@User_ID", SqlDbType.VarChar).Value = uid;
                    objcmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = name;
                    objcmd.Parameters.Add("@User_Password", SqlDbType.VarChar).Value = password;
                    objcmd.Parameters.Add("@User_Type", SqlDbType.VarChar).Value = "ADM";
                    objcmd.Parameters.Add("@Engg_Code", SqlDbType.VarChar).Value = uid;
                    objcmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                    objcmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }

                    objcmd.ExecuteNonQuery();
                    string strMsg = objcmd.Parameters["@msg"].Value.ToString();
                    //objcon.Close();
                    return strMsg;
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

            public static string InsertMaster_Complaint(string procName, string strConString,
                string compCode, string compDesc, string severityCode, string active, string flag)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                objcon = new SqlConnection(strConString);
                try
                {
                    objcmd = new SqlCommand(procName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                    objcmd.Parameters.Add("@Complaint_Code", SqlDbType.VarChar).Value = compCode;
                    objcmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = compDesc;
                    objcmd.Parameters.Add("@Severity_Code", SqlDbType.VarChar).Value = severityCode;
                    objcmd.Parameters.Add("@Active", SqlDbType.Char).Value = active;
                    objcmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                    objcmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }

                    objcmd.ExecuteNonQuery();
                    string strMsg = objcmd.Parameters["@msg"].Value.ToString();
                    //objcon.Close();
                    return strMsg;
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


            public static string InsertMaster_solution(string procName, string strConString,
                string solution_code, string solution_desc, string active, string flag)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                objcon = new SqlConnection(strConString);
                try
                {
                    objcmd = new SqlCommand(procName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                    objcmd.Parameters.Add("@solution_code", SqlDbType.VarChar).Value = solution_code;
                    objcmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = solution_desc;
                    objcmd.Parameters.Add("@Active", SqlDbType.Char).Value = active;
                    objcmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                    objcmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }

                    objcmd.ExecuteNonQuery();
                    string strMsg = objcmd.Parameters["@msg"].Value.ToString();
                    //objcon.Close();
                    return strMsg;
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


            public static string InsertMaster_Complaint_mapping(string procName, string strConString,
                string company_id, string service_line, string module, string complaint, string flag)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                objcon = new SqlConnection(strConString);
                try
                {
                    objcmd = new SqlCommand(procName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                    objcmd.Parameters.Add("@company_id", SqlDbType.VarChar).Value = company_id;
                    objcmd.Parameters.Add("@Service_line_Code", SqlDbType.VarChar).Value = service_line;
                    objcmd.Parameters.Add("@module_Code", SqlDbType.VarChar).Value = module;
                    objcmd.Parameters.Add("@Complaint_Code", SqlDbType.VarChar).Value = complaint;
                    objcmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                    objcmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }

                    objcmd.ExecuteNonQuery();
                    string strMsg = objcmd.Parameters["@msg"].Value.ToString();
                    //objcon.Close();
                    return strMsg;
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

            public static string InsertMaster_SOLUTION_mapping(string procName, string strConString,
                string company_id, string service_line, string module, string solution, string flag)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                objcon = new SqlConnection(strConString);
                try
                {
                    objcmd = new SqlCommand(procName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                    objcmd.Parameters.Add("@company_id", SqlDbType.VarChar).Value = company_id;
                    objcmd.Parameters.Add("@Service_line_Code", SqlDbType.VarChar).Value = service_line;
                    objcmd.Parameters.Add("@module_Code", SqlDbType.VarChar).Value = module;
                    objcmd.Parameters.Add("@solution_Code", SqlDbType.VarChar).Value = solution;
                    objcmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                    objcmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }

                    objcmd.ExecuteNonQuery();
                    string strMsg = objcmd.Parameters["@msg"].Value.ToString();
                    //objcon.Close();
                    return strMsg;
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



            public static string InsertMaster_ServiceLine(string ProcName, string strConString, string slCode,
                 string active, string slHead, string slEmail, string CompID, string engg_code, string flag)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                objcon = new SqlConnection(strConString);
                try
                {

                    objcmd = new SqlCommand(ProcName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;

                    objcmd.Parameters.Add("@companyid", SqlDbType.VarChar).Value = CompID;
                    objcmd.Parameters.Add("@Service_Line_Code", SqlDbType.VarChar).Value = slCode;
                    objcmd.Parameters.Add("@Engg_Code", SqlDbType.VarChar).Value = engg_code;
                    //objcmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = sldesc;
                    objcmd.Parameters.Add("@Active", SqlDbType.Bit).Value = Convert.ToInt16(active);
                    objcmd.Parameters.Add("@Service_Line_Head", SqlDbType.VarChar).Value = slHead;
                    objcmd.Parameters.Add("@ServiceLineHead_Email", SqlDbType.VarChar).Value = slEmail;
                    objcmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                    objcmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }

                    objcmd.ExecuteNonQuery();
                    string strMsg = objcmd.Parameters["@msg"].Value.ToString();
                    //objcon.Close();
                    return strMsg;
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

            public static string InsertMaster_ServiceLine_detail(string ProcName, string strConString, string slCode,
               string sldesc, int active, string flag)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                objcon = new SqlConnection(strConString);
                try
                {

                    objcmd = new SqlCommand(ProcName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;

                    objcmd.Parameters.Add("@Service_Line_Code", SqlDbType.VarChar).Value = slCode;
                    objcmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = sldesc;
                    objcmd.Parameters.Add("@Active", SqlDbType.Bit).Value = active;

                    objcmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                    objcmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }

                    objcmd.ExecuteNonQuery();
                    string strMsg = objcmd.Parameters["@msg"].Value.ToString();
                    //objcon.Close();
                    return strMsg;
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


            public static string InsertMaster_ServiceLineModule(string ProcName, string strConString, string slCode,
               string moduleCode, string moduledesc, string compCode, string active, string flag)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                objcon = new SqlConnection(strConString);
                try
                {
                    //objcon = new SqlConnection(strConString);
                    objcmd = new SqlCommand(ProcName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                    objcmd.Parameters.Add("@Service_Line_Code", SqlDbType.VarChar).Value = slCode;
                    objcmd.Parameters.Add("@Module_Code", SqlDbType.VarChar).Value = moduleCode;
                    objcmd.Parameters.Add("@Module_Description", SqlDbType.VarChar).Value = moduledesc;
                    objcmd.Parameters.Add("@Comp_Code", SqlDbType.VarChar).Value = compCode;
                    objcmd.Parameters.Add("@Active", SqlDbType.VarChar).Value = active;
                    objcmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                    objcmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }

                    objcmd.ExecuteNonQuery();
                    string strMsg = objcmd.Parameters["@msg"].Value.ToString();
                    //objcon.Close();
                    return strMsg;
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

            public static string InsertMaster_SolutionCetegory(string ProcName, string strConString, string slCode,
              string scCode, string scdesc, string moduleCode, string moduleDesc, string active, string flag)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                objcon = new SqlConnection(strConString);
                try
                {
                    //objcon = new SqlConnection(strConString);
                    objcmd = new SqlCommand(ProcName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                    objcmd.Parameters.Add("@Service_Line_Code", SqlDbType.VarChar).Value = slCode;
                    objcmd.Parameters.Add("@Solution_Code", SqlDbType.VarChar).Value = scCode;
                    objcmd.Parameters.Add("@Solution_Desc", SqlDbType.VarChar).Value = scdesc;
                    objcmd.Parameters.Add("@Module_Code", SqlDbType.VarChar).Value = moduleCode;
                    objcmd.Parameters.Add("@Module_Desc", SqlDbType.VarChar).Value = moduleDesc;
                    objcmd.Parameters.Add("@Active", SqlDbType.VarChar).Value = active;
                    objcmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                    objcmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }

                    objcmd.ExecuteNonQuery();
                    string strMsg = objcmd.Parameters["@msg"].Value.ToString();
                    //objcon.Close();
                    return strMsg;
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

            public static string InsertMaster_Vendor(string ProcName, string strConString,
             string vendCode, string venddesc, string active, string flag)
            {

                SqlConnection objcon;
                SqlCommand objcmd;
                objcon = new SqlConnection(strConString);
                try
                {
                    //objcon = new SqlConnection(strConString);
                    objcmd = new SqlCommand(ProcName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                    objcmd.Parameters.Add("@Vendor_Code", SqlDbType.VarChar).Value = vendCode;
                    objcmd.Parameters.Add("@Vendor_Description", SqlDbType.VarChar).Value = venddesc;

                    objcmd.Parameters.Add("@Active", SqlDbType.VarChar).Value = active;
                    objcmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                    objcmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }

                    objcmd.ExecuteNonQuery();
                    string strMsg = objcmd.Parameters["@msg"].Value.ToString();
                    //objcon.Close();
                    return strMsg;
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

            public static string InsertMaster_Company(string ProcName, string strConString, string compCode,
                string compdesc, string active, string flag)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                objcon = new SqlConnection(strConString);
                try
                {
                    objcmd = new SqlCommand(ProcName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                    objcmd.Parameters.Add("@Comp_Code", SqlDbType.VarChar).Value = compCode;
                    objcmd.Parameters.Add("@Comp_desc", SqlDbType.VarChar).Value = compdesc;
                    objcmd.Parameters.Add("@Active", SqlDbType.VarChar).Value = active;
                    objcmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                    objcmd.Parameters["@msg"].Direction = ParameterDirection.Output;

                    objcon.Open();
                    objcmd.ExecuteNonQuery();
                    string strMsg = objcmd.Parameters["@msg"].Value.ToString();
                    return strMsg;
                    objcmd.Dispose();

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

            public static string InsertMaster_Status(string ProcName, string strConString, string statusCode,
                string statusdesc, string active, string flag)
            {

                SqlConnection objcon;
                SqlCommand objcmd;
                objcon = new SqlConnection(strConString);
                try
                {
                    //objcon = new SqlConnection(strConString);
                    objcmd = new SqlCommand(ProcName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                    objcmd.Parameters.Add("@Status_Code", SqlDbType.VarChar).Value = statusCode;
                    objcmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = statusdesc;
                    objcmd.Parameters.Add("@Active", SqlDbType.VarChar).Value = active;
                    objcmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                    objcmd.Parameters["@msg"].Direction = ParameterDirection.Output;

                    objcon.Open();
                    objcmd.ExecuteNonQuery();
                    string strMsg = objcmd.Parameters["@msg"].Value.ToString();
                    //objcon.Close();
                    return strMsg;
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

            public static string InsertMaster_Location(string ProcName, string strConString, string locDesc,
                string desc, string active, string flag)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                objcon = new SqlConnection(strConString);
                try
                {
                    // objcon = new SqlConnection(strConString);
                    objcmd = new SqlCommand(ProcName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@Flag", SqlDbType.VarChar, 20).Value = flag;
                    objcmd.Parameters.Add("@Location_Code", SqlDbType.VarChar, 20).Value = locDesc;
                    objcmd.Parameters.Add("@Description", SqlDbType.VarChar, 50).Value = desc;
                    objcmd.Parameters.Add("@Active", SqlDbType.VarChar, 1).Value = active;
                    objcmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                    objcmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    objcmd.ExecuteNonQuery();
                    string strMsg = objcmd.Parameters["@msg"].Value.ToString();
                    //objcon.Close();
                    return strMsg;
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


            public static string InsertMaster_Department(string ProcName, string strConString, string depDesc,
               string desc, string active, string flag)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                objcon = new SqlConnection(strConString);
                try
                {
                    // objcon = new SqlConnection(strConString);
                    objcmd = new SqlCommand(ProcName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@Flag", SqlDbType.VarChar, 20).Value = flag;
                    objcmd.Parameters.Add("@Department_Code", SqlDbType.VarChar, 20).Value = depDesc;
                    objcmd.Parameters.Add("@Description", SqlDbType.VarChar, 50).Value = desc;
                    objcmd.Parameters.Add("@Active", SqlDbType.VarChar, 1).Value = active;
                    objcmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                    objcmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    objcmd.ExecuteNonQuery();
                    string strMsg = objcmd.Parameters["@msg"].Value.ToString();
                    //objcon.Close();
                    return strMsg;
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

            public static string InsertMaster_Department_mapping(string ProcName, string strConString,
                string COMPANY_ID, string locCode, string deptCode, string flag)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                objcon = new SqlConnection(strConString);
                try
                {
                    // objcon = new SqlConnection(strConString);
                    objcmd = new SqlCommand(ProcName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                    objcmd.Parameters.Add("@Location_Code", SqlDbType.VarChar).Value = locCode;
                    objcmd.Parameters.Add("@Department_Code", SqlDbType.VarChar).Value = deptCode;
                    objcmd.Parameters.Add("@COMPANY_ID", SqlDbType.VarChar).Value = COMPANY_ID;
                    objcmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                    objcmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }

                    objcmd.ExecuteNonQuery();
                    string strMsg = objcmd.Parameters["@msg"].Value.ToString();
                    //objcon.Close();
                    return strMsg;

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

            public static string InsertMaster_Client(string ProcName, string strConString, string empCode, string empName, string locCode,
               string deptCode, string email, string phone, bool active, string designation, string EmpType, string compCode, string flag)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                objcon = new SqlConnection(strConString);
                try
                {
                    //objcon = new SqlConnection(strConString);
                    objcmd = new SqlCommand(ProcName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                    objcmd.Parameters.Add("@Emp_Code", SqlDbType.VarChar).Value = empCode;
                    objcmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = empName;
                    objcmd.Parameters.Add("@Location_Code", SqlDbType.VarChar).Value = locCode;
                    objcmd.Parameters.Add("@Department_Code", SqlDbType.VarChar).Value = deptCode;
                    objcmd.Parameters.Add("@eMail_ID", SqlDbType.VarChar).Value = email;
                    objcmd.Parameters.Add("@Phone_Ext_No", SqlDbType.VarChar).Value = phone;
                    objcmd.Parameters.Add("@LandMark ", SqlDbType.VarChar).Value = "";
                    objcmd.Parameters.Add("@Active", SqlDbType.Bit).Value = active;
                    //objcmd.Parameters.Add("@password", SqlDbType.VarChar).Value = strpass;
                    objcmd.Parameters.Add("@User_Type", SqlDbType.VarChar).Value = EmpType;
                    objcmd.Parameters.Add("@Comp_Code", SqlDbType.VarChar).Value = compCode;
                    objcmd.Parameters.Add("@Designation", SqlDbType.VarChar).Value = designation;
                    objcmd.Parameters.Add("@login_access", SqlDbType.VarChar).Value = "";
                    objcmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                    objcmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }

                    objcmd.ExecuteNonQuery();
                    string strMsg = objcmd.Parameters["@msg"].Value.ToString();
                    //objcon.Close();
                    return strMsg;

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

            public static string InsertMaster_ServiceEngineer(string ProcName, string strConString, string engCode, string engName, string locCode,
              string servCode, string email, string phone, bool active, string emptype, string compCode, string strModule, string flag)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                objcon = new SqlConnection(strConString);
                objcmd = new SqlCommand(ProcName, objcon);
                try
                {
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                    objcmd.Parameters.Add("@Engineer_Code", SqlDbType.VarChar).Value = engCode;
                    objcmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = engName;
                    objcmd.Parameters.Add("@Location_Code", SqlDbType.VarChar).Value = locCode;
                    objcmd.Parameters.Add("@Service_Line_Code", SqlDbType.VarChar).Value = servCode;
                    objcmd.Parameters.Add("@eMail_ID", SqlDbType.VarChar).Value = email;
                    objcmd.Parameters.Add("@Phone_Ext_No", SqlDbType.VarChar).Value = phone;
                    objcmd.Parameters.Add("@Active", SqlDbType.Bit).Value = active;
                    objcmd.Parameters.Add("@Engineer_Type", SqlDbType.VarChar).Value = "Service Engineer Master";
                    objcmd.Parameters.Add("@Comp_Code", SqlDbType.VarChar).Value = compCode;
                    objcmd.Parameters.Add("@module_code", SqlDbType.VarChar).Value = strModule;
                    //objcmd.Parameters.Add("@password", SqlDbType.VarChar).Value = strPass;
                    objcmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                    objcmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }

                    objcmd.ExecuteNonQuery();
                    string strMsg = objcmd.Parameters["@msg"].Value.ToString();

                    //objcon.Close();
                    return strMsg;

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
                        objcmd.Parameters.Clear();
                        objcmd.Dispose();
                    }
                }
            }


            public static string Insert_New_CallLog(ref int i, string calNo, string ProcName, string strConString, string logBy, string locCode, string deptCode,
             string email, string phone, string slCode, string moduleCode, string complCode, string remark,
                string assignTo, string compCode, string CreatedBy, string flag, string assetName, string assetNumber, string severity, string strFileName, string strSavedFileName, string strAssetStickerNo, string newUserSLHEmail, string newUserPMEmail, bool hasGeneratedPortal, bool IsGLP)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                objcon = new SqlConnection(strConString);

                try
                {
                    //objcon = new SqlConnection(strConString);
                    objcmd = new SqlCommand(ProcName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                    objcmd.Parameters.Add("@Call_No", SqlDbType.VarChar).Value = calNo;
                    objcmd.Parameters.Add("@Emp_Code", SqlDbType.VarChar).Value = logBy;
                    objcmd.Parameters.Add("@ServiceLine_Code", SqlDbType.VarChar).Value = slCode;
                    objcmd.Parameters.Add("@Module_Code", SqlDbType.VarChar).Value = moduleCode;
                    objcmd.Parameters.Add("@Location_Code", SqlDbType.VarChar).Value = locCode;
                    objcmd.Parameters.Add("@Department_Code", SqlDbType.VarChar).Value = deptCode;
                    objcmd.Parameters.Add("@Complaint_Code", SqlDbType.VarChar).Value = complCode;
                    objcmd.Parameters.Add("@Phone_Ext_No", SqlDbType.VarChar).Value = phone;
                    objcmd.Parameters.Add("@Assign_To ", SqlDbType.VarChar).Value = assignTo;
                    objcmd.Parameters.Add("@Remarks", SqlDbType.VarChar).Value = remark;
                    objcmd.Parameters.Add("@Comp_Code", SqlDbType.VarChar).Value = compCode;
                    objcmd.Parameters.Add("@Call_Date ", SqlDbType.SmallDateTime).Value = System.DateTime.Now;
                    objcmd.Parameters.Add("@Call_Close_Date", SqlDbType.VarChar).Value = "";
                    objcmd.Parameters.Add("@mail_ID", SqlDbType.VarChar).Value = email;
                    objcmd.Parameters.Add("@created_by", SqlDbType.VarChar).Value = CreatedBy;
                    objcmd.Parameters.Add("@assetName", SqlDbType.VarChar).Value = assetName;
                    objcmd.Parameters.Add("@assetNumber", SqlDbType.VarChar).Value = assetNumber;
                    objcmd.Parameters.Add("@severity", SqlDbType.VarChar).Value = severity;
                    objcmd.Parameters.Add("@FileName", SqlDbType.VarChar).Value = strFileName;
                    objcmd.Parameters.Add("@SavedFileName", SqlDbType.VarChar).Value = strSavedFileName;
                    objcmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                    objcmd.Parameters.Add("@AssetStickerNo", SqlDbType.VarChar).Value = strAssetStickerNo; // new code 4 apr 12
                    objcmd.Parameters.Add("@HasGeneratedZydusPortal", SqlDbType.Bit).Value = hasGeneratedPortal;
                    objcmd.Parameters.Add("@ISGLP", SqlDbType.Bit).Value = IsGLP;
                    objcmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }

                    i = objcmd.ExecuteNonQuery();
                    string strMsg = objcmd.Parameters["@msg"].Value.ToString();
                    sendmail(calNo, logBy, locCode, deptCode, email, phone, slCode, moduleCode, complCode, remark, assignTo, compCode, severity, newUserSLHEmail, newUserPMEmail);
                    return strMsg;

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

            public static string Insert_Pending_Call_Info(string ProcName, string strConString, string calNo, int slNo,
                string logBy, string statusCode, DateTime statusDate, string assignTo, string comment, string slCode,
             string moduleCode, string complCode, string solutionCode, string severityCode, string vendCode, string comp_code, string rfc_no, string userID, string flag, string vCallType, string vAssetStickerNo, string vCallCategory,
                string newUserId, string expectedCloseDate, bool isGLP, string fileName, string savedFilename, string strFrontlineConnection, string loginUserId)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                objcon = new SqlConnection(strConString);
                try
                {

                    //objcon = new SqlConnection(strConString);
                    objcmd = new SqlCommand(ProcName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;

                    objcmd.Parameters.Add("@Call_No", SqlDbType.VarChar).Value = calNo;
                    objcmd.Parameters.Add("@User_ID", SqlDbType.VarChar).Value = logBy;
                    objcmd.Parameters.Add("@SL_No", SqlDbType.Int).Value = slNo;
                    objcmd.Parameters.Add("@Service_Line_Code", SqlDbType.VarChar).Value = slCode;
                    objcmd.Parameters.Add("@Module_Code", SqlDbType.VarChar).Value = moduleCode;
                    objcmd.Parameters.Add("@Status_Code", SqlDbType.VarChar).Value = statusCode;
                    objcmd.Parameters.Add("@Severity", SqlDbType.VarChar).Value = severityCode;
                    objcmd.Parameters.Add("@Complaint_Code", SqlDbType.VarChar).Value = complCode;
                    objcmd.Parameters.Add("@Comment", SqlDbType.VarChar).Value = comment;
                    objcmd.Parameters.Add("@Assigned_To", SqlDbType.VarChar).Value = assignTo;
                    objcmd.Parameters.Add("@Status_Date ", SqlDbType.DateTime).Value = System.DateTime.Now;//statusDate.ToLongDateString();
                    objcmd.Parameters.Add("@Call_Close_Date", SqlDbType.DateTime).Value = System.DateTime.Now;
                    objcmd.Parameters.Add("@Solution_Code", SqlDbType.VarChar).Value = solutionCode;
                    objcmd.Parameters.Add("@rfc_no", SqlDbType.VarChar).Value = rfc_no;
                    objcmd.Parameters.Add("@Vendor_Code", SqlDbType.VarChar).Value = vendCode;
                    objcmd.Parameters.Add("@Created_By", SqlDbType.VarChar).Value = userID;
                    objcmd.Parameters.Add("@NewUserId", SqlDbType.VarChar).Value = newUserId;

                    objcmd.Parameters.Add("@fileName", SqlDbType.VarChar).Value = fileName;
                    objcmd.Parameters.Add("@SavedFileName", SqlDbType.VarChar).Value = savedFilename;
                    objcmd.Parameters.Add("@LoginUserId", SqlDbType.VarChar).Value = loginUserId;

                    objcmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                    objcmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                    // 04 Apr 12
                    objcmd.Parameters.Add("@AssetStickerNo", SqlDbType.VarChar).Value = vAssetStickerNo;
                    objcmd.Parameters.Add("@vCall_Type", SqlDbType.VarChar).Value = vCallType; // New code 28 march 12
                                                                                               // 06 june 13 adding call category
                    objcmd.Parameters.Add("@CallCategory", SqlDbType.VarChar).Value = vCallCategory; // New code 28 march 12

                    if (string.IsNullOrEmpty(expectedCloseDate))
                        objcmd.Parameters.Add("@ExpectedCloseDate", SqlDbType.DateTime).Value = DBNull.Value;
                    else
                        objcmd.Parameters.Add("@ExpectedCloseDate", SqlDbType.DateTime).Value = Convert.ToDateTime(expectedCloseDate);



                    objcmd.Parameters.Add("@IsGLP", SqlDbType.Bit).Value = isGLP; // New code 28 march 12
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }

                    objcmd.ExecuteNonQuery();
                    string strMsg = objcmd.Parameters["@msg"].Value.ToString();
                    objcon.Close();
                    sendmailtoclient(calNo, statusCode, statusDate, assignTo, comment, slCode, moduleCode, complCode, solutionCode, severityCode, comp_code, strFrontlineConnection);
                    return strMsg;

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


            // // Vipin 21 apr  string logBy, 

            public static string Update_Call_Category(string ProcName, string strConString, string calNo,
               DateTime CallDate, string CallCategoryFrom, string CallCategoryTo, string Created_By)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                objcon = new SqlConnection(strConString);
                try
                {
                    objcmd = new SqlCommand(ProcName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;


                    objcmd.Parameters.Add("@Call_No", SqlDbType.VarChar).Value = calNo;
                    objcmd.Parameters.Add("@Call_Date ", SqlDbType.DateTime).Value = CallDate.ToLongDateString();
                    objcmd.Parameters.Add("@CallCategoryFrom", SqlDbType.VarChar).Value = CallCategoryFrom; // New code 28 march 12
                    objcmd.Parameters.Add("@CallCategoryTo", SqlDbType.VarChar).Value = CallCategoryTo; // New code 28 march 12
                    objcmd.Parameters.Add("@Created_By", SqlDbType.VarChar).Value = Created_By;

                    objcmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                    objcmd.Parameters["@msg"].Direction = ParameterDirection.Output;

                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }

                    objcmd.ExecuteNonQuery();
                    string strMsg = objcmd.Parameters["@msg"].Value.ToString();
                    objcon.Close();
                    return strMsg;



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

            // END code

            public static string Open_Call_Info(string ProcName, string strConString, string calNo, string statusCode,
             string flag)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                objcon = new SqlConnection(strConString);
                try
                {

                    objcmd = new SqlCommand(ProcName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;

                    objcmd.Parameters.Add("@Call_No", SqlDbType.VarChar).Value = calNo;
                    objcmd.Parameters.Add("@Status_code ", SqlDbType.VarChar, 20).Value = statusCode;//statusDate.ToLongDateString();
                    objcmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                    objcmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }

                    objcmd.ExecuteNonQuery();
                    string strMsg = objcmd.Parameters["@msg"].Value.ToString();
                    //sendmailtoclient(calNo, statusCode, statusDate, assignTo, comment, slCode, moduleCode, complCode, solutionCode, severityCode, comp_code);
                    return strMsg;

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


            // New proced 18 June 14
            // procedure to check if records exist for provided severity criteria

            public static DataSet Get_SL_LEADS_MAIL_CRITERIA_CHECK(string procName, string strConString,
                string CallNo, string AccessParam, string SeverityParam)
            {
                DataSet objds;
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                objcon = new SqlConnection(strConString);

                try
                {
                    objcmd = new SqlCommand(procName, objcon);

                    objcmd.Parameters.Add("@Call_No", SqlDbType.VarChar).Value = CallNo;
                    objcmd.Parameters.Add("@AccessParam", SqlDbType.VarChar).Value = AccessParam;
                    objcmd.Parameters.Add("@SeverityParam", SqlDbType.VarChar).Value = SeverityParam;

                    objcmd.CommandType = CommandType.StoredProcedure;


                    objadp = new SqlDataAdapter(objcmd);
                    objds = new DataSet();
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }

                    objcmd.ExecuteNonQuery();
                    objadp.Fill(objds);
                    return objds;
                    //objcon.Close();
                    //objcmd.Dispose();
                    objcon.Dispose();



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

            public static DataSet Get_Call_History_Leads(string procName, string strConString, string CallNo)
            {
                DataSet objds;
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                objcon = new SqlConnection(strConString);

                try
                {
                    objcmd = new SqlCommand(procName, objcon);

                    objcmd.Parameters.Add("@Call_No", SqlDbType.VarChar).Value = CallNo;
                    objcmd.CommandType = CommandType.StoredProcedure;

                    objadp = new SqlDataAdapter(objcmd);
                    objds = new DataSet();
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }

                    objcmd.ExecuteNonQuery();
                    objadp.Fill(objds);
                    return objds;
                    //objcon.Close();
                    //objcmd.Dispose();
                    objcon.Dispose();

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

            public static void sendExceptionEmail(string excpetionMessage, string stackTrace)
            {
                System.Net.Mail.MailMessage message;

                using (System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient())
                {
                    smtp.Host = mailServer; //172.24.52.151
                    string subject, body;
                    string newCallLogExceptionEmail = Convert.ToString(ConfigurationManager.AppSettings["NewCallLogException"]);
                    subject = "Exception in IMS  New Login Required:";
                    body = "<br><b>************************</b><br>";
                    body = body + "<b>Message     : </b>" + excpetionMessage + "<br>";
                    body = body + "<b>StackTrace     : </b>" + stackTrace + "<br>";
                    message = new System.Net.Mail.MailMessage(mailFromAddress, newCallLogExceptionEmail, subject, body);
                    message.IsBodyHtml = true;
                    smtp.Send(message);
                }
            }

            //  ################################################### 19 jun 14

            public static void sendmail(string calNo, string logBy, string locCode, string deptCode,
            string UserEmail, string phone, string slCode, string moduleCode, string complCode, string remark,
            string assignTo, string comp_code, string PrmStrSeverity, string newUserSLHEmail, string newUserPMEmail)
            {
                try
                {
                    string from = mailFromAddress;

                    string SEMailID, subject, body;
                    string call_No;
                    //*************** Mail being Sent To Engineer
                    string Description = "", ServiceLine = "", Name = "", ComplaintType = "";
                    string severity_code = "", SEVERITY_Description = "", Module_Description = "";
                    string ClientName = "";
                    string Email_ID = "";
                    call_No = calNo;
                    string conn = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
                    string sql = "Select distinct Email_ID,Name From ENGINEERS_MASTER Where Engineer_Code = '" + assignTo + "'";
                    SqlDataSource dataSource = new SqlDataSource(conn, sql);

                    string SaveFileName = string.Empty; // Vipin 29 jun 14

                    DataView d = (DataView)dataSource.Select(new DataSourceSelectArguments());
                    DataTable table = d.Table;
                    if (table.Rows.Count > 0)
                    {
                        Email_ID = table.Rows[0]["Email_ID"].ToString();
                        Name = table.Rows[0]["Name"].ToString();
                    }

                    sql = "SELECT SEVERITY_MASTER.Severity_Code, SEVERITY_MASTER.Description,COMPLAINT_MASTER.Description as CompDescription FROM COMPLAINT_MASTER INNER JOIN SEVERITY_MASTER ON COMPLAINT_MASTER.Severity_Code = SEVERITY_MASTER.Severity_Code INNER JOIN COMPLAINT_MAPPING C ON C.COMPLAINT_CODE =COMPLAINT_MASTER.COMPLAINT_CODE WHERE     (C.Service_Line_Code = '" + slCode + "') AND (C.Module_Code = '" + moduleCode + "') AND (COMPLAINT_MASTER.Complaint_Code = '" + complCode + "') AND (C.COMPANY_ID='" + comp_code + "')";
                    dataSource = new SqlDataSource(conn, sql);
                    d = (DataView)dataSource.Select(new DataSourceSelectArguments());
                    DataTable table1 = d.Table;
                    if (table1.Rows.Count > 0)
                    {
                        SEVERITY_Description = table1.Rows[0]["Description"].ToString();
                        severity_code = table1.Rows[0]["Severity_Code"].ToString();
                        ComplaintType = table1.Rows[0]["CompDescription"].ToString();
                    }

                    sql = "SELECT Severity_Code, Description From SEVERITY_MASTER where  Severity_Code ='" + PrmStrSeverity + "'";
                    dataSource = new SqlDataSource(conn, sql);
                    d = (DataView)dataSource.Select(new DataSourceSelectArguments());
                    DataTable dttblSeverity = d.Table;
                    if (dttblSeverity.Rows.Count > 0)
                    {
                        SEVERITY_Description = dttblSeverity.Rows[0]["Description"].ToString();
                        severity_code = dttblSeverity.Rows[0]["Severity_Code"].ToString();
                    }

                    sql = "SELECT Description FROM Department_MASTER WHERE Department_code = '" + deptCode + "'";
                    dataSource = new SqlDataSource(conn, sql);
                    d = (DataView)dataSource.Select(new DataSourceSelectArguments());
                    DataTable table2 = d.Table;
                    if (table2.Rows.Count > 0)
                    {
                        Description = table2.Rows[0]["Description"].ToString();
                    }

                    sql = "SELECT Description FROM service_master WHERE Service_Line_Code = '" + slCode + "'";// AND COMPANYID='" + comp_code + "'";
                    dataSource = new SqlDataSource(conn, sql);
                    d = (DataView)dataSource.Select(new DataSourceSelectArguments());
                    DataTable table3 = d.Table;
                    if (table3.Rows.Count > 0)
                    {
                        ServiceLine = table3.Rows[0]["Description"].ToString();
                    }

                    sql = "SELECT Module_Description FROM service_line_module WHERE Module_Code = '" + moduleCode + "' AND SERVICE_LINE_CODE='" + slCode + "' AND COMPANYID='" + comp_code + "'";
                    dataSource = new SqlDataSource(conn, sql);
                    d = (DataView)dataSource.Select(new DataSourceSelectArguments());
                    DataTable table4 = d.Table;
                    if (table4.Rows.Count > 0)
                    {
                        Module_Description = table4.Rows[0]["Module_Description"].ToString();
                    }

                    sql = "SELECT Name FROM client_master WHERE emp_code = '" + logBy + "'";
                    dataSource = new SqlDataSource(conn, sql);
                    d = (DataView)dataSource.Select(new DataSourceSelectArguments());
                    DataTable table5 = d.Table;
                    if (table5.Rows.Count > 0)
                    {
                        ClientName = table5.Rows[0]["Name"].ToString();
                    }

                    subject = "IMS Call No. : " + calNo;
                    body = "<br><b>********CALL DETAILS****************</b><br>";
                    body = body + "<b>Call No.     : </b>" + calNo + "<br>";
                    body = body + "<b>Logged Date     : </b>" + DateTime.Now.ToString();
                    body = body + "<br><b>Logged By       : </b>" + ClientName;
                    body = body + "<br><b>User Email : </b>" + UserEmail;
                    body = body + "<br><b>Phone/Ext No.:</b> " + phone;
                    body = body + "<br><b>Department   :</b> " + Description;
                    body = body + "<br><b>***********COMPLAINT DETAILS********************</b>";
                    body = body + "<br><b>Service Line : </b>" + ServiceLine;
                    body = body + "<br><b>Severity    :</b> " + SEVERITY_Description;
                    body = body + "<br><b>Assigned To :</b> " + Name;
                    body = body + "<br><b>Complaint Category :</b> " + Module_Description;
                    body = body + "<br><b>Complaint Type :</b> " + ComplaintType;
                    body = body + "<br><b>Case Detail :</b> " + remark;

                    SEMailID = Email_ID;


                    if (SEMailID.Contains("@"))
                    {
                        SendEmail(SEMailID, subject, body);
                    }
                    if (UserEmail.Contains("@"))
                    {
                        SendEmail(UserEmail, subject, body);
                    }
                    //else
                    //{
                    //    InsertToRmail(UserEmail, subject, body);
                    //}

                    if (!string.IsNullOrEmpty(newUserSLHEmail))
                    {
                        SendEmail(newUserSLHEmail, subject, body);
                        SendEmail(newUserPMEmail, subject, body);
                    }
                    // // #######################################################################################
                    // Code to send mail to Leads for critical / Fatal Calls
                    // Vipin 17.06.14

                    // Retrieve paramters to check if the call details are to be mailed to leads

                    string AccessParam = string.Empty, SeverityParam = string.Empty;

                    sql = "  Select StrParamCode as  AccessParam ,StrParamCode1 as  SeverityParam " +
                          "   From param_master WHERE PURPOSEINIT ='LEADALERT'";

                    dataSource = new SqlDataSource(conn, sql);
                    d = (DataView)dataSource.Select(new DataSourceSelectArguments());
                    DataTable table8 = d.Table;
                    if (table8.Rows.Count > 0)
                    {
                        AccessParam = table8.Rows[0]["AccessParam"].ToString();
                        SeverityParam = table8.Rows[0]["SeverityParam"].ToString();



                        //   if (severity_code == "1" || severity_code == "2")
                        DataSet dsSeverityRecods = new DataSet();
                        string strCon = ConfigurationManager.ConnectionStrings["ConStr"].ToString();

                        dsSeverityRecods = Get_SL_LEADS_MAIL_CRITERIA_CHECK("USP_SL_LEADS_MAIL_CRITERIA_CHECK", strCon, calNo, AccessParam, SeverityParam);
                        if (dsSeverityRecods.Tables[0].Rows.Count > 0)
                        {
                            String toLead = string.Empty, ccUnitLead = string.Empty, subjectLead = string.Empty, bodyLead = string.Empty;

                            sql = " Select Distinct sl.service_line_code , sl.service_line_head , sl.servicelinehead_email " +
                                  " From service_line sl INNER JOIN User_IDENTITY UI ON UI.User_ID = sl.service_line_head_id where sl.active = 1 and sl.service_line_head_id <>'super' AND UI.User_Type = 'SLH' " +
                                  " and service_line_code ='" + slCode + "' " +
                                  " Order by sl.servicelinehead_email ";

                            dataSource = new SqlDataSource(conn, sql);
                            d = (DataView)dataSource.Select(new DataSourceSelectArguments());
                            DataTable table6 = d.Table;
                            if (table6.Rows.Count > 0)
                            {
                                for (int count = 0; count < table6.Rows.Count; count++)
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(table6.Rows[count]["servicelinehead_email"])))
                                        toLead = toLead + Convert.ToString(table6.Rows[count]["servicelinehead_email"]) + ",";
                                }

                                if (!string.IsNullOrEmpty(toLead))
                                {
                                    toLead = toLead.TrimEnd(',');
                                }

                            }

                            sql = " SELECT   email_id , name FROM user_identity WHERE user_type ='ul'";

                            dataSource = new SqlDataSource(conn, sql);
                            d = (DataView)dataSource.Select(new DataSourceSelectArguments());
                            DataTable table7 = d.Table;
                            if (table7.Rows.Count > 0)
                            {
                                for (int count = 0; count < table7.Rows.Count; count++)
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(table7.Rows[count]["email_id"])))
                                        ccUnitLead = Convert.ToString(table7.Rows[count]["email_id"]) + ccUnitLead + ",";
                                }

                                if (!string.IsNullOrEmpty(ccUnitLead))
                                {
                                    ccUnitLead = ccUnitLead.TrimEnd(',');
                                }
                            }

                            // ## 
                            subjectLead = "IMS Ticket Alert : IMS ticket Logged under " + SEVERITY_Description + " severity " + " ( SL: " + ServiceLine + " | MO: " + Module_Description + " ) ";

                            string OnDate = DateTime.Now.ToShortDateString();


                            bodyLead = "<div style='font-family:Century Gothic;font-size:14px'>" + "Hi, <br/><br/>";
                            bodyLead = bodyLead + "This is to inform you that IMS Ticket number  " + calNo;
                            bodyLead = bodyLead + " is logged under " + SEVERITY_Description + " severity on " + OnDate + " at " + DateTime.Now.ToString("h:mm:ss tt") + ".<br/> Pls. find details below –<br/><br/> </div> ";

                            bodyLead = bodyLead + "<div style='font-family:Century Gothic;font-size:14px'>";
                            bodyLead = bodyLead + "<table style='font-size:14px;font-family:Century Gothic' width='100%'><tr><td width='20%'> ";
                            bodyLead = bodyLead + "Call No.  :  </td><td width='80%'> " + calNo + "</td></tr><tr><td>";
                            //   bodyLead = bodyLead + "<b>Logged Date  : </b></td><td>" + DateTime.Now.ToString() + "</td></tr><tr><td>"; ;
                            bodyLead = bodyLead + "Logged Date and Time  : </td><td>" + OnDate + " " + DateTime.Now.ToString("h:mm:ss tt") + "</td></tr><tr><td>"; ;
                            bodyLead = bodyLead + "Logged By  : </td><td>" + ClientName + "</td></tr><tr><td>"; ;
                            bodyLead = bodyLead + "User Email :</td><td>" + UserEmail + "</td></tr><tr><td>"; ;
                            bodyLead = bodyLead + "Phone/Ext No.  :</td><td>" + phone + "</td></tr><tr><td>"; ;
                            bodyLead = bodyLead + "Department   : </td><td>" + Description + "</td></tr><tr><td colspan='2'>&nbsp;</td></tr><tr><td colspan='2' >";
                            bodyLead = bodyLead + "<b>COMPLAINT DETAILS</b>" + "</td></tr><tr><td>";
                            bodyLead = bodyLead + "Service Line     </td><td> " + ServiceLine + "</td></tr><tr><td>";
                            bodyLead = bodyLead + "Module : </td><td>  " + Module_Description + "</td></tr><tr><td>";

                            if (SEVERITY_Description == "Fatal" || SEVERITY_Description == "Critical")
                            {
                                bodyLead = bodyLead + "Severity   : </td><td>  <b>" + SEVERITY_Description + "</b></td></tr><tr><td>";
                            }
                            else
                            {
                                bodyLead = bodyLead + "Severity   : </td><td>  " + SEVERITY_Description + "</td></tr><tr><td>";
                            }
                            bodyLead = bodyLead + "Assigned To :  </td><td> " + Name + "</td></tr><tr><td>";
                            bodyLead = bodyLead + "Complaint Type : </td><td>  " + ComplaintType + "</td></tr><tr><td>";
                            bodyLead = bodyLead + "Problem Description :  </td><td> " + remark + "</td></tr></table></div>";

                            bodyLead = bodyLead + "<div style='font-family:Century Gothic;font-size:14px'>";
                            bodyLead = bodyLead + "<br/> Thanks, <br/> ";
                            bodyLead = bodyLead + "FMS Helpdesk <br/><br/> ";
                            bodyLead = bodyLead + "<b> ***  Note: This is a system generated email message, Pl donot reply to it. For more details, please get in touch with FMS Lead OR Service Line Head **** </b> </div>";

                            sql = "select * from call_booking where Call_no = '" + calNo + "'";
                            dataSource = new SqlDataSource(conn, sql);
                            d = (DataView)dataSource.Select(new DataSourceSelectArguments());
                            DataTable table9 = d.Table;
                            if (table9.Rows.Count > 0)
                            {
                                SaveFileName = table9.Rows[0]["SavedFileName"].ToString();
                            }


                            SendEmail(toLead, subjectLead, bodyLead, ccUnitLead);
                        }

                    } // end fetch mail sending param

                }
                catch (Exception ex)
                {

                    throw new Exception("Please contact to Administrator. " + ex.Message);
                }
            }

            private static void smtpClient_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
            {

                string state = e.UserState as string;

                if (e.Error != null)
                {
                    Utility3.General.LogExcpetion(Convert.ToString("Error in Sending Email Error is" + e.Error + Environment.NewLine + "object State is:" + state));
                    sendExceptionEmail("Error in Sending Email Error is" + e.Error, "object State is:" + state);
                }

                if (e.Cancelled)
                {
                    Utility3.General.LogExcpetion(Convert.ToString("Email send event is cancelled by user." + Environment.NewLine + "object State is:" + state));
                    sendExceptionEmail("Email send event is cancelled by user.", "object State is:" + state);
                }

            }

            public static void SendEmail(string emailId, string subject, string body, string ccEmail = "")
            {
                if (!string.IsNullOrEmpty(emailId))
                {
                    string from = mailFromAddress;
                    MailMessage message = new MailMessage(from, emailId, subject, body);

                    if (!string.IsNullOrEmpty(ccEmail))
                    {
                        message.CC.Add(ccEmail);
                    }

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = mailServer;
                    smtp.Credentials = new System.Net.NetworkCredential(SMTPUserName, SMTPPassword);
                    message.IsBodyHtml = true;
                    Object smtpState = "To:" + Convert.ToString(message.To) + ", From:" + message.From + ", CC:" + Convert.ToString(message.CC);
                    smtp.SendCompleted += new SendCompletedEventHandler(smtpClient_SendCompleted);
                    smtp.SendAsync(message, smtpState);
                }
            }

            public static void SendCustomerReminderEmail(string userName, string CallNo, string emailId, string subject)
            {
                string mailBody = string.Empty;

                if (userName.IndexOf("(") > 0)
                    userName = userName.Substring(0, userName.IndexOf("("));

                mailBody = "Dear " + userName + "," + "<br /><br />  Reference:  <b>" + CallNo + ". </b>  <br /><br /> We seek your inputs for " + "\"CUSTOMER ACTION\"" + ", for an above-mentioned ticket. <br /> Request to you select, the option for closing the loop. <br /> Feel free to have further assistance from Helpdesk. <br /><br /> The ticket shall automatically CLOSE if not responded within three (3) days. <br /><br /> Regards, <br /> <strong> Incident Management Support and Services. </strong>";

                SendEmail(emailId, subject, mailBody);
            }
            public static void sendmailtoclient(string calNo, string statusCode, DateTime statusDate,
                 string assignTo, string comment, string slCode, string moduleCode, string complCode,
                 string solutionCode, string severityCode, string comp_code, string fronlineConnection)
            {
                try
                {

                    string from = mailFromAddress; //"Zydusmail-noreply@zyduscadila.com";
                                                   // send SMTP mail


                    //*************** Mail being Sent To Engineer
                    string call_No;
                    string locCode = "", deptCode = "", email = "", phone = "", remark = "";
                    string logBy = "";
                    string call_date = "";
                    string Description = "", ServiceLine = "", Name = "", ComplaintType = "";
                    string severity_code = "", SEVERITY_Description = "", Module_Description = "";
                    string ClientName = "", EngComments = "";
                    string engg_email = "", Status = "", Solution = "";
                    String client_email = "";
                    string conn = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
                    string SaveFileName = string.Empty; // Vipin 29 jun 14
                    bool needToSendEmailToClient = false;
                    string body = string.Empty;
                    string userMobile = string.Empty;
                    string engMobile = string.Empty;
                    bool isCloseCallSMSEnable = false;
                    bool isFeedbackEnable = false;
                    bool isFMEngineer = false;
                    call_No = calNo;
                    string surveyBody = string.Empty;
                    string endUserName = string.Empty;
                    //fetch the call information
                    string sql = "select CB.*,LM.IsFeedback from call_booking CB INNER JOIN Location_Master LM ON LM.location_Code = CB.location_code where Call_no = '" + calNo + "'";
                    SqlDataSource dataSource = new SqlDataSource(conn, sql);

                    DataView d = (DataView)dataSource.Select(new DataSourceSelectArguments());
                    DataTable table6 = d.Table;
                    if (table6.Rows.Count > 0)
                    {
                        locCode = table6.Rows[0]["Location_Code"].ToString();
                        deptCode = table6.Rows[0]["Department_Code"].ToString();
                        phone = table6.Rows[0]["Phone_No"].ToString();
                        remark = table6.Rows[0]["Remarks"].ToString();
                        logBy = table6.Rows[0]["Emp_Code"].ToString();
                        call_date = table6.Rows[0]["call_date"].ToString();
                        client_email = table6.Rows[0]["Email_ID"].ToString();
                        SaveFileName = table6.Rows[0]["SavedFileName"].ToString();
                        isFeedbackEnable = Convert.ToBoolean(table6.Rows[0]["IsFeedback"]);
                    }

                    sql = "Select distinct Email_ID,Name,Mobile_No From ENGINEERS_MASTER Where Engineer_Code = '" + assignTo + "'";
                    dataSource = new SqlDataSource(conn, sql);
                    d = (DataView)dataSource.Select(new DataSourceSelectArguments());
                    DataTable table = d.Table;
                    if (table.Rows.Count > 0)
                    {
                        engg_email = table.Rows[0]["Email_ID"].ToString();
                        Name = table.Rows[0]["Name"].ToString();
                        engMobile = Convert.ToString(table.Rows[0]["Mobile_No"]);
                    }


                    sql = "SELECT SEVERITY_MASTER.Severity_Code, SEVERITY_MASTER.Description,COMPLAINT_MASTER.Description as CompDescription FROM COMPLAINT_MASTER INNER JOIN SEVERITY_MASTER ON COMPLAINT_MASTER.Severity_Code = SEVERITY_MASTER.Severity_Code INNER JOIN COMPLAINT_MAPPING C ON C.COMPLAINT_CODE =COMPLAINT_MASTER.COMPLAINT_CODE WHERE     (C.Service_Line_Code = '" + slCode + "') AND (C.Module_Code = '" + moduleCode + "') AND (COMPLAINT_MASTER.Complaint_Code = '" + complCode + "') AND (C.COMPANY_ID='" + comp_code + "')";
                    dataSource = new SqlDataSource(conn, sql);
                    d = (DataView)dataSource.Select(new DataSourceSelectArguments());
                    DataTable table1 = d.Table;
                    if (table1.Rows.Count > 0)
                    {
                        SEVERITY_Description = table1.Rows[0]["Description"].ToString();
                        severity_code = table1.Rows[0]["Severity_Code"].ToString();
                        ComplaintType = table1.Rows[0]["CompDescription"].ToString();
                    }

                    sql = "SELECT Severity_Code, Description From SEVERITY_MASTER where  Severity_Code ='" + severityCode + "'";
                    dataSource = new SqlDataSource(conn, sql);
                    d = (DataView)dataSource.Select(new DataSourceSelectArguments());
                    DataTable dttblSeverity = d.Table;
                    if (dttblSeverity.Rows.Count > 0)
                    {
                        SEVERITY_Description = dttblSeverity.Rows[0]["Description"].ToString();
                        severity_code = dttblSeverity.Rows[0]["Severity_Code"].ToString();
                    }


                    sql = "SELECT Description FROM Department_MASTER WHERE Department_code = '" + deptCode + "'";
                    dataSource = new SqlDataSource(conn, sql);
                    d = (DataView)dataSource.Select(new DataSourceSelectArguments());
                    DataTable table2 = d.Table;
                    if (table2.Rows.Count > 0)
                    {
                        Description = table2.Rows[0]["Description"].ToString();
                    }

                    sql = "SELECT Description FROM service_master WHERE Service_Line_Code = '" + slCode + "'";// and companyid='" + comp_code + "'";
                    dataSource = new SqlDataSource(conn, sql);
                    d = (DataView)dataSource.Select(new DataSourceSelectArguments());
                    DataTable table3 = d.Table;
                    if (table3.Rows.Count > 0)
                    {
                        ServiceLine = table3.Rows[0]["Description"].ToString();
                    }

                    sql = "SELECT Module_Description FROM service_line_module WHERE Module_Code = '" + moduleCode + "' AND Service_Line_Code = '" + slCode + "' and companyid='" + comp_code + "'";
                    dataSource = new SqlDataSource(conn, sql);
                    d = (DataView)dataSource.Select(new DataSourceSelectArguments());
                    DataTable table4 = d.Table;
                    if (table4.Rows.Count > 0)
                    {
                        Module_Description = table4.Rows[0]["Module_Description"].ToString();
                    }

                    sql = "SELECT Name,eMail_ID,MobileNo,IsSMSEnable FROM client_master WHERE Emp_Code = '" + logBy + "'";
                    dataSource = new SqlDataSource(conn, sql);
                    d = (DataView)dataSource.Select(new DataSourceSelectArguments());
                    DataTable table5 = d.Table;
                    if (table5.Rows.Count > 0)
                    {
                        endUserName = ClientName = table5.Rows[0]["Name"].ToString();
                        email = table5.Rows[0]["eMail_ID"].ToString();
                        userMobile = Convert.ToString(table5.Rows[0]["MobileNo"]);
                        isCloseCallSMSEnable = Convert.ToBoolean(table5.Rows[0]["IsSMSEnable"]);
                    }

                    sql = "SELECT Comments FROM Call_booking_activity_log where Call_no = '" + calNo + "' And Status_Code='" + statusCode + "'";
                    dataSource = new SqlDataSource(conn, sql);
                    d = (DataView)dataSource.Select(new DataSourceSelectArguments());
                    DataTable table7 = d.Table;
                    if (table7.Rows.Count > 0)
                    {
                        EngComments = table7.Rows[0]["Comments"].ToString();
                    }

                    sql = "SELECT Status,ForEmail FROM Status_master where Status_Code='" + statusCode + "'";
                    dataSource = new SqlDataSource(conn, sql);
                    d = (DataView)dataSource.Select(new DataSourceSelectArguments());
                    DataTable table8 = d.Table;
                    if (table8.Rows.Count > 0)
                    {
                        Status = table8.Rows[0]["Status"].ToString();
                        needToSendEmailToClient = Convert.ToBoolean(table8.Rows[0]["ForEmail"]);
                    }

                    sql = "select description from solution_master where Solution_Code = '" + solutionCode + "'";
                    dataSource = new SqlDataSource(conn, sql);
                    d = (DataView)dataSource.Select(new DataSourceSelectArguments());
                    DataTable table9 = d.Table;
                    if (table9.Rows.Count > 0)
                    {
                        Solution = table9.Rows[0]["description"].ToString();
                    }

                    sql = "SELECT USER_GRP from USER_IDENTITY where Engg_Code = '" + assignTo + "'";
                    dataSource = new SqlDataSource(conn, sql);
                    d = (DataView)dataSource.Select(new DataSourceSelectArguments());
                    DataTable userGRP = d.Table;
                    if (userGRP.Rows.Count > 0)
                    {
                        isFMEngineer = Convert.ToString(userGRP.Rows[0]["USER_GRP"]).Equals("FM", StringComparison.CurrentCultureIgnoreCase);
                    }

                    string subject = "IMS Call No. : " + calNo;
                    ///If status code is completed and survey enable for location then link in an email, check call is closed by FMS engineer.
                    if (statusCode == "1" && isFeedbackEnable && slCode.Equals("F", StringComparison.CurrentCultureIgnoreCase) && isFMEngineer && !logBy.Equals("ZYIMSADMIN", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (endUserName.IndexOf("(") > 0)
                            endUserName = endUserName.Substring(0, endUserName.IndexOf("("));

                        surveyBody = feedbackEmailBody.Replace("[[USERNAME]]", endUserName)
                                                             .Replace("[[COMPLAINT]]", remark)
                                                             .Replace("[[EXPIRAYDATE]]", DateTime.Now.AddDays(expiryDays).ToString("dddd, MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("en-US")))
                                                             .Replace("[[URL]]", applicationURL + "Transaction/UserFeedback.aspx?ID=" + Utility2.EncryptString(call_No));


                    }

                    body = body + "<br><b>********CALL DETAILS***********</b><br>";
                    body = body + "<b>Call No.     : </b>" + calNo + "<br>";
                    body = body + "<b>Log Date     : </b>" + call_date;
                    body = body + "<br><b>Log By       : </b>" + ClientName;
                    body = body + "<br><b>User Email : </b>" + client_email;
                    body = body + "<br><b>Phone/Ext No.:</b> " + phone;
                    body = body + "<br><b>Department   :</b> " + Description;
                    body = body + "<br><b>********COMPLAINT DETAILS*********</b>";
                    body = body + "<br><b>Service Line : </b>" + ServiceLine;
                    body = body + "<br><b>Severity    :</b> " + SEVERITY_Description;
                    body = body + "<br><b>Assigned To :</b> " + Name;
                    body = body + "<br><b>Complaint Category :</b> " + Module_Description;
                    body = body + "<br><b>Complaint Type :</b> " + ComplaintType;
                    body = body + "<br><b>Case Detail :</b> " + remark;
                    body = body + "<br><b>*******CALL STATUS************</b>";
                    body = body + "<br><b>Status Date     : </b>" + DateTime.Now.ToString();
                    body = body + "<br><b>Call Status :</b> " + Status;
                    body = body + "<br><b>Solution :</b> " + Solution;
                    body = body + "<br><b>Comments :</b> " + EngComments;
                    body = body + "<br><b>***************************************</b>";


                    ///Check call Should not be logged by ZYIMSADMIN i.e.: Ignore call related to Ascia.
                    if (isFMEngineer && !logBy.Equals("ZYIMSADMIN", StringComparison.CurrentCultureIgnoreCase))
                        SendSMS(calNo, ClientName, userMobile, Convert.ToDateTime(call_date), engMobile, Name, statusCode, slCode, isCloseCallSMSEnable, fronlineConnection);


                    //SMTP mail send to Engg
                    if (engg_email.Contains("@"))
                    {
                        SendEmail(engg_email, subject, body);
                    }

                    if (client_email.Contains("@") && needToSendEmailToClient)
                    {
                        SendEmail(client_email, subject, body);

                        ///Check if feedback email i enable then send it.
                        if (!string.IsNullOrEmpty(surveyBody))
                        {
                            SendEmail(client_email, subject, surveyBody);
                        }
                    }
                    //else
                    //{
                    //    InsertToRmail(client_email, subject, body);

                    //}


                    // // ####################################################################################### ~
                    // Code to send mail to Leads for critical / Fatal Calls
                    // Vipin 17.06.14

                    string AccessParam = string.Empty, SeverityParam = string.Empty;

                    sql = "  Select StrParamCode as  AccessParam ,StrParamCode1 as  SeverityParam " +
                          "   From param_master WHERE PURPOSEINIT ='LEADALERT'";

                    dataSource = new SqlDataSource(conn, sql);
                    d = (DataView)dataSource.Select(new DataSourceSelectArguments());
                    DataTable table11 = d.Table;
                    if (table11.Rows.Count > 0)
                    {
                        AccessParam = table11.Rows[0]["AccessParam"].ToString();
                        SeverityParam = table11.Rows[0]["SeverityParam"].ToString();


                        //   if (severity_code == "1" || severity_code == "2")
                        // Check if the call fullfill the criteria to send mail to leads

                        DataSet dsSeverityRecods = new DataSet();
                        string strCon = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
                        dsSeverityRecods = Get_SL_LEADS_MAIL_CRITERIA_CHECK("USP_SL_LEADS_MAIL_CRITERIA_CHECK", strCon, calNo, AccessParam, SeverityParam);

                        if (dsSeverityRecods.Tables[0].Rows.Count > 0)
                        {

                            String toLead = string.Empty, ccUnitLead = string.Empty, subjectLead = string.Empty, bodyLead = string.Empty;

                            sql = " Select Distinct sl.service_line_code , sl.service_line_head , sl.servicelinehead_email " +
                                  " From service_line sl INNER JOIN User_IDENTITY UI ON UI.User_ID = sl.service_line_head_id where sl.active = 1 and sl.service_line_head_id <>'super' AND UI.User_Type = 'SLH' " +
                                  " and service_line_code ='" + slCode + "' " +
                                  " Order by sl.servicelinehead_email ";

                            dataSource = new SqlDataSource(conn, sql);
                            d = (DataView)dataSource.Select(new DataSourceSelectArguments());
                            DataTable table10 = d.Table;
                            if (table10.Rows.Count > 0)
                            {
                                for (int count = 0; count < table10.Rows.Count; count++)
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(table10.Rows[count]["servicelinehead_email"])))
                                        toLead = toLead + Convert.ToString(table10.Rows[count]["servicelinehead_email"]) + ",";
                                }

                                if (!string.IsNullOrEmpty(toLead))
                                {
                                    toLead = toLead.TrimEnd(',');
                                }

                            }

                            sql = " SELECT email_id , name FROM user_identity WHERE user_type ='ul'";

                            dataSource = new SqlDataSource(conn, sql);
                            d = (DataView)dataSource.Select(new DataSourceSelectArguments());
                            DataTable table12 = d.Table;
                            if (table12.Rows.Count > 0)
                            {
                                for (int count = 0; count < table12.Rows.Count; count++)
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(table12.Rows[count]["email_id"])))
                                        ccUnitLead = Convert.ToString(table12.Rows[count]["email_id"]) + ccUnitLead + ",";
                                }

                                if (!string.IsNullOrEmpty(ccUnitLead))
                                {
                                    ccUnitLead = ccUnitLead.TrimEnd(',');
                                }
                            }

                            // Fetch Original Severity
                            string originalSeverity = string.Empty;

                            sql = " select S.description from call_booking C inner join  " +
                                  " severity_master S on C.original_Severity =  S.severity_code " +
                                  " where C.call_no ='" + calNo + "'";

                            dataSource = new SqlDataSource(conn, sql);
                            d = (DataView)dataSource.Select(new DataSourceSelectArguments());
                            DataTable table13 = d.Table;
                            if (table13.Rows.Count > 0)
                            {
                                originalSeverity = table13.Rows[0]["description"].ToString();
                            }

                            subjectLead = "IMS Ticket Alert : IMS ticket attended by engineer " + " ( SL: " + ServiceLine + " | MO: " + Module_Description + " ) ";

                            string CurrentStatusDate = DateTime.Now.ToShortDateString();

                            bodyLead = "<div style='font-family:Century Gothic;font-size:14px'>" + "Hi, <br/><br/>";
                            bodyLead = bodyLead + "This is to inform that the IMS call number " + calNo + " booked under " + originalSeverity + " severity ";
                            bodyLead = bodyLead + "have been attended on  " + CurrentStatusDate + " at " + DateTime.Now.ToString("h:mm:ss tt") + " <br/>";
                            //call_date DateTime.Now.ToString()
                            bodyLead = bodyLead + " Pls. find details below –  <br/><br/></div> ";
                            bodyLead = bodyLead + "<div style='font-family:Century Gothic;font-size:14px'>";
                            bodyLead = bodyLead + "<table style='font-size:14px;font-family:Century Gothic' width='100%'><tr><td width='20%'> ";
                            bodyLead = bodyLead + "Call No.  : </td><td width='80%'> " + calNo + "</td></tr><tr><td>";
                            bodyLead = bodyLead + "Logged Date and Time : </td><td>" + call_date + "</td></tr><tr><td>"; ;
                            bodyLead = bodyLead + "Logged By  : </td><td>" + ClientName + "</td></tr><tr><td>"; ;
                            bodyLead = bodyLead + "User Email : </td><td>" + client_email + "</td></tr><tr><td>"; ;
                            bodyLead = bodyLead + "Phone/Ext No.  :</td><td>" + phone + "</td></tr><tr><td>"; ;
                            bodyLead = bodyLead + "Department   : </td><td>" + Description + "</td></tr><tr><td colspan='2'>&nbsp;</td></tr><tr><td colspan='2' >";
                            bodyLead = bodyLead + "<b>COMPLAINT DETAILS</b>" + "</td></tr><tr><td>";
                            bodyLead = bodyLead + "Service Line    </td><td> " + ServiceLine + "</td></tr><tr><td>";
                            bodyLead = bodyLead + "Module : </td><td>  " + Module_Description + "</td></tr><tr><td>";

                            if (SEVERITY_Description == "Fatal" || SEVERITY_Description == "Critical")
                            {
                                bodyLead = bodyLead + "Severity   : </td><td>  <b>" + SEVERITY_Description + "</b></td></tr><tr><td>";
                            }
                            else
                            {
                                bodyLead = bodyLead + "Severity   : </td><td>  " + SEVERITY_Description + "</td></tr><tr><td>";
                            }

                            bodyLead = bodyLead + "Assigned To :  </td><td> " + Name + "</td></tr><tr><td>";
                            bodyLead = bodyLead + "Complaint Type : </td><td>  " + ComplaintType + "</td></tr><tr><td>";
                            bodyLead = bodyLead + "Problem Description :  </td><td> " + remark + "</td></tr><tr><td colspan='2' >";

                            //bodyLead = bodyLead + "<b>UPDATES on " + CurrentStatusDate + " " + DateTime.Now.ToString("h:mm:ss tt") + "</b></td></tr><tr><td>";
                            //bodyLead = bodyLead + "<b>Status Date  : </b></td><td>" + CurrentStatusDate + " " + DateTime.Now.ToString("h:mm:ss tt") + "</td></tr><tr><td>";
                            //bodyLead = bodyLead + "<b>Call Status :</b></td><td> " + Status + "</td></tr><tr><td>";
                            //bodyLead = bodyLead + "<b>Solution :</b> </td><td>" + Solution + "</td></tr><tr><td>";
                            //bodyLead = bodyLead + "<b>Engineer’s Comments  :</b> </td><td>" + EngComments + "<br></td></tr>";

                            //// if call is atteneded earlier then display history                       
                            //DataSet dsCallHistory = new DataSet();
                            //dsCallHistory = Get_Call_History_Leads("usp_Call_log_history_Mail", strCon, calNo);

                            //string prevSeverity = string.Empty, currentSeverity = string.Empty, nextSeverity = string.Empty;


                            //if (dsCallHistory.Tables[0].Rows.Count > 0)
                            //{
                            //    //  bodyLead = bodyLead + "<tr><td></td><td></td></tr>";
                            //    bodyLead = bodyLead + "</td><td></td></tr>";

                            //    //if (dsCallHistory.Tables[0].Rows.Count > 1)
                            //    //   prevSeverity = dsCallHistory.Tables[0].Rows[1]["Severity"].ToString();
                            //    //else
                            //    //    prevSeverity = dsCallHistory.Tables[0].Rows[0]["Severity"].ToString();

                            //    for (int i = 0; i < dsCallHistory.Tables[0].Rows.Count; i++)
                            //    {
                            //        string statusDateHistory = dsCallHistory.Tables[0].Rows[i]["Status_date"].ToString();
                            //        currentSeverity = dsCallHistory.Tables[0].Rows[i]["Severity"].ToString();
                            //        //CallDateDMY = Convert.ToDateTime(statusDateHistory, dateFormat).ToShortDateString();
                            //       // CallDateDMY = Convert.ToDateTime(statusDateHistory).ToShortDateString();
                            //       // CallTimeHHMMSS = Convert.ToDateTime(statusDateHistory, dateFormat).ToString("h:mm:ss tt");

                            //        if (i < dsCallHistory.Tables[0].Rows.Count - 1)
                            //            nextSeverity = dsCallHistory.Tables[0].Rows[i + 1]["Severity"].ToString();


                            //        //if (currentSeverity == prevSeverity )
                            //        //    bodyLead = bodyLead + "<tr><td colspan='2'><br><b>UPDATES on" + CallDateDMY + " " + CallTimeHHMMSS + " ( Solution updates ) </b></td></tr><tr><td>";
                            //        //else
                            //        //    bodyLead = bodyLead + "<tr><td colspan='2'><br><b>UPDATES on" + CallDateDMY + " " + CallTimeHHMMSS + " ( Severity changed ) </b></td></tr><tr><td>";

                            //        if (currentSeverity == nextSeverity)
                            //            bodyLead = bodyLead + " <tr><td colspan='2'><br><b>UPDATES on " + statusDateHistory + " ( Solution updates ) </b></td></tr><tr><td>";
                            //        else
                            //            bodyLead = bodyLead + "<tr><td colspan='2'><br><b>UPDATES on " + statusDateHistory + " ( Severity changed ) </b></td></tr><tr><td>";


                            //        //  bodyLead = bodyLead + "<b>Status Date  : </b></td><td>" + dsCallHistory.Tables[0].Rows[i]["Status_date"].ToString() + "</td></tr><tr><td>";
                            //        if (currentSeverity == "Fatal" || currentSeverity == "Critical")
                            //            bodyLead = bodyLead + "Severity :</td><td> <b>" + currentSeverity + "</b></td></tr><tr><td>";
                            //        else
                            //            bodyLead = bodyLead + "Severity :</td><td> " + currentSeverity + "</td></tr><tr><td>";

                            //        bodyLead = bodyLead + "Call Status :</td><td> " + dsCallHistory.Tables[0].Rows[i]["Status"].ToString() + "</td></tr><tr><td>";
                            //        bodyLead = bodyLead + "Solution : </td><td>" + dsCallHistory.Tables[0].Rows[i]["Solution"].ToString() + "</td></tr><tr><td>";
                            //        bodyLead = bodyLead + "Engineer’s Comments  : </td><td>" + dsCallHistory.Tables[0].Rows[i]["Comments"].ToString() + "<br/></td></tr><tr><td>";
                            //        //bodyLead = bodyLead + " </td><td> &nbsp; <br/></td></tr><tr><td>";

                            //        bodyLead = bodyLead + " </td><td> &nbsp; <br/></td></tr>";
                            //        prevSeverity = dsCallHistory.Tables[0].Rows[i]["Severity"].ToString();

                            //    }

                            //    bodyLead = bodyLead + "<tr><td><br/></td><td></td></tr>";
                            //}

                            bodyLead = bodyLead + "</table></div>";

                            bodyLead = bodyLead + "<div style='font-family:Century Gothic;font-size:14px'>";
                            bodyLead = bodyLead + "<br/> Thanks, <br/> ";
                            bodyLead = bodyLead + "FMS Helpdesk <br/><br/> ";
                            bodyLead = bodyLead + "<b> ***  Note: This is a system generated email message, Pl donot reply to it. For more details, please get in touch with FMS Lead OR Service Line Head **** </b> </div>";

                            SendEmail(toLead, subjectLead, bodyLead, ccUnitLead);
                        }
                    } // end serevity criteria mail
                }
                catch (Exception)
                {
                    throw new Exception("Please contact to Administrator.");
                }
            }
            public static void InsertSMSEntry(string mobileNo, string messageBody, string connection)
            {
                OleDbConnection cn = new OleDbConnection(connection);
                OleDbCommand OleDbCommand;
                OleDbTransaction trans = null;
                try
                {
                    cn.Open();

                    OleDbCommand = new OleDbCommand();
                    OleDbCommand.Connection = cn;
                    OleDbCommand.CommandTimeout = 120;
                    OleDbCommand.Transaction = trans;
                    OleDbCommand.CommandText = "INSERT INTO frontora.sms (MOBILENO, MESSAGE,SMSGROUP, USER_DATE) VALUES (?, ?,'IMS', SYSDATE)";
                    OleDbCommand.CommandType = CommandType.Text;
                    OleDbParameter[] param = { new OleDbParameter("MOBILENO", mobileNo), new OleDbParameter("MESSAGE", messageBody) };
                    OleDbCommand.Parameters.AddRange(param);

                    OleDbCommand.ExecuteNonQuery();

                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                        cn.Close();

                }
            }
            private static string GetReassignSMSTemplate(string callNo, string userName, string userMobile, DateTime callLoggedDate, string engMobileNo)
            {

                if (userName.IndexOf("(") > 0)
                    userName = userName.Substring(0, userName.IndexOf("(")) + (string.IsNullOrEmpty(userMobile) ? "" : "(" + userMobile + ")");
                else
                    userName = userName + (string.IsNullOrEmpty(userMobile) ? "" : "(" + userMobile + ")");

                string messageBody = CallReassignTemplate.Replace("[[CALLNO]]", callNo)
                                                         .Replace("[[CALLLOGDATE]]", callLoggedDate.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.CreateSpecificCulture("en-US")))
                                                         .Replace("[[USERNAME]]", userName);
                return messageBody;
            }
            private static string GetCallCloseSMSTemplate(string callNo, string engName, DateTime callLoggedDate, string engMobileNo)
            {
                engName = engName + (string.IsNullOrEmpty(engMobileNo) ? string.Empty : "(" + engMobileNo + ")");
                string messageBody = CallCompleteTemplate.Replace("[[CALLNO]]", callNo)
                                             .Replace("[[CALLLOGDATE]]", callLoggedDate.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.CreateSpecificCulture("en-US")))
                                             .Replace("[[ENGINEERNAME]]", engName);
                return messageBody;
            }
            private static void SendSMS(string callNo, string userName, string userMobile, DateTime callLoggedDate, string engMobileNo, string engineerName, string statusCode, string serviceLineCode, bool isCloseSMSEnable, string connection)
            {
                string messageBody = string.Empty;
                try
                {
                    ///Check Reassign Call SMS is enable, call status is reassign and call is not auto logged then send sms.
                    if (sendReassignCallSMS.Equals("1") && statusCode.Equals("2") && serviceLineCode.Equals("F", StringComparison.CurrentCultureIgnoreCase) && !string.IsNullOrEmpty(engMobileNo))
                    {
                        messageBody = GetReassignSMSTemplate(callNo, userName, userMobile, callLoggedDate, engMobileNo);
                        InsertSMSEntry(engMobileNo, messageBody, connection);
                    }
                    ///Check 
                    if (sendCloseCallSMS.Equals("1") && statusCode.Equals("1") && serviceLineCode.Equals("F", StringComparison.CurrentCultureIgnoreCase) && !string.IsNullOrEmpty(userMobile) && isCloseSMSEnable)
                    {
                        messageBody = GetCallCloseSMSTemplate(callNo, engineerName, callLoggedDate, engMobileNo);
                        InsertSMSEntry(userMobile, messageBody, connection);
                    }
                }
                catch
                {
                    throw;
                }
            }
            public static void InsertToRmail(string StrAddress, string StrSubject, string StrBody)
            {
                OdbcConnection conn = null;
                try
                {
                    StrAddress = StrAddress + ";";
                    string strConString = ConfigurationManager.ConnectionStrings["RMailConStr"].ToString();
                    string sql = "insert into mail_details(company_code,sbu_code,doc_type,item_st,address_list,subject,message_body,sent_flag)values('ZYC','IMS','Email','M','" + StrAddress + "','" + StrSubject + "','" + StrBody + "','N')";
                    conn = new OdbcConnection(strConString);
                    conn.Open();
                    OdbcCommand comm = new OdbcCommand(sql, conn);
                    comm.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (conn != null && conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }

            }

            public static int InsertUpdateUserLoginHistory(string userId, string ipAddress, string strConString, int userLogId = 0)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                objcon = new SqlConnection(strConString);
                int userLoginHistoryId = 0;

                try
                {

                    objcmd = new SqlCommand("InsertUserLoginHistory", objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;

                    objcmd.Parameters.AddWithValue("@IPAddress", ipAddress);
                    objcmd.Parameters.AddWithValue("@UserID", userId);
                    objcmd.Parameters.AddWithValue("@UserLogId", userLogId);

                    objcon.Open();
                    userLoginHistoryId = Convert.ToInt32(objcmd.ExecuteScalar());

                    objcmd.Dispose();
                    objcon.Dispose();

                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (objcon.State == ConnectionState.Open)
                    {
                        objcon.Close();
                    }
                }

                return userLoginHistoryId;
            }

            public static object ExecuteScalar(string query, string strConn)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                objcon = new SqlConnection(strConn);
                object result = new object();

                try
                {

                    objcmd = new SqlCommand(query, objcon);
                    objcmd.CommandType = CommandType.Text;

                    objcon.Open();

                    result = objcmd.ExecuteScalar();

                    objcmd.Dispose();
                    objcon.Dispose();

                    return result;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (objcon.State == ConnectionState.Open)
                    {
                        objcon.Close();
                    }
                }
            }


            public static DataSet GetUserLoginHistory(string connectionString, DateTime fromDate, DateTime toDate, string userID)//common method to get dataset for a procedure
            {
                DataSet objds;
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                objcon = new SqlConnection(connectionString);

                try
                {
                    objcmd = new SqlCommand("GetUserLoginHistory", objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;

                    objcmd.Parameters.AddWithValue("@FromDate", fromDate);
                    objcmd.Parameters.AddWithValue("@ToDate", toDate);
                    if (string.IsNullOrEmpty(userID))
                        objcmd.Parameters.AddWithValue("@UserId", DBNull.Value);
                    else
                        objcmd.Parameters.AddWithValue("@UserId", userID);

                    objadp = new SqlDataAdapter(objcmd);
                    objds = new DataSet();
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    objcmd.ExecuteNonQuery();
                    objadp.Fill(objds);
                    objcon.Dispose();
                    return objds;
                    //objcon.Close();
                    //objcmd.Dispose();


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

            public static DataSet ViewUserAuditLog(string connectionString, DateTime fromDate, DateTime toDate, string empCode, int active)
            {
                DataSet objds;
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                objcon = new SqlConnection(connectionString);

                try
                {
                    objcmd = new SqlCommand("ViewUserAuditLog", objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;

                    objcmd.Parameters.AddWithValue("@FromDate", fromDate);
                    objcmd.Parameters.AddWithValue("@ToDate", toDate);

                    if (string.IsNullOrEmpty(empCode))
                        objcmd.Parameters.AddWithValue("@EmpCode", DBNull.Value);
                    else
                        objcmd.Parameters.AddWithValue("@EmpCode", empCode);

                    if (active == 1)
                        objcmd.Parameters.AddWithValue("@Active", true);
                    else if (active == 0)
                        objcmd.Parameters.AddWithValue("@Active", false);
                    else
                        objcmd.Parameters.AddWithValue("@Active", DBNull.Value);

                    objadp = new SqlDataAdapter(objcmd);
                    objds = new DataSet();
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    objcmd.ExecuteNonQuery();
                    objadp.Fill(objds);
                    objcon.Dispose();
                    return objds;
                    //objcon.Close();
                    //objcmd.Dispose(); 


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

            public static int AddEmployeeLeaveDetails(DateTime startDate, DateTime endDate, string engCode, string userId, string strConString)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                objcon = new SqlConnection(strConString);
                int result = 0;

                try
                {
                    objcmd = new SqlCommand("InsertLeaveDetails", objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@EngCode", SqlDbType.VarChar).Value = engCode;
                    objcmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = startDate;
                    objcmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = endDate;
                    objcmd.Parameters.Add("@UserId", SqlDbType.VarChar).Value = userId;
                    objcmd.Parameters.Add("@leaveId", SqlDbType.Int);
                    objcmd.Parameters["@leaveId"].Direction = ParameterDirection.Output;

                    objcon.Open();
                    objcmd.ExecuteNonQuery();
                    result = Convert.ToInt32(objcmd.Parameters["@leaveId"].Value);

                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (objcon.State == ConnectionState.Open)
                        objcon.Close();
                }

                return result;

            }

            public static void CancelLeaveRequest(int leaveId, string strConString, string User_Id)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                objcon = new SqlConnection(strConString);

                try
                {
                    objcmd = new SqlCommand("CancelLeaveRequest", objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@LeaveId", SqlDbType.Int).Value = leaveId;
                    objcmd.Parameters.Add("@User_Id", SqlDbType.VarChar).Value = User_Id;
                    objcon.Open();
                    objcmd.ExecuteNonQuery();

                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (objcon.State == ConnectionState.Open)
                        objcon.Close();
                }

            }
        }

        public class classDataGrid
        {
            //Function to fill data in GridView accept GridView Name & Query.
            public static DataSet fillgrid(GridView objgv, string strquery, string strconstring)
            {
                SqlConnection objcon;
                SqlDataAdapter objda;
                DataSet objDs = new DataSet();
                objcon = new SqlConnection(strconstring);
                objcon.Open();
                objda = new SqlDataAdapter(strquery, objcon);
                objda.Fill(objDs);
                objgv.DataSource = objDs;
                objgv.DataBind();
                return objDs;
                objDs.Dispose();
                objda.Dispose();
                objcon.Close();
                objcon.Dispose();

            }


            public static DataSet fillDatagrid(DataGrid objgv, string strquery, string strconstring)
            {
                SqlConnection objcon;
                SqlDataAdapter objda;
                DataSet objDs = new DataSet();
                objcon = new SqlConnection(strconstring);
                objcon.Open();
                objda = new SqlDataAdapter(strquery, objcon);
                objda.Fill(objDs);
                objgv.DataSource = objDs;
                objgv.DataBind();
                return objDs;
                objDs.Dispose();
                objda.Dispose();
                objcon.Close();
                objcon.Dispose();

            }
        }

        public class DropDownlist
        {

            public static void fillListQuery(DropDownList lst, string strQuery, string strvmember, string strdmember, string strconstring)
            {
                int i;
                SqlConnection objcon;
                SqlCommand objcom;
                DataSet objds = new DataSet();
                SqlDataAdapter objda;

                objcon = new SqlConnection(strconstring);


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

                }
                else { lst.Items.Insert(0, new ListItem("---Select---", "0")); }
                objds.Dispose();
                objda.Dispose();
                objcom.Dispose();
                objcon.Close();
                objcon.Dispose();
            }

            public static DataSet fillList(string ProcName, string strConString, string name, string flag)
            {
                DataSet objds;
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                objcon = new SqlConnection(strConString);

                try
                {
                    objcmd = new SqlCommand(ProcName, objcon);

                    if (flag == "LogBy" || flag == "Module" || flag == "COMP")
                    {
                        objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                        objcmd.Parameters.Add("@Log_By", SqlDbType.VarChar).Value = name;
                        objcmd.CommandType = CommandType.StoredProcedure;
                        objcmd.CommandTimeout = 0;
                    }
                    else if (flag == "Complaint" || flag == "PendingCallDetails")
                    {
                        objcmd.Parameters.Add("@Service_Line_Code", SqlDbType.VarChar).Value = name;
                        objcmd.CommandType = CommandType.StoredProcedure;
                    }
                    else if (flag == "ddlDepartment" || flag == "dgDepartment")
                    {
                        objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                        objcmd.Parameters.Add("@Location_Code", SqlDbType.VarChar).Value = name;
                        objcmd.CommandType = CommandType.StoredProcedure;
                    }
                    else if (flag == "Active" || flag == "InActive")
                    {
                        objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                        objcmd.CommandType = CommandType.StoredProcedure;
                    }

                    else if (flag == "ADM" || flag == "CL" || flag == "ENG" || flag == "SLH")
                    {
                        objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                        objcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = name;
                        objcmd.CommandType = CommandType.StoredProcedure;
                    }

                    else if (flag == "DDLList" || flag == "Grid" || flag == "Call Status")
                    {
                        objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                        objcmd.Parameters.Add("@UserId", SqlDbType.VarChar).Value = name;
                        objcmd.CommandType = CommandType.StoredProcedure;
                    }
                    else if (flag == "GridDetail" || flag == "DDLListDetail" || flag == "CallDetail")
                    {
                        objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                        objcmd.CommandType = CommandType.StoredProcedure;
                    }
                    else if (flag == "GridDetailByCompID" || flag == "DDLListDetailByCompID")
                    {
                        objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                        objcmd.Parameters.Add("@CompID", SqlDbType.VarChar).Value = name;
                        objcmd.CommandType = CommandType.StoredProcedure;
                    }

                    else
                    {
                        objcmd.CommandType = CommandType.StoredProcedure;
                    }
                    objadp = new SqlDataAdapter(objcmd);
                    objds = new DataSet();
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    objcmd.ExecuteNonQuery();
                    objadp.Fill(objds);
                    return objds;
                    //objcon.Close();
                    //objcmd.Dispose();
                    objcon.Dispose();

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

            // Vipin 25 Dec 13
            public static DataSet fillListNew(string ProcName, string strConString, string name, string flag, string PageType)
            {
                DataSet objds;
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                objcon = new SqlConnection(strConString);

                try
                {
                    objcmd = new SqlCommand(ProcName, objcon);

                    if (flag == "LogBy" || flag == "Module" || flag == "COMP")
                    {
                        objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                        objcmd.Parameters.Add("@Log_By", SqlDbType.VarChar).Value = name;
                        objcmd.Parameters.Add("@PageType", SqlDbType.VarChar).Value = PageType; // 25 dec 13
                                                                                                //@PageType new line 
                        objcmd.CommandType = CommandType.StoredProcedure;
                    }

                    else if (flag == "Complaint" || flag == "PendingCallDetails")
                    {
                        objcmd.Parameters.Add("@Service_Line_Code", SqlDbType.VarChar).Value = name;
                        objcmd.CommandType = CommandType.StoredProcedure;
                    }
                    else if (flag == "ddlDepartment" || flag == "dgDepartment")
                    {
                        objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                        objcmd.Parameters.Add("@Location_Code", SqlDbType.VarChar).Value = name;
                        objcmd.CommandType = CommandType.StoredProcedure;
                    }
                    else if (flag == "Active" || flag == "InActive")
                    {
                        objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                        objcmd.CommandType = CommandType.StoredProcedure;
                    }

                    else if (flag == "ADM" || flag == "CL" || flag == "ENG" || flag == "SLH")
                    {
                        objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                        objcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = name;
                        objcmd.CommandType = CommandType.StoredProcedure;
                    }

                    else if (flag == "DDLList" || flag == "Grid" || flag == "Call Status")
                    {
                        objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                        objcmd.Parameters.Add("@UserId", SqlDbType.VarChar).Value = name;
                        objcmd.CommandType = CommandType.StoredProcedure;
                    }
                    else if (flag == "GridDetail" || flag == "DDLListDetail" || flag == "CallDetail")
                    {
                        objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                        objcmd.CommandType = CommandType.StoredProcedure;
                    }
                    else if (flag == "GridDetailByCompID" || flag == "DDLListDetailByCompID")
                    {
                        objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                        objcmd.Parameters.Add("@CompID", SqlDbType.VarChar).Value = name;
                        objcmd.CommandType = CommandType.StoredProcedure;
                    }


                    objadp = new SqlDataAdapter(objcmd);
                    objds = new DataSet();
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    objcmd.ExecuteNonQuery();
                    objadp.Fill(objds);
                    return objds;
                    //objcon.Close();
                    //objcmd.Dispose();
                    objcon.Dispose();

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

            public static DataSet fillListStatus(string ProcName, string strConString, string name, string flag, string PageType, string USER_GRP)
            {
                DataSet objds;
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                objcon = new SqlConnection(strConString);

                try
                {
                    objcmd = new SqlCommand(ProcName, objcon);

                    if (flag == "LogBy" || flag == "Module" || flag == "COMP")
                    {
                        objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                        objcmd.Parameters.Add("@Log_By", SqlDbType.VarChar).Value = name;
                        objcmd.Parameters.Add("@PageType", SqlDbType.VarChar).Value = PageType; // 25 dec 13
                                                                                                //@PageType
                        objcmd.CommandType = CommandType.StoredProcedure;
                    }
                    else if (flag == "Complaint" || flag == "PendingCallDetails")
                    {
                        objcmd.Parameters.Add("@Service_Line_Code", SqlDbType.VarChar).Value = name;
                        objcmd.CommandType = CommandType.StoredProcedure;
                    }
                    else if (flag == "ddlDepartment" || flag == "dgDepartment")
                    {
                        objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                        objcmd.Parameters.Add("@Location_Code", SqlDbType.VarChar).Value = name;
                        objcmd.CommandType = CommandType.StoredProcedure;
                    }
                    else if (flag == "Active" || flag == "InActive")
                    {
                        objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                        objcmd.CommandType = CommandType.StoredProcedure;
                    }

                    else if (flag == "ADM" || flag == "CL" || flag == "ENG" || flag == "SLH")
                    {
                        objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                        objcmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = name;
                        objcmd.CommandType = CommandType.StoredProcedure;
                    }

                    else if (flag == "DDLList" || flag == "Grid" || flag == "Call Status")
                    {
                        objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                        objcmd.Parameters.Add("@UserId", SqlDbType.VarChar).Value = name;
                        objcmd.CommandType = CommandType.StoredProcedure;
                    }
                    else if (flag == "GridDetail" || flag == "CallDetail")
                    {
                        objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                        objcmd.CommandType = CommandType.StoredProcedure;

                    }
                    else if (flag == "DDLListDetail")
                    {
                        objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                        objcmd.Parameters.Add("@PageType", SqlDbType.VarChar).Value = PageType; // 25 dec 13
                        objcmd.Parameters.Add("@USER_GRP", SqlDbType.VarChar).Value = USER_GRP; // 25 dec 13
                        objcmd.CommandType = CommandType.StoredProcedure;

                    }
                    else if (flag == "GridDetailByCompID" || flag == "DDLListDetailByCompID")
                    {
                        objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                        objcmd.Parameters.Add("@CompID", SqlDbType.VarChar).Value = name;
                        objcmd.CommandType = CommandType.StoredProcedure;
                    }


                    objadp = new SqlDataAdapter(objcmd);
                    objds = new DataSet();
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    objcmd.ExecuteNonQuery();
                    objadp.Fill(objds);
                    return objds;
                    //objcon.Close();
                    //objcmd.Dispose();
                    objcon.Dispose();

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

            public static DataSet fillList(string ProcName, string strConString, string slCode, string moduleCode, string flag)
            {
                DataSet objds;
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                objcon = new SqlConnection(strConString);

                try
                {
                    objcmd = new SqlCommand(ProcName, objcon);

                    if (flag == "DDLList" || flag == "Grid")
                    {
                        objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                        objcmd.Parameters.Add("@Service_Line_Code", SqlDbType.VarChar).Value = slCode;
                        objcmd.Parameters.Add("@Module_Code", SqlDbType.VarChar).Value = moduleCode;
                        objcmd.CommandType = CommandType.StoredProcedure;
                    }

                    objadp = new SqlDataAdapter(objcmd);
                    objds = new DataSet();
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    objcmd.ExecuteNonQuery();
                    objadp.Fill(objds);
                    objcon.Dispose();
                    return objds;
                    //objcon.Close();
                    //objcmd.Dispose();


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

            public static DataSet fillList_ByComp(string ProcName, string strConString, string slCode, string moduleCode, string comp_code, string flag)
            {
                DataSet objds;
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                objcon = new SqlConnection(strConString);

                try
                {
                    objcmd = new SqlCommand(ProcName, objcon);

                    if (flag == "DDLList" || flag == "Grid")
                    {
                        objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                        objcmd.Parameters.Add("@Service_Line_Code", SqlDbType.VarChar).Value = slCode;
                        objcmd.Parameters.Add("@Module_Code", SqlDbType.VarChar).Value = moduleCode;
                        objcmd.Parameters.Add("@comp_Code", SqlDbType.VarChar).Value = comp_code;
                        objcmd.CommandType = CommandType.StoredProcedure;
                    }

                    objadp = new SqlDataAdapter(objcmd);
                    objds = new DataSet();
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    objcmd.ExecuteNonQuery();
                    objadp.Fill(objds);
                    objcon.Dispose();
                    return objds;
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
            // *** Vipin 25 Dec 
            public static DataSet fillList_ByComplaintType(string ProcName, string strConString, string slCode, string moduleCode, string comp_code, string flag, string complaintCode)
            {
                DataSet objds;
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                objcon = new SqlConnection(strConString);

                try
                {
                    objcmd = new SqlCommand(ProcName, objcon);


                    if (flag == "DDLList" || flag == "Grid")
                    {
                        objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                        objcmd.Parameters.Add("@Service_Line_Code", SqlDbType.VarChar).Value = slCode;
                        objcmd.Parameters.Add("@Module_Code", SqlDbType.VarChar).Value = moduleCode;
                        objcmd.Parameters.Add("@comp_Code", SqlDbType.VarChar).Value = comp_code;
                        objcmd.Parameters.Add("@complaintCode", SqlDbType.VarChar).Value = complaintCode;
                        objcmd.CommandType = CommandType.StoredProcedure;
                    }

                    objadp = new SqlDataAdapter(objcmd);
                    objds = new DataSet();
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    objcmd.ExecuteNonQuery();
                    objadp.Fill(objds);
                    objcon.Dispose();
                    return objds;
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

            public static string GetParamAlert(string strConString, string Param, string Servcice_line_code)
            {
                bool error_flag = false;
                SqlConnection objcon;
                SqlCommand objcmd;
                objcon = new SqlConnection(strConString);
                string procName = "usp_ims_GetParamAlerts";
                string ParamAlert = "";
                try
                {

                    objcmd = new SqlCommand(procName, objcon);
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@Param", SqlDbType.VarChar).Value = Param;
                    objcmd.Parameters.Add("@Servcice_line_code", SqlDbType.VarChar).Value = Servcice_line_code;

                    objcon.Open();
                    ParamAlert = Convert.ToString(objcmd.ExecuteScalar());
                    objcmd.Dispose();
                    objcon.Dispose();
                }
                catch (Exception ex)
                {
                    error_flag = true;
                    throw ex;
                }
                finally
                {
                    if (objcon.State == ConnectionState.Open)
                    {
                        objcon.Close();
                    }
                }
                if (error_flag)
                {
                    return "error";
                }
                else
                {
                    return ParamAlert;
                }

            }

            public static DataSet fillList_ByName(string ProcName, string strConString, string flag, string strname)
            {
                DataSet objds;
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                objcon = new SqlConnection(strConString);

                try
                {
                    objcmd = new SqlCommand(ProcName, objcon);

                    if (flag == "DDLList" || flag == "Grid")
                    {
                        objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                        objcmd.Parameters.Add("@EMP_NAME", SqlDbType.VarChar).Value = strname;
                        objcmd.CommandType = CommandType.StoredProcedure;
                    }

                    objadp = new SqlDataAdapter(objcmd);
                    objds = new DataSet();
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    objcmd.ExecuteNonQuery();
                    objadp.Fill(objds);
                    objcon.Dispose();
                    return objds;
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

            public static DataSet fillList_eng(string ProcName, string strConString, string compCode, string serviceCode, string moduleCode, string UserRole, string locationCode, string enggCode)
            {
                DataSet objds;
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                objcon = new SqlConnection(strConString);

                try
                {
                    objcmd = new SqlCommand(ProcName, objcon);
                    objcmd.Parameters.Add("@COMPCODE", SqlDbType.VarChar).Value = compCode;
                    objcmd.Parameters.Add("@SERVICE_LINE", SqlDbType.VarChar).Value = serviceCode;
                    objcmd.Parameters.Add("@MODULE_CODE", SqlDbType.VarChar).Value = moduleCode;
                    objcmd.Parameters.Add("@UserRole", SqlDbType.VarChar).Value = UserRole;
                    objcmd.Parameters.Add("@locationCode", SqlDbType.VarChar).Value = locationCode;
                    objcmd.Parameters.Add("@enggCode", SqlDbType.VarChar).Value = enggCode;
                    objcmd.CommandType = CommandType.StoredProcedure;

                    objadp = new SqlDataAdapter(objcmd);
                    objds = new DataSet();
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    objcmd.ExecuteNonQuery();
                    objadp.Fill(objds);
                    objcon.Dispose();
                    return objds;
                    //objcon.Close();
                    //objcmd.Dispose();


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

            public static DataSet fillList_Module(string ProcName, string strConString, string compCode, string serviceCode)
            {
                DataSet objds;
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                objcon = new SqlConnection(strConString);

                try
                {
                    objcmd = new SqlCommand(ProcName, objcon);
                    objcmd.Parameters.Add("@COMPCODE", SqlDbType.VarChar).Value = compCode;
                    objcmd.Parameters.Add("@SERVICE_LINE", SqlDbType.VarChar).Value = serviceCode;

                    objcmd.CommandType = CommandType.StoredProcedure;


                    objadp = new SqlDataAdapter(objcmd);
                    objds = new DataSet();
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    objcmd.ExecuteNonQuery();
                    objadp.Fill(objds);
                    objcon.Dispose();
                    return objds;

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

            public static string finddata_detail(string strquery, string strconstring)
            {
                string result = "";
                SqlConnection objcon;
                SqlCommand objcom;
                SqlDataReader objreader;
                objcon = new SqlConnection(strconstring);
                objcon.Open();
                objcom = new SqlCommand(strquery, objcon);
                objreader = objcom.ExecuteReader();
                while (objreader.Read())
                {
                    result = objreader[0].ToString();
                }
                objreader.Close();
                objreader.Dispose();
                objcon.Close();
                objcom.Dispose();
                objcon.Dispose();
                return result;
            }

            public static DataSet fillList_Complaint(string ProcName, string strConString, string flag, string moduleCode, string serviceCode, string company_code)
            {
                DataSet objds;
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                objcon = new SqlConnection(strConString);

                try
                {
                    objcmd = new SqlCommand(ProcName, objcon);
                    objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;

                    objcmd.Parameters.Add("@Log_By", SqlDbType.VarChar).Value = moduleCode;
                    objcmd.Parameters.Add("@SERVICE_LINE", SqlDbType.VarChar).Value = serviceCode;
                    objcmd.Parameters.Add("@COMPANY_ID", SqlDbType.VarChar).Value = company_code;

                    objcmd.CommandType = CommandType.StoredProcedure;


                    objadp = new SqlDataAdapter(objcmd);
                    objds = new DataSet();
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    objcmd.ExecuteNonQuery();
                    objadp.Fill(objds);
                    objcon.Dispose();
                    return objds;
                    //objcon.Close();
                    //objcmd.Dispose();


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

            public static DataSet getEvent(int currentdate, int sMonth, int sYear, string strConString, string ProcName)
            {
                DataSet objds;
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                objcon = new SqlConnection(strConString);
                string selectCmd = "Select * From Holiday_Calendar ";

                try
                {
                    objcmd = new SqlCommand(ProcName, objcon);

                    objcmd.CommandType = CommandType.Text;


                    objadp = new SqlDataAdapter(objcmd);
                    objds = new DataSet();
                    try
                    {
                        objcon.Open();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    objcmd.ExecuteNonQuery();
                    objadp.Fill(objds);
                    objcon.Dispose();
                    return objds;
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

            public static void ProperDateFormat(int d, int m, int y)
            {
                string[] month = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            }


        }
    }
}