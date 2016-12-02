using System.IO;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;

namespace AdministratorPanel {
    public class ImageDownloader {

        private string gddId;
        private string url;

        public ImageDownloader(string gddIdIn, string urlIn ) {
            this.gddId = gddIdIn;
            this.url = urlIn;
        }

        public Image DownloadImage() {
            using (WebClient client = new WebClient()) {

                byte[] data = client.DownloadData("http://"+url);

                using (MemoryStream memoryStream = new MemoryStream(data)) {

                    using (Image image = Image.FromStream(memoryStream)) {
                        return image;
                    }
                }
            }
        }

        public void saveImage(Image image) {
            image.Save("Images/" + gddId + ".Png", ImageFormat.Png);
        }
    }
}
