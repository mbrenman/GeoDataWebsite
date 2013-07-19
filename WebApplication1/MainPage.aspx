<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainPage.aspx.cs"  MasterPageFile="~/DataExplorer.Master" Inherits="WebApplication1.MainPage" %>
<asp:Content ID="Main" ContentPlaceHolderID="PageContent" runat="server">
    <h3>Welcome to the Data Explorer</h3>
    <h4>Here is a list of the ways to view the data:</h4>
    <ul>
        <li>Two Field Exploration
            <ul>
                <li>Compare any field with a numeric field (Ex: Compare Counties by Miles of Pipe)</li>
                <li>Contains a pie chart and table</li>
            </ul>
        </li>
        <li>Single Field Exploration
            <ul>
                <li>Find distinct values of a column and their counts (Ex: How much construction per year)</li>
            </ul>
        </li>
    </ul>

</asp:Content>