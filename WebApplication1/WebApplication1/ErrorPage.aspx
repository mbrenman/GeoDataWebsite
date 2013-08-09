<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" Inherits="WebApplication1.ErrorPage" MasterPageFile="~/DataExplorer.Master" %>


<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="server">
    <div>
        <h2>Sorry! Something wrong seems to have happened.</h2>
        <h4>Please email mattbrenman@gmail.com with details about what you were trying to access when this error occured, and we will try to resolve this problem for the future.</h4>
        <asp:Label ID="errorLabel" runat="server" />
    </div>
</asp:Content>