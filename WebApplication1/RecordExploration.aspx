<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecordExploration.aspx.cs" MasterPageFile="~/DataExplorer.Master" Inherits="WebApplication1.RecordExploration" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="server">
    <p>Choose all columns that you would like to view:</p>
    <%-- Check box list of column names. Allows users to choose which columns to view --%>
    <asp:CheckBoxList DataSourceID="Category_Source" ID="ColBoxes" DataValueField="Column name" runat="server">
    </asp:CheckBoxList>

    <%-- Submission button for requests --%>
    <asp:Button ID="sumbitLoadRequest" runat="server" OnClick="LoadData" Text="Run"/>

    <%-- NEED TO ADD DATA TABLE HERE AND ADD SQL IN CODE BEHIND FILE --%>

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