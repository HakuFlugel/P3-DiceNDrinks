using System;
using System.Drawing;
using System.IO;

namespace Shared
{
    public static class ImageHelper
    {
        public static string imageToBase64(Image image)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms,System.Drawing.Imaging.ImageFormat.Png);

            return Convert.ToBase64String(ms.ToArray());
        }

        public static Image byteArrayToImage(string base64)
        {
            MemoryStream ms = new MemoryStream(Convert.FromBase64String(base64));
            return Image.FromStream(ms);
        }
    }
}