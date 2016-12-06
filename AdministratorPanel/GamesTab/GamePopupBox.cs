using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System;
using Shared;
using System.Drawing.Imaging;
using System.Media;

namespace AdministratorPanel
{

    public class GamePopupBox : FancyPopupBox
    {
        protected TableLayoutPanel lft = new TableLayoutPanel()
        {
            ColumnCount = 1,
            GrowStyle = TableLayoutPanelGrowStyle.AddRows,
            //Dock = DockStyle.Fill
            AutoSize = true
        };

        protected TableLayoutPanel header = new TableLayoutPanel()
        {
            RowCount = 1,
            ColumnCount = 2,
            //Dock = DockStyle.Fill,
            AutoSize = true,
            GrowStyle = TableLayoutPanelGrowStyle.FixedSize
        };

        public NiceTextBox gameName = new NiceTextBox()
        {
            Width = 200,
            waterMark = "Game name",
            Margin = new Padding(5, 10, 20, 10)
        };

        private Button editGenre = new Button()
        {

            Text = "Edit",
            Dock = DockStyle.Right,
            MaximumSize = new Size(50, 17),
        };

        public ToolTip toolTip = new ToolTip()
        {
            AutoPopDelay = 5000,
            InitialDelay = 100,
            ReshowDelay = 500,
            ShowAlways = true

        };

        public NiceTextBox gameDescription = new NiceTextBox()
        {
            Width = 200,
            Height = 100,
            waterMark = "Game Description",
            Multiline = true,
            Margin = new Padding(5, 0, 20, 10)
        };

        public NiceTextBox imageText = new NiceTextBox()
        {
            Width = 130,
            waterMark = "Path to picture",
            Margin = new Padding(5, 0, 0, 20),
            Dock = DockStyle.Left
        };

        public Button imageSeach = new Button()
        {
            BackgroundImage = Image.FromFile("images/seachIcon.png"),
            BackgroundImageLayout = ImageLayout.Zoom,
            Width = 50,
            Margin = new Padding(5, 0, 0, 20),
        };

        public NiceTextBox yearPublished = new NiceTextBox()
        {
            Width = 200,
            waterMark = "Year published",
            Margin = new Padding(5, 10, 20, 10)
        };

        public Panel gameImage = new Panel()
        {
            Width = 200,
            Height = 200,
            Margin = new Padding(5, 0, 0, 0),
            BackgroundImage = Image.FromFile("images/_default.png"),
            BackgroundImageLayout = ImageLayout.Zoom,
        };

        public NiceTextBox time = new NiceTextBox()
        {
            Name =
                "minimum / maximum players" + Environment.NewLine + "it takes to play the game." + Environment.NewLine +
                "eg: 3/10",
            waterMark = "min / max players",
            Width = 95,
            Margin = new Padding(0, 0, 0, 20)
        };

        public NiceTextBox players = new NiceTextBox()
        {
            Name =
                "minimum / maximum time " + Environment.NewLine + "it takes to complete the game." + Environment.NewLine +
                "eg: 30/60",
            waterMark = "min / max time",
            Width = 95,
            Margin = new Padding(5, 0, 0, 20)
        };

        public ListView genreBox = new ListView()
        {
            View = View.Details,
            LabelEdit = true,
            AllowColumnReorder = true,
            CheckBoxes = true,
            FullRowSelect = true,
            GridLines = true,
            Sorting = SortOrder.Ascending,
            Size = new Size(200, 100)
        };

        public TrackBar gameDifficulty = new TrackBar()
        {
            Size = new Size(195, 45),
            Maximum = 10,
            TickFrequency = 1,
            LargeChange = 2,
            SmallChange = 1
        };


        GamePopupBoxRght rght;
        private List<ListViewItem> genreItems = new List<ListViewItem>();
        private GamesController gamesController;
        public Game game;
        private Image image;


        public GamePopupBox(GamesController gamesController, Game game)
        {

            Text = "Game";


            //Size = new Size(500,640);
            //this.genres = genres;
            this.gamesController = gamesController;

            genreBox.Columns.Add("Genre", -2, HorizontalAlignment.Left); //TODO: MOVE
            foreach (var item in gamesController.genres)
                            genreItems.Add(new ListViewItem { Name = item, Text = item});
            genreBox.Items.AddRange(genreItems.ToArray());

            this.game = game;

            if (game != null)
            {

                gameName.Text = this.game.name;
                gameDescription.Text = this.game.description;
                yearPublished.Text = this.game.publishedYear.ToString();
                time.Text = $"{game.minPlayers} - {game.maxPlayers}";
                players.Text = $"{game.minPlayTime} - {game.maxPlayTime}";
                //gameImage.BackgroundImage = Image.FromFile(this.gamesList.thumbnail); TODO: look at
                gameDifficulty.Value = game.difficulity;

                foreach(ListViewItem item in genreItems )
                    //TODO: add non existant genres to list
                    item.Checked = game.genres.Any(x => x == item.Text);

            }
            else
            {
                Controls.Find("delete", true).First().Enabled = false;
            }

            SubscriptionList();
            toolTipControl();
            Show();
        }


        private void SubscriptionList()
        {

            genreBox.ItemCheck += memeberChecked;

            imageText.Click += OpenFileOpener;

            gameDifficulty.Scroll += (s, e) => {
                toolTip.SetToolTip(gameDifficulty, $"{gameDifficulty.Value} / {gameDifficulty.Maximum}");
                toolTip.SetToolTip(gameDifficulty, "Current value: " + gameDifficulty.Value.ToString() + " out of {game}");

            };

            editGenre.Click += (s, e) => {
                EditGenrePopupbox bob = new EditGenrePopupbox(gamesController);
            }; //TODO: look at


        }

        private void toolTipControl()
        {
            toolTip.SetToolTip(time, time.Name);
            toolTip.SetToolTip(players, players.Name);
            toolTip.SetToolTip(gameName, "Game name");
            toolTip.SetToolTip(gameDescription, "Game description");
        }

        protected override Control CreateControls()
        {

            rght = new GamePopupBoxRght(this);
            Controls.Add(header);

            lft.ColumnCount = 1;
            lft.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            //lft.Dock = DockStyle.Fill;

            lft.Controls.Add(gameName);
            lft.Controls.Add(gameDescription);

            TableLayoutPanel generalInformaiton = new TableLayoutPanel();
            generalInformaiton.ColumnCount = 2;
            generalInformaiton.RowCount = 1;

            generalInformaiton.Height = time.Height;
            generalInformaiton.Controls.Add(time);
            generalInformaiton.Controls.Add(players);

            lft.Controls.Add(gameDifficulty);

            lft.Controls.Add(generalInformaiton);
            lft.Controls.Add(genreBox);
            //genreBox.Controls.Add(editGenre);
            lft.Controls.Add(editGenre);
            
            TableLayoutPanel imageSeachTable = new TableLayoutPanel();
            imageSeachTable.ColumnCount = 2;
            imageSeachTable.RowCount = 1;
            imageSeach.Height = imageSeach.Height;
            imageSeachTable.Height = imageSeach.Height;
            imageSeachTable.Controls.Add(imageText);
            imageSeachTable.Controls.Add(imageSeach);

            lft.Controls.Add(imageSeachTable);

            lft.Controls.Add(gameImage);





            header.Controls.Add(lft);
            header.Controls.Add(rght);
            return header;
        }

        protected override void save(object sender, EventArgs e)
        {

            //TODO: parse yearpublished

            Game newGame = new Game();

            newGame.name = gameName.Text;
            newGame.description = gameDescription.Text;
            game.difficulity = gameDifficulty.Value;

            if (yearPublished.Text != "")
            {
                try
                {
                    Int32.TryParse(yearPublished.Text, out game.publishedYear);
                }
                catch (Exception)
                {
                    MessageBox.Show("Published year is not a valid number", "Year invalid");
                    return;
                }
            }

            string[] timeSplit = time.Text.Split('-');
            if (timeSplit.Count() == 1)
            {
                try
                {
                    Int32.TryParse(timeSplit[0], out game.minPlayTime);
                    game.maxPlayTime = game.minPlayTime;
                }
                catch (Exception)
                {
                    MessageBox.Show("Time is not a number", "Play time error");
                    return;
                }
            } else if (timeSplit.Count() == 2)
            {
                try
                {
                    Int32.TryParse(timeSplit[0], out game.minPlayTime);
                    Int32.TryParse(timeSplit[1], out game.maxPlayTime);
                }
                catch (Exception)
                {
                    MessageBox.Show("Time is not a valid range", "Play time error");
                    return;
                }
            }
            else if (timeSplit.Count() > 3)
            {
                MessageBox.Show("Too many parts in play time", "Play time error");
                return;
            }

            string[] playersSplit = players.Text.Split('-');
            if (playersSplit.Length == 1)
            {
                try
                {
                    Int32.TryParse(playersSplit[0], out game.minPlayers);
                    game.maxPlayers = game.minPlayers;
                }
                catch (Exception)
                {
                    MessageBox.Show("Recommended players is not a valid number", "Players error");
                    return;
                }
            } else if (playersSplit.Length == 2)
            {
                try
                {
                    Int32.TryParse(playersSplit[0], out game.minPlayers);
                    Int32.TryParse(playersSplit[1], out game.maxPlayers);
                }
                catch (Exception)
                {
                    MessageBox.Show("Recommended players is not a valid range", "Players error");
                    return;
                }
            }
            else if (playersSplit.Length > 3)
            {
                MessageBox.Show("Too many parts in recommended players", "Players error");
                return;
            }



            if (game == null)
            {
                gamesController.addGame(newGame);
            }
            else
            {
                gamesController.updateGame(newGame);
            }


        }

        protected override void delete(object sender, EventArgs e) {
            gamesController.removeGame(game);
            Close();
        }

        private void memeberChecked(object sender, ItemCheckEventArgs e) {

            if (e.CurrentValue != CheckState.Checked) {
                string genre = genreBox.Items[e.Index].Text;
                game.genres.Add(genre);
                if (!gamesController.genres.Contains(genre))
                    gamesController.addGenre(genre);
            }
            else
                game.genres.Remove(genreBox.Items[e.Index].Text);

        }

        private void OpenFileOpener(object sender, EventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog();
            var pnis = ImageCodecInfo.GetImageDecoders();
            StringBuilder sb = new StringBuilder();

            foreach (ImageCodecInfo item in pnis) {
                sb.Append(item.FilenameExtension);
                sb.Append(";");
            }

            ofd.Title = "Open Image";
            ofd.Filter = "Image Files | " + sb.ToString();

            if (ofd.ShowDialog() == DialogResult.OK) {
                try {
                    game.imageName = ofd.SafeFileName; // name
                    image = Image.FromFile(ofd.FileName); // path + name
                    gameImage.BackgroundImage = image;
                    imageSeach.Text = ofd.FileName;
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}