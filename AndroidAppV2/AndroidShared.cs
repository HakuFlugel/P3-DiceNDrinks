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

namespace AndroidAppV2
{
    public class AndroidShared
    {
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

        public async void GetImagesFromSD(Context contex, string image, View view, int id, int[] sizes) {
            //sizes[0] is reqWidth sizes[1] is reqHeight

            string filename = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path, $"DnD/images/{image}");

            if (File.Exists(filename)) {
                BitmapFactory.Options options = await GetBitmapOptionsOfImage(filename);

                Bitmap bitmapToDisplay =
                    await LoadScaledDownBitmapForDisplayAsync(filename, options, sizes[0], sizes[1]);

                view.FindViewById<ImageView>(id).SetImageBitmap(bitmapToDisplay);
            }
            else {
                Stream stream = contex.Assets.Open("nopic.jpg");

                BitmapFactory.Options fileNotFound = new BitmapFactory.Options();

                Bitmap bitmapFileNotFound = await BitmapFactory.DecodeStreamAsync(stream, new Rect(), fileNotFound);

                view.FindViewById<ImageView>(id).SetImageBitmap(bitmapFileNotFound);
            }
        }

        public async void GetImagesFromResources(Context contex, Resources res, int resId, View view, int viewId,
            int[] sizes) {
            BitmapFactory.Options options = await GetBitmapOptionsOfImageFromRes(res, resId);

            Bitmap bitmapToDisplay =
                await LoadScaledDownBitmapForDisplayFromResAsync(res, resId, options, sizes[0], sizes[1]);

            view.FindViewById<ImageView>(viewId).SetImageBitmap(bitmapToDisplay);
        }

        public async void GetImagesFromAssets(Context contex, string image, View view, int id, int[] sizes) {
            //sizes[0] is reqWidth sizes[1] is reqHeight
            AssetManager am = contex.Assets;
            string fileNotFound = "nopic.jpg";
            try {
                am.Open(image);
            }
            catch (Exception) {
                BitmapFactory.Options fnFoptions = await GetBitmapOptionsOfImageFromAssetsAsync(contex, fileNotFound);
                Bitmap fnFbitmapToDisplay =
                    await
                        LoadScaledDownBitmapForDisplayFromAssetsAsync(contex, fileNotFound, fnFoptions, sizes[0],
                            sizes[1]);
                view.FindViewById<ImageView>(id).SetImageBitmap(fnFbitmapToDisplay);
                return;
            }

            BitmapFactory.Options options = await GetBitmapOptionsOfImageFromAssetsAsync(contex, image);
            Bitmap bitmapToDisplay =
                await LoadScaledDownBitmapForDisplayFromAssetsAsync(contex, image, options, sizes[0], sizes[1]);
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

        private static async Task<BitmapFactory.Options> GetBitmapOptionsOfImage(string image)
        {
            BitmapFactory.Options options = new BitmapFactory.Options
            {
                InJustDecodeBounds = true
            };
            // The result will be null because InJustDecodeBounds == true.

            await BitmapFactory.DecodeFileAsync(image, options);

            return options;
        }

        private static async Task<Bitmap> LoadScaledDownBitmapForDisplayAsync(string image,
            BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            // Calculate inSampleSize
            options.InSampleSize = CalculateInSampleSize(options, reqWidth, reqHeight);

            // Decode bitmap with inSampleSize set
            options.InJustDecodeBounds = false;

            return await BitmapFactory.DecodeFileAsync(image, options);
        }

        private static async Task<BitmapFactory.Options> GetBitmapOptionsOfImageFromRes(Resources res, int id)
        {
            BitmapFactory.Options options = new BitmapFactory.Options
            {
                InJustDecodeBounds = true
            };
            // The result will be null because InJustDecodeBounds == true.

            await BitmapFactory.DecodeResourceAsync(res, id);


            return options;
        }

        private static async Task<Bitmap> LoadScaledDownBitmapForDisplayFromResAsync(Resources res, int id,
            BitmapFactory.Options options, int reqWidth, int reqHeight) {
            // Calculate inSampleSize
            options.InSampleSize = CalculateInSampleSize(options, reqWidth, reqHeight);

            // Decode bitmap with inSampleSize set
            options.InJustDecodeBounds = false;

            return await BitmapFactory.DecodeResourceAsync(res, id, options);
        }

        private static async Task<BitmapFactory.Options> GetBitmapOptionsOfImageFromAssetsAsync(Context context,
            string path) {
            BitmapFactory.Options options = new BitmapFactory.Options
            {
                InJustDecodeBounds = true
            };
            // The result will be null because InJustDecodeBounds == true.

            Stream file = context.Assets.Open(path);

            await BitmapFactory.DecodeStreamAsync(file, new Rect(), options);

            return options;
        }

        private static async Task<Bitmap> LoadScaledDownBitmapForDisplayFromAssetsAsync(Context context, string path,
            BitmapFactory.Options options, int reqWidth, int reqHeight) {
            // Calculate inSampleSize
            options.InSampleSize = CalculateInSampleSize(options, reqWidth, reqHeight);

            // Decode bitmap with inSampleSize set
            options.InJustDecodeBounds = false;

            Stream file = context.Assets.Open(path);

            return await BitmapFactory.DecodeStreamAsync(file, new Rect(), options);
        }

        public List<Product> DownloadProductsAsObject() {
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

        public List<T> DownloadItemAsObject<T>(string type) {
            WebClient client = new WebClient();
            byte[] resp = client.UploadValues("http://172.25.11.113" + "/get.aspx",
                new NameValueCollection
                {
                    {"Type", type} });
            string result = System.Text.Encoding.UTF8.GetString(resp);

            return JsonConvert.DeserializeObject<List<T>>(result);
        }


        public string DownloadProducts() {
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

        public string DownloadItem(string type) {
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
    }
}