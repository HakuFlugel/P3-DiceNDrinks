using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System;
using Shared;
using System.IO;
using System.Data;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.Media;

namespace AdministratorPanel {

    public class GamePopupBox : FancyPopupBox {
        protected TableLayoutPanel lft = new TableLayoutPanel() {
            ColumnCount = 1,
            GrowStyle = TableLayoutPanelGrowStyle.AddRows,
            //Dock = DockStyle.Fill
            AutoSize = true
        };

        protected TableLayoutPanel header = new TableLayoutPanel() {
            RowCount = 1,
            ColumnCount = 2,
            //Dock = DockStyle.Fill,
            AutoSize = true,
            GrowStyle = TableLayoutPanelGrowStyle.FixedSize
        };
        
        public NiceTextBox gameName = new NiceTextBox() {
            Width = 200,
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
            Width = 200,
            Height = 100,
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
            Sorting = SortOrder.Ascending,
            Size = new Size(200, 100)
        };

        public TrackBar gameDifficulty = new TrackBar() {
            Size = new Size(195, 45),
            Maximum = 10,
            TickFrequency = 1,
            LargeChange = 2,
            SmallChange = 1
        };


        GamePopupBoxRght rght;
        private List<ListViewItem> genreItems = new List<ListViewItem>();
        private Genres genres;
        private GamesTab gametab;
        public Game game;
        private Game b4EditingGame;
        private Image seachImage = Image.FromFile("images/seachIcon.png");
        private Image image;
        public List<Game> games;
        

        public GamePopupBox(GamesTab gametab, Game game, Genres genres) {
            Size = new Size(500,640);
            this.genres = genres;
            this.gametab = gametab;
            genreBox.Columns.Add("Genre", -2, HorizontalAlignment.Left);
            

            foreach (var item in genres.differentGenres) 
                genreItems.Add(new ListViewItem { Name = item, Text = item});
            


            genreBox.Items.AddRange(genreItems.ToArray());

            
            if (game != null) {
                


                b4EditingGame = game;
                this.game = new Game(game);

                gameName.Text = this.game.name;
                gameDescription.Text = this.game.description;
                yearPublished.Text = this.game.publishedYear.ToString();
                time.Text = this.game.minPlayers.ToString() + " / " + this.game.maxPlayers.ToString();
                players.Text = this.game.minPlayTime.ToString() + " / " + this.game.maxPlayTime.ToString();
                //gameImage.BackgroundImage = Image.FromFile(this.game.thumbnail);
                gameDifficulty.Value = game.difficulity;

                foreach(ListViewItem item in genreItems )
                    item.Checked = (game.genre.Any(x => x == item.Text)) ? true : false;

            } else {
                this.game = new Game();
                Console.WriteLine(this.game.ToString());
                Controls.Find("delete", true).First().Enabled = false;
            }
            

            


            Show();
            SubscriptionList();
            toolTipControl();
        }

        
        private void SubscriptionList() {
            bool isNewGame = (b4EditingGame != null) ? true : false;
           
            /* 
             * Checks if there has been any changes in the game, if there is any changes,
             * and they are different from what the game looked before (if it is not a new game) then set hasBeenChanged to true,
             * hasBeenChanged is located in FancyPopupBox and is a bool to keep track of if something is changed,
             * if true, there will be a messagebox that asks if the user is sure it will close b4 saving.
             */
            yearPublished.TextChanged += (s, e) => { hasBeenChanged = (isNewGame) ? ((b4EditingGame.publishedYear.ToString() != yearPublished.Text) ? true : false) : true; };
            gameName.TextChanged += (s, e) => { hasBeenChanged = (isNewGame) ? ((b4EditingGame.name != gameName.Text) ? true : false) : true; };

            gameDescription.TextChanged += (s, e) => { hasBeenChanged = (isNewGame) ? ((b4EditingGame.description != gameDescription.Text) ? true : false) : true; };

            players.TextChanged += (s, e) => { hasBeenChanged = (isNewGame) ? (!((b4EditingGame.minPlayers.ToString() + "/" + b4EditingGame.maxPlayers.ToString()).Equals(players.Text)) ? true : false) : false; };

            time.TextChanged += (s, e) => {
                hasBeenChanged = (isNewGame) ? (!((b4EditingGame.minPlayTime.ToString() + "/" + b4EditingGame.maxPlayTime.ToString()).Equals(time.Text)) ? true : false) : false;
            };

            time.LostFocus += (s, e) => {

                
            };

            players.LostFocus += (s, e) => {
                
                
            };

            genreBox.ItemCheck += new ItemCheckEventHandler(memeberChecked);

            imageText.Click += OpenFileOpener;

            gameDifficulty.Scroll += (s, e) => {

                toolTip.SetToolTip(gameDifficulty, "Current value: " + gameDifficulty.Value.ToString() + " out of 100");
                hasBeenChanged = (isNewGame) ? ((b4EditingGame.difficulity != gameDifficulty.Value) ? true : false) : true;

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
            genreBox.Controls.Add(editGenre);

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

        protected override void save(object sender, EventArgs e)    {

            genres.Save();

            if (b4EditingGame != null) {
                game.description = (gameDescription.Text != null && gameDescription.Text != "") ? gameDescription.Text : "Undescriped game";
                game.name = (gameName.Text != null && gameName.Text != "") ? gameName.Text : "Unnamed game";
                game.difficulity = gameDifficulty.Value;
                if (yearPublished.Text != null && yearPublished.Text != "") {
                    try {
                        Int32.TryParse(yearPublished.Text, out game.publishedYear);
                    } catch (Exception) {
                        MessageBox.Show("Please refere from using letters or special characters in Publish year box" + Environment.NewLine + "Yearpublished is not a number: " + yearPublished.Text.ToString(), "Convertion failed");
                    }
                } else {
                    game.publishedYear = 1337;
                }
                string messageboxText = "Please only use whole integers" + Environment.NewLine + "In this format INTEGER/INTEGER";

                string[] timeWOS = time.Text.Split('/');
                try {
                    Int32.TryParse(timeWOS[0], out game.minPlayTime);
                    Int32.TryParse(timeWOS[1], out game.maxPlayTime);

                } catch (Exception) {
                    SystemSounds.Hand.Play();
                    time.Text = time.waterMark;
                    MessageBox.Show(messageboxText, "FATAL ERROR IN TIME");
                }

                string[] playersWOS = players.Text.Split('/');
                try {
                    Int32.TryParse(playersWOS[0], out game.minPlayers);
                    Int32.TryParse(playersWOS[1], out game.maxPlayers);
                } catch (Exception) {
                    SystemSounds.Hand.Play();
                    players.Text = players.waterMark;
                    MessageBox.Show(messageboxText, "FATAL ERROR IN PLAYERS");
                }



                gametab.games.Remove(b4EditingGame);
                
                b4EditingGame = game;
                
            }

            //create game.
            gametab.games.Add(game);
            gametab.game.makeItems("");
            base.save(sender, e);
        }

        protected override void delete(object sender, EventArgs e) {
            gametab.games.Remove(b4EditingGame);
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