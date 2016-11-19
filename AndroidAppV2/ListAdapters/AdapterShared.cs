using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AndroidAppV2.ListAdapters
{
    public static class AdapterShared
    {
        public static Drawable DLImage(Context context, string path)
        {
            try
            {
                Stream asset = context.Assets.Open(path);

                Drawable d = Drawable.CreateFromStream(asset, null);

                return d;
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}