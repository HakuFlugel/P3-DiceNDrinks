using System;
using System.Drawing;
using System.Windows.Forms;
using Shared;
using System.Collections.Generic;
using System.Net;
using System.Linq;

namespace AdministratorPanel {
    public class EditGenrePopupbox : Form {
        TableLayoutPanel head = new TableLayoutPanel() {
            ColumnCount = 1,
            RowCount = 2,
            Dock = DockStyle.Fill,
        };
        

        private TableLayoutPanel genreList = new TableLayoutPanel() {

            Dock = DockStyle.Fill,

            BorderStyle = BorderStyle.Fixed3D,
            ColumnCount = 1,

            GrowStyle = TableLayoutPanelGrowStyle.AddRows,
            MinimumSize = new Size(200, 200),

            AutoSize = false,
            AutoScroll = true,

        };

        Button Add = new Button() {
            MaximumSize = new Size(50, 40),
            Dock = DockStyle.Right,
            Text = "Add",
        };

        private GamesController gamesController;


        public EditGenrePopupbox(GamesController gamesController) {
            this.gamesController = gamesController;
            Size = new Size(276, 288);

            Controls.Add(head);
            head.Controls.Add(genreList);
            head.Controls.Add(Add);
            Add.Click += (s, e) => {
                gamesController.addGenre("new");
                makeItems();
            };
            Show();
            Focus();
            makeItems();

        }

        public void makeItems() {
            genreList.Controls.Clear();
            foreach (var item in gamesController.genres.OrderBy(x => x)) {
                
                genreList.Controls.Add(new GenreItem(gamesController,item,this));

            }
        }
    }
}