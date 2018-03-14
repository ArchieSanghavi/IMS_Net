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
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Globalization;
using ExcelLibrary.SpreadSheet;

namespace IMS_Net.Admin
{
    public class Utility2
    {
        public Utility2()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        static public string GetFormattedDateTime(object dateToFormat)
        {
            if (dateToFormat != null && !string.IsNullOrEmpty(Convert.ToString(dateToFormat)))
            {
                return Convert.ToDateTime(dateToFormat).ToString("MM/dd/yyyy hh:mm:ss tt");
            }
            else
                return string.Empty;
        }

        static public string GetFormattedDate(object dateToFormat)
        {
            if (dateToFormat != null && !string.IsNullOrEmpty(Convert.ToString(dateToFormat)))
            {
                return Convert.ToDateTime(dateToFormat).ToString("MM/dd/yyyy");
            }
            else
                return string.Empty;
        }

        public static string ExcelDateTimeFormat { get { return "mm/dd/yyyy HH:mm:ss AM/PM"; } }
        public static string ExcelDateFormat { get { return "mm/dd/yyyy"; } }
        public static string GetCurrentDateString()
        {
            return DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss").Replace("/", string.Empty).Replace(" ", string.Empty).Replace(":", string.Empty);
        }
        public static void DeleteDownloadedFile(string filePath)
        {
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);

            if (fileInfo.Exists)
                File.Delete(filePath);
        }
        public static void DownloadExcelFile(DataTable dtToExport, string[] dateTimeColumn, string fileName, string excelFormat, string[] numericFormatColumn = null)
        {
            fileName = fileName + "_" + GetCurrentDateString() + ".xls";
            string filePath = HttpContext.Current.Server.MapPath("../tempFile/" + fileName);

            Workbook workbook = new Workbook();
            Worksheet worksheet = new Worksheet(fileName);
            CellFormat dateTimeFormat = new CellFormat(CellFormatType.DateTime, excelFormat);
            CellFormat numericFormat = new CellFormat(CellFormatType.Number, "0.00");
            string cellValue = string.Empty;

            for (int i = 0; i < dtToExport.Columns.Count; i++)
            {
                // Add column header
                worksheet.Cells[0, i] = new Cell(dtToExport.Columns[i].ColumnName);

                // Populate row data
                for (int j = 0; j < dtToExport.Rows.Count; j++)
                {
                    cellValue = string.Empty;

                    if (dtToExport.Rows[j][i] != DBNull.Value)
                    {
                        cellValue = Convert.ToString(dtToExport.Rows[j][i]);

                        cellValue = cellValue.Replace(Environment.NewLine, "");
                        cellValue = cellValue.Replace("\r\n", string.Empty);
                        cellValue = cellValue.Replace("\t", string.Empty);
                        cellValue = cellValue.Replace("\r", string.Empty);
                        cellValue = cellValue.Replace("\n", string.Empty);
                        cellValue = cellValue.Replace("<b>", string.Empty);
                        cellValue = cellValue.Replace("</b>", string.Empty);
                        cellValue = cellValue.Replace("<br />", " ");

                    }

                    worksheet.Cells[j + 1, i] = new Cell(cellValue);



                    if (dateTimeColumn != null)
                    {
                        ///Apply DateTime format
                        foreach (string columnName in dateTimeColumn)
                        {
                            if (string.Equals(columnName, dtToExport.Columns[i].ColumnName, StringComparison.CurrentCultureIgnoreCase))
                            {
                                worksheet.Cells[j + 1, i].Format = dateTimeFormat;
                            }
                        }
                    }
                    ///Apply Numeric format
                    if (numericFormatColumn != null)
                    {
                        foreach (string columnName in numericFormatColumn)
                        {
                            if (string.Equals(columnName, dtToExport.Columns[i].ColumnName, StringComparison.CurrentCultureIgnoreCase))
                            {
                                worksheet.Cells[j + 1, i].Format = numericFormat;
                            }
                        }
                    }
                }

            }

            workbook.Worksheets.Add(worksheet);
            workbook.Save(filePath);

            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
            HttpContext.Current.Response.TransmitFile(filePath);

            HttpContext.Current.Response.Flush();

            DeleteDownloadedFile(filePath);
        }
        // populate tree dynamically.
        public class PopulateTreeview
        {
            public static TreeNode PopulateTreeView(string roles)
            {

                //try
                //{
                TreeNode rootNode = new TreeNode();
                TreeNode parentNode;
                TreeNode childNode;
                TreeNode subChildNode;

                rootNode = CreateMenuItem(SiteMap.RootNode.Title, SiteMap.RootNode.Url, SiteMap.RootNode.Description);

                if (roles == "ADM")
                {
                    for (int i = 0; i < SiteMap.RootNode.ChildNodes.Count; i++)
                    {

                        parentNode = new TreeNode();
                        parentNode = CreateMenuItem(SiteMap.RootNode.ChildNodes[i].Title, SiteMap.RootNode.ChildNodes[i].Url, SiteMap.RootNode.ChildNodes[i].Description);
                        rootNode.ChildNodes.AddAt(i, parentNode);

                        for (int j = 0; j < SiteMap.RootNode.ChildNodes[i].ChildNodes.Count; j++)
                        {
                            childNode = new TreeNode();
                            childNode = CreateMenuItem(SiteMap.RootNode.ChildNodes[i].ChildNodes[j].Title, SiteMap.RootNode.ChildNodes[i].ChildNodes[j].Url, SiteMap.RootNode.ChildNodes[i].ChildNodes[j].Description);
                            rootNode.ChildNodes[i].ChildNodes.AddAt(j, childNode);

                            for (int k = 0; k < SiteMap.RootNode.ChildNodes[i].ChildNodes[j].ChildNodes.Count; k++)
                            {
                                subChildNode = new TreeNode();
                                subChildNode = CreateMenuItem(SiteMap.RootNode.ChildNodes[i].ChildNodes[j].ChildNodes[k].Title, SiteMap.RootNode.ChildNodes[i].ChildNodes[j].ChildNodes[k].Url, SiteMap.RootNode.ChildNodes[i].ChildNodes[j].ChildNodes[k].Description);
                                rootNode.ChildNodes[i].ChildNodes[j].ChildNodes.AddAt(k, subChildNode);
                            }
                        }
                    }
                }
                else if (roles == "ENG")
                {

                    for (int i = 0; i < SiteMap.RootNode.ChildNodes.Count; i++)
                    {
                        if (SiteMap.RootNode.ChildNodes[i].Title != "Master")
                        {
                            if (SiteMap.RootNode.ChildNodes[i].Title != "User Admin")
                            {
                                if (SiteMap.RootNode.ChildNodes[i].Title != "Mapping Tables")
                                {

                                    parentNode = new TreeNode();
                                    parentNode = CreateMenuItem(SiteMap.RootNode.ChildNodes[i].Title, SiteMap.RootNode.ChildNodes[i].Url, SiteMap.RootNode.ChildNodes[i].Description);
                                    rootNode.ChildNodes.AddAt(i - 3, parentNode);

                                    //if (parentNode.Text == "User Admin")
                                    //{
                                    //    for (int j = 0; j <= SiteMap.RootNode.ChildNodes[i].ChildNodes.Count - 1; j++)
                                    //    {
                                    //        childNode = new TreeNode();
                                    //        childNode = CreateMenuItem(SiteMap.RootNode.ChildNodes[i].ChildNodes[j].Title, SiteMap.RootNode.ChildNodes[i].ChildNodes[j].Url, SiteMap.RootNode.ChildNodes[i].ChildNodes[j].Description);
                                    //        rootNode.ChildNodes[i - 1].ChildNodes.AddAt(j, childNode);

                                    //    }
                                    //}

                                    if (parentNode.Text == "Control Panel")
                                    {
                                        for (int j = 0; j <= SiteMap.RootNode.ChildNodes[i].ChildNodes.Count - 1; j++)
                                        {
                                            childNode = new TreeNode();
                                            childNode = CreateMenuItem(SiteMap.RootNode.ChildNodes[i].ChildNodes[j].Title, SiteMap.RootNode.ChildNodes[i].ChildNodes[j].Url, SiteMap.RootNode.ChildNodes[i].ChildNodes[j].Description);
                                            rootNode.ChildNodes[i - 3].ChildNodes.AddAt(j, childNode);

                                        }
                                    }

                                    if (parentNode.Text == "Transaction")
                                    {
                                        for (int j = 0; j <= SiteMap.RootNode.ChildNodes[i].ChildNodes.Count - 1; j++)
                                        {
                                            childNode = new TreeNode();
                                            childNode = CreateMenuItem(SiteMap.RootNode.ChildNodes[i].ChildNodes[j].Title, SiteMap.RootNode.ChildNodes[i].ChildNodes[j].Url, SiteMap.RootNode.ChildNodes[i].ChildNodes[j].Description);
                                            rootNode.ChildNodes[i - 3].ChildNodes.AddAt(j, childNode);

                                        }
                                    }
                                    if (parentNode.Text == "Reports")
                                    {
                                        for (int j = 0; j <= SiteMap.RootNode.ChildNodes[i].ChildNodes.Count - 1; j++)
                                        {
                                            childNode = new TreeNode();
                                            childNode = CreateMenuItem(SiteMap.RootNode.ChildNodes[i].ChildNodes[j].Title, SiteMap.RootNode.ChildNodes[i].ChildNodes[j].Url, SiteMap.RootNode.ChildNodes[i].ChildNodes[j].Description);

                                            if (childNode.Text != "Call Summary")
                                            {
                                                rootNode.ChildNodes[i - 3].ChildNodes.AddAt(j, childNode);

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                else if (roles == "SLH")
                {
                    for (int i = 0; i < SiteMap.RootNode.ChildNodes.Count; i++)
                    {
                        if (SiteMap.RootNode.ChildNodes[i].Title != "Master")
                        {
                            parentNode = new TreeNode();
                            parentNode = CreateMenuItem(SiteMap.RootNode.ChildNodes[i].Title, SiteMap.RootNode.ChildNodes[i].Url, SiteMap.RootNode.ChildNodes[i].Description);
                            rootNode.ChildNodes.AddAt(i - 1, parentNode);

                            if (parentNode.Text == "Control Panel")
                            {
                                for (int j = 0; j <= SiteMap.RootNode.ChildNodes[i].ChildNodes.Count - 1; j++)
                                {
                                    childNode = new TreeNode();
                                    childNode = CreateMenuItem(SiteMap.RootNode.ChildNodes[i].ChildNodes[j].Title, SiteMap.RootNode.ChildNodes[i].ChildNodes[j].Url, SiteMap.RootNode.ChildNodes[i].ChildNodes[j].Description);
                                    rootNode.ChildNodes[i - 1].ChildNodes.AddAt(j, childNode);

                                }
                            }

                            if (parentNode.Text == "Transaction")
                            {
                                for (int j = 0; j <= SiteMap.RootNode.ChildNodes[i].ChildNodes.Count - 1; j++)
                                {
                                    childNode = new TreeNode();
                                    childNode = CreateMenuItem(SiteMap.RootNode.ChildNodes[i].ChildNodes[j].Title, SiteMap.RootNode.ChildNodes[i].ChildNodes[j].Url, SiteMap.RootNode.ChildNodes[i].ChildNodes[j].Description);
                                    rootNode.ChildNodes[i - 1].ChildNodes.AddAt(j, childNode);

                                }
                            }
                            if (parentNode.Text == "Reports")
                            {
                                for (int j = 0; j <= SiteMap.RootNode.ChildNodes[i].ChildNodes.Count - 1; j++)
                                {
                                    childNode = new TreeNode();
                                    childNode = CreateMenuItem(SiteMap.RootNode.ChildNodes[i].ChildNodes[j].Title, SiteMap.RootNode.ChildNodes[i].ChildNodes[j].Url, SiteMap.RootNode.ChildNodes[i].ChildNodes[j].Description);
                                    rootNode.ChildNodes[i - 1].ChildNodes.AddAt(j, childNode);
                                }
                            }
                        }
                    }
                }
                else if (roles == "CL")
                {
                    //for (int r = 0; r < SiteMap.RootNode.ChildNodes[1].Roles.Count; r++)
                    //{
                    for (int i = 0; i < SiteMap.RootNode.ChildNodes.Count; i++)
                    {
                        if (SiteMap.RootNode.ChildNodes[i].Title != "Master")
                        {
                            parentNode = new TreeNode();
                            parentNode = CreateMenuItem(SiteMap.RootNode.ChildNodes[i].Title, SiteMap.RootNode.ChildNodes[i].Url, SiteMap.RootNode.ChildNodes[i].Description);
                            rootNode.ChildNodes.AddAt(i - 1, parentNode);

                            if (parentNode.Text == "Control Panel")
                            {
                                for (int j = 0; j <= SiteMap.RootNode.ChildNodes[i].ChildNodes.Count - 1; j++)
                                {
                                    childNode = new TreeNode();
                                    childNode = CreateMenuItem(SiteMap.RootNode.ChildNodes[i].ChildNodes[j].Title, SiteMap.RootNode.ChildNodes[i].ChildNodes[j].Url, SiteMap.RootNode.ChildNodes[i].ChildNodes[j].Description);
                                    rootNode.ChildNodes[i - 1].ChildNodes.AddAt(j, childNode);

                                }
                            }

                            if (parentNode.Text == "Transaction")
                            {
                                for (int j = 0; j <= SiteMap.RootNode.ChildNodes[i].ChildNodes.Count - 1; j++)
                                {
                                    childNode = new TreeNode();
                                    childNode = CreateMenuItem(SiteMap.RootNode.ChildNodes[i].ChildNodes[j].Title, SiteMap.RootNode.ChildNodes[i].ChildNodes[j].Url, SiteMap.RootNode.ChildNodes[i].ChildNodes[j].Description);
                                    if (childNode.Text != "Pending Call")
                                    {
                                        rootNode.ChildNodes[i - 1].ChildNodes.AddAt(j, childNode);
                                    }


                                }
                            }
                            if (parentNode.Text == "Reports")
                            {
                                for (int j = 0; j <= SiteMap.RootNode.ChildNodes[i].ChildNodes.Count - 1; j++)
                                {
                                    childNode = new TreeNode();
                                    childNode = CreateMenuItem(SiteMap.RootNode.ChildNodes[i].ChildNodes[j].Title, SiteMap.RootNode.ChildNodes[i].ChildNodes[j].Url, SiteMap.RootNode.ChildNodes[i].ChildNodes[j].Description);

                                    if (childNode.Text == "Call Status" && childNode.Text != "Call Summary")
                                    {
                                        rootNode.ChildNodes[i - 1].ChildNodes.AddAt(j, childNode);

                                    }
                                }
                            }
                        }
                    }
                    //}
                }
                else
                {
                    //for (int i = 0; i < SiteMap.RootNode.ChildNodes.Count; i++)
                    //{
                    //    if (SiteMap.RootNode.ChildNodes[i].Title == "Log Out")
                    //    {
                    //        parentNode = new TreeNode();
                    //        parentNode = CreateMenuItem(SiteMap.RootNode.ChildNodes[i].Title, SiteMap.RootNode.ChildNodes[i].Url, SiteMap.RootNode.ChildNodes[i].Description);
                    //        rootNode.ChildNodes.Add(parentNode);

                    //    }
                    //}
                }

                return (rootNode);
            }

            // create treeview node
            static private TreeNode CreateMenuItem(String title, String url, String description)
            {
                TreeNode node = new TreeNode();

                node.Text = title;
                node.NavigateUrl = url;
                node.ToolTip = description;

                return node;

            }
        }

        // Function to use for Bind Grid


        public class BindDataGrid
        {
            public static DataSet fillgrid(GridView objgv, string procName, string strconstring, string flag)
            //(string procName, string strCon, DataGrid dgRole,string appName, string flag)
            {
                SqlConnection objcon;
                SqlDataAdapter objda;
                DataSet objDs = new DataSet();
                objcon = new SqlConnection(strconstring);
                SqlCommand cmd = new SqlCommand(procName, objcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@flag", SqlDbType.VarChar, 20);
                cmd.Parameters["@flag"].Value = flag;
                objcon.Open();
                //SqlDataAdapter da=new SqlDataAdapter()
                objda = new SqlDataAdapter(cmd);

                objda.Fill(objDs);
                objgv.DataSource = objDs;
                objgv.DataBind();
                return objDs;
                objDs.Dispose();
                objda.Dispose();
                objcon.Close();
                objcon.Dispose();

            }


            public static DataSet fillgrid_Command(GridView objgv, string procName, string strconstring, string flag)
            //(string procName, string strCon, DataGrid dgRole,string appName, string flag)
            {
                SqlConnection objcon;
                SqlDataAdapter objda;
                DataSet objDs = new DataSet();
                objcon = new SqlConnection(strconstring);
                SqlCommand cmd = new SqlCommand(procName, objcon);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@flag", SqlDbType.VarChar, 20);
                cmd.Parameters["@flag"].Value = flag;
                objcon.Open();
                //SqlDataAdapter da=new SqlDataAdapter()
                objda = new SqlDataAdapter(cmd);

                objda.Fill(objDs);
                objgv.DataSource = objDs;
                objgv.DataBind();
                return objDs;
                objDs.Dispose();
                objda.Dispose();
                objcon.Close();
                objcon.Dispose();

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


            public static DataSet fillgrid_ByName(GridView objgv, string procName, string strconstring, string flag, string strName)
            //(string procName, string strCon, DataGrid dgRole,string appName, string flag)
            {
                SqlConnection objcon;
                SqlDataAdapter objda;
                DataSet objDs = new DataSet();
                objcon = new SqlConnection(strconstring);
                SqlCommand cmd = new SqlCommand(procName, objcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@flag", SqlDbType.VarChar, 20);
                cmd.Parameters["@flag"].Value = flag;
                cmd.Parameters.Add("@ENG_NAME", SqlDbType.VarChar, 50);
                cmd.Parameters["@ENG_NAME"].Value = strName;
                objcon.Open();
                //SqlDataAdapter da=new SqlDataAdapter()
                objda = new SqlDataAdapter(cmd);

                objda.Fill(objDs);
                objgv.DataSource = objDs;
                objgv.DataBind();
                return objDs;
                objDs.Dispose();
                objda.Dispose();
                objcon.Close();
                objcon.Dispose();

            }




            /// <summary>
            /// Procedure for datagrid
            /// </summary>
            /// <param name="procName"></param>
            /// <param name="strCon"></param>
            /// <param name="dgRole"></param>
            /// <param name="appName"></param>
            /// <param name="flag"></param>
            /// <returns></returns>
            static public string BindGrid(string procName, string strCon, DataGrid dgRole,
                string appName, string flag)
            {
                string errorMsg = "";
                try
                {
                    DataSet dsRole = new DataSet();
                    if (flag == "SolutionCategory")
                    {
                        dsRole = Utility.DropDownlist.fillList(procName, strCon, "", "", "Grid");
                    }
                    else
                    {
                        dsRole = Utility.DropDownlist.fillList(procName, strCon, appName, flag);
                    }

                    if (dsRole != null)
                    {
                        if (dsRole.Tables[0].Rows.Count > 0)
                        {
                            dgRole.DataSource = dsRole;
                            dgRole.DataBind();
                        }
                        else
                        {
                            errorMsg = "";
                        }

                    }
                    else
                    {
                        errorMsg = "";
                    }
                }

                catch (Exception ex)
                {
                    throw ex;
                }
                return errorMsg;
            }


            static public string BindGrid_BY_Name(string procName, string strCon, DataGrid dgRole,
               string appName, string flag, string strName)
            {
                string errorMsg = "";
                try
                {
                    DataSet dsRole = new DataSet();
                    if (flag == "SolutionCategory")
                    {
                        dsRole = Utility.DropDownlist.fillList_ByName(procName, strCon, "Grid", strName);
                    }
                    else
                    {
                        dsRole = Utility.DropDownlist.fillList_ByName(procName, strCon, flag, strName);
                    }

                    if (dsRole != null)
                    {
                        if (dsRole.Tables[0].Rows.Count > 0)
                        {
                            dgRole.DataSource = dsRole;
                            dgRole.DataBind();
                        }
                        else
                        {
                            errorMsg = "";
                        }

                    }
                    else
                    {
                        errorMsg = "";
                    }
                }

                catch (Exception ex)
                {
                    throw ex;
                }
                return errorMsg;
            }


        }
        public class Database
        {
            public static string GetData(string qry, string constr)
            {
                string result = "";
                SqlConnection objcon;
                SqlCommand objcom;
                SqlDataReader objreader;
                objcon = new SqlConnection(constr);
                objcon.Open();
                objcom = new SqlCommand(qry, objcon);
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
            public static DataTable ExecuteProcedure(string ProcedureName, string strconstring, string strtable, string flag, string Service_Line_Code, string compID, string Engg_Code, string Module_Code)
            {
                SqlConnection objcon = new SqlConnection(strconstring);
                SqlCommand cmd = new SqlCommand(ProcedureName, objcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@flag", SqlDbType.VarChar, 50);
                cmd.Parameters["@flag"].Value = flag;
                cmd.Parameters.Add("@Service_Line_Code", SqlDbType.VarChar, 20);
                cmd.Parameters["@Service_Line_Code"].Value = Service_Line_Code;
                cmd.Parameters.Add("@compID", SqlDbType.VarChar, 20);
                cmd.Parameters["@compID"].Value = compID;
                cmd.Parameters.Add("@Engg_Code", SqlDbType.VarChar, 20);
                cmd.Parameters["@Engg_Code"].Value = Engg_Code;
                cmd.Parameters.Add("@Module_Code", SqlDbType.VarChar, 20);
                cmd.Parameters["@Module_Code"].Value = Module_Code;

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

            public static DataTable GetFMOpenCallDetails(string strConstString, DateTime fromDate, DateTime endDate)
            {
                SqlConnection objcon = new SqlConnection(strConstString);
                SqlCommand cmd = new SqlCommand("usp_Report_FM_Open_Call_Status", objcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime);
                cmd.Parameters["@FromDate"].Value = fromDate;
                cmd.Parameters.Add("@EndDate", SqlDbType.DateTime);
                cmd.Parameters["@EndDate"].Value = endDate;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                adp.Dispose();
                objcon.Close();
                objcon.Dispose();
                return dt;
            }


            public static DataTable GetFMLocationwiseCall(string strConstString, DateTime fromDate, DateTime endDate)
            {
                SqlConnection objcon = new SqlConnection(strConstString);
                SqlCommand cmd = new SqlCommand("USP_REPORT_FM_LOCATION_WISE_CALL", objcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime);
                cmd.Parameters["@FromDate"].Value = fromDate;
                cmd.Parameters.Add("@EndDate", SqlDbType.DateTime);
                cmd.Parameters["@EndDate"].Value = endDate;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                adp.Dispose();
                objcon.Close();
                objcon.Dispose();
                return dt;
            }

            public static DataTable GetFMLocationwiseCallSummary(string strConstString, DateTime fromDate, DateTime endDate, string callType, string moduleCode, string locationCode, string OutsideSLA)
            {
                SqlConnection objcon = new SqlConnection(strConstString);
                SqlCommand cmd = new SqlCommand("USP_REPORT_FM_LOCATION_WISE_CALL_SUMMARY", objcon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime);
                cmd.Parameters["@FromDate"].Value = fromDate;
                cmd.Parameters.Add("@EndDate", SqlDbType.DateTime);
                cmd.Parameters["@EndDate"].Value = endDate;

                if (string.IsNullOrEmpty(callType))
                    cmd.Parameters.AddWithValue("@CallType", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@CallType", callType);

                if (string.IsNullOrEmpty(moduleCode))
                    cmd.Parameters.AddWithValue("@ModuleCode", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@ModuleCode", moduleCode);

                if (string.IsNullOrEmpty(locationCode))
                    cmd.Parameters.AddWithValue("@LocationCode", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@LocationCode", locationCode);

                if (string.IsNullOrEmpty(OutsideSLA))
                    cmd.Parameters.AddWithValue("@OutsideSLA", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@OutsideSLA", OutsideSLA);


                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                adp.Dispose();
                objcon.Close();
                objcon.Dispose();
                return dt;
            }

            public static DataTable GetEngineerwiseCall(string strConstString, DateTime fromDate, DateTime endDate)
            {
                SqlConnection objcon = new SqlConnection(strConstString);
                SqlCommand cmd = new SqlCommand("USP_REPORT_FM_ENGINEER_WISE_CALL", objcon);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime);
                cmd.Parameters["@FromDate"].Value = fromDate;
                cmd.Parameters.Add("@EndDate", SqlDbType.DateTime);
                cmd.Parameters["@EndDate"].Value = endDate;

                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                adp.Fill(dt);
                adp.Dispose();
                objcon.Close();
                objcon.Dispose();

                return dt;
            }

            public static DataTable GetEngineerwiseCallSummary(string strConstString, DateTime fromDate, DateTime endDate, string tac, string tcc, string toc, string tdoc, string sla, string engCode)
            {
                SqlConnection objcon = new SqlConnection(strConstString);
                SqlCommand cmd = new SqlCommand("USP_REPORT_FM_ENGINEER_WISE_CALL_SUMMARY", objcon);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime);
                cmd.Parameters["@FromDate"].Value = fromDate;
                cmd.Parameters.Add("@EndDate", SqlDbType.DateTime);
                cmd.Parameters["@EndDate"].Value = endDate;

                if (tac == "0")
                    cmd.Parameters.AddWithValue("@todayAssignCall", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@todayAssignCall", tac);

                if (tcc == "0")
                    cmd.Parameters.AddWithValue("@todayCompletedCall", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@todayCompletedCall", tcc);

                if (toc == "0")
                    cmd.Parameters.AddWithValue("@todayOpenCall", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@todayOpenCall", toc);

                if (tdoc == "0")
                    cmd.Parameters.AddWithValue("@tilldateOpenCall", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@tilldateOpenCall", tdoc);

                if (sla == "0")
                    cmd.Parameters.AddWithValue("@outsideSLA", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@outsideSLA", sla);

                cmd.Parameters.AddWithValue("@engCode", engCode);

                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                adp.Fill(dt);
                adp.Dispose();
                objcon.Close();
                objcon.Dispose();

                return dt;
            }

            public static void ResetUserPassword(string userId, string strConn, string Password, string UpdatedBy)
            {
                SqlConnection objConn = null;

                try
                {

                    SqlCommand objCmd;

                    objConn = new SqlConnection(strConn);
                    objCmd = new SqlCommand("ResetUserPassword", objConn);
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@UserID", userId);
                    objCmd.Parameters.AddWithValue("@Password", Password);
                    objCmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);

                    objConn.Open();
                    objCmd.ExecuteNonQuery();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (objConn != null)
                        objConn.Close();
                }

            }

            public static DataSet GetUserDetailByUserId(string userId, string strConn)
            {
                SqlConnection objConn = null;
                SqlDataAdapter da = null;
                DataSet ds = null;
                SqlCommand objCmd = null;
                try
                {
                    ds = new DataSet();

                    objConn = new SqlConnection(strConn);
                    objCmd = new SqlCommand("GetUserNameByUserID", objConn);
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@UserID", userId);
                    da = new SqlDataAdapter(objCmd);

                    da.Fill(ds);

                }
                catch
                {
                    throw;
                }

                return ds;
            }
            public static void AddChildLocation(string strConstString, string parentCode, DataTable dtParentLocation)
            {
                using (var connection = new SqlConnection(strConstString))
                {
                    try
                    {

                        connection.Open();

                        SqlCommand objcmd;
                        objcmd = new SqlCommand("InsertParentChildhistory", connection);
                        objcmd.CommandType = CommandType.StoredProcedure;
                        objcmd.Parameters.Add("@ParentLocationCode", SqlDbType.VarChar).Value = parentCode;
                        objcmd.ExecuteNonQuery();

                        SqlTransaction transaction = connection.BeginTransaction();
                        using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                        {
                            bulkCopy.DestinationTableName = "dbo.Parent_Child_Location_Mapping";

                            try
                            {
                                bulkCopy.WriteToServer(dtParentLocation);
                                transaction.Commit();
                            }
                            catch (Exception)
                            {
                                transaction.Rollback();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        public class DropDownlist
        {
            public static DataSet fillList(string ProcName, string strConString, string name, string CompanyCode, string flag)
            {
                DataSet objds;
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                objcon = new SqlConnection(strConString);

                try
                {
                    objcmd = new SqlCommand(ProcName, objcon);
                    //objcmd.CommandType = CommandType.StoredProcedure;
                    if (flag == "LogBy" || flag == "Module" || flag == "COMP")
                    {
                        objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                        objcmd.Parameters.Add("@Log_By", SqlDbType.VarChar).Value = name;
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
                        objcmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar).Value = CompanyCode;
                        objcmd.CommandType = CommandType.StoredProcedure;
                    }

                    else if (flag == "ADM" || flag == "CL" || flag == "ENG" || flag == "SLH")
                    {
                        objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
                        objcmd.Parameters.Add("@User_ID", SqlDbType.VarChar).Value = name;
                        objcmd.CommandType = CommandType.StoredProcedure;
                    }
                    else if (flag == "DDLList" || flag == "Grid" || flag == "Call Status")
                    {
                        objcmd.Parameters.Add("@Flag", SqlDbType.VarChar).Value = flag;
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

            public static DataSet fillList1(string ProcName, string strConString)
            {
                DataSet objds;
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                objcon = new SqlConnection(strConString);

                try
                {
                    objcmd = new SqlCommand(ProcName, objcon);
                    //objcmd.CommandType = CommandType.StoredProcedure;

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

            public static DataSet fillpopup(string ProcName, string strConString, string user_id)
            {
                DataSet objds;
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                objcon = new SqlConnection(strConString);

                try
                {
                    objcmd = new SqlCommand(ProcName, objcon);

                    objcmd.Parameters.Add("@user_ID", SqlDbType.VarChar).Value = user_id;
                    objcmd.CommandType = CommandType.StoredProcedure;
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
            public static DataSet fillList2(string ProcName, string strConString, string serviceline_code)
            {
                DataSet objds;
                SqlConnection objcon;
                SqlCommand objcmd;
                SqlDataAdapter objadp;
                objcon = new SqlConnection(strConString);

                try
                {
                    objcmd = new SqlCommand(ProcName, objcon);
                    //objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@serviceline_code", SqlDbType.VarChar).Value = serviceline_code;
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

        }

        public static string EncryptString(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;

        }

        public static string DecryptString(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;

        }


        public static bool UpdateFeedback_Location(string ProcName, string strConString, bool IsFeedback, string Location_Code, string UpdatedBy)
        {
            SqlConnection objcon;
            SqlCommand objcmd;
            objcon = new SqlConnection(strConString);
            try
            {
                // objcon = new SqlConnection(strConString);
                objcmd = new SqlCommand(ProcName, objcon);
                objcmd.CommandType = CommandType.StoredProcedure;
                objcmd.Parameters.Add("@IsFeedback", SqlDbType.Bit, 1).Value = IsFeedback;
                objcmd.Parameters.Add("@Location_Code", SqlDbType.VarChar, 20).Value = Location_Code;
                objcmd.Parameters.Add("@UpdatedBy", SqlDbType.VarChar, 30).Value = UpdatedBy;

                try
                {
                    objcon.Open();
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                objcmd.ExecuteNonQuery();
                objcon.Close();
                return true;
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

        public static bool Question_MasterOperation(string ProcName, string strConString, string Operation_Flag, string QuestionText, bool IsActive, bool IsDisplay, Int64 QuestionId, string CreatedBy)
        {
            SqlConnection objcon;
            SqlCommand objcmd;
            objcon = new SqlConnection(strConString);
            try
            {

                // objcon = new SqlConnection(strConString);
                objcmd = new SqlCommand(ProcName, objcon);
                objcmd.CommandType = CommandType.StoredProcedure;
                objcmd.Parameters.Add("@Operation_Flag", SqlDbType.VarChar, 50).Value = Operation_Flag;
                objcmd.Parameters.Add("@QuestionText", SqlDbType.VarChar, 2000).Value = QuestionText;
                objcmd.Parameters.Add("@IsActive", SqlDbType.Bit, 1).Value = IsActive;
                objcmd.Parameters.Add("@IsDisplay", SqlDbType.Bit, 1).Value = IsDisplay;
                objcmd.Parameters.Add("@QuestionId", SqlDbType.BigInt, 1).Value = QuestionId;
                objcmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar, 30).Value = CreatedBy;

                try
                {
                    objcon.Open();
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                objcmd.ExecuteNonQuery();
                objcon.Close();
                return true;
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

        public static string UserValidation_FeedbackQuestions(string strConString, string ProcName, string Call_No, DateTime Expiry_Time, string Emp_Code)
        {
            SqlConnection objcon;
            SqlCommand objcmd;
            objcon = new SqlConnection(strConString);
            try
            {
                // objcon = new SqlConnection(strConString);
                objcmd = new SqlCommand(ProcName, objcon);
                objcmd.CommandType = CommandType.StoredProcedure;
                objcmd.Parameters.Add("@Expiry_Time", SqlDbType.DateTime).Value = Expiry_Time;
                objcmd.Parameters.Add("@Call_No", SqlDbType.VarChar, 20).Value = Call_No;
                objcmd.Parameters.Add("@Emp_Code", SqlDbType.VarChar, 20).Value = Emp_Code;
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
                objcon.Close();
                string strMsg = objcmd.Parameters["@msg"].Value.ToString();
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

        public static bool InsertUserFeedBack_Ans(string ProcName, string strConString, string Call_No, int Qid, int AnsId, string LoggedUser)
        {
            SqlConnection objcon;
            SqlCommand objcmd;
            objcon = new SqlConnection(strConString);
            try
            {
                // objcon = new SqlConnection(strConString);
                objcmd = new SqlCommand(ProcName, objcon);
                objcmd.CommandType = CommandType.StoredProcedure;
                objcmd.Parameters.Add("@Call_No", SqlDbType.VarChar, 20).Value = Call_No;
                objcmd.Parameters.Add("@Qid", SqlDbType.Int).Value = Qid;
                objcmd.Parameters.Add("@AnsId", SqlDbType.Int).Value = AnsId;
                objcmd.Parameters.Add("@LoggedUser", SqlDbType.VarChar, 20).Value = LoggedUser;
                try
                {
                    objcon.Open();
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                objcmd.ExecuteNonQuery();
                objcon.Close();
                return true;
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

        public static string GetUserList(string Userdetails)
        {
            string StrUserId = "";
            if (Userdetails.Trim().Length != 10)
            {
                if ("ABCDEFGHIJKLMNOPQRSTUVWXZY".Contains(Userdetails.Trim().ToUpper().Substring(0, 1)))
                {
                    StrUserId = Userdetails.Trim();
                }
                else
                {

                    StrUserId = GenerateUserID(Userdetails.Trim());
                }
            }
            else
            {

                StrUserId = Userdetails.ToString().Trim();

            }

            return StrUserId;
        }

        public static string GenerateUserID(string InputText)
        {
            return "IM" + InputText.PadLeft(8, '0');
        }

    }
}