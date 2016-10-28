using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System;

namespace AdministratorPanel {
    abstract class FansyPopupBox : TableLayoutPanel {
        private Form messageBox = new Form();
        public string sName = "";
        public string sDescription = "";
        public string sID = "";
        public bool hasBeenEdi = false;
        public string sImage = "";
        public List<Control> control = new List<Control>();

        public FansyPopupBox() {
            TableLayoutPanel tb = new TableLayoutPanel();

            messageBox.FormBorderStyle = FormBorderStyle.FixedSingle;
            messageBox.Width = 800;
            messageBox.Height = 500;
            //1?
            GrowStyle = TableLayoutPanelGrowStyle.FixedSize; // note skal måske ændres
            RowCount = 3;
            ColumnCount = 1;
            //2?


            //3?

        }
        

        public void openMessageBox() {

            defaultButtons();
            messageBox.Controls.AddRange(control.ToArray());

        }

        private void defaultButtons() {
            Point startLoacttion = new Point(12, 393);

            for (int i = 0; i < 2; i++) {
                Button[] buttons = { new Button(), new Button(), new Button() };
                buttons[0].Click += cancle; buttons[1].Click += Delete; buttons[2].Click += Save;

                buttons[0].Location = new Point(496, 393);
                buttons[1].Location = new Point(636, 393);
                buttons[2].Location = new Point(12, 393);

                foreach (var item in buttons) {
                    item.Size = new Size(134, 48);
                    item.Location = new Point(startLoacttion.X, startLoacttion.Y);
                    

                }

            }
        }

        abstract public void Save(object sender, EventArgs e);

        abstract public void Delete(object sender, EventArgs e);

        private void cancle(object sender, EventArgs e) {
            if (hasBeenEdi) {
                if (DialogResult.Yes ==
                    MessageBox.Show("Are you sure? Everything unsaved will be lost.",
                    "About to close",
                    MessageBoxButtons.YesNo)) {
                    hasBeenEdi = false;
                    messageBox.Close();
                }
            }
        }
    }
    class ProductPopupbox : FansyPopupBox {
        //for products
        public int pRice;
        ProductPopupbox() {
            Text = "Product details";
            GrowStyle = TableLayoutPanelGrowStyle.FixedSize;

            RowCount = 3;
            ColumnCount = 2;

        }

        public override void Delete(object sender, EventArgs e) {

        }

        public override void Save(object sender, EventArgs e) {

        }
    }

    class GamesPopupbox : FansyPopupBox {
        //for games
        public int gYear;
        public string gGenre = "";
        public int gMinTime;
        public int gMaxTime;
        public int gMinPlayers;
        public int gMaxPlayers;

        GamesPopupbox() {
            Text = "Game details";
        }

        public override void Delete(object sender, EventArgs e) {
        }

        public override void Save(object sender, EventArgs e) {
        }
    }


}