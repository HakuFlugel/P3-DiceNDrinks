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
            delete.Click += this.delete;
            p.Controls.Add(delete);
            

            Button cancel = new Button();
            cancel.Text = "Cancel";
            cancel.Dock = DockStyle.Right;
            cancel.Click += this.cancel;
            p.Controls.Add(cancel);

            Button save = new Button();
            save.Text = "Save";
            save.Dock = DockStyle.Right;
            save.Click += this.save;
            p.Controls.Add(save);



            //2?


            //3?

        }

        protected abstract Control CreateControls();


        abstract public void save(object sender, EventArgs e);

        abstract public void delete(object sender, EventArgs e);

        private void cancel(object sender, EventArgs e) {
            if (isChanged) {
                if (DialogResult.Yes ==
                    MessageBox.Show("Are you sure? Everything unsaved will be lost.",
                    "About to close",
                    MessageBoxButtons.YesNo)) {
                    isChanged = false;
                    Close();
                }
            }
        }
    }

    class ProductPopupbox : FansyPopupBox {
        //for products
        Product product;
        public ProductPopupbox(Product product) {
            
            this.product = product;
            

        }

        public override void delete(object sender, EventArgs e) {
            throw new NotImplementedException();
        }

        public override void save(object sender, EventArgs e) {
            throw new NotImplementedException();
        }

        protected override Control CreateControls() {
            TableLayoutPanel tbtb = new TableLayoutPanel();
            tbtb.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            tbtb.RowCount = 1;
            tbtb.ColumnCount = 2;

            Panel leftPanel = new Panel();
            Panel one = new Panel();
            one.AutoSize = true;
            

            leftPanel.Controls.Add(one);


            NiceTextBox name = new NiceTextBox();
            name.clearable = true;
            name.waterMark = "Insert name....";
            name.TextChanged += (sender, ev) => {
                isChanged = true;
            };

            NiceDropDownBox catagory = new NiceDropDownBox();
            catagory.Name = "Catagory";
            catagory.TabIndex = 0;
            

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