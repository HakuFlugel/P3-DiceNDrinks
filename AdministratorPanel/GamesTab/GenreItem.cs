using System;
using System.Drawing;
using System.Windows.Forms;
using Shared;
using System.Text;

namespace AdministratorPanel {
    public class GenreItem : NiceButton {
        Button Remove = new Button() {
            MaximumSize = new Size(50, 17),
            Dock = DockStyle.Right,
            Text = "Delete"
        };

        TextBox genreItemTextBox = new TextBox();
        Button Save = new Button() {
            MaximumSize = new Size(50, 17),
            Dock = DockStyle.Right,
            Text = "Save",

        };
        public GenreItem(GamesController gamesController, string item, EditGenrePopupbox popbox) {
            Save.Click += (s, e) => {
                gamesController.renameGenre(item, genreItemTextBox.Text);
                popbox.makeItems();
            };

            Remove.Click += (s, e) => {
                gamesController.removeGenre(item);
                popbox.makeItems();
            };

            genreItemTextBox.Text = item;
            Controls.Add(genreItemTextBox);
            Controls.Add(Save);
            Controls.Add(Remove);
            


            RowCount = 1;
            ColumnCount = 3;
            bgColor = Color.LightGray;
            Dock = DockStyle.Top;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowOnly;
            Margin = new Padding(4, 4, 20, 4);
        }

       
    }
}