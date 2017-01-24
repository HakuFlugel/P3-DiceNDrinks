using System.Drawing;
using System.Windows.Forms;
using Shared;
using System.Linq;

namespace AdministratorPanel {
    public class EditGenrePopupbox : Form {
        private TableLayoutPanel headerTableLayoutPanel = new TableLayoutPanel() {
            ColumnCount = 1,
            RowCount = 2,
            Dock = DockStyle.Fill
        };

        private TableLayoutPanel buttonsPanel = new TableLayoutPanel() {
            ColumnCount = 3,
            RowCount = 1,
            Dock = DockStyle.Bottom,
            Height = 50,
        };

        private TableLayoutPanel genreTableLayoutPanel = new TableLayoutPanel() {
            //Dock = DockStyle.fill,
            BorderStyle = BorderStyle.Fixed3D,
            ColumnCount = 1,
            GrowStyle = TableLayoutPanelGrowStyle.AddRows,
            MinimumSize = new Size(275, 400),
            AutoSize = false,
            AutoScroll = true
        };

        private Button addGenreButton = new Button() {
            MaximumSize = new Size(50, 40),
            Margin = new Padding(0,0,0,0),
            Dock = DockStyle.Right,
             Cursor = Cursors.Hand,
            Text = "Add"
        };

        private Button saveGenresButton = new Button() {
            MaximumSize = new Size(50, 40),
            Margin = new Padding(10,0,10,0),
            Cursor = Cursors.Hand,
            Text = "Save",
            Dock = DockStyle.Right,

        };

        private Button cancelGenresButton = new Button() {
            MaximumSize = new Size(50, 40),
            Margin = new Padding(10, 0, 50, 0),
            Cursor = Cursors.Hand,
            Text = "Cancel",
            Dock = DockStyle.Left,
        };
        private Genres genre;
        private GamePopupBox gmpop;
        public EditGenrePopupbox(Genres genre, GamePopupBox gmpop) {
            this.genre = genre;
            this.gmpop = gmpop;
            Size = new Size(300, 500);

            Controls.Add(headerTableLayoutPanel);
            headerTableLayoutPanel.Controls.Add(genreTableLayoutPanel);
            headerTableLayoutPanel.Controls.Add(buttonsPanel);
            buttonsPanel.Controls.Add(cancelGenresButton);
            buttonsPanel.Controls.Add(saveGenresButton);
            buttonsPanel.Controls.Add(addGenreButton);
            
            

            addGenreButton.Click += (s, e) => {
                genre.add("");
                UpdateGenres();
            };

            saveGenresButton.Click += (s, e) => {
                genre.differentGenres.Clear();
                foreach (GenreItem item in genreTableLayoutPanel.Controls)
                    genre.differentGenres.Add(item.Name);
                gmpop.genreItems.Clear();
                gmpop.genreBox.Items.Clear();
                gmpop.GenreBoxAddItems();
                Close();
            };
            cancelGenresButton.Click += (s, e) => {
                if (DialogResult.Yes == NiceMessageBox.Show("All the changes will be lost, do you understand this?", "About to close GenreBox", MessageBoxButtons.YesNo))
                    Close();

            };

            Show();
            Focus();
            UpdateGenres();
            
        }

        public void UpdateGenres() {
            
            genreTableLayoutPanel.Controls.Clear();
            
            foreach (var item in genre.differentGenres.OrderBy(x => x)) {
                genreTableLayoutPanel.Controls.Add(new GenreItem(genre,item,this));
            }
            
        }
    }
}