using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
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
        public static void LoadData<T>(Context context, string file, out T type)
        {
            string path = string.Empty;
            string input = string.Empty;

            if (typeof(T) == typeof(Reservation))
            {
                path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
                if (!File.Exists(path + "/" + file)) //eg. /VirtualServerReservation.json
                {
                    type = default(T);
                    return;
                }
                var filename = Path.Combine(path, file); //eg. VirtualServerReservation.json

                input = File.ReadAllText(filename);
            }
            else
            {
                AssetManager am = context.Assets;

                using (StreamReader sr = new StreamReader(am.Open(file)))
                {
                    input = sr.ReadToEnd();
                }
            }


            type = JsonConvert.DeserializeObject<T>(input);
        }
    }
}