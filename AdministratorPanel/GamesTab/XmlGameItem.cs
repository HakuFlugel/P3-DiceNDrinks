using System;
using System.Drawing;
using System.Windows.Forms;
using Shared;
using System.Text;

namespace AdministratorPanel {
    public class XmlGameItem : NiceButton {
        Game game;
        GamePopupBox gamePopupbox;

        public XmlGameItem(Game game, GamePopupBox gamePopupbox) {
            RowCount = 1;
            this.gamePopupbox = gamePopupbox;
            this.game = game;
            ColumnCount = 1;
            bgColor = Color.LightGray;
            Dock = DockStyle.Top;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowOnly;
            Margin = new Padding(5, 4, 20, 5);
            MaximumSize = new Size(190, 60);

            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.ColumnCount = 1;
            tableLayoutPanel.RowCount = 2;
            tableLayoutPanel.Controls.Add(new Label { Text = game.name, AutoSize = true, Dock = DockStyle.Left, Font = new Font("Arial", 15) });
            tableLayoutPanel.Controls.Add(new Label { Text = game.bggid, AutoSize = true, Dock = DockStyle.Left, Font = new Font("Arial", 15) });

            ImageDownloader imageDownloader = new ImageDownloader(game.bggid,game.imageName);

            Panel pn = new Panel();
            pn.Name = "Image";
            pn.Height = 86;
            pn.Width = ClientSize.Width;
            try {
                pn.BackgroundImage = imageDownloader.DownloadImage();
            } catch (Exception) {

                throw;
            }

           
            pn.BackgroundImageLayout = ImageLayout.Zoom;
            pn.BackColor = Color.Black;
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
                gamePopupbox.game.bggid = game.bggid;
                gamePopupbox.gameDescription.Text = game.description;
                gamePopupbox.time.Text = game.minPlayTime.ToString() + "/" + game.maxPlayTime.ToString();
                gamePopupbox.players.Text = game.minPlayers.ToString() + "/" + game.maxPlayTime.ToString();

                gamePopupbox.gameDifficulty.Value = game.difficulity;
                
                gamePopupbox.hasBeenChanged = true;
            }
            base.OnClick(e);
        }
    }
}