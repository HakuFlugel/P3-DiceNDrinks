using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System;
using System.Collections.Specialized;
using Shared;
using System.Drawing.Imaging;
using System.IO;
using Newtonsoft.Json;

namespace AdministratorPanel
{

    public class GamePopupBox : FancyPopupBox
    {
        protected TableLayoutPanel mainLayoutPanel = new TableLayoutPanel()
        {
            RowCount = 1,
            ColumnCount = 2,
            AutoSize = true,
            GrowStyle = TableLayoutPanelGrowStyle.FixedSize
        };

        protected TableLayoutPanel leftTableLayoutPanel = new TableLayoutPanel()
        {
            ColumnCount = 1,
            GrowStyle = TableLayoutPanelGrowStyle.AddRows,
            AutoSize = true
        };

        private TableLayoutPanel LeftButtomPanel = new TableLayoutPanel()
        {
            RowCount = 1,
            ColumnCount = 2,
            AutoSize = true
        };

        private TableLayoutPanel LeftButtomPanelLeft = new TableLayoutPanel()
        {
            Dock = DockStyle.Left,
            AutoSize = true
        };

        private TableLayoutPanel LeftButtomPanelRight = new TableLayoutPanel()
        {
            Dock = DockStyle.Top,
            AutoSize = true
        };

        private TableLayoutPanel generalInformaiton = new TableLayoutPanel()
        {
            ColumnCount = 2,
            RowCount = 1,
            Dock = DockStyle.Top
        };

        private TableLayoutPanel imageSeachTable = new TableLayoutPanel()
        {
            ColumnCount = 2,
            RowCount = 1
        };

        public NiceTextBox gameName = new NiceTextBox()
        {
            Width = 400,
            waterMark = "Game name",
            Margin = new Padding(5, 10, 20, 10)
        };

        private Button editGenre = new Button()
        {
            Text = "Edit",
            Dock = DockStyle.Right,
            MaximumSize = new Size(50, 17)
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
            Width = 400,
            Height = 250,
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
            Margin = new Padding(5, 0, 0, 20)
        };

        public NiceTextBox yearPublishedBox = new NiceTextBox()
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
            BackgroundImageLayout = ImageLayout.Zoom
        };

        public NiceTextBox timeBox = new NiceTextBox()
        {
            Name =
                "minimum / maximum time " + Environment.NewLine + "it takes to complete the game." + Environment.NewLine +
                "eg: 30/60",
            waterMark = "min / max players",
            Width = 95,
            Margin = new Padding(0, 0, 0, 20)
        };

        public NiceTextBox playerBox = new NiceTextBox()
        {
            Name =
                "minimum / maximum players" + Environment.NewLine + "it takes to play the game." + Environment.NewLine +
                "eg: 3/10",
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
            Dock = DockStyle.Left,
            Sorting = SortOrder.Ascending,
            Width = 200,
        };

        public TrackBar gameDifficultyBar = new TrackBar()
        {
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
        public Image image;
        public List<Game> games;
        public string imagePath = "";

        public GamePopupBox(GamesTab gametab, Game game, Genres genres)
        {
            Text = "Game";

            Size = new Size(500, 640);
            this.genres = genres;
            this.gametab = gametab;
            genreBox.Columns.Add("Genre", -2, HorizontalAlignment.Left);

            foreach (var item in genres.differentGenres)
                genreItems.Add(new ListViewItem {Name = item, Text = item});

            genreBox.Items.AddRange(genreItems.ToArray());

            if (game != null)
            {
                beforeEditing = game;
                this.game = new Game(game);

                gameName.Text = this.game.name;
                gameDescription.Text = this.game.description;
                yearPublishedBox.Text = this.game.publishedYear.ToString();
                playerBox.Text = this.game.minPlayers.ToString() + " / " + this.game.maxPlayers.ToString();
                timeBox.Text = this.game.minPlayTime.ToString() + " / " + this.game.maxPlayTime.ToString();
                string curFile = $"images/{this.game.imageName}";
                gameImage.BackgroundImage = Image.FromFile(File.Exists(curFile) ? curFile : $"images/_default.png");
                gameDifficultyBar.Value = game.difficulity;
                imagePath = beforeEditing.imageName;

                foreach (ListViewItem item in genreItems)
                    item.Checked = (game.genre.Any(x => x == item.Text)) ? true : false;

            }
            else
            {
                this.game = new Game();
                Controls.Find("delete", true).First().Enabled = false;
            }
            SubscriptionList();
            toolTipControl();
        }

        private void SubscriptionList()
        {
            bool isNewGame = beforeEditing != null;

            /* 
             * Checks if there has been any changes in the game, if there is any changes,
             * and they are different from what the game looked before (if it is not a new game) then set hasBeenChanged to true,
             * hasBeenChanged is located in FancyPopupBox and is a bool to keep track of if something is changed,
             * if true, there will be a messagebox that asks if the user is sure it will close before edditing saving.
             */
            yearPublishedBox.TextChanged +=
                (s, e) =>
                {
                    hasBeenChanged = (isNewGame)
                        ? (beforeEditing.publishedYear.ToString() != yearPublishedBox.Text)
                        : true;
                };
            gameName.TextChanged +=
                (s, e) =>
                {
                    hasBeenChanged = (isNewGame) ? ((beforeEditing.name != gameName.Text) ? true : false) : true;
                };

            gameDescription.TextChanged +=
                (s, e) =>
                {
                    hasBeenChanged = (isNewGame)
                        ? ((beforeEditing.description != gameDescription.Text) ? true : false)
                        : true;
                };

            playerBox.TextChanged +=
                (s, e) =>
                {
                    hasBeenChanged = (isNewGame)
                        ? (!((beforeEditing.minPlayers.ToString() + "/" + beforeEditing.maxPlayers.ToString()).Equals(
                            playerBox.Text))
                            ? true
                            : false)
                        : false;
                };

            timeBox.TextChanged += (s, e) =>
            {
                hasBeenChanged = (isNewGame)
                    ? (!((beforeEditing.minPlayTime.ToString() + "/" + beforeEditing.maxPlayTime.ToString()).Equals(
                        timeBox.Text))
                        ? true
                        : false)
                    : false;
            };

            genreBox.ItemCheck += memeberChecked;

            imageSeach.Click += OpenFileOpener; //Så det ikke er textboxen som skal

            imageText.KeyPress += (s, e) =>
            {
                if (e.KeyChar != (char) Keys.Enter)
                    return;
                Image img;
                try
                {
                    img = Image.FromFile(imageText.Text);
                }
                catch (Exception)
                {
                    img = Image.FromFile($"images/_default.png");
                }
                gameImage.BackgroundImage = img;
            }; //Gør så man kan smide en path til et billed ind, unden at skulle trykke på search kanppen

            gameDifficultyBar.Scroll += (s, e) =>
            {
                toolTip.SetToolTip(gameDifficultyBar,
                    $"Current value: {gameDifficultyBar.Value} out of {gameDifficultyBar.Maximum}");
                hasBeenChanged = (isNewGame)
                    ? ((beforeEditing.difficulity != gameDifficultyBar.Value) ? true : false)
                    : true;
            };

            editGenre.Click += (s, e) => {
                new EditGenrePopupbox(genres);
            };
        }

        private void toolTipControl()
        {
            toolTip.SetToolTip(timeBox, timeBox.Name);
            toolTip.SetToolTip(playerBox, playerBox.Name);
            toolTip.SetToolTip(gameName, "Game name");
            toolTip.SetToolTip(gameDescription, "Game description");
        }

        protected override Control CreateControls()
        {

            rightBox = new GamePopupBoxRght(this);
            Controls.Add(mainLayoutPanel);

            leftTableLayoutPanel.Controls.Add(gameName);
            leftTableLayoutPanel.Controls.Add(gameDescription);

            generalInformaiton.Height = timeBox.Height;
            generalInformaiton.Controls.Add(timeBox);
            generalInformaiton.Controls.Add(playerBox);

            LeftButtomPanelRight.Controls.Add(gameDifficultyBar);

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

        protected override void save(object sender, EventArgs e)
        {
            Directory.CreateDirectory("images/games/");
            genres.Save();
            //name
            game.name = (gameName.Text != null && gameName.Text != gameName.waterMark && gameName.Text != "")
                ? gameName.Text
                : "Unnamed game";
            //decription
            game.description = (gameDescription.Text != null && gameDescription.Text != gameDescription.waterMark &&
                                gameDescription.Text != "")
                ? gameDescription.Text
                : "Undescriped game";
            //difficulty
            game.difficulity = gameDifficultyBar.Value;
            //year
            if (yearPublishedBox.Text != null && yearPublishedBox.Text != "" &&
                yearPublishedBox.Text != yearPublishedBox.waterMark)
            {
                try
                {
                    game.publishedYear = Int32.Parse(yearPublishedBox.Text);
                }
                catch (Exception)
                {
                    NiceMessageBox.Show("Published year is not a valid number", "Year invalid");
                }
            }
            else
            {
                game.publishedYear = 0;
            }
            //time
            string[] timePeriode = timeBox.Text.Split('/');
            try
            {
                game.minPlayTime = Int32.Parse(timePeriode[0]);
                game.maxPlayTime = Int32.Parse(timePeriode[1]);
                if (game.minPlayTime > game.maxPlayTime)
                {
                    game.maxPlayTime = game.minPlayTime;
                    timeBox.Text = game.minPlayTime + "/" + game.maxPlayTime;
                }

            }
            catch (Exception)
            {

                timeBox.Text = (beforeEditing != null)
                    ? beforeEditing.maxPlayTime + "/" + beforeEditing.minPlayTime
                    : timeBox.waterMark;
                NiceMessageBox.Show("Integer in min / max time is not withing limited borders.");
                return;
            }

            //players
            string[] playerRange = playerBox.Text.Split('/');
            try
            {
                game.minPlayers = Int32.Parse(playerRange[0]);
                game.maxPlayers = Int32.Parse(playerRange[1]);
                if (game.minPlayers > game.maxPlayers)
                {
                    game.maxPlayers = game.minPlayers;
                    playerBox.Text = game.minPlayers + "/" + game.maxPlayers;
                }

            }
            catch (Exception)
            {
                playerBox.Text = (beforeEditing != null)
                    ? beforeEditing.maxPlayers + "/" + beforeEditing.minPlayers
                    : playerBox.waterMark;
                NiceMessageBox.Show("Integer in min / max players is not withing limited borders.");
                return;
            }
            //image
            game.imageName = imagePath;

            try
            {
                image?.Save("images/games/"+ imagePath);
            }
            catch (Exception)
            {
                NiceMessageBox.Show("Could not save image locally");
            }

            try {
                string response = ServerConnection.sendRequest("/submitGame.aspx",
                new NameValueCollection() {
                    {"Action", beforeEditing == null ? "add" : "update"},
                    {"Game", JsonConvert.SerializeObject(game)},
                    {"Image", image != null ? ImageHelper.imageToBase64(image) : ""}
                }
            );
                if (response.StartsWith("exception")) {
                    throw new Exception(response);
                }
                NiceMessageBox.Show(response + " Fuck");

                if (beforeEditing != null) {
                    if (response != "updated") {
                        return;
                    }
                    beforeEditing = game;
                }
                else {

                    if (response.Split(' ')[0] != "added") {
                        NiceMessageBox.Show("Fuck");
                        return;
                        
                    }
                    int.TryParse(response.Split(' ')[1], out game.id);
                    gametab.games.Add(game);
                }
            }
            catch (Exception ex) {
                NiceMessageBox.Show("Failed to save to the server, changes will be lost if this window is closed", "Server connection problem");
                
                return;
            }
            gametab.game.makeItems("");
            base.save(sender, e);
        }

        protected override void delete(object sender, EventArgs e) {

            string response = ServerConnection.sendRequest("/submitGame.aspx",
                new NameValueCollection() {
                    {"Action", "delete"},
                    {"Game", JsonConvert.SerializeObject(beforeEditing)}
                }
            );

            Console.WriteLine(response);

            if (response != "deleted")
            {
                return;
            }

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
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var pnis = ImageCodecInfo.GetImageDecoders();
            StringBuilder sb = new StringBuilder();

            foreach (ImageCodecInfo item in pnis) {
                sb.Append(item.FilenameExtension);
                sb.Append(";");
            }

            openFileDialog.Title = "Open Image";
            openFileDialog.Filter = "Image Files | " + sb.ToString();

            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                try {
                    game.imageName = openFileDialog.SafeFileName; // name
                    imagePath = openFileDialog.SafeFileName;
                    image = Image.FromFile(openFileDialog.FileName); // path + name
                    gameImage.BackgroundImage = image;
                    imageText.Text = openFileDialog.FileName;

                } catch (Exception ex) {
                    NiceMessageBox.Show(ex.Message);
                }
            }
        }
    }
}