using System;
using System.Drawing;
using System.Windows.Forms;
using Shared;
using System.Text;

namespace AdministratorPanel {
    public class GenreItem : Panel {
        private Button Remove = new Button() {
            MaximumSize = new Size(50, 17),
            Dock = DockStyle.Right,
            Cursor = Cursors.Hand,
            Text = "Delete"
        };

        private TextBox genreItemTextBox = new TextBox();

        private Button saveButtom = new Button() {
            MaximumSize = new Size(50, 17),
            Dock = DockStyle.Right,
            Cursor = Cursors.Hand,
            Text = "Save"
        };

        public GenreItem(Genres genre, string item, EditGenrePopupbox popbox) {
            Remove.Size = saveButtom.Size = new Size(50,genreItemTextBox.Height);

            saveButtom.Click += (s, e) => {
                genre.rename(item, genreItemTextBox.Text);
                popbox.UpdateGenres();
            };

            Remove.Click += (s, e) => {
                genre.remove(item);
                popbox.UpdateGenres();
            };

            BackColor = Color.LightGray;
            genreItemTextBox.Text = item;
            Controls.Add(genreItemTextBox);
            Controls.Add(saveButtom);
            Controls.Add(Remove);

            
            Dock = DockStyle.Top;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowOnly;
            Margin = new Padding(4, 4, 20, 4);
        }
    }
}