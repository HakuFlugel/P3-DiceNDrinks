using System;
using System.Collections.Specialized;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using Newtonsoft.Json;

namespace AdministratorPanel
{

    public class ServerConnection
    {
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
                WebClient client = new WebClient();
                valueCollection.Add("AdminKey", AdminKey);

                byte[] resp = client.UploadValues(protocol + ip + page, valueCollection);

                return System.Text.Encoding.Default.GetString(resp);
            }
            catch (Exception e)
            {
                return "exception " + e.Message;
            }

        }
    }
}