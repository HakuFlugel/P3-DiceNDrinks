using System;
using System.IO;
using System.Threading.Tasks;

using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;

using Newtonsoft.Json;
using File = System.IO.File;
using Path = System.IO.Path;

namespace AndroidAppV2
{
    public class AndroidShared
    {
        public static void LoadData<T>(Context context, string file, out T type)
        {
            string path = Android.OS.Environment.ExternalStorageDirectory.Path + "/DnD";

            var filename = Path.Combine(path, file);

            if (!File.Exists(filename))
            {
                File.Create(filename);
                type = default(T);
                Log.WriteLine(LogPriority.Warn, $"X:{context}", $"Could not find file: {file} on path {filename}, creating new");
                return;
            }


            string input = File.ReadAllText(filename);
            try
            {
                type = JsonConvert.DeserializeObject<T>(input);
            }
            catch (Exception) //empty json container
            {
                Log.WriteLine(LogPriority.Info, $"X:{context}", $"Could not find data in file: {file} on path {filename}. If this happens to any other than reservation then it's fucked");
                type = default(T);
            }

        }

        private async Task<BitmapFactory.Options> GetBitmapOptionsOfImage(string image)
        {
            BitmapFactory.Options options = new BitmapFactory.Options
            {
                InJustDecodeBounds = true
            };
            // The result will be null because InJustDecodeBounds == true.

            await BitmapFactory.DecodeFileAsync(image, options);

            return options;
        }

        private async Task<Bitmap> LoadScaledDownBitmapForDisplayAsync(string image, BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            // Calculate inSampleSize
            options.InSampleSize = CalculateInSampleSize(options, reqWidth, reqHeight);

            // Decode bitmap with inSampleSize set
            options.InJustDecodeBounds = false;

            return await BitmapFactory.DecodeFileAsync(image, options);
        }

        private static int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            // Raw height and width of image
            float height = options.OutHeight;
            float width = options.OutWidth;
            double inSampleSize = 1D;

            if (height > reqHeight || width > reqWidth)
            {
                int halfHeight = (int)(height / 2);
                int halfWidth = (int)(width / 2);

                // Calculate a inSampleSize that is a power of 2 - the decoder will use a value that is a power of two anyway.
                while ((halfHeight / inSampleSize) > reqHeight && (halfWidth / inSampleSize) > reqWidth)
                {
                    inSampleSize *= 2;
                }
            }

            return (int)inSampleSize;
        }

        public async void GetImages(Context contex, string image, View view, int id, int[] sizes)
        {
            //sizes[0] is reqWidth sizes[1] is reqHeight

            string filename = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path, $"DnD/images/{image}");

            if (File.Exists(filename))
            {
                BitmapFactory.Options options = await GetBitmapOptionsOfImage(filename);

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

    }
}