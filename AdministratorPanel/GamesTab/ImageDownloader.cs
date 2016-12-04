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
            this.url = urlIn;
            DownloadImage();
        }

        public void DownloadImage() {
            using (WebClient client = new WebClient()) {

                byte[] data = client.DownloadData("http:" + url);
                using (MemoryStream memoryStream = new MemoryStream(data)) {
                    //System.Console.WriteLine("http:" + url);
                    using (Image imageIn = Image.FromStream(memoryStream)) {
                        image = imageIn;
                    }
                }
            }
        }

        public void saveImage(Image image) {
            image.Save("Images/" + gddId + ".Png", ImageFormat.Png);
        }
    }
}
