using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace WebApplication1
{
    public partial class Exploration : System.Web.UI.Page
    {
        //Contains the limits to the query, edited from the front-end controls
        private static string whereClause = "";
        //File path to the pivot table that can be launched in excel from a button on the user end
        private const string pivotTablePath = "P:\\QA\\QA-CompositeBasePipePZ.xlsx";
        private const string Count = "Record_Count";
        private string open = "Open Parenthesis";
        private string close = "Close Parenthesis";

        protected void Page_Load(object sender, EventArgs e)
        {
            ValList.ConnectionString = Globals.conString;
            Data_Source.ConnectionString = Globals.conString;
            if (!IsPostBack)
            {
                whereClause = ""; //Make sure this is always cleared at first load, but not deleted by the controls
                Category_Source.ConnectionString = Globals.conString;
                Category_Source.SelectCommand = string.Format("SELECT COLUMN_NAME AS [Column name] FROM information_schema.columns WHERE (DATA_TYPE != 'geometry') AND TABLE_NAME = '{0}';", Globals.tableName);
                Parens.Text = open;
            }
        }

        protected void LoadData(object sender, EventArgs e)
        {
            string cols = getSelectedColumns();
            //Only load data when at least one column is chosen
            if (cols.Length != 0)
            {
                string orderClause = getOrderClause();
                string groupClause = getGroupClause();
                string countClause = getCountClause();

                if (Parens.Text == close) {
                    //Try to close mismatched parens, may still throw an error
                    whereClause += ")";
                }

                Data_Source.SelectCommand = String.Format("SELECT {0} {1} FROM {2} {3} {4} {5};", cols, countClause, Globals.tableName, whereClause, groupClause, orderClause);
                
                //Since data is valid, make all charts and tables visible
                RecordTable.Visible = true;
            }
            else
            {
                //If no columns are chosen
                RecordTable.Visible = false;
            }
        }

        private string getCountClause() {
            if (addCount.Checked)
            {
                return String.Format(", COUNT(*) as {0}", Count);
            }
            return "";

        }

        private string getSelectedColumns()
        {
            string cols = "";
            foreach (ListItem i in ColBoxes.Items)
            {
                if (i.Selected)
                {
                    //Add all columns to the string of columns, separated with a comma (as valid SQL piece)
                    if (i.Text.Contains("Length_"))
                    {
                        //Removes misleading specificity from recorded lengths, also increasing readabbility
                        cols = cols + "CAST(" + i + " AS DECIMAL(12,2)) AS " + i + ",";
                    }
                    else
                    {
                        //Do not change other columns, but add them nonetheless
                        cols = cols + i + ",";
                    }
                }
            }
            if (cols.Length != 0)
            {
                //Remove trailing comma, if applicable. Will only exist if there is at least one column chosen.
                cols = cols.TrimEnd(',');
            }
            return cols; //Will be an empty string if no columns were chosen
        }

        protected void ColDDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ColDDL.SelectedValue != "--Select One--")
            {
                //ValDDL should contain the new contents, which are likely different, so we need to clear the list
                ValDDL.Items.Clear();
                //Always keep a default value
                ValDDL.Items.Add(new ListItem("--Select One--"));

                ValList.SelectCommand = String.Format("SELECT DISTINCT {0} as Val FROM {1} ORDER BY Val", ColDDL.SelectedValue, Globals.tableName);
                LimitQuestionLabel.Text = String.Format("<p> Limit {0} to which value? </p>", ColDDL.SelectedValue);

                setValueLimitControls(true);
            }
            else
            {
                //If default value is selected for the column, don't allow values to be chosen (would fail query)
                setValueLimitControls(false);
            }
            //If the values limit controls are visible, the default is selected, so the button shouldn't be visible
            AddLimitBtn.Visible = false;
            hideDataTable(); //Since query is changing, remove table and clear command to speed up processing
        }

        private void setValueLimitControls(bool flag)
        {
            ValDDL.Visible = flag;
            LimitQuestionLabel.Visible = flag;
        }

        protected void LaunchExcel(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(pivotTablePath); //Open QA Pivot table in Excel
            }
            catch
            {
                errorLabel.Text = "Cannot find file or cannot open Excel. Please check network connection and try again.<br />";
                //Will not correctly open if user does not have access to the file or if they do not have Excel
            }
        }

        protected void AddLimitBtn_Click(object sender, EventArgs e)
        {
            if (whereClause == "")
            {
                //For first limit added after initialization or a clear
                whereClause = String.Format("WHERE {0} = '{1}'", ColDDL.SelectedValue, ValDDL.SelectedValue);
            }
            else if (endsInParen()) {
                whereClause += String.Format("{0} = '{1}'", ColDDL.SelectedValue, ValDDL.SelectedValue);
            }
            else
            {
                //For adding any limit other than the first one
                whereClause += String.Format(" {0} {1} = '{2}'", AndOrList.SelectedItem.Text, ColDDL.SelectedValue, ValDDL.SelectedValue);
            }
            where.Text = whereClause + "<br />"; //Refreshes text to contain current class member var value
            AndOrList.Visible = true;
            hideDataTable(); //Since query is changing, remove table and clear command to speed up processing
        }

        protected void ValDDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            //AddLimitBtn should only be visible when the seleced value is valid (non default)
            if (ValDDL.SelectedValue != "--Select One--")
            {
                AddLimitBtn.Visible = true;
            }
            else
            {
                AddLimitBtn.Visible = false;
            }
            hideDataTable(); //Since query is changing, remove table and clear command to speed up processing
        }

        protected void ClearLimitBtn_Click(object sender, EventArgs e)
        {
            whereClause = ""; //Resets class member variable
            where.Text = whereClause + "<br />"; //Refreshes text to contain current class member var value
            AndOrList.Visible = false;
            hideDataTable(); //Since query is changing, remove table and clear command to speed up processing
        }

        protected void ColBoxes_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Keep a current list of selected boxes (adding and removing)
            OrderByDDL.Items.Clear();
            GroupByDDL.Items.Clear();
            GroupByList.Items.Clear();
            OrderByList.Items.Clear();

            //Always keep a default value
            OrderByDDL.Items.Add(new ListItem("--None--"));
            GroupByDDL.Items.Add(new ListItem("--None--"));

            //OrderLabel and OrderByDDL only visible when a box is selected
            bool visibility = false;

            foreach (ListItem i in ColBoxes.Items)
            {
                if (i.Selected)
                {
                    //Only keep text from selected boxes
                    OrderByDDL.Items.Add(new ListItem(i.Text));
                    GroupByDDL.Items.Add(new ListItem(i.Text));
                    //OrderLabel and OrderByDDL are now visible
                    visibility = true;
                }
            }

            if (addCount.Checked) {
                OrderByDDL.Items.Add(new ListItem(Count));
            }

            hideDataTable(); //Since query is changing, remove table and clear command to speed up processing
            setGroupByOrderByVisibility(visibility);
        }

        //Changes whether or not the order by and group by sections of the website are visible
        private void setGroupByOrderByVisibility(bool visibility)
        {
            OrderLabel.Visible = visibility;
            OrderByDDL.Visible = visibility;
            ClearOrderBy.Visible = visibility;

            GroupLabel.Visible = visibility;
            GroupByDDL.Visible = visibility;
            ClearGroupBy.Visible = visibility;

            GroupBySpace.Visible = visibility;

            GroupOrderSeparation.Visible = visibility;
        }

        private string getOrderClause()
        {
            //Returns the order by clause of the query if column is chosen, else returns empty string
            string orderClause = "";
            foreach (ListItem i in OrderByList.Items)
            {
                orderClause += String.Format(" {0},", i.Text);
            }
            if (orderClause != "")
            {
                orderClause = "ORDER BY" + orderClause;
                orderClause = orderClause.TrimEnd(',');
            }
            return orderClause;
        }

        private string getGroupClause()
        {
            //Returns the group by clause of the query if column is chosen, else returns empty string
            string groupClause = "";

            foreach (ListItem i in GroupByList.Items)
            {
                groupClause += String.Format(" {0},", i.Text);
            }
            if (needToGroupAllCols())
            {
                foreach (ListItem i in GroupByDDL.Items)
                {
                    if (i.Text != "--None--")
                    {
                        groupClause += String.Format(" {0},", i.Text);
                    }
                }
            }
            if (groupClause != "")
            {
                groupClause = "GROUP BY" + groupClause;
                groupClause = groupClause.TrimEnd(',');
            }
            return groupClause;
        }

        //If count, an aggregation function, is used, every selected column must be in the group by statement
        private bool needToGroupAllCols()
        {
            if (addCount.Checked) {
                return true;
            }
            return false;
        }

        private void hideDataTable()
        {
            Data_Source.SelectCommand = "";
            RecordTable.Visible = false;
        }

        //Add the selected value to the group by list when highlighted in the drop down
        protected void GroupByDDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GroupByDDL.SelectedValue != "--None--")
            {
                GroupByList.Items.Add(new ListItem(GroupByDDL.SelectedValue));
                GroupByDDL.Items.Remove(GroupByDDL.SelectedItem);
            }
        }

        //Restores every value from the group by list to the drop down list
        protected void ClearGroupBy_Click(object sender, EventArgs e)
        {
            foreach (ListItem i in GroupByList.Items)
            {
                GroupByDDL.Items.Add(new ListItem(i.Text));
            }
            GroupByList.Items.Clear();
        }

        //Add the selected value to the order by list when highlighted in the drop down
        protected void OrderByDDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OrderByDDL.SelectedValue != "--None--")
            {
                OrderByList.Items.Add(new ListItem(OrderByDDL.SelectedValue));
                OrderByDDL.Items.Remove(OrderByDDL.SelectedItem);
            }
        }

        //Restores every value from the order by list to the drop down list
        protected void ClearOrderBy_Click(object sender, EventArgs e)
        {
            foreach (ListItem i in OrderByList.Items)
            {
                OrderByDDL.Items.Add(new ListItem(i.Text));
            }
            OrderByList.Items.Clear();
        }

        //Allows users to order by the record count, but only when the aggregate function is being used
        protected void addCount_CheckedChanged(object sender, EventArgs e)
        {
            if (addCount.Checked)
            {
                if (countNotInOrderList())
                {
                    OrderByDDL.Items.Add(new ListItem(Count));
                }
            }
            else {
                removeCountFromOrderList();
            }
        }

        //returns whether or not the record count is already accounted for in the drop down list
        //or the selected list. This avoids having the count in the list multiple times.
        private bool countNotInOrderList()
        {
            foreach (ListItem i in OrderByDDL.Items)
            {
                if (i.Text == Count)
                {
                    return false;
                }
            }
            foreach (ListItem i in OrderByList.Items)
            {
                if (i.Text == Count)
                {
                    return false;
                }
            }
            return true;
        }

        //removes the count from the order by drop down and the list of selected items
        private void removeCountFromOrderList() {
            ListItem delete = null;
            foreach (ListItem i in OrderByDDL.Items)
            {
                if (i.Text == Count)
                {
                    delete = i;
                }
            }
            if (delete != null)
            {
                OrderByDDL.Items.Remove(delete);
            }
            else
            {
                //Invariant: A column (including count) cannot be in both the DDL and the selected list
                //therefore, we only need to check the selected list if it is not in the DDL
                foreach (ListItem i in OrderByList.Items)
                {
                    if (i.Text == Count)
                    {
                        delete = i;
                    }
                }
                if (delete != null)
                {
                    OrderByList.Items.Remove(delete);
                }
            }
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            Session["Exception"] = ex;
            Response.Redirect("ErrorPage.aspx");
        }

        protected void Parens_Click(object sender, EventArgs e)
        {
            if (Parens.Text == open)
            {
                if (endsInParen()) {
                    //Allows user to add a AND/OR with another paren
                    whereClause += String.Format(" {0} (", AndOrList.SelectedValue);
                    AndOrList.Visible = false;
                }
                else if (whereClause == "")
                {
                    //clause needs to start with WHERE
                    whereClause += "WHERE (";
                }
                else
                {
                    //Default case
                    whereClause += " (";
                }
                Parens.Text = close;
                AddLimitBtn.Visible = true;
            }
            else {
                whereClause += ")";
                //After a right paren, there needs to be another clause before a limit
                AddLimitBtn.Visible = false;
                Parens.Text = open;
            }
            whereClause = whereClause.Replace("()", "");
            where.Text = whereClause + "<br />"; //Refreshes text to contain current class member var value
        }

        private bool endsInParen() {
            if (whereClause != "") { //Only when there is a whereClause
                if (whereClause[whereClause.Length - 1] == '(' || whereClause[whereClause.Length - 1] == ')') {
                    return true;
                }
            } 
            return false;
        }
    }
}
