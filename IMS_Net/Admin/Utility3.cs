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
using System.IO;
namespace IMS_Net.Admin
{
    public class Utility3
    {

        public class ClsDataGrid
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
            public static DataSet fillGrid_Procedure(GridView objgv, string strProcName, string strConnection)
            {
                SqlConnection objcon;
                SqlDataAdapter objda = new SqlDataAdapter();
                SqlCommand sqlCmd;
                SqlDataReader sqlReader;
                DataSet objDs = new DataSet();
                objcon = new SqlConnection(strConnection);
                sqlCmd = new SqlCommand(strProcName, objcon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                objcon.Open();
                objda.SelectCommand = sqlCmd;
                objda.Fill(objDs);

                // objgv.DataSource = sqlCmd.ExecuteReader();
                objgv.DataBind();
                return objDs;
                objDs.Dispose();
                objda.Dispose();
                objcon.Close();
                objcon.Dispose();
            }
            public static DataSet fillGrid_Proc_parameter(string strProcName, string strParameter, string strConnection)
            {
                SqlConnection objcon;
                SqlDataAdapter objda = new SqlDataAdapter();
                SqlCommand sqlCmd;
                SqlDataReader sqlReader;
                DataSet objDs = new DataSet();
                objcon = new SqlConnection(strConnection);
                sqlCmd = new SqlCommand(strProcName, objcon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar, 30));
                sqlCmd.Parameters["@userid"].Value = strParameter;
                objcon.Open();
                objda.SelectCommand = sqlCmd;
                objda.Fill(objDs);

                return objDs;
                objDs.Dispose();
                objda.Dispose();
                objcon.Close();
                objcon.Dispose();
            }
            public static DataSet FillGrid_Sel_Flag(GridView objGv, string strProcName, string strConn, string SelFlag)
            {
                SqlConnection objConn;
                SqlDataAdapter objDa;
                SqlCommand objCmd;
                objCmd = new SqlCommand();

                DataSet objDs = new DataSet();
                objConn = new SqlConnection(strConn);
                objCmd.Connection = objConn;
                objCmd.CommandText = strProcName;
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.Parameters.Add(new SqlParameter("@Sel_Flag", SqlDbType.VarChar, 10));
                objCmd.Parameters["@Sel_Flag"].Value = SelFlag;

                objConn.Open();

                objDa = new SqlDataAdapter();
                objDa.SelectCommand = objCmd;
                objDa.Fill(objDs);

                objGv.DataSource = objDs;
                objGv.DataBind();
                return objDs;
                objConn.Close();
                objConn.Dispose();
                objDa.Dispose();
            }


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
                    lst.Items.Insert(0, new ListItem("---- Please Select ----", "0"));
                    lst.SelectedIndex = 0;
                }
                else
                {
                    lst.Items.Insert(0, new ListItem("---- Please Select ----", "0"));
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
            //Function To Find data from Table accept query & connection string & return String.
            public static string finddata(string strquery, string strconstring)
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

            //Function Which Returns DataSet & Accepts Query,TableName(Table in DataSet) & Connection String
            public static DataSet datashow(string strquery, string strconstring, string strtable)
            {
                SqlConnection objcon = new SqlConnection(strconstring);
                SqlDataAdapter adp = new SqlDataAdapter(strquery, objcon);
                DataSet ds = new DataSet();
                adp.Fill(ds, strtable);
                ds.Dispose();
                adp.Dispose();
                objcon.Close();
                objcon.Dispose();//Dispose the Connection Object
                return ds;
            }
            public static DataTable TableDataShow(string strquery, string strconstring, string strtable)
            {
                SqlConnection objcon = new SqlConnection(strconstring);
                SqlDataAdapter adp = new SqlDataAdapter(strquery, objcon);
                DataTable dt = new DataTable();
                adp.Fill(dt);//, strtable);
                adp.Dispose();
                objcon.Close();
                objcon.Dispose();//Dispose the Connection Object
                return dt;
            }
            public DataSet fill_datatbl(string strquery, string strconstring, string strtable)
            {
                SqlConnection objcon = new SqlConnection(strconstring);
                SqlDataAdapter adp = new SqlDataAdapter(strquery, objcon);
                DataSet ds = new DataSet();
                adp.Fill(ds, strtable);
                ds.Dispose();
                adp.Dispose();
                objcon.Close();
                objcon.Dispose();//Dispose the Connection Object
                return ds;
            }

            //Check for Duplicacy, Return True if Data exist in table otherwise False..

            public static bool dataexist(string strquery, string strconstring)
            {
                bool i = false;
                SqlConnection objcon;
                SqlCommand objcom;
                SqlDataReader objreader;

                objcon = new SqlConnection(strconstring);
                objcom = new SqlCommand(strquery, objcon);
                objcom.Connection = objcon;
                objcom.CommandText = strquery;
                objcon.Open();
                objreader = objcom.ExecuteReader();//row will contain no. of effected rows
                i = objreader.HasRows;
                objreader.Close();
                objreader.Dispose();
                objcom.Dispose();//Dispose the Command Object
                objcon.Close();
                objcon.Dispose();//Dispose the Connection Object
                return i;
            }

            //Insert,Update data onto Table & return No. of rows effected..
            //also work for delete data
            public static int insertdata(string strquery, string type, string strconstring)
            {
                int row = 0;

                SqlConnection objcon;
                SqlCommand objcom;

                objcon = new SqlConnection(strconstring);
                objcom = new SqlCommand(strquery, objcon);
                objcon.Open();
                if (type == "Table")
                {
                    objcom.ExecuteNonQuery();
                    row = 0;
                }
                else
                    row = objcom.ExecuteNonQuery();//row will contain no. of effected rows
                objcon.Close();
                objcom.Dispose();//Dispose the Command Object
                objcon.Dispose();//Dispose the Connection Object
                return row;
            }


            public static string CheckUser(string UserID, string Password, string strCountry, string strconstring)
            {
                string res = string.Empty;

                SqlConnection objcon;
                SqlCommand objcom;
                SqlDataReader objreader;
                string SqlQry = "Select user_name from x_user_master where user_id='" + UserID + "' and current_pwd='" + Password + "' and country_code = '" + strCountry + "' ";
                objcon = new SqlConnection(strconstring);
                objcon.Open();
                objcom = new SqlCommand(SqlQry, objcon);
                objreader = objcom.ExecuteReader();
                if (objreader.Read())
                {
                    res = objreader["user_name"].ToString();
                }
                return res;
                objreader.Dispose();
                objcon.Close();
                objcom.Dispose();//Dispose the Command Object
                objcon.Dispose();//Dispose the Connection Object
            }
            public static DataSet Get_CountryInfo(string strconString)
            {
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                DataSet ds;
                objcon = new SqlConnection(strconString);

                try
                {
                    objcmd = new SqlCommand("select * from Country_Master", objcon);
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

            public static string getConn()
            {
                string _strConn = ConfigurationManager.ConnectionStrings["ReachIBDConnectionString"].ToString();
                return _strConn;
            }

            public static DataSet GetData(string strConnectionString, string procedureName, SqlParameter[] objParameters)
            {
                DataSet ds = null;
                try
                {
                    ds = new DataSet();

                    using (SqlConnection con = new SqlConnection(strConnectionString))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(procedureName, con))
                        {
                            da.SelectCommand.CommandType = CommandType.StoredProcedure;
                            da.SelectCommand.CommandTimeout = 180;

                            ///Set Parameters
                            if (objParameters != null && objParameters.Length > 0)
                            {
                                foreach (SqlParameter param in objParameters)
                                    da.SelectCommand.Parameters.Add(param);
                            }
                            da.Fill(ds);
                        }
                    }
                    return ds;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public static string InsertUpdateData(string strConnectionString, string procedureName, SqlParameter[] objParameters)
            {
                string msg = null;
                SqlCommand objcmd;
                try
                {
                    using (SqlConnection objcon = new SqlConnection(strConnectionString))
                    {
                        objcmd = new SqlCommand(procedureName, objcon);
                        objcmd.CommandType = CommandType.StoredProcedure;
                        ///Set Parameters
                        if (objParameters != null && objParameters.Length > 0)
                        {
                            foreach (SqlParameter param in objParameters)
                                objcmd.Parameters.Add(param);
                        }
                        objcmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                        objcmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                        objcon.Open();
                        objcmd.ExecuteNonQuery();
                        msg = objcmd.Parameters["@msg"].Value.ToString();
                        objcon.Close();
                    }
                    return msg;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static string InsertUserAccountHistory(string strConnectionString, string ProcedureName, SqlParameter[] objParameters)
            {
                string msg = null;
                SqlCommand objcmd;
                try
                {
                    using (SqlConnection objcon = new SqlConnection(strConnectionString))
                    {
                        objcmd = new SqlCommand(ProcedureName, objcon);
                        objcmd.CommandType = CommandType.StoredProcedure;
                        ///Set Parameters
                        if (objParameters != null && objParameters.Length > 0)
                        {
                            foreach (SqlParameter param in objParameters)
                                objcmd.Parameters.Add(param);
                        }
                        objcmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                        objcmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                        objcon.Open();
                        objcmd.ExecuteNonQuery();
                        msg = objcmd.Parameters["@msg"].Value.ToString();
                        objcon.Close();
                    }
                    return msg;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static string getDate()
            {
                string CommaDate = "";
                string Startdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("MM/dd/yyyy");
                string Curretndate = DateTime.Today.ToString("MM/dd/yyyy");
                CommaDate = Startdate + "," + Curretndate;
                return CommaDate;
            }
        }
        public class General
        {
            static string logFilePath = Convert.ToString(ConfigurationManager.AppSettings["LogFilePath"]);
            /// <summary>
            /// Valdate Numreic Feild 
            /// </summary>
            /// <param name="tbox"></param>
            /// <returns></returns>
            public static Double Validate_NumericValeNull(TextBox tbox)
            {
                Double dbl = -1;
                if (tbox.Text == "")
                {
                    dbl = 0.00;
                }
                else
                {
                    dbl = Double.Parse(tbox.Text);
                }
                return dbl;
            }
            public static int Validate_LblNumericValeNull(Label tbox)
            {
                int dbl = -1;
                if (tbox.Text == "")
                {
                    dbl = 0;
                }
                else
                {
                    dbl = int.Parse(tbox.Text);
                }
                return dbl;
            }
            public static void Validate_NumericValeNullNull(TextBox tbox)
            {
                if (tbox.Text == "")
                {
                    tbox.Text = "0";
                }
            }
            public static float Validate_NullString(string str)
            {
                float flt = 0;
                if (str == "")
                {
                    flt = 0;
                }
                else
                {
                    flt = float.Parse(str);
                }
                return flt;
            }
            /// <summary>
            /// Valdate DateTime Field 
            /// </summary>
            /// <param name="tbox"></param>
            /// <returns></returns>
            public static DateTime Validate_DateTime(TextBox tbox)
            {
                DateTime dt;
                if (tbox.Text == "")
                {
                    // assign Min Value of Date in case of null value 
                    dt = DateTime.Parse("01/01/1900");
                }
                else
                {
                    dt = DateTime.Parse(tbox.Text);
                }
                return dt;

            }
            public static string DateTime_ISNULL(string str_datetime)
            {
                string str1 = string.Empty;
                if (str_datetime == string.Empty || str_datetime == "")
                {
                    str1 = string.Empty;
                }
                else
                {
                    str1 = DateTime.Parse(str_datetime).ToShortDateString().Replace("1/1/1900", "");
                }
                return str1;
            }
            /// <summary>
            /// validate Negative Integer
            /// </summary>
            /// <param name="tbox"></param>
            public static void Validate_NegativeInt(TextBox tbox)
            {
                if (double.Parse(tbox.Text.ToString()) < 0)
                    tbox.Text = "0";
            }
            /// <summary>
            /// Set Drop Down List item through value
            /// </summary>
            /// <param name="ddl"></param>
            /// <param name="str"></param>
            public static void SetDropDownValue(DropDownList ddl, string str)
            {
                ddl.SelectedIndex = -1;
                for (int i = 0; i < ddl.Items.Count; i++)
                {
                    if (ddl.Items[i].Value == str.Trim())
                    {
                        ddl.Items[i].Selected = true;
                        break;
                    }
                    else
                    {
                        ddl.Items[i].Selected = false;
                    }
                }
            }
            /// <summary>
            /// Set Drop Down List item through Text
            /// </summary>
            /// <param name="ddl"></param>
            /// <param name="str"></param>
            /// 
            public static void SetDropDownText(DropDownList ddl, string str)
            {
                ddl.SelectedIndex = -1;
                for (int i = 0; i < ddl.Items.Count; i++)
                {
                    if (ddl.Items[i].Text.Trim() == str.Trim())
                    {
                        ddl.Items[i].Selected = true;
                        break;
                    }
                    else
                    {
                        ddl.Items[i].Selected = false;
                    }
                }
            }

            public static void SetListBoxValue(ListBox ListBx, string str)
            {
                for (int i = 0; i < ListBx.Items.Count; i++)
                {
                    if (ListBx.Items[i].Value == str.Trim())
                    {
                        ListBx.Items[i].Selected = true;
                        break;
                    }
                }
            }

            public static void LogExcpetion(string Message)
            {
                try
                {
                    File.AppendAllText(logFilePath, Message);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
    }
}