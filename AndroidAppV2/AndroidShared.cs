using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Shared;

namespace AndroidAppV2
{
    public static class AndroidShared
    {
        public static void LoadData<T>(string file, out T type)
        {

            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            if (!File.Exists(path + "/" + file)) //eg. /VirtualServerReservation.json
            {
                type = default(T);
                return;
            }
            var filename = Path.Combine(path, file); //eg. VirtualServerReservation.json

            string input = File.ReadAllText(filename);

            type = JsonConvert.DeserializeObject<T>(input);
        }
    }
}