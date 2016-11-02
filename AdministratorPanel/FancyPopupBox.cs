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
        protected Padding labelPadding = new Padding(5, 0, 5, 0);
        protected Padding otherPadding = new Padding(5, 0, 5, 20);

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
        protected Product product;
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

            tbtb.Controls.Add(leftTable());
            tbtb.Controls.Add(reightTable());

            

            return tbtb;
        }

        private TableLayoutPanel leftTable() {
            TableLayoutPanel tb = new TableLayoutPanel();
            tb.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            tb.RowCount = 6;
            tb.ColumnCount = 1;

            Label nameLabel = new Label();
            nameLabel.Text = "Name";
            nameLabel.Margin = labelPadding;
            tb.Controls.Add(nameLabel);

            NiceTextBox nameTextBox = new NiceTextBox();
            nameTextBox.clearable = true;
            nameTextBox.waterMark = "Insert name....";
            nameTextBox.TextChanged += (sender, ev) => {
                isChanged = true;
            };
            nameTextBox.Margin = otherPadding;
            tb.Controls.Add(nameTextBox);

            Label catagoryLabel = new Label();
            catagoryLabel.Text = "Catagory";
            catagoryLabel.Margin = labelPadding;
            tb.Controls.Add(catagoryLabel);

            NiceDropDownBox catagoryDropDown = new NiceDropDownBox();
            if (product == null)
                catagoryDropDown.defaultSeletion = true;
            if (product != null)
                catagoryDropDown.Margin = otherPadding;
            catagoryDropDown.SelectedIndexChanged += (sender, ev) => {
                isChanged = true;
            };

            tb.Controls.Add(catagoryDropDown);

            Label sectionLabel = new Label();
            sectionLabel.Text = "Section";
            sectionLabel.Margin = labelPadding;
            tb.Controls.Add(sectionLabel);

            NiceDropDownBox sectionDropDown = new NiceDropDownBox();
            if (product == null)
                sectionDropDown.defaultSeletion = true;

            sectionDropDown.Margin = otherPadding;
            sectionDropDown.SelectedIndexChanged += (sender, ev) => {
                isChanged = true;
            };

            tb.Controls.Add(sectionDropDown);

            return tb;
        }

        private TableLayoutPanel reightTable() {
            TableLayoutPanel tb = new TableLayoutPanel();
            tb.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            tb.RowCount = 6;
            tb.ColumnCount = 1;

            Label nameLabel = new Label();
            nameLabel.Text = "Price range";
            nameLabel.Margin = labelPadding;
            tb.Controls.Add(nameLabel);

            TableLayoutPanel price = new TableLayoutPanel();
            price.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            price.RowCount = 3;
            price.ColumnCount = 1;

            TableLayoutPanel priceVname = new TableLayoutPanel();
            priceVname.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            priceVname.RowCount = 1;
            priceVname.ColumnCount = 2;

            







            return tb;
        }
    }


//class GamesPopupbox : FansyPopupBox {
//    //for games
//    public int gYear;
//    public string gGenre = "";
//    public int gMinTime;
//    public int gMaxTime;
//    public int gMinPlayers;
//    public int gMaxPlayers;

//    GamesPopupbox() {
//        Text = "Game details";
//    }


//        protected override Control CreateControls() {
//            Padding labelPadding = new Padding(5, 0, 5, 0);
//            Padding otherPadding = new Padding(5, 0, 5, 20);
//            TableLayoutPanel tbtb = new TableLayoutPanel();
//            tbtb.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
//            tbtb.RowCount = 1;
//            tbtb.ColumnCount = 2;

//            TableLayoutPanel left = new TableLayoutPanel();
//            left.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
//            left.RowCount = 1;
//            left.ColumnCount = 6;

//            Label nameLabel = new Label();
//            nameLabel.Text = "name";
//            nameLabel.Margin = labelPadding;

//            NiceTextBox nameTextBox = new NiceTextBox();
//            nameTextBox.clearable = true;
//            nameTextBox.waterMark = "Insert name....";
//            nameTextBox.TextChanged += (sender, ev) => {
//                isChanged = true;
//            };
//            nameTextBox.Margin = otherPadding;

//            Label catagoryLabel = new Label();
//            catagoryLabel.Text = "Catagory";
//            catagoryLabel.Margin = labelPadding;

//            NiceDropDownBox catagoryDropDown = new NiceDropDownBox();

//            if (product == null)
//                catagoryDropDown.defaultSeletion = true;
//            if (product != null)


//                catagoryDropDown.Margin = otherPadding;

//            Label sectionLabel = new Label();
//            sectionLabel.Text = "Section";
//            sectionLabel.Margin = labelPadding;

//            NiceDropDownBox sectionDropDown = new NiceDropDownBox();
//            if (product == null)
//                catagoryDropDown.defaultSeletion = true;


//            catagoryDropDown.Margin = otherPadding;

//            return tbtb;
//        }

//        public override void save(object sender, EventArgs e) {
//            throw new NotImplementedException();
//        }

//        public override void delete(object sender, EventArgs e) {
//            throw new NotImplementedException();
//        }
//    }


}