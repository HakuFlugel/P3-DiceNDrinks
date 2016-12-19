using System.IO;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using System;

namespace AdministratorPanel {
    public class ImageDownloader {

        private string bggId;
        private string url;
        public Image image { get; private set; }
        public string ImagePath = "";

        public ImageDownloader(string bggIdIn, string urlIn ) {
            this.bggId = bggIdIn;
            this.url = "http:" + urlIn;
            DownloadImage();
            ImagePath = $"{bggId}.png";
        }

        public void DownloadImage() {

            if (Uri.IsWellFormedUriString(url, UriKind.Absolute)) {
                WebRequest request = WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                Bitmap bitmap = new Bitmap(responseStream);
                image = (Image)bitmap;
            } else {
                image = null;
            }
        }
    }
}
