<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SingleFieldExploration.aspx.cs" MasterPageFile="~/DataExplorer.Master" Inherits="WebApplication1.SingleFieldExploration" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<script runat="server">

</script>

<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="server">
     <p>Data last updated:
        <asp:Label ID="time" runat="server" />
    </p>

    <%-- Drop down lists to choose table parameters --%>
    <p>Choose a category:&nbsp;
        <asp:DropDownList ID="Categories" runat="server" DataSourceID="Category_Source" DataValueField="Column name" AppendDataBoundItems="true">
            <asp:ListItem Text="--Select One--" Value="" />
        </asp:DropDownList>
    </p>

    <%-- Submission button for requests --%>
    <asp:Button ID="sumbitLoadRequest" runat="server" OnClick="LoadData" Text="Run"/>

    <br /> 
    <%-- Pie chart of the data in the table, initially set to not visible since the
         indices are initially not valid. Once valid data is chosen, the Visible flag
         is set to true --%>
    <asp:Chart ID="Data_Chart" runat="server" DataSourceID = "Data_Source" Visible="false">
        <series>
            <asp:Series ChartType="Pie" Name="Series1" >
            </asp:Series>
        </series>
        <chartareas>
            <asp:ChartArea Name="ChartArea1">
            </asp:ChartArea>
        </chartareas>
    </asp:Chart>

    <%-- Error label for bad indices --%>
    <asp:Label CssClass="error" ID="ErrorLabel" runat="server" ForeColor="Red" />

    <br /><br />
    <%-- Table of data defined by the parameters in the drop down lists. Initially set to not visible since the
         indices are initially not valid. Once valid data is chosen, the Visible flag is set to true--%>
    <asp:GridView ID="DataTable" DataSourceID="Data_Source" runat="server" EmptyDataText="No data selected" AlternatingRowStyle-BackColor="WhiteSmoke" BackColor="Silver" Visible="false" >
    </asp:GridView>

    <%-- SQL DATA SOURCES --%>
    <%--  This is a table of the column names --%>
    <asp:SqlDataSource ID="Category_Source" runat="server" 
        ConnectionString ="<%$ ConnectionStrings:Composite_base_pipe_pz %>" 
        SelectCommand="SELECT COLUMN_NAME AS [Column name]
                       FROM information_schema.columns
                       WHERE (DATA_TYPE != 'geometry')
                       AND TABLE_NAME = 'COMPOSITE_BASE_PIPE_PZ';" >
    </asp:SqlDataSource>

    <%-- Connection to the database --%>
    <asp:SqlDataSource ID="Data_Source" runat="server" 
        ConnectionString ="<%$ ConnectionStrings:Composite_base_pipe_pz %>" >
    </asp:SqlDataSource>

</asp:Content>
