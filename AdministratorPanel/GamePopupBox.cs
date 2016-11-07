using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System;
using Shared;

namespace AdministratorPanel {

    class GamePopupBox : FancyPopupBox {

        NiceTextBox gameName = new NiceTextBox() {
            Width = 200,
            waterMark = "Game name",
            Margin = new Padding(4, 10, 20, 10)
        };
        NiceTextBox gameDescription = new NiceTextBox() {
            Width = 200,
            Height = 100,
            waterMark = "Game Description",
            Multiline = true,
            Margin = new Padding(4, 0, 20, 10)
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

        public string[] differentGenres ={ "Horror", "Lying", "Other stuff","Third stuff","Strategy","Coop","Adventure","dnd","Entertainment","Comic","Ballzy","#360NoScope" };
        private GamesTab gametab;
        private Game game;
        private Game b4EditingGame;

        public GamePopupBox() {
            genreBox.ItemCheck += new ItemCheckEventHandler(memeberChecked);
            foreach (var item in differentGenres) {
                ListViewItem check = new ListViewItem(item);
                Console.WriteLine(check.Text);
               
                genreItems.Add(check);
            }
                

        }

        private void memeberChecked(object sender, ItemCheckEventArgs e) {
            if (e.CurrentValue == CheckState.Unchecked)
                game.genre.Add(differentGenres[e.Index]);
            else
                game.genre.Remove(differentGenres[e.Index]);
        }

        

        public GamePopupBox(GamesTab gametab, Game game) {
            this.gametab = gametab;
            b4EditingGame = game;
            this.game = new Game(game);
            
            if (this.game != null) {
                
                gameName.Text = this.game.name;
                gameDescription.Text = this.game.description;

                foreach(ListViewItem item in genreItems )
                    item.Checked = (game.genre.Any(x => x == item.Text)) ? true : false;

            } else {
                Controls.Find("delete", true).First().Enabled = false;
            }
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
            b4EditingGame = game;
        }

        protected override void delete(object sender, EventArgs e) {
            throw new NotImplementedException();
        }
    }
}