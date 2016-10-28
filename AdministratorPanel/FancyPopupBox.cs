using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System;
using Shared;
namespace AdministratorPanel {
    /*abstract*/
    abstract class FansyPopupBox : Form {
        public bool isChanged = false;

        public FansyPopupBox() {

            MinimumSize = new Size(800, 400);
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;

            TableLayoutPanel tb = new TableLayoutPanel();
            tb.BackColor = Color.AliceBlue;
            Controls.Add(tb);
            tb.Dock = DockStyle.Fill;

            tb.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            tb.RowCount = 2;
            tb.ColumnCount = 1;

            tb.Controls.Add(CreateControls());

            /*TableLayoutPanel splitButtons = new TableLayoutPanel();
            splitButtons.RowCount = 1;
            splitButtons.ColumnCount = 2;
            splitButtons.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;*/
            //tb.Controls.Add(splitButtons);

            Panel p = new Panel();
            p.Height = 42;
            p.Dock = DockStyle.Bottom;
            p.BackColor = Color.LightBlue;
            tb.Controls.Add(p);

            Button delete = new Button();
            delete.Text = "Delete";
            delete.Dock = DockStyle.Left;
            p.Controls.Add(delete);
            

            Button cancel = new Button();
            cancel.Text = "Cancel";
            cancel.Dock = DockStyle.Right;
            p.Controls.Add(cancel);

            Button save = new Button();
            save.Text = "Save";
            save.Dock = DockStyle.Right;
            p.Controls.Add(save);



            //2?


            //3?

        }

        protected abstract Control CreateControls();

        /*private void defaultButtons() {
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
        }*/

        /*abstract public void Save(object sender, EventArgs e);

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
        }*/
    }

    class ProductPopupbox : FansyPopupBox {
        //for products
        public int pRice;
        Product product;
        public ProductPopupbox(Product product) {
            
            this.product = product;
            

        }

        protected override Control CreateControls() {
            TableLayoutPanel tbtb = new TableLayoutPanel();
            tbtb.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            tbtb.RowCount = 1;
            tbtb.ColumnCount = 2;

            Panel leftPanel = new Panel();

            NiceTextBox name = new NiceTextBox();
            name.clearable = true;
            name.waterMark = "Insert name....";
            name.TextChanged += (sender, ev) => {
                isChanged = true;
            };
            ComboBox stuff = new ComboBox();

            return tbtb;
        }
    }
}

//    class GamesPopupbox : FansyPopupBox {
//        //for games
//        public int gYear;
//        public string gGenre = "";
//        public int gMinTime;
//        public int gMaxTime;
//        public int gMinPlayers;
//        public int gMaxPlayers;

//        GamesPopupbox() {
//            Text = "Game details";
//        }

//        public override void Delete(object sender, EventArgs e) {
//        }

//        public override void Save(object sender, EventArgs e) {
//        }
//    }


//}