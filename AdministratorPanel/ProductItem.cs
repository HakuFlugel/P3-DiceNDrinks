using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shared;
using System.Drawing;

namespace AdministratorPanel {
    class ProductItem : TableLayoutPanel {

        private int sizeX = 160;
        private int sizeY = 160;

        //private Image image = Image.FromFile("C:/Users/Bruger/Desktop/Red.png");

        public ProductItem(Product p) {
            Update();
            //table();
            //this.name = p.name;
            ////this.Text = Information();
            //this.priceElements = p.PriceElements;
            ////this.Image = image;

            //BorderStyle = BorderStyle.FixedSingle;
            BackColor = Color.LightGray;
            GrowStyle = TableLayoutPanelGrowStyle.FixedSize; // note skal måske ændres
            RowCount = 3;
            ColumnCount = 1;

            //1
            PictureBox pb = new PictureBox();
            //pb.Image = image;
            Controls.Add(pb);

            //2
            Label lb = new Label();
            lb.Text = p.name;
            Controls.Add(lb);

            //3
            TableLayoutPanel tb = new TableLayoutPanel();
            tb.AutoSize = true;
            tb.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                //RowCount = 0;
            ColumnCount = 2;
            Controls.Add(Information(p,tb));

            //done

            this.MakeSuperClickable((sender, ev) => {Console.WriteLine("Loli-pop"); });
        }

        private TableLayoutPanel Information(Product p, TableLayoutPanel tb) {
            StringBuilder sb = new StringBuilder();
            Label temp = new Label();
            List<Label> lList = new List<Label>();

            sb.AppendLine(p.name);
            
            foreach (var item in p.PriceElements) {
                sb.Append(item.name);
                temp.Text = sb.ToString();
                lList.Add(temp);
                sb.Clear();

                sb.Append("Price: ");
                sb.Append(item.name);
                temp.Text = sb.ToString();
                lList.Add(temp);
                sb.Clear();
            }

            //foreach (var item in lList) {
            //    Console.WriteLine(item.Text);
            //}

            tb.Controls.AddRange(lList.ToArray());
            return tb;
        }

        private void table() {

            Name = "Information";
            BackColor = Color.Transparent;
            Panel p = new Panel();

            Controls.Add(new Button { Height = 20, Width = 20, Text = "1" });
            Controls.Add(new Button { Height = 20, Width = 20, Text = "2" });
            Controls.Add(new Button { Height = 20, Width = 20, Text = "3" });
            Controls.Add(new Button { Height = 20, Width = 20, Text = "4" });
        }

        private void Update(){
            this.Height = sizeY;
            this.Width = sizeX;
            
        }








        //protected override void OnClick(EventArgs e) {
        //    base.OnClick(e);
        //    Console.WriteLine("clicked " + this);
        //}

        //private void ControlAdded(Control control) {

        //}

        //protected override void OnControlAdded(ControlEventArgs e) {
        //    EventHandler click = (sender, ev) => { OnClick(ev); };
        //    ControlEventHandler controladded = (sender, ev) => {
        //        //OnControlAdded(ev);

        //        foreach (Control control in e.Control.Controls) {
        //            ControlEventArgs cev = new ControlEventArgs(control);
        //            OnControlAdded(cev);
        //            //controladded(this, cev);
        //        }
        //    };

        //    e.Control.Click -= click;
        //    e.Control.Click += click;
        //    e.Control.ControlAdded -= controladded;
        //    e.Control.ControlAdded += controladded;

        //    base.OnControlAdded(e);
        //}
    }
}
