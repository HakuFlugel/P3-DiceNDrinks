using System;
using System.Drawing;
using System.Windows.Forms;
using Shared;
using System.Text;

namespace AdministratorPanel {
    public class XmlGameItem : NiceButton {
        Game game;
        GamePopupBox gamePopupbox;
        ImageDownloader imageDownloader;

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

            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.ColumnCount = 1;
            tableLayoutPanel.RowCount = 2;
            tableLayoutPanel.Controls.Add(new Label { Text = game.name, AutoSize = true, Dock = DockStyle.Left, Font = new Font("Arial", 15) });
            tableLayoutPanel.Controls.Add(new Label { Text = game.publishedYear.ToString(), AutoSize = true, Dock = DockStyle.Left, Font = new Font("Arial", 15) });

            Console.WriteLine("id = " + game.bggid + "  image = " + game.imageName);
            imageDownloader = new ImageDownloader(game.bggid,game.imageName);

            if (imageDownloader.image == null) {
                Console.WriteLine("Image not found ");
            }

            Panel pn = new Panel();
            pn.Dock = DockStyle.Left;
            pn.Name = "Image";
            pn.Height = 128;
            pn.Width =  128;
            pn.BackColor = Color.Black;
            pn.BackgroundImage = imageDownloader.image;
            pn.BackgroundImageLayout = ImageLayout.Zoom;


            Controls.Add(pn);
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

            if (MessageBox.Show(sb.ToString(),game.name,MessageBoxButtons.YesNo) == DialogResult.Yes) {

                gamePopupbox.gameName.Text = game.name;
                gamePopupbox.gameDescription.Text = game.description;
                gamePopupbox.time.Text = game.minPlayTime.ToString() + "/" + game.maxPlayTime.ToString();
                gamePopupbox.players.Text = game.minPlayers.ToString() + "/" + game.maxPlayTime.ToString();
                gamePopupbox.gameImage.BackgroundImage = imageDownloader.image;
                gamePopupbox.imagePath = imageDownloader.ImagePath;
                gamePopupbox.gameDifficulty.Value = game.difficulity;

                imageDownloader.saveImage();
                gamePopupbox.hasBeenChanged = true;

                gamePopupbox.game = game;
                game.imageName = imageDownloader.ImagePath;

            }
            base.OnClick(e);
        }
    }
}