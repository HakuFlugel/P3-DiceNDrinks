using System;
using System.IO;
using System.Threading.Tasks;
using Android.Content;
using Android.Content.Res;
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
        public static void LoadData<T>(string file, out T type)
        {
            string path = Android.OS.Environment.ExternalStorageDirectory.Path + "/DnD";

            if (!File.Exists(path + "/" + file))
            {
                type = default(T);
                Log.WriteLine(LogPriority.Warn, $"X:{typeof(AndroidShared)}",$"Could not find file: {file}");
                return;
            }
            var filename = Path.Combine(path, file);

            string input = File.ReadAllText(filename);

            type = JsonConvert.DeserializeObject<T>(input);
        }

        public async Task<BitmapFactory.Options> GetBitmapOptionsOfImageAsync(Context context, string image)
        {
            BitmapFactory.Options options = new BitmapFactory.Options
            {
                InJustDecodeBounds = true
            };
            // The result will be null because InJustDecodeBounds == true.

            Stream file = File.Open(Android.OS.Environment.ExternalStorageDirectory.Path + $"/DnD/images/{image}", FileMode.Open);
            
            await BitmapFactory.DecodeStreamAsync(file, new Rect(), options);

            return options;
        }

        public async Task<Bitmap> LoadScaledDownBitmapForDisplayAsync(Context context, string image, BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            // Calculate inSampleSize
            options.InSampleSize = CalculateInSampleSize(options, reqWidth, reqHeight);

            // Decode bitmap with inSampleSize set
            options.InJustDecodeBounds = false;

            Stream file = File.Open(Android.OS.Environment.ExternalStorageDirectory.Path + $"/DnD/images/{image}", FileMode.Open);

            return await BitmapFactory.DecodeStreamAsync(file, new Rect(), options);
        }

        public static int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
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

        //placeholders for methods to get images
        public async void GetImagesFromAssets(Context contex, string image, View view, int id, int[] sizes)
        {
            //sizes[0] is reqWidth sizes[1] is reqHeight
            AssetManager am = contex.Assets;
            const string fileNotFound = "nopic.jpg";
            try
            {
                am.Open(image);

            }
            catch (Exception)
            {
                Stream stream = am.Open(fileNotFound);

                BitmapFactory.Options FNF = new BitmapFactory.Options();

                Bitmap bitmapFNF = await BitmapFactory.DecodeStreamAsync(stream, new Rect(), FNF);

                view.FindViewById<ImageView>(id).SetImageBitmap(bitmapFNF);
                return;
            }

            BitmapFactory.Options options = await GetBitmapOptionsOfImageAsync(contex, image);
            Bitmap bitmapToDisplay =
                await LoadScaledDownBitmapForDisplayAsync(contex, image, options, sizes[0], sizes[1]);
            view.FindViewById<ImageView>(id).SetImageBitmap(bitmapToDisplay);

        }


    }
}