using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Net;

using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;

using Newtonsoft.Json;
using File = System.IO.File;
using Path = System.IO.Path;
using System.Collections.Generic;
using Shared;
using System.Net.NetworkInformation;
using Android.App;

namespace AndroidAppV2 {
    public class AndroidShared {

        public static void LoadData<T>(Context context, string file, out T type)
        {
            string path = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path, "DnD");

            string filename = Path.Combine(path, file);

            if (!File.Exists(filename))
            {
                Log.WriteLine(LogPriority.Warn, $"X:{context}",
                    $"Could not find file: {file} on path {filename}, creating new");
                File.Create(filename);
                type = default(T);
                return;
            }


            string input = File.ReadAllText(filename);

            try
            {
                type = JsonConvert.DeserializeObject<T>(input);
            }
            catch (Exception) //empty json container
            {
                Log.WriteLine(LogPriority.Warn, $"X:{context}",
                    $"Could not find data in file: {file} on path {filename} of type {typeof(T)}.");
                type = default(T);
            }
        }
        //from SD
        public async void GetImages(string image, View view, int id, int[] sizes, Context contex) {
            //sizes[0] is reqWidth sizes[1] is reqHeight

            string filename = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path, $"DnD/images/{image}");

            if (File.Exists(filename))
            {
                BitmapFactory.Options options = await GetBitmapOptionsOfImageAsync(filename);

                Bitmap bitmapToDisplay =
                    await LoadScaledDownBitmapForDisplayAsync(filename, options, sizes[0], sizes[1]);

                view.FindViewById<ImageView>(id).SetImageBitmap(bitmapToDisplay);
            }
            else
            {
                Stream stream = contex.Assets.Open("nopic.jpg");

                BitmapFactory.Options fileNotFound = new BitmapFactory.Options();

                Bitmap bitmapFileNotFound = await BitmapFactory.DecodeStreamAsync(stream, new Rect(), fileNotFound);

                view.FindViewById<ImageView>(id).SetImageBitmap(bitmapFileNotFound);
            }
        }
        //from res 
        public async void GetImages(Context contex, Resources res, int resId, View view, int viewId,
            int[] sizes) {
            BitmapFactory.Options options = await GetBitmapOptionsOfImageAsync(res, resId);

            Bitmap bitmapToDisplay =
                await LoadScaledDownBitmapForDisplayAsync(res, resId, options, sizes[0], sizes[1]);

            view.FindViewById<ImageView>(viewId).SetImageBitmap(bitmapToDisplay);
        }
        //from assets
        public async void GetImages(Context contex, string image, View view, int id, int[] sizes) {
            //sizes[0] is reqWidth sizes[1] is reqHeight
            AssetManager am = contex.Assets;
            try {
                am.Open(image);
            }
            catch (Exception) {
                Stream stream = contex.Assets.Open("nopic.jpg");

                BitmapFactory.Options fileNotFound = new BitmapFactory.Options();

                Bitmap bitmapFileNotFound = await BitmapFactory.DecodeStreamAsync(stream, new Rect(), fileNotFound);

                view.FindViewById<ImageView>(id).SetImageBitmap(bitmapFileNotFound);
                return;
            }

            BitmapFactory.Options options = await GetBitmapOptionsOfImageAsync(contex, image);
            Bitmap bitmapToDisplay =
                await LoadScaledDownBitmapForDisplayAsync(contex, image, options, sizes[0], sizes[1]);
            view.FindViewById<ImageView>(id).SetImageBitmap(bitmapToDisplay);
        }

        private static int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight) {

            float height = options.OutHeight;
            float width = options.OutWidth;
            double inSampleSize = 1D;

            if (height > reqHeight || width > reqWidth) {
                int halfHeight = (int) (height/2);
                int halfWidth = (int) (width/2);

                while ((halfHeight/inSampleSize) > reqHeight && (halfWidth/inSampleSize) > reqWidth) {
                    inSampleSize *= 2;
                }
            }
            return (int)inSampleSize;
        }
        //from sd
        private static async Task<BitmapFactory.Options> GetBitmapOptionsOfImageAsync(string image)
        {
            BitmapFactory.Options options = new BitmapFactory.Options
            {
                InJustDecodeBounds = true
            };
            // The result will be null because InJustDecodeBounds == true.

            await BitmapFactory.DecodeFileAsync(image, options);

            return options;
        }
        //from res
        private static async Task<BitmapFactory.Options> GetBitmapOptionsOfImageAsync(Resources res, int id) {
            BitmapFactory.Options options = new BitmapFactory.Options {
                InJustDecodeBounds = true
            };
            // The result will be null because InJustDecodeBounds == true.

            await BitmapFactory.DecodeResourceAsync(res, id);


            return options;
        }
        //from assets
        private static async Task<BitmapFactory.Options> GetBitmapOptionsOfImageAsync(Context context,
            string path) {
            BitmapFactory.Options options = new BitmapFactory.Options {
                InJustDecodeBounds = true
            };
            // The result will be null because InJustDecodeBounds == true.

            Stream file = context.Assets.Open(path);

            await BitmapFactory.DecodeStreamAsync(file, new Rect(), options);

            return options;
        }
        //from sd
        private static async Task<Bitmap> LoadScaledDownBitmapForDisplayAsync(string image,
            BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            // Calculate inSampleSize
            options.InSampleSize = CalculateInSampleSize(options, reqWidth, reqHeight);

            // Decode bitmap with inSampleSize set
            options.InJustDecodeBounds = false;

            return await BitmapFactory.DecodeFileAsync(image, options);
        }
        //from res
        private static async Task<Bitmap> LoadScaledDownBitmapForDisplayAsync(Resources res, int id,
            BitmapFactory.Options options, int reqWidth, int reqHeight) {
            // Calculate inSampleSize
            options.InSampleSize = CalculateInSampleSize(options, reqWidth, reqHeight);

            // Decode bitmap with inSampleSize set
            options.InJustDecodeBounds = false;

            return await BitmapFactory.DecodeResourceAsync(res, id, options);
        }
        //from assets
        private static async Task<Bitmap> LoadScaledDownBitmapForDisplayAsync(Context context, string path,
            BitmapFactory.Options options, int reqWidth, int reqHeight) {
            // Calculate inSampleSize
            options.InSampleSize = CalculateInSampleSize(options, reqWidth, reqHeight);

            // Decode bitmap with inSampleSize set
            options.InJustDecodeBounds = false;

            Stream file = context.Assets.Open(path);

            return await BitmapFactory.DecodeStreamAsync(file, new Rect(), options);
        }

        public static List<Product> DownloadProductsAsObject() {
            WebClient client = new WebClient();
            byte[] resp = client.UploadValues("http://172.25.11.113" + "/get.aspx",
                new NameValueCollection {
                    {"Type", "Products"} });
            string result = System.Text.Encoding.UTF8.GetString(resp);
            Tuple<List<ProductCategory>, List<Product>> values =
                JsonConvert.DeserializeObject<Tuple<List<ProductCategory>, List<Product>>>(result);

            List<Product> newlist = values.Item2;

            return newlist;
        }

        public static T DownloadItemAsObject<T>(string type) {
            WebClient client = new WebClient();
            byte[] resp = client.UploadValues("http://172.25.11.113" + "/get.aspx",
                new NameValueCollection
                {
                    {"Type", type} });
            string result = System.Text.Encoding.UTF8.GetString(resp);

            return JsonConvert.DeserializeObject<T>(result);
        }

        public static string DownloadProducts() {
            WebClient client = new WebClient();
            byte[] resp = client.UploadValues("http://172.25.11.113" + "/get.aspx",
                new NameValueCollection {
                    {"Type", "Products"} });
            string result = System.Text.Encoding.UTF8.GetString(resp);
            Tuple<List<ProductCategory>, List<Product>> values =
                JsonConvert.DeserializeObject<Tuple<List<ProductCategory>, List<Product>>>(result);

            result = JsonConvert.SerializeObject(values.Item2);

            return result;
        }

        public static string DownloadItem(string type) {
            WebClient client = new WebClient();
            byte[] resp = client.UploadValues("http://172.25.11.113" + "/get.aspx",
                new NameValueCollection {
                    {"Type", type} });
            string result = System.Text.Encoding.UTF8.GetString(resp);

            return result;
        }

        public static void ImageDownloader(string id, string category) {
            string url = $"http://172.25.11.113/images/{category}/{id}";
            Bitmap bitmap = DownloadImage(url);
            if (bitmap == null) return;
            SaveImage(bitmap, id);
        }

        private static Bitmap DownloadImage(string url) {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                return null;
            WebRequest request = WebRequest.Create(url);
            try {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                Bitmap bitmap = BitmapFactory.DecodeStream(responseStream);
                return bitmap;
            }
            catch (WebException e) {
                Log.WriteLine(LogPriority.Error, "X:" + e, $"could not compute url: {url}");
                return null;
            }
        }

        private static void SaveImage(Bitmap bitmap, string id) {
            string imagePath = Android.OS.Environment.ExternalStorageDirectory.Path + $"/DnD/images/{id}";

            if (File.Exists(imagePath))
                return;
            using (FileStream os = new FileStream(imagePath, FileMode.CreateNew)) {
                bitmap.Compress(Bitmap.CompressFormat.Png, 75, os);
            }
        }

        public static void ErrorDialog(string message, Context context) {
            AlertDialog.Builder error = new AlertDialog.Builder(context);
            error.SetMessage(message);
            error.SetTitle("Error");
            error.SetPositiveButton(Resource.String.yes, (senderAlert, args) => {
                /*Do Nothing*/
            });
            error.Show();
        }

        public static bool CheckForInternetConnection() {
            Ping ping = new Ping();
            const string address = "172.25.11.113";
            byte[] buffer = new byte[32];
            const int timeout = 1000;
            PingOptions pingOptions = new PingOptions();
            PingReply reply = ping.Send(address, timeout, buffer, pingOptions);
            return reply.Status == IPStatus.Success;
        }

        public static void TextDownloader(string path) {
            string url = $"http://172.25.11.113/{path}";
            string text = DownloadText(url);
            SaveText(text, path);
            
        }

        private static string DownloadText(string url) {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                return null;
            WebRequest request = WebRequest.Create(url);
            try {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string text = reader.ReadToEnd();
                return text;
            }
            catch (WebException e) {
                Log.WriteLine(LogPriority.Error, "X:" + e, $"could not compute url: {url}");
                return null;
            }
        }

        private static void SaveText(string text, string id) {
            string path = Android.OS.Environment.ExternalStorageDirectory.Path + $"/DnD/{id}";
            File.WriteAllText(path, text);
        }

        public static void Update() {
            string[] items = { "Events", "Games", "Products", "AboutUs" };

            Dictionary<string, DateTime> newTimes = DownloadTimestap();
            Dictionary<string, DateTime> oldTimes = LocalTimestap();

            foreach (string item in items) {
                if (!newTimes.ContainsKey(item) || !oldTimes.ContainsKey(item))
                    continue;
                if (newTimes[item].Ticks > oldTimes[item].Ticks)
                    DownloadUpdate(item);
            }
            UpdateTimestap(newTimes);
        }

        private static Dictionary<string, DateTime> DownloadTimestap() {
            return JsonConvert.DeserializeObject<Dictionary<string, DateTime>>(DownloadItem("timestamps"));
        }

        private static Dictionary<string, DateTime> LocalTimestap() {
            string file = File.ReadAllText(Android.OS.Environment.ExternalStorageDirectory.Path + "/DnD" + "/timestamps.json");

            return JsonConvert.DeserializeObject<Dictionary<string, DateTime>>(file);
        }

        private static void UpdateTimestap(Dictionary<string, DateTime> newTimes) {
            File.Delete(Android.OS.Environment.ExternalStorageDirectory.Path + "/DnD" + "/timestamps.json");
            File.WriteAllText(Android.OS.Environment.ExternalStorageDirectory.Path + "/DnD" + "/timestamps.json", JsonConvert.SerializeObject(newTimes));
        }

        private static void DownloadUpdate(string location) {
            string saveLocation = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path, $"{Android.OS.Environment.ExternalStorageDirectory.Path + "/DnD"}/{location}.json");
            string item;

            switch (location) {
                case "Products":
                    item = DownloadProducts();
                    File.WriteAllText(saveLocation, item);
                    DownloadProductImages(item);
                    break;
                case "Games":
                    item = DownloadItem(location);
                    File.WriteAllText(saveLocation, item);
                    DownloadGameImages(item);
                    break;
                case "Events":
                    item = DownloadItem(location);
                    File.WriteAllText(saveLocation, item);
                    break;
                case "AboutUs":
                    item = DownloadItem(location);
                    File.WriteAllText(saveLocation, item);
                    break;
            }
        }

        private static void DownloadGameImages(string jsonlist) {
            List<Game> list = JsonConvert.DeserializeObject<List<Game>>(jsonlist);

            foreach (Game item in list) {
                ImageDownloader(item.imageName, "games");
            }
        }

        private static void DownloadProductImages(string jsonlist) {
            List<Product> list = JsonConvert.DeserializeObject<List<Product>>(jsonlist);

            foreach (Product item in list) {
                ImageDownloader(item.image, "products");
            }
        }
    }
}