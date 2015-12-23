<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Hubic.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <h1>hubiC sample code</h1>
    <p>hubiC is a cloud storage system and it has an API so you can make integration from your own programs</p>
    <h3><a href="https://api.hubic.com/" target="_blank">API description</a></h3>
        <hr />
        <p>First thing to do is to login to your hubiC account (or create a new account - you get 25GB storage for free) and create an application.<br />
           Store the Client ID, Secret Client and Redirection domain in the web.config. They should be displayed here:</p>
        <p>
            <asp:Label ID="Label13" runat="server" Text="Client ID:"></asp:Label>
            <asp:TextBox ID="TextBoxClientID" runat="server" Width="400px"></asp:TextBox>
        </p>
        <p>
            <asp:Label ID="Label14" runat="server" Text="Secret Client:"></asp:Label>
            <asp:TextBox ID="TextBoxSecretClient" runat="server" Width="400px"></asp:TextBox>
        </p>
        <p>
            <asp:Label ID="Label15" runat="server" Text="Redirection domain:"></asp:Label>
            <asp:TextBox ID="TextBoxRedirectionDomain" runat="server" Width="400px"></asp:TextBox><br />
            <small>Note: To test on your localhost you must map this domain temporarely to 127.0.0.1 in your hosts file (C:\Windows\System32\drivers\etc\hosts).</small>
        </p>
        <hr />
        <p>With the Client ID and Redirection domain as parameters we call the hubiC login page.<br />
           Here you must login with your credentials and then you are redirected back to this site with an request code</p>
        <asp:Button ID="ButtonGetRequestCode" runat="server" OnClick="ButtonGetRequestCode_Click" Text="Get Request Code" />
        <p>
            <asp:Label ID="Label1" runat="server" Text="Code:"></asp:Label>
            <asp:TextBox ID="TextBoxCode" runat="server" Width="400px"></asp:TextBox>
        </p>
        <p>
            <asp:Label ID="Label2" runat="server" Text="Scope:"></asp:Label>
            <asp:TextBox ID="TextBoxScope" runat="server" Width="400px"></asp:TextBox>
        </p>
        <hr />
        <p>From the request code you can get an access token which you must use when you call the methods in the API</p>
        <asp:Button ID="ButtonGetAccessToken" runat="server" OnClick="ButtonGetAccessToken_Click" Text="Get Access Token" Width="155px" />
        <p>
            <asp:Label ID="Label3" runat="server" Text="Access Token:"></asp:Label>
            <asp:TextBox ID="TextBoxAccessToken" runat="server" Width="400px"></asp:TextBox>
        </p>
        <p>
            <asp:Label ID="Label4" runat="server" Text="Expires in (milliseconds):"></asp:Label>
            <asp:TextBox ID="TextBoxExpiresIn" runat="server" Width="400px"></asp:TextBox>
        </p>
        <p>
            <asp:Label ID="Label5" runat="server" Text="Refresh Token:"></asp:Label>
            <asp:TextBox ID="TextBoxRefreshToken" runat="server" Width="400px"></asp:TextBox>
        </p>
        <p>
            <asp:Label ID="Label6" runat="server" Text="Token type:"></asp:Label>
            <asp:TextBox ID="TextBoxTokenType" runat="server" Width="400px"></asp:TextBox>
        </p>
        <hr />
        <p>The access token expires after a short amount of time but you can use the refresh token to get a new access token:</p>
        <asp:Button ID="ButtonRefreshAccessToken" runat="server" OnClick="ButtonRefreshAccessToken_Click" Text="Refresh Access Token" Width="155px" />
        <hr />
        <p>With the access token you can get some information about the user account</p>
        <p>
            <asp:Button ID="ButtonGetAccount" runat="server" OnClick="ButtonGetAccount_Click" Text="Get Account" />
        </p>
        <p>
            <asp:Label ID="Label7" runat="server" Text="Email:"></asp:Label>
            <asp:TextBox ID="TextBoxEmail" runat="server" Width="400px"></asp:TextBox>
        </p>
        <p>
            <asp:Label ID="Label8" runat="server" Text="Creation Date:"></asp:Label>
            <asp:TextBox ID="TextBoxCreationDate" runat="server" Width="400px"></asp:TextBox>
        </p>
        <p>
            <asp:Label ID="Label9" runat="server" Text="Status:"></asp:Label>
            <asp:TextBox ID="TextBoxStatus" runat="server" Width="400px"></asp:TextBox>
        </p>
        <hr />
        <p>And you can get some information about used space and quota:</p>
        <p>
            <asp:Button ID="ButtonGetUsage" runat="server" OnClick="ButtonGetUsage_Click" Text="Get Usage" />
        </p>
        <p>
            <asp:Label ID="Label16" runat="server" Text="Quota:"></asp:Label>
            <asp:TextBox ID="TextBoxQuota" runat="server" Width="400px"></asp:TextBox>
        </p>
        <p>
            <asp:Label ID="Label17" runat="server" Text="Space used:"></asp:Label>
            <asp:TextBox ID="TextBoxSpaceUsed" runat="server" Width="400px"></asp:TextBox>
        </p>
        <hr />
        <p>To access the file storage you must first get an endpoint and a token:</p>
        <asp:Button ID="ButtonGetCredentials" runat="server" OnClick="ButtonGetCredentials_Click" Text="Get Credentials" Width="113px" />
        <p>
            <asp:Label ID="Label10" runat="server" Text="Token:"></asp:Label>
            <asp:TextBox ID="TextBoxToken" runat="server" Width="350px"></asp:TextBox>
        </p>
        <p>
            <asp:Label ID="Label11" runat="server" Text="Endpoint:"></asp:Label>
            <asp:TextBox ID="TextBoxEndpoint" runat="server" Width="350px"></asp:TextBox>
        </p>
        <p>
            <asp:Label ID="Label12" runat="server" Text="Expires:"></asp:Label>
            <asp:TextBox ID="TextBoxExpires" runat="server" Width="350px"></asp:TextBox>
        </p>
        <hr />
        <p>Then you can use this endpoint and token to access containers and objects (files):</p>
        <table>
            <tr>
                <td>
                    <asp:Button ID="ButtonListContainers" runat="server" OnClick="ButtonListContainers_Click" Text="List containers" Width="200px" />
                    <br />
                    <asp:ListBox ID="ListBoxContainers" runat="server" Width="295px" Height="141px"></asp:ListBox>
                </td>
                <td>
                    <asp:Label ID="Label18" runat="server" Text="New container name:"></asp:Label>
                    <br />
                    <asp:TextBox ID="TextBoxNewContainerName" runat="server">NewContainerName</asp:TextBox>
                    <br />
                    <asp:Button ID="ButtonCreateContainer" runat="server" OnClick="ButtonCreateContainer_Click" Text="Create container" Width="200px" />
                    <br />
                    <br />
                    <asp:Button ID="ButtonDeleteSelectedContainer" runat="server" OnClick="ButtonDeleteSelectedContainer_Click" Text="Delete selected container" Width="200px" />
                    <br />
                    <small>Note: To delete a container it must be empty.</small>
                </td>
            </tr>   
        </table>
        <table>
            <tr>
                <td>
                    <asp:Button ID="ButtonListFiles" runat="server" Text="List files in selected container" Width="200px" OnClick="ButtonListFiles_Click" />
                    <br />
                    <asp:ListBox ID="ListBoxObjects" runat="server" Width="295px" Height="141px"></asp:ListBox>
                </td>
                <td>
                    <asp:Label ID="Label19" runat="server" Text="Upload new file to selected container:"></asp:Label>
                    <br />
                    <asp:FileUpload ID="FileUpload1" runat="server" Width="348px" />
                    <br />
                    <asp:Button ID="ButtonUploadFile" runat="server" Text="Upload file" Width="200px" OnClick="ButtonUploadFile_Click" />
                    <br />
                    <br />
                    <asp:Button ID="ButtonDownload" runat="server" OnClick="ButtonDownload_Click" Text="Download selected file" Width="200px" />
                    <br />
                    <br />
                    <asp:Button ID="ButtonDeleteSelectedFile" runat="server" OnClick="ButtonDeleteSelectedFile_Click" Text="Delete selected file" Width="200px" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
