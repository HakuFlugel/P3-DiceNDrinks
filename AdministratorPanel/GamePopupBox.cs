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

namespace AdministratorPanel {

    class GamePopupBox : FancyPopupBox {

        NiceTextBox gameName = new NiceTextBox() {
            Width = 200,
            waterMark = "Game name",
            Margin = new Padding(5, 10, 20, 10)
        };
        NiceTextBox gameDescription = new NiceTextBox() {
            Width = 200,
            Height = 100,
            waterMark = "Game Description",
            Multiline = true,
            Margin = new Padding(5, 0, 20, 10)
        };

        NiceTextBox imageText = new NiceTextBox() {
            Width = 150,
            waterMark = "path to picture",

        };

        Button imageSeach = new Button() {
            Width = 50,
            Text = "🔍"
        };
        Panel productImage = new Panel() {
            Width = 200,
            Height = 200,
            BackgroundImage = Image.FromFile("images/_default.png"),
            Dock = DockStyle.Top,
            BackgroundImageLayout = ImageLayout.Zoom,
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
        private List<NiceTextBox> basicInformation = new List<NiceTextBox> {
                                                        new NiceTextBox { Name = "Minimum palyers" },
                                                        new NiceTextBox { Name = "Maximum players" },
                                                        new NiceTextBox { Name = "Minimum time" },
                                                        new NiceTextBox { Name = "Maximum time" } };

        public List<string> differentGenres = new List<string>{ "Horror", "Lying", "Other stuff","Third stuff","Strategy","Coop","Adventure","dnd","Entertainment","Comic","Ballzy","#360NoScope" };
        private GamesTab gametab;
        private Game game;
        private Game b4EditingGame;

        private Image image;
        
        public GamePopupBox(GamesTab gametab, Game game) {
            Size = new Size(500,500);
            imageSeach.Click += (s, e) => {

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
                        productImage.BackgroundImage = image;

                    } catch (Exception ex) {
                        MessageBox.Show(ex.Message);
                    }
                }
            };
            this.gametab = gametab;
            genreBox.Columns.Add("Genre", -2, HorizontalAlignment.Left);
            foreach(var item in differentGenres) {
                genreItems.Add(new ListViewItem { Name = item, Text = item});
                Console.WriteLine(genreItems.Count);
            }
            Console.WriteLine("starting adding item to listview");
            foreach (var item in genreItems)
                Console.WriteLine("item: " + item );

            genreBox.Items.AddRange(genreItems.ToArray());

            foreach (var item in basicInformation) {
                item.Width = 20;

            }
            if (this.game != null) {
                
                b4EditingGame = game;
                this.game = new Game(game);

                gameName.Text = this.game.name;
                gameDescription.Text = this.game.description;

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
            
            gameName.TextChanged += (s, e) => { hasBeenChanged = (b4EditingGame != null) ? ((b4EditingGame.name != gameName.Text) ? true : false) : true; };
            gameDescription.TextChanged += (s, e) => { hasBeenChanged = (b4EditingGame != null) ? ((b4EditingGame.description != gameDescription.Text) ? true : false) : true; };

            genreBox.ItemCheck += new ItemCheckEventHandler(memeberChecked);
            
            
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
            
                
            rght.Controls.Add(genreBox);


            TableLayoutPanel lft = new TableLayoutPanel();
            lft.ColumnCount = 1;
            lft.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            lft.Dock = DockStyle.Fill;

            

            header.Controls.Add(rght);
            header.Controls.Add(lft);
            return header;
        }

        protected override void save(object sender, EventArgs e) {
            if(b4EditingGame != null)
                b4EditingGame = game;
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
    }
}