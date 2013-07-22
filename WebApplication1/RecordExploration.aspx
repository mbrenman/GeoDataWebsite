<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecordExploration.aspx.cs" MasterPageFile="~/DataExplorer.Master" Inherits="WebApplication1.RecordExploration" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="server">
    <p>Which municipality would you like to view?</p>
    <asp:DropDownList ID="MunicipalityList" DataSourceID="Municipalities" DataValueField="Muni" runat="server" AppendDataBoundItems="true">
        <asp:ListItem Text="--Select All--" Value="" />
    </asp:DropDownList>

    <p>Choose all columns that you would like to view:</p>
    <%-- Check box list of column names. Allows users to choose which columns to view --%>
    <asp:CheckBoxList DataSourceID="Category_Source" ID="ColBoxes" DataValueField="Column name" runat="server">
    </asp:CheckBoxList>

    <%-- Submission button for requests --%>
    <asp:Button ID="sumbitLoadRequest" runat="server" OnClick="LoadData" Text="Run"/>


    <%-- Table of data defined by the parameters in the drop down lists. Initially set to not visible since the
     indices are initially not valid. Once valid data is chosen, the Visible flag is set to true--%>
    <asp:GridView ID="RecordTable" DataSourceID="Data_Source" runat="server" EmptyDataText="No data selected" AlternatingRowStyle-BackColor="WhiteSmoke" BackColor="Silver" Visible="false" >
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

    <%--  This is a table of the column names --%>
    <asp:SqlDataSource ID="Municipalities" runat="server" 
        ConnectionString ="<%$ ConnectionStrings:Composite_base_pipe_pz %>" 
        SelectCommand="SELECT DISTINCT Municipality AS Muni
                       FROM COMPOSITE_BASE_PIPE_PZ
                       ORDER BY Muni;" >
    </asp:SqlDataSource>

    <%-- Connection to the database --%>
    <asp:SqlDataSource ID="Data_Source" runat="server" 
        ConnectionString ="<%$ ConnectionStrings:Composite_base_pipe_pz %>" >
    </asp:SqlDataSource>
</asp:Content>