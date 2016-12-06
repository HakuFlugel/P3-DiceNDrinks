using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System;
using Shared;
using System.Drawing.Imaging;
using System.Media;

namespace AdministratorPanel {

    public class GamePopupBox : FancyPopupBox {
        protected TableLayoutPanel mainLayoutPanel = new TableLayoutPanel() {
            RowCount = 1,
            ColumnCount = 2,
            AutoSize = true,
            GrowStyle = TableLayoutPanelGrowStyle.FixedSize
        };

        protected TableLayoutPanel leftTableLayoutPanel = new TableLayoutPanel() {
            ColumnCount = 1,
            GrowStyle = TableLayoutPanelGrowStyle.AddRows,
            AutoSize = true
        };

        private TableLayoutPanel LeftButtomPanel = new TableLayoutPanel() {
            RowCount = 1,
            ColumnCount = 2,
            AutoSize = true,
        };

        private TableLayoutPanel LeftButtomPanelLeft = new TableLayoutPanel() {
            Dock = DockStyle.Left,
            AutoSize = true,
        };

        private TableLayoutPanel LeftButtomPanelRight = new TableLayoutPanel() {
            Dock = DockStyle.Top,
            AutoSize = true,
        };

        private TableLayoutPanel generalInformaiton = new TableLayoutPanel() {
            ColumnCount = 2,
            RowCount = 1,
            Dock = DockStyle.Top,
        };

        private TableLayoutPanel imageSeachTable = new TableLayoutPanel() {
            ColumnCount = 2,
            RowCount = 1,
        };
            
        public NiceTextBox gameName = new NiceTextBox() {
            Width = 400,
            waterMark = "Game name",
            Margin = new Padding(5, 10, 20, 10)
        };

        private Button editGenre = new Button() {
            Text = "Edit",
            Dock = DockStyle.Right,
            MaximumSize = new Size(50, 17),
        };

        public ToolTip toolTip = new ToolTip() {
            AutoPopDelay = 5000,
            InitialDelay = 100,
            ReshowDelay = 500,
            ShowAlways = true
        };

        public NiceTextBox gameDescription = new NiceTextBox() {
            Width = 400,
            Height = 250,
            waterMark = "Game Description",
            Multiline = true,
            Margin = new Padding(5, 0, 20, 10)
        };

        public NiceTextBox imageText = new NiceTextBox() {
            Width = 130,
            waterMark = "Path to picture",
            Margin = new Padding(5, 0, 0, 20),
            Dock = DockStyle.Left
        };

        public Button imageSeach = new Button() {
            BackgroundImage = Image.FromFile("images/seachIcon.png"),
            BackgroundImageLayout = ImageLayout.Zoom,
            Width = 50,
            Margin = new Padding(5, 0, 0, 20),
        };

        public NiceTextBox yearPublished = new NiceTextBox() {
            Width = 200,
            waterMark = "Year published",
            Margin = new Padding(5, 10, 20, 10)
        };

        public Panel gameImage = new Panel() {
            Width = 200,
            Height = 200,
            Margin = new Padding(5, 0, 0, 0),
            BackgroundImage = Image.FromFile("images/_default.png"),
            BackgroundImageLayout = ImageLayout.Zoom,
        };

        public NiceTextBox time = new NiceTextBox() {
            Name = "minimum / maximum players" + Environment.NewLine + "it takes to play the game." + Environment.NewLine + "eg: 3/10",
            waterMark = "min / max players",
            Width = 95,
            Margin = new Padding(0, 0, 0, 20)
        };

        public NiceTextBox players = new NiceTextBox() {
            Name = "minimum / maximum time " + Environment.NewLine + "it takes to complete the game." + Environment.NewLine + "eg: 30/60",
            waterMark = "min / max time",
            Width = 95,
            Margin = new Padding(5, 0, 0, 20)
        };

        public ListView genreBox = new ListView() {
            View = View.Details,
            LabelEdit = true,
            AllowColumnReorder = true,
            CheckBoxes = true,
            FullRowSelect = true,
            GridLines = true,
            Dock = DockStyle.Left,
            Sorting = SortOrder.Ascending,
            Width = 200,
        };

        public TrackBar gameDifficulty = new TrackBar() {
            Size = new Size(195, 45),
            Maximum = 10,
            TickFrequency = 1,
            LargeChange = 2,
            SmallChange = 1
        };

        private GamePopupBoxRght rightBox;
        private List<ListViewItem> genreItems = new List<ListViewItem>();
        private Genres genres;
        private GamesTab gametab;
        public Game game;
        private Game beforeEditing;
        private Image seachImage = Image.FromFile("images/seachIcon.png");
        private Image image;
        public List<Game> games;
        public string imagePath = "";

        public GamePopupBox(GamesTab gametab, Game game, Genres genres) {
            Text = "Game";

            Size = new Size(500,640);
            this.genres = genres;
            this.gametab = gametab;
            genreBox.Columns.Add("Genre", -2, HorizontalAlignment.Left);
            
            foreach (var item in genres.differentGenres) 
                genreItems.Add(new ListViewItem { Name = item, Text = item});
            
            genreBox.Items.AddRange(genreItems.ToArray());

            if (game != null) {
                Console.WriteLine("iamge path" + game.imageName);
                beforeEditing = game;
                this.game = new Game(game);

                gameName.Text = this.game.name;
                gameDescription.Text = this.game.description;
                yearPublished.Text = this.game.publishedYear.ToString();
                time.Text = this.game.minPlayers.ToString() + " / " + this.game.maxPlayers.ToString();
                players.Text = this.game.minPlayTime.ToString() + " / " + this.game.maxPlayTime.ToString();
                gameImage.BackgroundImage = Image.FromFile($"images/{this.game.imageName}");
                gameDifficulty.Value = game.difficulity;
                imagePath = beforeEditing.imageName;

                foreach(ListViewItem item in genreItems )
                    //TODO: add non existant genre to list
                    item.Checked = (game.genre.Any(x => x == item.Text)) ? true : false;

            } else {
                this.game = new Game();
                Controls.Find("delete", true).First().Enabled = false;
            }
            Show();
            SubscriptionList();
            toolTipControl();
        }
        
        private void SubscriptionList() {
            bool isNewGame = (beforeEditing != null) ? true : false;
           
            /* 
             * Checks if there has been any changes in the game, if there is any changes,
             * and they are different from what the game looked before (if it is not a new game) then set hasBeenChanged to true,
             * hasBeenChanged is located in FancyPopupBox and is a bool to keep track of if something is changed,
             * if true, there will be a messagebox that asks if the user is sure it will close b4 saving.
             */
            yearPublished.TextChanged += (s, e) => { hasBeenChanged = (isNewGame) ? ((beforeEditing.publishedYear.ToString() != yearPublished.Text) ? true : false) : true; };
            gameName.TextChanged += (s, e) => { hasBeenChanged = (isNewGame) ? ((beforeEditing.name != gameName.Text) ? true : false) : true; };

            gameDescription.TextChanged += (s, e) => { hasBeenChanged = (isNewGame) ? ((beforeEditing.description != gameDescription.Text) ? true : false) : true; };

            players.TextChanged += (s, e) => { hasBeenChanged = (isNewGame) ? (!((beforeEditing.minPlayers.ToString() + "/" + beforeEditing.maxPlayers.ToString()).Equals(players.Text)) ? true : false) : false; };

            time.TextChanged += (s, e) => {
                hasBeenChanged = (isNewGame) ? (!((beforeEditing.minPlayTime.ToString() + "/" + beforeEditing.maxPlayTime.ToString()).Equals(time.Text)) ? true : false) : false;
            };

            genreBox.ItemCheck += memeberChecked;

            imageText.Click += OpenFileOpener;

            gameDifficulty.Scroll += (s, e) => {
                toolTip.SetToolTip(gameDifficulty, $"Current value: {gameDifficulty.Value} out of {gameDifficulty.Maximum}");
                hasBeenChanged = (isNewGame) ? ((beforeEditing.difficulity != gameDifficulty.Value) ? true : false) : true;
            };

            editGenre.Click += (s, e) => {
                EditGenrePopupbox bob = new EditGenrePopupbox(genres);
            };
        }

        private void toolTipControl() {
            toolTip.SetToolTip(time, time.Name);
            toolTip.SetToolTip(players, players.Name);
            toolTip.SetToolTip(gameName, "Game name");
            toolTip.SetToolTip(gameDescription, "Game description");
        }

        protected override Control CreateControls() {

            rightBox = new GamePopupBoxRght(this);
            Controls.Add(mainLayoutPanel);

            leftTableLayoutPanel.Controls.Add(gameName);
            leftTableLayoutPanel.Controls.Add(gameDescription);

            generalInformaiton.Height = time.Height;
            generalInformaiton.Controls.Add(time);
            generalInformaiton.Controls.Add(players);

            LeftButtomPanelRight.Controls.Add(gameDifficulty); 

            LeftButtomPanelRight.Controls.Add(generalInformaiton);
            
            genreBox.Controls.Add(editGenre);

            LeftButtomPanelLeft.Controls.Add(genreBox);

            imageSeach.Height = imageSeach.Height;
            imageSeachTable.Height = imageSeach.Height;
            imageSeachTable.Controls.Add(imageText);
            imageSeachTable.Controls.Add(imageSeach);

            LeftButtomPanelRight.Controls.Add(imageSeachTable);
            LeftButtomPanelRight.Controls.Add(gameImage);

            LeftButtomPanel.Controls.Add(LeftButtomPanelLeft);
            LeftButtomPanel.Controls.Add(LeftButtomPanelRight);

            leftTableLayoutPanel.Controls.Add(LeftButtomPanel);

            mainLayoutPanel.Controls.Add(leftTableLayoutPanel);
            mainLayoutPanel.Controls.Add(rightBox);
            return mainLayoutPanel;
        }

        protected override void save(object sender, EventArgs e) {
           
            genres.Save();

            if (beforeEditing != null) {
                //name
                    game.name = (gameName.Text != null && gameName.Text != "") ? gameName.Text : "Unnamed game";
                //decription
                    game.description = (gameDescription.Text != null && gameDescription.Text != "") ? gameDescription.Text : "Undescriped game";
                //difficulty
                    game.difficulity = gameDifficulty.Value;
                //year
                    if (yearPublished.Text != null && yearPublished.Text != "") {
                        try {
                        game.publishedYear = Int32.Parse(yearPublished.Text);
                        } catch (Exception) {
                            MessageBox.Show("Published year is not a valid number", "Year invalid");
                        }
                    } else {
                        game.publishedYear = 0;
                    }
                //time
                    string messageboxText = "Please only use whole integers" + Environment.NewLine + "In this format INTEGER/INTEGER";
                    string[] timePeriode = time.Text.Split('/');
                    try {
                        game.minPlayTime = Int32.Parse(timePeriode[0]);
                        game.maxPlayTime = Int32.Parse(timePeriode[1]);

                    } catch (Exception) {
                        SystemSounds.Hand.Play();
                        time.Text = time.waterMark;
                        MessageBox.Show(messageboxText, " Error in time");
                    }

                //players
                    string[] playerRange = players.Text.Split('/');
                    try {
                        game.minPlayers = Int32.Parse(playerRange[0]);
                        game.maxPlayers = Int32.Parse(playerRange[1]);
                    } catch (Exception) {
                        SystemSounds.Hand.Play();
                        players.Text = players.waterMark;
                        MessageBox.Show(messageboxText, "Error in players");
                    }
                //image
                game.imageName = imagePath;

                gametab.games.Remove(beforeEditing);
                beforeEditing = game;
            }

            Console.WriteLine("iamge name before save"+game.imageName);
            gametab.games.Add(game);
            gametab.game.makeItems("");
            base.save(sender, e);
        }

        protected override void delete(object sender, EventArgs e) {
            gametab.games.Remove(beforeEditing);
            gametab.game.makeItems("");
            Close();
        }

        private void memeberChecked(object sender, ItemCheckEventArgs e) {
            
            if (e.CurrentValue != CheckState.Checked) {
                string temp = genreBox.Items[e.Index].Text;
                game.genre.Add(temp);
                if (!genres.differentGenres.Contains(temp))
                    genres.add(temp);
            }
            else 
                game.genre.Remove(genreBox.Items[e.Index].Text);
                hasBeenChanged = true;
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