using System;
using System.Collections.Specialized;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace AdministratorPanel
{

    public class ServerConnection
    {
        private class WebClientThatHasTimeout : WebClient
        {
            protected override WebRequest GetWebRequest(Uri address)
            {
                WebRequest wr = base.GetWebRequest(address);
                wr.Timeout = 2000;
                return wr;
            }
        }


        private const string protocol = "http://";
        public static string ip = "localhost";

        public static string AdminKey = "weeb"; //TODO: stuff

        //public ServerConnection(string ip)
        //{
        //    this.ip = ip;
        //}

        public static string sendRequest(string page, NameValueCollection valueCollection)
        {

            try
            {
                WebClient client = new WebClientThatHasTimeout();
                valueCollection.Add("AdminKey", AdminKey);

                byte[] resp = client.UploadValues(protocol + ip + page, valueCollection);

                return System.Text.Encoding.Default.GetString(resp);
            }
            catch (Exception e)
            {
                MessageBox.Show(null, e.Message, "Connection Error");
                return "exception " + e.Message;
            }

        }

//        public static string sendImageAsync(string page, string filePath)
//        {
//            try
//            {
//                WebClient client = new WebClientThatHasTimeout();
//                valueCollection.Add("AdminKey", AdminKey);
//                client.UploadFileAsync(page, filePath);
//                client.
//                byte[] resp = client.UploadValues(protocol + ip + page, valueCollection);
//
//                return System.Text.Encoding.Default.GetString(resp);
//            }
//            catch (Exception e)
//            {
//                MessageBox.Show(null, e.Message, "Connection Error");
//                return "exception " + e.Message;
//            }
//        }
    }


}