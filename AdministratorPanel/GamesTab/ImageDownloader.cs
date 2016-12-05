using System.IO;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;

namespace AdministratorPanel {
    public class ImageDownloader {

        private string gddId;
        private string url;
        public Image image { get; private set; }

        public ImageDownloader(string gddIdIn, string urlIn ) {
            this.gddId = gddIdIn;
            this.url = "http:" + urlIn;
            DownloadImage();
        }

        public void DownloadImage() {

            try {
                using (WebClient client = new WebClient()) {
                    byte[] data = client.DownloadData(url);

                    using (MemoryStream memoryStream = new MemoryStream(data)) {
                        using (Image imageIn = Image.FromStream(memoryStream)) {
                            image = imageIn;
                        }
                    }
                }
            } catch (System.Exception) {
                image = null;
            }     
        }

        public void saveImage() {
            image.Save($"Images/{gddId}.png", ImageFormat.Jpeg);
        }
    }
}
