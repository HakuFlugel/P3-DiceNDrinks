using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using IOException = System.IO.IOException;

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