using System;
using System.IO;
using System.Threading.Tasks;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Shared;
using File = System.IO.File;
using Path = System.IO.Path;

namespace AndroidAppV2
{
    public class AndroidShared
    {
        public static void LoadData<T>(Context context, string file, out T type)
        {
            string input;

            AssetManager am = context.Assets;

            using (StreamReader sr = new StreamReader(am.Open(file)))
            {
                input = sr.ReadToEnd();
            }

            type = JsonConvert.DeserializeObject<T>(input);
        }

        public static void LoadSavedData<T>(Context context, string file, out T type)
        {

            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (!File.Exists(path + "/" + file)) //eg. /VirtualServerReservation.json
            {
                type = default(T);
                return;
            }
            var filename = Path.Combine(path, file); //eg. VirtualServerReservation.json

            string input = File.ReadAllText(filename);



            type = JsonConvert.DeserializeObject<T>(input);
        }


        public async Task<BitmapFactory.Options> GetBitmapOptionsOfImageAsync(Context context, string path)
        {
            BitmapFactory.Options options = new BitmapFactory.Options
            {
                InJustDecodeBounds = true
            };
            // The result will be null because InJustDecodeBounds == true.

            Stream file = context.Assets.Open(path);
            
            await BitmapFactory.DecodeStreamAsync(file, new Rect(), options);

            return options;
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

        public async Task<Bitmap> LoadScaledDownBitmapForDisplayAsync(Context context, string path, BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            // Calculate inSampleSize
            options.InSampleSize = CalculateInSampleSize(options, reqWidth, reqHeight);

            // Decode bitmap with inSampleSize set
            options.InJustDecodeBounds = false;

            Stream file = context.Assets.Open(path);

            return await BitmapFactory.DecodeStreamAsync(file, new Rect(), options);
        }


        //placeholder for a method to get images
        public async void GetImages(Context contex, string image, View view, int id, int[] sizes)
        {
            //sizes[0] is reqWidth sizes[1] is reqHeight
            AssetManager am = contex.Assets;
            string fileNotFound = "nopic.jpg";
            try
            {
                am.Open(image);

            }
            catch (Exception)
            {
                BitmapFactory.Options fnFoptions = await GetBitmapOptionsOfImageAsync(contex, fileNotFound);
                Bitmap fnFbitmapToDisplay =
                await LoadScaledDownBitmapForDisplayAsync(contex, fileNotFound, fnFoptions, sizes[0], sizes[1]);
                view.FindViewById<ImageView>(id).SetImageBitmap(fnFbitmapToDisplay);
                return;
            }

            BitmapFactory.Options options = await GetBitmapOptionsOfImageAsync(contex, image);
            Bitmap bitmapToDisplay =
                await LoadScaledDownBitmapForDisplayAsync(contex, image, options, sizes[0], sizes[1]);
            view.FindViewById<ImageView>(id).SetImageBitmap(bitmapToDisplay);

        }

    }
}