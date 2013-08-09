<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Exploration.aspx.cs" MasterPageFile="~/DataExplorer.Master" Inherits="WebApplication1.Exploration" MaintainScrollPositionOnPostback="true" %>
<%--  --%>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="server">
    <div id="exploration">

        <%-- A button to launch the excel pivot table from the P drive --%>
        <asp:Button ID="ExcelLauncher" runat="server" Text="Open Excel Pivot Table" OnClick="LaunchExcel" />

        <fieldset>
            <legend>Where Clause</legend>
        <%-- Error label for failing to open the excel pivot table --%>
        <asp:Label ID="errorLabel" runat="server" BackColor="Red" />
        <%-- A label for which restrictions have been placed on the query --%>
        <asp:Label ID="where" runat="server" />

        <%-- The set of buttons and lists that control adding a limit to the query --%>
        <p>Which column would you like to limit</p>
        <%-- DDL for which column to restrict --%>
        <asp:DropDownList ID="ColDDL" DataSourceID="Category_Source" DataValueField="Column name" runat="server" AppendDataBoundItems="true" OnSelectedIndexChanged="ColDDL_SelectedIndexChanged" AutoPostBack="true">
            <asp:ListItem Text="--Select One--" Value="--Select One--" />
        </asp:DropDownList>


        <%-- Label for value to restrict prompt. Not visible until valid column is chosen --%>
        <asp:Label ID="LimitQuestionLabel" runat="server" Visible="false" />
        <%-- DDL for which value of the specified column to restrict. Not visible until valid column is chosen --%>
        <asp:DropDownList ID="ValDDL" DataSourceID="ValList" DataValueField="Val" runat="server" AppendDataBoundItems="true" OnSelectedIndexChanged="ValDDL_SelectedIndexChanged" AutoPostBack="true" Visible="false">
            <asp:ListItem Text="--Select One--" Value="--Select One--" />
        </asp:DropDownList>

        <br /><br />
        <asp:Button ID="Parens" runat="server" OnClick="Parens_Click" />
        <br /><br />
        <asp:RadioButtonList ID="AndOrList" runat="server" RepeatDirection="Horizontal" Visible="false">
            <asp:ListItem Selected="True" Text="AND"></asp:ListItem>
            <asp:ListItem Selected="False" Text="OR"></asp:ListItem>
        </asp:RadioButtonList>
        <%-- Button to add specified limit to query. Not visible until valid column and value are specified --%>
        <asp:Button ID="AddLimitBtn" runat="server" Text="Add Limit" OnClick="AddLimitBtn_Click" Visible="false" />
        <%-- Button to clear the query. Always visible. --%>
        <asp:Button ID="ClearLimitBtn" runat="server" Text="Clear All Limits" OnClick="ClearLimitBtn_Click" />

        </fieldset>

        <fieldset>
            <legend>Column Selection</legend>
        <p>Choose all columns that you would like to view:</p>
        <%-- Check box list of column names. Allows users to choose which columns to view --%>
        <asp:CheckBoxList DataSourceID="Category_Source" ID="ColBoxes" DataValueField="Column name" runat="server" OnSelectedIndexChanged="ColBoxes_SelectedIndexChanged" AutoPostBack="true">
        </asp:CheckBoxList>

        </fieldset>
        <fieldset>
            <legend>Display Controls</legend>
        <%-- Label to prompt choice of selected column to order by. Not visible until at least 1 box is checked --%>
        <asp:Label ID="OrderLabel" runat="server" Visible="false" Text="Order by which selected column?" />
        <%-- DDL for which selected column to order by. Not visible until at least 1 box is checked --%>
        <asp:DropDownList ID="OrderByDDL" runat="server" Visible="false" OnSelectedIndexChanged="OrderByDDL_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
        <asp:BulletedList ID="OrderByList" runat="server" />
        <asp:Button ID="ClearOrderBy" runat="server" Text="Clear Order By" OnClick="ClearOrderBy_Click" Visible="false" />

        <asp:Label ID="GroupOrderSeparation" runat="server" Text="<br /><br />" Visible="false" />

        <%-- Label to prompt choice of selected column to order by. Not visible until at least 1 box is checked --%>
        <asp:Label ID="GroupLabel" runat="server" Visible="false" Text="Group by which selected column(s)?" />
        <%-- DDL for which selected column to order by. Not visible until at least 1 box is checked --%>
        <asp:DropDownList ID="GroupByDDL" runat="server" Visible="false" OnSelectedIndexChanged="GroupByDDL_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
        <asp:BulletedList ID="GroupByList" runat="server" />

        <asp:Button ID="ClearGroupBy" runat="server" Text="Clear Group By" OnClick="ClearGroupBy_Click" Visible="false" />
        <asp:Label ID="GroupBySpace" runat="server" Text="<br />" Visible="false" />
        <br />

        <asp:CheckBox ID="addCount" runat="server" OnCheckedChanged="addCount_CheckedChanged" AutoPostBack="true" /> Add Count of Each Unique Record?
        </fieldset>

        <fieldset>
            <legend>Submission</legend>


        <%-- Submission button for requests --%>
        <asp:Button ID="sumbitLoadRequest" runat="server" OnClick="LoadData" Text="Run" />
        </fieldset>
    </div>
    <div id="recordTable">
        <%-- Table of data defined by the parameters in the drop down lists. Initially set to not visible since the
     indices are initially not valid. Once valid data is chosen, the Visible flag is set to true--%>
        <asp:GridView ID="RecordTable" DataSourceID="Data_Source" runat="server" EmptyDataText="No data selected" AlternatingRowStyle-BackColor="WhiteSmoke" BackColor="Silver" Visible="false">
        </asp:GridView>
    </div>

    <%-- SQL DATA SOURCES --%>
    <%--  This is a table of the column names --%>
    <asp:SqlDataSource ID="Category_Source" runat="server"></asp:SqlDataSource>

    <%--  This is a table of the column names --%>
    <asp:SqlDataSource ID="ValList" runat="server"></asp:SqlDataSource>

    <%-- Connection to the database --%>
    <asp:SqlDataSource ID="Data_Source" runat="server"></asp:SqlDataSource>
</asp:Content>
