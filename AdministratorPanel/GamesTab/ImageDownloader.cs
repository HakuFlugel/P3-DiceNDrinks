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
        }

        public void DownloadImage() {

            if (Uri.IsWellFormedUriString(url, UriKind.Absolute)) {
                WebRequest request = WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                Bitmap bitmap2 = new Bitmap(responseStream);
                image = (Image)bitmap2;
            }
        }

        public void saveImage() {
            ImagePath = $"{gddId}.png";

            if (File.Exists($"Images/{ImagePath}")) {
                File.Delete($"Images/{ImagePath}");
            }
            image.Save($"Images/{ImagePath}", ImageFormat.Jpeg);
        }
    }
}
