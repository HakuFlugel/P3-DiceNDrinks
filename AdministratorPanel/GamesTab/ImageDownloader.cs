using System.IO;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using System;

namespace AdministratorPanel {
    public class ImageDownloader {

        private string gddId;
        private string url;
        public Image image { get; private set; }
        public string ImagePath = "";

        public ImageDownloader(string gddIdIn, string urlIn ) {
            this.gddId = gddIdIn;
            this.url = "http:" + urlIn;
            DownloadImage();
            ImagePath = $"{gddId}.png";
        }

        public void DownloadImage() {

            if (Uri.IsWellFormedUriString(url, UriKind.Absolute)) {
                Console.WriteLine("ich bist url");
                WebRequest request = WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                Bitmap bitmap = new Bitmap(responseStream);
                image = (Image)bitmap;
            } else {
                Console.WriteLine("fuck asger");
                image = null;
            }
        }

        public void saveImage() {
            
            if (image != null) {
                image.Save($"Images/{ImagePath}", ImageFormat.Png);
            }
        }
    }
}
