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
            time.Text = DateTime.Now.ToString();
        }

        protected void LoadData(object sender, EventArgs e)
        {
            //Only load data when the two parameters are set - Index zero is the '--Select One--' field
            if (ValidQueryIndices(Categories.SelectedItem.Text, NumCategories.SelectedItem.Text)) {
                if (SumButton.Checked == true) {
                    Data_Source.SelectCommand = String.Format("SELECT DISTINCT {0} AS {0}, SUM({1}) AS {1} FROM COMPOSITE_BASE_PIPE_PZ GROUP BY {0} ORDER BY {0};", Categories.SelectedValue, NumCategories.SelectedValue);
                    
                } else {
                    Data_Source.SelectCommand = String.Format("SELECT DISTINCT {0} AS {0}, {1} AS {1} FROM COMPOSITE_BASE_PIPE_PZ ORDER BY {0};", Categories.SelectedValue, NumCategories.SelectedValue);
                }
                //Pie Chart should only be updated when the query is valid
                UpdateChartData(Categories.SelectedValue, NumCategories.SelectedValue);
                //Since data is valid, make all charts and tables visible
                setDataVisibility(true);
            } else {
                //If data is invalid or missing, do not show charts and tables
                setDataVisibility(false);
            }
            
        }

        private bool ValidQueryIndices(string val1, string val2)
        {
            if (val1 == "--Select One--" || val2 == "--Select One--") {
                ErrorLabel.Text = "One or more index has not been chosen.";
                return false;
            } else if (val1 == val2) {
                ErrorLabel.Text = "Indices cannot be the same.";
                return false;
            }
            //No errors if this point is hit
            ErrorLabel.Text = "";
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
    }
}