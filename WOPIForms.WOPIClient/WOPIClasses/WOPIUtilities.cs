using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Xsl;

namespace WOPIForms.WOPIClient.WOPIClasses
{
    public static class WOPIUtilities
    {
        public static byte[] CreateProofData(string url, DateTime time, string accesstoken)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] accessbytes = encoding.GetBytes(
                HttpUtility.UrlDecode(accesstoken));
            byte[] urlbytes = encoding.GetBytes(
                new Uri(url).AbsoluteUri.ToUpperInvariant());
            byte[] ticksbytes = getNetworkOrderBytes(time.Ticks);

            List<byte> list = new List<byte>();
            list.AddRange(getNetworkOrderBytes(accessbytes.Length));
            list.AddRange(accessbytes);
            list.AddRange(getNetworkOrderBytes(urlbytes.Length));
            list.AddRange(urlbytes);
            list.AddRange(getNetworkOrderBytes(ticksbytes.Length));
            list.AddRange(ticksbytes);
            return list.ToArray();
        }

        public static string Sign(byte[] data)
        {
            using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider(2048))
            {
                provider.ImportCspBlob(
                    Convert.FromBase64String(Constants.ProofKey));
                var signed = provider.SignData(data, "SHA256");
                return Convert.ToBase64String(signed);
            }
        }

        public static string Transform(string toBeTransFormed, string transformationFile)
        {
            string output = string.Empty;

            if (string.IsNullOrEmpty(toBeTransFormed))
            {
                return output;
            }

            using (StringReader srt = new StringReader(transformationFile))
            {
                using (StringReader sri = new StringReader(toBeTransFormed))
                {
                    using (XmlReader xrt = XmlReader.Create(srt))
                    {
                        using (XmlReader xri = XmlReader.Create(sri))
                        {
                            XslCompiledTransform xslt = new XslCompiledTransform();
                            xslt.Load(xrt);
                            using (StringWriter sw = new StringWriter())
                            {
                                using (XmlWriter xwo = XmlWriter.Create(sw, xslt.OutputSettings))
                                {
                                    xslt.Transform(xri, xwo);
                                    output = sw.ToString();
                                }
                            }
                        }
                    }
                }
            }

            return output;
        }

        private static byte[] getNetworkOrderBytes(int i)
        {
            return BitConverter.GetBytes(IPAddress.HostToNetworkOrder(i));
        }

        private static byte[] getNetworkOrderBytes(long i)
        {
            return BitConverter.GetBytes(IPAddress.HostToNetworkOrder(i));
        }
    }
}