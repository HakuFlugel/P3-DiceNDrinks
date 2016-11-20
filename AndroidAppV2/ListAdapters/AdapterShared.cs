using System;
using System.IO;

using Android.Content;
using Android.Graphics;


namespace AndroidAppV2.ListAdapters
{
    public static class AdapterShared
    {
        public static Bitmap getBitmapFromAsset(Context context, string filePath)
        {
            Bitmap bitmap = null;
            try
            {
                Stream str = context.Assets.Open(filePath);
                bitmap = BitmapFactory.DecodeStream(str);
            }
            catch (Exception e)
            {
                // handle exception
            }
            if (bitmap != null && bitmap.IsMutable)
            {
                bitmap.Width = 48;
                bitmap.Height = 48;
            }
            return bitmap;
        }
    }
}