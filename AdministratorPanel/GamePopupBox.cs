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

    class GamePopupBox : FancyPopupBox {

        NiceTextBox gameName = new NiceTextBox() {
            Width = 200,
            waterMark = "Game name",
            Margin = new Padding(5, 10, 20, 10)
        };

        ToolTip toolTip = new ToolTip() {
            AutoPopDelay = 5000,
            InitialDelay = 100,
            ReshowDelay = 500,
            ShowAlways = true

        };
        NiceTextBox gameDescription = new NiceTextBox() {
            Width = 200,
            Height = 100,
            waterMark = "Game Description",
            Multiline = true,
            Margin = new Padding(5, 0, 20, 10)
        };

        NiceTextBox imageText = new NiceTextBox() {
            Width = 130,
            waterMark = "Path to picture",
            Margin = new Padding(5, 0, 0, 20),
            Dock = DockStyle.Left
        };

        Button imageSeach = new Button() {
            BackgroundImage = Image.FromFile("images/seachIcon.png"),
            BackgroundImageLayout = ImageLayout.Zoom,
            Width = 50,
            Margin = new Padding(5, 0, 0, 20),
        };
        Panel gameImage = new Panel() {
            Width = 200,
            Height = 200,
            Margin = new Padding(5, 0, 0, 0),
            BackgroundImage = Image.FromFile("images/_default.png"),
            BackgroundImageLayout = ImageLayout.Zoom,
        };

        NiceTextBox time = new NiceTextBox() {
            Name = "minimum / maximum players" + Environment.NewLine + "it takes to play the game." + Environment.NewLine + "eg: 3/10",
            waterMark = "min / max players",
            Width = 95,
            Margin = new Padding(0, 0, 0, 20)
        };
        NiceTextBox players = new NiceTextBox() {
            Name = "minimum / maximum time " + Environment.NewLine + "it takes to complete the game." + Environment.NewLine + "eg: 30/60",
            waterMark = "min / max time",
            Width = 95,
            Margin = new Padding(5, 0, 0, 20)
        };

        ListView genreBox = new ListView() {
            View = View.Details,
            // Allow the user to edit item text.
            LabelEdit = true,
            // Allow the user to rearrange columns.
            AllowColumnReorder = true,
            // Display check boxes.
            CheckBoxes = true,
            // Select the item and subitems when selection is made.
            FullRowSelect = true,
            // Display grid lines.
            
            
            GridLines = true,
            // Sort the items in the list in ascending order.
            Sorting = SortOrder.Ascending,
            //sets bounds.
            Size = new Size(200, 200)
        };
        private List<ListViewItem> genreItems = new List<ListViewItem>();
        
        public List<string> differentGenres = new List<string>{ "Horror", "Lying", "Other stuff","Third stuff","Strategy","Coop","Adventure","dnd","Entertainment","Comic","Ballzy","#360NoScope" };
        private GamesTab gametab;
        private Game game;
        private Game b4EditingGame;
        private Image seachImage = Image.FromFile("images/seachIcon.png");
        private Image image;
        
        public GamePopupBox(GamesTab gametab, Game game) {
            Size = new Size(1000,700);
            SubscriptionList();



            this.gametab = gametab;
            genreBox.Columns.Add("Genre", -2, HorizontalAlignment.Left);


            foreach(var item in differentGenres) {
                genreItems.Add(new ListViewItem { Name = item, Text = item});
            }


            genreBox.Items.AddRange(genreItems.ToArray());

            
            if (this.game != null) {
                
                b4EditingGame = game;
                this.game = new Game(game);

                gameName.Text = this.game.name;
                gameDescription.Text = this.game.description;

                time.Text = this.game.minPlayers.ToString() + "/" + this.game.maxPlayers.ToString();
                players.Text = this.game.minPlayTime.ToString() + "/" + this.game.maxPlayTime.ToString(); 

                foreach(ListViewItem item in genreItems )
                    item.Checked = (game.genre.Any(x => x == item.Text)) ? true : false;

            } else {
                this.game = new Game();
                Console.WriteLine(this.game.ToString());
                Controls.Find("delete", true).First().Enabled = false;
            }
            
            /* 
             * Checks if there has been any changes in the game, if there is any changes,
             * and they are different from what the game looked before (if it is not a new game) then set hasBeenChanged to true,
             * hasBeenChanged is located in FancyPopupBox and is a bool to keep track of if something is changed,
             * if true, there will be a messagebox that asks if the user is sure it will close b4 saving.
             */
            
           
            
            
        }

        
        private void SubscriptionList() {
            bool isNewGame = (b4EditingGame != null) ? true : false;
            string messageboxText = "Please only use whole integers" + Environment.NewLine + "In this format INTEGER/INTEGER";

            gameName.TextChanged += (s, e) => { hasBeenChanged = (isNewGame) ? ((b4EditingGame.name != gameName.Text) ? true : false) : true; };

            gameDescription.TextChanged += (s, e) => { hasBeenChanged = (isNewGame) ? ((b4EditingGame.description != gameDescription.Text) ? true : false) : true; };

            players.TextChanged += (s, e) => { hasBeenChanged = (isNewGame) ? (!((b4EditingGame.minPlayers.ToString() + "/" + b4EditingGame.maxPlayers.ToString()).Equals(players.Text)) ? true : false) : false; };

            time.TextChanged += (s, e) => {
                hasBeenChanged = (isNewGame) ? (!((b4EditingGame.minPlayTime.ToString() + "/" + b4EditingGame.maxPlayTime.ToString()).Equals(time.Text)) ? true : false) : false;
            };

            time.LostFocus += (s, e) => {
                
                string[] withOutSlash = time.Text.Split('/');
                try {
                    Int32.TryParse(withOutSlash[0], out game.minPlayTime);
                    Int32.TryParse(withOutSlash[1], out game.maxPlayTime);
                    
                } catch (Exception) {
                    SystemSounds.Hand.Play();
                    time.Text = "";
                    MessageBox.Show(messageboxText, "FATAL ERROR IN TIME");
                }
            };

            players.LostFocus += (s, e) => {
                
                string[] withOutSlash = players.Text.Split('/');
                try {
                    Int32.TryParse(withOutSlash[0], out game.minPlayers);
                    Int32.TryParse(withOutSlash[1], out game.maxPlayers);
                } catch (Exception) {
                    SystemSounds.Hand.Play();
                    players.Text = "";
                    MessageBox.Show(messageboxText, "FATAL ERROR IN PLAYERS");
                }
                
            };

            genreBox.ItemCheck += new ItemCheckEventHandler(memeberChecked);

            imageSeach.Click += OpenFileOpener;

            imageText.Click += OpenFileOpener;

            




        }

        protected override Control CreateControls() {
            TableLayoutPanel header = new TableLayoutPanel();
            header.RowCount = 1;
            header.ColumnCount = 2;
            header.Dock = DockStyle.Fill;
            header.AutoSize = true;
            header.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;

            Controls.Add(header);

            TableLayoutPanel rght = new TableLayoutPanel();
            rght.ColumnCount = 1;
            rght.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            rght.Dock = DockStyle.Fill;

            rght.Controls.Add(gameName);
            rght.Controls.Add(gameDescription);

            TableLayoutPanel generalInformaiton = new TableLayoutPanel();
            generalInformaiton.ColumnCount = 2;
            generalInformaiton.RowCount = 1;

            generalInformaiton.Height = time.Height;
            generalInformaiton.Controls.Add(time);
            generalInformaiton.Controls.Add(players);

            

            rght.Controls.Add(generalInformaiton);
            rght.Controls.Add(genreBox);

            TableLayoutPanel imageSeachTable = new TableLayoutPanel();
            imageSeachTable.ColumnCount = 2;
            imageSeachTable.RowCount = 1;
            imageSeach.Height = imageSeach.Height;
            imageSeachTable.Height = imageSeach.Height;
            imageSeachTable.Controls.Add(imageText);
            imageSeachTable.Controls.Add(imageSeach);

            rght.Controls.Add(imageSeachTable);

            rght.Controls.Add(gameImage);

            TableLayoutPanel lft = new TableLayoutPanel();
            lft.ColumnCount = 1;
            lft.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            lft.Dock = DockStyle.Fill;

            

            header.Controls.Add(rght);
            header.Controls.Add(lft);
            return header;
        }

        protected override void save(object sender, EventArgs e) {
            if (b4EditingGame != null) {
                game.description = gameDescription.Text;
                game.name = gameName.Text;
                
                b4EditingGame = game;
            }
                
            else
                //create game.

            base.save(sender, e);
        }

        protected override void delete(object sender, EventArgs e) {
            //delete game
        }

        private void memeberChecked(object sender, ItemCheckEventArgs e) {
            
            if (e.CurrentValue != CheckState.Checked) {
                string temp = genreBox.Items[e.Index].Text;
                game.genre.Add(temp);
                if (!differentGenres.Contains(temp))
                    differentGenres.Add(temp);
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

                } catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }
            }
        }

    }
}