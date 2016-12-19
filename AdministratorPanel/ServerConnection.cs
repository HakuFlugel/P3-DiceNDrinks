using System;
using System.Collections.Specialized;
using System.Net;
using System.Windows.Forms;

namespace AdministratorPanel
{

    public class ServerConnection
    {
        private class WebClientThatHasTimeout : WebClient
        {
            protected override WebRequest GetWebRequest(Uri address)
            {
                WebRequest wr = base.GetWebRequest(address);
                wr.Timeout = 750;
                return wr;
            }
        }


        private const string protocol = "http://";
        public static string ip = "localhost";

        public static string AdminKey = "string12398754352352527662424123";

        public static string sendRequest(string page, NameValueCollection valueCollection)
        {

            try
            {
                WebClient client = new WebClientThatHasTimeout();
                valueCollection.Add("AdminKey", AdminKey);

                byte[] resp = client.UploadValues(protocol + ip + page, valueCollection);

                Console.WriteLine("Request Done");

                return System.Text.Encoding.Default.GetString(resp);
            }
            catch (Exception e)
            {
                MessageBox.Show(null, e.Message, "Connection Error");
                return "exception " + e.Message;
            }
        }

    }


}