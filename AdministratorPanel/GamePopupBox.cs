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

        DateTimePicker startDatePicker = new DateTimePicker() {
            Dock = DockStyle.Right,
            Margin = new Padding(0, 10, 20, 10)
        };
        NiceTextBox startTimePicker = new NiceTextBox() {
            waterMark = "hh:mm"
        };

        DateTimePicker endDatePicker = new DateTimePicker() {
            Dock = DockStyle.Right,
            Margin = new Padding(0, 0, 20, 10)
        };
        NiceTextBox endTimePicker = new NiceTextBox() {
            waterMark = "hh:mm"
        };
        private GamesTab gametab;
        private Game game;

        public GamePopupBox() {

        }

        public GamePopupBox(GamesTab gametab, Game game) {
            this.gametab = gametab;
            this.game = game;
            if (this.game != null) {
                //gameName.Text = this.game.name;
                //gameDescription.Text = this.game.description;
                //startDatePicker.Value = this.game.startDate;
                //endDatePicker.Value = this.game.endDate;
                //startTimePicker.Text = this.game.startDate.ToString("HH:mm");
                //endTimePicker.Text = this.game.endDate.ToString("HH:mm");
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

            TableLayoutPanel lft = new TableLayoutPanel();
            lft.ColumnCount = 1;
            lft.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            lft.Dock = DockStyle.Fill;

            lft.Controls.Add(startDatePicker);
            lft.Controls.Add(endDatePicker);
            lft.Controls.Add(startTimePicker);
            lft.Controls.Add(endTimePicker);

            header.Controls.Add(rght);
            header.Controls.Add(lft);
            return header;
        }

        protected override void save(object sender, EventArgs e) {
            throw new NotImplementedException();
        }

        protected override void delete(object sender, EventArgs e) {
            throw new NotImplementedException();
        }
    }
}