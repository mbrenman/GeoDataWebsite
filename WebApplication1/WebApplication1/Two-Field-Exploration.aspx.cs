using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class Front : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Data_Source.ConnectionString = Globals.conString;
            if (!IsPostBack)
            {
                Category_Source.ConnectionString = Globals.conString;
                Category_Source.SelectCommand = string.Format("SELECT COLUMN_NAME AS [Column name] FROM information_schema.columns WHERE (DATA_TYPE != 'geometry') AND TABLE_NAME = '{0}';", Globals.tableName);
                Countables.ConnectionString = Globals.conString;
                Countables.SelectCommand = string.Format("SELECT COLUMN_NAME AS [Column name] FROM information_schema.columns WHERE (DATA_TYPE = 'int' OR DATA_TYPE = 'numeric' OR DATA_TYPE = 'smallint') AND TABLE_NAME = '{0}';", Globals.tableName);
            }
        }

        protected void LoadData(object sender, EventArgs e)
        {
            //Only load data when the two parameters are set - Index zero is the '--Select One--' field
            if (ValidQueryIndices(Categories.SelectedItem.Text, NumCategories.SelectedItem.Text))
            {
                string numberType = "INT";
                if (NumCategories.SelectedValue.Contains("Length_"))
                {
                    //Length values should be truncated to 2 decimals, everything else is an int
                    numberType = "DECIMAL(12,2)";
                }
                if (SumButton.Checked == true)
                {
                    Data_Source.SelectCommand = String.Format("SELECT DISTINCT {0} AS {0}, CAST(SUM({1}) AS {2}) AS {1} FROM {3} GROUP BY {0} ORDER BY {0};", Categories.SelectedValue, NumCategories.SelectedValue, numberType, Globals.tableName);

                }
                else
                {
                    Data_Source.SelectCommand = String.Format("SELECT DISTINCT {0} AS {0}, CAST({1} AS {2}) AS {1} FROM {3} ORDER BY {0};", Categories.SelectedValue, NumCategories.SelectedValue, numberType, Globals.tableName);
                }
                //Pie Chart should only be updated when the query is valid
                UpdateChartData(Categories.SelectedValue, NumCategories.SelectedValue);
                //Since data is valid, make all charts and tables visible
                setDataVisibility(true);
            }
            else
            {
                //If data is invalid or missing, do not show charts and tables
                setDataVisibility(false);
            }

        }

        private bool ValidQueryIndices(string val1, string val2)
        {
            //Make sure that there are two meaningful (non-default) and different values
            if (val1 == "--Select One--" || val2 == "--Select One--")
            {
                ErrorLabel.Text = "One or more index has not been chosen.";
                return false;
            }
            else if (val1 == val2)
            {
                ErrorLabel.Text = "Indices cannot be the same.";
                return false;
            }
            ErrorLabel.Text = ""; //No errors if this point is hit
            return true;
        }

        private void UpdateChartData(string xVal, string yVal)
        {
            //Set the two inputs to the Pie Chart
            Data_Chart.Series["Series1"].XValueMember = xVal;
            Data_Chart.Series["Series1"].YValueMembers = yVal;
        }

        private void setDataVisibility(bool visibility)
        {
            Data_Chart.Visible = visibility;
            DataTable.Visible = visibility;
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            Session["Exception"] = ex;
            Response.Redirect("ErrorPage.aspx");
        }
    }
}