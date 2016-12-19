﻿using System.Drawing;
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
            Dock = DockStyle.Right,
             Cursor = Cursors.Hand,
            Text = "Add"
        };

        private Genres genre;

        public EditGenrePopupbox(Genres genre) {
            this.genre = genre;
            Size = new Size(300, 500);

            Controls.Add(headerTableLayoutPanel);
            headerTableLayoutPanel.Controls.Add(genreTableLayoutPanel);
            headerTableLayoutPanel.Controls.Add(addGenreButton);

            addGenreButton.Click += (s, e) => {
                genre.add("New Genre");
                UpdateGenres();
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