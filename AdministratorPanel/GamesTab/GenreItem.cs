using System.Drawing;
using System.Windows.Forms;
using Shared;

namespace AdministratorPanel {
    public class GenreItem : Panel {
        private Button Remove = new Button() {
            Dock = DockStyle.Right,
            Cursor = Cursors.Hand,
            Text = "Delete"
        };

        private NiceTextBox genreItemTextBox = new NiceTextBox() {
            waterMark = "New Genre",

        };
        public GenreItem(Genres genre, string oldname, EditGenrePopupbox popbox) {
            Remove.Size = new Size(50, genreItemTextBox.Height);
            Name = oldname;

            Remove.Click += (s, e) => {
                genre.remove(oldname);
                popbox.UpdateGenres();
            };

            genreItemTextBox.TextChanged += (s, e) => {
                Name = genreItemTextBox.Text;
            };

            BackColor = Color.LightGray;
            genreItemTextBox.Text = oldname;

            Controls.Add(genreItemTextBox);
            Controls.Add(Remove);


            Dock = DockStyle.Top;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowOnly;
            Margin = new Padding(4, 4, 20, 4);
        }
    }
}