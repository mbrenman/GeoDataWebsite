﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class SingleFieldExploration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Data_Source.ConnectionString = Globals.conString;
            if (!IsPostBack)
            {
                Category_Source.ConnectionString = Globals.conString;
                Category_Source.SelectCommand = string.Format("SELECT COLUMN_NAME AS [Column name] FROM information_schema.columns WHERE (DATA_TYPE != 'geometry') AND TABLE_NAME = '{0}';", Globals.tableName);
            }
        }

        protected void LoadData(object sender, EventArgs e)
        {
            //Only load data when the two parameters are set - Index zero is the '--Select One--' field
            if (ValidQueryIndex(Categories.SelectedItem.Text))
            {
                Data_Source.SelectCommand = String.Format("SELECT {0} AS {0}, COUNT(*) AS [Number of Records] FROM {1} GROUP BY {0} ORDER BY [Number of Records];", Categories.SelectedValue, Globals.tableName);
                //Pie Chart should only be updated when the query is valid
                UpdateChartData(Categories.SelectedValue, "Number of Records");
                //Since data is valid, make all charts and tables visible
                setDataVisibility(true);
            }
            else
            {
                //If data is invalid or missing, do not show charts and tables
                setDataVisibility(false);
            }
        }

        private bool ValidQueryIndex(string val1)
        {
            if (val1 == "--Select One--")
            {
                ErrorLabel.Text = "One or more index has not been chosen.";
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