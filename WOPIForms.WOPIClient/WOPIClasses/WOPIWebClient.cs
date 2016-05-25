using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;

namespace WOPIForms.WOPIClient.WOPIClasses
{
    public class WOPIWebClient: WebClient
    {
        private readonly string _access_token;
        private readonly string _access_token_ttl;
        private readonly DateTime _utc;

        public WOPIWebClient(string url, string access_token, string access_token_ttl)
        {
            _access_token = access_token;
            _access_token_ttl = access_token_ttl;
            _utc = DateTime.UtcNow;
        }
        protected override WebRequest GetWebRequest(Uri address)
        {
            UriBuilder builder = new UriBuilder(address);

            string append = String.Format("access_token={0}&access_token_ttl={1}",
                _access_token,
                _access_token_ttl);

            if (builder.Query == null || builder.Query.Length <= 1)
            {
                builder.Query = append;
            }
            else
            {
                builder.Query = builder.Query.Substring(1) + "&" + append;
            }

            WebRequest request = base.GetWebRequest(builder.Uri);
            if (request is HttpWebRequest)
            {
                // Add AuthZ header
                request.Headers.Add(
                    HttpRequestHeader.Authorization,
                    String.Format("Bearer {0}",
                        HttpUtility.UrlDecode(_access_token.Replace("\n", "").Replace("\r", ""))));

                request.Headers.Add(
                    "X-WOPI-Proof",
                    WOPIUtilities.Sign(
                        WOPIUtilities.CreateProofData(builder.Uri.ToString(), _utc, _access_token)));

                // TODO: Add X-WOPI-ProofOld here

                request.Headers.Add(
                    "X-WOPI-TimeStamp",
                    _utc.Ticks.ToString(CultureInfo.InvariantCulture));

                request.Headers.Add(
                    "X-WOPI-ClientVersion",
                    "Wictor.WOPIClient.1.0");

                request.Headers.Add(
                    "X-WOPI-MachineName",
                    Environment.MachineName);
            }
            return request;
        }
    }
}