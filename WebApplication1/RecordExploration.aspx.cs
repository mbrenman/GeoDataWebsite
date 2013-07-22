using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class RecordExploration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LoadData(object sender, EventArgs e)
        {
            string cols = getSelectedColumns();
            //Only load data when at least one column is chosen
            if (cols.Length != 0)
            {
                if (MunicipalityList.SelectedValue == "--Select All--")
                {
                    Data_Source.SelectCommand = String.Format("SELECT {0} FROM COMPOSITE_BASE_PIPE_PZ;", cols);
                }
                else {
                    Data_Source.SelectCommand = String.Format("SELECT {0} FROM COMPOSITE_BASE_PIPE_PZ WHERE Municipality = '{1}';", cols, MunicipalityList.SelectedValue);
                }
                //Since data is valid, make all charts and tables visible
                RecordTable.Visible = true;
            }
            else
            {
                //If no columns are chosen
                RecordTable.Visible = false;
            }
        }

        private string getSelectedColumns()
        { 
            string cols = "";
            foreach (ListItem i in ColBoxes.Items) {
                if (i.Selected) {
                    //Add all columns to the string of columns, separated with a comma (as valid SQL piece)
                    cols = cols + i + ",";
                }
            }
            if (cols.Length != 0) {
                //Remove trailing comma, if applicable. Will only exist if there is at least one column chosen.
                cols = cols.TrimEnd(',');
            }
            return cols; //Will be an empty string if no columns were chosen
        }

    }
}