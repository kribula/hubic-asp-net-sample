using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hubic
{
    public partial class Default : System.Web.UI.Page
    {
        private string hubic_auth_url = "https://api.hubic.com/oauth/auth/?client_id={0}&redirect_uri={1}&scope={2}&response_type={3}&state={4}";
        private string hubic_auth_token_url = "https://api.hubic.com/oauth/token";
        private string hubic_account_url = "https://api.hubic.com/1.0/account";
        private string hubic_usage_url = "https://api.hubic.com/1.0/account/usage";
        private string hubic_credentials_url = "https://api.hubic.com/1.0/account/credentials";

        private string Client_ID 
        {
            get
            {
                return TextBoxClientID.Text;
            }
        }

        private string SecretClient
        {
            get
            {
                return TextBoxSecretClient.Text;
            }
        }

        private string RedirectionDomain
        {
            get
            {
                return TextBoxRedirectionDomain.Text;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // If parameter state=code in query string then this is the redirect from the 
            // hubiC login page with the request code and scope
            string state = Request.QueryString["state"];
            switch (state)
            {
                case "code":
                    TextBoxCode.Text = Request.QueryString["code"];
                    TextBoxScope.Text = Request.QueryString["scope"];
                    break;
            }

            // Try to load Client ID, Secret Client and Redirection Domain from web.config
            if (string.IsNullOrEmpty(TextBoxClientID.Text))
            {
                TextBoxClientID.Text = System.Configuration.ConfigurationManager.AppSettings["ClientID"];
            }
            if (string.IsNullOrEmpty(TextBoxSecretClient.Text))
            {
                TextBoxSecretClient.Text = System.Configuration.ConfigurationManager.AppSettings["SecretClient"];
            }
            if (string.IsNullOrEmpty(TextBoxRedirectionDomain.Text))
            {
                TextBoxRedirectionDomain.Text = System.Configuration.ConfigurationManager.AppSettings["RedirectionDomain"];
            }
        }

        protected void ButtonGetRequestCode_Click(object sender, EventArgs e)
        {
            // Redirect to the hubiC login page to get the request code
            // When you have entered your credentials you are redirected back 
            // to this site and you can get the request code from the query string
            string response_type = "code";
            string scope = "usage.r,account.r,getAllLinks.r,credentials.r,sponsorCode.r,activate.w,sponsored.r,links.drw";
            string state = response_type;

            string url = string.Format(hubic_auth_url,
                Client_ID, HttpUtility.UrlEncode(RedirectionDomain), scope, response_type, state);

            Response.Redirect(url);
        }

        protected void ButtonGetAccessToken_Click(object sender, EventArgs e)
        {
            // From the request code you get the access token
            NameValueCollection parameters = new NameValueCollection()
            {
                {"code", TextBoxCode.Text},
                {"redirect_uri", RedirectionDomain},
                {"grant_type", "authorization_code"}
            };

            string response = DoHttpPost(parameters);

            JavaScriptSerializer jsonSerialize = new JavaScriptSerializer();
            AccessToken accessToken = jsonSerialize.Deserialize<AccessToken>(response);

            TextBoxAccessToken.Text = accessToken.access_token;
            TextBoxExpiresIn.Text = accessToken.expires_in;
            TextBoxRefreshToken.Text = accessToken.refresh_token;
            TextBoxTokenType.Text = accessToken.token_type;
        }

        protected void ButtonRefreshAccessToken_Click(object sender, EventArgs e)
        {
            // Refresh the access token
            NameValueCollection parameters = new NameValueCollection()
            {
                {"refresh_token", TextBoxRefreshToken.Text},
                {"grant_type", "refresh_token"}
            };

            string response = DoHttpPost(parameters);

            JavaScriptSerializer jsonSerialize = new JavaScriptSerializer();
            AccessToken accessToken = jsonSerialize.Deserialize<AccessToken>(response);

            TextBoxAccessToken.Text = accessToken.access_token;
            TextBoxExpiresIn.Text = accessToken.expires_in;
            TextBoxTokenType.Text = accessToken.token_type;
        }

        protected void ButtonGetAccount_Click(object sender, EventArgs e)
        {
            // Get account information
            string response = DoHttpGet(hubic_account_url);
            JavaScriptSerializer jsonSerialize = new JavaScriptSerializer();

            Account account = jsonSerialize.Deserialize<Account>(response);

            TextBoxEmail.Text = account.email;
            TextBoxCreationDate.Text = account.creationDate;
            TextBoxStatus.Text = account.status;
        }

        protected void ButtonGetUsage_Click(object sender, EventArgs e)
        {
            // Get usage information
            string response = DoHttpGet(hubic_usage_url);
            JavaScriptSerializer jsonSerialize = new JavaScriptSerializer();

            Usage usage = jsonSerialize.Deserialize<Usage>(response);

            TextBoxQuota.Text = usage.quota;
            TextBoxSpaceUsed.Text = usage.used;
        }

        protected void ButtonGetCredentials_Click(object sender, EventArgs e)
        {
            // Get endpoint and token to the file storage
            string response = DoHttpGet(hubic_credentials_url);
            JavaScriptSerializer jsonSerialize = new JavaScriptSerializer();

            Credentials credentials = jsonSerialize.Deserialize<Credentials>(response);

            TextBoxToken.Text = credentials.token;
            TextBoxEndpoint.Text = credentials.endpoint;
            TextBoxExpires.Text = credentials.expires;
        }

        protected void ButtonListContainers_Click(object sender, EventArgs e)
        {
            // List the containers
            ListContrainers();
        }

        protected void ButtonCreateContainer_Click(object sender, EventArgs e)
        {
            // Create a new container
            string containername = TextBoxNewContainerName.Text;

            string endpoint = TextBoxEndpoint.Text + "/" + containername;
            string token = TextBoxToken.Text;

            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("X-Auth-Token", token);
                byte[] data = new byte[0];
                byte[] responseData = wc.UploadData(endpoint, "PUT", data);
                string response = System.Text.Encoding.UTF8.GetString(responseData);

                ListContrainers();
            }
        }

        protected void ButtonDeleteSelectedContainer_Click(object sender, EventArgs e)
        {
            // Delete a container
            string containername = ListBoxContainers.SelectedItem.Text;

            string endpoint = TextBoxEndpoint.Text + "/" + containername;
            string token = TextBoxToken.Text;

            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("X-Auth-Token", token);
                byte[] data = new byte[0];
                byte[] responseData = wc.UploadData(endpoint, "DELETE", data);
                string response = System.Text.Encoding.UTF8.GetString(responseData);

                ListContrainers();
            }
        }

        protected void ButtonListFiles_Click(object sender, EventArgs e)
        {
            ListObjects();
        }

        protected void ButtonUploadFile_Click(object sender, EventArgs e)
        {
            byte[] fileData = FileUpload1.FileBytes;

            string containername = ListBoxContainers.SelectedItem.Text;
            string filename = FileUpload1.FileName;

            
            string endpoint = TextBoxEndpoint.Text + "/" + containername + "/" + filename;
            string token = TextBoxToken.Text;

            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("X-Auth-Token", token);
                byte[] responseData = wc.UploadData(endpoint, "PUT", fileData);
                string response = System.Text.Encoding.UTF8.GetString(responseData);

                ListObjects();
            }
        }

        protected void ButtonDownload_Click(object sender, EventArgs e)
        {
            string containername = ListBoxContainers.SelectedItem.Text;
            string filename = ListBoxObjects.SelectedItem.Text;

            string endpoint = TextBoxEndpoint.Text + "/" + containername + "/" + filename;
            string token = TextBoxToken.Text;

            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("X-Auth-Token", token);
                byte[] data = wc.DownloadData(endpoint);

                Response.OutputStream.Write(data, 0, data.Length);
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                Response.End();
            }           
        }

        protected void ButtonDeleteSelectedFile_Click(object sender, EventArgs e)
        {
            // Delete a file
            string containername = ListBoxContainers.SelectedItem.Text;
            string filename = ListBoxObjects.SelectedItem.Text;

            string endpoint = TextBoxEndpoint.Text + "/" + containername + "/" + filename;
            string token = TextBoxToken.Text;

            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("X-Auth-Token", token);
                byte[] data = new byte[0];
                byte[] responseData = wc.UploadData(endpoint, "DELETE", data);
                string response = System.Text.Encoding.UTF8.GetString(responseData);

                ListObjects();
            }
        }

        private void ListContrainers()
        {
            string endpoint = TextBoxEndpoint.Text;
            string token = TextBoxToken.Text;

            using (WebClient wc = new WebClient())
            {

                wc.Headers.Add("X-Auth-Token", token);
                using (StreamReader sr = new StreamReader(wc.OpenRead(endpoint)))
                {
                    string response = sr.ReadToEnd();
                    string[] containers = response.Split('\n');
                    ListBoxContainers.Items.Clear();
                    foreach (string s in containers)
                    {
                        ListBoxContainers.Items.Add(s);
                    }
                    ListBoxContainers.Items[0].Selected = true;
                }
            }
        }

        private void ListObjects()
        {
            string container = ListBoxContainers.SelectedItem.Text;

            string endpoint = TextBoxEndpoint.Text + "/" + container;
            string token = TextBoxToken.Text;

            using (WebClient wc = new WebClient())
            {

                wc.Headers.Add("X-Auth-Token", token);
                using (StreamReader sr = new StreamReader(wc.OpenRead(endpoint)))
                {
                    string response = sr.ReadToEnd();
                    string[] objects = response.Split('\n');
                    ListBoxObjects.Items.Clear();
                    if (!string.IsNullOrEmpty(response))
                    {
                        foreach (string s in objects)
                        {
                            ListBoxObjects.Items.Add(s);
                        }
                        ListBoxObjects.Items[0].Selected = true;
                    }
                }
            }
        }

        // Helper method for doing a HTTP POST
        private string DoHttpPost(NameValueCollection parameters)
        {
            string auth = "Basic " + Convert.ToBase64String(
                System.Text.Encoding.UTF8.GetBytes(Client_ID + ":" + SecretClient));

            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add(HttpRequestHeader.Authorization, auth);
                byte[] response = wc.UploadValues(hubic_auth_token_url, parameters);
                return System.Text.Encoding.UTF8.GetString(response);
            }
        }

        // Helper method for doing a HTTP GET
        private string DoHttpGet(string url)
        {
            string accessToken = TextBoxAccessToken.Text;
            string accessTokenType = TextBoxTokenType.Text;
            string auth = accessTokenType + " " + accessToken;

            using (WebClient wc = new WebClient())
            {

                wc.Headers.Add(HttpRequestHeader.Authorization, auth);
                using (StreamReader sr = new StreamReader(wc.OpenRead(url)))
                {
                    string response = sr.ReadToEnd();
                    return response;
                }
            }
        }

        // Helper classes for deserializing json
        private class AccessToken
        {
            public string access_token { get; set; }
            public string expires_in { get; set; }
            public string refresh_token { get; set; }
            public string token_type { get; set; }
        }

        private class Account
        {
            public string email { get; set; }
            public string creationDate { get; set; }
            public string status { get; set; }
        }

        private class Usage
        {
            public string quota { get; set; }
            public string used { get; set; }
        }

        private class Credentials
        {
            public string token { get; set; }
            public string endpoint { get; set; }
            public string expires { get; set; }
        }
    }
}