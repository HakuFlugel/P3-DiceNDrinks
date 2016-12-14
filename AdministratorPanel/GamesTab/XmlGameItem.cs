using System;
using System.Drawing;
using System.Windows.Forms;
using Shared;
using System.Text;

namespace AdministratorPanel {
    public class XmlGameItem : NiceButton {
        private Game game;
        private GamePopupBox gamePopupbox;
        private ImageDownloader imageDownloader;

        private TableLayoutPanel tableLayoutPanel = new TableLayoutPanel() {
            ColumnCount = 1,
            RowCount = 2
        };

        private Panel ImagePanel = new Panel() {
            Dock = DockStyle.Left,
            Name = "Image",
            Height = 128,
            Width =  128,
            BackColor = Color.Black,
            BackgroundImageLayout = ImageLayout.Zoom
        };

        public XmlGameItem(Game game, GamePopupBox gamePopupbox) {
            RowCount = 1;
            this.gamePopupbox = gamePopupbox;
            this.game = game;
            ColumnCount = 2;
            bgColor = Color.LightGray;
            Dock = DockStyle.Top;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowOnly;
            Margin = new Padding(5, 4, 20, 5);
            MinimumSize = new Size(256, 128);
         
            tableLayoutPanel.Controls.Add(new Label { Text = game.name, AutoSize = true, Dock = DockStyle.Left, Font = new Font("Arial", 15) });
            tableLayoutPanel.Controls.Add(new Label { Text = game.publishedYear.ToString(), AutoSize = true, Dock = DockStyle.Left, Font = new Font("Arial", 15) });


            imageDownloader = new ImageDownloader(game.bggid,game.imageName);
            ImagePanel.BackgroundImage = imageDownloader.image;


            Controls.Add(ImagePanel);
            Controls.Add(tableLayoutPanel);
        }

        protected override void OnClick(EventArgs e) {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Name:           " + game.name);
            sb.AppendLine("Id:             " + game.id);
            sb.AppendLine("Bggid:          " + game.bggid);
            sb.AppendLine("Year published: " + game.publishedYear);
            sb.AppendLine("Players:        " + game.minPlayers + "/" + game.maxPlayers);
            sb.AppendLine("Time:           " + game.minPlayTime + "/" + game.maxPlayTime);
            sb.AppendLine("Description:    " + game.description);

            if (NiceMessageBox.Show(sb.ToString(),game.name,MessageBoxButtons.YesNo) == DialogResult.Yes) {

                gamePopupbox.gameName.Text = game.name;
                gamePopupbox.gameDescription.Text = game.description;
                gamePopupbox.timeBox.Text = game.minPlayTime.ToString() + "/" + game.maxPlayTime.ToString();
                gamePopupbox.playerBox.Text = game.minPlayers.ToString() + "/" + game.maxPlayers.ToString();
                gamePopupbox.gameImage.BackgroundImage = imageDownloader.image;
                gamePopupbox.imagePath = imageDownloader.ImagePath;
                gamePopupbox.gameDifficultyBar.Value = game.difficulity;
                gamePopupbox.image = ImagePanel.BackgroundImage;
                //imageDownloader.saveImage();
                gamePopupbox.hasBeenChanged = true;

                gamePopupbox.game = game;
                game.imageName = imageDownloader.ImagePath;

            }
            base.OnClick(e);
        }
    }
}