using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using WOPIForms.WOPIClient.WOPIClasses;

namespace WOPIForms.WOPIClient.xml
{
    public partial class viewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string src = Request.QueryString["WOPISrc"];

            if (!String.IsNullOrEmpty(src))
            {
                string access_token = Request.Form["access_token"];
                string access_token_ttl = Request.Form["access_token_ttl"];

                // Get the metadata
                string url = String.Format("{0}", src);
                using (WOPIWebClient client =
                     new WOPIWebClient(url, access_token, access_token_ttl))
                {
                    string data = client.DownloadString(url);
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    var d = jss.Deserialize<Dictionary<string, string>>(data);
                }

                // Get the content
                url = String.Format("{0}/contents", src);
                using (WOPIWebClient client =
                    new WOPIWebClient(url, access_token, access_token_ttl))
                {
                    string data = client.DownloadString(url);
                    data = WOPIUtilities.Transform(data, Constants.ProductTransformationXslt);
                    litCode.Text = data;
                }
            }
        }
    }
}